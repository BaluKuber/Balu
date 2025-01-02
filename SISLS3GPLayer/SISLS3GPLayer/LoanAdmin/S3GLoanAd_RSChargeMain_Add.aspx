<%@ Page Title="S3G - RS Charge Maintenance" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GLoanAd_RSChargeMain_Add.aspx.cs"
    Inherits="LoanAdmin_S3GLoanAd_RSChargeMain_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LOV" Src="~/UserControls/LOBMasterView.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress" TagPrefix="uc4" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="RS Charge Maintenance - Create" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" Width="100%" GroupingText="RS Charge Maintenance">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblRSCMCode" runat="server" Text="RS Charge Maintenance Code">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtRSCMCode" Enabled="false" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line Of Business" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlLineOfBusiness" runat="server" Width="140px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblLocation" runat="server" Text="Location" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetLocationDetails" AutoPostBack="true" OnItem_Selected="ddlLocation_SelectedIndexChanged"
                                            ErrorMessage="Select Location" ValidationGroup="Main" IsMandatory="true" />
                                    </td>--%>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlCustomerName" runat="server" ServiceMethod="GetCustomerDetails" ErrorMessage="Select Customer Name" AutoPostBack="true"
                                            Width="135px" ValidationGroup="Main" IsMandatory="true" OnItem_Selected="ddlCustomerName_SelectedIndexChanged" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblTranche" runat="server" Text="Tranche" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlTranche" runat="server" ServiceMethod="GetTrancheDetails" AutoPostBack="true"
                                            ErrorMessage="Select Tranche" OnItem_Selected="ddlTranche_SelectedIndexChanged" Width="160px"
                                            ValidationGroup="Main" IsMandatory="true" />
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
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Customer Details">
                            <table width="100%" cellspacing="0">
                                <tr>
                                    <td style="width: 100%;">
                                        <uc4:S3GCustomerAddress ID="S3GCustomerAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                            ShowCustomerCode="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="Panel3" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Funder Details">
                            <table width="100%" cellspacing="0">
                                <tr>
                                    <td style="width: 100%;">
                                        <uc4:S3GCustomerAddress ID="S3GFunderAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                            FirstColumnWidth="35%%" SecondColumnWidth="61%" ShowCustomerCode="false" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Panel4" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Form 8 Details">
                            <table width="100%" cellspacing="0">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblfunderdisbursementdate" runat="server" Text="Funder Disbursement Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lbrfunderdisbursementdate" runat="server"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF8filingduedate" runat="server" Text="Filing Due Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lbrhF8filingduedate" runat="server"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF8filingdate" runat="server" Text="Filing Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF8FilingDate" runat="server" Width="48%" MaxLength="12" OnTextChanged="txthF8FilingDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Image ID="imghF8FilingDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="hCalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txthF8FilingDate" PopupButtonID="imghF8FilingDate" Enabled="True"></cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvhF8FilingDate" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthF8FilingDate" ErrorMessage="Enter the Filing Date For Form 8."
                                            ValidationGroup="F8Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF8ChargeID" runat="server" Text="Charge ID" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF8ChargeID" runat="server" MaxLength="15"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftehF8ChargeID" runat="server" TargetControlID="txthF8ChargeID" FilterMode="InvalidChars"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvhF8ChargeID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthF8ChargeID" ErrorMessage="Enter the Charge ID For Form 8."
                                            ValidationGroup="F8Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF8SRNNo" runat="server" Text="SRN No" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF8SRNNO" runat="server" MaxLength="15"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftehF8SRNNO" runat="server" TargetControlID="txthF8SRNNO" FilterMode="InvalidChars"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvhF8SRNNO" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthF8SRNNO" ErrorMessage="Enter the SRN No For Form 8."
                                            ValidationGroup="F8Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhPVAmount" runat="server" Text="Total Charge Amount" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthPVAmount" runat="server" MaxLength="10"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftehPVAmount" runat="server" TargetControlID="txthPVAmount" FilterMode="ValidChars"
                                            FilterType="Custom" ValidChars="1234567890" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvhPVAmount" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthPVAmount" ErrorMessage="Enter the Total Charge Amount For Form 8."
                                            ValidationGroup="F8Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhchbF8CLBApplicable" runat="server" Text="CLB Applicable" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:CheckBox ID="hchbF8CLBApplicable" runat="Server" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhf8remarks" runat="server" Text="Remarks" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF8Remarks" runat="server" MaxLength="100" TextMode="MultiLine" onkeyup="maxlengthfortxt(100);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftehF8Remarks" runat="server" TargetControlID="txthF8Remarks" FilterMode="InvalidChars"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnF8Go" runat="server" Text="Go" ValidationGroup="F8Go"
                                            CssClass="styleSubmitButton" OnClick="btnF8Go_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="Panel5" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Form 17 Details">
                            <table width="100%" cellspacing="0">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblforeclosuredate" runat="server" Text="ForeClosure Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lbrforeclosuredate" runat="server"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblnoddate" runat="server" Text="No Dues Certificate Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <%--<asp:Label ID="lbrnoddate" runat="server"></asp:Label>--%>
                                        <asp:TextBox ID="txthF17nocdate" runat="server" Width="50%" MaxLength="12" AutoPostBack="true" OnTextChanged="txthF17nocdate_TextChanged"></asp:TextBox>
                                        <asp:Image ID="ImgF17nocdate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="hcalext3" runat="server" Format="dd/MM/yyyy" TargetControlID="txthF17nocdate"
                                            PopupButtonID="ImgF17nocdate" Enabled="True">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvhF17nocdate" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthF17nocdate" ErrorMessage="Enter the No Dues Certificate Date"
                                            ValidationGroup="F17Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF17ReleasingDueDate" runat="server" Text="Releasing Due Date">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lbrhF17ReleasingDueDate" runat="server"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF17ReleasingDate" runat="server" Text="Releasing Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF17ReleasingDate" runat="server" Width="50%" MaxLength="12" AutoPostBack="true" OnTextChanged="txthF17ReleasingDate_TextChanged"></asp:TextBox>
                                        <asp:Image ID="ImghF17ReleasingDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="hCalendarExtender2" runat="server" Format="dd-MM-yyyy" TargetControlID="txthF17ReleasingDate" PopupButtonID="ImghF17ReleasingDate" Enabled="True"></cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvhF17ReleasingDate" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthF17ReleasingDate" ErrorMessage="Enter the Releasing Date For Form 17."
                                            ValidationGroup="F17Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF17ChargeID" runat="server" Text="Charge ID" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF17ChargeID" runat="server" MaxLength="15"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftehF17ChargeID" runat="server" TargetControlID="txthF17ChargeID" FilterMode="InvalidChars"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvhF17ChargeID" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthF17ChargeID" ErrorMessage="Enter the Charge ID For Form 17."
                                            ValidationGroup="F17Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhF17SRNNo" runat="server" Text="SRN No" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF17SRNNO" runat="server" MaxLength="15"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftehF17SRNNO" runat="server" TargetControlID="txthF17SRNNO" FilterMode="InvalidChars"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvhF17SRNNO" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="txthF17SRNNO" ErrorMessage="Enter the SRN No For Form 17."
                                            ValidationGroup="F17Go" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhchbF17CLBApplicable" runat="server" Text="CLB Applicable" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:CheckBox ID="hchbF17CLBApplicable" runat="Server" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblhf17remarks" runat="server" Text="Remarks" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txthF17Remarks" runat="server" MaxLength="100" TextMode="MultiLine" onkeyup="maxlengthfortxt(100);"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftehF17Remarks" runat="server" TargetControlID="txthF17Remarks" FilterMode="InvalidChars"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnF17Go" runat="server" Text="Go" ValidationGroup="F17Go"
                                            CssClass="styleSubmitButton" OnClick="btnF17Go_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <cc1:TabContainer ID="tcRSChargeMaintenance" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            Width="100%" ScrollBars="Auto" TabStripPlacement="top" AutoPostBack="false">
                            <cc1:TabPanel ID="tpRSChargeMaintenance" runat="server" HeaderText="RS Charge Maintenance">
                                <HeaderTemplate>RS Charge Maintenance Details</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upRSChargeMaintenance" runat="server">
                                        <ContentTemplate>
                                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 764px">
                                                <asp:GridView ID="gvRSChargeMaintenance" runat="server" AutoGenerateColumns="false" ShowFooter="false" Width="2000px"
                                                    OnRowDataBound="gvRSChargeMaintenance_RowDataBound" OnRowCreated="gvRSChargeMaintenance_RowCreated">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Select All
                                                            <asp:CheckBox ID="chkAll" runat="server" ToolTip="Select All" AutoPostBack="true" OnCheckedChanged="chkAll_OnCheckedChanged" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="gchkselect" runat="server" AutoPostBack="true" OnCheckedChanged="gchkselect_OnCheckedChanged"></asp:CheckBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="S.No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRSCMserialno" runat="server" Text='<%#Container.DataItemIndex+1%>' ToolTip="Serial No."></asp:Label>
                                                                <%--<asp:Label ID="lblsno" runat="server" Text='<%#Bind("SNO") %>' Visible="false"></asp:Label>--%>
                                                                <asp:Label ID="lblCharge_Mgmt_Dtl_ID" runat="server" Text='<%#Bind("Charge_Mgmt_Dtl_ID") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="RS No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRSNO" runat="server" Text='<%#Bind("RSNO") %>' ToolTip="RSNO"></asp:Label>
                                                                <asp:Label ID="lblPA_SA_Ref_ID" runat="server" Text='<%#Bind("PA_SA_Ref_ID") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="View Asset Dtls" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkbtnviewinvoice" runat="server" Text="View" ToolTip="View Invoice" OnClick="lnkbtnviewinvoice_Click"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Filing Due Date" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblORG_FILINGDATE" runat="server" Text='<%#Bind("ORG_FILINGDATE") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblF8FilingDueDate" runat="server" Text='<%#Bind("F8FilingDueDate") %>' ToolTip="Filing Due Date"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Filing Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF8FilingDate" runat="server" AutoPostBack="true" Width="70%" OnTextChanged="txtF8FilingDate_TextChanged" MaxLength="12" Text='<%# Eval("F8FilingDate") %>'></asp:TextBox>
                                                                <asp:Image ID="imgF8FilingDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <cc1:CalendarExtender runat="server" Format="dd-MM-YYYY" TargetControlID="txtF8FilingDate" PopupButtonID="imgF8FilingDate" ID="CalendarExtender1" Enabled="True"></cc1:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Charge ID">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF8ChargeID" runat="server" MaxLength="15" Text='<%# Eval("F8ChargeID") %>'></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="fteF8ChargeID" runat="server" TargetControlID="txtF8ChargeID" FilterMode="InvalidChars"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SRN NO.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF8SRNNO" runat="server" MaxLength="15" Text='<%# Eval("F8SRNNO") %>'></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="fteF8SRNNO" runat="server" TargetControlID="txtF8SRNNO" FilterMode="InvalidChars"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--  <asp:TemplateField HeaderText="Total Charge Amount">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPVAmount" runat="server" MaxLength="10" Text='<%# Eval("PVAmount") %>'></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftegPVAmount" runat="server" TargetControlID="txtPVAmount" FilterMode="ValidChars"
                                                                    FilterType="Custom" ValidChars="1234567890" Enabled="true">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF8Remarks" runat="server" MaxLength="100" TextMode="MultiLine" Text='<%# Eval("F8Remarks") %>' onkeyup="maxlengthfortxt(100);"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="fteF8Remarks" runat="server" TargetControlID="txtF8Remarks" FilterMode="InvalidChars"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CLB Applicable" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chbF8CLBApplicable" runat="Server" Checked='<%# subProgram((DataBinder.Eval(Container.DataItem, "F8CLBApplicable") + "")) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Release Due Date" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblORG_RELEASINGDATE" runat="server" Text='<%#Bind("ORG_RELEASINGDATE") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblF17ReleaseDueDate" runat="server" Text='<%#Bind("F17ReleaseDueDate") %>' ToolTip="Release Due Date"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Release Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF17ReleaseDate" AutoPostBack="true" OnTextChanged="txtF17ReleaseDate_TextChanged" runat="server" Width="70%" MaxLength="10" Text='<%# Eval("F17ReleaseDate") %>'></asp:TextBox>
                                                                <asp:Image ID="imgF17ReleaseDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <cc1:CalendarExtender runat="server" Format="dd-MM-yyyy" TargetControlID="txtF17ReleaseDate" PopupButtonID="imgF17ReleaseDate" ID="CalendarExtender2" Enabled="True"></cc1:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Charge ID">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF17ChargeID" runat="server" MaxLength="15" Text='<%# Eval("F17ChargeID") %>' Style="text-align: left"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="fteF17ChargeID" runat="server" TargetControlID="txtF17ChargeID" FilterMode="InvalidChars"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SRN NO.">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF17SRNNO" runat="server" MaxLength="15" Text='<%# Eval("F17SRNNO") %>' Style="text-align: left"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="fteF17SRNNO" runat="server" TargetControlID="txtF17SRNNO" FilterMode="InvalidChars"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtF17Remarks" runat="server" MaxLength="100" TextMode="MultiLine" Text='<%# Eval("F17Remarks") %>'
                                                                    onkeyup="maxlengthfortxt(100);"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="fteF17Remarks" runat="server" TargetControlID="txtF17Remarks" FilterMode="InvalidChars"
                                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CLB Applicable" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chbF17CLBApplicable" runat="Server" Checked='<%# subProgram((DataBinder.Eval(Container.DataItem, "F17CLBApplicable") + "")) %>' />
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
                    <td colspan="2" style="padding-top: 5px; padding-left: 5px;" align="center">
                        <span runat="server" id="lblErrorMessage1" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td colspan="2" align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" Text="Save" ValidationGroup="Main" OnClientClick="return fnCheckPageValidators('Main');" />

                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />

                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="Main" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="VSFormDetails" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="F8Go" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="F17Go" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                    </td>
                </tr>
                <input type="hidden" id="hdnID" runat="server" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<POP UP CODE STARTS>--%>
    <asp:Button ID="btnModal" Style="display: none" runat="server" />
    <cc1:ModalPopupExtender ID="ModalPopupExtenderApprover" runat="server" TargetControlID="btnModal"
        PopupControlID="pnlAssetDtls" BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlAssetDtls" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="50%">
        <asp:UpdatePanel ID="UppnlAssetDtls" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView ID="grvAssetDtls" runat="server" AutoGenerateColumns="false" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Invoice No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoice_No" runat="server" Text='<%# Bind("Invoice_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoice_Date" runat="server" Text='<%# Bind("Invoice_Date") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAsset_Description" runat="server" Text='<%# Bind("Asset_Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Value" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAsset_Value" runat="server" Text='<%# Bind("Asset_Value") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <uc1:PageNavigator ID="ucPopUpPaging" runat="server"></uc1:PageNavigator>
                            <input type="hidden" id="hdnSortDirection" runat="server" />
                            <input type="hidden" id="hdnSortExpression" runat="server" />
                            <input type="hidden" id="hdnSearch" runat="server" />
                            <input type="hidden" id="hdnOrderBy" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnDEVModalCancel" runat="server" Text="Close" OnClick="btnDEVModalCancel_Click"
                                ToolTip="Close" class="styleSubmitButton" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--<POP UP CODE ENDS>--%>
    <script language="javascript" type="text/javascript">
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
