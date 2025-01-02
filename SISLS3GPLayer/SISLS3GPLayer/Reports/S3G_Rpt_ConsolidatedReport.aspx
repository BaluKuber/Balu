<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Rpt_ConsolidatedReport.aspx.cs" Inherits="Reports_S3G_Rpt_ConsolidatedReport" %>

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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Consolidated Sales Invoice Register">
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
                                        <asp:Label ID="lblStartDate" runat="server" Text="Invoice Date From">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter Start Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" Enabled="false" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblEnddate" runat="server" Text="Invoice Date To">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtenddate" runat="server"></asp:TextBox>
                                        <asp:Image ID="Imgenddate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtenddate" PopupButtonID="Imgenddate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter End Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" Enabled="false" ControlToValidate="txtenddate"></asp:RequiredFieldValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" ToolTip="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails"
                                            WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLocation" runat="server" Text="State" CssClass="styleDisplayLabel" ToolTip="State">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList"
                                            WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblTrancheNo" runat="server" Text="Tranche Name" CssClass="styleDisplayLabel" ToolTip="Tranche Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlTrancheNo" runat="server" ServiceMethod="GetTrancheNo" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRSNo" runat="server" Text="SCH No." CssClass="styleDisplayLabel" ToolTip="SCH No.">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlRSNo" runat="server" ServiceMethod="GetRSNo" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceType" runat="server" Text="Invoice Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlinvoice" runat="server" Width="175px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlInvoiceNo" runat="server" ServiceMethod="GetInvoiceNo" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblOldInvoiceNo" runat="server" Text="Old Invoice No"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlOldInvoiceNo" runat="server" ServiceMethod="GetOldInvoiceNo" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLeaseType" runat="server" Text="Lease Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLeaseType" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" CausesValidation="true" ValidationGroup="Go" ToolTip="Go" OnClick="btnOk_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:Panel ID="pnlVAT" runat="server" CssClass="stylePanel" GroupingText="Consolidated Sales Invoice Register" Visible="false">
                            <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1150px;">
                            <asp:GridView ID="grvVAT" runat="server" OnRowDataBound="grvVAT_RowDataBound" Width="100%" FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center">
                            </asp:GridView>
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
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
</asp:Content>

