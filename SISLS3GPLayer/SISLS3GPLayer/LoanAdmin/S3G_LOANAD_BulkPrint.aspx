<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_LOANAD_BulkPrint.aspx.cs"
    Inherits="LoanAdmin_S3G_LOANAD_BulkPrint" EnableEventValidation="false" %>

<%@ Register TagPrefix="uc3" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
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
                    <td>
                        <asp:Panel GroupingText="Document Type" ID="pnlDocumentType" runat="server" CssClass="stylePanel"
                            ToolTip="Lessee / Vendor Details">
                            <table width="99%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDocumentType" runat="server" Text="Document Type" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldocumentType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddldocumentType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvDocumentType" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="ddldocumentType"
                                            ErrorMessage="Select Document Type" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>

                                    <asp:Panel GroupingText="Purchase Order Search Details" ID="pnlPOSearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Purchase Order Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPO_From_Date" CssClass="styleDisplayLabel" Text="PO From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPO_From_Date" runat="server" ToolTip="PO From Date"></asp:TextBox>
                                                    <asp:Image ID="imgPOFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderPOFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgPOFromDate" TargetControlID="txtPO_From_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPO_To_Date" CssClass="styleDisplayLabel" Text="PO To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPO_To_Date" runat="server" ToolTip="PO To Date"></asp:TextBox>
                                                    <asp:Image ID="imgPOToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderPOToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgPOToDate" TargetControlID="txtPO_To_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPOCustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlPOCustomerName" runat="server" ServiceMethod="GetCustomerNameDetails" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPOVendorName" runat="server" Text="Vendor Name" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlPOVendorName" runat="server" ServiceMethod="GetVendors" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPOLoadSequenceNo" CssClass="styleDisplayLabel" Text="PO Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlPOLoadSequenceNo" runat="server" ServiceMethod="GetLSQNo" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Rental Achedule Aggrement Search Details" ID="pnlRSASearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Rental Achedule Aggrement Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblRSFromDate" CssClass="styleReqFieldLabel" Text="RS From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRS_From_Date" runat="server" ToolTip="RS From Date"></asp:TextBox>
                                                    <asp:Image ID="imgRSFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderRSFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgRSFromDate" TargetControlID="txtRS_From_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvRSFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtRS_From_Date"
                                                        ErrorMessage="Enter RS From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblRSToDate" CssClass="styleReqFieldLabel" Text="RS To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRS_To_Date" runat="server" ToolTip="RS To Date"></asp:TextBox>
                                                    <asp:Image ID="imgRSToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderRSToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgRSToDate" TargetControlID="txtRS_To_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvRSToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtRS_To_Date"
                                                        ErrorMessage="Enter RS To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>

                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblRSAccountNumber" CssClass="styleDisplayLabel" Text="Account No."></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlRSAccountNumber" runat="server" ServiceMethod="GetRSNo" />
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblRSCustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlRSCustomerName" runat="server" ServiceMethod="GetCustomerNameDetails" />
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="MRA Search Details" ID="pnlMRASearch" runat="server" CssClass="stylePanel"
                                        ToolTip="MRA Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblMRAFromDate" CssClass="styleReqFieldLabel" Text="MRA From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMRAFromDate" runat="server" ToolTip="MRA From Date"></asp:TextBox>
                                                    <asp:Image ID="imgMRAFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderMRAFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgMRAFromDate" TargetControlID="txtMRAFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvMRAFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtMRAFromDate"
                                                        ErrorMessage="Enter MRA From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblMRAToDate" CssClass="styleReqFieldLabel" Text="MRA To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMRAToDate" runat="server" ToolTip="MRA To Date"></asp:TextBox>
                                                    <asp:Image ID="imgMRAToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderMRAToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgMRAToDate" TargetControlID="txtMRAToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvMRAToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtMRAToDate"
                                                        ErrorMessage="Enter MRA To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>

                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblMRACustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlMRACustomerName" runat="server" ServiceMethod="GetCustomerNameDetails" />
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Pricing Search Details" ID="pnlPricingSearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Pricing Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPricingFromDate" CssClass="styleReqFieldLabel" Text="Pricing From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPricingFromDate" runat="server" ToolTip="Pricing From Date"></asp:TextBox>
                                                    <asp:Image ID="imgPricingFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderPricingFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgPricingFromDate" TargetControlID="txtPricingFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvPricingFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPricingFromDate"
                                                        ErrorMessage="Enter Pricing From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPricingToDate" CssClass="styleReqFieldLabel" Text="Pricing To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPricingToDate" runat="server" ToolTip="Pricing To Date"></asp:TextBox>
                                                    <asp:Image ID="imgPricingToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderPricingToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgPricingToDate" TargetControlID="txtPricingToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvPricingToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPricingToDate"
                                                        ErrorMessage="Enter Pricing To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>

                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblPricingNumber" CssClass="styleDisplayLabel" Text="Pricing No."></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlPricingNo" runat="server" ServiceMethod="GetPricingNo" />
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblPricingCustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlPricingCustomerName" runat="server" ServiceMethod="GetCustomerNameDetails" />
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="NOA Search Details" ID="pnlNOASearch" runat="server" CssClass="stylePanel"
                                        ToolTip="NOA Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblNOAFromDate" CssClass="styleReqFieldLabel" Text="Deal From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtNOAFromDate" runat="server" ToolTip="Deal From Date"></asp:TextBox>
                                                    <asp:Image ID="imgNOAFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderNOAFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgNOAFromDate" TargetControlID="txtNOAFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvNOAFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtNOAFromDate"
                                                        ErrorMessage="Enter Deal From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblNOAToDate" CssClass="styleReqFieldLabel" Text="Deal To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtNOAToDate" runat="server" ToolTip="Deal To Date"></asp:TextBox>
                                                    <asp:Image ID="imgNOAToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderNOAToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgNOAToDate" TargetControlID="txtNOAToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvNOAToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtNOAToDate"
                                                        ErrorMessage="Enter Deal To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblNoaNumber" CssClass="styleDisplayLabel" Text="Deal No."></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlDealNo" runat="server" ServiceMethod="GetDealNo" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblNOACustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlNOACustomerName" runat="server" ServiceMethod="GetCustomerNameDetails" />
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblReportFormatType" CssClass="styleReqFieldLabel" Text="Report Format Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:DropDownList ID="ddlReportFormatType" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvNOAReportFormat" InitialValue="0" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="ddlReportFormatType"
                                                        ErrorMessage="Select Report Format" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Debit/Credit Note Search Details" ID="pnlDCSearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Debit/Credit Note Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblDCFromDate" CssClass="styleReqFieldLabel" Text="Debit/Credit From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtDCFromDate" runat="server" ToolTip="Debit/Credit From Date"></asp:TextBox>
                                                    <asp:Image ID="imgDCFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderDCFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgDCFromDate" TargetControlID="txtDCFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvDCFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtDCFromDate"
                                                        ErrorMessage="Enter Debit/Credit From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblDCToate" CssClass="styleReqFieldLabel" Text="Debit/Credit To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtDCToDate" runat="server" ToolTip="Debit/Credit To Date"></asp:TextBox>
                                                    <asp:Image ID="imgDCToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderDCToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgDCToDate" TargetControlID="txtDCToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvDCToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtDCToDate"
                                                        ErrorMessage="Enter Debit/Credit To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblDCEntityName" CssClass="styleDisplayLabel" Text="Lessee/Funder/Vendor Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlDCEntityName" runat="server" ServiceMethod="GetDCEntityName" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblDCNo" CssClass="styleDisplayLabel" Text="Debit/Credit No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlDCNo" runat="server" ServiceMethod="GetDCNo" />
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Credit Note Search Details" ID="pnlCNSearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Credit Note Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblCNFromDate" CssClass="styleReqFieldLabel" Text="Credit Note From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCNFromDate" runat="server" ToolTip="Credit Note From Date"></asp:TextBox>
                                                    <asp:Image ID="imgCNFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderCNFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgCNFromDate" TargetControlID="txtCNFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvCNFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtCNFromDate"
                                                        ErrorMessage="Enter Credit Note From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblCNToate" CssClass="styleReqFieldLabel" Text="Credit Note To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCNToDate" runat="server" ToolTip="Credit Note To Date"></asp:TextBox>
                                                    <asp:Image ID="imgCNToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderCNToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgCNToDate" TargetControlID="txtCNToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvCNToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtCNToDate"
                                                        ErrorMessage="Enter Credit Note To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Other CashFlow Search Details" ID="pnlOCPSearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Other CashFlow Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblOCPFromDate" CssClass="styleReqFieldLabel" Text="From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOCPFromdate" runat="server" ToolTip="From Date"></asp:TextBox>
                                                    <asp:Image ID="imgOCPFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderOCPFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgOCPFromDate" TargetControlID="txtOCPFromdate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvOCPFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtOCPFromdate"
                                                        ErrorMessage="Enter From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblOCPToDate" CssClass="styleReqFieldLabel" Text="To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOCPTodate" runat="server" ToolTip="To Date"></asp:TextBox>
                                                    <asp:Image ID="imgOCPToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderOCPToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgOCPToDate" TargetControlID="txtOCPTodate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvOCPToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtOCPTodate"
                                                        ErrorMessage="Enter To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Rental Invoice Search Details" ID="pnlRISearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Rental Invoice Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblRIFromDate" CssClass="styleReqFieldLabel" Text="From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRIFromDate" runat="server" ToolTip="From Date"></asp:TextBox>
                                                    <asp:Image ID="imgRIFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderRIFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgRIFromDate" TargetControlID="txtRIFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvRIFromDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtRIFromDate"
                                                        ErrorMessage="Enter From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblRIToDate" CssClass="styleReqFieldLabel" Text="To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRIToDate" runat="server" ToolTip="To Date"></asp:TextBox>
                                                    <asp:Image ID="imgRIToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderRIToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgRIToDate" TargetControlID="txtRIToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvRIToDate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtRIToDate"
                                                        ErrorMessage="Enter To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblRIBillNumber" CssClass="styleDisplayLabel" Text="Bill No."></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlRIBillNumber" runat="server" ServiceMethod="GetBillNo" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblRILocation" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlRILocation" runat="server" ServiceMethod="GetLocation" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Interim Invoice Search Details" ID="pnlIISearch" runat="server" CssClass="stylePanel"
                                        ToolTip="Interim Invoice Search Details" Style="display: none">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblIIFromDate" CssClass="styleReqFieldLabel" Text="From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtIIFromDate" runat="server" ToolTip="From Date"></asp:TextBox>
                                                    <asp:Image ID="imgIIFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderIIFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgIIFromDate" TargetControlID="txtIIFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvIIFromdate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtIIFromDate"
                                                        ErrorMessage="Enter From Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblIIToDate" CssClass="styleReqFieldLabel" Text="To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtIIToDate" runat="server" ToolTip="To Date"></asp:TextBox>
                                                    <asp:Image ID="imgToFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderIIToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgToFromDate" TargetControlID="txtIIToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvIITodate" ValidationGroup="Search" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtIIToDate"
                                                        ErrorMessage="Enter To Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblDocNumber" CssClass="styleDisplayLabel" Text="Document Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlIIDocNumber" runat="server" ServiceMethod="GetDocNo" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblAccNumber" CssClass="styleDisplayLabel" Text="Invoice Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlIIAccNumber" runat="server" ServiceMethod="GetAccountNumber" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="center" colspan="4">
                        <asp:Button runat="server" ID="btnSearch" CssClass="styleSubmitButton" Text="Search"
                            OnClick="btnSearch_Click" ValidationGroup="Search" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>

                                    <asp:Panel GroupingText="Purchase Order Details" ID="pnlPO" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvPO" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%" OnRowDataBound="gvPO_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPOSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblPO_dtl_ID" runat="server" Text='<%# Eval("PO_HDR_ID") %>' Visible="false" />
                                                        <asp:Label ID="lblPO_Vendor_Group" runat="server" Text='<%# Eval("PO_Vendor_Group") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PO Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPO_Number" runat="server" Text='<%# Eval("PO_Number") %>' ToolTip="PO Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PO Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPO_Date" runat="server" Text='<%# Eval("PO_Date") %>'
                                                            ToolTip="PO Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lessee Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Customer_Name") %>'
                                                            ToolTip="Lessee Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vendor Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEntity_Name" runat="server" Text='<%# Eval("Entity_Name") %>'
                                                            ToolTip="Entity Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotal_Bill_Amount" runat="server" Text='<%# Eval("Total_Bill_Amount") %>'
                                                            ToolTip="Total Bill Amount" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                            ToolTip="Status" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucPOCustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Rental Schedule Details" ID="pnlRS" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvRS" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvRS_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblRS_ID" runat="server" Text='<%# Eval("RS_ID") %>' Visible="false" />
                                                        <asp:Label ID="lblCustomer_ID" runat="server" Text='<%# Eval("Customer_ID") %>' Visible="false" />

                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRS_Number" runat="server" Text='<%# Eval("RS_Number") %>' ToolTip="Account Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccount_Date" runat="server" Text='<%# Eval("Account_Date") %>'
                                                            ToolTip="Account Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Customer_Name") %>'
                                                            ToolTip="Customer Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucRSCustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="MRA Details" ID="pnlMRA" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvMRA" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvMRA_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMRASlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblMRA_ID" runat="server" Text='<%# Eval("MRA_ID") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MRA Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMRA_Number" runat="server" Text='<%# Eval("MRA_Number") %>' ToolTip="Account Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Date" runat="server" Text='<%# Eval("MRA_Date") %>'
                                                            ToolTip="Account Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Customer_Name") %>'
                                                            ToolTip="Customer Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucMRACustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Pricing Details" ID="pnlPricing" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvPricing" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvPricing_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPRCSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblPricing_ID" runat="server" Text='<%# Eval("Pricing_ID") %>' Visible="false" />
                                                        <asp:Label ID="lblCustomer_ID" runat="server" Text='<%# Eval("Customer_ID") %>' Visible="false" />

                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pricing Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPricing_Number" runat="server" Text='<%# Eval("Pricing_Number") %>' ToolTip="Pricing Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Offer Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOffer_Date" runat="server" Text='<%# Eval("Offer_Date") %>'
                                                            ToolTip="Offer Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Customer_Name") %>'
                                                            ToolTip="Customer Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucPricingCustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Deal Details" ID="pnlNOA" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvNOA" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvNOA_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNOASlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblDeal_ID" runat="server" Text='<%# Eval("Deal_ID") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deal Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeal_Number" runat="server" Text='<%# Eval("Deal_Number") %>' ToolTip="Deal Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Deal Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOffer_Date" runat="server" Text='<%# Eval("Deal_Date") %>'
                                                            ToolTip="Deal Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tranche Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTranche_Name" runat="server" Text='<%# Eval("Tranche_Name") %>'
                                                            ToolTip="Tranche Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Customer_Name") %>'
                                                            ToolTip="Customer Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucNOACustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Debit/Credit Details" ID="pnlDC" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvDC" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvDC_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDCSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblDC_ID" runat="server" Text='<%# Eval("DC_ID") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Debit/Credit Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDC_Number" runat="server" Text='<%# Eval("DC_Number") %>' ToolTip="Debit/Credit Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Debit/Credit Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDC_Date" runat="server" Text='<%# Eval("DC_Date") %>'
                                                            ToolTip="Debit/Credit Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Type" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type") %>'
                                                            ToolTip="Type" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Location" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDCLocation" runat="server" Text='<%# Eval("Location") %>'
                                                            ToolTip="Location" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lessee/Funder/Vendor Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Entity_Name") %>'
                                                            ToolTip="Lessee/Funder/Vendor Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Rental Schedule Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRS_No" runat="server" Text='<%# Eval("ACCOUNT_CODE") %>'
                                                            ToolTip="Rental Schedule Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucDCCustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Credit Note Details" ID="pnlCN" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvCN" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvCN_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCNSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblRS_ID" runat="server" Text='<%# Eval("PASA_Ref_id") %>' Visible="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCN_Number" runat="server" Text='<%# Eval("CN_Number") %>' ToolTip="Document Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCust_Name" ToolTip="Customer Name" runat="server" Text='<%#Bind("Customer_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Account Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRS_Number" ToolTip="Account Number" runat="server" Text='<%#Bind("PANum") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" HeaderText="Installment Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInstallment_No" ToolTip="Invoice Date" runat="server" Text='<%#Bind("instal_no") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Left" HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" ToolTip="Amount" runat="server" Text='<%#Bind("TotalVATAmount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoice_Date" ToolTip="Invoice Date" runat="server" Text='<%#Bind("Invoice_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucCNCustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Other CashFlow Details" ID="pnlOCP" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvOCP" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvOCP_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDCSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblCashflow" Visible="false" runat="server" Text='<%# Eval("cashflow_id") %>' />
                                                        <asp:Label ID="lblCashflowFlag" Visible="false" runat="server" Text='<%# Eval("CashFlow_Flag_ID") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Tranche Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTranche_Name" ToolTip="Tranche Name" runat="server" Text='<%#Bind("TRANCHE_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Cashflow">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCashflowDescription" ToolTip="Cashflow" runat="server" Text='<%#Bind("CashFlow_Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceDate" ToolTip="Invoice Date" runat="server" Text='<%#Bind("Invoice_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucOCPCustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Rental Invoice Details" ID="pnlRI" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvRI" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%"
                                            OnRowDataBound="gvRI_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRISlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblBillId" Visible="false" runat="server" Text='<%# Eval("Billing_ID") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocation" ToolTip="Location" runat="server" Text='<%#Bind("Location_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Bill Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBillNumber" ToolTip="Bill Number" runat="server" Text='<%#Bind("Billing_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Account Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccNumber" ToolTip="Account Number" runat="server" Text='<%#Bind("PANum") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucRICustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                    <asp:Panel GroupingText="Rental Invoice Details" ID="pnlII" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvII" runat="server" AutoGenerateColumns="False" DataKeyNames="SNo" Width="100%"
                                            OnRowDataBound="gvII_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIISlNo" runat="server" Text='<%# Eval("[SNo]") %>' />
                                                        <asp:Label ID="lblInterimId" Visible="false" runat="server" Text='<%# Eval("ID") %>' />
                                                        <asp:Label ID="lblDocumentPath" ToolTip="Account Number" Visible="false" runat="server" Text='<%#Bind("Document_path") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Document Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocNumber" ToolTip="Document Number" runat="server" Text='<%#Bind("[Document Number]") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer" ToolTip="Customer" runat="server" Text='<%#Bind("Customer") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Account Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccNumber" ToolTip="Account Number" runat="server" Text='<%#Bind("[Rent Sch No]") %>'></asp:Label>
                                                         
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Pre EMI Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPreEMIDate" ToolTip="Pre EMI Date" runat="server" Text='<%#Bind("[Pre EMI Date]") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucIICustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Panel GroupingText="Print Details" ID="pnlPrintDetails" runat="server" CssClass="stylePanel">
                            <table width="99%">
                                <tr>
                                    <td style="width: 10%" class="styleFieldLabel">
                                        <asp:Label ID="lblPrintType" runat="server" Text="Print Type" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="ddlPrintType" runat="server">
                                            <asp:ListItem Value="P" Text="PDF"></asp:ListItem>
                                            <asp:ListItem Value="W" Text="WORD"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print"
                            OnClick="btnPrint_Click" ValidationGroup="Print" Enabled="false" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsSearch" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Search" />
                        <asp:ValidationSummary ID="vsPrint" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Print" />
                    </td>
                </tr>


            </table>
        </ContentTemplate>
        <%--added for print--%>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

