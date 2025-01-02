<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRPTCIBILReportGeneration.aspx.cs" Inherits="Reports_S3GRPTCIBILReportGeneration"
    Title="CIBIL Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td colspan="4" class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" EnableViewState="false" CssClass="styleInfoLabel"
                                Text="CIBIL Data Generation Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlCIBIL" runat="server" GroupingText="Header Details" CssClass="stylePanel"
                                Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label ID="lblCutoffDate" runat="server" Text="Cut off Month/Year" CssClass="styleReqFieldLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtCutoffDate" runat="server" OnTextChanged="txtCutoffDate_TextChanged"
                                                AutoPostBack="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calendar1" runat="server" PopupButtonID="imgStartMonth"
                                                BehaviorID="calendar1" Format="MM/yyyy" TargetControlID="txtCutoffDate" OnClientShown="onCalendarShown">
                                            </cc1:CalendarExtender>
                                            <asp:Image ID="imgStartMonth" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <asp:RequiredFieldValidator ID="rfvStartMonth" runat="server" ErrorMessage="Select the Cut off Month/Year"
                                                Display="None" SetFocusOnError="True" ControlToValidate="txtCutoffDate" ValidationGroup="btnGenerate"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtPresentDate" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label ID="lblPath" runat="server" Text="Document Path" CssClass="styleDisplayLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtPath" runat="server" ReadOnly="true" Width="260px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%">
                                                <tr>
                                                    <td class="styleFieldLabel" width="32%">
                                                        <asp:Label ID="lblShedule" runat="server" Text="Shedule" CssClass="styleReqFieldLabel"></asp:Label>
                                                        <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleDisplayLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label ID="lblSheduleNow" runat="server" Text="Schedule Now" CssClass="styleDisplayLabel"></asp:Label>
                                                        <asp:RadioButton ID="rdoSheduleNow" AutoPostBack="true" runat="server" OnCheckedChanged="rdoSheduleNow_CheckedChanged" />
                                                        <asp:Label ID="lblSheduleLater" runat="server" Text="Schedule Later" CssClass="styleDisplayLabel"></asp:Label>
                                                        <asp:RadioButton ID="rdoSheduleLater" AutoPostBack="true" runat="server" OnCheckedChanged="rdoSheduleLater_CheckedChanged" />
                                                        <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label ID="lblScheduleDate" runat="server" Text="Schedule Date" CssClass="styleReqFieldLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtSheduledDate" runat="server" OnTextChanged="txtSheduledDate_TextChanged"
                                                AutoPostBack="true"></asp:TextBox>
                                            <cc1:CalendarExtender runat="server" Format="MM/dd/yyyy" TargetControlID="txtSheduledDate"
                                                PopupButtonID="Image1" BehaviorID="calender2" ID="CalendarExtender2" Enabled="True">
                                            </cc1:CalendarExtender>
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select the Shedule Date"
                                                Display="None" SetFocusOnError="True" ControlToValidate="txtSheduledDate" ValidationGroup="btnGenerate"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="styleFieldLabel">
                                            <asp:Label ID="lblTime" runat="server" Text="Schedule Time" CssClass="styleDisplayLabel"></asp:Label>
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
                                                        <asp:Label ID="Label2" runat="server" Text="(HH:MM AM)"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:GridView ID="grvInvalid" runat="server" AutoGenerateColumns="False" Width="90%">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Prime Account Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" Text='<%#Eval("PANum")%>' runat="server" ToolTip="Date"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sub Account Number">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" Text='<%#Eval("SANum")%>' runat="server" ToolTip="Date"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reason">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDate" Text='<%#Eval("Reason")%>' runat="server" ToolTip="Date"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                    </tr>
                    <tr>
                        <td id="Td1" runat="server" align="center">
                            <asp:Button ID="btnGenerate" Text="Initiate" runat="server" ValidationGroup="btnGenerate"
                                CssClass="styleSubmitButton" OnClick="btnGenerate_Click" CausesValidation="true"
                                OnClientClick="return fnCheckPageValidators();" />
                            <asp:Button ID="btnGenerateFile" Text="Generate" runat="server" ValidationGroup="btnGenerate"
                                CssClass="styleSubmitButton" CausesValidation="true"  Enabled="false" 
                                onclick="btnGenerateFile_Click"/>
                            <asp:Button runat="server" ID="btnClear" Text="Clear" CssClass="styleSubmitButton"
                                CausesValidation="False" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                                CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td id="tdd" runat="server" align="center">
                            <asp:Button ID="lnkExcel" runat="server" Text="Download Error Log File" CausesValidation="False"
                                OnClick="lnkExcel_Click" CssClass="styleSubmitLongButton" />
                            <asp:Button ID="lnkText" runat="server" Text="Download CIBIL File" CausesValidation="False"
                                OnClick="lnkText_Click" CssClass="styleSubmitLongButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="vsCibil" runat="server" CssClass="styleMandatoryLabel"
                                ValidationGroup="btnGenerate" HeaderText="Correct the following validation(s):" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CustomValidator ID="cvCIBIL" runat="server" CssClass="styleMandatoryLabel" Enabled="true"
                                Width="98%" ValidationGroup="btnGenerate" Display="None" />
                            <span id="lblErrorMessage" runat="server" style="font-size: medium"></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript"> 
 
        var cal1; 
        var cal2; 
         var mode = ("<%= Request.QueryString["qsMode"] %>");
