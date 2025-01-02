<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdBilling_Regularisation.aspx.cs" Inherits="LoanAdmin_S3GClnReBilling" Title="Bill Generation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Bill Generation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
              
                        <cc1:TabContainer ID="tcBilling" runat="server" CssClass="styleTabPanel" ScrollBars="None"
                            Width="98%" ActiveTabIndex="0">
                            <cc1:TabPanel ID="TabMainPage" runat="server" BackColor="Red" CssClass="tabpan" HeaderText="Process"
                                Width="98%">
                                <HeaderTemplate>
                                    Process
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td width="100%" valign="top">
                                                <asp:Panel ID="Panel1" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                                    Width="99%">
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlLOB" runat="server" onchange="fnAssignLOB(this)" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select a Line of Business" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>
                                                           <%-- <td class="styleFieldLabel">
                                                                <asp:Label ID="lblFrequency" runat="server" CssClass="styleReqFieldLabel" Text="Frequency"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlFrequency" runat="server" onchange="fnAssignFrequency(this)">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="ddlFrequency"
                                                                    Display="None" InitialValue="0" ErrorMessage="Select a Frequency" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>--%>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblMonthYear" runat="server" CssClass="styleReqFieldLabel" Text="Month/Year"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtMonthYear" runat="server" Width="70px" AutoPostBack="true" OnTextChanged="txtMonthYear_TextChanged"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calMonthYear" Format="MMM-yyyy" TodaysDateFormat="MMM-yyyy"
                                                                    OnClientShown="onShown" OnClientHidden="onHidden"
                                                                    runat="server" DefaultView="Months" Enabled="True" TargetControlID="txtMonthYear"
                                                                    PopupButtonID="imgMonthYear">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgMonthYear" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblStartDate" runat="server" CssClass="styleReqFieldLabel" Text="Start Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtStartDate" runat="server" ReadOnly="true" Width="75px"></asp:TextBox>
                                                                <%--<cc1:CalendarExtender ID="calStartDate" runat="server" Enabled="True" TargetControlID="txtStartDate"
                                                                    PopupButtonID="imgStartDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />--%>
                                                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                                                                    Display="None" ErrorMessage="Select a Start Date" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblEndDate" runat="server" CssClass="styleReqFieldLabel" Text="End Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" colspan="2">
                                                                <asp:TextBox ID="txtEndDate" runat="server" ReadOnly="true" Width="75px"></asp:TextBox>
                                                                <%--<cc1:CalendarExtender ID="calEndDate" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                                    PopupButtonID="imgEndDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />--%>
                                                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                                                                    Display="None" ErrorMessage="Select a End Date" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>

                                    <table width="100%">
                                        <tr>
                                            <td width="100%">
                                                <asp:Panel ID="panSchedule" runat="server" GroupingText="Schedule Details" CssClass="stylePanel"
                                                    Width="99%">
                                                    <table width="100%" border="0">
                                                        <tr>
                                                            <td class="styleFieldLabel" width="33%">
                                                                <asp:RadioButtonList ID="rbtnSchedule" runat="server" AutoPostBack="True" AppendDataBoundItems="True"
                                                                    RepeatDirection="Horizontal" OnSelectedIndexChanged="rbtnSchedule_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Schedule Now" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Text="Schedule At :" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td class="styleFieldLabel" width="33%">
                                                                <asp:Label ID="lblScheduleDate" runat="server" Text="Date"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:TextBox ID="txtScheduleDate" runat="server" Width="90px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calScheduleDate" runat="server" Enabled="True" TargetControlID="txtScheduleDate"
                                                                    PopupButtonID="imgScheduleDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgScheduleDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            </td>
                                                            <td class="styleFieldLabel" width="33%">
                                                                <asp:Label ID="lblScheduleTime" runat="server" Text="Time (HH:MM AM)"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                        <asp:TextBox ID="txtScheduleTime" runat="server" Width="100px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="REVScheduleTime" ValidationGroup="Go" runat="server"
                                                                    Display="None" ErrorMessage="Schedule Time Should be HH:MM AM Fomat(12 Hours)"
                                                                    ControlToValidate="txtScheduleTime" SetFocusOnError="True" ValidationExpression="^([0]?[1-9]|1[0-2])(:)[0-5][0-9]((:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm)$"></asp:RegularExpressionValidator>
                                                            </td>
                                                           
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>

                                        <tr align="center">
                                            <td align="center" colspan="3">
                                                <table>
                                                    <tr align="center">
                                                         <td class="styleFieldAlign" align="center">
                                                                <asp:Button ID="btnGonw" runat="server" CssClass="styleSubmitShortButton" OnClick="btnGo_Click"
                                                                    Text="Go" ValidationGroup="Go"/>
                                                            </td>
                                                            
                                                  
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>


                                    <table width="100%">
                                        <tr>
                                            
                                            <td width="70%">
                                                <asp:Panel runat="server" ID="pnlBranch" CssClass="stylePanel" GroupingText="Location Details"
                                                    Width="99%" Visible="False">
                                                    <div runat="server" id="div1" class="container" style="height: 139px; overflow-x: hidden; overflow-y: scroll;">
                                                        <asp:GridView ID="gvBranchWise" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                                            EmptyDataText="Billing already processed…!" Width="97%" OnRowDataBound="gvBranchWise_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAllBranch" runat="server" onclick="javascript:fnSelectAll(this,'chkSelectBranch');" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectBranch" runat="server" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Branch Id" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBranchId" runat="server" Text='<%#Bind("Location_Id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Location" HeaderText="Location">
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="AccountCount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblaccountcount" runat="server" Text='<%#Bind("AccountCount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lbltotaccountcount" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                               <asp:TemplateField HeaderText="OPC Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblopcamount" runat="server" Text='<%#Bind("DebitAmount") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                     <FooterTemplate>
                                                                        <asp:Label ID="lbltotopcamount" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="JVReference" HeaderText="System JV Reference" />
                                                                <asp:TemplateField HeaderText="Funder Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFunderamt" runat="server" Text='<%#Bind("funder_amt") %>' MaxLength="100"></asp:Label>
                                                                    </ItemTemplate>
                                                                      <FooterTemplate>
                                                                        <asp:Label ID="lbltotFunderamt" runat="server"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Remarks">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Bind("Remarks") %>' MaxLength="100"></asp:TextBox>
                                                                    </ItemTemplate>
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
                            <cc1:TabPanel ID="TabControlDataSheet" runat="server" BackColor="Red" CssClass="tabpan"
                                HeaderText="Process" Width="98%">
                                <HeaderTemplate>
                                    Control Data Sheet
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%" Visible="false">
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblDataLOB" runat="server" CssClass="styleReqdFieldLabel" Text="Line of Business"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtDataLOB" runat="server" ReadOnly="True" Width="150px"></asp:TextBox>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="Label3" runat="server" CssClass="styleReqdFieldLabel" Text="Frequency"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtDataFrequency" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblDataMonthYear" runat="server" CssClass="styleReqdFieldLabel" Text="Month/Year"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtDataMonthYear" runat="server" ReadOnly="True" Width="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel runat="server" ID="pnlControlData" CssClass="stylePanel" GroupingText="Control Data for Billing"
                                                    Width="99%" Visible="False">
                                                    <br />
                                                    <div id="div2" style="overflow: auto; width: 98%; padding-left: 1%;" runat="server"
                                                        border="1">
                                                        <asp:GridView ID="gvControlDataSheet" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                                            Width="100%" OnRowCommand="gvControlDataSheet_RowCommand" EnableModelValidation="True">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Location" DataField="Location" />
                                                                <asp:BoundField HeaderText="Type of Billing" DataField="BillingType" />
                                                                <asp:BoundField HeaderText="No of Accounts" DataField="AccountCount">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:BoundField HeaderText="Debit Amount" DataField="DebitAmount">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="View" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                            CommandArgument='<%# Bind("Billing_Control_Id") %>' CommandName="Query" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="60px" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:GridView ID="gvBranchTotal" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                                            Width="100%" EnableModelValidation="True">
                                                            <Columns>
                                                                <asp:BoundField HeaderText="Location" DataField="Branch" />
                                                                <asp:BoundField HeaderText="Location Total Amount" DataField="BranchTotal">
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                    <br />
                                                    <asp:GridView ID="grvAccounts" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                                        Width="98%" EnableModelValidation="True">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Type of Billing" DataField="BillingType" />
                                                            <asp:BoundField HeaderText="Rental Schedule Number" DataField="PANum" />
                                                            <asp:BoundField HeaderText="Sub Account Number" DataField="SANum" Visible="false" />
                                                            <asp:BoundField HeaderText="Debit Amount" DataField="DebitAmount">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <br />
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="100%" align="right">
                                                                <asp:UpdatePanel ID="upAccounts" runat="server">
                                                                    <ContentTemplate>
                                                                        <asp:Button runat="server" ID="btnXLPorting" Text="Export" CssClass="styleGridShortButton"
                                                                            OnClick="btnXLPorting_Click" Enabled="false" ToolTip="Export Accounts" />
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:PostBackTrigger ControlID="btnXLPorting" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabBillOutput" runat="server" BackColor="Red" CssClass="tabpan"
                                HeaderText="Process" Width="98%">
                                
                                <HeaderTemplate>
                                   Debit/Credit Note
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                    <table>
                                       <tr>
                                            <td class="styleFieldLabel">
                                        <asp:Label ID="lblcust" runat="server" CssClass="styleReqFieldLabel" Text="Lessee"></asp:Label>

                                    </td>
                                    <td class="styleFieldLabel">
                                        <uc2:Suggest ID="ddlcust" runat="server" ServiceMethod="GetCustList"   />

                                       
                                    </td>
                                           </tr>
                                       </table>
                                    <table width="100%" align="center" >
                                        <tr align="center">
                                            
                                            <td>
                                                <asp:Button ID="btnFetch" runat="server" CssClass="styleSubmitButton" OnClick="btnFetch_Click"
                                                    Text="Fetch" />
                                             
                                            </td>
                                        </tr>   
                                          <tr>
                        <td>
                           
                                         <asp:Panel ID="pnlrs" runat="server" GroupingText="RS Details" Visible="False">
                            <div id="divacc" runat="server" style="height: 150px; overflow: auto; display: none">
                                 <table width="100%">
                                <tr>
                                    <td>
                                <asp:GridView ID="grvRental" runat="server" AutoGenerateColumns="False"
                                    BorderWidth="2px" EnableModelValidation="True" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl.No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSNO" runat="server"  Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>

                                            </ItemTemplate>

                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lessiee">
                                            <ItemTemplate>
                                                <asp:Label ID="lbllessiee"  runat="server" Text='<%#Eval("cust_name")%>' ToolTip="Account No"></asp:Label>

                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                            <asp:TemplateField HeaderText="State">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                   
                                    <ItemTemplate>
                                        <asp:Label ID="lblstate"  runat="server" Text='<%#Eval("State")%>' ToolTip="Account No"></asp:Label>
                                    </ItemTemplate>
                                             <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Debit">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                   
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit"  runat="server" Text='<%#Eval("Debit")%>' ToolTip="Account No"></asp:Label>
                                    </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Credit">
                                    <HeaderStyle CssClass="styleGridHeader" />
                                   
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit"  runat="server" Text='<%#Eval("Credit")%>' ToolTip="Account No"></asp:Label>
                                    </ItemTemplate>
                                             <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                    </Columns>

                                    <RowStyle HorizontalAlign="Center" />
                                </asp:GridView>
