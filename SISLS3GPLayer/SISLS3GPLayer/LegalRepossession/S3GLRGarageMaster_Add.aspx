<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/LegalRepossession/S3GLRGarageMaster_Add.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="LegalRepossession_S3GLRGarageMaster_Add"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading" style="width: 990px">
                <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleDisplayLabel">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <cc1:TabContainer ID="tcGarageMaster" runat="server" CssClass="styleTabPanel" Width="99%"
                    TabStripPlacement="top" ActiveTabIndex="1">
                    <cc1:TabPanel runat="server" HeaderText="Contact Details" ID="tpContact" CssClass="tabpan"
                        BackColor="Red">
                        <HeaderTemplate>
                            Contact Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upContact" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel GroupingText="Garage Owner Details" ID="pnlContact" runat="server" CssClass="stylePanel">
                                        <table cellspacing="0" width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="LblGarageCode" runat="server" CssClass="styleDisplayLabel" Text="Garage Code"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtGrgCode" runat="server" MaxLength="100" ToolTip="Garage Code"
                                                        ReadOnly="True"  Width="40%"></asp:TextBox>
                                                    <asp:CheckBox ID="ChkIsActive" runat="server" Text="Active" AutoPostBack="True" ToolTip="Active"
                                                        OnCheckedChanged="ChkIsActive_CheckedChanged" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    &nbsp;
                                                </td>
                                                <td class="styleFieldAlign">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGarageOwnerCode" runat="server" CssClass="styleDisplayLabel" Text="Garage Owner Code"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:DropDownList ID="ddlGarageOwnerCode" runat="server" AutoPostBack="True" ToolTip="Garage Owner Code"
                                                        OnSelectedIndexChanged="ddlGarageOwnerCode_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RqvddlGarageOwnerCode" runat="server" ControlToValidate="ddlGarageOwnerCode"
                                                        Display="None" ErrorMessage="Select a Garage Owner Code" InitialValue="0" SetFocusOnError="True"
                                                        ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    &nbsp;
                                                </td>
                                                <td class="styleFieldAlign">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgOwnerName" runat="server" CssClass="styleDisplayLabel" Text="Garage Owner Name"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtGrgOwnerName" runat="server" ContentEditable="false" MaxLength="100"
                                                        ToolTip="Garage Owner Name" onKeyPress="trimStartingSpace(this)" Width="40%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtGrgOwnerName" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="TxtGrgOwnerName"
                                                        ValidChars="-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvGrgOwnerName" runat="server" ControlToValidate="TxtGrgOwnerName"
                                                        Display="None" ErrorMessage="Enter the Garage Owner Name" SetFocusOnError="True"
                                                        ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgAddress1" runat="server" CssClass="styleDisplayLabel" Text="Address 1 "></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtGrgAddress1" runat="server" MaxLength="60" ContentEditable="false"
                                                        ToolTip="Address 1" Width="400px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtGrgAddress1" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtGrgAddress1" ValidChars="/,\,(,),.,-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvGrgAddress1" runat="server" ControlToValidate="TxtGrgAddress1"
                                                        Display="None" ErrorMessage=" Enter the Garage Address 1 in Garage Owner Details"
                                                        SetFocusOnError="True" ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgAddress2" runat="server" Text="Address 2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtGrgAddress2" runat="server" MaxLength="60" Width="400px" ToolTip="Address 2"
                                                        ContentEditable="false"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtGrgAddress2" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtGrgAddress2" ValidChars="/,\,(,),.,-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgCity" runat="server" Text="City" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtGrgCity" runat="server" MaxLength="30" ContentEditable="false"
                                                        ToolTip="City" Width="75%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtGrgCity" runat="server" ControlToValidate="TxtGrgCity"
                                                        Display="None" ErrorMessage="Enter the City in Garage Owner Details" SetFocusOnError="True"
                                                        ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtGrgCity" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtGrgAddress2" ValidChars="/,\,(,),.,-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <%--  <td colspan="2">
                                                </td>--%>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgState" runat="server" CssClass="styleDisplayLabel" Text="State"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="TxtGrgState" runat="server" ContentEditable="false" MaxLength="30"
                                                            ToolTip="State" Width="75%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvTxtGrgState" runat="server" ControlToValidate="TxtGrgState"
                                                            Display="None" ErrorMessage="Enter the State in Garage Owner Details" SetFocusOnError="True"
                                                            ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FTxtGrgState" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="TxtGrgState" ValidChars=" ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgCountry" runat="server" CssClass="styleDisplayLabel" Text="Country"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="TxtGrgCountry" runat="server" MaxLength="60" ContentEditable="false"
                                                            ToolTip="Country" Width="75%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtGrgCountry" runat="server" ControlToValidate="TxtGrgCountry"
                                                            Display="None" ErrorMessage="Enter the Country in Garage Owner Details" SetFocusOnError="True"
                                                            ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FTxtGrgCountry" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="TxtGrgCountry" ValidChars=" ">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblGrgPINCode" runat="server" CssClass="styleDisplayLabel" Text="PIN"></asp:Label>
                                                        <span class="styleMandatory">*</span>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="TxtGrgPINCode" runat="server" ContentEditable="false" MaxLength="10"
                                                            ToolTip="PIN" Width="60%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTxtGrgPINCode" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="TxtGrgPINCode">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvTxtGrgPINCode" runat="server" ControlToValidate="TxtGrgPINCode"
                                                            Display="None" ErrorMessage="Enter the PIN Code/Zip Code in Garage Owner Details"
                                                            SetFocusOnError="True" ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblTelephone" runat="server" CssClass="styleDisplayLabel" Text="Telephone Number"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtGrgTelephone" runat="server" MaxLength="12" ContentEditable="false"
                                                        ToolTip="Telephone Number" Width="75%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvtxtGrgTelephone" runat="server" ControlToValidate="TxtGrgTelephone"
                                                        Display="None" ErrorMessage="Enter the Telephone Number in Garage Owner Details"
                                                        SetFocusOnError="True" ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtGrgTelephone" runat="server" Enabled="True"
                                                        FilterType="Numbers" TargetControlID="TxtGrgTelephone" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgMobile" runat="server" Text="[M]"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtGrgMobile" runat="server" MaxLength="12" Width="60%" ContentEditable="false"
                                                        ToolTip="[M]"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtGrgMobile" runat="server" Enabled="True" FilterType="Numbers"
                                                        TargetControlID="TxtGrgMobile">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEmail" runat="server" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtGrgEmailId" runat="server" MaxLength="60" Width="90%" ContentEditable="false"
                                                        ToolTip="Email Id"></asp:TextBox>
                                                 <%--   <cc1:FilteredTextBoxExtender ID="FTxtGrgEmailId" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtGrgEmailId">
                                                    </cc1:FilteredTextBoxExtender>--%>
                                                    <asp:RequiredFieldValidator ID="rfvGrgEmailId" runat="server" ControlToValidate="TxtGrgEmailId"
                                                        Display="None" ErrorMessage="Enter the Email Id in Garage Owner Details" SetFocusOnError="True"
                                                        ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revGrgEmailId" runat="server" ControlToValidate="TxtGrgEmailId"
                                                        Display="None" ErrorMessage="Enter a valid Email Id" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        ValidationGroup="Contact Details"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblWebsite" runat="server" Text="Website"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtGrgWebsite" ToolTip="Website" runat="server" MaxLength="60" Width="90%"
                                                        ContentEditable="false" Style="text-transform: lowercase;"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revGrgWebsite" runat="server" ControlToValidate="TxtGrgWebsite"
                                                        Display="None" ErrorMessage="Enter a valid Website in Garage Owner Details" SetFocusOnError="True"
                                                        ValidationExpression="www\.([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" ValidationGroup="Contact Details"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel GroupingText="Garage Address Details" ID="pnlContactDtls" runat="server"
                                        CssClass="stylePanel">
                                        <table cellspacing="0" width="100%">
                                            <tr>
                                                <td colspan="6">
                                                    <asp:CheckBox ID="ChkCntctAddress" runat="server" AutoPostBack="true" ToolTip="Name and Address same as Owner's"
                                                        OnCheckedChanged="ChkCntctAddress_CheckedChanged" Text="Name and Address same as Owner's" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctName" runat="server" CssClass="styleDisplayLabel" Text="Garage Name"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtCntctName" runat="server" MaxLength="50" Width="400px" ToolTip="Garage Name"
                                                        onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtCntctName_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtCntctName" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvCntctName" runat="server" ControlToValidate="TxtCntctName"
                                                        Display="None" ErrorMessage="Enter the Garage Name" SetFocusOnError="True" ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    &nbsp;
                                                </td>
                                                <td class="styleFieldAlign">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctAddress1" runat="server" CssClass="styleDisplayLabel" Text="Address 1"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtCntctAddress1" runat="server" MaxLength="60" ToolTip="Address 1"
                                                        Width="400px" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtCntctAddress1" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="TxtCntctAddress1"
                                                        ValidChars="/,\,(,),.,-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvCntctAddress" runat="server" ControlToValidate="TxtCntctAddress1"
                                                        Display="None" ErrorMessage="Enter the Address 1 in Garage Address Details" SetFocusOnError="True"
                                                        ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctAddress2" runat="server" Text="Address 2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtCntctAddress2" runat="server" MaxLength="60" ToolTip="Address 2"
                                                        Width="400px" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtCntctAddress2" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="TxtCntctAddress2"
                                                        ValidChars="/,\,(,),.,-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctCity" runat="server" Text="City" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="TxtCntctCity" runat="server" MaxLength="30" ToolTip="City" Width="75%"
                                                            onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="TxtCntctCity_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="TxtCntctCity" ValidChars=" ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvtxtCntctCity" runat="server" ControlToValidate="TxtCntctCity"
                                                            Display="None" ErrorMessage="Enter the City in Garage Address Details" SetFocusOnError="True"
                                                            ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <%--                                                    <td colspan="2">
                                                    </td>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctState" runat="server" CssClass="styleDisplayLabel" Text="State"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="TxtCntctState" runat="server" MaxLength="30" ToolTip="State" Width="75%"
                                                            onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="TxtCntctState_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="TxtCntctState" ValidChars=" ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvtxtCntctState" runat="server" ControlToValidate="TxtCntctState"
                                                            Display="None" ErrorMessage="Enter the State in Garage Address Details" SetFocusOnError="True"
                                                            ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctCountry" runat="server" CssClass="styleDisplayLabel" Text="Country"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtCntctCountry" runat="server" ToolTip="Country" MaxLength="60"
                                                        Width="75%" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtCntctCountry_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtCntctCountry" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvtxtCntctCountry" runat="server" ControlToValidate="TxtCntctCountry"
                                                        Display="None" ErrorMessage="Enter the Country in Garage Address Details" SetFocusOnError="True"
                                                        ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctPINCode" runat="server" CssClass="styleDisplayLabel" Text="PIN"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtCntctPINCode" ToolTip="PIN" runat="server" MaxLength="10" Width="60%"
                                                        Wrap="False" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtCntctPINCode_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="txtCntctPINCode">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvtxtCntctPINCode" runat="server" ControlToValidate="txtCntctPINCode"
                                                        Display="None" ErrorMessage="Enter the PIN Code/Zip Code in Garage Address Details"
                                                        SetFocusOnError="True" ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctTelephone" runat="server" CssClass="styleDisplayLabel" Text="Telephone Number"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtCntctTelephone" ToolTip="Telephone Number" runat="server" MaxLength="12"
                                                        Width="75%" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtCntctTelephone_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Numbers" TargetControlID="TxtCntctTelephone" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtCntctTelephone" runat="server" ControlToValidate="TxtCntctTelephone"
                                                        Display="None" ErrorMessage="Enter the Telephone Number in Garage Address Details"
                                                        SetFocusOnError="True" ValidationGroup="Contact Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctMobile" runat="server" Text="[M]"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtCntctMobile" ToolTip="[M]" runat="server" MaxLength="12" Width="60%"
                                                        onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtCntctMobile_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Numbers" TargetControlID="TxtCntctMobile">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctEmail" runat="server" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtCntctEmailId" ToolTip="Email Id" runat="server" MaxLength="60"
                                                        Width="90%" Wrap="False" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <%--<cc1:FilteredTextBoxExtender ID="FTxtCntctEmailId" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="TxtCntctEmailId"></cc1:FilteredTextBoxExtender>--%>
                                                    <asp:RequiredFieldValidator ID="rfvCntctEmailId" runat="server" ControlToValidate="TxtCntctEmailId"
                                                        Display="None" ErrorMessage="Enter the Email Id in Garage Address Details" SetFocusOnError="True"
                                                        ValidationGroup="Contact Details">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revCntctEmailId" runat="server" ControlToValidate="TxtCntctEmailId"
                                                        Display="None" ErrorMessage="Enter a valid Email Id" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        ValidationGroup="Contact Details">
                                                    </asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCntctWebsite" runat="server" Text="Website"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtCntctWebsite" onKeyPress="trimStartingSpace(this)" ToolTip="Website"
                                                        runat="server" MaxLength="60" Width="90%" Style="text-transform: lowercase;"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revCntctWebsite" runat="server" ControlToValidate="TxtCntctWebsite"
                                                        Display="None" ErrorMessage="Enter a valid Website in Garage Address Details"
                                                        SetFocusOnError="True" ValidationExpression="www\.([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                                        ValidationGroup="Contact Details"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabGarageDetails" CssClass="tabpan" BackColor="Red"
                        HeaderText="Garage Details">
                        <HeaderTemplate>
                            Garage Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="Updrental" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel GroupingText="Garage Rental Details" ID="pnlGrgRentlDtls" runat="server"
                                        CssClass="stylePanel">
                                        <table border="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPymntFreq" runat="server" CssClass="styleDisplayLabel" Text="Payment Frequency"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlPymntFreq" ToolTip="Payment Frequency" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlPymntFreq" runat="server" ControlToValidate="ddlPymntFreq"
                                                        Display="None" ErrorMessage="Select a Payment Frequency" InitialValue="0" SetFocusOnError="True"
                                                        ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSqFeet" runat="server" CssClass="styleDisplayLabel" Text="Square Feet"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtSqFeet" ToolTip="Square Feet" runat="server" Style="text-align: right"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtSqFeet" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                        TargetControlID="TxtSqFeet" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RqvTxtSqFeet" runat="server" ControlToValidate="TxtSqFeet"
                                                        Display="None" ErrorMessage="Enter the Square Feet" SetFocusOnError="True" ValidationGroup="Garage Details"
                                                        CssClass="styleMandatoryLabel"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblRent" runat="server" CssClass="styleDisplayLabel" Text="Rent Per Sq.Feet"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtRntSqFeet" ToolTip="Rent Per Sq.Feet" runat="server" Style="text-align: right"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtRntSqFeet" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                        TargetControlID="TxtRntSqFeet" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RqvTxtRntSqFeet" runat="server" ControlToValidate="TxtRntSqFeet"
                                                        Display="None" ErrorMessage="Enter the Rent Per Square Feet" SetFocusOnError="True"
                                                        ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblTotRent" runat="server" CssClass="styleDisplayLabel" Text="Total Rent"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtTotRent" runat="server" ToolTip="Total Rent" ContentEditable="false"
                                                        Style="text-align: right"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtTotRent" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                        TargetControlID="TxtTotRent" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RqvTxtTotRent" runat="server" ControlToValidate="TxtTotRent"
                                                        Display="None" ErrorMessage="Calculate the Total Rent" SetFocusOnError="True"
                                                        ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGrgCapcity" runat="server" CssClass="styleDisplayLabel" Text="Garage Capacity"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtGrgCpcty" ToolTip="Garage Capacity" runat="server" MaxLength="3"
                                                        Style="text-align: right"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtGrgCpcty" runat="server" Enabled="True" FilterType="Numbers"
                                                        TargetControlID="TxtGrgCpcty" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RqvTxtGrgCpcty" runat="server" ControlToValidate="TxtGrgCpcty"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Garage Capacity"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCoverprk" runat="server" Text="Covered Parking"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:RadioButtonList ID="RdbCvrPrk" runat="server" ToolTip="Covered Parking" RepeatDirection="Horizontal"
                                                        RepeatLayout="Flow">
                                                        <asp:ListItem Value="True">Yes</asp:ListItem>
                                                        <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator ID="RqvCvrPrk" runat="server" ControlToValidate="RdbCvrPrk"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Whether Car Parking is available"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblRemarks0" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="5">
                                                    <asp:TextBox ID="TxtRemarks" ToolTip="Remarks" runat="server" Height="80px" MaxLength="100"
                                                        onkeyup="maxlengthfortxt(100)" onchange="maxlengthfortxt(100)" onKeyPress="trimStartingSpace(this)"
                                                        TextMode="MultiLine" Wrap="true" Columns="60" Width="75%"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel GroupingText="Garage Insurance Details" ID="PnlGrgInsurance" runat="server"
                                        CssClass="stylePanel">
                                        <table cellspacing="0" width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" style="padding-top: 10px">
                                                    <asp:Label ID="LblInsCvr" runat="server" CssClass="styleDisplayLabel" Text="Insurance Covered"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3" style="padding-top: 10px">
                                                    <asp:RadioButtonList ID="RdbInsCvr" ToolTip="Insurance Covered" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="RdbInsCvr_SelectedIndexChanged" RepeatDirection="Horizontal"
                                                        RepeatLayout="Flow">
                                                        <asp:ListItem Value="True">Yes</asp:ListItem>
                                                        <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:RequiredFieldValidator ID="RqvInsCvr" runat="server" ControlToValidate="RdbInsCvr"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Whether Insurance is covered"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="22%">
                                                    <asp:Label ID="LblInsCompany" runat="server" CssClass="styleDisplayLabel" Text="Insurance Company"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtInsCompany" ToolTip="Insurance Company" MaxLength="25" runat="server"
                                                        onKeyPress="trimStartingSpace(this)" Width="30%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtInsCompanyExtender" runat="server" Enabled="True"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="TxtInsCompany"
                                                        ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RqvTxtInsCompany" runat="server" ControlToValidate="TxtInsCompany"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Insurance Company Name"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                </td>
                                                <td class="styleFieldAlign">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPolNo" runat="server" CssClass="styleDisplayLabel" Text="Policy Number"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtPolicyNo" ToolTip="Policy Number" MaxLength="25" runat="server"
                                                        onKeyPress="trimStartingSpace(this)" Width="30%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtPolicyNo" runat="server" Enabled="True" FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtPolicyNo">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RqvTxtPolicyNo" runat="server" ControlToValidate="TxtPolicyNo"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Policy Number"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="LblPolicyAmnt" runat="server" CssClass="styleDisplayLabel" Text="Policy Amount"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtPlcyAmnt" ToolTip="Policy Amount" runat="server" Width="30%"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtPlcyAmnt" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                        TargetControlID="TxtPlcyAmnt" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RqvPolicyAmnt" runat="server" ControlToValidate="TxtPlcyAmnt"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Policy Amount"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsAddress" runat="server" CssClass="styleDisplayLabel" Text="Address 1"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsAddress1" ToolTip="Address 1" runat="server" MaxLength="60"
                                                        onKeyPress="trimStartingSpace(this)" Width="400px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtInsAddress1" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtInsAddress1" ValidChars="/,\,(,),.,-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvInsAddress1" runat="server" ControlToValidate="TxtInsAddress1"
                                                        Display="None" ErrorMessage="Enter the Address 1 in Garage Insurance Details"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsAddress2" runat="server" Text="Address 2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsAddress2" ToolTip="Address 2" runat="server" MaxLength="60"
                                                        onKeyPress="trimStartingSpace(this)" Width="400px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTxtInsAddress2" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtInsAddress2" ValidChars="/,\,(,),.,-, ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsCity" runat="server" CssClass="styleDisplayLabel" Text="City"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsCity" ToolTip="City" runat="server" MaxLength="30" Width="75%"
                                                        onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtInstCity_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtInsCity" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvtxtInsCity" runat="server" ControlToValidate="TxtInsCity"
                                                        Display="None" ErrorMessage="Enter the City in Garage Insurance Details" SetFocusOnError="True"
                                                        ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    &nbsp;
                                                </td>
                                                <td width="15%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsState" runat="server" CssClass="styleDisplayLabel" Text="State"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsState" ToolTip="State" runat="server" MaxLength="60" Width="75%"
                                                        onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtInsState_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtInsState" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvtxtInsState" runat="server" ControlToValidate="TxtInsState"
                                                        Display="None" ErrorMessage="Enter the State in Garage Insurance Details" SetFocusOnError="True"
                                                        ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsCountry" runat="server" CssClass="styleDisplayLabel" Text="Country"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsCountry" ToolTip="Country" runat="server" MaxLength="60" Width="75%"
                                                        onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtInsCountry_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="TxtInsCountry" ValidChars=" " />
                                                    <asp:RequiredFieldValidator ID="rfvtxtInsCountry" runat="server" ControlToValidate="TxtInsCountry"
                                                        Display="None" ErrorMessage="Enter the Country in Garage Insurance Details" SetFocusOnError="True"
                                                        ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsPINCode" runat="server" CssClass="styleDisplayLabel" Text="PIN"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsPINCode" ToolTip="PIN" runat="server" MaxLength="10" Width="60%"
                                                        Wrap="False" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtInsPINCode_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Numbers" TargetControlID="TxtInsPINCode">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvtxtInsPINCode" runat="server" ControlToValidate="TxtInsPINCode"
                                                        Display="None" ErrorMessage="Enter the PIN Code/Zip Code in Garage Insurance Details"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsTelephone" runat="server" CssClass="styleDisplayLabel" Text="Telephone Number"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsTelephone" ToolTip="Telephone Number" runat="server" MaxLength="12"
                                                        Width="75%" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtInsTelephone_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterType="Numbers" TargetControlID="TxtInsTelephone" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtInsTelephone" runat="server" ControlToValidate="TxtInsTelephone"
                                                        Display="None" ErrorMessage="Enter the Telephone Number in Garage Insurance Details"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsMobile" runat="server" Text="[M]"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtInsMobile" ToolTip="[M]" runat="server" MaxLength="12" Width="60%"
                                                        Wrap="False" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="TxtInsMobile_FilteredTextBoxExtender" runat="server"
                                                        BehaviorID="TxtInsMobile_FilteredTextBoxExtender" Enabled="True" FilterType="Numbers"
                                                        TargetControlID="TxtInsMobile">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsEmail" runat="server" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtInsEmailId" ToolTip="Email Id" runat="server" MaxLength="60"
                                                        Width="90%" Wrap="False" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvInsEmailId" runat="server" ControlToValidate="TxtInsEmailId"
                                                        Display="None" ErrorMessage="Enter the Email Id in Garage Insurance Details"
                                                        SetFocusOnError="True" ValidationGroup="Garage Details"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revInsEmailId" runat="server" ControlToValidate="TxtInsEmailId"
                                                        Display="None" ErrorMessage="Enter a valid Email Id in Garage Insurance Details"
                                                        SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        ValidationGroup="Garage Details"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsWebsite" runat="server" Text="Website"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="TxtInsWebsite" ToolTip="Website" runat="server" MaxLength="60" Style="text-transform: lowercase;"
                                                        Width="90%" onKeyPress="trimStartingSpace(this)"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvWebsite" runat="server" Display="None" SetFocusOnError="True"
                                                    ValidationGroup="EntityGroup" ErrorMessage="Enter the Website Name" ControlToValidate="txtWebsite"
                                                    meta:resourcekey="rfvWebsiteResource1"></asp:RequiredFieldValidator>--%><asp:RegularExpressionValidator
                                                        ID="revinsWebsite" runat="server" ControlToValidate="TxtInsWebsite" Display="None"
                                                        ErrorMessage="Enter a valid Website in Garage Insurance Details" SetFocusOnError="True"
                                                        ValidationExpression="www\.([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" ValidationGroup="Garage Details"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel GroupingText="Repossession Account Details" ID="PnlAccDtls" runat="server"
                                        CssClass="stylePanel" Width="1000px">
                                        <table width="100%" cellpadding="0" cellspacing="0" align="center">
                                            <tr>
                                                <td align="right" style="padding-right: 1%;">
                                                    <asp:Button ID="btnAll" runat="server" CssClass="styleSubmitButton" OnClick="btnAll_Click"
                                                        Text="All" />&nbsp;
                                                    <asp:Button ID="btnCurrent" runat="server" CssClass="styleSubmitButton" Text="Current"
                                                        OnClick="btnCurrent_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grvAccDtls" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        ToolTip="Repossession Account Details">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Prime Account Number">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblMSA" runat="server" Text='<%# Bind("MLA") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblMSA" runat="server" Text='<%# Bind("MLA") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sub Account Number">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblSLA" runat="server" Text='<%# Bind("SLA") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblSLA" runat="server" Text='<%# Bind("SLA") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Customer Name">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblCustName" runat="server" Text='<%# Bind("CUSTNAME") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblCustName" runat="server" Text='<%# Bind("CUSTNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Details">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblAssetDtls" runat="server" Text='<%# Bind("ASSETDESC") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblAssetDtls" runat="server" Text='<%# Bind("ASSETDESC") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Registration Number">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblRegNo" runat="server" Text='<%# Bind("REGNNO") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblRegNo" runat="server" Text='<%# Bind("REGNNO") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Garage In Date">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblGrgInDt" runat="server" Text='<%# Bind("GARAGEIN") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblGrgInDt" runat="server" Text='<%# Bind("GARAGEIN") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Garage Out Date">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblGrgOutDt" runat="server" Text='<%# Bind("GARAGEOUT") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblGrgOutDt" runat="server" Text='<%# Bind("GARAGEOUT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <AlternatingItemTemplate>
                                                                    <asp:Label ID="LblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                                                                </AlternatingItemTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="LblRemarks" runat="server" Text='<%# Bind("REMARKS") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 990px">
                <br />
                <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                    ToolTip="Save" OnClientClick="return fnCheckPageValidators('Garage Details');"
                    Text="Save" />
                <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    ToolTip="Clear" Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    ToolTip="Cancel" Text="Cancel" OnClick="btnCancel_Click" />
                <asp:Button ID="btnProceedSave" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    Text="Proceed Save" OnClick="btnProceedSave_Click" Style="display: none" />
            </td>
        </tr>
        <tr>
            <td style="width: 990px">
                <%--  <asp:ValidationSummary ID="vsGarageMaster" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                    ShowMessageBox="false" HeaderText="Correct the following validation(s):" DisplayMode="List" />--%>
                <asp:ValidationSummary ID="vsGarageMaster" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                    HeaderText="Please correct the following validation(s):" />
                <%--  <asp:ValidationSummary ID="vs1" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                    ShowMessageBox="false" HeaderText="Correct the following validation(s):" DisplayMode="List"  ValidationGroup="Garage Details"/>--%>
                <asp:CustomValidator ID="cvGarageMaster" runat="server" CssClass="styleMandatoryLabel"
                    Enabled="true" />
            </td>
        </tr>
    </table>
    <asp:TextBox ID="txthiddenfield" runat="server" Visible="false"></asp:TextBox>

    <script language="javascript" type="text/javascript">
       var tab ;

                function pageLoad() {
                   tab = $find('ctl00_ContentPlaceHolder1_tcGarageMaster');
                   var querymode=location.search.split("qsMode=");
                   if(querymode[1]!='Q')
                   {
                    tab.add_activeTabChanged(on_Change);
                       var newindex=tab.get_activeTabIndex(index);
                       var btnSave = document.getElementById('<%=btnSave.ClientID %>')
                       if(newindex == tab._tabs.length-1)
                            btnSave.disabled=false;
                        else
                            btnSave.disabled=true;
                   }
                }

