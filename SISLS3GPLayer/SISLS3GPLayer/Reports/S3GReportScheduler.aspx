<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GReportScheduler.aspx.cs" Inherits="Reports_ReportScheduler" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagName="PageNavigator" TagPrefix="uc1" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagName="PageTransNavigator" TagPrefix="uc2" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function Location_ItemSelected(sender, e) {
            var hdnLocationID = $get('<%= hdnLocationID.ClientID %>');
            hdnLocationID.value = e.get_value();
        }
        function Location_ItemPopulated(sender, e) {
            var hdnLocationID = $get('<%= hdnLocationID.ClientID %>');
           hdnLocationID.value = '';
       }

    </script>
    <table width="100%">
        <tr>
            <td valign="top" class="stylePageHeading">
                <asp:Label runat="server" Text="Report Scheduler" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
            </td>
        </tr>
    </table>
    <asp:Panel ID="PnlInputCriteria0" GroupingText="Input Criteria" runat="server" CssClass="stylePanel">
        <table width="100%">
            <tr>
                <td class="styleFieldLabel" style="width: 5%;">
                    <span>Scheduled From Date</span>
                </td>
                <td class="styleFieldAlign" style="width: 10%;">
                    <asp:TextBox ID="txtFromDate" Width="182px" ValidationGroup="btngo" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                </td>
                <td class="styleFieldLabel" style="width: 5%;">
                    <asp:Label runat="server" Text="Scheduled To Date" ID="lblDateTo"
                        ToolTip="To Date"></asp:Label>
                </td>
                <td class="styleFieldAlign" style="width: 10%;">
                    <asp:TextBox ID="txtToDate" Width="182px" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="txtToDate" Format="dd/MM/yyyy">
                    </cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td class="styleFieldLabel" style="width: 5%;">
                    <asp:Label runat="server" Text="Requester" ID="lblDate"
                        ToolTip="Date From"></asp:Label>
                </td>
                <td class="styleFieldAlign" style="width: 10%;">
                    <asp:TextBox ID="txtUserName" Width="182px" runat="server" AutoPostBack="true" OnTextChanged="txtUserName_TextChanged"></asp:TextBox>
                </td>
                <cc1:AutoCompleteExtender ID="autoUserName" MinimumPrefixLength="1" EnableCaching="false" OnClientPopulated="Location_ItemPopulated"
                    OnClientItemSelected="Location_ItemSelected" runat="server" TargetControlID="txtUserName"
                    ServiceMethod="GetUserName" Enabled="True" ServicePath="" CompletionSetCount="5"
                    CompletionListCssClass="CompletionList" DelimiterCharacters=";,:" CompletionListItemCssClass="CompletionListItemCssClass"
                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                    ShowOnlyCurrentWordInCompletionListItem="true">
                </cc1:AutoCompleteExtender>
                <cc1:TextBoxWatermarkExtender ID="txtBranchExtender" runat="server" TargetControlID="txtUserName"
                    WatermarkText="--Select--">
                </cc1:TextBoxWatermarkExtender>
                <td class="styleFieldLabel" style="width: 5%;">
                    <span>Report Name</span>
                </td>
                <td class="styleFieldAlign" style="width: 10%;">
                    <asp:DropDownList ID="ddlprogramName" Width="200px" runat="server"></asp:DropDownList>
                </td>

            </tr>
        </table>
    </asp:Panel>
    <table align="center">
        <tr>
            <td>
                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="styleSubmitButton" ValidationGroup="btngo"
                    OnClick="btnGo_Click" ToolTip="Go" />&nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();"
                    CausesValidation="false" ToolTip="Clear" OnClick="btnClear_Click1" />&nbsp;
                <asp:Button ID="btnAddSchedule" runat="server" Text="Add Schedule" OnClick="btn_Schedule_Onclick" CssClass="styleSubmitButton"
                    CausesValidation="false" ToolTip="Add Schedule" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="Panel1" GroupingText="Report Schedule Details" runat="server" Visible="false"
        CssClass="stylePanel">
        <asp:GridView ID="gvScheduleDetails" runat="server" EmptyDataText="No Disbursal Details Found!..."
            AutoGenerateColumns="false" OnRowCommand="GrdUsers_RowCommand" OnRowDataBound="GrdUsers_RowDataBound"
            Width="100%">
            <RowStyle HorizontalAlign="Left" />
            <HeaderStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>RequesterId</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="ReqIdhyplnkView" runat="server" Text='<%#Bind("RequesterId") %>' OnClick="LnkReqIdView_Click">DownLoad</asp:LinkButton>
                        <asp:Label ID="lblReport_Id" runat="server" Text='<%#Bind("REPORT_ID") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>ReportName</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblReport_Name" runat="server" Text='<%#Bind("REPORT_NAME") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>Requester Name</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblRequesterName" runat="server" Text='<%#Bind("Requester_Name") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>Date</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%#Bind("C_Date") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>Schedule At</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblSchedule" runat="server" Text='<%#Bind("ShceduleAt") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>From Date</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFromDate" runat="server" Text='<%#Bind("FROM_DATE") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>To Date</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblToDate" runat="server" Text='<%#Bind("TO_DATE") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>Status</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%#Bind("Status") %>'></asp:Label>
                        <asp:Label ID="statusId" Visible="false" runat="server" Text='<%#Bind("Status_Id") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                    HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <span>Download Link</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lbldownload" Visible="false" runat="server" Text='<%#Bind("DownLoad") %>'></asp:Label>
                        <asp:LinkButton ID="ImghyplnkView" runat="server" OnClick="ImghyplnkView_Click">Download</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader"></HeaderStyle>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
        <asp:ValidationSummary ID="vsApplicationProcessing" runat="server" CssClass="styleMandatoryLabel"
            Enabled="true" Width="98%" ShowMessageBox="true" ShowSummary="false" ValidationGroup="btngo"
            HeaderText="Correct the following validation(s):  " />
        <asp:HiddenField ID="hdnLocationID" runat="server" />
    </asp:Panel>
</asp:Content>
