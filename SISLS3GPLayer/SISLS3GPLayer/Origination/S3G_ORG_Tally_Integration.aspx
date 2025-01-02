<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_ORG_Tally_Integration.aspx.cs"
    EnableEventValidation="false" Inherits="Origination_S3G_ORG_Tally_Integration"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register TagPrefix="uc3" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
       
        function fnConfirmSaveTally() {

            if (confirm('Do you want to Post Data to Tally?')) {
                return true;
            }
            else
                return false;

        }


    </script>
    <%--<asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>--%>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
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
                                    <asp:Panel GroupingText="Lessee / Vendor Details" ID="Panel2" runat="server" CssClass="stylePanel"
                                        ToolTip="Lessee / Vendor Details">
                                        <table width="100%" align="center" border="0" cellspacing="0">
                                             <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="Label2" CssClass="styleDisplayLabel" Text="Invoice Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                     <asp:RadioButtonList RepeatDirection="Horizontal" CssClass="styleDisplayLabel" runat="server" ID="RBLPOType"
                                                                     ToolTip="Invoice Type" Width="98%">
                                                                    <asp:ListItem Selected="True" Value="0" Text="Purchase Invoice"></asp:ListItem>
                                                                    <asp:ListItem Value="1" Text="Sales Invoice"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                    </td>
                                                 <td colspan="2"></td>
                                                 </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPO_From_Date" CssClass="styleDisplayLabel" Text="Rental Schedule From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPO_From_Date" runat="server" ToolTip="Rental Schedule From Date"></asp:TextBox>
                                                    <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgFromDate" TargetControlID="txtPO_From_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <%--<asp:RequiredFieldValidator ID="RFVFromDate" ValidationGroup="Print" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPO_From_Date" SetFocusOnError="True"
                                                        ErrorMessage="Enter a PO From Date" Display="None"></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPO_To_Date" CssClass="styleDisplayLabel" Text="Rental Schedule To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPO_To_Date" runat="server" ToolTip="Rental Schedule To Date"></asp:TextBox>
                                                    <asp:Image ID="imgToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderToDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgToDate" TargetControlID="txtPO_To_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <%--<asp:RequiredFieldValidator ID="rfvPO_To_Date" ValidationGroup="Print" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPO_To_Date" SetFocusOnError="True"
                                                        ErrorMessage="Enter a PO To Date" Display="None"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>

                                                 <td class="styleFieldLabel" width="20%">
                                                    <asp:Label runat="server" Text="Status" ID="lblPOStatus"  CssClass="styleReqFieldLabel" ToolTip="Invoice Status">
                                                    </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="30%">
                                                    <asp:DropDownList ID="ddlStatus" runat="server"  Width="55%" ToolTip="Status">
                                                         <asp:ListItem Text="Select" Selected="true" Value="-1"></asp:ListItem>
                                                         <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="WIP & Processed" Value="0"></asp:ListItem>
                                                   </asp:DropDownList>
                                                     <asp:RequiredFieldValidator  ID="rfvddlStatus" runat="server" ControlToValidate="ddlStatus"
                                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Status"
                                                                    InitialValue="-1" SetFocusOnError="False" ValidationGroup="vsSave"></asp:RequiredFieldValidator>
                                                </td>

                                               
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlVendorName" runat="server" ServiceMethod="GetVendors" />
                                                </td>
                                            </tr>
                                            <tr>

                                                 <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Lessee Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlCustomerName"  Width="280px"  runat="server" ServiceMethod="GetCustomerNameDetails" />
                                                </td>

                                                <td class="styleFieldLabel" >
                                                    <asp:Label runat="server" ID="Label1" CssClass="styleDisplayLabel" Text="Rental Schedule No."></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlPONo" runat="server" ServiceMethod="GetRSNoDetails" />
                                                </td>
                                   
                                            </tr>
                                            <tr>

                                                
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblLoadSequenceNo" CssClass="styleDisplayLabel" Text="Tranche Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlLoadSequenceNo" runat="server" ServiceMethod="GetTrancheDetails" />
                                                </td>


                                               
                                                <td></td>
                                                <td></td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:Button runat="server" ID="btnSearch" CssClass="styleSubmitButton" Text="Search"
                                                        OnClick="btnSearch_Click"  ValidationGroup="vsSave" />

                                                    <asp:Button runat="server" ID="btnRefresh" CssClass="styleSubmitButton" Text="Refresh"  visible="false"
                                                        OnClick="btnSearch_Click" ValidationGroup="Print" />
                                                    
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
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Panel GroupingText="RS Details" ID="pnlPO" runat="server" Visible="false" CssClass="stylePanel">
                                       <%--  <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1100px;height:250px">--%>
                                        <asp:GridView ID="gvPO" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="95%" OnRowDataBound="gvPO_RowDataBound">
                                            <Columns>
											<asp:TemplateField HeaderText="Sl.No." Visible="false" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblTally_Integ_Hdr_ID" Visible="false" runat="server" Text='<%# Eval("Tally_Integ_Hdr_ID") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
												<asp:TemplateField HeaderText="Lessee Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer_Name" runat="server" Text='<%# Eval("Customer_Name") %>'
                                                            ToolTip="Lessee Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
												<asp:TemplateField HeaderText="Tranche Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTranche_Number" runat="server" Text='<%# Eval("Tranche_Name") %>' ToolTip="Tranche Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="RS Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPANum" runat="server" Text='<%# Eval("PANum") %>' ToolTip="RS Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Rental Schedule Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPO_Date" runat="server" Text='<%# Eval("Rental_Schedule_Date") %>'
                                                            ToolTip="Rental Schedule Date" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:TemplateField>

                                                 <asp:TemplateField HeaderText="Posted On" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPosted_On" runat="server" Text='<%# Eval("Created_On") %>'/>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Posted By" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPosted_By" runat="server" Text='<%# Eval("Created_by") %>' />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                            ToolTip="Status" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>

                                            <asp:TemplateField HeaderText="View">
                                               <ItemTemplate>
                                                   <asp:LinkButton ID="btnException" Enabled="false" runat="server" CausesValidation="false"
                                                    CommandName="Exception" Text="View/Exception" OnClick="btnException_Click" />
                                               </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="16%"></ItemStyle>
                                           </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Select">
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Select All
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkSelected" ToolTip="select" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
												</Columns>
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucCustomPaging" runat="server"></uc3:PageNavigator>
                                       <%-- </div>--%>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        
                           <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" visible="false"
                               OnClientClick="return fnConfirmSaveTally();" Text="Post to Tally"
                                                        OnClick="btnSave_Click"  />

                        <asp:Button runat="server" ID="btnExport1" CssClass="styleSubmitButton" Text="Export"  visible="false"
                            OnClick="btnExport1_Click"  Enabled="false" />
                                             
                        
                    </td>
                </tr>

                

                <tr>
                    <td>
                       <asp:ValidationSummary runat="server" ID="vsSave" ValidationGroup="vsSave"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
                 
            </table>
       <%-- </ContentTemplate>--%>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport1" />
           
         </Triggers>
   <%-- </asp:UpdatePanel>--%>
    <table>
     <tr><td>
                </td></tr>
        </table>
</asp:Content>
