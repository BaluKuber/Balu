<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Manual Journal Approval"
    CodeFile="S3GLoanAdManualJournalApproval_Add.aspx.cs" Inherits="S3GLoanAdManualJournalApproval_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" Text="Manual Journal Approval" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" align="left">
                                    <table cellpadding="0" cellspacing="0" border=".5" width="100%">
                                        <tr>
                                            <td valign="top" colspan="2">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" colspan="2">
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr runat="server" id="trCkbox">
                                                        <td colspan="5">
                                                            <table>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label1" runat="server" Text="Approval Status"></asp:Label>
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:CheckBox ID="chkUnapproval" AutoPostBack="true" Text="Unapproved" runat="server"
                                                                            OnCheckedChanged="chkUnapproval_CheckedChanged" ToolTip="Unapproved" />
                                                                        <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" TargetControlID="chkUnapproval"
                                                                            Key="1" runat="server">
                                                                        </cc1:MutuallyExclusiveCheckBoxExtender>
                                                                        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkApproved" Text="All" runat="server" OnCheckedChanged="chkApproved_CheckedChanged"
                                                                            AutoPostBack="true" ToolTip="All" />
                                                                        <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender1" TargetControlID="chkApproved"
                                                                            Key="1" runat="server">
                                                                        </cc1:MutuallyExclusiveCheckBoxExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;<table width="100%" border="0">
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
                                                                        <uc3:Suggest ID="ddlBranch" runat="server" AutoPostBack="True" ServiceMethod="GetBranchList" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                                            ErrorMessage="Select a Location" IsMandatory="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblmjvNo" runat="server" Text="MJV Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <uc3:Suggest ID="ddlMJVNO" runat="server" ServiceMethod="GetMJV" AutoPostBack="true"
                                                                                            OnItem_Selected="ddlMJVNO_SelectedIndexChanged" ErrorMessage="Select Manual Journal Vourcher Number"
                                                                                            IsMandatory="true" />
                                                                       <%-- <asp:DropDownList ID="ddlMJVNO" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMJVNO_SelectedIndexChanged"
                                                                            Width="200px" ToolTip="MJV Number">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rvfBusinessofferNo" runat="server" ControlToValidate="ddlMJVNO"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Manual Journal Vourcher Number"
                                                                            InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblDate" runat="server" Text="MJV Date"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMJVDate" runat="server" Width="90px" ReadOnly="true" ToolTip="MJV Date"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblstatus" runat="server" Text="MJV Status"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtStatus" runat="server" Width="90px" ReadOnly="true" ToolTip="MJV Status"></asp:TextBox>&nbsp;
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblvaluedate" runat="server" Text="Value Date"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtvaluedate" runat="server" Width="90px" ReadOnly="true" ToolTip="Value Date"></asp:TextBox>
                                                                        <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go"
                                                                            OnClick="btnGo_Click" ToolTip="Go" />&nbsp;
                                                                        <input type="hidden" value="0" runat="server" id="hdnID" />
                                                                        <%--<a href="#" id="ReqID" runat="server" onserverclick="ReqID_serverclick">View Details</a>--%>
                                                                        <asp:LinkButton Text="View Details" CausesValidation="false" runat="server" ID="ReqID"
                                                                            OnClick="ReqID_serverclick" ToolTip="View Details"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" colspan="5" style="width: 100%">
                                                            <br />
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="grvApprovalDetails" runat="server" AutoGenerateColumns="false"
                                                                            OnRowDataBound="grvApprovalDetails_RowDataBound" Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Approval No." ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApprovalSNO" Text='<%# Bind("Task_Approval_Serialvalue")%>' runat="server"
                                                                                            ToolTip="Approval No."></asp:Label>
                                                                                        <asp:Label ID="lblApproval_ID" Text='<%# Bind("Approval_ID")%>' Visible="false" runat="server"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Approver Name" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApprovarName" Text='<%# Bind("User_Name") %>' runat="server" ToolTip="Approver Name"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:DropDownList ID="ddlstatus" runat="server">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rvfNo" runat="server" ControlToValidate="ddlstatus"
                                                                                            ToolTip="Status" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select status"
                                                                                            InitialValue="0"></asp:RequiredFieldValidator>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password" ToolTip="Password"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="rvfNo2" runat="server" ControlToValidate="txtPassword"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter password"></asp:RequiredFieldValidator>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Approval Date" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApprovalDate" Text='<%# Bind("Task_StatusDate") %>' runat="server"
                                                                                            ToolTip="Approval Date" MaxLength="6"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="45%">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' Height="60px"
                                                                                            ToolTip="Remarks" Width="100%" onkeyup="maxlengthfortxt(100)" TextMode="MultiLine"></asp:TextBox>
                                                                                        <%--  Commented based on UAT round III / Case ID - MJVA_002  --%>
                                                                                        <%--onkeypress="wraptext(this,'20')--%>
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
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                                        ToolTip="Save" OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click"
                                        ValidationGroup="btnSave" />
                                    <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton" Text="Revoke"
                                        ToolTip="Revoke" CausesValidation="False" OnClick="btnRevoke_Click" OnClientClick="return confirm('Do you want to revoke this record?');" />
                                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                                        ToolTip="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                                        CausesValidation="False" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" Text="Cancel"
                                        ToolTip="Cancel" OnClick="btnCancel_Click" CausesValidation="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Please correct the following validation(s):" ShowSummary="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
