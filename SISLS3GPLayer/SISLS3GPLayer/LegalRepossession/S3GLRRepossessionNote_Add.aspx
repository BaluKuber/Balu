<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLRRepossessionNote_Add.aspx.cs" Inherits="LegalRepossession_S3GLRRepossessionNote_Add"
    Title="Untitled Page" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <table width="100%" class="stylePageHeading" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="height: 19px">
                                    <asp:Label runat="server" Text="Repossession Note - Create" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updatepanel2" runat="server">
                            <ContentTemplate>
                                <cc1:TabContainer ID="tcLRNote" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                    Width="99%" ScrollBars="Auto" meta:resourcekey="tcEntityMasterResource1">
                                    <cc1:TabPanel runat="server" HeaderText="LR Note Details" ID="tpLRNOTEDetails" CssClass="tabpan"
                                        BackColor="Red" meta:resourcekey="tbEntityResource1">
                                        <HeaderTemplate>
                                            LR NOTE Details &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <table width="99%">
                                                <tr>
                                                    <td width="100%" valign="top">
                                                        <table width="100%" border="0">
                                                            <tr>
                                                                <td width="50%">
                                                                    <asp:Panel ID="LRNoteInfo" runat="server" GroupingText="Legal Repossession Note Info"
                                                                        CssClass="stylePanel">
                                                                        <table border="0" height="215px">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                                        AutoPostBack="True">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                                        Display="None" InitialValue="0" ErrorMessage="Select a Line of Business" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblMLA" runat="server" CssClass="styleReqFieldLabel" Text="Prime Account"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <uc2:Suggest ID="ddlMLA" runat="server" ServiceMethod="GetPANUM" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlMLA_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit"
                                                                                        ErrorMessage="Select a Prime Account"/>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblSLA" runat="server" CssClass="styleReqFieldLabel" Text="Sub Account"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlSLA" AutoPostBack="True" runat="server" Width="165px" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:Button ID="btnAccount" runat="server" CssClass="styleSubmitButton" Text="View Account"
                                                                                        Visible="False" Width="102px" OnClick="btnAccount_Click" />
                                                                                    <asp:RequiredFieldValidator ID="rfvSLA" runat="server" ControlToValidate="ddlSLA"
                                                                                        Display="None" ErrorMessage="Select a Sub Account" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblAction" runat="server" CssClass="styleReqFieldLabel" Text="Action"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlAction" runat="server" Width="165px">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvMode" runat="server" ControlToValidate="ddlAction"
                                                                                        Display="None" ErrorMessage="Select a Action" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblLRNo" runat="server" CssClass="styleDisplayLabel" Text="LR No"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtLRNo" CssClass="styleAutoGenerated" runat="server" ReadOnly="True"></asp:TextBox>
                                                                                    <asp:LinkButton ID="lnkBtnViewExistingLRN" runat="server" OnClick="lnkBtnViewExistingLRN_Click"></asp:LinkButton>
                                                                                    <asp:Label ID="lblMappedLRNId" runat="server" Visible="False"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblLRDate" runat="server" CssClass="styleReqFieldLabel" Text="LR Date"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtLRDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td width="50%">
                                                                    <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
                                                                        CssClass="stylePanel">
                                                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                            SecondColumnStyle="styleFieldAlign" />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="100%" colspan="4">
                                                                    <asp:Panel ID="Panel1" runat="server" GroupingText="Action Points" CssClass="stylePanel"
                                                                        Width="100%">
                                                                        <table width="100%" border="0" cellspacing="0">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblActionPoints" AssociatedControlID="txtActionPoints" CssClass="styleDisplayLabel" Text="Action Points"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtActionPoints" runat="server" TextMode="MultiLine" MaxLength="800"
                                                                                        onkeyup="maxlengthfortxt(800);" Width="560px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblFlUpEmpID" runat="server" CssClass="styleReqFieldLabel" Text="Follow Up Person Emp ID"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <uc2:Suggest ID="ddlFollowUPEMPID" runat="server" ServiceMethod="GetUsers"
                                                                        AutoPostBack="true" OnItem_Selected="ddlFollowUPEMPID_SelectedIndexChanged" IsMandatory="true"
                                                                        ValidationGroup="Submit" ErrorMessage="Select Follow Up Person Emp ID" />
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblFlUpEmpName" runat="server" Visible = "False" CssClass="styleDisplayLabel" Text="Follow Up Person Name"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtFlUpEmpName" runat="server" Visible = "False" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblLRStatus" runat="server" CssClass="styleDisplayLabel" Text="LR Status"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtLRStatus" runat="server" ReadOnly="True" Width="165px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                    <cc1:TabPanel runat="server" HeaderText="LR Note Approval" ID="tpLRApproval" CssClass="tabpan"
                                        BackColor="Red" meta:resourcekey="tpLRApprovalResource1">
                                        <HeaderTemplate>
                                            LR NOTE Approval
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlLRNoteApproval" runat="server">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblApprovalName" runat="server" Text="Approval Name" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtApprovalName" runat="server" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="165px">
                                                            </asp:DropDownList>
                                                            <%--<asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                                                Display="None" ErrorMessage="Select Status" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblApprovalDate" runat="server" Text="Approval Date" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtApprovalDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                                                Display="None" ErrorMessage="Select Status" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblPassword" runat="server" Text="Password" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                            <%-- <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                                Display="None" ErrorMessage="Enter Password" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" colspan="3">
                                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" onkeyup="maxlengthfortxt(100);"
                                                                TextMode="MultiLine" Columns="60"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlApprovalHistory" runat="server" CssClass="stylePanel" GroupingText="Approval History"
                                                Width="99%">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <div id="div11" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                <asp:Label ID="lblGuarDetails" runat="server" Text="No Approval details available"
                                                                    Visible="false" />
                                                                <asp:GridView ID="gvApprovalHistory" runat="server" Width="100%">
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </cc1:TabPanel>
                                </cc1:TabContainer>
                                <table width="100%">
                                    <tr>
                                        <td width="100%">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td align="center" width="100%">
                                                        <asp:Button ID="btnSave" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                                                            Text="Save" ValidationGroup="Submit" OnClientClick="return fnCheckPageValidators('Submit');"
                                                            OnClick="btnSave_Click" />
                                                        <asp:Button ID="btnClear" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                                                            Text="Clear" OnClick="btnClear_Click" />
                                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                                                            Text="Cancel" OnClick="btnCancel_Click" />
                                                        <asp:Button ID="btnSMS" runat="server" CausesValidation="true" Visible="false" CssClass="styleSubmitButton"
                                                            Text="SMS" />
                                                        <asp:Button ID="btnEmail" runat="server" CausesValidation="true" Visible="false"
                                                            CssClass="styleSubmitButton" Text="Email" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 990px">
                                                        <br />
                                                        <%--<asp:ValidationSummary ID="vsLRNote" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" ValidationGroup="btnSave" />
                                                        <asp:CustomValidator ID="cvLRNote" runat="server" CssClass="styleMandatoryLabel"
                                                            Enabled="true" />--%>
                                                        <asp:ValidationSummary ID="vsLRNote" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                            ShowMessageBox="false" ValidationGroup="Submit" HeaderText="Correct the following validation(s):" />
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="hidLRNNo" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</asp:Panel>--%>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/jscript" language="javascript"> 
  javascript:window.history.forward(1);
    </script>

</asp:Content>
