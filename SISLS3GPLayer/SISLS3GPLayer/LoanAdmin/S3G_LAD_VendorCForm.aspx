<%@ Page Title="Vendor C Form" Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3G_LAD_VendorCForm.aspx.cs" Inherits="LoanAdmin_S3G_LAD_VendorCForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LOV" Src="~/UserControls/LOBMasterView.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="Vendor C Form - Data Entry" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlVendorType" runat="server" CssClass="stylePanel" Style="width: 330px;" GroupingText="Vendor - C Types">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" class="styleFieldLabel">
                                        <asp:RadioButtonList runat="server" ID="rdbtnType" CellPadding="15" AutoPostBack="true" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rdbtnType_SelectedIndexChanged">
                                            <asp:ListItem Text="Purchase" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="RentalSchedule" Value="2"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlPurchase" runat="server" CssClass="stylePanel" Width="90%" GroupingText="Purchase Option" Visible="false">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>

                                    <td style="width: 15%; padding-left: 10px;" align="left">
                                        <asp:Label ID="lblVendorName" runat="server" class="styleFieldLabel" Text="Vendor Name"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left">
                                        <uc2:Suggest ID="ddlVendName" runat="server" ServiceMethod="GetVendorName" ErrorMessage="Enter Vendor Name" />
                                    </td>

                                    <td style="width: 15%; padding-left: 10px;" align="left">
                                        <asp:Label ID="lblDeliveryState" runat="server" class="styleFieldLabel" Text="Delivery State"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left">
                                        <asp:DropDownList ID="ddlComState" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>

                                    <td style="width: 15%; padding-left: 10px;" align="left">
                                        <asp:Label ID="lblFromDate" runat="server" class="styleFieldLabel" Text="From Date"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left">
                                        <asp:TextBox ID="txtFromDate" runat="server" ToolTip="From Date"></asp:TextBox>
                                        <asp:Image ID="ImgtxtOfferDate" runat="server" ToolTip="From Date" ImageUrl="~/Images/calendaer.gif" />
                                        <%-- <asp:RequiredFieldValidator ID="rfvtxtOfferDate" ToolTip="From Date" runat="server" Display="None"
                                            ValidationGroup="Main Page" ErrorMessage="Enter the Proposal Date" ControlToValidate="txtOfferDate"
                                            SetFocusOnError="False"></asp:RequiredFieldValidator>--%>
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            TargetControlID="txtFromDate" PopupButtonID="ImgtxtOfferDate" ID="CEtxtFromDate"
                                            Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td style="width: 15%; padding-left: 10px;" align="left">
                                        <asp:Label ID="lblToDate" runat="server" class="styleFieldLabel" Text="To Date"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left">
                                        <asp:TextBox ID="txtToDate" runat="server" ToolTip="To Date"></asp:TextBox>
                                        <asp:Image ID="imgToDate" runat="server" ToolTip="To Date" ImageUrl="~/Images/calendaer.gif" />
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ToolTip="To Date" runat="server" Display="None"
                                            ValidationGroup="Main Page" ErrorMessage="Enter the Proposal Date" ControlToValidate="txtOfferDate"
                                            SetFocusOnError="False"></asp:RequiredFieldValidator>--%>
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            TargetControlID="txtToDate" PopupButtonID="imgToDate" ID="CEtxtToDate"
                                            Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; padding-left: 10px;" align="left">
                                        <asp:Label ID="lblStatus" runat="server" class="styleFieldLabel" Text="Status"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left" colspan="3">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="165px">
                                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Filled" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Unfilled" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="width: 35%; padding-left: 15px;" align="center">
                                        <asp:Button ID="btnPurchaseSearch" runat="server" Text="Search" CssClass="styleSubmitButton"
                                            CausesValidation="true" ValidationGroup="PurchaseSearch" OnClick="btnPurchaseSearch_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="gvVendorCFormPurchase" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
                                            OnRowDataBound="gvVendorCFormPurchase_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Copy From">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkFrom" runat="server" AutoPostBack="true" OnCheckedChanged="chkFrom_OnCheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Copy To">
                                                    <HeaderTemplate>
                                                        Copy To
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkTo" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl. No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPSlno" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="PA_SA_REF_ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSID" runat="server" Text='<%#Eval("PA_SA_REF_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="ENTITY_ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorID" runat="server" Text='<%#Eval("ENTITY_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="RS No.">
                                                    <%--  <FooterTemplate>
                                                        <uc2:Suggest ID="ddlFRSNo" AutoPostBack="true" OnItem_Selected="ddlFRSNo_OnItemSelected" runat="server" ServiceMethod="GetRentalScheduleNo" IsMandatory="true" ValidationGroup="FooterPurchase" ErrorMessage="Enter RS No" />
                                                    </FooterTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSNo" runat="server" Text='<%#Eval("PANUM")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Vendor Name">
                                                    <%--<FooterTemplate>
                                                        <uc2:Suggest ID="ddlFVendor" AutoPostBack="true" OnItem_Selected="ddlFVendor_OnItemSelected" runat="server" ServiceMethod="GetVendorName" IsMandatory="true" ValidationGroup="FooterPurchase" ErrorMessage="Enter Vendor Name" />
                                                    </FooterTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIVendorName" runat="server" Text='<%#Eval("ENTITY_NAME")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delivery State">
                                                    <%--<FooterTemplate>
                                                        <uc2:Suggest ID="ddlFVendor" AutoPostBack="true" OnItem_Selected="ddlFVendor_OnItemSelected" runat="server" ServiceMethod="GetVendorName" IsMandatory="true" ValidationGroup="FooterPurchase" ErrorMessage="Enter Vendor Name" />
                                                    </FooterTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIDeliveryState" runat="server" Text='<%#Eval("DEL_STATE")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="DeliveryStateID" Visible="false">
                                                    <%--<FooterTemplate>
                                                        <uc2:Suggest ID="ddlFVendor" AutoPostBack="true" OnItem_Selected="ddlFVendor_OnItemSelected" runat="server" ServiceMethod="GetVendorName" IsMandatory="true" ValidationGroup="FooterPurchase" ErrorMessage="Enter Vendor Name" />
                                                    </FooterTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIDeliveryStateID" runat="server" Text='<%#Eval("DEL_STATE_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Vendor Invoice No">
                                                    <%--                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlFVendorNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFVendorNo_OnSelectedIndexChanged"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvFVendorNo" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="ddlFVendorNo" ErrorMessage="Select Vendor Invoice No" InitialValue="0"
                                                            ValidationGroup="FooterPurchase" Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorNum" runat="server" Text='<%#Eval("INVOICE_NO")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Vendor Invoice Date">
                                                    <%--<FooterTemplate>
                                                        <asp:Label ID="lblFInvoiceDate" runat="server"></asp:Label>
                                                    </FooterTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("INVOICE_DATE")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Invoice Amount">
                                                    <%--<FooterTemplate>
                                                        <asp:Label ID="lblFInvoiceAmt" runat="server"></asp:Label>
                                                    </FooterTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceAmt" runat="server" Text='<%#Eval("INVOICE_AMT")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="C Form No">
                                                    <%--  <FooterTemplate>
                                                        <asp:TextBox ID="txtFCForm" runat="server" MaxLength="15"
                                                            Width="98%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvFCForm" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtFCForm" ErrorMessage="Enter the C Form No"
                                                            ValidationGroup="FooterPurchase" Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>--%>
                                                    <%-- <EditItemTemplate>
                                                        <asp:TextBox ID="txtECForm" runat="server" MaxLength="15" Width="98%" Text='<%#Eval("C_FORM_NO")%>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvECForm" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtECForm" ErrorMessage="Enter C Form No"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtECForm" runat="server" MaxLength="15" Width="98%" Text='<%#Eval("C_FORM_NO")%>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvECForm" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtECForm" ErrorMessage="Enter C Form No"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Issue Date">
                                                    <%-- <FooterTemplate>
                                                        <asp:TextBox ID="txtFPIssueDate" runat="server" Width="97%"
                                                            AutoPostBack="true" OnTextChanged="txtFPIssueDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEFPIssueDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image5" TargetControlID="txtFPIssueDate">
                                                        </cc1:CalendarExtender>
                                                       <asp:RequiredFieldValidator ID="rfvPID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtFPIssueDate" ErrorMessage="Enter Issue Date"
                                                            ValidationGroup="FooterPurchase" Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>--%>
                                                    <%--  <EditItemTemplate>
                                                        <asp:TextBox ID="txtEPIssueDate" runat="server" Width="97%" Text='<%# Eval("ISSUE_DATE") %>'
                                                            AutoPostBack="true" OnTextChanged="txtEPIssueDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEEPIssueDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image6" TargetControlID="txtEPIssueDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPEID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtEPIssueDate" ErrorMessage="Enter Issue Date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtEPIssueDate" runat="server" Width="97%" Text='<%# Eval("ISSUE_DATE") %>'
                                                            AutoPostBack="true" OnTextChanged="txtEPIssueDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEEPIssueDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image6" TargetControlID="txtEPIssueDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPEID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtEPIssueDate" ErrorMessage="Enter Issue Date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Valid Till Date">
                                                    <%-- <FooterTemplate>
                                                        <asp:TextBox ID="txtFPValidTillDate" runat="server" Width="97%"
                                                            AutoPostBack="true" OnTextChanged="txtFPValidTillDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEFPValidTill" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image7" TargetControlID="txtFPValidTillDate">
                                                        </cc1:CalendarExtender>
                                                         <asp:RequiredFieldValidator ID="rfvPVD" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtFPValidTillDate" ErrorMessage="Enter valid till date"
                                                            ValidationGroup="FooterPurchase" Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>--%>
                                                    <%--<EditItemTemplate>
                                                        <asp:TextBox ID="txtEPValidTillDate" runat="server" Width="97%" Text='<%# Eval("VALID_TLL_DATE") %>'
                                                            AutoPostBack="true" OnTextChanged="txtEPValidTillDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEEPValidTill" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image8" TargetControlID="txtEPValidTillDate">
                                                        </cc1:CalendarExtender>
                                                         <asp:RequiredFieldValidator ID="rfvPEVD" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtEPValidTillDate" ErrorMessage="Enter valid till date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtEPValidTillDate" runat="server" Width="97%" Text='<%# Eval("VALID_TLL_DATE") %>'
                                                            AutoPostBack="true" OnTextChanged="txtEPValidTillDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEEPValidTill" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image8" TargetControlID="txtEPValidTillDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPEVD" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtEPValidTillDate" ErrorMessage="Enter valid till date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                    <%--<FooterTemplate>
                                                        <asp:TextBox ID="txtFRemarks" Text='<%#Eval("REMARKS")%>' runat="server" TextMode="MultiLine" MaxLength="100" onkeyup="return maxlengthfortxt('100');"
                                                            Width="98%"></asp:TextBox>
                                                    </FooterTemplate>--%>
                                                    <%-- <EditItemTemplate>
                                                        <asp:TextBox ID="txtERemarks" runat="server" TextMode="MultiLine" Text='<%#Eval("REMARKS")%>'
                                                             MaxLength="100" onkeyup="return maxlengthfortxt('100');"
                                                            Width="98%"></asp:TextBox>
                                                    </EditItemTemplate>--%>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtERemarks" runat="server" TextMode="MultiLine" Text='<%#Eval("REMARKS")%>'
                                                            MaxLength="100" onkeyup="return maxlengthfortxt('100');"
                                                            Width="98%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:TemplateField HeaderText="">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            ValidationGroup="Update" ToolTip="Update"></asp:LinkButton>&nbsp;|
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPEdit" runat="server" Text="Edit" CommandName="Edit"
                                                            ToolTip="Edit">
                                                        </asp:LinkButton>
                                                        &nbsp;|
                                                        <asp:LinkButton ID="lnkPDelete" runat="server" ToolTip="Delete" CommandName="Delete" Text="Delete"
                                                            OnClientClick="return confirm('Do you want to Delete this record?');">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center" valign="top">
                                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlRentalSchedule" runat="server" CssClass="stylePanel" Width="90%" GroupingText="Rental Schedule Option" Visible="false">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 15%; padding-left: 10px;" align="left">
                                        <asp:Label ID="Label1" runat="server" class="styleFieldLabel" Text="Customer Name"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left">
                                        <uc2:Suggest ID="ddlCustomerName" runat="server" ServiceMethod="GetCustomerName" ErrorMessage="Enter Customer Name" />
                                    </td>

                                    <td style="width: 15%;">
                                        <asp:Label ID="lblRRSNo" runat="server" class="styleFieldLabel" Text="RS No."></asp:Label>
                                    </td>
                                    <td style="width: 15%;">
                                        <uc2:Suggest ID="ddlRRSNo" runat="server" ServiceMethod="GetRentalScheduleNoForRS" />
                                    </td>
                                </tr>
                                <tr>

                                    <td style="width: 15%; padding-left: 10px;" align="left">
                                        <asp:Label ID="Label3" runat="server" class="styleFieldLabel" Text="Rental Invoice From Date"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left">
                                        <asp:TextBox ID="TextBox1" runat="server" ToolTip="From Date"></asp:TextBox>
                                        <asp:Image ID="Image1" runat="server" ToolTip="From Date" ImageUrl="~/Images/calendaer.gif" />
                                        <%-- <asp:RequiredFieldValidator ID="rfvtxtOfferDate" ToolTip="From Date" runat="server" Display="None"
                                            ValidationGroup="Main Page" ErrorMessage="Enter the Proposal Date" ControlToValidate="txtOfferDate"
                                            SetFocusOnError="False"></asp:RequiredFieldValidator>--%>
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            TargetControlID="TextBox1" PopupButtonID="Image1" ID="CalendarExtender1"
                                            Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td style="width: 15%;">
                                        <asp:Label ID="Label4" runat="server" class="styleFieldLabel" Text="To Date"></asp:Label>
                                    </td>
                                    <td style="width: 15%;" align="left">
                                        <asp:TextBox ID="TextBox2" runat="server" ToolTip="To Date"></asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" ToolTip="To Date" ImageUrl="~/Images/calendaer.gif" />
                                        <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ToolTip="To Date" runat="server" Display="None"
                                            ValidationGroup="Main Page" ErrorMessage="Enter the Proposal Date" ControlToValidate="txtOfferDate"
                                            SetFocusOnError="False"></asp:RequiredFieldValidator>--%>
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            TargetControlID="TextBox2" PopupButtonID="Image2" ID="CalendarExtender2"
                                            Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="width: 35%; padding-left: 15px;" align="center">
                                        <asp:Button ID="btnRSSearch" runat="server" Text="Search" CssClass="styleSubmitButton"
                                            CausesValidation="true" ValidationGroup="RSSearch" OnClick="btnRSSearch_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:GridView ID="gvVendorCFRS" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
                                            OnRowDataBound="gvVendorCFormPurchase_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Copy From">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkFrom" runat="server" AutoPostBack="true" OnCheckedChanged="chkFrom_OnCheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Copy To">
                                                    <HeaderTemplate>
                                                        Copy To
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkTo" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl. No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPSlno" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PA_SA_REF_ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSID" runat="server" Text='<%#Eval("PA_SA_REF_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Customer_ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustID" runat="server" Text='<%#Eval("Customer_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="RS No.">
                                                   <ItemTemplate>
                                                        <asp:Label ID="lblRSNo" runat="server" Text='<%#Eval("PANUM")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Customer Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("Customer_Name")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Invoice No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorNum" runat="server" Text='<%#Eval("INVOICE_NO")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Invoice Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Eval("INVOICE_DATE")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Invoice Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceAmt" runat="server" Text='<%#Eval("INVOICE_AMT")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="C Form No">
                                                   <ItemTemplate>
                                                        <asp:TextBox ID="txtECForm" runat="server" MaxLength="15" Width="98%" Text='<%#Eval("C_FORM_NO")%>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvECForm" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtECForm" ErrorMessage="Enter C Form No"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Issue Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtEPIssueDate" runat="server" Width="97%" Text='<%# Eval("ISSUE_DATE") %>'
                                                            AutoPostBack="true" OnTextChanged="txtEPIssueDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image6" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEEPIssueDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image6" TargetControlID="txtEPIssueDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPEID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtEPIssueDate" ErrorMessage="Enter Issue Date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Valid Till Date">
                                                     <ItemTemplate>
                                                        <asp:TextBox ID="txtEPValidTillDate" runat="server" Width="97%" Text='<%# Eval("VALID_TLL_DATE") %>'
                                                            AutoPostBack="true" OnTextChanged="txtEPValidTillDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEEPValidTill" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image8" TargetControlID="txtEPValidTillDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPEVD" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtEPValidTillDate" ErrorMessage="Enter valid till date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                   <ItemTemplate>
                                                        <asp:TextBox ID="txtERemarks" runat="server" TextMode="MultiLine" Text='<%#Eval("REMARKS")%>'
                                                            MaxLength="100" onkeyup="return maxlengthfortxt('100');"
                                                            Width="98%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--<asp:GridView ID="gvVendorCFormRS" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="100%"
                                            OnRowCancelingEdit="gvVendorCFormRS_RowCancelingEdit" OnRowEditing="gvVendorCFormRS_RowEditing"
                                            OnRowUpdating="gvVendorCFormRS_RowUpdating" OnRowDeleting="gvVendorCFormRS_RowDeleting" DataKeyNames="RowNumber"
                                            OnRowCommand="gvVendorCFormRS_RowCommand" OnRowDataBound="gvVendorCFormRS_RowDataBound" Visible="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl. No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSSlno" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="VENDORC_ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorCID" runat="server" Text='<%#Eval("VENDORC_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="PA_SA_REF_ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSID" runat="server" Text='<%#Eval("PA_SA_REF_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="CUSTOMER_ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustID" runat="server" Text='<%#Eval("CUSTOMER_ID")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="RS No.">
                                                    <FooterTemplate>
                                                        <uc2:Suggest ID="ddlFRSNoRS" runat="server" AutoPostBack="true" OnItem_Selected="ddlFRSNoRS_OnItemSelected"
                                                            ServiceMethod="GetRentalScheduleNoForRS" IsMandatory="true" ValidationGroup="FooterRS" ErrorMessage="Enter RS No" />
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRSNo" runat="server" Text='<%#Eval("PANUM")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Customer Name">
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblICustomerName" runat="server"></asp:Label>
                                                        <asp:HiddenField ID="hdnCustomerValue" runat="server" />
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("CUSTOMER_NAME")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="C Form No">
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFRSCForm" runat="server" MaxLength="15" Text='<%#Eval("C_FORM_NO")%>'
                                                            Width="98%"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvFCForm" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtFRSCForm" ErrorMessage="Enter the C Form No"
                                                            ValidationGroup="FooterRS" Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtERSCForm" runat="server" MaxLength="15" Width="98%" Text='<%#Eval("C_FORM_NO")%>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvECForm" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtERSCForm" ErrorMessage="Enter C Form No"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCformNo" runat="server" Text='<%#Eval("C_FORM_NO")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Issue Date">
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFRSIssueDate" runat="server" Width="97%"
                                                            AutoPostBack="true" OnTextChanged="txtFRSIssueDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEFRSIssueDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image3" TargetControlID="txtFRSIssueDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvRID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtFRSIssueDate" ErrorMessage="Enter Issue Date"
                                                            ValidationGroup="FooterRS" Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtERSIssueDate" runat="server" Width="97%" Text='<%# Eval("ISSUE_DATE") %>'
                                                            AutoPostBack="true" OnTextChanged="txtERSIssueDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image9" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEERSIssueDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image9" TargetControlID="txtERSIssueDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvREID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtERSIssueDate" ErrorMessage="Enter Issue Date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueDate" runat="server" Text='<%#Eval("ISSUE_DATE")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Valid Till Date">
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFRSValidTillDate" runat="server" Width="97%" AutoPostBack="true"
                                                            OnTextChanged="txtFRSValidTillDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEFRSValidTill" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image4" TargetControlID="txtFRSValidTillDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvRVD" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtFRSValidTillDate" ErrorMessage="Enter valid till date"
                                                            ValidationGroup="FooterRS" Display="None"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtERSValidTillDate" runat="server" Width="97%" Text='<%# Eval("VALID_TLL_DATE") %>' AutoPostBack="true"
                                                            OnTextChanged="txtERSValidTillDate_TextChanged"></asp:TextBox>
                                                        <asp:Image ID="Image10" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEERSValidTill" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image10" TargetControlID="txtERSValidTillDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvREVD" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtERSValidTillDate" ErrorMessage="Enter valid till date"
                                                            ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValidTill" runat="server" Text='<%#Eval("VALID_TLL_DATE")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Remarks">
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFRSRemarks" runat="server" TextMode="MultiLine" MaxLength="100" onkeyup="return maxlengthfortxt('100');"
                                                            Width="98%"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtERSRemarks" runat="server" TextMode="MultiLine" Text='<%#Eval("REMARKS")%>'
                                                            MaxLength="100" onkeyup="return maxlengthfortxt('100');"
                                                            Width="98%"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS")%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="">
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAdd" Width="50px" runat="server" CommandName="AddNew" ValidationGroup="FooterRS"
                                                            CssClass="styleSubmitShortButton" Text="Add"></asp:Button>
                                                    </FooterTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            ValidationGroup="Update" ToolTip="Update"></asp:LinkButton>&nbsp;|
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                            ToolTip="Edit">
                                                        </asp:LinkButton>
                                                        &nbsp;|
                                                        <asp:LinkButton ID="lnkRSDelete" runat="server" ToolTip="Delete" CommandName="Delete" Text="Delete"
                                                            OnClientClick="return confirm('Do you want to Delete this record?');">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>--%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="4" align="center" valign="top">
                                        <uc1:PageNavigator ID="ucCustomPagingRS" runat="server"></uc1:PageNavigator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnMove" CssClass="styleSubmitButton" OnClick="btnMove_Click"
                            Text="Move" />
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton"
                            Text="Save" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                    </td>
                </tr>
            <tr>
                <td>
                    <asp:ValidationSummary runat="server" ID="vsVendorCP" HeaderText="Please correct the following validation(s):"
                        Height="250px" ValidationGroup="PurchaseSearch" CssClass="styleMandatoryLabel" ShowMessageBox="false"
                        ShowSummary="true" />
                    <asp:ValidationSummary ID="vsVendorCRS" runat="server" CssClass="styleMandatoryLabel" Height="250px"
                        HeaderText="Correct the following validation(s):  " ValidationGroup="RSSearch"
                        ShowMessageBox="false" ShowSummary="true" />
                    <asp:ValidationSummary ID="vsVendorPurchase" runat="server" CssClass="styleMandatoryLabel" Height="250px"
                        HeaderText="Correct the following validation(s):  " ValidationGroup="FooterPurchase"
                        ShowMessageBox="false" ShowSummary="true" />
                    <asp:ValidationSummary ID="vsVendorRS" runat="server" CssClass="styleMandatoryLabel" Height="250px"
                        HeaderText="Correct the following validation(s):  " ValidationGroup="FooterRS"
                        ShowMessageBox="false" ShowSummary="true" />
                    <asp:CustomValidator ID="cvVendor" runat="server" CssClass="styleMandatoryLabel"
                        Enabled="true" Width="98%" />
                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                </td>
            </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rdbtnType" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
