<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeFile="S3GSysAdminLOBMaster_Add.aspx.cs"
    Inherits="System_Admin_S3GSysAdminLOBMaster_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput)
        {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
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
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvLOBCode" runat="server" ErrorMessage="Enter the Line of Business Code"
                                                    ControlToValidate="txtLOBCode" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvLOBName" runat="server" ErrorMessage="Enter the Line of Business Name"
                                                    ControlToValidate="txtLOBName" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtLOBName"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars=" "
                                        Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtLOBCode"
                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers" Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business Code" ID="lblLOBCode" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtLOBCode" runat="server" MaxLength="2" Style="text-transform:uppercase"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business Name" ID="lblLOBName" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtLOBName" runat="server" MaxLength="40"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:CheckBox ID="chkActive" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                        CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                                
                                    <asp:Button ID="btnReset" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                        CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                            
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td colspan="2">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary CssClass="styleSummaryStyle" ID="vsLOB" runat="server" ShowMessageBox="false"
                                        ShowSummary="true" HeaderText="Please correct the following validation(s):  "  />
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
