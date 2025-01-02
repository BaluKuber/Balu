<%@ Page Title="Cash Flow Print" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_Loanad_CashFlowPrint.aspx.cs" Inherits="LoanAdmin_S3G_Loanad_CashFlowPrint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MultiSelect" Src="~/UserControls/S3GMultiSelect.ascx" %>
<%@ Register TagPrefix="uc3" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function Invoice_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnInvoice.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function Invoice_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnInvoice.ClientID %>');
            hdnLessee.value = '';
        }
    </script>
    <asp:UpdatePanel ID="udpCFormSalesReport" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="Other Cash Flow Print" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlCashFlw" runat="server" GroupingText="Filter Criteria" CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                                
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPostingDateFrom" runat="server" Text="RS Activated Start Date" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <input id="hidPostingDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtPostingDateFrom" runat="server" AutoPostBack="true" OnTextChanged="txtPostingDateFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateFrom" Format="dd/MM/yyyy" runat="server"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgPostingDateFrom"
                                            TargetControlID="txtPostingDateFrom">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvPostingDateFrom" runat="server" ErrorMessage="Select RS Activated Start Date." ValidationGroup="rfvSearch"
                                            Display="None" Enabled="false" SetFocusOnError="True" ControlToValidate="txtPostingDateFrom"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPostingDateTo" runat="server" Text="RS Activated End Date" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtPostingDateTo" runat="server" AutoPostBack="true" OnTextChanged="txtPostingDateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgPostingDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderPostingDateTo" runat="server" Format="dd/MM/yyyy"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgPostingDateTo"
                                            TargetControlID="txtPostingDateTo">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvPostingDateTo" runat="server" ErrorMessage="Select RS Activated End Date." ValidationGroup="rfvSearch"
                                            Display="None" Enabled="false" SetFocusOnError="True" ControlToValidate="txtPostingDateTo"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCashFlowStatus" runat="server" Text="Status" CssClass="styleReqFieldLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList Width="42%" ID="drpStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpStatus_SelectedIndexChanged">
                                            <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Processed" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Under Processing" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ErrorMessage="Select Status." ValidationGroup="rfvSearch"
                                            Display="None" SetFocusOnError="True" ControlToValidate="drpStatus" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlCustName" Width="280px" runat="server" WatermarkText="--All--" ServiceMethod="GetCustomerName" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblScheduleNo" runat="server" Text="RS No" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlRSNo" runat="server" WatermarkText="--All--" ServiceMethod="GetScheduleNo" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblTrancheNo" runat="server" Text="Tranche No" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlTrancheNo" runat="server" WatermarkText="--All--" ServiceMethod="GetTrancheNo" />
                                    </td>
                                </tr>
                                <tr id="trCashFlow" runat="server">
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCashFlowDesc" runat="server" Text="CashFlow Description" />
                                    </td>
                                    <td width="30%" class="styleFieldAlign" style="padding-left: 0px;">
                                        <uc1:MultiSelect ID="MultiSelect" runat="server" Width="130px" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label Text="Invoice No" runat="server" ID="lblInvoice" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:TextBox ID="txtInvoice" runat="server" OnTextChanged="txtInvoice_OnTextChanged"
                                            AutoPostBack="true"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="aceInvoice" MinimumPrefixLength="3" OnClientPopulated="Invoice_ItemPopulated"
                                            OnClientItemSelected="Invoice_ItemSelected" runat="server" TargetControlID="txtInvoice"
                                            ServiceMethod="GetInvoiceNo" Enabled="True" ServicePath="" CompletionSetCount="5"
                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </cc1:AutoCompleteExtender>
                                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtInvoice"
                                            WatermarkText="--Select--">
                                        </cc1:TextBoxWatermarkExtender>
                                        <asp:HiddenField ID="hdnInvoice" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDueDate" runat="server" Text="Due Date" CssClass="styleReqFieldLabel" />
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtDueDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgDueDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalExtenderDueDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgDueDate"
                                            TargetControlID="txtDueDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ErrorMessage="Select the Due Date." ValidationGroup="rvSave"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtDueDate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>

                                    </td>
                                    <td class="styleFieldLabel">
                                        <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">
                                        <asp:Button ID="btnGo" runat="server" Text="Process" UseSubmitBehavior="true" ValidationGroup="rfvSearch" CssClass="styleSubmitButton"
                                            OnClick="btnGo_Click" />
                                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();" OnClick="btnclear_Click" ToolTip="Clear" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="left">
                        <asp:Panel ID="pnlrs" runat="server" GroupingText="RS Details" Width="100%" CssClass="stylePanel">
                            <div id="divacc" runat="server" style="overflow: auto;">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grvCashflowPrnt" runat="server" AutoGenerateColumns="False"
                                                BorderWidth="2px" EnableModelValidation="True" Width="90%" OnRowDataBound="grvCashflowPrnt_RowDataBound"
                                                OnRowCommand="grvCashflowPrnt_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Tranche Name / RS Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRS_Number" ToolTip="Tranche Name" runat="server" Text='<%#Bind("Tranche_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" ToolTip="Status" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Select">
                                                        <HeaderTemplate>
                                                            <table align="center">
                                                                <tr>
                                                                    <td>Select All
                                                                    </td>
                                                              
                                                                    <td>
                                                                        <asp:CheckBox ID="chkAll" runat="server" />
                                                                    </td>
                                                               
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Select Account" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvoice_Date" ToolTip="Invoice Date" runat="server" Text='<%#Bind("Invoice_Date") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblPrint" runat="server" Text="Print"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnPrint" CommandArgument='<%# Bind("TRANCHE_NAME") %>' 
                                                                CommandName="Print" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Digi_Flag" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDigi_Flag" runat="server" Text='<%#Eval("Digi_Sign_Enable")%>'></asp:Label>
                                                            <asp:Label ID="lblTranche" runat="server" Text='<%#Eval("Tranche")%>'></asp:Label>
                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <uc3:PageNavigator ID="ucCustomPaging" runat="server"></uc3:PageNavigator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnSearch" runat="server" />
                                            <input type="hidden" id="hdnOrderBy" runat="server" />
                                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="100%" colspan="4" align="center">
                                            <asp:Button ID="btnGenerate" runat="server" Text="Generate" UseSubmitBehavior="true" ValidationGroup="rvSave"
                                                CssClass="styleSubmitButton" OnClick="btnGenerate_Click" ToolTip="Generate" />
                                            <asp:Button ID="btnPrint" runat="server" Text="Print" UseSubmitBehavior="true" ValidationGroup="rfvSearch"
                                                CssClass="styleSubmitButton" OnClick="btnPrint_Click" ToolTip="Print" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ID="vsTransLander" runat="server" ValidationGroup="rfvSearch"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                        <asp:ValidationSummary ID="rfvSave" runat="server" ValidationGroup="rvSave"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

