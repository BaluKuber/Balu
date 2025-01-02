<%@ Page Language="C#" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script type="text/javascript" language="javascript">
    function onload() {
         var ie7 = (document.all && !window.opera && window.XMLHttpRequest) ? true : false;

       if (ie7) {
           window.open('', '_self', '');
           window.close();
       }
       else {
           this.focus();

           self.opener = this;

           self.close();
       }

        window.open("LoginPage.aspx","_blank","toolbar=no, location=no, directories=no, status=yes, menubar=no, scrollbars=yes, resizable=no, copyhistory=yes, width=" +(screen.width-10)+", height=" + (screen.height - 90));
         // window.open("LoginPage.aspx",'DAC_Main1','fullscreen,scrollbars=no');
        
        
    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="onload()">
    <form id="form1" runat="server">
    
       </form>
</body>
</html>
