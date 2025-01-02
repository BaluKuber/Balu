<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminCurrencyMaster_Add.aspx.cs" Inherits="System_Admin_S3GSysAdminCurrencyMaster_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td  class="stylePageHeading">
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
                        <table cellpadding="0" cellspacing="0" style="width: 635px">
                          
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Currency Name" CssClass="styleReqFieldLabel" ID="lblCurrencyName">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlCurrencyName" runat="server"></asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 229px">
                                    <asp:RequiredFieldValidator ID="rfvCurrencyName" CssClass="styleMandatoryLabel" runat="server"
                                         InitialValue="0" ControlToValidate="ddlCurrencyName" Text="Select the Currency Name" ErrorMessage="Select the Currency Name"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Precision" CssClass="styleReqFieldLabel" ID="lblPrecision">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtPrecision" runat="server" MaxLength="1" Style="width: 70px"></asp:TextBox>
                                </td>
                                <td colspan="2" style="width: 229px">
                                    <asp:RequiredFieldValidator ID="rfvPrecision" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="txtPrecision" Text="Enter the Precision Digits" ErrorMessage="Enter the Precision Digits"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Active" ID="lblActive">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign" colspan="3">
                                    <asp:CheckBox Checked="true" ID="chkActive" runat="server"  />
                                </td>
                            </tr>
                            <tr class="styleButtonArea" style="padding-left: 4px">
                                <td>
                                </td>
                                <td colspan="3">
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                        CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                                    <asp:Button runat="server" ID="btnClear"  CssClass="styleSubmitButton" CausesValidation="false"
                                        Text="Clear" OnClick="btnClear_Click" />
                                    <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
                                        Enabled="True" TargetControlID="btnClear">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:ValidationSummary CssClass="styleSummaryStyle" runat="server" ID="vsCurrency"
                                        HeaderText="Please correct the following validation(s):  " ShowSummary="true"
                                        ShowMessageBox="false" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td colspan="4">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                    <br />
                                    <asp:RangeValidator ID="rvPrecision" runat="server" ControlToValidate="txtPrecision"
                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Maximum 4 is allowed for Precision"
                                        MaximumValue="4" Text="Maximum 4 is allowed for Precision"></asp:RangeValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPrecision"
                FilterType="Numbers">
            </cc1:FilteredTextBoxExtender>            
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
