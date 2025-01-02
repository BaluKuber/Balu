<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_RPT_ProjectedFunderDueReport.aspx.cs" Inherits="Reports_S3G_RPT_ProjectedFunderDueReport" %>

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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Projected Funder Due Report">
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
                                        <asp:Label runat="server" Text="From Date" ID="lblFromDate" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtFromDate" runat="server" ToolTip="From Date"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="ceDate" runat="server" TargetControlID="txtFromDate" PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter the From Date" ValidationGroup="vgGo" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="true" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="To Date" ID="lblToDate" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtToDate" runat="server" ToolTip="To Date"></asp:TextBox>
                                        <asp:Image ID="imgToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate" PopupButtonID="imgToDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Funder Name" ID="lblFunderName" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunderList" ToolTip="Funder Name"
                                            IsMandatory="false" Width="250px" WatermarkText="--All--" />
                                        <%--ErrorMessage="Select the Funder Name" ValidationGroup="vgGo"--%>
                                    </td>
                                   
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Tranche Number" ID="lblTranchNumber" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="ddlTranchNumber" runat="server" ServiceMethod="GetTrancheList" ToolTip="Tranche Number"
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
                                            IsMandatory="false" Width="250px" WatermarkText="--All--" />
                                        <%--ErrorMessage="Select the Lessee Name" ValidationGroup="vgGo" --%>
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
                        <asp:Panel ID="pnlRptDetails" runat="server" CssClass="stylePanel" GroupingText="Projected Funder Due Report" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 99%">
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

