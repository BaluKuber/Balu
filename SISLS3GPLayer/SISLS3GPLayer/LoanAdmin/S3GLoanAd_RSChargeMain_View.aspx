<%@ Page Title="S3G - RS Charge Maintenance" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GLoanAd_RSChargeMain_View.aspx.cs" Inherits="LoanAdmin_S3GLoanAd_RSChargeMain_View" %>

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
                                    <asp:Label runat="server" Text="RS Charge Maintenance - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="grvRSChargeMaintenance" AllowSorting="true" OnRowDataBound="grvRSChargeMaintenance_RowDataBound"
                            Width="100%" OnRowCommand="grvRSChargeMaintenance_RowCommand" AutoGenerateColumns="false"
                            RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" >
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("RSCM_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="10%"  >
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("RSCM_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RS Charge Maintenance Code" HeaderStyle-Width="20%"  ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRSCMCode" runat="server" Text='<%# Bind("RSCM_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnRSCMCode" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        OnClick="FunProSortingColumn" Text="RS Charge Maintenance Code"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnRSCMCode" runat="server" CssClass="styleImageSortingAsc"
                                                        AlternateText="Sort By RSCM_Code" ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="15"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftRSCMCode" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="/"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>                              
                                 <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%"  HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortCustomer_Name" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="Customer Name"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortCustomer_Name" runat="server" AlternateText="Sort By Customer_Name"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" MaxLength="12" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftCustomer_Name" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>   
                                <asp:TemplateField HeaderText="Tranche" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%"  HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranche" runat="server" Text='<%# Bind("Tranche_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortTranche" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" Text="Tranche"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortTranche" runat="server" AlternateText="Sort By Tranche_Name"
                                                        ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="12" runat="server"
                                                        Width="120px" AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftTranche" runat="server" TargetControlID="txtHeaderSearch4"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" " Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>                              
                                <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20%"  HeaderStyle-CssClass="styleGridHeader">
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
                    <td style="padding-top: 5px; padding-left: 5px;" align="center">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="center">
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
