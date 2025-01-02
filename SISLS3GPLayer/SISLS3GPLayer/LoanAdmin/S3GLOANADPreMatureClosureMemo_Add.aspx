<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLOANADPreMatureClosureMemo_Add.aspx.cs" Inherits="LoanAdmin_S3GLOANADPreMatureClosureMemo_Add"
    Title="Untitled Page" EnableEventValidation="false" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="CustomerDetails"
    TagPrefix="CD" %>
    <%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">
                        <div style="margin-right: 2px; margin-left: 2px;">
                            <cc1:TabContainer ID="tcPMCMemo" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                Width="100%" TabStripPlacement="top" AutoPostBack="True">
                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Pre Mature Closure
                                </HeaderTemplate>
                                    
<ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" style="display: none;">
                                                    <asp:Label runat="server" Text="Premature Closure No" ID="lblAccountClosureNo" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="display: none;">
                                                    <asp:TextBox ID="txtAccClosureNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:HiddenField ID="hidAccClosureNo" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hidClosureDetailId" runat="server" Value="0"></asp:HiddenField>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="PMC Memo Date" ID="lblAccountClosureDate" CssClass="styleDisplayLabel"></asp:Label><span
                                                        class="styleMandatory"> *</span>
                                                </td>
                                                <td class="styleFieldAlign" colspan="3">
                                                    <asp:TextBox ID="txtPMCReqDate" runat="server" AutoPostBack="True" OnTextChanged="txtPMCReqDate_TextChanged"></asp:TextBox>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtPMCReqDate"
                                                        PopupButtonID="Image1" ID="CalendarExtender2" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvAccClosureDate" runat="server" Display="None"
                                                        ControlToValidate="txtPMCReqDate" ErrorMessage="Enter the Premature Closure Date"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <div style="height: 1px;">
                                                        <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="None"
                                                            ControlToValidate="ddlLOB" ValidationGroup="btnSave" InitialValue="0" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator><br />
                                                      <%--  <asp:RequiredFieldValidator ID="rfvBranch" runat="server" Display="None" ControlToValidate="ddlBranch"
                                                            ValidationGroup="btnSave" InitialValue="0" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator><br />--%>
                                                        <asp:RequiredFieldValidator ID="rfvMLA" runat="server" Display="None" ControlToValidate="ddlMLA"
                                                            ValidationGroup="btnSave" InitialValue="0" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator><br />
                                                        <br />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label><span
                                                        class="styleMandatory"> *</span>
                                                </td>
                                                <td class="styleFieldAlign" width="27%">
                                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="169px" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label runat="server" ID="lblBranch" Text="Location" CssClass="styleDisplayLabel"></asp:Label><span
                                                        class="styleMandatory"> *</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                  <%--  <asp:DropDownList ID="ddlBranch" AutoPostBack="True" Width="169px" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                        runat="server">
                                                    </asp:DropDownList>--%>
                                                    <uc2:Suggest ID="ddlBranch" ValidationGroup="btnSave" runat="server" ToolTip="Location"
                                                                                ServiceMethod="GetBranchList" AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged"
                                                                                ErrorMessage="Select a Location" IsMandatory="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMLA" runat="server" Text="Prime Account Number" CssClass="styleDisplayLabel"></asp:Label><span
                                                        class="styleMandatory"> *</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <cc1:ComboBox ID="ddlMLA" runat="server" AutoPostBack="True" Width="139px" 
                                                    AutoCompleteMode="Append"  DropDownStyle="DropDownList"
                                                    OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged" CssClass="WindowsStyle">
                                                    </cc1:ComboBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblSLA" runat="server" Text="Sub Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlSLA" runat="server" AutoPostBack="True" Width="169px" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvSLA" Enabled="False" runat="server" ControlToValidate="ddlSLA"
                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="btnSave" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStatus" Text="Premature Closure Status" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="Panel3" runat="server" GroupingText="Customer Information" CssClass="stylePanel">
                                            <div style="margin-bottom: 10px;">
                                                <CD:CustomerDetails ID="ucdCustomer" runat="server" ActiveViewIndex="1" FirstColumnWidth="21%"
                                                    SecondColumnWidth="25%" ThirdColumnWidth="21%" FourthColumnWidth="25%" />
                                                <asp:HiddenField ID="hdnCustomerID" runat="server" />
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                
</cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbAccDetails" CssClass="tabpan"
                                    BackColor="Red" Enabled="false">
                                    <HeaderTemplate>
                                        Account Details
                                </HeaderTemplate>
                                    
<ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblPrincipal" runat="server" Text="Total Principal" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPrincipal" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>  
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblAccIRR" runat="server" Text="Accounting IRR" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtIRR" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>                                                                                          
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblFinanceCharge" runat="server" Text="Total Finance Charge" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 283px">
                                                    <asp:TextBox ID="txtFinanceCharge" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblCompanyIRR" runat="server" Text="Company IRR" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCompanyIRR" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>                                                
                                            </tr>
                                            <tr>
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblMatureDate" runat="server" Text="Maturity Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMatureDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblBusinessIRR" runat="server" Text="Business IRR" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtBusinessIRR" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblTenure" runat="server" Text="Tenure" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtTenure" runat="server" ReadOnly="True" Style="text-align: left;"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblFlatRate" runat="server" Text="Rate" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFlatRate" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="LblAccStatus" runat="server" Text="Status" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="TxtAccStatus" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblMode" runat="server" Text="Repayment Mode" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMode" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>                                                
                                                <td visible ="false" >
                                                    <asp:Label ID="lblAccDate" runat="server" Visible ="false" Text="Account Date"></asp:Label>
                                                </td>
                                                <td visible ="false">
                                                    <asp:TextBox ID="txtAccDate" Visible ="false" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel runat="server" ID="Panel7" CssClass="stylePanel" GroupingText="Asset Details">
                                            <div id="div3" style="overflow: auto; width: 100%;" runat="server">
                                                <asp:GridView ID="grvAsset" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                    RowStyle-HorizontalAlign="Center" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSlNo" Visible="true" runat="server" Text='<%#Container.DataItemIndex+1 %> '
                                                                    Style="text-align: center;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetID" Visible="false" runat="server" Text='<%#Eval("Asset_ID")%>'></asp:Label></ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Asset Description" DataField="ASSET_DESCription" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderText="Registration No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRegNo" MaxLength="20" Text='<%#Eval("REGN_NUMBER")%>' runat="server"
                                                                    Style="text-align: left;"></asp:Label></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="Panel1" CssClass="stylePanel" GroupingText="Account Balance">
                                            <div id="div1" style="overflow: auto; height: 200px; width: 100%" runat="server">
                                                <asp:GridView ID="grvAccountBalance" runat="server" ShowFooter="True" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Account Description">
                                                            <FooterTemplate>
                                                                <span>Total</span>
                                                            </FooterTemplate>                                                                                                                
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesc" MaxLength="20" Text='<%#Eval("Desc")%>' runat="server" Style="text-align: left;"></asp:Label></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Due">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDue" MaxLength="20" Text='<%#Eval("Due")%>' runat="server" Style="text-align: right;"></asp:Label></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Received">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReceived" MaxLength="20" Text='<%#Eval("Received")%>' runat="server"
                                                                    Style="text-align: right;"></asp:Label></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Outstanding">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOutstanding" MaxLength="20" Text='<%#Eval("Outstanding")%>' runat="server"
                                                                    Style="text-align: right;"></asp:Label></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                
</cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbCashFlow" CssClass="tabpan"
                                    BackColor="Red" Enabled="false">
                                    <HeaderTemplate>
                                        Cash Flow Details
                                </HeaderTemplate>
                                    
<ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblPreAmount" runat="server" Text="Preclosure Amount" CssClass="styleDisplayLabel"></asp:Label><span
                                                        class="styleMandatory"> *</span>
                                                    <asp:HiddenField ID="HidPMCStatus" Value="Request" runat="server" />
                                                    <asp:HiddenField ID="HidPMC_Receipt_Amount" Value="0" runat="server" />
                                                </td>
                                                <td class="styleFieldAlign" width="27%">
                                                    <asp:TextBox ID="txtPreAmount" runat="server" ReadOnly="True" Text="0" Style="text-align: right;"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvPreAmount" runat="server" Display="None" ControlToValidate="txtPreAmount"
                                                        ValidationGroup="btnSave" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel" width="24%" style="display: none;">
                                                    <asp:Label ID="lblClosureBy" runat="server" Text="Preclosure Done By" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" style="display: none;">
                                                    <asp:TextBox ID="txtClosureBy" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:RequiredFieldValidator Width="1px" ID="rfvClsoureBy" runat="server" Display="None"
                                                        ControlToValidate="txtClosureBy" ValidationGroup="btnSave" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblPreType" runat="server" Text="Preclosure Type" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlPreType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPreType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvPreType" runat="server" Display="None" ControlToValidate="ddlPreType"
                                                        ValidationGroup="btnSave" InitialValue="0" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblPreRate" runat="server" Text="Preclosure Rate" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPreRate" runat="server" MaxLength="7" ToolTip="Preclosure Rate"
                                                        AutoPostBack="True" Style="text-align: right;" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                        OnTextChanged="txtPreRate_TextChanged" />
                                                    <asp:RequiredFieldValidator ID="rfvPreRate" runat="server" Display="None" ControlToValidate="txtPreRate"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblPreCashDate" runat="server" Text="Preclosure Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPreclosureDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnIRR" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel runat="server" ID="Panel2" CssClass="stylePanel" GroupingText="Cash Flow Details"
                                            Width="99%">
                                            <%-- <div id="div2" style="overflow-x: hidden; overflow-y: scroll; width: 100%; height: 200px"
                                                runat="server">--%>
                                            <table width="100%">
                                                <tr>
                                                    <td width="100%">
                                                        <asp:GridView ID="grvCashFlow" Width="98%" runat="server" AutoGenerateColumns="False"
                                                            OnRowDataBound="grvCashFlow_RowDataBound" ShowFooter="True">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Cash flow description">
                                                                    <FooterTemplate>
                                                                        <span>Total</span>
                                                                    </FooterTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCash" MaxLength="20" Text='<%#Eval("Description")%>' runat="server"
                                                                            Style="text-align: left;"></asp:Label>
                                                                        <asp:HiddenField ID="hdnCashFlowID" Value='<%#Eval("ID")%>' runat="server" />
                                                                    </ItemTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Due Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDue" MaxLength="20" Text='<%#Eval("Due")%>' runat="server" Style="text-align: right;"></asp:Label></ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Waived Amount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtWaived" MaxLength="12" Width="100px" OnTextChanged="txtWaived_TextChanged"
                                                                            AutoPostBack="true" Style="text-align: right;border-color: White;" onkeypress="fnAllowNumbersOnly(true,false,this);"
                                                                            Text='<%#Eval("Waived")%>' runat="server"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Payable Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPayable" MaxLength="20" Text='<%#Eval("Payable")%>' runat="server"
                                                                            Style="text-align: right;"></asp:Label></ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Account Closure Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblClosure" MaxLength="20" Text='<%#Eval("Closure")%>' runat="server"
                                                                            Style="text-align: right;"></asp:Label></ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Received Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReceived" MaxLength="20" Text='<%#Eval("Received")%>' runat="server"
                                                                            Style="text-align: right;"></asp:Label></ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remarks" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemarks" Visible="false" MaxLength="100" TextMode="MultiLine"
                                                                            Wrap="true" onkeyup="maxlengthfortxt(100);" Rows="2" Columns="25" Text='<%#Eval("Remarks")%>'
                                                                            runat="server" Style="text-align: left;border-color: White;" AutoPostBack="True" OnTextChanged="txtRemarks_TextChanged"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="fRemarks" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                                                            TargetControlID="txtRemarks" ValidChars=" ">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%-- </div>--%></asp:Panel>
                                    </ContentTemplate>
                                
</cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        &nbsp;&nbsp;<asp:Button ID="btnSave" Text="Save" Visible="false" runat="server" CssClass="styleSubmitButton"
                            OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" CausesValidation="true"
                            ValidationGroup="btnSave" />
                        &nbsp;<asp:Button ID="btnEmail" runat="server" Text="Email" ValidationGroup="btnSave"
                            CssClass="styleSubmitButton" CausesValidation="true" OnClick="btnEmail_Click" />
                        &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" ValidationGroup="btnSave"
                            CssClass="styleSubmitButton" CausesValidation="true" OnClick="btnPrint_Click" />
                        &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            CausesValidation="false" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" Visible="false" />
                        &nbsp;<asp:Button ID="btnClosure" runat="server" Text="Closure Cancellation" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnClosure_Click" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                        <asp:CustomValidator ID="cvAccountPreClosure" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%">
                        </asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <asp:Button ID="Button1" runat="server" Text="Closure Cancellation" CausesValidation="false"
                OnClick="Button1_Click" Style="display: none;" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="Button1" />
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
