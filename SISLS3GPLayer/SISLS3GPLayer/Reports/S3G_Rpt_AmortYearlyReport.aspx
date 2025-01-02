<%@ Page Title="Amortisation Yearly Report" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_Rpt_AmortYearlyReport.aspx.cs" Inherits="Reports_S3G_Rpt_AmortYearlyReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpCFormSalesReport" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="Amortisation Report – Yearly" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlAmrtMnthly" runat="server" GroupingText="Filter Criteria" CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPostingDateFrom" runat="server" Text="From Date" CssClass="styleReqFieldLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <input id="hidPostingDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtPostingDateFrom" runat="server" AutoPostBack="true" OnTextChanged="txtPostingDateFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateFrom" runat="server" Format="dd/MM/yyyy"
                                            PopupButtonID="imgPostingDateFrom"
                                            TargetControlID="txtPostingDateFrom">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvPostingDateFrom" runat="server" ErrorMessage="Select From Date." ValidationGroup="rfvSearch"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtPostingDateFrom"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPostingDateTo" runat="server" Text="To Date" CssClass="styleReqFieldLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtPostingDateTo" runat="server" AutoPostBack="true" OnTextChanged="txtPostingDateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateTo" runat="server" Format="dd/MM/yyyy"
                                            PopupButtonID="imgPostingDateTo"
                                            TargetControlID="txtPostingDateTo">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvPostingDateTo" runat="server" ErrorMessage="Select To Date." ValidationGroup="rfvSearch"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtPostingDateTo"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlCustName" runat="server" AutoPostBack="true" WatermarkText="--All--" ServiceMethod="GetCustomerName" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <asp:DropDownList ID="ddlStatus" runat="server" ValidationGroup="rfvSearch" ErrorMessage="Select Status"
                                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblFunder" runat="server" Text="Funder Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlFunder" runat="server" AutoPostBack="true" WatermarkText="--All--" ServiceMethod="GetFunderName" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblTrancheNo" runat="server" Text="Tranche Number" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlTrancheNo" runat="server" AutoPostBack="true" WatermarkText="--All--" ServiceMethod="GetTrancheNo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblTranchegrp" runat="server" Text="Tranche Wise grouping" CssClass="styleReqFieldLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlTranchegrp" runat="server" Width="160px">
                                            <asp:ListItem Value="0" Text="--Select--" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvTranchegrp" ValidationGroup="rfvSearch" InitialValue="0"
                                            CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ddlTranchegrp" ErrorMessage="Select Tranche Wise grouping"
                                            SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td width="100%" colspan="4" align="center">&nbsp;
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
                        <asp:Button ID="btnSearch" runat="server" Text="Search" UseSubmitBehavior="true" ValidationGroup="rfvSearch" CssClass="styleSubmitButton" OnClick="btnSearch_Click" ToolTip="Search" />
                        <asp:Button ID="btnExport" runat="server" Text="Export" UseSubmitBehavior="true" ValidationGroup="rfvSearch" CssClass="styleSubmitButton" OnClick="btnExport_Click" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();" OnClick="btnclear_Click" ToolTip="Clear" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" colspan="4" align="center">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <%--<div id="divacc" runat="server" style="height: 750px; overflow: scroll;">--%>
                        <table width="99%">
                            <tr>
                                <td id="tdExportDtl" runat="server"></td>
                                <uc1:PageNavigator ID="ucCustomPaging" Visible="false" runat="server"></uc1:PageNavigator>
                            </tr>
                        </table>
                        <%-- </div>--%>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" align="left" width="100%" runat="server" id="pagetable" visible="false">
                            <tr class="stylePagingControl">
                                <td>
                                    <asp:Label runat="server" CssClass="stylePagingFieldLabel" ID="Label2" Text="No of Records : "></asp:Label>
                                    <asp:Label runat="server" CssClass="stylePagingFieldLabel" ID="lblTotalRecords"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnFirst" runat="server" ToolTip="First"
                                        Enabled="true" ImageUrl="../Images/First.gif" CausesValidation="false" OnClick="btnFirst_Click" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnPrevious" runat="server" Enabled="true" ToolTip="Prev"
                                        ImageUrl="../Images/Prev.gif" CausesValidation="false" OnClick="btnPrevious_Click" />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="1" CssClass="stylePagingRecFieldLabel" ID="lblCurrentPage"></asp:Label><span
                                        class="stylePagingRecFieldLabel"> / </span>
                                    <asp:Label runat="server" Text="1" CssClass="stylePagingRecFieldLabel" ID="lblTotalPages"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnNext" runat="server" Enabled="true" ToolTip="Next"
                                        ImageUrl="../Images/Next.gif" CausesValidation="false" OnClick="btnNext_Click" />
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnLast" runat="server" Enabled="true" ToolTip="Last"
                                        ImageUrl="../Images/Last.gif" CausesValidation="false" OnClick="btnLast_Click" />
                                </td>
                                <td style="padding-left: 10px" align="right">
                                    <asp:TextBox runat="server" ID="txtGotoPage" ReadOnly="true" CssClass="stylePagingTextBox"
                                        MaxLength="11" Width="150px"></asp:TextBox>
                                </td>
                                <td style="padding-bottom: 3px" align="left">
                                    <asp:Label runat="server" Text="Page Number" CssClass="stylePagingFieldLabel" ID="lblPageNo"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ID="vsTransLander" runat="server" ValidationGroup="rfvSearch"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlCustName" />
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <%--   <asp:PostBackTrigger ControlID="tdExportDtl" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

