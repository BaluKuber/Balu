<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="S3GSysAdminRoleCenterMaster_Add.aspx.cs"
    Inherits="System_Admin_S3GSysAdminRoleCenterMaster_Add" %>

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
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" width="900px">
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Role Center Code" ID="lblRoleCenterCode" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <cc1:ComboBox ID="cmbRoleCenterCode" runat="server" CssClass="WindowsStyle1" DropDownStyle="Simple"
                                        Width="150px" AutoPostBack="True" onkeyup="maxlengthfortxt(1);" AppendDataBoundItems="true" maxlength="1"
                                        ItemInsertLocation="Append" AutoCompleteMode="SuggestAppend" OnItemInserted="cmbRoleCenterCode_ItemInserted"
                                        TabIndex="1" OnSelectedIndexChanged="cmbRoleCenterCode_SelectedIndexChanged">
                                    </cc1:ComboBox>
                                <%-- onkeyup="maxlengthfortxt(1);" --%>
                                    <td style="width: 209px">
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="0" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="cmbRoleCenterCode" SetFocusOnError="True" ErrorMessage="Select/Enter Role Center Code"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                        <asp:RegularExpressionValidator ID="revUTPACode" runat="server" Display="None" ControlToValidate="cmbRoleCenterCode"
                                            ErrorMessage="Select/Enter a valid Role Center Code(1-9,A-Z)" CssClass="styleMandatoryLabel"
                                            SetFocusOnError="True" ValidationExpression="^[1-9a-zA-Z]+$"></asp:RegularExpressionValidator>
                                    </td>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Role Center Name" ID="lblRoleCentrName" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtRoleCenterName" runat="server" MaxLength="40" Width="235px" TabIndex="2"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtRoleCenterName"
                                        FilterType="UppercaseLetters, LowercaseLetters,Numbers,Custom" ValidChars=" "
                                        Enabled="true">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <td style="width: 209px">
                                    <asp:RequiredFieldValidator ID="rfvProductCode" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="txtRoleCenterName" ErrorMessage="Enter Role Center Name" Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Module" ID="lblModule" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlModule" runat="server" Style="width: 240px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" TabIndex="3" 
                                        onmouseover="ddl_itemchanged(this);">
                                    </asp:DropDownList>   <%--OnTextChanged="ddlModule_TextChanged" --%>
                                </td>
                                <td style="width: 209px">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel"
                                        runat="server" ControlToValidate="ddlModule" InitialValue="0" ErrorMessage="Select Module"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Program" ID="lblProgram" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlProgram" runat="server" Style="width: 240px" TabIndex="4"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"
                                        onmouseover="ddl_itemchanged(this);" ToolTip="--Select--">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 209px">
                                    <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlProgram" InitialValue="0" ErrorMessage="Select Program"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td class="styleFieldLabel">
                                    Workflow Program
                                </td>
                                <td class="styleFieldAlign">
                                    <%--<asp:DropDownList ID="ddlWFProgram" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlWFProgram_SelectedIndexChanged"
                                        Style="width: 240px" TabIndex="4">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="txtWFProgram" runat="server" MaxLength="3" Width="50px" AutoPostBack="True"
                                        OnTextChanged="txtWFProgram_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FTE1" runat="server" TargetControlID="txtWFProgram"
                                        FilterType="Numbers,Custom" Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <td style="width: 209px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Role Code" ID="lblRolecode" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtRoleCode" runat="server" MaxLength="5" ReadOnly="True" Style="text-transform: uppercase"
                                        Width="107px"></asp:TextBox>
                                </td>
                                <td style="width: 209px">
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active">
                                    </asp:Label>
                                </td>
                                <td colspan="2" class="styleFieldAlign">
                                    <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="5" ToolTip="Active" />
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    &nbsp;
                                </td>
                                <td class="styleFieldAlign" colspan="2">
                                    <table>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                                    OnClientClick="return fnCheckPageValidators();" TabIndex="6" Text="Save" ToolTip="Save" />
                                                <asp:Button ID="btnClear" runat="server" CausesValidation="false" class="styleSubmitButton"
                                                    OnClick="btnClear_Click" TabIndex="7" Text="Clear" ToolTip="Clear" />
                                                <cc1:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" ConfirmText="Do you want to clear?"
                                                    TargetControlID="btnClear">
                                                </cc1:ConfirmButtonExtender>
                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                                    OnClick="btnCancel_Click" TabIndex="8" Text="Cancel" ToolTip="Cancel" />
                                                <asp:Button ID="btnDelete" runat="server" AccessKey="I" CausesValidation="False"
                                                    ToolTip="Delete" CssClass="styleSubmitButton" OnClick="btnDelete_Click" Text="Delete"
                                                    Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="center">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="RoleCenterMasterDetailsAdd" runat="server" ShowSummary="true"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
     <script language="javascript" type="text/javascript">
     
    function fnAvoidZero(isSpaceAllowed)
                {
                //debugger;
                var sKeyCode = window.event.keyCode;
                //alert(sKeyCode);
                if ((!isSpaceAllowed) && (sKeyCode == 32))
                 {
                    window.event.keyCode = 0;
                    return false;
                }
                
                if (sKeyCode == 48 || sKeyCode == 96)
                {
                 window.event.keyCode = 0;
                    return false;
                }

//                if ((sKeyCode < 48 || sKeyCode > 57) && sKeyCode != 32 && sKeyCode != 95 && sKeyCode != 46) {
//                    window.event.keyCode = 0;
//                    return false;
//                }


            }
            </script>
</asp:Content>
