<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccessDenied.aspx.cs" Inherits="AccessDenied" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
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
                                            <td colspan="2">
                                                 <img src="Images/tab_hdr.png" style="width: 100%; height: 30px" />
                                            </td>

                                        </tr>
                                          <tr>
                                            <td valign="middle" align="center" style="height: 80px; background-image: url(images/login/login_box_bg_01.gif); background-repeat: repeat-x; border-bottom-color: #e1e6e9; border-bottom-style: solid; border-width: 1px;">
                                                <img src="Images/Task_pending.jpg" style="height: 33px; width: 43px;" />
                                            </td>
                                            <td>
                                                <b style="font-size: x-large;">Access Denied!</b>
                                                Contact Service Provider
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
