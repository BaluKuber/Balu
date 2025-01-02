<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdCompensation.aspx.cs" Inherits="LoanAdmin_S3GLoanAdCompensation"
    Title="Pre EMI Calculation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Bill Generation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="100%" valign="top">
                                    <asp:Panel ID="Panel1" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                        Width="99%">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLOB" runat="server"  AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                        Display="None" InitialValue="0" Enabled="true" ErrorMessage="Select a Line of Business"
                                                        ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFrequency" runat="server" CssClass="styleReqFieldLabel" Text="Frequency"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlFrequency" runat="server" onchange="AssignStartEndDate()">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="ddlFrequency"
                                                        Display="None" InitialValue="0" Enabled="true" ErrorMessage="Select a Frequency"
                                                        ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMonthYear" runat="server" CssClass="styleReqFieldLabel" Text="Month/Year"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMonthYear" runat="server" Width="70px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calMonthYear" Format="MMM-yyyy" TodaysDateFormat="MMM-yyyy"
                                                        OnClientDateSelectionChanged="AssignStartEndDate" OnClientShown="onShown" OnClientHidden="onHidden"
                                                        runat="server" DefaultView="Months" Enabled="True" TargetControlID="txtMonthYear"
                                                        PopupButtonID="imgMonthYear">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgMonthYear" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStartDate" runat="server" CssClass="styleReqFieldLabel" Text="Start Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtStartDate" runat="server" Width="75px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                        PopupButtonID="imgStartDate" Enabled="false">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false"/>
                                                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                                                        Display="None" Enabled="true" ErrorMessage="Select a Start Date" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="styleReqFieldLabel" Text="End Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="2">
                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="75px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calEndDate" runat="server" TargetControlID="txtEndDate"
                                                        PopupButtonID="imgEndDate" Enabled="false">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                                                        Display="None" Enabled="true" ErrorMessage="Select a End Date" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnGo1" runat="server" CssClass="styleSubmitShortButton" OnClick="btnGo_Click"
                                                        Text="Go" CausesValidation="true" ValidationGroup="Go" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="panSchedule" runat="server" GroupingText="Schedule Details" CssClass="stylePanel"
                                        Width="99%" Visible="false">
                                        <%--  <table cellpadding="1" cellspacing="1" border="0" width="100%">
                                                        <tr>
                                                            <td>--%>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td class="styleFieldLabel" width="35%" valign="top">
                                                    <asp:RadioButtonList ID="rbtnSchedule" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnSchedule_SelectedIndexChanged">
                                                        <asp:ListItem Text="Schedule Now" Value="1"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="Schedule At :" Value="0"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td class="styleFieldLabel" width="10%" valign="top">
                                                    <asp:Label ID="lblScheduleDate" runat="server" CssClass="styleReqFieldLabel" Text="Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="20%">
                                                    <asp:TextBox ID="txtScheduleDate" runat="server" Width="90px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="calScheduleDate" runat="server" Enabled="True" TargetControlID="txtScheduleDate"
                                                        PopupButtonID="imgScheduleDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgScheduleDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                </td>
                                                <td class="styleFieldLabel" width="10%">
                                                    <asp:Label ID="lblScheduleTime" runat="server" CssClass="styleReqFieldLabel" Text="Time (HH:MM AM)"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="25%">
                                                    <asp:TextBox ID="txtScheduleTime" runat="server" Width="100px"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="REVScheduleTime" ValidationGroup="Go" runat="server"
                                                        ErrorMessage="Schedule Time Should be HH:MM AM Fomat(12 Hours)" ControlToValidate="txtScheduleTime"
                                                        SetFocusOnError="True" ValidationExpression="^([0]?[1-9]|1[0-2])(:)[0-5][0-9]((:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm)$"></asp:RegularExpressionValidator>
                                                </td>
                                                <td class="styleFieldAlign" width="5%">
                                                    <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitShortButton" OnClick="btnGo_Click"
                                                        Text="Go" CausesValidation="true" ValidationGroup="Go" />
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- </td>
                                                        </tr>
                                                    </table>--%>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel runat="server" ID="pnlBranch" CssClass="stylePanel" GroupingText="Location Details"
                            Width="99%" Visible="false">
                            <br />
                            <div id="div1" style="overflow: auto; width: 100%; padding-left: 0%;" runat="server"
                                border="1">
                                <asp:GridView ID="gvBranchWise" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                    EmptyDataText="No Location Found for Pre EMI calculation !...." Width="100%" OnRowDataBound="gvBranchWise_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAllBranch" runat="server" onclick="javascript:fnSelectAll(this,'chkSelectBranch');" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelectBranch" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranchId" runat="server" Text='<%#Bind("Location_Id") %>'></asp:Label>
                                                <asp:Label ID="lblLastFinMonth" runat="server" Text='<%#Bind("LastFinMonth") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" />
                                        <asp:BoundField DataField="CC_Doc_Number" HeaderText="CC Document No." ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%"/>
                                        <asp:BoundField DataField="AccountCount" HeaderText="Number of Accounts" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%"/>
                                        <asp:BoundField DataField="CCAmount" HeaderText="Compensation Charge" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="20%"/>
                                        <%--<asp:BoundField DataField="JVReference" HeaderText="System JV Reference" />--%>
                                        <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Bind("Remarks") %>' MaxLength="100" Width="200px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </div>
                            
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" align="center">
                <tr>
                    <td colspan="5" align="center">
                        <br />
                                                                                 
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" Enabled="false" CausesValidation="true" CssClass="styleSubmitButton"
                            Text="Save" ValidationGroup="btnSave" OnClick="btnSave_OnClick" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            OnClientClick="return fnConfirmClear();" Text="Clear" OnClick="btnClear_OnClick" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Cancel" OnClick="btnCancel_Click" />
                        &nbsp;
                         <asp:Button runat="server" ID="btnPrint" Text="Generate PDF" CssClass="styleSubmitButton"
                            OnClick="btnPrint_Click" ToolTip="Print" />  
                        &nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                        <asp:CustomValidator ID="cvBilling" runat="server" CssClass="styleMandatoryLabel"
                            Display="None" ValidationGroup="Submit"></asp:CustomValidator>
                        <asp:ValidationSummary ID="vsBilling" runat="server" CssClass="styleMandatoryLabel"
                            ValidationGroup="Go" HeaderText="Please correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" src="../Scripts/jsBilling.js" type="text/javascript">
    </script>

    <script language="javascript" type="text/javascript">

        function AssignStartEndDate() {
            //debugger;
            var varFrequency = document.getElementById('ctl00_ContentPlaceHolder1_ddlFrequency');
            if (varFrequency.value == "4") {
                var varMonthYear = document.getElementById('ctl00_ContentPlaceHolder1_txtMonthYear').value;
                var varDateFormat = Date.parseInvariant(varMonthYear, "MMM-yyyy");

                var varGPSFormat = '<%=strDateFormat %>';
                var varStartDate = new Date(varDateFormat.getFullYear(), varDateFormat.getMonth(), 1);
                varStartDate = varStartDate.format(varGPSFormat);
                document.getElementById('ctl00_ContentPlaceHolder1_txtStartDate').value = varStartDate;
                var intLastDate = 28;
                if (varDateFormat.getMonth() == 0 || varDateFormat.getMonth() == 2 || varDateFormat.getMonth() == 4 || varDateFormat.getMonth() == 6 || varDateFormat.getMonth() == 7 || varDateFormat.getMonth() == 9 || varDateFormat.getMonth() == 11) {
                    intLastDate = 31;
                }
                else if (varDateFormat.getMonth() == 3 || varDateFormat.getMonth() == 5 || varDateFormat.getMonth() == 8 || varDateFormat.getMonth() == 10) {
                    intLastDate = 30;
                }
                else {
                    if (varDateFormat.getFullYear() % 4 == 0) {
                        intLastDate = 29;
                    }
                }
                var varEndDate = new Date(varDateFormat.getFullYear(), varDateFormat.getMonth(), intLastDate);
                varEndDate = varEndDate.format(varGPSFormat);
                document.getElementById('ctl00_ContentPlaceHolder1_txtEndDate').value = varEndDate;
            }
            else {

                document.getElementById('ctl00_ContentPlaceHolder1_txtStartDate').value = "";
                document.getElementById('ctl00_ContentPlaceHolder1_txtEndDate').value = "";
            }

        }

        function onShown() {
            var cal = $find("ctl00_ContentPlaceHolder1_calMonthYear");
            cal._switchMode("months", true);
            cal._today.innerText = "Current Month :" + cal._today.date.format("MMM-yyyy");
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild, "click", callMethod);
                    }
                }
            }
        }
        function onHidden() {
            var cal = $find("ctl00_ContentPlaceHolder1_calMonthYear");
            if (cal._monthsBody) {
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild, "click", callMethod);
                    }
                }
            }

        }

        function callMethod(eventElement) {
            var target = eventElement.target;
            var cal = $find("ctl00_ContentPlaceHolder1_calMonthYear");
            cal._visibleDate = target.date;
            cal.set_selectedDate(target.date);
            cal._switchMonth(target.date);
            cal._today.innerText = "Current Month :" + cal._today.date.format("MMM-yyyy");
            cal._blur.post(true);
            cal.raiseDateSelectionChanged();
        }

        ///Function for Select/Unselect All Branches
        function fnSelectAll(chkSelectAllBranch, chkSelectBranch) {
            var gvBranchWise = document.getElementById('ctl00_ContentPlaceHolder1_gvBranchWise');
            var TargetChildControl = chkSelectBranch;
            //Get all the control of the type INPUT in the base control.
            var Inputs = gvBranchWise.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = chkSelectAllBranch.checked;
        }

        function fnSelectBranch(chkSelectBranch, chkSelectAllBranch) {

            var gvBranchWise = document.getElementById('ctl00_ContentPlaceHolder1_gvBranchWise');
            var TargetChildControl = chkSelectAllBranch;
            var selectall = 0;
            var Inputs = gvBranchWise.getElementsByTagName("input");
            if (!chkSelectBranch.checked) {
                chkSelectAllBranch.checked = false;
            }
            else {
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'checkbox') {
                        if (Inputs[n].checked) {
                            selectall = selectall + 1;
                        }
                    }
                }
                if (selectall == gvBranchWise.rows.length - 1) {
                    chkSelectAllBranch.checked = true;
                }
            }


        }

    </script>

</asp:Content>

