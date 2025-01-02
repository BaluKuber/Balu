<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GOrgPDDTMaster_View.aspx.cs" Inherits="Origination_S3GOrgPDDTMaster_View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
      <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                <ContentTemplate>
                                 <table id="tbMain" runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="left" class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grvPaging" runat="server" CssClass="styleInfoLabel" AutoGenerateColumns="False"
                    DataKeyNames="PreDisbursement_Doc_Tran_ID" Width="100%" 
                    OnRowCommand="grvPaging_RowCommand" onrowdatabound="grvPaging_RowDataBound">
                    <Columns>
                        <asp:TemplateField Visible="False">
                            <ItemTemplate>
                                <asp:Label ID="lblCashflowId" runat="server" Text='<%# Eval("PreDisbursement_Doc_Tran_ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    CommandArgument='<%# Bind("PreDisbursement_Doc_Tran_ID") %>' CommandName="Query"
                                    runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                         <HeaderStyle CssClass="styleGridHeader" />
                        <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnModify" runat="server" CssClass="styleGridEdit" ImageUrl="~/Images/spacer.gif" CommandArgument='<%# Bind("PreDisbursement_Doc_Tran_ID") %>'
                                 CommandName="Modify" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="LOB" SortExpression="LOB" HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                                <asp:Label ID="lblALOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label></ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                OnClick="FunProSortingColumn" ToolTip="Sort By LOB" Text="Line of Business"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort1" CssClass="styleImageSortingAsc" runat="server"
                                                ImageAlign="Middle" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" MaxLength="50" runat="server"
                                                CssClass="styleSearchBox" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtHeaderSearch1"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderStyle CssClass="styleGridHeader" />
                            <ItemStyle Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Constitution Name" SortExpression="Constitution_Name"
                            HeaderStyle-CssClass="styleGridHeader">
                            <ItemTemplate>
                                <asp:Label ID="lblConstitutionName" runat="server" Text='<%# Bind("Constitution_Name") %>'></asp:Label></ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort2" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                OnClick="FunProSortingColumn" ToolTip="Sort By Constitution" Text="Constitution"></asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort2" CssClass="styleImageSortingAsc" runat="server"
                                                ImageAlign="Middle" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" MaxLength="50" runat="server"
                                                CssClass="styleSearchBox" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch2"
                                                FilterType="UppercaseLetters,LowercaseLetters,Numbers" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <HeaderStyle CssClass="styleGridHeader" />
                            <HeaderStyle CssClass="styleGridHeader" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Enquiry_No" HeaderText="Enquiry Number" HeaderStyle-CssClass="styleGridHeader">
                        </asp:BoundField>
                        <asp:BoundField DataField="PRDDC_Number" HeaderText="PRDDC Number" HeaderStyle-CssClass="styleGridHeader">
                        </asp:BoundField>
                        <asp:BoundField DataField="StatusName" HeaderText="Status" HeaderStyle-CssClass="styleGridHeader">
                        </asp:BoundField>
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
                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" Style="color: Red;
                    font-size: medium"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                    Text="Create" OnClick="btnCreate_Click" />
                &nbsp;
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
