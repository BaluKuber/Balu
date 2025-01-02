<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptPayment_DemandCollection_NPA.aspx.cs" Inherits="Reports_S3GRptPayment_DemandCollection_NPA" Title="Payment_DemandCollection_NPA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Payment Demand Collection NPA Report">
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
                                <asp:Label runat="server" Text="Location1" ID="lblRegion" CssClass="styleDisplayLabel" ToolTip="Location1">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlRegion" runat="server" Width="65%" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" ToolTip="Location1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location2" ID="lblBranch" CssClass="styleDisplayLabel" ToolTip="Location2">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlBranch" runat="server" Width="65%" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ToolTip="Location2" Enabled="false">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Product" ID="lblProduct" CssClass="styleDisplayLabel" ToolTip="Product">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" Width="65%" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" ToolTip="Product">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Start Month/Year" ID="lblStartMonth" CssClass="styleReqFieldLabel" ToolTip="Start Month/Year">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtStartMonth" runat="server" OnTextChanged="txtStartMonth_OnTextChanged" AutoPostBack="true" ToolTip="Start Month/Year"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgStartMonth" BehaviorID="calendar1" Format="MM/yyyy" TargetControlID="txtStartMonth" OnClientShown="onCalendarShown">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgStartMonth" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <asp:RequiredFieldValidator ID="rfvStartMonth" runat="server" ErrorMessage="Select Start Month/Year." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtStartMonth"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="End Month/Year" ID="lblEndMonth" CssClass="styleDisplayLabel" ToolTip="End Month/Year">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtEndMonth" runat="server" AutoPostBack="true" OnTextChanged="txtEndMonth_OnTextChanged" ToolTip="End Month/Year"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" BehaviorID="calendar2" Format="MM/yyyy" TargetControlID="txtEndMonth" PopupButtonID="imgEndMonth" OnClientShown="onCalendarShown">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgEndMonth" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            </td>
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination" CssClass="styleDisplayLabel" ToolTip="Denomination">
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
            <td align="center">
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" CausesValidation="true" OnClientClick="return fnCheckPageValidators('Ok',false);" ValidationGroup="Ok" OnClick="btnOk_Click" ToolTip="Go" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" ToolTip="Clear" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlDetails" runat="server" CssClass="stylePanel" GroupingText="Payment Demand Collection NPA Details" Width="100%" Visible="false">
                    <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <div id="divDetails" runat="server" style="overflow: auto; height: 300px; display: none">
                        <asp:GridView ID="grvDetails" runat="server" Width="100%" AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true" OnRowDataBound="grvDetails_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonth" runat="server" Text='<%# Bind("Month") %>' ToolTip="Month"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrndTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LOBId" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBId" runat="server" Text='<%# Bind("LobId") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line Of Business" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("Lob") %>' ToolTip="Line Of Business"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RegionId" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegionId" runat="server" Text='<%# Bind("RegionId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegion" runat="server" Text='<%# Bind("Region") %>' ToolTip="Region"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="BranchId" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchId" runat="server" Text='<%# Bind("BranchId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Branch" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>' ToolTip="Branch"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="ProductId" ItemStyle-HorizontalAlign="Left" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("ProductId") %>' ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("Product") %>' ToolTip="Product"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Growth % Last Month" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrowthLastMonth" runat="server" Text='<%# Bind("GrowthPercentageLastMonth") %>' ToolTip="Growth Percentage Last Month"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotGrowthLastMonth" runat="server" ToolTip="Total Growth Percentage Last Month"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cumulative" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCumulative" runat="server" Text='<%# Bind("Cumulative") %>' ToolTip="Cumulative"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotCumulative" runat="server" ToolTip="Total Cumulative"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Growth % Last Year" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGrowthLastYear" runat="server" Text='<%# Bind("GrowthPercentageLastYear") %>' ToolTip="Growth Percentage Last Year"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotGrowthLastYear" runat="server" ToolTip="Total Growth Percentage Last Year"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Arrear Demand" ItemStyle-HorizontalAlign="Right">
                                    <%--<HeaderTemplate>
                                        <table border="1" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -2px; margin-bottom: -4px">
                                            <tr>
                                                <td colspan="11">
                                                    Demand/Collection
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Arrear Demand
                                                </td>
                                                <td>
                                                    Arrear Col
                                                </td>
                                                <td>
                                                    %
                                                </td>
                                                <td>
                                                    Current Demand
                                                </td>
                                                <td>
                                                    Current Col
                                                </td>
                                                <td>
                                                    %
                                                </td>
                                                <td>
                                                    Tot Demand
                                                </td>
                                                <td>
                                                    Tot Col
                                                </td>
                                                <td>
                                                    %
                                                </td>
                                                <td>
                                                    Bad Debts
                                                </td>
                                                <td>
                                                    Stock
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table border="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblOpeningDemand" runat="server" Text='<%# Bind("ArrearDemand") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblOpeningCollection" runat="server" Text='<%# Bind("ArrearCollection") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblOpeningpercentage" runat="server" Text='<%# Bind("ArrearCollectionPercentage") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMonthlyDemand" runat="server" Text='<%# Bind("CurrentDemand") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMonthlyCollection" runat="server" Text='<%# Bind("CurrentCollection") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblMonthlypercentage" runat="server" Text='<%# Bind("CurrentPercentage") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClosingDemand" runat="server" Text='<%# Bind("TotalDemand") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClosingCollection" runat="server" Text='<%# Bind("TotalCollection") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClosingpercentage" runat="server" Text='<%# Bind("TotalPercentage") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBadDebts" runat="server" Text='<%# Bind("BadDebts") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblStock" runat="server" Text='<%# Bind("Stock") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <table border="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTotOpeningDemand" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotOpeningCollection" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotOpeningpercentage" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotMonthlyDemand" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotMonthlyCollection" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotMonthlypercentage" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotClosingDemand" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotClosingCollection" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotClosingpercentage" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotBadDebts" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotStock" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>--%>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpeningDemand" runat="server" Text='<%# Bind("ArrearDemand") %>' ToolTip="Arrear Demand"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotOpeningDemand" runat="server" ToolTip="Total Arrear Demand"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Arrear Collection" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpeningCollection" runat="server" Text='<%# Bind("ArrearCollection") %>' ToolTip="Arrear Collection"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotOpeningCollection" runat="server" ToolTip="Total Arrear Collection"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Arrear Collection %" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOpeningpercentage" runat="server" Text='<%# Bind("ArrearCollectionPercentage") %>' ToolTip="Arrear Collection Percentage"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotOpeningpercentage" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Demand" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonthlyDemand" runat="server" Text='<%# Bind("CurrentDemand") %>' ToolTip="Current Demand"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotMonthlyDemand" runat="server" ToolTip="Total Current Demand"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Collection" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonthlyCollection" runat="server" Text='<%# Bind("CurrentCollection") %>' ToolTip="Current Collection"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotMonthlyCollection" runat="server" ToolTip="Total Current Collection"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Current Collection %" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMonthlypercentage" runat="server" Text='<%# Bind("CurrentPercentage") %>' ToolTip="Current Collection Percentage"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotMonthlypercentage" runat="server" ToolTip="Total Current Collection Percentage"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Demand" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingDemand" runat="server" Text='<%# Bind("TotalDemand") %>' ToolTip="Total Demand"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotClosingDemand" runat="server" ToolTip="Total Demand"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Collection" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingCollection" runat="server" Text='<%# Bind("TotalCollection") %>' ToolTip="Total Collection"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotClosingCollection" runat="server" ToolTip="Total Collection"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Collection %" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingpercentage" runat="server" Text='<%# Bind("TotalPercentage") %>' ToolTip="Total Collection Percentage"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotClosingpercentage" runat="server" ToolTip="Total Collection Percentage"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Bad Debts" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBadDebts" runat="server" Text='<%# Bind("BadDebts") %>' ToolTip="Bad Debts"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotBadDebts" runat="server" ToolTip="Total Bad Debts"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stock" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStock" runat="server" Text='<%# Bind("Stock") %>' ToolTip="Stock"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotStock" runat="server" ToolTip="Total Stock"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NPA" ItemStyle-HorizontalAlign="Right">
                                    <%-- <HeaderTemplate>
                                        <table border="1" width="100%" cellpadding="0" cellspacing="0" style="margin-top: -2px; margin-bottom: -4px">
                                            <tr>
                                                <td colspan="4">
                                                    NPA
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    NPA
                                                </td>
                                                <td>
                                                    NPA%
                                                </td>
                                                <td>
                                                    Clossing Arrear
                                                </td>
                                                <td>
                                                    Arrear%
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table border="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblNpa" runat="server" Text='<%# Bind("Npa") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNpaPercentage" runat="server" Text='<%# Bind("NpaPercentage") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClosingArrear" runat="server" Text='<%# Bind("ClosingArrear") %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblArrearPercentage" runat="server" Text='<%# Bind("ArrearPercentage") %>'></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <table border="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTotNpa" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotNpaPercentage" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotClosingArrear" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblTotArrearPercentage" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>--%>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNpa" runat="server" Text='<%# Bind("Npa") %>' ToolTip="NPA"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotNpa" runat="server" ToolTip="Total NPA"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="NPA %" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNpaPercentage" runat="server" Text='<%# Bind("NpaPercentage") %>' ToolTip="NPA Percentage"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotNpaPercentage" runat="server" ToolTip="Total NPA Percentage"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Closing Arrear" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingArrear" runat="server" Text='<%# Bind("ClosingArrear") %>' ToolTip="Closing Arrear"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotClosingArrear" runat="server" ToolTip="Total Closing Arrear"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Arrear %" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArrearPercentage" runat="server" Text='<%# Bind("ArrearPercentage") %>' ToolTip="Arrear Percentage"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotArrearPercentage" runat="server" ToolTip="Total Arrear Percentage"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        
        <tr>
            <td align="left">
                <asp:Label ID="lblErrorMsg" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td align="center">
                <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click" ToolTip="Print" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="VSPayment" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVPayment" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>

    <script type="text/javascript"> 
 
        var cal1; 
        var cal2; 
 
       function pageLoad() { 
            cal1 = $find("calendar1"); 
            cal2 = $find("calendar2"); 
 
            modifyCalDelegates(cal1); 
            modifyCalDelegates(cal2); 
        } 
 
        function modifyCalDelegates(cal) { 
            //we need to modify the original delegate of the month cell. 
            cal._cell$delegates = { 
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover), 
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout), 
 
                click: Function.createDelegate(cal, function(e) { 
                    /// <summary>  
                    /// Handles the click event of a cell 
                    /// </summary> 
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param> 
 
                    e.stopPropagation(); 
                    e.preventDefault(); 
 
                    if (!cal._enabled) return; 
 
                    var target = e.target; 
                    var visibleDate = cal._getEffectiveVisibleDate(); 
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover"); 
                    switch (target.mode) { 
                        case "prev": 
                        case "next": 
                            cal._switchMonth(target.date); 
                            break; 
                        case "title": 
                            switch (cal._mode) { 
                                case "days": cal._switchMode("months"); break; 
                                case "months": cal._switchMode("years"); break; 
                            } 
                            break; 
                        case "month": 
                            //if the mode is month, then stop switching to day mode. 
                            if (target.month == visibleDate.getMonth()) { 
                                //this._switchMode("days"); 
                            } else { 
                                cal._visibleDate = target.date; 
                                //this._switchMode("days"); 
                            } 
                            cal.set_selectedDate(target.date); 
                            cal._switchMonth(target.date); 
                            cal._blur.post(true); 
                            cal.raiseDateSelectionChanged(); 
                            break; 
                        case "year": 
                            if (target.date.getFullYear() == visibleDate.getFullYear()) { 
                                cal._switchMode("months"); 
                            } else { 
                                cal._visibleDate = target.date; 
                                cal._switchMode("months"); 
                            } 
                            break; 
 
                        //                case "day":                             
                        //                    this.set_selectedDate(target.date);                             
                        //                    this._switchMonth(target.date);                             
                        //                    this._blur.post(true);                             
                        //                    this.raiseDateSelectionChanged();                             
                        //                    break;                             
                        case "today": 
                            cal.set_selectedDate(target.date); 
                            cal._switchMonth(target.date); 
                            cal._blur.post(true); 
                            cal.raiseDateSelectionChanged(); 
                            break; 
                    } 
 
                }) 
            } 
 
        } 
 
        function onCalendarShown(sender, args) { 
            //set the default mode to month 
            sender._switchMode("months", true); 
            changeCellHandlers(cal1); 
        } 
 
 
        function changeCellHandlers(cal) { 
 
            if (cal._monthsBody) { 
 
                //remove the old handler of each month body. 
                for (var i = 0; i < cal._monthsBody.rows.length; i++) { 
                    var row = cal._monthsBody.rows[i]; 
                    for (var j = 0; j < row.cells.length; j++) { 
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates); 
                    } 
                } 
                //add the new handler of each month body. 
                for (var i = 0; i < cal._monthsBody.rows.length; i++) { 
                    var row = cal._monthsBody.rows[i]; 
                    for (var j = 0; j < row.cells.length; j++) { 
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates); 
                    } 
                } 
 
            } 
        } 
 
        function onCalendarHidden(sender, args) { 
 
            if (sender.get_selectedDate()) { 
                if (cal1.get_selectedDate() && cal2.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) { 
                    alert('Start Month/Year cannot be Greater than the End Month/Year, please reselect!'); 
                    sender.show(); 
                    return; 
                } 
                //get the final date 
                var finalDate = new Date(sender.get_selectedDate()); 
                var selectedMonth = finalDate.getMonth(); 
                finalDate.setDate(1); 
                if (sender == cal2) { 
                    // set the calender2's default date as the last day 
                    finalDate.setMonth(selectedMonth + 1); 
                    finalDate = new Date(finalDate - 1); 
                } 
                //set the date to the TextBox 
                sender.get_element().value = finalDate.format(sender._format); 
            } 
        } 
        
        
        function Resize()
     {
       if(document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
       {
         if(document.getElementById('divMenu').style.display=='none')
            {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
         else
           {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - 270;
           }
        }  
      }
      
      
    function showMenu(show) 
      {
       if (show == 'T') 
         {
        
          if(document.getElementById('divGrid1')!=null)
            {
                document.getElementById('divGrid1').style.width="800px";
                document.getElementById('divGrid1').style.overflow="scroll";
            }
        
            document.getElementById('divMenu').style.display = 'Block';
            document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

            document.getElementById('ctl00_imgShowMenu').style.display = 'none';
            document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

            if(document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
            (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - 270;
        }
        if (show == 'F') 
        {
            if(document.getElementById('divGrid1')!=null)
                {
                    document.getElementById('divGrid1').style.width="960px";
                    document.getElementById('divGrid1').style.overflow="auto";
                }
                
            document.getElementById('divMenu').style.display = 'none';
            document.getElementById('ctl00_imgHideMenu').style.display = 'none';
            document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
           
            if(document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
           (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
        }
    }
 
    </script>

</asp:Content>