var index=0;
      function on_Change(sender,e)
      {
            //tab = $find('ctl00_ContentPlaceHolder1_tcPricing');
            //debugger;
            var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var Valgrp = document.getElementById('<%=vsGarageMaster.ClientID %>')
            var btnSave = document.getElementById('<%=btnSave.ClientID %>')
            
            //btnSave.disabled=true;
            Valgrp.validationGroup=strValgrp;
           
            var newindex=tab.get_activeTabIndex(index);
            if(newindex == tab._tabs.length-1)
                   btnSave.disabled=false;
            else
                btnSave.disabled=true;
            if(newindex >index)
            {
                if(!fnCheckPageValidators(strValgrp,false))
                {
                    
                      tab.set_activeTabIndex(index);
                    
                     
                }
                else
                {
                    index=tab.get_activeTabIndex(index);
                    var strValgrp_new = tab._tabs[index]._tab.outerText.trim();
                    Valgrp.validationGroup=strValgrp_new;
                }
            }
            else
            {
                 tab.set_activeTabIndex(newindex);
                 index=tab.get_activeTabIndex(newindex);
      
            }
            
      } 
       function trimStartingSpace(textbox)
   {
    var textValue = textbox.value;
    if(textValue.length==0 && window.event.keyCode == 32)
    {
     window.event.keyCode = 0;
     return false;
    }
   } 
      function CalcTotrent()
      {
//      alert('inside');
        flSqFt = parseFloat(document.getElementById('<%= TxtSqFeet.ClientID %>').value);
        flRntPerSqFt =parseFloat(document.getElementById('<%=TxtRntSqFeet.ClientID %>').value);
        flTotRent=  flSqFt * flRntPerSqFt;
        if(isNaN(flTotRent)==true)
        {
        document.getElementById('<%=TxtTotRent.ClientID %>').value = '0';
        }
        else
        {
        document.getElementById('<%=TxtTotRent.ClientID %>').value = flTotRent.toFixed(2);
        }
        
        return;
      }
    </script>

</asp:Content>
