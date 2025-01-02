<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdInvoiceVendor_Add.aspx.cs" Inherits="S3GLoanAdInvoiceVendor_Add" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
    <table width="100%" align="center" cellpadding="0" cellspacing="0" scrolling="no">
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
            <td valign="top">
                <cc1:TabContainer ID="tcInvoice" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                    Width="99%" ScrollBars="Auto">
                    <cc1:TabPanel runat="server" ID="tbInvoice" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Invoicing
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 18%">
                                                <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" style="width: 31%">
                                                <asp:DropDownList ID="ddlLOB" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                    Width="200px" runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                    CssClass="styleMandatoryLabel" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" style="width: 17%">
                                                <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" style="width: 33%">
                                               <%-- <asp:DropDownList OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ID="ddlBranch"
                                                    runat="server" AutoPostBack="True">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                 CssClass="styleMandatoryLabel" Display="None" InitialValue="0"></asp:RequiredFieldValidator> --%>  
                                                <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                                                    AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select a Location"
                                                    IsMandatory="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblRefDocType" runat="server" CssClass="styleReqFieldLabel" Text="Reference Doc Type"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlRefDocType" OnSelectedIndexChanged="ddlRefDocType_SelectedIndexChanged"
                                                    AutoPostBack="True" Width="200px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblRefDocNo" runat="server" CssClass="styleReqFieldLabel" Text="Reference Doc Number"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <%--<asp:DropDownList ID="ddlRefDocNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRefDocNo_SelectedIndexChanged"
                                                    Width="200px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvRefDocType" runat="server" ControlToValidate="ddlRefDocType"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Reference Doc Type"
                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvRefDocNo" runat="server" ControlToValidate="ddlRefDocNo"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Reference Doc No"
                                                    InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                <uc2:Suggest ID="ddlRefDocNo" runat="server" ServiceMethod="GetDocumentNumber" AutoPostBack="true"
                                                    OnItem_Selected="ddlRefDocNo_SelectedIndexChanged" IsMandatory="true" ErrorMessage="Select Reference Doc Number" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="Label7" runat="server" Text="Sub A/c Number"></asp:Label>
                                            </td>
                                            <td colspan="3" class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlSLA" Enabled="false" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged"
                                                    AutoPostBack="True" Width="200px" runat="server">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSLA" Enabled="false" runat="server" ControlToValidate="ddlSLA"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Sub A/c Number"
                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="50%">
                                                            <asp:Panel GroupingText="Customer Communication Address" ID="pnlCommAddress" runat="server"
                                                                CssClass="stylePanel">
                                                                <table width="100%" align="center" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                FirstColumnWidth="39%" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="50%">
                                                            <asp:Panel GroupingText="Vendor Address" ID="pnlPermenantAddress" runat="server"
                                                                CssClass="stylePanel">
                                                                <table align="center" width="100%" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" width="33%">
                                                                            <asp:Label ID="lblVendorCode" runat="server" CssClass="styleReqFieldLabel" Text="Vendor"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <cc1:ComboBox ID="ddlVendorCode" AutoPostBack="true" runat="server" AutoCompleteMode="SuggestAppend"
                                                                                DropDownStyle="DropDownList" MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ddlVendorCode_SelectedIndexChanged">
                                                                            </cc1:ComboBox>
                                                                            <input type="hidden" runat="server" id="hdnCustomerID" />
                                                                            <asp:RequiredFieldValidator ID="rfvVendorCode" runat="server" ControlToValidate="ddlVendorCode"
                                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblVendorName" Visible="false" runat="server" Text="Vendor Name"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtVendorName" Visible="false" runat="server" ReadOnly="true"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <uc1:S3GCustomerAddress ID="S3GVendorAddress" runat="server" ShowCustomerCode="false"
                                                                                Caption="Vendor" FirstColumnStyle="styleFieldLabel" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel GroupingText="Invoice Details" ID="Panel1" runat="server" CssClass="stylePanel">
                                                    <table width="100%" align="center" border="0" cellspacing="0">
                                                        <tr>
                                                            <td width="18%" class="styleFieldLabel">
                                                                <asp:Label ID="lblInvoiceNo" runat="server" CssClass="styleReqFieldLabel" Text="Invoice Number"></asp:Label>
                                                            </td>
                                                            <td width="31%" class="styleFieldAlign">
                                                                <asp:TextBox ID="txtInvoiceNo" MaxLength="20" runat="server" onblur="fnCheckZero()"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender24" runat="server" TargetControlID="txtInvoiceNo"
                                                                    FilterType="Custom,Numbers, UppercaseLetters, LowercaseLetters" ValidChars="/\-_"
                                                                    Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                            <td width="17%" class="styleFieldLabel">
                                                                <asp:Label ID="lblInvoiceDate" runat="server" CssClass="styleReqFieldLabel" Text="Invoice Date"></asp:Label>
                                                            </td>
                                                            <td width="33%" class="styleFieldAlign">
                                                                <asp:TextBox ID="txtInvoiceDate" runat="server"></asp:TextBox>
                                                                <asp:Image ID="imgInvoiceDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ID="rfvInvoiceNo" runat="server" ControlToValidate="txtInvoiceNo"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Invoice Number"></asp:RequiredFieldValidator>
                                                                <asp:RequiredFieldValidator ID="rfvInvoiceDate" runat="server" ControlToValidate="txtInvoiceDate"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Invoice Date"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                    OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgInvoiceDate"
                                                                    TargetControlID="txtInvoiceDate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblTaxRegNo" runat="server" Text="Tax Reg Number"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtTaxRegNo" MaxLength="20" ReadOnly="true" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblVATNo" runat="server" Text="VAT Number"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtVATNo" MaxLength="20" Width="150px" runat="server" ReadOnly="true"></asp:TextBox>
                                                                <%--<cc1:FilteredTextBoxExtender
                                                                          ID="FilteredTextBoxExtender25" runat="server" TargetControlID="txtVATNo" 
                                                                           FilterType="Custom,Numbers ,UppercaseLetters, LowercaseLetters"
                                                                               ValidChars=" " Enabled="True"></cc1:FilteredTextBoxExtender>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td runat="server" class="styleFieldLabel">
                                                                <asp:Label ID="lblInvoiceType" runat="server" CssClass="styleReqFieldLabel" Text="Invoice Type"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlInvoiceType" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceType_OnSelectedIndexChanged"
                                                                    runat="server">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlInvoiceType"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Invoice Type"
                                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td runat="server" id="tdBookIn" class="styleFieldLabel">
                                                                Book In Asset
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:CheckBox runat="server" ID="chkBookInAsset" Enabled="False" TextAlign="Left" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label1" runat="server" CssClass="styleReqFieldLabel" Text="Invoice Image"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:RadioButton ID="rdoYes" onclick="fnDisableControls(1);" GroupName="DocYes" runat="server"
                                                                    Text="Yes" />
                                                                <asp:RadioButton ID="rdoNo" onclick="fnDisableControls(0);" GroupName="DocYes" runat="server"
                                                                    Text="No" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblScanImage" runat="server" CssClass="styleReqFieldLabel" Text="Scan Image"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <cc1:AsyncFileUpload ID="fileScanImage" OnClientUploadComplete="uploadComplete" runat="server"
                                                                    Width="200px" FailedValidation="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                Download File
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:LinkButton runat="server" Enabled="False" Text="Download" CausesValidation="False"
                                                                    OnClick="lnkDownload_Click" ID="lnkDownload"></asp:LinkButton>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblDisplayFile" CssClass="styleAttachedFile" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top: 10px;" colspan="4">
                                                <asp:GridView runat="server" OnRowDataBound="grvBookInAsset_RowDataBound" ID="grvBookInAsset"
                                                    Width="100%" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No.">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Asset_Code" HeaderText="Asset Code">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LAN_Asset_Description" HeaderText="Asset Description">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Asset Value" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Bind("NetAmount")%>' ID="lblNetAmt"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="Asset Details" ID="TabPanel1" CssClass="tabpan"
                        BackColor="Red">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <div style="height: 300px; overflow-x: auto;">
                                                    <asp:GridView runat="server" ShowFooter="false" ID="grvAssetDetails" Width="99%"
                                                        OnRowDataBound="grvAssetDetails_RowDataBound" RowStyle-HorizontalAlign="Center"
                                                        FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sl.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Asset_Description" HeaderText="Asset Description" ItemStyle-HorizontalAlign="Left" />
                                                            <asp:TemplateField HeaderText="Unit Value">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtUnitValue" CssClass="styleTxtRightAlign" Text='<%# Bind("Unit_Value")%>'
                                                                        MaxLength="14" Width="104px" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtUnitValue"
                                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="No. of Units">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtNoofUnit" CssClass="styleTxtRightAlign" Text='<%# Bind("Numberofunits")%>'
                                                                        MaxLength="3" Width="40px" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtNoofUnit"
                                                                        FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Value">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtTotValue" CssClass="styleTxtRightAlign" Enabled="false" Text='<%# Bind("TotalValue")%>'
                                                                        Width="115px" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tax %">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtTax" CssClass="styleTxtRightAlign" Text='<%# Bind("Tax")%>' Width="33px"
                                                                        MaxLength="5" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtTax"
                                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VAT %">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtVAT" CssClass="styleTxtRightAlign" MaxLength="5" Text='<%# Bind("Vat")%>'
                                                                        Width="33px" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txtVAT"
                                                                        FilterType="Numbers,Custom" ValidChars=".">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:Label runat="server" ID="lblVATPer" Text='<%# Bind("VATPer")%>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Others">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtOthers" CssClass="styleTxtRightAlign" Text='<%# Bind("Others")%>'
                                                                        MaxLength="14" Width="90px" runat="server"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtOthers"
                                                                        FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tax Value">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtTaxVal" CssClass="styleTxtRightAlign" Text='<%# Bind("TaxValue")%>'
                                                                        Enabled="false" Width="110px" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="VAT Value">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtVATVal" CssClass="styleTxtRightAlign" Enabled="false" Text='<%# Bind("VATValue")%>'
                                                                        Width="110px" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Gross Amount">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtNetValue" CssClass="styleTxtRightAlign" Enabled="false" Text='<%# Bind("NetAmount")%>'
                                                                        Width="120px" runat="server"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Asset_ID">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("Asset_ID")%>' runat="server" ID="lblAssetID"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false" HeaderText="Lease_Asset_No">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("Lease_Asset_No")%>' runat="server" ID="lblLANNo"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <RowStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trTotal">
                                            <td style="text-align: right; padding-left: 224px; vertical-align: top">
                                                <asp:Label runat="server" Text="Total Gross Amount" BackColor="White" ID="lblTotAmount"
                                                    Font-Bold="true" CssClass="styleGridHeader">
                                                </asp:Label>
                                                <asp:TextBox ID="txtTotalAmt" Width="80px" CssClass="styleTxtRightAlign" ReadOnly="true"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel2" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Warranty Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:GridView runat="server" ShowFooter="True" ID="grvWarranty" Width="100%" OnRowDataBound="grvWarranty_RowDataBound"
                                                    AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No.">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Asset_Description" HeaderText="Asset Description" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderText="Warranty Type">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlWarrantyType" runat="server">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Free Warranty" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Paid Warranty" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFromDate" Width="80px" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDate"
                                                                    PopupButtonID="txtFromDate" ID="ceFromDate" Enabled="True">
                                                                </cc1:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtToDate" Width="80px" runat="server"></asp:TextBox>
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDate"
                                                                    PopupButtonID="txtToDate" ID="ceToDate" Enabled="True">
                                                                </cc1:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Service Frequency">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlServiceFrequency" runat="server">
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Text='<%# Bind("Remarks")%>' MaxLength="100"
                                                                    onkeyup="maxlengthfortxt(100);" Width="180px" runat="server"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="Asset_ID">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Bind("Asset_ID")%>' runat="server" ID="lblAssetID"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false" HeaderText="Lease_Asset_No">
                                                            <ItemTemplate>
                                                                <asp:Label Text='<%# Bind("Lease_Asset_No")%>' runat="server" ID="lblLANNo"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr class="styleButtonArea" align="center" style="padding-left: 4px">
            <td>
                <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                    CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                    CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr class="styleButtonArea">
            <td>
                <asp:CustomValidator ID="rfvFile" runat="server" CssClass="styleMandatoryLabel" Display="None"
                    ErrorMessage="Browse a File to upload"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvRow" runat="server" CssClass="styleMandatoryLabel" Display="None"
                    ErrorMessage="Enter atleast one row in Asset Details"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvUnitValue" runat="server" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                    Display="None" ErrorMessage="Enter the Unit Value in Asset Details"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvNoofUnit" runat="server" ErrorMessage="Enter the Number of Units in Asset Details"
                    CssClass="styleMandatoryLabel" Display="None"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvTax" runat="server" CssClass="styleMandatoryLabel" Display="None"
                    ErrorMessage="Either Tax % or VAT % should be entered in Asset Details"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvVAT" runat="server" CssClass="styleMandatoryLabel" Display="None"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvWarrantyType" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" ErrorMessage="Select the Warranty Type in Warranty Details"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvFromDate" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" ErrorMessage="Enter the From Date in Warranty Details"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvToDate" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" ErrorMessage="Enter the To Date in Warranty Details"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvServiceFrequency" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" ErrorMessage="Select the Service Frequency in Warranty Details"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvCompareDate" runat="server" CssClass="styleMandatoryLabel"
                    Display="None"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvAssetWarrantyCompare" runat="server" ErrorMessage="Asset details should be entered if warranty details exists for asset"
                    CssClass="styleMandatoryLabel" Display="None"></asp:CustomValidator>
                <asp:CustomValidator ID="rfvCompareWarrantyDate" runat="server" CssClass="styleMandatoryLabel"
                    Display="None"></asp:CustomValidator>
                <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
                    Height="250px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                    ShowSummary="true" />
                <input type="hidden" runat="server" id="hdnFile" />
                <input type="hidden" runat="server" value="0" id="hdnInvoiceID" />
                <input type="hidden" runat="server" id="hdnFileUploaded" />
                <input type="hidden" runat="server" value="0" id="hdnSLA" />
                <%--<input type="hidden" runat="server" id="hdnCustomerID" />--%>
                <input type="hidden" runat="server" id="hdnFileName" />
                <input type="hidden" runat="server" value="1" id="hdnRadioFile" />
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
            </td>
        </tr>
    </table>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>

    <script language="javascript" type="text/javascript">

        function fnCalculateTax(thisVal, dUnitval, iNoofUnit, dTaxPer, dVATPer, dOth, dVATVal, dTaxVal, dTotValue, dNetAmt) {
            //debugger;   

            strUnitval = document.getElementById(dUnitval).value;
            strNoofUnit = document.getElementById(iNoofUnit).value;
            strTaxPer = document.getElementById(dTaxPer).value;
            strVATPer = document.getElementById(dVATPer).value;
            strOth = document.getElementById(dOth).value;
            var InvType = document.getElementById('<%=ddlInvoiceType.ClientID%>');

            if (strUnitval != "" && strNoofUnit != "") {
                document.getElementById(dTotValue).value = (parseFloat(strUnitval) * parseFloat(strNoofUnit)).toFixed(2);
            }
            else {
                //document.getElementById(dTotValue).value="";
                return;

            }

            if (strTaxPer != "") {
                document.getElementById(dTaxVal).value = (parseFloat(document.getElementById(dTotValue).value / 100) * parseFloat(strTaxPer)).toFixed(2);
                strVATPer.disabled = true;
            }
            else {
                strVATPer.disabled = false;
            }
            if (strVATPer != "") {
                document.getElementById(dVATVal).value = (parseFloat(document.getElementById(dTotValue).value / 100) * parseFloat(strVATPer)).toFixed(2);
                strTaxPer.disabled = true;
                if (InvType.options[InvType.selectedIndex].value == "2") {
                    if ((thisVal.id.indexOf('_txtOthers') == -1) || (document.getElementById(dOth).value == '')) {
                        document.getElementById(dOth).value = document.getElementById(dVATVal).value;
                    }
                }

            }
            else {
                strTaxPer.disabled = false;
            }

            if (strVATPer != "" && strVATPer != 0 && strOth == "") {
                document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(document.getElementById(dVATVal).value)).toFixed(2);
            }
            if (strTaxPer != "" && strVATPer == "" && strOth == "") {
                document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(document.getElementById(dTaxVal).value)).toFixed(2);
            }

            if (strOth != "" && strVATPer == "" && strTaxPer == "") {
                if (InvType.options[InvType.selectedIndex].value == "2")
                    document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value)).toFixed(2);
                else
                    document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(strOth)).toFixed(2);
            }

            if (strOth != "" && strVATPer != "" && strVATPer != 0) {
                if (InvType.options[InvType.selectedIndex].value == "2")
                    document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(document.getElementById(dVATVal).value)).toFixed(2);
                else
                    document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(strOth) + parseFloat(document.getElementById(dVATVal).value)).toFixed(2);
            }

            if (strOth != "" && strVATPer == "" && strTaxPer != "") {
                document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(strOth) + parseFloat(document.getElementById(dTaxVal).value)).toFixed(2);
            }

            if (strOth == "" && strVATPer == "" && strTaxPer == "") {
                document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value)).toFixed(2);
            }

            if (strTaxPer == "") {
                document.getElementById(dTaxVal).value = "";
            }
            if (strVATPer == "") {
                document.getElementById(dVATVal).value = "";
            }

            //document.getElementById(dTotValue).value=strUnitval*(strTaxPer/100);

        }

        ////    function fnCalculateTax(dUnitval,iNoofUnit,dTaxPer,dVATPer,dOth,dVATVal,dTaxVal,dTotValue,dNetAmt)
        ////    {
        ////     //debugger;   
        ////     strUnitval=document.getElementById(dUnitval).value;
        ////     strNoofUnit=document.getElementById(iNoofUnit).value;
        ////     strTaxPer=document.getElementById(dTaxPer).value;
        ////     strVATPer=document.getElementById(dVATPer).value;
        ////     strOth=document.getElementById(dOth).value;
        ////     
        ////    if(strUnitval!="" && strNoofUnit!="")
        ////    {
        ////    document.getElementById(dTotValue).value=(parseFloat(strUnitval)*parseFloat(strNoofUnit)).toFixed(2);
        ////     }
        ////     else 
        ////     {
        ////        //document.getElementById(dTotValue).value="";
        ////        return;
        ////        
        ////        }
        ////     
        ////    if(strTaxPer!="")
        ////    {
        ////    document.getElementById(dTaxVal).value=(parseFloat(document.getElementById(dTotValue).value/100)*parseFloat(strTaxPer)).toFixed(2);
        ////    }
        ////    if(strVATPer!="")
        ////    {
        ////     document.getElementById(dVATVal).value=(parseFloat(document.getElementById(dTotValue).value/100)*parseFloat(strVATPer)).toFixed(2);
        ////     }
        ////      if(strTaxPer!="" && strVATPer!="" && strOth!="")
        ////        {
        ////           document.getElementById(dNetAmt).value=(parseFloat(document.getElementById(dTotValue).value)+parseFloat(strOth)+parseFloat(document.getElementById(dVATVal).value)).toFixed(2);
        ////        }           
        ////     //document.getElementById(dTotValue).value=strUnitval*(strTaxPer/100);

        //// }

        function fnDisableControls(sScanImage) {
            if (sScanImage == '0') {
                document.getElementById('<%=fileScanImage.ClientID%>').disabled = true;
                document.getElementById('<%=hdnRadioFile.ClientID%>').value = "0";
                document.getElementById('<%=lblDisplayFile.ClientID%>').style.display = "none";
            }
            else {
                document.getElementById('<%=fileScanImage.ClientID%>').disabled = false;
                document.getElementById('<%=hdnRadioFile.ClientID%>').value = "1";
                document.getElementById('<%=lblDisplayFile.ClientID%>').style.display = "block";
            }
        }

        function fnValidateUnit(ctrl) {
            if (ctrl.value == 0) {
                alert('Unit value cannot be zero or empty');
                ctrl.value = 1;
                ctrl.focus();
                return false;
            }
        }

        function fnValidateNoofUnit(ctrl) {
            if (ctrl.value == 0) {
                alert('Number of units cannot be zero or empty');
                ctrl.value = 1;
                ctrl.focus();
                return false;
            }
        }

        function uploadComplete(sender, args) {
            document.getElementById('<%=hdnFileName.ClientID%>').value = args._fileName;
            if (document.getElementById('<%=lblDisplayFile.ClientID%>') != null)
                document.getElementById('<%=lblDisplayFile.ClientID%>').innerText = args.get_fileName();
            //addToClientTable(args.get_fileName(), text);

            fnSetFormTarget();
        }

        function fnCheckKeyPress() {
            if (window.event.keyCode == 13) {
                return false;
            }
            return true;
        }

        function fnCheckZero() {
            var txtInvoiceNo = (document.getElementById('<%=txtInvoiceNo.ClientID %>'));

            if ((txtInvoiceNo.value != " ") && !(isNaN(txtInvoiceNo.value))) {
                var InvoiceNo
                InvoiceNo = parseInt(txtInvoiceNo.value, 10);
                if (InvoiceNo == "0") {
                    alert("Invoice Number cannot be Zero");
                    txtInvoiceNo.focus();
                }
                else {
                    return;
                }
            }
            else {
                return;
            }

        }

        function window.onerror() { return true; }
 
    </script>

</asp:Content>
