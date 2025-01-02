<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/LegalRepossession/S3GLRGarage_Maint_Add.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="LegalRepossession_S3GLRGarage_Maint_Add"
    EnableEventValidation="false" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" style="width: 990px">
                        <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" GroupingText="Garage Maintenance">
                            <table width="100%">
                                <asp:UpdatePanel ID="upContact" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <tr>
                                            <td class="styleFieldAlign">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="50%" align="left" valign="top">
                                                            <asp:Panel ID="pnlGarageMaint" runat="server" CssClass="stylePanel" GroupingText="Input Criteria">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblGarageOwnerCode" runat="server" CssClass="styleReqFieldLabel" Text="Garage Owner Code"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlGarageOwnerCode" runat="server" AutoPostBack="True" ToolTip="Garage Owner Code"
                                                                                OnSelectedIndexChanged="ddlGarageOwnerCode_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="RqvddlGarageOwnerCode" runat="server" ControlToValidate="ddlGarageOwnerCode"
                                                                                Display="None" ErrorMessage="Select a Garage Owner Code" InitialValue="0" SetFocusOnError="True"
                                                                                ValidationGroup="submit"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblGrgAddress1" runat="server" CssClass="styleDisplayLabel" Text="Address1"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="TxtGrgAddress1" TextMode="MultiLine" ReadOnly="true" runat="server"
                                                                                ToolTip="Address 1" Width="200px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select Line of Business"
                                                                                ValidationGroup="submit"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select the Location"
                                                                                ValidationGroup="submit"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label ID="lblMaintDocNo" runat="server" Text="Garage Maintenance Number" CssClass="styleDisplayLabel"
                                                                                Width="130px"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtMaintDocNo" runat="server" ReadOnly="True" Width="130px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label ID="lblGarageMaintDate" runat="server" ToolTip="GarageMaintenanceDate"
                                                                                CssClass="styleDisplayLabel" Text="Garage Maintenance Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtGarageMaintDate" runat="server" ToolTip="GarageMaintenanceDate"
                                                                                ReadOnly="True"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtGarageMaintDate" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                                TargetControlID="txtGarageMaintDate" ValidChars="/-">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="4px">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="50%" align="left" valign="top">
                                                            <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Information"
                                                                CssClass="stylePanel" Width="99%">
                                                                <table width="100%" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" width="35%">
                                                                            <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <uc2:LOV ID="ucCustomerCodeLov" runat="server" strLOV_Code="CMD" />
                                                                            <asp:HiddenField ID="hdnCustomerID" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" width="100%">
                                                                            <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none;" OnClick="btnLoadCustomer_OnClick"
                                                                                Text="Load Customer" CausesValidation="false" />
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                SecondColumnStyle="styleFieldAlign" ShowCustomerName="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel GroupingText="Garage Rental Details" ID="pnlGrgRentlDtls" runat="server"
                                                    CssClass="stylePanel">
                                                    <table border="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblPymntFreq" runat="server" CssClass="styleDisplayLabel" Text="Payment Frequency"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtPymntFreq" ToolTip="Payment Frequency" ReadOnly="true" runat="server"></asp:TextBox>
                                                                <%--<asp:DropDownList ID="ddlPymntFreq" ToolTip="Payment Frequency" runat="server">
                                                        </asp:DropDownList>--%>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblPolNo" runat="server" CssClass="styleDisplayLabel" Text="Policy Number"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtPolicyNo" ToolTip="Policy Number" runat="server" ReadOnly="true">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCoverprk" runat="server" Text="Covered Parking"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:RadioButtonList ID="RdbCvrPrk" runat="server" ToolTip="Covered Parking" Enabled="false"
                                                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                                    <asp:ListItem Value="True">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblSqFeet" runat="server" CssClass="styleDisplayLabel" Text="Square Feet"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="TxtSqFeet" ToolTip="Square Feet" runat="server" Style="text-align: right"
                                                                    ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="LblPolicyAmnt" runat="server" CssClass="styleDisplayLabel" Text="Policy Amount"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="TxtPlcyAmnt" ToolTip="Policy Amount" Style="text-align: right" runat="server"
                                                                    ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblRent" runat="server" CssClass="styleDisplayLabel" Text="Rent Per Sq.Feet"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="TxtRntSqFeet" ToolTip="Rent Per Sq.Feet" runat="server" Style="text-align: right"
                                                                    ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblGrgCapcity" runat="server" CssClass="styleDisplayLabel" Text="Garage Capacity"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="TxtGrgCpcty" ToolTip="Garage Capacity" runat="server" Style="text-align: right"
                                                                    ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblTotRent" runat="server" CssClass="styleDisplayLabel" Text="Total Rent"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="TxtTotRent" runat="server" ToolTip="Total Rent" ContentEditable="false"
                                                                    Style="text-align: right"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblInscompany" runat="server" CssClass="styleDisplayLabel" Text="Insurance Company"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtInscompany" ToolTip="Insurance Company" runat="server" ReadOnly="true"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlgarage" runat="server" CssClass="stylePanel" GroupingText="Garage Details">
                                                    <asp:Panel ID="pnlInner" runat="server" CssClass="stylePanel" ScrollBars="Horizontal">
                                                        <asp:GridView ID="gvGarageMaint" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvGarageMaint_RowDataBound"
                                                            OnRowDeleting="gvGarageMaint_RowDeleting" ShowFooter="true" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Prime Account Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPANUM" Text='<%#Bind("PANUM") %>' runat="server" ToolTip="Prime Account Number" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlPANUM" runat="server" ToolTip="Prime Account Number" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlPANUM_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvPANUM" runat="server" ControlToValidate="ddlPANUM"
                                                                            Display="None" ErrorMessage="Select Prime Account Number" InitialValue="0" ValidationGroup="Add">
                                                                        </asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sub Account Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSANUM" Text='<%#Bind("SANUM") %>' runat="server" ToolTip="Sub Account Number" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlSANUM" runat="server" ToolTip="Sub Account Number" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="ddlSANUM_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvSANUM" runat="server" ControlToValidate="ddlPANUM"
                                                                            Display="None" ErrorMessage="Select Sub Account Number" InitialValue="0" ValidationGroup="Add">
                                                                        </asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Number" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssetNo" Text='<%#Bind("AssetNumber") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssetId" Text='<%#Bind("AssetID") %>' runat="server" ToolTip="Asset Description"
                                                                            Visible="false" />
                                                                        <asp:Label ID="lblAssetDesc" Text='<%#Bind("AssetDescription") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAssetDescription" runat="server" ToolTip="Asset Description"
                                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlAssetDescription_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvAssetDescription" runat="server" ControlToValidate="ddlAssetDescription"
                                                                            Display="None" ErrorMessage="Select Asset Description" InitialValue="0" ValidationGroup="Add">
                                                                        </asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Regn No or Serial No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblMachineNo" Text='<%#Bind("MachineNo") %>' runat="server" ToolTip="Regn No or Serial No" />
                                                                        <asp:Label ID="lblAssetNumber" Text='<%#Bind("AssetNumber") %>' runat="server" ToolTip="Regn No or Serial No"
                                                                            Visible="false" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtRegNum" runat="server" MaxLength="16" Style="text-align: left"
                                                                            ToolTip="Registration Number" Width="80px" ReadOnly="true"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="In Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInDate" Text='<%# GVDateFormat(Eval("InDate").ToString()) %>' runat="server"
                                                                            ToolTip="In Date" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtInDate" runat="server" ToolTip="InDate" Width="80px"></asp:TextBox>
                                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select In Date"
                                                                            Visible="false" />
                                                                        <cc1:CalendarExtender ID="CEInDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                            PopupButtonID="Image1" TargetControlID="txtInDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtInDate" runat="server" Enabled="false" ControlToValidate="txtInDate"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter In Date"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Out Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOutDate" Text='<%# GVDateFormat(Eval("OutDate").ToString()) %>'
                                                                            runat="server" />
                                                                        <asp:TextBox ID="txtOutDate" runat="server" Visible="false" MaxLength="8" Width="80px"
                                                                            Text='<%# GVDateFormat(Eval("OutDate").ToString()) %>' ToolTip="OutDate"></asp:TextBox>
                                                                        <asp:Image ID="imgOutDateEdit" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Out Date"
                                                                            Visible="false" />
                                                                        <cc1:CalendarExtender ID="calOutDateEdit" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                            PopupButtonID="imgOutDateEdit" TargetControlID="txtOutDate">
                                                                        </cc1:CalendarExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtOutDate" runat="server" ToolTip="Out Date" Width="80px"></asp:TextBox>
                                                                        <asp:Image ID="imgOutDate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Out Date"
                                                                            Visible="false" />
                                                                        <cc1:CalendarExtender ID="calOutDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                            PopupButtonID="imgOutDate" TargetControlID="txtOutDate">
                                                                        </cc1:CalendarExtender>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Area Required">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAreaReq" Text='<%#Bind("Area") %>' runat="server" />
                                                                        <asp:TextBox ID="txtAreaReq" runat="server" Visible="false" Style="padding-left: 5px"
                                                                            Text='<%# Bind("Area") %>' ToolTip="AreaRequired" Width="80px"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtAreaReq" runat="server" FilterType="Numbers,Custom,UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtAreaReq" ValidChars=".">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtAreaReq" runat="server" ControlToValidate="txtAreaReq"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Area Required"></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAreaReq" runat="server" MaxLength="16" Style="text-align: left"
                                                                            ToolTip="Area Required" Width="80px"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtAreaReq" runat="server" FilterType="Numbers,Custom,UppercaseLetters, LowercaseLetters"
                                                                            TargetControlID="txtAreaReq" ValidChars=".">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtAreaReq" runat="server" ControlToValidate="txtAreaReq"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Area Required"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rental Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRental" Text='<%#Bind("Rental") %>' Style="text-align: right;"
                                                                            runat="server" ToolTip="RentalAmount" Width="80px" />
                                                                        <asp:TextBox ID="txtRental" Visible="false" runat="server" ToolTip="Rental Amount"
                                                                            Style="text-align: right" Width="80px" Text='<%#Bind("Rental") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtRental" runat="server" ToolTip="Rental Amount" Style="text-align: right"
                                                                            Width="80px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtRental" runat="server" ControlToValidate="txtRental"
                                                                            CssClass="styleMandatoryLabel" Display="None" Enabled="false" ValidationGroup="Add"
                                                                            SetFocusOnError="True" ErrorMessage="Enter Rental Amount"></asp:RequiredFieldValidator>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtRental" Enabled="false" runat="server" FilterType="Custom,Numbers"
                                                                            ValidChars="." TargetControlID="txtRental">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Debit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCust" Text='<%#Bind("Cust") %>' runat="server" Style="text-align: right" />
                                                                        <asp:TextBox ID="txtCust" runat="server" Visible="false" Style="padding-left: 5px"
                                                                            Text='<%# Bind("Cust") %>' ToolTip="CustomerDebit" Width="80px"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtCust" runat="server" FilterType="Custom,Numbers"
                                                                            ValidChars="." TargetControlID="txtCust">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtCust" runat="server" ControlToValidate="txtCust"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Customer Debit"></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtCust" runat="server" ToolTip="CustomerDebit" Width="80px" Style="text-align: right">
                                                                        </asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtCust" runat="server" ControlToValidate="txtCust"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Add" SetFocusOnError="True"
                                                                            ErrorMessage="Enter Customer Debit"></asp:RequiredFieldValidator>
                                                                        <cc1:FilteredTextBoxExtender ID="ftxtCust" runat="server" FilterType="Custom,Numbers"
                                                                            ValidChars="." TargetControlID="txtCust">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:RadioButton ID="rbnEdit" runat="server" onclick="fnDeselectAllOthers(this);"
                                                                            AutoPostBack="true" OnCheckedChanged="rbnEdit_CheckedChanged" />
                                                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                                                            runat="server" Visible="false" OnClick="imgbtnEdit_Click" />
                                                                        <asp:ImageButton ID="imgbtnDelete" ImageUrl="~/Images/delete.png" runat="server"
                                                                            CommandName="Delete" />
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="true" ValidationGroup="Add"
                                                                            CommandName="Add" Text="Add" OnClick="btnAddPolicy_OnClick" />
                                                                        <asp:Button ID="btnAddPolicy" runat="server" CausesValidation="true" CssClass="styleSubmitShortButton"
                                                                            Text="Add" ValidationGroup="Add" Visible="false" />
                                                                    </FooterTemplate>
                                                                    <ItemStyle Width="40px" />
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <%--</div>--%></asp:Panel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="width: 990px">
                        <br />
                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            ToolTip="Save" ValidationGroup="submit" OnClientClick="return fnCheckPageValidators('submit', 'true');"
                            Text="Save" />
                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            ToolTip="Clear" Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            ToolTip="Cancel" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 990px">
                        <%--  <asp:ValidationSummary ID="vsGarageMaster" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                    ShowMessageBox="false" HeaderText="Correct the following validation(s):" DisplayMode="List" />--%>
                        <asp:ValidationSummary ID="vsGarageMaster" runat="server" ShowSummary="true" ValidationGroup="Add"
                            CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):" />
                        <asp:ValidationSummary ID="vsMaintenance" runat="server" ShowSummary="true" ValidationGroup="submit"
                            CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):" />
                        <%--  <asp:ValidationSummary ID="vs1" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                    ShowMessageBox="false" HeaderText="Correct the following validation(s):" DisplayMode="List"  ValidationGroup="Garage Details"/>--%>
                        <asp:CustomValidator ID="cvGarageMaster" runat="server" CssClass="styleMandatoryLabel" Display="None"
                            Enabled="true" ValidationGroup="custom" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true" 
                            ValidationGroup="custom" CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):" />
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="txthiddenfield" runat="server" Visible="false"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_btnLoadCustomer').click();
        }

        function fnDeselectAllOthers(chkBox) {
            var gv = document.getElementById("ctl00_ContentPlaceHolder1_gvGarageMaint");

            for (var i = 0; i < gv.all.length; i++) {
                var node = gv.all[i];
                if (node != null && node.type == "radio" && node.checked && node != chkBox) {
                    node.checked = false;
                }
            }


        }
        function FnCalcRentaltxt(inputAreaReq, inputRentPer, inputRentAmt, inputCustDt) {
            var AreaReq = document.getElementById(inputAreaReq).value;
            var RentPer = document.getElementById(inputRentPer).value;

            document.getElementById(inputRentAmt).value = document.getElementById(inputCustDt).value = parseFloat(AreaReq) * parseFloat(RentPer);

        }

        function FnCalcRentallbl(inputAreaReq, inputRentPer, inputRentAmt, inputCustDt) {
            var AreaReq = document.getElementById(inputAreaReq).value;
            var RentPer = document.getElementById(inputRentPer).value;

            document.getElementById(inputCustDt).value = parseFloat(AreaReq) * parseFloat(RentPer);
            document.getElementById(inputRentAmt).value = parseFloat(AreaReq) * parseFloat(RentPer);
        }   
    </script>

</asp:Content>
