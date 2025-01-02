<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3gOrgAssetMaster_View.aspx.cs" Inherits="Origination_S3g_OrgAssetMaster_View"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
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
                    <td style="padding-top: 10px">
                        <div style="overflow: auto; width: 100%;">
                            <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                <ContentTemplate>

                                    <cc1:TabContainer ID="TabContainerAM" runat="server" CssClass="styleTabPanel" Width="100%"
                                        ScrollBars="None" ActiveTabIndex="0" AutoPostBack="true" OnActiveTabChanged="tccategoryCodes_ActiveTabChanged">
                                        <cc1:TabPanel runat="server" ID="TabAssetCat" CssClass="tabpan" BackColor="Red" Width="100%">
                                            <HeaderTemplate>
                                                Asset Category
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabAssetType" CssClass="tabpan" BackColor="Red" Width="100%">
                                            <HeaderTemplate>
                                                Asset Type
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabAssetSupType" CssClass="tabpan" BackColor="Red" Width="100%">
                                            <HeaderTemplate>
                                                Asset Sub Type
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                    <asp:GridView ID="grvAssetCategoryCodes" runat="server" AutoGenerateColumns="False"
                                        ToolTip="Asset Master View" Width="99%" OnRowCommand="grvAssetClassCodes_RowCommand"
                                        OnRowDataBound="grvAssetCategoryCodes_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                        AlternateText="Query" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Asset_Category_ID") %>'
                                                        runat="server" CommandName="Query" />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                                </HeaderTemplate>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modify" SortExpression="ID">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                                        AlternateText="Modify" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Asset_Category_ID")%>'
                                                        runat="server" CommandName="Modify" />
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Code" SortExpression="Category_Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClassCode" runat="server" Text='<%# Bind("Category_Code") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnSort1" runat="server" ToolTip="Sort By Code" Text="Code"
                                                                    CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn">
                                                                </asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                    CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch" MaxLength="4"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                    FilterType="UppercaseLetters, LowercaseLetters, Numbers" Enabled="True" ValidChars=" ">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" SortExpression="Category_Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClassDesc" runat="server" Text='<%# Bind("Category_Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Category Name" ToolTip="Sort By Description"
                                                                    OnClick="FunProSortingColumn" CssClass="styleGridHeaderLinkBtn">
                                                                </asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                                    CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch" MaxLength="50"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                    FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" Enabled="True" ValidChars=" ">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserID" runat="server" Text='<%#Eval("Created_By")%>'></asp:Label>
                                                    <asp:Label ID="lblUserLevelID" runat="server" Text='<%#Eval("User_Level_ID")%>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium"></span>
                        <%-- <asp:LinkButton ID="lnkbtnAssetCode" runat="server" Text="Asset Code" CssClass="styleGridHeader"> </asp:LinkButton>--%>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px;" align="center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Create" runat="server" Visible="true"></asp:Button>
                          <asp:Button ID="btnExportToExcel" OnClick="btnExportToExcel_Click" Visible="false" UseSubmitBehavior="true" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Export To Excel" runat="server"></asp:Button>
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                            OnClick="btnShowAll_Click" Visible="true" />
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
