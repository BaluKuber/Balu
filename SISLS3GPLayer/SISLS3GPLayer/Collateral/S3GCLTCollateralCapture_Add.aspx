<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GCLTCollateralCapture_Add.aspx.cs" Inherits="Collateral_S3GCLTCollateralCapture_Add"
    Title="Untitled Page" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">


        function window.onerror() { return true; }

        function showPopup() {

            //    alert("<%= MPE.ClientID %>")
            //    var modal = $find("<%= MPE.ClientID %>");
            //   alert(modal);
            //    modal.show();
            alert(document.getElementById('ctl00_ContentPlaceHolder1_Button2'));
            document.getElementById('ctl00_ContentPlaceHolder1_Button2').click();


        }
        //window.history.forward(-1);
        function trimStartingSpace(textbox) {
            var textValue = textbox.value;
            if (textValue.length == 0 && window.event.keyCode == 32) {
                window.event.keyCode = 0;
                return false;
            }
        }
        function confirm_delete() {

            var tmp = confirm("Collateral Security is mapped with Storage \n \n Are you sure you want to delete?");

            if (tmp == true) {

                document.getElementById('ctl00_ContentPlaceHolder1_tcCollateralCapture_tabgeneral_hdnValue').value = tmp;
                return true;
            }

            else {
                document.getElementById('ctl00_ContentPlaceHolder1_tcCollateralCapture_tabgeneral_hdnValue').value = tmp;
                return false;
            }
        }

        function fnLoadCustomer() {
            document.getElementById('ctl00_ContentPlaceHolder1_tcCollateralCapture_tabgeneral_btnLoadCustomer').click();
        }

        function fnGetValue(txtMeasurement, txtUnitRate, txtValue) {
            var Measurement = document.getElementById(txtMeasurement).value;
            var UnitRate = document.getElementById(txtUnitRate).value;
            var Value = Measurement * UnitRate;

            document.getElementById(txtValue).value = Value;

        }
        function fncheckMandatory(ddlID, rfvIssuedByID, rfvUnitFaceValueID, rfvNoOfUnitsID, rfvInterestID, rfvMaturityDateID, rfvMaturityValueID) {
            var ddlSecurity = document.getElementById(ddlID);
            var ddlSecValue = ddlSecurity.options[ddlSecurity.selectedIndex].text;
            var rfvIssuedBy = document.getElementById(rfvIssuedByID);
            var rfvUnitFaceValue = document.getElementById(rfvUnitFaceValueID);
            var rfvNoOfUnits = document.getElementById(rfvNoOfUnitsID);

            var rfvInterest = document.getElementById(rfvInterestID);
            var rfvMaturityDate = document.getElementById(rfvMaturityDateID);
            var rfvMaturityValue = document.getElementById(rfvMaturityValueID);

            if (ddlSecValue == "Deposits") {
                ValidatorEnable(rfvIssuedBy, false);
                ValidatorEnable(rfvUnitFaceValue, false);
                ValidatorEnable(rfvNoOfUnits, false);
            }
            else {
                ValidatorEnable(rfvIssuedBy, true);
                ValidatorEnable(rfvUnitFaceValue, true);
                ValidatorEnable(rfvNoOfUnits, true);
            }

            if (ddlSecValue == "Equity Shares") {
                ValidatorEnable(rfvIssuedBy, false);
                ValidatorEnable(rfvUnitFaceValue, false);
                ValidatorEnable(rfvNoOfUnits, false);
            }
            else {
                ValidatorEnable(rfvIssuedBy, true);
                ValidatorEnable(rfvUnitFaceValue, true);
                ValidatorEnable(rfvNoOfUnits, true);
            }
        }

        function fncheckMandatory1(ddlID, txtDPNameID, txtDPNoID, txtClientIDID, txtCertificateNoID, rfvDPNameID, rfvDPNOID, rfvClientIDID, rfvCertificateNoID) {
            //debugger;
            var ddlDemat = document.getElementById(ddlID);
            var ddlDematValue = ddlDemat.options[ddlDemat.selectedIndex].text;
            var txtDPName = document.getElementById(txtDPNameID);
            var txtDPNo = document.getElementById(txtDPNoID);
            var txtClientID = document.getElementById(txtClientIDID);
            var txtCertificateNo = document.getElementById(txtCertificateNoID);
            var rfvDPName = document.getElementById(rfvDPNameID);
            var rfvDPNO = document.getElementById(rfvDPNOID);
            var rfvClientID = document.getElementById(rfvClientIDID);
            var rfvCertificateNo = document.getElementById(rfvCertificateNoID);
            var lblhCertificateNo = document.getElementById('<%=lblhCertificateNo.ClientID%>');
            var lblhDPNo = document.getElementById('<%=lblhDPNo.ClientID%>');
            var lblhDPName = document.getElementById('<%=lblhDPName.ClientID%>');
            var lblhClientID = document.getElementById('<%=lblhClientID.ClientID%>');

            if (ddlDematValue == "No") {
                ValidatorEnable(rfvDPName, false);
                ValidatorEnable(rfvDPNO, false);
                ValidatorEnable(rfvClientID, false);
                ValidatorEnable(rfvCertificateNo, true);
                txtDPName.setAttribute("readOnly", "readOnly");
                txtClientID.setAttribute("readOnly", "readOnly");
                txtCertificateNo.removeAttribute("readOnly");
                txtDPNo.setAttribute("readOnly", "readOnly");

                txtDPNo.value = "";
                txtClientID.value = "";
                txtDPName.value = "";

                lblhDPNo.className = "";
                lblhDPName.className = "";
                lblhClientID.className = "";

                lblhCertificateNo.className = "styleReqFieldLabel";
                //                alert('No');

            }
            if (ddlDematValue == "Yes") {
                ValidatorEnable(rfvDPName, true);
                ValidatorEnable(rfvDPNO, true);
                ValidatorEnable(rfvClientID, true);
                ValidatorEnable(rfvCertificateNo, false);
                txtDPName.removeAttribute("readOnly");
                txtClientID.removeAttribute("readOnly");
                txtCertificateNo.setAttribute("readOnly", "readOnly");
                txtDPNo.removeAttribute("readOnly");
                //                alert('Yes');
                txtCertificateNo.value = "";
                lblhCertificateNo.className = "";

                lblhDPNo.className = "styleReqFieldLabel";
                lblhDPName.className = "styleReqFieldLabel";
                lblhClientID.className = "styleReqFieldLabel";
            }
        }

        function fncheckMandatoryInMedSec(ddlID, rfvRegistrationID, rfvSerialNoID) {
            var ddlSecurity = document.getElementById(ddlID);
            var ddlSecValue = ddlSecurity.options[ddlSecurity.selectedIndex].text;
            var rfvRegistration = document.getElementById(rfvRegistrationID);
            var rfvSerialNo = document.getElementById(rfvSerialNoID);
            if (ddlSecValue == "Machinery") {
                ValidatorEnable(rfvRegistration, false);
                ValidatorEnable(rfvSerialNo, true);
            }
            if (ddlSecValue == "Vehicles") {
                ValidatorEnable(rfvRegistration, true);
                ValidatorEnable(rfvSerialNo, false);
            }
        }
        
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <%-- Design for 1st Row       --%>
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="Collateral Capture" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        <input id="HidConfirm" type="hidden" runat="server" />
                    </td>
                </tr>
                <%-- Design for space in between Row       --%>
                <tr>
                    <td class="styleFieldLabel">
                        &nbsp;
                    </td>
                </tr>
                <%-- Design for 2nd Row       --%>
                <tr>
                    <td class="styleFieldLabel">
                        <cc1:TabContainer ID="tcCollateralCapture" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            ScrollBars="None" AutoPostBack="false">
                            <cc1:TabPanel runat="server" HeaderText="General" ID="tabgeneral" CssClass="tabpan"
                                BackColor="Red" ToolTip="General">
                                <HeaderTemplate>
                                    General
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="upMainPage" runat="server">
                                        <ContentTemplate>
                                            <table width="99%">
                                                <tr>
                                                    <td>
                                                        <table style="text-align: center" width="99%">
                                                            <tr>
                                                                <td align="center" colspan="2" valign="middle">
                                                                    <asp:RadioButtonList ID="rbtlCollCollected" runat="server" RepeatDirection="Horizontal"
                                                                        AutoPostBack="True" OnSelectedIndexChanged="rbtlCollCollected_SelectedIndexChanged">
                                                                        <asp:ListItem Selected="True" Value="1">Customer</asp:ListItem>
                                                                        <asp:ListItem Value="2">Others</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="50%">
                                                                    <asp:Panel ID="pnlCustDetails" runat="server" CssClass="stylePanel" GroupingText="Customer Details">
                                                                        <table border="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblType" runat="server" CssClass="styleReqFieldLabel" Text="Type"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddlType" Enabled="false"
                                                                                        Display="None" ErrorMessage="Select a Type" InitialValue="0" SetFocusOnError="True"
                                                                                        ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    <asp:RequiredFieldValidator ID="rfvTypeGo" runat="server" ControlToValidate="ddlType"
                                                                                        Display="None" ErrorMessage="Select a Type" InitialValue="0" SetFocusOnError="True" Enabled="false"
                                                                                        ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel" width="35%">
                                                                                    <asp:Label ID="lblCustorAgent" runat="server" CssClass="styleReqFieldLabel"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="styleSearchBox" MaxLength="50"
                                                                                        Style="display: none" ToolTip="Customer"></asp:TextBox>
                                                                                    <asp:DropDownList ID="ddlCollectionAgent" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCollectionAgent_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <uc2:LOV ID="ucCustomerCodeLov" runat="server" onblur="fnLoadCustomer()" strLOV_Code="CCP" />
                                                                                    <asp:RequiredFieldValidator ID="rfvCustomerCode" runat="server" ControlToValidate="txtCustomerCode"
                                                                                        Display="None" ErrorMessage="Select a Customer Code" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    <asp:RequiredFieldValidator ID="rfvCustomerCodeGo" runat="server" ControlToValidate="txtCustomerCode"
                                                                                        Display="None" ErrorMessage="Select a Customer Code" SetFocusOnError="True" ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" width="100%">
                                                                                    <asp:Button ID="btnLoadCustomer" runat="server" OnClick="btnLoadCustomer_Click" Style="display: none"
                                                                                        Text="Load Customer" />
                                                                                    <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" runat="server" FirstColumnStyle="styleFieldLabel"
                                                                                        SecondColumnStyle="styleFieldAlign" ShowCustomerCode="false" />
                                                                                    <input id="hdnCustID" runat="server" type="hidden" />
                                                                                </td>
                                                                                <tr>
                                                                                    <td>
                                                                                        <div id="divSpace" runat="server" style="height: 5px;">
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                            </tr>
                                                                        </table>
                                                                        <%--<uc1:S3GCustomerAddress ID="S3GCustomerAddress1" ActiveViewIndex="0" runat="server"
                                                                                    FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" />--%></asp:Panel>
                                                                    <asp:HiddenField ID="hdnValue" runat="server" />
                                                                </td>
                                                                <td width="50%">
                                                                    <asp:Panel ID="pnlHeader" runat="server" CssClass="stylePanel" GroupingText="Header Details">
                                                                        <table border="0" cellspacing="0" style="text-align: center" width="100%">
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblColleNo" runat="server" CssClass="styleDisplayLabel" Text="Collateral Trans No"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtCollTransNo" runat="server" ContentEditable="false" CssClass="styleAutoGenerated"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblCollTransDate" runat="server" CssClass="styleReqFieldLabel" Text="Collateral Trans Date"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtCollTransDate" runat="server" ReadOnly="True"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2">
                                                                                    <hr />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblLOB" runat="server" CssClass="styleReqFieldLabel" Text="Line of Business"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ControlToValidate="ddlLOB"
                                                                                        Display="None" ErrorMessage="Select a LOB" InitialValue="0" SetFocusOnError="True"
                                                                                        ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    <asp:RequiredFieldValidator ID="rfvLOBGo" runat="server" ControlToValidate="ddlLOB"
                                                                                        Display="None" ErrorMessage="Select a Line of business" InitialValue="0" SetFocusOnError="True"
                                                                                        ValidationGroup="Go"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblBranch" runat="server" CssClass="styleDisplayLabel" Text="Location"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <%-- <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                                                                        Style="height: 22px">
                                                                                    </asp:DropDownList>--%>
                                                                                    <uc2:Suggest ID="ddlBranch" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                        OnItem_Selected="ddlBranch_SelectedIndexChanged" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:Label ID="lblConstitution" runat="server" CssClass="styleDisplayLabel" Text="Constitution"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlConstitution" runat="server" Enabled="false" AutoPostBack="True"
                                                                                        OnSelectedIndexChanged="ddlConstitution_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:Label ID="lblProductCode" runat="server" CssClass="styleDisplayLabel" Text="Product Code"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlProductCode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProductCode_SelectedIndexChanged">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblCurrCode" runat="server" CssClass="styleReqFieldLabel" Text="Currency Code"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlCurrCode" runat="server">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvCurrCode" runat="server" ControlToValidate="ddlCurrCode"
                                                                                        Display="None" ErrorMessage="Select a Currency Code" InitialValue="0" SetFocusOnError="True"
                                                                                        ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblRefPoint" runat="server" Text="Ref Point"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlRefPoint" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRefPoint_SelectedIndexChanged"
                                                                                        Width="165px">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvRefPoint" runat="server" Enabled="false" ControlToValidate="ddlRefPoint"
                                                                                        Display="None" ErrorMessage="Select a Ref Point" InitialValue="0" SetFocusOnError="True"
                                                                                        ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblDYNRefPoint" runat="server" CssClass="styleDisplayLabel" Text="Ref Code"
                                                                                        Visible="false"></asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:DropDownList ID="ddlDYNRefPoint" runat="server" Width="165px" Visible="false">
                                                                                    </asp:DropDownList>
                                                                                    <asp:RequiredFieldValidator ID="rfvDYNRefPoint" runat="server" ControlToValidate="ddlDYNRefPoint"
                                                                                        Display="None" InitialValue="0" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="styleFieldLabel">
                                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="styleDisplayLabel" Text="Total"> </asp:Label>
                                                                                </td>
                                                                                <td class="styleFieldAlign">
                                                                                    <asp:TextBox ID="txtTotal" runat="server" ReadOnly="true" Width="150px" Style="text-align: right"
                                                                                        ToolTip="Total" MaxLength="20"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTotal"
                                                                                        Display="None" InitialValue="0" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                                                                    <asp:Button ID="btnGo" runat="server" CausesValidation="true" CssClass="styleGridShortButton"
                                                                                        OnClick="btnGo_Click" Text="Go" ValidationGroup="Go" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Panel ID="pnlPASADetails" runat="server" CssClass="stylePanel" GroupingText="Account Details"
                                                                        ScrollBars="None" Visible="False">
                                                                        <div runat="server" id="divAcc" class="container" style="height: 139px; overflow-x: hidden;
                                                                            overflow-y: scroll;">
                                                                            <asp:GridView ID="grvprimeaccount" runat="server" AutoGenerateColumns="False" BorderWidth="2px"
                                                                                ShowFooter="True" OnRowDataBound="grvprimeaccount_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField Visible="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblPASAID" runat="server" Text='<%# Eval("Account_ID") %>' />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Sl.No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="2%" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Prime A/c No.">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblMLA" runat="server" Text='<%#Eval("PRIMEACCOUNTNO")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Sub A/c No." Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSLA" runat="server" Text='<%#Eval("SUBACCOUNTNO")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Select">
                                                                                        <HeaderTemplate>
                                                                                            <table>
                                                                                                <tr align="center">
                                                                                                    <td align="center">
                                                                                                        <asp:Label ID="lblSelect" runat="server" Text="Select Account" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </HeaderTemplate>
                                                                                        <%--OnCheckedChanged="chkAccount_OnCheckedChanged"--%>
                                                                                        <ItemTemplate>
                                                                                            <asp:CheckBox ID="chkAccount" runat="server" AutoPostBack="false" ToolTip="Select Account" />
                                                                                            <asp:Label ID="lblAccount_ID" runat="server" Text='<%#Eval("Account_ID")%>' Visible="false" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle Width="10%" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <RowStyle HorizontalAlign="Center" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                    </td>
                                                </tr>
                                            </table>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                                                        Text="Save" OnClientClick="return fnCheckPageValidators('Submit');" ValidationGroup="Submit" />
                                                    <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                        OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" Text="Clear" />
                                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                        OnClick="btnCancel_Click" Text="Cancel" />
                                                </td>
                                            </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="High Liquid Securities" ID="tabHighLiq"
                                CssClass="tabpan" BackColor="Red" ToolTip="High Liquid Securities" witdh="99%">
                                <HeaderTemplate>
                                    High Liquid Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlHighLiqDetails" ScrollBars="Horizontal" CssClass="stylePanel"
                                        GroupingText="High Liquid Security Details">
                                        <asp:Label ID="lblHighLiqDetails" runat="server" Text="No Security Details are Available"
                                            Visible="False" Font-Size="Large" Font-Bold="False" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCollSecurities" runat="server" CssClass="styleReqFieldLabel" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblhSlNo" runat="server" Visible="False"></asp:Label>
                                                    <asp:Label ID="lblhMode" runat="server" Visible="False"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlhCollSecurities" ToolTip="Collateral Securities" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvhCollSecurities" runat="server" ControlToValidate="ddlhCollSecurities"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Select Collateral Securities"
                                                        InitialValue="0" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhIssuedBy" runat="server" CssClass="styleReqFieldLabel" Text="Issued By"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthIssuedBy" runat="server" Width="150px" ToolTip="Issued By" MaxLength="20"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvIssuedBy" runat="server" ControlToValidate="txthIssuedBy"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Issued By" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhIssuedBy" runat="server" TargetControlID="txthIssuedBy"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" ValidChars=" "
                                                        Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhDemat" CssClass="styleRegLabel" runat="server" Text="Demat"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlhDemat" runat="server" Width="50px" ToolTip="Demat" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlhDemat_SelectedIndexChanged">
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhDPName" runat="server" Text="DP Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthDPName" MaxLength="20" ReadOnly="True" Width="150px" ToolTip="DP Name"
                                                        onKeyPress="trimStartingSpace(this)" runat="server" Text='<%# Bind("Dp_Name") %>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvhDPName" runat="server" ControlToValidate="txthDPName"
                                                        Display="None" SetFocusOnError="True" Enabled="False" ErrorMessage="Enter DP Name"
                                                        ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhDPNo" runat="server" Text="DP No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthDPNo" runat="server" ReadOnly="True" MaxLength="15" Width="120px"
                                                        ToolTip="DP No"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhDPNo" runat="server" TargetControlID="txthDPNo"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                        ValidChars="/-#">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhDPNo" runat="server" ControlToValidate="txthDPNo"
                                                        Display="None" SetFocusOnError="True" Enabled="False" ErrorMessage="Enter DP No"
                                                        ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhClientID" runat="server" Text="Client ID"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthClientID" ToolTip="Client ID" ReadOnly="True" runat="server"
                                                        Width="120px" MaxLength="15"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexClientID" runat="server" TargetControlID="txthClientID"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                        ValidChars="/-#">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhClientID" runat="server" ControlToValidate="txthClientID"
                                                        Display="None" SetFocusOnError="True" Enabled="False" ErrorMessage="Enter Client ID"
                                                        ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCertificateNo" runat="server" CssClass="styleReqFieldLabel" Text="Certificate No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthCertificateNo" runat="server" Width="150px" ToolTip="Certificate No"
                                                        MaxLength="20"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhCertificateNo" runat="server" TargetControlID="txthCertificateNo"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                        ValidChars="/-#">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhCertificateNo" runat="server" ControlToValidate="txthCertificateNo"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Certificate No" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhFolioNo" runat="server" CssClass="styleRegLabel" Text="Folio No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthFolioNo" runat="server" Width="150px" ToolTip="Folio No" MaxLength="20"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhFolioNo" runat="server" TargetControlID="txthFolioNo"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True"
                                                        ValidChars="/-#">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhUnitFaceValue" runat="server" CssClass="styleReqFieldLabel" Text="Unit Face Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthUnitFaceValue" Width="120px" MaxLength="12" ToolTip="Unit Face Value"
                                                        runat="server" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhUnitFaceValue" runat="server" TargetControlID="txthUnitFaceValue"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhUnitFaceValue" runat="server" ControlToValidate="txthUnitFaceValue"
                                                        Display="None" ErrorMessage="Enter Unit Face Value" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhNoOfUnits" runat="server" CssClass="styleReqFieldLabel" Text="No Of Units"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthNoOfUnits" MaxLength="10" Width="90px" ToolTip="No of Units"
                                                        runat="server" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhNoOfUnits" runat="server" TargetControlID="txthNoOfUnits"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhNoOfUnits" runat="server" ControlToValidate="txthNoOfUnits"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter No Of Units" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhInterest" runat="server" CssClass="styleReqFieldLabel" Text="Interest Percent"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthInterest" runat="server" Width="65px" MaxLength="5" ToolTip="Interest Percent"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvhInterest" runat="server" ControlToValidate="txthInterest"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Interest Percent" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhMaturityDate" runat="server" CssClass="styleReqFieldLabel" Text="Maturity Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthMaturityDate" Width="90px" ToolTip="Maturity Date" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CEhMaturityDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="Image1"
                                                        TargetControlID="txthMaturityDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhMaturityDate" runat="server" ControlToValidate="txthMaturityDate"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Maturity Date" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhMaturityValue" runat="server" CssClass="styleReqFieldLabel" Text="Maturity Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthMaturityValue" runat="server" Width="90px" ToolTip="Maturity Value"
                                                        MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhMaturityValue" runat="server" TargetControlID="txthMaturityValue"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhMaturityValue" runat="server" ControlToValidate="txthMaturityValue"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Maturity Value" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhMarketRate" runat="server" CssClass="styleReqFieldLabel" Text="Market Rate"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthMarketRate" MaxLength="20" Width="150px" runat="server" ToolTip="Market Rate"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvMarketRate" runat="server" ControlToValidate="txthMarketRate"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Market Rate" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhMarketRate" runat="server" TargetControlID="txthMarketRate"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhMarketValue" runat="server" Text="Market Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthMarketValue" ContentEditable="false" ToolTip="Market Value"
                                                        Width="150px" MaxLength="20" runat="server" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCollateralRefNo" runat="server" CssClass="styleRegLabel" Text="Collateral Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthCollateralRefNo" ContentEditable="false" Width="80px" ToolTip="Collateral Ref No"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhScanRefNo" runat="server" CssClass="styleRegLabel" Text="Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthScanRefNo" runat="server" MaxLength="10" Width="90px" ToolTip="Scan Ref No"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexhScanRefNo" runat="server" TargetControlID="txthScanRefNo"
                                                        FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhItemRefNo" runat="server" CssClass="styleRegLabel" Text="Item Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthItemRefNo" ContentEditable="false" runat="server" Width="90px"
                                                        ToolTip="Item Ref No"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblTranRefNo" runat="server" CssClass="styleRegLabel" Text="Tran Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthTranRefNo" runat="server" ToolTip="Tran Ref No" Width="90px"
                                                        ContentEditable="false"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhownership" runat="server" CssClass="styleReqFieldLabel" Text="Ownership %"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthownership" MaxLength="3" Width="60px" runat="server" ToolTip="Ownership %"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvhownership" runat="server" ControlToValidate="txthownership"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Ownership %" ValidationGroup="AddHighLiqSec1"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="Ftexhownership" runat="server" TargetControlID="txthownership"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%" valign="top" height="30px">
                                                    <asp:Button ID="btnAddH" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                        ValidationGroup="AddHighLiqSec1" OnClick="btnAddH_Click" />
                                                    <asp:Button ID="btnModifyH" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddHighLiqSec1" Enabled="False" OnClick="btnModifyH_Click" />
                                                    <asp:Button ID="btnhClearH" runat="server" CausesValidation="False" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearH_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvhHighLiqDetails" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvHighLiqDetails_RowDeleting"
                                            OnRowDataBound="gvHighLiqDetails_RowDataBound" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblMode" runat="server" Text='<%# Bind("Mode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" OnCheckedChanged="rdHSelect_CheckedChanged"
                                                            AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Issued By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssuedBy" runat="server" Text='<%# Bind("Issued_By") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No of Units">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNoOfUnits" runat="server" Text='<%# Bind("No_Of_Units") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Interest Percent">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInterest" runat="server" Text='<%# Bind("Interest_Percentage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Bind("Maturity_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityValue" runat="server" Text='<%# Bind("Maturity_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketRate" runat="server" Text='<%# Bind("Market_Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketValue" runat="server" Text='<%# Bind("Market_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ownership %">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblownership" runat="server" Text='<%# Bind("Ownership") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvHighLiqDetails" runat="server" Visible="False" AutoGenerateColumns="False"
                                            ShowFooter="True" OnRowCommand="gvHighLiqDetails_RowCommand" OnRowDeleting="gvHighLiqDetails_RowDeleting"
                                            OnRowCancelingEdit="gvHighLiqDetails_RowCancelingEdit" OnRowUpdating="gvHighLiqDetails_RowUpdating"
                                            OnRowEditing="gvHighLiqDetails_RowEditing" OnRowDataBound="gvHighLiqDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlCollSecurities" ToolTip="Collateral Securities" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCollSecurities" runat="server" ControlToValidate="ddlCollSecurities"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                            InitialValue="0" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Issued By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssuedBy" runat="server" Text='<%# Bind("Issued_By") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtIssuedBy" runat="server" MaxLength="20" ToolTip="Issued By" Text='<%# Bind("Issued_By") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvIssuedBy" runat="server" ControlToValidate="txtIssuedBy"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Issued By" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FtexIssuedBy" runat="server" ValidChars=" " TargetControlID="txtIssuedBy"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Demat">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDemat" Visible="false" runat="server" Text='<%# Bind("Demat") %>'></asp:Label>
                                                        <asp:Label ID="lblDematDesc" runat="server" Text='<%# Bind("DematDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlDemat" runat="server" ToolTip="Demat">
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DP Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDPName" runat="server" Text='<%# Bind("Dp_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDPName" MaxLength="20" ToolTip="DP Name" onKeyPress="trimStartingSpace(this)"
                                                            runat="server" Text='<%# Bind("Dp_Name") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDPName" runat="server" ControlToValidate="txtDPName"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter DP Name" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DP No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDPNo" runat="server" Text='<%# Bind("Dp_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDPNo" runat="server" MaxLength="15" ToolTip="DP No" Text='<%# Bind("Dp_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexDPNo" runat="server" TargetControlID="txtDPNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="/-#">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvDPNo" runat="server" ControlToValidate="txtDPNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter DP No" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Client ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClientID" runat="server" Text='<%# Bind("Client_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtClientID" runat="server" ToolTip="Client ID" MaxLength="15" Text='<%# Bind("Client_ID") %>'>
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexClientID" runat="server" TargetControlID="txtClientID"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="/-#">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvClientID" runat="server" ControlToValidate="txtClientID"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Client ID" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Certificate No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCertificateNo" runat="server" Text='<%# Bind("Certificate_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCertificateNo" runat="server" ToolTip="Certificate No" MaxLength="20"
                                                            Text='<%# Bind("Certificate_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexCertificateNo" runat="server" TargetControlID="txtCertificateNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="/-#">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvCertificateNo" Enabled="false" runat="server"
                                                            ControlToValidate="txtCertificateNo" Display="None" SetFocusOnError="true" ErrorMessage="Enter Certificate No"
                                                            ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Folio No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFolioNo" runat="server" Text='<%# Bind("Folio_no") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtFolioNo" runat="server" ToolTip="Folio No" MaxLength="20" Text='<%# Bind("Folio_no") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexFolioNo" runat="server" TargetControlID="txtFolioNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="/-#">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Face Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitFaceValue" runat="server" Text='<%# Bind("Unit_Face_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtUnitFaceValue" runat="server" ToolTip="Unit Face Value" MaxLength="12"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Unit_Face_Value") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexUnitFaceValue" runat="server" TargetControlID="txtUnitFaceValue"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvUnitFaceValue" runat="server" ControlToValidate="txtUnitFaceValue"
                                                            Display="None" ErrorMessage="Enter Unit Face Value" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No of Units">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNoOfUnits" runat="server" Text='<%# Bind("No_Of_Units") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNoOfUnits" runat="server" ToolTip="No of Units" MaxLength="10"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("No_Of_Units") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexNoOfUnits" runat="server" TargetControlID="txtNoOfUnits"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvNoOfUnits" runat="server" ControlToValidate="txtNoOfUnits"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter No Of Units" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Interest Percent">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInterest" runat="server" Text='<%# Bind("Interest_Percentage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtInterest" runat="server" MaxLength="5" ToolTip="Interest Percent"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Interest_Percentage") %>'></asp:TextBox>
                                                        <%--<cc1:FilteredTextBoxExtender ID="FtexInterest" runat="server" TargetControlID="txtInterest"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvInterest" runat="server" ControlToValidate="txtInterest"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Interest Percent" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Bind("Maturity_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMaturityDate" ContentEditable="false" ToolTip="Maturity Date"
                                                            runat="server" Text='<%# Bind("Maturity_Date") %>'></asp:TextBox>
                                                        <cc1:CalendarExtender ID="CEMaturityDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="Image1"
                                                            TargetControlID="txtMaturityDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvMaturityDate" runat="server" ControlToValidate="txtMaturityDate"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Maturity Date" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityValue" runat="server" Text='<%# Bind("Maturity_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMaturityValue" runat="server" MaxLength="12" ToolTip="Maturity Value"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Maturity_Value") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexMaturityValue" runat="server" TargetControlID="txtMaturityValue"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvMaturityValue" runat="server" ControlToValidate="txtMaturityValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Maturity Value" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketRate" runat="server" Text='<%# Bind("Market_Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMarketRate" runat="server" MaxLength="20" ToolTip="Market Rate"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Market_Rate") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvMarketRate" runat="server" ControlToValidate="txtMarketRate"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Market Rate" ValidationGroup="AddHighLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FtexMarketRate" runat="server" TargetControlID="txtMarketRate"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketValue" runat="server" Text='<%# Bind("Market_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMarketValue" runat="server" ContentEditable="false" ToolTip="Market Value"
                                                            MaxLength="20" onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Market_Value") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralRefNo" runat="server" Text='<%# Bind("Collateral_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCollateralRefNo" ContentEditable="false" ToolTip="Collateral Ref No"
                                                            runat="server" Text='<%# Bind("Collateral_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblScanRefNo" runat="server" Text='<%# Bind("Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtScanRefNo" runat="server" MaxLength="10" ToolTip="Scan Ref No"
                                                            Text='<%# Bind("Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexScanRefNo" runat="server" TargetControlID="txtScanRefNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemRefNo" runat="server" Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtItemRefNo" runat="server" ContentEditable="false" ToolTip="Item Ref No"
                                                            Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tran Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTranRefNo" runat="server" Text='<%# Bind("Collateral_Tran_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtTranRefNo" runat="server" ToolTip="Tran Ref No" ContentEditable="false"
                                                            Text='<%# Bind("Collateral_Tran_No") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtTranRefNo" runat="server" ToolTip="Tran Ref No" ContentEditable="false"
                                                            Text='<%# Bind("Collateral_Tran_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            ToolTip="Update" CausesValidation="true" ValidationGroup="AddHighLiqSec"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            ToolTip="Cancel" CausesValidation="false"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" ToolTip="Edit"
                                                            CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add" CommandName="AddNew" ToolTip="Add"
                                                            CausesValidation="true" ValidationGroup="AddHighLiqSec"></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <asp:ValidationSummary ID="vgHighLIQSec" runat="server" ValidationGroup="AddHighLiqSec1"
                                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Medium Liquid Securities" ID="tabMedSeq"
                                CssClass="tabpan" BackColor="Red" ToolTip="Medium Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    Medium Liquid Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlMedLiqDetails" ScrollBars="Horizontal" CssClass="stylePanel"
                                        GroupingText="Medium Liquid Security Details">
                                        <asp:Label ID="lblMedLiqDetails" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMCollSecurities" runat="server" CssClass="styleReqFieldLabel" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblMSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblMMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlMCollSecurities" ToolTip="Collateral Securities" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvMCollSecurities" runat="server" ControlToValidate="ddlMCollSecurities"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                        InitialValue="0" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDescription" runat="server" CssClass="styleReqFieldLabel" Text="Description"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMDescription" TextMode="MultiLine" MaxLength="100" ToolTip="Description"
                                                        onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvMDescription" runat="server" ControlToValidate="txtMDescription"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Description" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMModel" runat="server" CssClass="styleReqFieldLabel" Text="Model"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMModel" runat="server" MaxLength="4" Width="50" ToolTip="Model"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexModel" runat="server" TargetControlID="txtMModel"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvMModel" runat="server" ControlToValidate="txtMModel"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Model" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMYear" runat="server" CssClass="styleReqFieldLabel" Text="Year"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMYear" runat="server" MaxLength="4" Width="50px" ToolTip="Year"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexMYear" runat="server" TargetControlID="txtMYear"
                                                        FilterType="custom,Numbers" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvMYear" runat="server" ControlToValidate="txtMYear"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Year" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMRegistrationNo" CssClass="styleReqFieldLabel" runat="server" Text="Registration No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMRegistrationNo" runat="server" ToolTip="Registration No" MaxLength="12">
                                                    </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexMRegistrationNo" runat="server" TargetControlID="txtMRegistrationNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="-/">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvMRegistration" runat="server" ControlToValidate="txtMRegistrationNo"
                                                        Display="None" ErrorMessage="Enter Registration No" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMSerialNo" runat="server" CssClass="styleReqFieldLabel" Text="Serial No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMSerialNo" runat="server" ToolTip="Serial No" Width="90px" MaxLength="12"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexSerialNo" runat="server" TargetControlID="txtMSerialNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="-/">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvMSerialNo" runat="server" ControlToValidate="txtMSerialNo"
                                                        Display="None" ErrorMessage="Enter Serial No" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMValue" runat="server" CssClass="styleReqFieldLabel" Text="Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMValue" runat="server" ToolTip="Value" Width="90px" MaxLength="12"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexMValue" runat="server" TargetControlID="txtMValue"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars="." />
                                                    <asp:RequiredFieldValidator ID="rfvMValue" runat="server" ControlToValidate="txtMValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Value" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMMarketValue" runat="server" CssClass="styleReqFieldLabel" Text="Market Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMMarketValue" runat="server" MaxLength="12" Width="90px" ToolTip="Market Value"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexMMarketValue" runat="server" TargetControlID="txtMMarketValue"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars="." />
                                                    <asp:RequiredFieldValidator ID="rfvMMarketValue" runat="server" ControlToValidate="txtMMarketValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Market Value" ValidationGroup="AddMediumLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMCollateralRefNo" runat="server" CssClass="styleRegLabel" Text="Collateral Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMCollateralRefNo" ReadOnly="true" ToolTip="Collateral Ref No"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMScanRefNo" runat="server" CssClass="styleRegLabel" Text="Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMScanRefNo" MaxLength="10" Width="90px" ToolTip="Scan Ref No"
                                                        runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexMScanRefNo" runat="server" TargetControlID="txtMScanRefNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMItemRefNo" runat="server" CssClass="styleRegLabel" Text="Item Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMItemRefNo" ReadOnly="true" runat="server" Width="90px" ToolTip="Item Ref No"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMOwnership" runat="server" CssClass="styleReqFieldLabel" Text="Ownership %"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMOwnership" MaxLength="3" Width="60px" runat="server" ToolTip="Ownership %"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvMOwnership" runat="server" ControlToValidate="txtMOwnership"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Ownership %" ValidationGroup="AddMediumLiqSec"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexMOwnership" runat="server" TargetControlID="txtMOwnership"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <asp:Button ID="btnAddM" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                        ValidationGroup="AddMediumLiqSec" OnClick="btnAddM_Click" />
                                                    <asp:Button ID="btnModifyM" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddMediumLiqSec" OnClick="btnModifyM_Click" />
                                                    <asp:Button ID="btnClearM" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearM_Click" />
                                                    <%-- <input id="hdnBankId" runat="server" type="hidden"></input>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvMMedLiqDetails" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvMedLiqDetails_RowDeleting"
                                            OnRowDataBound="gvMedLiqDetails_RowDataBound" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblMode" runat="server" Text='<%# Bind("Mode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" OnCheckedChanged="rdMSelect_CheckedChanged"
                                                            AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSlNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Model" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModel" runat="server" Text='<%# Bind("Model") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblYear" runat="server" Text='<%# Bind("Year") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Registration No" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%# Bind("Registration_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Serial No" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("Serial_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Value" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Value" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketValue" runat="server" Text='<%# Bind("Market_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ownership %" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMOwnership" runat="server" Text='<%# Bind("Ownership_Medium") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvMedLiqDetails" runat="server" AutoGenerateColumns="False" Visible="false"
                                            ShowFooter="True" OnRowCommand="gvMedLiqDetails_RowCommand" OnRowDeleting="gvMedLiqDetails_RowDeleting"
                                            OnRowCancelingEdit="gvMedLiqDetails_RowCancelingEdit" OnRowEditing="gvMedLiqDetails_RowEditing"
                                            OnRowUpdating="gvMedLiqDetails_RowUpdating" OnRowDataBound="gvMedLiqDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSlNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlCollSecurities" runat="server" ToolTip="Collateral Securities">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCollSecurities" runat="server" ControlToValidate="ddlCollSecurities"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                            InitialValue="0" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblDescription" TextMode="MultiLine" ContentEditable="false" runat="server"
                                                            Style="text-align: left" Text='<%# Bind("Description") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="100" ToolTip="Description"
                                                            onkeyup="maxlengthfortxt(100);" onkeypress="trimStartingSpace(this)" runat="server"
                                                            Text='<%# Bind("Description") %>'>
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Description" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Model">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblModel" runat="server" Text='<%# Bind("Model") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtModel" runat="server" MaxLength="4" ToolTip="Model" Text='<%# Bind("Model") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexModel" runat="server" TargetControlID="txtModel"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="txtModel"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Model" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblYear" runat="server" Text='<%# Bind("Year") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtYear" runat="server" MaxLength="4" ToolTip="Year" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            Text='<%# Bind("Year") %>'>
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexYear" runat="server" TargetControlID="txtYear"
                                                            FilterType="custom,Numbers" Enabled="True">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="txtYear"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Year" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Registration No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegistrationNo" runat="server" Text='<%# Bind("Registration_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtRegistrationNo" runat="server" ToolTip="Registration No" MaxLength="12"
                                                            Text='<%# Bind("Registration_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexRegistrationNo" runat="server" TargetControlID="txtRegistrationNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="-/" />
                                                        <asp:RequiredFieldValidator ID="rfvRegistration" runat="server" ControlToValidate="txtRegistrationNo"
                                                            Display="None" ErrorMessage="Enter Registration No" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Serial No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("Serial_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtSerialNo" runat="server" ToolTip="Serial No" MaxLength="12" Text='<%# Bind("Serial_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexSerialNo" runat="server" TargetControlID="txtSerialNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="-/">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvSerialNo" runat="server" ControlToValidate="txtSerialNo"
                                                            Display="None" ErrorMessage="Enter Serial No" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtValue" runat="server" MaxLength="12" ToolTip="Value" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            Text='<%# Bind("Value") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexValue" runat="server" TargetControlID="txtValue"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars="." />
                                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Value" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketValue" runat="server" Text='<%# Bind("Market_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMarketValue" runat="server" MaxLength="12" ToolTip="Market Value"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Market_Value") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexMarketValue" runat="server" TargetControlID="txtMarketValue"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars="." />
                                                        <asp:RequiredFieldValidator ID="rfvMarketValue" runat="server" ControlToValidate="txtMarketValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Market Value" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralRefNo" runat="server" Text='<%# Bind("Collateral_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCollateralRefNo" ReadOnly="true" ToolTip="Collateral Ref No"
                                                            runat="server" Text='<%# Bind("Collateral_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblScanRefNo" runat="server" Text='<%# Bind("Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<asp:RequiredFieldValidator ID="rfvScanRefNo" runat="server" ControlToValidate="txtScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Scan Ref No" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtScanRefNo" MaxLength="10" ToolTip="Scan Ref No" runat="server"
                                                            Text='<%# Bind("Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexScanRefNo" runat="server" TargetControlID="txtScanRefNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="" />
                                                        <%-- <asp:RequiredFieldValidator ID="rfvScanRefNo" runat="server" ControlToValidate="txtScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Scan Ref No" ValidationGroup="AddMediumLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemRefNo" runat="server" Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtItemRefNo" ReadOnly="true" runat="server" ToolTip="Item Ref Nof"
                                                            Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtItemRefNo" ReadOnly="true" runat="server" ToolTip="Item Ref Nof"
                                                            Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            ToolTip="Update" CausesValidation="true" ValidationGroup="AddMediumLiqSec"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            ToolTip="Cancel" CausesValidation="false"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                            ToolTip="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add" CommandName="AddNew" CausesValidation="true"
                                                            ToolTip="Add" ValidationGroup="AddMediumLiqSec"></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            ToolTip="Delete" CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <%-- </div>--%>
                                        <asp:ValidationSummary ID="vgMediumLIQSec" runat="server" ValidationGroup="AddMediumLiqSec"
                                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                                            ShowSummary="true" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Low Liquid Securities" ID="tabLow" CssClass="tabpan"
                                BackColor="Red" ToolTip="Low Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    Low Liquid Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlLowLiqDetails" ScrollBars="Horizontal" CssClass="stylePanel"
                                        GroupingText="Low Liquid Security Details">
                                        <asp:Label ID="lblLowLiquidsecurites" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCollSecurities" runat="server" CssClass="styleReqFieldLabel" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblLSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblLMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLCollSecurities" runat="server" ToolTip="Collateral Securities">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvLCollSecurities" runat="server" ControlToValidate="ddlLCollSecurities"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                        InitialValue="0" ValidationGroup="AddLOWLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLLocationDetails" runat="server" CssClass="styleReqFieldLabel"
                                                        Text="Location 
                                                            Details"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLLocationDetails" runat="server" ToolTip="Location Details" TextMode="MultiLine"
                                                        MaxLength="60" onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(60);">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvLLocationDetails" runat="server" ControlToValidate="txtLLocationDetails"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Location Details" ValidationGroup="AddLOWLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLMeasurement" runat="server" CssClass="styleReqFieldLabel" Text="Measurement"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLMeasurement" runat="server" MaxLength="12" Width="120px" ToolTip="Measurement"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLMeasurement" runat="server" TargetControlID="txtLMeasurement"
                                                        FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvLMeasurement" runat="server" ControlToValidate="txtLMeasurement"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter  Measurement" ValidationGroup="AddLOWLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLUnitRate" runat="server" CssClass="styleReqFieldLabel" Text="Unit Rate"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLUnitRate" runat="server" MaxLength="12" Width="120px" ToolTip="Unit Rate"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                    </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLUnitRate" runat="server" TargetControlID="txtLUnitRate"
                                                        FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvLUnitRate" runat="server" ControlToValidate="txtLUnitRate"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter  Unit Rate" ValidationGroup="AddLOWLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLValue" runat="server" CssClass="styleRegLabel" Text="Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLValue" ReadOnly="false" ContentEditable="false" ToolTip="Value"
                                                        runat="server" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvLValue" runat="server" ControlToValidate="txtLValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Value" ValidationGroup="AddLOWLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLMarketValue" runat="server" CssClass="styleReqFieldLabel" Text="Market Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLMarketValue" runat="server" MaxLength="12" Width="120px" ToolTip="Market Value"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLMarketValue" runat="server" TargetControlID="txtLMarketValue"
                                                        FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvLMarketValue" runat="server" ControlToValidate="txtLMarketValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter  Market Value" ValidationGroup="AddLOWLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCollateralRefNo" runat="server" CssClass="styleRegLabel" Text="Coll Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLCollateralRefNo" ReadOnly="true" ToolTip="Coll Ref No" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLScanRefNo" runat="server" CssClass="styleRegLabel" Text="Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLScanRefNo" runat="server" MaxLength="10" Width="120px" ToolTip="Scan Ref No">
                                                    </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLScanRefNo" runat="server" TargetControlID="txtLScanRefNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLLegalOpinionDesc" runat="server" CssClass="styleRegLabel" Text="Legal Opinion"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLLegalOpinion" runat="server" ToolTip="Legal Opinion">
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLLegalScanRefNo" runat="server" CssClass="styleRegLabel" Text="Legal Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLLegalScanRefNo" runat="server" MaxLength="10" Width="120px"
                                                        ToolTip="Legal Scan Ref No">
                                                    </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLLegalScanRefNo" runat="server" TargetControlID="txtLLegalScanRefNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLEncumbranceDesc" runat="server" CssClass="styleRegLabel" Text="Encumbrance"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLEncumbrance" runat="server" ToolTip="Encumbrance">
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLEncumbranceScanRefNo" runat="server" CssClass="styleRegLabel"
                                                        Text="Encumbrance Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLEncumbranceScanRefNo" runat="server" MaxLength="10" Width="120px"
                                                        ToolTip="Encumbrance Scan Ref No">
                                                    </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLEncumbranceScanRefNo" runat="server" TargetControlID="txtLEncumbranceScanRefNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLAssetDocumentDesc" runat="server" CssClass="styleRegLabel" Text="Asset Document"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLAssetDocument" runat="server" ToolTip="Asset Document">
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLAssetDocScanRefNo" runat="server" CssClass="styleRegLabel" Text="Asset Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLAssetDocScanRefNo" runat="server" MaxLength="10" Width="120px"
                                                        ToolTip="Asset Scan Ref No">
                                                    </asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLAssetDocScanRefNo" runat="server" TargetControlID="txtLAssetDocScanRefNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLValuationCertificateDesc" runat="server" CssClass="styleRegLabel"
                                                        Text="Valuation Certificate"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLValuationCertificate" runat="server" ToolTip="Valuation Certificate">
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="0">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLValCertificationScanRefNo" runat="server" CssClass="styleRegLabel"
                                                        Text="Valuation Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLValCertificationScanRefNo" MaxLength="10" Width="120px" ToolTip="Valuation Scan Ref No"
                                                        runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLValCertificationScanRefNo" runat="server" TargetControlID="txtLValCertificationScanRefNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLItemRefNo" runat="server" CssClass="styleRegLabel" Text="Item Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLItemRefNo" ReadOnly="true" ToolTip="Item Ref No" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLOwnership" runat="server" CssClass="styleReqFieldLabel" Text="Ownership %"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLOwnership" MaxLength="3" Width="60px" runat="server" ToolTip="Ownership %"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvLOwnership" runat="server" ControlToValidate="txtLOwnership"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Ownership %" ValidationGroup="AddLOWLiqSec"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLOwnership" runat="server" TargetControlID="txtLOwnership"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <asp:Button ID="btnAddL" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                        ValidationGroup="AddLOWLiqSec" OnClick="btnAddL_Click" />
                                                    <asp:Button ID="btnModifyL" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddLOWLiqSec" OnClick="btnModifyL_Click" />
                                                    <asp:Button ID="btnClearL" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearL_Click" />
                                                    <%-- <input id="hdnBankId" runat="server" type="hidden"></input>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvLLowLiqDetails" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvLowLiqDetails_RowDeleting"
                                            OnRowDataBound="gvLowLiqDetails_RowDataBound" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblMode" runat="server" Text='<%# Bind("Mode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" OnCheckedChanged="rdLSelect_CheckedChanged"
                                                            AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No" Visible="true" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Details">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocationDetails" runat="server" Text='<%# Bind("Location_Details") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Value" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketValue" runat="server" Text='<%# Bind("Market_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Legal Opinion">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLegalOpinion" runat="server" Visible="false" Text='<%# Bind("Legal_Opinion") %>'></asp:Label>
                                                        <asp:Label ID="lblLegalOpinionDesc" runat="server" Text='<%# Bind("Legal_OpinionDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Encumbrance Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncumbranceScanRefNo" runat="server" Text='<%# Bind("Encumbrance_Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ownership %" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLOwnership" runat="server" Text='<%# Bind("Ownership_Low") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Document">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetDocument" runat="server" Visible="false" Text='<%# Bind("Is_Asset_Document") %>'></asp:Label>
                                                        <asp:Label ID="lblAssetDocumentDesc" runat="server" Text='<%# Bind("Is_Asset_DocumentDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvLowLiqDetails" runat="server" Visible="false" AutoGenerateColumns="false"
                                            ShowFooter="true" OnRowCommand="gvLowLiqDetails_RowCommand" OnRowDeleting="gvLowLiqDetails_RowDeleting"
                                            OnRowCancelingEdit="gvLowLiqDetails_RowCancelingEdit" OnRowEditing="gvLowLiqDetails_RowEditing"
                                            OnRowUpdating="gvLowLiqDetails_RowUpdating" OnRowDataBound="gvLowLiqDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlCollSecurities" runat="server" ToolTip="Collateral Securities">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCollSecurities" runat="server" ControlToValidate="ddlCollSecurities"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                            InitialValue="0" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Details">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblLocationDetails" TextMode="MultiLine" ContentEditable="False"
                                                            runat="server" Text='<%# Bind("Location_Details") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtLocationDetails" runat="server" ToolTip="Location Details" TextMode="MultiLine"
                                                            MaxLength="60" onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(60);"
                                                            Text='<%# Bind("Location_Details") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvLocationDetails" runat="server" ControlToValidate="txtLocationDetails"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Location Details" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Measurement">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMeasurement" runat="server" Text='<%# Bind("Measurement") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMeasurement" runat="server" MaxLength="12" ToolTip="Measurement"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Measurement") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexMeasurement" runat="server" TargetControlID="txtMeasurement"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvMeasurement" runat="server" ControlToValidate="txtMeasurement"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter  Measurement" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%# Bind("Unit_Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtUnitRate" runat="server" MaxLength="12" ToolTip="Unit Rate" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            Text='<%# Bind("Unit_Rate") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexUnitRate" runat="server" TargetControlID="txtUnitRate"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvUnitRate" runat="server" ControlToValidate="txtUnitRate"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter  Unit Rate" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtValue" runat="server" ContentEditable="false" ToolTip="Value"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Value") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter  Value" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Market Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMarketValue" runat="server" Text='<%# Bind("Market_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMarketValue" runat="server" MaxLength="12" ToolTip="Market Value"
                                                            onkeypress="fnAllowNumbersOnly(true,false,this)" Text='<%# Bind("Market_Value") %>'>
                                                        </asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexMarketValue" runat="server" TargetControlID="txtMarketValue"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvMarketValue" runat="server" ControlToValidate="txtMarketValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter  Market Value" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Coll Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralRefNo" runat="server" Text='<%# Bind("Collateral_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCollateralRefNo" ReadOnly="true" ToolTip="Coll Ref No" runat="server"
                                                            Text='<%# Bind("Collateral_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblScanRefNo" runat="server" Text='<%# Bind("Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<asp:RequiredFieldValidator ID="rfvScanRefNo" runat="server" ControlToValidate="txtScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter  Scan Ref No" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtScanRefNo" runat="server" MaxLength="10" ToolTip="Scan Ref No"
                                                            Text='<%# Bind("Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexScanRefNo" runat="server" TargetControlID="txtScanRefNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="rfvScanRefNo" runat="server" ControlToValidate="txtScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Scan Ref No" ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Legal Opinion">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLegalOpinion" runat="server" Visible="false" Text='<%# Bind("Legal_Opinion") %>'></asp:Label>
                                                        <asp:Label ID="lblLegalOpinionDesc" runat="server" Text='<%# Bind("Legal_OpinionDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlLegalOpinion" runat="server" Width="120px" ToolTip="Legal Opinion">
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Legal Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLegalScanRefNo" runat="server" Text='<%# Bind("Legal_Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<asp:RequiredFieldValidator ID="rfvLegalScanRefNo" runat="server" ControlToValidate="txtLegalScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Legal Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtLegalScanRefNo" runat="server" MaxLength="10" ToolTip="Legal Scan Ref No"
                                                            Text='<%# Bind("Legal_Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexLegalScanRefNo" runat="server" TargetControlID="txtLegalScanRefNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="rfvLegalScanRefNo" runat="server" ControlToValidate="txtLegalScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Legal Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Encumbrance">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncumbrance" runat="server" Visible="false" Text='<%# Bind("Encumbrance") %>'></asp:Label>
                                                        <asp:Label ID="lblEncumbranceDesc" runat="server" Text='<%# Bind("EncumbranceDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlEncumbrance" runat="server" Width="120px" ToolTip="Encumbrance">
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Encumbrance Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEncumbranceScanRefNo" runat="server" Text='<%# Bind("Encumbrance_Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<asp:RequiredFieldValidator ID="rfvEncubranceScanRefNo" runat="server" ControlToValidate="txtEncumbranceScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Encumbrance Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtEncumbranceScanRefNo" runat="server" MaxLength="10" ToolTip="Encumbrance Scan Ref No"
                                                            Text='<%# Bind("Encumbrance_Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexEncumbranceScanRefNo" runat="server" TargetControlID="txtEncumbranceScanRefNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="rfvEncubranceScanRefNo" runat="server" ControlToValidate="txtEncumbranceScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Encumbrance Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Document">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetDocument" runat="server" Visible="false" Text='<%# Bind("Is_Asset_Document") %>'></asp:Label>
                                                        <asp:Label ID="lblAssetDocumentDesc" runat="server" Text='<%# Bind("Is_Asset_DocumentDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlAssetDocument" runat="server" Width="120px" ToolTip="Asset Document">
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Asset Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAssetDocScanRefNo" runat="server" Text='<%# Bind("AssetDoc_Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<asp:RequiredFieldValidator ID="rfvAssetScanRefNo" runat="server" ControlToValidate="txtAssetDocScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Asset Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtAssetDocScanRefNo" runat="server" MaxLength="10" ToolTip="Asset Scan Ref No"
                                                            Text='<%# Bind("AssetDoc_Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexAssetDocScanRefNo" runat="server" TargetControlID="txtAssetDocScanRefNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="rfvAssetScanRefNo" runat="server" ControlToValidate="txtAssetDocScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Asset Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valuation Certificate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValuationCertificate" runat="server" Visible="false" Text='<%# Bind("Is_Valuation_Certification") %>'></asp:Label>
                                                        <asp:Label ID="lblValuationCertificateDesc" runat="server" Text='<%# Bind("Is_Valuation_CertificationDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlValuationCertificate" runat="server" Width="120px" ToolTip="Valuation Certificate">
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valuation Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValCertificationScanRefNo" runat="server" Text='<%# Bind("ValCertification_Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<asp:RequiredFieldValidator ID="rfvValCertificationScanRefNo" runat="server" ControlToValidate="txtValCertificationScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Valuation Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtValCertificationScanRefNo" MaxLength="10" ToolTip="Valuation Scan Ref No"
                                                            runat="server" Text='<%# Bind("ValCertification_Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexValCertificationScanRefNo" runat="server" TargetControlID="txtValCertificationScanRefNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="rfvValCertificationScanRefNo" runat="server" ControlToValidate="txtValCertificationScanRefNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Valuation Scan Ref No"
                                                            ValidationGroup="AddLOWLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemRefNo" runat="server" Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtItemRefNo" ReadOnly="true" ToolTip="Item Ref No" runat="server"
                                                            Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Exclude" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRelease" Checked='<%# GVBoolFormat(Eval("Is_Release").ToString()) %>'
                                                            runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            ToolTip="Update" CausesValidation="true" ValidationGroup="AddLOWLiqSec"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            ToolTip="Cancel" CausesValidation="false"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                            ToolTip="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add" CommandName="AddNew" CausesValidation="true"
                                                            ToolTip="Add" ValidationGroup="AddLOWLiqSec"></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            ToolTip="Delete" CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <asp:ValidationSummary ID="vgLowLIQSec" runat="server" ValidationGroup="AddLOWLiqSec"
                                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                                            ShowSummary="true" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Commodities Securities" ID="tabCommodities"
                                CssClass="tabpan" BackColor="Red" ToolTip="Commodities Securities" Width="99%">
                                <HeaderTemplate>
                                    Commodities Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlCommodityDetails" ScrollBars="Horizontal" CssClass="stylePanel"
                                        GroupingText="Commodity Details">
                                        <asp:Label ID="lblCommSecurities" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCCollSecurities" runat="server" CssClass="styleReqFieldLabel" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblCSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblCMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlCCollSecurities" runat="server" ToolTip="Collateral Securities">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvCCollSecurities" runat="server" ControlToValidate="ddlCCollSecurities"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                        InitialValue="0" ValidationGroup="AddCOMMODITIESLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCDescription" ToolTip="Description" CssClass="styleReqFieldLabel"
                                                        runat="server" Text="Description"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCDescription" TextMode="MultiLine" ToolTip="Description" MaxLength="100"
                                                        onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCDescription" runat="server" ControlToValidate="txtCDescription"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Description" ValidationGroup="AddCOMMODITIESLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCUnitOfMeasure" CssClass="styleReqFieldLabel" runat="server" Text="Unit Of Measure"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlCUnitOfMeasure" runat="server" Width="120px" ToolTip="Unit Of Measure">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvCUnitOfMeasure" runat="server" ControlToValidate="ddlCUnitOfMeasure"
                                                        Display="None" SetFocusOnError="true" InitialValue="0" ErrorMessage="Select Unit of Measure"
                                                        ValidationGroup="AddCOMMODITIESLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCUnitQty" runat="server" CssClass="styleReqFieldLabel" Text="Unit Qty"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCUnitQty" MaxLength="5" Width="50px" runat="server" ToolTip="Unit Qty"
                                                        onkeypress="fnAllowNumbersOnly(false,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexCUnitQty" runat="server" TargetControlID="txtCUnitQty"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvCUnitQty" runat="server" ControlToValidate="txtCUnitQty"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Unit Qty" ValidationGroup="AddCOMMODITIESLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCUnitPrice" runat="server" CssClass="styleReqFieldLabel" Text="Unit Price"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCUnitPrice" MaxLength="12" Width="120px" ToolTip="Unit Price"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexCUnitPrice" runat="server" TargetControlID="txtCUnitPrice"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvCUnitPrice" runat="server" ControlToValidate="txtCUnitPrice"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Unit Price" ValidationGroup="AddCOMMODITIESLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCValue" runat="server" CssClass="styleRegLabel" Text="Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCValue" onkeypress="fnAllowNumbersOnly(true,false,this)" ToolTip="Value"
                                                        runat="server" Width="120px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCValue" runat="server" ControlToValidate="txtCValue"
                                                        Enabled="false" Display="None" SetFocusOnError="true" ErrorMessage="Enter Value"
                                                        ValidationGroup="AddCOMMODITIESLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCUnitMarketPrice" runat="server" CssClass="styleRegLabel" Text="Unit Market Price"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCUnitMarketPrice" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                        ToolTip="Unit Market Price" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexCUnitMarketPrice" runat="server" TargetControlID="txtCUnitMarketPrice"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDate" runat="server" CssClass="styleRegLabel" Text="Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCDate" runat="server" ToolTip="Date"></asp:TextBox>
                                                    <asp:Image ID="imgCDate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Date"
                                                        Visible="false" />
                                                    <cc1:CalendarExtender ID="CEDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                        PopupButtonID="Image1" TargetControlID="txtCDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCItemRefNo" runat="server" CssClass="styleRegLabel" Text="Item ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCItemRefNo" ReadOnly="true" runat="server" ToolTip="Item Ref No"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCOwnership" runat="server" CssClass="styleReqFieldLabel" Text="Ownership %"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCOwnership" MaxLength="3" Width="60px" runat="server" ToolTip="Ownership %"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCOwnership" runat="server" ControlToValidate="txtCOwnership"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Ownership %" ValidationGroup="AddCOMMODITIESLiqSec1"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexCOwnership" runat="server" TargetControlID="txtCOwnership"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <asp:Button ID="btnAddC" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                        ValidationGroup="AddCOMMODITIESLiqSec1" OnClick="btnAddC_Click" />
                                                    <asp:Button ID="btnModifyC" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddCOMMODITIESLiqSec1" OnClick="btnModifyC_Click" />
                                                    <asp:Button ID="btnClearC" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearC_Click" />
                                                    <%-- <input id="hdnBankId" runat="server" type="hidden"></input>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvCCommoDetails" runat="server" AutoGenerateColumns="false" OnRowCommand="gvCommoDetails_RowCommand"
                                            OnRowDeleting="gvCommoDetails_RowDeleting" Width="100%" OnRowDataBound="gvCommoDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" OnCheckedChanged="rdCSelect_CheckedChanged"
                                                            AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                        <asp:Label ID="lblMode" runat="server" Visible="false" Text='<%# Bind("Mode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No" Visible="true" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" ToolTip="Description" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Of Measure" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitOfMeasure" Visible="false" runat="server" Text='<%# Bind("Unit_Of_Measure") %>'></asp:Label>
                                                        <asp:Label ID="lblUOMText" runat="server" Text='<%# Bind("UOM_Text") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Qty" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitQty" runat="server" Text='<%# Bind("Unit_Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Market Price" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitMarketPrice" runat="server" Text='<%# Bind("Unit_Market_Price") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ownership %" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCOwnership" runat="server" Text='<%# Bind("Ownership_Comm") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvCommoDetails" runat="server" Visible="false" AutoGenerateColumns="false"
                                            ShowFooter="true" OnRowCommand="gvCommoDetails_RowCommand" OnRowDeleting="gvCommoDetails_RowDeleting"
                                            OnRowCancelingEdit="gvCommoDetails_RowCancelingEdit" OnRowEditing="gvCommoDetails_RowEditing"
                                            OnRowUpdating="gvCommoDetails_RowUpdating" OnRowDataBound="gvCommoDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlCollSecurities" runat="server" ToolTip="Collateral Securities">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCollSecurities" runat="server" ControlToValidate="ddlCollSecurities"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                            InitialValue="0" ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblDescription" TextMode="MultiLine" ToolTip="Description" ContentEditable="False"
                                                            runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="100" onkeypress="trimStartingSpace(this)"
                                                            onkeyup="maxlengthfortxt(100);" runat="server" Text='<%# Bind("Description") %>'>
                                                        </asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Description" ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Of Measure">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitOfMeasure" Visible="false" runat="server" Text='<%# Bind("Unit_Of_Measure") %>'></asp:Label>
                                                        <asp:Label ID="lblUOMText" runat="server" Text='<%# Bind("UOM_Text") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlUnitOfMeasure" runat="server" Width="90px" ToolTip="Unit Of Measure">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvUnitOfMeasure" runat="server" ControlToValidate="ddlUnitOfMeasure"
                                                            Display="None" SetFocusOnError="true" InitialValue="0" ErrorMessage="Select Unit of Measure"
                                                            ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Qty">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitQty" runat="server" Text='<%# Bind("Unit_Quantity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtUnitQty" runat="server" MaxLength="5" ToolTip="Unit Qty" onkeypress="fnAllowNumbersOnly(false,false,this)"
                                                            Text='<%# Bind("Unit_Quantity") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexUnitQty" runat="server" TargetControlID="txtUnitQty"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvUnitQty" runat="server" ControlToValidate="txtUnitQty"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Unit Qty" ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Bind("Unit_Price") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtUnitPrice" MaxLength="12" ToolTip="Unit Price" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            runat="server" Text='<%# Bind("Unit_Price") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexUnitPrice" runat="server" TargetControlID="txtUnitPrice"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Unit Price" ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValue" runat="server" Text='<%# Bind("Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtValue" ContentEditable="false" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            ToolTip="Value" runat="server" Text='<%# Bind("Value") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvValue" runat="server" ControlToValidate="txtValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Value" ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Market Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitMarketPrice" runat="server" Text='<%# Bind("Unit_Market_Price") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%--<asp:RequiredFieldValidator ID="rfvUnitMarketPrice" runat="server" ControlToValidate="txtUnitMarketPrice"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Unit Market Price"
                                                            ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtUnitMarketPrice" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            ToolTip="Unit Market Price" runat="server" Text='<%# Bind("Unit_Market_Price") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexUnitMarketPrice" runat="server" TargetControlID="txtUnitMarketPrice"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:RequiredFieldValidator ID="rfvUnitMarketPrice" runat="server" ControlToValidate="txtUnitMarketPrice"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Unit Market Price"
                                                            ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <%-- <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Date" ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtDate" ContentEditable="false" runat="server" Text='<%# Bind("Date") %>'
                                                            ToolTip="Date"></asp:TextBox>
                                                        <asp:Image ID="imgDate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Date"
                                                            Visible="false" />
                                                        <cc1:CalendarExtender ID="CEDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="Image1" TargetControlID="txtDate">
                                                        </cc1:CalendarExtender>
                                                        <%-- <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Date" ValidationGroup="AddCOMMODITIESLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemRefNo" runat="server" Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtItemRefNo" ReadOnly="true" runat="server" ToolTip="Item Ref No"
                                                            Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Exclude" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRelease" Checked='<%# GVBoolFormat(Eval("Is_Release").ToString()) %>'
                                                            runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            ToolTip="Update" CausesValidation="true" ValidationGroup="AddCOMMODITIESLiqSec"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            ToolTip="Cancel" CausesValidation="false"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                            ToolTip="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add" CommandName="AddNew" CausesValidation="true"
                                                            ToolTip="Add" ValidationGroup="AddCOMMODITIESLiqSec"></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            ToolTip="Delete" CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <asp:ValidationSummary ID="vgCommoditiesLIQSec" runat="server" ValidationGroup="AddCOMMODITIESLiqSec1"
                                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                                            ShowSummary="true" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Financial Securities" ID="tabFinancial"
                                CssClass="tabpan" BackColor="Red" ToolTip="Financial Securities" Width="99%">
                                <HeaderTemplate>
                                    Financial Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlFinDetails" ScrollBars="Horizontal" CssClass="stylePanel"
                                        GroupingText="Financial Investment Details">
                                        <%--<table style>
                                            <tr align="center">
                                                <td align="center">--%>
                                        <asp:Label ID="lblFinSecurities" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCollSecurities" runat="server" CssClass="styleReqFieldLabel" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblFSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblFMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlFCollSecurities" runat="server" ToolTip="Collateral Securities">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvFCollSecurities" runat="server" ControlToValidate="ddlFCollSecurities"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                        InitialValue="0" ValidationGroup="AddFINANCIALLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFInsuranceIssuedBy" runat="server" CssClass="styleReqFieldLabel"
                                                        Text="Insurance Issued By"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFInsuranceIssuedBy" MaxLength="20" Width="150px" ToolTip="Insurance Issued By"
                                                        onkeypress="trimStartingSpace(this)" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvFInsuranceIssuedBy" runat="server" ControlToValidate="txtFInsuranceIssuedBy"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Insurance Issued By"
                                                        ValidationGroup="AddFINANCIALLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexFInsuranceIssuedBy" runat="server" TargetControlID="txtFInsuranceIssuedBy"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars=" ">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFPolicyNo" runat="server" CssClass="styleReqFieldLabel" Text="Policy No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFPolicyNo" runat="server" MaxLength="20" Width="150px" ToolTip="Policy No"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexFPolicyNo" runat="server" TargetControlID="txtFPolicyNo"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvFPolicyNo" runat="server" ControlToValidate="txtFPolicyNo"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Policy No" ValidationGroup="AddFINANCIALLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFPolicyValue" runat="server" CssClass="styleReqFieldLabel" Text="Policy Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFPolicyValue" MaxLength="12" Width="120px" ToolTip="Policy Value"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexFPolicyValue" runat="server" TargetControlID="txtFPolicyValue"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvFPolicyValue" runat="server" ControlToValidate="txtFPolicyValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Policy Value" ValidationGroup="AddFINANCIALLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCurrentValue" runat="server" CssClass="styleReqFieldLabel" Text="Current Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFCurrentValue" MaxLength="12" Width="120px" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                        ToolTip="Current Value" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvFCurrentValue" runat="server" ControlToValidate="txtFCurrentValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Current Value" ValidationGroup="AddFINANCIALLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexFCurrentValue" runat="server" TargetControlID="txtFCurrentValue"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFMaturityDate" runat="server" CssClass="styleReqFieldLabel" Text="Maturity Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFMaturityDate" ToolTip="Maturity Date" runat="server"></asp:TextBox>
                                                    <asp:Image ID="imgFMaturityDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                        ToolTip="Select Maturity Date" Visible="false" />
                                                    <cc1:CalendarExtender ID="CEFMaturityDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="Image1"
                                                        TargetControlID="txtFMaturityDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvFMaturityDate" runat="server" ControlToValidate="txtFMaturityDate"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Maturity Date" ValidationGroup="AddFINANCIALLiqSec">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCollateralRef" runat="server" CssClass="styleRegLabel" Text="Collateral Ref"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFCollateralRef" ReadOnly="true" ToolTip="Collateral Ref" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFItemRefNo" runat="server" CssClass="styleRegLabel" Text="Item Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFItemRefNo" ReadOnly="true" ToolTip="Item Ref No" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFScanRefNo" runat="server" CssClass="styleRegLabel" Text="Scan Ref No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFScanRef" runat="server" ToolTip="Scan Ref No" MaxLength="10"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexScanRef" runat="server" TargetControlID="txtFScanRef"
                                                        FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                        ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFOwnership" runat="server" CssClass="styleReqFieldLabel" Text="Ownership %"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFOwnership" MaxLength="3" Width="60px" runat="server" ToolTip="Ownership %"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvFOwnership" runat="server" ControlToValidate="txtFOwnership"
                                                        Display="None" SetFocusOnError="True" ErrorMessage="Enter Ownership %" ValidationGroup="AddFINANCIALLiqSec"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexFOwnership" runat="server" TargetControlID="txtFOwnership"
                                                        FilterType="Custom, Numbers" Enabled="True" ValidChars="">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <asp:Button ID="btnAddF" runat="server" CssClass="styleSubmitShortButton" Text="Add"
                                                        ValidationGroup="AddFINANCIALLiqSec" OnClick="btnAddF_Click" />
                                                    <asp:Button ID="btnModifyF" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddFINANCIALLiqSec" OnClick="btnModifyF_Click" />
                                                    <asp:Button ID="btnClearF" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearF_Click" />
                                                    <%-- <input id="hdnBankId" runat="server" type="hidden"></input>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvFFinDetails" runat="server" AutoGenerateColumns="false" OnRowDeleting="gvFinDetails_RowDeleting"
                                            OnRowDataBound="gvFinDetails_RowDataBound" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblMode" runat="server" Visible="false" Text='<%# Bind("Mode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" OnCheckedChanged="rdFSelect_CheckedChanged"
                                                            AutoPostBack="true" Text="" Style="padding-left: 7px" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No" Visible="true" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy No" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyNo" runat="server" Text='<%# Bind("Policy_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy Value" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyValue" runat="server" Text='<%# Bind("Policy_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Bind("Maturity_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ownership %">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFOwnership" runat="server" Text='<%# Bind("Ownership_Fin") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            CausesValidation="false">
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvFinDetails" runat="server" Visible="false" AutoGenerateColumns="false"
                                            ShowFooter="true" OnRowCommand="gvFinDetails_RowCommand" OnRowDeleting="gvFinDetails_RowDeleting"
                                            OnRowCancelingEdit="gvFinDetails_RowCancelingEdit" OnRowEditing="gvFinDetails_RowEditing"
                                            OnRowUpdating="gvFinDetails_RowUpdating" OnRowDataBound="gvFinDetails_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Collateral Type ID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl No" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Container.DataItemIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollDetailID" runat="server" Visible="false" Text='<%# Bind("Collateral_Detail_ID") %>'></asp:Label>
                                                        <asp:Label ID="lblCollSecurities" runat="server" Text='<%# Bind("Collateral_Securities") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlCollSecurities" runat="server" ToolTip="Collateral Securities">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCollSecurities" runat="server" ControlToValidate="ddlCollSecurities"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                            InitialValue="0" ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Insurance Issued By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblInsuranceIssuedBy" runat="server" Text='<%# Bind("Insurance_Issued_By") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtInsuranceIssuedBy" MaxLength="20" ToolTip="Insurance Issued By"
                                                            onkeypress="trimStartingSpace(this)" runat="server" Text='<%# Bind("Insurance_Issued_By") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvInsuranceIssuedBy" runat="server" ControlToValidate="txtInsuranceIssuedBy"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Insurance Issued By"
                                                            ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FtexInsuranceIssuedBy" runat="server" TargetControlID="txtInsuranceIssuedBy"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="./-()">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyNo" runat="server" Text='<%# Bind("Policy_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtPolicyNo" runat="server" MaxLength="20" ToolTip="Policy No" Text='<%# Bind("Policy_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexPolicyNo" runat="server" TargetControlID="txtPolicyNo"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPolicyNo" runat="server" ControlToValidate="txtPolicyNo"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Policy No" ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyValue" runat="server" Text='<%# Bind("Policy_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtPolicyValue" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            ToolTip="Policy Value" runat="server" Text='<%# Bind("Policy_Value") %>' MaxLength="12"></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexPolicyValue" runat="server" TargetControlID="txtPolicyValue"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvPolicyValue" runat="server" ControlToValidate="txtPolicyValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Policy Value" ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Current Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrentValue" runat="server" Text='<%# Bind("Current_Value") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCurrentValue" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                            ToolTip="Current Value" runat="server" Text='<%# Bind("Current_Value") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvCurrentValue" runat="server" ControlToValidate="txtCurrentValue"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Current Value" ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:FilteredTextBoxExtender ID="FtexCurrentValue" runat="server" TargetControlID="txtCurrentValue"
                                                            FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Bind("Maturity_Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtMaturityDate" ContentEditable="false" ToolTip="Maturity Date"
                                                            runat="server" Text='<%# Bind("Maturity_Date") %>'></asp:TextBox>
                                                        <asp:Image ID="imgMaturityDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                            ToolTip="Select Maturity Date" Visible="false" />
                                                        <cc1:CalendarExtender ID="CEMaturityDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="Image1"
                                                            TargetControlID="txtMaturityDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="rfvMaturityDate" runat="server" ControlToValidate="txtMaturityDate"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Maturity Date" ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Ref">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralRef" runat="server" Text='<%# Bind("Collateral_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCollateralRef" ReadOnly="true" ToolTip="Collateral Ref" runat="server"
                                                            Text='<%# Bind("Collateral_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Scan Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblScanRef" runat="server" Text='<%# Bind("Scan_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtScanRef" runat="server" ToolTip="Scan Ref No" MaxLength="10"
                                                            Text='<%# Bind("Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexScanRef" runat="server" TargetControlID="txtScanRef"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%-- <asp:RequiredFieldValidator ID="rfvScanRef" runat="server" ControlToValidate="txtScanRef"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Scan Ref No" ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtScanRef" runat="server" ToolTip="Scan Ref No" MaxLength="10"
                                                            Text='<%# Bind("Scan_Ref_No") %>'></asp:TextBox>
                                                        <cc1:FilteredTextBoxExtender ID="FtexScanRef" runat="server" TargetControlID="txtScanRef"
                                                            FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                            ValidChars="" />
                                                        <%-- <asp:RequiredFieldValidator ID="rfvScanRef" runat="server" ControlToValidate="txtScanRef"
                                                            Display="None" SetFocusOnError="true" ErrorMessage="Enter Scan Ref No" ValidationGroup="AddFINANCIALLiqSec">
                                                        </asp:RequiredFieldValidator>--%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Ref No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemRefNo" runat="server" Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtItemRefNo" ReadOnly="true" ToolTip="Item Ref No" runat="server"
                                                            Text='<%# Bind("Collateral_Item_Ref_No") %>'></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField HeaderText="Exclude" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRelease" Checked='<%# GVBoolFormat(Eval("Is_Release").ToString()) %>'
                                                            runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                            ToolTip="Update" CausesValidation="true" ValidationGroup="AddFINANCIALLiqSec"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                            ToolTip="Cancel" CausesValidation="false"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" ToolTip="Edit"
                                                            CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add" CommandName="AddNew" CausesValidation="true"
                                                            ToolTip="Add" ValidationGroup="AddFINANCIALLiqSec"></asp:LinkButton>
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" CommandName="Delete"
                                                            ToolTip="Delete" CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <asp:ValidationSummary ID="vgFinancialLIQSec" runat="server" ValidationGroup="AddFINANCIALLiqSec"
                                            CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                                            ShowSummary="true" />
                                        <%--</table>--%></asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                        <%-- For Customer Control --%>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="vgSubmit" runat="server" ValidationGroup="Submit" CssClass="styleMandatoryLabel"
                HeaderText="Correct the following validation(s):" ShowSummary="true" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Go"
                CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                ShowSummary="true" />
            <asp:CustomValidator ID="cvCollateralCapture" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true" Width="98%" />
            <%--TESTING FOR MODEL POP UP--%>
            <table>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="Button2" CssClass="styleSubmitButton" Text="Ok" Style="display: none"
                            OnClick="Button2_Click" />
                        <asp:Button runat="server" ID="Button1" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />
                        <cc1:ModalPopupExtender ID="MPE" runat="server" TargetControlID="Button1" PopupControlID="pnlDeletePopUp"
                            BackgroundCssClass="styleModalBackground" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="pnlDeletePopUp" GroupingText="" Width="600px" runat="server" Style="display: none"
                            BackColor="White" BorderStyle="Solid">
                            <asp:UpdatePanel ID="UpDeletePopUp" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td colspan="2" class="stylePageHeading">
                                                <asp:Label runat="server" Text="Confirmation Box" ID="lblFollowUp" CssClass="styleDisplayLabel"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="5px" colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%">
                                                <asp:Label ID="lblExcludeDate" runat="server" Text="Exclude Date"></asp:Label>
                                                <%-- <asp:HiddenField runat="server" ID="hdnView" />--%>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:TextBox ID="txtExcludeDate" runat="server" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPassword" Width="300px" MaxLength="10" onkeyup="maxlengthfortxt(300);"
                                                    TextMode="Password" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="5px" colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="center">
                                                <asp:Button runat="server" ID="btnOK" ValidationGroup="vgOK" CssClass="styleSubmitButton"
                                                    Text="Ok" OnClick="btnOK_Click" />
                                                &nbsp;<asp:Button runat="server" ID="btnCan" CssClass="styleSubmitButton" Text="Cancel"
                                                    OnClick="btnCan_Click" />
                                            </td>
                                        </tr>
                                        <tr class="styleButtonArea">
                                            <td colspan="2">
                                                <asp:ValidationSummary runat="server" ID="vsOK" HeaderText="Correct the following validation(s):"
                                                    CssClass="styleMandatoryLabel" Width="400px" ShowMessageBox="false" ShowSummary="true"
                                                    ValidationGroup="vgOK" />
                                                <asp:CustomValidator ID="cvOK" runat="server" CssClass="styleMandatoryLabel" Enabled="true"
                                                    Width="98%"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnOK" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<ajax:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" EnableHistory="False" EnableSecureHistoryState="True" />--%>
</asp:Content>
