<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3g_RPT__BTNReport.aspx.cs" Inherits="Reports_S3g_RPT__BTNReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="BTN Report">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                            <table align="center" width="100%" border="0" cellspacing="0">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStartDate" runat="server" Text="BTN / DC start Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblenddate" runat="server" Text="BTN / DC End Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtenddate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgtxtenddate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtenddate" PopupButtonID="imgtxtenddate">
                                        </cc1:CalendarExtender>
                                    </td>
                                   
                                </tr>
                                <tr>
                                     <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" CssClass="styleDisplayLabel" ToolTip="Customer Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlCustomerName" runat="server" ServiceMethod="GetCustomerName"
                                            WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDocumentNumber" runat="server" Text="Document Number" CssClass="styleDisplayLabel" ToolTip="Document Number">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlDocumentNumber" runat="server" ServiceMethod="GetDocumentNo"
                                            WatermarkText="--All--" />
                                    </td>
                                   
                                </tr>
                                <tr>
                                     <td class="styleFieldLabel">
                                        <asp:Label ID="lblBTNNo" runat="server" Text="BTN / DC Number" CssClass="styleDisplayLabel" ToolTip="BTN / DC Number">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlBTNNo" runat="server" ServiceMethod="GetBTNNO" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblParentRSNumber" runat="server" Text="Parent RS Number" CssClass="styleDisplayLabel" ToolTip="Parent RS Number">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlTParentRSNumber" runat="server" ServiceMethod="GetRSNODetails" WatermarkText="--All--" />
                                    </td>
                                  
                                </tr>
                                <tr>
                                      <td class="styleFieldLabel">
                                        <asp:Label ID="lblNewRSNumber" runat="server" Text="New RS Number" CssClass="styleDisplayLabel" ToolTip="New RS Number">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlNewRSNumber" runat="server" ServiceMethod="GetNewRSNO"
                                            WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblshipTo" runat="server" Text="Delivery (Ship To) State" CssClass="styleDisplayLabel" ToolTip="Delivery (Ship To) State">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ucshipto" runat="server" ServiceMethod="GetShiptoAddress" WatermarkText="--All--" />
                                    </td>
                                  
                                </tr>
                                <tr>
                                      <td class="styleFieldLabel">
                                        <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleDisplayLabel" ToolTip="Status">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" Width="55%" ToolTip="Status">
                                            <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="RS configured" Value="47"></asp:ListItem>
                                            <asp:ListItem Text="RS Activated" Value="46"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" CausesValidation="true" ValidationGroup="Go" ToolTip="Go" OnClick="btnOk_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlVAT" runat="server" CssClass="stylePanel" GroupingText="BTN Report" Visible="false" Width="100%">
                            <%--<div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1085px">--%>
                            <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                            <asp:GridView ID="grvGST" runat="server" OnRowDataBound="grvGST_RowDataBound" Width="100%" FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center">
                            </asp:GridView>
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                            <%--</div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnExport" CssClass="styleSubmitButton" Text="Export" CausesValidation="false" ValidationGroup="Export" OnClick="btnExport_Click" Visible="false" ToolTip="Export" />
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:ValidationSummary ID="vsRepayment" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:CustomValidator ID="CVRepaymentSchedule" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

