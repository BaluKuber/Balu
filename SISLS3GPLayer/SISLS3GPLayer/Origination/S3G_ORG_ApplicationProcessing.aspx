<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3G_ORG_ApplicationProcessing.aspx.cs" Inherits="Origination_S3G_ORG_ApplicationProcessing"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        //Added By Shibu Resize Grid
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=DivFollow.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=DivFollow.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=DivFollow.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=DivFollow.ClientID %>').style.overflow = "scroll";
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Application Processing">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr width="100%">
                    <td valign="top" width="100%">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td valign="top" width="100%">
                                    <cc1:TabContainer ID="TabContainerAP" runat="server" CssClass="styleTabPanel" Width="100%"
                                        ScrollBars="None" ActiveTabIndex="0" onchange="fnSetDivWidth();">
                                        <cc1:TabPanel runat="server" ID="TabMainPage" CssClass="tabpan" BackColor="Red" Width="100%">
                                            <HeaderTemplate>
                                                Main Page
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="upMainPage" runat="server"><ContentTemplate>
                                                        <input id="hdnCustID" type="hidden" runat="server" />
                                                        <input id="hdnConstitutionId" runat="server" type="hidden" />
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="50%">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlApplicationDetails" runat="server" GroupingText="Application Details"
                                                                                    CssClass="stylePanel" Width="99%">
                                                                                    <table width="100%" border="0">
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label runat="server" ID="lblApplicationNo" CssClass="styleReqFieldLabel" Text="Application No."></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign" colspan="3">
                                                                                                <asp:TextBox ID="txtApplicationNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label runat="server" ID="lblDate" CssClass="styleReqFieldLabel" Text="Date"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign">
                                                                                                <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" Width="95%"></asp:TextBox>
                                                                                                <asp:TextBox ID="txtOfferDate" runat="server" Style="display: none;"></asp:TextBox>
                                                                                            </td>
                                                                                            <td class="styleFieldLabel">
                                                                                                <asp:Label runat="server" ID="lblStatus" CssClass="styleReqFieldLabel" Text="Status"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True" Width="95%"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="Panel_Input" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                                                                    Width="99%">
                                                                                    <table width="100%" border="0">
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label runat="server" ID="lblLOB" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign">
                                                                                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvLineofBusiness" runat="server" ControlToValidate="ddlLOB"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                                                    ErrorMessage="Select a Line of Business" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                                                    ErrorMessage="Select a Line of Business" ValidationGroup="tcAsset"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label runat="server" ID="lblBranch" Text="Location" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign">
                                                                                                <uc2:Suggest ID="ddlBranchList" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                                    OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select a Location"
                                                                                                    ValidationGroup="Main Page" IsMandatory="true" />
                                                                                                <%--<asp:DropDownList ID="ddlBranchList" runat="server" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                                                    AutoPostBack="true">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvddlBranchList" runat="server" ControlToValidate="ddlBranchList"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                                                    ErrorMessage="Select a Location" ValidationGroup="Main Page"></asp:RequiredFieldValidator>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label runat="server" ID="lblBusinessOfferNo" CssClass="styleDisplayLabel" Text="Business Offer No."></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign">
                                                                                                <asp:DropDownList ID="ddlBusinessOfferNoList" runat="server" AutoPostBack="True"
                                                                                                    OnSelectedIndexChanged="ddlBusinessOfferNoList_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvBusinessOfferNo" runat="server" ControlToValidate="ddlBusinessOfferNoList"
                                                                                                    ErrorMessage="Select a Business Offer No." ValidationGroup="Main Page" CssClass="styleMandatoryLabel"
                                                                                                    Display="None" SetFocusOnError="True" InitialValue="-1"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label ID="lblSalePersonCode" runat="server" CssClass="styleReqFieldLabel" Text="Sales Person Code"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign">
                                                                                                <%--   <asp:DropDownList ID="ddlSalePersonCodeList" runat="server">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvSalePersonCodeList" runat="server" ControlToValidate="ddlSalePersonCodeList"
                                                                                                    ValidationGroup="Main Page" CssClass="styleMandatoryLabel" Display="None" InitialValue="-1"
                                                                                                    SetFocusOnError="True" ErrorMessage="Select a Sales Person Code"></asp:RequiredFieldValidator>--%>
                                                                                                <uc2:Suggest ID="ddlSalePersonCodeList" runat="server" Width="250px" ServiceMethod="GetSalePersonList"
                                                                                                    ErrorMessage="Select a Sales Person Code" IsMandatory="true" ValidationGroup="Main Page" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label runat="server" ID="lblProductCode" CssClass="styleReqFieldLabel" Text="Product Code"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign">
                                                                                                <asp:DropDownList ID="ddlProductCodeList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProductCodeList_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvProductCodeList" runat="server" ControlToValidate="ddlProductCodeList"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Select a Product Code"
                                                                                                    ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlProductCodeList"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                                                    ErrorMessage="Select a Product Code" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="35%">
                                                                                                <asp:Label ID="lblRoundNumber" runat="server" Visible="false" Text="Round No :"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign">
                                                                                                <asp:Label ID="lblRoundNo" runat="server" Visible="false" Style="text-align: right"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td width="50%">
                                                                    <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
                                                                        CssClass="stylePanel" Width="99%">
                                                                        <table width="100%" border="0" cellspacing="0">
                                                                            <tr>
                                                                                <td class="styleFieldLabel" width="35%">
                                                                                    <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none;" MaxLength="50"></asp:TextBox>
                                                                                    <uc2:LOV ID="ucCustomerCodeLov" onblur="return fnLoadCustomer()" runat="server" strLOV_Code="CMD" />
                                                                                    <a href="#" onclick="window.open('../Origination/S3GOrgCustomerMaster_Add.aspx?IsFromApplication=Yes&NewCustomerID=0', 'newwindow', 'toolbar=no,location=no,menubar=no,width=950,height=600,resizable=no,scrollbars=yes,top=150,left=100');return false;">
                                                                                        <asp:Button ID="btnCreateCustomer" runat="server" UseSubmitBehavior="false" Text="Create"
                                                                                            CssClass="styleSubmitShortButton" CausesValidation="False" /></a>
                                                                                    <asp:RequiredFieldValidator ID="rfvcmbCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                                        ErrorMessage="Select a Customer Code" ValidationGroup="Main Page" CssClass="styleMandatoryLabel"
                                                                                        Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                    <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                                                                        ErrorMessage="Select a Customer Code" ValidationGroup="tcAsset" CssClass="styleMandatoryLabel"
                                                                                        Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" width="100%">
                                                                                    <asp:Button ID="btnLoadCustomer" runat="server" OnClick="btnLoadCustomer_OnClick"
                                                                                        Style="display: none" Text="Load Customer" />
                                                                                    <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                        SecondColumnStyle="styleFieldAlign" ShowCustomerCode="false" />
                                                                                </td>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div id="divSpace" runat="server" style="height: 6px;">
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="70%">
                                                                    <asp:Panel ID="Panel_Finance" runat="server" GroupingText="Finance details" CssClass="stylePanel"
                                                                        Width="99%">
                                                                        <table width="100%" border="0">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblFinanceAmount" CssClass="styleReqFieldLabel" Text="Finance Amount"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtFinanceAmount" runat="server" MaxLength="10" Style="text-align: right"
                                                                                        onchange="FunRestIrr();" Width="75px"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftxtFinanceAmount" runat="server" TargetControlID="txtFinanceAmount"
                                                                                        FilterType="Numbers" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvFinanceAmount" runat="server" ControlToValidate="txtFinanceAmount"
                                                                                        ValidationGroup="Main Page" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                        ErrorMessage="Enter the Finance Amount"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblTenure" CssClass="styleReqFieldLabel" Text="Tenure"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtTenure" runat="server" MaxLength="3" Style="text-align: right"
                                                                                        onchange="FunRestIrr();" Width="25px"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftxtTenure" runat="server" TargetControlID="txtTenure"
                                                                                        FilterType="Numbers" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvTenure" runat="server" ControlToValidate="txtTenure"
                                                                                        ValidationGroup="Main Page" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                        ErrorMessage="Enter the Tenure"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblTenureType" CssClass="styleReqFieldLabel" Text="Tenure Type"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlTenureType" runat="server" onchange="FunRestIrr();">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvTenureType" runat="server" ControlToValidate="ddlTenureType"
                                                                                        ValidationGroup="Main Page" CssClass="styleMandatoryLabel" Display="None" InitialValue="-1"
                                                                                        SetFocusOnError="True" ErrorMessage="Select a Tenure Type"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblRefinanceContract" runat="server" CssClass="styleDisplayLabel"
                                                                                        Text="Refinance Contract"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:CheckBox ID="ChkRefinanceContract" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblMarginAmount" CssClass="styleDisplayLabel" Text="Margin Amount"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtMarginAmount" runat="server" MaxLength="7" Style="text-align: right;"
                                                                                        Width="75px"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftxtMarginAmount" ValidChars="." runat="server"
                                                                                        TargetControlID="txtMarginAmount" FilterType="Custom,Numbers" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblResidualValue" CssClass="styleDisplayLabel" Text="Residual Value"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtResidualValue" runat="server" MaxLength="7" Style="text-align: right"
                                                                                        Width="75px"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftxtResidualAmount" ValidChars="." runat="server"
                                                                                        TargetControlID="txtResidualValue" FilterType="Custom,Numbers" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <%--Code added by Chandru k on 18/09/2013 for ISFC customization--%>
                                                                            <%--<tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblTypeOfMortgage" CssClass="styleDisplayLabel" Text="Type of Mortgage"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlTypeOfMortgage" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="Label2" CssClass="styleDisplayLabel" Text="Mortgage Fee"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtMortgageFee" runat="server" MaxLength="7" Style="text-align: right"
                                                                                        Width="75px"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftxttxtMortgageFee" ValidChars="." runat="server"
                                                                                        TargetControlID="txtMortgageFee" FilterType="Custom,Numbers" Enabled="True">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblStepDown" CssClass="styleDisplayLabel" Text="Step Down Revision Type"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlStepDown" runat="server">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                </td>
                                                                            </tr>--%>
                                                                            <%--End--%>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:Panel ID="pnlIRRDetails" runat="server" GroupingText="IRR details" CssClass="stylePanel"
                                                                        Width="98%">
                                                                        <table width="100%" border="0">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblAccountingIRR" CssClass="styleReqFieldLabel" Text="Accounting IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtAccountingIRR" runat="server" TabIndex="-1" Style="text-align: right"
                                                                                        Width="75px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblBusinessIRR" CssClass="styleReqFieldLabel" Text="Business IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtBusinessIRR" runat="server" TabIndex="-1" Style="text-align: right"
                                                                                        Width="75px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblCompanyIRR" CssClass="styleReqFieldLabel" Text="Company IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtCompanyIRR" runat="server" TabIndex="-1" Style="text-align: right"
                                                                                        Width="75px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table width="100%">
                                                            <tr>
                                                                <td width="100%">
                                                                    <cc1:TabContainer ID="TabContainerMainTab" runat="server" CssClass="styleTabPanel"
                                                                        Width="100%" ActiveTabIndex="1">
                                                                        <cc1:TabPanel ID="TabDocumentDetails" runat="server" BackColor="Red" CssClass="tabpan"
                                                                            Width="100%">
                                                                            <HeaderTemplate>
                                                                                Document Details
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td class="styleFieldLabel" width="25%">
                                                                                            <asp:Label ID="lblConstitutionCodeList" runat="server" CssClass="styleReqFieldLabel"
                                                                                                Text="Constitution Code"></asp:Label>
                                                                                        </td>
                                                                                        <td class="styleFieldAlign" align="left" width="75%">
                                                                                            <asp:TextBox ID="txtConstitution" runat="server" TabIndex="-1" ReadOnly="true">
                                                                                            </asp:TextBox>
                                                                                            <asp:DropDownList ID="ddlConstitutionCodeList" runat="server" Visible="false">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <asp:Panel runat="server" ID="Panel6" CssClass="stylePanel" GroupingText="Constitution Document Details"
                                                                                    Width="99%">
                                                                                    <%--<div id="div14" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">--%>
                                                                                    <div style="overflow-x: hidden; overflow-y: auto; width: 100%">
                                                                                        <div>
                                                                                            <br />
                                                                                            <asp:GridView runat="server" ID="grvConsDocuments" Width="100%" OnRowDataBound="grvConsDocs_RowDataBound"
                                                                                                AutoGenerateColumns="False">
                                                                                                <Columns>
                                                                                                    <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                                                    <asp:BoundField DataField="DocumentFlag" HeaderText="Document Flag" ItemStyle-Width="10%" />
                                                                                                    <asp:BoundField DataField="DocumentDescription" HeaderText="Document Description"
                                                                                                        ItemStyle-Width="15%" />
                                                                                                    <asp:TemplateField HeaderText="Is_Mandatory" ItemStyle-Width="10%">
                                                                                                        <HeaderTemplate>
                                                                                                            <asp:Label ID="lblOptman" runat="server" Text="Mandatory"></asp:Label>
                                                                                                        </HeaderTemplate>
                                                                                                        <ItemTemplate>
                                                                                                            <%-- '<%# Bind("IsMandatory") %>'--%>
                                                                                                            <asp:CheckBox ID="chkIsMandatory" runat="server" Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IsMandatory")))%>'></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Image Copy" ItemStyle-Width="10%">
                                                                                                        <ItemTemplate>
                                                                                                            <%--'<%# Bind("IsNeedImageCopy") %>'--%>
                                                                                                            <asp:CheckBox ID="chkIsNeedImageCopy" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IsNeedImageCopy")))%>'></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Collected" ItemStyle-Width="10%">
                                                                                                        <ItemTemplate>
                                                                                                            <%--'<%# Bind("Collected") %>'--%>
                                                                                                            <asp:CheckBox ID="chkCollected" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Collected")))%>'></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Scanned" ItemStyle-Width="10%">
                                                                                                        <ItemTemplate>
                                                                                                            <%--'<%# Bind("Scanned") %>'--%>
                                                                                                            <asp:CheckBox ID="chkScanned" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Scanned")))%>'></asp:CheckBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Remark" ItemStyle-Width="7%">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtRemark" runat="server" onkeypress="fnCheckSpecialChars(true)"
                                                                                                                Width="130px" onkeyup="maxlengthfortxt(60)" Text='<%# Bind("Remark") %>' MaxLength="60"
                                                                                                                TextMode="MultiLine"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Value" ItemStyle-Width="15%">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:TextBox ID="txtValues" runat="server" onkeypress="fnCheckSpecialChars(false)"
                                                                                                                Width="130px" onkeyup="fnNotAllowPasteSpecialChar(this,'special')" Text='<%# Bind("Values") %>'
                                                                                                                MaxLength="40"></asp:TextBox>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="FollowUp" ItemStyle-Width="10%">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:CheckBox Enabled="false" runat="server" ID="chkIs_FollowUp" />
                                                                                                            <asp:Label ID="lblchkIs_FollowUp" Visible="false" runat="server" Text='<%#Eval("Is_FollowUp")%>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Scanned Reference">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lnkScannedReference" runat="server" Text="View" OnClick="lnkScannedReference_Click">
                                                                                                            </asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Document ID" ItemStyle-Width="10%" Visible="false">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblDocumentId" runat="server" Text='<%#Eval("DocumentId")%>'></asp:Label>
                                                                                                            <asp:Label ID="lblDocumentPath" runat="server" Text='<%#Eval("Document_Path")%>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="Document_Path" HeaderText="Document Path" ItemStyle-Width="10%"
                                                                                                        Visible="false" />
                                                                                                </Columns>
                                                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                                                <RowStyle HorizontalAlign="Center" />
                                                                                            </asp:GridView>
                                                                                        </div>
                                                                                    </div>
                                                                                </asp:Panel>
                                                                                <asp:Panel runat="server" ID="Panel8" CssClass="stylePanel" GroupingText="Pre-Disbursement Document Details"
                                                                                    Width="99%" Visible="false">
                                                                                    <%--<div id="div11" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">--%>
                                                                                    <div style="overflow-x: hidden; overflow-y: auto; width: 100%">
                                                                                        <br />
                                                                                        <asp:GridView ID="gvPRDDT" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                            DataKeyNames="PRDDC_Doc_Cat_ID" CssClass="styleInfoLabel" OnRowDataBound="gvPRDDT_RowDataBound">
                                                                                            <Columns>
                                                                                                <%--BorderColor="Gray"--%>
                                                                                                <asp:TemplateField HeaderText="PRDDC TypeId" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblPRTID" runat="server" Text='<%# Bind("PRDDC_Doc_Cat_ID") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="PRDDC Type">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblType" runat="server" Text='<%# Bind("PRDDC_Doc_Type") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="PRDDC Description">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("PRDDC_Doc_Description") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="CollectedById" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblCollectedBy" runat="server" Text='<%# Bind("Collected_By_Id") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Collected By">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="ddlCollectedBy" runat="server" OnSelectedIndexChanged="ddlCollectedBy_SelectedIndexChanged"
                                                                                                            AutoPostBack="true">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Collected Date">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtCollectedDate" runat="server" Width="95%" Text='<%#Bind("GetDates") %>'>
                                                                                                        </asp:TextBox>
                                                                                                        <cc1:CalendarExtender ID="calCollectedDate" runat="server" Enabled="True" TargetControlID="txtCollectedDate"
                                                                                                            OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                                                        </cc1:CalendarExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="ScannedById" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblScannedBy" runat="server" Text='<%# Bind("Scanned_By_Id") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Scanned By">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="ddlScannedBy" runat="server" OnSelectedIndexChanged="ddlScannedBy_SelectedIndexChanged"
                                                                                                            AutoPostBack="true">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Scanned Date">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtScannedDate" runat="server" Width="95%" Text='<%# Bind("Scanned_Date") %>'>
                                                                                                        </asp:TextBox>
                                                                                                        <cc1:CalendarExtender ID="calScannedDate" runat="server" Enabled="True" TargetControlID="txtScannedDate"
                                                                                                            OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                                                        </cc1:CalendarExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="File Upload">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txOD" runat="server" Width="100px" MaxLength="500" Text='<%# Bind("Document_Path") %>'
                                                                                                            Visible="false"></asp:TextBox>
                                                                                                        <cc1:AsyncFileUpload ID="asyFileUpload" runat="server" Width="175px" OnClientUploadComplete="uploadComplete"
                                                                                                            OnUploadedComplete="asyncFileUpload_UploadedComplete" />
                                                                                                        <asp:Label runat="server" ID="myThrobber"></asp:Label>
                                                                                                        <asp:HiddenField ID="hidThrobber" runat="server" />
                                                                                                        <%--<asp:TextBox ID="txthidden" runat="server" Width="100px" MaxLength="500" Text='<%# Bind("Document_Path") %>' Visible="false"></asp:TextBox>--%>
                                                                                                        <%--<input id="bOD" onclick="return openFileDialog(this.id,'bOD','fileOpenDocument','txOD', 'paper')" style="width: 17px; height: 22px" type="button" runat="server" title="Click to browse file" value="..." visible="False" />--%>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Scan Ref No" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtScan" runat="server" Width="95%" MaxLength="12" Enabled="false"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="txtScan"
                                                                                                            FilterType="Numbers, Custom , UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                                                            Enabled="True">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Document">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton runat="server" ID="hyplnkView" CommandArgument='<%# Bind("Scanned_Ref_No") %>'
                                                                                                            OnClick="lnkPRDDView_Click" Text="View"></asp:LinkButton>
                                                                                                        <asp:Label runat="server" ID="lblPath" Text='<%# Eval("Scanned_Ref_No")%>' Visible="false"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Remarks">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtRemarks" runat="server" Width="200px" Text='<%# Eval("Remarks")%>'
                                                                                                            TextMode="MultiLine" onkeyup="maxlengthfortxt(100)" MaxLength="100"></asp:TextBox>
                                                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" TargetControlID="txtRemarks"
                                                                                                            FilterType="Numbers, Custom , UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                                                                            Enabled="True">
                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Collected">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:CheckBox ID="CbxCheck" runat="server" />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="IsScanned" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblScanned" runat="server" Text='<%#Bind("IS_NeedScanCopy") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="IsMandatory" Visible="false">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblMandatory" runat="server" Text='<%#Bind("IS_Mandatory") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                        <br />
                                                                                    </div>
                                                                                </asp:Panel>
                                                                            </ContentTemplate>
                                                                        </cc1:TabPanel>
                                                                        <cc1:TabPanel ID="TabAssetDetails" runat="server" BackColor="Red" CssClass="tabpan"
                                                                            Width="100%">
                                                                            <HeaderTemplate>
                                                                                Asset Details
                                                                            </HeaderTemplate>
                                                                            <ContentTemplate>
                                                                                <div id="div12" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                                    <br />
                                                                                    <asp:GridView ID="gvAssetDetails" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvAssetDetails_RowDeleting"
                                                                                        OnRowDataBound="gvAssetDetails_RowDataBound" Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="SlNo" ItemStyle-HorizontalAlign="Right">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkAssetSerialNo" runat="server" Text='<%#Bind("SlNo") %>' Style="text-align: right"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="Asset_Code" HeaderText="Asset" />
                                                                                            <asp:BoundField DataField="Noof_Units" HeaderText="Unit Count" ItemStyle-HorizontalAlign="Right" />
                                                                                            <asp:BoundField DataField="Unit_Value" HeaderText="Unit Value" ItemStyle-HorizontalAlign="Right" />
                                                                                            <asp:BoundField DataField="Finance_Amount" HeaderText="Finance Amount" ItemStyle-HorizontalAlign="Right" />
                                                                                            <asp:BoundField DataField="Required_FromDate" HeaderText="Required From" Visible="false" />
                                                                                            <asp:TemplateField HeaderText="Proforma">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkView" CausesValidation="false" runat="server" Text="View"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                                        Text="Remove"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="ProformaId" Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblProformaId" runat="server" Text='<%#Bind("Proforma_Id") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="Lease_Asset_No" HeaderText="Lease Asset No" ItemStyle-HorizontalAlign="Right" />
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                    <br />
                                                                                </div>
                                                                                  <div id="div1" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                                    <br />
                                                                                       <asp:GridView ID="grvloanassethdr" runat="server" AutoGenerateColumns="False" 
                                                                                       Width="100%" ShowFooter="true" OnRowCommand="grvloanassethdr_RowCommand" OnRowDataBound="grvloanassethdr_RowDataBound">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="SlNo" ItemStyle-HorizontalAlign="Right">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkAssetSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>' Style="text-align: right"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                             
                                                                                                
                                                                                            </asp:TemplateField>
                                                                                             <asp:TemplateField HeaderText="Property Name" ItemStyle-HorizontalAlign="Left">
                                                                                                <ItemTemplate>
                                                                                                    <asp:TextBox ID="txtpropertyname" runat="server" Text='<%#Bind("Property_Name") %>' Style="text-align: left" Width="60%"></asp:TextBox>
                                                                                                </ItemTemplate>
                                                                                                 
                                                                                                 <FooterTemplate>
                                                                                                     <asp:TextBox ID="txtpropertyfooter" runat="server" ></asp:TextBox>
                                                                                                 </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                           
                                                                                            <asp:TemplateField HeaderText="Action">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnRemove1" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                                        Text="Remove"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                         <asp:LinkButton ID="lnadd" CausesValidation="false" runat="server" CommandName="Add"
                                                                                                        Text="Add"></asp:LinkButton>
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                          
                                                                                           
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                   <%-- <asp:GridView ID="grvloanasset" runat="server" AutoGenerateColumns="False" OnRowDeleting="grvloanasset_RowDeleting"
                                                                                       Width="100%" OnRowDataBound="grvloanasset_RowDataBound" >
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="SlNo" ItemStyle-HorizontalAlign="Right">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnkAssetSerialNo" runat="server" Text='<%#Bind("SlNo") %>' Style="text-align: right"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:BoundField DataField="Asset_Type" HeaderText="Asset_Type"  Visible="false"/>
                                                                                            <asp:BoundField DataField="Asset_Type_Value" HeaderText="Item_type" />
                                                                                            <asp:BoundField DataField="Builder_Name" HeaderText="Builder_Name" ItemStyle-HorizontalAlign="Left" />
                                                                                            <asp:BoundField DataField="Flat_Area" HeaderText="Flat_Area" ItemStyle-HorizontalAlign="Right" />
                                                                                            <asp:BoundField DataField="Buildup_area" HeaderText="Buildup_Area" ItemStyle-HorizontalAlign="Right" />
                                                                                            <asp:BoundField DataField="Flat_No" HeaderText="Flat_No"/>
                                                                                             <asp:BoundField DataField="Address" HeaderText="Address"  />
                                                                                             <asp:BoundField DataField="Asset_Value" HeaderText="Asset_Value"  />
                                                                                             <asp:BoundField DataField="Land_Surveyor" HeaderText="Land_Surveyor"  />
                                                                                           
                                                                                            <asp:TemplateField HeaderText="">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnRemove1" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                                        Text="Remove"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                          
                                                                                           
                                                                                        </Columns>
                                                                                    </asp:GridView>--%>
                                                                                    <br />
                                                                                </div>
                                                                                <table width="98%">
                                                                                    <tr align="center">
                                                                                        <td>
                                                                                            <asp:Button ID="btnAddAsset" runat="server" CssClass="styleSubmitButton" Text="Add Asset"
                                                                                                UseSubmitBehavior="False" ValidationGroup=" " />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ContentTemplate>
                                                                        </cc1:TabPanel>
                                                                    </cc1:TabContainer>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <div id="Div16" style="overflow: auto; text-align: center">
                                                            <asp:Button ID="btnConfigure" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                OnClick="btnConfigure_Click" UseSubmitBehavior="False" Text="Configure" />
                                                            <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                UseSubmitBehavior="False" Text="Print" Enabled="false" />
                                                        </div>
                                                    
