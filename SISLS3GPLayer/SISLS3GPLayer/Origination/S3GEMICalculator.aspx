<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GEMICalculator.aspx.cs" EnableEventValidation="true" Inherits="S3GEMICalculator"
    Title="EMI Calculator" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function fnValidProspectName(txtProspectName) {
            if (txtProspectName.value == "0") {
                alert('Prospect Name should not be 0');
                document.getElementById(txtProspectName.id).focus();
            }
        }

        function fnLoadCustomer() {
            document.getElementById('<%=btnLoadCustomer.ClientID%>').click();
        }

        function FunpriGetAssetCost(input) {
            var txtAssetCost = document.getElementById('<%=txtAssetCost.ClientID%>');
            if (input.value != '') {
                txtAssetCost.value = input.value;
            }
            FungetMarginAmt();
        }


        function FunCalResidual(ResidualValue, type) {

            if (ResidualValue.value != '') {

                var FinanceAmt = document.getElementById('<%=txtAmount.ClientID%>');
                var ResidualValueAmt = document.getElementById('<%=txtResidualValueAmt.ClientID%>');
                var txtResidualPer = document.getElementById('<%=txtResidualPer.ClientID%>');
                if (type == 'Val') {
                    ResidualValueAmt.value = '';
                    if (FinanceAmt.value != '') {
                        ResidualValueAmt.value = parseFloat((FinanceAmt.value * ResidualValue.value) / 100).toFixed(2);
                    }
                }
                else if (type == 'Amt') {
                    txtResidualPer.value = '';
                    if (FinanceAmt.value != '') {
                        txtResidualPer.value = parseFloat((ResidualValueAmt.value * 100) / FinanceAmt.value).toFixed(2);
                    }
                }
            }
        }

        function FungetMarginAmt() {
            var txtAssetCost = document.getElementById('<%=txtAssetCost.ClientID%>');
            var txtMarginMoneyPer_Cashflow = document.getElementById('<%=txtMarginMoneyPer_Cashflow.ClientID%>');
            var txtMarginMoneyAmount_Cashflow = document.getElementById('<%=txtMarginMoneyAmount_Cashflow.ClientID%>');
            if (txtAssetCost.value != '' && txtMarginMoneyPer_Cashflow.value != '') {
                txtMarginMoneyAmount_Cashflow.value = parseFloat((txtAssetCost.value * txtMarginMoneyPer_Cashflow.value) / 100).toFixed(2);
            }

        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="EMI Calculator" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel runat="server" Visible="false" ID="panInputCriteria" CssClass="stylePanel" GroupingText="Input Criteria">
                            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                <tr class="styleGridHeader">
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Line of Business"  ToolTip="Line of Business"  ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business"  AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Query Number" ToolTip="Query Number" ID="lblQueryNo" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtQueryNo" runat="server" ToolTip="Query Number" Width="157px" MaxLength="4" Style="text-align: right;"
                                            AutoPostBack="True" OnTextChanged="txtQueryNo_TextChanged"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftTxtQueryNo" runat="server" Enabled="True" FilterType="Numbers"
                                            TargetControlID="txtQueryNo" ValidChars=" -">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Date" ID="lblDate" ToolTip="Date" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <%--<asp:TextBox ID="txtDate" runat="server" AutoPostBack="True" Width="135px" OnTextChanged="txtDate_TextChanged"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtDate" runat="server" ToolTip="Date" Width="135px"></asp:TextBox>
                                        <%--   <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false"/>
                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                            TargetControlID="txtDate" PopupButtonID="Image1" ID="ceDate" Enabled="false">
                        </cc1:CalendarExtender>--%>
                                        <asp:HiddenField ID="hdnPLR" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td width="98%">
                        <asp:Panel runat="server" ID="PnlAccInfo" CssClass="stylePanel" GroupingText="Financial Information"  ToolTip="Financial Information">
                            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Facility Amount" ToolTip="Facility Amount" ID="lblAmount" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAmount" runat="server" MaxLength="12" ToolTip="Facility Amount" onfocusout="FunpriGetAssetCost(this);"
                                                        onkeypress="fnAllowNumbersOnly(false,false,this)" onkeyup="fnNotAllowPasteSpecialChar(this,'special')"
                                                        Style="text-align: right"></asp:TextBox>
                                                    <%--AutoPostBack="True" OnTextChanged="txtAmount_TextChanged"--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Tenure Type/Tenure" ToolTip="Tenure Type/Tenure" ID="lblTenure" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlTenureType"  ToolTip="Tenure Type/Tenure" runat="server" Width="100px" >
                                                        <%--OnSelectedIndexChanged="ddlTenureType_SelectedIndexChanged"--%>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtTenure"  ToolTip="Tenure Type/Tenure" runat="server" Width="54px" MaxLength="3" Style="text-align: right;"></asp:TextBox>
                                                    <%--AutoPostBack="True" OnTextChanged="txtTenure_TextChanged"--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblReturnPattern" runat="server" Text="Return Pattern" ToolTip="Return Pattern"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlReturnPattern" runat="server" Width="165px" AutoPostBack="true"
                                                        ToolTip="Return Pattern" OnSelectedIndexChanged="ddlReturnPattern_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblRepaymentMode" runat="server" Text="Repayment Mode" CssClass="styleDisplayLabel"
                                                        ToolTip="Repayment Mode"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlRepaymentMode" runat="server" Width="165px" AutoPostBack="true"
                                                        ToolTip="Repayment Mode" OnSelectedIndexChanged="ddlRepaymentMode_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Asset / LAN"  ToolTip="Asset / LAN" ID="lblAssetLAN" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:DropDownList ID="ddlAssetRegister" ToolTip="Asset / LAN" runat="server" Width="165px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlAssetRegister_SelectedIndexChanged" onchange="ddl_itemchanged(this);">
                                                    </asp:DropDownList>
                                                    <asp:RadioButtonList ID="rdblAssetType"  ToolTip="Asset / LAN" AutoPostBack="true" Visible="false" runat="server"
                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="rdblAssetType_SelectedIndexChanged">
                                                        <asp:ListItem Value="0" Text="New" Selected="True" />
                                                        <asp:ListItem Value="1" Text="Existing" />
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFlatRate" runat="server" Text="Rate" ToolTip="Rate"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" id="tdRate">
                                                    <asp:TextBox ID="txtFlatRate" Width="80px" runat="server" Style="text-align: right"
                                                        MaxLength="10" ToolTip="Flat Rate"></asp:TextBox>
                                                    <%--AutoPostBack="True" OnTextChanged="txtRecoveryPatternYear1_TextChanged"--%>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                        FilterType="Custom,Numbers" TargetControlID="txtFlatRate" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFunder" runat="server" Text="Funder Rate" ToolTip="Funder Rate"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFunder" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Funder Rate" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCustPTPA" runat="server" Text="Cust PTPF" ToolTip="Cust PTPF"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCustPTPA" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Cust PTPA" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFunderReceipt" runat="server" Text="Funder rcpt" ToolTip="Funder Receipt"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFunderReceipt" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Funder Receipt" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblOnetimeFee" runat="server" Text="Onetime Fee" ToolTip="Onetime Fee"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOnetimeFee" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Onetime Fee" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInterimRent" runat="server" Text="Interim Rent" ToolTip="Interim Rent"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtInterimRent" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Interim Rent" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSecurityDeposit" runat="server" Text="Security Deposit" ToolTip="Security Deposit"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtSecurityDeposit" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Security Deposit" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDepreciationStructure" runat="server" Text="Depreciation Structure"
                                                        ToolTip="Depreciation Structure" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:CheckBox ID="chkDepreStrucure" Visible="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkDepreStrucure_CheckedChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ToolTip="Customer Name"  Text="Customer Name" ID="lblProspectName" CssClass="styleDisplayLabel"></asp:Label>

                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtProspectName" ToolTip="Customer Name" runat="server" MaxLength="100" onfocusOut="fnValidProspectName(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftxtCustomerName" ValidChars=" ." TargetControlID="txtProspectName"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                                                        OnClick="btnLoadCustomer_OnClick" CausesValidation="false" />
                                                    <uc2:LOV ID="ucCustomerCodeLov" runat="server" strLOV_Code="CMD" />
                                                    <asp:CheckBox ID="chkExisting" runat="server" AutoPostBack="true" ToolTip="Existing Customer"
                                                        OnCheckedChanged="rdblCustomer_SelectedIndexChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountingIRR"  runat="server" Text="CIRR1" ToolTip="CIRR1"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAccountingIRR" Width="80px" runat="server" ReadOnly="true" Style="text-align: right"
                                                        MaxLength="10" ToolTip="CIRR1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCompanyIRR" runat="server" Text="CIRR2" ToolTip="CIRR2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCompanyIRR" runat="server" Width="80px" ReadOnly="true" Style="text-align: right"
                                                        MaxLength="10" ToolTip="CIRR2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblgapPer" runat="server" Text="Gap %" ToolTip="Gap %"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtgapPer" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Gap %" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblRentalPV" runat="server" Text="Rental PV" ToolTip="Rental PV"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRentalPV" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Rental PV" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInvestment" runat="server" Text="Investment" ToolTip="Investment"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtInvestment" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Investment" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGAP" runat="server" Text="GAP" ToolTip="GAP"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtGAP" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="GAP" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCash" runat="server" Text="Cash" ToolTip="Cash"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCash" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Cash" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCashflow" runat="server" Text="Cashflow" ToolTip="Cashflow"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCashflow" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Cashflow" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCashflowPer" runat="server" Text="Cashflow %" ToolTip="Cashflow %"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCashflowPer" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Cashflow %" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblTotalLease" runat="server" Text="Total Lease Rent" ToolTip="Total Lease Rent"
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtTotalLease" runat="server" Width="100px" ReadOnly="true" Style="text-align: right"
                                                        ToolTip="Total Lease Rent" Visible="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInterestEarned" runat="server" Text="Interest Earned" ToolTip="InterestEarned"
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtInterestEarned" runat="server" Width="100px" Style="text-align: right"
                                                        ToolTip="Interest Earned" Visible="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtInterestEarned"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBusinessIRR" runat="server" Text="Business IRR" ToolTip="Business IRR" Visible="false"></asp:Label>
                                                    <span class="styleMandatory" id="spnBusinessIRR" runat="server" visible="false"></span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtBusinessIRR" runat="server" Width="80px" ReadOnly="true" Style="text-align: right;"
                                                        MaxLength="10" ToolTip="Business IRR" AutoPostBack="True" Visible="false" OnTextChanged="txtRecoveryPatternYear1_TextChanged"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Visible="false" Text="Address" ID="lblAddress" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAddress" Visible="false" runat="server" MaxLength="300" TextMode="MultiLine"
                                                        Wrap="true" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                                    <%-- <cc1:FilteredTextBoxExtender ID="ftAddress" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                            TargetControlID="txtAddress" ValidChars=" ">
                        </cc1:FilteredTextBoxExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Visible="false" Text="E-Mail" ID="lblEmail" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEmail" Visible="false" runat="server" MaxLength="30"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Visible="false" Text="Mobile" ID="lblMobile" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMobile" Visible="false" runat="server" MaxLength="12"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="fttxtMobile" runat="server" Enabled="True" FilterType="Numbers"
                                                        TargetControlID="txtMobile" ValidChars=" -">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Button ID="btnCalculate" ToolTip="Calculate" runat="server" Text="Calculate" CssClass="styleSubmitButton"
                                                        CausesValidation="true" OnClick="btnCalculate_Click" ValidationGroup="btnCalculate" />
 &nbsp;
                                                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitShortButton"
                                                        CausesValidation="false" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />

                                                    <asp:Button ID="btnEmail" runat="server" Text="Email" CssClass="styleSubmitShortButton"
                                                        ValidationGroup="btnEmail" CausesValidation="true" OnClick="btnEmail_Click" Visible="false" />
                                                    &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="styleSubmitShortButton"
                                                        ValidationGroup="btnPrint" CausesValidation="true" OnClick="btnPrint_Click" />&nbsp;
                                                      <asp:Button ID="btnPreView" runat="server" Text="Preview" Visible="false" CssClass="styleSubmitShortButton"
                                                          ValidationGroup="btnEmail" CausesValidation="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height: 1px;">
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <%-- <tr style="height: 1px;">
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>--%>
                <tr>
                    <td align="left" colspan="4">
                        <div style="margin-right: 2px; margin-left: 2px;">
                            <cc1:TabContainer ID="tcEMI" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                Width="100%" TabStripPlacement="top">
                                <cc1:TabPanel runat="server" HeaderText="General" ID="TabPanel2" Width="50px" CssClass="tabpan"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Terms
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="Panel2" ScrollBars="Auto" CssClass="stylePanel" GroupingText="Terms">
                                            <div style="overflow: auto; width: 98%; height: 175px; padding-left: 10px;">
                                                <table width="96%">
                                                    <tr class="styleGridHeader">
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRateType" runat="server" Text="Rate Type" ToolTip="Rate Type" Visible="False"></asp:Label>
                                                            <asp:Label ID="lblTime" runat="server" Text="Time" ToolTip="Time" CssClass="styleDisplayLabel"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlRateType" Width="165px" runat="server" AutoPostBack="True"
                                                                ToolTip="Rate Type" Visible="False" OnSelectedIndexChanged="ddlRateType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="ddlTime" runat="server" ToolTip="Time" Width="165px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlTime_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvTime" runat="server" Display="None" ControlToValidate="ddlTime"
                                                                ValidationGroup="btnCalculate" ErrorMessage="Select the Time" InitialValue="0"
                                                                CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel" width="25%">
                                                            <asp:Label ID="lblRecoveryPatternYear1" runat="server" Text="Year 1" ToolTip="Year1"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtRecoveryPatternYear1" runat="server" Style="text-align: right"
                                                                onkeypress="fnAllowNumbersOnly(true,false,this)" MaxLength="10" ToolTip="Year1">
                                                                <%--AutoPostBack="True" OnTextChanged="txtRecoveryPatternYear1_TextChanged">--%>
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="styleGridHeader">
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblFrequency" runat="server" Text="Frequency" CssClass="styleDisplayLabel"
                                                                ToolTip="Frequency"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlFrequency" runat="server" Width="165px" AutoPostBack="True"
                                                                ToolTip="Tenure Type" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" Display="None" ControlToValidate="ddlFrequency"
                                                                ValidationGroup="btnCalculate" ErrorMessage="Select the Frequency" InitialValue="0"
                                                                CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRecoveryPatternYear2" runat="server" Text="Year 2" ToolTip="Year2"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtRecoveryPatternYear2" runat="server" Style="text-align: right"
                                                                onkeypress="fnAllowNumbersOnly(true,false,this)" MaxLength="10" ToolTip="Year2">
                                                               <%-- AutoPostBack="True" OnTextChanged="txtRecoveryPatternYear1_TextChanged">--%>
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Text="Asset Cost" ID="lblAssetCost" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtAssetCost" runat="server" onfocusout="FungetMarginAmt();" ReadOnly="True"
                                                                onkeypress="fnAllowNumbersOnly(false,false,this)" onkeyup="fnNotAllowPasteSpecialChar(this,'special')"
                                                                Style="text-align: right"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvAssetCost" runat="server" Display="None" ControlToValidate="txtAssetCost"
                                                                ValidationGroup="btnCalculate" ErrorMessage="Enter the Asset Cost" CssClass="styleMandatoryLabel"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRecoveryPatternYear3" runat="server" Text="Year 3" ToolTip="Year3"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtRecoveryPatternYear3" runat="server" Style="text-align: right"
                                                                onkeypress="fnAllowNumbersOnly(true,false,this)" MaxLength="10" ToolTip="Year 3">
                                                                <%--AutoPostBack="True" OnTextChanged="txtRecoveryPatternYear1_TextChanged">--%>
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Text="Residual Value %" ID="lblResidualPer" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtResidualPer" runat="server" MaxLength="7" Style="text-align: right;"
                                                                ReadOnly="True" onchange="FunCalResidual(this,'Val');"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftResidualPer" runat="server" TargetControlID="txtResidualPer"
                                                                FilterType="Custom, Numbers" Enabled="True" ValidChars=".">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvResidualPer" runat="server" Display="None" ControlToValidate="txtResidualPer"
                                                                ValidationGroup="btnCalculate" ErrorMessage="Enter the Residual Value%" CssClass="styleMandatoryLabel"
                                                                SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRecoveryPatternRemaining" runat="server" Text="Remaining" ToolTip="Remaining"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtRecoveryPatternYearRest" runat="server" Style="text-align: right"
                                                                MaxLength="10" ToolTip="Rest" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                                <%--AutoPostBack="True" OnTextChanged="txtRecoveryPatternYear1_TextChanged">--%>
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Text="Residual Amount" ID="Label2" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtResidualValueAmt" ReadOnly="True" runat="server" MaxLength="9"
                                                                Style="text-align: right;" onchange="FunCalResidual(this,'Amt');"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftResidualValueAmt" runat="server" TargetControlID="txtResidualValueAmt"
                                                                FilterType="Custom, Numbers" Enabled="True" ValidChars=".">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvResidualValueAmt" runat="server" Display="None"
                                                                ControlToValidate="txtResidualValueAmt" ValidationGroup="btnCalculate" ErrorMessage="Enter the Residual Value Amount"
                                                                CssClass="styleMandatoryLabel" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Text="Margin %" ID="lblMargin" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <%--OnTextChanged="txtMarginMoneyPer_Cashflow_OnTextChanged" AutoPostBack="True"--%>
                                                            <asp:TextBox ID="txtMarginMoneyPer_Cashflow" onchange="FungetMarginAmt();" MaxLength="7"
                                                                runat="server" onkeypress="fnAllowNumbersOnly(false,true,this)" Style="text-align: right"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Text="Margin Amount" ID="lblMarginAmount" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtMarginMoneyAmount_Cashflow" runat="server" onkeyup="fnNotAllowPasteSpecialChar(this,'special')"
                                                                onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="7" Style="text-align: right"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Visible="false" Text="Depreciation Rate" ID="lblDepRate" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtDepRate" runat="server" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                onkeyup="fnNotAllowPasteSpecialChar(this,'special')" Style="text-align: right" Visible="false"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvDepreRate" runat="server" Display="None" ControlToValidate="txtDepRate"
                                                                ValidationGroup="btnCalculate" ErrorMessage="Enter the Depreciation Rate" CssClass="styleMandatoryLabel"
                                                                SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel" width="25%">
                                                            <asp:Label runat="server" Visible="false" Text="Corporate Tax" ID="lblCorporateTax" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtCorporateTax" onkeypress="fnAllowNumbersOnly(true,false,this)" Visible="false"
                                                                runat="server" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtCorporateTax_TextChanged"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvCorpTax" runat="server" Display="None" ControlToValidate="txtCorporateTax"
                                                                ValidationGroup="btnCalculate" ErrorMessage="Enter the Corporate Tax" CssClass="styleMandatoryLabel"
                                                                SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" ID="TabPanel1" Width="50px" CssClass="tabpan"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Cash Flows
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="div5" Width="100%" ScrollBars="Auto" CssClass="stylePanel"
                                            GroupingText="Cash flow details">
                                            <div style="overflow: auto; width: 98%; height: 175px; padding-left: 10px;">
                                                <asp:GridView ID="gvOutFlow" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                    Width="60%" OnRowDeleting="gvOutFlow_RowDeleting" OnRowCreated="gvOutFlow_RowCreated">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Flow Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFlowType" runat="server" Text='<%# Bind("FlowDesc") %>'></asp:Label>
                                                                <asp:Label ID="lblFlowId" runat="server" Text='<%# Bind("FlowType") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlFlowType" runat="server" Width="200px" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlFlowType_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDate_GridOutflow" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtDate_GridOutflow" runat="server" Width="100px"> </asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtenderSD_OutflowDate" runat="server" Enabled="True"
                                                                    TargetControlID="txtDate_GridOutflow" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtDate_GridOutflow" runat="server" ControlToValidate="txtDate_GridOutflow"
                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" ErrorMessage="Enter the Outflow Date"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cash flow Id" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOutflowid" runat="server" Text='<%# Bind("CashOutFlowID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cash flow Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOutflowDesc" runat="server" Text='<%# Bind("CashOutFlow") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlOutflowDesc" runat="server" Width="200px">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlOutflowDesc" runat="server" ControlToValidate="ddlOutflowDesc"
                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" SetFocusOnError="True"
                                                                    InitialValue="-1" ErrorMessage="Select Cash Outflow"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAmount_Outflow" runat="server" Text='<%# Bind("Amount") %>' Style="text-align: right"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtAmount_Outflow" runat="server" MaxLength="10" Style="text-align: right"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Outflow" runat="server" FilterType="Numbers"
                                                                    TargetControlID="txtAmount_Outflow">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtAmount_Outflow" runat="server" ControlToValidate="txtAmount_Outflow"
                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" SetFocusOnError="True"
                                                                    ErrorMessage="Enter the Outflow amount"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccountingIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBusinessIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblComapnyIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                    Text="Remove">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAddOut" runat="server" Text="Add" OnClick="CashOutflow_AddRow_OnClick"
                                                                    CausesValidation="true" ValidationGroup="TabOutflow" CssClass="styleGridShortButton"
                                                                    Width="50px" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div id="div14" style="overflow: auto; height: 100%; width: 98%" runat="server">
                                                <asp:ValidationSummary ID="vs_TabOutflow" runat="server" CssClass="styleMandatoryLabel"
                                                    Width="90%" ValidationGroup="TabOutflow" HeaderText="Correct the following validation(s):  " />
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbCashFlows" Width="50px" CssClass="tabpan"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Repayment Schedule
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel1" runat="server" Width="100%" Height="100%" CssClass="stylePanel"
                                            GroupingText="Repayment Schedule">
                                            <div style="overflow: auto; width: 98%; height: 175px; padding-left: 10px;">
                                                <asp:GridView ID="gvRepaymentDetails" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" ShowFooter="True" OnRowDeleting="gvRepaymentDetails_RowDeleting"
                                                    OnRowDataBound="gvRepaymentDetails_RowDataBound" OnRowCreated="gvRepaymentDetails_RowCreated">
                                                    <Columns>
                                                        <asp:BoundField DataField="slno" HeaderText="Sl.No" />
                                                        <asp:TemplateField HeaderText="Repayment CashFlowId" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCashFlowId" runat="server" Text='<%# Bind("CashFlowId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Repayment CashFlow">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCashFlow" runat="server" Text='<%# Bind("CashFlow") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlRepaymentCashFlow_RepayTab" runat="server" Width="200px"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlRepaymentCashFlow_RepayTab" runat="server"
                                                                    ControlToValidate="ddlRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                    Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" InitialValue="-1"
                                                                    ErrorMessage="Select a Repayment cashflow">
                                                                </asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Per Installment Amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPerInstallmentAmount_RepayTab" runat="server" Text='<%# Bind("PerInstall") %>'
                                                                    Style="text-align: right"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtPerInstallmentAmount_RepayTab" runat="server" Width="120px" MaxLength="7"
                                                                    Style="text-align: right"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                    FilterType="Numbers" TargetControlID="txtPerInstallmentAmount_RepayTab">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                    ControlToValidate="txtPerInstallmentAmount_RepayTab" CssClass="styleMandatoryLabel"
                                                                    Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the Per installment amount"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Breakup Percentage" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBreakup_RepayTab" runat="server" Text='<%# Bind("Breakup") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtBreakup_RepayTab" runat="server" Width="100px" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                    Style="text-align: right"> </asp:TextBox>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From Installment">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFromInstallment_RepayTab" runat="server" Text='<%# Bind("FromInstall") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFromInstallment_RepayTab" runat="server" Width="80px" MaxLength="3"
                                                                    Text="1" ReadOnly="true" Style="text-align: right"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtFromInstallment_RepayTab" runat="server"
                                                                    FilterType="Numbers" TargetControlID="txtFromInstallment_RepayTab">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtFromInstallment_RepayTab" runat="server" ControlToValidate="txtFromInstallment_RepayTab"
                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                    SetFocusOnError="True" ErrorMessage="Enter the From installment"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Installment">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblToInstallment_RepayTab" runat="server" Text='<%# Bind("ToInstall") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtToInstallment_RepayTab" runat="server" Width="80px" MaxLength="3"
                                                                    Style="text-align: right"> </asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtToInstallment_RepayTab" runat="server"
                                                                    FilterType="Numbers" TargetControlID="txtToInstallment_RepayTab">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvtxtToInstallment_RepayTab" runat="server" ControlToValidate="txtToInstallment_RepayTab"
                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                    SetFocusOnError="True" ErrorMessage="Enter the To installment"></asp:RequiredFieldValidator>
                                                                <asp:CompareValidator ID="cmpvFromTOInstall" runat="server" ErrorMessage="To installment should be greater than From installment"
                                                                    ControlToValidate="txtToInstallment_RepayTab" ControlToCompare="txtFromInstallment_RepayTab"
                                                                    Display="None" ValidationGroup="TabRepayment1" Type="Currency" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '></asp:Label>
                                                                <asp:TextBox ID="txRepaymentFromDate" runat="server" Visible="false" BackColor="Navy"
                                                                    ForeColor="White" Font-Names="calibri" Font-Size="12px" Width="100px" Style="color: White"
                                                                    Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '
                                                                    AutoPostBack="True" OnTextChanged="txRepaymentFromDate_TextChanged"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calext_FromDate" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                                    TargetControlID="txRepaymentFromDate">
                                                                </cc1:CalendarExtender>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtfromdate_RepayTab" runat="server" Width="100px"> </asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtenderSD_fromdate_RepayTab" runat="server" Enabled="True"
                                                                    OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" TargetControlID="txtfromdate_RepayTab">
                                                                </cc1:CalendarExtender>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTODate_ReapyTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ToDate")).ToString(strDateFormat) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtToDate_RepayTab" runat="server" Width="100px" Visible="false"> </asp:TextBox>
                                                                <cc1:CalendarExtender ID="CalendarExtenderSD_ToDate_RepayTab" runat="server" Enabled="True"
                                                                    OnClientDateSelectionChanged="checkDate_PrevSystemDate" TargetControlID="txtToDate_RepayTab">
                                                                </cc1:CalendarExtender>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnRemoveRepayment" CausesValidation="false" runat="server" CommandName="Delete"
                                                                    Text="Remove" Visible="false">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAddRepayment" runat="server" Text="Add" CausesValidation="true"
                                                                    CssClass="styleGridShortButton" OnClick="Repayment_AddRow_OnClick" ValidationGroup="TabRepayment1"
                                                                    Width="50px"></asp:Button>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAccIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBusIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblComIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Structure
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel ID="PanRepay" CssClass="stylePanel" runat="server" Width="100%" Height="100%"
                                            GroupingText="Repayment Structure">
                                            <div style="overflow: auto; width: 98%; height: 175px; padding-left: 10px;">
                                                <asp:GridView ID="grvRepayStructure" runat="server" AutoGenerateColumns="false" Width="98%">
                                                    <Columns>
                                                        <asp:BoundField DataField="InstallmentNo" HeaderText="Installment No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="From_Date" HeaderText="From Date" ItemStyle-HorizontalAlign="Left">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="To_Date" HeaderText="To Date" ItemStyle-HorizontalAlign="Left">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Installment_Date" HeaderText="Installment Date" ItemStyle-HorizontalAlign="Left">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NoofDays" HeaderText="No Of days" ItemStyle-HorizontalAlign="Right">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount" ItemStyle-HorizontalAlign="Right">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Charge" HeaderText="Finance Charges" ItemStyle-HorizontalAlign="Right"
                                                            Visible="true">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="PrincipalAmount" HeaderText="Principal Amount" ItemStyle-HorizontalAlign="Right"
                                                            Visible="true">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <%--  <asp:BoundField DataField="CurrBalance" HeaderText="CurrBalance" ItemStyle-HorizontalAlign="Right"
                                        Visible="true">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    
                                      <asp:BoundField DataField="PrevBalance" HeaderText="PrevBalance" ItemStyle-HorizontalAlign="Right"
                                        Visible="true">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>--%>
                                                        <asp:BoundField DataField="Insurance" Visible="false" HeaderText="Insurance" ItemStyle-HorizontalAlign="Right">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Others" HeaderText="Other Amount" ItemStyle-HorizontalAlign="Right">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <%-- <asp:BoundField DataField="Tax" HeaderText="Tax" ItemStyle-HorizontalAlign="Right">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>--%>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <br />
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                    </td>
                </tr>
                <tr style="height: 1px;">
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <div id="div15" style="overflow: auto; height: 100%; width: 90%; margin-left: 10px"
                            runat="server">
                            <asp:ValidationSummary ID="vs_TabRepayment" runat="server" CssClass="styleMandatoryLabel"
                                ValidationGroup="TabRepayment1" HeaderText="Correct the following validation(s):  " />
                            <asp:CustomValidator ID="cv_TabRepayment" runat="server" CssClass="styleMandatoryLabel"
                                Display="None" ValidationGroup="TabRepayment1"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="4">
                        <asp:ValidationSummary ID="vsEMICalculator" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnCalculate" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnEmail" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnPrint" />
                        <asp:CustomValidator ID="cvEMICalculator" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                        <%--    <asp:RequiredFieldValidator ID="rfvProspectName" runat="server" Display="None" ControlToValidate="txtProspectName"
                            ValidationGroup="btnEmail" ErrorMessage="Enter the Prospect Name" CssClass="styleMandatoryLabel"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvSaveProspectName" runat="server" Display="None"
                            ControlToValidate="txtProspectName" ValidationGroup="btnPrint" ErrorMessage="Enter the Prospect Name"
                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                        <asp:RequiredFieldValidator ID="rfvSaveEmail" runat="server" Display="None" ControlToValidate="txtEmail"
                            ValidationGroup="btnEmail" ErrorMessage="Enter the Email" CssClass="styleMandatoryLabel"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <%--  <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtEmail"
                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"
                            ValidationGroup="btnEmail" ErrorMessage="Enter the valid E-Mail"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="revPrintEmailId" runat="server" ControlToValidate="txtEmail"
                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"
                            ValidationGroup="btnPrint" ErrorMessage="Enter the valid E-Mail"></asp:RegularExpressionValidator>--%>
                        <asp:RequiredFieldValidator ID="rfvActivity" runat="server" Display="None" ControlToValidate="ddlLOB"
                            ValidationGroup="btnCalculate" InitialValue="0" ErrorMessage="Select the Activity"
                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="None" ControlToValidate="txtDate"
                            ValidationGroup="btnCalculate" ErrorMessage="Select the Date" CssClass="styleMandatoryLabel"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" Display="None" ControlToValidate="txtAmount"
                            ValidationGroup="btnCalculate" ErrorMessage="Enter the Finance Amount" CssClass="styleMandatoryLabel"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvAssetReg" runat="server" Display="None" Enabled="false"
                            ControlToValidate="ddlAssetRegister" ValidationGroup="btnCalculate" InitialValue="0"
                            ErrorMessage="Select the Asset Register" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvReturnPattern" runat="server" Display="None" ControlToValidate="ddlReturnPattern"
                            ValidationGroup="btnCalculate" ErrorMessage="Select the Return Pattern" InitialValue="0"
                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvTenureType" runat="server" Display="None" ControlToValidate="ddlTenureType"
                            ValidationGroup="btnCalculate" ErrorMessage="Select the Tenure Type" InitialValue="0"
                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvTenure" runat="server" Display="None" ControlToValidate="txtTenure"
                            ValidationGroup="btnCalculate" ErrorMessage="Enter the Tenure" CssClass="styleMandatoryLabel"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvRepaymentMode" runat="server" Display="None" ControlToValidate="ddlRepaymentMode"
                            ValidationGroup="btnCalculate" ErrorMessage="Select the Repayment Mode" InitialValue="0"
                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvFlatRate" runat="server" Display="None" ControlToValidate="txtFlatRate"
                            ValidationGroup="btnCalculate" ErrorMessage="Enter the Rate" CssClass="styleMandatoryLabel"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvBusinessIRR" runat="server" Display="None" ControlToValidate="txtBusinessIRR"
                            Enabled="false" ValidationGroup="btnCalculate" ErrorMessage="Enter the Business IRR"
                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <%-- <asp:RequiredFieldValidator ID="rfvCorporateTax" runat="server" Display="None" ControlToValidate="txtCorporateTax"
                             Enabled = "false" ValidationGroup="btnCalculate" ErrorMessage="Enter the Corporate Tax"
                             CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
