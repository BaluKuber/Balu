using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using S3GBusEntity;
using System.Collections.Generic;

/// <summary>
/// Summary description for ClsRepaymentStructure
/// </summary>
public class ClsRepaymentStructure
{
    public DateTime dtNextDate { get; set; }
    public int intNextInstall { get; set; }
    public string LineOfBus = string.Empty;
    public string RepayMode = string.Empty;
    public int FunPubGetNoofYearsFromTenure(string strTenureType, string strTenure)
    {
        int intNoofYears = 0;
        if (strTenure != "")
        {
            switch (strTenureType.ToLower())
            {
                case "months":
                    intNoofYears = (int)Math.Ceiling(Convert.ToDecimal(Convert.ToInt32(strTenure) / 12.00));
                    break;
                case "weeks":
                    intNoofYears = (int)Math.Ceiling(Convert.ToDecimal(Convert.ToInt32(strTenure) / 52.00));
                    break;
                case "days":
                    intNoofYears = (int)Math.Ceiling(Convert.ToDecimal(Convert.ToInt32(strTenure) / 365.00));
                    break;
            }
        }
        return intNoofYears;
    }

    public void FunPubGetNextRepaydate(DataTable DtRepayGrid, string strFrequency)
    {
        intNextInstall = 0;
        dtNextDate = DateTime.Now;
        DateTime dtNextFromdate = DateTime.Now;
        DataRow[] drRow = DtRepayGrid.Select("CashFlow_Flag_ID = 23  or CashFlow_Flag_ID = '23'", "ToInstall desc");
        if (drRow.Length > 0)
        {
            dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(strFrequency, Convert.ToDateTime(drRow[0]["ToDate"].ToString()));
            intNextInstall = Convert.ToInt32(drRow[0]["ToInstall"].ToString());
        }
        else
        {
            dtNextDate = dtNextFromdate;
            intNextInstall = 0;
        }
    }

    public void FunPubGetNextRepaydateTL(DataTable DtRepayGrid, string strFrequency, string strCashFlow)
    {
        intNextInstall = 0;
        dtNextDate = DateTime.Now;
        DateTime dtNextFromdate = DateTime.Now;
        DataRow[] drRow = DtRepayGrid.Select("CashFlow_Flag_ID in(" + strCashFlow + ")", "ToInstall desc");
        if (drRow.Length > 0)
        {
            dtNextDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(strFrequency, Convert.ToDateTime(drRow[0]["ToDate"].ToString()));
            intNextInstall = Convert.ToInt32(drRow[0]["ToInstall"].ToString());
        }
        else
        {
            dtNextDate = dtNextFromdate;
            intNextInstall = 0;
        }
    }
    public bool FunPubValidateTenurePeriod(DateTime dtStartDate, DateTime dtEndDate, string strTenureType, string strTenure)
    {
        DateTime dateInterval = new DateTime();

        switch (strTenureType.ToLower())
        {
            case "months":
                dateInterval = dtStartDate.AddMonths(Convert.ToInt32(strTenure));
                break;
            case "weeks":

                int intAddweeks = Convert.ToInt32(strTenure) * 7;
                dateInterval = dtStartDate.AddDays(intAddweeks);
                break;
            case "days":
                dateInterval = dtStartDate.AddDays(Convert.ToInt32(strTenure));
                break;
        }

        if (dtEndDate > dateInterval)
        {
            return false;
        }
        else
        {
            return true;
        }


    }

    public decimal FunPubGetMarginAmout(string strFinanceAmount, string strMarginPercentage)
    {
        decimal decMarginAmount = 0;
        if (strMarginPercentage != "")
        {
            decMarginAmount = (Convert.ToDecimal(strFinanceAmount) * (Convert.ToDecimal(strMarginPercentage) / 100));
        }
        return Math.Round(decMarginAmount, 0);
    }

    public decimal FunPubGetAmountFinanced(string strFinanceAmount, string strMarginPercentage)
    {
        decimal decFinanaceAmt = 0;
        if (strFinanceAmount != "")
            decFinanaceAmt = Convert.ToDecimal(strFinanceAmount); //- FunPubGetMarginAmout(strFinanceAmount, strMarginPercentage);
        return Math.Round(decFinanaceAmt, 0);

    }

    public bool FunPubValidateTotalAmount(DataTable DtRepayGrid,
        string strFinanceAmount, string strMarginPercentage, string strReturnPattern,
        string strRate, string strTenureType, string strTenure, out decimal decActualAmount,
        out decimal decTotalAmount, string strOption, decimal decRoundOff)
    {
        decimal decFinAmount = FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage);
        decimal decRate = 0;

        if (strRate == "")
            strRate = "0";
        decRate = Convert.ToDecimal(strRate);

