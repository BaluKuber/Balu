<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="~/Reports/S3GRPTDPDReport.aspx.cs" Inherits="Reports_S3GRPTDPDReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Days Past Dues Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <%--  <asp:UpdatePanel ID="UpdPnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <asp:Panel ID="PnlInputCriteria" runat="server" CssClass="stylePanel" Width="100%"
                    GroupingText="Input Criteria">
                    <table width="100%">
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="20%">
                                <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" Width="200px">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVLOB" Enabled="true" CssClass="styleMandatoryLabel"
                                    runat="server" InitialValue="0" ControlToValidate="ddlLOB" ValidationGroup="vsDPD"
                                    ErrorMessage="Select a Line of Business" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldAlign" width="1%">
                                &nbsp;
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblregion" runat="server" Text="Location1"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlRegion" ToolTip="Region" Width="200px" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="DDRegion_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="RFVRG" Enabled="true" CssClass="styleMandatoryLabel"
                                    runat="server" InitialValue="0" ControlToValidate="ddlRegion" ValidationGroup="vsDPD"
                                    ErrorMessage="Select the Location1" Display="None">
                                </asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Location2" ID="lblBranch" AutoPostBack="True"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlBranch" runat="server" ToolTip="Branch" Width="200px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldAlign">
                                &nbsp;
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblcut" runat="server" CssClass="styleReqFieldLabel" Text="Demand Month"> </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="TxtCutOff" runat="server" ToolTip="Demand Month" ContentEditable="false"
                                    Style="text-align: right" Width="35%" OnTextChanged="TxtCutOff_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalCutOff" runat="server" PopupButtonID="imgMonthYear"
                                    BehaviorID="calendar1" Format="yyyyMM" TargetControlID="TxtCutOff" OnClientShown="onCalendarShown">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgMonthYear" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <asp:RequiredFieldValidator ID="RqvTxtCutOff" Enabled="true" runat="server" ControlToValidate="TxtCutOff"
                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Demand Month"
                                    ValidationGroup="vsDPD">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Denomination" ID="Lblden" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlDenomination" runat="server" ToolTip="Denomination" Width="200px"
                                    OnSelectedIndexChanged="ddlDenomination_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFdeno" Enabled="true" CssClass="styleMandatoryLabel"
                                    runat="server" ControlToValidate="ddlDenomination" InitialValue="-1" ValidationGroup="vsDPD"
                                    ErrorMessage="Select the Denomination" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldAlign">
                                &nbsp;
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Account Status" ID="LblAccountStatus" >
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlAccountStatus" runat="server" ToolTip="Account Status" 
                                    Width="200px">
                                </asp:DropDownList>
                                <%-- <asp:RequiredFieldValidator ID="RqvReport" Enabled="true" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="Rdreport" ValidationGroup="vsDPD" ErrorMessage="Select the Report Basis"
                                        Display="None">
                                </asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" colspan="2">
                                <asp:CheckBox ID="ChkSubRep" runat="server" Text="Include Subsequent Receipts" AutoPostBack="true"
                                    OnCheckedChanged="ChkSubRep_CheckedChanged"></asp:CheckBox>
                            </td>
                            <td class="styleFieldAlign">
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="LblRcptDate" runat="server" Text="Receipt Date"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="TxtRcptDate" runat="server" ContentEditable="true" Width="35%"
                                    AutoPostBack="true" ToolTip="Receipt Date" OnTextChanged="TxtRcptDate_TextChanged"></asp:TextBox>
                                <cc1:CalendarExtender runat="server" TargetControlID="TxtRcptDate" ID="CalendarExtenderSD_TxtRcptDate"
                                    Enabled="True" Format="dd/MM/yyyy" PopupButtonID="ImgRcpt">
                                </cc1:CalendarExtender>
                                <asp:Image ID="ImgRcpt" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <asp:RequiredFieldValidator ID="RqvTxtRcptDate" Enabled="true" CssClass="styleMandatoryLabel"
                                    runat="server" ControlToValidate="TxtRcptDate" ValidationGroup="vsDPD" ErrorMessage="Enter the Receipt Date"
                                    Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Panel ID="pnlDPDParamDetails" runat="server" CssClass="stylePanel" GroupingText="DPD Parameter Setup"
                                    Width="50%">
                                    <asp:GridView ID="GRVbucket" runat="server" AutoGenerateColumns="False" ToolTip="Days"
                                        Width="100%">
                                        <Columns>
                                            <%--buckets--%>
                                            <asp:TemplateField HeaderText="Buckets" ItemStyle-Width="30px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbucket" runat="server" Text='<%# Bind("Bucket")  %>' Width="30px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" />
                                            </asp:TemplateField>
                                            <%--From Days--%>
                                            <asp:TemplateField HeaderText="From(Days)" ItemStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFromDays" runat="server" MaxLength="3" Text='<%# Bind("Bucket_From")%>'
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="150px" />
                                            </asp:TemplateField>
                                            <%--To days--%>
                                            <asp:TemplateField HeaderText="To(Days)" ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblToDays" runat="server" MaxLength="3" Text='<%# Bind("Bucket_To")%>'
                                                        Width="150px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="90px" />
                                            </asp:TemplateField>
                                            <%--Select NO OF DAYS CHKBOX--%>
                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle Width="90px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldAlign" colspan="5" align="center">
                                <table>
                                    <tr class="styleButtonArea">
                                        <td align="center">
                                            <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('vsDPD',false);" OnClick="btnGo_Click"
                                                Text="Go" CausesValidation="true" ValidationGroup="vsDPD" />
                                            <asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                                Text="Reset" OnClick="btnReset_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="vsDPDRep" runat="server" CssClass="styleMandatoryLabel"
                                                CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="vsDPD" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CustomValidator ID="CVDPD" runat="server" Display="None" ValidationGroup="btnGo"></asp:CustomValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <%--   </ContentTemplate>
                </asp:UpdatePanel>--%>
                <asp:Panel ID="PnlAsstDPDReport" runat="server" CssClass="stylePanel" GroupingText="Asset Class Details">
                    <asp:GridView ID="GrvAsstDPDReport" runat="server" AutoGenerateColumns="False" ToolTip="DPD Report Details">
                        <Columns>
                            <asp:TemplateField HeaderText="Asset Class Types">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkAssetClass" runat="server" Text='<%# Eval("CATEGORY_DESCRIPTION") %>'
                                        CommandArgument='<%# Eval("ASSET_CATEGORY_ID") %>' OnClick="LnkAssetClass_Click"></asp:LinkButton>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <asp:LinkButton ID="LnkAssetClass" runat="server" Text='<%# Eval("CATEGORY_DESCRIPTION") %>'
                                        CommandArgument='<%# Eval("ASSET_CATEGORY_ID") %>' OnClick="LnkAssetClass_Click"></asp:LinkButton>
                                </AlternatingItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="PnlAccDPDReport" runat="server" CssClass="stylePanel" GroupingText="DPD Report Details"
                    HorizontalAlign="Center" ScrollBars="Auto">
                    <table width="100%" align="center">
                        <tr>
                            <td align="right">
                            <asp:Label runat="server" ID="LblDenomination" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GrvAccDPDReport" runat="server" ToolTip="DPD Report Details" AllowPaging="true"
                                    OnPageIndexChanging="GrvAccDPDReport_PageIndexChanging" PageSize="100" >
                                    <%-- <HeaderStyle CssClass="FrozenHeader" />--%>
                                    <%-- <RowStyle HorizontalAlign="Right" />--%>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnPrintAccDPDReport" runat="server" CssClass="styleSubmitButton"
                        OnClick="btnPrintAccDPDReport_Click" Text="Print" />
                    <%--<asp:Button ID="btnPrintExcel" runat="server"  Text="Export to Excel"
                        OnClick="btnPrintExcel_Click" Visible ="false" />--%>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--<input type="hidden" runat="server" id="HTxtMonths" name="HTxtMonths" />--%>

    <script type="text/javascript"> 
 
        var cal1; 
 
       function pageLoad() { 
            cal1 = $find("calendar1"); 
 
            modifyCalDelegates(cal1); 
        } 
 
        function modifyCalDelegates(cal) { 
            //we need to modify the original delegate of the month cell. 
            cal._cell$delegates = { 
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover), 
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout), 
 
                click: Function.createDelegate(cal, function(e) { 
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
