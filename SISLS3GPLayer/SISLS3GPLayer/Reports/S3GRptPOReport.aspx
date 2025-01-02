<%@ Page Title="PO Report" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptPOReport.aspx.cs" Inherits="Reports_S3GRptPOReport" %>

<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="PO Report">
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
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel" ToolTip="Line of Business">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" Width="55%" ToolTip="Line of Business">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" ToolTip="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name" ToolTip="Vendor Name" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlVendorName" runat="server" ServiceMethod="GetVendorNameDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblAssetCategory" runat="server" Text="Asset Category" ToolTip="Asset Category" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlAssetCategory" runat="server" ServiceMethod="GetAssetCategoryDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblPODateFrom" runat="server" Text="PO Date From" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtPODateFrom" runat="server" Width="53%"  AutoPostBack="true" OnTextChanged="txtPODateFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgPODateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPODateFrom" PopupButtonID="imgPODateFrom" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvPODateFrom" runat="server" ErrorMessage="Enter the PO Date From." ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtPODateFrom"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblPODateTo" runat="server" Text="PO Date To" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtPODateTo" runat="server" Width="53%" AutoPostBack="true" OnTextChanged="txtPODateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="ImgPODateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtPODateTo" PopupButtonID="ImgPODateTo" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvPODateTo" runat="server" ErrorMessage="Enter the PO Date To." ValidationGroup="Go" Display="None" SetFocusOnError="True"
                                            CssClass="styleMandatoryLabel" ControlToValidate="txtPODateTo">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="PO Status" ID="lblPOStatus" CssClass="styleDisplayLabel" ToolTip="PO Status">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlPOStatus" runat="server" AutoPostBack="true" Width="55%" ToolTip="PO Status">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Scheduled" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Un Scheduled" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Cancelled" Value="3"></asp:ListItem>
                                            </asp:DropDownList>
                                    </td>
                                   
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblCreatedBy" runat="server" Text="Created By" ToolTip="Created By" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlCreatedBy" runat="server" ServiceMethod="GetUserNameDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea" style="padding-left: 4px">
                    <td colspan="4" align="center">
                        <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" ValidationGroup="Go" OnClick="btnGo_Click" />
                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlPOReportDetails" runat="server" CssClass="stylePanel" GroupingText="PO Report Details" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1150px">
                                <asp:GridView ID="grvPOReportDetails" runat="server" Width="100%" FooterStyle-HorizontalAlign="Right"
                                    RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0">                                    
                                </asp:GridView>
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server" Visible="true"></uc1:PageNavigator>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea" style="padding-left: 4px">
                    <td colspan="4" align="center">
                        <asp:Button ID="btnExport" runat="server" CssClass="styleSubmitButton" Text="Export To Excel" OnClick="btnExport_Click" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="center">
                        <span runat="server" id="lblErrorMessage1" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr class="styleButtonArea" style="padding-left: 4px">
                    <td colspan="4" align="center">
                        <asp:Button ID="btnPrint" runat="server" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsPOReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):"
                            Height="250px" Width="500px" ShowMessageBox="false" ValidationGroup="Go"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVPOReport" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>

                </tr>
                <tr>
                    <td id="tdHeight" runat="server"></td>
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
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - 260;
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

        function Resize() {
            document.getElementById('<%=tdHeight.ClientID %>').style.height = 100;
        }
    </script>
</asp:Content>
