<%@ Page Title="Amortisation Monthly Report" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Rpt_AmortMonthlyReport.aspx.cs"
    Inherits="Reports_S3G_Rpt_AmortMonthlyReport" %>

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
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="Amortisation Report - Monthly" CssClass="styleInfoLabel">
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
                                        <asp:Label ID="lblPostingDateFrom" runat="server" Text="RS Activated Start Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <input id="hidPostingDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtPostingDateFrom" runat="server" AutoPostBack="true" OnTextChanged="txtPostingDateFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateFrom" Format="dd/MM/yyyy" runat="server"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgPostingDateFrom"
                                            TargetControlID="txtPostingDateFrom">
                                        </cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator ID="rfvPostingDateFrom" runat="server" ErrorMessage="Select RS Activated Start Date" ValidationGroup="rfvSearch"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtPostingDateFrom"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPostingDateTo" runat="server" Text="RS Activated End Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtPostingDateTo" runat="server" AutoPostBack="true" OnTextChanged="txtPostingDateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateTo" runat="server" Format="dd/MM/yyyy"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgPostingDateTo"
                                            TargetControlID="txtPostingDateTo">
                                        </cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator ID="rfvPostingDateTo" runat="server" ErrorMessage="Select RS Activated End Date" ValidationGroup="rfvSearch"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtPostingDateTo"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStartMonth" runat="server" Text="Amort From Month/Year" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtStartMonth" runat="server" OnTextChanged="txtStartMonth_OnTextChanged" AutoPostBack="true" ToolTip="From Month/Year"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgStartMonth" BehaviorID="calendar1" Format="MMM-yyyy" TargetControlID="txtStartMonth" OnClientShown="onCalendarShown">
                                        </cc1:CalendarExtender>
                                        <asp:Image ID="imgStartMonth" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <%--<asp:RequiredFieldValidator ID="rfvStartMonth" runat="server" ErrorMessage="Select Amort From Month/Year" ValidationGroup="rfvSearch" Display="None" SetFocusOnError="True" ControlToValidate="txtStartMonth"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblEndMonth" runat="server" Text="Amort To Month/Year" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtEndMonth" runat="server" AutoPostBack="true" OnTextChanged="txtEndMonth_OnTextChanged" ToolTip="To Month/Year"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="calendar2" Format="MMM-yyyy" TargetControlID="txtEndMonth" PopupButtonID="imgEndMonth" OnClientShown="onCalendarShown">
                                        </cc1:CalendarExtender>
                                        <asp:Image ID="imgEndMonth" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <%--<asp:RequiredFieldValidator ID="rfvEndMonth" runat="server" ErrorMessage="Select Amort To Month/Year" ValidationGroup="rfvSearch" Display="None" SetFocusOnError="True" ControlToValidate="txtEndMonth"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
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
                                        <asp:DropDownList ID="ddlStatus" AutoPostBack="true" runat="server" ValidationGroup="rfvSearch" ErrorMessage="Select Status"
                                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" />
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
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
                               <%-- <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>--%>
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
                        <asp:Button ID="btnSearch" runat="server" Text="Search" UseSubmitBehavior="true" ValidationGroup="rfvSearch" CssClass="styleSubmitButton" OnClick="btnSearch_Click" ToolTip="Search" />
                        <asp:Button ID="btnExport" runat="server" Text="Export" UseSubmitBehavior="true" ValidationGroup="rfvSearch" CssClass="styleSubmitButton" OnClick="btnExport_Click" ToolTip="Export" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();" OnClick="btnclear_Click" ToolTip="Clear" />
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
                        <asp:ValidationSummary ID="vsTransLander" runat="server" ValidationGroup="rfvSearch"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                    </td>
                </tr>
<%--                <tr>
                    <td width="100%" colspan="4" align="center">&nbsp;
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="4">
                        <div id="divacc" runat="server">
                            <table width="100%">
                                <tr>
                                    <td id="tdExportDtl" runat="server" visible="false">
                                       
                                    </td>
                                </tr>
                            </table>
                        </div>
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
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlCustName" />
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">

        var cal1;
        var cal2;

        function pageLoad() {
            cal1 = $find("calendar1");
            cal2 = $find("calendar2");

            modifyCalDelegates(cal1);
            modifyCalDelegates(cal2);
        }

        function modifyCalDelegates(cal) {
            //we need to modify the original delegate of the month cell. 
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),

                click: Function.createDelegate(cal, function (e) {
                    /// <summary>  
                    /// Handles the click event of a cell 
                    /// </summary> 
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param> 

                    e.stopPropagation();
                    e.preventDefault();

                    if (!cal._enabled) return;

                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        case "month":
                            //if the mode is month, then stop switching to day mode. 
                            if (target.month == visibleDate.getMonth()) {
                                //this._switchMode("days"); 
                            } else {
                                cal._visibleDate = target.date;
                                //this._switchMode("days"); 
                            }
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                cal._switchMode("months");
                            }
                            break;

                            //                case "day":                             
                            //                    this.set_selectedDate(target.date);                             
                            //                    this._switchMonth(target.date);                             
                            //                    this._blur.post(true);                             
                            //                    this.raiseDateSelectionChanged();                             
                            //                    break;                             
                        case "today":
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }

                })
            }

        }

        function onCalendarShown(sender, args) {
            //set the default mode to month 
            sender._switchMode("months", true);
            changeCellHandlers(cal1);
        }


        function changeCellHandlers(cal) {

            if (cal._monthsBody) {

                //remove the old handler of each month body. 
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body. 
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }

            }
        }

        function onCalendarHidden(sender, args) {

            if (sender.get_selectedDate()) {
                if (cal1.get_selectedDate() && cal2.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) {
                    alert('Start Month/Year cannot be Greater than the End Month/Year, please reselect!');
                    sender.show();
                    return;
                }
                //get the final date 
                var finalDate = new Date(sender.get_selectedDate());
                var selectedMonth = finalDate.getMonth();
                finalDate.setDate(1);
                if (sender == cal2) {
                    // set the calender2's default date as the last day 
                    finalDate.setMonth(selectedMonth + 1);
                    finalDate = new Date(finalDate - 1);
                }
                //set the date to the TextBox 
                sender.get_element().value = finalDate.format(sender._format);
            }
        }
    </script>
</asp:Content>
