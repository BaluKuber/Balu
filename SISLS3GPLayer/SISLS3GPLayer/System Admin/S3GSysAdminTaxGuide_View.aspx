<%@ Page Title="S3G - Tax Guide Master" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminTaxGuide_View.aspx.cs" Inherits="S3GSysAdminTaxGuide_View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Tax Guide Master - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<div id="divGrid1" style="overflow: scroll; width: 800px;">--%>
                        <asp:GridView runat="server" ID="grvTaxGuide" AllowSorting="true" OnRowDataBound="grvTaxGuide_RowDataBound"
                            Width="100%" OnRowCommand="grvTaxGuide_RowCommand" AutoGenerateColumns="false"
                            RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Tax_Guide_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="styleGridHeader">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("Tax_Guide_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblState_Name" runat="server" Text='<%# Bind("State_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnstate" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="State"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnstate" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By State" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxCode" runat="server" Text='<%# Bind("Tax_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortCode" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="Tax Code"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortCode" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By TaxCode" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch2"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TaxType" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxType" runat="server" Text='<%# Bind("TaxType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortTaxType" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="TaxType"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortTaxType" runat="server" AlternateText="Sort By TaxType"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" MaxLength="12" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TaxClass" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxClass" runat="server" Text='<%# Bind("TaxClass") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnTaxClass" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="TaxClass"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnTaxClass" runat="server" AlternateText="Sort By TaxClass"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="8" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtHeaderSearch4"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LOB Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortName" CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"
                                                        runat="server" Text="LOB Code"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortName" runat="server" AlternateText="Sort By LOBCode"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" CssClass="styleSearchBox"
                                                        Width="80px" MaxLength="3" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch5"
                                                        FilterType="UppercaseLetters,LowercaseLetters" ValidChars="" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Right"
                                    DataField="RatePercentage" HeaderText="Rate%" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("USRID")%>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
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
