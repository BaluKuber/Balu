using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

/// Created By : Thangam & Rajendran
/// Created Date : 05/01/2010
/// Purpose     : Customer Details View Panel  


public partial class UserControls_S3GCustomerAddress : System.Web.UI.UserControl
{
    /// <summary>
    /// void
    /// Fill the controls with the given data row 
    /// The data columns should be fetched as it is
    /// </summary>
    /// <param name="dtrCustomer">
    /// Data row which is having the information
    /// </param>
    /// <param name="IsCommunication"> 
    /// True - if it is communication address
    /// </param>
    public void SetCustomerDetails(DataRow dtrCustomer, bool IsCommunication)
    {
        if (dtrCustomer != null)
        {
            if (dtrCustomer.Table.Columns["Customer_Code"] != null)
                txtCustomerCode1.Text = txtCustomerCode.Text = dtrCustomer["Customer_Code"].ToString();

            if (dtrCustomer.Table.Columns["Title"] != null)
                txtCustomerName.Text = txtCustomerName1.Text = dtrCustomer["Title"].ToString() + " " + dtrCustomer["Customer_Name"].ToString();
            else
                txtCustomerName.Text = txtCustomerName1.Text = dtrCustomer["Customer_Name"].ToString();

            if (IsCommunication)
            {
                if (dtrCustomer.Table.Columns["Comm_Mobile"] != null) txtMobile.Text = txtMobile1.Text = dtrCustomer["Comm_Mobile"].ToString();
                if (dtrCustomer.Table.Columns.Contains("Comm_Telephone"))
                {
                    txtPhone.Text = txtPhone1.Text = dtrCustomer["Comm_Telephone"].ToString();
                }
                if (dtrCustomer.Table.Columns["Comm_Email"] != null) txtEmail.Text = txtEmail1.Text = dtrCustomer["Comm_Email"].ToString();
                if (dtrCustomer.Table.Columns["Comm_WebSite"] != null) txtWebSite.Text = txtWebSite1.Text = dtrCustomer["Comm_WebSite"].ToString();
                txtCusAddress.Text = txtCusAddress1.Text = Utility.SetCustomerAddress(dtrCustomer);
            }
            else
            {
                if (dtrCustomer.Table.Columns["Perm_Mobile"] != null) txtMobile.Text = txtMobile1.Text = dtrCustomer["Perm_Mobile"].ToString();
                if (dtrCustomer.Table.Columns.Contains("Perm_Telephone"))
                {
                    txtPhone.Text = txtPhone1.Text = dtrCustomer["Perm_Telephone"].ToString();
                }
                if (dtrCustomer.Table.Columns["Perm_Email"] != null) txtEmail.Text = txtEmail1.Text = dtrCustomer["Perm_Email"].ToString();
                if (dtrCustomer.Table.Columns["Perm_WebSite"] != null) txtWebSite.Text = txtWebSite1.Text = dtrCustomer["Perm_WebSite"].ToString();
                txtCusAddress.Text = txtCusAddress1.Text = Utility.SetCustomerPerAddress(dtrCustomer);
            }
        }
    }

    public void SetCustomerDetails(int intCustomer_ID, bool IsCommunication)
    {
        if (intCustomer_ID > 0)
        {
            Dictionary<string, string> Procparam = new Dictionary<string, string>();

            Procparam.Add("@Customer_ID", intCustomer_ID.ToString());
            DataTable dtCustomer = Utility.GetDefaultData("S3G_GetCustomerDetails", Procparam);

            if (dtCustomer.Rows.Count > 0)
            {
                DataRow dtrCustomer = dtCustomer.Rows[0];

                if (dtrCustomer.Table.Columns["Customer_Code"] != null) txtCustomerCode1.Text = txtCustomerCode.Text = dtrCustomer["Customer_Code"].ToString();

                if (dtrCustomer.Table.Columns["Title"] != null)
                    txtCustomerName.Text = txtCustomerName1.Text = dtrCustomer["Title"].ToString() + " " + dtrCustomer["Customer_Name"].ToString();
                else
                    txtCustomerName.Text = txtCustomerName1.Text = dtrCustomer["Customer_Name"].ToString();

                if (IsCommunication)
                {
                    if (dtrCustomer.Table.Columns["Comm_Mobile"] != null) txtMobile.Text = txtMobile1.Text = dtrCustomer["Comm_Mobile"].ToString();
                    if (dtrCustomer.Table.Columns.Contains("Comm_Telephone"))
                    {
                        txtPhone.Text = txtPhone1.Text = dtrCustomer["Comm_Telephone"].ToString();
                    }
                    txtEmail.Text = txtEmail1.Text = dtrCustomer["Comm_Email"].ToString();
                    txtWebSite.Text = txtWebSite1.Text = dtrCustomer["Comm_WebSite"].ToString();
                    txtCusAddress.Text = txtCusAddress1.Text = Utility.SetCustomerAddress(dtrCustomer);
                }
                else
                {
                    txtMobile.Text = txtMobile1.Text = dtrCustomer["Perm_Mobile"].ToString();
                    if (dtrCustomer.Table.Columns.Contains("Perm_Telephone"))
                    {
                        txtPhone.Text = txtPhone1.Text = dtrCustomer["Perm_Telephone"].ToString();
                    }
                    txtEmail.Text = txtEmail1.Text = dtrCustomer["Perm_Email"].ToString();
                    txtWebSite.Text = txtWebSite1.Text = dtrCustomer["Perm_WebSite"].ToString();
                    txtCusAddress.Text = txtCusAddress1.Text = Utility.SetCustomerPerAddress(dtrCustomer);
                }
            }
            else
            {
                ClearCustomerDetails();
            }
        }
    }

