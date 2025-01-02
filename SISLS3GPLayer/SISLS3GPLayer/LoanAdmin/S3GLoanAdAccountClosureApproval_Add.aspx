<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Rental Schedule Closure Approval"
    CodeFile="S3GLoanAdAccountClosureApproval_Add.aspx.cs" Inherits="S3GLoanAdAccountClosureApproval_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading" width="100%">
                        <asp:Label runat="server" Text="Rental Schedule Closure Approval" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr runat="server" id="trCkbox">
                                <td colspan="2" align="left" valign="bottom">
                                    <asp:Label ID="Label1" runat="server" Text="Approval Status"></asp:Label>
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
                            <tr>
                                <td width="50%" align="left">
                                    <asp:Panel GroupingText="Rental Schedule Closure Details" ID="pnltopupDetails" runat="server"
                                        CssClass="stylePanel">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddllLineOfBusiness" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddllLineOfBusiness_SelectedIndexChanged"
                                                        Width="200px" ToolTip="All">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="ddllLineOfBusiness" InitialValue="0" ErrorMessage="Select the Line of Business"
                                                        Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBranch" Visible="false" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                        Width="200px" ToolTip="Location">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="ddlBranch" InitialValue="0" ErrorMessage="Select the Location"
                                                        Display="None"></asp:RequiredFieldValidator>--%>
                                                    <uc2:Suggest ID="ddlBranch" Visible="false" runat="server" ToolTip="Location"
                                                        ServiceMethod="GetBranchList" AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                        ErrorMessage="Select a Location" IsMandatory="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountClosureNo" runat="server" Text="Rental Schedule Closure No." CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlACNo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlACNo_SelectedIndexChanged"
                                                        Width="200px" ToolTip="Account Closure No.">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="ddlACNo" InitialValue="0" ErrorMessage="Select Account Closure Number"
                                                        Display="None"></asp:RequiredFieldValidator>--%>
                                                    <uc2:Suggest ID="ddlACNo" runat="server" ServiceMethod="GetACCNumber" AutoPostBack="true"
                                                        OnItem_Selected="ddlACNo_SelectedIndexChanged" ErrorMessage="Select Account Closure Number"
                                                        IsMandatory="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblACDate" runat="server" Text="Rental Schedule Closure Date"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtACDate" runat="server" ReadOnly="true" ToolTip="Account Closure Date"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblTranche" runat="server" Text="Tranche Name"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTranche" runat="server" ReadOnly="true" ToolTip="Tranche Name"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblmla" runat="server" Text="Rental Schedule Number "></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMLA" runat="server" ReadOnly="true" ToolTip="Rental Schedule Number"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblsla" runat="server" Text="Sub Account Number" Visible="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSubAccountNo" runat="server" Width="200px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlSubAccountNo_SelectedIndexChanged" ToolTip="Sub Account Number" Visible="false">
                                                    </asp:DropDownList>
                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStatus" runat="server" Text="Rental Schedule Closure Status"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStatus" ReadOnly="true" runat="server" ToolTip="Account Closure Status">
                                                    </asp:TextBox>
                                                    <input type="hidden" value="0" runat="server" id="hdnID" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td align="center" colspan="2" style="height: 60px">
                                                    <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitShortButton" OnClick="btnGo_Click"
                                                        Text="Go" ToolTip="Go" />
                                                    <asp:LinkButton Text="View Details" CausesValidation="false" runat="server" ID="ReqID"
                                                        OnClick="ReqID_serverclick" ToolTip="View Details"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td width="50%" valign="top">
                                    <asp:Panel GroupingText="Lessee Information" ID="pnlCustomerinformation" runat="server"
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
                                <td colspan="2">
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
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlstatus" runat="server" ToolTip="Action">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="ddlstatus" InitialValue="0" ErrorMessage="Select Status"
                                                        Display="None"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password" ToolTip="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPassword" ErrorMessage="Enter Password"
                                                        Display="None"></asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Approval Date" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblApprovalDate" Text='<%# Bind("Task_StatusDate") %>' runat="server"
                                                        MaxLength="6" ToolTip="Approval Date"></asp:Label>
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
                            <tr>
                                <td colspan="5" align="center">
                                    <input id="hdnClsDtlsID" visible="false" runat="server" value="0" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
