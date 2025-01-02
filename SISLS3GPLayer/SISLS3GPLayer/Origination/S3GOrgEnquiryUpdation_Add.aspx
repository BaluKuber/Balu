<%@ Page Language="C#" AutoEventWireup="true" Title="Add Enquiry Updation" ValidateRequest="false"
    CodeFile="S3GOrgEnquiryUpdation_Add.aspx.cs" Inherits="Origination_S3GOrgEnquiryUpdation_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/S3GAutoSuggest.ascx" TagName="SUG" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endRequest);
        var postbackElement;

        function initializeRequest(sender, args) {
            document.body.style.cursor = "wait";
            if (prm.get_isInAsyncPostBack()) {
                //debugger
                args.set_cancel(true);
            }
        }
        function endRequest(sender, args) {
            document.body.style.cursor = "default";
        }


        ////        function EnableDisableResidual(Obj1, Obj2) {
        ////            //----Modified By : Thangam M----
        ////            var iLevel=document.getElementById('<%=ddlInterestType.ClientID%>').options.value;
        ////            if (iLevel == "2") 
        ////            {
        ////                if ((Obj1.value == "__.__") || (Obj1.value == "__.____") || (Obj1.value == "")) {
        ////                    Obj2.disabled = false;
        ////                    // Obj2.value = "";
        ////                }
        ////                else if ((Obj1.value != "__.__") && (Obj1.value != "__.____") && (Obj1.value != "")) {
        ////                    Obj2.disabled = true;
        ////                    //Obj2.value = "";
        ////                }
        ////            }
        ////            else
        ////            {
        ////                if ((Obj1.value == "__.__") || (Obj1.value == "__.____") || (Obj1.value == "")) {
        ////                    Obj2.disabled = false;
        ////                    // Obj2.value = "";
        ////                }
        ////                else if ((Obj1.value != "__.__") && (Obj1.value != "__.____") && (Obj1.value != "")) {
        ////                    Obj2.disabled = true;
        ////                    //Obj2.value = "";
        ////                }
        ////            }
        ////       }

        function fnDisableRemarksMandatory() {
            var own = document.getElementById('<%=ddlAssetDetails.ClientID%>').options.value;
            if (own == "-1") {
                document.getElementById('<%=rfvRemarks.ClientID%>').enabled = true;
            }
            else {
                document.getElementById('<%=rfvRemarks.ClientID%>').enabled = false;
            }

        }

        function fnDisableAssetmandatory() {
            var own = document.getElementById('<%=txtRemarks.ClientID%>').value;
            if (own == "") {
                document.getElementById('<%=rfvAssetDetails.ClientID%>').enabled = true;
            }
            else {
                document.getElementById('<%=rfvAssetDetails.ClientID%>').enabled = false;
            }

        }

        //----Modified By : Thangam M----
        //    function fnDisableReportingLevel()
        //    {
        //    var iLevel=document.getElementById('<%=ddlInterestType.ClientID%>').options.value;
        //    if(iLevel=="2")
        //        {

        //            document.getElementById('<%=txtResidualValue.ClientID%>').disabled = true;
        //            document.getElementById('<%=txtResidualValue.ClientID%>').value = "";

        //            document.getElementById('<%=txtMargine.ClientID%>').disabled = false;
        //        }
        //        else
        //        {
        //        
        //        document.getElementById('<%=txtResidualValue.ClientID%>').disabled=false;        
        //        }            
        //    }    
        //   
        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_tcEnquiryUpdation_tbAddress_btnLoadCustomer').click();
        }

    
    </script>

    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UPTABControl" runat="server">
                            <ContentTemplate>
                                <cc1:TabContainer ID="tcEnquiryUpdation" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                    Width="99%" TabStripPlacement="top" AutoPostBack="true" OnActiveTabChanged="tcEnquiryUpdation_ActiveTabChanged">
                                    <cc1:TabPanel runat="server" HeaderText="Enquiry Details" ID="tbAddress" CssClass="tabpan"
                                        BackColor="Red">
                                        <HeaderTemplate>
                                            Enquiry Details</HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlEnquiryInfo" runat="server" GroupingText="Enquiry Information"
                                                        CssClass="stylePanel">
                                                        <table border="0" width="100%">
                                                            <tr>
                                                                <td width="13%" align="left">
                                                                    <asp:Label runat="server" Text="Enquiry Number" ID="lblEnqRefNo" CssClass="styleDisplayLabel"></asp:Label>
                                                                </td>
                                                                <td width="24%" align="left">
                                                                    <asp:TextBox ID="txtEnqRefNo" runat="server" MaxLength="2" Width="160px" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td width="13%" align="left">
                                                                    <asp:Label ID="lblEnqDate" runat="server" CssClass="styleDisplayLabel" Text="Enquiry Date"></asp:Label>
                                                                </td>
                                                                <td width="32%" align="left">
                                                                    <asp:TextBox ID="txtEnqDate" runat="server" MaxLength="2" Width="40%" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="13%" align="left">
                                                                    <asp:Label runat="server" Text="New Customer" ID="lblNewCustomer" CssClass="styleDisplayLabel"></asp:Label><span
                                                                        class="styleMandatory">*</span>
                                                                </td>
                                                                <td width="24%" align="left">
                                                                    <asp:DropDownList ID="ddlNewCustomer" Width="25%" runat="server" OnSelectedIndexChanged="ddlNewCustomer_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                        <asp:ListItem Text="Yes" Value="1" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    <%--   <a id="lnkCreate" runat="server" href="#">
                                                                        <asp:Label CssClass="styleSubmitButton" ID="lblCreate" runat="server" Text="Create"></asp:Label></a>--%>
                                                                </td>
                                                                <%-- <td width="13%" align="left">
                                                                    <asp:Label ID="lblCustomerReference" runat="server" CssClass="styleDisplayLabel"
                                                                        Text="Select Customer"></asp:Label>
                                                                </td>--%>
                                                                <td width="32%" align="left">
                                                                    <%-- <asp:DropDownList ID="ddlCustomer" runat="server" Width="200px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCustomerCode" runat="server" ControlToValidate="ddlCustomer"
                                                                Enabled="false" CssClass="styleMandatoryLabel" Display="None" ErrorMessage=" Select the Customer Number"
                                                                SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="45%" valign="top">
                                                                <%--<asp:Panel ID="pnlCustomerInfo" runat="server" GroupingText="Customer Information"
                                                                    CssClass="stylePanel">
                                                                    <table width="100%">
                                                                        <tr style="display: none">
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblConstitution" runat="server" CssClass="styleDisplayLabel" Text="Constitution"
                                                                                    Width="100px"></asp:Label>
                                                                            </td>
                                                                            <td width="30%" align="left">
                                                                                <asp:TextBox ID="txtConstitution" runat="server" Width="195px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>--%>
                                                                <asp:Panel ID="pnlCustomerHeader" runat="server" CssClass="stylePanel" GroupingText="Customer Information">
                                                                    <table width="100%" cellspacing="0">
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Code" CssClass="styleDisplayLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:TextBox ID="txtCustomerCode" runat="server" Width="50%" Enabled="false"></asp:TextBox>
                                                                                <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                                                                                    OnClick="btnLoadCustomer_OnClick" CausesValidation="false" />
                                                                                <uc2:LOV ID="ucCustomerCodeLov" runat="server" DispalyContent="Code" strLOV_Code="CMDENQU" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:TextBox ID="txtCustomerName" runat="server" Width="80%" MaxLength="60"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvCustomerName"
                                                                                    runat="server" ControlToValidate="txtCustomerName" CssClass="styleMandatoryLabel"
                                                                                    Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblConstitutionID" runat="server" Text="Constitution" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:DropDownList ID="ddlConstitutionId" runat="server" Width="80%">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvConstitutionId"
                                                                                    runat="server" ControlToValidate="ddlConstitutionId" CssClass="styleMandatoryLabel"
                                                                                    Display="None" InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblAddress" runat="server" Text="Address" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:TextBox ID="txtAddress" runat="server" Width="90%" TextMode="MultiLine" Rows="4"
                                                                                    onkeyup="maxlengthfortxt(150)" MaxLength="120"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvAddress" runat="server"
                                                                                    ControlToValidate="txtAddress" CssClass="styleMandatoryLabel" Display="None"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <cc1:FilteredTextBoxExtender ID="ftbeEnquiryDetails" ValidChars="-()\/., " TargetControlID="txtAddress"
                                                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" runat="server"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblCity" runat="server" Text="City" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:TextBox ID="txtCity" runat="server" Width="90%" MaxLength="30"></asp:TextBox>
                                                                                <uc3:SUG ID="ucAutoSuggest" runat="server" ErrorMessage="Select City" ValidationGroup="EnquiryDetails"
                                                                                    ServiceMethod="GetCityList" ItemToValidate="Value" IsMandatory="true" />
                                                                                <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxCity" ValidChars=" " TargetControlID="txtCity"
                                                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters" runat="server" Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>--%>
                                                                                <%--<asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvCity" runat="server"
                                                                                    ControlToValidate="txtCity" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblState" runat="server" Text="State" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:DropDownList ID="ddlState" runat="server" Width="90%">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvState" runat="server"
                                                                                    ControlToValidate="ddlState" CssClass="styleMandatoryLabel" Display="None" InitialValue="-1"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:DropDownList ID="ddlCountry" runat="server" Width="90%">
                                                                                    <asp:ListItem>India</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <%--<asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvCountry" runat="server"
                                                        ControlToValidate="ddlCountry" CssClass="styleMandatoryLabel" Display="None"
                                                        InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblPinCode" runat="server" Text="PinCode" CssClass="styleDisplayLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:TextBox ID="txtPinCode" runat="server" Width="90%" MaxLength="12"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftbePinCode" ValidChars=" " TargetControlID="txtPinCode"
                                                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" runat="server"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <%--<asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvPinCode" runat="server"
                                                        ControlToValidate="txtPinCode" CssClass="styleMandatoryLabel" Display="None"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblMobile" runat="server" Text="Mobile" CssClass="styleReqFieldLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:TextBox ID="txtMobile" runat="server" Width="60%" MaxLength="11"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftbeMobile" ValidChars=" " TargetControlID="txtMobile"
                                                                                    FilterType="Numbers" runat="server" Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvMobile" runat="server"
                                                                                    ControlToValidate="txtMobile" ErrorMessage="Enter Mobile" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldAlign" width="15%" style="padding-bottom: 0px">
                                                                                <asp:Label ID="lblEmail" runat="server" Text="Email" CssClass="styleDisplayLabel" />
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="25%" style="padding-bottom: 0px">
                                                                                <asp:TextBox ID="txtEmail" runat="server" Width="90%" MaxLength="60"></asp:TextBox>
                                                                                <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtEmail"
                                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression='/^(\".*\"|[A-Za-z]\w*)@(\[\d{1,3}(\.\d{1,3}){3}]|[A-Za-z]\w*(\.[A-Za-z]\w*)+)$/'
                                                                                    ValidationGroup="Main" Enabled="false" ErrorMessage="Enter Valid Mail ID"></asp:RegularExpressionValidator>
                                                                                <%--<asp:RequiredFieldValidator ValidationGroup="EnquiryDetails" ID="rfvEmail" runat="server"
                                                        ControlToValidate="txtEmail" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                            <td width="55%">
                                                                <asp:Panel ID="pnlFinanceInfo" runat="server" GroupingText="Financial Information"
                                                                    CssClass="stylePanel">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="34%" align="left">
                                                                                <asp:Label ID="lblFacilityType" runat="server" CssClass="styleDisplayLabel" Text="Facility Type"></asp:Label><span
                                                                                    class="styleMandatory">*</span>
                                                                            </td>
                                                                            <td width="34%" align="left" colspan="3">
                                                                                <asp:DropDownList ID="ddlFacilityType" runat="server" Width="70%" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlFacilityType_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvFacilityType" runat="server" ControlToValidate="ddlFacilityType"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage=" Select the Facility Type"
                                                                                    SetFocusOnError="True" Text=" Select the Facility Type" InitialValue="0"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblFacilityAmount" runat="server" CssClass="styleDisplayLabel" Text="Facility Amount"></asp:Label><span
                                                                                    class="styleMandatory">*</span>
                                                                            </td>
                                                                            <td width="30%" align="left" colspan="3">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td width="30%">
                                                                                            <asp:TextBox ID="txtFacilityAmount" runat="server" MaxLength="10" Width="85%" Style="text-align: right"></asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="ftxtAmount" runat="server" FilterType="Numbers"
                                                                                                TargetControlID="txtFacilityAmount">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvFacilityAmount" runat="server" ControlToValidate="txtFacilityAmount"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage=" Enter the Facility Amount"
                                                                                                SetFocusOnError="True" Text=" Enter the Facility Amount"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:RadioButtonList ID="rdblAssetType" AutoPostBack="true" OnSelectedIndexChanged="rdblAssetType_SelectedIndexChanged"
                                                                                                Visible="false" runat="server" RepeatDirection="Horizontal">
                                                                                                <asp:ListItem Value="0" Text="New" Selected="True" />
                                                                                                <asp:ListItem Value="1" Text="Existing" />
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblCurrencyCode" runat="server" CssClass="styleDisplayLabel" Text="Currency Code"></asp:Label><span
                                                                                    class="styleMandatory">*</span>
                                                                            </td>
                                                                            <td width="34%" align="left" colspan="3">
                                                                                <asp:DropDownList ID="ddlCurrencyCode" runat="server" Width="60%">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvddlCurrencyCode" runat="server" ControlToValidate="ddlCurrencyCode"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage=" Select the Currency Code"
                                                                                    SetFocusOnError="True" Text=" Select the Currency Code" InitialValue="0"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblTenureType" runat="server" CssClass="styleDisplayLabel" Text="Tenure Type"></asp:Label><span
                                                                                    class="styleMandatory">*</span>
                                                                            </td>
                                                                            <td width="34%" align="left">
                                                                                <asp:DropDownList ID="ddlTenureType" runat="server" Width="85%">
                                                                                    <%--<asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Months" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Weeks" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="Days" Value="3"></asp:ListItem>--%>
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlTenureType"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage=" Select the Tenure Type"
                                                                                    SetFocusOnError="True" Text=" Select the Tenure Type" InitialValue="0"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblTenure" runat="server" CssClass="styleDisplayLabel" Text="Tenure"></asp:Label><span
                                                                                    class="styleMandatory">*</span>
                                                                            </td>
                                                                            <td width="30%" align="left">
                                                                                <asp:TextBox ID="txtTenure" runat="server" MaxLength="3" Style="width: 40%; text-align: right"
                                                                                    onkeyup="fnNotAllowPasteSpecialChar(this,'special')"></asp:TextBox><asp:RequiredFieldValidator
                                                                                        ID="rfvTenure" runat="server" ControlToValidate="txtTenure" CssClass="styleMandatoryLabel"
                                                                                        Display="None" ErrorMessage=" Enter the Tenure" SetFocusOnError="True" Text=" Enter the Tenure"></asp:RequiredFieldValidator>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                                                    TargetControlID="txtTenure">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblInterestType" runat="server" CssClass="styleDisplayLabel" Text="Interest Type"></asp:Label><span
                                                                                    class="styleMandatory">*</span>
                                                                            </td>
                                                                            <td width="34%" align="left">
                                                                                <asp:DropDownList ID="ddlInterestType" runat="server" Width="85%">
                                                                                    <%--onchange="fnDisableReportingLevel();">--%>
                                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="IRR" Value="2"></asp:ListItem>
                                                                                    <asp:ListItem Text="Rate" Value="1"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvLOBName9" runat="server" ControlToValidate="ddlInterestType"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage=" Select the Interest Type"
                                                                                    SetFocusOnError="True" Text=" Select the Interest Type" InitialValue="0"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblInterestPercentage" runat="server" CssClass="styleDisplayLabel"
                                                                                    Text="Interest %"></asp:Label><span class="styleMandatory">*</span>
                                                                            </td>
                                                                            <td width="30%" align="left">
                                                                                <asp:TextBox ID="txtInterestPercentage" runat="server" MaxLength="7" Style="width: 95%;
                                                                                    text-align: right"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator10"
                                                                                        runat="server" ControlToValidate="txtInterestPercentage" CssClass="styleMandatoryLabel"
                                                                                        Display="None" ErrorMessage=" Enter the Interest %" SetFocusOnError="True" Text=" Enter the Interest %"></asp:RequiredFieldValidator>
                                                                                <cc1:FilteredTextBoxExtender ID="FTEInterestpercentage" runat="server" FilterType="Numbers,Custom"
                                                                                    ValidChars="." TargetControlID="txtInterestPercentage">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <%-- <cc1:MaskedEditExtender
                                                                    ID="MEEPercentageStake" runat="server" TargetControlID="txtInterestPercentage"
                                                                    Mask="99.9999" MaskType="Number" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" InputDirection="RightToLeft">
                                                                </cc1:MaskedEditExtender>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblMargineAmount" runat="server" CssClass="styleDisplayLabel" Text="Margin Amount"></asp:Label>
                                                                            </td>
                                                                            <td width="28%" align="left">
                                                                                <asp:TextBox ID="txtMargine" MaxLength="7" runat="server" Style="width: 85%; text-align: right"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                                                                    TargetControlID="txtMargine">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                            <td width="30%" align="left">
                                                                                <asp:Label ID="lblResidualValue" runat="server" CssClass="styleDisplayLabel" Text="Residual Value"></asp:Label>
                                                                            </td>
                                                                            <td width="35%" align="left">
                                                                                <asp:TextBox ID="txtResidualValue" runat="server" Style="width: 95%; text-align: right"
                                                                                    onkeyup="fnNotAllowPasteSpecialChar(this,'special')" MaxLength="7"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                                                                    TargetControlID="txtResidualValue">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="18%" align="left">
                                                                                <asp:Label ID="lblAssetDetails" runat="server" Text="Asset Details"></asp:Label>
                                                                            </td>
                                                                            <td width="34%" align="left" colspan="3">
                                                                                <asp:DropDownList ID="ddlAssetDetails" runat="server" Width="100%" OnSelectedIndexChanged="ddlAssetDetails_SelectedIndexChanged"
                                                                                    AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator Width="195px" ID="rfvAssetDetails" runat="server" ControlToValidate="ddlAssetDetails"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage=" Select the Asset Details"
                                                                                    Enabled="false" SetFocusOnError="True" Text=" Select the Asset Code" InitialValue="0"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="18%" align="left" valign="top">
                                                                                <asp:Label ID="lblRemarks" runat="server" CssClass="styleDisplayLabel" Text="Remarks"></asp:Label>
                                                                            </td>
                                                                            <td width="34%" align="left" colspan="3">
                                                                                <asp:TextBox ID="txtRemarks" runat="server" Columns="40" TextMode="MultiLine" onkeyup="maxlengthfortxt(60);"></asp:TextBox>
                                                                                <%-- onkeypress="wraptext(this,'20')" Rows="3"--%>
                                                                                <asp:RequiredFieldValidator ID="rfvRemarks" runat="server" ControlToValidate="txtRemarks"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the asset details/Enter the remarks"
                                                                                    Enabled="false" SetFocusOnError="True" Text=" Enter the Remarks"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel runat="server" HeaderText="Enquiry Details" ID="tbRepayment" CssClass="tabpan"
                                        BackColor="Red">
                                        <HeaderTemplate>
                                            Repayment Schedule</HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <table width="100%" align="center">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:GridView ID="GridView1" AllowPaging="True" runat="server" Width="35%" AutoGenerateColumns="False"
                                                                    OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="20">
                                                                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NextPreviousFirstLast"
                                                                        NextPageText="Next" PreviousPageText="Prev" PageButtonCount="10" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="SerialNumber" HeaderText="Sl.No.">
                                                                            <HeaderStyle Font-Bold="True" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        </asp:BoundField>
                                                                        <%-- <asp:BoundField DataField="InstallmentDate" HeaderText="Installment Date">
                                                            <HeaderStyle Font-Bold="True" />
                                                            <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                        </asp:BoundField>     --%>
                                                                        <asp:TemplateField HeaderText="Installment Date">
                                                                            <HeaderStyle Font-Bold="True" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="30%" />
                                                                            <ItemTemplate>
                                                                                <%# FormatDate(Eval("InstallmentDate").ToString())%>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="InstallmentAmount" HeaderText="Amount">
                                                                            <HeaderStyle Font-Bold="True" />
                                                                            <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
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
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr align="center">
                    <td align="center">
                        <asp:Button ID="btnSchedule" runat="server" Text="Repayment Schedule" CssClass="styleSubmitLongButton"
                            CausesValidation="true" OnClick="btnSchedule_Click" Style="display: none;" />
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                            CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:ValidationSummary ID="vsLOB" runat="server" CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
