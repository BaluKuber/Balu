<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminCurrencyMaster_View.aspx.cs" Inherits="S3GSysAdminCurrencyMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="grvCurrencyMaster" OnRowDataBound="grvCurrencyMaster_RowDataBound"
                            OnRowCommand="grvCurrencyMaster_RowCommand" Width="100%" 
                            AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="8%">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Currency_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="8%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("Currency_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency Code" ItemStyle-HorizontalAlign="Left" SortExpression="Currency_Code"
                                    HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Bind("Currency_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnCurrencyCode" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" ToolTip="Sort By Code" Text="Currency Code"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnCurrencyCode" runat="server" CssClass="styleImageSortingAsc"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" MaxLength="3" runat="server"
                                                        CssClass="styleSearchBox" AutoPostBack="true" OnTextChanged="FunProHeaderSearch" Width="120px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters, LowercaseLetters" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency Name" ItemStyle-HorizontalAlign="Left" SortExpression="Currency_Name"
                                    HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="40%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencyName" runat="server" Text='<%# Bind("Currency_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnCurrencyName" OnClick="FunProSortingColumn" ToolTip="Sort By Name"
                                                        CssClass="styleGridHeaderLinkBtn" runat="server" Text="Currency Name"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnCurrencyName" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" onkeypress="fnCheckSpecialChars(true);"
                                                        MaxLength="40" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                        CssClass="styleSearchBox"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                        FilterType="UppercaseLetters, LowercaseLetters,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Precision" HeaderStyle-CssClass="styleGridHeader"
                                    ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrecision" runat="server" Text='<%# Bind("Precision") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Precision
                                                    <asp:LinkButton ID="lnkbtnPrecision" Visible="false" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="Precision"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortPrecision" Visible="false" runat="server" AlternateText="Sort By Precision"
                                                        ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" Visible="false" ID="txtHeaderPrecision" MaxLength="1"
                                                        runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderPrecision"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%#Eval("Created_By")%>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%#Eval("User_Level_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Create" runat="server"></asp:Button>
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                            OnClick="btnShowAll_Click" />
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
