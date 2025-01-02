<%@  Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptPDCSummaryReport.aspx.cs" Inherits="Reports_S3GRptPDCSummaryReport" Title="PDC Summary Report" %>
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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="PDC Summary Report">
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
                                        <asp:Label ID="lblStartDate" runat="server" Text="Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" ToolTip="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblChequeRemarks" runat="server" Text="Cheque Remarks" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlChequeRemarks" runat="server" ToolTip="Note">
                                        </asp:DropDownList>
                                    </td>
                                     <td class="styleFieldLabel">
                                        <asp:Label ID="lblFunderName" runat="server" Text="Funder Name" CssClass="styleDisplayLabel" ToolTip="Funder Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunderNameDetails" WatermarkText="--All--" />
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
                        <asp:Panel ID="pnlPDC" runat="server" CssClass="stylePanel" GroupingText="PDC Summary Report" Visible="false">
                            <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                            <asp:GridView ID="grvPDC" runat="server" Width="100%" FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                            </asp:GridView>
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
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
