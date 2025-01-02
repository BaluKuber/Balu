using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class UserControls_AutoCompleteTextBox : System.Web.UI.UserControl
{
    int MinimumLength = 3;
    public int MinLength
    {
        get
        {
            return MinimumLength;
        }
        set
        {
            MinimumLength = value;
        }
    }
    public string Text
    {
        get
        {
            return txtAutoComplete.Text;
        }
        set
        {
            txtAutoComplete.Text = value;
        }
    }
    public string Value
    {
        get
        {
            return HdnSelValue.Value;
        }
        set
        {
            HdnSelValue.Value = value;
        }
    }
    Dictionary<String, String> Param = new Dictionary<String, String>();
    public Dictionary<String, String> Parameters
    {
        set
        {
            Param = value;
        }
    }
    String MethodName = "";
    public String ProcedureName
    {
        set
        {
            MethodName = value;
        }
    }
    String SFieldName = "";
    public String SearchFieldName
    {
        set
        {

            SFieldName = value;
        }

    }
    String VField = "";
    public String ValueField
    {
        set
        {
            VField = value;
        }
    }
    String TField = "";
    public String TextField
    {
        set
        {
            TField = value;
        }
    }
    public String ControlsClientID
    {

        set
        {
            HdnControlClientID.Value = value;
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
    public String ControlsParam
    {
        set
        {

            HdnControlParameters.Value = value;
        }

    }
    //Boolean AutoPostBackValue = false;
    public Boolean AutoPostBack
    {
        set
        {
            if (value)
            {
                HdnAutoPostBack.Value = "1";
            }
            else
            {
                HdnAutoPostBack.Value = "0";
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Control_ID = this.ID;

        String Par = "";
        foreach (var item in Param)
        {
            if (Par == "")
            {
                Par = item.Key + "," + item.Value;
            }
            else
            {
                Par += "#" + item.Key + "," + item.Value;
            }

        }

        if (!IsPostBack)
        {
            HdnOtherProps.Value = Convert.ToString(MinimumLength) + "|" + MethodName + "|" + SFieldName + "|" + TField + "|" + VField;
            HdnParams.Value = Par;
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), new Guid().ToString(), "<script>StartUp_" + this.ID + "('" + Par + "');</script>", false);
    }
    protected void BtnAsync_Click(object sender, EventArgs e)
    {

        if (SelectedIndexChanged != null)
        {
            SelectedIndexChanged(null, null);
        }

    }
    protected void BtnPostBack_Click(object sender, EventArgs e)
    {
        if (SelectedIndexChanged != null)
        {
            SelectedIndexChanged(null, null);
        }

    }
    public delegate void SelectedIndexChangedHandler(object sender, EventArgs e);
    public event SelectedIndexChangedHandler SelectedIndexChanged;
}
