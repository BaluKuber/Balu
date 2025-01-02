<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGProforma_Add.aspx.cs" Inherits="S3GORGProforma_Add" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                    <td valign="top">
                        <cc1:TabContainer ID="tcProforma" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            Width="99%" ScrollBars="Auto">
                            <cc1:TabPanel runat="server" HeaderText="Proforma Details" ID="tbProforma" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Proforma Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table cellpadding="0" width="100%" cellspacing="0">
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlLOB" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                            Width="200px" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Line of Business"
                                                            InitialValue="0"></asp:RequiredFieldValidator>
                                                        <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                            OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select the Location"
                                                            IsMandatory="true" />
                                                        <%-- <asp:DropDownList OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ID="ddlBranch"
                                                            Width="200px" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>--%>
                                                        <%--<asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Location"
                                                            InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblRefDocType" runat="server" CssClass="styleReqFieldLabel" Text="Reference Doc Type"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlRefDocType" Width="150px" OnSelectedIndexChanged="ddlRefDocType_SelectedIndexChanged"
                                                            AutoPostBack="True" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblRefDocNo" runat="server" CssClass="styleReqFieldLabel" Text="Reference Doc Number"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <uc2:Suggest ID="ddlRefDocNo" runat="server" ServiceMethod="GetDocumentNumber" AutoPostBack="true"
                                                            OnItem_Selected="ddlRefDocNo_SelectedIndexChanged" IsMandatory="true" ErrorMessage="Select Reference Doc Number" />
                                                        <%-- <asp:DropDownList ID="ddlRefDocNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRefDocNo_SelectedIndexChanged"
                                                            Width="200px">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvRefDocType" runat="server" ControlToValidate="ddlRefDocType"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Reference Doc Type"
                                                            InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="Label7" runat="server" Text="Sub A/c Number"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
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
                                                                    <asp:Panel GroupingText="Vendor Address" ID="Panel2" runat="server" CssClass="stylePanel">
                                                                        <table width="100%" align="center" border="0" cellspacing="0">
                                                                            <tr>
                                                                                <td class="styleFieldLabel" width="33%">
                                                                                    <asp:Label ID="Label5" runat="server" CssClass="styleReqFieldLabel" Text="Vendor Code"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldLabel" width="67%">
                                                                                    <%--<asp:DropDownList ID="ddlVendorCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlVendorCode_SelectedIndexChanged">
                                                                                    </asp:DropDownList>--%>
                                                                                    <uc2:Suggest ID="ddlVendorCode" runat="server" ServiceMethod="GetVendors" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlVendorCode_SelectedIndexChanged" ErrorMessage="Select the Vendor Code"
                                                                                        IsMandatory="true" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="Label10" runat="server" Text="Vendor Name"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtVendorName" runat="server" Width="95%" ReadOnly="True"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <uc1:S3GCustomerAddress ID="S3GVendorAddress" runat="server" ShowCustomerCode="false"
                                                                                        ShowCustomerName="false" FirstColumnStyle="styleFieldLabel" FirstColumnWidth="35%" />
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
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel GroupingText="Proforma Details" ID="Panel1" runat="server" CssClass="stylePanel">
                                                            <table width="100%" align="center" border="0" cellspacing="0">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="18%">
                                                                        <asp:Label ID="lblProformaNo" runat="server" CssClass="styleReqFieldLabel" Text="Proforma Number"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="33%">
                                                                        <asp:TextBox ID="txtProformaNo" MaxLength="20" Width="200px" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel" style="padding-right: 8px">
                                                                        <asp:Label ID="lblProformaDate" runat="server" CssClass="styleReqFieldLabel" Text="Proforma Date"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtProformaDate" runat="server" Width="40%"></asp:TextBox>
                                                                        <asp:Image ID="imgProformaDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                        <asp:RequiredFieldValidator ID="rfvProformaNo" runat="server" ControlToValidate="txtProformaNo"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Proforma Number"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvProformaDate" runat="server" ControlToValidate="txtProformaDate"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Proforma Date"></asp:RequiredFieldValidator>
                                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgProformaDate"
                                                                            TargetControlID="txtProformaDate">
                                                                        </cc1:CalendarExtender>
                                                                        <%--<asp:RequiredFieldValidator ID="rfvVendorCode" runat="server" ControlToValidate="ddlVendorCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Vendor Code"
                                                                            InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblTaxRegNo" runat="server" Text="Tax Reg Number"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtTaxRegNo" MaxLength="20" Width="130px" ReadOnly="true" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblVATNo" runat="server" Text="VAT Number"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtVATNo" MaxLength="20" Width="150px" runat="server"></asp:TextBox>
                                                                        <asp:CustomValidator ID="rfvVAT" runat="server" CssClass="styleMandatoryLabel" Display="None"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label1" runat="server" CssClass="styleReqFieldLabel" Text="Proforma Image"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:RadioButton ID="rdoYes" onclick="fnDisableControls(1);" AutoPostBack="true"
                                                                            OnCheckedChanged="File_CheckedChanged" GroupName="DocYes" runat="server" Text="Yes" />
                                                                        <asp:RadioButton ID="rdoNo" onclick="fnDisableControls(0);" AutoPostBack="true" OnCheckedChanged="File_CheckedChanged"
                                                                            GroupName="DocYes" runat="server" Text="No" />
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblScanImage" runat="server" CssClass="styleReqFieldLabel" Text="Scan Image"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <cc1:AsyncFileUpload ID="fileScanImage" runat="server" Width="200px" OnClientUploadComplete="uploadComplete" />
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
                                                                        <asp:CheckBox runat="server" ID="chkBookInAsset" Visible="false" Enabled="false"
                                                                            TextAlign="Left" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-top: 10px;" colspan="4">
                                                        <asp:GridView runat="server" ID="grvBookInAsset" Width="100%" OnRowDataBound="grvBookInAsset_RowDataBound"
                                                            AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Asset_Code" ItemStyle-HorizontalAlign="Left" HeaderText="Asset Code" />
                                                                <asp:BoundField DataField="Asset_Description" ItemStyle-HorizontalAlign="Left" HeaderText="Asset Description" />
                                                                <asp:TemplateField HeaderText="Asset Value">
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
                                                        <asp:GridView runat="server" ShowFooter="true" ID="grvAssetDetails" Width="95%" OnRowDataBound="grvAssetDetails_RowDataBound"
                                                            RowStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Asset_Description" HeaderText="Asset Description" />
                                                                <asp:TemplateField HeaderText="Unit Value">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox CssClass="styleTxtRightAlign" ID="txtUnitValue" Text='<%# Bind("Unit_Value")%>'
                                                                            MaxLength="14" Width="104px" runat="server"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtUnitValue"
                                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="No of Units">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtNoofUnit" CssClass="styleTxtRightAlign" Text='<%# Bind("No_Of_Units")%>'
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
                                                                        <asp:TextBox ID="txtVAT" MaxLength="5" CssClass="styleTxtRightAlign" Text='<%# Bind("Vat")%>'
                                                                            Width="33px" runat="server"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txtVAT"
                                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Others">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtOthers" CssClass="styleTxtRightAlign" Text='<%# Bind("Others")%>'
                                                                            MaxLength="13" Width="100px" runat="server"></asp:TextBox>
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
                                                                <asp:TemplateField HeaderText="Net Amount">
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
                            <cc1:TabPanel runat="server" HeaderText="Warranty Details" ID="TabPanel2" CssClass="tabpan"
                                BackColor="Red">
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
                                                                <asp:TemplateField HeaderText="S.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblSNO" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Asset_Description" HeaderText="Asset Description" />
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
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:CustomValidator ID="rfvUnitValue" runat="server" CssClass="styleMandatoryLabel"
                            Display="None" ErrorMessage="Enter the Unit Value in Asset Details"></asp:CustomValidator>
                        <asp:CustomValidator ID="rfvNoofUnit" runat="server" ErrorMessage="Enter the Number of Units in Asset Details"
                            CssClass="styleMandatoryLabel" Display="None"></asp:CustomValidator>
                        <asp:CustomValidator ID="rfvTax" runat="server" CssClass="styleMandatoryLabel" Display="None"
                            ErrorMessage="Either Tax % or VAT % should be entered in Asset Details"></asp:CustomValidator>
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
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <%--<asp:CustomValidator ID="custTaxVat" runat="server" 
                         OnServerValidate="checkTaxVatValidation" Display="None"></asp:CustomValidator>--%>
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
                            Height="250px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                        <input type="hidden" runat="server" value="0" id="hdnProformaID" />
                        <input type="hidden" runat="server" value="0" id="hdnSLA" />
                        <input type="hidden" runat="server" id="hdnCustomerID" />
                        <input type="hidden" runat="server" id="hdnFileUploaded" />
                        <input type="hidden" runat="server" id="hdnFileName" />
                        <input type="hidden" runat="server" id="hdnFile" />
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        function fnCalculateTax(dUnitval, iNoofUnit, dTaxPer, dVATPer, dOth, dVATVal, dTaxVal, dTotValue, dNetAmt) {
            //debugger;   
            strUnitval = document.getElementById(dUnitval).value;
            strNoofUnit = document.getElementById(iNoofUnit).value;
            strTaxPer = document.getElementById(dTaxPer).value;
            strVATPer = document.getElementById(dVATPer).value;
            strOth = document.getElementById(dOth).value;

            if (strUnitval != "" && strNoofUnit != "") {
                document.getElementById(dTotValue).value = (parseFloat(strUnitval) * parseFloat(strNoofUnit)).toFixed(2);
            }
            else {
                //document.getElementById(dTotValue).value="";
                return;

            }

            if (strTaxPer != "") {
                document.getElementById(dTaxVal).value = (parseFloat(document.getElementById(dTotValue).value / 100) * parseFloat(strTaxPer)).toFixed(2);
            }
            if (strVATPer != "") {
                document.getElementById(dVATVal).value = (parseFloat(document.getElementById(dTotValue).value / 100) * parseFloat(strVATPer)).toFixed(2);
            }

            if (strVATPer != "" && strVATPer != 0 && strOth == "") {
                document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(document.getElementById(dVATVal).value)).toFixed(2);
            }
            if (strTaxPer != "" && strVATPer == "" && strOth == "") {
                document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(document.getElementById(dTaxVal).value)).toFixed(2);
            }

            if (strOth != "" && strVATPer == "" && strTaxPer == "") {
                document.getElementById(dNetAmt).value = (parseFloat(document.getElementById(dTotValue).value) + parseFloat(strOth)).toFixed(2);
            }

            if (strOth != "" && strVATPer != "" && strVATPer != 0) {
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


        function fnDisableControls(sScanImage) {
            if (sScanImage == '0') {
                document.getElementById('<%=fileScanImage.ClientID%>').disabled = true;

            }
            else {
                document.getElementById('<%=fileScanImage.ClientID%>').disabled = false;

            }
        }

        function fnLogin() {
            window.event.keyCode = 0;
            document.getElementById('<%=txtVATNo.ClientID%>').value = "";
            return false;

        }

        function uploadComplete(sender, args) {
            //debugger;
            document.getElementById('<%=hdnFileName.ClientID%>').value = args._fileName;
            document.getElementById('<%=lblDisplayFile.ClientID%>').innerText = args._fileName;
            //addToClientTable(args.get_fileName(), text);

            fnSetFormTarget();
        }

    </script>

</asp:Content>
