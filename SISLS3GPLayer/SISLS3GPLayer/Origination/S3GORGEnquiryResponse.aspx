<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GORGEnquiryResponse.aspx.cs"
    Inherits="Orgination_S3GORGEnquiryResponse" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    EnableEventValidation="false" %>

<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        var tab;
        var querymode;
        function pageLoad() {
            //                debugger
            tab = $find('ctl00_ContentPlaceHolder1_TabContainerER');
            querymode = location.search.split("qsMode=");
            querymode = querymode[1];
            if (querymode != 'Q') {
                tab.add_activeTabChanged(on_Change);
                var newindex = tab.get_activeTabIndex(index);
                var btnSave = document.getElementById('<%=btnSave.ClientID %>')
                var btnclear = document.getElementById('<%=btnClear.ClientID %>')
                if (newindex == tab._tabs.length - 1)
                    btnSave.disabled = false;
                else

                    btnSave.disabled = true;
                //                    if(querymode!='M')
                //                    {
                var ddlresponse = document.getElementById('<%=ddlResponse.ClientID %>');
                var responsevalue = ddlresponse[ddlresponse.selectedIndex].value;
                if (responsevalue == '192' || responsevalue == '191' || responsevalue == '190') {
                    btnSave.disabled = false;
                }
                else {
                    btnSave.disabled = true;
                }
                if (newindex == 5)
                    btnSave.disabled = false;
                if (querymode == 'M') {
                    if (newindex == 4) {
                        btnSave.disabled = false;
                    }
                }
                if (querymode != 'M') {
                    if (newindex == 0)
                        btnclear.disabled = true;
                    else
                        btnclear.disabled = false;
                }

                //                    }
            }
        }

        function ToggleResidual(Obj1, Obj2) {
            // debugger
            if (Obj1.value != "") {
                // Obj2.disabled = false;
                // Obj2.value = "";
                Obj2.setAttribute("readOnly", "readOnly");
            }
            else {
                // Obj2.disabled = true;
                //Obj2.value = "";
                Obj2.removeAttribute("readOnly");
            }

        }

        function SetData(ObJ, data) {

            if (document.getElementById(ObJ).value == "___.__") {

                document.getElementById(ObJ).value = data;
            }
        }


        function EnableDisableResidual(Obj1, Obj2) {

            if (document.getElementById(Obj1).value == "" && document.getElementById(Obj2).value == "") {

                document.getElementById(Obj1).disabled = false;
                document.getElementById(Obj2).disabled = false;
            }

            //            if (document.getElementById(Obj1).value != "__.____") {
            //                document.getElementById(Obj2).disabled = true;
            //            }
            //            else if (document.getElementById(Obj2).value != "") {
            //                document.getElementById(Obj1).disabled = true;
            //            }
            //            else {
            //                document.getElementById(Obj1).disabled = false;
            //                document.getElementById(Obj2).disabled = false;
            //            }
        }

        function EnableDisableMargine(Obj1, Obj2) {

            if (document.getElementById(Obj1).value != "__.____") {
                document.getElementById(Obj2).disabled = true;
            }
            else if (document.getElementById(Obj2).value != "") {
                document.getElementById(Obj1).disabled = true;
            }
            else {
                document.getElementById(Obj1).disabled = false;
                document.getElementById(Obj2).disabled = false;
            }
        }

        function ValidGuarantorAmt(ObJ1) {

            //var ObjJ = 'ctl00_ContentPlaceHolder1_TabContainerAP_TabInvoice_gvGuarantor_ctl03_txtGuaranteeamount_GuarantorTab_Footer';
            var amt = document.getElementById(ObJ1).value;
            //alert(amt);
            if (amt == 0 && document.getElementById(ObJ1).value != "") {
                document.getElementById(ObJ1).value = 1;
            }
        }


        function fnValidateEMail(ObJ1, ObJ2) {

            var email = document.getElementById(ObJ1).checked;
            var sms = document.getElementById(ObJ2).checked;

            if (email == false && sms == false) {
                alert('Select EMail or SMS');
                return false;
            }
            else
                return true;
        }

        function fnAllowNumbersOnlyZero(isSpaceAllowed, Obj1) {
            var sKeyCode = window.event.keyCode;
            var anyval = document.getElementById(Obj1).value;

            //alert(sKeyCode);
            if ((!isSpaceAllowed) && (sKeyCode == 32)) {
                window.event.keyCode = 0;
                return false;
            }
            if ((sKeyCode < 48 || sKeyCode > 57) && (sKeyCode != 32 && sKeyCode != 95) || (sKeyCode == 48)) {
                if (anyval == 0) {
                    window.event.keyCode = 0;
                    return false;
                }
            }
        }

        function onNavigate(isMoveNext) {
            var tabs = $find('ctl00_ContentPlaceHolder1_TabContainerER');
            var totalNumberoftabs = tabs.get_tabs().length;
            if (totalNumberoftabs > 0) {
                var newTabIndex;
                var currentTabIndex = tabs.get_activeTabIndex();
                if (isMoveNext) {
                    if (currentTabIndex + 1 == totalNumberoftabs) {
                        newTabIndex = 0;
                    }
                    else {
                        newTabIndex = currentTabIndex + 1;
                    }
                }
                else {
                    if (currentTabIndex - 1 < 0) {
                        newTabIndex = totalNumberoftabs - 1;
                    }
                    else {
                        newTabIndex = currentTabIndex - 1;
                    }
                }
                tabs.set_activeTabIndex(newTabIndex);
            }
        }


        var index;
        querymode = location.search.split("qsMode=");
        querymode = querymode[1];
        if (querymode == "C") {
            index = 1;
        }
        if (querymode == "M") {
            index = 0;
        }
        if (querymode == "W") {
            index = 1;
        }


        function FunGetSelectedLob() {
            var ddlLob = document.getElementById('<%=ddlLOBAssign.ClientID %>');
            var txtLob = document.getElementById('<%=txtLOB.ClientID %>');
            var lobcode;
            if (ddlLob.selectedIndex > 0) {
                lobcode = ddlLob.item(ddlLob.selectedIndex).text.split('-')[0].trim();
            }
            else {
                lobcode = txtLob.value.split('-')[0].trim();
            }
            return lobcode;
        }

        function on_Change(sender, e) {
            //tab = $find('ctl00_ContentPlaceHolder1_tcPricing');
            //debugger;
            var lobcode = FunGetSelectedLob();
            var status = document.getElementById('<%=lblEnqStatus.ClientID %>');
            var strValgrp = tab._tabs[index]._tab.outerText.trim();
            var Valgrp = document.getElementById('<%=vs_Main.ClientID %>');
            Valgrp.validationGroup = strValgrp;
            var newindex = tab.get_activeTabIndex(index);
            var btnSave = document.getElementById('<%=btnSave.ClientID %>');
            var btnclear = document.getElementById('<%=btnClear.ClientID %>');
            var btnCalIRR = document.getElementById('<%=btnCalIRR.ClientID %>');
            var btnReset = document.getElementById('<%=btnReset.ClientID %>');

            var EditStatus = document.getElementById('<%=hdnEdit_Status.ClientID %>');

            if (newindex == tab._tabs.length - 1)
                btnSave.disabled = false;
            else
                btnSave.disabled = true;
            if (querymode == "C" || querymode == "W") {
                if (newindex == 0) {
                    btnclear.disabled = true;

                }
                else {
                    btnclear.disabled = false;

                }
            }
            if (querymode == "M") {

                var ddlresponse = document.getElementById('ctl00_ContentPlaceHolder1_TabContainerER_TabReponse_ddlResponse');
                var responsevalue = ddlresponse[ddlresponse.selectedIndex].value;
                if (responsevalue == '192' || responsevalue == '191' || responsevalue == '190') {
                    btnSave.disabled = false;
                }
                else {
                    btnSave.disabled = true;
                }
                if (newindex == 4) {
                    btnSave.disabled = false;
                }
            }
            if (newindex > index) {
                if (!fnCheckPageValidators(strValgrp, false)) {

                    tab.set_activeTabIndex(index);

                }
                else {
                    if (querymode == "C" || querymode == "W") {
                        switch (index) {
                            case 1:
                                if (FunchangeTofield()) {
                                    index = tab.get_activeTabIndex(index);
                                }
                                else {
                                    tab.set_activeTabIndex(index);
                                    alert('Both existing fields and change to fields are same');
                                }

                                break;

                            case 2:
                                if (lobcode != null && lobcode.toLowerCase() == 'ol') {
                                    if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                        document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                    }
                                    index = tab.get_activeTabIndex(index);
                                }
                                else {

                                    if (FunCheckGridIsEmpty('<%=gvOutFlow.ClientID %>', 'Yes')) {
                                        if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                            document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                        }
                                        index = tab.get_activeTabIndex(index);

                                    }
                                    else {
                                        tab.set_activeTabIndex(index);
                                        alert('Add atleast one Outflow details');

                                    }
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
                    else if (querymode == "M") {
                        switch (index) {

                            case 0:
                                //                            if (status.value != "194" && status.value != "199" && status.value != "198") {
                                if (FunchangeTofield()) {
                                    index = tab.get_activeTabIndex(index);
                                }
                                else {
                                    tab.set_activeTabIndex(index);
                                    alert('Both existing fields and change to fields should not be same');
                                }
                                //                            }
                                break;

                            case 1:
                                //                            if (status.value != "194" && status.value != "199" && status.value != "198") 
                                //                            {

                                if (lobcode != null && lobcode.toLowerCase() == 'ol') {
                                    if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                        document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                    }
                                    index = tab.get_activeTabIndex(index);
                                }
                                else {
                                    if (EditStatus.value == "1")//Added by saran for modification restricted Condition on 28-Dec-2011.
                                    {
                                        index = tab.get_activeTabIndex(index);
                                    }
                                    else {
                                        if (FunCheckGridIsEmpty('<%=gvOutFlow.ClientID %>', 'Yes')) {
                                            if (document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value == "") {
                                                document.getElementById('<%=btnGenerateRepay.ClientID %>').click();
                                            }
                                            index = tab.get_activeTabIndex(index);

                                        }
                                        else {
                                            tab.set_activeTabIndex(index);
                                            alert('Add atleast one Outflow details');
                                        }
                                    }
                                }
                                //                            }

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
            }
            else {
                tab.set_activeTabIndex(newindex);
                index = tab.get_activeTabIndex(newindex);
            }

        }

        function FunchangeTofield() {
            var re = /((\s*\S+)*)\s*/;
            
                return true;
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

        function FunChkAllFooterValues(gridid) {


            //Get all the control of the type INPUT in the base control.
            var Combos = gridid.getElementsByTagName("select");
            for (var n = 0; n < Combos.length; ++n) {
                if (Combos[n].innerText == "") {
                    return false;
                }
            }
        }


        function funvalidfinanceamount(financeamount) {
            if (financeamount.value == "0" || financeamount.value == "00" || financeamount.value == "000" || financeamount.value == "0000" || financeamount.value == "00000" || financeamount.value == "000000" || financeamount.value == "0000000" || financeamount.value == "00000000" || financeamount.value == "000000000" || financeamount.value == "0000000000") {
                alert('This field should not be 0');
                document.getElementById(financeamount.id).focus();
            }
        }

        function funvalidateamount(inputs) {
            var inputamount = inputs.value;
            var financeamount = document.getElementById('<%=txtFinanceAmount.ClientID%>').value;
            if (parseInt(inputamount) > parseInt(financeamount)) {
                alert('Amount should not be greater than Finance amount sought');
                document.getElementById(inputs.id).focus();
            }
            else if (inputs.value == "0" || inputs.value == "00" || inputs.value == "000" || inputs.value == "0000" || inputs.value == "00000") {
                alert('Amount should not be 0');
                document.getElementById(inputs.id).focus();
            }
        }

        function chageIndex() {
            var tabs = $find('ctl00_ContentPlaceHolder1_TabContainerER');
            tabs.set_activeTabIndex(1);
        }

        function Tabmanagement() {
            //debugger
            var ddlresponse = document.getElementById('<%=ddlResponse.ClientID %>');
            var responsevalue = ddlresponse[ddlresponse.selectedIndex].value;
            var tabs = $find('ctl00_ContentPlaceHolder1_TabContainerER');
            var totalNumberoftabs = tabs.get_tabs().length;
            if (responsevalue == '192' || responsevalue == '191' || responsevalue == '190') {
                var newtabno = 1;
                if (querymode == 'M')
                    newtabno = 0;
                for (var tabno = 0; tabno < totalNumberoftabs; tabno++) {

                    if (tabno == newtabno)
                        tabs.get_tabs()[tabno].set_enabled(true);
                    else
                        tabs.get_tabs()[tabno].set_enabled(false);

                }

            }
            else {
                for (var tabno = 0; tabno < totalNumberoftabs; tabno++) {
                    tabs.get_tabs()[tabno].set_enabled(true);
                }
            }

        }


        function FunddlRoiOnChange(varRoiPayment) {
            var Prevval;
            var Dropdown;
            if (varRoiPayment == 'Payment') {
                Prevval = document.getElementById('<%=hdnPayment.ClientID %>').value;
                Dropdown = document.getElementById('<%=ddlPaymentRuleList.ClientID %>')
            }
            else {
                Prevval = document.getElementById('<%=hdnROIRule.ClientID %>').value;
                Dropdown = document.getElementById('<%=ddlROIRuleList.ClientID %>')
            }
            if (Prevval != "") {
                if (confirm('All cashflow related data will be reset, Are you sure want to continue?')) {
                    // __doPostBack('ddlROIRuleList','');
                    return true;
                }
                else {
                    //Dropdown.selectedIndex = Prevval;
                    Dropdown.value = Prevval;
                    return false;
                }
            }
            else {
                //__doPostBack('ddlROIRuleList','');
                return true;
            }
        }


        function hideModalPopupViaClient() {
            //ev.preventDefault();        
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }


        function FunRestIrr() {
            //document.getElementById('<%=txtCompanyIRR_Repay.ClientID %>').value = "";
            document.getElementById('<%=txtBusinessIRR_Repay.ClientID %>').value = "";

        }
    
    </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <table width="100%" class="stylePageHeading" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Enquiry Response" ID="lblHeading" CssClass="styleDisplayLabel">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>--%>
                <table width="100%" align="center" cellpadding="0" cellspacing="0" border="1">
                    <tr>
                        <td valign="top">
                            <cc1:TabContainer ID="TabContainerER" runat="server" CssClass="styleTabPanel" Width="100%"
                                ScrollBars="None" ActiveTabIndex="4">
                                <cc1:TabPanel runat="server" ID="TabMode" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Assigned Enquires
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                            <ContentTemplate>
                                                <br />
                                                <input id="hdnCTR" type="hidden" runat="server" /><input id="hdnPLR" type="hidden"
                                                    runat="server" /><input id="hdnTenure" type="hidden" runat="server" /><input id="hdnRoundOff"
                                                        type="hidden" runat="server" />
                                                <input type="hidden" id="hdnSortDirection" runat="server" />
                                                <input type="hidden" id="hdnSortExpression" runat="server" />
                                                <input type="hidden" id="hdnSearch" runat="server" />
                                                <input type="hidden" id="hdnOrderBy" runat="server" />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr align="center">
                                                        <td colspan="1">
                                                            <asp:GridView ID="grvEnquiryUpdation" runat="server" AutoGenerateColumns="False"
                                                                OnRowEditing="grvEnquiryUpdation_RowEditing" Width="98%" OnRowDataBound="grvEnquiryUpdation_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%#Bind("ID")%>'> </asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Sl.No." ItemStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkSerialNo" runat="server" Text='<%#Bind("SLNo")%>' CausesValidation="false"
                                                                                OnClientClick="chageIndex();" CommandName="Edit"> </asp:LinkButton></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Line of Business" ToolTip="Sort By Line of Business"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLineOfBusiness" runat="server" Text='<%#Bind("LineOfBusiness")%>'> </asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Branch" ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Location" ToolTip="Sort By Location"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <%-- <asp:Label ID="lblBranch" runat="server" Text='<%#Bind("Branch")%>'> </asp:Label></ItemTemplate>--%>
                                                                            <asp:Label ID="lblBranch" runat="server" Text='<%#Bind("Location")%>'> </asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Product" ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort3" runat="server" Text="Product" ToolTip="Sort By Product"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort3" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch3" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductCode" runat="server" Text='<%#Bind("ProductCode")%>'> </asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Enquiry Number" ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort4" runat="server" Text="Enquiry Number" ToolTip="Sort By Enquiry Number"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort4" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch4" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEnquiryNumber" runat="server" Text='<%#Bind("EnquiryNumber")%>'> </asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Enquiry Date" ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort5" runat="server" Text="Enquiry Date" ToolTip="Sort By Enquiry Date"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort5" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch5" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEnquiryDate" runat="server" Text='<%#Bind("EnquiryDate")%>'> </asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Customer" ItemStyle-HorizontalAlign="Left">
                                                                        <HeaderTemplate>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr align="center">
                                                                                    <td>
                                                                                        <asp:LinkButton ID="lnkbtnSort6" runat="server" Text="Customer" ToolTip="Sort By Customer"
                                                                                            CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                                        <asp:ImageButton ID="imgbtnSort6" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr align="left">
                                                                                    <td>
                                                                                        <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch6" runat="server" AutoPostBack="true"
                                                                                            OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomerName" runat="server" Text='<%#Bind("CustomerName")%>'> </asp:Label></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--<asp:BoundField DataField="EnquiryNumber" HeaderText="Enquiry Number" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="EnquiryDate" HeaderText="Enquiry Date" />
                                                                        <asp:BoundField DataField="LineOfBusiness" HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="Branch" HeaderText="Branch" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="ProductCode" HeaderText="Product" ItemStyle-HorizontalAlign="Left" />
                                                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ItemStyle-HorizontalAlign="Left" />--%>
                                                                    <asp:TemplateField HeaderText="Tenure" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEnquiryTenure" runat="server" Text='<%#Bind("Tenure") %>' /></ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="Mode" HeaderText="Mode" Visible="false" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td>
                                                            <uc2:PageNavigator ID="ucCustomPaging" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:CustomValidator ID="CV_Qry" runat="server" CssClass="styleMandatoryLabel" Display="None"
                                                                ValidationGroup="TabMode"></asp:CustomValidator><br />
                                                            <div id="divId" style="overflow: None; height: 100%; width: 50%">
                                                                <asp:ValidationSummary ID="vsEnquiryAssignment" runat="server" CssClass="styleMandatoryLabel"
                                                                    Width="80%" ValidationGroup="TabMode" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabReponse" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Response Details
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel_Tap" runat="server" GroupingText="Enquiry Information" CssClass="stylePanel">
                                                    <table width="100%" align="center" border="0" cellspacing="0">
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblLOB" runat="server" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtLOB" runat="server" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblEnquiryno" runat="server" CssClass="styleDisplayLabel" Text="Enquiry number"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtEnquiryNo" runat="server" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblBranch" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtBranch" runat="server" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblEnquiryDate" runat="server" CssClass="styleDisplayLabel" Text="Enquiry Date"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtEnquiryDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                </asp:Panel>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="50%">
                                                            <asp:Panel ID="Panel_Address" runat="server" GroupingText="Customer details" CssClass="stylePanel">
                                                                <table width="100%" align="center" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 30%">
                                                                            <asp:Label ID="lblCustomerCode" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 70%">
                                                                            <asp:TextBox ID="txtCustomerCode" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 30%">
                                                                            <asp:Label ID="lblCustomerName" runat="server" CssClass="styleDisplayLabel" Text="Customer Name"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 70%">
                                                                            <asp:TextBox ID="txtCustomerName" runat="server" ReadOnly="True" Width="95%"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                ShowCustomerCode="false" ShowCustomerName="false" FirstColumnWidth="32%" SecondColumnWidth="70%" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                            </asp:Panel>
                                                        </td>
                                                        <td width="50%">
                                                            <asp:Panel ID="Panel_Responded" runat="server" GroupingText="Responded details" CssClass="stylePanel">
                                                                <table width="100%" align="center" border="0" cellspacing="0">
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 12%">
                                                                            <asp:Label ID="lblStatus" runat="server" CssClass="styleDisplayLabel" Text="Status"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 35%">
                                                                            <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 12%">
                                                                            <asp:Label ID="lblRespondedBy" runat="server" CssClass="styleDisplayLabel" Text="Responded By"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtRespondedBy" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 12%">
                                                                            <asp:Label ID="lblRespondedDate" runat="server" CssClass="styleDisplayLabel" Text="Responded Date"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtRespondedDate" runat="server" ReadOnly="True" Width="90px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 12%">
                                                                            <asp:Label ID="lblMobileNo" runat="server" CssClass="styleDisplayLabel" Text="Mobile Number"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtMobileNo" runat="server" ReadOnly="True" Width="90px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel" style="width: 12%">
                                                                            <asp:Label ID="lblEmailId" runat="server" CssClass="styleDisplayLabel" Text="EMail Id"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 25%">
                                                                            <asp:TextBox ID="txtEmailID" runat="server" ReadOnly="True"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 72px">
                                                                        </td>
                                                                        <td style="height: 72px">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel ID="Panel_Finance" runat="server" GroupingText="Finance details" CssClass="stylePanel">
                                                    <table width="100%" align="center" border="0" cellspacing="0">
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblAssetDescription" runat="server" CssClass="styleDisplayLabel" Text="Asset Description"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtAssetDescription" runat="server" ReadOnly="True" Width="250px"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblAssetValue" runat="server" CssClass="styleDisplayLabel" Text="Asset Value"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtAssetValue" runat="server" ReadOnly="True" Style="text-align: right"
                                                                    Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblFinanceAmount" runat="server" CssClass="styleDisplayLabel" Text="Finance Amount Sought"></asp:Label><span
                                                                    class="styleMandatory">*</span>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtFinanceAmount" runat="server" MaxLength="10" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                                    AutoPostBack="true" onkeyup="fnNotAllowPasteSpecialChar(this,'special')" Style="text-align: right"
                                                                    OnTextChanged="txtFinanceAmount_OnTextChanged" Width="80px"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvApplicationNo" runat="server" ControlToValidate="txtFinanceAmount"
                                                                    ValidationGroup="Response Details" CssClass="styleMandatoryLabel" Display="None"
                                                                    SetFocusOnError="True" ErrorMessage="Enter the Finance Amount"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblResidualvalue" runat="server" CssClass="styleDisplayLabel" Text="Residual Value"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtResidualvalue" runat="server" MaxLength="10" onkeypress="fnAllowNumbersOnly(false,false,this);"
                                                                    onkeyup="fnNotAllowPasteSpecialChar(this,'special')" Style="text-align: right"
                                                                    Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblTenure" runat="server" CssClass="styleDisplayLabel" Text="Tenure"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtTenure" runat="server" ReadOnly="True" Width="80px" Style="text-align: right"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblTenureType" runat="server" CssClass="styleDisplayLabel" Text="Tenure Type"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtTenureType" runat="server" ReadOnly="True" Style="text-align: left"
                                                                    Width="80px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                </asp:Panel>
                                                <asp:Panel ID="Panel_Assignment" runat="server" GroupingText="Assignment details"
                                                    CssClass="stylePanel">
                                                    <table width="100%" align="center" border="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td class="styleFieldLabel" align="center">
                                                                <asp:Label ID="Label4" runat="server" CssClass="stylePanel" Text="Existing"></asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td class="styleFieldAlign" align="center">
                                                                <asp:Label ID="Label2" runat="server" CssClass="stylePanel" Text="Change To"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblLOBAssign" runat="server" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtLOBAssign" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="Label5" runat="server" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:DropDownList ID="ddlLOBAssign" Width="200px" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlLOBAssign_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblBranchAssign" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtBranchAssign" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="Label6" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:DropDownList ID="ddlBranchAssign" Width="200px" runat="server" AutoPostBack="true" 
                                                                       OnSelectedIndexChanged="ddlBranchAssign_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblProductAssign" runat="server" CssClass="styleDisplayLabel" Text="Product"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtProductAssign" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="Label7" runat="server" CssClass="styleDisplayLabel" Text="Product"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:DropDownList ID="ddlProductAssign" Width="200px" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlProductAssign_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblworkflowSequence" runat="server" CssClass="styleDisplayLabel" Text="Workflow Sequence"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtworkflowSequence" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="Label8" runat="server" CssClass="styleDisplayLabel" Text="Workflow<br>Sequence"></asp:Label>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:TextBox ID="txtworkflowSequence_Change" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                            </td>
                                                            <td class="styleFieldAlign">
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 12%">
                                                                <asp:Label ID="lblResponseDetails" runat="server" CssClass="styleDisplayLabel" Text="Response Details"></asp:Label><span
                                                                    class="styleMandatory">*</span>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 25%">
                                                                <asp:DropDownList ID="ddlResponse" Width="200px" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlResponse_SelectedIndexChanged" onchange="Tabmanagement();">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvResponse" runat="server" ControlToValidate="ddlResponse"
                                                                    ValidationGroup="Response Details" CssClass="styleMandatoryLabel" Display="None"
                                                                    SetFocusOnError="True" InitialValue="0" ErrorMessage="Select a Response details"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                </asp:Panel>
                                                <%--<Trigger>
                                                <asp:AsyncPostBackTrigger ControlID="ddlResponse" EventName="SelectedIndexChanged"/>
                                        </Triggers>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabOfferTerms" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Offer Terms
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="divRoiRules" runat="server" CssClass="accordionHeader" Width="98.5%">
                                                    <div style="float: left;">
                                                        ROI/Payment Rule Card details</div>
                                                    <div style="float: left; margin-left: 20px;">
                                                        <asp:Label ID="lblDetails" runat="server">(Show Details...)</asp:Label>
                                                    </div>
                                                    <div style="float: right; vertical-align: middle;">
                                                        <asp:ImageButton ID="imgDetails" runat="server" ImageUrl="~/Images/expand_blue.jpg"
                                                            AlternateText="(Show Details...)" />
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel ID="Panel6" runat="server" GroupingText="ROI/Payment Rule Card details"
                                                    CssClass="stylePanel">
                                                    <table cellpadding="0" cellspacing="0" style="width: 95%">
                                                        <tr>
                                                            <td class="styleFieldLabel" style="width: 14%">
                                                                <asp:Label ID="lblROIRuleList" runat="server" CssClass="styleDisplayLabel" Text="ROI Rule"></asp:Label><span
                                                                    class="styleMandatory">*</span>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 40%">
                                                                <asp:DropDownList ID="ddlROIRuleList" runat="server" Width="80%">
                                                                </asp:DropDownList>
                                                                <asp:Button ID="btnFetchROI" Text="Go" runat="server" CssClass="styleSubmitShortButton"
                                                                    OnClick="btnFetchROI_Click" OnClientClick="return FunddlRoiOnChange('ROI')" />
                                                                <asp:RequiredFieldValidator ID="rfvddlROIRuleList" runat="server" ControlToValidate="ddlROIRuleList"
                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                                    ErrorMessage="Select a ROI rule from the list" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                <asp:HiddenField ID="hdnROIRule" runat="server" />
                                                            </td>
                                                            <td class="styleFieldLabel" style="width: 14%">
                                                                <asp:Label ID="lblPaymentRuleList" runat="server" CssClass="styleDisplayLabel" Text="Payment Rule"></asp:Label><span
                                                                    class="styleMandatory">*</span>
                                                            </td>
                                                            <td class="styleFieldAlign" style="width: 40%">
                                                                <asp:DropDownList ID="ddlPaymentRuleList" runat="server" Width="70%">
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="hdnPayment" runat="server" />
                                                                <asp:Button ID="btnFetchPayment" runat="server" CssClass="styleSubmitShortButton"
                                                                    OnClick="btnFetchPayment_Click" Text="Go" OnClientClick="return FunddlRoiOnChange('Payment')" />
                                                                <asp:RequiredFieldValidator ID="rfvddlPaymentRuleList" runat="server" ControlToValidate="ddlPaymentRuleList"
                                                                    CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                                    ErrorMessage="Select a Payment rule from the list" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <div id="divROIRuleInfo" style="overflow: visible;" runat="server">
                                                        <asp:Panel runat="server" ID="Panel3" ScrollBars="Auto" CssClass="stylePanel" GroupingText="ROI Rule"
                                                            Width="99%">
                                                            <table width="95%" border="1">
                                                                <tr>
                                                                    <td width="50%" valign="top">
                                                                        <div id="div7" style="overflow: visible;" runat="server">
                                                                            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                                                <tr>
                                                                                    <th>
                                                                                        Field Name
                                                                                    </th>
                                                                                    <th>
                                                                                        Field Value
                                                                                    </th>
                                                                                    <tr id="tr1" runat="server">
                                                                                        <td id="Td1" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblSerial_Number" runat="server" CssClass="styleDisplayLabel" Text="Serial Number"
                                                                                                Visible="false"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td2" runat="server" class="styleFieldAlign">
                                                                                            <asp:TextBox ID="txt_Serial_Number" runat="server" ReadOnly="True" Style="width: 145px"
                                                                                                Visible="false"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr4" runat="server">
                                                                                        <td id="Td7" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblROI_Rule_Number" runat="server" CssClass="styleDisplayLabel" Text="ROI Rule Number"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td8" runat="server" class="styleFieldAlign">
                                                                                            <asp:TextBox ID="txt_ROI_Rule_Number" runat="server" ReadOnly="True" Style="width: 70px">
                                                                                            </asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr3" runat="server">
                                                                                        <td id="Td5" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblRate_Type" runat="server" CssClass="styleDisplayLabel" Text="Rate Type"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td6" runat="server" class="styleFieldAlign">
                                                                                            <asp:DropDownList ID="ddl_Rate_Type" runat="server" Width="150px">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr2" runat="server">
                                                                                        <td id="Td3" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblModel_Description" runat="server" CssClass="styleDisplayLabel"
                                                                                                Text="Model Description"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td4" runat="server" class="styleFieldAlign">
                                                                                            <asp:TextBox ID="txt_Model_Description" runat="server" ReadOnly="True" Style="width: 185px"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr5" runat="server">
                                                                                        <td id="Td9" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblReturn_Pattern" runat="server" CssClass="styleDisplayLabel" Text="Return Pattern"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td10" runat="server" class="styleFieldAlign">
                                                                                            <asp:DropDownList ID="ddl_Return_Pattern" runat="server" Width="150px">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr6" runat="server">
                                                                                        <td id="Td11" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblTime_Value" runat="server" CssClass="styleDisplayLabel" Text="Time Value"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td12" runat="server" class="styleFieldAlign">
                                                                                            <asp:DropDownList ID="ddl_Time_Value" runat="server" onchange="FunRestIrr();" Width="150px">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvTimeValue" runat="server" ControlToValidate="ddl_Time_Value"
                                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="-1"
                                                                                                ErrorMessage="Select a time Value from the list" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr7" runat="server">
                                                                                        <td id="Td13" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblFrequency" runat="server" CssClass="styleDisplayLabel" Text="Frequency"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td14" runat="server" class="styleFieldAlign">
                                                                                            <asp:DropDownList ID="ddl_Frequency" runat="server" Width="150px" onchange="FunRestIrr();">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ControlToValidate="ddl_Frequency"
                                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="-1"
                                                                                                ErrorMessage="Select a frequency from the list" ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr8" runat="server">
                                                                                        <td id="Td15" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblRepayment_Mode" runat="server" CssClass="styleDisplayLabel" Text="Repayment Mode"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td16" runat="server" class="styleFieldAlign">
                                                                                            <asp:DropDownList ID="ddl_Repayment_Mode" runat="server" Width="150px">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr9" runat="server">
                                                                                        <td id="Td17" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblRate" runat="server" CssClass="styleDisplayLabel" Text="Rate/Quote">
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td id="Td18" runat="server" class="styleFieldAlign">
                                                                                            <asp:TextBox ID="txt_Rate" runat="server" Width="70px" MaxLength="10" onchange="FunRestIrr();"
                                                                                                Style="text-align: right">
                                                                                            </asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="rfvRate" runat="server" ControlToValidate="txt_Rate"
                                                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter the rate"
                                                                                                ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="tr18" runat="server">
                                                                                        <td id="Td35" runat="server" class="styleFieldLabel">
                                                                                            <asp:Label ID="lblResidual_Value" runat="server" CssClass="styleDisplayLabel" Text="Residual Value"></asp:Label>
                                                                                        </td>
                                                                                        <td id="Td36" runat="server" class="styleFieldAlign">
                                                                                            <asp:DropDownList ID="ddl_Residual_Value" runat="server" Width="150px">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                                                            <tr>
                                                                                <th>
                                                                                    Field Name
                                                                                </th>
                                                                                <th>
                                                                                    Field Value
                                                                                </th>
                                                                                <tr id="tr10" runat="server">
                                                                                    <td id="Td19" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblIRR_Rest" runat="server" CssClass="styleDisplayLabel" Text="IRR Rest"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td20" runat="server" class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddl_IRR_Rest" runat="server" Width="150px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr11" runat="server">
                                                                                    <td id="Td21" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblInterest_Calculation" runat="server" CssClass="styleDisplayLabel"
                                                                                            Text="Interest Calculation"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td22" runat="server" class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddl_Interest_Calculation" runat="server" Width="150px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr12" runat="server">
                                                                                    <td id="Td23" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblInterest_Levy" runat="server" CssClass="styleDisplayLabel" Text="Interest Levy"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td24" runat="server" class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddl_Interest_Levy" runat="server" Width="150px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr13" runat="server">
                                                                                    <td id="Td25" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblRecovery_Pattern_Year1" runat="server" CssClass="styleDisplayLabel"
                                                                                            Text="Recovery Pattern Year1"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td26" runat="server" class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txt_Recovery_Pattern_Year1" runat="server" Style="width: 70px; text-align: right">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr14" runat="server">
                                                                                    <td id="Td27" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblRecovery_Pattern_Year2" runat="server" CssClass="styleDisplayLabel"
                                                                                            Text="Recovery Pattern Year2"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td28" runat="server" class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txt_Recovery_Pattern_Year2" runat="server" Style="width: 70px; text-align: right">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr15" runat="server">
                                                                                    <td id="Td29" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblRecovery_Pattern_Year3" runat="server" CssClass="styleDisplayLabel"
                                                                                            Text="Recovery Pattern Year3"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td30" runat="server" class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txt_Recovery_Pattern_Year3" runat="server" Style="width: 70px; text-align: right">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr16" runat="server">
                                                                                    <td id="Td31" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblRecovery_Pattern_Rest" runat="server" CssClass="styleDisplayLabel"
                                                                                            Text="Recovery Pattern Rest"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td32" runat="server" class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txt_Recovery_Pattern_Rest" runat="server" Style="width: 70px; text-align: right">
                                                                                        </asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr17" runat="server">
                                                                                    <td id="Td33" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblInsurance" runat="server" CssClass="styleDisplayLabel" Text="Insurance"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td34" runat="server" class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddl_Insurance" runat="server" Width="150px" onchange="FunRestIrr();">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr19" runat="server">
                                                                                    <td id="Td37" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblMargin" runat="server" CssClass="styleDisplayLabel" Text="Margin"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td38" runat="server" class="styleFieldAlign">
                                                                                        <asp:DropDownList ID="ddl_Margin" runat="server" Width="150px">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr id="tr20" runat="server">
                                                                                    <td id="Td39" runat="server" class="styleFieldLabel">
                                                                                        <asp:Label ID="lblMargin_Percentage" runat="server" CssClass="styleDisplayLabel"
                                                                                            Text="Margin Percentage"></asp:Label>
                                                                                    </td>
                                                                                    <td id="Td40" runat="server" class="styleFieldAlign">
                                                                                        <asp:TextBox ID="txt_Margin_Percentage" MaxLength="7" runat="server" Style="width: 145px;
                                                                                            text-align: right" AutoPostBack="true" OnTextChanged="txt_Margin_Percentage_OnTextChanged"
                                                                                            onchange="FunRestIrr();">
                                                                                        </asp:TextBox>
                                                                                        <asp:RequiredFieldValidator ID="rfvMarginPercent" runat="server" ControlToValidate="txt_Margin_Percentage"
                                                                                            CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter the Margin Percentage in ROI Rule grid"
                                                                                            ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                    </td>
                                                                                </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </div>
                                                    <div id="div8" style="overflow: auto; width: 100%" runat="server">
                                                        <asp:Panel runat="server" ID="Panel4" ScrollBars="Auto" CssClass="stylePanel" GroupingText="Payment Rule"
                                                            Width="99%">
                                                            <asp:GridView ID="gvPaymentRuleDetails" runat="server" Width="100%">
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </div>
                                                </asp:Panel>
                                                <cc1:CollapsiblePanelExtender ID="cpeDemo" runat="Server" TargetControlID="Panel6"
                                                    ExpandControlID="divRoiRules" CollapseControlID="divRoiRules" Collapsed="True"
                                                    TextLabelID="lblDetails" ImageControlID="imgDetails" ExpandedText="(Hide Details...)"
                                                    CollapsedText="(Show Details...)" ExpandedImage="~/Images/collapse_blue.jpg"
                                                    CollapsedImage="~/Images/expand_blue.jpg" SuppressPostBack="true" SkinID="CollapsiblePanelDemo" />
                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlResidualAmount" runat="server" CssClass="stylePanel" Width="96%">
                                                            <table cellpadding="0" cellspacing="0" border="0" width="95%">
                                                                <tr>
                                                                    <td colspan="4" valign="bottom">
                                                                        <table style="height: inherit">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblResidualAmt_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                        Text="Residual Amount"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtResidualAmt_Cashflow" runat="server" Width="100px" Style="text-align: right"
                                                                                        MaxLength="7" AutoPostBack="true" OnTextChanged="txtResidualAmt_Cashflow_TextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftxtResidualAmount" runat="server" TargetControlID="txtResidualAmt_Cashflow"
                                                                                        FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblResidualValue_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                        Text="%"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtResidualValue_Cashflow" runat="server" Style="text-align: right"
                                                                                        MaxLength="7" Width="40px" OnTextChanged="txtResidualValue_Cashflow_TextChanged"></asp:TextBox>
                                                                                    <cc1:FilteredTextBoxExtender ID="ftxtResidualPercentage" runat="server" TargetControlID="txtResidualValue_Cashflow"
                                                                                        FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                                    <%-- <cc1:MaskedEditExtender ID="MasktxtResidualValue_Cashflow" runat="server" TargetControlID="txtResidualValue_Cashflow"
                                                            Mask="99.9999" MaskType="Number" Enabled="True" InputDirection="RightToLeft"
                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="">
                                                        </cc1:MaskedEditExtender>--%>
                                                                                    <asp:RequiredFieldValidator ID="RFVresidualvalue" runat="server" ControlToValidate="txtResidualValue_Cashflow"
                                                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ErrorMessage="Enter the Residual Value % Or Amount"
                                                                                        ValidationGroup="Offer Terms"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblMarginMoneyAmount_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                        Text="Margin Amount"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtMarginMoneyAmount_Cashflow" runat="server" Width="100px" onkeyup="fnNotAllowPasteSpecialChar(this,'special')"
                                                                                        onkeypress="fnAllowNumbersOnly(false,false,this)" MaxLength="7" Style="text-align: right"></asp:TextBox>
                                                                                </td>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblMarginMoneyPer_Cashflow" runat="server" CssClass="styleDisplayLabel"
                                                                                        Text="%"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtMarginMoneyPer_Cashflow" MaxLength="7" runat="server" Width="40px"
                                                                                        onkeypress="fnAllowNumbersOnly(false,true,this)" Style="text-align: right"></asp:TextBox>
                                                                                    <%--<cc1:MaskedEditExtender ID="MasktxtMarginMoneyPer_Cashflow" runat="server" TargetControlID="txtMarginMoneyPer_Cashflow"
                                                            Mask="99.9999" MaskType="Number" Enabled="True" InputDirection="RightToLeft"
                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                            CultureTimePlaceholder="">
                                                        </cc1:MaskedEditExtender>--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <asp:Panel ID="panInflow" runat="server" ScrollBars="Auto" CssClass="stylePanel"
                                                    GroupingText="Cash Inflow details">
                                                    <div style="overflow: auto; width: 940px; height: 125px; padding-left: 10px;">
                                                        <asp:GridView ID="gvInflow" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvInflow_RowDeleting"
                                                            ShowFooter="True" OnRowCreated="gvInflow_RowCreated" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDate_GridInflow" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtDate_GridInflow" runat="server" Width="100px"> </asp:TextBox>
                                                                        <cc1:CalendarExtender ID="CalendarExtenderSD_InflowDate" runat="server" Enabled="True"
                                                                            TargetControlID="txtDate_GridInflow" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtDate_GridInflow" runat="server" ControlToValidate="txtDate_GridInflow"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabInflow" ErrorMessage="Enter the Date in Inflow"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cash flow Id" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInflowid" runat="server" Text='<%# Bind("CashInFlowID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cash flow Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInflowDesc" runat="server" Text='<%# Bind("CashInFlow") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlInflowDesc" runat="server" Width="200px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlInflowDesc" runat="server" ControlToValidate="ddlInflowDesc"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabInflow" SetFocusOnError="True"
                                                                            InitialValue="-1" ErrorMessage="Select a Cash Inflow"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="In flow from">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInflowFrom" runat="server" Text='<%# Bind("InflowFrom") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlEntityName_InFlowFrom" runat="server" Width="200px" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="ddlEntityName_InFlowFrom_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlEntityName_InFlowFrom" runat="server" ControlToValidate="ddlEntityName_InFlowFrom"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabInflow" SetFocusOnError="True"
                                                                            InitialValue="-1" ErrorMessage="Select Inflow type"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="In flow from ID" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblInflowFromId" runat="server" Text='<%# Bind("InflowFromID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField HeaderText="In flow from ID" Visible="False">
                                                          <ItemTemplate>
                                                          <asp:Label ID="lblInflowFromId" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                                                          </ItemTemplate>
                                                          </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Entity ID" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEntityID_InFlow" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Name">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEntityName_InFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlEntityName_InFlow" runat="server" Width="200px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlEntityName_InFlow" runat="server" ControlToValidate="ddlEntityName_InFlow"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabInflow" SetFocusOnError="True"
                                                                            InitialValue="-1" ErrorMessage="Select a Inflow Entity/Customer name"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount_Inflow" runat="server" Text='<%# Bind("Amount") %>' Style="text-align: right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAmount_Inflow" runat="server" MaxLength="10" Style="text-align: right"> </asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Inflow" runat="server" FilterType="Numbers"
                                                                            TargetControlID="txtAmount_Inflow">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtAmount_Inflow" runat="server" ControlToValidate="txtAmount_Inflow"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabInflow" SetFocusOnError="True"
                                                                            ErrorMessage="Enter the Inflow amount"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
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
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                            Text="Remove">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <%-- <asp:LinkButton ID="LbtnAddCashFlow" runat="server" CausesValidation="true" 
                                                                Text="Add" ValidationGroup="TabOfferTerms" ForeColor="#FFFFFF"> </asp:LinkButton>--%>
                                                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="true" ValidationGroup="TabInflow"
                                                                            OnClick="CashInflow_AddRow_OnClick" CssClass="styleGridShortButton" Width="50px" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <%-- Columns to be added for IRR Calculation--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="div5" ScrollBars="Auto" CssClass="stylePanel" GroupingText="Cash Outflow details">
                                                    <div style="overflow: auto; width: 940px; height: 125px; padding-left: 10px;">
                                                        <asp:GridView ID="gvOutFlow" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvOutFlow_RowDeleting"
                                                            ShowFooter="True" OnRowCreated="gvOutFlow_RowCreated" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Date">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDate_GridOutflow" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"Date")).ToString(strDateFormat) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtDate_GridOutflow" runat="server" Width="100px"> </asp:TextBox>
                                                                        <cc1:CalendarExtender ID="CalendarExtenderSD_OutflowDate" runat="server" Enabled="True"
                                                                            TargetControlID="txtDate_GridOutflow" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtDate_GridOutflow" runat="server" ControlToValidate="txtDate_GridOutflow"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" ErrorMessage="Enter the Outflow Date"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cash flow Id" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOutflowid" runat="server" Text='<%# Bind("CashOutFlowID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Cash flow Description">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOutflowDesc" runat="server" Text='<%# Bind("CashOutFlow") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlOutflowDesc" runat="server" Width="200px" OnSelectedIndexChanged="ddlOutflowDesc_SelectedIndexChanged"
                                                                            AutoPostBack="True">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlOutflowDesc" runat="server" ControlToValidate="ddlOutflowDesc"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" SetFocusOnError="True"
                                                                            InitialValue="-1" ErrorMessage="Select Cash Outflow"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Payement to ID" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPayementToId" runat="server" Text='<%# Bind("OutflowFromID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Payment to">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPaymentto" runat="server" Text='<%# Bind("OutflowFrom") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlPaymentto_OutFlow" runat="server" Width="200px" AutoPostBack="True"
                                                                            OnSelectedIndexChanged="ddlPaymentto_OutFlow_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlPaymentto_OutFlow" runat="server" ControlToValidate="ddlPaymentto_OutFlow"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" SetFocusOnError="True"
                                                                            InitialValue="-1" ErrorMessage="Select Payment to"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Entity ID" Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEntityID_OutFlow" runat="server" Text='<%# Bind("EntityID") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Entity Name">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblHeading" runat="server" Text="Customer/Entity Name"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEntityName_OutFlow" runat="server" Text='<%# Bind("Entity") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlEntityName_OutFlow" runat="server" Width="200px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlEntityName_OutFlow" runat="server" ControlToValidate="ddlEntityName_OutFlow"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" SetFocusOnError="True"
                                                                            InitialValue="0" ErrorMessage="Select Outflow Entity Name">
                                                                        </asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAmount_Outflow" runat="server" Text='<%# Bind("Amount") %>' Style="text-align: right"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAmount_Outflow" runat="server" MaxLength="10" Style="text-align: right"> </asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="ftextExtxtAmount_Outflow" runat="server" FilterType="Numbers"
                                                                            TargetControlID="txtAmount_Outflow">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtAmount_Outflow" runat="server" ControlToValidate="txtAmount_Outflow"
                                                                            CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabOutflow" SetFocusOnError="True"
                                                                            ErrorMessage="Enter the Outflow amount"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
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
                                                                <%-- Columns to be added for IRR Calculation Ends--%>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnRemove" runat="server" CausesValidation="false" CommandName="Delete"
                                                                            Text="Remove">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnAddOut" runat="server" Text="Add" CausesValidation="true" ValidationGroup="TabOutflow"
                                                                            OnClick="CashOutflow_AddRow_OnClick" CssClass="styleGridShortButton" Width="50px" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                                <div id="div14" style="overflow: auto; height: 100%; width: 900px" runat="server">
                                                    <asp:ValidationSummary ID="vs_TabInflow" runat="server" CssClass="styleMandatoryLabel"
                                                        Width="80%" ValidationGroup="TabInflow" HeaderText="Correct the following validation(s):  " />
                                                    <asp:CustomValidator ID="cv_TabOfferTerms" runat="server" CssClass="styleMandatoryLabel"
                                                        Display="None" ValidationGroup="Offer Terms"></asp:CustomValidator>
                                                    <%--  <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txt_Margin_Percentage" ErrorMessage="Enter a Valid Percentage!" Type="Double" MinimumValue="0.01" MaximumValue="100"></asp:RangeValidator> --%>
                                                    <asp:ValidationSummary ID="vs_TabOutflow" runat="server" CssClass="styleMandatoryLabel"
                                                        Width="80%" ValidationGroup="TabOutflow" HeaderText="Correct the following validation(s):  " />
                                                    <asp:CustomValidator ID="cvEnquiryResponse" runat="server" CssClass="styleMandatoryLabel"
                                                        Enabled="true" Width="98%" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabRepayment" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Repayment Terms
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <%--  <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblAccountIRR_Repay" CssClass="styleDisplayLabel" Text="Accounting IRR"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtAccountIRR_Repay" runat="server" Width="80px" ReadOnly="True"
                                                                Style="text-align: right"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblCompanyIRR_Repay" CssClass="styleDisplayLabel" Text="Company IRR"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtCompanyIRR_Repay" runat="server" Width="80px" ReadOnly="True"
                                                                Style="text-align: right"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblBusinessIRR_Repay" CssClass="styleDisplayLabel"
                                                                Text="Business IRR"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtBusinessIRR_Repay" runat="server" Width="80px" ReadOnly="True"
                                                                Style="text-align: right"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                       
                                                             <asp:Label runat="server" ID="lblTotalAmount" CssClass="styleDisplayLabel" Font-Bold="true"></asp:Label>
                                          
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                        </td>
                                                    </tr>--%>
                                                    <tr style="width: 100%">
                                                        <td style="width: 20%">
                                                            &nbsp;
                                                        </td>
                                                        <td style="width: 54%">
                                                            &nbsp;
                                                        </td>
                                                        <td style="width: 28%">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 100%">
                                                        <td class="styleFieldLabel" colspan="3" style="width: 100%">
                                                            <asp:Label runat="server" ID="lblBlockdepreciation" CssClass="styleDisplayLabel"
                                                                Text="Block depreciation %" Width="12%"></asp:Label>
                                                            <asp:TextBox ID="txtBlockdepreciation" runat="server" ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Label runat="server" ID="lblBookdepreciation" CssClass="styleDisplayLabel" Text="Book depreciation %"
                                                                Width="12%"></asp:Label>
                                                            <asp:TextBox ID="txtBookdepreciation" runat="server" ReadOnly="True" Style="text-align: right"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr style="width: 100%">
                                                        <td style="width: 20%">
                                                            <asp:Panel ID="Panel11" runat="server" CssClass="stylePanel" GroupingText="Repayment Details"
                                                                Width="290px">
                                                                <table>
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
                                                        <td style="width: 54%">
                                                            <asp:Panel ID="pnlRepaymentSummary" runat="server" CssClass="stylePanel" ScrollBars="Auto"
                                                                Width="99%">
                                                                <asp:GridView ID="gvRepaymentSummary" runat="server" Width="96%" AutoGenerateColumns="false"
                                                                    Caption="Summary Details" CaptionAlign="Top">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CashFlow_Description" HeaderText="Cash Flow Description" />
                                                                        <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </td>
                                                        <td style="width: 28%">
                                                            <asp:Panel ID="panIRR" runat="server" CssClass="stylePanel" GroupingText="IRR Details"
                                                                Width="237px">
                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblAccountIRR_Repay" CssClass="styleReqFieldLabel"
                                                                                Text="Accounting IRR" Font-Bold="false"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtAccountIRR_Repay" runat="server" Width="100px" ReadOnly="True"
                                                                                Font-Bold="false" Style="text-align: right"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblBusinessIRR_Repay" CssClass="styleReqFieldLabel"
                                                                                Font-Bold="false" Text="Business IRR"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtBusinessIRR_Repay" runat="server" Width="100px" Font-Bold="false"
                                                                                Style="text-align: right"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label runat="server" ID="lblCompanyIRR_Repay" CssClass="styleReqFieldLabel"
                                                                                Font-Bold="false" Text="Company IRR"></asp:Label>
                                                                        </td>
                                                                        <td class="styleFieldAlign">
                                                                            <asp:TextBox ID="txtCompanyIRR_Repay" runat="server" Width="100px" Font-Bold="false"
                                                                                Style="text-align: right"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div id="div6" style="overflow: auto; height: auto; width: 100%; padding-left: 10px;"
                                                    runat="server">
                                                    <asp:GridView ID="gvRepaymentDetails" runat="server" AutoGenerateColumns="False"
                                                        ShowFooter="True" OnRowDeleting="gvRepaymentDetails_RowDeleting" OnRowDataBound="gvRepaymentDetails_RowDataBound"
                                                        OnRowCreated="gvRepaymentDetails_RowCreated" Width="1057px">
                                                        <Columns>
                                                            <asp:BoundField DataField="slno" HeaderText="Sl.No" />
                                                            <asp:TemplateField HeaderText="Repayment CashFlowId" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCashFlowId" runat="server" Text='<%# Bind("CashFlowId") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Repayment CashFlow">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCashFlow" runat="server" Text='<%# Bind("CashFlow") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlRepaymentCashFlow_RepayTab" runat="server" Width="200px"
                                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlRepaymentCashFlow_RepayTab_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvddlRepaymentCashFlow_RepayTab" runat="server"
                                                                        ControlToValidate="ddlRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                        Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" InitialValue="-1"
                                                                        ErrorMessage="Select a Repayment cashflow">
                                                                    </asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmountRepaymentCashFlow_RepayTab" runat="server" Text='<%# Bind("Amount") %>'
                                                                        Style="text-align: right"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAmountRepaymentCashFlow_RepayTab" runat="server" Width="80px"
                                                                        Style="text-align: right" MaxLength="10" ReadOnly="true"> </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtAmountRepaymentCashFlow_RepayTab" runat="server"
                                                                        FilterType="Numbers" TargetControlID="txtAmountRepaymentCashFlow_RepayTab">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtAmountRepaymentCashFlow_RepayTab" runat="server"
                                                                        ControlToValidate="txtAmountRepaymentCashFlow_RepayTab" CssClass="styleMandatoryLabel"
                                                                        Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the amount"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Per Installment Amount" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPerInstallmentAmount_RepayTab" runat="server" Text='<%# Bind("PerInstall") %>'
                                                                        Style="text-align: right"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtPerInstallmentAmount_RepayTab" runat="server" Width="120px" MaxLength="7"
                                                                        Style="text-align: right"> </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                        FilterType="Numbers" TargetControlID="txtPerInstallmentAmount_RepayTab">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtPerInstallmentAmount_RepayTab" runat="server"
                                                                        ControlToValidate="txtPerInstallmentAmount_RepayTab" CssClass="styleMandatoryLabel"
                                                                        Display="None" ValidationGroup="TabRepayment1" SetFocusOnError="True" ErrorMessage="Enter the Per installment amount"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Breakup Percentage" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBreakup_RepayTab" runat="server" Text='<%# Bind("Breakup") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtBreakup_RepayTab" runat="server" Width="100px" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                                        Style="text-align: right"> </asp:TextBox>
                                                                    <%--<cc1:MaskedEditExtender ID="MEE_txtBreakup_RepayTab" runat="server" TargetControlID="txtBreakup_RepayTab"
                                                                        Mask="99.99" MaskType="Number" CultureAMPMPlaceholder="" ClearMaskOnLostFocus="true"
                                                                        InputDirection="RightToLeft" Enabled="True">
                                                                    </cc1:MaskedEditExtender>--%>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Installment" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFromInstallment_RepayTab" runat="server" Text='<%# Bind("FromInstall") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtFromInstallment_RepayTab" runat="server" Width="80px" MaxLength="3"
                                                                        Text="1" ReadOnly="true" Style="text-align: right"> </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtFromInstallment_RepayTab" runat="server"
                                                                        FilterType="Numbers" TargetControlID="txtFromInstallment_RepayTab">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtFromInstallment_RepayTab" runat="server" ControlToValidate="txtFromInstallment_RepayTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                        SetFocusOnError="True" ErrorMessage="Enter the From installment"></asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Installment" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblToInstallment_RepayTab" runat="server" Text='<%# Bind("ToInstall") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtToInstallment_RepayTab" runat="server" Width="80px" MaxLength="3"
                                                                        Style="text-align: right"> </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="ftextExtxtToInstallment_RepayTab" runat="server"
                                                                        FilterType="Numbers" TargetControlID="txtToInstallment_RepayTab">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtToInstallment_RepayTab" runat="server" ControlToValidate="txtToInstallment_RepayTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                        SetFocusOnError="True" ErrorMessage="Enter the To installment"></asp:RequiredFieldValidator>
                                                                    <asp:CompareValidator ID="cmpvFromTOInstall" runat="server" ErrorMessage="To installment should be greater than From installment"
                                                                        ControlToValidate="txtToInstallment_RepayTab" ControlToCompare="txtFromInstallment_RepayTab"
                                                                        Display="None" ValidationGroup="TabRepayment1" Type="Currency" Operator="GreaterThanEqual"></asp:CompareValidator>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfromdate_RepayTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '></asp:Label>
                                                                    <asp:TextBox ID="txRepaymentFromDate" runat="server" Visible="false" BackColor="Navy"
                                                                        ForeColor="White" Font-Names="calibri" Font-Size="12px" Width="100px" Style="color: White"
                                                                        Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"FromDate")).ToString(strDateFormat) %> '
                                                                        AutoPostBack="True" OnTextChanged="txRepaymentFromDate_TextChanged"></asp:TextBox>
                                                                    <cc1:CalendarExtender ID="calext_FromDate" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                                        TargetControlID="txRepaymentFromDate">
                                                                    </cc1:CalendarExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtfromdate_RepayTab" runat="server" Width="100px"> </asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtenderSD_fromdate_RepayTab" runat="server" Enabled="True"
                                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" TargetControlID="txtfromdate_RepayTab">
                                                                    </cc1:CalendarExtender>
                                                                    <%-- <asp:RequiredFieldValidator ID="rfvtxtfromdate_RepayTab" runat="server" ControlToValidate="txtfromdate_RepayTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="TabRepayment1"
                                                                        SetFocusOnError="True" ErrorMessage="Enter the from date"></asp:RequiredFieldValidator>--%>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To Date">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTODate_ReapyTab" runat="server" Text='<%#Convert.ToDateTime(DataBinder.Eval(Container.DataItem,"ToDate")).ToString(strDateFormat) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtToDate_RepayTab" runat="server" Width="100px" Visible="false"> </asp:TextBox>
                                                                    <cc1:CalendarExtender ID="CalendarExtenderSD_ToDate_RepayTab" runat="server" Enabled="True"
                                                                        OnClientDateSelectionChanged="checkDate_PrevSystemDate" TargetControlID="txtToDate_RepayTab">
                                                                    </cc1:CalendarExtender>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnRemoveRepayment" CausesValidation="false" runat="server" CommandName="Delete"
                                                                        Text="Remove" Visible="false">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnAddRepayment" runat="server" Text="Add" CausesValidation="true"
                                                                        CssClass="styleGridShortButton" OnClick="Repayment_AddRow_OnClick" ValidationGroup="TabRepayment1"
                                                                        Width="50px"></asp:Button>
                                                                </FooterTemplate>
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
                                                <table width="98%">
                                                    <tr align="right">
                                                        <td>
                                                            <asp:Button ID="btnGenerateRepay" runat="server" Style="display: none" Text="Button"
                                                                OnClick="btnGenerateRepay_Click" />
                                                            <asp:Button runat="server" ID="btnCalIRR" Text="Calculate IRR" CssClass="styleSubmitLongButton"
                                                                OnClick="FunCalculateIRR" />
                                                            <asp:Button runat="server" ID="btnReset" Text="Reset" CssClass="styleSubmitShortButton"
                                                                OnClick="btnReset_Click" OnClientClick="return Confirmmsg('Are you sure want to Reset Repayment Structure?')"
                                                                Height="26px" Width="47px" />
                                                            <%--                                                                  <input id="Hidden1" type="hidden" runat="server" />
                                                            <input id="Hidden2" type="hidden" runat="server" />
                                                            <input id="hdnRoundOff" type="hidden" runat="server" />--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="PanRepay" runat="server" Width="99%" Height="100%">
                                                                <asp:GridView ID="grvRepayStructure" runat="server" AutoGenerateColumns="false" Width="101%">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="InstallmentNo" HeaderText="Installment No" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="From_Date" HeaderText="From Date" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="To_Date" HeaderText="To Date" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Installment_Date" HeaderText="Installment Date" ItemStyle-HorizontalAlign="Left">
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="NoofDays" HeaderText="No Of days" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InstallmentAmount" HeaderText="Installment Amount" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Charge" HeaderText="Finance Charges" ItemStyle-HorizontalAlign="Right"
                                                                            Visible="false">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="PrincipalAmount" HeaderText="Principal Amount" ItemStyle-HorizontalAlign="Right"
                                                                            Visible="false">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Insurance" HeaderText="Insurance" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="Others" HeaderText="Other Amount" ItemStyle-HorizontalAlign="Right">
                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                                <br />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <%--<a id="hideModalPopupClientButton" href="#" onclick="hideModalPopupViaClient();">
                                                        Close</a>
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtenderRepayDetails" runat="server" TargetControlID="btnShowRepayment"
                                                        PopupControlID="PanRepay" BackgroundCssClass="styleModalBackground" Enabled="True"
                                                        BehaviorID="programmaticModalPopupBehavior" RepositionMode="RepositionOnWindowResize"
                                                        X="200" Y="200">
                                                    </cc1:ModalPopupExtender>--%>
                                                <br />
                                                <div id="div15" style="overflow: auto; height: 100%; width: 900px" runat="server">
                                                    <asp:ValidationSummary ID="vs_TabRepayment" runat="server" CssClass="styleMandatoryLabel"
                                                        Width="80%" ValidationGroup="TabRepayment1" HeaderText="Correct the following validation(s):  " />
                                                    <asp:CustomValidator ID="cv_TabRepayment" runat="server" CssClass="styleMandatoryLabel"
                                                        Display="None" ValidationGroup="TabRepayment1"></asp:CustomValidator>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnCalIRR" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnGenerateRepay" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabAlerts" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Alerts
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <div id="div2" style="overflow: auto; height: 150px; width: 850px" runat="server"
                                                    border="1">
                                                    <asp:GridView ID="gvAlert" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                        OnRowDeleting="gvAlert_RowDeleting" onrowdatabound="gvAlert_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Type">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlType_AlertTab" runat="server" Width="200px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlType_AlertTab" runat="server" Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvddlType_AlertTab" runat="server" ControlToValidate="ddlType_AlertTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="gvAlert" SetFocusOnError="True"
                                                                        ErrorMessage="Select a Type" InitialValue="-1"></asp:RequiredFieldValidator></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="User Contact">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlContact_AlertTab" runat="server" Width="200px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlContact_AlertTab" runat="server" Width="200px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvddlContact_AlertTab" runat="server" ControlToValidate="ddlContact_AlertTab"
                                                                        CssClass="styleMandatoryLabel" Display="None" ValidationGroup="gvAlert" SetFocusOnError="True"
                                                                        ErrorMessage="Select a User Contact" InitialValue="-1"></asp:RequiredFieldValidator></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="EMail">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkEmail" runat="server"  Text ='<%# Bind("EMail") %>'  /></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:CheckBox ID="ChkEmail" runat="server"></asp:CheckBox></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SMS">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkSMS" runat="server"  Text ='<%# Bind("SMS") %>' /></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:CheckBox ID="ChkSMS" runat="server"></asp:CheckBox></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                        Text="Remove"></asp:LinkButton></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <%-- <asp:LinkButton ID="LbtnAddAlert" runat="server" Text="Add" CausesValidation="true"
                                                                        OnClick="Alert_AddRow_OnClick">                                                                                
                                                                    </asp:LinkButton>--%>
                                                                    <asp:Button ID="LbtnAddAlert" runat="server" CausesValidation="true" OnClick="Alert_AddRow_OnClick"
                                                                        Text="Add" CssClass="styleGridShortButton" Width="50px" ValidationGroup="gvAlert">
                                                                    </asp:Button></FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <br />
                                                <div id="div55" style="overflow: auto; height: 100%; width: 99%" runat="server">
                                                    <asp:ValidationSummary ID="vs_AlertS" runat="server" CssClass="styleMandatoryLabel"
                                                        Width="80%" HeaderText="Correct the following validation(s):  " ValidationGroup="gvAlert" />
                                                    <asp:CustomValidator ID="cv_AlertS" runat="server" CssClass="styleMandatoryLabel"
                                                        Display="None" ValidationGroup="Alerts"></asp:CustomValidator></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel runat="server" ID="TabFollowUp" CssClass="tabpan" BackColor="Red">
                                    <HeaderTemplate>
                                        Follow Up
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" style="width: 99%">
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblLOB_Followup" CssClass="styleDisplayLabel" Text="Line of Business"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtLOB_Followup" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblBranch_Followup" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtBranch_Followup" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblEnquiry_Followup" CssClass="styleDisplayLabel" Text="Enquiry Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtEnquiry_Followup" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblEnquiryDate_Followup" CssClass="styleDisplayLabel"
                                                                Text="Date"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtEnquiryDate_Followup" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblCustNameAdd_Followup" CssClass="styleDisplayLabel"
                                                                Text="Customer Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtProspectName_Followup" runat="server" Width="200px" ReadOnly="True"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblOfferNo_Followup" runat="server" CssClass="styleDisplayLabel" Text="Offer Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtOfferNo_Followup" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblApplication_Followup" runat="server" CssClass="styleDisplayLabel"
                                                                Text="Application Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" colspan="3">
                                                            <asp:TextBox ID="txtApplication_Followup" runat="server" ReadOnly="True" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <div id="div3" style="overflow: auto; height: 200px; width: 100%" runat="server">
                                                    <asp:GridView ID="gvFollowUp" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                        OnRowDeleting="gvFollowUp_RowDeleting" OnRowCreated="gvFollowUp_RowCreated" Width="99%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Date">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtDate_GridFollowup" runat="server" ReadOnly="true" Width="80px"
                                                                        Text='<%#Bind("Date") %>'> </asp:TextBox></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtDate_GridFollowup" runat="server" Width="80px"> </asp:TextBox><cc1:CalendarExtender
                                                                        runat="server" TargetControlID="txtDate_GridFollowup" ID="CalendarExtenderSD_FollowupDate"
                                                                        Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtDate_GridFollowup" runat="server" ControlToValidate="txtDate_GridFollowup"
                                                                        ValidationGroup="gvFollowUp" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Date"></asp:RequiredFieldValidator></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="From">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlfrom_GridFollowup" runat="server" Width="155px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlfrom_GridFollowup" runat="server" Width="155px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvddlfrom_GridFollowup" runat="server" ControlToValidate="ddlfrom_GridFollowup"
                                                                        ValidationGroup="gvFollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                        ErrorMessage="Select From" InitialValue="-1"></asp:RequiredFieldValidator></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="To">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlTo_GridFollowup" runat="server" Width="155px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlTo_GridFollowup" runat="server" Width="155px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvddlTo_GridFollowup" runat="server" ControlToValidate="ddlTo_GridFollowup"
                                                                        ValidationGroup="gvFollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                        ErrorMessage="Select To" InitialValue="-1"></asp:RequiredFieldValidator></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtAction_GridFollowup" runat="server" ReadOnly="true" TextMode="MultiLine"
                                                                        Text='<%#Bind("Action") %>'> </asp:TextBox></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAction_GridFollowup" runat="server" MaxLength="80" onkeypress="fnCheckSpecialChars(false)"
                                                                        TextMode="MultiLine" onkeyup="maxlengthfortxt(80)"> </asp:TextBox><asp:RequiredFieldValidator
                                                                            ID="rfvtxtAction_GridFollowup" runat="server" ControlToValidate="txtAction_GridFollowup"
                                                                            ValidationGroup="gvFollowUp" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                            ErrorMessage="Enter the Action"></asp:RequiredFieldValidator></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action Date">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtActionDate_GridFollowup" runat="server" ReadOnly="true" Width="80px"
                                                                        Text='<%#Bind("ActionDate") %>'> </asp:TextBox></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtActionDate_GridFollowup" runat="server" Width="80px"> </asp:TextBox><cc1:CalendarExtender
                                                                        runat="server" TargetControlID="txtActionDate_GridFollowup" ID="CalendarExtenderSD_FollowupActionDate"
                                                                        Enabled="True" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                                                    </cc1:CalendarExtender>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtActionDate_GridFollowup" runat="server" ControlToValidate="txtActionDate_GridFollowup"
                                                                        ValidationGroup="gvFollowUp" CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Action Date"></asp:RequiredFieldValidator></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Customer Response">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCustomerResponse_GridFollowup" runat="server" ReadOnly="true"
                                                                        TextMode="MultiLine" Text='<%#Bind("Customerresponse") %>'> </asp:TextBox></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtCustomerResponse_GridFollowup" runat="server" MaxLength="80"
                                                                        onkeypress="fnCheckSpecialChars(false)" TextMode="MultiLine" onkeyup="maxlengthfortxt(80)"> </asp:TextBox></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRemarks_GridFollowup" runat="server" ReadOnly="true" TextMode="MultiLine"
                                                                        Text='<%#Bind("Remarks") %>'> </asp:TextBox></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtRemarks_GridFollowup" runat="server" MaxLength="80" onkeypress="fnCheckSpecialChars(false)"
                                                                        TextMode="MultiLine" onkeyup="maxlengthfortxt(80)"> </asp:TextBox></FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnRemove" CausesValidation="false" runat="server" CommandName="Delete"
                                                                        Text="Remove"></asp:LinkButton></ItemTemplate>
                                                                <FooterTemplate>
                                                                    <%-- <asp:LinkButton ID="LbtnAddFollowup" runat="server" Text="Add" CausesValidation="true"
                                                                        OnClick="FollowUp_AddRow_OnClick" ValidationGroup="gvFollowUp">                                                                                
                                                                    </asp:LinkButton>--%><asp:Button ID="LbtnAddFollowup" runat="server" CausesValidation="true"
                                                                        OnClick="FollowUp_AddRow_OnClick" Text="Add" ValidationGroup="gvFollowUp" CssClass="styleGridShortButton">
                                                                    </asp:Button></FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                                <br />
                                                <div id="div17" style="overflow: auto; height: 100%; width: 900px" runat="server">
                                                    <asp:ValidationSummary ID="vs_gvFollowUp" runat="server" CssClass="styleMandatoryLabel"
                                                        Width="80%" HeaderText="Correct the following validation(s):  " ValidationGroup="gvFollowUp" />
                                                    <asp:CustomValidator ID="cv_gvFollowUp" runat="server" CssClass="styleMandatoryLabel"
                                                        Display="None" ValidationGroup="Follow Up"></asp:CustomValidator></div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </td>
                    </tr>
                </table>
                <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td>
            </td>
        </tr>
    </table>
    <br />
    <div id="div1" style="overflow: auto; height: 100%; width: 900px">
        <asp:ValidationSummary ID="vs_Main" runat="server" CssClass="styleMandatoryLabel"
            ShowSummary="true" Width="80%" HeaderText="Correct the following validation(s):  " />
        <asp:CustomValidator ID="cv_Main" runat="server" CssClass="styleMandatoryLabel" Display="None"
            ValidationGroup="Save"></asp:CustomValidator>
        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
        <asp:HiddenField ID="lblEnqStatus" runat="server" />
        <input type="hidden" id="hdnEdit_Status" runat="server" value="0" />
    </div>
    <br />
    <br />
    <div id="div9" style="overflow: auto; height: 100%; width: 100%" align="center">
        <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" Text="Save"
            OnClick="btnSave_OnClick" CausesValidation="true" />
        <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
            Text="Clear" OnClick="btnClear_OnClick" OnClientClick="return fnConfirmClear();" />
        <asp:Button ID="btnCalcel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
            UseSubmitBehavior="False" Text="Cancel" OnClick="btnCancel_Click" />
    </div>
    <br />
</asp:Content>
