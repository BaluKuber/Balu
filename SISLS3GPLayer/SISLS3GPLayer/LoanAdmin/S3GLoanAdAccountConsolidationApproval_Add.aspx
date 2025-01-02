<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Account Consolidation Approval"
    CodeFile="S3GLoanAdAccountConsolidationApproval_Add.aspx.cs" Inherits="S3GLoanAdAccountConsolidationApproval_Add"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" class="stylePageHeading">
                <asp:Label runat="server" Text="Account Consolidation Approval" ID="lblHeading" CssClass="styleDisplayLabel">
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
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="50%">
                                                    <asp:Panel GroupingText="Account Consolidation Details" ID="pnltopupDetails" runat="server"
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
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" CssClass="styleMandatoryLabel"
                                                                        runat="server" ControlToValidate="ddllLineOfBusiness" InitialValue="0" ErrorMessage="Select Line of Business"
                                                                        Display="None"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                        Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel"
                                                                        runat="server" ControlToValidate="ddlBranch" InitialValue="0" ErrorMessage="Select Location"
                                                                        Display="None"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblconsolidationNo" runat="server" Text="Consolidation Number" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlConsolidationNO" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlConsolidationNO_SelectedIndexChanged"
                                                                        Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel"
                                                                        runat="server" ControlToValidate="ddlConsolidationNO" InitialValue="0" ErrorMessage="Select Account Consolidation Number"
                                                                        Display="None"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblDate" runat="server" Text="Consolidation Date"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" Width="90px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtStatus" ReadOnly="true" runat="server" Width="90px">
                                                                    </asp:TextBox>
                                                                    <asp:LinkButton ID="ReqID" runat="server" CausesValidation="false" 
                                                                        OnClick="ReqID_serverclick" Text="View Details"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                </td>
                                                                <td style="height:79px">
                                                                    <input type="hidden" value="0" runat="server" id="hdnID" />
                                                                    <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go" OnClick="btnGo_Click"
                                                                         />
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
                                                                    <uc1:s3gcustomeraddress id="S3GCustomerPermAddress" runat="server" firstcolumnstyle="styleFieldLabel"
                                                                        secondcolumnstyle="styleFieldAlign" />
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
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" CssClass="styleMandatoryLabel"
                                                                                    runat="server" ControlToValidate="ddlstatus" InitialValue="0" ErrorMessage="Select the Status"
                                                                                    Display="None"></asp:RequiredFieldValidator>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" CssClass="styleMandatoryLabel"
                                                                                    runat="server" ControlToValidate="txtPassword" ErrorMessage="Enter the Password"
                                                                                    Display="None"></asp:RequiredFieldValidator>
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
                                                                                    Width="100%" onkeyup="maxlengthfortxt(100)" TextMode="MultiLine"></asp:TextBox>
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
</asp:Content>
