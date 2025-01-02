<%@ Page Title="S3G - Tax Exemption Master" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_ORG_TaxExemptionMaster_Add.aspx.cs"
    Inherits="Origination_S3G_ORG_TaxExemptionMaster_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Tax Exemption Master - Create" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Tax Exemption Master">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblTaxCode" runat="server" Text="Tax Code">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtTaxCode" Enabled="false" runat="server" Width="60%"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblLessee" runat="server" Text="Lessee" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlLessee" runat="server" ServiceMethod="GetLesseeDetails" AutoPostBack="true"
                                            OnItem_Selected="ddlLessee_SelectedIndexChanged" ErrorMessage="Select Lessee" Width="120%"
                                            ValidationGroup="Main" IsMandatory="true" />
                                    </td>

                                </tr>
                                <tr>
                                    <%--<td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblLocation" runat="server" Text="State" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetDeliveryStateDetails" AutoPostBack="true" OnItem_Selected="ddlLocation_SelectedIndexChanged"
                                            ErrorMessage="Select State" ValidationGroup="Main" IsMandatory="true" Width="195px"/>
                                    </td>--%>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblTAN" runat="server" Text="TAN" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlTAN" runat="server" Width="60%" TabIndex="2">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvTAN" CssClass="styleMandatoryLabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="ddlTAN" InitialValue="0" ErrorMessage="Select TAN" Width="500px"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblSection" runat="server" Text="Section" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtSection" MaxLength="10" runat="server" Width="60%" AutoPostBack="true" OnTextChanged="txtSection_TextChanged"
                                            TabIndex="3"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvSection" CssClass="styleMandatoryLabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="txtSection" ErrorMessage="Enter Section"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftbeSection" runat="server" TargetControlID="txtSection"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()/-" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtFromDate" runat="server" Width="60%" TabIndex="4" MaxLength="10"></asp:TextBox>
                                        <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="rfvFromDate" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtFromDate" ErrorMessage="Enter From Date" Display="None" ValidationGroup="Main">
                                        </asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender runat="server" TargetControlID="txtFromDate" PopupButtonID="imgFromDate" ID="CalendarExtender1" Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblToDate" runat="server" Text="To Date" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtToDate" runat="server" Width="58%" TabIndex="5" MaxLength="10"></asp:TextBox>
                                        <asp:Image ID="imgToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="rfvToDate" CssClass="styleMandatoryLabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="txtToDate" ErrorMessage="Enter To Date" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <cc1:CalendarExtender runat="server" TargetControlID="txtToDate" PopupButtonID="imgToDate" ID="CalendarExtender2" Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblCashFlowType" runat="server" Text="Cash Flow Type" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:DropDownList ID="ddlCashFlowType" runat="server" Width="60%" TabIndex="6">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCashFlowType" CssClass="styleMandatoryLabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="ddlCashFlowType" InitialValue="0" ErrorMessage="Select Cash Flow Type" Width="60%"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblCertificateNo" runat="server" Text="Certificate No." CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtCertificateNo" runat="server" MaxLength="12" TabIndex="7" Width="60%" Style="text-align: left"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCertificateNo" CssClass="stylemandatorylabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="txtCertificateNo" ErrorMessage="Enter Certificate No."
                                            Display="none">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="filteredtextboxextender4" runat="server" TargetControlID="txtCertificateNo"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers" ValidChars="()/-" Enabled="true">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblExemptionLimit" runat="server" Text="Exemption Limit" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtExemptionLimit" runat="server" MaxLength="10" Width="58%" TabIndex="8" Style="text-align: right"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtExemptionLimit"
                                            FilterType="Numbers,Custom" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvExemptionLimit" CssClass="styleMandatoryLabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="txtExemptionLimit" ErrorMessage="Enter Exemption Limit" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblBalanceLimit" runat="server" Text="Balance Limit">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtBalanceLimit" Enabled="false" runat="server" Width="60%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:CheckBox ID="chkActive" Checked="true" runat="server" TabIndex="9" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%"></td>
                                    <td class="styleFieldAlign" style="width: 25%"></td>
                                </tr>
                            </table>
                            <br />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcTaxExemptionHistory" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            Width="99%" ScrollBars="Auto" TabStripPlacement="top" AutoPostBack="false">
                            <cc1:TabPanel ID="tpTaxExemptionHistory" runat="server" HeaderText="Tax Exemption History">
                                <HeaderTemplate>
                                    Tax Exemption History
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upTaxExemptionHistory" runat="server">
                                        <ContentTemplate>
                                            <div style="height: 135px; overflow-x: hidden; overflow-y: auto; width: 100%">
                                                <div>
                                                    <asp:GridView ID="gvTaxExemptionHistory" runat="server" AutoGenerateColumns="false" ShowFooter="false" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No." ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTaxExemptionHistoryserialno" runat="server" Text='<%#Container.DataItemIndex+1%>'
                                                                        Width="20px" ToolTip="Serial No."></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TAN">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTAN" runat="server" Text='<%#Bind("TAN") %>' ToolTip="TAN"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Section">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSection" runat="server" Text='<%#Bind("Tax_Law_Section") %>' ToolTip="Section"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromDate" runat="server" Text='<%#Eval("Effective_From")%>' ToolTip="From Date"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToDate" runat="server" Text='<%#Eval("Effective_To")%>' ToolTip="To Date"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cash Flow Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCashFlowType" runat="server" Text='<%#Bind("CashFlowFlag_Desc") %>' ToolTip="Cash Flow Type"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Exemption Limit" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblExemptionLimit" runat="server" Text='<%#Bind("Exe_Limit_Amount") %>' ToolTip="Exemption Limit"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Certificate No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCertificateNo" runat="server" Text='<%#Bind("Certificate_No") %>' ToolTip="Certificate No."></asp:Label>
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
                    <td style="padding-top: 5px; padding-left: 5px;" align="Center">
                        <span runat="server" id="lblErrorMessage1" class="styleMandatoryLabel"></span>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" TabIndex="15" CssClass="styleSubmitButton"
                            Text="Save" OnClick="btnSave_Click" ValidationGroup="Main" OnClientClick="return fnCheckPageValidators('Main');" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                            TabIndex="16" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" TabIndex="17" />
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
                            Height="250px" ValidationGroup="Main" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                    </td>
                </tr>
                <input type="hidden" id="hdnID" runat="server" />
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
