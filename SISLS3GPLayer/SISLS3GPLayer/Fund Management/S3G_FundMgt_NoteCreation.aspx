<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_FundMgt_NoteCreation.aspx.cs"
    Inherits="Fund_Management_S3G_FundMgt_NoteCreation" %>

<%--<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript">
        function ChkDiscountRate(txt) {
            document.getElementById('<%=hidsave.ClientID %>').value = "0";
            if (txt.value == '') {
                alert('Discounting Rate should not be blank');
            }
        }

        function ChkDiscountRateBlank(txt) {
            var row = txt.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            if (row.cells[8].children[0].value == '') {
                row.cells[8].children[0].focus();
            }
        }
        function Funsetprocess() {


            document.getElementById('<%=hidsave.ClientID %>').value = "0";
        }

        function FunsetprocessText(txt, option) {

            document.getElementById('<%=hidsave.ClientID %>').value = "0";
            var txt1 = document.getElementById('<%=txtcheck.ClientID %>');
            txt1.value = "";

            var row = txt.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            if (txt.value == "")
                txt.value = 0;
            //var Disc_value = parseFloat(txt.value);
            //switch (option) {
            //    case 'A':
            //        var source_value = (parseFloat(row.cells[4].innerText));
            //        break;               
            //}

        }

    </script>
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>

            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
            </table>

            <cc1:TabContainer ID="tcFunder" runat="server" CssClass="styleTabPanel" Width="99%" ScrollBars="None" ActiveTabIndex="0">
                <cc1:TabPanel runat="server" HeaderText="Note" ID="tbgeneral" CssClass="tabpan"
                    BackColor="Red" ToolTip="General" TabIndex="0">
                    <HeaderTemplate>
                        Note
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table width="99%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td width="49%">
                                                <asp:Panel ID="pnlinput" GroupingText="Input Details" runat="server" Height="50%" CssClass="stylePanel">
                                                    <table>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblcust" runat="server" CssClass="styleReqFieldLabel" Text="Lessee Name"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc2:Suggest ID="ddlcust" runat="server" ServiceMethod="GetCustList" IsMandatory="true"
                                                                    ErrorMessage="Select Lessee Name" ValidationGroup="vsSave" Width="250px" />
                                                                <input type="hidden" runat="server" id="hdnID" />
                                                                </input>
                                                              </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblFunder" runat="server" CssClass="styleReqFieldLabel" Text="Funder"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <uc2:Suggest ID="ddlfund" runat="server" AutoPostBack="true" ServiceMethod="GetVendors" IsMandatory="true"
                                                                    OnItem_Selected="ddlfund_OnItem_Selected" ErrorMessage="Select Funder" ValidationGroup="vsSave" Width="250px" />
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblFunderState" runat="server" CssClass="styleReqFieldLabel" Text="Funder State"></asp:Label>
                                                           </td>
                                                               <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:DropDownList ID="ddlFunderState" runat="server" AutoPostBack="True">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvddlFunderState" runat="server" ControlToValidate="ddlFunderState"
                                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ErrorMessage="Select Funder State" ValidationGroup="vsSave" Width="250px"></asp:RequiredFieldValidator>
                                                                   <%--<cc1:TextBoxWatermarkExtender ID="txtFundStateExtender" runat="server" TargetControlID="ddlFunderState"
                                                                     WatermarkText="--Select--">
                                                                   </cc1:TextBoxWatermarkExtender>--%>
                                                                        </td>

                                                          </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblNotenumber" runat="server" CssClass="styleDisplayLabel" Text="Note Number"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtnote" runat="server" ReadOnly="True"></asp:TextBox>

                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblNotedate" runat="server" CssClass="styleDisplayLabel" Text="Note Date" Visible="False"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtnotedate" runat="server" Visible="False"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvnotedate" Visible="False" runat="server" ControlToValidate="txtnotedate" SetFocusOnError="True"
                                                                    ErrorMessage="Select Note Date" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender ID="CalendarExtenderNotedate" runat="server" Enabled="True"
                                                                    TargetControlID="txtnotedate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="LblNODDate" runat="server" CssClass="styleReqFieldLabel" Visible="False" Text="NOD Date"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtNODDate" Visible="False" runat="server"></asp:TextBox>

                                                                <cc1:CalendarExtender ID="CalendarExtenderNODDat" runat="server" Enabled="False"
                                                                    TargetControlID="txtNODDate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblDisbursementDate" runat="server" CssClass="styleReqFieldLabel" Text="Disbursement Date"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtDisbursementdate" runat="server" AutoPostBack="True" onchange="Funsetprocess();"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDisbursementdate" SetFocusOnError="True"
                                                                    ErrorMessage="Select Disbursementdate Date" CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                                <cc1:CalendarExtender ID="CalendarExtendertxtDisbursementdate" runat="server" Enabled="True"
                                                                    TargetControlID="txtDisbursementdate">
                                                                </cc1:CalendarExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblGraceDays" runat="server" CssClass="styleDisplayLabel" Text="Grace Days"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtgraceDays" runat="server" Text="0" Style="text-align: right;" MaxLength="4" onchange="Funsetprocess();"></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender ID="ftbeGraceDays" runat="server" FilterType="Numbers,Custom" ValidChars="-"
                                                                    TargetControlID="txtgraceDays" Enabled="True">
                                                                </cc1:FilteredTextBoxExtender>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblstatus" runat="server" CssClass="styleDisplayLabel" Text="Status"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True"></asp:TextBox>

                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblBankCode" runat="server" CssClass="styleReqFieldLabel" Text="Funder Bank"></asp:Label>

                                                            </td>
                                                            <td class="styleFieldAlign">
                                                                <asp:DropDownList ID="ddlFunderBank" runat="server"></asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlFunderBank" runat="server" ControlToValidate="ddlFunderBank" InitialValue="0"
                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="vsSave" SetFocusOnError="True"
                                                                    ErrorMessage="Select Funder Bank"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>

                                            </td>
                                            <td width="1%" valign="middle">
                                                <asp:Button ID="btnfetch" runat="server" Text=">>" ToolTip="Fetch" OnClick="btnfetch_OnClick" UseSubmitBehavior="False" />
                                            </td>

                                            <td width="49%">
                                                <asp:Panel ID="pnlTranche" CssClass="stylePanel" GroupingText="Tranche Details" runat="server" Visible="False">
                                                    <div id="divacc" runat="server" style="height: 210px; overflow: auto; display: none">
                                                        <asp:GridView ID="grvtranche" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvtranche_RowDataBound"
                                                            BorderWidth="2px">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle Width="2%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tranche Id" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTrancheid" runat="server" Text='<%#Eval("Tranche_id")%>' ToolTip="Account No"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tranche Name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTranchename" runat="server" Text='<%#Eval("Tranche_name")%>' ToolTip="Account No"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="PASA_id" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPASA_id" runat="server" Text='<%#Eval("PA_SA_REF_ID")%>' ToolTip="Account No"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Tenure" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTenure" runat="server" Text='<%#Eval("Tenure")%>' ToolTip="Sub Account No"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Frequency" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblfrequency" runat="server" Text='<%#Eval("frequency")%>' ToolTip="Sub Account No"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="dayss" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldayss" runat="server" Text='<%#Eval("dayss")%>' ToolTip="Sub Account No"></asp:Label>

                                                                    </ItemTemplate>

                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Select" SortExpression="Select">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelectAccount" runat="server" ToolTip="Select Account" AutoPostBack="true" OnCheckedChanged="chkSelectAccount_OnCheckedChanged" />
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
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <table>
                                        <tr class="styleButtonArea" align="center">
                                            <td align="center">
                                                <asp:Button ID="btnProcess" runat="server" CssClass="styleSubmitButton" Text="Process" Visible="False" OnClick="btnProcess_OnClick"></asp:Button>
                                                <asp:TextBox ID="txtcheck" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlFund" runat="server" GroupingText="Funder Details" CssClass="stylePanel" Visible="False">
                                        <div id="divfund" runat="server" style="width: 900px; height: 150px; overflow-x: scroll; overflow-y: scroll; display: none">
                                            <asp:GridView ID="grvfund" runat="server" AutoGenerateColumns="False"
                                                BorderWidth="2px" OnRowDataBound="grvfund_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl.No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sanction id" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsanction_id" runat="server" Text='<%#Eval("sanction_id")%>' ToolTip="Account No"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sanctioned Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSanctionumber" runat="server" Text='<%#Eval("Sanctioned_No")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="End User Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEnduser" runat="server" Text='<%#Eval("Enduser_name")%>' Width="150px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sanctioned Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSanctiondate" runat="server" Text='<%#Eval("Sanctioned_Date")%>' Width="100px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Expiry Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblexpirydate" runat="server" Text='<%#Eval("expiry_date")%>' Width="100px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Available limit">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblavailablelimit" runat="server" Text='<%#Eval("available_limit")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sanctioned limit">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblsanctionedlimit" runat="server" Text='<%#Eval("Sanctioned_limit")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fore Closure Rate">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblforeclosurerate" runat="server" Text='<%#Eval("Foreclosure_Rate")%>' Width="100px" Style="text-align: right"
                                                                MaxLength="15"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Discounting Rate">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblDiscountrate" runat="server" Text='<%#Eval("discount_rate")%>' Width="100px" Style="text-align: right" MaxLength="15" onChange="ChkDiscountRate(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Cheque Return Charges">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblChq_Rtn_Charges" runat="server" Text='<%#Eval("Chq_Rtn_Charges")%>' Width="100px" Style="text-align: right"
                                                                MaxLength="16" onfocus="ChkDiscountRateBlank(this);"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="ODI Rate">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblODI_Rate" runat="server" Text='<%#Eval("ODI_Rate")%>' Width="100px" Style="text-align: right"
                                                                MaxLength="5"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Processing FeeAmount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblProcessingFeeAmount" runat="server" Text='<%#Eval("Processing_Fee_Amount")%>' Width="120px" Style="text-align: right"
                                                                MaxLength="16" OnTextChanged="lblProcessing_Fee_Amount_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Misc Charges">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblMisc_Charges" runat="server" Text='<%#Eval("Misc_Charges")%>' Width="100px" Style="text-align: right"
                                                                MaxLength="16"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PV Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldiscountamount" runat="server" Text='<%#Eval("Discount_amount")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service Tax" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPFServiceTax" runat="server" Text='<%#Eval("PF_Service_Tax")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ServiceTax_Perc" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPFSTPerc" runat="server" Text='<%#Eval("Serice_Tax_Perc")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>

                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlcashflow" runat="server" GroupingText="Cash Flow Details" Visible="False" CssClass="stylePanel">
                                        <div id="divcashflow" runat="server" style="overflow: scroll; width: 900px; height: 150px; display: none">
                                            <asp:GridView ID="gvOutFlow" runat="server" AutoGenerateColumns="False" ShowFooter="True" BorderWidth="2px" Width="98%" OnRowDataBound="gvOutFlow_RowDataBound" OnRowDeleting="gvOutFlow_RowDeleting" OnRowCommand="gvOutFlow_RowCommand">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtDate_GridOutflow" runat="server" Width="100px">
                                                            </asp:TextBox>
                                                            <cc1:CalendarExtender ID="CalendarExtenderSD_OutflowDate" runat="server" Enabled="True"
                                                                TargetControlID="txtDate_GridOutflow">
                                                            </cc1:CalendarExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtDate_GridOutflow" runat="server" ControlToValidate="txtDate_GridOutflow"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Date in Outflow"></asp:RequiredFieldValidator>

                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate_GridOutflow" runat="server" Text='<%# Bind("Date") %>'></asp:Label>

                                                        </ItemTemplate>


                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cash flow Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbltype" runat="server" Text='<%# Bind("CashFlow_Type") %>'></asp:Label>
                                                            <asp:Label ID="lblid" runat="server" Visible="false" Text='<%# Bind("CashFlow_Type_id") %>'></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddloutflowtype" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvdloutflowtype" runat="server" ControlToValidate="ddloutflowtype"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                InitialValue="0" ErrorMessage="Select a Cash flow Type in Outflow"></asp:RequiredFieldValidator>

                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cash flow Description">
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlOutflowDesc" runat="server" Width="99%"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvddlOutflowDesc" runat="server" ControlToValidate="ddlOutflowDesc"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                InitialValue="0" ErrorMessage="Select a Cash flow Description in Outflow"></asp:RequiredFieldValidator>

                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOutflowDesc" runat="server" Text='<%# Bind("CashOutFlow") %>'></asp:Label>
                                                            <asp:Label ID="lblcahflowdesc" runat="server" Visible="false" Text='<%# Bind("CashOutFlow_id") %>'></asp:Label>

                                                        </ItemTemplate>


                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment to">
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlPaymentto_OutFlow" runat="server" Width="99%" AutoPostBack="True">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvddlPaymentto_OutFlow" runat="server" ControlToValidate="ddlPaymentto_OutFlow"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                InitialValue="0" ErrorMessage="Select Payment to in Outflow"></asp:RequiredFieldValidator>

                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPaymentto" runat="server" Text='<%# Bind("OutflowFrom") %>'></asp:Label>
                                                            <asp:Label ID="lbloutflowid" runat="server" Visible="false" Text='<%# Bind("OutflowFrom_id") %>'></asp:Label>

                                                        </ItemTemplate>


                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Entity Name">
                                                        <FooterTemplate>
                                                            <uc2:Suggest ID="ddlEntityName_OutFlow" runat="server" ServiceMethod="GetVendors"
                                                                ErrorMessage="Select a Customer/Entity Name in Outflow" Width="250px"
                                                                ValidationGroup="Outflow" IsMandatory="true" />


                                                        </FooterTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEntityName_OutFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                            <asp:Label ID="Label2" runat="server" Visible="false" Text='<%# Bind("Entity_id") %>'></asp:Label>

                                                        </ItemTemplate>


                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount">
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAmount_Outflow" runat="server" MaxLength="10" Width="100px" ToolTip="OutFlow Amount" Style="text-align: right">
                                                            </asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Outflow" runat="server" FilterType="Numbers"
                                                                TargetControlID="txtAmount_Outflow">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtAmount_Outflow" runat="server" ControlToValidate="txtAmount_Outflow"
                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                ErrorMessage="Enter the Amount in Outflow"></asp:RequiredFieldValidator>

                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAmount_Outflow" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>

                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />

                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <FooterTemplate>
                                                            <asp:Button ID="btnAddOut" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Outflow" CommandName="Add"
                                                                CssClass="styleGridShortButton" />

                                                        </FooterTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                Text="Remove"></asp:LinkButton>

                                                        </ItemTemplate>


                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" HeaderText="Note" ID="TabPanel1" CssClass="tabpan"
                    BackColor="Red" ToolTip="General" TabIndex="1">
                    <HeaderTemplate>
                        PV Working
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div style="border-style: solid; border-color: lightblue; border-width: thin; width: 120px; margin-left: 10px;">
                            <asp:CheckBox ID="CHKDSRA" runat="server" Text="DSRA Applicable" OnCheckedChanged="Chk_DSRAChanged" Font-Bold="true"
                                AutoPostBack="true" />
                        </div>
                        <table width="100%" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlpv" runat="server" GroupingText="PV Details" CssClass="stylePanel">
                                        <div id="divpv" runat="server" class="container" style="width: 100%; height: 500px; overflow-x: hidden; overflow-y: scroll; display: none">
                                            <asp:GridView ID="grvPV" runat="server" AutoGenerateColumns="False"
                                                BorderWidth="1px" EnableModelValidation="True" ShowFooter="true" OnRowDataBound="grvpv_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl.No.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                                        </FooterTemplate>

                                                        <ItemStyle Width="2%" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Due Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldue_date" runat="server" Text='<%#Eval("Due_date")%>' ToolTip="Due Date"></asp:Label>

                                                        </ItemTemplate>

                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Rental">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRental" runat="server" Text='<%#Eval("Rental")%>' ToolTip="Rental"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="Totlblrental" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="AMF">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAMF" runat="server" Text='<%#Eval("AMF")%>' ToolTip="AMF"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="TotlblAMF" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VAT">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVAT" runat="server" Text='<%#Eval("VAT")%>' ToolTip="VAT"></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="TotlblVAT" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Service Tax">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceTax" runat="server" Text='<%#Eval("ServiceTax")%>' ToolTip="Service Tax"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="TotlblServiceTax" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="DSRA">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDsra" Style="text-align: right" runat="server" Text='<%#Eval("DSRA")%>' Enabled="false" ToolTip="DSRA" Width="100px" onchange="FunsetprocessText(this,'A');"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" ValidChars="."
                                                                FilterType="Numbers,Custom" Enabled="True"
                                                                TargetControlID="txtDsra">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTOtDsra" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PV Factor">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPVFactor" runat="server" Text='<%#Eval("Factor_Value")%>' ToolTip="PV Factor"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalPVFactor" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PV Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPVAmount" runat="server" Text='<%#Eval("PV_Amount")%>' ToolTip="PV Amount"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="TotlblPVAmount" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Funder Principal">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrincipal" runat="server" Text='<%#Eval("Principal")%>' ToolTip="Funder Principal"></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="TotlblPrincipal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Funder Interest">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInterest" runat="server" Text='<%#Eval("Interest")%>' ToolTip="Funder Interest"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="TotlblInterest" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Funder Balance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBalance" runat="server" Text='<%#Eval("Balance")%>' ToolTip="Funder Balance"></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="TotlblBalance" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="PV Principal">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFPrincipal" runat="server" Text='<%#Eval("Funder_Principal")%>' ToolTip="PV Principal"></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalFPrincipal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PV Interest">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFInterest" runat="server" Text='<%#Eval("Funder_Interest")%>' ToolTip="PV Interest"></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalFInterest" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PV Balance">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFBalance" runat="server" Text='<%#Eval("Funder_balance")%>' ToolTip="PV Balance"></asp:Label>

                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalFBalance" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
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
            <table width="98%">
                <tr class="styleButtonArea" align="center">
                    <td align="center" colspan="2">
                        <asp:Button ID="btnCalculatePV" runat="server" CssClass="styleSubmitButton"
                            Visible="False" Text="Calculate PV" OnClick="btnCalculatePV_OnClick"></asp:Button>
                        <asp:Button runat="server" ID="btnSave" ValidationGroup="vsSave" OnClientClick="return fnCheckPageValidators('vsSave');"
                            CssClass="styleSubmitButton" Text="Save" Enabled="True" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClick="btnClear_Click" OnClientClick="return confirm('Invalid input, do you want to clear this record?');" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click"
                            CssClass="styleSubmitButton" />
                        <asp:Button ID="btnview" runat="server" CssClass="styleSubmitButton" Text="View Sanction" OnClick="btnview_OnClick"></asp:Button>
                        <asp:Button runat="server" ID="btnExcel" Text="Excel" OnClick="btnExport_Click"
                            CssClass="styleSubmitButton" />
                        <asp:Button runat="server" ID="btnRSExport" Text="RS Export" OnClick="btnRSExport_Click"
                            CssClass="styleSubmitButton" />
                        <input type="hidden" runat="server" id="hidsave" />
                    </td>
                </tr>
                <tr class="styleButtonArea" align="center">
                    <td align="center" colspan="2">
                        <asp:Label ID="lblrftype" runat="server" Text="Report Format" CssClass="styleDisplayLabel" />
                        <asp:DropDownList ID="ddlReportFormatType" runat="server" />
                        <asp:Button runat="server" ID="btnPrint" Text="Print" CausesValidation="false" OnClick="btnPrint_Click"
                            CssClass="styleSubmitButton" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsSave" ValidationGroup="vsSave"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                        <asp:ValidationSummary runat="server" ID="vsOutflow" ValidationGroup="Outflow"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                        <asp:CustomValidator ID="cvTranche" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>

            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnRSExport" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>


