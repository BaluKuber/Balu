<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" EnableEventValidation="false"
    Title="S3G - Proposal" AutoEventWireup="true" CodeFile="S3G_ORG_Proposal.aspx.cs" Inherits="Origination_S3G_ORG_Proposal" %>


<%@ OutputCache Location="None" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <cc1:TabContainer ID="tcProposal" runat="server" CssClass="styleTabPanel"
                    Width="99%" ScrollBars="None" TabStripPlacement="top">
                    <cc1:TabPanel runat="server" HeaderText="MainPage" ID="TabMainPage" CssClass="tabpan"
                        BackColor="Red">
                        <HeaderTemplate>
                            Main Page &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UPMainPage" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr style="width: 100%">
                                            <td width="60%">
                                                <asp:Panel ID="pnlProposalDetails" runat="server" ToolTip="Proposal Details" GroupingText="Proposal Details"
                                                    CssClass="stylePanel" Width="100%">
                                                    <table width="100%">
                                                        <tr style="width: 100%">
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblLob" runat="server" ToolTip="Line of Business" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <asp:DropDownList ID="ddlLob" runat="server" ToolTip="Line Of Business" ValidationGroup="Pricing" Width="155px"></asp:DropDownList>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblBranch" runat="server" Text="State" ToolTip="Location" Width="98%"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <uc2:Suggest ID="ddlBranchList" runat="server" ToolTip="Location" ServiceMethod="GetBranchList" ErrorMessage="Select a State" WatermarkText="--All--" width="97%" />
                                                                <%--AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged"--%> 
                                                             
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblProposalType" ToolTip="Proposal Type" runat="server" Text="Proposal Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <asp:DropDownList ID="ddlProposalType" ToolTip="Proposal Type" runat="server" ValidationGroup="Pricing" OnSelectedIndexChanged="ddlProposalType_SelectedIndexChanged" AutoPostBack="true" Width="99%"></asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlProposalType" runat="server" ControlToValidate="ddlProposalType"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Proposal Type"
                                                                    InitialValue="0" SetFocusOnError="False" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblProposalNumber" ToolTip="Proposal Number" runat="server" Text="Proposal Number" Width="98%"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <asp:TextBox ID="txtProposalNumber" ToolTip="Proposal Number" runat="server" ReadOnly="True" Width="98%"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblddlLeadRefNo" ToolTip="Lead Ref. No." runat="server" Text="Lead Ref. No."></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <uc2:Suggest ID="sugLeadRefNo" ToolTip="Lead Ref. No." runat="server" ServiceMethod="GetLeadRefNoList"
                                                                    ValidationGroup="Main Page" AutoPostBack="true" OnItem_Selected="sugLeadRefNo_Item_Selected" width="97%" />
                                                                <%--AutoPostBack="true" OnItem_Selected="sugLeadRefNo_SelectedIndexChanged"--%>
                                                            </td>

                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblOfferDate" runat="server" ToolTip="Proposal Date" Text="Proposal Date" Width="98%"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <asp:TextBox ID="txtOfferDate" runat="server" ToolTip="Proposal Date" Width="100px"></asp:TextBox>
                                                                <asp:Image ID="ImgtxtOfferDate" runat="server" ToolTip="Proposal Date" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ID="rfvtxtOfferDate" ToolTip="Proposal Date" runat="server" Display="None"
                                                                    ValidationGroup="Main Page" ErrorMessage="Enter the Proposal Date" ControlToValidate="txtOfferDate"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                    TargetControlID="txtOfferDate" PopupButtonID="ImgtxtOfferDate" ID="CEtxtOfferDate"
                                                                    Enabled="True">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblOfferValidTill" runat="server" ToolTip="Proposal valid till" Text="Proposal valid till" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <asp:TextBox ID="txtOfferValidTill" runat="server" ToolTip="Proposal valid till" Width="100px" ValidationGroup="Main Page"></asp:TextBox>
                                                                <asp:Image ID="imgCalValidpDate" runat="server" ToolTip="Proposal valid till" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ID="rfvOffervalidtill" runat="server" Display="None" ToolTip="Proposal valid till"
                                                                    ValidationGroup="Main Page" ErrorMessage="Enter the Offer valid till" ControlToValidate="txtOfferValidTill"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_PrevSystemDate"
                                                                    TargetControlID="txtOfferValidTill" PopupButtonID="imgCalValidpDate" ID="calExeOfferValidTill"
                                                                    Enabled="True">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblSecondaryTerm" runat="server" ToolTip="Secondary Term" Text="Secondary Term" Width="98%"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <%--<asp:DropDownList ID="ddlSecondaryTerm" runat="server" ValidationGroup="Pricing"></asp:DropDownList>--%>
                                                                <asp:RadioButtonList RepeatDirection="Horizontal" CssClass="styleDisplayLabel" runat="server" ID="RBLSecondaryTerm"
                                                                    OnSelectedIndexChanged="RBLSecondaryTerm_SelectedIndexChanged" AutoPostBack="true" ToolTip="Secondary Term" Width="98%">
                                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Value="0" Text="No"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblSecuritydeposit" runat="server" ToolTip="Security deposit Appl." Text="Sec. dep. Appl." CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <asp:DropDownList ID="ddlSecuritydeposit" ToolTip="Security deposit Appl." runat="server" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlSecuritydeposit_SelectedIndexChanged" Width="98%">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlSecuritydeposit" ToolTip="Security deposit Appl." runat="server" ControlToValidate="ddlSecuritydeposit"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Security deposit Appl."
                                                                    InitialValue="0" SetFocusOnError="False" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="AdvanceRent" runat="server" ToolTip="Advance Rent Appl." Text="Adv. Rent Appl." Width="98%"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <%--<asp:DropDownList ID="ddlAdvanceRent" runat="server" ValidationGroup="Pricing"></asp:DropDownList>--%>
                                                                <asp:RadioButtonList RepeatDirection="Horizontal" ToolTip="Advance Rent Appl." OnSelectedIndexChanged="RBLAdvanceRent_SelectedIndexChanged" AutoPostBack="true" CssClass="styleDisplayLabel" runat="server" ID="RBLAdvanceRent" Width="98%">
                                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Value="0" Text="No"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblSecDepAdvRent" ToolTip="Sec Dep/Adv Rent %" runat="server"
                                                                    Text="Sec Dep/Adv Rent %" CssClass="styleDisplayLabel">
                                                                </asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtSecDepAdvRent" ToolTip="Sec Dep/Adv Rent %" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                    ReadOnly="true" runat="server" MaxLength="15" Style="text-align: right"></asp:TextBox>
                                                                <asp:RangeValidator
                                                                    ID="rangevSecDepAdvRent" runat="server" ControlToValidate="txtSecDepAdvRent" CssClass="styleMandatoryLabel"
                                                                    Display="None" Type="Double" MaximumValue="100.00000" MinimumValue="0"
                                                                    ErrorMessage="Sec Dep/Adv Rent % cannot be greater than 100" SetFocusOnError="False" ValidationGroup="Main Page"> 
                                                                </asp:RangeValidator>
                                                                <asp:RequiredFieldValidator
                                                                    ID="rfvSecDepAdvRent" runat="server" Display="None" ControlToValidate="txtSecDepAdvRent"
                                                                    ErrorMessage="Enter the Sec Dep/Adv Rent %" ValidationGroup="Main Page" SetFocusOnError="true" CssClass="styleMandatoryLabel">
                                                                </asp:RequiredFieldValidator>

                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 24%">
                                                                <asp:Label ID="lblSecDep" runat="server" ToolTip="Security Deposit Appl. Based On" Text="Sec. Dep. Based On" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 26%">
                                                                <asp:DropDownList ID="ddlSecDepApp" ToolTip="Security deposit Appl." runat="server" Width="98%" Enabled="false">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Schedule Value" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Invoice Value" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Invoice Base Value" Value="3"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <%--<tr>
                                                            <td colspan="4" style="width: 100%">--%>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblOneTimeFee" runat="server" ToolTip="One Time Fee" Visible="false" Text="One Time Fee"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblProcessingFee" Visible="false" runat="server" ToolTip="Processing Fee" Text="Processing Fee"></asp:Label>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>

                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtOneTimeFee" Visible="false" ToolTip="One Time Fee" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                    runat="server" MaxLength="12" Style="text-align: right"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtProcessingFee" Visible="false" ToolTip="Processing Fee" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                    runat="server" MaxLength="12" Style="text-align: right"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>

                                                    <table width="100%">
                                                        <tr style="width: 100%">
                                                            <td valign="top" class="styleFieldAlign" style="width: 20%">

                                                                <%--<asp:RadioButton ID="rdbtnOneTime" AutoPostBack="true" OnCheckedChanged="rdbtnOneTime_CheckedChanged" runat="server" Text="One Time Fee" />--%>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">

                                                                <asp:Label ID="lblonetimerate" runat="server" Text="Rate Percentage" Width="100%"></asp:Label><br />

                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">

                                                                <asp:Label ID="lblonetimeamt" runat="server" Text="Amount" Width="100%"></asp:Label><br />

                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 30%">

                                                                <asp:Label ID="lblAmtBasedOn" runat="server" Text="Fee based on" Width="100%"></asp:Label>

                                                            </td>

                                                        </tr>
                                                        <tr style="width: 100%">

                                                            <td valign="top" class="styleFieldAlign" style="width: 20%">
                                                                <asp:Label ID="lblOneTime" runat="server" Text="One Time Fee" Width="98%"></asp:Label>
                                                            </td>

                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtOnetimeRate" AutoPostBack="true" OnTextChanged="txtOnetimeRate_TextChanged" runat="server" Style="text-align: right; width: 100%" MaxLength="12" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                <asp:CompareValidator ID="cvORate" runat="server" ControlToValidate="txtOnetimeRate" ErrorMessage="Rate cannot be greater than 100"
                                                                    Operator="LessThanEqual" Type="Double" Display="None" ValidationGroup="Main Page"
                                                                    ValueToCompare="100.00" /></td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtOneTimeAmount" AutoPostBack="true" OnTextChanged="txtOneTimeAmount_TextChanged" runat="server" Style="text-align: right; width: 100%" MaxLength="18"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 30%">
                                                                <asp:DropDownList ID="ddlAmtBasedOn" runat="server" Enabled="false" Width="100%">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Total Invoice Value" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Rental Schedule Value" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvOneTimeBasedOn" runat="server" ControlToValidate="ddlAmtBasedOn"
                                                                    ErrorMessage="Select a One Time Fee based on" ValidationGroup="Main Page" CssClass="styleMandatoryLabel"
                                                                    Display="None" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>

                                                            </td>

                                                        </tr>

                                                        <tr style="width: 100%">
                                                            <td valign="top" class="styleFieldAlign" style="width: 20%">

                                                                <%--<asp:RadioButton ID="rdtbnProcessing" AutoPostBack="true" OnCheckedChanged="rdtbnProcessing_CheckedChanged" runat="server" Text="Processing Fee" />--%>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">

                                                                <asp:Label ID="lblProcessingRate" runat="server" Text="Rate Percentage" Width="100%"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">

                                                                <asp:Label ID="lblProcessingAmount" runat="server" Text="Amount" Width="100%"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 30%">

                                                                <asp:Label ID="lblPAmtBasedOn" runat="server" Text="Fee based on" Width="100%"></asp:Label>

                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">

                                                            <td valign="top" class="styleFieldAlign" style="width: 20%">
                                                                <asp:Label ID="lblProcessFee" runat="server" Text="Processing Fee" Width="100%"></asp:Label>
                                                            </td>

                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtProcessingRate" AutoPostBack="true" OnTextChanged="txtProcessingRate_TextChanged" runat="server" Style="text-align: right; width: 100%" MaxLength="12" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                <asp:CompareValidator ID="cvPRate" runat="server" ControlToValidate="txtProcessingRate" ErrorMessage="Rate cannot be greater than 100"
                                                                    Operator="LessThanEqual" Type="Double" Display="None" ValidationGroup="Main Page"
                                                                    ValueToCompare="100.00" /></td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtProcessingAmount" AutoPostBack="true" OnTextChanged="txtProcessingAmount_TextChanged" runat="server" Style="text-align: right; width: 100%" MaxLength="18" onkeypress="fnAllowNumbersOnly(false,false,this)"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 30%">
                                                                <asp:DropDownList ID="ddlProcessingBasedOn" runat="server" Enabled="false" Width="100%">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Total Invoice Value" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Rental Schedule Value" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvProcessingBasedon" runat="server" ControlToValidate="ddlProcessingBasedOn"
                                                                    ErrorMessage="Select a Processing Fee based on" ValidationGroup="Main Page" CssClass="styleMandatoryLabel"
                                                                    Display="None" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>

                                                            </td>

                                                        </tr>
                                                    </table>

                                                    <table width="100%">
                                                        <tr style="width: 100%">
                                                            <td class="styleFieldLabel" style="width: 20%">
                                                                <asp:Label ID="lblRemarks" runat="server" ToolTip="Remarks" Text="Remarks"></asp:Label>
                                                            </td>
                                                            <td colspan="3" class="styleFieldAlign" style="width: 80%" align="center">
                                                                <asp:TextBox ID="txtRemarks" Width="99%" Rows="3" ToolTip="Remarks" runat="server" TextMode="MultiLine"
                                                                    MaxLength="1000" onkeyup="maxlengthfortxt(1000)"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%" colspan="4">
                                                                <asp:Label ID="lblRoundNo" runat="server" Visible="false" Text="lblRoundNo"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </table>

                                                    <table width="100%">
                                                        <tr style="width: 100%">
                                                            <td width="43%" colspan="2">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblGuaranteedEOTAppl" runat="server" CssClass="styleReqFieldLabel" Text="Guaranteed EOT Appl."></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">

                                                                            <asp:RadioButtonList RepeatDirection="Horizontal" CssClass="styleDisplayLabel" runat="server" ID="rblIs_Guaranteed_EOT_Appli"
                                                                                Width="98%" OnSelectedIndexChanged="rblIs_Guaranteed_EOT_Appli_SelectedIndexChanged" AutoPostBack="true">
                                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                                            </asp:RadioButtonList>

                                                                            <asp:RequiredFieldValidator ID="rfvGuaranteedEOTAppli" runat="server" Display="None"
                                                                                ValidationGroup="Main Page" ErrorMessage="Select Guaranteed EOT Appl."
                                                                                ControlToValidate="rblIs_Guaranteed_EOT_Appli"
                                                                                SetFocusOnError="False"></asp:RequiredFieldValidator>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblTermSheetDate" ToolTip="Term Sheet Date" runat="server" CssClass="styleReqFieldLabel" Text="Term Sheet Date"></asp:Label></td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtTermSheetDate" runat="server" ToolTip="Term Sheet Date" Width="100px" ValidationGroup="Main Page"></asp:TextBox>
                                                                <asp:Image ID="imgTermSheetDate" runat="server" ToolTip="Term Sheet Date" ImageUrl="~/Images/calendaer.gif" />
                                                                <asp:RequiredFieldValidator ID="rfvTermSheetDate" runat="server" Display="None" ToolTip="Term Sheet Date"
                                                                    ValidationGroup="Main Page" ErrorMessage="Enter Term Sheet Date" ControlToValidate="txtTermSheetDate"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                    TargetControlID="txtTermSheetDate" PopupButtonID="imgTermSheetDate" ID="calExeTermSheetDate"
                                                                    Enabled="True">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 100%">
                                                            <td colspan="2">
                                                                <table>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblGuaranteedEOT" runat="server" Text="Guaranteed EOT Sale"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">&nbsp;&nbsp;
                                                                <asp:TextBox ID="txtGuaranteedEOT" ToolTip="Guaranteed EOT Sale" runat="server" Width="45px" onkeypress="return isNumberKey(event)"
                                                                    MaxLength="9"></asp:TextBox>
                                                                        <asp:Label ID="lblGuaraEOTPer" runat="server" Font-Bold="true" Font-Size="Larger" Text=" %"></asp:Label>
                                                                        <asp:CompareValidator ID="cmpGuaranteedEOT" runat="server" ControlToValidate="txtGuaranteedEOT"
                                                                            ErrorMessage="Guaranteed EOT Sale % cannot be greater than 100"
                                                                            Operator="LessThanEqual" Type="Double" Display="None" ValidationGroup="Main Page"
                                                                            ValueToCompare="100.00" />

                                                                        <asp:RequiredFieldValidator ID="rfvGuaranteedEOT" runat="server" Display="None" Enabled="false"
                                                                            ValidationGroup="Main Page" ErrorMessage="Enter Guaranteed EOT Sale (%)"
                                                                            ControlToValidate="txtGuaranteedEOT" SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </table>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblGuaranteedEOTApp" runat="server" ToolTip="Guaranteed EOT Appl. on" Text="Guaranteed EOT Appl. on"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlGuaranteedEOTApp" runat="server">
                                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="RS Amount" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Invoice Amount" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Capitalized Amount" Value="3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvGuaranteedEOTApp" runat="server" Display="None" Enabled="false"
                                                                    ValidationGroup="Main Page" InitialValue="0" ErrorMessage="Select Guaranteed EOT Applicable on"
                                                                    ControlToValidate="ddlGuaranteedEOTApp"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </asp:Panel>
                                            </td>
                                            <td width="40%">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
                                                                CssClass="stylePanel" Width="99%">
                                                                <table width="100%" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" width="35%">
                                                                            <asp:Label runat="server" ID="lblCustomerName" ToolTip="Customer Code" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCustomerCode" runat="server" ToolTip="Customer Code" Style="display: none;" MaxLength="50"></asp:TextBox>
                                                                            <uc2:LOV ID="ucCustomerCodeLov" runat="server" strLOV_Code="CMD" onblur="return fnLoadCustomer()" />
                                                                            <%-- --%>
                                                                            <input id="hdnCustID" type="hidden" runat="server" />
                                                                            <asp:RequiredFieldValidator ID="rfvcmbCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                                ErrorMessage="Select a Customer Code" ValidationGroup="Main Page" CssClass="styleMandatoryLabel"
                                                                                Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                                ErrorMessage="Select a Customer Code" ValidationGroup="tcAsset" CssClass="styleMandatoryLabel"
                                                                                Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                            <asp:LinkButton ID="lnkViewCustomer" runat="server" Enabled="false" Text="View" OnClick="lnkViewCustomer_Click"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" width="100%">
                                                                            <asp:Button ID="btnLoadCustomer" runat="server"
                                                                                Style="display: none" Text="Load Customer" OnClick="btnLoadCustomer_OnClick" />
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                SecondColumnStyle="styleFieldAlign" ShowCustomerCode="false" />
                                                                        </td>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="divSpace" runat="server" style="height: 6px;">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlMRADetails" Visible="false" runat="server" ToolTip="MRA Details" CssClass="stylePanel" GroupingText="MRA Details" Width="100%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldAlign" width="45%">
                                                                            <asp:Label ID="lblMRANumber" ToolTip="MRA Number" runat="server" Text="MRA Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td width="53%">
                                                                            <asp:DropDownList ID="ddlMRANumber" runat="server" ToolTip="MRA Number" ValidationGroup="Pricing" Style="width: 92%"></asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvMRANumber" ToolTip="MRA Number" runat="server" ControlToValidate="ddlMRANumber"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the MRA Number"
                                                                                InitialValue="0" SetFocusOnError="False" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlOtherDetails" runat="server" ToolTip="Other Details" CssClass="stylePanel" GroupingText="Other Details" Width="100%">
                                                                <table width="100%">
                                                                    <tr style="width: 100%">
                                                                        <td width="45%">

                                                                            <asp:RadioButtonList RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLStructuredEI_SelectedIndexChanged" ToolTip="Structured Equated" runat="server" CssClass="styleDisplayLabel" ID="RBLStructuredEI">
                                                                                <asp:ListItem Selected="True" Value="1" Text="Equated"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="Structured"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>

                                                                    </tr>
                                                                    <tr style="width: 100%">
                                                                        <td class="styleFieldAlign" width="45%">
                                                                            <asp:Label ID="lblReturnPattern" ToolTip="Rental Pattern" runat="server" Text="Rental Pattern" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td width="53%">
                                                                            <asp:DropDownList ID="ddlReturnPattern" runat="server" ToolTip="Rental Pattern" AutoPostBack="true" OnSelectedIndexChanged="ddlReturnPattern_SelectedIndexChanged" ValidationGroup="Pricing" Style="width: 92%"></asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvddlReturnPattern" ToolTip="Rental Pattern" runat="server" ControlToValidate="ddlReturnPattern"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Rental Pattern"
                                                                                InitialValue="0" SetFocusOnError="False" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="width: 100%">
                                                                        <td class="styleFieldAlign" width="45%">
                                                                            <asp:Label ID="lblRentalBasedOn" ToolTip="Rental Based On" runat="server" Text="Primary Rental Based On" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td width="53%">
                                                                            <asp:DropDownList ID="ddlRentalBasedOn" runat="server" ToolTip="Rental Based On" AutoPostBack="true" OnSelectedIndexChanged="ddlRentalBasedOn_SelectedIndexChanged"
                                                                                ValidationGroup="Pricing" Style="width: 92%">
                                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Rental on Base Invoice Value" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="Rental on Gross Invoice Value" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvRentalBasedOn" ToolTip="Rental Based On" runat="server" ControlToValidate="ddlRentalBasedOn"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Rental Based On"
                                                                                InitialValue="0" SetFocusOnError="False" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="width: 100%; display: none;">
                                                                        <td class="styleFieldAlign" width="45%">
                                                                            <asp:Label Visible="false" ID="Label2" ToolTip="Secondary Rental Based On" runat="server" Text="Secondary Rental Based On" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td width="53%">
                                                                            <asp:DropDownList Visible="false" ID="ddlSecondaryRentalBasedOn" runat="server" ToolTip="Secondary Rental Based On"
                                                                                ValidationGroup="Pricing" Style="width: 92%">
                                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Rental on Base Invoice Value" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="Rental on Gross Invoice Value" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ToolTip="Secondary Rental Based On" runat="server" ControlToValidate="ddlSecondaryRentalBasedOn"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Secondary Rental Based On"
                                                                                InitialValue="0" SetFocusOnError="False" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td width="45%" class="styleFieldAlign">
                                                                            <asp:Label ID="lblTotalFacilityAmount" runat="server" ToolTip="Total Facility Amount" Text="Total Facility Amount" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td width="53%">
                                                                            <asp:TextBox ID="txtTotalFacilityAmount" ReadOnly="true" ToolTip="Total Facility Amount" onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="12" runat="server" Style="text-align: right; width: 90%"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtTotalFacilityAmount" runat="server" Display="None"
                                                                                ValidationGroup="Main Page" ErrorMessage="Enter the Total Facility Amount" ControlToValidate="txtTotalFacilityAmount"
                                                                                SetFocusOnError="False"></asp:RequiredFieldValidator>
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
                                            <td colspan="2"></td>
                                        </tr>

                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlPrimaryGrid" runat="server" CssClass="stylePanel" GroupingText="Primary Grid" Width="100%">
                                                    <asp:GridView ID="grvPrimaryGrid" runat="server" OnRowDeleting="grvPrimaryGrid_OnRowDeleting" OnRowCommand="grvPrimaryGrid_OnRowCommand"
                                                        AutoGenerateColumns="false" ShowFooter="true" Width="100%">
                                                        <Columns>

                                                            <%--slno--%>
                                                            <asp:TemplateField HeaderText="Sl.no." ItemStyle-Width="3%" FooterStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblslnoPG" runat="server" Text='<%# Bind("slno") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--Asset Category--%>
                                                            <asp:TemplateField HeaderText="Asset Category" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetCategoryPG" ToolTip="Asset Category" runat="server" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                                                    <asp:Label ID="lblAssetCategoryIDPG" ToolTip="Asset Category" Visible="false" runat="server" Text='<%# Bind("AssetCategory_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <%--<asp:DropDownList ID="ddlAssetCategoryPG" runat="server" Width="98%">
                                                                    </asp:DropDownList>--%>
                                                                    <uc2:Suggest ID="ddlAssetCategoryPG" ToolTip="Asset Category" runat="server"
                                                                        ServiceMethod="GetAssetCategoryList" ErrorMessage="Select a Asset Category"
                                                                        ValidationGroup="PrimaryGrid" InitialValue="-1" AutoPostBack="true"
                                                                        OnItem_Selected="ddlAssetCategoryPG_SelectedIndexChanged" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <%--Asset Type--%>
                                                            <asp:TemplateField HeaderText="Asset Type" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetTypePG" ToolTip="Asset Type" runat="server" Text='<%# Bind("AssetType") %>'></asp:Label>
                                                                    <asp:Label ID="lblAssetTypeIDPG" ToolTip="Asset Type" Visible="false" runat="server" Text='<%# Bind("AssetType_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <uc2:Suggest ID="ddlAssetTypePG" ToolTip="Asset Type" runat="server"
                                                                        ServiceMethod="GetAssetTypeList" ErrorMessage="Select a Asset Type"
                                                                        ValidationGroup="PrimaryGrid" InitialValue="-1" AutoPostBack="true"
                                                                        OnItem_Selected="ddlAssetCategoryPG_SelectedIndexChanged" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Asset Sub Type" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetSubTypePG" ToolTip="Asset Sub Type" runat="server" Text='<%# Bind("AssetSubType") %>'></asp:Label>
                                                                    <asp:Label ID="lblAssetSubTypeIDPG" ToolTip="Asset Sub Type" Visible="false" runat="server" Text='<%# Bind("AssetSubType_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <uc2:Suggest ID="ddlAssetSubTypePG" ToolTip="Asset Sub Type" runat="server"
                                                                        ServiceMethod="GetAssetSubTypeList" ErrorMessage="Select a Asset Sub Type"
                                                                        ValidationGroup="PrimaryGrid" InitialValue="-1" AutoPostBack="true"
                                                                        OnItem_Selected="ddlAssetCategoryPG_SelectedIndexChanged" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <%--Tenure--%>
                                                            <asp:TemplateField HeaderText="Tenure" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTenurePG" ToolTip="Tenure" runat="server" Text='<%# Bind("Tenure") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtTenurePG" ToolTip="Tenure" runat="server" Width="95%" Style="text-align: right"
                                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" MaxLength="5" AutoPostBack="true" OnTextChanged="txtTenurePG_OnTextChanged">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvTenurePG" runat="server" ToolTip="Tenure" ControlToValidate="txtTenurePG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Tenure" SetFocusOnError="False"
                                                                        ValidationGroup="PrimaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Rent Frequency--%>
                                                            <asp:TemplateField HeaderText="Rent Frequency" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRentFrequencyPG" ToolTip="Rent Frequency" runat="server" Text='<%# Bind("RentFrequency") %>'></asp:Label>
                                                                    <asp:Label ID="lblRentFrequencyIDPG" Visible="false" runat="server" Text='<%# Bind("RentFrequencyID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlRentFrequencyPG" ToolTip="Rent Frequency"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlRentFreqPG_SelectedIndexChanged" runat="server" Width="100px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvRentFrequencyPG" ToolTip="Rent Frequency" runat="server" ControlToValidate="ddlRentFrequencyPG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Rent Frequency"
                                                                        InitialValue="0" SetFocusOnError="False" ValidationGroup="PrimaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Rent Frequency--%>
                                                            <asp:TemplateField HeaderText="Adv/Arr" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAdvanceArrearPG" ToolTip="Adv/Arr" runat="server" Text='<%# Bind("AdvanceArrear") %>'></asp:Label>
                                                                    <asp:Label ID="lblAdvanceArrearIDPG" ToolTip="Adv/Arr" Visible="false" runat="server" Text='<%# Bind("AdvanceArrearID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlAdvanceArrearPG" ToolTip="Adv/Arr" runat="server" Width="98%">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvAdvanceArrearPG" ToolTip="Adv/Arr" runat="server" ControlToValidate="ddlAdvanceArrearPG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Adv/Arr"
                                                                        InitialValue="0" SetFocusOnError="False" ValidationGroup="PrimaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Facility Amount--%>
                                                            <asp:TemplateField HeaderText="Facility Amount" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFacilityAmountPG" ToolTip="Facility Amount" runat="server" Text='<%# Bind("FacilityAmount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFacilityAmountPG" ToolTip="Facility Amount" Style="text-align: right" runat="server" Width="100px"
                                                                        onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="12">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvFacilityAmountPG" ToolTip="Facility Amount" runat="server" ControlToValidate="txtFacilityAmountPG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Facility Amount" SetFocusOnError="False"
                                                                        ValidationGroup="PrimaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--From Install No--%>
                                                            <asp:TemplateField HeaderText="From Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromInstallNoPG" ToolTip="From Install No" runat="server" Text='<%# Bind("FromInstallNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFromInstallNoPG" ToolTip="From Install No" Style="text-align: right" runat="server" Width="95%" ReadOnly="true"
                                                                        onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="5">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvFromInstallNoPG" ToolTip="From Install No" runat="server" ControlToValidate="txtFromInstallNoPG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the From Install No" SetFocusOnError="False"
                                                                        ValidationGroup="PrimaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--To Install No--%>
                                                            <asp:TemplateField HeaderText="To Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToInstallNoPG" ToolTip="To Install No" runat="server" Text='<%# Bind("ToInstallNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtToInstallNoPG" ToolTip="To Install No" Style="text-align: right" runat="server" MaxLength="5"
                                                                        Width="95%" onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvToInstallNoPG" ToolTip="To Install No" runat="server" ControlToValidate="txtToInstallNoPG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the To Install No" SetFocusOnError="False"
                                                                        ValidationGroup="PrimaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Rent Rate--%>
                                                            <asp:TemplateField HeaderText="Rent Rate" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRentRatePG" runat="server" ToolTip="Rent Rate" Text='<%# Bind("RentRate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>

                                                                    <asp:TextBox ID="txtRentRatePG" runat="server" ToolTip="Rent Rate" MaxLength="19" Style="text-align: right" Width="60px"
                                                                        onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                        onchange="fnValidateRentRate_Primary();">
                                                                    </asp:TextBox>

                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--AMF Rate--%>
                                                            <asp:TemplateField HeaderText="AMF Rate" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAMFRentPG" runat="server" ToolTip="AMF Rate" Text='<%# Bind("AMFRent") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAMFRentPG" runat="server" ToolTip="AMF Rate" MaxLength="19" Style="text-align: right" Width="95%" onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                    </asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--START By siva  --%>
                                                            <asp:TemplateField HeaderText="Fixed Rent" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFixedRent" runat="server" ToolTip="Fixed Rent" Text='<%# Bind("FixedRent") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFixedRent" runat="server" ToolTip="Fixed Rent" MaxLength="17" Style="text-align: right" Width="60px" onchange="fnValidateRentRate_Primary();">
                                                                    </asp:TextBox>

                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Fixed AMF" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFixedAMF" runat="server" ToolTip="Fixed AMF" Text='<%# Bind("FixedAMF") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFixedAMF" runat="server" ToolTip="Fixed AMF" MaxLength="17" Style="text-align: right" Width="95%" onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                    </asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--END By siva  --%>
                                                            <%--Residual Per%--%>
                                                            <%--  <asp:TemplateField HeaderText="Residual Per%" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResidualPerPG" ToolTip="Residual Per%" runat="server" Text='<%# Bind("ResidualPer") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtResidualPerPG" ToolTip="Residual Per%" MaxLength="9" runat="server" Style="text-align: right"
                                                                        Width="95%" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvResidualPerPG" ToolTip="Residual Per%" runat="server" ControlToValidate="txtResidualPerPG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the To Residual Per%" SetFocusOnError="False"
                                                                        ValidationGroup="PrimaryGrid"></asp:RequiredFieldValidator>
                                                                    <asp:RangeValidator ID="rangevResidualPerPG" runat="server" ControlToValidate="txtResidualPerPG" CssClass="styleMandatoryLabel" Display="None" Type="Double" MaximumValue="100.00000" MinimumValue="1" ErrorMessage="Residual Percentage should be <=100" SetFocusOnError="False"
                                                                        ValidationGroup="PrimaryGrid" >
                                                                        </asp:RangeValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="5%" FooterStyle-Width="5%">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnRemoveRepayment" ToolTip="Remove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                        Text="Remove"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnAddRepayment" runat="server" ToolTip="ADD" Text="Add" CommandName="Add" CssClass="styleGridShortButton" ValidationGroup="PrimaryGrid"></asp:Button>
                                                                    <%--OnClick="btnAddRepayment_OnClick" ValidationGroup="TabRepayment1" OnClientClick="return fnCheckPageValidators('TabRepayment1',false)"--%>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Panel ID="pnlSecondaryGrid" runat="server" CssClass="stylePanel" GroupingText="Secondary Grid" Width="100%">
                                                    <asp:GridView ID="grvSecondaryGrid" runat="server" OnRowDeleting="grvSecondaryGrid_OnRowDeleting" OnRowCommand="grvSecondaryGrid_OnRowCommand"
                                                        AutoGenerateColumns="false" ShowFooter="true" Width="100%">
                                                        <Columns>

                                                            <%--slno--%>
                                                            <asp:TemplateField HeaderText="Sl.no." ItemStyle-Width="3%" FooterStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblslnoSG" runat="server" Text='<%# Bind("slno") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--Asset Category--%>
                                                            <asp:TemplateField HeaderText="Asset Category" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetCategorySG" ToolTip="Asset Category" runat="server" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                                                    <asp:Label ID="lblAssetCategoryIDSG" ToolTip="Asset Category" Visible="false" runat="server" Text='<%# Bind("AssetCategory_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <%--  <asp:DropDownList ID="ddlAssetCategorySG" runat="server" Width="98%" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategorySG_SelectedIndexChanged">
                                                                    </asp:DropDownList>--%>
                                                                    <%--<uc2:Suggest ID="ddlAssetCategorySG" runat="server" ServiceMethod="GetAssetCategoryList" ErrorMessage="Select a Asset Category"
                                                                        ValidationGroup="SecondaryGrid" IsMandatory="true" />--%>

                                                                    <uc2:Suggest ID="ddlAssetCategorySG" ToolTip="Asset Category" runat="server" ServiceMethod="GetAssetCategoryListSG" ErrorMessage="Select a Asset Category"
                                                                        ValidationGroup="SecondaryGrid" InitialValue="-1" AutoPostBack="true" OnItem_Selected="ddlAssetCategorySG_SelectedIndexChanged" />
                                                                    <%--  <asp:RequiredFieldValidator ID="rfvAssetCategorySG" runat="server" ControlToValidate="ddlAssetCategorySG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Asset Category"
                                                                        InitialValue="0" SetFocusOnError="False" ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>--%>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <%--Asset Type--%>

                                                            <asp:TemplateField HeaderText="Asset Type" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetTypeSG" ToolTip="Asset Type" runat="server" Text='<%# Bind("AssetType") %>'></asp:Label>
                                                                    <asp:Label ID="lblAssetTypeIDSG" ToolTip="Asset Type" Visible="false" runat="server" Text='<%# Bind("AssetType_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <uc2:Suggest ID="ddlAssetTypeSG" ToolTip="Asset Type" runat="server"
                                                                        ServiceMethod="GetAssetTypeListSG" ErrorMessage="Select a Asset Type"
                                                                        ValidationGroup="PrimaryGrid" InitialValue="-1" AutoPostBack="true"
                                                                        OnItem_Selected="ddlAssetCategorySG_SelectedIndexChanged" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Asset Sub Type" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetSubTypeSG" ToolTip="Asset Sub Type" runat="server" Text='<%# Bind("AssetSubType") %>'></asp:Label>
                                                                    <asp:Label ID="lblAssetSubTypeIDSG" ToolTip="Asset Sub Type" Visible="false" runat="server" Text='<%# Bind("AssetSubType_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <uc2:Suggest ID="ddlAssetSubTypeSG" ToolTip="Asset Sub Type" runat="server"
                                                                        ServiceMethod="GetAssetSubTypeListSG" ErrorMessage="Select a Asset Sub Type"
                                                                        ValidationGroup="PrimaryGrid" InitialValue="-1" AutoPostBack="true"
                                                                        OnItem_Selected="ddlAssetCategorySG_SelectedIndexChanged" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <%--Tenure--%>
                                                            <asp:TemplateField HeaderText="Tenure" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTenureSG" ToolTip="Tenure" runat="server" Text='<%# Bind("Tenure") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtTenureSG" ToolTip="Tenure" AutoPostBack="true" OnTextChanged="txtTenureSG_OnTextChanged" runat="server" Width="95%" Style="text-align: right"
                                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" MaxLength="5">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvTenureSG" ToolTip="Tenure" runat="server" ControlToValidate="txtTenureSG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Tenure" SetFocusOnError="False"
                                                                        ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Rent Frequency--%>
                                                            <asp:TemplateField HeaderText="Rent Frequency" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRentFrequencySG" ToolTip="Rent Frequency" runat="server" Text='<%# Bind("RentFrequency") %>'></asp:Label>
                                                                    <asp:Label ID="lblRentFrequencyIDSG" Visible="false" ToolTip="Rent Frequency" runat="server" Text='<%# Bind("RentFrequencyID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlRentFrequencySG" ToolTip="Rent Frequency" runat="server"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlRentFreqSG_SelectedIndexChanged" Width="100px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvRentFrequencySG" ToolTip="Rent Frequency" runat="server" ControlToValidate="ddlRentFrequencySG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Rent Frequency"
                                                                        InitialValue="0" SetFocusOnError="False" ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Rent Frequency--%>
                                                            <asp:TemplateField HeaderText="Adv/Arr" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAdvanceArrearSG" ToolTip="Adv/Arr" runat="server" Text='<%# Bind("AdvanceArrear") %>'></asp:Label>
                                                                    <asp:Label ID="lblAdvanceArrearIDSG" ToolTip="Adv/Arr" Visible="false" runat="server" Text='<%# Bind("AdvanceArrearID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlAdvanceArrearSG" ToolTip="Adv/Arr" runat="server" Width="98%">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvAdvanceArrearSG" ToolTip="Adv/Arr" runat="server" ControlToValidate="ddlAdvanceArrearSG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Adv/Arr"
                                                                        InitialValue="0" SetFocusOnError="False" ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Facility Amount--%>
                                                            <asp:TemplateField HeaderText="Facility Amount" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFacilityAmountSG" ToolTip="Facility Amount" runat="server" Text='<%# Bind("FacilityAmount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFacilityAmountSG" ToolTip="Facility Amount" Style="text-align: right" runat="server" Width="95%"
                                                                        onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="12">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvFacilityAmountSG" ToolTip="Facility Amount" runat="server" ControlToValidate="txtFacilityAmountSG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Facility Amount" SetFocusOnError="False"
                                                                        ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--From Install No--%>
                                                            <asp:TemplateField HeaderText="From Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromInstallNoSG" ToolTip="From Install No" runat="server" Text='<%# Bind("FromInstallNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFromInstallNoSG" ToolTip="From Install No" Style="text-align: right" runat="server" Width="95%" ReadOnly="true"
                                                                        onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="5">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvFromInstallNoSG" ToolTip="From Install No" runat="server" ControlToValidate="txtFromInstallNoSG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the From Install No" SetFocusOnError="False"
                                                                        ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--To Install No--%>
                                                            <asp:TemplateField HeaderText="To Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToInstallNoSG" ToolTip="To Install No" runat="server" Text='<%# Bind("ToInstallNo") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtToInstallNoSG" ToolTip="To Install No" Style="text-align: right" runat="server" MaxLength="5"
                                                                        Width="95%" onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvToInstallNoSG" ToolTip="To Install No" runat="server" ControlToValidate="txtToInstallNoSG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the To Install No" SetFocusOnError="False"
                                                                        ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--Rent Rate--%>
                                                            <asp:TemplateField HeaderText="Rent Rate" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRentRateSG" runat="server" ToolTip="Rent Rate" Text='<%# Bind("RentRate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtRentRateSG" runat="server" ToolTip="Rent Rate" MaxLength="10" Style="text-align: right" Width="60px" onkeypress="fnAllowNumbersOnly(false,false,this)" onchange="fnValidateRentRate_Secondary();">
                                                                    </asp:TextBox>

                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--AMF Rate--%>
                                                            <asp:TemplateField HeaderText="AMF Rate" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAMFRentSG" runat="server" ToolTip="AMF Rate" Text='<%# Bind("AMFRent") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAMFRentSG" runat="server" ToolTip="AMF Rate" MaxLength="10" Style="text-align: right" Width="95%" onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                    </asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Fixed Rent" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFixedRentSG" runat="server" ToolTip="Fixed Rent" Text='<%# Bind("FixedRent") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFixedRentSG" runat="server" ToolTip="Fixed Rent" MaxLength="17" Style="text-align: right" Width="60px" onkeypress="fnAllowNumbersOnly(false,false,this)" onchange="fnValidateRentRate_Secondary();">
                                                                    </asp:TextBox>

                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Fixed AMF" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFixedAMFSG" runat="server" ToolTip="Fixed AMF" Text='<%# Bind("FixedAMF") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFixedAMFSG" runat="server" ToolTip="Fixed AMF" MaxLength="17" Style="text-align: right" Width="95%" onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                    </asp:TextBox>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <%--Residual Per%--%>
                                                            <%--  By Siva.k on 01JUL2015 Remove the Residual Per
                                                              <asp:TemplateField HeaderText="Residual Per%" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResidualPerSG" runat="server" ToolTip="Residual Per%" Text='<%# Bind("ResidualPer") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtResidualPerSG" MaxLength="9" ToolTip="Residual Per%" runat="server" Style="text-align: right"
                                                                        Width="95%" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                                    </asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvResidualPerSG" ToolTip="Residual Per%" runat="server" ControlToValidate="txtResidualPerSG"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the To Residual Per%" SetFocusOnError="False"
                                                                        ValidationGroup="SecondaryGrid"></asp:RequiredFieldValidator>
                                                                     <asp:RangeValidator ID="rangevResidualPerSG" runat="server" ControlToValidate="txtResidualPerSG" CssClass="styleMandatoryLabel" Display="None" Type="Double" MaximumValue="100.00000" MinimumValue="1" ErrorMessage="Residual Percentage should be <=100" SetFocusOnError="False"
                                                                        ValidationGroup="SecondaryGrid" >
                                                                        </asp:RangeValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="5%" FooterStyle-Width="5%">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnRemoveRepayment" ToolTip="Remove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                        Text="Remove"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnAddRepayment" ToolTip="Add" runat="server" Text="Add" CommandName="Add" CssClass="styleGridShortButton" ValidationGroup="SecondaryGrid"></asp:Button>
                                                                    <%--OnClick="btnAddRepayment_OnClick" ValidationGroup="TabRepayment1" OnClientClick="return fnCheckPageValidators('TabRepayment1',false)"--%>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table width="90%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel runat="server" ID="pnlRebateDet" CssClass="stylePanel" GroupingText="Rebate Details">
                                                                <table>
                                                                    <tr>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label ID="lblRebateApp" runat="server" Text="Rebate Discount Allowed" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>

                                                                            <asp:RadioButtonList RepeatDirection="Horizontal" onclick="fnRebateChange(this);"
                                                                                OnSelectedIndexChanged="rblRebateDiscountApp_SelectedIndexChanged" AutoPostBack="true"
                                                                                CssClass="styleDisplayLabel" runat="server"
                                                                                ID="rblRebateDiscountApp" Width="48%">
                                                                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                                            </asp:RadioButtonList>

                                                                            <asp:RequiredFieldValidator ID="rfvRebateDiscountApp" runat="server" Display="None"
                                                                                ValidationGroup="Main Page" ErrorMessage="Select Rebate Discount Allowed" ControlToValidate="rblRebateDiscountApp"
                                                                                SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                                        </td>

                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblRebateNoofInstall" runat="server" Text="Rebate Allowed in installments (Nos.)"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRebateNoofInstall" runat="server" Style="text-align: right; width: 50px" MaxLength="3"
                                                                                onkeypress="return isNumberKey(event)"></asp:TextBox>

                                                                            <asp:RequiredFieldValidator ID="rfvRebateNoofInstall" runat="server" Display="None" Enabled="false"
                                                                                ValidationGroup="Main Page" ErrorMessage="Enter Rebate Allowed in installments (Nos.)" ControlToValidate="txtRebateNoofInstall"
                                                                                SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                                        </td>

                                                                    </tr>

                                                                    <tr>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label ID="lblRebateStructured" runat="server" Text="Rebate Allowed Method" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RadioButtonList RepeatDirection="Horizontal" AutoPostBack="true" runat="server"
                                                                                OnSelectedIndexChanged="RBLRebateStructuredEI_SelectedIndexChanged" CssClass="styleDisplayLabel" ID="RBLRebateStructuredEI">
                                                                                <asp:ListItem Value="1" Text="Equated"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="Structured"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                            <asp:RequiredFieldValidator ID="rfvRebateStructuredEI" runat="server" Display="None"
                                                                                ValidationGroup="Main Page" ErrorMessage="Select Rebate Allowed Method" ControlToValidate="RBLRebateStructuredEI"
                                                                                SetFocusOnError="False"></asp:RequiredFieldValidator>

                                                                        </td>
                                                                        <td colspan="2"></td>

                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblRebateDiscountPerc" runat="server" Text="Total Rebate Allowed" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRebateDiscountPerc" runat="server" Style="text-align: right; width: 100px" MaxLength="9"
                                                                                onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                            <asp:Label ID="lblPerc" runat="server" Font-Bold="true" Font-Size="Larger" Text=" %"></asp:Label>
                                                                            <asp:RequiredFieldValidator ID="rfvRebateDiscountPerc" runat="server" Display="None"
                                                                                ValidationGroup="Main Page" ErrorMessage="Enter Total Rebate Allowed %" ControlToValidate="txtRebateDiscountPerc"
                                                                                SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>

                                                            </asp:Panel>
                                                        </td>
                                                        <td>
                                                            <asp:Panel runat="server" CssClass="stylePanel" GroupingText="Other Cash Flow Details" ID="PnlOtherCashFlow">
                                                                <table>
                                                                    <tr>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Stamp Duty Applicable" ID="lblStampDuty"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlStampDuty">
                                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Interim Applicable" ID="lblInterimDetails"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList runat="server" ID="ddlInterimDetails">
                                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                                                            </asp:DropDownList>
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
                                        <td colspan="2">
                                            <asp:Panel runat="server" ID="pnlRebateStruc" CssClass="stylePanel" GroupingText="Rebate Structure Grid"
                                                Width="100%">
                                                <table>

                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRebateInput" runat="server" Text="Rebate Structure Allocation"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldLabel">

                                                            <asp:DropDownList ID="ddlRebateStrucAlloc" AutoPostBack="true" OnSelectedIndexChanged="ddlRebateStrucAlloc_SelectedIndexChanged" runat="server">
                                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Fixed Amount" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="Percentage" Value="2"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvRebateInput" runat="server" Display="None" Enabled="false"
                                                                ValidationGroup="Main Page" InitialValue="0" ErrorMessage="Select Rebate Structure Allocation"
                                                                ControlToValidate="ddlRebateStrucAlloc"
                                                                SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"></td>
                                                    </tr>

                                                </table>
                                                <asp:GridView ID="grvRebateDetGrid" runat="server" AutoGenerateColumns="False"
                                                    OnRowDeleting="grvRebateDetGrid_OnRowDeleting" OnRowCommand="grvRebateDetGrid_OnRowCommand"
                                                    ShowFooter="true" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="RebateDetId" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRebateDetID" runat="server" Text='<%# Bind("RebateDet_ID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <%--From Install No--%>
                                                        <asp:TemplateField HeaderText="From Installment No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFromInstallNoRebate" ToolTip="From Installment No" runat="server" Text='<%# Bind("FromInstallNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFromInstallNoRebate" ToolTip="From Install No" Style="text-align: right" runat="server" Width="95%"
                                                                    onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="5">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvFromInstallNoRebate" ToolTip="From Installment No" runat="server" ControlToValidate="txtFromInstallNoRebate"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the From Installment No" SetFocusOnError="False"
                                                                    ValidationGroup="RebateGrid"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="To Installment No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblToInstallNoRebate" ToolTip="To Install No" runat="server" Text='<%# Bind("ToInstallNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtToInstallNoRebate" ToolTip="To Install No" Style="text-align: right" runat="server" MaxLength="5"
                                                                    Width="95%" onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvToInstallNoRebate" ToolTip="To Install No" runat="server" ControlToValidate="txtToInstallNoRebate"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the To Installment No" SetFocusOnError="False"
                                                                    ValidationGroup="RebateGrid"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <%--Rent Rate--%>
                                                        <asp:TemplateField HeaderText="Rebate %" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRebatePerc" runat="server" Text='<%# Bind("RebatePerc") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>

                                                                <asp:TextBox ID="txtRebatePerc" runat="server" MaxLength="9" Style="text-align: right" Width="95%"
                                                                    onkeypress="fnAllowNumbersOnly(false,false,this)">
                                                                </asp:TextBox>

                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Fixed Rebate" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFixedRebate" runat="server" ToolTip="Fixed Rebate" Text='<%# Bind("FixedRebate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFixedRebate" runat="server" ToolTip="Fixed Rebate" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                    MaxLength="12" Style="text-align: right" Width="95%" onchange="fnValidateFixedRebate();">
                                                                </asp:TextBox>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="5%" FooterStyle-Width="5%">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnRemoveRepayment" CausesValidation="false" runat="server" CommandName="Delete"
                                                                    Text="Remove"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAddRepayment" runat="server" Text="Add" CommandName="Add" CssClass="styleGridShortButton" ValidationGroup="RebateGrid"></asp:Button>
                                                                <%--OnClick="btnAddRepayment_OnClick" ValidationGroup="TabRepayment1" OnClientClick="return fnCheckPageValidators('TabRepayment1',false)"--%>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                            </tr>
                                        <tr>

                                            <td colspan="2">

                                                <asp:Panel runat="server" ID="pnlAddiRebateDet" CssClass="stylePanel" GroupingText="Additional Rebate Details"
                                                    Width="90%">

                                                    <table width="95%">
                                                        <tr>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblAddiRebateApp" runat="server" Text="Additional Rebate Discount Allowed" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td>

                                                                <asp:RadioButtonList RepeatDirection="Horizontal" onclick="fnAddiRebateChange(this);"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="rblAddiRebateDiscountApp_SelectedIndexChanged"
                                                                    CssClass="styleDisplayLabel" runat="server"
                                                                    ID="rblAddiRebateDiscountApp" Width="48%">
                                                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                                </asp:RadioButtonList>

                                                                <asp:RequiredFieldValidator ID="rfvAddiRebateDiscountApp" runat="server" Display="None"
                                                                    ValidationGroup="Main Page" ErrorMessage="Select Additional Rebate Discount Allowed" ControlToValidate="rblAddiRebateDiscountApp"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                            </td>

                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblAddiRebateNoofInstall" runat="server" Text="Additional Rebate allowed in No. of installments:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAddiRebateNoofInstall" runat="server" Style="text-align: right; width: 50px" MaxLength="3"
                                                                    onkeypress="return isNumberKey(event)"></asp:TextBox>

                                                                <asp:RequiredFieldValidator ID="rfvAddiRebateNoofInstall" runat="server" Display="None" Enabled="false"
                                                                    ValidationGroup="Main Page" ErrorMessage="Enter Additional Rebate allowed in No. of installments:" ControlToValidate="txtAddiRebateNoofInstall"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="styleFieldAlign">
                                                                <asp:Label ID="lblAddiRebateStructured" runat="server" Text="Additional Rebate Allowed Method" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList RepeatDirection="Horizontal" AutoPostBack="true" runat="server"
                                                                    CssClass="styleDisplayLabel" ID="RBLAddiRebateStructuredEI">
                                                                    <asp:ListItem Value="1" Selected="True" Text="Equated"></asp:ListItem>

                                                                </asp:RadioButtonList>
                                                                <asp:RequiredFieldValidator ID="rfvAddiRebateStructuredEI" runat="server" Display="None"
                                                                    ValidationGroup="Main Page" ErrorMessage="Select Additional Rebate Allowed Method" ControlToValidate="RBLAddiRebateStructuredEI"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>

                                                            </td>
                                                            <td colspan="2"></td>
                                                        </tr>


                                                        <tr>

                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblAddiRebateDiscountPerc" runat="server" Text="Total Additional Rebate Allowed" CssClass="styleReqFieldLabel"></asp:Label>
                                                            </td>

                                                            <td>

                                                                <asp:TextBox ID="txtAddiRebateDiscountPerc" runat="server" Style="text-align: right; width: 100px" MaxLength="9"
                                                                    onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                <asp:Label ID="Label1" runat="server" Font-Size="Larger" Text=" % (on Invoice Value)"></asp:Label>
                                                                <asp:RequiredFieldValidator ID="rfvAddiRebateDiscountPerc" runat="server" Display="None"
                                                                    ValidationGroup="Main Page" ErrorMessage="Enter Additional Total Rebate Allowed %" ControlToValidate="txtAddiRebateDiscountPerc"
                                                                    SetFocusOnError="False"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td colspan="2"></td>

                                                        </tr>

                                                    </table>

                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grvPrimaryGrid" />
                                </Triggers>
                            </asp:UpdatePanel>

                            <asp:UpdatePanel ID="tempUpdate" runat="server">
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlFile" CssClass="stylePanel" GroupingText="File Upload"
                                        Width="90%">

                                        <asp:Label ID="lblActualPath" runat="server" Visible="false"></asp:Label>
                                        <asp:HiddenField ID="hdnSelectedPath" runat="server" />
                                        <asp:Button ID="btnBrowse" runat="server" OnClick="btnBrowse_OnClick" Style="display: none" />
                                        <table align="left" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" Text="Signed Proposal Document" ID="lblfilename" CssClass="styleFieldLabel"> </asp:Label>
                                                </td>
                                                <td style="padding: 0px">
                                                    <asp:CheckBox ID="chkSelect" runat="server" Enabled="false" Text="" Width="20px" />
                                                </td>
                                                <td style="padding: 0px">
                                                    <asp:FileUpload ID="flUpload" runat="server" ToolTip="Upload File" Width="90px" />
                                                    <asp:Button ID="btnDlg" runat="server" CausesValidation="false" CssClass="styleGridShortButton" Style="display: none" Text="Browse" />
                                                </td>
                                                <td class="styleFieldLabel" valign="top">
                                                    <asp:LinkButton ID="ReqIdhyplnkView" runat="server" Enabled="true" OnClick="LnkReqIdView_Click">Download</asp:LinkButton>
                                                </td>
                                                <td class="styleFieldLabel" valign="top">
                                                    <asp:Label ID="lblCurrentPath" runat="server" Style="position: absolute; color: Green; vertical-align: top" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnBrowse" />
                                </Triggers>
                            </asp:UpdatePanel>

                        </ContentTemplate>
                    </cc1:TabPanel>


                    <cc1:TabPanel runat="server" HeaderText="EndUseCustomerDeatils" ID="TabEUCDtls" CssClass="tabpan"
                        BackColor="Red">
                        <HeaderTemplate>
                            End Use Customer Name &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="upEUCDtls" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlEUCDtls" runat="server" CssClass="stylePanel" GroupingText="End Use Customer Details">
                                        <table width="99%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 10%; display: none;">
                                                    <asp:Label ID="lblAssetCategoryEUC" ToolTip="Asset Category" runat="server" Text="Asset Category" />
                                                </td>
                                                <td class="styleFieldAlign" style="width: 10%; display: none;">
                                                    <asp:DropDownList ID="ddlAssetCategoryEUC" ToolTip="Asset Category" onmouseover="ddl_itemchanged(this)" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RFVddlAssetCategoryEUC" ToolTip="Asset Category" runat="server" Display="None" ControlToValidate="ddlAssetCategoryEUC"
                                                        ValidationGroup="btnAddEUC" InitialValue="0" ErrorMessage="Select Asset Category"
                                                        CssClass="styleMandatoryLabel" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="RFVddlAssetCategoryEUCM" runat="server" Display="None" ControlToValidate="ddlAssetCategoryEUC"
                                                        ValidationGroup="btnModifyEUC" InitialValue="0" ErrorMessage="Select Asset Category"
                                                        CssClass="styleMandatoryLabel" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblCustomerName_EUC" ToolTip="Customer&#8217;s Customer name" runat="server" Text="Customer&#8217;s Customer name"
                                                        CssClass="styleReqFieldLabel" />
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:TextBox ID="txtCustomerName_EUC" runat="server"
                                                        onmouseover="txt_MouseoverTooltip(this)" MaxLength="100" />
                                                    <cc1:AutoCompleteExtender ID="autotxtCustomerName_EUC" MinimumPrefixLength="1" runat="server"
                                                        TargetControlID="txtCustomerName_EUC" ServiceMethod="GetCustomerName_EUCList" Enabled="True" ServicePath=""
                                                        CompletionSetCount="5" CompletionListCssClass="CompletionList" DelimiterCharacters=";,:"
                                                        CompletionListItemCssClass="CompletionListItemCssClass"
                                                        CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                        ShowOnlyCurrentWordInCompletionListItem="true">
                                                    </cc1:AutoCompleteExtender>
                                                    <asp:RequiredFieldValidator ID="rfvCustomerName_EUC" runat="server" ControlToValidate="txtCustomerName_EUC"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Customer&#8217;s Customer name" SetFocusOnError="False"
                                                        ValidationGroup="btnAddEUC"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvCustomerName_EUCM" runat="server" ControlToValidate="txtCustomerName_EUC"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Customer&#8217;s Customer name" SetFocusOnError="False"
                                                        ValidationGroup="btnModifyEUC"></asp:RequiredFieldValidator>
                                                </td>

                                                <td class="styleFieldLabel" style="width: 10%">
                                                    <asp:Label ID="lblEmailId_EUC" ToolTip="Email Id" runat="server" Text="Email Id" />
                                                </td>
                                                <td class="styleFieldAlign" style="width: 20%">
                                                    <asp:TextBox ID="txtEmailId_EUC" runat="server" ToolTip="Email Id"
                                                        onmouseover="txt_MouseoverTooltip(this)" MaxLength="60" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtEmailId_EUC" runat="server" ControlToValidate="txtEmailId_EUC" Enabled="false"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Email Id" SetFocusOnError="False"
                                                        ValidationGroup="btnAddEUC"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvtxtEmailId_EUCM" runat="server" ControlToValidate="txtEmailId_EUC" Enabled="false"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Email Id" SetFocusOnError="False"
                                                        ValidationGroup="btnModifyEUC"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FTEtxtEMailId" runat="server" TargetControlID="txtEmailId_EUC"
                                                        FilterType="Numbers, UppercaseLetters, LowercaseLetters,Custom" ValidChars="._-@"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RegularExpressionValidator ID="revEmailId_EUC" runat="server" ControlToValidate="txtEmailId_EUC"
                                                        ValidationGroup="btnAddEUC" Display="None" ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ID="revEmailId_EUCM" runat="server" ControlToValidate="txtEmailId_EUC"
                                                        ValidationGroup="btnModifyEUC" Display="None" ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblRemarks_EUC" ToolTip="Remarks" runat="server" Text="Remarks" />
                                                </td>
                                                <td class="styleFieldAlign" colspan="2">
                                                    <asp:TextBox ID="txtRemarks_EUC" Width="96%" TextMode="MultiLine" Rows="3" runat="server" ToolTip="Remarks"
                                                        onmouseover="txt_MouseoverTooltip(this)" MaxLength="1000" onkeyup="maxlengthfortxt(1000)" />
                                                    <asp:RequiredFieldValidator ID="rfvRemarks_EUC" runat="server" ControlToValidate="txtRemarks_EUC" Enabled="false"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Remarks" SetFocusOnError="False"
                                                        ValidationGroup="btnAddEUC"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvRemarks_EUCM" runat="server" ControlToValidate="txtRemarks_EUC" Enabled="false"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Remarks" SetFocusOnError="False"
                                                        ValidationGroup="btnModifyEUC"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td align="center" colspan="6" width="100%" valign="top" height="30px">
                                                    <asp:Button ID="btnAddEUC" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                        ValidationGroup="btnAddEUC" OnClick="btnAddEUC_Click" />
                                                    <asp:Button ID="btnModifyEUC" runat="server" CssClass="styleSubmitShortButton" Text="Edit"
                                                        ValidationGroup="btnModifyEUC" Enabled="False" OnClick="btnModifyEUC_Click" />
                                                    <asp:Button ID="btnhClearEUC" runat="server" CausesValidation="False" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearInt_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblEUCSlNo" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr width="100%">
                                                <td class="styleFieldLabel" colspan="6" align="center">
                                                    <asp:GridView ID="grvEUC" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Width="100%" ShowFooter="false" OnRowDeleting="grvEUC_RowDeleting">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:RadioButton ID="RBSelect" ToolTip="Select" runat="server" Checked="false" OnCheckedChanged="RBSelectInt_CheckedChanged"
                                                                        AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sl No" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialNo" ToolTip="Sl No" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="6%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Asset Category" ItemStyle-Width="20%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetCategoryGEUC" ToolTip="Asset Category" runat="server" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                                                    <asp:Label ID="lblAssetCategoryIDGEUC" ToolTip="Asset Category" Visible="false" runat="server" Text='<%# Bind("AssetCategory_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <%--CustomerName--%>
                                                            <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="25%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerNameGEUC" ToolTip="Customer Name" Width="96%" Style="word-break: break-all;" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--Email Id--%>
                                                            <asp:TemplateField HeaderText="Email Id" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEmailIdGEUC" Width="96%" ToolTip="Email Id" runat="server" Style="word-break: break-all;" Text='<%# Bind("EmailId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--Remarks--%>
                                                            <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="25%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRemarksGEUC" Width="96%" ToolTip="Remarks" ReadOnly="true" TextMode="MultiLine" Text='<%# Bind("Remarks") %>' Rows="3" runat="server"
                                                                        onmouseover="txt_MouseoverTooltip(this)" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnRemove" runat="server" ToolTip="Delete" Text="Delete" CommandName="Delete"
                                                                        CausesValidation="false">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    <asp:ValidationSummary ID="VSbtnAddEUC" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                        ValidationGroup="btnAddEUC" ShowMessageBox="false" HeaderText="Correct the following validation(s):" />
                                                    <asp:ValidationSummary ID="VSbtnModifyEUC" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                        ValidationGroup="btnModifyEUC" ShowMessageBox="false" HeaderText="Correct the following validation(s):" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" HeaderText="MainPage" ID="TabDANPricing" CssClass="tabpan"
                        BackColor="Red">
                        <HeaderTemplate>
                            DAN Pricing &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </HeaderTemplate>
                        <ContentTemplate>
                            <iframe runat="server" id="ifrmCRM" width="100%" frameborder="0" style="min-height: 700px; border-style: none;"
                                src="S3GEMICalculator.aspx?IsFromProposal=Yes"></iframe>
                            <%--<asp:Button runat="server" ID="btnFrameCancel" CssClass="styleSubmitButton" Text="Save" Style="display: none"
                            OnClick="btnFrameCancel_Click" />--%>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
                <asp:Button ID="btnAssetDAN" runat="server" OnClick="btnAssetDAN_Click" Style="display: none;" />
            </td>
        </tr>

        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton" OnClick="btnSave_Click" ValidationGroup="Main Page" />
                        <%--OnClientClick="return fnCheckPageValidators('Pricing')" --%>
                        <input type="hidden" runat="server" id="hdnAssetCatPG" />
                        <input type="hidden" runat="server" id="hdnAssetTypePG" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            CausesValidation="false" OnClick="btnClear_Click" /><%-- --%>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton" OnClick="btnCancel_Click"
                            CausesValidation="false" /><%-- --%>
                        <asp:Button ID="btnWithdraw" runat="server" Text="Proposal Cancel" CssClass="styleSubmitButton"
                            OnClientClick="return Confirmmsg('Do you want to cancel Proposal?'); "
                            CausesValidation="false" OnClick="btnWithdraw_Click" />
                        <asp:Button ID="BtnRevive" runat="server" Text="Revive Proposal" CssClass="styleSubmitButton"
                            OnClientClick="return Confirmmsg('Do you want to revive Proposal?'); "
                            CausesValidation="false" OnClick="btnWithdraw_Click" />
                        <%----%>
                    </ContentTemplate>

                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsProposal" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ValidationGroup="Main Page" />
                <asp:ValidationSummary ID="VSPrimaryGrid" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ValidationGroup="PrimaryGrid" />
                <asp:ValidationSummary ID="VSSecondaryGrid" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ValidationGroup="SecondaryGrid" />
                <asp:ValidationSummary ID="vsRebate" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" ValidationGroup="RebateGrid" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVProposal" runat="server" Display="None" ValidationGroup="btnSave"></asp:CustomValidator>
                <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>--%>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">

        //function pageLoad() {

        //    tab = $find('ctl00_ContentPlaceHolder1_tcProposal');
        //    var querymode = location.search.split("qsMode=");
        //    var queryupload = location.search.split("AsyncFileUploadID=");
        //    if (querymode.length > 1) {
        //        if (querymode[1].length > 1) {
        //            querymode = querymode[1].split("&");
        //            querymode = querymode[0];
        //        }
        //        else {
        //            querymode = querymode[1];
        //        }
        //        if (querymode != 'Q' && tab != null) {
        //            tab.add_activeTabChanged(on_Change);
        //        }
        //    }

        //}

        //var index = 0;
        //function on_Change(sender, e) {
        //    debugger;
        //   var newindex = tab.get_activeTabIndex(index);
        //   if (newindex == tab._tabs.length - 1) {
        //  document.getElementById('<%=btnAssetDAN.ClientID %>').click();
        //  }
        // }

        function fnRebateChange(obj) {
            var radio = obj.getElementsByTagName("input");
            for (var i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    if (radio[i].value == "0") {
                        document.getElementById('<%=txtRebateDiscountPerc.ClientID %>').disabled = true;
                        document.getElementById('<%=txtRebateNoofInstall.ClientID %>').disabled = true;
                        document.getElementById('<%=txtRebateDiscountPerc.ClientID %>').value = '';
                        document.getElementById('<%=txtRebateNoofInstall.ClientID %>').value = '';
                    }
                    else {
                        document.getElementById('<%=txtRebateDiscountPerc.ClientID %>').disabled = false;
                        document.getElementById('<%=txtRebateNoofInstall.ClientID %>').disabled = false;
                    }
                    break;
                }
            }
        }

        function fnClick() {
            document.getElementById("<%= ifrmCRM.ClientID %>").src = "S3GEMICalculator.aspx?IsFromProposal=Yes";
        }

        function fnLoadCustomer() {
            var VartxtCustomerCode = document.getElementById('<%=txtCustomerCode.ClientID %>').value;
            if (VartxtCustomerCode != null) {
                //if (document.getElementById('ctl00_ContentPlaceHolder1_tcProposal_TabMainPage_ucCustomerCodeLov_hdnID').value.trim() != "") { --By Siva 29MAY2015 handling Exception
                if (document.getElementById('<% =ucCustomerCodeLov.ClientID %>' + '_hdnID').value.trim() != "") {
                    document.getElementById('<%=btnLoadCustomer.ClientID %>').click();
                }
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }


        function fnValidateRentRate_Primary() {
            var txtRentRatePG = document.getElementById('<%=(grvPrimaryGrid.FooterRow.FindControl("txtRentRatePG")).ClientID %>');
            var txtAMFRentPG = document.getElementById('<%=(grvPrimaryGrid.FooterRow.FindControl("txtAMFRentPG")).ClientID %>');
            var txtFixedRent = document.getElementById('<%=(grvPrimaryGrid.FooterRow.FindControl("txtFixedRent")).ClientID %>');
            var txtFixedAMF = document.getElementById('<%=(grvPrimaryGrid.FooterRow.FindControl("txtFixedAMF")).ClientID %>');
            if (txtRentRatePG.value != '') {
                txtFixedRent.disabled = true;
                txtFixedAMF.disabled = true;
                txtFixedRent.value = '';
                txtFixedAMF.value = '';

            }
            else {
                txtFixedRent.disabled = false;
                txtFixedAMF.disabled = false;
            }
            if (txtFixedRent.value != '') {
                txtRentRatePG.disabled = true;
                txtAMFRentPG.disabled = true;
                txtRentRatePG.value = '';
                txtAMFRentPG.value = '';

            }
            else {
                txtRentRatePG.disabled = false;
                txtAMFRentPG.disabled = false;
            }
        }

        function fnValidateRentRate_Secondary() {
            var txtRentRateSG = document.getElementById('<%=(grvSecondaryGrid.FooterRow.FindControl("txtRentRateSG")).ClientID %>');
            var txtAMFRentSG = document.getElementById('<%=(grvSecondaryGrid.FooterRow.FindControl("txtAMFRentSG")).ClientID %>');
            var txtFixedRentSG = document.getElementById('<%=(grvSecondaryGrid.FooterRow.FindControl("txtFixedRentSG")).ClientID %>');
            var txtFixedAMFSG = document.getElementById('<%=(grvSecondaryGrid.FooterRow.FindControl("txtFixedAMFSG")).ClientID %>');
            if (txtRentRateSG.value != '') {
                txtFixedRentSG.disabled = true;
                txtFixedAMFSG.disabled = true;
                txtFixedRentSG.value = '';
                txtFixedAMFSG.value = '';

            }
            else {
                txtFixedRentSG.disabled = false;
                txtFixedAMFSG.disabled = false;
            }
            if (txtFixedRentSG.value != '') {
                txtRentRateSG.disabled = true;
                txtAMFRentSG.disabled = true;
                txtRentRateSG.value = '';
                txtAMFRentSG.value = '';

            }
            else {
                txtRentRateSG.disabled = false;
                txtAMFRentSG.disabled = false;
            }
        }

        function fnLoadPath(btnBrowse) {
            if (btnBrowse != null)
                document.getElementById(btnBrowse).click();
        }

        function fnAssignPath(btnBrowse, hdnPath) {
            if (btnBrowse != null)
                document.getElementById(hdnPath).value = document.getElementById(btnBrowse).value;
        }


    </script>
</asp:Content>