</ContentTemplate>
</asp:UpdatePanel>



                                            
</ContentTemplate>
                                        


</cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabOfferTerms" CssClass="tabpan" BackColor="Red"
                                            Width="100%">
                                            <HeaderTemplate>
                                                Offer Terms
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="upOfferTerms" runat="server">
                                                    <ContentTemplate>
                                                        <table width="99%" border="0">
                                                            <tr>
                                                                <td width="100%" class="styleContentTable">
                                                                    <asp:Panel ID="divRoiRules" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                                        <div style="float: left;">
                                                                            ROI/Payment Rule Card details
                                                                        </div>
                                                                        <div style="float: left; margin-left: 20px;">
                                                                            <asp:Label ID="lblDetails" runat="server">(Show Details...)</asp:Label>
                                                                        </div>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                            <asp:ImageButton ID="imgDetails" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                                AlternateText="(Show Details...)" />
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="divROIRuleInfo" Style="overflow: auto; height: 150px; width: 100%"
                                                                        runat="server">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td width="50%">
                                                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="25%">
                                                                                                <asp:Label ID="lblROIRuleList" runat="server" CssClass="styleReqFieldLabel" Text="ROI Rule"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign" width="70%">
                                                                                                <asp:DropDownList ID="ddlROIRuleList" runat="server" Width="100%" onchange="FunRestIrr();">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvddlROIRuleList" runat="server" ControlToValidate="ddlROIRuleList"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                                                                    ErrorMessage="Select a ROI rule from the list" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                            <td width="5%">
                                                                                                <asp:Button ID="btnFetchROI" Text="Go" runat="server" CssClass="styleSubmitShortButton"
                                                                                                    OnClientClick="return FunddlRoiOnChange('ROI')" OnClick="btnFetchROI_Click" Style="display: block" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td width="50%">
                                                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                                                        <tr>
                                                                                            <td class="styleFieldLabel" width="25%">
                                                                                                <asp:Label ID="lblPaymentRuleList" runat="server" CssClass="styleReqFieldLabel" Text="Payment Rule"></asp:Label>
                                                                                            </td>
                                                                                            <td class="styleFieldAlign" width="70%">
                                                                                                <asp:DropDownList ID="ddlPaymentRuleList" runat="server" Width="100%">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvddlPaymentRuleList" runat="server" ControlToValidate="ddlPaymentRuleList"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                                                                    ErrorMessage="Select a Payment rule from the list" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                            </td>
                                                                                            <td width="5%">
                                                                                                <asp:Button ID="btnFetchPayment" runat="server" CssClass="styleSubmitShortButton"
                                                                                                    OnClick="btnFetchPayment_Click" Text="Go" Style="display: block" OnClientClick="return FunddlRoiOnChange('Payment')" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td width="100%" valign="top">
                                                                                    <div id="div7" style="overflow: visible; width: 100%" runat="server">
                                                                                        <asp:Panel runat="server" ID="Panel3" ScrollBars="Auto" CssClass="stylePanel" GroupingText="ROI Rule"
                                                                                            Width="99%">
                                                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                                <tr>
                                                                                                    <td width="50%" valign="top">
                                                                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                                            <tr id="tr_lblROI_Rule_Number" runat="server">
                                                                                                                <td id="Td7" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblROI_Rule_Number" runat="server" CssClass="styleDisplayLabel" Text="ROI Rule Number"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td8" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txt_ROI_Rule_Number" runat="server" ReadOnly="True" Style="width: 145px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblRate_Type" runat="server">
                                                                                                                <td id="Td1" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblRate_Type" runat="server" CssClass="styleDisplayLabel" Text="Rate Type"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td6" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Rate_Type" runat="server" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblModel_Description" runat="server">
                                                                                                                <td id="Td4" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblModel_Description" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Model Description"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td5" runat="server" class="styleFieldAlign" style="margin-left: 40px">
                                                                                                                    <asp:TextBox ID="txt_Model_Description" runat="server" ReadOnly="True" Style="width: 145px"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblReturn_Pattern" runat="server">
                                                                                                                <td id="Td9" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblReturn_Pattern" runat="server" CssClass="styleDisplayLabel" Text="Return Pattern"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td10" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Return_Pattern" runat="server" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblTime_Value" runat="server">
                                                                                                                <td id="Td11" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblTime_Value" runat="server" CssClass="styleDisplayLabel" Text="Time"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td12" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Time_Value" runat="server" onchange="FunRestIrr();" Width="150px"
                                                                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_Time_Value_SelectedIndexChanged">
                                                                                                                    </asp:DropDownList>
                                                                                                                    <asp:RequiredFieldValidator ID="rfvTimeValue" runat="server" ControlToValidate="ddl_Time_Value"
                                                                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a time Value from the list"
                                                                                                                        InitialValue="0" SetFocusOnError="True" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblFrequency" runat="server">
                                                                                                                <td id="Td13" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblFrequency" runat="server" CssClass="styleDisplayLabel" Text="Frequency"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td14" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Frequency" runat="server" onchange="FunRestIrr();" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                    <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="ddl_Frequency"
                                                                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a frequency from the list"
                                                                                                                        InitialValue="0" SetFocusOnError="True" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblRepayment_Mode" runat="server">
                                                                                                                <td id="Td15" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblRepayment_Mode" runat="server" CssClass="styleDisplayLabel" Text="Repayment Mode"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td16" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Repayment_Mode" runat="server" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <%--<tr id="tr_lblCollateralTypeRate" runat="server">
                                                                                                                <td id="Td2" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblIRRRate" runat="server" CssClass="styleDisplayLabel" Text="IRR Rate"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td3" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txtIRRRate" runat="server" Style="width: 70px; text-align: right;"></asp:TextBox> &nbsp;&nbsp;&nbsp;&nbsp; 
                                                                                                                    <asp:Label ID="lblCollateralTypeRate" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Collateral Type Rate"></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                                    <asp:TextBox ID="txtCollateralTypeRate" runat="server" OnTextChanged="txtCollateralTypeRate_OnTextChanged" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                                                                        Style="width: 70px; text-align: right;" AutoPostBack="true" ></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>--%>
                                                                                                            <tr id="tr_lblRate" runat="server">
                                                                                                                <td id="Td17" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblRate" runat="server" CssClass="styleDisplayLabel" Text="Rate"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td18" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txtRate" runat="server" onchange="FunRestIrr();" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                                                                        Width="70px"></asp:TextBox>
                                                                                                                    <asp:RequiredFieldValidator ID="rfvRate" runat="server" ControlToValidate="txtRate"
                                                                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Rate" SetFocusOnError="True"
                                                                                                                        ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblIRR_Rest" runat="server">
                                                                                                                <td id="Td19" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblIRR_Rest" runat="server" CssClass="styleDisplayLabel" Text="IRR Rest"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td20" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_IRR_Rest" runat="server" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td width="50%" valign="top">
                                                                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                                            <tr id="tr_lblRecovery_Pattern_Year1" runat="server">
                                                                                                                <td id="Td25" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblRecovery_Pattern_Year1" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Recovery Pattern Year1"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td26" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txt_Recovery_Pattern_Year1" runat="server" Style="width: 70px; text-align: right;"
                                                                                                                        ReadOnly="true"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblRecovery_Pattern_Year2" runat="server">
                                                                                                                <td id="Td27" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblRecovery_Pattern_Year2" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Recovery Pattern Year2"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td28" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txt_Recovery_Pattern_Year2" runat="server" Style="width: 70px; text-align: right;"
                                                                                                                        ReadOnly="true"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblRecovery_Pattern_Year3" runat="server">
                                                                                                                <td id="Td29" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblRecovery_Pattern_Year3" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Recovery Pattern Year3"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td30" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txt_Recovery_Pattern_Year3" runat="server" Style="width: 70px; text-align: right;"
                                                                                                                        ReadOnly="true"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblRecovery_Pattern_Rest" runat="server">
                                                                                                                <td id="Td31" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblRecovery_Pattern_Rest" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Recovery Pattern Year Rest"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td32" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txt_Recovery_Pattern_Rest" runat="server" Style="width: 70px; text-align: right;"
                                                                                                                        ReadOnly="true"></asp:TextBox>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblInsurance" runat="server">
                                                                                                                <td id="Td33" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblInsurance" runat="server" CssClass="styleDisplayLabel" Text="Insurance"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td34" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Insurance" runat="server" onchange="FunRestIrr();" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblInterest_Calculation" runat="server">
                                                                                                                <td id="Td21" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblInterest_Calculation" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Interest Calculation"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td22" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Interest_Calculation" runat="server" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblInterest_Levy" runat="server">
                                                                                                                <td id="Td23" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblInterest_Levy" runat="server" CssClass="styleDisplayLabel" Text="Interest Levy"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td24" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:DropDownList ID="ddl_Interest_Levy" runat="server" Width="150px">
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblResidual_Value" runat="server">
                                                                                                                <td id="Td35" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblResidual_Value" runat="server" CssClass="styleDisplayLabel" Text="Residual Value"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td36" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:CheckBox ID="chk_lblResidual_Value" runat="server" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblMargin" runat="server">
                                                                                                                <td id="Td37" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblMargin" runat="server" CssClass="styleDisplayLabel" Text="Margin"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td38" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:CheckBox ID="chk_lblMargin" runat="server" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr id="tr_lblMargin_Percentage" runat="server">
                                                                                                                <td id="Td39" runat="server" class="styleFieldLabel">
                                                                                                                    <asp:Label ID="lblMargin_Percentage" runat="server" CssClass="styleDisplayLabel"
                                                                                                                        Text="Margin Percentage"></asp:Label>
                                                                                                                </td>
                                                                                                                <td id="Td40" runat="server" class="styleFieldAlign">
                                                                                                                    <asp:TextBox ID="txt_Margin_Percentage" runat="server" AutoPostBack="True" onchange="FunRestIrr();"
                                                                                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" OnTextChanged="txt_Margin_Percentage_TextChanged"
                                                                                                                        Style="width: 70px"></asp:TextBox>
                                                                                                                    <asp:RequiredFieldValidator ID="rfvMarginPercent" runat="server" ControlToValidate="txt_Margin_Percentage"
                                                                                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Margin Percentage"
                                                                                                                        SetFocusOnError="True" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top" width="100%">
                                                                                    <div id="div8" style="overflow: auto; width: 100%" runat="server">
                                                                                        <asp:Panel runat="server" ID="Panel4" ScrollBars="Auto" CssClass="stylePanel" GroupingText="Payment Rule"
                                                                                            Width="99%">
                                                                                            <asp:GridView ID="gvPaymentRuleDetails" runat="server" Width="100%">
                                                                                            </asp:GridView>
                                                                                        </asp:Panel>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                    <cc1:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="divROIRuleInfo"
                                                                        ExpandControlID="divRoiRules" CollapseControlID="divRoiRules" Collapsed="True"
                                                                        TextLabelID="lblDetails" ImageControlID="imgDetails" ExpandedText="(Hide Details...)"
                                                                        CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                        CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="98%">
                                                                        <tr>
                                                                            <td class="styleFieldLabel" width="8%">
                                                                                <asp:Label ID="lblResidualValue_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                    Text="Residual%"></asp:Label>
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="12%">
                                                                                <asp:TextBox ID="txtResidualValue_Cashflow" Width="90%" runat="server" Style="text-align: right;"
                                                                                    OnTextChanged="txtResidualValue_Cashflow_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftxtResidualPercentage" runat="server" TargetControlID="txtResidualValue_Cashflow"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvResidualValue" runat="server" ControlToValidate="txtResidualValue_Cashflow"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Offer Terms" SetFocusOnError="True"
                                                                                    ErrorMessage="Enter the Residual % or Amount"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                            <td class="styleFieldLabel" width="8%">
                                                                                <asp:Label ID="lblResidualAmt_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                    Text="Amount"></asp:Label>
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="12%">
                                                                                <asp:TextBox ID="txtResidualAmt_Cashflow" Width="90%" runat="server" MaxLength="7" Style="text-align: right;"
                                                                                    OnTextChanged="txtResidualAmt_Cashflow_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FtexResidualAmt_Cashflow" runat="server" TargetControlID="txtResidualAmt_Cashflow"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                            <td width="8%" class="styleFieldLabel">
                                                                                <asp:Label ID="lblMarginMoneyPer_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                    Text="Margin%"></asp:Label>
                                                                            </td>
                                                                            <td class="styleFieldAlign" width="12%">
                                                                                <asp:TextBox ID="txtMarginMoneyPer_Cashflow" Width="90%" runat="server" Style="text-align: right;"
                                                                                    OnTextChanged="txt_Margin_Percentage_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftxtMarginPercentage" runat="server" TargetControlID="txtMarginMoneyPer_Cashflow"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                            <td class="styleFieldLabel" width="8%">
                                                                                <asp:Label ID="lblMarginMoneyAmount_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                    Text="Amount"></asp:Label>
                                                                            </td>
                                                                            <td width="12%" class="styleFieldAlign">
                                                                                <asp:TextBox ID="txtMarginMoneyAmount_Cashflow" Width="90%" runat="server" MaxLength="7" Style="text-align: right;"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FtexMarginMoneyAmount_Cashflow" runat="server" TargetControlID="txtMarginMoneyAmount_Cashflow"
                                                                                    FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                            <td class="styleFieldLabel" width="8%">
                                                                                <asp:Label ID="lblFBDate" runat="server" CssClass="styleDisplayLabel" Text="FB Day"></asp:Label>
                                                                            </td>
                                                                            <td width="12%" class="styleFieldAlign">
                                                                                <asp:TextBox ID="txtFBDate" runat="server" onchange="FunRestIrr();" MaxLength="2"
                                                                                    Style="text-align: right; width: 50px;"></asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftxtFBDate" runat="server" TargetControlID="txtFBDate"
                                                                                    FilterType="Numbers" Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RangeValidator ID="rngFBDate" runat="server" ErrorMessage="FB Date Should be between 1 and 31"
                                                                                    Display="None" ControlToValidate="txtFBDate" CssClass="styleMandatoryLabel" ValidationGroup="Offer Terms"
                                                                                    MinimumValue="1" MaximumValue="31" Type="Integer">
                                                                                </asp:RangeValidator>
                                                                                <asp:RequiredFieldValidator ID="rfvFBDate" runat="server" ControlToValidate="txtFBDate"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Offer Terms" SetFocusOnError="True"
                                                                                    ErrorMessage="Enter the FB Date"></asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hdnROIRule" runat="server" />
                                                                    <asp:HiddenField ID="hdnPayment" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%">
                                                                    <asp:Panel ID="panInflow" runat="server" CssClass="stylePanel" GroupingText="Cash Inflow details"
                                                                        Width="100%">
                                                                        <div style="overflow: auto; width: 100%;">
                                                                            <asp:UpdatePanel ID="upInflow" runat="server">
                                                                                <ContentTemplate>
                                                                                    <asp:GridView ID="gvInflow" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvInflow_RowDeleting"
                                                                                        ShowFooter="True" OnRowCreated="gvInflow_RowCreated" Width="100%">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Date" ItemStyle-Width="12%" FooterStyle-Width="12%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblDate_GridInflow" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtDate_GridInflow" runat="server" Width="95%">
                                                                                                    </asp:TextBox>
                                                                                                    <cc1:CalendarExtender ID="CalendarExtenderSD_InflowDate" runat="server" Enabled="True"
                                                                                                        TargetControlID="txtDate_GridInflow" OnClientDateSelectionChanged="checkApplicationDate">
                                                                                                    </cc1:CalendarExtender>
                                                                                                    <asp:RequiredFieldValidator ID="rfvtxtDate_GridInflow" runat="server" ControlToValidate="txtDate_GridInflow"
                                                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                        ErrorMessage="Enter the Date in Inflow"></asp:RequiredFieldValidator>
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Cash flow Id" Visible="False">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblInflowid" runat="server" Text='<%# Bind("CashInFlowID") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Cash flow Description" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblInflowDesc" runat="server" Text='<%# Bind("CashInFlow") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:DropDownList ID="ddlInflowDesc" runat="server" Width="99%">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:RequiredFieldValidator ID="rfvddlInflowDesc" runat="server" ControlToValidate="ddlInflowDesc"
                                                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                        InitialValue="-1" ErrorMessage="Select a Cash flow Description in Inflow"></asp:RequiredFieldValidator>
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Inflow from" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblInflowFrom" runat="server" Text='<%# Bind("InflowFrom") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:DropDownList ID="ddlEntityName_InFlowFrom" runat="server" Width="99%" AutoPostBack="True"
                                                                                                        OnSelectedIndexChanged="ddlEntityName_InFlowFrom_SelectedIndexChanged">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:RequiredFieldValidator ID="rfvddlEntityName_InFlowFrom" runat="server" ControlToValidate="ddlEntityName_InFlowFrom"
                                                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                        InitialValue="-1" ErrorMessage="Select a Inflow from"></asp:RequiredFieldValidator>
                                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlEntityName_InFlowFrom"
                                                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                        InitialValue="0" ErrorMessage="Select a Inflow from in Inflow"></asp:RequiredFieldValidator>
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="In flow from ID" Visible="False">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblInflowFromId" runat="server" Text='<%# Bind("InflowFromID") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Entity ID" Visible="False">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblEntityID_InFlow" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                                <HeaderTemplate>
                                                                                                    <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>
                                                                                                </HeaderTemplate>
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblEntityName_InFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <uc2:Suggest ID="ddlEntityName_InFlow" runat="server" ServiceMethod="GetVendors"
                                                                                                ErrorMessage="Select a Customer/Entity Name in Inflow" ReadOnly="true" Width="250px"
                                                                                                ValidationGroup="Inflow" IsMandatory="true" />
                                                                                                    <%--<asp:DropDownList ID="ddlEntityName_InFlow" runat="server" Width="99%">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:RequiredFieldValidator ID="rfvddlEntityName_InFlow" runat="server" ControlToValidate="ddlEntityName_InFlow"
                                                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                        InitialValue="0" ErrorMessage="Select a Customer/Entity name in Inflow"></asp:RequiredFieldValidator>--%>
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Amount" ItemStyle-Width="11%" FooterStyle-Width="11%"
                                                                                                ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblAmount_Inflow" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:TextBox ID="txtAmount_Inflow" Width="95%" runat="server" MaxLength="10" Style="text-align: right">
                                                                                                    </asp:TextBox>
                                                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Inflow" runat="server" FilterType="Numbers"
                                                                                                        TargetControlID="txtAmount_Inflow">
                                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                                    <asp:RequiredFieldValidator ID="rfvtxtAmount_Inflow" runat="server" ControlToValidate="txtAmount_Inflow"
                                                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                        ErrorMessage="Enter the Amount in Inflow"></asp:RequiredFieldValidator>
                                                                                                </FooterTemplate>
                                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                                                ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                                                <ItemTemplate>
                                                                                                    <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                                                        Text="Remove"></asp:LinkButton>
                                                                                                </ItemTemplate>
                                                                                                <FooterTemplate>
                                                                                                    <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Inflow"
                                                                                                        OnClick="btnAddInflow_OnClick" CssClass="styleGridShortButton" />
                                                                                                </FooterTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <%-- Columns to be added for IRR Calculation--%>
                                                                                            <asp:TemplateField Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblAccountingIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblBusinessIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblComapnyIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <asp:TemplateField Visible="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                            <%-- Columns to be added for IRR Calculation Ends--%>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel runat="server" ID="div5" ScrollBars="Auto" CssClass="stylePanel" GroupingText="Cash Outflow details"
                                                                        Width="100%">
                                                                        <div style="overflow: auto; width: 100%;">
                                                                            <asp:GridView ID="gvOutFlow" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvOutFlow_RowDeleting"
                                                                                ShowFooter="True" OnRowCreated="gvOutFlow_RowCreated" Width="100%">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="12%" FooterStyle-Width="12%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDate_GridOutflow" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtDate_GridOutflow" runat="server" Width="95%">
                                                                                            </asp:TextBox>
                                                                                            <cc1:CalendarExtender ID="CalendarExtenderSD_OutflowDate" runat="server" Enabled="True"
                                                                                                TargetControlID="txtDate_GridOutflow" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                                            </cc1:CalendarExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvtxtDate_GridOutflow" runat="server" ControlToValidate="txtDate_GridOutflow"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                ErrorMessage="Enter the Date in Outflow"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Cash flow Id" Visible="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblOutflowid" runat="server" Text='<%# Bind("CashOutFlowID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Cash flow Description" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblOutflowDesc" runat="server" Text='<%# Bind("CashOutFlow") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlOutflowDesc" runat="server" Width="99%">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvddlOutflowDesc" runat="server" ControlToValidate="ddlOutflowDesc"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                InitialValue="-1" ErrorMessage="Select a Cash flow Description in Outflow"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Payement to ID" Visible="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPayementToId" runat="server" Text='<%# Bind("OutflowFromID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Payment to" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPaymentto" runat="server" Text='<%# Bind("OutflowFrom") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlPaymentto_OutFlow" runat="server" Width="99%" AutoPostBack="True"
                                                                                                OnSelectedIndexChanged="ddlPaymentto_OutFlow_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvddlPaymentto_OutFlow" runat="server" ControlToValidate="ddlPaymentto_OutFlow"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                InitialValue="-1" ErrorMessage="Select Payment to in Outflow"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Entity ID" Visible="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEntityID_OutFlow" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Entity Name" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEntityName_OutFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <uc2:Suggest ID="ddlEntityName_OutFlow" runat="server" ServiceMethod="GetVendors"
                                                                                                ErrorMessage="Select a Customer/Entity Name in Outflow" ReadOnly="true" Width="250px"
                                                                                                ValidationGroup="Outflow" IsMandatory="true" />
                                                                                            <%-- <asp:DropDownList ID="ddlEntityName_OutFlow" runat="server" Width="99%">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvddlEntityName_OutFlow" runat="server" ControlToValidate="ddlEntityName_OutFlow"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                InitialValue="0" ErrorMessage="Select a Customer/Entity Name in Outflow"></asp:RequiredFieldValidator>--%>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Amount" ItemStyle-Width="11%" FooterStyle-Width="11%"
                                                                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAmount_Outflow" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAmount_Outflow" Width="95%" runat="server" MaxLength="10" Style="text-align: right">
                                                                                            </asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Outflow" runat="server" FilterType="Numbers"
                                                                                                TargetControlID="txtAmount_Outflow">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvtxtAmount_Outflow" runat="server" ControlToValidate="txtAmount_Outflow"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                ErrorMessage="Enter the Amount in Outflow"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ItemStyle-Width="7%" FooterStyle-Width="7%" FooterStyle-HorizontalAlign="Center"
                                                                                        ItemStyle-HorizontalAlign="Center">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                                                Text="Remove"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Button ID="btnAddOut" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Outflow"
                                                                                                OnClick="btnAddOutflow_OnClick" CssClass="styleGridShortButton" />
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%-- Columns to be added for IRR Calculation--%>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblAccountingIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblBusinessIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblComapnyIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%-- Columns to be added for IRR Calculation Ends--%>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr width="98%">
                                                                <td align="right" class="styleGridHeader">
                                                                    <asp:Label ID="lblTotal" runat="server" Text="Total Out flow Amount :" Font-Bold="True"></asp:Label>
                                                                    <asp:Label ID="lblTotalOutFlowAmount" runat="server" Font-Bold="True" Text="0"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            
