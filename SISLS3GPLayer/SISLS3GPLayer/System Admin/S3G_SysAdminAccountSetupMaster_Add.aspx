<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_SysAdminAccountSetupMaster_Add.aspx.cs" Inherits="System_Admin_S3G_SysAdminAccountSetupMaster_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
        function ChkIsZeroChk(textbox) {

            if (parseFloat(textbox.value) == 0) {
                textbox.focus();
                textbox.value = '';
                alert('Value cannot be 0');
            }
        }
        function TrimSpace(textbox) {
            if (trim(textbox.value) == '') {
                textbox.value = '';
            }

        }

//Asset
        function GLCodeA_ItemSelected(sender, e) {
            var hdnGLCodeSearchIDA = $get('<%= hdnGLCodeSearchIDA.ClientID %>');
            hdnGLCodeSearchIDA.value = e.get_value();
        }
        function GLCodeA_ItemPopulated(sender, e) {
            var hdnGLCodeSearchIDA = $get('<%= hdnGLCodeSearchIDA.ClientID %>');
            hdnGLCodeSearchIDA.value = '';
        }
        function SLCodeA_ItemSelected(sender, e) {
            var hdnSLCodeSearchIDA = $get('<%= hdnSLCodeSearchIDA.ClientID %>');
            hdnSLCodeSearchIDA.value = e.get_value();
        }
        function SLCodeA_ItemPopulated(sender, e) {
            var hdnSLCodeSearchIDA = $get('<%= hdnSLCodeSearchIDA.ClientID %>');
            hdnSLCodeSearchIDA.value = '';
        }
        //Liability
        function GLCodeL_ItemSelected(sender, e) {
            var hdnGLCodeSearchIDL = $get('<%= hdnGLCodeSearchIDL.ClientID %>');
            hdnGLCodeSearchIDL.value = e.get_value();
        }
        function GLCodeL_ItemPopulated(sender, e) {
            var hdnGLCodeSearchIDL = $get('<%= hdnGLCodeSearchIDL.ClientID %>');
            hdnGLCodeSearchIDL.value = '';
        }
        function SLCodeL_ItemSelected(sender, e) {
            var hdnSLCodeSearchIDL = $get('<%= hdnSLCodeSearchIDL.ClientID %>');
            hdnSLCodeSearchIDL.value = e.get_value();
        }
        function SLCodeL_ItemPopulated(sender, e) {
            var hdnSLCodeSearchIDL = $get('<%= hdnSLCodeSearchIDL.ClientID %>');
            hdnSLCodeSearchIDL.value = '';
        }
        //INCOME
        function GLCodeI_ItemSelected(sender, e) {
            var hdnGLCodeSearchIDI = $get('<%= hdnGLCodeSearchIDI.ClientID %>');
            hdnGLCodeSearchIDI.value = e.get_value();
        }
        function GLCodeI_ItemPopulated(sender, e) {
            var hdnGLCodeSearchIDI = $get('<%= hdnGLCodeSearchIDI.ClientID %>');
            hdnGLCodeSearchIDI.value = '';
        }
        function SLCodeI_ItemSelected(sender, e) {
            var hdnSLCodeSearchIDI = $get('<%= hdnSLCodeSearchIDI.ClientID %>');
            hdnSLCodeSearchIDI.value = e.get_value();
        }
        function SLCodeI_ItemPopulated(sender, e) {
            var hdnSLCodeSearchIDI = $get('<%= hdnSLCodeSearchIDI.ClientID %>');
            hdnSLCodeSearchIDI.value = '';
        }
        //EXPENSE
        function GLCodeE_ItemSelected(sender, e) {
            var hdnGLCodeSearchIDE = $get('<%= hdnGLCodeSearchIDE.ClientID %>');
            hdnGLCodeSearchIDE.value = e.get_value();
        }
        function GLCodeE_ItemPopulated(sender, e) {
            var hdnGLCodeSearchIDE = $get('<%= hdnGLCodeSearchIDE.ClientID %>');
            hdnGLCodeSearchIDE.value = '';
        }
        function SLCodeE_ItemSelected(sender, e) {
            var hdnSLCodeSearchIDE = $get('<%= hdnSLCodeSearchIDE.ClientID %>');
            hdnSLCodeSearchIDE.value = e.get_value();
        }
        function SLCodeE_ItemPopulated(sender, e) {
            var hdnSLCodeSearchIDE = $get('<%= hdnSLCodeSearchIDE.ClientID %>');
            hdnSLCodeSearchIDE.value = '';
        }    
    </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" Text="Account Setup" CssClass="styleDisplayLabel">
                            </asp:Label>
                        </t>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px">
                <cc1:TabContainer ID="tcAcctCreation" runat="server" CssClass="styleTabPanel" Width="100%"
                    ScrollBars="Auto" AutoPostBack="true" ActiveTabIndex="0" TabIndex="1" OnActiveTabChanged="tcAcctCreation_ActiveTabChanged">
                    <cc1:TabPanel runat="server" HeaderText="Asset Tab" ID="tbAsset" CssClass="tabpan"
                        BackColor="Red">
                        <HeaderTemplate>
                            Asset
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblLob" runat="server" Text="Line of Business"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlLOB" runat="server" Width="205px" AutoPostBack="True" TabIndex="1"
                                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblBranch" runat="server" Text="Location"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlBranch" runat="server" Width="205px" AutoPostBack="True"
                                                    TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trOldA" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblGLAC" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtGLAC" runat="server" Width="200px" MaxLength="6" TabIndex="3"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvGLAC" runat="server" Display="None" ValidationGroup="Asset"
                                                    ErrorMessage="Enter the GL Account" ControlToValidate="txtGLAC" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtGLAC"
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblSLAC" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtSLAC" runat="server" Width="200px" MaxLength="12" TabIndex="4"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtSLAC"
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr id="trA" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlGLCodeA" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <%--OnTextChanged="txtGLCodeSearchA_OnTextChanged"--%>
                                                <asp:TextBox ID="txtGLCodeSearchA" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtGLCodeSearchA_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoGLCodeSearch" MinimumPrefixLength="2" OnClientPopulated="GLCodeA_ItemPopulated"
                                                    OnClientItemSelected="GLCodeA_ItemSelected" runat="server" TargetControlID="txtGLCodeSearchA"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtGLCodeSearchAExtender" runat="server" TargetControlID="txtGLCodeSearchA"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnGLCodeSearchIDA" runat="server" />
                                                <asp:RequiredFieldValidator ID="RFVtxtGLCodeSearchA" runat="server" Display="None"
                                                    ValidationGroup="Asset" ErrorMessage="Enter the GL Account" ControlToValidate="txtGLCodeSearchA"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlSLCodeA" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtSLCodeSearchA" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtSLCodeSearchA_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoSLCodeSearch" MinimumPrefixLength="2" OnClientPopulated="SLCodeA_ItemPopulated"
                                                    OnClientItemSelected="SLCodeA_ItemSelected" runat="server" TargetControlID="txtSLCodeSearchA"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtSLCodeSearchAExtender" runat="server" TargetControlID="txtSLCodeSearchA"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnSLCodeSearchIDA" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblActiveInd" runat="server" Text="Active"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%" style="padding-left: 7px;">
                                                <asp:CheckBox ID="CbxActive" runat="server" TabIndex="5" />
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%" valign="top">
                                                <asp:Label ID="lblAccountDesc" runat="server" CssClass="styleReqFieldLabel" Text="Account Description"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="4">
                                                <div style="height: 250px; overflow-x: hidden; overflow-y: auto;" width="90%">
                                                    <asp:GridView ID="gvAsset" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                        Width="97%" DataKeyNames="Account_Code_Desc_ID" BorderColor="Gray" OnRowCreated="gvAsset_RowCreated"
                                                        OnRowDataBound="gvAsset_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAAccId" runat="server" Text='<%# Eval("Account_Code_Desc_ID") %>' /></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CbxAsset" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkAAdd" runat="server" Text="Add" OnClick="lnkAAdd_Click" CausesValidation="false"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAccDes" runat="server" Text='<%# Bind("Account_Code_Desc") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" ID="txtADesc" onblur="TrimSpace(this);" runat="server"
                                                                                    MaxLength="50"></asp:TextBox>
                                                                                <%--<cc1:FilteredTextBoxExtender ID="filterA" runat="server" TargetControlID="txtADesc"
                                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                                            Enabled="True">
                                                                                        </cc1:FilteredTextBoxExtender>--%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Flag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFlag" runat="server" Text='<%# Bind("Account_Flag") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" onblur="TrimSpace(this);" ID="txtFlag" runat="server"
                                                                                    MaxLength="10"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="filterAF" runat="server" TargetControlID="txtFlag"
                                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" /"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td runat="server" align="center" colspan="5">
                                                <asp:Button runat="server" ID="btnAssetSave" ValidationGroup="Asset" CssClass="styleSubmitButton"
                                                    Text="Save" OnClientClick="return fnCheckPageValidators('Asset');" OnClick="btnAssetSave_Click" />
                                                <asp:Button runat="server" ID="btnAssetClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClick="btnAssetClear_Click" />
                                                <cc1:ConfirmButtonExtender ID="btnAssetClear_ConfirmButtonExtender" runat="server"
                                                    ConfirmText="Do you want to Clear?" Enabled="True" TargetControlID="btnAssetClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button runat="server" ID="btnAssetCancel" CssClass="styleSubmitButton" CausesValidation="False"
                                                    Text="Cancel" OnClick="btnAssetCancel_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:ValidationSummary ID="vsAsset" runat="server" ValidationGroup="Asset" CssClass="styleMandatoryLabel"
                                                    HeaderText="Please correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tbLib" runat="server" HeaderText="Liability Tab">
                        <HeaderTemplate>
                            Liability
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblLLOB" runat="server" Text="Line of Business"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlLLOB" runat="server" Width="205px" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblLBranch" runat="server" Text="Location"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlLBranch" runat="server" Width="205px" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr  id="trOldL" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblLGLAC" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtLGLAC" runat="server" Width="200px" MaxLength="6" TabIndex="3"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox><asp:RequiredFieldValidator ID="rfvLGLAC"
                                                        runat="server" Display="None" ValidationGroup="Liaibity" ErrorMessage="Enter the GL Account"
                                                        ControlToValidate="txtLGLAC" SetFocusOnError="True"></asp:RequiredFieldValidator><cc1:FilteredTextBoxExtender
                                                            ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txtLGLAC" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            ValidChars=" " Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblLSLAC" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtLSLAC" runat="server" Width="200px" MaxLength="12" TabIndex="4"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtLSLAC"
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                       <tr id="trL" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlGLCodeL" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <%--OnTextChanged="txtGLCodeSearchA_OnTextChanged"--%>
                                                <asp:TextBox ID="txtGLCodeSearchL" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtGLCodeSearchL_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoGLCodeSearchL" MinimumPrefixLength="2" OnClientPopulated="GLCodeL_ItemPopulated"
                                                    OnClientItemSelected="GLCodeL_ItemSelected" runat="server" TargetControlID="txtGLCodeSearchL"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtGLCodeSearchAExtenderL" runat="server" TargetControlID="txtGLCodeSearchL"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnGLCodeSearchIDL" runat="server" />
                                                <asp:RequiredFieldValidator ID="RFVtxtGLCodeSearchL" runat="server" Display="None"
                                                    ValidationGroup="Liaibity" ErrorMessage="Enter the GL Account" ControlToValidate="txtGLCodeSearchL"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlSLCodeL" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtSLCodeSearchL" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtSLCodeSearchL_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoSLCodeSearchL" MinimumPrefixLength="2" OnClientPopulated="SLCodeL_ItemPopulated"
                                                    OnClientItemSelected="SLCodeL_ItemSelected" runat="server" TargetControlID="txtSLCodeSearchL"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtSLCodeSearchAExtenderL" runat="server" TargetControlID="txtSLCodeSearchL"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnSLCodeSearchIDL" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblLActive" runat="server" Text="Active"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%" style="padding-left: 7px;">
                                                <asp:CheckBox ID="CbxLActive" runat="server" TabIndex="5" />
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%" valign="top">
                                                <asp:Label ID="lblLACdesc" runat="server" Text="Account Description" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="4">
                                                <div style="height: 250px; overflow: auto" width="90%">
                                                    <asp:GridView ID="gvLiability" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                        Width="97%" DataKeyNames="Account_Code_Desc_ID" OnRowCreated="gvLiability_RowCreated"
                                                        OnRowDataBound="gvLiability_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLAccId" runat="server" Text='<%# Eval("Account_Code_Desc_ID") %>' /></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CbxLibility" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkLAdd" runat="server" Text="Add" CausesValidation="False" OnClick="lnkLAdd_Click"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLAccDes" runat="server" Text='<%# Bind("Account_Code_Desc") %>'></asp:Label></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" onblur="TrimSpace(this);" ID="txtLDesc" runat="server"
                                                                                    MaxLength="50"></asp:TextBox>
                                                                                <%-- <cc1:FilteredTextBoxExtender ID="filterL" runat="server" TargetControlID="txtLDesc"
                                                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                                            Enabled="True">
                                                                                        </cc1:FilteredTextBoxExtender>--%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Flag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLFlag" runat="server" Text='<%# Bind("Account_Flag") %>'></asp:Label></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" onblur="TrimSpace(this);" ID="txtLFlag" runat="server"
                                                                                    MaxLength="10"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="filterLF" runat="server" TargetControlID="txtLFlag"
                                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" /"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="5">
                                                <asp:Button runat="server" ID="btnLiailitySave" ValidationGroup="Liaibity" CssClass="styleSubmitButton"
                                                    Text="Save" OnClientClick="return fnCheckPageValidators('Liaibity');" OnClick="btnLiailitySave_Click" />
                                                <asp:Button runat="server" ID="btnLiaibityClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClick="btnLiaibityClear_Click" />
                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Do you want to Clear?"
                                                    Enabled="True" TargetControlID="btnLiaibityClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button runat="server" ID="btnLiailityCancel" CssClass="styleSubmitButton" CausesValidation="False"
                                                    Text="Cancel" OnClick="btnLiailityCancel_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:ValidationSummary ID="vsLiaibily" runat="server" ValidationGroup="Liaibity"
                                                    CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tbIncome" runat="server" HeaderText="Income Tab">
                        <HeaderTemplate>
                            Income
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblILOB" runat="server" Text="Line of Business"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlILob" runat="server" Width="205px" TabIndex="1" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblIBranch" runat="server" Text="Location"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlIBranch" runat="server" Width="205px" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr  id="trOldI" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblIGLAC" runat="server" Text="GL Account" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtIGLAC" runat="server" Width="200px" MaxLength="6" TabIndex="3"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox><asp:RequiredFieldValidator ID="rfvIGLAC"
                                                        runat="server" Display="None" ValidationGroup="Income" ErrorMessage="Enter the GL Account"
                                                        ControlToValidate="txtIGLAC" SetFocusOnError="True"></asp:RequiredFieldValidator><cc1:FilteredTextBoxExtender
                                                            ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtIGLAC" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            ValidChars=" " Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblISLAC" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtISLAC" runat="server" Width="200px" MaxLength="12" TabIndex="4"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtISLAC"
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr id="trI" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlGLCodeI" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <%--OnTextChanged="txtGLCodeSearchA_OnTextChanged"--%>
                                                <asp:TextBox ID="txtGLCodeSearchI" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtGLCodeSearchI_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoGLCodeSearchI" MinimumPrefixLength="2" OnClientPopulated="GLCodeI_ItemPopulated"
                                                    OnClientItemSelected="GLCodeI_ItemSelected" runat="server" TargetControlID="txtGLCodeSearchI"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtGLCodeSearchAExtenderI" runat="server" TargetControlID="txtGLCodeSearchI"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnGLCodeSearchIDI" runat="server" />
                                                <asp:RequiredFieldValidator ID="RFVtxtGLCodeSearchI" runat="server" Display="None"
                                                    ValidationGroup="Income" ErrorMessage="Enter the GL Account" ControlToValidate="txtGLCodeSearchI"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlSLCodeI" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtSLCodeSearchI" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtSLCodeSearchI_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoSLCodeSearchI" MinimumPrefixLength="2" OnClientPopulated="SLCodeI_ItemPopulated"
                                                    OnClientItemSelected="SLCodeI_ItemSelected" runat="server" TargetControlID="txtSLCodeSearchI"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtSLCodeSearchAExtenderI" runat="server" TargetControlID="txtSLCodeSearchI"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnSLCodeSearchIDI" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblIActive" runat="server" Text="Active"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%" style="padding-left: 7px;">
                                                <asp:CheckBox ID="CbxIActive" runat="server" TabIndex="5" />
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%" valign="top">
                                                <asp:Label ID="lblIAcDesc" runat="server" Text="Account Description" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="4">
                                                <div style="height: 250px; overflow: auto" width="90%">
                                                    <asp:GridView ID="gvIncome" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                        Width="97%" DataKeyNames="Account_Code_Desc_ID" OnRowCreated="gvIncome_RowCreated"
                                                        OnRowDataBound="gvIncome_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIAccId" runat="server" Text='<%# Eval("Account_Code_Desc_ID") %>' /></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CbxIncome" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkIAdd" runat="server" Text="Add" CausesValidation="False" OnClick="lnkIAdd_Click"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Description">
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" onblur="TrimSpace(this);" ID="txtIDesc" runat="server"
                                                                                    MaxLength="50"></asp:TextBox>
                                                                                <%--<cc1:FilteredTextBoxExtender
                                                                                            ID="filterI" runat="server" TargetControlID="txtIDesc" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                                            ValidChars=" " Enabled="True">
                                                                                        </cc1:FilteredTextBoxExtender>--%>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIAccDes" runat="server" Text='<%# Bind("Account_Code_Desc") %>'></asp:Label></ItemTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Flag">
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" onblur="TrimSpace(this);" ID="txtIFlag" runat="server"
                                                                                    MaxLength="10"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="filterIF" runat="server" TargetControlID="txtIFlag"
                                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" /"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIFlag" runat="server" Text='<%# Bind("Account_Flag") %>'></asp:Label></ItemTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="5">
                                                <asp:Button runat="server" ID="btnIncomeSave" ValidationGroup="Income" CssClass="styleSubmitButton"
                                                    Text="Save" OnClientClick="return fnCheckPageValidators('Income');" OnClick="btnIncomeSave_Click" />
                                                <asp:Button runat="server" ID="btnInComeClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClick="btnInComeClear_Click" />
                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="Do you want to Clear?"
                                                    Enabled="True" TargetControlID="btnInComeClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button runat="server" ID="btnIncomeCancel" CssClass="styleSubmitButton" CausesValidation="False"
                                                    Text="Cancel" OnClick="btnIncomeCancel_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:ValidationSummary ID="vsIncome" runat="server" ValidationGroup="Income" CssClass="styleMandatoryLabel"
                                                    HeaderText="Please correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tbExpress" runat="server" HeaderText="Expenses Tab">
                        <HeaderTemplate>
                            Expenses
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblELob" runat="server" Text="Line of Business"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlELOB" runat="server" Width="205px" TabIndex="1" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblEBranch" runat="server" Text="Location"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:DropDownList ID="ddlEBranch" runat="server" Width="205px" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr  id="trOldE" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblEGLAC" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtEGLAC" runat="server" Width="200px" MaxLength="6" TabIndex="3"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox><asp:RequiredFieldValidator ID="rfvEGLAC"
                                                        runat="server" Display="None" ValidationGroup="Express" ErrorMessage="Enter the GL Account"
                                                        ControlToValidate="txtEGLAC" SetFocusOnError="True"></asp:RequiredFieldValidator><cc1:FilteredTextBoxExtender
                                                            ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtEGLAC" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                            ValidChars=" " Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblESLAC" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtESLAC" runat="server" Width="200px" MaxLength="12" TabIndex="4"
                                                    onblur="ChkIsZeroChk(this)"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtESLAC"
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                    Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr id="trE" runat="server" visible="false">
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlGLCodeE" runat="server" CssClass="styleReqFieldLabel" Text="GL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <%--OnTextChanged="txtGLCodeSearchA_OnTextChanged"--%>
                                                <asp:TextBox ID="txtGLCodeSearchE" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtGLCodeSearchE_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoGLCodeSearchE" MinimumPrefixLength="2" OnClientPopulated="GLCodeE_ItemPopulated"
                                                    OnClientItemSelected="GLCodeE_ItemSelected" runat="server" TargetControlID="txtGLCodeSearchE"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtGLCodeSearchAExtenderE" runat="server" TargetControlID="txtGLCodeSearchE"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnGLCodeSearchIDE" runat="server" />
                                                <asp:RequiredFieldValidator ID="RFVtxtGLCodeSearchE" runat="server" Display="None"
                                                    ValidationGroup="Express" ErrorMessage="Enter the GL Account" ControlToValidate="txtGLCodeSearchE"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblddlSLCodeE" runat="server" Text="SL Account"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                                <asp:TextBox ID="txtSLCodeSearchE" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Width="182px" OnTextChanged="txtSLCodeSearchE_OnTextChanged"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="autoSLCodeSearchE" MinimumPrefixLength="2" OnClientPopulated="SLCodeE_ItemPopulated"
                                                    OnClientItemSelected="SLCodeE_ItemSelected" runat="server" TargetControlID="txtSLCodeSearchE"
                                                    ServiceMethod="GetGLCodeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                    CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtSLCodeSearchAExtenderE" runat="server" TargetControlID="txtSLCodeSearchE"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnSLCodeSearchIDE" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblEActive" runat="server" Text="Active"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="25%" style="padding-left: 7px;">
                                                <asp:CheckBox ID="CbxEActive" runat="server" TabIndex="5" />
                                            </td>
                                            <td width="5%">
                                            </td>
                                            <td class="styleFieldLabel" width="20%">
                                            </td>
                                            <td class="styleFieldAlign" width="25%">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" width="20%" valign="top">
                                                <asp:Label ID="lblEAcDesc" runat="server" Text="Account Description" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" colspan="4">
                                                <div style="height: 250px; overflow: auto" width="90%">
                                                    <asp:GridView ID="gvExpress" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                        Width="97%" DataKeyNames="Account_Code_Desc_ID" OnRowCreated="gvExpress_RowCreated"
                                                        OnRowDataBound="gvExpress_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEAccId" runat="server" Text='<%# Eval("Account_Code_Desc_ID") %>' /></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CbxExpress" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkEAdd" runat="server" Text="Add" CausesValidation="False" OnClick="lnkEAdd_Click"></asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEAccDes" runat="server" Text='<%# Bind("Account_Code_Desc") %>'></asp:Label></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" onblur="TrimSpace(this);" ID="txtEDesc" runat="server"
                                                                                    MaxLength="50"></asp:TextBox>
                                                                                <%-- <cc1:FilteredTextBoxExtender ID="filterE" runat="server" TargetControlID="txtEDesc"
                                                                                             ValidChars=" "
                                                                                            Enabled="True">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                    </td>--%>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Flag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEFlag" runat="server" Text='<%# Bind("Account_Flag") %>'></asp:Label></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr align="left">
                                                                            <td>
                                                                                <asp:TextBox AutoCompleteType="None" onblur="TrimSpace(this);" ID="txtEFlag" runat="server"
                                                                                    MaxLength="10"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="filterEF" runat="server" TargetControlID="txtEFlag"
                                                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" /"
                                                                                    Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </FooterTemplate>
                                                                <FooterStyle CssClass="styleGridHeader" />
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="5">
                                                <asp:Button runat="server" ID="btnExpressSave" ValidationGroup="Express" CssClass="styleSubmitButton"
                                                    Text="Save" OnClientClick="return fnCheckPageValidators('Express');" OnClick="btnExpressSave_Click" />
                                                <asp:Button runat="server" ID="btnExpressClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClick="btnExpressClear_Click" />
                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server" ConfirmText="Do you want to Clear?"
                                                    Enabled="True" TargetControlID="btnExpressClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button runat="server" ID="btnExpressCancel" CssClass="styleSubmitButton" CausesValidation="False"
                                                    Text="Cancel" OnClick="btnExpressCancel_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:ValidationSummary ID="vsExpress" runat="server" ValidationGroup="Express" CssClass="styleMandatoryLabel"
                                                    HeaderText="Please correct the following validation(s):  " />
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
    </table>
    <table>
        <tr>
            <td>
                <asp:CustomValidator ID="cvAsset" runat="server" Display="None" ValidationGroup="Asset"
                    CssClass="styleMandatoryLabel"></asp:CustomValidator>
                <asp:CustomValidator ID="cvLib" runat="server" Display="None" ValidationGroup="Liaibity"
                    CssClass="styleMandatoryLabel"></asp:CustomValidator>
                <asp:CustomValidator ID="cvIncome" runat="server" Display="None" ValidationGroup="Income"
                    CssClass="styleMandatoryLabel"></asp:CustomValidator>
                <asp:CustomValidator ID="cvExp" runat="server" Display="None" ValidationGroup="Express"
                    CssClass="styleMandatoryLabel"></asp:CustomValidator>
                <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: medium"></asp:Label>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnID" runat="server" />
</asp:Content>
