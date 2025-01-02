<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_RPT_PurchaseInvoiceDetailsReport.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    Inherits="Reports_S3G_RPT_PurchaseInvoiceDetailsReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="ContentPurchaseInvoiceDetailsReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpPurchaseInvoiceDetailsReport" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="Purchase Invoice Details Report" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlPurchaseInvoiceDetailsReport" runat="server" GroupingText="Filter Criteria"
                            CssClass="stylePanel" Width="100%">
                            <table width="100%" cellpadding="0" style="width: 100%" align="center">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlCustName" runat="server" ServiceMethod="GetCustomerName" WatermarkText="--All--"
                                            ErrorMessage="Enter Customer Name" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlVendorName" runat="server" ServiceMethod="GetVendorName" WatermarkText="--All--"
                                            ErrorMessage="Enter Vendor Name" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblScheduleState" runat="server" Text="Schedule State" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlScheduleState" runat="server" ServiceMethod="GetScheduleState" WatermarkText="--All--"
                                            ErrorMessage="Enter Schedule State" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRentalScheduleNo" runat="server" Text="Rental Schedule No." CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlRentalScheduleNo" runat="server" ServiceMethod="GetRentalScheduleNo" WatermarkText="--All--"
                                            ErrorMessage="Enter RentalSchedule No" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoicePostingDateFrom" runat="server" Text="RS Activation From Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <input id="hidInvoicePostingDateDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtInvoicePostingDateFrom" runat="server" AutoPostBack="false" OnTextChanged="txtInvoicePostingDateFrom_TextChanged" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgInvoicePostingDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderInvoicePostingDateFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgInvoicePostingDateFrom"
                                            TargetControlID="txtInvoicePostingDateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoicePostingDateTo" runat="server" Text="RS Activation To Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoicePostingDateTo" runat="server" AutoPostBack="false" OnTextChanged="txtInvoicePostingDateTo_TextChanged" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgInvoicePostingDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderInvoicePostingDateTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgInvoicePostingDateTo"
                                            TargetControlID="txtInvoicePostingDateTo">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRSSignedOnFromDate" runat="server" Text="RS Signed On From Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRSSignedOnFromDate" runat="server" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgRSSignedOnFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="ceRSSignedOnFromDate" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgRSSignedOnFromDate"
                                            TargetControlID="txtRSSignedOnFromDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRSSignedOnToDate" runat="server" Text="RS Signed On To Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRSSignedOnToDate" runat="server" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgRSSignedOnToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="ceRSSignedOnToDate" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgRSSignedOnToDate"
                                            TargetControlID="txtRSSignedOnToDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNoteNo" runat="server" Text="Note No." CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlNoteNo" runat="server" ServiceMethod="GetNoteNo" WatermarkText="--All--"
                                            ErrorMessage="Enter Note No." />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="160px">
                                            <asp:ListItem Value="0" Text="--All--" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Scheduled"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Not Scheduled"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" colspan="4" align="center">
                        <asp:Button ID="btnSearch" runat="server" Text="Go" UseSubmitBehavior="true" ValidationGroup="Go" ToolTip="Go" CssClass="styleSubmitButton" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();" ToolTip="Clear" OnClick="btnclear_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="PnPurchaseInvoiceDetails" runat="server" GroupingText="Purchase Invoice Details"
                            CssClass="stylePanel" Width="100%">                            
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1150px">
                                <table width="100%">
                                    <tr>
                                        <td >
                                            <asp:GridView ID="grvPurchaseInvoiceDetailsReport" runat="server"  EmptyDataText="No Record Found" AutoGenerateColumns="true"
                                                OnRowDataBound="grvPurchaseInvoiceDetailsReport_RowDataBound">
                                            </asp:GridView>
                                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                        </td>
                                    </tr>                                    
                                </table>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center"></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnPrint" runat="server" Text="Export" Visible="false" ToolTip="Export" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnPrint_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:ValidationSummary ID="VSMRAReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="tdHeight" runat="server"></td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="ddlCustName" />
            <asp:PostBackTrigger ControlID="ddlVendorName" />
            <asp:PostBackTrigger ControlID="ddlScheduleState" />
            <asp:PostBackTrigger ControlID="ddlRentalScheduleNo" />
            <asp:PostBackTrigger ControlID="ddlNoteNo" />
        </Triggers>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
        }
        function pageLoad(s, a) {
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
        }
        function showMenu(show) {
            if (show == 'T') {
                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - 260;
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }
        function Resize() {
            document.getElementById('<%=tdHeight.ClientID %>').style.height = 100;
        }
    </script>
</asp:Content>
