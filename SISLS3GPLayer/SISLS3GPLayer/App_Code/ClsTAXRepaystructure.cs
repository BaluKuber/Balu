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
/// Summary description for ClsTAXRepaystructure
/// this class is used to calculate TAX for the repayment structure from TAX Master in Finacial Accounting
/// 
/// </summary>
public class ClsTAXRepaystructure
{
    /// <summary>
    /// This is used to calculate TAX for the repayment structure from TAX Master in Finacial Accounting
    /// </summary>
    /// <param name="dtRepaystruct"></param>
    /// <param name="objMethodParameters"></param>
    /// <returns></returns>
    public DataTable FunGetTaxReapystructure(DataTable dtRepaystruct, Dictionary<string, string> objMethodParameters)
    {
        DataTable dtRepaystructNew = new DataTable();
        try
        {
            int intCompany_Id = 0, intStartMonth = 4, intEndMonth = 3;
            if (!string.IsNullOrEmpty(objMethodParameters["Company_Id"].ToString()))
                intCompany_Id = Convert.ToInt32(objMethodParameters["Company_Id"].ToString());

            dtRepaystructNew = dtRepaystruct.Copy();
            //Adding new column For Tax
            dtRepaystructNew.Columns.Add("Tax_Section");
            dtRepaystructNew.Columns.Add("TaxPercentage", typeof(decimal));
            dtRepaystructNew.Columns.Add("TaxAmount", typeof(decimal));
            dtRepaystructNew.Columns.Add("Year");
            dtRepaystructNew.Columns.Add("UMFCAmount", typeof(decimal));
            if (!dtRepaystructNew.Columns.Contains("Tax"))
            dtRepaystructNew.Columns.Add("Tax", typeof(decimal));
            #region "Yearly UMFC"
            System.Configuration.AppSettingsReader AppReader = new System.Configuration.AppSettingsReader();
            intStartMonth = (int)AppReader.GetValue("StartMonth", typeof(int));
            decimal intTotUMFC = 0;
            for (int i = 0; i <= dtRepaystructNew.Rows.Count - 1; i++)
            {
                int intCurrentMonth = Utility.StringToDate(dtRepaystructNew.Rows[i]["Installment_Date"].ToString()).Month;
                int intCurrentYear = Utility.StringToDate(dtRepaystructNew.Rows[i]["Installment_Date"].ToString()).Year;
                if (intCurrentMonth >= intStartMonth)
                {
                    intCurrentYear = intCurrentYear + 1;
                }
                dtRepaystructNew.Rows[i]["Year"] = intCurrentYear.ToString();

                
            }


            for (int i = 0; i <= dtRepaystructNew.Rows.Count - 1; i++)
            {
                if (i > 0)
                    if (!string.Equals(dtRepaystructNew.Rows[i - 1]["Year"].ToString(), dtRepaystructNew.Rows[i]["Year"].ToString()))
                    {
                        dtRepaystructNew.Rows[i - 1]["UMFCAmount"] = intTotUMFC;
                        intTotUMFC = 0;
                    }
                intTotUMFC += Convert.ToDecimal(dtRepaystructNew.Rows[i]["Charge"].ToString());
                //For Last Row
                if (i == dtRepaystructNew.Rows.Count - 1)
                {
                    dtRepaystructNew.Rows[i]["UMFCAmount"] = intTotUMFC;
                }
            }


            #endregion

            #region "Getting Tax Percentage"
            DataTable dtTDSMaster = FunPubGetTAXdetails(intCompany_Id);
            #endregion

            if (dtTDSMaster.Rows.Count > 0)//If TDS defined in Financial accounting
            {
                DataRow[] drRepaystruct = dtRepaystructNew.Select("UMFCAmount > 0");
                DataRow[] drRepaystructOther = dtRepaystructNew.Select("UMFCAmount is null");


                foreach (DataRow drrepay in drRepaystruct)
                {
                    decimal Amount = Convert.ToDecimal(drrepay["UMFCAmount"]);
                    DateTime dtinstalldate = Utility.StringToDate(drrepay["Installment_Date"].ToString());
                    string strFilterExp = "Start_Date <= '" + dtinstalldate.ToString() + "'  and End_Date >='" + dtinstalldate.ToString() + "'";
                    string strFilterExpAmt = "From_Amount <= " + Amount.ToString() + "  and TO_Amount >=" + Amount.ToString() + "";
                    DataRow[] dtTDSRow = dtTDSMaster.Select(strFilterExp + " and " + strFilterExpAmt);
                    if (dtTDSRow.Length > 0)
                    {
                        drrepay["Tax_Section"] = dtTDSRow[0]["Tax_Section"].ToString();
                        drrepay["TaxPercentage"] = dtTDSRow[0]["Tax"].ToString();

                        foreach (DataRow drrepayOther in drRepaystructOther)
                        {
                            if (drrepayOther["Year"].ToString() == drrepay["Year"].ToString())
                            {
                                drrepayOther["Tax_Section"] = drrepay["Tax_Section"].ToString();
                                drrepayOther["TaxPercentage"] = drrepay["TaxPercentage"].ToString();
                            }
                        }
                    }
                }

                for(int i=0;i<=dtRepaystructNew.Rows.Count-1;i++)
                {
                    if (!string.IsNullOrEmpty(dtRepaystructNew.Rows[i]["TaxPercentage"].ToString()))
                    {
                        decimal decTaxPer = Convert.ToDecimal(dtRepaystructNew.Rows[i]["TaxPercentage"].ToString());
                        decimal decCharges = Convert.ToDecimal(dtRepaystructNew.Rows[i]["Charge"].ToString());
                        dtRepaystructNew.Rows[i]["Tax"] = decCharges * (decTaxPer / 100);
                    }
                }

            }





        }
        catch (Exception ex)
        {
            ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
            throw ex;
        }

        return dtRepaystructNew;
    }

    /// <summary>
    /// This function is used to get tax details from TDS master from Financial Accounting
    /// </summary>
    /// <param name="intCompanyId"></param>
    /// <returns></returns>
    public DataTable FunPubGetTAXdetails(int intCompanyId)
    {
        Dictionary<string, string> objProcedureParameter = new Dictionary<string, string>();
        objProcedureParameter.Add("@Company_ID", intCompanyId.ToString());
        DataTable dtTDSMaster = Utility.GetDefaultData("S3G_Get_TaxDtls_BW", objProcedureParameter);
        return dtTDSMaster;
    }


}
