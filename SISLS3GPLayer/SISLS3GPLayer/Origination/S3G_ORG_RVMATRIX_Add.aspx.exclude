<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_ORG_RVMATRIX_Add.aspx.cs" Inherits="Origination_S3G_ORG_RVMATRIX_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updCAPMAst" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table class="stylePageHeading" width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnCAPApp" runat="server" Visible="false" Value="0" />
                                    <asp:HiddenField ID="hdnDEV" runat="server" Visible="false" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" width="100%">
                                    <%-- <cc1:TabContainer ID="tcCAPMaster" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                        Width="100%" ScrollBars="Auto">
                                        <cc1:TabPanel runat="server" HeaderText="CAP Approval" ID="TabPanel1" CssClass="tabpan"
                                            BackColor="Red">
                                            <HeaderTemplate>
                                                CAP Approval
                                            </HeaderTemplate>
                                            <ContentTemplate>--%>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblLOB" runat="server" Text="Line Of Business" Width="120px"></asp:Label>
                                                <asp:DropDownList ID="ddlLOB" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <asp:Panel ID="panCAPApproval" class="container" GroupingText="RV Matrix"
                                                    runat="server" Width="100%" CssClass="stylePanel">
                                                    <asp:GridView runat="server" ShowFooter="True" ID="grvRVMatrix" AutoGenerateColumns="False"
                                                        OnRowCommand="grvRVMatrix_RowCommand" Width="100%" OnRowDataBound="grvRVMatrix_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No." ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSNo" runat="server" Text='<%# Bind("SNo") %>' Width="150px" Visible="false"></asp:Label>
                                                                    <asp:Label runat="server" ID="Label1" Width="70%" ToolTip="S.No." Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Category" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetCategory" runat="server" Text='<%#Bind("Category_Description") %>'></asp:Label>
                                                                    <asp:Label ID="lblCatgID" runat="server" Text='<%#Bind("Category_ID") %>' Visible="false"></asp:Label>

                                                                </ItemTemplate>
                                                                <FooterTemplate>

                                                                    <asp:DropDownList ID="ddlFAssetCategory" Width="80%" runat="server"></asp:DropDownList>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="W.E.F. Date" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEffectiveDate" Text='<%#Bind("Effective_From") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtValidupto" runat="server"></asp:TextBox><asp:Image
                                                                        ID="imgValidupto" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                    <cc1:CalendarExtender runat="server" TargetControlID="txtValidupto" PopupButtonID="imgValidupto"
                                                                        ID="CalendarValidupto" Enabled="True" Format="dd/MM/yyyy">
                                                                    </cc1:CalendarExtender>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Matrix" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnRange" runat="server" CssClass="styleSubmitShortButton" OnClick="btnRange_Click" Text="Range" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnFRange" CssClass="styleSubmitShortButton" OnClick="btnFRange_Click"
                                                                        OnClientClick="return fnCheckPageValidators('Add',false);" runat="server" Text="Range"
                                                                        ValidationGroup="Add" ToolTip="Range" />
                                                                </FooterTemplate>
                                                                <%-- <FooterTemplate>
                                                                                <asp:TextBox ID="txtFApprovalOrder" CssClass="styleTxtRightAlign" Enabled="true"
                                                                                    Width="50px" MaxLength="1" ToolTip="No.of Approvals" runat="server"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="fteFApprovalOrder" runat="server" TargetControlID="txtFApprovalOrder"
                                                                                    FilterType="Numbers,Custom" InvalidChars="0">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtFApprovalOrder" Display="None" ErrorMessage="Enter No.of Approvals"
                                                                                    runat="server" ControlToValidate="txtFApprovalOrder" ValidationGroup="Add">
                                                                                </asp:RequiredFieldValidator>
                                                                                <asp:RangeValidator ID="rngCAPNoofApproval" runat="server" ControlToValidate="txtFApprovalOrder"
                                                                                    MinimumValue="1" MaximumValue="9" Type="Integer" Display="None" ValidationGroup="Add"
                                                                                    ErrorMessage="No.of Approvals should be from 1 to 9">
                                                                                </asp:RangeValidator>
                                                                            </FooterTemplate>--%>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Active">
                                                                <ItemTemplate>

                                                                    <asp:HiddenField ID="hdnCheckActive" runat="server" Value='<%#Bind("Is_Active") %>' />
                                                                    <asp:CheckBox ID="chkIActive"
                                                                        runat="server" ToolTip="Active" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>

                                                                    <%-- <asp:HiddenField ID="hdnHeader" runat="server" Value='<% #Bind("RV_Matrix_Hdr_ID")%>' />--%>
                                                                    <asp:CheckBox ID="chkCAPFActive" runat="server" ToolTip="Active" Checked="true" Enabled="false" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">

                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnAdd" CssClass="styleSubmitShortButton" CommandName="Add" runat="server"
                                                                        OnClientClick="return fnCheckPageValidators('Add',false);" ToolTip="Add" Text="Add"
                                                                        ValidationGroup="Add" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <RowStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                        </table>

                        <table>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnCheck" runat="server" />


                                    <cc1:ModalPopupExtender ID="ModalPopupExtenderDEVApprover" runat="server" TargetControlID="btnModal1"
                                        PopupControlID="PnlApprover" BackgroundCssClass="styleModalBackground" Enabled="true">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="PnlApprover" Style="vertical-align: middle" runat="server"
                                        BorderStyle="Solid" BackColor="White" Width="610px">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <div id="div1" class="container" runat="server" style="width: auto; overflow: scroll">
                                                        <asp:Label ID="lblAssetCategoryName" runat="server" CssClass="styleFieldLabel" Text="Asset Category :"></asp:Label>
                                                        <asp:Label ID="lblAssetCatg" runat="server" CssClass="styleFieldLabel" Font-Bold="true"></asp:Label><br />
                                                        <asp:GridView Width="98%" ID="grvRange" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvRange_RowDataBound"
                                                            OnRowCommand="grvRange_OnRowCommand" ShowFooter="true" OnRowDeleting="grvRange_RowDeleting">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Lease Tenure From" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCatgID" runat="server" Text='<%#Bind("CatgID") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblEffDate" runat="server" Text='<%#Bind("Eff_Date") %>' Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblLeaseTenureFrom" runat="server" Text='<% #Bind("Tenure_Form")%>'></asp:Label>

                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtLeaseTenureFrom" MaxLength="3" Enabled="false" runat="server"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftbeLeaseTenureFrom" FilterType="Numbers" TargetControlID="txtLeaseTenureFrom"
                                                                            runat="server" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Lease Tenure To" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLeaseTenureTo" runat="server" Text='<% #Bind("Tenure_To")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtLeaseTenureTo" MaxLength="3" runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                                                                        <%--  <cc1:FilteredTextBoxExtender ID="ftbeLeaseTenureTo" FilterType="Numbers" TargetControlID="txtLeaseTenureTo"
                                                                            runat="server" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>--%>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RV%" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRVPercent" runat="server" Text='<% #Bind("RV_Percentage")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>

                                                                        <asp:TextBox ID="txtRVPercent" MaxLength="5" runat="server" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                                        <%--  <cc1:FilteredTextBoxExtender ID="ftbeRVPercent" FilterType="Numbers" TargetControlID="txtRVPercent"
                                                                            runat="server" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>--%>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" ToolTip="Delete" OnClientClick="return confirm('Do you want to Delete this record?');"
                                                                            CommandName="Delete"></asp:LinkButton>
                                                                        <%--<asp:Label ID="lblIsDelete" Visible="false" runat="server" Text='<%# Bind("Is_Delete") %>'></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnAddRange" runat="server" CommandName="Add" CssClass="styleSubmitShortButton" Text="Add" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnSaveRange" runat="server" OnClick="btnSaveRange_OnClick" Text="Save"
                                                        ToolTip="Save" class="styleSubmitButton" Visible="false" />
                                                    <asp:Button ID="btnCancelRange" runat="server" Text="Close" ToolTip="Close" class="styleSubmitButton"
                                                        OnClick="btnCancelRange_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>


                                    <asp:Button ID="btnModal1" Style="display: none" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSave" ToolTip="Save" runat="server" OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators('Submit');"
                                        CssClass="styleSubmitButton" Text="Save" />
                                    <asp:Button ID="btnExportToExcel" OnClick="btnExportToExcel_Click" UseSubmitBehavior="true" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Export To Excel" runat="server"></asp:Button>
                                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                                        Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                    <asp:Button ID="btnCancel" ToolTip="Cancel" CssClass="styleSubmitButton" runat="server"
                                        Visible="false" Text="Cancel" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsCAPMAster" ValidationGroup="Add" runat="server" ShowSummary="true"
                            HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                        <asp:ValidationSummary ID="vsDEVMaster" ValidationGroup="Add1" runat="server" ShowSummary="true"
                            HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        function FunCheckAmount() {
            var txtLTVPercentage = document.getElementById('ctl00$ContentPlaceHolder1$tcCAPMaster$TabPanel2$grvCAPDeviation$ctl14$txtFLTVPercentage');
            var txtIRRPercentage = document.getElementById('ctl00$ContentPlaceHolder1$tcCAPMaster$TabPanel2$grvCAPDeviation$ctl14$txtFIRRPercentage');
            var txtIDVAmount = document.getElementById('ctl00$ContentPlaceHolder1$tcCAPMaster$TabPanel2$grvCAPDeviation$ctl14$txtFIDV');
            var txtWaiverAmount = document.getElementById('ctl00$ContentPlaceHolder1$tcCAPMaster$TabPanel2$grvCAPDeviation$ctl14$txtFWaiverAmount');
            if ((parseFloat(txtLTVPercentage.value)) == 0 && (parseFloat(txtIRRPercentage.value)) == 0 && (parseFloat(txtIDVAmount.value)) == 0 && (parseFloat(txtWaiverAmount.value)) == 0) {
                alert('All values cannot be zero');
                Page_BlockSubmit = false;
                return false;
            }
        }

        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
              && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>

</asp:Content>
