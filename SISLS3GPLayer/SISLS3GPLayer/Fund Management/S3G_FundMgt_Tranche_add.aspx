<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_FundMgt_Tranche_add.aspx.cs"
    Inherits="Fund_Management_S3G_FundMgt_Tranche_add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="~/UserControls/S3GAutoSuggest.ascx" TagName="AutoSugg" TagPrefix="UC3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>
            &nbsp;<table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
            </table>
            <cc1:TabContainer ID="tcFunder" runat="server" CssClass="styleTabPanel" Width="99%" ScrollBars="None" AutoPostBack="true" OnActiveTabChanged="tbgeneral_ActiveTabChanged">
                <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                    BackColor="Red" ToolTip="General" Width="99%" TabIndex="0">
                    <HeaderTemplate>
                        General            
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblcust" runat="server" CssClass="styleReqFieldLabel" Text="Lessee"></asp:Label>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <uc2:Suggest ID="ddlcust" runat="server" AutoPostBack="True" OnItem_Selected="ddlcust_SelectedIndexChanged"
                                                    ServiceMethod="GetCustList" IsMandatory="true" ErrorMessage="Select Lessee" ValidationGroup="vsFetch" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lbltranche" runat="server" CssClass="styleDisplayLabel" ToolTip="Tranche" Text="Tranche Name"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txttranche" runat="server" MaxLength="20" ToolTip="Tranche" onkeyup="maxlengthfortxt(20);"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txttranche" SetFocusOnError="True"
                                                    ErrorMessage="Enter the Tranche Name" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="ftbtranche" runat="server" TargetControlID="txttranche"
                                                    FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars="-()" Enabled="True">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lbltranchedate" runat="server" CssClass="styleReqFieldLabel" ToolTip="Tranche date" Text="Tranche Date"></asp:Label>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:TextBox ID="txttranchedate" runat="server" ToolTip="Tranche Date" AutoPostBack="True" OnTextChanged="txttranchedate_OnTextChanged"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtenderTranchedate" runat="server" Enabled="True"
                                                    TargetControlID="txttranchedate">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txttranchedate" SetFocusOnError="True"
                                                    ErrorMessage="Enter the Tranche Date" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblstatus" runat="server" CssClass="styleDisplayLabel" ToolTip="Status" Text="Status"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtstatus" runat="server" MaxLength="100" ReadOnly="True" ToolTip="Status" onkeyup="maxlengthfortxt(500);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label runat="server" ID="lblInvCovLetter" CssClass="styleReqFieldLabel"
                                                    Text="Invoice Covering Letter"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlInvCovLetter" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                </asp:DropDownList>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <table>
                                        <tr>
                                            <td class="styleFieldLabel" align="center" colspan="2">
                                                <asp:Button ID="btnfetch" runat="server" ValidationGroup="vsFetch" CssClass="styleSubmitButton" Text="Fetch" ToolTip="Fetch" OnClick="btnfetch_OnClick"></asp:Button>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlrs" runat="server" GroupingText="RS Details" Visible="False">
                                        <div id="divacc" runat="server" class="container" style="height: 150px; overflow-x: hidden; overflow-y: scroll; display: none">
                                            <asp:GridView ID="grvRental" runat="server" AutoGenerateColumns="False"
                                                BorderWidth="2px" EnableModelValidation="True" OnRowDataBound="grvRental_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl.No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RS Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRS_Number" runat="server" Text='<%#Eval("RS_Number")%>' ToolTip="Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PASA_id" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPASA_id" runat="server" Text='<%#Eval("PA_SA_REF_ID")%>' ToolTip="Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RS Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRsType" runat="server" Text='<%#Eval("RS_Type")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Duedate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblduedat" runat="server" Text='<%#Eval("dayss")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tenure">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltenure" runat="server" Text='<%#Eval("tenure")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Frequency">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfrequency" runat="server" Text='<%#Eval("frequency")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRsstatus" runat="server" Text='<%#Eval("RS_Status")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnShow" ToolTip="Show" runat="server" ImageUrl="~/Images/Show.JPG" OnClick="imgbtnShow_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rental" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRental" runat="server" Text='<%#Eval("rental")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="AMF" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAMF" runat="server" Text='<%#Eval("AMF")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VAT/CST" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVAT" runat="server" Text='<%#Eval("VAT")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sevice Tax" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSTax" runat="server" Text='<%#Eval("ServiceTax")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select" SortExpression="Select">
                                                        <HeaderTemplate>
                                                            Select All
                                                            <asp:CheckBox ID="chkAll" runat="server" ToolTip="Select All" AutoPostBack="true" OnCheckedChanged="chkAll_OnCheckedChanged" CausesValidation="false" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Select Account" AutoPostBack="true" OnCheckedChanged="chkSelectAccount_OnCheckedChanged" CausesValidation="false" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnValidateRS" Text="Validate RS" runat="server" Enabled="False" CssClass="styleSubmitButton" OnClick="btnValidateRS_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlcashflow" runat="server" GroupingText="Cash Flow Details" Width="100%" Visible="False" CssClass="stylePanel">
                                        <div id="div1" runat="server" style="height: 150px; overflow: auto; display: none">
                                            <asp:GridView ID="gvOutFlow" runat="server" AutoGenerateColumns="False"
                                                ShowFooter="True" BorderWidth="2px" EnableModelValidation="True" OnRowDataBound="gvOutFlow_RowDataBound" Width="100%" OnRowCommand="gvOutFlow_RowCommand" OnRowDeleting="gvOutFlow_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtDate_GridOutflow" Width="100px" runat="server">
                                                            </asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtenderSD_OutflowDate" runat="server" Enabled="True"
                                                                TargetControlID="txtDate_GridOutflow">
                                                            </cc1:CalendarExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtDate_GridOutflow" runat="server" ControlToValidate="txtDate_GridOutflow"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Date in Outflow"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate_GridOutflow" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cash flow Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltype" runat="server" Text='<%# Bind("CashFlow_Type") %>'></asp:Label>
                                                            <asp:Label ID="lblid" runat="server" Visible="false" Text='<%# Bind("CashFlow_Type_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddloutflowtype" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvddloutflowtype" runat="server" ControlToValidate="ddloutflowtype"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True" InitialValue="0"
                                                                ErrorMessage="Select Cashflow Type" Enabled="true"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cash flow Description">
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlOutflowDesc" runat="server" Width="99%"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvddlOutflowDesc" runat="server" ControlToValidate="ddlOutflowDesc"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                InitialValue="0" ErrorMessage="Select a Cash flow Description in Outflow"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOutflowDesc" runat="server" Text='<%# Bind("CashOutFlow") %>'></asp:Label>
                                                            <asp:Label ID="lblcahflowdesc" runat="server" Visible="false" Text='<%# Bind("CashOutFlow_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment to">
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlPaymentto_OutFlow" runat="server" Width="99%" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvddlPaymentto_OutFlow" runat="server" ControlToValidate="ddlPaymentto_OutFlow"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                InitialValue="0" ErrorMessage="Select Payment to in Outflow"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPaymentto" runat="server" Text='<%# Bind("OutflowFrom") %>'></asp:Label>
                                                            <asp:Label ID="lbloutflowid" runat="server" Visible="false" Text='<%# Bind("OutflowFrom_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Entity Name">
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="ddlEntityName_OutFlow" runat="server" ServiceMethod="GetVendors"
                                                                ErrorMessage="Select a Customer/Entity Name in Outflow" Width="250px"
                                                                ValidationGroup="Outflow" IsMandatory="true" />
                                                        </FooterTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEntityName_OutFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                            <asp:Label ID="Label2" runat="server" Visible="false" Text='<%# Bind("Entity_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAmount_Outflow" runat="server" MaxLength="10" ToolTip="OutFlow Amount" Style="text-align: right">
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Outflow" runat="server" FilterType="Numbers"
                                                                TargetControlID="txtAmount_Outflow">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtAmount_Outflow" runat="server" ControlToValidate="txtAmount_Outflow"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Amount in Outflow" Enabled="true"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount_Outflow" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnAddOut" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Outflow" CommandName="Add"
                                                                CssClass="styleGridShortButton" />
                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                Text="Remove"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" HeaderText="Tranche-Modify" ID="TabPanel1" CssClass="tabpan"
                    BackColor="Red" ToolTip="General" Width="99%" TabIndex="1" Enabled="false">
                    <HeaderTemplate>
                        Tranche-Modify            
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblconsolidated" CssClass="styleDisplayLabel" runat="server" Text="Consolidated Entry"></asp:Label>
                                    <asp:CheckBox ID="chkconsolidated" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="chkconsolidated_OnCheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnltotal" runat="server" GroupingText="Total" CssClass="stylePanel">
                                        <table width="100%">
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="Label1" Text="" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="Label7" Text="Rental" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="Label3" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="Label8" Text="VAT/CST/GST" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="Label4" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="Label9" Text="AMF" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="Label5" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:Label ID="Label10" Text="Service Tax" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="Label6" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lblTotalRs" Text="Total RS Value" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtLR" runat="server" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lbll1" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtTax" runat="server" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lbll2" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtAMF" runat="server" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lbll3" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtST" runat="server" ReadOnly="true" Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lbll4" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="20%">
                                                    <asp:Label ID="lbltotdisc" CssClass="styleDisplayLabel" Text="Total Disc Value" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtLRdisc" runat="server" AutoPostBack="true" OnTextChanged="txtLRdisc_OnTextChanged"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars="."
                                                        FilterType="Numbers,Custom" Enabled="True"
                                                        TargetControlID="txtLRdisc">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lblLRper" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtTaxdisc" runat="server" AutoPostBack="true" OnTextChanged="txtTaxdisc_OnTextChanged"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" ValidChars="."
                                                        FilterType="Numbers,Custom" Enabled="True"
                                                        TargetControlID="txtTaxdisc">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lblTaxper" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtAMFdisc" runat="server" AutoPostBack="true" OnTextChanged="txtAMFdisc_OnTextChanged"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" ValidChars="."
                                                        FilterType="Numbers,Custom" Enabled="True"
                                                        TargetControlID="txtAMFdisc">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lblAMFPer" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                                <td width="15%">
                                                    <asp:TextBox ID="txtSerDisc" runat="server" AutoPostBack="true" OnTextChanged="txtSerDisc_OnTextChanged"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" ValidChars="."
                                                        FilterType="Numbers,Custom" Enabled="True"
                                                        TargetControlID="txtSerDisc">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td width="5%">
                                                    <asp:Label ID="lblserdisc" CssClass="styleDisplayLabel" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="hdnlrper" runat="server" />
                                                    <asp:HiddenField ID="hdnvatper" runat="server" />
                                                    <asp:HiddenField ID="hdnamfper" runat="server" />
                                                    <asp:HiddenField ID="hdnserper" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btnProcess" Text="Process" runat="server" CssClass="styleSubmitButton" OnClick="btnProcess_OnClick" />
                                    <asp:TextBox ID="txtcheck" Text="0" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlRswisebreakup" runat="server" GroupingText="RS Wise Breakup" CssClass="styleDisplayLabel" Visible="false">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" width="8%">
                                                    <asp:Label ID="lblRs" runat="server" CssClass="styleReqFieldLabel" Text="Rental Schedule Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" width="20%">
                                                    <asp:DropDownList ID="ddllRSnumber" runat="server" AutoPostBack="true"
                                                        ToolTip="Rental Schedule Number" OnSelectedIndexChanged="ddllRSnumber_OnSelectedIndexChanged">
                                                    </asp:DropDownList>

                                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CssClass="styleSubmitButton" OnClick="btnUpdate_Click" />


                                                    <%-- <asp:DropDownList ID="ddlfloat" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddllRSnumber_OnSelectedIndexChanged"></asp:DropDownList>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div id="divrswise" runat="server" class="container" style="height: 250px; overflow-x: hidden; overflow-y: scroll; display: none">
                                                        <asp:GridView ID="grvrswise" runat="server" AutoGenerateColumns="False"
                                                            BorderWidth="2px" Width="100%" ShowFooter="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="2%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RS Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRS_Number" runat="server" Text='<%#Eval("RS_Number")%>' ToolTip="Sub Account No"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PA_SA_REF_ID" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPA_SA_REF_ID" runat="server" Text='<%#Eval("PA_SA_REF_ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RS_Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRS_Type" runat="server" Text='<%#Eval("RS_Type")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Installment No">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblinstallmentno" runat="server" Text='<%#Eval("Installment_No")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rental">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblrental" runat="server" Text='<%#Eval("Rental_source")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="Totlblrental" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discounted Rental">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblrentaldisc" Style="text-align: right" runat="server" Text='<%#Eval("Rental_disc")%>' onblur="validate(this,'A');"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" ValidChars="."
                                                                            FilterType="Numbers,Custom" Enabled="True"
                                                                            TargetControlID="lblrentaldisc">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="Totlblrentaldisc" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VAT/CST/GST">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVAT" runat="server" Text='<%#Eval("Rental_Tax_source")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotllblVAT" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discounted VAT/CST/GST">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblVATdisc" Style="text-align: right" runat="server" onblur="validate(this,'B');" Text='<%#Eval("Rental_Tax_disc")%>'></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" ValidChars="."
                                                                            FilterType="Numbers,Custom" Enabled="True"
                                                                            TargetControlID="lblVATdisc">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblVATdisc" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="AMF">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAMF" runat="server" Text='<%#Eval("AMF_source")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblAMF" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discounted AMF">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblAMFdisc" Style="text-align: right" runat="server" onblur="validate(this,'C');" Text='<%#Eval("AMF_disc")%>'></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" ValidChars="."
                                                                            FilterType="Numbers,Custom" Enabled="True"
                                                                            TargetControlID="lblAMFdisc">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblAMFdisc" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Service Tax">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblServiceTax" runat="server" Text='<%#Eval("Service_Tax_source")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblServiceTax" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discounted Service Tax">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="lblServiceTaxdisc" Style="text-align: right" runat="server" onblur="validate(this,'D');" Text='<%#Eval("Service_Tax_disc")%>'></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" ValidChars="."
                                                                            FilterType="Numbers,Custom" Enabled="True"
                                                                            TargetControlID="lblServiceTaxdisc">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblServiceTaxdisc" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" HeaderText="Funder" ID="TabPanel2" CssClass="tabpan"
                    BackColor="Red" ToolTip="General" Width="99%" TabIndex="2" Enabled="false">
                    <HeaderTemplate>
                        Funder            
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblFunder" runat="server" Text="Funder"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<uc2:Suggest ID="txtfunder"  AutoPostBack="true" ServiceMethod="GetVendors" runat="server" ToolTip="Funder" />--%>

                                                <asp:TextBox ID="txtfunder" runat="server" MaxLength="50" OnTextChanged="txtfunder_OnTextChanged"
                                                    AutoPostBack="true" Width="182px"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="aceRSNumber" MinimumPrefixLength="1" OnClientPopulated="RS_ItemPopulated" OnClientItemSelected="RS_ItemSelected"
                                                    runat="server" TargetControlID="txtfunder" ServiceMethod="GetVendors"
                                                    CompletionSetCount="5" Enabled="True" ServicePath="" CompletionListCssClass="CompletionList"
                                                    DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="txtWatermarkExtender1" runat="server" TargetControlID="txtfunder"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnfunder" runat="server" />

                                            </td>
                                            <td>
                                                <asp:Label ID="lbldisbursement" runat="server" Visible="false" Text="Disbursement Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtdisbursementdate" runat="server" Visible="false" ToolTip="Disbursement Date" />
                                                <cc1:CalendarExtender ID="caldisb" runat="server" Enabled="false"
                                                    TargetControlID="txtdisbursementdate">
                                                </cc1:CalendarExtender>
                                                <%--   <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtdisbursementdate" SetFocusOnError="True"
                                ErrorMessage="Enter the Disbursement Date" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave"></asp:RequiredFieldValidator>--%>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblTenure" runat="server" Text="Tenure"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTenure" runat="server" ReadOnly="true" ToolTip="Tenure" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr align="center">

                                <td>
                                    <table>
                                        <tr>
                                            <td class="styleFieldLabel" align="center" colspan="2">
                                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_OnClick" ToolTip="Go" CssClass="styleSubmitButton" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="50%">
                                                <asp:Panel ID="pnlasset" runat="server" GroupingText="Asset Category Details" CssClass="styleDisplayLabel" Visible="false">
                                                    <div id="divasset" runat="server" style="height: 150px; overflow: auto; display: none">
                                                        <asp:GridView ID="grvasset" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                                            BorderWidth="2px" EnableModelValidation="True" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="2%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Category">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblassetcategory" runat="server" Text='<%#Eval("asset_category")%>' ToolTip="Sub Account No"></asp:Label>
                                                                        <asp:Label ID="lblassetid" runat="server" Text='<%#Eval("asset_id")%>' ToolTip="Sub Account No" Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rental">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblrentalsource" runat="server" Text='<%#Eval("Rental")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="Totlblrentalsource" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="VAT/CST">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblVAT" runat="server" Text='<%#Eval("VAT")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblVAT" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="AMF">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAMF" runat="server" Text='<%#Eval("AMF")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblAMF" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Service Tax">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblServiceTax" runat="server" Text='<%#Eval("ServiceTax")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="TotlblServiceTax" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                            <td width="50%">
                                                <asp:Panel ID="pnlfunder" runat="server" GroupingText="Sanction Details" CssClass="styleDisplayLabel">
                                                    <div id="divfund" runat="server" style="height: 150px; overflow: scroll; display: none">
                                                        <asp:GridView ID="grvfund" runat="server" AutoGenerateColumns="False"
                                                            BorderWidth="2px" EnableModelValidation="True" Width="100%" OnRowDataBound="grvfund_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="2%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Category">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblassetcategory" runat="server" Text='<%#Eval("asset_category")%>' ToolTip="Sub Account No"></asp:Label>
                                                                        <asp:Label ID="lblassetid" runat="server" Text='<%#Eval("asset_category_id")%>' ToolTip="Sub Account No" Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sanctioned Id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSanctionid" runat="server" Text='<%#Eval("Sanction_id")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sanctioned Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSanctionumber" runat="server" Text='<%#Eval("Sanctioned_No")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sanctioned Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSanctiondate" runat="server" Text='<%#Eval("Sanctioned_Date")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Expiry Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblexpirydate" runat="server" Text='<%#Eval("expiry_date")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Available limit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblavailablelimit" runat="server" Text='<%#Eval("available_limit")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sanctioned limit">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblsanctionedlimit" runat="server" Text='<%#Eval("Sanctioned_limit")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Discount Rate">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDiscountRate" runat="server" Text='<%#Eval("Discount_Rate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Fore Closure Rate">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblforeclosurerate" runat="server" Text='<%#Eval("Foreclosure_Rate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select" SortExpression="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Select Account" AutoPostBack="true" OnCheckedChanged="chkAccount_OnCheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btnvalidate" Text="Validate" Enabled="false" runat="server" OnClick="btnvalidate_Click" CssClass="styleSubmitButton" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td colspan="2">
                                                <table width="50%" align="center">
                                                    <tr>
                                                        <td align="center" width="50%">
                                                            <asp:Panel ID="pnluntagged" runat="server" GroupingText="Funder Limit Shortfall Details" CssClass="styleDisplayLabel" Visible="false">
                                                                <div id="divuntagged" runat="server" style="height: 150px; overflow: auto; display: none">
                                                                    <asp:GridView ID="grvuntagged" runat="server" AutoGenerateColumns="False"
                                                                        BorderWidth="2px" EnableModelValidation="True" Width="100%">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Sl.No.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="2%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Rental Schedule">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblarsnum" runat="server" Text='<%#Eval("RS_Number")%>' ToolTip="Sub Account No"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Rental Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblrental" runat="server" Text='<%#Eval("rental")%>' ToolTip="Sub Account No"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Tax">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblvat" runat="server" Text='<%#Eval("vat")%>' ToolTip="Sub Account No"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="AMF">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblamf" runat="server" Text='<%#Eval("AMF")%>' ToolTip="Sub Account No"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Service Tax">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblsevicetax" runat="server" Text='<%#Eval("servicetax")%>' ToolTip="Sub Account No"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="right" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle HorizontalAlign="Center" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="TabAlerts" CssClass="tabpan" HeaderText="TabAlerts"
                    BackColor="Red" Width="98%" Enabled="false">
                    <HeaderTemplate>
                        Alerts
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <table width="99%">
                                    <tr valign="middle">
                                        <td style="padding-top: 10px; width: 100%">
                                            <asp:Panel runat="server" ID="Panel5" CssClass="stylePanel" GroupingText="Alert Details"
                                                Width="100%">
                                                <asp:GridView ID="gvAlert" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                    OnRowDataBound="gvAlert_RowDataBound" OnRowDeleting="gvAlert_RowDeleting" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="TypeId" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTypeId" runat="server" Text='<%# Bind("TypeId") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlType_AlertTab" runat="server" Width="200px">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvAlertType" runat="server" ControlToValidate="ddlType_AlertTab"
                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Alert" InitialValue="-1"
                                                                    SetFocusOnError="True" ErrorMessage="Select a Type"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User ContactId" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserContactid" runat="server" Text='<%# Bind("UserContactId") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User Contact" FooterStyle-Width="45%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserContact" runat="server" Text='<%# Bind("UserContact") %>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <%-- <asp:DropDownList ID="ddlContact_AlertTab" runat="server" Width="99%">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvUserContact" runat="server" ControlToValidate="ddlContact_AlertTab"
                                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Select a User Contact"
                                                                                    InitialValue="-1" ValidationGroup="Alert"></asp:RequiredFieldValidator>--%>
                                                                <UC3:AutoSugg ID="ddlContact_AlertTab" runat="server" Width="300px" ServiceMethod="GetSalePersonList"
                                                                    ErrorMessage="Select a User Contact" IsMandatory="true" ValidationGroup="Alert" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EMail">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkEmail" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "EMail")))%>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:CheckBox ID="ChkEmail" runat="server"></asp:CheckBox>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="SMS">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkSMS" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "SMS")))%>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:CheckBox ID="ChkSMS" runat="server"></asp:CheckBox>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                    Text="Remove"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAddAlert" runat="server" Text="Add" CausesValidation="true" CssClass="styleGridShortButton"
                                                                    OnClick="Alert_AddRow_OnClick" ValidationGroup="Alert"></asp:Button>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>

                                <table width="99%">
                                    <tr valign="middle">
                                        <td style="padding-top: 10px; width: 100%">
                                            <asp:Panel runat="server" ID="pnlCustEmail" CssClass="stylePanel" GroupingText="Billing Email Alert Details"
                                                Width="100%">
                                                <asp:GridView ID="grvCustEmail" runat="server" AutoGenerateColumns="False"
                                                    OnRowDataBound="grvCustEmail_RowDataBound"  Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="TypeId" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustEmailDetID" runat="server" Text='<%# Bind("Cust_Email_Det_ID") %>' />
                                                        v    </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Group Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGroupName" runat="server" Text='<%# Bind("Group_Name") %>' />
                                                            </ItemTemplate>
                                                            
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="EMail ID" FooterStyle-Width="45%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEMailID" runat="server" Text='<%# Bind("EMail_ID") %>' />
                                                            </ItemTemplate>
                                                           </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Billing Email Alert">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail_Alert" runat="server" Visible="false" Text='<%# Bind("Email_Alert") %>' />
                                                                <asp:CheckBox ID="chkEmailAlert" runat="server" />
                                                            </ItemTemplate>
                                                           <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                       
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="TabFollowUp" BackColor="Red" CssClass="tabpan" Enabled="false">
                    <HeaderTemplate>
                        Follow Up
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>

                                <asp:Panel runat="server" ID="Panel7" CssClass="stylePanel" GroupingText="Follow Up Details"
                                    Width="100%">
                                    <div id="DivFollow" runat="server">
                                        <asp:GridView ID="gvFollowUp" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                            OnRowDeleting="gvFollowUp_RowDeleting" OnRowCreated="gvFollowUp_RowCreated" Width="99%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate_GridFollowup" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDate_GridFollowup" runat="server">
                                                        </asp:TextBox>
                                                        <cc1:CalendarExtender runat="server" TargetControlID="txtDate_GridFollowup" ID="CalendarExtenderSD_FollowupDate"
                                                            Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvtxtDate_GridFollowup" runat="server" ControlToValidate="txtDate_GridFollowup"
                                                            ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                            ErrorMessage="Select the Date"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From User" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfrom_GridFollowup_ID" runat="server" Text='<% #Bind("FromUserId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From UserName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfrom_GridFollowup" runat="server" Text='<% #Bind("From")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <%--                                                                    <asp:DropDownList ID="ddlfrom_GridFollowup" runat="server" Width="180px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvFromUser" runat="server" ControlToValidate="ddlfrom_GridFollowup"
                                                                        InitialValue="-1" ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None"
                                                                        SetFocusOnError="True" ErrorMessage="Select a From UserName"></asp:RequiredFieldValidator>--%>
                                                        <UC3:AutoSugg ID="ddlfrom_GridFollowup" runat="server" Width="180px" ServiceMethod="GetSalePersonList"
                                                            ErrorMessage="Select a From UserName" IsMandatory="true" ValidationGroup="FollowUp" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To User" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTo_GridFollowupid" runat="server" Text='<%#Bind("ToUserId")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To UserName">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTo_GridFollowup" runat="server" Text='<%#Bind("To")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <%--  <asp:DropDownList ID="ddlTo_GridFollowup" runat="server" Width="180px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvToUser" runat="server" ControlToValidate="ddlTo_GridFollowup"
                                                                        ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                        InitialValue="-1" ErrorMessage="Select a To UserName"></asp:RequiredFieldValidator>--%>
                                                        <UC3:AutoSugg ID="ddlTo_GridFollowup" runat="server" Width="180px" ServiceMethod="GetSalePersonList"
                                                            ErrorMessage="Select a To UserName" IsMandatory="true" ValidationGroup="FollowUp" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionDetails" runat="server" Text='<%#Bind("Action")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtAction_GridFollowup" runat="server" MaxLength="80" onkeyup="maxlengthfortxt(80)"
                                                            TextMode="MultiLine">
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvtxtAction_GridFollowup" runat="server" ControlToValidate="txtAction_GridFollowup"
                                                            ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                            ErrorMessage="Enter the Action"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActionDate" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ActionDate")).ToString(strDateFormat) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtActionDate_GridFollowup" runat="server" Width="90px">
                                                        </asp:TextBox>
                                                        <cc1:CalendarExtender runat="server" TargetControlID="txtActionDate_GridFollowup"
                                                            ID="CalendarExtenderSD_FollowupActionDate" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvtxtActionDate_GridFollowup" runat="server" ControlToValidate="txtActionDate_GridFollowup"
                                                            ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                            ErrorMessage="Select the Action Date"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Response">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerResponse" runat="server" Text='<%#Bind("CustomerResponse")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCustomerResponse_GridFollowup" runat="server" MaxLength="80"
                                                            TextMode="MultiLine" onkeyup="maxlengthfortxt(80)">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtRemarks_GridFollowup" runat="server" MaxLength="80" TextMode="MultiLine"
                                                            onkeyup="maxlengthfortxt(80)">
                                                        </asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                            Text="Remove"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAddFollowup" runat="server" Text="Add" CausesValidation="true"
                                                            CssClass="styleSubmitButton" OnClick="FollowUp_AddRow_OnClick" ValidationGroup="FollowUp"></asp:Button>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
            <table width="100%">
                <tr class="styleButtonArea" align="center">
                    <td align="center" colspan="2">
                        <asp:HiddenField ID="hdnValidateSanction" runat="server" />
                        <asp:Button runat="server" ID="btnSave" Enabled="false" ValidationGroup="vsSave" ToolTip="Save" OnClientClick="return fnCheckPageValidators('vsSave');"
                            CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" ToolTip="Clear" OnClick="btnClear_Click"
                            Text="Clear" OnClientClick="return confirm('Do you want to clear this record?');" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" ToolTip="Cancel"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        <asp:Button runat="server" ID="btnPrint" Text="Print" CausesValidation="false" ToolTip="Print"
                            CssClass="styleSubmitButton" OnClick="btnPrint_Click" />
                        <asp:Button runat="server" ID="btprintExcel" Text="Annexure with Amount" ToolTip="Print Annexure with Amount"
                            CssClass="styleSubmitLongButton" OnClick="btprintExcel_Click" />
                        <asp:Button runat="server" ID="btnprintwoa" Text="Annexure without Amount" ToolTip="Print Annexure without Amount"
                            CssClass="styleSubmitLongButton" OnClick="btnprintwoa_Click" />
                        <asp:Button runat="server" ID="btnExcel" Text="Excel" ToolTip="Excel"
                            CssClass="styleSubmitButton" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                        <asp:TextBox ID="txtchk" runat="server" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsSave" ValidationGroup="vsSave"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" ValidationGroup="Outflow"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="vsFetch" ValidationGroup="vsFetch"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                        <asp:ValidationSummary ID="vsAlert" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" ValidationGroup="Alert" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
                            ShowSummary="true" />
                        <asp:ValidationSummary ID="vsFollowUp" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" ValidationGroup="FollowUp" Width="98%" ShowMessageBox="false"
                            HeaderText="Correct the following validation(s):  " ShowSummary="true" />
                        <asp:CustomValidator ID="cvTranche" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
            <cc1:ModalPopupExtender ID="moeRS" runat="server" TargetControlID="btnModal" PopupControlID="pnlRSAdditioInfo"
                BackgroundCssClass="styleModalBackground" Enabled="true" />
            <asp:Panel ID="pnlRSAdditioInfo" Style="display: none; vertical-align: middle" runat="server"
                BorderStyle="Solid" BackColor="White" Width="80%" ScrollBars="Auto">
                <div id="divAdditional" runat="server" style="max-height: 500px; overflow: auto">
                    <asp:UpdatePanel ID="upPass" runat="server">
                        <ContentTemplate>
                            <table width="95%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grvpopup" runat="server" AutoGenerateColumns="False"
                                            BorderWidth="2px" EnableModelValidation="True" ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Due Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDueDate" runat="server" Text='<%#Eval("Due_Date")%>' ToolTip="Due Date"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblrental" runat="server" Text='<%#Eval("Rental")%>' ToolTip="Rental"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotrental" runat="server" ToolTip="Total Rental"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="AMF">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAMF" runat="server" Text='<%#Eval("AMF")%>' ToolTip="Sub Account No"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotAMF" runat="server" ToolTip="Total AMF"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT/CST">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVAT" runat="server" Text='<%#Eval("VAT")%>' ToolTip="Sub Account No"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotVAT" runat="server" ToolTip="Total VAT"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Service Tax">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblServicetax" runat="server" Text='<%#Eval("ServiceTax")%>' ToolTip="Sub Account No"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lbltotSTAX" runat="server" ToolTip="Total Service Tax"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="center">

                                        <asp:Button runat="server" ID="btnLesseeClose" CssClass="styleSubmitButton" Text="Close" Width="60px" OnClick="btnLesseeClose_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:Button ID="btnModal" Style="display: none" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btprintExcel" />
            <asp:PostBackTrigger ControlID="btnprintwoa" />
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function CheckOne(CheckboxClientId) {
            var Checkbox = document.getElementById(CheckboxClientId);
            if (Checkbox != null) {
                var CheckBoxInputs = Checkbox.getElementsByTagName("input");
                for (var i = 0; i < CheckBoxInputs.length; i++) {
                    var CurrentIndex;
                    if (CheckBoxInputs[i].checked) {
                        CurrentIndex = i;
                    }
                    if (CheckBoxInputs[i].checked) {
                        CheckBoxInputs[i].checked = false;
                    }
                    CheckBoxInputs[parseInt(CurrentIndex)].checked = true;
                }
            }
        }

        function validate(txt, option) {

            var txt1 = document.getElementById('<%=txtcheck.ClientID %>');
            txt1.value = "";

            //document.getElementById('ctl00_ContentPlaceHolder1_tcFunder_TabPanel1_txtcheck').value = vartodayformat;
            var row = txt.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            if (txt.value == "")
                txt.value = 0;
            var Disc_value = parseFloat(txt.value);
            switch (option) {
                case 'A':
                    var source_value = (parseFloat(row.cells[4].innerText));
                    break;
                case 'B':
                    var source_value = (parseFloat(row.cells[6].innerText));
                    break;
                case 'C':
                    var source_value = (parseFloat(row.cells[8].innerText));
                    break;
                case 'D':
                    var source_value = (parseFloat(row.cells[10].innerText));
                    break;

            }

            if (Disc_value > source_value) {
                alert('Discounted Value should not be greater than Source Value');
                txt.value = source_value.toFixed(2);
            }


        }

        function ConfirmOnDelete() {
            if (confirm("Existing schedule will get Affected") == true)
                return true;
            else
                return false;
        }

        function RS_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnfunder.ClientID %>');
            hdnLessee.value = e.get_value();
        }

        function RS_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnfunder.ClientID %>');
            hdnLessee.value = '';
        }


    </script>
</asp:Content>

