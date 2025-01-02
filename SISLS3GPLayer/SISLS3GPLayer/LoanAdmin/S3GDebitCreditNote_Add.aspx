<%@ Page Title="Debit Credit Note" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GDebitCreditNote_Add.aspx.cs" Inherits="S3GDebitCreditNote_Add" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/S3GAutoSuggest.ascx" TagName="Suggest" TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <script type="text/javascript">
        function Common_ItemSelected(sender, e) {
            var hdnCommonID = $get('<%= hdnCommonID.ClientID %>');
            hdnCommonID.value = e.get_value();
        }
        function Common_ItemPopulated(sender, e) {
            var hdnCommonID = $get('<%= hdnCommonID.ClientID %>');
            hdnCommonID.value = '';

        }
    </script>
    <table width="100%">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                <asp:HiddenField ID="hdnCommonID" runat="server" />
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>
    <table width="100%" align="center" cellpadding="0"
        cellspacing="0" border="1">
        <tr>
            <td valign="top">
                <cc1:TabContainer ID="TCDebitCreditNote" runat="server"
                    CssClass="styleTabPanel" Width="100%" ScrollBars="None"
                    ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" ID="TPDebitCreditNote"
                        CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Debit-Credit Note
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="100%" border="0">
                                        <tr width="100%">
                                            <td>
                                                <asp:Panel ID="pnlDebitCreditnote" runat="server" GroupingText="Debit-Credit Note"
                                                    CssClass="stylePanel">
                                                    <table width="100%" align="center" border="0">
                                                        <tr id="tractivity" runat="server" visible="false">
                                                            <td align="left" class="styleFieldLabel" width="20%">
                                                                <asp:Label runat="server" Text="Activity" CssClass="styleReqFieldLabel"
                                                                    ID="lblActivity"></asp:Label>
                                                            </td>
                                                            <td align="left" class="styleFieldAlign" style="width: 25%">
                                                                <asp:DropDownList ID="ddlActivity" runat="server" AutoPostBack="True"
                                                                    ToolTip="Activity" onmouseover="ddl_itemchanged(this)">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Doc Number" ID="lblDocNumber"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="TextBox1" runat="server" onmouseover="txt_MouseoverTooltip(this)" ReadOnly="true" ToolTip="Doc Number">
                                                                </asp:TextBox>
                                                            </td>

                                                        </tr>

                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Doc Number" ID="Label2"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtDocNumber" runat="server" onmouseover="txt_MouseoverTooltip(this)" ReadOnly="true" ToolTip="Doc Number">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Doc Invoice Number" ID="Label5"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtDocInvoice" runat="server" ReadOnly="true" ToolTip="Doc Invoice Number">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Line of Business" ID="Label4"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddllob" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvLineofBusiness" runat="server" ControlToValidate="ddlLOB"
                                                                    CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                    ErrorMessage="Select a Line of Business" ValidationGroup="Main"></asp:RequiredFieldValidator>

                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Location" ID="lblLocation"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <UC:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                    OnItem_Selected="ddlLocation_OnSelectedIndexChanged"
                                                                    ItemToValidate="Value" IsMandatory="true" ErrorMessage="Select Location" ValidationGroup="Main" />
                                                            </td>
                                                        </tr>
                                                        <tr>

                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Doc Date" ID="lblDate"
                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtDate" runat="server" onmouseover="txt_MouseoverTooltip(this)" AutoPostBack="true" OnTextChanged="txtDate_OnTextChanged"
                                                                    ToolTip="Date">
                                                                </asp:TextBox>
                                                                <asp:Image ID="imgDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <cc1:CalendarExtender runat="server" TargetControlID="txtDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                    PopupButtonID="imgDate" ID="CEDate" Enabled="True">
                                                                </cc1:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server"
                                                                    ControlToValidate="txtDate"
                                                                    ErrorMessage="Select Doc Date" Display="None" SetFocusOnError="True"
                                                                    ValidationGroup="Main" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label3" runat="server" CssClass="styleReqFieldLabel" Text="Value Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtCRDRValueDate" runat="server"></asp:TextBox>
                                                                <asp:Image ID="imgInvoiceDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ErrorMessage="Enter the Value Date" ValidationGroup="Main"
                                                                    ID="rfvMJVValueDate" runat="server" ControlToValidate="txtCRDRValueDate" CssClass="styleMandatoryLabel"
                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender ID="CalExtenderValuedate" runat="server" Enabled="True"
                                                                    PopupButtonID="imgInvoiceDate" TargetControlID="txtCRDRValueDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:CustomValidator ID="rfvCompareMJVDate" runat="server" Display="None" CssClass="styleMandatoryLabel"
                                                                    ValidationGroup="Header" ErrorMessage="Difference between Doc Date and CRDR Value Date must be 30 days"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Doc Type" ID="lblDocType"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlDocType" runat="server" ToolTip="Doc Type" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlDocType_OnSelectedIndexChanged"
                                                                    onmouseover="ddl_itemchanged(this)">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvDocType" runat="server"
                                                                    ControlToValidate="ddlDocType" InitialValue="0"
                                                                    ErrorMessage="Select DocType" Display="None" SetFocusOnError="True"
                                                                    ValidationGroup="Main" />
                                                                &nbsp;
                                                                <asp:Label runat="server" Text="Doc Sub Type" ID="lblDocSubType">
                                                                </asp:Label>
                                                                &nbsp;
                                                                 <asp:DropDownList ID="ddlDocSubType" runat="server" ToolTip="Doc Sub Type">
                                                                     <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                                                     <asp:ListItem Value="23" Text="Rental"></asp:ListItem>
                                                                     <asp:ListItem Value="137" Text="Interim Rental"></asp:ListItem>
                                                                     <asp:ListItem Value="105" Text="AMF"></asp:ListItem>
                                                                     <asp:ListItem Value="44" Text="Other Cash Flow"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Due Date" ID="lblDueDate" CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtDueDate" runat="server" ToolTip="Due Date">
                                                                </asp:TextBox>
                                                                <asp:Image ID="imgDueDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ErrorMessage="Enter the Due Date" ValidationGroup="Main"
                                                                    ID="rfvDueDate" runat="server" ControlToValidate="txtDueDate" CssClass="styleMandatoryLabel"
                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender ID="CalExtenderDueDate" runat="server" Enabled="True"
                                                                    PopupButtonID="imgDueDate" TargetControlID="txtDueDate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Entity Type" ID="lblEntityType"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlEntityType" runat="server"
                                                                    ToolTip="EntityType" AutoPostBack="true" OnSelectedIndexChanged="ddlEntityType_SelectedIndexChanged"
                                                                    onmouseover="ddl_itemchanged(this)">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvEntityType" runat="server"
                                                                    ControlToValidate="ddlEntityType" InitialValue="0"
                                                                    ErrorMessage="Select Entity Type" Display="None"
                                                                    SetFocusOnError="True" ValidationGroup="Main" />
                                                            </td>
                                                            <td class="styleFieldLabel" width="15%">
                                                                <asp:Label runat="server" ToolTip="Code" Text="Code"
                                                                    ID="Label1" CssClass="styleMandatoryLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" width="28%">
                                                                    <asp:TextBox ID="txtCode" ToolTip="Code" runat="server"
                                                                        Style="display: none;" MaxLength="50" Width="65%" ReadOnly="true"></asp:TextBox>
                                                                    <uc2:LOV ID="ucLov" onblur="fnLoadEntity()" runat="server"
                                                                        DispalyContent="Code" TextWidth="65%" />
                                                                    <asp:Button ID="btnCreateCustomer" runat="server" UseSubmitBehavior="true"
                                                                        Text="Create" Style="display: none;" OnClick="btnCreateCustomer_Click"
                                                                        CssClass="styleSubmitShortButton" CausesValidation="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Rental Schedule No." ID="lblContractNo"
                                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc3:Suggest ID="txtMLASearch" runat="server" ServiceMethod="GetMLAList" AutoPostBack="true"
                                                                    OnItem_Selected="txtMLASearch_OnTextChanged" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Invoice No" ID="lblInvoiceNo"
                                                                    CssClass="styleReqFRieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc3:Suggest ID="ddlInvoiceNo" runat="server" AutoPostBack="true" ServiceMethod="GetInvoiceList" Width="150px"
                                                                    OnItem_Selected="ddlInvoiceNo_Item_Selected" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Document Status" ID="lblNoteStatus"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtNoteStatus" runat="server" onmouseover="txt_MouseoverTooltip(this)" ReadOnly="true"
                                                                    ToolTip="Document Status"></asp:TextBox>
                                                                <asp:HiddenField ID="hfdcnstatus" runat="server" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Doc Invoice Status" ID="Label6"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="taxDocInvStatus" runat="server" onmouseover="txt_MouseoverTooltip(this)" ReadOnly="true"
                                                                    ToolTip="Doc Invoice Status"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Billing State" ID="lblBillingState"
                                                                    CssClass="styleReqFieldLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <UC:Suggest ID="ddlBillingState" runat="server" ServiceMethod="GetBillingState" AutoPostBack="true"
                                                                    OnItem_Selected="ddlBillingState_OnSelectedIndexChanged" ItemToValidate="Value" IsMandatory="true"
                                                                    ErrorMessage="Select Billing State" ValidationGroup="Main" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Doc Amount" ID="lblDocAmount">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtDocAmount" runat="server" Style="text-align: right;" onmouseover="txt_MouseoverTooltip(this)" ToolTip="Doc Amount"
                                                                    ReadOnly="true">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvdocamount" runat="server"
                                                                    ControlToValidate="txtDocAmount" InitialValue="0"
                                                                    ErrorMessage="Enter Document Amount" Display="None"
                                                                    SetFocusOnError="True" ValidationGroup="Main" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label runat="server" Text="Remarks" ID="lblRemarks"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)" runat="server"
                                                                    ToolTip="Remarks"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="PnlEntityInformation" runat="server"
                                                    ToolTip="Customer/Entity" GroupingText="Customer/Entity Informations"
                                                    CssClass="stylePanel">
                                                    <table width="100%" border="1">

                                                        <tr>
                                                            <td>
                                                                <uc1:S3GCustomerAddress ID="ucFAAddressDetail" ShowCustomerName="true" ActiveViewIndex="1"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="PanelGLSLDetails" ToolTip="Account Details"
                                                    Width="99%" runat="server" GroupingText="Account Details"
                                                    CssClass="stylePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:GridView runat="server" ShowFooter="true" ID="grvGLSLDetails"
                                                                    Width="100%" ToolTip="Account Details" Visible="true"
                                                                    OnRowDataBound="grvGLSLDetails_RowDataBound" OnRowCommand="grvGLSLDetails_RowCommand"
                                                                    OnRowDeleting="grvGLSLDetails_RowDeleting" RowStyle-HorizontalAlign="Center"
                                                                    HeaderStyle-CssClass="styleGridHeader" FooterStyle-HorizontalAlign="Center"
                                                                    AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                                <asp:HiddenField ID="hdnSlno" runat="server" Value='<%#Eval("Tran_Details_ID") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="CashFlow Flag" FooterStyle-Width="10%"
                                                                            ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCFid" ToolTip="CashFlow Flag" Visible="false"
                                                                                    Width="100%" runat="server" Text='<%#Eval("CashFlow_Flag_Id")%>'></asp:Label>
                                                                                <asp:Label ID="lblCFName" ToolTip="CashFlow Flag"
                                                                                    Width="100%" runat="server" Text='<%#Eval("CashFlow_Flag_Name")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:DropDownList ID="ddlcashflow_id" runat="server">
                                                                                </asp:DropDownList>
                                                                            </EditItemTemplate>
                                                                            <FooterTemplate>
                                                                                <br></br>
                                                                                <asp:HiddenField ID="hdn_Cashflow_id" runat="server" />

                                                                                <uc3:Suggest ID="txtFooterCashflow" runat="server" ToolTip="Cash Flow" AutoPostBack="true" OnItem_Selected="txtFooterCashflow_Item_Selected" ServiceMethod="GetCashflow" />

                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Account" FooterStyle-Width="10%"
                                                                            ItemStyle-Width="10%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGLAccountI" ToolTip="GL Account"
                                                                                    Width="100%" runat="server" Text='<%#Eval("GL_Desc")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:DropDownList ID="ddlGLCodeEdit" runat="server"
                                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlGLCodeEdit_OnSelectedIndexChanged"
                                                                                    onmouseover="ddl_itemchanged(this)">
                                                                                </asp:DropDownList>

                                                                            </EditItemTemplate>
                                                                            <FooterTemplate>
                                                                                <br></br>
                                                                                <asp:HiddenField ID="hdn_AccNature" runat="server" />

                                                                                <uc3:Suggest ID="txtFooterGLAccount" runat="server" ToolTip="GL Account" AutoPostBack="true"
                                                                                    OnItem_Selected="ddlGLCodeEdit_OnSelectedIndexChanged" ServiceMethod="GetGLAccount" />

                                                                                <asp:HiddenField ID="hdnglcode" runat="server" />

                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Sub Account">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSubAccountI" ToolTip="Sub Account"
                                                                                    Width="100%" runat="server" Text='<%#Eval("SL_Desc")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <%--  <EditItemTemplate>
                                                                                <asp:DropDownList ID="ddlSLCodeEdit" onmouseover="ddl_itemchanged(this)"
                                                                                    runat="server">
                                                                                </asp:DropDownList>
                                                                            </EditItemTemplate>--%>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblTot" runat="server" Text="Total :" />
                                                                                <br></br>
                                                                                <uc3:Suggest ID="txtFooterSubGLAccount" runat="server" ToolTip="GL Account" ServiceMethod="GetSubGLAccount" />
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right"
                                                                            HeaderText="Amount" FooterStyle-Width="20%" ItemStyle-Width="20%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAmount" ToolTip="Amount" Width="95%"
                                                                                    runat="server" Text='<%#Eval("Amount")%>'></asp:Label>

                                                                                <asp:Label ID="lblParentID"
                                                                                    runat="server" Text='<%#Eval("ParentID")%>' Visible="false"></asp:Label>

                                                                                <asp:Label ID="lblTranDetailsID"
                                                                                    runat="server" Text='<%#Eval("Tran_Details_ID")%>' Visible="false"></asp:Label>


                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtTargetAmount" Text='<%#Bind("Amount") %>'
                                                                                    runat="server" MaxLength="20" onmouseover="txt_MouseoverTooltip(this)" Style="text-align: right;"
                                                                                    ToolTip="Target Amount" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                                                </asp:TextBox>
                                                                            </EditItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txttotaldue" ToolTip="Amount" ReadOnly="true"
                                                                                    Width="95%" runat="server" MaxLength="20" Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                                <br></br>
                                                                                <asp:TextBox ID="txtFooterAmount" ToolTip="Amount" MaxLength="15"
                                                                                    runat="server" Style="text-align: right" onmouseover="txt_MouseoverTooltip(this)" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox><asp:Label
                                                                                        ID="lblFooterActualAmount" Width="100%" runat="server"
                                                                                        Visible="false"></asp:Label>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1Hdr" runat="server" TargetControlID="txtFooterAmount"
                                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtFooterAmount" runat="server" ControlToValidate="txtFooterAmount"
                                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="vgAdd"
                                                                                    ErrorMessage="Enter the Amount"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="left"
                                                                            HeaderText="Narration">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblNarration" ToolTip="Narration" Width="100%"
                                                                                    runat="server" Text='<%#Eval("Narration")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:TextBox ID="txtTargetNarration" onkeyup="maxlengthfortxt(100)" onmouseover="txt_MouseoverTooltip(this)" Text='<%#Bind("Narration")%>'
                                                                                    runat="server" MaxLength="100" Style="text-align: left;"
                                                                                    ToolTip="Narration" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                            </EditItemTemplate>
                                                                            <FooterTemplate>
                                                                                <br></br>
                                                                                <asp:TextBox ID="txtFooterNarration" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)" ToolTip="Narration" onmouseover="txt_MouseoverTooltip(this)"
                                                                                    runat="server" MaxLength="100" Style="text-align: left"></asp:TextBox>
                                                                                <%--<asp:RequiredFieldValidator ID="rfvtxtFooterNarration" runat="server" ControlToValidate="txtFooterNarration"
                                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="vgAdd"
                                                                                    ErrorMessage="Enter the Narration"></asp:RequiredFieldValidator>
                                                                                <headerstyle cssclass="styleGridHeader" />--%>
                                                                                <itemstyle horizontalalign="Left" />
                                                                            </FooterTemplate>

                                                                        </asp:TemplateField>

																		<asp:TemplateField HeaderText="Invoice Id" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblInvoiceId" runat="server" Text='<%#Eval("Invoice_Id")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkRemove" runat="server" Text="Delete" ToolTip="Delete"
                                                                                    CommandName="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                                    CausesValidation="false"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <EditItemTemplate>
                                                                                <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update"
                                                                                    CommandName="Update" CausesValidation="false" /><asp:LinkButton
                                                                                        ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                                                        CausesValidation="false" />
                                                                            </EditItemTemplate>
                                                                            <FooterTemplate>
                                                                                <br></br>
                                                                                <asp:LinkButton ID="lnkAdd" Width="100%" ToolTip="Add to the grid"
                                                                                    CommandName="Add" ValidationGroup="vgAdd"
                                                                                    runat="server" Text="Add"></asp:LinkButton>
                                                                            </FooterTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" align="center">
                                        <tr>
                                            <td>
                                                <asp:Panel GroupingText="Print Details" ID="pnlPrintDetails" runat="server" CssClass="stylePanel">
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
                                        <tr>
                                            <td align="center" colspan="4">&nbsp;<asp:Button ID="btnSave" runat="server" CausesValidation="false" ToolTip="Save"
                                                CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('Main');"
                                                ValidationGroup="Main" OnClick="btnSave_Click"
                                                Text="Save" />
                                                &nbsp;<asp:Button ID="btnClear" ToolTip="Clear"
                                                    runat="server" OnClick="btnClear_Click" CausesValidation="false"
                                                    CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();"
                                                    Text="Clear" />
                                                &nbsp;<asp:Button ID="btnBack" ToolTip="Back"
                                                    OnClick="btnBack_Click" runat="server" CausesValidation="false"
                                                    CssClass="styleSubmitButton" Text="Back" />
                                                &nbsp;
                                               <asp:Button ID="btn_print" ToolTip="Print"
                                                   OnClick="btn_print_Click" runat="server" CausesValidation="false"
                                                   CssClass="styleSubmitButton" Text="Print" />

                                                <asp:Button ID="btnCancel" ToolTip="Cancel Invoice"
                                                    runat="server" CausesValidation="false" OnClick="btnCancel_Click"
                                                    CssClass="styleSubmitButton" Text="Cancel Invoice" />
                                                <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to Cancel?"
                                                    Enabled="True" TargetControlID="btnCancel">
                                                </cc1:ConfirmButtonExtender>
                                                &nbsp;&nbsp;&nbsp;&nbsp;

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ValidationSummary ID="vsDCNote" runat="server" ShowSummary="true"
                                                    CssClass="styleMandatoryLabel" ValidationGroup="Main" ShowMessageBox="false"
                                                    HeaderText="Correct the following validation(s):" />
                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true"
                                                    CssClass="styleMandatoryLabel" ValidationGroup="vgAdd" ShowMessageBox="false"
                                                    HeaderText="Correct the following validation(s):" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:CustomValidator ID="cvNote" runat="server" Display="None"
                                                    CssClass="styleMandatoryLabel" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                    <asp:PostBackTrigger ControlID="btnClear" />
                                    <asp:PostBackTrigger ControlID="btn_print" />
                                    <asp:PostBackTrigger ControlID="btnCreateCustomer" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        function fnLoadEntity() {

            document.getElementById('<%=btnCreateCustomer.ClientID %>').click();
        }
    </script>
</asp:Content>
