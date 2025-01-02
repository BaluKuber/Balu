<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3GDebitCreditNote_View.aspx.cs" Inherits="LoanAdmin_S3GDebitCreditNote_View" %>

<%--Ajax Control Toolkit--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%--Content--%>
<asp:Content ID="ContentTransLander" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function Common_ItemSelected(sender, e) {
            var hdnCommonID = $get('<%= hdnCommonID.ClientID %>');
            hdnCommonID.value = e.get_value();
        }
        function Common_ItemPopulated(sender, e) {
            var hdnCommonID = $get('<%= hdnCommonID.ClientID %>');
            hdnCommonID.value = '';

        }
        function Branch_ItemSelected(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
            hdnBranchID.value = e.get_value();
        }
        function Branch_ItemPopulated(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
            hdnBranchID.value = '';
        }

        function Lessee_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnLessee.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function Lessee_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnLessee.ClientID %>');
            hdnLessee.value = '';
        }

        function ODIInvoice_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnODIInvoice.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function ODIInvoice_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnODIInvoice.ClientID %>');
            hdnLessee.value = '';
        }

        function DCNInvoice_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnDCNInvoice.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function DCNInvoice_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnDCNInvoice.ClientID %>');
            hdnLessee.value = '';
        }

        function Invoice_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnInvoice.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function Invoice_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnInvoice.ClientID %>');
            hdnLessee.value = '';
        }

        function OldInvoice_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnOldInvoice.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function OldInvoice_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnOldInvoice.ClientID %>');
            hdnLessee.value = '';
        }

        function Tranche_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnTranche.ClientID %>');
            hdnLessee.value = e.get_value();
        }

        function Tranche_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnTranche.ClientID %>');
            hdnLessee.value = '';
        }

        function RS_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnRSNumber.ClientID %>');
            hdnLessee.value = e.get_value();
        }

        function RS_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnRSNumber.ClientID %>');
            hdnLessee.value = '';
        }

        function fnSelectAll(chkSelectAllRS, chkSelectRS) {
            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_grvTransLander');
            var TargetChildControl = chkSelectRS;
            //Get all the control of the type INPUT in the base control.
            var Inputs = grvCashFlow.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllRS.checked;
        }

    </script>
    <%--Design Started--%>
    <table width="100%" cellpadding="0" cellspacing="2px" border="0">
        <tr>
            <td colspan="4" class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" EnableViewState="false" CssClass="styleInfoLabel">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                    <%--Row -1 with 1 columns--%>
                    <tr width="100%">
                        <%--Spacer--%>
                        <td width="100%" colspan="4" align="center">&nbsp;
                        </td>
                    </tr>
                    <%--Row 1 with 4 columns--%>
                    <tr width="100%">
                        <%--Line of Business--%>
                        <td class="styleFieldLabel">
                            <asp:Label runat="server" Text="Line of Business" ID="lblLOBSearch" CssClass="styleReqFieldLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <cc1:ComboBox ID="ComboBoxLOBSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxLOBSearch_SelectedIndexChanged">
                            </cc1:ComboBox>
                            <asp:RequiredFieldValidator ID="RFVComboLOB" ValidationGroup="RFVDTransLander" InitialValue="-- Select --"
                                CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxLOBSearch"
                                SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <%--Start Date--%>
                        <td class="styleFieldLabel">
                            <asp:Label runat="server" Text="Start Date" ID="lblStartDateSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtStartDateSearch" runat="server" Width="150px"
                                OnTextChanged="txtStartDateSearch_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <asp:Image ID="imgStartDateSearch" runat="server"
                                ImageUrl="~/Images/calendaer.gif" ToolTip="Start Date" />
                            <cc1:CalendarExtender ID="CalendarExtenderStartDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVStartDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel" Enabled="false"
                                runat="server" ControlToValidate="txtStartDateSearch" SetFocusOnError="True"
                                ErrorMessage="Enter a Start Date" Display="None">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%--Row 2 with 4 columns--%>
                    <tr width="100%">
                        <%--Branch--%>
                        <td class="styleFieldLabel">
                            <asp:Label Text="Location" runat="server" ID="lblBranchSearch" CssClass="styleReqFieldLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <%--<cc1:ComboBox ID="ComboBoxBranchSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxBranchSearch_SelectedIndexChanged">
                            </cc1:ComboBox>
                            <asp:RequiredFieldValidator ID="RFVComboBranch" ValidationGroup="RFVDTransLander"
                                InitialValue="-- Select --" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxBranchSearch"
                                SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>--%>
                            <asp:TextBox ID="txtBranchSearch" runat="server" MaxLength="50" OnTextChanged="txtBranchSearch_OnTextChanged"
                                AutoPostBack="true" Width="182px"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="autoBranchSearch" MinimumPrefixLength="3" OnClientPopulated="Branch_ItemPopulated"
                                OnClientItemSelected="Branch_ItemSelected" runat="server" TargetControlID="txtBranchSearch"
                                ServiceMethod="GetBranchList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="txtBranchSearchExtender" runat="server" TargetControlID="txtBranchSearch"
                                WatermarkText="--Select--">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:HiddenField ID="hdnBranchID" runat="server" />
                            <asp:RequiredFieldValidator ID="RFVComboBranch" ValidationGroup="RFVDTransLander"
                                InitialValue="" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="txtBranchSearch"
                                SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <%--End Date--%>
                        <td class="styleFieldLabel">
                            <asp:Label runat="server" ID="lblEndDateSearch" Text="End Date" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtEndDateSearch" runat="server" Width="150px"
                                OnTextChanged="txtEndDateSearch_TextChanged" AutoPostBack="True"></asp:TextBox>
                            <asp:Image ID="imgEndDateSearch" runat="server"
                                ImageUrl="~/Images/calendaer.gif" ToolTip="End Date" />
                            <cc1:CalendarExtender ID="CalendarExtenderEndDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVEndDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel" Enabled="false"
                                runat="server" ControlToValidate="txtEndDateSearch" SetFocusOnError="True" ErrorMessage="Enter a End Date"
                                Display="None">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%--Row 3 with 4 columns--%>
                    <tr width="100%">
                        <%--Multiple Document Number --%>
                        <td class="styleFieldLabel">
                            <asp:Label Visible="false" Text="Transaction" runat="server" ID="lblMultipleDNC"
                                CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:DropDownList Visible="false" AutoPostBack="true" ID="ddlMultipleDNC" runat="server"
                                Width="185px" OnSelectedIndexChanged="ddlMultipleDNC_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="styleFieldLabel">
                            <asp:Label Visible="false" Text="Select Status" runat="server" ID="lblDNCOption"
                                CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:DropDownList Visible="false" ID="ddlDNCOption" runat="server" Width="175px"
                                OnSelectedIndexChanged="ddlDNCOption_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr class="styleFieldLabel">
                        <td class="styleFieldLabel">
                            <asp:Label Text="Lessee Name" runat="server" ID="lblLesseeNameSrch" CssClass="styleDisplayLabel" Visible="false" />
                        </td>
                        <td class="styleFieldAlign">
                            <%-- <uc3:Suggest ID="ddlLessee" runat="server" ServiceMethod="GetLesseeList" Visible="false" Width="280px" AutoPostBack="true" OnItem_Selected="ddlLessee_SelectedIndexChanged"  />--%>

                            <asp:TextBox ID="txtLessee" runat="server" OnTextChanged="txtLessee_OnTextChanged" Visible="false"
                                AutoPostBack="true" Width="280px"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="aceLessee" MinimumPrefixLength="3" OnClientPopulated="Lessee_ItemPopulated"
                                OnClientItemSelected="Lessee_ItemSelected" runat="server" TargetControlID="txtLessee"
                                ServiceMethod="GetLesseeList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="txtLesseeWatermarkExt" runat="server" TargetControlID="txtLessee"
                                WatermarkText="--Select--">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:HiddenField ID="hdnLessee" runat="server" />
                        </td>

                        <td class="styleFieldLabel">
                            <asp:Label Text="Tranche Name" runat="server" ID="lblTrancheName" CssClass="styleDisplayLabel" Visible="false" />
                            <asp:Label Text="Transaction Type" runat="server" ID="lblType" CssClass="styleDisplayLabel" Visible="false" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtTrancheName" runat="server" OnTextChanged="txtTrancheName_TextChanged" Visible="false"
                                AutoPostBack="true" Width="182px"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="acexTranche" MinimumPrefixLength="3" OnClientPopulated="Tranche_ItemPopulated"
                                OnClientItemSelected="Tranche_ItemSelected" runat="server" TargetControlID="txtTrancheName"
                                ServiceMethod="GetTrancheList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="twmeTranche" runat="server" TargetControlID="txtTrancheName"
                                WatermarkText="--Select--">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:HiddenField ID="hdnTranche" runat="server" />

                            <asp:DropDownList Visible="false" ID="ddlType" runat="server">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Credit Note" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Debit Note" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Extension Rental" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Extension AMF" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Other Cash Flow" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Interim Rental" Value="6"></asp:ListItem>
                            </asp:DropDownList>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlType" SetFocusOnError="True"
                                            ErrorMessage="Select Transaction Type" InitialValue="0" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="RFVDTransLander"></asp:RequiredFieldValidator>

                        </td>
                    </tr>

                    <%--Row 3 with 4 columns--%>
                    <tr class="styleFieldAlign">
                        <%--Document Number--%>
                        <td colspan="2" visible="false" runat="server" id="tdManual" width="10%">
                            <asp:RadioButton ID="rdoManual" AutoPostBack="true" CssClass="styleDisplayLabel"
                                OnCheckedChanged="Journal_OnCheckedChanged" Checked="true" runat="server"
                                GroupName="DocYes" Text="Manual Journal" />
                            <asp:RadioButton ID="rdoSystem" AutoPostBack="true" CssClass="styleDisplayLabel"
                                OnCheckedChanged="Journal_OnCheckedChanged"
                                GroupName="DocYes" runat="server" Text="System Journal" />
                        </td>

                        <td class="styleFieldLabel">
                            <asp:Label Text="" runat="server" ID="lblAutosuggestProgramIDSearch" CssClass="styleDisplayLabel" />
                            <asp:Label Text="Invoice No" runat="server" ID="lblODIInvoice" CssClass="styleDisplayLabel" Visible="false" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:Panel ID="panAutoSuggest" runat="server">
                                <asp:TextBox ID="txtDocumentNumberSearch" runat="server" MaxLength="50" OnTextChanged="txtDocumentNumberSearch_OnTextChanged"
                                    AutoPostBack="true" Width="182px"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="aceCommonCon" MinimumPrefixLength="1" OnClientPopulated="Common_ItemPopulated" OnClientItemSelected="Common_ItemSelected"
                                    runat="server" TargetControlID="txtDocumentNumberSearch" ServiceMethod="GetCommonList"
                                    CompletionSetCount="5" Enabled="True" ServicePath="" CompletionListCssClass="CompletionList"
                                    DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                </cc1:AutoCompleteExtender>
                                <cc1:TextBoxWatermarkExtender ID="txtWatermarkExtender" runat="server" TargetControlID="txtDocumentNumberSearch"
                                    WatermarkText="--Select--">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:HiddenField ID="hdnCommonID" runat="server" />
                            </asp:Panel>

                            <asp:TextBox ID="txtODIInvoice" runat="server" OnTextChanged="txtODIInvoice_OnTextChanged"
                                AutoPostBack="true" Width="182px" Visible="false"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="aceODIInvoice" MinimumPrefixLength="3" OnClientPopulated="ODIInvoice_ItemPopulated"
                                OnClientItemSelected="ODIInvoice_ItemSelected" runat="server" TargetControlID="txtODIInvoice"
                                ServiceMethod="GetInvoiceNo" Enabled="True" ServicePath="" CompletionSetCount="5"
                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtODIInvoice"
                                WatermarkText="--Select--">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:HiddenField ID="hdnODIInvoice" runat="server" />

                        </td>
                        <%--Added for Call Id : 3474 CR_057--%>
                        <td class="styleFieldLabel">
                            <asp:Label Text="Rental Schedule Number " runat="server" ID="lblRSNumber" CssClass="styleDisplayLabel" Visible="false" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:TextBox ID="txtRSNumber" runat="server" MaxLength="50" OnTextChanged="txtRSNumber_OnTextChanged"
                                    AutoPostBack="true" Width="182px" Visible="false"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="aceRSNumber" MinimumPrefixLength="1" OnClientPopulated="RS_ItemPopulated" OnClientItemSelected="RS_ItemSelected"
                                    runat="server" TargetControlID="txtRSNumber" ServiceMethod="GetRSList"
                                    CompletionSetCount="5" Enabled="True" ServicePath="" CompletionListCssClass="CompletionList"
                                    DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                </cc1:AutoCompleteExtender>
                                <cc1:TextBoxWatermarkExtender ID="txtWatermarkExtender1" runat="server" TargetControlID="txtRSNumber"
                                    WatermarkText="--Select--">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:HiddenField ID="hdnRSNumber" runat="server" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="styleFieldLabel">
                            <asp:Label Text="Invoice No" runat="server" ID="lblInvoice" CssClass="styleDisplayLabel" Visible="false" />
                            <asp:Label Text="Old Invoice No" runat="server" ID="lblOldInvoice" CssClass="styleDisplayLabel" Visible="false" />
                        </td>
                        <td class="styleFieldAlign">

                            <asp:TextBox ID="txtInvoice" runat="server" OnTextChanged="txtInvoice_OnTextChanged"
                                AutoPostBack="true" Width="182px" Visible="false"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="aceInvoice" MinimumPrefixLength="3" OnClientPopulated="Invoice_ItemPopulated"
                                OnClientItemSelected="Invoice_ItemSelected" runat="server" TargetControlID="txtInvoice"
                                ServiceMethod="GetInvoiceNo" Enabled="True" ServicePath="" CompletionSetCount="5"
                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtInvoice"
                                WatermarkText="--Select--">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:HiddenField ID="hdnInvoice" runat="server" />

                            <asp:TextBox ID="txtOldInvoice" runat="server" OnTextChanged="txtOldInvoice_OnTextChanged"
                                AutoPostBack="true" Width="182px" Visible="false"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" MinimumPrefixLength="3" OnClientPopulated="OldInvoice_ItemPopulated"
                                OnClientItemSelected="OldInvoice_ItemSelected" runat="server" TargetControlID="txtOldInvoice"
                                ServiceMethod="GetOldInvoiceNo" Enabled="True" ServicePath="" CompletionSetCount="5"
                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtOldInvoice"
                                WatermarkText="--Select--">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:HiddenField ID="hdnOldInvoice" runat="server" />
                        </td>
                        <td class="styleFieldLabel">
                            <asp:Label Text="Invoice No" runat="server" ID="lblDCNInvoice" CssClass="styleDisplayLabel" Visible="false" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtDCNInvoice" runat="server" OnTextChanged="txtDCNInvoice_OnTextChanged"
                                AutoPostBack="true" Width="182px" Visible="false"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="aceDCNInvoice" MinimumPrefixLength="3" OnClientPopulated="DCNInvoice_ItemPopulated"
                                OnClientItemSelected="DCNInvoice_ItemSelected" runat="server" TargetControlID="txtDCNInvoice"
                                ServiceMethod="GetInvoiceNo" Enabled="True" ServicePath="" CompletionSetCount="5"
                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                ShowOnlyCurrentWordInCompletionListItem="true">
                            </cc1:AutoCompleteExtender>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtDCNInvoice"
                                WatermarkText="--Select--">
                            </cc1:TextBoxWatermarkExtender>
                            <asp:HiddenField ID="hdnDCNInvoice" runat="server" />
                        </td>
                    </tr>
                    <%--Row 4 with 1 columns--%>
                    <tr width="100%">
                        <%--Spacer--%>
                        <td width="100%" colspan="4" align="center">&nbsp;
                        </td>
                    </tr>
                    <%--Row 5 with 1 columns--%>
                    <tr width="100%">
                        <%--Search Records--%>
                        <td width="100%" colspan="4" align="center">
                            <asp:Button Text="Search" ValidationGroup="RFVDTransLander" ID="btnSearch" runat="server"
                                CssClass="styleSubmitButton" UseSubmitBehavior="true"
                                OnClick="btnSearch_Click" ToolTip="Search" />
                            <asp:Button Text="Create" ID="btnCreate" runat="server" CssClass="styleSubmitButton"
                                UseSubmitBehavior="true" OnClick="btnCreate_Click" ToolTip="Create" />
                            <asp:Button Text="Clear" ID="btnClear" runat="server" CssClass="styleSubmitButton"
                                UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();"
                                OnClick="btnClear_Click" ToolTip="Clear" />
                            <asp:Button runat="server" ID="btnPrint" Text="Print" CausesValidation="false" OnClick="btnPrint_Click"
                                CssClass="styleSubmitButton" ToolTip="Print" />
                        </td>
                    </tr>
                    <%--Row 6 with 1 columns--%>
                    <tr width="100%">
                        <%--Spacer--%>
                        <td width="100%" colspan="4" align="center">&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <%--Row 7 with 1 columns--%>
        <tr width="100%">
            <%--Grid--%>
            <td width="100%" colspan="4" align="center">
                <asp:GridView runat="server" ID="grvTransLander" Width="100%" AutoGenerateColumns="true"
                    OnRowCommand="grvTransLander_RowCommand" HeaderStyle-CssClass="styleGridHeader"
                    RowStyle-HorizontalAlign="Left" OnRowDataBound="grvTransLander_RowDataBound">
                    <Columns>
                        <%--Query Column--%>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("ID") %>' CommandName="Query" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--Edit Column--%>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                    CommandArgument='<%# Bind("ID") %>' CommandName="Modify" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--Created by - User ID Column--%>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By") %>'></asp:Label>
                                <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Select" SortExpression="Select" ItemStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <td>Select All</td>
                                        <td>
                                            <asp:CheckBox ID="chkSelectAllRS" runat="server" onclick="javascript:fnSelectAll(this,'chkSelectRS');" />
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelectRS" runat="server" ToolTip="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
            </td>
        </tr>
        <%--Row 8 with 1 columns--%>
        <tr width="100%">
            <%--Grid--%>
            <td width="100%" colspan="4" align="center"></td>
        </tr>
        <%--Row 9 with 1 columns--%>
        <tr width="100%">
            <%--Spacer--%>
            <td width="100%" colspan="4" align="center">&nbsp;
            </td>
        </tr>
        <%--Row 10 with 1 columns--%>
        <%-- <tr width="100%">--%>
        <%--Search Records--%>
        <%-- <td width="100%" colspan="4" align="center">
                        <asp:Button Text="Show All" ID="btnShowAll" runat="server" CssClass="styleSubmitButton"
                            UseSubmitBehavior="true" OnClick="btnShowAll_Click" />
                    </td>
                </tr>--%>
        <%--Row 11 with 1 columns--%>
        <tr width="100%">
            <%--Error Message--%>
            <td width="100%" colspan="4" align="center">
                <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
            </td>
        </tr>
        <%--Row 12 with 1 columns--%>
        <tr>
            <td colspan="4">
                <%--Hidden fields for grid usage--%>
                <input type="hidden" id="hdnSortDirection" runat="server" />
                <input type="hidden" id="hdnSortExpression" runat="server" />
                <input type="hidden" id="hdnSearch" runat="server" />
                <input type="hidden" id="hdnOrderBy" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:ValidationSummary ValidationGroup="RFVDTransLander" ID="vsTransLander" runat="server"
                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                    ShowSummary="true" />
            </td>
        </tr>
    </table>
</asp:Content>
