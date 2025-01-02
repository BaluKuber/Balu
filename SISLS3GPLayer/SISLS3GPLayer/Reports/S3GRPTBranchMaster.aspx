<%@ Page Title="Branch Master Report" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRPTBranchMaster.aspx.cs" Inherits="Reports_S3GRPTBranchMaster" %>
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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Branch Master Report">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlHeaderDetails" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="50%">
                            <table cellpadding="0" cellspacing="0" style="width: 50%" align="center">
                                <tr>
                                     <td class="styleFieldLabel" width="20%">
                                        <asp:Label ID="lblState" runat="server" Text="State" ToolTip="State" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <uc2:Suggest ID="ddlState" runat="server" ServiceMethod="GetStateDetails" AutoPostBack="true" Width="205px" WatermarkText="--ALL--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><br /></td>
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
                        <asp:Panel ID="pnlBranchtDetails" runat="server" CssClass="stylePanel" GroupingText="Branch Master Details" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 100%">
                                <asp:GridView ID="grvBranchtDetails" runat="server" Width="100%" AutoGenerateColumns="true" FooterStyle-HorizontalAlign="Right"
                                    RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0" ShowFooter="false">
                                 <%--<Columns>
                                       <asp:TemplateField HeaderText="State">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmonth" runat="server"  Text='<%# Bind("State") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="30%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Billing Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("BillingAddress") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="30%"/>
                                        </asp:TemplateField>
                                     <asp:TemplateField HeaderText="VAT TIN">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVATTIN" runat="server" Text='<%# Bind("VATTIN") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:TemplateField>
                                     <asp:TemplateField HeaderText="CST TIN">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCSTTIN" runat="server" Text='<%# Bind("CSTTIN") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:TemplateField>
                                      <asp:TemplateField HeaderText="WCT Regn No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWCTRegnNo" runat="server" Text='<%# Bind("WCTRegnNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="10%"/>
                                        </asp:TemplateField>
                                    
                                          
                                    </Columns>--%>
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

