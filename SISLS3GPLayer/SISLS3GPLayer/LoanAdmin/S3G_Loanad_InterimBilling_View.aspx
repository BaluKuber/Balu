<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3G_Loanad_InterimBilling_View.aspx.cs" Inherits="LoanAdmin_S3G_Loanad_InterimBilling_View" %>

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

        function Tranche_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnTranche.ClientID %>');
            hdnLessee.value = e.get_value();
        }

        function Tranche_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnTranche.ClientID %>');
            hdnLessee.value = '';
        }

        function Doc_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnDocNo.ClientID %>');
            hdnLessee.value = e.get_value();
        }

        function Doc_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnDocNo.ClientID %>');
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
                    <tr class="styleFieldLabel">
                        <td class="styleFieldLabel">
                            <asp:Label Text="Lessee Name" runat="server" ID="lblLesseeNameSrch" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtLessee" runat="server" OnTextChanged="txtLessee_OnTextChanged"
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
                            <asp:Label Text="Tranche Name" runat="server" ID="lblTrancheName" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtTrancheName" runat="server" OnTextChanged="txtTrancheName_TextChanged"
                                AutoPostBack="true" Width="200px"></asp:TextBox>
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

                        </td>
                    </tr>
                    <tr class="styleFieldAlign">
                        <td class="styleFieldLabel">
                            <asp:Label Text="Rental Schedule Number" runat="server" ID="lblAutosuggestProgramIDSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:Panel ID="panAutoSuggest" runat="server">
                                <asp:TextBox ID="txtDocumentNumberSearch" runat="server" MaxLength="50" OnTextChanged="txtDocumentNumberSearch_OnTextChanged"
                                    AutoPostBack="true" Width="182px"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="aceCommonCon" MinimumPrefixLength="1" OnClientPopulated="Common_ItemPopulated" OnClientItemSelected="Common_ItemSelected"
                                    runat="server" TargetControlID="txtDocumentNumberSearch" ServiceMethod="GetRSList"
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
                        </td>
                        <td class="styleFieldLabel">
                            <asp:Label Text="Document Number" runat="server" ID="Label1" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:TextBox ID="txtDocumentNumber" runat="server" MaxLength="50" OnTextChanged="txtDocumentNumber_OnTextChanged"
                                    AutoPostBack="true" Width="182px"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" MinimumPrefixLength="1" OnClientPopulated="Doc_ItemPopulated" OnClientItemSelected="Doc_ItemSelected"
                                    runat="server" TargetControlID="txtDocumentNumber" ServiceMethod="GetDocumentList"
                                    CompletionSetCount="5" Enabled="True" ServicePath="" CompletionListCssClass="CompletionList"
                                    DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                </cc1:AutoCompleteExtender>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtDocumentNumber"
                                    WatermarkText="--Select--">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:HiddenField ID="hdnDocNo" runat="server" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr class="styleFieldAlign">
                        <td class="styleFieldLabel">
                            <asp:Label Text="Invoice No" runat="server" ID="lblInvoice" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:Panel ID="Panel2" runat="server">
                                <asp:TextBox ID="txtInvoice" runat="server" MaxLength="50" OnTextChanged="txtInvoice_OnTextChanged"
                                    AutoPostBack="true" Width="182px"></asp:TextBox>
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" MinimumPrefixLength="1" OnClientPopulated="Invoice_ItemPopulated" OnClientItemSelected="Invoice_ItemSelected"
                                    runat="server" TargetControlID="txtInvoice" ServiceMethod="GetInvoiceNo"
                                    CompletionSetCount="5" Enabled="True" ServicePath="" CompletionListCssClass="CompletionList"
                                    DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                </cc1:AutoCompleteExtender>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtInvoice"
                                    WatermarkText="--Select--">
                                </cc1:TextBoxWatermarkExtender>
                                <asp:HiddenField ID="hdnInvoice" runat="server" />
                            </asp:Panel>
                        </td>
                        </tr>
                    <tr width="100%">
                        <td width="100%" colspan="4" align="center">&nbsp;
                        </td>
                    </tr>
                    <tr width="100%">
                        <td width="100%" colspan="4" align="center">
                            <asp:Button Text="Search" ValidationGroup="RFVDTransLander" ID="btnSearch" runat="server"
                                CssClass="styleSubmitButton" UseSubmitBehavior="true"
                                OnClick="btnSearch_Click" ToolTip="Search" />
                            <asp:Button Text="Create" ID="btnCreate" runat="server" CssClass="styleSubmitButton"
                                UseSubmitBehavior="true" OnClick="btnCreate_Click" ToolTip="Create" />
                            <asp:Button Text="Clear" ID="btnClear" runat="server" CssClass="styleSubmitButton"
                                UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();"
                                OnClick="btnClear_Click" ToolTip="Clear" />
                        </td>
                    </tr>
                    <tr width="100%">
                        <td width="100%" colspan="4" align="center">&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr width="100%">
            <td width="100%" colspan="4" align="center">
                <asp:GridView runat="server" ID="grvTransLander" Width="100%" AutoGenerateColumns="true"
                    OnRowCommand="grvTransLander_RowCommand" HeaderStyle-CssClass="styleGridHeader"
                    RowStyle-HorizontalAlign="Left" OnRowDataBound="grvTransLander_RowDataBound">
                </asp:GridView>
                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
            </td>
        </tr>

        <tr width="100%">
            <td width="100%" colspan="4" align="center">&nbsp;
            </td>
        </tr>

        <tr width="100%">
            <td width="100%" colspan="4" align="center">
            <asp:Button runat="server" ID="btnPrintRental" Text="Print Rental" CausesValidation="false" OnClick="btnPrintRental_Click"
                CssClass="styleSubmitButton" />
            <asp:Button runat="server" ID="btnPrintAMF" Text="Print AMF" CausesValidation="false" OnClick="btnPrintAMF_Click"
                CssClass="styleSubmitButton" />
            </td>
        </tr>
        <tr width="100%">
            <td width="100%" colspan="4" align="center">&nbsp;
            </td>
        </tr>

        <tr width="100%">

            <td width="100%" colspan="4" align="center">
                <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
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
