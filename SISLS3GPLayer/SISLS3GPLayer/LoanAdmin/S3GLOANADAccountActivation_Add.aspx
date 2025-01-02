<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLOANADAccountActivation_Add.aspx.cs"
    EnableEventValidation="false" Inherits="S3GLOANADAccountActivation_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function window.onerror() {
            return true;
        }

        function Trim(strInput) {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function fnGetCompensationValue(CompensationValue) {
            var exp = /_/gi;
            return parseFloat(CompensationValue.value.replace(exp, '0'));
        }

        function fnRevokeValidators() {
            if (confirm('Do you want to Revoke?')) {
                return true;
            }
            else {
                return false;
            }
        }


        function checkRSActivationDate(sender, args) {
            var varApplicationDate = document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtAccountCreationDate').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var selectedDate = sender._selectedDate;
            var vartoday = new Date();
            var vartodayformat = vartoday.format(sender._format);
            var intValid = 0;
            if (selectedDate < varapplndate) {
                alert('RS Activation Date should be greater than or equal to RS Creation Date');
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartodayformat;
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartoday.format(sender._format);
            }
            else if (selectedDate > vartoday)
            {
                alert('RS Activation Date should be less than or equal to Current Date');
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartodayformat;
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartoday.format(sender._format);
            }
            else 
            {
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = selectedDate.format(sender._format);
                intValid = 1;
            }
        }

        function fnSetDivWidth() {
            if (document.getElementById('divMenu').style.display == 'none') {
                document.getElementById("ctl00_ContentPlaceHolder1_tcAccountActivation_tpSystemJournal_divGrid1").style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_tcAccountActivation_tpSystemJournal_divGrid1").style.width = screen.width - 260;
            }
        }

        function finDisableTab(tabIndex1, tabIndex2, tabIndex3, tabIndex4) {
            var tab = $find("<%=tcAccountActivation.ClientID%>");
            if (tab != null) {
                tab.get_tabs()[1].set_enabled(tabIndex1);
                tab.get_tabs()[2].set_enabled(tabIndex2);
                tab.get_tabs()[3].set_enabled(tabIndex3);
                tab.get_tabs()[4].set_enabled(tabIndex4);
            }

            document.getElementById("<%=btnXLPorting.ClientID%>").disabled = !tabIndex1;
            document.getElementById("<%=btnExportJV.ClientID%>").disabled = !tabIndex2;
        }

        function fnClearAllTab(fromBtn) {
          
            if (fromBtn) {
                if (confirm('Do you want to clear?')) {
                    var tab = $find("<%=tcAccountActivation.ClientID%>");
                    if (tab != null) {
                        tab.get_tabs()[1].set_enabled(false);
                        tab.get_tabs()[2].set_enabled(false);
                        tab.get_tabs()[3].set_enabled(false);
                        tab.get_tabs()[4].set_enabled(false);
                    }

                    document.getElementById("<%=btnXLPorting.ClientID%>").disabled = true;
                    document.getElementById("<%=btnExportJV.ClientID%>").disabled = true;

                    return true;
                }
                else
                    return false;
            }
            else {
                var tab = $find("<%=tcAccountActivation.ClientID%>");
                if (tab != null) {
                    tab.get_tabs()[1].set_enabled(false);
                    tab.get_tabs()[2].set_enabled(false);
                    tab.get_tabs()[3].set_enabled(false);
                    tab.get_tabs()[4].set_enabled(false);
                }

                document.getElementById("<%=btnXLPorting.ClientID%>").disabled = true;
                document.getElementById("<%=btnExportJV.ClientID%>").disabled = true;
            }
        }
        
    </script>

    <table id="Table1" width="100%" align="center" cellpadding="0" cellspacing="0" runat="server">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="RS Activation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td id="Td1" style="padding-top: 10px" runat="server">
                <asp:UpdatePanel ID="upAddress" runat="server">
                    <ContentTemplate>
                        <cc1:TabContainer ID="tcAccountActivation" runat="server" CssClass="styleTabPanel"
                            AutoPostBack="true" Width="100%" ScrollBars="None" TabStripPlacement="top">
                            <cc1:TabPanel ID="tpMainPage" runat="server" CssClass="tabpan" HeaderText="Main Page"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Main Page
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td colspan="4">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblActivationType" Visible="false" runat="server" Text="Select Activation Type"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:RadioButtonList ID="rbtnType" runat="server" Visible="false" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="rbtnType_SelectedIndexChanged" ToolTip="Activation Type">
                                                                        <asp:ListItem Text="Prime Account Number Activation" Value="0" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="Sub Account Number Activation" Value="1"></asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" valign="top">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div style="height: 1px;">
                                                                       <%-- <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="None"
                                                                            ControlToValidate="ddlLOB" ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a Line of Business"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None"
                                                                            ControlToValidate="ddlLOB" ValidationGroup="btnView" InitialValue="0" ErrorMessage="Select a Line of Business"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>--%>
                                                                        <br />
                                                                        <%--    <asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" ControlToValidate="ddlBranch"
                                                                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a Location" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                                                            ControlToValidate="ddlBranch" ValidationGroup="btnView" InitialValue="0" ErrorMessage="Select a Location"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>--%>
                                                                        <br />
                                                                        <asp:RequiredFieldValidator ID="rfvMLA" runat="server" Display="None" ControlToValidate="ddlMLA"
                                                                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a Rental Schedule Number"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="None"
                                                                            ControlToValidate="ddlMLA" ValidationGroup="btnView" InitialValue="0" ErrorMessage="Select a Rental Schedule Number"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="None"
                                                                            ControlToValidate="ddlMLA" ValidationGroup="btnView" ErrorMessage="Select a Rental Schedule Number"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                        <br />
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="None"
                                                                            ControlToValidate="ddlMLA" ValidationGroup="btnSave" ErrorMessage="Select a Rental Schedule Number"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                       <%-- <asp:RequiredFieldValidator ID="rfvSubAc" runat="server" Display="None" ControlToValidate="ddlSLA"
                                                                            Enabled="false" ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a Sub Account Number"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvSubAc1" runat="server" Display="None" ControlToValidate="ddlSLA"
                                                                            Enabled="false" ValidationGroup="btnSave" ErrorMessage="Select a Sub Account No"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>--%>
                                                                        <asp:RequiredFieldValidator ID="rfvActivationDate" runat="server" Display="None"
                                                                            ControlToValidate="txtActivationDate" ValidationGroup="btnSave" ErrorMessage="Enter Activation Date"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvBusineesIRR" runat="server" Display="None"
                                                                            ControlToValidate="txtBusinessIRR" ValidationGroup="btnSave" ErrorMessage="Recalculate IRR"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True">
                                                                        </asp:RequiredFieldValidator>
                                                                        <asp:CustomValidator ID="custRouterLogic" runat="server" ValidationGroup="btnSave"
                                                                            OnServerValidate="custRouterLogic_ServerValidate" Display="None">
                                                                        </asp:CustomValidator>
                                                                        <br />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="21%" class="styleFieldLabel">
                                                        <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="29%">
                                                        <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" ToolTip="Line of Business"
                                                            OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" onchange="fnClearAllTab(false);">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td width="19%" class="styleFieldLabel">
                                                        <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                            ErrorMessage="Select a Location" IsMandatory="true" ValidationGroup="btnSave"                                                             
                                                            OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                                                        <%-- <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" ToolTip="Location"
                                                            OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" onchange="fnClearAllTab(false);">
                                                        </asp:DropDownList>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMLA" runat="server" Text="Rental Schedule Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlMLA" runat="server" AutoPostBack="true" ToolTip="Rental Schedule Number"
                                                            OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged" onchange="fnClearAllTab(false);">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblSLA" Visible="false" runat="server" Text="Sub Account Number"></asp:Label>
                                                        <asp:Label ID="lblLeaseType" runat="server" Text="Lease Type"></asp:Label>

                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:DropDownList ID="ddlSLA"  Visible="false" runat="server" AutoPostBack="true" ToolTip="Sub Account Number"
                                                            OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged" Enabled="false" onchange="fnClearAllTab(false);">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblLeaseType1" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblActivationDate" runat="server" Text="Activation Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtActivationDate" runat="server" ToolTip="Activation Date" AutoPostBack="true"
                                                            Width="50%" OnTextChanged="txtActivationDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="imgDateofActivation" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            ToolTip="Activation Date" />
                                                        <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtActivationDate"
                                                            OnClientDateSelectionChanged="checkRSActivationDate" PopupButtonID="imgDateofActivation"
                                                            ID="CalendarExtender2" Enabled="True">
                                                        </cc1:CalendarExtender>
                                                    </td>

                                                      <td width="19%" class="styleFieldLabel">
                                                        <asp:Label ID="lblInterim" runat="server" Text="Interim Calculation" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                 
                                                         <asp:DropDownList ID="ddlInterim" runat="server" AutoPostBack="true" ToolTip="Interim Billing" OnSelectedIndexChanged="ddlInterim_SelectedIndexChanged" 
                                                             >
                                                        </asp:DropDownList>
                                                         <asp:RequiredFieldValidator ID="RfvInterim" runat="server" Display="None"
                                                                            ControlToValidate="ddlInterim" ValidationGroup="btnSave" ErrorMessage="Select Interim calculation"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True" InitialValue="0" Enabled="false" Visible="false">
                                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblInterimrate" runat="server" Text="Interim Rate" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                      <td class="styleFieldLabel">
                                                        <asp:TextBox ID="txtinterimrate" runat="server" ReadOnly="true" Style="text-align: right;" MaxLength="3"  ></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftxtinterimrate" runat="server" FilterType="Custom,Numbers"
                                                                                                    TargetControlID="txtinterimrate" ValidChars=".">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvInterimrate" runat="server" Display="None"
                                                                            ControlToValidate="txtinterimrate" ValidationGroup="btnSave" ErrorMessage="Enter Interim Rate"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True" InitialValue="" Enabled="false" Visible="false">
                                                                        </asp:RequiredFieldValidator>
                                                          </td>

                                                     <td class="styleFieldLabel">
                                                        <asp:Label ID="lblInterimamt" runat="server" Text="Interim Amount"   CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                      <td class="styleFieldLabel">
                                                        <asp:TextBox ID="txtInterimamount" runat="server" ReadOnly="true" Style="text-align: right;" MaxLength="9" ></asp:TextBox>
                                                     <cc1:FilteredTextBoxExtender ID="ftxtinterimamount" runat="server" Enabled="false" FilterType="Numbers"
                                                                                                    TargetControlID="txtInterimamount">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvInterimamt" runat="server" Display="None"
                                                                            ControlToValidate="txtInterimamount" ValidationGroup="btnSave" ErrorMessage="Enter Interim Amount"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True" InitialValue="" Enabled="false" Visible="false">
                                                                        </asp:RequiredFieldValidator>
                                                          </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblTaxType" runat="server" Text="Tax Type" CssClass="styleDisplayLabel" Visible="false">
                                                        </asp:Label>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:DropDownList ID="ddlTaxType" Width="60%" AutoPostBack="true" runat="server" 
                                                             OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged" Visible="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%" colspan="4">
                                                        <table width="99%">
                                                            <tr>
                                                                <td width="50%" valign="top">
                                                                    <asp:Panel Width="99%" ID="Panel3" runat="server" GroupingText="Account Information"
                                                                        CssClass="stylePanel">
                                                                        <table width="100%" cellspacing="0">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblAccountDate" runat="server" Text="RS Creation Date"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign" width="60%">
                                                                                    <asp:TextBox ID="txtAccountCreationDate" runat="server" ToolTip="RS Creation Date"
                                                                                        Width="50%"></asp:TextBox>
                                                                                    <asp:Image ID="ImgAccountCreationDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                                                        ToolTip="Creation Date" Visible="false"/>
                                                                                    <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtAccountCreationDate"
                                                                                        PopupButtonID="ImgAccountCreationDate"
                                                                                        ID="CEAccountCreationDate" Enabled="false">
                                                                                    </cc1:CalendarExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="Label2" runat="server" Text="Status"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtStatus" runat="server" ReadOnly="true" ToolTip="Status" Width="90%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="Label5" runat="server" Text="View Rental Schedule"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <a href="#" runat="server" id="anchAcct" onserverclick="anchAcct_serverclick" validationgroup="btnView">
                                                                                        <asp:Button CssClass="styleGridShortButton" ID="lblView" runat="server" Text="View"
                                                                                            ValidationGroup="btnView" ToolTip="Rental Schedule View" OnClick="anchAcct_serverclick"></asp:Button></a>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblFinAount" runat="server" Text="Facility Amount"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtFinanceAmount" runat="server" ReadOnly="true" ToolTip="Facility Amount"
                                                                                        Style="text-align: right" Width="40%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblAccountingIRR" runat="server" Text="Accounting IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtAccIRR" runat="server" ReadOnly="true" ToolTip="Accounting IRR"
                                                                                        Style="text-align: right" Width="40%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblBusinessIRR" runat="server" Text="Business IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtBusinessIRR" runat="server" ReadOnly="true" ToolTip="Business IRR"
                                                                                        Style="text-align: right" Width="40%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblCompanyIRR" runat="server" Text="Company IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtCompanyIRR" runat="server" ReadOnly="true" ToolTip="Company IRR"
                                                                                        Style="text-align: right" Width="40%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel" height="22px" align="right">
                                                                                    <asp:Button ID="btnCalIRR" Text="Calculate IRR" Visible="false" CssClass="styleSubmitButton" runat="server" OnClick="btnCalIRR_Click" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td width="50%">
                                                                    <asp:Panel Width="100%" ID="Panel11" runat="server" GroupingText="Customer Information"
                                                                        CssClass="stylePanel">
                                                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                            SecondColumnStyle="styleFieldAlign" />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tpAmortizationSchedule" runat="server" CssClass="tabpan" Width="100%"
                                HeaderText="Amortization Schedule">
                                <ContentTemplate>
                                    <table id="Table2" width="99%" runat="server">
                                        <tr id="Tr1" runat="server">
                                            <td id="Td2" runat="server">
                                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel runat="server" ID="Panel2" CssClass="stylePanel" GroupingText="Cash Flow Details">
                                                            <table width="100%">
                                                                <tr visible="false" class="styleRecordCount" runat="server" id="trAmortizationMessage">
                                                                    <td colspan="2" align="center">
                                                                        <asp:Label runat="server" ID="Label4" Text="No Records Found" Font-Size="Medium"
                                                                            class="styleMandatoryLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:GridView ID="grvAmortization" runat="server" AutoGenerateColumns="False" Width="90%"
                                                                            OnRowDataBound="grvAmortization_RowDataBound" ShowFooter="true">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Date">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDate" Text='<%#Eval("Date")%>' runat="server" ToolTip="Date"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Cash Flow">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCashFlow" Text='<%#Eval("Cash Flow")%>' runat="server" ToolTip="Cash Flow"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <span>Total</span>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Installment">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblInstallment" Text='<%#Eval("Installment")%>' runat="server" ToolTip="Installment"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalInstallment" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Balance Payable">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblBalance" Text='<%#Eval("Balance Payable")%>' runat="server" ToolTip="Balance Payable"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Principal Portion">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPrincipal" Text='<%#Eval("Principal Portion")%>' ToolTip="Principal Portion"
                                                                                            runat="server"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalPrincipal" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Interest Portion">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblInterest" Text='<%#Eval("Interest Portion")%>' runat="server" ToolTip="Interest Portion"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalInterest" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Insurance">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblInsurance" Text='<%#Eval("Insurance")%>' runat="server" ToolTip="Insurance"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalInsurance" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Others">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOthers" Text='<%#Eval("Others")%>' runat="server" ToolTip="Others"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalOthers" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Tax">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTax" MaxLength="20" Text='<%#Eval("Rental_Tax")%>' runat="server" ToolTip="Tax"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalTax" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                            
                                                                                  <asp:TemplateField HeaderText="Service Tax">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblST" MaxLength="20" Text='<%#Eval("ST")%>' runat="server" ToolTip="Tax"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotalST" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="AMF">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAMF" MaxLength="20" Text='<%#Eval("AMF")%>' runat="server" ToolTip="Tax"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotAMF" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                 <asp:TemplateField HeaderText="AMF Principal">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAMFPrincipal" MaxLength="20" Text='<%#Eval("AMF_Principal")%>' runat="server" ToolTip="Tax"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotAMFPrincipal" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                 <asp:TemplateField HeaderText="AMF Interest">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAMFInterest" MaxLength="20" Text='<%#Eval("AMF_Interest")%>' runat="server" ToolTip="Tax"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblTotAMFInterst" runat="server" Style="text-align: right;"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <%--   <asp:TemplateField HeaderText="TAXSetoff">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTAXSetoff" MaxLength="20" Text='<%#Eval("TAXSetoff")%>' runat="server"
                                                                                                ToolTip="TAXSetoff"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>--%>
                                                                            </Columns>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <RowStyle HorizontalAlign="Center" />
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
                                    <table width="98%">
                                        <tr align="right">
                                            <td>
                                                <%--<asp:Button runat="server" ID="btnXLPorting" Text="Export to Excel" CssClass="styleSubmitButton"
                                            OnClick="btnXLPorting_Click" ToolTip="Export to Excel" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tpSystemJournal" runat="server" CssClass="tabpan" Width="100%"
                                HeaderText="System Journal">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0" onresize="fnSetDivWidth()">
                                                <tr>
                                                    <td colspan="5" valign="top">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div style="height: 1px;">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                        <asp:Panel runat="server" ID="Panel4" CssClass="stylePanel" GroupingText="System Journal Details">
                                                            <div id="divGrid1" runat="server" style="overflow: scroll; width: 750px">
                                                                <asp:GridView ID="gvSystemJournal" runat="server" AutoGenerateColumns="False" Width="1850px"
                                                                    OnRowDataBound="gvSystemJournal_RowDataBound" Style="overflow: scroll">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Company Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCompanyCode" runat="server" Text='<%# Bind("CompanyCode")%>' ToolTip="Company Code"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Line of Business">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB")%>' ToolTip="Line of Business"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Location">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Location")%>' ToolTip="Location"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Dimension 1">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDim1" runat="server" Text='<%# Bind("Dimension1")%>' ToolTip="Dimension 1"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Dimension 2" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDim2" runat="server" Text='<%# Bind("Dimension2")%>' ToolTip="Dimension 2"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Rental Schedule Number">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PrimeAccountNumber")%>' ToolTip="Rental Schedule Number"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Sub Account Number" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SubAccountNumber")%>' ToolTip="Sub Account Number"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="System JV No.">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSysJV" runat="server" Text='<%# Bind("SystemJVNumber")%>' ToolTip="System JV No."></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Document Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDocDate" runat="server" Text='<%# Bind("DocumentDate")%>' ToolTip="Document Date"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Value Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblValueDate" runat="server" Text='<%# Bind("ValueDate")%>' ToolTip="Value Date"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <%--Changed By Thangam M on 22/Nov/2011 Based on UAT--%>
                                                                        <asp:TemplateField HeaderText="GL A/C">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGLAC" runat="server" Text='<%# Bind("GLAccount")%>' ToolTip="GL A/C"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="SL A/C">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSLAC" runat="server" Text='<%# Bind("SLAccount")%>' ToolTip="SL A/C"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Debit A/C">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDebitAcc" runat="server" Text='<%# Bind("DebitAC")%>' ToolTip="Debit A/C"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Credit A/C">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCreditAcc" runat="server" Text='<%# Bind("CreditAC")%>' ToolTip="Credit A/C"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount")%>' ToolTip="Amount"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Remarks">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks")%>' ToolTip="Remarks"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Status">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status")%>' ToolTip="Status"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <table width="100%">
                                        <tr align="right">
                                            <td>
                                                <%--<asp:Button runat="server" ID="btnExportJV" Text="Export to Excel" CssClass="styleSubmitButton"
                                            ToolTip="Export to Excel" OnClick="btnExportJV_Click" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tpPreDisbursementDocument" runat="server" CssClass="tabpan" Width="100%"
                                HeaderText="Pre Disbursement Document">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr visible="false" class="styleRecordCount" runat="server" id="trPreMessage">
                                                    <td colspan="2" align="center">
                                                        <asp:Label runat="server" ID="lblMessage" Text="No Records Found" Font-Size="Medium"
                                                            class="styleMandatoryLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlProforma" runat="server" GroupingText="Proforma" CssClass="stylePanel">
                                                            <asp:GridView ID="grvFile" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                                Width="100%" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" OnRowDataBound="grvFile_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" Visible="false" runat="server" Text='<%#Eval("Proforma_ID")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="View">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="BtnView" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                                runat="server" ToolTip="View" />
                                                                            <%-- <asp:LinkButton ID="BtnView" runat="server" Text="View" 
                                                                    ToolTip="View"></asp:LinkButton>--%>
                                                                            <%--<asp:Button ID="BtnView" runat="server" Text="View" CssClass="styleGridShortButton"
                                                                            ToolTip="View"></asp:Button>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Sl.No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblslNo" runat="server" ToolTip="Sl.No."></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Proforma Number" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProformaNo" runat="server" Text='<%#Eval("Proforma_No") %>' ToolTip="Proforma Number"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("Vendor_Name") %>' ToolTip="Vendor Name"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlPreDisb" runat="server" GroupingText="Pre Disbursement Document"
                                                            CssClass="stylePanel">
                                                            <asp:GridView ID="gvPRDDT" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                DataKeyNames="PRDDC_Doc_Cat_ID" CssClass="styleInfoLabel" OnRowDataBound="gvPRDDT_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="PRDDC TypeId" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPRTID" runat="server" Text='<%# Bind("PRDDC_Doc_Cat_ID") %>'></asp:Label>
                                                                            <asp:Label ID="lblCanView" runat="server" Visible="false" Text='<%# Eval("CanView")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PRDDC Type" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblType" runat="server" Text='<%# Bind("PRDDC_Doc_Type") %>' ToolTip="PRDDC Type"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle HorizontalAlign="left" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PRDDC Description" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("PRDDC_Doc_Description") %>'
                                                                                ToolTip="PRDDC Description"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Collected By" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtColletedBy" runat="server" Text='<%# Bind("CollectedBy") %>' ToolTip="Collected By"></asp:Label>
                                                                            <asp:Label ID="lblColUser" Visible="false" runat="server" Text='<%# Bind("Collected_By") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Collected Date" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtColletedDate" ToolTip="Collected Date" runat="server" Width="70px"
                                                                                Text='<%# Bind("Collected_Date") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Scanned By" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtScannedBy" runat="server" ToolTip="Scanned By" Text='<%# Bind("Scandedby") %>'></asp:Label>
                                                                            <asp:Label ID="lblScanUser" runat="server" Visible="false" Text='<%# Bind("Scanned_By") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Scanned Date" ItemStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="txtScannedDate" ToolTip="Scanned Date" runat="server" Width="70px"
                                                                                Text='<%# Bind("Scanned_Date") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="View Document">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="hyplnkViewPre" CommandArgument='<%# Bind("Scanned_Ref_No") %>'
                                                                                ToolTip="View Document" OnClick="hyplnkViewPre_Click" ImageUrl="~/Images/spacer.gif"
                                                                                CssClass="styleGridQuery" runat="server" />
                                                                            <asp:Label runat="server" ID="lblPath" Text='<%# Eval("Scanned_Ref_No")%>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" Width="120px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                                                MaxLength="100" Text='<%# Eval("Remarks")%>' ToolTip="Remarks"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tpPostDisbursementDocument" runat="server" CssClass="tabpan" Width="100%"
                                HeaderText="Post Disbursement Document">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr visible="false" class="styleRecordCount" runat="server" id="trPostMessage">
                                                    <td colspan="2" align="center">
                                                        <asp:Label runat="server" ID="Label1" Text="No Records Found" Font-Size="Medium"
                                                            class="styleMandatoryLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlInvoice" runat="server" GroupingText="Invoice" CssClass="stylePanel">
                                                                <asp:GridView ID="grvInvoice" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                                    Width="100%" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" OnRowDataBound="grvInvoice_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("Invoice_ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="View">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="BtnView" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                                    runat="server" ToolTip="View" />
                                                                                <%-- <asp:LinkButton ID="BtnView" runat="server" Text="View" 
                                                                    ToolTip="View"></asp:LinkButton>--%>
                                                                                <%--<asp:Button ID="BtnView" runat="server" Text="View" CssClass="styleGridShortButton"
                                                                            ToolTip="View"></asp:Button>--%>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Sl.No.">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblslNo" runat="server" ToolTip="Sl.No."></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Invoice Number" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("Invoice_No") %>' ToolTip="Invoice Number"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblVendorName" runat="server" Text='<%#Eval("Vendor_Name") %>' ToolTip="Vendor Name"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlPostDisb" runat="server" GroupingText="Post Disbursement Document"
                                                                CssClass="stylePanel">
                                                                <asp:GridView ID="gvPDDT" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    DataKeyNames="PDDC_Doc_Cat_ID" CssClass="styleInfoLabel" OnRowDataBound="gvPDDT_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="PDDC TypeId" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPDTID" runat="server" Text='<%# Bind("PDDC_Doc_Cat_ID") %>'></asp:Label>
                                                                                <asp:Label ID="lblCanView" runat="server" Visible="false" Text='<%# Eval("CanView")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="PDDC Type" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("PDDC_Doc_Type") %>' ToolTip="PDDC Type"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="left" Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="PDDC Description" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("PDDC_Doc_Description") %>'
                                                                                    ToolTip="PDDC Description"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Collected By" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtColletedBy" runat="server" Text='<%# Bind("CollectedBy") %>' ToolTip="Collected By"></asp:Label>
                                                                                <asp:Label ID="lblColUser" Visible="false" runat="server" Text='<%# Bind("Collected_By") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Collected Date" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtColletedDate" ToolTip="Collected Date" runat="server" Width="70px"
                                                                                    Text='<%# Bind("Collected_Date") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Scanned By" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtScannedBy" ToolTip="Scanned By" runat="server" Text='<%# Bind("Scandedby") %>'></asp:Label>
                                                                                <asp:Label ID="lblScanUser" runat="server" Visible="false" Text='<%# Bind("Scanned_By") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Scanned Date" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtScannedDate" ToolTip="Scanned Date" runat="server" Width="70px"
                                                                                    Text='<%# Bind("Getdates")%> '></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="View Document">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="hyplnkView" CommandArgument='<%# Bind("Scanned_Ref_No") %>'
                                                                                    OnClick="hyplnkViewPost_Click" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                                    runat="server" ToolTip="View Documen" />
                                                                                <asp:Label runat="server" ID="lblPath" Text='<%# Eval("Scanned_Ref_No")%>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Remarks">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                                                    MaxLength="100" Width="120px" Text='<%# Eval("Remarks")%>' ToolTip="Remarks"></asp:TextBox>
                                                                                <asp:Label ID="lblProgramName" runat="server" Visible="false" Text='<%# Eval("ProgramName")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="2">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td valign="top" align="left" colspan="2">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" colspan="2">
                                        <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td colspan="5" align="center">
                                                    <br />
                                                    <asp:Button runat="server" ID="btnXLPorting" Text="Export Amortization" CssClass="styleSubmitLongButton"
                                                        OnClick="btnXLPorting_Click" ToolTip="Export Amortization" />
                                                    <asp:Button runat="server" ID="btnExportJV" Text="Export JV" CssClass="styleSubmitButton"
                                                        ToolTip="Export JV" OnClick="btnExportJV_Click" />
                                                    <asp:Button ID="btnDebitNote" runat="server" Text="Print Debit Note" Visible="false" CssClass="styleSubmitLongButton" OnClick="btnDebitNote_Click" />                                                    
                                                    <asp:Button runat="server" ID="btnDebitNotes" Text="Print Debit Note" 
                                                        ToolTip="Print" CssClass="styleSubmitButton" OnClick="btnDebitNote_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grvInvv" runat="server" AutoGenerateColumns="false" Width="100%"
                                                        CssClass="styleInfoLabel" ShowFooter="true" ShowHeader="true">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sr No." ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSrNo" runat="server" Text='<%#Container.DataItemIndex+1 %>' ToolTip="Sr No."></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice No." ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("INVOICE_NO") %>' ToolTip="Invoice No."></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="left"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("INVOICE_DATE") %>' ToolTip="Date"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="left"/>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Entity Name" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEntityName" runat="server" Text='<%# Bind("Entity_Name") %>' ToolTip="Entity Name"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblEntityNamef" runat="server" Text="Total"></asp:Label>
                                                                </FooterTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Bill Amt.(Rs.)" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBillamt" runat="server" Text='<%# Bind("TOT_BILL_AMT") %>' ToolTip="Total Bill Amt.(Rs.)"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblBillamtf" runat="server" ToolTip="Bill Amt.(Rs.)"></asp:Label>
                                                                </FooterTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTax" runat="server" Text='<%# Bind("Amount") %>' ToolTip="Total Bill Amt.(Rs.)"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTaxf" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <HeaderStyle HorizontalAlign="Right" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rental Schedule No." ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRS" runat="server" Text='<%# Bind("RS_NO") %>' ToolTip="Rental Schedule No."></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="center">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators('btnSave');"
                                                                ValidationGroup="btnSave" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                                                ToolTip="Activate" Text="Activate" />
                                                            <asp:Button runat="server" ID="btnRevoke" Text="Revoke" Visible="false" ToolTip="Revoke"
                                                                CssClass="styleSubmitButton" OnClick="btnRevoke_Click" OnClientClick="return fnRevokeValidators();" />
                                                            <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                                                                ToolTip="Clear" Text="Clear" OnClientClick="confirm('Do you want to Clear?');"
                                                                meta:resourcekey="btnClearResource1" OnClick="btnClear_Click" />
                                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                                                                ToolTip="Cancel" CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                                                  <asp:Button runat="server" ID="btnprint" Text="Print Interim" CausesValidation="False"
                                                                ToolTip="Print" CssClass="styleSubmitButton" OnClick="btnPrint_Click" Visible="false" />
                                                              <asp:Button runat="server" ID="btnCashflow" Text="Print CahFlow" CausesValidation="False"
                                                                ToolTip="Print" CssClass="styleSubmitButton" OnClick="btnCashflow_Click" Visible="false"/>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" align="center">
                                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hdnActivationType" runat="server" />
                                    <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnView" />
                                    <asp:HiddenField ID="hdnDate" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