        switch (strReturnPattern)
        {
            case "3":
                if (decRoundOff == 0)//Added by Kali tolsove divide by zero error on 15-Jun-2012
                    decTotalAmount = (Math.Round(((decFinAmount / 1000) * decRate), 0)) * Convert.ToInt32(strTenure);
                else
                    decTotalAmount = (Math.Round(((decFinAmount / 1000) * decRate) / decRoundOff, 0) * decRoundOff) * Convert.ToInt32(strTenure);
                break;
            case "4":
                if (decRoundOff == 0)
                    decTotalAmount = (Math.Round(((decFinAmount / 100000) * decRate), 0)) * Convert.ToInt32(strTenure);
                else
                    decTotalAmount = (Math.Round(((decFinAmount / 100000) * decRate) / decRoundOff, 0) * decRoundOff) * Convert.ToInt32(strTenure);
                break;
            case "5":
                if (decRoundOff == 0)
                    decTotalAmount = (Math.Round(((decFinAmount / 1000000) * decRate), 0)) * Convert.ToInt32(strTenure);
                else
                    decTotalAmount = (Math.Round(((decFinAmount / 1000000) * decRate) / decRoundOff, 0) * decRoundOff) * Convert.ToInt32(strTenure);
                break;
            default:
                decTotalAmount = decFinAmount +
                   Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount
                   (strTenureType, decFinAmount, decRate, Convert.ToInt32(strTenure)), 0);
                break;
        }

        decActualAmount = 0;

        decActualAmount = (decimal)(DtRepayGrid).Compute("Sum(TotalPeriodInstall)", "CashFlow_Flag_ID = 23  or CashFlow_Flag_ID = '23'");

        if (strOption == "1") /* Oprion 1 - Check Whether the total installment 
                               * amount exceed the Total repay amount when add 
                               * the repay details ( Structure Adhoc Method) */
        {
            if (decActualAmount > decTotalAmount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else /* strOption == "" - Check the total installment amount is equal to the Total repay amount when Re-Calculate the IRR */
        {
            if (decActualAmount == decTotalAmount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public int FunPubValidateTotalAmountTL(DataTable DtRepayGrid,
        string strFinanceAmount, string strMarginPercentage, string strReturnPattern,
        string strRate, string strTenureType, string strTenure, out decimal decActualAmount,
        out decimal decTotalAmount, string strOption, decimal decRoundOff)
    {
        decimal decFinAmount = FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage);
        decimal decRate = 0;

        if (strRate == "")
            strRate = "0";
        decRate = Convert.ToDecimal(strRate);

        switch (strReturnPattern)
        {

            case "3":
                if (decRoundOff == 0) //Added by Kali tolsove divide by zero error on 15-Jun-2012
                    decTotalAmount = (Math.Round(((decFinAmount / 1000) * decRate), 0)) * Convert.ToInt32(strTenure);
                else
                    decTotalAmount = (Math.Round(((decFinAmount / 1000) * decRate) / decRoundOff, 0) * decRoundOff) * Convert.ToInt32(strTenure);
                break;
            case "4":
                if (decRoundOff == 0)
                    decTotalAmount = (Math.Round(((decFinAmount / 100000) * decRate), 0)) * Convert.ToInt32(strTenure);
                else
                    decTotalAmount = (Math.Round(((decFinAmount / 100000) * decRate) / decRoundOff, 0) * decRoundOff) * Convert.ToInt32(strTenure);
                break;
            case "5":
                if (decRoundOff == 0)
                    decTotalAmount = (Math.Round(((decFinAmount / 1000000) * decRate), 0)) * Convert.ToInt32(strTenure);
                else
                    decTotalAmount = (Math.Round(((decFinAmount / 1000000) * decRate) / decRoundOff, 0) * decRoundOff) * Convert.ToInt32(strTenure);
                break;
            default:
                decTotalAmount = decFinAmount +
                   Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount
                   (strTenureType, decFinAmount, decRate, Convert.ToInt32(strTenure)), 0);
                break;
        }

        decActualAmount = 0;
        decimal decPrincipal = 0;
        decimal decInterest = 0;

        DataRow[] dr = (DtRepayGrid).Select("CashFlow_Flag_ID IN(35,91)");
        DataRow[] drPrin = null;
        DataRow[] drIntr = null;

        if (dr.Length == 0)
        {
            return 6;
        }
        else
        {
            decActualAmount = (decimal)(DtRepayGrid).Compute("Sum(TotalPeriodInstall)", "CashFlow_Flag_ID IN(35,91)");
            drPrin = (DtRepayGrid).Select("CashFlow_Flag_ID IN(91)");
            if (drPrin.Length == 0)
            {
                return 4;
            }
            drIntr = (DtRepayGrid).Select("CashFlow_Flag_ID IN(35)");
            if (drIntr.Length == 0)
            {
                return 5;
            }

            decPrincipal = (decimal)(DtRepayGrid).Compute("Sum(TotalPeriodInstall)", "CashFlow_Flag_ID IN(91)");
            decInterest = (decimal)(DtRepayGrid).Compute("Sum(TotalPeriodInstall)", "CashFlow_Flag_ID IN(35)");
        }

        if (strOption == "1") /* Oprion 1 - Check Whether the total installment 
                               * amount exceed the Total repay amount when add 
                               * the repay details ( Structure Adhoc Method) */
        {
            if (decActualAmount > decTotalAmount)
            {
                return 1;
            }
            else if (decPrincipal != decFinAmount)
            {
                return 2;
            }
            else if (decInterest != (decTotalAmount - decPrincipal))
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
        else /* strOption == "" - Check the total installment amount is equal to the Total repay amount when Re-Calculate the IRR */
        {

            if (decPrincipal != decFinAmount)
            {
                return 2;
            }
            else if (decInterest != (decTotalAmount - decPrincipal))
            {
                return 3;
            }
            else if (decActualAmount == decTotalAmount)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
    public void FunPubCalculateIRRForTL(string strDocumentDate, string strPLR, string strFrequency, string strDateFormat, string strResidualAmount, string strResidualPercentage, out double decAccountingIRR, out double decBusinessIRR, out double decCompanyIRR, out DataTable dtRepaymentStructure, DataTable DtRepayGrid, DataTable dtCashInflow, DataTable dtCashOutflow, string strFinanceAmount, string strMarginPercentage, string strReturnPattern, string strRate, string strTenureType, string strTenure, string strIRRRest, string strTimeValue, string strLOB, string strRepaymentMode, string strMorType, DataTable dtMoratoriumInput)
    {
        decimal decActualAmount, decTotalAmount = 0;
        string strType;
        string stroption;
        decAccountingIRR = 0;
        decBusinessIRR = 0;
        decCompanyIRR = 0;
        decimal decRate;
        double docRate;
        decimal? decResvalue = null;
        decimal? decResAmt = null;
        RepaymentType rePayType = new RepaymentType();
        strType = strLOB.ToLower().Split('-')[0].Trim();
        if (strRate == "") strRate = "0";
        switch (strType)
        {
            case "te":
            case "tl":
                if (strRepaymentMode == "5")
                {
                    rePayType = RepaymentType.TLE;
                }
                else
                    rePayType = RepaymentType.EMI;
                break;
            case "ft":
                rePayType = RepaymentType.FC;
                break;
            case "wc":
                rePayType = RepaymentType.WC;
                break;
            default:
                rePayType = RepaymentType.EMI;
                break;
        }

        if (strReturnPattern == "3" || strReturnPattern == "4" || strReturnPattern == "5")
        {
            stroption = "3";
        }
        else
        {
            stroption = "2";
        }

        CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();


        double decResultIrr = 0;
        decimal decPLR = 0;
        if (strPLR != "")
        {
            decPLR = Convert.ToDecimal(strPLR);
        }

        switch (strIRRRest.ToLower())
        {
            case "1":
                strIRRRest = "daily";
                break;
            case "2":
                strIRRRest = "monthly";
                break;
            default:
                strIRRRest = "daily";
                break;

        }
        switch (strTimeValue.ToLower())
        {
            case "1":
                strTimeValue = "advance";
                break;
            case "2":
                strTimeValue = "arrears";
                break;
            case "3":
                strTimeValue = "advancefbd";
                break;
            case "4":
                strTimeValue = "arrearsfbd";
                break;
            default:

                strTimeValue = "advance";
                break;
        }

        decRate = 0;//changed on 1st feb2011
        switch (strReturnPattern.ToUpper())
        {

            case "2":
                //ObjBusinessLogic.FunPubCalculateFlatRate(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat, FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), Convert.ToDouble(strRate), strIRRRest, "Empty", strTimeValue, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                //decRate = Convert.ToDecimal(docRate.ToString("0.0000"));
                break;
            case "1":
                decRate = Convert.ToDecimal(strRate);
                break;
            default:
                decRate = Convert.ToDecimal(strRate);
                break;
        }

        if (strResidualAmount != "")
        {
            decResAmt = Convert.ToDecimal(strResidualAmount);
        }
        if (strResidualPercentage != "")
        {
            decResvalue = Convert.ToDecimal(strResidualPercentage);
        }

        dtRepaymentStructure = ObjBusinessLogic.FunPubGenerateRepaymentStructureForTL(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
            FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
            decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType);

    }

    public void FunPubCalculateIRR(string strDocumentDate, string strPLR, string strFrequency, string strDateFormat, string strResidualAmount, string strResidualPercentage, out double decAccountingIRR, out double decBusinessIRR, out double decCompanyIRR, out DataTable dtRepaymentStructure, DataTable DtRepayGrid, DataTable dtCashInflow, DataTable dtCashOutflow, string strFinanceAmount, string strMarginPercentage, string strReturnPattern, string strRate, string strTenureType, string strTenure, string strIRRRest, string strTimeValue, string strLOB, string strRepaymentMode, string strMorType, DataTable dtMoratoriumInput, bool blApplicable,string strSpliType,int strDelTenure)
    {
        decimal decActualAmount, decTotalAmount = 0;
        string strType;
        string stroption;
        decAccountingIRR = 0;
        decBusinessIRR = 0;
        decCompanyIRR = 0;
        decimal decRate;
        double docRate;
        decimal? decResvalue = null;
        decimal? decResAmt = null;
        RepaymentType rePayType = new RepaymentType();
        strType = strLOB.ToLower().Split('-')[0].Trim();
        if (strRate == "") strRate = "0";
        switch (strType)
        {
            case "te":
            case "tl":
                if (strRepaymentMode == "5")
                {
                    rePayType = RepaymentType.TLE;
                }
                else
                    rePayType = RepaymentType.EMI;
                break;
            case "ft":
                rePayType = RepaymentType.FC;
                break;
            case "wc":
                rePayType = RepaymentType.WC;
                break;
            default:
                rePayType = RepaymentType.EMI;
                break;
        }

        if (strReturnPattern == "3" || strReturnPattern == "4" || strReturnPattern == "5")
        {
            stroption = "3";
        }
        else if (strReturnPattern == "6")
        {
            stroption = "6";
        }
        else
        {
            stroption = "2";
        }

        CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();


        double decResultIrr = 0;
        decimal decPLR = 0;
        if (strPLR != "")
        {
            decPLR = Convert.ToDecimal(strPLR);
        }

        switch (strIRRRest.ToLower())
        {
            case "1":
                strIRRRest = "daily";
                break;
            case "2":
                strIRRRest = "monthly";
                break;
            case "3":
                strIRRRest = "yearly";
                break;
            default:
                strIRRRest = "daily";
                break;

        }
        switch (strTimeValue.ToLower())
        {
            case "1":
                strTimeValue = "advance";
                break;
            case "2":
                strTimeValue = "arrears";
                break;
            case "3":
                strTimeValue = "advancefbd";
                break;
            case "4":
                strTimeValue = "arrearsfbd";
                break;
            default:

                strTimeValue = "advance";
                break;
        }

        decRate = 0;//changed on 1st feb2011
        switch (strReturnPattern.ToUpper())
        {

            case "2":
                //ObjBusinessLogic.FunPubCalculateFlatRate(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat, FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), Convert.ToDouble(strRate), strIRRRest, "Empty", strTimeValue, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                //decRate = Convert.ToDecimal(docRate.ToString("0.0000"));
                break;
            case "1":
                decRate = Convert.ToDecimal(strRate);
                break;
            default:
                decRate = Convert.ToDecimal(strRate);
                break;
        }

        if (strResidualAmount != "")
        {
            decResAmt = Convert.ToDecimal(strResidualAmount);
        }
        if (strResidualPercentage != "")
        {
            decResvalue = Convert.ToDecimal(strResidualPercentage);
        }

        //if (((strLOB.ToUpper().Contains("TL")) || (strLOB.ToUpper().Contains("TE"))) && (stroption == "6"))
        //{
        //    dtRepaymentStructure = ObjBusinessLogic.FunPubGenerateRepaymentStructure_Principal_Method(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
        //        FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
        //        decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType);   
        //}
        //else
        //{
        dtRepaymentStructure = ObjBusinessLogic.FunPubGenerateRepaymentStructure(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
            FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.20), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
            decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType,strSpliType,strDelTenure);
        //}

    }


    public void FunPubCalculateIRR(string strDocumentDate, string strPLR, string strFrequency, string strDateFormat, string strResidualAmount, string strResidualPercentage, out double decAccountingIRR, out double decBusinessIRR, out double decCompanyIRR, out DataTable dtRepaymentStructure, out DataTable dtRepayDetails, DataTable DtRepayGrid, DataTable dtCashInflow, DataTable dtCashOutflow, string strFinanceAmount, string strMarginPercentage, string strReturnPattern, string strRate, string strTenureType, string strTenure, string strIRRRest, string strTimeValue, string strLOB, string strRepaymentMode, string strMorType, DataTable dtMoratoriumInput, string strLevy, int intDeffered, string dtDocFBDate)
    {
        decimal decActualAmount, decTotalAmount = 0;
        string strType;
        string stroption;
        decAccountingIRR = 0;
        decBusinessIRR = 0;
        decCompanyIRR = 0;
        decimal decRate;
        double docRate;
        decimal? decResvalue = null;
        decimal? decResAmt = null;
        RepaymentType rePayType = new RepaymentType();
        strType = strLOB.ToLower().Split('-')[0].Trim();
        if (strRate == "") strRate = "0";
        switch (strType)
        {
            case "te":
            case "tl":
                if (strRepaymentMode == "5")
                {
                    rePayType = RepaymentType.TLE;
                }
                else
                    rePayType = RepaymentType.EMI;
                break;
            case "ft":
                rePayType = RepaymentType.FC;
                break;
            case "wc":
                rePayType = RepaymentType.WC;
                break;
            default:
                rePayType = RepaymentType.EMI;
                break;
        }

        if (strReturnPattern == "3" || strReturnPattern == "4" || strReturnPattern == "5")
        {
            stroption = "3";
        }
        else if (strReturnPattern == "6")
        {
            stroption = "6";
        }
        else
        {
            stroption = "2";
        }

        CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();


        double decResultIrr = 0;
        decimal decPLR = 0;
        if (strPLR != "")
        {
            decPLR = Convert.ToDecimal(strPLR);
        }

        switch (strIRRRest.ToLower())
        {
            case "1":
                strIRRRest = "daily";
                break;
            case "2":
                strIRRRest = "monthly";
                break;
            case "3":
                strIRRRest = "yearly";
                break;
            default:
                strIRRRest = "daily";
                break;

        }
        switch (strTimeValue.ToLower())
        {
            case "1":
                strTimeValue = "advance";
                break;
            case "2":
                strTimeValue = "arrears";
                break;
            case "3":
                strTimeValue = "advancefbd";
                break;
            case "4":
                strTimeValue = "arrearsfbd";
                break;
            default:

                strTimeValue = "advance";
                break;
        }

        decRate = 0;//changed on 1st feb2011
        switch (strReturnPattern.ToUpper())
        {

            case "2":
                //ObjBusinessLogic.FunPubCalculateFlatRate(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat, FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), Convert.ToDouble(strRate), strIRRRest, "Empty", strTimeValue, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                //decRate = Convert.ToDecimal(docRate.ToString("0.0000"));
                break;
            case "1":
                decRate = Convert.ToDecimal(strRate);
                break;
            default:
                decRate = Convert.ToDecimal(strRate);
                break;
        }

        if (strResidualAmount != "")
        {
            decResAmt = Convert.ToDecimal(strResidualAmount);
        }
        if (strResidualPercentage != "")
        {
            decResvalue = Convert.ToDecimal(strResidualPercentage);
        }

        //if (((strLOB.ToUpper().Contains("TL")) || (strLOB.ToUpper().Contains("TE"))) && (stroption == "6"))
        //{
        DataSet ds = new DataSet();
        if (intDeffered == 1)//For Defferred payment
        {
            ds = ObjBusinessLogic.FunPubGenerateRepaymentStructure_Principal_MethodNew(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
     FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
     decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType, Convert.ToInt32(strLevy), Utility.StringToDate(dtDocFBDate));
        }
        else
        {
            ds = ObjBusinessLogic.FunPubGenerateRepaymentStructure_Principal_MethodNew(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
                   FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
                   decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType, Convert.ToInt32(strLevy), Utility.StringToDate(dtDocFBDate));

        }
        dtRepaymentStructure = ds.Tables[0];
        dtRepayDetails = ds.Tables[1];
        //}
        //else
        //{
        //    dtRepaymentStructure = ObjBusinessLogic.FunPubGenerateRepaymentStructure(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
        //        FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
        //        decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType);
        //}

    }




    #region "New Repayment Logic CR_SISSL12E046_018"
    public void FunPubCalculateIRR(string strDocumentDate, string strPLR, string strFrequency, string strDateFormat, string strResidualAmount, string strResidualPercentage, out double decAccountingIRR, out double decBusinessIRR, out double decCompanyIRR, out DataTable dtRepaymentStructure, out DataTable dtRepayDetailsOthers, DataTable DtRepayGrid, DataTable dtCashInflow, DataTable dtCashOutflow, string strFinanceAmount, string strMarginPercentage, string strReturnPattern, string strRate, string strTenureType, string strTenure, string strIRRRest, string strTimeValue, string strLOB, string strRepaymentMode, string strMorType, DataTable dtMoratoriumInput, bool blApplicable)
    {
        decimal decActualAmount, decTotalAmount = 0;
        string strType;
        string stroption;
        decAccountingIRR = 0;
        decBusinessIRR = 0;
        decCompanyIRR = 0;
        decimal decRate;
        double docRate;
        decimal? decResvalue = null;
        decimal? decResAmt = null;
        RepaymentType rePayType = new RepaymentType();
        strType = strLOB.ToLower().Split('-')[0].Trim();
        if (strRate == "") strRate = "0";
        switch (strType)
        {
            case "te":
            case "tl":
                if (strRepaymentMode == "5")
                {
                    rePayType = RepaymentType.TLE;
                }
                else
                    rePayType = RepaymentType.EMI;
                break;
            case "ft":
                rePayType = RepaymentType.FC;
                break;
            case "wc":
                rePayType = RepaymentType.WC;
                break;
            default:
                rePayType = RepaymentType.EMI;
                break;
        }

        if (strReturnPattern == "3" || strReturnPattern == "4" || strReturnPattern == "5")
        {
            stroption = "3";
        }
        else if (strReturnPattern == "6")
        {
            stroption = "6";
        }
        else
        {
            stroption = "2";
        }

        CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();


        double decResultIrr = 0;
        decimal decPLR = 0;
        if (strPLR != "")
        {
            decPLR = Convert.ToDecimal(strPLR);
        }

        switch (strIRRRest.ToLower())
        {
            case "1":
                strIRRRest = "daily";
                break;
            case "2":
                strIRRRest = "monthly";
                break;
            case "3":
                strIRRRest = "yearly";
                break;
            default:
                strIRRRest = "daily";
                break;

        }
        switch (strTimeValue.ToLower())
        {
            case "1":
                strTimeValue = "advance";
                break;
            case "2":
                strTimeValue = "arrears";
                break;
            case "3":
                strTimeValue = "advancefbd";
                break;
            case "4":
                strTimeValue = "arrearsfbd";
                break;
            default:

                strTimeValue = "advance";
                break;
        }

        decRate = 0;//changed on 1st feb2011
        switch (strReturnPattern.ToUpper())
        {

            case "2":
                //ObjBusinessLogic.FunPubCalculateFlatRate(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat, FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), Convert.ToDouble(strRate), strIRRRest, "Empty", strTimeValue, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                //decRate = Convert.ToDecimal(docRate.ToString("0.0000"));
                break;
            case "1":
                decRate = Convert.ToDecimal(strRate);
                break;
            default:
                decRate = Convert.ToDecimal(strRate);
                break;
        }

        if (strResidualAmount != "")
        {
            decResAmt = Convert.ToDecimal(strResidualAmount);
        }
        if (strResidualPercentage != "")
        {
            decResvalue = Convert.ToDecimal(strResidualPercentage);
        }

        //if (((strLOB.ToUpper().Contains("TL")) || (strLOB.ToUpper().Contains("TE"))) && (stroption == "6"))
        //{
        //    dtRepaymentStructure = ObjBusinessLogic.FunPubGenerateRepaymentStructure_Principal_Method(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
        //        FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
        //        decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType);   
        //}
        //else
        //{





        dtRepaymentStructure = ObjBusinessLogic.FunPubGenerateRepaymentStructure(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
            FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
            decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType,"NIL",0);

        //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
        //Others
        dtRepayDetailsOthers = null;
        DataTable DtRepayGrid_New = DtRepayGrid.Copy();
        DataTable DtRepayGrid_Amort = DtRepayGrid.Clone();
        DataTable dtRepaymentStructure_New = dtRepaymentStructure.Copy();
        DataRow[] drRepay_OthersArray = DtRepayGrid.Select("CashFlow_Flag_ID <> 23 and CashFlow_Flag_ID <> 25 and Amort='2'", "slno asc");

        if (drRepay_OthersArray.Length > 0)
        {
            foreach (DataRow drOther in drRepay_OthersArray)
            {
                int intFromInstall = 0, intToInstall = 0;
                decimal decTotalPrincipal = 0, decPerInstallAmount = 0;

                intFromInstall = Convert.ToInt16(drOther["FromInstall"].ToString());
                intToInstall = Convert.ToInt16(drOther["ToInstall"].ToString());

                decPerInstallAmount = Convert.ToDecimal(drOther["PerInstall"].ToString());

                string strExpressionTotal = "InstallmentNo >='" + intFromInstall.ToString() + "' and InstallmentNo <='" + intToInstall.ToString() + "'";
                try
                {
                    decTotalPrincipal = (decimal)dtRepaymentStructure.Compute("sum(PrincipalAmount)", strExpressionTotal);
                }
                catch (Exception ex)
                {

                }

                for (int i = intFromInstall; i <= intToInstall; i++)
                {
                    decimal decCurrentPrincipal = 0, decAmortAmount = 0;
                    DateTime dtInstallDate = new DateTime();

                    string strExpression = "InstallmentNo='" + i.ToString() + "'";
                    try
                    {
                        decCurrentPrincipal = (decimal)dtRepaymentStructure.Compute("sum(PrincipalAmount)", strExpression);
                        dtInstallDate = (DateTime)dtRepaymentStructure.Compute("max(InstallmentDate)", strExpression);
                    }
                    catch (Exception ex)
                    {

                    }

                    decAmortAmount = decPerInstallAmount * (decCurrentPrincipal / decTotalPrincipal);

                    //Adding new repayment details
                    //DtRepayGrid_Amort
                    DataRow drAmort = DtRepayGrid_Amort.NewRow();
                    drAmort["CashFlow"] = drOther["CashFlow"].ToString();
                    drAmort["CashFlowId"] = drOther["CashFlowId"].ToString();
                    drAmort["Accounting_IRR"] = Convert.ToBoolean(drOther["Accounting_IRR"]);
                    drAmort["Business_IRR"] = Convert.ToBoolean(drOther["Business_IRR"]);
                    drAmort["Company_IRR"] = Convert.ToBoolean(drOther["Company_IRR"]);
                    drAmort["CashFlow_Flag_ID"] = drOther["CashFlow_Flag_ID"].ToString();
                    drAmort["PerInstall"] = Convert.ToDecimal(decAmortAmount.ToString("0.00"));
                    drAmort["Breakup"] = drOther["Breakup"].ToString();
                    drAmort["FromInstall"] = i.ToString();
                    drAmort["ToInstall"] = i.ToString();
                    drAmort["Amount"] = drAmort["Amount"];

                    drAmort["Outflow"] = drOther["Outflow"].ToString(); ;//Inflow/Outflow/Both
                    drAmort["Amort"] = drOther["Amort"].ToString(); ;//Recovery type
                    drAmort["Entity"] = drOther["Entity"].ToString();//Entity
                    drAmort["TotalPeriodInstall"] = drOther["TotalPeriodInstall"].ToString();
                    drAmort["FromDate"] = dtInstallDate.ToString();
                    drAmort["ToDate"] = dtInstallDate.ToString();

                    DtRepayGrid_Amort.Rows.Add(drAmort);


                    //Adding new repayment details
                    DataRow[] drRepayStructNew = dtRepaymentStructure_New.Select(strExpression);

                    decimal decAmount = 0;

                    switch (drOther["Outflow"].ToString())
                    {
                        case "1"://Inflow
                        case "3"://Both
                            if (drOther["Entity"].ToString() == "3")//Customer
                            {

                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["Others"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["Others"].ToString());
                                drRepayStructNew[0]["Others"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                //drIRR["TransactionType"] = "Others";
                            }
                            else//dealer
                            {
                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["ET_IW"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["ET_IW"].ToString());
                                drRepayStructNew[0]["ET_IW"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                //drIRR["TransactionType"] = "ET_IW";
                            }

                            break;
                        case "2"://outflow
                            if (drOther["Entity"].ToString() == "3")//Customer
                            {
                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["CUS_OW"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["CUS_OW"].ToString());
                                drRepayStructNew[0]["CUS_OW"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                // drIRR["TransactionType"] = "CUS_OW";
                            }

                            else//dealer
                            {
                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["ET_OW"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["ET_OW"].ToString());
                                drRepayStructNew[0]["ET_OW"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                //drIRR["TransactionType"] = "ET_OW";
                            }

                            break;
                        default:
                            if (!string.IsNullOrEmpty(drRepayStructNew[0]["Others"].ToString()))
                                decAmount = Convert.ToDecimal(drRepayStructNew[0]["Others"].ToString());
                            drRepayStructNew[0]["Others"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                            //drIRR["TransactionType"] = "Others";
                            break;
                    }
                }


            }
            dtRepaymentStructure = dtRepaymentStructure_New.Copy();
            dtRepayDetailsOthers = DtRepayGrid_Amort.Copy();
        }



        //}

    }


    public void FunPubCalculateIRR(string strDocumentDate, string strPLR, string strFrequency, string strDateFormat, string strResidualAmount, string strResidualPercentage, out double decAccountingIRR, out double decBusinessIRR, out double decCompanyIRR, out DataTable dtRepaymentStructure, out DataTable dtRepayDetails, out DataTable dtRepayDetailsOthers, DataTable DtRepayGrid, DataTable dtCashInflow, DataTable dtCashOutflow, string strFinanceAmount, string strMarginPercentage, string strReturnPattern, string strRate, string strTenureType, string strTenure, string strIRRRest, string strTimeValue, string strLOB, string strRepaymentMode, string strMorType, DataTable dtMoratoriumInput, string strLevy, int intDeffered, string dtDocFBDate)
    {
        decimal decActualAmount, decTotalAmount = 0;
        string strType;
        string stroption;
        decAccountingIRR = 0;
        decBusinessIRR = 0;
        decCompanyIRR = 0;
        decimal decRate;
        double docRate;
        decimal? decResvalue = null;
        decimal? decResAmt = null;
        RepaymentType rePayType = new RepaymentType();
        strType = strLOB.ToLower().Split('-')[0].Trim();
        if (strRate == "") strRate = "0";
        switch (strType)
        {
            case "te":
            case "tl":
                if (strRepaymentMode == "5")
                {
                    rePayType = RepaymentType.TLE;
                }
                else
                    rePayType = RepaymentType.EMI;
                break;
            case "ft":
                rePayType = RepaymentType.FC;
                break;
            case "wc":
                rePayType = RepaymentType.WC;
                break;
            default:
                rePayType = RepaymentType.EMI;
                break;
        }

        if (strReturnPattern == "3" || strReturnPattern == "4" || strReturnPattern == "5")
        {
            stroption = "3";
        }
        else if (strReturnPattern == "6")
        {
            stroption = "6";
        }
        else
        {
            stroption = "2";
        }

        CommonS3GBusLogic ObjBusinessLogic = new CommonS3GBusLogic();


        double decResultIrr = 0;
        decimal decPLR = 0;
        if (strPLR != "")
        {
            decPLR = Convert.ToDecimal(strPLR);
        }

        switch (strIRRRest.ToLower())
        {
            case "1":
                strIRRRest = "daily";
                break;
            case "2":
                strIRRRest = "monthly";
                break;
            case "3":
                strIRRRest = "yearly";
                break;
            default:
                strIRRRest = "daily";
                break;

        }
        switch (strTimeValue.ToLower())
        {
            case "1":
                strTimeValue = "advance";
                break;
            case "2":
                strTimeValue = "arrears";
                break;
            case "3":
                strTimeValue = "advancefbd";
                break;
            case "4":
                strTimeValue = "arrearsfbd";
                break;
            default:

                strTimeValue = "advance";
                break;
        }

        decRate = 0;//changed on 1st feb2011
        switch (strReturnPattern.ToUpper())
        {

            case "2":
                //ObjBusinessLogic.FunPubCalculateFlatRate(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat, FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), Convert.ToDouble(strRate), strIRRRest, "Empty", strTimeValue, Convert.ToDecimal(0.10), IRRType.Accounting_IRR, out docRate, Convert.ToDecimal(10.05), decPLR);
                //decRate = Convert.ToDecimal(docRate.ToString("0.0000"));
                break;
            case "1":
                decRate = Convert.ToDecimal(strRate);
                break;
            default:
                decRate = Convert.ToDecimal(strRate);
                break;
        }

        if (strResidualAmount != "")
        {
            decResAmt = Convert.ToDecimal(strResidualAmount);
        }
        if (strResidualPercentage != "")
        {
            decResvalue = Convert.ToDecimal(strResidualPercentage);
        }

        //if (((strLOB.ToUpper().Contains("TL")) || (strLOB.ToUpper().Contains("TE"))) && (stroption == "6"))
        //{
        DataSet ds = new DataSet();
        if (intDeffered == 1)//For Defferred payment
        {
            ds = ObjBusinessLogic.FunPubGenerateRepaymentStructure_Principal_MethodNew(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
     FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
     decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType, Convert.ToInt32(strLevy), Utility.StringToDate(dtDocFBDate));
        }
        else
        {
            ds = ObjBusinessLogic.FunPubGenerateRepaymentStructure_Principal_MethodNew(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
                   FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
                   decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType, Convert.ToInt32(strLevy), Utility.StringToDate(dtDocFBDate));

        }
        dtRepaymentStructure = ds.Tables[0];
        dtRepayDetails = ds.Tables[1];
        //}
        //else
        //{
        //    dtRepaymentStructure = ObjBusinessLogic.FunPubGenerateRepaymentStructure(DtRepayGrid, dtCashInflow, dtCashOutflow, strFrequency, Convert.ToInt32(strTenure), strTenureType, strDateFormat,
        //        FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage), decRate, strIRRRest, strTimeValue, "Empty", Convert.ToDecimal(0.10), Convert.ToDecimal(10.05), decPLR, Utility.StringToDate(strDocumentDate),
        //        decResvalue, decResAmt, rePayType, out decAccountingIRR, out decBusinessIRR, out decCompanyIRR, strMorType, dtMoratoriumInput, strType);
        //}

        //code added by saran in 3-Jul-2014 CR_SISSL12E046_018 start
        //Others
        dtRepayDetailsOthers = null;
        DataTable DtRepayGrid_New = DtRepayGrid.Copy();
        DataTable DtRepayGrid_Amort = DtRepayGrid.Clone();
        DataTable dtRepaymentStructure_New = dtRepaymentStructure.Copy();
        DataRow[] drRepay_OthersArray = DtRepayGrid.Select("CashFlow_Flag_ID <> 23 and CashFlow_Flag_ID <> 25 and Amort='2'", "slno asc");

        if (drRepay_OthersArray.Length > 0)
        {
            foreach (DataRow drOther in drRepay_OthersArray)
            {
                int intFromInstall = 0, intToInstall = 0;
                decimal decTotalPrincipal = 0, decPerInstallAmount = 0;

                intFromInstall = Convert.ToInt16(drOther["FromInstall"].ToString());
                intToInstall = Convert.ToInt16(drOther["ToInstall"].ToString());

                decPerInstallAmount = Convert.ToDecimal(drOther["PerInstall"].ToString());

                string strExpressionTotal = "InstallmentNo >='" + intFromInstall.ToString() + "' and InstallmentNo <='" + intToInstall.ToString() + "'";
                try
                {
                    decTotalPrincipal = (decimal)dtRepaymentStructure.Compute("sum(PrincipalAmount)", strExpressionTotal);
                }
                catch (Exception ex)
                {

                }

                for (int i = intFromInstall; i <= intToInstall; i++)
                {
                    decimal decCurrentPrincipal = 0, decAmortAmount = 0;
                    DateTime dtInstallDate = new DateTime();

                    string strExpression = "InstallmentNo='" + i.ToString() + "'";
                    try
                    {
                        decCurrentPrincipal = (decimal)dtRepaymentStructure.Compute("sum(PrincipalAmount)", strExpression);
                        dtInstallDate = (DateTime)dtRepaymentStructure.Compute("max(InstallmentDate)", strExpression);
                    }
                    catch (Exception ex)
                    {

                    }

                    decAmortAmount = decPerInstallAmount * (decCurrentPrincipal / decTotalPrincipal);

                    //Adding new repayment details
                    //DtRepayGrid_Amort
                    DataRow drAmort = DtRepayGrid_Amort.NewRow();
                    drAmort["CashFlow"] = drOther["CashFlow"].ToString();
                    drAmort["CashFlowId"] = drOther["CashFlowId"].ToString();
                    drAmort["Accounting_IRR"] = Convert.ToBoolean(drOther["Accounting_IRR"]);
                    drAmort["Business_IRR"] = Convert.ToBoolean(drOther["Business_IRR"]);
                    drAmort["Company_IRR"] = Convert.ToBoolean(drOther["Company_IRR"]);
                    drAmort["CashFlow_Flag_ID"] = drOther["CashFlow_Flag_ID"].ToString();
                    drAmort["PerInstall"] = Convert.ToDecimal(decAmortAmount.ToString("0.00"));
                    drAmort["Breakup"] = drOther["Breakup"].ToString();
                    drAmort["FromInstall"] = i.ToString();
                    drAmort["ToInstall"] = i.ToString();
                    drAmort["Amount"] = drAmort["Amount"];

                    drAmort["Outflow"] = drOther["Outflow"].ToString(); ;//Inflow/Outflow/Both
                    drAmort["Amort"] = drOther["Amort"].ToString(); ;//Recovery type
                    drAmort["Entity"] = drOther["Entity"].ToString();//Entity
                    drAmort["TotalPeriodInstall"] = drOther["TotalPeriodInstall"].ToString();
                    drAmort["FromDate"] = dtInstallDate.ToString();
                    drAmort["ToDate"] = dtInstallDate.ToString();

                    DtRepayGrid_Amort.Rows.Add(drAmort);


                    //Adding new repayment details
                    DataRow[] drRepayStructNew = dtRepaymentStructure_New.Select(strExpression);

                    decimal decAmount = 0;

                    switch (drOther["Outflow"].ToString())
                    {
                        case "1"://Inflow
                        case "3"://Both
                            if (drOther["Entity"].ToString() == "3")//Customer
                            {

                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["Others"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["Others"].ToString());
                                drRepayStructNew[0]["Others"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                //drIRR["TransactionType"] = "Others";
                            }
                            else//dealer
                            {
                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["ET_IW"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["ET_IW"].ToString());
                                drRepayStructNew[0]["ET_IW"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                //drIRR["TransactionType"] = "ET_IW";
                            }

                            break;
                        case "2"://outflow
                            if (drOther["Entity"].ToString() == "3")//Customer
                            {
                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["CUS_OW"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["CUS_OW"].ToString());
                                drRepayStructNew[0]["CUS_OW"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                // drIRR["TransactionType"] = "CUS_OW";
                            }

                            else//dealer
                            {
                                if (!string.IsNullOrEmpty(drRepayStructNew[0]["ET_OW"].ToString()))
                                    decAmount = Convert.ToDecimal(drRepayStructNew[0]["ET_OW"].ToString());
                                drRepayStructNew[0]["ET_OW"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                                //drIRR["TransactionType"] = "ET_OW";
                            }

                            break;
                        default:
                            if (!string.IsNullOrEmpty(drRepayStructNew[0]["Others"].ToString()))
                                decAmount = Convert.ToDecimal(drRepayStructNew[0]["Others"].ToString());
                            drRepayStructNew[0]["Others"] = Convert.ToDecimal((decAmount + decAmortAmount).ToString("0.00"));
                            //drIRR["TransactionType"] = "Others";
                            break;
                    }
                }


            }
            dtRepaymentStructure = dtRepaymentStructure_New.Copy();
            dtRepayDetailsOthers = DtRepayGrid_Amort.Copy();
        }

    }

    #endregion

    public void FunPubAddRepaymentforTL(out DateTime dtNextFromdate, out string strErrorMessage, out DataTable DtRepaySchedule, DataTable DtRepayGrid, Dictionary<string, string> objMethodParameters)
    {
        DateTime dtTodate;
        strErrorMessage = "";
        int intNoOfInstalment = 0;
        string repayMode_id = string.Empty;
        string strFrequency = string.Empty;
        string strLevyy = string.Empty;
        strFrequency = objMethodParameters["Frequency"].ToString();
        strLevyy = objMethodParameters["Levy"].ToString();




        LineOfBus = objMethodParameters["LOB"].ToString();
        RepayMode = objMethodParameters["REPAYMODE"].ToString();
        repayMode_id = objMethodParameters["repayMode_id"].ToString();

        DateTime dtFromdate = DateTime.Now;
        DataRow dr = DtRepayGrid.NewRow();

        dr["Slno"] = (DtRepayGrid.Rows.Count + 1).ToString();
        dr["CashFlow"] = objMethodParameters["CashFlow"].ToString();
        string[] strIds = objMethodParameters["CashFlowId"].ToString().Split(',');
        dr["CashFlowId"] = strIds[0];
        dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strIds[1]));
        dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strIds[2]));
        dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strIds[3]));
        dr["CashFlow_Flag_ID"] = strIds[4];
        dr["PerInstall"] = objMethodParameters["PerInstall"].ToString();
        dr["Breakup"] = objMethodParameters["Breakup"].ToString() == "" ? 0 : Convert.ToDecimal(objMethodParameters["Breakup"].ToString());
        dr["FromInstall"] = objMethodParameters["FromInstall"].ToString();
        dr["ToInstall"] = objMethodParameters["ToInstall"].ToString();
        dr["Amount"] = 0;


        //code added by saran in 1-Jul-2014 CR_SISSL12E046_018 start
        dr["Outflow"] = strIds[5];//Inflow/Outflow/Both
        dr["Amort"] = strIds[6];//Recovery type
        dr["Entity"] = strIds[7];//Entity
        //code added by saran in 1-Jul-2014 CR_SISSL12E046_018 start

        intNoOfInstalment = Convert.ToInt32(objMethodParameters["ToInstall"].ToString()) - Convert.ToInt32(objMethodParameters["FromInstall"].ToString()) + 1;

        dr["TotalPeriodInstall"] = Convert.ToDecimal(objMethodParameters["PerInstall"].ToString()) * (intNoOfInstalment);
        //if ((LineOfBus.ToUpper().Contains("TL") || LineOfBus.ToUpper().Contains("TE")) && (RepayMode.ToUpper() != "PRODUCT"))
        if ((LineOfBus.ToUpper().Contains("TL") || LineOfBus.ToUpper().Contains("TE")) && (repayMode_id == "2"))//Only for structure fixed 
        {
            if ((strIds[4] == "91") || (strIds[4] == "35"))
            {
                dr["FromDate"] = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
                dtFromdate = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
            }
            else
            {
                if (DtRepayGrid.Select("CashFlow_Flag_ID in(91,35)").Length > 0)
                {
                    DataRow[] drRow;
                    drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' and ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    if (drRow.Length == 0)
                    {
                        drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' or ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    }
                    int intNextInstall = Convert.ToInt32(drRow[0]["ToInstall"].ToString());
                    int intNoOfInstall = Convert.ToInt32(objMethodParameters["FromInstall"].ToString()) - intNextInstall + 1;
                    DateTime dtDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(strFrequency, Convert.ToDateTime(drRow[0]["ToDate"].ToString()), (intNoOfInstall - 1));
                    dr["FromDate"] = dtDate;
                    dtFromdate = dtDate;
                }
                //else
                //{
                //    strErrorMessage = "Cannot add other cashflows before adding Principal";
                //    DtRepaySchedule = null;
                //    dtNextFromdate = DateTime.Now.Date;
                //    return;
                //}
            }

        }
        else
        {
            //Setting Levy as frequency 
            //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 start
            if (Convert.ToInt32(strLevyy) > 0)
            {
                if (Convert.ToInt32(strFrequency) > Convert.ToInt32(strLevyy))
                {
                    strFrequency = strLevyy;
                }
            }
            //Added by saran on 4-Jul-2014 for CR_SISSL12E046_018 end
            if (strIds[4] == "23")
            {
                dr["FromDate"] = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
                dtFromdate = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
            }
            else
            {
                if (DtRepayGrid.Select("CashFlow_Flag_ID=23").Length > 0)
                {
                    DataRow[] drRow;
                    drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' and ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    if (drRow.Length == 0)
                    {
                        drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' or ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    }
                    int intNextInstall = Convert.ToInt32(drRow[0]["ToInstall"].ToString());
                    int intNoOfInstall = Convert.ToInt32(objMethodParameters["FromInstall"].ToString()) - intNextInstall + 1;
                    DateTime dtDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(strFrequency, Convert.ToDateTime(drRow[0]["ToDate"].ToString()), (intNoOfInstall - 1));
                    dr["FromDate"] = dtDate;
                    dtFromdate = dtDate;
                }
                else
                {
                    strErrorMessage = "Cannot add other cashflows before adding installment";
                    DtRepaySchedule = null;
                    dtNextFromdate = DateTime.Now.Date;
                    return;
                }
            }
        }
        dtTodate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(strFrequency, dtFromdate, (intNoOfInstalment - 1));
        dtNextFromdate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(strFrequency, dtTodate);
        dr["ToDate"] = dtTodate.ToString();


        if (!FunPubValidateTenurePeriod(Utility.StringToDate(objMethodParameters["DocumentDate"].ToString()), dtTodate, objMethodParameters["TenureType"].ToString(), objMethodParameters["Tenure"].ToString()))
        {
            strErrorMessage = "Repayment schedule should not exceed tenure period(" + objMethodParameters["Tenure"].ToString() + " " + objMethodParameters["TenureType"].ToString() + ")";
            DtRepaySchedule = null;
            return;
        }
        else
        {
            DtRepayGrid.Rows.Add(dr);
            DtRepaySchedule = DtRepayGrid;
        }
    }
    public void


        FunPubAddRepayment(out DateTime dtNextFromdate, out string strErrorMessage, out DataTable DtRepaySchedule, DataTable DtRepayGrid, Dictionary<string, string> objMethodParameters)
    {
        DateTime dtTodate;
        strErrorMessage = "";
        int intNoOfInstalment = 0;

        //LineOfBus = objMethodParameters["LOB"].ToString();
        //RepayMode = objMethodParameters["REPAYMODE"].ToString();

        DateTime dtFromdate = DateTime.Now;
        DataRow dr = DtRepayGrid.NewRow();

        dr["CashFlow"] = objMethodParameters["CashFlow"].ToString();
        string[] strIds = objMethodParameters["CashFlowId"].ToString().Split(',');
        dr["CashFlowId"] = strIds[0];
        dr["Accounting_IRR"] = Convert.ToBoolean(Convert.ToByte(strIds[1]));
        dr["Business_IRR"] = Convert.ToBoolean(Convert.ToByte(strIds[2]));
        dr["Company_IRR"] = Convert.ToBoolean(Convert.ToByte(strIds[3]));
        dr["CashFlow_Flag_ID"] = Convert.ToInt16(strIds[4]);
        dr["PerInstall"] = objMethodParameters["PerInstall"].ToString();
        dr["Breakup"] = objMethodParameters["Breakup"].ToString() == "" ? 0 : Convert.ToDecimal(objMethodParameters["Breakup"].ToString());
        dr["FromInstall"] = objMethodParameters["FromInstall"].ToString();
        dr["ToInstall"] = objMethodParameters["ToInstall"].ToString();
        dr["Amount"] = 0;

        //code added by saran in 1-Jul-2014 CR_SISSL12E046_018 start
        dr["Outflow"] = strIds[5];//Inflow/Outflow/Both
        dr["Amort"] = strIds[6];//Recovery type
        dr["Entity"] = strIds[7];//Entity
        //code added by saran in 1-Jul-2014 CR_SISSL12E046_018 start
        intNoOfInstalment = Convert.ToInt32(objMethodParameters["ToInstall"].ToString()) - Convert.ToInt32(objMethodParameters["FromInstall"].ToString()) + 1;

        dr["TotalPeriodInstall"] = Convert.ToDecimal(objMethodParameters["PerInstall"].ToString()) * (intNoOfInstalment);
        if ((LineOfBus.ToUpper().Contains("TL")) && (RepayMode.ToUpper() != "PRODUCT"))
        {
            if (strIds[4] == "91")
            {
                dr["FromDate"] = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
                dtFromdate = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
            }
            else
            {
                if (DtRepayGrid.Select("CashFlow_Flag_ID in(91,35)").Length > 0)
                {
                    DataRow[] drRow;
                    drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' and ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    if (drRow.Length == 0)
                    {
                        drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' or ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    }
                    int intNextInstall = Convert.ToInt32(drRow[0]["ToInstall"].ToString());
                    int intNoOfInstall = Convert.ToInt32(objMethodParameters["FromInstall"].ToString()) - intNextInstall + 1;
                    DateTime dtDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(objMethodParameters["Frequency"].ToString(), Convert.ToDateTime(drRow[0]["ToDate"].ToString()), (intNoOfInstall - 1));
                    dr["FromDate"] = dtDate;
                    dtFromdate = dtDate;
                }
                else
                {
                    strErrorMessage = "Cannot add other cashflows before adding Principal";
                    DtRepaySchedule = null;
                    dtNextFromdate = DateTime.Now.Date;
                    return;
                }
            }

        }
        else
        {
            if (strIds[4] == "23")
            {
                dr["FromDate"] = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
                dtFromdate = Utility.StringToDate(objMethodParameters["FromDate"].ToString());
            }
            else
            {
                if (DtRepayGrid.Select("CashFlow_Flag_ID=23").Length > 0)
                {
                    DataRow[] drRow;
                    drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' and ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    if (drRow.Length == 0)
                    {
                        drRow = DtRepayGrid.Select("FromInstall <='" + objMethodParameters["FromInstall"].ToString() + "' or ToInstall >='" + objMethodParameters["FromInstall"].ToString() + "'");
                    }
                    int intNextInstall = Convert.ToInt32(drRow[0]["ToInstall"].ToString());
                    int intNoOfInstall = Convert.ToInt32(objMethodParameters["FromInstall"].ToString()) - intNextInstall + 1;
                    DateTime dtDate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(objMethodParameters["Frequency"].ToString(), Convert.ToDateTime(drRow[0]["ToDate"].ToString()), (intNoOfInstall - 1));
                    dr["FromDate"] = dtDate;
                    dtFromdate = dtDate;
                }
                else
                {
                    strErrorMessage = "Cannot add other cashflows before adding installment";
                    DtRepaySchedule = null;
                    dtNextFromdate = DateTime.Now.Date;
                    return;
                }
            }
        }
        dtTodate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(objMethodParameters["Frequency"].ToString(), dtFromdate, (intNoOfInstalment - 1));
        dtNextFromdate = S3GBusEntity.CommonS3GBusLogic.FunPubGetNextDate(objMethodParameters["Frequency"].ToString(), dtTodate);
        dr["ToDate"] = dtTodate.ToString();
        string strTenure = objMethodParameters["Tenure"].ToString();
        if (objMethodParameters.ContainsKey("Time_Value"))
        {
            if (!string.IsNullOrEmpty(objMethodParameters["Time_Value"].ToString()))
            {
                strTenure = (Convert.ToInt32(strTenure) - 1).ToString();
            }
        }

        if (!FunPubValidateTenurePeriod(Utility.StringToDate(objMethodParameters["DocumentDate"].ToString()), dtTodate, objMethodParameters["TenureType"].ToString(), strTenure))
        {
            strErrorMessage = "Repayment schedule should not exceed tenure period(" + objMethodParameters["Tenure"].ToString() + " " + objMethodParameters["TenureType"].ToString() + ")";
            DtRepaySchedule = null;
            return;
        }
        else
        {
            DtRepayGrid.Rows.Add(dr);
            DtRepaySchedule = DtRepayGrid;
        }
    }


    public DataSet FunPubGenerateRepaymentSchedule(DateTime dtStartDate, Dictionary<string, string> objMethodParameters, DataTable dtMoratoriumInput)
    {

        string strType;
        strType = objMethodParameters["LOB"].ToString().ToLower().Split('-')[0].Trim();

        S3GBusEntity.CommonS3GBusLogic ObjCommonBusLogic = new CommonS3GBusLogic();
        DataTable dtRepay = new DataTable("Repayment Structure");
        DataSet dsRepayment = null;
        int tenure = Convert.ToInt32(objMethodParameters["Tenure"].ToString()); ;
        decimal decRate;
        decimal decFinAmount = FunPubGetAmountFinanced(objMethodParameters["FinanceAmount"].ToString(), objMethodParameters["MarginPercentage"].ToString());
        string strTimeVal = string.Empty;
        decRate = 0;//changed on 1st feb2011

        //switch (objMethodParameters["ReturnPattern"].ToString())
        //{
        //    case "2":
        //    case "1":

        //        break;
        //}
        if (!string.IsNullOrEmpty(objMethodParameters["Rate"].ToString()))
            decRate = Convert.ToDecimal(objMethodParameters["Rate"].ToString());

        switch (objMethodParameters["TimeValue"].ToString())
        {

            case "1":
                strTimeVal = "advance";
                break;
            case "2":
                strTimeVal = "arrears";
                break;
            case "3":
                strTimeVal = "advancefbd";
                break;
            case "4":
                strTimeVal = "arrearsfbd";
                break;
            default:

                strTimeVal = "advance";
                break;
        }
        RepaymentType rePayType = new RepaymentType();
        switch (objMethodParameters["RepaymentMode"].ToString())
        {
            case "1":
                if (objMethodParameters["ReturnPattern"].ToString() != "3" || objMethodParameters["ReturnPattern"].ToString() != "4" || objMethodParameters["ReturnPattern"].ToString() != "5")
                {
                    rePayType = RepaymentType.EMI;
                }
                else
                {
                    rePayType = RepaymentType.Others;
                }
                break;
            default:

                rePayType = RepaymentType.Others;

                break;

        }

        switch (objMethodParameters["ReturnPattern"].ToString())
        {
            case "3":
                if (objMethodParameters["RepaymentMode"].ToString() != "3")
                    rePayType = RepaymentType.PMPT;
                break;
            case "4":
                if (objMethodParameters["RepaymentMode"].ToString() != "3")
                    rePayType = RepaymentType.PMPL;
                break;
            case "5":
                if (objMethodParameters["RepaymentMode"].ToString() != "3")
                    rePayType = RepaymentType.PMPM;
                break;
        }

        bool blnRecovery = false; // For OL & FL with structure fixed 
        if (Convert.ToInt16(objMethodParameters["ReturnPattern"].ToString()) > 2 && objMethodParameters["RepaymentMode"].ToString() == "3")
        {
            blnRecovery = true;
        }

        switch (strType.ToLower())
        {
            case "tl":
            case "te":
                if (objMethodParameters["RepaymentMode"].ToString() == "5")
                {
                    rePayType = RepaymentType.TLE;
                }
                //tenure = 1;
                break;
            case "ft":
                rePayType = RepaymentType.FC;
                // tenure = 1;
                break;
            case "wc":
                rePayType = RepaymentType.WC;
                //tenure = 1;
                break;
        }
        /* If Repayment Mode is Structured Adhoc Pattern - Return Empty Table */
        if (objMethodParameters["RepaymentMode"].ToString() == "2")
        {
            return dsRepayment;
        }
        DataTable dtCashFlowTable = FunPubGetCashFlowDetails(Convert.ToInt32(objMethodParameters["CompanyId"]), Convert.ToInt32(objMethodParameters["LobId"]));
        if (dtCashFlowTable.Rows.Count > 0)
        {
            int intCashflowId = Convert.ToInt32(dtCashFlowTable.Rows[0]["CashFlow_ID"].ToString());
            string strCashflowdesc = Convert.ToString(dtCashFlowTable.Rows[0]["CashFlow_Description"].ToString());
            bool blnAccountingIRR = Convert.ToBoolean(dtCashFlowTable.Rows[0]["Accounting_IRR"].ToString());
            bool blnBusinessIRR = Convert.ToBoolean(dtCashFlowTable.Rows[0]["Business_IRR"].ToString());
            bool blnCompanyIRR = Convert.ToBoolean(dtCashFlowTable.Rows[0]["Company_IRR"].ToString());
            if (rePayType != RepaymentType.Others)
            {

                dsRepayment = ObjCommonBusLogic.FunPubGetRepaymentDetails(objMethodParameters["Frequency"].ToString(), tenure,
                    objMethodParameters["TenureType"].ToString(), decFinAmount,
                    decRate, rePayType, null, dtStartDate,
                    Utility.StringToDate(objMethodParameters["DocumentDate"].ToString()),
                    intCashflowId, strCashflowdesc, decimal.Parse(objMethodParameters["Roundoff"].ToString()), blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, strTimeVal, dtMoratoriumInput);

                dtRepay = dsRepayment.Tables["Table1"];


            }
            else
            {
                if (objMethodParameters["RepaymentMode"].ToString() != "2")
                {

                    switch (objMethodParameters["ReturnPattern"].ToString())
                    {
                        case "3":
                            rePayType = RepaymentType.PMPT;
                            break;
                        case "4":
                            rePayType = RepaymentType.PMPL;
                            break;
                        case "5":
                            rePayType = RepaymentType.PMPM;
                            break;
                    }

                    decimal[] decRecoveryPattern = { Convert.ToDecimal(objMethodParameters["RecoveryYear1"].ToString()), Convert.ToDecimal(objMethodParameters["RecoveryYear2"].ToString()), 
                                                           Convert.ToDecimal(objMethodParameters["RecoveryYear3"].ToString()), Convert.ToDecimal(objMethodParameters["RecoveryYear4"].ToString()) };

                    dsRepayment = ObjCommonBusLogic.FunPubGetRepaymentDetails(objMethodParameters["Frequency"].ToString(), tenure,
                    objMethodParameters["TenureType"].ToString(), decFinAmount,
                    decRate, rePayType, null, dtStartDate,
                    Utility.StringToDate(objMethodParameters["DocumentDate"].ToString()),
                    intCashflowId, strCashflowdesc, decimal.Parse(objMethodParameters["Roundoff"].ToString()), blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, strTimeVal, blnRecovery, dtMoratoriumInput, decRecoveryPattern);
                    dtRepay = dsRepayment.Tables["Table1"];
                }
            }
        }
        return dsRepayment;// dtRepay;
    }

    public DataSet FunPubGenerateRepaymentSchedule(DateTime dtStartDate, DataTable dtCashInflow, DataTable dtCashOutflow, Dictionary<string, string> objMethodParameters, DataTable dtMoratoriumInput, out decimal decRateOut)
    {

        string strType;
        strType = objMethodParameters["LOB"].ToString().ToLower().Split('-')[0].Trim();

        S3GBusEntity.CommonS3GBusLogic ObjCommonBusLogic = new CommonS3GBusLogic();
        DataTable dtRepay = new DataTable("Repayment Structure");
        DataSet dsRepayment = new DataSet();
        int tenure = Convert.ToInt32(objMethodParameters["Tenure"].ToString()); ;
        decimal decRate;
        decimal decFinAmount = FunPubGetAmountFinanced(objMethodParameters["FinanceAmount"].ToString(), objMethodParameters["MarginPercentage"].ToString());
        string strTimeVal = string.Empty;
        decRate = 0;//changed on 1st feb2011
        decRateOut = decRate;
        switch (objMethodParameters["ReturnPattern"].ToString())
        {
            case "1":
                decRate = Convert.ToDecimal(objMethodParameters["Rate"].ToString());
                break;
            default:
                decRate = Convert.ToDecimal(objMethodParameters["Rate"].ToString());
                break;
        }

        switch (objMethodParameters["TimeValue"].ToString())
        {

            case "1":
            case "3":
                strTimeVal = "advance";//Hard Coded for testing 
                break;
            case "2":
            case "4":
                strTimeVal = "arrears";
                break;
            default:
                strTimeVal = "advance";
                break;
        }
        RepaymentType rePayType = new RepaymentType();
        switch (objMethodParameters["RepaymentMode"].ToString())
        {
            case "1":
                if (objMethodParameters["ReturnPattern"].ToString() != "3" || objMethodParameters["ReturnPattern"].ToString() != "4" || objMethodParameters["ReturnPattern"].ToString() != "5")
                {
                    rePayType = RepaymentType.EMI;
                }
                else
                {
                    rePayType = RepaymentType.Others;
                }
                break;
            default:

                rePayType = RepaymentType.Others;

                break;

        }

        switch (objMethodParameters["ReturnPattern"].ToString())
        {
            case "3":
                rePayType = RepaymentType.PMPT;
                break;
            case "4":
                rePayType = RepaymentType.PMPL;
                break;
            case "5":
                rePayType = RepaymentType.PMPM;
                break;
        }

        bool blnRecovery = false; // For OL & FL with structure fixed 
        if (Convert.ToInt16(objMethodParameters["ReturnPattern"].ToString()) > 2 && objMethodParameters["RepaymentMode"].ToString() == "3")
        {
            blnRecovery = true;
        }


        switch (strType.ToLower())
        {
            case "tl":
            case "te":
                if (objMethodParameters["RepaymentMode"].ToString() == "5")
                {
                    rePayType = RepaymentType.TLE;
                }
                //tenure = 1;
                break;
            case "ft":
                rePayType = RepaymentType.FC;
                // tenure = 1;
                break;
            case "wc":
                rePayType = RepaymentType.WC;
                //tenure = 1;
                break;
        }

        DataTable dtCashFlowTable;
        if (objMethodParameters.ContainsKey("PrincipalMethod") && objMethodParameters["PrincipalMethod"] == "1")
        {
            dtCashFlowTable = FunPubGetCashFlowDetails_TL_Princ(Convert.ToInt32(objMethodParameters["CompanyId"]), Convert.ToInt32(objMethodParameters["LobId"]));
        }
        else
        {
            dtCashFlowTable = FunPubGetCashFlowDetails(Convert.ToInt32(objMethodParameters["CompanyId"]), Convert.ToInt32(objMethodParameters["LobId"]));
        }
        if (dtCashFlowTable.Rows.Count > 0)
        {
            int intCashflowId = Convert.ToInt32(dtCashFlowTable.Rows[0]["CashFlow_ID"].ToString());
            string strCashflowdesc = Convert.ToString(dtCashFlowTable.Rows[0]["CashFlow_Description"].ToString());
            bool blnAccountingIRR = Convert.ToBoolean(dtCashFlowTable.Rows[0]["Accounting_IRR"].ToString());
            bool blnBusinessIRR = Convert.ToBoolean(dtCashFlowTable.Rows[0]["Business_IRR"].ToString());
            bool blnCompanyIRR = Convert.ToBoolean(dtCashFlowTable.Rows[0]["Company_IRR"].ToString());
            if (rePayType != RepaymentType.Others)
            {
                switch (objMethodParameters["ReturnPattern"].ToString())
                {

                    case "2":
                        ClsPubIRR clsIRR = new ClsPubIRR();
                        clsIRR.strFrequency = objMethodParameters["Frequency"].ToString();
                        clsIRR.intTenure = tenure;
                        clsIRR.strTenureType = objMethodParameters["TenureType"].ToString();
                        clsIRR.decPrincipleAmount = decFinAmount;
                        clsIRR.decRateofInt = decRate;
                        clsIRR.returnType = rePayType;
                        if (objMethodParameters.Keys.Contains("decResidualValue"))
                        {
                            clsIRR.decResidualValue = decimal.Parse(objMethodParameters["decResidualValue"].ToString());
                        }
                        clsIRR.dtimeStartDate = dtStartDate;
                        clsIRR.dtimeDocDate = Utility.StringToDate(objMethodParameters["DocumentDate"].ToString());
                        clsIRR.intCashFlow = intCashflowId;
                        clsIRR.strCashflowDesc = strCashflowdesc;
                        clsIRR.decRoundOff = decimal.Parse(objMethodParameters["Roundoff"].ToString());
                        clsIRR.blnAccountingIRR = blnAccountingIRR;
                        clsIRR.blnBusinessIRR = blnBusinessIRR;
                        clsIRR.blnCompanyIRR = blnCompanyIRR;
                        clsIRR.strTimeVal = strTimeVal;
                        clsIRR.dtCashInflow = dtCashInflow;
                        clsIRR.dtCashOutflow = dtCashOutflow;

                        clsIRR.strIRRrest = objMethodParameters["strIRRrest"].ToString();

                        clsIRR.decLimit = decimal.Parse(objMethodParameters["decLimit"].ToString());

                        clsIRR.decPLR = decimal.Parse("10.85");
                        clsIRR.decIRR = decRate;
                        if (objMethodParameters.Keys.Contains("decResidualAmount"))
                        {
                            clsIRR.decResidualAmount = decimal.Parse(objMethodParameters["decResidualAmount"].ToString());
                        }
                        clsIRR.Irrtype = IRRType.Business_IRR;
                        clsIRR.dtMoratoriumInput = dtMoratoriumInput;

                        dsRepayment = ObjCommonBusLogic.FunPubCalculateFlatRate(clsIRR, out decRate);



                        break;
                    default:
                        dsRepayment = ObjCommonBusLogic.FunPubGetRepaymentDetails(objMethodParameters["Frequency"].ToString(), tenure,
                           objMethodParameters["TenureType"].ToString(), decFinAmount,
                           decRate, rePayType, null, dtStartDate,
                           Utility.StringToDate(objMethodParameters["DocumentDate"].ToString()),
                           intCashflowId, strCashflowdesc, decimal.Parse(objMethodParameters["Roundoff"].ToString()), blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, strTimeVal, dtMoratoriumInput);

                        dtRepay = dsRepayment.Tables["Table1"];
                        break;
                }

            }
            else
            {
                if (objMethodParameters["RepaymentMode"].ToString() != "2")
                {
                    decimal[] decRecoveryPattern = { Convert.ToDecimal(objMethodParameters["RecoveryYear1"].ToString()), Convert.ToDecimal(objMethodParameters["RecoveryYear2"].ToString()), 
                                                           Convert.ToDecimal(objMethodParameters["RecoveryYear3"].ToString()), Convert.ToDecimal(objMethodParameters["RecoveryYear4"].ToString()) };
                    dsRepayment = ObjCommonBusLogic.FunPubGetRepaymentDetails(objMethodParameters["Frequency"].ToString(), tenure,
                   objMethodParameters["TenureType"].ToString(), decFinAmount,
                   decRate, rePayType, null, dtStartDate,
                   Utility.StringToDate(objMethodParameters["DocumentDate"].ToString()),
                   intCashflowId, strCashflowdesc, decimal.Parse(objMethodParameters["Roundoff"].ToString()), blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, strTimeVal, blnRecovery, dtMoratoriumInput, decRecoveryPattern);
                    dtRepay = dsRepayment.Tables["Table1"];
                }
            }
        }

        decRateOut = decRate;
        return dsRepayment;// dtRepay;
    }


    public DataTable FunPubGetCashFlowDetails(int intCompanyId, int intLobId)
    {
        Dictionary<string, string> objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Option", "23");
        objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
        objProcedureParameter.Add("@LOB_ID", intLobId.ToString());
        DataTable dtCashFlowTable = Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, objProcedureParameter);
        return dtCashFlowTable;
    }
    public DataTable FunPubGetCashFlowDetails_TL_Princ(int intCompanyId, int intLobId)
    {
        Dictionary<string, string> objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Option", "25");
        objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
        objProcedureParameter.Add("@LOB_ID", intLobId.ToString());
        DataTable dtCashFlowTable = Utility.GetDefaultData(SPNames.S3G_ORG_GetPricing_List, objProcedureParameter);
        return dtCashFlowTable;
    }

    public decimal FunPubGetInterestAmount(string strFinanceAmount, string strMarginPercentage, string strReturnPattern, string strRate, string strTenureType, string strTenure)
    {
        decimal decFinAmount = FunPubGetAmountFinanced(strFinanceAmount, strMarginPercentage);
        decimal decRate = 0;
        switch (strReturnPattern)
        {
            case "1":
            case "2":
                decRate = Convert.ToDecimal(strRate);
                break;
        }

        return Math.Round(S3GBusEntity.CommonS3GBusLogic.FunPubInterestAmount(strTenureType, decFinAmount, decRate, int.Parse(strTenure)), 0);
    }

    public void FunPubGetRepaymentTax(out DataTable DtRepaySchedule, out DataTable DtRepayStructure, Dictionary<string, string> objMethodParameters)
    {
        Dictionary<string, string> objProcedureParameter = new Dictionary<string, string>();
        DataSet ds = Utility.GetDataset("S3G_Test", objProcedureParameter);

        DtRepaySchedule = ds.Tables[0];
        DtRepayStructure = ds.Tables[1];
        decimal decITCAmount = Convert.ToDecimal(objMethodParameters["ITCAmount"].ToString());

        if (objMethodParameters["Recovery"].ToString() == "Staggered")
        {
            foreach (DataRow drRepay in DtRepayStructure.Rows)
            {
                if (drRepay["BillStatus"].ToString() != "1")
                {
                    decimal decInstAmount = Convert.ToDecimal(drRepay["InstallmentAmount"].ToString());
                    decimal decTaxAmount = (decInstAmount * Convert.ToDecimal(objMethodParameters["VAT"].ToString()) / 100);
                    if (objMethodParameters["SetOff"] == "Nettling")
                    {
                        if (decITCAmount > 0)
                        {
                            if ((decITCAmount - decTaxAmount) > 0)
                                drRepay["TAX"] = 0;
                            else
                                drRepay["TAX"] = decTaxAmount - decITCAmount;

                            decITCAmount = decITCAmount - decTaxAmount;
                        }
                        else
                        {
                            drRepay["TAX"] = decTaxAmount;
                        }
                    }
                    else
                    {
                        drRepay["TAX"] = decTaxAmount;
                    }
                    drRepay.AcceptChanges();
                }
            }
        }
        else
        {
            if (DtRepayStructure.Select("IsNull(BillStatus,0)=False").Length > 0)
            {
                DtRepayStructure = DtRepayStructure.Select("IsNull(BillStatus,0)=False").CopyToDataTable();
                decimal decTotalInsAmount = Convert.ToDecimal(DtRepayStructure.Compute("sum(InstallmentAmount)", "InstallmentAmount<>0 and IsNull(BillStatus,0)=False").ToString());
                decimal decTotalTaxAmount = (decTotalInsAmount * Convert.ToDecimal(objMethodParameters["VAT"].ToString()) / 100);

                if (DtRepayStructure.Rows[0] != null)
                {
                    if (objMethodParameters["SetOff"] == "Nettling")
                    {
                        decTotalTaxAmount = decTotalTaxAmount - decITCAmount;
                        if (decTotalTaxAmount > 0)
                            DtRepayStructure.Rows[0]["TAX"] = decTotalTaxAmount;
                        else
                            DtRepayStructure.Rows[0]["TAX"] = 0;
                    }
                    else
                    {
                        DtRepayStructure.Rows[0]["TAX"] = decTotalTaxAmount;
                    }
                    DtRepayStructure.Rows[0].AcceptChanges();
                }
            }
        }


        //DtRepayStructure.

        //DataTable dtNewSchedule = DtRepaySchedule.Clone();
        //DataRow drNewSchedule = dtNewSchedule.NewRow();
        //drNewSchedule["Account_Repay_ID"] = 3;
        //drNewSchedule["PANUM"] = "";
        //drNewSchedule["Repayment_CashFlow"] = "";
        //drNewSchedule["Amount"] = "";
        //drNewSchedule["Per_Instalment_Amount"] = "";
        //drNewSchedule["From_Instalment"] = "";
        //drNewSchedule["To_Instalment"] = "";
        //drNewSchedule["From_Date"] = "";
        //drNewSchedule["To_Date"] = "";
        //drNewSchedule["Account_Repay_ID"] = "";
        //dtNewSchedule.Rows.Add(drNewSchedule);

        DataTable dt = FunPriGroupRepayDetails(DtRepaySchedule, DtRepayStructure, 46, "Tax", true, true, true);
        DtRepaySchedule.Merge(dt);
    }

    //Chaage this function as public Method .TO call in EMI Calculor page
    public DataTable FunPriGroupRepayDetails(DataTable DtRepaySchedule, DataTable dtRepaymentDetails, int intCashFlow, string strCashflowDesc, bool blnAccountingIRR, bool blnBusinessIRR, bool blnCompanyIRR)
    {
        int counter = 1;
        int iCounter = 1;
        decimal iCurAmt = 0;
        int iToPeriod = 0;
        decimal iPrvAmt = 0;
        int iFromPeriod = 0;
        DataTable DtNewRepaySchedule = DtRepaySchedule.Clone();

        DataRow drRepayRow;

        int j = 1;
        foreach (DataRow grvReapyRow in dtRepaymentDetails.Rows)
        {
            if (iCounter == counter)
            {
                int i = 1;
                foreach (DataRow grvNewReapyRow in dtRepaymentDetails.Rows)
                {

                    if (iCounter == 1)
                    {
                        //if(!Int64.TryParse(grvNewReapyRow["InstallmentAmount"].ToString(),out iCurAmt))
                        iCurAmt = Convert.ToDecimal(grvNewReapyRow["TAX"].ToString());
                        iPrvAmt = iCurAmt;
                        iFromPeriod = 1;
                        iToPeriod = 1;
                    }
                    else
                    {
                        if (iCounter == i)
                        {
                            //if (!Int64.TryParse(grvNewReapyRow["InstallmentAmount"].ToString(), out iCurAmt))
                            iCurAmt = Convert.ToDecimal(grvNewReapyRow["TAX"].ToString());
                            if (iCurAmt != iPrvAmt)
                            {
                                goto L1;
                            }
                            else
                            {
                                iToPeriod = iToPeriod + 1;
                            }
                        }
                        else
                        {
                            goto L2;
                        }
                    }
                    iCounter = iCounter + 1;
                L2: ++i;
                }
            L1: drRepayRow = DtNewRepaySchedule.NewRow();
                drRepayRow["Account_Repay_ID"] = j + DtRepaySchedule.Rows.Count;
                j++;
                //drRepayRow["Amount"] = dtRepaymentDetails.Rows[0]["TAX"].ToString();
                drRepayRow["Per_Instalment_Amount"] = iPrvAmt;
                drRepayRow["PANUM"] = dtRepaymentDetails.Rows[0]["PANUM"].ToString();
                drRepayRow["SANUM"] = dtRepaymentDetails.Rows[0]["SANUM"].ToString();
                drRepayRow["From_Instalment"] = iFromPeriod;
                drRepayRow["To_Instalment"] = iToPeriod;
                if (dtRepaymentDetails.Rows.Count > 1)
                {
                    if (counter < dtRepaymentDetails.Rows.Count)
                        drRepayRow["From_Date"] = dtRepaymentDetails.Rows[counter]["FromDate"].ToString();
                }
                else
                {
                    drRepayRow["From_Date"] = dtRepaymentDetails.Rows[0]["FromDate"].ToString();
                }
                drRepayRow["To_Date"] = dtRepaymentDetails.Rows[iCounter - 2]["ToDate"].ToString();
                drRepayRow["Repayment_CashFlow"] = 13;
                int intTotalInstall = iToPeriod - iFromPeriod + 1;

                //drRepayRow["Accounting_IRR"] = blnAccountingIRR;
                //drRepayRow["Business_IRR"] = blnBusinessIRR;
                //drRepayRow["Company_IRR"] = blnCompanyIRR;
                //drRepayRow["CashFlow_Flag_ID"] = 23;
                DtNewRepaySchedule.Rows.Add(drRepayRow);
                iPrvAmt = iCurAmt;
                iFromPeriod = iCounter;
                iToPeriod = iCounter - 1;

            }


            ++counter;
        }

        return DtNewRepaySchedule;
    }

    #region "XIRR"
    public class CashItemPMC
    {
        private DateTime Date;
        private Double Amount;

        public DateTime date
        {
            get { return Date; }
            set { Date = value; }
        }

        public Double amount
        {
            get { return Amount; }
            set { Amount = value; }
        }
    }

    public const double Tolerance = 0.001;
    //public const double Tolerance = 0.1;
    private const double Guess = 0.1;
    private const double DaysPerYear = 365.0;
    private delegate double fx(double x);
    double xirr;
    public string FunProGetEIRR(DataTable dt)
    {
        //Dictionary<string, string> objParameters1 = new Dictionary<string, string>();
        //objParameters1.Add("@PANUM", txtAccountList.Text.Trim());
        //DataSet dsDocDet1 = Utility.GetDataset("S3G_LA_GET_ACCCLOSURE_EIRR", objParameters1);
        //DataTable dt = new DataTable();
        //if (dsDocDet1.Tables.Count > 0)
        //{
        //    dt = dsDocDet1.Tables[0];
        //}
        //else
        //{
        //    dt = null;
        //}

        List<CashItemPMC> list = new List<CashItemPMC>();
        foreach (DataRow dr in dt.Rows)
        {
            CashItemPMC t = new CashItemPMC();
            t.amount = double.Parse(dr["TRANSACTION_AMOUNT"].ToString());
            DateTime dttime = Convert.ToDateTime(dr["VALUE_DATE"].ToString());
            t.date = new DateTime(dttime.Year, dttime.Month, dttime.Day);
            list.Add(t);
        }
 
        if (list.Count > 0)
        {
            xirr = Xirr(list);

            Double myInt = xirr;
            //Console.WriteLine(myInt.ToString("P",  System.Globalization.CultureInfo.InvariantCulture));
            string strCheckInfinityNan = myInt.ToString("P",  System.Globalization.CultureInfo.InvariantCulture);

            if (strCheckInfinityNan == "Infinity" || strCheckInfinityNan == "NaN")
            {
                return "0";
            }

            double strRemovePercentage = double.Parse(strCheckInfinityNan.Replace("%", ""));
            double intval1 = Math.Pow(strRemovePercentage / 100 + 1, 0.083333333333333333333333333333333) - 1;
            double strdoubleV = 12 * intval1;
            string strval = (strdoubleV * 100).ToString("0.0000"); // Percentage Convertion          
           return strval;
        }
        else
            return "0";
    }
    public static double Xirr(IList<CashItemPMC> cashItems)
    {
        var firstItem = cashItems[0];
        fx seed = x => 0.0;
        var fxTotal = cashItems.Aggregate(seed, (current, cashItem) => Compose(current, f(cashItem, firstItem)));
        var dfxTotal = cashItems.Aggregate(seed, (current, cashItem) => Compose(current, df(cashItem, firstItem)));

        var xirr = CalMethod(fxTotal, dfxTotal, Guess);
        return xirr;
    }
    private static fx f(CashItemPMC cashItem, CashItemPMC firstItem)
    {
        var totalDays = firstItem.date.Subtract(cashItem.date).TotalDays;
        return x => cashItem.amount * Math.Pow((1.0 + x), totalDays / DaysPerYear);
    }
    private static fx df(CashItemPMC cashItem, CashItemPMC firstItem)
    {
        var totalDays = firstItem.date.Subtract(cashItem.date).TotalDays;
        return x => (1.0 / DaysPerYear) * totalDays * cashItem.amount * Math.Pow((x + 1.0), ((totalDays / DaysPerYear) - 1.0));
    }
    private static double CalMethod(fx f, fx df, double guess)
    {
        var x0 = guess;
        var err = 1e+100;
        while (err > Tolerance)
        {
            var fx0 = f(x0);
            var dfx0 = df(x0);
            // check dfx == 0 !
            var x1 = (Math.Abs(dfx0 - 0) < double.Epsilon) ? x0 / 2 : x0 - fx0 / dfx0;
            err = Math.Abs(x1 - x0);
            x0 = x1;
        }
        return x0;
    }
    private static fx Compose(fx f1, fx f2)
    {
        return x => f1(x) + f2(x);
    }

#endregion


}