</ContentTemplate>
                                        


</cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabRepayment" CssClass="tabpan" BackColor="Red"
                                            Width="90%">
                                            <HeaderTemplate>
                                                Repayment
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="upRepayment" runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td align="left" width="33%" valign="top">
                                                                    <asp:Panel ID="Panel11" runat="server" CssClass="stylePanel" GroupingText="Repayment Details"
                                                                        Width="99%">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label runat="server" ID="lblTotalAmount" CssClass="styleDisplayLabel" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblFrequency_Display" runat="server" CssClass="styleDisplayLabel"
                                                                                        Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblMarginResidual" runat="server" CssClass="styleDisplayLabel" Font-Bold="false"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td align="left" valign="top" width="30%">
                                                                    <asp:Panel ID="Panel10" runat="server" CssClass="stylePanel" GroupingText="Repayment Summary"
                                                                        Width="99%">
                                                                        <asp:GridView ID="gvRepaymentSummary" runat="server" AutoGenerateColumns="false"
                                                                            Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="CashFlow" HeaderText="Cash Flow Description" />
                                                                                <asp:BoundField DataField="TotalPeriodInstall" HeaderText="Amount" ItemStyle-HorizontalAlign="Right" />
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td align="left" width="35%">
                                                                    <asp:Panel ID="panIRR" runat="server" CssClass="stylePanel" GroupingText="IRR Details"
                                                                        Width="97%">
                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblAccountIRR_Repay" CssClass="styleReqFieldLabel"
                                                                                        Text="Accounting IRR" Font-Bold="true"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtAccountIRR_Repay" runat="server" Width="100px" Style="text-align: right"
                                                                                        Font-Bold="true"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvAccountingIRR" runat="server" Display="None" ValidationGroup="Repayment Details"
                                                                                        ErrorMessage="Calculate Accounting IRR" ControlToValidate="txtAccountIRR_Repay"
                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblBusinessIRR_Repay" CssClass="styleReqFieldLabel"
                                                                                        Font-Bold="true" Text="Business IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtBusinessIRR_Repay" runat="server" Width="100px" Style="text-align: right"
                                                                                        Font-Bold="true"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvBusinessIRR" runat="server" Display="None" ValidationGroup="Repayment Details"
                                                                                        ErrorMessage="Calculate Business IRR" ControlToValidate="txtBusinessIRR_Repay"
                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label runat="server" ID="lblCompanyIRR_Repay" CssClass="styleReqFieldLabel"
                                                                                        Font-Bold="true" Text="Company IRR"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtCompanyIRR_Repay" runat="server" Width="100px" Style="text-align: right"
                                                                                        Font-Bold="true" ReadOnly="True"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="rfvCompanyIRR" runat="server" Display="None" ValidationGroup="Repayment Details"
                                                                                        ErrorMessage="Calculate Company IRR" ControlToValidate="txtCompanyIRR_Repay"
                                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Panel runat="server" ID="Panel2" CssClass="stylePanel" GroupingText="Repayment Details"
                                                                        Width="99%">
                                                                        <asp:GridView ID="gvRepaymentDetails" runat="server" AutoGenerateColumns="False"
                                                                            ShowFooter="True" OnRowDeleting="gvRepaymentDetails_RowDeleting" OnRowDataBound="gvRepaymentDetails_RowDataBound"
                                                                            OnRowCreated="gvRepaymentDetails_RowCreated" Width="100%">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="slno" HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                                                    <FooterStyle Width="2%" />
                                                                                    <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Repayment CashFlow" ItemStyle-Width="23%" FooterStyle-Width="23%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCashFlow" runat="server" Text='<%# Bind("CashFlow") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:DropDownList ID="ddlRepaymentCashFlow_RepayTab" runat="server" Width="98%" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rfvddlRepaymentCashFlow_RepayTab" runat="server"
                                                                                            ControlToValidate="ddlRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                                            Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" InitialValue="-1"
                                                                                            ErrorMessage="Select a Repayment cashflow"></asp:RequiredFieldValidator>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle Width="23%" />
                                                                                    <ItemStyle Width="23%" />
                                                                                </asp:TemplateField>
                                                                                <%--<asp:TemplateField HeaderText="Amount" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmountRepaymentCashFlow_RepayTab" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAmountRepaymentCashFlow_RepayTab" runat="server" Width="97%"
                                                                        MaxLength="10">
                                                                    </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtAmountRepaymentCashFlow_RepayTab" runat="server"
                                                                        FilterType="Numbers" TargetControlID="txtAmountRepaymentCashFlow_RepayTab">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtAmountRepaymentCashFlow_RepayTab" runat="server"
                                                                        ControlToValidate="txtAmountRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                        Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the amount"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>--%>
                                                                                <asp:TemplateField HeaderText="Per Installment Amount" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                                                                    FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPerInstallmentAmount_RepayTab" runat="server" Text='<%# Bind("PerInstall") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtPerInstallmentAmount_RepayTab" runat="server" Width="95%" Style="text-align: right;"
                                                                                            MaxLength="10">
                                                                                        </asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="ftextExtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="txtPerInstallmentAmount_RepayTab">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                                            ControlToValidate="txtPerInstallmentAmount_RepayTab" CssClass="styleMandatoryLabel"
                                                                                            Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the Per installment amount"></asp:RequiredFieldValidator>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Breakup Percentage" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblBreakup_RepayTab" runat="server" Text='<%# Bind("Breakup") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtBreakup_RepayTab" runat="server" Width="95%" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                                                        </asp:TextBox>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="From Installment" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFromInstallment_RepayTab" runat="server" Text='<%# Bind("FromInstall") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtFromInstallment_RepayTab" runat="server" Width="95%" MaxLength="3"
                                                                                            Style="text-align: right" Text="1"><%--ReadOnly="true" --%>
                                                                                        </asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="ftextExtxtFromInstallment_RepayTab" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="txtFromInstallment_RepayTab">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <%--<asp:RequiredFieldValidator ID="rfvtxtFromInstallment_RepayTab" runat="server" ControlToValidate="txtFromInstallment_RepayTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                        SetFocusOnError="True" ErrorMessage="Enter the From installment"></asp:RequiredFieldValidator>--%>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="To Installment" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblToInstallment_RepayTab" runat="server" Text='<%# Bind("ToInstall") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtToInstallment_RepayTab" runat="server" Width="95%" MaxLength="3"
                                                                                            Style="text-align: right">
                                                                                        </asp:TextBox>
                                                                                        <cc1:FilteredTextBoxExtender ID="ftextExtxtToInstallment_RepayTab" runat="server"
                                                                                            FilterType="Numbers" TargetControlID="txtToInstallment_RepayTab">
                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtToInstallment_RepayTab" runat="server" ControlToValidate="txtToInstallment_RepayTab"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                                            SetFocusOnError="True" ErrorMessage="Enter the To installment"></asp:RequiredFieldValidator>
                                                                                        <asp:CompareValidator ID="cmpvFromTOInstall" runat="server" ErrorMessage="To installment should be greater than the From installment"
                                                                                            ControlToValidate="txtToInstallment_RepayTab" ControlToCompare="txtFromInstallment_RepayTab"
                                                                                            Display="None" ValidationGroup="TabRepayment1" Type="Integer" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="From Date" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '></asp:Label>
                                                                                        <asp:TextBox ID="txRepaymentFromDate" runat="server" Visible="false" BackColor="Navy"
                                                                                            ForeColor="White" Font-Names="calibri" Font-Size="12px" Width="95%" Style="color: White"
                                                                                            Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '
                                                                                            AutoPostBack="True" OnTextChanged="txRepaymentFromDate_TextChanged"></asp:TextBox>
                                                                                        <cc1:CalendarExtender ID="calext_FromDate" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                                                            TargetControlID="txRepaymentFromDate">
                                                                                        </cc1:CalendarExtender>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtfromdate_RepayTab" runat="server" Width="95%">
                                                                                        </asp:TextBox>
                                                                                        <cc1:CalendarExtender ID="CalendarExtenderSD_fromdate_RepayTab" runat="server" Enabled="True"
                                                                                            TargetControlID="txtfromdate_RepayTab">
                                                                                        </cc1:CalendarExtender>
                                                                                        <%--  <asp:RequiredFieldValidator ID="rfvtxtfromdate_RepayTab" runat="server" ControlToValidate="txtfromdate_RepayTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                        SetFocusOnError="True" ErrorMessage="Enter the from date"></asp:RequiredFieldValidator>--%>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle Width="10%" />
                                                                                    <ItemStyle Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="To Date" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTODate_ReapyTab" runat="server" Width="100%" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ToDate")).ToString(strDateFormat) %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtToDate_RepayTab" runat="server" Width="95%" Visible="false">
                                                                                        </asp:TextBox>
                                                                                        <cc1:CalendarExtender ID="CalendarExtenderSD_ToDate_RepayTab" runat="server" Enabled="True"
                                                                                            OnClientDateSelectionChanged="checkDate_PrevSystemDate" TargetControlID="txtToDate_RepayTab">
                                                                                        </cc1:CalendarExtender>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle Width="10%" />
                                                                                    <ItemStyle Width="10%" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Add" ItemStyle-Width="5%" FooterStyle-Width="5%">
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnRemoveRepayment" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                            Text="Remove" Visible="false"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Button ID="btnAddRepayment" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                                                            OnClick="btnAddRepayment_OnClick" ValidationGroup="TabRepayment1" OnClientClick="return fnCheckPageValidators('TabRepayment1',false)"></asp:Button>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Repayment CashFlowId" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCashFlowId" runat="server" Text='<%# Bind("CashFlowId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <%-- Columns to be added for IRR Calculation--%>
                                                                                <asp:TemplateField Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAccountingIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblBusinessIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblComapnyIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <%-- Columns to be added for IRR Calculation--%>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3"></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <table width="99.5%">
                                                                        <tr align="right">
                                                                            <td>
                                                                                <asp:Button runat="server" ID="btnCalIRR" Text="Calculate IRR" CssClass="styleSubmitButton"
                                                                                    OnClick="btnCalIRR_Click" />
                                                                                <asp:Button runat="server" ID="btnReset" Text="Reset" CssClass="styleSubmitShortButton"
                                                                                    OnClick="btnReset_Click" OnClientClick="return Confirmmsg('Do you want to Reset Repayment Structure?')" />
                                                                                <input id="hdnCTR" type="hidden" runat="server" />
                                                                                <input id="hdnPLR" type="hidden" runat="server" />
                                                                                <input id="hdnRoundOff" type="hidden" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:GridView ID="grvRepayStructure" runat="server" Width="100%" AutoGenerateColumns="false">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="InstallmentNo" HeaderText="Installment No" ItemStyle-HorizontalAlign="Center" />
                                                                                        <asp:BoundField DataField="From_Date" HeaderText="From Date" ItemStyle-HorizontalAlign="Left" />
                                                                                        <asp:BoundField DataField="To_Date" HeaderText="To Date" ItemStyle-HorizontalAlign="Left" />
                                                                                        <asp:BoundField DataField="Installment_Date" HeaderText="Installment Date" ItemStyle-HorizontalAlign="Left" />
                                                                                        <asp:BoundField DataField="NoofDays" HeaderText="No of days" ItemStyle-HorizontalAlign="Right" />
                                                                                        <asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount" ItemStyle-HorizontalAlign="Right" />
                                                                                        <asp:BoundField DataField="FinanceCharges" HeaderText="Finance Charges" ItemStyle-HorizontalAlign="Right"
                                                                                            Visible="true" />
                                                                                        <asp:BoundField DataField="PrincipalAmount" HeaderText="Principal Amount" ItemStyle-HorizontalAlign="Right"
                                                                                            Visible="true" />
                                                                                        <asp:BoundField DataField="Insurance" HeaderText="Insurance" ItemStyle-HorizontalAlign="Right" />
                                                                                       <asp:BoundField DataField="Others" HeaderText="Cus IW" ItemStyle-HorizontalAlign="Right" />
                                                             <asp:BoundField DataField="CUS_OW" HeaderText="Cus OW" ItemStyle-HorizontalAlign="Right" />
                                                             <asp:BoundField DataField="ET_IW" HeaderText="ET IW" ItemStyle-HorizontalAlign="Right" />
                                                             <asp:BoundField DataField="ET_OW" HeaderText="ET OW" ItemStyle-HorizontalAlign="Right" />
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                                <br />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <asp:Button ID="btnGenerateRepay" runat="server" Style="display: none" Text="Button"
                                                                        CausesValidation="false" OnClick="btnGenerateRepay_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnCalIRR" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnGenerateRepay" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            
