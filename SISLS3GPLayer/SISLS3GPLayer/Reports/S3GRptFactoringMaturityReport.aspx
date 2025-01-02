<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GRptFactoringMaturityReport.aspx.cs" Inherits="Reports_S3GRptFactoringMaturityReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--  <asp:UpdatePanel ID="upnl1" runat="server">
        <ContentTemplate>--%>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Factoring Maturity Report">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                        <asp:Panel runat="server" ID="PnlFactoringMaturityreport" CssClass="stylePanel" GroupingText="INPUT CRITERIA"
                            Width="100%">
                            <table width="100%">
                                <tr>
                                    <td width="25%" class="styleFieldAlign">
                                        <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleMandatoryLabel"
                                            ToolTip="Line of Business"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                            AutoPostBack="True" ValidationGroup="Header" ToolTip="Line of Business">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:Label ID="lblLocation" runat="server" CssClass="styleMandatoryLabel" Text="Location"
                                            ToolTip="Location"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLocation" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                            ToolTip="Location" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblDate" runat="server" CssClass="styleMandatoryLabel" Text="Date"
                                            ToolTip="Date" />
                                    </td>
                                    <td class="styleFieldAlign">
                                <input id="hidDate" runat="server" type="hidden" />
                                <asp:TextBox ID="txtStartDateSearch" runat="server" AutoPostBack="true" Width="100" ToolTip="Date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" Display="None" ControlToValidate="txtStartDateSearch"
                                    ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select Date"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:Image ID="imgDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender ID="ceDate" runat="server" Enabled="True" PopupButtonID="imgDateSearch"
                                    TargetControlID="txtStartDateSearch">
                                </cc1:CalendarExtender>
                            </td>
                                    
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:Label ID="lblDenomination" runat="server" Text="Denomination" CssClass="styleMandatoryLabel"
                                            ToolTip="Denomination"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlDenomination" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                            ToolTip="Denomination" OnSelectedIndexChanged="ddlDenomination_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" OnClick="btnGo_Click"
                            ValidationGroup="btnGo" OnClientClick="return fnCheckPageValidators('btnGo',false);"
                            ToolTip="Go" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                            OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" ToolTip="Clear" />
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2" width="100%">
                        <asp:Label ID="lblAmounts" runat="server" Text="[All amounts are in" Visible="false"
                            CssClass="styleDisplayLabel"></asp:Label>
                        <asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign" colspan="4">
                        <div id="divMaturity" runat="server" style="overflow: scroll; height: 200px;" visible="false">
                            <asp:GridView ID="grvMaturity" runat="server" AutoGenerateColumns="False" BorderWidth="2"
                                CssClass="styleInfoLabel" Style="margin-bottom: 0px" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocationI" runat="server" Text='<%# Bind("Location") %>' ToolTip="Location"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Region">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegion" runat="server" Text='<%# Bind("Region") %>' ToolTip="Region"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerNameI" runat="server" Text='<%# Bind("CustomerName") %>'
                                                ToolTip="Customer Name"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account No" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountNoI" runat="server" Text='<%# Bind("PrimeAccount") %>' ToolTip="Account No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAccountNoI" runat="server" Style="text-align: right" Text='<%# Bind("SubAccount") %>'
                                                ToolTip="Sub Account No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FIL No" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFILNoI" runat="server" Style="text-align: right" Text='<%# Bind("FILNo") %>'
                                                ToolTip="FIL No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FIL Date" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFILDateI" runat="server" Style="text-align: right" Text='<%# Bind("DiscountDate") %>'
                                                ToolTip="FIL Date"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit Days" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreditDaysI" runat="server" Style="text-align: right" Text='<%# Bind("CreditDays") %>'
                                                ToolTip="Credit Days"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Maturity Date" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaturityDateI" runat="server" Style="text-align: right" Text='<%# Bind("MaturityDate") %>'
                                                ToolTip="Maturity Date"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prin O/s Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrinAmount" runat="server" Style="text-align: right" Text='<%# Bind("PrinAmount") %>'
                                                ToolTip="Prin O/s Amount"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dummy Column" Visible="false" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDummy1" runat="server" Style="text-align: right" Text='<%# Bind("VendorName") %>'
                                                ToolTip="Dummy"></asp:Label>
                                            <asp:Label ID="Label1" runat="server" Style="text-align: right" Text='<%# Bind("InvoiceNo") %>'
                                                ToolTip="Dummy"></asp:Label>
                                            <asp:Label ID="Label2" runat="server" Style="text-align: right" Text='<%# Bind("InvoiceAmount") %>'
                                                ToolTip="Dummy"></asp:Label>
                                            <asp:Label ID="Label3" runat="server" Style="text-align: right" Text='<%# Bind("MarginAmount") %>'
                                                ToolTip="Dummy"></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Style="text-align: right" Text='<%# Bind("INPUTDATE_1") %>'
                                                ToolTip="Dummy"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                
                <tr>
                    <td align="center" colspan="4">
                        <asp:Button ID="btnPrint" CssClass="styleSubmitButton" runat="server" Text="Print"
                            OnClick="btnPrint_Click" ToolTip="Print" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ID="VsMaturity" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnGo" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVMaturity" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnPrint" runat="server" />
       <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
