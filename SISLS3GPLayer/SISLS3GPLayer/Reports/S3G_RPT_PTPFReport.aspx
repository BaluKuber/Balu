<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_RPT_PTPFReport.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="Reports_S3G_RPT_PTPFReport"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="ContentPTPFReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpPTPFReport" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="PTPF Report" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlPTPFReport" runat="server" GroupingText="Filter Criteria"
                            CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDateFrom" runat="server" Text="From Date" CssClass="styleReqFieldLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <input id="hidDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtDateFrom" runat="server" Width="140px" AutoPostBack="false" OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="RFVDateFrom" runat="server" ControlToValidate="txtDateFrom" SetFocusOnError="True"
                                             ErrorMessage="Select the From Date" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender ID="CalendarExtenderDateFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateFrom"
                                            TargetControlID="txtDateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblDateTo" runat="server" Text="To Date" CssClass="styleReqFieldLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDateTo" runat="server" Width="140px" AutoPostBack="false" OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="FRVDateTo" runat="server" ErrorMessage="Select the To Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtDateTo"></asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender ID="CalendarExtenderDateTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateTo"
                                            TargetControlID="txtDateTo">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <uc2:Suggest ID="ddlCustName" runat="server" ServiceMethod="GetCustomerName" WatermarkText="--All--"
                                            ErrorMessage="Enter Customer Name" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblRentalScheduleNo" runat="server" Text="Rental Schedule No" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                         <uc2:Suggest ID="ddlRentalScheduleNo" runat="server" ServiceMethod="GetRentalScheduleNo" WatermarkText="--All--"
                                            ErrorMessage="Enter the Rental Schedule No" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNoteNo" runat="server" Text="Note Creation No" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                         <uc2:Suggest ID="ddlNoteNo" runat="server" ServiceMethod="GetNoteNo" WatermarkText="--All--"
                                            ErrorMessage="Enter the Note No" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 14%">&nbsp;
                                    </td>
                                    <td class="styleFieldAlign">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" colspan="4" align="center">
                        <asp:Button ID="btnSearch" runat="server" Text="Go" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnSearch_Click" ValidationGroup="Go"/>
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();" CssClass="styleSubmitButton" OnClick="btnclear_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                     

                        <asp:Panel ID="PnlPTPF" runat="server" Visible="false" GroupingText="PTPF Details" 
                            CssClass="stylePanel">
                            <div id="divAssetInsuranceStatus" style="overflow-x: scroll; overflow-y: auto; width: 1050px; height: 100%" runat="server">
                                <asp:GridView ID="grvPTPFReport" runat="server" EmptyDataText="No Record Found" OnRowDataBound="grvPTPFReport_RowDataBound"
                                 OnRowCreated="grvPTPFReport_RowCreated" RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" AutoGenerateColumns="True">
                                    <Columns>
                                        
                                    </Columns>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Button ID="btnDownload" runat="server" Text="Export" UseSubmitBehavior="true" Visible="false" CssClass="styleSubmitButton" OnClick="btnDownload_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="left">
                        
                        <asp:ValidationSummary ID="vsPTPFReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):"
                            Height="250px" Width="500px" ShowMessageBox="false" ValidationGroup="Go"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
            <asp:PostBackTrigger ControlID="ddlCustName" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

