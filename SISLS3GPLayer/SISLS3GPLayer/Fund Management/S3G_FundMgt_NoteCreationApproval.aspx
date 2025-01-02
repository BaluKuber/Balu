<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_FundMgt_NoteCreationApproval.aspx.cs" Inherits="Fund_Management_S3G_FundMgt_NoteCreationApproval" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr height="10px" runat="server" id="trCkbox">
                                <td colspan="4">
                                    <table>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="Label2" runat="server" Text="Approval Status"></asp:Label>
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
                            <tr height="30px">
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="lblFunder" runat="server" Text="Funder"></asp:Label>
                                </td>
                                <td class="styleFieldLabel" width="25%">
                                    <uc2:Suggest ID="ddlFunder" runat="server" ServiceMethod="GetFunderLst"
                                        OnItem_Selected="ddlFunder_Item_Selected" AutoPostBack="true" />
                                </td>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="lblCustomer" runat="server" Text="Customer"></asp:Label>
                                </td>
                                <td class="styleFieldLabel" width="25%">
                                    <uc2:Suggest ID="ddlCustomer" runat="server" ServiceMethod="GetCustomerLst"
                                        OnItem_Selected="ddlCustomer_Item_Selected" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>

                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="lblTranche" runat="server" Text="Tranche Number"></asp:Label>
                                </td>
                                <td class="styleFieldLabel">
                                     <uc2:Suggest ID="ddlTranche" runat="server" ServiceMethod="GetTrancheNumbers"
                                        AutoPostBack="true" OnItem_Selected="ddlTranche_SelectedIndexChanged"/>
                                </td>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="Label1" runat="server" Text="Status"></asp:Label>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:TextBox ID="txtStatus" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>

                            </tr>
                            <tr height="30px">

                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="lblnote" runat="server" Text="Note Number"></asp:Label>
                                </td>
                                <td class="styleFieldLabel" width="25%">
                                    <uc2:Suggest ID="ddlNoteNumber" runat="server" ServiceMethod="GetNoteNumbers"
                                        AutoPostBack="true" OnItem_Selected="ddlNoteNumber_SelectedIndexChanged"
                                        ErrorMessage="Select the Note Number" IsMandatory="true" />
                                </td>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="lblNoteDate" runat="server" Text="Note Creation Date"></asp:Label>
                                </td>
                                <td class="styleFieldLabel" width="25%">
                                    <asp:TextBox ID="txtnote" runat="server" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">

                            <tr align="center">
                                <td align="center" colspan="4" style="height: 78px">
                                    <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go"
                                        OnClick="btnGo_Click" />
                                    <input type="hidden" runat="server" id="hdnID" />
                                    <asp:LinkButton Text="View Note" CausesValidation="false" runat="server" ID="noteid"
                                        OnClick="NoteIdID_serverclick"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td class="styleFieldLabel" style="width: 100%">
                                    <br />
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grvApprovalDetails" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvApprovalDetails_RowDataBound"
                                                    Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Approval No" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblApprovalSNO" Text='<%# Bind("Approval_Serial_Number")%>' runat="server"></asp:Label>
                                                                <asp:Label ID="lblNoteApproval_ID" Text='<%# Bind("NoteApproval_ID")%>' Visible="false"
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Approver Name" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblApprovarName" Text='<%# Bind("User_Name") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlAction" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvAction" runat="server" ControlToValidate="ddlAction"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Action"
                                                                    InitialValue="0" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftxtPassword" runat="server" TargetControlID="txtPassword"
                                                                    FilterType="Custom" FilterMode="InvalidChars" InvalidChars=" ">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Password" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Approval Date" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblApprovalDate" Text='<%# Bind("Approvaldate") %>' runat="server"
                                                                    MaxLength="6"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="45%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemarks" runat="server" Text='<%# Bind("Remarks") %>' Height="40px"
                                                                    Width="100%" onkeyDown="maxlengthfortxt(100)" TextMode="MultiLine"></asp:TextBox>
                                                                <%--   <asp:RequiredFieldValidator ID="rfvremarks" runat="server" ControlToValidate="txtRemarks"onkeypress="wraptext(this,'20')"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Remarks"></asp:RequiredFieldValidator>--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                                        OnClientClick="return fnCheckPageValidators();" ValidationGroup="vsSave" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton"
                                        Text="Revoke" OnClientClick="return fnCheckPageValidators_Extn();" OnClick="btnRevoke_Click"/>
                                           <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" TargetControlID="btnRevoke"
                                        ConfirmText="Do you want to Revoke?" runat="server">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                                        OnClientClick="return fnConfirmClear();" CausesValidation="False" OnClick="btnClear_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" Text="Cancel"
                                        CausesValidation="False" OnClick="btnCancel_Click" />
                                    <asp:Button ID="Button1" runat="server" CssClass="styleSubmitButton" Text="Save"
                                        CausesValidation="False" OnClick="Button1_Click" Style="display: none;" />

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vsSave" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" Height="177px" ShowMessageBox="false"
                                        ShowSummary="true" ValidationGroup="vsSave" Width="500px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button1" />
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

