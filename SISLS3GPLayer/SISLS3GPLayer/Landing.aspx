<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Landing.aspx.cs" Inherits="LandingPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Smartlend 3G :: LandingPage ::</title>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release"></cc1:ToolkitScriptManager>
    
   
    <asp:UpdatePanel ID="updMain" runat="server">
        <ContentTemplate>
            <div id="wrapper">
                <div class="container">
                    <div class="headerPart">
                        <a href="#">
                            <img src="Images/login/s3g_logo.png" border="0" /></a>
                    </div>
                    <table width="100%">
                        <tr>
                            <td align="center" width="100%" style="padding-top: 60px;">
                                <%--<cc1:RoundedCornersExtender ID="round" runat="server" TargetControlID="fdg" Color="#e1e6e9"
                            Radius="4" Corners="All">
                        </cc1:RoundedCornersExtender>--%>
                                <table id="fdg" runat="server" style="border-color: #e1e6e9; border-width: 1px; border-style: solid;
                                    background-image: url(images/login/content_bg.gif); background-repeat: repeat-x;
                                    width: 70%">
                                    <tr>
                                        <td style="widows: 60%;" valign="top">
                                            <table align="center" width="100%">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:RadioButtonList ID="rdbtnList" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                                            OnSelectedIndexChanged="rdbtnList_SelectedIndexChanged">
                                                            <asp:ListItem Text="New User" Selected="True" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Existing User" Value="0"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 30px; width: 40%">
                                                        <label for="nameimg">
                                                            Full Name :</label>
                                                    </td>
                                                    <td width="60%">
                                                        <asp:TextBox ID="txtFullName" runat="server" MaxLength="50" class="nameimg"></asp:TextBox></li>
                                                        <cc1:FilteredTextBoxExtender ID="FFullName" runat="server" FilterType="custom,LowercaseLetters,UppercaseLetters"
                                                            TargetControlID="txtFullName" ValidChars=" ">
                                                        </cc1:FilteredTextBoxExtender>
                                                      <%--  <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Full Name"
                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 30px">
                                                        <label for="mobileimg">
                                                            Mobile Number :</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtMobilenumber" MaxLength="12" runat="server" class="mobileimg">
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtxtMobilenumber" runat="server" Enabled="True"
                                                            FilterType="Numbers" TargetControlID="txtMobilenumber">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="RtxtMobilenumber" runat="server" ControlToValidate="txtMobilenumber"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Mobile Number"
                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" style="padding-left: 30px">
                                                        <label for="emailimg">
                                                            Email ID :</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtemail" runat="server" MaxLength="60" class="emailimg"></asp:TextBox></li>
                                                        <asp:RegularExpressionValidator ID="revemail" runat="server" ControlToValidate="txtemail"
                                                            Display="None" ErrorMessage="Enter a valid Email Id" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ValidationGroup="Save"></asp:RegularExpressionValidator>
                                                     <%--   <asp:RequiredFieldValidator ID="Remail" runat="server" ControlToValidate="txtemail"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Email ID" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="tr1">
                                                    <td align="left" style="padding-left: 30px">
                                                        <label for="Desgimg">
                                                            Desgination :</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtDesg" runat="server" MaxLength="30" class="Desgimg"></asp:TextBox></li>
                                                        <cc1:FilteredTextBoxExtender ID="FDesg" runat="server" FilterType="custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                            TargetControlID="txtDesg" ValidChars=" ">
                                                        </cc1:FilteredTextBoxExtender>
                                                       <%-- <asp:RequiredFieldValidator ID="RDesg" runat="server" ControlToValidate="txtDesg"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Desgination"
                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="tr2">
                                                    <td align="left" style="padding-left: 30px">
                                                        <label for="compimg">
                                                            Company Name :</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcomp" runat="server" MaxLength="60" class="compimg"></asp:TextBox></li>
                                                        <cc1:FilteredTextBoxExtender ID="Fcomp" runat="server" FilterType="custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                            TargetControlID="txtcomp" ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="Rcomp" runat="server" ControlToValidate="txtcomp"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Email ID"
                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="tr3">
                                                    <td align="left" style="padding-left: 30px" valign="top">
                                                        <label for="operaimg">
                                                            Brief about your operations :</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtopera" runat="server" Height="50px" MaxLength="200" class="operaimg"
                                                            TextMode="MultiLine"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fopera" runat="server" FilterType="custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                            TargetControlID="txtopera" ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                       <%-- <asp:RequiredFieldValidator ID="Ropera" runat="server" ControlToValidate="txtopera"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Brief about your operations"
                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="tr4">
                                                    <td align="left" style="padding-left: 30px" valign="top">
                                                        <label for="currentimg">
                                                            Brief about your current lending software system :</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtcurrent" Height="50px" runat="server" MaxLength="100" TextMode="MultiLine"
                                                            class="currentimg"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="Fcurrent" runat="server" FilterType="custom,LowercaseLetters,UppercaseLetters,Numbers"
                                                            TargetControlID="txtcurrent" ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="Rcurrent" runat="server" ControlToValidate="txtcurrent"
                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Brief about your current lending software system"
                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="widows: 40%; padding-right: 10px" valign="top" align="left">
                                            <div class="contentLoadCenterImgPart" style="border-bottom-style: solid; border-bottom-color: #e1e6e9;
                                                border-bottom-width: 1px">
                                                <br />
                                                <p class="fll">
                                                    <h4>
                                                        Terms and conditions:</h4>
                                                    <p>
                                                    </p>
                                                    <p>
                                                        <ol>
                                                            <li>1. Sundaram Infotech Solutions Ltd (SISL) is the sole and exclusive owner of the
                                                                software – SmartLend 3G available on this server and, upon acceptance of these terms
                                                                and conditions, SISL grants you free of charge a non-exclusive, non-transferable
                                                                Licence to use the software upon the said terms and subject to the said conditions.
                                                                The proprietary rights and copyright of the software remain with SISL.</li>
                                                            <li>2. Use of the software for a limited period as preferred by SISL from time to time.
                                                            </li>
                                                            <li>3. You will supervise and control use of the software in accordance with these terms
                                                                and conditions.</li>
                                                            <li>4. The software shall not be put to any other use / redistributed to a third party
                                                                in any form in whole or in part without prior written agreement from SISL.</li>
                                                            <li>5. SISL does not undertake to assist in transporting or adapting the software to
                                                                your machines.</li>
                                                            <li>6. SISL will not undertake any maintenance or other support of the software.</li>
                                                            <li>7. The decision of going through this demo version of SmartLend 3G is purely out
                                                                of your own interest and SISL does not undertake any responsibility for any consequences.</li>
                                                            <li>8. These terms and conditions are in lieu of all warranties, conditions, terms,
                                                                undertakings and obligations implied by statute, common law, custom, trade usage,
                                                                course of dealing or otherwise, all of which are hereby excluded to the fullest
                                                                extent permitted by law.</li>
                                                            <li>9. If you accept these terms and conditions please press Accept to proceed with
                                                                the download of the software.</li>
                                                        </ol>
                                                        <p>
                                                        </p>
                                                    </p>
                                                </p>
                                            </div>
                                            <br />
                                            <asp:RadioButtonList ID="rbtAcceptCondtions" runat="server" AutoPostBack="false"
                                                RepeatDirection="Vertical">
                                                <asp:ListItem Text="I/We accept the terms and conditions set out as above." Value="0"></asp:ListItem>
                                                <asp:ListItem Text="I Decline to comply with above" Value="1"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RequiredFieldValidator ControlToValidate="rbtAcceptCondtions" SetFocusOnError="true"
                                                ValidationGroup="Save" Display="None" runat="server" ID="rfvAccept" ErrorMessage="Accept / Decline Terms and conditions"></asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="btnGo" Text="Login" runat="server" ImageUrl="~/Images/login/login_button.gif"
                                                Style="float: right" OnClick="btnGo_Click" ValidationGroup="Save" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                       <%-- <tr>
                            <td align="left">
                                <asp:ValidationSummary ID="vsLoading" ShowSummary="false" ShowMessageBox="true" runat="server" />
                                <asp:ValidationSummary ID="vsLoading1" runat="server" CssClass="styleMandatoryLabel"
                                    HeaderText="Correct the following validation(s):  " ValidationGroup="Save" ShowSummary="true" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="left">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFullName"
                                    CssClass="styleMandatoryLabelLogin" Display="Dynamic" ErrorMessage="Enter the Full Name"
                                    ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator></br>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtMobilenumber"
                                    CssClass="styleMandatoryLabelLogin" Display="Dynamic" ErrorMessage="Enter the Mobile Number"
                                    ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator></br>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtemail"
                                    CssClass="styleMandatoryLabelLogin" SetFocusOnError="true" Display="Dynamic"
                                    ErrorMessage="Enter Email ID" ValidationGroup="Save"></asp:RequiredFieldValidator></br>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDesg"
                                    SetFocusOnError="true" CssClass="styleMandatoryLabelLogin" Display="Dynamic"
                                    ErrorMessage="Enter the Desgination" ValidationGroup="Save"></asp:RequiredFieldValidator></br>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtcomp"
                                    CssClass="styleMandatoryLabelLogin" Display="Dynamic" ErrorMessage="Enter the Company Name"
                                    ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator></br>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtopera"
                                    CssClass="styleMandatoryLabelLogin" Display="Dynamic" ErrorMessage="Enter the Brief about your operations"
                                    SetFocusOnError="true" ValidationGroup="Save"></asp:RequiredFieldValidator></br>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtcurrent"
                                    SetFocusOnError="true" CssClass="styleMandatoryLabelLogin" Display="Dynamic"
                                    ErrorMessage="Enter the Brief about your current lending software system" ValidationGroup="Save"></asp:RequiredFieldValidator></br>
                                <asp:RequiredFieldValidator ControlToValidate="rbtAcceptCondtions" SetFocusOnError="true"
                                    CssClass="styleMandatoryLabelLogin" ValidationGroup="Save" Display="Dynamic"
                                    runat="server" ID="RequiredFieldValidator8" ErrorMessage="Accept / Decline Terms and conditions"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                    <div class="mainLoaRight">
                        <%--<div class="contentTopImgPart">
                    <img src="Images/login/content_leftside_topcurve.gif" border="0" class="fll" />
                </div>--%>
                    </div>
                </div>
                <div class="footer">
                    <p>
                        <a href="#">Copyright &copy; 2013 Sundaram Infotech Solutions Limited. Optimised for
                            Internet Explorer 7.0 & above. Recommended Resolution 1024x768 pixels. (Version
                            1.52)</a></p>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:DropDownList ID="ddlLanguage" runat="server" AutoPostBack="false" Visible="false">
        <asp:ListItem Text="English" Value=""></asp:ListItem>
        <asp:ListItem Text="Hindi" Value="hi-IN"></asp:ListItem>
        <asp:ListItem Text="French" Value="fr-FR"></asp:ListItem>
    </asp:DropDownList>
    </form>
</body>
</html>
