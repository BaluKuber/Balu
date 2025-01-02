﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GRptSanctionDetailsReport.aspx.cs" Inherits="Reports_S3GRptSanctionDetailsReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sanction Details Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CRVSanctoin" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="false" EnableDrillDown="false" Height="50px" Width="350px" HasToggleGroupTreeButton="false" ToolPanelView="None"/>
    </div>
    </form>
</body>
</html>
