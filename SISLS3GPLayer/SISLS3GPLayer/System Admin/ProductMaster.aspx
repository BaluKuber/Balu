<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProductMaster.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    Inherits="ProductMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1">

    <script language="javascript" type="text/javascript">
        function ValidateString() {
            if ((window.event.keyCode < 97 || window.event.keyCode > 122) && (window.event.keyCode < 65 || window.event.keyCode > 90) && (window.event.keyCode < 48 || window.event.keyCode > 57) && window.event.keyCode != 32 && window.event.keyCode != 95) {
                window.event.keyCode = 0;
            }
        }
        function ValidateNumeric() {

            if ((window.event.keyCode < 48 || window.event.keyCode > 57)) {
                window.event.keyCode = 0;
            }
        }
               
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr  class="styleMessageTd" >
                    <td align="center">
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Names="tahoma"
                            CssClass="styleMessageLabel" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                      <asp:GridView ID="gvMaster" runat="server" AutoGenerateColumns="False" Font-Names="Arial"
                            AllowPaging="true" OnRowDataBound="gvMaster_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Image ID="lblSign" runat="server" ImageUrl="~/Images/Edit.JPG" Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    Select
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:CheckBox ID="chkHeadSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckSelect" />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="CheckSelect" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHeadId" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                            <td> </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnGroupName" runat="server" OnClick="DoGroup" Text="Name"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortName" runat="server" AlternateText="Sort By Name"
                                                        ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" OnClick="DoSort" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:ImageButton runat="server" ID="imgbtnSearchName" ImageUrl="~/Images/Filter.jpg"  Visible="false"
                                                        OnClick="OpenCloseSearch" />
                                                       
                                                   
                                                </td>
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeadName" runat="server" AutoPostBack="true" onkeypress="ValidateString();"
                                                        OnTextChanged="HeadSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr><td><div runat="server" id="divNameSearch" visible="false" style="position:absolute">
                                                      <asp:ListBox runat="server" ID="lstbxSearchName" Height="50px" OnSelectedIndexChanged="ConditionSearch"  AutoPostBack="True">
                                                            <asp:ListItem Text="Contains" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Not Contains" Value="2"></asp:ListItem>
                                                        </asp:ListBox>
                                                    </div></td></tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="imgbtnPlusMinus" runat="server" ImageAlign="Middle" ImageUrl="~/Images/minusbox.gif"
                                                        OnClick="PlusMinus" />
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtName" runat="server" onkeypress="ValidateString();" Text='<%# Eval("name") %>'
                                                        Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                               <td></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnGroupPlace" runat="server" OnClick="DoGroup" Text="Place"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortPlace" runat="server" AlternateText="Sort By Place"
                                                        ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" OnClick="DoSort" />
                                                </td>
                                            </tr>
                                           <tr align="left">
                                                <td>
                                                    <asp:ImageButton runat="server" ID="imgbtnSearchPlace" ImageUrl="~/Images/Filter.jpg"  Visible="false"
                                                        OnClick="OpenCloseSearch" />
                                                   
                                                </td>
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeadPlace" runat="server" AutoPostBack="true" onkeypress="ValidateString();"
                                                        OnTextChanged="HeadSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr><td> <div runat="server" id="divPlaceSearch" visible="false" style="position:absolute">
                                                        <asp:ListBox runat="server" ID="lstbxSearchPlace" Height="50px" OnSelectedIndexChanged="ConditionSearch"  AutoPostBack="True">
                                                            <asp:ListItem Text="Contains" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Not Contains" Value="2"></asp:ListItem>
                                                        </asp:ListBox>
                                                    </div></td></tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPlace" runat="server" Text='<%# Eval("place") %>'></asp:Label>
                                        <asp:TextBox AutoCompleteType="None" ID="txtPlace" runat="server" onkeypress="ValidateString();" Text='<%# Eval("place") %>'
                                            Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                               <td></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnGroupCapital" runat="server" OnClick="DoGroup" Text="Capital"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortCapital" runat="server" AlternateText="Sort By Capital"
                                                        ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" OnClick="DoSort" />
                                                </td>
                                            </tr>
                                         <tr align="left">
                                                <td>
                                                    <asp:ImageButton runat="server" ID="imgbtnSearchCapital" ImageUrl="~/Images/Filter.jpg"  Visible="false"
                                                        OnClick="OpenCloseSearch" />
                                                  
                                                </td>
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeadCapital" runat="server" AutoPostBack="true" onkeypress="ValidateString();"
                                                        OnTextChanged="HeadSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr><td>  <div runat="server" id="divCapitalSearch" visible="false" style="position:absolute">
                                                        <asp:ListBox runat="server" ID="lstbxSearchCapital" Height="50px" OnSelectedIndexChanged="ConditionSearch"  AutoPostBack="True">
                                                            <asp:ListItem Text="Contains" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Not Contains" Value="2"></asp:ListItem>
                                                        </asp:ListBox>
                                                    </div></td></tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCapital" runat="server" Text='<%# Eval("capital") %>'></asp:Label>
                                        <asp:TextBox AutoCompleteType="None" ID="txtCapital" runat="server" onkeypress="ValidateString();" Text='<%# Eval("capital") %>'
                                            Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                               <td></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnGroupCountry" runat="server" OnClick="DoGroup" Text="Country"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortCountry" runat="server" AlternateText="Sort By Country"
                                                        ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" OnClick="DoSort" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:ImageButton runat="server" ID="imgbtnSearchCountry" ImageUrl="~/Images/Filter.jpg"  Visible="false"
                                                        OnClick="OpenCloseSearch" />
                                                  
                                                </td>
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeadCountry" runat="server" AutoPostBack="true" EnableViewState="true" 
                                                        onkeypress="ValidateString();" OnTextChanged="HeadSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr><td>  <div runat="server" id="divCountrySearch" visible="false" style="position:absolute">
                                                        <asp:ListBox runat="server" ID="lstbxSearchCountry" Height="50px" OnSelectedIndexChanged="ConditionSearch"  AutoPostBack="True">
                                                            <asp:ListItem Text="Contains" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Not Contains" Value="2"></asp:ListItem>
                                                        </asp:ListBox>
                                                    </div></td></tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("country") %>'></asp:Label>
                                        <asp:TextBox AutoCompleteType="None" ID="txtCountry" runat="server" onkeypress="ValidateString();" Text='<%# Eval("country") %>'
                                            Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr align="center">
                                        <td>
                                            <asp:ImageButton runat="server" ID="imgbtnFirst" ImageUrl="~/Images/First.gif" OnClick="imgbtnFirst_Click"
                                                Enabled="false" />
                                            <asp:ImageButton runat="server" ID="imgbtnPrev" ImageUrl="~/Images/Prev.gif" OnClick="imgbtnPrev_Click"
                                                Enabled="false" />
                                        </td>
                                        
                                        <td style="width: 30px;">
                                            
                                           <asp:TextBox AutoCompleteType="None"  runat="server" ID="txtPagerSearch" MaxLength="3" 
                                                AutoPostBack="true" onkeypress="ValidateNumeric();" OnTextChanged="PagerSearch"
                                                Width="20px" Height="10px" Text="1" ></asp:TextBox> 
                                           
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblPagerSearch" Font-Size="X-Small" Font-Names="Arial"
                                                ForeColor="Indigo"></asp:Label>&nbsp;&nbsp;
                                        </td>
                                        <td>
                                            <asp:DataList runat="server" ID="dlstPager" CellPadding="1" CellSpacing="1" OnItemCommand="dlstPager_ItemCommand"
                                                OnItemDataBound="dlstPager_ItemDataBound" RepeatDirection="Horizontal">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCount" runat="server" Text='<%# Eval("Count") %>' Visible="false" />
                                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' Font-Size="Small"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imgbtnNext" ImageUrl="~/Images/Next.gif" OnClick="imgbtnNext_Click"
                                                Enabled="false" />
                                            <asp:ImageButton runat="server" ID="imgbtnLast" ImageUrl="~/Images/Last.gif" OnClick="imgbtnLast_Click"
                                                Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </PagerTemplate>
                            <PagerStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button Text="Add" runat="server" ID="btnAdd" OnClick="btnAdd_Click" CssClass="styleSubmitButton" />
                        &nbsp;
                        <asp:Button Text="Edit" runat="server" ID="btnEdit" OnClick="btnEdit_Click" CssClass="styleSubmitButton" />
                        &nbsp;
                        <asp:Button Text="Delete" runat="server" ID="btnDelete" OnClick="btnDelete_Click"
                            CssClass="styleSubmitButton" />
                        &nbsp;
                        <asp:Button Text="Refresh" runat="server" ID="btnRefresh" OnClick="btnRefresh_Click"
                            CssClass="styleSubmitButton" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
