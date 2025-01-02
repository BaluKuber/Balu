<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Account Split Approval"
    CodeFile="S3GLoanAdAccountSplitApproval_Add.aspx.cs" Inherits="S3GLoanAdAccountSplitApproval_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" Text="Rental Schedule Split Approval" ID="lblHeading" CssClass="styleDisplayLabel">
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
                                                    <tr>
                                                        <td colspan="5">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%">
                                                            <asp:Panel GroupingText="Rental Schedule Split Details" ID="pnltopupDetails" runat="server"
                                                                CssClass="stylePanel">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddllLineOfBusiness" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddllLineOfBusiness_SelectedIndexChanged"
                                                                                Width="200px">
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
                                                                            <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                                                                                AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                                                ErrorMessage="Select a Location" IsMandatory="true" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblsplitNo" runat="server" Text="Split Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlSplitNO" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSplitNO_SelectedIndexChanged"
                                                                                Width="200px">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rvfBusinessofferNo" runat="server" ControlToValidate="ddlSplitNO"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Split Number"
                                                                                InitialValue="0"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblDate" runat="server" Text="Split Date"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDate" runat="server" ReadOnly="true" Width="90px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtStatus" ReadOnly="true" runat="server" Width="90px">
                                                                            </asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel"></td>
                                                                        <td style="height: 79px">
                                                                            <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go" OnClick="btnGo_Click" />
                                                                            <input type="hidden" value="0" runat="server" id="hdnID" />
                                                                            <asp:LinkButton Text="View Details" CausesValidation="false" runat="server" ID="ReqID"
                                                                                OnClick="ReqID_serverclick"></asp:LinkButton>
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
                                                        <td class="styleFieldLabel" colspan="5" style="width: 100%">
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
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select status" InitialValue="0"></asp:RequiredFieldValidator>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="rvfNo2" runat="server" ControlToValidate="txtPassword"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter password"></asp:RequiredFieldValidator>
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
                                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' Height="40px"
                                                                                            Width="100%" onkeyDown="maxlengthfortxt(100)" TextMode="MultiLine"></asp:TextBox>
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
                                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                                        OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" ValidationGroup="btnSave" />
                                    &nbsp;
                            <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                                OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" CausesValidation="False" />
                                    &nbsp;
                            <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" Text="Cancel"
                                OnClick="btnCancel_Click" CausesValidation="False" />
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
