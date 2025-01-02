using System;
using System.Threading;
using System.Resources;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class ProductMaster : ApplyThemeForProject
{
    private bool blnCheck = false;
    private bool blnSortStatus = false;
    int intCheck = 0;
    private Int32 intCount;

    SqlConnection sqlcon;
    PagedDataSource pdsPagerDataSource;
    GridViewRow gvrData;
    DataRow drData;
    DataView dvData;
    DataTable dtData;
    IEnumerator ienumData;
    DataRowView drvData;

    public ProductMaster()
    {
        System.Configuration.AppSettingsReader appread = new System.Configuration.AppSettingsReader();
        sqlcon = new SqlConnection((string)appread.GetValue("connectionString", typeof(string)));
        pdsPagerDataSource = new PagedDataSource();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            lblMessage.Text = "";
            GetSortStatusRW = false;
            ViewState["imgbtnLast_Click"] = null;
            ViewState["imgbtnNext_Click"] = null;
            ViewState["imgbtnPrev_Click"] = null;
            if (!IsPostBack)
            {
                Default();
                
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnEdit.Text == "Edit")
            {
                foreach (GridViewRow  gvrData in gvMaster.Rows)
                {
                    if (((Label)gvrData.FindControl("lblName")).Text.Trim().ToLower().Contains("(") == false)
                    {
                        if (((CheckBox)gvrData.FindControl("chkSelect")).Checked == true)
                        {
                            ((Label)gvrData.FindControl("lblName")).Visible = false;
                            ((TextBox)gvrData.FindControl("txtName")).Visible = true;
                            ((Label)gvrData.FindControl("lblPlace")).Visible = false;
                            ((TextBox)gvrData.FindControl("txtPlace")).Visible = true;
                            ((Label)gvrData.FindControl("lblCapital")).Visible = false;
                            ((TextBox)gvrData.FindControl("txtCapital")).Visible = true;
                            ((Label)gvrData.FindControl("lblCountry")).Visible = false;
                            ((TextBox)gvrData.FindControl("txtCountry")).Visible = true;

                            ((System.Web.UI.WebControls.Image)gvrData.FindControl("lblSign")).Visible = true;
                            intCheck = 1;
                        }

                        else
                        {
                            ((System.Web.UI.WebControls.Image)gvrData.FindControl("lblSign")).Visible = false;
                        }
                    }
                    else
                    {
                        ((CheckBox)gvrData.FindControl("chkSelect")).Checked = false;
                    }
                }
                if (intCheck == 1)
                {
                    btnEdit.Text = "Save";
                    gvMaster.Columns[0].Visible = true;
                }
                else
                {
                    gvMaster.Columns[0].Visible = false;
                }
            }
            else if (btnEdit.Text == "Save")
            {
                foreach (GridViewRow gvrData in gvMaster.Rows)
                {
                    if (((CheckBox)gvrData.FindControl("chkSelect")).Checked == true)
                    {
                        drData = ((DataSet)Session["DataMaintain"]).Tables[0].Rows[gvrData.DataItemIndex];
                        drData["name"] = ((TextBox)gvrData.FindControl("txtName")).Text;
                        drData["place"] = ((TextBox)gvrData.FindControl("txtPlace")).Text;
                        drData["capital"] = ((TextBox)gvrData.FindControl("txtCapital")).Text;
                        drData["country"] = ((TextBox)gvrData.FindControl("txtCountry")).Text;

                        ExecuteQuery("update TestGrid set name='" + drData["name"] + "', place ='" + drData["place"] + "',capital='" + drData["capital"] + "', country ='" + drData["country"] + "' where id=" + ((Label)gvrData.FindControl("lblHeadId")).Text + " ");
                    }
                }                
                ((ArrayList)ViewState["arl"]).Clear();
                PopulateGridSort((string)ViewState["Column"], 0);
                gvMaster.Columns[0].Visible = false;
                btnEdit.Text = "Edit";
                ShowMessage("Record Updated Successfully");
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PlusMinusDataMaintain"] = "";
            CurrentPageRW = 0;

            foreach (GridViewRow gvrData in gvMaster.Rows)
            {
                if (((CheckBox)gvrData.FindControl("chkSelect")).Checked == true)
                {
                    drData = ((DataSet)Session["DataMaintain"]).Tables[0].Rows[gvrData.DataItemIndex];
                    drData.Delete();

                    ExecuteQuery("delete from TestGrid  where id=" + ((Label)gvrData.FindControl("lblHeadId")).Text + " ");
                    intCheck = 1;
                }
                PopulateGridSort((string)ViewState["Column"], 0);
                if (intCheck == 1)
                { ShowMessage("Record Deleted Successfully"); }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            Default();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string lstrName;
        string lstrPlace;
        string lstrCapital;
        string lstrCountry;

        try
        {
            lstrName = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text;
            lstrPlace = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text;
            lstrCapital = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text;
            lstrCountry = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text;

            if (lstrName != "" && lstrPlace != "" && lstrCapital != "" && lstrCountry != "")
            {
                ExecuteQuery("insert into testgrid values('" + lstrName + "','" + lstrPlace + "','" + lstrCapital + "','" + lstrCountry + "')");
                blnCheck = true;
                PopulateGrid();
                Default();
                ShowMessage("Record Added Successfully");
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void HeadSearch(object sender, EventArgs e)
    {
        string strValue = "";
        string strData = null;
        string strVal1;
        string strVal2;
        string strVal3;
        string strVal4;
        int intFinal = 0;
        string strColumn = null;
        string strColumn1 = "";
        TextBox txtbx;
        DataView dvSet;

        try
        {
            txtbx = ((TextBox)sender);
            ViewState["Done"] = null;
            ViewState["txtSearchValue"] = txtbx.Text.Trim();
            ((ArrayList)ViewState["arl"]).Clear();
            CurrentPageRW = 0;
            ViewState["dtPaging"] = null;

            strVal1 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeadName")).Text.Trim();
            if (strVal1 != "")
            { intFinal += 1; strValue = strVal1; strColumn = "txtHeadName"; strColumn1 = "name"; }
            strVal2 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeadPlace")).Text.Trim();
            if (strVal2 != "")
            { intFinal += 1; strValue = strVal2; strColumn = "txtHeadPlace"; strColumn1 = "place"; }
            strVal3 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeadCapital")).Text.Trim();
            if (strVal3 != "")
            { intFinal += 1; strValue = strVal3; strColumn = "txtHeadCapital"; strColumn1 = "capital"; }
            strVal4 = ((TextBox)((GridViewRow)(((TextBox)sender).NamingContainer)).FindControl("txtHeadCountry")).Text.Trim();
            if (strVal4 != "")
            { intFinal += 1; strValue = strVal4; strColumn = "txtHeadCountry"; strColumn1 = "country"; }

            if (strColumn1 != "")
            {
                ViewState["Column"] = strColumn1;
            }
            if (intFinal > 1)
            {
                intCheck = 1;
                ViewState["HeadSearch"] = null;
            }
            else
            {
                intCheck = 0;
                ViewState["HeadSearch"] = true;
            }

            if (intCheck != 1)
            {
                if (strColumn == "txtHeadName")
                {
                    PopulateGridSort("Name", 0);
                }
                else if (strColumn == "txtHeadPlace")
                {
                    PopulateGridSort("Place", 0);
                }
                else if (strColumn == "txtHeadCapital")
                {
                    PopulateGridSort("Capital", 0);
                }
                else if (strColumn == "txtHeadCountry")
                {
                    PopulateGridSort("Country", 0);
                }

                if (ViewState["Done"] == null)
                {
                    if (Session["AfterSearch"] == null)
                    {
                        dvData = ((DataSet)Session["DataMaintain"]).DefaultViewManager.CreateDataView(((DataSet)Session["DataMaintain"]).Tables[0].Copy());
                    }
                    else
                    {
                        dvData = (DataView)Session["AfterSearch"];
                    }

                    if (strColumn == "txtHeadName")
                    {
                        if (ViewState["Condition"].ToString() == "1")
                        {
                            dvData.RowFilter = "name like '%" + strValue + "%'";
                        }
                        else if (ViewState["Condition"].ToString() == "2")
                        {
                            dvData.RowFilter = "name not like '%" + strValue + "%'";
                        }
                        else
                        {
                            dvData.RowFilter = "name like '" + strValue + "%'";
                        }
                    }
                    else if (strColumn == "txtHeadPlace")
                    {
                        if (ViewState["Condition"].ToString() == "1")
                        {
                            dvData.RowFilter = "name like '%" + strValue + "%' or place like '%" + strValue + "%'";
                        }
                        else if (ViewState["Condition"].ToString() == "2")
                        {
                            dvData.RowFilter = "name not like '%" + strValue + "%' or place not like '%" + strValue + "%'";
                        }
                        else
                        {
                            dvData.RowFilter = "name like '" + strValue + "%' or place like '" + strValue + "%'";
                        }

                        ienumData = dvData.GetEnumerator();
                        while (ienumData.MoveNext())
                        {
                            drvData = (DataRowView)ienumData.Current;

                            if (drvData["Name"] != null)
                            {
                                if (drvData["Name"].ToString().Contains("(") == false)
                                {
                                    if (strData == null)
                                    {
                                        drvData.Delete();
                                    }
                                    else if (strData.ToLower() != drvData["Place"].ToString().Trim().ToLower())
                                    {
                                        drvData.Delete();
                                    }
                                }
                                else
                                {
                                    strData = drvData["Name"].ToString().Split(new char[] { '(' }).GetValue(0).ToString().Trim();

                                }
                            }
                        }
                    }
                    else if (strColumn == "txtHeadCapital")
                    {
                        if (ViewState["Condition"].ToString() == "1")
                        {
                            dvData.RowFilter = "name like '%" + strValue + "%' or capital like '%" + strValue + "%'";
                        }
                        else if (ViewState["Condition"].ToString() == "2")
                        {
                            dvData.RowFilter = "name not like '%" + strValue + "%' or capital not like '%" + strValue + "%'";
                        }
                        else
                        {
                            dvData.RowFilter = "name like '" + strValue + "%' or capital like '" + strValue + "%'";
                        }

                        ienumData = dvData.GetEnumerator();
                        while (ienumData.MoveNext())
                        {
                            drvData = (DataRowView)ienumData.Current;

                            if (drvData["Name"] != null)
                            {
                                if (drvData["Name"].ToString().Contains("(") == false)
                                {
                                    if (strData == null)
                                    {
                                        drvData.Delete();
                                    }
                                    else if (strData.ToLower() != drvData["Capital"].ToString().Trim().ToLower())
                                    {
                                        drvData.Delete();
                                    }
                                }
                                else
                                {
                                    strData = drvData["Name"].ToString().Split(new char[] { '(' }).GetValue(0).ToString().Trim();
                                }
                            }
                        }
                    }
                    else if (strColumn == "txtHeadCountry")
                    {
                        if (ViewState["Condition"].ToString() == "1")
                        {
                            dvData.RowFilter = "name like '%" + strValue + "%' or country like '%" + strValue + "%'";
                        }
                        else if (ViewState["Condition"].ToString() == "2")
                        {
                            dvData.RowFilter = "name not like '%" + strValue + "%' or country not like '%" + strValue + "%'";
                        }
                        else
                        {
                            dvData.RowFilter = "name like '" + strValue + "%' or country like '" + strValue + "%'";
                        }

                        ienumData = dvData.GetEnumerator();
                        while (ienumData.MoveNext())
                        {
                            drvData = (DataRowView)ienumData.Current;

                            if (drvData["Name"] != null)
                            {
                                if (drvData["Name"].ToString().Contains("(") == false)
                                {
                                    if (strData == null)
                                    {
                                        drvData.Delete();
                                    }
                                    else if (strData.ToLower() != drvData["Country"].ToString().Trim().ToLower())
                                    {
                                        drvData.Delete();
                                    }

                                }
                                else
                                {
                                    strData = drvData["Name"].ToString().Split(new char[] { '(' }).GetValue(0).ToString().Trim();
                                }
                            }
                        }
                    }

                    if (dvData.Count > 0)
                    {
                        PopulatePager(dvData);
                        Session["AfterSearch"] = dvData;
                        Session["AfterSearchDATA"] = null;
                    }
                    gvMaster.Columns[0].Visible = false;
                    btnEdit.Text = "Edit";
                }
            }
            else
            {

                if (Session["AfterSearch"] == null)
                {
                    dvData = ((DataSet)Session["DataMaintain"]).DefaultViewManager.CreateDataView(((DataSet)Session["DataMaintain"]).Tables[0].Copy());
                }
                else
                {
                    dvData = (DataView)Session["AfterSearch"];
                }

                dtData = new DataTable();
                dtData = dvData.Table.Clone();
                ienumData = dvData.GetEnumerator();
                while (ienumData.MoveNext())
                {
                    drvData = (DataRowView)ienumData.Current;
                    drData = dtData.NewRow();
                    drData["id"] = drvData["id"];
                    drData["name"] = drvData["name"];
                    drData["place"] = drvData["place"];
                    drData["capital"] = drvData["capital"];
                    drData["country"] = drvData["country"];
                    drData["uniq"] = drvData["uniq"];
                    dtData.Rows.Add(drData);
                }
                dvSet = dtData.DefaultView;
                dvSet.RowFilter = "name like '" + strVal1 + "%'  and  place like '" + strVal2 + "%' and capital like '" + strVal3 + "%'  and  country like '" + strVal4 + "%'";
                if (dvSet.Count > 0)
                {
                    PopulatePager(dvSet);
                }
                gvMaster.Columns[0].Visible = false;
                btnEdit.Text = "Edit";
            }
            ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text = strVal1;
            ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text = strVal2;
            ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text = strVal3;
            ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text = strVal4;

            ViewState["PagerSearchValue"] = strVal1 + "," + strVal2 + "," + strVal3 + "," + strVal4;

            if (txtbx.ID == "txtHeadName")
            {
                ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Focus();
            }
            else if (txtbx.ID == "txtHeadPlace")
            {
                ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Focus();
            }
            else if (txtbx.ID == "txtHeadCapital")
            {
                ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Focus();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
        finally
        {
            ViewState["Condition"] = "0";
        }
    }

    protected void CheckSelect(object sender, EventArgs e)
    {
        bool blnCheck;

        try
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
                PopulateGrid();
                gvMaster.Columns[0].Visible = false;
                btnEdit.Text = "Edit";
            }
            else
            {
                blnCheck = ((CheckBox)sender).Checked;
                if (blnCheck == true)
                {
                    foreach (GridViewRow gvrData in gvMaster.Rows)
                    {
                        ((CheckBox)gvrData.FindControl("chkSelect")).Checked = true;
                    }
                }
                else
                {
                    foreach (GridViewRow gvrData in gvMaster.Rows)
                    {
                        ((CheckBox)gvrData.FindControl("chkSelect")).Checked = false;
                    }
                    gvMaster.Columns[0].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void gvMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strValue;
        string[] strarrData;

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((ArrayList)ViewState["arl"]).Contains(((Label)e.Row.FindControl("lblHeadId")).Text))
                {
                    e.Row.BackColor = Color.NavajoWhite ;
                    ((CheckBox)e.Row.FindControl("chkSelect")).Checked = true;
                }

                if (((Label)e.Row.FindControl("lblName")).Text.ToLower().Contains("(") == false)
                {
                    ((ImageButton)(e.Row.FindControl("imgbtnPlusMinus"))).Visible = false;

                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkSelect")).Visible = false;
                    ((ImageButton)(e.Row.FindControl("imgbtnPlusMinus"))).Visible = true;
                    ((Label)e.Row.FindControl("lblName")).Font.Bold = true;
                }

                if (ViewState["PlusMinusDataMaintain"].ToString().Trim() != "")
                {
                    strValue = ((Label)e.Row.FindControl("lblName")).Text.Trim().Split(new char[] { '(' }).GetValue(0).ToString().Trim();
                    strarrData = ViewState["PlusMinusDataMaintain"].ToString().Split(new char[] { ',' });
                    if (strarrData.Contains("'" + strValue + "'"))
                    {
                        ((ImageButton)(e.Row.FindControl("imgbtnPlusMinus"))).ImageUrl = "~/Images/plusbox.gif";
                    }
                    else
                    {
                        ((ImageButton)(e.Row.FindControl("imgbtnPlusMinus"))).ImageUrl = "~/Images/minusbox.gif";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void PlusMinus(object sender, EventArgs e)
    {
        string strValue;
        DataView dvSet;

        try
        {                     
            if (((ImageButton)(sender)).ImageUrl.ToLower().Contains("minus") == true)
            {
                ((ImageButton)(sender)).ImageUrl = "~/Images/plusbox.gif";

                strValue = ((Label)((GridViewRow)((ImageButton)(sender)).NamingContainer).FindControl("lblName")).Text.Split(new char[] { '(' }).GetValue(0).ToString().Trim();
                if (Session["AfterSearch"] == null)
                {
                    dvData = ((DataSet)Session["DataMaintain"]).DefaultViewManager.CreateDataView(((DataSet)Session["DataMaintain"]).Tables[0].Copy());
                }
                else
                {
                    dvData = (DataView)Session["AfterSearch"];
                }
                if (Session["AfterSearchDATA"] == null)
                {
                    Session["AfterSearchDATA"] = dvData;
                }

                if (ViewState["PlusMinusDataMaintain"].ToString().Trim() == "")
                {
                    ViewState["PlusMinusDataMaintain"] = "'" + strValue + "'";
                }
                else
                {
                    ViewState["PlusMinusDataMaintain"] += "," + "'" + strValue + "'";
                }
                dtData = new DataTable();
                dtData = dvData.Table.Clone();
                ienumData = dvData.GetEnumerator();
                while (ienumData.MoveNext())
                {
                    drvData = (DataRowView)ienumData.Current;
                    drData = dtData.NewRow();
                    drData["id"] = drvData["id"];
                    drData["name"] = drvData["name"];
                    drData["place"] = drvData["place"];
                    drData["capital"] = drvData["capital"];
                    drData["country"] = drvData["country"];
                    drData["uniq"] = drvData["uniq"];
                    dtData.Rows.Add(drData);
                 }

                dvSet = dtData.DefaultView;
                dvSet.RowFilter = (string)ViewState["Column"] + " not in (" + ViewState["PlusMinusDataMaintain"].ToString().Trim() + ")  or  id is null ";
                Session["AfterSearch"] = dvSet;
                PopulatePager(dvSet);
                if (ViewState["PagerSearchValue"] != null)
                {
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(0).ToString();
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(1).ToString();
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(2).ToString();
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(3).ToString();
                }
            }
            else
            {
                ((ImageButton)(sender)).ImageUrl = "~/Images/minusbox.gif";
                strValue = ((Label)((GridViewRow)((ImageButton)(sender)).NamingContainer).FindControl("lblName")).Text.Split(new char[] { '(' }).GetValue(0).ToString().Trim();
                ViewState["PlusMinusDataMaintain"] = ViewState["PlusMinusDataMaintain"].ToString().Replace("'" + strValue + "'", "");
                ViewState["PlusMinusDataMaintain"] = ViewState["PlusMinusDataMaintain"].ToString().Replace(",,", ",");
                ViewState["PlusMinusDataMaintain"] = ViewState["PlusMinusDataMaintain"].ToString().TrimStart(',');
                ViewState["PlusMinusDataMaintain"] = ViewState["PlusMinusDataMaintain"].ToString().TrimEnd(',');

                if (Session["AfterSearch"] == null)
                {
                    dvData = ((DataSet)Session["DataMaintain"]).DefaultViewManager.CreateDataView(((DataSet)Session["DataMaintain"]).Tables[0].Copy());
                }
                else
                {
                    dvData = (DataView)Session["AfterSearchDATA"];
                }
                dtData = new DataTable();
                dtData = dvData.Table.Clone();
                ienumData = dvData.GetEnumerator();
                while (ienumData.MoveNext())
                {
                    drvData = (DataRowView)ienumData.Current;
                    drData = dtData.NewRow();
                    drData["id"] = drvData["id"];
                    drData["name"] = drvData["name"];
                    drData["place"] = drvData["place"];
                    drData["capital"] = drvData["capital"];
                    drData["country"] = drvData["country"];
                    drData["uniq"] = drvData["uniq"];
                    dtData.Rows.Add(drData);
                }
                dvSet = dtData.DefaultView;
                Session["AfterSearch"] = dvSet;
                if (ViewState["PlusMinusDataMaintain"].ToString().Trim() != "")
                {
                    dvSet.RowFilter = (string)ViewState["Column"] + " not in (" + ViewState["PlusMinusDataMaintain"].ToString().Trim() + ")  or  id is null ";
                }
                PopulatePager(dvSet);

                if (ViewState["PagerSearchValue"] != null)
                {
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(0).ToString();
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(1).ToString();
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(2).ToString();
                    ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(3).ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }  

    protected void imgbtnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            ((ArrayList)ViewState["arl"]).Clear();
            CurrentPageRW = 0;
            ViewState["dtPaging"] = null;
            PopulateGrid();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void imgbtnLast_Click(object sender, EventArgs e)
    {
        try
        {
            ((ArrayList)ViewState["arl"]).Clear();
            CurrentPageRW = (int)ViewState["TotalPageCount"];
            ViewState["dtPaging"] = null;
            ViewState["imgbtnLast_Click"] = true;
            PopulateGrid();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void imgbtnPrev_Click(object sender, EventArgs e)
    {
        try
        {
            ((ArrayList)ViewState["arl"]).Clear();
            CurrentPageRW -= 1;
            PageCountRW = Convert.ToInt32(ViewState["dlstPager_ItemDataBound"]);
            ViewState["imgbtnPrev_Click"] = true;
            PopulateGrid();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void imgbtnNext_Click(object sender, EventArgs e)
    {     
        try
        {
            ((ArrayList)ViewState["arl"]).Clear();
            CurrentPageRW += 1;
            PageCountRW = Convert.ToInt32(ViewState["dlstPager_ItemDataBound"]);
            ViewState["imgbtnNext_Click"] = true;
            PopulateGrid();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void dlstPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("lnkbtnPaging"))
            {
                PageCountRW = Convert.ToInt32(((Label)e.Item.FindControl("lblCount")).Text.Trim());
                CurrentPageRW = Convert.ToInt32(e.CommandArgument.ToString());
                PopulateGrid();
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void dlstPager_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        System.Web.UI.WebControls.LinkButton lnkbtnPage;

        try
        {
            lnkbtnPage = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkbtnPaging");
            if (lnkbtnPage.Text == Convert.ToString(CurrentPageRW + 1))
            {
                lnkbtnPage.Enabled = false;
                ViewState["dlstPager_ItemDataBound"] = Convert.ToInt32(((Label)e.Item.FindControl("lblCount")).Text.Trim());
                ((Label)gvMaster.BottomPagerRow.FindControl("lblPagerSearch")).Text =" of  " + ((int)ViewState["TotalPageCount"] + 1).ToString().Trim();
                ((TextBox)gvMaster.BottomPagerRow.FindControl("txtPagerSearch")).Text = lnkbtnPage.Text;
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void DoGroup(object sender, EventArgs e)
    {
        string strColumn = null;
        LinkButton lnkbtn;

        try
        {
            CurrentPageRW = 0;
            ViewState["PlusMinusDataMaintain"] = "";
            ((ArrayList)ViewState["arl"]).Clear();
            ViewState["dtPaging"] = null;
           
            lnkbtn = (LinkButton)sender;

            if (lnkbtn.ID == "lnkbtnGroupName")
            {
                strColumn = "Name";
            }
            else if (lnkbtn.ID == "lnkbtnGroupPlace")
            {
                strColumn = "Place";
            }
            else if (lnkbtn.ID == "lnkbtnGroupCapital")
            {
                strColumn = "Capital";
            }
            else if (lnkbtn.ID == "lnkbtnGroupCountry")
            {
                strColumn = "Country";
            }
            ViewState["Column"] = strColumn;
            Session["AfterSearch"] = null;
            Session["AfterSearchDATA"] = null;
            PopulateGridSort(strColumn, 0);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void DoSort(object sender, EventArgs e)
    {
        string strColumn = null;
        string strName;
        string strPlace;
        string strCapital;
        string strCountry;
        ImageButton imgbtn;

        try
        {
            GetSortStatusRW = true;
            imgbtn = (ImageButton)sender;

            strName = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text;
            strPlace = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text;
            strCapital = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text;
            strCountry = ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text;
            ViewState["PagerSearchValue"] = strName + "," + strPlace + "," + strCapital + "," + strCountry;

            if (imgbtn.ID == "imgbtnSortName")
            {
                strColumn = "Name";
            }
            else if (imgbtn.ID == "imgbtnSortPlace")
            {
                strColumn = "Place";
            }
            else if (imgbtn.ID == "imgbtnSortCapital")
            {
                strColumn = "Capital";
            }
            else if (imgbtn.ID == "imgbtnSortCountry")
            {
                strColumn = "Country";
            }

            if (GetSortDirection(strColumn) == "ASC")
            {
                PopulateGridSort(strColumn, 0);
                ((ImageButton)gvMaster.HeaderRow.FindControl(imgbtn.ID)).ImageUrl = "~/Images/ArrowUp.gif";
            }
            else
            {
                PopulateGridSort(strColumn, 1);
                ((ImageButton)gvMaster.HeaderRow.FindControl(imgbtn.ID)).ImageUrl = "~/Images/ArrowDown.gif";
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    private void PopulateGridSort(string column, int sort)
    {
        int intCheck = 0;
        string strCln = "";
        string strData = null;
        string strSort;
        string strValue;
        DataSet dsData;
        DataView dvData;

        try
        {
            if (((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text.Trim() != "")
            { intCheck += 1; strCln = "txtHeadName"; }
            if (((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text.Trim() != "")
            { intCheck += 1; strCln = "txtHeadPlace"; }
            if (((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text.Trim() != "")
            { intCheck += 1; strCln = "txtHeadCapital"; }
            if (((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text.Trim() != "")
            { intCheck += 1; strCln = "txtHeadCountry"; }

            if (Session["AfterSearch"] != null)
            {
                if (intCheck == 1 && GetSortStatusRW == false)
                {
                    dsData = GetDataset("GetData  '" + strCln.Substring(7, strCln.Length - 7).ToString() + "'," + sort + "");
                    dvData = dsData.DefaultViewManager.CreateDataView(dsData.Tables[0].Copy());
                    strValue = ((TextBox)gvMaster.HeaderRow.FindControl(strCln)).Text.Trim();
                    dvData.RowFilter = "name like '" + strValue + "%' or  " + strCln.Substring(7, strCln.Length - 7).ToString() + "  like '" + strValue + "%'";

                    if (strCln != "txtHeadName")
                    {
                        ienumData = dvData.GetEnumerator();
                        while (ienumData.MoveNext())
                        {
                            drvData = (DataRowView)ienumData.Current;

                            if (drvData["Name"] != null)
                            {
                                if (drvData["Name"].ToString().Contains("(") == false)
                                {
                                    if (strData == null)
                                    {
                                        drvData.Delete();
                                    }
                                    else if (strData.ToLower() != drvData[strCln.Substring(7, strCln.Length - 7).ToString()].ToString().Trim().ToLower())
                                    {
                                        drvData.Delete();
                                    }
                                }
                                else
                                {
                                    strData = drvData["Name"].ToString().Split(new char[] { '(' }).GetValue(0).ToString().Trim();
                                }
                            }
                        }
                    }
                    if (dvData.Count > 0)
                    {
                        Session["AfterSearch"] = dvData;
                        PopulatePager(dvData);
                        ViewState["Done"] = true;
                    }
                }

                if (ViewState["HeadSearch"] == null)
                {
                    strSort = "Asc";
                    if (sort == 1)
                    {
                        strSort = "Desc";
                    }
                    dvData = (DataView)Session["AfterSearch"];
                    dvData.Sort = "Uniq" + " " + strSort;
                    Session["AfterSearch"] = dvData;
                    PopulatePager(dvData);

                    if (ViewState["PagerSearchValue"] != null)
                    {
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(0).ToString();
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(1).ToString();
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(2).ToString();
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(3).ToString();
                    }
                }
            }
            else
            {
                Session["DataMaintain"] = GetDataset("GetData  '" + column + "'," + sort + "");
                if (ViewState["HeadSearch"] == null)
                {
                    PopulatePager(((DataSet)Session["DataMaintain"]).DefaultViewManager.CreateDataView(((DataSet)Session["DataMaintain"]).Tables[0].Copy()));
                }
            }
            ViewState["HeadSearch"] = null;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    private void PopulatePager(object oData)
    {
        try
        {
            pdsPagerDataSource.DataSource = (IEnumerable)oData;
            pdsPagerDataSource.AllowPaging = true;
            pdsPagerDataSource.PageSize = 10;
            pdsPagerDataSource.CurrentPageIndex = CurrentPageRW;
            gvMaster.DataSource = pdsPagerDataSource;
            gvMaster.DataBind();
            gvMaster.BottomPagerRow.Visible = true;
            ((System.Web.UI.WebControls.ImageButton)gvMaster.BottomPagerRow.FindControl("imgbtnFirst")).Enabled = !pdsPagerDataSource.IsFirstPage;
            ((System.Web.UI.WebControls.ImageButton)gvMaster.BottomPagerRow.FindControl("imgbtnLast")).Enabled = !pdsPagerDataSource.IsLastPage;
            ((System.Web.UI.WebControls.ImageButton)gvMaster.BottomPagerRow.FindControl("imgbtnPrev")).Enabled = !pdsPagerDataSource.IsFirstPage;
            ((System.Web.UI.WebControls.ImageButton)gvMaster.BottomPagerRow.FindControl("imgbtnNext")).Enabled = !pdsPagerDataSource.IsLastPage;
            if (pdsPagerDataSource.PageCount == 0)
            { ViewState["TotalPageCount"] = 0; }
            else { ViewState["TotalPageCount"] = pdsPagerDataSource.PageCount - 1; }
            doPaging();
            gvMaster.Columns[0].Visible = false;
            btnEdit.Text = "Edit";

            ((TextBox)gvMaster.BottomPagerRow.FindControl("txtPagerSearch")).Width = 20;
            ((TextBox)gvMaster.BottomPagerRow.FindControl("txtPagerSearch")).Height = 13;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    private void PopulateGrid()
    {
        try
        {
            if (blnCheck == true || Page.IsPostBack == false)
            {
                Session["DataMaintain"] = GetDataset("GetData  'name'");
                PopulatePager(((DataSet)Session["DataMaintain"]).DefaultViewManager.CreateDataView(((DataSet)Session["DataMaintain"]).Tables[0].Copy()));
            }
            else
            {
                if (Session["AfterSearch"] != null)
                {
                    PopulatePager((DataView)Session["AfterSearch"]);
                    if (ViewState["PagerSearchValue"] != null)
                    {
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(0).ToString();
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(1).ToString();
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(2).ToString();
                        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text = ((string)ViewState["PagerSearchValue"]).Split(new char[] { ',' }).GetValue(3).ToString();
                    }
                }
                else
                {
                    PopulatePager(((DataSet)Session["DataMaintain"]).DefaultViewManager.CreateDataView(((DataSet)Session["DataMaintain"]).Tables[0].Copy()));
                }
            }
            //((TextBox)gvMaster.BottomPagerRow.FindControl("txtPagerSearch")).Width = 20;
            //((TextBox)gvMaster.BottomPagerRow.FindControl("txtPagerSearch")).Height = 13;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void PagerSearch(object sender, EventArgs e)
    {
        string strValue = "";
        try
        {
            strValue = ((TextBox)sender).Text.Trim();
            ((Label)gvMaster.BottomPagerRow.FindControl("lblPagerSearch")).Text = "";
            if (strValue != "")
            {
                if ((Convert.ToInt32(strValue) > 5 && (Convert.ToInt32(strValue) < (((int)ViewState["TotalPageCount"]) + 1))))
                {
                    CurrentPageRW = Convert.ToInt32(strValue) - 1;
                    PageCountRW = 5;
                    PopulateGrid();
                }
                else if ((Convert.ToInt32(strValue) == (((int)ViewState["TotalPageCount"]) + 1)))
                {
                    ViewState["imgbtnNext_Click"] = true;
                    CurrentPageRW = ((int)ViewState["TotalPageCount"]);
                    PageCountRW = 5;
                    PopulateGrid();
                }
                else if ((Convert.ToInt32(strValue) > ((int)ViewState["TotalPageCount"]) + 1))
                {
                    ViewState["imgbtnNext_Click"] = true;
                    CurrentPageRW = ((int)ViewState["TotalPageCount"]);
                    PageCountRW = 5;
                    PopulateGrid();
                    ShowMessage("No Data Found");
                    strValue = (((int)ViewState["TotalPageCount"]) + 1).ToString();
                }
                else if ((Convert.ToInt32(strValue) <= 5) && Convert.ToInt32(strValue) != 0)
                {
                    ViewState["dtPaging"] = null;
                    CurrentPageRW = Convert.ToInt32(strValue) - 1;
                    PopulateGrid();
                }
                ((TextBox )gvMaster.BottomPagerRow.FindControl("txtPagerSearch")).Text = strValue ;
                
                if (Convert.ToInt32(strValue) != 0)
                {
                    ((Label)gvMaster.BottomPagerRow.FindControl("lblPagerSearch")).Text = " of  " + ((int)ViewState["TotalPageCount"] + 1).ToString().Trim();
                }
                else
                {
                    ShowMessage("No Data Found");
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    protected void OpenCloseSearch(object sender, EventArgs e)
    {
        if (((TextBox)gvMaster.HeaderRow.FindControl("txtHead" + ((System.Web.UI.WebControls.ImageButton)sender).ID.Replace("imgbtnSearch", "").Trim() + "")).Text.Trim() != "")
        {
            if (((System.Web.UI.HtmlControls.HtmlGenericControl)gvMaster.HeaderRow.FindControl("div" + ((System.Web.UI.WebControls.ImageButton)sender).ID.Replace("imgbtnSearch", "").Trim() + "Search")).Visible == false)
            {
                ((System.Web.UI.HtmlControls.HtmlGenericControl)gvMaster.HeaderRow.FindControl("div" + ((System.Web.UI.WebControls.ImageButton)sender).ID.Replace("imgbtnSearch", "").Trim() + "Search")).Visible = true;
            }
            else
            {
                ((System.Web.UI.HtmlControls.HtmlGenericControl)gvMaster.HeaderRow.FindControl("div" + ((System.Web.UI.WebControls.ImageButton)sender).ID.Replace("imgbtnSearch", "").Trim() + "Search")).Visible = false;
            }
        }
        else
        {
            ShowMessage("Please Enter " + ((System.Web.UI.WebControls.ImageButton)sender).ID.Replace("imgbtnSearch", "").Trim() + " Search");
        }
    }

    protected void ConditionSearch(object sender, EventArgs e)
    {
        ViewState["Condition"] = ((ListBox)sender).SelectedValue;
        Session["AfterSearch"] = null;
        Session["AfterSearchDATA"] = null;
        ViewState["dtPaging"] = null;
        ((System.Web.UI.HtmlControls.HtmlGenericControl)gvMaster.HeaderRow.FindControl("div" + ((System.Web.UI.WebControls.ListBox)sender).ID.Replace("lstbxSearch", "").Trim() + "Search")).Visible = false;
        HeadSearch((TextBox)gvMaster.HeaderRow.FindControl("txtHead" + ((System.Web.UI.WebControls.ListBox)sender).ID.Replace("lstbxSearch", "").Trim() + ""), e);
    }

    private void ExecuteQuery(string strQuery)
    {
        SqlCommand sqlcom;
        try
        {
            sqlcom = new SqlCommand(strQuery, sqlcon);
            sqlcon.Open();
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    private void doPaging()
    {
        DataTable dtPaging;
        DataRow dr;
        DataView dvData;
        int intPager = 0;
        IEnumerator ie;
        DataRowView drv;


        try
        {
            dtPaging = new DataTable();
            dtPaging.Columns.Add("PageIndex");
            dtPaging.Columns.Add("PageText");
            dtPaging.Columns.Add("Count");

            if (ViewState["dtPaging"] == null)
            {
                for (int i = 0; i < pdsPagerDataSource.PageCount; i++)
                {
                    dr = dtPaging.NewRow();
                    dr[0] = i;
                    dr[1] = i + 1;
                    dtPaging.Rows.Add(dr);
                }
                ViewState["dtPaging"] = dtPaging;
                dvData = dtPaging.Copy().DefaultView;
                if (ViewState["imgbtnLast_Click"] == null)
                {
                    dvData.RowFilter = "PageIndex >=0 and PageIndex <5";
                }
                else
                {
                    dvData.RowFilter = "PageIndex >=" + (pdsPagerDataSource.PageCount - 5) + " and PageIndex <=" + (pdsPagerDataSource.PageCount - 1) + "";
                }

                ie = dvData.GetEnumerator();

                while (ie.MoveNext())
                {
                    intPager += 1;
                    drv = ((DataRowView)ie.Current);
                    drv["Count"] = intPager;

                }

                Session["dtPagingView"] = dvData;

                ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = dvData;
                ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();

            }
            else
            {
                dvData = ((DataTable)ViewState["dtPaging"]).Copy().DefaultView;
                if (PageCountRW == 1)
                {
                    if (CurrentPageRW != 0)
                    {
                        dvData.RowFilter = "PageIndex >= " + (CurrentPageRW - 1) + " and PageIndex <= " + (CurrentPageRW + 3) + "";

                        ie = dvData.GetEnumerator();

                        while (ie.MoveNext())
                        {
                            intPager += 1;
                            drv = ((DataRowView)ie.Current);
                            drv["Count"] = intPager;

                        }
                        Session["dtPagingView"] = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();
                    }
                    else if (ViewState["imgbtnPrev_Click"] != null)
                    {
                        dvData.RowFilter = "PageIndex >=0 and PageIndex <5";

                        ie = dvData.GetEnumerator();

                        while (ie.MoveNext())
                        {
                            intPager += 1;
                            drv = ((DataRowView)ie.Current);
                            drv["Count"] = intPager;

                        }
                        Session["dtPagingView"] = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();

                    }
                    else
                    {
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = ((DataView)Session["dtPagingView"]);
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();

                    }
                }
                else if (PageCountRW == 5)
                {
                    if (CurrentPageRW != pdsPagerDataSource.PageCount - 1)
                    {
                        dvData.RowFilter = "PageIndex >= " + (CurrentPageRW - 3) + " and PageIndex <= " + (CurrentPageRW + 1) + "";

                        ie = dvData.GetEnumerator();

                        while (ie.MoveNext())
                        {
                            intPager += 1;
                            drv = ((DataRowView)ie.Current);
                            drv["Count"] = intPager;
                        }
                        Session["dtPagingView"] = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();
                    }
                    else if (ViewState["imgbtnNext_Click"] != null)
                    {
                        dvData.RowFilter = "PageIndex >=" + (pdsPagerDataSource.PageCount - 5) + " and PageIndex <=" + (pdsPagerDataSource.PageCount - 1) + "";

                        ie = dvData.GetEnumerator();

                        while (ie.MoveNext())
                        {
                            intPager += 1;
                            drv = ((DataRowView)ie.Current);
                            drv["Count"] = intPager;

                        }
                        Session["dtPagingView"] = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = dvData;
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();

                    }
                    else
                    {
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = ((DataView)Session["dtPagingView"]);
                        ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();

                    }
                }
                else
                {
                    ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataSource = ((DataView)Session["dtPagingView"]);
                    ((System.Web.UI.WebControls.DataList)gvMaster.BottomPagerRow.FindControl("dlstPager")).DataBind();

                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
    }

    private void ShowMessage(string pstrMessage)
    {
        lblMessage.Text = pstrMessage;
    }

    private void Default()
    {
        ViewState["pageval"] = 0;
        ViewState["PlusMinusDataMaintain"] = "";
        ViewState["TotalPageCount"] = 0;
        ViewState["arl"] = new ArrayList();
        Session["AfterSearch"] = null;
        Session["AfterSearchDATA"] = null;
        ViewState["txtSearchValue"] = "";
        ViewState["Column"] = "name";
        ViewState["PagerSearchValue"] = null;
        ViewState["dtPaging"] = null;
        ViewState["Condition"] = "0";
        blnCheck = true;
        CurrentPageRW = 0;
        PopulateGrid();
        gvMaster.Columns[0].Visible = false;
        btnEdit.Text = "Edit";
        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadName")).Text = "";
        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadPlace")).Text = "";
        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCapital")).Text = "";
        ((TextBox)gvMaster.HeaderRow.FindControl("txtHeadCountry")).Text = "";

        
    }

    private DataSet GetDataset(string strQuery)
    {
        DataSet dsData = null;
        SqlCommand sqlcom;
        SqlDataAdapter sqlda;
        try
        {
            dsData = new DataSet();
            sqlcom = new SqlCommand(strQuery, sqlcon);
            sqlda = new SqlDataAdapter(sqlcom);
            sqlda.Fill(dsData);
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
        return dsData;
    }

    private string GetSortDirection(string column)
    {
        // By default, set the sort direction to ascending.
        string strSortDirection = "DESC";
        string strSortExpression;

        try
        {
            // Retrieve the last column that was sorted.
            strSortExpression = ViewState["strSortExpression"] as string;
            if (strSortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (strSortExpression == column)
                {
                    string lastDirection = ViewState["strSortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "DESC"))
                    {
                        strSortDirection = "ASC";
                    }
                }
            }
            // Save new values in ViewState.
            ViewState["strSortDirection"] = strSortDirection;
            ViewState["strSortExpression"] = column;
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
        return strSortDirection;
    }

    private bool GetSortStatusRW
    {
        get { return blnSortStatus; }
        set { blnSortStatus = value; }
    }

    public int CurrentPageRW
    {
        get
        {
            if (this.ViewState["CurrentPageRW"] == null)
                return 0;
            else
                return Convert.ToInt32(this.ViewState["CurrentPageRW"].ToString());
        }
        set
        {
            this.ViewState["CurrentPageRW"] = value;
        }
    }

    private Int32 PageCountRW
    {

        get { return intCount; }
        set { intCount = value; }
    }

}

