<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_FUNDMGT_FUNDERAPPROVAL_ADD.aspx.cs"
    Inherits="Fund_Management_S3G_FUNDMGT_FUNDERAPPROVAL_ADD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
        function fnSelectAll(chkSelectAllBranch, chkSelectBranch) {
            var gvBranchWise = document.getElementById('<%=gvSanctionDtl.ClientID%>');
            var TargetChildControl = chkSelectBranch;
            //Get all the control of the type INPUT in the base control.
            var Inputs = gvBranchWise.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllBranch.checked;
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" Text="Funder Limit Approval" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" align="left">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <table cellspacing="0" border="0" width="100%">

                                                    <tr>
                                                        <td>
                                                            <table width="100%">
                                                                <tr valign="top">
                                                                    <td width="50%">
                                                                        <asp:Panel GroupingText="Funder Details" ID="pnlFunderDetails" runat="server" CssClass="stylePanel">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td class="styleFieldLabel" style="width: 23%">
                                                                                        <asp:Label ID="lblFunderCode" runat="server" Text="Funder Name"
                                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <uc2:Suggest ID="ddlFunderCode" runat="server" ServiceMethod="GetFunderCode" Width="250px"
                                                                                            AutoPostBack="true" OnItem_Selected="ddlFunderCode_Item_Selected"
                                                                                            ErrorMessage="Select the Funder Name" IsMandatory="true" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldLabel" style="width: 23%">
                                                                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name"
                                                                                            CssClass="styleReqFieldLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <uc2:Suggest ID="ddlLessee" runat="server" ServiceMethod="GetLesseeList" Width="250px" AutoPostBack="true"
                                                                                            ErrorMessage="Select the Lessee Name" IsMandatory="true" OnItem_Selected="ddlLessee_Item_Selected" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblFunderDate" runat="server" Text="Creation Date" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtCreationDate" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtStatus" ReadOnly="true" runat="server" Width="100px">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center" colspan="2" style="height: 78px">
                                                                                        <asp:HiddenField ID="hProductID" runat="server" Visible="false" />
                                                                                        <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go"
                                                                                            OnClick="btnGo_Click" />
                                                                                        <input type="hidden" value="0" runat="server" id="hdnID" />
                                                                                        <asp:LinkButton Text="View Sanction" CausesValidation="false" runat="server" ID="PaymentReqID"
                                                                                            OnClick="PaymentReqID_serverclick"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td width="50%">
                                                                        <asp:Panel GroupingText="Funder Information" ID="pnlFunderinformation" runat="server"
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
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 100%">
                                                            <asp:Panel GroupingText="Sanction Details" ID="pnlSanctionDtls" runat="server" CssClass="stylePanel" Visible="false">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="gvSanctionDtl" runat="server" AutoGenerateColumns="false" Width="100%">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="S.No" ItemStyle-Width="10%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSNo" Text='<%# Container.DataItemIndex+1%>' runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Sanction Number" ItemStyle-Width="25%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSanctionNo" Text='<%# Bind("Sanction_No")%>' runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Sanction Date" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSanctionDate" Text='<%# Bind("Sanction_Date")%>' runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Sanction Amount" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSanctionAmount" Text='<%# Bind("Sanction_Amount")%>' runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Discounting Rate" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDiscountRate" Text='<%# Bind("Discount_Rate")%>' runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Select Indicator" ItemStyle-Width="10%">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblSelect" runat="server" Text="Select Indicator"></asp:Label>
                                                                                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:fnSelectAll(this,'chkSelect');" />
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>

                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 100%">
                                                            <br />
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="grvApprovalDetails" runat="server" AutoGenerateColumns="false"
                                                                            OnRowDataBound="grvApprovalDetails_RowDataBound" Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Approval No" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblApprovalSNO" Text='<%# Bind("Approval_Serial_Number")%>' runat="server"></asp:Label>
                                                                                        <asp:Label ID="lblFunderApproval_ID" Text='<%# Bind("FunderApproval_ID")%>' Visible="false"
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
                                                                                            InitialValue="0"></asp:RequiredFieldValidator>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="ftxtPassword" runat="server" TargetControlID="txtPassword"
                                                                                            FilterType="Custom" FilterMode="InvalidChars" InvalidChars=" ">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Password"></asp:RequiredFieldValidator>
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
                                                    <tr>
                                                        <td align="center">
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
                                <td align="center">
                                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                                        OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" ValidationGroup="btnSave" />
                                    <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton" Visible="false"
                                        Text="Revoke" OnClientClick="return fnCheckPageValidators_Extn();" />
                                    <%--       <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" TargetControlID="btnRevoke"
                                        ConfirmText="Do you want to Revoke?" runat="server">
                                    </cc1:ConfirmButtonExtender>--%>
                                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                                        OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" CausesValidation="False" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" Text="Cancel"
                                        OnClick="btnCancel_Click" CausesValidation="False" />
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

