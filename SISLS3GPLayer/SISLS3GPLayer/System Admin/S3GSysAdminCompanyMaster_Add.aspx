<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminCompanyMaster_Add.aspx.cs" Inherits="System_Admin_S3GSysAdminCompanyMaster_Add" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../Scripts/Common.js"></script>

    <script language="javascript" type="text/javascript">

        function funCheckFirstLetterisNumeric(textbox, msg) {

            var FieldValue = new Array();
            FieldValue = textbox.value.trim();
            if (FieldValue.length > 0 && FieldValue.value != '') {
                if (isNaN(FieldValue[0])) {
                    return true;
                }
                else {
                    alert(msg + ' name cannot begin with a number');
                    textbox.focus();
                    textbox.value = '';
                    event.returnValue = false;
                    return false;
                }
            }
        }

        function fnCheckPageValidation() {

            if ((!fnCheckPageValidators(null, 'false'))) {
                if (Page_ClientValidate() == false) {
                    Page_BlockSubmit = false;
                    return false;
                }
            }
            if (Page_ClientValidate()) {
                //Starting
                if (document.getElementById('<%=txtPinCode.ClientID %>').value.length > document.getElementById('<%=txtPinCode.ClientID %>').maxLength) {
                    alert('PIN Code/ZIP Code in Basic Details length should not exceed ' + document.getElementById('<%=txtOtherPin.ClientID %>').maxLength + ' chars');
                    return false;
                }
                if (document.getElementById('<%=txtOtherPin.ClientID %>').value.length > document.getElementById('<%=txtOtherPin.ClientID %>').maxLength) {
                    alert('PIN Code/ZIP Code in Other Details length should not exceed ' + document.getElementById('<%=txtOtherPin.ClientID %>').maxLength + ' chars');
                    return false;
                }

                if (confirm('Do you want to save?')) {
                    return true;
                }
                else
                    return false;
            }
            return true;
        }

        function CheckDateGreaterThanSysDate(strCntrlId, strCheck) {
            var dd = new Date();
            var dd1 = new Date();
            var strDate = document.getElementById(strCntrlId).value;
            var date = strDate.split("/");

            dd.setDate(date[0]);
            dd.setMonth(date[1] - 1);
            dd.setFullYear(date[2]);

            if (strCheck == 0) {
                if (dd1 > dd) {
                    alert('Date should be greater than system date');
                    document.getElementById(strCntrlId).focus();
                    return false;
                }
                else {
                    return true;
                }
            }
            if (strCheck == 1) {
                if (dd > dd1) {

                    alert('Date cannot be greater than or equal to system date');
                    document.getElementById(strCntrlId).focus();
                    return false;
                }
                else {
                    return true;
                }
            }

        }

        function CompareTwodate(strCntrlId1, strCntrlId2) {
            var dd = new Date();
            var dd1 = new Date();
            var strDateSmall = document.getElementById(strCntrlId1).value;
            var strDateLarge = document.getElementById(strCntrlId2).value;
            var date1 = strDateSmall.split("/");
            var date2 = strDateLarge.split("/");
            dd.setFullYear(strDateSmall[2], strDateSmall[1] - 1, strDateSmall[0]);
            dd1.setFullYear(strDateLarge[2], strDateLarge[1] - 1, strDateLarge[0]);
            if (dd > dd1) {
                alert('Date cannot be greater than date of Incroparation date');
                return false;
            }

        }



    </script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px">
                        <cc1:TabContainer ID="tcCompanyCreation" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            Width="99%" ScrollBars="Auto">
                            <cc1:TabPanel runat="server" HeaderText="Basic Details" ID="tbBasicDetails" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Basic Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCompanyCode" runat="server" Text="Company Code" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtCompanyCode" runat="server" Width="180px" MaxLength="3" onkeypress="fnCheckSpecialChars();"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvCompanyCode" runat="server" Display="None" ErrorMessage="Enter the Company Code"
                                                    ControlToValidate="txtCompanyCode" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:CheckBox
                                                        ID="chkboxActive" runat="server" Text="Active" Checked="true" Enabled="false" />
                                                <cc1:FilteredTextBoxExtender FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCompanyCode" runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCompanyName" runat="server" Text="Company Name" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtCompanyName" runat="server" Width="400px" MaxLength="80"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftexCompanyName" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCompanyName" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" Display="None" ErrorMessage="Enter the Company Name"
                                                    ControlToValidate="txtCompanyName" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblAddress" runat="server" Text="Address 1" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtAddress" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" Display="None" ErrorMessage="Enter the Address 1"
                                                    ControlToValidate="txtAddress" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblAddress2" runat="server" Text="Address 2"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="1">
                                                <asp:TextBox ID="txtAddress2" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCity" runat="server" Text="City" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <%--<asp:TextBox ID="txtCity" runat="server" Width="400px" MaxLength="30"></asp:TextBox>--%>
                                                <cc1:ComboBox ID="txtCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                    Width="377px" MaxLength="30">
                                                </cc1:ComboBox>
                                                <%--<cc1:FilteredTextBoxExtender ID="ftexCity" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCity" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                <asp:RequiredFieldValidator ID="rfvCity" runat="server" Display="None" ErrorMessage="Enter the City"
                                                    ControlToValidate="txtCity" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblState" runat="server" Text="State" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <%--<asp:TextBox ID="txtState" runat="server" Width="400px" MaxLength="60"></asp:TextBox>--%>
                                                <cc1:ComboBox ID="txtState" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                    Width="377px" MaxLength="60">
                                                </cc1:ComboBox>
                                                <%-- <cc1:FilteredTextBoxExtender ID="ftexState" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtState" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" Display="None" ErrorMessage="Enter the State"
                                                    ControlToValidate="txtState" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <%-- <asp:TextBox ID="txtCountry" runat="server" Width="400px" MaxLength="60" AutoPostBack="true"
                                                    OnTextChanged="txtCountry_TextChanged"></asp:TextBox>--%>
                                                <cc1:ComboBox ID="txtCountry" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoPostBack="true" AutoCompleteMode="SuggestAppend"
                                                    Width="377px" OnTextChanged="txtCountry_TextChanged" MaxLength="60">
                                                </cc1:ComboBox>
                                                <%--<cc1:FilteredTextBoxExtender ID="ftexCountry" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCountry" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                <asp:RequiredFieldValidator ID="rfvCountry" runat="server" Display="None" ErrorMessage="Enter the Country"
                                                    ControlToValidate="txtCountry" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblPIN" runat="server" Text="PIN Code/ZIP Code"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtPinCode" runat="server" Width="180px" MaxLength="10" onkeypress="fnCheckSpecialChars();"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPIN" runat="server" Display="None" ErrorMessage="Enter the PIN Code/ZIP Code"
                                                    ControlToValidate="txtPinCode" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revPIN" runat="server" ControlToValidate="txtPinCode"
                                                    ValidationExpression="\d{5}" ErrorMessage="PIN Code/ZIP Code Should be 5 Digits"
                                                    Display="None">
                                                </asp:RegularExpressionValidator>
                                                <cc1:FilteredTextBoxExtender ID="ftePIN" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtPinCode" runat="server" Enabled="True" ValidChars=" ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblConstitutionalStatus" runat="server" Text="Constitutional Status"
                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlConstitutionalStatus" runat="server">
                                                    <asp:ListItem Text="Company" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="PartnerShip" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Corporate Details" ID="tbCorporateDetails"
                                CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Corporate Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblHeadName" runat="server" Text="CEO/Head Name" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtHeadName" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvHeadName" runat="server" Display="None" ErrorMessage="Enter the CEO/Head Name"
                                                    ControlToValidate="txtHeadName" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="fteHeadName" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                                    TargetControlID="txtHeadName" runat="server" ValidChars=" ,.">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtMobileNumber" runat="server" Width="180px" MaxLength="12"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftextxtMobileNumber" FilterType="Numbers"
                                                    TargetControlID="txtMobileNumber" runat="server">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblTeleNumber" runat="server" Text="Telephone Number" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtTeleNumber" runat="server" Width="180px" MaxLength="12"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftexTeleNumber" FilterType="Custom,Numbers"
                                                    TargetControlID="txtTeleNumber" runat="server" ValidChars="-">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvTeleNumber" runat="server" Display="None" ErrorMessage="Enter the Telephone Number"
                                                    ControlToValidate="txtTeleNumber" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblEmaidId" runat="server" Text="EMail Id" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="1">
                                                <asp:TextBox ID="txtEmailId" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEmailId" runat="server" Display="None" ErrorMessage="Enter the Email Id"
                                                    ControlToValidate="txtEmailId" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtEmailId"
                                                    Display="None" ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblWebsite" runat="server" Text="Website" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtWebsite" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvWebsite" runat="server" Display="None" ErrorMessage="Enter the Website Name"
                                                    ControlToValidate="txtWebsite" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                        ID="revWebsite" runat="server" ControlToValidate="txtWebsite" Display="None"
                                                        ErrorMessage="Enter a valid Website Name" ValidationExpression="www\.([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblSystemAdminUser" runat="server" Text="System Admin User Code" CssClass="styleReqFieldLabel"></asp:Label><br />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtSystemAdminUser" runat="server" Width="180px" MaxLength="6" onkeypress="fnCheckSpecialChars();"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftexSystemAdminUser" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                                    TargetControlID="txtSystemAdminUser" runat="server">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvSystemAdminUser" runat="server" Display="None"
                                                    ErrorMessage="Enter the System Admin User Code" ControlToValidate="txtSystemAdminUser"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revtxtSystemAdminUser" runat="server" Display="None"
                                                    ErrorMessage="System Admin User Code must begin with an alphabet and length should be minimum of 4 Characters"
                                                    ControlToValidate="txtSystemAdminUser" ValidationExpression="^[A-Za-z](\w){3,5}"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <span class="styleMandatory">(Must begin with an alphabet and length should be minimum
                                                    of 4 Characters and maximum of 6 Characters)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblSystemAdminPwd" runat="server" Text="System Admin User Password"
                                                    CssClass="styleReqFieldLabel"></asp:Label><br />
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtSystemAdminPwd" runat="server" Width="180px" MaxLength="6" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtSystemAdminPwd" runat="server" Display="None"
                                                    ErrorMessage="Enter the System Admin User Password" ControlToValidate="txtSystemAdminPwd"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="ftexSystemAdminPass"
                                                    InvalidChars=" " FilterMode="InvalidChars" TargetControlID="txtSystemAdminPwd" runat="server">
                                                </cc1:FilteredTextBoxExtender>
                                                <%--<asp:RegularExpressionValidator ID="revSystemAdminPwd" runat="server" Display="None"
                                            ErrorMessage="System Admin User Password Must Begin with a alphabet and length should be of 6 charcters"
                                            ControlToValidate="txtSystemAdminPwd" ValidationExpression="^[A-Za-z]\w{5,}"></asp:RegularExpressionValidator>--%>
                                                <%--   <asp:RegularExpressionValidator ID="revSystemAdminPassword" runat="server" Display="None"
                                            ErrorMessage="System Admin User Password Must Contain alteast a Upper Case,a Lower Case and a Number"
                                            ControlToValidate="txtSystemAdminPwd" ValidationExpression="(?=^(?=.{6,6}$)(?=.[A[/\-.]Z])(?=.*[a[/\-.]z])(?=.*[0[/\-.]9])(?=^[a[/\-.]Za[/\-.]Z]{1})(?=.*[!@#$%^*_:])(?!.*[\&quot;\s&amp;()+}{;='~:\\|'?/>.<,])).*$"></asp:RegularExpressionValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <span class="styleMandatory">(Password must be of 6 Characters, it should contain atleast
                                                    3 of the following one lower case, one upper case, one number,one special character.
                                                    <%--Should contain atleast one upper case,one lower case and a number or a special character should be of 6 characters--%>
                                                    )</span>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Other Details" ID="tbOtherDetails" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Other Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCommunicationAdd" runat="server" Text="Communication Address" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtCommunicationAdd" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvCommunicationAdd" runat="server" Display="None"
                                                    ErrorMessage="Enter the Communication Address" ControlToValidate="txtCommunicationAdd"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblAddress1" runat="server" Text="Address 1"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtAddress1" runat="server" Width="400px" MaxLength="60"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblOtherCity" runat="server" Text="City" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <%--<asp:TextBox ID="txtOtherCity" runat="server" Width="400px" MaxLength="30"></asp:TextBox>--%>
                                                <cc1:ComboBox ID="txtOtherCity" runat="server" CssClass="WindowsStyle" DropDownStyle="Simple"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                    Width="377px" MaxLength="30">
                                                </cc1:ComboBox>
                                                <%-- <cc1:FilteredTextBoxExtender ID="ftexOtherCity" FilterType="Custom, UppercaseLetters, LowercaseLetters,Numbers"
                                                    TargetControlID="txtOtherCity" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                <asp:RequiredFieldValidator ID="rfvOtherCity" runat="server" Display="None" ErrorMessage="Enter the City in Other Details tab"
                                                    ControlToValidate="txtOtherCity" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblOtherState" runat="server" Text="State" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="1">
                                                <%--<asp:TextBox ID="txtOtherState" runat="server" Width="400px" MaxLength="60"></asp:TextBox>--%>
                                                <cc1:ComboBox ID="txtOtherState" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                    Width="377px" MaxLength="60">
                                                </cc1:ComboBox>
                                                <%-- <cc1:FilteredTextBoxExtender ID="ftexOtherState" FilterType="Custom, UppercaseLetters, LowercaseLetters,Numbers"
                                                    TargetControlID="txtOtherState" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                <asp:RequiredFieldValidator ID="rfvOtherState" runat="server" Display="None" ErrorMessage="Enter the State in Other Details tab"
                                                    ControlToValidate="txtOtherState" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblOtherCountry" runat="server" Text="Country" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <%--<asp:TextBox ID="txtOtherCountry" runat="server" Width="400px" MaxLength="60"></asp:TextBox>--%>
                                                <cc1:ComboBox ID="txtOtherCountry" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                    Width="377px" MaxLength="60">
                                                </cc1:ComboBox>
                                                <%--<cc1:FilteredTextBoxExtender ID="ftexOtherCountry" FilterType="Custom, UppercaseLetters, LowercaseLetters,Numbers"
                                                    TargetControlID="txtOtherCountry" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                                <asp:RequiredFieldValidator ID="rfvOtherCountry" runat="server" Display="None" ErrorMessage="Enter the Country in Other Details tab"
                                                    ControlToValidate="txtOtherCountry" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblOtherPin" runat="server" Text="PIN Code/ZIP Code"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtOtherPin" runat="server" Width="180px" MaxLength="10" onkeypress="fnCheckSpecialChars();"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvOtherPin" runat="server" Display="None" ErrorMessage="Enter the PIN Code/ZIP Code in Other Details tab"
                                                    ControlToValidate="txtOtherPin" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revOtherPIN" runat="server" ControlToValidate="txtOtherPin"
                                                    ValidationExpression="\d{5}" Display="None"
                                                    ErrorMessage="PIN Code/ZIP Code Should be 5 Digits in Other details tab">
                                                </asp:RegularExpressionValidator>
                                                <cc1:FilteredTextBoxExtender ID="fteOtherPIN" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtOtherPin" runat="server" Enabled="True" ValidChars=" ">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblDateOfIncorp" runat="server" Text="Date of Incorporation/Company Start date"
                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtDateOfIncorp" runat="server" Width="100px" MaxLength="12"></asp:TextBox>
                                                <asp:Image ID="imgCalIncorpDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <asp:RequiredFieldValidator ID="rfvDateOfIncorp" runat="server" Display="None"
                                                    ErrorMessage="Enter the date of Incorporation"
                                                    ControlToValidate="txtDateOfIncorp" SetFocusOnError="True">
                                                </asp:RequiredFieldValidator>
                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy"
                                                    OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                    TargetControlID="txtDateOfIncorp" PopupButtonID="imgCalIncorpDate"
                                                    ID="CalendarExtender1" Enabled="True">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblRegNumber" runat="server" Text="CIN"
                                                    CssClass="styleReqFieldLabel"></asp:Label><br />
                                                <span class="styleMandatory">(Must begin with an alphabet or a number)</span>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtRegNumber" runat="server" Width="180px" MaxLength="21"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvRegNumber" runat="server" Display="None" ErrorMessage="Enter the CIN Number"
                                                    ControlToValidate="txtRegNumber" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revtxtRegNumber" runat="server" Display="None"
                                                    ErrorMessage="Enter a valid Registration Number/License Number" ControlToValidate="txtRegNumber"
                                                    ValidationExpression="^[a-zA-Z_0-9](\w|\W)*"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr style="display:none;">
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCST" runat="server" Text="CST No" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtCSTNo" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None" Enabled="false" ErrorMessage="Enter the CST No"
                                                    ControlToValidate="txtCSTNo" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblSTNo" runat="server" Text="ST No" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtSTNo" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None" ErrorMessage="Enter the ST No"
                                                    ControlToValidate="txtSTNo" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblValdityOfRegNumber" runat="server" Text="Validity of Registration Number/License Number"
                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtValdityOfRegNumber" runat="server" Width="100px" MaxLength="12"></asp:TextBox>
                                                <asp:Image ID="imgCalRegNoValDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <asp:RequiredFieldValidator ID="rfvValdityOfRegNumber" runat="server" Display="None"
                                                    ErrorMessage="Enter Validity Of Registration/License Number" ControlToValidate="txtValdityOfRegNumber"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtValdityOfRegNumber"
                                                    PopupButtonID="imgCalRegNoValDate" OnClientDateSelectionChanged="checkDate_PrevSystemDate"
                                                    ID="cexValdityOfRegNumber" Enabled="True">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblPanNumber" runat="server" Text="Income Tax Number/PAN" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtPanNumber" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPanNumber" runat="server" Display="None" ErrorMessage="Enter the Income Tax Number/PAN"
                                                    ControlToValidate="txtPanNumber" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revtxtPanNumber" runat="server" Display="None"
                                                    ErrorMessage="Enter a valid Income tax number" ControlToValidate="txtPanNumber"
                                                    ValidationExpression="^[a-zA-Z_0-9](\w|\W)*"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCGSTIN" runat="server" Text="GSTIN" ></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtCGSTin" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                                                   <cc1:FilteredTextBoxExtender ID="filsgstin" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="txtCGSTin" runat="server" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                               <%-- <asp:RequiredFieldValidator ID="rfvcgstin" runat="server" Display="None" ErrorMessage="Enter the CGSTIN"
                                                    ControlToValidate="txtCGSTin" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCGSTREGDate" runat="server" Text="GST Registration Date"
                                                   ></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="TxtCGSTRegDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:Image ID="imgCGSTRegDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <%--<asp:RequiredFieldValidator ID="rfvcgstregdate" runat="server" Display="None"
                                                    ErrorMessage="Enter CGST Registration Date" ControlToValidate="TxtCGSTRegDate"
                                                    SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>--%>
                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="TxtCGSTRegDate"
                                                    PopupButtonID="imgCGSTRegDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                    ID="CalendarExtender2" Enabled="True">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblAccCurrency" runat="server" Text="Accounting Currency"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtAccCurrency" runat="server" Width="180px" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                       <%-- <tr>
                                            <td colspan="2" class="styleFieldLabel">
                                                <asp:Panel GroupingText="VAT-TIN State wise capture" ToolTip="VAT-TIN State wise capture"
                                                    ID="pnlVATTIN" runat="server" CssClass="stylePanel" Width="75%" Visible="false">
                                                   
                                                        <asp:GridView ID="gvVATTIN" runat="server" CssClass="styleInfoLabel" Width="100%"
                                                            AutoGenerateColumns="False" ShowFooter="true" OnRowDataBound="gvVATTIN_RowDataBound" OnRowCommand="gvVATTIN_RowCommand">
                                                            <Columns>
                                                               
                                                                <asp:TemplateField HeaderText="State" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblState" runat="server" Text='<%#Bind("STATE") %>'></asp:Label>
                                                                        <asp:Label ID="lblStateID" runat="server" Text='<%#Bind("STATE_ID") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlState" Width="90%" runat="server"></asp:DropDownList>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VAT-TIN" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVATTIN" Text='<%#Bind("VATTIN") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox Width="99%" ID="txtVATTIN" runat="server" MaxLength="9"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnAdd" CssClass="styleSubmitShortButton" CommandName="Add" runat="server"
                                                                            OnClientClick="return fnCheckPageValidators('Add',false);" ToolTip="Add" Text="Add"
                                                                            ValidationGroup="Add" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                   
                                                </asp:Panel>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="styleReqFieldLabel"></asp:Label><br />
                                                <span class="styleMandatory">(Maximum length cannot be greater than 60 Characters)</span>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="400px" MaxLength="60" TextMode="MultiLine"
                                                    onkeydown="maxlengthfortxt(60)" onblur="maxlengthfortxt(60)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvRemarks" runat="server" Display="None" ErrorMessage="Enter the Remarks"
                                                    ControlToValidate="txtRemarks" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revRemarks" runat="server" Display="None" ErrorMessage="Maximum length cannot be greater than 60 Characters"
                                                    ControlToValidate="txtRemarks" ValidationExpression="[\s\S]{1,60}"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnPrev" runat="server" Text="<<" CssClass="styleButtonNext" CausesValidation="false"
                            OnClick="btnPrev_Click" Visible="false" />
                        <asp:Button ID="btnNext" runat="server" Text=">>" CssClass="styleButtonNext" CausesValidation="false"
                            OnClick="btnNext_Click" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidation();" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="styleSubmitButton"
                            OnClick="btnDelete_Click" Visible="false" />
                        <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="reset" value="Clear" id="btnClear" runat="server" class="styleSubmitButton"
                    onclick="return confirm('Do you want to clear?')" />--%>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            OnClick="btnClear_Click" CausesValidation="False" OnClientClick="return fnConfirmClear();" />
                        <%--<cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to Clear"
                    Enabled="True" TargetControlID="btnClear">
                </cc1:ConfirmButtonExtender>--%>
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" CausesValidation="False" Visible="False" />
                        <input id="hdnIds" type="hidden" runat="server" />
                        <input id="hdnUserId" type="hidden" runat="server" value="0" />
                        <input id="hdnUserLevelId" type="hidden" runat="server" value="0" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium"></span>
                        <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" 
                    ForeColor="Red"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- <cc1:PopupControlExtender runat="server" TargetControlID="btnSave" PopupControlID="vsCompanyAdd"
                    Position="Top" OffsetX="-100" OffsetY="-350">
                </cc1:PopupControlExtender> Style="border: solid 1px red;
                    background-color: #ffffcc" --%>
                        <asp:ValidationSummary ID="vsCompanyAdd" ValidationGroup="Add" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
