<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptBranchPerformance.aspx.cs" Inherits="Reports_S3GRptBranchPerformance"Title="BranchPerformance"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
 <tr>
            <td class="stylePageHeading">
                <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Branch Performance">
                </asp:Label>
            </td>
        </tr>
 <tr>
            <td class="styleFieldAlign">
            <asp:Panel runat="server" ID="PnlHeaderDetails" CssClass="stylePanel" GroupingText="Input Criteria"
                    Width="100%">
                    <table width="100%">
                    <tr>
                    <td width="25%" class="styleFieldAlign">
                                <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleDisplayLabel" ToolTip="Line of Business"></asp:Label>
                                <span class="styleMandatory">*</span>
                               
                     </td>
                    <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlLOB" runat="server" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" AutoPostBack="True" ToolTip="Line of Business"> 
                                   
                                </asp:DropDownList>
                                   <asp:RequiredFieldValidator ID="rfvLOB" runat="server" Display="None" ControlToValidate="ddlLOB"
                                   InitialValue ="-1"  ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select Line Of Business"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                    <td class="styleFieldAlign" width="25%">
                                <asp:Label ID="lblregion" runat="server" Text="Location1" CssClass="styleDisplayLabel" ToolTip="Location1"></asp:Label>
                            </td>
                    <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                     onselectedindexchanged="ddlRegion_SelectedIndexChanged" ToolTip="Location1" >
                                </asp:DropDownList>
                            </td>
                    </tr>
                    <tr>
                    <td class="styleFieldAlign" width="25%">
                                <asp:Label ID="lblbranch" runat="server" Text="Location2" CssClass="styleDisplayLabel" ToolTip="Location2"></asp:Label>
                            </td>
                    <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlbranch" runat="server" AutoPostBack="True" onselectedindexchanged="ddlbranch_SelectedIndexChanged"  ValidationGroup="Header"  ToolTip="Location2">
                           
                                </asp:DropDownList>
                            </td>
                    <td class="styleFieldAlign">
                    <asp:Label ID="lblCutOffMonth" runat="server" CssClass="styleDisplayLabel" Text="Cutoff Month" ToolTip="Cut Off Month" />
                    <span class="styleMandatory">*</span>
                    </td>
                   <%-- <td class="styleFieldAlign">
                                      <asp:TextBox ID="txtCutoffMonthSearch" runat="server" Width="100" MaxLength="6"  
                                          AutoPostBack="true" ontextchanged="txtCutoffMonthSearch_TextChanged"></asp:TextBox>
                                           <asp:RequiredFieldValidator ID="rfvCutOffMonth" runat="server" Display="None" ControlToValidate="txtCutoffMonthSearch"
                                    ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Enter The CutOff Month"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                   
                            </td>--%>
                     <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtCutoffMonthSearch" runat="server" OnTextChanged="txtCutoffMonthSearch_OnTextChanged" AutoPostBack="true" Width="60%" ToolTip="Cut Off Month"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server"   PopupButtonID="imgStartMonth" BehaviorID="calendar1" Format="yyyyMM" TargetControlID="txtCutoffMonthSearch" OnClientShown="onCalendarShown">
                                </cc1:CalendarExtender>
                                <asp:Image ID="imgStartMonth" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <asp:RequiredFieldValidator ID="rfvcutoffmonth" runat="server" ErrorMessage="Select Cutoff Month" ValidationGroup="btnGo" Display="None" SetFocusOnError="True" ControlToValidate="txtCutoffMonthSearch"></asp:RequiredFieldValidator>
                           </td>
                                      
                    </tr>
                    <tr>
                      <td width="25%" class="styleFieldAlign">
                                <asp:Label ID="lblDenomination" runat="server" CssClass="styleDisplayLabel"  Text="Denomination"></asp:Label>
                                
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlDenomination" runat="server" AutoPostBack="True" onselectedindexchanged="ddlDenomination_SelectedIndexChanged" ValidationGroup="Header">
                                </asp:DropDownList>
                            </td>
                    </tr>
                     
                    </table>
                    </asp:Panel>
            </td>
            </tr>
 <tr>
            <td align="center" colspan="3">
                <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" 
                    ValidationGroup="btnGo" OnClientClick="return fnCheckPageValidators('btnGo',false);" OnClick="btnGo_Click" ToolTip="Go"/>&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnClear" runat="server" CssClass="styleSubmitButton" 
                    Text="Clear" OnClientClick="return fnConfirmClear();" onclick="btnClear_Click" ToolTip="Clear" />
            </td>
        </tr>
 <tr>
        <td align="right">
        <asp:Label ID="lblAmounts" runat="server" Text="" Visible=false />
         </td>
        </tr>
 <tr>
  <td valign="top" >
     <asp:Panel runat="server" ID="pnlpayments"  CssClass="stylePanel" GroupingText="Payment Details"
                    Width="100%">
        <table width="100%">
          
           <tr>
          <td>
            <asp:Panel runat="server" ID="pnlpayment" CssClass="stylePanel" GroupingText="Payment"
                    Width="100%">
                     <asp:GridView ID="grvpayment" runat="server" AutoGenerateColumns="False" 
                      BorderWidth="2"  Width="100%" 
                         >
                         <Columns>
                             <asp:TemplateField HeaderText="Location Code" ItemStyle-HorizontalAlign="Left" Visible="false">
                                 <ItemTemplate>
                                     <asp:Label ID="lblregion" runat="server" Text='<%#Eval("Region")%>' ToolTip="Region"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location Name" ItemStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                     <asp:Label ID="lblbranch" runat="server" Text='<%#Eval("Branch")%>' ToolTip="Branch"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Right">
                                 <ItemTemplate>
                                     <asp:Label ID="lblGYTM" runat="server" Text='<%#Eval("AllAssetsMonth")%>' ToolTip="Month"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             
                             <asp:TemplateField HeaderText="YTM" ItemStyle-HorizontalAlign="Right">
                                 <ItemTemplate>
                                     <asp:Label ID="lblGYTM" runat="server" Text='<%#Eval("AllAssetsYTM")%>' ToolTip="Year To Month"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                         </Columns>
                     </asp:GridView>
                    </asp:Panel>
             </td>
          </tr>
          <tr>
          <td >
            <asp:Panel runat="server" ID="pnlclass" CssClass="stylePanel" GroupingText="In Units"
                    Width="100%">
                    <div id="divasset" runat="server" style="overflow: scroll; height: 150px;  display:block " >
                     <asp:GridView ID="grvUnits" runat="server" AutoGenerateColumns="False" 
                      BorderWidth="2" Width="100%">
                         <Columns>
                         <asp:TemplateField HeaderText="Location Code" ItemStyle-HorizontalAlign="Left" Visible="false">
                                 <ItemTemplate>
                                     <asp:Label ID="lblregion" runat="server" Text='<%#Eval("Region")%>' ToolTip="Region"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location Name" ItemStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                     <asp:Label ID="lblbranch" runat="server" Text='<%#Eval("Branch")%>' ToolTip="Branch"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Asset Class" ItemStyle-HorizontalAlign="Left" FooterStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                     <asp:Label ID="lblAssetClass" runat="server" Text='<%#Eval("AssetClass")%>' ToolTip="Asset Class" ></asp:Label>
                                 </ItemTemplate>
                                <%-- <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server"  Text="Total"  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>--%>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Month" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                 <ItemTemplate>
                                     <asp:Label ID="lblGMonth" runat="server" Text='<%#Eval("AssetClassMonth")%>' ToolTip="Month"></asp:Label>
                                 </ItemTemplate>
                            <%--      <FooterTemplate>
                                        <asp:Label ID="lblTotalassetmonth" runat="server" Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>--%>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="YTM" ItemStyle-HorizontalAlign="Right"  FooterStyle-HorizontalAlign="Right">
                                 <ItemTemplate>
                                     <asp:Label ID="lblGYTM" runat="server" Text='<%#Eval("AssetClassYTM")%>' ToolTip="Year To Month"></asp:Label>
                                 </ItemTemplate>
                              <%--    <FooterTemplate>
                                        <asp:Label ID="lblTotalassetytm" runat="server" Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>--%>
                             </asp:TemplateField>
                         </Columns>
                     </asp:GridView>
                     </div>
                    </asp:Panel>
             </td>
          </tr>
       </table>
      </asp:Panel>
        
  </td>
  </tr>  
  <tr>
  <td >
  
  <asp:Panel runat="server" ID="PnlCollection" CssClass="stylePanel" GroupingText="Collection Details"
                    Width="100%" Height="100%">
   
  <asp:GridView ID="grvcollections" runat="server"  AutoGenerateColumns="False" 
                               BorderWidth="2" Width="100%">
                   <Columns>
                   <asp:TemplateField HeaderText="Location Code" ItemStyle-HorizontalAlign="Left" Visible="false">
                                 <ItemTemplate>
                                     <asp:Label ID="lblregion" runat="server" Text='<%#Eval("Region")%>' ToolTip="Region"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location Name" ItemStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                     <asp:Label ID="lblbranch" runat="server" Text='<%#Eval("Branch")%>' ToolTip="Branch"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                       <asp:TemplateField HeaderText="Year" FooterStyle-HorizontalAlign="Left">
                           <ItemTemplate>
                               <asp:Label ID="lblyear" runat="server" Text='<%# Bind("Year") %>' ToolTip="Year"></asp:Label>
                           </ItemTemplate>
                           <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server"  Text="Total"  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Left" Width="10%" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Current %" FooterStyle-HorizontalAlign="Right">
                           <ItemTemplate>
                               <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("CurrentCollection") %>' ToolTip="Current%"></asp:Label>
                           </ItemTemplate>
                        <%--   <FooterTemplate>
                                        <asp:Label ID="lblTotalCurrentCollection" runat="server" Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>--%>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Arrear %" FooterStyle-HorizontalAlign="Right">
                           <ItemTemplate>
                               <asp:Label ID="lblArrear" runat="server" Text='<%# Bind("ArrearCollection") %>' ToolTip="Arrear%"></asp:Label>
                           </ItemTemplate>
                        <%--   <FooterTemplate>
                                        <asp:Label ID="lblTotalArrearCollection" runat="server" Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                           <HeaderStyle HorizontalAlign="Center" />--%>
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Total %" FooterStyle-HorizontalAlign="Right">
                           <ItemTemplate>
                               <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("TotalCollection") %>' ToolTip="Total%"></asp:Label>
                           </ItemTemplate>
                       <%--    <FooterTemplate>
                                        <asp:Label ID="lblTotalCollection" runat="server" Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>--%>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                   </Columns>
               </asp:GridView>
       
  
   </asp:Panel>
   </td>
   </tr>
   <tr>
   <td>
    <asp:Panel runat="server" ID="pnlcumulativecollection" CssClass="stylePanel" GroupingText="Cumulative Collection"
                    Width="100%">
  <%--<div style="height: 250px; overflow:scroll;">--%>
  
  <asp:GridView ID="grvcumulativecollection" runat="server" AutoGenerateColumns="False" 
                   BorderWidth="2" Width="100%">
                   <Columns>
                   <asp:TemplateField HeaderText="Location Code" ItemStyle-HorizontalAlign="Left" Visible="false">
                                 <ItemTemplate>
                                     <asp:Label ID="lblregion" runat="server" Text='<%#Eval("Region")%>' ToolTip="Region"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location Name" ItemStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                     <asp:Label ID="lblbranch" runat="server" Text='<%#Eval("Branch")%>' ToolTip="Branch"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                       <asp:TemplateField HeaderText="Cumulative Collection">
                           <ItemTemplate>
                              <asp:Label ID="lblcumulativecollection" runat="server" Text='<%#Eval("CumulativeCollection")%>' ToolTip="Cumulative collection"></asp:Label>
                           </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                       
                            </Columns>
               </asp:GridView>
 