</ContentTemplate>
                                        


</cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabInvoice" CssClass="tabpan" BackColor="Red" Width="85%">
                                            <HeaderTemplate>
                                                Guarantee / Invoice Details
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="upGuarantor" runat="server">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Panel runat="server" ID="Panel1" CssClass="stylePanel" GroupingText="Guarantor Details">
                                                                        <div id="div18" style="overflow: auto; width: 100%;" runat="server">
                                                                            <asp:GridView ID="gvGuarantor" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                                Width="100%" OnRowDeleting="gvGuarantor_RowDeleting" OnRowDataBound="gvGuarantor_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Guarantor type" ItemStyle-Width="18%" FooterStyle-Width="18%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="ddlGuarantortype_GuarantorTab" runat="server" Text='<%# Bind("Guarantor") %>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlGuarantortype_GuarantorTab" runat="server" AutoPostBack="true"
                                                                                                OnSelectedIndexChanged="ddlGuarantortype_SelectedIndexChanged" Width="100%">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvddlGuarantortype_GuarantorTab" runat="server"
                                                                                                ControlToValidate="ddlGuarantortype_GuarantorTab" CssClass="styleMandatoryLabel"
                                                                                                ValidationGroup="Guarantor" Display="None" InitialValue="-1" SetFocusOnError="True"
                                                                                                ErrorMessage="Select a Guarantor type"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="GuaranteeID" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblGuaranteeID" runat="server" Text='<%# Bind("Code") %>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Guarantor" ItemStyle-Width="50%" FooterStyle-Width="50%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="ddlCode_GuarantorTab" runat="server" Text='<%# Bind("Name") %>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <uc2:LOV ID="ucCustomerLov" runat="server" TextWidth="80%" onblur="fnLoadCustomerg()" />
                                                                                            <%--<asp:DropDownList ID="ddlCode_GuarantorTab" runat="server" Width="100%">
                                                                                    </asp:DropDownList>--%>
                                                                                            <%-- <asp:RequiredFieldValidator ID="rfvddlCode_GuarantorTab" runat="server" ControlToValidate="ddlCode_GuarantorTab"
                                                                                        CssClass="styleMandatoryLabel" ValidationGroup="Guarantor" Display="None" InitialValue="-1"
                                                                                        SetFocusOnError="True" ErrorMessage="Select a Guarantor"></asp:RequiredFieldValidator>--%>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Guarantee Amount" ItemStyle-Width="15%" FooterStyle-Width="15%"
                                                                                        ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="txtGuaranteeamount_GuarantorTab" runat="server" Text='<%# Bind("Amount") %>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtGuaranteeamount_GuarantorTab_Footer" runat="server" Style="text-align: right"
                                                                                                Width="95%" MaxLength="10">
                                                                                            </asp:TextBox>
                                                                                            <cc1:FilteredTextBoxExtender ID="ftxtGuarateeAmount" runat="server" TargetControlID="txtGuaranteeamount_GuarantorTab_Footer"
                                                                                                FilterType="Numbers" Enabled="True">
                                                                                            </cc1:FilteredTextBoxExtender>
                                                                                            <%-- <asp:RequiredFieldValidator ID="rfvtxtGuaranteeamount_GuarantorTab" runat="server"
                                                                                                ControlToValidate="txtGuaranteeamount_GuarantorTab_Footer" CssClass="styleMandatoryLabel"
                                                                                                ValidationGroup="Guarantor" Display="None" SetFocusOnError="True" ErrorMessage="Enter the Guarantee Amount"></asp:RequiredFieldValidator>--%>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Charge Sequence" ItemStyle-Width="18%" FooterStyle-Width="18%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="ddlChargesequence_GuarantorTab" runat="server" Text='<%# Bind("ChargeSequence") %>'>
                                                                                            </asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:DropDownList ID="ddlChargesequence_GuarantorTab" runat="server" Width="96%">
                                                                                            </asp:DropDownList>
                                                                                            <%--<asp:RequiredFieldValidator ID="rfvddlChargesequence_GuarantorTab" runat="server"
                                                                                                ControlToValidate="ddlChargesequence_GuarantorTab" CssClass="styleMandatoryLabel"
                                                                                                ValidationGroup="Guarantor" Display="None" InitialValue="-1" SetFocusOnError="True"
                                                                                                ErrorMessage="Select a Charge Sequence"></asp:RequiredFieldValidator>--%>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lbtnViewCustomer" CausesValidation="false" runat="server" CommandName="Edit"
                                                                                                Text="View"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                                Text="Remove"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Button ID="LbtnAddInvoice" runat="server" CausesValidation="true" OnClick="btnAddGuarantor_OnClick"
                                                                                                Text="Add" ValidationGroup="Guarantor" CssClass="styleGridShortButton"></asp:Button>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <asp:Panel runat="server" ID="Panel5" CssClass="stylePanel" GroupingText="Collateral Details"
                                                                        Visible="false">
                                                                        <div id="div81" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                            <br />
                                                                            <asp:GridView ID="gvCollateralDetails" runat="server" AutoGenerateColumns="false"
                                                                                Width="98%" Caption="Collateral reference">
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                                    <asp:BoundField DataField="Collateralreference" HeaderText="Collateral reference" />
                                                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                                                    <asp:BoundField DataField="Collateralvalue" HeaderText="Collateral value" />
                                                                                    <asp:BoundField DataField="viewcollateral" HeaderText="View Collateral" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                            <br />
                                                                        </div>
                                                                    </asp:Panel>
                                                                    <asp:Panel runat="server" ID="pnlInvoiceDetails" CssClass="stylePanel" GroupingText="Invoice Details">
                                                                        <div id="div9" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                            <br />
                                                                            <asp:GridView ID="gvInvoiceDetails" runat="server" AutoGenerateColumns="false" Width="98%"
                                                                                OnRowDataBound="gvInvoiceDetails_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Invoice Transaction reference" ItemStyle-HorizontalAlign="Right"
                                                                                        Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblInvoiceReferNo" runat="server" Text='<%#Bind("Invoice_Id") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="Doc_Invoice_No" HeaderText="Invoice Reference" ItemStyle-Width="15%" />
                                                                                    <asp:BoundField DataField="Invoice_No" HeaderText="Invoice No" />
                                                                                    <asp:BoundField DataField="Vendor" HeaderText="Vendor Name" />
                                                                                    <asp:TemplateField HeaderText="Invoice Details">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lbtnViewInvoice" CausesValidation="false" runat="server" Text="View"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                            <br />
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            
</ContentTemplate>
                                        


</cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabAlerts" CssClass="tabpan" Width="80%" BackColor="Red">
                                            <HeaderTemplate>
                                                Alerts
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="upAlert" runat="server">
                                                    <ContentTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <div id="div2" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                        <br />
                                                                        <asp:GridView ID="gvAlert" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                            OnRowDataBound="gvAlert_RowDataBound" OnRowDeleting="gvAlert_RowDeleting" Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="TypeId" Visible="False" ItemStyle-Width="15%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTypeId" runat="server" Text='<%# Bind("TypeId") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Type" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblType" runat="server" Text='<%# Bind("Type") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:DropDownList ID="ddlType_AlertTab" runat="server" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rfvAlertType" runat="server" ControlToValidate="ddlType_AlertTab"
                                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Alert" InitialValue="-1"
                                                                                            SetFocusOnError="True" ErrorMessage="Select a Type"></asp:RequiredFieldValidator>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="User ContactId" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblUserContactid" runat="server" Text='<%# Bind("UserContactId") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="User Contact">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblUserContact" runat="server" Text='<%# Bind("UserContact") %>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <uc2:Suggest ID="ddlContact_AlertTab" runat="server" Width="300px" ServiceMethod="GetSalePersonList"
                                                                                            ErrorMessage="Select a User Contact" IsMandatory="true" ValidationGroup="Alert" />
                                                                                        <%--  <asp:DropDownList ID="ddlContact_AlertTab" runat="server" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rfvUserContact" runat="server" ControlToValidate="ddlContact_AlertTab"
                                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Select a User Contact"
                                                                                            InitialValue="-1" ValidationGroup="Alert"></asp:RequiredFieldValidator>--%>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="EMail">
                                                                                    <ItemTemplate>
                                                                                        <%--'<%# Bind("EMail") %>'--%>
                                                                                        <asp:CheckBox ID="ChkEmail" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "EMail")))%>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:CheckBox ID="ChkEmail" runat="server"></asp:CheckBox>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="SMS">
                                                                                    <ItemTemplate>
                                                                                        <%--'<%# Bind("SMS") %>'--%>
                                                                                        <asp:CheckBox ID="ChkSMS" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "SMS")))%>' />
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:CheckBox ID="ChkSMS" runat="server"></asp:CheckBox>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField FooterStyle-Width="10%">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                            Text="Remove"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Button ID="btnAddAlert" runat="server" Text="Add" CausesValidation="true" CssClass="styleGridShortButton"
                                                                                            OnClick="btnAddAlert_OnClick" ValidationGroup="Alert"></asp:Button>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <br />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            