//       if (mode == "C")
//       {
         function pageLoad() { 
         cal1 = $find("calendar1"); 
         if(cal1 != null)
         modifyCalDelegates(cal1); 
            
//        } 
        }
 
        function modifyCalDelegates(cal) { 
//         if (mode == "C")
//         {
            //we need to modify the original delegate of the month cell. 
            cal._cell$delegates = { 
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover), 
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout), 
 
                click: Function.createDelegate(cal, function(e) { 
                    /// <summary>  
                    /// Handles the click event of a cell 
                    /// </summary> 
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param> 
 
                    e.stopPropagation(); 
                    e.preventDefault(); 
 
                    if (!cal._enabled) return; 
 
                    var target = e.target; 
                    var visibleDate = cal._getEffectiveVisibleDate(); 
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover"); 
                    switch (target.mode) { 
                        case "prev": 
                        case "next": 
                            cal._switchMonth(target.date); 
                            break; 
                        case "title": 
                            switch (cal._mode) { 
                                case "days": cal._switchMode("months"); break; 
                                case "months": cal._switchMode("years"); break; 
                            } 
                            break; 
                        case "month": 
                            //if the mode is month, then stop switching to day mode. 
                            if (target.month == visibleDate.getMonth()) { 
                                //this._switchMode("days"); 
                            } else { 
                                cal._visibleDate = target.date; 
                                //this._switchMode("days"); 
                            } 
                            cal.set_selectedDate(target.date); 
                            cal._switchMonth(target.date); 
                            cal._blur.post(true); 
                            cal.raiseDateSelectionChanged(); 
                            break; 
                        case "year": 
                            if (target.date.getFullYear() == visibleDate.getFullYear()) { 
                                cal._switchMode("months"); 
                            } else { 
                                cal._visibleDate = target.date; 
                                cal._switchMode("months"); 
                            } 
                            break; 
 
                        //                case "day":                             
                        //                    this.set_selectedDate(target.date);                             
                        //                    this._switchMonth(target.date);                             
                        //                    this._blur.post(true);                             
                        //                    this.raiseDateSelectionChanged();                             
                        //                    break;                             
                        case "today": 
                            cal.set_selectedDate(target.date); 
                            cal._switchMonth(target.date); 
                            cal._blur.post(true); 
                            cal.raiseDateSelectionChanged(); 
                            break; 
                    } 
 
                }) 
            } 
 
//        } 
       }
 
        function onCalendarShown(sender, args) { 
            //set the default mode to month 
//           if (mode == "C")
//           {
             sender._switchMode("months", true); 
             changeCellHandlers(cal1); 
//            }
        } 
 
 
        function changeCellHandlers(cal) { 
//          if (mode == "C")
//           {
            if (cal._monthsBody) { 
 
                //remove the old handler of each month body. 
                for (var i = 0; i < cal._monthsBody.rows.length; i++) { 
                    var row = cal._monthsBody.rows[i]; 
                    for (var j = 0; j < row.cells.length; j++) { 
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates); 
                    } 
                } 
                //add the new handler of each month body. 
                for (var i = 0; i < cal._monthsBody.rows.length; i++) { 
                    var row = cal._monthsBody.rows[i]; 
                    for (var j = 0; j < row.cells.length; j++) { 
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates); 
                    } 
                } 
 
            } 
//           } 
        } 
 
        function onCalendarHidden(sender, args) { 
//        if (mode == "C")
//          {
            if (sender.get_selectedDate()) { 
                if (cal1.get_selectedDate() && cal2.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) { 
                    alert('Start Month/Year cannot be Greater than the End Month/Year, please reselect!'); 
                    sender.show(); 
                    return; 
                } 
                //get the final date 
                var finalDate = new Date(sender.get_selectedDate()); 
                var selectedMonth = finalDate.getMonth(); 
                finalDate.setDate(1); 
                if (sender == cal2) { 
                    // set the calender2's default date as the last day 
                    finalDate.setMonth(selectedMonth + 1); 
                    finalDate = new Date(finalDate - 1); 
                } 
                //set the date to the TextBox 
                sender.get_element().value = finalDate.format(sender._format); 
            } 
        } 
//       } 
        
        function Resize()
     {
//      if (mode == "C")
//       {
       if(document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
       {
         if(document.getElementById('divMenu').style.display=='none')
            {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
         else
           {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - 270;
           }
        } 
//        } 
      }
      
      
      function funSelect(txtBox)
        {
//         if (mode == "C")
//          {
            txtBox.select();
//          }
        }
        
        function funTrimLimit(txtBox,type)
        {
//           if (mode == "C")
//           {
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
//          }
        }
        
        function funIncreDecreValue(txtBox,type)
        {
//           if (mode == "C")
//           {
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
//           }
        }
        
        function fnGetIndex(txtBox)
        {
//         if (mode == "C")
//          {
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
//          }
        }
    </script>

</asp:Content>
