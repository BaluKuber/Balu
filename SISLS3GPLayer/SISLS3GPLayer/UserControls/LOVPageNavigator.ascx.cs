#region Page Header
/// © 2010 SUNDARAM INFOTECH SOLUTIONS P LTD . All rights reserved
/// 
/// <Program Summary>
/// Module Name			: Common  
/// Screen Name			: LOV Page Navigator
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

public partial class UserControls_LOVPageNavigator : System.Web.UI.UserControl
{
    #region Paging Properties

    int intCurrentPageNum = 0;

    int intCurrentPageSize = 0;

    private System.Delegate delegateValue;

    public System.Delegate callback
    {
        set
        {
            delegateValue = value;
        }
        //get
        //{
        //    return delegateValue;
        //}
    }

    public Int32 ProPageNumRW
    {
        get { return intCurrentPageNum; }
        set { intCurrentPageNum = value; }
    }

    public Int32 ProPageSizeRW
    {
        get { return intCurrentPageSize; }
        set { intCurrentPageSize = value; }
    }

    #endregion

    #region Page Methods

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Convert.ToInt32(lblCurrentPage.Text) == 1 || Convert.ToInt32(lblTotalPages.Text) == 1)
        {
            btnPrevious.Enabled = false;
        }
        else
        {
            btnPrevious.Enabled = true;
        }
        if (Convert.ToInt32(lblCurrentPage.Text) == Convert.ToInt32(lblTotalPages.Text))
        {
            btnNext.Enabled = false;
        }
        else
        {
            btnNext.Enabled = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //txtGotoPage.Text = txtGotoPage.Text.Split(',')[txtGotoPage.Text.Split(',').Length - 1];
        //lblCurrentPage.Text = lblCurrentPage.Text.Split(',')[lblCurrentPage.Text.Split(',').Length - 1];
        //txtPageSize.Text = txtPageSize.Text.Split(',')[txtPageSize.Text.Split(',').Length - 1];

    }

    public void Navigation(int intTotRecords, int intPageNumRW, int intPageSizeRW)
    {
        ProPageNumRW = intPageNumRW;
        ProPageSizeRW = intPageSizeRW;
        Double totPage = Math.Ceiling(((double)intTotRecords / ProPageSizeRW));
        if ((intTotRecords == 1) || (totPage == 1))
        {
            lblTotalPages.Text = "1";
        }
        txtGotoPage.Text = ProPageNumRW.ToString();
        lblCurrentPage.Text = ProPageNumRW.ToString();
        txtPageSize.Text = ProPageSizeRW.ToString();
        lblTotalPages.Text = totPage.ToString();
        hdnTotRec.Value = intTotRecords.ToString();
        lblTotalRecords.Text = "No of records : " + intTotRecords.ToString();
        if (intTotRecords == 0)
        {
            trMessage.Visible = true;
            trControl.Visible = false;
        }
        else
        {
            trMessage.Visible = false;
            trControl.Visible = true;
        }
    }

    public void setPageSize(int intPageSizeRW)
    {
        //if (intPageSizeRW > 100)
        //{
        //    txtPageSize.Text = "100";
        //}
        //else
        txtPageSize.Text = intPageSizeRW.ToString();
    }

    #endregion

    #region Page Events

    protected void Navigation_Link(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Prev":
                ProPageNumRW = Convert.ToInt32(lblCurrentPage.Text) - 1;
                break;
            case "Next":
                ProPageNumRW = Convert.ToInt32(lblCurrentPage.Text) + 1;
                break;
        }
        ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
        delegateValue.DynamicInvoke(ProPageNumRW, ProPageSizeRW);
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        int intMaxPage = 0;
        if (string.IsNullOrEmpty(txtGotoPage.Text.Trim()))
        {
            txtGotoPage.Text = "1";
        }
        else if (Convert.ToInt32(txtGotoPage.Text) == 0)
        {
            txtGotoPage.Text = "1";
        }
        intMaxPage = Convert.ToInt32(lblTotalPages.Text);
        int intGotoPage = Convert.ToInt32(txtGotoPage.Text);
        if (intGotoPage <= intMaxPage)
        {
            ProPageNumRW = intGotoPage;
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text);
            delegateValue.DynamicInvoke(ProPageNumRW, ProPageSizeRW);
        }
        else
        {
            txtGotoPage.Text = intMaxPage.ToString();
        }

    }

    protected void btnSize_Click(object sender, EventArgs e)
    {
        ProPageNumRW = 1;
        ProPageSizeRW = (txtPageSize.Text.Trim() == string.Empty) ? 1 : Convert.ToInt32(txtPageSize.Text) == 0 ? 1 : Convert.ToInt32(txtPageSize.Text);
        delegateValue.DynamicInvoke(ProPageNumRW, ProPageSizeRW);
        ViewState["LOVPageSize"] = ProPageSizeRW;
    }

    #endregion
}
