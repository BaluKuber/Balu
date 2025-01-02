#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Common  
/// Screen Name			: Get LOV user Control
/// Created By			: Tamilselvan.S
/// Created Date		: 25 Jan 2011
/// Purpose	            : 
#endregion

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using S3GBusEntity;

public partial class UserControls_LOBMasterView : System.Web.UI.UserControl
{

    #region [Variables]

    public enum enumContentType { Name, Code, Both };

    public string _strLOV_Code = string.Empty;
    public string _strControlID = string.Empty;

    public string strLOV_Code
    {
        get;
        set;
    }

    public string strControlID
    {
        get;
        set;
    }

    public Unit TextWidth
    {
        set
        {
            txtName.Width = value;
        }
    }

    public bool ButtonEnabled
    {
        set
        {
            btnGetLOV.Enabled = value;
        }
    }

    public enumContentType DispalyContent
    {
        get;
        set;
    }

    public string _strLobID = string.Empty;

    public string strLOBID
    {
        get { return _strLobID; }
        set { _strLobID = value; }
    }

    public string _strConstitutionID = string.Empty;

    public string strConstitutionID
    {
        get { return _strConstitutionID; }
        set { _strConstitutionID = value; }
    }
    public string _strBranchID = string.Empty;

    public string strBranchID
    {
        get { return _strBranchID; }
        set { _strBranchID = value; }
    }

    public string _strRegionID = string.Empty;

    public string strRegionID
    {
        get { return _strRegionID; }
        set { _strRegionID = value; }
    }

    #endregion [Variables]

    #region [Page Event's]
    protected void Page_Init(object sender, EventArgs n)
    {
        // FIRST 
         _strRegionID = string.Empty;
         _strBranchID = string.Empty;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (strLOV_Code == null && ViewState["strLOV_Code"] != null)
            strLOV_Code = ViewState["strLOV_Code"].ToString();

        //if (hdnLovCode.Value != "")
        //    strLOV_Code = hdnLovCode.Value;
        //if (hdnCtrlId.Value != "")
        //    strControlID = hdnCtrlId.Value;

        if (strLOV_Code != null)
        {
            string strQuery = "window.open('../common/GetLOV.aspx?LOV_Code=" + strLOV_Code + "&ControlID=" + strControlID + "&LOBID=" + strLOBID + "&RegionID=" + strRegionID + "&BranchID=" + strBranchID + "&ConstitutionID=" + strConstitutionID + "&DispCont=" + DispalyContent + "&Popup=Yes','newwindow','toolbar=no,location=no,menubar=no,width=500,height=330,resizable=no,scrollbars=yes,top=50,left=150'); return false; ";
            btnGetLOV.OnClientClick = strQuery;
            txtName.Attributes.Add("readonly", "true");
        }
        //txtName.Focus();

    }
    public void ReRegisterSearchControl(string LOVCode)
    {
        string strQuery = "window.open('../common/GetLOV.aspx?LOV_Code=" + LOVCode + "&ControlID=" + strControlID + "&LOBID=" + strLOBID + "&RegionID=" + strRegionID + "&BranchID=" + strBranchID + "&ConstitutionID=" + strConstitutionID + "&Popup=Yes','newwindow','toolbar=no,location=no,menubar=no,width=500,height=330,resizable=no,scrollbars=yes,top=50,left=150'); return false; ";
        ViewState["strLOV_Code"] = LOVCode;
        btnGetLOV.OnClientClick = strQuery;
        txtName.Text = "";
        hdnID.Value = "";
        txtName.Attributes.Add("readonly", "true");
    }

    public void ControlStatus(bool isEnabled)
    {
        this.btnGetLOV.Enabled = isEnabled;
        this.txtName.Enabled = isEnabled;
    }

    #endregion [Page Event's]

    #region [Function's]

    public void FunPubClearControlValue()
    {
        hdnID.Value = "";
        txtName.Text = "";
    }

    public void FunPubSetControlValue(string strID, string strName)
    {
        hdnID.Value = strID;
        txtName.Text = strName;
    }

    #endregion [Function's]
    protected void btnGetLOV_Click(object sender, EventArgs e)
    {
        if (hdnLovCode.Value != "")
        {
            //if (hdnLovCode.Value != "")
            strLOV_Code = hdnLovCode.Value;
            if (hdnCtrlId.Value != "")
                strControlID = hdnCtrlId.Value;

            if (strLOV_Code != null)
            {
                string strQuery = "window.open('../common/GetLOV.aspx?LOV_Code=" + strLOV_Code + "&ControlID=" + strControlID + "&LOBID=" + strLOBID + "&RegionID=" + strRegionID + "&BranchID=" + strBranchID + "&ConstitutionID=" + strConstitutionID + "&Popup=Yes','newwindow','toolbar=no,location=no,menubar=no,width=500,height=330,resizable=no,scrollbars=yes,top=50,left=150');";
                //btnGetLOV.OnClientClick = strQuery;

                txtName.Attributes.Add("readonly", "true");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strQuery, true);
            }
        }
    }
}
