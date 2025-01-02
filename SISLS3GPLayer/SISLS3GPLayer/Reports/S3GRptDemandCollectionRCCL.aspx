 
<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptDemandCollectionRCCL.aspx.cs" Inherits="Reports_S3GRptDemandCollectionRCCL" Title="Demand Collection Region Customer Code Level" %>
 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<table width="100%">
<tr>
<td>
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Demand Collection Region Customer Code Level Report">
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
                <asp:Panel ID="pnlDemand" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left">
                                <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlLOB" runat="server" ErrorMessage="Select Line of Business." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlLOB" InitialValue="-1"></asp:RequiredFieldValidator>
 
                            </td>
                        </tr>
                       <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" ID="lblRegion" Text="Location1" CssClass="styleMandatoryLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                </asp:DropDownList>
                                 <%-- %><asp:RequiredFieldValidator ID="rfvddlRegion" runat="server" ErrorMessage="Select Region." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlRegion" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                       <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" ID="lblBranch" Text="Location2" CssClass="styleMandatoryLabel"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlBranch" runat="server">
                                </asp:DropDownList>
                              <%-- <asp:RequiredFieldValidator ID="rfvddlBranch" runat="server" ErrorMessage="Select Branch." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlBranch" InitialValue="-1"></asp:RequiredFieldValidator>--%>
 
                            </td>
                        </tr>
                       <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" ID="lblReportDate" CssClass="styleReqFieldLabel" Text="Report Date"></asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:TextBox ID="txtReportDate" AutoPostBack="false" runat="server" Width="56%" 
                                    ontextchanged="txtReportDate_TextChanged" ></asp:TextBox>
                                <asp:Image ID="imgReportDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtReportDate" PopupButtonID="imgReportDate" ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvReportDate" runat="server" ErrorMessage="Select Report Date." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtReportDate"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                       <tr>
                            <td height="8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" Text="Denomination" ID="lblDenomination" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <asp:DropDownList ID="ddlDenomination" runat="server">
                                </asp:DropDownList>
