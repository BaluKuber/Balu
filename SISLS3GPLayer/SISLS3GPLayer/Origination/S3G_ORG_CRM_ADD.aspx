<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="S3G_ORG_CRM_ADD.aspx.cs" Inherits="Origination_S3G_ORG_CRM_ADD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/S3GCustomerAddress.ascx" TagName="CustomerDetails"
    TagPrefix="CD" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/S3GDynamicLOV.ascx" TagName="FLOV" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../App_Themes/S3GTheme_Blue/AutoSuggestBox.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../Scripts/jquery-1.3.2.min.js"></script>


    <script type="text/javascript" language="javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endRequest);
        var postbackElement;

        function initializeRequest(sender, args) {
            document.body.style.cursor = "wait";
            if (prm.get_isInAsyncPostBack()) {
                //debugger
                args.set_cancel(true);
            }
        }
        function endRequest(sender, args) {
            document.body.style.cursor = "default";
        }
        var tab;
        function pageLoad() {

            document.getElementById('ctl00_ContentPlaceHolder1_tcCRM_body').className = "";
            //tab = $find('ctl00_ContentPlaceHolder1_tcCRM');
            //tab.add_activeTabChanged(on_Change);
        }

        var index = 0;
        function on_Change(sender, e) {
            //debugger;
            var newindex = tab.get_activeTabIndex(index);
            if (newindex < 5) {
                fnSetAccordian(newindex + 1);
                //document.getElementById('ctl00_ContentPlaceHolder1_pnlLeadView').style.display = 'Block';
                //fnShowPopup('pnlLeadView');
                //document.getElementById('<%= btnLeadOk.ClientID %>').click();
            }
            else {
                document.getElementById('<%= btnLeadView.ClientID %>').click();
            }


            var strtabName = tab._tabs[newindex]._tab.outerText.trim();
            document.getElementById('<%= lblCRMDetail.ClientID %>').innerText = strtabName;
        }

        function fnLoadCustomer() {
            if (document.getElementById('<%= ddlType.ClientID %>').value == "0") {
                alert("Select at least one query type");
                document.getElementById('<%= ddlType.ClientID %>').focus();
                return false;
            }
            document.getElementById('<%= btnLoadCustomer.ClientID %>').click();

        }

        function fnSearchFocus() {
            if (document.getElementById('<%= ddlType.ClientID %>').value == "0") {
                alert("Select at least one query type");
                document.getElementById('<%= ddlType.ClientID %>').focus();
                return false;
            }
            return true;

        }

        function fnSetAccordian(numHeader) {
            if (document.getElementById('ctl00_ContentPlaceHolder1_hdnTab' + numHeader).value == "0") {
                document.getElementById('accHeader' + numHeader).click();
                for (var i = 1; i <= 6; i++) {
                    document.getElementById('ctl00_ContentPlaceHolder1_hdnTab' + i).value = "0";
                }
                //obj.className = "accordionHeader";
                document.getElementById('ctl00_ContentPlaceHolder1_hdnTab' + numHeader).value = "1";
            }
        }

        function fnResetAccordianIndex() {
            for (var i = 1; i <= 6; i++) {
                if (document.getElementById('ctl00_ContentPlaceHolder1_hdnTab' + i).value == "1") {
                    //document.getElementById('tdTab' + i).className = "accordionHeader";
                }
            }
        }

        function funUpValidate() {
            if (document.getElementById('<%= ddlType.ClientID %>') != null && document.getElementById('<%= ddlType.ClientID %>').value == "0") {
                alert("Select at least one query type");
                document.getElementById('<%= ddlType.ClientID %>').focus();
                return false;
            }
            if (document.getElementById('<%= hidCustomerId.ClientID %>').innerText == "") {
                alert("Select the search description");
                return false;
            }
            return true;
        }

        function SalesPerson_ItemSelected(sender, e) {
            var hdnSalesPersonID = $get('<%= hdnSalesPersonID.ClientID %>');
            hdnSalesPersonID.value = e.get_value();
        }
        function SalesPerson_ItemPopulated(sender, e) {
            var hdnSalesPersonID = $get('<%= hdnSalesPersonID.ClientID %>');
            hdnSalesPersonID.value = '';
        }

        function AcctManager1_ItemSelected(sender, e) {
            var hdnAcctManager1ID = $get('<%= hdnAcctManager1ID.ClientID %>');
            hdnAcctManager1ID.value = e.get_value();
        }
        function AcctManager1_ItemPopulated(sender, e) {
            var hdnAcctManager1ID = $get('<%= hdnAcctManager1ID.ClientID %>');
            hdnAcctManager1ID.value = '';
        }

        function AcctManager2_ItemSelected(sender, e) {
            var hdnAcctManager2ID = $get('<%= hdnAcctManager2ID.ClientID %>');
            hdnAcctManager2ID.value = e.get_value();
        }
        function AcctManager2_ItemPopulated(sender, e) {
            var hdnAcctManager2ID = $get('<%= hdnAcctManager2ID.ClientID %>');
            hdnAcctManager2ID.value = '';
        }

        function Branch_ItemSelected(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
            hdnBranchID.value = e.get_value();
        }
        function Branch_ItemPopulated(sender, e) {
            var hdnBranchID = $get('<%= hdnBranchID.ClientID %>');
            hdnBranchID.value = '';
        }

        function fnShowPopup(panel) {

            if (document.getElementById('ctl00_ContentPlaceHolder1_' + panel).style.display == 'block') {
                document.getElementById('ctl00_ContentPlaceHolder1_' + panel).style.display = 'none';
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_' + panel).style.display = 'Block';
            }
        }


        function fnSetProspectName(obj1, obj2) {
            document.getElementById(obj2).value = document.getElementById(obj1).value;
        }


        function uploadComplete(sender, args) {
            var objID = sender._inputFile.id.split("_");
            var obj = args._fileName.split("\\");
            objID = "<%= gvPRDDT.ClientID %>" + "_" + objID[5];
            if (document.getElementById(objID + "_myThrobber") != null) {
                document.getElementById(objID + "_myThrobber").innerText = args._fileName;
                document.getElementById(objID + "_hidThrobber").value = args._fileName;

                if (obj[obj.length - 1].length > 80) {
                    alert("File Name can't exceed more than 80 characters");
                    document.getElementById(objID + "_myThrobber").innerText = "";
                    document.getElementById(objID + "_hidThrobber").value = "";
                    return false;
                }
            }
        }

        function FunShowPath(input) {
            if (input != null) {
                var objID = input.id;
                var myThrobber = document.getElementById((input.id).replace('asyFileUpload', 'myThrobber'));
                if (myThrobber != null) {
                    if (myThrobber.innerText != "")
                        input.setAttribute('title', myThrobber.innerText);
                }
            }
        }

        function fnLoadPath(btnBrowse) {
            if (btnBrowse != null)
                document.getElementById(btnBrowse).click();
        }

        function fnAssignPath(btnBrowse, hdnPath) {
            if (btnBrowse != null)
                document.getElementById(hdnPath).value = document.getElementById(btnBrowse).value;
        }

        //function divexpandcollapse(divname) {
        //    debugger;
        //    var img = "img" + divname;
        //    if ($("#" + img).attr("src") == "../images/plusCo.png") {
        //        $("#" + img)
        //		.closest("tr")
        //		.after("<tr><td></td><td colspan = '100%'>" + $("#" + divname)
        //		.html() + "</td></tr>");
        //        $("#" + img).attr("src", "../images/MinusExp.png");
        //    } else {
        //        $("#" + img).closest("tr").next().remove();
        //        $("#" + img).attr("src", "../images/plusCo.png");
        //    }
        //}

        function changeTextBoxState(dropDown) {
            if (dropDown.value == "4" || dropDown.value == "5") {
                $('#<%= txtLeadRemarks.ClientID %>').removeAttr("disabled");
                $('#<%= txtLeadRemarks.ClientID %>').val("");
            }
            else {
                $('#<%= txtLeadRemarks.ClientID %>').attr("disabled", "disabled");
                $('#<%= txtLeadRemarks.ClientID %>').val("");
            }
        }

        function toggleSelectionGrid(source) {
            var isChecked = source.checked;
            $("#<%=grvCrossBorder.ClientID%> input[id*='chkCrossBorder']:checkbox").each(function (index) {
                $(this).attr('checked', false);
            });
            source.checked = isChecked;
        }

        function checkvalue(evt) {
            var e = window.event || evt;
            var charCode = e.which || e.keyCode;

            var Val = document.getElementById('<%=txtCntPerDesig.ClientID%>').value;
            if (Val.length < 1) {
                if (charCode >= 48 && charCode < 58) {
                    if (window.event) //IE
                        window.event.returnValue = false;
                }
            }
        }

        function checkvalue1(evt) {
            var e = window.event || evt;
            var charCode = e.which || e.keyCode;

            var Val = document.getElementById('<%=txtLeadContactDesg.ClientID%>').value;
            if (Val.length < 1) {
                if (charCode >= 48 && charCode < 58) {
                    if (window.event) //IE
                        window.event.returnValue = false;
                }
            }
        }

        function funCheckFirstLetterisNumeric(textbox, msg) {
            var FieldValue = new Array();
            FieldValue = textbox.value.trim();
            if (FieldValue.length > 0 && FieldValue.value != '') {
                var charcode = FieldValue.charCodeAt(0);
                if (charcode >= 48 && charcode < 58) {
                    alert(msg + ' name cannot begin with a number');
                    textbox.focus();
                    textbox.value = '';
                    event.returnValue = false;
                    return false;
                }
            }
        }

    </script>

    <style type="text/css">
        .ajax__calendar {
            z-index: 10002 !important;
        }
    </style>


    <table width="100%" align="center" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="stylePageHeading" colspan="4">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Follow Up Instructions" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td valign="top">
                        <table width="100%">
                            <tr style="display: none">
                                <td class="styleFieldLabel">
                                    <asp:Label runat="server" Text="FollowUp No" ID="lblFollowUpNo" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtFollowUpNo" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trHead" runat="server">
                                <td class="styleFieldLabel">
                                    <asp:UpdatePanel ID="updProspect" runat="server">
                                        <ContentTemplate>
                                            <asp:Label runat="server" Text="Search Type" ID="lblType" CssClass="styleReqFieldLabel" Width="75px"></asp:Label>

                                            <uc:FLOV ID="ucPopUp" runat="server" OnLoadCusotmer="btnLoadCustomer_OnClick" />
                                            <cc1:DropShadowExtender ID="dseProspect" runat="server" TargetControlID="pnlProspectView"
                                                Opacity=".4" Rounded="false" TrackPosition="true" />
                                            <asp:Panel ID="pnlProspectView" runat="server" Style="display: block; position: absolute"
                                                Width="800px" BackColor="White" Visible="false" Height="300px" ScrollBars="Horizontal">
                                                <table width="100%" class="styleMainTable" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="stylePageHeading" width="100%" colspan="2">
                                                            <asp:Label runat="server" ID="Label2" CssClass="styleDisplayLabel" Text="Prospect Details"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 20px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px" width="30%">
                                                            <asp:Label ID="lblName" runat="server" CssClass="styleReqFieldLabel" Text="Prospect Name"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-bottom: 0px" width="70%">
                                                            <asp:DropDownList ID="ddlTitle" runat="server" TabIndex="0">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTitle"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="vgProspect"
                                                                ErrorMessage="Select Prospect title" InitialValue="0"></asp:RequiredFieldValidator>
                                                            <asp:TextBox ID="txtProspectName" runat="server" MaxLength="50" Width="275px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtProspectName"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="vgProspect"
                                                                ErrorMessage="Enter the Prospect name"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px">
                                                            <asp:Label ID="lblCustomerType" runat="server" CssClass="styleDisplayLabel" Text="Lessee Type"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-bottom: 0px">
                                                            <asp:DropDownList ID="ddlCustomerType" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCustomerType" runat="server" ControlToValidate="ddlCustomerType"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Enquiry"
                                                                ErrorMessage="Select the Customer Type"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px">
                                                            <asp:Label ID="lblCompanyType" runat="server" CssClass="styleDisplayLabel" Text="Company Type"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-bottom: 0px">
                                                            <asp:DropDownList ID="ddlCompanyType" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCompanyType" runat="server" ControlToValidate="ddlCompanyType"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Enquiry"
                                                                ErrorMessage="Select the Company Type"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px">
                                                            <asp:Label ID="lblPostingGLCode" runat="server" CssClass="styleDisplayLabel" Text="Posting GL Code"
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-bottom: 0px">
                                                            <asp:DropDownList ID="ddlPostingGLCode" runat="server" Visible="false"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvPostingGLCode" runat="server" ControlToValidate="ddlPostingGLCode"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Enquiry"
                                                                ErrorMessage="Select the Posting GL Code"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px">
                                                            <asp:Label ID="lblIndustry" runat="server" CssClass="styleDisplayLabel" Text="Industry"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-bottom: 0px">
                                                            <asp:DropDownList ID="ddlIndustry" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvIndustry" runat="server" ControlToValidate="ddlIndustry"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Enquiry"
                                                                ErrorMessage="Select the Industry Type"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px">
                                                            <asp:Label ID="lblComAddress1" runat="server" CssClass="styleReqFieldLabel" Text="Address 1"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign" style="padding-bottom: 0px">
                                                            <asp:TextBox ID="txtComAddress1" runat="server" Width="60%" TextMode="MultiLine" onkeyup="maxlengthfortxt(60);" Height="60px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvComAddress1" runat="server" ControlToValidate="txtComAddress1" Enabled="true"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="true" ValidationGroup="vgProspect"
                                                                ErrorMessage="Enter the Address1"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px">
                                                            <asp:Label ID="lblComAddress2" runat="server" CssClass="styleDisplayLabel" Text="Address 2"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtCOmAddress2" runat="server" Width="60%" TextMode="MultiLine" onkeyup="maxlengthfortxt(60);" Height="60px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" style="padding-bottom: 0px">
                                                            <asp:Label ID="lblComcity" runat="server" CssClass="styleDisplayLabel" Text="City"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <cc1:ComboBox ID="txtComCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend">
                                                            </cc1:ComboBox>
                                                            <asp:RequiredFieldValidator ID="rfvComCity" runat="server" ControlToValidate="txtComCity"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Enquiry"
                                                                ErrorMessage="Enter the Prospect City" InitialValue="0"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label ID="lblComState" runat="server" CssClass="styleDisplayLabel" Text="State"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <%--<cc1:ComboBox ID="txtComState" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend">
                                                            </cc1:ComboBox>--%>
                                                            <asp:DropDownList ID="txtComState" runat="server"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvComState" runat="server" ControlToValidate="txtComState"
                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                ValidationGroup="Enquiry" ErrorMessage="Select the Prospect State"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label ID="lblComCountry" runat="server" CssClass="styleDisplayLabel" Text="Country"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <cc1:ComboBox ID="txtComCountry" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                                AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend" MaxLength="30"
                                                                Width="150px">
                                                            </cc1:ComboBox>
                                                            <asp:RequiredFieldValidator ID="rfvComCountry" runat="server" ControlToValidate="txtComCountry"
                                                                InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                                ValidationGroup="Enquiry" ErrorMessage="Enter the Prospect Country"></asp:RequiredFieldValidator>
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblCompincode" runat="server" CssClass="styleDisplayLabel" Text="Pincode"></asp:Label>
                                                            &nbsp;&nbsp;
                                                            <asp:TextBox ID="txtComPincode" runat="server" MaxLength="10" Width="34%"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtComPincode" runat="server" Enabled="True" FilterType="Numbers"
                                                                TargetControlID="txtComPincode">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvComPincode" runat="server" ControlToValidate="txtComPincode"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Enquiry"
                                                                ErrorMessage="Enter the Prospect Pincode"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblComTelephone" runat="server" CssClass="styleDisplayLabel" Text="Telephone"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtComTelephone" runat="server" MaxLength="12" Width="175px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtComTelephone" runat="server" Enabled="True"
                                                                FilterType="Custom,Numbers" TargetControlID="txtComTelephone"
                                                                ValidChars=" -">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvComTelephone" runat="server" ControlToValidate="txtComTelephone"
                                                                CssClass="styleMandatoryLabel" ErrorMessage="Enter the Telephone" Display="None" Enabled="false"
                                                                SetFocusOnError="true" ValidationGroup="vgProspect"></asp:RequiredFieldValidator>
                                                            &nbsp;&nbsp;
                                                            <asp:Label ID="lblComMobile" runat="server" CssClass="styleDisplayLabel" Text="Mobile"></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;
                                                            <asp:TextBox ID="txtComMobile" runat="server" MaxLength="12" Width="34%"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftxtComMobile" runat="server" Enabled="True" FilterType="Numbers"
                                                                TargetControlID="txtComMobile" ValidChars=" -">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblComEmail" runat="server" CssClass="styleDisplayLabel" Text="EMail Id"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtComEmail" runat="server" MaxLength="60" Width="60%"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="ftbeComEmail" runat="server" Enabled="True"
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtComEmail"
                                                                ValidChars=".@_">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RegularExpressionValidator ID="revEmailId" runat="server" ControlToValidate="txtComEmail"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="true" ValidationExpression="^[\w\.=-]+@[\w\.-]+\.[\w]{2,3}$"
                                                                ValidationGroup="vgProspect" ErrorMessage="Enter valid Email ID"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblComWebSite" runat="server" CssClass="styleDisplayLabel" Text="Web Site"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtComWebsite" runat="server" MaxLength="60" Width="60%"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="revComWebsite" runat="server" ControlToValidate="txtComWebsite"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationExpression="([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                                                ValidationGroup="Main"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblRefType" runat="server" Text="Reference Type" Visible="false"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlRefType" Visible="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRefType_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label ID="lblRefNumber" Visible="false" runat="server" Text="Reference Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlRefNumber" runat="server" Visible="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblConstitutionName" Text="Constitution" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:DropDownList ID="ddlConstitutionName" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvddlConstitutionName" runat="server" ControlToValidate="ddlConstitutionName"
                                                                CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" ValidationGroup="Main"
                                                                ErrorMessage="Select the Prospect Constitution" InitialValue="0"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label runat="server" ID="lblContactPerson" Text="Contact Person" CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtCntPer" runat="server" MaxLength="50" Style="text-align: left"
                                                                Width="60%"></asp:TextBox>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblContactPersonPhone" Text="Contact Person Phone No." CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtCntPerPh" runat="server" MaxLength="12" Style="text-align: left"
                                                                Width="175px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="fltCntPerPh" runat="server" Enabled="True"
                                                                FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtCntPerPh"
                                                                ValidChars=" -">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="styleFieldLabel" width="20%">
                                                            <asp:Label runat="server" ID="lblContactPerDesig" Text="Contact Person Desig." CssClass="styleDisplayLabel"></asp:Label>
                                                        </td>

                                                        <td class="styleFieldAlign">
                                                            <asp:TextBox ID="txtCntPerDesig" runat="server" MaxLength="50" Style="text-align: left"
                                                                onKeyPress="checkvalue();" onkeyup="checkvalue();" Width="175px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td align="right" style="padding-right: 25px">
                                                            <br />
                                                            <asp:Button runat="server" ID="btnProspectView" Text="Save" CssClass="styleGridShortButton"
                                                                OnClick="btnProspectView_Click" ValidationGroup="vgProspect" ToolTip="Save Prospect Details" />
                                                            <asp:Button runat="server" ID="btnProspectClear" Text="Clear" CssClass="styleGridShortButton"
                                                                OnClick="btnProspectClear_Click" ToolTip="Clear Prospect Details" />
                                                            <asp:Button runat="server" ID="btnProspectClose" Text="Close" CssClass="styleGridShortButton"
                                                                OnClick="btnProspectClose_Click" ToolTip="Close Prospect Details" />

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">
                                                            <asp:ValidationSummary ID="vgProspect" runat="server" ValidationGroup="vgProspect" CssClass="styleMandatoryLabel"
                                                                HeaderText="Correct the following validation(s):" ShowSummary="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="styleFieldLabel">
                                    <%-- <asp:UpdatePanel ID="updTop" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtName" runat="server" Width="120px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <fieldset id="fldButton" runat="server" class="accordionHeaderSelected" style="cursor: pointer; height: 9px; width: 13px; border-width: 1px; border-color: #bad4ff; border-style: solid; margin-top: 0px">
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="true" OnClick="btnGetLOV_Click" ToolTip="Search"
                                                        ImageUrl="../Images/search_blue.gif" Style="cursor: pointer; height: 14px; vertical-align: top; margin-top: -3px" />
                                                </fieldset>
                                            </td>
                                            <td style="padding-left: 5px">
                                                <%--<asp:Button ID="btnGetLOV" runat="server" Text="..." CausesValidation="true" OnClick="btnGetLOV_Click" />--%>
                                            </td>
                                            <td valign="top" style="padding-top: 8px">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <cc1:DropShadowExtender ID="dseCustomer" runat="server" TargetControlID="pnlCustomerView"
                                                            Opacity=".4" Rounded="false" TrackPosition="true" />
                                                        <%--<asp:Image ID="imgCustomer" runat="server" ImageUrl="../Images/search_blue.gif" onclick="fnShowPopup('pnlCustomerView');"
                                                            Style="cursor: pointer" />
                                                        <asp:ImageButton ID="imgProspect" runat="server" ImageUrl="../Images/search_blue.gif"
                                                            Style="cursor: pointer" Visible="false" OnClick="imgProspect_OnClick" />--%>
                                                        <asp:Image ID="imgCustomer" runat="server" ImageUrl="../Images/Edit.jpg" onclick="fnShowPopup('pnlCustomerView');"
                                                            Style="cursor: pointer; height: 20px" />
                                                        <asp:ImageButton ID="imgProspect" runat="server" ImageUrl="../Images/Edit.jpg"
                                                            Style="cursor: pointer; height: 20px" Visible="false" OnClick="imgProspect_OnClick" ToolTip="Create/View" />
                                                        <%--onclick="fnShowPopup('pnlProspectView');"--%>
                                                        <asp:Panel ID="pnlCustomerView" runat="server" Style="display: none; position: absolute"
                                                            Width="35%" BackColor="White">
                                                            <table width="100%" class="styleMainTable" border="1" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td class="stylePageHeading" width="100%">
                                                                        <asp:Label runat="server" ID="Label1" CssClass="styleDisplayLabel" Text="Lessee Details"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <CD:CustomerDetails ID="ucdCustomer" runat="server" ShowCustomerCode="true" FirstColumnStyle="styleFieldLabel"
                                                                            SecondColumnStyle="styleFieldAlign" />
                                                                        <asp:Label ID="hidCustomerId" runat="server" Style="display: none" />
                                                                        <br />
                                                                        <asp:Button ID="btnUp" runat="server" CssClass="styleGridShortButton" Style="float: right;"
                                                                            ToolTip="View Full Details" Text="View" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <cc1:ComboBox Visible="false" ID="ddlSearch" Width="180px" AutoPostBack="true" runat="server"
                                        AutoCompleteMode="Suggest" DropDownStyle="DropDownList" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                    </cc1:ComboBox>
                                    <asp:Button ID="btnLoadCustomer" runat="server" Style="display: none" Text="Load Customer"
                                        OnClick="btnLoadCustomer_OnClick" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <asp:UpdatePanel ID="updLead" runat="server">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="height: 30px">

                                            <cc1:TabContainer ID="tcCRM" runat="server" CssClass="styleTabPanel" Style="max-width: 520px"
                                                ScrollBars="None" TabStripPlacement="top" AutoPostBack="True"
                                                OnActiveTabChanged="tcCRM_ActiveTabChanged">
                                                <cc1:TabPanel ID="tpLeadDetails" runat="server" CssClass="tabpan" HeaderText="Lead Details"
                                                    Style="border: 0px;" BackColor="Red">
                                                    <HeaderTemplate>
                                                        Lead Details
                                                    </HeaderTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="tpTrackDetails" runat="server" CssClass="tabpan" HeaderText="Track Details"
                                                    Style="border-color: White" BackColor="Red">
                                                    <HeaderTemplate>
                                                        Track Details
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="tpDocumentDocument" runat="server" CssClass="tabpan" HeaderText="Documemt Details"
                                                    Style="border-color: White" BackColor="Red">
                                                    <HeaderTemplate>
                                                        Document Details
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="tpAccountDetails" runat="server" CssClass="tabpan" HeaderText="Rental Schedule Details"
                                                    Style="border-color: White" BackColor="Red">
                                                    <HeaderTemplate>
                                                        Rental Schedule Details
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="tpStatusDetails" runat="server" CssClass="tabpan" HeaderText="Status Details"
                                                    Style="border-color: White" BackColor="Red">
                                                    <HeaderTemplate>
                                                        Status Details
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>
                                                <cc1:TabPanel ID="tpGroupDetails" runat="server" CssClass="tabpan" HeaderText="Group Details"
                                                    Style="border-color: White" BackColor="Red">
                                                    <HeaderTemplate>
                                                        Group Details
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>

                                                <cc1:TabPanel ID="tpCrossBorder" runat="server" CssClass="tabpan" HeaderText="Cross Border Details"
                                                    Style="border-color: White" BackColor="Red">
                                                    <HeaderTemplate>
                                                        Cross Border Details
                                                    </HeaderTemplate>
                                                    <ContentTemplate>
                                                    </ContentTemplate>
                                                </cc1:TabPanel>

                                            </cc1:TabContainer>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel"></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <%--<asp:PostBackTrigger ControlID="btnLeadOk" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <table width="99%" align="center" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Lead Details" ID="lblCRMDetail" CssClass="styleDisplayLabel"> </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <table width="100%" cellpadding="0" cellspacing="0" class="styleMainTable" style="border-width: 2px">
                            <tr>
                                <td style="height: 10px">
                                    <asp:Panel ID="pnlFollowUp" runat="server" Width="99%" Visible="false" CssClass="stylePanel"
                                        Style="overflow: hidden">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="pnlAddFollow" GroupingText="" Width="99%" runat="server" BackColor="White"
                                                        Visible="false">
                                                        <table runat="server" id="tblFlwUp" class="styleLoginTable" width="100" cellpadding="0">
                                                            <tr>
                                                                <td style="height: 10px"></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblQuery" runat="server" Text="Query Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlQuery" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlQuery_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator Display="None" ID="rfvddlQuery" CssClass="styleMandatoryLabel"
                                                                        InitialValue="0" ErrorMessage="Select the Query Type" runat="server" ControlToValidate="ddlQuery"
                                                                        ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                                <td class="styleFieldLabel" width="15%">
                                                                    <asp:Label ID="lblDesc" runat="server" Text="Description" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td rowspan="2" width="35%">
                                                                    <asp:TextBox ID="txtDescription" Width="92%" Height="60px" MaxLength="300" onkeyup="maxlengthfortxt(500);"
                                                                        TextMode="MultiLine" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblFromType" runat="server" Text="From Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlFrom" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlFrom_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <uc2:LOV ID="ucFrom" runat="server" style="width: 50%" />
                                                                    <%--<asp:RequiredFieldValidator Display="None" ID="rfvddlFrom" CssClass="styleMandatoryLabel"
                                                                        InitialValue="0" ErrorMessage="Select the From Type" runat="server" ControlToValidate="ddlFrom"
                                                                        ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblToType" runat="server" Text="To Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlTo" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlTo_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <uc2:LOV ID="ucTo" style="width: 75%" runat="server" />
                                                                    <%-- <asp:RequiredFieldValidator Display="None" ID="rfvddlTo" CssClass="styleMandatoryLabel"
                                                                        InitialValue="0" ErrorMessage="Select the To Type" runat="server" ControlToValidate="ddlTo"
                                                                        ValidationGroup="vgOK" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblNotifyDate" runat="server" Text="Target Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtNotifyDt" Width="80px" runat="server"></asp:TextBox><asp:Image
                                                                        ID="imgNoDt" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                    <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtNotifyDt"
                                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="imgNoDt"
                                                                        ID="ceNotifyDt" Enabled="true">
                                                                    </cc1:CalendarExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblMode" runat="server" Text="Mode" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlMode" runat="server">
                                                                    </asp:DropDownList>
                                                                    <%--<asp:RequiredFieldValidator Display="None" ID="rfvMode" CssClass="styleMandatoryLabel"
                                                                        Enabled="false" InitialValue="0" ErrorMessage="Select the Mode" runat="server"
                                                                        ControlToValidate="ddlMode" ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleReqFieldLabel"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlStatus" Enabled="true" runat="server">
                                                                    </asp:DropDownList>
                                                                    <%--<asp:RequiredFieldValidator Display="None" ID="rfvStatus" CssClass="styleMandatoryLabel"
                                                                        InitialValue="0" ErrorMessage="Select the Status" runat="server" ControlToValidate="ddlStatus"
                                                                        ValidationGroup="vgOK" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" width="15%">
                                                                    <asp:Label ID="lblTicketNo" runat="server" Text="Ticket No" CssClass="styleDisplayLabel"></asp:Label>
                                                                    <asp:HiddenField runat="server" ID="hdnView" />
                                                                    <asp:HiddenField runat="server" ID="hdnTicketNo" />
                                                                </td>
                                                                <td width="35%">
                                                                    <asp:TextBox ID="txtTicketNo" Style="width: 50px" runat="server" ReadOnly="true"></asp:TextBox>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblDate" runat="server"
                                                                        Text="Date" CssClass="styleDisplayLabel"></asp:Label>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtDate" Style="width: 80px"
                                                                        runat="server" ReadOnly="true"></asp:TextBox>
                                                                    <asp:TextBox ID="txtTrackDetailID" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                                                </td>
                                                                <td></td>
                                                                <td align="right" style="padding-right: 10px">
                                                                    <asp:Button runat="server" ID="btnOK" ValidationGroup="vgOK" CssClass="styleGridShortButton"
                                                                        Text="Ok" OnClick="btnOK_Click" />
                                                                    &nbsp;<asp:Button runat="server" ID="btnCan" CssClass="styleGridShortButton" Text="Cancel"
                                                                        OnClick="btnCan_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <br />
                                                                    <asp:ValidationSummary runat="server" ID="vsOK" HeaderText="Correct the following validation(s):"
                                                                        CssClass="styleMandatoryLabel" ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgOK" />
                                                                    <asp:CustomValidator ID="cvOK" runat="server" CssClass="styleMandatoryLabel" Enabled="true"
                                                                        Display="Dynamic" Width="98%"></asp:CustomValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100%" align="left">
                                                    <table width="98%">
                                                        <tr>
                                                            <td width="70%">
                                                                <asp:Label ID="lblSTicketNo" runat="server" Text="Ticket Number" CssClass="styleDisplayLabel"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="txtSTicketNo" runat="server" Width="30px" ToolTip="Ticket NUmber"
                                                                    onkeypress="fnAllowNumbersOnly(true,false,this)" Style="text-align: right" MaxLength="3"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                                        ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txtSTicketNo"
                                                                        FilterType="Numbers" Enabled="True">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Label ID="lblSDate" runat="server" Text="Date" CssClass="styleDisplayLabel"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:TextBox ID="txtSDate" runat="server" Width="90px"></asp:TextBox><cc1:CalendarExtender
                                                                    ID="calStartDate" runat="server" Enabled="True" TargetControlID="txtSDate" PopupButtonID="imgStartDate">
                                                                </cc1:CalendarExtender>
                                                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="styleGridShortButton" CausesValidation="false"
                                                                    OnClick="btnGo_Click"></asp:Button>
                                                                <asp:Button ID="btnSClear" runat="server" OnClientClick="document.getElementById('ctl00_ContentPlaceHolder1_accPnl1_content_txtSTicketNo').value='';document.getElementById('ctl00_ContentPlaceHolder1_accPnl1_content_txtSDate').value='';"
                                                                    Text="Clear" CssClass="styleGridShortButton" CausesValidation="false" OnClick="btnSClear_Click"></asp:Button>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100%">
                                                    <div class="container" style="max-height: 220px; width: 100%; overflow-y: auto; overflow-x: hidden;">
                                                        <asp:GridView Width="98%" runat="server" ID="grvFollowUp" AutoGenerateColumns="False"
                                                            OnRowDataBound="grvFollowUp_RowDataBound">
                                                            <Columns>
                                                                <%-- Ticket no  --%>
                                                                <asp:TemplateField HeaderText="Tkt No" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtTicketNo" runat="server" Text='<%# Bind("TicketNo")%>'></asp:Label>
                                                                        <asp:Label ID="lblTrackLeadID" runat="server" Visible="false" Text='<%# Bind("Lead_ID")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                                <%-- Date  --%>
                                                                <asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtDate" runat="server" Text='<%#  FormatDate(Eval("Date").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                                                                </asp:TemplateField>
                                                                <%-- From  --%>
                                                                <asp:TemplateField HeaderText="From" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hidFromType" runat="server" Value='<%# Bind("From_Type")%>' />
                                                                        <asp:Label ID="hidFromUserName" runat="server" Text='<%# Bind("From_UserName")%>' />
                                                                        <asp:HiddenField ID="hidFromID" runat="server" Value='<%# Bind("From")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="13%" />
                                                                </asp:TemplateField>
                                                                <%-- To  --%>
                                                                <asp:TemplateField HeaderText="To" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hidToType" runat="server" Value='<%# Bind("To_Type")%>' />
                                                                        <asp:Label ID="hidToUserName" runat="server" Text='<%# Bind("To_UserName")%>'>
                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>
                                                                        <asp:HiddenField ID="hidToID" runat="server" Value='<%# Bind("To")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="13%" />
                                                                </asp:TemplateField>
                                                                <%-- Query Type  --%>
                                                                <asp:TemplateField HeaderText="Query Type" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hidQueryType" runat="server" Value='<%# Bind("QueryType")%>' />
                                                                        <asp:LinkButton ID="lnkQuery" runat="server" Text='<%# Bind("QueryTxt")%>' OnClick="btnView_Click"
                                                                            OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';">
                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:LinkButton>
                                                                        <asp:Label ID="txtQuery" runat="server" Text='<%# Bind("QueryTxt")%>'>
                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>
                                                                <%-- Description  --%>
                                                                <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <%--<div style="width:160px;height:50px; overflow-x: auto;">--%>
                                                                        <asp:Label ID="txtDescription" runat="server" Text='<%# Bind("Description")%>' Style="word-wrap: normal; word-break: break-all"></asp:Label><%--</div>--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                                </asp:TemplateField>
                                                                <%-- Notify Date  --%>
                                                                <asp:TemplateField HeaderText="Target Date" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtNotifyDate" runat="server" Text='<%# FormatDate(Eval("NotifyDate").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Mode  --%>
                                                                <asp:TemplateField HeaderText="Mode" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hidMode" runat="server" Value='<%# Bind("Mode")%>' />
                                                                        <asp:Label ID="txtMode" runat="server" Text='<%# Bind("ModeTxt")%>'>
                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Status  --%>
                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtStatus" runat="server" Text='<%# Bind("Status")%>'></asp:Label><asp:HiddenField
                                                                            ID="hidStatus" runat="server" Value='<%# Bind("Status_Code")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Add  --%>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3px">
                                                                    <HeaderTemplate>
                                                                        <asp:Button ID="btnAdd" runat="server" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                                                            Text="Add" CssClass="styleGridShortButton" CausesValidation="false" OnClick="btnAdd_Click"></asp:Button>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="btnRemove" runat="server" Visible="false" Text="Remove" OnClick="btnRemove_Click"
                                                                            CausesValidation="false"></asp:LinkButton><asp:HiddenField ID="hidVersionNo" runat="server"
                                                                                Value='<%# Bind("Version_No")%>' />
                                                                        <asp:LinkButton ID="btnView" runat="server" Text="Edit" Visible="false" OnClick="btnView_Click"
                                                                            CausesValidation="false"></asp:LinkButton><asp:HiddenField ID="IsMax" runat="server"
                                                                                Value='<%# Bind("IsMax")%>' />
                                                                        <asp:HiddenField ID="hidFollowup_Detail_ID" runat="server" Value='<%# Bind("Followup_Detail_ID")%>' />
                                                                        <asp:TextBox ID="hidToMailId" runat="server" Visible="false" Text='<%# Bind("ToMailId")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDocuments" runat="server" Width="99%" Visible="false" CssClass="stylePanel"
                                        Style="overflow: hidden">
                                        <asp:Panel ID="pnlAddDocument" runat="server" Width="99%" Visible="true" CssClass="stylePanel"
                                            Style="overflow: hidden">
                                            <asp:UpdatePanel ID="updDocuments" runat="server">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="styleFieldLabel" width="20%">
                                                                <asp:Label ID="lblDocumentType" runat="server" CssClass="styleDisplayLabel" Text="Document Type"></asp:Label>
                                                            </td>
                                                            <td width="30%">
                                                                <asp:DropDownList ID="ddlDocumentType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator Display="None" ID="rfvddlDocumentType" CssClass="styleMandatoryLabel"
                                                                    InitialValue="0" ErrorMessage="Select the Document Type" runat="server" ControlToValidate="ddlDocumentType"
                                                                    ValidationGroup="vgAddDoc" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td class="styleFieldLabel" width="20%">
                                                                <asp:Label ID="lblDocument" runat="server" CssClass="styleDisplayLabel" Text="Document"></asp:Label>
                                                            </td>
                                                            <td width="30%">
                                                                <asp:DropDownList ID="ddlDocument" runat="server" Style="width: 70%">
                                                                </asp:DropDownList>
                                                                <asp:CheckBox runat="server" ID="chkIsMandatory" Checked="true" TextAlign="Left"
                                                                    Enabled="false" />
                                                                <asp:RequiredFieldValidator Display="None" ID="rfvddlDocument" CssClass="styleMandatoryLabel"
                                                                    InitialValue="-1" ErrorMessage="Select the Document" runat="server" ControlToValidate="ddlDocument"
                                                                    ValidationGroup="vgAddDoc" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                <asp:RequiredFieldValidator Display="None" ID="rfvddlDocument1" CssClass="styleMandatoryLabel"
                                                                    ErrorMessage="Select the Document" runat="server" ControlToValidate="ddlDocument"
                                                                    ValidationGroup="vgAddDoc" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblCollectedBy" runat="server" Text="Collected By"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCollectedBy" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblcollectedDate" runat="server" Text="Collected Date" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCollectedDate" Width="80px" runat="server" MaxLength="12"></asp:TextBox>&nbsp;&nbsp;<asp:Image
                                                                    ID="imgtxtCollectedDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtCollectedDate"
                                                                    PopupButtonID="imgtxtCollectedDate" ID="cetxtCollectedDate" Enabled="true">
                                                                </cc1:CalendarExtender>
                                                                <asp:CheckBox runat="server" ID="chkIsCollected" />
                                                                <asp:Label ID="lblIsCollected" runat="server" Text="Collected" CssClass="styleDisplayLabel"></asp:Label>
                                                                <%--<asp:RequiredFieldValidator Display="None" ID="rfvCollectedDate" CssClass="styleMandatoryLabel"
                                                                    ErrorMessage="Enter the Collected Date" runat="server" ControlToValidate="txtCollectedDate"
                                                                    ValidationGroup="vgAddDoc" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblScannedBy" runat="server" Text="Scanned By"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlScannedBy" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblScannedDate" runat="server" Text="Scanned Date" CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtscannedDate" Width="80px" runat="server" MaxLength="12"></asp:TextBox>&nbsp;&nbsp;<asp:Image
                                                                    ID="imgtxtscannedDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtscannedDate"
                                                                    PopupButtonID="imgtxtscannedDate" ID="cetxtscannedDate" Enabled="true">
                                                                </cc1:CalendarExtender>
                                                                <%--<asp:RequiredFieldValidator Display="None" ID="rfvScannedDate" CssClass="styleMandatoryLabel"
                                                                    ErrorMessage="Enter the Scanned Date" runat="server" ControlToValidate="txtscannedDate"
                                                                    ValidationGroup="vgAddDoc" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" rowspan="2" valign="top">
                                                                <asp:Label ID="lblDocRemarks" runat="server" Text="Remarks"></asp:Label>
                                                            </td>
                                                            <td rowspan="2" valign="top">
                                                                <asp:TextBox ID="txtDocRemarks" Width="90%" MaxLength="200" Height="70px" onkeyup="maxlengthfortxt(200);"
                                                                    TextMode="MultiLine" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="styleFieldLabel" rowspan="2" valign="top">
                                                                <asp:Label ID="lblValue" runat="server" Text="Value"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:TextBox ID="txtValue" Width="70%" MaxLength="50"
                                                                    runat="server"></asp:TextBox>
                                                                <asp:TextBox ID="txtCRMDocumentID" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel" rowspan="2" valign="top"></td>
                                                            <td></td>
                                                            <tr>
                                                                <td class="styleFieldLabel">
                                                                    <asp:Label ID="lblUploadFile" runat="server" Text="File Upload"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:UpdatePanel ID="tempUpdate" runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:Label ID="lblActualPath" runat="server" Visible="false"></asp:Label>
                                                                            <asp:HiddenField ID="hdnSelectedPath" runat="server" />
                                                                            <asp:Button ID="btnBrowse" runat="server" OnClick="btnBrowse_OnClick" Style="display: none" />
                                                                            <table align="left" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td style="padding: 0px">
                                                                                        <asp:CheckBox ID="chkSelect" runat="server" Enabled="false" Text="" Width="20px" />
                                                                                    </td>
                                                                                    <td style="padding: 0px">
                                                                                        <asp:FileUpload ID="flUpload" runat="server" ToolTip="Upload File" Width="100px" />
                                                                                        <asp:Button ID="btnDlg" runat="server" CausesValidation="false" CssClass="styleGridShortButton" Style="display: none" Text="Browse" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:PostBackTrigger ControlID="btnBrowse" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="styleFieldLabel" valign="top">
                                                                    <asp:Label ID="lblCurrentPath" runat="server" Style="position: absolute; color: Green; vertical-align: top" />
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Button ID="btnDocAdd" runat="server" CssClass="styleGridShortButton" OnClick="btnDocAdd_Click"
                                                                        OnClientClick="return fnCheckPageValidators('vgAddDoc', false);" Text="Add" ValidationGroup="vgAddDoc" />
                                                                    <asp:Button ID="btnDocClear" runat="server" CssClass="styleGridShortButton" OnClick="btnDocClear_Click" Text="Clear" />
                                                                    <asp:Button ID="btnDocUpdate" runat="server" CssClass="styleGridShortButton" OnClick="btnDocUpdate_Click"
                                                                        OnClientClick="return fnCheckPageValidators('vgAddDoc', false);" Text="Update" Visible="false" ValidationGroup="vgAddDoc" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                                                                        ShowMessageBox="false" ShowSummary="true" ValidationGroup="vgAddDoc" />
                                                                </td>
                                                            </tr>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="ddlDocumentType" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                        <div class="container" style="max-height: 200px; width: 100%; overflow-y: auto; overflow-x: hidden;">
                                            <asp:GridView ID="gvPRDDT" runat="server" AutoGenerateColumns="False" Width="98%"
                                                BorderColor="Gray" DataKeyNames="Doc_Cat_ID" CssClass="styleInfoLabel" Visible="true"
                                                OnRowDataBound="gvPRDDT_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="PRDDC TypeId" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPRTID" runat="server" Text='<%# Bind("Doc_Cat_ID") %>'></asp:Label>
                                                            <asp:Label ID="lblDocumentID" runat="server" Text='<%# Bind("Documents_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" Visible="false">
                                                        <ItemTemplate>
                                                            <%--OnCheckedChanged="rdHSelect_CheckedChanged"--%>
                                                            <asp:RadioButton ID="rdSelect" runat="server" Checked="false"
                                                                AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Doc_Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Value" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="CollectedById" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCollectedBy" runat="server" Text='<%# Bind("Collected_By") %>'></asp:Label>
                                                            <asp:Label ID="lblIsCollected" runat="server" Text='<%# Bind("IS_Collected") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Collected By" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCollectedByName" runat="server" Text='<%# Bind("Collected_By_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Collected Date" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCollectedDate" runat="server" Text='<%# Bind("Collected_Date") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ScannedById" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScannedBy" runat="server" Text='<%# Bind("Scanned_By") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Scanned By" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScannedByName" runat="server" Text='<%# Bind("Scanned_By_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Scanned Date" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScannedDate" runat="server" Text='<%# Bind("Scanned_Date") %>'></asp:Label>
                                                            <asp:Label ID="lblDocPath" runat="server" Text='<%# Bind("Document_Path") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View Document">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCanView" runat="server" Text='<%# Bind("ViewDoc") %>' Visible="false"></asp:Label>
                                                            <asp:ImageButton ID="hyplnkView" CommandArgument='<%# Bind("Scanned_Ref_No") %>'
                                                                OnClick="hyplnkView_Click" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEditDisabled"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks")%>'></asp:Label>
                                                            <asp:Label ID="lblProgramName" runat="server" Visible="false" Text='<%# Eval("ProgramName")%>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CbxCheck" runat="server" Enabled="false" Checked='<%# Eval("Is_Scanned").ToString() == "1" ? true : false %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Editable" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDocEditable" runat="server" Visible="true" Text='<%# Eval("Is_Editable")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnDocEdit" runat="server" Text="Edit" OnClick="lnkbtnDocEdit_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remove">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkbtnDocRemove" runat="server" Text="Remove" OnClick="lnkbtnDocRemove_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlStatus" runat="server" Width="99%" Visible="false" CssClass="stylePanel"
                                        Style="overflow: hidden">
                                        <div class="container" style="height: 200px; width: 100%; overflow-y: auto; overflow-x: hidden;">
                                            <asp:GridView runat="server" ID="gvStatusDetails" AutoGenerateColumns="False" DataKeyNames="Status_ID" Width="100%" OnRowDataBound="gvStatusDetails_RowDataBound">
                                                <Columns>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 5%; border-style: solid; border-width: 1px;"></td>
                                                                    <td style="width: 20%; border-style: solid; border-width: 1px;">
                                                                        <asp:Label ID="lblScheduleLocationH" runat="server" Text="State"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%; border-style: solid; border-width: 1px;">
                                                                        <asp:Label ID="lblProposalNoH" runat="server" Text="Proposal No"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%; border-style: solid; border-width: 1px;">
                                                                        <asp:Label ID="lblRSNoH" runat="server" Text="R SNo"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10%; border-style: solid; border-width: 1px;">
                                                                        <asp:Label ID="lblStatusDate" runat="server" Text="Status Date"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%; border-style: solid; border-width: 1px;">
                                                                        <asp:Label ID="lblStatusRemarks" runat="server" Text="Remarks"></asp:Label>
                                                                    </td>
                                                                    <td style="border-style: solid; border-width: 1px; display: none;">
                                                                        <asp:Label ID="lblStatusIDH" runat="server" Text="Status ID" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td style="width: 5%;">
                                                                        <asp:ImageButton ID="imgOpenGrid" runat="server" ToolTip="Open" ImageUrl="~/Images/plusCo.png" OnClick="imgOpenGrid_Click" />
                                                                    </td>
                                                                    <td style="width: 20%;">
                                                                        <asp:Label ID="lblScheduleLocation" runat="server" Text='<%# Bind("Schedule_Location")%>' Style="text-align: left"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%;">
                                                                        <asp:Label ID="lblProposalNo" runat="server" Text='<%# Bind("Proposal_No")%>'></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%;">
                                                                        <asp:Label ID="lblRSNo" runat="server" Text='<%# Bind("Rental_Schedule_No")%>'></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10%;">
                                                                        <asp:Label ID="lblStatusDate" runat="server" Text='<%# Bind("Date")%>'></asp:Label>
                                                                    </td>
                                                                    <td style="width: 20%;">
                                                                        <asp:Label ID="lblStatusRemarks" runat="server" Text='<%# Bind("Remarks")%>'></asp:Label>
                                                                    </td>
                                                                    <td style="display: none;">
                                                                        <asp:Label ID="lblStatusID" runat="server" Visible="false" Text='<%# Bind("Status_ID")%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trStatusDetailsGrid" runat="server" visible="false">
                                                                    <td style="width: 20px;"></td>
                                                                    <td colspan="5">
                                                                        <asp:GridView runat="server" ID="gvDetailedStatus" AutoGenerateColumns="False" Width="100%">
                                                                            <Columns>

                                                                                <%-- Status Detailed ID  --%>
                                                                                <asp:TemplateField HeaderText="Status Detail ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblStatusDetailedID" runat="server" Text='<%# Bind("Status_Detail_ID")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                                                </asp:TemplateField>

                                                                                <%-- Program Name  --%>
                                                                                <asp:TemplateField HeaderText="Program Name" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblStatusProgramName" runat="server" Text='<%# Bind("Program_Name")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                                </asp:TemplateField>


                                                                                <%-- Document Refernce Number  --%>
                                                                                <asp:TemplateField HeaderText="Document Ref No" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDocumentRefNumber" runat="server" Text='<%# Bind("Document_Ref_No")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                                </asp:TemplateField>
                                                                                <%-- Status  --%>
                                                                                <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDetailedStatus" runat="server" Text='<%# Bind("Status")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                                </asp:TemplateField>

                                                                                <%-- Remarks  --%>
                                                                                <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtDetailedRemarks" runat="server" Text='<%# Bind("Remarks")%>' Width="90%" TextMode="MultiLine"
                                                                                            onkeyup="maxlengthfortxt(500);"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="50%" />
                                                                                </asp:TemplateField>
                                                                                <%-- Action  --%>
                                                                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Left">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkStatusEdit" runat="server" Text="Update" OnClick="lnkStatusEdit_Click"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                                                </asp:TemplateField>

                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </td>
                                                                    <td style="width: 20px; display: none;"></td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <%-- Status ID  --%>
                                                    <%--<asp:TemplateField HeaderText="Status ID" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                          
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>--%>

                                                    <%-- Schedule Location  --%>
                                                    <%--  <asp:TemplateField HeaderText="Schedule Location" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblScheduleLocation" runat="server" Text='<%# Bind("Schedule_Location")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>--%>

                                                    <%-- Proposal No  --%>
                                                    <%--  <asp:TemplateField HeaderText="Proposal No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProposalNo" runat="server" Text='<%# Bind("Proposal_No")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>--%>

                                                    <%-- Rental Schedule No  --%>
                                                    <%--  <asp:TemplateField HeaderText="Rental Schedule No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRSNo" runat="server" Text='<%# Bind("Rental_Schedule_No")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>--%>

                                                    <%-- Date  --%>
                                                    <%--<asp:TemplateField HeaderText="Date" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatusDate" runat="server" Text='<%# Bind("Date")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>--%>

                                                    <%-- Remarks  --%>
                                                    <%--  <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatusRemarks" runat="server" Text='<%# Bind("Remarks")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                    </asp:TemplateField>--%>
                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlAccount" runat="server" Width="99%" Visible="false" CssClass="stylePanel"
                                        Style="overflow: hidden">
                                        <table style="width: 100%" class="styleMainTable">
                                            <tr>
                                                <td align="center">
                                                    <div class="container" style="height: 200px; width: 100%; overflow-y: auto; overflow-x: hidden;">
                                                        <asp:GridView ID="grvMain" runat="server" AutoGenerateColumns="False" OnRowDataBound="grvMain_RowDataBound"
                                                            Width="100%">
                                                            <Columns>
                                                                <%-- LOB  --%>
                                                                <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtLob" runat="server" Text='<%# Bind("LOB")%>'></asp:Label><asp:Label
                                                                            ID="txtLobId" runat="server" Text='<%# Bind("LOB_ID")%>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                                </asp:TemplateField>
                                                                <%-- Branch  --%>
                                                                <asp:TemplateField HeaderText="State" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtBranch" runat="server" Text='<%# Bind("Location")%>'></asp:Label><asp:HiddenField
                                                                            ID="hidIsColor" runat="server" Value='<%# Bind("IsColor")%>' />
                                                                        <asp:Label ID="txtBranchId" runat="server" Text='<%# Bind("Location_Id")%>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                                </asp:TemplateField>
                                                                <%-- PrimeAccountNo  --%>
                                                                <asp:TemplateField HeaderText="Rental Schedule No" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtPrimeAccountNo" runat="server" Text='<%# Bind("PANum")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                                </asp:TemplateField>
                                                                <%-- SubAccountNo  --%>
                                                                <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtSubAccountNo" runat="server" Text='<%# Bind("SANum")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                                </asp:TemplateField>
                                                                <%-- Tranche Name  --%>
                                                                <asp:TemplateField HeaderText="Tranche Name" ItemStyle-HorizontalAlign="Left" Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRSTrancheName" runat="server" Text='<%# Bind("Tranche_Name")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                                </asp:TemplateField>
                                                                <%-- Select  --%>
                                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="false" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="7%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                            <EmptyDataTemplate>
                                                                <span>No Records Found...</span>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <br />
                                                    <asp:Button ID="btnDown" CssClass="styleGridShortButton" runat="server" ToolTip="Down"
                                                        Text="View" Visible="true" OnClick="btnDown_Click" CausesValidation="true" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';return fnCheckPageValidators('vgUp',false);"
                                                        ValidationGroup="vgUp" Style="float: right" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="100%">
                                                    <div class="container" style="max-height: 180px; overflow-y: auto; overflow-x: hidden;">
                                                        <asp:GridView runat="server" ID="grvAccountDetails" AutoGenerateColumns="False" OnRowDataBound="grvAccountDetails_RowDataBound">
                                                            <Columns>
                                                                <%-- Radion Button   --%>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkJE" Text="Ledger" runat="server" OnClick="lnkJE_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="4%" />
                                                                </asp:TemplateField>
                                                                <%-- LOB --%>
                                                                <%-- <asp:CommandField ShowSelectButton="True" Visible="false" />--%>
                                                                <asp:TemplateField HeaderText="Line of Business" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtLOBName" runat="server" Text='<%# Bind("LOB")%>'></asp:Label><asp:HiddenField
                                                                            ID="hidLOB" runat="server" Value='<%# Bind("LOB_ID")%>'></asp:HiddenField>
                                                                        <asp:HiddenField ID="hidBranch" runat="server" Value='<%# Bind("Location_ID")%>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- PANum  --%>
                                                                <asp:TemplateField HeaderText="Rental Schedule No" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtPANum" runat="server" Text='<%# Bind("PANum")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- SANum  --%>
                                                                <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtSANum" runat="server" Text='<%# Bind("SANum")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Begin Date  --%>
                                                                <asp:TemplateField HeaderText="Begin Date" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtBeginDate" runat="server" Text='<%# Bind("Creation_Date")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                                <%-- MatureDate   --%>
                                                                <asp:TemplateField HeaderText="Maturity Date" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtMatureDate" runat="server" Text='<%# Bind("MatureDate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                                <%-- Activation Date   --%>
                                                                <asp:TemplateField HeaderText="Activation Date" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAccActivationDate" runat="server" Text='<%# Bind("Activation_Date")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                                                </asp:TemplateField>
                                                                <%-- Finance Amount   --%>
                                                                <asp:TemplateField HeaderText="Finance Amount" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtFinanceAmount" runat="server" Text='<%# Bind("Finance_Amount")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- UMFC   --%>
                                                                <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtUMFC" runat="server" Text='<%# Bind("UMFC")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Billed Amount   --%>
                                                                <asp:TemplateField HeaderText="Billed Amount" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="txtBilledAmount" runat="server" Text='<%# Bind("BilledAmount")%>'
                                                                            OnClick="txtBilledAmount_Click"></asp:LinkButton><%--<cc1:PopupControlExtender ID="PopExBillAmt" runat="server" TargetControlID="txtBilledAmount"
                                                    PopupControlID="pnlNOD" Position="Bottom" />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Collected Amount   --%>
                                                                <asp:TemplateField HeaderText="Collected Amount" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="txtCollectedAmount" runat="server" Text='<%# Bind("CollectedAmount")%>'
                                                                            OnClick="txtCollectedAmount_Click"></asp:LinkButton><%--<cc1:PopupControlExtender ID="PopExColAmt" runat="server" TargetControlID="txtCollectedAmount"
                                                    PopupControlID="pnlNOD" Position="Bottom" />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- NOD   --%>
                                                                <asp:TemplateField HeaderText="NOD" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtNOD" runat="server" Text='<%# Bind("NOD")%>'></asp:Label><asp:LinkButton
                                                                            ID="lnkNOD" runat="server" Text='<%# Bind("NOD")%>' OnClick="lnkNOD_Click"></asp:LinkButton><%-- <cc1:PopupControlExtender ID="PopEx" runat="server" TargetControlID="lnkNOD" PopupControlID="pnlNOD"
                                                    Position="Bottom" />--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                                </asp:TemplateField>
                                                                <%-- O/S Amount   --%>
                                                                <asp:TemplateField HeaderText="O/S Amount" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtOSAmount" runat="server" Text='<%# Bind("OSAmount")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Future Principal   --%>
                                                                <asp:TemplateField HeaderText="Future Principal" ItemStyle-HorizontalAlign="Right">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtFuturePrincipal" runat="server" Text='<%# Bind("FuturePrincipal")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- LTV Amount   --%>
                                                                <asp:TemplateField HeaderText="LTV Amount" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtLTVAmount" runat="server" Text='<%# Bind("LTVAmount")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- Category Status   --%>
                                                                <asp:TemplateField HeaderText=" Category Status" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtCategoryStatus" runat="server" Text='<%# Bind("CategoryStatus")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                                <%-- DC   --%>
                                                                <asp:TemplateField HeaderText="DC" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="txtDC" runat="server" Text='<%# Bind("DC")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" Width="7%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <RowStyle HorizontalAlign="Center" />
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlLeadView" Style="display: block; margin-top: 0px; float: left; overflow-y: auto; position: absolute;"
                                        Width="98%" BackColor="White" Visible="false" Height="300px">
                                        <table width="99%" class="styleMainTable">
                                            <%-- <tr>
                                                <td class="stylePageHeading" width="700px" colspan="4">
                                                    <asp:Panel ID="pnlDrgLead" runat="server">
                                                        <asp:Label runat="server" ID="Label3" CssClass="styleDisplayLabel" Text="Lead Details"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td class="styleFieldLabel" width="100px">
                                                    <asp:Label ID="lblFinanceMode" runat="server" Text="Finance Mode" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" width="200px">
                                                    <asp:DropDownList ID="ddlFinanceMode" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLeadFinanceMode" runat="server" ControlToValidate="ddlFinanceMode"
                                                        InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                        ValidationGroup="LeadAdd" ErrorMessage="Select the Finance Mode"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel" width="100px">
                                                    <asp:Label ID="lblLLeadID" runat="server" Text="LeadID" CssClass="styleDisplayLabel" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" width="200px">
                                                    <asp:TextBox ID="txtLeadID" runat="server" Text="" ReadOnly="true" Visible="false"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                                                        InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                        ValidationGroup="LeadAdd" ErrorMessage="Select the Line of Business"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLocation" runat="server" Text="State" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtBranchSearch" runat="server" MaxLength="50" Width="182px"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="autoBranchSearch" MinimumPrefixLength="3" OnClientPopulated="Branch_ItemPopulated"
                                                        OnClientItemSelected="Branch_ItemSelected" runat="server" TargetControlID="txtBranchSearch"
                                                        ServiceMethod="GetBranchList" Enabled="True" ServicePath="" CompletionSetCount="2"
                                                        CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                        CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                        ShowOnlyCurrentWordInCompletionListItem="true">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="txtBranchSearchExtender" runat="server" TargetControlID="txtBranchSearch"
                                                        WatermarkText="--Select--">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <asp:HiddenField ID="hdnBranchID" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvLeadLocation" runat="server" ControlToValidate="txtBranchSearch"
                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                        ValidationGroup="LeadAdd" ErrorMessage="Enter the State"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadSourceType" runat="server" Text="Lead Source Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:DropDownList ID="ddlLeadSourceType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLeadSourceType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLeadSourceType" runat="server" ControlToValidate="ddlLeadSourceType"
                                                        InitialValue="0" CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                        ValidationGroup="LeadAdd" ErrorMessage="Select the Lead Source Type"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFundingType" runat="server" Text="Funding Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" width="200px">
                                                    <asp:DropDownList ID="ddlFundingType" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadSource" runat="server" Text="Lead Name" CssClass="styleDisplayLabel"></asp:Label>&nbsp;
                                                            <uc:FLOV ID="ucLead" runat="server" OnLoadCusotmer="btnLeadSource_OnClick" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtLeadSource" runat="server" Width="120px" ReadOnly="true"></asp:TextBox>
                                                    <%--<asp:Button ID="btnGetSource" runat="server" Text="..." CausesValidation="true" OnClick="btnGetSource_Click" />--%>
                                                    <fieldset id="Fieldset1" runat="server" class="accordionHeaderSelected" style="cursor: pointer; margin-left: -5px; height: 9px; width: 13px; border-width: 1px; border-color: #bad4ff; border-style: solid; margin-top: 5px">
                                                        <asp:ImageButton ID="btnGetSource" runat="server" CausesValidation="true" OnClick="btnGetSource_Click" ToolTip="Search"
                                                            ImageUrl="../Images/search_blue.gif" Style="cursor: pointer; height: 14px; vertical-align: top; margin-top: -3px" />
                                                    </fieldset>
                                                    <asp:TextBox ID="txtOtherLead" runat="server" Visible="false" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadSalesPerson" runat="server" Text="Sales Person" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtSalesPerson" runat="server" MaxLength="50" Width="182px" Visible="false"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="autoSalePerson" MinimumPrefixLength="3" OnClientPopulated="SalesPerson_ItemPopulated"
                                                        OnClientItemSelected="SalesPerson_ItemSelected" runat="server" TargetControlID="txtSalesPerson"
                                                        ServiceMethod="GetSalesPersonList" Enabled="True" ServicePath="" CompletionSetCount="2"
                                                        CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                        CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                        ShowOnlyCurrentWordInCompletionListItem="true">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="tbwmeSalesPerson" runat="server" TargetControlID="txtSalesPerson"
                                                        WatermarkText="--Select--">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <asp:HiddenField ID="hdnSalesPersonID" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" valign="top">
                                                    <asp:Label ID="lblLeadInformation" runat="server" Text="Lead Information" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtLeadInformation" runat="server" TextMode="MultiLine" Width="300px" Height="70px" onkeyup="maxlengthfortxt(200);"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel" valign="top">
                                                    <asp:Label ID="lblCompetitorInfo" runat="server" Text="Competitor Info"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtCompetitorInfo" runat="server" TextMode="MultiLine" Width="300px" Height="70px" onkeyup="maxlengthfortxt(200);"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountManager1" runat="server" Text="Account Manager1" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtAcctManager1" runat="server" MaxLength="50" Width="182px"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="aceAcctManager1" MinimumPrefixLength="3" OnClientPopulated="AcctManager1_ItemPopulated"
                                                        OnClientItemSelected="AcctManager1_ItemSelected" runat="server" TargetControlID="txtAcctManager1"
                                                        ServiceMethod="GetSalesPersonList" Enabled="True" ServicePath="" CompletionSetCount="2"
                                                        CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                        CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                        ShowOnlyCurrentWordInCompletionListItem="true">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="tbwmeAcctManager1" runat="server" TargetControlID="txtAcctManager1"
                                                        WatermarkText="--Select--">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <asp:HiddenField ID="hdnAcctManager1ID" runat="server" />
                                                    <asp:RequiredFieldValidator ID="rfvAcctManager1" runat="server" ControlToValidate="txtAcctManager1"
                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True"
                                                        ValidationGroup="LeadAdd" ErrorMessage="Enter the Account Manager 1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountManager2" runat="server" Text="Account Manager2"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtAcctManager2" runat="server" MaxLength="50" Width="182px"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="aceAcctManager2" MinimumPrefixLength="3" OnClientPopulated="AcctManager2_ItemPopulated"
                                                        OnClientItemSelected="AcctManager2_ItemSelected" runat="server" TargetControlID="txtAcctManager2"
                                                        ServiceMethod="GetSalesPersonList" Enabled="True" ServicePath="" CompletionSetCount="2"
                                                        CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                                        CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                                        ShowOnlyCurrentWordInCompletionListItem="true">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="tbwmeAcctManager2" runat="server" TargetControlID="txtAcctManager2"
                                                        WatermarkText="--Select--">
                                                    </cc1:TextBoxWatermarkExtender>
                                                    <asp:HiddenField ID="hdnAcctManager2ID" runat="server" />
                                                </td>

                                            </tr>

                                            <tr>

                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadContactName" runat="server" Text="Contact Person"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtLeadContactPerson" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftbeLeadContactPerson" runat="server" TargetControlID="txtLeadContactPerson"
                                                        Enabled="true" FilterType="LowercaseLetters,UppercaseLetters,Custom" ValidChars=". ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadContactNumber" runat="server" Text="Contact Person No"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtLeadContactNumber" runat="server" MaxLength="12"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="ftvLeadContactNumber" runat="server" TargetControlID="txtLeadContactNumber"
                                                        Enabled="true" FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadContactDesg" runat="server" Text="Contact Person Desg"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtLeadContactDesg" runat="server" MaxLength="50" onKeyPress="checkvalue1();"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadContactEmail" runat="server" Text="Email" MaxLength="50"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtLeadContactEmail" runat="server"></asp:TextBox>
                                                    <asp:DropDownList ID="ddlLeadConstitution" runat="server" Visible="false">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadStatus" runat="server" Text="Lead Status" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:DropDownList ID="ddlLeadStatus" runat="server" onChange="javascript:changeTextBoxState(this)"
                                                        OnSelectedIndexChanged="ddlLeadStatus_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLeadStatus" runat="server" ControlToValidate="ddlLeadStatus"
                                                        CssClass="styleMandatoryLabel" Display="None" SetFocusOnError="True" InitialValue="0"
                                                        ValidationGroup="LeadAdd" ErrorMessage="Select the Lead Status"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountStatus" runat="server" Text="Account Status" Visible="true"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:DropDownList ID="ddlAccountStatus" runat="server" Visible="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLeadRemarks" runat="server" Text="Rejected Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel" colspan="3">
                                                    <asp:TextBox ID="txtLeadRemarks" runat="server" TextMode="MultiLine" Width="500px" Height="40px" onkeyup="maxlengthfortxt(120);" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCusotmerStatus" runat="server" Text="Lessee Status" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:DropDownList ID="ddlCustomerStatus" runat="server" Visible="false">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtCustomerStatus" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                                </td>
                                                <td colspan="2"></td>
                                            </tr>

                                            <tr>
                                                <td colspan="4" valign="top">
                                                    <%-- <div style="max-height: 200px; width: 98%; height: 200px; overflow-y: auto; overflow-x: auto;">--%>
                                                    <asp:GridView ID="grvAssets" runat="server" AutoGenerateColumns="False" ShowFooter="true"
                                                        OnRowDeleting="grvAssets_DeleteClick" Width="97%" Visible="false">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sl No" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSlNo" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                                                    <asp:Label ID="lblAssetCategoryID" runat="server" Text='<%# Bind("Asset_CategoryID")%>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblAssetSubCategoryID" runat="server" Text='<%# Bind("Asset_SubCategoryID")%>' Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                            </asp:TemplateField>


                                                            <asp:TemplateField HeaderText="Asset Category" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetCategory" runat="server" Text='<%# Bind("Asset_Category")%>'
                                                                        Width="180px"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlAssetCategory" runat="server" Width="180px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvAssetCategory" runat="server" ControlToValidate="ddlAssetCategory"
                                                                        Display="None" ErrorMessage="Select the Asset Category" ValidationGroup="AssetAdd" InitialValue="0">
                                                                    </asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <%--<asp:TemplateField HeaderText="Asset SubCategory" ItemStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssetSubCategory" runat="server" Text='<%# Bind("Asset_SubCategory")%>'
                                                                            Width="180px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlAssetSubCategory" runat="server" Width="180px">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvAssetSubCategory" runat="server" ControlToValidate="ddlAssetSubCategory"
                                                                            Display="None" ErrorMessage="Select the Asset Sub Category" ValidationGroup="AssetAdd" InitialValue="0">
                                                                        </asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>--%>

                                                            <asp:TemplateField HeaderText="Asset Description" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssetDescription" runat="server" Text='<%# Bind("Asset_Description")%>' Style="word-wrap: normal; word-break: break-all"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtAssetDescription" runat="server" Width="180px" TextMode="MultiLine"
                                                                        Height="50px" onkeyup="maxlengthfortxt(200);"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvAssetDescription" runat="server" ControlToValidate="txtAssetDescription"
                                                                            Display="None" ErrorMessage="Enter the Asset Description" ValidationGroup="AssetAdd">
                                                                        </asp:RequiredFieldValidator>--%>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Left" Width="180px" />
                                                            </asp:TemplateField>


                                                            <%--<asp:TemplateField HeaderText="Cost">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAssetCost" runat="server" Text='<%# Bind("Asset_Cost")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAssetCost" runat="server" Style="text-align: right" Width="95px" MaxLength="18" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvAssetCost" runat="server" ControlToValidate="txtAssetCost"
                                                                            Display="None" ErrorMessage="Enter the Cost" ValidationGroup="AssetAdd">
                                                                        </asp:RequiredFieldValidator>
                                                                        <cc1:FilteredTextBoxExtender ID="ftvAssetCost" runat="server" TargetControlID="txtAssetCost" ValidChars="." Enabled="true" FilterType="Numbers,Custom">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>--%>

                                                            <%--Rate Type--%>
                                                            <asp:TemplateField HeaderText="Rate Type" ItemStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRateType" runat="server" Text='<%# Bind("Rate_Type")%>'
                                                                        Width="180px"></asp:Label>
                                                                    <asp:Label ID="lblRateTypeID" runat="server" Text='<%# Bind("Rate_Type_ID")%>'
                                                                        Width="180px" Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:DropDownList ID="ddlRateType" runat="server" Width="180px">
                                                                    </asp:DropDownList>
                                                                    <asp:RequiredFieldValidator ID="rfvRateType" runat="server" ControlToValidate="ddlRateType"
                                                                        Display="None" ErrorMessage="Select the Rate Type" ValidationGroup="AssetAdd" InitialValue="0">
                                                                    </asp:RequiredFieldValidator>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Rental Amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRate" runat="server" Text='<%# Bind("Rate")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtRate" runat="server" Style="text-align: right" Width="95px" MaxLength="8" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvRate" runat="server" ControlToValidate="txtRate"
                                                                        Display="None" ErrorMessage="Enter the Rental Amount" ValidationGroup="AssetAdd">
                                                                    </asp:RequiredFieldValidator>
                                                                    <cc1:FilteredTextBoxExtender ID="ftvRate" runat="server" TargetControlID="txtRate" ValidChars="." Enabled="true" FilterType="Numbers,Custom">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Tenure">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTenure" runat="server" Text='<%# Bind("Tenure")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtTenure" runat="server" Style="text-align: right" Width="70px" MaxLength="3"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvTenure" runat="server" ControlToValidate="txtTenure"
                                                                        Display="None" ErrorMessage="Enter the Tenure" ValidationGroup="AssetAdd">
                                                                    </asp:RequiredFieldValidator>
                                                                    <cc1:FilteredTextBoxExtender ID="ftvTenure" runat="server" TargetControlID="txtTenure" Enabled="true" FilterType="Numbers">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                                <FooterStyle HorizontalAlign="Center" Width="7%" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Sec Deposit%" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSecurityDeposit" runat="server" Text='<%# Bind("Security_Deposit")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtSecurityDeposit" runat="server" Style="text-align: right" Width="95px" MaxLength="8"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvSecurityDeposit" runat="server" ControlToValidate="txtSecurityDeposit"
                                                                            Display="None" ErrorMessage="Enter the Security Deposit" ValidationGroup="LeadAdd">
                                                                        </asp:RequiredFieldValidator>--%>
                                                                    <cc1:FilteredTextBoxExtender ID="ftvSecurityDeposit" runat="server" TargetControlID="txtSecurityDeposit" ValidChars="." Enabled="true" FilterType="Numbers,Custom">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Sanction Limit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSanctionAmount" runat="server" Text='<%# Bind("Sanction_Amount")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:TextBox ID="txtSanctionAmount" runat="server" Style="text-align: right" Width="95px" MaxLength="18" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="rfvSanctionAmount" runat="server" ControlToValidate="txtSanctionAmount"
                                                                            Display="None" ErrorMessage="Enter the Sanction Amount" ValidationGroup="AssetAdd">
                                                                        </asp:RequiredFieldValidator>--%>
                                                                    <cc1:FilteredTextBoxExtender ID="ftvSanctionAmount" runat="server" TargetControlID="txtSanctionAmount" ValidChars="." Enabled="true" FilterType="Numbers,Custom">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"
                                                                        OnClientClick="return confirm('Do you want to Delete this record?');" ToolTip="Delete">
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="styleGridShortButton"
                                                                        OnClick="btnAssetAdd_Click" Text="Add" ValidationGroup="AssetAdd" ToolTip="Add" />
                                                                </FooterTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <FooterStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <span>No Records Found...</span>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                    <%-- </div>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:Button ID="btnLeadOk" Text="Save" runat="server" CssClass="styleGridShortButton" ValidationGroup="LeadAdd"
                                                        OnClick="btnLeadOk_OnClick" ToolTip="Save Lead Details" />
                                                    <asp:Button ID="btnLeadClear" Text="Clear" runat="server" CssClass="styleGridShortButton"
                                                        ToolTip="Clear Lead Details" OnClick="btnLeadClear_Click" />
                                                    <asp:Button ID="btnLeadCancel" Text="Cancel" runat="server" CssClass="styleGridShortButton"
                                                        OnClick="btnLeadCancel_OnClick" ToolTip="Cancel Lead Details" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:ValidationSummary ID="ValidationSummary4" runat="server" CssClass="styleMandatoryLabel"
                                                        HeaderText="Correct the following validation(s):" ValidationGroup="AssetAdd" />
                                                    <asp:ValidationSummary ID="vsLeadAsset" runat="server" CssClass="styleMandatoryLabel"
                                                        HeaderText="Correct the following validation(s):" ValidationGroup="LeadAdd" />
                                                </td>
                                            </tr>
                                        </table>

                                        <table>
                                            <tr width="100%">
                                                <td width="100%">
                                                    <asp:Panel DefaultButton="btnPassword" ID="PnlPassword" Style="display: none" runat="server"
                                                        Height="130px" BackColor="White" BorderStyle="Solid" BorderColor="Black" Width="30%">
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="3" class="stylePageHeading" align="center">
                                                                    <asp:Label runat="server" ToolTip="password" Text="Enter your password" ID="lblPasswordHeader"
                                                                        CssClass="styleDisplayLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td>&nbsp;
                                                                <asp:Label ID="lblPassword" ToolTip="password" runat="server" Text="Password:" Font-Bold="true"></asp:Label>
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td align="left" class="styleFieldAlign">
                                                                    <asp:TextBox ID="txtPassword" ToolTip="password" runat="server" Width="200px" TextMode="Password"
                                                                        MaxLength="30"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <asp:Button ID="btnPassword" ToolTip="Authenticate password" CausesValidation="false"
                                                                        CssClass="styleSubmitButton" OnClick="btnPassword_Click" Text="Submit" runat="server" />
                                                                    &nbsp;
                                                                <asp:Button ID="btnCancelPass" ToolTip="Cancel" CausesValidation="false" OnClick="btnCancelPass_Click"
                                                                    CssClass="styleSubmitButton" Text="Cancel" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="3">
                                                                    <asp:Label ID="lblErrorMessagePass" runat="server" CssClass="styleMandatoryLabel" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                                <td>
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtenderPassword" runat="server" TargetControlID="ddlLeadStatus"
                                                        PopupControlID="PnlPassword" BackgroundCssClass="styleModalBackground" Enabled="True">
                                                    </cc1:ModalPopupExtender>
                                                </td>
                                            </tr>
                                        </table>

                                    </asp:Panel>
                                    <asp:Panel ID="pnlLeadViewDetails" Visible="false" Width="100%" runat="server"
                                        CssClass="stylePanel" Style="overflow: hidden; z-index: -1; position: inherit;">
                                        <div class="container" style="max-height: 350px; overflow-y: auto; overflow-x: hidden;">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnAddLead" runat="server" Text="ADD" CssClass="styleGridShortButton" OnClick="btnAddLead_Click" />
                                                    </td>
                                                    <td width="30%" align="right" runat="server" id="tdInitiate">
                                                        <asp:DropDownList ID="ddlPrograms" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator Display="None" ID="rfvddlPrograms" CssClass="styleMandatoryLabel"
                                                            InitialValue="0" ErrorMessage="Select the Program to initiate" runat="server"
                                                            ControlToValidate="ddlPrograms" ValidationGroup="Enquiry" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:Button runat="server" ID="btnMoveEnquiry" ValidationGroup="Enquiry" CssClass="styleGridShortButton"
                                                            Text="Initiate" OnClick="btnMoveEnquiry_Click" OnClientClick="return fnCheckPageValidators('Enquiry', false)" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:GridView runat="server" ID="gvLeadDetails" AutoGenerateColumns="False" EmptyDataText="No Records Found" OnRowDataBound="gvLeadDetails_RowDataBound">
                                                <Columns>
                                                    <%-- Check   --%>
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkLeadSelect" runat="server" AutoPostBack="true" Checked='<%# Eval("IS_Checked").ToString() == "1" ? true : false %>' OnCheckedChanged="chkLeadSelect_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="4%" />
                                                    </asp:TemplateField>

                                                    <%-- Lead ID   --%>
                                                    <asp:TemplateField HeaderText="Lead ID" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadID" runat="server" Text='<%# Bind("Lead_ID")%>'></asp:Label>
                                                            <asp:Label ID="lblPricingID" runat="server" Text='<%# Bind("Pricing_ID")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="4%" />
                                                    </asp:TemplateField>

                                                    <%-- Lead No   --%>
                                                    <asp:TemplateField HeaderText="Lead No" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadNo" runat="server" Text='<%# Bind("Lead_No")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>

                                                    <%-- Lead information   --%>
                                                    <asp:TemplateField HeaderText="Lead Information" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadInformation" runat="server" Text='<%# Bind("Lead_Information")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="35%" />
                                                    </asp:TemplateField>

                                                    <%-- Location   --%>
                                                    <asp:TemplateField HeaderText="State" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadLocation" runat="server" Text='<%# Bind("Lead_Location")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                                                    </asp:TemplateField>

                                                    <%-- Lead Name   --%>
                                                    <asp:TemplateField HeaderText="Lead Source" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadName" runat="server" Text='<%# Bind("Lead_Name")%>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                    </asp:TemplateField>

                                                    <%-- Lead Status   --%>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadStatus" runat="server" Text='<%# Bind("Lead_Status")%>'></asp:Label>
                                                            <asp:Label ID="lblLeadStatusID" runat="server" Text='<%# Bind("Lead_Status_ID")%>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>

                                                    <%-- Edit View   --%>
                                                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkLeadEdit" runat="server" Text="Edit" OnClick="lnkLeadEdit_Click"></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkLeadView" runat="server" Text="View" OnClick="lnkLeadView_Click"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                    </asp:TemplateField>

                                                </Columns>
                                                <HeaderStyle CssClass="styleGridHeader" />
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlGroupDetails" Width="100%" runat="server" Visible="false"
                                        CssClass="stylePanel" Style="overflow: hidden; z-index: -1; position: inherit;">

                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGroupCode" runat="server" Text="Group Code" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtGroupCode" runat="server" ReadOnly="true"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGroupName" runat="server" Text="Group Name" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtGroupName" runat="server" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>

                                        <div class="container" style="max-height: 350px; overflow-y: auto; overflow-x: hidden;">
                                            <asp:GridView runat="server" ID="grvGroupDetails" AutoGenerateColumns="False" EmptyDataText="No Records Found">
                                                <Columns>
                                                    <asp:BoundField DataField="Customer_Code" HeaderText="Lessee Code" ItemStyle-Width="120px" />
                                                    <asp:BoundField DataField="Customer_Name" HeaderText="Lessee Name" ItemStyle-Width="250px" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="pnlCrossBorder" Visible="false" Width="100%" runat="server"
                                        CssClass="stylePanel" Style="overflow: hidden; z-index: -1; position: inherit;">
                                        <asp:GridView runat="server" ID="grvCrossBorder" AutoGenerateColumns="False" EmptyDataText="No Records Found"
                                            Width="98%" HorizontalAlign="Center">
                                            <Columns>

                                                <%--Lead ID--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Cross Border ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrossLead_ID" runat="server" Text='<%# Bind("Cross_Border_ID")%>'></asp:Label>
                                                        <asp:Label ID="lblCrossGroupID" runat="server" Text='<%# Bind("Cross_Group_ID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <%--Lead No--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Lead Number">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrossLeadNo" runat="server" Text='<%# Bind("Lead_no")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateField>

                                                <%--Lead Information--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Lead Information">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrossLeadInformation" runat="server" Text='<%# Bind("Lead_information")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                </asp:TemplateField>

                                                <%--Location--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="State">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrossLocation" runat="server" Text='<%# Bind("Location")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateField>

                                                <%--Account Manager 1--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Account Manager 1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrossAcctMngr1" runat="server" Text='<%# Bind("Account_Manager1")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateField>

                                                <%--Account Manager 2--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Account Manager 2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCrossAcctMngr2" runat="server" Text='<%# Bind("Account_Manager2")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                </asp:TemplateField>

                                                <%--Remarks--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCrossRemarks" runat="server" TextMode="MultiLine" Width="250px" onkeyup="maxlengthfortxt(200);"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                                </asp:TemplateField>

                                                <%--Approved--%>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkCrossBorder" runat="server" OnClick="toggleSelectionGrid(this);" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" Width="4%" />
                                                </asp:TemplateField>

                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button runat="server" ID="btnSaveCBD" CssClass="styleSubmitButton" Text="Approve"
                                                        OnClick="btnSaveCBD_Click" ToolTip="Approve Cross Border Details" Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <cc1:DropShadowExtender ID="dseLeadDetails" runat="server" TargetControlID="pnlLeadView"
                                        Opacity=".4" Rounded="false" TrackPosition="true" />
                                    <asp:Button ID="btnLeadView" Text="Ok" runat="server" CssClass="styleGridShortButton"
                                        OnClick="btnLeadView_OnClick" Style="display: none;" />


                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center"></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlAccountInformation" Visible="false" Width="100%" runat="server"
                            GroupingText="Account Information" CssClass="stylePanel" Style="overflow: hidden">
                            <table width="98%">
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <%--     <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" ToolTip="Save"
                            Text="Save" OnClick="btnSave_Click" />--%>
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            OnClientClick="return fnCheckPageValidators('vgSave');" ValidationGroup="vgSave"
                            OnClick="btnSave_Click" Visible="false" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" ValidationGroup="VGODI"
                            CausesValidation="false" CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        <asp:RequiredFieldValidator ID="rfvddlType" runat="server" ControlToValidate="ddlType"
                            ErrorMessage="Select at least one query type" CssClass="styleMandatoryLabel"
                            Display="None" InitialValue="0" SetFocusOnError="True" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlType" ErrorMessage="Select at least one query type"
                            CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                            ValidationGroup="vgUp"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vgSave" runat="server" ValidationGroup="vgSave" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgUp"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                        <asp:CustomValidator ID="cvFollowUp" runat="server" CssClass="styleMandatoryLabel"
                            Enabled="true" Width="98%" />
                        <asp:CustomValidator ID="cvEnquiry" runat="server" CssClass="styleMandatoryLabel"
                            ValidationGroup="Enquiry" Enabled="true" Width="98%" Display="None" />
                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="Enquiry"
                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                            ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <iframe runat="server" id="ifrmCRM" width="100%" frameborder="0" style="min-height: 700px; border-style: none;" visible="false"></iframe>
                        <asp:Button runat="server" ID="btnFrameCancel" CssClass="styleSubmitButton" Text="Save" Style="display: none"
                            OnClick="btnFrameCancel_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnClear" />
        </Triggers>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>
                <asp:Button OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                    runat="server" ID="btnNOD" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />
                <cc1:ModalPopupExtender ID="moeNOD" runat="server" TargetControlID="btnNOD" PopupControlID="pnlNOD"
                    BackgroundCssClass="styleModalBackground" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <%--<asp:Button OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                    runat="server" ID="Button1" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />
                <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="Button1" PopupControlID="pnlAddFollow"
                    BackgroundCssClass="styleModalBackground" />--%>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td width="100%">
                <asp:Panel ID="pnlNOD" GroupingText="" Width="90%" Height="370px" runat="server"
                    Style="display: none" BackColor="White" BorderStyle="Solid">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td class="stylePageHeading" width="100%">
                                        <asp:Label runat="server" ID="lblNodHead" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div class="container" style="height: 300px; width: 100%; overflow: auto;">
                                            <asp:GridView runat="server" ID="grvPopUp" AutoGenerateColumns="true" Width="97%"
                                                OnRowDataBound="grvPopUp_RowDataBound">
                                                <Columns>
                                                </Columns>
                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;<asp:Button runat="server" ID="Button3" OnClientClick="if(document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow'))document.getElementById('ctl00_ContentPlaceHolder1_ucPopUp_hdnShow').value='0';"
                                        CssClass="styleSubmitButton" Text="Close" OnClick="Button3_OnClick" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td></td>
        </tr>
    </table>
</asp:Content>
