<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminDocPath_View.aspx.cs" Inherits="System_Admin_S3GSysAdminDocPath_View" %>

<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="grvDocPathMaster" AutoGenerateColumns="False" Width="100%"
                            OnRowCommand="grvDocPathMaster_RowCommand" OnRowDataBound="grvDocPathMaster_RowDataBound">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            AlternateText="Query" CommandArgument='<%# Bind("Document_Path_ID") %>' runat="server"
                                            CommandName="Query" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                            AlternateText="Modify" CommandArgument='<%# Bind("Document_Path_ID") %>' runat="server"
                                            CommandName="Modify" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line Of Business" SortExpression="LOB_Name" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLob" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortLOB" runat="server" ToolTip="Sort By Line of Business"
                                                        Text="Line Of Business" CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn">
                                                    </asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortLOB" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch" MaxLength="40"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="Custom,UppercaseLetters, LowercaseLetters" Enabled="True" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Code" SortExpression="Role_Code" HeaderStyle-Width="15%"><%--SortExpression="Role_Code + ' - ' + Program_Name"--%>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleCode" runat="server" Text='<%# Bind("Role_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortRole" runat="server" Text="Role Description" ToolTip="Sort By Role code"
                                                        OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn">
                                                    </asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortRole" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                        FilterType="UppercaseLetters, LowercaseLetters" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                               <%--<asp:TemplateField HeaderText="Document Flag" SortExpression="PM.Document_Flag"
                                    HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlagDesc" runat="server" Text='<%# Bind("Document_Flag_Desc") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortFlagDesc" runat="server" ToolTip="Sort By Document flag"
                                                        Text="Document Flag Desc" CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortFlagDesc" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch" MaxLength="50"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="Custom,UppercaseLetters, LowercaseLetters" Enabled="True" ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Document Path" SortExpression="Document_Path" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocPath" runat="server" Text='<%# Bind("Document_Path") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <%-- <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnDocPath" runat="server" Text="Document Path" CssClass="styleGridHeader"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSortPath" runat="server" AlternateText="Sort By Path"
                                                OnClick="FunProSortingColumn" ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderDocpath" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" Checked='<%# DataBinder.Eval(Container.DataItem,"Is_Active").ToString().ToUpper() == "TRUE"  %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By")%>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
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
                        <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium">
                        </span>
                        <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatory"></asp:Label--%>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px;" align="center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Create" runat="server"></asp:Button>
                      
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                            OnClick="btnShowAll_Click" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