</ContentTemplate>
                                        


</cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabFollowUp" CssClass="tabpan" BackColor="Red" Width="100%">
                                            <HeaderTemplate>
                                                Follow Up
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="upFollowup" runat="server">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="0" width="95%">
                                                            <tr>
                                                                <td class="styleFieldLabel" width="25%">
                                                                    <asp:Label runat="server" ID="lblLOB_Followup" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign" width="25%">
                                                                    <asp:TextBox ID="txtLOB_Followup" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel" width="20%">
                                                                    <asp:Label runat="server" ID="lblBranch_Followup" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign" width="25%">
                                                                    <asp:TextBox ID="txtBranch_Followup" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblEnquiry_Followup" CssClass="styleDisplayLabel" Text="Enquiry Number"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtEnquiry_Followup" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblEnquiryDate_Followup" CssClass="styleDisplayLabel"
                                                                        Text="Date"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtEnquiryDate_Followup" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblCustNameAdd_Followup" CssClass="styleDisplayLabel"
                                                                        Text="Prospect Name & Address"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtCustNameAdd_Followup" runat="server" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel" valign="top">
                                                                    <asp:Label ID="lblOfferNo_Followup" runat="server" CssClass="styleDisplayLabel" Text="Offer Number"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtOfferNo_Followup" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblApplication_Followup" runat="server" CssClass="styleDisplayLabel"
                                                                        Text="Application Number"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign" colspan="2">
                                                                    <asp:TextBox ID="txtApplication_Followup" runat="server" ReadOnly="True"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Panel runat="server" ID="Panel7" CssClass="stylePanel" GroupingText="Followup details"
                                                                        Width="100%">
                                                                        <div id="DivFollow" runat="server" style="width: 770px; overflow: scroll;">
                                                                            <br />
                                                                            <asp:GridView ID="gvFollowUp" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                                OnRowDeleting="gvFollowUp_RowDeleting" OnRowCreated="gvFollowUp_RowCreated" Width="1024px" Style="overflow: scroll">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDate_GridFollowup" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtDate_GridFollowup" runat="server" Width="90px">
                                                                                            </asp:TextBox>
                                                                                            <cc1:CalendarExtender runat="server" TargetControlID="txtDate_GridFollowup" ID="CalendarExtenderSD_FollowupDate"
                                                                                                Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                                            </cc1:CalendarExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvtxtDate_GridFollowup" runat="server" ControlToValidate="txtDate_GridFollowup"
                                                                                                ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                                ErrorMessage="Select the Date"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="From User" Visible="false" ItemStyle-Width="0%" FooterStyle-Width="0%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblfrom_GridFollowup_ID" runat="server" Text='<% #Bind("FromUserId")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="From UserName">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblfrom_GridFollowup" runat="server" Text='<% #Bind("From")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <uc2:Suggest ID="ddlfrom_GridFollowup" runat="server" Width="180px" ServiceMethod="GetSalePersonList"
                                                                                                ErrorMessage="Select a From UserName" IsMandatory="true" ValidationGroup="FollowUp" />
                                                                                            <%-- <asp:DropDownList ID="ddlfrom_GridFollowup" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvFromUser" runat="server" ControlToValidate="ddlfrom_GridFollowup"
                                                                                    InitialValue="-1" ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None"
                                                                                    SetFocusOnError="True" ErrorMessage="Select a From UserName"></asp:RequiredFieldValidator>--%>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="To User" Visible="false" ItemStyle-Width="0%" FooterStyle-Width="0%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTo_GridFollowupid" runat="server" Text='<%#Bind("ToUserId")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="To UserName" ItemStyle-Width="17%" FooterStyle-Width="17%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblTo_GridFollowup" runat="server" Text='<%#Bind("To")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <%-- <asp:DropDownList ID="ddlTo_GridFollowup" runat="server">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvToUser" runat="server" ControlToValidate="ddlTo_GridFollowup"
                                                                                    ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                    InitialValue="-1" ErrorMessage="Select a To UserName"></asp:RequiredFieldValidator>--%>
                                                                                            <uc2:Suggest ID="ddlTo_GridFollowup" runat="server" Width="180px" ServiceMethod="GetSalePersonList"
                                                                                                ErrorMessage="Select a To UserName" IsMandatory="true" ValidationGroup="FollowUp" />
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Action Details">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblActionDetails" runat="server" Text='<%#Bind("Action")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtAction_GridFollowup" runat="server" MaxLength="80" TextMode="MultiLine"
                                                                                                onkeyup="maxlengthfortxt(80)">
                                                                                            </asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="rfvtxtAction_GridFollowup" runat="server" ControlToValidate="txtAction_GridFollowup"
                                                                                                ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                                ErrorMessage="Enter an Action Details"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Action Date">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblActionDate" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ActionDate")).ToString(strDateFormat) %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtActionDate_GridFollowup" runat="server" Width="90px">
                                                                                            </asp:TextBox>
                                                                                            <cc1:CalendarExtender runat="server" TargetControlID="txtActionDate_GridFollowup"
                                                                                                ID="CalendarExtenderSD_FollowupActionDate" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                                            </cc1:CalendarExtender>
                                                                                            <asp:RequiredFieldValidator ID="rfvtxtActionDate_GridFollowup" runat="server" ControlToValidate="txtActionDate_GridFollowup"
                                                                                                ValidationGroup="FollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                                ErrorMessage="Enter an Action Date"></asp:RequiredFieldValidator>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Customer Response">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCustomerResponse" runat="server" Text='<%#Bind("CustomerResponse")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtCustomerResponse_GridFollowup" runat="server" MaxLength="80"
                                                                                                TextMode="MultiLine" onkeyup="maxlengthfortxt(80)">
                                                                                            </asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Remarks">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%#Bind("Remarks")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:TextBox ID="txtRemarks_GridFollowup" runat="server" MaxLength="80" TextMode="MultiLine"
                                                                                                onkeyup="maxlengthfortxt(80)">
                                                                                            </asp:TextBox>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                                Text="Remove"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <asp:Button ID="btnAddFollowup" runat="server" Text="Add" CausesValidation="true"
                                                                                                CssClass="styleGridShortButton" OnClick="btnAddFollowUp_OnClick" ValidationGroup="FollowUp"></asp:Button>
                                                                                        </FooterTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                            <br />
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            
</ContentTemplate>
                                        


</cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabMLA_SLA" CssClass="tabpan" BackColor="Red" Width="70%">
                                            <HeaderTemplate>
                                                Prime Account Details
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="upPrimeAccount" runat="server">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="0" width="98%">
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblDoyouWant" CssClass="styleReqFieldLabel" Text="Do you want this application to <br />base as Prime Account Number"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:DropDownList ID="ddlDoyouWant_MLA" runat="server">
                                                                        <asp:ListItem Value="1" Text="Yes" Selected="True"></asp:ListItem>
                                                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblPassword" CssClass="styleReqFieldLabel" Text="Password"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="15" TextMode="Password"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ControlToValidate="txtPassword"
                                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter the Password in PrimeAccount Details"
                                                                        ValidationGroup="Prime Account Details"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblMLANo" CssClass="styleDisplayLabel" Text="Prime Account Number"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtMLANo" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblMLAFinanceAmount" CssClass="styleDisplayLabel" Text="Finance Amount"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtMLAFinanceAmount" runat="server" Style="text-align: right" Width="80px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblValidFrom_MLA" CssClass="styleDisplayLabel" Text="Validity From"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtValidFrom_MLA" runat="server" Width="100px"></asp:TextBox>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label runat="server" ID="lblValidTo_MLA" CssClass="styleReqFieldLabel" Text="Validity To"></asp:Label>
                                                                </td>
                                                                <td class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtValidTo_MLA" runat="server" Width="100px"></asp:TextBox>
                                                                    <asp:Image ID="imgEndDateValidTo_MLA" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                    <cc1:CalendarExtender ID="CalendarExtenderED" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_PrevSystemDate"
                                                                        PopupButtonID="imgEndDateValidTo_MLA" TargetControlID="txtValidTo_MLA">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtValidTo_MLA" runat="server" ControlToValidate="txtValidTo_MLA"
                                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Choose the Validity To"
                                                                        ValidationGroup="Prime Account Details"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <table cellpadding="0" cellspacing="0" width="98%" border="0">
                                                            <tr align="center">
                                                                <td align="center" width="50%">
                                                                    <asp:Label runat="server" ID="lblMLAROI" CssClass="styleDisplayLabel" Text="ROI Rule"></asp:Label>
                                                                    <asp:TextBox ID="txtROIMLA" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                                <td align="center" width="50%">
                                                                    <asp:Label runat="server" ID="lblMLAPaymentCard" CssClass="styleDisplayLabel" Text="Payment Rule"></asp:Label>
                                                                    <asp:TextBox ID="txtPaymentCardMLA" runat="server" ReadOnly="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <table width="99%" border="0" class="styleContentTable">
                                                            <tr>
                                                                <td valign="top" width="50%">
                                                                    <div style="height: 125px; overflow-x: hidden; overflow-y: auto; width: 100%">
                                                                        <div style="height: 125px;">
                                                                            <%--<asp:Panel ID="Panel3" runat="server" ScrollBars="Auto" Height="125px" Width="98%">--%>
                                                                            <asp:GridView ID="gv_MLAROI" runat="server" Width="100%">
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <%-- </asp:Panel>--%>
                                                                </td>
                                                                <td valign="top" width="50%">
                                                                    <div style="height: 125px; overflow-x: hidden; overflow-y: auto; width: 100%">
                                                                        <div style="height: 125px;">
                                                                            <%--<asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Height="125px" Width="98%">--%>
                                                                            <asp:GridView ID="gv_MLARepayRuleCard" runat="server" Width="100%">
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                    <%-- </asp:Panel>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        <asp:Panel ID="pnlMLADetails" runat="server" Width="99%" CssClass="stylePanel" GroupingText="Account Details">
                                                            <div id="div15" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                <br />
                                                                <asp:GridView ID="gvMLADetails" runat="server" Width="100%" AutoGenerateColumns="true">
                                                                </asp:GridView>
                                                                <br />
                                                            </div>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            
</ContentTemplate>
                                        


</cc1:TabPanel>
                                        <cc1:TabPanel runat="server" ID="TabMoratorium" CssClass="tabpan" BackColor="Red"
                                            Width="65%">
                                            <HeaderTemplate>
                                                Moratorium
                                            
</HeaderTemplate>
                                            


