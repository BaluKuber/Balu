<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" 
AutoEventWireup="true" CodeFile="S3GRptInsuranceMaturityReport.aspx.cs" Inherits="Reports_S3GRptInsuranceMaturityReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" 
                            Text="Insurance Maturity Report"></asp:Label>
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
                                        <asp:Label ID="lblLocation1" runat="server" CssClass="styleMandatoryLabel" Text="Location1"
                                            ToolTip="Location1"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLocation1" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                            ToolTip="Location">
                                        </asp:DropDownList>
                                         <%--OnSelectedIndexChanged="ddlLocation1_SelectedIndexChanged"--%>
                                    </td>
                                </tr>
                                <%--<tr>                                   
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:Label ID="lblLocation2" runat="server" CssClass="styleMandatoryLabel" Text="Location2"
                                            ToolTip="Location2"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:DropDownList ID="ddlLocation2" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                            ToolTip="Location">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblStartDate" runat="server" CssClass="styleMandatoryLabel" Text="Start Date"
                                            ToolTip="Start Date" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <input id="hidStartDate" runat="server" type="hidden" />
                                        <asp:TextBox ID="txtStartDateSearch" runat="server" AutoPostBack="true" Width="100" ToolTip="Start Date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="None" ControlToValidate="txtStartDateSearch"
                                            ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select Start Date"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="ceStartDate" runat="server" Enabled="True" PopupButtonID="imgStartDateSearch"
                                            TargetControlID="txtStartDateSearch">
                                        </cc1:CalendarExtender>
                                    </td>                                    
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:Label ID="lblEndDate" runat="server" CssClass="styleMandatoryLabel" Text="End Date"
                                            ToolTip="End Date" />
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <input id="hidEndDate" runat="server" type="hidden" />
                                        <asp:TextBox ID="txtEndDateSearch" runat="server" AutoPostBack="true" Width="100" ToolTip="End Date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" Display="None" ControlToValidate="txtEndDateSearch"
                                            ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select End Date"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="ceEndDate" runat="server" Enabled="True" PopupButtonID="imgEndDateSearch"
                                            TargetControlID="txtEndDateSearch">
                                        </cc1:CalendarExtender>
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
                        <div id="divInsuranceMaturity" runat="server" style="overflow: scroll; height: 200px;" visible="false">
                            <asp:GridView ID="grvInsuranceMaturity" runat="server" AutoGenerateColumns="False" BorderWidth="2"
                                CssClass="styleInfoLabel" Style="margin-bottom: 0px" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocationI" runat="server" Text='<%# Bind("Location") %>'
                                                ToolTip="Location"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>                                                                
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
                                    <asp:TemplateField HeaderText="Registration No" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRegistrationNoI" runat="server" Text='<%# Bind("RegistrationNo") %>' ToolTip="Registration No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Policy Type" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPolicyTypeI" runat="server" Style="text-align: right" Text='<%# Bind("PolicyType") %>'
                                                ToolTip="Policy Type"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy Number" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPolicyNumberI" runat="server" Style="text-align: right" Text='<%# Bind("PolicyNumber") %>'
                                                ToolTip="Policy Number"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Insurance Company" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInsuranceCompanyI" runat="server" Style="text-align: right" Text='<%# Bind("InsuranceCompany") %>'
                                                ToolTip="Insurance Company"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Policy Maturity Date" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPolicyMaturityDateI" runat="server" Style="text-align: right" Text='<%# Bind("PolicyMaturityDate") %>'
                                                ToolTip="Policy Maturity Date"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Premium Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPremiumAmountI" runat="server" Style="text-align: right" Text='<%# Bind("PremiumAmount") %>'
                                                ToolTip="Premium Amount"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount Financed" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmountFinancedI" runat="server" Style="text-align: right" Text='<%# Bind("AmountFinanced") %>'
                                                ToolTip="Amount Financed"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Account Maturity Date" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountMaturityDateI" runat="server" Style="text-align: right" Text='<%# Bind("AccountMatDate") %>'
                                                ToolTip="Account Maturity Date"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />                                        
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
                        <asp:CustomValidator ID="CVInsuranceMaturity" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnPrint" runat="server" />
</asp:Content>

