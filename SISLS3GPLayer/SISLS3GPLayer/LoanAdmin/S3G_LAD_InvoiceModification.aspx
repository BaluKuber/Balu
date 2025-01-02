<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_LAD_InvoiceModification.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="LoanAdmin_S3G_LAD_InvoiceModification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LOV" Src="~/UserControls/LOBMasterView.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Invoice Modification" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Input Details">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" CssClass="styleReqFieldLabel" Text="Invoive Month">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlInvoiveMonth" runat="server" AutoPostBack="true" ValidationGroup="Main" ErrorMessage="Select Invoive Month" IsMandatory="true" ServiceMethod="GetInvoiceMonth" Width="200px" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label2" runat="server" CssClass="styleReqFieldLabel" Text="Invoice Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlInvoiceType" ValidationGroup="Main" runat="server" AutoPostBack="true">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Rental" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Interim" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Other Cashflow" Value="3"></asp:ListItem>
                                            <%--<asp:ListItem Value="4" Text="Debit Credit Note"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="LAS"></asp:ListItem>
                                            <asp:ListItem Value="6" Text="ODI"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvInvoiceType" runat="server" Enabled="true" ControlToValidate="ddlInvoiceType"
                                            ErrorMessage="Select Invoice Type" Display="None" ValidationGroup="Main" InitialValue="0">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label3" runat="server" CssClass="styleReqFieldLabel" Text="Tranche Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlTranche" runat="server" AutoPostBack="true" ValidationGroup="Main" ErrorMessage="Select Tranche Name" IsMandatory="true" ServiceMethod="GetTrancheName" Width="200px" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoicedateTo" runat="server" Text="Invoice Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoiceDateTo" runat="server" MaxLength="10"></asp:TextBox>
                                        <asp:Image ID="imgdateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtInvoiceDateTo"
                                            PopupButtonID="imgdateTo" Enabled="True">
                                        </cc1:CalendarExtender>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlInvoiceNo" runat="server" ServiceMethod="GetInvoiceList" Width="200px" />
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="4" align="center">
                                        <asp:Button runat="server" ID="btnGo" CssClass="styleSubmitButton"
                                            Text="Re Run" ValidationGroup="Main" OnClick="btnRegen_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table align="left">
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="Main" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="Main" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="cvTranche" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function fnConfirmSave() {
            if (confirm('Are you sure want to save?')) {
                return true;
            }
            else {
                return false;
            }
        }

    </script>
</asp:Content>