<ContentTemplate>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div id="div10" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server">
                                                                        <br />
                                                                        <asp:GridView ID="gvMoratorium" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                            OnRowDeleting="gvMoratorium_RowDeleting" Width="100%" HorizontalAlign="Center">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Moratorium type" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="ddlMoratoriumtype_MoratoriumTab" runat="server" Text='<%#Bind("Moratorium") %>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:DropDownList ID="ddlMoratoriumtype_MoratoriumTab" runat="server" Width="100%">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rfvddlMoratoriumtype_MoratoriumTab" runat="server"
                                                                                            ControlToValidate="ddlMoratoriumtype_MoratoriumTab" CssClass="styleMandatoryLabel"
                                                                                            Display="None" SetFocusOnError="True" ErrorMessage="Select the Moratorium type"
                                                                                            InitialValue="-1" ValidationGroup="Moratorium"></asp:RequiredFieldValidator>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="From date" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="txtFromdate_MoratoriumTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Fromdate")).ToString(strDateFormat) %>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtFromdate_MoratoriumTab" runat="server" ValidationGroup="TabMoratorium"
                                                                                            Width="90%">
                                                                                        </asp:TextBox>
                                                                                        <cc1:CalendarExtender ID="CalendarExtenderSD_FromDate_MoratoriumTab" runat="server"
                                                                                            Enabled="True" OnClientDateSelectionChanged="checkDate_PrevSystemDate" TargetControlID="txtFromdate_MoratoriumTab">
                                                                                        </cc1:CalendarExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtFromdate_MoratoriumTab" runat="server" ControlToValidate="txtFromdate_MoratoriumTab"
                                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter the From date"
                                                                                            ValidationGroup="Moratorium"></asp:RequiredFieldValidator>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="To date" ItemStyle-Width="20%" FooterStyle-Width="20%">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="txtTodate_MoratoriumTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Todate")).ToString(strDateFormat) %>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:TextBox ID="txtTodate_MoratoriumTab" runat="server" ValidationGroup="TabMoratorium"
                                                                                            Width="90%">
                                                                                        </asp:TextBox>
                                                                                        <cc1:CalendarExtender ID="CalendarExtenderSD_ToDate_MoratoriumTab" runat="server"
                                                                                            Enabled="True" OnClientDateSelectionChanged="checkDate_PrevSystemDate" TargetControlID="txtTodate_MoratoriumTab">
                                                                                        </cc1:CalendarExtender>
                                                                                        <asp:RequiredFieldValidator ID="rfvtxtTodate_MoratoriumTab" runat="server" ControlToValidate="txtTodate_MoratoriumTab"
                                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter the To date"
                                                                                            ValidationGroup="Moratorium"></asp:RequiredFieldValidator>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="Noofdays" HeaderText="No.of days" ItemStyle-Width="10%"
                                                                                    ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:TemplateField HeaderText="" ItemStyle-Width="10%" FooterStyle-Width="10%" FooterStyle-HorizontalAlign="Center"
                                                                                    ItemStyle-HorizontalAlign="Center">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnRemove" CausesValidation="false" OnClientClick="return fnAddMoratorium()"
                                                                                            runat="server" CommandName="Delete" Text="Remove"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <FooterTemplate>
                                                                                        <asp:Button ID="LbtnAddMoratorium" runat="server" CausesValidation="true" OnClientClick="return fnAddMoratorium()"
                                                                                            OnClick="btnAddMoratorium_OnClick" Text="Add" ValidationGroup="Moratorium" CssClass="styleGridShortButton"></asp:Button>
                                                                                    </FooterTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <br />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
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
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <input id="hdnBON" type="hidden" value="0" runat="server" />
    <br />
    <div id="btndiv" style="overflow: auto; text-align: center">
        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
            OnClick="btnSave_OnClick" CausesValidation="true" OnClientClick="return fnSaveValidation()" />
        <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
            UseSubmitBehavior="False" Text="Clear" OnClick="btnClear_OnClick" />
        <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
            Enabled="True" TargetControlID="btnClear">
        </cc1:ConfirmButtonExtender>
        <asp:Button ID="btnCalcel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
            UseSubmitBehavior="False" Text="Cancel" OnClick="btnCancel_Click" />
        <asp:Button ID="btnApplicationCancel" runat="server" CausesValidation="False" CssClass="styleSubmitLongButton"
            Visible="false" Text="Application Cancel" OnClientClick="return Confirmmsg('Do you want to cancel the Application?'); "
            OnClick="btnApplicationCancel_Click" />
    </div>
    <br />
    <asp:ValidationSummary ID="vsApplicationProcessing" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" Width="98%" ShowMessageBox="false" ValidationGroup="Main Page"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="TabRepayment1" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="TabRepayment1" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsAsset" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="tcAsset" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:ValidationSummary ID="vsInflow" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Inflow" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:ValidationSummary ID="vsOutflow" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Outflow" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:ValidationSummary ID="vsRepayment" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="GridRepayment" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsGuarantor" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Guarantor" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsAlert" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Alert" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:ValidationSummary ID="vsFollowUp" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="FollowUp" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsMoratorium" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Moratorium" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:CustomValidator ID="cvApplicationProcessing" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" Width="98%" />
    <asp:HiddenField ID="hdnEnquiryNo" runat="server" />
    <asp:HiddenField ID="Guarantee" runat="server" />

    <script language="javascript" type="text/javascript">
        var tab;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endRequest);
        var postbackElement;

        function initializeRequest(sender, args) {
            document.body.style.cursor = "wait";
            if (prm.get_isInAsyncPostBack()) {
                //debugger
                args.set_cancel(true);
            }
        }
        function endRequest(sender, args) {
            document.body.style.cursor = "default";
            
        }


        function pageLoad() {

            tab = $find('ctl00_ContentPlaceHolder1_TabContainerAP');
            var querymode = location.search.split("qsMode=");
            var queryupload = location.search.split("AsyncFileUploadID=");
            if (querymode.length > 1) {
                if (querymode[1].length > 1) {
                    querymode = querymode[1].split("&");
                    querymode = querymode[0];
                }
                else {
                    querymode = querymode[1];
                }
                if (querymode != 'Q' && tab != null) {
                    tab.add_activeTabChanged(on_Change);
                    var newindex = tab.get_activeTabIndex(index);
                    var btnSave = document.getElementById('<%=btnSave.ClientID %>')
                    var btnclear = document.getElementById('<%=btnClear.ClientID %>')
                    if (newindex == tab._tabs.length - 1)
                        btnSave.disabled = false;
                    else
                        btnSave.disabled = true;
                }
            }

        }
        function SetData(ObJ, data) {

            if ((document.getElementById(ObJ).value == "___.__") || (document.getElementById(ObJ).value == "")) {
                document.getElementById(ObJ).value = data;
            }
        }

        function fnAssignBranch(ddlBranchList) {
            var varBranch = ddlBranchList.selectedIndex;
            var txtFollowupBranch = document.getElementById('ctl00_ContentPlaceHolder1_TabContainerAP_TabFollowUp_txtBranch_Followup');
            if (txtFollowupBranch != null) {
                txtFollowupBranch.value = ddlBranchList.options[varBranch].innerText;
            }
        }


        function FunRestIrr() {
            document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value = "";
        }


        function fnValidateEMail(ObJ1, ObJ2) {
            var email = document.getElementById(ObJ1).checked;
            var sms = document.getElementById(ObJ2).checked;
            if (email == false && sms == false) {
                alert('Select EMail or SMS');
                return false;
            }
            else
                return true;
        }

        function fnAllowNumbersOnlyZero(isSpaceAllowed, Obj1) {
            var sKeyCode = window.event.keyCode;
            var anyval = document.getElementById(Obj1).value;

            //alert(sKeyCode);
            if ((!isSpaceAllowed) && (sKeyCode == 32)) {
                window.event.keyCode = 0;
                return false;
            }
            if ((sKeyCode < 48 || sKeyCode > 57) && (sKeyCode != 32 && sKeyCode != 95) || (sKeyCode == 48)) {
                if (anyval == 0) {
                    window.event.keyCode = 0;
                    return false;
                }
            }
        }
        var index = 0;
        function on_Change(sender, e) {
           
            var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var Valgrp = document.getElementById('<%=vsApplicationProcessing.ClientID %>')
            Valgrp.validationGroup = strValgrp;
            var newindex = tab.get_activeTabIndex(index);
            var btnSave = document.getElementById('<%=btnSave.ClientID %>')
            var btnclear = document.getElementById('<%=btnClear.ClientID %>')

            if (newindex == tab._tabs.length - 1)
                btnSave.disabled = false;
            else
                btnSave.disabled = true;
            var txtStatus = document.getElementById('<%=txtStatus.ClientID %>');
            if (txtStatus != null) {
                if (txtStatus.value.toLowerCase() == 'approved' || txtStatus.value.toLowerCase() == 'rejected' || txtStatus.value.toLowerCase() == 'cancelled') {
                    btnSave.disabled = true;
                }
            }
            var txtMLAFinanceAmount = document.getElementById('<%=txtMLAFinanceAmount.ClientID %>');
            var txtFinanceAmount = document.getElementById('<%=txtFinanceAmount.ClientID %>');
            if (txtMLAFinanceAmount != null && txtFinanceAmount != null) {
                txtMLAFinanceAmount.value = txtFinanceAmount.value;
            }



            if (newindex > index) {
                if (!fnCheckPageValidators(strValgrp, false)) {
                    tab.set_activeTabIndex(index);

                }
                else {
                    var lobcode = FunGetSelectedLob();
                    var IsAssetAvail;
                    var IsCheckAssetAvail;
                    switch (lobcode.toLowerCase()) {
                        case "te": //Term loan Extensible
                        case "tl": //Term loan
                        case "ft": //Factoring 
                        case "wc": //Working Capital
                            IsAssetAvail = "No";
                            break;
                        default:
                            IsAssetAvail = "Yes";
                            break;
                    }
                    switch (lobcode.toLowerCase()) {
                        case "te": //Term loan Extensible
                        case "tl": //Term loan
                        case "ft": //Factoring 
                        case "wc": //Working Capital
                        case "ln": //Working Capital
                            IsCheckAssetAvail = false;
                            break;
                        default:
                            IsCheckAssetAvail = true;
                            break;
                    }

                    switch (index) {

                        case 0:
                            if (IsAssetAvail == "No") {
                                if (IsCheckAssetAvail) {
                                    if (FunCheckGridIsEmpty('<%=gvAssetDetails.ClientID %>', 'No') ) {
                                        index = tab.get_activeTabIndex(index);

                                    }
                                    else {
                                        tab.set_activeTabIndex(index);
                                        alert('Add atleast one asset details');
                                    }
                                }
                                else {
                                    index = tab.get_activeTabIndex(index);
                                }
                            }
                            else {
                                if (IsCheckAssetAvail) {
                                    if (FunCheckGridIsEmpty('<%=gvAssetDetails.ClientID %>', 'No') ) {
                                        index = tab.get_activeTabIndex(index);
                                    }
                                    else {
                                        tab.set_activeTabIndex(index);
                                        alert('Add atleast one asset details');
                                    }
                                }
                                else {
                                    index = tab.get_activeTabIndex(index);
                                }
                            }


                            break;
                        case 1:

                            if (FunCheckGridIsEmpty('<%=gvOutFlow.ClientID %>', 'Yes')) {
                                if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                    document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                }
                                index = tab.get_activeTabIndex(index);

                            }
                            else {
                                if (lobcode.toLowerCase() != "ol") {
                                    tab.set_activeTabIndex(index);
                                    alert('Add atleast one Outflow details');
                                }
                                else {
                                    if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                        document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                    }
                                    index = tab.get_activeTabIndex(index);
                                }
                            }

                            break;
                        case 2:
                            //added by saranya
                            //                           if((lobcode.toLowerCase() != "te") && (document.getElementById('<%=ddl_Repayment_Mode.ClientID %>').value !="5"))
                            //                            {
                            if (FunCheckGridIsEmpty('<%=gvRepaymentSummary.ClientID %>', 'No')) {
                                index = tab.get_activeTabIndex(index);
                            }
                            else {
                                tab.set_activeTabIndex(index);
                                alert('Add atleast one Repayment details');
                            }
                            //                                }


                            break;
                        case 4:
                            index = tab.get_activeTabIndex(index);

                            break;
                        case 5:
                            index = tab.get_activeTabIndex(index);

                            break;
                        default:
                            index = tab.get_activeTabIndex(index);
                            break;

                    }


                }
            }
            else {
                tab.set_activeTabIndex(newindex);
                index = tab.get_activeTabIndex(newindex);





            }

        }
        function FunGetSelectedLob() {
            var ddlLob = document.getElementById('<%=ddlLOB.ClientID %>');
            return ddlLob.item(ddlLob.selectedIndex).text.split('-')[0].trim();
        }
        function FunCheckGridIsEmpty(gridview, isfooterexists) {
            if (document.getElementById(gridview) == null) {
                return false;
            }
            var table = document.getElementById(gridview);
            var rows = table.getElementsByTagName("tr");
            if (isfooterexists == 'No') {
                if (rows.length > 1) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                if (rows.length > 2) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function FunChkAllFooterValues(gridid) {
            //Get all the control of the type INPUT in the base control.
            var Combos = gridid.getElementsByTagName("select");
            for (var n = 0; n < Combos.length; ++n) {
                if (Combos[n].innerText == "") {
                    return false;
                }
            }
        }
        function FunddlRoiOnChange(varRoiPayment) {
            var Prevval;
            var Dropdown;
            if (varRoiPayment == 'Payment') {
                Prevval = document.getElementById('<%=hdnPayment.ClientID %>').value;
                Dropdown = document.getElementById('<%=ddlPaymentRuleList.ClientID %>')
            }
            else {
                Prevval = document.getElementById('<%=hdnROIRule.ClientID %>').value;
                Dropdown = document.getElementById('<%=ddlROIRuleList.ClientID %>')
            }
            if (Prevval != "") // IF Change the Existing value
            {
                if (Prevval != Dropdown.value.trim()) {
                    if (confirm('All cashflow related details will be reset. Do you want to continue?')) {
                        return true;
                    }
                    else {
                        Dropdown.value = Prevval;
                        return false;
                    }
                }
                else {
                    return false; // If value not changed.
                }
            }
            else // At First Time
            {
                if (Dropdown.value.trim() > 0) {
                    return true;
                }
                else // Without selecting the value
                {
                    return false;
                }
            }
        }
        function fnLoadCustomer() {
            var VartxtCustomerCode = document.getElementById('<%=txtCustomerCode.ClientID %>').value;
            if (VartxtCustomerCode != null) {
                if (document.getElementById('ctl00_ContentPlaceHolder1_TabContainerAP_TabMainPage_ucCustomerCodeLov_hdnID').value.trim() != "") {
                    document.getElementById('ctl00_ContentPlaceHolder1_TabContainerAP_TabMainPage_btnLoadCustomer').click(); 
                }
            }
        }
        function fnLoadCustomerg() {
            document.getElementById('ctl00_ContentPlaceHolder1_TabContainerAP_TabMainPage_btnLoadCustomer').click();
        }
        function checkApplicationDate(sender, args) {
            var varApplicationDate = document.getElementById('<%=txtDate.ClientID %>').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var selectedDate = sender._selectedDate;
            var today = new Date();
            var varOfferDate = document.getElementById('<%=txtOfferDate.ClientID %>');
            if (varOfferDate != null) {
                var varOfferDateValue = document.getElementById('<%=txtOfferDate.ClientID %>').value;
                if (varOfferDateValue != "") {
                    var varOffdate = Date.parseInvariant(varOfferDateValue, sender._format);
                    if (selectedDate < varOffdate) {
                        alert('InflowDate should be greater than or equal to Offer Date');
                        sender._textbox.set_Value(today.format(sender._format));
                    }
                    else {
                        sender._textbox.set_Value(selectedDate.format(sender._format));

                    }
                }
                else {
                    if (selectedDate < varapplndate) {
                        alert('InflowDate should be greater than or equal to Application Date');
                        sender._textbox.set_Value(today.format(sender._format));
                    }
                    else {
                        sender._textbox.set_Value(selectedDate.format(sender._format));
                    }
                }
            }

        }
        function uploadComplete(sender, args) {

            var objID = sender._inputFile.id.split("_");
            objID = "<%= gvPRDDT.ClientID %>" + "_" + objID[5];
            if (document.getElementById(objID + "_myThrobber") != null) {
                document.getElementById(objID + "_myThrobber").innerText = args._fileName;
                document.getElementById(objID + "_hidThrobber").value = args._fileName;
            }
        }

        function fnSaveValidation() {
            if (!fnCheckPageValidators("Offer Terms", false)) {
                alert("Fill the Mandatory value in Offer Terms tab");
                tab.set_activeTabIndex(1);

                var a = event.srcElement;
                //a.style.display = 'block';
                a.style.removeAttribute('display');

                return false;
            }

            var lobcode = FunGetSelectedLob();
            var Repayment_Mode = document.getElementById('<%=ddl_Repayment_Mode.ClientID %>').value;

            switch (lobcode.toLowerCase()) {
                case "tl": //Term loan
                    if (Repayment_Mode != "5") {
                        if (!fnCheckPageValidators("Repayment Details", false)) {
                            alert("Fill the Mandatory value in Repayment tab");
                            tab.set_activeTabIndex(2);

                            var a = event.srcElement;
                            //a.style.display = 'block';
                            a.style.removeAttribute('display');

                            return false;
                        }
                    }
                    break;
                case "ft": //Factoring 
                    if (Repayment_Mode != "5") {
                        if (!fnCheckPageValidators("Repayment Details", false)) {
                            alert("Fill the Mandatory value in Repayment tab");
                            tab.set_activeTabIndex(2);

                            var a = event.srcElement;
                            //a.style.display = 'block';
                            a.style.removeAttribute('display');

                            return false;
                        }
                    }
                    break;
                case "pl": //Factoring 
                   
                    break; 
                default:
                    if (!fnCheckPageValidators("Repayment Details", false)) {
                        alert("Fill the Mandatory value in Repayment tab");
                        tab.set_activeTabIndex(2);

                        var a = event.srcElement;
                        //a.style.display = 'block';
                        a.style.removeAttribute('display');

                        return false;
                    }
                    break;
            }


            if (!fnCheckPageValidators("Prime Account Details", false)) {
                alert("Fill the Mandatory value in Prime Account Details tab");
                tab.set_activeTabIndex(6);

                var a = event.srcElement;
                //a.style.display = 'block';
                a.style.removeAttribute('display');

                return false;
            }
            var a = event.srcElement;
            a.style.removeAttribute('display');
            return fnCheckPageValidators('Application');
        }

        function fnAddMoratorium() {
            var repaymentMode = document.getElementById('ctl00_ContentPlaceHolder1_TabContainerAP_TabOfferTerms_ddl_Repayment_Mode');
            if (repaymentMode.value == "2") {
                if (confirm('Repayment Schedule & IRR will be reset, Do you want to continue?')) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
    </script>

</asp:Content>
