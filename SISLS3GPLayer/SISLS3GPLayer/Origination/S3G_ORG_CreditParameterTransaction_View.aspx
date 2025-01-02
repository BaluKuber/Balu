 <%@ Page Title="LOB Master Details" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3G_ORG_CreditParameterTransaction_View.aspx.cs" Inherits="Origination_S3G_ORG_CreditParameterTransaction_View" %>
 <%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table class="stylePageHeading">
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Credit Parameter Transaction" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="false" OnRowCommand="grvPaging_RowCommand"
                            OnRowDataBound="grvPaging_RowDataBound" Width="100%">
                            <Columns>
                                 <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("CreditParamTrans_Id") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                        <asp:TemplateField HeaderText="Modify" SortExpression="EnquiryUpdation_Id" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    AlternateText="Modify" CommandArgument='<%# Bind("CreditParamTrans_Id") %>' runat="server"
                                    CommandName="Modify" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit Parameter Transaction Number">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Credit Parameter Transaction Number" ToolTip="Sort By Credit Paramter Transaction No"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
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
                            <ItemTemplate>
                                <asp:Label ID="lblCreditParamNumber" runat="server" Text='<%# Bind("CreditParamNumber") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit Parameter Date">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Credit Parameter Date" ToolTip="Sort By Credit Parameter Date"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
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
                                <asp:Label ID="lblCreditParamDate" runat="server" Text='<%# Bind("CreditParamDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Code" >
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Customer Code" ToolTip="Sort By Customer Code"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
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
                                <asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("Customer_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" >
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Status" ToolTip="Sort By Status"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
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
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
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
                <uc1:PageNavigator ID="ucCustomPaging" runat="server"  />
            </td>
        </tr>
                <tr>
                    <td style="padding-top: 5px;" align="center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Create" runat="server"></asp:Button>
                            &nbsp;&nbsp;
                <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                    OnClick="btnShowAll_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <span runat="server" id="lblErrorMessage" cssclass="styleMandatory" style="color: Red">
                        </span>
                        <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>--%>
                    </td>
                </tr>
            </table>
        
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
</asp:Content>
