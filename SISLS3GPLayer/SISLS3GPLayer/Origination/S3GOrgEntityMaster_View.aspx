<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"  EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="S3GOrgEntityMaster_View.aspx.cs" Inherits="Origination_S3GOrgEntityMaster_View" %>

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
                            <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleInfoLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%"
                    OnRowCommand="grvEntityMaster_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Query" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    AlternateText="Query" CommandArgument='<%# Bind("Entity_ID") %>' runat="server"
                                    CommandName="Query" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modify" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    AlternateText="Modify" CommandArgument='<%# Bind("Entity_ID") %>' runat="server"
                                    CommandName="Modify" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor Type">
                            <ItemTemplate>
                                <asp:Label ID="lblEntityType" runat="server" Text='<%# Bind("Entity_Type_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Vendor Type" ToolTip="Sort By Vendor type"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="50"></asp:TextBox>
                                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch1"
                                                            FilterType="UppercaseLetters,LowercaseLetters,Numbers" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor Code">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Vendor Code" ToolTip="Sort By Vendor code"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength=12></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEntityCode" runat="server" Text='<%# Bind("Entity_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor Name">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Vendor Name" ToolTip="Sort By Vendor name"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength=90></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEntityName" runat="server" Text='<%# Bind("Entity_Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                                <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%#Eval("USRID")%>'></asp:Label>
                                <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%#Eval("User_Level_ID")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="City">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="City" ToolTip="Sort By City"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
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
            <td style="padding-top: 5px;" align="center">
                <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Create" runat="server"></asp:Button>
                 <asp:Button ID="btnExportToExcel" OnClick="btnExportToExcel_Click" UseSubmitBehavior="true" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Export To Excel" runat="server"></asp:Button>
                <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                    OnClick="btnShowAll_Click" />
            </td>
        </tr>
        
         <tr>
            <td align="center">
               <%-- <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium">
                </span>--%>
                <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
     </ContentTemplate>
    <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
