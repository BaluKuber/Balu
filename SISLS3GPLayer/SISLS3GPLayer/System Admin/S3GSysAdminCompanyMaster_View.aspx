<%@ Page Title="S3G - Company Master" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminCompanyMaster_View.aspx.cs" Inherits="System_Admin_S3GSysAdminCompanyMaster_View" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
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
                                    <asp:Label runat="server" Text="Company Master - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView runat="server" ID="grvCompanyMaster" AutoGenerateColumns="False" Width="100%"
                            OnDataBound="grvCompanyMaster_DataBound" OnRowDataBound="grvCompanyMaster_RowDataBound">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/Edit.JPG" PostBackUrl='<%#Eval("Company_ID","~/System Admin/S3GSysAdminCompanyMaster_Add.aspx?qsCmpnyId={0}")%>'
                                            runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Code" SortExpression="Company_Code" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyCode" runat="server" Text='<%# Bind("Company_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnCompanyCode" runat="server" Text="Company Code" CssClass="styleGridHeaderLinkBtn">
                                                    </asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortCode" runat="server" AlternateText="Sort By Code"
                                                        OnClick="FunProSortingColumn" ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderCompanyCode" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" MaxLength="3" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Company Name" SortExpression="Company_Name" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%# Bind("Company_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnCompanyName" runat="server" Text="Company Name" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortName" runat="server" AlternateText="Sort By Name"
                                                        OnClick="FunProSortingColumn" ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderCompanyName" runat="server" AutoPostBack="true"
                                                        MaxLength="50" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="City" HeaderStyle-CssClass="styleGridHeader">
                                <ItemTemplate>
                                    <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr align="center">
                                            <td>
                                                <asp:LinkButton ID="lnkbtnCity" runat="server" Text="City" CssClass="styleGridHeader"></asp:LinkButton>
                                                <asp:ImageButton ID="imgbtnSortCity" runat="server" AlternateText="Sort By City"
                                                    ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderCity" runat="server" AutoPostBack="true"
                                                    OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                            </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="State" HeaderStyle-CssClass="styleGridHeader">
                                <ItemTemplate>
                                    <asp:Label ID="lblState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr align="center">
                                            <td>
                                                <asp:LinkButton ID="lnkbtnState" runat="server" Text="State" CssClass="styleGridHeader"></asp:LinkButton>
                                                <asp:ImageButton ID="imgbtnSortState" runat="server" AlternateText="Sort By State"
                                                    ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderState" runat="server" AutoPostBack="true"
                                                    OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                            </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Constitutional Status" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblConstStatus" runat="server" Text='<%# Bind("Constitutional_Status") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnStatus" runat="server" Text="Constitutional Status" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortStatus" runat="server" AlternateText="Sort By Status"
                                                        OnClick="FunProSortingColumn" ImageAlign="Middle" ImageUrl="~/Images/ArrowUp.gif" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderStatus" runat="server" AutoPostBack="true"
                                                        MaxLength="50" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Active")%>'></asp:Label>
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
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr align="center">
                                <td>
                                    <asp:ImageButton runat="server" ID="imgbtnFirst" ImageUrl="~/Images/First.gif" OnClick="imgbtnFirst_Click"
                                        Enabled="false" />
                                    <asp:ImageButton runat="server" ID="imgbtnPrev" ImageUrl="~/Images/Prev.gif" OnClick="imgbtnPrev_Click"
                                        Enabled="false" />
                                </td>
                                <td>
                                    <asp:DataList runat="server" ID="dlstPager" CellPadding="1" CellSpacing="1" OnItemCommand="dlstPager_ItemCommand"
                                        OnItemDataBound="dlstPager_ItemDataBound" RepeatDirection="Horizontal">
                                        <ItemTemplate>
                                            <%--  <asp:Label ID="lblCount" runat="server" Text='<%# Eval("Count") %>' Visible="false" />--%>
                                            <asp:LinkButton ID="lnkbtnPaging" runat="server" CommandArgument='<%# Eval("PageIndex") %>'
                                                CommandName="lnkbtnPaging" Text='<%# Eval("PageText") %>' Font-Size="Small"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" ID="imgbtnNext" ImageUrl="~/Images/Next.gif" OnClick="imgbtnNext_Click"
                                        Enabled="false" />
                                    <asp:ImageButton runat="server" ID="imgbtnLast" ImageUrl="~/Images/Last.gif" OnClick="imgbtnLast_Click"
                                        Enabled="false" />
                                </td>
                            </tr>
                        </table>
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
                        &nbsp;
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                            OnClick="btnShowAll_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
