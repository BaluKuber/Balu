<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="SG3SysAdminRegionMaster_View.aspx.cs" Inherits="System_Admin_SG3SysAdminRegionMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        <tr>
            <td style="padding-top: 10px">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdPan1">
                    <ContentTemplate>
                        <cc1:TabContainer ID="tcRegBranch" runat="server" Width="100%" CssClass="styleTabPanel"
                            OnActiveTabChanged="tcRegBranch_ActiveTabChanged" AutoPostBack="true">
                            <cc1:TabPanel runat="server" ID="TabPanelRegion" CssClass="tabpan" BackColor="Red"
                                Width="100%">
                                <HeaderTemplate>
                                    Region Master</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr width="100%">
                                                    <td width="100%" align="center">
                                                        <div id="divUser" runat="server" style="overflow: auto;">
                                                            <asp:GridView runat="server" ID="gvRegionDetails" Width="100%" AutoGenerateColumns="False"
                                                                DataKeyNames="Region_ID" OnDataBound="gvRegionDetails_DataBound" OnRowDataBound="gvRegionDetails_RowDataBound"
                                                                OnRowCommand="gvRegionDetails_RowCommand">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRegionID" runat="server" Text='<%# Eval("Region_ID") %>' /></ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="7%">
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblRQuery" runat="server" Text="Query"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                                CommandArgument='<%# Bind("Region_ID") %>' CommandName="Query" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-Width="7%">
                                                                        <HeaderTemplate>
                                                                            <asp:Label ID="lblRModify" runat="server" Text="Modify"></asp:Label>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="imgbtnModify" runat="server" ImageUrl="~/Images/spacer.gif"
                                                                                CommandArgument='<%# Bind("Region_ID") %>' CommandName="Modify" CssClass="styleGridEdit" /></ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Region Code" SortExpression="Region_Code" HeaderStyle-Width="8%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRegionCode" runat="server" Text='<%# Bind("Region_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnRegionCode" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                                            OnClick="FunProSortingColumn" Text="Region Code"></asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnRegionCode" CssClass="styleImageSortingAsc" runat="server"
                                                                                            OnClientClick="return false;" AlternateText="Sort By Region Code" ImageAlign="Middle" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" TabIndex="0"
                                                                                            CssClass="styleSearchBox" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                                                            Width="100px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Region Name" SortExpression="Region_Name" HeaderStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRegionName" runat="server" Text='<%# Bind("Region_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                </tr>
                                                                                <td>
                                                                                    <asp:LinkButton ID="lnkbtnRegionName" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                                        OnClick="FunProSortingColumn" Text="Region Name"></asp:LinkButton>
                                                                                    <asp:ImageButton ID="imgbtnRegionName" CssClass="styleImageSortingAsc" runat="server"
                                                                                        OnClientClick="return false;" AlternateText="Sort By Region Name" ImageAlign="Middle" />
                                                                                </td>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" TabIndex="1" runat="server"
                                                                                            AutoPostBack="true" CssClass="styleSearchBox" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Active" HeaderStyle-Width="7%">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox Enabled="false" runat="server" ID="CbxRegionActive" />
                                                                            <asp:Label ID="lblRegionActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
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
                                                                <HeaderStyle CssClass="styleInfoLabel" Font-Underline="False" HorizontalAlign="Center" />
                                                               
                                                            </asp:GridView>
                                                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                                            &nbsp;</div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr class="styleButtonArea" align="center">
                                                    <td align="center">
                                                        <asp:Label ID="lblErrorMessageRegion" runat="server" CssClass="styleMandatoryLabel"
                                                            Style="color: Red; font-size: medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="btnRegionCreate" runat="server" CssClass="styleSubmitButton" OnClick="btnCreate_Click"
                                                            Text="Create" />
                                                        
                                                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                                                            OnClick="btnShowAll_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabPanelBranch" CssClass="tabpan" BackColor="Red"
                                Width="100%">
                                <HeaderTemplate>
                                    Branch Master</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <div id="div1" runat="server" style="overflow: auto;">
                                                <asp:GridView ID="gvBranch" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    DataKeyNames="Branch_ID" OnDataBound="gvBranch_DataBound" OnRowDataBound="gvBranch_RowDataBound"
                                                    OnRowCommand="gvBranch_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBranchID" runat="server" Text='<%# Eval("Branch_ID") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblBQuery" runat="server" Text="Query"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnBQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                    CommandArgument='<%# Bind("Branch_ID") %>' CommandName="Query" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblBModify" runat="server" Text="Modify"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnBModify" CssClass="styleGridEdit" runat="server" ImageUrl="~/Images/spacer.gif"
                                                                    CommandArgument='<%# Bind("Branch_ID") %>' CommandName="Modify" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Branch Code" SortExpression="Branch_Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBranchCode" runat="server" Text='<%# Bind("Branch_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="110px" style="width: 114px">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnBranchCode" runat="server" Text="Branch Code" OnClick="FunProBSortingColumn"
                                                                                CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnBranchCode" runat="server" CssClass="styleImageSortingAsc"
                                                                                OnClientClick="return false;" ImageAlign="Middle" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" CssClass="styleSearchBox"
                                                                                runat="server" AutoPostBack="true" Width="114px" OnTextChanged="FunProBHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Branch Name" SortExpression="Branch_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("Branch_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="110px">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnBranchName" runat="server" Text="Branch Name" OnClick="FunProBSortingColumn"
                                                                                CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnBranchName" runat="server" AlternateText="Sort By Branch Name"
                                                                                OnClientClick="return false;" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" CssClass="styleSearchBox"
                                                                                runat="server" AutoPostBack="true" Width="114px" OnTextChanged="FunProBHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Region Code" SortExpression="Region_Code" HeaderStyle-Width="8%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBRegionCode" runat="server" Text='<%# Bind("Region_Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="110px">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnRegionCodeBTab" runat="server" Text="Region Code" OnClick="FunProBSortingColumn"
                                                                                CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnRegionCodeBTab" runat="server" AlternateText="Sort By Region Code"
                                                                                OnClientClick="return false;" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" CssClass="styleSearchBox"
                                                                                runat="server" AutoPostBack="true" Width="100px" OnTextChanged="FunProBHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Region Name" SortExpression="Region_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBRegionBName" runat="server" Text='<%# Bind("Region_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="110px">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnRegionNameBTab" runat="server" Text="Region Name" OnClick="FunProBSortingColumn"
                                                                                CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnRegionNameBTab" runat="server" CssClass="styleImageSortingAsc"
                                                                                OnClientClick="return false;" AlternateText="Sort By Region Name" ImageAlign="Middle" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch6" CssClass="styleSearchBox"
                                                                                runat="server" AutoPostBack="true" Width="114px" OnTextChanged="FunProBHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="State Name" SortExpression="State_Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblStateName" runat="server" Text='<%# Bind("State_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0" width="110px">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnStateCode" runat="server" Text="State Name" OnClick="FunProBSortingColumn"
                                                                                CssClass="styleGridHeaderLinkBtn"></asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnStateCode" CssClass="styleImageSortingAsc" runat="server"
                                                                                OnClientClick="return false;" AlternateText="Sort By State Name" ImageAlign="Middle" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch7" runat="server" CssClass="styleSearchBox"
                                                                                AutoPostBack="true" Width="114px" OnTextChanged="FunProBHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Active">
                                                            <ItemTemplate>
                                                                <asp:CheckBox Enabled="false" runat="server" ID="CbxBranchActive" />
                                                                <asp:Label ID="lblBranchActive" Visible="false" runat="server" Text='<%#Eval("Is_Active")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Font-Underline="False" HorizontalAlign="Center" />
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
                                                    <HeaderStyle Height="20px" Font-Underline="False" CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                   
                                                </asp:GridView>
                                                <uc1:PageNavigator ID="ucCustomPagingBranch" runat="server"></uc1:PageNavigator>
                                            </div>
                                            <table width="100%">
                                                <tr class="styleButtonArea" align="center">
                                                    <td align="center">
                                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                                                            font-size: medium"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="btnBranchCreate" runat="server" CausesValidation="False" TabIndex="2"
                                                            CssClass="styleSubmitButton" Text="Create" OnClick="btnBranchCreate_Click" />
                                                        <asp:Button ID="btnBranchShowAll" runat="server" Text="Show All" TabIndex="3" CssClass="styleSubmitButton"
                                                            OnClick="btnBranchShowAll_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
    <input type="hidden" id="hdnSortDirectionBranch" runat="server" />
    <input type="hidden" id="hdnSortExpressionBranch" runat="server" />
    <input type="hidden" id="hdnSearchBranch" runat="server" />
    <input type="hidden" id="hdnOrderByBranch" runat="server" />
</asp:Content>
