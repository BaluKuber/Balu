<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SessionReconnect.aspx.cs" Inherits="SessionReconnect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript" language="javascript">
    function Reconnect()
    {
       __doPostBack("Button1","");
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        For your safety and protection, your session is about to expire. do you want to 
        extend it ?<br />
        <asp:Button ID="Button1" runat="server" Text="Reconnect" />
        <asp:Button ID="Button2" runat="server" Text="Close" />
        <br />
    
    </div>
    </form>
</body>
</html>
