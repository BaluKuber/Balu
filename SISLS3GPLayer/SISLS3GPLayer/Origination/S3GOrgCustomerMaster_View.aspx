<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GOrgCustomerMaster_View.aspx.cs" Inherits="Origination_S3GOrgCustomerMaster_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
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
                            OnRowDataBound="grvPaging_RowDataBound" OnRowCommand="grvCustomerMaster_RowCommand">
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                            CommandArgument='<%# Bind("Customer_ID") %>' CommandName="Query" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Modify" SortExpression="Customer_ID" Visible="true"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                            AlternateText="Modify" CommandArgument='<%# Bind("Customer_ID") %>' runat="server"
                                            CommandName="Modify" />
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Code">
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Customer Code" ToolTip="Sort By CustomerCode"
                                                        CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
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
                                <asp:TemplateField HeaderText="Customer Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Customer Name" ToolTip="Sort By CustomerName"
                                                        CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
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
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerType" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Customer Type" ToolTip="Sort By CustomerType"
                                                        CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
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
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Manual Numbering">

                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr align="center">
                                                <td>
                                                    <asp:Label ID="lblManual_Num" runat="server" Text="Manual Numbering"></asp:Label>
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
                                        <asp:Label ID="lblManual_Numbering" runat="server" Text='<%# Bind("Is_Manual_Num") %>'></asp:Label>
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
                    <td align="center">
                        <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium"></span>
                        <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatory"></asp:Label--%>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px;" align="center">
                        <asp:Button ID="btnCreate" OnClick="btnCreate_Click" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Create" runat="server"></asp:Button>
                        <asp:Button ID="btnExportToExcel" OnClick="btnExportToExcel_Click" UseSubmitBehavior="true" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Export To Excel" runat="server"></asp:Button>
                        <asp:Button ID="btnExportToExcelAddr" width="150px"
                             OnClientClick="return fnExcelExportVal(this)" 
                            UseSubmitBehavior="true" CausesValidation="false" CssClass="styleSubmitLongButton"
                            Text="Export with Billing Address" runat="server"></asp:Button>
                        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                            OnClick="btnShowAll_Click" />
                        <asp:Button runat="server" ID="btnExportExcelRender" CausesValidation="false" OnClick="btnExportToExcelAddr_Click" Width="1px" style="visibility:hidden;" />
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
            <asp:PostBackTrigger ControlID="btnExportToExcelAddr" />
            <asp:PostBackTrigger ControlID="btnExportExcelRender" />
        </Triggers>
    </asp:UpdatePanel>

     <script language="javascript" type="text/javascript">

        function fnExcelExportVal(btn) {
            //btn.style.visibility = 'hidden';
            btn.disabled = true;
            //var a = event.srcElement;
            //a.style.display = 'block';
            //a.style.removeAttribute('display');
            document.getElementById('<%=btnExportExcelRender.ClientID%>').click();
             return true;
         }

     </script>

</asp:Content>
