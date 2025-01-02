<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdFactoringInvoiceLoading.aspx.cs" Inherits="LoanAdmin_S3GLoanAdFactoringInvoiceLoading"
    Title="Factoring Invoice Loading" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
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
                                        <asp:DropDownList ID="ddlLOB" Width="180px" runat="server" ToolTip="Line of Business"
                                            TabIndex="-1">
                                        </asp:DropDownList>
                                    </td>
                                    <%--Branch--%>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Location" ID="lblBranch" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                      <%--  <asp:DropDownList ID="ddlBranch" Width="180px" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ToolTip="Location">
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
                                        <asp:Label runat="server" Text="Prime Account Number" ID="lblprimeaccno" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlPAN" runat="server" ServiceMethod="GetPANUM" AutoPostBack="true"
                                            OnItem_Selected="ddlPAN_SelectedIndexChanged" ValidationGroup="VGFIL" IsMandatory="true"
                                            ErrorMessage="Enter a Prime Account Number" />
                                        <%--<asp:DropDownList ID="ddlPAN" Width="180px" runat="server" OnSelectedIndexChanged="ddlPAN_SelectedIndexChanged"
                                            AutoPostBack="True" ToolTip="Prime Account Number">
                                        </asp:DropDownList>--%>
                                        <%--<asp:RequiredFieldValidator ID="RFVMLA" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlPAN" InitialValue="0" ValidationGroup="VGFIL" ErrorMessage="Select a Prime Account Number"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <%--SLA--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Sub Account Number" ID="lblsubAccno" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlSAN" Width="180px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSAN_SelectedIndexChanged"
                                            ToolTip="Sub Account Number">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVSLA" CssClass="styleMandatoryLabel" runat="server"
                                            Enabled="false" ControlToValidate="ddlSAN" InitialValue="0" ValidationGroup="VGFIL"
                                            ErrorMessage="Select a Sub Account Number" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RFVSLA1" CssClass="styleMandatoryLabel" runat="server"
                                            Enabled="false" ControlToValidate="ddlSAN" InitialValue="0" ValidationGroup="VGFILAdd"
                                            ErrorMessage="Select a Sub Account Number" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%-- Third Row --%>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="FIL Number" ID="lblFILNo" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtFILNo" Width="175px" ReadOnly="True" TabIndex="-1"
                                            ToolTip="FIL Number"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="FIL Date" ID="lblFILDate" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtFILDate" Width="100px" TabIndex="-1" ToolTip="FIL Date"></asp:TextBox>
                                        <asp:Image ID="imgFILDate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="FIL Date" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFILAdd" ControlToValidate="txtFILDate" Display="None"
                                            ErrorMessage="Select Factoring Invoice Loading Date"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFIL" ControlToValidate="txtFILDate" Display="None"
                                            ErrorMessage="Select Factoring Invoice Loading Date"></asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender ID="CECFILDATE" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            PopupButtonID="imgFILDate" TargetControlID="txtFILDate" Format="dd/MM/yyyy">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <%--   Fourth Row--%>
                                <tr>
                                    <%--Status--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Status" ID="lblStatus" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtStatus" Width="175px" ReadOnly="True" TabIndex="-1"
                                            ToolTip="Status"></asp:TextBox>
                                    </td>
                                    <%--Margin %--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Margin %" ID="lblMargin" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtMargin" Width="55px" ReadOnly="True" Style="text-align: right"
                                            TabIndex="-1" ToolTip="Margin %"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   Fifth Row--%>
                                <tr>
                                    <%--Credit Limit --%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Credit Limit" ID="lblCreditLimit" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtCreditLimit" Width="100px" ReadOnly="True" Style="text-align: right"
                                            TabIndex="-1" ToolTip="Credit Limit"></asp:TextBox>
                                    </td>
                                    <%-- Credit Available --%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Credit Available" ID="lblCreditAvailable" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtCreditAvailable" ReadOnly="true" Width="100px"
                                            TabIndex="-1" Style="text-align: right" ToolTip="Credit Available"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--   Sixth Row--%>
                                <tr>
                                    <%-- Out Standing Amount --%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Outstanding Amount" ID="lblOutStandAmount" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtOutStandingAmount" Width="100px" Style="text-align: right"
                                            TabIndex="-1" ToolTip="Outstanding amount" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <%-- Minimum Invoice Value--%>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Minimum Invoice Value" ID="lblInvoiceloadvalue" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox runat="server" ID="txtInvoiceLoadValue" Width="100px" Style="text-align: right"
                                            onkeypress="fnAllowNumbersOnly(true,false,this)" MaxLength="13" ToolTip="Minimum Invoice Value"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFILAdd" ControlToValidate="txtInvoiceLoadValue"
                                            Display="None" ErrorMessage="Enter the Minimum Invoice Value "></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFIL" ControlToValidate="txtInvoiceLoadValue"
                                            Display="None" ErrorMessage="Enter the Minimum Invoice Value "></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtInvoiceLoadValue"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                        <%--<cc1:MaskedEditExtender ID="MEEInvoiceLoadValue" runat="server" InputDirection="RightToLeft"
                                             Mask="9999999999.99"  MaskType="Number"  TargetControlID="txtInvoiceLoadValue">
                                        </cc1:MaskedEditExtender>--%>
                                    </td>
                                </tr>
                                <%--   8`th  Row--%>
                                <%-- Credi--%>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCreditDays" runat="server" CssClass="styleDisplayLabel" Text="Credit Days"> </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCreditDays" runat="server" MaxLength="3" Width="30px" Style="text-align: right"
                                            ToolTip="Credit Days"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FTECreditDays" runat="server" TargetControlID="txtCreditDays"
                                            FilterType="Numbers" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvtxtCreditDays" CssClass="styleMandatoryLabel"
                                            runat="server" ValidationGroup="VGFIL" ControlToValidate="txtCreditDays" Display="None"
                                            ErrorMessage="Enter the Credit Days"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="padding-left: 5px">
                                        <asp:RadioButtonList ID="rbtlBillType" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="0" Selected="True" Text="Customer Bill"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Vendor Bill"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:Button ID="btnGo" CssClass="styleGridShortButton" runat="server" Text="Go" ValidationGroup="VGFILAdd"
                                            OnClick="btnGo_Click" ToolTip="Go" OnClientClick="return fnCheckPageValidators('VGFILAdd',false);" />
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
                    <td>
                        <asp:Panel ID="pnlInvDtl" runat="server" CssClass="stylePanel" GroupingText="Invoice Details"
                            Width="99%">
                            <table cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td align="center">
                                        <%--<div style="overflow-x: hidden; overflow-y: auto; height: 225px;">--%>
                                        <asp:GridView runat="server" ID="GRVFIL" AutoGenerateColumns="False" ShowFooter="true"
                                            OnRowCommand="GRVFIL_RowCommand" Width="100%" OnRowDataBound="GRVFIL_RowDataBound"
                                            OnRowDeleting="GRVFIL_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="SI No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtsno" runat="server" Text='<%# Bind("SNo")%>' Width="30px" ToolTip="SI No."></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="txtsnoF" runat="server" Width="30px"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice No" ItemStyle-Width="140px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtInvoiceNo" runat="server" MaxLength="20" Width="140px" Text='<%# Bind("InvoiceNO")%>'
                                                            Rows="2" ToolTip="Invoice No"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtInvoiceNoF" runat="server" MaxLength="20" Width="140px" ToolTip="Invoice No"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvInvoiceNo" runat="server" ErrorMessage="Enter the Invoice Number"
                                                            ValidationGroup="vgGridAdd" CssClass="styleLoginLabel" ControlToValidate="txtInvoiceNoF"
                                                            Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="140px" HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" ItemStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtInvoiceDate" runat="server" Width="75px" Text='<%# Bind("InvoiceDate")%>'
                                                            ToolTip="Date"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                            TargetControlID="txtInvoiceDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtInvoiceDateF" runat="server" Width="75px" ToolTip="Date"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFVInvoiceDateF" CssClass="styleMandatoryLabel" runat="server"
                                                            ValidationGroup="vgGridAdd" ControlToValidate="txtInvoiceDateF" Display="None"
                                                            ErrorMessage="Select Invoice Date"></asp:RequiredFieldValidator>
                                                        <cc1:CalendarExtender ID="CalendarExtender1F" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                            TargetControlID="txtInvoiceDateF">
                                                        </cc1:CalendarExtender>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Party Name" ItemStyle-Width="140px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPartyName" runat="server" MaxLength="80" Text='<%# Bind("PartyName")%>'
                                                            Width="140px" ToolTip="Party Name"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtPartyNameF" runat="server" MaxLength="80" Width="140px" ToolTip="Party Name"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvPartyName" runat="server" ErrorMessage="Enter the Party Name"
                                                            ValidationGroup="vgGridAdd" CssClass="styleLoginLabel" ControlToValidate="txtPartyNameF"
                                                            Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="140px" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date" ItemStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMaturityDate" runat="server" Text='<%# Bind("MaturityDate")%>'
                                                            Width="75px" ReadOnly="true" ToolTip="Maturity Date" Style="border-color: White"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <%--<asp:TextBox ID="txtMaturityDateF" runat="server" Width="75px" BackColor="Gray" Enabled="false"
                                                            ToolTip="Maturity Date"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtMaturityDateF" runat="server" Width="75px" ToolTip="Date"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RFVtxtMaturityDateF" CssClass="styleMandatoryLabel"
                                                            runat="server" ValidationGroup="vgGridAdd" ControlToValidate="txtMaturityDateF"
                                                            Display="None" ErrorMessage="Select Maturity Date"></asp:RequiredFieldValidator>
                                                        <cc1:CalendarExtender ID="CaltMaturityDateF" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                            TargetControlID="txtMaturityDateF">
                                                        </cc1:CalendarExtender>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-Width="90px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtInvoiceAmount" runat="server" MaxLength="13" Text='<%# Bind("InvoiceAmount")%>'
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Style="text-align: right; border-color: White"
                                                            Width="90px" ReadOnly="true" ToolTip="Invoice Amount"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtInvoiceAmount"
                                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtInvoiceAmountF" runat="server" MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            Width="90px" Style="text-align: right" ToolTip="Invoice Amount"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvInvoiceAmount" runat="server" ErrorMessage="Enter the Invoice Amount"
                                                            CssClass="styleLoginLabel" ControlToValidate="txtInvoiceAmountF" Display="None"
                                                            ValidationGroup="vgGridAdd"></asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtInvoiceAmountF"
                                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="90px" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="150px" HeaderText="Eligible Amount">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtEligibleAmount" runat="server" Style="text-align: right; border-color: White"
                                                            Width="100px" Text='<%# Bind("EligibleAmount")%>' ReadOnly="true" ToolTip="Eligible Amount"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtEligibleAmountF" BackColor="Gray" Enabled="false" Style="text-align: right"
                                                            runat="server" Width="100px" ToolTip="Eligible Amount"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="100px" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Factoring_Inv_Load_Details_ID" HeaderStyle-Width="50px"
                                                    Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("Factoring_Inv_Load_Details_ID")%>'
                                                            Visible="false" ToolTip="SI No."></asp:Label>
                                                        <asp:Label ID="lblLock" runat="server" Text='<%# Bind("Lock")%>' Visible="false"
                                                            ToolTip="SI No."></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" Text="Delete" CommandName="Delete" runat="server"
                                                            CausesValidation="false" ToolTip="Delete" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddrow" OnClientClick="return fnCheckPageValidators('vgGridAdd',false);"
                                                            runat="server" CommandName="AddNew" CssClass="styleGridShortButton" Text="Add"
                                                            ValidationGroup="vgGridAdd" ToolTip="Add"></asp:Button>
                                                    </FooterTemplate>
                                                    <ControlStyle Width="50px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <%--</div>--%>
                                    </td>
                                </tr>
                                <%--Total--%>
                                <tr class="styleGridHeader" align="right">
                                    <td>
                                        <asp:Label runat="server" Text="Total Invoice Amount" BackColor="White" ID="lbltotal"
                                            Font-Bold="True" CssClass="styleGridHeader" Style="padding-right: 5px"></asp:Label>
                                        <asp:TextBox ID="txtTotalInvoiceAmt" Width="100px" Style="text-align: right; border: none"
                                            runat="server" Wrap="False" TabIndex="-1" ToolTip="Total Invoice Amount">0</asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="styleGridHeader" align="right">
                                    <td>
                                        <asp:Label runat="server" Text="Total Eligible Amount" BackColor="White" ID="Label1"
                                            Font-Bold="True" CssClass="styleGridHeader" Style="padding-right: 5px"></asp:Label>
                                        <asp:TextBox ID="txtTotalEligibleAmt" Width="100px" Style="text-align: right; border: none"
                                            runat="server" Wrap="False" TabIndex="-1" ToolTip="Total Eligible Amount">0</asp:TextBox>
                                    </td>
                                </tr>
                            </table>
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
            if (parseFloat(TotalEligibleScore) >= 0)
                document.getElementById('<%=txtTotalEligibleAmt.ClientID%>').value = parseFloat(TotalEligibleScore).toFixed(parseInt(sufLen));

            if (parseFloat(TotalInvoiceScore) >= 0)
                document.getElementById('<%=txtTotalInvoiceAmt.ClientID%>').value = parseFloat(TotalInvoiceScore).toFixed(parseInt(sufLen));
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
