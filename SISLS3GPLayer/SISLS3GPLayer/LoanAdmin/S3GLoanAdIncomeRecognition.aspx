<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="S3GLoanAdIncomeRecognition.aspx.cs" Inherits="S3GLoanAdIncomeRecognition" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--Grid User Control--%>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <contenttemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td  class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading"
                                        CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                  <tr>
                    <td valign="top">
                    <asp:Panel ID="pnlIncomeRecog" runat="server" Width="100%" CssClass="stylePanel" GroupingText="Schedule Details">
                     <table  cellpadding="0" cellspacing="0" style="width: 100%;padding-bottom:8px"><tr><td style="width:50%">
                        <table cellpadding="0" cellspacing="0" style="width: 100%;padding-bottom:8px">
                            <tr>
                                <td Width="35%" class="styleFieldLabel">
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel"  Text="Line of Business" ID="lblLOB">
                                    </asp:Label>
                                </td>
                                <td Width="65%" class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlLOB"  OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                        Width="200px" runat="server" AutoPostBack="True" >
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlLOB" ValidationGroup="grpGO" InitialValue="0" Display="None">
                                    </asp:RequiredFieldValidator>
                                    
                                    <asp:RequiredFieldValidator ID="rfvLOBSave" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlLOB" ValidationGroup="grpSave" InitialValue="0" Display="None">
                                    </asp:RequiredFieldValidator>
                                    
                                </td>
                                
                            </tr>
                            <tr>
                            
                            <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="Frequency" ID="lblFrequency" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                </td>
                                <td class="styleFieldAlign"><asp:DropDownList ID="ddlFrequency" Width="150px" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvFrequency" ValidationGroup="grpGO" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlFrequency" InitialValue="0" Display="None">
                                    </asp:RequiredFieldValidator>
                                    
                                    <asp:RequiredFieldValidator ID="rfvFrequencySave" CssClass="styleMandatoryLabel" runat="server"
                                        ControlToValidate="ddlFrequency" ValidationGroup="grpSave"  InitialValue="0" Display="None">
                                    </asp:RequiredFieldValidator>
                                    
                                 </td>
                            
                            </tr>
                            
                            <tr>
                                <td class="styleFieldLabel" style="height: 22px;">
                                    <asp:Label runat="server" Text="Cut off Date" ID="lblCutoffDate" CssClass="styleReqFieldLabel">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox runat="server" Width="140px" ID="txtCutoffDate"></asp:TextBox>
                                    <asp:Image ID="imgCutoffDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" 
                                                    Format="dd/MM/yyyy"  OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                    PopupButtonID="imgCutoffDate" TargetControlID="txtCutoffDate">
                                                </cc1:CalendarExtender>
                                </td>
                                
                            </tr>
                           
                            </table>
                            </td>
                            <td style="width:50%">
                            <table cellpadding="0" cellspacing="0" style="width: 100%;padding-bottom:8px">
                                <tr>
                                
                                    <td class="styleFieldLabel" colspan="2" style="width: 100%">
                                                    <asp:RadioButtonList ID="rbtnSchedule" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                        OnSelectedIndexChanged="rbtnSchedule_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Text="Schedule at:" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Schedule Now" Value="1"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                
                                </tr>
                              <tr>
                                <td style="width:30%;" class="styleFieldLabel">
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel"  Text="Schedule Date" ID="Label2">
                                    </asp:Label>
                                </td>
                                
                                <td  style="width:70%;" class="styleFieldAlign">
                                    <asp:TextBox  runat="server" Width="100px" ID="txtScheduleDate"></asp:TextBox>
                                    <asp:Image ID="imgScheduleDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                    <cc1:CalendarExtender ID="CECScheduleDate" runat="server" Enabled="True" 
                                                    Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                    PopupButtonID="imgScheduleDate" TargetControlID="txtScheduleDate">
                                                </cc1:CalendarExtender>
                                                
                                    <asp:RequiredFieldValidator Display="None" ID="rfvScheduleDate" CssClass="styleMandatoryLabel"
                                        ErrorMessage="Enter Schedule Date"  ValidationGroup="grpSave" 
                                      runat="server" ControlToValidate="txtScheduleDate">
                                        </asp:RequiredFieldValidator>                                                
                                                
                                </td>
                               
                                </tr>
                                <tr>
                                
                                 <td  style="width:30;" class="styleFieldLabel">
                                    <asp:Label runat="server" CssClass="styleReqFieldLabel"  Text="Schedule Time" ID="Label4">
                                    </asp:Label>
                                </td>
                                
                                <td  style="width:70%;" class="styleFieldAlign">
                                    <asp:TextBox runat="server" Width="100px" ID="txtScheduleTime"></asp:TextBox>
                                    (HH:MM AM)
                                                    <asp:RequiredFieldValidator  ValidationGroup="grpSave"  Display="None" ID="rfvScheduleTime" CssClass="styleMandatoryLabel"
                                                        runat="server" ErrorMessage="Enter Schedule Time"  ControlToValidate="txtScheduleTime"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                              <asp:RegularExpressionValidator ID="REVScheduleTime" runat="server"
                                ErrorMessage="Enter Valid Schedule Time"  ValidationGroup="grpSave"  Enabled="false"
                             ControlToValidate="txtScheduleTime" SetFocusOnError="True" Display="None" 
                                ValidationExpression="(^([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)|(^([0-9]|[1][0-9]|[2][0-3])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)"></asp:RegularExpressionValidator>
                                    
                                </td>
                                
                                </tr>
                                
                                <tr>
                                 <td colspan="2" align="right" style="padding-right:100px" valign="top">
                                                    <span class="styleMandatory">(Should be alteast five minutes greater than current time)</span>
                                  </td>
                                </tr>
                                
                                
                                </table>
                            </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnGO" runat="server" CssClass="styleSubmitShortButton" 
                                        OnClick="btnGO_Click" Text="GO" ValidationGroup="grpGO" />
                                    <asp:RequiredFieldValidator ID="rfvCutoffDate" runat="server" 
                                        ControlToValidate="txtCutoffDate" CssClass="styleMandatoryLabel" Display="None" 
                                        ErrorMessage="Enter Cut off Date" ValidationGroup="grpGO">
                                    </asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvCutoffDateSave" runat="server" 
                                        ControlToValidate="txtCutoffDate" CssClass="styleMandatoryLabel" Display="None" 
                                        ErrorMessage="Enter Cut off Date" ValidationGroup="grpSave">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            </tr>
                            </table>
                            </asp:Panel>
                            </td>
                      </tr>      
                      <tr><td>
                        <asp:Panel ID="Panel1" Visible="false" runat="server" Width="100%" CssClass="stylePanel" GroupingText="Income Recognition Details">
                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                <tr><td colspan="4" style="width:100%;padding-left:10px">
                                    
                                    <asp:GridView runat="server" OnRowDataBound="grvIRConsolidate_RowDataBound"
                                    AutoGenerateColumns="FALSE" CellPadding="4" CellSpacing="2"
                                     OnRowCommand="grvIRConsolidate_RowCommand"
                                     ID="grvIRConsolidate" Width="99%">
                                     
                                     <Columns>
                                     
                                      <%-- Check Box  --%>
                                            <asp:TemplateField  HeaderText="Process">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIR" runat="server" ></asp:CheckBox>
                                                    <asp:HiddenField ID="hdnIRStatus" runat="server" Value='<%# Bind("Status")%>' ></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField  HeaderStyle-Wrap="true" HeaderText="Revoke">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRevoke" runat="server" ></asp:CheckBox>
                                                    <asp:Label ID="lblRevokeStatus" Visible="false" runat="server" Text='<%# Bind("Status")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                     
                                     <%-- Branch  --%>
                                     
                                            <asp:TemplateField  ItemStyle-Width="100px" HeaderText="Location Name" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtBranchName" runat="server" Text='<%# Bind("Location")%>' ></asp:Label>
                                                    <asp:Label ID="lblBranchID" runat="server" Visible="false" Text='<%# Bind("Location_Code")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <%-- Number of Inst  --%>
                                            <asp:TemplateField  HeaderText="Number of Installments" Visible="false" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoofInstallment" runat="server" Text='<%# Bind("NoofInstallment")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <%-- Number of A/c  --%>
                                            <asp:TemplateField  HeaderText="Number of Accounts" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNoOfAccounts" runat="server" Text='<%# Bind("ACC_COUNT")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- IR Amount  --%>
                                            <asp:TemplateField  HeaderText="Income Amount" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIRAmount" runat="server" Text='<%# Bind("IR_AMOUNT")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='Yet to Process'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                             <asp:TemplateField HeaderText="Income Recognition No">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncome_Recognition_No" runat="server" Text='<%# Bind("Income_Recognition_No")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                           <asp:TemplateField Visible="false" HeaderText="Posting">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnPosting" OnClientClick="return fnCheckPageValidationPosting();"  runat="server"  CommandArgument='<%# Bind("Income_Calculation_ID") %>' 
                                                    CssClass="styleGridShortButton" CommandName="Posting" Text="Posting" />
                                                    <asp:Label ID="lblPostingStatus" Visible="false" runat="server" Text='<%# Bind("PostingStatus")%>' ></asp:Label>
                                                    <asp:Label ID="lblMonthLock" Visible="false" runat="server" Text='<%# Bind("MonthLock")%>' ></asp:Label>
                                                    
                                                </ItemTemplate>
                                             </asp:TemplateField>
                                            
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Query">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnQuery" Enabled="false" ImageUrl="~/Images/spacer.gif" ToolTip="Query" CssClass="styleGridEditDisabled"
                                                        CommandArgument='<%# Bind("Income_Calculation_ID") %>' CommandName="Query" runat="server" />
                                                 </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px" HeaderText="XL Porting">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnPorting" Enabled="false" ImageUrl="~/Images/spacer.gif" ToolTip="XL Porting" CssClass="styleGridEditDisabled"
                                                        CommandArgument='<%# Bind("Income_Calculation_ID") %>' CommandName="Porting" runat="server" />
                                                 </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            
                                            
                       
                                     </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                            <RowStyle HorizontalAlign="Center" />
                                     </asp:GridView>
                                     
                                </td></tr>
                                
                              <tr>
                                <td Width="100%"  colspan="4">
                                
                                <asp:Panel ID="pnlIRDetails" Visible="false"  runat="server" Width="100%" CssClass="stylePanel" GroupingText="Income Calculation Details">
                                    
                                    <asp:GridView runat="server" AutoGenerateColumns="false" CellPadding="6" CellSpacing="2"
                                     ShowFooter="false" AllowPaging="false"  AllowSorting="false"
                                     ID="grvIncomeRecognition">
                                     <Columns>
                                     
                                        <asp:BoundField DataField="LOB_Name" HeaderText="Line of Business"  />
                                        <asp:BoundField DataField="Location" HeaderText="Location"  />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name"  />
                                        <asp:BoundField DataField="PANum" ItemStyle-Wrap="true" ItemStyle-Width="120px" HeaderText="Account Number"  />
                                        <asp:BoundField DataField="SANum" ItemStyle-Wrap="true" ItemStyle-Width="150px" HeaderText="Sub Account Number"  />
                                        <asp:BoundField DataField="Int.FromDate" HeaderText="Income From Date" HeaderStyle-Wrap="true"  />
                                        <asp:BoundField DataField="Int.ToDate" HeaderText="Income To Date"  HeaderStyle-Wrap="true" />
                                        <asp:BoundField DataField="NoofDays" ItemStyle-HorizontalAlign="Right" HeaderText="No of Days"  />
                                        <%--<asp:BoundField DataField="Int.Amount" ItemStyle-HorizontalAlign="Right" HeaderText="Int.Amount"  />--%>
                                        <asp:BoundField DataField="IncomeAmount" 
                                            ItemStyle-HorizontalAlign="Right"
                                            HeaderText="Income Amount"  />
                                        
                                        
                                     </Columns>
                                      <HeaderStyle CssClass="styleGridHeader" />
                            <RowStyle HorizontalAlign="Left" />
                                     </asp:GridView>
                                </asp:Panel>    
                                </td>
                                </tr>
                                <tr width="100%">
                                        <%--Grid--%>
                                            <td width="100%" colspan="4" align="center">
                                                <uc1:PageNavigator Visible="false" ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                            </td>
                                 </tr>
                               </table> 
                                <br />
                                </asp:Panel>
                            </td>
                            
                            </tr>
                      
                           
                            <tr class="styleButtonArea">
                                <td style="padding-left:100px">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                              <tr>
                                <td colspan="4"  style="width:100%;">
                                
                                <asp:Button runat="server" ID="btnCalculate"  Visible="false"
                                        CssClass="styleSubmitButton" Text="Save" OnClick="btnCalculate_Click" />
                                    
                                    <asp:Button runat="server" Enabled="false" ID="btnSave"  OnClientClick="return fnCheckPageValidation('grpSave');"
                                        CssClass="styleSubmitButton" ValidationGroup="grpSave"  Text="Save" OnClick="btnSave_Click" />
                            
                                    <asp:Button runat="server" ID="btnRevoke"   Enabled="false" OnClientClick="return fnCheckPageValidationRevoke();"
                                        CssClass="styleSubmitButton" Text="Revoke" OnClick="btnRevoke_Click" />
                                        
                                <asp:Button ID="btnClear" runat="server" CausesValidation="false" 
                                        CssClass="styleSubmitButton" OnClick="btnClear_Click" 
                                        OnClientClick="return fnConfirmClear();" Text="Clear" />
                                        
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="false" 
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" Text="Cancel" />
                                    
                                </td>
                            </tr>
                            </table>
                            </td>
                            </tr>
                            
                            <tr class="styleButtonArea"><td colspan="4">
      <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Correct the following validation(s):"
    Height="100px" CssClass="styleMandatoryLabel"  ValidationGroup="grpGO" 
         Width="500px" ShowMessageBox="false" ShowSummary="true" />
      
      <asp:ValidationSummary runat="server" ID="vsSave" HeaderText="Correct the following validation(s):"
    Height="100px" CssClass="styleMandatoryLabel"  ValidationGroup="grpSave" 
         Width="500px" ShowMessageBox="false" ShowSummary="true" />
         <input type="hidden" runat="server" id="hdnIncomeRecgId" />
         
          <asp:Label ID="lblErrorMessage" runat="server" 
              CssClass="styleMandatoryLabel"></asp:Label>
          </td>
      </tr>
        
        
        <tr class="styleButtonArea">
                                <td colspan="3">
                                <asp:CustomValidator ID="rfvCompareCutoffDate" runat="server"
                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="grpSave" 
                                        ErrorMessage="Cut off Date must be last date of month"></asp:CustomValidator>  
                                    <asp:Label ID="Label1" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                  
        </contenttemplate>

    <script language="javascript" type="text/javascript">

        var bResult;

        function fnCheckPageValidation(grpSave) {
           // if (!fnCheckPageValidators(grpSave, false))
             //   return false;

            bResult = fnIsCheckboxChecked('<%=grvIRConsolidate.ClientID%>', 'chkIR', 'Location');
        if (bResult) {

            if (confirm('Do you want to save Income Recognition?')) {
                return true;
            }
            else
                return false;
        }
        return bResult;
    }

    function fnCheckPageValidationRevoke() {

        bResult = fnIsCheckboxChecked('<%=grvIRConsolidate.ClientID%>', 'chkRevoke', 'Location');
        if (bResult) {
            if (confirm('Do you want to Revoke?')) {
                return true;
            }
            else
                return false;
        }
        return bResult;
    }

    function fnCheckPageValidationPosting() {

        if (confirm('Do you want to Post Sys Journal?')) {
            return true;
        }
        else
            return false;


    }


    </script>

</asp:Content>


