<%@ Page Title="Repossession Report" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_RPT_Repossession.aspx.cs"
    Inherits="Reports_S3G_RPT_Repossession" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Repossession Report">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                        <asp:Panel runat="server" ID="PnlRepreport" CssClass="stylePanel" GroupingText="INPUT CRITERIA"
                            Width="100%">
                            <table width="100%">

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel" ToolTip="Line of Business"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLOB" runat="server" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" AutoPostBack="True" ValidationGroup="btnGo" ToolTip="Line of Business">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Line of Business"
                                            InitialValue="-1" ValidationGroup="btnGo" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblregion" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true" ToolTip="Location"
                                            OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                            IsMandatory="false" ErrorMessage="Select Location" ValidationGroup="btnGo" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRepReport" runat="server" CssClass="styleReqFieldLabel" Text="Repossession Type" ToolTip="Repossession Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlRepoType" runat="server" OnSelectedIndexChanged="ddlRepoType_SelectedIndexChanged"
                                            AutoPostBack="True" ValidationGroup="btnGo" ToolTip="Repossession Type">
                                            <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Repossessed" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Released" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Sold" Value="4"></asp:ListItem>
                                        </asp:DropDownList>

                                        <asp:RequiredFieldValidator ID="rfvRepoType" runat="server" ControlToValidate="ddlRepoType"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Repossession Type"
                                            InitialValue="-1" ValidationGroup="btnGo" SetFocusOnError="True"></asp:RequiredFieldValidator>

                                        <%--<asp:CheckBox ID="chkRepossession" runat="server" Checked="true" OnCheckedChanged="chkRepossession_CheckedChanged" AutoPostBack="true" ToolTip="Repossession" />
                                        <asp:Label ID="lblAbstract" runat="server" CssClass="styleDisplayLabel" Text="Repossessed" ToolTip="Repossessed"></asp:Label>
                                        <asp:CheckBox ID="chkReleased" runat="server" OnCheckedChanged="chkReleased_CheckedChanged" AutoPostBack="true" ToolTip="Released" />
                                        <asp:Label ID="lblReleased" runat="server" CssClass="styleDisplayLabel" Text="Released" ToolTip="Released"></asp:Label>
                                        <asp:CheckBox ID="chkReSold" runat="server" OnCheckedChanged="chkReSold_CheckedChanged" AutoPostBack="true" ToolTip="Sold" />
                                        <asp:Label ID="lblResold" runat="server" CssClass="styleDisplayLabel" Text="Sold" ToolTip="Sold"></asp:Label>--%>

                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStartDateSearch" runat="server" CssClass="styleReqFieldLabel" Text="From Date" ToolTip="From Date" />
                                        <%--  <span class="styleMandatory">*</span>--%>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <input id="hidDate" runat="server" type="hidden" />
                                        <asp:TextBox ID="txtStartDateSearch" runat="server" OnTextChanged="txtStartDateSearch_TextChanged" AutoPostBack="true" Width="100" ToolTip="From Date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="None" ControlToValidate="txtStartDateSearch"
                                            ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select From Date"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                            PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>


                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblGarage" runat="server" CssClass="styleDisplayLabel" Text="Garage Wise"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlGaragecNo" runat="server" ServiceMethod="GetGarageWise" AutoPostBack="true" ToolTip="Garage Wise"
                                            OnItem_Selected="ddlGaragecNo_SelectedIndexChanged" IsMandatory="false" />

                                    </td>

                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblEndDateSearch" runat="server" CssClass="styleReqFieldLabel" Text="To Date" ToolTip="To Date" />
                                        <%-- <span class="styleMandatory">*</span>--%>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtEndDateSearch" runat="server" OnTextChanged="txtEndDateSearch_TextChanged" AutoPostBack="true" Width="100" ToolTip="To Date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" Display="None" ControlToValidate="txtEndDateSearch"
                                            ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select To Date"
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

                <tr>
                   <td align="center">
                        <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitLongButton" Text="Go" OnClick="btnGo_Click"
                            ValidationGroup="btnGo" OnClientClick="return fnCheckPageValidators('btnGo',false);" ToolTip="Go" />&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitLongButton" Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" ToolTip="Clear" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="PnlDetailedView" runat="server" CssClass="stylePanel" GroupingText="Repossession Detailed Report" Width="1024px" Wrap="true" ScrollBars="Horizontal" >
                            <asp:GridView ID="GrvDetails" runat="server" AutoGenerateColumns="False"
                                CssClass="styleInfoLabel" ShowFooter="false" OnRowDataBound="GrvDetails_RowDataBound" Width="99%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Account No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPrimeAccount" runat="server" Text='<%# Bind("PANUM") %>' ToolTip="Account No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sub Account No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubAccount" runat="server" Text='<%# Bind("SANUM") %>' ToolTip="Sub Account No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("CUSTOMER_NAME") %>' ToolTip="Customer"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetDesc" runat="server" Text='<%# Bind("ASSET_DESCRIPTION") %>' ToolTip="Asset Description"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vehicle No./Serial No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVehicle" runat="server" Text='<%# Bind("REGN_NUMBER") %>' ToolTip="Vehicle No"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Repossession Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRepoDate" runat="server" Text='<%# Bind("Repossession_Date") %>' ToolTip="Repossession Date"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Repossession Place">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPlace" runat="server" Text='<%# Bind("Repossession_Place") %>' ToolTip="Repossession Place"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Garage Name1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGarageName1" runat="server" Width="150px" Rows="4" Text='<%# Bind("Garage_Address1") %>' ToolTip="Garage Name1"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Current Garage" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="txtGarageAddress" runat="server" Text='<%# Bind("Garage_Address") %>' Width="150px" Rows="4" ToolTip="Garage Address"></asp:Label>
                                            <%--<asp:TextBox ID="TextBox1" CssClass="textmode" runat="server" Width="205px" Rows="4" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>--%>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Asset Condition">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAssetCongn" runat="server" Text='<%# Bind("Asset_Condition") %>' ToolTip="Asset Condition"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Release Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReleaseDate" runat="server" Text='<%# Bind("Release_Date") %>' ToolTip="Release Date"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sold Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSoldDate" runat="server" Text='<%# Bind("Sold_Date") %>' ToolTip="Sold Date"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                   
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnPrint" CssClass="styleSubmitLongButton" runat="server" Text="Export Excel" OnClick="btnPrint_Click" ToolTip="Print" />
                        <%--&nbsp;<asp:Button ID="btnCancel" ToolTip="Goto translander page" runat="server" CausesValidation="false"
                                            CssClass="styleSubmitButton" Text="Cancel" OnClick="btnCancel_Click" />--%>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ID="vsDis" runat="server" CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):"
                            ShowSummary="true" ValidationGroup="btnGo" />
                    </td>
                </tr>
                <%-- <tr>
                    <td>
                        <asp:CustomValidator ID="CVDisbursement" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>--%>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
        </Triggers>
    </asp:UpdatePanel>
    <%-- <script language="javascript" type="text/javascript">
        function Resize() {
            if (document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null) {               
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - 270;
                
            }
        }
    </script>--%>
    <%--<script language="javascript" type="text/javascript">
        function pageLoad(s, a) {
            document.getElementById('<%=gvGrigPDDT.ClientID %>').style.width = parseInt(screen.width) - 260;
            document.getElementById('<%=gvGrigPDDT.ClientID %>').style.overflow = "scroll";
        }
             </script>--%>
</asp:Content>

