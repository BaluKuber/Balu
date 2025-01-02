<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLoanAdAccountSpecificRevision.aspx.cs"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="LoanAdmin_S3GLoanAdAccountSpecificRevision" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblAccountSpecificRevision" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcSpecificRevision" runat="server" ActiveTabIndex="2" CssClass="styleTabPanel"
                            Width="100%" TabStripPlacement="top" OnActiveTabChanged="tcSpecificRevision_ActiveTabChanged">
                            <cc1:TabPanel runat="server" HeaderText="General" ID="tbgeneral" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Revision Header</HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlHeaderInfo" GroupingText="Revision Header Information" CssClass="stylePanel"
                                                                runat="server">
                                                                <table width="100%" align="center">
                                                                    <tr>
                                                                        <td width="20%" class="styleFieldLabel">
                                                                            <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                                        </td>
                                                                        <td width="25%" class="styleFieldLabel">
                                                                            <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" />
                                                                            <asp:RequiredFieldValidator ID="RFVddlLOB" runat="server" ControlToValidate="ddlLOB"
                                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td align="left" width="20%" class="styleFieldLabel">
                                                                            <asp:Label ID="lblBranchMain" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldLabel" width="35%">
                                                                           <%-- <asp:DropDownList ID="ddlBranchMain" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranchMain_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBranchMain"
                                                                                Display="None" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                                 <uc2:Suggest ID="ddlBranchMain" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        ErrorMessage="Select a Location" IsMandatory="true" OnItem_Selected="ddlBranchMain_SelectedIndexChanged" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" width="20%">
                                                                            <asp:Label CssClass="styleReqFieldLabel" runat="server" Text="Prime Account Number"
                                                                                ID="lblMLA"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                          <uc2:Suggest ID="ddlMLA" runat="server" ServiceMethod="GetPANUM" AutoPostBack="true" IsMandatory="true" OnItem_Selected="ddlMLA_SelectedIndexChanged"
                                                                                        ErrorMessage="Enter a Prime Account Number"/>
                                                                            <%--<asp:DropDownList ID="ddlMLA" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator SetFocusOnError="True" Display="None" ID="RequiredFieldValidator2"
                                                                                CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ddlMLA" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Sub Account Number" ID="lblSLA" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlSLA" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:CustomValidator ID="CVSLA" runat="server" ControlToValidate="ddlSLA" Display="None"
                                                                                ErrorMessage="Select SLA" OnServerValidate="CVSLA_ServerValidate" SetFocusOnError="True"></asp:CustomValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Account Revision No." ID="lblNumber" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtNumber" Enabled="False" runat="server" MaxLength="15" Width="72%"></asp:TextBox>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Account Revision Date" ID="lblDate" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtDate" Enabled="False" Width="30%" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Effective From" ID="lblEffectiveFrom" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtEffectiveFrom" Visible = "false" runat="server" ContentEditable="False" Width="45%"
                                                                                AutoPostBack="True" CausesValidation="false" OnTextChanged="txtEffectiveFrom_TextChanged"></asp:TextBox>
                                                                                <asp:DropDownList ID = "ddlEffectiveFrom" CausesValidation="true" runat = "server" Width = "60%" ></asp:DropDownList>
                                                                            &nbsp;
                                                                           <asp:Image ID="imgEffectiveFrom" runat="server" Visible="false" ImageUrl="~/Images/calendaer.gif"
                                                                                ImageAlign="AbsMiddle" />
                                                                            <cc1:CalendarExtender ID="CalendarExtenderToDate" runat="server" Enabled="True" PopupButtonID="imgEffectiveFrom"
                                                                                TargetControlID="txtEffectiveFrom">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator SetFocusOnError="True" Display="None" ID="RequiredFieldValidator4"
                                                                                CssClass="styleMandatoryLabel" runat="server"  InitialValue="0" ControlToValidate="ddlEffectiveFrom"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Status" ID="lblStatus" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlRevisionStatus" runat="server" Enabled="False" Width="45%">
                                                                                <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                                                <asp:ListItem Value="1">Pending</asp:ListItem>
                                                                                <asp:ListItem Value="2">Under Process</asp:ListItem>
                                                                                <asp:ListItem Value="3">Approved</asp:ListItem>
                                                                                <asp:ListItem Value="4">Rejected</asp:ListItem>
                                                                                <asp:ListItem Value="5">Canceled</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator10" CssClass="styleMandatoryLabel"
                                                                                InitialValue="0" runat="server" SetFocusOnError="True" ControlToValidate="ddlRevisionStatus"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Finance Amount" ID="lblFinAmt" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtFinAmt" runat="server" MaxLength="15" ReadOnly="True" Width="45%"
                                                                                CssClass="txtRightAlign"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator CssClass="styleReqFieldLabel" Display="None" ID="RequiredFieldValidator3"
                                                                                runat="server" ControlToValidate="txtFinAmt" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblFinAmt0" runat="server" CssClass="styleDisplayLabel" Text="Account Creation Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:TextBox ID="txtACDate" runat="server" Width="30%" ContentEditable="False"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                       <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Text="Revision Fee" ID="lblRevisionFee" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtRevisionFee" runat="server" MaxLength="15"  Width="45%"
                                                                                CssClass="txtRightAlign"></asp:TextBox>
                                                                            <%--<asp:RequiredFieldValidator CssClass="styleReqFieldLabel" Display="None" ID="RequiredFieldValidator5"
                                                                                runat="server" ControlToValidate="txtFinAmt" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" Visible="false" Text="Principal O/S Amount" ID="lblPlAmt" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtPLAmt" Visible="false" runat="server" MaxLength="15" ReadOnly="True" Width="45%"
                                                                                CssClass="txtRightAlign"></asp:TextBox>
                                                                            <%--<asp:RequiredFieldValidator CssClass="styleReqFieldLabel" Display="None" ID="RequiredFieldValidator5"
                                                                                runat="server" ControlToValidate="txtFinAmt" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlCustomerInfo0" runat="server" CssClass="stylePanel" GroupingText="Customer Information">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td height="5px">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" ActiveViewIndex="1"
                                                                                FirstColumnStyle="styleFieldLabel" FirstColumnWidth="20%" FourthColumnWidth="34%"
                                                                                SecondColumnWidth="26%" ThirdColumnWidth="20%" />
                                                                        </td>
                                                                        <td height="5px">
                                                                            &nbsp;
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
                                            <td align="center">
                                                <asp:Button ID="btnGO" CssClass="styleSubmitButton" runat="server" Text="GO" OnClick="btnGO_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="vs_TabMainPage" runat="server" CssClass="styleMandatoryLabel"
                                                                HeaderText="Please correct the following validation(s):  " />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tbExisting" runat="server">
                                <HeaderTemplate>
                                    Existing
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="85%">
                                        <tr>
                                            <td valign="top" width="75%">
                                                <asp:GridView ID="grvAccountRevisionDetails" runat="server" AutoGenerateColumns="False"
                                                    OnRowDataBound="grvAccountRevisionDetails_RowDataBound" Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTypeDetails" runat="server" Text='<%#Eval("Type")%>'> </asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Width="25%" />
                                                            <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Existing">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExistingdetails" runat="server" Text='<%#Eval("Existing")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" Width="25%" />
                                                            <ItemStyle HorizontalAlign="Right" Width="25%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Additional Revision">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="litDescription" Text="" runat="server"></asp:Literal>
                                                                <asp:TextBox ID="txtRevisedValue" runat="server" Style="text-align: right;" Text='<%#Eval("Revised")%>'
                                                                    CausesValidation="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <ItemStyle HorizontalAlign="Right" Width="50%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <RowStyle HorizontalAlign="Center" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" width="75%">
                                                <table>
                                                    <tr>
                                                        <td align="left" width="25%">
                                                            <asp:Label runat="server" Text="Principal O/S Amount" ID="lblPriAmt" CssClass="styleReqFieldLabel"></asp:Label>
                                                        </td>
                                                        <td align="left" width="25%">
                                                            <asp:TextBox ID="txtPriAmt" runat="server" MaxLength="15" ReadOnly="True" width="100px"
                                                                CssClass="txtRightAlign"></asp:TextBox>
                                                        </td>
                                                        <td align="right" width="50%">
                                                            <asp:Button ID="btnGenerateRevision" CssClass="styleSubmitButton" runat="server"
                                                                Text="Generate Revision" OnClick="GenerateSpecificRevisionDetails" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="50%">
                                                <asp:Panel ID="pnlRevisedDetails" runat="server" GroupingText="Existing/Revised Details"
                                                    CssClass="stylePanel">
                                                    <table width="100%" style="height: 100px" cellpadding="5%">
                                                        <tr>
                                                            <td valign="top" width="33%">
                                                                <!-- Existing Stock on Hire -->
                                                                <asp:GridView ID="gvExistingSOH" runat="server" AutoGenerateColumns="False" Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSOH" runat="server" Text='<%#Eval("Type")%>' Width="100%"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" Width="30%" Height="22px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Existing">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSOHExisting" runat="server" Text='<%#Eval("Existing")%>' Width="100%"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Right" Width="35%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Revised">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblSOHRevised" runat="server" Style="text-align: right;" Text='<%#Eval("Revised")%>'
                                                                                    Width="90%"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="35%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle HorizontalAlign="Center" />
                                                                </asp:GridView>
                                                            </td>
                                                            <td valign="top" width="33%">
                                                                <asp:GridView ID="grvAccountRevisionIRRDetails" runat="server" AutoGenerateColumns="False"
                                                                    Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblType0" runat="server" Text='<%#Eval("Type")%>' Width="100%"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" Width="30%" Height="22px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Existing">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblExisting0" runat="server" Text='<%#Eval("Existing")%>' Width="100%"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Right" Width="35%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Revised">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRevised0" runat="server" Style="text-align: right;" Text='<%#Eval("Revised")%>'
                                                                                    Width="90%"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="35%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle HorizontalAlign="Center" />
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="tbRevisedRepayment" runat="server">
                                <HeaderTemplate>
                                    Repayment Structure
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%" cellpadding="5%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlRepayAuto" runat="server">
                                                    <table>
                                                        <!-- Existing -->
                                                        <td valign="top" width="50%">
                                                            <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" GroupingText="Repayment Structure (Existing)">
                                                                <div style="width: 100%; overflow: scroll; overflow-x: hidden;">
                                                                    <asp:GridView ID="grvBill" Width="90%" runat="server" BorderWidth="1px" Align="center"
                                                                        AutoGenerateColumns="False" AllowPaging="true" OnPageIndexChanging="grvBill_PageIndexChanging">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="InstallmentNo" HeaderText="Installment Number">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Installment Date">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"InstallmentDate")).ToString(DateFormate) %> '></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                        <!-- Revised -->
                                                        <td valign="top" width="50%">
                                                            <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" GroupingText="Repayment Structure (Revised)">
                                                                <div style=" width: 100%; overflow: scroll; overflow-x: hidden; top: 0">
                                                                    <asp:GridView ID="grvBill2" Width="90%" Align="center" runat="server" AutoGenerateColumns="False" AllowPaging="true" OnPageIndexChanging="grvBill2_PageIndexChanging">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="InstallmentNo" HeaderText="Installment Number">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Installment Date">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"InstallmentDate")).ToString(DateFormate) %> '></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlRepayManual" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td valign="top" width="50%">
                                                            <div style="height: 450px; width: 100%; overflow: scroll; overflow-x: hidden; top: 0">
                                                                <asp:GridView ID="gvManualExisting" Width="90%" runat="server" BorderWidth="0px"
                                                                    Align="center" AutoGenerateColumns="False">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="InstallmentNo" HeaderText="Installment Number">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Installment Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"InstallmentDate")).ToString(DateFormate) %> '></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount">
                                                                            <HeaderStyle HorizontalAlign="Left" />
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                </div>
                                                            </td>
                                                             <!-- Revised -->
                                                            <td valign="top" width="50%">
                                                                <div style="height: 450px; width: 100%; overflow: scroll; overflow-x: hidden; top: 0">
                                                                    <asp:GridView ID="GrvSARevised" Width="90%" Align="center" runat="server" 
                                                                        AutoGenerateColumns="False" Visible="False">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="InstallmentNo" HeaderText="Installment Number">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Installment Date">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"InstallmentDate")).ToString(DateFormate) %> '></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount">
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                           <td colspan="2"></td>
                                                        </tr> 
                                                        <tr>
                                                            <td colspan="2">
                                                                <!-- Structure Adhoc -->
                                                                <asp:GridView ID="gvRepaymentDetails" runat="server" AutoGenerateColumns="False"
                                                                    ShowFooter="True" OnRowDeleting="gvRepaymentDetails_RowDeleting" OnRowDataBound="gvRepaymentDetails_RowDataBound"
                                                                    OnRowCreated="gvRepaymentDetails_RowCreated" Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="slno" HeaderText="Sl.No">
                                                                            <FooterStyle Width="2%" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Repayment CashFlow">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCashFlow" runat="server" Text='<%# Bind("CashFlow") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlRepaymentCashFlow_RepayTab" runat="server" Width="98%" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvddlRepaymentCashFlow_RepayTab" runat="server"
                                                                                    ControlToValidate="ddlRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                                    Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" InitialValue="0"
                                                                                    ErrorMessage="Select a Repayment cashflow"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                            <FooterStyle Width="23%" />
                                                                            <ItemStyle Width="23%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Per Installment Amount">
                                                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                                            <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblPerInstallmentAmount_RepayTab" runat="server" Text='<%# Bind("PerInstall") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtPerInstallmentAmount_RepayTab" runat="server" Width="95%" Style="text-align: right;"
                                                                                    MaxLength="7">
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                                    FilterType="Numbers" TargetControlID="txtPerInstallmentAmount_RepayTab">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                                    ControlToValidate="txtPerInstallmentAmount_RepayTab" CssClass="styleMandatoryLabel"
                                                                                    Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the Pre installment amount"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Breakup Percentage">
                                                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                                            <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBreakup_RepayTab" runat="server" Text='<%# Bind("Breakup") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtBreakup_RepayTab" runat="server" Width="95%" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                                                </asp:TextBox>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="From Installment">
                                                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                                            <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblFromInstallment_RepayTab" runat="server" Text='<%# Bind("FromInstall") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtFromInstallment_RepayTab" runat="server" Width="95%" MaxLength="3"
                                                                                    Style="text-align: right" Text="1"> 
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtFromInstallment_RepayTab" runat="server"
                                                                                    FilterType="Numbers" TargetControlID="txtFromInstallment_RepayTab">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="To Installment">
                                                                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                                            <FooterStyle HorizontalAlign="Right" Width="10%" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblToInstallment_RepayTab" runat="server" Text='<%# Bind("ToInstall") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtToInstallment_RepayTab" runat="server" Width="95%" MaxLength="3"
                                                                                    Style="text-align: right">
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtToInstallment_RepayTab" runat="server"
                                                                                    FilterType="Numbers" TargetControlID="txtToInstallment_RepayTab">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtToInstallment_RepayTab" runat="server" ControlToValidate="txtToInstallment_RepayTab"
                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                                    SetFocusOnError="True" ErrorMessage="Enter the To installment"></asp:RequiredFieldValidator>
                                                                                <asp:CompareValidator ID="cmpvFromTOInstall" runat="server" ErrorMessage="To installment should be greater than From installment"
                                                                                    ControlToValidate="txtToInstallment_RepayTab" ControlToCompare="txtFromInstallment_RepayTab"
                                                                                    Display="None" ValidationGroup="TabRepayment1" Type="Integer" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="From Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(DateFormate) %> '></asp:Label>
                                                                                <asp:TextBox ID="txRepaymentFromDate" runat="server" Visible="false" BackColor="Navy"
                                                                                    ForeColor="White" Font-Names="calibri" Font-Size="12px" Width="95%" Style="color: White"
                                                                                    Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(DateFormate) %> '
                                                                                    AutoPostBack="True" OnTextChanged="txRepaymentFromDate_TextChanged"></asp:TextBox>
                                                                                <cc1:CalendarExtender ID="calext_FromDate" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                                                    TargetControlID="txRepaymentFromDate">
                                                                                </cc1:CalendarExtender>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtfromdate_RepayTab" runat="server" Width="95%">
                                                                                </asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtenderSD_fromdate_RepayTab" runat="server" Enabled="True"
                                                                                    TargetControlID="txtfromdate_RepayTab">
                                                                                </cc1:CalendarExtender>
                                                                            </FooterTemplate>
                                                                            <FooterStyle Width="10%" />
                                                                            <ItemStyle Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="To Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblTODate_ReapyTab" runat="server" Width="100%" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ToDate")).ToString(DateFormate) %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtToDate_RepayTab" runat="server" Width="95%" Visible="false">
                                                                                </asp:TextBox>
                                                                                <cc1:CalendarExtender ID="CalendarExtenderSD_ToDate_RepayTab" runat="server" Enabled="True"
                                                                                    OnClientDateSelectionChanged="checkDate_PrevSystemDate" TargetControlID="txtToDate_RepayTab">
                                                                                </cc1:CalendarExtender>
                                                                            </FooterTemplate>
                                                                            <FooterStyle Width="10%" />
                                                                            <ItemStyle Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Add">
                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                            <FooterStyle HorizontalAlign="Center" Width="5%" />
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnRemoveRepayment" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                    Text="Remove" Visible="false"></asp:LinkButton></ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Button ID="btnAddRepayment" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                                                    OnClick="btnAddRepayment_OnClick" ValidationGroup="TabRepayment1" OnClientClick="return fnCheckPageValidators('TabRepayment1',false)">
                                                                                </asp:Button>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Repayment CashFlowId" Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCashFlowId" runat="server" Text='<%# Bind("CashFlowId") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAccountingIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblBusinessIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblComapnyIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="right">
                                                                <asp:Button ID="btnRecalculate" runat="server" Visible="False" CssClass="styleSubmitButton"
                                                                    Text="Calculate IRR" OnClick="btnRecalculate_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                                    CssClass="styleMandatoryLabel" 
                                                                    HeaderText="Correct the following validation(s):  " 
                                                                    ValidationGroup="TabRepayment1" Width="98%" />
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
                    </td>
                </tr>
                <tr>
                    <td align="center" width="100%">
                        <asp:UpdatePanel ID="updButtons" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" Text="Save" CssClass="styleSubmitButton" ID="btnSave"
                                                OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators();">
                                            </asp:Button>
                                            <asp:Button runat="server" ID="btnClear" Text="Clear" CssClass="styleSubmitButton"
                                                CausesValidation="False" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="styleSubmitButton"
                                                CausesValidation="False" OnClick="btnCancel_Click" />
                                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                OnClick="btnCancel_Click" Text="Cancel Revision" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <cc1:ModalPopupExtender ID="MPopUp" runat="server" TargetControlID="btnBack" PopupControlID="Panel3"
                                                BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="CancelButton">
                                            </cc1:ModalPopupExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="Panel3" runat="server" Style="display: none" BackColor="White" BorderStyle="Solid"
                                                            BorderColor="Black" Width="100%">
                                                            <table style="width: 70%">
                                                                <tr>
                                                                    <td>
                                                                        Remarks
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtCancelReason" runat="server" TextMode="MultiLine" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 50%" align="right">
                                                                    </td>
                                                                    <td style="width: 50%" align="left">
                                                                        <asp:Button ID="OkButton" runat="server" Text="OK" CssClass="styleSubmitShortButton"
                                                                            OnClick="OkButton_Click" />
                                                                        <asp:Button ID="CancelButton" CssClass="styleSubmitShortButton" runat="server" Text="Cancel"
                                                                            OnClick="CancelButton_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript" language="javascript">
         function funCalcResidualAmount(input) {
             var ResidualAmount = document.getElementById('ctl00_ContentPlaceHolder1_tcSpecificRevision_tbExisting_grvAccountRevisionDetails_ctl06_txtRevisedValue');
             var PriAmount = document.getElementById('ctl00_ContentPlaceHolder1_tcSpecificRevision_tbExisting_txtPriAmt');
             if (input.value != "" && PriAmount.value != "") {
                 ResidualAmount.value =  parseFloat(parseFloat(PriAmount.value) * (parseFloat(input.value) / 100)).toFixed(2);
             }
         
         }
     
     </script>
</asp:Content>
