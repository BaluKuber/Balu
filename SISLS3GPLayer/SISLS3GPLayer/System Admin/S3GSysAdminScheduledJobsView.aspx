<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3GSysAdminScheduledJobsView.aspx.cs" Inherits="S3GSysAdminScheduledJobsView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grvScheduledJobs" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowCommand="grvScheduledJobs_RowCommand" OnRowDataBound="grvScheduledJobs_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Query" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    AlternateText="Query" CommandArgument='<%# Bind("ID") %>' runat="server" CommandName="Query" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modify" SortExpression="ID" Visible="fALSE" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    AlternateText="Modify" CommandArgument='<%# Bind("ID") %>' runat="server" CommandName="Modify" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job Description">
                            <ItemTemplate>
                                <asp:Label ID="lblJOB" runat="server" Text='<%# Bind("JobDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0" align="center">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Job Description" ToolTip="Sort By Job Description"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                Width="120px" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filterLOB" runat="server" TargetControlID="txtHeaderSearch1"
                                                FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                ValidChars="- " />
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job Nature">
                            <ItemTemplate>
                                <asp:Label ID="lblJobNature" runat="server" Text='<%# Bind("JobNature") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Frequency">
                            <ItemTemplate>
                                <asp:Label ID="lblFrequency" runat="server" Text='<%# Bind("Frequency") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Date">
                            <ItemTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("StartDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Start Time">
                            <ItemTemplate>
                                <asp:Label ID="lblStartTime" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsActive" Enabled="false" runat="server" Checked='<%# Bind("Is_Active") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                                <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%#Eval("Created_By")%>'></asp:Label>
                                <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%#Eval("User_Level_ID")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr align="center">
            <td>
                <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:CustomValidator ID="cvErrormsg" runat="server">
                
                </asp:CustomValidator>
                <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatory"></asp:Label--%>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px;" align="center">
                <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Schedule" runat="server"></asp:Button>
                <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                    OnClick="btnShowAll_Click" />
                <asp:Button ID="btnMonitor" runat="server" Text="Schedule Monitor" ToolTip="Schedule Monitor" CssClass="styleSubmitButton"
                    OnClick="btnMonitor_Click" />
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
</asp:Content>
