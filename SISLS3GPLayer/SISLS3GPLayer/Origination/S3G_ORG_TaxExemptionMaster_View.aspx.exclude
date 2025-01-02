<%@ Page Title="S3G - Tax Exemption Master" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_ORG_TaxExemptionMaster_View.aspx.cs" Inherits="Origination_S3G_ORG_TaxExemptionMaster_View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Tax Exemption Master - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="grvTaxExemption" AllowSorting="true" OnRowDataBound="grvTaxExemption_RowDataBound"
                            Width="100%" OnRowCommand="grvTaxExemption_RowCommand" AutoGenerateColumns="false"
                            RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Tax_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="styleGridHeader">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("Tax_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax Exemption Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTax_Exemption_Code" runat="server" Text='<%# Bind("Tax_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnTax_Exemption_Code" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="Tax_Exemption_Code"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnTax_Exemption_Code" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By Tax_Exemption_Code" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lessee" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLessee" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnLessee" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="Lessee"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnLessee" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By Lessee" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="TAN" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTAN" runat="server" Text='<%# Bind("TAN") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortTAN" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="TAN"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortTAN" runat="server" AlternateText="Sort By TAN"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" MaxLength="12" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Section" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSection" runat="server" Text='<%# Bind("Tax_Law_Section") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSection" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="Section"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSection" runat="server" AlternateText="Sort By Section"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="8" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtHeaderSearch4"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFrom_Date" runat="server" Text='<%#Eval("Effective_From").ToString()%>'></asp:Label>

                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnFrom_Date" CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"
                                                        runat="server" Text="From_Date"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnFrom_Date" runat="server" AlternateText="Sort By From_Date"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Right"
                                    DataField="Exe_Limit_Amount" HeaderText="Exemption Limit" />
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
