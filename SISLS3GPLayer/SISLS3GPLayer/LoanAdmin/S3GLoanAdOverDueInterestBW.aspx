<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdOverDueInterestBW.aspx.cs" Inherits="LoanAdmin_S3GLoanAdOverDueInterestBW"
    Title="Over Due Interest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" language="javascript">
  function fnLoadCustomer() { 
  
if("<%= strMode %>" !="M" && "<%= strMode %>" !="Q" && document.getElementById('ctl00_ContentPlaceHolder1_rblAllSpecific_0').checked==false )
            document.getElementById('<%= btnLoadCustomer.ClientID %>').click();
        }
function Resize()
 {
     var dv=document .getElementById ('div1');
     var gv=document .getElementById ('grvODI');
     dv.style.height =100;
//     if((gv!=null) && (dv!=null))
//     {
//        dv.style.height =100;
//     }

 }

 //Call this Function if your Calender Should not allow values greater than system date
 function checkDate_NextSystemDateForODI(sender, args) 
 {

     var today = new Date();
     if (document.getElementById('ctl00_ContentPlaceHolder1_rblAllSpecific_0').checked == true) 
     {
        
     }
     else
      {

          if (sender._selectedDate > today) 
         {
             alert('Selected date cannot be greater than system date');
             sender._textbox.set_Value(today.format(sender._format));

         }
         document.getElementById(sender._textbox._element.id).focus();
     }
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
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                    <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                                        OnClick="btnLoadCustomer_OnClick" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:UpdatePanel ID="udpnlODI" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlOverDueInterest" runat="server" CssClass="stylePanel" GroupingText="Over Due Interest Details">
                                    <table cellpadding="1" cellspacing="1" border="0" width="100%">
                                        <%-- First Row --%>
                                        <tr>
                                            <td class="styleFieldLabel" width="15%">
                                                <asp:Label ID="lblSelection" runat="server" Text="Selection" />
                                            </td>
                                            <td align="left" width="25%">
                                                <asp:RadioButtonList runat="server" ID="rblAllSpecific" AutoPostBack="true" RepeatDirection="Horizontal"
                                                    Style="font-family: calibri,Verdana; font-size: 14px;" OnSelectedIndexChanged="rblAllSpecific_SelectedIndexChanged">
                                                    <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Specific" Value="Specific"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <%--   <asp:RequiredFieldValidator ID="RFVrblAllSpecific"  runat="server"
                                            ControlToValidate="rblAllSpecific" ValidationGroup="VGODI" SetFocusOnError="true"  ErrorMessage="Select the All / Specific"
                                            Display="None"></asp:RequiredFieldValidator>
                                            
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator1"  runat="server"
                                            ControlToValidate="rblAllSpecific" ValidationGroup="VGCAL" SetFocusOnError="true"  ErrorMessage="Select the All / Specific"
                                            Display="None"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td class="styleFieldLabel" width="25%">
                                                <asp:Label ID="Label1" runat="server" Text="Schedule" />
                                            </td>
                                            <td width="25%" align="left">
                                                <asp:RadioButtonList ID="rbtnSchedule" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                    Style="font-family: calibri,Verdana; font-size: 14px;" OnSelectedIndexChanged="rbtnSchedule_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True" Text="Schedule at" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Schedule Now" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <%-- Second Row --%>
                                        <tr>
                                            <%-- LOB --%>
                                            <td class="styleFieldLabel" width="15%">
                                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                                </asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:DropDownList ID="ddlLOB" Width="200px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVLOB" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlLOB" SetFocusOnError="true" InitialValue="0" ValidationGroup="VGODI"
                                                    Display="None"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvCALLOB" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlLOB" SetFocusOnError="true" InitialValue="0" ValidationGroup="VGCAL"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="25%">
                                                <asp:Label ID="lblScheduleDate" runat="server" Text="Schedule Date"></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:TextBox Width="100px" ID="txtScheduleDate" runat="server" Text="" Height="17px"
                                                    Enabled="False"></asp:TextBox>
                                                <asp:Image ID="imgScheduleDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <cc1:CalendarExtender ID="CECScheduleDate" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                    PopupButtonID="imgScheduleDate" TargetControlID="txtScheduleDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator Display="None" ID="RFVScheduleDate" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="txtScheduleDate" ValidationGroup="VGODI" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <%-- third Row --%>
                                        <tr>
                                            <td class="styleFieldLabel" width="15%">
                                                <asp:Label runat="server" Text="Location" ID="lblBranch"  CssClass="styleReqFieldLabel">
                                                </asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:DropDownList ID="ddlBranch" Width="200px" runat="server" AutoPostBack="True"
                                                    Enabled="false" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVOLEBranch" Enabled="false" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="ddlBranch" SetFocusOnError="true" InitialValue="0" ValidationGroup="VGODI"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvCALBranch" Enabled="false" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="ddlBranch" SetFocusOnError="true" InitialValue="0" ValidationGroup="VGCAL"
                                                    ErrorMessage="Select the Location" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="25%">
                                                <asp:Label ID="lblScheduleTime" runat="server" Text="Schedule Time"></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:TextBox ID="txtScheduleTime" runat="server" Width="88px" MaxLength="8"></asp:TextBox>
                                                <span class="styleMandatory">(HH:MM AM)</span>
                                                <asp:RequiredFieldValidator Display="None" ID="RFVScheduleTime" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="txtScheduleTime" ValidationGroup="VGODI" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="REVScheduleTime" ValidationGroup="VGODI" runat="server"
                                                    ControlToValidate="txtScheduleTime" SetFocusOnError="True" Display="None" ValidationExpression="^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$"></asp:RegularExpressionValidator>
                                                <%--</br><span class="styleMandatory">(Should be alteast five minutes greater than current
                                                    time)</span>--%>
                                            </td>
                                        </tr>
                                     <%--  <tr><td></td><td></td><td></td><td><span class="styleMandatory">(Should be alteast five minutes greater than current
                                                    time)</span></td></tr>--%>
                                        <%-- Fourth Row --%>
                                        <tr>
                                            <td class="styleFieldLabel" width="15%">
                                                <asp:Label ID="lblCuscode" runat="server" CssClass="styleReqFieldLabel" Text="Customer code"></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <%-- <asp:DropDownList ID="ddlCustomerCode"  Enabled="false" runat="server" Width="200px" AutoPostBack="True">
                                        </asp:DropDownList>--%>
                                                <asp:TextBox ID="cmbCustomerCode" runat="server" MaxLength="50" AutoPostBack="true"
                                                    Visible="false" ReadOnly="true" OnTextChanged="cmbCustomerCode_TextChanged"></asp:TextBox>
                                                <uc2:LOV ID="ucCustomerCode" DispalyContent="Both" runat="server" strLOV_Code="CMB"
                                                    strLOBID="0" />
                                                <%--     <asp:RequiredFieldValidator ID="RFVCustomerCode" Enabled="false" runat="server" ControlToValidate="cmbCustomerCode"
                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Customer Code"
                                        ValidationGroup="VGODI" Visible="False">
                                        </asp:RequiredFieldValidator>--%>
                                                <asp:RequiredFieldValidator ID="RFVCustomerCode" Enabled="false" CssClass="styleMandatoryLabel"
                                                    runat="server" ValidationGroup="VGODI" ControlToValidate="cmbCustomerCode" SetFocusOnError="true"
                                                    Display="None"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvCALcustomerCode" Enabled="false" CssClass="styleMandatoryLabel"
                                                    runat="server" ValidationGroup="VGCAL" ControlToValidate="cmbCustomerCode" SetFocusOnError="true"
                                                    Display="None"></asp:RequiredFieldValidator>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtenderDemo" runat="server" TargetControlID="cmbCustomerCode"
                                                    ServiceMethod="GetCustomerList" MinimumPrefixLength="1" CompletionSetCount="20"
                                                    DelimiterCharacters="" Enabled="True" ServicePath="">
                                                </cc1:AutoCompleteExtender>
                                                <asp:HiddenField ID="hidCustId" runat="server" />
                                                <asp:TextBox Visible="false" ID="txtCustomerName" runat="server" ReadOnly="true"
                                                    Width="200px"></asp:TextBox>
                                            </td>
                                            <%-- <td class="styleFieldLabel">
                                                <span>Customer Name</span>
                                            </td>--%>
                                            <td class="styleFieldLabel" width="25%">
                                                <asp:Label ID="lblLastODICalcDate" runat="server" Text="Last ODI Calculation Date"></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:TextBox runat="server" ID="txtLastODICalculationDate" ReadOnly></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%-- Fifth Row--%>
                                        <tr>
                                            <td class="styleFieldLabel" width="15%">
                                                <asp:Label ID="lblPrimeAcc" runat="server" CssClass="styleReqFieldLabel" Text="Prime A/C No."></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:DropDownList ID="ddlPrimeAccountNo" runat="server" Enabled="false" Width="200px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlPrimeAccountNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVPrimeAccountNo" Enabled="false" runat="server"
                                                    ControlToValidate="ddlPrimeAccountNo" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    Display="None" InitialValue="0" ValidationGroup="VGODI"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvCALPrimeAC" Enabled="false" runat="server" ControlToValidate="ddlPrimeAccountNo"
                                                    SetFocusOnError="true" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                    ValidationGroup="VGCAL"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="25%">
                                                <asp:Label ID="lblCurrentODIDate" runat="server" CssClass="styleReqFieldLabel" Text="Current ODI Calculation Date"></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:TextBox runat="server" ID="txtCurrentODIDate" Width="155px" AutoPostBack="true"
                                                    OnTextChanged="txtCurrentODIDate_TextChanged">                                    
                                                </asp:TextBox>
                                                <asp:Image ID="imgCurrentODIDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <asp:RequiredFieldValidator ID="RFVCurrentODIDate" CssClass="styleMandatoryLabel"
                                                    runat="server" ValidationGroup="VGODI" ControlToValidate="txtCurrentODIDate"
                                                    SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvCALCurrentODIcalculationDate" CssClass="styleMandatoryLabel"
                                                    runat="server" ValidationGroup="VGCAL" ControlToValidate="txtCurrentODIDate"
                                                    SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                                                <cc1:CalendarExtender ID="CECCurODICalcDate" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDateForODI"
                                                    PopupButtonID="imgCurrentODIDate" TargetControlID="txtCurrentODIDate" Format="dd/MM/yyyy">
                                                </cc1:CalendarExtender>
                                            </td>
                                        </tr>
                                        <%-- Sixth Row --%>
                                        <tr>
                                            <td class="styleFieldLabel" width="15%">
                                                <asp:Label ID="lblSAN" runat="server" Text="Sub A/C No"></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:DropDownList ID="ddlSubAccountNo" Enabled="false" runat="server" Width="200px"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlSubAccountNo_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVSubAccountNo" Enabled="false" runat="server" ControlToValidate="ddlSubAccountNo"
                                                    SetFocusOnError="true" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                    ValidationGroup="VGODI"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvCALsubAC" Enabled="false" runat="server" ControlToValidate="ddlSubAccountNo"
                                                    SetFocusOnError="true" CssClass="styleMandatoryLabel" Display="None" InitialValue="0"
                                                    ValidationGroup="VGCAL"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="styleFieldLabel" width="25%">
                                                <asp:Label ID="lblODIRate" runat="server" Text="Current ODI Rate"></asp:Label>
                                            </td>
                                            <td width="25%">
                                                <asp:TextBox runat="server" ID="txtODIRate" Width="50px" ReadOnly></asp:TextBox>
                                                &nbsp &nbsp &nbsp
                                                <asp:Button ID="btnCalculateODI" CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('VGCAL',false);"
                                                    ValidationGroup="VGCAL" runat="server" Text="Calculate ODI" OnClick="btnCalculateODI_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <%--  <asp:Button ID="btnStart" runat="server" Visible="false" Text="Start" CssClass="styleSubmitButton" OnClick="btnStart_Click" />
                                        <asp:Button ID="btnStop" runat="server" Text="Stop" Visible="false" CssClass="styleSubmitButton" OnClick="btnStop_Click" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align ="center" >
                        <asp:Panel ID="panSchedule" width="80%" Visible ="false"  runat="server" HorizontalAlign ="Center"  GroupingText="OverDue Information" CssClass="stylePanel">
                            <%--<div id ="div1" style="overflow-x: hidden; overflow-y: auto; height :auto  "  runat ="server" >--%>
                           <asp:Panel ID="Panel1"  Height ="100px" ScrollBars ="Vertical"  runat="server" HorizontalAlign ="Center" >

                            <asp:GridView runat="server" ID="grvODI" AutoGenerateColumns="False" 
                                HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" OnRowDataBound="grvODI_RowDataBound">
                                <Columns>
                                    <%-- Branch  --%>
                                    <asp:TemplateField HeaderText="Location" >
                                        <ItemTemplate>
                                            <asp:Label ID="txtBranchName" runat="server" Text='<%# Bind("Location")%>'></asp:Label>
                                            <asp:Label ID="txtBranchId" runat="server" Visible="false" Text='<%# Bind("Location_ID")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign ="Left" Width ="30%" />
                                    </asp:TemplateField>
                                    <%-- Number of A/c  --%>
                                    <asp:TemplateField  HeaderText="No. of Accounts" >
                                        <ItemTemplate>
                                            <asp:Label ID="txtNoOfAccounts" Style="text-align: right" runat="server" Text='<%# Bind("ACC_COUNT")%>'></asp:Label>
                                        </ItemTemplate>
                                      <ItemStyle HorizontalAlign ="Right"  Width ="10%" /> 
                                    </asp:TemplateField>
                                    <%-- ODI Amount  --%>
                                    <asp:TemplateField  HeaderText="ODI Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="txtODIAmount" Width="100%" runat="server" Text='<%# Bind("ODI_AMOUNT")%>'
                                                Style="text-align: right;"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign ="Right" Width ="15%" />
                                    </asp:TemplateField>
                                    <%-- Check Box  --%>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Center"
                                         HeaderText="Select">
                                        <HeaderTemplate>
                                            <span id="spnAll" style="text-align: center">All</span></br>
                                            <asp:CheckBox ID="ChkHeadODI" runat="server" AutoPostBack="true" OnCheckedChanged="ChkHeadODI_CheckedChanged">
                                            </asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkODI" runat="server" AutoPostBack="True" OnCheckedChanged="ChkODI_CheckedChanged">
                                            </asp:CheckBox>
                                            <asp:HiddenField ID="ODIStatus" runat="server" Value='<%# Bind("Status")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign ="Center" Width ="10%" />
                                    </asp:TemplateField>
                                    <%-- Status --%>
                                    <asp:TemplateField  HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Process")%>'></asp:Label>
                                            <asp:HiddenField ID="hidBillStatus" runat="server" Value='<%# Bind("BillStatus")%>'>
                                            </asp:HiddenField>
                                        </ItemTemplate>
                                       <ItemStyle HorizontalAlign ="Left" Width ="20%" />  
                                        
                                    </asp:TemplateField>
                                    <%-- Revoke --%>
                                    <asp:TemplateField Visible="false" HeaderText="Revoke">
                                        <HeaderTemplate>
                                            <span id="spnAll" style="text-align: center">Revoke</span>
                                           </br> <asp:CheckBox ID="ChkHeadRevoke" runat="server" AutoPostBack="true" OnCheckedChanged="ChkHeadRevoke_CheckedChanged">
                                            </asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkRevoke" runat="server" AutoPostBack="true" OnCheckedChanged="ChkRevoke_CheckedChanged">
                                            </asp:CheckBox>
                                            <asp:HiddenField ID="ODIID" runat="server" Value='<%# Bind("ODIID")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hidRevoke" runat="server" Value='<%# Bind("Is_Revoke")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                         <ItemStyle HorizontalAlign ="Center" Width ="15%" /> 
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="styleGridHeader" />
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                            </asp:Panel> 
                           <%-- </div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                    <td>
                        <asp:Button ID="btnRevoke" CssClass="styleSubmitButton" runat="server" Text="Revoke"
                            Width="100px" OnClick="btnRevoke_Click" Visible="False" />
                        &nbsp;<asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators('VGODI');"
                            CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="VGODI"
                            CausesValidation="false" CssClass="styleSubmitButton" Height="26px" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsFactoringInvoiceLoadinf" HeaderText="Correct the following validation(s):"
                            Height="177px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="VGODI" />
                        <asp:ValidationSummary runat="server" ID="ValidationSummary1" HeaderText="Correct the following validation(s):"
                            Height="177px" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="VGCAL" />
                        <asp:CustomValidator ID="cvOverDueInterest" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%"></asp:CustomValidator>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- Check Box  --%>
</asp:Content>
