<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Rpt_WDV.aspx.cs" Inherits="Reports_S3G_Rpt_WDV" Title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Lease Consolidation and WDV Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel runat="server" ID="pnlinputcriteria" CssClass="stylePanel" GroupingText="Input Criteria" Width="100%">
                    <table cellpadding="0" cellspacing="0" style="width: 100%" border="0">
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label ID="lblLOB" runat="server" Text="Line Of Business"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlLOB" runat="server" ToolTip="Line Of Business">
                                </asp:DropDownList>
                                
                            </td>
                              <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlDenomination" runat="server" ToolTip="Denomination">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RFdeno" runat="server" ControlToValidate="ddlDenomination" InitialValue="0" ValidationGroup="Go" ErrorMessage="Select the Denomination" Display="None">
                                </asp:RequiredFieldValidator>
                            </td>
                          
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                          <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location1" ID="lblregion"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlLocation1" runat="server" ToolTip="Location1" AutoPostBack="True" OnSelectedIndexChanged="ddlLocation1_SelectedIndexChanged" >
                                </asp:DropDownList>
                               
                            </td>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Location2" ID="lblBranch">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlLocation2" runat="server" ToolTip="Location2" Enabled="false" AutoPostBack="True">
                                   
                                </asp:DropDownList>
                            </td>
                          
                        
                        </tr>
                        <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                      
                             <td class="styleFieldLabel">
                                            <asp:Label ID="lblStartDateSearch" runat="server" CssClass="styleDisplayLabel" Text="Start Date" ToolTip="Start Date" />
                                            <span class="styleMandatory">*</span>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <input id="hidDate" runat="server" type="hidden" />
                                            <asp:TextBox ID="txtStartDateSearch" AutoPostBack="true" runat="server" Width="60%" ToolTip="Start Date" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" Display="None" ControlToValidate="txtStartDateSearch" ToolTip="Start Date"
                                                ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select Start Date"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch">
                                            </cc1:CalendarExtender>
                                        </td>
                                            <td class="styleFieldLabel">
                                            <asp:Label ID="lblEndDateSearch" runat="server" CssClass="styleDisplayLabel" Text="End Date" ToolTip="End Date" />
                                            <span class="styleMandatory">*</span>
                                        </td>
                                        <td class="styleFieldAlign">
                                            <asp:TextBox ID="txtEndDateSearch" AutoPostBack="true" runat="server" Width="60%" ToolTip="End Date"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" Display="None" ControlToValidate="txtEndDateSearch"
                                                ValidationGroup="btnGo" CssClass="styleMandatoryLabel" ErrorMessage="Select End Date"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:Image ID="imgEndDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate" 
                                                PopupButtonID="imgEndDateSearch" TargetControlID="txtEndDateSearch">
                                            </cc1:CalendarExtender>
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
            <tr align="center">
            <td>
                <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" OnClientClick="return fnCheckPageValidators('btnGo',false);" OnClick="btnGo_Click"
                    ValidationGroup="btnGo" />
                <asp:Button ID="btnclear" runat="server" CssClass="styleSubmitButton" Text="Clear" OnClick="btnclear_Click" OnClientClick="return fnConfirmClear();" 
                     />
            </td>
        </tr>
            <tr>
            <td class="styleFieldAlign">
                     <asp:Panel ID="pnlDetails" runat="server" CssClass="stylePanel" Visible="false" GroupingText="Lease Consolidation Details">
                    <div id="divdetails" runat="server" style="overflow: scroll; height: 200px; display:block" >
                        <asp:GridView ID="grvdetails" runat="server" AutoGenerateColumns="False"  BorderWidth="2"
                            CssClass="styleInfoLabel" Width="100%"   ShowFooter="true" ShowHeader="true" >
                            <Columns>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllOCATION" runat="server"  Text='<%# Bind("Location") %>' ToolTip="lOCATION"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomer" runat="server"  Text='<%# Bind("CUSTOMER_NAME") %>' ToolTip="Customer"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Asset Description" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAsset" runat="server"  Text='<%# Bind("ASSET_DESC") %>' ToolTip="Asset Description"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Lease Asset No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLAR_NO" runat="server"  Text='<%# Bind("LAR_NO") %>' ToolTip="Lease Asset Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Account No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpanum" runat="server"  Text='<%# Bind("PANUM") %>' ToolTip="Prime Account Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Sub Account No.">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsanum" runat="server"  Text='<%# Bind("SANUM") %>' ToolTip="Sub Account Number"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Account Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblaccountdate" runat="server"  Text='<%# Bind("ACCOUNT_DATE") %>' ToolTip="Account Date"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Maturity Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmaturitydate" runat="server"  Text='<%# Bind("Maturity_DATE") %>' ToolTip="Maturity Date"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Last Billed Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllastbilleddate" runat="server"  Text='<%# Bind("LASTBILLED_DATE") %>' ToolTip="Last Billed Date"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Grand Total" ToolTip="sum of Asset value"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Left" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Asset Cost w/o VAT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssetvalue" runat="server"  Text='<%# Bind("ASSET_VALUE1") %>' ToolTip="Last Billed Date"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblAssetvaluef" runat="server"  ToolTip="sum of Asset value"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                <%--     <asp:TemplateField HeaderText="Total VAT">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVAT" runat="server"  Text='<%# Bind("VAT") %>' ToolTip="Total VAT"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>--%>
                                   <asp:TemplateField HeaderText="Acquisition Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblacquisition" runat="server"  Text='<%# Bind("ACQUISITION_DATE") %>' ToolTip="ACQUISITION_DATE"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                            <%--     <asp:TemplateField HeaderText="Depreciation%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDP" runat="server"  Text='<%# Bind("DP") %>' ToolTip="Depreciation %"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>--%>
                                 <asp:TemplateField HeaderText="Depreciation Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDEPRECIATION_AMOUNT" runat="server"  Text='<%# Bind("DEPRECIATION_AMOUNT1") %>' ToolTip="DEPRECIATION AMOUNT"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblDEPRECIATION_AMOUNTf" runat="server"  ToolTip="sum of Depreciation Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                              <%--     <asp:TemplateField HeaderText="FY Depreciation">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFYDepreciation" runat="server"  Text='<%# Bind("FYDEPRECIATION_AMOUNT") %>' ToolTip="Financial Year Depreciation"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Month Depreciation">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrentMonthDepreciation" runat="server"  Text='<%# Bind("CURRDEPRECIATION_AMOUNT") %>' ToolTip="Current Month Depreciation"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>--%>
                          <%--       <asp:TemplateField HeaderText="Closing WDV">
                                    <ItemTemplate>
                                        <asp:Label ID="lblClosingWDV" runat="server"  Text='<%# Bind("WDV") %>' ToolTip="Closing WDV"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>--%>
                                 <asp:TemplateField HeaderText="Principal Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrincipalamount" runat="server"  Text='<%# Bind("PRINCIPAL1") %>' ToolTip="Closing WDV"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblPrincipalamountf" runat="server"  ToolTip="sum of Principal Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                           <%--      <asp:TemplateField HeaderText="RV %">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRV" runat="server"  Text='<%# Bind("RV") %>' ToolTip="Residual Value%"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>--%>
                                 <asp:TemplateField HeaderText="Residual Value Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResidualAmount" runat="server"  Text='<%# Bind("RVAMT1") %>' ToolTip="Residual Value Amount"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblResidualAmountf" runat="server"  ToolTip="sum of Residual Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Gross Income Earned">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGross" runat="server"  Text='<%# Bind("GROSS1") %>' ToolTip="Gross income Earned"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrossf" runat="server"  ToolTip="sum of Gross Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Expenses">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpenses" runat="server"  Text='<%# Bind("EXPENSES1") %>' ToolTip="Expenses"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblExpensesf" runat="server"  ToolTip="sum of Expenses Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Net Income">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNetIncome" runat="server"  Text='<%# Bind("NETINCOME1") %>' ToolTip="Net Income"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblNetIncomef" runat="server"  ToolTip="sum of Net Income Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                              <%--        <asp:TemplateField HeaderText="Arrears">
                                    <ItemTemplate>
                                        <asp:Label ID="lblArrears" runat="server"  Text='<%# Bind("ARREARS") %>' ToolTip="Arrears"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>--%>
                                   <asp:TemplateField HeaderText="PV of Gross Income">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPVGROSS" runat="server"  Text='<%# Bind("PVGROSS1") %>' ToolTip="PV of Gross Income"></asp:Label>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <asp:Label ID="lblPVGROSSf" runat="server"  ToolTip="sum of PV Gross Amount"></asp:Label>
                                    </FooterTemplate>
                                    <FooterStyle HorizontalAlign="Right" />
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right"  />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="PV of Arrears">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPVARREARS" runat="server"  Text='<%# Bind("PVARREARS") %>' ToolTip="PV of Arrears"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>--%>
                                  <asp:TemplateField HeaderText="Company_IRR">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompany_IRR" runat="server"  Text='<%# Bind("Company_IRR") %>' ToolTip="Company_IRR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left"  />
                                </asp:TemplateField>
                              
                            </Columns>
                          </asp:GridView>
                        </div>
                        </asp:Panel>
            </td>
        </tr>
                <tr align="center">
            <td >
                <asp:Button ID="BtnPrint" CssClass="styleSubmitButton" Visible="false" runat="server" Text="Print" OnClick="BtnPrint_Click"  ToolTip="Print" />
                      
            </td>
           
        </tr>
            <tr>
            <td>
                <br />
                <asp:ValidationSummary ID="vsWDV" runat="server" ShowSummary="true" CssClass="styleMandatoryLabel"
                    ValidationGroup="btnGo" ShowMessageBox="false" HeaderText="Correct the following validation(s):" />
                <%-- <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="true"
                    CssClass="styleMandatoryLabel" ValidationGroup="vgAdd" ShowMessageBox="false"
                    HeaderText="Correct the following validation(s):" />--%>
                <asp:CustomValidator ID="cvWDV" runat="server" CssClass="styleMandatoryLabel"
                    Enabled="true" />
            </td>
        </tr>
        
        </table>

  
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

