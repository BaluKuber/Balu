<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Org_HSN_View.aspx.cs" Inherits="Origination_S3G_Org_HSN_View" %>

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
                <asp:GridView runat="server" ID="grvHSNMaster" OnDataBound="grvHSNMaster_DataBound"
                    AutoGenerateColumns="false" OnRowDataBound="grvHSNMaster_RowDataBound" HeaderStyle-CssClass="styleGridHeader"
                    HeaderStyle-Width="100%" OnRowCommand="grvHSNMaster_RowCommand">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("HSN_ID") %>' CommandName="Query" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                        <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" CommandName="Modify" CommandArgument='<%# Bind("HSN_ID") %>'
                                    ImageUrl="~/Images/spacer.gif" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HSN/SAC Code" SortExpression="HSN_Code" HeaderStyle-CssClass="styleGridHeader"
                            HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Label ID="lblHSNCode" runat="server" Text='<%# Bind("HSN_Code") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnHSNCode" OnClick="FunProSortingColumn" ToolTip="Sort By HSN Code"
                                                runat="server" Text="HSN/SAC Code" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnHSNCode" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderHSNCode" runat="server" AutoPostBack="true"
                                                CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="HSN/SAC Name" SortExpression="HSN_Desc" HeaderStyle-CssClass="styleGridHeader"
                            HeaderStyle-Width="50%">
                            <ItemTemplate>
                                <asp:Label ID="lblHSNName" runat="server" Text='<%# Bind("HSN_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnHSNName" OnClick="FunProSortingColumn" ToolTip="Sort By HSN Name"
                                                runat="server" Text="HSN/SAC Name" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnHSNName" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderHSNName" runat="server" AutoPostBack="true"
                                                CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                     <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Code Type" HeaderStyle-Width="10%"
                            HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                              
                                <asp:Label ID="lblcodetype" runat="server" Text='<%#Eval("Code_Type")%>'></asp:Label>
                            </ItemTemplate>
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


