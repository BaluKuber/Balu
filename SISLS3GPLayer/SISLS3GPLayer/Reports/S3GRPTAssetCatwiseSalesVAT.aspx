<%@ Page Title="AssetCategorywise Sales VAT Report" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRPTAssetCatwiseSalesVAT.aspx.cs" Inherits="Reports_S3GRPTAssetCatwiseSalesVAT" %>


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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Asset Type wise VAT Rate Report">
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
                                        <asp:Label ID="lblState" runat="server" Text="State" ToolTip="State" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlState" runat="server" ServiceMethod="GetStateDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblAssetCategory" runat="server" Text="Asset Category" ToolTip="Asset Category" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlAssetCategory" runat="server" ServiceMethod="GetAssetCategoryDetails" AutoPostBack="true" Width="205px" WatermarkText="--All--"/>
                                    </td>
                                   
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                         <asp:Label ID="lblAssetType" runat="server" Text="Asset Type">    </asp:Label>
                                     
                                       <%-- <asp:Label ID="lblStartDate" runat="server" Text="Start Date" >-- CssClass="styleReqFieldLabel"
                                        </asp:Label>--%>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlAssettype" runat="server" ServiceMethod="GetAssettypeList" AutoPostBack="true" Width="205px" WatermarkText="--All--"/>
                                       <%-- <asp:TextBox ID="txtStartDate" runat="server" Width="53%" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>--%>
                                       <%-- <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter the Start Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <asp:Label ID="lblEndDate" runat="server" CssClass="styleReqFieldLabel" Text="UpTo Date">   <%--CssClass="styleReqFieldLabel"--%>
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtEndDate" runat="server" Width="53%" AutoPostBack="false" ></asp:TextBox>
                                        <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtEndDate" PopupButtonID="ImgEndDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                         <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter the Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
                                        <%--OnTextChanged="txtEndDate_TextChanged"<asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Enter the End Date" ValidationGroup="Go" Display="None" SetFocusOnError="True"
                                            CssClass="styleMandatoryLabel" ControlToValidate="txtEndDate">
                                        </asp:RequiredFieldValidator>--%>
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
                        <asp:Panel ID="pnlVATReportDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Type wise VAT Rate Details" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1150px">
                                <asp:GridView ID="grvVATReportDetails" runat="server" Width="100%" AutoGenerateColumns="true" FooterStyle-HorizontalAlign="Right"
                                    RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0" ShowFooter="false" OnRowDataBound="grvVATReportDetails_RowDataBound">
                                 <Columns>
                                       <asp:TemplateField HeaderText="AssetCategory">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetCategory" runat="server" Width="120px" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Asset Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetType" runat="server" Width="120px" Text='<%# Bind("AssetType") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
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
               
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsWCTReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):"
                            Height="250px" Width="500px" ShowMessageBox="false" ValidationGroup="Go"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVWCTReport" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
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

