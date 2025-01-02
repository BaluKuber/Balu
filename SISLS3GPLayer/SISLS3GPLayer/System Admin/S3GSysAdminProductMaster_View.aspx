<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"  EnableEventValidation="false"
    CodeFile="S3GSysAdminProductMaster_View.aspx.cs" Inherits="S3GSysAdminProductMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                <asp:GridView runat="server" ID="grvProductMaster" OnDataBound="grvProductMaster_DataBound"
                    AutoGenerateColumns="false" OnRowDataBound="grvProductMaster_RowDataBound" HeaderStyle-CssClass="styleGridHeader"
                    HeaderStyle-Width="100%" OnRowCommand="grvProductMaster_RowCommand">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("Product_LOB_Mapping_ID") %>' CommandName="Query" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                        <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" CommandName="Modify" CommandArgument='<%# Bind("Product_LOB_Mapping_ID") %>'
                                    ImageUrl="~/Images/spacer.gif" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Code" SortExpression="Product_Code" HeaderStyle-CssClass="styleGridHeader"
                            HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label ID="lblProductCode" runat="server" Text='<%# Bind("Product_Code") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnProductCode" OnClick="FunProSortingColumn" ToolTip="Sort By Product Code"
                                                runat="server" Text="Product Code" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnProductCode" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderProductCode" runat="server" AutoPostBack="true"
                                                CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Name" SortExpression="Product_Name" HeaderStyle-CssClass="styleGridHeader"
                            HeaderStyle-Width="50%">
                            <ItemTemplate>
                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Product_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnProductName" OnClick="FunProSortingColumn" ToolTip="Sort By Product Name"
                                                runat="server" Text="Product Name" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnProductName" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderProductName" runat="server" AutoPostBack="true"
                                                CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LOB Name" SortExpression="LOB_Name" HeaderStyle-CssClass="styleGridHeader"
                            HeaderStyle-Width="30%">
                            <ItemTemplate>
                                <asp:Label ID="lblLOBName" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnLobCode" OnClick="FunProSortingColumn" runat="server" ToolTip="Sort By Line of Business"
                                                Text="Line of Business" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnLobCode" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderLOBName" runat="server" AutoPostBack="true"
                                                CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-Width="10%"
                            HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                                <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
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
        <tr class="styleButtonArea">
            <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CssClass="styleSubmitButton"
                    Text="Create" runat="server"></asp:Button>
                <asp:Button ID="btnShow" OnClick="btnShow_Click" CssClass="styleSubmitButton" Text="Show All"
                    runat="server"></asp:Button>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
</asp:Content>
