<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_SysAdminDocumentNumberControl_View.aspx.cs" Inherits="System_Admin_S3G_SysAdminDocumentNumberControl_View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
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
                <tr width="100%">
                    <td width="100%">
                        <asp:GridView Width="100%" ID="gvDNC" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="Doc_Number_Seq_ID" OnDataBound="gvDNC_DataBound" OnRowDataBound="gvDNC_RowDataBound"
                            PageSize="100" OnRowCommand="gvDNC_RowCommand">
                            <Columns>
                                <asp:TemplateField Visible="False" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDNCID" runat="server" Text='<%# Eval("Doc_Number_Seq_ID") %>' /></ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" />
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Doc_Number_Seq_ID") %>' CommandName="Query" runat="server" /></ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Modify">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnModify" runat="server" ImageUrl="~/Images/spacer.gif"
                                            CssClass="styleGridEdit" CommandArgument='<%# Bind("Doc_Number_Seq_ID") %>' CommandName="Modify" /></ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line Of Business" SortExpression="LOB">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnLOB" OnClick="FunProSortingColumn" runat="server" Text="LOB"
                                                        CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnLOB" runat="server" CssClass="styleImageSortingAsc" AlternateText="Sort By LOB"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" Width="85px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftetxtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                        ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" SortExpression="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton OnClick="FunProSortingColumn" ID="lnkbtnRegionCode" runat="server"
                                                        Text="Location" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnRegionCode" CssClass="styleImageSortingAsc" runat="server"
                                                        AlternateText="Sort By Location" ImageAlign="Middle" />
                                                </td>
                                                <tr align="left">
                                                    <td>
                                                        <asp:TextBox ID="txtHeaderSearch2" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                            CssClass="styleSearchBox" Width="85px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftetxtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                            ValidChars="" FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Document" SortExpression="LOB">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescs" runat="server" Text='<%# Bind("DESCS") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnUser" OnClick="FunProSortingColumn" runat="server" Text="Document"
                                                        CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton CssClass="styleImageSortingAsc" ID="imgbtnUser" runat="server" AlternateText="Sort By Document"
                                                        ImageAlign="Middle" />
                                                </td>
                                                <tr align="left">
                                                    <td>
                                                        <asp:TextBox ID="txtHeaderSearch3" runat="server" AutoCompleteType="None" AutoPostBack="true"
                                                            CssClass="styleSearchBox" Width="85px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftetxtHeaderSearch3" runat="server" TargetControlID="txtHeaderSearch3"
                                                            ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Financial Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinYear" runat="server" Text='<%# Bind("Fin_Year") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatch" runat="server" Text='<%# Bind("Batch") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Level">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLevel" runat="server" Text='<%# Bind("Level") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFromNo" runat="server" Text='<%# Bind("From_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="To Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToNo" runat="server" Text='<%# Bind("To_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Used Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastUsedNo" runat="server" Text='<%# Bind("Last_Used_Number") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="CbxDNCActive" />
                                        <asp:Label ID="lblDNCActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
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
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                            font-size: medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                            Text="Create" OnClick="btnCreate_Click" />
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
