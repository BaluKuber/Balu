<%@ Page Title="Asset Serial Number Entry" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanADAssetSerialNoEntry.aspx.cs" Inherits="LoanAdmin_S3GLoanADAssetSerialNoEntry" %>

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
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Asset Serial Number Entry - Create" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Asset Serial Number">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Customer Name" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails" AutoPostBack="true" Width="200px"
                                            ErrorMessage="Select Customer Name" ValidationGroup="Main" IsMandatory="true" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlVendorName" runat="server" ServiceMethod="GetVendorNameDetails" AutoPostBack="true" Width="200px"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlInvoiceNo" runat="server" ServiceMethod="GetInvDetails" Width="200px" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblRSNO" runat="server" Text="Rent Schedule No." CssClass="styleDisplayFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                         <asp:TextBox ID="txtRSNo" Width="200px" runat="server" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="4" align="center">
                                        <asp:Button runat="server" ID="btnGo" CssClass="styleSubmitButton"
                                            Text="Go" OnClick="btnGo_Click" ValidationGroup="Main" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcAssetSerialNoEntry" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            Width="100%" ScrollBars="Auto" TabStripPlacement="top">
                            <cc1:TabPanel ID="tpAssetSerialNoEntry" runat="server" HeaderText="Asset Serial No Entry">
                                <HeaderTemplate>Asset Serial No Entry</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upAssetSerialNoEntry" runat="server">
                                        <ContentTemplate>
                                            <div id="DivForPanelScroll" runat="server" style="overflow: scroll;">
                                                <asp:GridView Width="100%" ID="gvAssetSerialNo" AllowPaging="true" runat="server" AutoGenerateColumns="false"
                                                     OnRowDataBound="gvAssetSerialNo_OnRowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
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
                                                        <asp:TemplateField HeaderText="PA_SA_REF_ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRSID" runat="server" Text='<%#Bind("PA_SA_REF_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="LOCATION_ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLocation" runat="server" Text='<%#Bind("LOCATION") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rental Schedule No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRSNo" runat="server" Text='<%#Bind("PANUM") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Invoice No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Bind("INVOICE_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asset Category">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetCategory" runat="server" Text='<%#Bind("ASSET_CATEGORY") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asset Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetType" runat="server" Text='<%#Bind("ASSET_TYPE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asset Sub Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetSubType" runat="server" Text='<%#Bind("ASSET_SUB_TYPE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Serial Number">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSerialNo" Width="98%" runat="server" Text='<%#Bind("SERIAL_NO") %>' MaxLength="20"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Existing Delivery Address">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExDelAdd" runat="server" Text='<%#Bind("Existing_Address") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Existing State">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExState" runat="server" Text='<%#Bind("LocationCat_Description") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Proposed Delivery Address">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDeliveryAddress" Width="98%" Text='<%#Bind("DELIVERY_ADDRESS") %>' onkeyup="maxlengthfortxt(300);" runat="server" TextMode="MultiLine" MaxLength="300"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Proposed State">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="drpLocation" Width="98%" runat="server"></asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asset_Serial_ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSerialId" runat="server" Text='<%#Bind("Asset_Serial_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asset_Category_ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetCatgID" runat="server" Text='<%#Bind("Asset_Category_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asset_Type_ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetTypeId" runat="server" Text='<%#Bind("Asset_Type_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Asset_Sub_Type_ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetSubTypeId" runat="server" Text='<%#Bind("Asset_Sub_Type_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Invoice_ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceID" runat="server" Text='<%#Bind("Invoice_ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <uc1:PageNavigator ID="ucCustomPaging" Visible="false" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnMove" TabIndex="15" CssClass="styleSubmitButton" OnClick="btnMove_Click"
                            Text="Move"/>
                        <asp:Button runat="server" ID="btnSave" TabIndex="15" CssClass="styleSubmitButton"
                            Text="Save" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" TabIndex="16" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="Main" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                         <asp:ValidationSummary ID="vsFooter" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Footer" />
                         <asp:CustomValidator ID="cvAsset" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>




    <script language="javascript" type="text/javascript">
        function CheckOne(CheckboxClientId) {
            var Checkbox = document.getElementById(CheckboxClientId);

            if (Checkbox != null) {
                var CheckBoxInputs = Checkbox.getElementsByTagName("input");
                for (var i = 0; i < CheckBoxInputs.length; i++) {
                    var CurrentIndex;
                    if (CheckBoxInputs[i].checked) {
                        CurrentIndex = i;
                    }
                    if (CheckBoxInputs[i].checked) {
                        CheckBoxInputs[i].checked = false;
                    }
                    CheckBoxInputs[parseInt(CurrentIndex)].checked = true;
                }
            }
        }

    </script>
</asp:Content>
