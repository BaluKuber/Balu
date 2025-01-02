<%@ Page Title="S3G - User Management" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminUserManagement_View.aspx.cs" Inherits="S3GSysAdminUserManagement_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="User Management - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<div id="divGrid1" style="overflow: scroll; width: 800px;">--%>
                        <asp:GridView runat="server" ID="grvUserManagement" OnRowCommand="grvUserManagement_RowCommand"
                            OnRowDataBound="grvUserManagement_RowDataBound" Width="100%" AutoGenerateColumns="false"
                            RowStyle-HorizontalAlign="left" HeaderStyle-CssClass="styleGridHeader">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("User_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("User_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Code" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserCode" runat="server" Text='<%# Bind("User_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnUserCode" OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn"
                                                        runat="server" ToolTip="Sort By User Code" Text="User Code"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnUserCode" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" CssClass="styleSearchBox" onpaste="return false;"
                                                        Width="100px" runat="server" MaxLength="6" AutoPostBack="true" 
                                                         OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                         <cc1:FilteredTextBoxExtender ID ="FtxtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1" 
                                                        FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@." Enabled="True"> </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("User_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnUserName" CssClass="styleGridHeaderLinkBtn" ToolTip="Sort By User Name"
                                                        runat="server" OnClick="FunProSortingColumn" Text="User Name"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnUserName" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" CssClass="styleSearchBox" onpaste="return false;"
                                                        onkeypress="fnCheckSpecialChars(true);" MaxLength="50" runat="server" AutoPostBack="true"
                                                        OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="EMail Id" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEMailId" runat="server" Text='<%# Bind("EMail_Id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnEMailId" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By EMail Id" Text="EMail Id"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnEMailId" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" CssClass="styleSearchBox" onpaste="return false;"
                                                        runat="server" AutoPostBack="true" MaxLength="60" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID ="FtxtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3" 
                                                        FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@." Enabled="True"> </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Designation" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Bind("Designation") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnDesignation" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Designation" Text="Designation"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnDesignation" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" CssClass="styleSearchBox" onpaste="return false;"
                                                        Width="130px" MaxLength="40" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID ="FtxtHeaderSearch4" runat="server" TargetControlID="txtHeaderSearch4" 
                                                        FilterType="Custom,Numbers,UppercaseLetters,LowercaseLetters" FilterMode="ValidChars" ValidChars="@." Enabled="True"> </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="User_Level_Desc" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderText="User Level" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
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
                        </asp:GridView>
                        <%--</div>--%>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
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
