<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="S3GSysAdminLOBMaster_View.aspx.cs" Inherits="S3GSysAdminLOBMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="stylePageHeading">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="grvLOBMaster" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvLOBMaster_RowDataBound"
                    HeaderStyle-CssClass="styleGridHeader" Width="100%" 
                    onrowcommand="grvLOBMaster_RowCommand">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("LOB_ID") %>' CommandName="Query" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    CommandArgument='<%# Bind("LOB_ID") %>' CommandName="Modify" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Line of Business Code" SortExpression="LOB_Code">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSortCode" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                ToolTip="Sort By Line of Business Code" OnClick="FunProSortingColumn" Text="Line of Business Code"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSortCode"  runat="server" CssClass="styleImageSortingAsc"
                                                ImageAlign="Middle" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="2"
                                                AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("LOB_Code") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Line of Business Name" SortExpression="LOB_Name">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSortName" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                ToolTip="Sort By Line of Business Name" OnClick="FunProSortingColumn" Text="Line of Business Name"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSortName" runat="server" CssClass="styleImageSortingAsc"
                                                ImageAlign="Middle" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" CssClass="styleSearchBox"
                                                AutoPostBack="true" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLOBName" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField  Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By")%>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                       </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr align="center">
            <td align="center" valign="top">
                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                <%--  <table border="0" cellpadding="0" cellspacing="0">
                    <tr align="center">
                        <td>
                            <asp:ImageButton runat="server" ID="imgbtnFirst" ImageUrl="~/Images/First.gif" Enabled="false" />
                            <asp:ImageButton runat="server" ID="imgbtnPrev" ImageUrl="~/Images/Prev.gif" Enabled="false" />
                        </td>
                        <td>
                            <asp:DataList runat="server" ID="dlstPager" CellPadding="1" CellSpacing="1" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                        CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' Font-Size="Small"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                        <td>
                            <asp:ImageButton runat="server" ID="imgbtnNext" ImageUrl="~/Images/Next.gif" Enabled="false" />
                            <asp:ImageButton runat="server" ID="imgbtnLast" ImageUrl="~/Images/Last.gif" Enabled="false" />
                        </td>
                    </tr>
                </table>--%>
            </td>
        </tr>
        <tr class="styleButtonArea">
            <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                <span runat="server" id="lblErrorMessage" cssclass="styleMandatory"></span>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                <asp:Button ID="btnCreate" runat="server" OnClick="btnCreate_Click" CssClass="styleSubmitButton"
                    Text="Create"></asp:Button>
          
                <asp:Button ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" CssClass="styleSubmitButton"
                    Text="Show All" />
            </td>
        </tr>
        <%--<tr>
            <td>
                <asp:ValidationSummary ID="vsLOB" runat="server" ShowMessageBox="false" CssClass="styleMandatoryLabel" />
            </td>
        </tr>--%>
    </table>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
</asp:Content>
