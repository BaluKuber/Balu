<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminAuditTrial.aspx.cs" Inherits="System_Admin_S3GSysAdminAuditTrial"
    Title="Untitled Page" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td class="stylePageHeading">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="margin-left: 40px">
                        <asp:GridView ID="grvAuditTrial" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader"
                            Width="100%" OnRowDataBound="grvAuditTrial_RowDataBound" ShowFooter="true" OnRowCommand="grvAuditTrial_RowCommand"
                            AllowPaging="false" OnRowCreated="grvAuditTrial_RowCreated">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SLNO") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program Description" SortExpression="LOB_Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProgramID" runat="server" Visible="false" Text='<%# Bind("ProgramID") %>'></asp:Label>
                                        <asp:Label ID="lblProgramCode" runat="server" Text='<%# Bind("ProgramName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlProgramCode" runat="server">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Edit Archive Data" SortExpression="LOB_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Archive Data
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEditArchive" Checked='<%# GVBoolFormat(Eval("EditArchive").ToString()) %>'
                                            runat="server" MaxLength="40"></asp:CheckBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkEditArchive" runat="server" MaxLength="40"></asp:CheckBox>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Log Data" SortExpression="LOB_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Log Data
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkEditLog" Checked='<%# GVBoolFormat(Eval("EditLog").ToString()) %>'
                                            runat="server" MaxLength="40"></asp:CheckBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkEditLog" runat="server" MaxLength="40"></asp:CheckBox>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete Archive Data" SortExpression="LOB_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Archive Data
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDeleteArchive" Checked='<%# GVBoolFormat(Eval("DeleteArchive").ToString()) %>'
                                            runat="server" MaxLength="40"></asp:CheckBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkDeleteArchive" runat="server" MaxLength="40"></asp:CheckBox>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete Log Data" SortExpression="LOB_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Log Data
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkDeleteLog" Checked='<%# GVBoolFormat(Eval("DeleteLog").ToString()) %>'
                                            runat="server" MaxLength="40"></asp:CheckBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkDeleteLog" runat="server" MaxLength="40"></asp:CheckBox>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Query Archive Data" SortExpression="LOB_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Archive Data
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkQueryArchive" Checked='<%# GVBoolFormat(Eval("QueryArchive").ToString()) %>'
                                            runat="server" MaxLength="40"></asp:CheckBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkQueryArchive" runat="server" MaxLength="40"></asp:CheckBox>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Query Log Data" SortExpression="LOB_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Log Data
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkQueryLog" Checked='<%# GVBoolFormat(Eval("QueryLog").ToString()) %>'
                                            runat="server" MaxLength="40"></asp:CheckBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:CheckBox ID="chkQueryLog" runat="server" MaxLength="40"></asp:CheckBox>
                                    </FooterTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-BackColor="#F2F8FF" HeaderText="Active" FooterStyle-BackColor="#F2F8FF">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    Active
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsActive" Checked='<%# GVBoolFormat(Eval("IsActive").ToString()) %>'
                                            runat="server" MaxLength="40"></asp:CheckBox>
                                        <%--<asp:LinkButton runat="server" ID="lnkRemove" Text="Remove" CommandName="Delete"></asp:LinkButton>--%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <%--<asp:LinkButton runat="server" ID="lnkAdd" Text="Add" CommandName="AddNew"></asp:LinkButton>--%>
                                        <asp:Button runat="server" ID="btnAdd" Text="Add" CommandName="AddNew"></asp:Button>
                                    </FooterTemplate>
                                    <%--<HeaderStyle CssClass="styleGridHeader" Width="10%" />--%>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By") %>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                        </asp:GridView>
                    </td>
                </tr>
                <tr width="100%">
                    <%--Grid--%>
                    <td width="100%" colspan="4" align="center">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr width="100%">
                    <%--Spacer--%>
                    <td width="100%" colspan="4" align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;&nbsp;<asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton"
                            Text="Save" OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                        &nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsAuditTrial" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " />
                        <asp:CustomValidator ID="cvAuditTrial" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
