<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdAccountSplit.aspx.cs" Inherits="LoanAdmin_S3GLoanAdAccountSplit"
    Title="S3G - Account Split" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="2" style="height: 19px">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Account Split">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%">
                        <asp:Panel ID="panAccontSplit" runat="server" CssClass="stylePanel" GroupingText="Rental Schedule Split Details"
                            Height="100%" Style="padding-bottom: 28px;">
                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLineofBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLineofBusiness" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLineofBusiness_SelectedIndexChanged"
                                            TabIndex="1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLineofBusiness" runat="server" ErrorMessage="Select Line of Business"
                                            InitialValue="0" Display="None" ControlToValidate="ddlLineofBusiness" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                                            AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ValidationGroup="Submit"
                                            ErrorMessage="Select a Location" IsMandatory="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblMLANumber" runat="server" CssClass="styleReqFieldLabel" Text="Rental Schedule Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlPANumber" runat="server" ToolTip="Rental Schedule Number" ServiceMethod="GetPANNUmber"
                                            AutoPostBack="true" OnItem_Selected="ddlPANumber_SelectedIndexChanged" ValidationGroup="Submit"
                                            ErrorMessage="Select a Rental Schedule Number" IsMandatory="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblOriginalAgrementDate" runat="server" Text="Original Agreement Date"
                                            CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="margin-left: 40px">
                                        <asp:TextBox ID="txtOriginalAgreementDate" runat="server" Width="100px" TabIndex="5"></asp:TextBox>
                                        <%--    <asp:RequiredFieldValidator ID="rfvOriginalAgreementDate" runat="server" ErrorMessage="Enter Original Agreement Date"
                            Display="None" ControlToValidate="txtOriginalAgreementDate" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblSplitNo" runat="server" Text="Split No"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtSplitNo" runat="server" Width="100px" ReadOnly="true" TabIndex="6"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblSplitDate" runat="server" Text="Split Date" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtSplitDate" runat="server" Width="100px" TabIndex="6"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSplitDate" runat="server" ErrorMessage="Enter Split Date"
                                            Display="None" ControlToValidate="txtSplitDate" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgSplitDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtSplitDate" PopupButtonID="imgSplitDate" ID="calExeSplitDate"
                                            Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td style="width: 50%">
                        <asp:Panel ID="pnlCustomerInformation" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                            Height="100%">
                            <table align="center" width="100%">
                                <tr>
                                    <td width="100%">
                                        <uc1:S3GCustomerAddress ID="S3GCustomerCommAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                            SecondColumnStyle="styleFieldAlign" showcusomername="false" ShowCustomerCode="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel runat="server" ID="panAccountDetails" CssClass="stylePanel" GroupingText="Asset Details">
                            <asp:GridView ID="GVACCDetails" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                EnableViewState="true" ShowFooter="true" Width="99%" OnRowDataBound="GVACCDetails_RowDataBound"
                                TabIndex="7">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" SortExpression="Select">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAllBranch" runat="server" onclick="javascript:fnchkSelectAllBranch(this,'chkSelect');" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reference No" SortExpression="ReferenceNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReferenceNo" runat="server" Text='<%# Bind("Reference_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BTN No" SortExpression="BTNNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockInvoiceNo" runat="server" Text='<%# Bind("Stock_Invoice_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" SortExpression="Invoice No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("Invoice_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Description" SortExpression="Asset Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetDesc" runat="server" Text='<%# Bind("Asset_Description") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Id" SortExpression="Asset Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetId" runat="server" Text='<%# Bind("Invoice_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                            <asp:TextBox ID="txtQty" Visible="false" runat="server" Width="50px" Text='<%# Bind("Quantity") %>' MaxLength="4"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTEQty" runat="server" TargetControlID="txtQty"
                                                FilterType="Numbers" Enabled="true">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalQuantity" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FinanceAmount" SortExpression="FinanceAmount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinanceAmount" runat="server" Text='<%# Bind("FinanceAmount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalFinanceAmount" runat="server"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Per_Asset_Amount" SortExpression="Per_Asset_Amount"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPer_Asset_Amount" runat="server" Text='<%# Bind("Per_Asset_Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click"
                            CssClass="styleSubmitButton" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel runat="server" ID="panSplitDetails" CssClass="stylePanel" HorizontalAlign="Center" GroupingText="Split Details">
                            <asp:GridView ID="grvSplitDetails" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                OnRowDataBound="grvSplitDetails_RowDataBound" Width="100%" TabIndex="8" OnRowDeleting="grvSplitDetails_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="Asset Reference No" SortExpression="Split_Ref_No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSplitRefNo" runat="server" Text='<%# Bind("Split_Ref_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlSplitRefNo" runat="server" Visible="false">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="BTN No" SortExpression="BTNNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockInvoiceNo" runat="server" Text='<%# Bind("Stock_Invoice_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" SortExpression="Invoice No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("Invoice_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetDesc" runat="server" Text='<%# Bind("Asset_Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Id" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetId" runat="server" Text='<%# Bind("Asset_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtQty" runat="server" Width="50px" MaxLength="4" Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvQty" runat="server" ErrorMessage="Enter Quantity" Enabled="false"
                                                Display="None" ControlToValidate="txtQty" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                            <cc1:FilteredTextBoxExtender ID="FTEQty" runat="server" TargetControlID="txtQty"
                                                FilterType="Numbers" Enabled="true">
                                            </cc1:FilteredTextBoxExtender>
                                        </FooterTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalSplitAsset" runat="server" Text="Total Asset :"></asp:Label>
                                            <asp:Label ID="lblTotalQuantity" runat="server" Text="0"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Finance Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFinanceAmt" runat="server" Text='<%# Bind("Finance_Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Split Ref No">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAccGroup" runat="server" OnTextChanged="txtAccGroup_TextChanged"
                                                MaxLength="2" onblur="return CheckRefNo(this)" Width="25px" Text='<%# Bind("Account_Group") %>'
                                                AutoPostBack="True" align="center"></asp:TextBox>
                                            <input id="hdnAccgrp" type="hidden" runat="server" />
                                            <cc1:FilteredTextBoxExtender ID="FTEQty" runat="server" TargetControlID="txtAccGroup"
                                                FilterType="Numbers" Enabled="true">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CommandName="Add" OnClick="btnAdd_Click"
                                                CssClass="styleSubmitButton" ValidationGroup="Add" Visible="false" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Split Percentage" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSplitPercentage" runat="server" Text='<%# Bind("Split_Percentage") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PANum" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("Split_PANum") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SANum" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("Split_SANum") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AccountId" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountId" runat="server" Text='<%# Bind("Account_Creation_ID") %>'></asp:Label>
                                            <asp:Label ID="lblAccount_Creation_ID1" runat="server" Text='<%# Bind("Account_Creation_ID1") %>'></asp:Label>
                                            <asp:Label ID="lblAccount_Creation_ID2" runat="server" Text='<%# Bind("Account_Creation_ID2") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete"
                                                CssClass="styleSubmitButton" Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnCreateAcc1" runat="server" Text="Create RS" CommandName="Create"
                                OnClick="btnCreateAcc1_Click" CssClass="styleSubmitButton" Enabled="false" Visible="false" />
                            <asp:Button ID="btnViewAcc1" runat="server" Text="View RS" CommandName="View"
                                CssClass="styleSubmitButton" Visible="false" />
                            <asp:Button ID="btnCreateAcc2" runat="server" Text="Modify Parent RS" CommandName="Create"
                                OnClick="btnCreateAcc2_Click" CssClass="styleSubmitButton" Enabled="false" Visible="false" />
                            <asp:Button ID="btnViewAcc2" runat="server" Text="View Parent RS" CommandName="View"
                                CssClass="styleSubmitButton" Visible="false" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="padding-top: 10px" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators('Submit')"
                            TabIndex="9" />

                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" CausesValidation="false"
                            TabIndex="10" />

                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" CausesValidation="false" TabIndex="11" />
                        <asp:Button ID="btnCancelAcc" runat="server" CssClass="styleSubmitButton" Text="Cancel Acc. Split"
                            Visible="False" OnClick="btnCancelAcc_Click" OnClientClick="return Confirmmsg('Do you want to cancel account split?'); "
                            TabIndex="12" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hdnFinanceAmount" runat="server" />
                        <asp:HiddenField runat="server" ID="hdnMLASLA" />
                        <asp:HiddenField runat="server" ID="hdnSplitNo" />
                        <asp:HiddenField runat="server" ID="hdnStatus" />
                        <asp:ValidationSummary ID="vsConsolidation" runat="server" ValidationGroup="Submit"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Add"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="Button1" Style="display: none" runat="server" />
    <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>

    <asp:Button ID="btnModal" Style="display: none" runat="server" />
    <cc1:ModalPopupExtender ID="ModalPopupExtenderApprover" runat="server" TargetControlID="btnModal"
        PopupControlID="pnlODICheck" BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlODICheck" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="50%" ScrollBars="Auto">
        <asp:UpdatePanel ID="UpdpnlODICheck" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <h3 style="height: 10px;">Error Details
                            </h3>
                            <asp:TextBox ID="txtErrmsg" runat="server" Text="Overdue arrears available..! Do you still want to proceed..!"
                                Width="100%" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnModalOk" runat="server" Text="Ok" ToolTip="Ok" class="styleSubmitButton"
                                OnClick="btnModalOk_Click" />
                            <asp:Button ID="btnModalCancel" runat="server" Text="Cancel" OnClick="btnModalCancel_Click"
                                ToolTip="Cancel" class="styleSubmitButton" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <script language="javascript" type="text/javascript">
        function SetPrevValueOnFocus(fromtxtbox, totxtBox) {
            document.getElementById(totxtBox).value = fromtxtbox.value;
        }

        function ViewModal(var1, var2) {
            //Browser compatability issue fixed on 08-Mar-2023 
            window.open('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsAccSplitNo=' + var1 + '&qsRefNo=' + var2 + '&IsFromAccount=AccSplit', 'newwindow', 'toolbar=no,location=no,menubar=no,width=1300,height=900,resizable=no,scrollbars=yes,top=150,left=100'); return true;

            //window.showModalDialog('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsAccSplitNo=' + var1 + '&qsRefNo=' + var2 + '&IsFromAccount=AccSplit', 'Account Creation', 'dialogwidth:1300px;dialogHeight:900px;');
        }

        function ViewAccountModal(var1)
        {
             //Browser compatability issue fixed on 08-Mar-2023 
            window.open('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsViewId=' + var1 + '&IsFromAccount=AccSplit&qsMode=Q', 'newwindow', 'toolbar=no,location=no,menubar=no,width=1300,height=900,resizable=no,scrollbars=yes,top=150,left=100'); return true;
            //window.showModalDialog('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsViewId=' + var1 + '&IsFromAccount=AccSplit&qsMode=Q', 'Account Creation', 'dialogwidth:1300px;dialogHeight:900px;');
        }
        function CheckRefNo(txtbox) {
            if (txtbox.value == '') {
                alert('Split Ref No cannot be empty');
                txtbox.focus();
                return false;
            }
            if (parseFloat(txtbox.value) == 0) {
                alert('Split Ref No should be greater than 0');
                txtbox.focus();
                return false;
            }
        }

        function fnchkSelectAllBranch(chkSelectAllBranch, chkSelectBranch) {
            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_GVACCDetails');
            var TargetChildControl = chkSelectBranch;
            //Get all the control of the type INPUT in the base control.
            var Inputs = grvCashFlow.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllBranch.checked;
        }

    </script>

</asp:Content>
