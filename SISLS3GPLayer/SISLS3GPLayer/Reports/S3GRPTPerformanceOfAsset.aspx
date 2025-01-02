<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRPTPerformanceOfAsset.aspx.cs" Inherits="Reports_S3GRPTPerformanceOfAsset"
    Title="Performance of Asset" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Performance of Asset Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlHeader" runat="server" CssClass="stylePanel" GroupingText="Header Details">
                    <table width="100%">
                        <tr align="left">
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblRptType" Text="Report Type" runat="server" />
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <%--  <asp:TextBox ID="txtRptType" runat ="server" Width ="160px" />--%>
                                <asp:DropDownList ID="ddlReportType" runat="server" Width="160px"  AutoPostBack ="true"  OnSelectedIndexChanged ="ddlReportType_SelectedIndexChanged" ToolTip="Report Type">
                                    <asp:ListItem Value="0" Text="--Select--" />
                                    <asp:ListItem Value="1" Text="Existing" />
                                    <asp:ListItem Value="2" Text="New" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None"
                                    InitialValue="0" ValidationGroup="btnGo" ErrorMessage="Select Report Type" ControlToValidate="ddlReportType"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblIRR" Text="IRR Type" runat="server" />
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlIRRType" ToolTip="IRR Type" AutoPostBack="true" OnSelectedIndexChanged="ddlIRRType_SelectedIndexChanged"
                                    runat="server" Width="160px">
                                    <asp:ListItem Value="0" Text="--Select--" />
                                    <asp:ListItem Value="3" Text="Accounting IRR" />
                                    <asp:ListItem Value="1" Text="Business IRR" />
                                    <asp:ListItem Value="2" Text="Company IRR" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                    InitialValue="0" ValidationGroup="btnGo" ErrorMessage="Select IRR Type" ControlToValidate="ddlIRRType"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr align="left">
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblLOB" Text="Line of Business" runat="server" />
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlLOB" ToolTip="Line of Business" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged"
                                    runat="server" Width="160px" />
                                <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" Display="None" InitialValue="-1"
                                    ValidationGroup="btnGo" ErrorMessage="Select Line of Business" ControlToValidate="ddlLob"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel">
                                <asp:Label ID="lblAmtFin" Text="Denomination" runat="server" />
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlDenomination" runat="server" OnSelectedIndexChanged="ddlDenomination_SelectedIndexChanged"
                                    AutoPostBack="true" ToolTip="Denomination" Width="160px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="styleFieldLabel">
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnGo" runat="server" Text="GO" ValidationGroup="btnGo" CausesValidation="true"
                    ToolTip="To Get the Weighted IRR details" OnClientClick="return fnCheckPageValidators('btnGo',false);" OnClick="btnGo_Click" CssClass="styleSubmitButton" />
                &nbsp;
                <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                    Text="Clear" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click"
                    ToolTip="To clear the contents" />
            </td>
        </tr>
        <tr>
            <td class="styleFieldLabel">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                <%--<asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel runat="server" ID="pnlWIRR" CssClass="stylePanel" GroupingText="Weighted IRR"
                    Width="100%">
                    <div style="overflow: auto; height: 250px;" id="divDetails" runat="server">
                    <asp:Label ID="lblNoRecords" runat ="server" Text ="No Records Found" Visible ="false"  />
                        <asp:GridView ID="gvPerformanceAsset" runat="server" AutoGenerateColumns="false"
                            ShowFooter="true" Width="125%">
                            <FooterStyle HorizontalAlign="Right" />
                            <Columns>
                                <%--    <asp:TemplateField  HeaderText ="Asset Code">
      <ItemTemplate >
      <asp:Label ID="lblAssetCode" Text ='<% #Eval("Asset_Code")%>' runat ="server" />
      </ItemTemplate>
      <ItemStyle Width ="14%" HorizontalAlign ="Left" />
      </asp:TemplateField>--%> 
      <%-- <asp:TemplateField HeaderText="Line of Business">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLOBgrid" Text='<% #Eval("LOB")%>' ToolTip ="Line of Business" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Asset Class">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("ASSETCLASS")%>' ToolTip ="Asset Class" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="14%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Make">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("ASSETMAKE")%>'  ToolTip ="Asset Make" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="14%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New/Old">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("NewOrOld")%>' ToolTip ="Asset Classification(New/Old)" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Left" />
                                    <FooterTemplate>
                                        <asp:Label ID="lblTot" runat="server" Text="Total"  ToolTip ="Total" /></FooterTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BCD %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BCD")%>' ToolTip ="Business created for the day Weighted IRR" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBCD" Text="BCD" runat="server" ToolTip ="Sum of BCD %" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BCD Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BCDAmt")%>' ToolTip ="BCD Amount" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBCDAmt" Text="BCDAmt" ToolTip ="Sum of BCD Amount" runat="server" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BDD %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BDD")%>' ToolTip ="Business authorized for the day Weighted IRR" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBDD" Text="BDD" runat="server" ToolTip ="Sum of BDD %" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BDD Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BDDAmt")%>' ToolTip ="BDD Amount" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBDDAmt" Text="BDDAmt" runat="server" ToolTip ="Sum of BDD Amount" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BCM %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BCM")%>' ToolTip ="Business created during the current month Weighted IRR" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBCM" Text="BCM" runat="server" ToolTip ="Sum of BCM %" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BCM Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BCMAmt")%>' ToolTip ="BCM Amount" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBCMAmt" Text="BCMAmt" ToolTip ="Sum of BCM Amount" runat="server" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BDM %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BDM")%>' ToolTip ="Business authorized during the current month Weighted IRR" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBDM" Text="BDM" runat="server" ToolTip ="Sum of BDM %" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BDM Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BDMAmt")%>' ToolTip ="BDM Amount" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBDMAmt" Text="BDMAmt" runat="server" ToolTip="Sum of BDM Amount" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BCY %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BCY")%>' ToolTip ="Business created during the current financial year Weighted IRR" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBCY" Text="BCY" runat="server" ToolTip ="Sum of BCY %" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BCY Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BCYAmt")%>' ToolTip ="BCY Amount" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBCYAmt" Text="BCYAmt" runat="server" ToolTip ="Sum of BCY Amount" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BDY %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BDY")%>' ToolTip ="Business authorized during the current financial year Weighted IRR" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBDY" Text="BDY" runat="server" ToolTip ="Sum of BDY %" />
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="BDY Amt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetCode" Text='<% #Eval("BDYAmt")%>' ToolTip ="BDY Amount" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblBDYAmt" Text="BDYAmt" runat="server"  ToolTip ="Sum of BDY Amount"/>
                                    </FooterTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="styleFieldLabel">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="2" align="center">
             <asp:Button runat="server" ID="btnChart" CssClass="styleSubmitButton" Text="View Chart"
                    ToolTip="To View the Pie Chart" CausesValidation="false" OnClick="btnChart_Click"
                    Visible="false" />
                <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print"
                    ToolTip="To Print the Report" CausesValidation="false" OnClick="btnPrint_Click"
                    Visible="false" />
            <%--    <asp:Button runat="server" ID="btnEmail" CssClass="styleSubmitButton" Text="Email"
                    ToolTip="To send the mail" CausesValidation="false" OnClick="btnEmail_Click"
                    Visible="false" />--%>
               
            </td>
        </tr>
      
        <tr>
            <td width="100%" colspan="2">
                <asp:ValidationSummary ID="vgAssetPerformance" runat="server" ValidationGroup="btnGo"
                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                    ShowSummary="true" />
                <asp:CustomValidator ID="CVPerformanceofAsset" runat="server" Display="None" ValidationGroup="btnGo"></asp:CustomValidator>
            </td>
        </tr>
    </table>
    <table >
    <tr><td>
  <asp:Button runat="server" ID="btnpop" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />
      <cc1:ModalPopupExtender ID="PopupSelectUsersToMail" runat="server" TargetControlID="btnpop"
            PopupControlID="PnlSendmail" BackgroundCssClass="styleModalBackground">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="PnlSendmail" GroupingText="" runat="server" Style="display: none" BackColor="White" 
                        BorderStyle="Solid">
            <asp:UpdatePanel ID="upnlmail" runat="server">
                <ContentTemplate>
                    <table align="center" >
                        <tr>
                            <td align="center" >
                               <div id="divmail" style ="width:400px; height:450px; overflow :scroll ; vertical-align :middle ;   "> 
                                <asp:GridView ID="gvmail" runat="server"  AutoGenerateColumns="false" Width ="100%"  OnRowDataBound ="gvmail_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField >
                                        <HeaderTemplate >
                                        <asp:Label ID="Hlblusername" runat="server" Text="Name" />
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign ="Center" />
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnUserID" runat="server" Value='<% #Eval("User_ID") %>' />
                                                <asp:Label ID="lblusername" runat="server" Text='<% #Eval("UserNames") %>' ToolTip ="Name" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign ="Left"  Width ="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                           <HeaderTemplate >
                                        <asp:Label ID="Hlblmailid" runat="server" Text="Mail ID" />
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign ="Center" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblmailid" runat="server" Text='<%#Eval("Email_ID")%>'  ToolTip ="Mail ID"/>
                                                <asp:HiddenField ID="hdnUserLevelID" runat="server" Value='<%#Eval("User_Level_ID") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign ="Left"   Width ="60%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <HeaderTemplate >
                                        <asp:Label ID="HlblSelect" runat="server" Text="Select All" />
                                         <asp:CheckBox ID="chkSelectAll" Checked="false" AutoPostBack ="true"  runat="server" ToolTip ="Select All" />
                                        </HeaderTemplate> <%--OnCheckedChanged ="chkSelectAll"--%>
                                        <HeaderStyle HorizontalAlign ="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" Checked="false" runat="server"  ToolTip ="Select" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign ="Center"  Width ="10%"  />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView></div>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" >
                                <asp:Button runat="server" ID="btnSend" CssClass="styleSubmitButton" Text="Send Mail"
                                    ToolTip="To send mail to selected users" CausesValidation="false" OnClick="btnSend_Click"
                                    Visible="true" />  &nbsp;&nbsp;
                                <asp:Button runat="server" ID="btnClosePopup" CssClass="styleSubmitButton" Text="Cancel"
                                    ToolTip="To canel" CausesValidation="false" OnClick="btnClosePopup_Click" Visible="true" />
                            </td>
                        </tr>
                        <tr ><td> &nbsp;</td></tr>
                            <tr>
            <td width="100%" colspan="2">
               <%-- <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="vgmail"
                    CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                    ShowSummary="true" />--%>
                <asp:CustomValidator ID="CVmails" runat="server"  ></asp:CustomValidator>
            </td>
        </tr>
                  
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    
    </td></tr>
    </table>
    

    <script type="text/javascript"> 

   function Resize()
     {
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
      }
      
 function showMenu(show) 
      {
       if (show == 'T') 
         {
        
          if(document.getElementById('divGrid1')!=null)
            {
                document.getElementById('divGrid1').style.width="800px";
                document.getElementById('divGrid1').style.overflow="scroll";
            }
        
            document.getElementById('divMenu').style.display = 'Block';
            document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

            document.getElementById('ctl00_imgShowMenu').style.display = 'none';
            document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

            if(document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
            (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - 270;
        }
        if (show == 'F') 
        {
            if(document.getElementById('divGrid1')!=null)
                {
                    document.getElementById('divGrid1').style.width="960px";
                    document.getElementById('divGrid1').style.overflow="auto";
                }
                
            document.getElementById('divMenu').style.display = 'none';
            document.getElementById('ctl00_imgHideMenu').style.display = 'none';
            document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
           
            if(document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
           (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
        }
    }
 



function fnSelectUser(chkSelect,chkSelectAll)
{
        
        var gvmail = document.getElementById('ctl00_ContentPlaceHolder1_divmail_gvmail');
        var TargetChildControl = chkSelectAll;
        var selectall = 0;
        var Inputs = gvmail.getElementsByTagName("input");
        if(!chkSelect.checked)
        {
            chkSelectAll.checked = false;
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
            if(selectall == gvmail.rows.length - 1)
            {
              chkSelectAll.checked = true;
            }
         }
         
         
}
 

    </script>

</asp:Content>
