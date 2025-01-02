<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GSysAdminUTPAMaster_Add.aspx.cs"
    Inherits="System_Admin_S3GSysAdminUTPAMaster_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput) {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function fnClearPassword() {
            var txtPassword = document.getElementById('<%=txtPassword.ClientID %>');
            txtPassword.disabled = false;
            txtPassword.value = "";
        }
        
        function fnEnablePwd()
        {
        if(document.getElementById('<%=chkResetPWD.ClientID%>').checked)
            {
            document.getElementById('<%=txtPassword.ClientID%>').disabled=false;
            document.getElementById('<%=txtPassword.ClientID%>').value='';
            document.getElementById('<%=txtPassword.ClientID%>').focus();
            }
         else
            {
                document.getElementById('<%=txtPassword.ClientID%>').disabled=true;
                document.getElementById('<%=txtPassword.ClientID%>').value='*************';
                
            }   
        }


        function fnSelect(strChkAdd, strChkModify, strChkQuery, strChkDelete, strSelectAll) {
            var FieldChkAdd = document.getElementById(strChkAdd).checked;
            var FieldChkModify = document.getElementById(strChkModify).checked;
            var FieldChkQuery = document.getElementById(strChkQuery).checked;
            var FieldChkDelete = document.getElementById(strChkDelete).checked;

            var FieldSelectAll = document.getElementById(strSelectAll);

            var blnStatus = FieldChkAdd && FieldChkModify && FieldChkQuery && FieldChkDelete;
            FieldSelectAll.checked = blnStatus;
        }


        function fnSelectAll(strChkAdd, strChkModify, strChkQuery, strChkDelete, strSelectAll) {
            //var FieldSelectAll = document.getElementById(strSelectAll).checked;

            var FieldChkAdd = document.getElementById(strChkAdd);
            var FieldChkModify = document.getElementById(strChkModify);
            var FieldChkQuery = document.getElementById(strChkQuery);
            var FieldChkDelete = document.getElementById(strChkDelete);


            var blnStatus = document.getElementById(strSelectAll).checked;

            //FieldChkAdd  && FieldChkModify && FieldChkQuery && FieldChkDelete;

            FieldChkAdd.checked = blnStatus;
            FieldChkModify.checked = blnStatus;
            FieldChkQuery.checked = blnStatus;
            FieldChkDelete.checked = blnStatus;
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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" align="left">
                                    <cc1:TabContainer ID="TabContainer1" runat="server" Width="100%" Height="350" CssClass="styleTabPanel"
                                        AutoPostBack="False" ActiveTabIndex="0">
                                        <cc1:TabPanel ID="TabPanel1" runat="server" CssClass="tabpan" BackColor="Red" Width="100%"
                                            HeaderText="UTPA Details">
                                            <HeaderTemplate>
                                                UTPA Details
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvRegionCode" runat="server" Display="None" ControlToValidate="ddlRegionCode"
                                                                                    Enabled="false" ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a Region"
                                                                                    CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" ControlToValidate="ddlBranchCode"
                                                                                    ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Location" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvUTPAType" runat="server" Display="None" ControlToValidate="ddlUTPAType"
                                                                                    ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a UTPA Type"
                                                                                    CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvUTPACode" runat="server" Display="None" ControlToValidate="ddlUTPACode"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Select a UTPA Code" InitialValue="0"
                                                                                    CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                                                                    ControlToValidate="ddlUTPACode" ValidationGroup="btnSave" ErrorMessage="Select a UTPA Code"
                                                                                    CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <br />
                                                                                <asp:RegularExpressionValidator ID="revUTPACode" runat="server" Display="None" ControlToValidate="txtUTPACode"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter a valid UTPA Code" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True" ValidationExpression="^[A-Z](\w){3,6}"></asp:RegularExpressionValidator>
                                                                            </td>
                                                                            <asp:RequiredFieldValidator ID="rfvLoginCode" runat="server" Display="None" ControlToValidate="txtLoginCode"
                                                                                ValidationGroup="btnSave" ErrorMessage="Enter UTPA Login Code" CssClass="styleMandatoryLabel"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvUTPAName" runat="server" Display="None" ControlToValidate="txtUTPAName"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter a UTPA Name" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" Display="None" ControlToValidate="txtPassword"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter the Password" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvAddress1" runat="server" Display="None" ControlToValidate="txtAddress1"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter the Address1" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvCity" runat="server" Display="None" ControlToValidate="txtCity"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter the City" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvState" runat="server" Display="None" ControlToValidate="txtState"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter the State" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvCountry" runat="server" Display="None" ControlToValidate="txtCountry"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter the Country" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvPincode" runat="server" Display="None" ControlToValidate="txtPincode"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter the PIN Code" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvGLCode" runat="server" Display="None" ControlToValidate="txtGLCODE"
                                                                                    ValidationGroup="btnSave" ErrorMessage="Enter the GL Account" CssClass="styleMandatoryLabel"
                                                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtUTPACode"
                                                                        FilterType="Numbers, UppercaseLetters" ValidChars=" " Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtUTPAName"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                        Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPassword"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" ~!@#$%^&*()_+|}{][;':,./<>?"
                                                                        Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtCity"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                        Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtState"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                        Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtCountry"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                        Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="ftePIN" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                        TargetControlID="txtPincode" runat="server" Enabled="True" ValidChars=" ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td style="width: 22%;" class="styleFieldLabel">
                                                                                <asp:Label ID="lblRegion" runat="server" Text="Region" Visible="false" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 25%;">
                                                                                <asp:DropDownList ID="ddlRegionCode" Visible="false" runat="server" OnSelectedIndexChanged="RegionCode_SelectedIndexChanged"
                                                                                    AutoPostBack="True">
                                                                                </asp:DropDownList>
                                                                                <asp:DropDownList ID="ddlBranchCode" OnSelectedIndexChanged="ddlBranchCode_SelectedIndexChanged"
                                                                                    AutoPostBack="true" runat="server">
                                                                                </asp:DropDownList>
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
                                                                                <asp:Label ID="lblUTPAType" runat="server" Text="UTPA Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlUTPAType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlUTPAType_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td colspan="2">
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblUTPACode" runat="server" Text="UTPA Code" CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUTPACode" runat="server" MaxLength="6" Visible="False"></asp:TextBox>
                                                                                <asp:DropDownList ID="ddlUTPACode" runat="server" Width="86%" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlUTPACode_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblUTPAName" runat="server" Text="UTPA Name"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUTPAName" runat="server" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblLoginCode" runat="server" Text="UTPA Login Code" CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtLoginCode" runat="server" MaxLength="6" CssClass="styleTextUpperCase"
                                                                                    onchange="fnClearPassword();"></asp:TextBox>
                                                                                <asp:Panel ID="PopupTool_LoginCode" runat="server" BorderWidth="1" CssClass="styleRecordCount"
                                                                                    Width="30%">
                                                                                    <asp:Label ID="lblLoginCodeTooltip" runat="server" Text=" Must begin with an alphabet and length should be of 6 characters " />
                                                                                </asp:Panel>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtLoginCode"
                                                                                    FilterType="Numbers, UppercaseLetters, LowercaseLetters" Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <cc1:HoverMenuExtender ID="HoverMenuExtender1" TargetControlID="txtLoginCode" runat="server"
                                                                                    PopupControlID="PopupTool_LoginCode" PopupPosition="Right" PopDelay="50" />
                                                                                <asp:RegularExpressionValidator ID="revLoginCode" runat="server" Display="None" ErrorMessage="UTPA Login Code must begin with an alphabet and length should be of 6 characters"
                                                                                    ControlToValidate="txtLoginCode" ValidationGroup="btnSave" ValidationExpression="^[A-Za-z](\w){3,5}"></asp:RegularExpressionValidator>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblPassword" runat="server" CssClass="styleReqFieldLabel" Text="Password"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"
                                                                                    Width="130px"></asp:TextBox>
                                                                                <asp:CheckBox ID="chkResetPWD" runat="server" AutoPostBack="false" 
                                                                                    ToolTip="Reset Password" onClick="fnEnablePwd();" />
                                                                                <asp:Panel ID="PopupMenu" runat="server" BorderWidth="1" CssClass="styleRecordCount"
                                                                                    Height="55px" Width="185px">
                                                                                    <asp:Label ID="lblPWDString" runat="server" Text="  Should contains atleast one upper case,one numeral and
                                                                                    a number or a special character and it should be of 6 characters  " />
                                                                                    <br />
                                                                                </asp:Panel>
                                                                                <cc1:HoverMenuExtender TargetControlID="txtPassword" runat="server" PopupControlID="PopupMenu"
                                                                                    PopupPosition="Bottom" PopDelay="50" />
                                                                            </td>
                                                                        </tr>
                                                            <%--<tr>
                                                                            <td colspan="5" class="styleFieldLabel">
                                                                                <span class="styleMandatory">(Should contain atleast one upper case,one lower case and
                                                                                    a number or a special character should be of 6 characters)</span>
                                                                            </td>
                                                                        </tr>--%>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblAddress1" runat="server" Text="Address1"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress1" runat="server" MaxLength="60" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblAddress2" runat="server" Text="Address2"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtAddress2" runat="server" MaxLength="60" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCity" runat="server" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblState" runat="server" Text="State"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtState" runat="server" MaxLength="60" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCountry" runat="server" MaxLength="60" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblPincode" runat="server" Text="PIN Code"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPincode" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblGLCode" runat="server" Text="GL Account"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlGLCode" runat="server" Visible="False">
                                                                    </asp:DropDownList>
                                                                    <asp:TextBox ID="txtGLCODE" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkActive" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <caption>
                                                                <br />
                                                                <tr>
                                                                    <td align="center" colspan="5">
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="Save_Click"
                                                                            OnClientClick="return fnCheckPageValidators('btnSave');" Text="Save" ValidationGroup="btnSave" />
                                                                        <asp:Button ID="BtnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                            OnClick="BtnClear_Click" OnClientClick="return fnConfirmClear();" Text="Clear" />
                                                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                            OnClick="btnCancel_Click" Text="Cancel" />
                                                                    </td>
                                                                </tr>
                                                            </caption>
                                                        </table>
                                                        </td> </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="TabPanel2" runat="server" CssClass="tabpan" BackColor="Red" Width="100%"
                                            HeaderText="Program Access">
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td valign="top" colspan="2">
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="btnProgramAccess"
                                                                                    InitialValue="0" ErrorMessage="Select the Line of Business" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                <asp:RequiredFieldValidator ID="rfvRoleCode" runat="server" ControlToValidate="ddlRoleCode"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="btnProgramAccess"
                                                                                    InitialValue="0" ErrorMessage="Select the Role Description" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" colspan="2">
                                                                    <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td style="width: 40%;" class="styleFieldLabel" colspan="2">
                                                                                <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business "></asp:Label>
                                                                            </td>
                                                                            <td style="width: 60%;" colspan="3">
                                                                                <asp:DropDownList ID="ddlLOB" runat="server">
                                                                                    <asp:ListItem Text="--select--" Value="0"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 40%;" class="styleFieldLabel" colspan="2">
                                                                                <asp:Label ID="lblRoleCode" runat="server" CssClass="styleReqFieldLabel" Text="Role Description"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 60%;" colspan="3">
                                                                                <asp:DropDownList ID="ddlRoleCode" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                Access Rights
                                                                            </td>
                                                                            <td colspan="4" align="left">
                                                                                <div style="vertical-align: text-top">
                                                                                    <asp:CheckBox ID="ChkAdd" runat="server" Text="Add" />
                                                                                    <asp:CheckBox ID="ChkModify" runat="server" Text="Modify" />
                                                                                    <asp:CheckBox ID="ChkQuery" runat="server" Text="Query" />
                                                                                    <asp:CheckBox ID="ChkDelete" runat="server" Text="Delete" />
                                                                                    <asp:CheckBox ID="ChkSelectAll" runat="server" Text="Select All" />
                                                                                </div>
                                                                            </td>
                                                                            <tr>
                                                                                <td align="center" colspan="5">
                                                                                    <asp:Label ID="Label15" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" colspan="5">
                                                                                    <asp:Button ID="btnProgramAccess" runat="server" CssClass="styleSubmitButton" OnClick="ProgramAccessSave_Click"
                                                                                        OnClientClick="return fnCheckPageValidators('btnProgramAccess');" Text="Set Rights"
                                                                                        ValidationGroup="btnProgramAccess" />
                                                                                    <asp:Button ID="btnClearProgramAccess" runat="server" CssClass="styleSubmitButton"
                                                                                        OnClick="ProgramAccessClear_Click" Text="Clear" />
                                                                                </td>
                                                                            </tr>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" colspan="2">
                                                                    <div style="height: 220px; overflow-x: hidden; overflow-y: auto;">
                                                                        <asp:GridView ID="gvProgramAccess" runat="server" AutoGenerateColumns="False" DataKeyNames="UTPA_Prog_Access_ID"
                                                                            OnRowEditing="gvProgramAccess_RowEditing" OnRowDeleting="gvProgramAccess_RowDeleting"
                                                                            OnRowDataBound="gvProgramAccess_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Line of Business">
                                                                                    <ItemTemplate>
                                                                                        <%-- <asp:Label ID="lblLOBName" runat="server" Text='<%# GetLineofBusinessName( DataBinder.Eval(Container.DataItem,"LOB_ID").ToString()) %>'></asp:Label>--%>
                                                                                        <asp:Label ID="lblLOBName" runat="server" Text='<%#Eval("LOBNAME")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="25%" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="Role_CODE" HeaderText="Role Code">
                                                                                    <HeaderStyle Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Program_CODe" HeaderText="Program Code">
                                                                                    <HeaderStyle Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Program_Name" HeaderText="Program Name">
                                                                                    <HeaderStyle Width="20%" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Add">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="ChkAdd_GV" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem,"Add_Access").ToString().ToUpper() == "TRUE" %>'
                                                                                            Enabled="false" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Modify">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="ChkModify_GV" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem,"Modify_Access").ToString().ToUpper() == "TRUE"  %>'
                                                                                            Enabled="false" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Query">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="ChkQuery_GV" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem,"Query").ToString().ToUpper() == "TRUE"  %>'
                                                                                            Enabled="false" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="ChkDelete_GV" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem,"Delete_Access").ToString().ToUpper() == "TRUE" %>'
                                                                                            Enabled="false" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                                                            OnClientClick="return confirm('Do you want to delete the selected detail?');"
                                                                                            CausesValidation="false"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="5%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Width="5%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblErrorMessage_Program" runat="server" Font-Size="15px" CssClass="styleDisplayLabel"></asp:Label>
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
                            <tr>
                                <td align="center">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                                    <asp:ValidationSummary ID="vsProgramAccess" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnProgramAccess" />
                                    <input type="hidden" id="hdnID" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
