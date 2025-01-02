<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Rpt_WDVReport.aspx.cs" Inherits="Reports_S3G_Rpt_WDVReport" %>

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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="WDV Report">
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
                                        <asp:Label ID="lblStartDate" runat="server" Text="As on Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="None" ControlToValidate="txtStartDate"
                                            ValidationGroup="Go" CssClass="styleMandatoryLabel" ErrorMessage="Select As on Date"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" ToolTip="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails"
                                            WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblType" runat="server" Text="Type" CssClass="styleDisplayLabel" ToolTip="Type">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlType" runat="server">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">RS Wise</asp:ListItem>
                                            <asp:ListItem Value="2">Tranche Wise</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblFunderName" runat="server" Text="Funder Name" CssClass="styleDisplayLabel" ToolTip="Funder Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetVendors"
                                            WatermarkText="--All--" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" ValidationGroup="Go" ToolTip="Go" OnClick="btnOk_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlVAT" runat="server" CssClass="stylePanel" GroupingText="WDV Report" Visible="false" Width="100%">
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
                        <asp:Button runat="server" ID="btnExport" CssClass="styleSubmitButton" Text="Export" ValidationGroup="Go" OnClick="btnExport_Click" Visible="false" ToolTip="Export" />
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

    <%--<script language="javascript" type="text/javascript">

        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 70;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 275;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
        }

    </script>--%>
</asp:Content>

