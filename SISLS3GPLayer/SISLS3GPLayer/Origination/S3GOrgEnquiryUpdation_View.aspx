<%@ Page Title="LOB Master Details" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"  EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="S3GOrgEnquiryUpdation_View.aspx.cs" Inherits="Origination_S3GOrgEnquiryUpdation_View" %>

<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table class="stylePageHeading">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Enquiry Updation" ID="lblHeading" CssClass="styleInfoLabel">
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
                                    CommandArgument='<%# Bind("EnquiryUpdation_Id") %>' CommandName="Query" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modify" SortExpression="EnquiryUpdation_Id" Visible="true"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    AlternateText="Modify" CommandArgument='<%# Bind("EnquiryUpdation_Id") %>' runat="server"
                                    CommandName="Modify" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enquiry Number">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Enquiry Number" ToolTip="Sort By EnquiryNo"
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
                                <asp:Label ID="lblEnquiryNumber" runat="server" Text='<%# Bind("Enquiry_No") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer" ItemStyle-HorizontalAlign="Left">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Customer" ToolTip="Sort By Customer"
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
                                <asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("Customer_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Facility Amount" ItemStyle-HorizontalAlign="Right">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Facility Amount" ToolTip="Sort By FacilityAmount"
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
                                <asp:Label ID="lblFacilityAmount" runat="server" Text='<%# Bind("Facility_Amount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enquiry Date">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Enquiry Date" ToolTip="Sort By Enquiry Date"
                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" Width="90px"></asp:TextBox>
                                            <cc1:calendarextender runat="server" TargetControlID="txtHeaderSearch4" PopupButtonID="txtHeaderSearch4"
                                                Format="dd/MM/YYYY" ID="clEnqDate" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                            </cc1:calendarextender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEnquiryDate" runat="server" Text='<%# Bind("Enquiry_Date") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
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
            <td style="padding-top: 5px;" align="center">
                <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Create" runat="server"></asp:Button>
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
