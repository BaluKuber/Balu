<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_RPT_MRAReport.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    EnableEventValidation="false" Inherits="Reports_Report_S3G_RPT_MRAReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="ContentMRAReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpMRAReport" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="MRA Report" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMRAReport" runat="server" GroupingText="Filter Criteria"
                            CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblMRACreationFrom" runat="server" Text="Creation From Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <input id="hidDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtMRACreationFrom" runat="server" OnTextChanged="txtMRACreationFrom_TextChanged" AutoPostBack="false" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgMRACreationFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderMRACreationFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgMRACreationFrom"
                                            TargetControlID="txtMRACreationFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblMRACreationTo" runat="server" Text="Creation To Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtMRACreationTo" runat="server" OnTextChanged="txtMRACreationTo_TextChanged" AutoPostBack="false" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgMRACreationTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderMRACreationTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgMRACreationTo"
                                            TargetControlID="txtMRACreationTo">
                                        </cc1:CalendarExtender>
                                      <%--  <asp:CompareValidator ID="CVMRACreation" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" runat="server" ControlToCompare="txtMRACreationFrom" ControlToValidate="txtMRACreationTo" ErrorMessage="Creation To date cannnot be lesser than Creation From date" ValidationGroup="Go"
                                            Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblMRADateFrom" runat="server" Text="MRA From Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <input id="hidMRADate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtMRADateFrom" runat="server" OnTextChanged="txtMRADateFrom_TextChanged" AutoPostBack="false" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgMRADateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderMRADateFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgMRADateFrom"
                                            TargetControlID="txtMRADateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblMRADateTo" runat="server" Text="MRA To Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txttxtMRADateTo" runat="server" OnTextChanged="txttxtMRADateTo_TextChanged" AutoPostBack="false" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgtxtMRADateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderMRADateTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgtxtMRADateTo"
                                            TargetControlID="txttxtMRADateTo">
                                        </cc1:CalendarExtender>
                                    <%--    <asp:CompareValidator ID="CVMRA" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" runat="server" ControlToCompare="txtMRADateFrom" ControlToValidate="txttxtMRADateTo" ErrorMessage="MRA To date cannnot be lesser than MRA From date" ValidationGroup="Go"
                                            Operator="GreaterThanEqual" Type="Date"></asp:CompareValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <uc2:Suggest ID="ddlCustName" runat="server" ServiceMethod="GetCustomerName" WatermarkText="--All--"
                                            ErrorMessage="Enter Customer Name" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblMRAStatus" runat="server" Text="MRA Status" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlMRAStatus" runat="server" Width="160px">
                                            <asp:ListItem Value="0" Text="--All--" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Signed"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="UnSigned"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" colspan="4" align="center">
                        <asp:Button ID="btnSearch" runat="server" Text="Go" UseSubmitBehavior="true" ToolTip="Go" CssClass="styleSubmitButton" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" ToolTip="Clear" CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();" OnClick="btnclear_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                         <asp:Panel ID="PnlMRADetails" runat="server" Visible="false" GroupingText="MRA Details"
                            CssClass="stylePanel">
                             <asp:GridView ID="grvMRAReport" runat="server" Width="100%" EmptyDataText="No Record Found"
                                 RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" AutoGenerateColumns="False">
                                 <Columns>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="95px" HeaderStyle-HorizontalAlign="Left" HeaderText="Creation Date">
                                         <ItemTemplate>
                                             <asp:Label ID="lblCreationDate" ToolTip="MRA Creation Date" Width="95px" runat="server" Text='<%#Bind("MRACreation_Date") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Left" HeaderText="MRA Date">
                                         <ItemTemplate>
                                             <asp:Label ID="lblMRADate" runat="server" ToolTip="MRA Date" Text='<%#Bind("MRAEffective_Date") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="180px" HeaderStyle-HorizontalAlign="Left" HeaderText="Lessee Name">
                                         <ItemTemplate>
                                             <asp:Label ID="lblLesseeName" runat="server" ToolTip="Customer Name" Text='<%#Bind("CustomerName") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" HeaderText="MRA Status">
                                         <ItemTemplate>
                                             <asp:Label ID="lblMRAStatus" runat="server" ToolTip="MRA Status" Text='<%#Bind("MRAStatus") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Left" HeaderText="Disbursed during the Current FY">
                                         <ItemTemplate>
                                             <asp:Label ID="lblDisbursedduringtheCurrentFY" runat="server" ToolTip="Disbursed during the Current FY" Text='<%#Bind("CurrentFY") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" HeaderText="Amendment Status">
                                         <ItemTemplate>
                                             <asp:Label ID="lblAmendmentStatus" runat="server" ToolTip="Amendment Status" Text='<%#Bind("AmendmendStatus") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Left" HeaderText="Amendment Date">
                                         <ItemTemplate>
                                             <asp:Label ID="lblAmendmentDate" runat="server" ToolTip="Amendment Date" Text='<%#Bind("AmendmentDate") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Account Manager 1">
                                         <ItemTemplate>
                                             <asp:Label ID="lblAccountManager1" runat="server" ToolTip="Account Manager1" Text='<%#Bind("AccountManager1") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Account Manager 2">
                                         <ItemTemplate>
                                             <asp:Label ID="lblAccountManager2" runat="server" ToolTip="Account Manager2" Text='<%#Bind("AccountManager2") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Regional Manager">
                                         <ItemTemplate>
                                             <asp:Label ID="lblRegionalManager" runat="server" ToolTip="Regional Manager" Text='<%#Bind("RegionalManager") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Standard Format">
                                         <ItemTemplate>
                                             <asp:Label ID="lblStandard_Format" runat="server" ToolTip="Standard Format" Text='<%#Bind("Standard_Format") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="OPC to Give Notice">
                                         <ItemTemplate>
                                             <asp:Label ID="lblOPC_Notice" runat="server" ToolTip="OPC to Give Notice" Text='<%#Bind("OPC_Notice") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="OPC to Customer Notice Period">
                                         <ItemTemplate>
                                             <asp:Label ID="lblOPC_to_Customer" runat="server" ToolTip="OPC to Customer Notice Period" Text='<%#Bind("OPC_to_Customer") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer to Give Notice">
                                         <ItemTemplate>
                                             <asp:Label ID="lblLessee_Notice" runat="server" ToolTip="Customer to Give Notice" Text='<%#Bind("Lessee_Notice") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Customer to OPC Notice Period">
                                         <ItemTemplate>
                                             <asp:Label ID="lblCustomer_to_OPC" runat="server" ToolTip="Customer to OPC Notice Period" Text='<%#Bind("Customer_to_OPC") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Auto Extension allowed">
                                         <ItemTemplate>
                                             <asp:Label ID="lblAuto_Extension_rental" runat="server" ToolTip="Auto Extension allowed" Text='<%#Bind("Auto_Extension_rental") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Auto Extension Term (in months)">
                                         <ItemTemplate>
                                             <asp:Label ID="lblAuto_Extension_term" runat="server" ToolTip="Auto Extension Term (in months)" Text='<%#Bind("Auto_Extension_term") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Cust Termination Notice Period">
                                         <ItemTemplate>
                                             <asp:Label ID="lblCustomer_Notice_Period" runat="server" ToolTip="Cust Termination Notice Period" Text='<%#Bind("Customer_Notice_Period") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Foreclosure Rate">
                                         <ItemTemplate>
                                             <asp:Label ID="lblForeclosure_Rate" runat="server" ToolTip="Foreclosure Rate" Text='<%#Bind("Foreclosure_Rate") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Break Cost">
                                         <ItemTemplate>
                                             <asp:Label ID="lblBreak_Cost" runat="server" ToolTip="Break Cost" Text='<%#Bind("Break_Cost") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                     <asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" HeaderText="Overdue Rate">
                                         <ItemTemplate>
                                             <asp:Label ID="lblOverdue_Rate" runat="server" ToolTip="Overdue Rate" Text='<%#Bind("Overdue_Rate") %>'></asp:Label>
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                 </Columns>
                                 <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                             </asp:GridView>
                             </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Button ID="btnPrint" runat="server" Text="Export" Visible="false" ToolTip="Export" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnPrint_Click" />
                    </td>
                </tr>
                 <tr>
                    <td width="100%">
                        <asp:ValidationSummary ID="VSMRAReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="ddlCustName" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>



