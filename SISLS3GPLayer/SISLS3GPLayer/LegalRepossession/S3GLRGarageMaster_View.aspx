<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/LegalRepossession/S3GLRGarageMaster_View.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="LegalRepossession_S3GLRGarageMaster_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ register src="../UserControls/PageNavigator.ascx" tagname="PageNavigator" tagprefix="uc1" %>
<table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowCommand="grvGarageMaster_RowCommand" OnRowDataBound="grvPaging_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Query" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    AlternateText="Query" CommandArgument='<%# Bind("Garage_ID") %>' runat="server"
                                    CommandName="Query" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modify" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    AlternateText="Modify" CommandArgument='<%# Bind("Garage_ID") %>' runat="server"
                                    CommandName="Modify" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Garage Code">
                            <ItemTemplate>
                                <asp:Label ID="lblGarageCode" runat="server" 
                                    Text='<%# Bind("Garage_Code") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Garage Code" ToolTip="Sort By Entity type"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Entity Code">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Garage Owner " ToolTip="Sort By Entity code"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblGarageOwner" runat="server" Text='<%# Bind("Garage_Owner_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Name">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Garage Name" ToolTip="Sort By Garage name"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblContactName" runat="server" Text='<%# Bind("Garage_Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Status">
                            <ItemTemplate>
                                <asp:Label ID="LblStatus" runat="server" Text='<%# Bind("Active_Status") %>'></asp:Label>
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

</asp:Content>