#region PageHeader
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name               : Common control
/// Screen Name               : LOV User Control
/// Created By                : Tamilselvan.S
/// Created Date              : 09-Mar-2011
/// Purpose                   : 
/// Last Updated By           : 
/// Last Updated Date         : 
/// Reason                    :

/// <Program Summary>
#endregion

#region [Namespace]
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
using S3GBusEntity;
#endregion [Namespace]

public partial class UserControls_S3GCommonLOV : System.Web.UI.UserControl
{
    #region [Local Variables & Paging Properties]

    PagingValues ObjPaging = new PagingValues();                                // grid paging
    public delegate void LOVPageAssignValue(int ProPageNumRW, int intPageSize);
    Dictionary<string, string> Procparam = new Dictionary<string, string>();

    UserInfo ObjUserInfo;                                                       // to maintain the user information      
    ArrayList arrSearchVal = new ArrayList(1);
    int intNoofSearch = 2;

    static string[] arrSortCol = new string[] { };
    public int _ProPageNumRW = 1;
    public int ProPageNumRW                                                     // to retain the current page size and number
    {
        get { return _ProPageNumRW; }
        set { _ProPageNumRW = value; }
    }

    public int ProPageSizeRW
    {
        get;
        set;
    }

    public int _ucShow = 0;
    public int ucShow
    {
        get { return _ucShow; }
        set { _ucShow = value; }
    }

    private static string _LOVCode;
    public string LOVCode
    {
        get { return _LOVCode; }
        set { _LOVCode = value; }
    }

    private static string _gvDisplay;
    public string gvDisplay
    {
        get { return _gvDisplay; }
        set { _gvDisplay = value; }
    }

    public bool SetFocus
    {
        get;
        set;
    }

    #region [Dispaly Column Details]

    public int intNoofColumn = 2;
    public string[] arrColumns = new string[2];

    #endregion [Dispaly Column Details]

    #endregion [Local Variables & Paging Properties]

    #region [Event's]

    #region [Page Event's]

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (hdnShow.Value == "1") _ucShow = 1;

