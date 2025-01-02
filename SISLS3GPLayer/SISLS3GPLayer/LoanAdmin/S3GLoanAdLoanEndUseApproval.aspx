<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GLoanAdLoanEndUseApproval.aspx.cs" Inherits="LoanAdmin_S3GLoanAdLoanEndUseApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">

        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_tcLoanEndUse_tbgeneral_btnCreateCustomer').click();
        }

        function User_ItemSelected(sender, e) {

            var hdnUsrID = $get('<%= hdnUserId.ClientID %>');
            hdnUsrID.value = e.get_value();
        }

        function User_ItemPopulated(sender, e) {
            var hdnUsrID = $get('<%= hdnUserId.ClientID %>');
            hdnUsrID.value = '';
        }

    </script>

    <table width="100%">
        <tr width="100%">
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Loan End Use Approval" ID="lblLoanEndUseApproval"
                    CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
    </table>
    <cc1:TabContainer ID="tcLoanEndUse" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
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
                                        <asp:Panel ID="pnlLobInfo" runat="server" ToolTip="Loan End Use Approval Information"
                                            GroupingText="Loan End Use Approval Information" CssClass="stylePanel">
                                            <div id="div1" runat="server" style="height: 250px; width: 100%; overflow-x: hidden; overflow-y: auto;">
                                                <asp:Panel ID="pnlAccountInfo" Width="98%" runat="server" CssClass="stylePanel">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr width="100%">
                                                            <td align="left" class="styleFieldLabel" valign="middle" style="width: 18%">
                                                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"
                                                                    ToolTip="Line of Business"></asp:Label>
                                                            </td>
                                                            <td align="left" class="styleFieldAlign" style="width: 28%">
                                                                <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator Display="None" ID="RFVddlLOB" CssClass="styleMandatoryLabel"
                                                                    SetFocusOnError="True" runat="server" ControlToValidate="ddlLOB" InitialValue="0"
                                                                    ErrorMessage="Select Line of Business" ValidationGroup="vsLoanApproval"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" Text="Location" ToolTip="Location" ID="lblBranch" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td align="left" class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlBranch" runat="server" ToolTip="Location" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator Display="None" ID="RFVddlBranch" CssClass="styleMandatoryLabel"
                                                                    SetFocusOnError="True" runat="server" ControlToValidate="ddlBranch" InitialValue="0"
                                                                    ErrorMessage="Select Location" ValidationGroup="vsLoanApproval"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Prime Account Number" ID="lblprimeaccno" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlPAN" runat="server" AutoPostBack="True" ToolTip="Prime A/c Number"
                                                                    OnSelectedIndexChanged="ddlPAN_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RFVPAN" CssClass="styleMandatoryLabel" runat="server"
                                                                    ControlToValidate="ddlPAN" InitialValue="0" ValidationGroup="vsLoanApproval"
                                                                    ErrorMessage="Select Prime Account Number" Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" Text="End Use Number" ToolTip="End Use Number" ID="lblEndUseNumber"
                                                                    CssClass="styleDisplayLabel" Visible="False"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" align="left">
                                                                <asp:TextBox ID="txtEndUseNumber" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" Text="End Use Date" ToolTip="End Use Date" ID="lblEndUseDate"
                                                                    CssClass="styleDisplayLabel" Visible="False"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" align="left">
                                                                <asp:TextBox ID="txtEndUseDate" ToolTip="End Use Date" Width="35%" Enabled="False"
                                                                    runat="server" Visible="False"></asp:TextBox>
                                                                <asp:Image ID="imgEndUseDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="False" />
                                                                <cc1:CalendarExtender ID="CalendarExtenderEndUseDate" runat="server" Enabled="True"
                                                                    PopupButtonID="imgEndUseDate" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                                    TargetControlID="txtEndUseDate">
                                                                </cc1:CalendarExtender>
                                                                &nbsp;&nbsp;&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" Text="Amount Utilized" ToolTip="Amount Utilized" ID="lblAmountUtilized"
                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td align="left" class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlAmountUtilized" runat="server" ToolTip="Amount Utilized" AutoPostBack="True" OnSelectedIndexChanged="ddlAmountUtilized_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvAmountUtilized" CssClass="styleMandatoryLabel" runat="server"
                                                                    ControlToValidate="ddlAmountUtilized" InitialValue="0" ValidationGroup="vsLoanApproval"
                                                                    ErrorMessage="Select Amount Utilized" Display="None"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="styleFieldLabel" valign="middle">
                                                                <asp:Label runat="server" Text="Components" ToolTip="Components" ID="lblComponents"
                                                                    CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" align="left">
                                                                <asp:TextBox ID="txtComponents" ToolTip="Components" Width="80%" TextMode="MultiLine"
                                                                    onkeyup="maxlengthfortxt(300);" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="PnlCustEntityInformation1" runat="server" ToolTip="Customer/Entity Information"
                                            GroupingText="Payee Informations" CssClass="stylePanel">
                                            <div id="divcustomerentity" runat="server" style="height: 250px; width: 100%; overflow-x: hidden; overflow-y: auto;">
                                                <asp:Panel ID="PnlCustEntityInformation" Width="95%" ToolTip="Customer or Entity Informations"
                                                    runat="server" CssClass="stylePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label runat="server" ToolTip="Cutomer or Entity" Text="Customer/Entity" ID="lblCode"
                                                                    CssClass="styleMandatoryLabel"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtCustomerCode" ToolTip="Cutomer or Entity code" runat="server"
                                                                    Style="display: none;" MaxLength="50"></asp:TextBox>
                                                                <uc2:LOV ID="ucCustomerCodeLov" onblur="fnLoadCustomer()" runat="server" strLOV_Code="CMD"
                                                                    DispalyContent="Code" />
                                                                <asp:Button ID="btnCreateCustomer" OnClick="btnCreateCustomer_Click"
                                                                    runat="server" Text="Create" Style="display: none;" CssClass="styleSubmitShortButton"
                                                                    CausesValidation="False" />
                                                                <asp:RequiredFieldValidator ID="rfvcmbCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                    ErrorMessage="Select a Customer/Entity" ValidationGroup="vsLoanApproval" CssClass="styleMandatoryLabel"
                                                                    Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
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
                        <td></td>
                    </tr>
                    <tr width="99%" align="center">
                        <td width="100%" align="center">
                            <asp:Panel ID="pnlPaymentDetails" ToolTip="Payment Details" Width="100%" runat="server"
                                GroupingText="Payment Details" CssClass="stylePanel" Visible="False">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:GridView runat="server" ID="grvPaymentDetails" Width="100%"
                                                ToolTip="Payment Details" AutoGenerateColumns="False" EnableModelValidation="True">
                                                <RowStyle HorizontalAlign="Center" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Outflow ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOutFlowID" ToolTip="OutFlow ID" Width="100%" runat="server" Text='<%#Eval("OutFlow_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle Width="15%" />
                                                        <HeaderStyle CssClass="styleGridHeader" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stage ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStageID" ToolTip="Stage No" Width="100%" runat="server" Text='<%#Eval("Stage_No")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle Width="15%" />
                                                        <HeaderStyle CssClass="styleGridHeader" Width="15%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stage Description">
                                                        <ItemTemplate>
                                                            <%--<asp:LinkButton ID="lnkbtnStageId" ToolTip="Pay Type" Width="100%" runat="server"
                                                                Text='<%#Eval("Stage_No")%>' OnClick="lnkbtnStageId_Click" Visible="false"></asp:LinkButton>--%>
                                                            <asp:Label ID="lblStageDesc" ToolTip="Stage Description" Width="100%" runat="server" Text='<%#Eval("Stage_Description")%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle Width="10%" />
                                                        <HeaderStyle CssClass="styleGridHeader" Width="10%" />
                                                        <ItemStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment Due Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPaymentDueDate" ToolTip="Payment Due Date" Width="100%" runat="server"
                                                                Text='<%#Eval("Payment_Due_Date")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle Width="15%" />
                                                        <HeaderStyle CssClass="styleGridHeader" Width="20%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stage Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStageAmount" ToolTip="Stage Amount" Width="100%" runat="server"
                                                                Text='<%#Eval("Stage_amount")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle Width="15%" />
                                                        <HeaderStyle CssClass="styleGridHeader" Width="20%" />
                                                        <ItemStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Paid Till Now">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPaidTillNow" ToolTip="Paid Till Now" Width="100%" runat="server"
                                                                Text='<%#Eval("Paid_Amount")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle Width="1%" />
                                                        <HeaderStyle CssClass="styleGridHeader" Width="1%" />
                                                        <ItemStyle Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Paid Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPaidDate" ToolTip="Paid Date" Width="100%" runat="server" Text='<%#Eval("Paid_Date")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle Width="1%" />
                                                        <HeaderStyle CssClass="styleGridHeader" Width="1%" />
                                                        <ItemStyle Width="20%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <HeaderStyle CssClass="styleGridHeader" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="Approval Details" ID="tbApprovalDetails"
            CssClass="tabpan" BackColor="Red" ToolTip="Existing PDC" Width="99%">
            <HeaderTemplate>
                Approval Details
            </HeaderTemplate>
            <ContentTemplate>
                <asp:UpdatePanel ID="UpapprovalDetails" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlLoanApprovedDtls" ToolTip="Payment Approved Details" Width="100%"
                            runat="server" GroupingText="Payment Approved Details" CssClass="stylePanel"
                            Visible="true">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView runat="server" ShowFooter="true" ID="grvPaymentApprovedDetails" Width="100%"
                                            ToolTip="Payment Approved Details" Visible="true" RowStyle-HorizontalAlign="Center"
                                            HeaderStyle-CssClass="styleGridHeader" FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="False" OnRowDataBound="grvPaymentApprovedDetails_RowDataBound">
                                            <RowStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <%-- Approval ID --%>
                                                <asp:TemplateField HeaderText="Approval ID" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="5%" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprovalID" ToolTip="Stage ID" Width="100%" runat="server" Text='<%#Eval("Approval_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterApprovalId" ToolTip="Approval ID" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="5%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:TemplateField>
                                                <%-- Stage ID --%>
                                                <asp:TemplateField HeaderText="Stage ID" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="5%" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStageID" ToolTip="Stage ID" Width="100%" runat="server" Text='<%#Eval("Stage_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterStageId" ToolTip="Stage ID" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="5%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:TemplateField>
                                                <%-- Stage Name --%>
                                                <asp:TemplateField HeaderText="Stage Name" ItemStyle-HorizontalAlign="Left" FooterStyle-Width="8%"
                                                    ItemStyle-Width="8%" HeaderStyle-Width="8%" FooterStyle-Wrap="true" ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStageName" ToolTip="Stage Name" Width="100%" runat="server" Text='<%#Eval("Stage_Name")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterStageName" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator Display="None" ID="RFVFooterStageName" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlFooterStageName" InitialValue="0"
                                                            ErrorMessage="Select Stage Name" ValidationGroup="btnAdd"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="8%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="8%" Wrap="true" />
                                                </asp:TemplateField>
                                                <%-- Approved Amount --%>
                                                <asp:TemplateField HeaderText="Approved Amount" FooterStyle-Width="12%" ItemStyle-Width="12%"
                                                    HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprovedAmount" ToolTip="Approved Amount" Width="100%" runat="server"
                                                            Text='<%#Eval("Approved_Amount")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtApprovedAmount" runat="server" Width="90%" MaxLength="15"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTEApprovedAmount" runat="server" TargetControlID="txtApprovedAmount"
                                                            FilterType="Numbers" Enabled="true">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator Display="None" ID="rfvApprovedAmount" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True" ValidationGroup="btnAdd" runat="server" ControlToValidate="txtApprovedAmount"
                                                            ErrorMessage="Enter Amount to be Approved"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="15%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                                </asp:TemplateField>
                                                <%-- User ID --%>
                                                <asp:TemplateField HeaderText="User ID" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="5%" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApproverID" ToolTip="Stage ID" Width="100%" runat="server" Text='<%#Eval("Approvered_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFooterApproverID" ToolTip="Stage ID" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="5%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:TemplateField>
                                                <%-- Approved By --%>
                                                <asp:TemplateField HeaderText="Approved By" FooterStyle-Width="15%" ItemStyle-Width="15%"
                                                    Visible="True" HeaderStyle-Width="15%" FooterStyle-Wrap="true" ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprovedBy" ToolTip="Approved By" Width="100%" runat="server" Text='<%#Eval("Approved_By")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtApprovedBy" runat="server" Width="90%"></asp:TextBox>
                                                        <cc1:AutoCompleteExtender ID="ACEApprovedBy" MinimumPrefixLength="3" OnClientPopulated="User_ItemPopulated"
                                                            OnClientItemSelected="User_ItemSelected" runat="server" TargetControlID="txtApprovedBy"
                                                            ServiceMethod="GetUser" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";,:" CompletionListItemCssClass="CompletionListItemCssClass"
                                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                            ShowOnlyCurrentWordInCompletionListItem="true" CompletionInterval="0">
                                                        </cc1:AutoCompleteExtender>
                                                        <cc1:FilteredTextBoxExtender ID="FTBApprovedBy" ValidChars=" ." TargetControlID="txtApprovedBy"
                                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" runat="server"
                                                            Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator Display="None" ID="rfvApprovedBy" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True" ValidationGroup="btnAdd" runat="server" ControlToValidate="txtApprovedBy"
                                                            ErrorMessage="Enter the Approved By"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="15%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="15%" Wrap="true" />
                                                </asp:TemplateField>
                                                <%-- Approved Date --%>
                                                <asp:TemplateField HeaderText="Approved Date" FooterStyle-Width="12%" ItemStyle-Width="12%"
                                                    HeaderStyle-Width="12%" Visible="true" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprovedDate" ToolTip="Paid Till Now" Width="100%" runat="server"
                                                            Text='<%#Eval("Approved_Date")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtApprovedDate" runat="server" Width="90%"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CECApprovedDate" runat="server" Format="dd/MM/yyyy" Enabled="True"
                                                            TargetControlID="txtApprovedDate">
                                                            <%--OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"--%>
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator Display="None" ID="rfvApprovedDate" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True" ValidationGroup="btnAdd" runat="server" ControlToValidate="txtApprovedDate"
                                                            ErrorMessage="Enter the approved Date"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="12%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="12%" Wrap="false" />
                                                </asp:TemplateField>
                                                <%--Photo Proof--%>
                                                <asp:TemplateField HeaderText="Upload Photo" FooterStyle-Width="7%" ItemStyle-Width="7%"
                                                    HeaderStyle-Width="7%" Visible="true" FooterStyle-Wrap="true" ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPhotoProof" ToolTip="Upload Photo" Width="100%" runat="server" Font-Underline="true"
                                                            Text='<%#Eval("Photo_Upload")%>' Visible="false"></asp:Label>
                                                        <asp:ImageButton ID="hyplnkView" CommandArgument='<%# Bind("Photo_Upload") %>'
                                                            OnClick="hyplnkView_Click" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                            runat="server" ToolTip="View Document" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <table align="left">
                                                            <tr>
                                                                <td style="padding: 0px">
                                                                    <asp:CheckBox ID="chkPhoto" runat="server" Text="" Width="20px" />
                                                                </td>
                                                                <td style="padding: 0px">
                                                                    <cc1:AsyncFileUpload ID="asyFilePhotoUpload" runat="server" Width="70%" OnUploadedComplete="asyFilePhotoUpload_UploadedComplete" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="7%" Wrap="true" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="7%" Wrap="true" />
                                                </asp:TemplateField>
                                                <%--Document Proof--%>
                                                <asp:TemplateField HeaderText="Upload Document" FooterStyle-Width="7%" ItemStyle-Width="7%"
                                                    HeaderStyle-Width="7%" Visible="true" FooterStyle-Wrap="true" ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocumentProof" ToolTip="Document Upload" Width="100%" runat="server"
                                                            Text='<%#Eval("Document_Upload")%>' Visible="false"></asp:Label>
                                                        <asp:ImageButton ID="hyplnkDocView" CommandArgument='<%# Bind("Document_Upload") %>'
                                                            OnClick="hyplnkDocView_Click" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                            runat="server" ToolTip="View Document" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <table align="left">
                                                            <tr>
                                                                <td style="padding: 0px">
                                                                    <asp:CheckBox ID="chkDocument" runat="server" Text="" Width="20px" />
                                                                </td>
                                                                <td style="padding: 0px">
                                                                    <cc1:AsyncFileUpload ID="asyFileDocumentUpload" runat="server" Width="70%" OnUploadedComplete="asyFileDocumentUpload_UploadedComplete" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="7%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                </asp:TemplateField>
                                                <%--End Use--%>
                                                <asp:TemplateField HeaderText="End Use" FooterStyle-Width="10%" ItemStyle-Width="10%"
                                                    HeaderStyle-Width="10%" Visible="true" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEndUse" ToolTip="End Use" Width="100%" runat="server" Text='<%#Eval("End_Use")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterEndUse" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator Display="None" ID="RFVFooterEndUse" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlFooterEndUse" InitialValue="0"
                                                            ErrorMessage="Select End Use" ValidationGroup="btnAdd"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="10%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateField>
                                                <%-- End Use ID --%>
                                                <asp:TemplateField HeaderText="End Use ID" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="5%" HeaderStyle-Width="15%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEndUseID" ToolTip="Stage ID" Width="100%" runat="server" Text='<%#Eval("End_Use_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle Width="5%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:TemplateField>
                                                <%--Action--%>
                                                <asp:TemplateField HeaderText="Action" FooterStyle-Width="5%" ItemStyle-Width="5%"
                                                    HeaderStyle-Width="5%" Visible="true" FooterStyle-Wrap="false" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAction" ToolTip="End Use" Width="100%" runat="server" Text='<%#Eval("Action")%>' Visible="false"></asp:Label>
                                                        <asp:DropDownList ID="ddlItemAction" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlItemAction_SelectedIndexChanged"></asp:DropDownList>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFooterAction" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator Display="None" ID="RFVFooterAction" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True" runat="server" ControlToValidate="ddlFooterAction" InitialValue="0"
                                                            ErrorMessage="Select Action" ValidationGroup="btnAdd"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="5%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:TemplateField>
                                                <%-- Action ID --%>
                                                <asp:TemplateField HeaderText="Action ID" FooterStyle-Width="5%" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="5%" HeaderStyle-Width="5%" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionID" ToolTip="Action ID" Width="100%" runat="server" Text='<%#Eval("Action_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle Width="5%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                </asp:TemplateField>
                                                <%-- Remarks --%>
                                                <asp:TemplateField HeaderText="Remarks" FooterStyle-Width="25%" ItemStyle-Width="25%"
                                                    HeaderStyle-Width="1%" Visible="true" FooterStyle-Wrap="true" ItemStyle-Wrap="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" ToolTip="Remarks" Style="word-break: break-all; word-wrap: break-word;"
                                                            runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="90%" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="25%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" Wrap="true" />
                                                </asp:TemplateField>
                                                <%-- Add --%>
                                                <asp:TemplateField HeaderText="" FooterStyle-Width="1%" ItemStyle-Width="20%" HeaderStyle-Width="1%"
                                                    Visible="true">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkRemove" runat="server" Text="Remove" OnClick="lnkRemove_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkAdd" Width="100%" ToolTip="Add to the grid" ValidationGroup="btnAdd"
                                                            OnClick="lnkAdd_Click" runat="server" Text="Add"></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <FooterStyle Width="15%" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle HorizontalAlign="Center" />
                                            <HeaderStyle CssClass="styleGridHeader" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:HiddenField ID="hdnUserId" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr class="styleButtonArea">
            <td align="center">
                <asp:Button runat="server" ID="btnSave" ValidationGroup="vsLoanApproval" OnClientClick="return fnCheckPageValidators('vsLoanApproval');"
                    CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Clear" OnClientClick="return confirm('Do you want to cancel this record?');"
                    OnClick="btnClear_Click" />
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
                <asp:ValidationSummary runat="server" ID="vsLoanApproval" ValidationGroup="vsLoanApproval"
                    HeaderText="Please correct the following validation(s):" Height="250px" CssClass="styleMandatoryLabel"
                    Width="500px" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary runat="server" ID="btnAdd" ValidationGroup="btnAdd" HeaderText="Please correct the following validation(s):"
                    Height="250px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                    ShowSummary="true" />
            </td>
        </tr>
    </table>
</asp:Content>
