<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LOVPageNavigator.ascx.cs"
    Inherits="UserControls_LOVPageNavigator" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table cellpadding="0" cellspacing="0" width="100%">
    <tr visible="false" class="styleRecordCount" runat="server" id="trMessage">
        <td colspan="2" align="center">
            <asp:Label runat="server" ID="lblMessage" Text="No Records Found" Font-Size="Medium"
                class="styleMandatoryLabel"></asp:Label>
        </td>
    </tr>
    <tr class="stylePagingControl" runat="server" id="trControl">
        <td style="border: 0px" align="left">
            <table cellpadding="0" cellspacing="0" align="left">
                <tr>
                    <td style="padding-right: 30px">
                        <asp:Label runat="server" CssClass="stylePagingFieldLabel" ID="lblTotalRecords"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnPrevious" runat="server" Enabled="true" CommandName="Prev"
                            OnCommand="Navigation_Link" ImageUrl="../Images/Prev.gif" CausesValidation="false" />
                    </td>
                    <td>
                        <asp:Label runat="server" Text="1" CssClass="stylePagingRecFieldLabel" ID="lblCurrentPage"></asp:Label><span
                            class="stylePagingRecFieldLabel"> / </span>
                        <asp:Label runat="server" Text="1" CssClass="stylePagingRecFieldLabel" ID="lblTotalPages"></asp:Label>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnNext" runat="server" Enabled="true" CommandName="Next" OnCommand="Navigation_Link"
                            ImageUrl="../Images/Next.gif" CausesValidation="false" />
                    </td>
                    <td style="padding-left: 10px">
                        <asp:TextBox runat="server" ID="txtGotoPage" AutoPostBack="true" CssClass="stylePagingTextBox"
                            MaxLength="3" onblur="return fnValidateEmptyGotoPage();" onpaste="return false;" OnTextChanged="btnGO_Click"
                            Width="40px"></asp:TextBox>&nbsp;
                    </td>
                    <td style="padding-bottom: 3px">
                        <asp:Label runat="server" Text="Page Number" CssClass="stylePagingFieldLabel" ID="lblPageNo"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td style="border: 0px" align="right">
            <table>
                <tr>
                    <td>
                        <asp:TextBox runat="server" CssClass="stylePagingTextBox" Width="40px" MaxLength="3" Text="10" style="display:none;"
                            OnTextChanged="btnSize_Click"  onpaste="return false;" AutoPostBack="true" onblur="return fnValidateEmpty();"
                            ID="txtPageSize"></asp:TextBox>
                    </td>
                    <td style="padding-bottom: 3px">
                        <asp:Label runat="server" Text="Page Size" CssClass="stylePagingFieldLabel" ID="lblPageSize" style="display:none;"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<input type="hidden" runat="server" style="width: 1px;" id="hdnTotRec" />
<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtGotoPage"
    FilterType="Numbers">
</cc1:FilteredTextBoxExtender>
<cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtPageSize"
    FilterType="Numbers">
</cc1:FilteredTextBoxExtender>

<script language="javascript" type="text/javascript">

function fnValidateGotoPage()
{
    var iTotPage;
    var iGotoPage;
    iTotPage=document.getElementById('<%=lblTotalPages.ClientID%>').innerText;
    iGotoPage=document.getElementById('<%=txtGotoPage.ClientID%>').value;
    if(iGotoPage==0)
        {
            alert('Go to page cannot be zero');
            document.getElementById('<%=txtGotoPage.ClientID%>').value=1;
            return false;
        }
    if (parseInt(iTotPage)<parseInt(iGotoPage))
        {
        alert('Go to page cannot be greater than total no of pages');
        document.getElementById('<%=txtGotoPage.ClientID%>').value=1;
        return false;
        }
        return true;
}

  function fnValidatePage()
  {      
    var iTotRec;
    var iPageSize;
    iTotRec=document.getElementById('<%=hdnTotRec.ClientID%>').value;
    iPageSize=document.getElementById('<%=txtPageSize.ClientID%>').value;
    
    if(iPageSize==0)
        {
            alert('Page size cannot be zero');
            document.getElementById('<%=txtPageSize.ClientID%>').value=100;
            return false;
        }
   
      // user can enter only page size of 1 - 100
       if (parseInt(iPageSize)>100)
      {
        alert('Page size cannot be greater than 100');
        document.getElementById('<%=txtPageSize.ClientID%>').value=100;
        return true;  
      }
     return true;          
}
function fnValidateEmpty()
{
    if(document.getElementById('<%=txtPageSize.ClientID%>').value=='' || document.getElementById('<%=txtPageSize.ClientID%>').value==0)
        {
            document.getElementById('<%=txtPageSize.ClientID%>').value=1;
            document.getElementById('<%=txtPageSize.ClientID%>').focus();
            alert('Page size cannot be empty or zero');
            return false;
        }
        return(fnValidatePage());
 }
    function fnValidateEmptyGotoPage()
    {    
        if(document.getElementById('<%=txtGotoPage.ClientID%>').value=='' || document.getElementById('<%=txtGotoPage.ClientID%>').value==0)
        {
            document.getElementById('<%=txtGotoPage.ClientID%>').value=1;
            document.getElementById('<%=txtGotoPage.ClientID%>').focus();
            alert('Go to page cannot be empty or zero');
            return false;
        }
        return(fnValidateGotoPage());
    }
</script>

