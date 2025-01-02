<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GLoanAdAccountCreation.aspx.cs" Inherits="S3GLoanAdAccountCreation"
    EnableEventValidation="false" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/S3GAutoSuggest.ascx" TagName="AutoSugg" TagPrefix="UC3" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigatorSummary" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc5" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GGetRsNo.ascx" TagName="RSNO" TagPrefix="UCRS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        var querymode;
        querymode = location.search.split("qsMode=");
        querymode = querymode[1];
        var tab;
        function pageLoad() {

            tab = $find('ctl00_ContentPlaceHolder1_tcAccountCreation');
            querymode = location.search.split("qsMode=");
            querymode = querymode[1];

            if (querymode != 'Q') {

                tab.add_activeTabChanged(on_Change);
                var newindex = tab.get_activeTabIndex(index);
                var btnSave = document.getElementById('<%=btnSave.ClientID %>');
                var btnclear = document.getElementById('<%=btnClear.ClientID %>');
                if (newindex == tab._tabs.length - 1)
                    btnSave.disabled = false;
                else

                    btnSave.disabled = true;

            }
        }

        function checkDate_ApplicationDate(sender, args) {
            var varApplicationDate = document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabOfferTerms_txtOfferDate').value;
            var varMRADate = document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtMRADate').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var varmracomparedate = Date.parseInvariant(varMRADate, sender._format);
            var selectedDate = sender._selectedDate;
            var vartoday = new Date();
            var vartodayformat = vartoday.format(sender._format);
            var intValid = 0;
            var strcomapre = '';
            if (varmracomparedate > varapplndate) {
                strcomapre = varmracomparedate;
            }
            else {
                strcomapre = varapplndate;
            }
            if (selectedDate < strcomapre) {
                alert('Rental Schedule Date should be greater than or equal to Proposal/MRA Date');
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value = vartodayformat;
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value = vartoday.format(sender._format);
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value = '';
            }

            else {
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value = selectedDate.format(sender._format);
                intValid = 1;
            }
            //if (vartoday < selectedDate) {
            //    alert('Account Date should be less than or equal to Current Date');
            //    document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value = vartodayformat;
            //    document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value = vartoday.format(sender._format);
            //}
            //else {
            //    if (intValid == 1) {
            //        document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value = selectedDate.format(sender._format);
            //    }
            //}

        }

        function checkRSCommenceDate(sender, args) {
            var varApplicationDate = document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabOfferTerms_txtOfferDate').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var selectedDate = sender._selectedDate;
            var vartoday = new Date();
            var vartodayformat = vartoday.format(sender._format);
            var intValid = 0;
            if (selectedDate < varapplndate) {
                alert('RS commencement Date should be greater than or equal to Proposal Date');
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_TxtCommenceDate').value = vartodayformat;
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_TxtCommenceDate').value = vartoday.format(sender._format);
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_TxtCommenceDate').value = selectedDate.format(sender._format);
                intValid = 1;
            }
        }

        function FunRestIrr() {

            //document.getElementById('<%=txtCompanyIRR_Repay.ClientID %>').value = "";
            document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value = "";

        }
        function FunDateRestIrr() {
            var varAccountDate = document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtAccountDate').value;
            var vartoday = new Date();
            if (vartoday != varAccountDate) {
                document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value = "";
            }
        }

        function checkApplicationDate(sender, args) {
            var varApplicationDate = document.getElementById('<%=txtAccountDate.ClientID %>').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var selectedDate = sender._selectedDate;
            var today = new Date();
            var varOfferDate = document.getElementById('<%=txtAccountDate.ClientID %>');
            if (varOfferDate != null) {
                var varOfferDateValue = document.getElementById('<%=txtAccountDate.ClientID %>').value;
                if (varOfferDateValue != "") {
                    var varOffdate = Date.parseInvariant(varOfferDateValue, sender._format);
                    if (selectedDate < varOffdate) {
                        alert('InflowDate should be greater than or equal to Rental schedule Date');
                        sender._textbox.set_Value(today.format(sender._format));
                        return;
                    }
                    else {
                        sender._textbox.set_Value(selectedDate.format(sender._format));

                    }
                }
                else {
                    if (selectedDate < varapplndate) {
                        alert('InflowDate should be greater than or equal to Rental schedule Date');
                        sender._textbox.set_Value(today.format(sender._format));
                        return;
                    }
                    else {
                        sender._textbox.set_Value(selectedDate.format(sender._format));
                    }
                }
            }

        }

        function checkOutflowApplicationDate(sender, args) {
            var varApplicationDate = document.getElementById('<%=txtAccountDate.ClientID %>').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var selectedDate = sender._selectedDate;
            var today = new Date();
            var varOfferDate = document.getElementById('<%=txtAccountDate.ClientID %>');
            if (varOfferDate != null) {
                var varOfferDateValue = document.getElementById('<%=txtAccountDate.ClientID %>').value;
                if (varOfferDateValue != "") {
                    var varOffdate = Date.parseInvariant(varOfferDateValue, sender._format);
                    if (selectedDate < varOffdate) {
                        alert('OutflowDate should be greater than or equal to Rental schedule Date');
                        sender._textbox.set_Value(today.format(sender._format));
                        return;
                    }
                    else {
                        sender._textbox.set_Value(selectedDate.format(sender._format));

                    }
                }
                else {
                    if (selectedDate < varapplndate) {
                        alert('OutflowDate should be greater than or equal to Rental schedule Date');
                        sender._textbox.set_Value(today.format(sender._format));
                    }
                    else {
                        sender._textbox.set_Value(selectedDate.format(sender._format));
                    }
                }
            }

        }

        function checkFirstInstallDate(sender, args) {

            var varApplicationDate = document.getElementById('<%=txtAccountDate.ClientID %>').value;
            var varFirstDueDate = document.getElementById('<%=TxtFirstInstallDue.ClientID %>').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var varDuedate = Date.parseInvariant(varFirstDueDate, sender._format);
            var selectedDate = sender._selectedDate;
            var today = new Date();

            if (varapplndate != null) {
                if (selectedDate < varapplndate) {
                    alert('First Installment Start Date should be greater than or equal to Rental schedule Date');
                    sender._textbox.set_Value(varapplndate.format(sender._format));
                    return;
                }
                else {
                    sender._textbox.set_Value(selectedDate.format(sender._format));
                }
            }
            if (varDuedate != null) {
                if (selectedDate > varDuedate) {
                    alert('First Installment Start Date should be Lesser than or equal to First Rental Due Date');
                    sender._textbox.set_Value(varapplndate.format(sender._format));
                    return;
                }
                else {
                    sender._textbox.set_Value(selectedDate.format(sender._format));
                }
            }

        }


        function checkFirstInstallDueDate(sender, args) {
            var varApplicationDate = document.getElementById('<%=txtfirstinstdate.ClientID %>').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var selectedDate = sender._selectedDate;
            var today = new Date();

            if (varapplndate != null) {
                if (selectedDate < varapplndate) {
                    alert('First Installment Due Date should be greater than or equal to First Installment Start Date');
                    sender._textbox.set_Value(varapplndate.format(sender._format));
                    return;
                }
                else {
                    sender._textbox.set_Value(selectedDate.format(sender._format));
                }
            }

        }

        function checkSignOnDate(sender, args) {
            var varApplicationDate = document.getElementById('<%=txtAccountDate.ClientID %>').value;
            var varapplndate = Date.parseInvariant(varApplicationDate, sender._format);
            var selectedDate = sender._selectedDate;
            var today = new Date();

            if (varapplndate != null) {
                if (selectedDate < varapplndate) {
                    alert('Rental Sign on Date should be greater than or equal to Rental schedule Date');
                    sender._textbox.set_Value(varapplndate.format(sender._format));
                }
                else {
                    sender._textbox.set_Value(selectedDate.format(sender._format));
                }
            }

        }

        var index = 0;
        function on_Change(sender, e) {

            var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var Valgrp = document.getElementById('<%=vs_TabMainPage.ClientID %>')
            Valgrp.validationGroup = strValgrp;

            var newindex = tab.get_activeTabIndex(index);
            var txtStatus = document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_txtStatus');
            var btnSave = document.getElementById('<%=btnSave.ClientID %>')
            var btnclear = document.getElementById('<%=btnClear.ClientID %>')
            if (newindex == tab._tabs.length - 1)
                btnSave.disabled = false;
            else
                btnSave.disabled = true;
            var status = txtStatus.value;
            var queryConsolidate;
            queryConsolidate = location.search.split("qsAccConNo=");
            var querySplit;
            querySplit = location.search.split("qsAccSplitNo=");
            if (querymode != "C" && querymode != "W") {
                if (queryConsolidate.length == 1 && querySplit.length == 1) {
                    if (status.substr(0, 1) == "7" || status.substr(0, 2) == "11" || status.substr(0, 2) == "13" || status.substr(0, 1) == "0" || status.substr(0, 1) == "6") {
                        btnSave.disabled = true;
                    }
                }
            }
            if (newindex > index) {
                if (!fnCheckPageValidators(strValgrp, false)) {
                    tab.set_activeTabIndex(index);

                }
                else {
                    var lobcode = FunGetSelectedLob();
                    var IsAssetAvail;
                    var IsCheckAssetAvail;
                    switch (lobcode.toLowerCase()) {
                        case "te": //Term loan Extensible
                        case "tl": //Term loan
                        case "ft": //Factoring 
                        case "wc": //Working Capital
                            IsAssetAvail = "No";
                            break;
                        default:
                            IsAssetAvail = "Yes";
                            break;
                    }
                    switch (lobcode.toLowerCase()) {
                        case "te": //Term loan Extensible
                        case "tl": //Term loan
                        case "ft": //Factoring 
                        case "wc": //Working Capital
                        case "ln": //Working Capital
                            IsCheckAssetAvail = false;
                            break;
                        default:
                            IsCheckAssetAvail = true;
                            break;
                    }

                    switch (index) {
                        case 0:
                            index = tab.get_activeTabIndex(index);
                            var t;
                            if (document.getElementById('<%=hdnGST.ClientID %>').value == "1")
                            {
                                if (confirm('Delivery State and Billing State are different.\n\nDo you want to proceed?'))
                                {
                                     t = "0";
                                }
                                else
                                {
                                    var sel = document.getElementById('<%=ddlBillingState.ClientID %>');
                                    sel.selectedIndex = 0;
                                    tab.set_activeTabIndex(0);
                                    return;
                                }
                            }
                            else
                            {
                                 t = "0";
                            }
                            
                            break;
                        case 1:

                            if (FunCheckGridIsEmpty('<%=gvOutFlow.ClientID %>', 'Yes')) {
                                if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                    document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                }
                                index = tab.get_activeTabIndex(index);

                            }
                            else {

                                if ((querymode != "C" && status.substr(0, 2) == "11") || status.substr(0, 2) == "13" || status.substr(0, 2) == "3 " || status.substr(0, 1) == "0" || status.substr(0, 1) == "7" || status.substr(0, 1) == "6") {

                                    index = tab.get_activeTabIndex(index);

                                }
                                else {
                                    if (lobcode.toLowerCase() != "ol") {
                                        tab.set_activeTabIndex(index);
                                        alert('Add atleast one Outflow details');
                                    }
                                    else {
                                        if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                            document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                        }
                                        index = tab.get_activeTabIndex(index);
                                    }

                                }
                            }

                            break;
                        case 2:

                            if (FunCheckGridIsEmpty('<%=gvRepaymentSummary.ClientID %>', 'No')) {
                                if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                    document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                }
                                index = tab.get_activeTabIndex(index);
                            }
                            else {
                                tab.set_activeTabIndex(index);
                                alert('Add atleast one Repayment details');
                            }


                            break;
                        case 4:
                            index = tab.get_activeTabIndex(index);
                            break;
                        case 5:
                            index = tab.get_activeTabIndex(index);
                            break;
                        default:
                            index = tab.get_activeTabIndex(index);
                            break;

                    }


                }
            }
            else {
                tab.set_activeTabIndex(newindex);
                index = tab.get_activeTabIndex(newindex);





            }

        }
        function FunGetSelectedLob() {
            var ddlLob = document.getElementById('<%=ddlLOB.ClientID %>');
            return ddlLob.item(ddlLob.selectedIndex).text.split('-')[0].trim();
        }
        function FunCheckGridIsEmpty(gridview, isfooterexists) {
            if (document.getElementById(gridview) == null) {
                return false;
            }
            var table = document.getElementById(gridview);
            var rows = table.getElementsByTagName("tr");
            if (isfooterexists == 'No') {
                if (rows.length > 1) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                if (rows.length > 2) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }



        function funCheckFirstLetterisNumeric(textbox, msg) {

            var FieldValue = new Array();
            FieldValue = textbox.value.trim();
            if (FieldValue.length > 0 && FieldValue.value != '') {
                if (isNaN(FieldValue[0])) {
                    return true;
                }
                else {
                    alert(msg + ' name cannot begin with a number');
                    textbox.focus();
                    textbox.value = '';
                    event.returnValue = false;
                    return false;
                }
            }
        }

        function fnLoadCustomer() {

            var VartxtCustomerCode = document.getElementById('<%=txtCustomerCode.ClientID %>').value;
            if (VartxtCustomerCode != null) {
                //if (document.getElementById('ctl00_ContentPlaceHolder1_tcAccountCreation_TabMainPage_ucCustomerCodeLov_hdnID').value.trim() != "") {


                if (document.getElementById('<%=ucCustomerCodeLov.ClientID %>' + '_hdnID').value.trim() != "") {
                    document.getElementById('<%=btnLoadCustomer.ClientID %>').click();
                }
            }
        }

        function fnLoadRsNo() {
            if (document.getElementById('<%=UcCtlRsno.ClientID %>' + '_hdnID').value.trim() != "") {
                document.getElementById('<%=btnLoadRS.ClientID %>').click();
            }

        }

    </script>
    <script type="text/javascript">
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=divInvoice.ClientID %>').style.width = parseInt(screen.width) - 80;
                document.getElementById('<%=divInvoice.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=divInvoice.ClientID %>').style.width = parseInt(screen.width) - 295;
                document.getElementById('<%=divInvoice.ClientID %>').style.overflow = "scroll";
            }
        }

    </script>
    <%-- <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>--%>
    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <table class="stylePageHeading" cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Rental Schedule Creation">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" width="100%">
                <table cellpadding="0" cellspacing="0" border="0" width="99%">
                    <tr>
                        <td valign="top" width="100%">
                            <cc1:TabContainer ID="tcAccountCreation" runat="server" CssClass="styleTabPanel"
                                Width="100%" ScrollBars="None" ActiveTabIndex="0">
                                <cc1:TabPanel runat="server" ID="TabMainPage" CssClass="tabpan" BackColor="Red" HeaderText="TabMainPage"
                                    Width="100%">
                                    <HeaderTemplate>
                                        Main Page
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <table width="100%" cellpadding="0">
                                                    <tr>
                                                        <td width="52%" valign="top">
                                                            <asp:Panel ID="Panel_Input" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                                                                Width="99%">
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%" cellpadding="0" border="0">
                                                                    <tr style="width: 100%">
                                                                        <td class="styleFieldLabel" style="width: 35%">
                                                                            <asp:Label runat="server" ID="lblLOB" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 65%">
                                                                            <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                                                                ErrorMessage="Select a Line of Business" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td width="35%">
                                                                            <%--  style="width: 65%" style="width: 35%"<asp:Button ID="btnCopyRS" runat="server" Text="Copy RS" CausesValidation="false" OnClick="btnCopyRS_OnClick" UseSubmitBehavior="False"/>--%> 
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <UCRS:RSNO ID="UcCtlRsno" runat="server" strLOV_Code="RSCUST" />
                                                                            <asp:Button ID="btnLoadRS" runat="server"
                                                                                Style="display: none" Text="Load RSNo" OnClick="btnLoadRS_OnClick" />
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 35%">
                                                                            <asp:Label runat="server" ID="lblBranch" Text="Schedule State" CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 65%">
                                                                            <UC3:AutoSugg ID="ddlBranchList" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                ErrorMessage="Select a Schedule State" IsMandatory="true" ValidationGroup="Main Page" OnItem_Selected="ddlBranchList_SelectedIndexChanged" />

                                                                        </td>
                                                                    </tr>


                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 35%">
                                                                            <asp:Label runat="server" ID="lblApplicationReferenceNo" Text="Proposal Pricing Number"
                                                                                CssClass="styleReqFieldLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 65%">
                                                                            <UC3:AutoSugg ID="ddlApplicationReferenceNo" runat="server" ServiceMethod="GetProposalNo" AutoPostBack="true" Width="250px"
                                                                                ErrorMessage="Select a Proposal Pricing Number" IsMandatory="true" ValidationGroup="Main Page" OnItem_Selected="ddlApplicationReferenceNo_SelectedIndexChanged" />

                                                                            <%--<asp:DropDownList ID="ddlApplicationReferenceNo" runat="server" AutoPostBack="True"
                                                                                OnSelectedIndexChanged="ddlApplicationReferenceNo_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvApplicationReferenceNo" runat="server" ControlToValidate="ddlApplicationReferenceNo"
                                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ErrorMessage="Select Proposal Pricing Number" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                            <asp:RequiredFieldValidator ID="rfvApplicationwithoutlob" runat="server" ControlToValidate="ddlApplicationReferenceNo"
                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Select Proposal Pricing Number"
                                                                                ValidationGroup="Main Page"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAccountDate" CssClass="styleReqFieldLabel" Text="Rental Schedule Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAccountDate" ContentEditable="false" onchange="FunDateRestIrr();" runat="server" Width="100px"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtAccountDate" runat="server" ValidChars="/-"
                                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                                                TargetControlID="txtAccountDate">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:Image ID="imgAccountdate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                            <cc1:CalendarExtender runat="server" TargetControlID="txtAccountDate" PopupButtonID="imgAccountdate"
                                                                                Format="DD/MM/YYYY" ID="CalendarAccountDate" Enabled="True"
                                                                                OnClientDateSelectionChanged="checkDate_ApplicationDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvAccountDate" runat="server" ControlToValidate="txtAccountDate"
                                                                                Display="None" ValidationGroup="Main Page" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Rental Schedule Date"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblFirstinstallment" CssClass="styleReqFieldLabel" Text="First Rental Start Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtfirstinstdate" runat="server" Width="100px" ContentEditable="false" onchange="FunRestIrr();"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtInstStartDate" runat="server" ValidChars="/-"
                                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                                                TargetControlID="txtfirstinstdate">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:Image ID="Imgfirstinst" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                            <cc1:CalendarExtender runat="server" TargetControlID="txtfirstinstdate" PopupButtonID="Imgfirstinst"
                                                                                Format="DD/MM/YYYY" ID="CalendarFirstinstdate" Enabled="True"
                                                                                OnClientDateSelectionChanged="checkFirstInstallDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="rfvFirstinstdate" runat="server" ControlToValidate="txtfirstinstdate"
                                                                                Display="None" ValidationGroup="Main Page" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                                                                ErrorMessage="Enter First Installment Start Date"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="Label1" CssClass="styleReqFieldLabel" Text="First Rental Due Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="TxtFirstInstallDue" runat="server" ContentEditable="false" Width="100px" onchange="FunRestIrr();"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtInstDueDate" runat="server" ValidChars="/-"
                                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                                                TargetControlID="TxtFirstInstallDue">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:Image ID="imgFrstDue" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                            <cc1:CalendarExtender runat="server" TargetControlID="TxtFirstInstallDue" PopupButtonID="imgFrstDue"
                                                                                Format="DD/MM/YYYY" ID="CalendarFirstinstDuedate" Enabled="True" OnClientDateSelectionChanged="checkFirstInstallDueDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtFirstInstallDue"
                                                                                Display="None" ValidationGroup="Main Page" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                                                                ErrorMessage="Enter First Rental Due Date"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="Label3" CssClass="styleReqFieldLabel" Text="RS commencement Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="TxtCommenceDate" runat="server" ContentEditable="false" Width="100px" onchange="FunRestIrr();"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtCommencementDate" runat="server" ValidChars="/-"
                                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                                                TargetControlID="TxtCommenceDate">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:Image ID="imgCommenceDue" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                            <cc1:CalendarExtender runat="server" TargetControlID="TxtCommenceDate" PopupButtonID="imgCommenceDue"
                                                                                Format="DD/MM/YYYY" ID="CalendarCommencedate" Enabled="True" OnClientDateSelectionChanged="checkRSCommenceDate">
                                                                            </cc1:CalendarExtender>
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtCommenceDate"
                                                                                Display="None" ValidationGroup="Main Page" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                                                                ErrorMessage="Enter RS commencement Date"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="Label4" CssClass="styleReqFieldLabel" Text="Rental Sign on Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="TxtRSSignDate" runat="server" Width="100px" ContentEditable="false" onchange="FunRestIrr();"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="ftxtRSSignDate" runat="server" ValidChars="/-"
                                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                                                TargetControlID="TxtRSSignDate">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:Image ID="imgSignDue" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                            <cc1:CalendarExtender runat="server" TargetControlID="TxtRSSignDate" PopupButtonID="imgSignDue"
                                                                                Format="DD/MM/YYYY" ID="CalendarSigndate" Enabled="True" OnClientDateSelectionChanged="checkSignOnDate">
                                                                            </cc1:CalendarExtender>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="48%" valign="top" style="padding-right: 2px">
                                                            <asp:Panel ID="Panel3" runat="server" GroupingText="Customer details" CssClass="stylePanel"
                                                                Width="99%">
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%; display: none" border="0">
                                                                    <tr>

                                                                        <td width="7%"></td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCustomerCode" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                        <td width="15%"></td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAddress1" CssClass="styleDisplayLabel" Text="Address1"></asp:Label>
                                                                        </td>
                                                                        <td></td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAddress1" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                        <td></td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAddress2" CssClass="styleDisplayLabel" Text="Address2"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAddress2" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblCity" CssClass="styleDisplayLabel" Text="City"></asp:Label>
                                                                        </td>
                                                                        <td></td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCity" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                        <td></td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblState" CssClass="styleDisplayLabel" Text="State"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtState" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblCountry" CssClass="styleDisplayLabel" Text="Country"></asp:Label>
                                                                        </td>
                                                                        <td></td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCountry" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                        <td></td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblPincode" CssClass="styleDisplayLabel" Text="Pincode/ZipCode"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtPincode" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" cellspacing="0">
                                                                    <tr>

                                                                        <td width="36%" class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblCustomerCode" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                                        </td>

                                                                        <td width="70%">
                                                                            <uc2:LOV ID="ucCustomerCodeLov" runat="server" strLOV_Code="CMD" />
                                                                            <asp:Button ID="btnLoadCustomer" runat="server"
                                                                                Style="display: none" Text="Load Customer" OnClick="btnLoadCustomer_OnClick" />

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" width="100%">
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" ShowCustomerCode="false"
                                                                                FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Panel ID="pnlApplicationDetails" runat="server" GroupingText="Rental Schedule Details"
                                                                CssClass="stylePanel" Width="99%">
                                                                <table width="100%" border="0" cellspacing="0" border="1">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 25%">
                                                                            <asp:Label runat="server" ID="lblPrimeAccountNo" CssClass="styleDisplayLabel" Text="Rental Schedule No"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtPrimeAccountNo" runat="server" Width="95%"></asp:TextBox>
                                                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                                FilterType="Custom, Numbers, UppercaseLetters, lowercaseLetters" ValidChars="-()" Enabled="True"
                                                                                TargetControlID="txtPrimeAccountNo">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td class="styleFieldLabel" style="width: 25%">
                                                                            <asp:Label runat="server" ID="lblStatus" CssClass="styleDisplayLabel" Text="Status"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" Visible="False">
                                                                            </asp:DropDownList>
                                                                            <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True" Width="95%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 25%">
                                                                            <asp:Label runat="server" ID="lblSubAccountNo" CssClass="styleDisplayLabel" Text="MRA No"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtMRANo" runat="server" ReadOnly="True" Width="95%"></asp:TextBox>
                                                                        </td>
                                                                        <td class="styleFieldLabel" style="width: 25%">
                                                                            <asp:Label runat="server" ID="lblApplicationDate" CssClass="styleDisplayLabel" Text="MRA Effective Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtMRADate" runat="server" ReadOnly="True" Width="45%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 25%">
                                                                            <asp:Label runat="server" ID="lblDeliveryState" CssClass="styleReqFieldLabel" Text="Delivery State"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:DropDownList ID="ddlDeliveryState" runat="server" Enabled="false" AutoPostBack="true" onchange="FunRestIrr();" OnSelectedIndexChanged="ddlDeliveryState_SelectedIndexChange">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvDeliveryState" runat="server" ControlToValidate="ddlDeliveryState"
                                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ErrorMessage="Select an Delivery State" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td class="styleFieldLabel" style="width: 25%">
                                                                            <asp:Label runat="server" ID="lblBillingState" CssClass="styleReqFieldLabel" Text="Billing State"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:DropDownList ID="ddlBillingState" runat="server" Enabled="false" AutoPostBack="true" onchange="FunRestIrr();" OnSelectedIndexChanged="ddlBillingState_SelectedIndexChange">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvBillingState" runat="server" ControlToValidate="ddlBillingState"
                                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ErrorMessage="Select an Billing State" ValidationGroup="Main Page"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 25%">
                                                                            <asp:Label runat="server" ID="lblProductCode" Visible="False" CssClass="styleDisplayLabel" Text="Product Code"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtProductCode" runat="server" Visible="False" ReadOnly="True" Width="95%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="70%">
                                                            <asp:Panel ID="Panel_Finance" runat="server" GroupingText="Finance details" CssClass="stylePanel"
                                                                Width="98%" Style="padding-left: 7px; padding-right: 2px">
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 20%;">
                                                                            <asp:Label runat="server" ID="lblFinanceAmount" CssClass="styleDisplayLabel" Text="Rental Schedule Amount"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 30%;">
                                                                            <asp:TextBox ID="txtFinanceAmount" runat="server" ReadOnly="true" ValidationGroup="Main Page" MaxLength="10"
                                                                                onkeypress="fnAllowNumbersOnly(false,false,this)" onkeyup="fnNotAllowPasteSpecialChar(this,'special')"
                                                                                Width="100px"></asp:TextBox>
                                                                            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFinanceAmount"
                                                                                ValidationGroup="Main Page" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ErrorMessage="Enter Finance Amount"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                        <td class="styleFieldLabel" style="width: 20%;">
                                                                            <asp:Label runat="server" ID="lblTenure" CssClass="styleDisplayLabel" Text="Tenure"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 30%;">
                                                                            <asp:TextBox ID="txtTenure" runat="server" MaxLength="3" ReadOnly="True" Style="text-align: right;"
                                                                                Width="50px" onchange="FunRestIrr();"></asp:TextBox>
                                                                            <%-- <asp:RequiredFieldValidator ID="rfvTenure" runat="server" ControlToValidate="txtTenure"
                                                                                ValidationGroup="Main Page" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                                ErrorMessage="Enter the Tenure"></asp:RequiredFieldValidator>--%>
                                                                            <asp:HiddenField runat="server" ID="hdnRentfrequency" />
                                                                            <asp:HiddenField runat="server" ID="hdnTimeValue" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblTenureType" CssClass="styleReqFieldLabel" Text="Tenure Type"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlTenureType" runat="server" onchange="FunRestIrr();">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvTenureType" runat="server" ControlToValidate="ddlTenureType"
                                                                                ValidationGroup="Main Page" CssClass="styleMandatoryLabel" Display="None" InitialValue="-1"
                                                                                SetFocusOnError="True" ErrorMessage="Select the Tenure Type"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblAccountManager1" runat="server" CssClass="styleDisplayLabel" Text="Account Manager 1"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <UC3:AutoSugg ID="ddlAccountManager1" runat="server" ServiceMethod="GetSalePersonList" />
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblAccountManager2" runat="server" CssClass="styleDisplayLabel" Text="Account Manager 2"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <UC3:AutoSugg ID="ddlAccountManager2" runat="server" ServiceMethod="GetSalePersonList" />
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblSalePersonCode" runat="server" CssClass="styleDisplayLabel" Text="Regional Manager"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <UC3:AutoSugg ID="ddlRegionalManager" runat="server" ServiceMethod="GetSalePersonList" />
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="30%">
                                                            <asp:Panel ID="Panel8" runat="server" GroupingText="IRR details" CssClass="stylePanel"
                                                                Width="98%" Style="padding-left: 7px; padding-right: 2px">
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAccountingIRR" Visible="False" CssClass="styleDisplayLabel" Text="Accounting IRR"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAccountingIRR" runat="server" Visible="False" Width="75px" Style="text-align: right;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblBusinessIRR" CssClass="styleDisplayLabel" Text="Business IRR"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtBusinessIRR" runat="server" Style="text-align: right;" Width="75px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblCompanyIRR" CssClass="styleDisplayLabel" Text="Company IRR"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCompanyIRR" runat="server" Width="75px" Style="text-align: right;"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table width="99%" border="0">
                                                    <tr>
                                                        <td width="100%" class="styleContentTable">
                                                            <asp:Panel ID="pnlDocDtls" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                                <div style="float: left;">
                                                                    Document details
                                                                </div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblDetailsDocDtls" runat="server">(Show Details...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="imgDetailsDocDtls" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                        AlternateText="(Show Details...)" />
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlDocDtlsInfo" CssClass="stylePanel" runat="server" Width="100%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblConstitutionCodeList" runat="server" CssClass="styleDisplayLabel"
                                                                                Text="Constitution Code"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtConstitutionCode" runat="server" CssClass="styleTExtBox" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <asp:Panel runat="server" ID="Panel6" CssClass="stylePanel" GroupingText="Document Details"
                                                                                Width="99%" Enabled="false">
                                                                                <asp:GridView runat="server" ID="grvConsDocuments" Width="100%" OnRowDataBound="grvConsDocs_RowDataBound"
                                                                                    AutoGenerateColumns="False" EnableModelValidation="True">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                                        <asp:BoundField DataField="DocumentFlag" HeaderText="Document Flag" />
                                                                                        <asp:BoundField DataField="DocumentDescription" HeaderText="Document Description">
                                                                                            <ItemStyle Width="15%" />
                                                                                        </asp:BoundField>
                                                                                        <asp:TemplateField HeaderText="Is_Mandatory">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblOptman" runat="server" Text="Optional / Mandatory"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <%--'<%# Bind("IsMandatory") %>'--%>
                                                                                                <asp:CheckBox ID="chkIsMandatory" runat="server" Enabled="false" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IsMandatory")))%>'></asp:CheckBox>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="10%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Image Copy">
                                                                                            <ItemTemplate>
                                                                                                <%-- '<%# Bind("IsNeedImageCopy") %>'--%>
                                                                                                <asp:CheckBox ID="chkIsNeedImageCopy" Enabled="false" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "IsNeedImageCopy")))%>'></asp:CheckBox>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="10%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Collected">
                                                                                            <ItemTemplate>
                                                                                                <%--'<%# Bind("Collected") %>'--%>
                                                                                                <asp:CheckBox ID="chkCollected" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Collected")))%>'></asp:CheckBox>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="10%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Scanned">
                                                                                            <ItemTemplate>
                                                                                                <%--'<%# Bind("Scanned") %>'--%>
                                                                                                <asp:CheckBox ID="chkScanned" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Scanned")))%>'></asp:CheckBox>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="10%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Remark">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtRemark" runat="server" onkeypress="fnCheckSpecialChars(true)"
                                                                                                    Width="130px" onkeyup="maxlengthfortxt(60)" Text='<%# Bind("Remark") %>' MaxLength="60"
                                                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="7%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Value">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtValues" runat="server" onkeypress="fnCheckSpecialChars(true)"
                                                                                                    Width="130px" onkeyup="fnNotAllowPasteSpecialChar(this,'special')" Text='<%# Bind("Values") %>'
                                                                                                    MaxLength="40"></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="15%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="FollowUp">
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox ID="chkIs_FollowUp" runat="server"></asp:CheckBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Scanned Reference" Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkScannedReference" runat="server" Text="View">
                                                                                                </asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                                    <RowStyle HorizontalAlign="Center" />
                                                                                </asp:GridView>
                                                                                <br />
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <cc1:CollapsiblePanelExtender ID="cpeDocDtls" runat="server" TargetControlID="pnlDocDtlsInfo"
                                                                ExpandControlID="pnlDocDtls" CollapseControlID="pnlDocDtls" Collapsed="True"
                                                                TextLabelID="lblDetailsDocDtls" ImageControlID="imgDetailsDocDtls" ExpandedText="(Hide Details...)"
                                                                CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="True" SkinID="CollapsiblePanelDemo" Enabled="True" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div id="Div16" style="overflow: auto; text-align: center">
                                                    <asp:Button ID="btnConfigure" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                        OnClick="btnConfigure_Click" UseSubmitBehavior="False" Text="Configure" Visible="false" />
                                                    <asp:Label runat="server" ID="lblReport_Format" CssClass="styleDisplayLabel" Text="Report Format"></asp:Label>
                                                    <asp:CheckBox ID="chkSecondary" runat="server" Text="Secondary" CssClass="styleDisplayLabel" />
                                                    <asp:DropDownList ID="ddlReportType" runat="server" />
                                                    <asp:Button ID="btnPrint" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                        ToolTip="Print"
                                                        UseSubmitBehavior="False" Text="Print" OnClick="btnPrint_click" Enabled="False" />
                                                    <input type="hidden" runat="server" id="hdnGST" />
                                                    
                                                </div>
                                                <br />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnLoadRS" />
                                            </Triggers>

                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabOfferTerms" HeaderText="TabOfferTerms" CssClass="tabpan"
                                    BackColor="Red" Width="98%">
                                    <HeaderTemplate>
                                        Offer Terms
                                    </HeaderTemplate>
                                    <ContentTemplate>

                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table width="99%" border="0">
                                                    <tr>
                                                        <td width="100%" class="styleContentTable">
                                                            <asp:Panel ID="divRoiRules" runat="server" CssClass="accordionHeader" Width="98.5%" Visible="false">
                                                                <div style="float: left;">
                                                                    ROI/Payment Rule Card details
                                                                </div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblDetails" runat="server">(Show Details...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="imgDetails" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                        AlternateText="(Show Details...)" />
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="divROIRuleInfo" CssClass="stylePanel" runat="server" Width="100%" Visible="false">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td width="50%"></td>
                                                                        <td width="50%">
                                                                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                                                <tr>
                                                                                    <td class="styleFieldLabel" width="20%">
                                                                                        <asp:Label ID="lblPaymentRuleList" runat="server" CssClass="styleReqFieldLabel" Text="Payment Rule"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign" width="30%">
                                                                                        <asp:DropDownList ID="ddlPaymentRuleList" runat="server" OnSelectedIndexChanged="ddlPaymentRuleList_SelectedIndexChanged"
                                                                                            AutoPostBack="True">
                                                                                        </asp:DropDownList>
                                                                                        <asp:RequiredFieldValidator ID="rfvddlPaymentRuleList" runat="server" ControlToValidate="ddlPaymentRuleList"
                                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                                                            ValidationGroup="Offer Terms" ErrorMessage="Select a Payment Rule"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td width="100%" valign="top"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" width="50%">
                                                                            <asp:Panel runat="server" ID="Panel4" CssClass="stylePanel" GroupingText="Payment Rule"
                                                                                Width="99%">
                                                                                <asp:GridView ID="gvPaymentRuleDetails" runat="server" Width="100%">
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <%-- <cc1:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="divROIRuleInfo"
                                                                ExpandControlID="divRoiRules" CollapseControlID="divRoiRules" Collapsed="True"
                                                                TextLabelID="lblDetails" ImageControlID="imgDetails" ExpandedText="(Hide Details...)"
                                                                CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />--%>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td width="100%" class="styleContentTable">
                                                            <asp:Panel ID="pnlProposals" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                                <div style="float: left;">
                                                                    Proposal details
                                                                </div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblDetailsProDtl" runat="server">(Show Details...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="imgDetailsProDtl" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                        AlternateText="(Show Details...)" />
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlProposalDtlInfo" CssClass="stylePanel" runat="server" Width="100%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblProposalType" runat="server" Text="Proposal Type" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlProposalType" Enabled="false" runat="server" ValidationGroup="Pricing"></asp:DropDownList>

                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblProposalNumber" runat="server" Text="Proposal Number"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtProposalNumber" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>


                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblLeadRefNo" runat="server" Text="Lead Ref. No."></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtLeadRefNo" runat="server" ReadOnly="true"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblOfferDate" runat="server" Text="Proposal Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtOfferDate" runat="server" ReadOnly="true" Width="100px"></asp:TextBox>

                                                                        </td>

                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblOfferValidTill" runat="server" Text="Offer valid till" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtOfferValidTill" runat="server" ReadOnly="true" Width="100px" ValidationGroup="Main Page"></asp:TextBox>

                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblSecuritydeposit" runat="server" Text="Security deposit Appl." CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlSecuritydeposit" Enabled="false" runat="server">
                                                                            </asp:DropDownList>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblReturnPattern" runat="server" Text="Return Pattern" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlReturnPattern" runat="server" Enabled="false" ValidationGroup="Pricing"></asp:DropDownList>

                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblTotalFacilityAmount" runat="server" Text="Total Facility Amount" CssClass="styleDisplayLabel"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtTotalFacilityAmount" ReadOnly="true" onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="12" runat="server" Style="text-align: right"></asp:TextBox>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <%-- <asp:Label ID="lblStructuredEquated" runat="server" Text="Structured Equated"></asp:Label><asp:DropDownList ID="ddlStructuredEquated" runat="server" ValidationGroup="Pricing"></asp:DropDownList>--%>
                                                                            <asp:RadioButtonList RepeatDirection="Horizontal" Enabled="false" runat="server" CssClass="styleDisplayLabel" ID="RBLStructuredEI">
                                                                                <asp:ListItem Selected="True" Value="1" Text="Equated"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="Structured"></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>



                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="6">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblSecDepAdvRent" runat="server" Text="Sec Dep/Adv Rent %"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblOneTimeFee" runat="server" Text="One Time Fee"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblProcessingFee" runat="server" Text="Processing Fee"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblSecondaryTerm" runat="server" Text="Secondary Term"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="AdvanceRent" runat="server" Text="Advance Rent Appl."></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblVATRebate" runat="server" Text="VAT Rebate" CssClass="styleDisplayLabel"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldAlign" width="10%">
                                                                                        <asp:TextBox ID="txtSecDepAdvRent" ReadOnly="true" runat="server" MaxLength="2" Style="text-align: right"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtOneTimeFee" ReadOnly="true" runat="server" MaxLength="12" Style="text-align: right"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtProcessingFee" ReadOnly="true" runat="server" MaxLength="12" Style="text-align: right"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <%--<asp:DropDownList ID="ddlSecondaryTerm" runat="server" ValidationGroup="Pricing"></asp:DropDownList>--%>
                                                                                        <asp:RadioButtonList RepeatDirection="Horizontal" Enabled="false" CssClass="styleDisplayLabel" runat="server" ID="RBLSecondaryTerm">
                                                                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                                            <asp:ListItem Selected="True" Value="0" Text="No"></asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <%--<asp:DropDownList ID="ddlAdvanceRent" runat="server" ValidationGroup="Pricing"></asp:DropDownList>--%>
                                                                                        <asp:RadioButtonList RepeatDirection="Horizontal" Enabled="false" CssClass="styleDisplayLabel" runat="server" ID="RBLAdvanceRent">
                                                                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                                            <asp:ListItem Selected="True" Value="0" Text="No"></asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:RadioButtonList RepeatDirection="Horizontal" Enabled="false" CssClass="styleDisplayLabel" runat="server" ID="RBLVATRebate">
                                                                                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                                                                            <asp:ListItem Selected="True" Value="0" Text="No"></asp:ListItem>
                                                                                        </asp:RadioButtonList>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>

                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td colspan="6">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblOneTimeRate" runat="server" Text="One Time Rate"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblProcessingfeeRate" runat="server" Text="Processing Fee Rate"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblOneTimeBasedOn" runat="server" Text="One Time Based On"></asp:Label>
                                                                                    </td>
                                                                                    <td class="styleFieldLabel">
                                                                                        <asp:Label ID="lblPAmtBasedOn" runat="server" Text="Processing Fee Based On"></asp:Label>
                                                                                    </td>

                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="styleFieldAlign" width="10%">
                                                                                        <asp:TextBox ID="txtOneTimeRate" ReadOnly="true" runat="server" Style="text-align: right"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txtProcessingfeeRate" ReadOnly="true" runat="server" MaxLength="12" Style="text-align: right"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddlAmtBasedOn" runat="server" Enabled="false">
                                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                            <asp:ListItem Text="Total Invoice Value" Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="Rental Schedule Value" Value="2"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td class="styleFieldAlign">
                                                                                        <%--<asp:DropDownList ID="ddlSecondaryTerm" runat="server" ValidationGroup="Pricing"></asp:DropDownList>--%>
                                                                                        <asp:DropDownList ID="ddlProcessingBasedOn" runat="server" Enabled="false">
                                                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                                                            <asp:ListItem Text="Total Invoice Value" Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="Rental Schedule Value" Value="2"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>

                                                                                </tr>
                                                                            </table>

                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblRemarks" runat="server" Text="Remarks"></asp:Label>
                                                                        </td>
                                                                        <td colspan="3" class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtRemarks" Width="95%" Rows="3" runat="server" ReadOnly="true" TextMode="MultiLine"
                                                                                MaxLength="1000" onkeyup="maxlengthfortxt(1000)"></asp:TextBox>
                                                                        </td>

                                                                        <%--  <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblIRR_Rest" runat="server" CssClass="styleDisplayLabel" Text="IRR Rest"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddl_IRR_Rest" runat="server">
                                                                            </asp:DropDownList>
                                                                        </td>--%>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblRoundNo" runat="server" Visible="false" Text="lblRoundNo"></asp:Label></td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtResidualValue_Cashflow" runat="server" Style="display: none;"></asp:TextBox>
                                                                            <asp:TextBox ID="txtResidualAmt_Cashflow" runat="server" Style="display: none;"></asp:TextBox>

                                                                        </td>

                                                                    </tr>

                                                                </table>
                                                            </asp:Panel>
                                                            <cc1:CollapsiblePanelExtender ID="cpeProposalDtl" runat="Server" TargetControlID="pnlProposalDtlInfo"
                                                                ExpandControlID="pnlProposals" CollapseControlID="pnlProposals" Collapsed="True"
                                                                TextLabelID="lblDetailsProDtl" ImageControlID="imgDetailsProDtl" ExpandedText="(Hide Details...)"
                                                                CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" class="styleContentTable">
                                                            <asp:Panel ID="pnlPriSec" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                                <div style="float: left;">
                                                                    Primary/Secondary details
                                                                </div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblDetailsPriSec" runat="server">(Show Details...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="imgDetailsPriSec" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                        AlternateText="(Show Details...)" />
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlPriSecinfo" CssClass="stylePanel" runat="server" Width="100%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:CheckBox ID="chkIsSecondary" runat="server" onclick="FunRestIrr();" AutoPostBack="true" Text="Separate Secondary Amort" ToolTip="Separate Secondary Amort" Enabled="false" OnCheckedChanged="chkIsSecondary_CheckedChanged"></asp:CheckBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="pnlPrimaryGrid" runat="server" CssClass="stylePanel" GroupingText="Primary Grid" Width="100%">
                                                                                <asp:GridView ID="grvPrimaryGrid" runat="server" OnRowDataBound="grvPrimaryGrid_OnRowDataBound"
                                                                                    AutoGenerateColumns="false" ShowFooter="true" Width="100%">
                                                                                    <%--OnRowDeleting="grvPrimaryGrid_OnRowDeleting" OnRowCommand="grvPrimaryGrid_OnRowCommand"--%>
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="3%" FooterStyle-Width="3%">
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox runat="server" ID="chkSelectPG" Checked="false" AutoPostBack="true" OnCheckedChanged="chkSelectPG_checkedChange" ToolTip="select" />
                                                                                                <asp:Label ID="lblSelected" Visible="false" runat="server" Text='<%# Bind("Selected") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--slno--%>
                                                                                        <asp:TemplateField HeaderText="sl.no." ItemStyle-Width="3%" FooterStyle-Width="3%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblslnoPG" runat="server" Text='<%#Container.DataItemIndex+1%>'>></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Asset Category--%>
                                                                                        <asp:TemplateField HeaderText="Asset Category" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAssetCategoryPG" runat="server" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                                                                                <asp:Label ID="lblAssetCategoryIDPG" Visible="false" runat="server" Text='<%# Bind("AssetCategory_ID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Tenure--%>
                                                                                        <asp:TemplateField HeaderText="Tenure" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblTenurePG" runat="server" Text='<%# Bind("Tenure") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Rent Frequency--%>
                                                                                        <asp:TemplateField HeaderText="Rent Frequency" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRentFrequencyPG" runat="server" Text='<%# Bind("RentFrequency") %>'></asp:Label>
                                                                                                <asp:Label ID="lblRentFrequencyIDPG" Visible="false" runat="server" Text='<%# Bind("RentFrequencyID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Rent Frequency--%>
                                                                                        <asp:TemplateField HeaderText="Adv/Arr" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAdvanceArrearPG" runat="server" Text='<%# Bind("AdvanceArrear") %>'></asp:Label>
                                                                                                <asp:Label ID="lblAdvanceArrearIDPG" Visible="false" runat="server" Text='<%# Bind("AdvanceArrearID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Facility Amount--%>
                                                                                        <asp:TemplateField HeaderText="Facility Amount" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFacilityAmountPG" runat="server" Text='<%# Bind("FacilityAmount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotFacilityAmountPG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--From Install No--%>
                                                                                        <asp:TemplateField HeaderText="From Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFromInstallNoPG" runat="server" Text='<%# Bind("FromInstallNo") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--To Install No--%>
                                                                                        <asp:TemplateField HeaderText="To Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblToInstallNoPG" runat="server" Text='<%# Bind("ToInstallNo") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Rent Rate--%>
                                                                                        <asp:TemplateField HeaderText="Rent Rate" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRentRatePG" runat="server" Text='<%# Bind("RentRate") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--AMF Rate--%>
                                                                                        <asp:TemplateField HeaderText="AMF Rent" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAMFRentPG" runat="server" Text='<%# Bind("AMFRent") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--START By siva  --%>
                                                                                        <asp:TemplateField HeaderText="Fixed Rent" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFixedRent" runat="server" ToolTip="Fixed Rent" Text='<%# Bind("FixedRent") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Fixed AMF" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFixedAMF" runat="server" ToolTip="Fixed AMF" Text='<%# Bind("FixedAMF") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>
                                                                                        <%--END By siva  --%>

                                                                                        <%--Residual Per%--%>
                                                                                        <asp:TemplateField HeaderText="Residual Per%" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <%--<asp:Label ID="lblResidualPerPG" runat="server" Text='<%# Bind("ResidualPer") %>'></asp:Label>--%>
                                                                                                <asp:TextBox ID="txtResidualPerPG" onchange="FunRestIrr();" Width="90%" AutoPostBack="true" OnTextChanged="txtResidualPerPG_textChanged" onkeypress="fnAllowNumbersOnly(true,false,this)" Style="text-align: right;" runat="server" Text='<%# Bind("ResidualPer") %>'></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Utilized Amount--%>
                                                                                        <asp:TemplateField HeaderText="Utilized Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblUtilizedAmountPG" runat="server" Text='<%# Bind("UtilizedAmount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotUtilizedAmountPG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Balance Amount--%>
                                                                                        <asp:TemplateField HeaderText="Balance Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblbalanceAmountPG" runat="server" Text='<%# Bind("balance_amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotbalanceAmountPG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Mapped Amount--%>
                                                                                        <asp:TemplateField HeaderText="RS Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblMappedAmountPG" runat="server" Text='<%# Bind("MappedAmount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotMappedAmountPG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Capitalization Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblCapitalizationamountPG" runat="server" Text='<%# Bind("Capitalised_Amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotCapitalizationamountPG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--RV Amount--%>
                                                                                        <asp:TemplateField HeaderText="RV Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRVamountPG" runat="server" Text='<%# Bind("RV_Amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotRVamountPG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="pnlSecondaryGrid" runat="server" CssClass="stylePanel" GroupingText="Secondary Grid" Width="100%">
                                                                                <asp:GridView ID="grvSecondaryGrid" runat="server" OnRowDataBound="grvSecondaryGrid_OnRowDataBound"
                                                                                    AutoGenerateColumns="false" ShowFooter="true" Width="100%">
                                                                                    <%--OnRowDeleting="grvSecondaryGrid_OnRowDeleting" OnRowCommand="grvSecondaryGrid_OnRowCommand"--%>
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="3%" FooterStyle-Width="3%">
                                                                                            <ItemTemplate>
                                                                                                <asp:CheckBox runat="server" Checked="false" AutoPostBack="true" OnCheckedChanged="chkSelectSG_checkedChange" ID="chkSelectSG" Enabled="false" ToolTip="select" />
                                                                                                <asp:Label ID="lblSelected" Visible="false" runat="server" Text='<%# Bind("Selected") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--slno--%>
                                                                                        <asp:TemplateField HeaderText="sl.no." ItemStyle-Width="3%" FooterStyle-Width="3%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblslnoSG" runat="server" Text='<%#Container.DataItemIndex+1%>'>></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Asset Category--%>
                                                                                        <asp:TemplateField HeaderText="Asset Category" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAssetCategorySG" runat="server" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                                                                                <asp:Label ID="lblAssetCategoryIDSG" Visible="false" runat="server" Text='<%# Bind("AssetCategory_ID") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>
                                                                                        <%--Tenure--%>
                                                                                        <asp:TemplateField HeaderText="Tenure" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblTenureSG" runat="server" Text='<%# Bind("Tenure") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>
                                                                                        <%--Rent Frequency--%>
                                                                                        <asp:TemplateField HeaderText="Rent Frequency" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRentFrequencySG" runat="server" Text='<%# Bind("RentFrequency") %>'></asp:Label>
                                                                                                <asp:Label ID="lblRentFrequencyIDSG" Visible="false" runat="server" Text='<%# Bind("RentFrequencyID") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>
                                                                                        <%--Rent Frequency--%>
                                                                                        <asp:TemplateField HeaderText="Adv/Arr" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAdvanceArrearSG" runat="server" Text='<%# Bind("AdvanceArrear") %>'></asp:Label>
                                                                                                <asp:Label ID="lblAdvanceArrearIDSG" Visible="false" runat="server" Text='<%# Bind("AdvanceArrearID") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>
                                                                                        <%--Facility Amount--%>
                                                                                        <asp:TemplateField HeaderText="Facility Amount" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFacilityAmountSG" runat="server" Text='<%# Bind("FacilityAmount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotFacilityAmountSG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--From Install No--%>
                                                                                        <asp:TemplateField HeaderText="From Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFromInstallNoSG" runat="server" Text='<%# Bind("FromInstallNo") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--To Install No--%>
                                                                                        <asp:TemplateField HeaderText="To Install No" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblToInstallNoSG" runat="server" Text='<%# Bind("ToInstallNo") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Rent Rate--%>
                                                                                        <asp:TemplateField HeaderText="Rent Rate" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRentRateSG" runat="server" Text='<%# Bind("RentRate") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--AMF Rate--%>
                                                                                        <asp:TemplateField HeaderText="AMF Rent" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAMFRentSG" runat="server" Text='<%# Bind("AMFRent") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--START By siva  --%>
                                                                                        <asp:TemplateField HeaderText="Fixed Rent" ItemStyle-Width="7%" FooterStyle-Width="7%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFixedRentSG" runat="server" ToolTip="Fixed Rent" Text='<%# Bind("FixedRent") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Fixed AMF" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblFixedAMFSG" runat="server" ToolTip="Fixed AMF" Text='<%# Bind("FixedAMF") %>'></asp:Label>
                                                                                            </ItemTemplate>

                                                                                        </asp:TemplateField>
                                                                                        <%--END By siva  --%>
                                                                                        <%--Residual Per%--%>
                                                                                        <asp:TemplateField HeaderText="Residual Per%" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <%--<asp:Label ID="lblResidualPerSG" runat="server" Text='<%# Bind("ResidualPer") %>'></asp:Label>--%>
                                                                                                <asp:TextBox ID="txtResidualPerSG" onchange="FunRestIrr();" AutoPostBack="true" OnTextChanged="txtResidualPerSG_textChanged" onkeypress="fnAllowNumbersOnly(true,false,this)" Style="text-align: right;" runat="server" Text='<%# Bind("ResidualPer") %>'></asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Utilized Amount--%>
                                                                                        <asp:TemplateField HeaderText="Utilized Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblUtilizedAmountSG" runat="server" Text='<%# Bind("UtilizedAmount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotUtilizedAmountSG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Balance Amount--%>
                                                                                        <asp:TemplateField HeaderText="Balance Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblbalanceAmountSG" runat="server" Text='<%# Bind("balance_amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotbalanceAmountSG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--Mapped Amount--%>
                                                                                        <asp:TemplateField HeaderText="RS Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblMappedAmountSG" runat="server" Text='<%# Bind("MappedAmount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotMappedAmountSG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Capitalization Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblCapitalizationamountSG" runat="server" Text='<%# Bind("Capitalised_Amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotCapitalizationamountSG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--RV Amount--%>
                                                                                        <asp:TemplateField HeaderText="RV Amount" ItemStyle-Width="6%" FooterStyle-Width="6%">
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblRVamountSG" runat="server" Text='<%# Bind("RV_Amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblTotRVamountSG" runat="server"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 50%">
                                                                            <asp:Panel ID="pnlSalesTax" GroupingText="Sales Tax" CssClass="stylePanel" runat="server" Width="100%">
                                                                                <table width="100%" border="0">
                                                                                    <tr>
                                                                                        <td class="styleFieldLabel">
                                                                                            <asp:Label ID="lblSalesTax" runat="server" Text="Sales Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td class="styleFieldAlign">
                                                                                            <asp:Label ID="ddlSalesTax" runat="server"></asp:Label>
                                                                                            <%--<asp:RequiredFieldValidator ID="rfvddlSalesTax" runat="server" ControlToValidate="ddlSalesTax"
                                                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                                                                                ValidationGroup="Offer Terms" ErrorMessage="Select a Sales Tax"></asp:RequiredFieldValidator>--%>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="chkCSTDeal" onclick="FunRestIrr();" runat="server" Text="CST Deal Option"
                                                                                                CssClass="styleDisplayLabel" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkCSTDeal_CheckedChanged" />
                                                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            <asp:CheckBox ID="chkcstwith" onclick="FunRestIrr();" runat="server" Text="CST(With C)"
                                                                                                CssClass="styleDisplayLabel" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkcstwith_CheckedChanged" />
                                                                                            <asp:TextBox ID="txtCFormNo" runat="server" Enabled="false"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="styleFieldLabel">
                                                                                            <asp:CheckBox ID="chkimport" runat="server" Text="Import" CssClass="styleDisplayLabel"
                                                                                                onclick="FunRestIrr();" AutoPostBack="true" OnCheckedChanged="chkimport_CheckedChanged" />
                                                                                            <asp:CheckBox ID="chkSEZ" runat="server" Text="SEZ" CssClass="styleDisplayLabel"
                                                                                                onclick="FunRestIrr();" AutoPostBack="true" OnCheckedChanged="chkSEZ_CheckedChanged" />
                                                                                            <asp:CheckBox ID="chkWithIGST" runat="server" Text="With IGST" CssClass="styleDisplayLabel"
                                                                                                onclick="FunRestIrr();" Enabled="false" />
                                                                                        </td>
                                                                                        <td class="styleFieldLabel">
                                                                                            <asp:Label ID="lblSEZZone" runat="server" Text="SEZ Zone" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                            <asp:DropDownList ID="ddlSEZZone" runat="server" onChange="FunRestIrr();" Enabled="false"></asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvddlSEZZone" runat="server" ControlToValidate="ddlSEZZone"
                                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                                                                ValidationGroup="Offer Terms" Enabled="false" ErrorMessage="Select a SEZ Zone"></asp:RequiredFieldValidator>

                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Label ID="Label2" runat="server" Text="Concessional ST" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                            <asp:CheckBox ID="chkSEZA1" runat="server" Text="A1" CssClass="styleDisplayLabel"
                                                                                                onclick="FunRestIrr();" Enabled="false" ToolTip="Concessional ST" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="styleFieldLabel">
                                                                                            <asp:CheckBox ID="chkAmfsold" runat="server" Text="AMF Sold" CssClass="styleDisplayLabel" />
                                                                                            <asp:CheckBox ID="chkVATSold" runat="server" Text="Rental Tax Sold" CssClass="styleDisplayLabel" />
                                                                                        </td>
                                                                                        <td class="styleFieldLabel">
                                                                                            <asp:CheckBox ID="chkServiceTaxSold" runat="server" Text="Service Tax Sold" CssClass="styleDisplayLabel" />

                                                                                        </td>

                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <cc1:CollapsiblePanelExtender ID="cpePriSec" runat="Server" TargetControlID="pnlPriSecinfo"
                                                                ExpandControlID="pnlPriSec" CollapseControlID="pnlPriSec" Collapsed="True"
                                                                TextLabelID="lblDetailsPriSec" ImageControlID="imgDetailsPriSec" ExpandedText="(Hide Details...)"
                                                                CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" class="styleContentTable">
                                                            <asp:Panel ID="pnlStandard" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                                <div style="float: left;">
                                                                    Standard Frequency Pattern
                                                                </div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblDetailsStandard" runat="server">(Show Details...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="imgDetailsStandard" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                        AlternateText="(Show Details...)" />
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlStandardInfo" CssClass="stylePanel" runat="server" Width="100%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox ID="chkApplicable" Enabled="false" runat="server" Text="Primary Applicable" onclick="FunRestIrr();" />
                                                                            <asp:CheckBox ID="chkApplicableSec" Enabled="false" runat="server" Text="Secondary Applicable" onclick="FunRestIrr();" />
                                                                            <asp:CheckBox ID="chkFullRental" runat="server" Text="Full Rental" onclick="FunRestIrr();" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" TargetControlID="pnlStandardInfo"
                                                                    ExpandControlID="pnlStandard" CollapseControlID="pnlStandard" Collapsed="True"
                                                                    TextLabelID="lblDetailsStandard" ImageControlID="imgDetailsStandard" ExpandedText="(Hide Details...)"
                                                                    CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                    CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" class="styleContentTable">
                                                            <asp:Panel ID="pnlInvoiceDtls" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                                <div style="float: left;">
                                                                    Invoice details
                                                                </div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblDetailsInvoiceDtl" runat="server">(Show Details...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="imgDetailsInvoiceDtls" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                        AlternateText="(Show Details...)" />
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlInvoiceDtlsinfo" CssClass="stylePanel" runat="server">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <%--<asp:Panel ID="pnlTaxDtlsinfo" CssClass="stylePanel" runat="server" Width="98%">--%>
                                                                            <table width="100%" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnlAddtionalTax" GroupingText="Additional Tax For RS Amount" CssClass="stylePanel" runat="server">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td class="styleDisplayLabel">
                                                                                                        <asp:CheckBox ID="chkLBT" Text="LBT" runat="server" onclick="FunRestIrr();" CssClass="styleDisplayLabel" AutoPostBack="true" OnCheckedChanged="chkLBT_CheckedChanged" />
                                                                                                        <%--AutoPostBack="true" OnCheckedChanged="chkAdditional_CheckedChanged"--%>
                                                                                                    </td>
                                                                                                    <td class="styleDisplayLabel" colspan="2">
                                                                                                        <asp:CheckBox ID="chkbxGSTRC" runat="server" Text="GST Reverse Charge" OnCheckedChanged="chkbxGSTRC_CheckedChanged"
                                                                                                            AutoPostBack="true" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="styleDisplayLabel">
                                                                                                        <asp:CheckBox ID="chkPT" Text="Purchase Tax" runat="server" onclick="FunRestIrr();" CssClass="styleDisplayLabel" AutoPostBack="true" OnCheckedChanged="chkLBT_CheckedChanged" />
                                                                                                    </td>
                                                                                                    <td class="styleDisplayLabel" colspan="2">
                                                                                                        <asp:CheckBox ID="chkITC_Cap" Text="ITC to be Removed for Capitalization" runat="server" onclick="FunRestIrr();" CssClass="styleDisplayLabel" AutoPostBack="true" OnCheckedChanged="chkLBT_CheckedChanged"/>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="styleDisplayLabel">
                                                                                                        <asp:CheckBox ID="chkET" Text="Entry Tax" runat="server" onclick="FunRestIrr();" CssClass="styleDisplayLabel" AutoPostBack="true" OnCheckedChanged="chkLBT_CheckedChanged" />
                                                                                                    </td>
                                                                                                    <td><asp:Label ID="lblRentalTDSSec" runat="server" Text="Rental TDS Section" CssClass="styleReqFieldLabel"></asp:Label></td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="ddlRentalTDSSec" runat="server" CssClass="styleDisplayLabel" />
                                                                                                        
                                                                                                    </td>

                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="styleDisplayLabel">
                                                                                                        <asp:CheckBox ID="chkRC" Text="Reverse Charge" runat="server" onclick="FunRestIrr();" CssClass="styleDisplayLabel" AutoPostBack="true" OnCheckedChanged="chkLBT_CheckedChanged" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <%--<td class="styleFieldLabel">
                                                                                                            <asp:Label ID="lblReverseChargeRate" runat="server" Text="Reverse Charge Rate" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                                        </td>
                                                                                                        <td class="styleFieldAlign">
                                                                                                            <asp:TextBox ID="txtReverseChargeRate" runat="server" Style="text-align: right"></asp:TextBox>
                                                                                                        </td>--%>
                                                                                                    <td class="styleDisplayLabel" colspan="2">
                                                                                                        <asp:CheckBox ID="chkITC" Text="ITC to be removed" runat="server" onclick="FunRestIrr();" CssClass="styleDisplayLabel" AutoPostBack="true" OnCheckedChanged="chkLBT_CheckedChanged" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                        <asp:Panel ID="Panel5" GroupingText="Lien Account Details" CssClass="stylePanel" runat="server">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td class="styleFieldLabel">
                                                                                                        <asp:Label ID="lblLienAccount" runat="server" CssClass="styleDisplayLabel" Text="Lien Account"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="styleFieldAlign" colspan="2">
                                                                                                        <UC3:AutoSugg ID="ddlLientContract" runat="server" Width="150px" ServiceMethod="GetLienAccount"
                                                                                                            IsMandatory="false" AutoPostBack="false" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                    <td valign="top" align="center">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Panel ID="PnlExtension" GroupingText="Extension/Rewrite" CssClass="stylePanel" runat="server">
                                                                                                        <asp:GridView ID="grvRentalDetails" runat="server" Enabled="false"
                                                                                                            AutoGenerateColumns="false" ShowFooter="true" OnRowDeleting="grvRentalDetails_OnRowDeleting" OnRowCommand="grvRentalDetails_OnRowCommand">
                                                                                                            <%--OnRowDeleting="grvPrimaryGrid_OnRowDeleting" OnRowCommand="grvPrimaryGrid_OnRowCommand" OnRowDeleting="grvSummaryInvoices_OnRowDeleting"--%>
                                                                                                            <Columns>
                                                                                                                <%--slno--%>
                                                                                                                <asp:TemplateField HeaderText="PASAREFID" Visible="false" ItemStyle-Width="3%" FooterStyle-Width="3%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblPASAREFID" runat="server" Text='<%# Bind("PA_SA_REF_ID") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>

                                                                                                                <%--Rental Schedule Number--%>
                                                                                                                <asp:TemplateField HeaderText="Rental Schedule Number" ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PANum") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <UC3:AutoSugg ID="ddlPANum" runat="server" ServiceMethod="GetPANumList" ErrorMessage="Select a Rental Schedule Number"
                                                                                                                            ValidationGroup="grvRentalDetails" IsMandatory="true" />
                                                                                                                    </FooterTemplate>
                                                                                                                </asp:TemplateField>

                                                                                                                <asp:TemplateField HeaderText="Tranch" ItemStyle-Width="15%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblTranch" runat="server" Text='<%# Bind("Tranche") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>

                                                                                                                <asp:TemplateField ItemStyle-Width="5%" FooterStyle-Width="5%">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Button ID="btnRemove" CssClass="styleGridShortButton" runat="server" CausesValidation="false" CommandName="Delete"
                                                                                                                            Text="Remove" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <FooterTemplate>
                                                                                                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true"
                                                                                                                            CommandName="Add" CssClass="styleGridShortButton" ValidationGroup="grvRentalDetails" />
                                                                                                                    </FooterTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                        <asp:Label ID="lblAddInvAmt" runat="server" Text="Reset Amount"></asp:Label>
                                                                                                        <asp:TextBox ID="txtAddInvAmt" runat="server" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                                                            onkeyup="fnNotAllowPasteSpecialChar(this,'special')" Width="100px" Enabled="false" onblur="FunRestIrr()">
                                                                                                        </asp:TextBox>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Panel ID="Panel7" GroupingText="Auto Extension" CssClass="stylePanel" runat="server">
                                                                                                        <asp:Label runat="server" ID="LblAEx" Text="Rental amount"></asp:Label>
                                                                                                        <asp:TextBox ID="TxtAutoExtnRental" runat="server" ValidationGroup="Main Page" MaxLength="10"
                                                                                                            onkeypress="fnAllowNumbersOnly(false,false,this)" onkeyup="fnNotAllowPasteSpecialChar(this,'special')"
                                                                                                            Width="100px">
                                                                                                        </asp:TextBox>
                                                                                                    </asp:Panel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Button ID="btnInvoices" runat="server" Text="Select For RS" ToolTip="Select For RS" class="styleSubmitButton" OnClick="btnInvoices_Click" />
                                                                                        <asp:Button ID="btnCalculateInvoices" Style="display: none" runat="server" Text="Calculate Invoices" ToolTip="Map Invoices" class="styleSubmitButton" OnClick="btnCalculateInvoices_Click" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <%--</asp:Panel>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="pnlSummaryInvoices" runat="server" CssClass="stylePanel" GroupingText="Invoice Details" Width="100%">
                                                                                <div id="divInvoice" style="overflow: scroll; height: auto;" runat="server">
                                                                                    <table width="98%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Repeater ID="grvSummaryInvoices" runat="server" OnItemCommand="grvPrimaryGrid_OnRowCommand">
                                                                                                    <HeaderTemplate>
                                                                                                        <table style="border: 1px solid #bad4ff;">
                                                                                                            <tr align="center" style="font-family: calibri; font-size: 13px; font-weight: bold; background-color: aliceblue;">

  																												<td>Invoice Id
                                                                                                                </td>                                                                                                                
																													<td>Asset Category
                                                                                                                </td>
                                                                                                                <td>Asset Type
                                                                                                                </td>
                                                                                                                <td>Asset Sub Type
                                                                                                                </td>
                                                                                                                <td>Vendor Branch State
                                                                                                                </td>
                                                                                                                <td>Vendor Name
                                                                                                                </td>
                                                                                                                <td>Invoice Type
                                                                                                                </td>
                                                                                                                <td>Invoice No
                                                                                                                </td>
                                                                                                                <td>Qty
                                                                                                                </td>
                                                                                                                <td>Invoice Amount
                                                                                                                </td>
                                                                                                                <td>LBT Amount
                                                                                                                </td>
                                                                                                                <td>Purchase Tax
                                                                                                                </td>
                                                                                                                <td>Entry Tax
                                                                                                                </td>
                                                                                                                <td>Reverse Charge
                                                                                                                </td>
                                                                                                                <td>Additional Tax for RS Amount
                                                                                                                </td>
                                                                                                                <td>Cr. Note
                                                                                                                </td>
                                                                                                                <td>Schedule Amount
                                                                                                                </td>
                                                                                                                <td>Invoice Date
                                                                                                                </td>
                                                                                                                <td>Invoice Material
                                                                                                                </td>
                                                                                                                <td>Labour Amount
                                                                                                                </td>
                                                                                                                <td>VAT
                                                                                                                </td>
                                                                                                                <td>CST
                                                                                                                </td>
                                                                                                                <td>HSN SGST	</td>
                                                                                                                <td>HSN CGST	</td>
                                                                                                                <td>HSN IGST	</td>
                                                                                                                <td>Cess Amount	</td>
                                                                                                                <td>Rebate Discount	</td>
                                                                                                                <td>SAC SGST	</td>
                                                                                                                <td>SAC CGST	</td>
                                                                                                                <td>SAC IGST	</td>
                                                                                                                <td>HSN SGST ITC Value	</td>
                                                                                                                <td>HSN CGST ITC Value	</td>
                                                                                                                <td>HSN IGST ITC Value	</td>
                                                                                                                <td>SAC SGST ITC Value	</td>
                                                                                                                <td>SAC CGST ITC Value	</td>
                                                                                                                <td>SAC IGST ITC Value	</td>
                                                                                                                <td>HSN SGST RC Value	</td>
                                                                                                                <td>HSN CGST RC Value	</td>
                                                                                                                <td>HSN IGST RC Value	</td>
                                                                                                                <td>SAC SGST RC Value	</td>
                                                                                                                <td>SAC CGST RC Value	</td>
                                                                                                                <td>SAC IGST RC Value	</td>
                                                                                                                <td>TCS Amount	</td>
                                                                                                                <td>Excise Duty CVD
                                                                                                                </td>
                                                                                                                <td>Service Tax
                                                                                                                </td>
                                                                                                                <td>Retention
                                                                                                                </td>
                                                                                                                <td>Others
                                                                                                                </td>
                                                                                                                <td>Total SCH Amt
                                                                                                                </td>
                                                                                                                <td>Base Amt VAT
                                                                                                                </td>
                                                                                                                <td>VAT ITC Value
                                                                                                                </td>
                                                                                                                <td>TDS
                                                                                                                </td>
                                                                                                                <td>WCT Amount
                                                                                                                </td>
                                                                                                                <td>CENVAT Amount
                                                                                                                </td>
                                                                                                                <td>Total Deduction
                                                                                                                </td>
                                                                                                                <td>Net Payable Amount
                                                                                                                </td>
                                                                                                                <td>Capitalised Amount
                                                                                                                </td>
                                                                                                                <td>Cur Capitalised Amount
                                                                                                                </td>
																												<%-- <td>User Name
                                                                                                                </td>
																												 <td>User ID
                                                                                                                </td>--%>
                                                                                                                <td>
                                                                                                                    <asp:LinkButton ID="lnRemoveAll" CausesValidation="false" runat="server" OnClick="btnRemoveAll_click"
                                                                                                                        Text="Remove All"></asp:LinkButton>
                                                                                                                    <cc1:ConfirmButtonExtender ID="lnRemoveAllConfirmButtonExtender" runat="server" ConfirmText="Do you want to remove All the Records?" Enabled="True" TargetControlID="lnRemoveAll">
                                                                                                                    </cc1:ConfirmButtonExtender>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                    </HeaderTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <tr style="font-family: calibri; font-size: 13px;">
 																											<td>
																										<asp:Label ID="lblInvoice_Id" runat="server" Text='<%# Bind("Invoice_Id") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label Visible="false" ID="lblPA_SA_REF_ID" runat="server" Text='<%# Bind("PA_SA_REF_ID") %>'></asp:Label>
                                                                                                                <asp:Label Visible="false" ID="lbltemp_Inv_Id" runat="server" Text='<%# Bind("temp_Inv_Id") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblAssetCategory" runat="server" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblAssetCategoryIDSG" Visible="false" runat="server" Text='<%# Bind("AssetCategory_ID") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblAssetType" runat="server" Text='<%# Bind("Asset_Type") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblAssetSubType" runat="server" Text='<%# Bind("Asset_Sub_Type") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblVendorStateId" Visible="false" runat="server" Text='<%# Bind("Vendor_State_Id") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblVendorState" runat="server" Text='<%# Bind("Vendor_State") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblVendor_Code" Visible="false" runat="server" Text='<%# Bind("Vendor_Code") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblVendor_Name" runat="server" Text='<%# Bind("Vendor_Name") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblInvoice_Type" runat="server" Text='<%# Bind("Invoice_Type") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblInvoiceId" runat="server" Text='<%# Bind("Invoice_Id") %>' Visible="false"></asp:Label>
                                                                                                                <asp:Label ID="lblInvoice_No" runat="server" Text='<%# Bind("Invoice_No") %>'></asp:Label>
                                                                                                            </td>

                                                                                                            <td>
                                                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                                                                            </td>

                                                                                                            <td>
                                                                                                                <asp:Label ID="lblInvoice_Amount" runat="server" Text='<%# Bind("Invoice_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblLBT_Amount" runat="server" Text='<%# Bind("LBT_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblPurchase_Tax" runat="server" Text='<%# Bind("Purchase_Tax") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblEntry_Tax_Amount" runat="server" Text='<%# Bind("Entry_Tax_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblReverse_Charge_Tax" runat="server" Text='<%# Bind("Reverse_Charge_Tax") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblAdditional_Amount" runat="server" Text='<%# Bind("Additional_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblCrNote" runat="server" Text='<%# Bind("Credit_Note_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSchedule_Amount" runat="server" Text='<%# Bind("Schedule_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblInvoice_Date" runat="server" Text='<%# Bind("Invoice_Date") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblBase_Inv_Amt_Mat" runat="server" Text='<%# Bind("Inv_Amt_Material") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblBase_Inv_Amt_lbr" runat="server" Text='<%# Bind("Inv_Amt_labour") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblVAT" runat="server" Text='<%# Bind("VAT") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblCST" runat="server" Text='<%# Bind("CST") %>'></asp:Label>
                                                                                                            </td>

                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNSGST" runat="server" Text='<%# Eval("HSNSGST") %>'
                                                                                                                    ToolTip="HSN SGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNCGST" runat="server" Text='<%# Eval("HSNCGST") %>'
                                                                                                                    ToolTip="HSN CGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNIGST" runat="server" Text='<%# Eval("HSNIGST") %>'
                                                                                                                    ToolTip="HSN IGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblCessAmount" runat="server" Text='<%# Eval("Cess_Amount") %>'
                                                                                                                    ToolTip="Cess Amount" /></td>

                                                                                                            <td>
                                                                                                                <asp:Label ID="lblRebateDiscount" runat="server" Text='<%# Eval("Rebate_Discount") %>'
                                                                                                                    ToolTip="Rebate Discount" /></td>
                                                                                                            
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACSGST" runat="server" Text='<%# Eval("SACSGST") %>'
                                                                                                                    ToolTip="SAC SGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACCGST" runat="server" Text='<%# Eval("SACCGST") %>'
                                                                                                                    ToolTip="SAC CGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACIGST" runat="server" Text='<%# Eval("SACIGST") %>'
                                                                                                                    ToolTip="SAC IGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNSGSTITC_Value" runat="server" Text='<%# Eval("HSNSGSTITC_Value") %>'
                                                                                                                    ToolTip="HSN SGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNCGSTITC_Value" runat="server" Text='<%# Eval("HSNCGSTITC_Value") %>'
                                                                                                                    ToolTip="HSN CGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNIGSTITC_Value" runat="server" Text='<%# Eval("HSNIGSTITC_Value") %>'
                                                                                                                    ToolTip="HSN IGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACSGSTITC_Value" runat="server" Text='<%# Eval("SACSGSTITC_Value") %>'
                                                                                                                    ToolTip="SAC SGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACCGSTITC_Value" runat="server" Text='<%# Eval("SACCGSTITC_Value") %>'
                                                                                                                    ToolTip="SAC CGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACIGSTITC_Value" runat="server" Text='<%# Eval("SACIGSTITC_Value") %>'
                                                                                                                    ToolTip="SAC IGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNSGSTRC_Value" runat="server" Text='<%# Eval("HSNSGSTRC_Value") %>'
                                                                                                                    ToolTip="HSN SGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNCGSTRC_Value" runat="server" Text='<%# Eval("HSNCGSTRC_Value") %>'
                                                                                                                    ToolTip="HSN CGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblHSNIGSTRC_Value" runat="server" Text='<%# Eval("HSNIGSTRC_Value") %>'
                                                                                                                    ToolTip="HSN IGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACSGSTRC_Value" runat="server" Text='<%# Eval("SACSGSTRC_Value") %>'
                                                                                                                    ToolTip="SAC SGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACCGSTRC_Value" runat="server" Text='<%# Eval("SACCGSTRC_Value") %>'
                                                                                                                    ToolTip="SAC CGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSACIGSTRC_Value" runat="server" Text='<%# Eval("SACIGSTRC_Value") %>'
                                                                                                                    ToolTip="SAC IGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTCSAmount" runat="server" Text='<%# Eval("TCS_Amount") %>'
                                                                                                                    ToolTip="TCS Amount" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblExcise_Duty_CVD" runat="server" Text='<%# Bind("Excise_Duty_CVD") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblService_Tax_Amt" runat="server" Text='<%# Bind("Service_Tax_Amt") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblRtntionAmount" runat="server" Text='<%# Bind("Retention_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblothers" runat="server" Text='<%# Bind("others") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotal_SCH_Amt" runat="server" Text='<%# Bind("Total_SCH_Amt") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td style="display: none;">
                                                                                                                <asp:Label ID="lblAsset_Amount" runat="server" Text='<%# Bind("Asset_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblBase_Amt_VAT" runat="server" Text='<%# Bind("Base_Amt_VAT") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblSmryITC_Value" runat="server" Text='<%# Bind("ITC_Value") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTDS" runat="server" Text='<%# Bind("TDS_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblWCT_Amount" runat="server" Text='<%# Bind("WCT_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblCENVAT_Amount" runat="server" Text='<%# Bind("CENVAT_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotal_Deduction" runat="server" Text='<%# Bind("Total_Deduction") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblNet_Payable_Amount" runat="server" Text='<%# Bind("Net_Payable_Amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblcapitalised_amount" runat="server" Text='<%# Bind("capitalised_amount") %>'></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblCur_Capitalised_Amount" runat="server" Text='<%# Bind("Cur_Capitalised_Amount") %>'></asp:Label>
                                                                                                            </td>
																											<%--<td>
                                                                                                                <asp:Label ID="lblUser_Name" runat="server" Text='<%# Bind("User_Name") %>'></asp:Label>
                                                                                                            </td>
																											<td>
                                                                                                                <asp:Label ID="lblUser_ID" runat="server" Text='<%# Bind("User_ID") %>'></asp:Label>
                                                                                                            </td>--%>
                                                                                                            <td>
                                                                                                                <asp:LinkButton ID="lnRemoveRepayment" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                                                    Text="Remove"></asp:LinkButton>
                                                                                                                <cc1:ConfirmButtonExtender ID="lnRemoveRepaymentConfirmButtonExtender" runat="server" ConfirmText="Do you want to remove the Record?" Enabled="True" TargetControlID="lnRemoveRepayment">
                                                                                                                </cc1:ConfirmButtonExtender>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </ItemTemplate>
                                                                                                    <FooterTemplate>
                                                                                                        <tr style="font-family: calibri; font-size: 13px; font-weight: bold; background-color: aliceblue;">
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
																											<td></td>

                                                                                                            <td><asp:Label ID="lblTot_Qty" runat="server"></asp:Label></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotInvoice_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotlblLBT_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotPurchase_Tax" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotlblEntry_Tax_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotlblReverse_Charge_Tax" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotAdditional_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotCrNote" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTotSchedule_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblBase_Inv_Amt_Mat" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblBase_Inv_Amt_lbr" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblVAT" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblCST" runat="server"></asp:Label>
                                                                                                            </td>

                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNSGST" runat="server"
                                                                                                                    ToolTip="HSN SGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNCGST" runat="server"
                                                                                                                    ToolTip="HSN CGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNIGST" runat="server"
                                                                                                                    ToolTip="HSN IGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFCessAmount" runat="server"
                                                                                                                    ToolTip="Cess Amount" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFRebateDiscount" runat="server"
                                                                                                                    ToolTip="Rebate Discount" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACSGST" runat="server"
                                                                                                                    ToolTip="SAC SGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACCGST" runat="server"
                                                                                                                    ToolTip="SAC CGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACIGST" runat="server"
                                                                                                                    ToolTip="SAC IGST" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNSGSTITC_Value" runat="server"
                                                                                                                    ToolTip="HSN SGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNCGSTITC_Value" runat="server"
                                                                                                                    ToolTip="HSN CGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNIGSTITC_Value" runat="server"
                                                                                                                    ToolTip="HSN IGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACSGSTITC_Value" runat="server"
                                                                                                                    ToolTip="SAC SGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACCGSTITC_Value" runat="server"
                                                                                                                    ToolTip="SAC CGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACIGSTITC_Value" runat="server"
                                                                                                                    ToolTip="SAC IGST ITC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNSGSTRC_Value" runat="server"
                                                                                                                    ToolTip="HSN SGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNCGSTRC_Value" runat="server"
                                                                                                                    ToolTip="HSN CGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFHSNIGSTRC_Value" runat="server"
                                                                                                                    ToolTip="HSN IGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACSGSTRC_Value" runat="server"
                                                                                                                    ToolTip="SAC SGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACCGSTRC_Value" runat="server"
                                                                                                                    ToolTip="SAC CGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblFSACIGSTRC_Value" runat="server"
                                                                                                                    ToolTip="SAC IGST RC Value" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTCS_Amount" runat="server"
                                                                                                                    ToolTip="TCS Amount" /></td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblExcise_Duty_CVD" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblService_Tax_Amt" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTtlRtntionAmount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="Totlblothers" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblTotal_SCH_Amt" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td style="display: none;">
                                                                                                                <asp:Label ID="TotlblAsset_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblBase_Amt_VAT" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lblTtlSmryITC_Value" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblTDS" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblWCT_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblCENVAT_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblTotal_Deduction" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotlblNet_Payable_Amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="Totlblcapitalised_amount" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="TotllblCur_Capitalised_Amount" runat="server"></asp:Label>
                                                                                                            </td>
																											<td></td>
																											<%--<td></td>
                                                                                                            <td></td>--%>
                                                                                                        </tr>
                                                                                                        </table>
                                                                                                    </FooterTemplate>
                                                                                                </asp:Repeater>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <uc4:PageNavigatorSummary ID="ucCustomPagingSummary" runat="server"></uc4:PageNavigatorSummary>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <cc1:CollapsiblePanelExtender ID="cpeInvoiceDtls" runat="Server" TargetControlID="pnlInvoiceDtlsinfo"
                                                                ExpandControlID="pnlInvoiceDtls" CollapseControlID="pnlInvoiceDtls" Collapsed="True"
                                                                TextLabelID="lblDetailsInvoiceDtl" ImageControlID="imgDetailsInvoiceDtls" ExpandedText="(Hide Details...)"
                                                                CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" class="styleContentTable">
                                                            <asp:Panel ID="pnlCashflows" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                                <div style="float: left;">
                                                                    Cashflow details
                                                                </div>
                                                                <div style="float: left; margin-left: 20px;">
                                                                    <asp:Label ID="lblDetailsCF" runat="server">(Show Details...)</asp:Label>
                                                                </div>
                                                                <div style="float: right; vertical-align: middle;">
                                                                    <asp:ImageButton ID="imgDetailsCF" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                                        AlternateText="(Show Details...)" />
                                                                </div>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlCashflowInfo" CssClass="stylePanel" runat="server" Width="100%">
                                                                <table width="100%" border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="panInflow" runat="server" CssClass="stylePanel" GroupingText="Cash Inflow details"
                                                                                Width="100%">
                                                                                <asp:GridView ID="gvInflow" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvInflow_RowDeleting"
                                                                                    ShowFooter="True" OnRowCreated="gvInflow_RowCreated" Width="100%" OnRowDataBound="gvInflow_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Date" ItemStyle-Width="12%" FooterStyle-Width="12%">
                                                                                            <ItemTemplate>
                                                                                                <%--<asp:Label ID="lblDate_GridInflow" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>--%>
                                                                                                <asp:TextBox ID="txtDate_GrdInflowSecuDept" AutoPostBack="true" runat="server" Width="95%" OnTextChanged="txtDate_GrdInflowSecuDept_TextChanged"
                                                                                                    MaxLength="10" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'>
                                                                                                </asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarExtenderSecuDept_InflowDate" runat="server" Enabled="false"
                                                                                                    TargetControlID="txtDate_GrdInflowSecuDept" OnClientDateSelectionChanged="checkApplicationDate">
                                                                                                </cc1:CalendarExtender>
                                                                                                <cc1:FilteredTextBoxExtender ID="FTEDate_GrdInflowSecuDept" runat="server" FilterMode="InvalidChars" TargetControlID="txtDate_GrdInflowSecuDept"
                                                                                                    InvalidChars="~`!@#$%^&*()_=+{}|[]\:;'<,>.?">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtDate_GridInflow" runat="server" Width="95%">
                                                                                                </asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarExtenderSD_InflowDate" runat="server" Enabled="True"
                                                                                                    TargetControlID="txtDate_GridInflow" OnClientDateSelectionChanged="checkApplicationDate">
                                                                                                </cc1:CalendarExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtDate_GridInflow" runat="server" ControlToValidate="txtDate_GridInflow"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                    ErrorMessage="Enter the Date in Inflow"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Cash flow Id" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblInflowid" runat="server" Text='<%# Bind("CashInFlowID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Cash flow Description" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblInflowDesc" runat="server" Text='<%# Bind("CashInFlow") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:DropDownList ID="ddlInflowDesc" runat="server" Width="99%">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvddlInflowDesc" runat="server" ControlToValidate="ddlInflowDesc"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                    InitialValue="-1" ErrorMessage="Select a Cashflow description in Inflow"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="In flow from" ItemStyle-Width="13%" FooterStyle-Width="13%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblInflowFrom" runat="server" Text='<%# Bind("InflowFrom") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:DropDownList ID="ddlEntityName_InFlowFrom" runat="server" AutoPostBack="True"
                                                                                                    Width="99%" OnSelectedIndexChanged="ddlEntityName_InFlowFrom_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvddlEntityName_InFlowFrom" runat="server" ControlToValidate="ddlEntityName_InFlowFrom"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                    InitialValue="-1" ErrorMessage="Select a Inflow from"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="In flow from ID" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblInflowFromId" runat="server" Text='<%# Bind("InflowFromID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Entity ID" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblEntityID_InFlow" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Customer Name" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblEntityName_InFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <UC3:AutoSugg ID="ddlEntityName_InFlow" runat="server" ServiceMethod="GetVendors"
                                                                                                    ErrorMessage="Select a Customer/Entity Name in Inflow" ReadOnly="true" Width="250px"
                                                                                                    ValidationGroup="Inflow" IsMandatory="true" />
                                                                                                <%-- <asp:DropDownList ID="ddlEntityName_InFlow" runat="server" Width="99%">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvddlEntityName_InFlow" runat="server" ControlToValidate="ddlEntityName_InFlow"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                InitialValue="0" ErrorMessage="Select a Customer/Entity name in Inflow"></asp:RequiredFieldValidator>--%>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%-- Remarks --%>
                                                                                        <asp:TemplateField HeaderText="Remarks" FooterStyle-Width="11%" ItemStyle-Width="11%"
                                                                                            HeaderStyle-Width="11%">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtRemarks" ToolTip="Remarks" Width="95%" runat="server" ReadOnly="true"
                                                                                                    TextMode="MultiLine" MaxLength="100" onkeyup="maxlengthfortxt(100);" Text='<%# Bind("Remarks")%>'> </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtFooterRemarks" ToolTip="Remarks" TextMode="MultiLine"
                                                                                                    Width="95%" MaxLength="100" onkeyup="maxlengthfortxt(100);" runat="server" Wrap="true"></asp:TextBox>
                                                                                            </FooterTemplate>
                                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="11%" FooterStyle-Width="11%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmount_Inflow" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtAmount_Inflow" runat="server" Width="94%" MaxLength="10" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Inflow" runat="server" FilterType="Numbers"
                                                                                                    TargetControlID="txtAmount_Inflow">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtAmount_Inflow" runat="server" ControlToValidate="txtAmount_Inflow"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Inflow" SetFocusOnError="True"
                                                                                                    ErrorMessage="Enter the Inflow amount"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                                                    Text="Remove"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Inflow"
                                                                                                    OnClick="CashInflow_AddRow_OnClick" CssClass="styleGridShortButton" />
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%-- Columns to be added for IRR Calculation--%>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAccountingIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblBusinessIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblComapnyIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%-- Columns to be added for IRR Calculation Ends--%>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel runat="server" ID="div5" CssClass="stylePanel" GroupingText="Cash Outflow details"
                                                                                Width="100%">
                                                                                <asp:GridView ID="gvOutFlow" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvOutFlow_RowDeleting"
                                                                                    ShowFooter="True" OnRowCreated="gvOutFlow_RowCreated" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Date" ItemStyle-Width="12%" FooterStyle-Width="12%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDate_GridOutflow" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtDate_GridOutflow" runat="server" Width="95%">
                                                                                                </asp:TextBox>
                                                                                                <cc1:CalendarExtender ID="CalendarExtenderSD_OutflowDate" runat="server" Enabled="True"
                                                                                                    TargetControlID="txtDate_GridOutflow" OnClientDateSelectionChanged="checkOutflowApplicationDate">
                                                                                                </cc1:CalendarExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtDate_GridOutflow" runat="server" ControlToValidate="txtDate_GridOutflow"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                    ErrorMessage="Enter the Outflow Date"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Cash flow Id" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblOutflowid" runat="server" Text='<%# Bind("CashOutFlowID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Cash flow Description" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblOutflowDesc" runat="server" Text='<%# Bind("CashOutFlow") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:DropDownList ID="ddlOutflowDesc" runat="server" Width="99%">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvddlOutflowDesc" runat="server" ControlToValidate="ddlOutflowDesc"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                    InitialValue="-1" ErrorMessage="Select a Cash Outflow"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Payement to ID" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPayementToId" runat="server" Text='<%# Bind("OutflowFromID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Payment to" ItemStyle-Width="13%" FooterStyle-Width="13%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPaymentto" runat="server" Text='<%# Bind("OutflowFrom") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:DropDownList ID="ddlPaymentto_OutFlow" Width="99%" runat="server" AutoPostBack="True"
                                                                                                    OnSelectedIndexChanged="ddlPaymentto_OutFlow_SelectedIndexChanged">
                                                                                                </asp:DropDownList>
                                                                                                <asp:RequiredFieldValidator ID="rfvddlPaymentto_OutFlow" runat="server" ControlToValidate="ddlPaymentto_OutFlow"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                    InitialValue="-1" ErrorMessage="Select Payment to"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Entity ID" Visible="False">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblEntityID_OutFlow" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Entity Name" ItemStyle-Width="25%" FooterStyle-Width="25%">
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblEntityName_OutFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <UC3:AutoSugg ID="ddlEntityName_OutFlow" runat="server" ServiceMethod="GetVendors"
                                                                                                    ErrorMessage="Select a Customer/Entity Name in Outflow" ReadOnly="true" Width="250px"
                                                                                                    ValidationGroup="Outflow" IsMandatory="true" />
                                                                                                <%-- <asp:DropDownList ID="ddlEntityName_OutFlow" runat="server" Width="99%">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvddlEntityName_OutFlow" runat="server" ControlToValidate="ddlEntityName_OutFlow"
                                                                                                CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                InitialValue="0" ErrorMessage="Select a Customer/Entity Name"></asp:RequiredFieldValidator>--%>
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%-- Remarks --%>
                                                                                        <asp:TemplateField HeaderText="Remarks" FooterStyle-Width="11%" ItemStyle-Width="11%"
                                                                                            HeaderStyle-Width="11%">
                                                                                            <ItemTemplate>
                                                                                                <asp:TextBox ID="txtRemarks" ToolTip="Remarks" Width="95%" runat="server" ReadOnly="true"
                                                                                                    TextMode="MultiLine" MaxLength="100" onkeyup="maxlengthfortxt(100);" Text='<%# Bind("Remarks")%>'> </asp:TextBox>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtFooterRemarks" ToolTip="Remarks" TextMode="MultiLine"
                                                                                                    Width="95%" MaxLength="100" onkeyup="maxlengthfortxt(100);" runat="server" Wrap="true"></asp:TextBox>
                                                                                            </FooterTemplate>
                                                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="11%" FooterStyle-Width="11%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmount_Outflow" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:TextBox ID="txtAmount_Outflow" runat="server" Width="94%" MaxLength="10" Style="text-align: right">
                                                                                                </asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="ftxtAmount_Outflow" runat="server" FilterType="Numbers"
                                                                                                    TargetControlID="txtAmount_Outflow">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtAmount_Outflow" runat="server" ControlToValidate="txtAmount_Outflow"
                                                                                                    CssClass="styleMandatoryLabel" Display="None" ValidationGroup="Outflow" SetFocusOnError="True"
                                                                                                    ErrorMessage="Enter the Outflow amount"></asp:RequiredFieldValidator>
                                                                                            </FooterTemplate>
                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField ItemStyle-Width="15%" FooterStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                                                    Text="Remove"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                            <FooterTemplate>
                                                                                                <asp:Button ID="btnAddOut" runat="server" Text="Add" CausesValidation="true" ValidationGroup="Outflow"
                                                                                                    OnClick="CashOutflow_AddRow_OnClick" CssClass="styleGridShortButton" />
                                                                                            </FooterTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%-- Columns to be added for IRR Calculation--%>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAccountingIRR" runat="server" Text='<%# Bind("Accounting_IRR") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblBusinessIRR" runat="server" Text='<%# Bind("Business_IRR") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblComapnyIRR" runat="server" Text='<%# Bind("Company_IRR") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField Visible="false">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblCashFlow_Flag_ID" runat="server" Text='<%# Bind("CashFlow_Flag_ID") %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%-- Columns to be added for IRR Calculation Ends--%>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="right" class="styleGridHeader" width="98%">
                                                                            <asp:Label ID="lblTotal" runat="server" Text="Total Out flow Amount :" Font-Bold="True"></asp:Label>
                                                                            <asp:Label ID="lblTotalOutFlowAmount" runat="server" Font-Bold="True" Text="0"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <cc1:CollapsiblePanelExtender ID="cpeCashflows" runat="Server" TargetControlID="pnlCashflowInfo"
                                                                ExpandControlID="pnlCashflows" CollapseControlID="pnlCashflows" Collapsed="True"
                                                                TextLabelID="lblDetailsCF" ImageControlID="imgDetailsCF" ExpandedText="(Hide Details...)"
                                                                CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                                CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:Button ID="btnModal" Style="display: none" runat="server" />
                                        <cc1:ModalPopupExtender ID="ModalPopupExtenderApprover" runat="server" TargetControlID="btnModal"
                                            PopupControlID="pnlInvoices" BackgroundCssClass="styleModalBackground" Enabled="True">
                                        </cc1:ModalPopupExtender>
                                        <asp:Panel ID="pnlInvoices" Style="display: none; right: auto" runat="server" BorderStyle="Solid" BackColor="White" Width="100%">
                                            <asp:UpdatePanel ID="upnlinvoices" runat="server">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>Type</td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlFilterType" runat="server">
                                                                            </asp:DropDownList></td>
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtFilterValue" runat="server"
                                                                                CssClass="styleSearchBox"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:Button ID="btnGo" runat="server" Text="Go" ToolTip="Go"
                                                                                CssClass="styleSubmitShortButton" OnClick="btnGoInvoice_Click" ValidationGroup="btngo" /></td>
                                                                        <td>
                                                                    </tr>

                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="pnlPopUp" runat="server" Width="99%">
                                                                    <div id="divPopUp" style="max-height: 500px; overflow: auto;" runat="server">
                                                                        <table width="99%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Repeater ID="grvInvoices" runat="server" OnItemDataBound="grvInvoices_ItemDataBound">
                                                                                        <HeaderTemplate>
                                                                                            <table style="border: 1px solid #bad4ff;">
                                                                                                <tr align="center" style="font-family: calibri; font-size: 13px; font-weight: bold; background-color: aliceblue;">
                                                                                                    <td>
                                                                                                        <asp:CheckBox runat="server" ID="chkSelectedH" AutoPostBack="true" OnCheckedChanged="chkSelectedH_CheckedChanged"
                                                                                                            ToolTip="select" />
                                                                                                    </td>
                                                                                                    <td>Vendor Name
                                                                                                    </td>
                                                                                                    <td>Vendor Branch State
                                                                                                    </td>
                                                                                                    <td>Asset Category
                                                                                                    </td>
                                                                                                    <td>Asset Type
                                                                                                    </td>
                                                                                                    <td>Asset Sub Type</td>
                                                                                                    <td>Invoice Type
                                                                                                    </td>
                                                                                                    <td>Invoice No
                                                                                                    </td>
                                                                                                    <td>Invoice Date
                                                                                                    </td>
                                                                                                    <td>Qty
                                                                                                    </td>
                                                                                                    <td>Invoice Material
                                                                                                    </td>
                                                                                                    <td>Labour Amount
                                                                                                    </td>
                                                                                                    <td>VAT
                                                                                                    </td>
                                                                                                    <td>CST
                                                                                                    </td>
                                                                                                    <td>Excise Duty CVD
                                                                                                    </td>
                                                                                                    <td>Service Tax
                                                                                                    </td>
                                                                                                    <td>Retention
                                                                                                    </td>
                                                                                                    <td>Others
                                                                                                    </td>
                                                                                                    <td>Total Bill Amount
                                                                                                    </td>
                                                                                                    <td>LBT Amount
                                                                                                    </td>
                                                                                                    <td>Purchase Tax
                                                                                                    </td>
                                                                                                    <td>Entry Tax
                                                                                                    </td>
                                                                                                    <td>Reverse Charge
                                                                                                    </td>
                                                                                                    <td>Cr. Note
                                                                                                    </td>
                                                                                                    <td>Total SCH Amt
                                                                                                    </td>
                                                                                                    <td>Base Amt VAT
                                                                                                    </td>
                                                                                                    <td>ITC Value
                                                                                                    </td>
                                                                                                    <td>TDS
                                                                                                    </td>
                                                                                                    <td>WCT Amount
                                                                                                    </td>
                                                                                                    <td>CENVAT Amount
                                                                                                    </td>
                                                                                                    <td>Total Deduction
                                                                                                    </td>
                                                                                                    <td>Net Payable Amount
                                                                                                    </td>
                                                                                                    <td>Capitalised Amount
                                                                                                    </td>
                                                                                                    <td style="display: none;"></td>
                                                                                                    <td style="display: none;"></td>
                                                                                                </tr>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <tr style="font-family: calibri; font-size: 13px;">
                                                                                                <td>
                                                                                                    <asp:CheckBox runat="server" ID="chkSelected" AutoPostBack="true" OnCheckedChanged="chkSelected_CheckedChanged"
                                                                                                        ToolTip="select" />
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblVendor_Code" Visible="false" runat="server" Text='<%# Bind("Vendor_Code") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblVendor_Name" runat="server" Text='<%# Bind("Vendor_Name") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblStateName" runat="server" Text='<%# Bind("State_Name") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblAssetCategorySG" runat="server" Text='<%# Bind("AssetCategory") %>'></asp:Label>
                                                                                                    <asp:Label ID="lblAssetCategoryIDSG" Visible="false" runat="server" Text='<%# Bind("Asset_Category_ID") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblAssetType" runat="server" Text='<%# Bind("Asset_Type") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblAssetSubType" runat="server" Text='<%# Bind("Asset_Sub_Type") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblInvoice_Type" runat="server" Text='<%# Bind("Invoice_Type") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblInvoiceId" runat="server" Text='<%# Bind("Invoice_Id") %>' Visible="false"></asp:Label>
                                                                                                    <asp:Label ID="lblInvoice_No" runat="server" Text='<%# Bind("Invoice_No") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblInvoice_Date" runat="server" Text='<%# Bind("Invoice_Date") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Quantity") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblBase_Inv_Amt_Mat" runat="server" Text='<%# Bind("Base_Inv_Amt_Mat") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblBase_Inv_Amt_lbr" runat="server" Text='<%# Bind("Base_Inv_Amt_lbr") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblVAT" runat="server" Text='<%# Bind("VAT") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblCST" runat="server" Text='<%# Bind("CST") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblExcise_Duty_CVD" runat="server" Text='<%# Bind("Excise_Duty_CVD") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblService_Tax_Amt" runat="server" Text='<%# Bind("Service_Tax_Amt") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblRtntionAmount" runat="server" Text='<%# Bind("Retention_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblothers" runat="server" Text='<%# Bind("others") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblTotal_Bill_Amount" runat="server" Text='<%# Bind("Total_Bill_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblLBT_Amount" runat="server" Text='<%# Bind("LBT_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblPurchase_Tax" runat="server" Text='<%# Bind("Purchase_Tax") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblEntry_Tax_Amount" runat="server" Text='<%# Bind("Entry_Tax_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblReverse_Charge_Tax" runat="server" Text='<%# Bind("Reverse_Charge_Tax") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblCrNote" runat="server" Text='<%# Bind("Credit_Note_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblTotal_SCH_Amt" runat="server" Text='<%# Bind("Total_SCH_Amt") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblBase_Amt_VAT" runat="server" Text='<%# Bind("Base_Amt_VAT") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblITC_Value" runat="server" Text='<%# Bind("ITC_Value") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblTDS" runat="server" Text='<%# Bind("TDS") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblWCT_Amount" runat="server" Text='<%# Bind("WCT_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblCENVAT_Amount" runat="server" Text='<%# Bind("CENVAT_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblTotal_Deduction" runat="server" Text='<%# Bind("Total_Deduction") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblNet_Payable_Amount" runat="server" Text='<%# Bind("Net_Payable_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblcapitalised_amount" runat="server" Text='<%# Bind("capitalised_amount") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td style="display: none;">
                                                                                                    <asp:Label ID="lblAsset_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td style="display: none;">
                                                                                                    <asp:Label ID="lblInvoice_Amount" runat="server" Text='<%# Bind("Invoice_Amount") %>'></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </ItemTemplate>
                                                                                        <FooterTemplate>
                                                                                            <tr style="font-family: calibri; font-size: 13px; font-weight: bold; background-color: aliceblue;">
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblBase_Inv_Amt_Mat" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblBase_Inv_Amt_lbr" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblVAT" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblCST" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblExcise_Duty_CVD" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblService_Tax_Amt" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblTotalRetention" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="Totlblothers" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblTotal_Bill_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblLBT_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblPurchase_Tax" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblEntry_Tax_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblReverse_Charge_Tax" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblTotCrNote" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblTotal_SCH_Amt" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblBase_Amt_VAT" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblITC_Value" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblTDS" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblWCT_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblCENVAT_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblTotal_Deduction" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="TotlblNet_Payable_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="Totlblcapitalised_amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td style="display: none;">
                                                                                                    <asp:Label ID="TotlblAsset_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td style="display: none;">
                                                                                                    <asp:Label ID="TotlblInvoice_Amount" runat="server"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            </table>
                                                                                        </FooterTemplate>
                                                                                    </asp:Repeater>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <uc5:PageNavigator ID="ucCustomPaging" runat="server"></uc5:PageNavigator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="top">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <input type="hidden" id="hdnSortDirection" runat="server" />
                                                                            <input type="hidden" id="hdnSortExpression" runat="server" />
                                                                            <input type="hidden" id="hdnSearch" runat="server" />
                                                                            <input type="hidden" id="hdnOrderBy" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btnDEVModalMove" runat="server" Text="Move" ToolTip="Move" class="styleSubmitButton"
                                                                    OnClick="btnDEVModalMove_Click" />
                                                                <asp:Button ID="btnDEVModalCancel" runat="server" Text="Close" OnClick="btnDEVModalCancel_Click"
                                                                    ToolTip="Close" class="styleSubmitButton" />
                                                                <asp:Button ID="btnDevModalTotal" runat="server" Text="View Total"
                                                                    ToolTip="View Total" class="styleSubmitButton" OnClick="btnDevModalTotal_Click" />
                                                                <asp:Button ID="btnModalMoveall" runat="server" Text="Move All"
                                                                    ToolTip="Move All" class="styleSubmitButton" OnClick="btnDEVModalMoveAll_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:CustomValidator ID="cvPopUP" runat="server" CssClass="styleMandatoryLabel"
                                                                    Enabled="true" Width="98%" ValidationGroup="btngo" /></td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>

                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabRepayment" HeaderText="TabRepayment" CssClass="tabpan"
                                    BackColor="Red" Width="98%">
                                    <HeaderTemplate>
                                        Repayment Details
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="upRepayment" runat="server">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" width="33%" valign="top">
                                                            <asp:Panel ID="Panel11" runat="server" CssClass="stylePanel" GroupingText="Repayment Details"
                                                                Width="99%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblTotalAmount" CssClass="styleDisplayLabel" Font-Bold="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblFrequency_Display" runat="server" CssClass="styleDisplayLabel"
                                                                                Font-Bold="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblMarginResidual" runat="server" CssClass="styleDisplayLabel" Font-Bold="false"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="left" valign="top" width="30%">
                                                            <asp:Panel ID="Panel10" runat="server" CssClass="stylePanel" GroupingText="Repayment Summary"
                                                                Width="99%">
                                                                <asp:GridView ID="gvRepaymentSummary" runat="server" AutoGenerateColumns="false"
                                                                    Width="100%">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CashFlow" HeaderText="Cash Flow Description" />
                                                                        <asp:BoundField DataField="TotalPeriodInstall" HeaderText="Amount" ItemStyle-HorizontalAlign="Right" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                        <td align="left" width="35%">
                                                            <asp:Panel ID="panIRR" runat="server" CssClass="stylePanel" GroupingText="IRR Details"
                                                                Width="99%">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAccountIRR_Repay" CssClass="styleDisplayLabel"
                                                                                Text="Accounting IRR" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAccountIRR_Repay" runat="server" Style="text-align: right" Width="100px"
                                                                                Font-Bold="true"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvAccountingIRR" runat="server" Display="None" ValidationGroup="Repayment Details"
                                                                                ErrorMessage="Calculate Accounting IRR" ControlToValidate="txtAccountIRR_Repay"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblBusinessIRR_Repay" CssClass="styleDisplayLabel"
                                                                                Font-Bold="true" Text="Business IRR"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtBusinessIRR_Repay" runat="server" Width="100px" Style="text-align: right"
                                                                                Font-Bold="true"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvBusinessIRR" runat="server" Display="None" ValidationGroup="Repayment Details"
                                                                                ErrorMessage="Calculate Business IRR" ControlToValidate="txtBusinessIRR_Repay"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblCompanyIRR_Repay" CssClass="styleDisplayLabel"
                                                                                Font-Bold="true" Text="Company IRR"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCompanyIRR_Repay" runat="server" Width="100px" Style="text-align: right"
                                                                                Font-Bold="true"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="rfvCompanyIRR" runat="server" Display="None" ValidationGroup="Repayment Details"
                                                                                ErrorMessage="Calculate Company IRR" ControlToValidate="txtCompanyIRR_Repay"
                                                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblRepaymentMode" CssClass="styleReqFieldLabel" Text="Installment Repayment Mode"></asp:Label>
                                                            <asp:DropDownList ID="ddlRepaymentMode" runat="server"></asp:DropDownList>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:Panel runat="server" ID="Panel2" CssClass="stylePanel" GroupingText="Repayment Details"
                                                                Width="99%">
                                                                <div id="div6" style="overflow: auto; width: 100%;" runat="server">
                                                                    <asp:GridView ID="gvRepaymentDetails" runat="server" AutoGenerateColumns="False"
                                                                        ShowFooter="True" OnRowDeleting="gvRepaymentDetails_RowDeleting" OnRowDataBound="gvRepaymentDetails_RowDataBound"
                                                                        OnRowCreated="gvRepaymentDetails_RowCreated" Width="100%">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="slno" HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                                                <FooterStyle Width="2%" />
                                                                                <ItemStyle HorizontalAlign="Center" Width="2%" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Repayment CashFlow" ItemStyle-Width="23%" FooterStyle-Width="23%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCashFlow" runat="server" Text='<%# Bind("CashFlow") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:DropDownList ID="ddlRepaymentCashFlow_RepayTab" runat="server" Width="98%" AutoPostBack="true"
                                                                                        OnSelectedIndexChanged="ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvddlRepaymentCashFlow_RepayTab" runat="server"
                                                                                        ControlToValidate="ddlRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                                        Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" InitialValue="-1"
                                                                                        ErrorMessage="Select a Repayment cashflow"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                                <FooterStyle Width="23%" />
                                                                                <ItemStyle Width="23%" />
                                                                            </asp:TemplateField>
                                                                            <%--<asp:TemplateField HeaderText="Amount" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterStyle HorizontalAlign="Right" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmountRepaymentCashFlow_RepayTab" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAmountRepaymentCashFlow_RepayTab" runat="server" Width="97%"
                                                                        MaxLength="10">
                                                                    </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtAmountRepaymentCashFlow_RepayTab" runat="server"
                                                                        FilterType="Numbers" TargetControlID="txtAmountRepaymentCashFlow_RepayTab">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtAmountRepaymentCashFlow_RepayTab" runat="server"
                                                                        ControlToValidate="txtAmountRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                        Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the amount"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>--%>
                                                                            <asp:TemplateField HeaderText="Per Installment Amount" ItemStyle-Width="10%" FooterStyle-Width="10%"
                                                                                FooterStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPerInstallmentAmount_RepayTab" runat="server" Text='<%# Bind("PerInstall") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtPerInstallmentAmount_RepayTab" runat="server" Width="95%" Style="text-align: right;"
                                                                                        MaxLength="10">
                                                                                    </asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                                        FilterType="Numbers" TargetControlID="txtPerInstallmentAmount_RepayTab">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <asp:RequiredFieldValidator ID="rfvtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                                        ControlToValidate="txtPerInstallmentAmount_RepayTab" CssClass="styleMandatoryLabel"
                                                                                        Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the Per installment amount"></asp:RequiredFieldValidator>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Breakup Percentage" Visible="false" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBreakup_RepayTab" runat="server" Text='<%# Bind("Breakup") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtBreakup_RepayTab" runat="server" Width="95%" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                                                    </asp:TextBox>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="From Installment" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblFromInstallment_RepayTab" runat="server" Text='<%# Bind("FromInstall") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:TextBox ID="txtFromInstallment_RepayTab" runat="server" Width="95%" MaxLength="3"
                                                                                        Style="text-align: right" Text="1"><%--ReadOnly="true" --%>
                                                                                    </asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtFromInstallment_RepayTab" runat="server"
                                                                                        FilterType="Numbers" TargetControlID="txtFromInstallment_RepayTab">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <%--<asp:RequiredFieldValidator ID="rfvtxtFromInstallment_RepayTab" runat="server" ControlToValidate="txtFromInstallment_RepayTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                        SetFocusOnError="True" ErrorMessage="Enter the From installment"></asp:RequiredFieldValidator>--%>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="To Installment" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterStyle HorizontalAlign="Right" />
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
                                                                            <asp:TemplateField HeaderText="From Due Date" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '></asp:Label>
                                                                                    <asp:TextBox ID="txRepaymentFromDate" runat="server" Visible="false" BackColor="Navy"
                                                                                        ForeColor="White" Font-Names="calibri" Font-Size="12px" Width="95%" Style="color: White"
                                                                                        Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '
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
                                                                                    <%--  <asp:RequiredFieldValidator ID="rfvtxtfromdate_RepayTab" runat="server" ControlToValidate="txtfromdate_RepayTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                        SetFocusOnError="True" ErrorMessage="Enter the from date"></asp:RequiredFieldValidator>--%>
                                                                                </FooterTemplate>
                                                                                <FooterStyle Width="10%" />
                                                                                <ItemStyle Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="To Due Date" ItemStyle-Width="10%" FooterStyle-Width="10%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTODate_ReapyTab" runat="server" Width="100%" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ToDate")).ToString(strDateFormat) %>'></asp:Label>
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
                                                                            <asp:TemplateField HeaderText="Add" ItemStyle-Width="5%" FooterStyle-Width="5%">
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                <FooterStyle HorizontalAlign="Center" />
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnRemoveRepayment" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                        Text="Remove" Visible="false"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Button ID="btnAddRepayment" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                                                        OnClick="btnAddRepayment_OnClick" ValidationGroup="TabRepayment1" OnClientClick="return fnCheckPageValidators('TabRepayment1',false)"></asp:Button>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Repayment CashFlowId" Visible="False">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCashFlowId" runat="server" Text='<%# Bind("CashFlowId") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <%-- Columns to be added for IRR Calculation--%>
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
                                                                            <%-- Columns to be added for IRR Calculation--%>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <table width="99%">
                                                                <tr align="right">
                                                                    <td>
                                                                        <asp:Button runat="server" ID="btnCalIRR" Text="Calculate IRR" CssClass="styleSubmitButton"
                                                                            OnClick="btnCalIRR_Click" />
                                                                        <asp:Button runat="server" ID="btnReset" Text="Reset" CssClass="styleSubmitShortButton"
                                                                            OnClick="btnReset_Click" OnClientClick="return Confirmmsg('Do you want to Reset Repayment Structure?')" />
                                                                        <input id="Hidden1" type="hidden" runat="server" />
                                                                        <input id="Hidden2" type="hidden" runat="server" />
                                                                        <input id="hdnRoundOff" type="hidden" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:GridView ID="grvRepayStructure" runat="server" Width="100%" AutoGenerateColumns="false" OnRowDataBound="grvRepayStructure_RowDataBound">
                                                                            <Columns>
                                                                                <asp:BoundField DataField="InstallmentNo" HeaderText="Installment No" ItemStyle-HorizontalAlign="Center" />
                                                                                <asp:BoundField DataField="From_Date" HeaderText="From Date" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="To_Date" HeaderText="To Date" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="Installment_Date" HeaderText="Installment Date" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" />
                                                                                <asp:BoundField DataField="NoofDays" HeaderText="No Of days" ItemStyle-HorizontalAlign="Right" />
                                                                                <%--<asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount" ItemStyle-HorizontalAlign="Right" />--%>
                                                                                <asp:TemplateField HeaderText="Rental Amount" ItemStyle-Width="100px"   ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblInstallmentAmount" Style="text-align: right" runat="server" Text='<%# Bind("InstallmentAmount") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="FinanceCharges" HeaderText="Finance Charges" ItemStyle-HorizontalAlign="Right"
                                                                                    Visible="false" />
                                                                                <asp:BoundField DataField="PrincipalAmount" HeaderText="Principal Amount" ItemStyle-HorizontalAlign="Right"
                                                                                    Visible="false" />
                                                                                <%-- <asp:BoundField DataField="Tax" HeaderText="Tax" ItemStyle-HorizontalAlign="Right" />
                                                                                   <asp:BoundField DataField="AMF" HeaderText="AMF" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:BoundField DataField="ServiceTax" HeaderText="Service Tax" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:BoundField DataField="Insurance" HeaderText="Insurance" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:BoundField DataField="Others" HeaderText="Others" ItemStyle-HorizontalAlign="Right" />--%>
                                                                                <asp:TemplateField HeaderText="Tax" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTax" Style="text-align: right" runat="server" Text='<%# Bind("Tax") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="AMF" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAMF" Style="text-align: right" runat="server" Text='<%# Bind("AMF") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Service Tax" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblServiceTax" Style="text-align: right" runat="server" Text='<%# Bind("ServiceTax") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Insurance" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblInsurance" Style="text-align: right" runat="server" Text='<%# Bind("Insurance") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Others" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOthers" Style="text-align: right" runat="server" Text='<%# Bind("Others") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="CurrBalance" HeaderText="Balance Principal" ItemStyle-HorizontalAlign="Right"
                                                                                    Visible="false" />
                                                                                
                                                                                <asp:TemplateField HeaderText="Cess Amount" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCessAmount" Style="text-align: right" runat="server" Text='<%# Bind("Cess_Amount") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                 <asp:TemplateField HeaderText="Rebate Discount" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblRebateDiscount" Style="text-align: right" runat="server" Text='<%# Bind("Rebate_Discount") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Addi. Rebate Discount" ItemStyle-HorizontalAlign="Right">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblxAddiRebateDiscount" Style="text-align: right" runat="server" Text='<%# Bind("Addi_Rebate_Discount") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                 
                                                                                <%-- <asp:BoundField DataField="CUS_OW" HeaderText="Cus OW" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:BoundField DataField="ET_IW" HeaderText="ET IW" ItemStyle-HorizontalAlign="Right" />
                                                                                <asp:BoundField DataField="ET_OW" HeaderText="ET OW" ItemStyle-HorizontalAlign="Right" />--%>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                        <br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Button ID="Button1" runat="server" Style="display: none" Text="Button" CausesValidation="false"
                                                                OnClick="btnGenerateRepay_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                                                                Enabled="true" ValidationGroup="TabRepayment1" Width="98%" ShowMessageBox="false"
                                                                HeaderText="Correct the following validation(s):  " ShowSummary="true" />
                                                            <asp:CustomValidator ID="CV_IRR" runat="server" CssClass="styleMandatoryLabel"
                                                                Display="None" ValidationGroup="TabRepayment1"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnCalIRR" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnGenerateRepay" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnExportRepay" runat="server" OnClick="btnExportRepay_Click" CssClass="styleSubmitButton"
                                                        Text="Export Repayment" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabInvoice" CssClass="tabpan" BackColor="Red" Width="98%">
                                    <HeaderTemplate>
                                        Guarantee 
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td valign="top" style="width: 100%">
                                                            <asp:Panel runat="server" ID="Panel1" CssClass="stylePanel" GroupingText="Guarantor Details">
                                                                <asp:GridView ID="gvGuarantor" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                                    Width="100%" OnRowDeleting="gvGuarantor_RowDeleting" OnRowDataBound="gvGuarantor_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Guarantor type" Visible="false" ItemStyle-Width="18%" FooterStyle-Width="18%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="ddlGuarantortype_GuarantorTab" runat="server" Text='<%# Bind("Guarantor") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlGuarantortype_GuarantorTab" runat="server" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlGuarantortype_SelectedIndexChanged" Width="100%">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvddlGuarantortype_GuarantorTab" runat="server"
                                                                                    ControlToValidate="ddlGuarantortype_GuarantorTab" CssClass="styleMandatoryLabel"
                                                                                    ValidationGroup="Guarantor" Display="None" InitialValue="-1" SetFocusOnError="True"
                                                                                    ErrorMessage="Select a Guarantor type"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="GuaranteeID" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblGuaranteeID" runat="server" Text='<%# Bind("Code") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Guarantor" ItemStyle-Width="50%" FooterStyle-Width="50%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="ddlCode_GuarantorTab" runat="server" Text='<%# Bind("Name") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtguarantor" runat="server"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Guarantee Amount" ItemStyle-HorizontalAlign="Right"
                                                                            ItemStyle-Width="15%" FooterStyle-Width="15%" FooterStyle-HorizontalAlign="Right">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="txtGuaranteeamount_GuarantorTab" runat="server" Text='<%# Bind("Amount") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtGuaranteeamount_GuarantorTab_Footer" runat="server" Style="text-align: right"
                                                                                    Width="95%" MaxLength="10">
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="ftxtGuarateeAmount" runat="server" TargetControlID="txtGuaranteeamount_GuarantorTab_Footer"
                                                                                    FilterType="Numbers" Enabled="True">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <asp:RequiredFieldValidator ID="rfvtxtGuaranteeamount_GuarantorTab" runat="server"
                                                                                    ControlToValidate="txtGuaranteeamount_GuarantorTab_Footer" CssClass="styleMandatoryLabel"
                                                                                    ValidationGroup="Guarantor" Display="None" SetFocusOnError="True" ErrorMessage="Enter the Guarantee Amount"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Charge Sequence" Visible="false" ItemStyle-Width="18%" FooterStyle-Width="18%">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="ddlChargesequence_GuarantorTab" runat="server" Text='<%# Bind("ChargeSequence") %>'>
                                                                                </asp:Label>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:DropDownList ID="ddlChargesequence_GuarantorTab" runat="server" Width="96%">
                                                                                </asp:DropDownList>
                                                                                <asp:RequiredFieldValidator ID="rfvddlChargesequence_GuarantorTab" runat="server"
                                                                                    ControlToValidate="ddlChargesequence_GuarantorTab" CssClass="styleMandatoryLabel"
                                                                                    ValidationGroup="Guarantor" Display="None" InitialValue="-1" SetFocusOnError="True"
                                                                                    ErrorMessage="Select a Charge Sequence"></asp:RequiredFieldValidator>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Customer" Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbtnViewCustomer" CausesValidation="false" runat="server" CommandName="Edit"
                                                                                    Text="View"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                                    Text="Remove"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Button ID="LbtnAddInvoice" runat="server" CausesValidation="true" OnClick="Guarantor_AddRow_OnClick"
                                                                                    Text="Add" ValidationGroup="Guarantor" CssClass="styleSubmitButton"></asp:Button>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                            <asp:GridView ID="gvCollateralDetails" runat="server" AutoGenerateColumns="false"
                                                                Width="98%" Caption="Collateral reference">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                    <asp:BoundField DataField="Collateralreference" HeaderText="Collateral reference" />
                                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                                    <asp:BoundField DataField="Collateralvalue" HeaderText="Collateral value" />
                                                                    <asp:BoundField DataField="viewcollateral" HeaderText="View Collateral" />
                                                                </Columns>
                                                            </asp:GridView>
                                                            <asp:Panel runat="server" ID="pnlInvoiceDetails" CssClass="stylePanel" GroupingText="Invoice Details"
                                                                Width="99%" Visible="false">
                                                                <asp:GridView ID="gvInvoiceDetails" runat="server" AutoGenerateColumns="false" Width="100%"
                                                                    OnRowDataBound="gvInvoiceDetails_RowDataBound">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Invoice Transaction reference" ItemStyle-HorizontalAlign="Right"
                                                                            Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblInvoiceReferNo" runat="server" Text='<%#Bind("Invoice_Id") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="Doc_Invoice_No" HeaderText="Invoice Reference" ItemStyle-Width="15%" />
                                                                        <asp:BoundField DataField="Invoice_No" HeaderText="Invoice No" />
                                                                        <asp:BoundField DataField="Vendor" HeaderText="Vendor Name" />
                                                                        <asp:TemplateField HeaderText="Invoice Details" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lbtnViewInvoice" CausesValidation="false" runat="server" Text="View"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabSLADetails" CssClass="tabpan" BackColor="Red"
                                    Width="98%" HeaderText=" Rental Schedule User Details">
                                    <HeaderTemplate>
                                        Rental Schedule Delivery Address
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlDeliveryAddress" runat="server" CssClass="stylePanel" GroupingText="Rental Schedule Delivery Address">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblDeliveryType" CssClass="styleReqFieldLabel" Text="Delivery Type"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:DropDownList ID="ddlDeliveryType" AutoPostBack="true" OnSelectedIndexChanged="ddlDeliveryType_SelectedIndexChange" runat="server">
                                                                            </asp:DropDownList>
                                                                            <%--   <asp:RequiredFieldValidator ID="rfvddlDeliveryType" InitialValue="0" runat="server" ControlToValidate="ddlDeliveryType"
                                                                                Display="None" ValidationGroup="AccountCreation" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                                                                ErrorMessage="Select a Delivery Type"></asp:RequiredFieldValidator>--%>
                                                                            <asp:DropDownList ID="ddlCust_Address" AutoPostBack="true" OnSelectedIndexChanged="ddlCust_Address_SelectedIndexChange" runat="server">
                                                                            </asp:DropDownList>
                                                                            <%--  <asp:RequiredFieldValidator ID="rfvddlCust_Address" InitialValue="0" runat="server" ControlToValidate="ddlCust_Address"
                                                                                Display="None" ValidationGroup="AccountCreation" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                                                                ErrorMessage="Select a Billing State" Enabled="false"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblLabel" CssClass="styleDisplayLabel" Text="Label"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtLabel" MaxLength="50" runat="server" Width="300"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAddress1DA" CssClass="styleReqFieldLabel" Text="Address1"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAddress1DA" onkeyup="maxlengthfortxt(300);" runat="server" TextMode="MultiLine" Height="100" Width="400"></asp:TextBox>
                                                                            <%--<asp:RequiredFieldValidator ID="rfvtxtAddress1DA" runat="server" ControlToValidate="txtAddress1DA"
                                                                                Display="None" ValidationGroup="AccountCreation" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                                                                ErrorMessage="Enter the Address1"></asp:RequiredFieldValidator>--%>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAddress" CssClass="styleDisplayLabel" Text="Address"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAddress" onkeyup="maxlengthfortxt(300);" runat="server" TextMode="MultiLine" Height="100" Width="400"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblTelephone" CssClass="styleDisplayLabel" Text="Telephone"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtTelephoneDA" MaxLength="12" runat="server"></asp:TextBox>&nbsp;&nbsp;
                                                                            <asp:Label runat="server" ID="lblGSTIN" CssClass="styleDisplayLabel" Text="GSTIN"></asp:Label>&nbsp;&nbsp;
                                                                            <asp:TextBox ID="txtGSTIN" MaxLength="15" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td class="styleFieldLabel">
                                                                            
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblMobileDA" CssClass="styleDisplayLabel" Text="Mobile"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtMobileDA" MaxLength="12" runat="server"></asp:TextBox>
                                                                        &nbsp;&nbsp;
                                                                            <asp:Label runat="server" ID="lblPin" CssClass="styleDisplayLabel" Text="Pin"></asp:Label>
                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                            <asp:TextBox ID="txtPin" MaxLength="6" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                        <tr>
                                                            <td>

                                                                 <table width="99%">
                                    <tr valign="middle">
                                        <td style="padding-top: 10px; width: 100%">
                                            <asp:Panel runat="server" ID="pnlCustEmail" CssClass="stylePanel" GroupingText="Billing Email Alert Details"
                                                Width="100%">
                                                <asp:GridView ID="grvCustEmail" OnRowDataBound="grvCustEmail_RowDataBound" runat="server" AutoGenerateColumns="False"
                                                      Width="100%">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="TypeId" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustEmailDetID" runat="server" Text='<%# Bind("Cust_Email_Det_ID") %>' />
                                                        v    </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Group Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGroupName" runat="server" Text='<%# Bind("Group_Name") %>' />
                                                            </ItemTemplate>
                                                            
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="EMail ID" FooterStyle-Width="45%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEMailID" runat="server" Text='<%# Bind("EMail_ID") %>' />
                                                            </ItemTemplate>
                                                           </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Billing Email Alert">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmail_Alert" runat="server" Visible="false" Text='<%# Bind("Email_Alert") %>' />
                                                                <asp:CheckBox ID="chkEmailAlert" runat="server" />
                                                            </ItemTemplate>
                                                           <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                       
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>

                                                             </td>

                                                        </tr>

                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlEUCDtls" runat="server" CssClass="stylePanel" GroupingText="End Use Customer Details">
                                                                <table width="99%" cellpadding="0" cellspacing="0">
                                                                    <tr>

                                                                        <td class="styleFieldLabel" style="width: 10%">
                                                                            <asp:Label ID="lblCustomerName_EUC" ToolTip="Customer&#8217;s Customer Name" runat="server" Text="Customer&#8217;s Customer name"
                                                                                CssClass="styleReqFieldLabel" />
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 20%">
                                                                            <asp:TextBox ID="txtCustomerName_EUC" runat="server"
                                                                                onmouseover="txt_MouseoverTooltip(this)" MaxLength="100" />
                                                                            <cc1:AutoCompleteExtender ID="autotxtCustomerName_EUC" MinimumPrefixLength="1" runat="server"
                                                                                TargetControlID="txtCustomerName_EUC" ServiceMethod="GetCustomerName_EUCList" Enabled="True" ServicePath=""
                                                                                CompletionSetCount="5" CompletionListCssClass="CompletionList" DelimiterCharacters=";,:"
                                                                                CompletionListItemCssClass="CompletionListItemCssClass"
                                                                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                                ShowOnlyCurrentWordInCompletionListItem="true">
                                                                            </cc1:AutoCompleteExtender>

                                                                            <asp:RequiredFieldValidator ID="rfvCustomerName_EUC" runat="server" ControlToValidate="txtCustomerName_EUC"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Customer&#8217;s Customer name" SetFocusOnError="False"
                                                                                ValidationGroup="btnAddEUC"></asp:RequiredFieldValidator>
                                                                            <asp:RequiredFieldValidator ID="rfvCustomerName_EUCM" runat="server" ControlToValidate="txtCustomerName_EUC"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Customer&#8217;s Customer name" SetFocusOnError="False"
                                                                                ValidationGroup="btnModifyEUC"></asp:RequiredFieldValidator>
                                                                        </td>

                                                                        <td class="styleFieldLabel" style="width: 10%">
                                                                            <asp:Label ID="lblEmailId_EUC" ToolTip="Email Id" runat="server" Text="Email Id"
                                                                                CssClass="styleReqFieldLabel" />
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 20%">
                                                                            <asp:TextBox ID="txtEmailId_EUC" runat="server"
                                                                                onmouseover="txt_MouseoverTooltip(this)" MaxLength="60" />
                                                                            <asp:RequiredFieldValidator ID="rfvtxtEmailId_EUC" runat="server" ControlToValidate="txtEmailId_EUC"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Email Id" SetFocusOnError="False"
                                                                                ValidationGroup="btnAddEUC" Enabled="false"></asp:RequiredFieldValidator>
                                                                            <asp:RequiredFieldValidator ID="rfvtxtEmailId_EUCM" runat="server" ControlToValidate="txtEmailId_EUC"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Email Id" SetFocusOnError="False"
                                                                                ValidationGroup="btnModifyEUC" Enabled="false"></asp:RequiredFieldValidator>
                                                                            <cc1:FilteredTextBoxExtender ID="FTEtxtEMailId" runat="server" TargetControlID="txtEmailId_EUC"
                                                                                FilterType="Numbers, UppercaseLetters, LowercaseLetters,Custom" ValidChars="._-@"
                                                                                Enabled="True">
                                                                            </cc1:FilteredTextBoxExtender>
                                                                            <asp:RegularExpressionValidator ID="revEmailId_EUC" runat="server" ControlToValidate="txtEmailId_EUC"
                                                                                ValidationGroup="btnAddEUC" Display="None" ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                            <asp:RegularExpressionValidator ID="revEmailId_EUCM" runat="server" ControlToValidate="txtEmailId_EUC"
                                                                                ValidationGroup="btnModifyEUC" Display="None" ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblRemarks_EUC" ToolTip="Remarks" runat="server" Text="Remarks"
                                                                                CssClass="styleReqFieldLabel" />
                                                                        </td>
                                                                        <td class="styleFieldAlign" colspan="2">
                                                                            <asp:TextBox ID="txtRemarks_EUC" Width="96%" TextMode="MultiLine" Rows="3" runat="server"
                                                                                onmouseover="txt_MouseoverTooltip(this)" MaxLength="1000" onkeyup="maxlengthfortxt(1000)" />
                                                                            <asp:RequiredFieldValidator ID="rfvRemarks_EUC" runat="server" ControlToValidate="txtRemarks_EUC"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Remarks" SetFocusOnError="False"
                                                                                ValidationGroup="btnAddEUC" Enabled="false"></asp:RequiredFieldValidator>
                                                                            <asp:RequiredFieldValidator ID="rfvRemarks_EUCM" runat="server" ControlToValidate="txtRemarks_EUC"
                                                                                CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Remarks" SetFocusOnError="False"
                                                                                ValidationGroup="btnModifyEUC" Enabled="false"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="center">
                                                                        <td align="center" colspan="6" width="100%" valign="top" height="30px">
                                                                            <asp:Button ID="btnAddEUC" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                                                ValidationGroup="btnAddEUC" OnClick="btnAddEUC_Click" />
                                                                            <asp:Button ID="btnModifyEUC" runat="server" CssClass="styleSubmitShortButton" Text="Edit"
                                                                                ValidationGroup="btnModifyEUC" Enabled="False" OnClick="btnModifyEUC_Click" />
                                                                            <asp:Button ID="btnhClearEUC" runat="server" CausesValidation="False" CssClass="styleSubmitShortButton"
                                                                                Text="Clear" OnClick="btnClearInt_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblEUCSlNo" runat="server" Visible="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" colspan="6" align="center">
                                                                            <asp:GridView ID="grvEUC" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                                Width="100%" ShowFooter="false" OnRowDeleting="grvEUC_RowDeleting">
                                                                                <Columns>
                                                                                    <asp:TemplateField HeaderText="Select" ItemStyle-Width="5%">
                                                                                        <ItemTemplate>
                                                                                            <asp:RadioButton ID="RBSelect" runat="server" Checked="false" OnCheckedChanged="RBSelectInt_CheckedChanged"
                                                                                                AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Sl No" ItemStyle-Width="5%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="6%" HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>

                                                                                    <%--CustomerName--%>
                                                                                    <asp:TemplateField HeaderText="Customer&#8217;s Customer Name" ItemStyle-Width="30%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblCustomerNameGEUC" Width="96%" Style="word-break: break-all;" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--Email Id--%>
                                                                                    <asp:TemplateField HeaderText="Email Id" ItemStyle-Width="20%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblEmailIdGEUC" Width="96%" runat="server" Style="word-break: break-all;" Text='<%# Bind("EmailId") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <%--Remarks--%>
                                                                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="30%">
                                                                                        <ItemTemplate>
                                                                                            <asp:TextBox ID="txtRemarksGEUC" Width="96%" ReadOnly="true" TextMode="MultiLine" Text='<%# Bind("Remarks") %>' Rows="3" runat="server"
                                                                                                onmouseover="txt_MouseoverTooltip(this)" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10%">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                                                                CausesValidation="false">
                                                                                            </asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="6">
                                                                            <asp:ValidationSummary ID="VSbtnAddEUC" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                                                ValidationGroup="btnAddEUC" ShowMessageBox="false" HeaderText="Correct the following validation(s):" />
                                                                            <asp:ValidationSummary ID="VSbtnModifyEUC" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                                                                                ValidationGroup="btnModifyEUC" ShowMessageBox="false" HeaderText="Correct the following validation(s):" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%-- Columns to be added for IRR Calculation Ends--%>
    <input id="hdnCTR" type="hidden" runat="server" />
    <input id="hdnPLR" type="hidden" runat="server" />
    <br />
    <asp:ValidationSummary ID="vs_TabMainPage" runat="server" CssClass="styleMandatoryLabel"
        Width="98%" ValidationGroup="Submit" HeaderText="Correct the following validation(s):  " />
    <asp:ValidationSummary ID="vs_AccountCreation" runat="server" CssClass="styleMandatoryLabel"
        Width="98%" ValidationGroup="AccountCreation" HeaderText="Correct the following validation(s):  " />
    <asp:ValidationSummary ID="vs_grvRentalDetails" runat="server" CssClass="styleMandatoryLabel"
        Width="98%" ValidationGroup="grvRentalDetails" HeaderText="Correct the following validation(s):  " />
    <asp:ValidationSummary ID="vsInflow" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Inflow" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:ValidationSummary ID="vsOutflow" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Outflow" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:ValidationSummary ID="vsRepayment" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Repayment" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsGuarantor" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Guarantor" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsAlert" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Alert" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:ValidationSummary ID="vsFollowUp" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="FollowUp" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsMoratorium" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="Moratorium" Width="98%" ShowMessageBox="false"
        HeaderText="Correct the following validation(s):  " ShowSummary="true" />
    <asp:ValidationSummary ID="vsAsset" runat="server" CssClass="styleMandatoryLabel"
        Enabled="true" ValidationGroup="tcAsset" Width="98%" ShowMessageBox="false" HeaderText="Correct the following validation(s):  "
        ShowSummary="true" />
    <asp:CustomValidator ID="cv_TabMainPage" runat="server" CssClass="styleMandatoryLabel"
        Display="None" ValidationGroup="Submit"></asp:CustomValidator>
    <%-- <asp:CustomValidator ID="CV_IRR" runat="server" CssClass="styleMandatoryLabel"
        Display="None" ValidationGroup="Repaymentt"></asp:CustomValidator>--%>

    <br />
    <div id="div20" style="overflow: hidden; text-align: right">
        <br />
        <div id="btndiv" style="overflow: hidden; text-align: center" width="98%">

            <asp:UpdatePanel ID="CS" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
                        OnClick="btnSave_OnClick" CausesValidation="True" OnClientClick="return fnCheckPageValidators('AccountCreation')" />
                    <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                        UseSubmitBehavior="False" Text="Clear" OnClick="btnClear_OnClick" />
                    <cc1:ConfirmButtonExtender ID="btnClearConfirmButtonExtender" runat="server" ConfirmText="Do you want to Clear the page?"
                        Enabled="True" TargetControlID="btnClear">
                    </cc1:ConfirmButtonExtender>
                    <asp:Button ID="btnCalcel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                        UseSubmitBehavior="False" Text="Cancel" OnClick="btnCancel_Click" />
                    <asp:Button ID="btnAcccountCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                        Visible="false" UseSubmitBehavior="False" Text="RS Cancel" OnClick="btnAcccountCancel_Click" />
                    <cc1:ConfirmButtonExtender ID="btnAcCancelConfirmButtonExtender" runat="server" ConfirmText="Do you want to cancel this Rental Schedule?"
                        Enabled="True" TargetControlID="btnAcccountCancel">
                    </cc1:ConfirmButtonExtender>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Button ID="btnGenerateRepay" runat="server" Style="display: none" Text="Button" ValidationGroup="Repaymentt"
                OnClick="btnGenerateRepay_Click" />
        </div>
        <input id="hdnCustomerId" type="hidden" runat="server" />
        <input id="hdnConstitutionId" type="hidden" runat="server" />
        <input id="hdnProductId" type="hidden" runat="server" />
        <br />

        <%--          </ContentTemplate>
    </asp:UpdatePanel>--%>

        <!-- unwanted code start(have to remove)-->


        <!-- unwanted code end(have to remove)-->
    </div>
</asp:Content>
