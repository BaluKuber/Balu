<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GInsAssetInsuranceClaim.aspx.cs" Inherits="Insurance_S3GInsAssetInsuranceClaim"
    EnableEventValidation="false" EnableViewState="true" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%--<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>--%>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Asset Insurance Claim" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="50%" valign="top">
                                    <asp:Panel ID="PnlInputCriteria" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                        Width="99%">
                                        <table width="100%" border="0">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblICNNO" runat="server" ToolTip="ICN No" CssClass="styleReqFieldLabel" Text="ICN No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtICNNO" runat="server" ToolTip="ICN No" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblICNDate" runat="server" ToolTip="ICN Date" CssClass="styleDisplayLabel" Text="ICN Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtICNDate" runat="server" ToolTip="ICN Date" ReadOnly="True"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftxtICNDate" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="txtICNDate" ValidChars="/-">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line of Business" Width="165px" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Line of Business" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvPolicyLOB" runat="server" ControlToValidate="ddlLOB"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Line of Business" ValidationGroup="Add"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                                    <%--<asp:DropDownList ID="ddlBranch" runat="server" ToolTip="Branch" Width="165px" Style="height: 22px"
                                                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                        Display="None" ErrorMessage="Select a Location" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvPolicyBranch" runat="server" ControlToValidate="ddlBranch"
                                                        Display="None" ErrorMessage="Select a Location" InitialValue="0" ValidationGroup="Add"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMLA" runat="server" CssClass="styleReqFieldLabel" Text="Prime Account Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlMLA" runat="server" ToolTip="Prime Account Number" Width="165px" OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvPrimeAccount" runat="server" ControlToValidate="ddlMLA"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Prime Account Number"
                                                        ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                    <%--<asp:RequiredFieldValidator ID="rfvPolicyMLA" runat="server" ControlToValidate="ddlMLA"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Prime Account Number"
                                                        ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSLA" runat="server" CssClass="styleReqFieldLabel" Text="Sub Account Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlSLA" runat="server" ToolTip="Sub Account Number" Width="165px" Style="height: 22px" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvSubAccount" runat="server" ControlToValidate="ddlSLA"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Sub Account Number" Enabled="false"
                                                        ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                 <td class="styleFieldLabel">
                                                     <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel">
                                                     </asp:Label>
                                                 </td>
                                                 <td class="styleFieldAlign">
                                                     <asp:CheckBox ID="chkActive" runat="server" />
                                                 </td>
                                             </tr>
                                            <tr>
                                                <td height="46px">
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td width="50%" valign="top">
                                    <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
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
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlPolicyDetails" runat="server" CssClass="stylePanel" GroupingText="Policy Details"
                                        Visible="True">
                                        <asp:Panel ID="pnlInner" runat="server" CssClass="stylePanel" Visible="True" ScrollBars="Horizontal">
                                            <%-- <div id="div1" style="width: 98%; padding-left: 1%;" runat="server">--%>
                                            <asp:GridView ID="gvPolicyDetails" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvPolicyDetails_RowDeleting"
                                                OnRowDataBound="gvPolicyDetails_RowDataBound" ShowFooter="True">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Claim No">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblAssetClaimNumber" runat="server" MaxLength="16" Style="padding-left: 5px"
                                                                Text='<%# Bind("AssetClaimNo") %>' ToolTip="Asset Claim Number" Width="120px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvlblAssetClaimNumber" runat="server" ControlToValidate="lblAssetClaimNumber"
                                                                ValidationGroup="Submit" Display="None" ErrorMessage="Enter the Claim Number"></asp:RequiredFieldValidator>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtAssetClaimNumber" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                                TargetControlID="lblAssetClaimNumber">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAssetClaimNumber" runat="server" MaxLength="16" ToolTip="Asset Claim Number"
                                                                Width="120px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtAssetClaimNumber" runat="server" ControlToValidate="txtAssetClaimNumber"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Claim Number"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Claim Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblAssetClaimDate" runat="server"  Width="80px" Text='<%# Bind("ClaimDate") %>'
                                                                ToolTip="AssetClaimDate"></asp:TextBox>
                                                            <asp:Image ID="imgAssetClaimDateEdit" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                                ToolTip="Select Asset Claim Date" Visible="false" />
                                                            <asp:RequiredFieldValidator ID="rfvlblAssetClaimDate" runat="server" ControlToValidate="lblAssetClaimDate"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Asset Claim Date"></asp:RequiredFieldValidator>
                                                            <cc1:CalendarExtender ID="calAssetClaimDateEdit" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                PopupButtonID="imgAssetClaimDateEdit" TargetControlID="lblAssetClaimDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                            </cc1:CalendarExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAssetClaimDate" runat="server"  ToolTip="AssetClaimDate" Width="80px"></asp:TextBox>
                                                            <asp:Image ID="imgAssetClaim" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Asset Claim Date"
                                                                Visible="false" />
                                                            <cc1:CalendarExtender ID="CEAssetClaimDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                PopupButtonID="Image1" TargetControlID="txtAssetClaimDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtAssetClaimDate" runat="server" ControlToValidate="txtAssetClaimDate"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                ErrorMessage="Select the Asset Claim Date"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Alert Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAssetAlertDate" runat="server"  Width="80px" Text='<%# Bind("AlertDate") %>'
                                                                ToolTip="Asset Alert Date" OnTextChanged="txtAssetAlertDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                            <asp:Image ID="imgAssetAlertDateEdit" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                                ToolTip="Select Asset Alert Date" Visible="false" />
                                                            <asp:RequiredFieldValidator ID="rfvlblAssetAlertDate" runat="server" ControlToValidate="txtAssetAlertDate"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                ErrorMessage="Select the Alert Date"></asp:RequiredFieldValidator>
                                                            <cc1:CalendarExtender ID="calAssetAlertDateEdit" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                PopupButtonID="imgAssetAlertDateEdit" TargetControlID="txtAssetAlertDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                            </cc1:CalendarExtender>
                                                        </ItemTemplate>
                                                        <%--<FooterTemplate>
                                                            <asp:TextBox ID="txtAlertDate" runat="server" MaxLength="8" ToolTip="AssetAlertDate" Width="80px"></asp:TextBox>
                                                            <asp:Image ID="imgAssetAlert" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Alert Date"
                                                                Visible="false" />
                                                            <cc1:CalendarExtender ID="CEAlertDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                                PopupButtonID="Image1" TargetControlID="txtAlertDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtAlertDate" runat="server" ControlToValidate="txtAlertDate"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                ErrorMessage="Select the Alert Date"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>--%>
                                                        </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Regn No or Serial No">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlMachineNoEdit" runat="server" Style="padding-left: 5px"
                                                                ToolTip="Regn No or Serial No" Width="120px" AutoPostBack="true" OnSelectedIndexChanged="ddlMachineNo_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                             <asp:RequiredFieldValidator ID="rfvMachineNo" runat="server" ControlToValidate="ddlMachineNoEdit"
                                                                Display="None" ValidationGroup="Submit" SetFocusOnError="True" InitialValue="0"
                                                                ErrorMessage="Enter the Regn No. or Serial No."></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlMachineNo" runat="server" MaxLength="20" ToolTip="Regn No or Serial No"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlMachineNo_SelectedIndexChanged"
                                                                Width="120px">
                                                            </asp:DropDownList>
                                                            <%--<asp:RequiredFieldValidator ID="rfvMachineNo" runat="server" ControlToValidate="txtRegNo"
                                                                Display="None" ErrorMessage="Enter the Machine No" ValidationGroup="Add">
                                                            </asp:RequiredFieldValidator>--%>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetDescriptionEdit" runat="server" Style="padding-left: 5px"
                                                                Text='<%# Bind("AssetDescription") %>' ToolTip="AssetDescription" Width="120px"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblAssetDescription" runat="server" MaxLength="20" ToolTip="Asset Description"
                                                                Width="120px"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insured By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsuredByEdit" runat="server" ToolTip="Insured By" Style="padding-left: 5px" Text='<%# Bind("InsuredBy") %>'
                                                                Width="120px"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblInsuredBy" runat="server" MaxLength="20" ToolTip="Insured By" Width="120px"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insurer Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsurerNameEdit" runat="server" Style="padding-left: 5px" Text='<%# Bind("Ins_Company_Name") %>'
                                                                ToolTip="Insurer Name" Width="120px"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblInsurerName" runat="server" MaxLength="20" ToolTip="Insurer Name"
                                                                Width="120px"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Policy No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPolicyNumberEdit" runat="server" Style="padding-left: 5px" Text='<%# Bind("PolicyNo") %>'
                                                                ToolTip="Policy Number" Width="120px"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblPolicyNumber" runat="server" MaxLength="20" ToolTip="Policy Number"
                                                                Width="120px"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Policy Setup Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPolicySetupDateEdit" runat="server" MaxLength="8" Width="80px"
                                                                Text='<%# Bind("PolicySetupDate") %>' ToolTip="PolicySetupDate"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblPolicySetupDate" runat="server" MaxLength="8" ToolTip="PolicySetupDate" Width="80px"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Policy Expiry Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPolicyExpiryDateEdit" runat="server" MaxLength="8" Width="80px"
                                                                Text='<%# Bind("PolicyExpiryDate") %>' ToolTip="Valid Till Date"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblPolicyExpiryDate" runat="server" MaxLength="8" ToolTip="Valid Till Date" Width="80px"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Insured Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInsuredAmountEdit" runat="server" Style="padding-left: 5px; text-align: right"
                                                                Text='<%# Bind("InsuredAmount") %>' ToolTip="Policy Value" Width="120px"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblInsuredAmount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Claim Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblClaimAmount" runat="server"  MaxLength="10" Style="padding-left: 5px; text-align: right"
                                                                Text='<%# Bind("ClaimAmount") %>' ToolTip="Claim Amount" Width="120px" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvlblClaimAmount" runat="server" ControlToValidate="lblClaimAmount"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Claim Amount"></asp:RequiredFieldValidator>
                                                                <cc1:FilteredTextBoxExtender ID="ftxtPremium1" runat="server" FilterType="Custom,Numbers"
                                                                ValidChars="." TargetControlID="lblClaimAmount">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtClaimAmount" runat="server" ToolTip="Claim Amount" Width="94%" MaxLength="10" Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                            </asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvtxtClaimAmount" runat="server" ControlToValidate="txtClaimAmount"
                                                                Display="None" ValidationGroup="Submit" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Claim Amount"></asp:RequiredFieldValidator>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtPremium" runat="server" FilterType="Custom,Numbers"
                                                                ValidChars="." TargetControlID="txtClaimAmount">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlStatusEdit" runat="server" Style="padding-left: 5px" ToolTip="Status"
                                                                Width="120px">
                                                              </asp:DropDownList>
                                                              <asp:RequiredFieldValidator ID="rfvddlstatus" runat="server" ControlToValidate="ddlStatusEdit"
                                                                Display="None" ValidationGroup="Submit" InitialValue ="0"
                                                                ErrorMessage="Select the Status"></asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlStatus" runat="server" MaxLength="20" ToolTip="Status" Width="120px">
                                                            </asp:DropDownList>
                                                              </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblRemarks" runat="server" TextMode="MultiLine" Style="padding-left: 5px"
                                                                Text='<%# Bind("Remarks") %>' ToolTip="Remarks" Width="120px" onkeyup="maxlengthfortxt(200)"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtRemarks" runat="server" onkeyup="maxlengthfortxt(200)" TextMode="MultiLine"
                                                                ToolTip="Remarks" Width="120px"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                Text="Remove">
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnAddPolicyClaim" runat="server" CausesValidation="true" CssClass="styleSubmitShortButton"
                                                                OnClick="btnAddPolicyClaim_OnClick" Text="Add" ValidationGroup="Add" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <%--</div>--%>
                                        </asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:Panel ID="pnlHistory" runat="server" CssClass="stylePanel" GroupingText="History"
                                        Width="99%">
                                        <div id="div11" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                            <asp:GridView ID="gvHistory" runat="server" OnRowDataBound="gvHistory_RowDataBound"
                                                EmptyDataText="No history found for selected account" AutoGenerateColumns="true"
                                                Visible="true" Width="100%">
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%" align="center">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                            Text="Save" ValidationGroup="Submit" OnClick="btnSave_OnClick" OnClientClick="return fnCheckPageValidators('Submit');" />
                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Clear" OnClick="btnClear_OnClick" OnClientClick="return fnConfirmClear();" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Cancel" OnClick="btnCancel_OnClick" />
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cvAssetInsuranceClaim" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true"></asp:CustomValidator>
            <asp:ValidationSummary ID="vsPolicyDetails" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="Submit" HeaderText="Correct the following validation(s):" />
            <asp:ValidationSummary ID="vsPolicyClaimDetails" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="Add" HeaderText="Correct the following validation(s):" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

 function showMenu(show) {
    if (show == 'T') {
        
        //Added by Kali K to solve error ( when menu is show scroll should appear in grid )
        //This is used to show scroll bar when div is used in GridView
        if(document.getElementById('divGrid1')!=null)
            {
                document.getElementById('divGrid1').style.width="800px";
                document.getElementById('divGrid1').style.overflow="scroll";
            }
        
        document.getElementById('divMenu').style.display = 'Block';
        document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

        document.getElementById('ctl00_imgShowMenu').style.display = 'none';
        document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

        (document.getElementById('ctl00_ContentPlaceHolder1_pnlPolicyDetails')).style.width = screen.width - 260;
        (document.getElementById('ctl00_ContentPlaceHolder1_gvPolicyDetails')).style.width = screen.width - 260;
        
        (document.getElementById('ctl00_ContentPlaceHolder1_pnlHistory')).style.width = screen.width - 260;
    }
    if (show == 'F') {
        
        //Added by Kali K to solve error ( when menu is hide scroll for is not hiding from grid view )
        //This is used to hide scroll bar when div is used in GridView
        if(document.getElementById('divGrid1')!=null)
            {
                document.getElementById('divGrid1').style.width="960px";
                document.getElementById('divGrid1').style.overflow="auto";
            }
            
        document.getElementById('divMenu').style.display = 'none';
        document.getElementById('ctl00_imgHideMenu').style.display = 'none';
        document.getElementById('ctl00_imgShowMenu').style.display = 'Block';

        //document.getElementById('divMenu').setAttribute('width', 0);
        
        (document.getElementById('ctl00_ContentPlaceHolder1_pnlPolicyDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
        (document.getElementById('ctl00_ContentPlaceHolder1_gvPolicyDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
        
        (document.getElementById('ctl00_ContentPlaceHolder1_pnlHistory')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
    }
}

function Resize()
        {
         if(document.getElementById('divMenu').style.display=='none')
          {
           (document.getElementById('ctl00_ContentPlaceHolder1_pnlPolicyDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
           (document.getElementById('ctl00_ContentPlaceHolder1_gvPolicyDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
           
           (document.getElementById('ctl00_ContentPlaceHolder1_pnlHistory')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
          }
         else
         {
           (document.getElementById('ctl00_ContentPlaceHolder1_pnlPolicyDetails')).style.width = screen.width - 260;
           (document.getElementById('ctl00_ContentPlaceHolder1_gvPolicyDetails')).style.width = screen.width - 260;
           
           (document.getElementById('ctl00_ContentPlaceHolder1_pnlHistory')).style.width = screen.width - 260;
         }  
        }
        
function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_btnLoadCustomer').click();
        }
    </script>

</asp:Content>
