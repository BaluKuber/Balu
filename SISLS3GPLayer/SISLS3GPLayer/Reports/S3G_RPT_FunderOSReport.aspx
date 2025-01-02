<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_RPT_FunderOSReport.aspx.cs" Inherits="Reports_S3G_RPT_FunderOSReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Funder O/S Report">
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
                        <asp:Panel ID="pnlHeaderDetails" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                            <table cellpadding="0" cellspacing="0" style="width: 100%" align="center">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Funder Name" ID="lblFunderName" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunderList" ToolTip="Funder Name"
                                            IsMandatory="false" Width="300px" WatermarkText="--All--" />
                                        <%--ErrorMessage="Select the Funder Name" ValidationGroup="vgGo"--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Note Number" ID="lblNoteNumber" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="ddlNoteNumber" runat="server" ServiceMethod="GetNoteList" ToolTip="Note Number"
                                            IsMandatory="false" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Lessee Name" ID="lblLesseeName" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeList" ToolTip="Lessee Name"
                                            IsMandatory="false" Width="300px" WatermarkText="--All--" />
                                        <%--ErrorMessage="Select the Lessee Name" ValidationGroup="vgGo" --%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Date" ID="lblDate"  CssClass="styleReqFieldLabel" ></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDate" runat="server" ToolTip="As on Date"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtDate" PopupButtonID="imgStartDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter the Date" ValidationGroup="vgGo" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="true" ControlToValidate="txtDate"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr class="styleButtonArea" style="padding-left: 4px">
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" ToolTip="Go" OnClick="btnGo_Click" ValidationGroup="vgGo" />
                                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear"
                                            OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlRptDetails" runat="server" CssClass="stylePanel" GroupingText="Funder O/S Details" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1050px">
                                <asp:GridView ID="grvFunderOsDtl" runat="server" Width="100%"
                                    RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0" OnRowDataBound="grvFunderOsDtl_RowDataBound">
                                </asp:GridView>
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server" Visible="true"></uc1:PageNavigator>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <span runat="server" id="lblPagingErrorMessage" style="color: Red; font-size: medium"></span>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td colspan="4" align="center">
                        <asp:Button ID="btnExport" runat="server" CssClass="styleSubmitButton" Text="Export" ToolTip="Export"
                            Visible="false" OnClick="btnExport_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vgGo" ValidationGroup="vgGo"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:CustomValidator ID="CVFunderRpt" runat="server" Display="None"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

