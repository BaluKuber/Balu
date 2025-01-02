<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminCustomerAlert_Add.aspx.cs" Inherits="System_Admin_S3GSysAdminCustomerAlert_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Entity Type" ID="lblEntitytype" CssClass="styleDisplayLabel">
                                    </asp:Label><span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlEntityType" runat="server" Style="width: 240px" onmouseover="ddl_itemchanged(this);">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 209px">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="styleMandatoryLabel"
                                        runat="server" InitialValue="0" ControlToValidate="ddlEntityType" ErrorMessage="Select Entity Type"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlLOB" runat="server" Style="width: 240px" 
                                        AutoPostBack="True" onselectedindexchanged="ddlLOB_SelectedIndexChanged" onmouseover="ddl_itemchanged(this);">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 209px">
                                    <asp:RequiredFieldValidator ID="rfvlob" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlLOB" ErrorMessage="Select Line of Business" InitialValue="0" Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Role Description" ID="lblRoleCode" CssClass="styleDisplayLabel">
                                    </asp:Label><span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlRoleCode" runat="server" Style="width: 240px" onmouseover="ddl_itemchanged(this);">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 209px">
                                    <asp:RequiredFieldValidator ID="rfvRoleDesc" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlRoleCode" ErrorMessage="Select Role Description" InitialValue="0" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlRoleCode" ErrorMessage="Select Role Description" Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                              <%--  <td colspan="2" style="width: 209px">
                                </td>--%>
                            </tr>
                            <%--Commented by saranya based on sudarsan observation--%>
                            <%--<tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Event" ID="lblEventtype" CssClass="styleDisplayLabel">
                                    </asp:Label><span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlEventType" runat="server" Style="width: 240px" onmouseover="ddl_itemchanged(this);">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 209px">
                                    <asp:RequiredFieldValidator ID="rfveventtype" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlEventType" InitialValue="0" ErrorMessage="Select Event"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>
                           <%-- End Here--%>
                            <tr>
                                <%--<td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Target SMS" ID="lblsms" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlSMS" runat="server" Style="width: 240px">
                                    </asp:DropDownList>
                                </td>--%>
                                <%--<td colspan="2" style="width: 209px">--%>
                                <td class="styleFieldLabel">
                                                <asp:Label runat="server" Text="Target SMS" ID="lblsms" CssClass="styleDisplayLabel">
                                                 
                                    </asp:Label>
                                            </td>
                                <td class="styleFieldAlign">
                               <asp:CheckBox ID="chkSMS" runat="server" ToolTip="Target SMS" />
                               <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="chkSMS"  ErrorMessage="Select Target SMS"
                                        Display="None">
                                    </asp:RequiredFieldValidator>--%>
                               </td>
                                    
                                    
                               <%-- </td>--%>
                            </tr>
                            <tr>
                                <%--<td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Target EMail" ID="lblEmail" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                    <span class="styleMandatory">*</span>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlEmail" runat="server" Style="width: 240px">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2" style="width: 209px">--%>
                                <td class="styleFieldLabel">
                                                 <asp:Label runat="server" Text="Target EMail" ID="lblEmail" CssClass="styleDisplayLabel">
                                                  
                                    </asp:Label>
                                            </td>
                                <td class="styleFieldAlign">
                               <asp:CheckBox ID="chkEMAIL" runat="server" ToolTip="Target EMail" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="chkEMAIL"  ErrorMessage="Select Target EMAIL"
                                        Display="None">
                                    </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign" colspan="3">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" Tooltip = "Active"/>
                                    <%--OnClick="fnCheckPageValidators('chkActive');" --%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="3" align="center">
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                        CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" ToolTip="Save" />
                                    <asp:Button ID="btnClear" CausesValidation="false" runat="server" Text="Clear"
                                        class="styleSubmitButton" OnClick="btnClear_Click" ToolTip="Clear"/>
                                    <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" TargetControlID="btnClear"
                                        ConfirmText="Do you want to clear?" runat="server">
                                    </cc1:ConfirmButtonExtender>
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" ToolTip="Cancel" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td colspan="4">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="RoleCenterMasterDetailsAdd" runat="server" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" CssClass="styleMandatoryLabel" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
