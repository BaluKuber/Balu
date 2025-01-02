<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GRptEntityLedgerReport.aspx.cs" Inherits="Reports_S3GRptEntityLedgerReport" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Entity Ledger Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <CR:CrystalReportViewer ID="CRVVendor" runat="server" AutoDataBind="True" Height="50px" Width="350px" EnableDatabaseLogonPrompt="false" EnableParameterPrompt="false" EnableDrillDown="false" />

    </div>
    </form>
</body>
</html>
