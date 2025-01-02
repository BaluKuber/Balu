<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_ORG_PurchaseOrder_Add.aspx.cs"
    EnableEventValidation="false" Inherits="Origination_S3G_ORG_PurchaseOrder_Add"
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
                                                    <asp:Label ID="lblLSQNo" runat="server" Text="Load Sequence Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLSQNo" runat="server" ToolTip="Load Sequence Number"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDate" runat="server" Text="PO Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtDate" runat="server" ToolTip="PO Date" OnTextChanged="txtDate_TextChanged"></asp:TextBox>
                                                    <asp:Label ID="LPODate" runat="server" value="" Visible="false"></asp:Label>
                                                    <asp:Label ID="GSTEffectiveDate" runat="server" value="" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDLNo" runat="server" Text="PO Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLPONo" Width="250px" runat="server" ReadOnly="true" ToolTip="PO Number"></asp:TextBox>
                                                </td>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblPOStatus" runat="server" Text="PO Status" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtPOStatus" runat="server" ReadOnly="true" ToolTip="PO Status"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="VendorName" runat="server" Text="Vendor Name" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="txtVendorName" runat="server" ServiceMethod="GetVendors" OnItem_Selected="txtVendorName_SelectedIndexChanged"
                                                        ErrorMessage="Select a Vendor Name" ValidationGroup="Save" IsMandatory="true" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCusCustomer_Name" runat="server" Text="Customer&#8217;s Customer Name" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCusCustomer_Name" runat="server" MaxLength="100" ToolTip="Customer&#8217;s Customer Name"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lbllblCust_PO_Ref_No" runat="server" Text="Customer PO Ref No" CssClass="style"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCust_PO_Ref_No" runat="server" MaxLength="40" ToolTip="Customer&#8217;s Customer Name"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLoc_Code" runat="server" Text="Location Code" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLoc_Code" runat="server" Width="160px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLoc_Code" runat="server" ErrorMessage="Select the Location Code"
                                                        ValidationGroup="Save" InitialValue="0" Display="None" SetFocusOnError="True" ControlToValidate="ddlLoc_Code"></asp:RequiredFieldValidator>
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
                        <asp:Panel GroupingText="Asset Details" ID="pnlAssetDet" runat="server" CssClass="stylePanel" Width="99%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 764px">
                                <table width="99%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvDlivery" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" OnRowDataBound="gvDlivery_RowDataBound"
                                                OnRowEditing="gvDlivery_RowEditing" OnRowUpdating="gvDlivery_RowUpdating" OnRowCancelingEdit="gvDlivery_RowCancelingEdit"
                                                OnRowCommand="gvDlivery_RowCommand" OnRowDeleting="gvDlivery_RowDeleting" ShowFooter="true" Width="4052px">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <%# Container.DataItemIndex + 1 %>
                                                            <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("Sl_No") %>' Visible="false" />
                                                            <asp:Label ID="lblPO_dtl_ID" runat="server" Text='<%# Eval("PO_dtl_ID") %>' Visible="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Category" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetCategory" runat="server" Text='<%# Eval("Asset_Category") %>' ToolTip="Asset Category" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlAssetCategory" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged" AutoPostBack="true"
                                                                ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvAssetCategory" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetCategory" InitialValue="0" ErrorMessage="Select the Asset Category"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlAssetCategoryhdr" OnSelectedIndexChanged="ddlAssetCategoryhdr_SelectedIndexChanged" AutoPostBack="true"
                                                                ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvAssetCategoryhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetCategoryhdr" InitialValue="0" ErrorMessage="Select the Asset Category"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvAssetCategory1" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetCategoryhdr" InitialValue="0" ErrorMessage="Select the Asset Category"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Type" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetType" runat="server" Text='<%# Eval("Asset_Type") %>'
                                                                ToolTip="Asset Type" />
                                                        </ItemTemplate>

                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlAssetType" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged" AutoPostBack="true"
                                                                ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvAssetType" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetType" InitialValue="0" ErrorMessage="Select the Asset Type"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlAssetTypehdr" OnSelectedIndexChanged="ddlAssetTypehdr_SelectedIndexChanged" AutoPostBack="true"
                                                                ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvAssetTypehdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetTypehdr" InitialValue="0" ErrorMessage="Select the Asset Type"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvAssetTypehdr1" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetTypehdr" InitialValue="0" ErrorMessage="Select the Asset Type"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Sub Type" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetSubType" runat="server" Text='<%# Eval("Asset_SubType") %>'
                                                                ToolTip="Asset Sub Type" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlAssetSubType" ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvAssetSubType" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetSubType" InitialValue="0" ErrorMessage="Select the Asset Sub Type"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlAssetSubTypehdr" ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvAssetSubTypehdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetSubTypehdr" InitialValue="0" ErrorMessage="Select the Asset Sub Type"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvAssetSubTypehdr1" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlAssetSubTypehdr" InitialValue="0" ErrorMessage="Select the Asset Sub Type"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor Branch State" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVendor_Branch" runat="server" Text='<%# Eval("Vendor_Branch") %>'
                                                                ToolTip="Vendor Branch State" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlVendor_Branch" ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvVendor_Branch" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlVendor_Branch" InitialValue="0" ErrorMessage="Select the Vendor Branch"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlVendor_Branchhdr" ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvVendor_Branchhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlVendor_Branchhdr" InitialValue="0" ErrorMessage="Select the Vendor Branch"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlVendor_Branchhdr" InitialValue="0" ErrorMessage="Select the Vendor Branch"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Location Code" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLoc_Code" runat="server" Text='<%# Eval("Loc_Code") %>'
                                                                ToolTip="Location Code" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtLoc_Code" runat="server" ToolTip="Location Code" ValidationGroup="Footer" MaxLength="10"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvLoc_Code" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtLoc_Code"  ErrorMessage="Enter the Location Code"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <cc1:FilteredTextBoxExtender ID="FTELoc_Code" FilterMode="ValidChars" TargetControlID="txtLoc_Code"
                                                            runat="server" Enabled="True" ValidChars="QWERTYUIOPLKJHGFDSAZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLoc_CodeHdr" runat="server" Text='<%# Eval("Loc_Code") %>' ToolTip="Location Code" MaxLength="10"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvLoc_CodeHdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtLoc_CodeHdr" ErrorMessage="Enter the Location Code"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                            <cc1:FilteredTextBoxExtender ID="FTELoc_CodeHdr" FilterMode="ValidChars" TargetControlID="txtLoc_CodeHdr"
                                                            runat="server" Enabled="True" ValidChars="QWERTYUIOPLKJHGFDSAZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="UI Number" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblASNo" runat="server" Text='<%# Eval("AS_NO") %>'
                                                                ToolTip="AS No" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtASNo" runat="server" ToolTip="AS No" MaxLength="5"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtASNohdr" runat="server" Text='<%# Eval("AS_NO") %>' ToolTip="AS NO" MaxLength="5"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Descirption" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAsset_Descirption" runat="server" Text='<%# Eval("Asset_Description") %>'
                                                                ToolTip="Asset Descirption" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAsset_Descirption" runat="server" ToolTip="Asset Descirption" MaxLength="1000"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvAsset_Descirption" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtAsset_Descirption" ErrorMessage="Enter the Asset Descirption"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtAsset_Descirptionhdr" runat="server" Text='<%# Eval("Asset_Description") %>' ToolTip="Asset Descirption" MaxLength="1000"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvAsset_Descirptionhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtAsset_Descirptionhdr" InitialValue="0" ErrorMessage="Enter the Asset Descirption"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                            <%--<asp:RequiredFieldValidator ID="txtAsset_Descirptionhdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtAsset_Descirptionhdr" InitialValue="0" ErrorMessage="Enter the Asset Descirption"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Model Name" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblModel_Name" runat="server" Text='<%# Eval("Model_Name") %>'
                                                                ToolTip="Model Name" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtModel_Name" runat="server" ToolTip="Model Name" MaxLength="50"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtModel_Namehdr" runat="server" Text='<%# Eval("Model_Name") %>' ToolTip="Model Name" MaxLength="50"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Manufacturer Name" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblManufacturer_Name" runat="server" Text='<%# Eval("Manufacturer_Name") %>'
                                                                ToolTip="Manufacturer Name" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtManufacturer_Name" runat="server" ToolTip="Manufacturer Name" MaxLength="50"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtManufacturer_Namehdr" runat="server" Text='<%# Eval("Manufacturer_Name") %>' ToolTip="Manufacturer Name" MaxLength="50"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Delivery State" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomer_State" runat="server" Text='<%# Eval("Customer_State") %>'
                                                                ToolTip="Delivery State" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlCustomer_State" ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCustomer_State" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlCustomer_State" InitialValue="0" ErrorMessage="Select the Delivery State"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlCustomer_Statehdr" ValidationGroup="Footer" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCustomer_Statehdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlCustomer_Statehdr" InitialValue="0" ErrorMessage="Select the Customer State"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvCustomer_Statehdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="ddlCustomer_Statehdr" InitialValue="0" ErrorMessage="Select the Customer State"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer PO Ref No" Visible="false" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCust_PO_Ref_No" runat="server" Text='<%# Eval("Customer_PO_Ref_No") %>'
                                                                ToolTip="Customer PO Ref No" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtCust_PO_Ref_No" runat="server" ToolTip="Customer PO Ref No" MaxLength="30"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvCust_PO_Ref_No" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtCust_PO_Ref_No" ErrorMessage="Enter the Customer PO Ref No"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCust_PO_Ref_Nohdr" runat="server" MaxLength="30" ToolTip="Customer PO Ref No" Text='<%# Eval("Customer_PO_Ref_No") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvCust_PO_Ref_Nohdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtCust_PO_Ref_Nohdr" ErrorMessage="Enter the Customer PO Ref No"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reference No" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuot_Ref_No" runat="server" Text='<%# Eval("Quotation_Ref_No") %>'
                                                                ToolTip="Reference No" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtQuot_Ref_No" runat="server" ToolTip="Reference No" MaxLength="100"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvQuot_Ref_No" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtQuot_Ref_No" ErrorMessage="Enter the Quotation Ref No"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtQuot_Ref_Nohdr" runat="server" MaxLength="100" ToolTip="Quotation Ref No" Text='<%# Eval("Quotation_Ref_No") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvQuot_Ref_Nohdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtQuot_Ref_Nohdr" ErrorMessage="Enter the Quotation Ref No"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="OPC Billing Address" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOPC_Billing_Address" runat="server" Text='<%# Eval("OPC_Billing_Address") %>'
                                                                ToolTip="OPC Billing Address" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtOPC_Billing_Address" runat="server" ToolTip="OPC Billing Address" MaxLength="1000"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvOPC_Billing_Address" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtOPC_Billing_Address" ErrorMessage="Enter the OPC Billing Address"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtOPC_Billing_Addresshdr" runat="server" ToolTip="OPC Billing Address" MaxLength="1000" Text='<%# Eval("OPC_Billing_Address") %>'></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvOPC_Billing_Addresshdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtOPC_Billing_Addresshdr" ErrorMessage="Enter the OPC Billing Address"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Delivery Address" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDelivery_Address" runat="server" Text='<%# Eval("Delivery_Address") %>'
                                                                ToolTip="Delivery Address" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtDelivery_Address" runat="server" ToolTip="Delivery Address" MaxLength="300"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvDelivery_Address" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtDelivery_Address" ErrorMessage="Enter the Delivery Address"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtDelivery_Addresshdr" runat="server" ToolTip="Delivery Address" MaxLength="300" Text='<%# Eval("Delivery_Address") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvDelivery_Addresshdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtDelivery_Addresshdr" ErrorMessage="Enter the Delivery Address"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                            <%--<asp:RequiredFieldValidator ID="rfvDelivery_Addresshdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtDelivery_Addresshdr" ErrorMessage="Enter the Delivery Address"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contact Person" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblContact_Person" runat="server" Text='<%# Eval("Contact_Person") %>'
                                                                ToolTip="Contact Person" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtContact_Person" runat="server" ToolTip="Contact Person" MaxLength="30"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvContact_Person" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtContact_Person" ErrorMessage="Enter the Contact Person"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtContact_Personhdr" runat="server" ToolTip="Contact Person" MaxLength="30" Text='<%# Eval("Contact_Person") %>'></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator ID="rfvContact_Personhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtContact_Personhdr" ErrorMessage="Enter the Contact Person"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contact No" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblContact_No" runat="server" Text='<%# Eval("Contact_No") %>'
                                                                ToolTip="Contact No" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtContact_No" runat="server" ToolTip="Contact No" MaxLength="15"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvContact_No" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtContact_No" ErrorMessage="Enter the Contact No"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtContact_Nohdr" runat="server" ToolTip="Contact No" Text='<%# Eval("Contact_No") %>' MaxLength="15"></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator ID="rfvContact_Nohdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtContact_Nohdr" ErrorMessage="Enter the Contact No"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contact Email Id" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblContact_EmailId" runat="server" Text='<%# Eval("Contact_EmailId") %>'
                                                                ToolTip="Contact Email Id" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtContact_EmailId" runat="server" ToolTip="Contact No" MaxLength="60"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtContact_EmailIdhdr" runat="server" ToolTip="Contact Email Id" Text='<%# Eval("Contact_EmailId") %>' MaxLength="60"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'
                                                                ToolTip="Quantity" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtQuantity" runat="server" ToolTip="Quantity"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvQuantity" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtQuantity" ErrorMessage="Enter the Quantity"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <cc1:FilteredTextBoxExtender ID="fteQuantity" runat="server" FilterType="Numbers"
                                                                TargetControlID="txtQuantity">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtQuantityhdr" runat="server" ToolTip="Quantity" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvQuantityhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtQuantityhdr" ErrorMessage="Enter the Quantity"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                            <asp:RequiredFieldValidator ID="rfvQuantityhdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtQuantityhdr" ErrorMessage="Enter the Quantity"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                            <cc1:FilteredTextBoxExtender ID="fteQuantityhdr" runat="server" FilterType="Numbers"
                                                                TargetControlID="txtQuantityhdr">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Per Unit Price" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPer_Unit_Price" runat="server" Text='<%# Eval("Unit_Price") %>'
                                                                ToolTip="Per Unit Price" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtPer_Unit_Price" runat="server" ToolTip="Per Unit Price"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvPer_Unit_Price" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtPer_Unit_Price" ErrorMessage="Enter the Per Unit Price"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtPer_Unit_Pricehdr" runat="server" ToolTip="Per Unit Price" Text='<%# Eval("Unit_Price") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvPer_Unit_Pricehdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtPer_Unit_Pricehdr" ErrorMessage="Enter the Per Unit Price"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                            <%--<asp:RequiredFieldValidator ID="rfvPer_Unit_Pricehdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtPer_Unit_Pricehdr" ErrorMessage="Enter the Per Unit Price"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Bill Amount in FC" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal_Bill_Amount_FC" runat="server" Text='<%# Eval("Bill_Amount_USD") %>'
                                                                ToolTip="Total Bill Amount in FC" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtBill_Amount_FC" runat="server" ToolTip="Total Bill Amount in FC"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvBill_Amount_FC" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBill_Amount_FC" ErrorMessage="Enter the Total Bill Amount in FC"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtBill_Amount_FChdr" runat="server" ToolTip="Total Bill Amount in FC" Text='<%# Eval("Bill_Amount_USD") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvBill_Amount_FChdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBill_Amount_FChdr" ErrorMessage="Enter the Total Bill Amount in FC"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Bill Amount in INR" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal_Bill_Amount_INR" runat="server" Text='<%# Eval("Bill_Amount_INR") %>'
                                                                ToolTip="Total Bill Amount in INR" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtBill_Amount_INR" runat="server" ToolTip="Total Bill Amount in INR"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvBill_Amount_INR" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBill_Amount_INR" ErrorMessage="Enter the Total Bill Amount in INR"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtBill_Amount_INRhdr" runat="server" ToolTip="Total Bill Amount in INR" Text='<%# Eval("Bill_Amount_INR") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvBill_Amount_INRhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBill_Amount_INRhdr" ErrorMessage="Enter the Total Bill Amount in INR"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                            <%--<asp:RequiredFieldValidator ID="rfvBill_Amount_INRhdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBill_Amount_INRhdr" ErrorMessage="Enter the Total Bill Amount in INR"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Inv Amt(Excl Tax)-Material" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBase_Inv_Amt_Mat" runat="server" Text='<%# Eval("Inv_Amt_Material") %>'
                                                                ToolTip="Base Inv Amt(Excl Tax) - Material" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtBase_Inv_Amt_Mat" runat="server" ToolTip="Base Inv Amt(Excl Tax) - Material"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvBase_Inv_Amt_Mat" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBase_Inv_Amt_Mat" ErrorMessage="Enter the Base Inv Amt(Excl Tax) - Material"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtBase_Inv_Amt_Mathdr" runat="server" ToolTip="Base Inv Amt(Excl Tax) - Material" Text='<%# Eval("Inv_Amt_Material") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvBase_Inv_Amt_Mathdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBase_Inv_Amt_Mathdr" ErrorMessage="Enter the Base Inv Amt(Excl Tax) - Material"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Base Inv Amt(Excl Tax)-Labour" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBase_Inv_Amt_Lab" runat="server" Text='<%# Eval("Inv_Amt_Labour") %>'
                                                                ToolTip="Base Inv Amt(Excl Tax) - Labour" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtBase_Inv_Amt_Lab" runat="server" ToolTip="Base Inv Amt(Excl Tax) - Labour"></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator ID="rfvBase_Inv_Amt_Lab" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBase_Inv_Amt_Lab" ErrorMessage="Enter the Base Inv Amt(Excl Tax) - Labour"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtBase_Inv_Amt_Labhdr" runat="server" ToolTip="Base Inv Amt(Excl Tax) - Labour" Text='<%# Eval("Inv_Amt_Labour") %>'></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator ID="rfvBase_Inv_Amt_Labhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtBase_Inv_Amt_Labhdr" ErrorMessage="Enter the Base Inv Amt(Excl Tax) - Labour"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
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
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="txtHSNCodeFT" runat="server" ToolTip="HSN Code" ServiceMethod="GetHSNCode" />
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="txtSACCodeFT" runat="server" ToolTip="SAC Code" ServiceMethod="GetSACCode" />
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtHSNSGSTFT" runat="server" ToolTip="HSN SGST"></asp:TextBox>
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtHSNCGSTFT" runat="server" ToolTip="HSN CGST"></asp:TextBox>
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtHSNIGSTFT" runat="server" ToolTip="HSN IGST"></asp:TextBox>
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtSACSGSTFT" runat="server" ToolTip="SAC SGST"></asp:TextBox>
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtSACCGSTFT" runat="server" ToolTip="SAC CGST"></asp:TextBox>
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtSACIGSTFT" runat="server" ToolTip="SAC IGST"></asp:TextBox>
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="txtBillingStateFT" runat="server" AutoPostBack="true" ToolTip="Billing State" ServiceMethod="GetState" />
                                                        </FooterTemplate>
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
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="txtBillingBranchFT" runat="server" ToolTip="Billing Branch" ServiceMethod="GetBranch" />
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <uc2:Suggest ID="txtBillingBranchIT" SelectedText='<%# Eval("BillingBranch") %>' SelectedValue='<%# Eval("BillingBranchID") %>'
                                                                runat="server" ToolTip="Billing Branch" ServiceMethod="GetBranchIT" />
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <%--GST End--%>
                                                    <asp:TemplateField HeaderText="VAT" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVAT" runat="server" Text='<%# Eval("VAT") %>'
                                                                ToolTip="VAT" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtVAT" runat="server" ToolTip="VAT"></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator ID="rfvVAT" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtVAT" ErrorMessage="Enter the VAT"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtVAThdr" runat="server" ToolTip="VAT" Text='<%# Eval("VAT") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvVAThdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtVAThdr" ErrorMessage="Enter the VAT"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CST" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCST" runat="server" Text='<%# Eval("CST") %>'
                                                                ToolTip="CST" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtCST" runat="server" ToolTip="CST"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvCST" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtCST" ErrorMessage="Enter the CST"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtCSThdr" runat="server" ToolTip="CST" Text='<%# Eval("CST") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvCSThdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtCSThdr" ErrorMessage="Enter the CST"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Excise Duty/CVD (Incl Cess)" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblExcise_Duty" runat="server" Text='<%# Eval("Excise_Duty_CVD") %>'
                                                                ToolTip="Excise Duty/CVD (Incl Cess)" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtExcise_Duty" runat="server" ToolTip="Excise Duty/CVD (Incl Cess)"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvExcise_Duty" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtExcise_Duty" ErrorMessage="Enter the Excise Duty/CVD (Incl Cess)"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtExcise_Dutyhdr" runat="server" ToolTip="Excise Duty/CVD (Incl Cess)" Text='<%# Eval("Excise_Duty_CVD") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvExcise_Dutyhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtExcise_Dutyhdr" ErrorMessage="Enter the Excise Duty/CVD (Incl Cess)"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service Tax Amt (If applicable)" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblService_Tax" runat="server" Text='<%# Eval("Service_Tax_Amt") %>'
                                                                ToolTip="Service Tax Amt (If applicable)" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtService_Tax" runat="server" ToolTip="Service Tax Amt"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvService_Tax" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtService_Tax" ErrorMessage="Enter the Service Tax Amt"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtService_Taxhdr" runat="server" ToolTip="Service Tax Amt" Text='<%# Eval("Service_Tax_Amt") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvService_Taxhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtService_Taxhdr" ErrorMessage="Enter the Service Tax Amt"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Others (Freight/ Octroi, etc)" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOthers" runat="server" Text='<%# Eval("Other_Bill_Component") %>'
                                                                ToolTip="Others (Freight/ Octroi, etc)" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtOthers" runat="server" ToolTip="Others (Freight/ Octroi, etc)"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvOthers" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtOthers" ErrorMessage="Enter the Others (Freight/ Octroi, etc)"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtOthershdr" runat="server" ToolTip="Others (Freight/ Octroi, etc)" Text='<%# Eval("Other_Bill_Component") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvOthershdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtOthershdr" ErrorMessage="Enter the Others (Freight/ Octroi, etc)"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Bill Amount" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal_Bill_Amount" runat="server" Text='<%# Eval("Total_Bill_Amount") %>'
                                                                ToolTip="Total Bill Amount" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtTotal_Bill_Amount" runat="server" ToolTip="Total Bill Amount"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvTotal_Bill_Amount" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtTotal_Bill_Amount" ErrorMessage="Enter the Total Bill Amount"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtTotal_Bill_Amounthdr" runat="server" ToolTip="Total Bill Amount" Text='<%# Eval("Total_Bill_Amount") %>'></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvTotal_Bill_Amounthdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtTotal_Bill_Amounthdr" ErrorMessage="Enter the Total Bill Amount"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="txtTotal_Bill_Amounthdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtTotal_Bill_Amounthdr" ErrorMessage="Enter the Total Bill Amount"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Retention" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRetention" runat="server" Text='<%# Eval("Retention") %>'
                                                                ToolTip="Retention" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtRetention" runat="server" ToolTip="Retention"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvRetention" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtRetention" ErrorMessage="Enter the Retention"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtRetentionhdr" runat="server" ToolTip="Retention" Text='<%# Eval("Retention") %>'></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvRetentionhdr" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtRetentionhdr" ErrorMessage="Enter the Retention"
                                                                ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>--%>
                                                            <%--<asp:RequiredFieldValidator ID="rfvRetentionhdr2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                                runat="server" ControlToValidate="txtRetentionhdr" ErrorMessage="Enter the Retention"
                                                                ValidationGroup="Update" Display="None"></asp:RequiredFieldValidator>--%>
                                                        </EditItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                                ToolTip="Edit">
                                                            </asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                               <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" CausesValidation="false"
                                                                   ToolTip="Delete">
                                                               </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnAdd" Width="50px" runat="server" CommandName="AddNew" ValidationGroup="Footer"
                                                                CssClass="styleSubmitShortButton" Text="Add"></asp:Button>
                                                        </FooterTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                                ValidationGroup="Update" ToolTip="Update"></asp:LinkButton>&nbsp;|
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
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel GroupingText="Terms and Conditions" ID="pnlOthers" runat="server" CssClass="stylePanel">
                            <table width="99%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPaymentTerms" runat="server" Text="Payment Terms" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtPaymentTerms" runat="server" ToolTip="Payment Terms" MaxLength="1000"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPaymentTerms" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txtPaymentTerms" ErrorMessage="Enter the Payment Terms"
                                            ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDeliveryTerms" runat="server" Text="Delivery Terms" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDeliveryTerms" runat="server" ToolTip="Delivery Terms" MaxLength="250"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvDeliveryTerms" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txtDeliveryTerms" ErrorMessage="Enter the Delivery Terms"
                                            ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblWarrantyTerms" runat="server" Text="Warranty Terms" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtWarrantyTerms" runat="server" ToolTip="Warranty Terms" MaxLength="1000"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvWarrantyTerms" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txtWarrantyTerms" ErrorMessage="Enter the Warranty Terms"
                                            ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNotes1" runat="server" Text="Notes 1" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtNotes1" runat="server" ToolTip="Notes 1" MaxLength="1000"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvNotes1" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txtNotes1" ErrorMessage="Enter the Notes 1"
                                            ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNotes2" runat="server" Text="Notes 2" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtNotes2" runat="server" ToolTip="Notes 2" MaxLength="500"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvNotes2" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txtNotes2" ErrorMessage="Enter the Notes 2"
                                            ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblOthers" runat="server" Text="Others" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtOthers1" runat="server" ToolTip="Others" MaxLength="1000"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="rfvOthers" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txtOthers1" ErrorMessage="Enter the Others"
                                            ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td><table width="100%"><tr>
                    <td style="width:40%">
                        <asp:Panel GroupingText="Print Details" ID="pnlPrintDetails" runat="server" CssClass="stylePanel">
                            <table width="99%">
                                <tr>
                                    <td style="width: 20%" class="styleFieldLabel">
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
                        <td style="width:60%">
                    <asp:Panel GroupingText="Cancellation" Visible="false" ID="pnlCancellationQry" runat="server" CssClass="stylePanel">
                            <table width="99%">
                                <tr>
                                    <td style="width: 25%" class="styleFieldLabel">
                                        <asp:Label ID="Label1" runat="server" Text="Reason for Cancellation" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td style="width: 75%">
                                        <asp:TextBox ID="txtCanReasonQry" runat="server" Width="90%" TextMode="MultiLine" ToolTip="Reason for Cancellation" MaxLength="500"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="rfvCancReason" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txtCancReason" ErrorMessage="Enter Reason for Cancellation"
                                            ValidationGroup="Cancellation" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                        </tr></table></td>
                </tr>
                <tr>
                    <td id="Td1" runat="server" align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save" Enabled="false"
                            OnClick="btnSave_Click" ValidationGroup="Save" OnClientClick="return fnCheckPageValidators('Save')" />
                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                            CausesValidation="False" OnClick="btnClear_Click" />
                        <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
                            Enabled="True" TargetControlID="btnClear">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel" OnClick="btnCancel_Click" ToolTip="Cancel" />
                        <asp:Button runat="server" ID="btnDLGeneration" CssClass="styleSubmitButton" Text="DI/LPO Generation"
                            CausesValidation="true" OnClick="btnDLGeneration_Click" ToolTip="DI/LPO Generation" Visible="false" />
                        <asp:Button runat="server" ID="btnEmail" CssClass="styleSubmitButton" Text="EMail" Visible="false"
                            CausesValidation="False" OnClick="btnEmail_Click" ToolTip="EMail" Enabled="false" />
                        <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print"
                            CausesValidation="False" OnClick="btnPrint_Click" ToolTip="Print" Enabled="false"/>
                        <asp:Button runat="server" ID="btnDICancel" CssClass="styleSubmitButton" Text="Cancel PO"/>
                    </td>
                </tr>
                <tr><td>
                <table width="100%">
                                        <tr width="100%">
                                            <td width="100%">
                                                <asp:Panel DefaultButton="btnSubmit" ID="PnlCancellation" Style="display: none" runat="server"
                                                    Height="27%" BackColor="White" BorderStyle="Solid" BorderColor="Black" Width="45%">
                                                    <table width="100%">
                                                        <tr width="100%">
                                                            <td colspan="3" class="stylePageHeading" align="center">
                                                                <asp:Label runat="server"  Text="PO Cancellation" ID="lblPasswordHeader"
                                                                    CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td style="width:20%;margin-left:120px" >
                                                                <asp:Label ID="lblCancReason" ToolTip="Reason for Cancellation" runat="server" CssClass="styleMandatoryLabel" 
                                                                     Text="Reason for Cancellation"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td align="left" style="width:80%" class="styleFieldAlign">
                                                                 <asp:TextBox ID="txtCancReason" runat="server" Width="90%" Height="50px" TextMode="MultiLine" 
                                                                      ToolTip ="Reason for Cancellation" MaxLength="1000"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                  <asp:RequiredFieldValidator ID="rvCancellation" ValidationGroup="Cancellation" runat="server" 
                                                                      ErrorMessage="Enter Reason for Cancellation"
                                                                      ControlToValidate="txtCancReason" CssClass="styleLoginLabel" Display="None"></asp:RequiredFieldValidator>
                                                                  </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="3">
                                                                <asp:Button ID="btnSubmit" ToolTip="Save Cancellation"  ValidationGroup="Cancellation" CausesValidation="true"
                                                                    CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('Cancellation','false')" 
                                                                     OnClick="btnDICancel_Click" Text="Submit" runat="server" />
                                                                &nbsp;
                                                                <asp:Button ID="btnCancelPopup" ToolTip="Cancel" CausesValidation="false" OnClick="btnCancelPopup_Click"
                                                                    CssClass="styleSubmitButton" Text="Cancel" runat="server" />

                                                                <asp:ValidationSummary ID="vsCancellation" runat="server" CssClass="styleMandatoryLabel"
                                                                    HeaderText="Correct the following validation(s):  " ValidationGroup="Cancellation" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="3">
                                                                <asp:Label ID="lblErrorMessagePass" runat="server" CssClass="styleMandatoryLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <cc1:ModalPopupExtender ID="ModalPopupExtenderPassword" runat="server" TargetControlID="btnDICancel"
                                                    PopupControlID="PnlCancellation" BackgroundCssClass="styleModalBackground" DynamicServicePath=""
                                                    Enabled="True">
                                                </cc1:ModalPopupExtender>
                                            </td>
                                        </tr>
                    </table></td></tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="rowindex" runat="server" />
                        <asp:ValidationSummary ID="vsDelivery" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Save" />

                        <asp:ValidationSummary ID="vsFooter" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Footer" />

                        <asp:ValidationSummary ID="vsgridupdate" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Update" />

                        <asp:CustomValidator ID="cvDelivery" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                        <asp:Label ID="lblCustID" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: medium"></asp:Label>
                        <asp:TextBox ID="txtSan" runat="server" Visible="false"></asp:TextBox>
                        <asp:Label ID="lblCompanyName" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCCity" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCZipcode" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblIsPrint" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblStatus" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVAddress1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVAddress2" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVCity" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVPincode" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVState" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblVCountry" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuAddress1" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuAddress2" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuCity" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuPincode" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuState" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblCuCountry" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <%--added for print--%>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        function fnConfirmCancelDI() {

            if (confirm('Do you want to cancel PO?')) {
                return true;
            }
            else
                return false;

        }

        function fnConfirmSavePO() {

            if (confirm('Do you want to save PO?')) {
                return true;
            }
            else
                return false;

        }

        function fnBlur() {
            debugger;
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
