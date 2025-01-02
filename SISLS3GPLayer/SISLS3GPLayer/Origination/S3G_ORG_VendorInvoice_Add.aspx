<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_ORG_VendorInvoice_Add.aspx.cs"
    EnableEventValidation="false" Inherits="Origination_S3G_ORG_VendorInvoice_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register TagPrefix="uc3" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="50%">
                                    <asp:Panel GroupingText="Customer Details" ID="Panel2" runat="server" CssClass="stylePanel"
                                        ToolTip="Customer Details">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel" width="35%">
                                                    <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:LOV ID="ucCustomerCodeLov" onblur="return fnLoadCustomer()" runat="server" strLOV_Code="CMD" DispalyContent="Code" />
                                                    <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none;"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvcmbCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                        ErrorMessage="Select a Customer Code" ValidationGroup="Save" CssClass="styleMandatoryLabel"
                                                        Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" width="100%">
                                                    <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer" OnClick="btnLoadCustomer_OnClick" />
                                                    <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                        ShowCustomerCode="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td width="50%" valign="top">
                                    <asp:Panel GroupingText="Purchase Order Details" ID="pnlCustomerInfo" runat="server" CssClass="stylePanel">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPONo" runat="server" Text="Purchase Order Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlPONo" runat="server" ServiceMethod="GetPurchaseOrderNo" OnItem_Selected="ddlPONo_SelectedIndexChanged" ReadOnly="true"
                                                        ErrorMessage="Select a Purchase Order No" ValidationGroup="Save" IsMandatory="true" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDate" runat="server" Text="Purchase Order Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtDate" runat="server" ToolTip="Purchase Order Date"></asp:TextBox>
                                                    <asp:Label ID="LPODate" runat="server" value="" Visible="false"></asp:Label>
                                                    <asp:Label ID="GSTEffectiveDate" runat="server" value="" Visible="false"></asp:Label>
                                                    <asp:Label ID="PremiumUser" runat="server" value="" Visible="false"></asp:Label>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtVendorName" runat="server" ReadOnly="true" ToolTip="Vendor Name"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLoadSequenceNumber" runat="server" Text="Load Sequence Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLoadSequenceNumber" runat="server" ReadOnly="true" ToolTip="Load Sequence Number"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPINo" runat="server" Text="Vendor Invoice No" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPINo" runat="server" ToolTip="Vendor Invoice No" MaxLength="50"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvPINo" ValidationGroup="Save" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPINo" SetFocusOnError="True"
                                                        ErrorMessage="Enter a Vendor Invoice No" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvPINoUpd" ValidationGroup="Footer" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPINo" SetFocusOnError="True"
                                                        ErrorMessage="Enter a Vendor Invoice No" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPIDate" runat="server" Text="Vendor Invoice Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPIDate" runat="server" ToolTip="Vendor Invoice Date"></asp:TextBox>
                                                    <asp:Image ID="imgPIDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderPIDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgPIDate" TargetControlID="txtPIDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RFVPIDate" ValidationGroup="Save" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPIDate" SetFocusOnError="True"
                                                        ErrorMessage="Enter a Vendor Invoice Date" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Footer" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPIDate" SetFocusOnError="True"
                                                        ErrorMessage="Enter a Vendor Invoice Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPIStatus" runat="server" Text="Vendor Invoice Status" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPIStatus" runat="server" ReadOnly="true" ToolTip="Vendor Invoice Status"></asp:TextBox>
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
                    <td>
                        <cc1:TabContainer ID="tcPI" runat="server" CssClass="styleTabPanel"
                            Width="100%" ScrollBars="None">
                            <cc1:TabPanel runat="server" ID="InvoiceDet" CssClass="tabpan" BackColor="Red" HeaderText="tabInvoiceDet"
                                Width="100%">
                                <HeaderTemplate>
                                    Main
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvInvoiceDet" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvInvoiceDet_RowDataBound" Width="100%" ShowFooter="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Category" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCategory" runat="server" Text='<%# Eval("Asset_Category") %>' ToolTip="Asset Category" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGrandTotal" runat="server" Text="Total" ToolTip="Total" />
                                                        </FooterTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPIQuantity" runat="server" Text='<%# Eval("Quantity") %>'
                                                                ToolTip="Quantity" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotQuantity" runat="server" ToolTip="Total Quantity" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>'
                                                                ToolTip="Amount" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotAmount" runat="server" ToolTip="Total" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTax_Amount" runat="server" Text='<%# Eval("Tax_Amount") %>'
                                                                ToolTip="Tax Amount" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalTax_Amount" runat="server" ToolTip="Total" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("Total_Amount") %>'
                                                                ToolTip="Total Amount" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblGTotal" runat="server" ToolTip="Total Amount" />
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="AssetDet" CssClass="tabpan" BackColor="Red" HeaderText="tabAssetDet"
                                Width="100%">
                                <HeaderTemplate>
                                    Other Details
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 764px">
                                                <table width="99%">
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnInvoices" runat="server" Text="Select PO Items" ToolTip="Select PO Items" class="styleSubmitButton" OnClick="btnInvoices_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="gvDlivery" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" OnRowDataBound="gvDlivery_RowDataBound"
                                                                OnRowEditing="gvDlivery_RowEditing" OnRowUpdating="gvDlivery_RowUpdating" OnRowCancelingEdit="gvDlivery_RowCancelingEdit"
                                                                OnRowCommand="gvDlivery_RowCommand" Width="10192px">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <%# Container.DataItemIndex + 1 %>
                                                                            <asp:Label ID="lblPO_dtl_ID" runat="server" Text='<%# Eval("PO_dtl_ID") %>' Visible="false" />
                                                                            <asp:Label ID="lblPO_HDR_ID" runat="server" Text='<%# Eval("PO_Hdr_ID") %>' Visible="false" />
                                                                            <asp:Label ID="lblPI_dtl_ID" runat="server" Text='<%# Eval("VI_det_ID") %>' Visible="false" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Asset Category" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAssetCategory" runat="server" Text='<%# Eval("Asset_Category") %>' ToolTip="Asset Category" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Asset Type" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAssetType" runat="server" Text='<%# Eval("Asset_Type") %>'
                                                                                ToolTip="Asset Type" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Asset Sub Type" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAssetSubType" runat="server" Text='<%# Eval("Asset_SubType") %>'
                                                                                ToolTip="Asset Sub Type" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Vendor Branch Code" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVendor_Branch" runat="server" Text='<%# Eval("Vendor_Branch") %>'
                                                                                ToolTip="Vendor Branch Code" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Way Bill No" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWay_Bill_No" runat="server" Text='<%# Eval("Way_Bill_No") %>'
                                                                                ToolTip="Way Bill No" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtWay_Bill_No" runat="server" ToolTip="Way Bill No" MaxLength="50" Text='<%# Eval("Way_Bill_No") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Way Bill Issue Date" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWay_Bill_Issue_Date" runat="server" Text='<%# Eval("Way_Bill_Issue_Date") %>'
                                                                                ToolTip="Way Bill Issue Date" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtWay_Bill_Issue_Date" runat="server" ToolTip="Way Bill Issue Date" MaxLength="50" Text='<%# Eval("Way_Bill_Issue_Date") %>'></asp:TextBox>
                                                                            <asp:Image ID="imgWay_BillDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                            <cc1:CalendarExtender ID="CalendarExtenderWay_BillDate" runat="server" Enabled="True"
                                                                                PopupButtonID="imgWay_BillDate" TargetControlID="txtWay_Bill_Issue_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                                            </cc1:CalendarExtender>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <%--<asp:TemplateField HeaderText="Registration Status" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRegistration_Status" runat="server" Text='<%# Eval("Registration_Desc") %>' ToolTip="Registration Status" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlRegistration_Status" ValidationGroup="Footer" runat="server">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvlRegistration_Status" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="ddlRegistration_Status" ErrorMessage="Select Registration Status" InitialValue="0"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                     <asp:TemplateField HeaderText="Constitution" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblConstitution" runat="server" Text='<%# Eval("Constitution_Name") %>' ToolTip="Constitution" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlConstitution" ValidationGroup="Footer" runat="server">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvlConstitution" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="ddlConstitution" ErrorMessage="Select Constitution" InitialValue="0"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>--%>

                                                                    <asp:TemplateField HeaderText="Asset Description" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAsset_Descirption" runat="server" Text='<%# Eval("Asset_Description") %>'
                                                                                ToolTip="Asset Descirption" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Asset Serial Number" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAsset_Serial_number" runat="server" Text='<%# Eval("Asset_Serial_number") %>'
                                                                                ToolTip="Asset Serial Number" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtAsset_Serial_number" runat="server" ToolTip="Asset Serial Number" MaxLength="50" Text='<%# Eval("Asset_Serial_number") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Customer State" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomer_State" runat="server" Text='<%# Eval("Customer_State") %>'
                                                                                ToolTip="Customer State" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO Quantity" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("PO_Quantity") %>'
                                                                                ToolTip="PO Quantity" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="VI Quantity" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPIQuantity" runat="server" Text='<%# Eval("VI_Quantity") %>'
                                                                                ToolTip="PI Quantity" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtPIQuantity" runat="server" ToolTip="VI Quantity" Text='<%# Eval("VI_Quantity") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvPIQuantity" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="txtPIQuantity" ErrorMessage="Enter the VI Quantity"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="LBT Applicable (Yes/No)" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLBT" runat="server" Text='<%# Eval("LBT_App") %>' ToolTip="LBT Applicable (Yes/No)" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlLBT" ValidationGroup="Footer" runat="server">
                                                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="LBT Circle No" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLBTCircleNo" runat="server" Text='<%# Eval("LBT_Circle_No") %>'
                                                                                ToolTip="LBT Circle No" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlLBT_Circle_No" ValidationGroup="Footer" runat="server">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="LBT Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLBTRate" runat="server" Text='<%# Eval("LBT_Rate") %>'
                                                                                ToolTip="LBT Rate" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtLBTRate" runat="server" ToolTip="LBT Rate" Text='<%# Eval("LBT_Rate") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="LBT Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLBTAmount" runat="server" Text='<%# Eval("LBT_Amount") %>'
                                                                                ToolTip="LBT Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Purchase Tax Applicable" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPurchase_Tax_App" runat="server" Text='<%# Eval("Purchase_Tax_App") %>' ToolTip="Purchase Tax Applicable" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlPurchaseTaxApp" ValidationGroup="Footer" runat="server">
                                                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Purchase Tax Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPurchase_Tax_Rate" runat="server" Text='<%# Eval("Purchase_Tax_Rate") %>'
                                                                                ToolTip="Purchase Tax Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Purchase Add Tax" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPurchase_Add_Tax" runat="server" Text='<%# Eval("Purchase_Add_Tax") %>'
                                                                                ToolTip="Purchase Add Tax" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Purchase Add Based On" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPurchase_Add_Based_On" runat="server" Text='<%# Eval("Purchase_Add_Based_On") %>'
                                                                                ToolTip="Purchase Add Based On" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Purchase Base Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPurchase_Base_Tax_Amount" runat="server" Text='<%# Eval("Purchase_Base_Tax_Amount") %>'
                                                                                ToolTip="Purchase Base Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Purchase Add Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPurchase_Add_Tax_Amount" runat="server" Text='<%# Eval("Purchase_Add_Tax_Amount") %>'
                                                                                ToolTip="Purchase Add Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Purchase Tax" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPurchase_Tax" runat="server" Text='<%# Eval("Purchase_Tax") %>'
                                                                                ToolTip="Purchase Tax" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Currency Code" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# Eval("Currency_Code") %>'
                                                                                ToolTip="Currency Code" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtCurrencyCode" runat="server" MaxLength="3" ToolTip="Currency Code" Text='<%# Eval("Currency_Code") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvCurrencyCode" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="txtCurrencyCode" ErrorMessage="Enter the Currency Code"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Exchange Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblExchangeRate" runat="server" Text='<%# Eval("Exchange_Rate") %>'
                                                                                ToolTip="Exchange Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Bill Amount in FC" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal_Bill_Amount_FC" runat="server" Text='<%# Eval("Bill_Amount_USD") %>'
                                                                                ToolTip="Total Bill Amount in FC" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtBill_Amount_FC" runat="server" ToolTip="Total Bill Amount in FC" Text='<%# Eval("Bill_Amount_USD") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvBill_Amount_FC" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="txtBill_Amount_FC" ErrorMessage="Enter the Total Bill Amount in FC"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Total Bill Amount in INR" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal_Bill_Amount_INR" runat="server" Text='<%# Eval("Bill_Amount_INR") %>'
                                                                                ToolTip="Total Bill Amount in INR" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtBill_Amount_INR" runat="server" ToolTip="Total Bill Amount in INR" Text='<%# Eval("Bill_Amount_INR") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvBill_Amount_INR" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="txtBill_Amount_INR" ErrorMessage="Enter the Total Bill Amount in INR"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Base Inv Amt(Excl Tax)-Material" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBase_Inv_Amt_Mat" runat="server" Text='<%# Eval("Inv_Amt_Material") %>'
                                                                                ToolTip="Base Inv Amt(Excl Tax) - Material" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtBase_Inv_Amt_Mat" runat="server" ToolTip="Base Inv Amt(Excl Tax) - Material" Text='<%# Eval("Inv_Amt_Material") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvBase_Inv_Amt_Mat" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="txtBase_Inv_Amt_Mat" ErrorMessage="Enter the Base Inv Amt(Excl Tax) - Material"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Base Inv Amt(Excl Tax)-Labour" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBase_Inv_Amt_Lab" runat="server" Text='<%# Eval("Inv_Amt_Labour") %>'
                                                                                ToolTip="Base Inv Amt(Excl Tax) - Labour" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtBase_Inv_Amt_Lab" runat="server" ToolTip="Base Inv Amt(Excl Tax) - Labour" Text='<%# Eval("Inv_Amt_Labour") %>'></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvBase_Inv_Amt_Lab" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                                runat="server" ControlToValidate="txtBase_Inv_Amt_Lab" ErrorMessage="Enter the Base Inv Amt(Excl Tax) - Labour"
                                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <%--GST Starts--%>
                                                                    <asp:TemplateField HeaderText="HSN Code" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNCode" runat="server" Text='<%# Eval("HSNCode") %>'
                                                                                ToolTip="HSN Code" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <uc2:Suggest ID="txtHSNCodeIT" SelectedText='<%# Eval("HSNCode") %>' SelectedValue='<%# Eval("HSNCodeID") %>' runat="server" ToolTip="HSN Code" ServiceMethod="GetHSNCode" />
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SAC Code" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACCode" runat="server" Text='<%# Eval("SACCode") %>'
                                                                                ToolTip="SAC Code" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <uc2:Suggest ID="txtSACCodeIT" SelectedText='<%# Eval("SACCode") %>' SelectedValue='<%# Eval("SACCodeID") %>' runat="server" ToolTip="SAC Code" ServiceMethod="GetSACCode" />
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HSN SGST" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNSGST" runat="server" Text='<%# Eval("HSNSGST") %>'
                                                                                ToolTip="HSN SGST" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtHSNSGSTIT" runat="server" ToolTip="HSN SGST" Text='<%# Eval("HSNSGST") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="HSN CGST" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNCGST" runat="server" Text='<%# Eval("HSNCGST") %>'
                                                                                ToolTip="HSN CGST" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtHSNCGSTIT" runat="server" ToolTip="HSN CGST" Text='<%# Eval("HSNCGST") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="HSN IGST" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNIGST" runat="server" Text='<%# Eval("HSNIGST") %>'
                                                                                ToolTip="HSN IGST" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtHSNIGSTIT" runat="server" ToolTip="HSN IGST" Text='<%# Eval("HSNIGST") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SAC SGST" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACSGST" runat="server" Text='<%# Eval("SACSGST") %>'
                                                                                ToolTip="SAC SGST" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtSACSGSTIT" runat="server" ToolTip="SAC SGST" Text='<%# Eval("SACSGST") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SAC CGST" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACCGST" runat="server" Text='<%# Eval("SACCGST") %>'
                                                                                ToolTip="SAC CGST" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtSACCGSTIT" runat="server" ToolTip="SAC CGST" Text='<%# Eval("SACCGST") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SAC IGST" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACIGST" runat="server" Text='<%# Eval("SACIGST") %>'
                                                                                ToolTip="SAC IGST" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtSACIGSTIT" runat="server" ToolTip="SAC IGST" Text='<%# Eval("SACIGST") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Billing State" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillingState" runat="server" Text='<%# Eval("BillingState") %>'
                                                                                ToolTip="Billing State" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <uc2:Suggest ID="txtBillingStateIT" SelectedText='<%# Eval("BillingState") %>' SelectedValue='<%# Eval("BillingStateID") %>'
                                                                                runat="server" AutoPostBack="true" ToolTip="Billing State" ServiceMethod="GetState" />
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Billing Branch" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBillingBranch" runat="server" Text='<%# Eval("BillingBranch") %>'
                                                                                ToolTip="Billing Branch" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <uc2:Suggest ID="txtBillingBranchIT" SelectedText='<%# Eval("BillingBranch") %>' SelectedValue='<%# Eval("BillingBranchID") %>'
                                                                                runat="server" ToolTip="Billing Branch" ServiceMethod="GetBranchIT" />
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="HSN SGST ITC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNSGSTITC_Value" runat="server" Text='<%# Eval("HSNSGSTITC_Value") %>'
                                                                                ToolTip="HSN SGST ITC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HSN CGST ITC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNCGSTITC_Value" runat="server" Text='<%# Eval("HSNCGSTITC_Value") %>'
                                                                                ToolTip="HSN CGST ITC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HSN IGST ITC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNIGSTITC_Value" runat="server" Text='<%# Eval("HSNIGSTITC_Value") %>'
                                                                                ToolTip="HSN IGST ITC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SAC SGST ITC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACSGSTITC_Value" runat="server" Text='<%# Eval("SACSGSTITC_Value") %>'
                                                                                ToolTip="SAC SGST ITC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SAC CGST ITC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACCGSTITC_Value" runat="server" Text='<%# Eval("SACCGSTITC_Value") %>'
                                                                                ToolTip="SAC CGST ITC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SAC IGST ITC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACIGSTITC_Value" runat="server" Text='<%# Eval("SACIGSTITC_Value") %>'
                                                                                ToolTip="SAC IGST ITC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="HSN SGST RC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNSGSTRC_Value" runat="server" Text='<%# Eval("HSNSGSTRC_Value") %>'
                                                                                ToolTip="HSN SGST RC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HSN CGST RC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNCGSTRC_Value" runat="server" Text='<%# Eval("HSNCGSTRC_Value") %>'
                                                                                ToolTip="HSN CGST RC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="HSN IGST RC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblHSNIGSTRC_Value" runat="server" Text='<%# Eval("HSNIGSTRC_Value") %>'
                                                                                ToolTip="HSN IGST RC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SAC SGST RC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACSGSTRC_Value" runat="server" Text='<%# Eval("SACSGSTRC_Value") %>'
                                                                                ToolTip="SAC SGST RC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SAC CGST RC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACCGSTRC_Value" runat="server" Text='<%# Eval("SACCGSTRC_Value") %>'
                                                                                ToolTip="SAC CGST RC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="SAC IGST RC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSACIGSTRC_Value" runat="server" Text='<%# Eval("SACIGSTRC_Value") %>'
                                                                                ToolTip="SAC IGST RC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <%--GST End--%>
                                                                    <asp:TemplateField HeaderText="VAT" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVAT" runat="server" Text='<%# Eval("VAT") %>'
                                                                                ToolTip="VAT" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtVAT" runat="server" ToolTip="VAT" Text='<%# Eval("VAT") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="CST" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCST" runat="server" Text='<%# Eval("CST") %>'
                                                                                ToolTip="CST" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtCST" runat="server" ToolTip="CST" Text='<%# Eval("CST") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Excise Duty/CVD (Incl Cess)" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblExcise_Duty" runat="server" Text='<%# Eval("Excise_Duty_CVD") %>'
                                                                                ToolTip="Excise Duty/CVD (Incl Cess)" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtExcise_Duty" runat="server" ToolTip="Excise Duty/CVD (Incl Cess)" Text='<%# Eval("Excise_Duty_CVD") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Service Tax Amt (If applicable)" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblService_Tax" runat="server" Text='<%# Eval("Service_Tax_Amt") %>'
                                                                                ToolTip="Service Tax Amt (If applicable)" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtService_Tax" runat="server" ToolTip="Service Tax Amt" Text='<%# Eval("Service_Tax_Amt") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Others (Freight/ Octroi, etc)" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOthers" runat="server" Text='<%# Eval("Others") %>'
                                                                                ToolTip="Others (Freight/ Octroi, etc)" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtOthers" runat="server" ToolTip="Others (Freight/ Octroi, etc)" Text='<%# Eval("Others") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Bill Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal_Bill_Amount" runat="server" Text='<%# Eval("Total_Bill_Amount") %>'
                                                                                ToolTip="Total Bill Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Cr Note Number" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCrNoteNumber" runat="server" Text='<%# Eval("Cr_Note_Number") %>'
                                                                                ToolTip="Cr Note Number" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtCrNoteNumber" runat="server" MaxLength="50" ToolTip="Cr Note Number" Text='<%# Eval("Cr_Note_Number") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Cr Note Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCrNoteAmount" runat="server" Text='<%# Eval("Credit_Note_Amount") %>'
                                                                                ToolTip="Cr Note Amount" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtCrNoteAmount" runat="server" ToolTip="Cr Note Amount" Text='<%# Eval("Credit_Note_Amount") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total SCH Amt" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal_SCH_Amt" runat="server" Text='<%# Eval("Total_SCH_Amt") %>'
                                                                                ToolTip="Total SCH Amt" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Asset Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAsset_Amount" runat="server" Text='<%# Eval("Asset_Amount") %>'
                                                                                ToolTip="Asset Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Base Amt VAT" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBaseAmtVAT" runat="server" Text='<%# Eval("Base_Amt_VAT") %>'
                                                                                ToolTip="Base Amt VAT" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtBaseAmtVAT" runat="server" ToolTip="Base Amt VAT" Text='<%# Eval("Base_Amt_VAT") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="ITC" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblITC" runat="server" Text='<%# Eval("ITC_App") %>'
                                                                                ToolTip="ITC" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlITC" ValidationGroup="Footer" runat="server">
                                                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="ITC Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblITC_Value" runat="server" Text='<%# Eval("ITC_Value") %>'
                                                                                ToolTip="ITC Value" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="VAT %" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVAT1" runat="server" Text='<%# Eval("VAT_Percentage") %>'
                                                                                ToolTip="VAT %" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtVAT1" runat="server" ToolTip="VAT %" Text='<%# Eval("VAT_Percentage") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Other Bill Component(Service/Freight)" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblOtherBillComponent" runat="server" Text='<%# Eval("Other_Bill_Component") %>'
                                                                                ToolTip="Other Bill Component(Service/Freight)" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtOtherBillComponent" runat="server" ToolTip="Other Bill Component(Service/Freight)" Text='<%# Eval("Other_Bill_Component") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Tax Rebate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVATRebate" runat="server" Text='<%# Eval("VAT_Rebt") %>'
                                                                                ToolTip="Tax Rebate" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlVATRebate" ValidationGroup="Footer" runat="server">
                                                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="TDS Section" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTDSSection" runat="server" Text='<%# Eval("TDS_Section") %>'
                                                                                ToolTip="TDS Section" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtTDSSection" runat="server" ToolTip="TDS Section" Text='<%# Eval("TDS_Section") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="TDS Base Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTDSBaseValue" runat="server" Text='<%# Eval("TDS_Base_Value") %>'
                                                                                ToolTip="TDS Base Value" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtTDSBaseValue" runat="server" ToolTip="TDS Base Value" Text='<%# Eval("TDS_Base_Value") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="TDS Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTDSRate" runat="server" Text='<%# Eval("TDS_Rate") %>'
                                                                                ToolTip="TDS Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="TDS Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTDSAmount" runat="server" Text='<%# Eval("TDS_Amount") %>'
                                                                                ToolTip="TDS Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="WCT Base Value" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWCTBaseValue" runat="server" Text='<%# Eval("WCT_Base_Value") %>'
                                                                                ToolTip="WCT Base Value" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtWCTBaseValue" runat="server" ToolTip="TDS Base Value" Text='<%# Eval("WCT_Base_Value") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="WCT Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWCTRate" runat="server" Text='<%# Eval("WCT_Rate") %>'
                                                                                ToolTip="WCT Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="WCT Add. Tax Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWCT_Add_Tax" runat="server" Text='<%# Eval("WCT_Add_Tax") %>'
                                                                                ToolTip="WCT Add. Tax Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Add. Tax based on" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWCT_Add_Based_On" runat="server" Text='<%# Eval("WCT_Add_Based_On") %>'
                                                                                ToolTip="Add. Tax based on" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Base Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWCT_Base_Tax_Amount" runat="server" Text='<%# Eval("WCT_Base_Tax_Amount") %>'
                                                                                ToolTip="Base Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Add. Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWCT_Add_Tax_Amount" runat="server" Text='<%# Eval("WCT_Add_Tax_Amount") %>'
                                                                                ToolTip="Add. Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="WCT" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblWCT" runat="server" Text='<%# Eval("WCT_Amount") %>'
                                                                                ToolTip="WCT" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="CENVAT Amount Passed on to Cusomer" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCENVAT" runat="server" Text='<%# Eval("CENVAT_Amount") %>'
                                                                                ToolTip="CENVAT Amount passed on to cusomer" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtCENVAT" runat="server" ToolTip="CENVAT Amount Passed on to Cusomer" Text='<%# Eval("CENVAT_Amount") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Retention" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRetention" runat="server" Text='<%# Eval("Retention") %>'
                                                                                ToolTip="Retention" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtRetention" runat="server" ToolTip="Retention" Text='<%# Eval("Retention") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Deduction" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotal_Deduction" runat="server" Text='<%# Eval("Total_Deduction") %>'
                                                                                ToolTip="Total Deduction" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Net Payable Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNet_Payable_Amount" runat="server" Text='<%# Eval("Net_Payable_Amount") %>'
                                                                                ToolTip="Net Payable Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Entry Tax Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEntry_Tax_Rate" runat="server" Text='<%# Eval("Entry_Tax_Rate") %>'
                                                                                ToolTip="Entry Tax Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Add. Entry Tax Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEntry_Add_Tax_Rate" runat="server" Text='<%# Eval("Entry_Add_Tax_Rate") %>'
                                                                                ToolTip="Add. Entry Tax Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Base Entry Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEntry_Base_Tax_Amount" runat="server" Text='<%# Eval("Entry_Base_Tax_Amount") %>'
                                                                                ToolTip="Base Entry Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Add. Entry Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEntry_Add_Tax_Amount" runat="server" Text='<%# Eval("Entry_Add_Tax_Amount") %>'
                                                                                ToolTip="Add. Entry Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Entry Tax" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEntry_Tax_Amount" runat="server" Text='<%# Eval("Entry_Tax_Amount") %>'
                                                                                ToolTip="Entry Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Reverse Charge Catogory Type" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReverseCharge" runat="server" Text='<%# Eval("Reverse_Charge_Type") %>'
                                                                                ToolTip="Reverse Charge Catogory Type" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlReverseCharge" ValidationGroup="Footer" runat="server">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Base Value for Reverse Charge Computation" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBaseValue" runat="server" Text='<%# Eval("reverse_charge_base_Value") %>'
                                                                                ToolTip="Base Value for reverse charge computation" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtBaseValue" runat="server" ToolTip="Base Value for reverse charge computation" Text='<%# Eval("reverse_charge_base_Value") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Service Tax Payable by OPC" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReverse_Charge_Tax" runat="server" Text='<%# Eval("Reverse_Charge_Tax") %>'
                                                                                ToolTip="Service Tax Payable by OPC" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Add. Tax Rate" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReverse_Charge_Add_Tax" runat="server" Text='<%# Eval("Reverse_Charge_Add_Tax") %>'
                                                                                ToolTip="Add. Tax Rate" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Add. Tax Based On" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRSCT_Add_Tax_BasedOn" runat="server" Text='<%# Eval("RSCT_Add_Tax_BasedOn") %>'
                                                                                ToolTip="Add. Tax Based On" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Base RCC Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRSCT_Base_Tax_Amount" runat="server" Text='<%# Eval("RSCT_Base_Tax_Amount") %>'
                                                                                ToolTip="Base RCC Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Add. RCC Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRSCT_Add_Tax_Amount" runat="server" Text='<%# Eval("RSCT_Add_Tax_Amount") %>'
                                                                                ToolTip="Add. RCC Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Reverse Charge Tax Amount" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReverse_Charge_Tax_Amount" runat="server" Text='<%# Eval("Reverse_Charge_Tax_Amount") %>'
                                                                                ToolTip="Reverse Charge Tax Amount" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Vend Inv Type" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblVendInvType" runat="server" Text='<%# Eval("Vend_Inv_Type") %>'
                                                                                ToolTip="Vend Inv Type" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList ID="ddlVendInvType" ValidationGroup="Footer" runat="server">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Remarks" HeaderStyle-CssClass="styleGridHeader">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'
                                                                                ToolTip="Remarks" />
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" ToolTip="Remarks" Text='<%# Eval("Remarks") %>'></asp:TextBox>
                                                                        </EditItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                                                ToolTip="Edit">
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                                                ValidationGroup="Footer" ToolTip="Update"></asp:LinkButton>&nbsp;|
                                                                            <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                                                CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                            <uc3:PageNavigator ID="ucCustomPaging" runat="server"></uc3:PageNavigator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td id="Td1" runat="server" align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            OnClick="btnSave_Click" ValidationGroup="Save" OnClientClick="return fnCheckPageValidators('Save')" />
                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                            CausesValidation="False" OnClick="btnClear_Click" />
                        <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
                            Enabled="True" TargetControlID="btnClear">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel" OnClick="btnCancel_Click" ToolTip="Cancel" />
                        <asp:Button runat="server" ID="btnDICancel" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel VI" OnClick="btnDICancel_Click" ToolTip="Cancel VI" OnClientClick="return fnConfirmCancelDI()" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="rowindex" runat="server" />
                        <asp:ValidationSummary ID="vsDelivery" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Save" />
                        <asp:ValidationSummary ID="vsFooter" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Footer" />
                        <asp:CustomValidator ID="cvDelivery" runat="server" Display="None" CssClass="styleMandatoryLabel">
                        </asp:CustomValidator>
                        <asp:CustomValidator ID="cv_TabMainPage" runat="server" CssClass="styleMandatoryLabel"
                            Display="None" ValidationGroup="Submit"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Visible="false" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="btnModal" Style="display: none" runat="server" />
    <cc1:ModalPopupExtender ID="ModalPopupExtenderApprover" runat="server" TargetControlID="btnModal"
        PopupControlID="pnlInvoices" BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlInvoices" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="90%">
        <asp:UpdatePanel ID="upnlinvoices" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>Type</td>
                                    <td>
                                        <asp:DropDownList ID="ddlFilterType" runat="server">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Asset Category</asp:ListItem>
                                            <asp:ListItem Value="2">Asset Type</asp:ListItem>
                                            <asp:ListItem Value="3">Asset Sub Type</asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td>
                                        <asp:TextBox AutoCompleteType="None" ID="txtFilterValue" runat="server"
                                            CssClass="styleSearchBox"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" ToolTip="Go"
                                            CssClass="styleSubmitShortButton" OnClick="btnGoInvoice_Click" ValidationGroup="btngo" /></td>
                                    <td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grvInvoices" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grvInvoices_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            <table>
                                                <tr>
                                                    <td>Select All
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkAll" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                            <asp:Label ID="lblPO_dtl_ID1" runat="server" Text='<%# Eval("PO_dtl_ID") %>' Visible="false" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Asset Category">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetCategory" runat="server" Text='<%# Bind("Asset_Category") %>'></asp:Label>
                                            <asp:Label ID="lblAssetCategoryID" Visible="false" runat="server" Text='<%# Bind("Asset_Category_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Asset Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetType" runat="server" Text='<%# Bind("Asset_Type") %>'></asp:Label>
                                            <asp:Label ID="lblAssetTypeID" Visible="false" runat="server" Text='<%# Bind("Asset_Type_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Asset Sub Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetCategorySG" runat="server" Text='<%# Bind("Asset_SubType") %>'></asp:Label>
                                            <asp:Label ID="lblAssetCategoryIDSG" Visible="false" runat="server" Text='<%# Bind("Asset_Sub_Type_ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Asset Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetDescription" runat="server" Text='<%# Bind("Asset_Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <uc3:PageNavigator ID="ucPopUpPaging" runat="server"></uc3:PageNavigator>
                            <input type="hidden" id="hdnSortDirection" runat="server" />
                            <input type="hidden" id="hdnSortExpression" runat="server" />
                            <input type="hidden" id="hdnSearch" runat="server" />
                            <input type="hidden" id="hdnOrderBy" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnDEVModalMove" runat="server" Text="Move" ToolTip="Move" class="styleSubmitButton"
                                OnClick="btnDEVModalMove_Click" />
                            <asp:Button ID="btnDEVModalCancel" runat="server" Text="Close" OnClick="btnDEVModalCancel_Click"
                                ToolTip="Close" class="styleSubmitButton" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:CustomValidator ID="cvPopUP" runat="server" CssClass="styleMandatoryLabel"
                                Enabled="true" Width="98%" ValidationGroup="btngo" /></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <script language="javascript" type="text/javascript">
        function fnConfirmCancelDI() {

            if (confirm('Do you want to cancel VI?')) {
                return true;
            }
            else
                return false;

        }
        function fnConfirmSavePI() {

            if (confirm('Do you want to save VI?')) {
                return true;
            }
            else
                return false;

        }

        function fnBlur() {
            var Quantity;
            var total;
            var actualvalue;
            var mode = ("<%= Request.QueryString["qsMode"] %>");
            if (mode == "C") {
                var Grid = document.getElementById("<%=gvDlivery.ClientID%>");
                if (Grid != null) {
                    var intGridLen = Grid.rows.length;
                    for (var i = 2; i <= intGridLen; i++) {
                        if (i < 10)
                            i = "0" + i;
                        var txtQuantity = document.getElementById(Grid.id + "_ctl" + i + "_txtQuantity");
                        var txtDyAsset = document.getElementById(Grid.id + "_ctl" + i + "_txtDyAsset");
                        var txtAssetValue = document.getElementById(Grid.id + "_ctl" + i + "_txtAssetValue");
                        if (txtAssetValue == null) {
                            txtAssetValue.value = 0;
                        }
                        else if (txtDyAsset == null) {
                            txtDyAsset.value = 0;
                        }

                        if ((txtQuantity.value != "") && (txtAssetValue.value != "") && (txtDyAsset.value != "")) {
                            txtAssetValue.value = ((txtQuantity.value) * (txtDyAsset.value));

                        }
                        else {
                            txtAssetValue.value = 0;
                        }
                    }
                }
            }
        }
        function fnLoadCustomer() {
            document.getElementById('<%=btnLoadCustomer.ClientID %>').click();
        }

        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
        }
        function pageLoad(s, a) {
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
        }
        function showMenu(show) {
            if (show == 'T') {
                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - 260;
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

    </script>
</asp:Content>
