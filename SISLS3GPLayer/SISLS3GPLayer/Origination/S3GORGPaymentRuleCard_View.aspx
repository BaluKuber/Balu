<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGPaymentRuleCard_View.aspx.cs" Inherits="S3GORGPaymentRuleCard_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
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
                <tr>
                    <td align="center">
                        <asp:GridView ID="grvPaging" runat="server" Width="100%" AutoGenerateColumns="false"
                            OnRowCommand="grvPaging_RowCommand" OnRowDataBound="grvPaging_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Payment_RuleCard_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" runat="server" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("Payment_RuleCard_ID") %>' CommandName="Modify" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LOB" SortExpression="LOB">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort1" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Line of Business" Text="Line of Business"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort1" runat="server" CssClass="styleImageSortingAsc"
                                                        OnClientClick="return false;" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="AccountType">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort2" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By AccountType" Text="Account Type"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort2" runat="server" CssClass="styleImageSortingAsc"
                                                        OnClientClick="return false;" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBName" runat="server" Text='<%# Bind("AccountType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Payment_Rule_Number">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort3" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Payment Rule Number" Text="Payment Rule Number"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort3" runat="server" CssClass="styleImageSortingAsc"
                                                        OnClientClick="return false;" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" MaxLength="40" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="-/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentRuleNumber" runat="server" Text='<%# Bind("Payment_Rule_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" Checked='<%# FunPriCheckBool(Eval("Is_Active").ToString()) %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
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
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
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
