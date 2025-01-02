<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3G_ORG_SanctionNumberQuery.aspx.cs" Inherits="Origination_S3G_ORG_SactionNumberQuery"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelSNQ" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <table class="stylePageHeading" cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr width="99%">
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" width="100%" border="0">
                            <tr>
                                <td valign="top">
                                    <cc1:TabContainer ID="TCEnqu" runat="server" ActiveTabIndex="1" CssClass="styleTabPanel"
                                        Width="99%" ScrollBars="None">
                                        <cc1:TabPanel runat="server" Visible="false" ID="TabSanctionQuery" CssClass="tabpan"
                                            Width="99%" BackColor="Red">
                                            <HeaderTemplate>
                                                Sanction Number Query
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" style="width: 80%">
                                                    <tr>
                                                        <td class="styleFieldLabel" width="30%">
                                                            <asp:Label ID="lblOption" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" align="left">
                                                            <asp:DropDownList ID="ddlOptions" runat="server" Width="140px" AutoPostBack="True"
                                                                OnSelectedIndexChanged="ddlOptions_SelectedIndexChanged">
                                                                <asp:ListItem Text="Sanction Number" Value="1" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Date" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSanctionNumber" runat="server">
                                                        <td class="styleFieldLabel" runat="server" width="30%">
                                                            <asp:Label ID="lblSanctionNumbers" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign" align="left" runat="server">
                                                            <asp:DropDownList ID="ddlSanctionNumberbatch" runat="server" Width="140px" Visible="False">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtSanctionNumber" runat="server" Width="164px" onkeypress="fnAllowNumbersOnly(false)"
                                                                MaxLength="17"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvSanctionNumber" runat="server" ControlToValidate="txtSanctionNumber"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Please Enter the Valid Sanction No"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="trStartdate" runat="server" visible="False">
                                                        <td class="styleFieldLabel" runat="server" align="center" width="30%">
                                                            <asp:Label ID="lblStartDate" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign" runat="server" align="left">
                                                            <asp:TextBox ID="txtStartDate" runat="server" Width="90px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtenderSD" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                PopupButtonID="imgStartDate" TargetControlID="txtStartDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Please Enter the Valid Start date"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr id="trEndDate" runat="server" visible="False">
                                                        <td class="styleFieldLabel" runat="server" align="center" width="30%">
                                                            <asp:Label ID="lblEndDate" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign" runat="server" align="left">
                                                            <asp:TextBox ID="txtEndDate" runat="server" Width="90px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtenderED" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                PopupButtonID="imgEndDate" TargetControlID="txtEndDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Please Enter the Valid End date"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellpadding="0" align="center" cellspacing="0" style="width: 60%">
                                                    <tr align="center">
                                                        <td align="center" class="styleFieldAlign">
                                                            <asp:Button ID="btnShowDetails1" runat="server" CssClass="styleSubmitButton" OnClick="btnGo_Click"
                                                                UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label2" runat="server"></asp:Label>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr align="center" width="100%">
                                                        <td colspan="2">
                                                            <asp:GridView ID="gvSanctionNumberQuery" runat="server" AutoGenerateColumns="False"
                                                                Width="98%" OnRowDataBound="gvSanctionNumberQuery_RowDataBound" HeaderStyle-CssClass="styleGridHeader"
                                                                OnSelectedIndexChanged="gvSanctionNumberQuery_SelectedIndexChanged">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                    <asp:BoundField DataField="SanctionNumber" HeaderText="Sanction Number" />
                                                                    <asp:BoundField DataField="SanctionDate" HeaderText="Sanction Date" />
                                                                    <asp:BoundField DataField="EnquiryNumber" HeaderText="Enquiry Number" />
                                                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                                                    <asp:BoundField DataField="LineOfBusiness" HeaderText="Line of Business" />
                                                                    <asp:BoundField DataField="Branch" HeaderText="Location" />
                                                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 52px">
                                                            &nbsp;
                                                        </td>
                                                        <td valign="top">
                                                            <br />
                                                            <br />
                                                            <div id="divId" style="overflow: None; height: 100%; width: 100%">
                                                                <asp:ValidationSummary ID="vsEnquiryAssignment" runat="server" CssClass="styleMandatoryLabel"
                                                                    Width="100%" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CustomValidator ID="cvQuery" runat="server" CssClass="styleMandatoryLabel" Display="None"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" Visible="false" ID="tbEnquAssign" CssClass="tabpan"
                                            BackColor="Red">
                                            <HeaderTemplate>
                                                <%--Credit Parameter Approval Details--%>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                    <asp:Panel ID="pnlEnquirylevel" runat="server" CssClass="stylePanel" Width="99%"
                                        Visible="false" GroupingText="Enquiry Level">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblApprovalFor" CssClass="styleDisplayLabel" Text="Approval For"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtApprovalFor" runat="server" Width="100px" ReadOnly="True" ToolTip="Approved For"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSanctionNumber" runat="server" CssClass="styleDisplayLabel" Text="Sanction Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtSanctionNumberFld" runat="server" Width="170px" ReadOnly="True"
                                                        ToolTip="Sanction Number"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLOB" runat="server" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLOB" runat="server" Style="width: 200px" ReadOnly="True" ToolTip="Line Of Business"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblSanctionDate" CssClass="styleDisplayLabel" Text="Sanction Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtSanctionDate" runat="server" Width="85px" ReadOnly="True" ToolTip="Sanction Date"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBranch" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtBranch" runat="server" Style="width: 200px" ReadOnly="True" ToolTip="Location"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEnquiryBo" runat="server" CssClass="styleDisplayLabel" Text="Enquiry Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEnquiryno" runat="server" Style="width: 170px" ReadOnly="True"
                                                        ToolTip="Enquiry Number"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblProduct" runat="server" CssClass="styleDisplayLabel" Text="Product"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtProduct" runat="server" Style="width: 200px" ReadOnly="True"
                                                        ToolTip="Product"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEnquiryDate" runat="server" CssClass="styleDisplayLabel" Text="Enquiry Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEnquiryDate" runat="server" Style="width: 85px" ReadOnly="True"
                                                        ToolTip="Enquiry Date"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <hr class="styleCheklist" style="height: 0.5" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCustomerCode" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                    &nbsp;
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCustomerCode" runat="server" Style="width: 170px" ReadOnly="True"
                                                        ToolTip="Customer Code"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAsset" runat="server" CssClass="styleDisplayLabel" Text="Asset Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAssetName" runat="server" Style="width: 220px" ReadOnly="True"
                                                        ToolTip="Asset Name"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCustomerName" runat="server" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCustomerName" runat="server" Style="width: 200px" ReadOnly="True"
                                                        ToolTip="Customer Name"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAssetValue" runat="server" CssClass="styleDisplayLabel" Text="Asset Value"
                                                        Style="text-align: right"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAssetValue" runat="server" Style="width: 85px; text-align: right"
                                                        ReadOnly="True" ToolTip="Asset Value"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblOfferCard" runat="server" CssClass="styleDisplayLabel" Text="Offer Card"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOfferCard" runat="server" Style="width: 85px" ReadOnly="True"
                                                        ToolTip="Offer Card"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblRequiredFacility" runat="server" CssClass="styleDisplayLabel" Text="Required Facility"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRequiredFacility" runat="server" Style="width: 85px; text-align: right"
                                                        ReadOnly="True" ToolTip="Required Facility"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEmployeeName" runat="server" CssClass="styleDisplayLabel" Text="Employee"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEmployeeName" runat="server" Style="width: 200px" ReadOnly="True"
                                                        ToolTip="Employee"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSanctionedAmount" runat="server" CssClass="styleDisplayLabel" Text="Sanctioned Amount"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtSanctionedLimit" runat="server" Style="width: 85px; text-align: right"
                                                        ReadOnly="True" ToolTip="Sanctioned Amount"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblApprovalSerialNo" runat="server" CssClass="styleDisplayLabel" Text="Approval Serial No."></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtApprovalSerialNo" runat="server" Style="width: 20px; text-align: right"
                                                        ReadOnly="True" ToolTip="Approval Sl.No."></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblApprovalStatus" runat="server" CssClass="styleDisplayLabel" Text="Approval Status"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtApprovalStatus" runat="server" Style="width: 85px" ReadOnly="True"
                                                        ToolTip="Approval Status"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblReamrk" runat="server" CssClass="styleDisplayLabel" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" rowspan="2">
                                                    <asp:TextBox ID="txtRemark" runat="server" Style="width: 200px" ReadOnly="True" TextMode="MultiLine"
                                                        ToolTip="Remarks"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblApprovalDate" runat="server" CssClass="styleDisplayLabel" Text="Approval Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtApprovalDate" runat="server" Style="width: 85px" ReadOnly="True"
                                                        ToolTip="Approval Date"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlCustomerLevel" runat="server" CssClass="stylePanel" Width="99%"
                                        Visible="false" GroupingText="Customer Level">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblApprovalFor1" CssClass="styleDisplayLabel" Text="Approval For"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtApprovalFor1" runat="server" Width="85px" ReadOnly="True" ToolTip="Approval For"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSanctoinNumber1" runat="server" CssClass="styleDisplayLabel" Text="Sanction Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtSanctionNumber1" runat="server" Width="165px" ReadOnly="True"
                                                        ToolTip="Sanction Number"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCustomerCode1" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCustomerCode1" runat="server" Style="width: 165px" ReadOnly="True"
                                                        ToolTip="Customer Code"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblSanctionDate1" CssClass="styleDisplayLabel" Text="Sanction Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtSanctionDate1" runat="server" Width="85px" ReadOnly="True" ToolTip="Sanction Date"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCustomerName1" runat="server" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCustomerName1" runat="server" Style="width: 220px" ReadOnly="True"
                                                        ToolTip="Customer Name"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLOB1" runat="server" Visible="false" CssClass="styleDisplayLabel"
                                                        Text="Line of Business"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLOB1" runat="server" Visible="false" Style="width: 200px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblRequiredFacility1" runat="server" CssClass="styleDisplayLabel"
                                                        Visible="false" Text="Required Facility"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRequiredFacility1" Visible="false" runat="server" Style="width: 200px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSanctionedLimit1" runat="server" Visible="false" CssClass="styleDisplayLabel"
                                                        Text="Sanctioned Limit"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtSanctionedLimit1" runat="server" Visible="false" Style="width: 200px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblOfferCard1" runat="server" Visible="false" CssClass="styleDisplayLabel"
                                                        Text="Offer Card"></asp:Label>
                                                    &nbsp;
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOfferCard1" runat="server" Visible="false" Style="width: 200px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblOverRide1" runat="server" Visible="false" CssClass="styleDisplayLabel"
                                                        Text="Over Ride"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOverRide1" runat="server" Visible="false" Style="width: 200px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEmployee1" runat="server" Visible="false" CssClass="styleDisplayLabel"
                                                        Text="Employee"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEmployee1" runat="server" Visible="false" Style="width: 200px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFinalSanctionedLimit1" runat="server" CssClass="styleDisplayLabel"
                                                        Visible="false" Text="Final Sanctioned Limit"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFinalSanctionedLimit1" runat="server" Visible="false" Style="width: 200px"
                                                        ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td align="center" colspan="4">
                                                    &nbsp;
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td valign="top" colspan="4">
                                                    <asp:Panel ID="pnlLobDetailsHeader" runat="server" CssClass="accordionHeader">
                                                        <div style="float: left;">
                                                            Line of Business details</div>
                                                        <div style="float: left; margin-left: 20px;">
                                                            <asp:Label ID="lblDetails" runat="server">(Show Details...)</asp:Label>
                                                        </div>
                                                        <div style="float: right; vertical-align: middle;">
                                                            <asp:ImageButton ID="imgDetails" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                AlternateText="(Show Details...)" />
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlLOBInfo" runat="server">
                                                        <asp:GridView ID="grvLOBDetails" runat="server" AutoGenerateColumns="false" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLOB" runat="server" Text="Line of Business"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="llblLOBI" runat="server" Text='<%# Bind("LineofBusiness") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblRequiredfacility" runat="server" Text="Required Facility"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRequiredfacilityI" runat="server" Text='<%# Bind("RequiredFacility") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblSanctionedLimit" runat="server" Text="Sanctioned Limit"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSanctionedLimitI" runat="server" Text='<%# Bind("SanctionedLimit") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblOverride" runat="server" Text="Override"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOverrideI" runat="server" Text='<%# Bind("Override") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblUserName" runat="server" Text="Employee"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUserNameI" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblFinalSanctionedAmount" runat="server" Text="Final Sanctioned Amount"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFinalSanctionedAmountI" runat="server" Text='<%# Bind("FinalSanctionedAmount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblApprovedDate" runat="server" Text="Approved Date"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblApprovedDateI" runat="server" Text='<%# Bind("ApprovedDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <%--<cc1:Accordion ID="Acc01" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeader"
                                                        ContentCssClass="accordionContent" FadeTransitions="false" FramesPerSecond="40"
                                                        SelectedIndex="-1" TransitionDuration="250" AutoSize="None" RequireOpenedPane="false"
                                                        SuppressHeaderPostbacks="true">
                                                        <Panes>
                                                            <cc1:AccordionPane ID="ACPLOBDetails" runat="server" Width="100%">
                                                                <Header>
                                                                    Line of Business Details
                                                                </Header>
                                                                <Content>
                                                                </Content>
                                                            </cc1:AccordionPane>
                                                        </Panes>
                                                    </cc1:Accordion>--%>
                                                </td>
                                                <td valign="top" colspan="4">
                                                    <cc1:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="pnlLOBInfo"
                                                        ExpandControlID="pnlLobDetailsHeader" CollapseControlID="pnlLobDetailsHeader"
                                                        Collapsed="true" TextLabelID="lblDetails" ImageControlID="imgDetails" ExpandedText="(Hide Details...)"
                                                        CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                        CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" Width="99%" GroupingText="Score Details">
                                        <div id="divScore" style="overflow: None; height: 100%; width: 100%">
                                            <asp:GridView ID="gvCreditScore" runat="server" Width="75%" AutoGenerateColumns="False"
                                                HorizontalAlign="Center" ToolTip="Credit Score Details">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbldesc" runat="server" Text="Description"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldescI" runat="server" Text='<%# Bind("CreditScore") %>' Style="padding-left: 10px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblPerOfImport" runat="server" Text="% of Importance"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPerOfImportI" runat="server" Text='<%# Bind("PercentageOfImportance") %>'
                                                                Style="padding-right: 20px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblScore" runat="server" Text="Required Score"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScoreI" runat="server" Text='<%# Bind("RequiredScore") %>' Style="padding-right: 20px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblActualValues" runat="server" Text="Actual Score"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActualValuesI" runat="server" Text='<%# Bind("ActualScore") %>'
                                                                Style="padding-right: 15px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="30%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                                                    OnClick="btnCancel_Click" />
                                                <asp:Button ID="btnPrint" runat="server" CssClass="styleSubmitButton" Text="Print"
                                                    OnClick="btnPrint_Click" ToolTip="Print" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr width="99%">
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" width="100%" border="0">
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
