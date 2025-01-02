<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLoanAdAssetAcquisition_Add.aspx.cs"
    Inherits="S3GLoanAdAssetAcquisition" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput)
        {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function fnGetCompensationValue(CompensationValue)
        {
            var exp = /_/gi;
            return parseFloat(CompensationValue.value.replace(exp, '0'));
        }                
     
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" align="left">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td class="styleFieldLabel" colspan="2">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td colspan="4" valign="top">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <div style="height: 1px;">
                                                                            <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="None"
                                                                                ControlToValidate="ddlLineofBusiness" ValidationGroup="btnSave" InitialValue="0"
                                                                                ErrorMessage="Select a Line of Business" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <br />
                                                                       <%--     <asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" ControlToValidate="ddlBranch"
                                                                                ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select a Location" CssClass="styleMandatoryLabel"
                                                                                SetFocusOnError="True">
                                                                            </asp:RequiredFieldValidator>--%>
                                                                            <br />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlLineofBusiness" runat="server" 
                                                                ToolTip="Line of Business"  >
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                          <%--  <asp:DropDownList ID="ddlBranch" runat="server" ToolTip="Location" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                            </asp:DropDownList>--%>
                                                             <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ValidationGroup="btnSave" ServiceMethod="GetBranchList"
                                                    AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select a Location"
                                                    IsMandatory="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" rowspan="2">
                                                            <asp:Label ID="lblLAAEDate" runat="server" Text="LAAE Date"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" rowspan="2">
                                                            <asp:TextBox ID="txtLAAEDate" runat="server" Width="60%" ToolTip="LAAE Date"></asp:TextBox>
                                                            <asp:Image ID="imgCalRegNoValDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            <cc1:CalendarExtender ID="cexDate" runat="server" TargetControlID="txtLAAEDate" PopupButtonID="imgCalRegNoValDate"
                                                                Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                        <td class="styleFieldLabel" width="22%">
                                                            <asp:Label ID="lblAssetAcquisitionNO" runat="server" Text="Asset Acquisition Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtAssetAcquisitionNO" runat="server" ReadOnly="true" ToolTip="Asset Acquisition Number">
                                                            </asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblDOGenerated" runat="server" Text="Invoice Based" Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:CheckBox ID="chkDOGenerated" runat="server" Visible="false" AutoPostBack="true" Checked="true"
                                                                OnCheckedChanged="chkDOGenerated_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblClosureMonth" runat="server" Text="Closure Month" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMonthLock" runat="server" Text="Active" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4">
                                                            <%--<asp:UpdatePanel ID="upAcq" runat="server">
                                                            
                                                                <ContentTemplate>--%>
                                                            <cc1:TabContainer ID="AssetAcquisition" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                                                Width="99%" ScrollBars="Auto" TabStripPlacement="top" AutoPostBack="false">
                                                                <cc1:TabPanel ID="AssetAcquisition_Create" runat="server" HeaderText="General">
                                                                    <HeaderTemplate>
                                                                        General
                                                                    </HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:UpdatePanel ID="UpdatePanel_AssetAcquisition" runat="server">
                                                                            <ContentTemplate>
                                                                                <div style="height: 235px; overflow-x: hidden; overflow-y: auto; width: 100%">
                                                                                    <div style="height: 235px;">
                                                                                        <asp:GridView ID="gvAssetAcquisition" runat="server" AutoGenerateColumns="false"
                                                                                            ShowFooter="true" OnRowDataBound="gvAssetAcquisition_RowDataBound" Width="100%"
                                                                                            OnRowCommand="gvAssetAcquisition_RowCommand" OnRowDeleting="gvAssetAcquisition_RowDeleting">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Serial No." ItemStyle-Width="100px">
                                                                                                    <ItemTemplate>
                                                                                                        <%--<asp:Label ID="lblAssetSerialNo" runat="server" Text='<%#Bind("Asset_Serial_Number") %>' Width="70px"></asp:Label>--%>
                                                                                                        <asp:Label ID="lblAssetSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'
                                                                                                            Width="20px" ToolTip="Serial No."></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Asset Type">
                                                                                                    <HeaderTemplate>
                                                                                                        <asp:Label ID="lblAssetType" runat="server" Text="Asset Type"></asp:Label>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetType" runat="server" Text='<%#Bind("Asset_Type") %>' ToolTip="Asset Type"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:DropDownList ID="ddlAssetType" runat="server" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged"
                                                                                                            AutoPostBack="true" ToolTip="Asset Type">
                                                                                                        </asp:DropDownList>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle HorizontalAlign="Center" Width="25%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Asset Code" ItemStyle-Width="25%">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetCode" runat="server" Text='<%#Bind("Asset_Code") %>' ToolTip="Asset Code"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:DropDownList ID="ddlAssetCode" runat="server" OnSelectedIndexChanged="ddlAssetCode_SelectedIndexChanged"
                                                                                                            AutoPostBack="true" Width="100%" ToolTip="Asset Code">
                                                                                                        </asp:DropDownList>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle HorizontalAlign="Center" Width="25%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Asset Description" ItemStyle-Width="40%">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetDescription" runat="server" Text='<%#Bind("Asset_Description") %>'
                                                                                                            ToolTip="Asset Description"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:DropDownList ID="ddlAssetDescription" runat="server" Visible="false" Width="0px"
                                                                                                            ToolTip="Asset Description">
                                                                                                        </asp:DropDownList>
                                                                                                        <asp:TextBox ID="txtAssetDescription" runat="server" ReadOnly="true" Width="99%"
                                                                                                            TabIndex="-1" ToolTip="Asset Description"></asp:TextBox>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle Width="50%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Add | Delete" ItemStyle-Width="75px" HeaderStyle-Width="75px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="lnkRemove" runat="server" Text="Delete" CommandName="Delete"
                                                                                                            ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                                                            CausesValidation="false"></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" CommandName="Add" CausesValidation="false"
                                                                                                            ToolTip="Add"></asp:LinkButton>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle Width="75px" />
                                                                                                </asp:TemplateField>
                                                                                                <%--<asp:TemplateField HeaderText="Entity Code">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEntityCode" runat="server" Text='<%#Bind("Entity_Code") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="5%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Invoice No">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Bind("Invoice_No") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="5%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Invoice Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#Bind("Invoice_Date") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="5%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="AINS No">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAINSNo" runat="server" Text='<%#Bind("AINS_No") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="5%" />
                                                                                    </asp:TemplateField>
                                                                                   --%>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                        <asp:GridView ID="gvAssetAcquisition_View" runat="server" AutoGenerateColumns="false"
                                                                                            Width="100%" ShowFooter="false" OnRowDataBound="gvAssetAcquisition_View_RowDataBound">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Asset Serial No." ItemStyle-Width="50px">
                                                                                                    <ItemTemplate>
                                                                                                        <%--<asp:Label ID="lblAssetSerialNo_View" runat="server" Text='<%#Bind("Asset_Serial_Number") %>' Width="50px"></asp:Label>--%>
                                                                                                        <asp:Label ID="lblAssetSerialNo_View" runat="server" Text='<%#Container.DataItemIndex+1%>'
                                                                                                            Width="50px" ToolTip="Asset Serial No."></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                    </FooterTemplate>
                                                                                                    <ItemStyle Width="50px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Asset Type">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetType" runat="server" Text='<%#Bind("Asset_Type") %>' ToolTip="Asset Type"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Asset Code" ItemStyle-Width="100px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetCode_View" runat="server" Text='<%#Bind("Asset_Code") %>'
                                                                                                            Width="100px" ToolTip="Asset Code"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Asset Description">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetDescription_View" runat="server" Text='<%#Bind("Asset_Description") %>'
                                                                                                            ToolTip="Asset Description"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Asset Value" ItemStyle-Width="70px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetValue_View" runat="server" Text='<%#Bind("Asset_Value") %>'
                                                                                                            Width="70px" ToolTip="Asset Value"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="70px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Lease Asset No." ItemStyle-Width="100px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLeaseAssetNo" runat="server" Text='<%#Bind("Lease_Asset_No") %>'
                                                                                                            Width="100px" ToolTip="Lease Asset No."></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Entity Code" ItemStyle-Width="40px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblEntityCode_View" runat="server" Text='<%#Bind("Entity_Code") %>'
                                                                                                            Width="40px" ToolTip="Entity Code"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="40px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Invoice No." ItemStyle-Width="60px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblInvoiceNo_View" runat="server" Text='<%#Bind("Invoice_No") %>'
                                                                                                            Width="60px" ToolTip="Invoice No."></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="60px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="100px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblInvoiceDate_view" runat="server" Text='<%#Bind("Invoice_Date") %>'
                                                                                                            ToolTip="Invoice Date"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="100px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="AINS No." ItemStyle-Width="40px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAINSNo_View" runat="server" Text='<%#Bind("AINS_No") %>' Width="40px"
                                                                                                            ToolTip="AINS No."></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="40px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Policy No." ItemStyle-Width="40px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblPolicyNo_View" runat="server" Text='<%#Bind("Policy_No") %>' Width="40px"
                                                                                                            ToolTip="Policy No."></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="40px" />
                                                                                                </asp:TemplateField>
                                                                                                <%--
                                        <asp:TemplateField HeaderText="Vendor Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtVendorCode" runat="server" Text='<%#Bind("Vendor_Code") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtVendorCode" runat="server" Text='<%#Bind("Vendor_Code") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtVendorCode" runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                      
                                    
                                        <asp:TemplateField HeaderText="Policy No">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPolicyNo" runat="server" Text='<%#Bind("Policy_No") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPolicyNo" runat="server" Text='<%#Bind("Policy_No") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtPolicyNo" runat="server"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>--%>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                            <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="chkDOGenerated" EventName="CheckedChanged" />
                                                                            </Triggers>
                                                                        </asp:UpdatePanel>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                                <cc1:TabPanel ID="AssetAcquisition_Availability" runat="server">
                                                                    <HeaderTemplate>
                                                                        Availability</HeaderTemplate>
                                                                    <ContentTemplate>
                                                                        <asp:UpdatePanel ID="upPanel2" runat="server">
                                                                            <ContentTemplate>
                                                                                <div style="height: 235px; overflow-x: hidden; overflow-y: auto;">
                                                                                    <div style="height: 235px;">
                                                                                        <asp:GridView ID="gvAssetAcquisitionAvailability" runat="server" AutoGenerateColumns="False"
                                                                                            Width="100%" DataKeyNames="Lease_Asset_No" OnRowDataBound="gvAssetAcquisitionAvailability_RowDataBound">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Lease Asset No.">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLeaseAssetNo" runat="server" Text='<%#Bind("Lease_Asset_No") %>'
                                                                                                            ToolTip="Lease Asset No."></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="LAN Booking From">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLANBookingFromDate" runat="server" Text='<%#Bind("LAN_Booking_From_Date") %>'
                                                                                                            Style="padding-left: 10px" ToolTip="LAN Booking From"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="LAN Booking Upto">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLANBookingToDate" runat="server" Text='<%#Bind("LAN_Booking_To_Date") %>'
                                                                                                            ToolTip="LAN Booking Upto"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Current Asset Value">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAssetValue" runat="server" Text='<%#Bind("CurrentAssetValue") %>'
                                                                                                            Style="padding-right: 10px" ToolTip="Current Asset Value"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Availability Status">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLookup_Description" runat="server" Text='<%#Bind("Avalilability_Status") %>'
                                                                                                            ToolTip="Availability Status"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Performing Status">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%#Bind("Status") %>' Visible="false"
                                                                                                            ToolTip="Performing Status"></asp:Label>
                                                                                                        <asp:DropDownList ID="ddlPerformance" runat="server" Visible="false" ToolTip="Performing Status" />
                                                                                                        <asp:Label ID="lblPerforming" runat="server" Text='<%#Bind("Lookup_Description") %>'
                                                                                                            ToolTip="Performing Status"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Depreciation">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDepreciationAmount" runat="server" Text='<%#Bind("Depreciation_Amount") %>'
                                                                                                            ToolTip="Depreciation"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Addition">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblAddition" runat="server" Text='<%#Bind("Addition") %>' ToolTip="Addition"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="CanEdit" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblCanEdit" runat="server" Text='<%#Bind("CanEdit") %>' Style="padding-right: 10px"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </div>
                                                                            </ContentTemplate>
                                                                        </asp:UpdatePanel>
                                                                    </ContentTemplate>
                                                                </cc1:TabPanel>
                                                            </cc1:TabContainer>
                                                            <%-- </ContentTemplate>
                                                            </asp:UpdatePanel>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <br />
                                                            <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                                                ValidationGroup="btnSave" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                                                Text="Save" ToolTip="Save" />
                                                            <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                                                CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" ToolTip="Clear" />
                                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                                                                CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" align="center">
                                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
