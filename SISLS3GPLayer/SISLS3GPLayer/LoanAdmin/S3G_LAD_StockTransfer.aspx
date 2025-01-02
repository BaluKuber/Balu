<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_LAD_StockTransfer.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    Inherits="LoanAdmin_S3G_LAD_StockTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LOV" Src="~/UserControls/LOBMasterView.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Stock Transfer" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Input Details">
                            <table cellpadding="0" cellspacing="0" width="100%">

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label2" runat="server" CssClass="styleReqFieldLabel" Text="Transfer Type">
                                        </asp:Label>
                                    </td>

                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlTransferType" OnSelectedIndexChanged="ddlTransferType_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            <asp:ListItem Value="0" Text="--Select--" />
                                            <asp:ListItem Value="1" Text="Stock Transfer" />
                                            <asp:ListItem Value="2" Text="Delivery Challan" />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTransferType" SetFocusOnError="True"
                                            ErrorMessage="Select From Transfer Type" InitialValue="0" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text="BTN Invoice No"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoiceNo" runat="server" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" CssClass="styleReqFieldLabel" Text="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" OnItem_Selected="ddlLesseeName_Item_Selected" AutoPostBack="true" ValidationGroup="Main" ErrorMessage="Select Lessee Name" IsMandatory="true" ServiceMethod="GetLesseeNameDetails" Width="200px" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRSNO" runat="server" Text="RS Number" CssClass="styleDisplayFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlRSNO" runat="server" ServiceMethod="GetRSNODetails" OnItem_Selected="ddlRSNO_Item_Selected" AutoPostBack="true" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoicedatefrom" runat="server" Text="Invoice Date From">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoiceDateFrom" OnTextChanged="txtInvoiceDateFrom_TextChanged" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                        <asp:Image ID="imgdatefrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                            TargetControlID="txtInvoiceDateFrom"
                                            PopupButtonID="imgdatefrom" Enabled="True">
                                        </cc1:CalendarExtender>

                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoicedateTo" runat="server" Text="Invoice Date To">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoiceDateTo" OnTextChanged="txtInvoiceDateTo_TextChanged" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                        <asp:Image ID="imgdateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtInvoiceDateTo"
                                            PopupButtonID="imgdateTo" Enabled="True">
                                        </cc1:CalendarExtender>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlVendName" runat="server" OnItem_Selected="ddlVendName_Item_Selected" ServiceMethod="GetVendorName" ErrorMessage="Enter Vendor Name" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblDocumentDate" runat="server" Text="Document Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDocumentDate" runat="server" OnTextChanged="txtDocumentDate_TextChanged" Width="200px" TabIndex="5" MaxLength="10"></asp:TextBox>
                                        <asp:Image ID="imgLCdatefrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDocumentDate"
                                            PopupButtonID="imgLCdatefrom" Enabled="True">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDocumentDate" SetFocusOnError="True"
                                            ErrorMessage="Enter the Document Date" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    </td>


                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDeliveryState" runat="server" Text="Schedule State" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlComState" OnSelectedIndexChanged="ddlComState_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVddlcomstate" runat="server" ControlToValidate="ddlComState" SetFocusOnError="True"
                                            ErrorMessage="Select From Deliverystate" InitialValue="0" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblTodeliverystate" runat="server" Text="To Delivery State" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddltodeliverystate" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddltodeliverystate" SetFocusOnError="True"
                                            ErrorMessage="Select TO Delivery State" InitialValue="0" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceType" runat="server" Text="Invoice Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlInvoiceType" runat="server" OnSelectedIndexChanged="ddlInvoiceType_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="PI"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="VI"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlInvoiceNumber" runat="server"
                                            OnItem_Selected="ddlInvoiceNumber_Item_Selected" AutoPostBack="true"
                                            ServiceMethod="GetInvoiceNumberDetails" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="LblType" runat="server" Text="Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="1" Text="Inventory"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="New"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblToLessee" runat="server" CssClass="styleReqFieldLabel" Text="To Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlToLessee" runat="server" ValidationGroup="Main" ErrorMessage="Select To Lessee Name" IsMandatory="true" ServiceMethod="GetLesseeNameDetails" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShip_From" runat="server" CssClass="styleReqFieldLabel" Text="Ship From Address"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtShip_From" runat="server" Height="40px" Width="250px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShip_To" runat="server" CssClass="styleReqFieldLabel" Text="Ship To Address"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtShip_To" runat="server" Height="40px" Width="250px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShipFromState" runat="server" Text="Ship From State" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlShipFromState" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlShipFromState" SetFocusOnError="True"
                                            ErrorMessage="Select From Deliverystate" InitialValue="0" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShipToState" runat="server" Text="Ship To State" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlShipToState" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlShipToState" SetFocusOnError="True"
                                            ErrorMessage="Select Ship To State" InitialValue="0" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShipFromPin" runat="server" CssClass="styleReqFieldLabel" Text="Ship From Pin"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtShipFromPin" MaxLength="6" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtShipFromPin" SetFocusOnError="True"
                                            ErrorMessage="Enter Ship From Pin" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftxtFromPincode" runat="server" Enabled="True" FilterType="Numbers"
                                            TargetControlID="txtShipFromPin">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShipToPin" runat="server" CssClass="styleReqFieldLabel" Text="Ship To Pin"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtShipToPin" MaxLength="6" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtShipToPin" SetFocusOnError="True"
                                            ErrorMessage="Enter Ship To Pin" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftxtToPincode" runat="server" Enabled="True" FilterType="Numbers"
                                            TargetControlID="txtShipToPin">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShipFromGSTIN" runat="server" CssClass="styleReqFieldLabel" Text="Ship From GSTIN"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtShipFromGSTIN" MaxLength="15" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblShipToGSTIN" runat="server" CssClass="styleReqFieldLabel" Text="Ship To GSTIN"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtShipToGSTIN" MaxLength="15" runat="server"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNote" runat="server" Text="Note"></asp:Label>
                                    </td>
                                     <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtNote" runat="server" Height="40px" Width="250px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr class="styleButtonArea">
                                    <td colspan="4" align="center">
                                        <asp:Button runat="server" ID="btnGo" CssClass="styleSubmitButton"
                                            Text="Go" ValidationGroup="Main" OnClick="btnGo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel GroupingText="Stock Transfer Details" ID="pnlStock" runat="server" CssClass="stylePanel" Visible="false" Width="100%">
                                        <div id="divinterim" runat="server" style="overflow: scroll;">
                                            <asp:GridView ID="gvInvoiceMapping" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvInvoiceMapping_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkgvISelectAll" runat="server" Text="Select All" OnClick="javascript:fnSelectAllMapInvoice(this, 'chkgvISelectInv');" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkgvISelectInv" runat="server" OnClick="javascript:fnChkIsInvoiceSelectAll(this);"
                                                                Checked='<%# Eval("Is_Checked").ToString() == "1" ?  true:false %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="PO Dtl ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapPODtlID" runat="server" Text='<%# Bind("PO_Dtl_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PI Dtl ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapPIDtlID" runat="server" Text='<%# Bind("PI_Dtl_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VI Dtl ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapVIDtlID" runat="server" Text='<%# Bind("VI_Dtl_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="HSN ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapHSNID" runat="server" Text='<%# Bind("HSN_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice Dtl Id" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapInvoicedtlID" runat="server" Text='<%# Bind("Invoice_Dtl_Id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Entity ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapEntityID" runat="server" Text='<%# Bind("Entity_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Ship From" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblShipFrom" runat="server" Text='<%# Bind("Delivery_Location") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="PO Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapPONumber" runat="server" Text='<%# Bind("PO_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
													<asp:TemplateField HeaderText="Asset Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetDesc" runat="server" Text='<%# Bind("Asset_Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Category">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapAssetCategory" runat="server" Text='<%# Bind("Asset_Category") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapAssetType" runat="server" Text='<%# Bind("Asset_Type") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Sub Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapAssetClass" runat="server" Text='<%# Bind("Asset_Class") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Serial Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetSerialNumber" runat="server" Text='<%# Bind("Asset_Serial_number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapEntityName" runat="server" Text='<%# Bind("Entity_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice Type" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapInvoiceType" runat="server" Text='<%# Bind("Invoice_Type") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapInvoiceNumber" runat="server" Text='<%# Bind("Invoice_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RS Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapAccountNumber" runat="server" Text='<%# Bind("Panum") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delivery State">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Delivery_state") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapInvoiceAmount" runat="server" Text='<%# Bind("Invoice_Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Invoice Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblgvmapInvoiceDate" runat="server" Text='<%# Bind("Invoice_Date") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblquantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Moved Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMovedquantity" runat="server" Text='<%# Bind("Moved_Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Appl Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtapplquantity" OnTextChanged="txtapplquantity_TextChanged" Width="90%" AutoPostBack="true" runat="server" Style="text-align: right" Text='<%# Bind("Appl_quantity") %>'></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="Ftxtapplquantity" runat="server"
                                                                FilterType="Numbers" TargetControlID="txtapplquantity">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Unit Price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtUnitPrice" Style="text-align: right" runat="server" OnTextChanged="txtUnitPrice_TextChanged" AutoPostBack="true" Text='<%# Bind("Unit_Price") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Transfer Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtgvmapFacilityAmount" Style="text-align: right" runat="server" Text='<%# Bind("Transfer_Amount") %>'></asp:TextBox>
                                                            <%--<cc1:FilteredTextBoxExtender ID="FtxtTransferAmount" runat="server"
                                                                                                FilterType="Numbers" ValidChars="." TargetControlID="txtgvmapFacilityAmount">
                                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                            <uc1:PageNavigator ID="ucInvoiceMapping" Visible="false" runat="server" />
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnIsMapInvoiceChanged" runat="server" />
            <table align="center">
                <tr align="center" class="styleButtonArea">
                    <td align="center" colspan="4">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleSubmitButton"
                            OnClick="btnUpdate_Click" ToolTip="Update" />
                        <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" ValidationGroup="vsSave"
                            OnClientClick="return SaveConfirm(1);" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" ToolTip="Save" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" OnClientClick="return fnConfirmClear();"
                            CssClass="styleSubmitButton" OnClick="btnclear_Click" />
                        <asp:Button ID="btncancel" runat="server" Text="Cancel"
                            CssClass="styleSubmitButton" OnClick="btncancel_Click" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" Visible="false"
                            CssClass="styleSubmitButton" OnClick="btnPrint_Click" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="Main" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="Main" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="vsSave" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>

                        <asp:CustomValidator ID="cvTranche" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function fnConfirmSave() {
            if (confirm('Are you sure want to save?')) {
                return true;
            }
            else {
                return false;
            }
        }


        function fnSelectAllMapInvoice(chkSelectAllInvoice, chkSelectInvoice)        //
        {
            fnIsModifyInvMap();
            var gvInvMapping = document.getElementById('<%=gvInvoiceMapping.ClientID%>');
            var TargetChildControl = chkSelectInvoice;
            //Get all the control of the type INPUT in the base control.
            var Inputs = gvInvMapping.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllInvoice.checked;
        }

        //To Check/ Uncheck Select All based on Invoice Mapping selction
        function fnChkIsInvoiceSelectAll(chkSelect) {
            fnIsModifyInvMap();
            var gv = document.getElementById('<%= gvInvoiceMapping.ClientID%>');
            var chk = document.getElementById('ctl00_ContentPlaceHolder1_gvInvoiceMapping_ctl01_chkgvISelectAll');

            if (chkSelect.checked == false) {
                chk.checked = false;
            }
            else {
                var gvRwCnt = gv.rows.length - 1;
                var ChcCnt = 0;
                for (var i = 0; i < gv.rows.length; ++i) {
                    var Inputs = gv.rows[i].getElementsByTagName("input");
                    for (var n = 0; n < Inputs.length; ++n) {
                        if (Inputs[n].type == 'checkbox') {
                            if (Inputs[n].checked == true)
                                ++ChcCnt;
                        }
                    }
                }
                if (ChcCnt == gvRwCnt)
                    chk.checked = true;
                else
                    chk.checked = false;
            }
        }
        function SaveConfirm(varoption) {
            //debugger;
            if (varoption == "1") {
                return confirm('Do you want to Save?');
            }
            else {
                return confirm('Do you want to Update?');
            }
        }
        function fnIsModifyInvMap() {
            document.getElementById('<%= hdnIsMapInvoiceChanged.ClientID %>').value = "1";
        }

    </script>
</asp:Content>

