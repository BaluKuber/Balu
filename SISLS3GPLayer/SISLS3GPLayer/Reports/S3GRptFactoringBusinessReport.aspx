<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" 
AutoEventWireup="true" CodeFile="S3GRptFactoringBusinessReport.aspx.cs" Inherits="Reports_S3GRptFactoringBusinessReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="upnl1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Factoring Business Report">
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
                                    <td class="styleFieldAlign" style="height: 27px">
                                        <asp:Label ID="lblStartDateSearch" runat="server" CssClass="styleDisplayLabel" Text="Start Date" ToolTip="Start Date" />
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" style="height: 27px">
                                        <input id="hidDate" runat="server" type="hidden" />
                                        <asp:TextBox ID="txtStartDateSearch" runat="server" AutoPostBack="true" Width="100" ToolTip="Start Date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="None" ControlToValidate="txtStartDateSearch"
                                            ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select Start Date"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch" >
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldAlign" style="height: 27px">
                                        <asp:Label ID="lblEndDateSearch" runat="server" CssClass="styleDisplayLabel" Text="End Date" ToolTip="End Date" />
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" style="height: 27px">
                                        <asp:TextBox ID="txtEndDateSearch" runat="server" AutoPostBack="true" Width="100" ToolTip="End Date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" Display="None" ControlToValidate="txtEndDateSearch"
                                            ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select End Date"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:Label ID="lblDenomination" runat="server" CssClass="styleDisplayLabel" Text="Denomination" ToolTip="Denomination" />
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">                                        
                                        <asp:DropDownList ID="ddlDenomination" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                            ToolTip="Denomination">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="2">
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
                            ToolTip="Go" style="height: 26px" />&nbsp;&nbsp;&nbsp;
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
                    <asp:Panel ID="pnlDemand" runat="server" GroupingText="Factoring Business Report" CssClass="stylePanel" Width="100%" Visible="false">
                    <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                        <div id="divMaturity" runat="server" style="overflow: scroll; height: 200px;" visible="false">
                            <asp:GridView ID="grvBusiness" runat="server" AutoGenerateColumns="False" BorderWidth="2"
                                CssClass="styleInfoLabel" Style="margin-bottom: 0px" Width="100%">
                                <Columns>
                                    <%--<asp:TemplateField HeaderText="FIL No" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFILNoI" runat="server" Text='<%# Bind("FILNo") %>' ToolTip="FIL No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>--%>
                                     <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>' ToolTip="Location"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="FIL Date" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFILDateI" runat="server"  Style="text-align: right"  Text='<%# Bind("FILDate") %>' ToolTip="FIL Date"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomerNameI" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="Customer Name"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prime Account" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrimeAccountI" runat="server" Style="text-align: right" Text='<%# Bind("PrimeAccount") %>'
                                                ToolTip="Prime Account"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Account" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAccountI" runat="server" Style="text-align: right" Text='<%# Bind("SubAccount") %>'
                                                ToolTip="Sub Account"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit Amount" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreditAmountI" runat="server" Style="text-align: right" Text='<%# Bind("Credit_Amount") %>'
                                                ToolTip="Credit Amount"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit Available" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreditAvailableI" runat="server" Style="text-align: right" Text='<%# Bind("CreditAvailable") %>'
                                                ToolTip="Credit Available"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceAmountI" runat="server" Style="text-align: right" Text='<%# Bind("InvoiceAmount") %>'
                                                ToolTip="Invoice Amount"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Discount Amount" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDiscountAmountI" runat="server" Style="text-align: right" Text='<%# Bind("DISCOUNT_AMOUNT") %>'
                                                ToolTip="Discount Amount"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interest Rate" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInterestRateI" runat="server" Style="text-align: right" Text='<%# Bind("INTEREST_RATE") %>'
                                                ToolTip="Interest Rate"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vendor Name" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorNameI" runat="server" Style="text-align: right" Text='<%# Bind("VendorName") %>'
                                                ToolTip="Vendor Name"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceNumberI" runat="server" Style="text-align: right" Text='<%# Bind("InvoiceNo") %>'
                                                ToolTip="Invoice Number"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Maturity Date" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMaturityDateI" runat="server" Style="text-align: right" Text='<%# Bind("MaturityDate") %>'
                                                ToolTip="Maturity Date"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        </asp:Panel>
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
                        <asp:ValidationSummary ID="VsBusinessMaturity" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Please correct the following validation(s):" ShowSummary="true" ValidationGroup="btnGo" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVBusinessMaturity" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

