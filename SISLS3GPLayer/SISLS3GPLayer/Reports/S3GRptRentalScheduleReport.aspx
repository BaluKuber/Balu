<%@  Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptRentalScheduleReport.aspx.cs" Inherits="Reports_S3GRptRentalScheduleReport" Title="Rental Schedule Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - 260;
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Rental Schedule Report">
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
                                        <asp:Label ID="lblStartDate" runat="server" Text="RS From Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter From Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate">
											</asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblEndDate" runat="server" Text="RS To Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" PopupButtonID="ImgEndDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Enter To Date" ValidationGroup="Go" Display="None" SetFocusOnError="True"
                                            CssClass="styleMandatoryLabel" ControlToValidate="txtEndDate">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label2" runat="server" Text="RS Activation From Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:TextBox ID="txtRSActivationFrom" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgRSActivationFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtRSActivationFrom" PopupButtonID="imgRSActivationFrom" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label3" runat="server" Text="RS Activation To Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    
                                    
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRSActivationTo" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgRSActivationTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtRSActivationTo" PopupButtonID="imgRSActivationTo" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label ID="lblRSCreatedOnStart" runat="server" Text="RS Created On Strat Date" CssClass="styleDisplayLabel">
                                            </asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtRSCreatedOnStart" runat="server"></asp:TextBox>
                                                <asp:Image ID="imgRSCreatedOnStart" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <cc1:CalendarExtender ID="ceRSCreatedOnStart" runat="server" TargetControlID="txtRSCreatedOnStart" PopupButtonID="imgRSCreatedOnStart" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                       
                                        </td>
                                 
                                        <td class="styleFieldLabel">
                                            <asp:Label ID="lblRSCreatedOnEnd" runat="server" Text="RS Created On Date End Date" CssClass="styleDisplayLabel">
                                            </asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtRSCreatedOnEnd" runat="server"></asp:TextBox>
                                            <asp:Image ID="imgRSCreatedOnEnd" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <cc1:CalendarExtender ID="ceRSCreatedOnEnd" runat="server" TargetControlID="txtRSCreatedOnEnd" PopupButtonID="imgRSCreatedOnEnd" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                       
                                        </td>
                                            </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" ToolTip="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server"  Width="280px"  ServiceMethod="GetLesseeNameDetails" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblFunderName" runat="server" Text="Funder Name" CssClass="styleDisplayLabel" ToolTip="Funder Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunderNameDetails" WatermarkText="--All--" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblFundingStatus" runat="server" Text="Funding Status" CssClass="styleDisplayLabel" ToolTip="Funding Status">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlFundingStatus" runat="server">
                                             <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Funded"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Unfunded"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblBookingStatus" runat="server" Text="Booking Status" CssClass="styleDisplayLabel" ToolTip="Booking Status">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlBookingStatus" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Live"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Configured"></asp:ListItem>
											<asp:ListItem Value="3" Text="Closed"></asp:ListItem>
											<asp:ListItem Value="4" Text="Cancelled"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLocation" runat="server" Text="State" CssClass="styleDisplayLabel" ToolTip="State">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList"
                                            WatermarkText="--All--" />
                                    </td>
									<td class="styleFieldLabel">
                                        <asp:Label ID="Label1" runat="server" Text="Tranche Name" CssClass="styleDisplayLabel" ToolTip="Tranche Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlTrancheName" runat="server" ServiceMethod="GetTrancheNameDetails" WatermarkText="--All--" />
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
                    <td width="100%">
                        <asp:Panel ID="pnlPDC" runat="server" Style="display:none" CssClass="stylePanel" GroupingText="Rental Schedule Report">
                            <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1150px">
                                <table width="99%">
                                    <tr>
                                        <td>
                                            <asp:GridView ID="grvPDC" runat="server" Width="100%" FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                            </asp:GridView>
                                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                        </td>
                                    </tr>
                                </table>
                                </div>
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
