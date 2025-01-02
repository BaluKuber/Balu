<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_ORG_MRA_ADD.aspx.cs" Inherits="Origination_S3G_ORG_MRA_ADD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Scripts/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" language="javascript">

        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_tcMRA_tbgeneral_btnCreateCustomer').click();
        }

        function changeTextBoxStateonSignatory() {
            var dropDown = $get('<%=ddlSignedReceived.ClientID %>');
            var signNamevalidator = $get('<%=rfvSignatoryName.ClientID %>');
            var signContactvalidator = $get('<%=rfvSignatoryNumber.ClientID %>');
            var signDesignvalidator = $get('<%=rfvSignatoryDesignation.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtSignatoryName.ClientID %>').removeAttr("disabled");
                $('#<%= txtSignatoryDesignation.ClientID %>').removeAttr("disabled");
                $('#<%= txtSignatoryEmail.ClientID %>').removeAttr("disabled");
                $('#<%= txtSignatoryNumber.ClientID %>').removeAttr("disabled");

            }
            else {
                $('#<%= txtSignatoryName.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtSignatoryDesignation.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtSignatoryEmail.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtSignatoryNumber.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtSignatoryName.ClientID %>').val("");
                $('#<%= txtSignatoryDesignation.ClientID %>').val("");
                $('#<%= txtSignatoryEmail.ClientID %>').val("");
                $('#<%= txtSignatoryNumber.ClientID %>').val("");




            }
        }



        function changeTextBoxStateonArbiteration() {
            var dropDown = $get('<%=ddlArbiterationClause.ClientID %>');
            var ClauseValidator = $get('<%=rfvClauseNumber.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtClauseNumber.ClientID %>').removeAttr("disabled");
                $('#<%= txtClauseNumber.ClientID %>').focus();
                $('#<%= lblClauseNumber.ClientID %>').addClass("styleReqFieldLabel");
                ClauseValidator.enabled = true;
            }
            else {
                $('#<%= txtClauseNumber.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtClauseNumber.ClientID %>').val("");
                $('#<%= lblClauseNumber.ClientID %>').removeClass("styleReqFieldLabel");
                ClauseValidator.enabled = false;
            }
        }


        function onOPCNotice_Change() {
            var dropDown = $get('<%=ddlOPCNotice.ClientID %>');
            var ClauseValidator = $get('<%=rfvOPCtoCustomer.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtOPCtoCustomer.ClientID %>').removeAttr("disabled");
                $('#<%= txtOPCtoCustomer.ClientID %>').focus();
                $('#<%= lblOPCtoCustomer.ClientID %>').addClass("styleReqFieldLabel");
                ClauseValidator.enabled = true;
            }
            else {
                $('#<%= txtOPCtoCustomer.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtOPCtoCustomer.ClientID %>').val("");
                $('#<%= lblOPCtoCustomer.ClientID %>').removeClass("styleReqFieldLabel");
                ClauseValidator.enabled = false;
            }
        }

        function onLesseeNotice_Change() {
            var dropDown = $get('<%=ddlCustomerNtPrd.ClientID %>');
            var ClauseValidator1 = $get('<%=rfvCustomertoOPC.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtCustomertoOPC.ClientID %>').removeAttr("disabled");
                $('#<%= txtCustomertoOPC.ClientID %>').focus();
                $('#<%= lblLesseeNoD.ClientID %>').addClass("styleReqFieldLabel");
                ClauseValidator1.enabled = true;
            }
            else {
                $('#<%= txtCustomertoOPC.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtCustomertoOPC.ClientID %>').val("");
                $('#<%= lblLesseeNoD.ClientID %>').removeClass("styleReqFieldLabel");
                ClauseValidator1.enabled = false;
            }
        }

        function onJurisdictionChange() {
            var dropDown = $get('<%=ddlJurisdiction.ClientID %>');
            var ClauseValidator1 = $get('<%=rfvCity.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtJurisdictionCity.ClientID %>').removeAttr("disabled");
                $('#<%= txtJurisdictionCity.ClientID %>').focus();
                $('#<%= lblCity.ClientID %>').addClass("styleReqFieldLabel");
                ClauseValidator1.enabled = true;
            }
            else {
                $('#<%= txtJurisdictionCity.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtJurisdictionCity.ClientID %>').val("");
                $('#<%= lblCity.ClientID %>').removeClass("styleReqFieldLabel");
                ClauseValidator1.enabled = false;
            }
        }

        function onAmendment() {
            var dropDown = $get('<%=ddlMRAAmended.ClientID %>');
            var ClauseValidator1 = $get('<%=rfvAmendedDate.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtAmendmentDate.ClientID %>').removeAttr("disabled");
                $('#<%= txtAmendmentDate.ClientID %>').focus();
                $('#<%= lblAmendmentDate.ClientID %>').addClass("styleReqFieldLabel");
                ClauseValidator1.enabled = true;
            }
            else {
                $('#<%= txtAmendmentDate.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtAmendmentDate.ClientID %>').val("");
                $('#<%= lblAmendmentDate.ClientID %>').removeClass("styleReqFieldLabel");
                ClauseValidator1.enabled = false;
            }
        }

        function changeTextBoxStateonAutoExtension() {
            var dropDown = $get('<%=ddlAutoExtension.ClientID %>');
            var extensionTermValidator = $get('<%=rfvAutoExtensionTerm.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtAutoExtensionTerm.ClientID %>').removeAttr("disabled");
                $('#<%= txtAutoExtensionTerm.ClientID %>').focus();
                $('#<%= lblAutoExtensionTerm.ClientID %>').addClass("styleReqFieldLabel");
                $('#<%= txtAutoExtensionCond.ClientID %>').removeAttr("disabled");
                extensionTermValidator.enabled = true;
            }
            else {
                $('#<%= txtAutoExtensionTerm.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtAutoExtensionTerm.ClientID %>').val("");
                $('#<%= lblAutoExtensionTerm.ClientID %>').removeClass("styleReqFieldLabel");
                $('#<%= txtAutoExtensionCond.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtAutoExtensionCond.ClientID %>').val("");
                extensionTermValidator.enabled = false;
            }
        }

        function onInterimRent_Change() {
            var dropDown = $get('<%=ddlInterimRent.ClientID %>');
            if (dropDown.value == "1") {
                $('#<%= txtInsuranceCond.ClientID %>').removeAttr("disabled");
            }
            else {
                $('#<%= txtInsuranceCond.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtInsuranceCond.ClientID %>').val("");
            }
        }

        function RegionalManager_ItemSelected(sender, e) {
            var hdnRegionalMangerID = $get('<%= hdnRegionalMangerID.ClientID %>');
            hdnRegionalMangerID.value = e.get_value();
        }
        function RegionalManager_ItemPopulated(sender, e) {
            var hdnRegionalMangerID = $get('<%= hdnRegionalMangerID.ClientID %>');
            hdnRegionalMangerID.value = '';
        }

        function AcctManager1_ItemSelected(sender, e) {
            var hdnAcctManager1ID = $get('<%= hdnAcctManager1ID.ClientID %>');
            hdnAcctManager1ID.value = e.get_value();
        }
        function AcctManager1_ItemPopulated(sender, e) {
            var hdnAcctManager1ID = $get('<%= hdnAcctManager1ID.ClientID %>');
            hdnAcctManager1ID.value = '';
        }

        function AcctManager2_ItemSelected(sender, e) {
            var hdnAcctManager2ID = $get('<%= hdnAcctManager2ID.ClientID %>');
            hdnAcctManager2ID.value = e.get_value();
        }
        function AcctManager2_ItemPopulated(sender, e) {
            var hdnAcctManager2ID = $get('<%= hdnAcctManager2ID.ClientID %>');
            hdnAcctManager2ID.value = '';
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
    <asp:UpdatePanel ID="updtpnl1" runat="server">
        <ContentTemplate>
            <cc1:TabContainer ID="tcMRA" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                Width="99%" ScrollBars="None">
                <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                    BackColor="Red" ToolTip="General" Width="99%">
                    <HeaderTemplate>
                        General
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0" border="1">
                            <tr>
                                <td valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr width="100%">
                                            <td width="55%">
                                                <asp:Panel ID="pnlLobInfo" runat="server" ToolTip="MRA Information"
                                                    GroupingText="MRA Information" CssClass="stylePanel">
                                                    <div id="div1" runat="server" style="height: 400px; width: 100%; overflow-x: hidden; overflow-y: auto;">
                                                        <asp:Panel ID="pnlMRAInfo" Width="98%" runat="server" CssClass="stylePanel">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="MRA Creation Date" ID="lblMRACretionDate" CssClass="styleReqFieldLabel"
                                                                            ToolTip="MRA Creation Date"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtMRACreationDate" ToolTip="MRA Creation Date" Width="35%" runat="server" MaxLength="12" ReadOnly="True"></asp:TextBox>
                                                                        <asp:Image ID="imgMRACreationDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                                                        <cc1:CalendarExtender ID="ceMRACreationDate" runat="server" Enabled="false"
                                                                            PopupButtonID="imgMRACreationDate" TargetControlID="txtMRACreationDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvMRACreationDate" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="txtMRACreationDate"
                                                                            ErrorMessage="Enter MRA Creation Date" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldAlign" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="MRA Effective Date" ID="lblMRAEffectiveDate" CssClass="styleReqFieldLabel"
                                                                            ToolTip="MRA Effective Date"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtMRAEffectiveDate" ToolTip="MRA Effective Date" Width="35%" runat="server" MaxLength="12"></asp:TextBox>
                                                                        <asp:Image ID="imgMRAEffectiveDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                                                        <cc1:CalendarExtender ID="ceMRAEffectiveDate" runat="server" Enabled="True"
                                                                            PopupButtonID="imgMRAEffectiveDate"
                                                                            TargetControlID="txtMRAEffectiveDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvMRAEffectiveDate" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="txtMRAEffectiveDate"
                                                                            ErrorMessage="Enter MRA Effective Date" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Standard Format" ID="lblStandardFormat" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:DropDownList ID="ddlStandardFormat" runat="server" ToolTip="Standard Format">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvStandardFormat" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="false" runat="server" ControlToValidate="ddlStandardFormat" InitialValue="0"
                                                                            ErrorMessage="Select the Standard Format" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Signed & Received" ID="lblSignreceived" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:DropDownList ID="ddlSignedReceived" runat="server" ToolTip="Signed & Received" onChange="javascript:changeTextBoxStateonSignatory()">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvSignedReceived" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="false" runat="server" ControlToValidate="ddlSignedReceived" InitialValue="0"
                                                                            ErrorMessage="Select the Signed & Received" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Signatory Name" ID="lblSignatoryName" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtSignatoryName" runat="server" ToolTip="Signatory Name" MaxLength="50" Width="300px"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftbeSignatoryName" runat="server" Enabled="True" FilterType="Custom, UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtSignatoryName" ValidChars=". ">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvSignatoryName" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="txtSignatoryName" Enabled="false"
                                                                            ErrorMessage="Enter the Signatory Name" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Signatory Designation" ID="lblSignatoryDesignation" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtSignatoryDesignation" runat="server" ToolTip="Signatory Designation" MaxLength="50"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvSignatoryDesignation" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="txtSignatoryDesignation" Enabled="false"
                                                                            ErrorMessage="Enter the Signatory Designation" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Signatory Contact Number" ID="lblSignatoryNumber" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtSignatoryNumber" runat="server" ToolTip="Signatory Contact Number" MaxLength="15"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftbeSignatoryNumber" runat="server" Enabled="True" FilterType="Custom,Numbers"
                                                                            TargetControlID="txtSignatoryNumber" ValidChars="+-">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator Display="None" ID="rfvSignatoryNumber" CssClass="styleMandatoryLabel"
                                                                            SetFocusOnError="True" runat="server" ControlToValidate="txtSignatoryNumber" Enabled="false"
                                                                            ErrorMessage="Enter the Signatory Contact Number" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Signatory Email" ID="lblSignatoryEmail" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtSignatoryEmail" runat="server" ToolTip="Signatory Email" MaxLength="60"></asp:TextBox>
                                                                        <asp:RegularExpressionValidator
                                                                            ID="revEmailId" runat="server" ControlToValidate="txtSignatoryEmail" Display="None" ValidationGroup="vsSave"
                                                                            ErrorMessage="Enter a valid Signatory Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Auth Basis of Signatory" ID="lblAuthorizationBasis" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:DropDownList ID="ddlAuthorizationBasis" runat="server" ToolTip="Authorization Basis of Signatory"></asp:DropDownList>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label ID="lblAuthorizationDate" runat="server" Text="Authorization Date" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtAuthorizationDate" runat="server" ToolTip="Authorization Date" MaxLength="12"></asp:TextBox>
                                                                        <asp:Image ID="imgAuthorizationDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                                                        <cc1:CalendarExtender ID="ceAuthorizationDate" runat="server" Enabled="True"
                                                                            PopupButtonID="imgAuthorizationDate"
                                                                            TargetControlID="txtAuthorizationDate">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label runat="server" Text="Board Resln-Lease Auth" ID="lblBoardAuthorization" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:DropDownList ID="ddlBoardAuthorization" runat="server" ToolTip="Lessee Board Resolution "></asp:DropDownList>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label ID="lblResolutionDate" runat="server" Text="Board Resolution Date" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtResolutionDate" runat="server" ToolTip="Board Resolution Date" MaxLength="12"></asp:TextBox>
                                                                        <asp:Image ID="imgResolutionDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                                                        <cc1:CalendarExtender ID="ceResolutionDate" runat="server" Enabled="True"
                                                                            PopupButtonID="imgResolutionDate"
                                                                            TargetControlID="txtResolutionDate">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label ID="lblAccountManager1" runat="server" Text="Account Manager 1" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtAcctmanager1" runat="server" ToolTip="Account Manager 1" MaxLength="50"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="autoAcctmanager1" OnClientPopulated="AcctManager1_ItemPopulated"
                                                                            OnClientItemSelected="AcctManager1_ItemSelected" runat="server" TargetControlID="txtAcctmanager1"
                                                                            ServiceMethod="GetSalesPersonList" Enabled="True" ServicePath="" CompletionSetCount="2"
                                                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                            ShowOnlyCurrentWordInCompletionListItem="True">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <cc1:TextBoxWatermarkExtender ID="tbwmeAcctmanager1" runat="server" TargetControlID="txtAcctmanager1"
                                                                            WatermarkText="--Select--" Enabled="True">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:HiddenField ID="hdnAcctManager1ID" runat="server" />
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label ID="lblAccountManager2" runat="server" Text="Account Manager 2" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtAcctmanager2" runat="server" ToolTip="Account Manager 2"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="autoAcctmanager2" OnClientPopulated="AcctManager2_ItemPopulated"
                                                                            OnClientItemSelected="AcctManager2_ItemSelected" runat="server" TargetControlID="txtAcctmanager2"
                                                                            ServiceMethod="GetSalesPersonList" Enabled="True" ServicePath="" CompletionSetCount="2"
                                                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                            ShowOnlyCurrentWordInCompletionListItem="True">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <cc1:TextBoxWatermarkExtender ID="tbwmeAcctmanager2" runat="server" TargetControlID="txtAcctmanager2"
                                                                            WatermarkText="--Select--" Enabled="True">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:HiddenField ID="hdnAcctManager2ID" runat="server" />
                                                                    </td>
                                                                </tr>

                                                                <tr width="100%">
                                                                    <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                                        <asp:Label ID="lblRegionalManager" runat="server" Text="Regional Manager" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                        <asp:TextBox ID="txtRegionalManager" runat="server" ToolTip="Regional Manager" MaxLength="50"></asp:TextBox>
                                                                        <cc1:AutoCompleteExtender ID="autoRegionalManager" OnClientPopulated="RegionalManager_ItemPopulated"
                                                                            OnClientItemSelected="RegionalManager_ItemSelected" runat="server" TargetControlID="txtRegionalManager"
                                                                            ServiceMethod="GetSalesPersonList" Enabled="True" ServicePath="" CompletionSetCount="2"
                                                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                            ShowOnlyCurrentWordInCompletionListItem="True">
                                                                        </cc1:AutoCompleteExtender>
                                                                        <cc1:TextBoxWatermarkExtender ID="tbwmeRegionalManager" runat="server" TargetControlID="txtRegionalManager"
                                                                            WatermarkText="--Select--" Enabled="True">
                                                                        </cc1:TextBoxWatermarkExtender>
                                                                        <asp:HiddenField ID="hdnRegionalMangerID" runat="server" />
                                                                    </td>
                                                                </tr>

                                                            </table>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="PnlCustEntityInformation1" runat="server" ToolTip="Lessee Information"
                                                    GroupingText="Lessee Informations" CssClass="stylePanel">
                                                    <div id="divcustomerentity" runat="server" style="height: 400px; width: 100%; overflow-x: hidden; overflow-y: auto;">
                                                        <asp:Panel ID="PnlCustEntityInformation" Width="95%" ToolTip="Lessee Informations"
                                                            runat="server" CssClass="stylePanel">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label runat="server" ToolTip="Cutomer or Entity" Text="Lessee" ID="lblCode"
                                                                            CssClass="styleMandatoryLabel"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtCustomerCode" ToolTip="Lessee code" runat="server"
                                                                            Style="display: none;" MaxLength="50"></asp:TextBox>
                                                                        <uc2:LOV ID="ucCustomerCodeLov" onblur="fnLoadCustomer()" runat="server" strLOV_Code="MRA"
                                                                            DispalyContent="Code" />
                                                                        <asp:Button ID="btnCreateCustomer" OnClick="btnCreateCustomer_Click"
                                                                            runat="server" Text="Create" Style="display: none;" CssClass="styleSubmitButton"
                                                                            CausesValidation="false" />
                                                                        <asp:RequiredFieldValidator ID="rfvcmbCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                            ErrorMessage="Select a Lessee" ValidationGroup="vsSave" CssClass="styleMandatoryLabel"
                                                                            Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        <asp:LinkButton ID="lnkViewCustomer" runat="server" Text="View Lessee" OnClick="lnkViewCustomer_Click"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Panel ID="pnlAddressDetails" runat="server">
                                                                            <uc1:S3GCustomerAddress ID="ucCustomerAddress" runat="server" />
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Panel ID="pnlGraceDays" runat="server" ToolTip="Lessee Information"
                                                                GroupingText=" Invoice Grace Days" CssClass="stylePanel">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ToolTip="Invoice Grace Type" Text="Invoice Grace Type" ID="lblGraceType"
                                                                                CssClass="styleFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ToolTip="Invoice Grace Type" ID="ddlInvoiceGraceType"
                                                                                CssClass="styleFieldLabel">


                                                                                <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                                                <asp:ListItem Value="1" Text="Month"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="Days"></asp:ListItem>

                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator Display="None" ID="rfvInvoiceGraceType" CssClass="styleMandatoryLabel"
                                                                                SetFocusOnError="True" runat="server" InitialValue="0" ControlToValidate="ddlInvoiceGraceType"
                                                                                ErrorMessage="Select the Invoice Grace Period Type" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                        </td>

                                                                    </tr>

                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ToolTip="No. Of Grace Days / Month" Text="No. Of Grace Days / Month" ID="lblInvoiceDays"
                                                                                CssClass="styleFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox runat="server" Width="40px" OnTextChanged="txtInvoiceGraceDays_TextChanged" AutoPostBack="true" ToolTip="Invoice Invoice Days" ID="txtInvoiceGraceDays"
                                                                                CssClass="styleMandatoryLabel"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="fltInvoiceDays" runat="server" TargetControlID="txtInvoiceGraceDays"
                                                                                FilterType="Numbers" Enabled="true">
                                                                            </cc1:FilteredTextBoxExtender>

                                                                            <asp:RequiredFieldValidator Display="None" ID="rfvGraceDays" CssClass="styleMandatoryLabel"
                                                                                SetFocusOnError="True" runat="server" ControlToValidate="txtInvoiceGraceDays" Enabled="false"
                                                                                ErrorMessage="Enter the No. Of Grace Days / Month" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlNoticeInfo" runat="server" ToolTip="Notice Intimating End of Term Renting"
                                        GroupingText="Notice Intimating End of Term Renting" CssClass="stylePanel">
                                        <table>
                                            <tr width="100%">
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblOPCNotice" runat="server" Text="OPC To Give Notice" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 10%">
                                                    <asp:DropDownList ID="ddlOPCNotice" runat="server" onChange="javascript:onOPCNotice_Change()"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvOPCNotice" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="false" runat="server" ControlToValidate="ddlOPCNotice" InitialValue="0"
                                                        ErrorMessage="Select the OPC to give notice" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 23%">
                                                    <asp:Label ID="lblOPCtoCustomer" runat="server" Text="OPC to Customer(Lessee)" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 15%">
                                                    <asp:TextBox ID="txtOPCtoCustomer" runat="server" ToolTip="OPC to Customer" MaxLength="3"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeOPCtoCustomer" runat="server" Enabled="True" TargetControlID="txtOPCtoCustomer"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvOPCtoCustomer" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtOPCtoCustomer" Enabled="false"
                                                        ErrorMessage="Enter the OPC to Customer(Lessee)" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>

                                            </tr>
                                            <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                <asp:Label ID="lblCustomertoOPC" runat="server" Text="Customer (Lessee) to OPC give Notice" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" style="width: 15%">
                                                <asp:DropDownList ID="ddlCustomerNtPrd" runat="server" onChange="javascript:onLesseeNotice_Change()"></asp:DropDownList>
                                                <asp:RequiredFieldValidator Display="None" ID="rfvddlCustomerNtPrd" CssClass="styleMandatoryLabel"
                                                    SetFocusOnError="false" runat="server" ControlToValidate="ddlCustomerNtPrd" InitialValue="0"
                                                    ErrorMessage="Select the Customer (Lessee) to OPC give Notice" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                            </td>
                                            <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                <asp:Label ID="lblLesseeNoD" runat="server" Text="Customer (Lessee) to OPC Number of Days" CssClass="styleDisplayLabel"></asp:Label>
                                            </td>
                                            <td align="left" class="styleFieldAlign" style="width: 15%">
                                                <asp:TextBox ID="txtCustomertoOPC" runat="server" ToolTip="OPC to Customer" MaxLength="3"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="ftbeCustomertoOPC" runat="server" Enabled="True" TargetControlID="txtCustomertoOPC"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <asp:RequiredFieldValidator Display="None" ID="rfvCustomertoOPC" CssClass="styleMandatoryLabel"
                                                    SetFocusOnError="True" runat="server" ControlToValidate="txtCustomertoOPC" Enabled="false"
                                                    ErrorMessage="Enter the Customer (Lessee) to OPC Number of Days" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                            </td>
                                            <tr>
                                            </tr>

                                            <tr width="100%">
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblAutoExtension" runat="server" Text="Auto Extension Allowed" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 10%; vertical-align: middle">
                                                    <asp:DropDownList ID="ddlAutoExtension" runat="server" ToolTip="Auto Extension Allowed" onChange="javascript:changeTextBoxStateonAutoExtension()"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvAutoExtension" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="ddlAutoExtension" InitialValue="0"
                                                        ErrorMessage="Select the Auto Extension Allowed" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 23%">
                                                    <asp:Label ID="lblAutoExtensionTerm" runat="server" Text="Auto Extension Term (in months)" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 15%; vertical-align: middle">
                                                    <asp:TextBox ID="txtAutoExtensionTerm" runat="server" ToolTip="Auto Extension Term" MaxLength="3"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeAutoExtensionTerm" runat="server" Enabled="True" TargetControlID="txtAutoExtensionTerm"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvAutoExtensionTerm" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtAutoExtensionTerm"
                                                        ErrorMessage="Enter the Auto Extension Term (in months)" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr width="100%">
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblInterimRent" runat="server" Text="Interim Rent Applicable" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 10%; vertical-align: middle">
                                                    <asp:DropDownList ID="ddlInterimRent" runat="server" ToolTip="Interim Rent Applicable" onChange="javascript: onInterimRent_Change()"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvInterimRent" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="ddlInterimRent" InitialValue="0"
                                                        ErrorMessage="Select the Interim Rent Applicable" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 23%">
                                                    <asp:Label ID="lblInterimCalculation" runat="server" Text="Interim Rent Calculation Basis" CssClass="styleDisplayLabel"
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 15%; vertical-align: middle">
                                                    <asp:DropDownList ID="ddlInterimCalculation" runat="server" ToolTip="Interim Rent Calculation Basis" Visible="false"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvInterimCalculation" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="ddlInterimCalculation" InitialValue="0"
                                                        ErrorMessage="Select the Interim Rent Calculation Basis" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblInterimRentRate" runat="server" Text="Interim Rent Rate" CssClass="styleDisplayLabel"
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 10%; vertical-align: middle">
                                                    <asp:TextBox ID="txtInterimRentRate" runat="server" ToolTip="Interim Rent Rate" MaxLength="7" Visible="false"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" Width="70px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeInterimRentRate" runat="server" Enabled="True" TargetControlID="txtInterimRentRate"
                                                        FilterType="Custom,Numbers" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvInterimRentRate" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtInterimRentRate"
                                                        ErrorMessage="Enter the Interim Rent Rate" ValidationGroup="vsSave">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td colspan="4"></td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblAutoExtensionCond" runat="server" Text="Auto Extension Conditions" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 15%">
                                                    <asp:TextBox ID="txtAutoExtensionCond" runat="server" ToolTip="Auto Extension Conditions" TextMode="MultiLine" MaxLength="500" onkeyup="maxlengthfortxt(500);"
                                                        Height="60px"></asp:TextBox>
                                                </td>

                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblInsuranceCond" runat="server" Text="Insurance Conditions" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 15%">
                                                    <asp:TextBox ID="txtInsuranceCond" runat="server" ToolTip="Insurance Conditions" TextMode="MultiLine" MaxLength="500" onkeyup="maxlengthfortxt(500);"
                                                        Height="60px" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlTermination" runat="server" ToolTip="Termination"
                                        GroupingText="Termination" CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td align="left" class="styleFieldLabel" valign="middle">
                                                    <asp:Label ID="lblTerminationCond" runat="server" Text="Termination Conditions" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="txtTerminationCond" runat="server" TextMode="MultiLine" MaxLength="300" Width="700px" Height="60px"
                                                        ToolTip="Termination Conditions" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr width="100%">
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblCustomerNoticePeriod" runat="server" Text="Cust Termn Notice Period (in days)" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCustomerNoticePeriod" runat="server" MaxLength="3" ToolTip="Lessee Notice Period" Width="60px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeCustomerNoticePeriod" runat="server" Enabled="True" TargetControlID="txtCustomerNoticePeriod"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvCustomerNoticePeriod" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtCustomerNoticePeriod"
                                                        ErrorMessage="Enter the Cust Termn Notice Period (in days)" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblOPCNoticePeriod" runat="server" Text="OPC Termn Notice Period (in days)" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOPCNoticePeriod" runat="server" MaxLength="3" ToolTip="OPC Notice Period" Width="60px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeOPCNoticePeriod" runat="server" Enabled="True" TargetControlID="txtOPCNoticePeriod"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr width="100%">
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblForeclosureRate" runat="server" Text="Foreclosure Rate(%)" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtForeClosureRate" runat="server" MaxLength="5" ToolTip="Foreclosure Rate" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeForeClosureRate" runat="server" Enabled="True" TargetControlID="txtForeClosureRate"
                                                        FilterType="Custom, Numbers" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvForeClosureRate" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtForeClosureRate"
                                                        ErrorMessage="Enter the Foreclosure Rate" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblBreakCost" runat="server" Text="Break Cost(%)" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtBreakCost" runat="server" MaxLength="5" ToolTip="Break Cost" Width="60px" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeBreakCost" runat="server" Enabled="True" TargetControlID="txtBreakCost"
                                                        FilterType="Custom, Numbers" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            <tr width="100%">
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblArbiterationClause" runat="server" Text="Arbitration Clause Exists" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlArbiterationClause" runat="server" ToolTip="Arbitration  Clause" onChange="javascript:changeTextBoxStateonArbiteration()"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvArbiterationClause" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="ddlArbiterationClause" InitialValue="0"
                                                        ErrorMessage="Select the Arbitration Clause Exists" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblClauseNumber" runat="server" Text="Clause Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtClauseNumber" runat="server" MaxLength="10" ToolTip="Clause Number" Width="70px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvClauseNumber" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtClauseNumber"
                                                        ErrorMessage="Enter the Clause Number" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr width="100%">
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblJurisdiction" runat="server" Text="Jurisdiction" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlJurisdiction" runat="server" ToolTip="Jurisdiction" onChange="javascript:onJurisdictionChange()"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvJurisdiction" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="ddlJurisdiction" InitialValue="0"
                                                        ErrorMessage="Select the Jurisdiction" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel">
                                                    <asp:Label ID="lblCity" runat="server" Text="City" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtJurisdictionCity" runat="server" MaxLength="50" ToolTip="City"></asp:TextBox>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvCity" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtJurisdictionCity"
                                                        ErrorMessage="Enter the City" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlAmendment" runat="server" ToolTip="Amendment"
                                        GroupingText="Amendment" CssClass="stylePanel">
                                        <table>

                                            <tr>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblOverdueRate" runat="server" Text="Overdue Rate(%)" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 7%">
                                                    <asp:TextBox ID="txtOverdueRate" runat="server" MaxLength="5" ToolTip="Overdue Rate" Width="60px" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeOverdueRate" runat="server" Enabled="True" TargetControlID="txtOverdueRate"
                                                        FilterType="Custom, Numbers" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvOverdueRate" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtOverdueRate"
                                                        ErrorMessage="Enter the Overdue Rate" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblMRAAmended" runat="server" Text="Is MRA Amended" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 10%">
                                                    <asp:DropDownList ID="ddlMRAAmended" runat="server" ToolTip="Is MRA Amended" onChange="javascript:onAmendment()"></asp:DropDownList>
                                                    <%--<asp:RequiredFieldValidator Display="None" ID="rfvMRAAmended" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="true" runat="server" ControlToValidate="ddlMRAAmended" InitialValue="0"
                                                        ErrorMessage="Select the Is MRA Amended" ValidationGroup="vsSave"></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 20%">
                                                    <asp:Label ID="lblAmendmentDate" runat="server" Text="Amendment Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" style="width: 20%; vertical-align: middle">
                                                    <asp:TextBox ID="txtAmendmentDate" runat="server" MaxLength="12"></asp:TextBox>
                                                    <asp:Image ID="imgAmendmentDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                                    <cc1:CalendarExtender ID="ceAmendmentDate" runat="server" Enabled="true"
                                                        PopupButtonID="imgAmendmentDate"
                                                        TargetControlID="txtAmendmentDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvAmendedDate" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True" runat="server" ControlToValidate="txtAmendmentDate"
                                                        ErrorMessage="Enter the Amendment Date" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td align="left" class="styleFieldLabel" valign="middle" style="width: 18%">
                                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td align="left" class="styleFieldAlign" colspan="6">
                                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" ToolTip="Remarks" Width="700px" Height="50px" onkeyup="maxlengthfortxt(200);"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
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
                        <asp:CustomValidator ID="cvMRA" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            onOPCNotice_Change();
            changeTextBoxStateonAutoExtension();
            onInterimRent_Change();
            changeTextBoxStateonArbiteration();
            changeTextBoxStateonSignatory();
            onLesseeNotice_Change();
            onJurisdictionChange();
            onAmendment();
        });
    </script>
</asp:Content>

