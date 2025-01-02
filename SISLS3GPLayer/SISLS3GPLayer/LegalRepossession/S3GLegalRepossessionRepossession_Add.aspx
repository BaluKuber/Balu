<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLegalRepossessionRepossession_Add.aspx.cs" Inherits="LegalRepossession_S3GLegalRepossessionRepossession_Add"
    Title="S3G - Repossession" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0" scrolling="no">
        <tr>
            <td class="stylePageHeading">
                <table>
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
                <cc1:TabContainer ID="tcRepossession" runat="server" ActiveTabIndex="1" CssClass="styleTabPanel"
                    Width="99%" ScrollBars="Auto" Visible="true">
                    <cc1:TabPanel runat="server" ID="tbRepossession" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Repossession
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 25%">
                                                            <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:DropDownList ID="ddlLOB" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                                runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="styleFieldLabel" style="width: 25%">
                                                            <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">

                                                            <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                                            <%-- <asp:DropDownList OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ID="ddlBranch"
                                                        runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>--%>
                                                            <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select Line of Business"
                                                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="rfvRepLOB" runat="server" ControlToValidate="ddlLOB"
                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select Line of Business"
                                                                ValidationGroup="tbRepossession"></asp:RequiredFieldValidator>
                                                            <%--   <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select the Location"
                                                        ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvRepBranch" runat="server" ControlToValidate="ddlBranch"
                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select the Location"
                                                        ValidationGroup="tbRepossession"></asp:RequiredFieldValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="width: 25%">
                                                            <asp:Label ID="lblLRNNumber" runat="server" CssClass="styleReqFieldLabel" Text="LRN Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:DropDownList OnSelectedIndexChanged="ddlLRNNumber_SelectedIndexChanged" ID="ddlLRNNumber"
                                                                runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvLRNNo" runat="server" ControlToValidate="ddlLRNNumber"
                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select LRN Number"
                                                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlLRNNumber"
                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select LRN Number"
                                                                ValidationGroup="tbRepossession"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlLRNNumber"
                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select LRN Number"
                                                                ValidationGroup="tbRepossession"></asp:RequiredFieldValidator>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlLRNNumber"
                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select LRN Number"
                                                                ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:Label ID="lblPANum" runat="server" CssClass="styleDisplayLabel" Text="Prime Account Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:TextBox ID="txtPANum" runat="server" Width="130px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:Label ID="lblSANum" runat="server" CssClass="styleDisplayLabel" Text="Sub Account Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:TextBox ID="txtSANum" runat="server" Width="130px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25">
                                                            <asp:Label ID="lblRepoDocNo" runat="server" Text="Repo Docket Number" CssClass="styleDisplayLabel"
                                                                Width="130px"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:TextBox ID="txtRepoNo" runat="server" ReadOnly="True" Width="130px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:Label ID="lblRepoDate" runat="server" CssClass="styleDisplayLabel" Text="Repossession Docket Date"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="width: 25%">
                                                            <asp:TextBox ID="txtRepoDate" runat="server" ReadOnly="True" Width="90px"></asp:TextBox>
                                                        </td>
                                                        <%--<td class="styleFieldAlign" style="width: 25%">
                                                    <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 25%">
                                                    <%--<asp:CheckBox ID="ChkActive" runat="server" ToolTip="Select Asset" />
                                                </td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel GroupingText="Customer Communication Address" ID="pnlCommAddress" runat="server"
                                                                CssClass="stylePanel">
                                                                <table width="100%" align="center" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" Visible="False" ID="lblCustID"></asp:Label>
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                FirstColumnWidth="20%" SecondColumnWidth="30%" ThirdColumnWidth="18%" FourthColumnWidth="32%"
                                                                                ActiveViewIndex="1" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100%">
                                                            <asp:Panel GroupingText="Guarantor Address" ID="pnlGuarantorAddress" runat="server"
                                                                CssClass="stylePanel" Width="100%">
                                                                <table align="center" width="100%" border="0" cellspacing="0">
                                                                    <asp:Label ID="lblGuarDetails" runat="server" Text="No Guarantor details Available"
                                                                        Visible="False" />
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView Width="100%" ID="gvGuarDetails" runat="server" AutoGenerateColumns="False">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="S.No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField HeaderText="Guarantor Code" DataField="Guarantor_Code" />
                                                                                    <asp:BoundField HeaderText="Guarantor Name" DataField="Guarantor_Name" />
                                                                                    <asp:BoundField HeaderText="Guarantor Address" DataField="Guarantor_Address1" />
                                                                                    <asp:BoundField HeaderText="Guarantee Amount" DataField="Guarantee_Amount" ItemStyle-HorizontalAlign="Right" />
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 25%">
                                                            <asp:Label ID="lblAccountDate" runat="server" Text="Account Date" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td style="width: 25%">
                                                            <asp:TextBox ID="txtAccountDate" runat="server" ReadOnly="True" Width="90px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 25%">
                                                            <asp:Label ID="lblTenure" runat="server" Text="Tenure" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td style="width: 25%">
                                                            <asp:TextBox ID="txtTenure" runat="server" ReadOnly="True" Style="text-align: right"
                                                                Width="20%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 25%">
                                                            <asp:Label ID="lblAmtFinanced" runat="server" Text="Amount Financed" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td style="width: 15%">
                                                            <asp:TextBox ID="txtAmtFinanced" runat="server" ReadOnly="True" Style="text-align: right"
                                                                Width="40%"> </asp:TextBox>
                                                        </td>
                                                        <td style="width: 35%">
                                                            <asp:Button ID="btnViewAccount" runat="server" CssClass="styleSubmitButton" Text="View Account"
                                                                OnClick="btnViewAccount_Click" CausesValidation="false" />
                                                            <asp:Button ID="btnLedgerView" runat="server" CssClass="styleSubmitButton" Text="View Ledger"
                                                                CausesValidation="false" />
                                                        </td>
                                                        <td style="width: 25%"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <table width="100%" align="center" border="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="Panel3" runat="server" CssClass="stylePanel" GroupingText="Asset Details">
                                                                <asp:GridView ID="gvAssetDetails" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                                                    DataKeyNames="Asset_Number" ToolTip="Asset Details" Width="100%" OnRowDataBound="gvAssetDetails_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAssetID" runat="server" Text='<%# Eval("Asset_Number") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="SL.No.">
                                                                            <ItemTemplate>
                                                                                <%# Container.DataItemIndex + 1 %>
                                                                                <%---<asp:Label ID="lblProgramRefNo" runat="server" Text='<%# Eval("SerialNo") %>' />--%>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="5%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAsset_Type" runat="server" Text='<%# Eval("AssetType_ID") %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Asset Code">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAssetCode" runat="server" Text='<%# Eval("Asset_Code") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Asset Description">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAssetDesc1" runat="server" Text='<%# Eval("Asset_Description") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="15%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Vehicle Registration">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEngineNumber" runat="server" Text='<%# Eval("REGN_NUMBER") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="25%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Serial Number">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Eval("SERIAL_NUMBER") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="25%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Asset Value" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAssetCost" runat="server" Text='<%# Eval("Asset_Cost") %>' />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                            <ItemStyle Width="25%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Select">
                                                                            <ItemTemplate>
                                                                                <table>
                                                                                    <tr align="center">
                                                                                        <td align="center">
                                                                                            <asp:UpdatePanel ID="updChk" runat="server">
                                                                                                <ContentTemplate>
                                                                                                    <asp:CheckBox ID="CbAssets" runat="server" ToolTip="Select Asset" AutoPostBack="true"
                                                                                                        OnCheckedChanged="CbAssets_OnCheckedChanged" />
                                                                                                </ContentTemplate>
                                                                                                <Triggers>
                                                                                                    <asp:PostBackTrigger ControlID="CbAssets" />
                                                                                                </Triggers>
                                                                                            </asp:UpdatePanel>
                                                                                            <asp:Label ID="lblAsset_ID" runat="server" Visible="false" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </ItemTemplate>
                                                                            <HeaderTemplate>
                                                                                <table>
                                                                                    <tr align="center">
                                                                                        <td align="center">Select Asset
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="tbRepossession" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tbRepossessionDetails" CssClass="tabpan" BackColor="Red" Enabled="false">
                        <HeaderTemplate>
                            Repossession Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:Label ID="lblRepossessionDate" runat="server" CssClass="styleReqFieldLabel"
                                                                            Text="Repossession Date"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:TextBox ID="txtRepossessionDate" runat="server" Width="90px" OnTextChanged="txtRepossessionDate_TextChanged"
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                        <asp:Image ID="imgDateofActivation" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                                            ToolTip="Repossession Date" />
                                                                        <cc1:CalendarExtender ID="CalendarExtenderRepossessionDate" runat="server" Enabled="True"
                                                                            Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateofActivation"
                                                                            TargetControlID="txtRepossessionDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvRepossDate" runat="server" ControlToValidate="txtRepossessionDate"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Repossession Date"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvRepRepossDate" runat="server" ControlToValidate="txtRepossessionDate"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Repossession Date"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:Label ID="lblPlace" runat="server" CssClass="styleReqFieldLabel" Text="Repossession Place"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:TextBox ID="txtPlace" TextMode="MultiLine" MaxLength="200" onkeyup="maxlengthfortxt(200);"
                                                                            Width="180px" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvPlace" runat="server" ControlToValidate="txtPlace"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Repossession Place"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfdRepPlace" runat="server" ControlToValidate="txtPlace"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Repossession Place"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblPoliceStation" runat="server" CssClass="styleDisplayLabel" Text="Whether Nearest Police Station has been Informed in Writing"></asp:Label>
                                                                        <span style="font-size: 14px;">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:RadioButton ID="rdoYes" AutoPostBack="true" GroupName="DocYes" runat="server"
                                                                            Text="Yes" />
                                                                        <asp:RadioButton ID="rdoNo" AutoPostBack="true" GroupName="DocYes" runat="server"
                                                                            Text="No" />
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblCondition" runat="server" CssClass="styleReqFieldLabel" Text="Condition of Asset"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtCondition" TextMode="MultiLine" MaxLength="200" onkeyup="maxlengthfortxt(200);"
                                                                            Width="180px" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvCondition" runat="server" ControlToValidate="txtCondition"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Condition of Asset"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfdRepCondi" runat="server" ControlToValidate="txtCondition"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Condition of Asset"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblRepossessionDoneBy" runat="server" CssClass="styleReqFieldLabel"
                                                                            Text="Repossession Done By"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlRepossessionDoneBy" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRepossessionDoneBy_SelectedIndexChanged"
                                                                            Width="100px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvRepDoneBy" runat="server" ControlToValidate="ddlRepossessionDoneBy"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Repossession Done By"
                                                                            ValidationGroup="Save" InitialValue="0"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfdRepdone" runat="server" ControlToValidate="ddlRepossessionDoneBy"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Repossession Done By"
                                                                            ValidationGroup="tbRepossessionDetails" InitialValue="0"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblEntity" runat="server" CssClass="styleReqFieldLabel" Text="Employee/Agent Code "></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlAgentCode" runat="server" AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvAgentCode" runat="server" ControlToValidate="ddlAgentCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select Employee/Agent Code"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvRepAgent" runat="server" ControlToValidate="ddlAgentCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select Employee/Agent Code"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlAgentCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Employee/Agent Code"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlAgentCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Employee/Agent Code"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblGarageCode" runat="server" CssClass="styleReqFieldLabel" Text="Garage Code"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlGarageCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGarageCode_SelectedIndexChanged"
                                                                            EnableTheming="True" Width="130px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvGarageCode" runat="server" ControlToValidate="ddlGarageCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select the Garage Code"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlGarageCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select the Garage Code"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblGarageIn" runat="server" CssClass="styleDisplayLabel" Text="Garage In"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtGarageIn" runat="server" ReadOnly="True" Width="90px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td width="100%">
                                                                        <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" GroupingText="Garage Details">
                                                                            <table align="center" border="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td class="styleFieldAlign">
                                                                                        <uc1:S3GCustomerAddress ID="S3GGarageAddress" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                            ShowCustomerCode="false" ShowCustomerName="false" Caption="Garage" ActiveViewIndex="1"
                                                                                            FirstColumnWidth="20%" SecondColumnWidth="30%" ThirdColumnWidth="18%" FourthColumnWidth="32%" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:Label ID="lblInsuranceDate" runat="server" CssClass="styleDisplayLabel" Text="Insurance Valid Upto"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:TextBox ID="txtInsuranceDate" runat="server" ReadOnly="true" Width="90px">
                                                                        </asp:TextBox>
                                                                        <asp:Label ID="hdnInsurance_ID" Visible="false" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:Label ID="lblCurrentMarketValue" runat="server" CssClass="styleReqFieldLabel"
                                                                            Text=" Current Market Value"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:TextBox ID="txtCurrentMarket" runat="server" MaxLength="10" Style="text-align: right"
                                                                            Width="100px" onblur="fnAllowNumbersOnly(true,false,this)">
                                                                        </asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1Hdr" runat="server" FilterType="Numbers,Custom"
                                                                            TargetControlID="txtCurrentMarket" ValidChars=".">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvCurrentMarket" runat="server" ControlToValidate="txtCurrentMarket"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Current Market Value"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvRepCurrent" runat="server" ControlToValidate="txtCurrentMarket"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Current Market Value"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblRemarks" runat="server" CssClass="styleDisplayLabel" Text="Remarks"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="200" onkeyup="maxlengthfortxt(200);"
                                                                            TextMode="MultiLine" Width="180px"></asp:TextBox>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblRepoExp" runat="server" CssClass="styleDisplayLabel" Text="Expenses Incurred During Repossession"></asp:Label>
                                                                        <span class="styleMandatoryLabel" style="font-size: 14px;">*</span>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtRepoExp" runat="server" MaxLength="300" onkeyup="maxlengthfortxt(300);"
                                                                            TextMode="MultiLine" Width="180px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtRepoExp" runat="server" ControlToValidate="txtRepoExp"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Expenses Incurred During Repossession"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvRepRepEx" runat="server" ControlToValidate="txtRepoExp"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Expenses Incurred During Repossession"
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblInventoryDetails" runat="server" CssClass="styleReqFieldLabel"
                                                                            Text="Asset Inventory Details"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtAssetInventory" runat="server" MaxLength="100" onkeyup="maxlengthfortxt(100);"
                                                                            TextMode="MultiLine" Width="180px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvAssetInventory" runat="server" ControlToValidate="txtAssetInventory"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Asset Inventory Details "
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvRepInv" runat="server" ControlToValidate="txtAssetInventory"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Asset Inventory Details "
                                                                            ValidationGroup="tbRepossessionDetails"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldAlign"></td>
                                                                    <td class="styleFieldAlign"></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="tbRepossessionDetails" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr style="height: 10px">
            <td></td>
        </tr>
        <tr>
            <td id="Td1" runat="server" align="center">
                <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                    OnClientClick="return fnCheckPageValidators('tbRepossessionDetails',false);" OnClick="btnSave_Click" />
                <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                    CausesValidation="False" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                    Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="rdfgdfff343" runat="server">
                    <ContentTemplate>
                        <asp:ValidationSummary ID="vsRepossession1" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Save" ShowSummary="true" />
                        <%--<asp:ValidationSummary ID="vsRepossession" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):  " ValidationGroup="tbRepossession" />--%>
                        <asp:CustomValidator ID="cvRepossession" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        function pageLoad() {
            tab = $find('ctl00_ContentPlaceHolder1_tcRepossession');
            var querymode = location.search.split("qsMode=");

            if (querymode.length > 1) {
                if (querymode[1].length > 1) {
                    querymode = querymode[1].split("&");
                    querymode = querymode[0];
                }
                else {
                    querymode = querymode[1];
                }
                if (querymode != 'Q') {
                    tab.add_activeTabChanged(on_Change);
                    var newindex = tab.get_activeTabIndex(index);
                    var btnSave = document.getElementById('<%=btnSave.ClientID %>')
                    if (newindex == tab._tabs.length - 1)
                        btnSave.disabled = false;
                    else
                        btnSave.disabled = true;
                }
            }

        }


        var index = 0;
        function on_Change(sender, e) {

            var newindex = tab.get_activeTabIndex();
            var btnSave = document.getElementById('<%=btnSave.ClientID %>')
                var btnclear = document.getElementById('<%=btnClear.ClientID %>')

                if (newindex == tab._tabs.length - 1)
                    btnSave.disabled = false;
                else
                    btnSave.disabled = true;
                if (newindex > index) {
                    switch (newindex) {
                        case 1:
                            if (!Page_ClientValidate('tbRepossession')) {
                                tab.set_activeTabIndex(0);
                                index = 0;

                                break;
                            }
                            break;

                    }
                }
                //       else
                //       {
                //            tab.set_activeTabIndex(newindex);
                //            index=tab.get_activeTabIndex(newindex);
                //       }     

                Page_BlockSubmit = false;

            }


            function trimStartingSpace(textbox) {
                var textValue = textbox.value;
                if (textValue.length == 0 && window.event.keyCode == 32) {
                    window.event.keyCode = 0;
                    return false;
                }
            }



    </script>

</asp:Content>
