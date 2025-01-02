<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GSysAdminGlobalParameterSetup_Add.aspx.cs"
    Inherits="System_Admin_S3GSysAdminGlobalParamerterSetup_Add" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        var TotalChkBx;
        var Counter;
        
        function pageLoad() {
            $addHandler($get("hideModalPopupClientButton"), 'click', hideModalPopupViaClient);
           
        }

        window.onload = function() {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.grvMothEndParam.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
        }
        
        function hideModalPopupViaClient() {
            //ev.preventDefault();        
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        function HeaderClick(CheckBox, chilId) {
            //Get target base & child control.
            var TargetBaseControl =
       document.getElementById('<%= this.grvMothEndParam.ClientID %>');
            var TargetChildControl = chilId;

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
                if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;

            //Reset Counter
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }

        function ChildClick(CheckBox, HCheckBox) {
            //get target control.
            var HeaderCheckBox = document.getElementById(HCheckBox);
        
            //Modifiy Counter; 
            if (CheckBox.checked && Counter < TotalChkBx)
                Counter++;
            else if (Counter > 0)
                Counter--;

            //Change state of the header CheckBox.
            if (Counter < TotalChkBx)
                HeaderCheckBox.checked = false;
            else if (Counter == TotalChkBx)
                HeaderCheckBox.checked = true;
        }
        
        function fnCheckZero() 
        {
            var txtDaysPWD = (document.getElementById('<%=txtDaysPWD.ClientID %>'));
            var txtDisableAccess = (document.getElementById('<%=txtDisableAccess.ClientID %>'));
            var txtMiniPWDLen = (document.getElementById('<%=txtMiniPWDLen.ClientID %>'));
           
             //debugger;
             if (document.getElementById('<%=chkForcePWDChange.ClientID %>').checked == true)
             {
             
               if((txtDaysPWD.value != " ") && !(isNaN(txtDaysPWD.value)))
               {
                 var pwdDays
                 pwdDays = parseInt(txtDaysPWD.value, 10);
               }
               
               if((document.getElementById('<%=chkForcePWDChange.ClientID %>').checked == true) && pwdDays == "0")
               {
                    alert("On force password change - Days should not be zero");
               }
               else if ((document.getElementById('<%=chkForcePWDChange.ClientID %>').checked == true) && (isNaN(txtDaysPWD.value)))
               {
                  alert("On force password change - Days should not be empty");
               }
               else
               {
                  return;
               }
               
             }

            if ((txtDaysPWD.value != " ") && !(isNaN(txtDaysPWD.value)) || (txtDisableAccess.value != " ") && !(isNaN(txtDisableAccess.value))|| (txtMiniPWDLen.value != " ") && !(isNaN(txtMiniPWDLen.value)))
            {
                var pwdDays
                var disAccess
                var pwdlen
               
                pwdDays = parseInt(txtDaysPWD.value, 10);
                disAccess = parseInt(txtDisableAccess.value, 10);
                pwdlen = parseInt(txtMiniPWDLen.value, 10);
                 
//                if(document.getElementById('ctl00_ContentPlaceHolder1_tcGlobalParamSetup_tpPasswordPloicy_chkForcePWDChange').checked==true)
//                {
//                    if (pwdDays == "0") 
//                    {
//                        alert("Days cannot be Zero");
//                        txtDaysPWD.focus();
//                    }
//                }
//                
//                if(disAccess == 0)
//                {
//                    alert("Disable Access Count cannot be Zero");
//                    txtDisableAccess.focus();
//                }
//                else if(pwdlen == 0)
//                {
//                    alert("Password Length cannot be Zero");
//                    txtMiniPWDLen.focus();
//                }
//                else 
                
                
                if(pwdlen < "6")
                {
                    alert("Password Length should be minimum 6");
                    txtMiniPWDLen.focus();
                }
                else
                {
                    return;
                }
            }
            else 
            {
                return;
            }

        }

     
    </script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 10px">
                        <cc1:TabContainer ID="tcGlobalParamSetup" runat="server" CssClass="styleTabPanel"
                            Width="99%" ScrollBars="Auto" ActiveTabIndex="0">
                            <cc1:TabPanel runat="server" HeaderText="General Particulars" ID="tbGlobalParam1"
                                CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Global Parameters &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel ID="panDateFormat" GroupingText="Date Format" runat="server" CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDateFormat" runat="server" Text="Date Format" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="padding-left: 50px">
                                                    <asp:DropDownList ID="ddlDateFormat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDateFormat_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="dd/MM/yyyy">dd/MM/yyyy</asp:ListItem>
                                                        <asp:ListItem Value="dd-MM-yyyy">dd-MM-yyyy</asp:ListItem>
                                                        <asp:ListItem Value="MM/dd/yyyy">MM/dd/yyyy</asp:ListItem>
                                                        <asp:ListItem Value="MM-dd-yyyy">MM-dd-yyyy</asp:ListItem>
                                                        <asp:ListItem Value="dd-MMM-yyyy">dd-MMM-yyyy</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvDateFormat" ControlToValidate="ddlDateFormat"
                                                        runat="server" ErrorMessage="Select Date Format" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panCurrency" GroupingText="Default Currency Particulars"
                                        CssClass="stylePanel" Style="padding-top: 0px">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCurrency" runat="server" Text="Currency Name" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvCurrency" ControlToValidate="ddlCurrency" runat="server"
                                                        ErrorMessage="Select an Accounting currency" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMaxDigit" runat="server" Text="Maximum Digits" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMaxDigit" runat="server" MaxLength="2" onblur="ChkIsZero(this,'Maximum Digits')"
                                                        Style="text-align: right" Width="15%" onkeypress="fnAllowNumbersOnly('false',false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvMaxDigit" ControlToValidate="txtMaxDigit" runat="server"
                                                        Display="None" ErrorMessage="Enter Maximum Digits"></asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtMaxDigit"
                                                        FilterType="Numbers" InvalidChars="." FilterMode="InvalidChars" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMaxDecimal" runat="server" Text="Maximum Decimals" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMaxDecimal" runat="server" MaxLength="1" onblur="ChkIsZero(this)"
                                                        Style="text-align: right" Width="15%" onkeypress="fnAllowNumbersOnly('false',false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvMaxDecimal" ControlToValidate="txtMaxDecimal"
                                                        runat="server" ErrorMessage="Enter maximum Decimals" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtEffectiveDate" runat="server"></asp:TextBox>
                                                    <asp:Image ID="imgEffectiveDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEffectiveDate"
                                                        PopupButtonID="imgEffectiveDate" ID="cexEffectiveDate" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvEffectiveDate" ControlToValidate="txtEffectiveDate"
                                                        runat="server" ErrorMessage="Enter Effecitve Date" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblGSTEffectiveFrom" runat="server" Text="GST Effective From" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtGSTEffectivefrom" runat="server"></asp:TextBox>
                                                    <asp:Image ID="imgGSTEffectiveFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtGSTEffectivefrom"
                                                        PopupButtonID="imgGSTEffectiveFrom" ID="CEGSTEffectivefrom" Enabled="True">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="RFVGST" ControlToValidate="txtGSTEffectivefrom"
                                                        runat="server" Enabled="false" ErrorMessage="Enter GST Effective From" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panPIN" GroupingText="Postal Code Particulars" runat="server" CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFieldType" runat="server" Text="Field Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="padding-left: 60px">
                                                    <asp:DropDownList ID="ddlFieldType" runat="server">
                                                        <asp:ListItem Value="Alpha Numeric">Alpha Numeric</asp:ListItem>
                                                        <asp:ListItem Value="Numeric">Numeric</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvFieldType" ControlToValidate="ddlFieldType" runat="server"
                                                        ErrorMessage="Select PIN Code field Type" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDigits" runat="server" Text="Digits" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="padding-left: 60px">
                                                    <asp:TextBox ID="txtFieldDigits" runat="server" MaxLength="2" onblur="ChkIsZero(this,'Postal Code Digits')"
                                                        Style="text-align: right" Width="25%" onkeypress="fnAllowNumbersOnly('false',false,this)"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDigits" ControlToValidate="txtFieldDigits" runat="server"
                                                        ErrorMessage="Enter Digits in Postal Code" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ID="rgvPinCode" MaximumValue="10" MinimumValue="1" Display="None"
                                                        Type="Integer" ControlToValidate="txtFieldDigits" runat="server" ErrorMessage="Postal Code Digits value can be maximum 10 and Minimum 1"></asp:RangeValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender42" runat="server" TargetControlID="txtFieldDigits"
                                                        FilterType="Numbers" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="panGeneralParameters" GroupingText="General Parameters"
                                        CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:RadioButton ID="rbtnIntegratedSystem" runat="server" Enabled ="False" 
GroupName="SystemType"
                                                        Text="Integrated System" TextAlign="Left" Checked="True" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:RadioButton ID="rbtnStandAlone" runat="server" GroupName="SystemType" Enabled ="False" 
Text="Stand alone System"
                                                        TextAlign="Left" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblOPCBANk" runat="server" Text="Bank Details" />
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlOPCBank" runat="server"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:RadioButton ID="rbtnMLA" runat="server" GroupName="AccountType" Checked="True"
                                                        Text="Only MLA" TextAlign="Left" Visible="False" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:RadioButton ID="rbtnMLASLA" runat="server" GroupName="AccountType" Text="MLA and SLA"
                                                        TextAlign="Left" Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel runat="server" Visible="false" ID="Panel1" GroupingText="Account Credit" CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblAccountCredit" runat="server" Text="Account Credit" />
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:CheckBox ID="chkAccountCredit" runat="server" Checked="true" />
                                                </td>
                                                
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Month End Parameters" ID="TabPanel1" CssClass="tabpan"
                                BackColor="Red" Visible="false">
                                <HeaderTemplate>
                                    Prime/Sub Accounts Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <div id="divMLASLA" style="overflow: auto; height: 345px;" width="99%">
                                                    <asp:GridView ID="grvMLASLA" runat="server" AutoGenerateColumns="False" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="LOB Id" SortExpression="LOB_ID" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLOBId" runat="server" Text='<%# Bind("LOB_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Line of Business" SortExpression="LOB_Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLOB_NAME" runat="server" Text='<%# Bind("LOB_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="styleGridHeader" Width="65%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Only Prime Account">
                                                                <ItemTemplate>
                                                                    <%--      <asp:CheckBox ID="chkMLA" runat="server" Checked='<%# Bind("OnlyMLA") %>' />--%>
                                                                    <asp:RadioButton ID="chkMLA" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "OnlyMLA")))%>'
                                                                        GroupName='<%# Bind("LOB_ID") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="styleGridHeader" Width="15%" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Prime and Sub Account">
                                                                <ItemTemplate>
                                                                    <%-- <asp:CheckBox ID="chkMLASLA" runat="server" Checked='<%# Bind("MLAandSLA") %>'/>--%>
                                                                    <asp:RadioButton ID="chkMLASLA" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "MLAandSLA")))%>'
                                                                        GroupName='<%# Bind("LOB_ID") %>' />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="styleGridHeader" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Month End Parameters" ID="tpMonthEndParameters"
                                CssClass="tabpan" BackColor="Red">
                                <HeaderTemplate>
                                    Month End Parameters&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" GroupingText="Month end parameters" ID="panMonthEndPrev"
                                        CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel" style="padding-top: 10px">
                                                    <asp:Label ID="lblYearandMonth" runat="server" Text="Financial Year and Month"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" style="padding-top: 10px">
                                                    <asp:DropDownList ID="ddlFinacialYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinacialYear_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlFinancialMonth" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinancialMonth_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvFinancialYear" ControlToValidate="ddlFinacialYear"
                                                        runat="server" ErrorMessage="Select a Financial Year" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvFinancialMonth" ControlToValidate="ddlFinancialMonth"
                                                        runat="server" ErrorMessage="Select a Financial Month" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblYearLockDispaly" runat="server" Text="Year Lock"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblYearLock" runat="server" Font-Bold="True"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:CheckBox ID="chkYearLock" runat="server" AutoPostBack="True" OnCheckedChanged="chkYearLock_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMonthLockDisplay" runat="server" Text="Month Lock"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblMonthLock" runat="server" Font-Bold="True"></asp:Label>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:CheckBox ID="chkMonthLock" runat="server" AutoPostBack="True" OnCheckedChanged="chkMonthLock_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center" class="styleFieldAlign">
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="div1" class="container" style="margin-left: 2px; margin-right: 2px; overflow: auto;
                                            height: 200px" width="100%">
                                            <asp:GridView ID="grvMothEndParam" runat="server" AutoGenerateColumns="False" Width="98%"
                                                OnRowCreated="grvMothEndParam_RowCreated" OnRowCommand="grvMothEndParam_RowCommand">
                                                <%--OnRowDataBound="grvMothEndParam_RowDataBound"--%>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Branch Id" SortExpression="Branch_id" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBranchId" runat="server" Text='<%# Bind("Location_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Location" SortExpression="Branch">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Location_Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Region_id" SortExpression="Region_id" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRegionId" runat="server" Text='<%# Bind("Region_id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="Month Lock">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblHdrCB" Text="Month Lock" runat="server" /><br />
                                                            <asp:CheckBox ID="chkHdrMonthLock" runat="server" CssClass="styleGridHeader" onclick="javascript:HeaderClick(this,'chkMonth');"
                                                                AutoPostBack="True" OnCheckedChanged="chkHdrLockEvent_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%--'<%# Bind("Month_Lock") %>'--%>
                                                            <asp:CheckBox ID="chkMonth" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Month_Lock")))%>'
                                                                AutoPostBack="True" OnCheckedChanged="chkLockEvent_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Billing">
                                                        <ItemTemplate>
                                                            <%--'<%# Bind("Billing") %>'--%>
                                                            <asp:CheckBox ID="chkBilling" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Billing")))%>'
                                                                AutoPostBack="true" Enabled="true" OnCheckedChanged="chkLockEvent_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Income Recognition">
                                                        <%-- Interest Calculation  --%>
                                                        <ItemTemplate>
                                                            <%--'<%# Bind("Interest_Calculation") %>'--%>
                                                            <asp:CheckBox ID="chkInterest" runat="server" AutoPostBack="true" OnCheckedChanged="chkLockEvent_CheckedChanged"
                                                                Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Interest_Calculation")))%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" HeaderText="Deliquency">
                                                        <ItemTemplate>
                                                            <%--'<%# Bind("Deliquency") %>'--%>
                                                            <asp:CheckBox ID="chkDeliquency" runat="server" AutoPostBack="true" OnCheckedChanged="chkLockEvent_CheckedChanged"
                                                                Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Deliquency")))%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ODI" Visible="false">
                                                        <ItemTemplate>
                                                            <%--'<%# Bind("ODI") %>'--%>
                                                            <asp:CheckBox ID="chkOdi" runat="server" AutoPostBack="true" OnCheckedChanged="chkLockEvent_CheckedChanged"
                                                                Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "ODI")))%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false" HeaderText="Demand">
                                                        <ItemTemplate>
                                                            <%--'<%# Bind("Demand") %>'--%>
                                                            <asp:CheckBox ID="chkDemand" runat="server" AutoPostBack="true" OnCheckedChanged="chkLockEvent_CheckedChanged"
                                                                Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Demand")))%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField Visible="false" HeaderText="Depreciation">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkDepreciation" runat="server" AutoPostBack="true" OnCheckedChanged="chkLockEvent_CheckedChanged"
                                                                Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Depreciation")))%>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField HeaderText="DetailId" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMonthEndId" runat="server" Text='<%# Bind("Month_End_Params_Det_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkView" Width="100%" runat="server" Text="View" CommandName="view"
                                                                CommandArgument='<%# Bind("Location_ID") %>'></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="styleGridHeader" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No Records Found
                                                </EmptyDataTemplate>
                                                <EmptyDataRowStyle HorizontalAlign="Center" Font-Size="Medium" VerticalAlign="Middle"
                                                    Width="100%" CssClass="styleRecordCount" />
                                            </asp:GridView>
                                        </div>
                                        <br></br>
                                        <asp:Panel ID="PnlPassword" Style="display: none; vertical-align: middle" runat="server"
                                            BorderStyle="Solid" BackColor="White" Width="50%">
                                            <table width="100%">
                                                <tr width="100%">
                                                    <td width="100%">
                                                        <asp:GridView ID="GridViewLOB" runat="server" AutoGenerateColumns="False" Width="100%">
                                                            <%--OnRowDataBound="GridViewLOB_RowDataBound"--%>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Line of Business">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB_Name") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <ItemStyle Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Billing">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkBilling" runat="server" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Billing")))%>'
                                                                            Enabled="true" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Income Recognition">
                                                                    <%-- Interest Calculation--%>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkInterest" runat="server" Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Interest_Calculation")))%>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Deliquency">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkDeliquency" runat="server" Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Deliquency")))%>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="ODI">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkOdi" runat="server" Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "ODI")))%>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="styleGridHeader" Width="5%" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Demand">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkDemand" runat="server" Enabled="true" Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Demand")))%>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <cc1:ModalPopupExtender ID="ModalPopupExtenderPassword" runat="server" TargetControlID="btnModal"
                                                            PopupControlID="PnlPassword" BackgroundCssClass="styleModalBackground" DynamicServicePath=""
                                                            Enabled="True" BehaviorID="programmaticModalPopupBehavior">
                                                        </cc1:ModalPopupExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblmodalerror" runat="server" />
                                                        <br />
                                                        <a id="hideModalPopupClientButton" href="#" onclick="hideModalPopupViaClient();">Close</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Button ID="btnModal" Style="display: none" runat="server" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Password Policy" ID="tpPasswordPloicy" CssClass="tabpan"
                                BackColor="Red">
                                <HeaderTemplate>
                                    Password Policy&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" GroupingText="Password Rules" ID="Panel2" CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblForcePWDchange" runat="server" Text="Force Password Change" CssClass="styleDisplayLabel"></asp:Label>
                                                    <asp:CheckBox ID="chkForcePWDChange" runat="server" OnCheckedChanged="chkForcePWDChange_CheckedChanged"
                                                        Checked="true" />
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDays" runat="server" Text="Days" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtDaysPWD" runat="server" MaxLength="2" Width="30px" onblur="fnCheckZero()"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtDaysPWD"
                                                        FilterType="Numbers" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <%--<asp:RequiredFieldValidator ID="rfvDays" ControlToValidate="txtDaysPWD" runat="server"
                                                        Display="None" ErrorMessage="Enter Days"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" colspan="2">
                                                    <asp:Label ID="lblEnforcePWD" runat="server" Text="Enforce initial change password"
                                                        CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:CheckBox ID="chkInitialChangePWD" runat="server" OnCheckedChanged="chkInitialChangePWD_CheckedChanged"
                                                        Checked="true" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" colspan="2">
                                                    <asp:Label ID="lblDisableAccess" runat="server" Text="Disable access after wrong password"
                                                        CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtDisableAccess" runat="server" MaxLength="2" Width="30px" onblur="fnCheckZero()"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtDisableAccess"
                                                        FilterType="Numbers" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvDisableAccess" ControlToValidate="txtDisableAccess"
                                                        runat="server" Display="None" ErrorMessage="Enter Disable Access Count"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" colspan="2">
                                                    <asp:Label ID="lblMiniPWDLen" runat="server" Text="Minimum password length" CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtMiniPWDLen" runat="server" MaxLength="2" Width="30px" onblur="fnCheckZero()"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtMiniPWDLen"
                                                        FilterType="Numbers" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvMinPwdlen" ControlToValidate="txtMiniPWDLen" runat="server"
                                                        Display="None" ErrorMessage="Enter Minimum Password Length"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" colspan="2">
                                                    <asp:Label ID="lblpwdrecycleitr" runat="server" Text="Password Re-cycle Iteration"
                                                        CssClass="styleReqFieldLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:TextBox ID="txtpwdrecycleitr" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtpwdrecycleitr"
                                                        FilterType="Numbers" Enabled="True">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvPwdrecycle" ControlToValidate="txtpwdrecycleitr"
                                                        runat="server" Display="None" ErrorMessage="Enter Password Re-cycle Iteration"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel runat="server" GroupingText="Password Composition" ID="Panel3" CssClass="stylePanel">
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblUpperCase" runat="server" Text="Upper Case character" CssClass="styleDisplayLabel"></asp:Label>
                                                    <asp:CheckBox ID="chkUpperCaseChar" runat="server" OnCheckedChanged="chkForcePWDChange_CheckedChanged"
                                                        Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblNumbers" runat="server" Text="Number" CssClass="styleDisplayLabel"></asp:Label>
                                                    <asp:CheckBox ID="chkNumbers" runat="server" OnCheckedChanged="chkNumbers_CheckedChanged"
                                                        Checked="true" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblSpecialCharacters" runat="server" Text="Special characters" CssClass="styleDisplayLabel"></asp:Label>
                                                    <asp:CheckBox ID="chkSpecialCharacters" runat="server" OnCheckedChanged="chkSpecialCharacters_CheckedChanged"
                                                        Checked="true" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                        <input id="hdnGlobalParamId" type="hidden" runat="server" value="0" />
                        <input id="hdnMonth_End_Params_ID" type="hidden" runat="server" value="0" />
                        <input id="hdnMonth_End_Params_Det_ID" type="hidden" runat="server" value="0" />
                        <input id="hdnUserId" type="hidden" runat="server" value="0" />
                        <input id="hdnUserLevelId" type="hidden" runat="server" value="0" />
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium">
                        </span>
                        <%--<asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" 
                    ForeColor="Red"></asp:Label>--%>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="styleSubmitButton"
                            OnClientClick="return fnCheckPageValidators()" OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                            OnClientClick="return fnConfirmClear();" CausesValidation="false" OnClick="btnClear_Click" />
                        <%-- <input type="reset" value="Clear" class="styleSubmitButton" runat="server" id="btnClear"
                    onclick="return confirm('Do you want to clear?')" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsGlobalParamAdd" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                            ShowMessageBox="false" HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
