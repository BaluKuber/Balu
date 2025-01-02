<%@ Control Language="C#" AutoEventWireup="true" CodeFile="S3GDynamicLOV.ascx.cs"
    Inherits="UserControls_S3GDynamicLOV" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/LOVPageNavigator.ascx" %>

<table>
    <tr>
        <td>
            <asp:Button ID="btnGetLOV" Style="display: none;" runat="server" Text="..." CausesValidation="true"
                OnClick="btnGetLOV_Click" />
            <asp:HiddenField ID="hdnID" runat="server" Visible="false" />
            <asp:HiddenField ID="hdnText" runat="server" Visible="false" />
        </td>
    </tr>
</table>
<table style="width: 100%">
    <tr>
        <td style="width: 100%">
            <cc1:DropShadowExtender ID="dseCustomer" runat="server" TargetControlID="pnlLoadLOV"
                Opacity=".4" Rounded="false" TrackPosition="true" />
            <asp:Panel ID="pnlLoadLOV" GroupingText="" runat="server" BackColor="White" Style="position: absolute;
                width: auto; min-width: 40%; max-width: 70%;" Visible="false">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="stylePageHeading" width="100%">
                            <asp:Label runat="server" ID="lblHeader" CssClass="styleDisplayLabel" Text="Filter Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upLOV" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table align="center" style="width: 100%" cellpadding="0" cellspacing="0">
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
                                                <asp:GridView ID="gvList" Width="100%" runat="server" AutoGenerateColumns="false"
                                                    OnRowCommand="gvList_RowCommand" HeaderStyle-CssClass="styleGridHeader" ShowHeader="true">
                                                    <Columns>
                                                        <%--    <asp:CommandField ShowSelectButton="True" Visible="false" />--%>
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="AliceBlue" />
                                                    <PagerStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:GridView>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%" class="styleMainTable">
                                                <uc1:PageNavigator ID="ucLOVPageNavigater" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <table align="center">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="styleSubmitShortButton"
                                            OnClick="btnClose_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
