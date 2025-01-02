<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3GTemplate_View.aspx.cs" Inherits="System_Admin_S3GTemplate_View" ValidateRequest="false"%>

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
                                    <asp:Label runat="server" Text="Template Master - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grvTemplatePaging" runat="server" AutoGenerateColumns="False" Width="100%"
                            OnRowCommand="grvTemplatePaging_RowCommand" OnRowDataBound="grvTemplatePaging_RowDataBound">
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
                                <asp:TemplateField HeaderText="Modify" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                            AlternateText="Modify" CommandArgument='<%# Bind("ID") %>' runat="server" CommandName="Modify" />
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Line of Business">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineofBusiness" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Line of Business" ToolTip="Sort By Line of Business"
                                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                        Width="120px" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="- " />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Location" ToolTip="Sort By Location"
                                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                        Width="120px" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="- " />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Template Ref No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTemplateRefNo" runat="server" Text='<%# Bind("TemplateRefNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Template Ref No" ToolTip="Sort By Template Ref No"
                                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                        Width="120px" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="- /" />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Template Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTemplateType" runat="server" Text='<%# Bind("TemplateType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" align="center">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Template Type" ToolTip="Sort By Template Type"
                                                        CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" AutoPostBack="true"
                                                        Width="120px" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="filtertxtHeaderSearch4" runat="server" TargetControlID="txtHeaderSearch4"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="- " />
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsActive" Enabled="false" runat="server" />
                                        <asp:Label ID="lblActive" runat="server" Visible="false" Text='<%# Eval("Is_Active") %>'></asp:Label>
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
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <asp:CustomValidator ID="cvErrormsg" runat="server"></asp:CustomValidator>
                        <%-- <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>--%>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Create" runat="server"></asp:Button>
                        <asp:Button ID="btnShowAll" runat="server" OnClick="btnShowAll_Click" Text="Show All" CssClass="styleSubmitButton" />
                        <%----%>
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