<%--  </div> --%>                
  </asp:Panel>
   </td>
   </tr>
  
   <tr>
  
   <td>     
   <asp:Panel runat="server" ID="pnlstock" CssClass="stylePanel" GroupingText="Stock Details"
                    Width="100%">
    <%--<div style="height: 250px; overflow:scroll;">--%>
  
  <asp:GridView ID="grvstock" runat="server" AutoGenerateColumns="False" 
                   BorderWidth="2" Width="100%">
                   <Columns>
                   <asp:TemplateField HeaderText="Location Code" ItemStyle-HorizontalAlign="Left" Visible="false">
                                 <ItemTemplate>
                                     <asp:Label ID="lblregion" runat="server" Text='<%#Eval("Region")%>' ToolTip="Region"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location Name" ItemStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                     <asp:Label ID="lblbranch" runat="server" Text='<%#Eval("Branch")%>' ToolTip="Branch"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                       <asp:TemplateField HeaderText="Stock on Hire">
                           <ItemTemplate>
                              <asp:Label ID="lblstockonhire" runat="server" Text='<%#Eval("StockOnHire")%>' ToolTip="Stock on Hire"></asp:Label>
                           </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Arrears as on Cutoff Month">
                           <ItemTemplate>
                               <asp:Label ID="lblarrearasoncutoffmonth" runat="server" Text='<%#Eval("Arrears")%>' ToolTip="Arrears as on cut off month%"></asp:Label>
                           </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Arrear/Stock %">
                           <ItemTemplate>
                               <asp:Label ID="lblarrearstock" runat="server" Text='<%#Eval("ArrearsStock")%>' ToolTip="Arrear/stock%"></asp:Label>
                           </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                            </Columns>
               </asp:GridView>
 
  <%--</div> --%>                
  </asp:Panel>
  </td>
  </tr>
  <tr>
  <td>
   <asp:Panel runat="server" ID="pnlnoofaccounts" CssClass="stylePanel" GroupingText="Total Accounts"
                    Width="100%">
 <%--   <div style="height: 250px; overflow:scroll;">--%>
  
  <asp:GridView ID="grvnoofaccounts" runat="server" AutoGenerateColumns="False" 
                   BorderWidth="2" Width="100%">
                   <Columns>
                   <asp:TemplateField HeaderText="Location Code" ItemStyle-HorizontalAlign="Left" Visible="false">
                                 <ItemTemplate>
                                     <asp:Label ID="lblregion" runat="server" Text='<%#Eval("Region")%>' ToolTip="Region"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location Name" ItemStyle-HorizontalAlign="Left">
                                 <ItemTemplate>
                                     <asp:Label ID="lblbranch" runat="server" Text='<%#Eval("Branch")%>' ToolTip="Branch"></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                       <asp:TemplateField HeaderText="Total Accounts">
                           <ItemTemplate>
                              <asp:Label ID="lbltotalaccounts" runat="server" Text='<%#Eval("Accounts")%>' ToolTip="Total Accounts"></asp:Label>
                           </ItemTemplate>
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Right" Width="10%" />
                       </asp:TemplateField>
                       
                            </Columns>
               </asp:GridView>
 <%--
  </div> --%>                
  </asp:Panel>
  </td>
  </tr>
  
  <tr>
  <td >
    <asp:Panel runat="server" ID="pnlAccount" CssClass="stylePanel" GroupingText="NPA Account Details">
    <div id="divNpa" runat="server" style="overflow: scroll; height: 200px;  display:block " >
        <asp:GridView ID="grvNPAAccount" runat="server" AutoGenerateColumns="False" 
                             ShowFooter="true" BorderWidth="2" Width="100%">
            <Columns>
            <asp:TemplateField HeaderText="Location Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblregion" runat="server" Text='<%# Bind("Region") %>'></asp:Label>
                                         <asp:Label ID="lblregionid" runat="server" Visible="false" Text='<%# Bind("RegionId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left" />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Location Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                                         <asp:Label ID="lblBranchId" runat="server" Visible="false" Text='<%# Bind("BranchId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left"  />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Class">
                    <ItemTemplate>
                        <asp:Label ID="lblAsset" runat="server"  Text='<%# Bind("AssetClass") %>' ToolTip="Asset Class"></asp:Label>
                        <asp:Label ID="lblclassid"  runat="server" Visible="false"  Text='<%# Bind("ClassId") %>' ToolTip="Asset Class"></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server"  Text="Total"  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                     <FooterStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Opening NPA">
                    
                    <ItemTemplate>
                <asp:LinkButton ID="LnkBtnopening" runat="server"  OnClick="LnkBtnopening_Click" Text='<%# Bind("OpeningNoOfAccounts") %>' ToolTip="Opening Account"></asp:LinkButton>
                       </ItemTemplate>
                        <FooterTemplate>
                        <asp:LinkButton ID="LnkTotalBtnopening" runat="server"  OnClick="LnkTotalBtnopening_Click" Text="" ToolTip="Opening Account"></asp:LinkButton>
                                        <%--<asp:Label ID="lblTotalopeningacct" runat="server"  Text=""  ToolTip="Total"></asp:Label>--%>
                                    </FooterTemplate>
                                     <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                       </asp:TemplateField> 
                       <asp:TemplateField HeaderText="Stock">
                <ItemTemplate>
                         <asp:Label ID="lblStockopening" runat="server" Text='<%# Bind("OpeningStock") %>' ToolTip="Opening Stock"></asp:Label>
                      </ItemTemplate>
                       <FooterTemplate>
                                        <asp:Label ID="lblTotalopeningstock" runat="server"  Text=""  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                      </asp:TemplateField> 
                      <asp:TemplateField HeaderText="Arrears">
                   <ItemTemplate>
                        <asp:Label ID="lblArrearopening" runat="server" Text='<%# Bind("OpeningArrear") %>' ToolTip="Opening Arrear"></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                                        <asp:Label ID="lblTotalopeningarrear" runat="server" Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Additions during the month" >
                    <ItemTemplate>
                        <asp:LinkButton ID="LnkBtnAddition" runat="server" OnClick="LnkBtnAddition_Click" Text='<%# Bind("AdditionNoOfAccounts") %>' ToolTip="Addition Account"></asp:LinkButton>
                       </ItemTemplate>
                        <FooterTemplate>
                         <asp:LinkButton ID="LnkBtnTotalAddition" runat="server" OnClick="LnkBtnTotalAddition_Click" Text="" ToolTip="Addition Account"></asp:LinkButton>
                                        <%--<asp:Label ID="lblTotalAdditionacct" runat="server"  Text="" ToolTip="Total"></asp:Label>--%>
                                    </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                       </asp:TemplateField>
                       <asp:TemplateField HeaderText="Stock">
                        <ItemTemplate>
                       <asp:Label ID="lblStockaddition" runat="server" Text='<%# Bind("AdditionStock") %>' ToolTip="Addition Stock"></asp:Label>
                     </ItemTemplate>
                      <FooterTemplate>
                                        <asp:Label ID="lblTotaladditionstock" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Arrears">
                     <ItemTemplate>
                        <asp:Label ID="lblArrearaddition" runat="server" Text='<%# Bind("AdditionArrear") %>' ToolTip="Addition Arrear"></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                                        <asp:Label ID="lblTotaladditionarrear" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Deletions during the month">
                    <ItemTemplate>
                   
                       <asp:LinkButton ID="LnkBtnDeletion" runat="server" OnClick="LnkBtnDeletion_Click"  Text='<%# Bind("DeletionNoOfAccounts") %>' ToolTip="Deletion Account"></asp:LinkButton>
                    </ItemTemplate>
                     <FooterTemplate>
                                        <asp:LinkButton ID="LnkBtnTotalDeletion" runat="server" OnClick="LnkBtnTotalDeletion_Click"  Text="" ToolTip="Deletion Account"></asp:LinkButton>
                                        <%--<asp:Label ID="lblTotalDeletionacct" runat="server"  Text="" ToolTip="Total"></asp:Label>--%>
                                    </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Stock">
                  <ItemTemplate>
                   
                        <asp:Label ID="lblStockdeletion" runat="server" Text='<%# Bind("DeletionStock") %>' ToolTip="Deletion Stock"></asp:Label>
                   </ItemTemplate>
                     <FooterTemplate>
                                        <asp:Label ID="lblTotalDeletionstock" runat="server" Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                     </asp:TemplateField>
                       <asp:TemplateField HeaderText="Arrears">
                   <ItemTemplate>
                       <asp:Label ID="lblArreardeletion" runat="server" Text='<%# Bind("DeletionArrear") %>' ToolTip="Deletion Arrear"></asp:Label>
                     </ItemTemplate>
                       <FooterTemplate>
                    <asp:Label ID="lblTotalDeletionarrear" runat="server"  Text="" ToolTip="Total"></asp:Label>
                    </FooterTemplate>
                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Closing NPA during the month" ItemStyle-Width="400px" 
                                    HeaderStyle-Width="400px">
                    <ItemTemplate>
                     <asp:LinkButton ID="LnkBtnclosingact" runat="server" OnClick="LnkBtnclosingact_Click"  Text='<%# Bind("ClosingNoOfAccounts") %>' ToolTip="Closing Accounts"></asp:LinkButton>
                    </ItemTemplate>
                      <FooterTemplate>
                      <asp:LinkButton ID="LnkBtTotalclosingact" runat="server" OnClick="LnkBtTotalclosingact_Click"  Text="" ToolTip="Closing Accounts"></asp:LinkButton>
                                </FooterTemplate>
                                   <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stock">
                     <ItemTemplate>
                        <asp:Label ID="lblStockclosing" runat="server" Text='<%# Bind("ClosingStock") %>' ToolTip="Closing Stock"></asp:Label>
                     </ItemTemplate>
                       <FooterTemplate>
                         <asp:Label ID="lblTotalClosingstock" runat="server"  Text="" ToolTip="Total"></asp:Label>
                       </FooterTemplate>
                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Arrears">
                      <ItemTemplate>
                       <asp:Label ID="lblArrearclosing" runat="server" Text='<%# Bind("ClosingArrear") %>' ToolTip="Closing Arrear"></asp:Label>
                     </ItemTemplate>
                      <FooterTemplate>
                     <asp:Label ID="lblTotalClosingarrear" runat="server"  Text="" ToolTip="Total"></asp:Label>
               </FooterTemplate>
                                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="% of NPA Stock to Total Stock">
                    <ItemTemplate>
                        <asp:Label ID="lblstock" runat="server"  Text='<%# Bind("stock") %>' ToolTip="Stock"></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                                        <asp:Label ID="lblTotalstock" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                       <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="% of NPA Arrears to NPA Stock">
                    <ItemTemplate>
                        <asp:Label ID="lblarrears" runat="server"  Text='<%# Bind("Arrears") %>' ToolTip="Arrear"></asp:Label>
                    </ItemTemplate>
                     <FooterTemplate>
                                        <asp:Label ID="lblTotalarrears" runat="server" Text="" ToolTip="Total"></asp:Label>
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
  <td>
    <asp:Panel runat="server" ID="pnlOpeningaccountdetails" CssClass="stylePanel" GroupingText="NPA Opening Account Details"
                    Width="100%">
                    <div id="divopening" runat="server" style="overflow: scroll; height: 200px;  display:block " >
        <asp:GridView ID="grvOpeningaccountdetails" runat="server" AutoGenerateColumns="False"
                            CssClass="styleInfoLabel" ShowFooter="true" BorderWidth="2" Width="100%" 
                             >
            <Columns>
                        <asp:TemplateField HeaderText="Location Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblregion" runat="server" Text='<%# Bind("Region") %>'></asp:Label>
                                         <asp:Label ID="lblregionid" runat="server" Visible="false" Text='<%# Bind("RegionId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left" />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Location Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                                         <asp:Label ID="lblBranchId" runat="server" Visible="false" Text='<%# Bind("BranchId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left"  />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Class">
                    <ItemTemplate>
                        <asp:Label ID="lblAsset" runat="server"  Text='<%# Bind("AssetClass") %>' ToolTip="Asset Class"></asp:Label>
                        <asp:Label ID="lblclassid"  runat="server" Visible="false"  Text='<%# Bind("ClassId") %>' ToolTip="Asset Class"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblPrimeAcc" runat="server" 
                                   Text='<%# Bind("PrimeAccountNumber") %>' ToolTip="Account no"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblSubAcc" runat="server" Text='<%# Bind("SubAccountNumber") %>' ToolTip="Sub Account Number"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server"  Text="Total"  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                     <FooterStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stock">
                    <ItemTemplate>
                        <asp:Label ID="lblstocko" runat="server" Text='<%# Bind("Stock") %>' ToolTip="Stock"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalOpeningstock" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"  />
                     <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Arrears">
                    <ItemTemplate>
                        <asp:Label ID="lblArrearo" runat="server" Text='<%# Bind("Arrear") %>' ToolTip="Arrear"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalOpeningArrear" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"  />
                     <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
                    </asp:Panel>
  </td>
  </tr>
   <tr>
  <td>
    <asp:Panel runat="server" ID="pnlAddtionaccountdetails" CssClass="stylePanel" GroupingText="NPA Addition Account Details"
                    Width="100%">
                     <div id="divaddition" runat="server" style="overflow: scroll; height: 200px;  display:block " >
        <asp:GridView ID="grvAdditionAccountDetails" runat="server" AutoGenerateColumns="False" 
                            CssClass="styleInfoLabel" ShowFooter="true" BorderWidth="2" Width="100%">
            <Columns>
                        <asp:TemplateField HeaderText="Location Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblregion" runat="server" Text='<%# Bind("Region") %>'></asp:Label>
                                         <asp:Label ID="lblregionid" runat="server" Visible="false" Text='<%# Bind("RegionId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left"  />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Location Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                                         <asp:Label ID="lblBranchId" runat="server" Visible="false" Text='<%# Bind("BranchId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left"  />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Class">
                    <ItemTemplate>
                        <asp:Label ID="lblAsset" runat="server"  Text='<%# Bind("AssetClass") %>' ToolTip="Asset Class"></asp:Label>
                        <asp:Label ID="lblclassid"  runat="server" Visible="false"  Text='<%# Bind("ClassId") %>' ToolTip="Asset Class"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblPrimeAcc" runat="server" 
                                   Text='<%# Bind("PrimeAccountNumber") %>' ToolTip="Account No"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblSubAcc" runat="server" Text='<%# Bind("SubAccountNumber") %>' ToolTip="Sub Account Number"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server"  Text="Total"  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                     <FooterStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stock">
                    <ItemTemplate>
                        <asp:Label ID="lblstocka" runat="server" Text='<%# Bind("Stock") %>' ToolTip="Stock"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalAdditionstock" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"  />
                     <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Arrears">
                    <ItemTemplate>
                        <asp:Label ID="lblArreara" runat="server" Text='<%# Bind("Arrear") %>' ToolTip="Arrear"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalAdditionArrear" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
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
  <td>
    <asp:Panel runat="server" ID="pnlDeletionAccountDetails" CssClass="stylePanel" GroupingText="NPA Deletion Account Details"
                    Width="100%">
                     <div id="divdeletion" runat="server" style="overflow: scroll; height: 200px;  display:block " >
        <asp:GridView ID="grvDeletionAccountDetails" runat="server" AutoGenerateColumns="False" 
                            CssClass="styleInfoLabel" ShowFooter="true" BorderWidth="2" Width="100%">
            <Columns>
                        <asp:TemplateField HeaderText="Location Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblregion" runat="server" Text='<%# Bind("Region") %>'></asp:Label>
                                         <asp:Label ID="lblregionid" runat="server" Visible="false" Text='<%# Bind("RegionId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left" />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Location Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                                         <asp:Label ID="lblBranchId" runat="server" Visible="false" Text='<%# Bind("BranchId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left" />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Class">
                    <ItemTemplate>
                        <asp:Label ID="lblAsset" runat="server"  Text='<%# Bind("AssetClass") %>' ToolTip="Asset Class"></asp:Label>
                        <asp:Label ID="lblclassid"  runat="server" Visible="false"  Text='<%# Bind("ClassId") %>' ToolTip="Asset Class"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblPrimeAcc" runat="server" 
                                   Text='<%# Bind("PrimeAccountNumber") %>' ToolTip="Account No"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblSubAcc" runat="server" Text='<%# Bind("SubAccountNumber") %>' ToolTip="Sub Account Number"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server"  Text="Total"  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                     <FooterStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stock">
                    <ItemTemplate>
                        <asp:Label ID="lblstockd" runat="server" Text='<%# Bind("Stock") %>' ToolTip="Stock"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalDeletionstock" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"  />
                     <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Arrears">
                    <ItemTemplate>
                        <asp:Label ID="lblArreard" runat="server" Text='<%# Bind("Arrear") %>' ToolTip="Arrear"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalDeletionArrear" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
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
  <td>
    <asp:Panel runat="server" ID="pnlClosingAccountDetails" CssClass="stylePanel" GroupingText="NPA Closing Account Details"
                    Width="100%">
                     <div id="divclosing" runat="server" style="overflow: scroll; height: 200px;  display:block " >
        <asp:GridView ID="grvClosingAccountDetails" runat="server" AutoGenerateColumns="False" 
                            CssClass="styleInfoLabel" ShowFooter="true" BorderWidth="2" Width="100%">
            <Columns>
                        <asp:TemplateField HeaderText="Location Code" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblregion" runat="server" Text='<%# Bind("Region") %>'></asp:Label>
                                         <asp:Label ID="lblregionid" runat="server" Visible="false" Text='<%# Bind("RegionId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left"  />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Branch">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                                         <asp:Label ID="lblBranchId" runat="server" Visible="false" Text='<%# Bind("BranchId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left"  />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
            <asp:TemplateField HeaderText="Asset Class">
                    <ItemTemplate>
                        <asp:Label ID="lblAsset" runat="server"  Text='<%# Bind("AssetClass") %>' ToolTip="Asset Class"></asp:Label>
                        <asp:Label ID="lblclassid"  runat="server" Visible="false"  Text='<%# Bind("ClassId") %>' ToolTip="Asset Class"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblPrimeAcc" runat="server" 
                                   Text='<%# Bind("PrimeAccountNumber") %>' ToolTip="Account No"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sub Account Number">
                    <ItemTemplate>
                        <asp:Label ID="lblSubAcc" runat="server" Text='<%# Bind("SubAccountNumber") %>' ToolTip="Sub Account Number"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server"  Text="Total"  ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"  />
                     <FooterStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stock">
                    <ItemTemplate>
                        <asp:Label ID="lblstockc" runat="server" Text='<%# Bind("Stock") %>' ToolTip="Stock"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalClosingstock" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"  />
                     <FooterStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Arrears">
                    <ItemTemplate>
                        <asp:Label ID="lblArrearc" runat="server" Text='<%# Bind("Arrear") %>' ToolTip="Arrear"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                                        <asp:Label ID="lblTotalClosingarrear" runat="server"  Text="" ToolTip="Total"></asp:Label>
                                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Right"  />
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
             <asp:Button ID="btnView" runat="server" CssClass="styleSubmitButton" Text="View Chart" 
                  ToolTip="View Chart" OnClick="btnView_Click"/>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnPrint" CssClass="styleSubmitButton" runat="server" Text="Print" OnClick="btnPrint_Click" ToolTip="Print" />
            </td>
        </tr>
  
    <tr>
            <td align="center" colspan="4">
                <asp:ValidationSummary ID="vsBranch" runat="server" CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):"
                    ShowSummary="true" ValidationGroup="btnGo" />
            </td>
        </tr>
 </table>
 
  <script type="text/javascript"> 
 
        var cal1; 
 
       function pageLoad() { 
            cal1 = $find("calendar1"); 
            modifyCalDelegates(cal1); 
        } 
 
        function modifyCalDelegates(cal) { 
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
 
        } 
 
        function onCalendarShown(sender, args) { 
            //set the default mode to month 
            sender._switchMode("months", true); 
            changeCellHandlers(cal1); 
        } 
 
 
        function changeCellHandlers(cal) { 
 
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
        } 
 
        function onCalendarHidden(sender, args) { 
 
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
       
    function Resize()
     {
       if(document.getElementById('ctl00_ContentPlaceHolder1_pnlAccount') != null)
       {
         if(document.getElementById('divMenu').style.display=='none')
            {
             (document.getElementById('ctl00_ContentPlaceHolder1_pnlAccount')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
         else
           {
             (document.getElementById('ctl00_ContentPlaceHolder1_pnlAccount')).style.width = screen.width - 270;
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

            if(document.getElementById('ctl00_ContentPlaceHolder1_pnlAccount') != null)
            (document.getElementById('ctl00_ContentPlaceHolder1_pnlAccount')).style.width = screen.width - 270;
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
           
            if(document.getElementById('ctl00_ContentPlaceHolder1_pnlAccount') != null)
           (document.getElementById('ctl00_ContentPlaceHolder1_pnlAccount')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
        }
    }

 
 
    </script>
</asp:Content>
