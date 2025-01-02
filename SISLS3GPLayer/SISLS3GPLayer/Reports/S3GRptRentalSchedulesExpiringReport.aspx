<%@  Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptRentalSchedulesExpiringReport.aspx.cs" Inherits="Reports_S3GRptRentalSchedulesExpiringReport" Title="Rental Schedules Expired Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Rental Schedules Expiring Report">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                            <table align="center" width="100%" border="0" cellspacing="0">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Expiry Date Range From" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter Expiry Date Range From" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Expiry Date Range To" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" PopupButtonID="ImgEndDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Enter Expiry Date Range To" ValidationGroup="Go" Display="None" SetFocusOnError="True"
                                            CssClass="styleMandatoryLabel" ControlToValidate="txtEndDate">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" ToolTip="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails" WatermarkText="--All--" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" CausesValidation="true" ValidationGroup="Go" OnClick="btnOk_Click" ToolTip="Go" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset class="stylePanel">
                            <legend>Rental Schedules Expiring Report</legend>
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1170px">

                                <table width="99%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grvRSExpired" EmptyDataRowStyle-HorizontalAlign="Left" runat="server" Width="4052px" FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" OnRowDataBound="grvRSExpired_RowDataBound">
                                            </asp:GridView>
                                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </fieldset>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnExport" CssClass="styleSubmitButton" Text="Export" CausesValidation="false" ValidationGroup="Export" OnClick="btnExport_Click" Visible="false" ToolTip="Export" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsRepayment" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVRepaymentSchedule" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
        }
        function pageLoad(s, a) {
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
        }
        function showMenu(show) {
            if (show == 'T') {
                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

    </script>
</asp:Content>
