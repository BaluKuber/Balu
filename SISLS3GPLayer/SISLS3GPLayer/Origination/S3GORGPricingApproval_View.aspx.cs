#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name         :   Origination
/// Screen Name         :   S3GORGROIRules_View
/// Created By          :   Suresh P
/// Created Date        :   22-Jun-2010
/// Purpose             :   To view ROI Rules details
/// Last Updated By		:   NULL
/// Last Updated Date   :   NULL
/// Reason              :   NULL
/// <Program Summary>
#endregion

#region Namespaces
using System;
using System.Web.Security;
using System.Globalization;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using S3GBusEntity;
#endregion

public partial class S3GORGPricingApproval_View : ApplyThemeForProject
{
    SerializationMode SerMode;
    UserInfo uinfo = null;
    S3GSession ObjS3GSession = new S3GSession();
    DataSet Ds = new DataSet();
    Dictionary<string, string> dictDropdownListParam;
    protected void Page_Load(object sender, EventArgs e)
    {
        CalendarExtender1.Format = ObjS3GSession.ProDateFormatRW;
        CalendarExtender2.Format = ObjS3GSession.ProDateFormatRW;
        lblErrorMessage.InnerText = "";

        if (!IsPostBack)
        {

        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        int intCompareDate = Utility.CompareDates(txtFromDate.Text.Trim(), txtToDate.Text.Trim());
        if (intCompareDate == 1)
        {
            grvApprovalDetails.DataSource = Ds.Tables.Add();  //make empty grid
            grvApprovalDetails.DataBind();

            FunPubGetApplicationNumberDetails();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('From Date should not be greater than To Date');", true);
            return;
        }
    }
    private void FunPubGetApplicationNumberDetails()
    {
        uinfo = new UserInfo();
        dictDropdownListParam = new Dictionary<string, string>();
        dictDropdownListParam.Add("@Company_ID", uinfo.ProCompanyIdRW.ToString());
        dictDropdownListParam.Add("@FROM_DATE", txtFromDate.Text.Trim());
        dictDropdownListParam.Add("@TO_DATE", txtToDate.Text.Trim());
        Ds = Utility.GetDataset("S3G_ORG_GetPricingOfferNumber", dictDropdownListParam);
        if (Ds.Tables[4].Rows.Count > 0)
        {
            grvApprovalDetails.DataSource = Ds.Tables[4];
            grvApprovalDetails.DataBind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", "alert('No Record Found.');", true);
            return;
        }
        dictDropdownListParam = null;
    }
    protected void grvApprovalDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        Response.Redirect("~/Origination/S3GORGPricingApproval_Add.aspx?qsPAId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");

    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Origination/S3GORGPricingApproval_Add.aspx");

    }

}
