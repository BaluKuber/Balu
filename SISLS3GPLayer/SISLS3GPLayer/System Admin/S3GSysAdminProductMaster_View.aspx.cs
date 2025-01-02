/// Module Name     :   System Admin
/// Screen Name     :   S3GSysAdminProductMaster_View
/// Created By      :   Ramesh M
/// Created Date    :   10-May-2010
/// Purpose         :   To view product master details
#region Namespaces
using System;
using System.Web.Security;
using System.Data;
using System.ServiceModel;
using System.Web.UI.WebControls;
using S3GBusEntity;
using System.Configuration;

#endregion

public partial class S3GSysAdminProductMaster_View : ApplyThemeForProject
{
    #region Intialization
    string strRedirectPage = "~/System Admin/S3GSysAdminProductMaster_Add.aspx";
    CompanyMgtServicesReference.CompanyMgtServicesClient objProductMasterClient;
    CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable ObjS3G_ProductMaster_ViewDataTable = new CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable();
    #endregion

    #region Paging Config

    string strSearchVal1 = string.Empty;
    string strSearchVal2 = string.Empty;
    string strSearchVal3 = string.Empty;
    string strSearchVal4 = string.Empty;
    string strSearchVal5 = string.Empty;
    int intUserID = 0;
    int intCompanyID = 0;
    bool bModify = false;
    bool bQuery = false;
    bool bCreate = false;
    bool bIsActive = false;
    bool bMakerChecker = false;
    UserInfo ObjUserInfo = null;
    PagingValues ObjPaging = new PagingValues();

