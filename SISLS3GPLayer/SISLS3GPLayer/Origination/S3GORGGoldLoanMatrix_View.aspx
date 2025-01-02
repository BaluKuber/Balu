<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGGoldLoanMatrix_View.aspx.cs" Inherits="Origination_S3GORGGoldLoanMatrix_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
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
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="grvPaging" OnRowCommand="grvPaging_RowCommand"
                            OnRowDataBound="grvPaging_RowDataBound" Width="100%" AutoGenerateColumns="false"
                            HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Left">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ToolTip="Query" ImageUrl="~/Images/spacer.gif"
                                            CssClass="styleGridQuery" CommandArgument='<%# Bind("GLMatrix_ID") %>' CommandName="Query"
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" ToolTip="Modify" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("GLMatrix_ID") %>' CommandName="Modify" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort1" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Asset Category" Text="Asset Category"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort1" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" MaxLength="5" runat="server"
                                                        CssClass="styleSearchBox" ToolTip="Asset Category" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort2" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Asset Description" Text="Asset Description"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort2" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" ToolTip="Asset Description"
                                                        MaxLength="40" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                        CssClass="styleSearchBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetDescription" ToolTip="Asset Description" runat="server" Text='<%# Bind("Asset_Description") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort3" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Product" Text="Product"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort3" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" MaxLength="40" ToolTip="Product"
                                                        runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Product_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort4" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Constitution" Text="Constitution"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort4" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="40" runat="server"
                                                        ToolTip="Constitution" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                        CssClass="styleSearchBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblConstitutionName" runat="server" Text='<%# Bind("Constitution_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort5" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By Location" Text="Location"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort5" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" MaxLength="40" runat="server"
                                                        AutoPostBack="true" OnTextChanged="FunProHeaderSearch" ToolTip="State" CssClass="styleSearchBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblState" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort6" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                        OnClick="FunProSortingColumn" ToolTip="Sort By EffDate" Text="Eff Date"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort6" CssClass="styleImageSortingAsc" runat="server"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffDate" Width="60px" runat="server" Text='<%# ShowDate(Eval("Eff_Date").ToString())%>'></asp:Label>
                                    </ItemTemplate>
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
                            Text="Create" ToolTip="Create" runat="server"></asp:Button>
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" ToolTip="Show All" CssClass="styleSubmitButton"
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
