<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Title="Collection Report" CodeFile="S3GRptCollection.aspx.cs" Inherits="Reports_S3GRptCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>

<asp:Content ID="ContentCollection" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
 <tr>  
 <td> 
     <table width="100%" border="0" cellpadding="0" cellspacing="2px">
        <tr>
            <td class="stylePageHeading" colspan="4">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Collection Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
 </td>
 </tr>
 <tr>
 <td>
     <table width="100%" border="0">
      <tr>
            <td width="50%">
             <asp:Panel ID="pnlInput" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                       <tr>
                         <td class="styleFieldLabel" width="20%" align="left">
                                <asp:Label runat="server" Text="Line Of Business" ToolTip="Line of Business" ID="LblLOB" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="35%" align="left">
                                <asp:DropDownList ID="DdlLOB" ToolTip="Line of Business" runat="server" Height="23px" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="DdlLOB_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFVLOB" runat="server" ErrorMessage="Select a Line Of Business." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="DdlLOB" InitialValue="-1">
                                </asp:RequiredFieldValidator> 
                            </td>
                            <td class="styleFieldLabel" width="20%" align="left">
                                <asp:Label runat="server" Text="Location 1" ToolTip="Location 1" ID="lblLocation1" CssClass="styleMandatoryLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="35%" align="left">
                                <asp:DropDownList ID="ddlLocation1" runat="server" AutoPostBack="true" 
                                    Height="23px" Width="63%" ToolTip="Location 1" 
                                    OnSelectedIndexChanged="ddlLocation1_SelectedIndexChanged">
                                </asp:DropDownList>
                                <%--<asp:RequiredFieldValidator ID="rfvddlHierarchy" runat="server" ErrorMessage="Select Location1." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlLocation1" InitialValue="-1">
                                </asp:RequiredFieldValidator> --%>
                            </td>
                            
                            
                        </tr>
                         <tr>
                            <td height="5px">
                            </td>
                        </tr>
                        <tr>
                           <td class="styleFieldLabel" width="20%" align="left">
                                <asp:Label runat="server" Text="Location 2" ToolTip="Location 2" ID="LblLocation2" CssClass="styleMandatoryLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="35%" align="left">
                                <asp:DropDownList ID="DdlLocation2" ToolTip="Location 2" runat="server" Height="23px" Width="60%">
                                </asp:DropDownList>
                               <%-- <asp:RequiredFieldValidator ID="RfvddlHierarchyName" runat="server" ErrorMessage="Select Location2." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="DdlLocation2" InitialValue="-1">
                                </asp:RequiredFieldValidator>--%>
 
                            </td>
                            
                             <td class="styleFieldLabel" width="20%" align="left">
                                <asp:Label runat="server" Text="Account Level" ToolTip="Account Level" ID="LblAccountLevel" CssClass="styleMandatoryLabel">
                                </asp:Label>
                            </td>
                            
                            <td class="styleFieldAlign" align="left" width="35%">
                            <asp:CheckBox ID="chkAccountLevel" runat="server" ToolTip="Account Level" />
                            </td>
                          </tr>  
                         <tr>
                            <td height="5px">
                            </td>
                        </tr>
                         <tr>
                        <%--From date--%>
                        <td width="20%" class="styleFieldLabel" align="left">
                            <asp:Label runat="server" Text="From Date" ID="lblFromDateSearch" ToolTip="From Date" CssClass="styleReqFieldLabel" />
                        </td>
                        <td align="left" width="35%" class="styleFieldAlign">
                            <asp:TextBox ID="txtFromDateSearch" runat="server" Width="100px" 
                                AutoPostBack="true" Height="19px" ToolTip="From Date" 
                                ontextchanged="txtFromDateSearch_TextChanged"></asp:TextBox>
                            <asp:Image ID="imgFromDateSearch" ToolTip="From Date" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            <cc1:CalendarExtender ID="CalendarExtenderFromDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgFromDateSearch" TargetControlID="txtFromDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RFVFromDate" ValidationGroup="Ok" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="txtFromDateSearch" SetFocusOnError="True"
                                ErrorMessage="Select From Date" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <%--To Date--%>
                        <td width="20%" class="styleFieldLabel" align="left">
                            <asp:Label runat="server" ID="lblToDateSearch" Text="To Date" ToolTip="To Date" CssClass="styleReqFieldLabel" />
                        </td>
                        <td align="left" width="35%" class="styleFieldAlign">
                            <asp:TextBox ID="txtToDateSearch" runat="server" Width="100px" 
                                AutoPostBack="true" Height="19px" ToolTip="To Date" 
                                ontextchanged="txtToDateSearch_TextChanged"></asp:TextBox>
                            <asp:Image ID="imgToDateSearch" runat="server" ToolTip="To Date" ImageUrl="~/Images/calendaer.gif" />
                           <cc1:CalendarExtender ID="CalendarExtenderToDateSearch" runat="server" Enabled="True"
                                PopupButtonID="imgToDateSearch" TargetControlID="txtToDateSearch" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                            </cc1:CalendarExtender>                           
                            <asp:RequiredFieldValidator ID="RFVToDate" ValidationGroup="Ok" CssClass="styleMandatoryLabel"
                                runat="server" ControlToValidate="txtToDateSearch" SetFocusOnError="True" ErrorMessage="Select To Date"
                                Display="None"></asp:RequiredFieldValidator>
                        </td>
                        </tr>
                         <tr>
                            <td height="5px">
                            </td>
                        </tr>
                        <tr>
                         <td class="styleFieldLabel" width="20%" align="left">
                                <asp:Label runat="server" Text="Denomination" ToolTip="Denomination" ID="LblDenomination" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="35%" align="left">
                                <asp:DropDownList ID="DdlDenomination" runat="server" ToolTip="Denomination" Height="23px" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="DdlDenomination_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RfvDenomination" runat="server" ErrorMessage="Select a Denomination." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="DdlDenomination" InitialValue="-1">
                                </asp:RequiredFieldValidator> 
                            </td>
                        </tr>
                      </table>
              </asp:Panel>
           </td>
      </tr>
     </table>  
 </td>
 </tr> 
 <tr> 
 <td>
     <table width="100%" border="0">   
     <tr align="center">
     <td align="center">
     <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" 
             ValidationGroup="Ok" OnClientClick="return fnCheckPageValidators('Ok',false);" CausesValidation="true" Visible="true" ToolTip="Go" 
             onclick="btnGo_Click" />
     <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" Text="Clear" 
             Visible="true" OnClientClick="return fnConfirmClear();" ToolTip="Clear" 
             onclick="btnClear_Click" />
     </td>
     </tr>
      <tr align="right">
     <td align="right">
     <asp:Label ID="lblCurrency" runat="server" ToolTip="Currency"></asp:Label>
     </td>
     </tr>
     </table>
 </td>
 </tr>
 <tr>
 <td>
     <table width="100%">
      <tr>   
            <td class="styleFieldAlign">
                <asp:Panel ID="PnlCollectionDetails" runat="server" GroupingText="Collection Details" CssClass="stylePanel" Width="100%">
                 <div id="divCollection" runat="server" style="height:200px; overflow:scroll">
                    <asp:GridView ID="grvCollection" runat="server" 
                        Width="100%" AutoGenerateColumns="false" CssClass="styleInfoLabel" Style="margin-bottom: 0px" 
                        ShowFooter="true" HeaderStyle-CssClass="Freezing">
                        <Columns>
                         <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("Location") %>' ToolTip="Location"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotal" ToolTip="Total" runat="server" Text="Total" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                <FooterStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="CustomerName" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerName" ToolTip="Customer Name" runat="server" Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                               <%-- <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Total" align="center"></asp:Label>
                                    </FooterTemplate>--%>
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                                <%--<FooterStyle HorizontalAlign="center" />--%>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Account No." 
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrimeAccountNo" runat="server" ToolTip="Account No." 
                                        Text='<%# Bind("PrimeAccountNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="Sub Account No." ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubAccountNo" runat="server" ToolTip="Sub Account No." 
                                        Text='<%# Bind("SubAccountNumber") %>'></asp:Label>
                                </ItemTemplate>                               
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Receipt Number" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceiptNumber" runat="server" ToolTip="Receipt No." Text='<%# Bind("ReceiptNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Receipt Date" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceiptDate" ToolTip="Receipt Date" runat="server" Text='<%# Bind("ReceiptDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="10%" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Receipt Amount" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblReceiptAmount" runat="server" Text='<%# Bind("ReceiptAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalReceiptAmount" ToolTip="Receipt Amount" runat="server" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="left" Width="10%" />
                                 <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>
                         <asp:TemplateField HeaderText="Total Collection Amount" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalCollection" ToolTip="Total Collection Amount" runat="server" Text='<%# Bind("TotalCollectionAmount") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalCollection" ToolTip="Total Collection Amount" runat="server" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Current Collection" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrentCollection" ToolTip="Current Collection" runat="server" Text='<%# Bind("CurrentCollection") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalCurrentCollection" ToolTip="Current Collection" runat="server" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Arrears Collection" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblArrearsCollection" runat="server" ToolTip="Arrears Collection" Text='<%# Bind("ArrearsCollection") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalArrearsCollection" runat="server" ToolTip="Arrears Collection" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="Insurance" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsurance" ToolTip="Insurance" runat="server" Text='<%# Bind("Insurance") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalInsurance" ToolTip="Total Insurance Amount" runat="server" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>   
                             <asp:TemplateField HeaderText="Interest" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblInterest" runat="server" 
                                        Text='<%# Bind("Interest") %>' ToolTip="Interest"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalInterest" runat="server" ToolTip="Total Interest Amount" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField> 
                                  <asp:TemplateField HeaderText="ODI" 
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblODI" runat="server" ToolTip="ODI." 
                                        Text='<%# Bind("OverDueInterest") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalODI" runat="server" ToolTip="Total ODI Amount" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Memo Charges" 
                                ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblMemoCharges" runat="server" 
                                        Text='<%# Bind("MemoCharges") %>' ToolTip="Memo Charges"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalMemoCharges" runat="server" ToolTip="Total Memo Charges" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="Others" ItemStyle-HorizontalAlign="center">
                                <ItemTemplate>
                                    <asp:Label ID="lblOthers" runat="server" ToolTip="Others" 
                                        Text='<%# Bind("Others") %>'></asp:Label>
                                </ItemTemplate> 
                                <FooterTemplate>
                                        <asp:Label ID="lblTotalOthers" runat="server" align="center" ToolTip="Total Other Charges"></asp:Label>
                                    </FooterTemplate>                              
                                <ItemStyle HorizontalAlign="right" Width="10%" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>                                               
                        </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>
            </td>    
     </tr>
      <tr align="center">
     <td align="center">
     <asp:Button ID="btnPrint" ToolTip="Print" Text="Print" Enabled="true" runat="server" 
             CssClass="styleSubmitButton" onclick="btnPrint_Click" />
     </tr>
     </table>
 </td>
 </tr>
 <tr>
 <td>
     <table>
     <tr>
     <td width="50%" align="center">
        <asp:ValidationSummary ValidationGroup="Ok" ID="vsCollection" runat="server"
        CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):"
        ShowSummary="true" />     
     </td>   
     </tr>     
     </table> 
 </td>
 </tr>
</table>
<%--<script language="javascript" type="text/javascript">
 function Resize()
     {
       if(document.getElementById('ctl00_ContentPlaceHolder1_divCollection') != null)
       {
         if(document.getElementById('divMenu').style.display=='none')
            {
             (document.getElementById('ctl00_ContentPlaceHolder1_divCollection')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
         else
           {
             (document.getElementById('ctl00_ContentPlaceHolder1_divCollection')).style.width = screen.width - 270;
           }
        }  
      }
    </script>   --%>
</asp:Content>