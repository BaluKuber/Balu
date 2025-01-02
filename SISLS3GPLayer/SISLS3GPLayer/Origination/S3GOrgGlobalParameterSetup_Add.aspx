<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GOrgGlobalParameterSetup_Add.aspx.cs" Inherits="Origination_S3GOrgGlobalParameterSetup" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function fnGetRecoveryValue(strRecoveryPatternYear) {
            var exp = /_/gi;
            return parseFloat(document.getElementById(strRecoveryPatternYear).value.replace(exp, '0'));
        }

        function fnCalculateMarginPercentage(strMarginPercentage) {
            var RecoveryPatternYear1;
            RecoveryPatternYear1 = fnGetRecoveryValue(strMarginPercentage);
            if (RecoveryPatternYear1 > 100) {
                alert('CTR and PLR % should not be greater than 100%');
                document.getElementById(strMarginPercentage).focus();
                return;
            }
        }

        function fnAssignText(objCheck) {

            var chkbox;
            var i = 2;
            //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid;            
            var gridId = "<%= gvProgram.ClientID %>"; //"ctl00_ContentPlaceHolder1_tcGlobal_TabPanel2_gvProgram";
            var objid = "CbxProgram";

            chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);

            var cnt = 0;
            while (chkbox != null) {

                if (chkbox.checked) cnt++; //else cnt--;
                i = i + 1;
                if (i <= 9) {
                    chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
                }
                else
                    chkbox = document.getElementById(gridId + '_ctl' + i + '_' + objid);
            }
            if (cnt == 0) cnt = "";
            document.getElementById("<%= txtPro.ClientID %>").value = cnt;
        }

        function fnSpaceNotAllowed(isSpaceAllowed) {
            debugger;
            var sKeyCode = window.event.keyCode;
            if ((isSpaceAllowed) && (sKeyCode == 32)) {
                window.event.keyCode = 0;
                return false;
            }
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="width: 100%">
                    <td>
                        <cc1:TabContainer ID="tcGlobal" runat="server" CssClass="styleTabPanel" Width="100%"
                            ScrollBars="Auto" ActiveTabIndex="3">
                            <!--Process Details -->
                            <cc1:TabPanel runat="server" HeaderText="Process" ID="tbProcess" CssClass="tabpan"
                                BackColor="Red" Width="100%" Visible="false">
                                <HeaderTemplate>
                                    Process
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td width="20%">
                                                <asp:Label ID="Label5" runat="server" Text="Enquiry Number change after offer is generated"
                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlModify" runat="server" Width="105px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvModify" runat="server" Display="None" ErrorMessage="Define Enquiry change rule - Process Tab"
                                                    ControlToValidate="ddlModify" Enabled="false" InitialValue="-1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="display:none;" width="20%">
                                                <asp:Label ID="lblProductInflow" runat="server" Text="Product Inflow Adjust"></asp:Label>
                                            </td>
                                            <td style="display:none;" width="20%">
                                                <asp:CheckBox ID="cbxProductInflow" runat="server" />
                                            </td>
                                        </tr>
                                        <tr style="height: 20px" align="left">
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <bold><asp:Label ID="Label1" runat="server" Text="Starting point of the process" 
                                                    Font-Bold="True"></asp:Label>
</bold>
                                            </td>
                                            <td class="styleFieldAlign" width="30%"></td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <asp:Label ID="lblEnquiryNo" runat="server" Text="Enquiry Number"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:CheckBox runat="server" ID="CbxEnquiryNo" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <asp:Label ID="Label2" runat="server" Text="Offer Number"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:CheckBox runat="server" ID="CbxOfferNo" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <asp:Label ID="Label3" runat="server" Text="Application Number"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:CheckBox runat="server" ID="CbxApplicationNo" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 30px"></td>
                                        </tr>
                                        <tr>
                                            <td width="20%">
                                                <asp:Label ID="Label4" CssClass="styleDisplayLabel" runat="server" Text="Credit Score negative variance applicable"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:CheckBox runat="server" ID="chkNegative" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <!--IRR Details -->
                            <cc1:TabPanel runat="server" HeaderText="Internal Rate of Return (IRR)" ID="TabPanel1"
                                CssClass="tabpan" BackColor="Red" Width="100%" Visible="false">
                                <HeaderTemplate>
                                    Internal Rate of Return (IRR)
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td width="20%">
                                                <asp:Label ID="lblIRR" runat="server" Text="IRR Rule Details" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:DropDownList ID="ddlIRR" runat="server" Width="105px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator Enabled="false" ID="rfvIRR" runat="server" Display="None" ErrorMessage="Define IRR applicability - IRR Tab"
                                                    ControlToValidate="ddlIRR" InitialValue="-1"></asp:RequiredFieldValidator>
                                            </td>
                                            <td width="20%"></td>
                                            <td width="20%"></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <div style="height: 300px; width: 75%">
                                                    <asp:GridView ID="gvIRR" runat="server" AutoGenerateColumns="False" BorderColor="Gray"
                                                        Width="100%" DataKeyNames="LOB_ID" CssClass="styleInfoLabel">
                                                        <Columns>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLobID" runat="server" Text='<%# Bind("LOB_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Line of Business">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLob" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice Required" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlvoice" runat="server" Width="90px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Depreciation Method">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlDesc" runat="server" Width="90px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CTR">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCTR" runat="server" Width="90px" onkeypress="fnAllowNumbersOnly(true,false,this);"
                                                                        MaxLength="6"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtCTR"
                                                                        FilterType="Numbers" InvalidChars=" " FilterMode="InvalidChars" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="PLR">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPLR" runat="server" Width="90px" onkeypress="fnAllowNumbersOnly(true,false,this);"
                                                                        MaxLength="6"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtPLR"
                                                                        FilterType="Numbers" InvalidChars=" " FilterMode="InvalidChars" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CbxIRR" runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <!--ROI Details -->
                            <cc1:TabPanel runat="server" HeaderText="Return on Investment(ROI)" ID="TabPanel2"
                                CssClass="tabpan" BackColor="Red" Visible="false">
                                <HeaderTemplate>
                                    Return on Investment Rule (ROI)
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="50%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="Label12" runat="server" Text="ROI Rule Modification" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:TextBox ID="txtPro" runat="server" Style="display: none">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None" Enabled="false"
                                                    ErrorMessage="Define ROI modification rule - ROI Rule Tab" ControlToValidate="txtPro"></asp:RequiredFieldValidator>
                                                <asp:GridView ID="gvProgram" runat="server" CssClass="styleInfoLabel" Width="100%"
                                                    AutoGenerateColumns="False" DataKeyNames="Program_ID" OnRowDataBound="gvProgram_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProgramid" runat="server" Text='<%# Eval("Program_ID") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Program Ref. No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProgramRefNo" runat="server" Text='<%# Eval("Program_Ref_No") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Program Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProgramName" runat="server" Text='<%# Eval("Program_Name") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Capture">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="CbxProgram" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <!--Currency Details -->
                            <cc1:TabPanel runat="server" HeaderText="Currency" ID="TabPanel3" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Currency Round Off
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="50%">
                                        <tr>
                                            <td class="styleFieldLabel" width="20%">
                                                <asp:Label ID="lblAmount" runat="server" Text="Amount" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign" width="30%">
                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="4" Style="text-align: right"
                                                    onkeypress="fnAllowNumbersOnly(false,false,this)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvAmount" runat="server" Display="None" ErrorMessage="Enter the amount - Currency Round Off Tab"
                                                    ControlToValidate="txtAmount"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtAmount"
                                                    FilterType="Numbers" InvalidChars="" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <!--Application Details -->
                            <cc1:TabPanel runat="server" HeaderText="Application" ID="TabPanel4" CssClass="tabpan"
                                BackColor="Red" Visible="false">
                                <HeaderTemplate>
                                    Application Approval
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="50%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAccount" runat="server" Text="Create Account on Application Approval"
                                                    CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlAccount" runat="server" Width="105px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvAccount" runat="server" Display="None" Enabled="false" ErrorMessage="Define Account creation rule - Application Approval Tab"
                                                    ControlToValidate="ddlAccount" InitialValue="-1"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr style="height: 10px">
                                            <td></td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Application" ID="TabPanel5" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    General
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="50%">
                                        <tr>
                                            <asp:GridView ID="gvGeneral" runat="server" CssClass="styleInfoLabel" Width="100%"
                                                AutoGenerateColumns="False" OnRowDataBound="gvGeneral_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="LOB_ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLOBID" runat="server" Text='<%# Eval("LOB_ID") %>' Visible="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Line of Business">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text='<%# Eval("LOB") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cashier Account">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlCashierAc" runat="server">
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                        <input id="hdnGlobalParamId" type="hidden" runat="server" value="0" />
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                            CausesValidation="False" OnClick="btnClear_Click" />
                        <cc1:ConfirmButtonExtender ID="btnAssetClear_ConfirmButtonExtender" runat="server"
                            ConfirmText="Do you want to Clear?" Enabled="True" TargetControlID="btnClear">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                            Text="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsGlobal" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " />
                        <asp:CustomValidator ID="cvGlobal" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: medium"></asp:Label>
                        <input id="hdnUserId" type="hidden" runat="server" value="0" />
                        <input id="hdnUserLevelId" type="hidden" runat="server" value="0" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
