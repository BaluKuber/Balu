<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLOANADAssetVeification_Add.aspx.cs"
    Inherits="LoanAdmin_S3GLOANADAssetVeification_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="left" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <table class="stylePageHeading" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" Text="Asset Verification" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel Visible="true" ID="Panel4" runat="server" GroupingText="Details" CssClass="stylePanel">
                <table align="center" cellpadding="0" cellspacing="0" border="1">
                </table>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td width="50%" valign="top">
                        <asp:Panel ID="Panel5" Width="99%" runat="server" GroupingText="Details" CssClass="stylePanel">
                            <table cellpadding="0" cellspacing="3" border="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustddlCode" runat="server" Text="Customer Code" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <%--<asp:DropDownList ID="ddlCustCode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCustCode_SelectedIndexChanged"
                                Width="165px">
                            </asp:DropDownList>--%>
                                        <asp:TextBox ID="ddlCustCode" ToolTip="Customer Code" runat="server" Style="display: none;"
                                            MaxLength="50"></asp:TextBox>
                                        <uc2:LOV ID="ucCustomerCodeLov" onblur="return fnLoadCustomer();" runat="server"
                                            strLOV_Code="CMD" />
                                        <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" OnClick="btnLoadCustomer_Click"
                                            Style="display: none" /><input id="hdnCustID" type="hidden" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLOB" runat="server" Width="165px" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                            ToolTip="Line of business">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblBranch" Text="Branch" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlBranch" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                                            Width="165px" ToolTip="Branch">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblMLA" runat="server" Text="Prime Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlMLA" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMLA_SelectedIndexChanged"
                                            Width="165px" ToolTip="Prime Account Number ">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblSLA" runat="server" Text="Sub Account Number" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlSLA" runat="server" Width="165px" AutoPostBack="true" OnSelectedIndexChanged="ddlSLA_SelectedIndexChanged"
                                            ToolTip="Sub Account Number">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel1" runat="server" Width="99%" GroupingText="Asset Information"
                            CssClass="stylePanel">
                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblAVNo" Text="Asset Verification Number" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtAVNo" runat="server" ReadOnly="True" Width="75%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblAssetCode" runat="server" Text="Asset Code" CssClass="styleDisplayLabel"></asp:Label>
                                        <span class="styleMandatory">*</span>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlAssetCode" runat="server">
                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblAVDate" Text="Asset Verification Date" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtAVDate" runat="server" Width="100px" ToolTip="Asset Verification Date"
                                            ContentEditable="False"></asp:TextBox>
                                        <%-- <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtAVDate"
                                OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateofJoining"
                                ID="CalendarExtender2" Enabled="True">
                            </cc1:CalendarExtender>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td width="50%">
                        <asp:Panel ID="Panel2" runat="server" ToolTip="Customer Information" GroupingText="Customer Information"
                            CssClass="stylePanel">
                            <br />
                            <uc1:S3GCustomerAddress ID="ucCustDetails" runat="server" FirstColumnStyle="styleFieldLabel"
                                FirstColumnWidth="20%" SecondColumnWidth="30%" ActiveViewIndex="0" ThirdColumnWidth="20%"
                                FourthColumnWidth="30%" ShowCustomerCode="false"/>
                            <br />
                            <br />
                            <br />
                        </asp:Panel>
                    </td>
                </tr>
                <table runat="server" width="100%">
                </table>
                <asp:Panel Width="98%" ID="Panel3" runat="server" GroupingText="Verification Information"
                    ToolTip="Verification Information" CssClass="stylePanel">
                    <table width="100%">
                        <tr>
                            <td class="styleFieldLabel" width="23%">
                                <asp:Label ID="lblInspBy" runat="server" Text="Inspection By" CssClass="styleDisplayLabel"></asp:Label>
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign" width="27%">
                                <asp:DropDownList ID="ddlInspBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlInspBy_SelectedIndexChanged"
                                    Width="165px" ToolTip="Inspection By">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label ID="lblInspCode" runat="server" Text="Inspection Code" CssClass="styleDisplayLabel"></asp:Label>
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlInspCode" runat="server" ToolTip="Inspection Code">
                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblInspDate" runat="server" Text="Inspection Date" CssClass="styleDisplayLabel"></asp:Label>
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtInspDate" runat="server" ContentEditable="False" Width="50%"
                                    ToolTip="Inspection date"></asp:TextBox>
                                <asp:Image ID="imgDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtInspDate"
                                    OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDate"
                                    ID="CalendarExtender1" Enabled="True">
                                </cc1:CalendarExtender>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblInspTime" runat="server" Text="Inspection Time" CssClass="styleDisplayLabel"></asp:Label>
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <table cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="styleTableData" width="40px">
                                            <asp:TextBox ID="txtHour" runat="server" Width="13px" MaxLength="2" Text="12" Style="border: none;
                                                height: 17px; text-align: right;" onclick="funSelect(this);" onblur="funTrimLimit(this,'1');"
                                                onkeydown="return funIncreDecreValue(this,'1');" ToolTip="Inspection Time">
                                            </asp:TextBox>
                                            <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtHour" FilterType="Numbers"
                                                TargetControlID="txtHour" ValidChars="">
                                            </cc1:FilteredTextBoxExtender>
                                            <asp:Label ID="lblColon" runat="server" Width="3px" Text=":" ToolTip="Inspection Time"
                                                Style="vertical-align: text-top">
                                            </asp:Label>
                                            <asp:TextBox ID="txtMins" runat="server" Text="00" Width="13px" MaxLength="2" Style="border: none;
                                                height: 17px; text-align: right" onclick="funSelect(this);" onblur="funTrimLimit(this,'2');"
                                                onkeydown="funIncreDecreValue(this,'2');" ToolTip="Inspection Time">
                                            </asp:TextBox>
                                            <cc1:FilteredTextBoxExtender runat="server" ID="fltrtxtMins" FilterType="Numbers"
                                                TargetControlID="txtMins" ValidChars="">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAMPM" runat="server" Style="border-color: White" ToolTip="Inspection Time">
                                                <asp:ListItem Text="AM" Value="AM" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label1" runat="server" Text="(HH:MM AM)"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <asp:TextBox ID="txtInspTime" Visible="false" runat="server" Width="80px" MaxLength="8"
                                    ToolTip="Inspection Time"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender runat="server" ID="ftTime" FilterType="Numbers,Custom"
                                    TargetControlID="txtInspTime" ValidChars=" :APMapm">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblVerification" runat="server" Text="Verification Charges" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtVerification" runat="server" Width="50%" MaxLength="10" align="Right"
                                    ToolTip="Verification Charges"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender runat="server" ID="ftxtVerification" FilterType="Numbers"
                                    TargetControlID="txtVerification">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblMemo" runat="server" Text="Memo Charges" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtMemo" runat="server" MaxLength="10" Width="50%" align="Right"
                                    ToolTip="Memo Charges"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender runat="server" ID="ftxtMemo" FilterType="Numbers" TargetControlID="txtMemo">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblVerified" Text="Physically Verified" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:CheckBox ID="chkVerified" runat="server" />
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblAssetstatus" runat="server" Text="Asset Status" CssClass="styleDisplayLabel"></asp:Label>
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlAssetStatus" runat="server" Width="165px" ToolTip="Asset Status">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblLocation" runat="server" Text="Location" CssClass="styleDisplayLabel"></asp:Label>
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign" colspan="3">
                                <asp:TextBox ID="txtLocation" runat="server" Style="width: 93%" TextMode="MultiLine"
                                    onkeyup="maxlengthfortxt(150);" ToolTip="Location"></asp:TextBox>
                               <%-- <cc1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender2" FilterType="LowercaseLetters,UppercaseLetters,Custom"
                                    TargetControlID="txtLocation" ValidChars=" ">
                                </cc1:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblRemarks" Text="Remarks" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                <%--<span class="styleMandatory">*</span>--%>
                            </td>
                            <td class="styleFieldAlign" colspan="3">
                                <asp:TextBox ID="txtRemarks" onkeyup="maxlengthfortxt(150);" Style="width: 93%" ToolTip="Remarks"
                                    runat="server" TextMode="MultiLine"></asp:TextBox>
                                <%--<cc1:FilteredTextBoxExtender runat="server" ID="FilteredTextBoxExtender1" FilterType="LowercaseLetters,UppercaseLetters,Custom"
                                    TargetControlID="txtRemarks" ValidChars=" ">
                                </cc1:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table width="100%" runat="server">
                    <tr>
                        <td>
                        </td>
                        <td align="center">
                            &nbsp;&nbsp;<asp:Button ID="btnSave" Text="Save" runat="server" ValidationGroup="btnSave"
                                CssClass="styleSubmitButton" OnClick="btnSave_Click" CausesValidation="true"
                                OnClientClick="return fnCheckPageValidators();" />
                            &nbsp;<asp:Button runat="server" ID="btnClear" Text="Clear" CssClass="styleSubmitButton"
                                CausesValidation="False" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                            &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ControlToValidate="ddlLOB"
                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvddlCust" runat="server" ControlToValidate="ddlCustCode"
                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvddlBranch" runat="server" ControlToValidate="ddlBranch"
                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvddlMLA" InitialValue="0" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="ddlMLA" SetFocusOnError="True" Display="None"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <%--<asp:RequiredFieldValidator ID="rfvAsstCode" InitialValue="0" CssClass="styleMandatoryLabel"
                            runat="server" ControlToValidate="ddlAssetCode" SetFocusOnError="True" ErrorMessage="Select an Asset Status"
                            Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>--%>
                            <asp:RequiredFieldValidator ID="rfvAssetCode" InitialValue="0" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="ddlAssetCode" SetFocusOnError="True" Display="None"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvddlInspBy" runat="server" ControlToValidate="ddlInspBy"
                                CssClass="styleMandatoryLabel" Display="None" InitialValue="0" SetFocusOnError="True"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvInspCode" InitialValue="0" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="ddlInspCode" SetFocusOnError="True" Display="None"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtInspDate"
                                ValidationGroup="btnSave" Display="None" CssClass="styleSummaryStyle"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvInspecTime" runat="server" ControlToValidate="txtInspTime"
                                Enabled="false" Display="None" SetFocusOnError="True" CssClass="styleSummaryStyle"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rfvInsTime" InitialValue="0" runat="server" ControlToValidate="txtInspTime"
                                SetFocusOnError="True" ValidationGroup="btnSave" Display="None" ValidationExpression="(^([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)|(^([0-9]|[1][0-9]|[2][0-3])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvddlAssetStatus" InitialValue="0" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="ddlAssetStatus" SetFocusOnError="True" Display="None"
                                ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvLocation" CssClass="styleMandatoryLabel" runat="server"
                                ControlToValidate="txtLocation" SetFocusOnError="True" Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                            <%-- <asp:RequiredFieldValidator ID="rfvRemarks" CssClass="styleMandatoryLabel" runat="server"
                            ControlToValidate="txtRemarks" SetFocusOnError="True" ErrorMessage="Enter the Remarks"
                            Display="None" ValidationGroup="btnSave"></asp:RequiredFieldValidator>--%>
                            <asp:CustomValidator ValidationGroup="btnSave" ID="custCheck" runat="server" Display="None"
                                OnServerValidate="custCheck_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <div id="div1" style="height: 100%; width: 100%">
                                <asp:ValidationSummary ID="vsAssetVerification" runat="server" HeaderText="Correct the following validation(s):"
                                    CssClass="styleMandatoryLabel" Width="100%" ValidationGroup="btnSave" />
                            </div>
                        </td>
                    </tr>
                </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance(); 
        prm.add_initializeRequest(initializeRequest);
        prm.add_endRequest(endRequest);
        var postbackElement; 

        function initializeRequest(sender, args) { 
        document.body.style.cursor = "wait";
        if (prm.get_isInAsyncPostBack()) 
        {
        //debugger
        args.set_cancel(true); 
        }
        }
        function endRequest(sender, args) {document.body.style.cursor = "default"; 
        }

         function fnLoadCustomer() {
        
            document.getElementById('ctl00_ContentPlaceHolder1_btnLoadCustomer').click();
        }
        
        function funSelect(txtBox)
        {
          txtBox.select();
        }
        
        function funTrimLimit(txtBox,type)
        {
              var val = parseFloat(txtBox.value);
              
              if(type == '1')
              {
                if(val > 12) 
                  {
                    alert('Hours limit cannot exceed 12.');
                    txtBox.value = '12';
                  }
               if(isNaN(val) || val == 0)
                  {
                    txtBox.value = '12';
                  }
              }
              else if(type == '2')
              {
                if(val > 59)
                  {
                    alert('Minutes limit cannot exceed 59.');
                    txtBox.value = '59';
                  }
                if(isNaN(val))
                {
                  txtBox.value = '00';
                }
              }
              
             if(txtBox.value.toString().length == 1)
             {
               txtBox.value = '0' + txtBox.value.toString();
             }
        }
        
        function funIncreDecreValue(txtBox,type)
        {
          if(!document.getElementById("<%=btnSave.ClientID%>").disabled)
          {
              var sKeyCode = window.event.keyCode;
              var val = txtBox.value;
              var KeyValue = String.fromCharCode(parseFloat(sKeyCode))
              
              if(sKeyCode == 40)
               {
                 if(type == '1')
                  {
                    if(val == 1 || val > 12)
                    {
                      val = 12;
                    }
                    else
                    {
                      val = val - 1;
                    }
                    txtBox.value = val;
                    if(txtBox.value.toString().length == 1)
                    {
                      txtBox.value = '0' + txtBox.value.toString();
                    }
                    txtBox.select();
                  }
                  if(type == '2')
                  {
                    if(val == 0 || val > 59)
                    {
                      val = 59;
                    }
                    else
                    {
                      val = val - 1;
                    }
                    txtBox.value = val;
                    if(txtBox.value.toString().length == 1)
                    {
                      txtBox.value = '0' + txtBox.value.toString();
                    }
                    txtBox.select();
                  }
               }
              if(sKeyCode == 38)
               {
                 if(type == '1')
                  {
                    if(parseFloat(val) >= 12)
                    {
                      val = 1;
                    }
                    else
                    {
                      val = parseFloat(val) + 1;
                    }
                    txtBox.value = val.toString();
                    if(txtBox.value.toString().length == 1)
                    {
                      txtBox.value = '0' + txtBox.value.toString();
                    }
                    txtBox.select();
                  }
                 if(type == '2')
                  {
                    if(parseFloat(val) >= 59)
                    {
                      val = 0;
                    }
                    else
                    {
                      val = parseFloat(val) + 1;
                    }  
                    txtBox.value = val.toString();
                    if(txtBox.value.toString().length == 1)
                    {
                     txtBox.value = '0' + txtBox.value.toString();
                    }
                    txtBox.select();
                  } 
               }
//             if((sKeyCode >= 48 && sKeyCode <= 57) || (sKeyCode >= 96 && sKeyCode <= 105))
//             {
//               if(sKeyCode >= 96)
//               {
//                 sKeyCode = sKeyCode - 48;
//               }
//               KeyValue = String.fromCharCode(parseFloat(sKeyCode))
//               var CurIndex = fnGetIndex(txtBox);
//               if(txtBox.value.toString().length == 2)
//               {
//                 //txtBox.value = '';
//               }
//               if(type == '1' && parseFloat(txtBox.value) > 12)
//               { 
//                 return false;
//               }
//             }  
           return true;
           }
        }
        
        function fnGetIndex(txtBox)
        {
          if(txtBox.createTextRange())
          {
            var r = document.selection.createRange();
            r.moveEnd('character',txtBox.value.length);
            if(r.text == '')
            {
              return txtBox.value.length;
            }
            return txtBox.value.lastIndexOf(r.text);
          }
          else
          {
            return txtBox.selectionStart;
          }
        }
    </script>

</asp:Content>
