
#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using CashflowMgtServicesReference;
using S3GBusEntity.Origination;
using S3GBusEntity;
#endregion



public partial class Origination_NegativeListCustomers : ApplyThemeForProject
{
    
    CashflowMgtServicesClient ObjCashFlowClient = null;
    CashflowMgtServices.S3G_ORG_NegCustListDataTable ObjNegCustListDataTable = new CashflowMgtServices.S3G_ORG_NegCustListDataTable();
    CashflowMgtServices.S3G_ORG_NegCustListRow ObjNegCustListRow = null;
     SerializationMode SerMode = SerializationMode.Binary;
    int intCashFlowID = 0;
    int Neg_ID=0;
    int intErrCode = 0;
    int intCompanyId;
    int intUserId;
    UserInfo ObjUserInfo = new UserInfo();
    StringBuilder sbLoadingXML = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        //StatusMessageLbl.Visible = false;
    }

    //protected void Uploadbtn_Click(object sender, EventArgs e)
    //{
    //     DataSet ds = new DataSet();
    //   // string path = fupXML.FileName;
    //    ds.ReadXml("C:\\Users\\008417\\Desktop\\Aqlist.xml");
    //    Grid1.DataSource = ds.Tables[0];
    //    Grid1.DataBind();
    //}
    protected void Uploadbtn_Click(object sender, EventArgs e)
    {


        //StatusMessageLbl.Visible = true;



        try
        {




            if (FileUpload.HasFile)
            {




                string path = FileUpload.FileName;
                DataSet ds = new DataSet();


                FileUpload.SaveAs(Server.MapPath("." + path));



                DataTable dt = new DataTable();
                //dt.Columns.Add("INDIVIDUAL_ID");
                dt.Columns.Add("INDIVIDUAL_ID");
                dt.Columns.Add("FIRST_NAME");
                dt.Columns.Add("SECOND_NAME");
                //dt.Columns.Add("QUALITY");
                ;
                dt.Columns.Add("NOTE");
                dt.Columns.Add("NUMBER");

                //dt.Columns.Add("TYPE_OF_DOCUMENT");

                ds.ReadXml(Server.MapPath("." + path));
                XmlDocument docxml = new XmlDocument();
                docxml.Load(Server.MapPath("." + path));
                XmlNodeList nodeOneList = docxml.GetElementsByTagName("INDIVIDUAL");



                int i = 1;
                foreach (XmlNode node in nodeOneList)
                {
                    dictparam = new Dictionary<string, string>();
                    dictparam.Add("Id", i.ToString());
                    FunNodeloop(node, i.ToString());
                    DataRow dr = dt.NewRow();
                    foreach (KeyValuePair<string, string> k in dictparam)
                    {
                        if (k.Key == "Id")
                            dr["INDIVIDUAL_ID"] = k.Value;

                        if (k.Key == "FIRST_NAME")
                            dr["FIRST_NAME"] = k.Value;

                        if (k.Key == "NUMBER")
                            dr["NUMBER"] = k.Value;

                        if (k.Key == "NOTE")
                            dr["NOTE"] = k.Value;


                    }
                    dt.Rows.Add(dr);
                    i++;

                }
                ViewState["dtCustomers"] = dt;
                Grid1.DataSource = dt;
                Grid1.DataBind();


            }
            else
            {
                Utility.FunShowAlertMsg(this, "Please upload a xml file");
                return;
            }
         

                ObjNegCustListRow = ObjNegCustListDataTable.NewS3G_ORG_NegCustListRow();
                //ObjNegCustListRow. = intCashFlowID;
                sbLoadingXML = new StringBuilder();
                ObjNegCustListRow.Company_id = ObjUserInfo.ProCompanyIdRW.ToString();
                ObjNegCustListRow.User_Id = ObjUserInfo.ProUserIdRW.ToString();

                
                ObjNegCustListRow.StrCustomerList = FunPriLoadingXML();
                string strCustomerList = sbLoadingXML.ToString();
                ObjCashFlowClient = new CashflowMgtServicesClient();

                //Dictionary<string, string> Procparam;
                //Procparam = new Dictionary<string, string>();
                //Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
                //Procparam.Add("@UserId", ObjUserInfo.ProUserIdRW.ToString());
                //Procparam.Add("@strCustomerList", strCustomerList.ToString());
                //Procparam.Add("@ErrorCode", strCustomerList.ToString());

                ObjNegCustListDataTable.AddS3G_ORG_NegCustListRow(ObjNegCustListRow);

                // DataSet dssa = Utility.GetDefaultData("S3G_ORG_NegativeListCustomers", Procparam);

                if (intCashFlowID == 0)
                {
                    intErrCode = ObjCashFlowClient.FunPubCreateCashFlowRulesInt(SerMode, ClsPubSerialize.Serialize(ObjNegCustListDataTable, SerMode));
                    if (intErrCode == 0)
                    {
                        Utility.FunShowAlertMsg(this, "Details added successfully");
                    }
                }
         


            }
            catch (IOException ex)
            {
                Utility.FunShowAlertMsg(this, "unable to find the path");
            }
    }

        
    

    


   
   
    private void FunNodeloop(XmlNode e, string id)
    {
        try
        {

            for (int i = 0; i < e.ChildNodes.Count; i++)
            {
                if (e.ChildNodes[i].ChildNodes.Count > 0)
                    FunNodeloop(e.ChildNodes[i], id);
                if (e.ChildNodes[i].Name == "FIRST_NAME")
                {
                    //Response.Write(e.ChildNodes[i].InnerText);
                    //FunAddcolumn(id, e.ChildNodes[i].Name, e.ChildNodes[i].InnerText);
                    dictparam.Add(e.ChildNodes[i].Name, e.ChildNodes[i].InnerText);
                }

                if (e.ChildNodes[i].Name == "SECOND_NAME")
                {
                    dictparam.Add(e.ChildNodes[i].Name, e.ChildNodes[i].InnerText);

                }
           
                if (e.ChildNodes[i].Name == "TYPE_OF_DOCUMENT")
                {
                    if (e.ChildNodes[i].InnerText.ToUpper() == "PASSPORT")
                    {
                        if (e.ChildNodes[i + 1].Name == "NUMBER")
                        {
                            //Response.Write(e.ChildNodes[i + 1].InnerText);
                            //FunAddcolumn(id, e.ChildNodes[i].Name, e.ChildNodes[i].InnerText);
                            dictparam.Add(e.ChildNodes[i+1].Name, e.ChildNodes[i + 1].InnerText);
                        }
                    }
                    else if (e.ChildNodes[i].InnerText.ToUpper() == "NATIONAL IDENTIFICATION NUMBER")
                    {
                        if (e.ChildNodes[i + 1].Name == "NOTE")
                        {
                            //Response.Write(e.ChildNodes[i + 1].InnerText);
                            //FunAddcolumn(id, e.ChildNodes[i].Name, e.ChildNodes[i].InnerText);
                            dictparam.Add(e.ChildNodes[i+1].Name, e.ChildNodes[i+1].InnerText);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    Dictionary<string, string> dictparam = new Dictionary<string, string>();
    private void FunAddcolumn(string Id, string Type, string value)
    {
       
           
    }
    //protected void FunPubInsertingDetails()
    //{
    //    objAdminService = new S3GAdminServicesReference.S3GAdminServicesClient();
    //    objList_DataTable = new S3GBusEntity.SystemAdmin.S3G_SYSAD_LocationCategoryDataTable();
    //    objNegList_DataRow.Company_ID = intCompanyId;
    //    objNegList_DataRow.Created_By = intUserId;

    //}
    
    //protected void FunSavingDetails()
    //{
    //    Dictionary<string, string> dictparam1 = new Dictionary<string, string>();
    //    dictparam1.Add("@")
    //}
    //public static string FunPubFormXml(DataTable DtXml)
    //{
    //    int intcolcount = 0;
    //    string strColValue = string.Empty;
    //    StringBuilder strbXml = new StringBuilder();
    //    strbXml.Append("<Root>");
    //    foreach (DataRow grvRow in DtXml.Rows)
    //    {
    //        intcolcount = 0;
    //        strbXml.Append(" <Details ");
    //        foreach (DataColumn dtCols in DtXml.Columns)
    //        {
    //            strColValue = grvRow.ItemArray[intcolcount].ToString();
    //            strColValue = strColValue.Replace("&", "").Replace("<", "").Replace(">", "");
    //            strColValue = strColValue.Replace("'", "\"");
    //            if (!string.IsNullOrEmpty(strColValue))
    //            {
    //                if (grvRow.ItemArray[intcolcount].ToString() != "" || dtCols.ColumnName != string.Empty)
    //                {

    //                    if (dtCols.ColumnName.ToUpper().Contains("DATE"))
    //                        strbXml.Append(dtCols.ColumnName + "='" + StringToDate(strColValue).ToString() + "' ");
    //                    else
    //                        strbXml.Append(dtCols.ColumnName + "='" + strColValue + "' ");

    //                }
    //            }
    //            intcolcount++;
    //        }
    //        strColValue = "";
    //        strbXml.Append(" /> ");
    //    }
    //    strbXml.Append("</Root>");
    //    return strbXml.ToString();
    //}
    private string FunPriLoadingXML()
    {
       
        try
        {
            DataTable dt = new DataTable();

            sbLoadingXML = new StringBuilder();
            sbLoadingXML.Append("<Root>");
      
            dt = (DataTable)ViewState["dtCustomers"];
            return Utility.FunPubFormXml(dt);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{

            //    sbLoadingXML.Append("<Details  Name='" + dt.Rows[i]["FIRST_NAME"].ToString() + "' ");
            //    //sbLoadingXML.Append(" Fund_Ref_No='" + S + "' ");
            //    sbLoadingXML.Append("Passport No='" + dt.Rows[i]["NUMBER"].ToString() + "' ");
            //    sbLoadingXML.Append("National Identification No='" + dt.Rows[i]["NOTE"].ToString() + "'/> ");
            //    //sbLoadingXML.Append("PANum='" + dtloading.Rows[i]["PANum"].ToString() + "' ");
            //    //sbLoadingXML.Append("SANum='" + dtloading.Rows[i]["SANum"].ToString() + "' ");
            //    //sbLoadingXML.Append("Account_Creation_Date='" + Utility.StringToDate(dtloading.Rows[i]["Account_Creation_Date"].ToString()) + "' ");
            //    //sbLoadingXML.Append("Remarks='" + dtloading.Rows[i]["Remarks"].ToString() + "' ");
            //    //sbLoadingXML.Append("Allocation_Status='" + dtloading.Rows[i]["Allocation_Status"].ToString() + "' ");
            //    //sbLoadingXML.Append("Maturity_Date='" + Utility.StringToDate(dtloading.Rows[i]["Maturity_Date"].ToString()) + "' ");
            //    //sbLoadingXML.Append("NetExposure='" + dtloading.Rows[i]["NetExposure"].ToString() + "' ");
            //    //sbLoadingXML.Append("Funding_Type='" + dtloading.Rows[i]["Funding_Type"].ToString() + "' ");
            //    //sbLoadingXML.Append("Gross_Exposure='" + dtloading.Rows[i]["Gross_Exposure"].ToString() + "' ");
            //    //sbLoadingXML.Append("New_Old='" + dtloading.Rows[i]["New_Old"].ToString() + "'  /> ");
            //}
            ////foreach (GridViewRow grvRow in gvLoading.Rows)
            ////{
            ////    CheckBox chkAllocation = (CheckBox)grvRow.FindControl("chkAllocation");
            ////    if (chkAllocation.Checked == true)
            ////    {
            ////        if (!bExists)
            ////            bExists = true;
            ////        HiddenField hdnLOB_ID = (HiddenField)grvRow.FindControl("hdnLOB_ID");
            ////        HiddenField hdnCustomer_ID = (HiddenField)grvRow.FindControl("hdnCustomer_ID");
            ////        //Label lblSno = (Label)grvRow.FindControl("lblSno");
            ////        // Label lblRefNo = (Label)grvRow.FindControl("lblRefNo");
            ////        Label lblPANum = (Label)grvRow.FindControl("lblPANum");
            ////        Label lblSANum = (Label)grvRow.FindControl("lblSANum");
            ////        Label lblAccountDate = (Label)grvRow.FindControl("lblAccountDate");
            ////        Label lblMaturityDate = (Label)grvRow.FindControl("lblMaturityDate");
            ////        Label lblNetExposureT = (Label)grvRow.FindControl("lblNetExposureT");
            ////        Label lblGrossT = (Label)grvRow.FindControl("lblGrossT");
            ////        Label lblfunding = (Label)grvRow.FindControl("lblfunding");
            ////        TextBox txtRemarks = (TextBox)grvRow.FindControl("txtRemarks");
            ////        HiddenField hdn_NewOld = (HiddenField)grvRow.FindControl("hdn_NewOld");
            ////        HiddenField hdn_DTL_ID = (HiddenField)grvRow.FindControl("hdn_DTL_ID");

            ////        //sbLoadingXML.Append("<Details  Serial_Number='" + lblSno.Text.Trim() + "' Fund_Ref_No='" + lblRefNo.Text.Trim() + "' LOB_ID ='" + hdnLOB_ID.Value .ToString ()+ "' ");
            ////        if (hdn_NewOld.Value == "O")
            ////            sbLoadingXML.Append("<Details  Serial_Number='" + hdn_DTL_ID.Value.ToString() + "' ");
            ////        else if (hdn_NewOld.Value == "N")
            ////            sbLoadingXML.Append("<Details  Serial_Number= '0' ");
            ////        //sbLoadingXML.Append(" Fund_Ref_No='" + S + "' ");
            ////        sbLoadingXML.Append(" LOB_ID ='" + hdnLOB_ID.Value.ToString() + "' ");
            ////        sbLoadingXML.Append(" Customer_ID='" + hdnCustomer_ID.Value.ToString() + "'  PANum='" + lblPANum.Text.Trim() + "'  SANum='" + lblSANum.Text.Trim() + "' ");
            ////        sbLoadingXML.Append(" Account_Creation_Date='" + Utility.StringToDate(lblAccountDate.Text.Trim()).ToString() + "' ");
            ////        sbLoadingXML.Append(" Remarks='" + txtRemarks.Text.Trim() + "' Allocation_Status='" + chkAllocation.Checked.ToString() + "' ");
            ////        if (lblMaturityDate.Text != "")
            ////            sbLoadingXML.Append(" Maturity_Date='" + Utility.StringToDate(lblMaturityDate.Text.Trim()) + "'");
            ////        sbLoadingXML.Append(" NetExposure='" + lblNetExposureT.Text.Trim() + "' ");
            ////        sbLoadingXML.Append(" Funding_Type='" + lblfunding.Text.Trim() + "' ");
            ////        sbLoadingXML.Append(" Gross_Exposure='" + lblGrossT.Text.Trim() + "' ");
            ////        sbLoadingXML.Append(" New_Old='" + hdn_NewOld.Value.ToString() + "'  /> ");
            ////    }
            ////}
            //sbLoadingXML.Append("</Root>");
        }

        catch (Exception ex)
        {
            throw ex;
        }
        return sbLoadingXML.ToString();
    }

}

