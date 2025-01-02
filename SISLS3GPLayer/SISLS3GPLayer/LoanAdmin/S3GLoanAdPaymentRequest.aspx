<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLoanAdPaymentRequest.aspx.cs"
    Title="Payment Request" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    Inherits="Loan_Admin_S3GLoanAdminPaymentRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagPrefix="uc1" TagName="PageNavigator" %>
<asp:Content ID="ContentPaymentRequest" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">

    <script type="text/javascript" language="javascript">


        function addcommas(input) {
            var nstr = input.value;
            nstr += '';
            var xarr = nstr.split('.');
            var x1 = xarr[0];
            var x2 = x1.length > 1 ? '.' + xarr[1] : '';

            var rgx = /(\d+)(\d{2})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            alert(x1 + x2);

        }


        function checkValueDate_NextSystemDate(sender, args) {

            // debugger;
            var today = new Date();
            var currentyear = today.getYear();
            var currentmonth = today.getMonth() + 1;


            if (currentmonth > 3) {
                pastvalidyear = currentyear;
                futurevalidyear = currentyear + 1;

            }
            else {
                pastvalidyear = currentyear - 1;
                futurevalidyear = currentyear;

            }
            if ((sender._selectedDate.getMonth() + 1) > currentmonth && (sender._selectedDate.getYear() == currentyear))//Future month date cannot be selected
            {
                alert('You cannot select a date greater than the current month end and lesser than the current year');
                sender._textbox.set_Value(today.format(sender._format));
                return;
            }
            if (sender._selectedDate.getYear() == pastvalidyear || sender._selectedDate.getYear() == futurevalidyear) {
                if (sender._selectedDate.getYear() == futurevalidyear && (sender._selectedDate.getMonth() + 1) > 3) {
                    alert('You cannot select a date greater than the current month end and lesser than the current year');
                    sender._textbox.set_Value(today.format(sender._format));
                    return;
                }
                if (sender._selectedDate.getYear() == pastvalidyear && (sender._selectedDate.getMonth() + 1) <= 3) {
                    alert('You cannot select a date greater than the current month end and lesser than the current year');
                    sender._textbox.set_Value(today.format(sender._format));
                    return;
                }
            }
            else {
                alert('You cannot select a date greater than the current month end and lesser than the current year');
                sender._textbox.set_Value(today.format(sender._format));
                return;

            }

        }

        function Funcheckvaliddecimal(input) {

            var Amountvalue = input.value;
            var count = 0;
            if (Amountvalue != '') {
                for (var i = 0; i < Amountvalue.length; i++) {
                    var c = Amountvalue.charAt(i);
                    if (c == '.') {
                        count++;
                    }
                }
                if (count > 1) {
                    alert('Enter a valid Decimal');
                    input.value = '';
                    input.focus();
                    return;
                }

            }
        }
        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainerER_TabPanelPR_btnCreateCustomer').click();
        }

        function fnConfirmSave() {

            if (confirm('Do you want to save?')) {
                return true;
            }
            else
                return false;

        }

        function fnCheckPayableAmount(input) {
            var PayableAmt = input;
            if (input.value == '') {
                alert('Payable Amount should not be blank');
                input.focus();
                return;
            }
            if (input.value == '.') {
                alert('Enter a Valid Decimal');
                input.focus();
                return;
            }
            if (parseFloat(input.value) <= 0) {
                alert('Payable Amount should be greater than 0');
                input.focus();
                return;
            }

            //var array = new Array();
            //array = PayableAmt.split(".");
            //if (array.length > 0) {
            //    if (array[0].length > 13) {
            //        alert('Prefix should not exceed 13 Digits');
            //        input.focus();
            //        return;
            //    }
            //    else if (array[1] != null && array[1].length > 2) {
            //        alert('Suffix should not exceed 2 Digits');
            //        input.focus();
            //        return;
            //    }
            //    else if (array[1] != null && array[1].length == 0) {
            //        alert('Enter a Valid Decimal');
            //        input.focus();
            //        return;
            //    }
            //}
        }

        function calendarShown(sender, args) {
            sender._popupBehavior._element.style.zIndex = 99999999;
        }

    </script>

    <%--Header--%>
    <table width="100%">
        <tr width="100%">
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Payment Request" ID="lblPaymentRequest" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="1">
        <tr>
            <td valign="top">
                <cc1:TabContainer ID="TabContainerER" runat="server" CssClass="styleTabPanel" Width="100%"
                    ScrollBars="None">
                    <cc1:TabPanel runat="server" ID="TabPanelPR" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Payment Request
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr width="100%" valign="top">
                                            <td width="55%">
                                                <asp:Panel ID="pnlPaymentInformation1" runat="server" ToolTip="Payment Information"
                                                    GroupingText="Payment Information" CssClass="stylePanel">
                                                    <div id="div1" runat="server" style="height: 300px; width: 100%; overflow-x: hidden; overflow-y: auto;">
                                                        <asp:Panel ID="pnlPaymentInformation" DefaultButton="btnSave" Width="98%" runat="server"
                                                            CssClass="stylePanel">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 18%">
                                                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"
                                                                            ToolTip="Line of Business"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator Display="None" ID="RFVddlLOB" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlLOB" InitialValue="0"
                                                                            ErrorMessage="Select Line of Business" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="State" ToolTip="State" ID="lblBranch" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign">
                                                                        <uc3:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                            ValidationGroup="Save" ErrorMessage="Select a State" IsMandatory="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 15%">
                                                                        <asp:Label runat="server" ToolTip="Payment Request Number" Text="Voucher No." ID="lblPaymentRequestNo"
                                                                            CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" align="left">
                                                                        <asp:TextBox ID="txtPaymentRequestNo" ToolTip="Payment Voucher No(This is an autogenerated value)"
                                                                            Width="40%" ReadOnly="true" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="Status" ToolTip="Payment Status" ID="lblPaymentStatus"
                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" align="left">
                                                                        <%--<asp:TextBox ID="txtPaymentStatus" runat="server" ></asp:TextBox>--%>
                                                                        <asp:DropDownList ID="ddlPaymentStatus" ToolTip="Payment Status" runat="server" Enabled="false"
                                                                            Width="50%">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator10" CssClass="styleMandatoryLabel"
                                                                            InitialValue="0" runat="server" SetFocusOnError="True" ControlToValidate="ddlPaymentStatus"
                                                                            ErrorMessage="Select Status" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="Voucher Date" ToolTip="Payment Voucher Date" ID="lblPaymentRequestDate"
                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" align="left">
                                                                        <asp:TextBox ID="txtPaymentRequestDate" ToolTip="Payment Voucher Date" Width="35%" runat="server"></asp:TextBox>
                                                                        <asp:Image ID="imgPaymentRequestDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                        <cc1:CalendarExtender ID="CalendarExtenderPaymentRequestDate" runat="server" Enabled="True"
                                                                            PopupButtonID="imgPaymentRequestDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                            TargetControlID="txtPaymentRequestDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ValidationGroup="Save" ControlToValidate="txtPaymentRequestDate"
                                                                            ErrorMessage="Enter Voucher Date"></asp:RequiredFieldValidator>
                                                                        &nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="display:none">
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="Value Date" ToolTip="Value date" ID="lblValueDate"
                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" align="left">
                                                                        <asp:TextBox ID="txtValueDate" runat="server" ToolTip="Value date" Width="35%"></asp:TextBox>
                                                                        <asp:Image ID="imgValueDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                                                        <cc1:CalendarExtender ID="CalendarExtenderValueDate" runat="server" Enabled="True"
                                                                            PopupButtonID="imgValueDate" OnClientDateSelectionChanged="checkValueDate_NextSystemDate"
                                                                            TargetControlID="txtValueDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ValidationGroup="Save" ControlToValidate="txtValueDate"
                                                                            ErrorMessage="Enter Value Date"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="Payment Type" ToolTip="Payment Type" ID="lblPaymentType"
                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" align="left">
                                                                        <asp:DropDownList ID="ddlPaymentType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentType_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvPaymentType" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlPaymentType" InitialValue="0"
                                                                            ErrorMessage="Select Payment Type" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvshowPaymentType" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlPaymentType" InitialValue="0"
                                                                            ErrorMessage="Select Payment Type" ValidationGroup="Show"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="Pay Mode" ToolTip="Pay mode" ID="lblPayMode" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" align="left">
                                                                        <asp:DropDownList ID="ddlPayMode" runat="server" ToolTip="Pay mode" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="ddlPayMode_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator3" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlPayMode" InitialValue="0"
                                                                            ErrorMessage="Select Pay Mode" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="Pay To" ToolTip="Pay to" ID="lblPayTo" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlPayTo" runat="server" ToolTip="Pay to" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="ddlPayTo_SelectedIndexChanged" Enabled="false">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator66" CssClass="styleMandatoryLabel"
                                                                            runat="server" SetFocusOnError="True" InitialValue="0" ControlToValidate="ddlPayTo"
                                                                            ErrorMessage="Select Pay To" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator7" CssClass="styleMandatoryLabel"
                                                                            runat="server" SetFocusOnError="True" InitialValue="0" ControlToValidate="ddlPayTo"
                                                                            ErrorMessage="Select Pay To" ValidationGroup="Show"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="GL Based" ToolTip="Account Based Payment" ID="lblAccountBased"
                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign">
                                                                        <asp:DropDownList ID="chkAccountBased" AutoPostBack="True" ToolTip="Account Based Payment"
                                                                            runat="server" OnSelectedIndexChanged="chkAccountBased_SelectedIndexChanged"
                                                                            Enabled="false">
                                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle">
                                                                        <asp:Label runat="server" Text="Contract Ref" ToolTip="Contract Ref" ID="lblContractRef"
                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign">
                                                                        <asp:CheckBox ID="ChkContractRef" AutoPostBack="true" OnCheckedChanged="ChkContractRef_CheckedChanged" runat="server" Enabled="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr visible="false" runat="server">
                                                                    <td align="left" class="styleFieldLabel" valign="middle">&nbsp;
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" colspan="4"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <%--Row 2:  curreny Value and Doc Amount --%>
                                                                <%-- <tr width="100%">
                                    <td colspan="4" align="left" valign="middle">
                                        &nbsp;
                                    </td>
                                </tr>--%>
                                                            </table>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="PanelPayType" Visible="true" runat="server" ToolTip="Pay Type" GroupingText="Pay Type"
                                                    CssClass="stylePanel">
                                                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblReceiptFrom" runat="server" CssClass="styleDisplayLabel" Text="Lessee Name"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc3:Suggest ID="ddlReceiptFrom" runat="server" ServiceMethod="GetReceiptList" AutoPostBack="true" Width="250px"
                                                                    IsMandatory="false" ValidationGroup="Show" ErrorMessage="Select a Lessee Name" />
                                                            </td>
                                                        </tr>
                                                        <tr style="display: none">
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblTranche" runat="server" CssClass="styleDisplayLabel" Text="Tranche"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc3:Suggest ID="ddlTranche" runat="server" ServiceMethod="GetTrancheList" AutoPostBack="true" Width="250px"
                                                                    IsMandatory="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                                <td colspan="2" align="center">
                                                                    <asp:Button runat="server" ID="btnShow" Text="Show" ToolTip="Show Invoice Details" Width="60px"
                                                                        ValidationGroup="Show" CssClass="styleSubmitButton" OnClick="btnShow_Click" />
                                                                </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="PnlCustEntityInformation1" runat="server" ToolTip="Customer/Entity Information"
                                                    GroupingText="Payee Informations" CssClass="stylePanel">
                                                    <div id="divcustomerentity" runat="server" style="height: 250px; width: 100%; overflow-x: hidden; overflow-y: auto;">
                                                        <asp:Panel ID="PnlCustEntityInformation" Width="95%" ToolTip="Customer or Entity Informations"
                                                            runat="server" CssClass="stylePanel">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label runat="server" ToolTip="Cutomer or Entity" Text="Customer/Entity" ID="lblCode"
                                                                            CssClass="styleMandatoryLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtCustomerCode" ToolTip="Cutomer or Entity code" runat="server"
                                                                            Style="display: none;" MaxLength="50"></asp:TextBox>
                                                                        <uc2:LOV ID="ucCustomerCodeLov" onblur="fnLoadCustomer()" runat="server" strLOV_Code="CMD"
                                                                            DispalyContent="Code" />
                                                                        <asp:Button ID="btnCreateCustomer" runat="server" UseSubmitBehavior="true" Text="Create"
                                                                            Style="display: none;" OnClick="btnCreateCustomer_Click" CssClass="styleSubmitShortButton"
                                                                            CausesValidation="false" />
                                                                        <asp:RequiredFieldValidator ID="rfvcmbCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                            ErrorMessage="Select a Customer/Entity" ValidationGroup="Save" CssClass="styleMandatoryLabel"
                                                                            Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvShowCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                            ErrorMessage="Select a Customer/Entity" ValidationGroup="Show" CssClass="styleMandatoryLabel"
                                                                            Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" align="left">
                                                                        <asp:Panel ID="pnlAddressDetails" runat="server" GroupingText="">
                                                                            <uc1:S3GCustomerAddress ID="ucCustomerAddress" runat="server" />
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label runat="server" ToolTip="Payee Bank Account" Text="Bank Account" ID="lblPayeeBankName"
                                                                            CssClass="styleMandatoryLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlPayeeBankAccount" runat="server" ToolTip="Payee Bank Account"></asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="Currency" runat="server" CssClass="stylePanel" ToolTip="Currency Informations"
                                                    GroupingText="Currency Information">
                                                    <table>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" Text="Currency Code" ToolTip="Currency Code" ID="lblCurrencyCode"
                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" align="left">
                                                                <asp:DropDownList ID="ddlCurrencyCode" ToolTip="Currency Code" runat="server" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlCurrencyCode_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator4" CssClass="styleMandatoryLabel"
                                                                    runat="server" SetFocusOnError="True" ControlToValidate="ddlCurrencyCode" InitialValue="0"
                                                                    ErrorMessage="Select Currency Code" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ToolTip="Default Currency Code " Text="(Default Currency)"
                                                                ID="defaultCurrency" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" Text="Currency Value" ToolTip="Currency Value" ID="lblCurrencyValue"
                                                                    CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td align="left" class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCurrencyValue" runat="server" Width="45%" ToolTip="Exchange Value"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" ToolTip="Document Amount" Text="Document Amount" ID="lblDocAmount"
                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td align="left" class="styleFieldAlign">
                                                                <asp:TextBox ID="txtDocAmount" runat="server" ToolTip="Document Amount" MaxLength="12"
                                                                    Width="60%" Style="text-align: right" Enabled="false"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeDocAmount" runat="server" TargetControlID="txtDocAmount"
                                                                    ValidChars="." FilterType="Numbers,Custom" Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator Display="None" ID="rfvDocAmount" CssClass="styleMandatoryLabel"
                                                                    runat="server" SetFocusOnError="True" ValidationGroup="Save" ControlToValidate="txtDocAmount"
                                                                    ErrorMessage="Enter Document Amount"></asp:RequiredFieldValidator>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lbldefaultcurrencyvalue" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr width="100%">
                                            <td width="100%" colspan="2"></td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <%--<asp:LinkButton CausesValidation="false" runat="server" ToolTip="Expand Grid to view better"
                                                    ID="lnkGridSize" OnClick="lnkGridSize_Click" Width="40%" Enabled="false"></asp:LinkButton>--%>
                                            </td>
                                        </tr>
                                        <%--Grid view- Payment details--%>
                                        <tr width="99%" align="center">
                                            <td width="100%" align="center">
                                                <asp:Panel ID="PanelPaymentDetails" Width="100%" runat="server"
                                                    GroupingText="Payment Details" CssClass="stylePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView runat="server" ShowFooter="true" ID="grvPaymentDetails" Width="100%"
                                                                    OnRowDataBound="grvPaymentDetails_RowDataBound" Visible="true"
                                                                    OnRowDeleting="grvPaymentDetails_RowDeleting" RowStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-CssClass="styleGridHeader" FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <%--Prime Account Number - From Entity Master (in PaymentDetails SQL Table - column PANum) --%>
                                                                        <asp:TemplateField HeaderText="Rental Schedule Number" FooterStyle-Width="10%" ItemStyle-HorizontalAlign="Left"
                                                                            ItemStyle-Width="20%" HeaderStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPrimeAccountNumber" ToolTip="Rental Schedule Number" Width="100%"
                                                                                    runat="server" Text='<%#Eval("Panum")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <uc3:Suggest ID="ddlFooterPrimeAccountNumber" OnItem_Selected="ddlFooterPrimeAccountNumber_SelectedIndexChanged" AutoPostBack="true"
                                                                                    runat="server" ServiceMethod="GetPANum" Width="170px" />
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--Sub Account Number - From Entity Master (in PaymentDetails SQL Table - column SANum)--%>
                                                                        <asp:TemplateField HeaderText="Sub Account Number" ItemStyle-HorizontalAlign="Left" Visible="false"
                                                                            FooterStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSubAccountNumber" ToolTip="Sub Account Number" Width="100%" runat="server"
                                                                                    Text='<%#Eval("Sanum")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlFooterSubAccountNumber" ToolTip="Sub Account Number" AutoPostBack="true" runat="server">
                                                                                </asp:DropDownList>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--CashFlow Flag ID--%>
                                                                        <asp:TemplateField HeaderText="CashFLow Flag ID" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="1%"
                                                                            ItemStyle-Width="1%" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCashFlowFlagID" Text='<%#Eval("CashFlow_ID")%>' Width="100%" runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--CashFlow ID--%>
                                                                        <asp:TemplateField HeaderText="Cashflow ID" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="1%"
                                                                            ItemStyle-Width="1%" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCashFlow_ID" Text='<%#Eval("CashFlow_ID")%>' Width="100%" runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--CashFlow Description--%>
                                                                        <asp:TemplateField HeaderText="Pay Type" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="15%"
                                                                            ItemStyle-Width="15%" Visible="true">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCashflow_Description" Text='<%#Eval("CashFlow_Description")%>' Width="100%" runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlFooterFlowType" Width="100%" ToolTip="Pay Type" runat="server"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFooterFlowType_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator Display="None" ID="RFVddlFooterFlowType" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPayment" runat="server" ControlToValidate="ddlFooterFlowType"
                                                                                    InitialValue="0" ErrorMessage="Select Pay Type"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- GL Account --%>
                                                                        <asp:TemplateField HeaderText="GL Account" FooterStyle-Width="10%" ItemStyle-Width="10%"
                                                                            HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGLCode" Width="100%" ToolTip="GL Account" runat="server" Text='<%#Eval("GL_Code")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--<asp:TextBox ID="txtFooterGL_Code" ReadOnly="true" Width="100%" runat="server" ></asp:TextBox>--%>
                                                                                <asp:DropDownList ID="ddlFooterGL_Code" ToolTip="GL Account" runat="server" Width="100%"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFooterGL_Code_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvddlFooterGL_Code" runat="server" ControlToValidate="ddlFooterGL_Code"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPayment" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                                    Display="None" ErrorMessage="Select GL Account"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- SL Account --%>
                                                                        <asp:TemplateField HeaderText="SL Account" FooterStyle-Width="10%" ItemStyle-Width="10%"
                                                                            HeaderStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSL_Code" Width="100%" ToolTip="SL Account" runat="server" Text='<%#Eval("SL_Code")%>'></asp:Label>
                                                                                <%-- <asp:HiddenField ID="hndSLAccountID" runat="server" Value='<%#Eval("SL_Code")%>' />--%>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <uc3:Suggest ID="ddlFooterSL_Code" runat="server" ServiceMethod="GetSLCodes" ToolTip="SL Account" Width="100px" />
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- Remarks --%>
                                                                        <asp:TemplateField HeaderText="Remarks" FooterStyle-Width="20%" ItemStyle-Width="20%"
                                                                            HeaderStyle-Width="15%">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="lblDescription" ToolTip="Remarks" Width="75%" runat="server"
                                                                                    TextMode="MultiLine" onkeyup="maxlengthfortxt(500);" Text='<%#Eval("Remarks")%>'
                                                                                    AutoPostBack="true" OnTextChanged="Desc_TextChanged"> </asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtFooterDescription" ToolTip="Remarks" TextMode="MultiLine"
                                                                                    Width="75%" MaxLength="100" onkeyup="maxlengthfortxt(500);" runat="server" Wrap="true"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- Amount --%>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Amount" FooterStyle-Width="20%"
                                                                            ItemStyle-Width="20%" HeaderStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="lblAmount" ToolTip="Amount" Style="text-align: right" Width="90%"
                                                                                    MaxLength="12" runat="server" Text='<%#Eval("Amount")%>' AutoPostBack="true"
                                                                                    OnTextChanged="Desc_TextChanged"></asp:TextBox>
                                                                                <asp:Label ID="lblActualAmount" Width="1%" runat="server" Text='<%#Eval("ActualAmount")%>'
                                                                                    Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtFooterAmount" ToolTip="Amount" Width="90%" runat="server" MaxLength="12"
                                                                                    Style="text-align: right"></asp:TextBox>
                                                                                <asp:Label ID="lblFooterActualAmount" Width="1%" runat="server" Visible="false"></asp:Label>
                                                                                <cc1:FilteredTextBoxExtender ID="ftbeFtrAmt" runat="server" TargetControlID="txtFooterAmount"
                                                                                    FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtFooterAmount" runat="server" ControlToValidate="txtFooterAmount"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPayment" CssClass="styleMandatoryLabel"
                                                                                    Display="None" ErrorMessage="Enter Amount"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- Action --%>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <%--<asp:LinkButton ID="lnkRemove" Width="100%" runat="server" CausesValidation="false"
                                            OnClick="lnkRemove_Click" Text="Remove"></asp:LinkButton>--%>
                                                                                <asp:LinkButton ID="lnkRemove" ToolTip="Remove from the grid" Width="100%" runat="server"
                                                                                    CausesValidation="false" CommandName="Delete" Text="Remove"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:LinkButton ID="lnkAdd" Width="100%" ToolTip="Add to the grid" ValidationGroup="GridPayment"
                                                                                    OnClick="lnkAdd_Click" runat="server" Text="Add"></asp:LinkButton>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--PA Sa Ref ID--%>
                                                                        <asp:TemplateField HeaderText="PA Sa Ref ID" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="1%"
                                                                            ItemStyle-Width="1%" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPASAREFID" Text='<%#Eval("Pa_Sa_Ref_ID")%>' Width="100%" runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                              
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:GridView ID="grvFunderReceipt" runat="server" AutoGenerateColumns="false" Width="99%" ShowFooter="true"
                                                                    OnRowDataBound="grvFunderReceipt_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="S No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFunderSNo" Text='<%# Container.DataItemIndex+1%>' runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Note ID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvFndrNoteID" runat="server" Text='<%#Bind("Note_ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Note No">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvFndrNoteNo" runat="server" Text='<%#Bind("Note_No") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <uc3:Suggest ID="ddlNoteNo" runat="server" ServiceMethod="GetNoteList" IsMandatory="true" ValidationGroup="vsFunderAdd"
                                                                                    ErrorMessage="Enter and Select the Note No" ToolTip="Note No" OnItem_Selected="ddlNoteNo_Item_Selected"
                                                                                    AutoPostBack="true" />
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                            <FooterStyle Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Tranche ID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvFndrTrancheID" runat="server" Text='<%#Bind("Tranche_ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Tranche Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvFndrTrancheName" runat="server" Text='<%#Bind("Tranche_Name") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <uc3:Suggest ID="ddlfndrTrancheNo" runat="server" ServiceMethod="GetNoteTrancheList" IsMandatory="false" ToolTip="Tranche Name" />
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                            <FooterStyle Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Account Description ID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFndrCashFlowID" runat="server" Text='<%#Bind("CashFlow_ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Account Description">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFndrCFDesc" runat="server" Text='<%#Bind("CashFlow_Desc") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlFndrCFDesc" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFndrCFDesc_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvFndrCFDesc" runat="server" ControlToValidate="ddlFndrCFDesc" Enabled="true" Display="None" InitialValue="0"
                                                                                    CssClass="styleMandatoryLabel" ErrorMessage="Select the Account Description" ValidationGroup="vsFunderAdd"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                                            <FooterStyle Width="20%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="GL Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFndrGLCodeDesc" runat="server" Text='<%#Bind("GL_Code_Desc") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <uc3:Suggest ID="ddlFndrGLCode" runat="server" ServiceMethod="GetGLCodeList" IsMandatory="true" ValidationGroup="vsFunderAdd"
                                                                                    ErrorMessage="Select GL Code" Width="80px" />
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                            <FooterStyle Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="SL Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFndrSLCodeDesc" runat="server" Text='<%#Bind("SL_Code_Desc") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <uc3:Suggest ID="ddlFndrSLCode" runat="server" ServiceMethod="GetSLCodeList" IsMandatory="true" ValidationGroup="vsFunderAdd"
                                                                                    ErrorMessage="Select SL Code" Width="80px" />
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                                            <FooterStyle Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Cashflow Flag ID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFndrCFFlagID" runat="server" Text='<%#Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblFooterFndrCFFlagID" runat="server" Text=""></asp:Label>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Amount">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblgvFunderAmount" runat="server" Text='<%#Bind("Amount") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtFooterFunderAmount" runat="server" Text="" MaxLength="16" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftbeFooterFunderAmount" runat="server" TargetControlID="txtFooterFunderAmount" Enabled="true"
                                                                                    FilterType="Custom,Numbers" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvFooterFunderAmount" runat="server" ControlToValidate="txtFooterFunderAmount" Enabled="true" Display="None"
                                                                                    CssClass="styleMandatoryLabel" ErrorMessage="Enter the Amount" ValidationGroup="vsFunderAdd"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                            <FooterStyle Width="15%" HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkgvFunderRemove" runat="server" Text="Remove" ToolTip="Remove Detail" OnClick="lnkgvFunderRemove_Click"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Button ID="btnFooterFunderAdd" runat="server" CssClass="styleSubmitButton" Width="80px" Text="Add" ValidationGroup="vsFunderAdd"
                                                                                    OnClick="btnFooterFunderAdd_Click"></asp:Button>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                            <FooterStyle Width="10%" HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td align="center">
                                                                <table width="100%" class="stylePagingControl">
                                                                    <tr align="right" class="stylePagingControl">
                                                                        <td align="right" width="80%">
                                                                            <asp:Label ID="lblAmount" CssClass="styleDisplayLabel" runat="server" Text="Sub-Total"></asp:Label>
                                                                        </td>
                                                                        <td width="15%" align="right" colspan="2">
                                                                            <asp:Label ID="lblPaymentDetailsTotal" ToolTip="Total amount of payment details grid"
                                                                                CssClass="styleDisplayLabel" runat="server" Text="0"></asp:Label>
                                                                        </td>
                                                                        <%--<td width="5%">
                                                                            &nbsp;
                                                                        </td>--%>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <%-- </asp:Panel>--%>
                                    <%--  <tr id="trlblPaymentAdjustment" runat="server">
                    <td>
                        <b><u>
                            <asp:Label ID="textadjust" runat="server" Text="Payment Adjustment" CssClass="stylePanel"></asp:Label>
                        </u></b>
                    </td>
                </tr>--%>
                                    <%--Grid view- Payment details--%>
                                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                        <tr width="100%" align="center">
                                            <td width="100%" align="center">
                                                <asp:Panel ID="PanelPaymentAdjustment" ToolTip="Payment Adjustment Informations"
                                                    Width="100%" runat="server" GroupingText="Payment Adjustment" CssClass="stylePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView runat="server" ToolTip="Payment Adjustment grid" ShowFooter="true"
                                                                    ID="grvPaymentAdjustment" Width="100%" OnRowDataBound="grvPaymentAdjustment_RowDataBound"
                                                                    OnRowDeleting="grvPaymentAdjustment_RowDeleting" RowStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-CssClass="styleGridHeader" FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <%--Add or Less grid --%>
                                                                        <asp:TemplateField HeaderText="Add or Less" FooterStyle-Width="10%" ItemStyle-HorizontalAlign="Left"
                                                                            ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAddOrLess" ToolTip="Add or Less" Width="100%" runat="server" Text='<%#Eval("AddOrLess")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--<asp:TextBox ID="txtFooterPrimeAccountNumber" Width="100%" runat="server"></asp:TextBox>--%>
                                                                                <asp:DropDownList ID="ddlFooterAddOrLess" ToolTip="Add or Less" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlFooterAddOrLess_OnSelectedIndexChanged" runat="server">
                                                                                    <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Add</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Less</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator Display="None" ID="RFVddlFooterAddOrLess" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPaymentadj" runat="server" ControlToValidate="ddlFooterAddOrLess"
                                                                                    InitialValue="0" ErrorMessage="Select Add/Less"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--Prime Account Number - From Entity Master (in PaymentDetails SQL Table - column PANum) --%>
                                                                        <asp:TemplateField HeaderText="Rental Schedule Number" FooterStyle-Width="10%" ItemStyle-HorizontalAlign="Left"
                                                                            ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPrimeAccountNumber" ToolTip="Rental Schedule Number" Width="100%"
                                                                                    runat="server" Text='<%#Eval("PANum")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--<asp:TextBox ID="txtFooterPrimeAccountNumber" Width="100%" runat="server"></asp:TextBox>--%>
                                                                                <uc3:Suggest ID="ddlFooterPrimeAccountNumber" OnItem_Selected="ddlFooterPrimeAccountNumberA_SelectedIndexChanged" AutoPostBack="true" runat="server" ServiceMethod="GetPANumA" Width="100px" />
                                                                                <%-- <asp:DropDownList ID="ddlFooterPrimeAccountNumber" ToolTip="Prime Account Number"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFooterPrimeAccountNumberA_SelectedIndexChanged"
                                                                                    runat="server">
                                                                                </asp:DropDownList>--%>
                                                                                <%--  <asp:RequiredFieldValidator Display="None" ID="RFVddlPrimeAccountNumber" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPaymentadj" runat="server" ControlToValidate="ddlFooterPrimeAccountNumber"
                                                                                    InitialValue="0" ErrorMessage="Select Prime Account Number"></asp:RequiredFieldValidator>--%>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--Sub Account Number - From Entity Master (in PaymentDetails SQL Table - column SANum)--%>
                                                                        <asp:TemplateField HeaderText="Sub Account Number" ItemStyle-HorizontalAlign="Left" Visible="false"
                                                                            FooterStyle-Width="15%" ItemStyle-Width="15%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSubAccountNumber" ToolTip="Sub Account Number" Width="100%" runat="server"
                                                                                    Text='<%#Eval("SANum")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--<asp:TextBox ID="txtFooterSubAccountNumber" Width="100%" runat="server"></asp:TextBox>--%>
                                                                                <asp:DropDownList ID="ddlFooterSubAccountNumber" Width="100%" ToolTip="Sub Account Number"
                                                                                    runat="server">
                                                                                    <%--OnSelectedIndexChanged="ddlFooterSubAccountNumberA_SelectedIndexChanged">--%>
                                                                                </asp:DropDownList>
                                                                                <%--<asp:RequiredFieldValidator Display="None" ID="RFVddlFooterSubAccountNumber" CssClass="styleMandatoryLabel"
                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlFooterSubAccountNumber"
                                            InitialValue="0" ValidationGroup="GridPaymentadj" ErrorMessage="Select Sub Account Number"></asp:RequiredFieldValidator>--%>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--Pay type --%>
                                                                        <asp:TemplateField HeaderText="Pay Type" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="15%"
                                                                            ItemStyle-Width="15%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPayType" ToolTip="Pay Type" Text='<%#Eval("PayType")%>' Width="100%"
                                                                                    runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--<asp:TextBox ID="txtFooterPayType"  Width="100%" Text='<%#Eval("PayType")%>' Enabled="false" runat="server"></asp:TextBox>--%>
                                                                                <asp:DropDownList ID="ddlFooterPayType" Width="100%" ToolTip="Pay Type" runat="server"
                                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFooterPayType_OnSelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator Display="None" ID="RFVddlFooterFlowType" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPaymentadj" runat="server" ControlToValidate="ddlFooterPayType"
                                                                                    InitialValue="0" ErrorMessage="Select Pay Type"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%--Sub Account Number - From Entity Master (in PaymentDetails SQL Table - column SANum)--%>
                                                                        <asp:TemplateField HeaderText="PayTypeCode" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="15%"
                                                                            ItemStyle-Width="15%" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPayTypeID" Text='<%#Eval("PayTypeID")%>' Width="100%" runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- GL Account --%>
                                                                        <asp:TemplateField HeaderText="GL Account" FooterStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGLCode" ToolTip="GL Account" Width="100%" runat="server" Text='<%#Eval("GL_Code")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--<asp:TextBox ID="txtFooterGL_Code" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>--%>
                                                                                <asp:DropDownList ID="ddlFooterGL_Code" ToolTip="GL Account" runat="server" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlFooterGL_CodeA_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtFooterGL_Code" runat="server" ControlToValidate="ddlFooterGL_Code"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPaymentadj" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                                    Display="None" ErrorMessage="Select GLCode"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- SL Account --%>
                                                                        <asp:TemplateField HeaderText="SL Account" FooterStyle-Width="10%" ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSL_Code" Width="100%" ToolTip="SL Account" runat="server" Text='<%#Eval("SL_Code")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <uc3:Suggest ID="ddlFooterSL_Code" runat="server" ServiceMethod="GetSLCodesA" ToolTip="SL Account" Width="100px" />
                                                                                <%--<asp:TextBox ID="txtFooterSL_Code" Width="100%" ReadOnly="true" runat="server"></asp:TextBox>--%>
                                                                                <%--<asp:DropDownList ID="ddlFooterSL_Code" ToolTip="SL Account" runat="server">--%>
                                                                                <%--AutoPostBack="true" OnSelectedIndexChanged="ddlFooterFlowType_SelectedIndexChanged">--%>
                                                                                <%--</asp:DropDownList>--%>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- Description --%>
                                                                        <asp:TemplateField HeaderText="Remarks" FooterStyle-Width="20%" ItemStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDescription" ToolTip="Description" Width="100%" runat="server"
                                                                                    Text='<%#Eval("Remarks")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtFooterDescription" ToolTip="Description" TextMode="MultiLine"
                                                                                    Width="75%" Wrap="true" MaxLength="60" onkeyup="maxlengthfortxt(60);" runat="server"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- Amount --%>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Amount" FooterStyle-Width="20%"
                                                                            ItemStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmount" Width="100%" ToolTip="Amount" runat="server" Text='<%#Eval("Amount")%>'
                                                                                    Style="text-align: right"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtFooterAmount" Width="95%" ToolTip="Amount" runat="server" MaxLength="12"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtFooterAmount"
                                                                                    FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtFooterAmount" runat="server" ControlToValidate="txtFooterAmount"
                                                                                    SetFocusOnError="True" ValidationGroup="GridPaymentadj" CssClass="styleMandatoryLabel"
                                                                                    Display="None" ErrorMessage="Enter Amount"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                        <%-- Action --%>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkRemove" Width="100%" ToolTip="Remove from the grid" runat="server"
                                                                                    CommandName="Delete" CausesValidation="false" Text="Remove"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:LinkButton ID="lnkAdd" Width="100%" ToolTip="Add to the grid" OnClick="lnkAddAdjust_Click"
                                                                                    runat="server" ValidationGroup="GridPaymentadj" Text="Add"></asp:LinkButton>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="adjustmentGridtotal" align="center">
                                                            <td align="center">
                                                                <table width="100%" class="stylePagingControl">
                                                                    <tr align="right" class="stylePagingControl">
                                                                        <td align="right" width="80%">
                                                                            <asp:Label ID="lblPaymentAdjust" CssClass="styleDisplayLabel" runat="server" Text="Total"></asp:Label>
                                                                        </td>
                                                                        <td width="15%" align="right" colspan="2">
                                                                            <asp:Label ID="lbltotalPaymentAdjust" ToolTip="Total amount of payment adjustment grid"
                                                                                CssClass="styleDisplayLabel" runat="server" Text="0"></asp:Label>
                                                                        </td>
                                                                        <%-- <td width="5%">
                                                                            &nbsp;
                                                                        </td>--%>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                        <%--Buttons--%>
                                        <tr>
                                            <td align="center">&nbsp;
                                                <asp:Button runat="server" ID="btnSave" Text="Save" ToolTip="Save the Payment request"
                                                    CssClass="styleSubmitButton" OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators('Save');"
                                                    ValidationGroup="Save" />
                                                <%--OnClientClick="return fnConfirmSave();"--%>
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnCancelPayment" ToolTip="Cancel Payment" Text="Cancel Payment"
                                                    CausesValidation="false" CssClass="styleSubmitButton" OnClientClick="return confirm('Do you want to cancel this record?');"
                                                    OnClick="btnCancelPayment_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" ToolTip="Cancel" CausesValidation="false"
                                                    CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                                 &nbsp;
                                                 <asp:Button runat="server" ID="btnPrintVoucher" ToolTip="Print Voucher" Text="Print Voucher" 
                                                    CssClass="styleSubmitButton" OnClick="btnPrintVoucher_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%">
                                        <tr width="100%">
                                            <td width="100%">
                                                <asp:Panel DefaultButton="btnPassword" ID="PnlPassword" Style="display: none" runat="server"
                                                    Height="25%" BackColor="White" BorderStyle="Solid" BorderColor="Black" Width="30%">
                                                    <table width="100%">
                                                        <tr width="100%">
                                                            <td colspan="3" class="stylePageHeading" align="center">
                                                                <asp:Label runat="server" ToolTip="password" Text="Enter your password" ID="lblPasswordHeader"
                                                                    CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr align="center">
                                                            <td>&nbsp;
                                                                <asp:Label ID="lblPassword" ToolTip="password" runat="server" Text="Password:" Font-Bold="true"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="left" class="styleFieldAlign">
                                                                <asp:TextBox ID="txtPassword" ToolTip="password" runat="server" Width="200px" TextMode="Password"
                                                                    MaxLength="30"></asp:TextBox>
                                                            </td>
                                                            <%--<td>
                                        <asp:RequiredFieldValidator ID="rvPassword" runat="server" ErrorMessage="Password must be entered"
                                            ControlToValidate="txtPassword" CssClass="styleLoginLabel" Display="None"></asp:RequiredFieldValidator>
                                    </td>--%>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="3">
                                                                <asp:Button ID="btnPassword" ToolTip="Authenticate password" CausesValidation="false"
                                                                    CssClass="styleSubmitButton" OnClick="btnPassword_Click" Text="Submit" runat="server" />
                                                                &nbsp;
                                                                <asp:Button ID="btnCancelPass" ToolTip="Cancel" CausesValidation="false" OnClick="btnCancelPass_Click"
                                                                    CssClass="styleSubmitButton" Text="Cancel" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="3">
                                                                <asp:Label ID="lblErrorMessagePass" runat="server" CssClass="styleMandatoryLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <cc1:ModalPopupExtender ID="ModalPopupExtenderPassword" runat="server" TargetControlID="chkAccountBased"
                                                    PopupControlID="PnlPassword" BackgroundCssClass="styleModalBackground" DynamicServicePath=""
                                                    Enabled="True">
                                                </cc1:ModalPopupExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ValidationGroup="Save" ID="vs_PaymentRequest" runat="server"
                                                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ValidationGroup="GridPayment" ID="ValidationSummary1" runat="server"
                                                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):  " />
                                                <asp:ValidationSummary runat="server" ID="vsFunderAdd" ValidationGroup="vsFunderAdd"
                                                    HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                                                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ValidationGroup="GridPaymentadj" ID="ValidationSummary2" runat="server"
                                                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ValidationGroup="Print" ID="ValidationSummary3" runat="server"
                                                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ValidationGroup="Show" ID="vsShow" runat="server"
                                                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
