<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLOANADAccountClosure_Add.aspx.cs" Inherits="LoanAdmin_S3GLOANADAccountClosure_Add"
    EnableEventValidation="false" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="CustomerDetails"
    TagPrefix="CD" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function EnableTabControl() {
            $find('ctl00_ContentPlaceHolder1_tcEnquiryAppraisal_tbAccDetails')._enabled = true;
            $find('ctl00_ContentPlaceHolder1_tcEnquiryAppraisal_tbCashFlow')._enabled = true;
            $find('ctl00_ContentPlaceHolder1_tcEnquiryAppraisal').set_activeTabIndex(1);
            $find('ctl00_ContentPlaceHolder1_tcEnquiryAppraisal').set_activeTabIndex(2);
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Rental Schedule Closure" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="margin-right: 2px; margin-left: 2px;">
                            <cc1:TabContainer ID="tcEnquiryAppraisal" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                Width="100%" TabStripPlacement="top">
                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                    BackColor="Red">
                                    <HeaderTemplate>
                                        Rental Schedule Closure
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td valign="top" colspan="4">
                                                    <div style="height: 1px;">
                                                        <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="None"
                                                            ControlToValidate="ddlLOB" ValidationGroup="btnSave" InitialValue="0" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Rental Schedule Closure No" ID="lblAccountClosureNo" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAccClosureNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                    <asp:HiddenField ID="hidAccClosureNo" runat="server"></asp:HiddenField>
                                                    <asp:HiddenField ID="hidClosureDetailId" runat="server" Value="0"></asp:HiddenField>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Rental Schedule Closure Date" ID="lblAccountClosureDate"
                                                        CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtAccClosureDate" AutoPostBack="true" OnTextChanged="txtAccClosureDate_TextChanged" runat="server"></asp:TextBox>
                                                     <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtAccClosureDate"
                                                        PopupButtonID="Image1" ID="CalendarExtender2" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvAccClosureDate" runat="server" Display="None"
                                                        ControlToValidate="txtAccClosureDate" ErrorMessage="Enter the Rental Schedule Closure Date"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="25%">
                                                    <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                                    <span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" width="27%">
                                                    <asp:DropDownList ID="ddlLOB" Width="165px" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                      <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMLA" runat="server" Text="Rental Schedule Number" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlMLA" runat="server" ServiceMethod="GetPANUM" AutoPostBack="true"
                                                        OnItem_Selected="ddlMLA_SelectedIndexChanged" />
                                                </td>
                                               
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblTrancheName" runat="server" Text="Tranche Name" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlTrancheName" runat="server" ServiceMethod="GetTrancheList" AutoPostBack="true"
                                                        OnItem_Selected="ddlTrancheName_SelectedIndexChanged" ValidationGroup="btnSave"
                                                        ErrorMessage="Enter a Tranche Name" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStatus" Text="Rental Schedule Closure Status" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            
                                          
                                            </tr>
                                          
                                        </table>
                                        <asp:Panel ID="Panel3" runat="server" GroupingText="Customer Information" CssClass="stylePanel">
                                            <CD:CustomerDetails ID="ucdCustomer" runat="server" ActiveViewIndex="1" FirstColumnWidth="23%"
                                                SecondColumnWidth="25%" ThirdColumnWidth="21%" FourthColumnWidth="23%" />
                                            <asp:HiddenField ID="hdnCustomerID" runat="server" />
                                            <table width="100%" border="0" style="display: none;">
                                                <tr>
                                                    <td class="styleFieldLabel" width="25%">
                                                        <asp:Label ID="lblCustCode" runat="server" Text="Customer Code"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign" width="27%">
                                                        <asp:TextBox ID="txtCustCode" ReadOnly="True" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td class="styleFieldLabel" width="23%">
                                                        <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMAddress" runat="server" Text="Address1" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMAddress" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="Label3" runat="server" Text="Address2" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMAddress2" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMCity" runat="server" Text="City" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMCity" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMState" runat="server" Text="State" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMState" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMCountry" runat="server" Text="Country" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMCountry" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMPincode" runat="server" Text="Pincode/Zipcode" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMPincode" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMMobile" runat="server" Text="Mobile" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMMobile" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblMEmailid" runat="server" Text="EMail Id" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtMEmailid" runat="server" ReadOnly="True"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbAccDetails" CssClass="tabpan"
                                    BackColor="Red" Enabled="false" Visible="false">
                                    <HeaderTemplate>
                                        Rental Schedule Details
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%" style="display: none;">
                                            <tr>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblAccDate" runat="server" Text="Account Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="27%">
                                                    <asp:TextBox ID="txtAccDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblMatureDate" runat="server" Text="Maturity Date" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMatureDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblPrincipal" runat="server" Text="Total Principal" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 283px">
                                                    <asp:TextBox ID="txtPrincipal" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblFinanceCharge" runat="server" Text="Total Finance Charge" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 283px">
                                                    <asp:TextBox ID="txtFinanceCharge" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblAccIRR" runat="server" Text="Account IRR" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtIRR" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblBusinessIRR" runat="server" Text="Business IRR" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 283px">
                                                    <asp:TextBox ID="txtBusinessIRR" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="23%" class="styleFieldLabel">
                                                    <asp:Label ID="lblCompanyIRR" runat="server" Text="Company IRR" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCompanyIRR" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" width="23%">
                                                    <asp:Label ID="lblFlatRate" runat="server" Text="Rate" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 283px">
                                                    <asp:TextBox ID="txtFlatRate" runat="server" ReadOnly="True" Style="text-align: right;"></asp:TextBox>
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
                                                    <asp:Label ID="lblMode" runat="server" Text="Repayment Mode" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="width: 283px">
                                                    <asp:TextBox ID="txtMode" runat="server" ReadOnly="True"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel runat="server" ID="Panel7" CssClass="stylePanel" GroupingText="Asset Details" Visible="false">
                                            <div id="div3" style="overflow: auto; width: 100%;" runat="server">
                                                <asp:GridView ID="grvAsset" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                    RowStyle-HorizontalAlign="Center" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSlNo" Visible="true" runat="server" Text='<%#Container.DataItemIndex+1 %> '
                                                                    Style="text-align: center;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetID" Visible="false" runat="server" Text='<%#Eval("Asset_ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Asset Description" DataField="ASSET_DESCription" ItemStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderText="Registration No">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRegNo" MaxLength="20" Text='<%#Eval("REGN_NUMBER")%>' Style="text-align: left;"
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel runat="server" ID="pnlAccount_Details" CssClass="stylePanel" GroupingText="Account Details">
                                                        <div id="divaccounts" style="overflow: auto; width: 100%;" runat="server">
                                                            <asp:GridView ID="grvaccount" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                                RowStyle-HorizontalAlign="Center" AutoGenerateColumns="False" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sl.No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSlNo" Visible="true" runat="server" Text='<%#Container.DataItemIndex+1 %> '
                                                                                Style="text-align: center;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rental Schedule Number">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRsno" MaxLength="20" Text='<%#Eval("RS_Number")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Accounting IRR">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblaccountIRR" MaxLength="20" Text='<%#Eval("Accounting_IRR")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Company IRR">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcompanyirr" MaxLength="20" Text='<%#Eval("Company_IRR")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Business IRR">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBussinessIRR" MaxLength="20" Text='<%#Eval("Business_IRR")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Maturity Date">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMatureDate" MaxLength="20" Text='<%#Eval("MatureDate")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" MaxLength="20" Text='<%#Eval("Status")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                <RowStyle HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel runat="server" ID="Panel1" CssClass="stylePanel" GroupingText="Account Balance">
                                            <div id="div1" style="overflow: auto; width: 100%;" runat="server">
                                                <asp:GridView ID="grvAccountBalance" runat="server" ShowFooter="True" AutoGenerateColumns="False" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Pasa_id" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblpasaid" MaxLength="20" Text='<%#Eval("pasa_id")%>' runat="server" Style="text-align: left;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rental Schedule Number">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPANum" Text='<%#Eval("panum")%>' runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Account Description">
                                                            <FooterTemplate>
                                                                <span>Total</span>
                                                            </FooterTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesc" MaxLength="20" Text='<%#Eval("Desc")%>' Style="text-align: left;"
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <FooterStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="Due">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDue" MaxLength="20" Text='<%#Eval("Due")%>' Style="text-align: right;"
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Received">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReceived" MaxLength="20" Text='<%#Eval("Received")%>' Style="text-align: right;"
                                                                    runat="server"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Outstanding">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOutstanding" MaxLength="20" Text='<%#Eval("Outstanding")%>' runat="server"
                                                                    Style="text-align: right;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OPC Fut Receivables" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblrec" MaxLength="20" Text='<%#Eval("OPC_fut_rec")%>' runat="server" Style="text-align: right;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fund Fut receivables" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfundrec" MaxLength="20" Text='<%#Eval("fund_fut_rec")%>' runat="server"
                                                                    Style="text-align: right;"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                    </Columns>
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
                                        <asp:Panel runat="server" ID="Panel2" Width="99%" CssClass="stylePanel" GroupingText="Cash Flow Details">
                                            <table width="100%">
                                                <tr>
                                                    <td class="styleFieldLabel" width="24%">
                                                        <asp:Label ID="lblClosureBy" runat="server" Text="Rental Schedule Closure Done By" CssClass="styleDisplayLabel"></asp:Label>
                                                        <span class="styleMandatory">*</span>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtClosureBy" runat="server" ReadOnly="true"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvClsoureBy" runat="server" Display="None" ControlToValidate="txtClosureBy"
                                                            ValidationGroup="btnSave" CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div id="div2" style="overflow-x: hidden; overflow-y: scroll; width: 100%; height: 300px"
                                                            runat="server">
                                                            <asp:GridView ID="grvCashFlow" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvCashFlow_RowDataBound"
                                                                ShowFooter="True">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Pasa_id" Visible="false">

                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpasaid" MaxLength="20" Text='<%#Eval("pasa_id")%>' runat="server" Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rental Schedule Number">

                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpanum" MaxLength="20" Text='<%#Eval("panum")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cash flow description">
                                                                        <FooterTemplate>
                                                                            <span>Total</span>
                                                                        </FooterTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCash" MaxLength="20" Text='<%#Eval("Description")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                            <asp:HiddenField ID="hdnCashFlowID" Value='<%#Eval("ID")%>' runat="server" />
                                                                            <%--<asp:HiddenField ID="hdnClosureID" Value='<%#Eval("Closure_Details_ID")%>' runat="server" />--%>
                                                                        </ItemTemplate>
                                                                        <FooterStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Due Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDue" MaxLength="20" Text='<%#Eval("Due")%>' runat="server" Style="text-align: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Waived Amount">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtWaived" MaxLength="10" Width="70px" OnTextChanged="txtWaived_TextChanged"
                                                                                AutoPostBack="true" Style="text-align: right; border-color: White;" onkeypress="fnAllowNumbersOnly(true,false,this);"
                                                                                Text='<%#Eval("Waived")%>' runat="server"></asp:TextBox>
                                                                            <%-- <cc1:FilteredTextBoxExtender ID="ftxtWaived" runat="server" FilterType="Numbers"
                                                                                TargetControlID="txtWaived">
                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Payable Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPayable" MaxLength="20" Text='<%#Eval("Payable")%>' runat="server"
                                                                                Style="text-align: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rental Schedule Closure Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblClosure" MaxLength="20" Text='<%#Eval("Closure")%>' runat="server"
                                                                                Style="text-align: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Received Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReceived" MaxLength="20" Text='<%#Eval("Received")%>' runat="server"
                                                                                Style="text-align: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remarks">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRemarks" MaxLength="100" TextMode="MultiLine" Wrap="true" onkeyup="maxlengthfortxt(100);"
                                                                                Rows="2" Columns="25" Text='<%#Eval("Remarks")%>' runat="server" Style="text-align: left; border-color: White;"
                                                                                AutoPostBack="True" OnTextChanged="txtRemarks_TextChanged"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="fRemarks" runat="server" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                                                                TargetControlID="txtRemarks" ValidChars=" ">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                <RowStyle HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Funder Details" ID="tbFunderDetails"
                                    CssClass="tabpan" BackColor="Red" Enabled="false">
                                    <HeaderTemplate>
                                        Funder Details
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel runat="server" ID="Panel5" CssClass="stylePanel" GroupingText="Funder Details">
                                                        <div id="div4" style="overflow: auto; width: 100%;" runat="server">
                                                            <asp:GridView ID="grvfunderdetailss" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                                RowStyle-HorizontalAlign="Center" AutoGenerateColumns="False" Width="100%">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sl.No.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSlNo" Visible="true" runat="server" Text='<%#Container.DataItemIndex+1 %> '
                                                                                Style="text-align: center;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Rental Schedule id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblRsnoid" MaxLength="20" Text='<%#Eval("Doc_id")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Document Number">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocno" MaxLength="20" Text='<%#Eval("Doc_no")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Due">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldue" MaxLength="20" Text='<%#Eval("Due")%>' runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Future Receivables" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFuture_receivables" MaxLength="20" Text='<%#Eval("Future_receivables")%>' runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="NPV Amount" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNPV_amount" MaxLength="20" Text='<%#Eval("NPV_amount")%>' runat="server"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>


                                                                </Columns>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                <RowStyle HorizontalAlign="Center" />
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;&nbsp;<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="styleSubmitButton"
                        OnClientClick="return fnCheckPageValidators();" ToolTip="Save" OnClick="btnSave_Click"
                        CausesValidation="true" ValidationGroup="btnSave" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                            ToolTip="Cancel" CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        <asp:Button ID="btnEmail" runat="server" Text="Email" Enabled="false" CssClass="styleSubmitButton" Style="display: none"
                            ToolTip="Email" OnClick="btnEmail_Click" />
                        &nbsp;<asp:Button ID="btnPrint" runat="server" Text="Print" Enabled="false" CssClass="styleSubmitButton"
                            ToolTip="Print" OnClick="btnPrint_Click" Visible="false"/>
                        &nbsp;<asp:Button ID="btnClosure" runat="server" Text="Closure Cancellation" CausesValidation="False"
                            ToolTip="Closure Cancellation" CssClass="styleSubmitButton" OnClick="btnClosure_Click"
                            Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                        <asp:CustomValidator ID="cvAccountClosure" runat="server" CssClass="styleMandatoryLabel"
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
            <%-- <asp:PostBackTrigger ControlID="btnSave" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
