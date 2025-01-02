<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master" 
CodeFile="S3GRptPDCReminderReport.aspx.cs" Inherits="Reports_S3GRptPDCReminderReport" Title="PDC Reminder" %>

<%--Ajax Control Toolkit--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%--Content--%>

<asp:Content ID="ContentPDCReminderReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" cellpadding="0" cellspacing="2px" border="0">
<tr>
<td colspan="4" class="stylePageHeading">
    <table>
            <tr>
                 <td>
                     <asp:Label runat="server" ID="lblHeading" EnableViewState="false" CssClass="styleInfoLabel" Text="PDC Reminder Report">
                     </asp:Label>       
                 </td>       
            </tr>
     </table>    
</td>
</tr>
<tr>
    <td>
       <table width="100%" cellpadding="0" cellspacing="2px" border="0">
          
                <tr>    
                    <td>
                        <asp:Panel ID="pnlPDCReminderHeader" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                        <table width="100%">
                        <tr>         
                        <td width="10%">
                            <asp:Label runat="server" Text="Line of Business" ID="lblLOBSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td align="left" width="10%">
                            <asp:DropDownList ID="ComboBoxLOBSearch" AutoPostBack="true" ValidationGroup="RFVPDCReminder"
                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                MaxLength="0" CssClass="WindowsStyle" Height="23px" 
                                Width="100%" ToolTip="Line of Business" OnSelectedIndexChanged="ComboBoxLOBSearch_SelectedIndexChanged">
                            </asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="RFVComboLOB" ValidationGroup="RFVPDCReminder" InitialValue="-1"
                                CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxLOBSearch"
                                SetFocusOnError="True" ErrorMessage="Select a Line Of Business" Display="None" Width="2%"></asp:RequiredFieldValidator>--%>
                        </td>
                        
                         <td width="15%" align="left">
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label Text="Location1" runat="server" ID="lblBranchSearch" CssClass="styleDisplayLabel" />
                        </td>
                        <td align="left" width="15%">
                            <asp:DropDownList ID="ComboBoxBranchSearch" AutoPostBack="true" ValidationGroup="RFVPDCReminder"
                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                MaxLength="0" CssClass="WindowsStyle" 
                                Height="23px" Width="100%" ToolTip="Location1" OnSelectedIndexChanged="ComboBoxBranchSearch_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RFVComboBranch" ValidationGroup="RFVPDCReminder" InitialValue="-1"
                             CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxBranchSearch" Enabled="false"
                             SetFocusOnError="True" ErrorMessage="Select Location1" Display="None" Width="2%"></asp:RequiredFieldValidator>
                        </td>
                        </tr>  
                        <tr>
                        <td width="10px">
                        
                        </td>
                        </tr> 
                        <tr>
                         <td width="15%" align="left">
                            <asp:Label Text="Location2" runat="server" ID="lblLocation" CssClass="styleDisplayLabel" />
                        </td>
                        <td align="left" width="10%">
                            <asp:DropDownList ID="ComboBoxLocationSearch" AutoPostBack="true" ValidationGroup="RFVPDCReminder"
                                runat="server" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                MaxLength="0" CssClass="WindowsStyle" 
                                Height="23px" Width="100%" ToolTip="Location2" OnSelectedIndexChanged="ComboBoxLocationSearch_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="RFVPDCReminder" InitialValue="-1"
                             CssClass="styleMandatoryLabel" runat="server" ControlToValidate="ComboBoxLocationSearch" Enabled="false"
                             SetFocusOnError="True" ErrorMessage="Select Location2" Display="None" Width="2%"></asp:RequiredFieldValidator>
                        </td>
                        <td width="10%" align="center">
                            <asp:Label runat="server" Text="Start Date" ID="lblStartDateSearch" CssClass="styleReqFieldLabel" />
                        </td>
                        <td width="10%" align="left">
                            <asp:TextBox ID="txtStartDateSearch" runat="server" Width="120px" 
                                AutoPostBack="false" Height="19px" ToolTip="Start Date"></asp:TextBox>
                            <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Start Date" />
                            <cc1:CalendarExtender ID="CalendarExtenderStartDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVStartDate" ValidationGroup="RFVPDCReminder" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="txtStartDateSearch" SetFocusOnError="True"
                                ErrorMessage="Select Start Date" Display="None" Width="1%"></asp:RequiredFieldValidator>
                        </td>                      
                       
                        </tr>
                        <tr>
                        <td width="10px">
                        
                        </td>
                        </tr> 
                        <tr>
                        &nbsp;  
                         <td width="15%" align="left">
                        
                            <asp:Label runat="server" ID="lblEndDateSearch" Text="End Date" CssClass="styleReqFieldLabel" />
                        </td>
                        <td width="15%" align="left">
                            <asp:TextBox ID="txtEndDateSearch" runat="server" Width="120px" 
                                AutoPostBack="false" Height="19px" ToolTip="End Date"></asp:TextBox>
                            <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="End Date" />
                            <cc1:CalendarExtender ID="CalendarExtenderEndDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVEndDate" ValidationGroup="RFVPDCReminder" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="txtEndDateSearch" SetFocusOnError="True" ErrorMessage="Select End Date"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>                                                          
                        <td width="10%" align="center">                                                
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label runat="server" ID="LblPath" Text="Document Path" CssClass="styleDisplayLabel" />
                        </td>
                        <td width="10%" align="left">
                            <asp:TextBox ID="txtFilePath" runat="server" Width="172px" 
                                AutoPostBack="false" Height="19px" ToolTip="File Path"></asp:TextBox>
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
    <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="styleSubmitButton" 
          onclick="btnGo_Click" CausesValidation="true" OnClientClick="return fnCheckPageValidators('RFVPDCReminder',false);" ValidationGroup="RFVPDCReminder" ToolTip="Go" /> 
    <asp:Button ID="btnClear" runat="server" Text="Clear" 
          CssClass="styleSubmitButton" UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();"
          onclick="btnClear_Click" ToolTip="Clear" />
    </td>    
    </tr>
