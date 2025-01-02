/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved

/// <Program Summary>
/// Module Name               : SysAdmin 
/// Screen Name               : Hierchy Master
/// Created By                : Irsathameen .K
/// Created Date              : 03-Mar-2011
/// Purpose                   : To create Hierarchy Master Generation
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    : 

/// <Program Summary>


#region NameSpaces

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
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using Resources;
using S3GBusEntity;
using System.Text;

#endregion

public partial class System_Admin_S3GSysAdminHierachyMaster : ApplyThemeForProject
{
    #region Variable declaration

    CompanyMgtServicesReference.CompanyMgtServicesClient objHierachyClient;
    S3GBusEntity.CompanyMgtServices.S3G_SYSAD_HierachyMasterDataTable ObjS3G_SYS_HierachyMasterDataTable = null;
    SerializationMode SerMode = SerializationMode.Binary;

    //User Authorization
    UserInfo ObjUserInfo = new UserInfo();
    string strMode = string.Empty;
    bool bCreate = false;
    bool bClearList = false;
    bool bModify = false;
    bool bQuery = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    //Code end

    int intErrCode, intUserID, intCompanyID = 0, i, j, check = 0;
    static string strPageName = "Hierarchy Master";
    DataTable dt = new DataTable();
    Dictionary<string, string> dictParam = null;
    string StrXMLHierachyDetails;
    string strRedirectPage = "../System Admin/S3GSysAdminHierachyMaster.aspx";
    string strRedirectPageView = "window.location.href='../System Admin/S3GSysAdminHierachyMaster.aspx';";
    int[] numbers = new int[5];

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            FunPriLoadPage();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
        }
    }

    #endregion

    #region User Defined Functions

    private void FunPrisetIntialRow()
    {
        try
        {
            DataRow dr;
            dt.Columns.Add("Hierachy");
            dt.Columns.Add("Width");
            dt.Columns.Add("Location_Description");
            dt.Columns.Add("Active");
            dt.Columns.Add("Operational");
            for (i = 1; i <= 5; i++)
            {
                dr = dt.NewRow();
                dr["Hierachy"] = Convert.ToString(i);
                dr["Width"] = string.Empty;
                dr["Location_Description"] = string.Empty;
                dr["Active"] = true;
                dt.Rows.Add(dr);
            }
            GRVHeirachy.Columns[3].Visible = false;
            GRVHeirachy.DataSource = dt;
            GRVHeirachy.DataBind();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriLoadPage()
    {
        try
        {
            this.Page.Title = FunPubGetPageTitles(enumPageTitle.PageTitle);
            intCompanyID = ObjUserInfo.ProCompanyIdRW;
            intUserID = ObjUserInfo.ProUserIdRW;
            S3GSession ObjS3GSession = new S3GSession();

            //User Authorization
            bCreate = ObjUserInfo.ProCreateRW;
            bModify = ObjUserInfo.ProModifyRW;
            bQuery = ObjUserInfo.ProViewRW;
            bIsActive = ObjUserInfo.ProIsActiveRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW;

            if (!IsPostBack)
            {
                FunPriGetHierachyDetails();
                if (bModify && (ObjUserInfo.ProUserLevelIdRW == 5 || ObjUserInfo.ProUserLevelIdRW == 4))
                    btnSave.Enabled = true;
                else
                    btnSave.Enabled = false;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }
    private void FunPriGenerateHierachyXMLDetails()
    {
        try
        {
            StrXMLHierachyDetails = GRVHeirachy.FunPubFormXml();
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }
    private void FunPriGetHierachyDetails()
    {

        try
        {
            DataSet DS = new DataSet();
            dictParam = new Dictionary<string, string>();
            dictParam.Add("@Company_ID", Convert.ToString(intCompanyID));
            DS = Utility.GetDataset(SPNames.S3G_SYSAD_GetHierachyMasterDetails, dictParam);
            UserInfo ObjUserInfo = new UserInfo();
            intUserID = ObjUserInfo.ProUserIdRW;
            bMakerChecker = ObjUserInfo.ProMakerCheckerRW; //Modified by Tamilselvan.s on 15/12/2011 for UAT Molan bug Fixing
            // MakerChecker_Access
            //if (DS.Tables[1].Rows.Count > 0 && DS.Tables[0].Rows.Count > 0)
            //{
            //    if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(DS.Tables[1].Rows[0]["Created_By"]), Convert.ToInt32(DS.Tables[1].Rows[0]["User_Level_ID"]))))
            //        bMakerChecker = false;
            //    else
            //        bMakerChecker = true; ;
            //}

            //  Create Mode           
            if (DS.Tables[0].Rows.Count == 0)
            {
                strMode = "C";
                FunPrisetIntialRow();
                lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Create);
            }

            // Modify Mode
            else if (DS.Tables[0].Rows.Count > 0)
            {
                strMode = "M";
                if (DS.Tables[0].Rows.Count == 5)   // All five Rows available
                {
                    GRVHeirachy.DataSource = DS.Tables[0];
                    GRVHeirachy.DataBind();
                    if (bModify && ObjUserInfo.ProUserLevelIdRW == 5)
                    {
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    }
                    else
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View); //Modified by Tamilselvan.S on 15/12/2011 for UAT Malolan Observation fixing
                }
                else if (DS.Tables[0].Rows.Count < 5)   //  Not Five Rows Available  Bind All  5 Rows
                {
                    DataRow dr;
                    for (i = DS.Tables[0].Rows.Count + 1; i <= 5; i++)
                    {
                        dr = DS.Tables[0].NewRow();
                        dr["Hierachy"] = Convert.ToString(i);
                        dr["Width"] = 0;
                        dr["Location_Description"] = string.Empty;
                        dr["Active"] = false;
                        dr["Operational"] = false;
                        DS.Tables[0].Rows.Add(dr);
                    }
                    GRVHeirachy.DataSource = DS.Tables[0];
                    GRVHeirachy.DataBind();
                    if (bModify && ObjUserInfo.ProUserLevelIdRW == 5)
                    {
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Modify);
                    }
                    else
                        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.View); //Modified by Tamilselvan.S on 15/12/2011 for UAT Malolan Observation fixing
                }
                //Added by Tamilselvan.S on 06/09/2011 for allowed only low level modification
                if (DS.Tables[0].Rows.Count > 0)
                {
                    var vartotalCount = Convert.ToInt64((from c in DS.Tables[0].AsEnumerable()
                                                         where !string.IsNullOrEmpty(c.Field<string>("Location_description"))
                                                         && c.Field<string>("Active").ToUpper() == "TRUE"
                                                         select c.Field<string>("Active")).Count());
                    int intRowCount = 0;
                    foreach (GridViewRow gvr in GRVHeirachy.Rows)
                    {
                        intRowCount++;
                        CheckBox chk = (CheckBox)gvr.FindControl("chkActive");
                        chk.Enabled = false;
                        if (bModify && (ObjUserInfo.ProUserLevelIdRW == 5 || ObjUserInfo.ProUserLevelIdRW == 4)) //Modified by Tamilselvan.S on 15/12/2011 for UAT Malolan Observation fixing
                        {
                            if (intRowCount > vartotalCount - 1)
                                chk.Enabled = true;
                        }
                    }
                }
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    #endregion

    #region Button Events (Save,Cancel)

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int intTotalWidth = 0;//Added by Tamilselvan.S on 12/09/2011
            for (i = 0; i < 5; i++)
            {
                TextBox txtWidth = (TextBox)GRVHeirachy.Rows[i].FindControl("txtWidth");
                TextBox txtLocationDesc = (TextBox)GRVHeirachy.Rows[i].FindControl("txtLocationDesc");
                CheckBox chkActive = (CheckBox)GRVHeirachy.Rows[i].FindControl("chkActive");
                // Validation for Leaving Empty in width
                if (string.IsNullOrEmpty(txtWidth.Text.Trim()) && (!string.IsNullOrEmpty(txtLocationDesc.Text.Trim())))
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter the width value in Hierarchy " + Convert.ToInt32(i + 1));
                    if (strMode == "C")
                        chkActive.Checked = false;
                    return;
                }
                // Validation Leaving Empty in Description 
                else if (!string.IsNullOrEmpty(txtWidth.Text.Trim()) && (string.IsNullOrEmpty(txtLocationDesc.Text.Trim())))
                {
                    Utility.FunShowAlertMsg(this.Page, "Enter the Location Description in Hierarchy " + Convert.ToInt32(i + 1));
                    if (strMode == "C")
                        chkActive.Checked = false;
                    return;
                }
                //Check Not Empty in Width and Description
                else if (!string.IsNullOrEmpty(txtWidth.Text.Trim()) && (!string.IsNullOrEmpty(txtLocationDesc.Text.Trim())))
                {
                    numbers[i] = i + 1;
                    if (strMode == "C")
                        chkActive.Checked = true;
                }
                //validation for Leaving Empty both Width and Description
                else if (string.IsNullOrEmpty(txtWidth.Text.Trim()) && (string.IsNullOrEmpty(txtLocationDesc.Text.Trim())))
                    chkActive.Checked = false;
                //Validation for total width length should be Eight.
                if (!string.IsNullOrEmpty(txtWidth.Text.Trim()) && (!string.IsNullOrEmpty(txtLocationDesc.Text.Trim())))
                    intTotalWidth += Convert.ToInt32(txtWidth.Text.Trim()); //Added by Tamilselvan.S on 12/09/2011
                string strLoc = ((TextBox)GRVHeirachy.Rows[i].FindControl("txtLocationDesc")).Text.Trim().ToUpper();
                for (int loopVal = i + 1; loopVal < 5; loopVal++)  // check the Duplicate description
                {
                    if ((!string.IsNullOrEmpty(strLoc)) && (((TextBox)GRVHeirachy.Rows[loopVal].FindControl("txtLocationDesc")).Text.Trim().ToUpper() ==
                       ((TextBox)GRVHeirachy.Rows[i].FindControl("txtLocationDesc")).Text.Trim().ToUpper()))
                    {
                        Utility.FunShowAlertMsg(this.Page, "Duplicate Location Description : " +
                            (((TextBox)GRVHeirachy.Rows[loopVal].FindControl("txtLocationDesc")).Text.Trim()));
                        return;
                    }
                }

            }
            //Validation for total width length should be Eight. //Added by Tamilselvan.S on 12/09/2011
            if (intTotalWidth > 8)
            {
                Utility.FunShowAlertMsg(this.Page, "All the Location width should be Less than or Equal to Eight");
                return;
            }

            for (i = 0; i < 5; i++)  // check whether values given in Sequence order
            {
                if (numbers[i] == 0)
                {
                    for (j = i + 1; j < 5; j++)
                    {
                        if (numbers[j] != 0)
                        {
                            Utility.FunShowAlertMsg(this.Page, "Enter the values in Sequence order");
                            return;
                        }
                    }
                }
            }
            if (numbers.Sum() == 0)
            {
                Utility.FunShowAlertMsg(this.Page, "Add atleast one row in hierarchy details");
                return;
            }
            if (Page.IsValid)
            {
                ObjS3G_SYS_HierachyMasterDataTable = new S3GBusEntity.CompanyMgtServices.S3G_SYSAD_HierachyMasterDataTable();
                S3GBusEntity.CompanyMgtServices.S3G_SYSAD_HierachyMasterRow ObjHierachyRow;
                ObjHierachyRow = ObjS3G_SYS_HierachyMasterDataTable.NewS3G_SYSAD_HierachyMasterRow();
                ObjHierachyRow.Company_ID = intCompanyID;
                ObjHierachyRow.Created_By = intUserID;
                FunPriGenerateHierachyXMLDetails();
                ObjHierachyRow.XMLHierachyDetails = StrXMLHierachyDetails.Replace("Hierarchy", "Hierachy"); ;
                ObjS3G_SYS_HierachyMasterDataTable.AddS3G_SYSAD_HierachyMasterRow(ObjHierachyRow);
                objHierachyClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
                intErrCode = objHierachyClient.FunPubCreateHierachyMasterDetails(SerMode, ClsPubSerialize.Serialize(ObjS3G_SYS_HierachyMasterDataTable, SerMode));
                if (intErrCode == 0)
                {
                    if (lblHeading.Text.Contains("Create"))
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                        Utility.FunShowAlertMsg(this.Page, "Hierarchy Details Saved Successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Insert", strRedirectPageView, true);
                        return;
                    }
                    else
                    {
                        //Added by Bhuvana M on 19/Oct/2012 to avoid double save click
                        btnSave.Enabled = false;
                        //End here
                        Utility.FunShowAlertMsg(this.Page, "Hierarchy Details Updated Successfully");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Update", strRedirectPageView, true);
                        return;
                    }
                }
                else if (intErrCode == 29)//added by Tamilselvan.S on 06/09/2011 for Checking with Location 
                {
                    Utility.FunShowAlertMsg(this.Page, "Hierarchy is Mapped with Location");
                    return;
                }
                //else if (intErrCode == 39)//added by Saran on  22-Nov-2011 for checking operation location once created and try to deselect all.
                //{
                //    Utility.FunShowAlertMsg(this.Page, "Select Operational Location");
                //    return;
                //}
            }
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            lblErrorMessage.Text = "Unable Save the Hierarchy Master";
            cvCustomerMaster.IsValid = false;
        }
        finally
        {
            if (objHierachyClient != null)
                objHierachyClient.Close();
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ReturnToWFHome(); //Modified by Tamilselvan.S on 15/12/2011 for UAT Malolan Observation fixing
            //Response.Redirect(strRedirectPage);
        }
        catch (Exception objException)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(objException, strPageName);
            throw objException;
        }
    }

    #endregion

    #region Gridview Events

    protected void GRVHeirachy_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblActive = (Label)e.Row.FindControl("lblActive");
                CheckBox chkActive = (CheckBox)e.Row.FindControl("chkActive");
                TextBox txtWidth = (TextBox)e.Row.FindControl("txtWidth");
                TextBox txtLocationDesc = (TextBox)e.Row.FindControl("txtLocationDesc");
                CheckBox chkOperational = (CheckBox)e.Row.FindControl("chkOperational");
                chkActive.Checked = Convert.ToBoolean(lblActive.Text);
                txtWidth.SetDecimalPrefixSuffix(1, 0, true);
                txtWidth.Text = txtWidth.Text == "0" ? txtWidth.Text = string.Empty : txtWidth.Text;
                if (!string.IsNullOrEmpty(txtWidth.Text) && !string.IsNullOrEmpty(txtLocationDesc.Text))
                    txtWidth.ReadOnly = true;// txtLocationDesc.ReadOnly
                if (bMakerChecker) //Modified by Tamilselvan.S on 15/12/2011 for UAT Malolan Observation fixing
                {
                    txtLocationDesc.ReadOnly = txtWidth.ReadOnly = true;
                    chkActive.Enabled = chkOperational.Enabled = false;
                }
                if (bModify && ObjUserInfo.ProUserLevelIdRW != 5) //Modified by Tamilselvan.S on 15/12/2011 for UAT Malolan Observation fixing
                {
                    chkOperational.Enabled = chkActive.Enabled = false;
                    txtLocationDesc.ReadOnly = txtWidth.ReadOnly = true;
                }
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion

    //Added by Saran on 22-Nov-2011 to fix operational Location.
    #region "Checkbox Events"

    protected void chkOperational_CheckedChanged(object sender, EventArgs e)
    {
        try
        {


            CheckBox chk = (CheckBox)sender;

            GridViewRow gvRow = (GridViewRow)chk.Parent.Parent;
            CheckBox chkActive = (CheckBox)gvRow.FindControl("chkActive");

            if (chkActive.Checked)
            {
                /*foreach (GridViewRow grow in GRVHeirachy.Rows)
                {
                    CheckBox chkOperational = (CheckBox)grow.FindControl("chkOperational");
                    if (gvRow.RowIndex != grow.RowIndex)
                    {
                        chkOperational.Checked = false;
                    }
                }*/
            }
            else
            {
                /* foreach (GridViewRow grow in GRVHeirachy.Rows)
                 {
                     CheckBox chkOperational = (CheckBox)grow.FindControl("chkOperational");
                     chkOperational.Checked = false;
                 }*/
                chk.Checked = false;
                Utility.FunShowAlertMsg(this.Page, "Operational Location should be set to active Level");
                return;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    protected void chkActive_CheckedChanged(object sender, EventArgs e)
    {
        try
        {


            CheckBox chk = (CheckBox)sender;

            GridViewRow gvRow = (GridViewRow)chk.Parent.Parent;
            CheckBox chkActive = (CheckBox)gvRow.FindControl("chkActive");
            CheckBox chkOperational = (CheckBox)gvRow.FindControl("chkOperational");
            if (chkOperational.Checked && (!chkActive.Checked))
            {
                chkActive.Checked = true;
                Utility.FunShowAlertMsg(this.Page, "Operational Location is mapped with the selected Location Level.");
                return;
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, strPageName);
            throw ex;
        }
    }

    #endregion
}
