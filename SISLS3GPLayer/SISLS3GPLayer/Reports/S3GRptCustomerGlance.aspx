<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptCustomerGlance.aspx.cs" Inherits="Reports_S3GRptCustomerGlance"
    Title="Customer At a Glance" %>

<%--Ajax Control Toolkit--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%--Content--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td>
                <table width="100%" cellpadding="0" border="0">
                    <tr>
                        <td class="stylePageHeading">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblHeading" EnableViewState="false" CssClass="styleInfoLabel"
                                            Text="Customer At a Glance-Report">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" border="0">
                    <tr>
                        <td width="50%" valign="top" align="left">
                            <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
                                CssClass="stylePanel" Width="100%">
                                <table width="100%" border="0" cellspacing="0">
                                    <tr>
                                        <td class="styleFieldLabel" width="35%">
                                            <asp:Label ID="CustomerName" Text="Customer Name" Enabled="true" runat="server" ToolTip="CustomerName">
                                            </asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                          <uc2:LOV ID="ucCustomerCodeLov" onfocus="return fnLoadCustomer();" runat="server"
                                                strLOV_Code="CMD" />
                                            <asp:TextBox ID="txtCustomerCode" runat="server" Style="display:none" MaxLength="90" AutoPostBack="true" ToolTip="CustomerCode"></asp:TextBox>                                          
                                            <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" OnClick="btnLoadCustomer_OnClick"
                                                Style="display: none" Width="100%" /><input id="hdnCustID" type="hidden" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <uc1:S3GCustomerAddress ID="ucCustDetails" runat="server" FirstColumnStyle="styleFieldLabel"
                                                SecondColumnStyle="styleFieldAlign" ShowCustomerName="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td width="50%">
                            <asp:Panel ID="pnlHeaderDetails" runat="server" GroupingText="Input Criteria" CssClass="stylePanel">
                                <table width="100%" border="0">
                                    <tr>
                                        <%--Content--%>
                                        <td class="styleFieldLabel">
                                            <asp:Label runat="server" Text="Line of Business" ID="lblLOBSearch" CssClass="styleMandatoryLabel" ToolTip="Line of Business" />
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:DropDownList ID="ComboBoxLOBSearch" AutoPostBack="true" ValidationGroup="RFVPDCReminder"
                                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                                MaxLength="0" CssClass="WindowsStyle" Height="23px" CausesValidation="true" Width="172px"
                                                OnSelectedIndexChanged="ComboBoxLOBSearch_SelectedIndexChanged" ToolTip="Line of Business">
                                            </asp:DropDownList>
                                            <%-- <asp:RequiredFieldValidator ID="RFVLineOfBusiness" runat="server" SetFocusOnError="true" ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel" Display="none"
                                ErrorMessage="Select a Line Of Business" Enabled="true" ControlToValidate="ComboBoxLOBSearch" InitialValue="-1"> 
                            RfvLOB</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <%--Content--%>
                                        <td class="styleFieldLabel">
                                            <asp:Label runat="server" Text="Location1" ID="lblRegionSearch" CssClass="styleMandatoryLabel" ToolTip="Location1" />
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:DropDownList ID="ComboBoxRegion" AutoPostBack="true" runat="server" AutoCompleteMode="SuggestAppend"
                                                DropDownStyle="DropDownList" MaxLength="0" CssClass="WindowsStyle" OnSelectedIndexChanged="ComboBoxRegion_SelectedIndexChanged"
                                                Width="172px" Height="23px" ToolTip="Location1">
                                            </asp:DropDownList>
                                            <%-- <asp:RequiredFieldValidator ID="RFVRegion" runat="server"  ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel" Display="none"
                                ErrorMessage="Select a Region" Enabled="true" SetFocusOnError="true" ControlToValidate="ComboBoxRegion" InitialValue="-1">RfvRegion</asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5px">
                                        </td>
                                    </tr>
                                    <%--Spacer--%>
                                    <tr>
                                        <%--Row 1 with 4 columns--%>
                                        <td class="styleFieldLabel">
                                            <asp:Label runat="server" Text="Location2" ID="lblBranchSearch" CssClass="styleMandatoryLabel" ToolTip="Location2" />
                                        </td>
                                        <td class="styleFieldAlign"">
                                            <asp:DropDownList ID="ComboBoxBranch" AutoPostBack="true" ValidationGroup="RFVDTransLander"
                                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                                MaxLength="0" CssClass="WindowsStyle" Width="172px" Height="23px" OnSelectedIndexChanged="ComboBoxBranch_SelectedIndexChanged" ToolTip="Location2">
                                            </asp:DropDownList>
                                            <%-- <asp:RequiredFieldValidator ID="RFVBranch" runat="server"  ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel" InitialValue="-1" Display="none"
                                ErrorMessage="Select a Branch" SetFocusOnError="true" ControlToValidate="ComboBoxBranch"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label runat="server" Text="Product Name" ID="lblProductCodeSearch" CssClass="styleMandatoryLabel" ToolTip="Product" />
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:DropDownList ID="ComboBoxProductCode" AutoPostBack="true" runat="server" AutoCompleteMode="SuggestAppend"
                                                DropDownStyle="DropDownList" MaxLength="0" CssClass="WindowsStyle" Width="172px"
                                                Height="23px" OnSelectedIndexChanged="ComboBoxProductCode_SelectedIndexChanged" ToolTip="Product">
                                            </asp:DropDownList>
                                            <%-- <asp:RequiredFieldValidator ID="RfvProductCode" runat="server"  ValidationGroup="RFVDTransLander" CssClass="styleMandatoryLabel" InitialValue="-1" Display="none"
                                ErrorMessage="Select a Product" SetFocusOnError="false" ControlToValidate="ComboBoxProductCode"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label runat="server" Text="Start Date" ID="lblStartDateSearch" CssClass="styleReqFieldLabel" ToolTip="Start Date" />
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtStartDateSearch" runat="server" Width="50%" AutoPostBack="false" ToolTip="Start Date"></asp:TextBox>
                                            <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Start Date" />
                                            <cc1:CalendarExtender ID="CalendarExtenderStartDateSearch" runat="server" Enabled="True"
                                                PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RfvStartDate" runat="server" ValidationGroup="RFVDTransLander"
                                                CssClass="styleMandatoryLabel" Display="none" ErrorMessage="Select Start Date"
                                                SetFocusOnError="True" ControlToValidate="txtStartDateSearch">RfvStartDate</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="8px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label runat="server" ID="lblEndDateSearch" Text="End Date" CssClass="styleReqFieldLabel" ToolTip="End Date" />
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtEndDateSearch" runat="server" Width="50%" AutoPostBack="false" ToolTip="End Date"></asp:TextBox>
                                            <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="End Date" />
                                            <cc1:CalendarExtender ID="CalendarExtenderEndDateSearch" runat="server" Enabled="True"
                                                PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="RfvEndDate" runat="server" ValidationGroup="RFVDTransLander"
                                                CssClass="styleMandatoryLabel" Display="none" ErrorMessage="Select End Date"
                                                SetFocusOnError="false" ControlToValidate="txtEndDateSearch" Width="5%">RfvEndDate</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </td>
            </tr>
        <tr>
                <td>
                    <table width="70%" align="center">
                        <tr align="center">
                            <td width="100%">                               
                                    <div id="divPASA" runat="server" style="height: 225px; overflow: auto; display:none">
                                        <asp:GridView ID="grvprimeaccount" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                                            BorderWidth="2" Width="100%" OnRowDataBound="grvprimeaccount_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No." ItemStyle-Width="2%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="S.No"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account No." ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMLA" runat="server" Text='<%#Eval("PRIMEACCOUNTNO")%>' ToolTip="Account No."></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Account No." ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSLA" runat="server" Text='<%#Eval("SUBACCOUNTNO")%>' ToolTip="Sub Account No."></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select All" SortExpression="Sellect All">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSelectAll" runat="server" Text="Select All" ToolTip="Select All"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="false" ToolTip="Select All" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Select" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                    </div>                             
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr>
                <td>
                    <table width="100%">
                        <tr align="center">
                            <td width="100%" colspan="2" align="center">
                                <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" OnClick="btnGo_Click"
                                    Text="Go" UseSubmitBehavior="true" ValidationGroup="RFVDTransLander" OnClientClick="return fnCheckPageValidators('RFVDTransLander',false);" CausesValidation="true" ToolTip="Go" />
                                <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" OnClick="btnClear_Click"
                                    OnClientClick="return fnConfirmClear();" Text="Clear" UseSubmitBehavior="true" ToolTip="Clear" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                            </td>
                            <td align="right">
                                <asp:Label ID="LblCurrency" runat="server" Text="" ToolTip="Currency">
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="LblErrorText" runat="server" Text="">
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr>
                <td>
                    <table width="100%" align="left">
                        <tr>
                            <td>
                                <asp:Panel ID="PnlCustomerDetails" runat="server" GroupingText="Customer Account Details"
                                    CssClass="stylePanel" Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td width="100%">
                                                <div id="divCustomerGlance" runat="server" style="position: relative; height: 200px;width: 100%; overflow: scroll;" align="left">
                                                    <asp:GridView ID="grvCustomer" runat="server" Width="100%" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:BoundField DataField="LOB" HeaderText="LOB" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="Location" HeaderText="Location" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left" ItemStyle-Width="130" />
                                                            <%--<asp:BoundField DataField="Branch" HeaderText="Location2" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%" />--%>
                                                            <%-- <asp:BoundField DataField="CustomerCode" HeaderText="Customer Name" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="left" ItemStyle-Width="100" />--%>
                                                            <asp:BoundField DataField="Product" HeaderText="Product" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="Status" HeaderText="Active" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="Primeac" HeaderText="Account No." HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="Subac" HeaderText="Sub Account No." HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="left" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="AppliedAmt" HeaderText="Applied Amt." HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="CollateralValue" HeaderText="Collateral Value"  HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="SancAmt" HeaderText="Sanctioned Amt." HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="DisbursedAmount" HeaderText="Disbursed Amt." HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="GrossExposure" HeaderText="Gross Exposure" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="NetExposure" HeaderText="Net Exposure" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="Dues" HeaderText="Dues" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="Collected" HeaderText="Collected" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                           <%-- <asp:BoundField DataField="Pending" HeaderText="Pending" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%" />--%>
                                                            <asp:BoundField DataField="AverageDueDates" HeaderText="Average Due Dates" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-HorizontalAlign="center" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="ODIDue" HeaderText="ODI Due" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="MemoDue" HeaderText="Memo Due" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="10%"  />
                                                            <asp:BoundField DataField="Others" HeaderText="Other Charges" HeaderStyle-HorizontalAlign="center"
                                                                ItemStyle-HorizontalAlign="right" ItemStyle-Width="50%"  />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>                    
                </td>
            </tr>
        <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlasset" runat="server" CssClass="stylePanel" GroupingText="Asset Details"
                                    Width="100%">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <div id="divAsset" runat="server" style="position: relative; overflow: scroll; width: 100%;
                                                    height: 200px;" align="left">
                                                    <asp:GridView ID="gvasset" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                                        ShowFooter="false" Style="margin-bottom: 0px" Width="100%" OnRowDataBound="gvasset_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="LOB">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLobid" runat="server" Text='<%# Bind("LobName") %>' ToolTip="Line of Business"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Account No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PRIMEACCOUNTNO") %>' ToolTip="Account No."></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sub Account No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SUBACCOUNTNO") %>' ToolTip="Sub Account No."></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Details">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAsset" runat="server" Text='<%# Bind("AssetDesc") %>' ToolTip="Asset Description"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SI/RegNo.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSINo" runat="server" Text='<%# Bind("RegNo") %>' ToolTip="SI.No."></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Terms">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblterms" runat="server" Text='<%# Bind("Terms") %>' ToolTip="Terms"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2" width="100%">
                                <asp:Label ID="lblAmounts" runat="server" Text="All amounts are in" Visible="false"
                                    CssClass="styleDisplayLabel"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td class="styleFieldAlign">
                                <asp:Panel ID="pnlAccountDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Account Details"
                                    Width="100%">
                                    <div id="divAccount" runat="server" style="overflow: scroll; height: 200px;">
                                        <asp:GridView ID="grvAccount" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                            ShowFooter="true" Style="margin-bottom: 0px" Width="100%" OnRowDataBound="grvAccount_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="LOB">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLobid" runat="server" Text='<%# Bind("LobName") %>' ToolTip="Line Of Business"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Account No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PRIMEACCOUNTNO") %>' ToolTip="Prime Account No."></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Account No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SUBACCOUNTNO") %>' ToolTip="Sub Account No."></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text="Grand Total:"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                    <FooterStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount financed">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("AmountFinanced") %>' ToolTip="Amount Financed"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalAmount" runat="server" ToolTip="sum of Total Amount"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                    <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                </asp:TemplateField>
                                               <%-- <asp:TemplateField HeaderText="Terms">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblterms" runat="server" Text='<%# Bind("Terms") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Yet To be billed">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblYetbilled" runat="server" Text='<%# Bind("YetToBeBilled") %>' ToolTip="Yet to be Billed"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalYetbilled" runat="server" ToolTip="sum of  Yet to be billed amount"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="right" Width="10%" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Billed">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbilled" runat="server" Text='<%# Bind("Billed") %>' ToolTip="Billed"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalbilled" runat="server" ToolTip="sum of   billed amount"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Installment Received">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblbalance" runat="server" Text='<%# Bind("Balance") %>' ToolTip="Installment Received"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalbilledbalance" runat="server" ToolTip="sum of   Balance  amount"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlTransactionDetails" runat="server" CssClass="stylePanel" GroupingText="Transaction Details"
                                    Width="100%">  
                                     <asp:Label ID="lblOpeningBalance" runat="server" ></asp:Label>
                                    <br /><br />                                 
                                    <div id="divTransaction" runat="server" style="height: 200px; overflow:scroll">
                                        <asp:GridView ID="grvtransaction" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                            Width="100%" ShowFooter="true" ShowHeader="true" OnRowDataBound="grvtransaction_RowDataBound" OnDataBound="grvtransaction_DataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Account No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PrimeAccountNo") %>' ToolTip="Account No."></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Account No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SubAccountNo") %>' ToolTip="Sub Account No."></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocDate" runat="server" Text='<%# Bind("DocumentDate") %>' ToolTip="Document Date"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Value Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblvaluedate" runat="server" Text='<%# Bind("ValueDate") %>' ToolTip="Value Date"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Doc Ref.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocumentReference" runat="server" Text='<%# Bind("DocumentReference") %>' ToolTip="Document Reference"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldesc" runat="server" Text='<%# Bind("Description") %>' ToolTip="Description"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" ToolTip="sum of  Dues amount" Text="Grand Total:"
                                                            HorizontalAlign="center"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dues">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDues" runat="server" Text='<%# Bind("Dues") %>' ToolTip="Dues"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDues" runat="server" ToolTip="sum of  Dues amount"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="center" />
                                                    <ItemStyle HorizontalAlign="right" Width="10%" />
                                                    <FooterStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Receipts">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceipts" runat="server" Text='<%# Bind("Receipts") %>' ToolTip="Receipts"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalReceipts" runat="server" ToolTip="sum of  Receipts amount"></asp:Label>
                                                    </FooterTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="center" />
                                                    <ItemStyle HorizontalAlign="right" Width="10%" />
                                                    <FooterStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Balance">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbalance" runat="server" Text='<%# Bind("Balance") %>' ToolTip="Balance"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalbalance" runat="server" ToolTip="sum of  Balance amount"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="center" />
                                    <ItemStyle HorizontalAlign="right" Width="10%" />
                                    <FooterStyle HorizontalAlign="right" />
                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        <tr>
                <td>
                    <table width="100%" align="center">
                        <tr align="center">
                            <td align="center">
                                <asp:Button ID="BtnPrint" runat="server" CssClass="styleSubmitButton" Text="Print"
                                    OnClick="BtnPrint_Click" ToolTip="Print" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="2px" border="0" style="height: 208px; width: 100%">
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="vsCustomerAtaGlance" runat="server" CssClass="styleMandatoryLabel"
                                                Enabled="true" Width="98%" ShowMessageBox="false" ValidationGroup="RFVDTransLander"
                                                HeaderText="Correct the following validation(s):  " ShowSummary="true" Height="129px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
    function fnLoadCustomer()
        {
        document.getElementById("<%= btnLoadCustomer.ClientID %>").click();

        }
        
   
    
//   function Resize()
//     {
//       if(document.getElementById('ctl00_ContentPlaceHolder1_divCustomerGlance') != null)
//       {
//         if(document.getElementById('divMenu').style.display=='none')
//            {
//             (document.getElementById('ctl00_ContentPlaceHolder1_divCustomerGlance')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
//            }
//         else
//           {
//             (document.getElementById('ctl00_ContentPlaceHolder1_divCustomerGlance')).style.width = screen.width - 270;
//           }
//        }  
//      }
    
        
    </script>
    <style type="text/css">
    .Freezing
    {
      position:relative;
      top:auto;
      z-index:auto;
    }
  </style>

</asp:Content>
