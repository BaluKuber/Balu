<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLRRepossessionReleaseAdd.aspx.cs" Inherits="LegalRepossession_S3GLRRepossessionRelease"
    Title="Untitled Page" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpInsDetails" runat="server">
        <ContentTemplate>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" valign="top">
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
            </table>
            <cc1:TabContainer ID="tcRepossessionRelease" runat="server" Width="100%" CssClass="styleTabPanel"
                ActiveTabIndex="0" AutoPostBack="True">
                <cc1:TabPanel ID="tplRepossessionDetails" runat="server" CssClass="tabpan" TabIndex="0"
                    BackColor="Red" Width="100%" Enabled="true" HeaderText="Repossession Details">
                    <HeaderTemplate>
                        Repossession Details</HeaderTemplate>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td style="width: 30%" valign="top">
                                    <asp:Panel ID="pnlGeneralDetails" runat="server" CssClass="stylePanel" GroupingText="General Details"
                                        Width="100%">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 15%" align="left">
                                                    <asp:Label ID="lblLRN" runat="server" Text="LRN No" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 22%" align="left">
                                                    <asp:DropDownList ID="ddlLRN" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlLRN_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLRN" runat="server" ControlToValidate="ddlLRN"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a LRN Number" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 15%" align="left">
                                                    <asp:Label ID="lblRepoDocketNo" runat="server" Text="Docket Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 22%" align="left">
                                                    <asp:DropDownList ID="ddlDocketNo" runat="server" Width="165px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlDocketNo_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvDocketNo" runat="server" ControlToValidate="ddlDocketNo"
                                                        Display="None" InitialValue="0" ErrorMessage="Select a Docket Number" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblRRNo" runat="server" Text="RR No" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtRRNo" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblReleaseDate" runat="server" Text="Release Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtReleaseDate" runat="server"></asp:TextBox><cc1:CalendarExtender
                                                        ID="calReleaseDate" runat="server" Enabled="False" PopupButtonID="imgReleaseDate"
                                                        TargetControlID="txtReleaseDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:Image ID="imgReleaseDate" runat="server" Visible="False" ImageUrl="~/Images/calendaer.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblAction" runat="server" Text="Action" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtAction" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblRepoDate" runat="server" Text="Docket Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtRepoDate" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblLOB" runat="server" Text="Line of Bunsiness" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtLOB" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%; height: 26px;">
                                                    <asp:Label ID="lblBranch" runat="server" Text="Branch" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%; height: 26px;">
                                                    <asp:TextBox ID="txtBranch" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblPANum" runat="server" Text="Prime Account No" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtPANum" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblSANum" runat="server" Text="Sub Account No" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtSANum" runat="server"></asp:TextBox>
                                                    <asp:Button ID="btnRepo" runat="server" CssClass="styleSubmitLongButton" Text="View Repo Details"
                                                        Visible="False" Width="110px" OnClick="btnRepo_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblAmountFinanced" runat="server" Text="Amount Financed" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtAmountFinanced" runat="server" Style="text-align: right"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" style="width: 20%">
                                                    <asp:Label ID="lblTenure" runat="server" Text="Tenure"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" style="width: 30%">
                                                    <asp:TextBox ID="txtTenure" runat="server" Style="text-align: right"></asp:TextBox><asp:Button
                                                        ID="btnLedger" runat="server" CssClass="styleSubmitButton" Text="View Ledger"
                                                        Visible="False" Width="90px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td class="styleFieldLabel" style="width: 30%" valign="top">
                                    <asp:Panel ID="pnlCustomerDetails" runat="server" CssClass="stylePanel" GroupingText="Customer Details"
                                        Width="100%">
                                        <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                            SecondColumnStyle="styleFieldAlign" ShowCustomerName="true" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:Panel ID="pnlAssetDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Details"
                                                    Width="100%">
                                                    <asp:GridView ID="grvRepossessionAssetDetails" runat="server" AutoGenerateColumns="False"
                                                        EmptyDataText="Data Not Found" Width="99%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Asset Code">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetCode" runat="server" Text='<%#Bind("Asset_Code") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Asset Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetDesc" runat="server" Text='<%#Bind("Asset_Description") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Serial Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialNumber" runat="server" Text='<%#Bind("Serial_No") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Registration Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRegistrationNo" runat="server" Text='<%#Bind("REGN_No") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Repossession Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRepossessionDate" runat="server" Text='<%#Bind("Repossession_Date") %>'></asp:Label></ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
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
                <cc1:TabPanel ID="tplGuarantorDetails" Enabled="true" runat="server" CssClass="tabpan"
                    TabIndex="1" BackColor="Red" Width="100%" HeaderText="Guarantor Details">
                    <HeaderTemplate>
                        Guarantor Details</HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlGuarantor" runat="server" CssClass="stylePanel" GroupingText="Guarator Details"
                                        Width="100%">
                                        <asp:Label ID="lblEmpty" runat="server" Visible="False" Text="No Guarantor Details Available "></asp:Label><asp:GridView
                                            ID="grvGuarantor" runat="server" AutoGenerateColumns="False" Width="99%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Guarantor Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGuarantorCode" runat="server" Text='<%#Bind("Guarantor_Code") %>'></asp:Label></ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Guarantor Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGuarantorName" runat="server" Text='<%#Bind("Guarantor_Name") %>'></asp:Label></ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Guarantor Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGuarantorAddress" runat="server" TextMode="MultiLine" Text='<%#Bind("Guarantor_Address1") %>'
                                                            ReadOnly="true" BorderStyle="None" Width="200px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Guarantee Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGuaranteeAmount" runat="server" Text='<%#Bind("Guarantee_Amount") %>'></asp:Label></ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Right" Width="15%" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tplGarageDetails" Enabled="true" runat="server" CssClass="tabpan"
                    TabIndex="2" BackColor="Red" Width="100%" HeaderText="Garage Details">
                    <HeaderTemplate>
                        Garage Details</HeaderTemplate>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td valign="top">
                                    <asp:Panel ID="PnlGarageDetails" runat="server" CssClass="stylePanel" GroupingText="Garage Details">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGarageCode" runat="server" Text="Garage Code" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" rowspan="1">
                                                    <asp:TextBox ID="txtGarageCode" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAssetCondition" runat="server" Text="Condition of Asset" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtAssetCondition" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGarageAddress" runat="server" Text="Garage Address" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtGarageAddress" runat="server" Width="255px" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAssetInventory" runat="server" Text="Asset Inventory" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtAssetInventory" runat="server" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldAlign">
                                                    <asp:Label ID="lblInsuranceValidUpto" runat="server" Text="Insurance Validity Upto"
                                                        CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtInsuranceValidUpto" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                </td>
                                                <td class="styleFieldLabel">
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
            <table width="100%" align="center">
                <tr>
                    <td height="5px">
                    </td>
                </tr>
                <tr width="100%" align="center">
                    <td align="center" style="width: 100%">
                        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            Text="Release" OnClientClick="return fnCheckPageValidators('Submit');"  ValidationGroup="Submit" />
                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" OnClick="btnClear_Click"
                            Text="Clear" OnClientClick="return fnConfirmClear();" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="styleSubmitButton" Text="Cancel"
                            OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="vsSave" runat="server" CssClass="styleMandatoryLabel"
                ValidationGroup="Submit" HeaderText="Correct the following validation(s):" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
