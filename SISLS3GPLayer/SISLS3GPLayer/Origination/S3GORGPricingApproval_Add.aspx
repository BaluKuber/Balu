<%@ Page Language="C#" AutoEventWireup="true" Title="S3G - Pricing Approval" CodeFile="S3GORGPricingApproval_Add.aspx.cs"
    Inherits="Origination_S3GORGPricingApproval" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function fnCheckPageValidators_Extn(strValGrp, blnConfirm) {

            if (Page_ClientValidate() == false) {
                var iResult = "1";
                for (i = 0; i < Page_Validators.length; i++) {
                    val = Page_Validators[i];
                    controlToValidate = val.getAttribute("controltovalidate");
                    if (controlToValidate != undefined) {
                        if (document.getElementById(controlToValidate) != null) {
                            document.getElementById(controlToValidate).border = "1";
                        }
                    }
                }

                for (i = 0; i < Page_Validators.length; i++) { //For loop1
                    val = Page_Validators[i];
                    var isValidAttribute = val.getAttribute("isvalid");
                    var controlToValidate = val.getAttribute("controltovalidate");
                    if (controlToValidate != undefined) {
                        if (strValGrp == undefined) {
                            if (document.getElementById(controlToValidate) != null) {
                                if (isValidAttribute == false) {

                                    document.getElementById(controlToValidate).className = 'styleReqFieldFocus';
                                    //This is to avoid not to validate control which is already in false state (valid attribute)
                                    document.getElementById(controlToValidate).border = "0";
                                    iResult = "0";
                                }
                                else if (document.getElementById(controlToValidate).border != "0") {
                                    document.getElementById(controlToValidate).className = 'styleReqFieldDefalut';
                                }
                            }
                        }

                    }  //Undefined loop condition

                } //For loop1 End

                Page_BlockSubmit = false; ////Added by Kali on 12-Jun-2010 This function is used to solve issues
                // Need twice click of cancel and clear button after validation summary is visible
            } //

            if (Page_ClientValidate(strValGrp)) {

                if (blnConfirm == undefined) {
                    if (confirm('Do you want to Revoke?')) {
                        Page_BlockSubmit = false;
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        var a = event.srcElement;
                        if (a.type == "submit") {
                            a.style.display = 'none';
                        }
                        //End here
                        return true;
                    }
                    else
                        return false;
                }
                else {
                    Page_BlockSubmit = false;
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    var a = event.srcElement;
                    if (a.type == "submit") {
                        a.style.display = 'none';
                    }
                    //End here
                    return true;
                }

            }
            else {
                Page_BlockSubmit = false;
                return false;
            }

        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" Text="Proposal Pricing Approval" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel" valign="top" align="left">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="Label1" runat="server" CssClass="styleReqFieldLabel" Text="Approval Status"></asp:Label>
                                            </td>
                                            <td colspan="2" class="styleFieldLabel">
                                                <asp:DropDownList ID="ddlApprovalStatus" runat="server"></asp:DropDownList>

                                                <asp:CheckBox ID="chkUnapproval" AutoPostBack="true" Text="Unapproved" runat="server"
                                                    OnCheckedChanged="chkUnapproval_CheckedChanged" Visible="false" />
                                                <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" TargetControlID="chkUnapproval"
                                                    Key="1" runat="server">
                                                </cc1:MutuallyExclusiveCheckBoxExtender>
                                                &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkApproved" Visible="false"  Text="Approved" runat="server" OnCheckedChanged="chkApproved_CheckedChanged"
                                                    AutoPostBack="true" />
                                                <cc1:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender1" TargetControlID="chkApproved"
                                                    Key="1" runat="server">
                                                </cc1:MutuallyExclusiveCheckBoxExtender>
                                                <asp:RequiredFieldValidator ID="rfvApprovalStatus" runat="server" ControlToValidate="ddlApprovalStatus" ValidationGroup="Go"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Approval Status"
                                                        InitialValue="0"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:Panel GroupingText="Pricing Details" ID="pnlPricingDetails" runat="server" CssClass="stylePanel">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddllLineOfBusiness" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddllLineOfBusiness_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddllLineOfBusiness" ValidationGroup="Go"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Line of Business"
                                                        InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDateFrom" runat="server" Text="From Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                        Visible="false" />
                                                    <cc1:CalendarExtender ID="CEEPValidTill" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                        PopupButtonID="Image8" TargetControlID="txtFromDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvPEVD" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtFromDate" ErrorMessage="Enter valid From date"
                                                        ValidationGroup="Go" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBranch" runat="server" Text="Location"></asp:Label>
                                                </td>
                                                <td>
                                                    <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true" ValidationGroup="Go"
                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select the Location" WatermarkText="--All--"/>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblToDate" runat="server" Text="To Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                        Visible="false" />
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                        PopupButtonID="Image8" TargetControlID="txtToDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtToDate" ErrorMessage="Enter valid To date"
                                                        ValidationGroup="Go" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBusinessOfferNumber" runat="server" Text="Lessee Name"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <uc2:Suggest ID="ddlBusinessOfferNumber" runat="server" ServiceMethod="GetCustomerName"  WatermarkText="--All--"
                                                        AutoPostBack="true" OnItem_Selected="ddlBusinessOfferNumber_SelectedIndexChanged" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblOfferDate" runat="server" Text="Offer Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOfferDate" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtStatus" ReadOnly="true" runat="server" Width="100px">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:HiddenField ID="hProductID" runat="server" Visible="false" />
                                                    <asp:Button ID="btnGo" CssClass="styleSubmitShortButton" runat="server" Text="Go" ValidationGroup="Go"
                                                        OnClick="btnGo_Click" />
                                                    <asp:LinkButton Text="View Pricing" Visible="false" CausesValidation="false" runat="server" ID="PaymentReqID"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel" style="width: 100%">

                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:GridView Visible="false" ID="grvApprovalDetails" runat="server" AutoGenerateColumns="false"
                                                    OnRowDataBound="grvApprovalDetails_RowDataBound" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Approval No" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Approver Name" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblApprovarName" Text='<%# Bind("User_Name") %>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlAction1" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvAction" runat="server" ControlToValidate="ddlAction1"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Action"
                                                                    InitialValue="0"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Password" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtPassword1" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftxtPassword" runat="server" TargetControlID="txtPassword1"
                                                                    FilterType="Custom" FilterMode="InvalidChars" InvalidChars=" ">
                                                                </cc1:FilteredTextBoxExtender>
                                                                <%--<asp:RequiredFieldValidator Enabled="false" ID="rfvPassword" runat="server" ControlToValidate="txtPassword1"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Password"></asp:RequiredFieldValidator>--%>
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
                                                                    Width="100%" onkeyDown="maxlengthfortxt(80)" TextMode="MultiLine"></asp:TextBox>
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
                                <td>
                                     <asp:Panel GroupingText="Offer Details" Visible="false" ID="pnlOfferDetails" runat="server" CssClass="stylePanel">
                                        <div id="divGrid1" runat="server" style="overflow: scroll; height: 300px; width: 100%;">
                                        <asp:GridView ID="grdPricingApproval" runat="server" AutoGenerateColumns="false"
                                            OnRowDataBound="grdPricingApproval_OnRowDataBound" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSlno" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Lessee Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLesseeName" Text='<%# Bind("Customer_Name") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Offer No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblApprovalSNO" Visible="false" Text='<%# Bind("Approval_Serial_Number")%>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblPricingApproval_ID" Text='<%# Bind("PricingApproval_ID")%>' Visible="false"
                                                            runat="server"></asp:Label>
                                                        <asp:Label ID="lblPricingNo" Text='<%# Bind("Business_Offer_Number") %>' runat="server"></asp:Label>
                                                        <asp:Label ID="lblOfferID" Visible="false" Text='<%# Bind("Pricing_ID") %>' runat="server"></asp:Label>
                                                        <asp:HiddenField ID="hdnID" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Offer Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOfferDate" Text='<%# Bind("OFFER_DATE") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Account Manager 1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccMngr1" Text='<%# Bind("AccountManager1") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Account Manager 2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccMngr2" Text='<%# Bind("AccountManager2") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Regional Manager">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegMngr" Text='<%# Bind("RegionalManager") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Current Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcurrentStatus" Text='<%# Bind("STATUS") %>' runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                      Select<asp:CheckBox ID="chkAll"  runat="server" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged"/>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                     <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="View">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkView" runat="server" Text="View Pricing" OnClick="lnkView_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                     </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel GroupingText="Approver Details" ID="pnlAppDetails" runat="server" CssClass="stylePanel" Visible="false">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblAppDate" runat="server" Text="Approval Date :"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:Label ID="lblApprovalDate" runat="server"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 50%"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblAppName" runat="server" Text="Approver Name :"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:Label ID="lblApproverName" runat="server"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 50%"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblAction" runat="server" Text="Action :"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:DropDownList ID="ddlAction" runat="server"></asp:DropDownList>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 50%"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblPassword" runat="server" Text="Password :"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password" Width="80%"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftxtPassword" runat="server" TargetControlID="txtPassword"
                                                        FilterType="Custom" FilterMode="InvalidChars" InvalidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="btnSave"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Password"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 50%"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblRemarks" runat="server" Text="Remarks :"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtRemarks" runat="server" Height="40px" Width="80%"
                                                        onkeyDown="maxlengthfortxt(80)" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 50%"></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel GroupingText="Customer Information" ID="pnlCustomerinformation" runat="server"
                                        CssClass="stylePanel" Visible="false">
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
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                            OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" ValidationGroup="btnSave" />
                        <asp:Button ID="btnRevoke" runat="server" CssClass="styleSubmitButton" OnClick="btnRevoke_Click"
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
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel" ValidationGroup="btnSave"
                            HeaderText="Please correct the following validation(s):" ShowSummary="true"  />

                        <asp:ValidationSummary ID="vsGo" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="Go" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
