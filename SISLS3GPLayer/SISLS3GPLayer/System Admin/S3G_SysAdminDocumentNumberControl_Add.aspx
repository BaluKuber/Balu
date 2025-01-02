<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="S3G_SysAdminDocumentNumberControl_Add.aspx.cs"
    Inherits="System_Admin_S3G_SysAdminDocumentNumberControl_Add" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput) {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="tbDNC" runat="server" border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td>
                                </td>
                            </tr>
                            <tr>
                               
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Financial Year" ID="lblFinYear"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" >                                  
                                    <asp:DropDownList ID="ddlFinYear" runat="server" Width="205px" TabIndex="3">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20%">
                                    <%--<asp:RequiredFieldValidator ID="rfvFinYear" runat="server" Display="None" ErrorMessage="Select the Financial Year"
                                        ControlToValidate="ddlFinYear" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                               
                                <td class="styleFieldLabel" >
                                    <asp:Label runat="server" Text="Line of Business " ID="lbllOB"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" >
                                  
                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="205px" TabIndex="1">
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="rfvLOB" runat="server" Display="None" ErrorMessage="Select the Line of Business or Branch"
                                  ControlToValidate="ddlLOB" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                </td>
                               
                            </tr>
                            <tr>
                               
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Location" ID="lblBranch"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" >
                                   
                                     <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true" TabIndex="2" Width="200px"
                                           OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                                    <%--<asp:DropDownList ID="ddlBranch" runat="server" Width="205px" TabIndex="2" onmouseover="ddl_itemchanged(this);">
                                    </asp:DropDownList>--%>
                                    <%-- <asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" ErrorMessage="Select the Line of Business or Branch"
                                  ControlToValidate="ddlBranch" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                </td>
                                
                            </tr>
                            <tr>
                               
                                <td class="styleFieldLabel" >
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Document Type" ID="lblDocType"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">                                 
                                    <asp:DropDownList ID="ddlDocType" onmouseover="ddl_itemchanged(this);" runat="server"
                                        Width="205px" TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20%">
                                    <asp:RequiredFieldValidator ID="rfvDocType" runat="server" Display="None" ErrorMessage="Select the Document Type"
                                        ControlToValidate="ddlDocType" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                               
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Batch" ID="Batch"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" >                                  
                                    <asp:TextBox ID="txtBatch" runat="server" Width="100px" MaxLength="5" TabIndex="5"></asp:TextBox>
                                    <asp:DropDownList ID="ddlBatch" runat="server" Width="105px" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                </td>
                                <td style="width: 20%">
                                    <asp:RequiredFieldValidator ID="rfvBatch" runat="server" Display="None" ErrorMessage="Enter the Batch"
                                        ControlToValidate="txtBatch"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvddlBatch" runat="server" Display="None" ErrorMessage="Select the Batch"
                                        ControlToValidate="ddlBatch" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtBatch"
                                        FilterType="Custom,LowercaseLetters,Numbers,UppercaseLetters" InvalidChars=""
                                        Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                               
                                <td class="styleFieldLabel" >
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="From Number" ID="lblFromNo"></asp:Label>
                                </td>
                                <td  class="styleFieldAlign" >                                   
                                    <cc2:NumericTextBox ID="txtFromNo" runat="server" Width="100px" MaxLength="12" TabIndex="6"></cc2:NumericTextBox>
                                </td>
                                <td style="width: 20%">
                                    <asp:RequiredFieldValidator ID="rfvFromNos" runat="server" Display="None" ErrorMessage="Enter the From Number"
                                        ControlToValidate="txtFromNo" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                              
                                <td  class="styleFieldLabel" >
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="To Number" ID="lblToNo"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" >
                                    
                                    <cc2:NumericTextBox ID="txtToNo" runat="server" Width="100px" MaxLength="12" TabIndex="7"></cc2:NumericTextBox>
                                </td>
                                <td style="width: 20%">
                                    <asp:RequiredFieldValidator ID="rfvToNo" runat="server" Display="None" ErrorMessage="Enter the To Number"
                                        ControlToValidate="txtToNo" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                               
                                <td class="styleFieldLabel" >
                                    <asp:Label runat="server" Text="Last Used Number" ID="lblLastNo"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" >
                                  
                                    <cc2:NumericTextBox ID="txtLastNo" runat="server" Width="100px"></cc2:NumericTextBox>
                                </td>
                               
                            </tr>
                            <tr>                              
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Active" ID="lblActive"></asp:Label>
                                </td>
                                <td class="styleFieldAlign" >
                                   <asp:CheckBox ID="CbxActive" runat="server" />
                                </td>                               
                            </tr>
                            <tr>
                                <td style="height: 20px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                                                    OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                                                <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClick="btnClear_Click" />
                                                <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
                                                    Enabled="True" TargetControlID="btnClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" Text="Cancel"
                                                    CausesValidation="False" OnClick="btnCancel_Click" />
                                                <asp:Button runat="server" ID="btnDelete" CssClass="styleSubmitButton" Text="Delete"
                                                    CausesValidation="False" OnClick="btnDelete_Click" AccessKey="I" />
                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Do you want to delete?"
                                                    Enabled="True" TargetControlID="btnDelete">
                                                </cc1:ConfirmButtonExtender>
                                            </td>
                                        </tr>
                                        <tr class="styleButtonArea">
                                            <td>
                                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                                                    font-size: medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 220px" valign="top">
                                            <td colspan="5">
                                                <asp:ValidationSummary ID="vsDocTypeSummary" runat="server" Height="30px" Width="716px"
                                                    CssClass="styleMandatoryLabel" ShowSummary="true" HeaderText="Please correct the following validation(s):  " />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:CustomValidator ID="cvDNC" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
