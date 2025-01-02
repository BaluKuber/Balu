
/// Screen Name     :   PageNavigator.ascx
/// Created By      :   Kaliraj K
/// Created Date    :   06-Jun-2010
/// Purpose         :   User control for Grid view paging

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

public partial class PageNavigator : System.Web.UI.UserControl
{

    #region Paging Properties

    int intCurrentPageNum = 0;

    int intCurrentPageSize = 0;

    private System.Delegate delegateValue;
    private System.Delegate delegateValue1;

    public System.Delegate callback
    {
        set
        {
            delegateValue = value;
        }
    }

    public System.Delegate callback1
    {
        set
        {
            delegateValue1 = value;
        }
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
            btnFirst.Enabled = false;
            btnPrevious.Enabled = false;

            btnFirst.ImageUrl = "../Images/First_Desabled.gif";
            btnPrevious.ImageUrl = "../Images/Prev_Desabled.gif";
        }
        else
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = true;

            btnFirst.ImageUrl = "../Images/First.gif";
            btnPrevious.ImageUrl = "../Images/Prev.gif";
        }
        if (Convert.ToInt32(lblCurrentPage.Text) == Convert.ToInt32(lblTotalPages.Text))
        {
            btnLast.Enabled = false;
            btnNext.Enabled = false;

            btnLast.ImageUrl = "../Images/Last_Desabled.gif";
            btnNext.ImageUrl = "../Images/Next_Desabled.gif";
        }
        else
        {
            btnLast.Enabled = true;
            btnNext.Enabled = true;

            btnLast.ImageUrl = "../Images/Last.gif";
            btnNext.ImageUrl = "../Images/Next.gif";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Navigation(int intTotRecords, int intPageNumRW, int intPageSizeRW)
    {
        ProPageNumRW = intPageNumRW;
        ProPageSizeRW = intPageSizeRW;
        if (ProPageSizeRW == 0)
        {
            ProPageSizeRW = 1;
        }
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
        lblTotalRecords.Text = "No. of records : " + intTotRecords.ToString();
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
        if (intPageSizeRW == 0)
        {
            intPageSizeRW = 1;

        }
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
            case "First":

                ProPageNumRW = 1;
                break;
            case "Prev":
                ProPageNumRW = Convert.ToInt32(lblCurrentPage.Text) - 1;
                break;
            case "Next":
                ProPageNumRW = Convert.ToInt32(lblCurrentPage.Text) + 1;
                break;
            case "Last":
                ProPageNumRW = Convert.ToInt32(lblTotalPages.Text);
                break;
        }
        ProPageSizeRW = Convert.ToInt32(txtPageSize.Text.Trim());

        if (delegateValue == null)
        {
            delegateValue1.DynamicInvoke(ProPageNumRW, ProPageSizeRW);
        }

        else
        {

            delegateValue.DynamicInvoke(ProPageNumRW, ProPageSizeRW);
        }
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
        int intGotoPage = Convert.ToInt32(txtGotoPage.Text.Trim());
        if (intGotoPage <= intMaxPage)
        {
            ProPageNumRW = intGotoPage;
            ProPageSizeRW = Convert.ToInt32(txtPageSize.Text.Trim());
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

        if (delegateValue != null)
        {
            delegateValue.DynamicInvoke(ProPageNumRW, ProPageSizeRW);
        }
        else
        {
            delegateValue1.DynamicInvoke(ProPageNumRW, ProPageSizeRW);
        }
        ViewState["PageSize"] = ProPageSizeRW;
    }

    #endregion

}
