<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_FundMgt_GrossMarginCalculation.aspx.cs" Inherits="Fund_Management_S3G_FundMgt_GrossMarginCalculation" %>

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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Gross Margin Calculation">
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
                        <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Input Details" CssClass="stylePanel" Width="100%">
                            <table align="center" width="100%" border="0" cellspacing="0">
                                <%--<tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStartDate" runat="server" Text="Rental Start Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblenddate" runat="server" Text="Rental End Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtenddate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgtxtenddate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtenddate" PopupButtonID="imgtxtenddate">
                                        </cc1:CalendarExtender>
                                    </td>
                                   
                                </tr>--%>
                                <tr>
                                      <td class="styleFieldLabel">
                                        <asp:Label ID="lblTrancheNumber" runat="server" Text="Tranche Number" CssClass="styleDisplayLabel" ToolTip="RS Number">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlTrancheNumber" runat="server" ServiceMethod="GetTrancheList" AutoPostBack="true"
                                            WatermarkText="--All--" OnItem_Selected="ddlTrancheNumber_SelectedIndexChanged" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRSNo" runat="server" Text="Rental Schedule No" CssClass="styleDisplayLabel" ToolTip="Rental Schedule No">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlRSNo" runat="server"  AutoPostBack="true" ServiceMethod="GetRSNoDetails"
										OnItem_Selected="ddlTrancheNumber_SelectedIndexChanged" WatermarkText="--All--" />
                                    </td>
                                  
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlVAT" runat="server" CssClass="stylePanel" GroupingText="Gross Margin Calculation" Visible="false" Width="100%">

                            <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" Width="100%">
                                <table align="center" width="100%" border="0" cellspacing="0">
                                    <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label3" runat="server" Text="Invoice Value" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoiceValue" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label4" runat="server" Text="ITC Available" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtITCAvailable" ReadOnly="true" runat="server"></asp:TextBox>
                                        
                                    </td>
                                        <td class="styleFieldLabel">
                                        <asp:Label ID="Label5" runat="server" Text="Discounting Rate" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDiscountingRate" AutoPostBack="true" OnTextChanged="txtDiscountingRate_TextChanged" runat="server"></asp:TextBox>
                                    </td>
                                         <td class="styleFieldLabel">
                                        <asp:Label ID="Label10" runat="server" Text="Gross Margin / NBR" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtGrossMarginNBR" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                    <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label7" runat="server" Text="RS  Value" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRSValue" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label8" runat="server" Text="Rebate Discount Value" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRebateDiscountValue" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                     <td class="styleFieldLabel">
                                        <asp:Label ID="Label9" runat="server" Text="Security Deposit" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtSecurityDeposit" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                        <td></td><td></td>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label1" runat="server" Text="Rental Start Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRentalStartDate" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="Label2" runat="server" Text="Rental End Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRentalEndDate" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                     <td class="styleFieldLabel">
                                        <asp:Label ID="Label6" runat="server" Text="EOT Guaranteed Value" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtEOTGuaranteedValue" ReadOnly="true" runat="server"></asp:TextBox>
                                    </td>
                                    <td></td><td></td>
                                </tr>
                                   </table>
                            </asp:Panel>
                            <%--<div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1085px">--%>
                            <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                            <asp:GridView ID="grvGST" runat="server" OnRowDataBound="grvGST_RowDataBound" Width="100%" FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center">
                            </asp:GridView>
                            <%--<uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>--%>
                            <%--</div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Save" CausesValidation="true" ValidationGroup="Go" ToolTip="Save" OnClick="btnSave_Click" OnClientClick="return SaveConfirm();"/>
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                        &nbsp;<asp:Button runat="server" ID="btnCancel" CausesValidation="false" CssClass="styleSubmitButton" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnExport" CssClass="styleSubmitButton" Text="Export" CausesValidation="false" ValidationGroup="Export" OnClick="btnExport_Click" Visible="false" ToolTip="Export" />
                        &nbsp;<asp:Button runat="server" ID="btnReCal" CssClass="styleSubmitButton" Text="Re Calculate" CausesValidation="false" OnClick="btnReCal_Click" Visible="false" ToolTip="Re Calculate" />
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
    <script type="text/javascript">
        function SaveConfirm() {
            //debugger;

            return confirm('Do you want to Save?');

        }
    </script>
</asp:Content>

