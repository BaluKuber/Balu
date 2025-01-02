<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminUserLoginDetails.aspx.cs" Inherits="System_Admin_S3GSysAdminUserLoginDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagName="PageNavigator" TagPrefix="uc1" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagName="PageTransNavigator" TagPrefix="uc2" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div runat="server">
        <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />
    </div>

    <script type="text/javascript">

        function UserName_ItemSelected(sender, e) {
            var hdnBranchID = $get('<%= hdnUserName.ClientID %>');
            hdnBranchID.value = e.get_value();
            document.getElementById('<%= BtnRefresh.ClientID %>').click();
        }
        function UserName_ItemPopulated(sender, e) {
            var hdnBranchID = $get('<%= hdnUserName.ClientID %>');
            hdnBranchID.value = '';
        }
        function Removetext(e) {
            document.getElementById('<%= hdnUserName.ClientID %>').value = e.value;

            if (e.value == "" || event.keyCode == 13) {
                document.getElementById('<%= BtnRefresh.ClientID %>').click();
            }
        }
        function ShowTrans() {
            document.getElementById('<%= PnlUserTrans.ClientID %>').style.display = 'block';
        }

       
       
    </script>

    <%--<asp:Timer ID="timer" runat="server" Interval="600000" Enabled="true" OnTick="Timer_Tick">
    </asp:Timer>--%>
    <table width="100%">
        <tr>
            <td valign="top" class="stylePageHeading">
                <asp:Label runat="server" Text="User Login Details" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="Up" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PnlInputCriteria0" GroupingText="Input Criteria" runat="server" CssClass="stylePanel">
                <table width="100%">
                    <tr>
                        <td class="styleFieldLabel" style="width: 5%;">
                            <span>Location</span>
                        </td>
                        <td class="styleFieldAlign" style="width: 10%;">
                            <asp:DropDownList ID="ddlLocation" runat="server" OnSelectedIndexChanged="ddlLocation_OnSelectedIndexChanged"
                                AutoPostBack="true" Width="220px">
                            </asp:DropDownList>
                        </td>
                        <td class="styleFieldAlign" style="width: 8%;">
                            <asp:RadioButtonList ID="RBList" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                OnSelectedIndexChanged="RBList_SelectedIndexChanged">
                                <asp:ListItem Text="Active Users" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="All Users" Value="Yes"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="styleFieldLabel" style="width: 5%;">
                            <span>Active Users Count</span>
                        </td>
                        <td class="styleFieldAlign" style="width: 3%;">
                            <asp:TextBox ID="txtUserCount" runat="server" Text="0" Enabled="false" Width="60px"
                                Style="vertical-align: middle;"></asp:TextBox>
                        </td>
                        <td style="width: 7%;">
                            <asp:Button ID="BtnRefresh" runat="server" Text="Go" CssClass="styleSubmitButton"
                                OnClick="BtnRefresh_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlDetails" GroupingText="Details" runat="server" CssClass="stylePanel">
                <table width="100%">
                    <tr>
                        <td>
                            <div style="height: 290px; overflow-y: scroll;">
                                <asp:GridView ID="GrdUsers" DataKeyNames="Pagecounts,Company_ID,User_Name,Is_LoggedIn,User_ID,Is_CurrentlyActive"
                                    runat="server" AutoGenerateColumns="false" OnRowCommand="GrdUsers_RowCommand"
                                    OnRowDataBound="GrdUsers_RowDataBound" Width="100%">
                                    <RowStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <Columns>
                                        <asp:BoundField HeaderText="User Code" DataField="User_Code" HeaderStyle-CssClass="styleGridHeader"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <span>User Name</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox oncontextmenu="return false;" onpaste="return false;" ID="txtUserNameSearch"
                                                                runat="server" CssClass="styleSearchBox" MaxLength="50" onkeyup="Removetext(this);"
                                                                OnTextChanged="txtUserNameSearch_OnTextChanged" ToolTip="Enter minimum 3 characters to begin search"
                                                                AutoPostBack="true" Width="182px"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="autoBranchSearch" MinimumPrefixLength="3" OnClientPopulated="UserName_ItemPopulated"
                                                                OnClientItemSelected="UserName_ItemSelected" runat="server" TargetControlID="txtUserNameSearch"
                                                                ServiceMethod="GetUserNameList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                ShowOnlyCurrentWordInCompletionListItem="true">
                                                            </cc1:AutoCompleteExtender>
                                                            <cc1:TextBoxWatermarkExtender ID="txtUserNameSearchExtender" runat="server" TargetControlID="txtUserNameSearch"
                                                                WatermarkText="-- Search --">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("User_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Designation" DataField="Designation" HeaderStyle-CssClass="styleGridHeader"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Login Time" DataField="Last_LoginDate" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                            HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="LogOut Time" DataField="Last_LogOutDate" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                            HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <span>History</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgHistory" runat="server" ImageUrl="../Images/search_gray.jpg"
                                                    CommandArgument='<%# Bind("User_ID") %>' CommandName="History" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <span>Logged In</span>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ChkDisconnect" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                <asp:HiddenField ID="hdnUserName" runat="server" />
                            </div>
                            <div style="display: none;">
                                <asp:GridView ID="GrdUsersExcel" runat="server" DataKeyNames="Is_LoggedIn,Is_CurrentlyActive"
                                    OnRowDataBound="GrdUsersExcel_RowDataBound" AutoGenerateColumns="false" Width="100%">
                                    <Columns>
                                        <asp:BoundField HeaderText="User Code" DataField="User_Code" HeaderStyle-CssClass="styleGridHeader"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="User Name" DataField="User_Name" HeaderStyle-CssClass="styleGridHeader"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Designation" DataField="Designation" HeaderStyle-CssClass="styleGridHeader"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Login Time" DataField="Last_LoginDate" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                            HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="LogOut Time" DataField="Last_LogOutDate" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                            HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Logged In" DataField="Is_LoggedIn" HeaderStyle-CssClass="styleGridHeader"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%" align="center">
                            <input type="hidden" id="hdnSortDirection" runat="server" />
                            <input type="hidden" id="hdnSortExpression" runat="server" />
                            <input type="hidden" id="hdnSearch" runat="server" />
                            <input type="hidden" id="hdnOrderBy" runat="server" />
                            <input type="hidden" id="hdnShowAll" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="BtnExportToExcel" runat="server" Text="Export To Excel" CssClass="styleSubmitButton"
                                OnClick="BtnExportToExcel_Click" />
                            <asp:Button ID="BtnDisconnect" runat="server" Text="Disconnect Users" CssClass="styleSubmitButton"
                                OnClick="BtnDisconnect_Click" />
                            <asp:Button ID="BtnShowAll" runat="server" Text="Show All Users" Style="display: none;"
                                CssClass="styleSubmitButton" OnClick="BtnShowAll_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="PnlUserTrans" runat="server" Width="70%" GroupingText="User Login Transaction Details"
                Style="position: absolute; margin-left: 80px; margin-top: 15px;" CssClass="stylePanel">
                <div style="height: 310px; overflow-y: scroll;">
                    <asp:UpdatePanel ID="UpTransDetails" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <span>From Date</span>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:TextBox ID="txtFromDate" runat="server"> </asp:TextBox>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="From Date" />
                                        <cc1:CalendarExtender ID="CEFromDate" runat="server" Enabled="True" Format="dd-MMM-yyyy"
                                            PopupButtonID="Image1" TargetControlID="txtFromDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <span>To Date</span>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:TextBox ID="txtToDate" runat="server"> </asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="To Date" />
                                        <cc1:CalendarExtender ID="CEToDate" runat="server" Enabled="True" Format="dd-MMM-yyyy"
                                            PopupButtonID="Image2" TargetControlID="txtToDate" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Button ID="btnSearchTrans" runat="server" Text="Filter" CssClass="styleSubmitButton"
                                            OnClick="btnSearchTrans_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:GridView ID="GrdTransDetails" runat="server" DataKeyNames="Pagecounts" AutoGenerateColumns="false"
                                            Width="100%">
                                            <RowStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Logged In" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                                    DataField="LoggedIn_Date" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Logged Out" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                                    DataField="LoggedOut_Date" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="IP Address" DataField="IP_Address" HeaderStyle-CssClass="styleGridHeader"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Host Name" DataField="Host_Name" HeaderStyle-CssClass="styleGridHeader"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="LogOut Status" DataField="LogOut_Status_Code" HeaderStyle-CssClass="styleGridHeader"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--ctl00$ContentPlaceHolder1$UCCustomTransPaging$txtPageSize--%>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td colspan="5">
                                        <asp:GridView ID="GrdTransDetailsExcel" runat="server" DataKeyNames="Pagecounts"
                                            AutoGenerateColumns="false" Width="100%">
                                            <RowStyle HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:BoundField HeaderText="Logged In" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                                    DataField="LoggedIn_Date" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Logged Out" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"
                                                    DataField="LoggedOut_Date" HeaderStyle-CssClass="styleGridHeader" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="IP Address" DataField="IP_Address" HeaderStyle-CssClass="styleGridHeader"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Host Name" DataField="Host_Name" HeaderStyle-CssClass="styleGridHeader"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="LogOut Status" DataField="LogOut_Status_Code" HeaderStyle-CssClass="styleGridHeader"
                                                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                    <HeaderStyle HorizontalAlign="Left" CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <uc2:PageTransNavigator ID="UCCustomTransPaging" Visible="false" runat="server">
                                        </uc2:PageTransNavigator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" width="100%" align="center">
                                        <input type="hidden" id="hdnTransSortDirection" runat="server" />
                                        <input type="hidden" id="hdnTransSortExpression" runat="server" />
                                        <input type="hidden" id="hdnTransSearch" runat="server" />
                                        <input type="hidden" id="hdnTransOrderBy" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="center">
                                        <asp:Button ID="BtnExportTrans" runat="server" Text="Export To Excel" CssClass="styleSubmitButton"
                                            OnClick="BtnExportTrans_Click" />
                                        <asp:Button ID="BtnCloseTransPanel" runat="server" Text="Close" CssClass="styleSubmitButton"
                                            OnClick="BtnCloseTransPanel_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField ID="hdnUserId" runat="server" />
                            <asp:HiddenField ID="HdnCompanyId" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnExportTrans" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="timer" />--%>
            <asp:AsyncPostBackTrigger ControlID="BtnCloseTransPanel" />
            <asp:PostBackTrigger ControlID="BtnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
