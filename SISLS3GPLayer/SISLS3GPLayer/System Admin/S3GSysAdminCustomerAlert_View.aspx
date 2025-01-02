<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    CodeFile="S3GSysAdminCustomerAlert_View.aspx.cs" Inherits="System_Admin_S3GSysAdminCustomerAlert_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- <div id="divGrid1" style="overflow: auto; width: 100%;">--%>
                        <asp:GridView runat="server" ID="grvCustomerAlert" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader"
                            OnDataBound="grvCustomerAlert_DataBound" OnRowDataBound="grvCustomerAlert_RowDataBound"
                            Width="100%" OnRowCommand="grvCustomerAlert_RowCommand">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Customer_Alert_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandName="Modify" CommandArgument='<%# Bind("Customer_Alert_ID")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Entity Type" SortExpression="Entity_Type_Name" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEntityType" runat="server" Text='<%# Bind("Entity_Type_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnEntityType" OnClick="FunProSortingColumn" ToolTip="Sort By EntityType"
                                                        runat="server" Text="Entity Type" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnEntityType" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderEntity_Type" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderEntity_Type"
                                                        FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars=" ,-" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Event" SortExpression="Event_Type_Name" HeaderStyle-CssClass="styleGridHeader"
                                        HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEventType" runat="server" Text='<%# Bind("Event_Type_Name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr align="center">
                                                    <td>
                                                        <asp:LinkButton ID="lnkbtnEventType" OnClick="FunProSortingColumn" ToolTip="Sort By EventType"
                                                            runat="server" Text="Event Type" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnEventType" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                    </td>
                                                </tr>
                                                <tr align="left">
                                                    <td>
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderEventType" runat="server" AutoPostBack="true"
                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>
                                    </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Role Code" SortExpression="Program_Name + ' - ' + Role_Code"
                                    HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRoleCode" runat="server" Text='<%# Bind("Role_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnRoleCode" OnClick="FunProSortingColumn" ToolTip="Sort By RoleCode"
                                                        runat="server" Text="Role Description" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnRoleCode" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderRoleCode" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderRoleCode"
                                                        FilterType="UppercaseLetters,lowercaseLetters,Custom,Numbers" ValidChars=" ,-" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="LOB" SortExpression="LOB_Name" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnLOBCode" OnClick="FunProSortingColumn" ToolTip="Sort By Line of Business"
                                                        runat="server" Text="Line of Business" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnLOBCode" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderLOBCode" runat="server" AutoPostBack="true"
                                                        CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtHeaderLOBCode"
                                                        FilterType="UppercaseLetters,lowercaseLetters,Custom" ValidChars=" ,-" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Target EMail" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkEmail" />
                                        <asp:Label ID="lblEmail" Visible="false" runat="server" Text='<%#Eval("TargetEmail")%>'></asp:Label>
                                        <%--<asp:Label ID="lblEmail" runat="server" Text='<%# Bind("TargetEmail") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <%-- <HeaderTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr align="center">
                                                    <td>
                                                        <asp:LinkButton ID="lnkbtnTargetEmail" OnClick="FunProSortingColumn" ToolTip="Sort By TargetEmail"
                                                            runat="server" Text="Target Email" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnTargetEmail" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                    </td>
                                                </tr>
                                                <tr align="left">
                                                    <td>
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderTargetEmail" runat="server" AutoPostBack="true"
                                                            CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Target SMS" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                                    HeaderStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkSMS" />
                                        <asp:Label ID="lblSMS" Visible="false" runat="server" Text='<%#Eval("TargetSMS")%>'></asp:Label>
                                        <%--<asp:Label ID="lblProgramCode" runat="server" Text='<%# Bind("TargetSMS") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                    <%-- <HeaderTemplate>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr align="center">
                                                    <td>
                                                        <asp:LinkButton ID="lnkbtnTargetSMS" OnClick="FunProSortingColumn" ToolTip="Sort By TargerSMS"
                                                            runat="server" Text="Target SMS" CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnTargetSMS" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                    </td>
                                                </tr>
                                                <tr align="left">
                                                    <td>
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderTargetSMS" CssClass="styleSearchBox"
                                                            runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </HeaderTemplate>--%>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Active" HeaderStyle-Width="5%"
                                    HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:CheckBox Enabled="false" runat="server" ID="chkActive" />
                                        <asp:Label ID="lblActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
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
                        <%--</div>--%>
                        <%--     </td>
                </tr>
                </table>--%>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
