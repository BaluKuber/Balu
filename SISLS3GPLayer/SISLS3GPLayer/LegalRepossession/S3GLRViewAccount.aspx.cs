using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Web.Security;
using System.IO;
using System.Xml;
using S3GBusEntity.LegalRepossession;
using System.Linq;

public partial class LegalRepossession_S3GLRViewAccount : ApplyThemeForProject
{
    string strPANum = "";
    string strSANum = "";
    UserInfo ObjUserInfo = new UserInfo();
    Dictionary<string, string> Procparam = null;
    DataTable dt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (Request.QueryString["qsPANum"] != null)
        {

            strPANum = Request.QueryString["qsPANum"];
            
        }
        if (Request.QueryString["qsSANum"] != null)
        {

            strSANum = Request.QueryString["qsSANum"];

        }

        Procparam = new Dictionary<string, string>();
        Procparam.Add("@Company_ID", ObjUserInfo.ProCompanyIdRW.ToString());
        Procparam.Add("@PANum", strPANum);
        Procparam.Add("@SANum", strSANum);

        DataTable dt = Utility.GetDefaultData("S3g_LR_AccountTransactionDetails", Procparam);
        //txtPANum.Text = strPANum;
        //txtSANum.Text = strSANum;
        //GV1.DataSource = dt;
        //GV1.DataBind(); 
       
        Decimal  c=0;
        decimal  decTotDues = 0;
        decimal decTotRec = 0;
        decimal  decTotBal = 0;
      
        if (dt.Rows.Count > 0)
        {

            decTotDues = (decimal)dt.Compute("SUM(DUES)", "PRIMEACCOUNTNO IS NOT NULL");
            decTotRec = (decimal)dt.Compute("SUM(RECEIPTS)", "PRIMEACCOUNTNO IS NOT NULL");
           
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                c = c + Convert.ToDecimal(dt.Rows[i]["DUES"]) - Convert.ToDecimal(dt.Rows[i]["RECEIPTS"]);
                dt.Rows[i]["BALANCE"] = c;
                //decTotDues = decTotDues + Convert.ToDecimal(dt.Rows[i]["DUES"]);
                //decTotRec = decTotRec + Convert.ToDecimal(dt.Rows[i]["RECEIPTS"]);

                //intTotBal = intTotBal + Convert.ToInt32(dt.Rows[i]["BALANCE"]);
            }
           
            lblGridDetails.Visible = false;
            pnlTransactionDetails.Visible = true;
            grvtransaction.DataSource = dt;
            grvtransaction.DataBind();
            Label lblTotalDues = (Label)grvtransaction.FooterRow.FindControl("lblTotalDues");
            Label lblTotalReceipts = (Label)grvtransaction.FooterRow.FindControl("lblTotalReceipts");
            Label lblTotalbalance = (Label)grvtransaction.FooterRow.FindControl("lblTotalbalance");

            lblTotalDues.Text = decTotDues.ToString();
            lblTotalReceipts.Text = decTotRec.ToString();
            lblTotalbalance.Text = c.ToString();
        }
        else
        {
            lblGridDetails.Visible = true;
            pnlTransactionDetails.Visible = false;
            grvtransaction.DataSource = null;
            grvtransaction.DataBind();

        }
    }

    protected new void Page_PreInit(object sender, EventArgs e)
    {
        try
        {
            //if (Request.QueryString["IsFromEnquiry"] != null || Request.QueryString["IsFromApplication"] != null)
            //{
            this.Page.MasterPageFile = "~/Common/MasterPage.master";
            UserInfo ObjUserInfo = new UserInfo();
            this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            //}
            //else
            //{
                //this.Page.MasterPageFile = "~/Common/S3GMasterPageCollapse.master";
                //UserInfo ObjUserInfo = new UserInfo();
                //this.Page.Theme = ObjUserInfo.ProUserThemeRW;
            //}
        }
        catch (Exception objException)
        {

            throw (objException);
            //  ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            //cvCustomerMaster.ErrorMessage = "Unable to Initialize the Customer Details";
            //cvCustomerMaster.IsValid = false;
        }
    }
}
