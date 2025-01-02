<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_RPT_CSTPurchaseDetailsReport.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="Reports_S3G_RPT_CSTPurchaseDetailsReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="ContentCSTPurchaseDetailsReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpCSTPurchaseDetailsReport" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="CST Purchase Details Report" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlCSTPurchaseDetailsReport" runat="server" GroupingText="Filter Criteria"
                            CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPostingDateFrom" runat="server" Text="From Posting Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <%--<input id="hidPostingDate" type="hidden" runat="server" />--%>
                                        <asp:TextBox ID="txtPostingDateFrom" runat="server" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgPostingDateFrom"
                                            TargetControlID="txtPostingDateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblPostingDateTo" runat="server" Text="To Posting Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtPostingDateTo" runat="server" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgPostingDateTo"
                                            TargetControlID="txtPostingDateTo">
                                        </cc1:CalendarExtender>
                                       <%-- <asp:CompareValidator ID="CVCFormPosting" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" runat="server" ControlToCompare="txtPostingDateFrom" ControlToValidate="txtPostingDateTo" ErrorMessage="Activation To date cannnot be lesser than Activation From date" ValidationGroup="Go"
                                            Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>--%>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceDateFrom" runat="server" Text="From Invoice Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <%--<input id="hidInvoiceDate" type="hidden" runat="server" />--%>
                                        <asp:TextBox ID="txtInvoiceDateFrom" runat="server" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgInvoiceDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderInvoiceDateFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgInvoiceDateFrom"
                                            TargetControlID="txtInvoiceDateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblInvoiceDateTo" runat="server" Text="To Invoice Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoiceDateTo" runat="server" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgInvoiceDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderInvoiceDateTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgInvoiceDateTo"
                                            TargetControlID="txtInvoiceDateTo">
                                        </cc1:CalendarExtender>
                                        <asp:CompareValidator ID="CVCFormInvoice" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" runat="server" ControlToCompare="txtInvoiceDateFrom" ControlToValidate="txtInvoiceDateTo" ErrorMessage="Billing Invoice To date cannnot be lesser than Billing Invoice From date" ValidationGroup="Go"
                                            Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblState" runat="server" Text="State" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <uc2:Suggest ID="ddlStateName" runat="server" ValidationGroup="rfvSearch" WatermarkText="--All--" IsMandatory="true" ServiceMethod="GetStateDetails"
                                            ErrorMessage="Enter State" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlCustName" runat="server" WatermarkText="--All--" ServiceMethod="GetLesseeName"/>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
                                 <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblVendor" runat="server" Text="Vendor" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                       <uc2:Suggest ID="ddlVendorName" runat="server" ServiceMethod="GetVendorName" WatermarkText="--All--"
                                            ErrorMessage="Enter Vendor Name" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlInvoiceNo" runat="server" WatermarkText="--All--" ServiceMethod="GetInvoiceNo"
                                            ErrorMessage="Enter Invoice Number" />
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="styleFieldLabel">
                                         <asp:Label ID="lblCForm" runat="server" Text="C Form" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <asp:DropDownList ID="ddlCForm" runat="server" Width="160px" >
                                            <asp:ListItem Value="0" Text="--All--" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Issued"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Not Issued"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                       <asp:Label ID="lblCFormNumber" runat="server" Text="C Form Number" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                       <uc2:Suggest ID="ddlCFormNumber" runat="server" WatermarkText="--All--" ServiceMethod="GetCFormNo" />
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" colspan="4" align="center">
                        <asp:Button ID="btnSearch" runat="server" Text="Go"  UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();" CssClass="styleSubmitButton" OnClick="btnclear_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Panel ID="PnlCSTPurchaseDetails" runat="server" Visible="false" GroupingText="CST Purchase Details"
                            CssClass="stylePanel">
                             <div class="container" style="height: 100%; width: 100%; overflow-x: scroll; overflow-y: auto;">
                            <asp:GridView ID="grvCSTPurchaseDetailsReport" runat="server" Width="100%" OnRowDataBound="grvCSTPurchaseDetailsReport_RowDataBound"  EmptyDataText="No Record Found"
                                AutoGenerateColumns="true" >
                                <HeaderStyle CssClass="styleGridHeader" ></HeaderStyle>
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
                        <asp:Button ID="btnPrint" runat="server" Text="Export" Visible="false" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnPrint_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                      <asp:ValidationSummary ID="VSCFormReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="ddlCustName" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
