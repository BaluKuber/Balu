<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3gRptEnquiryPerformance.aspx.cs" Inherits="Reports_S3gRptEnquiryPerformance" Title="Enquiry Performance Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="ENQUIRY PERFORMANCE REPORT">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
          </tr>
    <tr> 
           <td>
                <asp:Panel ID="Pnl1" runat="server"  CssClass="stylePanel" Width="100%" GroupingText="Input Criteria">
                <table width="100%">
                    <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddLOB"  runat="server"  ToolTip="Line of Business" 
                                AutoPostBack="true" onselectedindexchanged="ddLOB_SelectedIndexChanged" > </asp:DropDownList>
                               
                                <asp:RequiredFieldValidator ID="RFVLOB" CssClass="styleMandatoryLabel" runat="server"
                                ControlToValidate="ddLOB" InitialValue="-1" ValidationGroup="Go"
                                ErrorMessage="Select the Line of Business" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Location1" ID="lblregion"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="DDRegion"  runat="server"  ToolTip="Location1" 
                                AutoPostBack="True" onselectedindexchanged="DDRegion_SelectedIndexChanged" > </asp:DropDownList>
                            </td>
                    </tr>
                     <tr>
                     <td height="8px">
                     </td>
                     </tr>        
                    <tr>
                        <td class="styleFieldLabel" width="25%">
                            <asp:Label runat="server" Text="Location2" ID="lblBranch" AutoPostBack="True">
                            </asp:Label>
                        </td>
                        <td class="styleFieldAlign" width="25%">
                            <asp:DropDownList ID="ddlBranch" runat="server" ToolTip="Location2"  Enabled=false
                                onselectedindexchanged="ddlBranch_SelectedIndexChanged" >
                            </asp:DropDownList>
                        </td>
                         
                        <td class="styleFieldLabel" width="25%">
                              <asp:Label ID="lblProduct" runat="server" Text="Product" CssClass="StyleReqFieldLabel">
                              </asp:Label>
                         </td>
                         <td class="styleFieldAlign" width="25%">
                               <asp:DropDownList ID="ddlProduct" runat="server"  AutoPostBack="True"
                                   onselectedindexchanged="ddlProduct_SelectedIndexChanged" ToolTip="Product">
                               </asp:DropDownList>
                         </td>
                     </tr>
                     <tr>
                     <td height="8px">
                     </td>
                     </tr> 
                     <tr>
                         <td class="styleFieldLabel" width="25%">
                               <asp:Label runat="server" Text="Start Date" ID="lblStartDate" CssClass="styleReqFieldLabel">
                               </asp:Label>
                         </td>
                         <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtStartDate" runat="server" width="60%"  ToolTip="Start Date" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate"
                                OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgStartDate"
                                ID="CalendarExtender1">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Select Start Date"
                                ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                </td>
                         <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="End Date" ID="lblEndDate" CssClass="styleReqFieldLabel">
                                </asp:Label>
                         </td>
                         <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtEndDate" runat="server" width="40%" ToolTip="End Date" AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate"
                                 OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgEndDate"
                                 ID="CalendarExtender2">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Select End Date"
                                ValidationGroup="Go" Display="None" SetFocusOnError="True" ControlToValidate="txtEndDate">
                                </asp:RequiredFieldValidator>
                         </td>
                     </tr>
                     <tr>
                     <td height="8px">
                     </td>
                     </tr> 
                     <tr>
                        <td class="styleFieldLabel" width="25%">
                            <asp:Label runat="server" Text="Denomination" ID="Lblden" CssClass="styleReqFieldLabel">
                            </asp:Label>
                        </td>
                        <td class="styleFieldAlign" width="25%">
                            <asp:DropDownList ID="DropDenomination" runat="server" ToolTip="Denomination"  AutoPostBack="true"  onselectedindexchanged="DropDenomination_SelectedIndexChanged" >
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RFdeno" CssClass="styleMandatoryLabel" runat="server"
                            ControlToValidate="DropDenomination" InitialValue="0" ValidationGroup="Go"
                            ErrorMessage="Select the Denomination" Display="None">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                     <td height="8px">
                     </td>
                     </tr> 
                    </table>
                    </asp:Panel>
                    </td>
                    </tr>
                  
                  <tr>
                     <td height="8px">
                     </td>
                     </tr> 
                        <tr class="styleButtonArea" style="padding-left: 4px">
                            <td align="center" colspan="4">
                                <asp:Button ID="btnGo" runat="server"  ToolTip="Go" CssClass="styleSubmitButton" 
                                  onclick="btnGo_Click" Text="Go" ValidationGroup="Go"  />
                                &nbsp; &nbsp;
                                <asp:Button ID="btnClear" runat="server" ToolTip="Clear" CausesValidation="false" 
                                    CssClass="styleSubmitButton" Text="Clear"  OnClientClick="return fnConfirmClear();" onclick="btnClear_Click" />
                                    &nbsp; &nbsp;
                                
                            </td>
                        </tr>
                        <tr>
                 <td align="right" colspan="2" width="99%">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel">
                </asp:Label> 
                
                </td>
                 </tr>
                     <tr>
                    <td>
                        <asp:Panel ID="pnlEnquiry" runat="server" CssClass="stylePanel" GroupingText="Enquiry Details"
                            Width="100%" Visible="false">
                        <asp:GridView ID="grvEnquirydetails" runat="server" AutoGenerateColumns="False" 
                            CssClass="styleInfoLabel" ShowFooter="True" 
                           Visible="False" Width="100%">
                            <Columns>
                            <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Location") %>' ToolTip="Location"></asp:Label>
                                       <asp:Label ID="lblBranchId" runat="server" Visible="false" Text='<%# Bind("LocationId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left" Width ="10%" />
                                  <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                              <%--  <asp:TemplateField HeaderText="LocationID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBranchId" runat="server" Text='<%# Bind("LocationId") %>'></asp:Label>
                                   </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                               
                                </asp:TemplateField>--%>
                                
                                <asp:TemplateField HeaderText="Product">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("Product") %>' ToolTip="Product"></asp:Label>
                                        <asp:Label ID="lblProductId" runat="server" Visible="false" Text='<%# Bind("ProductId") %>'></asp:Label>
                                    </ItemTemplate>
                                     <ItemStyle  HorizontalAlign ="Left"  />
                                     <FooterStyle HorizontalAlign ="Center" />
                                     <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                <FooterTemplate>
                                   <asp:Label ID="lblTotal" runat="server" ToolTip="Grand Total" Text="Grand Total"></asp:Label>
                                </FooterTemplate>
                                    
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="ProductId" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductId" runat="server" Text='<%# Bind("ProductId") %>'></asp:Label>
                                    </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                              
                                <%-- <asp:TemplateField HeaderText="Received">
                        <HeaderTemplate>
                        <table width="100%" >
                          <tr>
                            <td colspan="2">
                                    Received
                                </td>
                            </tr>
                            <tr>
                                <td width="30%" >
                                    Count
                                </td>
                                <td  width="30%">
                                    Value
                                </td>
                                
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table width ="100%" >
                        <tr>
                    <td width="30%">
                        <asp:LinkButton ID="LnkBtnRC" runat="server" OnClick="ReceivedCountDetails" Text='<%# Bind("ReceivedCount") %>'></asp:LinkButton>
                        </td>
                        <td width="30%" >
                   <asp:Label ID="lblNoofAccounts4" runat="server" 
                                            Text='<%# Bind("ReceivedValue") %>'></asp:Label>
                        </td>
                   
                    </tr>
                    </table>
                    </ItemTemplate>
                    <ItemStyle  HorizontalAlign ="Right" Width ="5%" />
                    <FooterStyle HorizontalAlign ="Right" Width ="5%" />
                                 <FooterTemplate>
                                  <table width ="100%" cellpadding="5%">
                                    <tr>
                                <td width="30%" colspan="2">
                                   <asp:LinkButton ID="LnkBtnRCAll" runat="server"  OnClick ="AllReceivedDetails"/>
                                   </td>
                                   <td width="30%">
                                     <asp:Label ID="lblRecVal" runat="server"></asp:Label>
                                   </td>
                                   </tr>
                                   </table>
                                </FooterTemplate>
                                </asp:TemplateField>--%>
                               <asp:TemplateField HeaderText="Received Count">
                                   <ItemTemplate>
                                        <asp:LinkButton ID="LnkBtnRC" runat="server"  OnClick ="ReceivedCountDetails"
                                   Text='<%# Bind("ReceivedCount") %>' ToolTip="Received Count"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle  HorizontalAlign ="Right"  />
                                    <FooterStyle HorizontalAlign ="Right" />
                                     <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                 <FooterTemplate>
                                   <asp:LinkButton ID="LnkBtnRCAll" runat="server" ToolTip="Total Received Count" OnClick ="AllReceivedDetails"/>
                                </FooterTemplate>
                                 </asp:TemplateField>
                               
                               
                               
                                 <asp:TemplateField HeaderText="Received Value">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoofAccounts4"  runat="server" 
                                            Text='<%# Bind("ReceivedValue") %>' ToolTip="Received Value"></asp:Label>
                                    </ItemTemplate>
                                     <ItemStyle HorizontalAlign ="Right"  />
                                     <FooterStyle HorizontalAlign ="Right" />
                                      <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <FooterTemplate>
                                <asp:Label ID="lblRecVal" runat="server" ToolTip="Total Received Value"></asp:Label>
                                </FooterTemplate>
                                    
                                </asp:TemplateField>
                                
                                
                                <asp:TemplateField HeaderText="Successful Count">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkBtnSuC" runat="server"  OnClick="SuccessfulCountDetails" 
                                            Text='<%# Bind("SuccessfulCount") %>' ToolTip="Successful Count"></asp:LinkButton>
                                    </ItemTemplate>
                                     <ItemStyle HorizontalAlign ="Right"   />
                                     <FooterStyle HorizontalAlign ="Right"  />
                                      <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <FooterTemplate>
                                     <%--Text='<%# Bind("SuccessfulCount") %>'--%>
                                    <asp:LinkButton ID="LnkBtnSucAll" runat="server" ToolTip="Total Successful Count" OnClick="AllSuccessfulDetails">
                                    </asp:LinkButton>
                                 <%--     <asp:Label ID="lblSuccess" runat="server"></asp:Label>--%>
                                </FooterTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Successful Value">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoofAccounts5" runat="server" 
                                            Text='<%# Bind("SuccessfulValue") %>' ToolTip="Successful Value"></asp:Label>
                                    </ItemTemplate>
                                     <ItemStyle HorizontalAlign ="Right"  />
                                     <FooterStyle HorizontalAlign ="Right" />
                                      <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <FooterTemplate>
                                   <asp:Label ID="lblSuccessVal" runat="server" ToolTip="Total Successful Value"></asp:Label>
                                </FooterTemplate>
                                   
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Under Followup Count">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LnkBtnFuC" runat="server" OnClick="FollowupCountDetails" Text='<%# Bind("UnderFollowupCount") %>' ToolTip="Under FollowupCount"> 
                                        <%-- Text='<%# Bind("UnderFollowupCount") %>'--%>
                                            </asp:LinkButton>
                                    </ItemTemplate>
                                     <ItemStyle HorizontalAlign ="Right" />
                                    <FooterStyle HorizontalAlign ="Right" />
                                     <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <FooterTemplate>
                                    
                                 <asp:LinkButton ID="LnkBtnFucAll" runat="server" ToolTip="Total Under FollowupCount" OnClick="AllFollowupDetails">
                                 </asp:LinkButton>   
                                 <%--  <asp:Label ID="lblFollow" runat="server"></asp:Label>--%>
                                </FooterTemplate>
                                   
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Under Followup Value">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNoofAccounts2" runat="server"  
                                            Text='<%# Bind("UnderFollowupValue") %>' ToolTip="UnderFollowupValue"></asp:Label>
                                    </ItemTemplate>
                                     <ItemStyle HorizontalAlign ="Right"   />
                                    <FooterStyle HorizontalAlign ="Right" />
                                     <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <FooterTemplate>
                                   <asp:Label ID="lblFollowVal" runat="server" ToolTip="Total UnderFollowupValue"></asp:Label>
                                    </FooterTemplate>
                                   
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rejected Count">
                                    <ItemTemplate>
                                     <asp:LinkButton ID="LnkBtnREV" runat="server" OnClick="RejectedValueDetails" Text='<%# Bind("RejectedCount") %>' ToolTip="Rejected Count">
                                            </asp:LinkButton>
                                     <%-- Text='<%# Bind("RejectedCount") %>'--%>
                                        <%--<asp:Label ID="lblNoofAccounts3" runat="server" 
                                            Text='<%# Bind("RejectedCount") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                     <ItemStyle HorizontalAlign ="Right"  />
                                     <FooterStyle HorizontalAlign ="Right"  />
                                      <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <FooterTemplate>
                                    <asp:LinkButton ID="LnkBtnRejAll" runat="server" ToolTip="Total Rejected Count" OnClick="AllRejectedDetails">
                                    </asp:LinkButton>
                                  <%-- <asp:Label ID="lblReject" runat="server"></asp:Label>--%>
                                </FooterTemplate>
                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rejected Value" >
                                    <ItemTemplate>
                                     <asp:Label ID="lblNoofAccounts3" runat="server" 
                                            Text='<%# Bind("RejectedValue") %>' ToolTip="Rejected Value"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                   <asp:Label ID="lblRejVal" runat="server"  ToolTip="Total Rejected Value"></asp:Label>
                                </FooterTemplate>
                                   <ItemStyle HorizontalAlign ="Right"  />
                                   <FooterStyle HorizontalAlign ="Right"  />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                      </asp:Panel>
                    </td>
                    </tr>           
        <tr>
                     <td height="8px">
                     </td>
                     </tr> 
              
         <%--xxxxx--%>
         <tr>
         <td> 
         
         <asp:Panel ID="pnlRC" runat="server" CssClass="stylePanel" GroupingText="Received Details"
                            Width="100%" Visible="false">
                            <div style="overflow: auto; height: 250px">
                                <asp:GridView ID="grvRC" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                    Width="100%">
                                    <Columns>
                                          <%--<asp:TemplateField HeaderText="Product">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("Product") %>' ToolTip="Product"></asp:Label>
                                        <asp:Label ID="lblProductId" runat="server" Visible="false" Text='<%# Bind("ProductId") %>'></asp:Label>
                                    </ItemTemplate>
                                     <ItemStyle  HorizontalAlign ="Left"  />
                                     <FooterStyle HorizontalAlign ="Center" />
                                <FooterTemplate>
                                   <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                </FooterTemplate>
                                    
                                </asp:TemplateField>--%>
                                 <asp:TemplateField HeaderText="Product">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("Product") %>' ToolTip="Product"></asp:Label>
                                        <asp:Label ID="lblProductId" runat="server" Visible="false" Text='<%# Bind("ProductId") %>'></asp:Label>
                                    </ItemTemplate>
                                     <ItemStyle  HorizontalAlign ="Left"  />
                                     <FooterStyle HorizontalAlign ="Center" />
                                <%--<FooterTemplate>
                                   <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
                                </FooterTemplate>--%>
                                    
                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnq" runat="server" Text='<%# Bind("EnqNo") %>' ToolTip="Enquiry No"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEdate" runat="server" Text='<%# Bind("EnquiryDate") %>' ToolTip="Enquiry Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomername" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="Customer Name"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetDet" runat="server" Text='<%# Bind("AssetDetails") %>' ToolTip="Asset Details"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"   />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Facility Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFacility" runat="server" Text='<%# Bind("FacilityAmount") %>' ToolTip="Facility Amount"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' ToolTip="Status"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"   />
                                            </asp:TemplateField>
                                            </Columns>
                              </asp:GridView>
                            </div>
                        </asp:Panel> 
            </td>
         </tr>
         <tr>
                     <td height="8px">
                     </td>
                     </tr> 
        <%--Successful Count grid--%>
         <tr>
         <td> 
         <asp:Panel ID="PnlSc" runat="server" CssClass="stylePanel" GroupingText="Successful Details "
                            Width="100%" Visible="false">
                           <div style="overflow: auto; height: 200px">
                                <asp:GridView ID="GrvSc" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                    Width="99%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Enquiry No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnqNo" runat="server" Text='<%# Bind("EnqNo") %>' ToolTip="Enquiry No"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEnqDate" runat="server" Text='<%# Bind("EnquiryDate") %>' ToolTip="Enquiry Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustname" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="Customer Name"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssDet" runat="server" Text='<%# Bind("AssetDetails") %>' ToolTip="Asset Details"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Facility Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFacAmt" runat="server" Text='<%# Bind("FacilityAmount") %>' ToolTip="Facility Amount"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPac" runat="server" Text='<%# Bind("PrimeAccNo") %>' ToolTip="Account No"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubac" runat="server" Text='<%# Bind("SubAccNo") %>' ToolTip="Sub Account No"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                      </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel> 
            </td>
         </tr>
         <tr>
                     <td height="8px">
                     </td>
                     </tr> 
        <%--xxxxx--%> 
            <tr>
            <td>
          <asp:Panel ID="PnlUFC" runat="server" CssClass="stylePanel" GroupingText="FollowUp Details"
                            Width="100%" Visible="false">
                            <div style="overflow: auto; height: 200px">
                                <asp:GridView ID="GrvFollowUp" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Enquiry No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblENo" runat="server" Text='<%# Bind("EnqNo") %>' ToolTip="Enquiry No"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndate" runat="server" Text='<%# Bind("EnquiryDate") %>' ToolTip="Enquiry Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCuname" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="CustomerName"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetDet1" runat="server" Text='<%# Bind("AssetDetails") %>'  ToolTip="Asset Details"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Facility Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAM" runat="server" Text='<%# Bind("FacilityAmount") %>' ToolTip="Facility Amount"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stage">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Stage") %>' ToolTip="Stage"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                      </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel> 
                </td>
              </tr>   
              <tr>
                     <td height="8px">
                     </td>
                     </tr>      
             <%--Rejected Count grid--%>
            <tr>
               <td>
          <asp:Panel ID="PRejcount" runat="server" CssClass="stylePanel" GroupingText="Rejected Details"
                            Width="100%" Visible="false">
                            <div style="overflow: auto; height: 200px">
                                <asp:GridView ID="GrvRejected" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Enquiry No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblENo1" runat="server" Text='<%# Bind("EnqNo") %>' ToolTip="Enquiry No"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Enquiry Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndate1" runat="server" Text='<%# Bind("EnquiryDate") %>' ToolTip="Enquiry Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCuname1" runat="server" Text='<%# Bind("CustomerName") %>' ToolTip="Customer Name"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Details">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssetDe1" runat="server" Text='<%# Bind("AssetDetails") %>' ToolTip="Asset Details"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Facility Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFAM1" runat="server" Text='<%# Bind("FacilityAmount") %>' ToolTip="Facility Amount"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remarks") %>' ToolTip="Remarks"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center"  />
                                        </asp:TemplateField>
                                      </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel> 
                        <%--xxxxx--%>
                </td>
              </tr>     
              <tr class="styleButtonArea" style="padding-left: 4px">
              <td align="center">
               <asp:Button ID="btnPrint" runat="server" ValidationGroup="Print" ToolTip="Print" CssClass="styleSubmitButton" CausesValidation="false" Text="Print" onclick="btnPrint_Click" 
                                    Visible="False"/>   
      <%--   <asp:Button ID="BtnEmail" runat="server" Text="E-Mail" CssClass="styleSubmitButton" 
         ToolTip="To Send the Email" CausesValidation="False" onclick="BtnEmail_Click" Visible="False" /> --%>                    
              </td>
              </tr>
               <tr>
            <td>
                <asp:ValidationSummary ID="vsEnquiry" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVLAN" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>   
            </table>
 <%-- <table >
  <tr><td>--%>
  <%--<asp:Button runat="server" ID="btnpop" CssClass="styleSubmitButton" Text="Ok" Style="display: none" />--%>
 <%-- <cc1:ModalPopupExtender ID="PopupSelectUsersToMail" runat="server" TargetControlID="btnpop"
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
                                <asp:GridView ID="gvmail" runat="server" AutoGenerateColumns="false" Width ="100%" OnRowDataBound ="gvmail_RowDataBound">
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
                                        <asp:Label ID="Hlblmailid" runat="server" Text="MailID" />
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
                                         <asp:CheckBox ID="chkSelectAll" Checked="false" runat="server" ToolTip ="Select" />
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign ="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" Checked="false" runat="server" />
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
                                    Visible="true" />  &nbsp;
                                <asp:Button runat="server" ID="btnClosePopup" CssClass="styleSubmitButton" Text="Cancel"
                                    ToolTip="To canel" CausesValidation="false" OnClick="btnClosePopup_Click" Visible="true" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>--%>
    
   <%-- </td></tr>
    </table>--%>

    </asp:Content>