            if (_ucShow == 1)
            {
                //hdnShow.Value = "0";
                FunPubPageLoad();
            }
            //if (((TextBox)gvList.HeaderRow.FindControl("")) != null && ((TextBox)gvList.HeaderRow.FindControl("")).Text)

        }
        catch (Exception ex)
        { }
    }

    #endregion [Page Event's]

    #region [Grid Event's]

    protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //if (e.CommandName == "Select")
        //{
        if (e.CommandArgument.ToString().Trim() != "")
        {
            txtName.Text = e.CommandArgument.ToString();//((LinkButton)e.CommandSource).Text;
            hdnID.Value = e.CommandArgument.ToString();
            ucMPE.Hide();
            txtName.Focus();
        }
        else
        {
            Utility.FunShowAlertMsg((Control)sender, "Selected row does not have " + gvDisplay.Replace("_", " ") + " ");
            //ucMPE.Show();
        }

        //}
    }

    #endregion [Grid Event's]

    #region [Button Event's]

    protected void btnGetLOV_Click(object sender, EventArgs e)
    {
        try
        {
            if (arrSearchVal.Count > 0)
            {
                arrSearchVal.Clear();
            }
            ProPageNumRW = 1;

            hdnSearch.Value = "";
            hdnOrderBy.Value = "";
            FunPriClearSearchValue();
            FunPubOnPopupGridLoad();
        }
        catch (Exception ex)
        { }
    }

    #endregion [Button Event's]

    #region [Text Box Event's]

    protected void FunProHeaderSearch(object sender, EventArgs e)
    {
        intNoofSearch = arrColumns.Length;
        arrSearchVal = new ArrayList(intNoofSearch);
        string strSearchVal = string.Empty;
        TextBox txtboxSearch;
        try
        {
            txtboxSearch = ((TextBox)sender);
            FunPriGetSearchValue();
            //Replace the corresponding fields needs to search in sqlserver

            for (int iCount = 0; iCount < arrSearchVal.Count; iCount++)
            {
                if (arrSearchVal[iCount].ToString() != "")
                {
                    if (strSearchVal == "")
                        strSearchVal = " and " + arrColumns[iCount + 1].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                    else
                        strSearchVal += " and " + arrColumns[iCount + 1].ToString() + " like '%" + arrSearchVal[iCount].ToString() + "%'";
                }
            }

            hdnSearch.Value = strSearchVal;
            FunPubOnPopupGridLoad();
            if (txtboxSearch.Text != "")
                ((TextBox)gvList.HeaderRow.FindControl(txtboxSearch.ID)).Focus();

            ucMPE.Show();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    protected void FunProSortingColumn(object sender, EventArgs e)
    {
        intNoofSearch = arrColumns.Length;
        arrSearchVal = new ArrayList(intNoofSearch);
        var imgbtnSearch = string.Empty;
        try
        {
            LinkButton lnkbtnSearch = (LinkButton)sender;
            string strSortColName = string.Empty;
            //To identify image button which needs to get chnanged
            imgbtnSearch = lnkbtnSearch.ID.Replace("lnkbtn", "imgbtn");

            int ii = 0;
            foreach (string strclm in arrColumns)
            {
                if (lnkbtnSearch.ID == "lnkHeader" + ii.ToString())//strclm.Replace("_", ""))                
                {
                    strSortColName = strclm.ToString();
                    break;
                }
                ii++;
            }
            string strDirection = FunPriGetSortDirectionStr(strSortColName);
            FunPubOnPopupGridLoad();

            if (strDirection == "ASC")
            {
                ((ImageButton)gvList.HeaderRow.FindControl("imgbtnSearch" + ii)).CssClass = "styleImageSortingAsc";
            }
            else
            {
                ((ImageButton)gvList.HeaderRow.FindControl("imgbtnSearch" + ii)).CssClass = "styleImageSortingDesc";
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
        ucMPE.Show();
    }

    #endregion [Text Box Event's]

    #endregion [Event's]

    #region [Function's]

    public void FunPubPageLoad()
    {
        try
        {
            TextBox txtPageSize = (TextBox)ucLOVPageNavigater.FindControl("txtPageSize");
            TextBox txtGotoPage = (TextBox)ucLOVPageNavigater.FindControl("txtGotoPage");
            Label lblCurrentPAge = (Label)ucLOVPageNavigater.FindControl("lblCurrentPage");

            #region Grid Paging Config
            arrSearchVal = new ArrayList(intNoofSearch);
            //lblCurrentPAge.Text = txtGotoPage.Text=  lblCurrentPAge.Text = "";

            if (txtPageSize.Text != "")
                ProPageSizeRW = Convert.ToInt32(txtPageSize.Text.Split(',')[txtPageSize.Text.Split(',').Length - 1]);
            else
                ProPageSizeRW = Convert.ToInt32(ConfigurationManager.AppSettings.Get("GridPageSize"));

            if (lblCurrentPAge.Text != "")
                ProPageNumRW = Convert.ToInt32(lblCurrentPAge.Text);

            if (txtGotoPage.Text != "")
                ProPageNumRW = Convert.ToInt32(txtGotoPage.Text.Split(',')[txtGotoPage.Text.Split(',').Length - 1]);
            if (ProPageNumRW == 0)
                ProPageNumRW = 1;

            LOVPageAssignValue obj = new LOVPageAssignValue(this.AssignValue);
            ucLOVPageNavigater.callback = obj;
            ucLOVPageNavigater.ProPageNumRW = ProPageNumRW;
            ucLOVPageNavigater.ProPageSizeRW = ProPageSizeRW;

            if (!IsPostBack)
            {
                ucMPE.Hide();
            }
            else
            {
                hdnSearch.Value = "";
                FunPubOnPopupGridLoad();
            }
            #endregion
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    public void FunPubOnPopupGridLoad()
    {
        try
        {
            FunPubSetColumnToDispaly();
            FunPriSetHeaderText();
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex);
        }
    }

    private void FunPriSetHeaderText()
    {
        string strHeaderText = "";
        switch (LOVCode)
        {
            case "LOB":
                strHeaderText = "Line Of Business";
                break;
            case "CMD":
                strHeaderText = "Customer Master";
                break;
            case "USRM":
                strHeaderText = "User Master";
                break;
            case "RGN":
                strHeaderText = "Region Master";
                break;
            case "BRAH":
                strHeaderText = "Branch Master";
                break;
        }
        pnlLoadLOV.GroupingText = strHeaderText;
    }

    /// <summary>
    /// Will add the common parameters to the Dictionary - to pass it to the Common SP.
    /// </summary>
    private void FunPriAddCommonParameters()
    {
        //Paging Properties set  
        int intTotalRecords = 0;
        ObjPaging.ProCompany_ID = ObjUserInfo.ProCompanyIdRW;
        ObjPaging.ProUser_ID = ObjUserInfo.ProUserIdRW;
        ObjPaging.ProTotalRecords = intTotalRecords;
        ObjPaging.ProCurrentPage = ProPageNumRW;
        ObjPaging.ProPageSize = ProPageSizeRW;
        ObjPaging.ProSearchValue = hdnSearch.Value;
        ObjPaging.ProOrderBy = hdnOrderBy.Value;

        Procparam = new Dictionary<string, string>();
        if (Procparam != null)
        {
            Procparam.Clear();
        }
    }

    /// <summary>
    /// To Bind the Landing Grid
    /// </summary>
    /// <param name="intPageNum"> Current Page Number of the grid </param>
    /// <param name="intPageSize"> Current Page size of the grid </param>
    protected void AssignValue(int intPageNum, int intPageSize)
    {
        ProPageNumRW = intPageNum;              // To set the page Number
        ProPageSizeRW = intPageSize;            // To set the page size    
        FunPubOnPopupGridLoad();
    }

    private void FunPriGetSearchValue()
    {
        arrSearchVal = gvList.FunPriGetSearchValue(arrSearchVal, intNoofSearch);
    }

    private void FunPriClearSearchValue()
    {
        gvList.FunPriClearSearchValue(arrSearchVal);
    }

    private void FunPriSetSearchValue()
    {
        gvList.FunPriSetSearchValue(arrSearchVal);
    }

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

    public void FunPubSetColumnToDispaly()
    {
        DataTable dt1 = new DataTable();
        gvList.Columns.Clear();
        gvList.DataSource = null;
        gvList.DataBind();
        try
        {
            if (LOVCode == "FWUPD")
            {
                ObjUserInfo = new UserInfo();
                FunPriGetSearchValue();

                FunPriAddCommonParameters();                                                        // Adding the Common parameters to the dictionary            
                Procparam.Add("@IsActive", "1");
                if (Session["FU_TYPE"] != null)
                {
                    Procparam.Add("@FU_Type", Session["FU_TYPE"].ToString());
                }
                int intTotalRecords = 0;

                dt1 = Utility.GetGridData("S3G_CLN_GetFollowUpTypeDetails", Procparam, out intTotalRecords, ObjPaging);

                int intRowCnt = dt1.Rows.Count;
                arrColumns = new string[dt1.Columns.Count];
                int ii = 0;
                DataRow dr = null;

                foreach (DataColumn dc in dt1.Columns)
                {
                    arrColumns[ii] = dc.ColumnName;
                    ii++;
                    if (intRowCnt == 0)
                    {
                        dr = dt1.NewRow();
                        if (dc.DataType.FullName.ToString() == "System.Int32")
                            dr[dc.ColumnName] = 0;
                        else if (dc.DataType.FullName.ToString() == "System.DateTime")
                            dr[dc.ColumnName] = DateTime.Now;
                        else
                            dr[dc.ColumnName] = "";
                    }
                }
                if (intRowCnt == 0) dt1.Rows.Add(dr);

                ii = 0;
                foreach (string strclm in arrColumns)
                {
                    BoundField bf = new BoundField();
                    bf.DataField = strclm;
                    TemplateField tf = new TemplateField();
                    tf.HeaderTemplate = new GridViewTemplate(ListItemType.Header, strclm.Replace("_", " "), ii.ToString(), gvDisplay);
                    tf.ItemTemplate = new GridViewTemplate(ListItemType.Item, strclm, ii.ToString(), gvDisplay);
                    gvList.Columns.Add(tf);
                    ii++;
                }

                ucLOVPageNavigater.Visible = true;
                int intModevalue = intTotalRecords % ProPageSizeRW == 0 ? 0 : 1;
                if (Convert.ToInt32(intTotalRecords / ProPageSizeRW + intModevalue) >= ProPageNumRW)
                {
                    if (ProPageNumRW == 0)
                        ProPageNumRW = 1;
                    ucLOVPageNavigater.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                }
                else
                {
                    ProPageNumRW = Convert.ToInt32(intTotalRecords / ProPageSizeRW + intModevalue);
                    ucLOVPageNavigater.Navigation(intTotalRecords, ProPageNumRW, ProPageSizeRW);
                }
                ucLOVPageNavigater.setPageSize(ProPageSizeRW);

                gvList.DataSource = dt1;
                gvList.DataBind();
                if (intRowCnt == 0) gvList.Rows[0].Visible = false;

                ii = 0;
                if (gvList.HeaderRow != null)
                {
                    foreach (string strclm in arrColumns)
                    {
                        LinkButton lnkHeader = (LinkButton)gvList.HeaderRow.FindControl("lnkHeader" + ii.ToString());//strclm.Replace("_", ""));
                        TextBox txtHead = (TextBox)gvList.HeaderRow.FindControl("txtHeaderSearch" + ii.ToString());// strclm.Replace("_", ""));                   
                        if (txtHead != null)
                        {
                            lnkHeader.Click += new EventHandler(FunProSortingColumn);
                            txtHead.TextChanged += new EventHandler(FunProHeaderSearch);
                            txtHead.AutoPostBack = true;
                        }
                        ii++;
                    }
                }
                FunPriSetSearchValue();
                ucMPE.Show();
            }
        }
        catch (Exception ex)
        {
              ClsPubCommErrorLogDB.CustomErrorRoutine(ex, "Common User Control");
            throw;
        }
    }
    #endregion [Function's]

}
