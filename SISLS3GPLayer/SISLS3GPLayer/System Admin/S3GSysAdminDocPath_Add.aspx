<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminDocPath_Add.aspx.cs" Inherits="System_Admin_S3GSysAdminDocPath_Add_" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Document Path Setup - Create" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblLOBcode" runat="server" Text="Line Of Business" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlLOBcode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLOBcode_SelectedIndexChanged"  onmouseover="ddl_itemchanged(this);">
                        </asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="rfvLOBCode" runat="server" ErrorMessage="Select Line of business"
                            ControlToValidate="ddlLOBcode" Display="None" InitialValue="0"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblRolecode" runat="server" Text="Role Description" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlRolecode" runat="server" AutoPostBack="false"  onmouseover="ddl_itemchanged(this);">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvRoleCode" runat="server" ErrorMessage="Select Role Description"
                            ControlToValidate="ddlRolecode" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                  <asp:DropDownList ID="ddlDocumentflag" runat="server" Visible="false"  onmouseover="ddl_itemchanged(this);">
                        </asp:DropDownList>
              <%--  <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblDocumentflag" runat="server" Text="Document Flag" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlDocumentflag" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvDocumentflag" runat="server" ErrorMessage="Select Document Flag"
                            ControlToValidate="ddlDocumentflag" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                </tr>--%>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblDocPath" runat="server" Text="Document Path" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtDocPath" runat="server" Width="400px" MaxLength="450"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID ="FtxtDocPath" runat="server" TargetControlID="txtDocPath" 
                        FilterType="Numbers,Custom" FilterMode="InvalidChars" InvalidChars="." Enabled="True"> </cc1:FilteredTextBoxExtender>
                        <asp:RequiredFieldValidator ID="rfvDocPath" runat="server" ErrorMessage="Enter Document Path"
                            ControlToValidate="txtDocPath" Display="None"></asp:RequiredFieldValidator>
                        <%-- <cc1:TextBoxWatermarkExtender ID="txtDocPath_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="txtDocPath" WatermarkText="Enter Filepath to store files" WatermarkCssClass="styleWaterMarked"> 
                </cc1:TextBoxWatermarkExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:CheckBox ID="chkActive" runat="server" Checked="true" Tooltip = "Active"/>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center" colspan="2">
                        <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium">
                        </span>
                        <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" 
                    ForeColor="Red"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            OnClientClick="return fnCheckPageValidators()" OnClick="btnSave_Click" ToolTip="Save" />
                        
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            CausesValidation="false" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" ToolTip="Clear" />
                        
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" CausesValidation="false" ToolTip="Cancel" />
                        <%--<input type="reset" value="Clear" class="styleSubmitButton" runat="server" id="btnClear"
                    onclick="return confirm('Do you want to clear?')" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ValidationSummary ID="vsDocPathAdd" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following Error(s):" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