<%--                               <asp:RequiredFieldValidator ID="rfvddlDenomination" runat="server" ErrorMessage="Select Denomination." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlDenomination" InitialValue="-1"></asp:RequiredFieldValidator>
--%>
                            </td>
                        </tr>
                       <tr>
                            <td height="8px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td width="50%" valign="top">
        <asp:Panel ID="pnlGroupingCriteria" runat="server" GroupingText="Grouping Criteria" 
                CssClass="stylePanel" Width="99%">
        <table>
      
          <tr>
            <td align="left" width="35%">
        <asp:Label ID="lblCustomerCodeLevel" runat="server" Text="Customer Code level" CssClass="styleFieldLabel" Height="25%">
        </asp:Label>
       </td>
        <td align="left" width="45%">
        <asp:CheckBox ID="chkCustomerCodeLevel" runat="server" AutoPostBack="true"
                oncheckedchanged="chkCustomerCodeLevel_CheckedChanged" Height="25%" />
        </td>
        </tr>
       
        <tr>
          
            <td align="left" width="45%">
        <asp:Label ID="lblCustomerGroupCodeLevel" runat="server" Text="Customer Group Code level" CssClass="styleFieldLabel">
        </asp:Label>        
       </td>       
        <td align="left" width="45%">
        <asp:CheckBox ID="ChkCustomerGroupCodeLevel" runat="server" AutoPostBack="true" 
                oncheckedchanged="ChkCustomerGroupCodeLevel_CheckedChanged" />
        </td>
       </tr>
      
       <tr>
            <td align="left" width="35%">
        <asp:Label ID="lblAccountLevel" runat="server" Text="Account level" CssClass="styleFieldLabel">
        </asp:Label>
      </td>
        <td align="left" width="45%">
        <asp:CheckBox ID="ChkAccountLevel" runat="server" AutoPostBack="true"  
                oncheckedchanged="ChkAccountLevel_CheckedChanged" />
        </td>
        </tr>
        </table>        
        </asp:Panel> 
       </td>      
      <%--  <table width="100%" border="0">--%>
       <%-- <tr>--%>
                <%-- <td width="50%">
                <asp:Panel ID="PnlFrequency" runat="server" GroupingText="Frequency" CssClass="stylePanel" Width="100%">
                    <asp:GridView ID="grvFrequency" runat="server" Width="100%" AutoGenerateColumns="false" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" ShowFooter="true">
                        <Columns>
                        <asp:TemplateField HeaderText="FrequencyId" ItemStyle-HorizontalAlign="Left" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblFrequencyId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FrequencyName" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblFrequencyName" runat="server" Text='<%# Bind("NAME") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ChkSelect" AutoPostBack="true" runat="server" Enabled="false" OnCheckedChanged="Chkselect_OnCheckedChanged" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
           </td>--%>
           <%-- <td width="50%">--%>
               <%-- <asp:Panel ID="PnlCategories" runat="server" GroupingText="Asset Categories" CssClass="stylePanel" Width="80%" Height="100%">
                    <table cellpadding="0" cellspacing="1" style="width:100%" border="0">
                       <tr>
                            <td class="styleFieldLabel" width="150%">
                                <asp:Label runat="server" Text="Asset Categories Type" ID="LblAssetCategoriesType" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left">
                                <asp:DropDownList ID="DdlAssetCategoriesType" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RfvAssetCategoriesType" runat="server" ErrorMessage="Select a Asset Categories Type." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="DdlAssetCategoriesType" InitialValue="-1"></asp:RequiredFieldValidator>
 
                            </td>
                        </tr>
                        <tr>                            
                            <td class="styleFieldAlign">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <div style="height: 27px;" id="div1" runat="server">
                    </div>
                </asp:Panel>--%>
               <%-- <td>
                <asp:Panel ID="PnlCategories" runat="server" GroupingText="Asset Categories" CssClass="stylePanel" Width="100%" Height="30%">
                    <table cellpadding="0" cellspacing="1" style="width: 50%" border="0">
                        <tr>
                            <td class="styleFieldLabel" align="right">
                                <asp:Label runat="server" Text="Categories" ID="lblCategories" CssClass="styleReqFieldLabel">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign">
                                <%--<asp:RadioButtonList ID="rbtlCategories" runat="server" RepeatDirection="Vertical" AutoPostBack="true" class="styleFieldLabel" OnSelectedIndexChanged="rbtlCategories_SelectedIndexChanged">
                                    <asp:ListItem Text="Machinery" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Vehicles" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Both" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>--%>
               <%--<asp:DropDownList ID="DdlAssetCategoriesType" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RfvAssetCategoriesType" runat="server" ErrorMessage="Select a Asset Categories Type." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="DdlAssetCategoriesType" InitialValue="-1"></asp:RequiredFieldValidator>--%>
                                <%--                                                                <asp:RequiredFieldValidator ID="rfvCategories" runat="server" ErrorMessage="Select Asset Categories." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlCategories" InitialValue="0"></asp:RequiredFieldValidator>
--%>
                           
                     
        </tr>
        </table>
