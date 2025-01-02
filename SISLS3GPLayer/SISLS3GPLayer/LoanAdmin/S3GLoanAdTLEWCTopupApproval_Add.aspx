<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Topup Approval" CodeFile="S3GLoanAdTLEWCTopupApproval_Add.aspx.cs"
    Inherits="S3GLoanAdTLEWCTopupApproval_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updPanelNew" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" Text="TE/WC Topup Approval" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <div style="margin-top: 0px; margin-bottom: 10px;">
                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <table cellpadding="0" cellspacing="0" border="1" width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" colspan="2">
                                                    <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                        <tr id="trAppStatus" runat="server">
                                                            <td class="styleFieldLabel" colspan="2">
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 50%;">
                                                                            <asp:Label ID="Label1" runat="server" Text="Approval Status"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkUnapproval" AutoPostBack="true" Text="Unapproved" runat="server"
                                                                                Checked="true" OnCheckedChanged="chkUnapproval_CheckedChanged" />
                                                                            <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" TargetControlID="chkUnapproval"
                                                                                Key="1" runat="server">
                                                                            </cc1:MutuallyExclusiveCheckBoxExtender>
                                                                            &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkApproved" Text="All" runat="server" AutoPostBack="true"
                                                                                OnCheckedChanged="chkApproved_CheckedChanged" />
                                                                            <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender1" TargetControlID="chkApproved"
                                                                                Key="1" runat="server">
                                                                            </cc1:MutuallyExclusiveCheckBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="50%">
                                                                <asp:Panel GroupingText="Top up Details" ID="pnltopupDetails" runat="server" CssClass="stylePanel">
                                                                    <table width="100%" border="0">
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddllLineOfBusiness" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddllLineOfBusiness_SelectedIndexChanged"
                                                                                    Width="200px" ToolTip="Line of Business">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddllLineOfBusiness"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Line of Business"
                                                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <%--  <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                            Width="200px" ToolTip="Location">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Location"
                                                                            InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                                <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                                                                                    AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                                                    ErrorMessage="Select a Location" IsMandatory="true" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblTLEWCNo" runat="server" Text="TE/WC Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlTLEWCNO" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTLEWCNO_SelectedIndexChanged"
                                                                                    Width="200px" ToolTip="TE/WC Number">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rvfBusinessofferNo" runat="server" ControlToValidate="ddlTLEWCNO"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the TE/WC Number"
                                                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblDate" runat="server" Text="TE/WC Date"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTLEWCDate" ToolTip="Topup Date" runat="server" Width="90px" ReadOnly="true"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtStatus" ReadOnly="true" ToolTip="Topup Status" runat="server"
                                                                                    Width="90px">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblamount" runat="server" Text="Current Finance Required"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtamount" ToolTip="Current Finance Required" runat="server" Width="90px"
                                                                                    ReadOnly="true" Style="text-align: right;"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblmla" runat="server" Text="Prime Account Number "></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMLA" ToolTip="Prime Account Number" runat="server" ReadOnly="true"
                                                                                    Width="180px"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="styleFieldLabel">
                                                                                <asp:Label ID="lblsla" runat="server" Text="Sub Account Number"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtSLA" runat="server" ReadOnly="true" ToolTip="Sub Account Number"
                                                                                    Width="180px"></asp:TextBox>&nbsp;
                                                                        <input type="hidden" value="0" runat="server" id="hdnID" />
                                                                                <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go"
                                                                                    OnClick="btnGo_Click" />
                                                                                <asp:LinkButton Text="View Details" CausesValidation="false" runat="server" ID="ReqID"
                                                                                    OnClick="ReqID_serverclick" ToolTip="View Topup details"></asp:LinkButton>
                                                                                <asp:HiddenField ID="hdnTotalNoApproval" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                            <td width="50%">
                                                                <asp:Panel GroupingText="Customer Information" ID="pnlCustomerinformation" runat="server"
                                                                    CssClass="stylePanel">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="100%">
                                                                                <uc1:S3GCustomerAddress ID="S3GCustomerPermAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                    SecondColumnStyle="styleFieldAlign" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5" style="width: 100%">
                                                                <br />
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="grvApprovalDetails" runat="server" AutoGenerateColumns="false"
                                                                                OnRowDataBound="grvApprovalDetails_RowDataBound" Width="100%">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Approval No" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblApprovalSNO" Text='<%# Bind("Task_Approval_Serialvalue")%>' runat="server"></asp:Label>
                                                                                            <asp:Label ID="lblApproval_ID" Text='<%# Bind("Approval_ID")%>' Visible="false" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Approver Name" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblApprovarName" Text='<%# Bind("User_Name") %>' runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:DropDownList ID="ddlstatus" runat="server">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rvfNo" runat="server" ControlToValidate="ddlstatus"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Status"
                                                                                                InitialValue="0"></asp:RequiredFieldValidator>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="rvfNo2" runat="server" ControlToValidate="txtPassword"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Password"></asp:RequiredFieldValidator>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Approval Date" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblApprovalDate" Text='<%# Bind("Task_StatusDate") %>' runat="server"
                                                                                                MaxLength="6"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="45%">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' Height="60px"
                                                                                                Width="100%" onkeyup="maxlengthfortxt(100)" TextMode="MultiLine" onkeypress="//wraptext(this,'20')"></asp:TextBox>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5" align="center">
                                                                <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="center">
                                        <div style="margin-bottom: 10px; margin-top: 10px;">
                                            <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                                                OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" ValidationGroup="btnSave" />
                                            &nbsp;
                                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                                        OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" CausesValidation="False" />
                                            &nbsp;
                                    <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton" Text="Revoke"
                                        Enabled="false" CausesValidation="False" OnClick="btnRevoke_Click" OnClientClick="return confirm('Do you want to revoke this record?');" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" Text="Cancel"
                                                OnClick="btnCancel_Click" CausesValidation="False" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                            HeaderText="Correct the following validation(s):" ShowSummary="true" />
                                        <asp:CustomValidator ID="cvTopUpApproval" runat="server" CssClass="styleMandatoryLabel"
                                            Height="50px">
                                    
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

