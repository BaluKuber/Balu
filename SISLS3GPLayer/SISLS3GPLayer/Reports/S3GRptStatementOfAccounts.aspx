<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptStatementOfAccounts.aspx.cs" Inherits="Reports_S3GRptStatementOfAccounts"
    Title="SOA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading" width="100%">

                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Statement Of Accounts">
                        </asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="styleFieldAlign">
                        <table width="100%">
                            <tr>
                                <td width="50%">
                                    <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
                                        CssClass="stylePanel">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" width="35%">
                                                    <asp:Label runat="server" ID="lblCustomerName" CssClass="styleReqFieldLabel" Text="Customer Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="2">
                                                    <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none;" MaxLength="50"></asp:TextBox>
                                                    <uc2:LOV ID="ucCustomerCodeLov" onblur="return fnLoadCustomer();" runat="server"
                                                        strLOV_Code="CMD" />
                                                    <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" OnClick="btnLoadCustomer_OnClick"
                                                        Style="display: none" /><input id="hdnCustID" type="hidden" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" width="100%">
                                                    <uc1:S3GCustomerAddress ID="ucCustDetails" ShowCustomerName="false" runat="server" FirstColumnStyle="styleFieldLabel"
                                                        SecondColumnStyle="styleFieldAlign" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <%--  <asp:RequiredFieldValidator ID="rfvcustomer" runat="server" ErrorMessage="Select Customer Name" ValidationGroup="btnGo" Enabled="true" Display="None" SetFocusOnError="True" ControlToValidate="txtCustomerCode">
                                                     
                </asp:RequiredFieldValidator>--%>
                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20px"></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td width="50%">
                                    <asp:Panel ID="pnlStatementOfAccounts" runat="server" CssClass="stylePanel" GroupingText="Input Criteria">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" width="25%">
                                                    <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel" ToolTip="Line of Business"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="25%">
                                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                                        OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" ToolTip="Line of Business">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvlob" runat="server" Display="None" ControlToValidate="ddlLOB" ToolTip="Start Date"
                                                        ValidationGroup="btnGo" InitialValue="-1" CssClass="styleMandatoryLabel" ErrorMessage="Select Line Of Business"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>

                                                </td>
                                            </tr>

                                            <tr>
                                                <td height="4px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="25%">
                                                    <asp:Label ID="lblregion" runat="server" Text="Location1" CssClass="styleDisplayLabel" ToolTip="Location1"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="25%">
                                                    <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                                        OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" ToolTip="Location1">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="4px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="25%">
                                                    <asp:Label ID="lblbranch" runat="server" Text="Location2" CssClass="styleDisplayLabel" ToolTip="Location2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="25%">
                                                    <asp:DropDownList ID="ddlbranch" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                                        OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged" ToolTip="Location2">
                                                    </asp:DropDownList>
                                                </td>



                                            </tr>
                                            <tr>
                                                <td height="4px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPoff" CssClass="styleDisplayLabel" Text="Print Off" ToolTip="Print Off"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" align="left">
                                                    <asp:CheckBox ID="chkPoff" runat="server" TextAlign="Right" ToolTip="Print Off" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="4px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStartDateSearch" runat="server" CssClass="styleDisplayLabel" Text="Start Date" ToolTip="Start Date" />
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <input id="hidDate" runat="server" type="hidden" />
                                                    <asp:TextBox ID="txtStartDateSearch" AutoPostBack="true" runat="server" Width="60%" ToolTip="Start Date" OnTextChanged="txtStartDateSearch_OnTextChanged"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="None" ControlToValidate="txtStartDateSearch" ToolTip="Start Date"
                                                        ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select Start Date"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                        PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="4px"></td>
                                            </tr>
                                            <tr>

                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEndDateSearch" runat="server" CssClass="styleDisplayLabel" Text="End Date" ToolTip="End Date" />
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEndDateSearch" AutoPostBack="true" runat="server" Width="60%" OnTextChanged="txtEndDateSearch_OnTextChanged" ToolTip="End Date"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" Display="None" ControlToValidate="txtEndDateSearch"
                                                        ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select End Date"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                        PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch">
                                                    </cc1:CalendarExtender>
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
                    <td align="center">
                        <div id="divacc" runat="server" style="height: 150px; overflow: auto; width: 625px; display: none">
                            <asp:GridView ID="grvprimeaccount" runat="server" AutoGenerateColumns="False"
                                BorderWidth="2" OnRowDataBound="grvprimeaccount_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl.No." ItemStyle-Width="2%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rental Schedule No." ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMLA" runat="server" Text='<%#Eval("PRIMEACCOUNTNO")%>' ToolTip="Rental Schedule No"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Account No." ItemStyle-HorizontalAlign="Left" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSLA" runat="server" Text='<%#Eval("SUBACCOUNTNO")%>' ToolTip="Sub Account No"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select All" SortExpression="Sellect All">
                                        <HeaderTemplate>
                                            <asp:Label ID="lblSelectAll" runat="server" Text="Select All" ToolTip="Select All"></asp:Label>
                                            <br />
                                            <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" />
                                        </HeaderTemplate>
                                        <HeaderStyle />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelectAccount" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" ToolTip="Select Account" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </td>
                </tr>

                <tr align="center">
                    <td>
                        <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" OnClientClick="return fnCheckPageValidators('btnGo',false);" OnClick="btnGo_Click"
                            ValidationGroup="btnGo" />
                        <asp:Button ID="btnclear" runat="server" CssClass="styleSubmitButton" Text="Clear" OnClientClick="return fnConfirmClear();"
                            OnClick="btnclear_Click" />
                    </td>
                </tr>


                <tr>
                    <td align="right" colspan="2" width="100%">
                        <asp:Label ID="lblAmounts" runat="server" Text="All amounts are in" Visible="false" CssClass="styleDisplayLabel"></asp:Label>

                    </td>
                </tr>


                <tr>
                    <td class="styleFieldAlign">
                        <asp:Panel ID="divAssetview" runat="server" CssClass="accordionHeader" Width="98.5%" Visible="false">
                            <div style="float: left;">
                                Invoice Details
                            </div>
                            <div style="float: left; margin-left: 20px;">
                                <asp:Label ID="lblDetails" runat="server" onclick='funshowaddless()'>(Show Details...)</asp:Label>
                            </div>
                            <div style="float: right; vertical-align: middle;">
                                <asp:ImageButton ID="imgDetails" runat="server" OnClientClick="funshowaddless()" ImageUrl="~/Images/expand_blue.jpg"
                                    AlternateText="(Show Details...)" />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlasset" runat="server" CssClass="stylePanel" GroupingText="Invoice Details">
                            <div id="divAsset" runat="server" style="overflow: scroll; height: 100px; display: block">
                                <asp:GridView ID="gvasset" runat="server" AutoGenerateColumns="False" BorderWidth="2"
                                    CssClass="styleInfoLabel" OnRowDataBound="gvasset_RowDataBound" ShowFooter="true" Style="margin-bottom: 0px" Width="150%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Rental Schedule No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PRIMEACCOUNTNO") %>' ToolTip="Rental Schedule No"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account No." Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SUBACCOUNTNO") %>' ToolTip="Sub Account No"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAsset" runat="server" Text='<%# Bind("AssetDesc") %>' ToolTip="Asset Description"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPO_Number" runat="server" Text='<%# Bind("PO_Number") %>' ToolTip="PO Number"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPO_Date" runat="server" Text='<%# Bind("PO_Date") %>' ToolTip="PO Date"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PI No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPI_Number" runat="server" Text='<%# Bind("PI_Number") %>' ToolTip="PO Number"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="PI Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPI_Date" runat="server" Text='<%# Bind("PI_Date") %>' ToolTip="PI Date"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VI No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVI_Number" runat="server" Text='<%# Bind("VI_Number") %>' ToolTip="PO Number"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="VI Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVI_Date" runat="server" Text='<%# Bind("VI_Date") %>' ToolTip="PI Date"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Entity Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntity_Name" runat="server" Text='<%# Bind("Entity_Name") %>' ToolTip="Entity Name"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAsset_Category" runat="server" Text='<%# Bind("Asset_Category") %>' ToolTip="Asset Category"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAsset_Type" runat="server" Text='<%# Bind("Asset_Type") %>' ToolTip="Asset Type"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Sub Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAsset_Sub_Type" runat="server" Text='<%# Bind("Asset_Sub_Type") %>' ToolTip="Asset Sub Type"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SI/Reg No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSINo" runat="server" Text='<%# Bind("RegNo") %>' ToolTip="Reg No"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Terms">
                                            <ItemTemplate>
                                                <asp:Label ID="lblterms" runat="server" Text='<%# Bind("Terms") %>' ToolTip="Terms"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total" ToolTip="Total"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal_SCH_Amt" runat="server" Text='<%# Bind("Total_SCH_Amt") %>' ToolTip="Amount"></asp:Label>
                                            </ItemTemplate>
                                             <FooterTemplate>
                                                <asp:Label ID="lblTotalAmt" runat="server"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <table align="center">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAsset" CssClass="styleSubmitButton" runat="server" Text="Asset Annexure" OnClick="btnAsset_Click" ToolTip="Submit" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <cc1:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="pnlasset"
                            ExpandControlID="divAssetview" CollapseControlID="divAssetview" Collapsed="True"
                            TextLabelID="lblDetails" ImageControlID="imgDetails" ExpandedText="(Hide Details...)"
                            CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                            CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="false" SkinID="CollapsiblePanelDemo" />

                    </td>
                </tr>

                <tr>
                    <td class="styleFieldAlign">
                        <asp:Panel ID="divAccountsummary" runat="server" CssClass="accordionHeader" Width="98.5%" Visible="false">
                            <div style="float: left;">
                                Account Summary Details
                            </div>
                            <div style="float: left; margin-left: 20px;">
                                <asp:Label ID="lblaccountsummary" runat="server" onclick='funshowaddless1()'>(Show Details...)</asp:Label>
                            </div>
                            <div style="float: right; vertical-align: middle;">
                                <asp:ImageButton ID="imgDetails1" runat="server" OnClientClick="funshowaddless1()" ImageUrl="~/Images/expand_blue.jpg"
                                    AlternateText="(Show Details...)" />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlAccountDetails" runat="server" CssClass="stylePanel" GroupingText="Account Summary Details">
                            <div id="divAccount" runat="server" style="overflow: auto; height: 100px; display: block">
                                <asp:GridView ID="grvAccount" runat="server" AutoGenerateColumns="False" BorderWidth="2"
                                    CssClass="styleInfoLabel" ShowFooter="true" OnRowDataBound="grvAccount_RowDataBound" Style="margin-bottom: 0px" Width="98%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Line of Business" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLobid" runat="server" Text='<%# Bind("LobName") %>' ToolTip="Line of Business"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Rental Schedule No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PRIMEACCOUNTNO") %>' ToolTip="Rental Schedule No"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total" ToolTip="Total"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account No." Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SUBACCOUNTNO") %>' ToolTip="Sub Account No"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Schedule Amount" FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("AmountFinanced") %>' ToolTip="Amount Financed"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalAmountFinanced" runat="server" ToolTip="Total Finance Amount"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Invoice Value" FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalInv" runat="server" Text='<%# Bind("InvoiceAmount") %>' ToolTip="Total Invoice Value"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal_Invoice" runat="server" ToolTip="Total Invoice Value"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Capitalised Amount" FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCapitalisedAmount" runat="server" Text='<%# Bind("CapitalisedAmount") %>' ToolTip="Capitalised Amount"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCapitalised" runat="server" ToolTip="Total Capitalised Amount"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <%--  <asp:TemplateField HeaderText="Terms">
                                    <ItemTemplate>
                                        <asp:Label ID="lblterms" runat="server" Text='<%# Bind("Terms") %>' ToolTip="Terms"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Total" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Yet To be billed">
                                            <ItemTemplate>
                                                <asp:Label ID="lblYetbilled" runat="server" Text='<%# Bind("YetToBeBilled") %>' ToolTip="Yet To be billed"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalYetbilled" runat="server" ToolTip="sum of  Yet to be billed amount"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
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
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Installment Received">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbalance" runat="server" Text='<%# Bind("Balance") %>' ToolTip="Balance"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalbilledbalance" runat="server" ToolTip="sum of   Balance  amount"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <cc1:CollapsiblePanelExtender ID="cpeDemo1" runat="Server" TargetControlID="pnlAccountDetails"
                            ExpandControlID="divAccountsummary" CollapseControlID="divAccountsummary" Collapsed="True"
                            TextLabelID="lblaccountsummary" ImageControlID="imgDetails1" ExpandedText="(Hide Details...)"
                            CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                            CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="false" SkinID="CollapsiblePanelDemo" />

                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                        <asp:Panel ID="pnlTransactionDetails" runat="server" CssClass="stylePanel" GroupingText="Transaction Details">
                            <asp:Label ID="lblOpeningBalance" runat="server"></asp:Label>
                            <br />
                            <br />
                            <div id="divTransaction" runat="server" style="overflow: auto; height: 200px; display: block">
                                <asp:GridView ID="grvtransaction" runat="server" AutoGenerateColumns="False" BorderWidth="2"
                                    CssClass="styleInfoLabel" Width="98%" OnRowDataBound="grvtransaction_RowDataBound" ShowFooter="true" ShowHeader="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Rental Schedule No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PrimeAccountNo") %>' ToolTip="Rental Schedule No"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account No." Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SubAccountNo") %>' ToolTip="Sub Account No"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDocDate" runat="server" Text='<%# Bind("DocumentDate") %>' ToolTip="Document Date"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblvaluedate" runat="server" Text='<%# Bind("ValueDate") %>' ToolTip="Value Date"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Document Reference">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDocumentReference" runat="server" Text='<%# Bind("DocumentReference") %>' ToolTip="Document Reference"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldesc" runat="server" Text='<%# Bind("Description") %>' ToolTip="Description"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="Total" ToolTip="Total"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDues" runat="server" Text='<%# Bind("Dues") %>' ToolTip="Dues"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalDues" runat="server" ToolTip="sum of  Dues amount"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReceipts" runat="server" Text='<%# Bind("Receipts") %>' ToolTip="Receipts"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalReceipts" runat="server" ToolTip="sum of  Receipts amount"></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbalance" runat="server" Text='<%# Bind("Balance") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalbalance" runat="server" ToolTip="sum of  Balance amount"></asp:Label>
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
                    <td class="styleFieldAlign">
                        <table width="100%">
                            <tr>
                                <td class="styleFieldAlign">
                                    <asp:Panel ID="pnlsummary" runat="server" CssClass="stylePanel" GroupingText="summary Details"
                                        Width="100%">
                                        <table border="0" width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInstallmentDues" runat="server" Text="Rental Due" ToolTip="Rental Dues"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtInstallmentDues" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Rental Dues">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInterestDues" runat="server" Text="VAT Due" ToolTip="VAT Dues"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtInterestDues" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Tax Dues">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblInsuranceDues" runat="server" Text="AMF Due" ToolTip="AMF Dues"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="height: 26px">
                                                    <asp:TextBox ID="txtInsuranceDues" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Insurance Dues">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblODIDues" runat="server" Text="Service Tax Due" ToolTip="Service Tax Dues"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtODIDues" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Service Tax Dues">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblOtherDues" runat="server" Text="Other Due" ToolTip="Other Dues"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtOtherDues" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Other Dues">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:Panel ID="PnlMemorandum" runat="server" CssClass="stylePanel" GroupingText="Memorandum Details">
                                        <table width="100%">

                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblChequeReturnDue" runat="server" Text="Cheque Return Due" ToolTip="Cheque Return Due"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtChequeReturnDue" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Cheque Return Due">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDocumentDue" runat="server" Text="Document Charges Due" ToolTip="Document Charges Due "> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="height: 26px">
                                                    <asp:TextBox ID="txtDocumentChargesDue" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Document Charges Due">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblODIDue" runat="server" Text="ODI Due" ToolTip="ODI Due"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtODIDue" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="ODI Due">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblVerifiDue" runat="server" Text="Verification Due" ToolTip="Verification Due"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtverifiDue" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Verification Due">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="10px"></td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMemoOtherDues" runat="server" Text="Other Due" ToolTip="Other Due"> </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMemoOtherDues" runat="server" Style="text-align: Right" ReadOnly="True" ToolTip="Other Due">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <%--     <tr>
                                        <td>
                                            <div style="height: 30px;" id="div1" runat="server">
                                            </div>
                                        </td>
                                    </tr>--%>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <asp:Button ID="BtnPrint" CssClass="styleSubmitButton" runat="server" Text="Print" OnClick="btnPrint_Click" ToolTip="Submit" />

                    </td>
                </tr>
                <%-- <tr>
            <td align="center">
               
            </td>
        </tr>--%>
                <tr>
                    <td width="100%" colspan="2">
                        <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ErrorMessage="Select Customer Name" ValidationGroup="btnGo" Enabled="true" Display="None" SetFocusOnError="True" ControlToValidate="txtCustomerCode">
                                              
                        </asp:RequiredFieldValidator>
                        <asp:ValidationSummary ID="vsSOA" runat="server" CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):"
                            ShowSummary="true" ValidationGroup="btnGo" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAsset" />
        </Triggers>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function fnLoadCustomer() {
            document.getElementById("<%= btnLoadCustomer.ClientID %>").click();

        }
        function fnSelectUser(chkSelect, chkSelectAll) {

            var grvprimeaccount = document.getElementById('ctl00_ContentPlaceHolder1_divacc_grvprimeaccount');
            var TargetChildControl = chkSelectAll;
            var selectall = 0;
            var Inputs = gvmail.getElementsByTagName("input");
            if (!chkSelectAccount.checked) {
                chkSelectAll.checked = false;
            }
            else {
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'checkbox') {
                        if (Inputs[n].checked) {
                            selectall = selectall + 1;
                        }
                    }
                }
                if (selectall == grvprimeaccount.rows.length - 1) {
                    chkSelectAll.checked = true;
                }
            }


        }
        function funshowaddless() {

            //document.getElementById('ctl00_ContentPlaceHolder1_tcReceipt_TabMainPage_btnAddLess').click();
            document.getElementById('<%=divAssetview.ClientID%>').collapsed = !document.getElementById('<%=divAssetview.ClientID%>').collapsed;

        }
        function funshowaddless1() {

            //document.getElementById('ctl00_ContentPlaceHolder1_tcReceipt_TabMainPage_btnAddLess').click();
            document.getElementById('<%=divAccountsummary.ClientID%>').collapsed = !document.getElementById('<%=divAccountsummary.ClientID%>').collapsed;

        }

    </script>



</asp:Content>
