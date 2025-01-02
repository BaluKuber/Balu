using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Drawing;

public partial class Add_TabControlPage : ApplyThemeForProject
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblHeading.Text = "Customer Enquiry";
            TabContainer1.Tabs[8].Visible = false;
            TabContainer1.Tabs[9].Visible = false;
            TabContainer1.Tabs[10].Visible = false;
            TabContainer1.Tabs[11].Visible = false;
            TabContainer1.Tabs[12].Visible = false;
            //TabContainer1.Tabs[13].Visible = false;
            TabContainer1.ActiveTabIndex = 0;
           
                ViewState["arl"] = new ArrayList();
                string strConnectionString = Convert.ToString(ConfigurationManager.AppSettings["connectionString3g"]);
                SqlConnection con = new SqlConnection(strConnectionString);
                SqlCommand cmd = new SqlCommand("select * from TestGrid", con);
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                ViewState["val"] = ds;

                GridView1.DataSource = ds;
                GridView1.DataBind();
                if (Session["Theme"] == "S3GTheme_Silver")
                {
                    imgbtnLeft.ImageUrl = "~/Images/layout_button_left_silver.gif";
                    imgbtnRight.ImageUrl = "~/Images/layout_button_right_silver.gif";
                }
                else
                {
                    imgbtnLeft.ImageUrl = "~/Images/layout_button_left.gif";
                    imgbtnRight.ImageUrl = "~/Images/layout_button_right.gif";
                }
        }
    }
   
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }

    protected void HeadSearch(object sender, EventArgs e)
    {
        TextBox txt = ((TextBox)sender);
        string val = ((TextBox)sender).Text;

        DataView dv = ((DataSet)ViewState["val"]).DefaultViewManager.CreateDataView(((DataSet)ViewState["val"]).Tables[0]);

        if (txt.ID == "txtHeadName")
        {
            dv.RowFilter = "name like '" + val + "%'";
        }
        else if (txt.ID == "txtHeadPlace")
        {
            dv.RowFilter = "place like '" + val + "%'";
        }
        else if (txt.ID == "txtHeadCapital")
        {
            dv.RowFilter = "capital like '" + val + "%'";
        }
        else if (txt.ID == "txtHeadCountry")
        {
            dv.RowFilter = "country like '" + val + "%'";
        }

        if (dv.Count > 0)
        {
            GridView1.DataSource = dv;
            GridView1.DataBind();
        }
        // txt.Text = val;
        GridView1.Columns[0].Visible = false;
        //lnkbtnEdit.Text = "Edit";
    }

    protected void CheckSelect(object sender, EventArgs e)
    {

        if (((CheckBox)sender).ID == "chkSelect")
        {
            if (((CheckBox)sender).Checked == true)
            {
                ((ArrayList)ViewState["arl"]).Add(((Label)((GridViewRow)((CheckBox)sender).NamingContainer).FindControl("lblHeadId")).Text);

            }
            else
            {
                ((ArrayList)ViewState["arl"]).Remove((((Label)((GridViewRow)((CheckBox)sender).NamingContainer).FindControl("lblHeadId")).Text));
            }

            GridView1.DataSource = (DataSet)ViewState["val"];
            GridView1.DataBind();
            GridView1.Columns[0].Visible = false;
            //lnkbtnEdit.Text = "Edit";
        }
        else
        {

            bool chk = ((CheckBox)sender).Checked;
            if (chk == true)
            {
                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    ((CheckBox)gvr.FindControl("chkSelect")).Checked = true;

                }

            }
            else
            {
                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    ((CheckBox)gvr.FindControl("chkSelect")).Checked = false;

                }
                GridView1.Columns[0].Visible = false;
            }

        }
    }

    protected void DoSort(object sender, EventArgs e)
    {
        string col = null;
        DataView dv = ((DataSet)ViewState["val"]).DefaultViewManager.CreateDataView(((DataSet)ViewState["val"]).Tables[0]);
        LinkButton lnkbtn = (LinkButton)sender;

        if (lnkbtn.ID == "lnkbtnSortName")
        {
            col = "Name";
        }
        else if (lnkbtn.ID == "lnkbtnSortPlace")
        {
            col = "Place";
        }
        else if (lnkbtn.ID == "lnkbtnSortCapital")
        {
            col = "Capital";
        }
        else if (lnkbtn.ID == "lnkbtnSortCountry")
        {
            col = "Country";
        }

        if (dv.Count > 0)
        {
            dv.Sort = col + " " + GetSortDirection(col);
            GridView1.DataSource = dv;
            GridView1.DataBind();
        }


    }

    private string GetSortDirection(string column)
    {

        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        return sortDirection;
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataSource = (DataSet)ViewState["val"];
        GridView1.DataBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((ArrayList)ViewState["arl"]).Contains(((Label)e.Row.FindControl("lblHeadId")).Text))
            {
                e.Row.BackColor = Color.SkyBlue;
                ((CheckBox)e.Row.FindControl("chkSelect")).Checked = true;
            }
        }
    }

    protected void imgbtnRight_Click(object sender, ImageClickEventArgs e)
    {
        

        if (TabContainer1.Tabs[0].Visible == true)
        {
            TabContainer1.Tabs[0].Visible = false;
            TabContainer1.Tabs[8].Visible = true;
            TabContainer1.ActiveTabIndex = 8;

        }
        else if (TabContainer1.Tabs[1].Visible == true)
        {
            TabContainer1.Tabs[1].Visible = false;
            TabContainer1.Tabs[9].Visible = true;
            TabContainer1.ActiveTabIndex = 9;
        }
        else if (TabContainer1.Tabs[2].Visible == true)
        {
            TabContainer1.Tabs[2].Visible = false;
            TabContainer1.Tabs[10].Visible = true;
            TabContainer1.ActiveTabIndex = 10;
        }
    }

    protected void imgbtnLeft_Click(object sender, ImageClickEventArgs e)
    {
        if (TabContainer1.Tabs[10].Visible == true)
        {
            TabContainer1.Tabs[2].Visible = true;
            TabContainer1.Tabs[10].Visible = false;
            TabContainer1.ActiveTabIndex = 2;

        }
        else if (TabContainer1.Tabs[9].Visible == true)
        {
            TabContainer1.Tabs[1].Visible = true;
            TabContainer1.Tabs[9].Visible = false;
            TabContainer1.ActiveTabIndex = 1;
        }
        else if (TabContainer1.Tabs[8].Visible == true)
        {
            TabContainer1.Tabs[0].Visible = true;
            TabContainer1.Tabs[8].Visible = false;
            TabContainer1.ActiveTabIndex = 0;
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {

        if (TabContainer1.ActiveTabIndex >= 7)
        {
            if (TabContainer1.Tabs[0].Visible == true)
            {
                TabContainer1.Tabs[0].Visible = false;
                TabContainer1.Tabs[8].Visible = true;
                TabContainer1.ActiveTabIndex = 8;

            }
            else if ((TabContainer1.Tabs[1].Visible == true)&& (TabContainer1.ActiveTabIndex ==8))
            {
                TabContainer1.Tabs[1].Visible = false;
                TabContainer1.Tabs[9].Visible = true;
                TabContainer1.ActiveTabIndex = 9;
            }
            else if ((TabContainer1.Tabs[2].Visible == true) && (TabContainer1.ActiveTabIndex ==9))
            {
                TabContainer1.Tabs[2].Visible = false;
                TabContainer1.Tabs[10].Visible = true;
                TabContainer1.ActiveTabIndex = 10;
            }
            else
            {
                TabContainer1.ActiveTabIndex = TabContainer1.ActiveTabIndex + 1;
            }
        }
        else
        {
            TabContainer1.ActiveTabIndex = TabContainer1.ActiveTabIndex + 1;
        }
    }
}

