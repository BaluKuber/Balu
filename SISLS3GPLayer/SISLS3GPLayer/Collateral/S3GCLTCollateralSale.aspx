<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="~/Collateral/S3GCLTCollateralSale.aspx.cs" Inherits="Collateral_S3GCLTCollateralSale" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" style="width: 100%">
                        <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Panel ID="PnlCollateralInfo" runat="server" CssClass="stylePanel" GroupingText="Collateral Information"
                            Width="50%">
                            <table border="0" width="95%">
                                <tr>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblCollateralSaleNo" runat="server" CssClass="styleDisplayLabel" Text="Collateral Sale Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCollateralSaleNo" runat="server" ContentEditable="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblCollateralSaleDate" runat="server" CssClass="styleDisplayLabel"
                                            Text="Collateral Sale Date"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCollateralSaleDate" runat="server" Contenteditable="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="LblColTranNo" runat="server" CssClass="styleDisplayLabel" Text="Collateral Transaction Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="TxtColTranNo" runat="server" Contenteditable="false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcCollateralSale" runat="server" CssClass="styleTabPanel" Width="100%"
                            TabStripPlacement="top" ActiveTabIndex="2" OnClientActiveTabChanged="ActiveTabChange">
                            <cc1:TabPanel runat="server" HeaderText="Customer / Agent" ID="tpColCustAgnt" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Customer / Agent</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upColCustAgnt" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table align="center" width="100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:RadioButton ID="rbtCustomer" runat="server" AutoPostBack="True" Checked="True"
                                                            class="styleFieldLabel" OnCheckedChanged="GetCust" Text="Customer" /><asp:RadioButton
                                                                ID="rbtAgent" runat="server" AutoPostBack="True" class="styleFieldLabel" OnCheckedChanged="GetAgent"
                                                                Text="Collection Agent" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label></HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnQuery" runat="server" CommandArgument='<%# Bind("Collateral_Capture_ID") %>'
                                                                            CommandName="Query" CssClass="styleGridQuery" ImageUrl="~/Images/spacer.gif" /></ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Modify" SortExpression="Customer_ID" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnEdit" runat="server" AlternateText="Modify" CommandArgument='<%# Bind("Collateral_Capture_ID") %>'
                                                                            CommandName="Modify" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif" /></ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblModify" runat="server" CssClass="styleGridHeader" Text="Modify"></asp:Label></HeaderTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <HeaderTemplate>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr align="center">
                                                                                <td>
                                                                                    <asp:LinkButton ID="lnkbtnSort1" runat="server" CssClass="styleGridHeader" OnClick="FunProSortingColumn"
                                                                                        Text="Name" ToolTip="Sort By Customer Name"> </asp:LinkButton><asp:ImageButton ID="imgbtnSort1"
                                                                                            runat="server" CssClass="styleImageSortingAsc" ImageAlign="Middle" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left">
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHeaderSearch1" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label><asp:HiddenField
                                                                            ID="hidCA_ID" runat="server" Value='<%# Eval("CA_ID")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Collateral Capture Number">
                                                                    <HeaderTemplate>
                                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                                            <tr align="center">
                                                                                <td>
                                                                                    <asp:LinkButton ID="lnkbtnSort2" runat="server" CssClass="styleGridHeader" OnClick="FunProSortingColumn"
                                                                                        Text="Collateral Capture Number" ToolTip="Sort By Collateral Capture Number"> </asp:LinkButton><asp:ImageButton
                                                                                            ID="imgbtnSort2" runat="server" CssClass="styleImageSortingAsc" ImageAlign="Middle" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr align="left">
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHeaderSearch2" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCType_No" runat="server" Text='<%# Bind("Collateral_Tran_No") %>'></asp:Label></ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Captured Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Eval("Captured_Date")%>'></asp:Label></ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkIsActive" runat="server" AutoPostBack="true" OnCheckedChanged="Move" /><asp:HiddenField
                                                                            ID="hidCLT_ID" runat="server" Value='<%# Eval("Collateral_Capture_ID")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUserID" runat="server" Text='<%#Eval("Created_By")%>' Visible="false"></asp:Label><asp:Label
                                                                            ID="lblUserLevelID" runat="server" Text='<%#Eval("User_Level_ID")%>' Visible="false"></asp:Label></ItemTemplate>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <input type="hidden" id="hdnSortDirection" runat="server">
                                                        <input id="hdnSortExpression" runat="server" type="hidden"></input>
                                                        <input id="hdnSearch" runat="server" type="hidden"></input>
                                                        <input id="hdnOrderBy" runat="server" type="hidden"></input>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <uc3:PageNavigator ID="ucCustomPaging" runat="server"></uc3:PageNavigator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="General" ID="tpColGeneral" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    General</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdColGen" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlHeader" Width="99%" runat="server" GroupingText="Customer Details"
                                                            CssClass="stylePanel">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" ActiveViewIndex="1" runat="server"
                                                                            FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlAgent" Width="99%" runat="server" GroupingText="Collection Agent Details"
                                                            CssClass="stylePanel">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label1" runat="server" Text="Collection Agent Code" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtACode" runat="server" />
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblAName" runat="server" Text="Collection Agent Name" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAName" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label2" runat="server" Text="Collection Agent Address" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAaddress" TextMode="MultiLine" runat="server" />
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label3" runat="server" Text="City" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAcity" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label4" runat="server" Text="State" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtAState" runat="server" />
                                                                    </td>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label5" runat="server" Text="Country" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtACountry" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlAccDetails" Width="99%" runat="server" GroupingText="Account Details"
                                                            Visible="False" Height="10%" ScrollBars="Vertical" CssClass="stylePanel">
                                                            <asp:GridView ID="gvAccDetails" runat="server" AutoGenerateColumns="False" Width="99%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sl No">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSNoMedium" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Line of Business" DataField="LOB_Code" />
                                                                    <asp:BoundField HeaderText="Prime Account Number" DataField="Prime_Account_Number" />
                                                                    <asp:BoundField HeaderText="Sub Account Number" DataField="Sub_Account_Number" />
                                                                    <asp:BoundField HeaderText="Account Creation Date" DataField="Account_Date" />
                                                                    <asp:BoundField HeaderText="Finance Amount" DataField="Original_Amount_Financed">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField HeaderText="Maturity Date" DataField="Maturity_Date" />
                                                                </Columns>
                                                                <RowStyle HorizontalAlign="Left" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Collateral Details" ID="tpColDtl" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Collateral Details</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdColDtl" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="styleFieldAlign">
                                                        <asp:CheckBox ID="ChkHS" runat="server" OnCheckedChanged="ChkHS_CheckedChanged" Text="High Liquid Security Details"
                                                            AutoPostBack="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldAlign">
                                                        <asp:GridView ID="gvHighLiqDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="True" OnDataBound="gvHighLiqDetails_DataBound">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Serial No" DataField="SLNo" />
                                                                <asp:BoundField HeaderText="Issued By" DataField="Issue_By" />
                                                                <asp:BoundField HeaderText="Acquisition Date" DataField="Maturity_Date" />
                                                                <asp:BoundField HeaderText="Maturity Date" DataField="Maturity_Date" />
                                                                <asp:BoundField HeaderText="Face Value" DataField="Unit_Face_Value" />
                                                                <asp:TemplateField HeaderText="Available Units">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblAvlUnits" runat="server" Style="text-align: right" Text='<% #Bind("No_Of_Units") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblAvlUnits" runat="server" Style="text-align: right" Text='<% #Bind("No_Of_Units") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Current Unit Value" DataField="CURRENT_UNIT_VALUE" />
                                                                <asp:TemplateField HeaderText="Current Market Value">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Style="text-align: right" Text='<% #Bind("Valuation_Current_Market_Value") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Style="text-align: right" Text='<% #Bind("Valuation_Current_Market_Value") %>'></asp:Label></AlternatingItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LblTotal" runat="server" Text="Grand Total"></asp:Label></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Unit">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtHigSaleUnit" MaxLength="10" runat="server" Style="text-align: right"
                                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" OnTextChanged="txtHigSaleUnit_TextChanged"
                                                                            AutoPostBack="True" /><cc1:FilteredTextBoxExtender ID="FtxtHigSaleUnit" runat="server"
                                                                                Enabled="true" FilterType="Numbers,Custom" TargetControlID="txtHigSaleUnit" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalUnits" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Amount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtHigAmount" MaxLength="10" runat="server" Style="text-align: right"
                                                                            AutoPostBack="true" onkeypress="fnAllowNumbersOnly(true,false,this)" OnTextChanged="txtHigAmount_TextChanged" /><cc1:FilteredTextBoxExtender
                                                                                ID="ftxtHigAmount" runat="server" Enabled="true" FilterType="Numbers,Custom"
                                                                                TargetControlID="txtHigAmount" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalAmount" MaxLength="10" runat="server" Style="text-align: right"
                                                                            ContentEditable="false"></asp:TextBox></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select Sale Asset">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkHigSale" runat="server" AutoPostBack="True" OnCheckedChanged="chkHigSale_CheckedChanged" /></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:CheckBox ID="chkHigSale" runat="server" AutoPostBack="True" OnCheckedChanged="chkHigSale_CheckedChanged" /></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" class="styleFieldAlign">
                                                        <asp:CheckBox runat="server" ID="ChkMS" Text="Medium Liquid Security Details" OnCheckedChanged="ChkMS_CheckedChanged"
                                                            AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldAlign">
                                                        <asp:GridView ID="gvMedLiqDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="True" OnDataBound="gvMedLiqDetails_DataBound">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Serial No" DataField="SLNo" />
                                                                <asp:BoundField HeaderText="Description" DataField="Description" />
                                                                <asp:BoundField HeaderText="Model" DataField="Model" />
                                                                <asp:BoundField HeaderText="Year" DataField="Year" />
                                                                <asp:BoundField HeaderText="Reg No" DataField="Registration_No" />
                                                                <asp:BoundField HeaderText="Serial Number" DataField="Serial_No" />
                                                                <asp:BoundField HeaderText="Original Value" DataField="Value" />
                                                                <asp:BoundField HeaderText="Last Valuation Date" DataField="Valuation_Date" />
                                                                <asp:TemplateField HeaderText="Valuation Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></AlternatingItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LblTotal" runat="server" Text="Grand Total"></asp:Label></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Unit">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMedSaleUnit" MaxLength="10" runat="server" ContentEditable="false"
                                                                            AutoPostBack="true" OnTextChanged="txtMedSaleUnit_TextChanged" Style="text-align: right"
                                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" /><cc1:FilteredTextBoxExtender ID="FtxtMedSaleUnit"
                                                                                Enabled="true" runat="server" FilterType="Numbers,Custom" TargetControlID="txtMedSaleUnit"
                                                                                ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalUnits" runat="server" ContentEditable="false" Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Amount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMedAmount" MaxLength="10" runat="server" Style="text-align: right"
                                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" OnTextChanged="txtMedAmount_TextChanged" /><cc1:FilteredTextBoxExtender
                                                                                ID="ftxtMedAmount" runat="server" Enabled="true" FilterType="Numbers,Custom"
                                                                                TargetControlID="txtMedAmount" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalAmount" runat="server" ContentEditable="false" Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select Sale Asset">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkMedSale" runat="server" AutoPostBack="True" OnCheckedChanged="chkMedSale_CheckedChanged" /></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:CheckBox ID="chkMedSale" runat="server" AutoPostBack="True" OnCheckedChanged="chkMedSale_CheckedChanged" /></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" class="styleFieldAlign">
                                                        <asp:CheckBox runat="server" ID="ChkLS" Text="Low Liquid Security Details" OnCheckedChanged="ChkLS_CheckedChanged"
                                                            AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldAlign">
                                                        <asp:GridView ID="gvLowLiqDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="True" OnDataBound="gvLowLiqDetails_DataBound">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Serial No" DataField="SLNo" />
                                                                <asp:BoundField HeaderText="Location Details" DataField="Location_Details" />
                                                                <asp:TemplateField HeaderText="Measurement">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblAvlUnits" runat="server" Style="text-align: right" Text='<% #Bind("Measurement") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblAvlUnits" runat="server" Style="text-align: right" Text='<% #Bind("Measurement") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Unit Rate" DataField="Unit_Rate" />
                                                                <asp:BoundField HeaderText="Value" DataField="Value" />
                                                                <asp:BoundField HeaderText="Current Unit Value" DataField="CURRENT_UNIT_VALUE" />
                                                                <asp:TemplateField HeaderText="Current Market Value">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></AlternatingItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LblTotal" runat="server" Text="Grand Total"></asp:Label></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Unit">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLowSaleUnit" MaxLength="10" runat="server" Style="text-align: right"
                                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" OnTextChanged="txtLowSaleUnit_TextChanged"
                                                                            AutoPostBack="True" /><cc1:FilteredTextBoxExtender ID="ftxtLowSaleUnit" runat="server"
                                                                                Enabled="true" FilterType="Numbers,Custom" TargetControlID="txtLowSaleUnit" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalUnits" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Amount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtLowAmount" MaxLength="10" runat="server" Style="text-align: right"
                                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" OnTextChanged="txtLowAmount_TextChanged"
                                                                            AutoPostBack="True" /><cc1:FilteredTextBoxExtender ID="FtxtLowAmount" runat="server"
                                                                                Enabled="true" FilterType="Numbers,Custom" TargetControlID="txtLowAmount" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalAmount" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select Sale Asset">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkLowSale" runat="server" AutoPostBack="True" OnCheckedChanged="chkLowSale_CheckedChanged" /></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:CheckBox ID="chkLowSale" runat="server" AutoPostBack="True" OnCheckedChanged="chkLowSale_CheckedChanged" /></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" class="styleFieldAlign">
                                                        <asp:CheckBox runat="server" ID="ChkCOM" Text="Commodity Details" OnCheckedChanged="ChkCOM_CheckedChanged"
                                                            AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldAlign">
                                                        <asp:GridView ID="gvCommoDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="True" OnDataBound="gvCommoDetails_DataBound">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Serial No" DataField="SLNo" />
                                                                <asp:BoundField HeaderText="Description" DataField="Description" />
                                                                <asp:BoundField HeaderText="Unit of Measurement" DataField="Unit_Of_Measure" />
                                                                <asp:BoundField HeaderText="Unit Quantity" DataField="Unit_Quantity" />
                                                                <asp:BoundField HeaderText="Unit Price" DataField="UNIT_MARKET_PRICE" />
                                                                <asp:TemplateField HeaderText="Total Quantity">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblAvlUnits" runat="server" Style="text-align: right" Text='<% #Bind("TotalQuantity") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblAvlUnits" runat="server" Style="text-align: right" Text='<% #Bind("TotalQuantity") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Current Unit Value" DataField="CURRENT_UNIT_VALUE" />
                                                                <asp:TemplateField HeaderText="Current Market Value">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></AlternatingItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LblTotal" runat="server" Text="Grand Total"></asp:Label></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Unit">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCommSaleUnit" MaxLength="10" runat="server" Style="text-align: right"
                                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" AutoPostBack="true" OnTextChanged="txtCommSaleUnit_TextChanged" /><cc1:FilteredTextBoxExtender
                                                                                ID="FtxtCommSaleUnit" Enabled="true" runat="server" FilterType="Numbers,Custom"
                                                                                TargetControlID="txtCommSaleUnit" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalUnits" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Amount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtCommAmount" MaxLength="10" runat="server" Style="text-align: right"
                                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" AutoPostBack="true" OnTextChanged="txtCommAmount_TextChanged" /><cc1:FilteredTextBoxExtender
                                                                                ID="FtxtCommAmount" runat="server" Enabled="true" FilterType="Numbers,Custom"
                                                                                TargetControlID="txtCommAmount" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalAmount" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select Sale Asset">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkCommSale" runat="server" OnCheckedChanged="chkCommSale_CheckedChanged"
                                                                            AutoPostBack="True" /></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:CheckBox ID="chkCommSale" runat="server" OnCheckedChanged="chkCommSale_CheckedChanged"
                                                                            AutoPostBack="True" /></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" class="styleFieldAlign">
                                                        <asp:CheckBox runat="server" ID="ChkFS" Text="Financial Investment Details" OnCheckedChanged="ChkFS_CheckedChanged"
                                                            AutoPostBack="True"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldAlign">
                                                        <asp:GridView ID="gvFinDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            ShowFooter="True" OnDataBound="gvFinDetails_DataBound">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Serial No" DataField="SLNo" />
                                                                <asp:BoundField HeaderText="Policy Issued By" DataField="Insurance_Issued_By" />
                                                                <asp:BoundField HeaderText="Policy No" DataField="Policy_No" />
                                                                <asp:BoundField HeaderText="Policy Value" DataField="Policy_Value" />
                                                                <asp:BoundField HeaderText="Maturity Date" DataField="Maturity_Date" />
                                                                <asp:BoundField HeaderText="Last Valuation Date" DataField="Valuation_Date" />
                                                                <asp:TemplateField HeaderText="Valuation Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblValAmnt" runat="server" Text='<% #Bind("Valuation_Current_Market_Value") %>'
                                                                            Style="text-align: right"></asp:Label></AlternatingItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="LblTotal" runat="server" Text="Grand Total"></asp:Label></FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Unit">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtFinSaleUnit" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)" AutoPostBack="True"
                                                                            OnTextChanged="txtFinSaleUnit_TextChanged" /><cc1:FilteredTextBoxExtender ID="FtxtFinSaleUnit"
                                                                                Enabled="true" FilterType="Numbers,Custom" TargetControlID="txtFinSaleUnit" runat="server"
                                                                                ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalUnits" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Sale Amount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtFinAmount" MaxLength="10"  runat="server"
                                                                            Style="text-align: right" onkeypress="fnAllowNumbersOnly(true,false,this)" AutoPostBack="True" /><cc1:FilteredTextBoxExtender
                                                                                ID="ftxtFinAmount" Enabled="true" FilterType="Numbers,Custom" TargetControlID="txtFinAmount"
                                                                                runat="server" ValidChars="., ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="TxtTotalAmount" MaxLength="10" runat="server" ContentEditable="false"
                                                                            Style="text-align: right"></asp:TextBox></FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Select Sale Asset">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkFinSale" runat="server" OnCheckedChanged="chkFinSale_CheckedChanged"
                                                                            AutoPostBack="True" /></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:CheckBox ID="chkFinSale" runat="server" OnCheckedChanged="chkFinSale_CheckedChanged"
                                                                            AutoPostBack="True" /></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblItemRefNo" runat="server" Text='<% #Bind("COLLATERAL_ITEM_REF_NO") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <asp:Label ID="LblIsSold" runat="server" Text='<% #Bind("IS_SOLD") %>'></asp:Label></AlternatingItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tpColSaleApp" CssClass="tabpan" BackColor="Red"
                                HeaderText="  Collateral Sale Approval">
                                <HeaderTemplate>
                                    Collateral Sale Approval</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdColSaleApp" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="plnApproval" runat="server" CssClass="stylePanel" GroupingText="Approval Details"
                                                Width="980px">
                                                <table id="table3" runat="server" align="left" width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label ID="lblApprovalName" runat="server" Text="Approver Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtApprovalName" runat="server" ContentEditable="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlStatus" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"
                                                                AutoPostBack="true" runat="server">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr class="styleFieldLabel">
                                                        <td>
                                                            <asp:Label ID="lblPassword" runat="server" CssClass="styleFieldLabel" Text="Password"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="6" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr class="styleFieldLabel">
                                                        <td>
                                                            <asp:Label ID="lblRemarks" runat="server" CssClass="styleFieldLabel" Text="Remarks"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtRemarks" MaxLength="80" onkeyup="maxlengthfortxt(80);" runat="server"
                                                                TextMode="MultiLine" Width="50%" Height="90px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="plnApprovalHistory" runat="server" CssClass="stylePanel" GroupingText="Approval History">
                                                <asp:GridView ID="grdApprovalHistory" runat="server" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:BoundField DataField="SLNO" HeaderText="SNo" />
                                                        <asp:BoundField DataField="Approver_Name" HeaderText="Approver Name" />
                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tpRevoke" CssClass="tabpan" BackColor="Red" HeaderText="Revoke">
                                <HeaderTemplate>
                                    Revoke</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdRevoke" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="PnlRevoke" runat="server" CssClass="stylePanel" GroupingText="Revoke Details"
                                                Width="980px">
                                                <table align="left" cellpadding="5" width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label ID="LblRvkAppName" Text="Revoker Name" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtRvkAppName" runat="server" ContentEditable="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td cssclass="styleFieldLabel">
                                                            <asp:Label ID="LblRvkStatus" CssClass="styleFieldLabel" Text="Status" runat="server"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="LblRevoke" Text="Revoked" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td cssclass="styleFieldLabel">
                                                            <asp:Label ID="LblRvkPwd" CssClass="styleFieldLabel" runat="server" Text="Password"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtRvkPwd" runat="server" TextMode="Password" MaxLength="6"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td cssclass="styleFieldLabel">
                                                            <asp:Label ID="LblRvkRemarks" CssClass="styleFieldLabel" runat="server" Text="Remarks"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TxtRvkRemarks" MaxLength="80" onkeyup="maxlengthfortxt(80);" runat="server"
                                                                TextMode="MultiLine" Width="50%" Height="90px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tpSaleInvoice" runat="server" HeaderText="Sale Invoice">
                                <HeaderTemplate>
                                    Sale Invoice</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdSaleInvoice" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Panel ID="PnlInvoiceDetails" runat="server" CssClass="stylePanel" GroupingText="Invoice Details">
                                                <table cellspacing="0" width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:GridView ID="GrvSInvoiceDetails" runat="server" AutoGenerateColumns="False"
                                                                OnRowDataBound="GrvSInvoiceDetails_RowDataBound" Width="100%" ShowFooter="true">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="AccountType">
                                                                        <ItemStyle Wrap="true" HorizontalAlign="Center" />
                                                                        <AlternatingItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvAccountType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountType_SelectedIndexChanged">
                                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="C">Customer</asp:ListItem>
                                                                                <asp:ListItem Value="E">Entity</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </AlternatingItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvAccountType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountType_SelectedIndexChanged">
                                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="C">Customer</asp:ListItem>
                                                                                <asp:ListItem Value="E">Entity</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Account Code">
                                                                        <ItemStyle Wrap="true" HorizontalAlign="Center" Width="23%" />
                                                                        <AlternatingItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvAccountCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountCode_SelectedIndexChanged"
                                                                                Width="230px">
                                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </AlternatingItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvAccountCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSInvAccountCode_SelectedIndexChanged"
                                                                                Width="230px">
                                                                                <asp:ListItem>--Select--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="23%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Debit GL Code">
                                                                        <ItemStyle Wrap="true" Width="10%" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LblSInvDebitGLCode" runat="server" Text='<%# Bind("DBT_GL_CODE") %>'> </asp:Label></ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <asp:Label ID="LblSInvDebitGLCode" runat="server" Text='<%# Bind("DBT_GL_CODE") %>'> </asp:Label></AlternatingItemTemplate>
                                                                        <HeaderStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Debit SL Code">
                                                                        <ItemStyle Wrap="true" Width="10%" />
                                                                        <AlternatingItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvDebitSLCode" runat="server">
                                                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvddlSInvDebitSLCode" runat="server" ControlToValidate="ddlSInvDebitSLCode"
                                                                                Display="None" ErrorMessage="Select the Debit SL Code" SetFocusOnError="True"
                                                                                InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator></AlternatingItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvDebitSLCode" runat="server">
                                                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Description">
                                                                        <ItemStyle Wrap="true" />
                                                                        <AlternatingItemTemplate>
                                                                            <asp:Label ID="LblSInvGlDesc" runat="server" Text='<%# Bind("DBT_GL_DESC") %>'> </asp:Label></AlternatingItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LblSInvGlDesc" runat="server" Text='<%# Bind("DBT_GL_DESC") %>'> </asp:Label></ItemTemplate>
                                                                        <HeaderStyle Width="15%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Credit GL Code">
                                                                        <ItemStyle Wrap="true" Width="10%" />
                                                                        <AlternatingItemTemplate>
                                                                            <asp:Label ID="LblSInvCrdGLCode" runat="server" Text='<%# Bind("CRDT_GL_CODE") %>'> </asp:Label></AlternatingItemTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LblSInvCrdGLCode" runat="server" Text='<%# Bind("CRDT_GL_CODE") %>'> </asp:Label></ItemTemplate>
                                                                        <HeaderStyle Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Credit SL Code">
                                                                        <ItemStyle Wrap="true" Width="10%" />
                                                                        <AlternatingItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvCrdSLCode" runat="server">
                                                                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </AlternatingItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="LblSInvTotal" runat="server" Text="Grand Total" /></FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlSInvCrdSLCode" runat="server">
                                                                                <asp:ListItem Value="">--Select--</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle Width="5%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount">
                                                                        <ItemStyle Wrap="true" HorizontalAlign="Center" Width="20%" />
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="TxtSInvAmount" runat="server" Text='<%# Bind("AMNT") %>' Style="text-align: right"
                                                                                Width="98%" ContentEditable="false"> </asp:TextBox><cc1:FilteredTextBoxExtender ID="fTxtSInvAmount"
                                                                                    runat="server" FilterType="Custom,Numbers" ValidChars="." TargetControlID="TxtSInvAmount">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <asp:TextBox ID="TxtSInvAmount" runat="server" Text='<%# Bind("AMNT") %>' Style="text-align: right"
                                                                                Width="98%" ContentEditable="false"> </asp:TextBox><cc1:FilteredTextBoxExtender ID="FTxtSInvAmount"
                                                                                    runat="server" FilterType="Custom,Numbers" ValidChars="." TargetControlID="TxtSInvAmount">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                        </AlternatingItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="TxtSInvTotal" runat="server" ContentEditable="false" Text='<%# Bind("AMNT") %>'
                                                                                Style="text-align: right" Width="98%" /><cc1:FilteredTextBoxExtender ID="FTxtSInvTotal"
                                                                                    runat="server" FilterType="Custom,Numbers" ValidChars="." TargetControlID="TxtSInvTotal">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <HeaderStyle Width="20%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="PnlReceiptDetails" runat="server" CssClass="stylePanel" GroupingText="Receipt Details">
                                                <table cellspacing="0" width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:GridView ID="GrvReceiptDetails" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                                                OnRowDataBound="GrvReceiptDetails_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Receipt Reference Number">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="TxtRcptRefNo" runat="server" Width="100%" Text='<%# Bind("RECEIPT_NO") %>'
                                                                                ReadOnly="true"> </asp:TextBox></ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <asp:TextBox ID="TxtRcptRefNo" runat="server" Width="100%" Text='<%# Bind("RECEIPT_NO") %>'
                                                                                ReadOnly="true"> </asp:TextBox></AlternatingItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Transaction Date">
                                                                        <ItemStyle Wrap="true" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="LblGrndTotal" runat="server" Text="Grand Total"></asp:Label></FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="TxtSInvTxnDate" runat="server" Text='<%# Bind("TRXNDT") %>' ReadOnly="true"> </asp:TextBox></ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <asp:TextBox ID="TxtSInvTxnDate" runat="server" Text='<%# Bind("TRXNDT") %>' ReadOnly="true"> </asp:TextBox></AlternatingItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Transaction Amount">
                                                                        <ItemStyle Wrap="true" />
                                                                        <FooterTemplate>
                                                                            <asp:TextBox ID="TxtSInvTxnTotal" runat="server" ReadOnly="true" Width="100%"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                                ID="ftxtPremium" runat="server" FilterType="Custom,Numbers" TargetControlID="TxtSInvTxnTotal"
                                                                                ValidChars=".">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="TxtSInvTxnAmnt" runat="server" Text='<%# Bind("TRXNAMNT") %>' Width="100%"
                                                                                ReadOnly="true"> </asp:TextBox><cc1:FilteredTextBoxExtender ID="ftxtPremium" runat="server"
                                                                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="TxtSInvTxnAmnt">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <asp:TextBox ID="TxtSInvTxnAmnt" runat="server" Text='<%# Bind("TRXNAMNT") %>' Width="100%"
                                                                                ReadOnly="true"> </asp:TextBox><cc1:FilteredTextBoxExtender ID="ftxtPremium" runat="server"
                                                                                    FilterType="Custom,Numbers" ValidChars="." TargetControlID="TxtSInvTxnAmnt">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                        </AlternatingItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemStyle Wrap="true" />
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LblRcptID" runat="server" Text='<%# Bind("Receipt_ID") %>' Width="100%"> </asp:Label></ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <asp:Label ID="LblRcptID" runat="server" Text='<%# Bind("Receipt_ID") %>' Width="100%"> </asp:Label></AlternatingItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle Wrap="true" />
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="ChkRcpt" runat="server" AutoPostBack="true" OnCheckedChanged="ChkRcpt_CheckedChanged" /></ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <asp:CheckBox ID="ChkRcpt" runat="server" OnCheckedChanged="ChkRcpt_CheckedChanged"
                                                                                AutoPostBack="true" /></AlternatingItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                        <span id="lblErrorMessage" runat="server" style="color: Red; font-size: medium">
                        </span>
                        <asp:TextBox ID="txthiddenfield" runat="server" Style="display: none;"></asp:TextBox>
                        <div style="visibility: hidden">
                            <asp:Button runat="server" ID="btnTab" OnClick="btnTab_Click" /></div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" ValidationGroup="btnSave" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators('btnSave');"  Text="Save" />
                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                            CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 990px">
                        <asp:ValidationSummary ID="vsColSale" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" DisplayMode="List" />
                        <asp:ValidationSummary ID="vs1" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" DisplayMode="List"
                            ValidationGroup="Collateral Sale Details" />
                        <asp:CustomValidator ID="cvColSale" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

 function ActiveTabChange()
 {
        var tabclickbutton = document.getElementById("<%=btnTab.ClientID  %>")
        if(tabclickbutton !=null)
        {
            tabclickbutton.click();
        }
 }
         
   function AddGrid(rows, Grid,textbox,txttotal,Chk)
    {
        var tab=document.getElementById(Grid);
        var GridText=Grid +'_ctl0';
        var total;
        total=0;
     for(var index=2;index<=rows + 1 ;index++)
      {
            if(index >=10)
            {
                GridText=Grid +'_ctl';    
            }
            if(document.getElementById(GridText + index + '_' + Chk).checked==true)
            {
                var search=GridText + index + '_' + textbox;
                var val= document.getElementById(search).value;
                if(val!="")
                {
                total= parseFloat(total) + parseFloat(val);
                }
            }
           
      }
    
      document.getElementById(txttotal).value =total;
      
        
    }
    function CalcSaleAmt(SaleUnit,NoUnits,ValAmnt,TxtAmnt)
    {
        var NoUnits;
        NoUnits= parseFloat(NoUnits);
        var Sal;
        Sal= parseFloat(document.getElementById(SaleUnit).value);
        var MrktVal 
        MrktVal=parseFloat(ValAmnt);
        var Tot;
        if(Sal > NoUnits)
        {
            alert('The Sale Unit should not be greater than the Available Units');
            document.getElementById(SaleUnit).focus();
            return;
        }
        else 
        {
        Tot= Sal * MrktVal;
        document.getElementById(TxtAmnt).value= Tot.toString();
        }
         
         document.getElementById('<%= btnSave.ClientID %>').disabled = false;
         
        return;
        
    }
    
    

 
    </script>

</asp:Content>
