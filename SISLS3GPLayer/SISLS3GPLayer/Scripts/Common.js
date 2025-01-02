

/// Page Name       :   Common.js
/// Created By      :   Kali K
/// Created Date    :   22-APR-2010
/// Purpose         :   To include common Jscript functions

/// This function is used to identify mandatory controls which are not entered

function fnCheckPageValidators(strValGrp,blnConfirm) 
{
   
    if (Page_ClientValidate() == false)  
    {
        var i, val, strGroupName;
        var iResult="1";
        for (i = 0; i < Page_Validators.length; i++) 
        {
            val = Page_Validators[i];
          
            controlToValidate = val.getAttribute("controltovalidate");
            if(controlToValidate!=undefined)
            {
               if(document.getElementById(controlToValidate) !=null)
               {
                 document.getElementById(controlToValidate).border="1";
               }
            }
        }
        
        for (i = 0; i < Page_Validators.length; i++) 
     { //For loop1
            val = Page_Validators[i];
            var isValidAttribute = val.getAttribute("isvalid");
            var controlToValidate = val.getAttribute("controltovalidate");
            if(controlToValidate!=undefined)
            {
                if (strValGrp == undefined) 
                {
                    if(document.getElementById(controlToValidate) !=null)
                        {
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
                }
                
                else 
                {
                    strGroupName = val.getAttribute("validationgroup");
                    if(document.getElementById(controlToValidate) !=null)
                    {
                        if ((isValidAttribute == false) && (strValGrp==strGroupName))
                        {
                           
                            document.getElementById(controlToValidate).className = 'styleReqFieldFocus';
                            //This is to avoid not to validate control which is already in false state (valid attribute)
                            document.getElementById(controlToValidate).border = "0";
                            iResult = "0";
                        }
                        else if (document.getElementById(controlToValidate).border != "0") {
                            document.getElementById(controlToValidate).className = 'styleReqFieldDefalut';
                        }
                    }
                }
            
         }  //Undefined loop condition
         
       }//For loop1 End
       
         Page_BlockSubmit=false; ////Added by Kali on 12-Jun-2010 This function is used to solve issues
         // Need twice click of cancel and clear button after validation summary is visible
    } //

      if (Page_ClientValidate(strValGrp)) 
    {
    
        if (blnConfirm == undefined) 
        {
            if (confirm('Do you want to save?')) 
            {
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
        else
        {
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
     else
        {
        Page_BlockSubmit=false;
       return false;
       }

}

//Added by Kali K on 05-May-2010 
//This function is used to check all the check box in the grid

function fnDGSelectOrUnselectAll(grdid, obj, objlist) 
{
//this function decides whether to check or uncheck all
    if (obj.checked)
        fnDGSelectAll(grdid, objlist)
    else
        fnDGUnselectAll(grdid, objlist)
}
//---------- 

function fnDGSelectAll(grdid, objid) {
    //.this function is to check all the items
    var chkbox;
    var i = 2;
    //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid;
    var gridId = grdid;
    
    chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);

    while (chkbox != null) {
    if(chkbox.disabled == false)
        chkbox.checked = true;
        i = i + 1;
        if(i<=9) 
        {
            chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
        }
        else
            chkbox = document.getElementById(gridId + '_ctl' + i + '_' + objid);
    }
    

} //-------------- 

function fnDGUnselectAll(grdid, objid) {
    //.this function is to uncheckcheck all the items
    var chkbox;
    var chkStatus=false;
    var i = 2;
    //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid;
    var gridId = grdid;
    chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);

    while (chkbox != null) {
        // Obly if the Check box is enabled 
        if(chkbox.disabled == false)
            chkbox.checked = false;
        i = i + 1;
        if(i<=9) 
        {
            chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
        }
        else
            chkbox = document.getElementById(gridId + '_ctl' + i + '_' + objid);
    }
    
}
function fnDGUnselectAllExpectSelected(gridid,SelectedChkboxid) {
    //Get target base & child control.
    var txtClass, txtMake, txtType, txtModel;
    var TargetBaseControl =
       document.getElementById(gridid);
   
    var TargetControl = SelectedChkboxid; 

    //Get all the control of the type INPUT in the base control.
    var Inputs = TargetBaseControl.getElementsByTagName("input");
    
    //Checked/Unchecked all the checkBoxes in side the GridView.
    for (var n = 0; n < Inputs.length; ++n)
        if (Inputs[n].type == 'checkbox' && Inputs[n].uniqueID != TargetControl.uniqueID)
        Inputs[n].checked = false;

}
//Code end


function fnDGSelectAllCheckBox(grdid, objChkAll,objid) {

    //.this function is to check all the items
    var chkbox;
    var i = 2;
    //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid;
    var gridId = grdid;
    var chkStatus=true;
    chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);

    while (chkbox != null) 
    {
        if(chkbox.checked==false)
        {
            chkStatus=false;
        }
        i = i + 1;
        if(i<=9) 
        {
            chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
        }
        else
            chkbox = document.getElementById(gridId + '_ctl' + i + '_' + objid);
    }
    document.getElementById(gridId + '_ctl01' + '_' + objChkAll)=chkStatus;
    
} //--------------

//Code added by Kali on 09-Aug-2010 to unSelect "Select All" CheckBox if any row chckbox is unselected.

function fnGridUnSelect(grdid,chkAll,objid)
 {
   
    var chkbox;
    var chkStatus=true;
    var i = 2;
    //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid;
   
    chkbox = document.getElementById(grdid + '_ctl0' + i + '_' + objid);  
    while (chkbox != null)
     {
        if(!chkbox.checked)
         {
              chkStatus=false; 
              break;
         }
        i = i + 1;
        if(i<=9) 
        {
            chkbox = document.getElementById(grdid + '_ctl0' + i + '_' + objid);
        }
        else
            chkbox = document.getElementById(grdid + '_ctl' + i + '_' + objid);
     }

            //Change state of the header CheckBox.
            if (chkStatus)
                document.getElementById(grdid + '_ctl01_'+chkAll).checked = true;
                //HeaderCheckBox.checked = false;
            else 
                document.getElementById(grdid + '_ctl01_'+chkAll).checked = false;
                //HeaderCheckBox.checked = true;
 }

//Added by Kali K on 05-May-2010 
//This is to check special chracters

function fnCheckSpecialChars(isSpaceAllowed) {
    var sKeyCode = window.event.keyCode;
    if ((!isSpaceAllowed)&&(sKeyCode==32))
    {
       window.event.keyCode=0;
       return false;
   }
  
    if ((sKeyCode < 97 || sKeyCode > 122) && (sKeyCode < 65 || sKeyCode > 90) && (sKeyCode < 48 || sKeyCode > 57) && sKeyCode != 32 && sKeyCode != 95 ) 
    {
        window.event.keyCode=0;
        return false;
    }
}

//added by Narayanan -  Start

            //Purpose is Allow Numeric and decimal points only
            function fnAllowNumbersDesimal(isSpaceAllowed) {
                var sKeyCode = window.event.keyCode;
                
                if ((!isSpaceAllowed) && (sKeyCode == 32)) {
                    window.event.keyCode = 0;
                    return false;
                }

                if ((sKeyCode < 48 || sKeyCode > 57) && sKeyCode != 32 && sKeyCode != 95 && sKeyCode != 46) {
                    window.event.keyCode = 0;
                    return false;
                }
            }
        
        //Purpose is Allow Numeric only
//        function fnAllowNumbersOnly(isSpaceAllowed) {
//            var sKeyCode = window.event.keyCode;
//            if ((!isSpaceAllowed) && (sKeyCode == 32)) {
//                window.event.keyCode = 0;
//                return false;
//            }

//            if ((sKeyCode < 48 || sKeyCode > 57) && sKeyCode != 32 && sKeyCode != 95)
//             {
//                window.event.keyCode = 0;
//                return false;
//            }
//        }
        //Purpose is only allow Alphabets only
        function fnAllowAlphabetsOnly(isSpaceAllowed) {
            var sKeyCode = window.event.keyCode;
            if ((!isSpaceAllowed) && (sKeyCode == 32)) {
                window.event.keyCode = 0;
                return false;
            }

            if ((sKeyCode < 97 || sKeyCode > 122) && (sKeyCode < 65 || sKeyCode > 90) &&  sKeyCode != 32 && sKeyCode != 95) 
            {
                window.event.keyCode = 0;
                return false;
            }
        }
        //How to Call this function in markup e.g  onkeyup = "fnNotAllowPasteSpecialChar(this,'special')"
        //Purpose is not allow copy and paste in this particular control
        function fnNotAllowPasteSpecialChar(o, w) {
            var sKeyCode = window.event.keyCode;
            if (sKeyCode == 37 || sKeyCode > 39 || sKeyCode == 32) {
                window.event.keyCode = 0;
                return false;
            }
            else {
                var r = {
                    'special': /[\W]/g,
                    'quotes': /['\''&'\"']/g,
                    'notnumbers': /[^\d]/g
                }
                o.value = o.value.replace(r[w], '');
            }
        }
        //Purpsoe is Clear Clip Board data
        function fnClearClipBoard() {

            window.clipboardData.clearData();
        }

        var oldgridSelectedColor;

        //Purpose is set a colour on mouse over
        function setMouseOverColor(element) {
            oldgridSelectedColor = element.style.backgroundColor;
            element.style.backgroundColor = '#CEE3F6';
            element.style.cursor = 'hand';
            //        element.style.textDecoration = 'underline';
        }

        //Purpose is set a colour on mouse out
        function setMouseOutColor(element) {
            element.style.backgroundColor = oldgridSelectedColor;
            element.style.textDecoration = 'none';
        }
        
//narayaan - End



//Added by Kali K on 05-May-2010 

//This is used to validate any checkbox checked or not in the grid
function fnIsCheckboxChecked(grdid, objid,msg) {
    
    var chkbox;
    var bChecked = false;
    var i = 2;
    //var gridId = 'ctl00_ContentPlaceHolder1_' + grdid ;
    var gridId = grdid;
  
    chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
   
    while ((chkbox != null)) 
    {
        if (chkbox.checked) 
        {
            bChecked = true;
            break;
        }
        i = i + 1;
        if(i<=9) 
        {
            chkbox = document.getElementById(gridId + '_ctl0' + i + '_' + objid);
        }
        else
            chkbox = document.getElementById(gridId + '_ctl' + i + '_' + objid);
        
    }
    
    if (bChecked)
        return true;
    else {
        alert('Select atleast one ' + msg);
        return false;
    }
}


//Added by Kali K on 20-May-2010 to show alert message before clear action

function fnConfirmClear()
{

if(confirm('Do you want to clear?'))
            {
                return true;
            }
        else
            return false;
            
}


//Added By nataraj for Calendar Control Validations & Max Length Check

//call both calendar methods in  OnClientDateSelectionChanged of the calendar control extender
//Call this Function if your Calender Should not allow values greater than system date
function checkDate_NextSystemDate(sender, args) {
    
       var today = new Date();
    if (sender._selectedDate > today) {
       
        //alert('You cannot select a date greater than system date');
        alert('Selected date cannot be greater than system date');
        sender._textbox.set_Value(today.format(sender._format));

    }
    document.getElementById(sender._textbox._element.id).focus();
}
//Call this Function if your Calender Should not allow values less than system date
function checkDate_PrevSystemDate(sender, args) {
    
    if (sender._selectedDate < new Date()) {

        //alert('You cannot select a date less than or equal to system date');
        alert('Selected date cannot be less than or equal to system date');
        sender._textbox.set_Value('');
    }
    document.getElementById(sender._textbox._element.id).focus();

}
function checkDate_OnlyPrevSystemDate(sender, args) {
   var today = new Date();
  
   var today_new = today.format(sender._format);
    var selectedDate = sender._selectedDate;
     var selectedDate_new=sender._selectedDate.format(sender._format)
//    selectedDate = selectedDate.format(sender._format);
    if (selectedDate < today) {
        if(today_new !=selectedDate_new)
        {
        //alert('You cannot select a date less than system date');
        alert('Selected date cannot be less than system date');
        sender._textbox.set_Value(today.format(sender._format));
        }
    }
    document.getElementById(sender._textbox._element.id).focus();

}

function checkDate_OnlyProspective(sender, args) {
    var today = new Date();

    var today_new = today.format(sender._format);
    var selectedDate = sender._selectedDate;
    var selectedDate_new = sender._selectedDate.format(sender._format)
    //    selectedDate = selectedDate.format(sender._format);
    if (selectedDate < today) {
        if (today_new != selectedDate_new) {
            //alert('You cannot select a date less than system date');
            alert('Retrospective (backward date) revision is not applicable for the selected account');
            sender._textbox.set_Value(today.format(sender._format));
        }
    }
    document.getElementById(sender._textbox._element.id).focus();

}

//this method is used to Check the max lenth for text box especially Multiline text box provide max length for it
//Call the methosd in key press and onchange event of the text box
function maxlengthfortxt(maxlength) 
{
     var dispalyLength=0;
        if(maxlength == 310)
        {
            dispalyLength = 300;
        }
        else if(maxlength == 42)
        {
            dispalyLength = 40;
        }
        else if(maxlength == 66)
        {
            dispalyLength = 60;
        }
         else if(maxlength == 88)
        {
            dispalyLength = 80;
        }
        else if(maxlength == 64)
        {
            dispalyLength = 60;
        }
       else if(maxlength == 44)
        {
            dispalyLength = 40;
        }
        else
        {
            dispalyLength=maxlength;
        }
        
        //Code added by Kali on 10-Aug-2011 not to allow space in first letter
        
    if(document.getElementById(document.activeElement.id).value.length==1 && window.event.keyCode==32)
    {
     document.getElementById(document.activeElement.id).value="";
     return false;
    }
      // debugger; 
    if (document.getElementById(document.activeElement.id).value.length > maxlength) 
    {
        document.getElementById(document.activeElement.id).value = document.getElementById(document.activeElement.id).value.substr(0, maxlength);
        // if (window.event.keyCode != 8 && window.event.keyCode != 46 && window.event.keyCode != 37 && window.            event.keyCode != 38 && window.event.keyCode != 40 && window.event.keyCode != 39) {
        if(dispalyLength > 1)
        {
        alert('Maximum length cannot be greater than ' + dispalyLength + ' characters'); 
        }
        else if(dispalyLength = 1)
        {
         alert('Maximum length cannot be greater than ' + dispalyLength + ' character');
        }
        window.event.returnValue = false;
        //  }
    }

}

//Added by Kali on 31-MAR-2010 to show and hide menu box.
function showMenu(show) {
    if (show == 'T') {
        
        //Added by Kali K to solve error ( when menu is show scroll should appear in grid )
        //This is used to show scroll bar when div is used in GridView
        if(document.getElementById('divGrid1')!=null)
            {
                document.getElementById('divGrid1').style.width="800px";
                document.getElementById('divGrid1').style.overflow="scroll";
            }
        
        document.getElementById('divMenu').style.display = 'Block';
        document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

        document.getElementById('ctl00_imgShowMenu').style.display = 'none';
        document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

        //document.getElementById('tdMenu').className = 'styleMenuTd';

    }
    if (show == 'F') {
        
        //Added by Kali K to solve error ( when menu is hide scroll for is not hiding from grid view )
        //This is used to hide scroll bar when div is used in GridView
        if(document.getElementById('divGrid1')!=null)
            {
                document.getElementById('divGrid1').style.width="960px";
                document.getElementById('divGrid1').style.overflow="auto";
            }
            
        document.getElementById('divMenu').style.display = 'none';
        document.getElementById('ctl00_imgHideMenu').style.display = 'none';
        document.getElementById('ctl00_imgShowMenu').style.display = 'Block';

        //document.getElementById('tdMenu').className = '';
        //document.getElementById('divMenu').setAttribute('width', 0);
    }
}

function hideMenu(show) {
    if (show == 'T') {
        document.getElementById('divMenu').style.display = 'Block';

    }
}
function showtooltip(show) {
    if (show == 'S') {
        document.getElementById('div1').style.display = 'Block';
        document.getElementById('div1').style.position = 'absolute';
    }
    if (show == 'H') {
        document.getElementById('div2').style.display = 'Block';
        document.getElementById('div2').style.position = 'absolute';
    }
}
function hidetooltip(show) {
    if (show == 'S') {
        document.getElementById('div1').style.display = 'none';
    }
    if (show == 'H') {
        document.getElementById('div2').style.display = 'none';
    }
}

//Kali code end

// Check Number of Decimal(.) in the Field
function CheckDecimal(sender,args)
{
    var txt = (args.Value);
    var startindex= txt.indexOf(".")
    var lastindex=txt.lastIndexOf(".")
    if(startindex != lastindex)
    {          
        args.IsValid = false;
        return;
    }
         
    args.IsValid = true;
}

//Added by nataraj  on 23/06/2010
//for text wrap on key press call this method from cs file
//For. e.g.txtBankName.Attributes.Add("onkeypress", "wraptext(" + txtBankName.ClientID + ", 20);");
//wraplength is the length for which it needs to be wrapped
function wraptext(textbox, wraplength) {

    var textval = textbox.value;
    //Code added by Kali on 10-Aug-2011 not to allow space in first letter
    if(textval.length==0 && window.event.keyCode==32)
    {
     window.event.keyCode = 0;
     return false;
    }
     
    if (textval.length > 0) {
        if ((textval.length % wraplength) == 0) {
            textval = textval + "\n";
            textbox.value = textval;
        }
    }
}

function Confirmmsg(ConfirmMsg)
{
        if (confirm(ConfirmMsg)) 
                          
                return true;
        else
                return false;
}

//Addeb by Nataraj On 21-10-2010
///Added To check Only Numbers, to allow numbers without or with decimals,negative,space 

  function fnAllowNumbersOnly(isdecimalAllowed,isNegativeAllowed,txtbox) {
            
            var sKeyCode = window.event.keyCode;
            if ((sKeyCode < 48 || sKeyCode > 57) && (sKeyCode != 32 && sKeyCode != 95) || (sKeyCode == 46)) {
                
                if(isdecimalAllowed)
                {
                //alert(sKeyCode);
                    if(sKeyCode==46)
                    {
                          
                        if(txtbox.value.indexOf('.')>-1)
                        {
                            window.event.keyCode = 0;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                 }
                 if(isNegativeAllowed)
                 {
                 if(sKeyCode==45)
                    {
                          
                        if(txtbox.value.indexOf('-')>-1)
                        {
                            window.event.keyCode = 0;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                 }
                window.event.keyCode = 0;
                return false;
            }
            if(txtbox.value.length > txtbox.maxlength)
            {
                window.event.keyCode = 0;
                return false;
            }
        }
       
        ///Added To check Only Numbers, to allow numbers with decimals,negative 
        //Created by Tamilselvan.S on 14/02/2011 
         function fnAllowNumbersAndPlusMin(isdecimalAllowed,isNegativeAllowed,txtbox) {
            
            var sKeyCode = window.event.keyCode;
            if ((sKeyCode < 48 || sKeyCode > 57) || (sKeyCode == 95) || (sKeyCode == 32) || (sKeyCode == 46) ) // && (sKeyCode == 32 || sKeyCode == 95 || sKeyCode == 46) && (sKeyCode < 97 || sKeyCode > 123) )
            {
                if(isdecimalAllowed)
                {
                    if(sKeyCode==46)
                    {
                        if(txtbox.value.indexOf('.')>-1)
                        {
                            window.event.keyCode = 0;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                 }
                 if(isNegativeAllowed)
                 {
                    if(sKeyCode==45)
                    {
                        if(txtbox.value.indexOf('-')>-1)
                        {
                            window.event.keyCode = 0;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                 }
                window.event.keyCode = 0;
                return false;
            }
            if(txtbox.value.length > txtbox.maxlength)
            {
                window.event.keyCode = 0;
                return false;
            }
        }
        
   //Method to Check zero only in the Text box     
          function ChkIsZero(textbox,strFieldName)
        {
            if(parseFloat(textbox.value)==0)
            {
                textbox.focus();
                textbox.value='';
                if(strFieldName == undefined)
                {
                alert('Value should be greater than 0');
                }
                else
                {
                alert(strFieldName + ' should be greater than 0');
                }
            }
        }
        
      //Round of to desired prefix and sufix   
        function funChkDecimial(txtbox,prefixLen,sufixLen,strFieldName,IsZeroCheckReq)
          {
            if(txtbox.value !='')
            {
                if(IsZeroCheckReq)
                {
                     if(parseFloat(txtbox.value)==0)
                        {
                            txtbox.focus();
                            txtbox.value='';
                            if(strFieldName == undefined)
                            {
                                alert('Value should be greater than 0');
                                 txtbox.value="0";
                                 txtbox.focus();
                                 return false;
                            }
                            else
                            {
                                alert(strFieldName + ' should be greater than 0');
                                 txtbox.value="0";
                                 txtbox.focus();
                                 return false;
                            }
                        }
                 }
                if(isNaN(parseFloat(txtbox.value)))
                {
                    alert('Enter a valid decimal');
                    txtbox.value="";
                    txtbox.focus();
                    return false;
                }
                else
                {
                    var str = txtbox.value.split('.');
                   
                    if(str[0].length > prefixLen) {
                        //precision Remove by Thangam M on 15/Feb/2012
                        if(strFieldName == '')
                        {
                            alert('Value should not exceed '+prefixLen +' digits');
                        }
                        else
                        {
                            alert(strFieldName + ' should not exceed '+ prefixLen +' digits');
                        }
                        txtbox.focus();
                        return false;
                    }
                    txtbox.value = parseFloat(txtbox.value).toFixed(sufixLen);

                    //Added by Thangam M on 22/Feb/2012 to check for zero after round off.
                    if (IsZeroCheckReq) {
                        if (parseFloat(txtbox.value) == 0) {
                            txtbox.focus();
                            txtbox.value = '';
                            if (strFieldName == undefined) {
                                alert('Value should be greater than 0');
                                txtbox.value = "0";
                                txtbox.focus();
                                return false;
                            }
                            else {
                                alert(strFieldName + ' should be greater than 0');
                                txtbox.value = "0";
                                txtbox.focus();
                                return false;
                            }
                        }
                    }
                    
                    
                }
            }
            return true;
        }
        
        function funChkDecimial_OPC(txtbox, prefixLen, sufixLen, strFieldName, IsZeroCheckReq) {
            if (txtbox.value != '') {
                if (IsZeroCheckReq) {
                    if (parseFloat(txtbox.value) == 0) {
                        txtbox.focus();
                        txtbox.value = '';
                        if (strFieldName == undefined) {
                            alert('Value should be greater than 0');
                            txtbox.value = "0";
                            txtbox.focus();
                            return false;
                        }
                        else {
                            alert(strFieldName + ' should be greater than 0');
                            txtbox.value = "0";
                            txtbox.focus();
                            return false;
                        }
                    }
                }
                if (isNaN(parseFloat(txtbox.value))) {
                    alert('Enter a valid decimal');
                    txtbox.value = "";
                    txtbox.focus();
                    return false;
                }
                else {
                    var str = txtbox.value.split('.');

                    if (str[0].length > prefixLen) {
                        //precision Remove by Thangam M on 15/Feb/2012
                        if (strFieldName == '') {
                            alert('Value should not exceed ' + prefixLen + ' digits');
                        }
                        else {
                            alert(strFieldName + ' should not exceed ' + prefixLen + ' digits');
                        }
                        txtbox.focus();
                        return false;
                    }

                    if (str.length > 1 && str[1].length > sufixLen) {
                        txtbox.value = parseFloat(txtbox.value);
                        var s = txtbox.value.split('.');
                        if (s.length > 1 && s[1].length > sufixLen)
                            txtbox.value = parseFloat(txtbox.value).toFixed(sufixLen);
                    }
                    else {
                        txtbox.value = parseFloat(txtbox.value);
                    }

                    //Added by Thangam M on 22/Feb/2012 to check for zero after round off.
                    if (IsZeroCheckReq) {
                        if (parseFloat(txtbox.value) == 0) {
                            txtbox.focus();
                            txtbox.value = '';
                            if (strFieldName == undefined) {
                                alert('Value should be greater than 0');
                                txtbox.value = "0";
                                txtbox.focus();
                                return false;
                            }
                            else {
                                alert(strFieldName + ' should be greater than 0');
                                txtbox.value = "0";
                                txtbox.focus();
                                return false;
                            }
                        }
                    }


                }
            }
            return true;
        }

        //Created by Tamilselvan.S on 14/02/2011 
        function funChkDecimialWithPlusMin(txtbox,prefixLen,sufixLen,strFieldName,IsZeroCheckReq)
        {
            if(txtbox.value !='')
            {
                if(IsZeroCheckReq)
                {
                     if(parseFloat(txtbox.value)==0)
                        {
                            txtbox.focus();
                            txtbox.value='';
                            if(strFieldName == undefined)
                            {
                                alert('Value should be greater than 0');
                                 txtbox.value="0";
                                 txtbox.focus();
                                 return false;
                            }
                            else
                            {
                                alert(strFieldName + ' should be greater than 0');
                                 txtbox.value="0";
                                 txtbox.focus();
                                 return false;
                            }
                        }
                }
                if(isNaN(parseFloat(txtbox.value)))
                {
                    alert('Enter a valid decimal');
                    txtbox.value="";
                    txtbox.focus();
                    return false;
                }                
                var prefixLenF=prefixLen;
                if(txtbox.value.startsWith("-"))
                {
                      prefixLenF++;
                }
                var str = txtbox.value.split('.');
                if(str[0].length > prefixLenF)
                {
                     if(strFieldName == '')
                     {
                          alert('Value should not exceed '+prefixLen +' digits');
                     }
                     else
                     {
                          alert(strFieldName + ' should not exceed '+ prefixLen +' digits');
                     }
                     txtbox.focus();
                     return false;
                }
                if(str.length>1 && str[1].length > sufixLen)
                {
                     if(strFieldName == '')
                     {
                          alert('Value Suffix should not exceed '+sufixLen +' digits');
                     }
                     else
                     {
                          alert(strFieldName + ' Suffix should not exceed '+ sufixLen +' digits');
                     }
                     txtbox.focus();
                     return false;
                }                
                if(str.length==1 || str[1].length==0 || str[1].length==1)
                {
                    txtbox.value=parseFloat(txtbox.value).toFixed(sufixLen); 
                }               
            }
            else if(IsZeroCheckReq && txtbox.value =='')
            {
                 alert(strFieldName + ' should be greater than 0');
                 txtbox.focus();
                 return false;
            }
            return true;
        }
        
        function funCutDecimal(txtbox,sufixLen)
        {
            var key;
            var isCtrl = false;
            var keychar;
            var reg;
            key = window.event.keyCode; isCtrl = window.event.ctrlKey ;
            //alert(key);
            if(key!=8 && key != 9 && key != 37 && key != 38 && key != 39 && key != 40 && key != 16)
            {
                var str = txtbox.value.split('.');
                if(str.length > 1)
                {
                    if(str[1].length > 0)
                    {
                        if(str[1].length > sufixLen)
                        {
                            //var reg3Right = str[1].replace(str[1].substring(sufixLen),'');
                            var reg3Right = str[1].slice(0, -1);
                            txtbox.value=str[0]+ '.'+ reg3Right;
                        }
                       
                    }
                }
            }
            
        }
        
        //Round of to desired prefix and sufix   
        function funChkDecimialMsg(msg,txtbox,prefixLen,sufixLen)
        {
            if(txtbox.value !='')
            {
                if(isNaN(parseFloat(txtbox.value)))
                {
                    alert('Enter a valid decimal');
                    txtbox.focus();
                    return false;
                }
                else
                {
                    var str = txtbox.value.split('.');
                   
                    if(str[0].length > prefixLen)
                    {
                        alert(msg);
                        txtbox.focus();
                        return false;
                    }
                    txtbox.value=parseFloat(txtbox.value).toFixed(sufixLen);
                }
            }
        }
        
        
//Code added By Nataraj on 21/10/2010 ends here .        

function openFileDialog(clientId,txtField,fileOpenDocument)
{         
      var elmentId = clientId.substring(0, clientId.lastIndexOf(txtField));               
      var clientId = elmentId + 'txOD';     
         
      var StrOut = '<input style="DISPLAY:none;" id="File1" type="file" runat="server" defaultValue="a"/>';
      document.getElementById('td1').innerHTML=StrOut;
      document.getElementById('File1').click()               
      var StrOut = '';          
      if(document.getElementById('File1').value != '')
            document.getElementById(clientId).value= document.getElementById('File1').value;  
            
      document.getElementById('td1').innerHTML=StrOut;          

}
//Code added By kannan rc on 01/11/2010 start here .      
function openFile1(fileName)
{
    
    var Fo =new ActiveXObject("Scripting.FileSystemObject");  
    
     if (Fo.FileExists(fileName))
     {
            var extention= Fo.GetExtensionName(fileName).toLowerCase();        
    
    
            if(fileName == '') 
              { 
               return;   
              }         
     
                if(extention=="pdf")//Pdf File
                {
                 if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                     var commandtoRun = "AcroRd32.exe";                                          
                }
                else if(extention=="doc")//Word File
                {
                if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                var commandtoRun = "winword"; 
                }
                
                else if(extention=="docx")//Word File
                {
                if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                var commandtoRun = "winword"; 
                }
                
                else if(extention=="xls")//Excel File
                {
                if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                var commandtoRun = "excel"; 
                }
                
                else if(extention=="xlsx")//Excel File
                {
                if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                var commandtoRun = "excel"; 
                }
                
                else  if(extention=="txt")//Excel File
                {
                if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                var commandtoRun = "notepad"; 
                }
                
                else  if(extention=="jpeg")//Excel File
                {
                if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                var commandtoRun = "Image"; 
                }
                
                else  if(extention=="bmp")//paint File
                {
                if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                     var commandtoRun = "Paint Image"; 
                //var commandtoRun = "Paint Image"; 
                }
                else
                {
                     var word = new ActiveXObject('InternetExplorer.Application');                 
                    if (word != null)  
                      {  
                              word.Visible = true;
                              word.Navigate(fileName);
                      } 
                     return; 
                }  
    
              var commandParms = '"' + fileName + '"'; 
              var oShell = new ActiveXObject("Shell.Application"); 
              oShell.ShellExecute(commandtoRun, commandParms, "", "open", "1");
        }
        else if (Fo.FolderExists(fileName))
        {
              if(!OpenFolder(fileName))
              {
                    //alert('Unable to Open!');
                    isURL(fileName); 
              }
        }  
        else
             // alert('Unable to Open!');  
             isURL(fileName);  
          
}

function openFileDialog(clientId, txtField)
{
    
     var elmentId = clientId.substring(0, clientId.lastIndexOf(txtField));      
     //alert(elmentId);
     var clientId = elmentId + 'txOD';
      
      var StrOut = '<input style="DISPLAY:none;" id="File2" type="file" runat="server" defaultValue="a"/>';
      document.getElementById('td1').innerHTML = StrOut;
      document.getElementById('File2').click()               
      var StrOut = '';          
      if(document.getElementById('File2').value != '')
            document.getElementById(clientId).value = document.getElementById('File2').value;   

      document.getElementById('td1').innerHTML = StrOut;          
}

function openFile(textBoxId)
{
      
  /* open linked file */
    var Fo =new ActiveXObject("Scripting.FileSystemObject");  
    var fileName = document.getElementById(textBoxId).value;
//    
//    if(trim(fileName) == '') 
//    { 
//       return;   
//    } 
    
     if (Fo.FileExists(fileName))
     {
            var extention= Fo.GetExtensionName(fileName).toLowerCase();        
    
    
            if(fileName == '') 
              { 
               return;   
              }         
     
                if(extention=="pdf")//Pdf File
                {
                 if (Fo.FileExists(fileName))
                     var commandtoRun = fileName; 
                 else   
                     var commandtoRun = "AcroRd32.exe";                                          
                }
                else if(extention=="doc")//Word File
                {
                var commandtoRun = "winword"; 
                }
                else if(extention=="xls")//Excel File
                {
                var commandtoRun = "excel"; 
                }
                else  if(extention=="txt")//Excel File
                {
                var commandtoRun = "notepad"; 
                }
                else
                {
                     var word = new ActiveXObject('InternetExplorer.Application');                 
                    if (word != null)  
                      {  
                              word.Visible = true;
                              word.Navigate(fileName);
                      } 
                     return; 
                }  
    
              var commandParms = '"' + fileName + '"'; 
              var oShell = new ActiveXObject("Shell.Application"); 
              oShell.ShellExecute(commandtoRun, commandParms, "", "open", "1");
        }
        else if (Fo.FolderExists(fileName))
        {
              if(!OpenFolder(textBoxId))
              {
                    //alert('Unable to Open!');
                    isURL(fileName); 
              }
        }  
        else
             // alert('Unable to Open!');  
             isURL(fileName);  
          
}
//Code added By kannan rc on 01/11/2010 end here .      

 function funChkMaxGPSLength(txtbox,lbel,gpslen,maxlen)
        {
                if(gpslen > maxlen)
                {
                   txtbox.maxLength = maxlen;
                  
                }
                else if(gpslen < maxlen && txtbox.value.length > gpslen)
                {
                    txtbox.maxLength = gpslen;
                    txtbox.value = '';
                    txtbox.focus();
                    alert(lbel.innerText + ' cannot be greater than ' + txtbox.maxLength);
                    return false;
                }
                else
                {
                   txtbox.maxLength = maxlen;
                }
        
        }
function fnAllowPasteSpecialChar(textbox, allowSpecialChar,typenottoallow) {
            var sKeyCode = window.event.keyCode;
            
            if(allowSpecialChar)
            {
             
            //38-&,60-<,62->
              if (sKeyCode == 39 || sKeyCode == 34) {
                    
                    window.event.keyCode = 0;
                    return false;
                }
               
               
            }
            else {
                
                var r = {
                    'special': /[\W]/g,
                    'quotes': /['\''&'\"']/g,
                    'notnumbers': /[^\d]/g
                }
                textbox.value = textbox.value.replace(r[typenottoallow], '');
                }
           
        }
        
        function fnGetRecoveryValue(strRecoveryPatternYear)
        {
            var exp = /_/gi;
            return parseFloat(document.getElementById(strRecoveryPatternYear).value.replace(exp, '0'));
        }
        
        function fnCalculateMarginPercentage(strMarginPercentage)
        {      
            var RecoveryPatternYear1;
            RecoveryPatternYear1 = fnGetRecoveryValue(strMarginPercentage);
            if(RecoveryPatternYear1 > 100) 
            {
                alert('CTR and PLR % should not be greater than 100%');
                document.getElementById(strMarginPercentage).focus();
                return;
            }    
        }
        
     /* To display tooltip for Dropdown list */   
    function showDDTooltip(objDrop,e,divId)
    {        
        if(objDrop.options.length>0)
        {
            document.getElementById(divId).style.display="inline";
            document.getElementById(divId).innerHTML="&nbsp;"+objDrop.options[objDrop.selectedIndex].text+"&nbsp;";
            document.getElementById(divId).style.left=e.x;
            document.getElementById(divId).style.top=e.y;
        }
    }
    
     /* To hide tooltip for Dropdown list */   
    function hideDDTooltip(divId)
    {
        document.getElementById(divId).style.display="none";
    } 
    

//This is to first character should start with  albhabets. not a numbers & special characters 

function fnCheckSpecialCharsStartWithAlphabets(isSpaceAllowed,strValue) 
{
    if(trim(strValue).length==0)
    {
        var sKeyCode = window.event.keyCode;               
        if ((!isSpaceAllowed)&&(sKeyCode==32))
        {
            window.event.keyCode=0;
            return false;
        }      
        if ((sKeyCode < 97 || sKeyCode > 122) && (sKeyCode < 65 || sKeyCode > 90)  && sKeyCode != 32 && sKeyCode != 95 ) 
        {
            window.event.keyCode=0;
            return false;
        }    
    }
}

function fnCheckSingleAlphabets(strValue,obj,strMsg)
{
    obj.value = strValue = trim(strValue);
    if(trim(strValue)!="")
    {
        var re = /[a-z]/;   
        var re1 =/[A-Z]/;        
        if (!re.test(strValue.charAt(0)) && !re1.test(strValue.charAt(0))) {   
            alert(strMsg +" should be start with alphabets!");       
            obj.value="";
            obj.focus();            
            return false;   
        }        
        if (!re.test(strValue) && !re1.test(strValue)) {   
            obj.value="";
            obj.focus();
            alert(strMsg +" must contain at least one alphabets!");        
            return false;   
        }
    }   
}

function trim(str) {
        return str.replace(/^\s+|\s+$/g,"");
}

///Created By Tamilselvan.S 
///Created date 10/02/2012
///for Item value added for on drop down change.
function ddl_itemchanged(ddlControl)
{  
    if(ddlControl.options.length>0)
    {    
        ddlControl.title = ddlControl.options[ddlControl.selectedIndex].text;
    }
}
///Created By Tamilselvan.S 
///Created date 15/02/2012
///for tool tip added for on text box mouse over.
function txt_MouseoverTooltip(txtControl)
{
    txtControl.title = txtControl.value;
}

///Created By Thalaiselvam N
///Created date 06-04-2012
///To avoid first character as space in Textbox.

function fnAvoidFirstCharSpace(key,txt)
{
    if (key == 32 && txt.value.length <= 0)
    {
        return false;
    }
    else if (txt.value.length > 0 )
    {
        if (txt.value.charCodeAt(0) == 32)
        {
            txt.value = trim(txt.value);
            return true;
        }
    }
    return true;
}

 //Code Developed by Chandru on 18/Sep/2012
    var monthNames = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    var monthNo = ['01','02','03','04','05','06','07','08','09','10','11','12'];

        function fnDoDate( el, erID, eFormat,IsFutureDateValidation,IsBackDateValidation)
        {         
        
                                       
            var s = el.value;
            var d, m, y;
            var erString = 'Enter the valid date format (' + eFormat + ')';
            var len = s.length;    
            var DtConvDate;           
            if(eFormat == 'MM/dd/yyyy' ||  eFormat == 'MM-dd-yyyy')
            {
                //alert('Front mm');
                if (10 == len)
                {
                    d = s.substring(3,5);
                    m = s.substring(0,2);
                    y = s.substring(6,10);
                } 
                else if (8 == len) 
                {
                    d = s.substring(2,4);
                    m = s.substring(0,2);
                    y = s.substring(4,8);
                } 
                else if (6 == len) 
                {
                    d = s.substring(2,4);
                    m = s.substring(0,2);
                    y = '20' + s.substring(4,6);
                }
                else if (0 == len) 
                {
                    erString = '';                 
                }
                else if (11 == len) 
                {
                    d = s.substring(3,6);
                    m = s.substring(0,2);
                    y = s.substring(7,11);
                    
                    if(m.toUpperCase() == 'JAN')
                        m = '01';
                    if(m.toUpperCase() == 'FEB')
                        m = '02';
                    if(m.toUpperCase() == 'MAR')
                        m = '03';
                    if(m.toUpperCase() == 'APR')
                        m = '04';
                    if(m.toUpperCase() == 'MAY')
                        m = '05';
                    if(m.toUpperCase() == 'JUN')
                        m = '06';
                    if(m.toUpperCase() == 'JUL')
                        m = '07';
                    if(m.toUpperCase() == 'AUG')
                        m = '08';
                    if(m.toUpperCase() == 'SEP')
                        m = '09';
                    if(m.toUpperCase() == 'OCT')
                        m = '10';
                    if(m.toUpperCase() == 'NOV')
                        m = '11';
                    if(m.toUpperCase() == 'DEC')
                        m = '12';                                    
                }
            }
            else
            {
                //alert('Front dd');
                if (10 == len)
                {
                    d = s.substring(0,2);
                    m = s.substring(3,5);
                    y = s.substring(6,10);
                } 
                else if (8 == len) 
                {
                    d = s.substring(0,2);
                    m = s.substring(2,4);
                    y = s.substring(4,8);
                } 
                else if (6 == len) 
                {
                    d = s.substring(0,2);
                    m = s.substring(2,4);
                    y = '20' + s.substring(4,6);
                }
                else if (0 == len) 
                {
                    erString = '';                 
                }
                else if (11 == len) 
                {
                    d = s.substring(0,2);
                    m = s.substring(3,6);
                    y = s.substring(7,11);
                    
                    if(m.toUpperCase() == 'JAN')
                        m = '01';
                    if(m.toUpperCase() == 'FEB')
                        m = '02';
                    if(m.toUpperCase() == 'MAR')
                        m = '03';
                    if(m.toUpperCase() == 'APR')
                        m = '04';
                    if(m.toUpperCase() == 'MAY')
                        m = '05';
                    if(m.toUpperCase() == 'JUN')
                        m = '06';
                    if(m.toUpperCase() == 'JUL')
                        m = '07';
                    if(m.toUpperCase() == 'AUG')
                        m = '08';
                    if(m.toUpperCase() == 'SEP')
                        m = '09';
                    if(m.toUpperCase() == 'OCT')
                        m = '10';
                    if(m.toUpperCase() == 'NOV')
                        m = '11';
                    if(m.toUpperCase() == 'DEC')
                        m = '12';                
                    
                    //alert(m);
                }
            }
            
            // debugger;            
            if (checkDate(y,m,d))
            {                
                if(eFormat == 'dd/MM/yyyy')
                    el.value = d + '/' + monthNo[m-1] + '/' + y;
                if(eFormat == 'dd-MM-yyyy')
                    el.value = d + '-' + monthNo[m-1] + '-' + y;
                if(eFormat == 'MM/dd/yyyy')
                    el.value = monthNo[m-1] + '/' + d + '/' + y;
                if(eFormat == 'MM-dd-yyyy')
                    el.value = monthNo[m-1] + '-' + d + '-' + y;
                if(eFormat == 'dd-MMM-yyyy')
                {   
                    el.value = d + '-' + monthNames[m-1] + '-' + y;
                    DtConvDate = monthNo[m-1] + '/' + d + '/' + y; 
                }
                
                erString = '';                
            }            
            if (document.getElementById)
            {
                //document.getElementById(erID).innerHTML = erString;
                //document.getElementById(erID).value = erString;                
                if(erString != "")
                {                    
                    alert(erString);
                    document.getElementById(erID).value = "";
                }
            }
            if (erString)
            {
                if (el.focus)
                {                    
                    el.focus();                    
                }
            }            
            var varEnteredDateValue;
            var varEnteredDate;
            //var varEnteredDateVal = varEnteredDate;                          
            var vartoday = new Date();
            var dd = vartoday.format('MM/dd/yyyy');
            
            var varCurrentDate = Date.parse(dd);
            
            if(eFormat=='dd-MMM-yyyy')
            {
                varEnteredDateValue = Date.parse(DtConvDate);     
            }
            else
            {                   
                //varEnteredDate= d + '/' + monthNo[m-1] + '/' + y;     
                varEnteredDate= monthNo[m-1] + '/' + d + '/' + y;     
                varEnteredDateValue=Date.parse(varEnteredDate);                  
            }                 
            if(IsFutureDateValidation==true)
            {                               
                 if (varEnteredDateValue > varCurrentDate)
                 {
                    alert('Entered date cannot be greater than system date');
                    document.getElementById(erID).value='';                    
                 }
            }
            if(IsBackDateValidation==true)
            {
                if (varEnteredDateValue < varCurrentDate)
                 {
                    alert('Entered date cannot be less than system date');
                    document.getElementById(erID).value='';                    
                 }
            }
            
             if(IsBackDateValidation=='P')
             {
                if (varEnteredDateValue >= varCurrentDate)
                 {
                    alert('Entered date cannot be greater than or equal to system date');
                    document.getElementById(erID).value='';                    
                 }
            }
             if(IsBackDateValidation=='F')
             {
                if (varEnteredDateValue <= varCurrentDate)
                 {
                    alert('Entered date cannot be less than or equal to system date');
                    document.getElementById(erID).value='';                    
                 }
            }
        } 
                
        function checkDate(y, m, d)
        {
            m = '' + (m-1);                            
            var checkDate = new Date(y,m,d);   
            //alert(m + ' - chandru');         
            return ( checkDate.getMonth() == m && checkDate.getFullYear() == y);
        }
//Code Developed by Chandru on 18/Sep/2012	

   
//This code for AsyncFileUpload control issue : target cannot be  "_top"  : Added by Thangam M on 01/01/2014  
        function fnSetFormTarget() {
            if (window.parent.document.getElementById(window.frames.frameElement.id)) {
                var theForms = document.getElementsByTagName("form");
                theForms[0].target = "";
            }
        }

        