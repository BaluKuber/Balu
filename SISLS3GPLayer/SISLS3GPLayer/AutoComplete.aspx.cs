using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class AutoComplete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //if (!IsPostBack)
        //{
            Dictionary<String, String> Param = new Dictionary<String, String>();
            //  Param.Add("Name", "Q");
            // Param.Add("Ql", "1");
            
           // AutoTxt.Parameters = Param;
          //  AutoTxt.ControlsClientID = ddlLocation.ClientID + "|" + hdnT.ClientID;
            //AutoTxt.ControlsParam = "@LocationCode" + "|" + "@Hel";
           
          
            
            if (!IsPostBack) {
                AutoTxt.TextField = "Customer_Name";
                AutoTxt.ValueField = "Customer_ID";
                AutoTxt.SearchFieldName = "@SearchText";
                AutoTxt.ProcedureName = "GetUserList";
                AutoTxt.AutoPostBack = false;
                AutoTxt.Text = "Santhosh";
                AutoTxt.Value = "2";


                AutoComplete1.TextField = "Customer_Name";
                AutoComplete1.ValueField = "Customer_ID";
                AutoComplete1.SearchFieldName = "@SearchText";
                AutoComplete1.ProcedureName = "GetUserList";
                AutoComplete1.AutoPostBack = true;
                AutoComplete1.Text = "Santhosh";
                AutoComplete1.Value = "2";    
            }

            //AutoComplete1.TextField = "Customer_Name";
            //AutoComplete1.ValueField = "Customer_ID";
            //AutoComplete1.SearchFieldName = "@SearchText";
            //AutoComplete1.ProcedureName = "GetUserList";
            //AutoComplete1.Parameters = Param;
            //AutoComplete1.SetDefaults();
        //}

    }
    protected void Index_Changed(object sender, EventArgs e)
    {
        string q = AutoTxt.Value;
    }
}
