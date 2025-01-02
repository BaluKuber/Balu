<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GORGPaymentRuleCard_Add.aspx.cs"
    Inherits="Origination_S3GORGPaymentRuleCard" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput)
        {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function fnGetCompensationValue(CompensationValue)
        {
            var exp = /_/gi;
            return parseFloat(CompensationValue.value.replace(exp, '0'));
        }        
        function CompensationPercentage_OnBlur(values)
        {
            var qMode =('<%= Request.QueryString["qsMode"]%>');
            if (qMode.toLowerCase() != 'q')
            {
                document.getElementById('<%=rfvCompensationLevyPattern.ClientID%>').enabled = false;
                document.getElementById('<%=rfvFrequency.ClientID%>').enabled = false;
                
                document.getElementById('<%=lblCompensationLevyPattern.ClientID%>').className = "";
                document.getElementById('<%=lblFrequency.ClientID%>').className = "";
                var varCompensationValue = fnGetCompensationValue(values);
                if(varCompensationValue> 0)
                {
                    document.getElementById('<%=rfvCompensationLevyPattern.ClientID%>').enabled = true;
                    document.getElementById('<%=rfvFrequency.ClientID%>').enabled = true;
            
                    document.getElementById('<%=lblCompensationLevyPattern.ClientID%>').className = "styleReqFieldLabel";
                    document.getElementById('<%=lblFrequency.ClientID%>').className = "styleReqFieldLabel";       
                }
            }
//            alert(values.value);
            var exp = /_/gi;
            values.value = parseFloat(values.value.replace(exp, '0'));
             
            if(values.value=='0')
            {
              alert('Compensation % should not be Zero'); 
              values.value="";
              values.focus();
              return false;
            }
        }
        
         function fncheckvalidpercentage(ObjCtrl)
        {
            if(ObjCtrl.value != '')
            {
                if(parseFloat(ObjCtrl.value)>100)
                {
                alert('Value should not be greater than 100%');
                ObjCtrl.value='';
                ObjCtrl.focus();
                }
            }
            
        }
        
        function chkemptycompensation(input)
        {
                 if(input.value=='')
                {
                  alert('Compensation% cannot be zero or empty');
                  //input.focus();
                  return false;
                }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" align="left">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" colspan="2">
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td colspan="5">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="None"
                                                                            ControlToValidate="ddlLOB1" ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Line of Business"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvAccountType" runat="server" Display="None" ControlToValidate="ddlAccountType"
                                                                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Account type"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvEntityType" runat="server" Display="None" ControlToValidate="ddlEntityType"
                                                                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Entity type"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvCompensationLevyPattern" runat="server" Display="None"
                                                                            Enabled="false" ControlToValidate="ddlCompensationLevyPattern" ValidationGroup="btnSave"
                                                                            InitialValue="0" ErrorMessage="Select Compensation levy pattern" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" Display="None" Enabled="false"
                                                                            ControlToValidate="ddlFrequency" ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select levy frequency"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvOnTapRefinance" runat="server" Display="None"
                                                                            ControlToValidate="ddlOnTapRefinance" ValidationGroup="btnSave" InitialValue="-1"
                                                                            ErrorMessage="Select On tap refinance" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <%--<cc1:MaskedEditExtender ID="MEExtCompensationPercentage" runat="server" InputDirection="RightToLeft"
                                                                            ClearMaskOnLostFocus="true" Mask="99.9999" AutoComplete="true" MaskType="Number"
                                                                            TargetControlID="txtCompensationPercentage">
                                                                        </cc1:MaskedEditExtender>--%>
                                                                         <asp:RequiredFieldValidator ID="rfvCompensationPercentage" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="txtCompensationPercentage" ErrorMessage="Enter Compensation %" Enabled="false" ValidationGroup="btnSave" Display="None"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 17%;" class="styleFieldLabel">
                                                            <asp:Label ID="lblPaymentRuleNumber" runat="server" Text="Payment Rule Number" ToolTip="Payment Rule Number"></asp:Label>
                                                        </td>
                                                        <td style="width: 25%;">
                                                            <asp:TextBox ID="txtPaymentRuleNumber" runat="server" Style="margin-left: 0px" ToolTip="Payment Rule Number"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 20%;" class="styleFieldLabel">
                                                        </td>
                                                        <td style="width: 25%;">
                                                        </td>
                                                    </tr>
                                                    <%--<tr><td style="width: 17%">&nbsp;</td></tr>--%>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 17%">
                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line Of Business" ToolTip="Line Of Business"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlLOB1" runat="server" OnSelectedIndexChanged="ddlLOB1_SelectedIndexChanged"
                                                                AutoPostBack="true" ToolTip="Line Of Business">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="styleFieldLabel" style="width: 17%">
                                                            <asp:Label ID="lblAccountType" runat="server" Text="Account Type" ToolTip="Account Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAccountType" runat="server" OnSelectedIndexChanged="AccountType_SelectedIndexChanged"
                                                                AutoPostBack="true" ToolTip="Account Type">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <%-- <tr><td style="width: 17%">&nbsp;</td></tr>--%>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 20%">
                                                            <asp:Label ID="lblProductName" runat="server" Text="Product Name" ToolTip="Product Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlProductName" runat="server" ToolTip="Product Name">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="styleFieldLabel" style="width: 20%">
                                                            <asp:Label ID="lblEntityType" runat="server" Text="Entity Type" ToolTip="Entity Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlEntityType" runat="server" ToolTip="Entity Type">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <%--<tr><td style="width: 17%">&nbsp;</td></tr>--%>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 17%">
                                                            <asp:Label ID="lblCompensationPercentage" runat="server" Text="Compensation %" ToolTip="Compensation %"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtCompensationPercentage" Style="text-align: right" MaxLength="7"
                                                                runat="server" ToolTip="Compensation %" Width="35%" 
                                                                  ></asp:TextBox>
                                                                 <cc1:FilteredTextBoxExtender ID="ftxtCompensationPercentage" runat="server" TargetControlID="txtCompensationPercentage"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars="."></cc1:FilteredTextBoxExtender>                                                           
                                                        </td>
                                                        <td class="styleFieldLabel" style="width: 25%">
                                                            <asp:Label ID="lblCompensationLevyPattern" runat="server" Text="Compensation Levy Pattern"
                                                                ToolTip="Compensation Levy Pattern"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCompensationLevyPattern" runat="server" ToolTip="Compensation Levy Pattern">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <%--<tr><td style="width: 17%">&nbsp;</td></tr>--%>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 17%">
                                                            <asp:Label ID="lblFrequency" runat="server" Text="Levy Frequency" ToolTip="Levy Frequency"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFrequency" runat="server" ToolTip="Levy Frequency">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="styleFieldLabel" style="width: 20%">
                                                            <asp:Label ID="lblOnTapRefinance" runat="server" Text="On Tap Refinance" ToolTip="On Tap Refinance"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlOnTapRefinance" runat="server" ToolTip="On Tap Refinance">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <%--<tr><td style="width: 17%">&nbsp;</td></tr>--%>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 17%">
                                                            <asp:Label ID="lblRate" runat="server" Text="Active" ToolTip="Active Indicator"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActive" runat="server" ToolTip="Active Indicator" />
                                                        </td>
                                                        <td class="styleFieldLabel" style="width: 20%">
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <%--<tr><td style="width: 17%">&nbsp;</td></tr>--%>
                                                    <tr>
                                                        <td colspan="5" align="center">
                                                            <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                                                ValidationGroup="btnSave" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                                                Text="Save" ToolTip="Save" />
                                                            <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                                                CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" ToolTip="Clear" />
                                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                                                                CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                                                        </td>
                                                    </tr>
                                                    <%--<tr><td style="width: 17%">&nbsp;</td></tr>--%>
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
                                <td align="center">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
