using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;

public partial class UserControls_S3GAutoSuggest : System.Web.UI.UserControl
{
    Dictionary<string, string> Procparam = new Dictionary<string, string>();
    UserInfo ObjUserInfo;

    public delegate void ddlItem_Selected(object Sender, EventArgs e);
    public event ddlItem_Selected Item_Selected;

    /// <summary>
    /// string AutoSuggest.SelectedValue
    /// Get or Set the of the selected item's value in the list control
    /// </summary>
    public string SelectedValue
    {
        get { return hdnSelectedValue.Text; }
        set { hdnSelectedValue.Text = value; }
    }

    /// <summary>
    /// string AutoSuggest.SelectedText
    /// Get or Set the of the selected item's text in the list control
    /// </summary>
    public string SelectedText
    {
        get { return txtItemName.Text; }
        set { txtItemName.Text = value; }
    }

    public string ToolTip
    {
        get { return txtItemName.ToolTip; }
        set { txtItemName.ToolTip = value; }
    }

    /// <summary>
    /// bool AutoSuggest.AutoPostBack
    /// Get or Set a value indicating whether a postback to the server automactically occurs when user changes the list selection.
    /// </summary>
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

    /// <summary>
    /// bool AutoSuggest.ReadOnly
    /// Get or Set a value indicating whether the AutoSuggest is readonly mode.
    /// </summary>
    public bool ReadOnly
    {
        get
        {
            return txtItemName.ReadOnly;
        }
        set
        {
            txtItemName.ReadOnly = value;
            autoCompletor.Enabled = !value;
        }
    }

    /// <summary>
    /// bool AutoSuggest.Enabled
    /// Get or Set a value indicating whether the AutoSuggest is Enabled mode.
    /// </summary>
    public bool Enabled
    {
        get
        {
            return txtItemName.Enabled;
        }
        set
        {
            txtItemName.Enabled = value;
            autoCompletor.Enabled = value;
        }
    }

    /// <summary>
    /// Unit AutoSuggest.Width
    /// Get or Set a Width of the AutoSuggest.
    /// </summary>
    public Unit Width
    {
        set
        {
            txtItemName.Width = value;
            tblContent.Width = Unit.Pixel(38 + Convert.ToInt32(value.Value)).ToString();
        }
    }

    /// <summary>
    /// string AutoSuggest.Control_ID
    /// Get or Set the name of the Auto suggest control.
    /// </summary>
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

    /// <summary>
    /// void AutoSuggest.Clear()
    /// Cleat the selected item of the Auto suggest control.
    /// </summary>
    public void Clear()
    {
        hdnSelectedValue.Text = "0";
        txtItemName.Text = "";
        _AvailableRecords = 0;
    }

    public enum ValidateItem
    {
       Text = 0,
       Value = 1,
    }

    public static int _AvailableRecords;
    public int AvailableRecords
    {
        get
        {
            return _AvailableRecords;
        }
        set
        {
            _AvailableRecords = value;
        }
    }

    public static ValidateItem _ItemToValidate;
    public ValidateItem ItemToValidate
    {
        get
        {
            return _ItemToValidate;
        }
        set
        {
            _ItemToValidate = value;
            if (_ItemToValidate == ValidateItem.Text)
            {
                rfvMultiSuggest.ControlToValidate = "txtItemName";
                txtBranchSearchExtender.Enabled = false;
                rfvMultiSuggest.InitialValue = "";
            }
            else
            {
                rfvMultiSuggest.ControlToValidate = "hdnSelectedValue";
                txtBranchSearchExtender.Enabled = true;
            }
        }
    }

    public bool FirstRowSelected
    {
        set
        {
            autoCompletor.FirstRowSelected = value;
        }
    }

    public TextBox TextBox
    {
        get { return txtItemName; }
    }

    public bool IsMandatory
    {
        get { return rfvMultiSuggest.Enabled; }
        set { rfvMultiSuggest.Enabled = value; }
    }

    public string ServiceMethod
    {
        set { autoCompletor.ServiceMethod = value; }
    }

    public string ServicePath
    {
        set { autoCompletor.ServicePath = value; }
    }

    public string ValidationGroup
    {
        get { return rfvMultiSuggest.ValidationGroup; }
        set { rfvMultiSuggest.ValidationGroup = value; }
    }

    public string ErrorMessage
    {
        get { return rfvMultiSuggest.ErrorMessage; }
        set { rfvMultiSuggest.ErrorMessage = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Control_ID = this.ID;

        autoCompletor.OnClientItemSelected = "AutoSuggest_ItemSelected";
        autoCompletor.OnClientPopulated = "AutoSuggest_ItemPopulated";
        autoCompletor.OnClientShown = "AutoSuggest_ShowOptions";
        autoCompletor.ServicePath = this.Parent.Page.Request.FilePath;
    }




    protected void btnItem_Selected(object sender, EventArgs e)
    {
        if (Item_Selected != null)
        {
            Item_Selected(sender, e);
        }
        this.Focus();
    }
    public string WatermarkText
    {
        set
        {
            txtBranchSearchExtender.WatermarkText = value;
        }
    }
}