<tr>
    <td align="right">
    <asp:Label ID="lblCurrency" runat="server" ToolTip="Currency">
    </asp:Label>
    </td>
    </tr>
<tr>                     
                        
        <td  width="100%">  
        <asp:Panel ID="pnlPDCDetails" runat="server" GroupingText="PDC Reminder Details" CssClass="stylePanel" Width="100%">                     
           <div style="height: 200px; overflow: auto;">
            <asp:GridView ID="grvPDCDetails" runat="server" AutoGenerateColumns="False" ShowFooter="true"
            BorderWidth="2" Width="100%" Height="20px" HeaderStyle-CssClass="Freezing" OnRowDataBound="grvPDCDetails_RowDataBound">
        <Columns>
             <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="2%">
             <ItemTemplate>
             <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("CUSTOMER_NAME")%>' ToolTip="Customer Name"></asp:Label>
             </ItemTemplate>
             <ItemStyle Width="20%" HorizontalAlign="left"></ItemStyle>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Account No." ItemStyle-HorizontalAlign="center">
             <ItemTemplate>
             <asp:Label ID="lblPrimeAccountNumber" runat="server" Text='<%#Eval("PRIMEACCOUNTNO")%>' ToolTip="Prime Account No"></asp:Label>
             </ItemTemplate>
             <ItemStyle HorizontalAlign="left"></ItemStyle>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Sub Account No." ItemStyle-HorizontalAlign="center">
             <ItemTemplate>
             <asp:Label ID="lblSubAccountNumber" runat="server" Text='<%#Eval("SUBACCOUNTNO")%>' ToolTip="Sub Account No"></asp:Label>
             </ItemTemplate>
             <ItemStyle HorizontalAlign="left"></ItemStyle>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Last Collected PDC Date" ItemStyle-HorizontalAlign="center">
             <ItemTemplate>
             <asp:Label ID="lblLastCollectedPDCDate" runat="server" Text='<%#Eval("LASTCOLLECTEDPDCDATE")%>' ToolTip="Last Collected PDC Date"></asp:Label>
             </ItemTemplate>
             <ItemStyle HorizontalAlign="left"></ItemStyle>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Exclude All" SortExpression="Exclude All">
             <HeaderTemplate>
             <asp:Label ID="lblExcludeAll" runat="server" Text="Exclude All" ToolTip="Exclude"></asp:Label>
             <br />
             <asp:CheckBox ID="chkExcludeAll" runat="server" AutoPostBack="false" ToolTip="Exclude" />
             </HeaderTemplate>
             <HeaderStyle Width="10%" />
             <ItemTemplate>
             <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Exclude" />
             </ItemTemplate>
             </asp:TemplateField>
       </Columns>
             <RowStyle HorizontalAlign="Center" />
             </asp:GridView>                            
             </div>
             </asp:Panel>
             </td>
             </tr>
<tr>
             <td align="center" width="50%">
                 <asp:Button ID="BtnPrint" runat="server" Text="Print" 
                     CssClass="styleSubmitButton" onclick="BtnPrint_Click" ToolTip="Print"  />
                 <asp:Button ID="btnGeneratePDF" runat="server" CssClass="styleSubmitButton" Text="GeneratePDF" onclick="btnGeneratePDF_Click" ToolTip="GeneratePDF"/>
                 <asp:Button ID="btnEMail" runat="server" CssClass="styleSubmitButton" Text="EMail" ToolTip="EMail" OnClick="btnEMail_Click" />
             </td>            
             </tr>
<tr>
     <td>        
     <table>
     <tr>
     <td width="50%" align="center">
        <asp:ValidationSummary ValidationGroup="RFVPDCReminder" ID="vsPDCReminder" runat="server"
        CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):"
        ShowSummary="true" />     
     </td>      
     </tr>
     <tr>
     <td>
     <asp:CustomValidator ID="cvPDCReminder" runat="server" Display="None" ValidationGroup="RFVPDCReminder">
     </asp:CustomValidator>
     </td>
     </tr>     
     </table>    
     </td>
     </tr>
     

</table>

<style type="text/css">
    .Freezing
    {
      position:static;
      top:auto;
      z-index:auto;
    }
    
    .GVFixedHeader 
    { font-weight:bold; background-color: Green; position:relative; top:expression(this.parentNode.parentNode.parentNode.scrollTop-1); }
                 
      .container {overflow:auto;}

/* Keep the header cells positioned as we scroll */
.container table th {position:relative;}

/* For alignment of the scroll bar */
.container table tbody {overflow-x:hidden;} 
  </style>

</asp:Content>