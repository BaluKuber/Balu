<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptLeaseAssetNumberRegister.aspx.cs" Inherits="Reports_S3GRptLeaseAssetNumberRegister" Title="Lease Asset Register Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Lease Asset Register Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel runat="server" ID="pnlinputcriteria" CssClass="stylePanel" GroupingText="Input Criteria" Width="100%">
                    <table cellpadding="0" cellspacing="0" style="width: 100%" border="0">
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblLOB" runat="server" Text="Line Of Business"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line Of Business">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location1" ID="lblregion"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlRegion" runat="server" ToolTip="Location1" AutoPostBack="True" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%-- onselectedindexchanged="ddlRegion_SelectedIndexChanged" --%>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location2" ID="lblBranch">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlBranch" runat="server" ToolTip="Location2" Enabled="false" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="True">
                                    <%-- onselectedindexchanged="ddlBranch_SelectedIndexChanged"--%>
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblReportDate" runat="server" Text="Report Date"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtReportDate" runat="server" ToolTip="Report Date"></asp:TextBox>
                            </td>
                           <%-- <td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Name"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none" MaxLength="50" ToolTip="Customer Name"></asp:TextBox>
                                <uc2:LOV ID="ucCustomerCodeLov" onfocus="return fnLoadCustomer();" runat="server" strLOV_Code="LAN" />
                                <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" Style="display: none" OnClick="btnLoadCustomer_OnClick" />
                                <input id="hdnCustID" type="hidden" runat="server" />
                                <input id="hdnCustName" type="hidden" runat="server" />
                            </td>--%>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                        <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlDenomination" runat="server" ToolTip="Denomination">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFdeno" runat="server" ControlToValidate="ddlDenomination" InitialValue="0" ValidationGroup="Go" ErrorMessage="Select the Denomination" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                            </td>
                            <td class="styleFieldAlign" width="30%">
                            </td>
                            <%--<td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblPanum" runat="server" Text="Account Number"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlPanum" AutoPostBack="true" runat="server" CssClass="styleDisplayLabel" OnSelectedIndexChanged="ddlPanum_SelectedIndexChanged" ToolTip="Account Number">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPNum" runat="server" ErrorMessage="Select Account Number" ValidationGroup="Go" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlPanum" Enabled="false">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblSanum" runat="server" CssClass="styleDisplayLabel" Text="Sub Account Number"></asp:Label>
                                <%-- OnSelectedIndexChanged="ddlSanum_SelectedIndexChanged"--%>
                            <%--</td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlSanum" AutoPostBack="true" CssClass="styleDisplayLabel" OnSelectedIndexChanged="ddlSanum_SelectedIndexChanged" runat="server" ToolTip="Sub Account Number">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvSNum" runat="server" ErrorMessage="Select Sub Account Number" ValidationGroup="Go" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlSanum" Enabled="false">
                                </asp:RequiredFieldValidator>
                            </td>--%>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                       
                        <%--<tr>--%>
                            <%--<td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlDenomination" runat="server" ToolTip="Denomination">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFdeno" runat="server" ControlToValidate="ddlDenomination" InitialValue="0" ValidationGroup="Go" ErrorMessage="Select the Denomination" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>--%>
                            <%--<td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblReportDate" runat="server" Text="Report Date"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtReportDate" runat="server" ToolTip="Report Date"></asp:TextBox>
                            </td>--%>
                       <%-- </tr>--%>
                        <%--<tr>
                            <td height="8px">
                            </td>
                        </tr>--%>
                       
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel runat="server" ID="PnlDateRange" CssClass="stylePanel" GroupingText="Date Range" Width="100%">
                    <table width="100%">
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblLaNumber" runat="server" Text="LAN."></asp:Label>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblLanFrom" runat="server" Text="From"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <cc1:ComboBox ID="cmbLanFrom" runat="server" ToolTip="LAN From" DropDownStyle="DropDownList" AutoPostBack="true" AutoCompleteMode="SuggestAppend" MaxLength="0" OnSelectedIndexChanged="cmbLanFrom_SelectedIndexChanged" InitialValue="0" CssClass="WindowsStyle" ValidationGroup="Go">
                                </cc1:ComboBox>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblLanTo" runat="server" Text="To"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <cc1:ComboBox ID="cmbLanTo" runat="server" ToolTip="LAN To" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList" AutoPostBack="true" ValidationGroup="Go" MaxLength="0" OnSelectedIndexChanged="cmbLanTo_SelectedIndexChanged" InitialValue="0" CssClass="WindowsStyle">
                                </cc1:ComboBox>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:CheckBox ID="chkWithYield" runat="server" Text="With Yield" TextAlign="Right" ToolTip="With Yield" OnCheckedChanged="chkWithYield_CheckedChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblDateFromRange" runat="server" Text="Date Range"></asp:Label>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblDateFrom" runat="server" CssClass="styleReqFieldLabel" Text="From"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtDateFrom" runat="server" ToolTip="Date Range From"></asp:TextBox>
                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateFrom" OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgStartDate" ID="CalendarExtender1">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter the Date Range From" ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtDateFrom"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblTo" runat="server" CssClass="styleReqFieldLabel" Text="To"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtDateTo" runat="server" ToolTip="Date Range To"></asp:TextBox>
                                <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateTo" OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgEndDate" ID="CalendarExtender2">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Enter the Date Range To" ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtDateTo">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:CheckBox ID="ChkExpense" runat="server" Text="With Revenue Expenses" TextAlign="Right" ToolTip="With Revenue Expenses" AutoPostBack="true" OnCheckedChanged="ChkExpense_CheckedChanged" />
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
            <td style="height: 8px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td align="center">
                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="styleSubmitButton" OnClick="btnGo_Click" OnClientClick="return fnCheckPageValidators('Go',false);"  ValidationGroup="Go" ToolTip="Go" CausesValidation="True" />
                &nbsp;&nbsp;
                <asp:Button ID="btnReset" runat="server" Text="Clear" CssClass="styleSubmitButton" CausesValidation="false" OnClientClick="return fnConfirmClear();" OnClick="btnReset_Click" ToolTip="Clear" />
               
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlLease" runat="server" CssClass="stylePanel" GroupingText="Lease Asset Number Details" Width="100%" Visible="false">
                    <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <div id="divLeaseDetails" runat="server" style="overflow: auto; height: 300px; display: none">
                        <asp:GridView ID="grvLeaseDetails" runat="server" Width="100%" AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DocumentDate") %>' ToolTip="Date"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocumentNo" runat="server" Text='<%# Bind("DocumentNo") %>' ToolTip="Document Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LAN.">
                                    <ItemTemplate>
                                    <asp:LinkButton ID="lnkLan" runat="server" Text='<%# Bind("Lan") %>' ToolTip="Lease Asset Number" OnClick="lnkLan_Click"></asp:LinkButton>
