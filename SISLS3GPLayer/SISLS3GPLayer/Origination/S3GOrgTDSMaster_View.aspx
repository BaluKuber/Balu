<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"  EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="S3GOrgTDSMaster_View.aspx.cs" Inherits="Origination_S3GOrgTDSMaster_View" %>

<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                    OnRowCommand="grvTDSMaster_RowCommand" OnRowDataBound="grvTDSMaster_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Query" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                    AlternateText="Query" CommandArgument='<%# Bind("Tax_ID") %>' runat="server"
                                    CommandName="Query" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblQuery" runat="server" Text="Query" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Modify" SortExpression="ID" Visible="true" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                    AlternateText="Modify" CommandArgument='<%# Bind("Tax_ID") %>' runat="server"
                                    CommandName="Modify" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Company Type">
                            <ItemTemplate>
                                <asp:Label ID="lblCompanyType" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Company Type" ToolTip="Sort By Company type"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="50"></asp:TextBox>
                                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtHeaderSearch1"
                                                  FilterType="UppercaseLetters,LowercaseLetters,Numbers" Enabled="True">
                                                 </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax Code">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Tax Code" ToolTip="Sort By Tax code"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="12"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTaxCode" runat="server" Text='<%# Bind("Tax_Code") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Constituition">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Constituition" ToolTip="Sort By Constituition"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblConstituition" runat="server" Text='<%# Bind("ConstitutionName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax Section">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Tax Section" ToolTip="Sort By Tax Section"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTaxSection" runat="server" Text='<%# Bind("Tax_Law_Section") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>



                         <asp:TemplateField HeaderText="Tax Description">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort5" runat="server" Text="Tax Description" ToolTip="Sort By Tax Description"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort5" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTaxDescription" runat="server" Text='<%# Bind("Tax_Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>



                         <%--<asp:TemplateField HeaderText="Tax Type">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort5" runat="server" Text="Tax Type" ToolTip="Sort By Tax Type"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort5" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTaxtype" runat="server" Text='<%# Bind("Tax_Type") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                         <asp:TemplateField HeaderText="PAN Applicability">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort6" runat="server" Text="PAN Applicability" ToolTip="Sort By PAN Applicabilitye"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort6" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch6" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTaxtype" runat="server" Text='<%# Bind("PAN_APPLICABILITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Effective Date">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort7" runat="server" Text="Effective Date" ToolTip="Sort By Effective Date"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort7" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr style="display:none;" align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch7" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveFrom" runat="server" Text='<%# Bind("Effective_From") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Threshold Limit">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort8" runat="server" Text="Threshold Limit" ToolTip="Sort By Threshold Limit"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort8" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch8" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblThresholdLevel" runat="server" Text='<%# Bind("Threshold_Level") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                         <asp:TemplateField HeaderText="Tax">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr align="center">
                                        <td>
                                            <asp:LinkButton ID="lnkbtnSort9" runat="server" Text="Tax" ToolTip="Sort By Tax"
                                                CssClass="styleGridHeaderLinkBtn" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                            <asp:ImageButton ID="imgbtnSort9" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td>
                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch9" runat="server" AutoPostBack="true"
                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox" MaxLength="90"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTax" runat="server" Text='<%# Bind("Tax") %>'></asp:Label>
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
        
         <tr>
            <td align="center">
               <%-- <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium">
                </span>--%>
                <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
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
