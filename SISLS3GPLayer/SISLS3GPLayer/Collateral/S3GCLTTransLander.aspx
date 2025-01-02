<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GCLTTransLander.aspx.cs" Inherits="Collateral_S3GCLTTransLander"
    Title="Untitled Page" %>

<%--Ajax Control Toolkit--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%--Content--%>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="ContentTransLander" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />
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
                        <td width="100%" colspan="4" align="center">
                            &nbsp;
                        </td>
                    </tr>
                    <%--Row 1 with 4 columns--%>
                    <tr width="100%">
                        <%--Line of Business--%>
                        <td width="15%" class="styleFieldLabel">
                            <asp:Label runat="server" Text="Line of Business" ID="lblLOBSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td width="35%" align="left" class="styleFieldAlign">
                            <cc1:ComboBox ID="ComboBoxLOBSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxLOBSearch_SelectedIndexChanged">
                            </cc1:ComboBox>
                            <asp:RequiredFieldValidator ID="RFVComboLOB" ValidationGroup="RFVDTransLander" InitialValue="-- Select --"
                                CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxLOBSearch"
                                SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <%--Start Date--%>
                        <td width="10%" class="styleFieldLabel">
                            <asp:Label runat="server" Text="Start Date" ID="lblStartDateSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td width="25%" align="left" class="styleFieldAlign">
                            <input id="hidDate" type="hidden" runat="server" />
                            <asp:TextBox ID="txtStartDateSearch" runat="server" Width="150px" AutoPostBack="True"
                                OnTextChanged="txtStartDateSearch_TextChanged"></asp:TextBox>
                            <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            <cc1:CalendarExtender ID="CalendarExtenderStartDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVStartDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="txtStartDateSearch" SetFocusOnError="True"
                                ErrorMessage="Enter a Start Date" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%--Row 2 with 4 columns--%>
                    <tr>
                        <%--Branch--%>
                        <td class="styleFieldLabel">
                            <asp:Label Text="Location" runat="server" ID="lblBranchSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td class="styleFieldAlign">
                            <%--<cc1:ComboBox ID="ComboBoxBranchSearch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxBranchSearch_SelectedIndexChanged">
                            </cc1:ComboBox>--%>
                            <uc2:Suggest ID="ComboBoxBranchSearch" runat="server" ServiceMethod="GetBranchList" />
                            <asp:RequiredFieldValidator ID="RFVComboBranch" ValidationGroup="RFVDTransLander"
                                InitialValue="-- Select --" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="txtStartDateSearch"
                                SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <%--End Date--%>
                        <td width="10%" class="styleFieldLabel">
                            <asp:Label runat="server" ID="lblEndDateSearch" Text="End Date" CssClass="styleDisplayLabel" />
                        </td>
                        <td width="25%" align="left" class="styleFieldAlign">
                            <asp:TextBox ID="txtEndDateSearch" runat="server" Width="150px" AutoPostBack="True"
                                OnTextChanged="txtEndDateSearch_TextChanged"></asp:TextBox>
                            <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            <cc1:CalendarExtender ID="CalendarExtenderEndDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgEndDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                TargetControlID="txtEndDateSearch">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVEndDate" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="txtEndDateSearch" SetFocusOnError="True" ErrorMessage="Enter a End Date"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <%--Row 3 with 4 columns--%>
                    <tr width="100%">
                        <%--Multiple Document Number --%>
                        <td width="15%" class="styleFieldLabel">
                            <asp:Label Visible="false" Text="Transaction" runat="server" ID="lblMultipleDNC"
                                CssClass="styleDisplayLabel" />
                        </td>
                        <td width="35%" valign="baseline" align="left" class="styleFieldAlign">
                            <asp:DropDownList Visible="false" AutoPostBack="true" ID="ddlMultipleDNC" runat="server"
                                Width="185px" OnSelectedIndexChanged="ddlMultipleDNC_SelectedIndexChanged">
                            </asp:DropDownList>
                            
                        </td>
                        <td width="10%" class="styleFieldLabel">
                            <asp:Label Visible="false" Text="Select Status" runat="server" ID="lblDNCOption"
                                CssClass="styleDisplayLabel" />
                        </td>
                        <td width="25%" class="styleFieldAlign">
                            <asp:DropDownList Visible="false" ID="ddlDNCOption" runat="server" Width="175px"
                                OnSelectedIndexChanged="ddlDNCOption_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%--Row 3 with 4 columns--%>
                    <tr width="100%">
                        <%--Document Number--%>
                        <td width="15%" class="styleFieldLabel">
                            <asp:Label Text="" runat="server" ID="lblProgramIDSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td width="35%" align="left" class="styleFieldAlign">
                           <%-- <cc1:ComboBox ID="cmbDocumentNumberSearch" runat="server" AutoCompleteMode="SuggestAppend"
                                onkeyup="maxlengthfortxt(113)" DropDownStyle="DropDownList" MaxLength="0" CssClass="WindowsStyle">
                            </cc1:ComboBox>--%>
                              <uc2:Suggest ID="cmbDocumentNumberSearch" runat="server" ServiceMethod="GetDocumentNumber" />
                        </td>
                        <td width="10%">
                        </td>
                        <td width="25%">
                        </td>
                    </tr>
                    <tr>
                        <td class="styleFieldLabel">
                            <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Code" CssClass="styleDisplayLabel"
                                Visible="false">
                            </asp:Label>
                        </td>
                        <td class="styleFieldAlign">
                            <uc2:LOV ID="ucCustomerCodeLov" onblur="fnLoadCustomer()" runat="server" strLOV_Code="CMB"
                                DispalyContent="Code" />
                            <a href="#" onclick="window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromApplication=Yes&NewCustomerID=0', 'newwindow','toolbar=no,location=no,menubar=no,width=950,height=600,resizable=no,scrollbars=yes,top=150,left=100');return false;">
                                <%-- <asp:Panel ID="disp" runat="server" Height="300px" ScrollBars="Vertical" Style="display: none" />
                            <asp:TextBox ID="cmbCustomerCode" runat="server" MaxLength="50" AutoPostBack="true"
                                OnTextChanged="cmbCustomerCode_TextChanged" CssClass="styleSearchBox" ToolTip="Customer Code"
                                Width="180px" Visible="false"></asp:TextBox>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtenderCust" runat="server" TargetControlID="cmbCustomerCode"
                                ServiceMethod="GetCustomerList" MinimumPrefixLength="1" CompletionSetCount="5"
                                CompletionInterval="1" DelimiterCharacters="" Enabled="True" ServicePath="" FirstRowSelected="true"
                                CompletionListElementID="disp">
                            </cc1:AutoCompleteExtender>--%>
                        </td>
                        <td width="10%">
                        </td>
                        <td width="25%">
                        </td>
                    </tr>
                    <%--Row 4 with 1 columns--%>
                    <tr width="100%">
                        <%--Spacer--%>
                        <td width="100%" colspan="4" align="center">
                            &nbsp;
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
                            <%--<asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                                OnClick="btnLoadCustomer_OnClick" />--%>
                        </td>
                    </tr>
                    <%--Row 6 with 1 columns--%>
                    <tr width="100%">
                        <%--Spacer--%>
                        <td width="100%" colspan="4" align="center">
                            &nbsp;
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
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <%--Row 8 with 1 columns--%>
        <tr width="100%">
            <%--Grid--%>
            <td width="100%" colspan="4" align="center">
                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
            </td>
        </tr>
        <%--Row 9 with 1 columns--%>
        <tr width="100%">
            <%--Spacer--%>
            <td width="100%" colspan="4" align="center">
                &nbsp;
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
