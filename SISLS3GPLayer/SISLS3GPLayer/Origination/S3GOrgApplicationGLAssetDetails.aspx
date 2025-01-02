<%@ Page Title="Asset Details" Language="C#" MasterPageFile="~/Common/MasterPage.master"
    AutoEventWireup="true" CodeFile="S3GOrgApplicationGLAssetDetails.aspx.cs" Inherits="Origination_S3G_OrgApplicationGLAssetDetails" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <base target="_self" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Application Process - Asset details">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" GroupingText="Application Asset Details"
                            Height="75%" Width="99%">
                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblSlNo" runat="server" CssClass="styleDisplayLabel" Text="Sl. No."></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtSlNo" runat="server" Width="20px" ReadOnly="true" TabIndex="-1"
                                            Style="text-align: right" ToolTip="Sl. No."></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblAssetCodeList" CssClass="styleReqFieldLabel" Text="Asset"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" colspan="3">
                                        <asp:DropDownList ID="ddlAssetCodeList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlAssetCodeList_SelectedIndexChanged"
                                            TabIndex="1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvAssetCodeList" runat="server" ControlToValidate="ddlAssetCodeList"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            InitialValue="0" SetFocusOnError="True" ErrorMessage="Select an Asset Code"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblPurity" CssClass="styleReqFieldLabel" Text="Purity"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlPurity" runat="server" AutoPostBack="True" ToolTip="Purity"
                                            OnSelectedIndexChanged="ddlPurity_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlPurity" runat="server" ControlToValidate="ddlPurity"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            InitialValue="0" SetFocusOnError="True" ErrorMessage="Select the Purity"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblMeasurement" runat="server" CssClass="styleReqFieldLabel" Text="Measurement Unit"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtMeasurement" runat="server" Width="80px" ReadOnly="true" TabIndex="-1"
                                            ToolTip="Measurement"></asp:TextBox>
                                        <asp:Label ID="lblMeasurementID" runat="server" Visible="false" Text="1"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblUnitCount" CssClass="styleReqFieldLabel" Text="Unit Count"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtUnitCount" Text="1" runat="server" Width="35px"
                                            MaxLength="4" Style="text-align: right"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUnitCount" runat="server" ControlToValidate="txtUnitCount"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            SetFocusOnError="True" ErrorMessage="Enter the Unit count"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredUnitCount" TargetControlID="txtUnitCount"
                                            FilterType="Numbers" runat="server">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RangeValidator ID="RangeVUnitCount" runat="server" SetFocusOnError="True" CssClass="styleMandatoryLabel"
                                            Display="None" ValidationGroup="TabAssetDetails" ErrorMessage="Unit Count cannot be Zero"
                                            ControlToValidate="txtUnitCount" MaximumValue="9999" MinimumValue="1">
                                        </asp:RangeValidator>
                                        <input id="HdnGPSDecimal" type="hidden" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblValuationDate" runat="server" CssClass="styleDisplayLabel" Text="Valuation Date & Tme"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtValuationDate" runat="server" Width="80px" ReadOnly="true" TabIndex="-1"
                                            ToolTip="Valuation Date & Tme"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblMarketValue" runat="server" CssClass="styleDisplayLabel" Text="Market Value"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtMarketValue" runat="server" Width="80px" ReadOnly="true" TabIndex="-1"
                                                        Style="text-align: right" ToolTip="Market Value"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMarketValUnit" runat="server" Width="40" ReadOnly="true" TabIndex="-1"
                                                        Style="text-align: right; border-left-width: 0px; margin-left: 0px" ToolTip="Market Value"
                                                        Text=" /Gram"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblGrossWeight" runat="server" CssClass="styleReqFieldLabel" Text="Gross Weight"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtGrossWeight" runat="server" MaxLength="10" Style="text-align: right"
                                            Width="80px" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvGrossWeight" runat="server" ControlToValidate="txtGrossWeight"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Gross Weight"
                                            SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNetWeight" runat="server" CssClass="styleReqFieldLabel" Text="Net Weight"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtNetWeight" runat="server" MaxLength="10" AutoPostBack="true"
                                            Style="text-align: right" Width="80px" 
                                            onkeypress="fnAllowNumbersOnly(true,false,this)" 
                                            ontextchanged="txtNetWeight_TextChanged"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNetWeight"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Net Weight"
                                            SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblPureGoldMass" CssClass="styleDisplayLabel" Text="Pure Gold Mass (Grams)"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtPureGoldMass" runat="server" Style="text-align: right" Width="80px"
                                            ToolTip="Pure Gold Mass" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblPureGoldValue" CssClass="styleDisplayLabel" Text="Pure Gold Value"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtPureGoldValue" runat="server" Style="text-align: right" Width="80px"
                                            ToolTip="Pure Gold Value" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <br />
                                        <asp:Label ID="lblMaxLoan" runat="server" CssClass="styleReqFieldLabel" Style="font-weight: bold"
                                            Text="Maximum Loan"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <br />
                                        <asp:Label ID="lblMaxLoanValue" runat="server" Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtMaxLoan" runat="server" Style="text-align: right; font-weight: bold;
                                            border-style: double" Width="100px" AutoPostBack="true" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfcMaxLoan" runat="server" ControlToValidate="txtMaxLoan"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            SetFocusOnError="True" ErrorMessage="Enter the Manximum Loan"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <table width="100%">
                            <tr>
                                <td align="center" style="width: 100%;">
                                    <asp:Button ID="btnOK" ValidationGroup="TabAssetDetails" runat="server" CausesValidation="true"
                                        CssClass="styleSubmitButton" Text="OK" UseSubmitBehavior="False" OnClick="btnOK_Click" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Cancel" UseSubmitBehavior="False" ValidationGroup="TabAssetDetails" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="99%">
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vs_TabAssetDetails" runat="server" CssClass="styleMandatoryLabel"
                                        Width="705px" ValidationGroup="TabAssetDetails" HeaderText="Correct the following validation(s):  " />
                                    <asp:CustomValidator ID="cv_TabAssetDetails" runat="server" CssClass="styleMandatoryLabel"
                                        Display="None" ValidationGroup="TabAssetDetails"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrorMessage" runat="server" Style="color: Red; font-size: 14px">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cvApplicationAsset" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true" Width="98%" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <input type="hidden" id="hdnCustomerID" runat="server" />
    <input type="hidden" id="hdnAssetAvailDate" runat="server" />

    <script language="javascript" type="text/javascript">


        function ViewModal() {

            window.showModalDialog('../Origination/S3GOrgApplicationAssetDetails.aspx?qsMaster=Yes', 'Application Asset Details', 'dialogwidth:900px;dialogHeight:900px;');
        }
    </script>

</asp:Content>
