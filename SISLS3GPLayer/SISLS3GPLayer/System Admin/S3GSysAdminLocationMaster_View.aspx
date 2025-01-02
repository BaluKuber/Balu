<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminLocationMaster_View.aspx.cs" Inherits="System_Admin_S3GSysAdminLocationMaster_View"
    Title="S3G - Location Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="upLocationMaster" runat="server">
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
                            <cc1:TabContainer ID="tcLocation" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                Width="99%" ScrollBars="None" TabStripPlacement="top" AutoPostBack="True" OnActiveTabChanged="tcLocation_ActiveTabChanged">
                                <%-- --%>
                                <cc1:TabPanel runat="server" ID="tpLocationDef" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Location Definition &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <cc1:TabContainer ID="tcLocationDef" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                            ScrollBars="Auto" AutoPostBack="True" OnActiveTabChanged="tcLocationDef_ActiveTabChanged">
                                        </cc1:TabContainer>
                                        <asp:GridView ID="gvLocationDef" runat="server" AutoGenerateColumns="False" ToolTip="Location Definition"
                                            Width="100%" OnRowCommand="gvLocationDef_RowCommand" OnRowDataBound="gvLocationDef_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                            AlternateText="Query" runat="server" CommandName="Query" CommandArgument='<%# Bind("Location_Category_ID") %>' />

                                                    </ItemTemplate>

                                                    <HeaderStyle CssClass="styleGridHeader" />

                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Modify" SortExpression="Location_Category_ID">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                                            AlternateText="Modify" runat="server" CommandName="Modify" CommandArgument='<%# Bind("Location_Category_ID") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Code" SortExpression="Location_Code">
                                                    <HeaderTemplate>
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:LinkButton ID="lnkbtnSort1" runat="server" ToolTip="Sort By Location Code" Text="Location Code"
                                                                        CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn">
                                                                    </asp:LinkButton>
                                                                    <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td>
                                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                        CssClass="styleSearchBox" MaxLength="50" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" Enabled="True"
                                                                        ValidChars=" ">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocationCode" runat="server" Text='<%# Bind("Location_Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="GST State Code" SortExpression="GST_State_Code">
                                                    <HeaderTemplate>
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:LinkButton ID="lnkbtnGSTState" runat="server" ToolTip="Sort By GST State Code" Text="GST State Code"
                                                                        CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn">
                                                                    </asp:LinkButton>
                                                                    <asp:ImageButton ID="imgbtnGSTState" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td>
                                                                    <asp:TextBox AutoCompleteType="None" ID="txtGSTState" runat="server" AutoPostBack="true"
                                                                        CssClass="styleSearchBox" MaxLength="50" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGSTStateCode" runat="server" Text='<%# Bind("GST_State_Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Location Description" SortExpression="LocationCat_Description">
                                                    <HeaderTemplate>
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Location Description" ToolTip="Sort By Location Description"
                                                                        CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn">
                                                                    </asp:LinkButton>
                                                                    <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                </td>
                                                            </tr>
                                                            <tr align="left">
                                                                <td>
                                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                                        CssClass="styleSearchBox" MaxLength="50" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                        FilterType="Custom, UppercaseLetters, LowercaseLetters, Numbers" ValidChars=" "
                                                                        Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocationDesc" runat="server" Text='<%# Bind("LocationCat_Description") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Hierarchy Description" SortExpression="Location_Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHierarchyDescription" runat="server" Text='<%# Bind("Location_Description") %>'></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CheckBoxField HeaderText="Active" DataField="Is_Active" />
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By")%>'></asp:Label>
                                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>

                                                    </ItemTemplate>

                                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>

                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="tpLocationMap" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Location Mapping &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upLocationMap" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvLocationMapping" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    OnRowCommand="gvLocationMapping_RowCommand" OnRowDataBound="gvLocationDef_RowDataBound">
                                                    <%-- OnRowDataBound="grvAssetCategoryCodes_RowDataBound"--%>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                    AlternateText="Query" runat="server" CommandName="Query" CommandArgument='<%# Bind("Location_ID") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Modify" SortExpression="Location_ID">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                                                    AlternateText="Modify" runat="server" CommandName="Modify" CommandArgument='<%# Bind("Location_ID") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Location Code" SortExpression="Location_Code">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Location Code" CssClass="styleGridHeaderLinkBtn"
                                                                                ToolTip="Sort By Location Code" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" OnTextChanged="FunProHeaderSearch"
                                                                                runat="server" AutoPostBack="true" CssClass="styleSearchBox"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch1" runat="server" TargetControlID="txtHeaderSearch1"
                                                                                ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                                                Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLocationCode" runat="server" Text='<%# Bind("Location_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Location Description" SortExpression="Location">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Location Description" CssClass="styleGridHeaderLinkBtn"
                                                                                ToolTip="Sort By Location Description" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" OnTextChanged="FunProHeaderSearch"
                                                                                runat="server" AutoPostBack="true" CssClass="styleSearchBox"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="txtExtHeaderSearch2" runat="server" TargetControlID="txtHeaderSearch2"
                                                                                ValidChars="- " FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom"
                                                                                Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Active">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblActve" runat="server" Text="Active" CssClass="styleGridHeader"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkActive" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Active")))%>' runat="server" Enabled="false" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <%--Code added by saran on 13-Jan-2012 for Operational Location start--%>
                                                        <asp:TemplateField HeaderText="Operational">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblIs_Operational" runat="server" Text="Operational" CssClass="styleGridHeader"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkIs_Operational" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Is_Operational")))%>' runat="server"
                                                                    Enabled="false" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <%--Code added by saran on 13-Jan-2012 for Operational Location end--%>
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
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                    </td>
                </tr>
                <tr align="center">
                    <td>
                        <table width="99%;">
                            <tr>
                                <td>
                                    <uc1:PageNavigator ID="ucCustomPaging" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px;" align="center">
                        <asp:Button ID="btnCreate" CausesValidation="false" OnClick="btnCreate_Click" CssClass="styleSubmitButton"
                            Text="Create" runat="server" Visible="true"></asp:Button>
                        <asp:Button ID="btnExportToExcel" UseSubmitBehavior="true" OnClick="btnExportToExcel_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Export To Excel" runat="server"></asp:Button>
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" OnClick="btnShowAll_Click"
                            CssClass="styleSubmitButton" Visible="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="cvLocation" runat="server"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnSortDirection" runat="server" />
            <input type="hidden" id="hdnSortExpression" runat="server" />
            <input type="hidden" id="hdnSearch" runat="server" />
            <input type="hidden" id="hdnOrderBy" runat="server" />
            <input type="hidden" id="hdnHierarc" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
