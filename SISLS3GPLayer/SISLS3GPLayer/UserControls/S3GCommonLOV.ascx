<%@ Control Language="C#" AutoEventWireup="true" CodeFile="S3GCommonLOV.ascx.cs"
    Inherits="UserControls_S3GCommonLOV" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/LOVPageNavigator.ascx" %>
<table>
    <tr>
        <td>
            <asp:TextBox ID="txtName" runat="server" ReadOnly="true"></asp:TextBox><asp:Button
                ID="btnGetLOV" runat="server" Text="..." CausesValidation="true" OnClick="btnGetLOV_Click" />
                <asp:HiddenField
                    ID="hdnID" runat="server" Visible="false" />
                    <asp:HiddenField
                    ID="hdnShow" runat="server" />
        </td>
    </tr>
</table>
<table style="width: 100%">
    <tr>
        <td style="width: 100%">
            <div style="display: none; width: auto; min-width: 40%; max-width: 100%;">
                <asp:Panel ID="pnlLoadLOV" GroupingText="" runat="server" BackColor="White" BorderStyle="Solid"
                    Style="display: none; width: auto; min-width: 40%; max-width: 86%;">
                    <asp:UpdatePanel ID="upLOV" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <table align="center" style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdnQuery" runat="server" />
                                            <input type="hidden" id="hdnSearch" runat="server" />
                                            <input type="hidden" id="hdnOrderBy" runat="server" />
                                            <input type="hidden" id="hdnSortDirection" runat="server" />
                                            <input type="hidden" id="hdnSortExpression" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
                                            <%--OnRowDataBound="gvList_RowDataBound" --%>
                                            <div style="width: 100%">
                                                <asp:GridView ID="gvList" Width="100%" runat="server" AutoGenerateColumns="false"
                                                    OnRowCommand="gvList_RowCommand" HeaderStyle-CssClass="styleGridHeader" ShowHeader="true">
                                                    <Columns>
                                                        <%--    <asp:CommandField ShowSelectButton="True" Visible="false" />--%>
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="AliceBlue" />
                                                    <PagerStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:GridView>
                                                <uc1:PageNavigator ID="ucLOVPageNavigater" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <%--   <Triggers>
                            <asp:PostBackTrigger ControlID="gvList" />
                            <asp:PostBackTrigger ControlID="ucLOVPageNavigater" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                    <table align="center">
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="styleSubmitShortButton" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </td>
    </tr>
</table>
<table style="height: 10px; width: 10px">
    <tr>
        <td>
            <asp:Button runat="server" ID="btnNODdf" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />
            <cc1:ModalPopupExtender ID="ucMPE" runat="server" TargetControlID="btnNODdf" PopupControlID="pnlLoadLOV"
                BackgroundCssClass="styleModalBackground" CancelControlID="btnClose" />
        </td>
    </tr>
</table>
<%--</ContentTemplate>
</asp:UpdatePanel>--%>