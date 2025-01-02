<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GOrgEntityMaster_Add.aspx.cs" Inherits="Origination_S3GOrgEntityMaster"
    EnableEventValidation="false" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" meta:resourcekey="lblHeadingResource1"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <cc1:TabContainer ID="tcEntityMaster" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                    Width="99%" ScrollBars="Auto" meta:resourcekey="tcEntityMasterResource1">
                    <cc1:TabPanel runat="server" HeaderText="Entity Details" ID="tbEntity" CssClass="tabpan"
                        BackColor="Red" meta:resourcekey="tbEntityResource1">
                        <HeaderTemplate>
                            Vendor
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                <ContentTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0" align="center">
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblResidentialStatus" runat="server" Text="Residential Status" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlVendorResidentialStatus" runat="server" Width="160px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvVendorResStatus" runat="server" ErrorMessage="Select the Residential Status"
                                                    ValidationGroup="Vendor" InitialValue="-1" Display="None" SetFocusOnError="True" ControlToValidate="ddlVendorResidentialStatus"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblEntityCode" runat="server" Text="Vendor Code"
                                                    meta:resourcekey="lblEntityCodeResource1"></asp:Label>
                                                <td class="styleFieldAlign" width="30%">
                                                    <asp:TextBox ID="txtEntityCode" runat="server" ReadOnly="True" MaxLength="12"></asp:TextBox>
                                                </td>
                                            </td>
                                            <%--Changed By Thangam M on 14/Feb/2012 to remove the color of Code--%>
                                        </tr>
                                        <tr>

                                            <td class="styleFieldAlign" width="20%">
                                                <asp:Label ID="lblEntityType" runat="server" Text="Vendor Type" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlEntityType" Width="161px" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvEntitytype" runat="server" ErrorMessage="Select a Vendor Type"
                                                    ValidationGroup="Vendor" Display="None" SetFocusOnError="True" InitialValue="0"
                                                    ControlToValidate="ddlEntityType"></asp:RequiredFieldValidator>
                                                <%--<asp:CheckBox runat="server" ID="chkTradeAdvance" Text="Trade Advance" />--%>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblEntityName" runat="server" Text="Vendor Name" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtEntityName" runat="server" MaxLength="100"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvEntityName" runat="server" ValidationGroup="Vendor" ErrorMessage="Enter Vendor Name"
                                                    Display="None" SetFocusOnError="True" ControlToValidate="txtEntityName"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="ftexEntityName" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtEntityName" ValidChars=" .!@#$%^&*()" FilterMode="ValidChars" runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>

                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblGLPostingCode" runat="server" Text="GL Posting Code" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:RequiredFieldValidator ID="rfvGlCode" runat="server" ErrorMessage="Select a GL Posting Code"
                                                    ValidationGroup="Vendor" Display="None" SetFocusOnError="True" InitialValue="0"
                                                    ControlToValidate="ddlGLPostingCode"></asp:RequiredFieldValidator>
                                                <asp:DropDownList ID="ddlGLPostingCode" Width="161px" runat="server">
                                                </asp:DropDownList>

                                            </td>
                                            <td class="styleFieldLabel" valign="top" width="20%">
                                                <asp:Label runat="server" ID="lblCompanyType" CssClass="styleReqFieldLabel" Text="Company Type"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlCompanyType" runat="server" Width="161px"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCompanyTYpe" runat="server" ErrorMessage="Select a Company Type"
                                                    ValidationGroup="Vendor" Display="None" SetFocusOnError="True" InitialValue="-1"
                                                    ControlToValidate="ddlCompanyType"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblTaxNumber" runat="server" Text="Service Tax Reg. No."></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:UpdatePanel ID="updpan" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtTaxNumber" runat="server" MaxLength="20"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTBEtaxnumber" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            TargetControlID="txtTaxNumber" runat="server" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblPAN" runat="server" Text="PAN" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtPAN" runat="server" MaxLength="20" ToolTip="PAN Number"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvComPAN" runat="server" ControlToValidate="txtPAN" ErrorMessage="Enter the PAN number"
                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                    ValidationGroup="Vendor"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revPAN" runat="server" ErrorMessage="Enter a valid PAN number"
                                                    ControlToValidate="txtPAN" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Vendor"
                                                    ValidationExpression="^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblCIN" runat="server" Text="CIN"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtCIN" runat="server" MaxLength="21" ToolTip="CIN"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCIN" runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblWebsite" runat="server" Text="Website"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtWebsite" runat="server" MaxLength="60"
                                                    Style="text-transform: lowercase;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvWebsite" runat="server" Display="None" SetFocusOnError="True" Enabled="false"
                                                    ValidationGroup="Vendor" ErrorMessage="Enter the Website Name" ControlToValidate="txtWebsite"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revWebsite" runat="server" ControlToValidate="txtWebsite"
                                                    Display="None" SetFocusOnError="True" ValidationGroup="Vendor" ErrorMessage="Enter a valid Website"
                                                    ValidationExpression="(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCGSTIN" runat="server" Text="CGSTIN" Visible="false" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtCGSTin" runat="server" Visible="false" Width="180px" MaxLength="15"></asp:TextBox>

                                            </td>

                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblCGSTREGDate" Visible="false" runat="server" Text="CGST Date"
                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="TxtCGSTRegDate" Visible="false" runat="server" Width="100px"></asp:TextBox>
                                                <asp:Image ID="imgCGSTRegDate" Visible="false" runat="server" ImageUrl="~/Images/calendaer.gif" />

                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="TxtCGSTRegDate"
                                                    PopupButtonID="imgCGSTRegDate"
                                                    ID="CalendarExtender2" Enabled="True">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lblConstitutionName" CssClass="styleReqFieldLabel"
                                                    Text="Constitution Name"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlConstitutionName" runat="server" Width="43%">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ValidationGroup="Vendor" ID="rfvConstitutionName" runat="server"
                                                    ControlToValidate="ddlConstitutionName" CssClass="styleMandatoryLabel" ErrorMessage="Select Constitution" Display="None"
                                                    InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>

                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lblComposition" CssClass="styleReqFieldLabel"
                                                    Text="Composition"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlComposition" runat="server">
                                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ValidationGroup="Vendor" ID="rfvComposition" runat="server" Enabled="false"
                                                    ControlToValidate="ddlComposition" CssClass="styleMandatoryLabel" ErrorMessage="Select Composition" Display="None"
                                                    InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>

                                            <%--<td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lblRegStatus" CssClass="styleReqFieldLabel"
                                                    Text="Registration Status"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlRegStatus" runat="server" Width="43%">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ValidationGroup="Vendor" ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="ddlRegStatus" CssClass="styleMandatoryLabel" ErrorMessage="Select Registration Status" Display="None"
                                                    InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lblMSMERegistered" Text="MSME Registered" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlMSMERegistered" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMSMERegistered_SelectedIndexChanged" >
                                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ValidationGroup="Vendor" ID="RequiredFieldValidator2" runat="server"
                                                    ControlToValidate="ddlMSMERegistered" CssClass="styleMandatoryLabel" ErrorMessage="Select MSME Registered" Display="None"
                                                    InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lblType" Text="Type (Vendor)"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlType" runat="server" Enabled="false">
                                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Micro"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Small"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Medium"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lblCertificateReceived" Text="Certificate Received"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlCertificateReceived" runat="server" Enabled="false">
                                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lbleInvoice" Text="e-Invoicing"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlEInvoice" runat="server">
                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblState" runat="server" Text="State" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <%-- <asp:TextBox ID="txtState" runat="server"  MaxLength="60" meta:resourcekey="txtStateResource1"></asp:TextBox>--%>
                                                <%-- <cc1:ComboBox ID="txtState" runat="server" Width="135px" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend" ValidationGroup="EntityDetails">
                                                </cc1:ComboBox>--%>
                                                <asp:DropDownList ID="ddlComState" runat="server" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlComState_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvtxtState" runat="server" ErrorMessage="Select the State"
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="ddlComState"
                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                <%-- <cc1:FilteredTextBoxExtender ID="ftexState" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtState" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblLoc_Code" runat="server" Text="Location Code"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtLoc_Code" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                                <%--<asp:HiddenField ID="hvEntityAdrz_ID" runat="server" />
                                                <cc1:FilteredTextBoxExtender ID="FTELoc_Code" FilterMode="ValidChars" TargetControlID="txtLoc_Code"
                                                    runat="server" Enabled="True" ValidChars="QWERTYUIOPLKJHGFDSAZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvLoc_Code" runat="server" ErrorMessage="Enter Location Code" 
                                                ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="txtLoc_Code">
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label runat="server" ID="lblRegStatus" CssClass="styleReqFieldLabel"
                                                    Text="State-Wise Registration Status"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlStateRegStatus" runat="server" Width="43%">
                                                </asp:DropDownList>
                                                <asp:Label runat="server" ID="lblShowRegStatus" Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lblShowRegID" Visible="false"></asp:Label>
                                                <asp:RequiredFieldValidator ValidationGroup="EntityDetails" ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="ddlStateRegStatus" CssClass="styleMandatoryLabel" ErrorMessage="Select the State-Wise Registration Status" Display="None"
                                                    InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblStateResidentialStatus" runat="server" Text="State-Wise Residential Status" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlResidentialStatus" runat="server" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddlResidentialStatus_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvResidentialStatus" runat="server" ErrorMessage="Select the State-Wise Residential Status"
                                                    ValidationGroup="EntityDetails" InitialValue="-1" Display="None" SetFocusOnError="True" ControlToValidate="ddlResidentialStatus"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblAddress" runat="server" Text="Address 1" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtAddress" runat="server" MaxLength="60"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ErrorMessage="Enter the Address 1"
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="txtAddress"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblAddress2" runat="server" Text="Address 2"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtAddress2" runat="server" MaxLength="60"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblCity" runat="server" Text="City" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <uc2:Suggest ID="txtCity" Width="157px" runat="server" ServiceMethod="GetCityList" ValidationGroup="EntityDetails"
                                                    IsMandatory="true" ItemToValidate="Text" ErrorMessage="Enter the City" />
                                                <%--<cc1:FilteredTextBoxExtender ID="ftexCity" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCity" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblResState" runat="server" Text="Address State"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlResState" runat="server" Width="160px" AutoPostBack="true" Enabled="false">
                                                </asp:DropDownList>
                                                <%-- <cc1:FilteredTextBoxExtender ID="ftexState" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtState" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblCountry" runat="server" Text="Country" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <cc1:ComboBox ID="txtCountry" runat="server" Width="135px" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                    AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend">
                                                </cc1:ComboBox>
                                                <%--<asp:TextBox ID="txtCountry" runat="server" MaxLength="60" AutoPostBack="True" OnTextChanged="txtCountry_TextChanged"
                                                    meta:resourcekey="txtCountryResource1"></asp:TextBox>--%>
                                                <asp:RequiredFieldValidator ID="rfvtxtCountry" runat="server" ErrorMessage="Select the Country"
                                                    Display="None" SetFocusOnError="True" ControlToValidate="txtCountry" ValidationGroup="EntityDetails"
                                                    InitialValue="-1"></asp:RequiredFieldValidator>
                                                <%--<cc1:FilteredTextBoxExtender ID="fevtexCountry" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCountry" ValidChars=" " runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblPINCode" runat="server" Text="PIN"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtPINCode" runat="server" MaxLength="6"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Numbers" TargetControlID="txtPINCode"
                                                    runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvPIN" runat="server" ErrorMessage="Enter PIN" Enabled="false"
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="txtPINCode"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblVATTIN" runat="server" Text="VAT-TIN"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtVATTIN" runat="server" MaxLength="20"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtVATTIN" runat="server" Enabled="True" ValidChars="~,`,!,@,#,$,%,^,&,*,(,),_,-,=,+,{,[,},]">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvvattin" runat="server" ErrorMessage="Enter VAT-TIN" Enabled="false"
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="txtVATTIN"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblCSTTIN" runat="server" Text="CST-TIN"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtCSTTIN" runat="server" MaxLength="20"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtCSTTIN" runat="server" Enabled="True" ValidChars="~,`,!,@,#,$,%,^,&,*,(,),_,-,=,+,{,[,},]">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvcsttin" runat="server" ErrorMessage="Enter CST-TIN" Enabled="false"
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="txtCSTTIN"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblMobile" runat="server" Text="Mobile No." CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtMobile" runat="server" MaxLength="12"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="fevtexMobile" FilterType="Numbers" TargetControlID="txtMobile"
                                                    runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter Mobile No." 
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="txtMobile"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblTelephone" runat="server" Text="Telephone No."></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtTelephone" runat="server" MaxLength="12"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvtxtTelephone" runat="server" ErrorMessage="Enter Telephone Number" Enabled="false"
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ControlToValidate="txtTelephone"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="TelFilteredTextBoxExtender1" FilterType="Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtTelephone" runat="server" Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblEmail" runat="server" Text="Email Id" CssClass="styleReqFieldLabel"
                                                    meta:resourcekey="lblEmailResource1"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <%--Revision History--%>
                                                <%--  Done by: Srivatsan
                                                      Date   : 03/11/2011
                                                      Purpose: To solve the UAT Observation bug. "The Email id accepts capital letters". 
                                                      Steps  : Added a Filter to filter the capital letters 
                                                --%>
                                                <asp:TextBox ID="txtVEndorEmailId" runat="server" MaxLength="180"></asp:TextBox>

                                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="None" SetFocusOnError="True"
                                                    ErrorMessage="Enter Email Id" ControlToValidate="txtVEndorEmailId" ValidationGroup="EntityDetails"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtVEndorEmailId"
                                                    ValidationGroup="EntityDetails" Display="None" SetFocusOnError="True" ErrorMessage="Enter valid Email id"
                                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;|,]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblTAN" runat="server" Text="TAN"></asp:Label>
                                            </td>

                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtTAN" runat="server" MaxLength="10"></asp:TextBox>
                                                <asp:HiddenField ID="hentadrz_id" runat="server" />
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtTAN" runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td class="styleFieldAlign">
                                                <asp:Label ID="lblSGSTIN" runat="server" Text="GSTIN" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtSGSTin" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="filsgstin" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                    TargetControlID="txtSGSTin" runat="server" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvSgstin" runat="server" Display="None" ErrorMessage="Enter the SGSTIN"
                                                    ControlToValidate="txtSGSTin" ValidationGroup="EntityDetails" Enabled="false" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>



                                            <td class="styleFieldAlign">
                                                <asp:Label ID="lblSGSTREGDate" runat="server" Text="GST Date"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="TxtSGSTRegDate" runat="server" Width="100px"></asp:TextBox>
                                                <asp:Image ID="imgSGSTRegDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <asp:RequiredFieldValidator ID="rfvSgstregdate" runat="server" Display="None"
                                                    ErrorMessage="Enter SGST Registration Date" ValidationGroup="EntityDetails" ControlToValidate="TxtSGSTRegDate"
                                                    SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="TxtSGSTRegDate"
                                                    PopupButtonID="imgSGSTRegDate"
                                                    ID="CalendarExtender3" Enabled="True">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:Button ID="btnAddAddress" runat="server" CssClass="styleSubmitShortButton"
                                                    Text="Add" OnClick="btnAddAddress_OnClick" ValidationGroup="EntityDetails" />
                                                <input id="hdnAddID" runat="server" type="hidden"></input>
                                                <asp:Button ID="btnModifyAddress" runat="server" CssClass="styleSubmitShortButton" ValidationGroup="EntityDetails"
                                                    Text="Modify" OnClick="btnModifyAddress_OnClick" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="gvBAddress" runat="server" AutoGenerateColumns="False"
                                                    EmptyDataText="No Statewise Address Found!..." OnSelectedIndexChanged="gvBAddress_SelectedIndexChanged"
                                                    OnRowDataBound="gvBAddress_RowDataBound" OnRowDeleting="gvBAddress_RowDeleting"
                                                    Width="100%">
                                                    <Columns>
                                                        <asp:CommandField ShowSelectButton="True" Visible="false" />
                                                        <asp:TemplateField HeaderText="Residential Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblResStatusCode" Visible="false" runat="server" Text='<%#Bind("RES_ADD") %>'></asp:Label>
                                                                <asp:Label ID="lblResStatus" runat="server" Text='<%#Bind("NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Registration Status">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRegStatusCode" Visible="false" runat="server" Text='<%#Bind("REG_ADD") %>'></asp:Label>
                                                                <asp:Label ID="lblStaRegStatus" runat="server" Text='<%#Bind("REGSTATUS") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Address 1">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRowNo" runat="server" Visible="false" Text='<%#Bind("Rowno") %>'></asp:Label>
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
                                                        <asp:TemplateField HeaderText="City">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCity" runat="server" Text='<%#Bind("City") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="State">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblState" runat="server" Text='<%#Bind("State") %>'></asp:Label>
                                                                <asp:Label ID="lblStateCode" Visible="false" runat="server" Text='<%#Bind("Location_Category_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Location Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLoc_Code" runat="server" Text='<%#Bind("Loc_Code") %>'></asp:Label>
                                                                <asp:HiddenField ID="gvhentityadrz_id" runat="server" Value='<%#Bind("Address_ID") %>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Country">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCountry" runat="server" Text='<%#Bind("Country") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Address State">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblResiState" runat="server" Text='<%#Bind("Resident_State") %>'></asp:Label>
                                                                <asp:Label ID="lblResiStateID" Visible="false" runat="server" Text='<%#Bind("Resident_State_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="PIN">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPIN" runat="server" Text='<%#Bind("Pincode") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Telephone Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTelephoneNo" runat="server" Text='<%#Bind("Telephone") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mobile No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMobNo" runat="server" Text='<%#Bind("Mobile") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EmailID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmailID" runat="server" Text='<%#Bind("Email") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="VAT-TIN">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVATTIN" runat="server" Text='<%#Bind("VAT_Number") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CST-TIN">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCSTTIN" runat="server" Text='<%#Bind("CST_TIN") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TAN">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTAN" runat="server" Text='<%#Bind("Tax_Account_Number") %>'></asp:Label>
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
                                    <asp:PostBackTrigger ControlID="btnAddAddress" />
                                    <asp:PostBackTrigger ControlID="btnModifyAddress" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Bank Details" ID="tpBankdetails" CssClass="tabpan"
                        BackColor="Red" meta:resourcekey="tpBankdetailsResource1">
                        <HeaderTemplate>
                            Bank Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlBankDetails" runat="server">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <%-- <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBranch" runat="server" Text="Own Location" CssClass="styleReqFieldLabel"
                                                        meta:resourcekey="lblBranchResource1"></asp:Label>
                                                </td>--%>
                                                <%--<td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlBranch" runat="server" meta:resourcekey="ddlBranchResource1">
                                                    </asp:DropDownList>
                                                </td>--%>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountType" runat="server" Text="Account Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlAccountType" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountNumber" runat="server" Text="Account Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAccountNumber" runat="server" MaxLength="16"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMICRCode" runat="server" Text="MICR Code"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMICRCode" runat="server" MaxLength="9" onblur="jsMICRvaildate(this);"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftexMICRCode" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="txtMICRCode" runat="server" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtendertxtBankName" runat="server"
                                                        TargetControlID="txtBankName" FilterType="Custom" FilterMode="InvalidChars" Enabled="True"
                                                        InvalidChars="!,@,#,$,%,^,&,*,(,),_,-,+,~,`,?,.,:,;,/,\,},{,[,],|,',=,'<','>'">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:TextBox ID="txtBankName" runat="server" MaxLength="40" Wrap="true" onkeyup="maxlengthfortxt(40)"
                                                        Width="70%" onchange="maxlengthfortxt(40)" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblIFSC_Code" Text="IFSC Code"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtIFSC_Code" runat="server" MaxLength="11"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FTEtxtIFSC_Code" runat="server" ValidChars=" ." FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        Enabled="True" TargetControlID="txtIFSC_Code">
                                                    </cc1:FilteredTextBoxExtender>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBranchName" runat="server" Text="Branch Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtBranchName" Width="70%" runat="server" Columns="20" Wrap="true"
                                                        onkeyup="maxlengthfortxt(40)" onchange="maxlengthfortxt(40)" TextMode="MultiLine"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="fteBranchName" FilterType="Custom" FilterMode="InvalidChars"
                                                        TargetControlID="txtBranchName" runat="server" InvalidChars="!,@,#,$,%,^,&,*,(,),_,-,+,~,`,?,.,:,;,/,\,},{,[,],|,',=,'<','>'"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkDefaultAccount" CssClass="styleDisplayLabel"
                                                        Text="Default Account" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBankAddress" runat="server" Text="Branch Address"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtBankAddress" Width="70%" runat="server" MaxLength="300" onkeyup="maxlengthfortxt(300)"
                                                        onchange="maxlengthfortxt(300)" TextMode="MultiLine" Wrap="true" Columns="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAddState" runat="server" Text="State"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlAddState" Width="72%" runat="server">
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td border="1">
                                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" meta:resourcekey="lblErrorMessageResource1"></asp:Label>
                                                    <%--  <asp:RequiredFieldValidator ID="rfvddlBranch" runat="server" ErrorMessage="Select an Own Location"
                                                        ValidationGroup="Add" Display="None" SetFocusOnError="True" ControlToValidate="ddlBranch"
                                                        InitialValue="0" meta:resourcekey="rfvddlBranchResource1"></asp:RequiredFieldValidator>--%>
                                                    <asp:RequiredFieldValidator ID="rfvddlAccountType" runat="server" ErrorMessage="Select an Account Type"
                                                        ValidationGroup="Add" Display="None" SetFocusOnError="True" ControlToValidate="ddlAccountType"
                                                        InitialValue="0"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvAccountType" runat="server" ErrorMessage="Enter the Account Number"
                                                        ValidationGroup="Add" Display="None" SetFocusOnError="True" ControlToValidate="txtAccountNumber"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvtxtMICRCode" runat="server" ErrorMessage="Enter the MICR Code"
                                                        ValidationGroup="Add" Display="None" SetFocusOnError="True" ControlToValidate="txtMICRCode"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvtxtIFSCCode" runat="server" ErrorMessage="Enter the IFSC Code"
                                                        ValidationGroup="Add" Display="None" SetFocusOnError="True" ControlToValidate="txtIFSC_Code"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvtxtBankName" runat="server" ErrorMessage="Enter the Bank Name"
                                                        ValidationGroup="Add" Display="None" SetFocusOnError="True" ControlToValidate="txtBankName"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvtxtBranchName" runat="server" ErrorMessage="Enter the Branch Name"
                                                        ValidationGroup="Add" Display="None" SetFocusOnError="True" ControlToValidate="txtBranchName"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvtxtBankAddress" runat="server" ControlToValidate="txtBankAddress"
                                                        Display="None" ErrorMessage="Enter the Address" SetFocusOnError="True" ValidationGroup="Add" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvAddState" runat="server" Display="None" SetFocusOnError="True"
                                                        ErrorMessage="Select Bank State" ControlToValidate="ddlAddState" ValidationGroup="Add" InitialValue="0"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="fteAccountNumber" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="txtAccountNumber" runat="server" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td align="center" colspan="2">&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnBnkAdd" runat="server" CssClass="styleSubmitShortButton"
                                                        OnClick="btnBnkAdd_Click" OnClientClick="return fnCheckPageValidators('Add',false);"
                                                        Text="Add" ValidationGroup="Add" />
                                                    <asp:Button ID="btnBnkModify" runat="server" CssClass="styleSubmitShortButton" OnClick="btnBnkModify_Click"
                                                        OnClientClick="return fnCheckPageValidators('Add',false);" Text="Modify" ValidationGroup="Add" />
                                                    <asp:Button ID="btnBnkClear" runat="server" CausesValidation="False" Visible="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnBnkClear_Click" />
                                                    <input id="hdnBankId" runat="server" type="hidden" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <table width="100%">
                                        <tr>
                                            <td align="left" width="80%">
                                                <div id="divGrid">
                                                    <asp:GridView ID="grvBankDetails" runat="server" AutoGenerateColumns="False"
                                                        Width="100%" meta:resourcekey="grvBankDetailsResource1" EmptyDataText="No Bank Details Found!..." OnRowDataBound="grvBankDetails_RowDataBound"
                                                        OnRowDeleting="grvBankDetails_RowDeleting" OnSelectedIndexChanged="grvBankDetails_SelectedIndexChanged">
                                                        <HeaderStyle CssClass="FrozenHeader" />
                                                        <Columns>
                                                            <asp:CommandField ShowSelectButton="True" Visible="false" />
                                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAccountID" runat="server" Text='<%#Bind("Account_Id") %>'></asp:Label>
                                                                    <asp:Label ID="lblRowNo" runat="server" Visible="false" Text='<%#Bind("Rowno") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" Wrap="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAccountType" runat="server" Text='<%#Bind("Account_type") %>'></asp:Label>
                                                                    <asp:Label ID="lblAccTypeID" Visible="false" runat="server" Text='<%#Bind("AccountType_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAccountNumber" runat="server" Text='<%#Bind("Account_Number") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Bank Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBankName" runat="server" Text='<%#Bind("Bank_name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Branch Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBranchName" runat="server" Text='<%#Bind("Branch_name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Branch Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBranchAddress" runat="server" Text='<%#Bind("Branch_Address") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="State">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblState" runat="server" Text='<%#Bind("State") %>'></asp:Label>
                                                                    <asp:Label ID="lblStateCode" Visible="false" runat="server" Text='<%#Bind("Location_Category_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="MICR Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMICRCode" runat="server" Text='<%#Bind("MICR_code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="IFSC Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIFSCCode" runat="server" Text='<%#Bind("IFSC_code") %>'></asp:Label>
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
                                                </div>
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
                <br />
                <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                    Text="Save" ValidationGroup="Submit" />
                <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                    Text="Cancel" OnClick="btnCancel_Click" />
                <br />
            </td>
        </tr>
        <tr>
            <td width="100%">
                <asp:ValidationSummary ID="vsEntityMaster" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                    ShowMessageBox="false" HeaderText="Correct the following validation(s):" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ValidationGroup="Submit" />
                <asp:ValidationSummary ID="vsEntityMaster_bank" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ValidationGroup="Add" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ValidationGroup="EntityDetails" />
                <asp:CustomValidator ID="cvEntityMaster" runat="server" CssClass="styleMandatoryLabel"
                    Enabled="true" />
                <asp:CustomValidator ID="cvEntity" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s): " ValidationGroup="EntityDetails" />
                <asp:CustomValidator ID="vcSubmit" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s): " ValidationGroup="Submit" />
                <asp:CustomValidator ID="cvEntity_Add" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" HeaderText="Correct the following validation(s): " />
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnID" runat="server" />

    <script language="javascript" type="text/javascript">

        var tab;

        function pageLoad() {
            tab = $find('ctl00_ContentPlaceHolder1_tcEntityMaster');
            var querymode = location.search.split("qsMode=");
            if (querymode[1] != 'Q') {
                tab.add_activeTabChanged(on_Change);
                var newindex = tab.get_activeTabIndex(index);
                var btnSave = document.getElementById('<%=btnSave.ClientID %>')
                if (newindex == tab._tabs.length - 1)
                    btnSave.disabled = false;
                else
                    btnSave.disabled = true;
            }
        }


        var index = 0;

        function on_Change(sender, e) {

            var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var Valgrp = document.getElementById('<%=vsEntityMaster.ClientID %>');
            Valgrp.validationGroup = strValgrp;
            var newindex = tab.get_activeTabIndex(index);

            var btnSave = document.getElementById('<%=btnSave.ClientID %>');
            var btnclear = document.getElementById('<%=btnClear.ClientID %>');
            if (newindex > 2) {
                btnSave.disabled = false;
            }
            else {
                btnSave.disabled = true;

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
                                index = tab.get_activeTabIndex(index);


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

        function jsMICRvaildate(txtMICRCode) {



            if (txtMICRCode.value.length > 0) {

                if (!isNaN(txtMICRCode.value)) {
                    if (parseFloat(txtMICRCode.value) == "0") {
                        alert('MICR Code cannot be zero');
                        txtMICRCode.value = '';
                        document.getElementById(txtMICRCode.id).focus();
                        return;
                    }
                }

                if (txtMICRCode.value.length < txtMICRCode.maxLength) {
                    //--Added by Guna on 19th-Oct-2010 to address the bug 1790                
                    alert('MICR Code should not be less than ' + txtMICRCode.maxLength + ' digits');
                    ////alert('Please enter a valid MICR Code');
                    //--Ends Here

                    //alert("max" + txtMICRCode.maxLength+"valu len" + txtMICRCode.value.length);
                    document.getElementById(txtMICRCode.id).focus();


                }

            }
        }
        var index = 0;
        function on_Change(sender, e) {
            //tab = $find('ctl00_ContentPlaceHolder1_tcPricing');
            //debugger;
            var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var Valgrp = document.getElementById('<%=vsEntityMaster.ClientID %>')
            var btnSave = document.getElementById('<%=btnSave.ClientID %>')

            //btnSave.disabled=true;
            Valgrp.validationGroup = strValgrp;

            var newindex = tab.get_activeTabIndex(index);
            if (newindex == tab._tabs.length - 1)
                btnSave.disabled = false;
            else
                btnSave.disabled = true;
            if (newindex > index) {
                if (!fnCheckPageValidators(strValgrp, false)) {

                    tab.set_activeTabIndex(index);


                }
                else {
                    index = tab.get_activeTabIndex(index);
                    //var strValgrp_new = tab._tabs[index]._tab.outerText.trim();
                }
            }
            else {
                tab.set_activeTabIndex(newindex);
                index = tab.get_activeTabIndex(newindex);

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
        //Added By Thangam M on 13/Feb/2012 to make the address lable as textbox
        function funSetColor(textbox, option) {
            //debugger;
            var txt = document.getElementById(textbox)
            if (option == 1) {
                txt.style.backgroundColor = 'yellow';
                txt.style.cursor = 'hand';
            }
            else {
                txt.style.backgroundColor = 'white';
                txt.style.textDecoration = 'none';
            }
        }

    </script>

    <style type="text/css">
        .wraptext {
            word-wrap: break-word;
        }
    </style>
</asp:Content>
