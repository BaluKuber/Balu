using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AjaxControlToolkit;

/// <summary>
/// Summary description for GridViewTemplate
/// </summary>
public class GridViewTemplate : ITemplate
{

    //A variable to hold the type of ListItemType.
    ListItemType _templateType;
    //A variable to hold the column name.
    string _columnName;
    string _strCol;
    string _gvDisplay;

    //Constructor where we define the template type and column name.
    public GridViewTemplate(ListItemType type, string colname, string strCol, string gvDisplay)
    {
        //Stores the template type.
        _templateType = type;      
        _columnName = colname;
        _strCol = strCol;
        _gvDisplay = gvDisplay;
    }

    void ITemplate.InstantiateIn(System.Web.UI.Control container)
    {
        switch (_templateType)
        {
            case ListItemType.Header:
                LinkButton lnkHeaderSearch = new LinkButton();
               
                lnkHeaderSearch.ID = "lnkHeader" + _strCol;//_columnName.Replace(" ", "");
                lnkHeaderSearch.CssClass = "";
                lnkHeaderSearch.Text = _columnName;
                lnkHeaderSearch.ToolTip = "Sort By " + _columnName;
                container.Controls.Add(lnkHeaderSearch);

                ImageButton imgbtnSearch = new ImageButton();
                imgbtnSearch.ID = "imgbtnSearch" + _strCol;
                imgbtnSearch.CssClass = "styleImageSortingAsc";
                imgbtnSearch.Enabled = false;
                container.Controls.Add(imgbtnSearch);


                TextBox txtHeaderSearch = new TextBox();
                //FilteredTextBoxExtender ftxtHeaderSearch = new FilteredTextBoxExtender();
                txtHeaderSearch.AutoPostBack = true;

                txtHeaderSearch.ID = "txtHeaderSearch" + _strCol;// _columnName.Replace(" ", "");
                txtHeaderSearch.Width = 100;
                txtHeaderSearch.MaxLength = 50;
                //ftxtheadersearch.targetcontrolid = txtheadersearch.id;
                //ftxtheadersearch.validchars = "- /";
                //ftxtheadersearch.filtermode = filtermodes.validchars;
                //ftxtHeaderSearch.FilterType = FilterTypes.LowercaseLetters;
                //ftxtHeaderSearch.FilterType = FilterTypes.Custom;
                //ftxtHeaderSearch.FilterType = FilterTypes.Numbers;
                //ftxtHeaderSearch.FilterType = FilterTypes.UppercaseLetters;
                //ftxtHeaderSearch.Enabled = true;
                container.Controls.Add(txtHeaderSearch);              

                if (_columnName != "ID")
                {
                    txtHeaderSearch.Visible = true;
                }
                else
                {
                    container.Visible = false;
                    txtHeaderSearch.Visible = false;
                }
                //container.Controls.Add();
                break;
            case ListItemType.Item:
                LinkButton lnkName = new LinkButton();
                //Added by Thangam M for Dynamic LOV
                lnkName.ID = "lnk" + _strCol;
                lnkName.DataBinding += new EventHandler(DateItem_DataBinding);
                lnkName.ForeColor = System.Drawing.Color.Black;
                lnkName.Font.Underline = false;
                container.Controls.Add(lnkName);
                break;
            case ListItemType.EditItem:
                //As, I am not using any EditItem, I didnot added any code here.
                break;
            case ListItemType.Footer:
                //no use
                break;
        }
    }

    //private void txtHeaderSearch_TextChanged(object sender, EventArgs e)
    //{
    //    string s = "";
    //}

    /// <summary>
    /// Binding a grid view Link Button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void lnkName_DataBinding(object sender, EventArgs e)
    {
        LinkButton lnkName = (LinkButton)sender;       
        GridViewRow gvr = ((GridViewRow)(((DataControlFieldHeaderCell)(((LinkButton)sender).Parent)).NamingContainer));
        DataControlFieldHeaderCell container = ((DataControlFieldHeaderCell)(((LinkButton)sender).Parent));
        GridViewTemplate gvt = ((GridViewTemplate)(((TemplateField)(((DataControlFieldCell)(container)).ContainingField)).HeaderTemplate));
     
    }

    void DateItem_DataBinding(object sender, EventArgs e)
    {
        LinkButton lnkName = (LinkButton)sender;
        GridViewRow container = (GridViewRow)lnkName.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, _columnName);
        object dataValueID = DataBinder.Eval(container.DataItem,_gvDisplay);// "ID");
        //if(dataValueID.ToString()=="") dataValueID = DataBinder.Eval(container.DataItem,"ID");
        if (dataValue != DBNull.Value && dataValueID != DBNull.Value)
        {
            lnkName.Text = dataValue.ToString();
            lnkName.ToolTip = dataValue.ToString();
            lnkName.CommandArgument = dataValueID.ToString();
        }
        container.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
        if (_columnName == "ID")
            ((System.Web.UI.WebControls.TableRow)(container)).Cells[0].Visible = false;
        //container.Visible = false;
    }

    /// <summary>
    /// This is the event, which will be raised when the binding happens.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param> 

}

public class GridViewHeaderRow : ITemplate
{
    ListItemType _templateType;

    public GridViewHeaderRow(ListItemType type)
    {
        _templateType = type;
    }
    void ITemplate.InstantiateIn(System.Web.UI.Control container)
    {
        switch (_templateType)
        {
            case ListItemType.Header:
                //LinkButton lnkHeaderSearch = new LinkButton();
                //lnkHeaderSearch.ID = "lnkHeader" + _columnName;
                //lnkHeaderSearch.DataBinding += new EventHandler(lnkName_DataBinding);
                //container.Controls.Add(lnkHeaderSearch);
                break;
        }
    }

}

