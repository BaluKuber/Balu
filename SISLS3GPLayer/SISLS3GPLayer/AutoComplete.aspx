<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoComplete.aspx.cs" Inherits="AutoComplete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="~/UserControls/AutoCompleteTextBox.ascx" TagName="AutoComplete"
    TagPrefix="asp" %>
<%@ Register Src="~/UserControls/AutoCompleteTextBox.ascx" TagName="AutoComplete"
    TagPrefix="asp1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="Scripts/JQuery-1.9.2/css/ui-lightness/jquery-ui-1.9.2.custom.min.css"
        rel="stylesheet" type="text/css" />

    <script src="Scripts/JQuery-1.9.2/js/jquery-1.8.3.js" type="text/javascript"></script>

    <script src="Scripts/JQuery-1.9.2/js/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
     <asp:ScriptManager  ID="sm" runat="server" ScriptMode="Release"  ></asp:ScriptManager>
    <div>
        <asp:UpdatePanel ID="Up" runat="server">
            <ContentTemplate>
                <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Chennai" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Hyderabad" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField ID="hdnT" runat="server" Value="Hel" />
                <asp:AutoComplete ID="AutoTxt" MinLength="3" runat="server" OnSelectedIndexChanged="Index_Changed" />
                <asp1:AutoComplete ID="AutoComplete1" MinLength="3" runat="server" OnSelectedIndexChanged="Index_Changed" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ddlLocation" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
