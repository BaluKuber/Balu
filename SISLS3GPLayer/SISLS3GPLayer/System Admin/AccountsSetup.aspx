<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="AccountsSetup.aspx.cs" Inherits="System_Admin_AccountsSetup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<table width="100%">

<tr>
<td valign="bottom" align="Right" > 
    <asp:ImageButton ID="imgbtnLeft" runat="server" CausesValidation="False" 
        ImageUrl="~/Images/layout_button_left.gif" onclick="imgbtnLeft_Click" Visible=false/>
    <asp:ImageButton ID="imgbtnRight" runat="server" CausesValidation="False" 
        ImageUrl="~/Images/layout_button_right.gif" onclick="imgbtnRight_Click" Visible=false/>
</td>
</tr>
<tr>

<td valign="top" >
<cc1:TabContainer ID="TabContainer1" runat="server"   ActiveTabIndex="0" 
         EnableTheming="true" ScrollBars="Vertical" Height="350px" width="100%" CssClass="styleTabPanel"><!--CssClass="styleTabPanel"-->
         
        <cc1:TabPanel runat="server"  HeaderText="General Details" ID="TabPanel1" CssClass="tabpan" 
        BackColor="Red">
        <HeaderTemplate>General Details</HeaderTemplate>
        <ContentTemplate>
            <table width="90%">
                <tr>
                    <td>
                        <table width="100%" >
                            <tr>
                                <td><asp:Label ID="lblLOB" runat="server" Text="Line of Business"></asp:Label></td>
                                <td><asp:TextBox ID="tbLOB" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="lblbranch" runat="server" Text="Branch"></asp:Label></td>
                                <td><asp:TextBox ID="tbBranch" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblAccDesc" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="ddlAccountDesc" runat="server">
                                        <asp:ListItem>Account Description1</asp:ListItem>
                                        <asp:ListItem>Account Description2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="lblAccFlag" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="tbAccflag" runat="server"></asp:TextBox></td>
                           </tr>
                            <tr>
                                <td><asp:Label ID="Label1" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList1" runat="server">
                                        <asp:ListItem>Acc Desc1</asp:ListItem>
                                        <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label2" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label3" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox6" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label4" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label5" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList3" runat="server">
                                         <asp:ListItem>Acc Desc1</asp:ListItem>
                                            <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label6" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label7" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label8" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
                            </tr>
                      </table>
                    </td>
                </tr>
                <tr>
                  <td>
                         <table width="100%" >
                            <tr>
                                <td><asp:Label ID="Label9" runat="server" Text="Line of Business"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox7" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label10" runat="server" Text="Branch"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox8" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label11" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList2" runat="server">
                                        <asp:ListItem>Acc Desc1</asp:ListItem>
                                        <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label12" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox9" runat="server"></asp:TextBox></td>
                           </tr>
                            <tr>
                                <td><asp:Label ID="Label13" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList4" runat="server">
                                        <asp:ListItem>Acc Desc1</asp:ListItem>
                                        <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label14" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox10" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label15" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox11" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label16" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox12" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label17" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList5" runat="server">
                                         <asp:ListItem>Acc Desc1</asp:ListItem>
                                            <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label18" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox13" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label19" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox14" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label20" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox15" runat="server"></asp:TextBox></td>
                            </tr>
                      </table>
                  </td>
                </tr>
                <tr>
                    <td>
                          <asp:GridView ID="GridView2" runat="server"  
                                                        AllowPaging="True"  
                            AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                                                 onrowdatabound="GridView1_RowDataBound" PageSize=7><Columns><asp:TemplateField HeaderText="ID" Visible="False" ><ItemTemplate><asp:Label runat="server" ID="lblHeadId" Text='<%# Eval("id") %>'  ></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField><ItemTemplate><asp:Label runat="server" ID="lblName" Text='<%# Eval("name") %>'></asp:Label><asp:TextBox ID="txtName" runat="server" Text='<%# Eval("name") %>' 
                                                                Visible="false"></asp:TextBox></ItemTemplate><HeaderTemplate><asp:LinkButton ID="lnkbtnSortName" runat="server" OnClick="DoSort" text="Name" CssClass="styleGridHeader"></asp:LinkButton><br /><br /><asp:TextBox ID="txtHeadName" runat="server" AutoPostBack="true" 
                                                                OnTextChanged="HeadSearch"></asp:TextBox></HeaderTemplate></asp:TemplateField><asp:TemplateField ><HeaderTemplate><asp:LinkButton runat="server" ID="lnkbtnSortPlace"  text="Place" OnClick="DoSort" CssClass="styleGridHeader"></asp:LinkButton><br /><br /><asp:TextBox runat="server" ID="txtHeadPlace" AutoPostBack="true" 
                                                                            OnTextChanged="HeadSearch"></asp:TextBox></HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblPlace" Text='<%# Eval("place") %>'></asp:Label><asp:TextBox runat="server" ID="txtPlace" Text='<%# Eval("place") %>' 
                                                                Visible="false" ></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField ><HeaderTemplate><asp:LinkButton runat="server" ID="lnkbtnSortCapital"  text="Capital" CssClass="styleGridHeader"
                                                                            OnClick="DoSort"></asp:LinkButton><br /><br /><asp:TextBox runat="server" ID="txtHeadCapital" AutoPostBack="true" 
                                                                            OnTextChanged="HeadSearch"></asp:TextBox></HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblCapital" Text='<%# Eval("capital") %>'></asp:Label><asp:TextBox runat="server" ID="txtCapital" Text='<%# Eval("capital") %>' 
                                                                Visible="false" ></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField ><HeaderTemplate><asp:LinkButton runat="server" ID="lnkbtnSortCountry"  text="Country" CssClass="styleGridHeader"
                                                                            OnClick="DoSort"></asp:LinkButton><br /><br /><asp:TextBox runat="server" ID="txtHeadCountry" AutoPostBack="true" 
                                                                            OnTextChanged="HeadSearch" EnableViewState="true"></asp:TextBox></HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblCountry" Text='<%# Eval("country") %>'></asp:Label><asp:TextBox runat="server" ID="txtCountry" Text='<%# Eval("country") %>' 
                                                                Visible="false" ></asp:TextBox></ItemTemplate></asp:TemplateField></Columns></asp:GridView>
                    </td>
              </tr>
      </table>
      </ContentTemplate>
      </cc1:TabPanel>
        
        <cc1:TabPanel runat="server"  HeaderText="Personal Details" ID="TabPanel2"><ContentTemplate><br /><asp:GridView ID="GridView1" runat="server"  
                                    AllowPaging="True"  
        AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                             onrowdatabound="GridView1_RowDataBound" PageSize=7><Columns><asp:TemplateField HeaderText="ID" Visible="False" ><ItemTemplate><asp:Label runat="server" ID="lblHeadId" Text='<%# Eval("id") %>'  ></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField><ItemTemplate><asp:Label runat="server" ID="lblName" Text='<%# Eval("name") %>'></asp:Label><asp:TextBox ID="txtName" runat="server" Text='<%# Eval("name") %>' 
                                            Visible="false"></asp:TextBox></ItemTemplate><HeaderTemplate><asp:LinkButton ID="lnkbtnSortName" runat="server" OnClick="DoSort" text="Name" CssClass="styleGridHeader"></asp:LinkButton><br /><br /><asp:TextBox ID="txtHeadName" runat="server" AutoPostBack="true" 
                                            OnTextChanged="HeadSearch"></asp:TextBox></HeaderTemplate></asp:TemplateField><asp:TemplateField ><HeaderTemplate><asp:LinkButton runat="server" ID="lnkbtnSortPlace"  text="Place" OnClick="DoSort" CssClass="styleGridHeader"></asp:LinkButton><br /><br /><asp:TextBox runat="server" ID="txtHeadPlace" AutoPostBack="true" 
                                                        OnTextChanged="HeadSearch"></asp:TextBox></HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblPlace" Text='<%# Eval("place") %>'></asp:Label><asp:TextBox runat="server" ID="txtPlace" Text='<%# Eval("place") %>' 
                                            Visible="false" ></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField ><HeaderTemplate><asp:LinkButton runat="server" ID="lnkbtnSortCapital"  text="Capital" CssClass="styleGridHeader"
                                                        OnClick="DoSort"></asp:LinkButton><br /><br /><asp:TextBox runat="server" ID="txtHeadCapital" AutoPostBack="true" 
                                                        OnTextChanged="HeadSearch"></asp:TextBox></HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblCapital" Text='<%# Eval("capital") %>'></asp:Label><asp:TextBox runat="server" ID="txtCapital" Text='<%# Eval("capital") %>' 
                                            Visible="false" ></asp:TextBox></ItemTemplate></asp:TemplateField><asp:TemplateField ><HeaderTemplate><asp:LinkButton runat="server" ID="lnkbtnSortCountry"  text="Country" CssClass="styleGridHeader"
                                                        OnClick="DoSort"></asp:LinkButton><br /><br /><asp:TextBox runat="server" ID="txtHeadCountry" AutoPostBack="true" 
                                                        OnTextChanged="HeadSearch" EnableViewState="true"></asp:TextBox></HeaderTemplate><ItemTemplate><asp:Label runat="server" ID="lblCountry" Text='<%# Eval("country") %>'></asp:Label><asp:TextBox runat="server" ID="txtCountry" Text='<%# Eval("country") %>' 
                                            Visible="false" ></asp:TextBox></ItemTemplate></asp:TemplateField></Columns></asp:GridView></ContentTemplate></cc1:TabPanel>
      
        <cc1:TabPanel runat="server" HeaderText="Bank Details" ID="TabPanel3">
        <ContentTemplate>
        <table width="90%">
                <tr>
                    <td>
                        <table width="100%" >
                            <tr>
                                <td><asp:Label ID="Label21" runat="server" Text="Line of Business"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox16" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label22" runat="server" Text="Branch"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox17" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label23" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList6" runat="server">
                                        <asp:ListItem>Account Description1</asp:ListItem>
                                        <asp:ListItem>Account Description2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label24" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox18" runat="server"></asp:TextBox></td>
                           </tr>
                            <tr>
                                <td><asp:Label ID="Label25" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList7" runat="server">
                                        <asp:ListItem>Acc Desc1</asp:ListItem>
                                        <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label26" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox19" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label27" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox20" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label28" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox21" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label29" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList8" runat="server">
                                         <asp:ListItem>Acc Desc1</asp:ListItem>
                                            <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label30" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox22" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label31" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox23" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label32" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox24" runat="server"></asp:TextBox></td>
                            </tr>
                      </table>
                    </td>
                </tr>
                <tr>
                  <td>
                         <table width="100%" >
                            <tr>
                                <td><asp:Label ID="Label33" runat="server" Text="Line of Business"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox25" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label34" runat="server" Text="Branch"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox26" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label35" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList9" runat="server">
                                        <asp:ListItem>Acc Desc1</asp:ListItem>
                                        <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label36" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox27" runat="server"></asp:TextBox></td>
                           </tr>
                            <tr>
                                <td><asp:Label ID="Label37" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList10" runat="server">
                                        <asp:ListItem>Acc Desc1</asp:ListItem>
                                        <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label38" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox28" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label39" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox29" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label40" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox30" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label41" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:DropDownList ID="DropDownList11" runat="server">
                                         <asp:ListItem>Acc Desc1</asp:ListItem>
                                            <asp:ListItem>Acc Desc2</asp:ListItem></asp:DropDownList></td>
                                <td><asp:Label ID="Label42" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox31" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="Label43" runat="server" Text="Account Description"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox32" runat="server"></asp:TextBox></td>
                                <td><asp:Label ID="Label44" runat="server" Text="Account Flag"></asp:Label></td>
                                <td><asp:TextBox ID="TextBox33" runat="server"></asp:TextBox></td>
                            </tr>
                      </table>
                  </td>
                </tr>
                <tr>
                    <td>
                                             </td>
              </tr>
      </table>
                      </ContentTemplate></cc1:TabPanel>
       
          
    <cc1:TabPanel runat="server" HeaderText="Business Details" ID="TabPanel9"><ContentTemplate>Page 9</ContentTemplate></cc1:TabPanel>
        
      
            
            
</cc1:TabContainer>
</td>

</tr>
<tr><td align="center" class="styleTabSubmitTd"><asp:Button runat="server" ID="btnSave"
 CssClass="styleSubmitButton" Text="Submit"  onclick="btnSave_Click"  /></td></tr>

</table>
</ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

