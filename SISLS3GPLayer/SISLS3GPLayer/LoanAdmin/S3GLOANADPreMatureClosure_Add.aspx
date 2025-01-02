<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLOANADPreMatureClosure_Add.aspx.cs" Inherits="LoanAdmin_S3GLOANADPreMatureClosure_Add"
    Title="Untitled Page" EnableEventValidation="false" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="CustomerDetails"
    TagPrefix="CD" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function FunRestIrr() {
            document.getElementById('<%=hdnIRR.ClientID %>').value = 0;
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
                                    <asp:Label runat="server" Text="Enquiry Appraisal" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px;">
                        <div style="margin-right: 2px; margin-left: 2px;">
                            <cc1:TabContainer ID="tcEnquiryAppraisal" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                                Width="100%" TabStripPlacement="top">

                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                    BackColor="Red">

                                    <HeaderTemplate>
                                        Pre Mature Closure
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Text="Premature Closure No" ID="lblAccountClosureNo" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtAccClosureNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                            <asp:HiddenField ID="hidAccClosureNo" runat="server"></asp:HiddenField>
                                                            <asp:HiddenField ID="hidClosureDetailId" runat="server" Value="0"></asp:HiddenField>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" Text="Premature Closure Date" ID="lblAccountClosureDate"
                                                                CssClass="styleDisplayLabel"></asp:Label><span class="styleMandatory"> *</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
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
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblStatus" Text="Premature Closure Status" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <%--   <td class="styleFieldLabel" width="23%" >
                                                    <asp:Label runat="server" ID="lblBranch" Text="Location " CssClass="styleDisplayLabel"></asp:Label><span
                                                        class="styleMandatory"> *</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                     <uc2:Suggest ID="ddlBranch" ValidationGroup="btnSave" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                                                    AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" ErrorMessage="Select a Location"
                                                    IsMandatory="true" />
                                                </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblTranche" runat="server" Text="Tranche Name" CssClass="styleDisplayLabel"></asp:Label><span
                                                                class="styleMandatory"> *</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <uc2:Suggest ID="uctranche" runat="server" ToolTip="Tranche Name" ServiceMethod="GetTrancheDetails"
                                                                AutoPostBack="true" OnItem_Selected="ddlMLA_SelectedIndexChanged" />
                                                        </td>

                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblMLA" runat="server" Text="Rental Schedule Number" CssClass="styleDisplayLabel"></asp:Label><span
                                                                class="styleMandatory"> *</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <uc2:Suggest ID="ddlMLA" ValidationGroup="btnSave" runat="server" ToolTip="Prime Account Number" ServiceMethod="GetPanumList"
                                                                AutoPostBack="true" OnItem_Selected="ddlMLA1_SelectedIndexChanged" />
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
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>


                                <cc1:TabPanel runat="server" HeaderText="General" ID="tbCashFlow" CssClass="tabpan"
                                    BackColor="Red" Enabled="false">
                                    <HeaderTemplate>
                                        Lessee Dues
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
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
                                                        <td class="styleFieldLabel" width="24%">
                                                            <asp:Label ID="lblClosureBy" runat="server" Text="Preclosure Done By" CssClass="styleDisplayLabel"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtClosureBy" runat="server" ReadOnly="True"></asp:TextBox>
                                                            <asp:RequiredFieldValidator Width="1px" ID="rfvClsoureBy" runat="server" Display="None"
                                                                ControlToValidate="txtClosureBy" ValidationGroup="btnSave" CssClass="styleMandatoryLabel"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
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
                                                        <td class="styleFieldLabel" width="23%">
                                                            <asp:Label ID="lblPreRate" runat="server" Text="Foreclosure Rate" CssClass="styleDisplayLabel"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtPreRate" runat="server" MaxLength="7" ToolTip="Foreclosure Rate "
                                                                Style="text-align: right;" onkeypress="fnAllowNumbersOnly(true,false,this)" onchange="FunRestIrr();" />
                                                            <asp:RequiredFieldValidator ID="rfvPreRate" runat="server" Display="None" InitialValue="" ControlToValidate="txtPreRate"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="23%" class="styleFieldLabel">
                                                            <asp:Label ID="Label1" runat="server" Text="Funder Preclosure Date" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtFunderdate" runat="server" onchange="FunRestIrr();"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                                TargetControlID="txtFunderdate">
                                                            </cc1:CalendarExtender>
                                                            <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                        </td>

                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblDiscountrate" runat="server" Text="Break-Up Cost" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:TextBox ID="txtDiscRate" runat="server" MaxLength="7" ToolTip="Break-Up Cost" onchange="FunRestIrr();"
                                                                AutoPostBack="True" Style="text-align: right;" ValidationGroup="btnnpv" onkeypress="fnAllowNumbersOnly(true,false,this)" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="23%">
                                                            <asp:Label ID="lblFunderPreRate" runat="server" Text="Funder Preclosure Rate" CssClass="styleDisplayLabel"></asp:Label>
                                                            <span class="styleMandatory">*</span>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtfunderprerate" runat="server" MaxLength="7" onchange="FunRestIrr();" ToolTip="Funder Preclosure Rate"
                                                                Style="text-align: right;" onkeypress="fnAllowNumbersOnly(true,false,this)" />
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkAMF" runat="server" Checked="true" onchange="FunRestIrr();" Text="AMF" />
                                                                        <asp:CheckBox ID="chkVAT" runat="server" Checked="true" onchange="FunRestIrr();" Text="VAT" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkServiceTax" runat="server" Checked="true" onchange="FunRestIrr();" Text="Service Tax" />
                                                            <asp:CheckBox ID="chkwaived" runat="server" Text="Waive All" AutoPostBack="true" OnCheckedChanged="chkwaived_CheckedChanged" />
                                                        </td>

                                                    </tr>
                                                    <tr>

                                                        <td width="23%" class="styleFieldLabel">

                                                            <asp:Label ID="lblPreCashDate" runat="server" Text="Preclosure Date" Style="display: none" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtPreclosureDate" runat="server" onchange="FunRestIrr();" Style="display: none"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtAccountDate" runat="server" ValidChars="/-"
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                                TargetControlID="txtPreclosureDate">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <cc1:CalendarExtender ID="CalendarExtenderTranchedate" runat="server" Enabled="True"
                                                                TargetControlID="txtPreclosureDate">
                                                            </cc1:CalendarExtender>
                                                            <asp:HiddenField ID="hdnIRR" runat="server" />
                                                        </td>
                                                        <%--<td width="23%" class="styleFieldLabel"></td>--%>
                                                        <td class="styleFieldAlign">
                                                            <asp:Label ID="lblNOCDate" runat="server" Text="NOC Date" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtNOCDate" runat="server" onchange="FunRestIrr();" Visible="false"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True"
                                                                TargetControlID="txtNOCDate">
                                                            </cc1:CalendarExtender>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCalculateNPV" runat="server" Text="Calculate NPV" CssClass="styleSubmitButton" ValidationGroup="btnnpv" OnClick="btnCalculateNPV_OnClick" />
                                                            <asp:Button ID="btnExcel" runat="server" Text="Excel PV" CssClass="styleSubmitButton" OnClick="btnExport_Click" />
                                                        </td>

                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel runat="server" ID="Panel2" CssClass="stylePanel" Visible="false" GroupingText="Lessee-Company"
                                                                Width="99%" Height="100%">

                                                                <div id="div2" style="overflow-x: hidden; overflow-y: scroll; display: none; width: 99%; height: 200px"
                                                                    runat="server" class="container">

                                                                    <asp:GridView ID="grvCashFlow" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvCashFlow_RowDataBound"
                                                                        ShowFooter="True" Width="98%" HeaderStyle-CssClass="header">
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
                                                                                    <asp:Label ID="lblpanum" Text='<%#Eval("panum")%>' runat="server"
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
                                                                                    <asp:Label ID="lblCash" Text='<%#Eval("Description")%>' runat="server"
                                                                                        Style="text-align: left;"></asp:Label>
                                                                                    <asp:HiddenField ID="hdnCashFlowID" Value='<%#Eval("ID")%>' runat="server" />
                                                                                    <asp:HiddenField ID="hdnRSTypeID" Value='<%#Eval("RSType_ID")%>' runat="server" />
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
                                                                            <asp:TemplateField HeaderText="PV Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPayable" Width="100%" Text='<%#Eval("PV")%>' runat="server" Style="text-align: right;"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Waived Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtWaived" OnTextChanged="txtWaived_TextChanged"
                                                                                        AutoPostBack="true" Style="text-align: right; border-color: White;" onkeypress="fnAllowNumbersOnly(true,false,this);"
                                                                                        Text='<%#Eval("Waived")%>' runat="server"></asp:TextBox>
                                                                                    <%-- <cc1:FilteredTextBoxExtender ID="ftxtWaived" runat="server" FilterType="Numbers"
                                                                                TargetControlID="txtWaived">
                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                                                    <asp:HiddenField ID="hdnIs_Due" Value='<%#Eval("Is_Due")%>' runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" Width="30px" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Account Closure Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblClosure" MaxLength="20" Text='<%#Eval("Closure")%>' runat="server"
                                                                                        Style="text-align: right;"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                        <%--<HeaderStyle CssClass="styleGridHeader" />--%>
                                                                        <RowStyle HorizontalAlign="Center" />
                                                                    </asp:GridView>

                                                                </div>
                                                            </asp:Panel>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel runat="server" ID="Panel5" CssClass="stylePanel" GroupingText="Funder Details">
                                                                <div id="div4" style="overflow: auto; width: 100%;" runat="server">
                                                                    <asp:GridView ID="grvfunderdetailss" runat="server" HeaderStyle-CssClass="styleGridHeader"
                                                                        RowStyle-HorizontalAlign="Center" AutoGenerateColumns="False" Width="100%" ShowFooter="true">
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
                                                                                    <asp:Label ID="lblRsnoid" MaxLength="20" Text='<%#Eval("pa_sa_ref_id")%>' runat="server"
                                                                                        Style="text-align: left;"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Rental Schedule Number">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDocno" MaxLength="20" Text='<%#Eval("panum")%>' runat="server"
                                                                                        Style="text-align: left;"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <span>Total</span>
                                                                                </FooterTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Due">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lbldue" MaxLength="20" Text='<%#Eval("Due")%>' runat="server"
                                                                                        Style="text-align: left;"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Future Receivables">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFuture_receivables" MaxLength="20" Text='<%#Eval("Future_receivables")%>' runat="server"
                                                                                        Style="text-align: left;"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="NPV Amount">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblNPV_amount" MaxLength="20" Text='<%#Eval("NPV_amount")%>' runat="server"
                                                                                        Style="text-align: left;"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>


                                                                        </Columns>
                                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                                        <RowStyle HorizontalAlign="Center" />
                                                                        <FooterStyle HorizontalAlign="Right" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr align="right">
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblbreaking" runat="server" CssClass="styleDisplayLabel" Text="Funder Breaking Charges"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtbreaking" runat="server" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                            OnTextChanged="txtbreaking_OnTextChanged" AutoPostBack="true" Style="text-align: right"></asp:TextBox>
                                                                        <%-- <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" ValidChars="."
                                                                                FilterType="Numbers" Enabled="True"
                                                                                TargetControlID="txtbreaking">
                                                                            </cc1:FilteredTextBoxExtender>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblTotal" runat="server" CssClass="styleDisplayLabel" Text="Total"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtToTal" runat="server" Style="text-align: right" ReadOnly="true"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnExcel" />
                                            </Triggers>
                                        </asp:UpdatePanel>

                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" HeaderText="Asset Details" ID="tbReceipt"
                                    CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Asset Details
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel runat="server" ID="Panel4" CssClass="stylePanel" GroupingText="Rental Schedule Details">
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
                                                                    <asp:TemplateField HeaderText="pasa_id" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblpasaid" MaxLength="20" Text='<%#Eval("pasa_id")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Accounting IRR">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblaccountIRR" MaxLength="20" Text='<%#Eval("Accounting_IRR")%>' runat="server"
                                                                                Style="text-align: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Company IRR">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcompanyirr" MaxLength="20" Text='<%#Eval("Company_IRR")%>' runat="server"
                                                                                Style="text-align: right;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Business IRR">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblBussinessIRR" MaxLength="20" Text='<%#Eval("Business_IRR")%>' runat="server"
                                                                                Style="text-align: right;"></asp:Label>
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
                                            <tr>
                                                <td>
                                                    <asp:Panel runat="server" ID="Panel6" CssClass="stylePanel" GroupingText="Asset Details" Visible="false">
                                                        <div id="div5" style="overflow: auto; width: 100%;" runat="server">
                                                            <asp:GridView ID="grvasset1" runat="server" HeaderStyle-CssClass="styleGridHeader"
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
                                                                            <asp:Label ID="lblRsnoid" MaxLength="20" Text='<%#Eval("pasa_id")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Account Number">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDocno" MaxLength="20" Text='<%#Eval("panum")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Invoice Number">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldue" MaxLength="20" Text='<%#Eval("INV_No")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Asset Description">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblFuture_receivables" MaxLength="20" Text='<%#Eval("decription")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="left" />
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Total Bill Amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblNPV_amount" MaxLength="20" Text='<%#Eval("total_bill_amount")%>' runat="server"
                                                                                Style="text-align: left;"></asp:Label>
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
                    <td align="center">&nbsp;&nbsp;<asp:Button ID="btnSave" Text="Save" runat="server" CssClass="styleSubmitButton"
                        OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" CausesValidation="true"
                        ValidationGroup="btnSave" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        &nbsp;<asp:Button ID="btnEmail" runat="server" Text="Email" Visible="false" CssClass="styleSubmitButton"
                            CausesValidation="False" OnClick="btnEmail_Click" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" Enabled="false" CssClass="styleSubmitButton"
                            CausesValidation="False" OnClick="btnPrintRental_Click" Visible="false" />
                        <asp:Button ID="btnPrintST" runat="server" Text="Print" Enabled="false" CssClass="styleSubmitButton"
                            CausesValidation="False" OnClick="btnPrintAMF_Click" />
                        &nbsp;<asp:Button ID="btnexportann" runat="server" Text="Print Annexures" Enabled="false" CssClass="styleSubmitButton"
                            CausesValidation="False" OnClick="btnPrintAnnexures_Click" />
                        &nbsp;<asp:Button ID="btnClosure" runat="server" Text="Closure Cancellation" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnClosure_Click" Visible="false" />
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
            <asp:PostBackTrigger ControlID="btnexportann" />
            <asp:PostBackTrigger ControlID="btnPrintST" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