</td>
</tr>
<tr>
<td>
    <table>
    <tr>   
        <td width="50%">
               <asp:Panel ID="PnlDemandData" runat="server" GroupingText="Demand Data" CssClass="stylePanel" Width="100%">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%" align="center">
                                    <tr>
                                        <td align="center" class="styleFieldLabel">
                                            <asp:Label ID="lblFinancialYearBase" runat="server" Text="Financial Years "></asp:Label>
                                        </td>
                                        <td align="center" class="styleFieldAlign">
                                            <asp:DropDownList ID="ddlFinacialYearBase" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinacialYearBase_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center" class="styleFieldLabel">
                                            <asp:Label ID="lblFromYearMonthBase" runat="server" Text="From Year Month" CssClass="styleReqFieldLabel"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" align="center">
                                            <asp:DropDownList ID="ddlFromYearMonthBase" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFromYearMonthBase_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlFromYearMonthBase" runat="server" ErrorMessage="Select From Year Month." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlFromYearMonthBase" InitialValue="0"></asp:RequiredFieldValidator>
                                        </td>
                                        <td align="center" class="styleFieldLabel">
                                            <asp:Label ID="lblToYearMonthBase" runat="server" Text="To Year Month" CssClass="styleReqFieldLabel"> </asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" align="center">
                                            <asp:DropDownList ID="ddlToYearMonthBase" runat="server" OnSelectedIndexChanged="ddlToYearMonthBase_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlToYearMonthBase" runat="server" ErrorMessage="Select To Year Month." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="ddlToYearMonthBase" InitialValue="0"></asp:RequiredFieldValidator>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td height="8px">
                                        </td>
                                    </tr>
                                    
                                    <tr align="center">
                                        <td align="center" class="styleFieldLabel">
                                            <asp:Label ID="lblFinancialYearCompare" runat="server" Text="Financial Years "></asp:Label>
                                        </td>
                                        <td align="center" class="styleFieldAlign">
                                            <asp:DropDownList ID="ddlFinancialYearCompare" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinancialYearCompare_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            
                                        </td>
                                        <td align="center" class="styleFieldLabel">
                                            <asp:Label ID="lblFromYearMonthCompare" runat="server" Text="From Year Month"></asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" align="center">
                                            <asp:DropDownList ID="ddlFromYearMonthCompare" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFromYearMonthCompare_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center" class="styleFieldLabel">
                                            <asp:Label ID="lblToYearMonthCompare" runat="server" Text="To Year Month"> </asp:Label>
                                        </td>
                                        <td class="styleFieldAlign" align="center">
                                            <asp:DropDownList ID="ddlToYearMonthCompare" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlToYearMonthCompare_SelectedIndexChanged">
                                            </asp:DropDownList>
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
    </table>         
