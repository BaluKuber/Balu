#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Origination
/// Screen Name         :   S3GORGEnquiryAppraisal_View
/// Created By          :   Irsathameen .k
/// Created Date        :   01-July-2010
/// Purpose             :   To Create 
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion



#region Namespaces

using System;
using System.Globalization;
using System.Resources;
using System.Collections.Generic;
using System.Web.UI;
using System.ServiceModel;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using S3GBusEntity.Origination;
using S3GBusEntity;
using System.Web.Security;


#endregion

public partial class Origination_S3GORGEnquiryAppraisal_View : ApplyThemeForProject
{

    #region Intialization

    int intCompanyID = 0;
    int intUserID = 0;
    string strSearchColName;
    PagedDataSource pdsPagerDataSource;
    DataView dvSearchView;
    private Int32 intCount;

    UserInfo ObjUserInfo = new UserInfo();
    string strRedirectPageAdd = "~/Origination/S3GORGEnquiryAppraisal_Add.aspx";
    string strRedirectPage = "~/Origination/S3GORGTransLander.aspx?Code=ENCA&MultipleDNC=1";
    EnquiryMgtServicesReference.EnquiryMgtServicesClient objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
    EnquiryService.S3G_ORG_EnquiryCustomerDetailsRow objEnquiryCustomerRow;
    EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable objS3G_EnquiryCustomerDatatable = new EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable();
    FormsAuthenticationTicket Ticket;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPubSetIndex(1);
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            lblErrorMessage.InnerText = "";

            if (!IsPostBack)
            {
                //FunPubAppraisedCustomerDetail();
                FunPubGetEnquiryDetails(intCompanyID, intUserID);
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    #region "Enquiry Mode"

    protected void FunPubGetEnquiryDetails(int intCompany_Id, int intUserID)
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        pnl_Enquiry.Visible = true;
        try
        {
            gvEnquiryAppraisal.Columns[4].Visible = true;
            byte[] bytesEnquiryCustomerDetails = objEnquiryMgtServicesClient.FunPubGetEnquiryDetails(intCompany_Id, intUserID);
            objS3G_EnquiryCustomerDatatable = (EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable)ClsPubSerialize.DeSerialize(bytesEnquiryCustomerDetails, SerializationMode.Binary, typeof(EnquiryService.S3G_ORG_EnquiryCustomerDetailsDataTable));

            gvEnquiryAppraisal.DataSource = objS3G_EnquiryCustomerDatatable;
            gvEnquiryAppraisal.DataBind();
        }
        catch (FaultException<EnquiryMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            objEnquiryMgtServicesClient.Close();
        }
    }

    //protected void FunPubGetEnquiryDetailsModify()
    //{
    //    pnl_Enquiry.Visible = true;
    //    try
    //    {
    //        gvEnquiryAppraisal.Columns[4].Visible = false;
    //        DataTable dtEnquiryModify = new DataTable();

    //        dtEnquiryModify = objEnquiryMgtServicesClient.FunPubGetEnquiryDetailsModify();

    //        gvEnquiryAppraisal.DataSource = dtEnquiryModify;
    //        gvEnquiryAppraisal.DataBind();
    //    }
    //    catch (FaultException<EnquiryMgtServicesReference.ClsPubFaultException> objFaultExp)
    //    {
    //        lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrorMessage.InnerText = ex.Message;
    //    }
    //    finally
    //    {
    //        objEnquiryMgtServicesClient.Close();

    //    }
    //}


    #endregion

    #region "Customer Mode"

    private void FunPubAppraisedCustomerDetail()
    {
        objEnquiryMgtServicesClient = new EnquiryMgtServicesReference.EnquiryMgtServicesClient();
        DataTable dtCustomerDocTable = new DataTable();
        try
        {
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] byteObjCaptureData = objEnquiryMgtServicesClient.FunPubAppraisedCustomerDetail();

            dtCustomerDocTable = (DataTable)ClsPubSerialize.DeSerialize(byteObjCaptureData, SerializationMode.Binary, typeof(DataTable));

            gvCustomerAppraisal.DataSource = dtCustomerDocTable;
            gvCustomerAppraisal.DataBind();

        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
             lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
        finally
        {
            dtCustomerDocTable.Dispose();
            dtCustomerDocTable = null;
            objEnquiryMgtServicesClient.Close();
        }

    }

    #endregion

       
    protected void rdbCustomEnquiry_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbCustomEnquiry.SelectedValue == "0")
            {
                pnl_Enquiry.Visible = true;
                pnl_Customer.Visible = false;
                FunPubGetEnquiryDetails(intCompanyID, intUserID);
            }
            else
            {
                pnl_Enquiry.Visible = false;
                pnl_Customer.Visible = true;
                Response.Redirect(strRedirectPageAdd + "?qsTransactionType=1" + "&qsMode=C");
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    protected void chkEnquiryNumber_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int intEnquiryUpdation_ID = 0;
            string strEnquiryUpdation_ID = string.Empty;
            CheckBox chkEnquiryNumber = null;

            foreach (GridViewRow gvEnquiryRow in gvEnquiryAppraisal.Rows)
            {
                chkEnquiryNumber = ((CheckBox)gvEnquiryRow.FindControl("chkEnquiryNumber"));
                if (chkEnquiryNumber.Checked)
                {
                    intEnquiryUpdation_ID = Convert.ToInt32(((Label)gvEnquiryRow.FindControl("lblEnquiryUpdationID")).Text);
                    strEnquiryUpdation_ID = Convert.ToString(intEnquiryUpdation_ID);

                    FormsAuthenticationTicket TicketID = new FormsAuthenticationTicket(strEnquiryUpdation_ID, false, 0);
                    Response.Redirect(strRedirectPageAdd + "?qsTransactionType=2" + "&qsMode=C" + "&qsEnquiryUpdationID=" +
                    FormsAuthentication.Encrypt(TicketID));
                }
                else
                    chkEnquiryNumber.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    #region "Customer Appraisal Edit Quey grid commands"

    #endregion

    #region "Enquiry Appraisal Edit Quey grid commands"

    protected void gvEnquiryAppraisal_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkEnquiryNumber = (CheckBox)e.Row.FindControl("chkEnquiryNumber");
                chkEnquiryNumber.Checked = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
        }
    }

    #endregion
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }
}
