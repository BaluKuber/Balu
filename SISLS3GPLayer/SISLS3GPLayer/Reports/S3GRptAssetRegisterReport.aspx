<%@ Page Title="Asset Register Report" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptAssetRegisterReport.aspx.cs" Inherits="Reports_S3GRptAssetRegisterReport" %>

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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Asset Register Report">
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
                                        <asp:Label ID="lblRSNo" runat="server" Text="Rental Schedule No." ToolTip="Rental Schedule No." CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlRSNo" runat="server" ServiceMethod="GetRSNoDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblTrancheNo" runat="server" Text="Tranche No." ToolTip="Tranche No." CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlTrancheNo" runat="server" ServiceMethod="GetTrancheDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblAssetCategory" runat="server" Text="Asset Category" ToolTip="AssetCategory" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlAssetCategory" runat="server" ServiceMethod="GetAssetCategoryDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblAssetStatus" runat="server" Text="Asset Status" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlAssetStatus" runat="server" Width="55%" ToolTip="Asset Status">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblFunderName" runat="server" Text="Funder Name" ToolTip="Funder Name" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunders" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblCustomersCustomerName" runat="server" Text="Customer's Customer Name" ToolTip="Customer's Customer Name" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlCustomersCustomerName" runat="server" ServiceMethod="GetCustomersCustomerName" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Rental Sch Booking Date Range From" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtStartDate" runat="server" Width="53%" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter the Rental Sch Booking Date Range From" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblEndDate" runat="server" Text="Rental Sch Booking Date Range To" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtEndDate" runat="server" Width="53%" AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                        <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" PopupButtonID="ImgEndDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Enter the Rental Sch Booking Date Range To" ValidationGroup="Go" Display="None" SetFocusOnError="True"
                                            CssClass="styleMandatoryLabel" ControlToValidate="txtEndDate">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblVendor" runat="server" Text="Vendor Name" ToolTip="Vendor Name" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlVendor" runat="server" ServiceMethod="GetVendor" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblVendorInvoiceStatus" runat="server" Text="Vendor Invoice Status" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlVendorInvoiceStatus" runat="server" AutoPostBack="true" Width="55%" ToolTip="Vendor Invoice Status">
                                            <asp:ListItem Value="0" Text="--All--" />
                                            <asp:ListItem Value="2" Text="Received" />
                                            <asp:ListItem Value="1" Text="Not received" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLocation" runat="server" Text="State" CssClass="styleDisplayLabel" ToolTip="State">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList" Width="205px"
                                            WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblProposalType" runat="server" Text="Proposal Type" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlProposalType" runat="server" Width="55%" ToolTip="Proposal Type">
                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblHSNCode" runat="server" Text="HSN Code" CssClass="styleDisplayLabel" ToolTip="HSN Code">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlHSNCode" runat="server" ServiceMethod="GetHSNCodeList" Width="205px"
                                            WatermarkText="--All--" />
                                    </td>
                                     <td class="styleFieldLabel">
                                        <asp:Label ID="lblSACCode" runat="server" Text="SAC Code" CssClass="styleDisplayLabel" ToolTip="SAC Code">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlSACCode" runat="server" ServiceMethod="GetSACCodeList" Width="205px"
                                            WatermarkText="--All--" />
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
                        <asp:Panel ID="pnlAssetRegisterReportDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Register Report Details" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1050px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:GridView ID="grvAssetRegisterReportDetails" runat="server" Width="100%" HeaderStyle-CssClass="styleGridHeader" RowStyle-Wrap="false"
                                                RowStyle-Height="25px" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="true" OnRowDataBound="grvAssetRegisterReportDetails_RowDataBound"
                                                OnRowCommand="grvAssetRegisterReportDetails_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false">
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblQuery" runat="server" Text="Download"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/pdf.png" CssClass="styleGridQuery"
                                                                CommandArgument='<%# Bind("Doc_Path") %>' CommandName="Print" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                        </td>
                                    </tr>
                                </table>
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
                        <asp:ValidationSummary ID="vsAssetRegisterReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):"
                            Height="250px" Width="500px" ShowMessageBox="false" ValidationGroup="Go"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVAssetRegisterReport" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>

                </tr>
                <tr>
                    <td id="tdHeight" runat="server"></td>
                </tr>
                <%--    <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="center">
                        <span runat="server" id="lblErrorMessage1" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr class="styleButtonArea" style="padding-left: 4px">
                    <td colspan="4" align="center">
                        <asp:Button ID="btnExport" runat="server" CssClass="styleSubmitButton" Text="Export To Excel" OnClick="btnExport_Click" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsAssetRegisterReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):"
                            Height="250px" Width="500px" ShowMessageBox="false" ValidationGroup="Go"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td id="tdHeight" runat="server"></td>
                </tr>--%>
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