</td>
</tr>
<tr>
<td>
    <table width="100%" border="0">   
     
     <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="3" align="center">
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="GO" CausesValidation="true" OnClientClick="return fnCheckPageValidators('Ok',false);" ValidationGroup="Ok" Visible="true" OnClick="btnOk_Click"/>
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();"/>
            </td>
     </tr>    
     <tr>
     <td align="right">
      <asp:Label ID="lblCurrency" runat="server" Text="" Width="100%">
     </asp:Label>
     </td>
     </tr>
     </table>
 </td>
 </tr>
 <tr>
 <td>
    <table width="100%">
     <tr>   
            <td>
                        
                <asp:Panel ID="PnlDemandCollectionCusomerCodeLevel" runat="server" GroupingText="Demand Collection Region Customer Code Level" CssClass="stylePanel" Width="100%">
                 <div id="divDemand" runat="server" style="overflow:auto;height:300px;display: none">
                    <asp:GridView ID="grvDemandCollectionCustomerCodeLevel" runat="server" 
                        Width="100%" Height="200px" AutoGenerateColumns="false"
                        RowStyle-HorizontalAlign="Center" ShowFooter="true" HeaderStyle-CssClass="Freezing">
                        <Columns>
                       <%--  <asp:TemplateField HeaderText="Frequency" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblFrequency" runat="server" Text='<%# Bind("Frequency") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text="Total" align="center"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                <FooterStyle HorizontalAlign="center" />
                            </asp:TemplateField>--%>
                         <asp:TemplateField HeaderText="Demand Month" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblDemandMonth" runat="server" Text='<%# Bind("DemandMonth") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                                 <FooterTemplate>
                                <asp:Label ID="lblTotal" runat="server" Text="Grand Total"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                   <asp:TemplateField HeaderText="LOB" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LineOfBusiness") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Region" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegion" runat="server" Text='<%# Bind("Region") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>   
                             <asp:TemplateField HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerCode" runat="server" 
                                        Text='<%# Bind("CustomerName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField> 
                                  <asp:TemplateField HeaderText="Customer Group Name" 
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblCustomerGroupCode" runat="server" 
                                        Text='<%# Bind("CustomerGroupCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Account No" 
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrimeAccountNo" runat="server" 
                                        Text='<%# Bind("PrimeAccountNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="Sub Account No" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubAccountNo" runat="server" 
                                        Text='<%# Bind("SubAccountNo") %>'></asp:Label>
                                </ItemTemplate>                               
                                <ItemStyle HorizontalAlign="Left" />                              
                            </asp:TemplateField>  
                                 <asp:TemplateField HeaderText="Opening Demand" 
                                ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpeningDemand" runat="server" 
                                        Text='<%# Bind("OpeningDemand") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lbltotOpeningDemand" runat="server" align="right"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>  
                                   <asp:TemplateField HeaderText="Opening Collection" 
                                ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpeningCollection" runat="server" 
                                        Text='<%# Bind("OpeningCollection") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lbltotOpeningCollection" runat="server"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="Collection %" 
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblOpeningPercentage" runat="server" 
                                        Text='<%# Bind("OpeningPercentage") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lbltotOpeningPercentage" runat="server"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>   
                                <asp:TemplateField HeaderText="Monthly Demand" 
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriodDemand" runat="server" 
                                        Text='<%# Bind("MonthlyDemand") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lbltotMonthlyDemand" runat="server"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField> 
                               <asp:TemplateField HeaderText="Monthly Collection" 
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriodCollection" runat="server" 
                                        Text='<%# Bind("MonthlyCollection") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <asp:Label ID="lbltotMonthlyCollection" runat="server"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>  
                                <asp:TemplateField HeaderText="Collection %" 
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriodPercentage" runat="server" 
                                        Text='<%# Bind("MonthlyPercentage") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lbltotMonthlyPercentage" runat="server"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right"/>
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField> 
                             <asp:TemplateField HeaderText="Cumulative Demand" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lblClosingDemand" runat="server" 
                                        Text='<%# Bind("ClosingDemand") %>'></asp:Label>
                                </ItemTemplate> 
                                 <FooterTemplate>
                                        <asp:Label ID="lbltotClosingDemand" runat="server"></asp:Label>
                                    </FooterTemplate>                              
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Cumulative Collection" 
                                ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lblClosingCollection" runat="server" 
                                        Text='<%# Bind("ClosingCollection") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lbltotClosingCollection" runat="server"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>    
                                    <asp:TemplateField HeaderText="Collection %" 
                                ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lblClosingPercentage" runat="server" 
                                        Text='<%# Bind("ClosingPercentage") %>'></asp:Label>
                                </ItemTemplate>
                                 <FooterTemplate>
                                        <asp:Label ID="lbltotClosingPercentage" runat="server"></asp:Label>
                                    </FooterTemplate>
                                <ItemStyle HorizontalAlign="right" />
                                <FooterStyle HorizontalAlign="right" />
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>               
            </td>    
     </tr>
     </table>
  </td>
 </tr>
 <tr>
    <table width="100%">
     <tr align="center">
     <td align="center" width="100%">
         <asp:Button ID="BtnPrint" runat="server" Text="Print" CssClass="styleSubmitButton" OnClick="BtnPrint_Click" />
         </td>
     </tr>
      <tr>
            <td>
                <asp:ValidationSummary ID="vsDemandCollection" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
 
            </td>
      </tr>
      <tr>
            <td>
<%--                <asp:CustomValidator ID="CVDemandCollection" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
--%>        </td>
      </tr>
    </table>
 </tr>
</table>
   <script language="javascript" type="text/javascript">
   function Resize()
     {
       if(document.getElementById('ctl00_ContentPlaceHolder1_divDemand') != null)
       {
         if(document.getElementById('divMenu').style.display=='none')
            {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
         else
           {
             (document.getElementById('ctl00_ContentPlaceHolder1_divDemand')).style.width = screen.width - 270;
           }
        }  
      }     
     
      
    
//    function fnScaleFactorX() {
//    var nScaleFactor = screen.deviceXDPI / screen.logicalXDPI;
//    return nScaleFactor; 

    </script>  
     <style type="text/css">
    .Freezing
    {
      position:relative;
      top:auto;
      z-index:auto;
    }
  </style>
</asp:Content>

 
 
 
 
 