</td>
                                </tr>
                                        
                            </table>
                            </div>
                                </asp:Panel>
                                    
                             
                           

                        </td>
                    </tr>
                                          

                                    </table>
                                 <table width="100%" align="center" >
                                     <tr align="center">
                                         <td align="center">
                                               <asp:Button ID="btnExcel" runat="server" Visible="false" CausesValidation="False" CssClass="styleSubmitButton"
                                                                        Text="Debit Annexures" OnClick="btnExcel_Click" />
                                                            &nbsp;&nbsp;&nbsp;
                                                             <asp:Button ID="btncredit" Visible="false" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                        Text="Credit Annexures" OnClick="btncredit_Click" />

                                         </td>
                                     </tr>
                                     <tr align="center">
                                         <td align="center">
                                           <asp:Button ID="btnDebitPrint" Visible="false" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                        Text="Debit Note" OnClick="btnDebitPrint_Click" />
                                                            &nbsp;&nbsp;&nbsp;
                                                             <asp:Button ID="btnCreditPrint" Visible="false" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                        Text="Credit Note" OnClick="btnCreditPrint_Click"  />
                                         </td>
                                     </tr>
                                 </table>
                                              </ContentTemplate>
                                          <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
             <asp:PostBackTrigger ControlID="btncredit" />
        </Triggers>
                                        </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                        </ContentTemplate>
                   
       
                     </asp:UpdatePanel>                    
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="2">
                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td valign="top" align="left" colspan="2">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" colspan="2">
                                        <table cellpadding="2" cellspacing="0" border="0" width="100%">

                                            <tr>
                                                <td colspan="5" align="center">
                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnSave" runat="server" Enabled="false" CausesValidation="true" CssClass="styleSubmitButton"
                                                                Text="Save" ValidationGroup="btnSave" OnClick="btnSave_OnClick" OnClientClick="return fnCheckPageValidators();" />
                                                            &nbsp;
                                                                    <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                        OnClientClick="return fnConfirmClear();" Text="Clear" OnClick="btnClear_OnClick" />
                                                            &nbsp;
                                                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                        Text="Cancel" OnClick="btnCancel_Click" />
                                                            &nbsp;&nbsp;&nbsp;
                                                           
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                          
                  
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldAlign">
                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                            <ContentTemplate>
                                                <asp:CustomValidator ID="cvBilling" runat="server" CssClass="styleMandatoryLabel"
                                                    Display="None" ValidationGroup="Submit"></asp:CustomValidator>
                                                <asp:ValidationSummary ID="vsBilling" runat="server" CssClass="styleMandatoryLabel"
                                                    ValidationGroup="Go" HeaderText="Please correct the following validation(s):" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
    <script language="javascript" src="../Scripts/jsBilling.js" type="text/javascript">
    </script>

    <script language="javascript" type="text/javascript">
        function fnAssignLOB(ddlLOB) {
            var varLob = ddlLOB.selectedIndex;
            var txtBillLOB = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabBillOutput_txtBillLOB');
            if (txtBillLOB != null) {
                txtBillLOB.value = ddlLOB.options[varLob].innerText;
            }
            //AssignStartEndDate();
        }
      
        function AssignStartEndDate() {
            
                var varMonthYear = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_txtMonthYear').value;
                var varDateFormat = Date.parseInvariant(varMonthYear, "MMM-yyyy");

                var varGPSFormat = '<%=strDateFormat %>';
                var varStartDate = new Date(varDateFormat.getFullYear(), varDateFormat.getMonth(), 1);
                varStartDate = varStartDate.format(varGPSFormat);
                document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_txtStartDate').value = varStartDate;
                var intLastDate = 28;
                if (varDateFormat.getMonth() == 0 || varDateFormat.getMonth() == 2 || varDateFormat.getMonth() == 4 || varDateFormat.getMonth() == 6 || varDateFormat.getMonth() == 7 || varDateFormat.getMonth() == 9 || varDateFormat.getMonth() == 11) {
                    intLastDate = 31;

                }

        }

        function fnSelectCashFlow(chkSelectCashFlow, chkSelectAllCF) {

            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_grvCashFlow');
            var TargetChildControl = chkSelectAllCF;
            var selectall = 0;
            var Inputs = grvCashFlow.getElementsByTagName("input");
            if (!chkSelectCashFlow.checked) {
                chkSelectAllCF.checked = false;
            }
            else {
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'checkbox') {
                        if (Inputs[n].checked) {
                            selectall = selectall + 1;
                        }
                    }
                }
                if (selectall == grvCashFlow.rows.length - 1) {
                    chkSelectAllCF.checked = true;
                }
            }

        }

        ///Function for Select/Unselect All Branches
        function fnSelectAllCF(chkSelectAllCF, chkSelectCashFlow) {
            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_grvCashFlow');
            var TargetChildControl = chkSelectCashFlow;
            //Get all the control of the type INPUT in the base control.
            var Inputs = grvCashFlow.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllCF.checked;
        }

    </script>

</asp:Content>
