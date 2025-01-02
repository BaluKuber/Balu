<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" 
CodeFile="S3G_ORG_FIRMasterView.aspx.cs" Inherits="Origination_S3G_ORG_FIRMasterView" Title="Untitled Page" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server"  ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                                               
                            <asp:GridView runat="server" ID="grvPaging" OnRowCommand="grvPaging_RowCommand"
                            OnRowDataBound="grvPaging_RowDataBound"
                            Width="100%" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader"
                            RowStyle-HorizontalAlign="Left">
                            <Columns>
                            
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("FIR_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" >
                                    
                                    <HeaderStyle CssClass="styleGridHeader" />
                                
                                     <HeaderTemplate>
                                        <asp:Label ID="lblEdit" runat="server" Text="Modify"></asp:Label>
                                    </HeaderTemplate>
                                    
                                    <ItemTemplate>
                                       <asp:ImageButton ID="imgbtnEdit" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif"
                                            CommandArgument='<%# Bind("FIR_ID") %>'  CommandName="Modify"
                                        runat="server" />
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                
                                <asp:TemplateField >
                                
                                <HeaderStyle CssClass="styleGridHeader" />
                                
                                <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort1" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                           OnClick="FunProSortingColumn" ToolTip="Sort By Line of Business" Text="Line of Business"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort1" CssClass="styleImageSortingAsc"  
                                                            runat="server" ImageAlign="Middle"/>
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" MaxLength="45" runat="server"
                                                        CssClass="styleSearchBox" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    
                                <ItemTemplate>
                                        <asp:Label ID="lblLOBName" runat="server" Text='<%# Bind("LOB_Code") %>'></asp:Label>
                                 </ItemTemplate>
                                 
                                </asp:TemplateField>
                                
                                <asp:TemplateField >
                                
                                <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                
                                <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort2" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                     OnClick="FunProSortingColumn" ToolTip="Sort By Product"
                                                        Text="Product"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort2" CssClass="styleImageSortingAsc" runat="server"
                                                         ImageAlign="Middle"  />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2"
                                                        MaxLength="46" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                        CssClass="styleSearchBox"></asp:TextBox>
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Product_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField >
                                
                                <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                
                                <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort3" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                     OnClick="FunProSortingColumn" ToolTip="Sort By Constitution"
                                                        Text="Constitution"></asp:LinkButton>
                                                    <asp:ImageButton ID="imgbtnSort3" CssClass="styleImageSortingAsc" runat="server"
                                                         ImageAlign="Middle"  />
                                                </td>
                                            </tr>
                                            <tr align="left">
                                                <td>
                                                    <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3"
                                                        MaxLength="45" runat="server" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"
                                                        CssClass="styleSearchBox"></asp:TextBox>
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lblConstitutionName" runat="server" Text='<%# Bind("Constitution_Code") %>'></asp:Label>
                                    </ItemTemplate>
                                    
                                </asp:TemplateField>
                                
                                 <asp:TemplateField  Visible="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader">
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
               
                <tr>
                    <td align="center" valign="top">
                    <uc1:PageNavigator  id="ucCustomPaging" runat="server"></uc1:PageNavigator></td>
                </tr>
                <tr >
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
