<%@ Page Title="Credit Parameter Transaction" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3G_ORG_CreditParameterTransaction.aspx.cs"
    Inherits="Origination_S3G_ORG_CreditParameterTransaction" EnableEventValidation="false" %>

<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <asp:UpdatePanel ID="UpdatePanelCPT" runat="server">
        <ContentTemplate>--%>
    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <table class="stylePageHeading" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td width="100%">
                <cc1:TabContainer ID="TabContainerCPT" runat="server" ActiveTabIndex="1" CssClass="styleTabPanel"
                    Width="100%" ScrollBars="None">
                    <cc1:TabPanel runat="server" ID="TabCreditParameterQuery" CssClass="tabpan" Width="100%"
                        BackColor="Red">
                        <HeaderTemplate>
                            Appraisal Type
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlParameterType" runat="server" GroupingText="Appraisal Type" CssClass="stylePanel"
                                        Width="98%">
                                        <table width="100%" align="center">
                                            <tr align="center">
                                                <td>
                                                    <asp:Label ID="lblDocumentType" runat="server" CssClass="styleDisplayLabel" Text="Document Type"> </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <%--<asp:RadioButtonList ID="rdnlCreditType" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="True" Visible="false" OnSelectedIndexChanged="rdnlCreditType_SelectedIndexChanged">
                                                        <asp:ListItem Text="Enquiry" Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>--%>
                                                    <asp:DropDownList ID="ddlDocumentType" runat="server" ToolTip="Document Type" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlCreateCreditParam" runat="server" CssClass="stylePanel" Visible="false">
                                        <asp:GridView ID="grvEnquiryPaging" runat="server" AutoGenerateColumns="False" Width="100%"
                                            Visible="false" OnRowEditing="grvEnquiryPaging_RowEditing" OnRowDataBound="grvEnquiryPaging_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEnqCusAppId" runat="server" Text='<%# Bind("Enq_Cus_App_ID") %>'
                                                            CssClass="styleGridHeader"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerId" runat="server" Text='<%# Bind("Customer_Id") %>' CssClass="styleGridHeader"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEnquiryResponseId" runat="server" Text='<%# Bind("Document_Type_Id") %>'
                                                            CssClass="styleGridHeader"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="LOB Name">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Line of Business" ToolTip="Sort By LOBName"
                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"
                                                            Width="140px"></asp:TextBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLobName" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Location" ToolTip="Sort By Location"
                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"
                                                            Width="130px"></asp:TextBox>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Product" ToolTip="Sort By Product"
                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"
                                                            Width="130px"></asp:TextBox>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Product_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEnquiryNumber" runat="server" Text='<%# Bind("Doc_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Document Number" ToolTip="Sort By Document Number"
                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"
                                                            Width="120px"></asp:TextBox>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEnquiryDate" runat="server" Text='<%# Bind("Doc_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort5" runat="server" Text="Document Date" ToolTip="Sort By Document Date"
                                                            CssClass="styleGridHeader"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort5" runat="server" ImageAlign="Middle" Visible="false"
                                                            CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"
                                                            Width="100px" Visible="false"></asp:TextBox>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerCode" runat="server" Text='<%# Bind("Customer_Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort6" runat="server" Text="Customer" ToolTip="Sort By Customer"
                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort6" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch6" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"
                                                            Width="100px"></asp:TextBox>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Name" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort7" runat="server" Text="Customer Name" ToolTip="Sort By Customer Name"
                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort7" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch7" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Constitution" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConstitution" runat="server" Text='<%# Bind("Constitution") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="lnkbtnSort8" runat="server" Text="Constitution" ToolTip="Sort By Constitution"
                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                        <asp:ImageButton ID="imgbtnSort8" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch8" MaxLength="40" runat="server"
                                                            AutoPostBack="true" OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" Visible="true" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="true" OnCheckedChanged="chkselect_OnCheckedChanged"
                                                            Visible="true" />
                                                        <asp:Label ID="SerialNumber" Text='<%# Bind("Enq_Cus_App_ID")%>' Visible="false"
                                                            runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUserID" Visible="false" runat="server"></asp:Label>
                                                        <asp:Label ID="lblUserLevelID" Visible="false" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--   <tr>--%>
                                        <%--<td align="center" valign="top">--%>
                                        <uc1:PageNavigator ID="ucCustomPaging" runat="server" Visible="false"></uc1:PageNavigator>
                                        <%--</td>--%>
                                        <%-- </tr>--%>
                                        <%-- <tr >--%>
                                        <%--<td style="padding-top: 5px; padding-left: 5px;" align="Center">--%>
                                        <span runat="server" id="Span1" class="styleMandatoryLabel"></span>
                                        <%--</td>--%>
                                        <%--</tr>--%>
                                        <asp:Panel ID="pan1" runat="server" CssClass="stylePanel">
                                            <asp:GridView Width="100%" ID="gvCreateCreditParameter" runat="server" CssClass="styleGrid">
                                            </asp:GridView>
                                        </asp:Panel>
                                    </asp:Panel>
                                    <table width="100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnTab1Cancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                    Text="Cancel" UseSubmitBehavior="False" OnClick="btnClear_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="grvEnquiryPaging" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabCreditParameter" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Credit Parameter Transaction
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlLOBInfo" GroupingText="Credit Parameter Transaction" runat="server"
                                        CssClass="stylePanel">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 16%">
                                                    <asp:Label runat="server" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 30%">
                                                    <asp:TextBox ID="txtLOB" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 23%">
                                                    <asp:Label ID="lblCreditParamNumber" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCreditParamNumber" runat="server" Width="180px" ReadOnly="True"
                                                        ValidationGroup="TabCreditParameter"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCreditParamNumber" runat="server" ControlToValidate="txtCreditParamNumber"
                                                        Enabled="false" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                        ErrorMessage="" ValidationGroup="TabCreditParameter"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblBranch" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtBranch" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCreditParamDate" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCreditParamDate" Width="140px" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblProductCode" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtProductCode" Width="200px" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStatus" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colsapn="2">
                                                    <asp:DropDownList ID="ddlStatus" runat="server" Visible="False">
                                                        <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="Under Process" Value="5"></asp:ListItem>
                                                        <asp:ListItem Text="Approved" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="Offer Card" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="Rejected" Value="8"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                                        InitialValue="-1" CssClass="styleMandatoryLabel" Display="None" ErrorMessage=""
                                                        SetFocusOnError="True" ValidationGroup="TabCreditParameter"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtStatus" runat="server" Width="140px" ReadOnly="True" ValidationGroup="TabCreditParameter"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trEnqNo">
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEnquiryNo" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEnquiryNo" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trEnquiryDate">
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEnquiryDate" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEnquiryDate" runat="server" Width="140px" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <br />
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlCustomerInfo" runat="server" CssClass="stylePanel" GroupingText="Customer Information">
                                        <table width="100%">
                                            <tr>
                                                <td width="100%" valign="top">
                                                    <uc1:S3GCustomerAddress ID="S3GCustomerPermAddress" ActiveViewIndex="1" runat="server"
                                                        FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" SecondColumnWidth="31%"
                                                        FirstColumnWidth="16%" ThirdColumnWidth="23%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlCreditScoreDetails" runat="server" CssClass="stylePanel" GroupingText="Credit Score Details">
                                        <table width="98%" align="center">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gvCreditScoreDetails" runat="server" AutoGenerateColumns="False"
                                                        HorizontalAlign="Center" ShowFooter="true" Width="80%" OnRowDataBound="gvCreditScoreDetails_RowDataBound">
                                                        <Columns>
                                                            <%--<asp:BoundField DataField="Description" HeaderText="Description" />--%>
                                                            <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#  Bind("Description") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Percentage of Importance" HeaderStyle-HorizontalAlign="Right"
                                                                FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPerImp" runat="server" Text='<%#  Bind("Percentage_of_Importance") %>'
                                                                        Style="padding-right: 10px"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalPerImp" runat="server" Style="padding-right: 10px"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:BoundField DataField="Percentage_of_Importance" HeaderText="Percentage of Importance" />--%>
                                                            <%--<asp:BoundField DataField="Score" HeaderText="Required Score" />--%>
                                                            <asp:TemplateField HeaderText="Required Score" HeaderStyle-HorizontalAlign="Right"
                                                                FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqScore" runat="server" Text='<%#  Bind("Score") %>' Style="padding-right: 10px"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalReqScore" runat="server" Style="padding-right: 10px"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Actual Score" HeaderStyle-HorizontalAlign="Right"
                                                                FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtActualValues" Width="55px" runat="server" Style="text-align: right"
                                                                        onblur="return fnBlur(this);" onkeypress="//funChkDecimial(this,4,3,'Actual Score');"
                                                                        MaxLength="8" Text='<%#  Bind("ActualValues") %>' onpaste="return false;"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftxtActualScore" runat="server" ValidChars="." FilterType="Custom,Numbers"
                                                                        Enabled="true" TargetControlID="txtActualValues">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <%-- <cc1:MaskedEditExtender ID="MEEtxtActualValues" runat="server" 
                                     InputDirection="RightToLeft" Mask="9999.999" MaskType="Number" TargetControlID="txtActualValues"></cc1:MaskedEditExtender>--%>
                                                                    <%--Hidden Values--%>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalActScore" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGlobalParameterID" runat="server" Text='<%# Bind("ID") %>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CreditScoreItemID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCreditScoreItemID" runat="server" Text='<%# Bind("CreditScore_Item_ID") %>'
                                                                        Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <%--<asp:Label ID="lblApproved" runat="server" Text = "" Visible="false"></asp:Label>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="Div2" style="overflow: hidden; text-align: center">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr visible="false" style="display: none">
                                                <td align="right" visible="false">
                                                    <asp:Label ID="txtTotal" runat="server" BackColor="AliceBlue" BorderColor="AliceBlue"
                                                        Font-Size="Large" ReadOnly="true"></asp:Label>
                                                </td>
                                            </tr>
                                            </div>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tpOtherInfo" runat="server" CssClass="tabpan" HeaderText="Main Page"
                        BackColor="Red" Style="padding: 0px">
                        <HeaderTemplate>
                            Other Static Informations
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="100%">
                                                <asp:GridView runat="server" ShowFooter="true" ID="grvOtherInfo" Width="100%" OnRowDataBound="grvOtherInfo_RowDataBound"
                                                    OnRowCommand="grvOtherInfo_RowCommand" OnRowDeleting="grvOtherInfo_RowDeleting"
                                                    AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Line No" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLineNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Group Ref." HeaderStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGroupRef" runat="server" Text='<%#Eval("Group_Ref")%>' Visible="true"></asp:Label>
                                                                <asp:Label ID="lblKey" runat="server" Text='<%#Eval("Key")%>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblLink" runat="server" Text='<%#Eval("Link")%>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="90px" HorizontalAlign="Left" />
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlFGroupRef" ToolTip="Line Type" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFGroupRef_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvFddlFGroupRef" InitialValue="0" ValidationGroup="AddNumber"
                                                                    runat="server" ControlToValidate="ddlFGroupRef" Display="None" SetFocusOnError="true"
                                                                    ErrorMessage="Select the Group Reference Number"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle Width="90px" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Flag" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFlag" runat="server" Text='<%#Eval("Flag")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblFFlag" runat="server" Visible="false"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                            <FooterStyle Width="100px" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Desc")%>'></asp:Label>
                                                                <asp:Label ID="lblDescID" runat="server" Text='<%#Eval("Flag_ID")%>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFDescription" runat="server" Visible="false" MaxLength="100">
                                                                </asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvFtxtFDescription" ValidationGroup="AddNumber"
                                                                    runat="server" ControlToValidate="txtFDescription" Display="None" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Description"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Left"/>
                                                            <FooterStyle HorizontalAlign="Left"/>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Value" Visible="true" >
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtValue" runat="server" Width="220px" Text='<%#Eval("Value")%>' MaxLength="100"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFValue" runat="server" Width="220px" Visible="false"  MaxLength="100"></asp:TextBox>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"/><FooterStyle HorizontalAlign="Center"/>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRemarks" runat="server" Width="220px" Style="text-align: left"
                                                                    Text='<%#Eval("Remarks")%>' Visible="true"  MaxLength="100"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFRemarks" runat="server" Width="220px" Visible="false"  MaxLength="100"></asp:TextBox>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                             <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:LinkButton Text="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                    runat="server" ID="lnkbtnDelete" CausesValidation="false" CommandName="Delete"
                                                                    ToolTip="Delete"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAddCredit" runat="server" CommandName="AddNew" CssClass="styleGridShortButton"
                                                                    Text="Add" ToolTip="Add" ValidationGroup="AddNumber"></asp:Button>
                                                            </FooterTemplate>
                                                            <FooterStyle Width="7%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowSummary="true"
                                                    HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel"
                                                    ValidationGroup="AddNumber" />
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
            <td>
                <div id="divSave" style="overflow: hidden; text-align: center;" visible="false" runat="server">
                    <table width="100%">
                        <tr width="100%">
                            <td align="center">
                                <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('TabCreditParameter');"
                                    OnClick="btnSave_Click" ValidationGroup="TabCreditParameter" />
                                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                    Text="Clear" OnClientClick="return fnClear();" />
                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                    OnClick="btnClear_Click" />
                                <asp:Button ID="Button1" runat="server" CssClass="styleSubmitButton" OnClick="Button1_Click"
                                    Style="display: none;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="vsCreditParameterTrans" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):" Width="879px" ValidationGroup="TabCreditParameter"
                    EnableTheming="True" />
                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
            </td>
        </tr>
    </table>
    <input type="hidden" id="hdnSortDirection" runat="server" />
    <input type="hidden" id="hdnSortExpression" runat="server" />
    <input type="hidden" id="hdnSearch" runat="server" />
    <input type="hidden" id="hdnOrderBy" runat="server" />
    <input type="hidden" id="hdnLOBId" runat="server" />
    <%--  </ContentTemplate>
    </asp:UpdatePanel>--%>

    <script language="javascript" type="text/javascript">

        function fnClear() {
            document.getElementById("<%= lblErrorMessage.ClientID %>").innerText = "";
            if (confirm('Do you want to clear?')) {
                var intGridLen = document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails").rows.length;
                var sumValue = 0;
                for (var i = 2; i <= intGridLen; i++) {
                    if (i < 10)
                        i = "0" + i;

                    var txtActualScore = document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails_ctl" + i + "_txtActualValues");
                    if (txtActualScore != null && txtActualScore.value != "") {
                        if (txtActualScore.disabled == false && txtActualScore.readOnly == false)
                            txtActualScore.value = "";
                        else
                            sumValue = parseFloat(sumValue) + parseFloat(txtActualScore.value);
                    }
                }
                if (intGridLen < 10)
                    intGridLen = "0" + intGridLen;

                if (document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails_ctl" + intGridLen + "_lblTotalActScore") != null) {
                    document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails_ctl" + intGridLen + "_lblTotalActScore").innerText = parseFloat(sumValue).toFixed(3);
                }
            }
            return false;
        }

        function fnCalculateSum(objActScore, ObjId) {
            var arrID = ObjId.split("_");
            arrID = arrID[5].substring(3, arrID[5].length);
            var txtReqScore = document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails").rows[arrID - 1].cells[2];
            if (txtReqScore != null) {
                if (isNaN(objActScore.value)) {
                    alert("Actual score should accepts numbers & decimal only");
                    objActScore.value = "";
                    objActScore.focus();
                    return false;
                }
                if (parseFloat(objActScore.value) < 0) {
                    alert("Actual score should be 0 to Required Score");
                    objActScore.value = "";
                    objActScore.focus();
                    return false;
                }
                if (parseFloat(txtReqScore.innerText) < parseFloat(objActScore.value)) {
                    alert("Actual score should be less than or equal to Required score");
                    objActScore.value = "";
                    objActScore.focus();
                    return false;
                }
            }

            var intGridLen = document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails").rows.length;
            var sumValue = 0;
            for (var i = 2; i <= intGridLen; i++) {
                if (i < 10)
                    i = "0" + i;

                var txtActualScore = document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails_ctl" + i + "_txtActualValues");
                if (txtActualScore != null && txtActualScore.value != "" && (!isNaN(txtActualScore.value)))
                    sumValue = parseFloat(sumValue) + parseFloat(txtActualScore.value);
            }

            if (intGridLen < 10)
                intGridLen = "0" + intGridLen;

            if (document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails_ctl" + intGridLen + "_lblTotalActScore") != null) {
                if (parseFloat(sumValue) != 0)
                    document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails_ctl" + intGridLen + "_lblTotalActScore").innerText = parseFloat(sumValue).toFixed(3);
                else
                    document.getElementById("ctl00_ContentPlaceHolder1_TabContainerCPT_TabCreditParameter_gvCreditScoreDetails_ctl" + intGridLen + "_lblTotalActScore").innerText = "";
            }
        }

        function fnBlur(obj) {

            //if (!funChkDecimial(obj, "<%= intPrefix %>", "<%= intSuffix %>", 'Actual Score')) {
            if (!funChkDecimial(obj, 4, 3, 'Actual Score')) {
                return false;
            }
            if (obj.value != "") {
                var actValue = obj.value.split('.');
                if (actValue.length == 2)
                    actValue = actValue[0] + "." + actValue[1].substring(0, 3);

                obj.value = actValue; //parseFloat(obj.value).toFixed(3);    
            }

            if (!fnCalculateSum(obj, obj.id)) {
                return false;
            }
            return true;
        }
    </script>

    <%--</td>--%>
</asp:Content>
