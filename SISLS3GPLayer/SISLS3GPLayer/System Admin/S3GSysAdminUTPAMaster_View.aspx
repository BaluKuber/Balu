<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminUTPAMaster_View.aspx.cs" Inherits="S3GSysAdminUTPAMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="grvLOBMaster" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvLOBMaster_RowDataBound"
                            HeaderStyle-CssClass="styleGridHeader" Width="100%" OnRowCommand="grvLOBMaster_RowCommand">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("UTPA_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                            CommandArgument='<%# Bind("UTPA_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UTPA Code" SortExpression="UTPA_Code">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortCode" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        ToolTip="Sort By UTPACode" OnClick="FunProSortingColumn" Text="UTPA Code"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortCode" runat="server" CssClass="styleImageSortingAsc"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="6"
                                                        AutoPostBack="true" CssClass="styleSearchBox" ToolTip="Type UpperCase Letters"
                                                        OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters,Numbers,Custom,LowercaseLetters" ValidChars="/" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("UTPA_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UTPA Name" SortExpression="UTPA_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortName" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        ToolTip="Sort By UTPAName" OnClick="FunProSortingColumn" Text="UTPA Name"></asp:LinkButton>
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
                                        <asp:Label ID="lblLOBName" runat="server" Text='<%# Bind("UTPA_Name") %>'></asp:Label>
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
                                <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
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
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage" cssclass="styleMandatory"></span>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CssClass="styleSubmitButton"
                            CausesValidation="false" Text="Create" runat="server"></asp:Button>
                       
                        <asp:Button ID="btnShowAll" OnClick="btnShowAll_Click" runat="server" Text="Show All"
                            CssClass="styleSubmitButton" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
