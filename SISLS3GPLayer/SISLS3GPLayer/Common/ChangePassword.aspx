<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    Inherits="ChangePassword" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content runat="server" ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1">

    <script language="javascript" type="text/javascript">
        function fnhideMenuButton()
        {
             document.getElementById('ctl00_imgShowMenu').style.width = '0';
             document.getElementById('ctl00_imgHideMenu').style.width = '0';
             document.getElementById('divMenu').style.width = '0';
        }
        
        function fnhideHeaderControls()
        {
            document.getElementById('tblHeaderControls').style.display = 'none';
        }
        
        function fnCheckNewPWD()
        {
            //debugger;
            var txtNewPWD = (document.getElementById('<%=txtNewPwd.ClientID %>'));
            var txtRetypeNewPwd = (document.getElementById('<%=txtRetypeNewPwd.ClientID %>'));

            if ((txtNewPWD.value != " ") && (txtRetypeNewPwd.value != " ")) 
            {
                 ValidatorEnable(document.getElementById('<%=cvNewPWD.ClientID%>'), true);
            }
            else
            {
                 ValidatorEnable(document.getElementById('<%=cvNewPWD.ClientID%>'), false);
            }
        }
        
        function fnCheckOldPWD()
        {
            var txtOldPWD = (document.getElementById('<%=txtOldPwd.ClientID %>'));
            var txtNewPWD = (document.getElementById('<%=txtNewPwd.ClientID %>'));
            
            if ((txtOldPWD.value.Trim() != " ") && (txtNewPWD.value != " ")) 
            {
                 ValidatorEnable(document.getElementById('<%=cvOldNewPWD.ClientID%>'), true);
            }
            else
            {
                 ValidatorEnable(document.getElementById('<%=cvOldNewPWD.ClientID%>'), false);
            }

            
        }
        
          function fnValidatePWd()
          {
          //debugger;
            var uppercaseVal;
            var NumeralVal;
            var specCharVal;
            var pwdLengVal;
            
            uppercaseVal = document.getElementById('<%=hdnUpperCase.ClientID%>');
            NumeralVal = document.getElementById('<%=hdnNumeral.ClientID%>');
            specCharVal = document.getElementById('<%=hdnSpecChar.ClientID%>');
            pwdLengVal = document.getElementById('<%=hdnPwdLength.ClientID%>');
          }
        
    </script>

    <asp:UpdatePanel ID="UPChangePWD" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td width="100%" align="center" valign="middle">
                        <asp:Panel ID="PnlChangePassword" runat="server" BackColor="White" Width="380px"
                            BorderColor="DimGray" BorderWidth="1px">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="stylePageHeading" width="100%">
                                                    <asp:Label runat="server" Text="Change Password" ID="lblChangePwd" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Old Password" ID="lblOldpwd" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="45%">
                                                    <asp:TextBox ID="txtOldPwd" runat="server" CssClass="pwordimg" TextMode="Password" MaxLength="15"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvOldPwd" runat="server" ControlToValidate="txtOldPwd"
                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter Old password"
                                                        ValidationGroup="btnSaveChgPWD"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="New Password" ID="lblNewPwd" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="padding-right: 10px">
                                                    <asp:TextBox ID="txtNewPwd" EnableTheming="true" runat="server" TextMode="Password" MaxLength="15" onchange="fnCheckOldPWD();"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvNewPwd" runat="server" ControlToValidate="txtNewPwd"
                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter New password"
                                                        ValidationGroup="btnSaveChgPWD"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvOldNewPWD" runat="server" ControlToValidate="txtNewPwd"
                                                        ControlToCompare="txtOldPwd" Display="None" Operator="NotEqual" ErrorMessage="Old and New Password should not be same"
                                                        ValidationGroup="btnSaveChgPWD"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Re-Type New Password" ID="lblRetypeNewPwd" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRetypeNewPwd" EnableTheming="true" runat="server" MaxLength="15" TextMode="Password"
                                                        onchange="fnCheckNewPWD();"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvRetypeNewPwd" runat="server" ControlToValidate="txtRetypeNewPwd"
                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Re-Type New password"
                                                        ValidationGroup="btnSaveChgPWD"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="cvNewPWD" runat="server" ControlToValidate="txtRetypeNewPwd"
                                                        ControlToCompare="txtNewPwd" Display="None" ErrorMessage="New Password and Re-Type PWD must be same"
                                                        ValidationGroup="btnSaveChgPWD"></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center" style="padding-top: 10px">
                                                    <asp:Button ID="btnSaveChgPWD" Text="Change Password" runat="server" ValidationGroup="btnSaveChgPWD"
                                                        OnClick="btnSave_Click" CssClass="styleSubmitButton" CausesValidation="true"
                                                        OnClientClick="return fnCheckPageValidators('btnSaveChgPWD');" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                                                        OnClick="btnCancel_Click" CausesValidation="true" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                    <input type="hidden" id="hdnUpperCase" runat="server"/>
                                    <input type="hidden" id="hdnNumeral" runat="server"/>
                                    <input type="hidden" id="hdnSpecChar" runat="server"/>
                                    <input type="hidden" id="hdnPwdLength" runat="server"/>
                                </tr>
                                <tr class="stylePagingControl">
                                    <td align="left">
                                        <span>*</span>
                                        <asp:Label ID="lblPwdMessage" runat="server" Style="font-size: 11px"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="vsChangePassword" runat="server" HeaderText="Correct the following validation(s):"
                                            CssClass="styleMandatoryLabel" Width="100%" ValidationGroup="btnSaveChgPWD" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
