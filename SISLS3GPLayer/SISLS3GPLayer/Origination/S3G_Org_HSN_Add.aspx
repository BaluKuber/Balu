<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Org_HSN_Add.aspx.cs" Inherits="Origination_S3G_Org_HSN_Add" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="mbcbb" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <table>
                                         <tr>
                                <td class="styleFieldLabel">
                                    <asp:RadioButtonList ID="rdbcode" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbcode_SelectedIndexChanged" CssClass="styleFieldLabel" >
                                        <asp:ListItem Text="HSN Code" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="SAC Code" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>

                                </td>
                            </tr>
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="lblHSNCode" runat="server" CssClass="styleReqFieldLabel" Text="HSN Code"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                              <asp:TextBox ID="txtHSNCode" runat="server" MaxLength="15" Style="width: 240px"
                                                     ToolTip="HSN Description"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvHSNCode" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="txtHSNCode" InitialValue="" ErrorMessage="Enter HSN Code" ValidationGroup="Add"
                                                    SetFocusOnError="True" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 232px"></td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="lblHSNDesc" runat="server" CssClass="styleReqFieldLabel" Text="HSN Description"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtHSNDesc" runat="server" MaxLength="100" Style="width: 240px"
                                                     ToolTip="HSN Description"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvHSNDesc" runat="server" ControlToValidate="txtHSNDesc"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter HSN Description" 
                                                    ValidationGroup="Add" Text="Enter HSN Description">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="LblSAC" runat="server" CssClass="styleReqFieldLabel" Text="SAC Details"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <uc2:Suggest ID="TxtSAC" runat="server" ToolTip="SAC Details" ServiceMethod="GetHSNCode" 
                                                    IsMandatory="true" ValidationGroup = "Add" ErrorMessage="Enter SAC Details" width="240px"/>                                                
                                            </td>

                                        </tr>
                                    
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active" 
                                                    Width="13%"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" ToolTip="Active" Width="40%" />
                                            </td>
                                        </tr>
                                    
                                </td>
                            </tr>
                        </table>
                 
                        <tr>
                            <td>
                                <table align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnSave" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                                Text="Save" TabIndex="4" OnClick="btnSave_Click"  ToolTip="Save" OnClientClick="return fnCheckPageValidators('Add');"
                                                ValidationGroup="Add" />
                                            <%-- OnClientClick="return fnCheckPageValidators();"--%>
                                            <asp:Button ID="btnClear" runat="server" class="styleSubmitButton" CausesValidation="false"
                                                Text="Clear" TabIndex="5" OnClick="btnClear_Click" ToolTip="Clear" />
                                            <mbcbb:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure want to clear?"
                                                TargetControlID="btnClear">
                                            </mbcbb:ConfirmButtonExtender>
                                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" CssClass="styleSubmitButton"
                                                 Text="Cancel" TabIndex="6" ToolTip="Cancel" />
                                        </td>
                                    </tr>
                                </table>
                                <table align="left">
                                    <tr class="styleButtonArea">
                                        <td>
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="HSNDetailsAdd" runat="server" HeaderText="Correct the following validation(s):"
                                                CssClass="styleMandatoryLabel" ShowSummary="true" ValidationGroup="Add" />
                                            <%--  <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Correct the following validation(s):"
                                                CssClass="styleMandatoryLabel" ShowSummary="true" ValidationGroup="Grid" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </td>
                </tr>
                <%-- Added by sathish for grid model popup--%>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