    public delegate void PageAssignValue(int ProPageNumRW, int intPageSize);
    /// <summary>
    ///Assign Page Number
    /// </summary
    public int ProPageNumRW
    {
        get;
        set;
    }
    /// <summary>
    ///Assign Page Size
    /// </summary
    public int ProPageSizeRW
    {
        get;
        set;

    }

    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;
        ProPageSizeRW = intPageSize;
        FunPriBindGrid();
    }
    #endregion

    #region PageLoad
    /// <summary>
    ///Page Load Events
    /// </summary
    protected void Page_Load(object sender, EventArgs e)
    {
        lblHeading.Text = FunPubGetPageTitles(enumPageTitle.Details);
        #region Paging Config
        ProPageNumRW = 1;
        TextBox txtPageSize = (TextBox)ucCustomPaging.FindControl("txtPageSize");
        if (txtPageSize.Text != "")
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        else
            ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

        PageAssignValue obj = new PageAssignValue(this.AssignValue);
        ucCustomPaging.callback = obj;
        ucCustomPaging.ProPageNumRW = ProPageNumRW;
        ucCustomPaging.ProPageSizeRW = ProPageSizeRW;

        #endregion

        ObjUserInfo = new UserInfo();
        intCompanyID = ObjUserInfo.ProCompanyIdRW;
        intUserID = ObjUserInfo.ProUserIdRW;
        bModify = ObjUserInfo.ProModifyRW;
        bQuery = ObjUserInfo.ProViewRW;
        bCreate = ObjUserInfo.ProCreateRW;
        bIsActive = ObjUserInfo.ProIsActiveRW;
        bMakerChecker = ObjUserInfo.ProMakerCheckerRW;
        if (!IsPostBack)
        {
            FunPriBindGrid();
            if (!bIsActive)
            {
                grvProductMaster.Columns[1].Visible = false;
                grvProductMaster.Columns[0].Visible = false;
                btnCreate.Enabled = false;
                return;
            }
            
                if (!bModify)
                {
                    grvProductMaster.Columns[1].Visible = false;
                }              
                if (!bQuery)
                {
                    grvProductMaster.Columns[0].Visible = false;
                }
                if (!bCreate)
                {
                    btnCreate.Enabled = false;
                }
            



        }
    }
    #endregion

    #region Page Load Methods
    /// <summary>
    ///Bind Product Details
    /// </summary
    private void FunPriBindGrid()
    {
        objProductMasterClient = new CompanyMgtServicesReference.CompanyMgtServicesClient();
        try
        {
            int intTotalRecords = 0;
            CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewRow ObjProductMasterterRow = ObjS3G_ProductMaster_ViewDataTable.NewS3G_SYSAD_ProductMaster_ViewRow();
            //Paging Properties set
            //ObjProductMasterterRow.LOB_ID = 0;
            ObjPaging.ProCompany_ID = intCompanyID;
            ObjPaging.ProUser_ID = intUserID;
            ObjPaging.ProTotalRecords = intTotalRecords;
            ObjPaging.ProCurrentPage = ProPageNumRW;
            ObjPaging.ProPageSize = ProPageSizeRW;
            ObjPaging.ProSearchValue = hdnSearch.Value;
            ObjPaging.ProOrderBy = hdnOrderBy.Value;

            //Paging code end

            ObjS3G_ProductMaster_ViewDataTable.AddS3G_SYSAD_ProductMaster_ViewRow(ObjProductMasterterRow);
            SerializationMode SerMode = SerializationMode.Binary;

            byte[] byteProductDetails = objProductMasterClient.FunPubQueryProductPaging(out  intTotalRecords, SerMode, ClsPubSerialize.Serialize(ObjS3G_ProductMaster_ViewDataTable, SerMode), ObjPaging);

            CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable  dtProduct = (CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable)ClsPubSerialize.DeSerialize(byteProductDetails, SerializationMode.Binary, typeof(CompanyMgtServices.S3G_SYSAD_ProductMaster_ViewDataTable ));
            DataView dvProduct = dtProduct.DefaultView;

            //Paging Config

            FunPriGetSearchValue();

            //This is to show grid header
            bool bIsNewRow = false;
            if (dvProduct.Count == 0)
            {
                dvProduct.AddNew();
                bIsNewRow = true;
            }

            grvProductMaster.DataSource = dvProduct;
            grvProductMaster.DataBind();

            //This is to hide first row if grid is empty
            if (bIsNewRow)
            {
                grvProductMaster.Rows[0].Visible = false;
            }

            FunPriSetSearchValue();

            ucCustomPaging.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
            ucCustomPaging.setPageSize(ProPageSizeRW);

            //Paging Config End

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
            objProductMasterClient.Close();
        }
    }
    /// <summary>
    /// This method is used to display product details
    /// </summary>
    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    

    /// <summary>
    /// To clear value after show all is clicked
    /// </summary>
    private void FunPriClearSearchValue()
    {
        if (grvProductMaster .HeaderRow != null)
        {
            ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderProductCode")).Text = "";
            ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderProductName")).Text = "";
            ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderLOBName")).Text = "";
            //((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = "";
            //((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = "";
        }
    }
    /// <summary>
    /// Tos et search value after sorting or paging changed
    /// </summary>
    private void FunPriSetSearchValue()
    {
        if (grvProductMaster.HeaderRow != null)
        {
            ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderProductCode")).Text = strSearchVal1;
            ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderProductName")).Text = strSearchVal2;
            ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderLOBName")).Text = strSearchVal3;
            //((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text = strSearchVal4;
            //((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text = strSearchVal5;
        }
    }


    /// <summary>
    /// To Search in Grid view Gets the text box as sender and gets its text
    /// </summary>
    /// <param name="sender">Text box in gridview</param>
    /// <param name="e"></param>

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {

        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);

            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver
            if (strSearchVal1 != "")
            {
                strSearchVal += "Product_Code like '" + strSearchVal1 + "%'";
            }
            if (strSearchVal2 != "")
            {
                strSearchVal += " and Product_Name like '" + strSearchVal2 + "%'";
            }
            if (strSearchVal3 != "")
            {
                strSearchVal += " and LOB_Name like '" + strSearchVal3 + "%'";
            }
            //if (strSearchVal4 != "")
            //{
            //    strSearchVal += " and LOB_Name like '" + strSearchVal4 + "%'";
            //}
            //if (strSearchVal5 != "")
            //{
            //    strSearchVal += " and LOB_Name like '" + strSearchVal5 + "%'";
            //}

            if (strSearchVal.StartsWith(" and "))
            {
                strSearchVal = strSearchVal.Remove(0, 5);
            }
            hdnSearch.Value = strSearchVal;
            FunPriBindGrid();
            FunPriSetSearchValue();
            if (txtboxSearch.Text != "")
                ((TextBox)grvProductMaster.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    /// <summary>
    /// Gets the Sort Direction of the strColumn in the Grid View Using hidden control
    /// </summary>
    /// <param name="strColumn"> Colunm Name is Passed</param>
    /// <returns>Sort Direction as a String </returns>

    private string FunPriGetSortDirectionStr(string strColumn)
    {
        string strSortDirection = string.Empty;
        string strSortExpression = string.Empty;
        // By default, set the sort direction to ascending.
        string strOrderBy = string.Empty;
        strSortDirection = "DESC";
        try
        {
            // Retrieve the last strColumn that was sorted.
            // Check if the same strColumn is being sorted.
            // Otherwise, the default value can be returned.
            strSortExpression = hdnSortExpression.Value;
            if ((strSortExpression != "") && (strSortExpression == strColumn) && (hdnSortDirection.Value != null) && (hdnSortDirection.Value == "DESC"))
            {
                strSortDirection = "ASC";
            }
            // Save new values in hidden control.
            hdnSortDirection.Value = strSortDirection;
            hdnSortExpression.Value = strColumn;
            strOrderBy = " " + strColumn + " " + strSortDirection;
            hdnOrderBy.Value = strOrderBy;
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        return strSortDirection;
    }

    /// <summary>
    /// Will Perform Sorting On Colunm base upon the link button id calling the function
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");
            switch (lnkbtnSearch.ID)
            {
                case "lnkbtnProductCode":
                    strSortColName = "Product_Code";
                    break;
                case "lnkbtnProductName":
                    strSortColName = "Product_Name";
                    break;
                case "lnkbtnLobCode":
                    strSortColName = "LOB_Name";
                    break;
            }

            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPriBindGrid();

            if (strDirection == "ASC")
            {
                ((ImageButton)grvProductMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingAsc";
            }
            else
            {

                ((ImageButton)grvProductMaster.HeaderRow.FindControl(imgbtnSearch)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    #endregion
    
    
    #endregion

    #region Page Events

    protected void grvProductMaster_DataBound(object sender, EventArgs e)
    {
        if (grvProductMaster.Rows.Count > 0)
        {
            grvProductMaster.UseAccessibleHeader = true;
            grvProductMaster.HeaderRow.TableSection = TableRowSection.TableHeader;
            grvProductMaster.FooterRow.TableSection = TableRowSection.TableFooter;
        }

    }
    protected void grvProductMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if(e.Row.DataItem(""))
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActive = (Label)e.Row.FindControl("lblActive");
            CheckBox chkAct = (CheckBox)e.Row.FindControl("chkActive");
            if (lblActive.Text == "True")
            {
                chkAct.Checked = true;
            }

            //grvProductMaster.RowHeaderColumn.

            //User Authorization
            Label lblUserID = (Label)e.Row.FindControl("lblUserID");
            Label lblUserLevelID = (Label)e.Row.FindControl("lblUserLevelID");
            ImageButton imgbtnEdit = (ImageButton)e.Row.FindControl("imgbtnEdit");
            if (lblUserID.Text != "")
            {
                //Modified by saranya 10-Feb-2012 to validate user based on user level and Maker Checker
                //if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text))))
                if ((bModify) && (ObjUserInfo.IsUserLevelUpdate(Convert.ToInt32(lblUserID.Text), Convert.ToInt32(lblUserLevelID.Text),true)))
                {
                    imgbtnEdit.Enabled = true;
                }
                else
                {
                    imgbtnEdit.Enabled = false;
                    imgbtnEdit.CssClass = "styleGridEditDisabled";
                }
            }
            //Authorization code end   


        }

    }
    protected void grvProductMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(e.CommandArgument.ToString(), false, 0);
        switch (e.CommandName.ToLower())
        {
            case "modify":
                Response.Redirect("~/System Admin/S3GSysAdminProductMaster_Add.aspx?qsPId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=M");
                break;
            case "query":
                Response.Redirect("~/System Admin/S3GSysAdminProductMaster_Add.aspx?qsPId=" + FormsAuthentication.Encrypt(Ticket) + "&qsMode=Q");
                break;

        }
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        Response.Redirect(strRedirectPage,false);
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ProPageNumRW = 1;
            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPriBindGrid();
        }
        catch (FaultException<UserMgtServicesReference.ClsPubFaultException> objFaultExp)
        {
            lblErrorMessage.InnerText = objFaultExp.Detail.ProReasonRW;
        }
        catch (Exception ex)
        {
            lblErrorMessage.InnerText = ex.Message;
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }
    #endregion
    #region Paging and Searching Methods For Grid

    /// <summary>
    /// To Get search value to display value after sorting or paging changed
    /// </summary>

    private void FunPriGetSearchValue()
    {
        if (grvProductMaster.HeaderRow != null)
        {
            strSearchVal1 = ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderProductCode")).Text.Trim();
            strSearchVal2 = ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderProductName")).Text.Trim();
            strSearchVal3 = ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderLOBName")).Text.Trim();
            //strSearchVal4 = ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch4")).Text.Trim();
            //strSearchVal5 = ((TextBox)grvProductMaster.HeaderRow.FindControl("txtHeaderSearch5")).Text.Trim();
        }
    }

    public string FunPriGetURL(string strId)
    {
        return (strRedirectPage + "?pid=" + strId + "&mode=Q");
    }

    #endregion

    
}
