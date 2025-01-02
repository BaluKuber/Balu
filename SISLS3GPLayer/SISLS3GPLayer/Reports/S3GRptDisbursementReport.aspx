<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptDisbursementReport.aspx.cs" Inherits="Reports_S3GRptDisbursementReport"
    Title="Disbursement Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Disbursement Report">
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td class="styleFieldAlign">
                <asp:Panel runat="server" ID="PnlDisbursementreport" CssClass="stylePanel" GroupingText="INPUT CRITERIA"
                    Width="100%">
                    <table width="100%">
                        <tr>
                            <td class="styleFieldAlign">
                                <asp:Label ID="lblDisbursementReport" runat="server" CssClass="styleDisplayLabel"  Text="Disbursement Report" ToolTip="Disbursement Report"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" colspan="3">
                                <asp:CheckBox ID="chkAbstract" runat="server" Checked="true" OnCheckedChanged="chkAbstract_CheckedChanged" AutoPostBack="true"  ToolTip="Abstract"/>
                                <asp:Label ID="lblAbstract" runat="server" CssClass="styleDisplayLabel"  Text="Abstract" ToolTip="Abstract"></asp:Label>
                                <asp:CheckBox ID="chkDetailed" runat="server"   OnCheckedChanged="chkDetailed_CheckedChanged" AutoPostBack="true" ToolTip="Detailed" />
                                <asp:Label ID="lblDetailed" runat="server" CssClass="styleDisplayLabel"  Text="Detailed" ToolTip="Detailed"></asp:Label>
                                
                            </td>
                        </tr>
                        <tr>
                            <td width="25%" class="styleFieldAlign">
                                <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleDisplayLabel" ToolTip="Line of Business"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%" >
                                <asp:DropDownList ID="ddlLOB" runat="server"  OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" AutoPostBack="True"  ValidationGroup="Header" ToolTip="Line of Business"
                                    >
                                </asp:DropDownList>
                            </td>
                           <td class="styleFieldAlign" width="25%">
                                <asp:Label ID="lblregion" runat="server" CssClass="styleDisplayLabel"  Text="Location1" ToolTip="Location1"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%" >
                                <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                    onselectedindexchanged="ddlRegion_SelectedIndexChanged" ToolTip="Location1" >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                        <td class="styleFieldAlign" width="25%">
                                <asp:Label ID="lblbranch" runat="server" CssClass="styleDisplayLabel" Text="Location2" ToolTip="Location2"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlbranch" runat="server" AutoPostBack="True" ValidationGroup="Header" ToolTip="Location2"
                                    >
                                </asp:DropDownList>
                            </td>
                           
                            <td class="styleFieldAlign" width="25%">
                                <asp:Label ID="lblProduct" runat="server" Text="Product" CssClass="styleDisplayLabel" ToolTip="Product"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="True" onselectedindexchanged="ddlProduct_SelectedIndexChanged"  ValidationGroup="Header" ToolTip="Product"
                                   >
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldAlign">
                                <asp:Label ID="lblStartDateSearch" runat="server" CssClass="styleDisplayLabel" Text="Start Date" ToolTip="Start Date" />
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <input id="hidDate" runat="server" type="hidden" />
                                <asp:TextBox ID="txtStartDateSearch" runat="server" OnTextChanged="txtStartDateSearch_TextChanged" AutoPostBack="true" Width="100" ToolTip="Start Date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="None" ControlToValidate="txtStartDateSearch"
                                    ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select Start Date"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                    PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch" >
                                </cc1:CalendarExtender>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:Label ID="lblEndDateSearch" runat="server" CssClass="styleDisplayLabel" Text="End Date" ToolTip="End Date" />
                                <span class="styleMandatory">*</span>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtEndDateSearch" runat="server" OnTextChanged="txtEndDateSearch_TextChanged" AutoPostBack="true" Width="100" ToolTip="End Date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" Display="None" ControlToValidate="txtEndDateSearch"
                                    ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select End Date"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                    PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" OnClick="btnGo_Click"
                    ValidationGroup="btnGo"  OnClientClick="return fnCheckPageValidators('btnGo',false);" ToolTip="Go" />&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear" OnClientClick="return fnConfirmClear();"  OnClick="btnClear_Click" ToolTip="Clear"/>
            </td>
        </tr>
        <tr>
        <td align="right" colspan="2" width="100%">
        <asp:Label ID="lblAmounts" runat="server" Text="[All amounts are in" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
        <asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
        </td>
        </tr>
        <tr>
            <td class="styleFieldAlign" colspan="4">
                <asp:Panel ID="PnlAbstract" runat="server" CssClass="stylePanel" GroupingText="Abstract Report">
                   <div id="divAbstract" runat="server" style="overflow: scroll; height: 200px; display:none" >
                    <asp:GridView ID="grvAbstract" runat="server" AutoGenerateColumns="False"  BorderWidth="2"
                        CssClass="styleInfoLabel" ShowFooter="true" Style="margin-bottom: 0px" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Line Of Business">
                                <ItemTemplate>
                                    <asp:Label ID="lblLOBGA" runat="server" Text='<%# Bind("LobName") %>' ToolTip="Line Of Business"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"  />
                            </asp:TemplateField>
                           <%-- <asp:TemplateField HeaderText="Region">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegion" runat="server" Text='<%# Bind("Region") %>' ToolTip="Region"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchGA" runat="server" Text='<%# Bind("Branch") %>' ToolTip="Branch"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductGA" runat="server" Text='<%# Bind("Product") %>' ToolTip="Product"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                         <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approved Amount" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedAmountA" runat="server" style="text-align:right" Text='<%# Bind("ApprovedAmount") %>' ToolTip="Approved Amount"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>
                                        <asp:Label ID="lblFApprovedAmountA" runat="server" ToolTip="sum of  Approved Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right"  />
                                <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField >
                            <asp:TemplateField HeaderText="Paid Amount" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaidAmountA" runat="server" style="text-align:right" Text='<%# Bind("PaidAmount") %>' ToolTip="Paid Amount"></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lblFPaidAmountA" runat="server" ToolTip="sum of  Paid Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right"  />
                                 <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remaining Amount" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemainingAmountA" runat="server" style="text-align:right" Text='<%# Bind("RemainingAmount") %>' ToolTip="Remaining Amount"></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lblFRemainingAmountA" runat="server" ToolTip="sum of  Remaining Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right"/>
                                 <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Not Yet Created " ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountYetToBeCreatedA" runat="server" style="text-align:right" Text='<%# Bind("AccountYetToBeCreated") %>' ToolTip="Account Yet To Be Created"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>
                                        <asp:Label ID="lblFAccountYetToBeCreatedA" runat="server" ToolTip="sum of Account Yet to be Created Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                 <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ageing 0-30 Days">
                                <ItemTemplate>
                                    <asp:Label ID="lblageing0days" runat="server" style="text-align:right" Text='<%# Bind("ageing0days") %>' ToolTip="Ageing0-30Days"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>
                                        <asp:Label ID="lblageing0daysf" runat="server" ToolTip="sum of Ageing 0-30 Days"></asp:Label>
                                    </FooterTemplate>
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                  <FooterStyle HorizontalAlign="Right" />
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Ageing 31-60 days">
                                <ItemTemplate>
                                    <asp:Label ID="lblageing30days" runat="server" style="text-align:right" Text='<%# Bind("ageing30days") %>' ToolTip="Ageing31-60days"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>
                                        <asp:Label ID="lblageing30daysf" runat="server" ToolTip="sum of Ageing 31-60 Days"></asp:Label>
                                    </FooterTemplate>
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right"  />
                                <FooterStyle HorizontalAlign="Right" />
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Ageing Above 60 days">
                                <ItemTemplate>
                                    <asp:Label ID="lblageing60days" runat="server" style="text-align:right" Text='<%# Bind("ageing60days") %>' ToolTip="Ageing61-90days"></asp:Label>
                                </ItemTemplate>
                                   <FooterTemplate>
                                        <asp:Label ID="lblageing60daysf" runat="server" ToolTip="sum of Ageing 61-90 Days"></asp:Label>
                                    </FooterTemplate>
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                  <FooterStyle HorizontalAlign="Right" />
                                 </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="styleFieldAlign" colspan="4">
                <asp:Panel ID="PnlDetailedView" runat="server" CssClass="stylePanel" GroupingText="Detailed Report">
                 <div id="divDetails" runat="server" style="overflow: scroll; height: 200px;" >
                    <asp:GridView ID="GrvDetails" runat="server" AutoGenerateColumns="False"  BorderWidth="2"
                        CssClass="styleInfoLabel" ShowFooter="true" Style="margin-bottom: 0px" Width="100%">
                        <Columns>
                            
                            <asp:TemplateField HeaderText="Line Of Business">
                                <ItemTemplate>
                                    <asp:Label ID="lblLOBGD" runat="server" Text='<%# Bind("LobName") %>' ToolTip="Line Of Business"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"  />
                            </asp:TemplateField>
                         <%--   <asp:TemplateField HeaderText="Region">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegionGD" runat="server" Text='<%# Bind("Region") %>' ToolTip="Region"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchGD" runat="server" Text='<%# Bind("Branch") %>' ToolTip="Branch"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <asp:Label ID="lblProductGD" runat="server" Text='<%# Bind("Product") %>' ToolTip="Product"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"  />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Application No">
                                <ItemTemplate>
                                    <asp:Label ID="lblApplicationNo" runat="server" Text='<%# Bind("ApplicationNumber") %>' ToolTip="Application No"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="Customer"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account No">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrimeAccount" runat="server" Text='<%# Bind("PrimeAccount") %>' ToolTip="Account No"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Account No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubAccount" runat="server" Text='<%# Bind("SubAccount") %>' ToolTip="Sub Account No"></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Left"  />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approved Amount" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedAmountGD" runat="server" style="text-align:right" Text='<%# Bind("ApprovedAmount") %>' ToolTip="Approved Amount"></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lblFApprovedAmountGD" runat="server" ToolTip="sum of  Approved Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                 <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Paid Amount" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaidAmountGD" runat="server" style="text-align:right" Text='<%# Bind("PaidAmount") %>' ToolTip="Paid Amount"></asp:Label>
                                </ItemTemplate>
                                           <FooterTemplate>
                                        <asp:Label ID="lblFPaidAmountGD" runat="server" ToolTip="sum of Paid Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                 <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Remaining Amount" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblRemainingAmountGD" runat="server" style="text-align:right" Text='<%# Bind("RemainingAmount") %>' ToolTip="Remaining Amount"></asp:Label>
                                </ItemTemplate>
                                         <FooterTemplate>
                                        <asp:Label ID="lblFRemainingAmountGD" runat="server" ToolTip="sum of Remaining Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                 <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Not Yet Created" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountYetToBeCreatedGD" runat="server" style="text-align:right" Text='<%# Bind("AccountYetToBeCreated") %>' ToolTip="Account Yet To Be Created"></asp:Label>
                                </ItemTemplate>
                                      <FooterTemplate>
                                        <asp:Label ID="lblFAccountYetToBeCreatedGD" runat="server" ToolTip="sum of Account Yet To Be Created Amount"></asp:Label>
                                    </FooterTemplate>
                                <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                 <FooterStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ageing 0-30 Days">
                                <ItemTemplate>
                                    <asp:Label ID="lblageing0days" runat="server" style="text-align:right" Text='<%# Bind("ageing0days") %>' ToolTip="Ageing0-30Days"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>
                                        <asp:Label ID="lblageing0daysfd" runat="server" ToolTip="sum of Ageing 0-30 Days"></asp:Label>
                                    </FooterTemplate>
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                  <FooterStyle HorizontalAlign="Right" />
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Ageing 31-60 days">
                                <ItemTemplate>
                                    <asp:Label ID="lblageing30days" runat="server" style="text-align:right" Text='<%# Bind("ageing30days") %>' ToolTip="Ageing31-60days"></asp:Label>
                                </ItemTemplate>
                                  <FooterTemplate>
                                        <asp:Label ID="lblageing30daysfd" runat="server" ToolTip="sum of Ageing 31-60 Days"></asp:Label>
                                    </FooterTemplate>
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right"  />
                                <FooterStyle HorizontalAlign="Right" />
                                 </asp:TemplateField>
                                 
                                 <asp:TemplateField HeaderText="Ageing Above 60 days">
                                <ItemTemplate>
                                    <asp:Label ID="lblageing60days" runat="server" style="text-align:right" Text='<%# Bind("ageing60days") %>' ToolTip="Ageing61-90days"></asp:Label>
                                </ItemTemplate>
                                   <FooterTemplate>
                                        <asp:Label ID="lblageing60daysfd" runat="server" ToolTip="sum of Ageing 61-90 Days"></asp:Label>
                                    </FooterTemplate>
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Right" />
                                  <FooterStyle HorizontalAlign="Right" />
                                 </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="4">
                <asp:Button ID="btnPrint" CssClass="styleSubmitButton" runat="server" Text="Print" OnClick="btnPrint_Click" ToolTip="Print" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:ValidationSummary ID="vsDis" runat="server" CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):"
                    ShowSummary="true" ValidationGroup="btnGo" />
            </td>
        </tr>
        <tr>
                    <td>
                        <asp:CustomValidator ID="CVDisbursement" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
    </table>
   <script language="javascript" type="text/javascript">
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

    </script>
</asp:Content>
