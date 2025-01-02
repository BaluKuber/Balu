<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLOANADOperatingLeaseDepreciation_Add.aspx.cs" Inherits="LoanAdmin_S3GLOANADOperatingLeaseDepreciation_Add"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    t<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" runat="server" align="center" cellpadding="0" cellspacing="0"
                border="0">
                <tr>
                    <td valign="top" colspan="4">
                        <table width="100%" class="stylePageHeading" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="height: 19px">
                                    <asp:Label runat="server" ID="lblHeading" Text="Operating Lease Depreciation" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtLOB" runat="server" ContentEditable="False"></asp:TextBox>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" ID="lblBranch" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <%--   <asp:DropDownList ID="ddlBranch" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            runat="server" onmouseover="ddl_itemchanged(this);">
                        </asp:DropDownList>--%>
                        <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                            AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select a Location"
                            IsMandatory="true" ValidationGroup="btnSave" />
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblMonthYear" runat="server" CssClass="styleReqFieldLabel" Text="Month/Year"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtMonthYear" runat="server" Width="70px" OnTextChanged="txtMonthYear_TextChanged"
                            AutoPostBack="true"></asp:TextBox>
                        <cc1:CalendarExtender ID="calMonthYear" Format="MMM-yyyy" TodaysDateFormat="MMM-yyyy"
                            OnClientShown="onShown" OnClientHidden="onHidden" runat="server" DefaultView="Months"
                            Enabled="True" TargetControlID="txtMonthYear" PopupButtonID="imgMonthYear">
                        </cc1:CalendarExtender>
                        <asp:Image ID="imgMonthYear" runat="server" ImageUrl="~/Images/calendaer.gif" />
                    </td>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Last Calculated Date" ID="lblLastDate" Visible="false"
                            CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtLastDate" runat="server" ContentEditable="False" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Depreciation Method" ID="Label1" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtDepMethod" runat="server" ContentEditable="False"></asp:TextBox>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label runat="server" ID="lblCurrentDate" Text="Schduled Date" CssClass="styleReqFieldLabel"
                            Visible="false"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtCurrentDate" runat="server" AutoPostBack="true" CausesValidation="true"
                            OnTextChanged="txtCurrentDate_TextChanged" Visible="false"></asp:TextBox>
                        <%--<asp:Image ID="imgDate" runat="server" ImageUrl="~/Images/calendaer.gif" />--%>
                        <%-- <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtCurrentDate"
                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDate"
                            ID="CalendarExtender1" Enabled="false" >
                        </cc1:CalendarExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Sys JV Number" ID="LblJVN1" CssClass="styleDisplayLabel"
                            Visible="False"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtSysJVNumber" runat="server" ReadOnly="true" Visible="False"></asp:TextBox>
                    </td>
                    <td class="styleFieldLabel" width="23%">
                        <asp:Label runat="server" Text="Sys JV Date" ID="LblJVN2" CssClass="styleDisplayLabel"
                            Visible="False"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtSysJVDate" runat="server" ReadOnly="true" Visible="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlAssetDetails" runat="server" GroupingText="Asset Details" CssClass="stylePanel">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grvDepreciation" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvDepreciation_RowDataBound"
                                            RowStyle-HorizontalAlign="Center" Width="100%" ShowFooter="false" DataKeyNames="Location_Id">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Lease Asset Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetNo" runat="server" Text='<%#Eval("Lease_Asset_No")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("Location_Name")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetDesc" runat="server" Text='<%#Eval("Asset_Description")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acquisition Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAcqDate" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Acquisition_Date")).ToString(strDateFormat) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Depreciation %" ControlStyle-CssClass="GridNumeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDepPercent" runat="server" Text='<%#Eval("Book_Depreciation_Rate")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValue" runat="server" Text='<%#Eval("Asset_Value")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Existing Depreciation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExistPercent" runat="server" Text='<%#Eval("Depreciation_Existing")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Depreciation Days" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastCalculated" runat="server" Text='<%#Eval("Depreciation_Days")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Current Depreciation">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrentPercent" runat="server" Text='<%#Eval("Depreciation_Current")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Current value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrentValue" runat="server" Text='<%#Eval("Current_Value")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel UpdateMode="Conditional" ID="UpdButtons" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:Button ID="btnSave" Text="Schdule Now" runat="server" CssClass="styleSubmitButton"
                                        OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators();" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnCalculate" runat="server" Text="Calculate" CausesValidation="True"
                                        CssClass="styleSubmitButton" ValidationGroup="btnSave" Visible="false" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnXL" runat="server" Text="XL Porting" CausesValidation="False"
                                        CssClass="styleSubmitButton" OnClick="btnXL_Click" Visible="false" Enabled="false" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnErrorLog" runat="server" Text="Error Log" CausesValidation="False"
                                        CssClass="styleSubmitButton" OnClick="btnErrorLog_Click" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnDelete" runat="server" Text="Revoke" CausesValidation="False"
                                        CssClass="styleSubmitButton" OnClientClick="return confirm('Do you want to revoke this depreciation entry?');"
                                        OnClick="btnDelete_Click" />
                                    <%--     <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" 
                            TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>--%>
                                </td>
                                <td align="left">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblErrorMsg" runat="server" Text="" Visible="false"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtCurrentDate"
                            Display="None" ValidationGroup="btnSave" ErrorMessage="Select the Current calculation date"
                            CssClass="styleSummaryStyle"></asp:RequiredFieldValidator>
                        <br />
                        <%--  <asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" ControlToValidate="ddlBranch"
                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a Location" CssClass="styleMandatoryLabel"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <div id="div1" style="height: 100%; width: 800px">
                            <asp:ValidationSummary ID="vsDepreciation" runat="server" HeaderText="Correct the following validation(s):"
                                CssClass="styleMandatoryLabel" Width="879px" ValidationGroup="btnSave" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnLOBID" runat="server" />
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXL" />
        </Triggers>
    </asp:UpdatePanel>

    <script language="javascript">
        function onCalendarShown(sender, args) {

            sender._switchMode("months", true);
            changeCellHandlers(calendar1);
            //            
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


        function changeCellHandlers(calendar1) {
            //          if (mode == "C")
            //           {
            if (cal._monthsBody) {

                //remove the old handler of each month body. 
                for (var i = 0; i < calendar1._monthsBody.rows.length; i++) {
                    var row = calendar1._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, calendar1._cell$delegates);
                    }
                }
                //add the new handler of each month body. 
                for (var i = 0; i < calendar1._monthsBody.rows.length; i++) {
                    var row = calendar1._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, calendar1._cell$delegates);
                    }
                }

            }
            //           } 
        }


        //       } 
    </script>

</asp:Content>
