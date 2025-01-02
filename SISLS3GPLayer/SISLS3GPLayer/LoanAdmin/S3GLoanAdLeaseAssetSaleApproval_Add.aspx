<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Lease Asset Sale Approval"
    CodeFile="S3GLoanAdLeaseAssetSaleApproval_Add.aspx.cs" Inherits="S3GLoanAdLeaseAssetSaleApproval_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" Text="Lease Asset Sale Approval" ID="lblHeading" CssClass="styleDisplayLabel">
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
                                                            <asp:Panel GroupingText="Lease Asset Sale Details" ID="pnltopupDetails" runat="server"
                                                                CssClass="stylePanel">
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
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <%--  <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                                Width="200px" ToolTip="Location">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Location"
                                                                                InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                            <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ErrorMessage="Enter a Location"  Visible="false"/>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblLASNO" runat="server" Text="LAS Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlLASNO" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLASNO_SelectedIndexChanged"
                                                                                Width="200px" ToolTip="LAS Number">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rvfBusinessofferNo" runat="server" ControlToValidate="ddlLASNO"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the LAS Number"
                                                                                InitialValue="0"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblDate" runat="server" Text="LAS Date"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtLASDate" runat="server" ReadOnly="true" Width="90px" ToolTip="LAS Date"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblStatus" runat="server" Text="LAS Status"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtStatus" ReadOnly="true" runat="server" Width="80px" ToolTip="LAS Status">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go"
                                                                                OnClick="btnGo_Click" ToolTip="Go" />
                                                                            <input type="hidden" value="0" runat="server" id="hdnID" />&nbsp;
                                                                            <asp:LinkButton Text="View Details" CausesValidation="false" runat="server" ID="ReqID"
                                                                                OnClick="ReqID_serverclick" ToolTip="View Details"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
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
                                                                                        <asp:DropDownList ID="ddlstatus" Width="95%" runat="server" ToolTip="Status">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rvfNo" runat="server" ControlToValidate="ddlstatus"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Status"
                                                                                            InitialValue="0"></asp:RequiredFieldValidator>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" Width="95%" TextMode="Password" ToolTip="Password"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="rvfNo2" runat="server" ControlToValidate="txtPassword"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Password"></asp:RequiredFieldValidator>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Approval Date" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApprovalDate" Text='<%# Bind("Task_StatusDate") %>' runat="server"
                                                                                            MaxLength="6" ToolTip="Approval Date"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                 <asp:TemplateField HeaderText="Type" ItemStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblType" Text='<%# Bind("Type") %>' runat="server"
                                                                                            ToolTip="Type"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="45%">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' Height="40px"
                                                                                            Width="100%" onkeyDown="maxlengthfortxt(100)" TextMode="MultiLine" ToolTip="Remarks"></asp:TextBox>
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
                                        OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" ValidationGroup="btnSave"
                                        ToolTip="Save" />
                                    <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton" Text="Revoke"
                                        ToolTip="Revoke" CausesValidation="False" OnClick="btnRevoke_Click" OnClientClick="return confirm('Do you want to revoke this record?');" />
                                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                                        ToolTip="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                                        CausesValidation="False" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" Text="Cancel"
                                        OnClick="btnCancel_Click" CausesValidation="False" ToolTip="Cancel" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
