<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_SysAdminAccountSetupMaster_View.aspx.cs" Inherits="System_Admin_S3G_SysAdminAccountSetupMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table id="tbMain" runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px">
                        <cc1:TabContainer ID="tcAcctSetup" runat="server" Width="100%" AutoPostBack="True"
                            CssClass="styleTabPanel" RowStyle-ForeColor="Blue" OnActiveTabChanged="tcAcctSetup_ActiveTabChanged">
                            <cc1:TabPanel runat="server" ID="TabPanel1" CssClass="tabpan" BackColor="Red" Width="100%">
                                <HeaderTemplate>
                                    Asset
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabPanel2" CssClass="tabpan" BackColor="Red" Width="100%">
                                <HeaderTemplate>
                                    Liability
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabPanel3" CssClass="tabpan" BackColor="Red" Width="100%">
                                <HeaderTemplate>
                                    Income
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabPanel4" CssClass="tabpan" BackColor="Red" Width="100%">
                                <HeaderTemplate>
                                    Expenses
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                        <asp:GridView ID="GridView1" runat="server" CssClass="styleInfoLabel" Width="100%"
                            AutoGenerateColumns="False" DataKeyNames="Account_Setup_ID" OnRowCommand="GridView1_RowCommand"
                            OnDataBound="GridView1_DataBound" OnRowDataBound="GridView1_RowDataBound">
                            <Columns>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccSetId" runat="server" Text='<%# Eval("Account_Setup_ID") %>' /></ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Account_Setup_ID") %>' runat="server" CommandName="Query" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnModify" runat="server" ImageUrl="~/Images/spacer.gif"
                                            CssClass="styleGridEdit" CommandArgument='<%# Bind("Account_Setup_ID") %>' CommandName="Modify" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LOB" SortExpression="LOB" HeaderStyle-CssClass="styleGridHeader">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortLOB" runat="server" ToolTip="Sort By LOB" OnClick="FunProSortingColumn"
                                                        Text="Line of Business" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortLOB" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" Width="85px" MaxLength="50" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblELOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeaderLinkBtn" Font-Underline="false" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" SortExpression="Location">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                            </tr>
                                            <td>
                                                <asp:LinkButton ID="lnkbtnSortBranch" runat="server" Text="Location" CssClass="styleGridHeaderLinkBtn"
                                                    ToolTip="Sort By Location" OnClick="FunProSortingColumn"></asp:LinkButton>
                                                <asp:ImageButton ID="imgbtnSortBranch" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                            </td>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" OnTextChanged="FunProHeaderSearch"
                                                        AutoPostBack="true" CssClass="styleSearchBox" MaxLength="25"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                        ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEBranch" runat="server" Text='<%# Bind("Location") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeaderLinkBtn" Font-Underline="false" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Flag" SortExpression="Account_Flag">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                            </tr>
                                            <td>
                                                <asp:LinkButton ID="lnkbtnSortAccount" runat="server" Text="Account Flag" CssClass="styleGridHeaderLinkBtn"
                                                    ToolTip="Sort By AccountFlag" OnClick="FunProSortingColumn"></asp:LinkButton>
                                                <asp:ImageButton ID="imgbtnSortAccount" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                            </td>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" CssClass="styleSearchBox"
                                                        OnTextChanged="FunProHeaderSearch" AutoPostBack="true" MaxLength="6"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                        ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEAcFlag" runat="server" Text='<%# Bind("Account_Flag") %>'></asp:Label></ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeaderLinkBtn" Font-Underline="false" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Level" HeaderText="Level">
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="GL_CODE" HeaderText="GL Code">
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SL_CODE" HeaderText="SL Code">
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="CbxActive" /><asp:Label ID="lblActive"
                                            Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label></ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
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
                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                        </asp:GridView>
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnCreate" runat="server" CssClass="styleSubmitButton" Text="Create"
                            OnClick="btnCreate_Click" />
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                            OnClick="btnShowAll_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                            font-size: medium"></asp:Label>
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