    /// <summary>
    /// void
    /// Fill the controls with the given user defined column value 
    /// </summary>
    /// <param name="customerCode"></param>
    /// <param name="Com_Address"></param>
    /// <param name="customerName"></param>
    /// <param name="Com_Mobile"></param>
    /// <param name="Com_Email"></param>
    /// <param name="Com_WebSite"></param>

    public void SetCustomerDetails(string customerCode, string Com_Address, string customerName, string Com_Phone, string Com_Mobile, string Com_Email, string Com_WebSite)
    {
        txtCustomerCode1.Text = txtCustomerCode.Text = customerCode;
        txtCustomerName.Text = txtCustomerName1.Text = customerName;
        txtCusAddress.Text = txtCusAddress1.Text = Com_Address;
        txtMobile.Text = txtMobile1.Text = Com_Mobile;
        txtPhone.Text = txtPhone1.Text = Com_Phone;
        txtEmail.Text = txtEmail1.Text = Com_Email;
        txtWebSite.Text = txtWebSite1.Text = Com_WebSite;
    }

    public void SetCustomerDetails(string customerCode, string Com_Address, string customerName, string Com_Mobile, string Com_Email, string Com_WebSite)
    {
        txtCustomerCode1.Text = txtCustomerCode.Text = customerCode;
        txtCustomerName.Text = txtCustomerName1.Text = customerName;
        txtCusAddress.Text = txtCusAddress1.Text = Com_Address;
        txtMobile.Text = txtMobile1.Text = Com_Mobile;
        txtEmail.Text = txtEmail1.Text = Com_Email;
        txtWebSite.Text = txtWebSite1.Text = Com_WebSite;
    }

    /// <summary>
    /// void 
    /// Clear the values 
    /// </summary>
    public void ClearCustomerDetails()
    {
        txtCustomerCode1.Text = txtCustomerCode.Text =
        txtCustomerName.Text = txtCustomerName1.Text =
        txtMobile.Text = txtMobile1.Text =
        txtPhone.Text = txtPhone1.Text =
        txtEmail.Text = txtEmail1.Text =
        txtWebSite.Text = txtWebSite1.Text =
        txtCusAddress.Text = txtCusAddress1.Text = string.Empty;

    }

    public String CustomerId { get; set; }

    /// <summary>
    /// int 
    /// 0 - To show Vertical view
    /// 1 - To show Horizontal view
    /// </summary>
    public int ActiveViewIndex { set { MCustomerView.ActiveViewIndex = value; } }

    /// <summary>
    /// bool 
    /// Determine the visibility of Code field
    /// </summary>
    public bool ShowCustomerCode
    {
        set
        {
            V1FirstColumn1.Visible = V1SecondColumn1.Visible =
            FirstColumn1.Visible = SecondColumn1.Visible = value;
        }
    }

    /// <summary>
    /// bool
    /// Determine the visibility of Name field
    /// </summary>
    public bool ShowCustomerName
    {
        set
        {
            V1FirstColumn2.Visible = V1SecondColumn2.Visible =
            ThirdColumn1.Visible = FourthColumn1.Visible = value;
        }
    }

