<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Payment Approval" CodeFile="S3GLoanAdPaymentApproval_Add.aspx.cs"
    Inherits="S3GLoanAdPaymentApproval_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="Up" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="Payment Approval" ID="lblHeading" CssClass="styleDisplayLabel">
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
                                            <td class="styleFieldLabel" colspan="2">
                                                <table>
                                                    <caption>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label1" runat="server" Text="Approval Status"></asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:CheckBox ID="chkUnapproval" runat="server" AutoPostBack="true" OnCheckedChanged="chkUnapproval_CheckedChanged" Text="Unapproved" ToolTip="Unapproved" />
                                                                <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" runat="server" Key="1" TargetControlID="chkUnapproval">
                                                                </cc1:MutuallyExclusiveCheckBoxExtender>
                                                                &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkApproved" runat="server" AutoPostBack="true" OnCheckedChanged="chkApproved_CheckedChanged" Text="All" ToolTip="All" />
                                                                <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender1" runat="server" Key="1" TargetControlID="chkApproved">
                                                                </cc1:MutuallyExclusiveCheckBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </caption>
                                                </table>
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td width="50%">
                                                            <asp:Panel GroupingText="Payment Approval Details" ID="pnlPricingDetails" runat="server"
                                                                CssClass="stylePanel">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td><%--AutoPostBack="True" OnSelectedIndexChanged="ddllLineOfBusiness_SelectedIndexChanged"--%>
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
                                                                            <%--<asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                        Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Location"
                                                                        InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                            <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                ErrorMessage="Select a Location" IsMandatory="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblPaymentreqno" runat="server" Text="Request Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <uc2:Suggest ID="ddlPaymentReqNumber" runat="server" ServiceMethod="GetApplications" AutoPostBack="true"
                                                                                OnItem_Selected="ddlPaymentReqNumber_SelectedIndexChanged" ErrorMessage="Select the Request Number"
                                                                                IsMandatory="true" />
                                                                            <%--<asp:DropDownList ID="ddlPaymentReqNumber" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPaymentReqNumber_SelectedIndexChanged"
                                                                        Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rvfBusinessofferNo" runat="server" ControlToValidate="ddlPaymentReqNumber"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Request Number"
                                                                        InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblPaymentReqDate" runat="server" Text="Request Date"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPaymentReqDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblamount" runat="server" Text="Document Amount"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtamount" runat="server" Width="100px" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblpaymode" runat="server" Text="Pay Mode" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPayMode" runat="server" ReadOnly="true" Width="80px" Visible="false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblCustomerName" runat="server" Text="Pay To" Visible="false"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtpayto" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblEntityCode" runat="server" Text="Entity Code"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEntityCode" runat="server" ReadOnly="true" Style="text-align: left"> </asp:TextBox>
                                                                            <input type="hidden" value="0" runat="server" id="hdnID" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-bottom: 30px" class="styleFieldLabel">
                                                                            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-bottom: 30px">
                                                                            <asp:TextBox ID="txtStatus" ReadOnly="true" runat="server" Width="90px">
                                                                            </asp:TextBox>
                                                                            <asp:Button ID="btnGo" CssClass="styleGridShortButton" runat="server" Text="Go" OnClick="btnGo_Click" />
                                                                            <asp:LinkButton Text="View Details" CausesValidation="false" runat="server" ID="PaymentReqID"
                                                                                OnClick="PaymentReqID_serverclick"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="50%">
                                                            <asp:Panel GroupingText="Entity Information" ID="pnlCustomerDetails" runat="server"
                                                                CssClass="stylePanel">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td width="100%">
                                                                            <table width="100%" cellspacing="0">
                                                                                <tr>
                                                                                    <%--<td style="width: 144px">
                                                                                &nbsp;&nbsp;&nbsp;
                                                                                <asp:Label ID="lblCustomerCode" runat="server" CssClass="styleDisplayLabel" Text="Entity Code"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;<asp:TextBox ID="txtCustomerCode" runat="server" ReadOnly="True" Width="143px"
                                                                                    ToolTip="Code" Style="margin-left: 32px"></asp:TextBox>
                                                                            </td>--%>
                                                                                </tr>
                                                                                <tr>
                                                                                    <%--<td style="width: 144px">
                                                                                &nbsp;&nbsp;&nbsp;
                                                                                <asp:Label ID="Label1" runat="server" CssClass="styleDisplayLabel" Text="Name">
                                                                                </asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                                <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="True" Style="margin-left: 29px"
                                                                                    ToolTip="Name" Width="216px"></asp:TextBox>
                                                                            </td>--%>
                                                                                </tr>
                                                                            </table>
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerPermAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                SecondColumnStyle="styleFieldAlign" Caption="Entity" />
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
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Status"
                                                                                            InitialValue="0"></asp:RequiredFieldValidator>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password" onkeypress="return (event.keyCode != 32&&event.which!=32)"></asp:TextBox>
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
                                     <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton" Text="Revoke"
                                        ToolTip="Revoke" CausesValidation="False" OnClick="btnRevoke_Click" OnClientClick="return confirm('Do you want to revoke this record?');" />
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
