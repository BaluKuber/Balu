<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_LAD_BillingException.aspx.cs" Inherits="LoanAdmin_S3G_LAD_BillingException" %>

<%@ Register TagPrefix="uc3" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel GroupingText="Input Details" ID="Panel2" runat="server" CssClass="stylePanel"
                                        ToolTip="Input Details">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                            <tr>
                                                <td class="styleFieldLabel" width="25%">
                                                    <asp:Label runat="server" ID="lblFrom_Date" CssClass="styleReqFieldLabel" Text="Month"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="25%">
                                                     <asp:TextBox ID="txtMonthYear" runat="server"   ></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calMonthYear" Format="MMM-yyyy" TodaysDateFormat="MMM-yyyy"
                                                                   
                                                                    runat="server" DefaultView="Months" Enabled="True" TargetControlID="txtMonthYear"
                                                                    PopupButtonID="imgMonthYear">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgMonthYear" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <asp:RequiredFieldValidator ID="RFVFromDate" ValidationGroup="Print" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtMonthYear" SetFocusOnError="True"
                                                        ErrorMessage="Select Month" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                   <td class="styleFieldLabel" width="25%">
                                                                            <asp:Label runat="server" ID="lblBranch" Text="State"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <uc2:Suggest ID="ddlBranchList" runat="server" ServiceMethod="GetBranchList"  AutoPostBack="true"
                                                                                ErrorMessage="Select a  State"   />
                                                                        </td>
                                              
                                            </tr>
                                             
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:Button runat="server" ID="btnSearch" CssClass="styleSubmitButton" Text="Fetch"
                                                        ValidationGroup="Print" OnClick="btnFetch_Click" />
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
           <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel GroupingText="Exception Details" ID="pnlexp" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gexp" runat="server" AutoGenerateColumns="False"  Width="100%">
                                            <Columns>
                                            <asp:TemplateField HeaderText="Tranche Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrancheName" runat="server" Text='<%# Eval("tranche_name") %>'
                                                            ToolTip="PO Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RS Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrs_number" runat="server" Text='<%# Eval("rs_number") %>' ToolTip="PO Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Invoice Type" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInvoiceType" runat="server" Text='<%# Eval("Invoice_Type") %>'
                                                            ToolTip="PO Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                             
                                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Customer_Name") %>'
                                                            ToolTip="Customer Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("Location") %>'
                                                            ToolTip="Location" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Due Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblduedate" runat="server" Text='<%# Eval("installment_date") %>'
                                                            ToolTip="Due Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                     <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Rs Activation Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblactivationdate" runat="server" Text='<%# Eval("Account_activated_date") %>'
                                                            ToolTip="Rs Activation Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                     <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Rental" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrental" runat="server" Text='<%# Eval("Rental") %>'
                                                            ToolTip="Rental" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Tax" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVAT" runat="server" Text='<%# Eval("VAT") %>'
                                                            ToolTip="VAT" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="AMF" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAMF" runat="server" Text='<%# Eval("AMF") %>'
                                                            ToolTip="AMF" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Service Tax" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblservicetax" runat="server" Text='<%# Eval("service_tax") %>'
                                                            ToolTip="Rental" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>

                                              
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        
                        <asp:Button runat="server" ID="btnExport1" CssClass="styleSubmitButton" Text="Export Excel"
                           ValidationGroup="Print" Enabled="false" OnClick="btnExport_Click" />
                     

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsDelivery" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  "  ValidationGroup="Print" />
                    </td>
                </tr>

            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport1" />
            <%-- <asp:AsyncPostBackTrigger ControlID="ddlBranchList" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


