<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLoanAdAssetVerification_Add.aspx.cs"
    Inherits="LoanAdmin_S3GLOANADAssetVeification_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td width="100%">
                        <table class="stylePageHeading" cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td width="100%">
                                    <asp:Label runat="server" ID="lblHeading" Text="Asset Verification" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="100%">
                                    <cc1:TabContainer ID="tcASV" runat="server" Width="100%" CssClass="styleTabPanel"
                                        ActiveTabIndex="0">
                                        <cc1:TabPanel runat="server" ID="TPASV1" CssClass="tabpan" TabIndex="0" BackColor="Red"
                                            Width="100%" HeaderText="Verification Request">
                                            <HeaderTemplate>
                                                Verification Request</HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel12"  runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="50%" valign="top">
                                                                    <asp:Panel ID="Panel5" Width="99%" runat="server" GroupingText="Details" CssClass="stylePanel">
                                                                        <table cellpadding="0" cellspacing="3" border="0" width="100%">
                                                                          
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    <span class="styleMandatory">*</span>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                                        ToolTip="Line of business">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblBranch" Text="Location" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    <span class="styleMandatory">*</span>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <uc3:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        ErrorMessage="Select a Location" IsMandatory="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                                                                                </td>
                                                                            </tr>
                                                                              <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblCustddlCode" runat="server" Text="Customer Code" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    <span class="styleMandatory">*</span>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="ddlCustCode" ToolTip="Customer Code" runat="server" Style="display: none;"
                                                                                        MaxLength="50"></asp:TextBox>
                                                                                    <uc2:LOV ID="ucCustomerCodeLov" onblur="return fnLoadCustomer();" runat="server"
                                                                                        strLOV_Code="CMD" />
                                                                                    <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" OnClick="btnLoadCustomer_Click"
                                                                                        Style="display: none" /><input id="hdnCustID" type="hidden" runat="server" />
                                                                                    
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblMLA" runat="server" Text="Prime Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    <span class="styleMandatory">*</span>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlMLA" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged"
                                                                                        Width="165px" ToolTip="Prime Account Number ">
                                                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblSLA" runat="server" Text="Sub Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlSLA" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged"
                                                                                        ToolTip="Sub Account Number">
                                                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="Panel1" runat="server" Width="99%" GroupingText="Asset Information"
                                                                        CssClass="stylePanel">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblAVNo" Text="Asset Verification Number" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtAVNo" runat="server" ReadOnly="True" Width="75%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblAssetCode" runat="server" Text="Asset Code" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    <span class="styleMandatory">*</span>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlAssetCode" runat="server">
                                                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblAVDate" Text="Asset Verification Date" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtAVDate" runat="server" Width="100px" ToolTip="Asset Verification Date"
                                                                                        ContentEditable="False"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td width="50%">
                                                                    <asp:Panel ID="Panel2" runat="server" ToolTip="Customer Information" GroupingText="Customer Information"
                                                                        CssClass="stylePanel">
                                                                        <br />
                                                                        <uc1:S3GCustomerAddress ID="ucCustDetails" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                            FirstColumnWidth="20%" SecondColumnWidth="30%" ActiveViewIndex="0" ThirdColumnWidth="20%"
                                                                            FourthColumnWidth="30%" ShowCustomerCode="false" />
                                                                        <br />
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="right" style="padding-right: 7%">
                                                                                    <asp:UpdatePanel ID="updid" runat="server" UpdateMode="Conditional">
                                                                                        <ContentTemplate>
                                                                                            <asp:Button ID="btnMailPreview" runat="server" CssClass="styleSubmitButton" ValidationGroup="btnSave"
                                                                                                Text="Mail Preview" ToolTip="Preview" OnClick="btnMailPreview_Click" OnClientClick="return fnCheckPageValidators('btnSave',false);" /><%--OnClientClick="return FillMailPreview();"--%>
                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:PostBackTrigger ControlID="btnMailPreview" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel Width="98%" ID="Panel3" runat="server" GroupingText="Verification Information"
                                                            ToolTip="Verification Information" CssClass="stylePanel">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="18%">
                                                                        <asp:Label ID="lblInspBy" runat="server" Text="Requested To" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="40%">
                                                                        <asp:DropDownList ID="ddlInspBy" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlInspBy_SelectedIndexChanged"
                                                                            Width="165px" ToolTip="Requested To">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td class="styleFieldLabel" width="18%">
                                                                        <asp:Label ID="lblInspCode" runat="server" Text="Name" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="32%">
                                                                        <asp:DropDownList ID="ddlInspCode" runat="server" ToolTip="Name" AutoPostBack="True"
                                                                            Width="140px">
                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblInspDate" runat="server" Text="Inspection Date" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblInspecDateFrom" runat="server" Text="From" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <asp:TextBox ID="txtInspDate" runat="server" ContentEditable="False" Width="75px"
                                                                            ToolTip="Inspection date"></asp:TextBox>
                                                                        <asp:Image ID="imgDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                        <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtInspDate"
                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDate"
                                                                            ID="CalendarExtender1" Enabled="True">
                                                                        </cc1:CalendarExtender>
                                                                        &nbsp;&nbsp;
                                                                        <asp:Label ID="lblInspecDateTo" runat="server" Text="To" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <asp:TextBox ID="txtInspDateTo" runat="server" ContentEditable="False" Width="75px"
                                                                            ToolTip="Inspection date"></asp:TextBox>
                                                                        <asp:Image ID="imgDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                        <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtInspDateTo"
                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateTo"
                                                                            ID="CalendarExtender1To" Enabled="True">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblVerification" runat="server" Text="Verification Charges" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtVerification" runat="server" Width="50%" MaxLength="10" align="Right"
                                                                            ToolTip="Verification Charges"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender runat="server" ID="ftxtVerification" FilterType="Numbers"
                                                                            TargetControlID="txtVerification" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblInspTime" runat="server" Text="Inspection Time" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <table cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblTimeFrom" runat="server" Text="From" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="styleTableData">
                                                                                    <asp:TextBox ID="txtHour" runat="server" Width="13px" MaxLength="2" Text="12" Style="border: none;
                                                                                        height: 17px; text-align: right;" onclick="funSelect(this);" onblur="funTrimLimit(this,'1');"
                                                                                        onkeydown="return funIncreDecreValue(this,'1');" ToolTip="Inspection Time"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtHour" FilterType="Numbers"
                                                                                        TargetControlID="txtHour" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:Label ID="lblColon" runat="server" Width="3px" Text=":" ToolTip="Inspection Time"
                                                                                        Style="vertical-align: text-top"></asp:Label>
                                                                                    <asp:TextBox ID="txtMins" runat="server" Text="00" Width="13px" MaxLength="2" Style="border: none;
                                                                                        height: 17px; text-align: right" onclick="funSelect(this);" onblur="funTrimLimit(this,'2');"
                                                                                        onkeydown="funIncreDecreValue(this,'2');" ToolTip="Inspection Time"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtMins" FilterType="Numbers"
                                                                                        TargetControlID="txtMins" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlAMPM" runat="server" Style="border-color: White" ToolTip="Inspection Time">
                                                                                        <asp:ListItem Text="AM" Value="AM" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblTimeTo" runat="server" Text="To" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td class="styleTableData">
                                                                                    <asp:TextBox ID="txtHourTo" runat="server" Width="13px" MaxLength="2" Text="12" Style="border: none;
                                                                                        height: 17px; text-align: right;" onclick="funSelect(this);" onblur="funTrimLimit(this,'1');"
                                                                                        onkeydown="return funIncreDecreValue(this,'1');" ToolTip="Inspection Time"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtHourTo" FilterType="Numbers"
                                                                                        TargetControlID="txtHourTo" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:Label ID="lblColonTo" runat="server" Width="3px" Text=":" ToolTip="Inspection Time"
                                                                                        Style="vertical-align: text-top"></asp:Label>
                                                                                    <asp:TextBox ID="txtMinsTo" runat="server" Text="00" Width="13px" MaxLength="2" Style="border: none;
                                                                                        height: 17px; text-align: right" onclick="funSelect(this);" onblur="funTrimLimit(this,'2');"
                                                                                        onkeydown="funIncreDecreValue(this,'2');" ToolTip="Inspection Time"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtMinsTo" FilterType="Numbers"
                                                                                        TargetControlID="txtMinsTo" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlAMPMTo" runat="server" Style="border-color: White" ToolTip="Inspection Time">
                                                                                        <asp:ListItem Text="AM" Value="AM" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:TextBox ID="txtInspTime" Visible="False" runat="server" Width="80px" MaxLength="8"
                                                                            ToolTip="Inspection Time"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender runat="server" ID="ftTime" FilterType="Custom, Numbers"
                                                                            TargetControlID="txtInspTime" ValidChars=" :APMapm" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:TextBox ID="txtInspTimeTo" Visible="False" runat="server" Width="80px" MaxLength="8"
                                                                            ToolTip="Inspection Time"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender runat="server" ID="ftTimeTo" FilterType="Custom, Numbers"
                                                                            TargetControlID="txtInspTimeTo" ValidChars=" :APMapm" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblMemo" runat="server" Text="Memo Charges" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtMemo" runat="server" MaxLength="10" Width="50%" align="Right"
                                                                            ToolTip="Memo Charges"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender runat="server" ID="ftxtMemo" FilterType="Numbers" TargetControlID="txtMemo"
                                                                            Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblLocation" runat="server" Text="Instructions" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign" colspan="3">
                                                                        <asp:TextBox ID="txtInstructions" runat="server" Style="width: 93%" TextMode="MultiLine"
                                                                            onkeyup="maxlengthfortxt(150);" ToolTip="Instruction"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblRemarks" Text="Remarks" runat="server" CssClass="styleDisplayLabel"
                                                                            Visible="False"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" colspan="3">
                                                                        <asp:TextBox ID="txtRemarks" onkeyup="maxlengthfortxt(150);" Style="width: 93%" ToolTip="Remarks"
                                                                            runat="server" TextMode="MultiLine" Visible="False"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <table id="Table1" width="100%" runat="server">
                                                            <tr runat="server">
                                                                <td runat="server">
                                                                </td>
                                                                <td runat="server">
                                                                    <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvddlCust" runat="server" ControlToValidate="ddlCustCode"
                                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvddlMLA" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                        runat="server" ControlToValidate="ddlMLA" SetFocusOnError="True" Display="None"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvddlSLA" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                        runat="server" ErrorMessage="Select a Sub Account Number" ControlToValidate="ddlSLA"
                                                                        SetFocusOnError="True" Display="None" Enabled="False" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvAssetCode" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                        runat="server" ControlToValidate="ddlAssetCode" SetFocusOnError="True" Display="None"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvddlInspBy" runat="server" ControlToValidate="ddlInspBy"
                                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvInspCode" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                        runat="server" ControlToValidate="ddlInspCode" SetFocusOnError="True" Display="None"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtInspDate"
                                                                        ValidationGroup="btnSave" Display="None" CssClass="styleSummaryStyle"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvInspecTime" runat="server" ControlToValidate="txtInspTime"
                                                                        Enabled="False" Display="None" SetFocusOnError="True" CssClass="styleSummaryStyle"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="rfvInsTime" InitialValue="0" runat="server" ControlToValidate="txtInspTime"
                                                                        SetFocusOnError="True" ValidationGroup="btnSave" Display="None" ValidationExpression="(^([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)|(^([0-9]|[1][0-9]|[2][0-3])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)"></asp:RegularExpressionValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvDateTo" runat="server" ControlToValidate="txtInspDateTo"
                                                                        ValidationGroup="btnSave" Display="None" CssClass="styleSummaryStyle"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="rfvInsTimeTo" InitialValue="0" runat="server"
                                                                        ControlToValidate="txtInspTimeTo" SetFocusOnError="True" ValidationGroup="btnSave"
                                                                        Display="None" ValidationExpression="(^([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)|(^([0-9]|[1][0-9]|[2][0-3])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)"></asp:RegularExpressionValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvLocation" CssClass="styleMandatoryLabel" runat="server"
                                                                        ControlToValidate="txtInstructions" SetFocusOnError="True" Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:CustomValidator ValidationGroup="btnSave" ID="custCheck" runat="server" Display="None"
                                                                        OnServerValidate="custCheck_ServerValidate"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TPASV2" CssClass="tabpan" TabIndex="0" BackColor="Red"
                                            Width="100%" HeaderText="Respond Information">
                                            <HeaderTemplate>
                                                Respond Information</HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel Width="98%" ID="Panel4" runat="server" GroupingText="Respond Information"
                                                            ToolTip="Response Information" CssClass="stylePanel">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="23%">
                                                                        <asp:Label ID="lblInspDate1" runat="server" Text="Inspection Date" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="20%">
                                                                        <asp:TextBox ID="txtInspDate1" runat="server" ContentEditable="False" Width="50%"
                                                                            ToolTip="Inspection date"></asp:TextBox>
                                                                        <asp:Image ID="imgDate1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                        <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtInspDate1"
                                                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDate1"
                                                                            ID="CalendarExtender2" Enabled="True">
                                                                        </cc1:CalendarExtender>
                                                                    </td>
                                                                    <td class="styleFieldLabel" width="25%">
                                                                        <asp:Label ID="lblInspTime1" runat="server" Text="Inspection Time" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <table cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td class="styleTableData" width="40px">
                                                                                    <asp:TextBox ID="txtHour1" runat="server" Width="13px" MaxLength="2" Text="12" Style="border: none;
                                                                                        height: 17px; text-align: right;" onclick="funSelect(this);" onblur="funTrimLimit(this,'1');"
                                                                                        onkeydown="return funIncreDecreValue(this,'1');" ToolTip="Inspection Time">
                                                                                    </asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtHour1" FilterType="Numbers"
                                                                                        TargetControlID="txtHour1" ValidChars="">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:Label ID="lblColon1" runat="server" Width="3px" Text=":" ToolTip="Inspection Time"
                                                                                        Style="vertical-align: text-top">
                                                                                    </asp:Label>
                                                                                    <asp:TextBox ID="txtMins1" runat="server" Text="00" Width="13px" MaxLength="2" Style="border: none;
                                                                                        height: 17px; text-align: right" onclick="funSelect(this);" onblur="funTrimLimit(this,'2');"
                                                                                        onkeydown="funIncreDecreValue(this,'2');" ToolTip="Inspection Time">
                                                                                    </asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtMins1" FilterType="Numbers"
                                                                                        TargetControlID="txtMins1" ValidChars="">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlAMPM1" runat="server" Style="border-color: White" ToolTip="Inspection Time">
                                                                                        <asp:ListItem Text="AM" Value="AM" Selected="True"></asp:ListItem>
                                                                                        <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <asp:Label ID="Label11" runat="server" Text="(HH:MM AM)"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:TextBox ID="txtInspTime1" Visible="false" runat="server" Width="80px" MaxLength="8"
                                                                            ToolTip="Inspection Time"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender runat="server" ID="ftTime1" FilterType="Numbers,Custom"
                                                                            TargetControlID="txtInspTime1" ValidChars=" :APMapm">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblVerified" Text="Physically Verified" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:CheckBox ID="chkVerified" runat="server" />
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblAssetstatus" runat="server" Text="Asset Status" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlAssetStatus" runat="server" Width="165px" ToolTip="Asset Status">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblLocation1" runat="server" Text="Location" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <span class="styleMandatory">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign" colspan="3">
                                                                        <asp:TextBox ID="txtLocation1" runat="server" Style="width: 93%" TextMode="MultiLine"
                                                                            onkeyup="maxlengthfortxt(150);" ToolTip="Location"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblRemarks1" Text="Remarks" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                                        <%--<span class="styleMandatory">*</span>--%>
                                                                    </td>
                                                                    <td class="styleFieldAlign" colspan="3">
                                                                        <asp:TextBox ID="txtRemarks1" onkeyup="maxlengthfortxt(150);" Style="width: 93%"
                                                                            ToolTip="Remarks" runat="server" TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblPhoto" Text="Verification Photo" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <cc1:AsyncFileUpload ID="fileScanImage" runat="server" Width="200px" OnUploadedComplete="asyncFileUpload_UploadedComplete"
                                                                            OnClientUploadComplete="uploadComplete" />
                                                                    </td>
                                                                    <td align="left" colspan="2">
                                                                        <asp:Label ID="lblDisplayFile" CssClass="styleAttachedFile" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        Download File
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="40px">
                                                                        <asp:LinkButton runat="server" Enabled="False" Text="Download" CausesValidation="False"
                                                                            OnClick="lnkDownload_Click" ID="lnkDownload"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblErrorMessage1" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <table id="Table2" width="100%" runat="server">
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="rfvDate1" runat="server" ControlToValidate="txtInspDate1"
                                                                        ValidationGroup="btnSave" Display="None" CssClass="styleSummaryStyle"></asp:RequiredFieldValidator>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvInspecTime1" runat="server" ControlToValidate="txtInspTime1"
                                                                        Enabled="false" Display="None" SetFocusOnError="True" CssClass="styleSummaryStyle"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>--%>
                                                                    <asp:RegularExpressionValidator ID="rfvInsTime1" InitialValue="0" runat="server"
                                                                        ControlToValidate="txtInspTime1" SetFocusOnError="True" ValidationGroup="btnSave"
                                                                        Display="None" ValidationExpression="(^([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)|(^([0-9]|[1][0-9]|[2][0-3])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)">
                                                                    </asp:RegularExpressionValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvLocation1" CssClass="styleMandatoryLabel" runat="server"
                                                                        ControlToValidate="txtLocation1" SetFocusOnError="True" Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvddlAssetStatus" InitialValue="0" CssClass="styleMandatoryLabel"
                                                                        runat="server" ControlToValidate="ddlAssetStatus" SetFocusOnError="True" Display="None"
                                                                        ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                                    <input type="hidden" runat="server" id="hdnFile" />
                                                                    <input type="hidden" runat="server" id="hdnFileName" />
                                                                    <input type="hidden" runat="server" id="hdnFileUploaded" />
                                                                    <%-- <asp:CustomValidator ID="rfvFile" runat="server" CssClass="styleMandatoryLabel" Display="None"
                                                                        ErrorMessage="Browse a File to upload"></asp:CustomValidator>--%>
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
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="TableButtons" width="100%" runat="server">
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vsAssetVerification" runat="server" HeaderText="Correct the following validation(s):"
                                        CssClass="styleMandatoryLabel" Width="100%" ValidationGroup="btnSave" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    &nbsp;&nbsp;<asp:Button ID="btnSave" Text="Save" runat="server" ValidationGroup="btnSave"
                                        CssClass="styleSubmitButton" OnClick="btnSave_Click" CausesValidation="true"
                                        OnClientClick="return fnCheckPageValidators('btnSave');" />
                                    &nbsp;<asp:Button runat="server" ID="btnClear" Text="Clear" CssClass="styleSubmitButton"
                                        CausesValidation="False" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Panel ID="PnlLetterPreview" Style="display: none" runat="server" Height="80%"
                    BackColor="White" BorderStyle="Solid" BorderColor="Black" Width="50%">
                    <table width="100%">
                        <tr>
                            <td colspan="2" class="stylePageHeading">
                                <asp:Label runat="server" Text="Mail Preview" ID="lblMailPreview" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="5px" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="styleFieldLabel" width="10%" valign="top">
                                <asp:Label runat="server" Text="To" ID="lblTo" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtTo" ReadOnly="true" EnableTheming="true" runat="server" Width="99%"
                                    MaxLength="12"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="styleFieldLabel" width="10%" valign="top">
                                <asp:Label runat="server" Text="Subject" ID="lblSubject" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtSubject" runat="server" Width="99%" Text="" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="2px" colspan="2">
                            </td>
                        </tr>
                        <tr height="50%" valign="top">
                            <td colspan="2" height="50%" valign="top">
                                <asp:TextBox ID="txtBody" TextMode="MultiLine" runat="server" Width="99%" Height="300px"
                                    ReadOnly="true" Style="text-align: justify;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px" colspan="2">
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center" colspan="2">
                                <%-- <asp:Button runat="server" ID="btnGenereatePreviewPDF" Text="Create Pdf" OnClick="PreviewPDF_Click"
                                    CausesValidation="False" CssClass="styleSubmitButton" />--%>
                                <asp:Button runat="server" ID="btnSendMail" Text="Send Mail" OnClick="btnSendMail_Click"
                                    CausesValidation="False" CssClass="styleSubmitButton" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="styleSubmitButton"
                                    OnClick="btnClosePreview_Click" />
                                <asp:Button ID="btnModal" runat="server" Width="0px" Height="0px" />
                                <cc1:ModalPopupExtender ID="ModalPopupExtenderMailPreview" runat="server" TargetControlID="btnModal"
                                    PopupControlID="PnlLetterPreview" BackgroundCssClass="styleModalBackground" DynamicServicePath=""
                                    Enabled="True">
                                </cc1:ModalPopupExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        //        var prm = Sys.WebForms.PageRequestManager.getInstance(); 
        //        prm.add_initializeRequest(initializeRequest);
        //        prm.add_endRequest(endRequest);
        //        var postbackElement; 

        //        function initializeRequest(sender, args) { 
        //        document.body.style.cursor = "wait";
        //        if (prm.get_isInAsyncPostBack()) 
        //        {
        //        //debugger
        //        args.set_cancel(true); 
        //        }
        //        }
        //        function endRequest(sender, args) {document.body.style.cursor = "default"; 
        //        }

        function fnLoadCustomer() {
                 
                        var btnLoadCustomer = document.getElementById("<%=btnLoadCustomer.ClientID%>");
            // document.getElementById('ctl00_ContentPlaceHolder1_btnLoadCustomer').click();
            btnLoadCustomer.click();
        }

        function funSelect(txtBox) {
            txtBox.select();
        }

        function funTrimLimit(txtBox, type) {
            var val = parseFloat(txtBox.value);

            if (type == '1') {
                if (val > 12) {
                    alert('Hours limit cannot exceed 12.');
                    txtBox.value = '12';
                }
                if (isNaN(val) || val == 0) {
                    txtBox.value = '12';
                }
            }
            else if (type == '2') {
                if (val > 59) {
                    alert('Minutes limit cannot exceed 59.');
                    txtBox.value = '59';
                }
                if (isNaN(val)) {
                    txtBox.value = '00';
                }
            }

            if (txtBox.value.toString().length == 1) {
                txtBox.value = '0' + txtBox.value.toString();
            }
        }

        function funIncreDecreValue(txtBox, type) {
            if (!document.getElementById("<%=btnSave.ClientID%>").disabled) {
                var sKeyCode = window.event.keyCode;
                var val = txtBox.value;
                var KeyValue = String.fromCharCode(parseFloat(sKeyCode))

                if (sKeyCode == 40) {
                    if (type == '1') {
                        if (val == 1 || val > 12) {
                            val = 12;
                        }
                        else {
                            val = val - 1;
                        }
                        txtBox.value = val;
                        if (txtBox.value.toString().length == 1) {
                            txtBox.value = '0' + txtBox.value.toString();
                        }
                        txtBox.select();
                    }
                    if (type == '2') {
                        if (val == 0 || val > 59) {
                            val = 59;
                        }
                        else {
                            val = val - 1;
                        }
                        txtBox.value = val;
                        if (txtBox.value.toString().length == 1) {
                            txtBox.value = '0' + txtBox.value.toString();
                        }
                        txtBox.select();
                    }
                }
                if (sKeyCode == 38) {
                    if (type == '1') {
                        if (parseFloat(val) >= 12) {
                            val = 1;
                        }
                        else {
                            val = parseFloat(val) + 1;
                        }
                        txtBox.value = val.toString();
                        if (txtBox.value.toString().length == 1) {
                            txtBox.value = '0' + txtBox.value.toString();
                        }
                        txtBox.select();
                    }
                    if (type == '2') {
                        if (parseFloat(val) >= 59) {
                            val = 0;
                        }
                        else {
                            val = parseFloat(val) + 1;
                        }
                        txtBox.value = val.toString();
                        if (txtBox.value.toString().length == 1) {
                            txtBox.value = '0' + txtBox.value.toString();
                        }
                        txtBox.select();
                    }
                }
                //             if((sKeyCode >= 48 && sKeyCode <= 57) || (sKeyCode >= 96 && sKeyCode <= 105))
                //             {
                //               if(sKeyCode >= 96)
                //               {
                //                 sKeyCode = sKeyCode - 48;
                //               }
                //               KeyValue = String.fromCharCode(parseFloat(sKeyCode))
                //               var CurIndex = fnGetIndex(txtBox);
                //               if(txtBox.value.toString().length == 2)
                //               {
                //                 //txtBox.value = '';
                //               }
                //               if(type == '1' && parseFloat(txtBox.value) > 12)
                //               { 
                //                 return false;
                //               }
                //             }  
                return true;
            }
        }

        function fnGetIndex(txtBox) {
            if (txtBox.createTextRange()) {
                var r = document.selection.createRange();
                r.moveEnd('character', txtBox.value.length);
                if (r.text == '') {
                    return txtBox.value.length;
                }
                return txtBox.value.lastIndexOf(r.text);
            }
            else {
                return txtBox.selectionStart;
            }
        }

        function getfile(objFileupload, objtxtFilePath, hidFilePath) {
            objFileupload.click();
            if (objFileupload.value == "") {
                hidFilePath.value = objtxtFilePath.value;
            }
            else {
                hidFilePath.value = objFileupload.value;
            }
        }

        function uploadComplete(sender, args) {
            //debugger;
            document.getElementById('<%=hdnFileName.ClientID%>').value = args._fileName;
            if (document.getElementById('<%=lblDisplayFile.ClientID%>') != null)
                document.getElementById('<%=lblDisplayFile.ClientID%>').innerText = args.get_fileName();
        }

        //         var tab;//,tab1,tab2,tab3 ;

        //    function pageLoad() 
        //    {       
        //       tab = $find('ctl00_ContentPlaceHolder1_tcASV');
        //       tab.add_activeTabChanged(on_Change);
        //    }
        //    
        //      var index=0;
        //      function on_Change(sender,e)
        //      {         
        //        var newindex=tab.get_activeTabIndex();
        //        
        //        if(newindex > index)          
        //        {
        //            switch (newindex)
        //            {
        //                case 1:
        //                    if(!Page_ClientValidate('btnSave'))
        //                    {                  
        //                        tab.set_activeTabIndex(0);
        //                        index=0;  
        //                        break;                
        //                    }                 
        //                    break;     
        //              
        //          }          
        //       }     
        //      }

    </script>

</asp:Content>
