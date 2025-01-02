 /// Page Name       :   Bill Generation
/// Created By      :   Prabhu K
/// Created Date    :   24-Dec-2010
/// Purpose         :   To include Jscript functions

function fnSelectBranch(chkSelectBranch,chkSelectAllBranch)
{
        
        var gvBranchWise = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_gvBranchWise');
        var TargetChildControl = chkSelectAllBranch;
        var selectall = 0;
        var Inputs = gvBranchWise.getElementsByTagName("input");
        if(!chkSelectBranch.checked)
        {
            chkSelectAllBranch.checked = false;
        }
        else
        {
            for (var n = 0; n < Inputs.length; ++n)
            {
            if (Inputs[n].type == 'checkbox')
                {
                    if(Inputs[n].checked)
                    {
                        selectall = selectall + 1;
                    }
                }
            }
            if(selectall == gvBranchWise.rows.length - 1)
            {
              chkSelectAllBranch.checked = true;
            }
         }
         
         
}
 
///Function for Assigning LOB to other Tabs 
 
 function fnAssignMonthYear(txtMonthYear)
 {
    var varMonthYear = txtMonthYear.value;
    if(varMonthYear != "")
    {
        document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabBillOutput_txtBillMonthYear').value = varMonthYear;
        document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabControlDataSheet_txtDataMonthYear').value = varMonthYear;
        
        
    }
 }
 ///Function for Assigning Frequency to other Tabs
          
             
///Function for Select/Unselect All Branches
function fnSelectAll(chkSelectAllBranch,chkSelectBranch)
{
        var gvBranchWise = document.getElementById('ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_gvBranchWise');
        var TargetChildControl = chkSelectBranch;
        //Get all the control of the type INPUT in the base control.
        var Inputs = gvBranchWise.getElementsByTagName("input");
        //Checked/Unchecked all the checkBoxes in side the GridView.
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' &&
            Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
            Inputs[n].checked = chkSelectAllBranch.checked;
}


function onShown() {
            var cal = $find("ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_calMonthYear");
            cal._switchMode("months",true);
            cal._today.innerText = "Current Month :" + cal._today.date.format("MMM-yyyy");
            if(cal._monthsBody)
            {
                for(var i=0;i<cal._monthsBody.rows.length;i++)
                {
                    var row = cal._monthsBody.rows[i];
                    for(var j=0;j<row.cells.length;j++)
                    {
                        Sys.UI.DomEvent.addHandler(row.cells[j].firstChild,"click",callMethod);
                    }
                }
            }
        }
        function onHidden() {
            var cal = $find("ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_calMonthYear");
            if(cal._monthsBody)
            {
                for(var i=0;i<cal._monthsBody.rows.length;i++)
                {
                    var row = cal._monthsBody.rows[i];
                    for(var j=0;j<row.cells.length;j++)
                    {
                        Sys.UI.DomEvent.removeHandler(row.cells[j].firstChild,"click",callMethod);
                    }
                }
            }
            
        }
        
        function callMethod(eventElement)
        {
            var target = eventElement.target;
            var cal = $find("ctl00_ContentPlaceHolder1_tcBilling_TabMainPage_calMonthYear");
            cal._visibleDate = target.date;
            cal.set_selectedDate( target.date);
            cal._switchMonth(target.date);
            cal._today.innerText = "Current Month :" + cal._today.date.format("MMM-yyyy");
            cal._blur.post(true);
            cal.raiseDateSelectionChanged();
        }
        
        
        
      