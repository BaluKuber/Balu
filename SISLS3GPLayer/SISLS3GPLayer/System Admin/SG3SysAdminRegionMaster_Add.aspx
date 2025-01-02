<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="SG3SysAdminRegionMaster_Add.aspx.cs" Inherits="System_Admin_SG3SysAdminRegionMaster_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput)
        {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function RemoveStartingNumeric(strInput)
        {       
            var FieldValue = document.getElementById(strInput).value;           
            document.getElementById(strInput).value = FieldValue.trim();
            FieldValue = document.getElementById(strInput).value;           
            if( FieldValue.charAt(0)>= 0 || FieldValue.charAt(0) <= 9)
            {
                alert("Numbers are not allowed in the begining");
                return false;
            }           
        }
     
        function funCheckFirstLetterisNumeric(textbox)
        {
        
            var FieldValue=new Array();
             FieldValue = textbox.value;  
             if(FieldValue.length>0)
             {         
            if(isNaN(FieldValue[0]))
            {
                return true;
            }
            else
            {
                 alert('Value cannot begin with a number');
                textbox.focus();
                textbox.value='';
                event.returnValue = false;
                return false;
            } 
            }    
        }
    </script>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="left" class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-top: 10px">
                <cc1:TabContainer ID="tcRegBranch" runat="server" Width="100%" AutoPostBack="true"
                    CssClass="styleTabPanel" ActiveTabIndex="1">
                    <cc1:TabPanel runat="server" ID="TabPanel1" CssClass="tabpan" BackColor="Red" Width="100%"
                        HeaderText="Region Details">
                        <HeaderTemplate>
                            Region Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                        <tr>
                                            <td align="left" class="styleFieldLabel" width="25%">
                                                <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Region Code" ID="lblRegionC"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" style="width: 25%">
                                                <asp:TextBox ID="txtRegionCode" runat="server" MaxLength="1" Width="200px" onpaste="return false"
                                                    oncopy="return false" TabIndex="1" OnTextChanged="txtRegionCode_TextChanged"
                                                    onblur="ChkIsZero(this)"></asp:TextBox>
                                            </td>
                                            <td align="left" class="styleFieldAlign" style="width: 50%">
                                                <asp:RequiredFieldValidator ID="rfvRegionCode" runat="server" ValidationGroup="btnRegionSave"
                                                    Display="None" ErrorMessage="Enter the Region Code" ControlToValidate="txtRegionCode"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtRegionCode"
                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars="1,2,3,4,5,6,7,8,9"
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="styleFieldLabel" width="25%">
                                                <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Region Name" ID="lblRegionN"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtRegionName" runat="server" MaxLength="30" Width="200px" onpaste="return false"
                                                    oncopy="return false" TabIndex="2" OnTextChanged="txtRegionName_TextChanged"
                                                    onblur="funCheckFirstLetterisNumeric(this)"></asp:TextBox>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="50%">
                                                <asp:RequiredFieldValidator ID="rfvRegionName" runat="server" ValidationGroup="btnRegionSave"
                                                    Display="None" ErrorMessage="Enter the Region Name" ControlToValidate="txtRegionName"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRegionName"
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="styleFieldAlign" width="25%">
                                                <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%" style="padding-left: 7px;">
                                                <asp:CheckBox ID="CbxRegion" runat="server" TabIndex="3" OnCheckedChanged="CbxRegion_CheckedChanged" />
                                            </td>
                                            <td width="50%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:Button runat="server" ID="btnRegionSave" ValidationGroup="btnRegionSave" CssClass="styleSubmitButton"
                                                    Text="Save" OnClientClick="return fnCheckPageValidators('btnRegionSave');" OnClick="btnRegionSave_Click"
                                                    TabIndex="4" />
                                                <asp:Button runat="server" ID="btnRegionClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClick="btnRegionClear_Click" TabIndex="5" />
                                                <cc1:ConfirmButtonExtender ID="btnRegionClear_ConfirmButtonExtender" runat="server"
                                                    ConfirmText="Do you want to Clear?" Enabled="True" TargetControlID="btnRegionClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button runat="server" ID="btnRegionCancel" CssClass="styleSubmitButton" Text="Cancel"
                                                    OnClick="btnRegionCancel_Click" TabIndex="6" CausesValidation="false" />
                                            </td>
                                        </tr>
                                        <tr style="height: 300px" valign="top">
                                            <td colspan="3">
                                                <asp:ValidationSummary ID="vsRegionSummary" ValidationGroup="btnRegionSave" runat="server"
                                                    Height="44px" Width="716px" CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel2" CssClass="tabpan" Width="100%" HeaderText="Branch Details">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                        <tr runat="server" id="trBranchCode">
                                            <td align="left" class="styleFieldLabel" width="25%" runat="server">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Branch Code"
                                                    ID="lblBranchR"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%" runat="server">
                                                <asp:TextBox ID="txtBranchCode" runat="server" MaxLength="3" Width="200px" TabIndex="1"
                                                    OnTextChanged="txtBranchCode_TextChanged" onblur="ChkIsZero(this)"></asp:TextBox>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="50%" runat="server">
                                                <asp:RequiredFieldValidator ID="rfvBranchCode" runat="server" Display="None" ErrorMessage="Enter the Branch Code"
                                                    ControlToValidate="txtBranchCode" ValidationGroup="btnBranchSave"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="styleFieldLabel" width="25%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" CssClass="styleReqFieldLabel" ID="lblBranchC"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtBranchName" runat="server" Width="200px" MaxLength="60" onchange="funCheckFirstLetterisNumeric(this)"
                                                    TabIndex="2"></asp:TextBox>
                                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="styleFieldAlign" Width="205px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="50%">
                                                <asp:RequiredFieldValidator ID="rfvBranchName" ValidationGroup="btnBranchSave" Display="None"
                                                    runat="server" ErrorMessage="Enter the Branch Name" ControlToValidate="txtBranchName"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvBranch" ValidationGroup="btnBranchSave" InitialValue="0"
                                                    Display="None" runat="server" ErrorMessage="Select the Branch" ControlToValidate="ddlBranch"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="styleFieldLabel" width="25%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRegionCode" runat="server" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlRegionCode" runat="server" CssClass="styleFieldAlign" TabIndex="3">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlBranchCode" runat="server" Width="205px" CssClass="styleFieldAlign"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="50%">
                                                <asp:RequiredFieldValidator ID="rfvBranchRegionCode" runat="server" Display="None"
                                                    ErrorMessage="Select the Region" ValidationGroup="btnBranchSave" ControlToValidate="ddlRegionCode"
                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="styleFieldLabel" width="25%">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblStateName" runat="server" Text="State Name"
                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%">
                                                 <asp:TextBox ID="txtStateName1" runat="server" MaxLength="60" Width="200px" TabIndex="4"
                                                    onchange="funCheckFirstLetterisNumeric(this)" OnTextChanged="txtStateName_TextChanged" Visible="false"></asp:TextBox>
                                                <cc1:ComboBox ID="txtStateName"  runat="server" CssClass="WindowsStyle"  DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" ItemInsertLocation="Append" CaseSensitive="false"  
                                                    onchange="funCheckFirstLetterisNumeric(this)" AutoCompleteMode="SuggestAppend" Width="176px" TabIndex="4">
                                                </cc1:ComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblRegionName" runat="server" CssClass="styleFieldAlign"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlToRegion" runat="server" CssClass="styleFieldAlign" Width="205px"
                                                    AutoPostBack="True">
                                                </asp:DropDownList>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtStateName1"
                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters" ValidChars=" 0,1,2,3,4,5,6,7,8,9"
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="50%">
                                                <asp:RequiredFieldValidator ID="rfvStateName" runat="server" ValidationGroup="btnBranchSave" InitialValue="0"
                                                    Display="None" ErrorMessage="Enter the State Name" ControlToValidate="txtStateName"></asp:RequiredFieldValidator>
                                                <%-- <asp:RequiredFieldValidator ID="rfvToRegion" ValidationGroup="btnBranchSave" runat="server"
                                                    Display="None" ErrorMessage="Select To Region" ControlToValidate="ddlToRegion"
                                                    InitialValue="0"></asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr id="Tr7" runat="server">
                                            <td align="left" class="styleFieldLabel" width="25%" runat="server">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" Text="Active" ID="lblActiveBranch"
                                                    CssClass="styleDisplayLabel"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="25%" runat="server" style="padding-left: 7px;">
                                                <asp:CheckBox ID="CbxBranch" runat="server" BorderStyle="None" TabIndex="5" Enabled="False"
                                                    AutoPostBack="true" OnCheckedChanged="CbxBranch_CheckedChanged" />
                                            </td>
                                            <td align="left" class="styleFieldAlign" width="50%" runat="server">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Button runat="server" ID="btnBranchSave" CssClass="styleSubmitButton" ValidationGroup="btnBranchSave"
                                                    Text="Save" OnClientClick="return fnCheckPageValidators('btnBranchSave');" OnClick="btnBranchSave_Click"
                                                    TabIndex="6" />
                                                <asp:Button runat="server" ID="btnBranchClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClick="btnBranchClear_Click" TabIndex="7" />
                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Do you want to Clear?"
                                                    Enabled="True" TargetControlID="btnBranchClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button runat="server" ID="btnBranchCancel" CssClass="styleSubmitButton" Text="Cancel"
                                                    CausesValidation="False" OnClick="btnBranchCancel_Click" TabIndex="8" />
                                            </td>
                                        </tr>
                                        <tr style="height: 280px" valign="top">
                                            <td colspan="3">
                                                <asp:ValidationSummary ID="btnBranchSaveSummary" ValidationGroup="btnBranchSave"
                                                    runat="server" CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="Label1" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                                                    font-size: medium"></asp:Label>
                                            </td>
                                        </tr>
                                        </td> </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                <input type="hidden" id="hdnSortDirection" runat="server" />
                <input type="hidden" id="hdnSortExpression" runat="server" />
                <input type="hidden" id="hdnSearch" runat="server" />
                <input type="hidden" id="hdnOrderBy" runat="server" />
                <input type="hidden" id="hdnID" runat="server" />
            </td>
        </tr>
    </table>
    </ContentTemplate> </asp:UpdatePanel>
</asp:Content>
