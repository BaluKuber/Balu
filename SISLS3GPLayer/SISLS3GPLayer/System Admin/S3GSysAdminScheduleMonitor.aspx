<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminScheduleMonitor.aspx.cs" Inherits="S3GSysAdminScheduleMonitor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="Scheduled Jobs Monitor" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 30px">
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table width="800px" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="100%">
                                            <table width="100%" class="styleContentTable" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="stylePagingControl" width="50%" align="left">
                                                        <asp:Label ID="Label1" runat="server" CssClass="styleDisplayLabel" Text="Scheduled Jobs Monitor"
                                                            Style="padding-left: 5px; font-weight: bold"></asp:Label>
                                                    </td>
                                                    <td class="stylePagingControl" align="right" width="50%">
                                                        <asp:Label ID="lblStatus" runat="server" CssClass="styleDisplayLabel" Text="Status"></asp:Label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                            <asp:ListItem Text="All" Value="0" Selected="True">
                                                            </asp:ListItem>
                                                            <asp:ListItem Text="Completed" Value="C"></asp:ListItem>
                                                            <asp:ListItem Text="In Progress" Value="WIP"></asp:ListItem>
                                                            <asp:ListItem Text="Pending" Value="O"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="800px" align="center">
                                            <table width="99%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="grvJobs" runat="server" AutoGenerateColumns="false" Width="100%"
                                                            OnRowDataBound="grvJobs_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl. No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSl" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocation" Style="padding-left: 3px" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Job" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblJob" Style="padding-left: 3px" runat="server" Text='<%# Bind("Job") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Start Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStartDate" Style="padding-left: 3px" runat="server" Text='<%# Bind("StartDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="End Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEndDate" Style="padding-left: 3px" runat="server" Text='<%# Bind("EndDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:Image ID="imgStatus" runat="server" ImageUrl="~/Images/animation_processing.gif"
                                                                            Height="15px" Width="15px" />
                                                                        <asp:Label ID="lblProcess" Style="padding-left: 2px" runat="server" Text='<%# Bind("Process") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="800px" align="center">
                                            <table width="99%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trNoRecord">
                                        <td>
                                            <br />
                                            <table width="99%" class="styleContentTable" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="stylePagingControl" width="99%" align="center">
                                                        <asp:Label ID="lblNoRecord" runat="server" CssClass="styleDisplayLabel" Text="-- No Records found to show --"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 5px; padding-left: 5px;">
                                            <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:Timer ID="Timer1" runat="server" Interval="7000" Enabled="false" OnTick="Timer1_Tick">
                        </asp:Timer>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:CustomValidator ID="cvScheduledJobs" runat="server" CssClass="styleReqFieldLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click"
                    CssClass="styleSubmitButton" />
            </td>
        </tr>
    </table>
</asp:Content>
