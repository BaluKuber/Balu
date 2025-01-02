<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLoanAdRSActivation_Add.aspx.cs"
    Inherits="LoanAdmin_S3GLoanAdRSActivation_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Title="RS Bulk Activation" %>

<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        //function window.onerror() {
        //    return true;
        //}

        //function Trim(strInput) {
        //    var FieldValue = document.getElementById(strInput).value;
        //    document.getElementById(strInput).value = FieldValue.trim();
        //}
        //function fnGetCompensationValue(CompensationValue) {
        //    var exp = /_/gi;
        //    return parseFloat(CompensationValue.value.replace(exp, '0'));
        //}

        function fnRevokeValidators() {
            if (confirm('Do you want to Revoke?')) {
                return true;
            }
            else {
                return false;
            }
        }


        //}

        // code added by C.Aswinkrishna Start on 26-Feb-2016//

        function fnidtrent(id)
        {
           
            var amount = 0;
            var amount1 = 0;
            var txtbox = document.getElementById('<%= txtInterimRent.ClientID %>');
            var grid = document.getElementById('<%= grvRSDet.ClientID %>');
      
                var controls = grid.getElementsByTagName("input");
                for (var j = 0; j < controls.length; j++) {

                    
                    if (controls[j].name.indexOf('lblInterimRentGrid') > -1)
                    {
                        
                        if (controls[j].value == '')
                            controls[j].value = 0;
                        amount = Number(controls[j].value);
                        amount1=Number(amount1) + Number(amount);
                        document.getElementById('<%=lbltotinterim.ClientID%>').innerHTML = amount1;
                        if (amount1 == 0) {
                            txtbox.readOnly = false;
                            document.getElementById('<%=hdnscript1.ClientID%>').value = "false";
                        }
                        else {
                            txtbox.readOnly = true;
                            document.getElementById('<%=hdnscript1.ClientID%>').value = "true";
                        }
                        
                       }
                    }
               
        }

        
        function fnidtrentAMF(id) {
           
            var amount = 0;
            var amount1 = 0;
            var txtbox = document.getElementById('<%= txtInterAMF.ClientID %>');
            var grid = document.getElementById('<%= grvRSDet.ClientID %>');

            var controls = grid.getElementsByTagName("input");
            for (var j = 0; j < controls.length; j++) {


                if (controls[j].name.indexOf('lblInterimAMFGrid') > -1) {

                    if (controls[j].value == '')
                        controls[j].value = 0;
                    amount = Number(controls[j].value);
                    amount1 = Number(amount1) + Number(amount);
                    document.getElementById('<%=lblTotAmf.ClientID%>').innerHTML = amount1;
                    if (amount1 == 0) {
                        txtbox.readOnly = false;
                        document.getElementById('<%=hdnscript.ClientID%>').value = "false";
                    }
                    else {

                     txtbox.readOnly = true;
                     document.getElementById('<%=hdnscript.ClientID%>').value = "true";
                 }

                    }
                }

            }
        

        function fneidtrent1(id) {
            var grid = document.getElementById('<%= grvRSDet.ClientID %>');


            var amount = id.value;
            if (amount == '')
                amount = id.value = 0;
            var txtbox = document.getElementById('<%= txtInterAMF.ClientID %>');
            var element = document.getElementById('<%=lblTotAmf.ClientID%>').innerHTML;
            for (var i = 0; i < grid.rows.length; i++) {
                var controls = grid.getElementsByTagName("input");
                for (var j = 0; j < controls.length; j++) {
                    if (controls[j].name.indexOf('lblInterimAMFGrid') > -1) {
                        //txtbox.setAttribute('readonly', 'readonly');
                        if (controls[j].value > 0) {

                            txtbox.readOnly = true;
                            if (amount == 0)
                            { document.getElementById('<%=hdnTotAmf.ClientID%>').value = Number(controls[j].value == '' ? 0 : controls[j].value); }
                            else
                            {
                                document.getElementById('<%=hdnTotAmf.ClientID%>').value = Number(document.getElementById('<%=hdnTotAmf.ClientID%>').value) + Number(amount);
                            }
                            document.getElementById('<%=hdnscript.ClientID%>').value = "true";
                            document.getElementById('<%=lblTotAmf.ClientID%>').innerHTML = Number(document.getElementById('<%=hdnTotAmf.ClientID%>').value);
                            return;
                        }

                        else {

                            txtbox.readOnly = false;
                            document.getElementById('<%=hdnscript.ClientID%>').value = "false";

                            document.getElementById('<%=lblTotAmf.ClientID%>').innerHTML = amount;
                            controls[j].value = document.getElementById('<%=hdnTotAmf.ClientID%>').value = 0;

                        }

                    }
                }
            }
            }


            // code added by C.Aswinkrishna End on 26-Feb-2016//

            //function checkRSActivationDate(sender, args) {
            //    var varApplicationDate = document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtAccountCreationDate').value;
            //    var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            //    var selectedDate = sender._selectedDate;
            //    var vartoday = new Date();
            //    var vartodayformat = vartoday.format(sender._format);
            //    var intValid = 0;
            //    if (selectedDate < varapplndate) {
            //        alert('RS Activation Date should be greater than or equal to RS Creation Date');
            //        document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartodayformat;
            //        document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartoday.format(sender._format);
            //    }
            //    else if (selectedDate > vartoday)
            //    {
            //        alert('RS Activation Date should be less than or equal to Current Date');
            //        document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartodayformat;
            //        document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = vartoday.format(sender._format);
            //    }
            //    else 
            //    {
            //        document.getElementById('ctl00_ContentPlaceHolder1_tcAccountActivation_tpMainPage_txtActivationDate').value = selectedDate.format(sender._format);
            //        intValid = 1;
            //    }
            //}

            //function fnSetDivWidth() {
            //    if (document.getElementById('divMenu').style.display == 'none') {
            //        document.getElementById("ctl00_ContentPlaceHolder1_tcAccountActivation_tpSystemJournal_divGrid1").style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            //    }
            //    else {
            //        document.getElementById("ctl00_ContentPlaceHolder1_tcAccountActivation_tpSystemJournal_divGrid1").style.width = screen.width - 260;
            //    }
            //}

    </script>
    <asp:UpdatePanel ID="upAddress" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="RS Bulk Activation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel Width="99%" ID="Panel1" runat="server" GroupingText="Account Information"
                            CssClass="stylePanel">
                            <table width="100%" cellspacing="0">
                                <tr>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblLessee" CssClass="styleReqFieldLabel" runat="server" Text="Lessee"></asp:Label>
                                        <asp:HiddenField ID="hdnscript" runat="server" />
                                        <asp:HiddenField ID="hdnscript1" runat="server" />
                                        <asp:HiddenField ID="hdnactidate" runat="server" />
                                        <asp:HiddenField ID="hdnfromdate" runat="server" />
                                        <asp:HiddenField ID="hdnTodate" runat="server" />
                                        <asp:HiddenField ID="hdnTotinterim" Value="0" runat="server" />
                                        <asp:HiddenField ID="hdnTotAmf" Value="0" runat="server" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLessee" runat="server" ServiceMethod="GetCustomer" AutoPostBack="true"
                                            ErrorMessage="Select a Lessee" IsMandatory="true" ValidationGroup="btnSave"
                                            OnItem_Selected="ddlLessee_SelectedIndexChanged" Width="280px" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblTranche" runat="server" Text="Tranche"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlTranche" runat="server" AutoPostBack="true" ToolTip="Tranche Number" OnSelectedIndexChanged="ddlTranche_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInterimMethod" runat="server" CssClass="styleReqFieldLabel" Text="Interim Method"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlInterimMethod" runat="server" AutoPostBack="true" Width="160px" OnSelectedIndexChanged="ddlInterim_SelectedIndexChanged" ToolTip="Interim Method">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvInterimMethod" runat="server" ControlToValidate="ddlInterimMethod" CssClass="styleMandatoryLabel" Display="None"
                                            ErrorMessage="Select Interim Method" InitialValue="0" SetFocusOnError="True" ValidationGroup="btnSave">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInterimRate" runat="server" CssClass="styleReqFieldLabel" Text="Interim Rate"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInterimRate" runat="server" Style="text-align: right;" MaxLength="3" ToolTip="Interim Rate"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvInterimRate" runat="server" ControlToValidate="txtInterimRate" CssClass="styleMandatoryLabel" Display="None" Enabled="false" ErrorMessage="Enter Interim Rate" SetFocusOnError="True" ValidationGroup="btnSave" Visible="false">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftbeInterimRate" runat="server" TargetControlID="txtInterimRate" Enabled="true"
                                            FilterType="Custom,Numbers" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInterimRent" runat="server" Text="Interim Rental Amount"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInterimRent" runat="server" AutoPostBack="true"
                                            OnTextChanged="txtInterimRent_TextChanged" Style="text-align: right;" ReadOnly="true"
                                            MaxLength="15" ToolTip="Interim Amount"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvInterimRent" runat="server" ControlToValidate="txtInterimRent"
                                            Display="None" Enabled="false" ErrorMessage="Enter Interim Amount" SetFocusOnError="True"
                                            ValidationGroup="btnSave">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="ftbeInterimRent" runat="server" TargetControlID="txtInterimRent" Enabled="true"
                                            FilterType="Custom,Numbers" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblActivationDate" runat="server" CssClass="styleReqFieldLabel" Text="Activation Date"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtActivationDate" runat="server" AutoPostBack="true"
                                            OnTextChanged="txtActivationDate_TextChanged" ToolTip="Activation Date"></asp:TextBox>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Activation Date" Visible="false" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="MM/dd/yyyy"
                                            PopupButtonID="imgDateofActivation" TargetControlID="txtActivationDate" OnClientDateSelectionChanged="funcheckdate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvActivationDate" runat="server" ControlToValidate="txtActivationDate"
                                            CssClass="styleMandatoryLabel" Display="None"
                                            Enabled="true" ErrorMessage="Enter Activation Date" SetFocusOnError="true" ValidationGroup="btnSave">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr style="width: 100%">
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblInterimAMF" runat="server" Text="Interim AMF"></asp:Label>
                                        <asp:TextBox ID="txtInterAMF" runat="server" Style="text-align: right; width: 85px" ReadOnly="true"
                                            OnTextChanged="txtInterAMF_TextChanged" AutoPostBack="true" MaxLength="15" ToolTip="Interim AMF"> </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvAMF" runat="server" ControlToValidate="txtInterAMF"
                                            CssClass="styleMandatoryLabel" Display="None" Enabled="false"
                                            ErrorMessage="Enter Interim AMF" SetFocusOnError="True" ValidationGroup="btnSave">
                                        </asp:RequiredFieldValidator>
                                        <cc1:FilteredTextBoxExtender ID="filAMF" runat="server" TargetControlID="txtInterAMF" Enabled="true"
                                            FilterType="Custom,Numbers" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 37%">

                                        <asp:Label ID="lblInterFrom" runat="server" Text="Interim Period From"></asp:Label>

                                        <asp:TextBox ID="txtInterFrom" runat="server" Style="text-align: right; width: 100px" ReadOnly="true" MaxLength="15" ToolTip="Interim Period From"> </asp:TextBox>
                                        <asp:Image ID="imgInterFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="clndrFrom" runat="server" Enabled="false"
                                            PopupButtonID="imgInterFrom" OnClientDateSelectionChanged="funchkdate1"
                                            TargetControlID="txtInterFrom">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvInterimfrom" runat="server" ControlToValidate="txtInterFrom" CssClass="styleMandatoryLabel" Display="None" Enabled="false"
                                            ErrorMessage="Enter Interim From" SetFocusOnError="True" ValidationGroup="btnSave">
                                        </asp:RequiredFieldValidator>

                                    </td>
                                    <td class="styleFieldAlign" style="width: 37%">

                                        <asp:Label ID="lblInterTo" runat="server" Text="Interim Period To"></asp:Label>

                                        <asp:TextBox ID="txtInterTo" runat="server" Style="text-align: right; width: 100px" ReadOnly="true" MaxLength="15" ToolTip="     Period To"> </asp:TextBox>
                                        <asp:Image ID="imgInterTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="clndrTo" runat="server" Enabled="false"
                                            PopupButtonID="imgInterTo" OnClientDateSelectionChanged="funchkdate2"
                                            TargetControlID="txtInterTo">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvInterTo" runat="server" ControlToValidate="txtInterTo" CssClass="styleMandatoryLabel" Display="None" Enabled="false"
                                            ErrorMessage="Enter Interim To" SetFocusOnError="True" ValidationGroup="btnSave">
                                        </asp:RequiredFieldValidator>

                                    </td>
                                    <%--<td class="styleFieldLabel">
                                        
                                    </td>
                                    <td class="styleFieldAlign">
                                       
                                    </td>--%>
                                </tr>
                                <%--<tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStatus" runat="server" CssClass="styleReqFieldLabel" Text="Status"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStatus" runat="server" ToolTip="Status"></asp:TextBox>
                                    </td>
                                </tr>--%>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel Width="99%" ID="Panel5" runat="server" GroupingText="RS Information"
                            CssClass="stylePanel">
                            <asp:GridView ID="grvRSDet" runat="server" Width="99%" AutoGenerateColumns="False" OnRowDataBound="grvRSDet_RowDataBound" HeaderStyle-CssClass="styleGridHeader">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl.No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSlNo" Text='<%#Eval("RowNumber")%>' runat="server" ToolTip="SlNo"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RS No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRSNo" Text='<%#Eval("PANum")%>' runat="server" ToolTip="RS No."></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RS Creation Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" Text='<%#Eval("Creation_Date")%>' runat="server" ToolTip="RS Creation Date"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sch Amt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSchAmt" Text='<%#Eval("Finance_Amount")%>' runat="server" ToolTip="Sch Amt"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" Text='<%#Eval("Status")%>' runat="server" ToolTip="Sch Amt"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interim Rent" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblInterimRentGrid" ReadOnly="false" Text='<%#Bind("Interim_amt") %>' Style="text-align: right" onchange="fnidtrent(this);" runat="server" ToolTip="Interim Rent"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filInterimRent" runat="server" TargetControlID="lblInterimRentGrid" Enabled="true"
                                                FilterType="Custom,Numbers" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interim Rent" ItemStyle-HorizontalAlign="Right" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInterimRentGridavg" Visible="false" Text='<%#Bind("Avg_Interim_rent") %>' runat="server"></asp:Label>
                                            
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interim AMF" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="lblInterimAMFGrid" ReadOnly="false" runat="server" Style="text-align: right" Text='<%#Bind("Interim_AMF") %>' onchange="fnidtrentAMF(this);" ToolTip="Interim AMF"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filInterimAMF" runat="server" TargetControlID="lblInterimAMFGrid" Enabled="true"
                                                FilterType="Custom,Numbers" ValidChars=".">
                                            </cc1:FilteredTextBoxExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblinterimAMFtotal" runat="server" Style="text-align: right" ToolTip="Interim AMF"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Interim AMF" ItemStyle-HorizontalAlign="Right" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInterimAMFGridavg" Visible="false" runat="server" Style="text-align: right" Text='<%#Bind("Avg_AMF") %>' onchange="fnidtrentAMF(this);" ToolTip="Interim AMF"></asp:Label>
                                            
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblinterimAMFtotal" runat="server" Style="text-align: right" ToolTip="Interim AMF"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="View">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="hyplnkViewPre" CommandArgument='<%# Bind("Account_Creation_ID") %>'
                                                ToolTip="View RS" OnClick="anchAcct_serverclick" ImageUrl="~/Images/spacer.gif"
                                                CssClass="styleGridQuery" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <HeaderTemplate>
                                            <span id="spnAll" style="text-align: center">Select All</span>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged"></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Bind("chk") %>' AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged"></asp:CheckBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <table width="100%" style="text-align:right">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblInterim_tot" runat="server"  Text="Total Interim Rent: " Font-Bold="true"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="lbltotinterim" runat="server" Text="0"  Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblInterim_totAMF" runat="server" style="text-align:right" Text="Total Interim AMF: " Font-Bold="true"></asp:Label>
                                        &nbsp;
                                        <asp:Label ID="lblTotAmf" runat="server" Text="0"  style="text-align:right" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>


                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators('btnSave');"
                            ValidationGroup="btnSave" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            ToolTip="Activate" Text="Activate" />
                        <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" CausesValidation="False"
                            ToolTip="Clear" Text="Clear" OnClientClick="confirm('Do you want to Clear?');"
                            meta:resourcekey="btnClearResource1" OnClick="btnClear_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                            ToolTip="Cancel" CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        <asp:Button runat="server" ID="btnExportAmort" Text="Export Amort RS" CausesValidation="False"
                            ToolTip="Export Amort RS" CssClass="styleSubmitButton" OnClick="btnExportAmort_Click" />
                        <asp:Button runat="server" ID="btnExportAmortTranche" Text="Export Amort Tranche" CausesValidation="False"
                            ToolTip="Export Amort Tranche" CssClass="styleSubmitButton" OnClick="btnExportAmortTranche_Click" />
                        <asp:Button runat="server" ID="btnExportJV" Text="Export JV" CausesValidation="False"
                            ToolTip="Export JV" CssClass="styleSubmitButton" OnClick="btnExportJV_Click" />
                        <asp:Button runat="server" ID="btnCashflow" Text="Print CahFlow" CausesValidation="False"
                            ToolTip="Print" CssClass="styleSubmitButton" OnClick="btnCashflow_Click" />
                        <asp:Button runat="server" ID="btnRevoke" Text="Revoke" CausesValidation="False" OnClientClick="return fnRevokeValidators();"
                            ToolTip="Revoke" CssClass="styleSubmitButton" OnClick="btnRevoke_Click" />
                        <asp:CustomValidator ID="custRouterLogic" runat="server" ValidationGroup="btnSave"
                            OnServerValidate="custRouterLogic_ServerValidate" Display="None">
                        </asp:CustomValidator>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportAmort" />
            <asp:PostBackTrigger ControlID="btnExportAmortTranche" />
            <asp:PostBackTrigger ControlID="btnExportJV" />
            <%--            <asp:PostBackTrigger ControlID="txtInterimRent" />--%>
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">
        function funcheckdate(sender, args) {
            document.getElementById('<%=hdnactidate.ClientID%>').value = sender._selectedDate;

            if (sender._selectedDate < new Date(document.getElementById('<%=hdnfromdate.ClientID%>').value)) {
                document.getElementById('<%=txtActivationDate.ClientID%>').value = "";
                alert('Interim From should not exceed RS Activation Date');
                return;
            }

        }

        function funchkdate1(sender, args) {
            document.getElementById('<%=hdnfromdate.ClientID%>').value = sender._selectedDate;

            if (sender._selectedDate > new Date()) {
                alert('Interim From should not exceed System Date');
                document.getElementById('<%=txtInterFrom.ClientID%>').value = "";
                return;
            }
        }

        function funchkdate2(sender, args) {
            document.getElementById('<%=hdnTodate.ClientID%>').value = sender._selectedDate;

            if (sender._selectedDate < new Date(document.getElementById('<%=hdnfromdate.ClientID%>').value)) {
                alert('Interim To Date cannot be before Interim From Date');
                document.getElementById('<%=txtInterTo.ClientID%>').value = "";
                return;
            }
        }
    </script>

</asp:Content>
