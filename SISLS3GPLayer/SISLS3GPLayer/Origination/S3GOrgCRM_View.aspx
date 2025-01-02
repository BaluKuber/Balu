<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GOrgCRM_View.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    Inherits="Origination_S3GORGTransLander" %>

<%--Ajax Control Toolkit--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/S3GMultiSelect.ascx" TagName="MultiSelect" TagPrefix="ucms" %>
<%@ Register Src="~/UserControls/S3GDynamicLOV.ascx" TagName="FLOV" TagPrefix="uc" %>
<%--Content--%>
<asp:Content ID="ContentTransLander" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />
    <%--Design Started--%>

    <script type="text/javascript">
        function Branch_ItemSelected(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
            hdnBranchID.value = e.get_value();
        }
        function Branch_ItemPopulated(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
            hdnBranchID.value = '';
        }

        function funLoadUsers() {
            document.getElementById('<%= btnGetUsers.ClientID %>').click();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
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
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOBSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <cc1:ComboBox ID="ComboBoxLOBSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                        runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                        MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxLOBSearch_SelectedIndexChanged">
                                    </cc1:ComboBox>
                                    <asp:RequiredFieldValidator ID="RFVComboLOB" ValidationGroup="RFVDTransLander" InitialValue="-- Select --"
                                        CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxLOBSearch"
                                        Enabled="false" SetFocusOnError="True" Display="None" ErrorMessage="Select the Line of Business"></asp:RequiredFieldValidator>
                                </td>
                                <%--Start Date--%>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Start Date" ID="lblStartDateSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <input id="hidDate" type="hidden" runat="server" />
                                    <asp:TextBox ID="txtStartDateSearch" runat="server" Width="150px"></asp:TextBox>
                                    <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender ID="CalendarExtenderStartDateSearch" runat="server" Enabled="True"
                                        PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RFVStartDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="txtStartDateSearch" SetFocusOnError="True"
                                        Enabled="false" ErrorMessage="Enter a Start Date" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--Row 2 with 4 columns--%>
                            <tr width="100%">
                                <%--Branch--%>
                                <td class="styleFieldLabel">
                                    <asp:Label Text="Location" runat="server" ID="lblBranchSearch" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <%-- <cc1:ComboBox ID="ComboBoxBranchSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                        runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                        MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxBranchSearch_SelectedIndexChanged">
                                    </cc1:ComboBox>
                                    <asp:RequiredFieldValidator ID="RFVComboBranch" ValidationGroup="RFVDTransLander"
                                        InitialValue="-- Select --" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxBranchSearch"
                                        SetFocusOnError="True" Display="None" ErrorMessage="Select the Location"></asp:RequiredFieldValidator>--%>
                                    <asp:TextBox ID="txtBranchSearch" runat="server" MaxLength="50" Width="182px"></asp:TextBox>
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
                                        Enabled="false" InitialValue="" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="txtBranchSearch" SetFocusOnError="True" Display="None" ErrorMessage="Select the Location"></asp:RequiredFieldValidator>
                                </td>
                                <%--End Date--%>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" ID="lblEndDateSearch" Text="End Date" CssClass="styleDisplayLabel" />
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtEndDateSearch" runat="server" Width="150px"></asp:TextBox>
                                    <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender ID="CalendarExtenderEndDateSearch" runat="server" Enabled="True"
                                        PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RFVEndDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="txtEndDateSearch" SetFocusOnError="True" ErrorMessage="Enter a End Date"
                                        Enabled="false" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                                <td colspan="2" class="styleFieldAlign">
                                    <cc1:DropShadowExtender ID="dseCustomer" runat="server" TargetControlID="pnlSearchMore"
                                        Opacity=".2" Rounded="false" TrackPosition="true" Enabled="true" />
                                    <asp:Panel ID="pnlSearchMore" runat="server" Style="display: none; position: absolute"
                                        Width="350px" BackColor="White">
                                        <table width="100%" class="styleMainTable" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="stylePageHeading" width="100%" style="height: 22px">
                                                    <asp:Label runat="server" ID="Label1" CssClass="styleDisplayLabel" Text="More Search Criteria"></asp:Label>

                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/delete1.png" ToolTip="Close Search Option"
                                                        OnClick="imgClose_Click" Width="20px" Height="20px" Style="vertical-align: top; text-align: right" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label Text="User Side" runat="server" ID="lblUserSide" CssClass="styleDisplayLabel" />
                                                            </td>
                                                            <td>
                                                                <ucms:MultiSelect runat="server" ID="ddlUserSide" Width="60px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label Text="User Name" runat="server" ID="lblUserName" CssClass="styleDisplayLabel" />
                                                                <uc:FLOV ID="popUserName" runat="server" LOVCode="CRMUM" OnLoadCusotmer="UserSelected_Click" />
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" class="styleTableData" width="160px" runat="server"
                                                                                id="tblInner">
                                                                                <tr>
                                                                                    <td style="height: 16px">
                                                                                        <asp:TextBox ID="txtUserName" runat="server" Width="160px" Style="border: none; height: 14px; cursor: default;"
                                                                                            Text="--Select--"></asp:TextBox>
                                                                                    </td>
                                                                                    <td valign="middle" height="10px" align="right" onclick="funLoadUsers()">
                                                                                        <fieldset id="fldButton" runat="server" class="accordionHeaderSelected" style="cursor: pointer; height: 7px; width: 13px; border-width: 1px; border-color: #bad4ff; border-style: solid">
                                                                                            <asp:Image ID="imgShowList" runat="server" ImageUrl="../Images/search_blue.gif" Style="cursor: pointer; height: 14px; vertical-align: top; margin-top: -3px" />
                                                                                        </fieldset>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <asp:Button ID="btnGetUsers" Style="display: none" runat="server" Text="..." CausesValidation="true"
                                                                                OnClick="btnGetUser_Click" />
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:CheckBox ID="chkDefault" runat="server" AutoPostBack="true" OnCheckedChanged="chkDefault_CheckedChanged" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label Text="Task Status" runat="server" ID="Label2" CssClass="styleDisplayLabel" />
                                                            </td>
                                                            <td>
                                                                <ucms:MultiSelect runat="server" ID="ddlTaskStatus" Width="100px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label Text="Query Type" runat="server" ID="Label3" CssClass="styleDisplayLabel" />
                                                            </td>
                                                            <td>
                                                                <ucms:MultiSelect runat="server" ID="ddlQueryType" Width="120px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-right: 10px; padding-bottom: 10px">
                                                    <br />
                                                    <asp:Button ID="btnSearchMoreOk" runat="server" CssClass="styleGridShortButton" Style="float: right;"
                                                        ToolTip="Search" Text="Search" OnClick="btnSearchMoreOk_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Button ID="btnMore" runat="server" Text="More" CssClass="styleGridShortButton"
                                        Style="float: left" OnClick="btnMore_Click" />
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
                                        CssClass="styleSubmitButton" UseSubmitBehavior="true" OnClick="btnSearch_Click" />
                                    <asp:Button Text="Create" ID="btnCreate" runat="server" CssClass="styleSubmitButton"
                                        UseSubmitBehavior="true" OnClick="btnCreate_Click" />
                                    <asp:Button Text="Clear" ID="btnClear" runat="server" CssClass="styleSubmitButton"
                                        UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
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
                    <td width="100%" align="center">
                        <asp:GridView runat="server" ID="grvTransLander" Width="100%" AutoGenerateColumns="true"
                            HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Left">
                        </asp:GridView>
                    </td>
                </tr>
                <%--Row 8 with 1 columns--%>
                <tr width="100%">
                    <%--Grid--%>
                    <td width="100%" align="center">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <%--Row 9 with 1 columns--%>
                <tr width="100%">
                    <%--Spacer--%>
                    <td width="100%" align="center">&nbsp;
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
                    <td width="100%" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <%--Row 12 with 1 columns--%>
                <tr>
                    <td>
                        <%--Hidden fields for grid usage--%>
                        <input type="hidden" id="hdnSortDirection" runat="server" />
                        <input type="hidden" id="hdnSortExpression" runat="server" />
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ValidationGroup="RFVDTransLander" ID="vsTransLander" runat="server"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
