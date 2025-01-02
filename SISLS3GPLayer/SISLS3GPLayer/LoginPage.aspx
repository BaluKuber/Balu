<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Smartlend 3G :: Login ::</title>
    <link rel="shortcut icon" href="Images/TemplateImages/1/CompanyLogo_Old.png" />
</head>
<body>

    <form id="form1" runat="server">

        <script type="text/javascript">
            function PassEnterKey() {
                if (event.keyCode == 13)
                    document.getElementById('<%=imgbtnLogin.ClientID %>').click();
            }
        </script>

        <div id="wrapper">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div class="container">
                <div class="headerPart" style="background-image: url(Images/login/hdr_bg.png); background-repeat: no-repeat; background-position: right;">
                    <a href="#">
                        <img src="Images/Opclogo_new.png" border="0" width="180px" /></a>
                </div>
                <div class="main">
                    <div class="mainLeft">
                        <div class="contentPart">
                            <div class="contentTopImgPart">
                                <img src="Images/login/content_leftside_topcurve.gif" border="0" class="fll" />
                            </div>
                            <div class="contentCenterImgPart" runat="server" id="dvText" style="height: auto">
                                <%--<p class="fll">
                                SmartLend 3G is a lending suite designed for those who originate and service loans.
                                Its modular design includes an easy to configure and maintain grants management
                                system, loan origination, loan administration, collateral and an insurance management
                                system. Engineered for ease of use, this application collaborates fully with Microsoft
                                Office suite of products and is designed to make your loan servicing business more
                                profitable and more efficient.</p>
                            <p class="fll" style="margin-top: 7px;">
                                The software is ready adaptable to your way of doing business. You can easily configure
                                and govern the data flow and tasks from one employee to another. Automation of these
                                processes ensures establishment of a standard operating procedure that is consistently
                                followed, elimination of unnecessary jobs, and makes every member fully aware of
                                his or her responsibilities.</p>--%>
                            </div>
                            <div class="contentBottomImgPart">
                                <img src="Images/login/content_leftside_bottomcurve.gif" border="0" class="flr" />
                            </div>
                        </div>
                        <div class="fll" style="padding-left: 30px">
                            <img src="Images/login/girl_bottom_left.png" border="0" height="280px" />
                        </div>
                    </div>
                    <!--end of main left part-->
                    <div class="mainRight">
                        <div class="clientLogoPart">
                            <div class="clientLogoPartLeft">
                            </div>
                            <%--<div class="clientLogoCenterPart">
                            <a href="#">
                                <img src="Images/login/abc_logo.jpg" border="0" class="abcLogo" /></a>
                        </div>--%>
                            <div class="clientLogoPartRight">
                            </div>
                        </div>
                        <div class="clr">
                        </div>
                        <div class="clientLoginPart">
                            <div class="clientLoginTopCurvePart">
                                <a href="#">
                                    <img src="Images/login/loginBox_left_topcurve.gif" border="0" class="fll" /></a>
                                <a href="#">
                                    <img src="Images/login/loginBox_right_topcurve.gif" border="0" class="flr" /></a>
                            </div>
                            <div class="clientLoginTxtPartBg">
                                <div class="clientLoginUserPwordPart">
                                    <ul>
                                        <li>
                                            <label for="userid">
                                                User ID :</label><asp:TextBox ID="txtUserCode" runat="server" MaxLength="15" class="useridimg"></asp:TextBox></li>
                                        <li>
                                            <label for="pword">Password :</label>
                                            <asp:TextBox ID="txtPassword" runat="server" class="pwordimg" TextMode="Password"
                                                Style="font-family: Verdana" onkeypress="return PassEnterKey()" MaxLength="30"></asp:TextBox>
                                        </li>
                                    </ul>
                                    <div class="loginButton">
                                        <asp:ImageButton ID="imgbtnLogin" runat="server" ImageUrl="~/Images/login/login_button.gif" ValidationGroup="Login"
                                            OnClick="imgbtnLogin_Click" />

                                        <cc1:ModalPopupExtender ID="modalPopSession" runat="server" PopupControlID="PnlPopup"
                                            TargetControlID="BtnModel" CancelControlID="btnNo" BackgroundCssClass="styleModalBackground">
                                        </cc1:ModalPopupExtender>
                                        <asp:Panel ID="PnlPopup" runat="server" Style="vertical-align: middle" runat="server"
                                            BorderStyle="Solid" BackColor="White" Width="300px">
                                            <table id="Table1" runat="server" border="0" width="100%">
                                                <tr>
                                                    <td colspan="2" height="50px"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblSession" runat="server"
                                                            Text="The same user's Login exist in another session, </BR> Do you want to continue?." CssClass="styleDisplayLabel"> </asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" height="10px"></td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <asp:Button ID="btnYes" runat="server" CssClass="styleSubmitButton" Text="Yes"
                                                            OnClick="btnYes_Click" CausesValidation="false" />
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnNo" runat="server" CssClass="styleSubmitButton" Text="No"
                                                            OnClick="btnNo_Click" CausesValidation="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" height="50px"></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Button ID="BtnModel" runat="server" Style="display: none" CausesValidation="false" />

                                    </div>
                                </div>
                                <div class="clr">
                                </div>
                            </div>
                            <div class="clientLoginButtonPart">
                                <div class="ppolicy">
                                    <a href="#">Password Policy</a>
                                </div>
                                <div class="sepimg">
                                    <img src="Images/login/login_bottombutton_seperate.gif" border="0" />
                                </div>
                                <div class="fpword">
                                    <asp:LinkButton ID="lnkForgetPass" runat="server" Text="Forgot your Password?" ValidationGroup="ForgetPass"
                                        OnClick="lnkForgetPass_Click" ToolTip="Forgot your Password?" OnClientClick="return confirm('Do you want to reset password?');">
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvUserCode" runat="server" ErrorMessage="User ID must be entered"
                                    ControlToValidate="txtUserCode" Display="None" CssClass="styleMandatoryLabelLogin"
                                    ValidationGroup="Login"></asp:RequiredFieldValidator>
                                <br />
                                <asp:RequiredFieldValidator ID="rvPassword" runat="server" ErrorMessage="Password must be entered"
                                    ControlToValidate="txtPassword" Display="None" CssClass="styleMandatoryLabelLogin"
                                    ValidationGroup="Login"></asp:RequiredFieldValidator>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvUserCode1" runat="server" ErrorMessage="User ID must be entered"
                                    ControlToValidate="txtUserCode" Display="None" CssClass="styleMandatoryLabelLogin"
                                    ValidationGroup="ForgetPass"></asp:RequiredFieldValidator>
                                <br />

                                <asp:CustomValidator ID="cvLogin" runat="server" Display="Dynamic" CssClass="styleMandatoryLabelLogin"></asp:CustomValidator>
                                <asp:ValidationSummary ID="vsLogin" DisplayMode="List" ValidationGroup="Login" runat="server" />
                                <asp:ValidationSummary ID="VsForgetPass" ValidationGroup="ForgetPass" runat="server" />
                            </div>
                        </div>
                        <a href="#">
                            <img src="Images/login/rightEnd_bottom_img_new.gif" border="0" width="312" height="258"
                                class="mainRightBottom" /></a>
                    </div>
                    <!--end of main right part-->
                </div>
                <!--end of main-->
                <div class="footer">
                    <p>
                        <a href="#">Copyright &copy; 2010 Sundaram Infotech Solutions Limited. Optimised for
                        Internet Explorer 7.0 & above. Recommended Resolution 1024x768 pixels. (Version
                        1.52)</a>
                    </p>
                </div>
            </div>
        </div>

        <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="false" Visible="false">
            <asp:ListItem Text="English" Value=""></asp:ListItem>
            <asp:ListItem Text="Hindi" Value="hi-IN"></asp:ListItem>
            <asp:ListItem Text="French" Value="fr-FR"></asp:ListItem>
        </asp:DropDownList>
    </form>
</body>
</html>
