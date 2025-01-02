<%@ Page Language="C#" MasterPageFile="~/Common/MasterPage.master" AutoEventWireup="true"
    CodeFile="S3GLRRepossession.aspx.cs" Inherits="LegalRepossession_S3GLRRepossession"
    Title="S3G-Repossession" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
    <%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
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
                    Width="99%" Visible="true">
                    <cc1:TabPanel runat="server" ID="tbRepossession" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Repossession
                        </HeaderTemplate>
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
                                                        Width="140px" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 25%">
                                                    <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Branch"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 25%">
                                                <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" IsMandatory="true" ValidationGroup="Submit" ErrorMessage="Enter Location" />
                                                   <%-- <asp:DropDownList OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ID="ddlBranch"
                                                        runat="server" AutoPostBack="True" Width="140px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select Line of Business"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ControlToValidate="ddlBranch"
                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select the Branch"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 25%">
                                                    <asp:Label ID="lblLRNNumber" runat="server" CssClass="styleReqFieldLabel" Text="LRN Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 25%">
                                                    <asp:DropDownList OnSelectedIndexChanged="ddlLRNNumber_SelectedIndexChanged" ID="ddlLRNNumber"
                                                        runat="server" AutoPostBack="True" Width="140px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLRNNo" runat="server" ControlToValidate="ddlLRNNumber"
                                                        CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select LRN Number"></asp:RequiredFieldValidator>
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
                                                <td class="styleFieldAlign" style="width: 25%">
                                                    <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active"
                                                        Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 25%">
                                                    <asp:CheckBox ID="ChkActive" runat="server" ToolTip="Select Asset" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel GroupingText="Customer Communication Address" ID="pnlCommAddress" runat="server"
                                            CssClass="stylePanel" Width="100%">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" Visible="False" ID="lblCustID"></asp:Label>
                                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                            FirstColumnWidth="25%" SecondColumnWidth="25%" ThirdColumnWidth="25%" FourthColumnWidth="25%"
                                                            ActiveViewIndex="1" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
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
                                                <td style="width: 25%">
                                                </td>
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
                                                            DataKeyNames="Asset_Number" ToolTip="Asset Details" Width="100%">
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
                                                                                    <asp:CheckBox ID="CbAssets" runat="server" ToolTip="Select Asset" AutoPostBack="true"
                                                                                        OnCheckedChanged="CbAssets_OnCheckedChanged" />
                                                                                    <asp:Label ID="lblAsset_ID" runat="server" Visible="false" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                    <HeaderTemplate>
                                                                        <table>
                                                                            <tr align="center">
                                                                                <td align="center">
                                                                                    Select Asset
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
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tbRepossessionDetails" CssClass="tabpan" BackColor="Red">
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
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Repossession Date"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:Label ID="lblPlace" runat="server" CssClass="styleReqFieldLabel" Text="Repossession Place"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign" style="width: 25%">
                                                                        <asp:TextBox ID="txtPlace" TextMode="MultiLine" MaxLength="200" onkeyup="maxlengthfortxt(200);"
                                                                            Width="180px" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvPlace" runat="server" ControlToValidate="txtPlace"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Place"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblPoliceStation" runat="server" CssClass="styleReqFieldLabel" Text="Whether Nearest Police Station has been Informed in Writing"></asp:Label>
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
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Present Condition of the Asset"></asp:RequiredFieldValidator>
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
                                                                            <asp:ListItem>--Select--</asp:ListItem>
                                                                            <asp:ListItem>Employee</asp:ListItem>
                                                                            <asp:ListItem>Agent</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvRepDoneBy" runat="server" ControlToValidate="ddlRepossessionDoneBy"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Repossession Done By"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:Label ID="lblEntity" runat="server" CssClass="styleReqFieldLabel" Text="Employee/Agent Code "></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:DropDownList ID="ddlAgentCode" runat="server" AutoPostBack="True" Width="140px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvAgentCode" runat="server" ControlToValidate="ddlAgentCode"
                                                                            CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select Employee/Agent Code"></asp:RequiredFieldValidator>
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
                                                                            CssClass="styleMandatoryLabel" Display="None" InitialValue="0" ErrorMessage="Select the Garage Code"></asp:RequiredFieldValidator>
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
                                                                                            FirstColumnWidth="22%" SecondColumnWidth="24%" ThirdColumnWidth="25%" FourthColumnWidth="29%" />
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
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Current Market Rate"></asp:RequiredFieldValidator>
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
                                                                        <asp:Label ID="lblRepoExp" runat="server" CssClass="styleReqFieldLabel" Text="Expenses Incurred During Repossession"></asp:Label>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                        <asp:TextBox ID="txtRepoExp" runat="server" MaxLength="300" onkeyup="maxlengthfortxt(300);"
                                                                            TextMode="MultiLine" Width="180px"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtRepoExp" runat="server" ControlToValidate="txtRepoExp"
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Expenses Incurred During Repossession"></asp:RequiredFieldValidator>
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
                                                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Asset Inventory Details "></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                    </td>
                                                                    <td class="styleFieldAlign">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
        <tr style="height: 10px">
            <td>
            </td>
        </tr>
        <tr>
            <td id="Td1" runat="server" align="center">
                <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                    OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                    CausesValidation="False" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                    Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsRepossession" runat="server" CssClass="styleMandatoryLabel"
                    HeaderText="Correct the following validation(s):  " />
                <asp:CustomValidator ID="cvRepossession" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
//       fntxtDate()
//       {
//            var txtGarageInDate = "ctl00_ContentPlaceHolder1_tcRepossession_tbRepossessionDetails_UpdatePanel1_txtGarageIn";
//            var txtRepossessionDate = "ctl00_ContentPlaceHolder1_tcRepossession_tbRepossessionDetails_UpdatePanel1_txtRepossessionDate";
//            txtGarageInDate = txtRepossessionDate.value;
//       }
       
    </script>

</asp:Content>
