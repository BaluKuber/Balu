<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SessionExpired.aspx.cs" Inherits="SessionExpired" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Session Expired</title>
</head>
<body>
    <form id="form1" runat="server">
        <%-- <div style="font-family:Calibri,Verdana;font-size:14px;color:Navy">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/warning.gif" />
        Session Expired.<a href=LoginPage.aspx target="_parent"> Login </a> again.
    </div>--%>

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <cc1:DropShadowExtender ID="dseProspect" runat="server" TargetControlID="pnlProspectView"
            Opacity=".3" Rounded="false" TrackPosition="true" />
        <table style="height:400px" width="100%">
            <tr>
                <td align="center" valign="middle">
                    <table cellpadding="0" cellspacing="0" width="35%" align="center" style="height: 120px;">
                        <tr>
                            <td width="100%" valign="top">
                                <asp:Panel ID="pnlProspectView" runat="server" Style="display: block; height: 120px"
                                    Width="350px" BackColor="White" BorderWidth="1px" BorderColor="#e1e6e9">
                                    <table cellpadding="0" cellspacing="0" width="100%" style="height: 120px">
                                        <tr>
                                            <td width="100%" align="right" valign="top" style="">
                                                <img src="Images/Blue_2/s3g_logo.gif" style="position: absolute; height: 20px; padding-left: 10px; padding-top: 5px" />
                                                <img src="Images/tab_hdr.png" style="width: 100%; height: 30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" align="center" style="height: 80px; background-image: url(images/login/login_box_bg_01.gif); background-repeat: repeat-x; border-bottom-color: #e1e6e9; border-bottom-style: solid; border-width: 1px;">
                                                <font style="color: #808080; font-family: calibri,Arial, Sans-Serif; font-size: 16px; font-weight: bold;">Session Expired.</font>
                                                <a href="LoginPage.aspx" target="_parent" style="border: 0px; width: 78px; height: 22px; color: black; font-family: calibri,Arial, Sans-Serif; font-size: 16px; cursor: pointer; text-decoration: none">
                                                    <img src="Images/btn_silver.jpg" style="border:none; margin-bottom:-5px" />
                                                    <font style="color: #336699; font-family: calibri,Arial, Sans-Serif; font-size: 16px; padding-bottom: 5px; margin-left:-60px; position:absolute">Login</font></a>
                                                <font style="color: #808080; font-family: calibri,Arial, Sans-Serif; font-size: 16px; font-weight: bold;">&nbsp;again</font>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
