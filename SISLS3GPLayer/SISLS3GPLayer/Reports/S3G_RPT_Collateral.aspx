<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3G_RPT_Collateral.aspx.cs" Inherits="Reports_S3G_RPT_Collateral" Title="Collateral Report" %>

<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Collateral Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="PnlInputCriteria" runat="server" GroupingText="Input Criteria" CssClass="stylePanel">
                    <table cellpadding="0" cellspacing="0" style="width: 100%" border="0">
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"
                                    ToolTip="Line of Business">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlLOB" runat="server" Width="75%" AutoPostBack="true" ToolTip="Line of Business">
                                    <%--onselectedindexchanged="ddlLOB_SelectedIndexChanged">--%>
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblLocation1" Text="Location 1" CssClass="styleDisplayLabel"
                                    ToolTip="Location 1"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlLocation1" runat="server" Width="75%" AutoPostBack="true"
                                    ToolTip="Location 1" OnSelectedIndexChanged="ddlLocation1_SelectedIndexChanged">
                                    <%--onselectedindexchanged="ddlLocation1_SelectedIndexChanged">--%>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label ID="lbllocation2" runat="server" Text="Location 2" CssClass="styleDisplayLabel"
                                    ToolTip="Location 2">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddllocation2" runat="server" AutoPostBack="true" Visible="true"
                                    Width="75%" ToolTip="Location 2">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Customer Name"
                                    ToolTip="Customer Name"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtCustomerName" runat="server" Style="display: none;" Width="100%"
                                    CausesValidation="true"></asp:TextBox>
                                <uc2:LOV ID="ucCustomerCodeLov" onfocus="return fnLoadCustomer();" runat="server"
                                    strLOV_Code="CMD" />
                                <asp:Button ID="btnLoadCustomer" OnClick="btnLoadCustomer_OnClick" runat="server"
                                    Text="Load Customer" Style="display: none" ToolTip="Customer Name" />
                                <input id="hdnCustID" type="hidden" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvcustname" runat="server" ControlToValidate="txtCustomerName"
                                    ErrorMessage="Select a Customer Name" Display="None" ValidationGroup="Go" Enabled="false">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblCollateralType" CssClass="styleDisplayLabel" Text="Collateral Type"
                                    ToolTip="Collateral Type"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlCollateralType" runat="server" Width="75%" AutoPostBack="true"
                                    ToolTip="Collateral Type">
                                    <%--OnSelectedIndexChanged="ddlCollateralType_SelectedIndexChanged">--%>
                                </asp:DropDownList>                                
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblCollateralStatus" Text="Collateral Status" CssClass="styleDisplayLabel"
                                    ToolTip="Collateral Status"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlCollateralStatus" runat="server" Width="75%" AutoPostBack="true"
                                    ToolTip="Collateral Status">                                    
                                    <asp:ListItem Text="All" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Available" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Released" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Sold" Value="4"></asp:ListItem>
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="From Report Date" ID="lblFromReportDate" CssClass="styleReqFieldLabel"
                                    ToolTip="From Report Date" />
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtFromReportDate" runat="server" Width="50%" AutoPostBack="false"
                                    ToolTip="From Report Date"></asp:TextBox>
                                <asp:Image ID="imgFromReportDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                    ToolTip="From Report Date" />
                                <cc1:CalendarExtender ID="ceFromReportDate" runat="server" Enabled="True" PopupButtonID="imgFromReportDate"
                                    TargetControlID="txtFromReportDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="RfvFromReportDate" runat="server" ValidationGroup="Go"
                                    CssClass="styleMandatoryLabel" Display="none" ErrorMessage="Enter From Report Date"
                                    SetFocusOnError="True" ControlToValidate="txtFromReportDate">RfvStartDate</asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="To Report Date" ID="lblToReportDate" CssClass="styleReqFieldLabel"
                                    ToolTip="To Report Date" />
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtToReportDate" runat="server" Width="50%" AutoPostBack="false"
                                    ToolTip="To Report Date"></asp:TextBox>
                                <asp:Image ID="ImgToReportDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                    ToolTip="To Report Date" />
                                <cc1:CalendarExtender ID="ceToReportDate" runat="server" Enabled="True" PopupButtonID="ImgToReportDate"
                                    TargetControlID="txtToReportDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvToReportDate" runat="server" ValidationGroup="Go"
                                    CssClass="styleMandatoryLabel" Display="none" ErrorMessage="Enter To Report Date"
                                    SetFocusOnError="True" ControlToValidate="txtToReportDate">RfvStartDate</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="10px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td height="8px" align="center">
                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="styleSubmitButton" ValidationGroup="Go"
                    CausesValidation="true" ToolTip="Go" OnClick="btnGo_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                    CausesValidation="false" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" ToolTip="Clear" />
            </td>
        </tr>
        <tr>
            <td height="10px">
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:Label ID="lblAmounts" runat="server" Text="All Amounts are in" Visible="false"
                    CssClass="styleDisplayLabel"></asp:Label>
                <asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlCollateraldetails" runat="server" CssClass="stylePanel" GroupingText="Collateral Details"
                    Width="100%">
                    <div class="container" style="height: 200px; overflow: auto;">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grvCollateraldetails" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="Customer Name"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ref.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReferenceNo" runat="server" Text='<%# Bind("ReferenceNo") %>'
                                                    ToolTip="Ref.No."></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Desc.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetDesc" runat="server" Text='<%# Bind("AssetDesc") %>' ToolTip="Asset Desc."></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Units">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnits" runat="server" Text='<%# Bind("Units") %>' ToolTip="Units"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>' ToolTip="Value"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="City">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>' ToolTip="City"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server" Text='<%# Bind("Address") %>' ToolTip="Address"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Storage1">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStorage1" runat="server" Text='<%# Bind("Storage1") %>' ToolTip="Storage1"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Storage2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStorage2" runat="server" Text='<%# Bind("Storage2") %>' ToolTip="Storage2"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Storage3">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStorage3" runat="server" Text='<%# Bind("Storage3") %>' ToolTip="Storage3"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' ToolTip="Status"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="StatusDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusDate" runat="server" Text='<%# Bind("StatusDate") %>' ToolTip="StatusDate"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="10px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td align="center">                
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="styleSubmitButton"
                    CausesValidation="false" OnClick="btnPrint_Click" ValidationGroup="Print" Visible="false" ToolTip="Print" />                
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary runat="server" ID="VSPDCAcknow" HeaderText="Please correct the following validation(s):"
                    CssClass="styleSummaryStyle" ShowMessageBox="false" ValidationGroup="Go" ShowSummary="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVPDCAcknowledgement" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        function fnLoadCustomer() {
            document.getElementById("<%= btnLoadCustomer.ClientID %>").click();

        }
    </script>

</asp:Content>
