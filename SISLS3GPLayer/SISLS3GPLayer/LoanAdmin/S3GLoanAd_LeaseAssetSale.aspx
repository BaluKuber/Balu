<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GLoanAd_LeaseAssetSale.aspx.cs" Inherits="LoanAdmin_S3GLoanAd_LeaseAssetSale" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc3" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PageNavigator.ascx" TagPrefix="uc4" TagName="PageNavigator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function fnLoadCustomer() {
            document.getElementById('<%=btnCreateCustomer.ClientID%>').click();
        }

        function fnLoadPath(btnBrowse) {
            if (btnBrowse != null)
                document.getElementById(btnBrowse).click();
        }

        function fnAssignPath(btnBrowse, hdnPath) {
            if (btnBrowse != null)
                document.getElementById(hdnPath).value = document.getElementById(btnBrowse).value;
        }

        function fnCalculateVAT(txt) {

            var row = txt.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            if (txt.value == "")
                txt.value = 0;
            var Sale_Price = parseFloat(txt.value);
            //Vat Amount
            row.cells[15].innerText = parseFloat(Sale_Price * (parseFloat(row.cells[14].innerText) / 100)).toFixed(2);
            //Additional Vat Amount
            row.cells[17].innerText = parseFloat(Sale_Price * (parseFloat(row.cells[16].innerText) / 100)).toFixed(2);
            //Profit/Loss
            row.cells[19].innerText = parseFloat(Sale_Price - ((parseFloat(row.cells[18].innerText) / parseFloat(row.cells[11].innerText)) * parseFloat(row.cells[12].children[0].value))).toFixed(2);
        }

        function fnCheckDspQty(txt) {

            var row = txt.parentNode.parentNode;

            var rowIndex = row.rowIndex - 1;

            //Available Quantity
            var AvlbQty = parseInt(row.cells[11].innerText);

            if (txt.value == "") {
                txt.value = AvlbQty;
                alert('Saleable Quantity should not be empty');
                return;
            }

            if (parseInt(txt.value) == 0) {
                txt.value = AvlbQty;
                alert('Saleable Quantity should be greater than 0');
                return;
            }

            var DspQty = parseInt(txt.value);

            if (DspQty > AvlbQty) {
                alert('Saleable Quantity should not exceed than Available Quantity');
                txt.value = AvlbQty;
            }

            //Profit/Loss
            row.cells[19].innerText = parseFloat(parseFloat(row.cells[13].children[0].value) - ((parseFloat(row.cells[18].innerText) / parseFloat(row.cells[11].innerText)) * txt.value)).toFixed(2);
        }

        function fnSelectAll(chkSelectAllBranch, chkSelectBranch) {
            var gvBranchWise = document.getElementById('<%=grvDisPosedDtl.ClientID%>');
            var TargetChildControl = chkSelectBranch;
            //Get all the control of the type INPUT in the base control.
            var Inputs = gvBranchWise.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllBranch.checked;
        }

        function fnCalculateSaleAmt()   //Calculate Sale Price
        {
            var i = 0;
            var ttlRV = 0;
            var count = 0;
            var aggAmt = document.getElementById('<%= txtAggregateAmt.ClientID %>');
            var gvDsp = document.getElementById('<%= grvDisPosedDtl.ClientID%>');

            if (aggAmt.value == "")
                aggAmt.value = 0;
            else if (aggAmt != '') {
                if (aggAmt == ".") {
                    alert('Enter a valid Decimal');
                    aggAmt.value = 0;
                }
                else {
                    for (var i = 0; i < aggAmt.length; i++) {
                        var c = aggAmt.charAt(i);
                        if (c == '.') {
                            count++;
                        }
                    }
                    if (count > 1) {
                        alert('Enter a valid Decimal');
                        aggAmt.value = 0;
                    }
                }
            }

            for (i = 1; i < gvDsp.rows.length; i++) //Calculate Total RV Amt
            {
                ttlRV = parseFloat(ttlRV) + parseFloat(gvDsp.rows[i].cells[18].innerText);  //RV Amount
            }
            ttlRV = parseFloat(ttlRV).toFixed(2);

            for (i = 1; i < gvDsp.rows.length; i++) //Calculate Sale Price
            {
                gvDsp.rows[i].cells[13].children[0].value = parseFloat((parseFloat(gvDsp.rows[i].cells[18].innerText) / parseFloat(ttlRV)) * parseFloat(aggAmt.value)).toFixed(2);
                gvDsp.rows[i].cells[19].innerText = parseFloat(parseFloat(gvDsp.rows[i].cells[13].children[0].value) - ((parseFloat(gvDsp.rows[i].cells[18].innerText) / parseFloat(gvDsp.rows[i].cells[11].innerText)) * parseFloat(gvDsp.rows[i].cells[12].children[0].value))).toFixed(2);
            }
        }

        function Lessee_ItemSelected(sender, e) {
            var hdnLessee = $get('<%= hdnLessee.ClientID %>');
            hdnLessee.value = e.get_value();
        }
        function Lessee_ItemPopulated(sender, e) {
            var hdnLessee = $get('<%= hdnLessee.ClientID %>');
            hdnLessee.value = '';
        }

        function Tranche_ItemSelected(sender, e) {
            var hdnTranche = $get('<%= hdnTranche.ClientID %>');
            hdnTranche.value = e.get_value();
        }
        function Tranche_ItemPopulated(sender, e) {
            var hdnTranche = $get('<%= hdnTranche.ClientID %>');
            hdnTranche.value = '';
        }

        function SaleState_ItemSelected(sender, e) {
            var hdnSaleStateID = $get('<%= hdnSaleStateID.ClientID %>');
            hdnSaleStateID.value = e.get_value();
        }
        function SaleState_ItemPopulated(sender, e) {
            var hdnSaleStateID = $get('<%= hdnSaleStateID.ClientID %>');
            hdnSaleStateID.value = '';
        }

        function fnSelectTransfer(chkgdtrnSelect) {
            document.getElementById('<%= hdnIsChange.ClientID %>').value = "1";
            var varTrancheID = chkgdtrnSelect.id;
            varTrancheID = varTrancheID.replace("chkgdtrnSelect", "lblgdtrnTrancheID");
            var label = document.getElementById(varTrancheID.toString());
            varTrancheID = label.innerText;

            var gvDtl = document.getElementById('<%= gvRSDtls.ClientID%>');

            for (i = 1; i < gvDtl.rows.length; i++) //Calculate Total RV Amt
            {
                var varRsTrnID = gvDtl.rows[i].cells[1].all[0].innerText.toString();
                if (parseInt(varRsTrnID) == parseInt(varTrancheID))   //
                {
                    var Inputs = gvDtl.getElementsByTagName("input");
                    Inputs[i].checked = chkgdtrnSelect.checked;
                }
            }

            fnChkIsTransferSelectAll(chkgdtrnSelect);
            fnChkIsRSSelectAll(chkgdtrnSelect);
        }

        function fnChkRs(cbx) {
            document.getElementById('<%= hdnIsChange.ClientID %>').value = "1";
            fnSelectDeselectTransferRS();
            fnChkIsRSSelectAll(cbx);
            fnChkIsTransferSelectAll(cbx);
        }

        function fnCbxSelectAll(chkSelectAllBranch, chkSelectBranch) {
            document.getElementById('<%= hdnIsChange.ClientID %>').value = "1";
            var gvBranchWise = document.getElementById('<%=gvRSDtls.ClientID%>');
            var TargetChildControl = chkSelectBranch;
            //Get all the control of the type INPUT in the base control.
            var Inputs = gvBranchWise.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllBranch.checked;

            fnSelectRSAll(chkSelectAllBranch);
        }

        function fnCheckMapQty(txt) {
            var row = txt.parentNode.parentNode;
            //Available Quantity
            fnIsModifyDSP();
            var AvlbQty = parseInt(row.cells[17].innerText);
            if (txt.value == "") {
                txt.value = AvlbQty;
                alert('Sale Quantity should not be empty');
                return;
            }

            if (parseInt(txt.value) == 0) {
                txt.value = AvlbQty;
                alert('Sale Quantity should be greater than 0');
                return;
            }

            var DspQty = parseInt(txt.value);

            if (DspQty > AvlbQty) {
                alert('Saleable Quantity should not exceed than Available Quantity');
                txt.value = AvlbQty;
            }
        }

        function fnChangeState(txt) {
            var hdnState = document.getElementById('<%=hdnSaleStateID.ClientID%>');
            var varCFormID = txt.id;
            varCFormID = varCFormID.replace("txtmapSellingState", "txtmapCFormNo");
            var label = document.getElementById(varCFormID.toString());
            var row = txt.parentNode.parentNode;
            if (hdnState.value != "") {
                if (parseInt(hdnState.value) == parseInt(row.cells[12].children[0].innerText)) {
                    row.cells[22].children[0].value = 'SGST & CGST';
                    row.cells[21].children[0].value = hdnState.value;
                    row.cells[23].disabled = true;
                    row.cells[23].getElementsByTagName("input")[0].checked = false;
                    label.value = '';
                    label.disabled = true;
                }
                else {
                    row.cells[21].children[0].value = hdnState.value;
                    row.cells[22].children[0].value = 'IGST';
                    row.cells[23].disabled = false;
                    row.cells[23].getElementsByTagName("input")[0].checked = false;
                    label.value = '';
                    label.disabled = true;
                }
            }
            hdnState.value = "";
        }

        function fnCheckCformAppl(cbxmapCForm) {
            var varCFormID = cbxmapCForm.id;
            varCFormID = varCFormID.replace("cbxmapCForm", "txtmapCFormNo");
            var label = document.getElementById(varCFormID.toString());
            label.value = '';
            label.disabled = (cbxmapCForm.checked == true) ? false : true;
            fnIsModifyDSP();
        }

        function fnIsModifyDSP() {
            document.getElementById('<%= hdnIsMapInvoiceChanged.ClientID %>').value = "1";
        }

        function fnChkSalePrice(txt)   //Calculate Sale Price
        {
            var i = 0;
            var ttlRV = 0;
            var count = 0;
            var aggAmt = txt;

            if (aggAmt.value == "")
                aggAmt.value = 0;
            else if (aggAmt != '') {
                if (aggAmt == ".") {
                    alert('Enter a valid Decimal');
                    aggAmt.value = 0;
                }
                else {
                    for (var i = 0; i < aggAmt.value.length; i++) {
                        var c = aggAmt.value.charAt(i);
                        if (c == '.') {
                            count++;
                        }
                    }
                    if (count > 1) {
                        alert('Enter a valid Decimal');
                        aggAmt.value = 0;
                    }
                }
            }
            fnIsModifyDSP();
        }

        function fnMapSelectAll(chkSelectAll, chkSelect) {
            fnIsModifyDSP();
            var gvMapInvoice = document.getElementById('<%=gvMapInvoiceDtl.ClientID%>');
            var TargetChildControl = chkSelect;
            //Get all the control of the type INPUT in the base control.
            var Inputs = gvMapInvoice.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAll.checked;
        }

        function fnChkIsSelect(chkSelect) {
            fnIsModifyDSP();
            var gv = document.getElementById('<%= gvMapInvoiceDtl.ClientID%>');
            var chk = document.getElementById('ctl00_ContentPlaceHolder1_gvMapInvoiceDtl_ctl01_chkDspAll');

            if (chkSelect.checked == false) {
                chk.checked = false;
            }
            else {
                var gvRwCnt = gv.rows.length - 1;
                var ChcCnt = 0;
                for (var i = 0; i < gv.rows.length; ++i) {
                    var Inputs = gv.rows[i].getElementsByTagName("input");
                    for (var n = 0; n < Inputs.length; ++n) {
                        if (Inputs[n].type == 'checkbox') {
                            if (Inputs[n].checked == true)
                                ++ChcCnt;
                        }
                    }
                }
                if (ChcCnt == gvRwCnt)
                    chk.checked = true;
                else
                    chk.checked = false;
            }
        }

        function fnSelectDeselectTransferRS()       //
        {
            var gvTranche = document.getElementById('<%= gvTrancheDtl.ClientID%>');
            var gvRSDtls = document.getElementById('<%= gvRSDtls.ClientID%>');
            var TtlCnt = 0;
            var RsCnt = 0;

            for (var i = 1 ; i < gvTranche.rows.length; i++) {
                TtlCnt = 0;
                var TrancheID = gvTranche.rows[i].cells[1].children[0].innerText;
                for (var j = 1 ; j < gvRSDtls.rows.length; j++) {
                    var RSTrancheID = gvRSDtls.rows[j].cells[1].all[0].innerText.toString();
                    if (parseInt(RSTrancheID) == parseInt(TrancheID))   //
                    {
                        TtlCnt = TtlCnt + 1;
                        var Inputs = gvRSDtls.getElementsByTagName("input");
                        if (Inputs[j].checked == true) {
                            RsCnt = RsCnt + 1;
                        }
                    }
                }
                var Inputs = gvTranche.getElementsByTagName("input");
                if (TtlCnt == RsCnt && RsCnt > 0) {
                    Inputs[i].checked = true;
                }
                else {
                    Inputs[i].checked = false;
                }
                RsCnt = 0;
            }
        }

        function fnSelectRSAll(chkSelect)       //
        {
            var gvTranche = document.getElementById('<%= gvTrancheDtl.ClientID%>');
            for (var i = 0 ; i < gvTranche.rows.length; i++) {
                var Inputs = gvTranche.getElementsByTagName("input");
                Inputs[i].checked = chkSelect.checked;
            }
        }

        function fnSelectTrancheAll(chkSelectAllTranche, chkSelectTranche)        //
        {
            document.getElementById('<%= hdnIsChange.ClientID %>').value = "1";
            var gvTranche = document.getElementById('<%=gvTrancheDtl.ClientID%>');
            var TargetChildControl = chkSelectTranche;
            //Get all the control of the type INPUT in the base control.
            var Inputs = gvTranche.getElementsByTagName("input");
            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                    Inputs[n].checked = chkSelectAllTranche.checked;

            var gvRSDtls = document.getElementById('<%= gvRSDtls.ClientID%>');

            for (var i = 1 ; i < gvTranche.rows.length; i++) {
                var TrancheID = gvTranche.rows[i].cells[1].children[0].innerText;
                for (var j = 1 ; j < gvRSDtls.rows.length; j++) {
                    var RSTrancheID = gvRSDtls.rows[j].cells[1].all[0].innerText.toString();
                    if (parseInt(RSTrancheID) == parseInt(TrancheID))   //
                    {
                        var Inputs = gvRSDtls.getElementsByTagName("input");
                        Inputs[j].checked = chkSelectAllTranche.checked;
                    }
                }
            }

            fnChkIsRSSelectAll(chkSelectAllTranche);
        }

        function fnChkIsTransferSelectAll(chkSelect) {
            var gv = document.getElementById('<%= gvTrancheDtl.ClientID%>');
            var chk = document.getElementById('ctl00_ContentPlaceHolder1_gvTrancheDtl_ctl01_cbxSelectAllTransfer');

            if (chkSelect.checked == false) {
                chk.checked = false;
            }
            else {
                var gvRwCnt = gv.rows.length - 1;
                var ChcCnt = 0;
                for (var i = 0; i < gv.rows.length; ++i) {
                    var Inputs = gv.rows[i].getElementsByTagName("input");
                    for (var n = 0; n < Inputs.length; ++n) {
                        if (Inputs[n].type == 'checkbox') {
                            if (Inputs[n].checked == true)
                                ++ChcCnt;
                        }
                    }
                }
                if (ChcCnt == gvRwCnt)
                    chk.checked = true;
                else
                    chk.checked = false;
            }
        }

        function fnChkIsRSSelectAll(chkSelect) {
            var gv = document.getElementById('<%= gvRSDtls.ClientID%>');
            var chk = document.getElementById('ctl00_ContentPlaceHolder1_gvRSDtls_ctl01_cbxSelectAllRs');

            if (chkSelect.checked == false) {
                chk.checked = false;
            }
            else {
                var gvRwCnt = gv.rows.length - 1;
                var ChcCnt = 0;
                for (var i = 0; i < gv.rows.length; ++i) {
                    var Inputs = gv.rows[i].getElementsByTagName("input");
                    for (var n = 0; n < Inputs.length; ++n) {
                        if (Inputs[n].type == 'checkbox') {
                            if (Inputs[n].checked == true)
                                ++ChcCnt;
                        }
                    }
                }
                if (ChcCnt == gvRwCnt)
                    chk.checked = true;
                else
                    chk.checked = false;
            }
        }
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=GridViewContainer.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=GridViewContainer.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=GridViewContainer.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=GridViewContainer.ClientID %>').style.overflow = "scroll";
            }
        }
        function pageLoad(s, a) {
            //document.getElementById('<%=GridViewContainer.ClientID %>').style.width = parseInt(screen.width) - 260;
            //document.getElementById('<%=GridViewContainer.ClientID %>').style.overflow = "scroll";            
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
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" CssClass="stylePanel" GroupingText="Input Criteria"
                            Width="99%">
                            <table cellpadding="0" cellspacing="0" width="99%">

                                <tr>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" TabIndex="1" ToolTip="Line of Business">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvLOB" runat="server" ControlToValidate="ddlLOB"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Line of Business"
                                            InitialValue="0" ValidationGroup="VGLAS"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="15%">
                                        <asp:Label ID="lblLASDate" runat="server" CssClass="styleRequiredFieldLabel" Text="LAS Date"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox ID="txtLASDate" runat="server" Width="35%" ToolTip="LAS Date" AutoPostBack="true" OnTextChanged="txtLASDate_TextChanged"></asp:TextBox>
                                        <asp:Image ID="ImgLASDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" TargetControlID="txtLASDate" PopupButtonID="ImgLASDate"
                                            ID="CalendarLASDate" Enabled="True">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvLASDate" runat="server" ControlToValidate="txtLASDate"
                                            Display="None" ValidationGroup="VGLAS" CssClass="styleMandatoryLabel" SetFocusOnError="True"
                                            ErrorMessage="Enter LAS Date"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblLASNo" runat="server" CssClass="styleDisplayLabel" Text="LAS Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <asp:TextBox ID="txtLASNo" runat="server" ReadOnly="true" ToolTip="LAS Number"></asp:TextBox>
                                    </td>
                                    <td class="styleFieldLabel" width="15%">
                                        <asp:Label ID="lblLASStatus" runat="server" CssClass="styleDisplayLabel" Text="Status"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox ID="txtLASStatus" runat="server" ReadOnly="true" Width="50%" ToolTip="Status"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblLasType" runat="server" CssClass="styleReqFieldLabel" Text="LAS Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <asp:DropDownList ID="ddllastype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddllastype_SelectedIndexChanged"
                                            TabIndex="3" ToolTip="LAS Type">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVLASTYPE" runat="server" ControlToValidate="ddlLASType"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a LAS Type"
                                            InitialValue="0" ValidationGroup="VGLAS"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblEntityType" runat="server" CssClass="styleDisplayLabel" Text="Buyer Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <asp:DropDownList ID="ddlEntitytype" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEntitytype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEntityType" runat="server" ControlToValidate="ddlEntitytype"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Buyer Type"
                                            InitialValue="0" ValidationGroup="VGLAS"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr id="trTransfer" runat="server">
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblLesseeName" runat="server" CssClass="styleReqFieldLabel" Text="Lessee Name"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <%--<uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetCustomerList" AutoPostBack="true" Width="300px"
                                            ValidationGroup="VGLAS" IsMandatory="true" ErrorMessage="Enter a Lessee Name" OnItem_Selected="ddlLesseeName_Item_Selected" />--%>

                                        <asp:TextBox ID="txtLessee" runat="server" Visible="true" Width="280px" AutoPostBack="true"
                                            OnTextChanged="txtLessee_TextChanged"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="aceLessee" MinimumPrefixLength="3" OnClientPopulated="Lessee_ItemPopulated"
                                            OnClientItemSelected="Lessee_ItemSelected" runat="server" TargetControlID="txtLessee"
                                            ServiceMethod="GetCustomerList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </cc1:AutoCompleteExtender>
                                        <cc1:TextBoxWatermarkExtender ID="txtLesseeWatermarkExt" runat="server" TargetControlID="txtLessee"
                                            WatermarkText="--Select--">
                                        </cc1:TextBoxWatermarkExtender>
                                        <asp:HiddenField ID="hdnLessee" runat="server" />
                                        <asp:RequiredFieldValidator ID="rfvLeseeName" runat="server" ControlToValidate="txtLessee"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter the Lessee Name" ValidationGroup="VGLAS">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblTrancheName" runat="server" CssClass="styleDisplayLabel" Text="Tranche Name"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <%-- <uc2:Suggest ID="ddlTrancheName" runat="server" ServiceMethod="GetTrancheList" AutoPostBack="true" Width="220px"
                                            ValidationGroup="VGLAS" IsMandatory="false" ErrorMessage="Enter a Tranche Name" OnItem_Selected="ddlTrancheName_Item_Selected" />--%>

                                        <asp:TextBox ID="txtTrancheName" runat="server" Visible="true" Width="280px" AutoPostBack="true"
                                            OnTextChanged="txtTrancheName_TextChanged"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="aceTranche" MinimumPrefixLength="3" OnClientPopulated="Tranche_ItemPopulated"
                                            OnClientItemSelected="Tranche_ItemSelected" runat="server" TargetControlID="txtTrancheName"
                                            ServiceMethod="GetTrancheList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </cc1:AutoCompleteExtender>
                                        <cc1:TextBoxWatermarkExtender ID="tbweTranche" runat="server" TargetControlID="txtTrancheName" WatermarkText="--Select--">
                                        </cc1:TextBoxWatermarkExtender>
                                        <asp:HiddenField ID="hdnTranche" runat="server" />
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel" width="15%">
                                        <asp:Label ID="lblBranch" runat="server" CssClass="styleReqFieldLabel" Text="Location" Visible="false"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign " width="25%">
                                        <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                            OnItem_Selected="ddlBranch_SelectedIndexChanged" ValidationGroup="VGLAS" IsMandatory="true" Visible="false"
                                            ErrorMessage="Enter a Location" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="18%">
                                        <asp:Label ID="lblRSNo" runat="server" CssClass="styleReqFieldLabel" Text="Rental Schedule Number" Visible="false"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="20%">
                                        <uc2:Suggest ID="ddlTPAN" runat="server" ServiceMethod="GetPANUM" AutoPostBack="true"
                                            OnItem_Selected="ddlTPAN_SelectedIndexChanged" ValidationGroup="VGLAS" IsMandatory="true" Visible="false"
                                            ErrorMessage="Enter a Rental Schedule Number" />
                                    </td>
                                    <td class="styleFieldLabel" width="15%">
                                        <asp:Label ID="lblClosureDate" runat="server" CssClass="styleDisplayLabel" Text="RS Closure Date" Visible="false"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="25%">
                                        <asp:TextBox ID="txtTAcClosure" runat="server" ReadOnly="True" ToolTip="A/C Closure Date" Width="50%" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnIsChange" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlLASTransfer" runat="server" CssClass="stylePanel" GroupingText="Lease Asset Transfer"
                            Width="99%" Visible="False">
                            <table cellpadding="0" cellspacing="0" width="99%">
                            </table>
                        </asp:Panel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Panel GroupingText="Buyer Information" ID="pnlLesseeInfo" runat="server" CssClass="stylePanel">
                            <table width="100%">
                                <tr>
                                    <td align="left" width="29%">
                                        <asp:Label runat="server" ToolTip="Lessee Name" Text="Buyer Code" ID="lblCode"
                                            CssClass="styleMandatoryLabel"></asp:Label>
                                    </td>
                                    <td align="left" width="71%">
                                        <asp:TextBox ID="txtCustomerCode" ToolTip="Buyer Code" runat="server"
                                            Style="display: none;" MaxLength="50"></asp:TextBox>
                                        <uc3:LOV ID="ucCustomerCodeLov" onblur="fnLoadCustomer()" runat="server" strLOV_Code="CMD"
                                            DispalyContent="Code" />
                                        <asp:Button ID="btnCreateCustomer"
                                            runat="server" Text="Create" Style="display: none;" CssClass="styleSubmitButton" OnClick="btnCreateCustomer_Click"
                                            CausesValidation="false" />
                                        <asp:RequiredFieldValidator ID="rfvcmbCustomer" runat="server" ControlToValidate="txtCustomerCode"
                                            ErrorMessage="Select a Buyer" ValidationGroup="VGLAS" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Panel ID="pnlAddressDetails" runat="server" Width="84%">
                                            <uc1:S3GCustomerAddress ID="ucCustomerAddress" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlRSDtls" runat="server" CssClass="stylePanel" GroupingText="Rental Schedule" Width="99%">
                            <table width="100%">
                                <tr>
                                    <td width="40%" valign="top">
                                        <asp:GridView ID="gvTrancheDtl" runat="server" AutoGenerateColumns="false" Width="100%">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblTransferSelectAll" runat="server" Text="Select ALL"></asp:Label>
                                                        <center>
                                                            <asp:CheckBox ID="cbxSelectAllTransfer" runat="server" OnClick="javascript:fnSelectTrancheAll(this, 'chkgdtrnSelect');" />
                                                        </center>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkgdtrnSelect" runat="server" OnClick="javascript:fnSelectTransfer(this);"
                                                            Checked='<%# Eval("Is_Checked").ToString() == "1" ?  true:false %>'></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tranche Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdtrnTrancheID" runat="server" Text='<%#Bind("Tranche_ID") %>' Style="display: none"></asp:Label>
                                                        <asp:Label ID="lblgdtrnTrancheName" runat="server" Text='<%#Bind("Tranche_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="70%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                    <td width="60%" valign="top">

                                        <asp:GridView ID="gvRSDtls" runat="server" AutoGenerateColumns="false" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblSelectAll" runat="server" Text="Select ALL"></asp:Label>
                                                        <center>
                                                            <asp:CheckBox ID="cbxSelectAllRs" runat="server" OnClick="javascript:fnCbxSelectAll(this,'cbxSelectRS');" />
                                                        </center>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:CheckBox ID="cbxSelectRS" runat="server" OnClick="javascript:fnChkRs(this);"
                                                                Checked='<%# Eval("Is_Checked").ToString() == "1" ?  true:false %>'></asp:CheckBox>
                                                        </center>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                    <HeaderStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RS ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdPaSaRefID" runat="server" Text='<%#Bind("Pa_Sa_Ref_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdRSTranchID" runat="server" Text='<%#Bind("Tranche_ID") %>' Style="display: none"></asp:Label>
                                                        <asp:Label ID="lblgdRSNo" runat="server" Text='<%#Bind("RS_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="40%" />
                                                    <HeaderStyle Width="40%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RS Closure Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdRSClosureDate" runat="server" Text='<%#Bind("RS_Closure_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                    <HeaderStyle Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnMoveTransferDtl" runat="server" Text="Move" OnClick="btnMoveTransferDtl_Click" CssClass="styleGridShortButton" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMapInvoiceDtls" runat="server" CssClass="stylePanel" GroupingText="Map Invoice" ScrollBars="Auto">
                            <table width="99%">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkDspManual" runat="server" Text="Manual" AutoPostBack="true" OnCheckedChanged="chkDspManual_CheckedChanged" />
                                        &nbsp;
                                        <asp:CheckBox ID="chkDspUpload" runat="server" Text="Upload" AutoPostBack="true" OnCheckedChanged="chkDspUpload_CheckedChanged" />
                                        &nbsp;
                                        <asp:Label ID="lblAssetFlag" runat="server" CssClass="styleReqFieldLabel" Text="Asset Flag"></asp:Label>
                                        &nbsp;
                                        <asp:DropDownList ID="ddlAssetFlag" runat="server" ToolTip="Asset Flag">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Scheduled Assets" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Non-Scheduled Assets" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlAssetFlag"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select a Asset Flag"
                                            InitialValue ="0" ValidationGroup="VGLAS"></asp:RequiredFieldValidator>--%>
                                        &nbsp;
                                        <asp:Label ID="lblDueDate" runat="server" Text="Due Date" CssClass="styleReqFieldLabel"/>
                                        &nbsp;
                                        <asp:TextBox ID="txtDueDate" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgDueDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalExtenderDueDate" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgDueDate"
                                            TargetControlID="txtDueDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ErrorMessage="Select the Due Date." ValidationGroup="VGLAS"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtDueDate"></asp:RequiredFieldValidator>
                                        &nbsp;
                                        <asp:CheckBox ID="chkTCS" runat="server" Text="TCS Not Applicable" />
                                    </td>
                                </tr>
                            </table>
                            <table width="99%">
                                <tr>
                                    <td>
                                        <table id="tblManual" runat="server" width="99%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblmapSrchLessee" runat="server" Text="Lessee Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlmapsrchLessee" runat="server" ServiceMethod="GetCustomerList" Width="250px" ErrorMessage="Select Lessee Name"
                                                        IsMandatory="true" ValidationGroup="LASExport" AutoPostBack="true" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLATNo" runat="server" Text="Lease Asset Transfer No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlLATNo" runat="server" ServiceMethod="GetLATNoList" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblmapSrchTranche" runat="server" Text="Tranche Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlmapSrchTranche" runat="server" ServiceMethod="GetSrchTrancheList" Width="250px" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblmapSrchAstCategory" runat="server" Text="Asset Category"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlmapSrchAsset" runat="server" ServiceMethod="GetAssetList" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblmapSrchAssetType" runat="server" Text="Asset Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlmapSrchAssetType" runat="server" ServiceMethod="GetAssetTypeList" Width="250px" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblmapSrchAssetSubType" runat="server" Text="Asset Sub Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlmapSrchAssetSubType" runat="server" ServiceMethod="GetAssetSubTypeList" Width="250px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="btnExport" runat="server" Text="LAS Upload Extract" ValidationGroup="LASExport" OnClick="btnExport_Click" CssClass="styleSubmitButton" />
                                                    <asp:Button ID="btnGoInvoice" runat="server" Text="Go" OnClick="btnGoInvoice_Click" CssClass="styleGridShortButton" />
                                                    <asp:Button ID="btnInvClear" runat="server" Text="Clear" OnClick="btnInvClear_Click" CssClass="styleGridShortButton" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="tblUpload" runat="server" width="99%">
                                            <tr id="trDispose" runat="server">
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" Text="File Name" ID="lblfilename" CssClass="styleDisplayLabel"> 
                                                    </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" colspan="2">
                                                    <asp:Label ID="lblCurrentPath" runat="server" Visible="false" Text="" />
                                                    <asp:HiddenField ID="hdnSelectedPath" runat="server" />
                                                    <asp:Button ID="btnBrowse" runat="server" OnClick="btnBrowse_Click" CssClass="styleSubmitButton" Style="display: none"></asp:Button>
                                                    <asp:CheckBox ID="chkSelect" runat="server" Text="" Enabled="false" />
                                                    <asp:FileUpload ID="flUpload" runat="server" CssClass="upload" />
                                                    <asp:Button CssClass="styleSubmitButton" ID="btnDlg" runat="server" Text="Browse"
                                                        Style="display: none" CausesValidation="false"></asp:Button>
                                                    <asp:ImageButton ID="hyplnkView" OnClick="hyplnkView_Click" ImageUrl="~/Images/spacer.gif" CommandArgument=""
                                                        CssClass="styleGridEditDisabled" runat="server" Enabled="false" />
                                                    <asp:Button ID="btnUpload" runat="server" CssClass="styleGridShortButton" Text="Upload"
                                                        ToolTip="Upload" OnClick="btnUpload_Click" />
                                                    <asp:Label ID="lblExcelCurrentPath" runat="server" Visible="true" Text="" ForeColor="Red" />
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkbtnDownloadTemplate" runat="server" Text="Download Upload Format"
                                                        OnClick="lnkbtnDownloadTemplate_Click"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="center">
                                                    <asp:Button ID="btnValidate" runat="server" Text="Validate" CssClass="styleGridShortButton" OnClick="btnValidate_Click" />
                                                    <asp:Button ID="btnMoveUpload" runat="server" Text="Go" CssClass="styleGridShortButton" OnClick="btnMoveUpload_Click" />
                                                    <asp:Button ID="btnException" runat="server" Text="Exception" CssClass="styleGridShortButton" OnClick="btnException_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <span runat="server" id="lblUploadErrorMsg" style="color: Red; font-size: medium"></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table width="99%">
                                <tr>
                                    <td>
                                        <div style="overflow: scroll;" id="GridViewContainer" runat="server">
                                            <asp:GridView ID="gvMapInvoiceDtl" runat="server" AutoGenerateColumns="false" Width="100%" ShowFooter="true">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblgdDspHdrSelect" runat="server" Text="Select"></asp:Label>
                                                            <center>
                                                                <asp:CheckBox ID="chkDspAll" Style="display: none" runat="server" onclick="javascript:fnMapSelectAll(this,'ChkSelectInvoice');" />
                                                                <%--AutoPostBack="true" OnCheckedChanged="chkDspAll_CheckedChanged"--%>
                                                            </center>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkSelectInvoice" runat="server" Checked='<%# Eval("Chk_Selected").ToString() == "1" ?  true:false %>'
                                                                onclick="javascript:fnChkIsSelect(this);" />
                                                            <%--OnCheckedChanged="ChkSelectInvoice_CheckedChanged" AutoPostBack="true"--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapInvoiceID" runat="server" Text='<%#Bind("Invoice_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tranche Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapTrancheName" runat="server" Text='<%#Bind("Tranche_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RS No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapRSNo" runat="server" Text='<%#Bind("RS_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapVendorCode" runat="server" Text='<%#Bind("Vendor_Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapVendorName" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor State">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapVendorState" runat="server" Text='<%#Bind("Vendor_State") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PO Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapPONumber" runat="server" Text='<%#Bind("PO_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PO Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapPODate" runat="server" Text='<%#Bind("PO_Date") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PI Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapPINo" runat="server" Text='<%#Bind("PI_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PI Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapPIDate" runat="server" Text='<%#Bind("PI_Date") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VI Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapVINo" runat="server" Text='<%#Bind("VI_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VI Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapVIDate" runat="server" Text='<%#Bind("VI_Date") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice No" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapInvoiceNo" runat="server" Text='<%#Bind("Invoice_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PaSaRefID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapPaSaRefID" runat="server" Text='<%#Bind("Pa_Sa_Ref_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RS State">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapLocationID" runat="server" Text='<%#Bind("RS_State_ID") %>' Style="display: none"></asp:Label>
                                                            <asp:Label ID="lblmapRSState" runat="server" Text='<%#Bind("RS_State") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Category">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapAssetCategory" runat="server" Text='<%#Bind("Asset_Category") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapAssetType" runat="server" Text='<%#Bind("Asset_Type") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Sub Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapAssetSubType" runat="server" Text='<%#Bind("Asset_SubType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset serial No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapAssetSerialNo" runat="server" Text='<%#Bind("Asset_SerialNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avlbl Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapAvlblQty" runat="server" Text='<%#Bind("Invoice_Qty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFAvlblQty" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Avlbl Inv Amt">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAvlblInvAmt" runat="server" Text='<%#Bind("Invoice_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFAvlblInvAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sale Qty">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMapSaleQty" runat="server" Width="80px" Text='<%#Bind("Sale_Qty") %>' MaxLength="10"
                                                                Style="text-align: right;" onchange="fnCheckMapQty(this);"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftbeMapSaleQty" runat="server" TargetControlID="txtMapSaleQty" Enabled="true"
                                                                FilterType="Numbers">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sale Price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMapSalePrice" runat="server" Width="120px" Text='<%#Bind("Sale_Price") %>' MaxLength="20"
                                                                Style="text-align: right;" onchange="fnChkSalePrice(this);"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftbeMapSalePrice" runat="server" TargetControlID="txtMapSalePrice" Enabled="true"
                                                                FilterType="Custom,Numbers" ValidChars=".">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFSalePrice" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Selling State">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSellingStateID" runat="server" Text='<%#Bind("Selling_State_ID") %>' Style="display: none;"></asp:TextBox>
                                                            <asp:TextBox ID="txtmapSellingState" runat="server" Visible="true" Width="120px" Text='<%#Bind("Selling_State") %>' onblur="fnChangeState(this)"
                                                                onchange="fnIsModifyDSP();"></asp:TextBox>
                                                            <cc1:AutoCompleteExtender ID="aceSaleState" MinimumPrefixLength="3" OnClientPopulated="SaleState_ItemPopulated"
                                                                OnClientItemSelected="SaleState_ItemSelected" runat="server" TargetControlID="txtmapSellingState"
                                                                ServiceMethod="GetBranchList" Enabled="True" ServicePath="" CompletionSetCount="5"
                                                                CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                                CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                                ShowOnlyCurrentWordInCompletionListItem="true">
                                                            </cc1:AutoCompleteExtender>
                                                            <cc1:TextBoxWatermarkExtender ID="txtSaleStateWtrmrkExt" runat="server" TargetControlID="txtmapSellingState"
                                                                WatermarkText="--Select--">
                                                            </cc1:TextBoxWatermarkExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tax Type">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtmapTaxType" runat="server" Text='<%#Bind("Tax_Type") %>' Width="70px" Enabled="false"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C Form Appli" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbxmapCForm" runat="server" onclick="javascript:fnCheckCformAppl(this);" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="C Form No." Visible="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtmapCFormNo" runat="server" Width="100px" Text='<%#Bind("C_Form_No") %>' onkeyup="maxlengthfortxt(15);"
                                                                onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Buyer Branch">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMapBuyerBranch" runat="server" Width="150px" Height="40px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"
                                                                Text='<%#Bind("Buyer_Branch") %>' onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Buyer PIN Code">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBuyer_PIN_Code" runat="server" Width="25px" Height="40px" TextMode="SingleLine" onkeyup="maxlengthfortxt(8);"
                                                                Text='<%#Bind("Buyer_PIN_Code") %>' onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Buyer GSTIN">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBuyer_GSTIN" runat="server" Width="150px"
                                                                Text='<%#Bind("Buyer_GSTIN") %>' onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Ship From">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtShip_From" runat="server" Width="150px" Height="40px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"
                                                                Text='<%#Bind("Ship_From") %>' onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ship From Pin">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtShip_From_Pin" runat="server" Width="150px" onkeyup="maxlengthfortxt(10);"
                                                                Text='<%#Bind("Ship_From_Pin") %>' onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ship To">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtBuyer_Delivery_Address" runat="server" Width="150px" Height="40px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300);"
                                                                Text='<%#Bind("Buyer_Delivery_Address") %>' onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ship To Pin">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtShip_To_Pin" runat="server" Width="150px" onkeyup="maxlengthfortxt(10);"
                                                                Text='<%#Bind("Ship_To_Pin") %>' onchange="fnIsModifyDSP();"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RV Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblmapRVAmt" runat="server" Text='<%#Bind("RV_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFRVAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:HiddenField ID="hdnSaleStateID" runat="server" />
                                            <asp:HiddenField ID="hdnIsMapInvoiceChanged" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc4:PageNavigator ID="ucDisposedPaging" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <span runat="server" id="lblDspErrorMsg" style="color: Red; font-size: medium"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnSelectAll" runat="server" Text="Select All" OnClick="btnSelectAll_Click" CssClass="styleGridShortButton" />
                                        <asp:Button ID="btnMove" runat="server" Text="Move" OnClick="btnMove_Click" CssClass="styleGridShortButton" ValidationGroup="vgMove" />
                                        <asp:Button ID="btnMoveAll" runat="server" Text="Move All" OnClick="btnMoveAll_Click" CssClass="styleGridShortButton"
                                            ValidationGroup="vgMove" Visible="false" />
                                        <asp:Button ID="btnMapInvUpdate" runat="server" Text="Update" OnClick="btnMapInvUpdate_Click" CssClass="styleGridShortButton" />
                                        <asp:Button ID="btnViewTot" runat="server" Text="View Total" OnClick="btnViewTot_Click" CssClass="styleSubmitButton" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>

                        <asp:Panel ID="pnlLeaseGrid" runat="server" CssClass="stylePanel" GroupingText="Invoice Information"
                            Width="99%">

                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel" colspan="2" width="10%">
                                        <asp:Button ID="btnExportDspDtl" runat="server" Text="Export" CssClass="styleSubmitButton" OnClick="btnExportDspDtl_Click" />
                                    </td>
                                </tr>
                            </table>

                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:GridView ID="grvTransferDtl" runat="server" AutoGenerateColumns="false" Width="100%" HorizontalAlign="Center" ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="AccountInvoiceID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActInvID" runat="server" Text='<%#Bind("Account_Invoice_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="AssetCategoryID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetCatID" runat="server" Text='<%#Bind("Asset_Category_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Category Desc">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetCatDesc" runat="server" Text='<%#Bind("Asset_Category_Desc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VendorID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorID" runat="server" Text='<%#Bind("Vendor_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tranche Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdTrnsfrTranche" runat="server" Text='<%#Bind("Tranche_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblgdTrnsfrRSNo" runat="server" Text='<%#Bind("RS_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dealer Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorName" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVINumber" runat="server" Text='<%#Bind("Invoice_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVIDate" runat="server" Text='<%#Bind("Invoice_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrTotal" runat="server" Text="Total"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Invoice Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVIAmount" runat="server" Text='<%#Bind("Invoice_Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrInvAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="LBT Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbllbtAmount" runat="server" Text='<%#Bind("LBT_Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrLBTAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Purchase Tax">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPurchaseTax" runat="server" Text='<%#Bind("Purchase_Tax") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrPTAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Entry Tax">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEntryTax" runat="server" Text='<%#Bind("Entry_Tax") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrETAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reverse Charge">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReverseCharge" runat="server" Text='<%#Bind("Reverse_Charge") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrRCAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Schedule Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSchedule_Amount" runat="server" Text='<%#Bind("Schedule_Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrSchdAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Capitalisation Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCapitalize_Amount" runat="server" Text='<%#Bind("Capitalize_Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrCapitalizeAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="WDV Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRV_Amount" runat="server" Text='<%#Bind("RV_Amount") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblFtrRVAmt" runat="server" Text=""></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc4:PageNavigator ID="ucCustomPaging" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <span runat="server" id="lblPagingErrorMessage" style="color: Red; font-size: medium"></span>
                                    </td>
                                </tr>
                            </table>

                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel" width="10%">
                                        <asp:Label ID="lblAggregate" runat="server" Text="Aggregate Amount" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtAggregateAmt" runat="server" Text="" Style="text-align: right;" MaxLength="15"
                                            OnTextChanged="txtAggregateAmt_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftbeAggrAmt" runat="server" TargetControlID="txtAggregateAmt" Enabled="true"
                                            FilterType="Custom,Numbers" ValidChars=".">
                                        </cc1:FilteredTextBoxExtender>
                                        &nbsp;<asp:Button ID="btnAggregateAmt" runat="server" Text="Allocate" CssClass="styleSubmitButton"
                                            CausesValidation="False" OnClick="btnAggregateAmt_Click" />

                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlDspDtl" runat="server" CssClass="stylePanel" Width="100%">
                                        <div style="overflow: scroll;" id="divDspDtl" runat="server">
                                            <asp:GridView ID="grvDisPosedDtl" runat="server" AutoGenerateColumns="false" HorizontalAlign="Center" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspSNo" Text='<%#Bind("ROWNumber") %>' runat="server" Width="50px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Invoice ID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspInvoiceID" runat="server" Text='<%#Bind("Invoice_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tranche Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspTranch" runat="server" Text='<%#Bind("Tranch_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RS No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspRSNo" runat="server" Text='<%#Bind("RS_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVendorCode" runat="server" Text='<%#Bind("Vendor_Code") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVendorName" runat="server" Text='<%#Bind("Vendor_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor State">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVendorState" runat="server" Text='<%#Bind("Vendor_State") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PO Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDSPPONumber" runat="server" Text='<%#Bind("PO_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PO Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDSPPODate" runat="server" Text='<%#Bind("PO_Date") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PI Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspPINo" runat="server" Text='<%#Bind("PI_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PI Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspPIDate" runat="server" Text='<%#Bind("PI_Date") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VI Number">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVINo" runat="server" Text='<%#Bind("VI_Number") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VI Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVIDate" runat="server" Text='<%#Bind("VI_Date") %>' Width="80px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice Number" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspInvNo" runat="server" Text='<%#Bind("Invoice_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Category">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAstCtgry" runat="server" Text='<%#Bind("Asset_Category") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAstType" runat="server" Text='<%#Bind("Asset_Type") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Sub Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAstSubType" runat="server" Text='<%#Bind("Asset_SubType") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Asset Serail No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAssetSerialNo" runat="server" Text='<%#Bind("Asset_SerialNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="RS State">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspRSState" runat="server" Text='<%#Bind("RS_State") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Available Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAvlbQty" runat="server" Text='<%#Bind("Avlb_Quantity") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFAvlbQty" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Available Invoice Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAvlbl_Inv_Amt" runat="server" Text='<%#Bind("Avlbl_Inv_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFAvlbl_Inv_Amt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Saleable Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspSaleQty" runat="server" Text='<%#Bind("Dsp_Quantity") %>'></asp:Label>
                                                            <%--<asp:TextBox ID="txtDspQty" runat="server" Text='<%#Bind("Dsp_Quantity") %>' Style="text-align: right" MaxLength="15"
                                                            onblur="fnCheckDspQty(this);" Width="100px"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftbeDspQty" runat="server" TargetControlID="txtDspQty" FilterType="Numbers" Enabled="true"></cc1:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblFDspSaleQty" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle HorizontalAlign="Right" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sale Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspSalePrice" runat="server" Text='<%#Bind("Sale_Price") %>'></asp:Label>
                                                            <%--<asp:TextBox ID="txtDspSaleAmount" runat="server" Text='<%#Bind("Sale_Price") %>' Style="text-align: right" MaxLength="13"
                                                            onblur="fnCalculateVAT(this);" Width="100px"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="ftbeDspSalePrice" runat="server" TargetControlID="txtDspSaleAmount" FilterType="Custom,Numbers"
                                                            Enabled="true" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblDspFtrSaleAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Selling State">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspSaleStateID" runat="server" Text='<%#Bind("Sale_State_ID") %>' Style="display: none"></asp:Label>
                                                            <asp:Label ID="lblDspSaleStateDesc" runat="server" Text='<%#Bind("Sale_State_Desc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Tax Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspTaxType" runat="server" Text='<%#Bind("Tax_Type") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="C Form Appl." Visible="false">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbxCFormAppl" runat="server" Checked='<%# Eval("C_Form_Appl").ToString() == "1" ?  true:false %>'
                                                                Enabled="false" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="C Form Number" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDspCFormNo" runat="server" Text='<%#Bind("C_Form_No") %>' Width="120px" onkeyup="maxlengthfortxt(15);"
                                                                Enabled='<%# Eval("C_Form_Appl").ToString() == "1" ?  true:false %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Buyer Branch">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspBuyerBranch" runat="server" Text='<%#Bind("Buyer_Branch") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Buyer GSTIN">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBuyer_GSTIN" runat="server" Text='<%#Bind("Buyer_GSTIN") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%--    <asp:TemplateField HeaderText="VAT/CST %">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVATPerc" runat="server" Text='<%#Bind("VAT_CST_Perc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="VAT/CST Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVatAmt" runat="server" Text='<%#Bind("VAT_CST_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblDspFtrVATAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Addtional VAT/CST%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAddVatPerc" runat="server" Text='<%#Bind("Addtl_VAT_CST_Perc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Additional VAT/CST Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAddVatAmt" runat="server" Text='<%#Bind("Addtl_VAT_CST_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblDspFtrAddtVATAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>--%>

                                                    <asp:TemplateField HeaderText="GST %">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVATPerc" runat="server" Text='<%#Bind("GST_Perc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GST Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspVatAmt" runat="server" Text='<%#Bind("GST_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblDspFtrVATAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Addtional GST%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAddVatPerc" runat="server" Text='<%#Bind("Addtl_GST_Perc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Additional GST Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspAddVatAmt" runat="server" Text='<%#Bind("Addtl_GST_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblDspFtrAddtVATAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Residual Value">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspRSValue" runat="server" Text='<%#Bind("RV_Amt") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblDspFtrRVAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Profit/Loss">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDspProfitLoss" runat="server" Text='<%#Bind("Profit_Loss") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblDspFtrPLAmt" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblSlctAll" runat="server" Text="Select All"></asp:Label>
                                                            <asp:CheckBox ID="chkSelectAllInvoice" runat="server" onclick="javascript:fnSelectAll(this,'chkDspSelect');" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDspSelect" runat="server" />
                                                            <%--AutoPostBack="true" OnCheckedChanged="chkDspSelect_CheckedChanged"
                                                Checked='<%# Eval("Is_Checked").ToString() == "1" ?  true:false %>'--%>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lnkDspRemoveAll" runat="server" Text="Remove All" OnClick="lnkDspRemoveAll_Click"></asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDspRemove" runat="server" Text="Remove" OnClick="lnkDspRemove_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                       </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <uc4:PageNavigator ID="ucPageDispose" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <span runat="server" id="lblSalePgingErrMsg" style="color: Red; font-size: medium"></span>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsFactoringInvoiceLoadinf" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" Height="177px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="VGLAS" Width="500px" />

                        <asp:ValidationSummary ID="vsMove" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" Height="177px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="vgMove" Width="500px" />

                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" Height="177px" ShowMessageBox="false"
                            ShowSummary="true" ValidationGroup="LASExport" Width="500px" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators(VGLAS);"
                            CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" ValidationGroup="VGLAS"
                            TabIndex="11" ToolTip="Save" />
                        <asp:Button runat="server" ID="btnInvoicePrinting" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Invoice Printing" OnClick="btnInvoicePrinting_Click" ToolTip="Invoice Printing"
                            Visible="False" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                            TabIndex="12" ToolTip="Clear" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="PRDDC" CausesValidation="false"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" TabIndex="13" ToolTip="Cancel" />
                        &nbsp;<asp:Button ID="btnPrintSale" runat="server" Text="Print" Enabled="false" CssClass="styleSubmitButton"
                            CausesValidation="False" OnClick="btnPrintSale_Click" />

                        &nbsp;<asp:Button ID="btnPrintAnnexures" runat="server" Text="Print Annexures" Enabled="false" CssClass="styleSubmitButton"
                            CausesValidation="False" OnClick="btnPrintAnnexures_Click" />
                        <asp:HiddenField ID="hdnFlag" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBrowse" />
            <asp:PostBackTrigger ControlID="btnPrintAnnexures" />
            <asp:PostBackTrigger ControlID="lnkbtnDownloadTemplate" />
            <asp:PostBackTrigger ControlID="btnException" />
            <asp:PostBackTrigger ControlID="btnExportDspDtl" />
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:PostBackTrigger ControlID="btnPrintSale" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
