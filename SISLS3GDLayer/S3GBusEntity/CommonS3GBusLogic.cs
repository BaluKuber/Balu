using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using Microsoft.VisualBasic;
using System.Globalization;
using Microsoft.Office;
using System.IO;
using System.Xml;
using System.Data.Common;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace S3GBusEntity
{
    public class CommonS3GBusLogic
    {
        #region Amortaization Calculation Methods
        /// <summary>
        /// This is to calculate the ESM
        /// </summary> 
        /// <param name="startdate"></param>
        /// <param name="FinancedAmt"></param>
        /// <param name="rate"></param>
        /// <param name="tenure"></param>
        /// <returns></returns>
        public static DataTable FunPubESMCalculation(DataTable dtRepay, DateTime startdate, decimal FinancedAmt, decimal rate, int Tenure, string TenureDesc, decimal FinCharge)
        {
            int NoOfInst = dtRepay.Rows.Count;
            //decimal interest = Math.Round(FunPubInterestAmount(TenureDesc, FinancedAmt, rate, Tenure), 0);
            decimal interest = Math.Round(FinCharge, 0);

            decimal totalReceivable = FinancedAmt + interest;

            // creating a return datatable.
            DataTable dtESM = FunGetSODESMCloneTable(dtRepay);

            // adding the first Row - of cash flow
            DataRow drFirstESM = dtESM.NewRow();
            drFirstESM["Date"] = startdate;
            drFirstESM["Cash Flow"] = (FinancedAmt * -1).ToString("0.00");
            drFirstESM["Installment"] = Convert.ToDecimal("0.00").ToString("0.00");
            drFirstESM["Balance Payable"] = totalReceivable.ToString("0.00");
            drFirstESM["Interest Portion"] = Convert.ToDecimal("0.00").ToString("0.00");
            drFirstESM["Principal Portion"] = Convert.ToDecimal("0.00").ToString("0.00");
            dtESM.Rows.InsertAt(drFirstESM, 0);

            for (int i_tenure = 1; i_tenure <= NoOfInst; i_tenure++)
            {
                drFirstESM = dtESM.Rows[i_tenure];

                decimal EMI = Convert.ToDecimal(drFirstESM["Installment"].ToString());

                drFirstESM["Cash Flow"] = EMI.ToString("0.00");
                drFirstESM["Balance Payable"] = (Convert.ToDecimal(dtESM.Rows[i_tenure - 1]["Balance Payable"]) - EMI).ToString("0.00");
                if (i_tenure == NoOfInst)
                {
                    if (Convert.ToDecimal(drFirstESM["Balance Payable"]) != 0)
                    {
                        drFirstESM["Installment"] = (EMI + Convert.ToDecimal(drFirstESM["Balance Payable"])).ToString("0.00");
                        drFirstESM["Balance Payable"] = Convert.ToDecimal("0").ToString("0.00");
                    }
                }
                drFirstESM["Interest Portion"] = (interest / NoOfInst).ToString("0.00");
                drFirstESM["Principal Portion"] = (Convert.ToDecimal(drFirstESM["Installment"]) - Convert.ToDecimal(drFirstESM["Interest Portion"])).ToString("0.00");

                dtESM.AcceptChanges();
            }

            return dtESM;

        }

        /// <summary>
        /// This is to calculate the SOD
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="FinancedAmt"></param>
        /// <param name="IRR"></param>
        /// <param name="rate"></param>
        /// <param name="tenure"></param>
        /// <returns></returns>
        public static DataTable FunPubSODCalculation(DataTable dtRepay, DateTime startdate, decimal FinancedAmt, decimal rate, int Tenure, string TenureDesc, decimal FinCharge)
        {

            //decimal interest = FinancedAmt * NoOfInst / 12 * rate / 100;
            int NoOfInst = dtRepay.Rows.Count;
            //decimal interest = Math.Round(FunPubInterestAmount(TenureDesc, FinancedAmt, rate, Tenure), 0);
            decimal interest = Math.Round(FinCharge, 0);
            decimal totalReceivable = FinancedAmt + interest;
            decimal totalBalancePayable = Convert.ToDecimal("0.00");

            // creating a return datatable.
            DataTable dtSOD = FunGetSODESMCloneTable(dtRepay);

            // adding the first Row - of cash flow
            DataRow drFirstSOD = dtSOD.NewRow();
            drFirstSOD["Date"] = startdate;
            drFirstSOD["Cash Flow"] = (FinancedAmt * -1).ToString("0.00");
            drFirstSOD["Installment"] = Convert.ToDecimal("0.00").ToString("0.00");
            drFirstSOD["Balance Payable"] = totalReceivable.ToString("0.00");
            drFirstSOD["Interest Portion"] = Convert.ToDecimal("0.00").ToString("0.00");
            drFirstSOD["Principal Portion"] = Convert.ToDecimal("0.00").ToString("0.00");
            dtSOD.Rows.InsertAt(drFirstSOD, 0);

            for (int i_tenure = 1; i_tenure <= NoOfInst; i_tenure++)
            {
                drFirstSOD = dtSOD.Rows[i_tenure];

                decimal EMI = Convert.ToDecimal(drFirstSOD["Installment"].ToString());
                drFirstSOD["Cash Flow"] = EMI.ToString("0.00");
                drFirstSOD["Balance Payable"] = (Convert.ToDecimal(dtSOD.Rows[i_tenure - 1]["Balance Payable"]) - EMI).ToString("0.00");
                if (i_tenure == NoOfInst)
                {
                    if (Convert.ToDecimal(drFirstSOD["Balance Payable"]) != 0)
                    {
                        drFirstSOD["Installment"] = (EMI + Convert.ToDecimal(drFirstSOD["Balance Payable"])).ToString("0.00");
                        drFirstSOD["Balance Payable"] = Convert.ToDecimal("0").ToString("0.00");
                    }
                }
                drFirstSOD["Interest Portion"] = Convert.ToDecimal("0.00").ToString("0.00");  // this is to calculate once the total Balance Payable done
                drFirstSOD["Principal Portion"] = Convert.ToDecimal("0.00").ToString("0.00"); // this is to calculate once the total Balance Payable done

                totalBalancePayable += Convert.ToDecimal(drFirstSOD["Balance Payable"]);

                dtSOD.AcceptChanges();
            }

            // to calculate "Interest Portion" and "Principal Portion"
            for (int i_tenure = 1; i_tenure <= NoOfInst; i_tenure++)
            {
                dtSOD.Rows[i_tenure]["Interest Portion"] = ((interest * Convert.ToDecimal(dtSOD.Rows[i_tenure]["Balance Payable"])) / totalBalancePayable).ToString("0.00");
                dtSOD.Rows[i_tenure]["Principal Portion"] = (Convert.ToDecimal(dtSOD.Rows[i_tenure]["Installment"]) - Convert.ToDecimal(dtSOD.Rows[i_tenure]["Interest Portion"])).ToString("0.00");
            }

            return dtSOD;

        }

        /// <summary>
        /// This is to get the 
        /// </summary>
        /// <returns></returns>
        private static DataTable FunGetSODESMCloneTable(DataTable dtRepay)
        {
            //DataColumn dcCashFlow = new DataColumn("Cash Flow", System.Type.GetType("System.Decimal"));
            DataColumn dcBalancePayable = new DataColumn("Balance Payable", System.Type.GetType("System.Decimal"));
            DataColumn dcInterestPortion = new DataColumn("Interest Portion", System.Type.GetType("System.Decimal"));
            DataColumn dcPrincipalPortion = new DataColumn("Principal Portion", System.Type.GetType("System.Decimal"));

            //dtRepay.Columns.Add(dcCashFlow);
            dtRepay.Columns.Add(dcBalancePayable);
            dtRepay.Columns.Add(dcInterestPortion);
            dtRepay.Columns.Add(dcPrincipalPortion);

            return dtRepay;
        }


        #endregion

        #region Interest Calculation

        /// <summary>
        /// Function To calculate Interest Amount
        /// </summary>
        /// <param name="strTenureType"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decRateofInt"></param>
        /// <param name="intTenure"></param>
        /// <returns></returns>
        public static decimal FunPubInterestAmount(string strTenureType, decimal decPrincipleAmount, decimal decRateofInt, int intTenure)
        {
            decimal decInterestAmount = 0;
            switch (strTenureType.ToLower())
            {
                case "months":
                    decInterestAmount = Math.Round(decPrincipleAmount * decRateofInt * intTenure / 1200, 4);
                    break;
                case "weeks":
                    decInterestAmount = Math.Round(decPrincipleAmount * decRateofInt * intTenure / 5200, 4);
                    break;
                case "days":
                    decInterestAmount = Math.Round(decPrincipleAmount * decRateofInt * intTenure / 36500, 4);
                    break;
            }
            return decInterestAmount;
        }

        #endregion

        #region Repyment Structure Methods

        ///<summary>
        /// This method with recovery pattern for patterns other than Structured fixed only
        /// </summary>
        ///<param name="strFrequency">Frequency to be given as value ref GetNextDate Function</param>
        ///<param name="intTenure">Tenure given in application</param>
        ///<param name="strTenureType">Tenure type as text months/weeks/days</param>
        ///<param name="decPrincipleAmount">Finance amount from document</param>
        ///<param name="decRateofInt">Rate o finterest in all cases</param>
        ///<param name="returnType">Return type will be based on ReturnType/RepaymentMode/LOB</param>
        ///<param name="decresidualValue">Will be there if we give residual value based on ROI Rule</param>
        ///<param name="dtimeStartDate">Start date opf the repayment structure(in case of FBD FBD Date)</param>
        ///<param name="dtimeDocDate">Document date</param>
        ///<param name="decRoundOff">Round off value taken from Global Parameter</param>
        ///<param name="blnIsRounded"></param>
        ///<param name="strTimeVal">Value of Time Vlaue i.e., advance/arrears/advancefbd/arrearsfbd</param>
        ///<returns> Expanded Repayment structure</returns>
        public DataTable FunPubCalculateRepaymentDetails(string strFrequency, int intTenure, string strTenureType, decimal decPrincipleAmount, decimal decRateofInt, RepaymentType returnType, decimal? decresidualValue, DateTime dtimeStartDate, DateTime dtimeDocDate, decimal decRoundOff, out bool blnIsRounded, string strTimeVal)
        {
            blnIsRounded = false;

            DataTable dtRepaymentDetails = new DataTable();
            dtRepaymentDetails.Columns.Add("NoofDays");
            dtRepaymentDetails.Columns.Add("InstallmentNo");
            dtRepaymentDetails.Columns.Add("FromDate");
            dtRepaymentDetails.Columns.Add("ToDate");
            dtRepaymentDetails.Columns.Add("InstallmentDate");
            dtRepaymentDetails.Columns.Add("CurrentBalance");
            dtRepaymentDetails.Columns.Add("InstallmentAmount");
            dtRepaymentDetails.Columns.Add("Amount");
            dtRepaymentDetails.Columns["InstallmentAmount"].DataType = typeof(decimal);
            dtRepaymentDetails.Columns["Amount"].DataType = typeof(decimal);
            dtRepaymentDetails.Columns["NoofDays"].DataType = typeof(int);
            dtRepaymentDetails.Columns["InstallmentDate"].DataType = typeof(DateTime);
            dtRepaymentDetails.Columns.Add("RecoveryYear"); // For Moratorium


            DataRow drRepayment;
            decimal decInterestAmount = 0;
            //Intrest amount 
            //Added by Thangam on 19-Jun-2012 tol solve rounding off issue.
            decInterestAmount = FunPubInterestAmount(strTenureType, decPrincipleAmount, decRateofInt, intTenure);

            decimal decTotalAmt = decPrincipleAmount;
            decimal decPerInstallmentAmt = 0;
            int intLoopCount = 0;

            DateTime dtActualStartDate = Convert.ToDateTime(dtimeStartDate);//StringToDate(dtRepayment.Rows[0]["FromDate"].ToString(), strDateFormat);
            DateTime dtStartDate = dtActualStartDate;
            DateTime dtEndDate = new DateTime();
            if (dtimeStartDate == dtimeDocDate)
            {
                if (strTimeVal == "advance")
                {
                    dtEndDate = dtStartDate;
                }
                else
                {
                    //For Normal Arrears case
                    dtEndDate = FunPubGetNextDate(strFrequency, dtStartDate, 1);
                    dtStartDate = dtEndDate;
                }
            }
            else
            {
                if (strTimeVal == "advance")
                {
                    dtEndDate = dtStartDate;
                }
                else if (strTimeVal == "arrears")
                {
                    //For Normal Arrears case
                    dtEndDate = FunPubGetNextDate(strFrequency, dtStartDate, 1);
                    dtStartDate = dtEndDate;
                }
                else if (strTimeVal == "advancefbd")
                {
                    dtStartDate = dtimeDocDate;
                    dtEndDate = dtimeDocDate;
                }
                else
                {
                    //For arrears FBD
                    dtEndDate = FunPubGetNextDate(strFrequency, dtStartDate, 1);
                    dtStartDate = dtEndDate;
                }
            }

            //Time interval found to ensure Repay structure not exccceding tenure
            DateTime dateInterval = new DateTime();

            switch (strTenureType.ToLower())
            {
                case "months":
                    dateInterval = dtimeDocDate.AddMonths(intTenure);
                    break;
                case "weeks":

                    int intAddweeks = Convert.ToInt32(intTenure) * 7;
                    dateInterval = dtimeDocDate.AddDays(intAddweeks);
                    break;
                case "days":
                    dateInterval = dtimeDocDate.AddDays(intTenure);
                    break;
            }


            int intLoop = 0;
            #region Loop for adding Repayment structure dates
        looplabel:
            drRepayment = dtRepaymentDetails.NewRow();
            drRepayment["Amount"] = Math.Round(decTotalAmt, 0);
            drRepayment["InstallmentAmount"] = Math.Round(decPerInstallmentAmt, 0);
            drRepayment["InstallmentNo"] = intLoop + 1;
            //for any case 1st line from date should be document date
            if (intLoop == 0)
            {
                drRepayment["FromDate"] = dtimeDocDate;
            }
            else
            {
                drRepayment["FromDate"] = dtEndDate;
            }
            switch (returnType)
            {
                case RepaymentType.FC:
                case RepaymentType.WC:
                case RepaymentType.TLE:
                    //dtEndDate = FunPubGetNextDate(strFrequency, dtStartDate, 1);
                    dtEndDate = dateInterval;
                    break;
                default:
                    dtEndDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoop);
                    break;
            }
            drRepayment["ToDate"] = dtEndDate;
            drRepayment["InstallmentDate"] = dtEndDate;
            //date differnce 
            drRepayment["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drRepayment["FromDate"]), Convert.ToDateTime(drRepayment["ToDate"]), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
            switch (returnType)
            {
                //for factoring,working capital,Term loan,Term loan Extn(For only product and Tem loan) only
                //one line of Repay structure.
                case RepaymentType.FC:
                case RepaymentType.WC:
                case RepaymentType.TLE:
                    if (dtEndDate <= dateInterval)
                    {
                        dtRepaymentDetails.Rows.Add(drRepayment);
                        intLoop = intLoop + 1;
                        if (dtEndDate == dateInterval)
                            goto exitloop;
                        else
                            goto looplabel;
                    }
                    break;
                default:
                    if (strTimeVal == "advance" || strTimeVal == "advancefbd")
                    {
                        //In case of advance repayment structure should exit one frequency before tenure
                        //end
                        if (dtEndDate <= FunPubGetNextDate(strFrequency, dateInterval, -1))
                        {
                            dtRepaymentDetails.Rows.Add(drRepayment);
                            if (strTimeVal == "advancefbd" && intLoop == 0)
                            {
                                dtStartDate = dtimeStartDate;
                            }
                            intLoop = intLoop + 1;
                            goto looplabel;
                        }
                    }
                    else
                    {
                        if (dtEndDate <= dateInterval)
                        {
                            dtRepaymentDetails.Rows.Add(drRepayment);
                            intLoop = intLoop + 1;
                            if (dtEndDate == dateInterval)
                                goto exitloop;
                            else
                                goto looplabel;
                        }
                    }
                    break;
            }
            #endregion

        exitloop:
            intLoopCount = dtRepaymentDetails.Rows.Count;//No of Installments
            #region Loop for arriving per installment amount
            if (intLoopCount > 0)
            {
                switch (returnType)
                {

                    case RepaymentType.FC:
                    case RepaymentType.WC:
                    case RepaymentType.TLE:
                        decTotalAmt = decInterestAmount + decTotalAmt;
                        decPerInstallmentAmt = decTotalAmt;
                        break;
                    case RepaymentType.EMI:
                        decTotalAmt = decInterestAmount + decTotalAmt;
                        decPerInstallmentAmt = decTotalAmt / intLoopCount;
                        break;
                    case RepaymentType.PMPT:
                        decPerInstallmentAmt = (decTotalAmt / 1000) *
                            FunPubFrequencyBasedRate(strFrequency, decRateofInt);
                        break;
                    case RepaymentType.PMPL:
                        decPerInstallmentAmt = (decTotalAmt / 100000) *
                            FunPubFrequencyBasedRate(strFrequency, decRateofInt);
                        break;
                    case RepaymentType.PMPM:
                        decPerInstallmentAmt = (decTotalAmt / 1000000) *
                            FunPubFrequencyBasedRate(strFrequency, decRateofInt);
                        break;
                }
                dtRepaymentDetails.Columns["InstallmentAmount"].DefaultValue = Math.Round(decPerInstallmentAmt, 0);
                decimal decPerInstallAmt = 0;
                if (decRoundOff != 0)
                {
                    decPerInstallAmt = Math.Round((decPerInstallmentAmt / decRoundOff), 0) * decRoundOff;
                }
                else
                {
                    decPerInstallAmt = decPerInstallmentAmt;
                }

                foreach (DataRow drRepaymentRow in dtRepaymentDetails.Rows)
                {
                    drRepaymentRow["InstallmentAmount"] = decPerInstallAmt;
                    drRepaymentRow["Amount"] = Math.Round(decTotalAmt, 0);
                }
            #endregion

                #region Check Round off impact and correct in 1st installment
                if (returnType != RepaymentType.PMPL && returnType != RepaymentType.PMPM && returnType != RepaymentType.PMPT)
                {
                    if (decRoundOff != 0)
                    {
                        decimal decActualAmount = (decimal)dtRepaymentDetails.Compute("Sum(InstallmentAmount)", "NoofDays >=0 ");
                        decimal decbalamt;
                        if (decActualAmount < decTotalAmt)
                        {
                            blnIsRounded = true;
                            decbalamt = Convert.ToInt64(decTotalAmt) - decActualAmount;
                            dtRepaymentDetails.Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dtRepaymentDetails.Rows[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
                        }
                        else if (decActualAmount > decTotalAmt)
                        {
                            blnIsRounded = true;
                            decbalamt = decActualAmount - Convert.ToInt64(decTotalAmt);
                            dtRepaymentDetails.Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dtRepaymentDetails.Rows[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
                        }
                    }
                }
                #endregion

                return dtRepaymentDetails;
            }
            else
            {
                throw new Exception("Cannot generate repayment structure");
            }

        }

        ///<summary>
        /// This method to convert the interest rate based on the Frequency
        ///</summary>
        ///<param name="strFrequency">Frequency to be given as value ref GetNextDate Function</param>
        public static decimal FunPubFrequencyBasedRate(string strFrequency, decimal DecRate)
        {
            Decimal DecCalculatedRate;
            switch (strFrequency.ToLower())
            {
                //Weekly
                case "2":
                    DecCalculatedRate = DecRate / 7;
                    break;
                //Fortnightly
                case "3":
                    DecCalculatedRate = DecRate / 2;
                    break;
                //Monthly
                case "4":
                    DecCalculatedRate = DecRate;
                    break;
                //bi monthly
                case "5":
                    DecCalculatedRate = DecRate * 2;
                    break;
                //quarterly
                case "6":
                    DecCalculatedRate = DecRate * 3;
                    break;
                // half yearly
                case "7":
                    DecCalculatedRate = DecRate * 6;
                    break;
                //annually
                case "8":
                    DecCalculatedRate = DecRate * 12;
                    break;
                //daily
                case "0":
                    DecCalculatedRate = DecRate / 30;
                    break;
                //daily
                case "1":
                    DecCalculatedRate = DecRate / 30;
                    break;
                default:
                    DecCalculatedRate = DecRate;
                    break;
            }
            return DecCalculatedRate;
        }


        ///<summary>
        /// This method with recovery pattern for Structure fixed pattern only
        ///</summary>
        ///<param name="strFrequency">Frequency to be given as value ref GetNextDate Function</param>
        ///<param name="intTenure">Tenure given in application</param>
        ///<param name="strTenureType">Tenure type as text months/weeks/days</param>
        ///<param name="decPrincipleAmount">Finance amount from document</param>
        ///<param name="decRateofInt">Rate o finterest in all cases</param>
        ///<param name="returnType">Return type will be based on ReturnType/RepaymentMode/LOB</param>
        ///<param name="decresidualValue">Will be there if we give residual value based on ROI Rule</param>
        ///<param name="dtimeStartDate">Start date opf the repayment structure(in case of FBD FBD Date)</param>
        ///<param name="dtimeDocDate">Document date</param>
        ///<param name="decRoundOff">Round off value taken from Global Parameter</param>
        ///<param name="blnIsRounded"></param>
        ///<param name="strTimeVal">Value of Time Vlaue i.e., advance/arrears/advancefbd/arrearsfbd</param>
        /// <param name="decRecoveryPattern">Array containing recovery pattern</param>
        ///<returns> Expanded Repayment structure</returns>
        public DataTable FunPubCalculateRepaymentDetails(string strFrequency, int intTenure, string strTenureType, decimal decPrincipleAmount, decimal decRateofInt, RepaymentType returnType, decimal? decresidualValue, DateTime dtimeStartDate, DateTime dtimeDocDate, decimal decRoundOff, out bool blnIsRounded, string strTimeVal, bool blnRecovery, params decimal[] decRecoveryPattern)
        {
            blnIsRounded = false;
            DataTable dtRepaymentDetails = new DataTable();
            dtRepaymentDetails.Columns.Add("NoofDays");
            dtRepaymentDetails.Columns.Add("InstallmentNo");
            dtRepaymentDetails.Columns.Add("FromDate");
            dtRepaymentDetails.Columns.Add("ToDate");
            dtRepaymentDetails.Columns.Add("InstallmentDate");
            dtRepaymentDetails.Columns.Add("CurrentBalance");
            dtRepaymentDetails.Columns.Add("InstallmentAmount");
            dtRepaymentDetails.Columns.Add("Amount");
            dtRepaymentDetails.Columns["InstallmentAmount"].DataType = typeof(decimal);
            dtRepaymentDetails.Columns["Amount"].DataType = typeof(decimal);
            dtRepaymentDetails.Columns["NoofDays"].DataType = typeof(int);
            dtRepaymentDetails.Columns["InstallmentDate"].DataType = typeof(DateTime);
            dtRepaymentDetails.Columns.Add("RecoveryYear");//For Moratorium

            DataRow drRepayment;
            decimal decInterestAmount = 0;

            //decInterestAmount = Math.Round(FunPubInterestAmount(strTenureType, decPrincipleAmount, decRateofInt, intTenure), 0);
            decInterestAmount = FunPubInterestAmount(strTenureType, decPrincipleAmount, decRateofInt, intTenure);
            decimal decTotalAmt = decPrincipleAmount;
            decimal decPerInstallmentAmt = 0;
            int intLoopCount = 0;

            DateTime dtActualStartDate = Convert.ToDateTime(dtimeStartDate);//StringToDate(dtRepayment.Rows[0]["FromDate"].ToString(), strDateFormat);
            DateTime dtStartDate = dtActualStartDate;
            DateTime dtEndDate = new DateTime();
            if (strTimeVal == "advance")
            {
                dtEndDate = dtStartDate;
            }
            else
            {
                if (strTimeVal == "advancefbd")
                {
                    dtStartDate = dtimeDocDate;
                    dtEndDate = dtimeDocDate;
                }
                else
                {
                    dtEndDate = FunPubGetNextDate(strFrequency, dtStartDate, 1);
                    //dtStartDate = dtEndDate;
                }

            }

            DateTime dateInterval = new DateTime();

            switch (strTenureType.ToLower())
            {
                case "months":
                    dateInterval = dtimeDocDate.AddMonths(intTenure);
                    break;
                case "weeks":

                    int intAddweeks = Convert.ToInt32(intTenure) * 7;
                    dateInterval = dtimeDocDate.AddDays(intAddweeks);
                    break;
                case "days":
                    dateInterval = dtimeDocDate.AddDays(intTenure);
                    break;
            }


            int intLoop = 0;

        looplabel:
            drRepayment = dtRepaymentDetails.NewRow();
            drRepayment["Amount"] = Math.Round(decTotalAmt, 0);
            drRepayment["InstallmentAmount"] = Math.Round(decPerInstallmentAmt, 0);
            drRepayment["InstallmentNo"] = intLoop + 1;
            drRepayment["FromDate"] = dtEndDate;
            dtEndDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoop);
            drRepayment["ToDate"] = dtEndDate;
            drRepayment["InstallmentDate"] = dtEndDate;
            drRepayment["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drRepayment["FromDate"]), Convert.ToDateTime(drRepayment["ToDate"]), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

            if (strTimeVal == "advance" || strTimeVal == "advancefbd")
            {

                if (dtEndDate <= FunPubGetNextDate(strFrequency, dateInterval, -1))
                {
                    dtRepaymentDetails.Rows.Add(drRepayment);
                    if (strTimeVal == "advancefbd" && intLoop == 0)
                    {
                        dtStartDate = dtimeStartDate;
                    }
                    intLoop = intLoop + 1;
                    goto looplabel;
                }
            }
            else
            {
                if (dtEndDate <= dateInterval)
                {
                    dtRepaymentDetails.Rows.Add(drRepayment);
                    intLoop = intLoop + 1;
                    if (dtEndDate == dateInterval)
                        goto exitloop;
                    else
                        goto looplabel;
                }
            }
        exitloop:
            DateTime datStructDate;

            #region Finding Installment amount based on Recovery years
            datStructDate = Convert.ToDateTime(dtRepaymentDetails.Rows[0]["FromDate"].ToString());

            /* Calculate Total Repay amount for PMPT/PMPL/PMPM 
             * (**For this Repayment type Tenure should be Month)  */
            switch (returnType)
            {
                case RepaymentType.PMPT:
                    decTotalAmt = (decTotalAmt / 1000) * decRateofInt * intTenure;
                    break;
                case RepaymentType.PMPL:
                    decTotalAmt = (decTotalAmt / 100000) * decRateofInt * intTenure;
                    break;
                case RepaymentType.PMPM:
                    decTotalAmt = (decTotalAmt / 1000000) * decRateofInt * intTenure;
                    break;
                default:
                    decTotalAmt = decTotalAmt + decInterestAmount;
                    break;
            }

            for (int intRecoveryLoopCount = 0; intRecoveryLoopCount < decRecoveryPattern.Length; intRecoveryLoopCount++)
            {
                //if (Convert.ToDecimal(decRecoveryPattern[intRecoveryLoopCount].ToString()) != 0)
                //{
                decimal InstallAmount = 0;
                //if (!blnRecovery) // OL & FL with Repayment Mode Structured
                //{
                if (Convert.ToDecimal(decRecoveryPattern[intRecoveryLoopCount]) != 0)
                {
                    InstallAmount = (decTotalAmt * decRecoveryPattern[intRecoveryLoopCount]) / 100;
                }
                else
                {
                    InstallAmount = 0;
                }
                //}
                //else
                //{
                //    switch (returnType)
                //    {
                //        case RepaymentType.PMPT:
                //            InstallAmount = (decTotalAmt / 100) * decRecoveryPattern[intRecoveryLoopCount];
                //            break;
                //        case RepaymentType.PMPL:
                //            InstallAmount = (decTotalAmt / 100) * decRecoveryPattern[intRecoveryLoopCount];
                //            break;
                //        case RepaymentType.PMPM:
                //            InstallAmount = (decTotalAmt / 100) * decRecoveryPattern[intRecoveryLoopCount];
                //            break;
                //    }
                //}

                if (decRoundOff != 0)
                {
                    DataRow[] drRepaymentRow;

                    if (intRecoveryLoopCount == decRecoveryPattern.Length - 1)
                        drRepaymentRow = dtRepaymentDetails.Select("InstallmentDate >= #" + datStructDate + "#");
                    else
                        drRepaymentRow = dtRepaymentDetails.Select("InstallmentDate >= #" + datStructDate + "# and  InstallmentDate <#" + datStructDate.AddYears(1) + "#");

                    decimal decActualAmount = 0;
                    if (drRepaymentRow.Length > 0)
                    {
                        intLoopCount = drRepaymentRow.Length;
                        //if (!blnRecovery)
                        decPerInstallmentAmt = (InstallAmount / intLoopCount);
                        //else
                        //    decPerInstallmentAmt = InstallAmount;

                        for (int i = 0; i < drRepaymentRow.Length; i++)
                        {
                            drRepaymentRow[i]["RecoveryYear"] = intRecoveryLoopCount.ToString();
                            drRepaymentRow[i]["InstallmentAmount"] = Math.Round((decPerInstallmentAmt / decRoundOff), 0) * decRoundOff;
                            decActualAmount += Math.Round((decPerInstallmentAmt / decRoundOff), 0) * decRoundOff;
                            drRepaymentRow[i]["Amount"] = InstallAmount;
                            drRepaymentRow[i].AcceptChanges();
                        }

                    }
                    if (!blnRecovery) // OL & FL with Repayment Mode Structured
                    {
                        decimal decbalamt;
                        if (decActualAmount < InstallAmount)
                        {
                            blnIsRounded = true;
                            decbalamt = Convert.ToInt64(InstallAmount) - decActualAmount;
                            drRepaymentRow[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(drRepaymentRow[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
                            drRepaymentRow[0].AcceptChanges();
                        }
                        else if (decActualAmount > InstallAmount)
                        {
                            blnIsRounded = true;
                            decbalamt = decActualAmount - Convert.ToInt64(InstallAmount);
                            drRepaymentRow[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(drRepaymentRow[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
                            drRepaymentRow[0].AcceptChanges();
                        }
                    }
                }
                //}
                datStructDate = datStructDate.AddYears(1);
            }
            #endregion

            return dtRepaymentDetails;
            //else
            //{
            //    throw new Exception("Cannot generate repayment structure");
            //}

        }
        /// <summary>
        /// Gets repayment structure based on frequency tenure and tenure type
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="intTenure"></param>
        /// <param name="strTenureType"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decRateofInt"></param>
        /// <param name="returnType"></param>
        /// <param name="decresidualValue"></param>
        /// <param name="dtimeStartDate"></param>
        /// <param name="intCashFlow"></param>
        /// <param name="strCashflowDesc"></param>
        /// <param name="strTimeVal"></param>
        /// <returns></returns>
        public DataSet FunPubGetRepaymentDetails(string strFrequency, int intTenure, string strTenureType, decimal decPrincipleAmount, decimal decRateofInt, RepaymentType returnType, decimal? decresidualValue, DateTime dtimeStartDate, DateTime dtimeDocDate, int intCashFlow, string strCashflowDesc, decimal decRoundOff, bool blnAccountingIRR, bool blnBusinessIRR, bool blnCompanyIRR, string strTimeVal, DataTable dtMoratoriumInput)
        {
            DataSet dsRepaymentDetails = new DataSet("Repayment");
            bool blnIsRounded;

            DataTable dtRepaymentStructure;
            DataTable dtIRRMoratorium = new DataTable("dtIRRMoratorium");
            DataTable dtRepayMoratorium = new DataTable("dtRepayMoratorium");
            DataTable dtRepayMoratoriumStructure = new DataTable("dtRepayMoratoriumStructure");
            //Method that gives expanded repayment structure
            dtRepaymentStructure = FunPubCalculateRepaymentDetails(strFrequency, intTenure, strTenureType, decPrincipleAmount, decRateofInt, returnType, decresidualValue, dtimeStartDate, dtimeDocDate, decRoundOff, out blnIsRounded, strTimeVal);
            dtRepaymentStructure.TableName = "RepaymentStructure";

            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                dtRepayMoratoriumStructure = FunGetMoratoriumRepayStructure(dtRepaymentStructure, strFrequency, intTenure, strTenureType, decPrincipleAmount, decRateofInt, returnType, decresidualValue, dtimeStartDate, dtimeDocDate, decRoundOff, out blnIsRounded, strTimeVal, dtMoratoriumInput);
                dtIRRMoratorium = FunPriGroupRepayDetails(dtRepayMoratoriumStructure, intCashFlow, strCashflowDesc, blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, "");
            }

            DataTable dtRepaymentDetails = new DataTable("RepaymentDetails");
            //Method that gives grouped repayment structure based on installment amount
            dtRepaymentDetails = FunPriGroupRepayDetails(dtRepaymentStructure, intCashFlow, strCashflowDesc, blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, "");

            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                if (dtRepayMoratoriumStructure.Select("InstallmentAmount<>0").Length > 0)
                {
                    dtRepayMoratorium = dtRepayMoratoriumStructure.Select("InstallmentAmount<>0").CopyToDataTable();
                    DataView dvRepayMoratorium = dtRepayMoratorium.DefaultView;
                    dvRepayMoratorium.Sort = "SlNo";

                    dtRepayMoratorium = dvRepayMoratorium.ToTable();

                    int InstMentNo = 0;
                    foreach (DataRow drRepaymentRow in dtRepayMoratorium.Rows)
                    {
                        drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                        InstMentNo++;
                    }
                    dtRepayMoratorium = FunPriGroupRepayDetails(dtRepayMoratorium, intCashFlow, strCashflowDesc, blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, "MORATORIUM");


                }
            }

            dsRepaymentDetails.Tables.Add(dtRepaymentDetails);
            dsRepaymentDetails.Tables.Add(dtRepaymentStructure);
            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                dsRepaymentDetails.Tables.Add(dtIRRMoratorium);
                dsRepaymentDetails.Tables.Add(dtRepayMoratorium);
                //dsRepaymentDetails.Tables.Add(dtRepayMoratoriumStructure);
            }
            return dsRepaymentDetails;
        }



        public DataSet FunPubGetRepaymentDetails(string strFrequency, int intTenure, string strTenureType, decimal decPrincipleAmount, decimal decRateofInt, RepaymentType returnType, decimal? decresidualValue, DateTime dtimeStartDate, DateTime dtimeDocDate, int intCashFlow, string strCashflowDesc, decimal decRoundOff, bool blnAccountingIRR, bool blnBusinessIRR, bool blnCompanyIRR, string strTimeVal, bool blnRecovery, DataTable dtMoratoriumInput, params decimal[] decRecoveryPattern)
        {
            DataSet dsRepaymentDetails = new DataSet("Repayment");
            bool blnIsRounded;

            DataTable dtRepaymentStructure;

            DataTable dtIRRMoratorium = new DataTable("dtIRRMoratorium");
            DataTable dtRepayMoratorium = new DataTable("dtRepayMoratorium");
            DataTable dtRepayMoratoriumStructure = new DataTable("dtRepayMoratoriumStructure");

            //Method that gives expanded repayment structure
            dtRepaymentStructure = FunPubCalculateRepaymentDetails(strFrequency, intTenure, strTenureType, decPrincipleAmount, decRateofInt, returnType, decresidualValue, dtimeStartDate, dtimeDocDate, decRoundOff, out blnIsRounded, strTimeVal, blnRecovery, decRecoveryPattern);
            dtRepaymentStructure.TableName = "RepaymentStructure";

            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                dtRepayMoratoriumStructure = FunGetMoratoriumRepayStructure(dtRepaymentStructure, strFrequency, intTenure, strTenureType, decPrincipleAmount, decRateofInt, returnType, decresidualValue, dtimeStartDate, dtimeDocDate, decRoundOff, out blnIsRounded, strTimeVal, blnRecovery, dtMoratoriumInput, decRecoveryPattern);
                dtIRRMoratorium = FunPriGroupRepayDetails(dtRepayMoratoriumStructure, intCashFlow, strCashflowDesc, blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, "");
            }

            DataTable dtRepaymentDetails = new DataTable("RepaymentDetails");
            //Method that gives grouped repayment structure based on installment amount
            dtRepaymentDetails = FunPriGroupRepayDetails(dtRepaymentStructure, intCashFlow, strCashflowDesc, blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, "");

            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                if (dtRepayMoratoriumStructure.Select("InstallmentAmount<>0").Length > 0)
                {
                    dtRepayMoratorium = dtRepayMoratoriumStructure.Select("InstallmentAmount<>0").CopyToDataTable();
                    DataView dvRepayMoratorium = dtRepayMoratorium.DefaultView;
                    dvRepayMoratorium.Sort = "SlNo";

                    dtRepayMoratorium = dvRepayMoratorium.ToTable();

                    int InstMentNo = 0;
                    foreach (DataRow drRepaymentRow in dtRepayMoratorium.Rows)
                    {
                        drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                        InstMentNo++;
                    }
                    dtRepayMoratorium = FunPriGroupRepayDetails(dtRepayMoratorium, intCashFlow, strCashflowDesc, blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, "MORATORIUM");


                }
            }

            dsRepaymentDetails.Tables.Add(dtRepaymentDetails);
            dsRepaymentDetails.Tables.Add(dtRepaymentStructure);

            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                dsRepaymentDetails.Tables.Add(dtIRRMoratorium);
                dsRepaymentDetails.Tables.Add(dtRepayMoratorium);
            }

            return dsRepaymentDetails;
        }

        /// <summary>
        /// Get repayment structure from repayment Details
        /// </summary>
        /// <param name="dtRepaymentDetails"></param>
        /// <param name="intCashFlow"></param>
        /// <param name="strCashflowDesc"></param>
        /// <returns></returns>
        public DataTable FunPubGetRepaymentDetails(DataTable dtRepaymentDetails, int intCashFlow, string strCashflowDesc, bool blnAccountingIRR, bool blnBusinessIRR, bool blnCompanyIRR)
        {

            DataTable dtRepaymentStructure;
            dtRepaymentStructure = FunPriGroupRepayDetails(dtRepaymentDetails, intCashFlow, strCashflowDesc, blnAccountingIRR, blnBusinessIRR, blnCompanyIRR, "");
            return dtRepaymentStructure;
        }

        /// <summary>
        /// Grouping Repaymentdetails based on full repayment Structure
        /// </summary>
        /// <param name="dtRepaymentDetails"></param>
        /// <param name="intCashFlow"></param>
        /// <param name="strCashflowDesc"></param>
        /// <returns></returns>
        private DataTable FunPriGroupRepayDetails(DataTable dtRepaymentDetails, int intCashFlow, string strCashflowDesc, bool blnAccountingIRR, bool blnBusinessIRR, bool blnCompanyIRR, string strType)
        {
            DataTable dtRepaymentStructure = new DataTable();
            dtRepaymentStructure.Columns.Add("slno");
            dtRepaymentStructure.Columns.Add("CashFlow");
            dtRepaymentStructure.Columns.Add("Amount", typeof(decimal));
            dtRepaymentStructure.Columns.Add("PerInstall", typeof(decimal));
            dtRepaymentStructure.Columns.Add("Breakup", typeof(decimal));
            dtRepaymentStructure.Columns.Add("FromInstall");
            dtRepaymentStructure.Columns.Add("ToInstall");
            dtRepaymentStructure.Columns.Add("FromDate");
            dtRepaymentStructure.Columns.Add("ToDate");
            dtRepaymentStructure.Columns.Add("FlowDesc");
            dtRepaymentStructure.Columns.Add("CashFlowId");
            dtRepaymentStructure.Columns.Add("TotalPeriodInstall", typeof(decimal));
            dtRepaymentStructure.Columns.Add("Accounting_IRR");
            dtRepaymentStructure.Columns.Add("Business_IRR");
            dtRepaymentStructure.Columns.Add("Company_IRR");
            dtRepaymentStructure.Columns.Add("CashFlow_Flag_ID", typeof(Int16));
            //
            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
            dtRepaymentStructure.Columns.Add("Outflow");
            dtRepaymentStructure.Columns.Add("Amort");
            dtRepaymentStructure.Columns.Add("Entity");
            //
            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
            DataRow drRepayRow;

            if (strType == "")
            {
                int counter = 1;
                int iCounter = 1;
                decimal iCurAmt = 0;
                int iToPeriod = 0;
                decimal iPrvAmt = 0;
                int iFromPeriod = 0;

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
                                iCurAmt = Convert.ToDecimal(grvNewReapyRow["InstallmentAmount"].ToString());
                                iPrvAmt = iCurAmt;
                                iFromPeriod = 1;
                                iToPeriod = 1;
                            }
                            else
                            {
                                if (iCounter == i)
                                {
                                    //if (!Int64.TryParse(grvNewReapyRow["InstallmentAmount"].ToString(), out iCurAmt))
                                    iCurAmt = Convert.ToDecimal(grvNewReapyRow["InstallmentAmount"].ToString());
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

                    L1: drRepayRow = dtRepaymentStructure.NewRow();
                        drRepayRow["slno"] = "1";
                        drRepayRow["Amount"] = dtRepaymentDetails.Rows[0]["Amount"].ToString();
                        if (GPSRoundOff == 0)
                        {
                            drRepayRow["PerInstall"] = Math.Round(iPrvAmt, 2);
                        }
                        else
                        {
                            drRepayRow["PerInstall"] = iPrvAmt;
                        }
                        drRepayRow["Breakup"] = 0;
                        drRepayRow["FromInstall"] = iFromPeriod;
                        drRepayRow["ToInstall"] = iToPeriod;
                        if (dtRepaymentDetails.Rows.Count > 1)
                        {
                            if (counter < dtRepaymentDetails.Rows.Count)
                            {
                                //if (dtRepaymentDetails.Rows[counter]["FromDate"].ToString() == dtRepaymentDetails.Rows[counter - 1]["ToDate"].ToString())
                                drRepayRow["FromDate"] = dtRepaymentDetails.Rows[counter]["FromDate"].ToString();
                                //else //for moratorium
                                //    drRepayRow["FromDate"] = dtRepaymentDetails.Rows[counter - 1]["FromDate"].ToString();
                            }
                            else if (counter == dtRepaymentDetails.Rows.Count)
                                drRepayRow["FromDate"] = dtRepaymentDetails.Rows[counter - 1]["FromDate"].ToString();
                        }
                        else
                        {
                            drRepayRow["FromDate"] = dtRepaymentDetails.Rows[0]["FromDate"].ToString();
                        }

                        drRepayRow["ToDate"] = dtRepaymentDetails.Rows[iCounter - 2]["ToDate"].ToString();
                        //drRepayRow["TotalPeriodInstall"] = dtRepaymentDetails.Rows[counter]["Amount"].ToString();
                        int intTotalInstall = iToPeriod - iFromPeriod + 1;
                        if (GPSRoundOff == 0)
                        {
                            drRepayRow["TotalPeriodInstall"] = Math.Round(iPrvAmt, 2) * intTotalInstall;
                        }
                        else
                        {
                            drRepayRow["TotalPeriodInstall"] = iPrvAmt * intTotalInstall;
                        }
                        drRepayRow["CashFlow"] = strCashflowDesc;
                        drRepayRow["CashFlowId"] = intCashFlow;
                        drRepayRow["Accounting_IRR"] = blnAccountingIRR;
                        drRepayRow["Business_IRR"] = blnBusinessIRR;
                        drRepayRow["Company_IRR"] = blnCompanyIRR;
                        drRepayRow["CashFlow_Flag_ID"] = 23;
                        dtRepaymentStructure.Rows.Add(drRepayRow);
                        iPrvAmt = iCurAmt;
                        iFromPeriod = iCounter;
                        iToPeriod = iCounter - 1;
                    }
                    ++counter;
                }

            }
            else
            {


                var query = (from row in dtRepaymentDetails.AsEnumerable()
                             group row by new
                             {
                                 InstallmentAmount = row.Field<decimal>("InstallmentAmount"),
                                 RecoveryYear = row.Field<string>("RecoveryYear")
                                 //Amount = row.Field<decimal>("Amount")

                             } into grp

                             select new
                             {
                                 //Key = grp.Key,
                                 PerInstall = grp.Key.InstallmentAmount,
                                 Amount = grp.Select(r => r.Field<decimal>("Amount")),
                                 Breakup = 0,
                                 FromInstall = grp.Min(r => Convert.ToInt32(r.Field<string>("InstallmentNo"))),
                                 ToInstall = grp.Max(r => Convert.ToInt32(r.Field<string>("InstallmentNo"))),
                                 TotalPeriodInstall = grp.Sum(r => Convert.ToInt32(r.Field<decimal>("InstallmentAmount"))),
                                 FromDate = grp.Select(r => r.Field<string>("FromDate")),
                                 ToDate = grp.Select(r => r.Field<string>("ToDate"))

                             }).ToList();



                int SlNo = 0;
                foreach (var x in query)
                {
                    SlNo++;
                    drRepayRow = dtRepaymentStructure.NewRow();
                    drRepayRow["slno"] = SlNo.ToString();
                    drRepayRow["Amount"] = x.Amount.ToArray()[x.Amount.ToArray().Length - 1].ToString();
                    if (GPSRoundOff == 0)
                    {
                        drRepayRow["PerInstall"] = Convert.ToDecimal(((decimal)x.PerInstall).ToString("0.00"));
                    }
                    else
                    {
                        drRepayRow["PerInstall"] = x.PerInstall;
                    }
                    drRepayRow["Breakup"] = Convert.ToDecimal(x.Breakup);
                    drRepayRow["FromInstall"] = x.FromInstall.ToString();
                    drRepayRow["ToInstall"] = x.ToInstall.ToString();
                    drRepayRow["FromDate"] = x.ToDate.ToArray()[0].ToString();
                    drRepayRow["ToDate"] = x.ToDate.ToArray()[x.ToDate.ToArray().Length - 1].ToString();
                    drRepayRow["TotalPeriodInstall"] = x.TotalPeriodInstall;
                    drRepayRow["CashFlow"] = strCashflowDesc;
                    drRepayRow["CashFlowId"] = intCashFlow;
                    drRepayRow["Accounting_IRR"] = blnAccountingIRR;
                    drRepayRow["Business_IRR"] = blnBusinessIRR;
                    drRepayRow["Company_IRR"] = blnCompanyIRR;
                    drRepayRow["CashFlow_Flag_ID"] = 23;
                    dtRepaymentStructure.Rows.Add(drRepayRow);
                }

            }

            return dtRepaymentStructure;
        }

        private decimal FunPubGetBalAmount(decimal decFinAmount, decimal decRevisedRate, int inttenure, string strTenuretype, int intNoofInstall)
        {
            //decimal decRevisedInt = Math.Round(FunPubInterestAmount(strTenuretype, decFinAmount, decRevisedRate, intNoofInstall), 0);
            decimal decRevisedInt = FunPubInterestAmount(strTenuretype, decFinAmount, decRevisedRate, intNoofInstall);
            return decRevisedInt / intNoofInstall;
        }

        /// <summary>
        /// This method will form XML for DataTable its is a extended method available for 
        /// all Datatable.
        /// </summary>
        /// <param name="DtXml"></param>
        /// <returns></returns>
        public string FunPubFormXml(DataTable DtXml, bool IsNeedUpperCase)
        {
            int intcolcount = 0;
            string strColValue = string.Empty;
            StringBuilder strbXml = new StringBuilder();
            strbXml.Append("<Root>");
            foreach (DataRow grvRow in DtXml.Rows)
            {
                intcolcount = 0;
                strbXml.Append(" <Details ");
                foreach (DataColumn dtCols in DtXml.Columns)
                {
                    strColValue = grvRow.ItemArray[intcolcount].ToString();
                    if (grvRow.ItemArray[intcolcount].ToString() != "" || dtCols.ColumnName != string.Empty)
                    {
                        if (IsNeedUpperCase)
                        {
                            strbXml.Append(dtCols.ColumnName.ToUpper().Replace(" ", "") + "='" + strColValue + "' ");
                        }
                        else
                        {
                            strbXml.Append(dtCols.ColumnName.ToLower().Replace(" ", "") + "='" + strColValue + "' ");
                        }

                    }

                    intcolcount++;
                }
                strColValue = "";
                strbXml.Append(" /> ");
            }
            strbXml.Append("</Root>");
            return strbXml.ToString();
        }

        #endregion

        #region IRR Calculation Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtRepaymentSchedule"></param>
        /// <param name="dtCashInflow"></param>
        /// <param name="dtCashOutflow"></param>
        /// <param name="strFrequency"></param>
        /// <param name="intTenure"></param>
        /// <param name="strTenureType"></param>
        /// <param name="strDateFormat"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decRateofInt"></param>
        /// <param name="strIRRrest"></param>
        /// <param name="strTimeval"></param>
        /// <param name="strInstallmentType"></param>
        /// <param name="decLimit"></param>
        /// <param name="Irrtype"></param>
        /// <param name="decResultIRR"></param>
        /// <param name="decCTR"></param>
        /// <param name="decPLR"></param>
        /// <param name="dtAppdate"></param>
        /// <param name="decResidualValue"></param>
        /// <param name="decResidualAmount"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public DataTable FunPubGenerateRepaymentStructureForTL(DataTable dtRepaymentSchedule, DataTable dtCashInflow, DataTable dtCashOutflow, string strFrequency, int intTenure, string strTenureType, string strDateFormat, decimal decPrincipleAmount, decimal decRateofInt, string strIRRrest, string strTimeval, string strInstallmentType, decimal decLimit, decimal decCTR, decimal decPLR, DateTime dtAppdate, decimal? decResidualValue, decimal? decResidualAmount, RepaymentType returnType, out double decAccountingIRR, out double decBussinessIRR, out double decComapnyIRR, string strMorType, DataTable dtMoratoriumInput, string StrLob)
        {
            decAccountingIRR = 0;
            decBussinessIRR = 0;
            decComapnyIRR = 0;
            bool blnAccountingIRR = false, blnBusinessIRR = false, blnCompanyIRR = false;
            DataTable dtAccountingIRR = new DataTable();
            IRRType Irrtype = IRRType.Accounting_IRR;
            DataTable dtRepayStructure = new DataTable();
        Start:
            #region IRRTable declaration and colunm addition

            DataTable dtIRRDetails = new DataTable();

            dtIRRDetails.Columns.Add("NoofDays", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentNo", typeof(int));
            dtIRRDetails.Columns.Add("FromDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("ToDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("From_Date");
            dtIRRDetails.Columns.Add("To_Date");
            dtIRRDetails.Columns.Add("Installment_Date");
            dtIRRDetails.Columns.Add("InstallmentDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("Amount", typeof(decimal));
            dtIRRDetails.Columns.Add("TransactionType");
            dtIRRDetails.Columns.Add("PrevBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("CurrBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("Slno", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("Charge", typeof(decimal));
            dtIRRDetails.Columns.Add("PrincipalAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("IsMor");
            #endregion

            #region Only for Moratorium
            string MoraStDate = "";
            string MoraEndDate = "";
            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                MoraStDate = dtMoratoriumInput.Rows[0]["Fromdate"].ToString();// DateTime.Now.AddDays(61).ToString("MM/dd/yyyy");
                MoraEndDate = dtMoratoriumInput.Rows[0]["Todate"].ToString(); //Convert.ToDateTime(MoraStDate).AddDays(45).ToString("MM/dd/yyyy");
                strMorType = dtMoratoriumInput.Rows[0]["Moratoriumtype"].ToString().ToUpper();
            }
            #endregion
            //IRR Row
            DataRow drIRR;

            #region Adding First Row By default
            DateTime dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());//StringToDate(dtRepaymentSchedule.Rows[0]["FromDate"].ToString(), strDateFormat); ;
            DateTime dtEndDate = dtStartDate;

            #endregion

            #region Adding Cash Inflow Details
            foreach (DataRow drCashInflow in dtCashInflow.Rows)
            {
                ///CashFlow_Flag_ID 34 Hardcoded for Not to Include UMFC In IRR Calculation
                ///drCashInflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if ((drCashInflow[Irrtype.ToString()].ToString() == "True" || drCashInflow[Irrtype.ToString()].ToString() == "1")
                    && drCashInflow["CashFlow_Flag_ID"].ToString() != "34")
                {

                    drIRR = dtIRRDetails.NewRow();
                    drIRR["Amount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentAmount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentNo"] = "0";
                    drIRR["FromDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["InstallmentDate"] = drCashInflow["Date"].ToString();// StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["ToDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["NoofDays"] = "0";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            #endregion

            #region Adding Cash Outflow Details
            foreach (DataRow drCashOutflow in dtCashOutflow.Rows)
            {
                ///drCashOutflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drCashOutflow[Irrtype.ToString()].ToString() == "True" || drCashOutflow[Irrtype.ToString()].ToString() == "1")
                {
                    // considering Cash flow other than Margin amount (43 flag id for Margin payment)
                    if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "43")
                    {
                        //adding cashflow other than margin cashflows for Company IRR and Business IRR
                        if (Irrtype != IRRType.Accounting_IRR)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                        }
                        //In case of Accounting IRR adding all outflows at application date itself Changed on 09-Mar-2011
                        else if (Irrtype == IRRType.Accounting_IRR)
                        {
                            //adding cashflow other than Delear payment for accounting IRR  (41 flag id for Dealer payment)
                            //if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "41")
                            //{
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["InstallmentDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["ToDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                            //}
                        }
                    }

                }

            }
            #endregion

            /***********************For Accounting IRR add finance amount as ouflow happebd on the document date itself*****************/
            #region Adding Finance Amount as first row for Accounting IRR
            //if (Irrtype == IRRType.Accounting_IRR)
            //{
            //    drIRR = dtIRRDetails.NewRow();
            //    drIRR["Amount"] = 0 - decPrincipleAmount;
            //    drIRR["InstallmentAmount"] = 0 - decPrincipleAmount;
            //    drIRR["InstallmentNo"] = "0";
            //    drIRR["FromDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["InstallmentDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["ToDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["Charge"] = "0.00";
            //    drIRR["PrevBalance"] = "0.00";
            //    drIRR["CurrBalance"] = decPrincipleAmount;//Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
            //    drIRR["NoofDays"] = "0";
            //    dtIRRDetails.Rows.Add(drIRR);
            //}
            int intLoop = 0;
            int intNoofInstallment = 0;
            #endregion

            #region Loop for adding Repayment Structure
            DateTime dtFromDate = new DateTime();
            int intNoLocalInstallment = 0;
            //dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());
            //installments
            DataRow[] drRepay_Array = dtRepaymentSchedule.Select("CashFlow_Flag_ID in(91,35)", "slno asc");
            foreach (DataRow drRepay in drRepay_Array)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"]);
                    dtFromDate = dtStartDate;
                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);//Modify on 22/11/2011

                        if (intLoopCount == intTotalInstallment - 1 && drRepay_Array.Length - 1 == intLoop)
                        {
                            drIRR["ToDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                            drIRR["InstallmentDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                        }

                        switch (returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (strTenureType.ToLower())
                                {
                                    case "months":
                                        strFrequency = "4";
                                        break;
                                    case "weeks":
                                        strFrequency = "2";
                                        break;
                                    case "days":
                                        strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                                drIRR["ToDate"] = dtFromDate;
                                drIRR["InstallmentDate"] = dtFromDate;
                                break;
                            default:
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
                                break;
                        }
                        if (drRepay["Amount"].ToString().Trim() != "")
                            drIRR["Amount"] = drRepay["Amount"].ToString();
                        else
                            drIRR["Amount"] = "0";
                        drIRR["TransactionType"] = "Installment";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }

            //Insurance 
            DataRow[] drRepay_InsuranceArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID =25", "slno asc");
            foreach (DataRow drRepay in drRepay_InsuranceArray)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                    dtFromDate = dtStartDate;

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        switch (returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (strTenureType.ToLower())
                                {
                                    case "months":
                                        strFrequency = "4";
                                        break;
                                    case "weeks":
                                        strFrequency = "2";
                                        break;
                                    case "days":
                                        strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                break;
                            default:
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount);
                                break;
                        }

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                        drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                        drIRR["TransactionType"] = "Insurance";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            //Others
            DataRow[] drRepay_OthersArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID not in(91,35,25)", "slno asc");
            foreach (DataRow drRepay in drRepay_OthersArray)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                    dtFromDate = dtStartDate;

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        switch (returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (strTenureType.ToLower())
                                {
                                    case "months":
                                        strFrequency = "4";
                                        break;
                                    case "weeks":
                                        strFrequency = "2";
                                        break;
                                    case "days":
                                        strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                break;
                            default:
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount);
                                break;
                        }

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                        drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                        drIRR["TransactionType"] = "Others";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            #endregion

            #region NewCode
            foreach (DataRow drrpy in dtRepaymentSchedule.Rows)
            {
                int intFromInstallment = Convert.ToInt32(drrpy["FromInstall"].ToString());
                int intToInstallment = Convert.ToInt32(drrpy["ToInstall"].ToString());
                int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                dtStartDate = Convert.ToDateTime(drrpy["FromDate"]);
                dtFromDate = dtStartDate;
                intNoLocalInstallment = intNoofInstallment;
                for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                {
                    drIRR = dtIRRDetails.NewRow();
                    drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                    drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                    drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);//Modify on 22/11/2011

                    if (intLoopCount == intTotalInstallment - 1 && drRepay_Array.Length - 1 == intLoop)
                    {
                        drIRR["ToDate"] = Convert.ToDateTime(drrpy["ToDate"]);
                        drIRR["InstallmentDate"] = Convert.ToDateTime(drrpy["ToDate"]);
                    }

                    switch (returnType)
                    {
                        case RepaymentType.FC:
                        case RepaymentType.WC:
                        case RepaymentType.TLE:
                            switch (strTenureType.ToLower())
                            {
                                case "months":
                                    strFrequency = "4";
                                    break;
                                case "weeks":
                                    strFrequency = "2";
                                    break;
                                case "days":
                                    strFrequency = "0";
                                    break;
                            }
                            dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                            //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                            drIRR["ToDate"] = dtFromDate;
                            drIRR["InstallmentDate"] = dtFromDate;
                            break;
                        default:
                            dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
                            break;
                    }

                    if (drrpy["CashFlow_Flag_ID"].ToString() == "91")
                    {
                        drIRR["TransactionType"] = "Principal";
                        drIRR["PrincipalAmount"] = drrpy["PerInstall"].ToString();
                    }
                    else if (drrpy["CashFlow_Flag_ID"].ToString() == "35")
                    {
                        drIRR["TransactionType"] = "Interest";
                        drIRR["Charge"] = drrpy["PerInstall"].ToString();
                    }
                    drIRR["InstallmentAmount"] = "0";
                    intNoLocalInstallment = intNoLocalInstallment + 1;
                    drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = "0.00";
                    dtIRRDetails.Rows.Add(drIRR);
                }

                intNoofInstallment = intNoLocalInstallment;
                intLoop++;
            }
            int intEndDateCount = 0;
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Slno"] = intEndDateCount + 1;
                intEndDateCount++;
            }

            DataTable DtIRRNwTable = dtIRRDetails.Copy();
            for (int i = 1; i <= DtIRRNwTable.Rows.Count - 1; i++)
            {
                DtIRRNwTable.Rows.RemoveAt(i);
                i = i - 1;
            }

            DataTable Dtdist = dtIRRDetails.DefaultView.ToTable(true, "TransactionType");

            int PrinciCount = 0;
            int InterestCount = 0;
            int diffCount = 0;
            for (int disidx = 0; disidx < Dtdist.Rows.Count; disidx++)
            {
                DataRow[] drsel = dtIRRDetails.Select("TransactionType='" + Dtdist.Rows[disidx]["TransactionType"].ToString() + "'");

                if (Dtdist.Rows[disidx]["TransactionType"].ToString() == "Principal")
                {
                    PrinciCount = drsel.Length;
                    for (int selidx = 0; selidx < drsel.Length; selidx++)
                    {
                        DataRow drNewIRR = DtIRRNwTable.NewRow();
                        drNewIRR["FromDate"] = drsel[selidx]["FromDate"];
                        drNewIRR["ToDate"] = drsel[selidx]["ToDate"];
                        drNewIRR["InstallmentDate"] = drsel[selidx]["InstallmentDate"];
                        drNewIRR["ToDate"] = drsel[selidx]["ToDate"];
                        drNewIRR["InstallmentDate"] = drsel[selidx]["InstallmentDate"];
                        drNewIRR["TransactionType"] = "Installment";
                        drNewIRR["PrincipalAmount"] = drsel[selidx]["PrincipalAmount"];
                        //drNewIRR["InstallmentAmount"] = drsel[selidx]["InstallmentAmount"];
                        drNewIRR["InstallmentNo"] = (DtIRRNwTable.Rows.Count).ToString(); //drsel[selidx]["InstallmentNo"];
                        drNewIRR["PrevBalance"] = drsel[selidx]["PrevBalance"];
                        drNewIRR["CurrBalance"] = drsel[selidx]["CurrBalance"];
                        drNewIRR["charge"] = drsel[selidx]["charge"].ToString() == string.Empty ? 0 : drsel[selidx]["charge"];
                        drNewIRR["InstallmentAmount"] = Convert.ToDecimal(drNewIRR["charge"].ToString()) + Convert.ToDecimal(drNewIRR["PrincipalAmount"].ToString());
                        DtIRRNwTable.Rows.Add(drNewIRR);
                    }
                }
                else if (Dtdist.Rows[disidx]["TransactionType"].ToString() == "Interest")
                {
                    for (int selidx = 0; selidx < drsel.Length; selidx++)
                    {
                        DataRow[] dr = DtIRRNwTable.Select("InstallmentDate='" + drsel[selidx]["InstallmentDate"].ToString() + "' and TransactionType = 'Installment'");
                        if (dr.Length == 0)
                        {
                            DataRow drNewIRR = DtIRRNwTable.NewRow();
                            drNewIRR["FromDate"] = drsel[selidx]["FromDate"];
                            drNewIRR["ToDate"] = drsel[selidx]["ToDate"];
                            drNewIRR["InstallmentDate"] = drsel[selidx]["InstallmentDate"];
                            drNewIRR["ToDate"] = drsel[selidx]["ToDate"];
                            drNewIRR["InstallmentDate"] = drsel[selidx]["InstallmentDate"];
                            drNewIRR["TransactionType"] = "Installment";
                            drNewIRR["PrincipalAmount"] = drsel[selidx]["PrincipalAmount"].ToString() == string.Empty ? 0 : drsel[selidx]["PrincipalAmount"];
                            drNewIRR["Charge"] = drsel[selidx]["Charge"];
                            drNewIRR["InstallmentAmount"] = drsel[selidx]["InstallmentAmount"];
                            drNewIRR["InstallmentNo"] = (DtIRRNwTable.Rows.Count).ToString(); //drsel[selidx]["InstallmentNo"];
                            drNewIRR["PrevBalance"] = drsel[selidx]["PrevBalance"];
                            drNewIRR["CurrBalance"] = drsel[selidx]["CurrBalance"];
                            drNewIRR["InstallmentAmount"] = Convert.ToDecimal(drNewIRR["charge"].ToString()) + Convert.ToDecimal(drNewIRR["PrincipalAmount"].ToString());
                            DtIRRNwTable.Rows.Add(drNewIRR);
                        }
                        else
                        {
                            DataRow drInst = DtIRRNwTable.AsEnumerable().Where(r => r["InstallmentDate"].Equals(drsel[selidx]["InstallmentDate"]) && r["TransactionType"].Equals("Installment")).First();
                            drInst["charge"] = drsel[selidx]["charge"];
                            drInst["InstallmentAmount"] = (Convert.ToDecimal(drInst["PrincipalAmount"].ToString()) + Convert.ToDecimal(drInst["charge"].ToString())).ToString();
                            //DtIRRNwTable.Rows[selidx]["charge"] = drsel[selidx]["charge"].ToString();
                            //DtIRRNwTable.Rows[selidx]["InstallmentAmount"] = (Convert.ToDecimal(DtIRRNwTable.Rows[selidx]["PrincipalAmount"].ToString()) + Convert.ToDecimal(drsel[intidx]["charge"].ToString())).ToString();
                        }
                    }

                }
                else if (Dtdist.Rows[disidx]["TransactionType"].ToString() == "Insurance")
                {
                    for (int selidx = 0; selidx < drsel.Length; selidx++)
                    {
                        DataRow drNewIRR = DtIRRNwTable.NewRow();
                        drNewIRR["FromDate"] = drsel[selidx]["FromDate"];
                        drNewIRR["ToDate"] = drsel[selidx]["ToDate"];
                        drNewIRR["InstallmentDate"] = drsel[selidx]["InstallmentDate"];
                        drNewIRR["ToDate"] = drsel[selidx]["ToDate"];
                        drNewIRR["InstallmentDate"] = drsel[selidx]["InstallmentDate"];
                        drNewIRR["TransactionType"] = "Insurance";
                        drNewIRR["PrincipalAmount"] = "0";// drsel[selidx]["PrincipalAmount"];
                        drNewIRR["InstallmentAmount"] = drsel[selidx]["InstallmentAmount"];
                        drNewIRR["InstallmentNo"] = (DtIRRNwTable.Rows.Count).ToString(); //drsel[selidx]["InstallmentNo"];
                        drNewIRR["PrevBalance"] = "0"; //drsel[selidx]["PrevBalance"];
                        drNewIRR["CurrBalance"] = "0"; // drsel[selidx]["CurrBalance"];
                        drNewIRR["charge"] = "0"; // drsel[selidx]["charge"].ToString() == string.Empty ? 0 : drsel[selidx]["charge"];
                        //drNewIRR["InstallmentAmount"] = Convert.ToDecimal(drNewIRR["charge"].ToString()) + Convert.ToDecimal(drNewIRR["PrincipalAmount"].ToString());
                        DtIRRNwTable.Rows.Add(drNewIRR);
                    }
                }
            }
            DtIRRNwTable.AcceptChanges();

            #region For Deffered Payments
            DataView dvIrrDetails = DtIRRNwTable.DefaultView;
            dvIrrDetails.Sort = "ToDate,InstallmentNo,InstallmentAmount ASC";

            DtIRRNwTable = dvIrrDetails.ToTable();

            bool blnBusIRRdone = false;
            intEndDateCount = 0;
            DataTable dtNwRpymentSchedule = dtRepaymentSchedule.Clone();
            DataRow[] drNewRpySchedule = dtRepaymentSchedule.Select();
            foreach (DataRow dr in drNewRpySchedule)
            {
                dtNwRpymentSchedule.ImportRow(dr);
            }

            foreach (DataRow dr in dtNwRpymentSchedule.Rows)
            {
                dr["CashFlow_Flag_ID"] = "23";
            }
            DataTable dtNwRepaymentSchedule = dtRepaymentSchedule.Clone();
            DataTable dtGroup = getGroupBY(dtNwRpymentSchedule, "CashFlow_Flag_ID,PerInstall", "CashFlow_Flag_ID", "sum");
            DataRow drNewRpy = dtNwRpymentSchedule.Select().First();
            dtNwRepaymentSchedule.ImportRow(drNewRpy);
            dtNwRepaymentSchedule.Rows[0]["Amount"] = decPrincipleAmount;
            dtNwRepaymentSchedule.Rows[0]["CashFlow"] = "Installment";
            dtNwRepaymentSchedule.Rows[0]["PerInstall"] = dtGroup.Rows[0]["PerInstall"];
            dtNwRepaymentSchedule.Rows[0]["CashFlowID"] = "-1";
            dtNwRepaymentSchedule.Rows[0]["Business_IRR"] = "True";
            dtNwRepaymentSchedule.Rows[0]["Company_IRR"] = "True";
            dtNwRepaymentSchedule.Rows[0]["Accounting_IRR"] = "True";
            dtNwRepaymentSchedule.Rows[0]["CashFlow_Flag_ID"] = dtGroup.Rows[0]["CashFlow_Flag_ID"];
            dtNwRepaymentSchedule.Rows[0]["TotalPeriodInstall"] = (Convert.ToDecimal(dtGroup.Rows[0]["PerInstall"]) * Convert.ToDecimal(dtNwRepaymentSchedule.Rows[0]["ToInstall"]));
            dtNwRepaymentSchedule.AcceptChanges();

            #endregion
            foreach (DataRow drIrrRows in DtIRRNwTable.Rows)
            {
                //drIrrRows["FromDate"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);

                //if (drIrrRows["TransactionType"].ToString() == "Installment")
                //{
                if (intEndDateCount != 0)
                    drIrrRows["FromDate"] = DtIRRNwTable.Rows[intEndDateCount - 1]["ToDate"];
                else
                    drIrrRows["FromDate"] = dtAppdate;
                //}

                #region For Moratorium
                bool blnMor = false;
                if (strMorType != "")// == "INTEREST")
                {
                    if (Convert.ToDateTime(drIrrRows["FromDate"].ToString()) > Convert.ToDateTime(MoraStDate) && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) < Convert.ToDateTime(MoraEndDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraEndDate) && blnMor == false)
                    {
                        drIrrRows["FromDate"] = Convert.ToDateTime(dtMoratoriumInput.Rows[0]["ToDate"].ToString());
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        blnMor = true;
                    }
                    else if (Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                       && Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) < Convert.ToDateTime(MoraEndDate) &&
                        blnMor == false)
                    {
                        //drIrrRows["NoofDays"] = 0;
                        drIrrRows["IsMor"] = "0";
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                    else if (blnMor == false && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) <= Convert.ToDateTime(MoraStDate) &&
                        Convert.ToDateTime(drIrrRows["ToDate"].ToString()) >= Convert.ToDateTime(MoraEndDate))
                    //dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").Length > 0)
                    {
                        blnMor = true;
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = Convert.ToInt32(drIrrRows["NoofDays"].ToString()) - DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(MoraStDate), Convert.ToDateTime(MoraEndDate), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

                    }
                    else
                    {
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                }
                #endregion
                else
                {
                    drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    /*For sakthi finanace
                   //Code Comment and added by saran on 17-feb-2012 start
                   // drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    switch (strIRRrest)
                    {
                        case "monthly":
                            drIrrRows["NoofDays"] = 30;
                            break;
                        case "daily":
                            drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            break;
                    }
                    //Code Comment and added by saran on 17-feb-2012 end*/
                }
                intEndDateCount++;
            }

            #endregion

            decimal decAmount = (decimal)DtIRRNwTable.Compute("Sum(InstallmentAmount)", "NoofDays >=0 ");
            foreach (DataRow drIrrRows in DtIRRNwTable.Rows)
            {
                drIrrRows["Amount"] = decAmount;
            }


            DtIRRNwTable.Rows[0]["Amount"] = -decPrincipleAmount;
            DtIRRNwTable.Rows[0]["PrevBalance"] = decPrincipleAmount;
            DtIRRNwTable.Rows[0]["InstallmentAmount"] = -decPrincipleAmount;

            double intMinVal = 0.00;
            double intMaxVal = 1000.00;

            #region Min val Max val Assiging part
        loopforIRR:
            if (intMaxVal == intMinVal)
            {
                throw new Exception("Incorrect cashflow details,cannot calculate IRR");
            }
            double decIRR = (intMaxVal + intMinVal) / 2;
            //decIRR = Math.Round(decIRR, 4);
            int intnoofdays = 0;
            switch (strIRRrest)
            {
                case "monthly":
                    intnoofdays = 1200;
                    break;
                case "daily":
                    intnoofdays = 36500;
                    break;
                case "yearly":
                    intnoofdays = 100;
                    break;
            }
            double decExp = 1 + (decIRR / intnoofdays);
            int intdtRowcount = 0;
            decimal dbPrevBalance = 0;
            decimal dbCharge = 0;
            //Double dbCurrPrevBalance;
            #endregion

            #region IRR Calculation Loop
            foreach (DataRow drIRRRow in DtIRRNwTable.Rows)
            {
                if (intdtRowcount != 0)
                {
                    dbPrevBalance = Convert.ToDecimal(DtIRRNwTable.Rows[intdtRowcount - 1]["CurrBalance"].ToString());
                }
                switch (strIRRrest)
                {
                    case "monthly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"]) / 30))) - dbPrevBalance;
                        break;
                    case "daily":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"])))) - dbPrevBalance;
                        break;
                    case "yearly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                        break;
                }
                // drIRRRow["PrevBalance"] = Math.Round(dbPrevBalance, 2) + Math.Round(dbCharge, 2);

                #region For Moratorium
                if (strMorType.ToUpper() == "INTEREST")
                {
                    if (drIRRRow["IsMor"].ToString() != "")
                    {
                        //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                        switch (strIRRrest)
                        {
                            case "monthly":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                break;
                            case "daily":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                break;
                            case "yearly":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                                break;
                        }

                    }


                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    //drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);// -Convert.ToDecimal(drIRRRow["InstallmentAmount"]);

                }
                #endregion
                else
                {
                    #region For Moratorium
                    if (strMorType.ToUpper() == "BOTH")
                    {
                        //if (drIRRRow["InstallmentAmount"].ToString() == "0")
                        //    dbCharge = 0;
                        if (drIRRRow["IsMor"].ToString() != "")
                        {
                            //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                            //drIRRRow["InstallmentAmount"] = 0;

                            switch (strIRRrest)
                            {
                                case "monthly":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                    break;
                                case "daily":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                    break;
                                case "yearly":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                                    break;
                            }
                        }
                    }
                    //else
                    //{
                    //    if (drIRRRow["IsMor"].ToString() != "")
                    //    {
                    //        drIRRRow["InstallmentAmount"] = 0;
                    //    }
                    //}
                    #endregion

                    //drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    //drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);// -Convert.ToDecimal(drIRRRow["InstallmentAmount"]);
                }
                intdtRowcount++;
            }

            DtIRRNwTable.AcceptChanges();

            #endregion

            //*******Checking Whether to Continue the Loop or not********************// 
            if (Convert.ToDecimal(DtIRRNwTable.Rows[DtIRRNwTable.Rows.Count - 1]["CurrBalance"].ToString()) > decLimit)
            {
                intMaxVal = decIRR;
                goto loopforIRR;
            }
            else if (Convert.ToDecimal(DtIRRNwTable.Rows[DtIRRNwTable.Rows.Count - 1]["CurrBalance"].ToString()) < (0 - decLimit))
            {
                intMinVal = decIRR;
                goto loopforIRR;
            }
            //*******End of loop Checking Whether to Continue the Loop or not********************//
            else
            {
                //If IRR is calculated then return the datatable
                DataTable dtOthers = new DataTable();
                DataTable dtInsurance = new DataTable();
                string strLogIrr, strIRRPath = string.Empty;
                System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                try //Try Catch block if No LogIRR is defined in config
                {
                    strLogIrr = (string)AppReader.GetValue("LogIRR", typeof(string));
                    strIRRPath = (string)AppReader.GetValue("FilePath", typeof(string));
                }
                catch
                {
                    strLogIrr = "No";
                }
                DtIRRNwTable.TableName = Irrtype.ToString();
                if (strLogIrr == "Yes")
                {
                    try
                    {
                        if (strIRRPath == string.Empty)
                            strIRRPath = System.Environment.CurrentDirectory + "\\Data\\IRR";
                        if (!Directory.Exists(strIRRPath))
                        {
                            Directory.CreateDirectory(strIRRPath);
                        }
                        DtIRRNwTable.WriteXml(Path.Combine(strIRRPath, DtIRRNwTable.TableName + System.Guid.NewGuid().ToString() + ".xls"));
                    }
                    catch (Exception ex)
                    {
                        ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    }
                }
                double decIRRCutoff = 0;//Try Catch block if No IrrCutoff is defined in config
                try
                {
                    decIRRCutoff = (double)AppReader.GetValue("IRRCutOff", typeof(double));
                }
                catch
                {

                }
                if (decIRRCutoff != 0)
                {
                    if (decIRR > decIRRCutoff)
                    {
                        throw new Exception("Unrealistic " + Irrtype.ToString().Replace("_", " ") + ", change the parameters to arrive at a nominal IRR");
                    }
                }
                switch (Irrtype)
                {
                    case IRRType.Accounting_IRR:
                        decAccountingIRR = Math.Round(decIRR, 4);
                        dtAccountingIRR = DtIRRNwTable.Copy();
                        Irrtype = IRRType.Business_IRR;
                        blnAccountingIRR = true;
                        break;
                    case IRRType.Business_IRR:
                        decBussinessIRR = Math.Round(decIRR, 4);
                        dtRepayStructure = DtIRRNwTable.Clone();
                        if (DtIRRNwTable.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = DtIRRNwTable.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (DtIRRNwTable.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = DtIRRNwTable.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        Irrtype = IRRType.Company_IRR;
                        blnBusinessIRR = true;
                        break;
                    case IRRType.Company_IRR:
                        decComapnyIRR = Math.Round(decIRR, 4);

                        if (DtIRRNwTable.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = DtIRRNwTable.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (DtIRRNwTable.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = DtIRRNwTable.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        blnCompanyIRR = true;
                        break;
                }
                if (!blnAccountingIRR || !blnBusinessIRR || !blnCompanyIRR)
                {
                    goto Start;
                }

                DataView dvRepayStructure = dtRepayStructure.DefaultView;
                //                string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType" };

                //Thangam M on 24/Oct/2012 for Other amount issue
                string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType", "PrevBalance" };
                //End here

                DataTable dtRepayStruct = dvRepayStructure.ToTable(true, strColumn);
                if (!dtAccountingIRR.Columns.Contains("Insurance"))
                {
                    dtAccountingIRR.Columns.Add("Insurance", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("Others"))
                {
                    dtAccountingIRR.Columns.Add("Others", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW start
                if (!dtAccountingIRR.Columns.Contains("Tax"))
                {
                    dtAccountingIRR.Columns.Add("Tax", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW end

                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                if (!dtAccountingIRR.Columns.Contains("ET_IW"))
                {
                    dtAccountingIRR.Columns.Add("ET_IW", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("ET_OW"))
                {
                    dtAccountingIRR.Columns.Add("ET_OW", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("CUS_OW"))
                {
                    dtAccountingIRR.Columns.Add("CUS_OW", typeof(decimal));
                }
                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end

                if (dtRepayStruct.Rows.Count > 0)
                {
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Insurance");
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Others");
                }
                //Filtering only installments as it will used to arrive repayment structure.

                dtAccountingIRR.DefaultView.RowFilter = "TransactionType ='Installment'";
                //dtAccountingIRR.Columns["PrincipalAmount"].ReadOnly = false;


                foreach (DataRow drIrrRows in dtAccountingIRR.Rows)
                {
                    drIrrRows["InstallmentDate"] = drIrrRows["InstallmentDate"];
                    drIrrRows["Installment_Date"] = Convert.ToDateTime(drIrrRows["InstallmentDate"]).ToString(strDateFormat);
                    drIrrRows["From_Date"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);
                    drIrrRows["To_Date"] = Convert.ToDateTime(drIrrRows["ToDate"]).ToString(strDateFormat);
                    drIrrRows["Charge"] = Convert.ToDecimal(((decimal)drIrrRows["Charge"]).ToString("0.00"));

                    drIrrRows["InstallmentAmount"] = Convert.ToDecimal(((decimal)drIrrRows["InstallmentAmount"]).ToString("0.00"));
                    if (drIrrRows["Insurance"].ToString() != "")
                    {
                        drIrrRows["Insurance"] = Convert.ToDecimal(((decimal)drIrrRows["Insurance"]).ToString("0.00"));
                    }
                    if (drIrrRows["Others"].ToString() != "")
                    {
                        drIrrRows["Others"] = Convert.ToDecimal(((decimal)drIrrRows["Others"]).ToString("0.00"));
                    }
                }

                //if (dtAccountingIRR.Columns.Contains("PrincipalAmount"))
                //{
                //    dtAccountingIRR.Columns["PrincipalAmount"].Expression = "InstallmentAmount-Charge";
                //}
                DataRow[] drsel = dtIRRDetails.Select("TransactionType='INTEREST'");

                //for (int i = 0; i < drsel.Length; i++)
                //{
                //    dtAccountingIRR.Rows[i]["Charge"] = drsel[i]["Charge"].ToString();
                //}

                dtAccountingIRR.AcceptChanges();



                return dtAccountingIRR.DefaultView.ToTable("Repayment Structure");
            }
        }

        protected DateTime FunGetLastDate(DateTime dtCurr_date)
        {
            int intCurMnth = dtCurr_date.Month;
            int intCurYr = dtCurr_date.Year;
            int intLastDay = DateTime.DaysInMonth(intCurYr, intCurMnth);

            return Convert.ToDateTime(intCurMnth.ToString() + "/" + intLastDay + "/" + intCurYr + " 12:00:00 AM");
        }

        //Latest Principal method
        public DataSet FunPubGenerateRepaymentStructure_Principal_MethodNew(DataTable dtRepaymentSchedule, DataTable dtCashInflow, DataTable dtCashOutflow, string strFrequency, int intTenure, string strTenureType, string strDateFormat, decimal decPrincipleAmount, decimal decRateofInt, string strIRRrest, string strTimeval, string strInstallmentType, decimal decLimit, decimal decCTR, decimal decPLR, DateTime dtAppdate, decimal? decResidualValue, decimal? decResidualAmount, RepaymentType returnType, out double decAccountingIRR, out double decBussinessIRR, out double decComapnyIRR, string strMorType, DataTable dtMoratoriumInput, string StrLob, int intLevy, DateTime dtDocFBDate)
        {
            decAccountingIRR = 0;
            decBussinessIRR = 0;
            decComapnyIRR = 0;
            DateTime DtAccStartDate;
            int intCoutFlowCount = 0;
            bool blnAccountingIRR = false, blnBusinessIRR = false, blnCompanyIRR = false;
            DataTable dtAccountingIRR = new DataTable();
            IRRType Irrtype = IRRType.Accounting_IRR;
            DataTable dtRepayStructure = new DataTable();
            intCoutFlowCount = dtCashOutflow.Rows.Count;

        Start:
            #region IRRTable declaration and colunm addition
            DataTable dtIRRDetails = new DataTable();

            dtIRRDetails.Columns.Add("NoofDays", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentNo", typeof(int));
            dtIRRDetails.Columns.Add("FromDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("ToDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("From_Date");
            dtIRRDetails.Columns.Add("To_Date");
            dtIRRDetails.Columns.Add("Installment_Date");
            dtIRRDetails.Columns.Add("InstallmentDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("Amount", typeof(decimal));
            dtIRRDetails.Columns.Add("TransactionType");
            dtIRRDetails.Columns.Add("PrevBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("CurrBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("Slno", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("Charge", typeof(decimal));
            dtIRRDetails.Columns.Add("PrincipalAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("IsMor");
            #endregion

            #region Only for Moratorium
            string MoraStDate = "";
            string MoraEndDate = "";
            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                MoraStDate = dtMoratoriumInput.Rows[0]["Fromdate"].ToString();// DateTime.Now.AddDays(61).ToString("MM/dd/yyyy");
                MoraEndDate = dtMoratoriumInput.Rows[0]["Todate"].ToString(); //Convert.ToDateTime(MoraStDate).AddDays(45).ToString("MM/dd/yyyy");
                strMorType = dtMoratoriumInput.Rows[0]["Moratoriumtype"].ToString().ToUpper();
            }
            #endregion
            //IRR Row
            DataRow drIRR;

            #region Adding First Row By default
            DateTime dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());//StringToDate(dtRepaymentSchedule.Rows[0]["FromDate"].ToString(), strDateFormat); ;
            DateTime dtEndDate = dtStartDate;

            #endregion

            #region Adding Cash Inflow Details
            foreach (DataRow drCashInflow in dtCashInflow.Rows)
            {
                ///CashFlow_Flag_ID 34 Hardcoded for Not to Include UMFC In IRR Calculation
                ///drCashInflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if ((drCashInflow[Irrtype.ToString()].ToString() == "True" || drCashInflow[Irrtype.ToString()].ToString() == "1")
                    && drCashInflow["CashFlow_Flag_ID"].ToString() != "34")
                {

                    drIRR = dtIRRDetails.NewRow();
                    drIRR["Amount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentAmount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentNo"] = "0";
                    drIRR["FromDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["InstallmentDate"] = drCashInflow["Date"].ToString();// StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["ToDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["NoofDays"] = "0";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            #endregion

            #region Adding Cash Outflow Details
            foreach (DataRow drCashOutflow in dtCashOutflow.Rows)
            {
                ///drCashOutflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drCashOutflow[Irrtype.ToString()].ToString() == "True" || drCashOutflow[Irrtype.ToString()].ToString() == "1")
                {
                    // considering Cash flow other than Margin amount (43 flag id for Margin payment)
                    if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "43")
                    {
                        //adding cashflow other than margin cashflows for Company IRR and Business IRR
                        if (Irrtype != IRRType.Accounting_IRR)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            //changing for Defferred payment multiple row issue
                            //drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            //drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["Amount"] = 0 - decPrincipleAmount;
                            drIRR["InstallmentAmount"] = 0 - decPrincipleAmount;
                            drIRR["InstallmentNo"] = "0";
                            //changing for Defferred payment multiple row issue it should be a latest payment date i.e cashflow date
                            //drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            //drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            //drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["FromDate"] = dtAppdate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["InstallmentDate"] = dtAppdate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["ToDate"] = dtAppdate;
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            //changing for Defferred payment multiple row issue
                            //drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";                          
                            drIRR["CurrBalance"] = decPrincipleAmount;//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                            break;
                        }
                        //In case of Accounting IRR adding all outflows at application date itself Changed on 09-Mar-2011
                        else if (Irrtype == IRRType.Accounting_IRR)
                        {
                            //adding cashflow other than Delear payment for accounting IRR  (41 flag id for Dealer payment)
                            //if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "41")
                            //{
                            drIRR = dtIRRDetails.NewRow();
                            //drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            //drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["Amount"] = 0 - decPrincipleAmount;
                            drIRR["InstallmentAmount"] = 0 - decPrincipleAmount;
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["InstallmentDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["ToDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            //changing for Defferred payment multiple row issue
                            //drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";                          
                            drIRR["CurrBalance"] = decPrincipleAmount;//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                            break;
                            //}
                        }
                    }

                }

            }
            #endregion
            int intLoop = 0;
            int intNoofInstallment = 0;
            DateTime dtFromDate = new DateTime();
            DateTime dtForFBDate = new DateTime();
            int intNoLocalInstallment = 0;
            DtAccStartDate = dtAppdate;
            //strFrequency = "6";
            if (strFrequency != "9")  // For Bullet Principal
            {
                DataRow[] drRepay_Array = dtRepaymentSchedule.Select("CashFlow_Flag_ID=23", "slno asc");
                foreach (DataRow drRepay in drRepay_Array)
                {
                    if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                    {
                        int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                        int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                        int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                        dtStartDate = Convert.ToDateTime(drRepay["FromDate"]);
                        dtForFBDate = Convert.ToDateTime(drRepay["ToDate"]);
                        dtFromDate = dtStartDate;
                        if (intLoop == 0)
                        {
                            DtAccStartDate = dtStartDate;
                        }
                        intNoLocalInstallment = intNoofInstallment;

                        for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["FromDate"] = dtFromDate;
                            drIRR["ToDate"] = dtFromDate;
                            drIRR["InstallmentDate"] = dtFromDate;

                            if (intLoopCount == intTotalInstallment - 1 && drRepay_Array.Length - 1 == intLoop)
                            {
                                drIRR["ToDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                                drIRR["InstallmentDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                            }

                            switch (returnType)
                            {
                                case RepaymentType.FC:
                                case RepaymentType.WC:
                                case RepaymentType.TLE:
                                    switch (strTenureType.ToLower())
                                    {
                                        case "months":
                                            strFrequency = "4";
                                            break;
                                        case "weeks":
                                            strFrequency = "2";
                                            break;
                                        case "days":
                                            strFrequency = "0";
                                            break;
                                    }
                                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                    //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                                    drIRR["ToDate"] = dtFromDate;
                                    drIRR["InstallmentDate"] = dtFromDate;
                                    break;
                                default:
                                    if (strTimeval == "advancefbd" || strTimeval == "arrearsfbd")
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, dtForFBDate, intLoopCount + 1);
                                    else
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1, DtAccStartDate);
                                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
                                    break;
                            }
                            //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

                            if (drRepay["Amount"].ToString().Trim() != "")
                                drIRR["Amount"] = drRepay["Amount"].ToString();
                            else
                                drIRR["Amount"] = "0";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = "0.00";
                            drIRR["Amount"] = decPrincipleAmount;
                            drIRR["TransactionType"] = "Installment";
                            drIRR["InstallmentAmount"] = drRepay["PerInstall"].ToString();
                            intNoLocalInstallment = intNoLocalInstallment + 1;
                            drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                            drIRR["Charge"] = "0.00";

                            dtIRRDetails.Rows.Add(drIRR);

                        }

                        intNoofInstallment = intNoLocalInstallment;
                        intLoop++;
                    }
                }
            }
            else
            {
                DataRow[] drRepay_Array = dtRepaymentSchedule.Select("CashFlow_Flag_ID=23", "slno asc");
                foreach (DataRow drRepay in drRepay_Array)
                {
                    if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                    {
                        int intFromInstallment = 1;
                        int intToInstallment = intTenure;
                        int intTotalInstallment = 1;

                        dtStartDate = Convert.ToDateTime(drRepay["FromDate"]);
                        dtFromDate = dtStartDate;
                        intNoLocalInstallment = intNoofInstallment;

                        for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["FromDate"] = dtStartDate.AddMonths(-1);
                            drIRR["ToDate"] =
                            drIRR["InstallmentDate"] = FunPubGetNextDate(strFrequency, dtStartDate.AddMonths(-1), intTenure);

                            if (drRepay["Amount"].ToString().Trim() != "")
                                drIRR["Amount"] = drRepay["Amount"].ToString();
                            else
                                drIRR["Amount"] = "0";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = "0.00";
                            drIRR["Amount"] = decPrincipleAmount;
                            drIRR["TransactionType"] = "Installment";
                            drIRR["InstallmentAmount"] = drRepay["PerInstall"].ToString();
                            intNoLocalInstallment = intNoLocalInstallment + 1;
                            drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                            drIRR["Charge"] = "0.00";

                            dtIRRDetails.Rows.Add(drIRR);

                        }

                        intNoofInstallment = intNoLocalInstallment;
                        intLoop++;
                    }
                }
            }



            int intEndDateCount = 0;
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Slno"] = intEndDateCount + 1;
                intEndDateCount++;
            }

            #region For Deffered Payments
            DataView dvIrrDetails = dtIRRDetails.DefaultView;
            dvIrrDetails.Sort = "ToDate,InstallmentNo,InstallmentAmount ASC";

            dtIRRDetails = dvIrrDetails.ToTable();

            bool blnBusIRRdone = false;
            intEndDateCount = 0;

            #endregion

            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {

                if (intEndDateCount != 0)
                    drIrrRows["FromDate"] = dtIRRDetails.Rows[intEndDateCount - 1]["ToDate"];
                else
                    drIrrRows["FromDate"] = dtAppdate;


                #region For Moratorium
                bool blnMor = false;
                if (strMorType != "")// == "INTEREST")
                {
                    if (Convert.ToDateTime(drIrrRows["FromDate"].ToString()) > Convert.ToDateTime(MoraStDate) && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) < Convert.ToDateTime(MoraEndDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraEndDate) && blnMor == false)
                    {
                        drIrrRows["FromDate"] = Convert.ToDateTime(dtMoratoriumInput.Rows[0]["ToDate"].ToString());
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        blnMor = true;
                    }
                    else if (Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                       && Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) < Convert.ToDateTime(MoraEndDate) &&
                        blnMor == false)
                    {
                        //drIrrRows["NoofDays"] = 0;
                        drIrrRows["IsMor"] = "0";
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                    else if (blnMor == false && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) <= Convert.ToDateTime(MoraStDate) &&
                        Convert.ToDateTime(drIrrRows["ToDate"].ToString()) >= Convert.ToDateTime(MoraEndDate))
                    //dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").Length > 0)
                    {
                        blnMor = true;
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = Convert.ToInt32(drIrrRows["NoofDays"].ToString()) - DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(MoraStDate), Convert.ToDateTime(MoraEndDate), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

                    }
                    else
                    {
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                }
                #endregion
                else
                {
                    drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

                }
                intEndDateCount++;
            }
            //dtRepaymentSchedule, DataTable dtCashInflow, DataTable dtCashOutflow, string strFrequency, int intTenure, string strTenureType, string strDateFormat, decimal decPrincipleAmount,
            //decimal decRateofInt, string strIRRrest, string strTimeval, string strInstallmentType, decimal decLimit, decimal decCTR, decimal decPLR, DateTime dtAppdate, decimal? decResidualValue, decimal? decResidualAmount, RepaymentType returnType, out double decAccountingIRR, out double decBussinessIRR, out double decComapnyIRR, string strMorType, DataTable dtMoratoriumInput, string StrLob, int intLevy)



            #region Reformation of Repay Structure

            DataTable dtNwIRRDetails = dtIRRDetails.Clone();
            dtNwIRRDetails.Columns.Add("Installment_Interest", typeof(decimal));

            decimal decdivval = 0;
            int intRowCount = dtIRRDetails.Rows.Count;
            int intLevyMonths = 0;
            //to be changed
            switch (strFrequency)
            {
                case "4":
                    decdivval = intTenure / 1;
                    break;
                case "5":
                    decdivval = intTenure / 2;
                    break;
                case "6":
                    decdivval = intTenure / 3;
                    break;
                case "7":
                    decdivval = intTenure / 6;
                    break;
                case "8":
                    decdivval = intTenure / 12;
                    break;
                case "9":
                    decdivval = 1;
                    break;
            }

            //getting equated principal amount
            if (strFrequency == "9")//For Bullet
                decdivval = 1;
            else
                decdivval = Convert.ToDecimal(dtIRRDetails.Compute("max(InstallmentNo)", ""));





            decimal decPrincipal = decPrincipleAmount / (Convert.ToDecimal(decdivval));


            #region "Levy structure"
            bool blnIsRoundedL;
            DataTable dtRepayLevyStruct = new DataTable();
            if (intLevy == 9)//Bullet
            {
                dtRepayLevyStruct.Columns.Add("NoofDays");
                dtRepayLevyStruct.Columns.Add("InstallmentNo");
                dtRepayLevyStruct.Columns.Add("FromDate");
                dtRepayLevyStruct.Columns.Add("ToDate");
                dtRepayLevyStruct.Columns.Add("InstallmentDate");
                dtRepayLevyStruct.Columns.Add("CurrentBalance");
                dtRepayLevyStruct.Columns.Add("InstallmentAmount");
                dtRepayLevyStruct.Columns.Add("Amount");
                dtRepayLevyStruct.Columns["InstallmentAmount"].DataType = typeof(decimal);
                dtRepayLevyStruct.Columns["Amount"].DataType = typeof(decimal);
                dtRepayLevyStruct.Columns["NoofDays"].DataType = typeof(int);
                dtRepayLevyStruct.Columns["InstallmentDate"].DataType = typeof(DateTime);
                dtRepayLevyStruct.Columns.Add("RecoveryYear"); // For Moratorium
                drIRR = dtRepayLevyStruct.NewRow();
                drIRR["Amount"] = decPrincipleAmount;
                drIRR["InstallmentAmount"] = decPrincipleAmount;
                drIRR["InstallmentNo"] = "1";
                drIRR["FromDate"] = dtAppdate;
                drIRR["InstallmentDate"] =
                drIRR["ToDate"] = dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["InstallmentDate"].ToString();//drCashOutflow["Date"].ToString();

                drIRR["CurrentBalance"] = decPrincipleAmount;//"0.00";
                drIRR["NoofDays"] = "0";
                dtRepayLevyStruct.Rows.Add(drIRR);
            }
            else
            {

                if (strTimeval == "advancefbd" || strTimeval == "arrearsfbd")
                    dtRepayLevyStruct = FunPubCalculateRepaymentDetails(intLevy.ToString(), intTenure, strTenureType, decPrincipleAmount, decRateofInt, returnType, decResidualValue, dtDocFBDate, dtAppdate, 10, out blnIsRoundedL, strTimeval);
                else
                    dtRepayLevyStruct = FunPubCalculateRepaymentDetails(intLevy.ToString(), intTenure, strTenureType, decPrincipleAmount, decRateofInt, returnType, decResidualValue, dtAppdate, dtAppdate, 10, out blnIsRoundedL, strTimeval);

            }


            drIRR = dtRepayLevyStruct.NewRow();
            drIRR["Amount"] = 0 - decPrincipleAmount;
            drIRR["InstallmentAmount"] = 0 - decPrincipleAmount;
            drIRR["InstallmentNo"] = "0";
            drIRR["FromDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
            drIRR["InstallmentDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
            drIRR["ToDate"] = dtAppdate;//drCashOutflow["Date"].ToString();

            drIRR["CurrentBalance"] = decPrincipleAmount;//"0.00";
            drIRR["NoofDays"] = "0";
            dtRepayLevyStruct.Rows.Add(drIRR);

            DataView dvRepayLevyStruct = dtRepayLevyStruct.DefaultView;
            dvRepayLevyStruct.Sort = "InstallmentDate,InstallmentNo ASC";
            dtRepayLevyStruct = dvRepayLevyStruct.ToTable();

            #endregion

            #region "arriving structure"
            if (Convert.ToInt32(strFrequency) <= intLevy)
            {
                for (int i = 0; i <= dtIRRDetails.Rows.Count - 1; i++)
                {
                    DataRow drNwIRR = dtNwIRRDetails.NewRow();
                    drNwIRR["InstallmentNo"] = dtIRRDetails.Rows[i]["InstallmentNo"].ToString();
                    drNwIRR["FromDate"] = dtIRRDetails.Rows[i]["FromDate"].ToString();
                    drNwIRR["ToDate"] = dtIRRDetails.Rows[i]["ToDate"].ToString();
                    drNwIRR["InstallmentDate"] = dtIRRDetails.Rows[i]["InstallmentDate"].ToString();
                    drNwIRR["Amount"] = dtIRRDetails.Rows[i]["Amount"].ToString();
                    drNwIRR["TransactionType"] = dtIRRDetails.Rows[i]["TransactionType"].ToString();
                    drNwIRR["PrevBalance"] = dtIRRDetails.Rows[i]["PrevBalance"].ToString(); ;
                    drNwIRR["CurrBalance"] = dtIRRDetails.Rows[i]["CurrBalance"].ToString(); ;
                    drNwIRR["Slno"] = (dtNwIRRDetails.Rows.Count).ToString(); //dtIRRDetails.Rows[i]["Slno"].ToString();
                    // dtIRRDetails.Rows[i]["InstallmentAmount"].ToString();
                    drNwIRR["Charge"] = 0; // dtIRRDetails.Rows[i]["Charge"].ToString();
                    if (dtIRRDetails.Rows[i]["InstallmentNo"].ToString() == "0")
                    {
                        drNwIRR["PrincipalAmount"] = 0;
                        drNwIRR["InstallmentAmount"] = -decPrincipleAmount;
                    }
                    else
                    {
                        drNwIRR["PrincipalAmount"] = decPrincipal;//dtIRRDetails.Rows[i]["PrincipalAmount"].ToString();
                        drNwIRR["InstallmentAmount"] = 0;
                    }
                    drNwIRR["IsMor"] = dtIRRDetails.Rows[i]["IsMor"].ToString();
                    drNwIRR["Installment_Interest"] = 0;
                    drNwIRR["NoofDays"] = dtIRRDetails.Rows[i]["NoofDays"].ToString();
                    dtNwIRRDetails.Rows.Add(drNwIRR);

                }
                //To calculate Interest
                for (int j = 1; j <= dtNwIRRDetails.Rows.Count - 1; j++)
                {
                    dtNwIRRDetails.Rows[j]["PrevBalance"] = dtNwIRRDetails.Rows[j - 1]["CurrBalance"].ToString();
                    dtNwIRRDetails.Rows[j]["CurrBalance"] = Convert.ToDecimal(dtNwIRRDetails.Rows[j]["CurrBalance"].ToString())
                        + Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrevBalance"].ToString())
                        - Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrincipalAmount"].ToString());

                    int intNOD = Convert.ToInt32(dtNwIRRDetails.Rows[j]["NoofDays"].ToString());
                    Decimal PrevBal = Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrevBalance"].ToString());

                    dtNwIRRDetails.Rows[j]["Charge"] = ((PrevBal * intNOD * (decRateofInt / 100)) / 365);
                }

                //For Advance cases we have same installmentdate 
                DataTable dtRepayLevyStructnew = dtRepayLevyStruct.Copy();
                if (strTimeval == "advancefbd" || strTimeval == "advance")
                    dtRepayLevyStructnew.Rows[0].Delete();


                int intslno = 1;
                decimal decSumcharges = 0;
                string StrFilterSLno = "";
                for (int j = 1; j <= dtNwIRRDetails.Rows.Count - 1; j++)
                {
                    foreach (DataRow dr in dtRepayLevyStructnew.Rows)
                    {
                        if (dr["InstallmentDate"].ToString() == dtNwIRRDetails.Rows[j]["InstallmentDate"].ToString())
                        {
                            decSumcharges = (decimal)dtNwIRRDetails.Compute("Sum(Charge)", " Slno >= " + intslno.ToString() + " and Slno <= " + j.ToString());
                            dtNwIRRDetails.Rows[j]["Charge"] = decSumcharges;
                            intslno = j + 1;
                            StrFilterSLno += j.ToString() + ",";
                        }
                    }
                }
                StrFilterSLno = StrFilterSLno.Remove(StrFilterSLno.Length - 1);
                DataRow[] drNoCharges = dtNwIRRDetails.Select("Slno not in(" + StrFilterSLno + ")");
                foreach (DataRow dr in drNoCharges)
                {
                    dr["Charge"] = 0;
                }

                for (int j = 1; j <= dtNwIRRDetails.Rows.Count - 1; j++)
                {

                    dtNwIRRDetails.Rows[j]["InstallmentAmount"] =
                        dtNwIRRDetails.Rows[j]["Installment_Interest"] = Convert.ToDecimal(dtNwIRRDetails.Rows[j]["Charge"]) + Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrincipalAmount"]);
                }

            }
            else
            {
                for (int i = 0; i <= dtRepayLevyStruct.Rows.Count - 1; i++)
                {
                    DataRow drNwIRR = dtNwIRRDetails.NewRow();
                    drNwIRR["InstallmentNo"] = dtRepayLevyStruct.Rows[i]["InstallmentNo"].ToString();
                    drNwIRR["FromDate"] = dtRepayLevyStruct.Rows[i]["FromDate"].ToString();
                    drNwIRR["ToDate"] = dtRepayLevyStruct.Rows[i]["ToDate"].ToString();
                    drNwIRR["InstallmentDate"] = dtRepayLevyStruct.Rows[i]["InstallmentDate"].ToString();
                    drNwIRR["Amount"] = dtRepayLevyStruct.Rows[i]["Amount"].ToString();
                    drNwIRR["TransactionType"] = "Installment";
                    drNwIRR["PrevBalance"] = 0;
                    if (i == 0)
                    {
                        drNwIRR["CurrBalance"] = decPrincipleAmount;
                        drNwIRR["InstallmentAmount"] = -decPrincipleAmount;
                    }
                    else
                    {
                        drNwIRR["CurrBalance"] = 0;
                        drNwIRR["InstallmentAmount"] = 0;// dtIRRDetails.Rows[i]["InstallmentAmount"].ToString();
                    }
                    drNwIRR["Slno"] = (dtNwIRRDetails.Rows.Count).ToString(); //dtIRRDetails.Rows[i]["Slno"].ToString();
                    drNwIRR["Charge"] = 0; // dtIRRDetails.Rows[i]["Charge"].ToString();
                    drNwIRR["PrincipalAmount"] = 0;//dtIRRDetails.Rows[i]["PrincipalAmount"].ToString();
                    //drNwIRR["IsMor"] = dtIRRDetails.Rows[i]["IsMor"].ToString();
                    drNwIRR["Installment_Interest"] = 0;
                    drNwIRR["NoofDays"] = dtRepayLevyStruct.Rows[i]["NoofDays"].ToString();
                    dtNwIRRDetails.Rows.Add(drNwIRR);
                }



                for (int j = 1; j <= dtNwIRRDetails.Rows.Count - 1; j++)
                {
                    foreach (DataRow dr in dtIRRDetails.Rows)
                    {
                        if (dr["InstallmentDate"].ToString() == dtNwIRRDetails.Rows[j]["InstallmentDate"].ToString())
                        {
                            dtNwIRRDetails.Rows[j]["PrincipalAmount"] = decPrincipal;
                        }
                    }
                }

                //To calculate Interest
                for (int j = 1; j <= dtNwIRRDetails.Rows.Count - 1; j++)
                {
                    dtNwIRRDetails.Rows[j]["PrevBalance"] = dtNwIRRDetails.Rows[j - 1]["CurrBalance"].ToString();
                    dtNwIRRDetails.Rows[j]["CurrBalance"] = Convert.ToDecimal(dtNwIRRDetails.Rows[j]["CurrBalance"].ToString())
                        + Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrevBalance"].ToString())
                        - Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrincipalAmount"].ToString());

                    int intNOD = Convert.ToInt32(dtNwIRRDetails.Rows[j]["NoofDays"].ToString());
                    Decimal PrevBal = Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrevBalance"].ToString());

                    dtNwIRRDetails.Rows[j]["Charge"] = ((PrevBal * intNOD * (decRateofInt / 100)) / 365);

                    dtNwIRRDetails.Rows[j]["InstallmentAmount"] =
                       dtNwIRRDetails.Rows[j]["Installment_Interest"] = Convert.ToDecimal(dtNwIRRDetails.Rows[j]["Charge"]) + Convert.ToDecimal(dtNwIRRDetails.Rows[j]["PrincipalAmount"]);
                }

            }


            intNoLocalInstallment = dtNwIRRDetails.Rows.Count;



            #endregion

            #endregion

            #region Insurance
            DataRow[] drRepay_InsuranceArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID =25", "slno asc");
            foreach (DataRow drRepay in drRepay_InsuranceArray)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                    dtFromDate = dtStartDate;

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = intFromInstallment; intLoopCount <= intToInstallment; intLoopCount++)
                    {

                        DataRow drDetail = dtNwIRRDetails.Select("InstallmentNo='" + intLoopCount.ToString() + "' and TransactionType ='Installment'").Last();

                        drIRR = dtNwIRRDetails.NewRow();
                        drIRR["FromDate"] = drDetail["FromDate"].ToString();//dtFromDate.ToString(strDateFormat);
                        drIRR["ToDate"] = drDetail["FromDate"].ToString();//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = drDetail["InstallmentDate"].ToString();//.ToString(strDateFormat);
                        drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                        drIRR["TransactionType"] = "Insurance";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["NoofDays"] = "0";
                        drIRR["PrincipalAmount"] = "0";
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        drIRR["Installment_Interest"] = "0.00";
                        dtNwIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }

            #endregion

            #region Others
            DataRow[] drRepay_OthersArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID <> 23 and CashFlow_Flag_ID <> 25", "slno asc");
            foreach (DataRow drRepay in drRepay_OthersArray)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                    dtFromDate = dtStartDate;

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = intFromInstallment; intLoopCount <= intToInstallment; intLoopCount++)
                    {
                        DataRow drDetail = dtNwIRRDetails.Select("InstallmentNo='" + intLoopCount.ToString() + "' and TransactionType ='Installment'").Last();
                        drIRR = dtNwIRRDetails.NewRow();
                        drIRR["FromDate"] = drDetail["FromDate"].ToString();//dtFromDate.ToString(strDateFormat);
                        drIRR["ToDate"] = drDetail["FromDate"].ToString();//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = drDetail["InstallmentDate"].ToString();//.ToString(strDateFormat);
                        drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                        //Inflow/Outflow/Both 1/2/3
                        switch (drRepay["Outflow"].ToString())
                        {
                            case "1"://Inflow
                            case "3"://Both
                                if (drRepay["Entity"].ToString() == "3")//Customer
                                    drIRR["TransactionType"] = "Others";
                                else//dealer
                                    drIRR["TransactionType"] = "ET_IW";
                                break;
                            case "2"://outflow
                                if (drRepay["Entity"].ToString() == "3")//Customer
                                    drIRR["TransactionType"] = "CUS_OW";
                                else//dealer
                                    drIRR["TransactionType"] = "ET_OW";
                                break;
                            default:
                                drIRR["TransactionType"] = "Others";
                                break;
                        }
                        //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                        if (drRepay["Amort"].ToString() == "2")//Amort
                            drIRR["InstallmentAmount"] = 0;
                        else
                            drIRR["InstallmentAmount"] = drRepay["PerInstall"];  //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["NoofDays"] = "0";
                        drIRR["PrincipalAmount"] = "0";
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        drIRR["Installment_Interest"] = "0.00";
                        dtNwIRRDetails.Rows.Add(drIRR);
                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            #endregion

            // Code removed 2
            decimal decAmount = (decimal)dtNwIRRDetails.Compute("Sum(InstallmentAmount)", "NoofDays >0"); //and TransactionType = 'Installment'
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Amount"] = decAmount;
            }

            //DataTable dtDupIRRTable = dtNwIRRDetails.Copy();
            DataTable dtChkIRRTable = dtNwIRRDetails.Copy();

            DataView dvChkIRRTable = dtChkIRRTable.DefaultView;
            dvChkIRRTable.Sort = "ToDate,InstallmentNo,InstallmentAmount ASC";

            dtChkIRRTable = dvChkIRRTable.ToTable();

            double intMinVal = 0.00;
            double intMaxVal = 1000.00;

            #region Min val Max val Assiging part
        loopforIRR:
            if (intMaxVal == intMinVal)
            {
                throw new Exception("Incorrect cashflow details,cannot calculate IRR");
            }
            double decIRR = (intMaxVal + intMinVal) / 2;
            //decIRR = Math.Round(decIRR, 4);
            int intnoofdays = 0;
            switch (strIRRrest)
            {
                case "monthly":
                    intnoofdays = 1200;
                    break;
                case "daily":
                    intnoofdays = 36500;
                    break;
                case "yearly":
                    intnoofdays = 100;
                    break;
            }
            double decExp = 1 + (decIRR / intnoofdays);
            int intdtRowcount = 0;
            decimal dbPrevBalance = 0;
            decimal dbCharge = 0;
            //Double dbCurrPrevBalance;
            #endregion

            #region IRR Calculation Loop
            foreach (DataRow drIRRRow in dtChkIRRTable.Rows)
            {
                if (intdtRowcount != 0)
                {
                    dbPrevBalance = Convert.ToDecimal(dtChkIRRTable.Rows[intdtRowcount - 1]["CurrBalance"].ToString());
                }
                switch (strIRRrest)
                {
                    case "monthly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"]) / 30))) - dbPrevBalance;
                        break;
                    case "daily":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"])))) - dbPrevBalance;
                        break;
                    case "yearly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                        break;
                }
                // drIRRRow["PrevBalance"] = Math.Round(dbPrevBalance, 2) + Math.Round(dbCharge, 2);

                #region For Moratorium
                if (strMorType.ToUpper() == "INTEREST")
                {
                    if (drIRRRow["IsMor"].ToString() != "")
                    {
                        //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                        switch (strIRRrest)
                        {
                            case "monthly":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                break;
                            case "daily":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                break;
                            case "yearly":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                                break;
                        }

                    }


                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);

                }
                #endregion
                else
                {
                    #region For Moratorium
                    if (strMorType.ToUpper() == "BOTH")
                    {
                        //if (drIRRRow["InstallmentAmount"].ToString() == "0")
                        //    dbCharge = 0;
                        if (drIRRRow["IsMor"].ToString() != "")
                        {
                            //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                            //drIRRRow["InstallmentAmount"] = 0;

                            switch (strIRRrest)
                            {
                                case "monthly":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                    break;
                                case "daily":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                    break;
                                case "yearly":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                                    break;
                            }
                        }
                    }
                    //else
                    //{
                    //    if (drIRRRow["IsMor"].ToString() != "")
                    //    {
                    //        drIRRRow["InstallmentAmount"] = 0;
                    //    }
                    //}
                    #endregion

                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);
                }
                intdtRowcount++;
            }

            dtChkIRRTable.AcceptChanges();

            #endregion

            //*******Checking Whether to Continue the Loop or not********************// 
            if (Convert.ToDecimal(dtChkIRRTable.Rows[dtChkIRRTable.Rows.Count - 1]["CurrBalance"].ToString()) > decLimit)
            {
                intMaxVal = decIRR;
                goto loopforIRR;
            }
            else if (Convert.ToDecimal(dtChkIRRTable.Rows[dtChkIRRTable.Rows.Count - 1]["CurrBalance"].ToString()) < (0 - decLimit))
            {
                intMinVal = decIRR;
                goto loopforIRR;
            }
            //*******End of loop Checking Whether to Continue the Loop or not********************//
            else
            {
                //If IRR is calculated then return the datatable
                DataTable dtOthers = new DataTable();
                DataTable dtInsurance = new DataTable();
                string strLogIrr, strIRRPath = string.Empty;
                System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
                try //Try Catch block if No LogIRR is defined in config
                {
                    strLogIrr = (string)AppReader.GetValue("LogIRR", typeof(string));
                    strIRRPath = (string)AppReader.GetValue("FilePath", typeof(string));
                }
                catch
                {
                    strLogIrr = "No";
                }
                dtChkIRRTable.TableName = Irrtype.ToString();
                if (strLogIrr == "Yes")
                {
                    try
                    {
                        if (strIRRPath == string.Empty)
                            strIRRPath = System.Environment.CurrentDirectory + "\\Data\\IRR";
                        if (!Directory.Exists(strIRRPath))
                        {
                            Directory.CreateDirectory(strIRRPath);
                        }
                        dtChkIRRTable.WriteXml(Path.Combine(strIRRPath, dtChkIRRTable.TableName + System.Guid.NewGuid().ToString() + ".xls"));
                    }
                    catch (Exception ex)
                    {
                        ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    }
                }
                double decIRRCutoff = 0;//Try Catch block if No IrrCutoff is defined in config
                try
                {
                    decIRRCutoff = (double)AppReader.GetValue("IRRCutOff", typeof(double));
                }
                catch
                {

                }
                if (decIRRCutoff != 0)
                {
                    if (decIRR > decIRRCutoff)
                    {
                        throw new Exception("Unrealistic " + Irrtype.ToString().Replace("_", " ") + ", change the parameters to arrive at a nominal IRR");
                    }
                }
                switch (Irrtype)
                {
                    case IRRType.Accounting_IRR:
                        decAccountingIRR = Math.Round(decIRR, 4);
                        dtAccountingIRR = dtNwIRRDetails.Copy();
                        Irrtype = IRRType.Business_IRR;
                        blnAccountingIRR = true;
                        break;
                    case IRRType.Business_IRR:
                        decBussinessIRR = Math.Round(decIRR, 4);
                        dtRepayStructure = dtNwIRRDetails.Clone();
                        if (dtNwIRRDetails.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = dtChkIRRTable.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (dtNwIRRDetails.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }

                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 start
                        if (dtNwIRRDetails.Select("TransactionType='ET_IW'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='ET_IW'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        if (dtNwIRRDetails.Select("TransactionType='ET_OW'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='ET_OW'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        if (dtNwIRRDetails.Select("TransactionType='CUS_OW'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='CUS_OW'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 end

                        Irrtype = IRRType.Company_IRR;
                        blnBusinessIRR = true;
                        break;
                    case IRRType.Company_IRR:
                        decComapnyIRR = Math.Round(decIRR, 4);

                        if (dtNwIRRDetails.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = dtNwIRRDetails.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (dtNwIRRDetails.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }

                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 start
                        if (dtNwIRRDetails.Select("TransactionType='ET_IW'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='ET_IW'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        if (dtNwIRRDetails.Select("TransactionType='ET_OW'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='ET_OW'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        if (dtNwIRRDetails.Select("TransactionType='CUS_OW'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtNwIRRDetails.Select("TransactionType='CUS_OW'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 end

                        blnCompanyIRR = true;
                        break;
                }
                if (!blnAccountingIRR || !blnBusinessIRR || !blnCompanyIRR)
                {
                    goto Start;
                }

                DataView dvRepayStructure = dtRepayStructure.DefaultView;
                string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType" };

                //Thangam M on 24/Oct/2012 for Other amount issue
                //string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType", "PrevBalance" };
                //End here

                DataTable dtRepayStruct = dvRepayStructure.ToTable(true, strColumn);
                if (!dtAccountingIRR.Columns.Contains("Insurance"))
                {
                    dtAccountingIRR.Columns.Add("Insurance", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("Others"))
                {
                    dtAccountingIRR.Columns.Add("Others", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW start
                if (!dtAccountingIRR.Columns.Contains("Tax"))
                {
                    dtAccountingIRR.Columns.Add("Tax", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW end

                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                if (!dtAccountingIRR.Columns.Contains("ET_IW"))
                {
                    dtAccountingIRR.Columns.Add("ET_IW", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("ET_OW"))
                {
                    dtAccountingIRR.Columns.Add("ET_OW", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("CUS_OW"))
                {
                    dtAccountingIRR.Columns.Add("CUS_OW", typeof(decimal));
                }
                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end

                if (dtRepayStruct.Rows.Count > 0)
                {
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Insurance");
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Others");

                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "ET_IW");
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "ET_OW");
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "CUS_OW");
                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end

                }

                dtAccountingIRR.DefaultView.RowFilter = "TransactionType ='Installment'";
                DataRow[] DrArrCheck = dtAccountingIRR.Select("InstallmentNo = '0'");
                foreach (DataRow DrCheck in DrArrCheck)
                {
                    dtAccountingIRR.Rows.Remove(DrCheck);
                }

                foreach (DataRow drIrrRows in dtAccountingIRR.Rows)
                {
                    drIrrRows["InstallmentDate"] = drIrrRows["InstallmentDate"];
                    drIrrRows["Installment_Date"] = Convert.ToDateTime(drIrrRows["InstallmentDate"]).ToString(strDateFormat);
                    drIrrRows["From_Date"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);
                    drIrrRows["To_Date"] = Convert.ToDateTime(drIrrRows["ToDate"]).ToString(strDateFormat);
                    drIrrRows["Charge"] = Convert.ToDecimal(((decimal)drIrrRows["Charge"]).ToString("0.00"));

                    drIrrRows["InstallmentAmount"] = Convert.ToDecimal(((decimal)drIrrRows["InstallmentAmount"]).ToString("0.00"));
                    if (drIrrRows["Insurance"].ToString() != "")
                    {
                        drIrrRows["Insurance"] = Convert.ToDecimal(((decimal)drIrrRows["Insurance"]).ToString("0.00"));
                    }
                    if (drIrrRows["Others"].ToString() != "")
                    {
                        drIrrRows["Others"] = Convert.ToDecimal(((decimal)drIrrRows["Others"]).ToString("0.00"));
                    }

                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                    if (drIrrRows["ET_IW"].ToString() != "")
                    {
                        drIrrRows["ET_IW"] = Convert.ToDecimal(((decimal)drIrrRows["ET_IW"]).ToString("0.00"));
                    }
                    if (drIrrRows["ET_OW"].ToString() != "")
                    {
                        drIrrRows["ET_OW"] = Convert.ToDecimal(((decimal)drIrrRows["ET_OW"]).ToString("0.00"));
                    }
                    if (drIrrRows["CUS_OW"].ToString() != "")
                    {
                        drIrrRows["CUS_OW"] = Convert.ToDecimal(((decimal)drIrrRows["CUS_OW"]).ToString("0.00"));
                    }

                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end

                }

                if (dtAccountingIRR.Columns.Contains("PrincipalAmount"))
                {
                    dtAccountingIRR.Columns["PrincipalAmount"].Expression = "InstallmentAmount-Charge";
                }
                string strcashflow_Id = dtRepaymentSchedule.Rows[0]["CashFlowID"].ToString();
                string strcashflow = dtRepaymentSchedule.Rows[0]["CashFlow"].ToString();
                string strcashflow_flag_Id = dtRepaymentSchedule.Rows[0]["CashFlow_Flag_ID"].ToString();

                DataTable dtRetRepayDetails = dtRepaymentSchedule.Clone();





                //For installment
                int intSLnno = 0;
                DataRow[] dtAccountingIRRInstall = dtAccountingIRR.Select("TransactionType ='Installment'");
                foreach (DataRow drAccountingIRRInstall in dtAccountingIRRInstall)
                {
                    DataRow dRow = dtRetRepayDetails.NewRow();
                    dRow["Slno"] = (intSLnno).ToString();
                    dRow["Amount"] = Convert.ToDecimal(((decimal)drAccountingIRRInstall["Installment_Interest"]).ToString("0.00"));
                    dRow["CashFlow"] = strcashflow;
                    dRow["PerInstall"] = Convert.ToDecimal(((decimal)drAccountingIRRInstall["Installment_Interest"]).ToString("0.00"));
                    dRow["CashFlowID"] = strcashflow_Id;
                    dRow["FromInstall"] = (intSLnno + 1).ToString();
                    dRow["ToInstall"] = (intSLnno + 1).ToString();
                    dRow["Breakup"] = "0";
                    dRow["FromDate"] = drAccountingIRRInstall["FromDate"].ToString();
                    dRow["ToDate"] = drAccountingIRRInstall["ToDate"].ToString();
                    dRow["Business_IRR"] = "True"; ;
                    dRow["Company_IRR"] = "True";
                    dRow["Accounting_IRR"] = "True";
                    dRow["CashFlow_Flag_ID"] = strcashflow_flag_Id;
                    dRow["TotalPeriodInstall"] = Convert.ToDecimal(((decimal)drAccountingIRRInstall["Installment_Interest"]).ToString("0.00"));
                    dtRetRepayDetails.Rows.Add(dRow);
                    intSLnno++;
                }


                //For Others and insurance Flag
                DataTable dtRepayGridNew = dtRepaymentSchedule.Copy();
                DataRow[] drREpayInstall = dtRepayGridNew.Select("CashFlow_Flag_ID=23");
                foreach (DataRow drow in drREpayInstall)
                {
                    drow.Delete();
                }
                dtRepayGridNew.AcceptChanges();
                for (int i = 0; i <= dtRepayGridNew.Rows.Count - 1; i++)
                {
                    DataRow dRow = dtRetRepayDetails.NewRow();
                    dRow["Slno"] = (intSLnno).ToString();
                    dRow["Amount"] = Convert.ToDecimal(((decimal)dtRepayGridNew.Rows[i]["Amount"]).ToString("0.00"));
                    dRow["CashFlow"] = dtRepayGridNew.Rows[i]["CashFlow"].ToString();
                    dRow["PerInstall"] = Convert.ToDecimal(((decimal)dtRepayGridNew.Rows[i]["PerInstall"]).ToString("0.00"));
                    dRow["CashFlowID"] = dtRepayGridNew.Rows[i]["CashFlowID"].ToString();
                    dRow["FromInstall"] = dtRepayGridNew.Rows[i]["FromInstall"].ToString();
                    dRow["ToInstall"] = dtRepayGridNew.Rows[i]["ToInstall"].ToString();
                    dRow["Breakup"] = "0";
                    dRow["FromDate"] = dtRepayGridNew.Rows[i]["FromDate"].ToString();
                    dRow["ToDate"] = dtRepayGridNew.Rows[i]["ToDate"].ToString();
                    dRow["Business_IRR"] = dtRepayGridNew.Rows[i]["Business_IRR"].ToString();
                    dRow["Company_IRR"] = dtRepayGridNew.Rows[i]["Company_IRR"].ToString();
                    dRow["Accounting_IRR"] = dtRepayGridNew.Rows[i]["Accounting_IRR"].ToString();
                    dRow["CashFlow_Flag_ID"] = dtRepayGridNew.Rows[i]["CashFlow_Flag_ID"].ToString();
                    dRow["TotalPeriodInstall"] = Convert.ToDecimal(((decimal)dtRepayGridNew.Rows[i]["TotalPeriodInstall"]).ToString("0.00"));
                    dtRetRepayDetails.Rows.Add(dRow);
                    intSLnno++;
                }



                DataSet dsRet = new DataSet();
                dsRet.Tables.Add(dtAccountingIRR.DefaultView.ToTable("Repayment Structure"));
                dsRet.Tables.Add(dtRetRepayDetails);
                return dsRet;

            }

            //return dtNwIRRDetails;

        }

        protected DateTime FunProCheckLastDate(string strOldDate, string strNewDate)
        {
            try
            {
                DateTime dtOldDate = Convert.ToDateTime(strOldDate);
                DateTime dtNewDate = Convert.ToDateTime(strNewDate);
                dtNewDate = Convert.ToDateTime(dtNewDate.Month.ToString() + "/" + dtOldDate.Day.ToString() + "/" + dtNewDate.Year.ToString());
                return dtNewDate;
            }
            catch
            {
                DateTime dtNewDate = Convert.ToDateTime(strNewDate);
                int intDays = DateTime.DaysInMonth(dtNewDate.Year, dtNewDate.Month);
                dtNewDate = new DateTime(dtNewDate.Year, dtNewDate.Month, intDays);

                return dtNewDate;
            }
        }


        public DataTable getGroupBY(DataTable dt, string ColumnNamesinDt, string GroupbyColumns, string TypeofCalculation)
        {
            if (ColumnNamesinDt == string.Empty || GroupbyColumns == string.Empty)
            {
                return dt;
            }

            string[] ColumnsinDt = ColumnNamesinDt.Split(',');

            DataTable _dt = dt.DefaultView.ToTable(true, GroupbyColumns);

            for (int idx = 0; idx < ColumnsinDt.Length; idx++)
            {
                if (ColumnsinDt[idx] != GroupbyColumns)
                {
                    _dt.Columns.Add(ColumnsinDt[idx]);
                }
            }

            for (int rowidx = 0; rowidx < _dt.Rows.Count; rowidx++)
            {
                for (int colidx = 0; colidx < ColumnsinDt.Length; colidx++)
                {
                    if (ColumnsinDt[colidx] != GroupbyColumns)
                    {
                        _dt.Rows[rowidx][colidx] = dt.Compute(TypeofCalculation + "(" + ColumnsinDt[colidx] + ")", GroupbyColumns + "='" + _dt.Rows[rowidx][GroupbyColumns].ToString() + "'");
                    }
                }
            }
            return _dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtRepaymentSchedule"></param>
        /// <param name="dtCashInflow"></param>
        /// <param name="dtCashOutflow"></param>
        /// <param name="strFrequency"></param>
        /// <param name="intTenure"></param>
        /// <param name="strTenureType"></param>
        /// <param name="strDateFormat"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decRateofInt"></param>
        /// <param name="strIRRrest"></param>
        /// <param name="strTimeval"></param>
        /// <param name="strInstallmentType"></param>
        /// <param name="decLimit"></param>
        /// <param name="Irrtype"></param>
        /// <param name="decResultIRR"></param>
        /// <param name="decCTR"></param>
        /// <param name="decPLR"></param>
        /// <param name="dtAppdate"></param>
        /// <param name="decResidualValue"></param>
        /// <param name="decResidualAmount"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public DataTable FunPubGenerateRepaymentStructure(DataTable dtRepaymentSchedule, DataTable dtCashInflow, DataTable dtCashOutflow, string strFrequency, int intTenure, string strTenureType, string strDateFormat, decimal decPrincipleAmount, decimal decRateofInt, string strIRRrest, string strTimeval, string strInstallmentType, decimal decLimit, decimal decCTR, decimal decPLR, DateTime dtAppdate, decimal? decResidualValue, decimal? decResidualAmount, RepaymentType returnType, out double decAccountingIRR, out double decBussinessIRR, out double decComapnyIRR, string strMorType, DataTable dtMoratoriumInput, string StrLob,string strSpliType,int intDelTenure)
        {
            decAccountingIRR = 0;
            decBussinessIRR = 0;
            decComapnyIRR = 0;
            bool blnAccountingIRR = false, blnBusinessIRR = false, blnCompanyIRR = false;
            DataTable dtAccountingIRR = new DataTable();
            DataTable dtFilterRows = new DataTable();
            IRRType Irrtype = IRRType.Accounting_IRR;
            DateTime DtAccStartDate;
            DataTable dtRepayStructure = new DataTable();
            decimal decFinAmout = 0, decRVAmt = 0;
            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            if (decResidualAmount.HasValue && decResidualAmount != 0)
                decRVAmt = Math.Round(decResidualAmount.Value);
        Start:
            #region IRRTable declaration and colunm addition
            DataTable dtIRRDetails = new DataTable();

            dtIRRDetails.Columns.Add("NoofDays", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentNo", typeof(int));
            dtIRRDetails.Columns.Add("FromDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("ToDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("From_Date");
            dtIRRDetails.Columns.Add("To_Date");
            dtIRRDetails.Columns.Add("Installment_Date");
            dtIRRDetails.Columns.Add("InstallmentDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("Amount", typeof(decimal));
            dtIRRDetails.Columns.Add("TransactionType");
            dtIRRDetails.Columns.Add("PrevBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("CurrBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("Slno", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("Charge", typeof(decimal));
            dtIRRDetails.Columns.Add("PrincipalAmount", typeof(decimal), "InstallmentAmount-Charge");
            dtIRRDetails.Columns.Add("IsMor");
            dtIRRDetails.Columns.Add("Rebate_Discount");
            dtIRRDetails.Columns.Add("Addi_Rebate_Discount");
            dtIRRDetails.Columns.Add("Cess_Amount");
            #endregion

            #region Only for Moratorium
            string MoraStDate = "";
            string MoraEndDate = "";
            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                MoraStDate = dtMoratoriumInput.Rows[0]["Fromdate"].ToString();// DateTime.Now.AddDays(61).ToString("MM/dd/yyyy");
                MoraEndDate = dtMoratoriumInput.Rows[0]["Todate"].ToString(); //Convert.ToDateTime(MoraStDate).AddDays(45).ToString("MM/dd/yyyy");
                strMorType = dtMoratoriumInput.Rows[0]["Moratoriumtype"].ToString().ToUpper();
            }
            #endregion

            //IRR Row
            DataRow drIRR;

            #region Adding First Row By default
            DateTime dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());//StringToDate(dtRepaymentSchedule.Rows[0]["FromDate"].ToString(), strDateFormat); ;
            DateTime dtEndDate = dtStartDate;

            #endregion

            #region Adding Cash Inflow Details
            foreach (DataRow drCashInflow in dtCashInflow.Rows)
            {
                ///CashFlow_Flag_ID 34 Hardcoded for Not to Include UMFC In IRR Calculation
                ///drCashInflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if ((drCashInflow[Irrtype.ToString()].ToString() == "True" || drCashInflow[Irrtype.ToString()].ToString() == "1")
                    && drCashInflow["CashFlow_Flag_ID"].ToString() != "34")
                {

                    drIRR = dtIRRDetails.NewRow();
                    drIRR["Amount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentAmount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentNo"] = "0";
                    drIRR["FromDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["InstallmentDate"] = drCashInflow["Date"].ToString();// StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["ToDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["NoofDays"] = "0";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            #endregion

            #region Adding Cash Outflow Details
            foreach (DataRow drCashOutflow in dtCashOutflow.Rows)
            {
                ///drCashOutflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drCashOutflow[Irrtype.ToString()].ToString() == "True" || drCashOutflow[Irrtype.ToString()].ToString() == "1")
                {
                    // considering Cash flow other than Margin amount (43 flag id for Margin payment)
                    if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "43")
                    {
                        //adding cashflow other than margin cashflows for Company IRR and Business IRR
                        decFinAmout = Math.Round(Convert.ToDecimal(drCashOutflow["Amount"].ToString()));
                        if (Irrtype != IRRType.Accounting_IRR)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                        }
                        //In case of Accounting IRR adding all outflows at application date itself Changed on 09-Mar-2011
                        else if (Irrtype == IRRType.Accounting_IRR)
                        {
                            //adding cashflow other than Delear payment for accounting IRR  (41 flag id for Dealer payment)
                            //if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "41")
                            //{
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["InstallmentDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["ToDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                            //}
                        }
                    }

                }

            }
            #endregion
            /***********************For Accounting IRR add finance amount as ouflow happebd on the document date itself*****************/
            #region Adding Finance Amount as first row for Accounting IRR
            //if (Irrtype == IRRType.Accounting_IRR)
            //{
            //    drIRR = dtIRRDetails.NewRow();
            //    drIRR["Amount"] = 0 - decPrincipleAmount;
            //    drIRR["InstallmentAmount"] = 0 - decPrincipleAmount;
            //    drIRR["InstallmentNo"] = "0";
            //    drIRR["FromDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["InstallmentDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["ToDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["Charge"] = "0.00";
            //    drIRR["PrevBalance"] = "0.00";
            //    drIRR["CurrBalance"] = decPrincipleAmount;//Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
            //    drIRR["NoofDays"] = "0";
            //    dtIRRDetails.Rows.Add(drIRR);
            //}
            int intLoop = 0;
            int intNoofInstallment = 0;
            #endregion

            #region Loop for adding Repayment Structure
            DateTime dtFromDate = new DateTime();
            DateTime dtForFBDate = new DateTime();
            int intNoLocalInstallment = 0;
            //dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());
            //installments
            DtAccStartDate = dtAppdate;
            DataRow[] drRepay_Array = dtRepaymentSchedule.Select("CashFlow_Flag_ID=23", "slno asc");
            foreach (DataRow drRepay in drRepay_Array)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"]);
                    dtForFBDate = Convert.ToDateTime(drRepay["ToDate"]);
                    dtFromDate = dtStartDate;
                    if (intLoop == 0)
                    {
                        DtAccStartDate = dtStartDate;
                    }
                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);//Modify on 22/11/2011

                        if (intLoopCount == intTotalInstallment - 1 && drRepay_Array.Length - 1 == intLoop)
                        {
                            drIRR["ToDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                            drIRR["InstallmentDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                        }

                        switch (returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (strTenureType.ToLower())
                                {
                                    case "months":
                                        strFrequency = "4";
                                        break;
                                    case "weeks":
                                        strFrequency = "2";
                                        break;
                                    case "days":
                                        strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                                drIRR["ToDate"] = dtFromDate;
                                drIRR["InstallmentDate"] = dtFromDate;
                                break;
                            default:
                                //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
                                //break;
                                if (strTimeval == "advancefbd" || strTimeval == "arrearsfbd")
                                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, dtForFBDate, intLoopCount + 1);
                                else
                                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1, DtAccStartDate);
                                break;
                        }
                        if (drRepay["Amount"].ToString().Trim() != "")
                            drIRR["Amount"] = drRepay["Amount"].ToString();
                        else
                            drIRR["Amount"] = "0";
                        drIRR["TransactionType"] = "Installment";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            //DtAccStartDate = dtAppdate;
            intLoop = 0;
            //Insurance 
            if (Irrtype != IRRType.Accounting_IRR)//as per the discussion with sudarsan sir . added this condition
            {
                DataRow[] drRepay_InsuranceArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID =25", "slno asc");
                foreach (DataRow drRepay in drRepay_InsuranceArray)
                {

                    if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                    {
                        int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                        int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                        int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                        //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                        dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                        dtFromDate = dtStartDate;

                        intNoLocalInstallment = intNoofInstallment;

                        for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                            drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                            drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);//Modify on 22/11/2011

                            if (intLoopCount == intTotalInstallment - 1 && drRepay_Array.Length - 1 == intLoop)
                            {
                                drIRR["ToDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                                drIRR["InstallmentDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                            }

                            switch (returnType)
                            {
                                case RepaymentType.FC:
                                case RepaymentType.WC:
                                case RepaymentType.TLE:
                                    switch (strTenureType.ToLower())
                                    {
                                        case "months":
                                            strFrequency = "4";
                                            break;
                                        case "weeks":
                                            strFrequency = "2";
                                            break;
                                        case "days":
                                            strFrequency = "0";
                                            break;
                                    }
                                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                    //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                                    drIRR["ToDate"] = dtFromDate;
                                    drIRR["InstallmentDate"] = dtFromDate;
                                    break;
                                default:
                                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
                                    //break;
                                    if (strTimeval == "advancefbd" || strTimeval == "arrearsfbd")
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, dtForFBDate, intLoopCount + 1);
                                    else
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1, DtAccStartDate);
                                    break;
                            }
                            drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                            drIRR["TransactionType"] = "Insurance";
                            drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                            intNoLocalInstallment = intNoLocalInstallment + 1;
                            drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                            //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = "0.00";
                            dtIRRDetails.Rows.Add(drIRR);

                        }

                        intNoofInstallment = intNoLocalInstallment;
                        intLoop++;
                    }

                }

                //AMF 
                //DtAccStartDate = dtAppdate;
                intLoop = 0;
                DataRow[] drRepay_AMFArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID =105", "slno asc");
                foreach (DataRow drRepay in drRepay_AMFArray)
                {
                    if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                    {
                        int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                        int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                        int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                        //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                        dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                        dtFromDate = dtStartDate;

                        intNoLocalInstallment = intNoofInstallment;

                        for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                            drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                            drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);//Modify on 22/11/2011

                            if (intLoopCount == intTotalInstallment - 1 && drRepay_AMFArray.Length - 1 == intLoop)
                            {
                                drIRR["ToDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                                drIRR["InstallmentDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                            }

                            switch (returnType)
                            {
                                case RepaymentType.FC:
                                case RepaymentType.WC:
                                case RepaymentType.TLE:
                                    switch (strTenureType.ToLower())
                                    {
                                        case "months":
                                            strFrequency = "4";
                                            break;
                                        case "weeks":
                                            strFrequency = "2";
                                            break;
                                        case "days":
                                            strFrequency = "0";
                                            break;
                                    }
                                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                    //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                                    drIRR["ToDate"] = dtFromDate;
                                    drIRR["InstallmentDate"] = dtFromDate;
                                    break;
                                default:
                                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
                                    //break;
                                    if (strTimeval == "advancefbd" || strTimeval == "arrearsfbd")
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, dtForFBDate, intLoopCount + 1);
                                    else
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1, DtAccStartDate);
                                    break;
                            }
                            drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                            drIRR["TransactionType"] = "AMF";
                            drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                            intNoLocalInstallment = intNoLocalInstallment + 1;
                            drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                            //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = "0.00";
                            dtIRRDetails.Rows.Add(drIRR);

                        }

                        intNoofInstallment = intNoLocalInstallment;
                        intLoop++;
                    }
                }                
                //Others
                //DtAccStartDate = dtAppdate;
                intLoop = 0;
                DataRow[] drRepay_OthersArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID <> 23 and CashFlow_Flag_ID <> 25 and CashFlow_Flag_ID <> 105", "slno asc");
                foreach (DataRow drRepay in drRepay_OthersArray)
                {
                    if (drRepay[Irrtype.ToString()].ToString() == "True" || drRepay[Irrtype.ToString()].ToString() == "1")
                    {
                        int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                        int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                        int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                        //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                        dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                        dtFromDate = dtStartDate;

                        intNoLocalInstallment = intNoofInstallment;

                        for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                            drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                            drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);//Modify on 22/11/2011

                            if (intLoopCount == intTotalInstallment - 1 && drRepay_OthersArray.Length - 1 == intLoop)
                            {
                                drIRR["ToDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                                drIRR["InstallmentDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                            }

                            switch (returnType)
                            {
                                case RepaymentType.FC:
                                case RepaymentType.WC:
                                case RepaymentType.TLE:
                                    switch (strTenureType.ToLower())
                                    {
                                        case "months":
                                            strFrequency = "4";
                                            break;
                                        case "weeks":
                                            strFrequency = "2";
                                            break;
                                        case "days":
                                            strFrequency = "0";
                                            break;
                                    }
                                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                    //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                                    drIRR["ToDate"] = dtFromDate;
                                    drIRR["InstallmentDate"] = dtFromDate;
                                    break;
                                default:
                                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1);
                                    //break;
                                    if (strTimeval == "advancefbd" || strTimeval == "arrearsfbd")
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, dtForFBDate, intLoopCount + 1);
                                    else
                                        dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount + 1, DtAccStartDate);
                                    break;
                            }
                            drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();

                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 start
                            //Inflow/Outflow/Both 1/2/3
                            switch (drRepay["Outflow"].ToString())
                            {
                                //case "1"://Inflow
                                //case "3"://Both
                                //    if (drRepay["Entity"].ToString() == "3")//Customer
                                //        drIRR["TransactionType"] = "Others";
                                //else//dealer
                                //        drIRR["TransactionType"] = "ET_IW";
                                //    break;
                                //case "2"://outflow
                                //    if (drRepay["Entity"].ToString() == "3")//Customer
                                //        drIRR["TransactionType"] = "CUS_OW";
                                //    else//dealer
                                //        drIRR["TransactionType"] = "ET_OW";
                                //    break;
                                default:
                                    drIRR["TransactionType"] = "Others";
                                    break;
                            }
                            //code added by saran in 2-Jul-2014 CR_SISSL12E046_018 end
                            if (drRepay["Amort"].ToString() == "2")//Amort
                                drIRR["InstallmentAmount"] = 0;
                            else
                                drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                            intNoLocalInstallment = intNoLocalInstallment + 1;
                            drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                            //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = "0.00";
                            dtIRRDetails.Rows.Add(drIRR);

                        }

                        intNoofInstallment = intNoLocalInstallment;
                        intLoop++;
                    }
                }
            }

            //Adding residual Value/Amount for IRR other than Accouning IRR Added on 24/06/2011
            //if (Irrtype != IRRType.Accounting_IRR)
            //Code added by saran on 06-Jan-2012 as per the sudarshan sir mail 
            if (Irrtype != IRRType.Accounting_IRR || StrLob.Trim().ToUpper() == "OL")
            {
                DateTime dtfrmdate = Convert.ToDateTime(dtRepaymentSchedule.Select("CashFlow_Flag_ID=23", "ToInstall desc")[0]["ToDate"].ToString());
                if (decResidualValue.HasValue && decResidualValue != 0)
                {
                    drIRR = dtIRRDetails.NewRow();
                    drIRR["FromDate"] = dtfrmdate;//dtFromDate.ToString(strDateFormat);
                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intNoLocalInstallment);
                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intNoLocalInstallment, Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString()));
                    drIRR["ToDate"] = dtfrmdate;//dtFromDate.ToString(strDateFormat);
                    drIRR["InstallmentDate"] = dtfrmdate;//.ToString(strDateFormat);
                    drIRR["Amount"] = dtRepaymentSchedule.Rows[0]["Amount"].ToString();
                    drIRR["InstallmentAmount"] = decPrincipleAmount * ((decResidualValue.Value) / 100);
                    intNoLocalInstallment = intNoLocalInstallment + 1;
                    drIRR["InstallmentNo"] = intNoLocalInstallment;//Convert.ToInt32(dtIRRDetails.Rows[]["InstallmentNo"].ToString())+1;
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = "0.00";
                    drIRR["TransactionType"] = "Others1";
                    dtIRRDetails.Rows.Add(drIRR);
                }
                else if (decResidualAmount.HasValue && decResidualAmount != 0)
                {
                    drIRR = dtIRRDetails.NewRow();
                    drIRR["FromDate"] = dtfrmdate;//dtFromDate.ToString(strDateFormat);
                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intNoLocalInstallment);
                    //dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intNoLocalInstallment, Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString()));
                    drIRR["ToDate"] = dtfrmdate;//dtFromDate.ToString(strDateFormat);
                    drIRR["InstallmentDate"] = dtfrmdate;//.ToString(strDateFormat);
                    drIRR["Amount"] = dtRepaymentSchedule.Rows[0]["Amount"].ToString();
                    //drIRR["InstallmentAmount"] = decPrincipleAmount + decResidualAmount.Value;
                    drIRR["InstallmentAmount"] = decResidualAmount.Value;
                    intNoLocalInstallment = intNoLocalInstallment + 1;
                    drIRR["InstallmentNo"] = intNoLocalInstallment;//Convert.ToInt32(dtIRRDetails.Rows[]["InstallmentNo"].ToString())+1;
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = "0.00";
                    drIRR["TransactionType"] = "Others1";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            int intEndDateCount = 0;

            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Slno"] = intEndDateCount + 1;
                intEndDateCount++;
            }

            // Change Start
            if (strSpliType == "PRIMARY")
            {
                DataRow[] resultCut = dtIRRDetails.Select("InstallmentNo=" + (intDelTenure).ToString()+"");
                dtFilterRows = resultCut.CopyToDataTable();                
                DataRow[] result = dtIRRDetails.Select("InstallmentNo>" + intDelTenure.ToString() + " AND InstallmentNo<=" + intTenure.ToString() + "");
                foreach (DataRow row in result)
                {
                    dtIRRDetails.Rows.Remove(row);
                }
                dtIRRDetails.AcceptChanges();

                DataRow[] updResult = dtIRRDetails.Select("TransactionType='Others1'");
                if (updResult.Length > 0)
                {
                    updResult[0]["FromDate"] = dtFilterRows.Rows[0]["FromDate"];
                    updResult[0]["ToDate"] = dtFilterRows.Rows[0]["ToDate"];
                    updResult[0]["InstallmentDate"] = dtFilterRows.Rows[0]["InstallmentDate"];
                }
                dtIRRDetails.AcceptChanges();
            }
            else if (strSpliType == "SECONDARY")
            {
                DataRow[] resultCut = dtIRRDetails.Select("InstallmentNo=" + (intDelTenure).ToString() + "");
                dtFilterRows = resultCut.CopyToDataTable();   
                DataRow[] result = dtIRRDetails.Select("InstallmentNo>0 AND InstallmentNo<=" + intDelTenure.ToString() + "");
                foreach (DataRow row in result)
                {
                    dtIRRDetails.Rows.Remove(row);
                }
                dtIRRDetails.AcceptChanges();
                foreach (DataColumn col in dtIRRDetails.Columns)
                {
                    if(col.Expression==null || col.Expression==string.Empty)
                        col.ReadOnly = false;
                }
                DataRow[] updResult = dtIRRDetails.Select("InstallmentNo=0");
                if (updResult.Length > 0)
                {
                    updResult[0]["FromDate"] = dtFilterRows.Rows[0]["FromDate"];
                    updResult[0]["ToDate"] = dtFilterRows.Rows[0]["ToDate"];
                    updResult[0]["InstallmentDate"] = dtFilterRows.Rows[0]["InstallmentDate"];
                    updResult[0]["CurrBalance"] = (decPrincipleAmount);
                    //updResult[0]["PrincipalAmount"] = (-1 * decPrincipleAmount);
                    updResult[0]["Amount"] = (-1 * decPrincipleAmount);
                    updResult[0]["InstallmentAmount"] = (-1 * decPrincipleAmount);
                }
                dtIRRDetails.AcceptChanges();
            }
            // Change End

            #region For Deffered Payments
            DataView dvIrrDetails = dtIRRDetails.DefaultView;
            dvIrrDetails.Sort = "ToDate,InstallmentNo,InstallmentAmount ASC";

            dtIRRDetails = dvIrrDetails.ToTable();

            bool blnBusIRRdone = false;
            intEndDateCount = 0;

            #endregion


            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                //drIrrRows["FromDate"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);

                //if (drIrrRows["TransactionType"].ToString() == "Installment")
                //{
                if (intEndDateCount != 0)
                    drIrrRows["FromDate"] = dtIRRDetails.Rows[intEndDateCount - 1]["ToDate"];
                else
                    drIrrRows["FromDate"] = dtAppdate;
                //}

                #region For Moratorium
                bool blnMor = false;
                if (strMorType != "")// == "INTEREST")
                {
                    if (Convert.ToDateTime(drIrrRows["FromDate"].ToString()) > Convert.ToDateTime(MoraStDate) && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) < Convert.ToDateTime(MoraEndDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraEndDate) && blnMor == false)
                    {
                        drIrrRows["FromDate"] = Convert.ToDateTime(dtMoratoriumInput.Rows[0]["ToDate"].ToString());
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        blnMor = true;
                    }
                    else if (Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                       && Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) < Convert.ToDateTime(MoraEndDate) &&
                        blnMor == false)
                    {
                        //drIrrRows["NoofDays"] = 0;
                        drIrrRows["IsMor"] = "0";
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                    else if (blnMor == false && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) <= Convert.ToDateTime(MoraStDate) &&
                        Convert.ToDateTime(drIrrRows["ToDate"].ToString()) >= Convert.ToDateTime(MoraEndDate))
                    //dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").Length > 0)
                    {
                        blnMor = true;
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = Convert.ToInt32(drIrrRows["NoofDays"].ToString()) - DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(MoraStDate), Convert.ToDateTime(MoraEndDate), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

                    }
                    else
                    {
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                }
                #endregion
                else
                {
                    //drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    /*For sakthi finanace*/
                    //Code Comment and added by saran on 17-feb-2012 start
                    // drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    switch (strIRRrest)
                    {
                        case "monthly":
                            if (DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) > 27)
                                drIrrRows["NoofDays"] = 30;
                            else
                                drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            break;
                        case "daily":
                            // Check Finance Amt and RV for OPC to keep all installment as Interest by Thalai on 20-Jul-2015 - Start
                            if (decRVAmt == decFinAmout && DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) == 0)
                                drIrrRows["NoofDays"] = 30;
                            else
                                drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            // Check Finance Amt and RV for OPC to keep all installment as Interest by Thalai on 20-Jul-2015 - End
                            break;
                        default:
                            drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            break;
                    }
                    //Code Comment and added by saran on 17-feb-2012 end
                }
                intEndDateCount++;
            }

            #endregion

            decimal decAmount = (decimal)dtIRRDetails.Compute("Sum(InstallmentAmount)", "NoofDays >0 ");
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Amount"] = decAmount;
            }

            double intMinVal = 0.00;
            //double intMaxVal = 2000.00;
            double intMaxVal = (double)AppReader.GetValue("IRRCutOff", typeof(double));

            #region Min val Max val Assiging part
        loopforIRR:
            if (intMaxVal == intMinVal)
            {
                throw new Exception("Incorrect cashflow details,cannot calculate IRR");
            }
            double decIRR = (intMaxVal + intMinVal) / 2;
            //decIRR = Math.Round(decIRR, 4);
            int intnoofdays = 0;
            switch (strIRRrest)
            {
                case "monthly":
                    intnoofdays = 1200;
                    break;
                case "daily":
                    intnoofdays = 36500;
                    break;
                case "yearly":
                    intnoofdays = 100;
                    break;
            }
            double decExp = 1 + (decIRR / intnoofdays);
            int intdtRowcount = 0;
            decimal dbPrevBalance = 0;
            decimal dbCharge = 0;
            //Double dbCurrPrevBalance;
            #endregion

            #region IRR Calculation Loop
            foreach (DataRow drIRRRow in dtIRRDetails.Rows)
            {
                if (intdtRowcount != 0)
                {
                    dbPrevBalance = Convert.ToDecimal(dtIRRDetails.Rows[intdtRowcount - 1]["CurrBalance"].ToString());
                }
                switch (strIRRrest)
                {
                    case "monthly":
                        //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"]) / 30))) - dbPrevBalance;
                        if (Convert.ToDouble(drIRRRow["NoofDays"]) < 30)
                            dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(1 + (decIRR / (365 * 100))), (Convert.ToDouble(drIRRRow["NoofDays"])))) - dbPrevBalance;
                        else
                            dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"]) / 30))) - dbPrevBalance;
                        break;
                    case "daily":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"])))) - dbPrevBalance;
                        break;
                    case "yearly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                        break;
                }
                // drIRRRow["PrevBalance"] = Math.Round(dbPrevBalance, 2) + Math.Round(dbCharge, 2);

                #region For Moratorium
                if (strMorType.ToUpper() == "INTEREST")
                {
                    if (drIRRRow["IsMor"].ToString() != "")
                    {
                        //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                        switch (strIRRrest)
                        {
                            case "monthly":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                break;
                            case "daily":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                break;
                        }

                    }

                    if (intdtRowcount != 0 && dbCharge >= Convert.ToDecimal(drIRRRow["InstallmentAmount"]))
                        dbCharge = Convert.ToDecimal(drIRRRow["InstallmentAmount"]);

                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);

                }
                #endregion
                else
                {
                    #region For Moratorium
                    if (strMorType.ToUpper() == "BOTH")
                    {
                        //if (drIRRRow["InstallmentAmount"].ToString() == "0")
                        //    dbCharge = 0;
                        if (drIRRRow["IsMor"].ToString() != "")
                        {
                            //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                            //drIRRRow["InstallmentAmount"] = 0;

                            switch (strIRRrest)
                            {
                                case "monthly":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                    break;
                                case "daily":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                    break;
                            }
                        }
                    }
                    //else
                    //{
                    //    if (drIRRRow["IsMor"].ToString() != "")
                    //    {
                    //        drIRRRow["InstallmentAmount"] = 0;
                    //    }
                    //}
                    #endregion

                    if (intdtRowcount != 0 && dbCharge >= Convert.ToDecimal(drIRRRow["InstallmentAmount"]))
                        dbCharge = Convert.ToDecimal(drIRRRow["InstallmentAmount"]);

                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);
                }

                intdtRowcount++;
            }

            dtIRRDetails.AcceptChanges();

            #endregion

            //*******Checking Whether to Continue the Loop or not********************// 
            if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) > decLimit)
            {
                intMaxVal = decIRR;
                goto loopforIRR;
            }
            else if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) < (0 - decLimit))
            {
                intMinVal = decIRR;
                goto loopforIRR;
            }
            //*******End of loop Checking Whether to Continue the Loop or not********************//
            else
            {
                //If IRR is calculated then return the datatable
                DataTable dtOthers = new DataTable();
                DataTable dtInsurance = new DataTable();
                string strLogIrr, strIRRPath = string.Empty;
                try //Try Catch block if No LogIRR is defined in config
                {
                    strLogIrr = (string)AppReader.GetValue("LogIRR", typeof(string));
                    strIRRPath = (string)AppReader.GetValue("FilePath", typeof(string));
                }
                catch
                {
                    strLogIrr = "No";
                }
                dtIRRDetails.TableName = Irrtype.ToString();
                if (strLogIrr == "Yes")
                {
                    try
                    {
                        if (strIRRPath == string.Empty)
                            strIRRPath = System.Environment.CurrentDirectory + "\\Data\\IRR";
                        if (!Directory.Exists(strIRRPath))
                        {
                            Directory.CreateDirectory(strIRRPath);
                        }
                        dtIRRDetails.WriteXml(Path.Combine(strIRRPath, dtIRRDetails.TableName + System.Guid.NewGuid().ToString() + ".xls"));
                    }
                    catch (Exception ex)
                    {
                        ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    }
                }
                double decIRRCutoff = 0;//Try Catch block if No IrrCutoff is defined in config
                try
                {
                    decIRRCutoff = (double)AppReader.GetValue("IRRCutOff", typeof(double));
                }
                catch
                {

                }
                if (decIRRCutoff != 0)
                {
                    if (decIRR > decIRRCutoff)
                    {
                        throw new Exception("Unrealistic " + Irrtype.ToString().Replace("_", " ") + ", change the parameters to arrive at a nominal IRR");
                    }
                }
                switch (Irrtype)
                {
                    case IRRType.Accounting_IRR:
                        decAccountingIRR = Math.Round(decIRR, 4);
                        dtAccountingIRR = dtIRRDetails.Copy();
                        Irrtype = IRRType.Business_IRR;
                        blnAccountingIRR = true;
                        break;
                    case IRRType.Business_IRR:
                        decBussinessIRR = Math.Round(decIRR, 4);
                        dtRepayStructure = dtIRRDetails.Clone();
                        if (dtIRRDetails.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = dtIRRDetails.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (dtIRRDetails.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtIRRDetails.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        if (dtIRRDetails.Select("TransactionType='AMF'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drAMF = dtIRRDetails.Select("TransactionType='AMF'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drAMF)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 start
                        //if (dtIRRDetails.Select("TransactionType='ET_IW'").AsEnumerable().Count() > 0)
                        //{
                        //    DataRow[] drOthers = dtIRRDetails.Select("TransactionType='ET_IW'");//.CopyToDataTable<DataRow>();
                        //    foreach (DataRow drOtherRow in drOthers)
                        //    {
                        //        drOtherRow["InstallmentNo"] = 0;
                        //        drOtherRow["Slno"] = 0;
                        //        drOtherRow.AcceptChanges();
                        //        dtRepayStructure.ImportRow(drOtherRow);
                        //    }
                        //}
                        //if (dtIRRDetails.Select("TransactionType='ET_OW'").AsEnumerable().Count() > 0)
                        //{
                        //    DataRow[] drOthers = dtIRRDetails.Select("TransactionType='ET_OW'");//.CopyToDataTable<DataRow>();
                        //    foreach (DataRow drOtherRow in drOthers)
                        //    {
                        //        drOtherRow["InstallmentNo"] = 0;
                        //        drOtherRow["Slno"] = 0;
                        //        drOtherRow.AcceptChanges();
                        //        dtRepayStructure.ImportRow(drOtherRow);
                        //    }
                        //}
                        //if (dtIRRDetails.Select("TransactionType='CUS_OW'").AsEnumerable().Count() > 0)
                        //{
                        //    DataRow[] drOthers = dtIRRDetails.Select("TransactionType='CUS_OW'");//.CopyToDataTable<DataRow>();
                        //    foreach (DataRow drOtherRow in drOthers)
                        //    {
                        //        drOtherRow["InstallmentNo"] = 0;
                        //        drOtherRow["Slno"] = 0;
                        //        drOtherRow.AcceptChanges();
                        //        dtRepayStructure.ImportRow(drOtherRow);
                        //    }
                        //}
                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 end
                        Irrtype = IRRType.Company_IRR;
                        blnBusinessIRR = true;
                        break;
                    case IRRType.Company_IRR:
                        decComapnyIRR = Math.Round(decIRR, 4);

                        if (dtIRRDetails.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = dtIRRDetails.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (dtIRRDetails.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtIRRDetails.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        if (dtIRRDetails.Select("TransactionType='AMF'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drAMF = dtIRRDetails.Select("TransactionType='AMF'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drAMF)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 start
                        //if (dtIRRDetails.Select("TransactionType='ET_IW'").AsEnumerable().Count() > 0)
                        //{
                        //    DataRow[] drOthers = dtIRRDetails.Select("TransactionType='ET_IW'");//.CopyToDataTable<DataRow>();
                        //    foreach (DataRow drOtherRow in drOthers)
                        //    {
                        //        drOtherRow["InstallmentNo"] = 0;
                        //        drOtherRow["Slno"] = 0;
                        //        drOtherRow.AcceptChanges();
                        //        dtRepayStructure.ImportRow(drOtherRow);
                        //    }
                        //}
                        //if (dtIRRDetails.Select("TransactionType='ET_OW'").AsEnumerable().Count() > 0)
                        //{
                        //    DataRow[] drOthers = dtIRRDetails.Select("TransactionType='ET_OW'");//.CopyToDataTable<DataRow>();
                        //    foreach (DataRow drOtherRow in drOthers)
                        //    {
                        //        drOtherRow["InstallmentNo"] = 0;
                        //        drOtherRow["Slno"] = 0;
                        //        drOtherRow.AcceptChanges();
                        //        dtRepayStructure.ImportRow(drOtherRow);
                        //    }
                        //}
                        //if (dtIRRDetails.Select("TransactionType='CUS_OW'").AsEnumerable().Count() > 0)
                        //{
                        //    DataRow[] drOthers = dtIRRDetails.Select("TransactionType='CUS_OW'");//.CopyToDataTable<DataRow>();
                        //    foreach (DataRow drOtherRow in drOthers)
                        //    {
                        //        drOtherRow["InstallmentNo"] = 0;
                        //        drOtherRow["Slno"] = 0;
                        //        drOtherRow.AcceptChanges();
                        //        dtRepayStructure.ImportRow(drOtherRow);
                        //    }
                        //}
                        //Added by saran on 2-Jul-2014 for CR_SISSL12E046_018 end
                        blnCompanyIRR = true;
                        break;
                }
                if (!blnAccountingIRR || !blnBusinessIRR || !blnCompanyIRR)
                {
                    goto Start;
                }

                DataView dvRepayStructure = dtRepayStructure.DefaultView;

                //Thangam M on 24/Oct/2012 for Other amount issue
                //string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType", "PrevBalance" };
                //End here

                string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType" };

                DataTable dtRepayStruct = dvRepayStructure.ToTable(true, strColumn);
                if (!dtAccountingIRR.Columns.Contains("Insurance"))
                {
                    dtAccountingIRR.Columns.Add("Insurance", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("Others"))
                {
                    dtAccountingIRR.Columns.Add("Others", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW start
                if (!dtAccountingIRR.Columns.Contains("Tax"))
                {
                    dtAccountingIRR.Columns.Add("Tax", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("ServiceTax"))
                {
                    dtAccountingIRR.Columns.Add("ServiceTax", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("AMF"))
                {
                    dtAccountingIRR.Columns.Add("AMF", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("AMF_Principal"))
                {
                    dtAccountingIRR.Columns.Add("AMF_Principal", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("AMF_Charges"))
                {
                    dtAccountingIRR.Columns.Add("AMF_Charges", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW end

                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                //if (!dtAccountingIRR.Columns.Contains("ET_IW"))
                //{
                //    dtAccountingIRR.Columns.Add("ET_IW", typeof(decimal));
                //}
                //if (!dtAccountingIRR.Columns.Contains("ET_OW"))
                //{
                //    dtAccountingIRR.Columns.Add("ET_OW", typeof(decimal));
                //}
                //if (!dtAccountingIRR.Columns.Contains("CUS_OW"))
                //{
                //    dtAccountingIRR.Columns.Add("CUS_OW", typeof(decimal));
                //}
                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end

                if (dtRepayStruct.Rows.Count > 0)
                {
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Insurance");
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Others");
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "AMF");
                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                    //FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "ET_IW");
                    //FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "ET_OW");
                    //FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "CUS_OW");
                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end
                }
                //Filtering only installments as it will used to arrive repayment structure.

                dtAccountingIRR.DefaultView.RowFilter = "TransactionType ='Installment'";
                //dtAccountingIRR.Columns["PrincipalAmount"].ReadOnly = false;

                //int i = 0;

                foreach (DataRow drIrrRows in dtAccountingIRR.Rows)
                {
                    drIrrRows["InstallmentDate"] = drIrrRows["InstallmentDate"];
                    drIrrRows["Installment_Date"] = Convert.ToDateTime(drIrrRows["InstallmentDate"]).ToString(strDateFormat);
                    drIrrRows["From_Date"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);
                    drIrrRows["To_Date"] = Convert.ToDateTime(drIrrRows["ToDate"]).ToString(strDateFormat);
                    drIrrRows["Charge"] = Convert.ToDecimal(((decimal)drIrrRows["Charge"]).ToString("0.00"));

                    drIrrRows["InstallmentAmount"] = Convert.ToDecimal(((decimal)drIrrRows["InstallmentAmount"]).ToString("0.00"));
                    if (drIrrRows["Insurance"].ToString() != "")
                    {
                        drIrrRows["Insurance"] = Convert.ToDecimal(((decimal)drIrrRows["Insurance"]).ToString("0.00"));
                    }
                    if (drIrrRows["Others"].ToString() != "")
                    {
                        drIrrRows["Others"] = Convert.ToDecimal(((decimal)drIrrRows["Others"]).ToString("0.00"));
                    }
                    if (drIrrRows["AMF"].ToString() != "")
                    {
                        drIrrRows["AMF"] = Convert.ToDecimal(((decimal)drIrrRows["AMF"]).ToString("0.00"));
                    }
                    if (drIrrRows["ServiceTax"].ToString() != "")
                    {
                        drIrrRows["ServiceTax"] = Convert.ToDecimal(((decimal)drIrrRows["ServiceTax"]).ToString("0.00"));
                    }
                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                    //if (drIrrRows["ET_IW"].ToString() != "")
                    //{
                    //    drIrrRows["ET_IW"] = Convert.ToDecimal(((decimal)drIrrRows["ET_IW"]).ToString("0.00"));
                    //}
                    //if (drIrrRows["ET_OW"].ToString() != "")
                    //{
                    //    drIrrRows["ET_OW"] = Convert.ToDecimal(((decimal)drIrrRows["ET_OW"]).ToString("0.00"));
                    //}
                    //if (drIrrRows["CUS_OW"].ToString() != "")
                    //{
                    //    drIrrRows["CUS_OW"] = Convert.ToDecimal(((decimal)drIrrRows["CUS_OW"]).ToString("0.00"));
                    //}

                    //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end

                    // Code Added By Chandru K On 12 Mar 2015

                    //if (i != 0 && Convert.ToDecimal(drIrrRows["InstallmentAmount"]) < Convert.ToDecimal(drIrrRows["Charge"]))
                    //    drIrrRows["Charge"] = drIrrRows["InstallmentAmount"];
                    //i++;

                    // Code End
                }

                if (dtAccountingIRR.Columns.Contains("PrincipalAmount"))
                {
                    dtAccountingIRR.Columns["PrincipalAmount"].Expression = "InstallmentAmount-Charge";
                }
                return dtAccountingIRR.DefaultView.ToTable("Repayment Structure");
            }

        }

        /// <summary>
        /// Method for Specific Revision Irr calculation
        /// </summary>
        /// <param name="dtRepaymentSchedule"></param>
        /// <param name="dtCashInflow"></param>
        /// <param name="dtCashOutflow"></param>
        /// <param name="strFrequency"></param>
        /// <param name="intTenure"></param>
        /// <param name="strTenureType"></param>
        /// <param name="strDateFormat"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decRateofInt"></param>
        /// <param name="strIRRrest"></param>
        /// <param name="strTimeval"></param>
        /// <param name="strInstallmentType"></param>
        /// <param name="decLimit"></param>
        /// <param name="decCTR"></param>
        /// <param name="decPLR"></param>
        /// <param name="dtAppdate"></param>
        /// <param name="decResidualValue"></param>
        /// <param name="decResidualAmount"></param>
        /// <param name="returnType"></param>
        /// <param name="decAccountingIRR"></param>
        /// <param name="decBussinessIRR"></param>
        /// <param name="decComapnyIRR"></param>
        /// <param name="dtAdditionalCashflow"> Addtional Cashflows for specific revision</param>
        /// <returns></returns>
        public DataTable FunPubGenerateRepaymentStructure(DataTable dtRepaymentSchedule, DataTable dtCashInflow, DataTable dtCashOutflow, string strFrequency, int intTenure, string strTenureType, string strDateFormat, decimal decPrincipleAmount, decimal decRateofInt, string strIRRrest, string strTimeval, string strInstallmentType, decimal decLimit, decimal decCTR, decimal decPLR, DateTime dtAppdate, decimal? decResidualValue, decimal? decResidualAmount, RepaymentType returnType, out double decAccountingIRR, out double decBussinessIRR, out double decComapnyIRR, DataTable dtAdditionalCashflow, string StrLob)
        {
            decAccountingIRR = 0;
            decBussinessIRR = 0;
            decComapnyIRR = 0;
            bool blnAccountingIRR = false, blnBusinessIRR = false, blnCompanyIRR = false;
            DataTable dtAccountingIRR = new DataTable();
            IRRType Irrtype = IRRType.Accounting_IRR;
            DataTable dtRepayStructure = new DataTable();
        Start:
            #region IRRTable declaration and colunm addition
            DataTable dtIRRDetails = new DataTable();

            dtIRRDetails.Columns.Add("NoofDays", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentNo", typeof(int));
            dtIRRDetails.Columns.Add("FromDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("ToDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("From_Date");
            dtIRRDetails.Columns.Add("To_Date");
            dtIRRDetails.Columns.Add("Installment_Date");
            dtIRRDetails.Columns.Add("InstallmentDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("InstallmentAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("Amount", typeof(decimal));
            dtIRRDetails.Columns.Add("TransactionType");
            dtIRRDetails.Columns.Add("Charge", typeof(decimal));
            dtIRRDetails.Columns.Add("PrevBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("CurrBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("Slno", typeof(int));
            dtIRRDetails.Columns.Add("PrincipalAmount", typeof(decimal), "InstallmentAmount-Charge");
            #endregion
            //IRR Row
            DataRow drIRR;

            #region Adding First Row By default
            DateTime dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());//StringToDate(dtRepaymentSchedule.Rows[0]["FromDate"].ToString(), strDateFormat); ;
            DateTime dtEndDate = dtStartDate;

            #endregion

            #region Adding Cash Inflow Details
            foreach (DataRow drCashInflow in dtCashInflow.Rows)
            {
                ///CashFlow_Flag_ID 34 Hardcoded for Not to Include UMFC In IRR Calculation
                ///drCashInflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drCashInflow[Irrtype.ToString()].ToString() == "True" && drCashInflow["CashFlow_Flag_ID"].ToString() != "34")
                {

                    drIRR = dtIRRDetails.NewRow();
                    drIRR["Amount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentAmount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentNo"] = "0";
                    drIRR["FromDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["InstallmentDate"] = drCashInflow["Date"].ToString();// StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["ToDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["NoofDays"] = "0";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            #endregion

            #region Adding Cash Outflow Details
            foreach (DataRow drCashOutflow in dtCashOutflow.Rows)
            {
                ///drCashOutflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drCashOutflow[Irrtype.ToString()].ToString() == "True")
                {
                    // considering Cash flow other than Margin amount (43 flag id for Margin payment)
                    if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "43")
                    {
                        //adding cashflow other than margin cashflows for Company IRR and Business IRR
                        if (Irrtype != IRRType.Accounting_IRR)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                        }
                        //In case of Accounting IRR adding all outflows at application date itself Changed on 09-Mar-2011
                        else if (Irrtype == IRRType.Accounting_IRR)
                        {
                            //adding cashflow other than Delear payment for accounting IRR  (41 flag id for Dealer payment)
                            //if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "41")
                            //{
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["InstallmentDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["ToDate"] = dtAppdate;//drCashOutflow["Date"].ToString();
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                            //}
                        }
                    }

                }

            }
            #endregion

            #region Adding AdditionalCashflow Details Specially desined for Specific Revision On 22/June/2011
            foreach (DataRow drAdditionalCashflow in dtAdditionalCashflow.Rows)
            {
                ///drdrAdditionalCashflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drAdditionalCashflow[Irrtype.ToString()].ToString() == "True")
                {

                    drIRR = dtIRRDetails.NewRow();
                    drIRR["Amount"] = 0 - Convert.ToDouble(drAdditionalCashflow["Amount"].ToString());
                    drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drAdditionalCashflow["Amount"].ToString());
                    drIRR["InstallmentNo"] = "0";
                    drIRR["FromDate"] = drAdditionalCashflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["InstallmentDate"] = drAdditionalCashflow["Date"].ToString();// StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["ToDate"] = drAdditionalCashflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = Convert.ToDouble(drAdditionalCashflow["Amount"].ToString());
                    drIRR["NoofDays"] = "0";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            #endregion

            /***********************For Accounting IRR add finance amount as ouflow happebd on the document date itself*****************/
            #region Adding Finance Amount as first row for Accounting IRR
            //if (Irrtype == IRRType.Accounting_IRR)
            //{
            //    drIRR = dtIRRDetails.NewRow();
            //    drIRR["Amount"] = 0 - decPrincipleAmount;
            //    drIRR["InstallmentAmount"] = 0 - decPrincipleAmount;
            //    drIRR["InstallmentNo"] = "0";
            //    drIRR["FromDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["InstallmentDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["ToDate"] = dtAppdate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
            //    drIRR["Charge"] = "0.00";
            //    drIRR["PrevBalance"] = "0.00";
            //    drIRR["CurrBalance"] = decPrincipleAmount;//Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
            //    drIRR["NoofDays"] = "0";
            //    dtIRRDetails.Rows.Add(drIRR);
            //}
            int intLoop = 0;
            int intNoofInstallment = 0;
            #endregion

            #region Loop for adding Repayment Structure
            DateTime dtFromDate = new DateTime();
            int intNoLocalInstallment = 0;
            dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());
            //installments
            DataRow[] drRepay_Array = dtRepaymentSchedule.Select("CashFlow_Flag_ID=23", "slno asc");
            dtFromDate = dtStartDate;// added on 21 Dec....By Rao.
            foreach (DataRow drRepay in drRepay_Array)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    //dtFromDate = dtStartDate;  // Commented on 21 Dec.... By Rao

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);

                        if (intLoopCount == intTotalInstallment - 1 && drRepay_Array.Length - 1 == intLoop)
                        {
                            drIRR["ToDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                            drIRR["InstallmentDate"] = Convert.ToDateTime(drRepay["ToDate"]);
                        }

                        switch (returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (strTenureType.ToLower())
                                {
                                    case "months":
                                        strFrequency = "4";
                                        break;
                                    case "weeks":
                                        strFrequency = "2";
                                        break;
                                    case "days":
                                        strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                //Modified for WC/Factoring Not working ass reported by Prabhu on 29/07/2011
                                drIRR["ToDate"] = dtFromDate;
                                drIRR["InstallmentDate"] = dtFromDate;
                                break;
                            default:
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intNoLocalInstallment + 1); // Added Installment +1 On 21Dec By Rao.
                                break;
                        }



                        drIRR["Amount"] = drRepay["Amount"].ToString();
                        drIRR["TransactionType"] = "Installment";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            //Insurance 
            DataRow[] drRepay_InsuranceArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID =25", "slno asc");
            foreach (DataRow drRepay in drRepay_InsuranceArray)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                    dtFromDate = dtStartDate;

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        switch (returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (strTenureType.ToLower())
                                {
                                    case "months":
                                        strFrequency = "4";
                                        break;
                                    case "weeks":
                                        strFrequency = "2";
                                        break;
                                    case "days":
                                        strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                break;
                            default:
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount);
                                break;
                        }

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                        drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                        drIRR["TransactionType"] = "Insurance";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            //Others
            DataRow[] drRepay_OthersArray = dtRepaymentSchedule.Select("CashFlow_Flag_ID <> 23 and CashFlow_Flag_ID <> 25", "slno asc");
            foreach (DataRow drRepay in drRepay_OthersArray)
            {
                if (drRepay[Irrtype.ToString()].ToString() == "True")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;

                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                    dtFromDate = dtStartDate;

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        switch (returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (strTenureType.ToLower())
                                {
                                    case "months":
                                        strFrequency = "4";
                                        break;
                                    case "weeks":
                                        strFrequency = "2";
                                        break;
                                    case "days":
                                        strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intTenure);
                                break;
                            default:
                                dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount);
                                break;
                        }

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                        drIRR["Amount"] = drRepay["Amount"];//dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                        drIRR["TransactionType"] = "Others";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"]; //dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            //Adding residual Value/Amount for IRR other than Accouning IRR Added on 24/06/2011
            //if (Irrtype != IRRType.Accounting_IRR)
            //Code added by saran on 06-Jan-2012 as per the sudarshan sir mail 
            if (Irrtype != IRRType.Accounting_IRR || StrLob.Trim().ToUpper() == "OL")
            {
                if (decResidualValue.HasValue && decResidualValue != 0)
                {
                    drIRR = dtIRRDetails.NewRow();
                    drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intNoLocalInstallment);
                    drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                    drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                    drIRR["Amount"] = dtRepaymentSchedule.Rows[0]["Amount"].ToString();
                    drIRR["InstallmentAmount"] = decPrincipleAmount * ((decResidualValue.Value) / 100);
                    intNoLocalInstallment = intNoLocalInstallment + 1;
                    drIRR["InstallmentNo"] = intNoLocalInstallment;//Convert.ToInt32(dtIRRDetails.Rows[]["InstallmentNo"].ToString())+1;
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = "0.00";
                    drIRR["TransactionType"] = "Others";
                    dtIRRDetails.Rows.Add(drIRR);
                }
                else if (decResidualAmount.HasValue && decResidualAmount != 0)
                {
                    drIRR = dtIRRDetails.NewRow();
                    drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intNoLocalInstallment);
                    drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                    drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                    drIRR["Amount"] = dtRepaymentSchedule.Rows[0]["Amount"].ToString();
                    //drIRR["InstallmentAmount"] = decPrincipleAmount + decResidualAmount.Value;
                    drIRR["InstallmentAmount"] = decResidualAmount.Value;
                    intNoLocalInstallment = intNoLocalInstallment + 1;
                    drIRR["InstallmentNo"] = intNoLocalInstallment;//Convert.ToInt32(dtIRRDetails.Rows[]["InstallmentNo"].ToString())+1;
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = "0.00";
                    drIRR["TransactionType"] = "Others";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }

            int intEndDateCount = 0;

            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {

                drIrrRows["Slno"] = intEndDateCount + 1;
                intEndDateCount++;
            }

            #region For Deffered Payments
            DataView dvIrrDetails = dtIRRDetails.DefaultView;
            dvIrrDetails.Sort = "ToDate,InstallmentNo,InstallmentAmount ASC";

            dtIRRDetails = dvIrrDetails.ToTable();

            bool blnBusIRRdone = false;
            intEndDateCount = 0;

            #endregion

            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                // drIrrRows["FromDate"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);
                //if (intEndDateCount != 0)
                //    drIrrRows["FromDate"] = dtIRRDetails.Rows[intEndDateCount - 1]["ToDate"];


                if (intEndDateCount != 0)
                    drIrrRows["FromDate"] = dtIRRDetails.Rows[intEndDateCount - 1]["ToDate"];
                else
                    drIrrRows["FromDate"] = dtAppdate;

                drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

                /*
                    //Code Comment and added by saran on 17-feb-2012 start for sakthi finance
                    // drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    switch (strIRRrest)
                    {
                        case "monthly":
                            drIrrRows["NoofDays"] = 30;
                            break;
                        case "daily":
                            drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            break;
                    }
                    //Code Comment and added by saran on 17-feb-2012 end for sakthi finance*/
                intEndDateCount++;
            }

            #endregion

            decimal decAmount = (decimal)dtIRRDetails.Compute("Sum(InstallmentAmount)", "NoofDays >0 ");
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Amount"] = decAmount;
            }
            double intMinVal = 0.00;
            double intMaxVal = 1000.00;

            #region Min val Max val Assiging part
        loopforIRR:
            if (intMaxVal == intMinVal)
            {
                throw new Exception("Incorrect cashflow details,cannot calculate IRR");
            }
            double decIRR = (intMaxVal + intMinVal) / 2;
            //decIRR = Math.Round(decIRR, 4);
            int intnoofdays = 0;
            switch (strIRRrest)
            {
                case "monthly":
                    intnoofdays = 1200;
                    break;
                case "daily":
                    intnoofdays = 36500;
                    break;
                case "yearly":
                    intnoofdays = 100;
                    break;
            }
            double decExp = 1 + (decIRR / intnoofdays);
            int intdtRowcount = 0;
            decimal dbPrevBalance = 0;
            decimal dbCharge = 0;
            //Double dbCurrPrevBalance;
            #endregion

            #region IRR Calculation Loop
            foreach (DataRow drIRRRow in dtIRRDetails.Rows)
            {
                if (intdtRowcount != 0)
                {
                    dbPrevBalance = Convert.ToDecimal(dtIRRDetails.Rows[intdtRowcount - 1]["CurrBalance"].ToString());
                }
                switch (strIRRrest)
                {
                    case "monthly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"]) / 30))) - dbPrevBalance;
                        break;
                    case "daily":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"])))) - dbPrevBalance;
                        break;
                    case "yearly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), ((Convert.ToDouble(drIRRRow["NoofDays"])) / 365))) - dbPrevBalance;
                        break;
                }
                // drIRRRow["PrevBalance"] = Math.Round(dbPrevBalance, 2) + Math.Round(dbCharge, 2);
                drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);
                drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);
                intdtRowcount++;
            }

            dtIRRDetails.AcceptChanges();

            #endregion

            //*******Checking Whether to Continue the Loop or not********************// 
            if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) > decLimit)
            {
                intMaxVal = decIRR;
                goto loopforIRR;
            }
            else if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) < (0 - decLimit))
            {
                intMinVal = decIRR;
                goto loopforIRR;
            }
            //*******End of loop Checking Whether to Continue the Loop or not********************//
            else
            {
                //If IRR is calculated then return the datatable
                DataTable dtOthers = new DataTable();
                DataTable dtInsurance = new DataTable();
                string strLogIrr, strIRRPath = string.Empty;
                System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();

                try //Try Catch block if No LogIRR is defined in config
                {
                    if (dtIRRDetails.Columns.Contains("PrincipalAmount"))
                        dtIRRDetails.Columns["PrincipalAmount"].Expression = "InstallmentAmount-Charge";
                    strLogIrr = (string)AppReader.GetValue("LogIRR", typeof(string));
                    strIRRPath = (string)AppReader.GetValue("FilePath", typeof(string));
                }
                catch
                {
                    strLogIrr = "No";
                }
                dtIRRDetails.TableName = Irrtype.ToString();
                if (strLogIrr == "Yes")
                {
                    try
                    {
                        if (strIRRPath == string.Empty)
                            strIRRPath = System.Environment.CurrentDirectory + "\\Data\\IRR";
                        if (!Directory.Exists(strIRRPath))
                        {
                            Directory.CreateDirectory(strIRRPath);
                        }
                        dtIRRDetails.WriteXml(Path.Combine(strIRRPath, dtIRRDetails.TableName + System.Guid.NewGuid().ToString() + ".xls"));
                    }
                    catch (Exception ex)
                    {
                        ClsPubCommErrorLog.CustomErrorRoutine(ex);
                    }
                }

                double decIRRCutoff = 0;//Try Catch block if No IrrCutoff is defined in config
                try
                {
                    decIRRCutoff = (double)AppReader.GetValue("IRRCutOff", typeof(double));
                }
                catch
                {

                }
                if (decIRR > decIRRCutoff)
                {
                    throw new Exception("Unrealistic " + Irrtype.ToString().Replace("_", " ") + ", change the parameters to arrive at a nominal IRR");
                }
                switch (Irrtype)
                {
                    case IRRType.Accounting_IRR:
                        decAccountingIRR = Math.Round(decIRR, 4);
                        dtAccountingIRR = dtIRRDetails.Copy();
                        Irrtype = IRRType.Business_IRR;
                        blnAccountingIRR = true;
                        break;
                    case IRRType.Business_IRR:
                        decBussinessIRR = Math.Round(decIRR, 4);
                        dtRepayStructure = dtIRRDetails.Clone();
                        if (dtIRRDetails.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = dtIRRDetails.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (dtIRRDetails.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtIRRDetails.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        Irrtype = IRRType.Company_IRR;
                        blnBusinessIRR = true;
                        break;
                    case IRRType.Company_IRR:
                        decComapnyIRR = Math.Round(decIRR, 4);

                        if (dtIRRDetails.Select("TransactionType='Insurance'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drInsurance = dtIRRDetails.Select("TransactionType='Insurance'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drInsuranceRow in drInsurance)
                            {
                                drInsuranceRow["InstallmentNo"] = 0;
                                drInsuranceRow["Slno"] = 0;
                                drInsuranceRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drInsuranceRow);
                            }
                        }
                        if (dtIRRDetails.Select("TransactionType='Others'").AsEnumerable().Count() > 0)
                        {
                            DataRow[] drOthers = dtIRRDetails.Select("TransactionType='Others'");//.CopyToDataTable<DataRow>();
                            foreach (DataRow drOtherRow in drOthers)
                            {
                                drOtherRow["InstallmentNo"] = 0;
                                drOtherRow["Slno"] = 0;
                                drOtherRow.AcceptChanges();
                                dtRepayStructure.ImportRow(drOtherRow);
                            }
                        }
                        blnCompanyIRR = true;
                        break;
                }
                if (!blnAccountingIRR || !blnBusinessIRR || !blnCompanyIRR)
                {
                    goto Start;
                }

                DataView dvRepayStructure = dtRepayStructure.DefaultView;
                //string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType" };

                //Thangam M on 24/Oct/2012 for Other amount issue
                string[] strColumn = { "FromDate", "ToDate", "InstallmentDate", "InstallmentAmount", "TransactionType", "PrevBalance" };
                //End here

                DataTable dtRepayStruct = dvRepayStructure.ToTable(true, strColumn);
                if (!dtAccountingIRR.Columns.Contains("Insurance"))
                {
                    dtAccountingIRR.Columns.Add("Insurance", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("Others"))
                {
                    dtAccountingIRR.Columns.Add("Others", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW start
                if (!dtAccountingIRR.Columns.Contains("Tax"))
                {
                    dtAccountingIRR.Columns.Add("Tax", typeof(decimal));
                }
                //Added by saran on 2-Aug-2013 for BW end


                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 start
                if (!dtAccountingIRR.Columns.Contains("ET_IW"))
                {
                    dtAccountingIRR.Columns.Add("ET_IW", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("ET_OW"))
                {
                    dtAccountingIRR.Columns.Add("ET_OW", typeof(decimal));
                }
                if (!dtAccountingIRR.Columns.Contains("CUS_OW"))
                {
                    dtAccountingIRR.Columns.Add("CUS_OW", typeof(decimal));
                }
                //Added by saran on 1-Jul-2014 for CR_SISSL12E046_018 end



                if (dtRepayStruct.Rows.Count > 0)
                {
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Insurance");
                    FunPriUpdateRepaymentStructure(dtAccountingIRR, dtRepayStruct, "Others");
                }

                //Filtering only installments as it will used to arrive repayment structure.

                dtAccountingIRR.DefaultView.RowFilter = "TransactionType ='Installment'";
                //dtAccountingIRR.Columns["PrincipalAmount"].ReadOnly = false;
                if (dtAccountingIRR.Columns.Contains("PrincipalAmount"))
                    dtAccountingIRR.Columns["PrincipalAmount"].Expression = "InstallmentAmount-Charge";

                foreach (DataRow drIrrRows in dtAccountingIRR.Rows)
                {
                    drIrrRows["InstallmentDate"] = drIrrRows["InstallmentDate"];
                    drIrrRows["Installment_Date"] = Convert.ToDateTime(drIrrRows["InstallmentDate"]).ToString(strDateFormat);
                    drIrrRows["From_Date"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);
                    drIrrRows["To_Date"] = Convert.ToDateTime(drIrrRows["ToDate"]).ToString(strDateFormat);

                    drIrrRows["Charge"] = Convert.ToDecimal(((decimal)drIrrRows["Charge"]).ToString("0.00"));
                    drIrrRows["InstallmentAmount"] = Convert.ToDecimal(((decimal)drIrrRows["InstallmentAmount"]).ToString("0.00"));
                    if (drIrrRows["Insurance"].ToString() != "")
                    {
                        drIrrRows["Insurance"] = Convert.ToDecimal(((decimal)drIrrRows["Insurance"]).ToString("0.00"));
                    }
                    if (drIrrRows["Others"].ToString() != "")
                    {
                        drIrrRows["Others"] = Convert.ToDecimal(((decimal)drIrrRows["Others"]).ToString("0.00"));
                    }

                    //if (Convert.ToDecimal(drIrrRows["PrincipalAmount"]) < 0)
                    //{
                    //    drIrrRows.BeginEdit();
                    //    drIrrRows["PrincipalAmount"] = 0;
                    //    drIrrRows.AcceptChanges();
                    // }
                }

                return dtAccountingIRR.DefaultView.ToTable("Repayment Structure");
            }

        }

        /// <summary>
        /// Function to update Insurance,Others etc, in irr details table after IRR Calculation,
        /// this is done in order to arrive at Repayment Structure.
        /// </summary>
        /// <param name="dtIRRDetails"></param>
        /// <param name="strColName"></param>
        private void FunPriUpdateRepaymentStructure(DataTable dtAccountingIRR, DataTable dtIRRDetails, string strColName)
        {

            dtIRRDetails.DefaultView.RowFilter = "TransactionType ='" + strColName + "'";
            DataTable dtDetails = dtIRRDetails.DefaultView.ToTable();


            if (!dtAccountingIRR.Columns.Contains(strColName))
            {
                dtAccountingIRR.Columns.Add(strColName, typeof(decimal));
            }
            foreach (DataRow drDetailsRow in dtDetails.Rows)
            {
                DataRow[] dtrRows = dtAccountingIRR.Select("InstallmentDate = '" + drDetailsRow["InstallmentDate"].ToString() + "' and TransactionType='Installment' ");

                if (dtrRows.Length > 0)
                {
                    decimal decAmt = 0;
                    if (dtrRows[0][strColName].ToString() != "")
                        decAmt = Convert.ToDecimal(dtrRows[0][strColName]);
                    dtrRows[0][strColName] = decAmt + Convert.ToDecimal(drDetailsRow["InstallmentAmount"]);
                    dtrRows[0].AcceptChanges();

                }
            }
        }

        /// <summary>
        /// Function used to arrive Flat Rate from given IRR
        /// </summary>
        /// <param name="dtRepaymentSchedule"></param>
        /// <param name="dtCashInflow"></param>
        /// <param name="dtCashOutflow"></param>
        /// <param name="strFrequency"></param>
        /// <param name="intTenure"></param>
        /// <param name="strTenureType"></param>
        /// <param name="strDateFormat"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decIRR"></param>
        /// <param name="strIRRrest"></param>
        /// <param name="strTimeval"></param>
        /// <param name="strInstallmentType"></param>
        /// <param name="decLimit"></param>
        /// <param name="Irrtype"></param>
        /// <param name="decResultRate">Out parameters Which return Rate</param>
        /// <param name="decCTR"></param>
        /// <param name="decPLR"></param>
        public void FunPubCalculateFlatRate(DataTable dtRepaymentSchedule, DataTable dtCashInflow, DataTable dtCashOutflow, string strFrequency, int intTenure, string strTenureType, string strDateFormat, decimal decPrincipleAmount, double decIRR, string strIRRrest, string strTimeval, string strInstallmentType, decimal decLimit, IRRType Irrtype, out double decResultRate, decimal decCTR, decimal decPLR)
        {

            #region Flat Rate Loop Starting
            double decRateStartValue = 1;
            double decRateEndValue = decIRR;
            decResultRate = 0;
        FlatRateLoop:
            double decRateofInt = (decRateStartValue + decRateEndValue) / 2;


            #endregion

            double decResultIRR = 0;
            #region IRRTable declaration and colunm addition
            DataTable dtIRRDetails = new DataTable();

            dtIRRDetails.Columns.Add("NoofDays", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentNo", typeof(int));
            dtIRRDetails.Columns.Add("FromDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("ToDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("From Date");
            dtIRRDetails.Columns.Add("To Date");
            dtIRRDetails.Columns.Add("InstallmentDate");
            dtIRRDetails.Columns.Add("InstallmentAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("Amount", typeof(decimal));
            dtIRRDetails.Columns.Add("TransactionType");
            dtIRRDetails.Columns.Add("Charge", typeof(decimal));
            dtIRRDetails.Columns.Add("PrevBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("CurrBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("Slno", typeof(int));
            dtIRRDetails.Columns.Add("PrincipalAmount", typeof(decimal), "InstallmentAmount-Charge");
            #endregion

            //IRR Row
            DataRow drIRR;

            #region Adding First Row By default
            DateTime dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());//StringToDate(dtRepaymentSchedule.Rows[0]["FromDate"].ToString(), strDateFormat); ;
            DateTime dtEndDate = dtStartDate;

            #endregion

            #region Adding Cash Inflow Details
            foreach (DataRow drCashInflow in dtCashInflow.Rows)
            {
                if (drCashInflow[Irrtype.ToString()].ToString() == "True")
                {

                    drIRR = dtIRRDetails.NewRow();
                    drIRR["Amount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentAmount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentNo"] = "0";
                    drIRR["FromDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["InstallmentDate"] = drCashInflow["Date"].ToString();// StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["ToDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = "0.00";
                    drIRR["NoofDays"] = "0";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            #endregion

            #region Adding Cash Outflow Details
            foreach (DataRow drCashOutflow in dtCashOutflow.Rows)
            {
                if (drCashOutflow[Irrtype.ToString()].ToString() == "True")
                {
                    // considering Cash flow other than Margin amount (43 flag id for Margin payment)
                    if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "43")
                    {
                        //adding cashflow other than margin cashflows for Company IRR and Business IRR
                        if (Irrtype != IRRType.Accounting_IRR)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                        }

                        else if (Irrtype == IRRType.Accounting_IRR)
                        {
                            //adding cashflow other than Delear payment for accounting IRR  (41 flag id for Dealer payment)
                            if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "41")
                            {
                                drIRR = dtIRRDetails.NewRow();
                                drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                                drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                                drIRR["InstallmentNo"] = "0";
                                drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                                drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                                drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                                drIRR["Charge"] = "0.00";
                                drIRR["PrevBalance"] = "0.00";
                                drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                                drIRR["NoofDays"] = "0";
                                dtIRRDetails.Rows.Add(drIRR);
                            }
                        }
                    }

                }

            }
            #endregion

            #region Adding Finance Amount as first row for Accounting IRR
            if (Irrtype == IRRType.Accounting_IRR)
            {
                drIRR = dtIRRDetails.NewRow();
                drIRR["Amount"] = 0 - decPrincipleAmount;
                drIRR["InstallmentAmount"] = 0 - decPrincipleAmount;
                drIRR["InstallmentNo"] = "0";
                drIRR["FromDate"] = dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                drIRR["InstallmentDate"] = dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                drIRR["ToDate"] = dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                drIRR["Charge"] = "0.00";
                drIRR["PrevBalance"] = "0.00";
                drIRR["CurrBalance"] = decPrincipleAmount;//Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                drIRR["NoofDays"] = "0";
                dtIRRDetails.Rows.Add(drIRR);
            }
            int intLoop = 0;
            int intNoofInstallment = 0;
            #endregion

            //decimal decTotalAmt = Math.Round(FunPubInterestAmount(strTenureType, decPrincipleAmount, Convert.ToDecimal(decRateofInt), intTenure), 0) + decPrincipleAmount;
            decimal decTotalAmt = FunPubInterestAmount(strTenureType, decPrincipleAmount, Convert.ToDecimal(decRateofInt), intTenure) + decPrincipleAmount;

            #region Loop for adding Repayment Structure
            foreach (DataRow drRepay in dtRepaymentSchedule.Rows)
            {
                int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                int intTotalInstallment = intToInstallment - intFromInstallment + 1;
                dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());

                DateTime dtFromDate = dtStartDate;
                decimal decPerInstallAmount = decTotalAmt / Convert.ToInt64(dtRepaymentSchedule.Rows[dtRepaymentSchedule.Rows.Count - 1]["ToInstall"].ToString());
                int intNoLocalInstallment = intNoofInstallment;
                for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                {
                    drIRR = dtIRRDetails.NewRow();
                    drIRR["FromDate"] = dtFromDate;

                    dtFromDate = FunPubGetNextDate(strFrequency, dtStartDate, intLoopCount);

                    drIRR["ToDate"] = dtFromDate;
                    drIRR["InstallmentDate"] = dtFromDate;
                    drIRR["Amount"] = dtRepaymentSchedule.Rows[intLoop]["Amount"].ToString();
                    drIRR["InstallmentAmount"] = decPerInstallAmount;//dtRepaymentSchedule.Rows[intLoop]["PerInstall"].ToString();
                    intNoLocalInstallment = intNoLocalInstallment + 1;
                    drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = "0.00";
                    dtIRRDetails.Rows.Add(drIRR);

                }
                intNoofInstallment = intNoLocalInstallment;
                intLoop++;
            }
            int intEndDateCount = 0;

            DataView dvIrrDetails = dtIRRDetails.DefaultView;
            dvIrrDetails.Sort = "ToDate ASC";
            dtIRRDetails = dvIrrDetails.ToTable();

            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Slno"] = intEndDateCount + 1;
                intEndDateCount++;
            }
            bool blnBusIRRdone = false;
            intEndDateCount = 0;
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {

                if (Irrtype != IRRType.Accounting_IRR)
                {
                    if (Convert.ToDecimal(drIrrRows["Amount"]) < 0 && (!blnBusIRRdone))
                    {
                        int inslno = Convert.ToInt32(drIrrRows["Slno"].ToString());

                        DataView dvAmount = dtIRRDetails.DefaultView;
                        dvAmount.RowFilter = "Slno < " + inslno.ToString();
                        DataTable dtPreEMI = dvAmount.ToTable();
                        decimal decToalPreEMIAmount = 0;
                        foreach (DataRow drEMIRow in dtPreEMI.Rows)
                        {
                            long lnNoofInterestDays = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drEMIRow["InstallmentDate"].ToString()), Convert.ToDateTime(dtPreEMI.Rows[dtPreEMI.Rows.Count - 1]["InstallmentDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            decimal decperInstallAmount = Convert.ToDecimal(drEMIRow["InstallmentAmount"]) + ((Convert.ToDecimal(drEMIRow["InstallmentAmount"]) * lnNoofInterestDays * decPLR) / 36500);

                            decToalPreEMIAmount += decperInstallAmount;
                        }
                        DataView dvIrrStruct = dtIRRDetails.DefaultView;
                        dvIrrStruct.RowFilter = "Slno >= " + inslno.ToString();
                        dtIRRDetails = dvIrrStruct.ToTable();
                        dtIRRDetails.Rows[0]["Amount"] = Convert.ToDecimal(dtIRRDetails.Rows[0]["Amount"].ToString()) + decToalPreEMIAmount;
                        dtIRRDetails.Rows[0]["CurrBalance"] = Convert.ToDecimal(dtIRRDetails.Rows[0]["CurrBalance"].ToString()) - decToalPreEMIAmount;
                        dtIRRDetails.Rows[0]["InstallmentAmount"] = Convert.ToDecimal(dtIRRDetails.Rows[0]["InstallmentAmount"].ToString()) + decToalPreEMIAmount;
                        blnBusIRRdone = true;
                    }

                }

            }
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                if (intEndDateCount != 0)
                    drIrrRows["FromDate"] = dtIRRDetails.Rows[intEndDateCount - 1]["ToDate"];
                //drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                /*For sakthi finanace
             //Code Comment and added by Thalai on 18-Jun-2012 start
             // drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);*/
                switch (strIRRrest)
                {
                    case "monthly":
                        if (strTimeval.ToUpper() == "ADVANCE" || strTimeval.ToUpper() == "ADVANCEFBD")
                            if (DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) > 0)
                                drIrrRows["NoofDays"] = 30;
                            else
                                drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        else
                            drIrrRows["NoofDays"] = 30;
                        break;
                    case "daily":
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        break;
                    default:
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        break;
                }
                //Code Comment and added by Thalai on 18-Jun-2012 end
                intEndDateCount++;
            }
            #endregion

            double intMinVal = 0.00;
            double intMaxVal = 1000.00;

            #region Min val Max val Assiging part
        loopforIRR:
            if (intMaxVal == intMinVal)
            {
                throw new Exception("Cannot Calculate Flat Rate");
            }
            double decIRR_Dynamic = (intMaxVal + intMinVal) / 2;
            int intnoofdays = 0;
            switch (strIRRrest)
            {
                case "monthly":
                    intnoofdays = 1200;
                    break;
                case "daily":
                    intnoofdays = 36500;
                    break;
                case "yearly":
                    intnoofdays = 100;
                    break;
            }
            double decExp = 1 + (decIRR_Dynamic / intnoofdays);
            int intdtRowcount = 0;
            Double dbPrevBalance;
            Double dbCharge = 0.00;
            #endregion

            #region IRR Calculation Loop
            foreach (DataRow drIRRRow in dtIRRDetails.Rows)
            {
                if (intdtRowcount != 0)
                {
                    dbPrevBalance = Convert.ToDouble(dtIRRDetails.Rows[intdtRowcount - 1]["CurrBalance"].ToString());
                    switch (strIRRrest)
                    {
                        case "monthly":
                            dbCharge = dbPrevBalance * Math.Pow(decExp, (Convert.ToDouble(drIRRRow["NoofDays"]) / 30)) - dbPrevBalance;
                            break;
                        case "daily":
                            dbCharge = dbPrevBalance * Math.Pow(decExp, (Convert.ToDouble(drIRRRow["NoofDays"]))) - dbPrevBalance;
                            break;
                    }
                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    drIRRRow["Charge"] = dbCharge;
                    drIRRRow["PrevBalance"] = Convert.ToDouble(drIRRRow["PrevBalance"]);
                    drIRRRow["CurrBalance"] = Convert.ToDouble(drIRRRow["PrevBalance"]) - Convert.ToDouble(drIRRRow["InstallmentAmount"]);
                }
                intdtRowcount++;
            }
            #endregion

            //*******Checking Whether to Continue the Loop or not********************// 
            if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) > decLimit)
            {
                intMaxVal = decIRR_Dynamic;
                goto loopforIRR;
            }
            else if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) < (0 - decLimit))
            {
                intMinVal = decIRR_Dynamic;
                goto loopforIRR;
            }
            //*******End of loop Checking Whether to Continue the Loop or not********************//
            else
            {

                decResultIRR = Math.Round(decIRR_Dynamic, 3);
                if (decResultIRR == decIRR)//Checking Whether the Irr Arrived in trail Error Method is Equal to Given IRR 
                {
                    decResultRate = decRateofInt;
                }
                else if (decIRR > decResultIRR)//If  Given IRR is greater than arrived irr then set Rate start value as arrived IRR
                {
                    decRateStartValue = decRateofInt;
                    goto FlatRateLoop;
                }
                else if (decIRR < decResultIRR)//If  Given IRR is less than arrived irr then set Rate end value as arrived IRR
                {
                    decRateEndValue = decRateofInt;
                    goto FlatRateLoop;
                }

            }

        }

        /// <summary>
        /// Function used to arrive Flat Rate from given IRR
        /// </summary>
        /// <param name="clsIRRParams"></param>
        /// <param name="decResultRate"></param>
        /// <returns></returns>
        public DataSet FunPubCalculateFlatRate(ClsPubIRR clsIRRParams, out decimal decResultRate)
        {
            DataSet dsRepaySchedule = new DataSet();
            #region Flat Rate Loop Starting Step 1
            decimal decRateStartValue = 1;
            decimal decRateEndValue = clsIRRParams.decIRR;
            decResultRate = 0;
            #endregion

            #region Step 2
        FlatRateLoop:
            decimal decRateofInt = Math.Round(((decRateStartValue + decRateEndValue) / 2), 4);

            if (Math.Round(decRateStartValue, 4) == Math.Round(decRateEndValue, 4))
            {
                throw new Exception("Cannot Calculate Flat Rate");
            }
            #endregion

            decimal decResultIRR = 0;

            #region IRRTable declaration and colunm addition
            DataTable dtIRRDetails = new DataTable();

            dtIRRDetails.Columns.Add("NoofDays", typeof(int));
            dtIRRDetails.Columns.Add("InstallmentNo", typeof(int));
            dtIRRDetails.Columns.Add("FromDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("ToDate", typeof(DateTime));
            dtIRRDetails.Columns.Add("From Date");
            dtIRRDetails.Columns.Add("To Date");
            dtIRRDetails.Columns.Add("InstallmentDate");
            dtIRRDetails.Columns.Add("InstallmentAmount", typeof(decimal));
            dtIRRDetails.Columns.Add("Amount", typeof(decimal));
            dtIRRDetails.Columns.Add("TransactionType");
            dtIRRDetails.Columns.Add("Charge", typeof(decimal));
            dtIRRDetails.Columns.Add("PrevBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("CurrBalance", typeof(decimal));
            dtIRRDetails.Columns.Add("Slno", typeof(int));
            dtIRRDetails.Columns.Add("IsMor");
            dtIRRDetails.Columns.Add("PrincipalAmount", typeof(decimal), "InstallmentAmount-Charge");
            #endregion



            decRateofInt = Math.Round(decRateofInt, 4);
            DataTable dtRepaymentSchedule = null;

            #region Step 3 & 4
            dsRepaySchedule = FunPubGetRepaymentDetails(clsIRRParams.strFrequency, clsIRRParams.intTenure, clsIRRParams.strTenureType, clsIRRParams.decPrincipleAmount, decRateofInt, clsIRRParams.returnType, clsIRRParams.decResidualValue, clsIRRParams.dtimeStartDate, clsIRRParams.dtimeDocDate, clsIRRParams.intCashFlow, clsIRRParams.strCashflowDesc, clsIRRParams.decRoundOff, clsIRRParams.blnAccountingIRR, clsIRRParams.blnBusinessIRR, clsIRRParams.blnCompanyIRR, clsIRRParams.strTimeVal, clsIRRParams.dtMoratoriumInput);
            if (clsIRRParams.dtMoratoriumInput != null && clsIRRParams.dtMoratoriumInput.Rows.Count > 0)
                dtRepaymentSchedule = dsRepaySchedule.Tables[2];
            else
                dtRepaymentSchedule = dsRepaySchedule.Tables[0];

            #region Only for Moratorium
            DataTable dtMoratoriumInput = clsIRRParams.dtMoratoriumInput;
            string MoraStDate = "";
            string MoraEndDate = "";
            string strMorType = "";
            if (dtMoratoriumInput != null && dtMoratoriumInput.Rows.Count > 0)
            {
                MoraStDate = dtMoratoriumInput.Rows[0]["Fromdate"].ToString();// DateTime.Now.AddDays(61).ToString("MM/dd/yyyy");
                MoraEndDate = dtMoratoriumInput.Rows[0]["Todate"].ToString(); //Convert.ToDateTime(MoraStDate).AddDays(45).ToString("MM/dd/yyyy");
                strMorType = dtMoratoriumInput.Rows[0]["Moratoriumtype"].ToString().ToUpper();
            }
            #endregion
            //  DataTable dtRepaymentSchedule = FunPriGroupRepayDetails(dtRepaymentStructure, clsIRRParams.intCashFlow, clsIRRParams.strCashflowDesc, clsIRRParams.blnAccountingIRR, clsIRRParams.blnBusinessIRR, clsIRRParams.blnCompanyIRR);
            //IRR Row


            DataRow drIRR;

            #region Adding First Row By default
            DateTime dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());//StringToDate(dtRepaymentSchedule.Rows[0]["FromDate"].ToString(), strDateFormat); ;
            DateTime dtEndDate = dtStartDate;


            #endregion

            #region Adding Cash Inflow Details
            foreach (DataRow drCashInflow in clsIRRParams.dtCashInflow.Rows)
            {
                ///CashFlow_Flag_ID 34 Hardcoded for Not to Include UMFC In IRR Calculation
                ///drCashInflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drCashInflow[clsIRRParams.Irrtype.ToString()].ToString() == "True")//&& drCashInflow["CashFlow_Flag_ID"].ToString() != "34"
                {

                    drIRR = dtIRRDetails.NewRow();
                    drIRR["Amount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentAmount"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["InstallmentNo"] = "0";
                    drIRR["FromDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["InstallmentDate"] = drCashInflow["Date"].ToString();// StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["ToDate"] = drCashInflow["Date"].ToString();//StringToDate(drCashInflow["Date"].ToString(), strDateFormat);
                    drIRR["Charge"] = "0.00";
                    drIRR["PrevBalance"] = "0.00";
                    drIRR["CurrBalance"] = Convert.ToDouble(drCashInflow["Amount"].ToString());
                    drIRR["NoofDays"] = "0";
                    dtIRRDetails.Rows.Add(drIRR);
                }
            }
            #endregion

            #region Adding Cash Outflow Details
            foreach (DataRow drCashOutflow in clsIRRParams.dtCashOutflow.Rows)
            {
                ///drCashOutflow[Irrtype.ToString()].ToString() == "True" to check weather the cash flow has been mapped for the IRR 
                if (drCashOutflow[clsIRRParams.Irrtype.ToString()].ToString() == "True")
                {
                    // considering Cash flow other than Margin amount (43 flag id for Margin payment)
                    if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "43")
                    {
                        //adding cashflow other than margin cashflows for Company IRR and Business IRR
                        if (clsIRRParams.Irrtype != IRRType.Accounting_IRR)
                        {
                            drIRR = dtIRRDetails.NewRow();
                            drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                            drIRR["InstallmentNo"] = "0";
                            drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                            drIRR["Charge"] = "0.00";
                            drIRR["PrevBalance"] = "0.00";
                            drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                            drIRR["NoofDays"] = "0";
                            dtIRRDetails.Rows.Add(drIRR);
                        }

                        else if (clsIRRParams.Irrtype == IRRType.Accounting_IRR)
                        {
                            //adding cashflow other than Delear payment for accounting IRR  (41 flag id for Dealer payment)
                            if (drCashOutflow["CashFlow_Flag_ID"].ToString() != "41")
                            {
                                drIRR = dtIRRDetails.NewRow();
                                drIRR["Amount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                                drIRR["InstallmentAmount"] = 0 - Convert.ToDouble(drCashOutflow["Amount"].ToString());
                                drIRR["InstallmentNo"] = "0";
                                drIRR["FromDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                                drIRR["InstallmentDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                                drIRR["ToDate"] = drCashOutflow["Date"].ToString();//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                                drIRR["Charge"] = "0.00";
                                drIRR["PrevBalance"] = "0.00";
                                drIRR["CurrBalance"] = Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                                drIRR["NoofDays"] = "0";
                                dtIRRDetails.Rows.Add(drIRR);
                            }
                        }
                    }

                }

            }
            #endregion

            /***********************For Accounting IRR add finance amount as ouflow happebd on the document date itself*****************/
            #region Adding Finance Amount as first row for Accounting IRR
            if (clsIRRParams.Irrtype == IRRType.Accounting_IRR)
            {
                drIRR = dtIRRDetails.NewRow();
                drIRR["Amount"] = 0 - clsIRRParams.decPrincipleAmount;
                drIRR["InstallmentAmount"] = 0 - clsIRRParams.decPrincipleAmount;
                drIRR["InstallmentNo"] = "0";
                drIRR["FromDate"] = clsIRRParams.dtimeDocDate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                drIRR["InstallmentDate"] = clsIRRParams.dtimeDocDate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                drIRR["ToDate"] = clsIRRParams.dtimeDocDate;//dtStartDate;//StringToDate(drCashOutflow["Date"].ToString(), strDateFormat);
                drIRR["Charge"] = "0.00";
                drIRR["PrevBalance"] = "0.00";
                drIRR["CurrBalance"] = clsIRRParams.decPrincipleAmount;//Convert.ToDouble(drCashOutflow["Amount"].ToString());//"0.00";
                drIRR["NoofDays"] = "0";
                dtIRRDetails.Rows.Add(drIRR);
            }
            int intLoop = 0;
            int intNoofInstallment = 0;
            #endregion

            #region Loop for adding Repayment Structure
            DateTime dtFromDate = new DateTime();
            int intNoLocalInstallment = 0;
            dtStartDate = Convert.ToDateTime(dtRepaymentSchedule.Rows[0]["FromDate"].ToString());
            //installments
            DataRow[] drRepay_Array = dtRepaymentSchedule.Select("CashFlow_Flag_ID=23", "slno asc");
            foreach (DataRow drRepay in drRepay_Array)
            {
                if (drRepay[clsIRRParams.Irrtype.ToString()].ToString() == "True")
                {
                    int intFromInstallment = Convert.ToInt32(drRepay["FromInstall"].ToString());
                    int intToInstallment = Convert.ToInt32(drRepay["ToInstall"].ToString());
                    int intTotalInstallment = intToInstallment - intFromInstallment + 1;
                    //dtStartDate = Convert.ToDateTime(drRepay["FromDate"].ToString());
                    //StringToDate(drRepay["FromDate"].ToString(), strDateFormat);

                    dtFromDate = dtStartDate;

                    intNoLocalInstallment = intNoofInstallment;

                    for (int intLoopCount = 0; intLoopCount < intTotalInstallment; intLoopCount++)
                    {
                        drIRR = dtIRRDetails.NewRow();
                        drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);

                        switch (clsIRRParams.returnType)
                        {
                            case RepaymentType.FC:
                            case RepaymentType.WC:
                            case RepaymentType.TLE:
                                switch (clsIRRParams.strTenureType.ToLower())
                                {
                                    case "months":
                                        clsIRRParams.strFrequency = "4";
                                        break;
                                    case "weeks":
                                        clsIRRParams.strFrequency = "2";
                                        break;
                                    case "days":
                                        clsIRRParams.strFrequency = "0";
                                        break;
                                }
                                dtFromDate = FunPubGetNextDate(clsIRRParams.strFrequency, dtStartDate, clsIRRParams.intTenure);
                                break;
                            default:
                                dtFromDate = FunPubGetNextDate(clsIRRParams.strFrequency, dtStartDate, intNoLocalInstallment);
                                break;
                        }

                        drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                        drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                        drIRR["Amount"] = drRepay["Amount"].ToString();
                        drIRR["TransactionType"] = "Installment";
                        drIRR["InstallmentAmount"] = drRepay["PerInstall"].ToString();
                        intNoLocalInstallment = intNoLocalInstallment + 1;
                        drIRR["InstallmentNo"] = Convert.ToString(intNoLocalInstallment);
                        //drIRR["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIRR["FromDate"].ToString()), Convert.ToDateTime(drIRR["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIRR["Charge"] = "0.00";
                        drIRR["PrevBalance"] = "0.00";
                        drIRR["CurrBalance"] = "0.00";
                        dtIRRDetails.Rows.Add(drIRR);

                    }

                    intNoofInstallment = intNoLocalInstallment;
                    intLoop++;
                }
            }
            if (clsIRRParams.decResidualValue.HasValue)
            {
                drIRR = dtIRRDetails.NewRow();
                drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                dtFromDate = FunPubGetNextDate(clsIRRParams.strFrequency, dtStartDate, intNoLocalInstallment);
                drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                drIRR["Amount"] = dtRepaymentSchedule.Rows[0]["Amount"].ToString();
                drIRR["InstallmentAmount"] = clsIRRParams.decPrincipleAmount * ((clsIRRParams.decResidualValue.Value) / 100);
                intNoLocalInstallment = intNoLocalInstallment + 1;
                drIRR["InstallmentNo"] = intNoLocalInstallment;//Convert.ToInt32(dtIRRDetails.Rows[]["InstallmentNo"].ToString())+1;
                drIRR["Charge"] = "0.00";
                drIRR["PrevBalance"] = "0.00";
                drIRR["CurrBalance"] = "0.00";
                drIRR["TransactionType"] = "Others";
                dtIRRDetails.Rows.Add(drIRR);
            }
            else if (clsIRRParams.decResidualAmount.HasValue)
            {
                drIRR = dtIRRDetails.NewRow();
                drIRR["FromDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                dtFromDate = FunPubGetNextDate(clsIRRParams.strFrequency, dtStartDate, intNoLocalInstallment);
                drIRR["ToDate"] = dtFromDate;//dtFromDate.ToString(strDateFormat);
                drIRR["InstallmentDate"] = dtFromDate;//.ToString(strDateFormat);
                drIRR["Amount"] = dtRepaymentSchedule.Rows[0]["Amount"].ToString();
                //drIRR["InstallmentAmount"] = decPrincipleAmount + decResidualAmount.Value;
                drIRR["InstallmentAmount"] = clsIRRParams.decResidualAmount.Value;
                intNoLocalInstallment = intNoLocalInstallment + 1;
                drIRR["InstallmentNo"] = intNoLocalInstallment;//Convert.ToInt32(dtIRRDetails.Rows[]["InstallmentNo"].ToString())+1;
                drIRR["Charge"] = "0.00";
                drIRR["PrevBalance"] = "0.00";
                drIRR["CurrBalance"] = "0.00";
                drIRR["TransactionType"] = "Others";
                dtIRRDetails.Rows.Add(drIRR);
            }

            int intEndDateCount = 0;

            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {

                drIrrRows["Slno"] = intEndDateCount + 1;
                intEndDateCount++;
            }

            #region For Deffered Payments
            DataView dvIrrDetails = dtIRRDetails.DefaultView;
            dvIrrDetails.Sort = "ToDate,InstallmentAmount ASC";

            dtIRRDetails = dvIrrDetails.ToTable();

            bool blnBusIRRdone = false;
            intEndDateCount = 0;

            #endregion

            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                // drIrrRows["FromDate"] = Convert.ToDateTime(drIrrRows["FromDate"]).ToString(strDateFormat);
                //if (intEndDateCount != 0)
                //    drIrrRows["FromDate"] = dtIRRDetails.Rows[intEndDateCount - 1]["ToDate"];

                if (drIrrRows["TransactionType"].ToString() == "Installment")
                {
                    if (intEndDateCount != 0)
                        drIrrRows["FromDate"] = dtIRRDetails.Rows[intEndDateCount - 1]["ToDate"];
                    //else
                    //    drIrrRows["FromDate"] = dtAppdate;
                }

                #region For Moratorium
                bool blnMor = false;
                if (strMorType != "" && drIrrRows["TransactionType"].ToString() == "Installment")// == "INTEREST")
                {
                    if (Convert.ToDateTime(drIrrRows["FromDate"].ToString()) > Convert.ToDateTime(MoraStDate) && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) < Convert.ToDateTime(MoraEndDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                        && Convert.ToDateTime(drIrrRows["ToDate"].ToString()) > Convert.ToDateTime(MoraEndDate) && blnMor == false)
                    {
                        drIrrRows["FromDate"] = Convert.ToDateTime(dtMoratoriumInput.Rows[0]["ToDate"].ToString());
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        blnMor = true;
                    }
                    else if (Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) > Convert.ToDateTime(MoraStDate)
                       && Convert.ToDateTime(drIrrRows["InstallmentDate"].ToString()) < Convert.ToDateTime(MoraEndDate) &&
                        blnMor == false)
                    {
                        //drIrrRows["NoofDays"] = 0;
                        drIrrRows["IsMor"] = "0";
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                    else if (blnMor == false && Convert.ToDateTime(drIrrRows["FromDate"].ToString()) <= Convert.ToDateTime(MoraStDate) &&
                        Convert.ToDateTime(drIrrRows["ToDate"].ToString()) >= Convert.ToDateTime(MoraEndDate))
                    //dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").Length > 0)
                    {
                        blnMor = true;
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        drIrrRows["IsMor"] = Convert.ToInt32(drIrrRows["NoofDays"].ToString()) - DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(MoraStDate), Convert.ToDateTime(MoraEndDate), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);

                    }
                    else
                    {
                        drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                    }
                }
                #endregion
                else
                {
                    //Code Comment and added by Thalai on 18-Jun-2012 start
                    // drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);*/
                    switch (clsIRRParams.strIRRrest)
                    {
                        case "monthly":
                            if (clsIRRParams.strTimeVal.ToUpper() == "ADVANCE" || clsIRRParams.strTimeVal.ToUpper() == "ADVANCEFBD")
                                if (DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1) > 0)
                                    drIrrRows["NoofDays"] = 30;
                                else
                                    drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            else
                                drIrrRows["NoofDays"] = 30;
                            break;
                        case "daily":
                            drIrrRows["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drIrrRows["FromDate"].ToString()), Convert.ToDateTime(drIrrRows["ToDate"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                            break;
                    }
                    //Code Comment and added by Thalai on 18-Jun-2012 end
                }
                intEndDateCount++;
            }

            #endregion

            decimal decAmount = (decimal)dtIRRDetails.Compute("Sum(InstallmentAmount)", "NoofDays >0 ");
            foreach (DataRow drIrrRows in dtIRRDetails.Rows)
            {
                drIrrRows["Amount"] = decAmount;
            }
            double intMinVal = 0.00;
            double intMaxVal = 1000.00;
            #endregion

            #region Step 5
            #region Min val Max val Assiging part
        loopforIRR:
            if (intMaxVal == intMinVal)
            {
                throw new Exception("Cannot Calculate IRR");
            }
            double decIRR_Dynamic = (intMaxVal + intMinVal) / 2;
            //decIRR = Math.Round(decIRR, 4);
            int intnoofdays = 0;
            switch (clsIRRParams.strIRRrest)
            {
                case "monthly":
                    intnoofdays = 1200;
                    break;
                case "daily":
                    intnoofdays = 36500;
                    break;
                case "yearly":
                    intnoofdays = 100;
                    break;
            }
            double decExp = 1 + (decIRR_Dynamic / intnoofdays);
            int intdtRowcount = 0;
            decimal dbPrevBalance = 0;
            decimal dbCharge = 0;
            //Double dbCurrPrevBalance;
            #endregion

            #region IRR Calculation Loop
            foreach (DataRow drIRRRow in dtIRRDetails.Rows)
            {
                if (intdtRowcount != 0)
                {
                    dbPrevBalance = Convert.ToDecimal(dtIRRDetails.Rows[intdtRowcount - 1]["CurrBalance"].ToString());
                }
                switch (clsIRRParams.strIRRrest)
                {
                    case "monthly":
                        //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"]) / 30))) - dbPrevBalance;
                        if (Convert.ToDouble(drIRRRow["NoofDays"]) < 30)
                            dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(1 + (decIRR_Dynamic / (365 * 100))), (Convert.ToDouble(drIRRRow["NoofDays"])))) - dbPrevBalance;
                        else
                            dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"]) / 30))) - dbPrevBalance;
                        break;
                        break;
                    case "daily":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["NoofDays"])))) - dbPrevBalance;
                        break;
                    case "Yearly":
                        dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(1 + decExp / 12), ((Convert.ToDouble(drIRRRow["NoofDays"]) * 12) / 365))) - dbPrevBalance;
                        break;
                }
                // drIRRRow["PrevBalance"] = Math.Round(dbPrevBalance, 2) + Math.Round(dbCharge, 2);


                //#region For Moratorium
                //if (strMorType.ToUpper() == "INTEREST")
                //{
                //    if (drIRRRow["IsMor"].ToString() == "1")
                //        dbCharge = 0;

                //    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                //    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                //    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                //    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);

                //}
                //#endregion
                //else
                //{
                //    #region For Moratorium
                //    if (strMorType.ToUpper() == "BOTH")
                //    {
                //        if (drIRRRow["InstallmentAmount"].ToString() == "0")
                //            dbCharge = 0;
                //    }
                //    #endregion

                //    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                //    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                //    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                //    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);
                //}

                #region For Moratorium
                if (strMorType.ToUpper() == "INTEREST")
                {
                    if (drIRRRow["IsMor"].ToString() != "")
                    {
                        //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                        switch (clsIRRParams.strIRRrest)
                        {
                            case "monthly":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                break;
                            case "daily":
                                dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                break;
                        }
                    }

                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);

                }
                #endregion
                else
                {
                    #region For Moratorium
                    if (strMorType.ToUpper() == "BOTH")
                    {
                        if (drIRRRow["IsMor"].ToString() != "")
                        {
                            //dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                            switch (clsIRRParams.strIRRrest)
                            {
                                case "monthly":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"]) / 30))) - dbPrevBalance;
                                    break;
                                case "daily":
                                    dbCharge = dbPrevBalance * Convert.ToDecimal(Math.Pow(Convert.ToDouble(decExp), (Convert.ToDouble(drIRRRow["IsMor"])))) - dbPrevBalance;
                                    break;
                            }
                        }
                    }

                    #endregion

                    drIRRRow["PrevBalance"] = dbPrevBalance + dbCharge;
                    drIRRRow["Charge"] = dbCharge;//Math.Round(dbCharge, 2);

                    drIRRRow["PrevBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]);//Math.Round(Convert.ToDouble(drIRRRow["PrevBalance"]), 2);//
                    drIRRRow["CurrBalance"] = Convert.ToDecimal(drIRRRow["PrevBalance"]) - Convert.ToDecimal(drIRRRow["InstallmentAmount"]);
                }

                intdtRowcount++;
            }

            dtIRRDetails.AcceptChanges();

            #endregion
            #endregion

            //*******Checking Whether to Continue the Loop or not********************// 
            if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) > clsIRRParams.decLimit)
            {
                intMaxVal = decIRR_Dynamic;
                goto loopforIRR;
            }
            else if (Convert.ToDecimal(dtIRRDetails.Rows[dtIRRDetails.Rows.Count - 1]["CurrBalance"].ToString()) < (0 - clsIRRParams.decLimit))
            {
                intMinVal = decIRR_Dynamic;
                goto loopforIRR;
            }

            //*******End of loop Checking Whether to Continue the Loop or not********************//
            else
            {

                //decResultRate = decRateofInt;

                decResultIRR = Convert.ToDecimal(Math.Round(decIRR_Dynamic, 4));
                //Setp 6
                //S3GLogger.LogMessage("HERE Logging " + decResultIRR.ToString(), "IRR Calculation");
                //if ((clsIRRParams.decIRR - decResultIRR) <= Convert.ToDecimal(0.0005) && (clsIRRParams.decIRR - decResultIRR) >= Convert.ToDecimal(-0.0005))//Checking Whether the Irr Arrived in trail Error Method is Equal to Given IRR 
                if (Math.Round(clsIRRParams.decIRR, 4) == decResultIRR)//Checking Whether the Irr Arrived in trail Error Method is Equal to Given IRR 
                {
                    decResultRate = decRateofInt;
                    //S3GLogger.LogMessage("HERE Logging Rate Arrived" + decRateofInt.ToString(), "IRR Calculation");
                }
                //Step 7
                else if (clsIRRParams.decIRR > decResultIRR)//If  Given IRR is greater than arrived irr then set Rate start value as arrived IRR
                {
                    //If Prev value & Current value are same Set it as Result Rate
                    if (decRateStartValue == decRateofInt)
                    {
                        decResultRate = decRateofInt;
                    }
                    else
                    {
                        decRateStartValue = decRateofInt;
                        //S3GLogger.LogMessage("HERE Logging in Greater than Loop " + decRateofInt.ToString() + "," + decResultIRR.ToString() + "," + clsIRRParams.decIRR.ToString(), "IRR Calculation");
                        goto FlatRateLoop;
                    }
                }
                else if (clsIRRParams.decIRR < decResultIRR)//If  Given IRR is less than arrived irr then set Rate end value as arrived IRR
                {
                    //If Prev value & Current value are same Set it as Result Rate
                    if (decRateEndValue == decRateofInt)
                    {
                        decResultRate = decRateofInt;
                    }
                    else
                    {
                        decRateEndValue = decRateofInt;
                        //S3GLogger.LogMessage("HERE Logging in lesser than Loop " + decRateofInt.ToString() + "," + decResultIRR.ToString() + "," + clsIRRParams.decIRR.ToString(), "IRR Calculation");
                        goto FlatRateLoop;
                    }
                }

            }
            return dsRepaySchedule;

        }

        #endregion

        #region Other Common BusLogic Methods

        /// <summary>
        /// Change string to date
        /// </summary>
        /// <param name="strInput"></param>
        /// <param name="dateformat"></param>
        /// <returns></returns>
        public static DateTime StringToDate(string strInput, string dateformat)
        {

            DateTimeFormatInfo dtformat = new DateTimeFormatInfo();
            dtformat.ShortDatePattern = dateformat;
            DateTime dt = DateTime.Parse(strInput, dtformat);
            return Convert.ToDateTime(dt.ToString("yyyy/MM/dd"));
        }

        /// <summary>
        /// Validation of cashflow amount and asset value
        /// </summary>
        /// <param name="dtCashOutflow"></param>
        /// <param name="decToatalAssetValue"></param>
        /// <param name="decMarginAmount"></param>
        /// <param name="decMarginPercent"></param>
        /// <param name="decTotalAmount"></param>
        /// <returns></returns>
        public static bool CashoutflowAssetValueValidate(DataTable dtCashOutflow, decimal decToatalAssetValue, decimal decMargin, out decimal decTotalAmount)
        {
            decimal decTotalOutflow = 0;

            DataRow[] drFinanAmtRow = dtCashOutflow.Select("CashFlow_Flag_ID = 41");
            if (drFinanAmtRow.Length > 0)
            {
                decTotalOutflow = (decimal)dtCashOutflow.Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");
            }
            //if (decMarginAmount.HasValue)
            //{
            //    decToatalAssetValue = Math.Round(decToatalAssetValue,0) - Math.Round(decMarginAmount.Value,0);
            //}
            //if (decMarginPercent.HasValue)
            //{
            //    decToatalAssetValue = Math.Round(decToatalAssetValue,0) - Math.Round((decToatalAssetValue * (decMarginPercent.Value / 100)),0);
            //}
            if (decTotalOutflow == 0)
                throw new Exception("Total Outflow amount cannot be 0");
            decTotalAmount = decToatalAssetValue;
            if (decTotalOutflow > (decToatalAssetValue - decMargin))
                return false;
            else
                return true;
        }

        public static bool CashoutflowAssetValueValidate(decimal decFinanceAmount, decimal decToatalAssetValue, decimal decMargin, out decimal decTotalAmount)
        {

            decTotalAmount = decToatalAssetValue - decMargin;

            if (decFinanceAmount == 0)
                throw new Exception("Total Finance amount cannot be 0");

            if (decFinanceAmount > (decTotalAmount))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Validate cash flow with facility amount
        /// </summary>
        /// <param name="dtCashOutflow"></param>
        /// <param name="decFacilityAmount"></param>
        /// <param name="decMarginAmount"></param>
        /// <param name="decMarginPercent"></param>
        /// <param name="decTotalAmount"></param>
        /// <returns></returns>
        public static bool CashoutflowFacilityValidate(DataTable dtCashOutflow, decimal decFacilityAmount, decimal? decMarginAmount, decimal? decMarginPercent, out decimal decTotalAmount)
        {
            decimal decTotalOutflow = 0;
            //foreach (DataRow drCashoutflowrow in dtCashOutflow.Rows)
            //{
            //    if (drCashoutflowrow["CashFlow_Flag_ID"].ToString() == "41")
            //    {
            //        decTotalOutflow += Convert.ToDecimal(drCashoutflowrow["Amount"]);
            //    }
            //}
            DataRow[] drFinanAmtRow = dtCashOutflow.Select("CashFlow_Flag_ID = 41");
            if (drFinanAmtRow.Length > 0)
            {
                decTotalOutflow = (decimal)dtCashOutflow.Compute("Sum(Amount)", "CashFlow_Flag_ID = 41");
            }
            if (decMarginAmount.HasValue)
            {
                decFacilityAmount = decFacilityAmount - decMarginAmount.Value;
            }
            if (decMarginPercent.HasValue)
            {
                decFacilityAmount = decFacilityAmount - (decFacilityAmount * (decMarginPercent.Value / 100));
            }
            decTotalAmount = decFacilityAmount;
            if (decTotalOutflow == 0)
                throw new Exception("Total Outflow amount cannot be 0");
            if (decTotalOutflow > decFacilityAmount)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Function to get next date when frequency and start date is given
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="dtFromDate"></param>
        /// <returns></returns>
        public static DateTime FunPubGetNextDate(string strFrequency, DateTime dtFromDate)
        {
            DateTime dtToDate;
            switch (strFrequency.ToLower())
            {
                //Weekly
                case "2":
                    dtToDate = dtFromDate.AddDays(7);
                    break;
                //Fortnightly
                case "3":
                    dtToDate = dtFromDate.AddDays(15);
                    break;
                //Monthly
                case "4":
                    dtToDate = dtFromDate.AddMonths(1);
                    break;
                //bi monthly
                case "5":
                    dtToDate = dtFromDate.AddMonths(2);
                    break;
                //quarterly
                case "6":
                    dtToDate = dtFromDate.AddMonths(3);
                    break;
                // half yearly
                case "7":
                    dtToDate = dtFromDate.AddMonths(6);
                    break;
                //annually
                case "8":
                    dtToDate = dtFromDate.AddYears(1);
                    break;
                //daily
                case "0":
                    dtToDate = dtFromDate.AddDays(1);
                    break;
                //daily
                case "1":
                    dtToDate = dtFromDate.AddDays(1);
                    break;
                default:
                    dtToDate = dtFromDate.AddMonths(1);
                    break;
            }
            /*   DateTime lastDayOfCurrentMonth = new DateTime(dtToDate.Year, dtToDate.Month,
                          DateTime.DaysInMonth(dtToDate.Year, dtToDate.Month));

               if (lastDayOfCurrentMonth != dtToDate)
               {
                   if (dtFromDate.Day >= dtToDate.Day)
                   {
                       if (dtFromDate.Day > lastDayOfCurrentMonth.Day)
                           dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + lastDayOfCurrentMonth.Day.ToString() + "/" + dtToDate.Year.ToString());
                       else
                           dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + dtFromDate.Day.ToString() + "/" + dtToDate.Year.ToString());
                   }
               }*/
            dtToDate = FunPubMonthEndDate(dtFromDate, dtToDate);
            return dtToDate;
        }

        /// <summary>
        /// Function to get next date when frequency and start date is given
        /// and validate the last day of month
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="dtFromDate"></param>
        /// <returns></returns>
        public static DateTime FunPubGetNextDate(string strFrequency, DateTime dtFromDate, DateTime dtStartDate)
        {
            DateTime dtToDate;
            switch (strFrequency.ToLower())
            {
                //Weekly
                case "2":
                    dtToDate = dtFromDate.AddDays(7);
                    break;
                //Fortnightly
                case "3":
                    dtToDate = dtFromDate.AddDays(15);
                    break;
                //Monthly
                case "4":
                    dtToDate = dtFromDate.AddMonths(1);


                    break;
                //bi monthly
                case "5":
                    dtToDate = dtFromDate.AddMonths(2);
                    break;
                //quarterly
                case "6":
                    dtToDate = dtFromDate.AddMonths(3);
                    break;
                // half yearly
                case "7":
                    dtToDate = dtFromDate.AddMonths(6);
                    break;
                //annually
                case "8":
                    dtToDate = dtFromDate.AddYears(1);
                    break;
                //daily
                case "0":
                    dtToDate = dtFromDate.AddDays(1);
                    break;
                //daily
                case "1":
                    dtToDate = dtFromDate.AddDays(1);
                    break;
                default:
                    dtToDate = dtFromDate.AddMonths(1);
                    break;
            }
            /*   DateTime lastDayOfCurrentMonth = new DateTime(dtToDate.Year, dtToDate.Month,
                          DateTime.DaysInMonth(dtToDate.Year, dtToDate.Month));

               if (lastDayOfCurrentMonth != dtToDate)
               {
                   if (dtFromDate.Day >= dtToDate.Day)
                   {
                       if (dtStartDate.Day > lastDayOfCurrentMonth.Day)
                           dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + lastDayOfCurrentMonth.Day.ToString() + "/" + dtToDate.Year.ToString());
                       else
                           dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + dtStartDate.Day.ToString() + "/" + dtToDate.Year.ToString());
                   }
               }*/
            dtToDate = FunPubMonthEndDate(dtStartDate, dtToDate);
            return dtToDate;
        }

        /// <summary>
        /// Function to get next date when frequency,start date and No of installment is given
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="dtFromDate"></param>
        /// <param name="dtEndDate"></param>
        /// <param name="intNoInstalment"></param>
        /// <returns></returns>
        public static DateTime FunPubGetNextDate(string strFrequency, DateTime dtFromDate, DateTime dtEndDate, int intNoInstalment)
        {
            DateTime dtToDate;
            switch (strFrequency.ToLower())
            {
                //Weekly
                case "2":
                    intNoInstalment = intNoInstalment * 7;
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //Fortnightly
                case "3":
                    intNoInstalment = intNoInstalment * 15;
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //Monthly
                case "4":
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);

                    break;
                //bi monthly
                case "5":
                    intNoInstalment = intNoInstalment * 2;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //quarterly
                case "6":
                    intNoInstalment = intNoInstalment * 3;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                // half yearly
                case "7":
                    intNoInstalment = intNoInstalment * 6;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //annually
                case "8":
                    intNoInstalment = intNoInstalment * 12;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //daily
                case "0":
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //daily
                case "1":
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                default:
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
            }

            /* DateTime lastDayOfCurrentMonth = new DateTime(dtToDate.Year, dtToDate.Month,
                     DateTime.DaysInMonth(dtToDate.Year, dtToDate.Month));

             if (lastDayOfCurrentMonth != dtToDate)
             {
                 if (dtFromDate.Day >= dtToDate.Day)
                 {
                     if (dtFromDate.Day > lastDayOfCurrentMonth.Day)
                         dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + lastDayOfCurrentMonth.Day.ToString() + "/" + dtToDate.Year.ToString());
                     else
                         dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + dtFromDate.Day.ToString() + "/" + dtToDate.Year.ToString());
                 }
             }*/
            dtToDate = FunPubMonthEndDate(dtFromDate, dtToDate);
            return dtToDate;
        }


        /// <summary>
        /// Function to get next date when frequency,start date and No of installment is given
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="dtFromDate"></param>
        /// <param name="intNoInstalment"></param>
        /// <returns></returns>
        public static DateTime FunPubGetNextDate(string strFrequency, DateTime dtFromDate, int intNoInstalment)
        {
            DateTime dtToDate;
            switch (strFrequency.ToLower())
            {
                //Weekly
                case "2":
                    intNoInstalment = intNoInstalment * 7;
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //Fortnightly
                case "3":
                    intNoInstalment = intNoInstalment * 15;
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //Monthly
                case "4":
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //bi monthly
                case "5":
                    intNoInstalment = intNoInstalment * 2;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //quarterly
                case "6":
                    intNoInstalment = intNoInstalment * 3;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                // half yearly
                case "7":
                    intNoInstalment = intNoInstalment * 6;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //annually
                case "8":
                    intNoInstalment = intNoInstalment * 12;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //daily
                case "0":
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //daily
                case "1":
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                default:
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
            }


            /*   DateTime lastDayOfCurrentMonth = new DateTime(dtToDate.Year, dtToDate.Month,
                         DateTime.DaysInMonth(dtToDate.Year, dtToDate.Month));

               if (lastDayOfCurrentMonth != dtToDate)
               {
                   if (dtFromDate.Day >= dtToDate.Day)
                   {
                       if (dtFromDate.Day > lastDayOfCurrentMonth.Day)
                       dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + lastDayOfCurrentMonth.Day.ToString() + "/" + dtToDate.Year.ToString());
                       else
                           dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + dtFromDate.Day.ToString() + "/" + dtToDate.Year.ToString());
                   }
               }*/
            dtToDate = FunPubMonthEndDate(dtFromDate, dtToDate);

            return dtToDate;
        }




        /// <summary>
        /// Function to get next date when frequency,
        /// start date and No of installment is given 
        /// and validate the last day of month
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="dtFromDate"></param>
        /// <param name="intNoInstalment"></param>
        /// <returns></returns>
        public static DateTime FunPubGetNextDate(string strFrequency, DateTime dtFromDate, int intNoInstalment, DateTime dtStartDate)
        {
            DateTime dtToDate;
            switch (strFrequency.ToLower())
            {
                //Weekly
                case "2":
                    intNoInstalment = intNoInstalment * 7;
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //Fortnightly
                case "3":
                    intNoInstalment = intNoInstalment * 15;
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //Monthly
                case "4":
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);



                    break;
                //bi monthly
                case "5":
                    intNoInstalment = intNoInstalment * 2;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //quarterly
                case "6":
                    intNoInstalment = intNoInstalment * 3;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                // half yearly
                case "7":
                    intNoInstalment = intNoInstalment * 6;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //annually
                case "8":
                    intNoInstalment = intNoInstalment * 12;
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
                //daily
                case "0":
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                //daily
                case "1":
                    dtToDate = dtFromDate.AddDays(intNoInstalment);
                    break;
                default:
                    dtToDate = dtFromDate.AddMonths(intNoInstalment);
                    break;
            }

            /*DateTime lastDayOfCurrentMonth = new DateTime(dtToDate.Year, dtToDate.Month,
                        DateTime.DaysInMonth(dtToDate.Year, dtToDate.Month));

            if (lastDayOfCurrentMonth != dtToDate)
            {
                if (dtStartDate.Day >= dtToDate.Day)
                {
                    if (dtStartDate.Day > lastDayOfCurrentMonth.Day)
                    dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + lastDayOfCurrentMonth.Day.ToString() + "/" + dtToDate.Year.ToString());
                    else
                        dtToDate = DateTime.Parse(dtToDate.Month.ToString() + "/" + dtStartDate.Day.ToString() + "/" + dtToDate.Year.ToString());
                }
            }*/

            dtToDate = FunPubMonthEndDate(dtStartDate, dtToDate);

            return dtToDate;
        }


        /// <summary>
        /// To check if start date is month end then all dates coming inside will return month end date
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static DateTime FunPubMonthEndDate(DateTime dtFromdate, DateTime dtTodate)
        {
            DateTime dtOutDate = dtTodate;
            DateTime lastDayOfFromMonth = new DateTime(dtFromdate.Year, dtFromdate.Month,
                      DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month));
            if (dtFromdate == lastDayOfFromMonth)//if start date is month end
            {
                DateTime lastDayOfToMonth = new DateTime(dtTodate.Year, dtTodate.Month,
                      DateTime.DaysInMonth(dtTodate.Year, dtTodate.Month));
                if (dtTodate != lastDayOfToMonth)//if to date is different then arrive month end
                {
                    dtOutDate = DateTime.Parse(dtTodate.Month.ToString() + "/" + lastDayOfToMonth.Day.ToString() + "/" + dtTodate.Year.ToString());
                }

            }

            return dtOutDate;
        }


        public static decimal FunPubGetRate(decimal decIRR)
        {
            //Microsoft.Office.Interop.Excel.WorksheetClass ExcelSheet= new Microsoft.Office.Interop.Excel.WorksheetClass();
            //ExcelSheet.Application.WorksheetFunction.Pmt(de
            return 0;
        }

        #endregion

        #region Methods for Bulk Revision
        public DataTable FunPubGetRevisedEmiDetails(string strFrequency, int intTenure, string strTenureType, decimal decFinanceAmount, decimal decRate, RepaymentType returnType, decimal? decresidualValue, DateTime dtimeStartDate, DateTime dtimeDocDate, decimal decRoundOff, string strTimeVal, DateTime dtEffectivefromdate, decimal decRevisedRate, decimal decRevisedAmount, string stInter_Interest)
        {
            bool blnRounded;

            DataTable dtOldEmiDetails = FunPubCalculateRepaymentDetails(strFrequency, intTenure, strTenureType, decFinanceAmount, decRate, returnType, decresidualValue, dtimeStartDate, dtimeDocDate, decRoundOff, out blnRounded, strTimeVal);

            dtOldEmiDetails.PrimaryKey = new DataColumn[] { dtOldEmiDetails.Columns["InstallmentNo"] };
            DataRow[] drFilteredRow = dtOldEmiDetails.Select("InstallmentDate >=#" + dtEffectivefromdate + "#");
            DataSet dsFiltered = new DataSet();
            dsFiltered.Merge(drFilteredRow);
            foreach (DataRow drRow in dsFiltered.Tables[0].Rows)
            {
                drRow["InstallmentAmount"] = Math.Round(Convert.ToDecimal(drRow["InstallmentAmount"].ToString()) + FunPubGetBalAmount(decRevisedAmount, decRevisedRate, intTenure, strTenureType, drFilteredRow.Length), 0);
            }

            #region [//changes Done by Tamilselvan.S on 03/12/2011 for finance amount calculation]
            //decimal decTotalAmt = decRevisedAmount + Math.Round(FunPubInterestAmount(strTenureType, decRevisedAmount, decRate, intTenure), 0) + Math.Round(FunPubInterestAmount(strTenureType, decRevisedAmount, decRevisedRate, intTenure), 0);

            decimal decOldFinanceAmt = 0;
            if (dtOldEmiDetails.Columns.Contains("Installment_Number"))
                decOldFinanceAmt = (decimal)dtOldEmiDetails.Compute("Sum(FinanceCharges)", "Installment_Number>0");
            else
                decOldFinanceAmt = (decimal)dtOldEmiDetails.Compute("Sum(FinanceCharges)", "InstallmentNo>0");
            //decimal decTotalAmt = decRevisedAmount + Math.Round(decOldFinanceAmt, 0) + Math.Round(FunPubInterestAmount(strTenureType, decRevisedAmount, decRevisedRate, intTenure), 0);
            decimal decTotalAmt = decRevisedAmount + Math.Round(decOldFinanceAmt, 0) + FunPubInterestAmount(strTenureType, decRevisedAmount, decRevisedRate, intTenure);
            #endregion

            DataSet dsDetails = new DataSet();
            dsDetails.Merge(dtOldEmiDetails);
            dsDetails.Tables[0].Merge(dsFiltered.Tables[0]);

            decimal decActualAmount = (decimal)dsDetails.Tables[0].Compute("Sum(InstallmentAmount)", "NoofDays >=0 ");
            decimal decbalamt;
            if (decActualAmount < decTotalAmt)
            {
                decbalamt = Convert.ToInt64(decTotalAmt) - decActualAmount;
                dsFiltered.Tables[0].Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dsFiltered.Tables[0].Rows[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
            }
            else if (decActualAmount > decTotalAmt)
            {
                decbalamt = decActualAmount - Convert.ToInt64(decTotalAmt);
                dsFiltered.Tables[0].Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dsFiltered.Tables[0].Rows[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
            }

            dsDetails.Tables[0].Merge(dsFiltered.Tables[0]);


            return FunPriGroupRepayDetails(dsDetails.Tables[0], 1, "Installment Amount", true, true, true, "");
        }

        public DataTable FunPubGetRevisedEmiDetails(DataTable dtOldEmiDetails, int intTenure, string strTenureType, decimal decFinanceAmount, decimal decRate, DateTime dtEffectivefromdate, decimal decRevisedRate, decimal decRevisedAmount, string stInter_Interest, decimal decRoundoff, out decimal decinterInterest)
        {
            decinterInterest = 0;
            if (dtOldEmiDetails.Columns.Contains("Installment_Number"))
                dtOldEmiDetails.PrimaryKey = new DataColumn[] { dtOldEmiDetails.Columns["Installment_Number"] };
            else
                dtOldEmiDetails.PrimaryKey = new DataColumn[] { dtOldEmiDetails.Columns["InstallmentNo"] };

            DataRow[] drFilteredRow;
            if (dtOldEmiDetails.Columns.Contains("Installment_Date"))
                drFilteredRow = dtOldEmiDetails.Select("Installment_Date >=#" + dtEffectivefromdate + "# AND BillStatus = false");
            else
                drFilteredRow = dtOldEmiDetails.Select("InstallmentDate >=#" + dtEffectivefromdate + "# AND BillStatus = false");

            DataSet dsFiltered = new DataSet();
            dsFiltered.Merge(drFilteredRow);


            foreach (DataRow drRow in dsFiltered.Tables[0].Rows)
            {
                if (decRoundoff == 0)
                    drRow["InstallmentAmount"] = Math.Round(Convert.ToDecimal(drRow["InstallmentAmount"].ToString()) + FunPubGetBalAmount(decRevisedAmount, decRevisedRate, intTenure, strTenureType, drFilteredRow.Length), 0);
                else
                    drRow["InstallmentAmount"] = Math.Round((Convert.ToDecimal(drRow["InstallmentAmount"].ToString()) + FunPubGetBalAmount(decRevisedAmount, decRevisedRate, intTenure, strTenureType, drFilteredRow.Length)) / decRoundoff, 0) * decRoundoff;
            }

            #region [//changes Done by Tamilselvan.S on 03/12/2011 for finance amount calculation]
            //decimal decTotalAmt = decRevisedAmount + Math.Round(FunPubInterestAmount(strTenureType, decRevisedAmount, decRate, intTenure), 0) + Math.Round(FunPubInterestAmount(strTenureType, decRevisedAmount, decRevisedRate, drFilteredRow.Length), 0);

            decimal decOldFinanceAmt = 0;
            if (dtOldEmiDetails.Columns.Contains("Installment_Number"))
                decOldFinanceAmt = (decimal)dtOldEmiDetails.Compute("Sum(FinanceCharges)", "Installment_Number>0");
            else
                decOldFinanceAmt = (decimal)dtOldEmiDetails.Compute("Sum(FinanceCharges)", "InstallmentNo>0");

            //decimal decTotalAmt = decRevisedAmount + Math.Round(decOldFinanceAmt, 0) + Math.Round(FunPubInterestAmount(strTenureType, decRevisedAmount, decRevisedRate, drFilteredRow.Length), 0);
            decimal decTotalAmt = decRevisedAmount + Math.Round(decOldFinanceAmt, 0) + FunPubInterestAmount(strTenureType, decRevisedAmount, decRevisedRate, drFilteredRow.Length);

            #endregion

            DataSet dsDetails = new DataSet();
            dsDetails.Merge(dtOldEmiDetails);
            dsDetails.Tables[0].Merge(dsFiltered.Tables[0]);
            decimal decActualAmount;
            if (dtOldEmiDetails.Columns.Contains("Installment_Number"))
                decActualAmount = (decimal)dsDetails.Tables[0].Compute("Sum(InstallmentAmount)", "Installment_Number >0 ");
            else
                decActualAmount = (decimal)dsDetails.Tables[0].Compute("Sum(InstallmentAmount)", "InstallmentNo >0 ");
            decimal decbalamt;
            if (decActualAmount < decTotalAmt)
            {
                decbalamt = Convert.ToInt64(decTotalAmt) - decActualAmount;
                dsFiltered.Tables[0].Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dsFiltered.Tables[0].Rows[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
            }
            else if (decActualAmount > decTotalAmt)
            {
                decbalamt = decActualAmount - Convert.ToInt64(decTotalAmt);
                dsFiltered.Tables[0].Rows[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(dsFiltered.Tables[0].Rows[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
            }
            //Bug ID 3297 Intervening Period Interest
            if (stInter_Interest.ToUpper() == "TRUE")
            {

                DataRow[] drInterveningRow = new DataRow[1];
                if (dtOldEmiDetails.Columns.Contains("Installment_Date"))
                    drInterveningRow = dtOldEmiDetails.Select("Installment_Date >=#" + dtEffectivefromdate + "# AND BillStatus = true");
                if (drInterveningRow.Length > 0)
                {
                    DataSet dsInterveningFiltered = new DataSet();
                    dsInterveningFiltered.Merge(drInterveningRow);
                    int intGapDays = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Day, dtEffectivefromdate, Convert.ToDateTime(dsInterveningFiltered.Tables[0].Rows[0]["Installment_Date"].ToString()), FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1));

                    //decimal decInterInt = Math.Round(FunPubInterestAmount("days", decRevisedAmount, decRevisedRate, intGapDays), 0);
                    //Changed the previous revised rate into difference between current rate and rev rate for bug 3725
                    decimal decInterInt = FunPubInterestAmount("days", decRevisedAmount, decRevisedRate, intGapDays);
                    decinterInterest = decInterInt;
                    S3GLogger.LogMessage("HERE WHEN Intervening is yes " + intGapDays.ToString() + "," + decInterInt.ToString() + "," + dsFiltered.Tables[0].Rows[0]["InstallmentAmount"], "FunPubGetRevisedEmiDetails");
                    dsFiltered.Tables[0].Rows[0]["InstallmentAmount"] = ((decimal)dsFiltered.Tables[0].Rows[0]["InstallmentAmount"]) + decInterInt;
                    S3GLogger.LogMessage("HERE WHEN After Update" + intGapDays.ToString() + "," + decInterInt.ToString() + "," + dsFiltered.Tables[0].Rows[0]["InstallmentAmount"], "FunPubGetRevisedEmiDetails");
                }

            }
            if (Convert.ToDateTime(dsFiltered.Tables[0].Rows[0]["FromDate"].ToString()) < dtEffectivefromdate)
            {
                int intAdjustmentGapDays = Convert.ToInt32(DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(dsFiltered.Tables[0].Rows[0]["FromDate"].ToString()), dtEffectivefromdate, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1));

                decimal decAdjusmentInterest = FunPubInterestAmount("days", decRevisedAmount, decRevisedRate, intAdjustmentGapDays);

                dsFiltered.Tables[0].Rows[0]["InstallmentAmount"] = ((decimal)dsFiltered.Tables[0].Rows[0]["InstallmentAmount"]) + decAdjusmentInterest;
            }
            dsDetails.Tables[0].Merge(dsFiltered.Tables[0]);


            return FunPriGroupRepayDetails(dsDetails.Tables[0], 1, "Installment Amount", true, true, true, "");
        }
        #endregion


        #region Moratorium


        /// <summary>
        /// To get repayment structure with Moratorium Logic for Noraml Cases 
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="intTenure"></param>
        /// <param name="strTenureType"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decRateofInt"></param>
        /// <param name="returnType"></param>
        /// <param name="decresidualValue"></param>
        /// <param name="dtimeStartDate"></param>
        /// <param name="intCashFlow"></param>
        /// <param name="strCashflowDesc"></param>
        /// <param name="strTimeVal"></param>
        /// <returns></returns>
        /// 

        public DataTable FunGetMoratoriumRepayStructure(DataTable dtRepaymentDetails, string strFrequency, int intTenure, string strTenureType, decimal decPrincipleAmount, decimal decRateofInt, RepaymentType returnType, decimal? decresidualValue, DateTime dtimeStartDate, DateTime dtimeDocDate, decimal decRoundOff, out bool blnIsRounded, string strTimeVal, DataTable dtMoratoriumInput)
        {
            #region For Moratorium Logic

            string MoraStDate = dtMoratoriumInput.Rows[0]["Fromdate"].ToString();// DateTime.Now.AddDays(61).ToString("MM/dd/yyyy");
            string MoraEndDate = dtMoratoriumInput.Rows[0]["Todate"].ToString(); //Convert.ToDateTime(MoraStDate).AddDays(45).ToString("MM/dd/yyyy");
            if (dtMoratoriumInput.Rows[0]["Moratoriumtype"].ToString().ToUpper() != "INTEREST")
            {

                blnIsRounded = false;
                decimal decInterestAmount = 0;
                //Intrest amount 
                //decInterestAmount = Math.Round(FunPubInterestAmount(strTenureType, decPrincipleAmount, decRateofInt, intTenure), 0);
                decInterestAmount = FunPubInterestAmount(strTenureType, decPrincipleAmount, decRateofInt, intTenure);

                decimal decTotalAmt = decPrincipleAmount;
                decimal decPerInstallmentAmt = 0;
                int intLoopCount = 0;

                DataTable DtMoraSt = dtRepaymentDetails.Clone();
                //DataTable DtMoraEnd = null;
                DataTable DtMora = null;


                if (dtRepaymentDetails.Select("InstallmentDate<='" + MoraStDate + "' ").Length > 0)
                    DtMoraSt = dtRepaymentDetails.Select("InstallmentDate<='" + MoraStDate + "' ").CopyToDataTable();

                if (dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").Length > 0)
                    DtMora = dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").CopyToDataTable();

                if (dtRepaymentDetails.Select("InstallmentDate>='" + MoraEndDate + "' ").Length > 0)
                {
                    if (DtMoraSt != null)
                        DtMoraSt.Merge(dtRepaymentDetails.Select("InstallmentDate>='" + MoraEndDate + "'").CopyToDataTable());
                    else
                        DtMoraSt = dtRepaymentDetails.Select("InstallmentDate>='" + MoraEndDate + "'").CopyToDataTable().Copy();
                }


                if (DtMora != null && DtMora.Rows.Count > 0)
                {
                    foreach (DataRow drRepaymentRow in DtMora.Rows)
                    {
                        drRepaymentRow["InstallmentAmount"] = 0;
                        drRepaymentRow["Amount"] = 0;
                        drRepaymentRow.AcceptChanges();
                    }
                }

                intLoopCount = 0;
                #region Loop for arriving per installment amount
                intLoopCount = DtMoraSt.Rows.Count;

                if (DtMoraSt.Select("FromDate='" + Convert.ToDateTime(MoraStDate) + "' and Todate='" + Convert.ToDateTime(MoraEndDate) + "'").Length > 0)
                {
                    intLoopCount = intLoopCount - 1;
                }


                if (intLoopCount > 0)
                {
                    switch (returnType)
                    {
                        case RepaymentType.FC:
                        case RepaymentType.WC:
                        case RepaymentType.TLE:
                            decTotalAmt = decInterestAmount + decTotalAmt;
                            decPerInstallmentAmt = decTotalAmt;
                            break;
                        case RepaymentType.EMI:
                            decTotalAmt = decInterestAmount + decTotalAmt;
                            decPerInstallmentAmt = decTotalAmt / intLoopCount;
                            break;
                        case RepaymentType.PMPT:
                            decPerInstallmentAmt = (decTotalAmt / 1000) * decRateofInt;
                            break;
                        case RepaymentType.PMPL:
                            decPerInstallmentAmt = (decTotalAmt / 100000) * decRateofInt;
                            break;
                        case RepaymentType.PMPM:
                            decPerInstallmentAmt = (decTotalAmt / 1000000) * decRateofInt;
                            break;

                    }

                    dtRepaymentDetails.Columns["InstallmentAmount"].DefaultValue = Math.Round(decPerInstallmentAmt, 0);
                    decimal decPerInstallAmt = 0;
                    int InstMentNo = 0;
                    if (decRoundOff != 0)
                    {
                        decPerInstallAmt = Math.Round((decPerInstallmentAmt / decRoundOff), 0) * decRoundOff;
                    }
                    else
                    {
                        decPerInstallAmt = decPerInstallmentAmt;
                    }

                    foreach (DataRow drRepaymentRow in DtMoraSt.Rows)
                    {
                        drRepaymentRow["InstallmentAmount"] = decPerInstallAmt;
                        drRepaymentRow["Amount"] = Math.Round(decTotalAmt, 0);
                    }

                    #region Moratorium days

                    if (DtMora != null && DtMora.Rows.Count > 0)
                    {
                        DataRow drMora = DtMoraSt.NewRow();
                        drMora["NoofDays"] = 1;
                        drMora["InstallmentNo"] = "1";
                        if (strTimeVal.Contains("adv"))
                        {
                            if (DtMora != null && DtMora.Rows.Count > 0)
                                drMora["FromDate"] = Convert.ToDateTime(MoraEndDate);
                            //else
                            //    drMora["FromDate"] = Convert.ToDateTime(MoraStDate);

                        }
                        else
                        {
                            if (DtMora != null && DtMora.Rows.Count > 0)
                                drMora["FromDate"] = Convert.ToDateTime(DtMora.Rows[DtMora.Rows.Count - 1]["ToDate"].ToString());// DateTime.Now;                                                
                            //else
                            //    drMora["FromDate"] = Convert.ToDateTime(MoraStDate);
                        }

                        drMora["ToDate"] = Convert.ToDateTime(MoraEndDate); // DateTime.Now.AddDays(45);
                        drMora["InstallmentDate"] = MoraEndDate;
                        drMora["InstallmentAmount"] = 0;
                        drMora["Amount"] = 0;
                        drMora["CurrentBalance"] = "0.00";

                        DtMoraSt.Rows.Add(drMora);
                    }
                    #endregion

                    if (DtMora != null && DtMora.Rows.Count > 0)
                        DtMoraSt.Merge(DtMora);

                    DataView dvMoraSt = DtMoraSt.DefaultView;
                    dvMoraSt.Sort = "InstallmentDate ";
                    DtMoraSt = dvMoraSt.ToTable();

                    foreach (DataRow drRepaymentRow in DtMoraSt.Rows)
                    {
                        drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                        if (InstMentNo != 0)
                        {
                            drRepaymentRow["FromDate"] = Convert.ToDateTime(DtMoraSt.Rows[InstMentNo - 1]["ToDate"].ToString());
                        }
                        drRepaymentRow["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drRepaymentRow["FromDate"].ToString()), Convert.ToDateTime(drRepaymentRow["ToDate"].ToString()), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        InstMentNo++;
                    }

                    if (DtMora != null && DtMora.Rows.Count > 0)
                    {
                        DtMoraSt = DtMoraSt.Select("ToDate<>'" + Convert.ToDateTime(MoraEndDate) + "'").CopyToDataTable();
                    }
                    else
                    {
                        if (DtMoraSt.Select("FromDate='" + Convert.ToDateTime(MoraStDate) + "' and Todate='" + Convert.ToDateTime(MoraEndDate) + "'").Length > 0)
                        {
                            DtMoraSt.Select("FromDate='" + Convert.ToDateTime(MoraStDate) + "' and Todate='" + Convert.ToDateTime(MoraEndDate) + "'")[0]["InstallmentAmount"] = 0;
                        }
                    }

                    dvMoraSt = DtMoraSt.DefaultView;
                    dvMoraSt.Sort = "InstallmentDate ";
                    DtMoraSt = dvMoraSt.ToTable();

                    InstMentNo = 0;
                    DtMoraSt.Columns.Add("SlNo", System.Type.GetType("System.Int32"));
                    foreach (DataRow drRepaymentRow in DtMoraSt.Rows)
                    {
                        drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                        drRepaymentRow["SlNo"] = InstMentNo + 1;
                        InstMentNo++;
                    }

                }
                #endregion

                #region Check Round off impact and correct in 1st installment
                if (returnType != RepaymentType.PMPL && returnType != RepaymentType.PMPM && returnType != RepaymentType.PMPT)
                {
                    if (decRoundOff != 0)
                    {
                        decimal decActualAmount = (decimal)DtMoraSt.Compute("Sum(InstallmentAmount)", "NoofDays >=0 ");
                        decimal decbalamt;
                        if (decActualAmount < decTotalAmt)
                        {
                            blnIsRounded = true;
                            decbalamt = Convert.ToInt64(decTotalAmt) - decActualAmount;
                            DtMoraSt.Select("InstallmentAmount<>0")[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal((DtMoraSt.Select("InstallmentAmount<>0").CopyToDataTable()).Rows[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
                        }
                        else if (decActualAmount > decTotalAmt)
                        {
                            blnIsRounded = true;
                            decbalamt = decActualAmount - Convert.ToInt64(decTotalAmt);
                            DtMoraSt.Select("InstallmentAmount<>0")[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal((DtMoraSt.Select("InstallmentAmount<>0").CopyToDataTable()).Rows[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
                        }
                    }
                }
                #endregion

                return DtMoraSt;
            }
            else
            {
                blnIsRounded = true;
                int InstMentNo = 0;
                dtRepaymentDetails.Columns.Add("SlNo", System.Type.GetType("System.Int32"));
                foreach (DataRow drRepaymentRow in dtRepaymentDetails.Rows)
                {
                    drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                    drRepaymentRow["SlNo"] = InstMentNo + 1;
                    InstMentNo++;
                }
                return dtRepaymentDetails;
            }

            #endregion
        }




        /// <summary>
        /// To get repayment structure with Moratorium and Recovery Pattern Logic for Noraml Cases 
        /// </summary>
        /// <param name="strFrequency"></param>
        /// <param name="intTenure"></param>
        /// <param name="strTenureType"></param>
        /// <param name="decPrincipleAmount"></param>
        /// <param name="decRateofInt"></param>
        /// <param name="returnType"></param>
        /// <param name="decresidualValue"></param>
        /// <param name="dtimeStartDate"></param>
        /// <param name="intCashFlow"></param>
        /// <param name="strCashflowDesc"></param>
        /// <param name="strTimeVal"></param>
        /// <returns></returns>
        public DataTable FunGetMoratoriumRepayStructure(DataTable dtRepaymentDetails, string strFrequency, int intTenure, string strTenureType, decimal decPrincipleAmount, decimal decRateofInt, RepaymentType returnType, decimal? decresidualValue, DateTime dtimeStartDate, DateTime dtimeDocDate, decimal decRoundOff, out bool blnIsRounded, string strTimeVal, bool blnRecovery, DataTable dtMoratoriumInput, params decimal[] decRecoveryPattern)
        {
            #region For Moratorium Logic

            blnIsRounded = false;
            decimal decInterestAmount = 0;

            string MoraStDate = dtMoratoriumInput.Rows[0]["Fromdate"].ToString();// DateTime.Now.AddDays(61).ToString("MM/dd/yyyy");
            string MoraEndDate = dtMoratoriumInput.Rows[0]["Todate"].ToString(); //Convert.ToDateTime(MoraStDate).AddDays(45).ToString("MM/dd/yyyy");
            if (dtMoratoriumInput.Rows[0]["Moratoriumtype"].ToString().ToUpper() != "INTEREST")
            {

                //Intrest amount 
                //decInterestAmount = Math.Round(FunPubInterestAmount(strTenureType, decPrincipleAmount, decRateofInt, intTenure), 0);
                decInterestAmount = FunPubInterestAmount(strTenureType, decPrincipleAmount, decRateofInt, intTenure);

                decimal decTotalAmt = decPrincipleAmount;
                decimal decPerInstallmentAmt = 0;
                int intLoopCount = 0;

                DataTable DtMoraSt = dtRepaymentDetails.Clone();
                //DataTable DtMoraEnd = null;
                DataTable DtMora = null;


                if (dtRepaymentDetails.Select("InstallmentDate<='" + MoraStDate + "' ").Length > 0)
                    DtMoraSt = dtRepaymentDetails.Select("InstallmentDate<='" + MoraStDate + "' ").CopyToDataTable();

                if (dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").Length > 0)
                    DtMora = dtRepaymentDetails.Select("InstallmentDate>'" + MoraStDate + "' and InstallmentDate<'" + MoraEndDate + "' ").CopyToDataTable();

                if (dtRepaymentDetails.Select("InstallmentDate>='" + MoraEndDate + "' ").Length > 0)
                {
                    if (DtMoraSt != null)
                        DtMoraSt.Merge(dtRepaymentDetails.Select("InstallmentDate>='" + MoraEndDate + "'").CopyToDataTable());
                    else
                        DtMoraSt = dtRepaymentDetails.Select("InstallmentDate>='" + MoraEndDate + "'").CopyToDataTable().Copy();
                }

                if (DtMora != null && DtMora.Rows.Count > 0)
                {
                    foreach (DataRow drRepaymentRow in DtMora.Rows)
                    {
                        drRepaymentRow["InstallmentAmount"] = 0;
                        drRepaymentRow["Amount"] = 0;
                        drRepaymentRow.AcceptChanges();
                    }
                }

                intLoopCount = 0;
                #region Loop for arriving per installment amount
                intLoopCount = DtMoraSt.Rows.Count;
                if (DtMoraSt.Select("FromDate='" + Convert.ToDateTime(MoraStDate) + "' and Todate='" + Convert.ToDateTime(MoraEndDate) + "'").Length > 0)
                {
                    intLoopCount = intLoopCount - 1;
                }
                //decimal decPerInstallAmt = 0;
                int InstMentNo = 0;

                if (intLoopCount > 0)
                {
                    DateTime datStructDate;

                    #region Finding Installment amount based on Recovery years
                    datStructDate = Convert.ToDateTime(DtMoraSt.Rows[0]["FromDate"].ToString());

                    /* Calculate Total Repay amount for PMPT/PMPL/PMPM 
                     * (**For this Repayment type Tenure should be Month)  */
                    switch (returnType)
                    {
                        case RepaymentType.PMPT:
                            decTotalAmt = (decTotalAmt / 1000) * decRateofInt * intTenure;
                            break;
                        case RepaymentType.PMPL:
                            decTotalAmt = (decTotalAmt / 100000) * decRateofInt * intTenure;
                            break;
                        case RepaymentType.PMPM:
                            decTotalAmt = (decTotalAmt / 1000000) * decRateofInt * intTenure;
                            break;
                        default:
                            decTotalAmt = decTotalAmt + decInterestAmount;
                            break;
                    }

                    for (int intRecoveryLoopCount = 0; intRecoveryLoopCount < decRecoveryPattern.Length; intRecoveryLoopCount++)
                    {
                        //if (Convert.ToDecimal(decRecoveryPattern[intRecoveryLoopCount].ToString()) != 0)
                        //{
                        decimal InstallAmount = 0;
                        //if (!blnRecovery) // OL & FL with Repayment Mode Structured
                        //{
                        if (Convert.ToDecimal(decRecoveryPattern[intRecoveryLoopCount]) != 0)
                        {
                            InstallAmount = (decTotalAmt * decRecoveryPattern[intRecoveryLoopCount]) / 100;
                        }
                        else
                        {
                            InstallAmount = 0;
                        }
                        //}
                        //else
                        //{
                        //    switch (returnType)
                        //    {
                        //        case RepaymentType.PMPT:
                        //            InstallAmount = (decTotalAmt / 100) * decRecoveryPattern[intRecoveryLoopCount];
                        //            break;

                        //        case RepaymentType.PMPL:
                        //            InstallAmount = (decTotalAmt / 100) * decRecoveryPattern[intRecoveryLoopCount];
                        //            break;
                        //        case RepaymentType.PMPM:
                        //            InstallAmount = (decTotalAmt / 100) * decRecoveryPattern[intRecoveryLoopCount];
                        //            break;
                        //    }
                        //}

                        if (decRoundOff != 0)
                        {
                            DataRow[] drRepaymentRow;
                            if (intRecoveryLoopCount == decRecoveryPattern.Length - 1)
                                drRepaymentRow = DtMoraSt.Select("InstallmentDate >= #" + datStructDate + "#");
                            else
                                drRepaymentRow = DtMoraSt.Select("InstallmentDate >= #" + datStructDate + "# and  InstallmentDate <#" + datStructDate.AddYears(1) + "#");

                            decimal decActualAmount = 0;
                            if (drRepaymentRow.Length > 0)
                            {
                                intLoopCount = drRepaymentRow.Length;
                                //if (!blnRecovery)
                                decPerInstallmentAmt = (InstallAmount / intLoopCount);
                                //else
                                //    decPerInstallmentAmt = InstallAmount;

                                for (int i = 0; i < drRepaymentRow.Length; i++)
                                {
                                    drRepaymentRow[i]["InstallmentAmount"] = Math.Round((decPerInstallmentAmt / decRoundOff), 0) * decRoundOff;
                                    decActualAmount += Math.Round((decPerInstallmentAmt / decRoundOff), 0) * decRoundOff;
                                    drRepaymentRow[i]["Amount"] = InstallAmount;
                                    drRepaymentRow[i].AcceptChanges();
                                }

                            }
                            if (!blnRecovery) // OL & FL with Repayment Mode Structured
                            {
                                decimal decbalamt;
                                if (decActualAmount < InstallAmount)
                                {
                                    blnIsRounded = true;
                                    decbalamt = Convert.ToInt64(InstallAmount) - decActualAmount;
                                    drRepaymentRow[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(drRepaymentRow[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
                                    drRepaymentRow[0].AcceptChanges();
                                }
                                else if (decActualAmount > InstallAmount)
                                {
                                    blnIsRounded = true;
                                    decbalamt = decActualAmount - Convert.ToInt64(InstallAmount);
                                    drRepaymentRow[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal(drRepaymentRow[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
                                    drRepaymentRow[0].AcceptChanges();
                                }
                            }
                        }
                        //}
                        datStructDate = datStructDate.AddYears(1);
                    }
                    #endregion

                    #region Moratorium days
                    if (DtMora != null && DtMora.Rows.Count > 0)
                    {
                        DataRow drMora = DtMoraSt.NewRow();
                        drMora["NoofDays"] = 1;
                        drMora["InstallmentNo"] = "1";

                        if (strTimeVal.Contains("adv"))
                        {
                            if (DtMora != null && DtMora.Rows.Count > 0)
                                drMora["FromDate"] = Convert.ToDateTime(MoraEndDate);
                            //else
                            //    drMora["FromDate"] = Convert.ToDateTime(MoraStDate);
                        }
                        else
                        {
                            if (DtMora != null && DtMora.Rows.Count > 0)
                                drMora["FromDate"] = Convert.ToDateTime(DtMora.Rows[DtMora.Rows.Count - 1]["ToDate"].ToString());// DateTime.Now;                                                
                            //else
                            //    drMora["FromDate"] = Convert.ToDateTime(MoraStDate);
                        }
                        drMora["ToDate"] = Convert.ToDateTime(MoraEndDate);
                        drMora["InstallmentDate"] = MoraEndDate;
                        drMora["InstallmentAmount"] = 0;
                        drMora["Amount"] = 0;
                        drMora["CurrentBalance"] = "0.00";

                        DtMoraSt.Rows.Add(drMora);
                    }
                    #endregion
                    if (DtMora != null && DtMora.Rows.Count > 0)
                        DtMoraSt.Merge(DtMora);

                    DataView dvMoraSt = DtMoraSt.DefaultView;
                    dvMoraSt.Sort = "InstallmentDate ";
                    DtMoraSt = dvMoraSt.ToTable();

                    foreach (DataRow drRepaymentRow in DtMoraSt.Rows)
                    {
                        drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                        if (InstMentNo != 0)
                        {
                            drRepaymentRow["FromDate"] = Convert.ToDateTime(DtMoraSt.Rows[InstMentNo - 1]["ToDate"].ToString());
                        }
                        drRepaymentRow["NoofDays"] = DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(drRepaymentRow["FromDate"].ToString()), Convert.ToDateTime(drRepaymentRow["ToDate"].ToString()), Microsoft.VisualBasic.FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1);
                        InstMentNo++;
                    }
                    if (DtMora != null && DtMora.Rows.Count > 0)
                    {
                        DtMoraSt = DtMoraSt.Select("ToDate<>'" + Convert.ToDateTime(MoraEndDate) + "'").CopyToDataTable();
                    }
                    else
                    {
                        if (DtMoraSt.Select("FromDate='" + Convert.ToDateTime(MoraStDate) + "' and Todate='" + Convert.ToDateTime(MoraEndDate) + "'").Length > 0)
                        {
                            DtMoraSt.Select("FromDate='" + Convert.ToDateTime(MoraStDate) + "' and Todate='" + Convert.ToDateTime(MoraEndDate) + "'")[0]["InstallmentAmount"] = 0;
                        }
                    }

                    dvMoraSt = DtMoraSt.DefaultView;
                    dvMoraSt.Sort = "InstallmentDate";
                    DtMoraSt = dvMoraSt.ToTable();

                    InstMentNo = 0;
                    DtMoraSt.Columns.Add("SlNo", System.Type.GetType("System.Int32"));
                    foreach (DataRow drRepaymentRow in DtMoraSt.Rows)
                    {
                        drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                        drRepaymentRow["SlNo"] = InstMentNo + 1;
                        InstMentNo++;
                    }
                }
                #endregion

                #region Check Round off impact and correct in 1st installment
                if (returnType != RepaymentType.PMPL && returnType != RepaymentType.PMPM && returnType != RepaymentType.PMPT)
                {
                    if (decRoundOff != 0)
                    {
                        decimal decActualAmount = (decimal)DtMoraSt.Compute("Sum(InstallmentAmount)", "NoofDays >=0 ");
                        decimal decbalamt;
                        if (decActualAmount < decTotalAmt)
                        {
                            blnIsRounded = true;
                            decbalamt = Convert.ToInt64(decTotalAmt) - decActualAmount;
                            DtMoraSt.Select("InstallmentAmount<>0")[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal((DtMoraSt.Select("InstallmentAmount<>0").CopyToDataTable()).Rows[0]["InstallmentAmount"].ToString()) + decbalamt, 0);
                        }
                        else if (decActualAmount > decTotalAmt)
                        {
                            blnIsRounded = true;
                            decbalamt = decActualAmount - Convert.ToInt64(decTotalAmt);
                            DtMoraSt.Select("InstallmentAmount<>0")[0]["InstallmentAmount"] = Math.Round(Convert.ToDecimal((DtMoraSt.Select("InstallmentAmount<>0").CopyToDataTable()).Rows[0]["InstallmentAmount"].ToString()) - decbalamt, 0);
                        }
                    }
                }
                #endregion

                return DtMoraSt;
            }
            else
            {
                blnIsRounded = true;
                int InstMentNo = 0;
                dtRepaymentDetails.Columns.Add("SlNo", System.Type.GetType("System.Int32"));
                foreach (DataRow drRepaymentRow in dtRepaymentDetails.Rows)
                {
                    drRepaymentRow["InstallmentNo"] = (InstMentNo + 1).ToString();
                    drRepaymentRow["SlNo"] = InstMentNo + 1;
                    InstMentNo++;
                }
                return dtRepaymentDetails;
            }

            #endregion
        }

        #endregion

        public static int intGPSRoundOff;
        public static int GPSRoundOff
        {
            get
            {
                return intGPSRoundOff;
            }
            set
            {
                intGPSRoundOff = value;
            }
        }


    }

    public enum IRRType
    {
        Accounting_IRR,
        Business_IRR,
        Company_IRR
    }

    public enum RepaymentType
    {
        EMI,
        PMPT,
        PMPL,
        PMPM,
        WC,
        FC,
        TLE,
        Others,
        StructureFixed
    }


}

