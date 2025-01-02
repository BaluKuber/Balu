<%@ Page Title="Asset Details" Language="C#" MasterPageFile="~/Common/MasterPage.master"
    AutoEventWireup="true" CodeFile="S3GLoanAdAccountAssetDetails.aspx.cs" Inherits="LoanAd_S3G_LoanAdAccountAssetDetails" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<base target="_self" />
    <script language="javascript" type="text/javascript">
        function funcalassetvalue() {
            if (document.getElementById('<%=txtUnitCount.ClientID %>').value != "" && document.getElementById('<%=txtUnitValue.ClientID %>').value != "") {
                document.getElementById('<%=txtTotalAssetValue.ClientID %>').value = (parseFloat(document.getElementById('<%=txtUnitCount.ClientID %>').value) * parseFloat(document.getElementById('<%=txtUnitValue.ClientID %>').value)).toFixed(2);
            }
        }

        function funIsEmpty(textBox) 
        {
            if (textBox.value == "")
            {
                textBox.value = "0";
            }
        }
        
        function ViewModal() {

            window.showModalDialog('../Origination/S3GOrgApplicationAssetDetails.aspx?qsMaster=Yes', 'Application Asset Details', 'dialogwidth:900px;dialogHeight:900px;');
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table width="99%">
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" GroupingText="Account Asset Details"
                            Height="75%" Width="99%">
                            <table width="100%">
                                <tr align="center">
                                    <td>
                                        <asp:RadioButtonList ID="rdnlAssetType" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="True" OnSelectedIndexChanged="rdnlAssetType_SelectedIndexChanged">
                                            <asp:ListItem Text="Lease New Purchase" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Lease Own Assets" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="99%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblSlNo" runat="server" CssClass="styleDisplayLabel" Text="Sl. No."></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtSlNo" runat="server" Width="20px" ReadOnly="true" TabIndex="-1"
                                            Style="text-align: right;"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRequiredFromDate" runat="server" CssClass="styleDisplayLabel" Text="Required From"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRequiredFromDate" runat="server" Width="100px" TabIndex="-1"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtRequiredFromDate_CalendarExtender" runat="server" Enabled="false"
                                            OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="ImgtxtRequiredFromDate"
                                            TargetControlID="txtRequiredFromDate">
                                        </cc1:CalendarExtender>
                                        <asp:Image ID="ImgtxtRequiredFromDate" runat="server" Height="16px" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="rfvRequiredFromDate" runat="server" ControlToValidate="txtRequiredFromDate"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Choose a Required from date"
                                            SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 20%">
                                        <asp:Label runat="server" ID="lblAssetCodeList" CssClass="styleReqFieldLabel" Text="Asset Code"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" colspan="3">
                                        <asp:DropDownList ID="ddlAssetCodeList" runat="server" TabIndex="1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvAssetCodeList" runat="server" ControlToValidate="ddlAssetCodeList"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            InitialValue="0" SetFocusOnError="True" ErrorMessage="Select an Asset Code" Enabled="False"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 20%">
                                        
                                        <asp:Label ID="lblLeaseAssetNo" runat="server" CssClass="styleReqFieldLabel" Text="Lease Asset No."
                                            ></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" colspan="3">
                                        
                                        <asp:DropDownList ID="ddlLeaseAssetNo" runat="server"  TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLeastAssetCodeNo" runat="server" ControlToValidate="ddlLeaseAssetNo"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select an Least Asset Number"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblUnitCount" CssClass="styleReqFieldLabel" Text="Unit Count"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtUnitCount" onchange="funcalassetvalue();" runat="server" Width="35px"
                                            MaxLength="4" TabIndex="3" Style="text-align: right;"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUnitCount" runat="server" ControlToValidate="txtUnitCount"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            SetFocusOnError="True" ErrorMessage="Enter the Unit count"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredUnitCount" TargetControlID="txtUnitCount"
                                            FilterType="Numbers,Custom" runat="server" ValidChars="." FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RangeValidator ID="RangeVUnitCount" runat="server" SetFocusOnError="True" CssClass="styleMandatoryLabel"
                                            Display="None" ValidationGroup="TabAssetDetails" ErrorMessage="Unit Count cannot be Zero"
                                            ControlToValidate="txtUnitCount" MaximumValue="9999" MinimumValue="1">
                                        </asp:RangeValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblUnitValue" runat="server" CssClass="styleReqFieldLabel" Text="Unit Value"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtUnitValue" runat="server" MaxLength="13" Style="text-align: right;"
                                            onchange="funcalassetvalue();" Width="100px" TabIndex="5" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUnitValue" runat="server" ControlToValidate="txtUnitValue"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Unit value"
                                            SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblTotalAssetValue" CssClass="styleDisplayLabel" Text="Total Asset Value"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtTotalAssetValue" Style="text-align: right;" runat="server" Width="150px"
                                            TabIndex="-1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblMarginPercentage" runat="server" CssClass="styleDisplayLabel" Text="Margin %"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtMarginPercentage" Style="text-align: right;" runat="server" Width="50px"
                                            TabIndex="6" MaxLength="6" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                        <asp:RangeValidator ID="rngvMarginPercentage" runat="server" ErrorMessage="Margin% should be between 1 and 100"
                                            ControlToValidate="txtMarginPercentage" MaximumValue="100" MinimumValue="0.0001"
                                            ValidationGroup="TabAssetDetails" Display="None" Type="Double">
                                        </asp:RangeValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblMarginAmountAsset" CssClass="styleDisplayLabel"
                                            Text="Margin Amount"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtMarginAmountAsset" Style="text-align: right;" runat="server"
                                            Width="100px" TabIndex="7" MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblBookDepreciationPerc" runat="server" CssClass="styleDisplayLabel"
                                            Text="Book Depreciation %"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtBookDepreciationPerc" Style="text-align: right;" runat="server"
                                            ReadOnly="True" Width="50px" TabIndex="-1"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblBlockDepreciationPerc" CssClass="styleDisplayLabel"
                                            Text="Block Depreciation %"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtBlockDepreciationPerc" Style="text-align: right;" runat="server"
                                            Width="50px" ReadOnly="True" TabIndex="-1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblFinanceAmountAsset0" runat="server" CssClass="styleReqFieldLabel"
                                            Text="Finance Amount"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtFinanceAmountAsset" Style="text-align: right;" runat="server"
                                            MaxLength="10" Width="100px" TabIndex="8"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFinanceAmountAsset" runat="server" ControlToValidate="txtFinanceAmountAsset"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Finance Amount"
                                            SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FEFinAmt" TargetControlID="txtFinanceAmountAsset"
                                            FilterType="Numbers,Custom" runat="server" ValidChars="." FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RangeValidator ID="RVFinAmt" runat="server" SetFocusOnError="True" CssClass="styleMandatoryLabel"
                                            Display="None" ValidationGroup="TabAssetDetails" ErrorMessage="Finance Amount cannot be Zero"
                                            ControlToValidate="txtFinanceAmountAsset" MaximumValue="9999999999" MinimumValue="1">
                                        </asp:RangeValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblCapitalPortion" CssClass="styleReqFieldLabel" Text="Capital Portion"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCapitalPortion" Style="text-align: right;" runat="server" MaxLength="10"
                                            TabIndex="9" Width="100px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCapitalPortion" runat="server" ControlToValidate="txtCapitalPortion"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            SetFocusOnError="True" ErrorMessage="Enter the Capital Portion">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredCapitalPortion" TargetControlID="txtCapitalPortion"
                                            FilterType="Numbers" runat="server">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RangeValidator ID="RangeVCapitalPortion" runat="server" SetFocusOnError="True"
                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabAssetDetails"
                                            ErrorMessage="Capital Portion cannot be Zero." ControlToValidate="txtCapitalPortion"
                                            MaximumValue="9999999999" MinimumValue="1">
                                        </asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNonCapitalPortion" runat="server" CssClass="styleReqFieldLabel"
                                            Text="Non-Capital Portion"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtNonCapitalPortion" Style="text-align: right;" runat="server"
                                            MaxLength="10" Width="100px" Text="0" TabIndex="10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNonCapitalPortion" runat="server" ControlToValidate="txtNonCapitalPortion"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Non-Capital Portion"
                                            SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="FTTxtnonCap" TargetControlID="txtNonCapitalPortion"
                                            FilterType="Numbers,Custom" runat="server" ValidChars="." FilterMode="ValidChars">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblPayTo" CssClass="styleReqFieldLabel" Text="Pay To"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlPayTo" runat="server" OnSelectedIndexChanged="ddlPayTo_SelectedIndexChanged"
                                            AutoPostBack="true" TabIndex="11">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvPayTo" runat="server" ControlToValidate="ddlPayTo"
                                            ValidationGroup="TabAssetDetails" CssClass="styleMandatoryLabel" Display="None"
                                            InitialValue="-1" SetFocusOnError="True" ErrorMessage="Select a Pay To"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblNewLeaseAssetNo" runat="server" CssClass="styleReqFieldLabel" Text="Lease Asset No."
                                            Visible="False"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlNewLeaseAssetNo" runat="server" Visible="False" OnSelectedIndexChanged="ddlNewLeaseAssetNo_SelectedIndexChanged" AutoPostBack="True">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvNewLeaseAssetNo" runat="server" ControlToValidate="ddlNewLeaseAssetNo"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select an Least Asset Number"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="TabAssetDetails"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblEntityNameList" CssClass="styleReqFieldLabel" Text="Entity Name"
                                            ></asp:Label>
                                        <asp:Label ID="lblCustomerName" runat="server" CssClass="styleDisplayLabel" Text="Customer Name" Visible="False"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" colspan="3">
                                        <%--<asp:DropDownList ID="ddlEntityNameList" runat="server" Visible="False" TabIndex="13">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEntityNameList" runat="server" ControlToValidate="ddlEntityNameList"
                                            CssClass="styleMandatoryLabel" Display="None" InitialValue="-1" SetFocusOnError="True"
                                            ValidationGroup="TabAssetDetails" ErrorMessage="Select an Entity Name"></asp:RequiredFieldValidator>--%>
                                        <uc2:Suggest ID="ddlEntityNameList" runat="server" ServiceMethod="GetVendors"
                                            ErrorMessage="Select an Entity Name"
                                            ValidationGroup="TabAssetDetails" IsMandatory="true" />
                                        <asp:TextBox ID="txtCustomerName" runat="server" Width="250px" ReadOnly="true" TabIndex="-1" Visible="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblPaymentPercentage" runat="server" CssClass="styleDisplayLabel"
                                            Style="display: none;" Text="Payment %"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtPaymentPercentage" Style="text-align: right; display: none;"
                                            runat="server" MaxLength="6" Width="100px" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                            TabIndex="12"></asp:TextBox>
                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="Payment % should be between 1 and 100"
                                            ControlToValidate="txtPaymentPercentage" MaximumValue="100" MinimumValue="0.0001"
                                            ValidationGroup="TabAssetDetails" Display="None" Type="Double">
                                        </asp:RangeValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="99%">
                            <tr>
                                <td class="styleFieldLabel" align="center" style="width: 99%; padding-left: 35%">
                                    <asp:Button ID="btnOK" runat="server" CausesValidation="true" CssClass="styleSubmitButton"
                                        Text="OK" UseSubmitBehavior="False" ValidationGroup="TabAssetDetails" OnClick="btnOK_Click"
                                        TabIndex="15" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Cancel" UseSubmitBehavior="False" ValidationGroup="TabAssetDetails" OnClick="btnCancel_Click"
                                        TabIndex="16" />
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
                                        Width="705px" ValidationGroup="TabAssetDetails" HeaderText="Please correct the following validation(s):  " />
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
</asp:Content>
