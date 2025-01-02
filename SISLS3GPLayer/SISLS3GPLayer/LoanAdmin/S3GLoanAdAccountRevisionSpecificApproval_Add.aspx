<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Account Revision Specific Approval"
    CodeFile="S3GLoanAdAccountRevisionSpecificApproval_Add.aspx.cs" Inherits="S3GLoanAdAccountRevisionSpecificApproval_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server" ID="updMain">
        <ContentTemplate>
           <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" class="stylePageHeading">
                <asp:Label runat="server" Text="Account Revision Specific Approval" ID="lblHeading"
                    CssClass="styleDisplayLabel">
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
                                                    <table>
                                                        <tr>
                                                            <br />
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label4" runat="server" Text="Approval Status"></asp:Label>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:CheckBox ID="chkUnapproval" AutoPostBack="true" Text="Unapproved" runat="server"
                                                                    OnCheckedChanged="chkUnapproval_CheckedChanged" />
                                                                <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" TargetControlID="chkUnapproval"
                                                                    Key="1" runat="server">
                                                                </cc1:MutuallyExclusiveCheckBoxExtender>
                                                                &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkApproved" Text="All" runat="server" OnCheckedChanged="chkApproved_CheckedChanged"
                                                                    AutoPostBack="true" />
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
                                                    <asp:Panel GroupingText="Account Revision Specific Details" ID="pnltopupDetails"
                                                        runat="server" CssClass="stylePanel">
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
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <%-- <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                        Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Location"
                                                                        InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                    <uc2:suggest id="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                        ErrorMessage="Select a Location" IsMandatory="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblArNo" runat="server" Text="AR Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlARNO" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlARNO_SelectedIndexChanged"
                                                                        Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rvfBusinessofferNo" runat="server" ControlToValidate="ddlARNO"
                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select AR NO" InitialValue="0"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblDate" runat="server" Text="AR Date"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtARDate" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblmla" runat="server" Text="Prime Account Number "></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtMLA" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblsla" runat="server" Text="Sub Account Number"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtSLA" runat="server"></asp:TextBox>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblFinAmtOption" runat="server" Text="Finance Amount"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButton ID="Rdadd" runat="server" GroupName="A" Text="Add" Enabled="false" />
                                                                    <asp:RadioButton ID="Rdless" runat="server" GroupName="A" Text="Less" Enabled="false" />
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lbladdorless" runat="server" Text="Add/Less Finance Amount"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFinAmount" runat="server" ContentEditable="False"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblStatus" runat="server" Text="Revision Status"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtStatus" ContentEditable="false" runat="server">
                                                                    </asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblrevisioneff" runat="server" Text="Revision Eff.From"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRevisionEffective" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblreate" runat="server" Text="Rate"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkRate" Text="Rate" runat="server" Enabled="false" />
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="Label1" runat="server" Text="Rate"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtRate" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblamount" runat="server" Text="Finance Amount"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkAmount" Text="Finance Amount" runat="server" Enabled="false" />
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="Label2" runat="server" Text="Finance Amount"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtFinanceAmount" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lbltenure" runat="server" Text="Tenure"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="ChkTenure" Text="Tenure" runat="server" Enabled="false" />
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="Label3" runat="server" Text="Tenure"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTenure" runat="server"></asp:TextBox>
                                                                    <asp:Button ID="btnGo" CssClass="styleSubmitButton" runat="server" Text="Go" OnClick="btnGo_Click"
                                                                        Width="25px" />
                                                                    <input type="hidden" value="0" runat="server" id="hdnID" />
                                                                    <asp:LinkButton Text="View Details" CausesValidation="false" runat="server" ID="ReqID"
                                                                        OnClick="ReqID_serverclick"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel GroupingText="Customer Information" ID="Panel1" runat="server" CssClass="stylePanel">
                                                        <table>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblcustomerCode" runat="server" Text="Customer Code"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustomerCode" ReadOnly="true" runat="server">
                                                                    </asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblCustomerAddress1" runat="server" Text="Address1"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustomerAddress1" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblCustomerAddress2" runat="server" Text="Address2"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustomerAddress2" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                </td>
                                                                <td align="left">
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
                                                                                    Width="100%" onkeyup="maxlengthfortxt(100)" TextMode="MultiLine" onkeypress="wraptext(this,'20')"></asp:TextBox>
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
                            &nbsp;
                            <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton" Text="Revoke"
                                CausesValidation="False" Enabled="false" OnClick="btnRevoke_Click" OnClientClick="return confirm('Do you want to revoke this record?');" />
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
