<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="~/LegalRepossession/S3GLRSaleInvoiceEntry.aspx.cs" Inherits="LegalRepossession_S3GLRSaleInvoiceEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
    <%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" ID="ContentSalesInvoice"
    runat="server">
    <asp:UpdatePanel ID="UpdatePanelAsset" runat="server">
        <ContentTemplate>
            <table cellspacing="0" width="100%">
                <tr>
                    <td class="stylePageHeading" colspan="4">
                        <asp:Label ID="lblHeading" runat="server" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" width="25%">
                        <asp:Label ID="LblSInvInvNo" runat="server" CssClass="styleReqFieldLabel" Text="Sales Invoice Number"></asp:Label>
                    </td>
                    <td class="styleFieldAlign" width="25%">
                        <asp:TextBox ID="TxtSInvInvNo" runat="server" MaxLength="12" ReadOnly="True" Width="148px"></asp:TextBox>
                    </td>
                    <td class="styleFieldAlign" width="20%">
                        <asp:Label ID="LblSInvInvDate" runat="server" CssClass="styleReqFieldLabel" Text="Sales Invoice Date"></asp:Label>
                    </td>
                    <td class="styleFieldAlign" width="20%">
                        <asp:TextBox ID="TxtSInvInvDate" runat="server" MaxLength="12" ReadOnly="True" Width="148px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="LblSInvLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlSInvLOB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSInvLOB_SelectIndexChanged"
                            Visible="true" >
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvddSInvLOB" runat="server" ControlToValidate="ddlSInvLOB"
                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Line of Business"
                            InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:Label ID="LblSInvBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                    
                    <uc2:Suggest ID="ddlSInvBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlSInvBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                      <%--  <asp:DropDownList ID="ddlSInvBranch" runat="server" AppendDataBoundItems="True" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlSInvBranch_SelectedIndexChanged" Visible="true" >
                        </asp:DropDownList> ddlBranch
                        <asp:RequiredFieldValidator ID="rfvddlSInvBranch" runat="server" ControlToValidate="ddlSInvBranch"
                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Location"
                            InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                        <asp:Label ID="LblSInvSSNo" runat="server" CssClass="styleReqFieldLabel" Text="Sale Notification Number"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlSInvSSNo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSInvSSNo_SelectIndexChanged"
                            Visible="true" >
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvddlSInvSSNo" runat="server" ControlToValidate="ddlSInvSSNo"
                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Sale Notification Number"
                            InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:Label ID="lblSBidIsActive" runat="server" Text="Active"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:CheckBox ID="ChkSInvIsActive" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="LblSInvMLANo" runat="server" CssClass="styleReqFieldLabel" Text="Prime Account Number"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="TxtSInvMLANo" runat="server" AutoCompleteType="Disabled" MaxLength="100"
                            ReadOnly="true" Width="148px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTxtSBidMLANo" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                            TargetControlID="TxtSInvMLANo" ValidChars="-,/, ">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvTxtSInvMLANo" runat="server" ControlToValidate="TxtSInvMLANo"
                            Display="None" ErrorMessage="Enter the MLA Number" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:Label ID="LblSInvSLANo" runat="server" CssClass="styleReqFieldLabel" Text="Sub Account Number"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="TxtSInvSLANo" runat="server" MaxLength="100" ReadOnly="true" Width="148px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTxtSBidSLANo" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                            TargetControlID="TxtSInvSLANo" ValidChars="-,/, ">
                        </cc1:FilteredTextBoxExtender>
                        <%-- <asp:RequiredFieldValidator ID="rfvTxtSInvSLANo" runat="server" 
                            ControlToValidate="TxtSInvSLANo" Display="None" 
                            ErrorMessage="Enter the SLA Number" SetFocusOnError="True" 
                            ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="LblSInvRepDocNo" runat="server" CssClass="styleReqFieldLabel" Text="Repossession Docket Number"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="TxtSInvRepDocNo" runat="server" MaxLength="100" ReadOnly="true"
                            Width="148px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTxtSBidRepDocNo" runat="server" Enabled="True"
                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="TxtSInvRepDocNo"
                            ValidChars="-,/, ">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvTxtSInvRepDocNo" runat="server" ControlToValidate="TxtSInvRepDocNo"
                            Display="None" ErrorMessage="Enter the Repossession Docket Number" SetFocusOnError="True"
                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:Label ID="LblSInvRefNo" runat="server" CssClass="styleReqFieldLabel" Text="Reference Number"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="TxtSInvRefNo" runat="server" MaxLength="100" ReadOnly="true" Width="148px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FTxtSBidRefNo" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                            TargetControlID="TxtSInvRefNo" ValidChars="-,/, ">
                        </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvTxtSInvRefNo" runat="server" ControlToValidate="TxtSInvRefNo"
                            Display="None" ErrorMessage="Enter the Reference Number" SetFocusOnError="True"
                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" colspan="4">
                        <asp:Panel ID="pnlSInvCustInfo" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                            Width="99%">
                            <table cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hidcuscode" runat="server" />
                                        <asp:HiddenField ID="hidcusID" runat="server" />
                                        <uc1:S3GCustomerAddress ID="UcSInvCustomerDetails" ActiveViewIndex="1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" colspan="4">
                        <asp:Panel ID="PNLAssetDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Details"
                            Width="98%" Visible="false">
                            <table cellpadding="0" cellspacing="0" width="98%">
                                <tr>
                                    <td colspan="4 " width="80%">
                                        <%--<div style="overflow-x: hidden; overflow-y: auto; height: 225px;">--%>
                                        <asp:GridView runat="server" ID="GRVAssetDetails" AutoGenerateColumns="False" Width="100%"
                                            ToolTip="Asset Details" OnRowDataBound="GRVAssetDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Asset Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAssetCode" runat="server" Text='<%# Bind("Asset_Code")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAssetDesc" runat="server" Text='<%# Bind("Asset_Description")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Reg Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtAssetRegNo" runat="server" Text='<%# Bind("Regno")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Repossessed Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRepDate" runat="server" Text='<%# Bind("Repossession_Date")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Latest Market Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="txtMarketvalue" runat="server" Text='<%# Bind("Current_Market_Value")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <%--</div>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" colspan="4">
                        <asp:Panel ID="divBidDetails" CssClass="stylePanel" runat="server" ScrollBars="Horizontal"
                            Width="1000px" GroupingText="Bid Details">
                            <asp:GridView ID="GrvBidDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                OnRowDataBound="GrvBidDetails_RowDataBound" Width="100%">
                                <HeaderStyle CssClass="styleGridHeader" />
                                <Columns>
                                    <asp:BoundField DataField="Bid_No" HeaderText="Bid No">
                                        <ItemStyle Wrap="True" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Bidder_Name" HeaderText="Bidder Name">
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Bidder_Address" HeaderText="Bidder Address">
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="VALIDITY" HeaderText="Bid valid Upto">
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Bid_Value" HeaderText="Bid Value">
                                        <ItemStyle Wrap="True" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Bid_Amount" HeaderText="Bid Amount">
                                        <ItemStyle Wrap="True" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" colspan="4">
                        <asp:Panel ID="PnlInvoiceDetails" runat="server" CssClass="stylePanel" GroupingText="Invoice Details">
                            <table cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:GridView ID="GrvSInvoiceDetails" runat="server" AutoGenerateColumns="False"
                                            Width="100%" ShowFooter="True">
                                            <Columns>
                                                <asp:TemplateField HeaderText="AccountType">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                                    <AlternatingItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvAccountType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountType_SelectedIndexChanged">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="C">Customer</asp:ListItem>
                                                            <asp:ListItem Value="E">Entity</asp:ListItem>
                                                            <%--<asp:ListItem Value="G">General</asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvAccountType" runat="server" ControlToValidate="ddlSInvAccountType"
                                                            Display="None" ErrorMessage="Select the Account Type" SetFocusOnError="True"
                                                            ValidationGroup="Submit" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvAccountType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountType_SelectedIndexChanged">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="C">Customer</asp:ListItem>
                                                            <asp:ListItem Value="E">Entity</asp:ListItem>
                                                            <%--<asp:ListItem Value="G">General</asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvAccountType" runat="server" ControlToValidate="ddlSInvAccountType"
                                                            Display="None" ErrorMessage="Select the Account Type" SetFocusOnError="True"
                                                            InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account Code">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                                    <AlternatingItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvAccountCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountCode_SelectedIndexChanged"
                                                            Width="200px">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvAccountCode" runat="server" ControlToValidate="ddlSInvAccountCode"
                                                            Display="None" ErrorMessage="Select the Account Code" SetFocusOnError="True"
                                                            ValidationGroup="Submit" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvAccountCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountCode_SelectedIndexChanged"
                                                            Width="200px">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvAccountCode" runat="server" ControlToValidate="ddlSInvAccountCode"
                                                            Display="None" ErrorMessage="Select the Account Code" SetFocusOnError="True"
                                                            ValidationGroup="Submit" InitialValue="0"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Debit GL Code">
                                                    <ItemStyle Wrap="true" Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSInvDebitGLCode" runat="server" Text='<%# Bind("DBT_GL_CODE") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <asp:Label ID="LblSInvDebitGLCode" runat="server" Text='<%# Bind("DBT_GL_CODE") %>'>
                                                        </asp:Label>
                                                    </AlternatingItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Debit SL Code">
                                                    <ItemStyle Wrap="true" Width="10%" />
                                                    <AlternatingItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvDebitSLCode" runat="server">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvDebitSLCode" runat="server" ControlToValidate="ddlSInvDebitSLCode"
                                                            Display="None" ErrorMessage="Select the Debit SL Code" SetFocusOnError="True"
                                                            InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvDebitSLCode" runat="server">
                                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvDebitSLCode" runat="server" ControlToValidate="ddlSInvDebitSLCode"
                                                            Display="None" ErrorMessage="Select the Debit SL Code" SetFocusOnError="True"
                                                            InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemStyle Wrap="true" />
                                                    <AlternatingItemTemplate>
                                                        <asp:Label ID="LblSInvGlDesc" runat="server" Text='<%# Bind("DBT_GL_DESC") %>'> </asp:Label>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSInvGlDesc" runat="server" Text='<%# Bind("DBT_GL_DESC") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Credit GL Code">
                                                    <ItemStyle Wrap="true" Width="10%" />
                                                    <AlternatingItemTemplate>
                                                        <asp:Label ID="LblSInvCrdGLCode" runat="server" Text='<%# Bind("CRDT_GL_CODE") %>'> </asp:Label>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblSInvCrdGLCode" runat="server" Text='<%# Bind("CRDT_GL_CODE") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Credit SL Code">
                                                    <ItemStyle Wrap="true" Width="10%" />
                                                    <AlternatingItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvCrdSLCode" runat="server">
                                                            <asp:ListItem Value="">--Select--</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvCrdSLCode" runat="server" ControlToValidate="ddlSInvCrdSLCode"
                                                            Display="None" ErrorMessage="Select the Credit SL Code" SetFocusOnError="True"
                                                            InitialValue="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    </AlternatingItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="LblSInvTotal" runat="server" Text="Grand Total" />
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlSInvCrdSLCode" runat="server">
                                                            <asp:ListItem Value="">--Select--</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvddlSInvCrdSLCode" runat="server" ControlToValidate="ddlSInvCrdSLCode"
                                                            Display="None" ErrorMessage="Select the Credit SL Code" SetFocusOnError="True"
                                                            InitialValue="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <AlternatingItemTemplate>
                                                        <asp:Label ID="LblCrdDescription" runat="server" Text='<%# Bind("CRDT_GL_DESC") %>'></asp:Label>
                                                    </AlternatingItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblCrdDescription" runat="server" Text='<%# Bind("CRDT_GL_DESC") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="30%" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtSInvAmount" runat="server" Text='<%# Bind("AMNT") %>' Style="text-align: right"
                                                            Width="96%" ReadOnly="true">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="fTxtSInvAmount" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="." TargetControlID="TxtSInvAmount">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvTxtSInvAmount" runat="server" ControlToValidate="TxtSInvAmount"
                                                            Display="None" ErrorMessage="Enter the Invoice Amount" SetFocusOnError="True"
                                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <asp:TextBox ID="TxtSInvAmount" runat="server" Text='<%# Bind("AMNT") %>' Style="text-align: right"
                                                            Width="96%">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FTxtSInvAmount" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="." TargetControlID="TxtSInvAmount">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvTxtSInvAmount" runat="server" ControlToValidate="TxtSInvAmount"
                                                            Display="None" ErrorMessage="Enter the Invoice Amount" SetFocusOnError="True"
                                                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    </AlternatingItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="TxtSInvTotal" runat="server" ContentEditable="false" Text='<%# Bind("AMNT") %>'
                                                            Style="text-align: right" Width="96%" />
                                                        <cc1:FilteredTextBoxExtender ID="FTxtSInvTotal" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="." TargetControlID="TxtSInvTotal">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="30%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" colspan="4">
                        <asp:Panel ID="PnlReceiptDetails" runat="server" CssClass="stylePanel" GroupingText="Receipt Details">
                            <table cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:GridView ID="GrvReceiptDetails" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                            OnRowDataBound="GrvReceiptDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Receipt Reference Number">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtRcptRefNo" runat="server" Width="100%" Text='<%# Bind("RECEIPT_NO") %>'
                                                            ReadOnly="true">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <asp:TextBox ID="TxtRcptRefNo" runat="server" Width="100%" Text='<%# Bind("RECEIPT_NO") %>'
                                                            ReadOnly="true">
                                                        </asp:TextBox>
                                                    </AlternatingItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Transaction Date">
                                                    <ItemStyle Wrap="true" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="LblGrndTotal" runat="server" Text="Grand Total"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtSInvTxnDate" runat="server" Text='<%# Bind("TRXNDT") %>' ReadOnly="true">
                                                        </asp:TextBox>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <asp:TextBox ID="TxtSInvTxnDate" runat="server" Text='<%# Bind("TRXNDT") %>' ReadOnly="true">
                                                        </asp:TextBox>
                                                    </AlternatingItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Transaction Amount">
                                                    <ItemStyle Wrap="true" />
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="TxtSInvTxnTotal" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="fTxtSInvTxnTotal" runat="server" FilterType="Custom,Numbers"
                                                            TargetControlID="TxtSInvTxnTotal" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="TxtSInvTxnAmnt" runat="server" Text='<%# Bind("TRXNAMNT") %>' Width="100%"
                                                            ReadOnly="true">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="fTxtSInvTxnAmnt" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="." TargetControlID="TxtSInvTxnAmnt">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <asp:TextBox ID="TxtSInvTxnAmnt" runat="server" Text='<%# Bind("TRXNAMNT") %>' Width="100%"
                                                            ReadOnly="true">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="fTxtSInvTxnAmnt" runat="server" FilterType="Custom,Numbers"
                                                            ValidChars="." TargetControlID="TxtSInvTxnAmnt">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </AlternatingItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblRcptID" runat="server" Text='<%# Bind("Receipt_ID") %>' Width="100%">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <asp:Label ID="LblRcptID" runat="server" Text='<%# Bind("Receipt_ID") %>' Width="100%">
                                                        </asp:Label>
                                                    </AlternatingItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkRcpt" runat="server" AutoPostBack="true" OnCheckedChanged="ChkRcpt_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <asp:CheckBox ID="ChkRcpt" runat="server" OnCheckedChanged="ChkRcpt_CheckedChanged"
                                                            AutoPostBack="true" />
                                                    </AlternatingItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <br />
                                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                            OnClientClick="return fnCheckPageValidators('Submit');" Text="Save" ValidationGroup="Submit" />
                                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                                            Text="Cancel" OnClick="btnCancel_Click" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%">
                                        <asp:ValidationSummary ID="vsSaleInvEntry" runat="server" CssClass="styleMandatoryLabel"
                                            HeaderText="Please correct the following validation(s):" ValidationGroup="Add2" />
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                                            HeaderText="Please correct the following validation(s):" ValidationGroup="Submit" />
                                        <asp:ValidationSummary ID="vsSaleInvEntry_Inv" runat="server" CssClass="styleMandatoryLabel"
                                            HeaderText="Please correct the following validation(s):" ValidationGroup="Add" />
                                        <asp:CustomValidator ID="cvSaleInventry" runat="server" CssClass="styleMandatoryLabel"
                                            Display="None" HeaderText="Please correct the following validation(s): " />
                                        <asp:CustomValidator ID="cvInvDtls_Add" runat="server" CssClass="styleMandatoryLabel"
                                            Display="None" HeaderText="Please correct the following validation(s): " />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
