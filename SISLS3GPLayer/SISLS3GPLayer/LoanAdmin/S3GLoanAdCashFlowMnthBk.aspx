<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdCashFlowMnthBk.aspx.cs" Inherits="LoanAdmin_S3GLoanAdCashFlowMnthBk"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="4" style="height: 19px">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Cashflow Monthly Booking">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblLineofBusiness" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlLineofBusiness" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLineofBusiness_SelectedIndexChanged"
                            TabIndex="3" ToolTip="Line Of Business">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvLineofBusiness" runat="server" ErrorMessage="Select Line of Business"
                            InitialValue="0" Display="None" ControlToValidate="ddlLineofBusiness" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblBranch" runat="server" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            TabIndex="4" ToolTip="Branch">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ErrorMessage="Select Location"
                            InitialValue="0" Display="None" ControlToValidate="ddlBranch" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlFinancialYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged"
                            TabIndex="1" ToolTip="Financial Year">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFinYear" runat="server" ErrorMessage="Select Financial Year"
                            InitialValue="0" Display="None" ControlToValidate="ddlFinancialYear" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblFinancialMonth" runat="server" Text="Financial Month" CssClass="styleReqFieldLabel"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlFinancialMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialMonth_SelectedIndexChanged"
                            TabIndex="2" ToolTip="Financial Month">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvFinMonth" runat="server" ErrorMessage="Select Financial month"
                            InitialValue="0" Display="None" ControlToValidate="ddlFinancialMonth" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblCahflowType" runat="server" CssClass="styleReqFieldLabel" Text="Cashflow Type"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:DropDownList ID="ddlCashFlowType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCashFlowType_SelectedIndexChanged"
                            TabIndex="5" ToolTip="Cashflow Type">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="2">Both</asp:ListItem>
                            <asp:ListItem Value="1">Payables</asp:ListItem>
                            <asp:ListItem Value="0">Receivables</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCashFlowType" runat="server" ErrorMessage="Select Cashflow Type"
                            InitialValue="-1" Display="None" ControlToValidate="ddlCashFlowType" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblCFMDate" runat="server" CssClass="styleReqFieldLabel" Text="Cashflow Monthly Booking Date"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtCFMDate" runat="server" TabIndex="7" ToolTip="Cashflow Monthly Booking Date "
                            Width="90px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:Label ID="lblCFMNumber" runat="server" CssClass="styleReqFieldLabel" Text="Cashflow Monthly Booking Number"></asp:Label>
                    </td>
                    <td class="styleFieldAlign">
                        <asp:TextBox ID="txtCFMNumber" runat="server" Width="150px" TabIndex="6" ToolTip="Auto Generated Cashflow Monthly Booking Number"></asp:TextBox>
                    </td>
                    <td class="styleFieldLabel">
                    </td>
                    <td class="styleFieldAlign">
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding-top: 10px">
                        <asp:GridView ID="grvCashflowDetails" runat="server" AutoGenerateColumns="False"
                            Width="100%" ToolTip="Cashflow details for selected Line Of Business and Financial Month">
                            <Columns>
                                <asp:BoundField DataField="PANumber" HeaderText="Rental Schedule Number" SortExpression="PANumber">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SANumber" Visible="false" HeaderText="Sub Account Number">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cust_Name" HeaderText="Customer Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CASHFLOW_DATE" HeaderText="Cashflow Date">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cash_Flow_Desc" HeaderText="Cashflow Description">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Cashflow_id" HeaderText="Cashflow_id" ReadOnly="true" />
                                <asp:BoundField DataField="AccountCashFlow_Details_ID" HeaderText="AccountCashFlow_Details_ID"
                                    ReadOnly="true">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="Amount">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Sys_JV_Ref" HeaderText="Sys JV Ref">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center" style="padding-top: 10px">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators('Submit')"
                            TabIndex="8" />
                        <asp:Button ID="btnRevoke" runat="server" Text="Revoke" CssClass="styleSubmitButton"
                            OnClick="btnRevoke_Click" OnClientClick="return confirm('Do you want to Revoke?');"
                            CausesValidation="false" Visible="false" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" CausesValidation="false"
                            TabIndex="9" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" CausesValidation="false" TabIndex="10" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:CustomValidator runat="server" ID="cvCashFlowMnthBk" Display="None" CssClass="styleMandatoryLabel"
                            ValidationGroup="Submit"></asp:CustomValidator>
                        <asp:ValidationSummary ID="vsCashFlowMnthBk" runat="server" ValidationGroup="Submit"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Add"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
