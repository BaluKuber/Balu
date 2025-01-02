<%@ Page Title="S3G - Tax Guide Master" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminTaxGuide_Add.aspx.cs" Inherits="S3GSysAdminTaxGuide_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Tax Guide Master - Create" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="pnlcopyProfile" Style="display: none" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Copy Profile">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="From Asset" ID="lblFromAsset" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlFromAsset" runat="server" AutoPostBack="true" Width="50%">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvFromAsset" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlFromAsset" InitialValue="0" Enabled="false" ErrorMessage="Select From Asset"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="To Asset" ID="lblToAsset" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                        <asp:DropDownList ID="ddlToAsset" runat="server" AutoPostBack="true" Width="50%">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvToAsset" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlToAsset" InitialValue="0" ErrorMessage="Select To Asset" Enabled="false"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <%-- <td class="styleFieldLabel" style="width: 20%">
                                        <asp:Label runat="server" Text="Effective Date" ID="lblEffectiveDate" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>--%>
                                    <%-- <td class="styleFieldLabel">
                                        <asp:TextBox ID="txtEffectiveDate" MaxLength="50" runat="server" Width="80%"></asp:TextBox>
                                        <asp:Image ID="imgEffectiveDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="rfvEffectiveDate" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtEffectiveDate" ErrorMessage="Select Effective Date" Display="None" ValidationGroup="copyprofile">
                                        </asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEffectiveDate"
                                            PopupButtonID="imgEffectiveDate" ID="calEffectiveDate" Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>--%>
                                </tr>
                                <br />
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button runat="server" ID="btnCopy" CssClass="styleSubmitButton" ValidationGroup="copyprofile" Text="Copy" OnClick="btnCopy_Click" />
                                    </td>
                                </tr>
                                <br />
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Tax Guide Master">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Tax Class" ID="lblTaxClass" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:RadioButton ID="rbAll" runat="server" Text="All" OnCheckedChanged="rbAll_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rbpurchase" runat="server" Text="Purchase" OnCheckedChanged="rbpurchase_CheckedChanged" AutoPostBack="true" />
                                        <asp:RadioButton ID="rbsales" runat="server" Text="Sales" OnCheckedChanged="rbsales_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" Width="60%">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select Line of Business"
                                            Display="None" ValidationGroup="save">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Tax Type" ID="lblTaxType" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlTaxType" AutoPostBack="true" OnSelectedIndexChanged="ddlTaxType_OnSelectedIndexChanged"
                                            runat="server" Width="60%">
                                        </asp:DropDownList>
                                        <%-- <asp:RequiredFieldValidator ID="rfvTaxType" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlTaxType" InitialValue="0" ErrorMessage="Select Tax Type"
                                            Display="None">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblleasestate" runat="server" Text="Lease State" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlleasestate" runat="server" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="ddlleasestate_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvleasestate" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlleasestate" InitialValue="0" ErrorMessage="Select Lease State"
                                            Display="None" ValidationGroup="save">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Tax Code" ID="lblTaxCode">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtTaxCode" Enabled="false" runat="server" Width="60%"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblTaxDesc" runat="server" Text="Tax Desc.">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtTaxDesc" MaxLength="50" runat="server" Width="60%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTaxDesc" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtTaxDesc" ErrorMessage="Enter Tax Desc."
                                            Display="None" ValidationGroup="save" Enabled="false">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Reverse Charge Type/Zone" ID="lblReverseChargeTypeorZone" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlReverseChargeTypeorZone" Width="60%" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlReverseChargeTypeorZone_OnSelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Effective From" ID="lblEffFrom" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtEffFrom" runat="server" Width="60%" MaxLength="11" AutoPostBack="true" OnTextChanged="txtEffFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgEffFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="rfvEffFrom" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtEffFrom" ErrorMessage="Enter Effective From" Display="None" ValidationGroup="save">
                                        </asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEffFrom" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                            PopupButtonID="imgEffFrom" ID="CalendarExtender1" Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="GL Code" ID="lblGLCode" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlGLCode" runat="server" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="ddlGLCode_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvGLCode" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlGLCode" InitialValue="0" ErrorMessage="Select GL Code"
                                            Display="None" Enabled="false" ValidationGroup="save">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lblSLCode" runat="server" Text="SL Code" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlSLCode" runat="server" Width="60%">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvSLCode" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlSLCode" InitialValue="0" ErrorMessage="Select SL Code"
                                            Display="None" Enabled="false" ValidationGroup="save">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Tax" ID="lblTax" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtTax" onchange="fnCalculateRate();" MaxLength="12" runat="server" Width="60%"
                                            Style="text-align: right" AutoCompleteType="Disabled"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtTax"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvTax" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtTax" ErrorMessage="Enter Tax" Display="None" ValidationGroup="save">
                                        </asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rvTax" runat="server" ControlToValidate="txtTax" Type="Double" MaximumValue="999.99999999" ErrorMessage="Tax should not exceed 999.99999999%"
                                            Display="None">
                                        </asp:RangeValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Surcharge" ID="lblSurcharge" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtsurcharge" onchange="fnCalculateRate();" runat="server" MaxLength="12"
                                            Width="60%" Style="text-align: right" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RangeValidator ID="rvsurcharge" runat="server" ControlToValidate="txtsurcharge" Type="double" MaximumValue="999.99999999"
                                            ErrorMessage="surcharge should not exceed 999.99999999%" Display="none">
                                        </asp:RangeValidator>
                                        <cc1:FilteredTextBoxExtender ID="filteredtextboxextender4" runat="server" TargetControlID="txtsurcharge"
                                            FilterType="numbers,custom" ValidChars="." Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Cess" ID="lblCess" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtCess" onchange="fnCalculateRate();" runat="server" MaxLength="12"
                                            Width="60%" Style="text-align: right" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RangeValidator ID="rvCess" runat="server" ControlToValidate="txtCess"
                                            Type="Double" MaximumValue="999.99999999" ErrorMessage="Cess should not exceed 999.99999999%"
                                            Display="None">
                                        </asp:RangeValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtCess"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Educational Cess" ID="Label1" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtEduCess" onchange="fnCalculateRate();" runat="server" MaxLength="12"
                                            Width="60%" Style="text-align: right" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RangeValidator ID="rvEduCess" runat="server" ControlToValidate="txtEduCess"
                                            Type="Double" MaximumValue="999.99999999" ErrorMessage="Educational Cess should not exceed 999.99999999%"
                                            Display="None">
                                        </asp:RangeValidator>
                                        <cc1:FilteredTextBoxExtender ID="fteEduCess" runat="server" TargetControlID="txtEduCess"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Rate %" ID="lblRate" CssClass="styleDisplayLabel">
                                        </asp:Label>

                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtRate" MaxLength="12" runat="server" Width="60%" Style="text-align: right"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Additional Tax" ID="lblAdditionalTax" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtAdditionalTax" AutoPostBack="True" runat="server" MaxLength="12" Width="60%" Style="text-align: right" OnTextChanged="txtAdditionalTax_TextChanged" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RangeValidator ID="rvAdditionalTax" runat="server" ControlToValidate="txtAdditionalTax"
                                            Type="Double" MaximumValue="999.99999999" ErrorMessage="Additional Tax should not exceed 999.99999999%"
                                            Display="None">
                                        </asp:RangeValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftbetxtAdditionalTax" runat="server" TargetControlID="txtAdditionalTax"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblAdditionalTaxName" runat="server" Text="Additional Tax Name" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:TextBox ID="txtAdditionalTaxName" runat="server" MaxLength="50" Width="60%"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="fteAdditionalTaxName" runat="server" TargetControlID="txtAdditionalTaxName" FilterMode="InvalidChars"
                                            FilterType="UppercaseLetters,LowercaseLetters" InvalidChars="!@#$%^&*()_+=-~`<>?,./;':{}|[]\" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Additional Tax Based On" ID="lbladditionaltaxbasedon" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlAdditionalTaxBasedOn" runat="server" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="ddlAdditionalTaxBasedOn_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblOtherTaxName" runat="server" Text="Other Tax Name" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlOtherTaxName" runat="server" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="ddlOtherTaxName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblOtherTaxBasedOn" runat="server" Text="Other Tax Based On" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlOtherTaxBasedOn" runat="server" Width="60%" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlOtherTaxBasedOn_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblOtherTaxRate" runat="server" Text="Other Tax Rate" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtOtherTaxRate" runat="server" MaxLength="12" Width="60%" Style="text-align: right"
                                            AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="txtOtherTaxRate_TextChanged"></asp:TextBox>
                                        <asp:RangeValidator ID="rvOtherTaxRate" runat="server" ControlToValidate="txtOtherTaxRate"
                                            Type="Double" MaximumValue="999.99999999" ErrorMessage="Other Tax Rate should not exceed 999.99999999%"
                                            Display="None">
                                        </asp:RangeValidator>
                                        <cc1:FilteredTextBoxExtender ID="fteOtherTaxRate" runat="server" TargetControlID="txtOtherTaxRate"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%"></td>
                                    <td class="styleFieldLabel" style="width: 25%"></td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lbltotaltax" runat="server" CssClass="styleDisplayLabel" Text="Total Tax" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:TextBox ID="txttotaltax" runat="server" Width="60%" Style="text-align: right"></asp:TextBox>
                                    </td>
                                    <asp:HiddenField ID="hfAssetTypeDesc" runat="server" />
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:CheckBox ID="chkActive" Checked="true" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblAbatement" runat="server" Text="Abatement" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtAbatement" runat="server" Text="0"
                                            Width="60%" Style="text-align: right"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvAbatement" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtAbatement" ErrorMessage="Enter Abatement" ValidationGroup="Save" Display="None" Enabled="false">
                                        </asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator1" CssClass="styleMandatoryLabel" runat="server" ControlToValidate="txtEduCess"
                                            Type="Double" MaximumValue="100" ErrorMessage="Abatement should not exceed 99.99%"
                                            Display="None">
                                        </asp:RangeValidator>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtAbatement"
                                            FilterType="Numbers,Custom" ValidChars="." Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblserviceType" runat="server" Text="Service Type" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlServiceType" runat="server" Width="62%" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvServiceType" CssClass="styleMandatoryLabel" runat="server" Enabled ="false"
                                            ControlToValidate="ddlServiceType" InitialValue="0" ValidationGroup="Save" ErrorMessage="Select Service Type"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="pnlHSN" GroupingText="HSN Details" CssClass="stylePanel" runat="server" Visible="false">
                                    <div style="height: 635px; overflow-x: hidden; overflow-y: auto;">
                                        <asp:GridView ID="grvHSNCOde" runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                            Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No." ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHSNSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'
                                                            ToolTip="Serial No."></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHSNID" runat="server" Visible="false" Text='<%#Eval("HSN_ID")%>'></asp:Label>
                                                        <asp:Label ID="lblHSNCode" runat="server" Text='<%#Bind("HSN_Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <uc3:Suggest ID="txtHSNCode" AutoPostBack="True" runat="server" ServiceMethod="GetHSNCode" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="styleSubmitShortButton" OnClick="btnRemove_Click" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="styleSubmitShortButton" OnClick="btnAdd_Click" />
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                </tr>

                <tr>
                    <td style="width: 100%" align="center">
                        <asp:Button ID="btnGo" runat="server" Text="View History" ValidationGroup="save" CssClass="styleSubmitButton" OnClick="btnGo_Click" />
                    </td>
                </tr>
                <tr>
                    <td>

                        <asp:Panel ID="pnlAsset" Visible="false" GroupingText="Asset Type Wise Details" CssClass="stylePanel" runat="server">
                            <asp:UpdatePanel ID="upTaxAsset" runat="server">
                                <ContentTemplate>
                                    <div style="height: 135px; overflow-x: hidden; overflow-y: auto; width: 100%">
                                        <div>
                                            <asp:GridView ID="gvTaxAsset" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                                OnRowDataBound="gvTaxAsset_RowDataBound" Width="100%" OnRowCommand="gvTaxAsset_RowCommand"
                                                OnRowDeleting="gvTaxAsset_RowDeleting">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No." ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Right">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetSerialNoo" runat="server" Text='<%#Bind("Asset_Serial_Number") %>' Width="70px" Visible="false"></asp:Label>
                                                            <%#Container.DataItemIndex+1%>
                                                        </ItemTemplate>
                                                        <FooterStyle HorizontalAlign="Right" Width="15%" />
                                                    </asp:TemplateField>
                                                    <%-- <asp:TemplateField HeaderText="Asset Category" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetCategory" runat="server" Text='<%#Bind("Asset_Category") %>' ToolTip="Asset Category"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlAssetCategory" runat="server" ToolTip="Asset Category" Width="150px">
                                                                    </asp:DropDownList>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Left" Width="25%" />
                                                                  </asp:TemplateField>--%>
                                                    <%--OnSelectedIndexChanged="ddlAssetCategory_Grd_SelectedIndexChanged"--%>
                                                    <asp:TemplateField HeaderText="Asset Type" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAssetType" runat="server" Text='<%#Bind("Asset_Type") %>' ToolTip="Asset Type"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlAssetType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetType_Grd_SelectedIndexChanged"
                                                                ToolTip="Asset Type" Width="150px">
                                                            </asp:DropDownList>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Left" Width="25%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Add | Delete" HeaderStyle-Wrap="false" Visible="false" ItemStyle-Width="20%">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkRemove" runat="server" Text="Delete" CommandName="Delete"
                                                                ToolTip="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                CausesValidation="false"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                        <FooterTemplate>
                                                            <asp:HiddenField ID="ghTax_Guide_Detail_ID" runat="server" Value='<%#Bind("Tax_Guide_Detail_ID") %>'></asp:HiddenField>
                                                            <asp:LinkButton ID="lnkAdd" runat="server" Text="Add" CommandName="Add" CausesValidation="false"
                                                                ToolTip="Add"></asp:LinkButton>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlservice" Visible="false" GroupingText="Services" CssClass="stylePanel" runat="server">
                            <div style="height: 135px; overflow-x: hidden; overflow-y: auto;">
                                <asp:GridView ID="grvServices" runat="server" AutoGenerateColumns="false"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No." ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'
                                                    ToolTip="Serial No."></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cashflow Flag">
                                            <ItemTemplate>
                                                <asp:Label ID="lblserviceID" runat="server" Visible="false" Text='<%#Eval("ID")%>'></asp:Label>
                                                <asp:Label ID="lblserviceName" runat="server" Text='<%#Bind("NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <tr>
                        <td>
                            <cc1:TabContainer ID="tcTaxAssetHistory" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                Width="99%" ScrollBars="Auto" TabStripPlacement="top" AutoPostBack="false">
                                <cc1:TabPanel ID="tpTaxAssetHistory" runat="server" HeaderText="Asset History">
                                    <HeaderTemplate>
                                        Asset History
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upTaxAssetHistory" runat="server">
                                            <ContentTemplate>
                                                <div style="height: 135px; overflow-x: hidden; overflow-y: auto; width: 100%">
                                                    <div>
                                                        <asp:GridView ID="gvTaxAssetHistory" runat="server" AutoGenerateColumns="false" ShowFooter="false" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="S.No." ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblassethistoryserialno" runat="server" Text='<%#Container.DataItemIndex+1%>'
                                                                            Width="20px" ToolTip="Serial No."></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Category" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblassetcategorydesc" runat="server" ToolTip="Asset Category"></asp:Label>
                                                                        <%--Text='<%#Bind("Asset_Category_Desc") %>' --%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Asset Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblassettypedesc" runat="server" Text='<%#Bind("Asset_Type_Desc") %>' ToolTip="Asset Type"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Effective From">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbleffectivefrom" runat="server" Text='<%#Eval("Effective_From")%>' ToolTip="Effective From"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Effective To" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbleffectiveto" runat="server" Text='<%#Bind("Effective_To") %>' ToolTip="Effective To"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tax Rate" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbltaxrate" runat="server" Text='<%#Bind("RatePercentage") %>' ToolTip="Tax Rate"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" valign="top">
                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 5px; padding-left: 5px;" align="center">
                            <span runat="server" id="lblErrorMessage1" class="styleMandatoryLabel"></span>
                        </td>
                    </tr>
                    <tr class="styleButtonArea">
                        <td align="center">
                            <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click"
                                OnClientClick="return fnCheckPageValidation();" ValidationGroup="save" />

                            <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                                Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr class="styleButtonArea">
                        <td>
                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Please correct the following validation(s):"
                                Height="250px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                                ShowSummary="true" ValidationGroup="save" />
                            <%-- <asp:ValidationSummary runat="server" ID="vsCopyProfile" HeaderText="Please correct the following validation(s):"
                            Height="250px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="copyprofile" />--%>
                        </td>
                    </tr>
                    <input type="hidden" id="hdnID" runat="server" />
                    <input type="hidden" id="hdnSave" runat="server" />
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlServiceType" />
        </Triggers>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        function fnValidateTax() {
            if (document.getElementById('<%=txtTax.ClientID%>').value != '' || document.getElementById('<%=txtsurcharge.ClientID%>').value != '' || document.getElementById('<%=txtCess.ClientID%>').value != '')//If onFC is Yes
            {
                if (document.getElementById('<%=txtTax.ClientID%>').value == '') {
                    document.getElementById('<%=rfvTax.ClientID%>').enabled = true;
                    return false;
                }

                if (document.getElementById('<%=txtTax.ClientID %>').value > 99.99) {
                    document.getElementById('<%=rvTax.ClientID%>').enabled = true;
                    return false;
                }

                if (document.getElementById('<%=txtCess.ClientID %>').value > 99.99) {
                    document.getElementById('<%=rvCess.ClientID%>').enabled = true;
                    return false;
                }
            }
            else {
                document.getElementById('<%=rfvTax.ClientID%>').enabled = false;
            }
        }

        function fnCalculateRate() {

            var Tax = document.getElementById('<%=txtTax.ClientID%>').value;

            var Surcharge = document.getElementById('<%=txtsurcharge.ClientID%>').value;
            var Surcharge_val = document.getElementById('<%=txtsurcharge.ClientID%>').value;

            var Cess = document.getElementById('<%=txtCess.ClientID%>').value;
            var Cess_val = document.getElementById('<%=txtCess.ClientID%>').value;

            var EduCess = document.getElementById('<%=txtEduCess.ClientID%>').value;

            var Add_Tax = document.getElementById('<%=txtAdditionalTax.ClientID%>').value;
            var Add_Tax_Val = document.getElementById('<%=txtAdditionalTax.ClientID%>').value;
            var AT_BasOn = document.getElementById('<%=ddlAdditionalTaxBasedOn.ClientID%>').value;

            var Other_Tax = document.getElementById('<%=txtOtherTaxRate.ClientID%>').value;
            var Other_Tax_Val = document.getElementById('<%=txtOtherTaxRate.ClientID%>').value;
            var OT_BasOn = document.getElementById('<%=ddlOtherTaxBasedOn.ClientID%>').value;


            if (Tax == '')
                Tax = 0;
            if (Surcharge == '')
                Surcharge = 0;
            if (Surcharge_val == '')
                Surcharge_val = 0;
            if (Cess == '')
                Cess = 0;
            if (Cess_val == '')
                Cess_val = 0;
            if (EduCess == '')
                EduCess = 0;

            if (Add_Tax == '')
                Add_Tax = 0;
            if (Add_Tax_Val == '')
                Add_Tax_Val = 0;
            if (AT_BasOn == '')
                AT_BasOn = 0;

            if (Other_Tax == '')
                Other_Tax = 0;
            if (Other_Tax_Val == '')
                Other_Tax_Val = 0;
            if (OT_BasOn == '')
                OT_BasOn = 0;

            var Result, Tot_Tax;

            var Surcharge = parseFloat((parseFloat(Tax) * (parseFloat(Surcharge) / 100)));
            var Cess = ((parseFloat(Tax) + parseFloat(Surcharge_val)) * parseFloat(Cess)) / 100
            //var Cess = Math.round(parseFloat((parseFloat(Tax) + Surcharge) * (parseFloat(Cess) / 100)) * 100) / 100
            var EduCess = ((parseFloat(Tax) + parseFloat(Surcharge_val)) * parseFloat(EduCess)) / 100
            //var EduCess = Math.round((EduCess) * parseFloat(Cess_val)) / 100

            Result = Math.round((parseFloat(Tax) + parseFloat(Surcharge) + parseFloat(Cess) + parseFloat(EduCess)) * 100000000) / 100000000
            Result = parseFloat(Result).toFixed(8);
            document.getElementById('<%=txtRate.ClientID%>').value = Result;

            if (AT_BasOn == 1) {
                Add_Tax_Val = (Add_Tax_Val / 100) * Result;
                Add_Tax = Add_Tax_Val;
            }

            if (OT_BasOn == 1) {
                Other_Tax_Val = (Other_Tax_Val / 100) * Result;
                Other_Tax = Other_Tax_Val;
            }

            Tot_Tax = parseFloat(Result) + parseFloat(Add_Tax) + parseFloat(Other_Tax);
            Tot_Tax = parseFloat(Tot_Tax).toFixed(8);
            document.getElementById('<%=txttotaltax.ClientID%>').value = Tot_Tax;
        }

        function fnCheckPageValidation() {
            if ((!fnCheckPageValidators(null, 'false'))) {
                if (Page_ClientValidate() == false) {
                    Page_BlockSubmit = false;
                    return false;
                }
            }
            document.getElementById('<%=txtTax.ClientID%>').className = 'styleReqFieldDefalut';
            document.getElementById('<%=txtsurcharge.ClientID%>').className = 'styleReqFieldDefalut';
            document.getElementById('<%=txtCess.ClientID%>').className = 'styleReqFieldDefalut';
            document.getElementById('<%=txtRate.ClientID%>').className = 'styleReqFieldDefalut';


            if (Page_ClientValidate()) {
                if (document.getElementById('<%=txtTax.ClientID%>').value != '' || document.getElementById('<%=txtsurcharge.ClientID%>').value != '' || document.getElementById('<%=txtCess.ClientID%>').value != '')//If onFC is Yes
                {
                    if (document.getElementById('<%=txtTax.ClientID%>').value == '') {
                        document.getElementById('<%=rfvTax.ClientID%>').enabled = true;
                        return false;
                    }
                }
                document.getElementById('<%=btnSave.ClientID%>').style.display = "inline";

                //                    if (parseFloat(Result, 2) != parseFloat(RatePer, 2))
                //                  {
                //                    alert('Please enter correct rate percentage (Tax+[(surcharge on Tax)+(cess(Tax+Surcharge of Tax))]=Rate%)');                        
                //return false;
                //              }
            }
            if (confirm('Do you want to save?')) {
                return true;
            }
            else {
                //Added by Santhosh S on 29/Jun/2013 to avoid Save button to be hidden in Modify mode, when modification is Cancelled.
                document.getElementById('<%=btnSave.ClientID%>').style.display = "inline";
                //End here
                return false;
            }
        }

        function fnChange(chkSelect) {
            document.getElementById('<%=hdnSave.ClientID%>').value = "1";
        }
    </script>
</asp:Content>
