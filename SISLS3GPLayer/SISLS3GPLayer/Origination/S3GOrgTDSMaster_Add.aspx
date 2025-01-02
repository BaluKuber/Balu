<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GOrgTDSMaster_Add.aspx.cs" Inherits="Origination_S3GOrgTDSMaster_Add"
    EnableEventValidation="false" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function funShowGrid() {
            document.getElementById("btnShowGrid").click();
        }


        function fnCalculateRate() {
            var Tax = document.getElementById('<%=txtTax.ClientID%>').value;
            var Surcharge = document.getElementById('<%=txtSurcharge.ClientID%>').value;
            var Cess = document.getElementById('<%=txtCess.ClientID%>').value;
            var CessOrg = Cess;
            var EdCess = document.getElementById('<%=txtEdCess.ClientID%>').value;
            if (Surcharge == '')
                Surcharge = 0;
            if (Cess == '')
                Cess = 0;
            if (CessOrg == '')
                CessOrg = 0;
            if (Tax == '')
                Tax = 0;
            if (EdCess == '')
                EdCess = 0;
            var Result;
            var Surcharge = parseFloat((parseFloat(Tax) * (parseFloat(Surcharge) / 100)));
            var Cess = parseFloat((parseFloat(Tax) * (parseFloat(Cess) / 100)));
            var EdCess = Math.round((EdCess) * parseFloat(CessOrg)) / 100
            if (EdCess == NaN) {
                EdCess = 0;
            }

            Result = Math.round((parseFloat(Tax) + parseFloat(Surcharge) + parseFloat(Cess) + parseFloat(EdCess)) * 100) / 100
            document.getElementById('<%=txtEffectiveRate.ClientID%>').value = Result;
            document.getElementById('<%=hdnRate.ClientID%>').value = Result;
        }


    </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" meta:resourcekey="lblHeadingResource1"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                    <ContentTemplate>
                        <table width="80%" cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr style="height: 30px;">
                                <td class="styleFieldLabel" valign="top" width="20%">
                                    <asp:Label runat="server" ID="lblComptype" CssClass="styleReqFieldLabel" Text="Company Type"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:DropDownList ID="ddlCompanyType" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyType_OnSelectedIndexChanged" runat="server" Width="161px"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvCompanyType" runat="server" ControlToValidate="ddlCompanyType"
                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblTaxCode" runat="server" Text="Tax Code" CssClass="styleDisplayLabel"
                                        meta:resourcekey="lblEntityCodeResource1"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtTaxCode" runat="server" ReadOnly="true" MaxLength="12" meta:resourcekey="txtTaxCodeResource1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" ID="lblConstitutionName" CssClass="styleReqFieldLabel"
                                        Text="Constitution"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlConstitutionName" runat="server" Width="161px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvConsti" runat="server" ControlToValidate="ddlConstitutionName"
                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="-1" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblEffectiveDate" CssClass="styleReqFieldLabel" Text="Effective Date" runat="server"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtValidupto" runat="server"></asp:TextBox><asp:Image
                                        ID="imgValidupto" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvValidupto" runat="server" ControlToValidate="txtValidupto"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender runat="server" TargetControlID="txtValidupto" PopupButtonID="imgValidupto"
                                        ID="CalendarValidupto" Enabled="True" Format="dd/MM/yyyy">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>



                            <tr style="height: 30px;">
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblTaxSection" runat="server" Text="Tax Section" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtTaxSection" runat="server" AutoPostBack="true" OnTextChanged="txtTaxSection_OnTextChanged" Width="153px" MaxLength="12"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvTaxSection" runat="server" ControlToValidate="txtTaxSection"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblTaxDescription" runat="server" Text="Tax Description" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtTaxDescription" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvTaxDesc" runat="server" ControlToValidate="txtTaxDescription"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td class="styleFieldLabel" valign="top" width="20%">
                                    <asp:Label runat="server" ID="lblGLCode" CssClass="styleReqFieldLabel" Text="GL Code"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:DropDownList ID="ddlGLCode" AutoPostBack="true" OnSelectedIndexChanged="ddlGLCode_OnSelectedIndexChanged" runat="server" Width="161px"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvGLAccount" runat="server" ControlToValidate="ddlGLCode" InitialValue="0"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                <td class="styleFieldLabel" valign="top" width="20%">
                                    <asp:Label runat="server" ID="lblSLCode" CssClass="styleDisplayLabel" Text="SL Code"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:DropDownList ID="ddlSLCode" runat="server" Width="163px"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblTax" runat="server" Text="Tax" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtTax" runat="server" Width="153px" onchange="fnCalculateRate();" MaxLength="6" Style="text-align: right;" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvTax" runat="server" ControlToValidate="txtTax"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvTax" runat="server" ControlToValidate="txtTax"
                                        Operator="LessThanEqual" ValidationGroup="Submit" Type="Double" Display="None"
                                        ValueToCompare="100.00" />
                                    <asp:CompareValidator ID="cvTaxZ" runat="server" ControlToValidate="txtTax"
                                        Operator="NotEqual" ValidationGroup="Submit" Type="Double" Display="None" ErrorMessage="Tax Value cannot be Zero"
                                        ValueToCompare="0.00" />
                                </td>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblThresholdLimit" runat="server" Text="Threshold Limit" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtThresholdLimit" runat="server" Style="text-align: right;" MaxLength="10" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvThreshold" runat="server" ControlToValidate="txtThresholdLimit" ErrorMessage="Enter the Threshold Limit"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblSurcharge" runat="server" Text="Surcharge"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtSurcharge" Style="text-align: right;" onchange="fnCalculateRate();" runat="server" Width="153px" MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" Enabled="false" ID="rfvSurcharge" runat="server" ControlToValidate="txtSurcharge" ErrorMessage="Enter Surcharge"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvSurcharge" runat="server" ControlToValidate="txtSurcharge"
                                        Operator="LessThanEqual" Type="Double" Display="None" ValidationGroup="Submit"
                                        ValueToCompare="100.00" />
                                    <asp:CompareValidator ID="cvSurchargeZ" runat="server" Enabled="false" ControlToValidate="txtSurcharge"
                                        Operator="NotEqual" Type="Double" Display="None" ValidationGroup="Submit" ErrorMessage="Surcharge Value cannot be Zero"
                                        ValueToCompare="0.00" />
                                </td>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:CheckBox runat="server" ID="chkGrossUp" Width="200px" CssClass="styleDisplayLabel"
                                        Text="Gross up" />
                                </td>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:CheckBox runat="server" ID="chkActive" CssClass="styleDisplayLabel"
                                        Text="Active" />
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblCess" runat="server" Text="Cess" ></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtCess" Style="text-align: right;" onchange="fnCalculateRate();" runat="server" Width="153px" MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" Enabled="false" ID="rfvCess" runat="server" ControlToValidate="txtCess" ErrorMessage="Enter Cess"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvCess" runat="server" ControlToValidate="txtCess" ErrorMessage="Cess cannot be greater than 100"
                                        Operator="LessThanEqual" Type="Double" Display="None" ValidationGroup="Submit"
                                        ValueToCompare="100.00" />
                                    <asp:CompareValidator ID="cvCessZ" runat="server" ControlToValidate="txtCess" Enabled="false"
                                        Operator="NotEqual" Type="Double" Display="None" ValidationGroup="Submit" ErrorMessage="Cess Value cannot be Zero"
                                        ValueToCompare="0.00" />
                                </td>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblResidentialStatus" runat="server" Text="Residential Status" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:DropDownList ID="ddlResidentialStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlResidentialStatus_SelectedIndexChanged" Width="160px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvVendorResStatus" runat="server" ErrorMessage="Select the Residential Status"
                                        ValidationGroup="Submit" InitialValue="-1" Display="None" SetFocusOnError="True" ControlToValidate="ddlResidentialStatus"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblEdCess" runat="server" Text="Educational Cess" ></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtEdCess" Style="text-align: right;" onchange="fnCalculateRate();" runat="server" Width="153px" 
                                        MaxLength="6" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" Enabled="false" ID="RequiredFieldValidator1" runat="server" 
                                        ControlToValidate="txtEdCess" ErrorMessage="Enter Educational Cess"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtEdCess"
                                        Operator="LessThanEqual" Type="Double" Display="None" ValidationGroup="Submit" ErrorMessage="Educational Cess cannot be greater than 100"
                                        ValueToCompare="100.00" />
                                 </td>
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblPanApplicability" runat="server" Text="PAN Applicability" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:DropDownList ID="drpPANAppl" runat="server" Width="160px">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvPANAppl" runat="server" ErrorMessage="Select PAN Applicability"
                                        ValidationGroup="Submit" InitialValue="0" Display="None" SetFocusOnError="True" ControlToValidate="drpPANAppl"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height: 30px;">
                                <td class="styleFieldLabel" width="20%">
                                    <asp:Label ID="lblRate" runat="server" Text="Effective Rate"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="30%">
                                    <asp:TextBox ID="txtEffectiveRate" Style="text-align: right;" runat="server" Width="153px" MaxLength="3" ReadOnly="true" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    <asp:HiddenField ID="hdnRate" runat="server" />
                                    <asp:RequiredFieldValidator ValidationGroup="Submit" ID="rfvEffRate" runat="server" ControlToValidate="txtEffectiveRate" Enabled="false"
                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                                
                            </tr>
                            <tr style="height: 20px;">
                                <td colspan="4"></td>
                            </tr>
                        </table>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center">
                            <tr style="height: 20px;">
                                <td class="stylePageHeading">
                                    <asp:Label runat="server" ID="lblHistoryDetails" CssClass="styleDisplayLabel" Text="History Details"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False"
                                        EmptyDataText="No Records found!..." OnRowDataBound="gvHistory_RowDataBound"
                                        Width="100%">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sl No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSlNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Residential Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblResStatus" runat="server" Text='<%#Bind("RESSTATUS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Section">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxSection" runat="server" Text='<%#Bind("Tax_Law_Section") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTaxDesc" runat="server" Text='<%#Bind("Tax_Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GL Account">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGLAcc" runat="server" Text='<%#Bind("GLAccountDesc") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SL Account">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSLAcc" runat="server" Text='<%#Bind("SL_Account_Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Effective Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffDate" runat="server" Text='<%#Bind("Effective_From") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Threshold Limit">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblThreshold" runat="server" Text='<%#Bind("Threshold_Level") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tax">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTax" runat="server" Text='<%#Bind("Tax") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Surcharge">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSurcharge" runat="server" Text='<%#Bind("Surcharge") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cess">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCess" runat="server" Text='<%#Bind("Cess") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eff Rate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEffRate" runat="server" Text='<%#Bind("RatePercentage") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Gross Up">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGrossUp" runat="server" Visible="false" Text='<%#Bind("Gross_up") %>'></asp:Label>
                                                    <asp:CheckBox ID="chkgGrossUp" runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Active">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Bind("Is_active") %>'></asp:Label>
                                                    <asp:CheckBox ID="chkgActive" runat="server" Enabled="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                        OnClientClick="return fnCheckPageValidators('Submit');" Text="Save" ValidationGroup="Submit" />
                                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                                        Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                                        Text="Cancel" OnClick="btnCancel_Click" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td width="100%">
                                    <asp:ValidationSummary ID="vsTDSMaster" runat="server" CssClass="styleMandatoryLabel" ShowSummary="true" Enabled="true" ShowMessageBox="false"
                                        HeaderText="Correct the following validation(s):" ValidationGroup="Submit" />
                                    <asp:CustomValidator ID="cvTDS_Add" runat="server" CssClass="styleMandatoryLabel"
                                        Display="None" HeaderText="Correct the following validation(s): " />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

    </table>
    <input type="hidden" id="hdnID" runat="server" />


</asp:Content>
