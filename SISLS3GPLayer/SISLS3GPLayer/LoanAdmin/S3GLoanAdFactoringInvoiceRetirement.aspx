<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdFactoringInvoiceRetirement.aspx.cs" Inherits="LoanAdmin_S3GLoanAdFactoringInvoiceRetirement"
    Title="Factoring Invoice Retirement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
    <%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlFactoringDetails" runat="server" CssClass="stylePanel" GroupingText="Factoring Invoice Details"
                            Width="99%">
                            <table cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <%-- LOB --%>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <%--<asp:TextBox runat="server" ID="txtLOB" Width="175px" Wrap="False" ReadOnly="True"></asp:TextBox>    --%>
                                        <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" TabIndex="-1">
                                        </asp:DropDownList>
                                    </td>
                                    <%--Branch--%>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                       <%-- <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                            ToolTip="Location">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVFILBranch" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlBranch" InitialValue="0" ValidationGroup="VGFIL" ErrorMessage="Select a Location"
                                            Display="None">
                                        </asp:RequiredFieldValidator>--%>
                                           <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"  AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                                        ValidationGroup="VGFIL"     ErrorMessage="Select a Location" IsMandatory="true" />
                                    </td>
                                </tr>
                                <%-- second Row--%>
                                <tr>
                                    <%--MLA --%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Factoring Invoice No" ID="lblFactdocno" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlFactdocno" runat="server" AutoPostBack="True" ToolTip="Factoring Invoice No"
                                            TabIndex="4" OnSelectedIndexChanged="ddlFactdocno_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVFactDocNo" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlFactdocno" InitialValue="0" ValidationGroup="VGFIL" ErrorMessage="Select a Factoring Invoice No"
                                            Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Prime Account Number" ID="lblprimeaccno"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtPAN" Width="120px" ReadOnly="True" ToolTip="Prime Account Number"></asp:TextBox>
                                        <asp:DropDownList ID="ddlPAN" WidthVisible="false" runat="server" OnSelectedIndexChanged="ddlPAN_SelectedIndexChanged"
                                            AutoPostBack="True" ToolTip="Prime Account Number" Visible="false">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVMLA" Enabled="false" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="ddlPAN" InitialValue="0" ValidationGroup="VGFIL"
                                            ErrorMessage="Select a Prime Account Number" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%-- Third Row --%>
                                <tr>
                                    <%--SLA--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Sub Account Number" ID="lblsubAccno" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtSAN" Width="120px" ReadOnly="True" ToolTip="Sub Account Number"></asp:TextBox>
                                        <asp:DropDownList ID="ddlSAN" Width="180px" Visible="false" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlSAN_SelectedIndexChanged" ToolTip="Sub Account Number">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVSLA" Enabled="False" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="ddlSAN" InitialValue="0" ValidationGroup="VGFIL"
                                            ErrorMessage="Select a Sub Account Number" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFVSLA1" Enabled="False" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="ddlSAN" InitialValue="0" ValidationGroup="VGFILAdd"
                                            ErrorMessage="Select a Sub Account Number" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="FIL Number" ID="lblFILNo" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtFILNo" Width="100px" ReadOnly="True" ToolTip="FIL Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   Fourth Row--%>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="FIL Date" ID="lblFILRDate">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtFILDate" Width="100px" ReadOnly="true" ToolTip="FIL Date"></asp:TextBox>
                                        <asp:Image ID="imgFILDate" runat="server" Visible="False" ImageUrl="~/Images/calendaer.gif"
                                            ToolTip="FIL Date" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Enabled="False" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFILAdd" ControlToValidate="txtFILDate" Display="None"
                                            ErrorMessage="Select Factoring Invoice Loading Date"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Enabled="False" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFIL" ControlToValidate="txtFILDate" Display="None"
                                            ErrorMessage="Select Factoring Invoice Loading Date"></asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender ID="CECFILDATE" runat="server" Enabled="False" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            PopupButtonID="imgFILDate" TargetControlID="txtFILDate" Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <%--Status--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Status" ID="lblStatus" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtStatus" Width="100px" ReadOnly="True" ToolTip="Status"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   Fifth Row--%>
                                <tr>
                                    <%--Margin %--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Margin %" ID="lblMargin" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtMargin" Width="55px" ReadOnly="True" Style="text-align: right"
                                            ToolTip="Margin %"></asp:TextBox>
                                    </td>
                                    <%--Credit Limit --%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Credit Limit" ID="lblCreditLimit" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtCreditLimit" Width="100px" ReadOnly="True" Style="text-align: right"
                                            ToolTip="Credit Limit"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   Sixth Row--%>
                                <tr>
                                    <%-- Credit Available --%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Credit Available" ID="lblCreditAvailable" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtCreditAvailable" ReadOnly="true" Width="100px"
                                            Style="text-align: right" ToolTip="Credit Available"></asp:TextBox>
                                    </td>
                                    <%-- Out Standing Amount --%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Outstanding Amount" ID="lblOutStandAmount" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtOutStandingAmount" Width="100px" Style="text-align: right"
                                            ToolTip="Outstanding amount" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   8`th  Row--%>
                                <%-- Credi--%>
                                <tr>
                                    <%-- Invoice Load Value--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Invoice Load Value" ID="lblInvoiceloadvalue">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtInvoiceLoadValue" ReadOnly="True" Width="100px"
                                            Style="text-align: right" ToolTip="Invoice Load Value"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Enabled="false" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFILAdd" ControlToValidate="txtInvoiceLoadValue"
                                            Display="None" ErrorMessage="Enter the Invoice Load Value "></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Enabled="false" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFIL" ControlToValidate="txtInvoiceLoadValue"
                                            Display="None" ErrorMessage="Enter the Invoice Load Value "></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtInvoiceLoadValue"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="False">
                                        </cc1:FilteredTextBoxExtender>
                                        <%--<cc1:MaskedEditExtender ID="MEEInvoiceLoadValue" runat="server" InputDirection="RightToLeft"
                                             Mask="9999999999.99"  MaskType="Number"  TargetControlID="txtInvoiceLoadValue">
                                        </cc1:MaskedEditExtender>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCreditDays" runat="server" CssClass="styleDisplayLabel" Text="Credit Days"> </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCreditDays" runat="server" MaxLength="3" Width="30px" Style="text-align: right"
                                            ToolTip="Credit Days" ReadOnly="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTECreditDays" runat="server" Enabled="false" TargetControlID="txtCreditDays"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                        <br />
                                    </td>
                                    <td>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:Button ID="btnGo" Visible="false" CssClass="styleGridShortButton" runat="server"
                                            Text="Go" ValidationGroup="VGFILAdd" OnClick="btnGo_Click" ToolTip="Go" OnClientClick="return fnCheckPageValidators('VGFILAdd',false);" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <%--   9`th  Row--%>
                <tr>
                    <td>
                        <asp:Panel ID="pnlCommAddress" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                            Width="99%">
                            <asp:HiddenField ID="hidcuscode" runat="server" />
                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" ActiveViewIndex="1"
                                FirstColumnWidth="20%" SecondColumnWidth="30%" ThirdColumnWidth="20%" FourthColumnWidth="30%" />
                        </asp:Panel>
                    </td>
                </tr>
                <%--Second Row For Gridview Heading--%>
                <tr>
                    <td width="100%">
                        <asp:Panel ID="pnlInvDtl" runat="server" CssClass="stylePanel" GroupingText="Invoice Details"
                            Width="99%">
                            <asp:GridView runat="server" ID="GRVFIL" AutoGenerateColumns="False" OnRowDataBound="GRVFIL_RowDataBound"
                                Width="100%">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="SI No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text='<%# Bind("SNo")%>' Width="30px" ToolTip="SI No."></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("InvoiceNO")%>' Rows="2"
                                                ToolTip="Invoice No"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("InvoiceDate")%>' ToolTip="Date"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Party Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPartyName" runat="server" Text='<%# Bind("PartyName")%>' ToolTip="Party Name"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maturity Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Bind("MaturityDate")%>'
                                                ReadOnly="true" ToolTip="Maturity Date" Style="border-color: White"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <FooterStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Bind("InvoiceAmount")%>'
                                                ToolTip="Invoice Amount"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eligible Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEligibleAmount" runat="server" Text='<%# Bind("EligibleAmount")%>'
                                                ToolTip="Eligible Amount"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Received Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceivedAmount" runat="server" Text='<%# Bind("ReceivedAmount")%>'
                                                ToolTip="Received Amount"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="styleGridHeader" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <%--Third Row For Gridview Heading--%>
                <tr>
                    <td>
                        <asp:Panel ID="pnlReceiptDtl" runat="server" CssClass="stylePanel" GroupingText="Receipt Information"
                            Width="99%">
                            <asp:GridView runat="server" ID="grvMapping" AutoGenerateColumns="False" ShowFooter="true"
                                Width="100%" OnRowCommand="grvMapping_RowCommand" OnRowDataBound="grvMapping_RowDataBound"
                                OnRowDeleting="grvMapping_RowDeleting">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="40px" HeaderText="SI No.">
                                        <ItemTemplate>
                                            <asp:Label ID="txtsno" runat="server" Text='<%# Bind("SNo")%>' Width="30px" ToolTip="SI No."></asp:Label>
                                            <asp:Label ID="lblFT_Retirement_Dtl_ID" runat="server" Visible="false" 
                                                Text='<%# Bind("FT_Retirement_Dtl_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("InvoiceNO")%>' ToolTip="Invoice Number"></asp:Label>
                                            <asp:Label ID="lblInvoiceID" runat="server" Rows="2" Visible="false" ToolTip="Invoice No"
                                                Text='<%# Bind("Factoring_Inv_Load_Details_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlInvoiceNo" runat="server" AutoPostBack="True" ToolTip="Invoice No"
                                                OnSelectedIndexChanged="ddlInvoiceNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvInvoiceNo" runat="server" ErrorMessage="Select the Invoice Number"
                                                ValidationGroup="vgGridMapp" CssClass="styleLoginLabel" ControlToValidate="ddlInvoiceNo"
                                                InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemStyle Width="140px" HorizontalAlign="Left" />
                                        <FooterStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnreceivedAmount" runat="server" Text='<%# Bind("UnreceivedAmount")%>'
                                                Style="text-align: right;" ToolTip="Balance Amount"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblUnreceivedAmountF" runat="server" ToolTip="Balance Amount" Style="text-align: right;"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Receipt No" ItemStyle-Width="140px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceiptNo" runat="server" Text='<%# Bind("ReceiptNO")%>' ToolTip="Invoice Number"></asp:Label>
                                            <asp:Label ID="lblReceiptID" runat="server" Rows="2" Visible="false" ToolTip="Receipt No"
                                                Text='<%# Bind("Receipt_Details_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlReceiptNo" runat="server" AutoPostBack="True" ToolTip="Invoice No"
                                                OnSelectedIndexChanged="ddlReceiptNo_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvReceiptNo" runat="server" ErrorMessage="Select the Receipt Number"
                                                ValidationGroup="vgGridMapp" CssClass="styleLoginLabel" ControlToValidate="ddlReceiptNo"
                                                InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemStyle Width="140px" HorizontalAlign="Left" />
                                        <FooterStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Receipt Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceiptDate" runat="server" Text='<%# Bind("ReceiptDate")%>' ToolTip="Receipt Date"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblReceiptDateF" runat="server" ToolTip="Receipt Date"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Receipt Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceiptAmount" runat="server" Text='<%# Bind("ReceiptAmount")%>'
                                                Style="text-align: right;" ToolTip="Receipt Amount"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblReceiptAmountF" runat="server" ToolTip="Receipt Amount" Style="text-align: right;"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unused Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnusedAmount" runat="server" Text='<%# Bind("UnusedAmount")%>'
                                                Style="text-align: right;" ToolTip="Unused Amount"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblUnusedAmountF" runat="server" ToolTip="Unused Amount" Style="text-align: right;"></asp:Label>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Appropriation Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApprAmount" runat="server" Text='<%# Bind("App_Amount")%>' Style="text-align: right;"
                                                ToolTip="Appropriation Amount"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtApprAmountF" runat="server" MaxLength="13" Style="text-align: right;"
                                                onkeypress="fnAllowNumbersOnly(true,false,this)" Width="90px" ToolTip="Appropriation Amount"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="fextxtApprAmountF" runat="server" TargetControlID="txtApprAmountF"
                                                FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="rfvApprAmountF" runat="server" ErrorMessage="Enter the Appropriation Amount"
                                                ValidationGroup="vgGridMapp" CssClass="styleLoginLabel" ControlToValidate="txtApprAmountF"
                                                Display="None"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnRemove" Text="Delete" CommandName="Delete" runat="server"
                                                CausesValidation="false" ToolTip="Delete" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:Button ID="btnAddrow" OnClientClick="return fnCheckPageValidators('vgGridMapp',false);"
                                                runat="server" CommandName="AddNew" CssClass="styleGridShortButton" Text="Add"
                                                ValidationGroup="vgGridMapp" ToolTip="Add"></asp:Button>
                                        </FooterTemplate>
                                        <ControlStyle Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea" style="display: none">
                    <td>
                        <asp:CustomValidator ID="cvCustomerMaster" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
                <tr align="center" valign="bottom">
                    <td>
                        <br />
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators('VGFIL');"
                            CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" ValidationGroup="VGFIL"
                            ToolTip="Save" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                            ToolTip="Clear" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="PRDDC" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 24px">
                        <br />
                        <asp:CustomValidator ID="cvFactoring" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Visible="false" Width="98%" ValidationGroup="VGFIL" />
                        <asp:ValidationSummary runat="server" ID="vsFactoringInvoiceLoadinf" HeaderText="Correct the following validation(s):"
                            CssClass="styleMandatoryLabel" Width="98%" ShowMessageBox="false" ShowSummary="true"
                            ValidationGroup="VGFIL" />
                        <asp:ValidationSummary runat="server" ID="vsFactoringInvoiceLoadinfAdd" HeaderText="Correct the following validation(s):"
                            CssClass="styleMandatoryLabel" Width="98%" ShowMessageBox="false" ShowSummary="true"
                            ValidationGroup="vgGridMapp" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary2" HeaderText="Correct the following validation(s):"
                            CssClass="styleMandatoryLabel" Width="98%" ShowMessageBox="false" ShowSummary="true"
                            ValidationGroup="VGFILAdd" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                            CssClass="styleMandatoryLabel" Width="98%" ShowMessageBox="false" ShowSummary="true"
                            ValidationGroup="vgGridAdd" />
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        function SumScore(sufLen) {

            if (parseInt(sufLen) > 2) {
                sufLen = 2;
            }
            (document.getElementById('ctl00_ContentPlaceHolder1_txtTotalInvoiceAmt')).style.left = (document.getElementById('ctl00_ContentPlaceHolder1_GRVFIL_ctl02_txtInvoiceAmount').clientLeft);
            var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_GRVFIL');
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var TotalInvoiceScore = 0;
            var TotalEligibleScore = 0;
            var MarginPer = 0;
            var InvoiceAmt = 0;
            MarginPer = document.getElementById('<%=txtMargin.ClientID%>').value;
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'text') {
                    if (Inputs[n].value != '') {
                        if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 13) == 'InvoiceAmount') {
                            TotalInvoiceScore = (parseFloat(TotalInvoiceScore) + parseFloat(Inputs[n].value)).toFixed(parseInt(sufLen));
                            InvoiceAmt = (parseFloat(Inputs[n].value)).toFixed(parseInt(sufLen));
                        }

                        if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 14) == 'EligibleAmount') {
                            Inputs[n].value = (parseFloat(InvoiceAmt - (InvoiceAmt * MarginPer) / 100)).toFixed(parseInt(sufLen));
                            TotalEligibleScore = (parseFloat(TotalEligibleScore) + parseFloat(Inputs[n].value)).toFixed(parseInt(sufLen));
                        }
                    }
                }

            }

        }




        function Resize() {
            if (document.getElementById('divMenu').style.display == 'none') {
                (document.getElementById('ctl00_ContentPlaceHolder1_tdEmpty')).style.width = screen.width - document.getElementById('divMenu').style.width - 500;
            }
            else {
                (document.getElementById('ctl00_ContentPlaceHolder1_tdEmpty')).style.width = '55px';
            }
        }
        
    </script>

</asp:Content>
