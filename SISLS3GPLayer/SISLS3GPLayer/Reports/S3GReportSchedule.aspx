<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GReportSchedule.aspx.cs" Inherits="Reports_S3GReportSchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagName="PageNavigator" TagPrefix="uc1" Src="~/UserControls/PageNavigator.ascx" %>
<%--<%@ Register TagName="PageTransNavigator" TagPrefix="uc2" Src="~/UserControls/PageNavigator.ascx" %>--%>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function onCalendarShown(sender, args) {
            sender._switchMode("months", true);
            changeCellHandlers(cal1);
        }
        function FunControlCheck(sender, args) {
            var chk = document.getElementById('<%=CheckAll.ClientID%>');
            chk.checked = false;
        }
    </script>
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" Text="Report Schedule Details" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                    </td>
                </tr>
            </table>

            <asp:Panel ID="PnlInputCriteria0" GroupingText="Input Criteria" runat="server" CssClass="stylePanel">
                <table width="100%" style="height: 70%;" align="center">
                    <tr>
                        <td width="100%">
                            <table>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Report Name" ID="lblReportName" CssClass="styleReqFieldLabel"
                                            ToolTip="Report Name"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlprogramName" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlprogramName_SelectedIndexChange" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvreportname" runat="server" ControlToValidate="ddlprogramName" InitialValue="0" ValidationGroup="btngo" Display="None" ErrorMessage="Select Report Name"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Lessee Name" ID="lblCustomer" ToolTip="Lessee Name"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" Width="182px" ServiceMethod="GetLesseeNameDetails" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td class="styleFieldLabel">
                                        <span>Line of Business</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:Panel runat="server" ID="Panel1" Enabled="false" CssClass="stylePanel" ScrollBars="None" GroupingText="" Style="width: 60%; height: 100px;"
                                            BorderColor="#77B6E9" BorderWidth="1">

                                            <asp:CheckBoxList ID="ChkLSTLob" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ChkLSTLob_SelectedIndexChanged"></asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="From Date" ID="lblDate" CssClass="styleReqFieldLabel"
                                            ToolTip="From Date"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <%-- <asp:DropDownList ID="ddlDemandMonth" runat="server" Visible="true">
                                        </asp:DropDownList>--%>
                                        <asp:TextBox ID="txtFromDate" Width="182px" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calFromDate" runat="server"
                                            TargetControlID="txtFromDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtFromDate" InitialValue="" ErrorMessage="Select From Date"
                                            Display="None" ValidationGroup="btngo"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblFunderName" runat="server" Text="Funder Name" ToolTip="Funder Name" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunders" AutoPostBack="true" Width="182px" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="To Date" ID="lblToMonth" CssClass="styleReqFieldLabel"
                                            ToolTip="To Date"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtToDate" Width="182px" runat="server"></asp:TextBox>
                                        <%--<asp:DropDownList ID="ddlDemandToMonth" runat="server" Visible="true">
                                        </asp:DropDownList>--%>
                                        <cc1:CalendarExtender ID="calToDate" runat="server"
                                            TargetControlID="txtToDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvDemandToMonth" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtToDate" InitialValue="" ErrorMessage="Select To Date"
                                            Display="None" ValidationGroup="btngo"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlVendorName" runat="server" ServiceMethod="GetVendorName" WatermarkText="--All--" Width="182px"
                                            ErrorMessage="Enter Vendor Name" />
                                    </td>
                                </tr>
                                <asp:HiddenField ID="hdnProgram" runat="server" />
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Format" ID="Label1" CssClass="styleReqFieldLabel" Visible="false"
                                            ToolTip="File Format"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlfileformate" Enabled="false" Visible="false" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvFileFormate" runat="server" ControlToValidate="ddlfileformate" InitialValue="0" ValidationGroup="btngo" Display="None" ErrorMessage="Select File Format"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Schedule At" ID="lblScheduleAt" CssClass="styleReqFieldLabel"
                                            ToolTip="Schedule At"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtScheduleat" Width="182px" runat="server"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calScheduleAt" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                            runat="server"
                                            TargetControlID="txtScheduleat">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvSheduleDate" runat="server" ControlToValidate="txtScheduleat" ValidationGroup="btngo" Display="None" ErrorMessage="Select Schedule At"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblAssetCategory" runat="server" Text="Asset Category" ToolTip="AssetCategory" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlAssetCategory" runat="server" ServiceMethod="GetAssetCategoryDetails" Width="182px" WatermarkText="--All--" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Invoice Type" ID="lblInvoiceType" CssClass="styleDisplayLabel"
                                            ToolTip="Invoice Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlinvoiceType" runat="server" Width="175px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLocation" runat="server" Text="Location" ToolTip="Location" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList"
                                            WatermarkText="--All--" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblVendorInvoiceStatus" runat="server" Text="Vendor Invoice Status" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlVendorInvoiceStatus" runat="server" Width="150px" ToolTip="Vendor Invoice Status">
                                            <asp:ListItem Value="0" Text="--All--" />
                                            <asp:ListItem Value="2" Text="Received" />
                                            <asp:ListItem Value="1" Text="Not Received" />
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStatus" runat="server" Text="Invoice Status" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="150px">
                                            <asp:ListItem Value="0" Text="--All--" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Scheduled"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Not Scheduled"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblAssetStatus" runat="server" Text="Asset Status" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlAssetStatus" runat="server" ToolTip="Asset Status" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblProposalType" runat="server" Text="Proposal Type" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlProposalType" runat="server" Width="150px" ToolTip="Proposal Type">
                                        </asp:DropDownList>
                                    </td>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label ID="Label2" runat="server" Text="Rental Group" CssClass="styleDisplayLabel" />
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:DropDownList ID="ddlRentalGroup" runat="server" Width="150px">
                                                <asp:ListItem Value="1">HSN Wise</asp:ListItem>
                                                <asp:ListItem Value="2">RS Wise</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="styleFieldLabel">
                                        <asp:Label ID="lblTranche" runat="server" Text="Tranche Name"/>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <uc2:Suggest ID="ddlTranche" runat="server" ServiceMethod="GetTrancheName" WatermarkText="--All--"
                                            ErrorMessage="Enter Customer Name" IsMandatory="false"
                                            ItemToValidate="Value" AutoPostBack="true" OnItem_Selected="ddlTranche_Item_Selected" />
                                    </td>
                                    </tr>
                                </tr>
                            </table>
                        </td>

                        <td width="40%">
                            <table width="70%">
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlChekBox" Visible="false" CssClass="stylePanel" ScrollBars="Vertical" GroupingText="" Style="width: 105%; height: 280px;"
                                            BorderColor="#77B6E9" BorderWidth="1">
                                            <asp:CheckBox ID="CheckAll" runat="server" OnCheckedChanged="ChckAll_SelectedIndexChanged" Text="All" AutoPostBack="true" Checked="true"></asp:CheckBox>
                                            <asp:CheckBoxList ID="ChckBoxListLocation" onclick="FunControlCheck();" runat="server" Checked="true"></asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table align="center">
                <tr>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Schedule" ValidationGroup="btngo" OnClientClick="return fnCheckPageValidators()" CssClass="styleSubmitButton"
                            ToolTip="Go" OnClick="btnGo_Schedule" />&nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                    CausesValidation="false" ToolTip="Clear" OnClick="btnClear_Click" />
                        &nbsp;
                        <asp:Button ID="Button1" runat="server" OnClick="btn_CancelClick" Text="Cancel" CssClass="styleSubmitButton"
                            CausesValidation="false" ToolTip="Cancel" />

                    </td>
                </tr>
            </table>
            <table align="left">
                <tr>
                    <td>
                        <asp:ValidationSummary ID="lblsErrorMessage" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btngo" />
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <asp:Button ID="btncanenReq" runat="server" Text="Cancel Request" OnClick="btn_CancelRequest" CssClass="styleSubmitButton" ValidationGroup="Go"
                            CausesValidation="true" ToolTip="Cancel Request" />
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cvApplicationProcessing" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true" Width="98%" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