<asp:PostBackTrigger ControlID="ddlPayMode" />
</Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                        <cc1:TabPanel runat="server" ID="tpInvoiceLoading" CssClass="tabpan" BackColor="Red">
                            <HeaderTemplate>
                                Invoice Loading
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:UpdatePanel ID="updtpnlInvoiceDtl" runat="server">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:GridView runat="server" ID="grvPaymentInvoiceDtl" Visible="true" RowStyle-HorizontalAlign="Center"
                                                        HeaderStyle-CssClass="styleGridHeader" AutoGenerateColumns="False" OnRowDataBound="grvPaymentInvoiceDtl_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvSNo" Text='<%# Container.DataItemIndex+1%>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="PO ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvPoID" runat="server" Text='<%#Eval("PO_Det_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="PI ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvPIID" runat="server" Text='<%#Eval("PI_Det_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="PI Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvPINo" runat="server" Text='<%#Eval("PI_No")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="PI Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvPIDate" runat="server" Text='<%#Eval("PI_Date")%>' Width="80px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VI ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvVIID" runat="server" Text='<%#Eval("VI_Det_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VI Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvVINo" runat="server" Text='<%#Eval("VI_No")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VI Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvVIDate" runat="server" Text='<%#Eval("VI_Date")%>' Width="80px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RS ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvRSID" runat="server" Text='<%#Eval("PA_SA_Ref_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Rental Schd. No">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvRSNo" runat="server" Text='<%#Eval("RS_No")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="RS Activation Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvRSActDate" runat="server" Text='<%#Eval("RS_Activation_Date")%>' Width="80px"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Vendor ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvVendorID" runat="server" Text='<%#Eval("Vendor_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dealer Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvVendorName" runat="server" Text='<%#Eval("Vendor_Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tranche ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvTrancheID" runat="server" Text='<%#Eval("Tranche_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Lessee Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvCustomerName" runat="server" Text='<%#Eval("Customer_Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tranche Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvTrancheName" runat="server" Text='<%#Eval("Tranche_Name")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Desc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvAssetID" Text='<%# Bind("Asset_ID")%>' runat="server" Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblgdinvAssetDesc" Text='<%# Bind("Asset_Desc")%>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvInvoiceAmount" runat="server" Text='<%#Eval("Invoice_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgdFtrTtlInvoice" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TDS Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvTDSAmount" runat="server" Text='<%#Eval("TDS_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgdFtrTtlTDS" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="WCT Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvWCTAmount" runat="server" Text='<%#Eval("WCT_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgdFtrTtlWCT" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CENVAT Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvCenvatAmount" runat="server" Text='<%#Eval("Cenvat_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgdFtrTtlCenvat" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Actual Retention Amount" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvHoldAmount" runat="server" Text='<%#Eval("Actual_Retention_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Retention Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvEnteredRetntionAmount" runat="server" Text='<%#Eval("Hold_Amount")%>' Visible="false"></asp:Label>
                                                                    <asp:TextBox ID="txtgdinvRetentionAmt" runat="server" Text='<%#Eval("Hold_Amount")%>'
                                                                        Style="text-align: right" Width="90px" AutoPostBack="true" OnTextChanged="txtgdinvRetentionAmt_TextChanged"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbegdinvRetentionAmt" runat="server" TargetControlID="txtgdinvRetentionAmt" Enabled="true"
                                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgdFtrTtlRetention" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvTotalAmount" runat="server" Text='<%#Eval("Total_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgdFtrTtlAmount" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Advance Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvAdvanceAmount" runat="server" Text='<%#Eval("Advance_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Advance Paid Amount" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvAdvancePaidAmount" runat="server" Text='<%#Eval("Advance_Paid_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Paid Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblgdinvPaidAmount" runat="server" Text='<%#Eval("Paid_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Payable Amount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtgdinvPayableAmount" runat="server" Text='<%#Eval("Payable_Amount")%>' AutoPostBack="true"
                                                                        OnTextChanged="txtgdinvPayableAmount_TextChanged" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftbegdinvPayableAmount" runat="server" TargetControlID="txtgdinvPayableAmount" Enabled="true"
                                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:Label ID="lblgdinvPayableAmount" runat="server" Text='<%#Eval("Payable_Amount")%>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblgdFtrTtlPayable" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgbtnDeleteInvoice" runat="server" ImageUrl="~/Images/delete1.png" Height="20px" ToolTip="Remove"
                                                                        OnClick="imgbtnDeleteInvoice_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <uc1:pagenavigator id="Pagenavigator1" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button runat="server" Visible="false" ID="btnremoveall" ToolTip="Remove" Text="Remove All"
                                                        CssClass="styleSubmitButton" OnClick="btnremoveall_Click" />

                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanelPBD" CssClass="tabpan" BackColor="Red" Enabled="false">
                        <HeaderTemplate>
                            Payment Bank Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table width="99%" cellpadding="0" cellspacing="0">
                                        <tr width="100%">
                                            <td class="styleFieldLabel" colspan="4" align="center">
                                                <asp:RadioButtonList ID="RBLCompanyCashorBankAcct" runat="server" ToolTip="Company cash or bank account for Demand draft"
                                                    RepeatDirection="Horizontal" OnSelectedIndexChanged="RBLCompanyCashorBankAcct_SelectedIndexChanged"
                                                    AutoPostBack="true" Visible="false">
                                                    <asp:ListItem Text="Company Cash" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Bank Account" Value="1" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr width="100%">
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblBankName" ToolTip="Bank Name" runat="server" Text="Bank Name" CssClass="styleReqFieldLabel" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlbankname" ToolTip="Bank Name" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlbankname_OnSelectedIndexChanged" />
                                                <asp:RequiredFieldValidator ID="rfvbankname" runat="server" Display="None" ControlToValidate="ddlbankname"
                                                    ValidationGroup="Print" InitialValue="0" ErrorMessage="Select Bank Name" CssClass="styleMandatoryLabel"
                                                    Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvbanknameC" runat="server" Display="None" ControlToValidate="ddlbankname"
                                                    ValidationGroup="PrintC" ErrorMessage="Enter GL Code" CssClass="styleMandatoryLabel"
                                                    Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblAcctnumber" ToolTip="Account Number" runat="server" Text="Account Number"
                                                    CssClass="styleReqFieldLabel" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlAcctNumber" ToolTip="Account Number" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlAcctNumber_OnSelectedIndexChanged" />
                                                <asp:RequiredFieldValidator ID="RFVAcctNumberC" runat="server" Display="None" ControlToValidate="ddlAcctNumber"
                                                    ValidationGroup="PrintC" InitialValue="0" ErrorMessage="Select Account Number"
                                                    CssClass="styleMandatoryLabel" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RFVAcctNumber" runat="server" Display="None" ControlToValidate="ddlAcctNumber"
                                                    ValidationGroup="Print" InitialValue="0" ErrorMessage="Select Account Number"
                                                    CssClass="styleMandatoryLabel" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblIFSC_Code" runat="server" ToolTip="IFSC Code" Text="IFSC Code"
                                                    CssClass="styleReqFieldLabel" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtIFSC_Code" runat="server" ToolTip="IFSC_Code" ReadOnly="true" />
                                                <asp:RequiredFieldValidator ID="RFVIFSC_CodeC" runat="server" Display="None" ControlToValidate="txtIFSC_Code"
                                                    ValidationGroup="PrintC" ErrorMessage="Enter IFSC Code" CssClass="styleMandatoryLabel"
                                                    Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RFVIFSC_Code" runat="server" Display="None" ControlToValidate="txtIFSC_Code"
                                                    ValidationGroup="Print" ErrorMessage="Enter IFSC Code" CssClass="styleMandatoryLabel"
                                                    Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblBankbranch" runat="server" ToolTip="Bank Branch" Text="Bank Branch" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtBankbranch" runat="server" ToolTip="Bank Branch" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblGLcode" runat="server" ToolTip="GL Code" Text="GL Code" CssClass="styleReqFieldLabel" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtGLCode" runat="server" ToolTip="GL Code" ReadOnly="true" />
                                                <asp:RequiredFieldValidator ID="rfvtxtGLCode" runat="server" Display="None" ControlToValidate="txtGLCode"
                                                    ValidationGroup="Print" ErrorMessage="Enter GL Code" CssClass="styleMandatoryLabel"
                                                    Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvtxtGLCodeC" runat="server" Display="None" ControlToValidate="txtGLCode"
                                                    ValidationGroup="PrintC" ErrorMessage="Enter GL Code" CssClass="styleMandatoryLabel"
                                                    Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblSLcode" runat="server" ToolTip="SL Code" Text="SL Code" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtSLCode" runat="server" ToolTip="SL Code" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblInstrumentNumber" runat="server" ToolTip="Instrument Number" Text="Instrument Number"
                                                    CssClass="styleReqFieldLabel" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtInstrumentNumber" runat="server" ToolTip="Instrument Number"
                                                    ReadOnly="true" MaxLength="12" />
                                                <asp:RequiredFieldValidator ID="rfvtxtInstrumentNumber" runat="server" Display="None"
                                                    ControlToValidate="txtInstrumentNumber" ValidationGroup="Print" ErrorMessage="Enter Instrument Number"
                                                    CssClass="styleMandatoryLabel" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvtxtInstrumentNumberC" runat="server" Display="None"
                                                    ControlToValidate="txtInstrumentNumber" ValidationGroup="PrintC" ErrorMessage="Enter Instrument Number"
                                                    CssClass="styleMandatoryLabel" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtInstrumentNumber"
                                                    FilterType="Numbers" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblInstrumentDate" runat="server" ToolTip="Instrument Date" Text="Instrument Date"
                                                    CssClass="styleReqFieldLabel" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtInstrumentDate" ToolTip="Instrument Date" runat="server" AutoPostBack="true"
                                                    OnTextChanged="txtInstrumentDate_OnTextChanged" />
                                                <asp:Image ID="ImageInstrumentDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                    Height="16px" />
                                                <cc1:CalendarExtender ID="CalendarExtenderInstrumentDate" runat="server" Enabled="True"
                                                    PopupButtonID="ImageInstrumentDate" TargetControlID="txtInstrumentDate">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvtxtInstrumentDate" runat="server" Display="None"
                                                    ControlToValidate="txtInstrumentDate" ValidationGroup="Print" ErrorMessage="Enter Instrument Date"
                                                    CssClass="styleMandatoryLabel" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblChequeStatus" ToolTip="Cheque status" runat="server" Text="Cheque Print Status" />
                                            </td>
                                            <td class="styleFieldAlign" valign="middle">
                                                <asp:DropDownList ID="ddlChequeStatus" ToolTip="Cheque status" runat="server" Enabled="false" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblRemarks" runat="server" ToolTip="Remarks" Text="Remarks" />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" ToolTip="Remarks" runat="server" Width="250px" Height="40px"
                                                    MaxLength="100" onkeyup="maxlengthfortxt(100);" />
                                                <asp:RequiredFieldValidator ID="RFVRemarks" runat="server" Display="None" ControlToValidate="txtRemarks"
                                                    ValidationGroup="Print" ErrorMessage="Enter Remarks" CssClass="styleMandatoryLabel"
                                                    Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblPaymentGatewayRefNo" ToolTip="Payment Gateway Reference No" runat="server" Text="Payment Gateway Ref No" />
                                            </td>
                                            <td class="styleFieldAlign" valign="middle">
                                                <asp:TextBox ID="txtPmtGatewayRefNo" runat="server" ToolTip="Payment GateWay Ref No" MaxLength="30" Width="250px"></asp:TextBox>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblFavouringName" ToolTip="Favouring Name" runat="server" Text="Favouring Name" CssClass="styleReqFieldLabel" />
                                            </td>
                                            <td class="styleFieldAlign" valign="middle">
                                                <asp:TextBox ID="txtFavouringName" runat="server" ToolTip="Favouring Name" MaxLength="60" Width="250px"></asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="95%">
                                        <tr align="center">
                                            <td colspan="4" align="center">
                                                <%--<asp:Button runat="server" ID="btnPrintVoucher" ToolTip="Print Voucher" Text="Print Voucher" 
                                                    CssClass="styleSubmitButton" OnClick="btnPrintVoucher_Click" />--%>
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnPrintCheque" ToolTip="Print Cheque" Text="Print Cheque"
                                                    CssClass="styleSubmitButton" ValidationGroup="Print" OnClick="btnPrintCheque_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnCoveringLetter" ToolTip="Covering Letter" Text="Covering Letter"
                                                    CssClass="styleSubmitButton" OnClick="btnCoveringLetter_Click" Visible="false" />
                                                <%--&nbsp;
                                                <asp:Button runat="server" ID="btncrtVoucher" ToolTip="Covering Letter" Text="RPT Voucher"
                                                    CssClass="styleSubmitButton" OnClick="btncrtVoucher_Click" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:ValidationSummary ValidationGroup="Print" ID="ValidationSummary4" runat="server"
                                                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>

    <cc1:ModalPopupExtender ID="moePoInvoiceDtls" runat="server" TargetControlID="btnModal" PopupControlID="pnlPoInvoiceDtls"
        BackgroundCssClass="styleModalBackground" Enabled="true" />
    <asp:Panel ID="pnlPoInvoiceDtls" Style="display: none; vertical-align: middle;" runat="server"
        BorderStyle="Solid" BackColor="White" Width="95%" ScrollBars="Auto">
        <div id="GridViewContainer" runat="server" style="max-height: 500px; overflow: auto">
            <asp:UpdatePanel ID="updtPnlPoInvoiceDtls" runat="server">
                <ContentTemplate>
                    <table width="100%" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblInvoiceSortBy" runat="server" Text="Sort By" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInvoiceSortBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceSortBy_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvSrchText" runat="server" ToolTip="Invoice Number" Visible="false" />
                                            <uc3:Suggest ID="ddlInvSrchTxt" runat="server" ServiceMethod="GetInvSrchLst" Width="250px" />
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnInvoiceGo" ToolTip="Get Invoice" Text="GO"
                                                CssClass="styleSubmitButton" OnClick="btnInvoiceGo_Click" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvoiceStartDtae" runat="server" Text="Invoice Start Date" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvoiceStartDate" runat="server" ToolTip="Invoice Start Date" Width="90px" Visible="false" />
                                            <asp:Image ID="imgInvoiceStartDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                Height="16px" Visible="false" />
                                            <cc1:CalendarExtender ID="ceInvoiceStartDate" runat="server" Enabled="True" OnClientShown="calendarShown"
                                                PopupButtonID="imgInvoiceStartDate" TargetControlID="txtInvoiceStartDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="rfvInvoiceStartDate" runat="server" Display="None" ControlToValidate="txtInvoiceStartDate"
                                                ValidationGroup="vsPOSearch" ErrorMessage="Enter Invoice Start Date" CssClass="styleMandatoryLabel"
                                                Enabled="false" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblInvoiceEndDate" runat="server" Text="Invoice End Date" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInvoiceEndDate" runat="server" ToolTip="Invoice Start Date" Width="90px" Visible="false" />
                                            <asp:Image ID="imgInvoiceEndDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                Height="16px" Visible="false" />
                                            <cc1:CalendarExtender ID="ceInvoiceEndDate" runat="server" Enabled="True" OnClientShown="calendarShown"
                                                PopupButtonID="imgInvoiceEndDate" TargetControlID="txtInvoiceEndDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="rfvInvoiceEndDate" runat="server" Display="None" ControlToValidate="txtInvoiceEndDate"
                                                ValidationGroup="vsPOSearch" ErrorMessage="Enter Invoice End Date" CssClass="styleMandatoryLabel"
                                                Enabled="false" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="right" valign="top">
                                <asp:ImageButton ID="imgPopupClose" runat="server" ImageUrl="~/Images/delete1.png" Height="20px" ToolTip="Close"
                                    OnClick="imgPopupClose_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grvLoadInvoiceDtl" runat="server" AutoGenerateColumns="false" Width="2000px"
                                            OnRowDataBound="grvLoadInvoiceDtl_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="PO No" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPoID" Text='<%# Bind("PO_Det_ID")%>' runat="server" Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PI Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPIID" Text='<%# Bind("PI_Det_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblPINo" Text='<%# Bind("PI_No")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PI Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPIDate" Text='<%# Bind("PI_Date")%>' runat="server" Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VI Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorInvoiceID" Text='<%# Bind("VI_Det_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblVendorInvoiceNo" Text='<%# Bind("VI_No")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VI Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVIDate" Text='<%# Bind("VI_Date")%>' runat="server" Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dealer" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorID" Text='<%# Bind("Vendor_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblVendorName" Text='<%# Bind("Vendor_Name")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRentalScheduleID" Text='<%# Bind("PA_SA_Ref_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblRentalScheduleNo" Text='<%# Bind("RS_No")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RS Activation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSActvDate" Text='<%# Bind("RS_Activation_Date")%>' runat="server" Width="80px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Lessee Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSLesseeName" Text='<%# Bind("Customer_Name")%>' runat="server" Width="120px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tranche Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrancheID" Text='<%# Bind("Tranche_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblTrancheNo" Text='<%# Bind("Tranche_Name")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Note No" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNoteID" Text='<%# Bind("Note_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblNoteNo" Text='<%# Bind("Note_No")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Desc">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetID" Text='<%# Bind("Asset_ID")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAssetDesc" Text='<%# Bind("Asset_Desc")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceAmount" Text='<%# Bind("Invoice_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTtlInvoice" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TDS Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTDSAmount" Text='<%# Bind("TDS_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTtlTDS" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="WCT Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWCTAmount" Text='<%# Bind("WCT_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTtlWCT" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CENVAT Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCenvatAmount" Text='<%# Bind("Cenvat_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTtlCenvat" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Retention Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHoldAmount" Text='<%# Bind("Actual_Retention_Amount")%>' runat="server" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtRetentionAmt" runat="server" MaxLength="16" Text='<%# Bind("Hold_Amount")%>'
                                                            Style="text-align: right" Width="90px" AutoPostBack="true" OnTextChanged="txtRetentionAmt_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftbeRetentionAmt" runat="server" TargetControlID="txtRetentionAmt" Enabled="true"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTtlRetension" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalAmount" Text='<%# Bind("Total_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTtlAmount" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Advance Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdvanceAmount" Text='<%# Bind("Advance_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Advance Paid Amount" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdvancePaidAmount" Text='<%# Bind("Advance_Paid_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Paid Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPaidAmount" Text='<%# Bind("Paid_Amount")%>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="120px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select All">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSelectAll" runat="server" Text="Select All"></asp:Label>
                                                        <br></br>
                                                        <asp:CheckBox ID="chkSelectAllInvoices" runat="server" OnCheckedChanged="chkSelectAllInvoices_CheckedChanged" AutoPostBack="true" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectIndicator" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectIndicator_CheckedChanged"
                                                            Checked='<%# Eval("Chk_Selected").ToString() == "1" ?  true:false %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount to be disbursed">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPayableAmount" runat="server" MaxLength="16" Text='<%# Bind("Payable_Amount")%>'
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" AutoPostBack="true" Style="text-align: right"
                                                            OnTextChanged="txtPayableAmount_TextChanged"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftbePayableAmount" runat="server" TargetControlID="txtPayableAmount" Enabled="true"
                                                            FilterType="Custom,Numbers" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTtlDisposed" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <ItemStyle Width="120px" />
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr align="center">
                            <td colspan="2">
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <span runat="server" id="lblPagingErrorMessage" style="color: Red; font-size: medium"></span>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" ID="btnAddPOInvoice" CssClass="styleSubmitButton" Text="Move" OnClick="btnAddPOInvoice_Click" />
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:ValidationSummary runat="server" ID="vsPOSearch" ValidationGroup="vsPOSearch"
            HeaderText="Please correct the following validation(s):" Height="10px" CssClass="styleMandatoryLabel"
            Width="500px" ShowMessageBox="false" ShowSummary="true" />
        <input type="hidden" id="hdnSortDirection" runat="server" />
        <input type="hidden" id="hdnSortExpression" runat="server" />
        <input type="hidden" id="hdnSearch" runat="server" />
        <input type="hidden" id="hdnOrderBy" runat="server" />
    </asp:Panel>
    <asp:Button ID="btnModal" Style="display: none" runat="server" />

</asp:Content>
