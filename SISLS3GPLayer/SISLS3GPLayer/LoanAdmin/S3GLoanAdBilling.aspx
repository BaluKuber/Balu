<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLoanAdBilling.aspx.cs" Inherits="LoanAdmin_S3GClnBilling" Title="Bill Generation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

     <script type="text/javascript">
        

         function fnEMailVal(btn) {
             //btn.style.visibility = 'hidden';
             btn.disabled = true;
             //var a = event.srcElement;
             //a.style.display = 'block';
             //a.style.removeAttribute('display');
             document.getElementById('<%=btnEmailRender.ClientID%>').click();
             return true;
         }

     </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Bill Generation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <cc1:TabContainer ID="tcBilling" runat="server" CssClass="styleTabPanel" ScrollBars="None"
                            Width="98%" ActiveTabIndex="2">
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
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblFrequency" runat="server" CssClass="styleReqFieldLabel" Text="Month Year"></asp:Label>

                                                            </td>

                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtMonthYear" runat="server" Width="70px" AutoPostBack="true" OnTextChanged="txtMonthYear_TextChanged"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calMonthYear" Format="MMM-yyyy" TodaysDateFormat="MMM-yyyy"
                                                                    OnClientShown="onShown" OnClientHidden="onHidden"
                                                                    runat="server" DefaultView="Months" Enabled="True" TargetControlID="txtMonthYear"
                                                                    PopupButtonID="imgMonthYear">
                                                                </cc1:CalendarExtender>
                                                                <asp:HiddenField ID="hdndate" runat="server" />

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
                                                                <asp:TextBox ID="txtEndDate" runat="server" ReadOnly="true" Width="75px" AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                                                <%-- <cc1:CalendarExtender ID="calEndDate" runat="server" Enabled="True" TargetControlID="txtEndDate"
                                                                    PopupButtonID="imgEndDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />--%>
                                                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                                                                    Display="None" ErrorMessage="Select a End Date" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                            </td>

                                                        </tr>

                                                         <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblBillNumber" runat="server" CssClass="styleDisplayLabel" Text="Bill Number"></asp:Label>

                                                            </td>
                                                             <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtBillNumber" Enabled="false" runat="server" ReadOnly="true" Width="75px"></asp:TextBox></td>
                                                          </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>

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
                                                                <asp:Label ID="lblScheduleDate" runat="server" Text="Date"></asp:Label>&nbsp;&nbsp; 
                                                                        <asp:TextBox ID="txtScheduleDate" runat="server" Width="90px"></asp:TextBox>
                                                                <cc1:CalendarExtender ID="calScheduleDate" runat="server" Enabled="True" TargetControlID="txtScheduleDate"
                                                                    PopupButtonID="imgScheduleDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgScheduleDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                            </td>
                                                            <td class="styleFieldLabel" width="34%">
                                                                <asp:Label ID="lblScheduleTime" runat="server" Text="Time (HH:MM AM)"></asp:Label>&nbsp;&nbsp;
                                                                        <asp:TextBox ID="txtScheduleTime" runat="server" Width="100px"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="REVScheduleTime" ValidationGroup="Go" runat="server"
                                                                    Display="None" ErrorMessage="Schedule Time Should be HH:MM AM Fomat(12 Hours)"
                                                                    ControlToValidate="txtScheduleTime" SetFocusOnError="True" ValidationExpression="^([0]?[1-9]|1[0-2])(:)[0-5][0-9]((:)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm)$"></asp:RegularExpressionValidator>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" align="center">
                                                                <asp:Button ID="btnGonw" runat="server" CssClass="styleSubmitShortButton" OnClick="btnGo_Click"
                                                                    Text="Go" ValidationGroup="Go" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel runat="server" ID="pnlBranch" GroupingText="Location Details"
                                                    Width="99%" Visible="False">
                                                    <div runat="server" id="div1" class="container" style="height: 436px; overflow-x: hidden; overflow-y: scroll;">
                                                        <asp:GridView ID="gvBranchWise" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                                            EmptyDataText="Billing already processed....!" Width="97%" ShowFooter="true" OnRowDataBound="gvBranchWise_RowDataBound" EnableModelValidation="True">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAllBranch" runat="server" onclick="javascript:fnchkSelectAllBranch(this,'chkSelectBranch');" />
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

                                                                <asp:BoundField DataField="JVReference" Visible="false" HeaderText="System JV Reference" />
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
                                                                <asp:TemplateField HeaderText="Status">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStat" runat="server" Text='<%#Bind("Status") %>' MaxLength="100"></asp:Label>
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
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabControlDataSheet" runat="server" BackColor="Red" CssClass="tabpan"
                                HeaderText="Process" Width="98%" Visible="false">
                                <HeaderTemplate>
                                    Control Data Sheet
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
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

                                        <tr>
                                            <td colspan="6">
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
                                                            <asp:BoundField HeaderText="Prime Account Number" DataField="PANum" />
                                                            <asp:BoundField HeaderText="Sub Account Number" DataField="SANum" />
                                                            <asp:BoundField HeaderText="Debit Amount" DataField="DebitAmount">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <br />
                                                    <asp:UpdatePanel ID="upAccounts" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button runat="server" ID="btnXLPorting" Text="Export" CssClass="styleGridShortButton"
                                                                OnClick="btnXLPorting_Click" Enabled="false" ToolTip="Export Accounts" />

                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnXLPorting" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabBillOutput" runat="server" BackColor="Red" CssClass="tabpan"
                                HeaderText="Process" Width="98%">
                                <HeaderTemplate>
                                    Bill Generation Output
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="99%" border="0">
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel runat="server" ID="pnlCustomer" GroupingText="Lessee Details"
                                                    Width="98%" Visible="False">
                                                    <div runat="server" id="div3" class="container" style="height: 200px; overflow-x: hidden; overflow-y: scroll; vertical-align: top">
                                                        <asp:GridView ID="gvCustomer" runat="server" HorizontalAlign="Center" AutoGenerateColumns="False"
                                                            EmptyDataText="Billing already processed....!" Width="97%" ShowFooter="true"
                                                            OnRowDataBound="gvCustomer_RowDataBound" EnableModelValidation="True">
                                                            <Columns>

                                                                <asp:TemplateField HeaderText="Select">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAllBranch" runat="server" onclick="javascript:fnchkSelectAllCustomer(this,'chkSelectBranch');" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectBranch" runat="server" Checked='<%#Eval("Is_Checked")%>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Customer Id" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustomerId" runat="server" Text='<%#Bind("Customer_ID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Lessee Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblaccountcount" runat="server" Text='<%#Bind("Customer_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblcust" runat="server" Text="Lessee"></asp:Label>

                                            </td>
                                            <td class="styleFieldLabel">
                                                <uc2:Suggest ID="ddlcust" runat="server" ServiceMethod="GetCustList"
                                                    Width="250px" />
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblFun" runat="server" Text="Funder"></asp:Label>

                                            </td>
                                            <td class="styleFieldLabel">
                                                <uc2:Suggest ID="ddlFunder" runat="server" ServiceMethod="GetFundList"
                                                    Width="240px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="Label1" runat="server" CssClass="styleReqFieldLabel" Text="Action"></asp:Label>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:DropDownList ID="ddlRerun" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlRerun_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="PDF"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Rerun"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="To Cancel"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Cancelled"></asp:ListItem>
                                                    <asp:ListItem Value="4" Text="Send E-mail"></asp:ListItem>
                                                </asp:DropDownList>

                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="LblTranch" runat="server" Text="Tranche"></asp:Label>

                                            </td>
                                            <td class="styleFieldLabel">
                                                <uc2:Suggest ID="ddlTranche" runat="server" ServiceMethod="GetTrancheList"
                                                    Width="240px" />
                                            </td>
                                        </tr>
                                        <tr class="styleFieldAlign">
                                            <td class="styleFieldLabel">
                                                <asp:Label Text="Sales Invoice No" runat="server" ID="lblInvoice" CssClass="styleDisplayLabel" />
                                            </td>
                                            <td class="styleFieldAlign">

                                                <asp:TextBox ID="txtInvoice" runat="server" MaxLength="50" OnTextChanged="txtInvoice_OnTextChanged"
                                                    AutoPostBack="true" Width="182px"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" MinimumPrefixLength="1" OnClientPopulated="Invoice_ItemPopulated" OnClientItemSelected="Invoice_ItemSelected"
                                                    runat="server" TargetControlID="txtInvoice" ServiceMethod="GetInvoiceNo"
                                                    CompletionSetCount="5" Enabled="True" ServicePath="" CompletionListCssClass="CompletionList"
                                                    DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtInvoice"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnInvoice" runat="server" />

                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label Text="AMF Invoice No" runat="server" ID="lblAMFInvoice" CssClass="styleDisplayLabel" />
                                            </td>
                                            <td class="styleFieldAlign">

                                                <asp:TextBox ID="txtAMFInvoice" runat="server" MaxLength="50" OnTextChanged="txtAMFInvoice_OnTextChanged"
                                                    AutoPostBack="true" Width="182px"></asp:TextBox>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" MinimumPrefixLength="1" OnClientPopulated="AMFInvoice_ItemPopulated" OnClientItemSelected="AMFInvoice_ItemSelected"
                                                    runat="server" TargetControlID="txtAMFInvoice" ServiceMethod="GetAMFInvoiceNo"
                                                    CompletionSetCount="5" Enabled="True" ServicePath="" CompletionListCssClass="CompletionList"
                                                    DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                    CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </cc1:AutoCompleteExtender>
                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtAMFInvoice"
                                                    WatermarkText="--Select--">
                                                </cc1:TextBoxWatermarkExtender>
                                                <asp:HiddenField ID="hdnAMFInvoice" runat="server" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>

                                            </td>
                                            <td class="styleFieldLabel">
                                                <uc2:Suggest ID="ddlLocation" runat="server" ServiceMethod="GetBranchList"
                                                    Width="250px" />
                                            </td>
                                        </tr>
                                        <tr align="center">

                                            <td colspan="4">
                                                <asp:Button ID="btnFetch" runat="server" CssClass="styleSubmitButton" OnClick="btnFetch_Click"
                                                    Text="Fetch" ValidationGroup="Fetch" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">

                                                <asp:Panel ID="pnlrs" runat="server" GroupingText="RS Details" Visible="False" CssClass="stylePanel">
                                                    <div id="divacc" runat="server" style="height: 250px; overflow: auto; display: none">
                                                        <table width="98%">
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="grvRental" runat="server"  AutoGenerateColumns="False" OnRowDataBound="grvRental_RowDataBound"
                                                                        BorderWidth="2px" EnableModelValidation="True" OnRowCommand="grvRental_RowCommand">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Sl.No.">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>

                                                                                </ItemTemplate>

                                                                                <ItemStyle Width="2%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="RS Number">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblRS_Number" runat="server" Text='<%#Eval("RS_Number")%>' ToolTip="Account No"></asp:Label>
                                                                                     <asp:Label ID="lblInvoiceno" runat="server" Text='<%#Eval("Invoice_No")%>' ToolTip="Account No" Visible="false"></asp:Label>
                                                                                     <asp:Label ID="lblInvoiceNoamf" runat="server" Text='<%#Eval("Invoice_No_AMF")%>' ToolTip="Account No" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblStatewiseBilling" runat="server" Text='<%#Eval("State_Wise_Billing")%>' ToolTip="Account No" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblIs_Rental" runat="server" Text='<%#Eval("Is_Rental")%>' ToolTip="Account No" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblIs_AMF" runat="server" Text='<%#Eval("Is_AMF")%>' ToolTip="Account No" Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Location">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("Location")%>' ToolTip="Location"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField HeaderText="Invoice Generated On">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDateTime" runat="server" Text='<%#Eval("Print_Date")%>' ToolTip="Invoice Generated On"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                             <asp:TemplateField HeaderText="Email Group" ItemStyle-Width="200px">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEmail_Group" runat="server" Text='<%#Eval("Email_Group")%>' ToolTip="Email Group"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblPrintRental" runat="server" Text="Print Rental"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgbtnPrint" CssClass="styleGridEdit" ImageUrl="~/Images/pdf.png"
                                                                                        CommandArgument='<%# Bind("RS_Number") %>' CommandName="Print" runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblPrint" runat="server" Text="Print AMF"></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:ImageButton ID="imgbtnPrint1" CssClass="styleGridEdit" ImageUrl="~/Images/pdf.png"
                                                                                        CommandArgument='<%# Bind("RS_Number") %>' CommandName="Print AMF" runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Select" SortExpression="Select">
                                                                                <HeaderTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>Select All</td>
                                                                                            <td>
                                                                                                <asp:CheckBox ID="chkSelectAllRS" runat="server" onclick="javascript:fnSelectAll(this,'chkSelectRS');" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelectRS" runat="server" ToolTip="Select Account" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Digi_Flag" Visible ="false">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDigi_Flag" runat="server" Text='<%#Eval("Digi_Sign_Enable")%>' ></asp:Label>
                                                                                </ItemTemplate>                                                                                
                                                                            </asp:TemplateField>
                                                                        </Columns>

                                                                        <RowStyle HorizontalAlign="Center" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr align="right">
                                                                <td>
                                                                    <asp:Button ID="btnRerun" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                                        Text="Re-run" OnClick="btnRerun_Click" Visible="False" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </asp:Panel>




                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Panel ID="pnlTranche" runat="server" GroupingText="Tranche Details" Visible="False">
                                                    <div id="divtranche" runat="server" style="height: 200px; overflow: auto; display: none">
                                                        <asp:GridView ID="grvtranche" runat="server" AutoGenerateColumns="False"
                                                            BorderWidth="2px" EnableModelValidation="True">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle Width="2%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RS Number">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRS_Number" runat="server" Text='<%#Eval("Tranche_Name")%>' ToolTip="Account No"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="Select" SortExpression="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Select Account" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr>
            <td class="styleFieldLabel" align="center">
                <table align="center">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnSave" runat="server" Enabled="false" CausesValidation="true" CssClass="styleSubmitButton"
                                        Text="Save" ValidationGroup="btnSave" OnClick="btnSave_OnClick" OnClientClick="return fnCheckPageValidators();" />

                                    <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                        OnClientClick="return fnConfirmClear();" Text="Clear" OnClick="btnClear_OnClick" />

                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                        Text="Back" OnClick="btnCancel_Click" />

                                    <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                        Text="Print Rental" OnClick="btnPrint_Click" />

                                    <asp:Button ID="btnPrintAMF" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                        Text="Print AMF" OnClick="btnPrintAMF_Click" />

                                    <asp:Button ID="btnRentalCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Rental Invoice Cancel" ValidationGroup="btnInvoiceCancel" OnClick="btnRentalCancel_Click"
                                        Enabled="false" />

                                    <asp:Button ID="BtnAMFCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="AMF Invoice Cancel" ValidationGroup="btnInvoiceCancel" OnClick="btnAMFCancel_Click"
                                        Enabled="false" />

                                    
                                    <asp:Button ID="bntDownload" runat="server" CausesValidation="false" CssClass="styleSubmitButton" Visible="false"
                                        Text="Download Invoice" OnClick="bntDownload_Click" />

                                     <asp:Button ID="btnSendEmail" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Send EMail"  OnClientClick="return fnEMailVal(this)" ToolTip="Send EMail"
                                        Enabled="false" />

                                    <asp:Button runat="server" ID="btnEmailRender" CausesValidation="false" OnClick="btnEmail_Click" Width="1px" style="visibility:hidden;" />

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <tr>
            <td class="styleFieldAlign">
                <asp:CustomValidator ID="cvBilling" runat="server" CssClass="styleMandatoryLabel"
                    Display="None" ValidationGroup="Submit"></asp:CustomValidator>
                <asp:ValidationSummary ID="vsBilling" runat="server" CssClass="styleMandatoryLabel"
                    ValidationGroup="Go" HeaderText="Please correct the following validation(s):" />
                <asp:ValidationSummary ID="vsBilling1" runat="server" CssClass="styleMandatoryLabel"
                    ValidationGroup="Fetch" HeaderText="Please correct the following validation(s):" />
            </td>
        </tr>


    </table>


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
            var varMonthYear = document.getElementById('<%=txtMonthYear.ClientID%>').value;
            var varDateFormat = Date.parseInvariant(varMonthYear, "MMM-yyyy");

            var varGPSFormat = '<%=strDateFormat %>';
            var varStartDate = new Date(varDateFormat.getFullYear(), varDateFormat.getMonth(), 1);
            varStartDate = varStartDate.format(varGPSFormat);
            document.getElementById('<%=txtStartDate.ClientID%>').value = varStartDate;

            var varEndDate = new Date(varDateFormat.getFullYear(), varDateFormat.getMonth() + 1, 0);
            varEndDate = varEndDate.format(varGPSFormat);
            document.getElementById('<%=txtEndDate.ClientID%>').value = varEndDate;
            (document.getElementById('<%=txtStartDate.ClientID%>').value).ReadOnly = true;
        }

        //function fnSelectCashFlow(chkSelectCashFlow, chkSelectAllCF) {

        //    var grvCashFlow1 = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_grvCashFlow');
        //    var TargetChildControl = chkSelectAllCF;
        //    var selectall = 0;
        //    var Inputs = grvCashFlow1.getElementsByTagName("input");
        //    if (!chkSelectCashFlow.checked) {
        //        chkSelectAllCF.checked = false;
        //    }
        //    else {
        //        for (var n = 0; n < Inputs.length; ++n) {
        //            if (Inputs[n].type == 'checkbox') {
        //                if (Inputs[n].checked) {
        //                    selectall = selectall + 1;
        //                }
        //            }
        //        }
        //        if (selectall == grvCashFlow1.rows.length - 1) {
        //            chkSelectAllCF.checked = true;
        //        }
        //    }
        //}

        ///Function for Select/Unselect All Branches

        function fnSelectAll(chkSelectAllRS, chkSelectRS) {
            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabBillOutput_grvRental');
            var TargetChildControl = chkSelectRS;
            //Get all the control of the type INPUT in the base control.
            var Inputs = grvCashFlow.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllRS.checked;
        }

        function fnchkSelectAllBranch(chkSelectAllBranch, chkSelectBranch) {
            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_gvBranchWise');
            var TargetChildControl = chkSelectBranch;
            //Get all the control of the type INPUT in the base control.
            var Inputs = grvCashFlow.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllBranch.checked;
        }

        function fnchkSelectAllCustomer(chkSelectAllBranch, chkSelectBranch) {
            var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabBillOutput_gvCustomer');
            var TargetChildControl = chkSelectBranch;
            //Get all the control of the type INPUT in the base control.
            var Inputs = grvCashFlow.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllBranch.checked;
        }


        //function chkSelectAllBranch(chkSelectAllBranch, chkSelectBranch) {
        //    var grvCashFlow = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_gvBranchWise');
        //    var TargetChildControl = chkSelectBranch;
        //    //Get all the control of the type INPUT in the base control.
        //    var Inputs = grvCashFlow.getElementsByTagName("input");
        //    //Checked/Unchecked all the checkBoxes in side the GridView.
        //    for (var n = 0; n < Inputs.length; ++n)
        //        if (Inputs[n].type == 'checkbox' &&
        //    Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
        //            Inputs[n].checked = chkSelectBranch.checked;
        //}

        //function fnSelectAllCF1(objHeaderChk, grdId) {

        //    debugger;

        //    var IsChecked = objHeaderChk.checked;
        //    var tbl = document.getElementById(grdId);
        //    var items = tbl.getElementsByTagName('input');

        //    for (i = 0; i < items.length; i++) {
        //        if (items[i].type == "checkbox") {
        //            if (items[i].checked != IsChecked) {
        //                items[i].click();
        //            }
        //        }
        //    }

        //}

        function Invoice_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnInvoice.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function Invoice_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnInvoice.ClientID %>');
            hdnLessee.value = '';
        }

        function AMFInvoice_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnAMFInvoice.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function AMFInvoice_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnAMFInvoice.ClientID %>');
            hdnLessee.value = '';
        }
    </script>

</asp:Content>
