<%@ Control Language="C#" AutoEventWireup="true" CodeFile="S3GGetRsNo.ascx.cs"
    Inherits="UserControls_S3GGetRsNo" %>
<asp:TextBox ID="txtName" runat="server" ></asp:TextBox>
<asp:Button ID="btnGetLOV" runat="server" Text="Copy RS" CausesValidation="false" CssClass="styleSubmitShortButton"
    onclick="btnGetLOV_Click" />
<asp:HiddenField ID="hdnID" runat="server" />
<asp:HiddenField ID="hdnCtrlId" runat="server" />
<asp:HiddenField ID="hdnLovCode" runat="server" />

