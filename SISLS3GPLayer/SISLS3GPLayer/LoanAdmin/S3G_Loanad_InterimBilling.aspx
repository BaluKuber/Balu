<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Loanad_InterimBilling.aspx.cs" Inherits="LoanAdmin_S3G_Loanad_InterimBilling" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LOV" Src="~/UserControls/LOBMasterView.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Interim Billing- Create" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="INTERIM BILLING">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails" AutoPostBack="true" Width="200px" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblTranche" runat="server" Text="Tranche">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlTranche" runat="server" ServiceMethod="GetTrancheDetails" AutoPostBack="true" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblRSNO" runat="server" Text="Rent Schedule No." CssClass="styleDisplayFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlRSNO" runat="server" ServiceMethod="GetRSNODetails" AutoPostBack="true" Width="200px" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lblLCdatefrom" runat="server" Text="Interim Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtInterimDate" runat="server" Width="200px" TabIndex="5" MaxLength="10"></asp:TextBox>
                                        <asp:Image ID="imgLCdatefrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtInterimDate"
                                            PopupButtonID="imgLCdatefrom" Enabled="True">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtInterimDate" SetFocusOnError="True"
                                            ErrorMessage="Enter the Interim Date" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Main"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lbldocno" runat="server" Text="Document Number" CssClass="styleDisplayFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtDocno" runat="server" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Due Date" ID="lblDueDate" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDueDate" runat="server" ToolTip="Due Date" Width="200px">
                                        </asp:TextBox>
                                        <asp:Image ID="imgDueDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ErrorMessage="Enter the Due Date" ValidationGroup="Main"
                                            ID="rfvDueDate" runat="server" ControlToValidate="txtDueDate" CssClass="styleMandatoryLabel"
                                            Display="None"></asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender ID="CalExtenderDueDate" runat="server" Enabled="True"
                                            PopupButtonID="imgDueDate" TargetControlID="txtDueDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="4" align="center">
                                        <asp:Button runat="server" ID="btnGo" CssClass="styleSubmitButton"
                                            Text="Go" ValidationGroup="Main" OnClick="btnGo_click" />
                                    </td>

                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel GroupingText="Interim Details" ID="pnlAssetDet" runat="server" CssClass="stylePanel" Width="99%">
                            <div id="divinterim" runat="server" style="overflow: scroll;">
                                <asp:GridView ID="gvInterim" runat="server" AutoGenerateColumns="false" ShowFooter="true" Width="100%" OnRowDataBound="gvInterim_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lessee">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcustomer" runat="server" Text='<%#Bind("lessee") %>' ToolTip="RSNO"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rent Sch No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%#Bind("PANum") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Rental Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRentalDate" runat="server" Text='<%#Bind("First_start_Date") %>' ToolTip="Filing Due Date"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Calc Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcalcdate" runat="server" Text='<%#Bind("Last_calc_date") %>' ToolTip="Invoice Date"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Amount" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpaymentamount" runat="server" Text='<%#Bind("payment_amount") %>' ToolTip="Invoice Amount"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Interim Rate" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lbInterimRate" runat="server" Text='<%#Bind("Interim_Rate") %>' ToolTip="Invoice Amount"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Interim Amount" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInterimAmount" runat="server" Text='<%#Bind("Interim_rent") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Interim Amount AMF" HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInterimAmount_amf" runat="server" Text='<%# Eval("Interim_rent_amf") %>'
                                                    ToolTip="Way Bill No." />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rental Tax Amount" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVATAmount" runat="server" Text='<%# Eval("VAT_Amount") %>' ToolTip="Way Bill Issue Date" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AMF Tax Amount" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserviceTaxAount" runat="server" Text='<%# Eval("ServiceTax_Amount") %>' ToolTip="Way Bill Issue Date" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                            <HeaderTemplate>
                                                <table>
                                                    <tr>
                                                        <td>Select All
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>

                            </div>
                        </asp:Panel>
                    </td>
                </tr>

                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            Text="Save" ValidationGroup="Main" Enabled="false" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" TabIndex="16" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click"
                            CssClass="styleSubmitButton" />
                        <asp:Button runat="server" ID="btnPrintRental" Text="Print Rental" CausesValidation="false" OnClick="btnPrintRental_Click"
                            CssClass="styleSubmitButton" />
                        <asp:Button runat="server" ID="btnPrintAMF" Text="Print AMF" CausesValidation="false" OnClick="btnPrintAMF_Click"
                            CssClass="styleSubmitButton" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="Main" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="Main" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>

                        <asp:CustomValidator ID="cvTranche" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrintRental" />
            <asp:PostBackTrigger ControlID="btnPrintAMF" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

