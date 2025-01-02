<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GOrgCreditParameterApproval.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" Title="Credit Parameter Approval"
    Inherits="Origination_S3GOrgCreditParameterApproval" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Common/S3GMasterPageCollapse.master" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="ContentCreditParameterApproval" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Credit Parameter Approval" ID="lblHeading" CssClass="styleDisplayLabel">
                </asp:Label>
            </td>
        </tr>
        <%--Spacer--%>
        <tr>
            <td height="5px">
            </td>
        </tr>
    </table>
    <asp:UpdatePanel runat="server" UpdateMode="Always" ID="updPanelHead">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="80%" align="center">
                                    <table width="80%">
                                        <tr>
                                            <td width="30%" valign="middle">
                                                <asp:Panel ID="Panel1" runat="server" GroupingText="Credit Parameter Type" CssClass="stylePanel"
                                                    EnableTheming="true">
                                                    <table width="100%" align="center">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:Label ID="lblDocumentType" runat="server" CssClass="styleDisplayLabel" Text="Document Type"> </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                               <%-- <asp:RadioButton ID="RBEnquiryNumber" runat="server" Checked="True" GroupName="CreditParametersSorting"
                                                                    OnCheckedChanged="RBEnquiryNumber_CheckedChanged" Text="Enquiry" Visible="false" />
                                                                <asp:RadioButton ID="RBCustomerCode" runat="server" GroupName="CreditParametersSorting"
                                                                    OnCheckedChanged="RBCustomerCode_CheckedChanged" Text="Customer" Visible="false" />--%>
                                                                <asp:DropDownList ID="ddlDocumentType" runat="server" ToolTip="Document Type"> 
                                                                <%--AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged"--%>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td width="30%" valign="middle">
                                                <asp:Panel ID="Panel2" runat="server" GroupingText="Status" CssClass="stylePanel"
                                                    EnableTheming="true">
                                                    <asp:RadioButton ID="RBAll" runat="server" GroupName="CreditParameters" OnCheckedChanged="RBAll_CheckedChanged"
                                                        Text="All" />
                                                    <asp:RadioButton ID="RBUnapproved" runat="server" GroupName="CreditParameters" OnCheckedChanged="RBUnapproved_CheckedChanged"
                                                        Text="Unapproved" Checked="True" />
                                                </asp:Panel>
                                            </td>
                                            <td width="20%" align="left" valign="middle">
                                                <asp:Button ID="btnFindRecords" runat="server" CssClass="styleSubmitShortButton"
                                                    OnClick="btnFindRecords_Click" TabIndex="0" Text="Go" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%--go button column--%>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="5px">
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:GridView runat="server" ID="gvCreditParameter" OnRowDataBound="gvCreditParameter_RowDataBound"
                                         AutoGenerateColumns="False" Visible="false" 
                                        Width="100%">
                                       
                                        <Columns>
                                            <%--Column 2 - serail Number Column  --%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Serial Number">
                                                <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActive" runat="server" Text='<%#Eval("SerialNumber")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <%--Column 3 - LOB Column  --%>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnSortLOB" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Line of Business"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSortLOB" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Line of Business" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" CssClass="styleSearchBox"
                                                                    Width="130px" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOBCodeName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Column 4 - Branch Column  --%>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnSortBranch" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Location"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSortBranch" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Location" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" CssClass="styleSearchBox"
                                                                    Width="130px" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("LocationCodeName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Column 5 - Product Column  --%>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnSortProduct" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Product"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSortProduct" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Product" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" CssClass="styleSearchBox"
                                                                    Width="130px" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("ProductCodeName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Column 5 - Sanction Number Column  --%>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkSanctionNo" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Product"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSortSanction" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Product" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="lnkSanctionNoSearch" runat="server" CssClass="styleSearchBox"
                                                                    Width="130px" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSanctionNo" runat="server" Text='<%# Bind("ProductCodeName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Column 6 - Enquiry No Column  --%>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnSortEnquiryNo" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Document No"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSortEnquiryNo" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Enquiry No" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" CssClass="styleSearchBox"
                                                                    AutoPostBack="true" Width="70px" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryNo" runat="server" Text='<%# Bind("DOC_NUMBER") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Column 7 - Enquiry Date Column  --%>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnSortEnquiryDate" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Document Date"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnSortEnquiryDate" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Enquiry Date" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" runat="server" CssClass="styleSearchBox"
                                                                    Width="80px" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnquiryDate" runat="server" Text='<%# Bind("DOC_Date")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Column 9 - Customenr Name Column  --%>
                                            <asp:TemplateField>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnCustomerName" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Customer"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnCustomerName" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Customer Name" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch7" runat="server" CssClass="styleSearchBox"
                                                                    Width="100px" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("CustomerCodeName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--Column 9 - Constitution Name Column  --%>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr align="center">
                                                            <td>
                                                                <asp:LinkButton ID="lnkbtnConstitutionName" CssClass="styleGridHeaderLinkBtn" runat="server"
                                                                    OnClick="FunProSortingColumn" Text="Constitution"></asp:LinkButton>
                                                                <asp:ImageButton ID="imgbtnConstitutionName" CssClass="styleImageSortingAsc" runat="server"
                                                                    AlternateText="Sort By Constitution Name" ImageAlign="Middle" />
                                                            </td>
                                                        </tr>
                                                        <tr align="left">
                                                            <td>
                                                                <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch8" runat="server" CssClass="styleSearchBox"
                                                                    Width="100px" AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblConstitutionName" runat="server" Text='<%#Eval("ConstitutionName")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Click to approve">
                                                <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkApprove" OnCheckedChanged="chkApprove_Changed" AutoPostBack="true"
                                                        runat="server" Enabled="true" Checked='<%# Bind("Approve") %>'></asp:CheckBox>
                                                    <asp:Label Visible="false" ID="lblCompanyId" runat="server" Text='<%#Eval("CompanyId")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblLOBId" runat="server" Text='<%#Eval("LOBId")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblProductId" runat="server" Text='<%#Eval("ProductId")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblConstitutionID" runat="server" Text='<%#Eval("ConstitutionID")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblCustomerId" runat="server" Text='<%#Eval("CustomerId")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblCustomerCode" runat="server" Text='<%#Eval("CustomerCode")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblEnquiryNoHdn" runat="server" Text='<%#Eval("DOC_NUMBER")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblLocationId" runat="server" Text='<%#Eval("LocationID")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblNumOfApproved" runat="server" Text='<%#Eval("NumOfApproved")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblApprovedStatus" runat="server" Text='<%#Eval("ApprovalStatus")%>'></asp:Label>
                                                    <asp:Label Visible="false" ID="lblCreditParamTransID" runat="server" Text='<%#Eval("CreditParamTransID")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" valign="top">
                                    <uc1:PageNavigator ID="ucCustomPaging" Visible="false" runat="server"></uc1:PageNavigator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <asp:Button runat="server" ID="btnShowAll" Text="Show All" CssClass="styleSubmitButton"
                            OnClick="btnShowAll_Click"  Visible="false"/>
                        &nbsp;&nbsp;
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage" class="styleMandatoryLabel"></span>
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
