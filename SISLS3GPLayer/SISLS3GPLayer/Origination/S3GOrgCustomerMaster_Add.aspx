<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GOrgCustomerMaster_Add.aspx.cs" Inherits="Origination_S3GOrgCustomerMaster_Add"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcCustomerMaster" runat="server" CssClass="styleTabPanel" Width="99%"
                            TabStripPlacement="top" ActiveTabIndex="0">
                            <cc1:TabPanel runat="server" HeaderText="Customer Details" ID="tbAddress" CssClass="tabpan"
                                BackColor="Red" TabIndex="0">
                                <HeaderTemplate>
                                    Main
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upAddress" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel GroupingText="Customer Informations" ID="pnlCustomerInfo" runat="server"
                                                CssClass="stylePanel">
                                                <table width="100%" border="0" cellspacing="0">
                                                    <tr>
                                                        <td class="styleFieldAlign">
                                                            <asp:CheckBox runat="server" ID="chkCustomer" Text="Customer" TextAlign="Right" />
                                                            <asp:CheckBox runat="server" ID="chkGuarantor1" Text="Guarantor1" TextAlign="Right" />
                                                            <asp:CheckBox runat="server" ID="chkGuarantor2" Text="Guarantor2" TextAlign="Right" />
                                                            <asp:CheckBox runat="server" ID="chkCoApplicant" Text="Co-Applicant" TextAlign="Right" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%" border="0" cellspacing="0">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblCustomercode" CssClass="styleReqFieldLabel" Text="Customer Code"
                                                                Visible="False"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtCustomercode" runat="server" Width="60%" ReadOnly="True" Visible="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblTitle" CssClass="styleReqFieldLabel" Text="Title"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="30%">
                                                            <asp:DropDownList ID="ddlTitle" runat="server" Width="33%" TabIndex="0">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvTitle" runat="server" ControlToValidate="ddlTitle"
                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblCustomerName" CssClass="styleReqFieldLabel" Text="Customer Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="30%">
                                                            <asp:TextBox ID="txtCustomerName" runat="server" Width="95%" MaxLength="100" onfocusOut="fnvalidcustomername(this);"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtCustomerName" ValidChars=" .!@#$%^&*()" TargetControlID="txtCustomerName"
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server" FilterMode="ValidChars"
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ValidationGroup="Main" ID="rvfCustomerName" runat="server"
                                                                ControlToValidate="txtCustomerName" CssClass="styleMandatoryLabel" Display="None"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblCustomerType" CssClass="styleReqFieldLabel" Text="Customer Type"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:UpdatePanel ID="upCustomerType" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:DropDownList ID="ddlCustomerType" runat="server" Width="50%">
                                                                    </asp:DropDownList>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                            <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvCustomerType" runat="server"
                                                                ControlToValidate="ddlCustomerType" CssClass="styleMandatoryLabel" Display="None"
                                                                InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblFamilyName" CssClass="styleReqFieldLabel" Text="Short Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtFamilyName" runat="server" MaxLength="6" Width="75%"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvShortName" runat="server"
                                                                ControlToValidate="txtFamilyName" CssClass="styleMandatoryLabel" Display="None"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <asp:UpdatePanel ID="upGroupCode" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblgroupcode" CssClass="styleDisplayLabel" Text="Group Code / Name"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="cmbGroupCode" runat="server" MaxLength="4" OnTextChanged="cmbGroupCode_OnTextChanged"
                                                                        AutoPostBack="true" Width="23%"></asp:TextBox>
                                                                    <cc1:TextBoxWatermarkExtender ID="txtwmkGroupCode" WatermarkText="--Select--" runat="server"
                                                                        TargetControlID="cmbGroupCode" Enabled="true">
                                                                    </cc1:TextBoxWatermarkExtender>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtenderDemo" runat="server" TargetControlID="cmbGroupCode"
                                                                        ServiceMethod="GetCompletionList" MinimumPrefixLength="1" CompletionSetCount="20"
                                                                        DelimiterCharacters="" Enabled="True" ServicePath="">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="cmbGroupCode"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server"
                                                                        Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:TextBox ID="txtGroupName" runat="server" MaxLength="50" Width="60%"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvGroupName" runat="server" ControlToValidate="txtGroupName"
                                                                        Enabled="false" Display="None" SetFocusOnError="true" ValidationGroup="Main"
                                                                        ErrorMessage="Enter the Group Name"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblConstitutionName" CssClass="styleReqFieldLabel"
                                                                        Text="Customer Constitution"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:UpdatePanel ID="upConstitutionName" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:DropDownList ID="ddlConstitutionName" runat="server" Width="77%" AutoPostBack="True"
                                                                                OnSelectedIndexChanged="ddlConstitutionName_OnSelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                    <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvConstitutionName" runat="server"
                                                                        ControlToValidate="ddlConstitutionName" CssClass="styleMandatoryLabel" Display="None"
                                                                        InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:PostBackTrigger ControlID="cmbGroupCode" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </tr>
                                                    <tr>
                                                        <asp:UpdatePanel ID="upIndustryCode" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblIndustryCode" CssClass="styleDisplayLabel" Text="Industry Code / Name"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="cmbIndustryCode" runat="server" MaxLength="3" OnTextChanged="cmbIndustryCode_OnTextChanged"
                                                                        AutoPostBack="true" Width="23%"></asp:TextBox>
                                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" WatermarkText="--Select--"
                                                                        runat="server" TargetControlID="cmbIndustryCode" Enabled="true">
                                                                    </cc1:TextBoxWatermarkExtender>
                                                                    <cc1:AutoCompleteExtender ID="AutoCompleteList" runat="server" TargetControlID="cmbIndustryCode"
                                                                        ServiceMethod="GetIndustryList" MinimumPrefixLength="1" CompletionSetCount="3"
                                                                        DelimiterCharacters="" Enabled="True" ServicePath="">
                                                                    </cc1:AutoCompleteExtender>
                                                                    <cc1:FilteredTextBoxExtender ID="FTBEIndustryCode" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                        Enabled="True" TargetControlID="cmbIndustryCode">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvIndustryCode" runat="server" ControlToValidate="cmbIndustryCode"
                                                                        Display="None" InitialValue="" SetFocusOnError="true" ValidationGroup="Main"
                                                                        Enabled="false" ErrorMessage="Select / Enter the Industry Code"></asp:RequiredFieldValidator>
                                                                    <asp:TextBox ID="txtIndustryName" runat="server" Width="60%" MaxLength="50" OnTextChanged="txtIndustryName_OnTextChanged"
                                                                        AutoPostBack="true"></asp:TextBox>

                                                                    <cc1:FilteredTextBoxExtender ID="ftxtIndustryName" runat="server" Enabled="True"
                                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtIndustryName"
                                                                        ValidChars="  .~`!@#$%^&amp;*()_-+=[]{};':&lt;&gt;,?/">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvIndustryName" runat="server" ControlToValidate="txtIndustryName"
                                                                        Enabled="false" Display="None" SetFocusOnError="true" ValidationGroup="Main"
                                                                        ErrorMessage="Enter the Industry Name"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="styleFieldLabel" valign="top">
                                                                    <asp:Label runat="server" ID="lblNotes" CssClass="styleDisplayLabel" Text="Notes"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtNotes" runat="server" Width="95%" MaxLength="250" TextMode="MultiLine"
                                                                        onkeyup="maxlengthfortxt(250);"></asp:TextBox>
                                                                </td>

                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblCustomerPostingGroup" CssClass="styleReqFieldLabel"
                                                                Text="Posting Group Code"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlPostingGroup" runat="server" Width="230px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvPostingGroup" runat="server"
                                                                ControlToValidate="ddlPostingGroup" CssClass="styleMandatoryLabel" Display="None"
                                                                SetFocusOnError="True" InitialValue="-1"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel" valign="top">
                                                            <asp:Label runat="server" ID="lblCompanyType" CssClass="styleReqFieldLabel" Text="Company Type"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlCompanyType" runat="server" Width="77%"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvCompanyType" runat="server"
                                                                ControlToValidate="ddlCompanyType" CssClass="styleMandatoryLabel" Display="None"
                                                                InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblCreditType" Text="Credit Type"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:RadioButtonList ID="rbCreditType" runat="server" RepeatDirection="Horizontal"
                                                                AutoPostBack="false" ToolTip="Credit Type">
                                                                <asp:ListItem Text="Oneshot" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Revolving" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblBillAdd" CssClass="styleReqFieldLabel"
                                                                Text="Billing Address"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlBillAddress" runat="server" Width="230px">
                                                                <asp:ListItem Text="Corporate" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Branch" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <%-- <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvBillAddress" runat="server" ErrorMessage="Select Billing Address"
                                                        ControlToValidate="ddlBillAddress" CssClass="styleMandatoryLabel" Display="None"
                                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblInvCovLetter" CssClass="styleReqFieldLabel"
                                                                Text="Invoice Covering Letter"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlInvCovLetter" runat="server">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>

                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblStateWiseBilling" CssClass="styleDisplayLabel"
                                                                Text="Statewise Billing"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                              <asp:CheckBox runat="server" ID="ChkstatewiseBilling" CssClass="styleDisplayLabel"
                                                             />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <table width="100%">
                                                <tr>
                                                    <td valign="top" style="width: 50%;">
                                                        <asp:Panel GroupingText="Corporate Address" ID="pnlCommAddress" runat="server"
                                                            CssClass="stylePanel">
                                                            <table width="100%" align="center" cellspacing="0">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%" style="padding-bottom: 0px">
                                                                        <asp:Label ID="lblComAddress1" runat="server" CssClass="styleReqFieldLabel" Text="Address 1"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="padding-bottom: 0px">
                                                                        <%--<asp:TextBox ID="txtComAddress1" runat="server" MaxLength="60" TextMode="MultiLine"
                                                                    Columns="30" onkeyup="maxlengthfortxt(64)"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="txtComAddress1" runat="server" MaxLength="60" Width="95%"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvComAddress1" runat="server" ControlToValidate="txtComAddress1"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%" style="padding-bottom: 0px">
                                                                        <asp:Label ID="lblComAddress2" runat="server" CssClass="styleDisplayLabel" Text="Address 2"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%-- <asp:TextBox ID="txtCOmAddress2" runat="server" MaxLength="60" TextMode="MultiLine"
                                                                    Columns="30" onkeyup="maxlengthfortxt(64)"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="txtCOmAddress2" runat="server" MaxLength="60" Width="95%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%" style="padding-bottom: 0px">
                                                                        <asp:Label ID="lblComcity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%--<asp:TextBox ID="txtComCity" runat="server" MaxLength="30" Width="60%"></asp:TextBox>--%>
                                                                        <uc2:Suggest ID="txtComCity" runat="server" ServiceMethod="GetCityList"
                                                                            ValidationGroup="Main" IsMandatory="true" ItemToValidate="Text" Width="170px" />
                                                                        <%--<cc1:ComboBox ID="txtComCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend">
                                                                </cc1:ComboBox>--%>
                                                                        <%--<cc1:FilteredTextBoxExtender ID="ftxtComCity" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                    TargetControlID="txtComCity" ValidChars=" ">
                                                                </cc1:FilteredTextBoxExtender>--%>
                                                                        <%--<asp:RequiredFieldValidator ID="rfvComCity" runat="server" ControlToValidate="txtComCity"
                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Main"
                                                                    InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblComState" runat="server" CssClass="styleReqFieldLabel" Text="State"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%--<asp:TextBox ID="txtComState" runat="server" MaxLength="60" Width="60%"></asp:TextBox>--%>
                                                                        <%-- <cc1:ComboBox ID="txtComState" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend">
                                                                </cc1:ComboBox>--%>
                                                                        <asp:DropDownList ID="ddlComState" runat="server" Width="35%">
                                                                        </asp:DropDownList>
                                                                        <%--<cc1:FilteredTextBoxExtender ID="ftxtComState" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                    TargetControlID="txtComState" ValidChars=" ">
                                                                </cc1:FilteredTextBoxExtender>--%>
                                                                        <asp:RequiredFieldValidator ID="rfvComState" runat="server" ControlToValidate="ddlComState"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                            ValidationGroup="Main"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblComCountry" runat="server" CssClass="styleReqFieldLabel" Text="Country"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%-- <asp:TextBox ID="txtComCountry" runat="server" MaxLength="60" Width="37%"></asp:TextBox>--%>
                                                                        <cc1:ComboBox ID="txtComCountry" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                                            Width="80px">
                                                                        </cc1:ComboBox>
                                                                        <%-- <cc1:FilteredTextBoxExtender ID="ftxtComCountry" runat="server" Enabled="True" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                                    TargetControlID="txtComCountry" ValidChars=" ">
                                                                </cc1:FilteredTextBoxExtender>--%>
                                                                        <asp:RequiredFieldValidator ID="rfvComCountry" runat="server" ControlToValidate="txtComCountry"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                            ValidationGroup="Main"></asp:RequiredFieldValidator>
                                                                        <asp:Label ID="lblCompincode" runat="server" CssClass="styleDisplayLabel" Text="PIN"></asp:Label>
                                                                        <asp:TextBox ID="txtComPincode" runat="server" MaxLength="10" Width="34%"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtComPincode" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtComPincode" ValidChars=" ">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvComPincode" runat="server" ControlToValidate="txtComPincode" Enabled="false"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblComTelephone" runat="server" CssClass="styleDisplayLabel" Text="Telephone"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtComTelephone" runat="server" MaxLength="12" Width="102px"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtComTelephone" runat="server" Enabled="True"
                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtComTelephone"
                                                                            ValidChars=" -">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvComTelephone" Enabled="false" runat="server" ControlToValidate="txtComTelephone"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                                                        <asp:Label ID="lblComMobile" runat="server" CssClass="styleDisplayLabel" Text="[M]"></asp:Label>
                                                                        &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtComMobile" runat="server" MaxLength="12" Width="34%"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtComMobile" runat="server" Enabled="True" FilterType="Numbers"
                                                                            TargetControlID="txtComMobile" ValidChars=" -">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblComEmail" runat="server" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtComEmail" runat="server" MaxLength="180" Width="85%"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtComEmail"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;|,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
                                                                            ValidationGroup="Main"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblComWebSite" runat="server" Text="Web Site"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtComWebsite" runat="server" MaxLength="60" Width="85%"></asp:TextBox>

                                                                        <asp:RegularExpressionValidator ID="revComWebsite" runat="server" ControlToValidate="txtComWebsite"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                                                            ValidationGroup="Main"></asp:RegularExpressionValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvComWebsite" runat="server" ControlToValidate="txtComWebsite" Enabled="false"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                            ValidationGroup="Main"></asp:RequiredFieldValidator>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtComWebsite" ValidChars=".">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblPAN" runat="server" CssClass="styleReqFieldLabel" Text="PAN"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPAN" runat="server" MaxLength="15" Width="85%"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvComPAN" runat="server" ControlToValidate="txtPAN"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                            ValidationGroup="Main"></asp:RequiredFieldValidator>
                                                                        <asp:RegularExpressionValidator ID="revPAN" runat="server"
                                                                            ControlToValidate="txtPAN" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Main"
                                                                            ValidationExpression="^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$">
                                                                        </asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblCIN" runat="server" CssClass="styleDisplayLabel" Text="CIN"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtCIN" runat="server" MaxLength="21" Width="85%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblTIN" runat="server" CssClass="styleDisplayLabel" Text="TIN"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtTIN" runat="server" MaxLength="15" Width="85%"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="revComTIN" runat="server" ControlToValidate="txtTIN"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" Enabled="false"
                                                                            ValidationGroup="Main"></asp:RegularExpressionValidator>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtTIN" ValidChars=". " runat="server" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblTAN" runat="server" CssClass="styleReqFieldLabel" Text="TAN"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtTAN" runat="server" MaxLength="10" Width="85%"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ValidationGroup="Main" ID="rfvComTAN" runat="server"
                                                                            ControlToValidate="txtTAN" CssClass="styleMandatoryLabel" Display="None"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        <asp:RegularExpressionValidator ID="recComTAN" runat="server" ControlToValidate="txtTAN"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter a valid TAN in Corporate Address"
                                                                            ValidationExpression="^([a-zA-Z]){4}([0-9]){5}([a-zA-Z]){1}?$"
                                                                            ValidationGroup="Main"></asp:RegularExpressionValidator>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtTAN" ValidChars=". " runat="server" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
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
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                    <td valign="top" style="width: 50%;">
                                                        <asp:Panel GroupingText="Billing Address" ID="pnlPermenantAddress" runat="server"
                                                            CssClass="stylePanel">
                                                            <table width="100%" cellspacing="0">


                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblPerState" CssClass="styleReqFieldLabel" Text="State"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%--<asp:TextBox ID="txtPerState" runat="server" Width="60%" MaxLength="60"></asp:TextBox>--%>
                                                                        <%-- <cc1:ComboBox ID="txtPerState" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend">
                                                                </cc1:ComboBox>--%>
                                                                        <asp:DropDownList ID="ddlPerState" runat="server" Width="35%"></asp:DropDownList>
                                                                        <%-- <cc1:FilteredTextBoxExtender ID="ftxtPerState" runat="server" ValidChars=" " FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                    Enabled="True" TargetControlID="txtPerState">
                                                                </cc1:FilteredTextBoxExtender>--%>
                                                                        <asp:RequiredFieldValidator ValidationGroup="Baddress" ID="rfvPerState" runat="server"
                                                                            ControlToValidate="ddlPerState" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblPerCity" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%-- <asp:TextBox ID="txtPerCity" runat="server" Width="60%" MaxLength="30"></asp:TextBox>--%>
                                                                        <uc2:Suggest ID="txtPerCity" runat="server" ServiceMethod="GetCityList" ErrorMessage="Enter the city in Billing Address"
                                                                            ValidationGroup="Baddress" IsMandatory="true" ItemToValidate="Text" Width="170px" />
                                                                        <%--<cc1:ComboBox ID="txtPerCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend">
                                                                </cc1:ComboBox>--%>
                                                                        <%-- <cc1:FilteredTextBoxExtender ID="ftxtPerCity" runat="server" ValidChars=" " FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                    Enabled="True" TargetControlID="txtPerCity">
                                                                </cc1:FilteredTextBoxExtender>--%>
                                                                        <%-- <asp:RequiredFieldValidator ValidationGroup="Bank" ID="rfvPerCity" runat="server"
                                                                    ControlToValidate="txtPerCity" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter city in billing Address"
                                                                    SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblPerAddress1" CssClass="styleReqFieldLabel" Text="Address 1"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%-- <asp:TextBox ID="txtPerAddress1" runat="server" MaxLength="60" TextMode="MultiLine"
                                                                    onkeyup="maxlengthfortxt(64)" Columns="30"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="txtPerAddress1" runat="server" MaxLength="60" Width="95%"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ValidationGroup="Baddress" ID="rfvtxtPerAddress1" runat="server" ErrorMessage="Enter Address1 in Billing Address"
                                                                            ControlToValidate="txtPerAddress1" CssClass="styleMandatoryLabel" Display="None"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblPerAddress2" CssClass="styleDisplayLabel" Text="Address 2"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <%-- <asp:TextBox ID="txtPerAddress2" runat="server" Columns="30" MaxLength="60" TextMode="MultiLine"
                                                                    onkeyup="maxlengthfortxt(64)"></asp:TextBox>--%>
                                                                        <asp:TextBox ID="txtPerAddress2" runat="server" Columns="30" MaxLength="60" Width="95%"></asp:TextBox>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblPerpincode" CssClass="styleDisplayLabel" Text="PIN"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPerPincode" runat="server" Width="34%" MaxLength="10"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtPerPincode" runat="server" ValidChars=" " FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            Enabled="True" TargetControlID="txtPerPincode">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ValidationGroup="Baddress" ID="rvfPerPincode" runat="server" Enabled="false" ErrorMessage="Enter PIN code in Billing Address"
                                                                            ControlToValidate="txtPerPincode" CssClass="styleMandatoryLabel" Display="None"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblPerEmail" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPerEmail" runat="server" Width="85%" MaxLength="180"></asp:TextBox>

                                                                        <asp:RegularExpressionValidator ValidationGroup="Baddress" ID="revPerEmail" ErrorMessage="Enter a valid EmailID in Billing Address"
                                                                            runat="server" ControlToValidate="txtPerEmail" Display="None" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;|,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
                                                                        <asp:RequiredFieldValidator ValidationGroup="Baddress" ID="rfvPerEmail" runat="server" ErrorMessage="Enter the EmailID in Billing Address"
                                                                            ControlToValidate="txtPerEmail" CssClass="styleMandatoryLabel" Display="None" Enabled="false"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblContName" CssClass="styleDisplayLabel" Text="Contact Name"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPerContName" runat="server" Width="85%" MaxLength="50"></asp:TextBox>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label runat="server" ID="lblPerContactNo" CssClass="styleDisplayLabel" Text="Contact No"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPerContactNo" runat="server" Width="85%" MaxLength="12"></asp:TextBox>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblPerTIN" runat="server" CssClass="styleDisplayLabel" Text="TIN"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPerTIN" runat="server" MaxLength="15" Width="85%"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator ID="revPerTIN" runat="server" ControlToValidate="txtPerTIN"
                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="^[^/\\()~!@#$%^&*]*$?"
                                                                            ValidationGroup="Baddress"></asp:RegularExpressionValidator>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtPerTIN" ValidChars="." runat="server" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="20%">
                                                                        <asp:Label ID="lblPerTAN" runat="server" Text="TAN"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPerTAN" runat="server" MaxLength="10" Width="85%"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftexEntityName" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtPerTAN" ValidChars="." runat="server" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ValidationGroup="Baddress" ID="rfvComBTAN" runat="server" Enabled="false"
                                                                            ControlToValidate="txtPerTAN" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Tan Number in Billing Address"
                                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
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
                                                                <tr>
                                                                    <td colspan="2" align="right">
                                                                        <asp:Button ID="btnAddAddress" runat="server" ValidationGroup="Baddress" CssClass="styleSubmitShortButton"
                                                                            Text="Add" OnClick="btnAddAddress_Click" />
                                                                        <asp:Button ID="btnModifyAddress" runat="server" CssClass="styleSubmitShortButton" OnClick="btnModifyAddress_Click"
                                                                            Text="Modify" ValidationGroup="Baddress" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>

                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2" style="width: 100%;">
                                                        <asp:GridView ID="gvBAddress" runat="server" AutoGenerateColumns="False"
                                                            EmptyDataText="No Statewise Address Found!..." OnRowDataBound="gvBAddress_RowDataBound"
                                                            OnRowDeleting="gvBAddress_RowDeleting" Width="100%" OnSelectedIndexChanged="gvBAddress_SelectedIndexChanged">
                                                            <Columns>
                                                                <asp:CommandField ShowSelectButton="True" Visible="false" />
                                                                <asp:TemplateField HeaderText="State">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblState" runat="server" Text='<%#Bind("LocationCat_Description") %>'></asp:Label>
                                                                        <asp:Label ID="lblStateCode" Visible="false" runat="server" Text='<%#Bind("Location_Category_ID") %>'></asp:Label>
                                                                        <asp:Label ID="lblRowNo" runat="server" Visible="false" Text='<%#Bind("Rowno") %>'></asp:Label>

                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="City">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Bind("City") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Address 1">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddress1" runat="server" Text='<%#Bind("Address1") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Address 2">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddress2" runat="server" Text='<%#Bind("Address2") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PIN">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPIN" runat="server" Text='<%#Bind("PINCODE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="EmailID">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEmailID" runat="server" Text='<%#Bind("Email") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Contact Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactName" runat="server" Text='<%#Bind("Contact_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Contact No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblContactNo" runat="server" Text='<%#Bind("Contact_No") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TIN">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgTIN" runat="server" Text='<%#Bind("TIN") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="TAN">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgTAN" runat="server" Text='<%#Bind("TAN") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="GSTIN">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSGSTIN" runat="server" Text='<%#Bind("SGSTIN") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="GST Reg. Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSGSTRegDate" runat="server" Text='<%#Bind("SGST_Reg_Date") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomerType" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtIndustryName" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Personal Details" ID="tpPersonal" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Personal Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upPersonal" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel GroupingText="Non Individual Customer" ID="pnlNonIndividual" runat="server"
                                                CssClass="stylePanel">
                                                <table width="99%" cellspacing="0">
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblPublic" Text="Public/Closely held"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:DropDownList ID="ddlPublic" runat="server" Width="60%">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ValidationGroup="Personal Details" ID="rfvPublic" runat="server" Enabled="false"
                                                                ControlToValidate="ddlPublic" CssClass="styleMandatoryLabel" Display="None" InitialValue="-1"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel" width="25%">
                                                            <asp:Label runat="server" ID="lblDirectors" Text="No.of Directors/Partners"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtDirectors" runat="server" Width="15%" MaxLength="2" Style="text-align: right;"
                                                                onblur="ChkIsZero(this, 'No.of Directors/Partners')"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtDirectors" runat="server" ValidChars="" FilterType="Numbers"
                                                                Enabled="true" TargetControlID="txtDirectors">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ValidationGroup="Personal Details" ID="rfvDirectors" Enabled="false"
                                                                runat="server" ControlToValidate="txtDirectors" CssClass="styleMandatoryLabel"
                                                                Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblStockExchange" CssClass="styleDisplayLabel" Text="Listed Exchange"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtSE" runat="server" Width="145px" MaxLength="40"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel" width="25%">
                                                            <asp:Label runat="server" ID="lblPaidCapital" CssClass="styleDisplayLabel" Text="Paid Up Capital"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtPaidCapital" runat="server" Width="45%" MaxLength="11" Style="text-align: right;"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtPaidCapital" runat="server" ValidChars="" FilterType="Numbers"
                                                                Enabled="true" TargetControlID="txtPaidCapital">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblfacevalue" CssClass="styleDisplayLabel" Text="Face Value of Shares"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtfacevalue" runat="server" Width="35%" MaxLength="7" Style="text-align: right;"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtfacevalue" runat="server" ValidChars="" FilterType="Numbers"
                                                                Enabled="true" TargetControlID="txtfacevalue">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                        <td class="styleFieldLabel" width="25%">
                                                            <asp:Label runat="server" ID="lblbookvalue" CssClass="styleDisplayLabel" Text="Book Value of Shares"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtbookvalue" runat="server" Width="30%" MaxLength="7" Style="text-align: right;"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtbookvalue" runat="server" ValidChars="" FilterType="Numbers"
                                                                Enabled="true" TargetControlID="txtbookvalue">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblBusinessProfile" CssClass="styleDisplayLabel" Text="Business Profile"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="25%">
                                                            <asp:TextBox ID="txtBusinessProfile" runat="server" Width="145px" MaxLength="240"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtBusinessProfile" runat="server" ValidChars=" .~`!@#$%^&*()_-+=[]{};':<>,?/"
                                                                FilterType="Custom,LowercaseLetters,UppercaseLetters,Numbers" Enabled="true"
                                                                TargetControlID="txtBusinessProfile">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ValidationGroup="Personal Details" ID="rfvBusinessProfile" Enabled="false"
                                                                runat="server" ControlToValidate="txtBusinessProfile" CssClass="styleMandatoryLabel"
                                                                Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblStake" CssClass="styleDisplayLabel" Text="Promoters Stake %"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtPercentageStake" runat="server" Width="25%" Style="text-align: right;"
                                                                MaxLength="6" onkeypress="fnAllowNumbersOnly(true,false,this);"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ValidationGroup="Personal Details" ID="rfvPercentageStake"
                                                        runat="server" ControlToValidate="txtPercentageStake" CssClass="styleMandatoryLabel"
                                                        Display="None"></asp:RequiredFieldValidator>--%>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="25%">
                                                            <asp:Label runat="server" ID="lblCEO" Text="CEO Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" width="20%">
                                                            <asp:TextBox ID="txtCEOName" runat="server" Width="210px" MaxLength="50"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ValidationGroup="Personal Details" ID="rfvCEOName" runat="server"
                                                                ControlToValidate="txtCEOName" CssClass="styleMandatoryLabel" Display="None" Enabled="false"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tbBankDetails" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Bank Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <table cellpadding="0" id="tblBankDetails" runat="server" border="0" cellspacing="0"
                                                style="width: 99%">
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblAccountType" CssClass="styleReqFieldLabel" Text="Account Type"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlAccountType" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ValidationGroup="Add" ID="rfvAccountType" runat="server"
                                                            ControlToValidate="ddlAccountType" CssClass="styleMandatoryLabel" Display="None"
                                                            InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblMICRCode" Text="MICR Code"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMICRCode" runat="server" Width="45%" MaxLength="10" onblur="jsMICRvaildate(this);"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftxtMICRCode" runat="server" ValidChars=" ." FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            Enabled="True" TargetControlID="txtMICRCode">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ValidationGroup="Add" ID="rfvMICRCode" runat="server" Enabled="false"
                                                            ControlToValidate="txtMICRCode" CssClass="styleMandatoryLabel" Display="None"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblAccountNumber" CssClass="styleReqFieldLabel" Text="Account Number"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="16"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftxtAccountNumber" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            Enabled="True" TargetControlID="txtAccountNumber">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ValidationGroup="Add" ID="rfvAccountNumber" runat="server"
                                                            ControlToValidate="txtAccountNumber" CssClass="styleMandatoryLabel" Display="None"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblIFSC_Code" CssClass="styleReqFieldLabel" Text="IFSC Code"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtIFSC_Code" runat="server" Width="45%" MaxLength="11"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTEtxtIFSC_Code" runat="server" ValidChars=" ." FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            Enabled="True" TargetControlID="txtIFSC_Code">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ValidationGroup="Add" ID="RFVtxtIFSC_Code" runat="server"
                                                            ControlToValidate="txtIFSC_Code" CssClass="styleMandatoryLabel" Display="None"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblBankName" CssClass="styleReqFieldLabel" Text="Bank Name"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtBankName" runat="server" MaxLength="40" Width="40%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ValidationGroup="Add" ID="rfvBankName" runat="server"
                                                            ControlToValidate="txtBankName" CssClass="styleMandatoryLabel" Display="None"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblPANum" CssClass="styleDisplayLabel" Text="Rental Schedule No"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlRSNum" runat="server" Width="30%" AutoPostBack="true" OnSelectedIndexChanged="ddlRSNum_OnSelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <%--<asp:RequiredFieldValidator ValidationGroup="Add" ID="RFVddlPANum" runat="server"
                                                    ControlToValidate="ddlPANum" CssClass="styleMandatoryLabel" Display="None"
                                                    InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblBankBranch" CssClass="styleReqFieldLabel" Text="Bank Branch"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtBankBranch" runat="server" MaxLength="40" Width="40%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftxtBankBranch" runat="server" ValidChars=" ." FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            Enabled="True" TargetControlID="txtBankBranch">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ValidationGroup="Add" ID="rfvBankBranch" runat="server"
                                                            ControlToValidate="txtBankBranch" CssClass="styleMandatoryLabel" Display="None"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblSAnum" CssClass="styleDisplayLabel" Text="Tranche No"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlSANum" runat="server" Width="30%">
                                                        </asp:DropDownList>
                                                        <%-- <asp:RequiredFieldValidator ValidationGroup="Add" ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="ddlPANum" CssClass="styleMandatoryLabel" Display="None"
                                                    InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblBankAddress" CssClass="styleReqFieldLabel" Text="Bank Address"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" colspan="2">
                                                        <asp:TextBox ID="txtBankAddress" runat="server" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftxtBankAddress" runat="server" ValidChars="  :;!@$%^&*_-'.#,/(`)?+<>\~[]{}=|"
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                            TargetControlID="txtBankAddress">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ValidationGroup="Add" ID="rfvBankAddress" runat="server"
                                                            ControlToValidate="txtBankAddress" CssClass="styleMandatoryLabel" Display="None"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:CheckBox runat="server" ID="chkDefaultAccount" CssClass="styleDisplayLabel"
                                                            Text="Default Account" />
                                                        &nbsp;
                                                <asp:Button ID="btnAdd" runat="server" CssClass="styleSubmitShortButton" OnClick="btnAdd_Click"
                                                    OnClientClick="return fnCheckPageValidators('Add','f');" Text="Add" ValidationGroup="Add" />
                                                        <asp:Button ID="btnModify" runat="server" CssClass="styleSubmitShortButton" OnClick="btnModify_Click"
                                                            OnClientClick="return fnCheckPageValidators('Add','f');" Text="Modify" ValidationGroup="Add" />
                                                        <asp:Button ID="btnBnkClear" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                            Text="Clear" OnClick="btnBnkClear_Click" />
                                                        <input id="hdnBankId" runat="server" type="hidden"></input>
                                                        <input id="hdnAddID" runat="server" type="hidden"></input>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" width="80%">
                                                        <div style="overflow: auto; width: 98%;">
                                                            <br />
                                                            <asp:GridView ID="grvBankDetails" runat="server" AutoGenerateColumns="False"
                                                                EmptyDataText="No Bank Details Found!..." OnRowDataBound="grvBankDetails_RowDataBound"
                                                                OnRowDeleting="grvBankDetails_RowDeleting" OnSelectedIndexChanged="grvBankDetails_SelectedIndexChanged"
                                                                Width="100%">
                                                                <Columns>
                                                                    <asp:CommandField ShowSelectButton="True" Visible="false" />
                                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBankMappingId" runat="server" Text='<%#Bind("Customer_Bank_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <%--<asp:TemplateField HeaderText="Entity ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMasterId" runat="server" Text='<%#Bind("Master_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>--%>
                                                                    <asp:TemplateField HeaderText="Account Type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountType" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="AccountType_ID" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountTypeId" runat="server" Text='<%#Bind("ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Account_Number" HeaderText="Account Number">
                                                                        <ItemStyle Width="5%" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="IFSC_Code" HeaderText="IFSC Code">
                                                                        <ItemStyle Width="10%" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <%--<asp:BoundField DataField="PANum" HeaderText="Contract Number">
                                                                <ItemStyle Width="10%" Wrap="True" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SANum" HeaderText="Sub Contract Number">
                                                                <ItemStyle Width="10%" Wrap="True" />
                                                            </asp:BoundField>--%>
                                                                    <asp:BoundField DataField="MICR_Code" HeaderText="MICR Code">
                                                                        <ItemStyle Width="10%" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Bank_Name" HeaderText="Bank Name">
                                                                        <ItemStyle Width="10%" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Branch_Name" HeaderText="Bank Branch">
                                                                        <ItemStyle Width="10%" Wrap="True" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Bank Address">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="lblgvBankAddress" Enabled="false" runat="server" Text='<%#Bind("Bank_Address") %>'
                                                                                TextMode="MultiLine"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rental Schedule Number">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRS" runat="server" Text='<%#Bind("PANUM") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tranche NO">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTranche" runat="server" Text='<%#Bind("Tranche_Name") %>'></asp:Label>
                                                                            <asp:Label ID="lblTrancheID" Visible="false" runat="server" Text='<%#Bind("Tranche_Header_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Default Account">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkgvDefaultAccount" Enabled="false" runat="server" />
                                                                            <asp:Label ID="lblDefAccount" Visible="false" runat="server" Text='<%#Bind("Is_Default_Account") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" Text="Remove"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <br />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="Bank" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" ValidationGroup="Add" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabConstitution" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Constitution Document Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upConstitution" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div id="div2" style="overflow: auto; width: 98%; padding-left: 10px;" runat="server"
                                                border="1">
                                                <br />
                                                <asp:GridView ID="gvConstitutionalDocuments" runat="server" AutoGenerateColumns="False"
                                                    Width="100%" OnRowDataBound="gvConstitutionalDocuments_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                                        <asp:BoundField DataField="DocumentFlag" HeaderText="Document Flag">
                                                            <HeaderStyle Width="5%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DocumentDescription" HeaderText="Document Description">
                                                            <HeaderStyle Width="35%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td colspan="2">Mandatory</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">Doc</td>
                                                                        <td align="right">Image Copy</td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td align="left"><%--<%# Bind("IsMandatory") %>--%>
                                                                            <asp:CheckBox ID="chkIsMandatory" runat="server" Enabled="false" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IsMandatory")))%>'></asp:CheckBox></td>
                                                                        <td align="right"><%--<%# Bind("IsNeedImageCopy") %>--%>
                                                                            <asp:CheckBox ID="chkIsNeedImageCopy" runat="server" Enabled="false" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IsNeedImageCopy")))%>'></asp:CheckBox></td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Remark" HeaderText="Remarks">
                                                            <HeaderStyle Width="35%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Collected">
                                                            <ItemTemplate>
                                                                <%--<%# Bind("Collected") %>--%>
                                                                <asp:CheckBox ID="chkCollected" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Collected")))%>'></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Values">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtValues" ValidationGroup="Customer" runat="server" MaxLength="30"
                                                                    Text='<%# Bind("Values") %>'></asp:TextBox>

                                                            </ItemTemplate>
                                                            <HeaderStyle Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Scanned">
                                                            <ItemTemplate>
                                                                <%--<%# Bind("Scanned") %>--%>
                                                                <asp:CheckBox ID="chkScanned" AutoPostBack="true" OnCheckedChanged="chkScanned_CheckedChanged" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Scanned")))%>'></asp:CheckBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <%-- <asp:TemplateField HeaderText="File Upload" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txOD" runat="server" Width="100px" MaxLength="500" Text='<%# Bind("Document_Path") %>'
                                                                    Visible="false"></asp:TextBox><cc1:AsyncFileUpload ID="asyFileUpload" runat="server"
                                                                        Width="175px" OnClientUploadComplete="uploadComplete" OnUploadedComplete="asyncFileUpload_UploadedComplete" />
                                                                <asp:Label runat="server" ID="myThrobber" Visible="false" Text='<%# Bind("Document_Path") %>'></asp:Label><asp:HiddenField
                                                                    ID="hidThrobber" runat="server" />
                                                                <asp:TextBox ID="txthidden" runat="server" Width="100px" MaxLength="500" Text='<%# Bind("Document_Path") %>'
                                                                    Visible="false"></asp:TextBox><input id="bOD" onclick="return openFileDialog(this.id, 'bOD', 'fileOpenDocument', 'txOD', 'paper')"
                                                                        style="width: 17px; height: 22px" type="button" runat="server" title="Click to browse file"
                                                                        value="..." visible="False" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="File Upload">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActualPath" runat="server" Visible="false" Text='<%# Bind("Document_Path") %>'></asp:Label>
                                                                <asp:UpdatePanel ID="tempUpdate" runat="server" UpdateMode="Conditional">
                                                                    <ContentTemplate>

                                                                        <asp:Button ID="btnBrowse" runat="server" OnClick="btnBrowse_OnClick" Style="display: none"></asp:Button>
                                                                        <asp:FileUpload runat="server" ID="flUpload" Width="150px" ToolTip="Upload File" />
                                                                        <asp:TextBox ID="txtFileupld" runat="server" Style="position: absolute; margin-left: -153px; width: 65px" ReadOnly="true" />

                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="btnBrowse" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="150px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <%--<%# Bind("IsNeedImageCopy") %>--%>
                                                                <asp:LinkButton ID="hlnkView" runat="server" OnClick="hlnkView_Click" Text="View"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="5%" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="CategoryID" HeaderText="CategoryID" Visible="false" />
                                                    </Columns>
                                                </asp:GridView>
                                                <br />
                                            </div>
                                        </ContentTemplate>
                                        <%-- <Triggers>
                                        <asp:AsyncPostBackTrigger  ControlID="btnSave" EventName="Click" />
                                   </Triggers>--%>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabCustomerTrack" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Customer Track
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <%--<td style="width: 30%;" valign="top"></td>--%>
                                            <td valign="top" align="center" colspan="2">
                                                <asp:CheckBox runat="server" ID="chkBadTrack" Text="Black Listed" TextAlign="Left"
                                                    Font-Bold="true" Font-Size="Larger" /> &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox runat="server" ID="chkHotListed" Text="Hot Listed" TextAlign="Left"
                                                    Font-Bold="true" Font-Size="Larger" /> &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox runat="server" ID="chkPOBlack" Text="PO Block" TextAlign="Left"
                                                    Font-Bold="true" Font-Size="Larger" /> &nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox runat="server" ID="chkManualNum" Text="Is Manual Numbering" TextAlign="Left" 
                                                    Font-Bold="true" Font-Size="Larger" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%;" valign="top">
                                                <asp:Label ID="lblHotListReason" runat="server" CssClass="styleDisplayLabel" Text="Reason"></asp:Label>
                                            </td>
                                            <td valign="top">
                                                <asp:TextBox ID="txtReson" runat="server" Width="350px" MaxLength="200" TextMode="MultiLine" Height="60px"
                                                    onkeyup="maxlengthfortxt(200);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 30%;" valign="top">
                                                <asp:Label ID="lblCustomerRating" runat="server" Text="Customer Rating" CssClass="styleDisplayLabel" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustomerRating" runat="server" Width="350px" MaxLength="300" TextMode="MultiLine" Height="60px"
                                                    onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblNoCustomerTrack" CssClass="styleDisplayLabel" runat="server" Text="No Customer Track Details Found!..."
                                        Visible="false"></asp:Label>
                                    <div visible="false" id="divTrack" style="overflow: auto; width: 98%; padding-left: 1%;"
                                        runat="server" border="1">
                                        <br />
                                        <asp:GridView ID="gvTrack" Visible="false" AutoGenerateColumns="false" HorizontalAlign="Center" ShowFooter="true"
                                            runat="server" Width="100%" OnRowCreated="gvTrack_RowCreated" OnRowDataBound="gvTrack_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField Visible="false" HeaderText="Lob Id">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLobId" runat="server" Text='<%#Bind("LOB_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Line of Business">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgvLOBName" runat="server" Text='<%#Bind("LOB_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlLOBTrack" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" HeaderText="PA SA REF ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPASAREFId" runat="server" Text='<%#Bind("PA_SA_REF_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccountNo" runat="server" Text='<%#Bind("ACCOUNTNO") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlAccountNo" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Sub Account No.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAccountNo" runat="server" Text='<%#Bind("SANUM") %>'></asp:Label></ItemTemplate><FooterTemplate>
                                                    <asp:DropDownList ID="ddlAccountNo" runat="server">
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>--%>
                                                <asp:TemplateField Visible="false" HeaderText="TRACKTYPEID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrackTypeId" runat="server" Text='<%#Bind("TRACK_TYPE_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTypeId" runat="server" Text='<%#Bind("TRACK_TYPE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlType" runat="server">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%#Bind("DATE")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDate" runat="server" Width="128px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="ftbDate" runat="server" ValidChars="/-" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                            Enabled="true" TargetControlID="txtDate">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <cc1:CalendarExtender runat="server" TargetControlID="txtDate" ID="CalendarDate"
                                                            Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                        </cc1:CalendarExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reason">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReason" runat="server" Text='<%#Bind("REASON") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtReason" runat="server" Width="140px" MaxLength="100" TextMode="MultiLine"
                                                            onkeyup="maxlengthfortxt(100)"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="LoginBy" HeaderText="Login By" />
                                                <asp:TemplateField HeaderText="Release Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReleaseDate" runat="server" Text='<%#Bind("RELEASEDATE")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtReleaseDate" runat="server" Width="128px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="ftbReleaseDate" runat="server" ValidChars="/-" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                            Enabled="true" TargetControlID="txtReleaseDate">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <cc1:CalendarExtender runat="server" TargetControlID="txtReleaseDate" ID="CalendarReleaseDate"
                                                            Enabled="True" OnClientDateSelectionChanged="checkDate_PrevSystemDate">
                                                        </cc1:CalendarExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Released By Id" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReleasedBy" runat="server" Text='<%#Bind("RELEASED_BY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ReleasedBy" HeaderText="Released By" />
                                                <asp:TemplateField ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Track"
                                                            CssClass="styleSubmitButton" OnClick="Track_AddRow_OnClick" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabCreditDetails" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Customer Credit Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <%--<tr>
                                                <td style="padding-top: 10px">
                                                    <table class="styleGridView" width="100%" cellspacing="0">
                                                        <tr>
                                                            <th class="stylePagingControl" align="left">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td width="160px" align="center">
                                                                            <asp:Label ID="Label2" runat="server" Text="Proposal No" />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="Label4" runat="server" Text="Proposal Date" />
                                                                        </td>
                                                                        <td width="100px" align="center">
                                                                            <asp:Label ID="Label5" runat="server" Text="Proposal Amt" />
                                                                        </td>
                                                                        <td width="100px" align="center">
                                                                            <asp:Label ID="Label3" runat="server" Text="Proposal Expiry Dt" />
                                                                        </td>
                                                                        <td width="100px" align="center">
                                                                            <asp:Label ID="Label6" runat="server" Text="Rental Sch Ulti Amt" />
                                                                        </td>
                                                                        <td width="100px" align="center">
                                                                            <asp:Label ID="Label1" runat="server" Text="Bal Limit" />
                                                                        </td>
                                                                        <td width="100px" align="center">
                                                                            <asp:Label ID="Label7" runat="server" Text="Deactivate" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </th>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>--%>
                                        <tr>
                                            <td style="padding-top: 10px">
                                                <div style="overflow: auto; width: 98%;">
                                                    <br />
                                                    <asp:GridView ID="grvFacilityGroup" runat="server" AutoGenerateColumns="false"
                                                        OnRowDataBound="grvFacilityGroup_RowDataBound" EmptyDataText="No Records Found!..." Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Proposal No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="12%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLOB" runat="server" Text='<%#Eval("BUSINESS_OFFER_NUMBER")%>' />
                                                                    <asp:Label ID="lblPricingID" runat="server" Visible="false" Text='<%#Eval("PRICING_ID")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Category" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="9%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssCategory" runat="server" Text='<%#Eval("ASSETCATEGORY")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Proposal Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="14%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("OFFERDATE")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Facility Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFacilityAmount" runat="server" Text='<%#Eval("FACILITY_AMOUNT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFacilityAmount1" runat="server" Text=""></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Proposal Expiry Dt" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSanctionedAmount" runat="server" Text='<%#Eval("OFFERVALIDTILL")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Utilised Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApprovedAmount" runat="server" Text='<%#Eval("SCHEDULED_AMOUNT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Bal Limit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUtilizedAmount" runat="server" Text='<%#Eval("BAL_LIMIT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFacilityAmount2" runat="server" Text=""></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Deactivate" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeactive" runat="server" Visible="false" Text='<%#Eval("IS_ACTIVE")%>'></asp:Label>
                                                                    <asp:CheckBox ID="chkDeactivate" AutoPostBack="true" OnCheckedChanged="chkDeactivate_CheckedChanged" runat="Server"></asp:CheckBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <uc1:pagenavigator id="ucCustomPaging" runat="server" />
                                                    <br />
                                                </div>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="center">
                                                <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium"></span>
                                                <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatory"></asp:Label--%>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="height: 10px; border: none"></div>


                                    <asp:Label ID="lblNoCreditDetails" CssClass="styleDisplayLabel" runat="server" Text="No Records Found!..."
                                        Visible="False"></asp:Label>
                                    <asp:Panel Visible="false" ID="Panel1" runat="server">
                                        <asp:GridView ID="gvCredit" HorizontalAlign="Center" EmptyDataText="No Customer Credit Details Found!..."
                                            OnRowDataBound="gvCredit_RowDataBound" runat="server" Width="100%">
                                        </asp:GridView>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlCustomerCreditDetails" runat="server" Visible="False">
                                        <table cellpadding="0" cellspacing="0" style="width: 99%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblLOBName" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLOBName" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblFinanceAmount" CssClass="styleDisplayLabel" Style="text-align: right;"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" align="right">
                                                    <asp:TextBox ID="txtFinanceAmount" runat="server" Style="text-align: right;" Width="200px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" align="right">
                                                    <asp:Label runat="server" ID="lblSanctionamt" CssClass="styleDisplayLabel" Style="text-align: right;"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" align="right">
                                                    <asp:TextBox ID="txtSanctionamt" runat="server" Width="200px" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblValid" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtValidupto" runat="server" Width="178px" ReadOnly="True"></asp:TextBox><asp:Image
                                                        ID="imgValidupto" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender runat="server" TargetControlID="txtValidupto" PopupButtonID="imgValidupto"
                                                        ID="CalendarValidupto" Enabled="True" OnClientDateSelectionChanged="checkDate_PrevSystemDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr align="right">
                                                <td class="styleFieldLabel" align="right">
                                                    <asp:Label runat="server" ID="lblUtilizedAmt" CssClass="styleDisplayLabel" Style="text-align: right;"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" align="right">
                                                    <asp:TextBox ID="txtUtilizedAmt" runat="server" Width="200px" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" align="right">
                                                    <asp:Label runat="server" ID="lblRemainingAmount" CssClass="styleDisplayLabel" Style="text-align: right;"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" align="right">
                                                    <asp:TextBox ID="txtRemainingAmount" runat="server" Width="200px" ReadOnly="True"
                                                        Style="text-align: right;"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblFacilityType" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFacilityType" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>

                            <cc1:TabPanel runat="server" ID="tabPODOMappings" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    PO / DO Mappings
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table style="width: 99%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="lblReferenceType" CssClass="styleReqFieldLabel" Text="Reference Type"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlReferenceType" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="PO" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="DO" Value="2"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvReferenceType" runat="server" ControlToValidate="ddlReferenceType"
                                                    Display="None" ErrorMessage="Select Reference Type" ValidationGroup="Accounts" InitialValue="0"></asp:RequiredFieldValidator>

                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="lblReferenceDate" Text="Reference Date"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtReferenceDate" runat="server"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="fteReferenceDate" runat="server" ValidChars="/-" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                    Enabled="true" TargetControlID="txtReferenceDate">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:Image ID="imgReferenceDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <cc1:CalendarExtender runat="server" TargetControlID="txtReferenceDate" PopupButtonID="imgReferenceDate"
                                                    ID="ceReferenceDate" Enabled="True">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvReferenceDate" Enabled="false" runat="server" ControlToValidate="txtReferenceDate"
                                                    Display="None" ErrorMessage="Enter Reference Date" ValidationGroup="Accounts"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="lblReferenceNumber" Text="Reference Number" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtReferenceNumber" runat="server" MaxLength="50"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvReferenceNumber" runat="server" ControlToValidate="txtReferenceNumber"
                                                    Display="None" ErrorMessage="Enter Reference Number" ValidationGroup="Accounts"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="lblValidFrom" Text="Due Date From" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtValidfrom" runat="server"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="fteValidFrom" runat="server" ValidChars="/-" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                    Enabled="true" TargetControlID="txtValidfrom">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:Image ID="imgValidFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <cc1:CalendarExtender runat="server" TargetControlID="txtValidfrom" PopupButtonID="imgValidFrom"
                                                    ID="ceValidFrom" Enabled="True">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvValidFrom" runat="server" ControlToValidate="txtValidfrom"
                                                    Display="None" ErrorMessage="Enter Due Date From" ValidationGroup="Accounts"></asp:RequiredFieldValidator>


                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="lblValidTo" Text="Due Date To" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtValidTo" runat="server"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="fteValidTo" runat="server" ValidChars="/-" FilterType="Custom,UppercaseLetters,LowercaseLetters,Numbers"
                                                    Enabled="true" TargetControlID="txtValidTo">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:Image ID="imgValidTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <cc1:CalendarExtender runat="server" TargetControlID="txtValidTo" PopupButtonID="imgValidTo"
                                                    ID="ceValidTo" Enabled="True">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvValidTo" runat="server" ControlToValidate="txtValidTo"
                                                    Display="None" ErrorMessage="Enter Due Date To" ValidationGroup="Accounts"></asp:RequiredFieldValidator>


                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="lblCashflow" CssClass="styleReqFieldLabel" Text="CashFlow Type"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlCashflow" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCashflow" runat="server" ControlToValidate="ddlCashflow"
                                                    Display="None" ErrorMessage="Select Cashflow" ValidationGroup="Accounts" InitialValue="0"></asp:RequiredFieldValidator>

                                            </td>
                                        </tr>

                                        <br />

                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:Button ID="btnReferenceNumberGo" runat="server" CssClass="styleSubmitButton" OnClick="btnReferenceNumberGo_Click"
                                                    Text="Go" CausesValidation="true" ValidationGroup="Accounts" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:Panel GroupingText="Accounts" ID="pnlAccounts" runat="server" CssClass="stylePanel" Width="99%" Visible="false">
                                                    <div id="Div1" runat="server" style="overflow-y: scroll; overflow-x: hidden; max-height: 200px; height: auto">
                                                        <asp:GridView ID="grvAccounts" runat="server" AutoGenerateColumns="False"
                                                            EnableModelValidation="True" Width="99%" OnRowDataBound="grvAccounts_RowDataBound" DataKeyNames="PA_SA_REF_ID">
                                                            <Columns>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Tranche Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTranche_Name" ToolTip="Tranche Name" Text='<%#Bind("Tranche") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Account Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRS_Number" ToolTip="Account Number" Text='<%#Bind("PANum") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField SortExpression="Select">
                                                                    <HeaderTemplate>
                                                                        <table align="center">
                                                                            <tr>
                                                                                <td>Select All
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="center">
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Select Account" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <asp:Button ID="btnAddAccounts" runat="server" CssClass="styleSubmitButton" OnClick="btnAddAccounts_Click"
                                                        Text="Add" CausesValidation="true" ValidationGroup="Accounts" Visible="false" />
                                                </asp:Panel>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td colspan="6" align="center">
                                                <asp:Panel GroupingText="History" ID="pnlHistory" runat="server" CssClass="stylePanel" Width="99%">
                                                    <div id="Div3" runat="server" style="overflow-y: scroll; overflow-x: hidden; max-height: 200px; height: auto">
                                                        <asp:GridView ID="grvHistory" runat="server" AutoGenerateColumns="False"
                                                            EnableModelValidation="True" Width="99%" DataKeyNames="InvoiceRef_ID">
                                                            <Columns>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Reference Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRefType" ToolTip="Reference Type" Text='<%#Bind("RefType") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Reference Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRef_Number" ToolTip="Reference Number" Text='<%#Bind("RefNumber") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="CashFlow Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCashflowType" ToolTip="CashFlow Type" Text='<%#Bind("CashflowType") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Reference Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRefDate" ToolTip="Reference Date" Text='<%#Bind("RefDate") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Transaction Date" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTransDate" ToolTip="Transaction Date" Text='<%#Bind("TransDate") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid From">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblValidFrom" ToolTip="Due Date From" Text='<%#Bind("ValidFrom") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Valid To">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblValidTo" ToolTip="Due Date To" Text='<%#Bind("ValidTo") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Defined On">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddedOn" ToolTip="Added On" Text='<%#Bind("Added_On") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="View">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkView" Text="View" runat="server" OnClick="lnkView_Click"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Modify">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkModify" Text="Modify" runat="server" OnClick="lnkModify_Click"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="slno" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSlNo" ToolTip="Valid To" Text='<%#Bind("SlNo") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td width="99%" colspan="4">
                                                <asp:Panel ID="pnlHistoryinDetail" runat="server" GroupingText="History in Detail" CssClass="stylePanel" Visible="false">
                                                    <div id="Div4" runat="server" style="overflow-y: scroll; overflow-x: hidden; max-height: 200px; height: auto">

                                                        <asp:GridView ID="gvHistoryindetail" runat="server" AutoGenerateColumns="false"
                                                            RowStyle-HorizontalAlign="Center" Width="100%" ShowFooter="false">
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="SL.No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSlNo" ToolTip="SL.No" Text='<%#Container.DataItemIndex+1 %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Tranche Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTranche_Name" ToolTip="Tranche Name" Text='<%#Bind("Tranche") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Account Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRS_Number" ToolTip="Account Number" Text='<%#Bind("PANum") %>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <asp:HiddenField runat="server" ID="hdnInvoiceRef_ID" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdnSlNo" Value="0" />

                                    </table>

                                </ContentTemplate>
                            </cc1:TabPanel>

                            
                            <cc1:TabPanel runat="server" ID="tabEmailMap" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Email Mappings
                                </HeaderTemplate>
                                <ContentTemplate>
                                     <table style="width: 40%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="Label1" CssClass="styleReqFieldLabel" Text="Billing Email Type"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" align="left" " style="vertical-align:middle">
                                                <asp:DropDownList ID="ddlBillEmailType" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlBillEmailType_OnSelectedIndexChanged">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Single" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Multiple ( Tranche wise )" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Multiple ( State wise )" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                              </td>
                                            <%--<td class="styleFieldLabel">
                                                <asp:Label runat="server" CssClass="styleReqFieldLabel" ID="lblEMailIDSingle" Text="Email ID"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign"" style="vertical-align:middle">
                                                <asp:TextBox ID="txtEMailIDSingle" Width="250px"   TextMode="MultiLine" Height="50px" MaxLength="5000"  runat="server"></asp:TextBox>
                                                
                                                <asp:RequiredFieldValidator ID="rfvEMailIDSingle" Enabled="false" runat="server" ControlToValidate="txtEMailIDSingle"
                                                    Display="None" ErrorMessage="Enter Email ID"></asp:RequiredFieldValidator>
                                            </td>--%>
                                         </tr>

                                         </table>

                                     <asp:Panel GroupingText="Email Group" Visible="false" ID="pnlEmailGroup" runat="server"  width="100%" 
                                                CssClass="stylePanel">
                                                <table width="75%" cellspacing="0">

                                                    <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblStateGroup" runat="server" CssClass="styleReqFieldLabel" Text="State"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" colspan="3" >
                                                                       
                                                                        <asp:DropDownList ID="ddlStateGroup" runat="server">
                                                                        </asp:DropDownList>
                                                                       
                                                                        <asp:RequiredFieldValidator ID="rfvStateGroup" runat="server" Enabled="true" ControlToValidate="ddlStateGroup"
                                                                            InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                            ValidationGroup="AddGroup"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                
                                                    </tr>
                                                    <tr>
                                                    <td class="styleFieldLabel">
                                                         <asp:Label runat="server" ID="lblGroupName" CssClass="styleReqFieldLabel" Text="Group Name"></asp:Label>
                                                    </td>
                                                        <td class="styleFieldAlign" colspan="3" style="vertical-align:middle">
                                                            <asp:TextBox ID="txtEmailGroupName" runat="server"  MaxLength="100" Width="200px" Style="text-align: left;"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" InvalidChars=".!@#$%^&*()" TargetControlID="txtEmailGroupName"
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server" FilterMode="InvalidChars"
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ValidationGroup="AddGroup" ID="rfvEmailGroupName"
                                                                runat="server" ControlToValidate="txtEmailGroupName" CssClass="styleMandatoryLabel"
                                                                Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                         </td>

                                                        </tr>
                                                    <tr>
                                                    <td class="styleFieldLabel">
                                                         <asp:Label runat="server" ID="lblEmailIDGroup" CssClass="styleReqFieldLabel" Text="Email ID"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" colspan="3">
                                                            <asp:TextBox ID="txtEmailIDGroup" runat="server"  TextMode="MultiLine" Height="50px" MaxLength="5000" Width="600px" Style="text-align: left;"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" InvalidChars="!#$%^&*()" TargetControlID="txtEmailIDGroup"
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server" FilterMode="InvalidChars"
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ValidationGroup="AddGroup" ID="rfvEmailIDGroup"
                                                                runat="server" ControlToValidate="txtEmailIDGroup" CssClass="styleMandatoryLabel"
                                                                Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                         </td>
                                                    </tr>
                                                    <tr>
                                                    <td class="styleFieldLabel">
                                                         <asp:Label runat="server" ID="lblEmailCCGroup" CssClass="styleReqFieldLabel" Text="Email CC"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" colspan="3">
                                                            <asp:TextBox ID="txtEmailCCGroup" runat="server"  TextMode="MultiLine" Height="50px" MaxLength="5000" Width="600px" Style="text-align: left;"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" InvalidChars="!#$%^&*()" TargetControlID="txtEmailCCGroup"
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server" FilterMode="InvalidChars"
                                                                Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ValidationGroup="AddGroup" ID="rfvEmailCCGroup"
                                                                runat="server" ControlToValidate="txtEmailCCGroup" CssClass="styleMandatoryLabel"
                                                                Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                         </td>
                                                    </tr>
                                                    <tr>
                                                       <td colspan="2"></td>
                                                        <td align="left" class="styleFieldLabel">
                                                            <asp:Button ID="btnAddGroup" runat="server" CssClass="styleSubmitShortButton" OnClick="btnAddEmailGroup_Click"
                                                        OnClientClick="return fnCheckPageValidators('AddGroup','f');" Text="Add" ValidationGroup="AddGroup" />
                                                           &nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnModifyGroup" runat="server" CssClass="styleSubmitShortButton" OnClick="btnModifyEmailGroup_Click"
                                                        OnClientClick="return fnCheckPageValidators('AddGroup','f');" Text="Modify" ValidationGroup="AddGroup" />
                                                            <input id="hdnEmailGroupId" runat="server" type="hidden"></input>

                                                            <asp:Button ID="btnClearEmailGroup" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                                                            Text="Clear" OnClientClick="return fnConfirmClearEmailGroup();" OnClick="btnClearEmailGroup_Click" />

                                                        </td>
                                                    </tr></table>

                                           <table width="95%" style="margin-left:10px">
                                                <tr>
                                                    <td align="left" width="95%">
                                                        <div  style="overflow: auto; width: 98%;">
                                                            <br />
                                                            <asp:GridView ID="grvEmailGroup" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvEmailGroup_RowDataBound"
                                                                EmptyDataText="No Email Group Details Found!..." OnRowDeleting="grvEmailGroup_RowDeleting" 
                                                                 OnSelectedIndexChanged="grvEmailGroup_SelectedIndexChanged" Width="100%">
                                                                <Columns>
                                                                    <asp:CommandField ShowSelectButton="True" Visible="false" />
                                                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustEmailDetID" runat="server" Text='<%#Bind("Cust_Email_Det_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                  
                                                                    <asp:TemplateField HeaderText="State Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStateNameGroup" runat="server" Text='<%#Bind("State_Name") %>'></asp:Label>
                                                                            <asp:Label ID="lblStateIDGroup" runat="server" Visible="false" Text='<%#Bind("State_ID") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField HeaderText="Group Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGroupName" runat="server" Text='<%#Bind("Group_Name") %>'></asp:Label>
                                                                         </ItemTemplate>
                                                                        <ItemStyle Width="10%" />
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField HeaderText="Email ID">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmailID" runat="server" Text='<%#Bind("Email_ID") %>'></asp:Label>
                                                                         </ItemTemplate>
                                                                        <ItemStyle Width="70%" />
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField HeaderText="Email CC">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmailCC" runat="server" Text='<%#Bind("Email_CC") %>'></asp:Label>
                                                                         </ItemTemplate>
                                                                        <ItemStyle Width="70%" />
                                                                    </asp:TemplateField>
                                                                   
                                                                    <asp:TemplateField HeaderText="">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkbtnDelete" runat="server" CommandName="Delete" Text="Remove"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <br />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" ValidationGroup="Add" />
                                                    </td>
                                                </tr>
                                            </table>

                                     </asp:Panel>

                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            Text="Save" CausesValidation="true" ValidationGroup="Customer" OnClientClick="return fnCheckPageValidators('Customer')" />
                        <asp:Button ID="btnExportToExcel" OnClick="btnExportToExcel_Click" UseSubmitBehavior="true" 
                            CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Export To Excel" runat="server"></asp:Button>
                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel" OnClick="btnCancel_Click" />
                        <asp:Button ID="btnProceedSave" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Proceed Save" OnClick="btnProceedSave_Click" Style="display: none" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:ValidationSummary ID="vsCustomerMaster" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" Enabled="true" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true"
                            CssClass="styleMandatoryLabel" ShowMessageBox="false" HeaderText="Correct the following validation(s):"
                            ValidationGroup="Customer" />
                        <asp:CustomValidator ID="cvCustomerMaster" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" />
                        <asp:ValidationSummary ID="vsBankAddress" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" ValidationGroup="Bank" />
                        <asp:ValidationSummary ID="vbillingaddress" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" ValidationGroup="Baddress" />
                        <asp:ValidationSummary ID="vsAccounts" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" ValidationGroup="Accounts" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />


        </Triggers>
    </asp:UpdatePanel>
    <asp:TextBox ID="txthiddenfield" runat="server" Visible="false"></asp:TextBox>
    <script
        language="javascript" type="text/javascript">
        var querymode;
        querymode = location.search.split("qsMode=");
        querymode = querymode[1];

        var tab;
        function pageLoad() {

            tab = $find('ctl00_ContentPlaceHolder1_tcCustomerMaster');
            querymode = location.search.split("qsMode=");
            querymode = querymode[1];
            //if (querymode.length > 1) {
            //    querymode = querymode.split("&");
            //    querymode = querymode[0];
            //}

            if (querymode != 'Q' && tab != null) {
                tab.add_activeTabChanged(on_Change);
                var newindex = tab.get_activeTabIndex(index);
                var btnSave = document.getElementById('<%=btnSave.ClientID %>')
                var btnclear = document.getElementById('<%=btnClear.ClientID %>')
                var btnExport = document.getElementById('<%=btnExportToExcel.ClientID %>');
                if (newindex > 2)

                    btnSave.disabled = false;
                else

                    btnSave.disabled = true;

                if (newindex == 5) {
                    btnExport.disabled = false;
                }
                else {
                    btnExport.disabled = true;

                }
            }
        }

        function Calculate(Industry, BusinessProfile) {

            if (Industry.val.length != 0)
                BusinessProfile.value = Industry.value;
        }


        function fnDisableOwnMandatory(ddlOwn) {
            var own = ddlOwn.options.value;
            if (own == "1") {
                //document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_frvCurrentMarketValue').enabled = true;
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_lblCurrentMarketValue').setAttribute("className", "styleReqFieldLabel");
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtCurrentMarketValue').disabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtRemainingLoanValue').disabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtTotNetWorth').disabled = false;



            }
            else if (own == "0") {
                //document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_frvCurrentMarketValue').enabled = false;
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_lblCurrentMarketValue').setAttribute("className", "styleDisplayLabel");
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtCurrentMarketValue').disabled = true;
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtRemainingLoanValue').disabled = true;
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtTotNetWorth').disabled = true;
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtCurrentMarketValue').value = "";
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtRemainingLoanValue').value = "";
                document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tpPersonal_txtTotNetWorth').value = "";
            }

        }
        function jsMICRvaildate(txtMICRCode) {
            if (txtMICRCode.value.length > 0) {
                if (txtMICRCode.value.length < txtMICRCode.maxLength) {
                    alert('Enter a valid MICR Code');
                    document.getElementById(txtMICRCode.id).focus();
                }
            }
        }
        function fnSetEmptyText(txtControl) {
            txtControl.setAttribute("value", "");
            txtControl.setAttribute("readonly", true);
        }




        function fnvalidcustomername(txtCustomerName) {
            if (txtCustomerName.value == "0") {
                alert('Customer Name should not be 0');
                document.getElementById(txtCustomerName.id).focus();
            }
            //Added by saranya for issue raised by vasanth on 03-Mar-2012
            if (txtCustomerName.value.split('  ')[1] != null) {
                alert('Customer Name should not carry more than one space between two words');
                document.getElementById(txtCustomerName.id).value = "";
                document.getElementById(txtCustomerName.id).focus();
            }
            //End Here
        }
        var index = 0;

        function on_Change(sender, e) {

            var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var Valgrp = document.getElementById('<%=vsCustomerMaster.ClientID %>');
            Valgrp.validationGroup = strValgrp;
            var newindex = tab.get_activeTabIndex(index);
            var btnExport = document.getElementById('<%=btnExportToExcel.ClientID %>');
            var btnSave = document.getElementById('<%=btnSave.ClientID %>');
            var btnclear = document.getElementById('<%=btnClear.ClientID %>');


            if (newindex > 2) {
                btnSave.disabled = false;
            }
            else {
                btnSave.disabled = true;

            }
            if (newindex == 5) {
                btnExport.disabled = false;
            }
            else {
                btnExport.disabled = true;

            }
            if (newindex > index) {
                if (!fnCheckPageValidators(strValgrp, false)) {
                    tab.set_activeTabIndex(index);
                    return;
                }
                else {
                    switch (index) {
                        case 0:
                            {
                                var cmbGroupCode = document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tbAddress_cmbGroupCode');
                                var txtGroupName = document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tbAddress_txtGroupName');
                                var chkCustomer = document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tbAddress_chkCustomer');
                                var chkGuarantor1 = document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tbAddress_chkGuarantor1');
                                var chkGuarantor2 = document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tbAddress_chkGuarantor2');
                                var chkCoApplicant = document.getElementById('ctl00_ContentPlaceHolder1_tcCustomerMaster_tbAddress_chkCoApplicant');
                                if ((cmbGroupCode.value != '--Select--' || cmbGroupCode.value != '') && txtGroupName.value != '') {
                                    alert('Enter the Group Name');
                                    tab.set_activeTabIndex(index);
                                }
                                else if ((!chkCustomer.checked) && (!chkGuarantor1.checked) && (!chkGuarantor2.checked) && (!chkCoApplicant.checked)) {
                                    alert('Select atleast one Relation Type(Customer/Guarantor1/Guarantor2/Co-Applicant)');
                                    tab.set_activeTabIndex(index);
                                }
                                else {
                                    index = tab.get_activeTabIndex(index);

                                }
                            }
                            break;
                    }
                    index = tab.get_activeTabIndex(index);
                    return;
                }
            }
            else {
                tab.set_activeTabIndex(newindex);
                index = tab.get_activeTabIndex(newindex);
            }

        }
        function uploadComplete(sender, args) {

            var objID = sender._inputFile.id.split("_");
            objID = "<%= gvConstitutionalDocuments.ClientID %>" + "_" + objID[5];
            if (document.getElementById("ctl00_ContentPlaceHolder1_tcCustomerMaster_TabConstitution_gvConstitutionalDocuments_ctl02_myThrobber") != null) {
                document.getElementById("ctl00_ContentPlaceHolder1_tcCustomerMaster_TabConstitution_gvConstitutionalDocuments_ctl02_myThrobber").innerText = args._fileName;
                document.getElementById("ctl00_ContentPlaceHolder1_tcCustomerMaster_TabConstitution_gvConstitutionalDocuments_ctl02_hidThrobber").value = args._fileName;
            }
        }
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




        //Added by Thangam M on 12/Mar/2012 to fix bug id - 5451
        function fnClearAsyncUploader(rowCount) {
            //            debugger;

            for (var x = 0; x < rowCount; x++) {
                var ctrlName = 'ctl00_ContentPlaceHolder1_tcCustomerMaster_TabConstitution_gvConstitutionalDocuments_ctl';
                if (x.toString().length == 1)
                    ctrlName = ctrlName + '0' + (x + 2).toString();
                else
                    ctrlName = ctrlName + (x + 2).toString();

                ctrlName = ctrlName + '_asyFileUpload'

                var AsyncUploader = $get(ctrlName);
                if (AsyncUploader != null) {
                    var txts = AsyncUploader.getElementsByTagName("input");

                    for (var i = 0; i < txts.length; i++) {
                        if (txts[i].type == "text") {
                            txts[i].value = "";
                        }
                    }
                }
            }
        }

        function fnLoadPath(btnBrowse) {
            if (btnBrowse != null)
                //debugger;
                document.getElementById(btnBrowse).click();
        }

        function fnAssignPath(flUpload, hdnPath, btnBrowse) {
            if (flUpload != null)
                document.getElementById(hdnPath).value = document.getElementById(flUpload).value;
            //debugger;
            document.getElementById(btnBrowse).click();
        }


    </script>
</asp:Content>
