<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3G_LAD_StockTransfer_View.aspx.cs" Inherits="LoanAdmin_S3G_LAD_StockTransfer_View" %>

<%--Ajax Control Toolkit--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%--Content--%>
<asp:content id="ContentTransLander" contentplaceholderid="ContentPlaceHolder1" runat="Server">
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
                            <%-- <asp:RequiredFieldValidator ID="RFVComboLOB" ValidationGroup="RFVDTransLander" InitialValue="-- Select --"
                                CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxLOBSearch"
                                SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>--%>
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
                            <asp:Label Text="Document Number" runat="server" ID="Label1" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:TextBox ID="txtDocumentNumber" runat="server" MaxLength="50" OnTextChanged="txtDocumentNumber_OnTextChanged"
                                    AutoPostBack="true" Width="200px"></asp:TextBox>
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
                            <asp:Label Text="RS Number" runat="server" ID="lblAutosuggestProgramIDSearch" CssClass="styleDisplayLabel" />
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
                            <asp:Label Text="Invoice No" runat="server" ID="lblTrancheName" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:TextBox ID="txtTrancheName" runat="server" OnTextChanged="txtTrancheName_TextChanged"
                                AutoPostBack="true" Width="200px"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="acexTranche" MinimumPrefixLength="3" OnClientPopulated="Tranche_ItemPopulated"
                                OnClientItemSelected="Tranche_ItemSelected" runat="server" TargetControlID="txtTrancheName"
                                ServiceMethod="GetInvoiceNoList" Enabled="True" ServicePath="" CompletionSetCount="5"
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
                    <tr>

                        <td class="styleFieldLabel">
                            <asp:Label Text="Transfer Type" runat="server" ID="lblTransferType"
                                CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <asp:DropDownList ID="ddlTransferType" runat="server" Width="182px">
                                <asp:ListItem Value="0" Text="--All--" />
                                <asp:ListItem Value="1" Text="Stock Transfer" />
                                <asp:ListItem Value="2" Text="Delivery Challan" />
                            </asp:DropDownList>
                        </td>
                        <%-- Added for RS status code starts --%>
                        <td class="styleFieldLabel">
                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleDisplayLabel" ToolTip="RS Status">
                            </asp:Label>
                        </td>
                        <td class="styleFieldAlign">
                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" Width="55%" ToolTip="RS Status">
                                <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                                <asp:ListItem Text="RS configured" Value="47"></asp:ListItem>
                                <asp:ListItem Text="RS Activated" Value="46"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <%-- Added for RS status code ends --%>
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
                    <Columns>
                        <%--Query Column--%>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("Stock_Hdr_Id") %>' CommandName="Query" runat="server" />
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
                                    CommandArgument='<%# Bind("Stock_Hdr_Id") %>' CommandName="Modify" runat="server" />
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

        <tr width="100%">
            <td width="100%" align="center">
                
				<asp:Button runat="server" ID="btnExport" CssClass="styleSubmitButton" Text="Export" CausesValidation="false" OnClick="btnExport_Click" 
				Visible="false" ToolTip="Export" />

				<asp:Button runat="server" ID="btnPrint" Text="Print" CausesValidation="false" OnClick="btnPrint_Click"
                    CssClass="styleSubmitButton" ToolTip="Print"/> 
			</td>
        </tr>

        <tr>
            <td>
                <asp:Panel GroupingText="Print Details" ID="pnlPrintDetails" runat="server" CssClass="stylePanel" Visible="false">
                    <table width="99%">
                        <tr>
                            <td style="width: 10%" class="styleFieldLabel">
                                <asp:Label ID="lblPrintType" runat="server" Text="Print Type" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td style="width: 15%">
                                <asp:DropDownList ID="ddlPrintType" runat="server">
                                    <asp:ListItem Value="P" Text="PDF"></asp:ListItem>
                                    <asp:ListItem Value="W" Text="WORD"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>

        <tr width="100%">
            <td width="100%" colspan="4" align="center">
                
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
</asp:content>