<%--                                        <asp:Label ID="lblLan" runat="server" Text='<%# Bind("Lan") %>' ToolTip="Lease Asset Number"></asp:Label>
--%>                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPanum" runat="server" Text='<%# Bind("Panum") %>' ToolTip="Account Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sub Account No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSanum" runat="server" Text='<%# Bind("Sanum") %>' ToolTip="Sub Account Number"></asp:Label>
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
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' ToolTip="Description"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Debit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit" runat="server" Text='<%# Bind("Debit") %>' ToolTip="Debit"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalDebit" runat="server" ToolTip="Sum of Debit Amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <FooterStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit" runat="server" Text='<%# Bind("Credit") %>' ToolTip="Credit"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalCredit" runat="server" ToolTip="Sum of  Credit Amount"></asp:Label>
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
                                        <asp:Label ID="lblTotalBalance" runat="server" ToolTip="Sum of Balance Amount"></asp:Label>
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
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlAssetdetails" runat="server" CssClass="stylePanel" GroupingText="Asset Details" Width="100%" Visible="false">
                   <%-- <div id="divAssetDetails" runat="server" style="overflow: auto; height: 50px; display: none">--%>
                        <asp:GridView ID="grvAssetdetails" runat="server" AutoGenerateColumns="False" Width="100%" FooterStyle-HorizontalAlign="Right" RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0" >
                            <Columns>
                                <asp:TemplateField HeaderText="LAN.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLan" runat="server" Text='<%# Bind("Lan") %>' ToolTip="Lan Number"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Desc.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetDescription" runat="server" Text='<%# Bind("AssetDescription") %>' ToolTip="Asset Description"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Regn. No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegnNumber" runat="server" Text='<%# Bind("RegnNumber") %>' ToolTip="Registration Number"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chasis No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblChasisNumber" runat="server" Text='<%# Bind("ChasisNumber") %>' ToolTip="Chasis Number"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Engine No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEngineNumber" runat="server" Text='<%# Bind("EngineNumber") %>' ToolTip="Engine Number"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Serial No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Bind("SerialNumber") %>' ToolTip="Serial Number"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Performing Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPerformingStatus" runat="server" Text='<%# Bind("PerformingStatus") %>' ToolTip="PerformingStatus"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Availability Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailabilityStatus" runat="server" Text='<%# Bind("AvailabilityStatus") %>' ToolTip="AvailabilityStatus"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LAN. Booking Upto">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLanBookingUpto" runat="server" Text='<%# Bind("LanBookingUpto") %>' ToolTip="LAN. Booking Upto"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                              
                            </Columns>
                        </asp:GridView>
                    <%--</div>--%>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary runat="server" ID="vsLAN" HeaderText="Please correct the following validation(s):" CssClass="styleSummaryStyle" ShowMessageBox="false" ValidationGroup="Go" ShowSummary="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVLAN" runat="server" Display="None" ValidationGroup="Go"></asp:CustomValidator>
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td align="center">
                <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnprint_Click"/>
            </td>
        </tr>
    </table>

    <%--<script language="javascript" type="text/javascript">
    function fnLoadCustomer()
        {
        document.getElementById("<%= btnLoadCustomer.ClientID %>").click();

        }
      

        
    </script>--%>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
 