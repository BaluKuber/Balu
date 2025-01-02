using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class UserControls_S3GMultiSelect : System.Web.UI.UserControl
{
    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    UserInfo ObjUserInfo;

    public delegate void ddlMultiItem_Selected(object Sender, EventArgs e);
    public event ddlMultiItem_Selected OnMultiItem_Selected;

    public string SelectedValue
    {
        get { return hdnSelectedValue.Text; }
        set { hdnSelectedValue.Text = value; }
    }

    public string SelectedText
    {
        get { return txtItemName.Text; }
        set { txtItemName.Text = value; }
    }

    public bool AutoPostBack
    {
        get
        {
            if (hdnAutoPostBack.Value == "0")
                return false;
            else
                return true;
        }
        set
        {
            if (value)
                hdnAutoPostBack.Value = "1";
            else
                hdnAutoPostBack.Value = "0";
        }
    }

    public Unit Width
    {
        set
        {
            txtItemName.Width = value;
            grvList.Width = Unit.Pixel(28 + Convert.ToInt32(value.Value));
            tblContent.Width = Unit.Pixel(38 + Convert.ToInt32(value.Value)).ToString();
        }
    }

    public string Control_ID
    {
        get
        {
            return hdnControlID.Value;
        }
        set
        {
            hdnControlID.Value = value;
        }
    }

    public void Clear()
    {
        grvList.DataSource = null;
        grvList.DataBind();

        hdnSelectedValue.Text = "0";
        txtItemName.Text = "--Select--";
    }

    public bool IsMandatory
    {
        get { return rfvMultiSelect.Enabled; }
        set { rfvMultiSelect.Enabled = value; }
    }

    public string ValidationGroup
    {
        get { return rfvMultiSelect.ValidationGroup; }
        set { rfvMultiSelect.ValidationGroup = value; }
    }

    public string ErrorMessage
    {
        get { return rfvMultiSelect.ErrorMessage; }
        set { rfvMultiSelect.ErrorMessage = value; }
    }

    public bool TEnabled
    {
        get { return grvList.Enabled; }
        set { grvList.Enabled = value; }
    }

    public string DisableCheck
    {
        get { return hdnDisableCheck.Value; }
        set { hdnDisableCheck.Value = value; }
    }

    private static string _DataTextField;
    public string DataTextField
    {
        get { return _DataTextField; }
        set { _DataTextField = value; }
    }

    private static string _DataValueField;
    public string DataValueField
    {
        get { return _DataValueField; }
        set { _DataValueField = value; }
    }

    public DataTable DataSource
    {
        set
        {
            if (value != null && value.Columns.Count >= 2)
            {
                if (string.IsNullOrEmpty(DataValueField))
                {
                    DataValueField = value.Columns[0].ColumnName;
                }

                if (string.IsNullOrEmpty(DataTextField))
                {
                    DataTextField = value.Columns[1].ColumnName;
                }

                value.Columns[DataValueField].ColumnName = "ID";
                value.Columns[DataTextField].ColumnName = "Text";
            }


            grvList.DataSource = value;
            grvList.DataBind();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Control_ID = this.ID;

        grvList.GridLines = GridLines.None;
        grvList.Style.Add("cellspacing", "0");
        grvList.CssClass = "styleTableData";

        tblContent.Attributes.Add("onfocusout", "javascript:fnHidePopup_" + Control_ID + "('pnlDropDownView');");
        tblContent.Attributes.Add("onfocusin", "javascript:fnShowPopup_" + Control_ID + "('pnlDropDownView');");
        tblInner.Attributes.Add("onmouseover", "javascript:fnMouseoverTooltip_" + Control_ID + "('" + tblInner.ClientID + "');");
        txtItemName.Attributes.Add("readonly", "readonly");
    }

    protected void btnItem_Selected(object sender, EventArgs e)
    {
        if (OnMultiItem_Selected != null)
        {
            OnMultiItem_Selected(sender, e);
        }
    }

    protected void grvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Control_ID = this.ID;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chkAll = grvList.HeaderRow.FindControl("chkAll") as CheckBox;
            CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
            Label lblText = e.Row.FindControl("lblText") as Label;
            chkSelect.Text = lblText.Text;
            if (chkSelect != null)
            {
                chkSelect.Attributes.Add("onclick", "javascript:fnSelectOne_DDL_" + Control_ID + "(" + chkSelect.ClientID + "," + chkAll.ClientID + ");");
            }
            if (hdnDisableCheck.Value == "0")
            {
                chkSelect.Checked = false;
                chkAll.Checked = false;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Header)
        {
            CheckBox chkAll = e.Row.FindControl("chkAll") as CheckBox;
            chkAll.Attributes.Add("onclick", "javascript:fnSelectAll_DDL_" + Control_ID + "(" + chkAll.ClientID + ",'chkSelect');");
        }
        
    }
}
