<%@ Page Title="Global Credit Parameter Setup" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GOrgGlobalCreditParameter_Add.aspx.cs" Inherits="Origination_S3GOrgGlobalCreditParameter_Add"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        //debugger;
        function OnSumScore(objScore) {
            var objScore = document.getElementById(objScore);
            if (isNaN(objScore.value)) {
                objScore.value = "";
                alert("Enter a valid Score")
                objScore.focus();
                objScore.value = "";
                return false;

            }
            SumScore();
        }


        function SumImportance(inpimportant) {
            //debugger
            var inpimp = document.getElementById(inpimportant);

            if (isNaN(inpimp.value)) {
                alert("Enter the valid % of Importance")
                inpimp.focus();
                inpimp.value = "";

                var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParam');
                if (TargetBaseControl == null)
                    TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate');
                var Inputs = TargetBaseControl.getElementsByTagName("input");
                var TotalHygiene = 0;
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'text') {
                        if (Inputs[n].value != '') {
                            if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 10) == 'Importance') {
                                if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 18) != 'txtTotalImportance') {
                                    var str = Inputs[n].value.split('.');
                                    if (funChkDecimial(Inputs[n], '3', '2', 'Importance', true) == true)
                                        //                {
                                        TotalHygiene = parseFloat(TotalHygiene) + parseFloat(Inputs[n].value);
                                    //                  Inputs[n].value=parseFloat(Inputs[n].value).toFixed(3)
                                    Inputs[n].value = parseFloat(Inputs[n].value).toFixed(2)
                                    if (parseFloat(TotalHygiene).toFixed(2) > 100) {
                                        //document.getElementById(Inputs[n].id).value="";
                                        //document.getElementById(Inputs[n].id).focus();
                                        tab.set_activeTabIndex(0);
                                        alert('% of Importance should not exceed 100%');
                                        inpimp.value = "";
                                        inpimp.focus();
                                        return false;

                                    }
                                    //                }
                                }
                            }
                        }
                    }

                    if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 18) == 'txtTotalImportance') {
                        document.getElementById(Inputs[n].id).value = "";
                        if (TotalHygiene == 0)
                            document.getElementById(Inputs[n].id).value = "";
                        else
                            document.getElementById(Inputs[n].id).value = parseFloat(TotalHygiene).toFixed(2);

                    }
                }
                return false;

            }

            var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParam');
            if (TargetBaseControl == null)
                TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate');
            var Inputs = TargetBaseControl.getElementsByTagName("input");


            var TotalHygiene = 0;
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'text') {
                    if (Inputs[n].value != '') {
                        if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 10) == 'Importance') {
                            if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 18) != 'txtTotalImportance') {
                                //alert(Inputs[n].value);
                                var str = Inputs[n].value.split('.');

                                if (str[0].length > 3) {
                                    alert('importance % should not exceed 3 digits');
                                    Inputs[n].focus();
                                    return false;
                                }
                                //                if(funChkDecimial(Inputs[n],'3','2','Importance',true)==true)
                                //                {
                                TotalHygiene = parseFloat(TotalHygiene) + parseFloat(Inputs[n].value);
                                //                  Inputs[n].value=parseFloat(Inputs[n].value).toFixed(3)
                                Inputs[n].value = parseFloat(Inputs[n].value).toFixed(2)
                                if (parseFloat(TotalHygiene).toFixed(2) > 100) {
                                    //document.getElementById(Inputs[n].id).value="";
                                    //document.getElementById(Inputs[n].id).focus();
                                    tab.set_activeTabIndex(0);
                                    alert('% of Importance should not exceed 100%');
                                    inpimp.value = "";
                                    inpimp.focus();
                                    return false;

                                }
                                //                }
                            }
                        }

                    }
                    if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 18) == 'txtTotalImportance') {
                        document.getElementById(Inputs[n].id).value = "";
                        if (TotalHygiene == 0)
                            document.getElementById(Inputs[n].id).value = "";
                        else
                            document.getElementById(Inputs[n].id).value = parseFloat(TotalHygiene).toFixed(2);

                    }
                }
            }
        }

        function ValidateCreditScoreMandatory() {
            var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParam');
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            var TotalHygiene = 0;
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'text') {
                    if (Inputs[n].value == '') {
                        alert('Remove not defined credit score');
                        return false;
                    }
                }
            }
        }

        function SumScore() {
            try {
                //    if(isNaN(inpimportant.value))
                //    {
                //        alert("Enter the valid % of Imporanace")
                //        inpimportant.focus();
                //        return false;    
                //    }

                var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParam');
                if (TargetBaseControl == null)
                    TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate');
                var Inputs = TargetBaseControl.getElementsByTagName("input");

                var TotalScore = 0;
                //    var MSG = 'Score should not be greater than 9999';
                //    var chk =0;
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'text') {
                        if (Inputs[n].value != '') {
                            if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 5) == 'Score') {
                                if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 13) != 'txtTotalScore' && n != 19) {
                                    //alert(TargetBaseControl.getElementsByTagName("span")[9].innerText);
                                    //alert(n);

                                    TotalScore = parseFloat(TotalScore) + parseFloat(Inputs[n].value);


                                    if (parseFloat(TotalScore) > 9999) {
                                        document.getElementById(Inputs[n].id).value = "";
                                        document.getElementById(Inputs[n].id).focus();
                                        tab.set_activeTabIndex(0);
                                        alert('Score should not be greater than 9999');
                                        return false;
                                        //chk=1;
                                        //TotalScore=0;
                                        break;
                                    }
                                }
                                Inputs[n].value = parseFloat(Inputs[n].value).toFixed(2);
                            }
                        }

                        if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 13) == 'txtTotalScore') {
                            document.getElementById(Inputs[n].id).value = "";
                            if (TotalScore == 0) {
                                document.getElementById(Inputs[n].id).value = "";
                            }
                            else {
                                if (isNaN(TotalScore) == false) {
                                    document.getElementById(Inputs[n].id).value = parseFloat(TotalScore).toFixed(2);
                                }
                            }
                        }
                    }
                }
                //        if(chk)
                //        {
                //            alert(MSG);
                //            return false;
                //        }
                //        else
                //        {
                //            return true;
                //        }
            }
            catch (ex) {
                //alert('Catch');
                return false;
            }
        }

        function Validate100() {
            var txtaddHygiene = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tpApprovalLimits_grvApprovals_ctl03_txtaddHygiene');
            if (parseFloat(document.getElementById(txtaddHygiene.id).value) > 100) {
                alert('Hygiene should not be greater than 100 %');
                document.getElementById(txtaddHygiene.id).value = "";
                document.getElementById(txtaddHygiene.id).focus();
                return false;
            }
        }

        function ValidateMandatory() {
            //debugger;
            var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tpApprovalLimits_grvApprovals');
            var Inputs = TargetBaseControl.getElementsByTagName("input");
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'text') {
                    if (Inputs[n].value == '') {
                        if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 21) == 'txtaddFacilityFromAmt') {
                            document.getElementById(Inputs[n].id).focus();
                            alert('Enter the Facility From');
                            tab.set_activeTabIndex(2);
                            return false;
                        }
                        if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 19) == 'txtaddFacilityToAmt') {
                            document.getElementById(Inputs[n].id).focus();
                            alert('Enter the Facility To');
                            tab.set_activeTabIndex(2);
                            return false;
                        }
                    }
                    if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 19) == 'txtaddFacilityToAmt') {
                        if (parseFloat(document.getElementById(Inputs[n].id).value) < parseFloat(document.getElementById(Inputs[n - 1].id).value)) {
                            document.getElementById(Inputs[n].id).focus();
                            alert('Facility To should be greater than Facility From');
                            tab.set_activeTabIndex(2);
                            return false;
                        }
                    }
                    if (Inputs[n].id.substring(Inputs[n].id.length, Inputs[n].id.length - 13) == 'txtaddHygiene') {
                        if (document.getElementById(Inputs[n].id).value != '' && parseFloat(document.getElementById(Inputs[n].id).value) > 100) {
                            document.getElementById(Inputs[n].id).focus();
                            alert('Hygiene should not be greater than 100 %');
                            tab.set_activeTabIndex(2);
                            return false;
                        }
                    }

                }
            }
        }


        function funvalidatepercentage(input) {
            if (input.value != '') {
                if (isNaN(input.value)) {
                    alert('Hygiene% should accept valid entry');
                    input.value = "";
                    input.focus();
                    return false;
                }
                if (input.value > 100) {
                    alert('Hygiene% should not be greater than 100%');
                    input.value = "";
                    input.focus();
                    return false;
                }
            }
            ChkIsZero(input, 'Hygiene%');
        }

        function FunscoreEnabled(inputi, inputs, strDesc) {
            // debugger        
            var importance = document.getElementById(inputi).value;
            var score = document.getElementById(inputs).value;
            var hygienefactorscore = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl11_txtScore');
            if (importance == '') {
                document.getElementById(inputs).setAttribute("readOnly", "readOnly");
                if (strDesc != "HYGIENE FACTOR") document.getElementById(inputs).value = "";
            }
            else {
                document.getElementById(inputs).removeAttribute("readOnly");
            }
            hygienefactorscore.removeAttribute("readOnly");
            SumScore();
        }

        function fnValidationCRPDetails() {
            var importance = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl02_txtImportance');
            var Score = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl02_txtScore')
            var totScore = 0;
            var rowCnt = 0;
            var TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParam');
            if (TargetBaseControl == null)
                TargetBaseControl = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate');
            if (TargetBaseControl != null) {
                var Inputs = TargetBaseControl.getElementsByTagName("input");
                var i = 2;
                for (var n = 0; n < (Inputs.length / 2) ; ++n) {
                    if (i <= 9) {
                        var importance = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl0' + i + '_txtImportance');
                        var Score = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl0' + i + '_txtScore')
                        var Decsription = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl0' + i + '_lblDec')

                        if (importance != null && Score != null) {
                            if (importance.value != '') {
                                rowCnt++;
                                var totScore = parseFloat(totScore) + parseFloat(importance.value);
                                if (Score.value == '' && Score.disabled == false) {
                                    tab.set_activeTabIndex(0);
                                    alert('Enter the Score for ' + Decsription.innerText);
                                    Score.focus();
                                    return false;
                                }
                            }
                            if (document.getElementById("<%= hidMode.ClientID %>").value == "FromProfile") {
                                if (importance.value == '' && importance.disabled == false) {
                                    tab.set_activeTabIndex(0);
                                    alert('Enter the Importance % for ' + Decsription.innerText);
                                    importance.focus();
                                    return false;
                                }
                            }
                        }

                        if (i == 6) {
                            var ScoreH = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl0' + i + '_txtScore');
                            if (ScoreH.value == '') {
                                tab.set_activeTabIndex(0);
                                alert('Enter the Credit Score');
                                ScoreH.focus();
                                return false;
                            }
                        }

                    }
                    else {
                        var importance = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl' + i + '_txtImportance');
                        var Score = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl' + i + '_txtScore')
                        var Decsription = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl' + i + '_lblDec')

                        if (importance != null && Score != null) {
                            if (importance.value != '') {
                                rowCnt++;
                                var totScore = parseFloat(totScore) + parseFloat(importance.value);
                                if (Score.value == '' && Score.disabled == false) {
                                    tab.set_activeTabIndex(0);
                                    Score.focus();
                                    alert('Enter the Score for ' + Decsription.innerText);
                                    return false;
                                }
                            }
                            if (document.getElementById("<%= hidMode.ClientID %>").value == "FromProfile") {
                                if (importance.value == '' && importance.disabled == false) {
                                    tab.set_activeTabIndex(0);
                                    alert('Enter Importance % for ' + Decsription.innerText);
                                    importance.focus();
                                    return false;
                                }
                            }
                        }
                    }
                    if (i == (Inputs.length / 2)) {
                        var ScoreH = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP_GrvGlobalCreditParamUpdate_ctl' + i + '_txtScore')
                        if (ScoreH.value == '') {
                            tab.set_activeTabIndex(0);
                            alert('Enter the Hygiene Factor Score');
                            ScoreH.focus();
                            return false;
                        }
                    }
                    ++i;
                }
                if (rowCnt <= 1) {
                    tab.set_activeTabIndex(0);
                    alert('Enter minimum 2 Importance% and Score apart from Hygiene factor');
                    return false;
                }
                if (totScore != 100) {
                    tab.set_activeTabIndex(0);
                    alert('Importance should be 100 %');
                    return false;
                }
            }
            return true;
        }

        function fnCheckPageValidatorsGCP(strValGrp, blnConfirm) {
            if (Page_ClientValidate('grpGlobalCredit') == false) {
                tab.set_activeTabIndex(0);
                return false;
            }

            if (!fnValidationCRPDetails()) {
                tab.set_activeTabIndex(0);
                return false;
            }

            tab.set_activeTabIndex(1);
            if (Page_ClientValidate('grpGlobalOffer') == false) {
                tab.set_activeTabIndex(1);
                return false;
            }
            tab.set_activeTabIndex(2);

            if (Page_ClientValidate('grpGlobalCredit') == false && Page_ClientValidate('grpGlobalOffer') == false) {
                var i, val, strGroupName;
                var iResult = "1";
                for (i = 0; i < Page_Validators.length; i++) {
                    val = Page_Validators[i];

                    controlToValidate = val.getAttribute("controltovalidate");
                    if (controlToValidate != undefined) {
                        document.getElementById(controlToValidate).border = "1";
                    }
                }

                for (i = 0; i < Page_Validators.length; i++) { //For loop1
                    val = Page_Validators[i];
                    var isValidAttribute = val.getAttribute("isvalid");
                    var controlToValidate = val.getAttribute("controltovalidate");
                    if (controlToValidate != undefined) {
                        if (strValGrp == undefined) {

                            if (isValidAttribute == false) {

                                document.getElementById(controlToValidate).className = 'styleReqFieldFocus';
                                //This is to avoid not to validate control which is already in false state (valid attribute)
                                document.getElementById(controlToValidate).border = "0";
                                iResult = "0";
                            }
                            else if (document.getElementById(controlToValidate).border != "0") {
                                document.getElementById(controlToValidate).className = 'styleReqFieldDefalut';
                            }
                        }

                        else {
                            strGroupName = val.getAttribute("validationgroup");
                            if ((isValidAttribute == false) && (strValGrp == strGroupName)) {
                                document.getElementById(controlToValidate).className = 'styleReqFieldFocus';
                                //This is to avoid not to validate control which is already in false state (valid attribute)
                                document.getElementById(controlToValidate).border = "0";
                                iResult = "0";
                            }
                            else if (document.getElementById(controlToValidate).border != "0") {
                                document.getElementById(controlToValidate).className = 'styleReqFieldDefalut';
                            }
                        }

                    }  //Undefined loop condition

                } //For loop1 End

                Page_BlockSubmit = false; ////Added by Kali on 12-Jun-2010 This function is used to solve issues
                // Need twice click of cancel and clear button after validation summary is visible
            } //

            //    if(!Page_ClientValidate('grpGlobalCredit'))
            //    {                  
            //        tab.set_activeTabIndex(0);        
            //        return false;                           
            //    }
            //    else if(!Page_ClientValidate('grpGlobalOffer'))
            //    {                  
            //        tab.set_activeTabIndex(1);    
            //        return false;        
            //    }

            if (Page_ClientValidate('grpGlobalCredit') == true && Page_ClientValidate('grpGlobalOffer') == true) {

                if (blnConfirm == undefined) {
                    if (confirm('Do you want to save?')) {
                        Page_BlockSubmit = false;
                        //Added by Thangam M on 18/Oct/2012 to avoid double save click
                        var a = event.srcElement;
                        if (a.type == "submit") {
                            a.style.display = 'none';
                        }
                        //End here
                        return true;
                    }
                    else
                        return false;
                }
                else {
                    Page_BlockSubmit = false;
                    //Added by Thangam M on 18/Oct/2012 to avoid double save click
                    var a = event.srcElement;
                    if (a.type == "submit") {
                        a.style.display = 'none';
                    }
                    //End here
                    return true;
                }
            }
            else {
                Page_BlockSubmit = false;
                return false;
            }
        }

        function fnCheckApprovals(objHygiene, objApp) {
            if (document.getElementById(objHygiene).value == '') {
                document.getElementById(objApp).value = '';
            }
        }

        function fnCheckHygiene(objHygiene, objApp) {
            if (document.getElementById(objHygiene).value == '') {
                alert('Enter the Hygiene %');
                document.getElementById(objHygiene).focus();
                return false;
            }
        }



        function Funminapproval(inputM) {
            //alert(inputM.id);
            var s = inputM.id;
            var inputH = s.replace('minApprovalreq', 'Hygiene');
            // alert(s+'-------'+inputH);
            if (document.getElementById(inputH) != null) {
                if (document.getElementById(inputH).value == '') {
                    alert('Enter the Hygiene %');
                    document.getElementById(inputH).focus();
                    return false;
                }
            }
        }


        var tab; //,tab1,tab2,tab3 ;
        // var  trSummary2;
        function pageLoad() {
            tab = $find('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter');
            //       
            //       tab1 = $find('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tbglobalParameterSetUP');
            //       tab2 = $find('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tpOffer');
            //       tab3 = $find('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tpApprovalLimits');     
            // debugger;   

            //trSummary2 = document.getElementById('ctl00_ContentPlaceHolder1_tcGloblaCreditParameter_tpOffer_trSummary2');     
            tab.add_activeTabChanged(on_Change);
        }

        var index = 0;
        function on_Change(sender, e) {
            var newindex = tab.get_activeTabIndex();

            if (newindex > index) {
                switch (newindex) {
                    case 1:

                        //trSummary2.style.display="none";                   
                        if (!Page_ClientValidate('grpGlobalCredit')) {
                            tab.set_activeTabIndex(0);
                            index = 0;
                            break;
                        }
                        if (!fnValidationCRPDetails()) {
                            tab.set_activeTabIndex(0);
                            index = 0;
                            break;
                        }
                        tab.set_activeTabIndex(1);
                        break;

                    case 2:

                        if (!Page_ClientValidate('grpGlobalCredit')) {
                            tab.set_activeTabIndex(0);
                            index = 0;
                            break;
                        }
                        if (!fnValidationCRPDetails()) {
                            tab.set_activeTabIndex(0);
                            index = 0;
                            break;
                        }
                        //tab.set_activeTabIndex(1);        
                        if (!Page_ClientValidate('grpGlobalOffer')) {
                            tab.set_activeTabIndex(1);
                            index = 1;
                            break;
                        }
                        tab.set_activeTabIndex(2);
                        break;
                }
            }
        }




    </script>

    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" Text="Global Credit Parameter" ID="lblHeading" CssClass="styleDisplayLabel">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px">
                <cc1:TabContainer ID="tcGloblaCreditParameter" runat="server" CssClass="styleTabPanel"
                    Width="99%" ActiveTabIndex="0">
                    <cc1:TabPanel runat="server" HeaderText="Global Parameter Set Up" ID="tbglobalParameterSetUP"
                        CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Global Parameter Set Up&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 240px">
                                                <asp:RadioButtonList ID="rdbLevel" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdbLevel_SelectedIndexChanged">
                                                    <asp:ListItem Text="Transaction Level" Value="1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Customer Level" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 109px">
                                                <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"
                                                    ToolTip="Line of Business"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" Width="240px" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                                    onmouseover="ddl_itemchanged(this);">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlLOB"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select the Line of Business"
                                                    InitialValue="0" ValidationGroup="grpGlobalCredit"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 109px">
                                                <asp:Label ID="lblProduct" runat="server" CssClass="styleDisplayLabel" Text="Product"
                                                    ToolTip="Product"></asp:Label>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlProduct" runat="server" Width="240px" TabIndex="5" onmouseover="ddl_itemchanged(this);">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 109px">
                                                <asp:Label ID="lblConstitution" runat="server" CssClass="styleReqFieldLabel" Text="Constitution"
                                                    ToolTip="Constitution"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlConstitution" runat="server" AutoPostBack="True" Width="240px"
                                                    onmouseover="ddl_itemchanged(this);">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="0" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="ddlConstitution" ValidationGroup="grpGlobalCredit"
                                                    ErrorMessage="Select the Constitution" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 109px">
                                                <asp:Label runat="server" Text="Entity Type" ID="lblEntityType" CssClass="styleReqFieldLabel">
                                                </asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlEntityType" ValidationGroup="grpGlobalCredit" runat="server">
                                                    <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Designation" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="User Level" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="User Name" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="2">
                                                <asp:RequiredFieldValidator ID="rfvEntityType" Enabled="true" CssClass="styleMandatoryLabel"
                                                    runat="server" InitialValue="-1" ControlToValidate="ddlEntityType" ErrorMessage="Select Entity Type"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center" width="100%">
                                                <asp:Button ID="btnGo" runat="server" ValidationGroup="grpGlobalCredit" CssClass="styleSubmitShortButton"
                                                    TabIndex="9" Text="Go" OnClick="btnGo_Click" ToolTip="Go" />
                                                <asp:Button ID="btnCopyProfile" runat="server" Enabled="false" CssClass="styleSubmitButton"
                                                    TabIndex="9" ValidationGroup="grpGlobalCredit" Text="Copy Profile" OnClick="btnCopyProfile_Click"
                                                    ToolTip="Copy Profile" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                <input id="hidGridNo" runat="server" type="hidden" />
                                                <asp:Button ID="btnSubmit" runat="server" CssClass="styleSubmitButton" TabIndex="10"
                                                    Text="Submit" CausesValidation="False" OnClick="btnSubmit_Click" Visible="False"
                                                    ToolTip="Submit" />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr class="styleRecordCount" runat="server" id="trCopyProfileMessage" visible="false">
                                            <td align="center" width="98%">
                                                <br />
                                                <asp:Label runat="server" ID="Label4" Text="No Records Found" Font-Size="Medium"
                                                    class="styleMandatoryLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:HiddenField ID="hidMode" runat="server" Value="New" />
                                                <div id="divCopyProfile" runat="server" style="overflow-x: hidden; overflow-y: auto; height: 112px; width: 100%; display: none">
                                                    <asp:GridView ID="GrvGlobalCreditMaster" runat="server" AutoGenerateColumns="False"
                                                        OnRowCreated="GrvGlobalCreditMaster_RowCreated">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkselect" runat="server" OnCheckedChanged="chkselect_CheckedChanged"
                                                                        AutoPostBack="true" /><asp:Label ID="lblGlobalCreditParameterID" Text='<%# Bind("Global_Credit_Parameter_ID")%>'
                                                                            Visible="false" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="LOB_Code_Name" HeaderText="Line of Business" />
                                                            <asp:BoundField DataField="Product_Code_Name" HeaderText="Product" />
                                                            <asp:BoundField DataField="Constitution_Code_Name" HeaderText="Constitution" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="panelGlobalCredit" GroupingText="Global Credit Parameter Details"
                                                    runat="server" CssClass="stylePanel">
                                                    <table>
                                                        <tr>
                                                            <td width="100%" valign="top" align="center">
                                                                <asp:GridView ID="GrvGlobalCreditParamUpdate" DataKeyNames="Creditscore_Item_Id"
                                                                    runat="server" AutoGenerateColumns="False" ShowFooter="True" OnRowDataBound="GrvGlobalCreditParamUpdate_RowDataBound"
                                                                    Width="100%">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Credit Score" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="left">
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="lblID" Text="Total" runat="server">
                                                                                </asp:Label>
                                                                            </FooterTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDec" Text='<%# Bind("Description")%>' runat="server" Width="300px">
                                                                                </asp:Label>
                                                                                <asp:Label ID="lblScore_Item_ID" Visible="false" Text='<%# Bind("Creditscore_Item_Id")%>'
                                                                                    runat="server">
                                                                                </asp:Label>
                                                                                <asp:Label ID="lblGlobal_Credit_Parameter_CreditScore_ID" Visible="false" Text='<%# Bind("GLOBAL_CRDT_PARAM_CTSCORE_ID")%>'
                                                                                    runat="server"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Importance %" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="Right">
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtTotalImportance" Enabled="false" Width="50px" runat="server"
                                                                                    Style="text-align: right" ToolTip="Total Importance%"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                            <ItemTemplate>
                                                                                <%-- <asp:TextBox ID="txtImportance" Text='<%# Bind("Percentage_Of_Importance")%>' 
                                                                                    MaxLength="5" runat="server" OnTextChanged="Funscoreenabled" AutoPostBack="true"></asp:TextBox>--%>
                                                                                <asp:TextBox Style="text-align: right" ID="txtImportance" Text='<%# Bind("Percentage_Of_Importance")%>'
                                                                                    Width="50px" MaxLength="5" runat="server" ToolTip="Importance %">
                                                                                    <%--onblur="SumImportance(this);" --%>
                                                                                </asp:TextBox>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtImportance"
                                                                                    FilterType="Numbers,Custom" ValidChars="." Enabled="true">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Score" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="Right">
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="txtTotalScore" Enabled="false" runat="server" Width="50px" Style="text-align: right"
                                                                                    ToolTip="Total Score"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="txtScore" Width="50px" Style="text-align: right" Text='<%# Bind("Score")%>'
                                                                                    MaxLength="5" runat="server" ToolTip="Score"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                                        ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtScore" FilterType="Numbers,Custom"
                                                                                        Enabled="true" ValidChars=".">
                                                                                    </cc1:FilteredTextBoxExtender>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="grpGlobalCredit"
                                                    runat="server" ShowSummary="true" HeaderText="Correct the following validation(s):"
                                                    CssClass="styleMandatoryLabel" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tpOffer" CssClass="tabpan" BackColor="Red" ToolTip="Offer Card Details">
                        <HeaderTemplate>
                            Offer Card Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="panDateFormat23" GroupingText="Offer Card Details" runat="server"
                                        CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Negative Variation %" ID="Label2" CssClass="styleDisplayLabel"
                                                        ToolTip="Negative Variation %"> </asp:Label><span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtNegativeVariation" Style="text-align: right" MaxLength="6" onkeypress="fnAllowNumbersOnly(false,true,this)"
                                                        runat="server" Width="35%" ToolTip="Negative Variation %"> 
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtNegativeVariation" ErrorMessage="Enter Negative Variation %"
                                                        ValidationGroup="grpGlobalOffer" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ID="RangeValidator2" CssClass="styleMandatoryLabel" ControlToValidate="txtNegativeVariation"
                                                        ValidationGroup="grpGlobalOffer" MinimumValue="0" MaximumValue="100" Type="Double"
                                                        runat="server" Display="None" ErrorMessage="Negative Variation % should not be greater than 100 %"></asp:RangeValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="Exposure Variation %" ID="Label3" CssClass="styleDisplayLabel"
                                                        ToolTip="Exposure Variation %"> </asp:Label><span class="styleMandatory">*</span>
                                                </td>
                                                <td class="styleFieldAlign" width="50%">
                                                    <asp:TextBox ID="txtExposureVariation" Style="text-align: right" onkeypress="fnAllowNumbersOnly(false,true,this)"
                                                        MaxLength="6" runat="server" Width="35%" ToolTip="Exposure Variation %"> </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" CssClass="styleMandatoryLabel"
                                                        ValidationGroup="grpGlobalOffer" runat="server" ControlToValidate="txtExposureVariation"
                                                        ErrorMessage="Enter Exposure Variation %" Display="None"></asp:RequiredFieldValidator><asp:RangeValidator
                                                            ID="RangeValidator1" CssClass="styleMandatoryLabel" ValidationGroup="grpGlobalOffer"
                                                            ControlToValidate="txtExposureVariation" MinimumValue="0" Type="Double" MaximumValue="100"
                                                            runat="server" Display="None" ErrorMessage="Exposure Variation % should not be greater than 100 %"></asp:RangeValidator>
                                                </td>
                                            </tr>
                                            <tr id="trSummary2" runat="server" style="display: inline">
                                                <td colspan="3">
                                                    <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="grpGlobalOffer" runat="server"
                                                        ShowSummary="true" HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tpApprovalLimits" CssClass="tabpan" BackColor="Red"
                        ToolTip="Approvals Limits">
                        <HeaderTemplate>
                            Approvals Limits
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="panelApproval" GroupingText="Approvals Limits Details" runat="server"
                                        CssClass="stylePanel" ToolTip="Approvals Limits">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grvApprovals" ShowFooter="True" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        OnRowCommand="grvApprovals_RowCommand" OnRowDeleting="grvApprovals_RowDeleting"
                                                        OnRowDataBound="grvApprovals_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SL No" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSNo" Visible="false" runat="server" Text='<%# Bind("SNo") %>'></asp:Label>
                                                                    <asp:Label runat="server" ID="Label1" ToolTip="S.No." Width="70%" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="GCP_Approval_ID" ItemStyle-HorizontalAlign="Right"
                                                                Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblApprovallimitID" Text='<%# Bind("GCP_Approval_ID")%>' runat="server"
                                                                        Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Facility From">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtFacilityFromAmt" Style="text-align: right" MaxLength="11" Text='<%# Bind("Facility_From")%>'
                                                                        runat="server" Width="98%" ToolTip="Facility From"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                            ID="FilteredTextBoxExtender11" runat="server" TargetControlID="txtFacilityFromAmt"
                                                                            FilterType="Numbers" Enabled="true">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox Style="text-align: right" ID="txtaddFacilityFromAmt" MaxLength="11"
                                                                        Width="98%" runat="server" ToolTip="Facility From"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                            ID="FilteredTextBoxExtender10" runat="server" TargetControlID="txtaddFacilityFromAmt"
                                                                            FilterType="Numbers" Enabled="true">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Facility To">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtFacilityToAmt" Width="60%" MaxLength="11" Text='<%# Bind("Facility_To")%>'
                                                                        runat="server" ToolTip="Facility To" Style="text-align: right">
                                                                    </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" TargetControlID="txtFacilityToAmt"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtaddFacilityToAmt" Style="text-align: right" Width="60%" MaxLength="11"
                                                                        runat="server" ToolTip="Facility To"></asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtaddFacilityToAmt"
                                                                        FilterType="Numbers" Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <HeaderStyle Width="20%" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Range From">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRange_From" runat="server" MaxLength="6" Text='<%# Bind("Range_From")%>'
                                                                        Width="50%" ToolTip="Range From" Style="text-align: right">
                                                                    </asp:TextBox>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" TargetControlID="txtRange_From"
                                                                        FilterType="Numbers,custom" ValidChars="." Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtaddRange_From" Style="text-align: right" MaxLength="6" runat="server"
                                                                        Width="50%" ToolTip="Range From">
                                                                    </asp:TextBox><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                                        TargetControlID="txtaddRange_From" FilterType="Numbers,custom" ValidChars="."
                                                                        Enabled="true">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Range To">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtRange_To" Style="text-align: right" runat="server" Text='<%# Bind("Range_To")%>'
                                                                        Width="50%" ToolTip="Range To"></asp:TextBox><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender15"
                                                                            runat="server" TargetControlID="txtRange_To" FilterType="Numbers,custom" ValidChars="."
                                                                            Enabled="true">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtaddRange_To" Style="text-align: right" runat="server" Width="50%"
                                                                        ToolTip="Range To"> </asp:TextBox><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                                                                            runat="server" TargetControlID="txtaddRange_To" FilterType="Numbers,custom" ValidChars="."
                                                                            Enabled="true">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Approver">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="btnIApprover" CssClass="styleSubmitShortButton" runat="server" Text="Approver"
                                                                        OnClick="btnIApprover_Click" />
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnFApprover" CssClass="styleSubmitShortButton" runat="server" Text="Approver"
                                                                        OnClick="btnFApprover_Click" />
                                                                </FooterTemplate>
                                                                <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnAddrow" OnClientClick="return ValidateMandatory();" CausesValidation="false"
                                                                        runat="server" CommandName="AddNew" CssClass="styleGridShortButton" Text="Add"
                                                                        ToolTip="AddRow"></asp:Button>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="Center" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnRemove" Text="Remove" CommandName="Delete" runat="server"
                                                                        Width="50%" CausesValidation="false" ToolTip="Remove Row" />
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="10%" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblactive" runat="server" Text="Active" ToolTip="Active Indicator"></asp:Label>
                                                    &nbsp;<asp:CheckBox ID="chkActive" runat="server" Checked="True" ToolTip="Active Indicator" />
                                                    <asp:Button ID="btnReset" Visible="false" CssClass="styleSubmitButton" Text="Reset Approvals"
                                                        runat="server" OnClick="btnReset_Click" ToolTip="Active Indicator" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:ValidationSummary ID="valTab3" ValidationGroup="grptab3" runat="server" ShowSummary="true"
                                                        HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tpOtherInfo" runat="server" CssClass="tabpan" HeaderText="Main Page"
                        BackColor="Red" Style="padding: 0px">
                        <HeaderTemplate>
                            Other Static Informations
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="100%">
                                                <asp:GridView runat="server" ShowFooter="true" ID="grvOtherInfo" Width="100%" OnRowDataBound="grvOtherInfo_RowDataBound"
                                                    OnRowCommand="grvOtherInfo_RowCommand" OnRowDeleting="grvOtherInfo_RowDeleting"
                                                    AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Line No" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLineNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Flag" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFlag" runat="server" Text='<%#Eval("Flag")%>'></asp:Label>
                                                                <asp:DropDownList ID="ddlFlag" ToolTip="Flag" runat="server" AutoPostBack="true"
                                                                    Visible="false">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlFlag" InitialValue="0" ValidationGroup="AddNumber"
                                                                    runat="server" ControlToValidate="ddlFlag" Display="None" SetFocusOnError="true"
                                                                    ErrorMessage="Select the Flag"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="100px" HorizontalAlign="Center" />
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlFFlag" ToolTip="Line Type" runat="server" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ddlFFlag_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvFddlFFlag" InitialValue="0" ValidationGroup="AddNumber"
                                                                    runat="server" ControlToValidate="ddlFFlag" Display="None" SetFocusOnError="true"
                                                                    ErrorMessage="Select the Flag"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="50%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("Desc")%>'></asp:Label>
                                                                <asp:Label ID="lblDescID" runat="server" Text='<%#Eval("Flag_ID")%>' Visible="false"></asp:Label>
                                                                <cc1:ComboBox ID="txtDesc" Width="220px" runat="server" ToolTip="Description" Visible="false"
                                                                    AutoCompleteMode="Suggest">
                                                                </cc1:ComboBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtDesc" ValidationGroup="AddNumber" runat="server"
                                                                    ControlToValidate="txtDesc" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Description"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblFDesc" runat="server" Visible="true"></asp:Label>
                                                                <cc1:ComboBox ID="txtFDesc" runat="server" ToolTip="Description" Visible="false"
                                                                    AutoCompleteMode="Suggest">
                                                                </cc1:ComboBox>
                                                                <asp:RequiredFieldValidator ID="rfvFtxtFDesc" ValidationGroup="AddNumber" runat="server"
                                                                    ControlToValidate="txtFDesc" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Description"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Value" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblValue" runat="server" Text='<%#Eval("Value")%>' Visible="false"
                                                                    Style="text-align: right" Width="120px"></asp:Label>
                                                                <asp:TextBox ID="txtValue" runat="server" Width="120px" Style="text-align: right"
                                                                    Text='<%#Eval("Value")%>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFValue" MaxLength="15" Width="90px" runat="server" ToolTip="Value"
                                                                    Style="text-align: right"></asp:TextBox>
                                                            </FooterTemplate>
                                                            <FooterStyle Width="90px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Group Ref." HeaderStyle-Width="90px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGroupRef" runat="server" Text='<%#Eval("Group_Ref")%>' Visible="true"
                                                                    Style="text-align: center" Width="120px"></asp:Label>
                                                                <asp:TextBox ID="txtGroupRef" runat="server" MaxLength="4" Width="120px" Style="text-align: right"
                                                                    Text='<%#Eval("Group_Ref")%>' Visible="false"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtGroupRef" ValidationGroup="AddNumber" runat="server" Enabled="false"
                                                                    ControlToValidate="txtGroupRef" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Group Reference"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="90px" HorizontalAlign="Left" />
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFGroupRef" MaxLength="4" Width="90px" runat="server" ToolTip="Group Ref."></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvftxtFGroupRef" ValidationGroup="AddNumber" runat="server" Enabled="false"
                                                                    ControlToValidate="txtFGroupRef" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Group Reference"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle Width="90px" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-Wrap="true" HeaderText="Remarks">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' Visible="true"
                                                                    Style="text-align: left"></asp:Label>
                                                                <asp:TextBox ID="txtRemarks" runat="server" Width="200px" Style="text-align: left"
                                                                    Text='<%#Eval("Remarks")%>' Visible="false"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvtxtRemarks" ValidationGroup="AddNumber" runat="server"
                                                                    ControlToValidate="txtRemarks" Display="None" SetFocusOnError="true" ErrorMessage="Enter the Remarks"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                                                            <FooterTemplate>
                                                                <asp:TextBox ID="txtFRemarks" Width="200px" runat="server" ToolTip="Remarks"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvftxtFRemarks" ValidationGroup="AddNumber" runat="server"
                                                                    Enabled="false" ControlToValidate="txtFRemarks" Display="None" SetFocusOnError="true"
                                                                    ErrorMessage="Enter the Remarks"></asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                            <FooterStyle Width="200px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:LinkButton Text="Delete" OnClientClick="return confirm('Do you want to delete?');"
                                                                    runat="server" ID="lnkbtnDelete" CausesValidation="false" CommandName="Delete"
                                                                    ToolTip="Delete"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAddCredit" runat="server" CommandName="AddNew" CssClass="styleGridShortButton"
                                                                    Text="Add" ToolTip="Add" ValidationGroup="AddNumber"></asp:Button>
                                                            </FooterTemplate>
                                                            <FooterStyle Width="7%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ValidationSummary ID="ValidationSummary3"
                                                    runat="server" ShowSummary="true" HeaderText="Correct the following validation(s):"
                                                    CssClass="styleMandatoryLabel" ValidationGroup="AddNumber" />
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
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="UpdatePanel1Head" runat="server">
                    <ContentTemplate>
                        <br />
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            OnClientClick="return fnCheckPageValidatorsGCP('')" OnClick="btnSave_Click" ToolTip="Save" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CausesValidation="false" class="styleSubmitButton"
                            TabIndex="7" OnClick="btnClear_Click" ToolTip="Clear" OnClientClick="return fnConfirmClear();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="styleSubmitButton"
                            CausesValidation="False" OnClick="btnCancel_Click" ToolTip="Cancel" />
                        <br />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderApprover" runat="server" TargetControlID="btnModal"
                    PopupControlID="PnlApprover" BackgroundCssClass="styleModalBackground" Enabled="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="PnlApprover" Style="display: none; vertical-align: middle" runat="server"
                    BorderStyle="Solid" BackColor="White" Width="850px">
                    <asp:UpdatePanel ID="upPass" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <div id="divTransaction" class="container" runat="server" style="height: 200px; overflow: scroll; width: auto;">
                                                        <asp:GridView ID="grvApprover" runat="server" AutoGenerateColumns="false" Height="50px"
                                                            OnRowDataBound="grvApprover_DataBound" OnRowDeleting="grvApprover_RowDeleting"
                                                            ShowFooter="true">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNo" Text='<% #Bind("SNo")%>' Visible="false" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Approval Number" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSequenceNumber" ToolTip="Sequence Number" Text='<% #Bind("SequenceNumber")%>'
                                                                            runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="Approval Entity" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblApprovalEntity" Visible="false" runat="server" Text='<% #Bind("ApprovEntity") %>'>
                                                            </asp:Label>
                                                            <asp:DropDownList ID="ddlApprovalEntity" ToolTip="Approval Entity" runat="server"
                                                                Visible="false">
                                                                <asp:ListItem Text="--Select--" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="Designation" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="User Level" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="User Name" Value="3"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                                <asp:TemplateField HeaderText="Location">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocation" runat="server" Text='<% #Bind("Location")%>'>
                                                                        </asp:Label>
                                                                        <asp:Label ID="lblLocationID" runat="server" Visible="false" Text='<% #Bind("LocationID")%>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlLocation" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"
                                                                            ToolTip="Location" runat="server" Style="width: 340px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ValidationGroup="PopUpAdd"
                                                                            InitialValue="0" ControlToValidate="ddlLocation" Display="None" ErrorMessage="Select Location"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Approval Authority">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblApprovalPerson" runat="server" Text='<% #Bind("ApprovPerson")%>'>
                                                                        </asp:Label>
                                                                        <asp:Label ID="ApprovPersonID" runat="server" Visible="false" Text='<% #Bind("ApprovPersonID")%>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlApprovPerson" ToolTip="Approval Authority" runat="server"
                                                                            Style="width: 340px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvApprovalPerson" runat="server" ValidationGroup="PopUpAdd"
                                                                            ControlToValidate="ddlApprovPerson" Display="None" InitialValue="0" ErrorMessage="Select Approval Authority"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnDelete" Text="Delete" CommandName="Delete" runat="server"
                                                                            CausesValidation="false" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnDetails" Text="Add" ValidationGroup="PopUpAdd" CommandName="Add"
                                                                            runat="server" CssClass="styleGridShortButton" CausesValidation="false" OnClick="btnDetails_Click"
                                                                            OnClientClick="return fnCheckPageValidators('PopUpAdd',false);" />
                                                                    </FooterTemplate>
                                                                    <FooterStyle Width="15%" HorizontalAlign="Center" />
                                                                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnDEVModalAdd" runat="server" Text="Save" ToolTip="Save" class="styleSubmitButton"
                                                        OnClick="btnDEVModalAdd_Click" />
                                                    <asp:Button ID="btnDEVModalCancel" runat="server" Text="Close" OnClick="btnDEVModalCancel_Click"
                                                        ToolTip="Close" class="styleSubmitButton" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="vsPopUp" runat="server" ShowSummary="true" ValidationGroup="PopUpAdd"
                                                        HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Button ID="btnModal" Style="display: none" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
