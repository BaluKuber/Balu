<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_SysAdminEscalationMaster_Add.aspx.cs" Inherits="System_Admin_S3G_SysAdminEscalationMaster_Add" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
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
                        <table id="tbEscalation" runat="server" border="0" cellspacing="0" cellpadding="0"
                            width="100%">
                            <tr id="Tr1" runat="server">
                                <td id="Td1" runat="server">
                                </td>
                            </tr>
                            <tr id="Tr2" runat="server">
                                <td id="Td2" align="left" class="styleFieldLabel" runat="server" width="20%">
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Line of Business "
                                        ID="lbllOB"></asp:Label>
                                </td>
                                <td id="Td3" align="left" class="styleFieldAlign" runat="server" style="width: 55%">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="250px" TabIndex="1" 
                                        AutoPostBack="True" onselectedindexchanged="ddlLOB_SelectedIndexChanged" 
                                      onmouseover="ddl_itemchanged(this);" >
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ErrorMessage="Select the Line of Business"
                                        ControlToValidate="ddlLOB" InitialValue="0">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 20%">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Role Description " ID="lbllOBRolecode"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 55%">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlRoleCode" runat="server" Width="250px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlRoleCode_SelectedIndexChanged" TabIndex="2" onmouseover="ddl_itemchanged(this);">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvRolecode" runat="server" ErrorMessage="Select the Role Description"
                                        InitialValue="0" ControlToValidate="ddlRoleCode">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 20%">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="User " ID="lblUser"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 55%">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddlUser" runat="server" Width="250px" TabIndex="3" AutoPostBack="True" 
                                    onmouseover="ddl_itemchanged(this);" onselectedindexchanged="ddlUser_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvUser" runat="server" ErrorMessage="Select the User"
                                        InitialValue="0" ControlToValidate="ddlUser">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 20%">
                                </td>
                            </tr>
                            <tr id="Tr5" runat="server">
                                <td id="Td8" align="left" class="styleFieldLabel" runat="server" width="20%">
                                    <asp:Label runat="server" CssClass="styleDisplayLabel" Text="NL Hours" ID="lblNLDays"></asp:Label>
                                </td>
                                <td id="Td9" align="left" class="styleFieldAlign" runat="server" style="width: 55%">
                                    <table id="Table1" runat="server" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td class="styleFieldAlign">
                                                &nbsp;<asp:TextBox ID="txtNLDays" runat="server" AutoPostBack="true" OnTextChanged="txtNLDays_TextChanged" MaxLength="5" Width="45px" TabIndex="4"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="rfvNLDays" runat="server" ErrorMessage="Enter the NL Hours"
                                                    ControlToValidate="txtNLDays">&nbsp;</asp:RequiredFieldValidator>--%>
                                                <cc1:MaskedEditExtender ID="NLDays" runat="server" InputDirection="RightToLeft" ClearMaskOnLostFocus="true"
                                                    Mask="99.99" Enabled="true" MaskType="Number" TargetControlID="txtNLDays">
                                                </cc1:MaskedEditExtender>
                                            </td>
                                            <td class="styleFieldLabel">
                                                NL SMS
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="CbxNLSms" runat="server" TabIndex="5" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                NL EMAIL
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="CbxNLEmail" runat="server" TabIndex="6" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                NL CC1
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="CbxNLcc1" runat="server" TabIndex="7" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 20%">
                                </td>
                            </tr>
                            <tr id="Tr6" runat="server">
                                <td id="Td10" align="left" class="styleFieldLabel" runat="server" width="20%">
                                    <asp:Label runat="server" CssClass="styleDisplayLabel" Text="HL Hours" ID="lblHLDays"></asp:Label>
                                </td>
                                <td id="Td11" align="left" class="styleFieldAlign" runat="server" style="width: 55%">
                                    <table id="Table2" runat="server" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td class="styleFieldAlign">
                                                &nbsp;<asp:TextBox ID="txtHLDays" runat="server" AutoPostBack="true" OnTextChanged="txtHLDays_TextChanged" MaxLength="5" Width="45px" TabIndex="8"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="rfvHLDays" runat="server" ErrorMessage="Enter the HL Hours"
                                                    ControlToValidate="txtHLDays">&nbsp;</asp:RequiredFieldValidator>--%>
                                                <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtHLDays"
                                                    InputDirection="RightToLeft" ClearMaskOnLostFocus="true" Mask="99.99" Enabled="true"
                                                    MaskType="Number">
                                                </cc1:MaskedEditExtender>
                                            </td>
                                            <td class="styleFieldLabel">
                                                HL SMS
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="CbxHLSms" runat="server" TabIndex="9" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                HL EMAIL
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="CbxHLEmail" runat="server" TabIndex="10" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                HL CC1
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="CbxHLcc1" runat="server" TabIndex="12" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                HL CC2
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="CbxHLcc2" runat="server" TabIndex="13" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 20%">
                                </td>
                            </tr>
                            <tr id="Tr7" runat="server">
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Active" ID="lblActive"></asp:Label>
                                </td>
                                <td id="Td13" class="styleFieldAlign" runat="server" style="width: 55%">
                                    &nbsp;&nbsp;
                                    <asp:CheckBox ID="CbxActive" runat="server" TabIndex="14" Checked="True" Enabled="False" />
                                </td>
                                <td style="width: 20%">
                                </td>
                            </tr>
                        </table>
                        <tr>
                            <td style="height: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                                                OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" TabIndex="15" />
                                            
                                            <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                                                CausesValidation="False" OnClick="btnClear_Click" TabIndex="16" />
                                            <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
                                                Enabled="True" TargetControlID="btnClear">
                                            </cc1:ConfirmButtonExtender>
                                           
                                            <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" Text="Cancel"
                                                CausesValidation="False" OnClick="btnCancel_Click" TabIndex="16" />
                                            
                                            <asp:Button runat="server" ID="btnDelete" CssClass="styleSubmitButton" Text="Delete"
                                                CausesValidation="False" OnClick="btnDelete_Click" />
                                        </td>
                                    </tr>
                                    <tr id="Tr8" class="styleButtonArea" runat="server">
                                        <td id="Td14" runat="server">
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                                                font-size: medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="Tr10" runat="server" style="height: 220px" valign="top">
                                        <td id="Td15" colspan="5" runat="server">
                                            <asp:ValidationSummary ID="vsEscalSummary" runat="server" Height="30px" Width="716px"
                                                CssClass="styleMandatoryLabel" ShowSummary="true" HeaderText="Correct the following validation(s):  " />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
