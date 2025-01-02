<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptEntityLedger.aspx.cs" Inherits="Reports_S3GRptEntityLedger" Title="Entity Ledger" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Entity Ledger Report">
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
                                <asp:Label runat="server" Text="Entity Name" ToolTip="Entity Name" ID="lblEntityName" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlEntityName" ToolTip="Entity Name" runat="server" AutoPostBack="true" Width="65%" OnSelectedIndexChanged="ddlEntityName_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvEntityName" runat="server" ErrorMessage="Select Entity Name" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="0" ControlToValidate="ddlEntityName">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Denomination" ToolTip="Denomination" ID="lblDenomination" CssClass="styleDisplayLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlDenomination" runat="server"  ToolTip="Denomination" Width="65%">
                                </asp:DropDownList>
                                </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Start Date" ToolTip="Start Date" ID="lblStartDate" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtStartDate" ToolTip="Start Date" runat="server" ></asp:TextBox>
                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Select Start Date" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="End Date" ToolTip="End Date" ID="lblEndDate" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtEndDate" runat="server"  ToolTip="End Date"></asp:TextBox>
                                <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate" PopupButtonID="imgEndDate" ID="CalendarExtender2" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Select End Date" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtEndDate">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="GO" CausesValidation="true" OnClientClick="return fnCheckPageValidators('Ok',false);" ValidationGroup="Ok" OnClick="btnOk_Click" ToolTip="Go" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlVendorDetails" runat="server" CssClass="stylePanel" GroupingText="Entity Ledger" Width="100%" Visible="false">
                    <asp:Label ID="lblOpeningBalance" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <div id="divVendorDetails" runat="server" style="overflow: auto; height: 300px; display: none">
                        <asp:GridView ID="grvVendorDetails" runat="server" Width="100%" AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Account No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PrimeAccountNo") %>' ToolTip="Account Number"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Account No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SubAccountNo") %>' ToolTip="Sub Account Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc. Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocDate" runat="server" Text='<%# Bind("DocumentDate") %>' ToolTip="Document Date"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvaluedate" runat="server" Text='<%# Bind("ValueDate") %>' ToolTip="Value Date"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Doc. Reference">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentReference" runat="server" Text='<%# Bind("DocumentReference") %>' ToolTip="Document Reference Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Bind("Description") %>' ToolTip="Description"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Width="20%"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Debit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDues" runat="server" Text='<%# Bind("Dues") %>' ToolTip="Debit"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalDues" runat="server" ToolTip="sum of Dues Amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceipts" runat="server" Text='<%# Bind("Receipts") %>' ToolTip="Credit"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalReceipts" runat="server" ToolTip="sum of  Receipts Amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Balance">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBalance" runat="server" Text='<%# Bind("Balance") %>' ToolTip="Balance"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalBalance" runat="server" ToolTip="sum of Balance Amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnPrint" ToolTip="Print" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsVendor" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVVendorDetails" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>
</asp:Content>
