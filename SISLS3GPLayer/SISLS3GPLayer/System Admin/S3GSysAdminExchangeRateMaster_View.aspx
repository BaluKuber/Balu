<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminExchangeRateMaster_View.aspx.cs" Inherits="S3GSysAdminExchangeMaster_View" %>

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
                    <td align="center">
                        <asp:GridView ID="grvLOBMaster" runat="server" Width="100%" AutoGenerateColumns="False"
                            DataKeyNames="Exchange_Rate_ID" OnDataBound="grvLOBMaster_DataBound" OnRowDataBound="grvLOBMaster_RowDataBound"
                            OnRowCommand="grvLOBMaster_RowCommand">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Exchange_Rate_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Modify">
                                    <%--                            <HeaderTemplate >
                                <asp:Label ID="lblEdit" runat="server" ></asp:Label>
                            </HeaderTemplate>--%>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" runat="server" ImageUrl="~/Images/spacer.gif" AlternateText="Modify"
                                            CommandArgument='<%# Bind("Exchange_Rate_ID") %>' CommandName="Modify" CssClass="styleGridEdit" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedDate" runat="server" Text='<%# ShowDate(Eval("Created_On").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="8%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Created Time">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCreatedTime" runat="server" Text='<%# ShowTime(Eval("Created_On").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Effective Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# ShowDate(Eval("Effective_Date").ToString())%>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="8%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Default Currency">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDefaultCurrency" runat="server" Text='<%# DefaultCurrencyDisplay() %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Default Value">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDefaultValue" runat="server" Text='<%# string.Format("{0:0}", DataBinder.Eval(Container.DataItem, "Default_Value")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exchange Currency" SortExpression="Exchange_Currency_Name">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSortCode" runat="server" CssClass="styleGridHeaderLinkBtn"
                                                        ToolTip="Sort By Exchange Currency" OnClick="FunProSortingColumn" Text="Exchange Currency"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSortCode" runat="server" CssClass="styleImageSortingAsc"
                                                        ImageAlign="Middle" />
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" MaxLength="40"
                                                        AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch1"
                                                        FilterType="UppercaseLetters,LowercaseLetters,Numbers,Custom" ValidChars="- "
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBCode" runat="server" Text='<%# Bind("Exchange_Currency_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exchange Value">
                                    <ItemTemplate>
                                        <%--<asp:Label ID="lblLOBName" runat="server" Text='<%# string.Format("{0:0.0000}", DataBinder.Eval(Container.DataItem, "Exchange_Value")) %>'></asp:Label>--%>
                                        <asp:Label ID="lblLOBName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Exchange_Value") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" Width="10%"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%# Bind("Created_By")%>'></asp:Label>
                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%# Bind("User_Level_ID")%>'></asp:Label>
                                        <asp:Label ID="lblPrecision" Visible="false" runat="server" Text='<%# Bind("Precision")%>'></asp:Label>
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
                    <td align="center" valign="top">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage" cssclass="styleMandatory" style="color: Red">
                        </span>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CssClass="styleSubmitButton"
                            CausesValidation="false" Text="Create" runat="server"></asp:Button>
                        
                        <asp:Button ID="btnShowAll" OnClick="btnShowAll_Click" runat="server" Text="Show All"
                            CssClass="styleSubmitButton" />
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