    /// <summary>
    /// bool
    /// Determine the visibility of Name field
    /// </summary>
    public bool ShowTelePhone
    {
        set
        {
            lblMobile.Visible = lblMobile1.Visible =
            txtPhone.Visible = txtPhone1.Visible = false;

            lblPhone.Text = lblPhone1.Text = "Mobile";
        }
    }

    /// <summary>
    /// string
    /// Set the width of first column
    /// </summary>
    public string FirstColumnWidth
    {
        set
        {
            V1FirstColumn1.Width = V1FirstColumn2.Width =
            V1FirstColumn3.Width = V1FirstColumn4.Width =
            V1FirstColumn5.Width = V1FirstColumn6.Width =
            FirstColumn1.Width = FirstColumn2.Width =
            FirstColumn3.Width = FirstColumn4.Width = value;
        }
    }

    /// <summary>
    /// string
    /// Set the width of second column
    /// </summary>
    public string SecondColumnWidth
    {
        set
        {
            V1SecondColumn1.Width = V1SecondColumn2.Width =
            V1SecondColumn3.Width = V1SecondColumn4.Width =
            V1SecondColumn5.Width = V1SecondColumn6.Width =
            SecondColumn1.Width = SecondColumn2.Width = value;
        }
    }

    /// <summary>
    /// string
    /// Set the width of third column
    /// </summary>
    public string ThirdColumnWidth
    {
        set
        {
            ThirdColumn1.Width = ThirdColumn2.Width =
            ThirdColumn3.Width = ThirdColumn4.Width = value;
        }
    }

    /// <summary>
    /// string
    /// Set the width of fourth column
    /// </summary>
    public string FourthColumnWidth
    {
        set
        {
            FourthColumn1.Width = FourthColumn2.Width =
            FourthColumn3.Width = FourthColumn4.Width = value;
        }
    }

    /// <summary>
    /// string
    /// Set the style for first column
    /// </summary>
    public string FirstColumnStyle
    {
        set
        {
            V1FirstColumn1.Attributes.Add("class", value);
            V1FirstColumn2.Attributes.Add("class", value);
            V1FirstColumn3.Attributes.Add("class", value);
            V1FirstColumn4.Attributes.Add("class", value);
            V1FirstColumn5.Attributes.Add("class", value);
            V1FirstColumn6.Attributes.Add("class", value);
        }
    }

    /// <summary>
    /// string
    /// Set the style for second column
    /// </summary>
    public string SecondColumnStyle
    {
        set
        {
            V1SecondColumn1.Attributes.Add("class", value);
            V1SecondColumn2.Attributes.Add("class", value);
            V1SecondColumn3.Attributes.Add("class", value);
            V1SecondColumn4.Attributes.Add("class", value);
            V1SecondColumn5.Attributes.Add("class", value);
            V1SecondColumn6.Attributes.Add("class", value);
        }
    }

    /// <summary>
    /// string
    /// Get the Code value
    /// </summary>
    public string CustomerCode
    {
        get
        {
            return txtCustomerCode.Text;
        }
    }

    /// <summary>
    /// string
    /// Get the Name value
    /// </summary>
    public string CustomerName
    {
        get
        {
            return txtCustomerName.Text;
        }
    }

    /// <summary>
    /// string
    /// Get the address value
    /// </summary>
    public string CustomerAddress
    {
        get
        {
            return txtCusAddress.Text;
        }
    }

    /// <summary>
    /// string
    /// Get the Mobile Number
    /// </summary>
    public string Mobile
    {
        get
        {
            return txtMobile.Text;
        }
    }

    /// <summary>
    /// string
    /// Get the Phone Number
    /// </summary>
    public string TelePhone
    {
        get
        {
            return txtPhone.Text;
        }
    }

    /// <summary>
    /// string
    /// Get the EMail ID
    /// </summary>
    public string EmailID
    {
        get
        {
            return txtEmail.Text;
        }
    }

    /// <summary>
    /// string
    /// Get the Website
    /// </summary>
    public string Website
    {
        get
        {
            return txtWebSite.Text;
        }
    }

    /// <summary>
    /// string
    /// Set the value to display the caption
    /// </summary>
    public string Caption
    {
        set
        {
            txtCustomerCode.ToolTip =
            txtCustomerCode1.ToolTip =
            lblCustomerCode.Text =
            lblCustomerCode1.Text = value + " Code";

            txtCustomerName.ToolTip =
            txtCustomerName1.ToolTip =
            lblCustomerName.Text =
            lblCustomerName1.Text = value + " Name";
        }
    }
}
