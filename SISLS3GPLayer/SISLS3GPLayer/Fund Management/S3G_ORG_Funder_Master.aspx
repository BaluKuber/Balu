<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_ORG_Funder_Master.aspx.cs"
    Inherits="Origination_S3G_ORG_Funder_Master" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagPrefix="uc1" TagName="PageNavigator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript">

        function fnClearBankDtls() {
            document.getElementById('<%=txtBankName.ClientID%>').value =
            document.getElementById('<%=txtBenificiaryName.ClientID%>').value =
            document.getElementById('<%=txtAccountNo.ClientID%>').value =
            document.getElementById('<%=txtBankCity.ClientID%>').value =
            document.getElementById('<%=txtBankBranch.ClientID%>').value =
            document.getElementById('<%=txtMICRCode.ClientID%>').value =
            document.getElementById('<%=txtIFSCCode.ClientID%>').value = "";
            document.getElementById('<%=ddlAccountType.ClientID%>').selectedIndex = 0;
            document.getElementById('<%=chkDefaultAccount.ClientID%>').checked = false;
            document.getElementById('<%=btnBankAdd.ClientID%>').disabled = false;
            document.getElementById('<%=btnBankModify.ClientID%>').disabled = true;
        }

        function jsMICRvaildate(txtMICRCode) {
            if (txtMICRCode.value.length > 0) {
                if (txtMICRCode.value.length < 9) {
                    alert('MICR Code should be 9 digits');
                    document.getElementById(txtMICRCode.id).focus();
                }
            }
        }

        function Customer_ItemSelected(sender, e) {
            var hdnCustomerID = $get('<%= hdnCustomerID.ClientID %>');
            hdnCustomerID.value = e.get_value();
        }
        function Customer_ItemPopulated(sender, e) {
            var hdnCustomerID = $get('<%= hdnCustomerID.ClientID %>');
            hdnCustomerID.value = '';
        }

        function Asset_ItemSelected(sender, e) {
            var hdnAssetID = $get('<%= hdnAssetID.ClientID %>');
            hdnAssetID.value = e.get_value();
        }
        function Asset_ItemPopulated(sender, e) {
            var hdnAssetID = $get('<%= hdnAssetID.ClientID %>');
            hdnAssetID.value = '';
        }

        function ChkIsZero(sender) {
            if (sender.value != "") {
                if (sender.value == 0) {
                    alert('Sanction Limit should be greater than 0');
                    sender.value = "";
                }
            }
        }

        function onFunderChange() {
            document.getElementById('<%=lblLimitFunderName.ClientID%>').innerHTML = document.getElementById('<%=txtFunderName.ClientID%>').value;
        }

        function fnValidateIFSCCode(ISFC_Code) {
            var re = /[A-Z|a-z]{4}[0][A-Z|a-z|0-9]{6}$/;
            if (ISFC_Code.value != "") {
                if (!re.test(ISFC_Code.value)) {
                    alert('Enter Valid IFSC Code.Eg[AAAA0AAAAAA]');
                    ISFC_Code.value = "";
                }
            }
        }

        function fnValidatePAN(PAN) {
            var re = /^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$/;
            if (PAN.value != "") {
                if (!re.test(PAN.value)) {
                    alert('Enter Valid PAN.Eg[AAAAA1234A]');
                    PAN.value = "";
                }
            }
        }

        function fnLoadPath(btnBrowse) {
            if (btnBrowse != null)
                document.getElementById(btnBrowse).click();
        }

        function fnAssignPath(btnBrowse, hdnPath) {
            if (btnBrowse != null)
                document.getElementById(hdnPath).value = document.getElementById(btnBrowse).value;
        }

        function fnLoadPath1(btnBrowse1) {
            if (btnBrowse1 != null)
                //debugger;
                document.getElementById(btnBrowse1).click();
        }

        function fnAssignPath1(flUpload1, hdnPath1, btnBrowse1) {
            if (flUpload1 != null)
                document.getElementById(hdnPath1).value = document.getElementById(flUpload1).value;
            //debugger;
            document.getElementById(btnBrowse1).click();
        }

        var querymode;
        querymode = location.search.split("qsMode=");
        querymode = querymode[1];

        var tab;
        function pageLoad() {
            tab = $find('ctl00_ContentPlaceHolder1_tcFunder');
            querymode = location.search.split("qsMode=");
            querymode = querymode[1];
            if (querymode != 'Q' && tab != null) {
                tab.add_activeTabChanged(on_Change);
                var newindex = tab.get_activeTabIndex(index);
            }
        }
        var index = 0;
        function on_Change(sender, e) {
            //var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var strValgrp = "vsSave";
            var Valgrp = document.getElementById('<%=vsSave.ClientID %>')
            Valgrp.validationGroup = strValgrp;
            var newindex = tab.get_activeTabIndex(index);

            if (newindex > index) {
                if (!fnCheckPageValidators(strValgrp, false)) {
                    tab.set_activeTabIndex(index);
                    return;
                }
                else {
                    index = tab.get_activeTabIndex(index);
                    return;
                }
            }
            else {
                tab.set_activeTabIndex(newindex);
                index = tab.get_activeTabIndex(newindex);
            }
        }

        function NetDiscountRate(sender, e) {

            var txtNetDiscountRate = document.getElementById('<%=txtNetDiscountRate.ClientID %>');
            txtNetDiscountRate.value = parseFloat(document.getElementById('<%=txtDiscountRate.ClientID %>').value).toFixed(2);
            var DiscountRate = document.getElementById('<%=txtDiscountRate.ClientID %>').value;
            var ProcessingFeePerc = document.getElementById('<%=txtProcessingFeePerc.ClientID %>').value;
            var UpfrontInt = document.getElementById('<%=txtUpfrontInt.ClientID %>').value;
            var NetDiscountRate = txtNetDiscountRate.value;
            var chkDiscountProcessing = document.getElementById('<%=chkDiscountProcessing.ClientID %>');
            var chkUpfront = document.getElementById('<%=chkUpfront.ClientID %>');

            if (chkDiscountProcessing.checked && ProcessingFeePerc > 0) {
                txtNetDiscountRate.value = (parseFloat(NetDiscountRate) - parseFloat(ProcessingFeePerc)).toFixed(2);
            }

            NetDiscountRate = parseFloat(txtNetDiscountRate.value).toFixed(2);

            if (chkUpfront.checked && UpfrontInt > 0) {
                    txtNetDiscountRate.value = (parseFloat(NetDiscountRate) - parseFloat(UpfrontInt)).toFixed(2);
            }
        }

        function NetDiscountRatePF(sender, e) {

            var DiscountRate = document.getElementById('<%=txtDiscountRate.ClientID %>').value;
            var ProcessingFeePerc = document.getElementById('<%=txtProcessingFeePerc.ClientID %>').value;
            var txtNetDiscountRate = document.getElementById('<%=txtNetDiscountRate.ClientID %>');
            var NetDiscountRate = document.getElementById('<%=txtNetDiscountRate.ClientID %>').value;
            var chkDiscountProcessing = document.getElementById('<%=chkDiscountProcessing.ClientID %>');
            var chkUpfront = document.getElementById('<%=chkUpfront.ClientID %>');

            if (chkDiscountProcessing.checked) {
                if (NetDiscountRate > 0)
                    txtNetDiscountRate.value = (parseFloat(NetDiscountRate) - parseFloat(ProcessingFeePerc)).toFixed(2);
                else
                    txtNetDiscountRate.value = (parseFloat(DiscountRate) - parseFloat(ProcessingFeePerc)).toFixed(2);
            }

            if (!chkDiscountProcessing.checked) {
                if (NetDiscountRate > 0)
                    txtNetDiscountRate.value = (parseFloat(NetDiscountRate) + parseFloat(ProcessingFeePerc)).toFixed(2);
                else
                    txtNetDiscountRate.value = (parseFloat(DiscountRate) + parseFloat(ProcessingFeePerc)).toFixed(2);
            }

            if (!chkDiscountProcessing.checked && !chkUpfront.checked)
                txtNetDiscountRate.value = parseFloat(DiscountRate).toFixed(2);
        }

        function NetDiscountRateUR(sender, e) {
            
            var DiscountRate = document.getElementById('<%=txtDiscountRate.ClientID %>').value;
            var UpfrontInt = document.getElementById('<%=txtUpfrontInt.ClientID %>').value;
            var txtNetDiscountRate = document.getElementById('<%=txtNetDiscountRate.ClientID %>');
            var NetDiscountRate = document.getElementById('<%=txtNetDiscountRate.ClientID %>').value;
            var chkDiscountProcessing = document.getElementById('<%=chkDiscountProcessing.ClientID %>');
            var chkUpfront = document.getElementById('<%=chkUpfront.ClientID %>');

            if (chkUpfront.checked && UpfrontInt > 0) {
                if (NetDiscountRate > 0)
                    txtNetDiscountRate.value = (parseFloat(NetDiscountRate) - parseFloat(UpfrontInt)).toFixed(2);
                else
                    txtNetDiscountRate.value = (parseFloat(DiscountRate) - parseFloat(UpfrontInt)).toFixed(2);
            }
            if (!chkUpfront.checked && UpfrontInt > 0) {
                if (NetDiscountRate > 0)
                    txtNetDiscountRate.value = (parseFloat(NetDiscountRate) + parseFloat(UpfrontInt)).toFixed(2);
                else
                    txtNetDiscountRate.value = (parseFloat(DiscountRate) + parseFloat(UpfrontInt)).toFixed(2);
            }

            if (!chkDiscountProcessing.checked && !chkUpfront.checked)
                txtNetDiscountRate.value = parseFloat(DiscountRate).toFixed(2);
        }

    </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                </asp:Label>
            </td>
        </tr>
    </table>

    <cc1:TabContainer ID="tcFunder" runat="server" CssClass="styleTabPanel" TabStripPlacement="top" ScrollBars="None" ActiveTabIndex="0">
        <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
            BackColor="Red" ToolTip="General" Width="99%">
            <HeaderTemplate>
                General
            </HeaderTemplate>
            <ContentTemplate>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblFunderCode" runat="server" CssClass="styleDisplayLabel" Text="Funder Code"></asp:Label>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:TextBox ID="txtFunderCode" runat="server" MaxLength="50" ToolTip="Funder Code" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="styleFieldLabel" rowspan="3">
                                    <asp:Label ID="lblNote" runat="server" CssClass="styleDisplayLabel" Text="Remarks"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" rowspan="3">
                                    <asp:TextBox ID="txtNote" runat="server" MaxLength="100" ToolTip="Remarks" onkeyup="maxlengthfortxt(500);" TextMode="MultiLine"
                                        Height="60px" Width="300px">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblFunderName" runat="server" CssClass="styleReqFieldLabel" Text="Funder Name"></asp:Label>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:TextBox ID="txtFunderName" runat="server" MaxLength="50" ToolTip="Funder Name" Width="250px" onblur="onFunderChange()"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFunderName" runat="server" ControlToValidate="txtFunderName" SetFocusOnError="true"
                                        ErrorMessage="Enter the Funder Name" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblGLAccountCode" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:DropDownList ID="ddlGLAccount" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvGLAccount" runat="server" ControlToValidate="ddlGLAccount" SetFocusOnError="true" InitialValue="0"
                                        ErrorMessage="Select the GL Account" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblFunderShortName" runat="server" CssClass="styleReqFieldLabel" Text="Short Name"></asp:Label>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:TextBox ID="txtShortName" runat="server" MaxLength="6" ToolTip="Funder Name"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvShortName" runat="server" ControlToValidate="txtShortName" SetFocusOnError="true"
                                        ErrorMessage="Enter the Short Name" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                    </asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="ftbeShortName" runat="server" Enabled="true" TargetControlID="txtShortName"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" width="49%">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr width="100%">
                                            <td>
                                                <asp:Panel ID="pnlCorporateAddress" Width="98%" runat="server" CssClass="stylePanel" GroupingText="Corporate Address">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpAddress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpAddress" runat="server" onkeyup="maxlengthfortxt(100);" ToolTip="Corporate Address"
                                                                    TextMode="MultiLine" Height="60px" Width="300px"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvCorpAddress" runat="server" ControlToValidate="txtCorpAddress" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Corporate Address" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpCity" runat="server" MaxLength="60" ToolTip="Corporate City"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvCorpCity" runat="server" ControlToValidate="txtCorpCity" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Corporate City" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorporateState" runat="server" CssClass="styleReqFieldLabel" Text="State"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <%--<cc1:ComboBox ID="txtCorpState" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend" MaxLength="30"
                                                            Width="80px">
                                                        </cc1:ComboBox>
                                                        <asp:RequiredFieldValidator ID="rfvCorporateState" runat="server" ControlToValidate="txtCorpState" SetFocusOnError="true" InitialValue="0"
                                                            ErrorMessage="Select the Corporate State" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                        </asp:RequiredFieldValidator>--%>
                                                                <uc2:Suggest ID="txtCorpState" runat="server" ServiceMethod="GetStateCmnList" IsMandatory="true" ValidationGroup="vsSave"
                                                                    ErrorMessage="Enter the Corporate State" ToolTip="Corporate State" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorporateCountry" runat="server" CssClass="styleReqFieldLabel" Text="Country"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <cc1:ComboBox ID="txtCorpCountry" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend" MaxLength="30"
                                                                    Width="80px">
                                                                </cc1:ComboBox>
                                                                <asp:RequiredFieldValidator ID="rfvCorporateCountry" runat="server" ControlToValidate="txtCorpCountry" SetFocusOnError="true" InitialValue="0"
                                                                    ErrorMessage="Enter the Corporate Country" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorporatePincode" runat="server" CssClass="styleReqFieldLabel" Text="Pincode"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpPincode" runat="server" MaxLength="10" ToolTip="Corporate Pincode"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvCorpPincode" runat="server" ControlToValidate="txtCorpPincode" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Corporate Pincode" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCorpPincode" runat="server" Enabled="true" TargetControlID="txtCorpPincode"
                                                                    FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpTelephoneNo" runat="server" Text="Telephone No"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpTelephoneNo" runat="server" MaxLength="12" ToolTip="Corporate Telephone No"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCorpTelephoneNo" runat="server" Enabled="true" TargetControlID="txtCorpTelephoneNo"
                                                                    FilterType="Numbers,Custom" ValidChars="+-">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvCorpTelephoneNo" runat="server" ControlToValidate="txtCorpTelephoneNo" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Corporate Telephone" Enabled="false" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpMobileNo" runat="server" CssClass="styleDisplayLabel" Text="Mobile No"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpMobileNo" runat="server" MaxLength="12" ToolTip="Corporate Mobile No"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCorpMobileNo" runat="server" Enabled="true" TargetControlID="txtCorpMobileNo"
                                                                    FilterType="Numbers,Custom" ValidChars="+">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpEmailID" runat="server" CssClass="styleDisplayLabel" Text="Email ID"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpEmailID" runat="server" MaxLength="180" ToolTip="Corporate Email ID" Width="300px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator
                                                                    ID="revCorpEmailID" runat="server" ControlToValidate="txtCorpEmailID" Display="None" ValidationGroup="vsSave"
                                                                    ErrorMessage="Enter a valid Corporate Email ID" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;|,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpTAN" runat="server" CssClass="styleDisplayLabel" Text="TAN"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpTAN" runat="server" MaxLength="60" ToolTip="Corporate TAN"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpTIN" runat="server" CssClass="styleDisplayLabel" Text="TIN"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpTIN" runat="server" MaxLength="11" ToolTip="Corporate TIN"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCorpTIN" runat="server" Enabled="true" TargetControlID="txtCorpTIN"
                                                                    FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCorpPAN" runat="server" Text="PAN"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCorpPAN" runat="server" MaxLength="20" ToolTip="Corporate PAN" onblur="fnValidatePAN(this)"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCorpPAN" runat="server" Enabled="true" TargetControlID="txtCorpPAN"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvCorpPAN" runat="server" ControlToValidate="txtCorpPAN" SetFocusOnError="true" Enabled="false"
                                                                    ErrorMessage="Enter the Corporate PAN" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblCGSTIN" runat="server" Text="GSTIN"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtCGSTin" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                        TargetControlID="txtCGSTin" ValidChars=". " runat="server" Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <%--   <asp:RequiredFieldValidator ID="rfvcgstin" runat="server" Display="None" ErrorMessage="Enter the CGSTIN"
                                                    ControlToValidate="txtCGSTin" ValidationGroup="Main" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblCGSTREGDate" runat="server" Text="GST Date"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="TxtCGSTRegDate" runat="server" Width="100px"></asp:TextBox>
                                                                    <asp:Image ID="imgCGSTRegDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                    <%-- <asp:RequiredFieldValidator ID="rfvcgstregdate" runat="server" Display="None"
                                                    ErrorMessage="Enter CGST Registration Date" ControlToValidate="TxtCGSTRegDate"
                                                    SetFocusOnError="True" ValidationGroup="Main" Enabled="false"></asp:RequiredFieldValidator>--%>
                                                                    <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="TxtCGSTRegDate"
                                                                        PopupButtonID="imgCGSTRegDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                        ID="CalendarExtender2" Enabled="True">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="1%" valign="middle">
                                    <asp:Button ID="btnCopyAddress" runat="server" Text=">>" ToolTip="Copy Address" OnClick="btnCopyAddress_Click" />
                                </td>
                                <td valign="top" width="49%">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr width="100%">
                                            <td>
                                                <asp:Panel ID="pnlCommunicationAddress" Width="98%" runat="server" CssClass="stylePanel" GroupingText="Communication Address">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommAddress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommAdress" runat="server" onkeyup="maxlengthfortxt(100);" ToolTip="Communication Address"
                                                                    TextMode="MultiLine" Height="60px" Width="300px"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvCommAdress" runat="server" ControlToValidate="txtCommAdress" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Communication Address" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommCity" runat="server" MaxLength="60" ToolTip="Communication City"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvCommCity" runat="server" ControlToValidate="txtCommCity" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Communication City" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommState" runat="server" CssClass="styleReqFieldLabel" Text="State"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <%-- <cc1:ComboBox ID="txtCommState" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend" MaxLength="30"
                                                            Width="80px">
                                                        </cc1:ComboBox>
                                                        <asp:RequiredFieldValidator ID="rfvCommState" runat="server" ControlToValidate="txtCommState" SetFocusOnError="true" InitialValue="0"
                                                            ErrorMessage="Enter the Communication State" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                        </asp:RequiredFieldValidator>--%>
                                                                <uc2:Suggest ID="txtCommState" runat="server" ServiceMethod="GetStateCmnList" IsMandatory="true" ValidationGroup="vsSave"
                                                                    ErrorMessage="Enter the Communication State" ToolTip="Communication State" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommCountry" runat="server" CssClass="styleReqFieldLabel" Text="Country"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <cc1:ComboBox ID="txtCommCountry" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend" MaxLength="30"
                                                                    Width="80px">
                                                                </cc1:ComboBox>
                                                                <asp:RequiredFieldValidator ID="rfvCommCountry" runat="server" ControlToValidate="txtCommCountry" SetFocusOnError="true" InitialValue="0"
                                                                    ErrorMessage="Enter the Communication Country" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommPincode" runat="server" CssClass="styleReqFieldLabel" Text="Pincode"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommPincode" runat="server" MaxLength="10" ToolTip="Communication Pincode"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvCommPincode" runat="server" ControlToValidate="txtCommPincode" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Communication Pincode" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCommPincode" runat="server" Enabled="true" TargetControlID="txtCommPincode"
                                                                    FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommTelephoneNo" runat="server" Text="Telephone No"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommTelephoneNo" runat="server" MaxLength="12" ToolTip="Communication Telephone No"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCommTelephoneNo" runat="server" Enabled="true" TargetControlID="txtCommTelephoneNo"
                                                                    FilterType="Numbers,Custom" ValidChars="+-">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvCommTelephoneNo" runat="server" ControlToValidate="txtCommTelephoneNo" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Communication Telephone" Enabled="false" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommMobileNo" runat="server" CssClass="styleDisplayLabel" Text="Mobile No"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommMobileNo" runat="server" MaxLength="12" ToolTip="Communication Mobile No"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCommMobileNo" runat="server" Enabled="true" TargetControlID="txtCommMobileNo"
                                                                    FilterType="Numbers,Custom" ValidChars="+">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommEmailID" runat="server" CssClass="styleDisplayLabel" Text="Email ID"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommEmailID" runat="server" MaxLength="180" ToolTip="Communication Email ID" Width="300px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator
                                                                    ID="revCommEmailId" runat="server" ControlToValidate="txtCommEmailID" Display="None" ValidationGroup="vsSave"
                                                                    ErrorMessage="Enter a valid Communication Email ID" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;|,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*">
                                                                </asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommTAN" runat="server" CssClass="styleDisplayLabel" Text="TAN"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommTAN" runat="server" MaxLength="60" ToolTip="Communication TAN"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCommTIN" runat="server" CssClass="styleDisplayLabel" Text="TIN"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCommTIN" runat="server" MaxLength="11" ToolTip="Communication TIN"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeCommTIN" runat="server" Enabled="true" TargetControlID="txtCommTIN"
                                                                    FilterType="Numbers">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>

                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblSGSTIN" runat="server" Text="GSTIN"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtSGSTin" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="filsgstin" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                    TargetControlID="txtSGSTin" runat="server" Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <%--<asp:RequiredFieldValidator ID="rfvSgstin" runat="server" Display="None" ErrorMessage="Enter the SGSTIN"
                                                    ControlToValidate="txtSGSTin" ValidationGroup="Baddress" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                            </td>


                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblSGSTREGDate" runat="server" Text="GST Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="TxtSGSTRegDate" runat="server" Width="100px"></asp:TextBox>
                                                                <asp:Image ID="imgSGSTRegDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <%--<asp:RequiredFieldValidator ID="rfvSgstregdate" runat="server" Display="None"
                                                    ErrorMessage="Enter SGST Registration Date" ValidationGroup="Baddress" ControlToValidate="TxtSGSTRegDate"
                                                    SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>--%>
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="TxtSGSTRegDate"
                                                                    PopupButtonID="imgSGSTRegDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                    ID="CalendarExtender3" Enabled="True">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
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
        <cc1:TabPanel runat="server" HeaderText="Limit Setting" ID="tpLimitSetting" CssClass="tabpan"
            BackColor="Red" ToolTip="Limit Setting" Width="99%">
            <HeaderTemplate>
                Limit Setting
            </HeaderTemplate>
            <ContentTemplate>
                <asp:UpdatePanel ID="updtpnlLesseeInfo" runat="server">
                    <ContentTemplate>
                        <table width="100%" align="center">
                            <tr>
                                <td>
                                    <asp:Label ID="lblLimitFunderName" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table width="50%">
                                        <tr>
                                            <td width="15%">
                                                <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel"></asp:Label>
                                            </td>
                                            <td width="40%">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="70%">
                                                            <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetCustomerList" Width="240px" ValidationGroup="vsSanction"
                                                                IsMandatory="true" ErrorMessage="Select Lessee Name" />
                                                        </td>
                                                        <td width="30%" align="left">
                                                            <asp:CheckBox ID="chkZeroRcrd" runat="server" ToolTip="Include 0 Balance Sanction Details" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td width="10%">
                                                <asp:Label ID="lblviewall" runat="server" Text="View All" ToolTip="View All" CssClass="styleDisplayLabel"></asp:Label>
                                            </td>
                                            <td width="35%" align="left">
                                                <asp:Button ID="btnLesseeSearch" runat="server" Text="Search" CssClass="styleSubmitButton" ValidationGroup="vsSanction"
                                                    OnClick="btnLesseeSearch_Click" ToolTip="Search Lessee Sanction Details" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlLesseeInformation" Width="99%" runat="server" CssClass="stylePanel" GroupingText="Lessee Wise Limit Details for a Funder">
                                        <div class="container" style="max-height: 440px; width: 99%; overflow-y: auto; overflow-x: hidden; vertical-align: middle; margin-left: 5px">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:GridView Width="100%" runat="server" ID="grvLesseeDetails" AutoGenerateColumns="False" EnableModelValidation="true"
                                                            ShowFooter="true" OnRowDataBound="grvLesseeDetails_RowDataBound">
                                                            <PagerStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSlNo" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                                                        <asp:Label ID="lblgdLesseeFunderID" runat="server" Visible="false" Text='<%# Bind("Lessee_Funder_ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Lessee Name*">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEditCustomerName" runat="server" MaxLength="100" Text='<%# Bind("Customer_Name")%>' ReadOnly="true" ToolTip="Lessee Name" Width="180px"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="autoEditCustomerName" MinimumPrefixLength="3" OnClientPopulated="Customer_ItemPopulated"
                                                                            OnClientItemSelected="Customer_ItemSelected" runat="server" TargetControlID="txtEditCustomerName"
                                                                            ServiceMethod="GetCustomerList" Enabled="true" ServicePath="" CompletionSetCount="2"
                                                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <cc1:TextBoxWatermarkExtender ID="txtEditCustomerSearchExtender" runat="server" TargetControlID="txtEditCustomerName"
                                                                            WatermarkText="--Select--">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:Label ID="lblgdCustomerID" runat="server" Visible="false" Text='<%# Bind("Customer_ID")%>'></asp:Label>
                                                                        <asp:RequiredFieldValidator ID="rfvEditCustomerName" runat="server" ControlToValidate="txtEditCustomerName" Enabled="true"
                                                                            ErrorMessage="Enter the Lessee Name" Display="None" ValidationGroup="vsEditCustomer"></asp:RequiredFieldValidator>
                                                                        <asp:Label ID="lblEditCustomerID" runat="server" Visible="false" Text='<%# Bind("Customer_ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <uc2:Suggest ID="txtCustomerName" runat="server" ServiceMethod="GetCustomerList" IsMandatory="true" ValidationGroup="vsCustomer" ToolTip="Lessee Name"
                                                                            ErrorMessage="Enter the Lessee Name" AutoPostBack="true" OnItem_Selected="txtCustomerName_Item_Selected" Width="180px" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="20%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Asset Category*">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEditAssetCategory" runat="server" MaxLength="100" Text='<%# Bind("Asset_Category_Desc")%>' ReadOnly="true"
                                                                            Width="180px"></asp:TextBox>
                                                                        <asp:Label ID="lblgdAssetCategoryID" runat="server" Visible="false" Text='<%# Bind("Asset_Category_ID")%>'></asp:Label>
                                                                        <cc1:AutoCompleteExtender ID="autoEditAssetName" MinimumPrefixLength="3" OnClientPopulated="Asset_ItemPopulated"
                                                                            OnClientItemSelected="Asset_ItemSelected" runat="server" TargetControlID="txtEditAssetCategory"
                                                                            ServiceMethod="GetAssetCategoryList" Enabled="true" ServicePath="" CompletionSetCount="2"
                                                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <cc1:TextBoxWatermarkExtender ID="txtEditAssetSearchExtender" runat="server" TargetControlID="txtEditAssetCategory"
                                                                            WatermarkText="--Select--">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvEditAssetCategory" runat="server" ControlToValidate="txtEditAssetCategory" Enabled="true"
                                                                            ErrorMessage="Enter the Asset Category" Display="None" ValidationGroup="vsEditCustomer"></asp:RequiredFieldValidator>
                                                                        <asp:Label ID="lblEditAssetCategoryID" runat="server" Visible="false" Text='<%# Bind("Asset_Category_ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <uc2:Suggest ID="txtgdAssetCategory" runat="server" ServiceMethod="GetAssetCategoryList" ValidationGroup="vsCustomer"
                                                                            ToolTip="Asset Category" Width="180px" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="20%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Sanction Ref. No*">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEditSanctionNo" runat="server" MaxLength="50" Width="150px" Text='<%# Bind("Sanction_Ref_No")%>' ReadOnly="true"></asp:TextBox>
                                                                        <asp:Label ID="lblgdSanctionNo" runat="server" Visible="false" Text='<%# Bind("Sanction_Ref_No")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtgdSanctionRefNo" runat="server" MaxLength="50" Width="150px" ToolTip="Sanction Reference No"
                                                                            OnTextChanged="txtgdSanctionRefNo_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvgdSanctionNo" runat="server" ControlToValidate="txtgdSanctionRefNo" Enabled="true"
                                                                            ErrorMessage="Enter the Sanction Ref. No" Display="None" ValidationGroup="vsCustomer"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Sanction Date*">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEditSanctionDate" runat="server" MaxLength="12" Width="80px" Text='<%# Bind("Sanction_Date")%>' ReadOnly="true"></asp:TextBox>
                                                                        <asp:Image ID="imgEditSanctionDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="False" />
                                                                        <cc1:CalendarExtender ID="ceEditSanctionDate" runat="server" Enabled="false"
                                                                            PopupButtonID="imgEditSanctionDate"
                                                                            TargetControlID="txtEditSanctionDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvEditSanctionDate" runat="server" ControlToValidate="txtEditSanctionDate" Enabled="true"
                                                                            ErrorMessage="Enter the Sanction Date" Display="None" ValidationGroup="vsEditCustomer"></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtgdSanctionDate" runat="server" MaxLength="12" Width="80px" ToolTip="Sanction Date"></asp:TextBox>
                                                                        <asp:Image ID="imgSanctionDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="False" />
                                                                        <cc1:CalendarExtender ID="ceSanctionDate" runat="server" Enabled="true"
                                                                            PopupButtonID="imgSanctionDate"
                                                                            TargetControlID="txtgdSanctionDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvgdSanctionDate" runat="server" ControlToValidate="txtgdSanctionDate" Enabled="true"
                                                                            ErrorMessage="Enter the Sanction Date" Display="None" ValidationGroup="vsCustomer"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Sanction Limit*">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEditSanctionLimit" runat="server" MaxLength="13" Width="120px" Text='<%# Bind("Limit")%>'
                                                                            Style="text-align: right" onblur="ChkIsZero(this)" ReadOnly="true"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftbeEditLimit" runat="server" Enabled="true" TargetControlID="txtEditSanctionLimit"
                                                                            FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvEditSanctionLimit" runat="server" ControlToValidate="txtEditSanctionLimit" Enabled="true"
                                                                            ErrorMessage="Enter the Sanction Limit" Display="None" ValidationGroup="vsEditCustomer"></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtSanctionLimit" runat="server" Width="120px" MaxLength="13" onblur="ChkIsZero(this)" ToolTip="Sanction Limit"
                                                                            Style="text-align: right" OnTextChanged="txtSanctionLimit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftbeLimit" runat="server" Enabled="true" TargetControlID="txtSanctionLimit"
                                                                            FilterType="Numbers">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvgdSanctionLimit" runat="server" ControlToValidate="txtSanctionLimit" Enabled="true"
                                                                            ErrorMessage="Enter the Sanction Limit" Display="None" ValidationGroup="vsCustomer"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Transfer Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgdTransferAmount" runat="server" Text='<%# Bind("Transfer_Amount")%>' Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Utilized Limit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgdUtilizedLimit" runat="server" Text='<%# Bind("Utilized_Limit")%>' Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Balance Limit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgdBalanceLimit" runat="server" Text='<%# Bind("Balance_Limit")%>' Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Expiry Date*">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtEditExpiryDate" runat="server" MaxLength="12" Width="80px" Text='<%# Bind("Expiry_Date")%>' ReadOnly="true"></asp:TextBox>
                                                                        <asp:Image ID="imgEditExpiryDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="False" />
                                                                        <cc1:CalendarExtender ID="ceEditExpiryDate" runat="server" Enabled="false"
                                                                            PopupButtonID="imgEditExpiryDate"
                                                                            TargetControlID="txtEditExpiryDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvEditExpiryDate" runat="server" ControlToValidate="txtEditExpiryDate" Enabled="true"
                                                                            ErrorMessage="Enter the Expiry Date" Display="None" ValidationGroup="vsEditCustomer"></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtgdExpiryDate" runat="server" MaxLength="12" Width="80px" ToolTip="Expiry Date"></asp:TextBox>
                                                                        <asp:Image ID="imgExpiryDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="False" />
                                                                        <cc1:CalendarExtender ID="ceExpiryDate" runat="server" Enabled="true"
                                                                            PopupButtonID="imgExpiryDate"
                                                                            TargetControlID="txtgdExpiryDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvgdExpiryDate" runat="server" ControlToValidate="txtgdExpiryDate" Enabled="true"
                                                                            ErrorMessage="Enter the Expiry Date" Display="None" ValidationGroup="vsCustomer"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnEditLesseeInfo" runat="server" ImageUrl="~/Images/Edit.JPG" OnClick="imgbtnEditLesseeInfo_Click" ToolTip="Edit Additional Details" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:ImageButton ID="imgbtnAddLesseeInfo" runat="server" ImageUrl="~/Images/plusCo.png" OnClick="imgbtnAddLesseeInfo_Click" ToolTip="Add Addition Details" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Upload">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDocumentProof" ToolTip="Document Upload" Width="80px" runat="server"
                                                                            Text='<%#Eval("Document_Upload")%>' Visible="false"></asp:Label>
                                                                        <asp:ImageButton ID="hyplnkDocView" CommandArgument='<%# Bind("Document_Upload") %>'
                                                                            OnClick="hyplnkDocView_Click" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                            runat="server" ToolTip="View Document" />
                                                                        <asp:UpdatePanel ID="tempUpdate1" runat="server" UpdateMode="Conditional">
                                                                            <ContentTemplate>
                                                                                <asp:Button ID="btnBrowse1" runat="server" OnClick="btnBrowse1_Click" Style="display: none" Visible="false"></asp:Button>
                                                                                <asp:FileUpload runat="server" ID="flUpload1" Width="150px" ToolTip="Upload File" Visible="false" />
                                                                                <asp:TextBox ID="txtFileupld1" runat="server" Style="position: absolute; margin-left: -153px; width: 65px" ReadOnly="true" Visible="false" />
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="btnBrowse1" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentPath" runat="server" Style="position: absolute; color: Green; vertical-align: top" />
                                                                        <asp:UpdatePanel ID="tempUpdate" runat="server">
                                                                            <ContentTemplate>
                                                                                <asp:HiddenField ID="hdnSelectedPath" runat="server" />
                                                                                <asp:Button ID="btnBrowse" runat="server" OnClick="btnBrowse_OnClick" Style="display: none" />
                                                                                <table align="left" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td style="padding: 0px">
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" Enabled="false" Text="" Width="20px" />
                                                                                        </td>
                                                                                        <td style="padding: 0px">
                                                                                            <asp:FileUpload ID="flUpload" runat="server" ToolTip="Upload File" Width="100px" />
                                                                                            <asp:Button ID="btnDlg" runat="server" CausesValidation="false" CssClass="styleGridShortButton" Style="display: none" Text="Browse" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:PostBackTrigger ControlID="btnBrowse" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                        <br />
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkLesseeEdit" runat="server" Text="Edit" OnClick="lnkLesseeEdit_Click" ToolTip="Edit Details"></asp:LinkButton>
                                                                        <asp:LinkButton ID="lnkLesseeRemove" runat="server" Text="Remove" OnClick="lnkLesseeRemove_Click" ToolTip="Delete Details"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnLesseeAdd" runat="server" CssClass="styleSubmitButton" Text="Add" OnClick="lnkLesseeADD_Click" ToolTip="Add Details"
                                                                            ValidationGroup="vsCustomer" Width="50px" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    <FooterStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSancStatus" runat="server" Text='<%#Eval("Status_Desc")%>' Width="80px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr align="center" width="98%">
                                                    <td>
                                                        <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <span runat="server" id="lblPagingErrorMessage" style="color: Red; font-size: medium"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                    <asp:HiddenField ID="hdnCustomerID" runat="server" />
                                    <asp:HiddenField ID="hdnAssetID" runat="server" />
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlLimit" Width="99%" runat="server" CssClass="stylePanel" GroupingText="Limit Consolidation">
                                        <div class="container" style="max-height: 220px; width: 100%; overflow-y: auto; overflow-x: hidden; margin-left: 5px;">
                                            <asp:GridView Width="95%" runat="server" ID="grvLimitConsolidation" AutoGenerateColumns="False" EnableModelValidation="true" ShowFooter="true"
                                                OnRowDataBound="grvLimitConsolidation_RowDataBound" AllowPaging="true" PageSize="10" OnPageIndexChanging="grvLimitConsolidation_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblConsSlNo" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                                            <asp:Label ID="lblgdDetailID" runat="server" Visible="false" Text='<%# Bind("Consolidation_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <FooterStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Existing Sanction No*">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgdExistingSanctionNo" runat="server" Text='<%# Bind("Existing_sanction_No")%>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                                            <asp:Label ID="lblgdExsitingSanctionID" runat="server" Visible="false" Text='<%# Bind("Existing_sanction_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="ddlSanctionNo" runat="server" ServiceMethod="GetSanctionNoList" AutoPostBack="true" IsMandatory="true" OnItem_Selected="ddlSanctionNo_Item_Selected"
                                                                ErrorMessage="Enter a Existing Sanction No" ValidationGroup="vsConsolidation" Width="200px" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                        <FooterStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Balance Unit*">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgdBalanceUnit" runat="server" Text='<%# Bind("Balance_Unit")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblExistBalanceLimit" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                        <FooterStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Transfer Amount*">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTransferAmount" runat="server" Text='<%# Bind("Transfer_Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtTransferAmount" runat="server" MaxLength="13" onblur="ChkIsZero(this)" Style="text-align: right"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftbeTransferAmount" runat="server" Enabled="true" TargetControlID="txtTransferAmount"
                                                                FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                        <FooterStyle Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="New Sanction No*">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNewSanctionID" runat="server" Text='<%# Bind("New_Sanction_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblNewSanctionNo" runat="server" Text='<%# Bind("New_Sanction_No") %>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="ddlNewSanctionNo" runat="server" ServiceMethod="GetNewSanctionNoList" IsMandatory="true" ErrorMessage="Enter a New Sanction No"
                                                                ValidationGroup="vsConsolidation" Width="200px" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                        <FooterStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkRemoveConsolidation" runat="server" Text="Remove" OnClick="lnkRemoveConsolidation_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnIncludeConsolidation" runat="server" CssClass="styleSubmitButton" Text="Include" OnClick="btnIncludeConsolidation_Click"
                                                                ValidationGroup="vsConsolidation" />
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                        <FooterStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                    <asp:HiddenField ID="hdnNewSanctionID" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="Bank Details" ID="tpBankDetail" CssClass="tabpan"
            BackColor="Red" ToolTip="Bank Details" Width="99%">
            <HeaderTemplate>
                Bank Details
            </HeaderTemplate>
            <ContentTemplate>
                <asp:UpdatePanel ID="updtpnlBankDtl" runat="server">
                    <ContentTemplate>
                        <table width="100%" align="center">
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblBankName" runat="server" CssClass="styleReqFieldLabel" Text="Bank Name"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtBankName" runat="server" MaxLength="60" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBankName" runat="server" Display="None" ControlToValidate="txtBankName"
                                        ErrorMessage="Enter the Bank Name" ValidationGroup="vgBank" SetFocusOnError="true" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblAccountType" runat="server" CssClass="styleReqFieldLabel" Text="Account Type"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlAccountType" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvAccountType" runat="server" Display="None" ControlToValidate="ddlAccountType"
                                        ErrorMessage="Select the Account Type" ValidationGroup="vgBank" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblBenificiaryName" runat="server" CssClass="styleReqFieldLabel" Text="Beneficiary Account Name"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtBenificiaryName" runat="server" MaxLength="60" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBenificiaryName" runat="server" Display="None" ControlToValidate="txtBenificiaryName"
                                        ErrorMessage="Enter the Beneficiary Account Name" ValidationGroup="vgBank" SetFocusOnError="true" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblAccountNo" runat="server" CssClass="styleReqFieldLabel" Text="Beneficairy Account No"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtAccountNo" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAccountNo" runat="server" Display="None" ControlToValidate="txtAccountNo"
                                        ErrorMessage="Enter the Beneficairy Account No" ValidationGroup="vgBank" SetFocusOnError="true" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblBankCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtBankCity" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBankCity" runat="server" Display="None" ControlToValidate="txtBankCity"
                                        ErrorMessage="Enter the City" ValidationGroup="vgBank" SetFocusOnError="true" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel" rowspan="3">
                                    <asp:Label ID="lblBankBranch" runat="server" CssClass="styleReqFieldLabel" Text="Branch"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" rowspan="3">
                                    <asp:TextBox ID="txtBankBranch" runat="server" onkeyup="maxlengthfortxt(200);" TextMode="MultiLine" Width="300px" Height="60px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvBankBranch" runat="server" Display="None" ControlToValidate="txtBankBranch"
                                        ErrorMessage="Enter the Branch" ValidationGroup="vgBank" SetFocusOnError="true" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblMICR" runat="server" CssClass="styleDisplayLabel" Text="MICR Code"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtMICRCode" runat="server" MaxLength="10" onblur="jsMICRvaildate(this);"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="ftxtMICRCode" runat="server" FilterType="Numbers"
                                        Enabled="true" TargetControlID="txtMICRCode">
                                    </cc1:FilteredTextBoxExtender>
                                    <%--                                    <asp:RequiredFieldValidator ID="rfvMICRCode" runat="server" Display="None" ControlToValidate="txtMICRCode"
                                        ErrorMessage="Enter the MICR Code" ValidationGroup="vgBank" SetFocusOnError="true" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblIFSC" runat="server" CssClass="styleDisplayLabel" Text="IFSC Code"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtIFSCCode" runat="server" MaxLength="11" onblur="fnValidateIFSCCode(this)"></asp:TextBox>
                                    <%--                                    <asp:RequiredFieldValidator ID="rfvIFSCCode" runat="server" Display="None" ControlToValidate="txtIFSCCode"
                                        ErrorMessage="Enter the IFSC Code" ValidationGroup="vgBank" SetFocusOnError="true" CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>--%>
                                    <%--<asp:RegularExpressionValidator
                                        ID="refvISFCCode" runat="server" ControlToValidate="txtIFSCCode"
                                        ErrorMessage="Enter a valid IFSC Code" ValidationExpression="[A-Z|a-z]{4}[0][\d]{6}$">
                                    </asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="right">
                                    <asp:CheckBox ID="chkActiveAccount" runat="server" Text="Active Account" />
                                    <asp:CheckBox ID="chkDefaultAccount" runat="server" Text="Default Account" />
                                    <asp:Button runat="server" ID="btnBankAdd" CssClass="styleSubmitButton" Text="Add" Width="60px" ValidationGroup="vgBank"
                                        OnClick="btnBankAdd_Click" />
                                    <asp:Button runat="server" ID="btnBankModify" CssClass="styleSubmitButton" Text="Modify" Width="60px" Enabled="false"
                                        ValidationGroup="vgBank" OnClick="btnBankModify_Click" />
                                    <asp:Button runat="server" ID="btnBankClear" CssClass="styleSubmitButton" Text="Clear" Width="60px" OnClientClick="return fnClearBankDtls();" UseSubmitBehavior="False" />
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlBankDtls" ToolTip="Bank Details" runat="server" CssClass="stylePanel" GroupingText="Bank Details" Height="300px">
                            <table width="100%">
                                <tr valign="top">
                                    <td>
                                        <%--<div class="container" style="max-height: 220px; width: 100%;">--%>
                                        <asp:GridView Width="98%" runat="server" ID="grvBankDetails" AutoGenerateColumns="False" EnableModelValidation="true" CssClass="styleGridView"
                                            HorizontalAlign="Center" OnSelectedIndexChanged="grvBankDetails_SelectedIndexChanged" OnRowDataBound="grvBankDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Bank Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdBankName" runat="server" Text='<%# Bind("Bank_Name")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                        <asp:Label ID="lblgdBankID" runat="server" Visible="false" Text='<%# Bind("Bank_ID")%>'></asp:Label>
                                                        <asp:Label ID="lblTransaction" runat="server" Visible="false" Text='<%# Bind("Is_Transaction")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdAccountType" runat="server" Text='<%# Bind("Account_Type_Desc")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                        <asp:Label ID="lblAccountTypeID" runat="server" Text='<%# Bind("Account_Type_ID")%>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Benificiary Account Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdBenificiaryName" runat="server" Text='<%# Bind("Benificiary_name")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Benificiary Account No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdBenificiaryNo" runat="server" Text='<%# Bind("Account_No")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdBankCity" runat="server" Text='<%# Bind("City")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdBranch" runat="server" Text='<%# Bind("Branch")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MICR">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdMicr" runat="server" Text='<%# Bind("MICR_Code")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IFSC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdIFSC" runat="server" Text='<%# Bind("IFSC")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Active">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsActiveAccount" runat="server" Enabled="false" Checked='<%# Eval("Is_Active").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Default Account">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsDefaultAccount" runat="server" Enabled="false" Checked='<%# Eval("Is_DefaultAccount").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBankRemove" runat="server" Text="Remove" OnClick="lnkBankRemove_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <%--</div>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlBankHistory" ToolTip="Bank History" runat="server" CssClass="stylePanel" GroupingText="Bank History" Width="100%" Height="300px">
                            <table width="99%">
                                <tr>
                                    <td>
                                        <%--<div class="container" style="max-height: 50px; width: 100%; overflow-y: auto; overflow-x: hidden;">--%>
                                        <asp:GridView Width="100%" runat="server" ID="grvBankHistory" AutoGenerateColumns="False" CssClass="styleGridView"
                                            HorizontalAlign="Center">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Bank Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstBankName" runat="server" Text='<%# Bind("Bank_Name")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstAccountType" runat="server" Text='<%# Bind("Account_Type_Desc")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Benificiary Account Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstBenificiaryName" runat="server" Text='<%# Bind("Benificiary_name")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Benificiary Account No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstBenificiaryNo" runat="server" Text='<%# Bind("Account_No")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstBankCity" runat="server" Text='<%# Bind("City")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstBranch" runat="server" Text='<%# Bind("Branch")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="200px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MICR">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstMicr" runat="server" Text='<%# Bind("MICR_Code")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IFSC">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdhstIFSC" runat="server" Text='<%# Bind("IFSC")%>' Style="word-wrap: normal; word-break: break-all;"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" Wrap="true" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <%--</div>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="Rental Schedule" ID="tbRSDetails" CssClass="tabpan"
            BackColor="Red" ToolTip="Rental Schedule Details" Width="99%">
            <HeaderTemplate>
                Rental Schedule
            </HeaderTemplate>
            <ContentTemplate>
                <asp:Panel ID="pnlRSDetails" Width="99%" ToolTip="Bank Details" runat="server" CssClass="stylePanel">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlSearchType" runat="server"></asp:DropDownList>
                                <asp:TextBox ID="txtSearchText" runat="server" Text="" Width="180px" onkeyup="maxlengthfortxt(60);"></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="styleSubmitButton" OnClick="btnSearch_Click" ValidationGroup="vsRSInfo" />
                                <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton" OnClick="btnShowAll_Click" />
                                <asp:RequiredFieldValidator ID="rfvSearchType" runat="server" ControlToValidate="ddlSearchType" SetFocusOnError="true"
                                    ErrorMessage="Select the Search Type" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsRSInfo"
                                    InitialValue="0">
                                </asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="rfvSearchText" runat="server" ControlToValidate="txtSearchText" SetFocusOnError="true"
                                    ErrorMessage="Enter the Search Text" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsRSInfo">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div class="container" style="max-height: 220px; width: 100%; overflow-y: auto; overflow-x: hidden;">
                                    <asp:GridView Width="100%" runat="server" ID="grvRSDetail" AutoGenerateColumns="False" EnableModelValidation="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRSSlNo" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                <FooterStyle HorizontalAlign="Left" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sanction Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdRSSanctionNo" runat="server" Text='<%# Bind("Sanction_No")%>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Lessee Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdLesseeName" runat="server" Text='<%# Bind("Lessee_Name")%>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                                    <asp:Label ID="lblgdRSID" runat="server" Visible="false" Text='<%# Bind("RS_ID")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tranche Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdTrancheNo" runat="server" Text='<%# Bind("Tranche_No")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rental Schd. No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdRSNo" runat="server" Text='<%# Bind("RS_No")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tenure">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdTenure" runat="server" Text='<%# Bind("Tenure")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Asset Category">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdAssetCategory" runat="server" Text='<%# Bind("Asset_Category")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Sanction Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdSanctionAmt" runat="server" Text='<%# Bind("Sanction_Amount")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Utilized Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdUtilizedAmt" runat="server" Text='<%# Bind("Utilized_Amount")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance Sanction Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdBalanceAmt" runat="server" Text='<%# Bind("Balance_Amount")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Maturity Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgdMaturityDate" runat="server" Text='<%# Bind("Maturity_Date")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="styleGridHeader" />
                                        <RowStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr align="center" width="100%">
                            <td>
                                <uc1:PageNavigator ID="ucCustomPagingSummary" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" width="100%">
                                <span runat="server" id="lblSummaryErrMsg" style="color: Red; font-size: medium"></span>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr class="styleButtonArea">
            <td align="center">
                <asp:Button runat="server" ID="btnSave" ValidationGroup="vsSave" OnClientClick="return fnCheckPageValidators('vsSave');"
                    CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Clear" OnClientClick="return confirm('Do you want to cancel this record?');" OnClick="btnClear_Click" />
                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                    CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr class="styleButtonArea">
            <td>
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary runat="server" ID="vsSave" ValidationGroup="vsSave"
                    HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsCustomer" ValidationGroup="vsCustomer"
                    HeaderText="Please correct the following validation(s):" Height="200px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsEditCustomer" ValidationGroup="vsEditCustomer"
                    HeaderText="Please correct the following validation(s):" Height="200px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsConsolidation" ValidationGroup="vsConsolidation"
                    HeaderText="Please correct the following validation(s):" Height="200px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vgBank" ValidationGroup="vgBank"
                    HeaderText="Please correct the following validation(s):" Height="200px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsRSInfo" ValidationGroup="vsRSInfo"
                    HeaderText="Please correct the following validation(s):" Height="200px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="vsSanction" ValidationGroup="vsSanction"
                    HeaderText="Please correct the following validation(s):" Height="200px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:CustomValidator ID="cvFunder" runat="server" CssClass="styleMandatoryLabel"
                    Enabled="true" Width="98%" />
            </td>
        </tr>
    </table>
    <cc1:ModalPopupExtender ID="moeAdditionalInfo" runat="server" TargetControlID="btnModal" PopupControlID="pnlLesseeAdditioInfo"
        BackgroundCssClass="styleModalBackground" Enabled="true" />
    <asp:Panel ID="pnlLesseeAdditioInfo" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="80%">
        <div id="divAdditional" runat="server" style="height:400px; overflow-x:auto;" >
            <asp:UpdatePanel ID="upPass" runat="server">
                <ContentTemplate>
                    <table width="95%">
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblPVCalcMethod" runat="server" CssClass="styleReqFieldLabel" Text="PV Calc Method"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlPVCalcMethod" runat="server"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPVCalcMethod" runat="server" InitialValue="0" ValidationGroup="vsLessee"
                                    ControlToValidate="ddlPVCalcMethod" ErrorMessage="Select the PV Calc Method" Enabled="true" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblDiscountRate" runat="server" CssClass="styleReqFieldLabel" Text="Discount Rate(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtDiscountRate" runat="server" MaxLength="18" onchange="NetDiscountRate();"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeDiscountRate" runat="server" Enabled="true" TargetControlID="txtDiscountRate"
                                    FilterType="Custom, Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvDiscountRate" runat="server" ValidationGroup="vsLessee"
                                    ControlToValidate="txtDiscountRate" ErrorMessage="Enter the Discounting Rate" Enabled="true" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblProcessingFeePerc" runat="server" CssClass="styleDisplayLabel" Text="Processing Fee(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtProcessingFeePerc" runat="server" MaxLength="5" AutoPostBack="true" OnTextChanged="txtProcessingFeePerc_TextChanged"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeProcessingFeePerc" runat="server" Enabled="true" TargetControlID="txtProcessingFeePerc"
                                    FilterType="Custom, Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblProcessingFee" runat="server" CssClass="styleDisplayLabel" Text="Processing Fee"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtProcessingFee" runat="server" MaxLength="10" AutoPostBack="true" OnTextChanged="txtProcessingFee_TextChanged"
                                    onkeypress="fnAllowNumbersOnly(true,false,this)" Style="text-align: right"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeProcessingFee" runat="server" Enabled="true" TargetControlID="txtProcessingFee"
                                    FilterType="Custom,Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>

                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblpfservicetax" runat="server" CssClass="styleDisplayLabel" Text="Processing Fee ST"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtPFServiceTax" runat="server" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblTotalPf" runat="server" CssClass="styleDisplayLabel" Text="Total Processing Fee"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtTotalPF" runat="server" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblForeclosureRate" runat="server" CssClass="styleDisplayLabel" Text="Fore Closure Rate(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtForeClosureRate" runat="server" MaxLength="5" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeForeClosureRate" runat="server" Enabled="true" TargetControlID="txtForeClosureRate"
                                    FilterType="Custom, Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblMiscCharges" runat="server" CssClass="styleDisplayLabel" Text="Misc Charges"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtMiscCharges" runat="server" MaxLength="10" Style="text-align: right"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeMiscCharges" runat="server" Enabled="true" TargetControlID="txtMiscCharges"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblTenor" runat="server" CssClass="styleReqFieldLabel" Text="Tenor"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtTenor" runat="server" MaxLength="4" Style="text-align: right"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeTenor" runat="server" Enabled="true" TargetControlID="txtTenor"
                                    FilterType="Numbers">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvTenor" runat="server" ValidationGroup="vsLessee" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                    ControlToValidate="txtTenor" ErrorMessage="Enter the Tenor" Enabled="true" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblOverDueRate" runat="server" CssClass="styleDisplayLabel" Text="Over Due Rate(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtOverDueRate" runat="server" MaxLength="5" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeOverDueRate" runat="server" Enabled="true" TargetControlID="txtOverDueRate"
                                    FilterType="Custom, Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>

                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblEndCustomer" runat="server" CssClass="styleDisplayLabel" Text="End Customer"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <uc2:Suggest ID="ddlEndCustomer" runat="server" ServiceMethod="GetEndCustomerLst" ToolTip="End Customer" />
                            </td>

                            <td class="styleFieldLabel">
                                <asp:Label ID="lblChqRtnChrgs" runat="server" CssClass="styleDisplayLabel" Text="Cheq Retn Chrgs"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtChqRtnChrgs" runat="server" MaxLength="7" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="ftbeChqRtnChrgs" runat="server" Enabled="true" TargetControlID="txtChqRtnChrgs"
                                    FilterType="Custom, Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblDiscountProcessing" runat="server" CssClass="styleDisplayLabel" Text="Discounting Net off Processing Fee(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:CheckBox ID="chkDiscountProcessing" runat="server" onclick="NetDiscountRatePF();" />
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblUpfrontInt" runat="server" CssClass="styleDisplayLabel" Text="Upfront Interest(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtUpfrontInt" runat="server" MaxLength="5" onchange="NetDiscountRate();" onkeypress="fnAllowNumbersOnly(true,false,this)" Style="text-align: right;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" TargetControlID="txtUpfrontInt"
                                    FilterType="Custom, Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>

                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblUpfront" runat="server" CssClass="styleDisplayLabel" Text="Discounting Net off Upfront Interest(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:CheckBox ID="chkUpfront" runat="server" onclick="NetDiscountRateUR();" />
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblNetDiscountRate" runat="server" CssClass="styleDisplayLabel" Text="Net Discount Rate(%)"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtNetDiscountRate" runat="server" MaxLength="10" Style="text-align: right;"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="true" TargetControlID="txtNetDiscountRate"
                                    FilterType="Custom,Numbers" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                            </td>

                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblCollateralDtls" runat="server" CssClass="styleDisplayLabel" Text="Collateral Details"></asp:Label>
                                <asp:Label ID="lblSearchSanctionNo" runat="server" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblSearchAssetCategoryID" runat="server" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblSearchCustomerID" runat="server" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblEditAssetID" runat="server" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblSerachSanctionDate" runat="server" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblSrchFndrDtlID" runat="server" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" colspan="3">
                                <asp:TextBox ID="txtCollateralDetails" runat="server" TextMode="MultiLine" onkeyup="maxlengthfortxt(500);" Height="40px" Width="450px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblRemarks" runat="server" CssClass="styleDisplayLabel" Text="Remarks"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" colspan="3">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" onkeyup="maxlengthfortxt(500);" Height="40px" Width="450px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblPopupLimit" runat="server" CssClass="styleDisplayLabel" Text="Limit" Visible="false"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" colspan="3">
                                <asp:TextBox ID="txtPopupLimit" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Button runat="server" ID="btnLesseeAdd" CssClass="styleSubmitButton" Text="Add" Width="60px" ValidationGroup="vsLessee"
                                    OnClick="btnLesseeAdd_Click" Visible="false" />
                                <asp:Button runat="server" ID="btnLesseeUpdate" CssClass="styleSubmitButton" Text="Modify" Width="60px" ValidationGroup="vsLessee"
                                    OnClick="btnLesseeUpdate_Click" Visible="false" />
                                <asp:Button runat="server" ID="btnLesseeClose" CssClass="styleSubmitButton" Text="Close" Width="60px" OnClick="btnLesseeClose_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:ValidationSummary runat="server" ID="vsLessee" ValidationGroup="vsLessee"
                                    HeaderText="Please correct the following validation(s):" Height="200px" CssClass="styleMandatoryLabel"
                                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                                <asp:CustomValidator ID="cvLessee" runat="server" CssClass="styleMandatoryLabel"
                                    Enabled="true" Width="98%" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:Button ID="btnModal" Style="display: none" runat="server" />
</asp:Content>

