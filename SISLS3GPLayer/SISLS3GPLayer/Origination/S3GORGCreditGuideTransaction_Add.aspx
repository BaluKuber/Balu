<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GORGCreditGuideTransaction_Add.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="Origination_S3GORGCreditGuideTransaction_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function fnOk(hdnalert) {
            document.getElementById(hdnalert).value = "1";
        }
        function fnCancel(hdnalert) {
            document.getElementById(hdnalert).value = "0";
        }

        function fnChkFreeze(ctrl) {
            document.getElementById(ctrl).checked = !document.getElementById(ctrl).checked;
        }

        function fnAllowRatioOnly(isdecimalAllowed, isNegativeAllowed, txtbox) {

            var sKeyCode = window.event.keyCode;
            if ((sKeyCode < 48 || sKeyCode > 57) && (sKeyCode != 32 && sKeyCode != 95) || (sKeyCode == 58)) {

                if (isdecimalAllowed) {
                    //alert(sKeyCode);
                    if (sKeyCode == 58) {

                        if (txtbox.value.indexOf(':') > -1) {
                            window.event.keyCode = 0;
                            return false;
                        }
                        else {
                            if (txtbox.value.length == 0 && sKeyCode == 58) {
                                window.event.keyCode = 0;
                                return false;
                            }
                            else
                                return true;
                        }
                    }
                }
                if (isNegativeAllowed) {
                    if (sKeyCode == 45) {

                        if (txtbox.value.indexOf('-') > -1) {
                            window.event.keyCode = 0;
                            return false;
                        }
                        else {
                            return true;
                        }
                    }
                }
                window.event.keyCode = 0;
                return false;
            }
            if (txtbox.value.length > txtbox.maxlength) {
                window.event.keyCode = 0;
                return false;
            }
        }

        function funpastyearstart() {
            var da = new Date();
            //alert(da.getFullYear()+'-'+da.getMonth());
            var currentmonth = da.getMonth();
            var currentyear = da.getFullYear();
            //  alert(currentmonth+'-'+currentyear);
            if (currentmonth < 4) {
                currentyear = currentyear - 1;
            }
            if (document.getElementById('<%=txtPastYears.ClientID %>').value > 0) {
                if (document.getElementById('<%=txtPastYearStartFrom.ClientID %>').value > 0) {
                    var temp = currentyear - document.getElementById('<%=txtPastYears.ClientID %>').value;
                    document.getElementById('<%=txtPastYearStartFrom.ClientID %>').value = temp;
                }
                else {
                    document.getElementById('<%=txtPastYearStartFrom.ClientID %>').value = currentyear;
                }
            }
            else {
                document.getElementById('<%=txtPastYearStartFrom.ClientID %>').value = currentyear;
            }
            // alert(document.getElementById('<%=txtPastYears.ClientID %>').value+'--'+ document.getElementById('<%=txtPastYearStartFrom.ClientID %>').value);
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
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
                    <td>
                        <cc1:TabContainer ID="TabContainerCGT" runat="server" ActiveTabIndex="1" CssClass="styleTabPanel"
                            Width="99%" ScrollBars="Auto">
                            <cc1:TabPanel runat="server" ID="TabCreditGuideQuery" CssClass="tabpan" Width="98%"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Transaction Type
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <%--<asp:Panel ID="pnlTransactionType" runat="server" GroupingText="Transaction Type"
                                                CssClass="stylePanel" Width="98%">--%>
                                            <table cellpadding="0" width="98%" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblDocumentType" runat="server" CssClass="styleDisplayLabel" Text="Document Type"> </asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <%--<asp:RadioButtonList ID="rdnlCreditType" Visible="false" runat="server" RepeatDirection="Horizontal"
                                                                OnSelectedIndexChanged="rdnlCreditType_SelectedIndexChanged" AutoPostBack="True">
                                                                <asp:ListItem Text="Enquiry" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>--%>
                                                        <asp:DropDownList ID="ddlDocumentType" runat="server" ToolTip="Document Type" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--  </asp:Panel>--%>
                                            <asp:Panel ID="pnlCreateCreditGuide" runat="server" CssClass="stylePanel">
                                                <table cellpadding="0" width="98%" cellspacing="0">
                                                    <%-- <tr>
                                                        <td class="stylePageHeading">
                                                            <table align="center" width="98%">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="Label1" CssClass="styleInfoLabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                OnRowDataBound="grvPaging_RowDataBound" Visible="false">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Line Of Business">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Line of Business" ToolTip="Sort By LOB_Name"
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
                                                                            <asp:Label ID="lblLobName" runat="server" Text='<%# Bind("LineofBusiness") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Constitution">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort6" runat="server" Text="Constitution" ToolTip="Sort By LOB_Name"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort6" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch6" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblConstitutionName" runat="server" Text='<%# Bind("Constitution") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Product">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort5" runat="server" Text="Product" ToolTip="Sort By LOB_Name"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort5" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProdName" runat="server" Text='<%# Bind("Product") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Document No">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Document No" ToolTip="Sort By Document_No"
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
                                                                            <asp:Label ID="lblEnquiryno" runat="server" Text='<%# Bind("Doc_Number") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Customer">
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
                                                                    <asp:TemplateField HeaderText="Customer Name">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Customer Name" ToolTip="Sort By Customer_Name"
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
                                                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("Customer_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Select">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkselect" runat="server" AutoPostBack="true" OnCheckedChanged="chkselect_OnCheckedChanged" />
                                                                            <asp:Label ID="lblEnqCusAppID" Text='<%# Bind("Enq_Cus_App_ID")%>' Visible="false"
                                                                                runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td>
                                                            <uc1:PageNavigator ID="ucCustomPaging" runat="server" Visible="false" />
                                                        </td>
                                                        <tr>
                                                            <td style="padding-top: 5px;" align="center">
                                                                <asp:Button ID="btnCancelView" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                    OnClick="btnCancel_Click" Text="Cancel" />
                                                                <asp:Button ID="btnBranchShowAll" runat="server" Text="Show All" CssClass="styleSubmitButton"
                                                                    OnClick="btnBranchShowAll_Click" Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <span id="lblErrorMessage" runat="server" style="color: Red; font-size: medium">
                                                            </span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabCreditGuideTransaction" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Credit Guide Transaction
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="Updatepanel5" runat="server">
                                        <ContentTemplate>
                                            <table width="100%">
                                                <tr>
                                                    <td width="50%">
                                                        <asp:Panel ID="pnlCustomerInformation" GroupingText="Customer Information" runat="server"
                                                            CssClass="stylePanel">
                                                            <table width="100%" align="center">
                                                                <tr>
                                                                    <td width="100%">
                                                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                    <td width="50%" valign="top">
                                                        <asp:Panel ID="pnlEnquiryDetails" GroupingText="Enquiry Details" runat="server" CssClass="stylePanel">
                                                            <table width="98%" align="center">
                                                                <tr>
                                                                    <td class="styleFieldLabel" width="35%">
                                                                        <asp:Label runat="server" Text="Line of Business" ID="Label2" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" width="65%">
                                                                        <asp:TextBox ID="txtlob" runat="server" ReadOnly="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Product" ID="Lblproduct" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtproduct" runat="server" ReadOnly="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="Label3" runat="server" CssClass="styleDisplayLabel" Text="Constitution"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtConstitution" runat="server" ReadOnly="True" Style="margin-left: 2px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label runat="server" Text="Enquiry Number" ID="lblEnqNo" CssClass="styleDisplayLabel"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtEnquiryNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <table>
                                                            <tr>
                                                                <td align="center">
                                                                    <br />
                                                                    <br />
                                                                    <br />
                                                                    <br />
                                                                    <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitShortButton" CausesValidation="true"
                                                                        Text="Go" OnClick="btnGo_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td width="50%">
                                                    </td>
                                                    <td width="50%">
                                                        <asp:Panel ID="pnlCreditGuideDetails" GroupingText="Credit Guide Details" runat="server"
                                                            CssClass="stylePanel" Visible="false">
                                                            <table width="100%" align="center">
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblPastYears" runat="server" CssClass="styleDisplayLabel" Text="Past Years"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPastYears" runat="server" MaxLength="1" onfocusOut="funpastyearstart();"
                                                                            AutoPostBack="false" Style="margin-left: 0px; text-align: right;" Width="30PX"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderN" runat="server" TargetControlID="txtPastYears"
                                                                            FilterType="Numbers,Custom" InvalidChars="0" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RangeValidator ID="RangeValidator2" CssClass="styleMandatoryLabel" ControlToValidate="txtPastYears"
                                                                            MinimumValue="0" MaximumValue="3" Type="Double" runat="server" Display="None"
                                                                            ErrorMessage="Past Year(s) should not be greater than 3 years"></asp:RangeValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblPastYearStartFrom0" runat="server" CssClass="styleDisplayLabel"
                                                                            Text="Past Year Starting From"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtPastYearStartFrom" runat="server" Enabled="false" Style="text-align: right;"
                                                                            Width="50PX"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPastYearStartFrom"
                                                                            FilterType="Numbers" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblFutureYears" runat="server" CssClass="styleDisplayLabel" Text="Future Years"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtFutureYears" runat="server" MaxLength="1" Style="text-align: right;"
                                                                            Width="30PX">
                                                                        </asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtFutureYears"
                                                                            FilterType="Numbers" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RangeValidator ID="RangeValidator1" CssClass="styleMandatoryLabel" ControlToValidate="txtFutureYears"
                                                                            MinimumValue="0" MaximumValue="3" Type="Double" runat="server" Display="None"
                                                                            ErrorMessage="Future Year(s) should not be greater than 3 years"></asp:RangeValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldLabel">
                                                                        <asp:Label ID="lblYear" runat="server" Text="Year Value" Visible="false">
                                                                        </asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Year_SelectedIndexChanged"
                                                                            Visible="false">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 82px;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td width="100%" class="styleContentTable">
                                                        <table style="float: right;" width="100%" cellpadding="0" cellspacing="0">
                                                            <tr class="stylePagingControl">
                                                                <td>
                                                                    <asp:RadioButtonList runat="server" ID="rbtYears" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="rbtYears_SelectedIndexChanged">
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbltotalscore" runat="server" CssClass="styleDisplayLabel" Text="Total Score"> </asp:Label>
                                                                    <asp:TextBox ID="txtTotalScore" runat="server" ReadOnly="true" Style="text-align: right;"
                                                                        Width="80px"></asp:TextBox>
                                                                    <asp:DropDownList runat="server" Width="100px" AutoPostBack="true" ID="ddlFinanceYear"
                                                                        ToolTip="Year" Visible="false">
                                                                    </asp:DropDownList>
                                                                    <asp:Button ID="btnAddYear" CssClass="styleSubmitButton" runat="server" Text="Add Year"
                                                                        ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px;
                                                                        width: 78px; height: 22px;" Enabled="false" Visible="true" OnClick="btnAddYear_Click">
                                                                    </asp:Button>
                                                                    <asp:Button ID="btnUpdateYear" CssClass="styleSubmitButton" runat="server" Text="Update Year"
                                                                        ToolTip="Add" Style="background-image: url(../Images/btn_Silver.jpg); border-width: 0px;
                                                                        width: 78px; height: 22px;" Enabled="true" Visible="false" OnClick="btnUpdateYear_Click">
                                                                    </asp:Button>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                        <br />
                                                        <cc1:TabContainer ID="tcCreditScore" runat="server" CssClass="styleTabPanel" Width="100%"
                                                            ScrollBars="None" TabStripPlacement="top" ActiveTabIndex="1">
                                                            <cc1:TabPanel ID="tpMainPage" runat="server" CssClass="tabpan" HeaderText="Main Page"
                                                                BackColor="Red" Style="padding: 0px">
                                                                <HeaderTemplate>
                                                                    Main Page
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="upMainPage" runat="server">
                                                                        <ContentTemplate>
                                                                            <center>
                                                                                <asp:GridView ID="grvCreditScore" runat="server" AutoGenerateColumns="False" FooterStyle-HorizontalAlign="Center"
                                                                                    RowStyle-HorizontalAlign="Center" Width="80%" OnRowDataBound="grvCreditScore_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true"
                                                                                            ItemStyle-Width="180px">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Desc")%>'></asp:Label>
                                                                                                <asp:Label ID="lblCrScoreParam_ID" runat="server" Text='<%#Eval("CrScoreParam_ID")%>'
                                                                                                    Visible="false"></asp:Label>
                                                                                                <asp:Label ID="lblYearvaluesID" runat="server" Text='<%#Eval("CRDTGUIDETRANSYEARVAL_ID")%>'
                                                                                                    Visible="false"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle Width="23%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Actual Value" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtActualValue1" Style="text-align: right;" runat="server" AutoPostBack="true"
                                                                                                    Text='<%# Bind("Actual_Value")%>' Width="140px" MaxLength="10" OnTextChanged="FunCalculation"></asp:TextBox>
                                                                                                <asp:DropDownList ID="ddlYes" runat="server" AutoPostBack="true" Width="140px" OnSelectedIndexChanged="ddlYes_SelectedIndexChanged">
                                                                                                    <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="False" Format="dd/MM/yyyy"
                                                                                                    PopupButtonID="imgDate" TargetControlID="txtActualValue1">
                                                                                                </cc1:CalendarExtender>
                                                                                                <cc1:FilteredTextBoxExtender ID="fteAmount1" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                                                                    TargetControlID="txtActualValue1" ValidChars=".">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <cc1:FilteredTextBoxExtender ID="fteAmount2" runat="server" Enabled="True" FilterType="Numbers,Custom"
                                                                                                    TargetControlID="txtActualValue1" ValidChars=":,.">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <asp:RangeValidator ID="RangePercentage" CssClass="styleMandatoryLabel" ControlToValidate="txtActualValue1"
                                                                                                    MinimumValue="0" MaximumValue="100" Type="Double" runat="server" Display="None"
                                                                                                    ErrorMessage="Percentage should not be greater than 100 %"></asp:RangeValidator>
                                                                                                <asp:RequiredFieldValidator ID="RequiredvalidatorTextbox" runat="server" ControlToValidate="txtActualValue1"
                                                                                                    Display="None" ErrorMessage="Enter Actual value"></asp:RequiredFieldValidator>
                                                                                                <asp:RequiredFieldValidator ID="RequiredvalidatorCombo" InitialValue="-1" runat="server"
                                                                                                    ControlToValidate="ddlYes" Display="None" ErrorMessage="Select Actual value"></asp:RequiredFieldValidator>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Actual Score" ItemStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtActualScore1" Style="text-align: right;" runat="server" ReadOnly="true"
                                                                                                    Text='<%# Bind("Actual_Score")%>' Width="140px"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Field Attribute">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlFieldAtt" runat="server" AutoPostBack="true" Enabled="false"
                                                                                                    Width="100px" OnSelectedIndexChanged="ddlFieldAtt_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle Width="10%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Required Value" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtReqValue1" Enabled="false" runat="server" MaxLength="30" Text='<%# Bind("ReqValue")%>'
                                                                                                    Width="80px"></asp:TextBox>
                                                                                                <asp:DropDownList ID="ddlYes1" runat="server" Enabled="false" Width="80px">
                                                                                                    <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Numeric Operator">
                                                                                            <ItemTemplate>
                                                                                                <asp:DropDownList ID="ddlNumeric" runat="server" Enabled="false" Width="80px">
                                                                                                </asp:DropDownList>
                                                                                            </ItemTemplate>
                                                                                            <HeaderStyle Width="10%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Score" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtScore1" runat="server" Enabled="false" MaxLength="15" Text='<%# Bind("Score")%>'
                                                                                                    Width="80px"></asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                                    FilterType="Numbers,Custom" TargetControlID="txtScore1" ValidChars=".">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Difference %">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtDiffPer" runat="server" Enabled="false" MaxLength="3" Text='<%# Bind("DiffPer")%>'
                                                                                                    Width="60px"></asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender31" runat="server" Enabled="True"
                                                                                                    FilterType="Numbers,Custom" TargetControlID="txtDiffPer" ValidChars=".">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" HeaderText="Difference Mark">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtDiffMark" runat="server" Enabled="false" MaxLength="6" Text='<%# Bind("DiffMark")%>'
                                                                                                    Width="80px"></asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender41" runat="server" Enabled="True"
                                                                                                    FilterType="Numbers,Custom" TargetControlID="txtDiffMark" ValidChars=".">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                                    <RowStyle HorizontalAlign="Center" />
                                                                                </asp:GridView>
                                                                            </center>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </ContentTemplate>
                                                            </cc1:TabPanel>
                                                            <cc1:TabPanel ID="tpNumber" runat="server" CssClass="tabpan" HeaderText="Main Page"
                                                                BackColor="Red" Style="padding: 0px">
                                                                <HeaderTemplate>
                                                                    Number
                                                                </HeaderTemplate>
                                                                <ContentTemplate>
                                                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                        <ContentTemplate>
                                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td width="100%">
                                                                                        <asp:GridView runat="server" ID="grvNumbers" Width="100%" OnRowDataBound="grvNumbers_RowDataBound"
                                                                                            RowStyle-HorizontalAlign="Center" ShowFooter="false" FooterStyle-HorizontalAlign="Center"
                                                                                            AutoGenerateColumns="False">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Line No">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLineNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Line Type">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblLineType" runat="server" Text='<%#Eval("LineType")%>'></asp:Label>
                                                                                                        <asp:Label ID="lblLineTypeID" runat="server" Text='<%#Eval("LineTypeID")%>' Visible="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                    <FooterTemplate>
                                                                                                        <asp:DropDownList ID="ddlFLineType" ToolTip="Line Type" runat="server" AutoPostBack="true">
                                                                                                        </asp:DropDownList>
                                                                                                    </FooterTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Flag">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblFlag" runat="server" Text='<%#Eval("Flag")%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                    <FooterTemplate>
                                                                                                        <asp:DropDownList ID="ddlFFlag" ToolTip="Line Type" runat="server" AutoPostBack="true">
                                                                                                        </asp:DropDownList>
                                                                                                    </FooterTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Description">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Desc")%>'></asp:Label>
                                                                                                        <asp:Label ID="lblDescID" runat="server" Text='<%#Eval("Flag_ID")%>' Visible="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                    <FooterTemplate>
                                                                                                        <asp:TextBox ID="txtFDesc" AutoPostBack="false" runat="server" ToolTip="Description"></asp:TextBox>
                                                                                                    </FooterTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Value">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblValue" runat="server" Text='<%#Eval("Value")%>' Visible="false"
                                                                                                            Style="text-align: right" Width="120px"></asp:Label>
                                                                                                        <asp:TextBox ID="txtValue" AutoPostBack="true" runat="server" OnTextChanged="txtValue_TextChanged"
                                                                                                            Width="120px" Style="text-align: right" Text='<%#Eval("Value")%>'></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Right" Width="90px" />
                                                                                                    <FooterTemplate>
                                                                                                        <asp:TextBox ID="txtFValue" MaxLength="15" Width="90px" runat="server" ToolTip="Value"
                                                                                                            Style="text-align: right"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="fetxtFValue" runat="server" TargetControlID="txtFValue"
                                                                                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                        <asp:RequiredFieldValidator ID="rfvFtxtFValue" ValidationGroup="AddNumber" runat="server"
                                                                                                            ControlToValidate="txtFValue" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Value"></asp:RequiredFieldValidator>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle Width="90px" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Formula" ItemStyle-Width="250px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblFormula" runat="server" Text='<%#Eval("Formula")%>'></asp:Label>
                                                                                                        <asp:CheckBox ID="chkApplyOptional" runat="server" AutoPostBack="true" Visible="false" OnCheckedChanged="chkApplyOptional_CheckedChanged" />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Panel runat="server" ID="pnlFFml" Enabled="false">
                                                                                                            <table>
                                                                                                                <tr>
                                                                                                                    <td rowspan="2">
                                                                                                                        <asp:TextBox ID="txtFFormula" TextMode="MultiLine" Width="100px" runat="server" ToolTip="Value"
                                                                                                                            Style="text-align: left; height: 40px; width: 140px;"></asp:TextBox>
                                                                                                                        <cc1:FilteredTextBoxExtender ID="fetxtFFormula" runat="server" TargetControlID="txtFFormula"
                                                                                                                            FilterType="Numbers,Custom,UppercaseLetters" ValidChars=".,+,-,*,/,%,(,),<,>"
                                                                                                                            Enabled="True">
                                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <asp:DropDownList ID="ddlFFmlLines" runat="server">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <button id="btnAddfml" type="button" runat="server" class="styleGridShortButton"
                                                                                                                            value="Add" title="Add">
                                                                                                                            <img src="../Images/Prev_Desabled.gif" style="position: absolute" runat="server"
                                                                                                                                id="imgPrev1" />
                                                                                                                            &nbsp;
                                                                                                                            <img src="../Images/Prev_Desabled.gif" runat="server" id="imgPrev2" />
                                                                                                                        </button>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                        </asp:Panel>
                                                                                                    </FooterTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Link">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="chkLink" runat="server" />
                                                                                                        <asp:Label ID="lblLink" runat="server" Text='<%#Eval("IsLink")%>' Visible="false"
                                                                                                            Enabled="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <asp:CheckBox ID="chkFLink" runat="server" />
                                                                                                    </FooterTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Param Num">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblParamText" runat="server" Text='<%#Eval("ParamText")%>'></asp:Label>
                                                                                                        <asp:Label ID="lblParamNum" runat="server" Text='<%#Eval("ParamNum")%>' Visible="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                                    <FooterTemplate>
                                                                                                        <asp:DropDownList ID="ddlFParamNum" ToolTip="Param Num" runat="server" Width="150px">
                                                                                                        </asp:DropDownList>
                                                                                                    </FooterTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Action">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton Text="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                                                            runat="server" ID="lnkbtnDelete" CommandName="Delete" ToolTip="Delete"></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                    <FooterTemplate>
                                                                                                        <asp:Button ID="btnAddCredit" runat="server" CssClass="styleGridShortButton" Text="Add"
                                                                                                            ToolTip="Add" ValidationGroup="AddNumber"></asp:Button>
                                                                                                    </FooterTemplate>
                                                                                                    <FooterStyle Width="7%" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                                        </asp:GridView>
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
                                                    <td style="text-align: right; padding-right: 180px">
                                                        <asp:Label runat="server" Text="Actual Total Score" BackColor="White" ID="lblActualtotalscore"
                                                            Font-Bold="true" CssClass="styleGridHeader">
                                                        </asp:Label>
                                                        <asp:TextBox ID="txtActualTotal" Width="80px" ReadOnly="true" runat="server" ToolTip="Total Score"
                                                            Style="text-align: right"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                            OnClick="btnAdd_Click" Visible="false" />
                                                    </td>
                                                </tr>
                                                <caption>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                                                                OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators();" />
                                                            <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                Text="Cancel" OnClick="btnCancel_Click" />
                                                            <input id="hdnCreditScoreID" runat="server" type="hidden" value="0" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="lblsErrorMessage" runat="server" CssClass="styleMandatoryLabel"
                                                                HeaderText="Please correct the following validation(s):" ShowSummary="true" />
                                                        </td>
                                                        <td>
                                                            <input id="hdnalert" runat="server" type="hidden" value="0" />
                                                            <input id="hdnCreditScoreTran_ID" runat="server" type="hidden" value="0" />
                                                            <input type="hidden" id="hdnSortDirection" runat="server" />
                                                            <input type="hidden" id="hdnSortExpression" runat="server" />
                                                            <input type="hidden" id="hdnSearch" runat="server" />
                                                            <input type="hidden" id="hdnEdit_Status" runat="server" value="0" />
                                                            <input type="hidden" id="hdnOrderBy" runat="server" />
                                                            <asp:HiddenField ID="hdnLobid" runat="server" />
                                                            <asp:HiddenField ID="hdnproductid" runat="server" />
                                                        </td>
                                                    </tr>
                                                </caption>
                                            </table>
                                            <table width="100%">
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="grvCreditScoreYearValues" runat="server" ShowFooter="true" AutoGenerateColumns="true"
                                                            OnRowDataBound="grvCreditScoreYearValues_RowDataBound" Visible="false">
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="TabAdditional" CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Additional Information
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="Updatepanel3" runat="server">
                                        <ContentTemplate>
                                            <table width="99%" align="center">
                                                <tr>
                                                    <td align="left" class="accordionHeaderSelected" style="width: 100%;">
                                                        <asp:Label ID="lblAddtMsg" runat="server" CssClass="styleHeaderInfo" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="grvAdditional" runat="server" OnRowDataBound="grvAdditional_RowDataBound"
                                                            AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Center"
                                                            Width="70%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddtDesc" runat="server" Text='<%# Bind("Desc")%>'></asp:Label>
                                                                        <asp:Label ID="lblAddtCrScoreParam_ID" runat="server" Text='<%#Eval("CrScoreParam_ID")%>'
                                                                            Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblAddtYearvaluesID" runat="server" Text='<%#Eval("CRDTGUIDETRANSYEARVAL_ID")%>'
                                                                            Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="23%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Actual Value1" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAddtActualValue1" Style="text-align: right;" runat="server" Text='<%# Bind("Actual_Value1")%>'
                                                                            Width="140px" MaxLength="100"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Actual Value2" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAddtActualValue2" Style="text-align: right;" runat="server" Text='<%# Bind("Actual_Value2")%>'
                                                                            Width="140px" MaxLength="100"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Actual Value3" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAddtActualValue3" Style="text-align: right;" runat="server" Text='<%# Bind("Actual_Value3")%>'
                                                                            Width="140px" MaxLength="100"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
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
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
