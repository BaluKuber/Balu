<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PDCReminder.aspx.cs" Inherits="Reports_PDC_Reminder_" Title="PDC Reminder Report" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <CR:CrystalReportViewer ID="CRVPDCReminder" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="false" EnableParameterPrompt="false" EnableDrillDown="false" HasToggleGroupTreeButton="false" ToolPanelView="None"/>
    
    
    </div>
    </form>
</body>
</html>
