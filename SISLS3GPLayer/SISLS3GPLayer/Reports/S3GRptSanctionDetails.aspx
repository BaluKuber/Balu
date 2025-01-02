<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptSanctionDetails.aspx.cs" Inherits="Reports_S3GRptSanctionDetails" Title="Sanction Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Sanction Details Report">
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
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel" ToolTip="Line of Business">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" Width="65%" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" ToolTip="Line of Business">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ErrorMessage="Select Line Of Business" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlLOB">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location1" ID="lblRegion" ToolTip="Location1">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" Width="65%" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" ToolTip="Location1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location2" ID="lblBranch" ToolTip="Location2">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" Width="65%" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ToolTip="Location2" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Product" ID="lblProduct" ToolTip="Product">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" ToolTip="Product">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Start Date" ID="lblStartDate" CssClass="styleReqFieldLabel" ToolTip="Start Date">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtStartDate" runat="server" Width="56%" ToolTip="Start Date"></asp:TextBox>
                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Select Start Date" ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="End Date" ID="lblEndDate" CssClass="styleReqFieldLabel" ToolTip="End Date">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtEndDate" runat="server" Width="56%" ToolTip="End Date"></asp:TextBox>
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
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination" CssClass="styleReqFieldLabel" ToolTip="Denomination">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlDenomination" runat="server" Width="65%" ToolTip="Denomination">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="rfvDenomination" runat="server" ErrorMessage="Select Denomination" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="1" ControlToValidate="ddlDenomination">
                                </asp:RequiredFieldValidator>--%>
                            </td>
                            <td width="20%">
                            </td>
                            <td width="30%">
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
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="GO" OnClientClick="return fnCheckPageValidators('Ok',false);" CausesValidation="true" ValidationGroup="Ok" OnClick="btnOk_Click" ToolTip="Go" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" ToolTip="Clear" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                <%--<asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlSanctionDetails" runat="server" CssClass="stylePanel" GroupingText="Sanction Details" Width="100%" Visible="false">
                    <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <div id="divSanctionDetails" runat="server" style="overflow: auto; height: 300px; display: none">
                        <asp:GridView ID="grvSanctionDetails" runat="server" Width="100%" AutoGenerateColumns="false" 
                                                    FooterStyle-HorizontalAlign="Right" RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0"
                                                     ShowFooter="true" OnRowDataBound="grvSanctionDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegion" runat="server" Text='<%# Bind("REGION") %>' ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrandTotalR" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Branch" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("BRANCH") %>' ToolTip="Branch"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalB" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("PRODUCT_NAME") %>' ToolTip="Product"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotP" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Application No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApplicationNo" runat="server" Text='<%# Bind("APPLICATIONNO") %>' ToolTip="Application No"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGranTotalA" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("CUSTOMERNAME") %>' ToolTip="Customer Name"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PANUM") %>' ToolTip="Account Number"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Account No." ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SANUM") %>' ToolTip="Sub Account Number"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Finance Amt. Sought" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinSaught" runat="server" Text='<%# Bind("FINANCEAMOUNTSOUGHT") %>' ToolTip="Finance Amount Saught"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblFinamtSaught" runat="server" ToolTip="Total Finance Amount Saught"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Finance Amt. Offered" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinOffered" runat="server" Text='<%# Bind("FINANCEAMOUNTOFFERED") %>' ToolTip="Finance Amount Offered"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblFinamtOffered" runat="server" ToolTip="Total Finance Amount Offered"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disbursable" ItemStyle-Width="400px" HeaderStyle-Width="400px">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" height="100%" style="margin-top: -2px; margin-bottom: -4px">
                                            <tr>
                                                <td width="43%">
                                                    Disbursable Date
                                                </td>
                                                <td height="100%" width="1%">
                                                    <img src="../Images/Saranya/border.png" height="35px" width="1px" />
                                                </td>
                                                <td>
                                                    Disbursable Amt.
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:GridView ID="grvDisbursable" CellPadding="0" CellSpacing="0" runat="server" AutoGenerateColumns="false" GridLines="None" DataSource='<%# Bind("Disbursable")%>' ShowHeader="false" Width="100%" Height="100%" OnRowDataBound="grvDisbursable_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Disbursable Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="44%" HeaderStyle-Width="44%">
                                                    <ItemTemplate>
                                                        <%# Eval("DISBURSABLE_DATE") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Disbursable Amt" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDisbursableAmnt" runat="server" Text='<%# Bind("DISBURSABLE_AMOUNT") %>' ToolTip="Disbursable Amount"></asp:Label>
                                                        <%-- <%# Eval("DISBURSABLE_AMOUNT") %>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblDisbursableAmt" runat="server" ToolTip="Total Disbursable Amount"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disbursed" ItemStyle-Width="400px" HeaderStyle-Width="400px">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0" height="100%" style="margin-top: -2px; margin-bottom: -4px">
                                            <tr>
                                                <td width="30%">
                                                    Payment Request No.
                                                </td>
                                                <td height="100%" width="1%">
                                                    <img src="../Images/Saranya/border.png" height="35px" width="1px" />
                                                </td>
                                                <td width="30%">
                                                    Disbursed Date
                                                </td>
                                                <td height="100%" width="1%">
                                                    <img src="../Images/Saranya/border.png" height="35px" width="1px" />
                                                </td>
                                                <td>
                                                    Disbursed Amt.
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:GridView ID="grvDisbursed" runat="server" AutoGenerateColumns="false" CellPadding="0" CellSpacing="0" DataSource='<%# Bind("Disbursed")%>' ShowHeader="false" Width="100%" Height="100%" OnRowDataBound="grvDisbursed_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Payment Request No" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" HeaderStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <%# Eval("DISBURSED_NO") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Disbursed Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="32%" HeaderStyle-Width="32%">
                                                    <ItemTemplate>
                                                        <%# Eval("DISBURSED_DATE") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Disbursed Amt" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDisbursedAmnt" runat="server" Text='<%# Bind("DISBURSED_AMOUNT") %>' ToolTip="Disbursed Amount"></asp:Label>
                                                        <%-- <%# Eval("DISBURSED_AMOUNT") %>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblDisbursedAmt" runat="server" ToolTip="Total Disbursed Amount"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlSummary" runat="server" CssClass="stylePanel" GroupingText="Sanction Details" Width="100%" Visible="false">
                    <asp:Label ID="lblgridError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <div id="divSummary" runat="server" style="overflow: auto; height: 300px; display: none">
                        <asp:GridView ID="grvSummary" runat="server" Width="100%" AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true" OnRowDataBound="grvSummary_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegion" runat="server" Text='<%# Bind("REGION") %>' ToolTip="Location"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrndTotalR" runat="server" Text="Grand Total" Visible="false" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Branch" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("BRANCH") %>' ToolTip="Branch"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGranTotalB" runat="server" Text="Grand Total" Visible="false" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("PRODUCT_NAME") %>' ToolTip="Product"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGranTotalP" runat="server" Text="Grand Total" Visible="false"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fin. Amount Sought" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinamountSaught" runat="server" Text='<%# Bind("FINANCEAMOUNTSOUGHT") %>' ToolTip="Finance Amount Saught"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotFinanceAmountSaught" runat="server" ToolTip="Total Finance Amount Saught"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fin. Amount Offered" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinamountOffered" runat="server" Text='<%# Bind("FINANCEAMOUNTOFFERED") %>' ToolTip="Finance Amount Offered"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotFinanceAmountOffered" runat="server" ToolTip="Total Finance Amount Offered"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disbursable Amt." ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDisbursableAmount" runat="server" Text='<%# Bind("DISBURSABLE_AMOUNT") %>' ToolTip="Disbursable Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotDisbursableAmount" runat="server" ToolTip="Total Disbursable Amount"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Disbursed Amt." ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDisbursedAmount" runat="server" Text='<%# Bind("DISBURSED_AMOUNT") %>' ToolTip="Disbursed Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotDisbursedAmount" runat="server" ToolTip="Total Disbursed Amount"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="4" align="center">
                <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click" ToolTip="Print" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsSanction" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok"  />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVSanctionDetails" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>
</asp:Content>
