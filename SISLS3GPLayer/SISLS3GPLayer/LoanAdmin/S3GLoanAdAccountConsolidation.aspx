<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdAccountConsolidation.aspx.cs" Inherits="LoanAdmin_S3GLoanAdAccountConsolidation"
    Title="S3G - Account Consolidation" MaintainScrollPositionOnPostback="true" SmartNavigation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading" colspan="2" style="height: 19px">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Account Consolidation">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:50%;">
                        <asp:Panel ID="Panel1" GroupingText="Consolidation Information" runat="server" CssClass="stylePanel"
                            Height="100%">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Code" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <cc1:ComboBox ID="cmbCustomerCode" runat="server" AutoPostBack="True" AutoCompleteMode="SuggestAppend"
                                            onkeyup="maxlengthfortxt(113)" DropDownStyle="DropDownList" MaxLength="0" CssClass="WindowStyle"
                                            OnSelectedIndexChanged="cmbCustomerCode_SelectedIndexChanged" Width="160px" TabIndex="1">
                                        </cc1:ComboBox>
                                        <asp:RequiredFieldValidator ID="rfvCustomerCode" runat="server" ErrorMessage="Select Customer Code"
                                            Display="None" ControlToValidate="cmbCustomerCode" InitialValue="--Select--"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblConsNumber" runat="server" Text="Consolidation Number" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtConsNumber" runat="server" TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLineofBusiness" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="margin-left: 40px">
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_PrevSystemDate"
                                            TargetControlID="txtConsolidateAccountDate" PopupButtonID="imgConsAccDate" ID="calExeConsAccDate"
                                            Enabled="false">
                                        </cc1:CalendarExtender>
                                        <asp:DropDownList ID="ddlLineofBusiness" runat="server" AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlLineofBusiness_SelectedIndexChanged" TabIndex="3">
                                        </asp:DropDownList>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlLineofBusiness"
                                            Display="None" ErrorMessage="Select a Line Of Business" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                      <%--  <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" 
                                            OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" TabIndex="4">
                                        </asp:DropDownList>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlBranch"
                                            Display="None" ErrorMessage="Select a Location" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                               <uc2:Suggest ID="ddlBranch" runat="server" ToolTip="Location" ServiceMethod="GetBranchList"
                                            AutoPostBack="true" OnItem_Selected="ddlBranch_SelectedIndexChanged" 
                                            ErrorMessage="Select a Location" IsMandatory="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblConsolidationDate" runat="server" Text="Consolidation Date" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtConsolidationDate" runat="server" Width="100px" 
                                            onchange="ConsolidationAccountDate()" TabIndex="5" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvtxtConsolidationDate" runat="server" ErrorMessage="Enter Consolidate Date"
                                            Display="None" ControlToValidate="txtConsolidationDate" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                        <asp:Image ID="imgConsDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate_Cons"
                                            TargetControlID="txtConsolidationDate" PopupButtonID="imgConsDate" ID="calExeConsDate"
                                            Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblConsolidateAccountDate" runat="server" CssClass="styleReqFieldLabel"
                                            Text="Consolidation Account Date"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtConsolidateAccountDate" runat="server" Width="100px" 
                                            TabIndex="6" ></asp:TextBox>
                                        <asp:Image ID="imgConsAccDate" runat="server" ImageUrl="~/Images/calendaer.gif" style="display:none"/>
                                        <asp:RequiredFieldValidator ID="rfvConsolidateAccountDate" runat="server" ControlToValidate="txtConsolidateAccountDate"
                                            Display="None" ErrorMessage="Enter Consolidate Account Date" ValidationGroup="Submit" Enabled="false"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                
                                <tr>
                                <td>
                                
                                </td>
                                    <td style="padding-top:1%;padding-bottom:1%">
                                        <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Enabled="False"
                                            OnClick="btnGo_Click" Text="GO" OnClientClick="ConsolidationAccountDate()" 
                                            TabIndex="7"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td style="width:50%;">
                        <asp:Panel ID="pnlCustomerInformation" GroupingText="Customer Information" runat="server"
                            CssClass="stylePanel" Height="100%">
                            <table width="100%" align="center">
                                <tr>
                                    <td width="100%">
                                        <uc1:S3GCustomerAddress ID="S3GCustomerCommAddress" runat="server" ShowCustomerCode="false"
                                            showcusomername="false" FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="PanAccount" runat="server" CssClass="stylePanel" GroupingText="Account Consolidation Details">
                            <table width="100%">
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="grvConsolidation" runat="server" AutoGenerateColumns="False" 
                                            Width="100%" TabIndex="8">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Slno" SortExpression="Slno">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSlno" runat="server" Text='<%# Bind("Slno") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField AccessibleHeaderText="Select Account" HeaderText="Select Account">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="ChkSelectAccount" runat="server" AutoPostBack="True" OnCheckedChanged="ChkSelectAccount_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Prime Account Number" SortExpression="Prime Account Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPanNo" runat="server" Text='<%# Bind("Prime_Account_Number") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sub Account Number" SortExpression="Sub Account Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubNo" runat="server" Text='<%# Bind("Sub_Account_Number") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Original Agreement Date" SortExpression="Original Aggrement Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrginalAgrDate" runat="server" Text='<%# Bind("Aggrement_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Original Amount Financed" SortExpression="Original Amount Financed">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrginalAmt" runat="server" Text='<%# Bind("Original_Amount_Financed") %>'></asp:Label>
                                                    </ItemTemplate>
                                                     <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Principal Outstanding" SortExpression="Pricipal Outstanding">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrincipalOutStanding" runat="server" Text='<%# Bind("Pricipal_Outstanding") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="Staus">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Current Status Type" SortExpression="Current Status Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrTypeStatus" runat="server" Text='<%# Bind("Current_Status_Type_Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Current Status" SortExpression="Current Staus">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrStatus" runat="server" Text='<%# Bind("Current_Status_Type") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNewFinanceAmount" runat="server" Text="New Financed Amount" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewFinancedAmount" runat="server" Style ="text-align:right" Text="0" TabIndex="9"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvNewFinanceAmount" runat="server" ControlToValidate="txtNewFinancedAmount"
                                            Display="None" ErrorMessage="Finance amount Cannot be Zero" ValidationGroup="Submit"
                                            InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding-top: 10px" colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            OnClick="btnSave_Click" OnClientClick="return fnCheckPageValidators()" 
                            TabIndex="10" />&nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" 
                            CausesValidation="false" TabIndex="11" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" CausesValidation="false" TabIndex="12" />&nbsp;
                        <asp:Button ID="btnCreateAcc" runat="server" Text="Create Account" CssClass="styleSubmitButton"
                            OnClick="btnCreateAcc_Click" CausesValidation="false" TabIndex="13" />&nbsp;
                        <asp:Button ID="btnConsCancel" runat="server" Text="Consolidation Cancel" CssClass="styleSubmitLongButton"
                            CausesValidation="false" OnClick="btnConsCancel_Click"  
                            OnClientClick="return Confirmmsg('Do you want to cancel account consolidation?'); " 
                            TabIndex="14" />
                        &nbsp;
                             <asp:Button ID="btnViewAccount" runat="server" 
                            Text="View Consolidated Account" CssClass="styleSubmitLongButton"
                            CausesValidation="false" onclick="btnViewAccount_Click" TabIndex="15" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField runat="server" ID="hdnMLASLA" />
                        <asp:HiddenField runat="server" ID="hdnStatus" />
                        <asp:HiddenField runat="server" ID="hdnConsPAN" />
                        <br />
                        <asp:HiddenField ID="hdnAccID" runat="server" />
                        <asp:HiddenField runat="server" ID="hdnConsSAN" />
                          <asp:CustomValidator ID="cv_Consolidation" runat="server" CssClass="styleMandatoryLabel"
                            Display="None" ></asp:CustomValidator>
                        <asp:ValidationSummary ID="vsConsolidation" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
        <script language="javascript" type="text/javascript">
    
    function ViewModal(var1) {

    window.showModalDialog('../LoanAdmin/S3GLoanAdAccountCreation.aspx?qsAccConNo=' + var1 + '&IsFromAccount=Acccon','Account Creation','dialogwidth:1000px;dialogHeight:1000px;');
}
    function ConsolidationAccountDate() 
    {
     var ConsolidationDate =  document.getElementById('<%= txtConsolidationDate.ClientID %>').value;
     document.getElementById('<%= txtConsolidateAccountDate.ClientID %>').value =ConsolidationDate ;
     document.getElementById('ctl00_ContentPlaceHolder1_txtConsolidationDate').focus();

 }
 function checkDate_NextSystemDate_Cons(sender, args) {
     var today = new Date();
     if (sender._selectedDate > today) {

         //alert('You cannot select a date greater than system date');
         alert('Selected date cannot be greater than system date');
         sender._textbox.set_Value(today.format(sender._format));
         document.getElementById('<%= txtConsolidateAccountDate.ClientID %>').value = today.format(sender._format);
     }
     sender.focus();

 }
    </script>
</asp:Content>
