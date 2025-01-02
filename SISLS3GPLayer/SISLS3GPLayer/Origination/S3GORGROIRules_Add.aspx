<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GORGROIRules_Add.aspx.cs"
    Inherits="Origination_S3GORGROIRules" MasterPageFile="~/Common/S3GMasterPageCollapse.master"  EnableViewState="true"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput)
        {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function fnRecoveryPatternYearIsEmpty(RecoveryPatternYear)
        {
            //return false;
        }        
        function fnGetRecoveryValue(strRecoveryPatternYear)
        {
            var exp = /_/gi;
            return parseFloat(document.getElementById(strRecoveryPatternYear).value.replace(exp, '0'));
            //return parseFloat(document.getElementById(strRecoveryPatternYear).value);
        }
        function fnEnableRecoveryValue(strRecoveryPatternYear, blnStatus, dcmlRecoveryValue)
        {
            
                document.getElementById(strRecoveryPatternYear).disabled = blnStatus;
                document.getElementById(strRecoveryPatternYear).value = dcmlRecoveryValue.toFixed(2);
            
        }

        function fnShowErrorMessage(intoption, strRecoveryPatternYear)
        {
            if( intoption == 1)
            {
                if(document.getElementById('<%=ddlRepaymentMode.ClientID%>').value == 3)
                {
                    if(fnAllowNumbersOnly(true,false,strRecoveryPatternYear))
                    {
                         alert('Please enter a recovery structure for the rule.');
                    }
                    document.getElementById(strRecoveryPatternYear).focus();
                    return;
                }
            }
            /*else if(intoption == 2)
            {
                alert('Recovery structure % should not be greater than 100%');
                document.getElementById(strRecoveryPatternYear).focus();
                return;
            }
            else if(intoption == 3)
            {
                alert('Total recovery structure % should not be greater than 100%');  
                //document.getElementById(strRecoveryPatternYear).focus();
                return;
            }*/
            
        }
        
        function fnCalculateRecoveryPattern(strControlID, strRecoveryPatternYear1, strRecoveryPatternYear2, strRecoveryPatternYear3, strRecoveryPatternYearRest)
        {     
                   
            var RecoveryPatternYear1;            
            var RecoveryPatternYear2;
            var RecoveryPatternYear3;
            var RecoveryPatternYearRest;         
            var Total = 0;

            if( parseInt(strControlID) == 3 )
            {
                Total = 0;
                
//                fnEnableRecoveryValue(strRecoveryPatternYearRest, true, Total);
                
                RecoveryPatternYear1 = fnGetRecoveryValue(strRecoveryPatternYear1);
                RecoveryPatternYear2 = fnGetRecoveryValue(strRecoveryPatternYear2);
                RecoveryPatternYear3 = fnGetRecoveryValue(strRecoveryPatternYear3);
                RecoveryPatternYearRest = fnGetRecoveryValue(strRecoveryPatternYearRest);
                Total = RecoveryPatternYear1 + RecoveryPatternYear2 + RecoveryPatternYear3 + RecoveryPatternYearRest;
                
                if(RecoveryPatternYear3 == 0) 
                {
                  
//                    fnShowErrorMessage(1, strRecoveryPatternYear3);
//                    return;
                }                               
                if(RecoveryPatternYear3 > 100) 
                {
                    fnShowErrorMessage(2, strRecoveryPatternYear3);
                    return;
                }                
                if( Total > 100 )
                {
                    fnShowErrorMessage(3, strRecoveryPatternYear3);
//                    document.getElementById(strRecoveryPatternYear3).focus();
                    return;
                }                
                else if ( Total < 100 )
                {
                    Total = 100.00 - Total;
                    Total=Total+RecoveryPatternYearRest;
                    fnEnableRecoveryValue(strRecoveryPatternYearRest, true, Total);                    
                }
                
            }
            else if( parseInt(strControlID) == 2 )
            {            
                Total = 0;

//                fnEnableRecoveryValue(strRecoveryPatternYear3, true, Total);
//                fnEnableRecoveryValue(strRecoveryPatternYearRest, true, Total);               
                        
                RecoveryPatternYear1 = fnGetRecoveryValue(strRecoveryPatternYear1);            
                RecoveryPatternYear2 = fnGetRecoveryValue(strRecoveryPatternYear2);
                RecoveryPatternYear3 = fnGetRecoveryValue(strRecoveryPatternYear3);
                RecoveryPatternYearRest = fnGetRecoveryValue(strRecoveryPatternYearRest);
                Total = RecoveryPatternYear1 + RecoveryPatternYear2 + RecoveryPatternYear3 + RecoveryPatternYearRest;
                
                if(RecoveryPatternYear2 == 0) 
                {
//                    fnShowErrorMessage(1, strRecoveryPatternYear2);
//                    return;
                }                                
                if(RecoveryPatternYear2 > 100) 
                {
                    fnShowErrorMessage(2, strRecoveryPatternYear2);
                    return;
                }                                
                if( Total > 100 )
                {
                    fnShowErrorMessage(3, strRecoveryPatternYear2);
//                     document.getElementById(strRecoveryPatternYear2).focus();
                    return;
                }                
                else if ( Total < 100 )
                {
                    Total = 100.00 - Total;
                    Total=Total+RecoveryPatternYear3;
                    //fnEnableRecoveryValue(strRecoveryPatternYear3, false, Total);
                    
                    fnEnableRecoveryValue(strRecoveryPatternYear3, false, Total);
                    document.getElementById(strRecoveryPatternYear3).focus();
                }
               
            }
            
            else if( parseInt(strControlID) == 1 )
            {
                Total = 0;

               // fnEnableRecoveryValue(strRecoveryPatternYear2, true, Total);
               // fnEnableRecoveryValue(strRecoveryPatternYear3, true, Total);
               // fnEnableRecoveryValue(strRecoveryPatternYearRest, true, Total);

                RecoveryPatternYear1 = fnGetRecoveryValue(strRecoveryPatternYear1);
                RecoveryPatternYear2 = fnGetRecoveryValue(strRecoveryPatternYear2);
                RecoveryPatternYear3 = fnGetRecoveryValue(strRecoveryPatternYear3);
                RecoveryPatternYearRest = fnGetRecoveryValue(strRecoveryPatternYearRest);
                Total = RecoveryPatternYear1 + RecoveryPatternYear2 + RecoveryPatternYear3 + RecoveryPatternYearRest;
               
                if(RecoveryPatternYear1 == 0) 
                {
//                    fnShowErrorMessage(1, strRecoveryPatternYear1);
//                    return;
                }
                if(RecoveryPatternYear1 > 100) 
                {                
                    fnShowErrorMessage(2, strRecoveryPatternYear1);
                    return ;
                }
                if( Total > 100)
                {
                    fnShowErrorMessage(3, strRecoveryPatternYear1);
//                    document.getElementById(strRecoveryPatternYear1).focus();
                    return;
                }
                else if( Total < 100)
                {
                     if(document.getElementById('<%=ddlMargin.ClientID%>').value==1)
                     {
                         if(document.getElementById('<%=ddlMargin.ClientID%>').value!='' && RecoveryPatternYear1==0)
                         {

                        }
                        else
                        { 
                            Total = 100.00 - Total;
                            Total=Total+RecoveryPatternYear2;
                            fnEnableRecoveryValue(strRecoveryPatternYear2, false, Total);
                            //document.getElementById(strRecoveryPatternYear2).focus();
                            
                        }
                    }
                    else
                    {
                            Total = 100.00 - Total;
                            Total=Total+RecoveryPatternYear2;
                            fnEnableRecoveryValue(strRecoveryPatternYear2, false, Total);
                            document.getElementById(strRecoveryPatternYear2).focus();
                    }
                }
                
            }
            
        }
 
 
        
        function fnCalculateMarginPercentage(strMarginPercentage,intprefix,intsuffix)
        {      
            if(isNaN(parseFloat(strMarginPercentage.value)))
                {
                    if(strMarginPercentage.value=='')
                        alert('Margin Percentage cannot be empty');
                     else
                        alert('Enter a valid decimal');
                    strMarginPercentage.value='';
                    //strMarginPercentage.focus();
                    return false;
                }
                else
                {
                   var str = strMarginPercentage.value.split('.');
                   if(str !=null)
                   {
                    if(str[0].length > intprefix)
                    {
                       alert('Margin% precision should not exceed '+ intprefix +' digits');
                       //strMarginPercentage.focus();
                       return false;
                    }
                    }
              }
            if(strMarginPercentage.value == 0) 
            {
                alert('Margin% should be greater than 0.');
                strMarginPercentage.value='0';
                //strMarginPercentage.focus();
                return;
            }                               
            else if(strMarginPercentage.value > 100) 
            {
                alert('Margin% should not be greater than 100%');
                //strMarginPercentage.focus();
                return;
            }                
            else if (strMarginPercentage.value <= 100)
            {
                strMarginPercentage.value = parseFloat(strMarginPercentage.value).toFixed(intsuffix);
            }
            
            
        }
        
        //function to check percentage should not exceed 100
        
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
        
        
        function lob_Change(values)
        {
        <%--
        
            document.getElementById('<%=rfvRateType.ClientID%>').enabled = false;
            document.getElementById('<%=lblRateType.ClientID%>').className = "";
             
            var OptionText = values.options[values.selectedIndex].text.toLowerCase();           
            if(OptionText.indexOf('hire') != -1)
            {
                document.getElementById('<%=rfvRateType.ClientID%>').enabled = true;
                document.getElementById('<%=lblRateType.ClientID%>').className = "styleReqFieldLabel";
            }

            if(OptionText.indexOf('loan') != -1)
            {
                document.getElementById('<%=rfvRateType.ClientID%>').enabled = true;
                document.getElementById('<%=lblRateType.ClientID%>').className = "styleReqFieldLabel";
            }

            if(OptionText.indexOf('financial') != -1)
            {
                document.getElementById('<%=rfvRateType.ClientID%>').enabled = true;
                document.getElementById('<%=lblRateType.ClientID%>').className = "styleReqFieldLabel";
            } 
           
               --%>    
        }
            
      
    </script>

    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" align="left">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td colspan="5">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="None"
                                                                            ControlToValidate="ddlLineofBusiness" ValidationGroup="btnSave" InitialValue="0"
                                                                            ErrorMessage="Select the Line of Business" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvProduct" runat="server" Display="None" ControlToValidate="txtModelDescription"
                                                                            ValidationGroup="btnSave" ErrorMessage="Enter Model Description" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvRateType" runat="server" Display="None" ControlToValidate="ddlRateType"
                                                                            ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select the Rate Type"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvROIRuleNumber" runat="server" Display="None" ControlToValidate="txtROIRuleNumber"
                                                                            ValidationGroup="btnSave" ErrorMessage="Enter ROI Rule Number" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RegularExpressionValidator ID="regROIRuleNumber" runat="server" Display="None"
                                                                            ControlToValidate="txtROIRuleNumber" ValidationGroup="btnSave" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvRatePattern" runat="server" Display="None" ControlToValidate="ddlRatePattern"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the Return Pattern"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvTime" runat="server" Display="None" ControlToValidate="ddlTime"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the Time" InitialValue="0"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" Display="None" ControlToValidate="ddlFrequency"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the Frequency" InitialValue="0"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvRepaymentMode" runat="server" Display="None" ControlToValidate="ddlRepaymentMode"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the Repayment Mode"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvRate" runat="server" Display="None" ControlToValidate="txtRate"
                                                                            ValidationGroup="btnSave" ErrorMessage="Enter Rate" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvIRRRest" runat="server" Display="None" ControlToValidate="ddlIRRRest"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the IRR rest" InitialValue="0"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvIntrestCalculation" runat="server" Display="None"
                                                                            ControlToValidate="ddlIntrestCalculation" ValidationGroup="btnSave" ErrorMessage="Select the Interest Calculation"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvIntrestLevy" runat="server" Display="None" ControlToValidate="ddlIntrestLevy"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the Interest Levy"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <%--<td>
                                                                        <asp:RequiredFieldValidator ID="rfvRecoveryPatternYear1" runat="server" Display="None"
                                                                            InitialValue="0.00" ControlToValidate="txtRecoveryPatternYear1" ValidationGroup="btnSave"
                                                                            ErrorMessage="select a Recorvery Pattern Year 1" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvRecoveryPatternYear2" runat="server" Display="None"
                                                                            ControlToValidate="txtRecoveryPatternYear2" ValidationGroup="btnSave" ErrorMessage="select a Recorvery Pattern Year 2"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvRecoveryPatternYear3" runat="server" Display="None"
                                                                            ControlToValidate="txtRecoveryPatternYear3" ValidationGroup="btnSave" ErrorMessage="select a Recorvery Pattern Year 3"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvRecoveryPatternYearRest" runat="server" Display="None"
                                                                            ControlToValidate="txtRecoveryPatternYearRest" ValidationGroup="btnSave" ErrorMessage="select a Recorvery Pattern Year Rest"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>--%>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvInsurance" runat="server" Display="None" ControlToValidate="ddlInsurance"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the Insurance" InitialValue="0"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvResidualValue" runat="server" Display="None" ControlToValidate="ddlResidualValue"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the ResidualValue"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvMargin" runat="server" Display="None" ControlToValidate="ddlMargin"
                                                                            ValidationGroup="btnSave" ErrorMessage="Select the Margin" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rfvMarginPercentage" runat="server" Display="None"
                                                                            ControlToValidate="txtMarginPercentage" ValidationGroup="btnSave" ErrorMessage="Enter Margin %"
                                                                            CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <%-- <cc1:MaskedEditExtender ID="MEExtRate" runat="server" InputDirection="RightToLeft"
                                                                            ClearMaskOnLostFocus="true" Mask="99999.9999" AutoComplete="true" MaskType="Number"
                                                                            TargetControlID="txtRate">
                                                                        </cc1:MaskedEditExtender>--%>
                                                                    </td>
                                                                    <td>
                                                                        <%--<cc1:MaskedEditExtender ID="MEExtRecoveryPatternYear1" runat="server" InputDirection="RightToLeft"
                                                                            ClearMaskOnLostFocus="true" Mask="999.99" AutoComplete="true" MaskType="Number"
                                                                            TargetControlID="txtRecoveryPatternYear1">
                                                                        </cc1:MaskedEditExtender>--%>
                                                                    </td>
                                                                    <td>
                                                                        <%-- <cc1:MaskedEditExtender ID="MEExtRecoveryPatternYear2" runat="server" InputDirection="RightToLeft"
                                                                            ClearMaskOnLostFocus="true" Mask="999.99" AutoComplete="true" MaskType="Number"
                                                                            TargetControlID="txtRecoveryPatternYear2">
                                                                        </cc1:MaskedEditExtender>--%>
                                                                    </td>
                                                                    <td>
                                                                        <%--<cc1:MaskedEditExtender ID="MEExtRecoveryPatternYear3" runat="server" InputDirection="RightToLeft"
                                                                            ClearMaskOnLostFocus="true" Mask="999.99" AutoComplete="true" MaskType="Number"
                                                                            TargetControlID="txtRecoveryPatternYear3">
                                                                        </cc1:MaskedEditExtender>--%>
                                                                    </td>
                                                                    <td>
                                                                        <%--<cc1:MaskedEditExtender ID="MEExtMarginPercentage" runat="server" InputDirection="RightToLeft"
                                                                            ClearMaskOnLostFocus="true" Mask="999.99" AutoComplete="true" MaskType="Number"
                                                                            TargetControlID="txtMarginPercentage">
                                                                        </cc1:MaskedEditExtender>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSerial" runat="server" visible="false">
                                                        <td style="width: 22%;" class="styleFieldLabel">
                                                            <asp:Label ID="lblSerialNumber" runat="server" CssClass="styleReqFieldLabel" Text="Serial Number"
                                                                ToolTip="Serial Number"></asp:Label>
                                                        </td>
                                                        <td style="width: 25%;">
                                                            <asp:TextBox ID="txtSerialNumber" runat="server" Text="1" ToolTip="Serial Number"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%;">
                                                        </td>
                                                        <td style="width: 22%;" class="styleFieldLabel">
                                                        </td>
                                                        <td style="width: 25%;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" ToolTip="Line of Business"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlLineofBusiness" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLineofBusiness_SelectedIndexChanged"
                                                                onchange="javascript:lob_Change(this);" ToolTip="Line of Business">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblModelDescription" runat="server" Text="Model Description" ToolTip="Model Description"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtModelDescription" runat="server" MaxLength="40" ToolTip="Model Description"></asp:TextBox>
                                                            <%--<cc1:FilteredTextBoxExtender ID="tbExtModelDescription" runat="server" TargetControlID="txtModelDescription"
                                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRateType" runat="server" Text="Rate Type" ToolTip="Rate Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRateType" runat="server" OnSelectedIndexChanged="ddlRateType_SelectedIndexChanged"
                                                                AutoPostBack="true" ToolTip="Rate Type" Width="50%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblROIRuleNumber" runat="server" Text="ROI Rule Number" ToolTip="ROI Rule Number"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtROIRuleNumber" runat="server" MaxLength="6" Width="40%" ToolTip="ROI Rule Number"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRatePattern" runat="server" Text="Return Pattern" ToolTip="Return Pattern"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRatePattern" runat="server" OnSelectedIndexChanged="ddlRatePattern_SelectedIndexChanged"
                                                                AutoPostBack="true" ToolTip="Return Pattern">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblTime" runat="server" Text="Time" ToolTip="Time"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlTime" runat="server" ToolTip="Time" Width="65%">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblFrequency" runat="server" Text="Frequency" ToolTip="Frequency"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFrequency" runat="server" ToolTip="Frequency" Width="50%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRepaymentMode" runat="server" Text="Repayment Mode" ToolTip="Repayment Mode"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRepaymentMode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRepaymentMode_SelectedIndexChanged"
                                                                ToolTip="Repayment Mode">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRate" runat="server" Text="Rate" ToolTip="Rate"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRate" runat="server" Style="text-align: right" MaxLength="5"
                                                                Width="48%" ToolTip="Rate" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblIRRRest" runat="server" Text="IRR Rest" ToolTip="IRR Rest"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlIRRRest" runat="server" ToolTip="IRR Rest" Width="65%">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblIntrestCalculation" runat="server" Text="Interest Calculation" 
                                                                ToolTip="Interest Calculation"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlIntrestCalculation" runat="server" ToolTip="Interest Calculation" Width="50%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblIntrestLevy" runat="server" Text="Interest Levy" ToolTip="Interest Levy"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlIntrestLevy" runat="server" ToolTip="Interest Levy" Width="65%">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblInsurance" runat="server" Text="Insurance" ToolTip="Insurance"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlInsurance" runat="server" ToolTip="Insurance" Width="50%">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblResidualValue" runat="server" Text="Residual Value" ToolTip="Residual Value"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlResidualValue" runat="server" ToolTip="Residual Value" Width="27%">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblMargin" runat="server" Text="Margin" ToolTip="Margin"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMargin" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMargin_SelectedIndexChanged"
                                                                ToolTip="Margin">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblMarginPercentage" runat="server" Text="Margin %" ToolTip="Margin %"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtMarginPercentage" Style="text-align: right" runat="server" MaxLength="6"
                                                                Width="25%" ToolTip="Margin %" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                               <%-- <cc1:FilteredTextBoxExtender ID="ftxtMarginPercentage" runat="server" TargetControlID="txtMarginPercentage"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars="."></cc1:FilteredTextBoxExtender>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active"
                                                                ToolTip="Active Indicator"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkActive" runat="server" ToolTip="Active Indicator" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="5">
                                                            <br />
                                                            <table width="90%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label1" runat="server" Text="Recovery Pattern" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table class="styleContentTable" width="90%" style="text-align: center">
                                                                <tr class="styleGridHeader">
                                                                    <td width="25%">
                                                                        <asp:Label ID="lblRecoveryPatternYear1" runat="server" Text="Year 1" ToolTip="Year1"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:Label ID="Label3" runat="server" Text="Year 2" ToolTip="Year2" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:Label ID="Label4" runat="server" Text="Year 3" ToolTip="Year3" Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:Label ID="Label5" runat="server" Text="Rest of the Period" ToolTip="Rest Periods"
                                                                            Font-Bold="true"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="25%">
                                                                        <asp:TextBox ID="txtRecoveryPatternYear1" runat="server" Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                            Width="40%" MaxLength="10" ToolTip="Year1"></asp:TextBox>
                                                                           <%-- <cc1:FilteredTextBoxExtender ID="ftxtRecoveryPatternYear1" runat="server" TargetControlID="txtRecoveryPatternYear1"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars="."></cc1:FilteredTextBoxExtender>--%>
                                                                        
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:TextBox ID="txtRecoveryPatternYear2" runat="server" Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                            Width="40%" MaxLength="10" ToolTip="Year2"></asp:TextBox>
                                                                            <%-- <cc1:FilteredTextBoxExtender ID="ftxtRecoveryPatternYear2" runat="server" TargetControlID="txtRecoveryPatternYear2"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars="."></cc1:FilteredTextBoxExtender>--%>
                                                                        
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:TextBox ID="txtRecoveryPatternYear3" runat="server" Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                            Width="40%" MaxLength="10" ToolTip="Year 3" ></asp:TextBox>
                                                                          <%--<cc1:FilteredTextBoxExtender ID="ftxtRecoveryPatternYear3" runat="server" TargetControlID="txtRecoveryPatternYear3"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars="."></cc1:FilteredTextBoxExtender>  --%>
                                                                    </td>
                                                                    <td width="25%">
                                                                        <asp:TextBox ID="txtRecoveryPatternYearRest" runat="server" Style="text-align: right"
                                                                            Width="40%" MaxLength="10" ToolTip="Rest of the Period" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="5">
                                                            <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                                                ValidationGroup="btnSave" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                                                Text="Save" ToolTip="Save" />
                                                            <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                                                CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" ToolTip="Clear" />
                                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                                                                CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                                                        </td>
                                                    </tr>
                                                   
                                                    <tr>
                                                        <td align="center">
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
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
