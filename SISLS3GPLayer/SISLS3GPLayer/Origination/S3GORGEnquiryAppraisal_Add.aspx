<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GORGEnquiryAppraisal_Add.aspx.cs" Inherits="Origination_S3GORGEnquiryAppraisal_Add"
    Title="Enquiry Customer Appraisal" EnableEventValidation="false" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Enquiry / Customer Appraisal" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcEnquiryAppraisal" runat="server" ActiveTabIndex="2" CssClass="styleTabPanel"
                            Width="100%" TabStripPlacement="top" AutoPostBack="true" OnActiveTabChanged="tcEnquiryAppraisal_TabIndexChanged">
                            <cc1:TabPanel ID="tbAppraisalType" runat="server" HeaderText="Appraisal Type" CssClass="tabpan"
                                BackColor="Red" Width="99%">
                                <HeaderTemplate>
                                    Appraisal Type
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td>
                                                <table width="100%" align="center">
                                                    <tr align="center">
                                                        <td>
                                                            <asp:Label ID="lblDocumentType" runat="server" CssClass="styleDisplayLabel" Text="Document Type"></asp:Label>
                                                            &nbsp;
                                                            
                                                              <asp:DropDownList ID="ddlDocumentType" runat="server" ToolTip="Document Type" AutoPostBack="True" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                                                              </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvEnquiryAppraisal" runat="server" Width="100%" AutoGenerateColumns="False"
                                                    AllowSorting="True" OnRowCreated="gvEnquiryAppraisal_RowCreated" Visible="False" EnableModelValidation="True">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort1" CssClass="styleGridHeaderLinkBtn" runat="server" CausesValidation="false"
                                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Constitution Name" Text="Constitution"> </asp:LinkButton><asp:ImageButton
                                                                                    ID="imgbtnSort1" CssClass="styleImageSortingAsc" runat="server" ImageAlign="Middle"
                                                                                    CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" CssClass="styleSearchBox"
                                                                                AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblConstitutionName" runat="server" Text='<%# Bind("Constitution_Name") %>'
                                                                    align="left" Height="10px"> </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <ItemStyle Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CustomerID" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("Customer_ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Enquiry No" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEnquiryUpdationID" runat="server" Text='<%#Eval("ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort3" CssClass="styleGridHeaderLinkBtn" runat="server" CausesValidation="false"
                                                                                OnClick="FunProSortingColumn" ToolTip="Sort By LOB" Text="Line of Business"></asp:LinkButton><asp:ImageButton
                                                                                    ID="imgbtnSort3" CssClass="styleImageSortingAsc" runat="server" ImageAlign="Middle"
                                                                                    CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" CssClass="styleSearchBox"
                                                                                AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort4" CssClass="styleGridHeaderLinkBtn" runat="server" CausesValidation="false"
                                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Location" Text="Location"></asp:LinkButton><asp:ImageButton
                                                                                    ID="imgbtnSort4" CssClass="styleImageSortingAsc" runat="server" ImageAlign="Middle"
                                                                                    CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" CssClass="styleSearchBox"
                                                                                AutoPostBack="true" OnTextChanged="FunProHeaderSearch" Width="140px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Location_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort5" CssClass="styleGridHeaderLinkBtn" runat="server" CausesValidation="false"
                                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Product" Text="Product"></asp:LinkButton><asp:ImageButton
                                                                                    ID="imgbtnSort5" CssClass="styleImageSortingAsc" runat="server" ImageAlign="Middle"
                                                                                    CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" runat="server" CssClass="styleSearchBox"
                                                                                AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("Product_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort2" CssClass="styleGridHeaderLinkBtn" runat="server" CausesValidation="false"
                                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Enquiry No." Text="Document Number">
                                                                            </asp:LinkButton><asp:ImageButton
                                                                                ID="imgbtnSort2" CssClass="styleImageSortingAsc" runat="server" ImageAlign="Middle"
                                                                                CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" CssClass="styleSearchBox"
                                                                                AutoPostBack="true" OnTextChanged="FunProHeaderSearch" Width="120px" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEnquiryNo" runat="server" Text='<%# Bind("Number") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort6" CssClass="styleGridHeaderLinkBtn" runat="server" CausesValidation="false"
                                                                                OnClick="FunProSortingColumn" ToolTip="Sort By Customer Name" Text="Customer"></asp:LinkButton><asp:ImageButton
                                                                                    ID="imgbtnSort6" CssClass="styleImageSortingAsc" runat="server" ImageAlign="Middle"
                                                                                    CausesValidation="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch6" runat="server" CssClass="styleSearchBox"
                                                                                AutoPostBack="true" OnTextChanged="FunProHeaderSearch"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <ItemStyle Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Select
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelectRecord" runat="server" AutoPostBack="true" align="center"
                                                                    OnCheckedChanged="chkSelectRecord_OnCheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <RowStyle HorizontalAlign="Left" Height="10px" />
                                                </asp:GridView>
                                                <uc1:PageNavigator ID="ucCustomPaging" runat="server" Visible="False"></uc1:PageNavigator>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                Enabled="false" BackColor="Red">
                                <HeaderTemplate>
                                    General
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlHeader" runat="server" CssClass="stylePanel" GroupingText="Header">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel" width="16%">
                                                            <asp:Label ID="lblEnquiryno" runat="server" Text="Enquiry Number" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldLabel" width="31%">
                                                            <asp:TextBox ID="txtEnquiryNo" runat="server" ContentEditable="False"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel" width="22%">
                                                            <asp:Label ID="lblproductcode" runat="server" Text="Product" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:TextBox ID="txtproductcode" runat="server" ContentEditable="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="16%">
                                                            <asp:Label ID="lblTransaction" CssClass="styleDisplayLabel" runat="server" Text="Transaction Type"
                                                                Style="padding-top: 10px;"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldLabel" width="31%">
                                                            <asp:TextBox ID="txtTransactionType" runat="server" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel" width="22%">
                                                            <asp:Label ID="lblDate" runat="server" ContentEditable="False" Text="Date" CssClass="styleReqFieldLabel"></asp:Label><asp:Label
                                                                ID="lblConstitution" runat="server" Text="Constitution" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:TextBox ID="txtDate" ContentEditable="False" runat="server" Width="120px"></asp:TextBox><asp:Image
                                                                ID="imgDate" runat="server" ImageUrl="~/Images/calendaer.gif" /><asp:RequiredFieldValidator
                                                                    ID="rfvDate" runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Enter the Date of Joining"
                                                                    CssClass="styleSummaryStyle"></asp:RequiredFieldValidator><cc1:CalendarExtender runat="server"
                                                                        Format="dd/MM/yyyy" TargetControlID="txtDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                        PopupButtonID="imgDate" ID="CalendarExtender1" Enabled="True">
                                                                    </cc1:CalendarExtender>
                                                            <asp:TextBox ID="txtConstitution" ContentEditable="False" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlCustomerInfo" runat="server" CssClass="stylePanel" GroupingText="Customer Information"
                                                ScrollBars="None">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <uc1:S3GCustomerAddress ID="S3GCustomerPermAddress" ActiveViewIndex="1" runat="server"
                                                                FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" SecondColumnWidth="31%"
                                                                FirstColumnWidth="16%" ThirdColumnWidth="23%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel1" runat="server" GroupingText="Document Information" CssClass="stylePanel">
                                                <table width="100%">
                                                    <tr runat="server">
                                                        <td colspan="4" runat="server">
                                                            <asp:GridView ID="GrvEnquiryDocDetails" runat="server" AutoGenerateColumns="False"
                                                                Width="100%" EditIndex="99">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAppDocID" Text='<%# Eval("Enq_Cus_App_Doc_ID")%>' MaxLength="50"
                                                                                Width="110px" runat="server" Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Document ID" Visible="False">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEnqDocCatID" runat="server" Text='<%# Eval("Doc_ID") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Document Pending" ItemStyle-Width="50%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEnqDocPending" CssClass="styleReqFieldLabel" runat="server" Text='<%# Eval("Doc_Pending") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Shift Progress To">
                                                                        <ItemTemplate>
                                                                            <asp:DropDownList ID="ddlEnqShiftProgTo" runat="server">
                                                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="1">Pricing</asp:ListItem>
                                                                                <asp:ListItem Value="2">Application</asp:ListItem>
                                                                                <asp:ListItem Value="3">Ignore</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <asp:CustomValidator ID="csvShift" Display="None" runat="server" CssClass="styleSummaryStyle"
                                                                OnServerValidate="csvShift_ServerValidate" ValidationGroup="General"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="16%">
                                                            <asp:Label ID="lblfurtherprocess" runat="server" Text="Further Process  " CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:RadioButton ID="Rdbfurtherprocyes" runat="server" GroupName="gn" Text="Yes"
                                                                Checked="True" OnCheckedChanged="Rdbfurtherprocyes_CheckedChanged" />
                                                            <asp:RadioButton ID="Rdbfurtherprocno" runat="server" GroupName="gn" Text="No"
                                                                OnCheckedChanged="Rdbfurtherprocno_CheckedChanged" />
                                                            <asp:CustomValidator ID="csvOption"
                                                                Display="None" runat="server" CssClass="styleSummaryStyle" OnServerValidate="csvOption_ServerValidate"> </asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Facility" ID="tbfacility" CssClass="tabpan"
                                Enabled="false" BackColor="Red">
                                <HeaderTemplate>
                                    Facility
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <table width="99%">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="pnlFacilityDDL" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td class="styleFieldLabel" style="padding-top: 10px" width="20%">
                                                                        <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="padding-top: 10px">
                                                                        <asp:TextBox ID="txtLOB" ContentEditable="False" runat="server"></asp:TextBox><asp:DropDownList
                                                                            ID="ddlLOB" Visible="false" runat="server" Width="205px">
                                                                        </asp:DropDownList>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="rfvLOB" runat="server" InitialValue="0" ValidationGroup="General"
                                                                                ErrorMessage="Select a Line of Business" ControlToValidate="ddlLOB" Display="None"
                                                                                Enabled="false" CssClass="styleSummaryStyle" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel" style="padding-top: 10px">
                                                                        <asp:Label ID="lblfacilityamt" runat="server" Text="Facility Amount" CssClass="styleReqFieldLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="padding-top: 10px">
                                                                        <asp:TextBox ID="txtFacilityamt" MaxLength="10" runat="server" Width="90px"></asp:TextBox><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                                            runat="server" FilterType="Numbers" TargetControlID="txtFacilityamt">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RequiredFieldValidator ID="rqvAmount" runat="server" ControlToValidate="txtFacilityamt"
                                                                            ErrorMessage="Enter a Facility Amount" CssClass="styleSummaryStyle" Display="None"
                                                                            ValidationGroup="General" Enabled="false" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <table cellpadding="0" cellspacing="0" runat="server" id="tblFacility">
                                                            <tr>
                                                                <td>
                                                                    <table class="styleGridView" width="100%" cellspacing="0">
                                                                        <tr>
                                                                            <th class="stylePagingControl" align="left">
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td>&nbsp;&nbsp;<asp:Label ID="lblCaption" runat="server" Text="FACILITY" />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <span style="font-weight: normal">Total</span>
                                                                                        </td>
                                                                                        <td style="width: 100px" align="right">
                                                                                            <asp:Label ID="lblTotalAmount" Style="font-weight: normal" runat="server" CssClass="txtRightAlign"></asp:Label>
                                                                                        </td>
                                                                                        <td style="width: 65px" align="center"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </th>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-top: 10px">
                                                                    <table class="styleGridView" width="100%" cellspacing="0">
                                                                        <tr>
                                                                            <th class="stylePagingControl" align="left">
                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                    <tr>
                                                                                        <td width="160px" align="center">
                                                                                            <asp:Label ID="Label2" runat="server" Text="Line of Bussiness" />
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="Label4" runat="server" Text="Product" />
                                                                                        </td>
                                                                                        <td width="100px" align="center">
                                                                                            <asp:Label ID="Label5" runat="server" Text="Facility Amount" />
                                                                                        </td>
                                                                                        <td style="width: 65px" align="center" runat="server" id="tdAction">
                                                                                            <asp:Label ID="Label7" runat="server" Text="Action" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </th>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="padding-top: 10px">
                                                                    <asp:GridView ID="grvFacilityGroup" runat="server" ShowFooter="true" ShowHeader="false" Style="min-width: 500px; border: none;" GridLines="None"
                                                                        AutoGenerateColumns="false" OnRowDataBound="grvFacilityGroup_RowDataBound">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <table width="100%" class="styleGridView" cellspacing="0" cellpadding="0" style="border-bottom: none; padding: 0px;">
                                                                                        <tr>
                                                                                            <th align="left" style="height: 20px; width: 100%">
                                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label Visible="false" ID="lblHGroupId" runat="server" Text='<%#Eval("Group_ID")%>' />
                                                                                                            &nbsp;&nbsp;<asp:Label ID="lblhead1" runat="server" Text="Group : " />
                                                                                                            <asp:Label ID="lblHGroup" runat="server" Text='<%#Eval("Group_Text")%>' />
                                                                                                        </td>
                                                                                                        <td align="right">
                                                                                                            <span style="font-weight: normal; display:none">Sub-Total</span>
                                                                                                        </td>
                                                                                                        <td style="width: 100px">
                                                                                                            <asp:Label ID="lblHFacilityAmount" Style="font-weight: normal" runat="server" Width="100px" Text='<%#Eval("Total_Amount")%>' CssClass="txtRightAlign"></asp:Label>

                                                                                                        </td>
                                                                                                        <td style="width: 65px" align="center" runat="server" id="tdGroupDelete">
                                                                                                            <asp:ImageButton ID="imgbtnGroupDelete" OnClick="imgbtnGroupDelete_OnClick" runat="server" ImageUrl="~/Images/delete1.png" Style="width: 13px" ToolTip="Delete Group" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </th>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td width="100%" style="padding: 0px;">
                                                                                                <asp:GridView ID="grvFacility" ShowHeader="false" Style="padding: 0px; border-left: none; border-right: none;" Width="100%" runat="server" AutoGenerateColumns="false" OnRowDataBound="grvFacility_RowDataBound" ShowFooter="true">
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="Line of Bussiness" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px" FooterStyle-Width="160px">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label Visible="false" ID="lblLOBId" runat="server" Text='<%#Eval("LOB_ID")%>' />
                                                                                                                <asp:Label ID="lblLOB" runat="server" Text='<%#Eval("LOB")%>' />
                                                                                                                <asp:DropDownList ID="ddlLOB" Visible="false" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                                                                    AutoPostBack="true" runat="server">
                                                                                                                </asp:DropDownList>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:DropDownList ID="ddlFLOB" runat="server" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                                                                    AutoPostBack="true">
                                                                                                                </asp:DropDownList>
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label Visible="false" ID="lblProductId" runat="server" Text='<%#Eval("Product_ID")%>' />
                                                                                                                <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("Product")%>' />
                                                                                                                <asp:DropDownList Visible="false" ID="ddlProduct" runat="server" Style="min-width: 120px">
                                                                                                                </asp:DropDownList>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:DropDownList ID="ddlFProduct" runat="server" Style="min-width: 120px">
                                                                                                                </asp:DropDownList>
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <%--<asp:TemplateField HeaderText="Grouping">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label Visible="false" ID="lblGroupId" runat="server" Text='<%#Eval("Group_ID")%>' />
                                                                                                                <asp:DropDownList ID="ddlGroup" runat="server">
                                                                                                                </asp:DropDownList>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:DropDownList ID="ddlFGroup" runat="server">
                                                                                                                </asp:DropDownList>
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>--%>
                                                                                                        <asp:TemplateField HeaderText="Facility Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" FooterStyle-Width="100px">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblFacilityAmount" runat="server" Text='<%#Eval("Facility_Amount")%>'></asp:Label>
                                                                                                                <asp:TextBox ID="txtFacilityAmount" Visible="false" MaxLength="10" runat="server" Text='<%#Eval("Facility_Amount")%>'
                                                                                                                    Width="100px" CssClass="txtRightAlign"></asp:TextBox>
                                                                                                                <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" TargetControlID="txtFacilityAmount"
                                                                                                                    FilterType="Numbers" Enabled="True">
                                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:TextBox ID="txtFFacilityAmount" MaxLength="10" runat="server"
                                                                                                                    Width="100px" CssClass="txtRightAlign"></asp:TextBox>
                                                                                                                <cc1:FilteredTextBoxExtender ID="fteFAmount1" runat="server" TargetControlID="txtFFacilityAmount"
                                                                                                                    FilterType="Numbers" Enabled="True">
                                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="60px" FooterStyle-Width="60px">
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Panel runat="server" ID="pnlItemActions">
                                                                                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" ImageUrl="~/Images/Blue_2/modify_blue.gif" OnClick="imgbtnEdit_OnClick" ToolTip="Edit" />
                                                                                                                    &nbsp;
                                                                                                                <asp:ImageButton ID="imgbtnDelete" OnClick="imgbtnDelete_OnClick" runat="server" ImageUrl="~/Images/delete1.png" Style="width: 13px" ToolTip="Delete" />
                                                                                                                </asp:Panel>
                                                                                                                <asp:Panel runat="server" ID="pnlEditActions" Visible="false">
                                                                                                                    <asp:ImageButton ID="imgbtnUpdate" runat="server" ImageUrl="~/Images/Blue_2/modify_blue.gif" OnClick="imgbtnUpdate_OnClick" ToolTip="Update" />
                                                                                                                    &nbsp;
                                                                                                                <asp:ImageButton ID="imgbtnCancel" OnClick="imgbtnCancel_OnClick" runat="server" ImageUrl="~/Images/uparrow_07.png" Style="width: 15px; height: 15px" ToolTip="Cancel" />
                                                                                                                </asp:Panel>
                                                                                                                <%--<asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Inflow"
                                                                            OnClick="btnAdd_OnClick" CssClass="styleGridShortButton" />--%>
                                                                                                            </ItemTemplate>
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Grid"
                                                                                                                    OnClick="btnAdd_OnClick" CssClass="styleGridShortButton" />
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                </asp:GridView>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <div style="height: 10px; border: none"></div>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <table cellpadding="0" class="styleGridView" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td align="left">&nbsp;&nbsp;<asp:Label ID="lblFoot1" runat="server" Text="Group : " />
                                                                                                <asp:DropDownList ID="ddlHFGroup" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHFGroup_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <asp:Label runat="server" ID="lblSpace" Width="150px"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <span style="font-weight: normal">Sub-Total</span>
                                                                                            </td>
                                                                                            <td style="width: 100px">
                                                                                                <asp:TextBox ID="txtFFacilityAmount" MaxLength="10" runat="server"
                                                                                                    Width="100px" CssClass="txtRightAlign"></asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="fteFFacilityAmount" runat="server" TargetControlID="txtFFacilityAmount"
                                                                                                    FilterType="Numbers" Enabled="True">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </td>
                                                                                            <td style="width: 60px">
                                                                                                <asp:Button ID="btnHAdd" runat="server" Text="Add" CausesValidation="true"
                                                                                                    OnClick="btnHAdd_OnClick" CssClass="styleGridShortButton" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <%--<asp:Panel ID="pnlfacility"  GroupingText="Facility" runat="server" CssClass="stylePanel">--%>

                                                        <%--</asp:Panel>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CustomValidator ID="cusGrid" runat="server" CssClass="styleMandatoryLabel"
                                                            Enabled="true" Width="98%" Visible="false" ValidationGroup="Grid" />
                                                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                                                            CssClass="styleSummaryStyle" Width="99%" ShowMessageBox="false"
                                                            ShowSummary="true" ValidationGroup="Grid" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px" width="94%">&nbsp;
                    </td>
                </tr>
                <tr align="center" class="styleButtonArea" style="padding-left: 4px">
                    <td>
                        <asp:Button runat="server" ID="btnFacSave" Text="Save" OnClick="btnFacSave_Click"
                            CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('vgTabFirst');"
                            CausesValidation="true" Height="26px" ValidationGroup="General" Visible="false" />
                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClick="btnClear_Click"
                            CssClass="styleSubmitButton" Text="Clear" OnClientClick="return fnConfirmClear();" Visible="false" />
                        <asp:Button ID="btnShowAll" runat="server" CausesValidation="False"
                            CssClass="styleSubmitButton" Text="Show All" Visible="false"
                            OnClick="btnShowAll_Click" ToolTip="Show All" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click"
                            CssClass="styleSubmitButton" Text="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsGeneral" HeaderText="Correct the following validation(s):"
                            Height="250px" CssClass="styleSummaryStyle" Width="99%" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="General" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
</asp:Content>
