<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_ORG_PurchaseOrder_Print.aspx.cs"
    EnableEventValidation="false" Inherits="Origination_S3G_ORG_PurchaseOrder_Print"
    MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register TagPrefix="uc3" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function fnConfirmCancel() {

            if (confirm('Do you want to cancel PO?')) {
                return true;
            }
            else
                return false;
        }

        function fnEMailVal(btn) {
            //btn.style.visibility = 'hidden';
            btn.disabled = true;
            //var a = event.srcElement;
            //a.style.display = 'block';
            //a.style.removeAttribute('display');
            document.getElementById('<%=btnEmailRender.ClientID%>').click();
            return true;
        }

    </script>
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
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
                                                    <asp:Label runat="server" ID="Label2" CssClass="styleDisplayLabel" Text="PO Type"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                     <asp:RadioButtonList RepeatDirection="Horizontal" AutoPostBack="true" CssClass="styleDisplayLabel" runat="server" ID="RBLPOType"
                                                                     ToolTip="PO Type" Width="98%">
                                                                    <asp:ListItem Selected="True" Value="0" Text="Purchase Order"></asp:ListItem>
                                                                    <asp:ListItem Value="1" Text="Master Purchase Order"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                    </td>
                                                 <td colspan="2"></td>
                                                 </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPO_From_Date" CssClass="styleDisplayLabel" Text="PO From Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPO_From_Date" runat="server" ToolTip="PO From Date"></asp:TextBox>
                                                    <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderFromDate" runat="server" Enabled="True"
                                                        PopupButtonID="imgFromDate" TargetControlID="txtPO_From_Date" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                                    </cc1:CalendarExtender>
                                                    <%--<asp:RequiredFieldValidator ID="RFVFromDate" ValidationGroup="Print" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="txtPO_From_Date" SetFocusOnError="True"
                                                        ErrorMessage="Enter a PO From Date" Display="None"></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblPO_To_Date" CssClass="styleDisplayLabel" Text="PO To Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtPO_To_Date" runat="server" ToolTip="PO To Date"></asp:TextBox>
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
                                                <td class="styleFieldLabel">
                                                    <asp:Label runat="server" ID="lblCustomerName" CssClass="styleDisplayLabel" Text="Lessee Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlCustomerName" runat="server" ServiceMethod="GetCustomerNameDetails" />
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
                                                    <asp:Label runat="server" ID="lblLoadSequenceNo" CssClass="styleDisplayLabel" Text="Load Sequence No."></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlLoadSequenceNo" runat="server" ServiceMethod="GetLSQNo" />
                                                </td>

                                                <td class="styleFieldLabel" width="20%">
                                                    <asp:Label runat="server" Text="PO Status" ID="lblPOStatus" CssClass="styleDisplayLabel" ToolTip="PO Status">
                                                    </asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" width="30%">
                                                    <asp:DropDownList ID="ddlPOStatus" runat="server" AutoPostBack="true" Width="55%" ToolTip="PO Status">
                                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Scheduled" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Un Scheduled" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="Cancelled" Value="3"></asp:ListItem>
                                                        </asp:DropDownList>
                                                </td>
                                   
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" >
                                                    <asp:Label runat="server" ID="Label1" CssClass="styleDisplayLabel" Text="Purchase Order No."></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <uc2:Suggest ID="ddlPONo" Width="250px" runat="server" ServiceMethod="GetPONo" />
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:Button runat="server" ID="btnSearch" CssClass="styleSubmitButton" Text="Search"
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
                                    <asp:Panel GroupingText="Purchase Order Details" ID="pnlPO" runat="server" Visible="false" CssClass="stylePanel">
                                        <asp:GridView ID="gvPO" runat="server" AutoGenerateColumns="False" DataKeyNames="RowNumber" Width="100%" OnRowDataBound="gvPO_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL.No." HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("RowNumber") %>' />
                                                        <asp:Label ID="lblPO_dtl_ID" runat="server" Text='<%# Eval("PO_HDR_ID") %>' Visible="false" />
                                                        <asp:Label ID="lblPO_Vendor_Group" runat="server" Text='<%# Eval("PO_Vendor_Group") %>' Visible="false" />
                                                        <asp:Label ID="lblEntity_ID" runat="server" Text='<%# Eval("Entity_ID") %>' Visible="false" />
                                                        <asp:Label ID="lblPOType" runat="server" Text='<%# Eval("PO_Type") %>' Visible="false" />
                                                        
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PO Number" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPO_Number" runat="server" Text='<%# Eval("PO_Number") %>' ToolTip="PO Number" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PO Date" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPO_Date" runat="server" Text='<%# Eval("PO_Date") %>'
                                                            ToolTip="PO Date" />
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
                                                <asp:TemplateField HeaderText="Vendor Name" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEntity_Name" runat="server" Text='<%# Eval("Entity_Name") %>'
                                                            ToolTip="Entity Name" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Asset Descirption" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAsset_Descirption" runat="server" Text='<%# Eval("Asset_Description") %>'
                                                            ToolTip="Asset Descirption" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotal_Bill_Amount" runat="server" Text='<%# Eval("Total_Bill_Amount") %>'
                                                            ToolTip="Total Bill Amount" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="styleGridHeader">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                            ToolTip="Status" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" />
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
                                                <asp:TemplateField HeaderText="Update PO">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="false"
                                                            CommandName="Exception" Text="Update" OnClick="lnkUpdate_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                        </asp:GridView>
                                        <uc3:PageNavigator ID="ucCustomPaging" runat="server"></uc3:PageNavigator>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel GroupingText="Print Details" ID="pnlPrintDetails" runat="server" CssClass="stylePanel">
                            <table width="99%">
                                <tr>
                                    <td style="width: 10%" class="styleFieldLabel">
                                        <asp:Label ID="lblPrintType" runat="server" Text="Print Type" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td style="width: 15%">
                                        <asp:DropDownList ID="ddlPrintType" runat="server">
                                            <asp:ListItem Value="P" Text="PDF"></asp:ListItem>
                                            <asp:ListItem Value="W" Text="WORD"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print"
                            OnClick="btnPrint_Click" ValidationGroup="Print" Enabled="false" />
                        <asp:Button runat="server" ID="btnExport1" CssClass="styleSubmitButton" Text="PO Export"
                            OnClick="btnExport1_Click" ValidationGroup="Print" Enabled="false" />
                        <asp:Button runat="server" ID="btnExport2" CssClass="styleSubmitButton" Text="PI Export"
                            OnClick="btnExport2_Click" ValidationGroup="Print" Enabled="false" />
                        <asp:Button runat="server" ID="btnExport3" CssClass="styleSubmitButton" Text="VI Export"
                            OnClick="btnExport3_Click" ValidationGroup="Print" Enabled="false" />
                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                            CausesValidation="False" OnClick="btnClear_Click" />
                        <asp:Button runat="server" ID="btnCancelPO" CssClass="styleSubmitButton" Text="Cancel PO" Enabled="false"
                            CausesValidation="False" />

                        <asp:Button runat="server" ID="btnEmail" CssClass="styleSubmitButton" Text="Send EMail" 
                            OnClientClick="return fnEMailVal(this)" CausesValidation="false" 
                             ToolTip="Send EMail" />
                        
                        <asp:Button runat="server" ID="btnEmailRender" CausesValidation="false" OnClick="btnEmail_Click" Width="1px" style="visibility:hidden;" />


                    </td>
                </tr>

                

                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsDelivery" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " ValidationGroup="Print" />
                    </td>
                </tr>
                 <cc1:ModalPopupExtender ID="ModalPopupExtenderPassword" runat="server" TargetControlID="btnCancelPO"
                                                    PopupControlID="PnlCancellation" BackgroundCssClass="styleModalBackground" DynamicServicePath=""
                                                    Enabled="True">
                                                </cc1:ModalPopupExtender>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport1" />
            <asp:PostBackTrigger ControlID="btnExport2" />
            <asp:PostBackTrigger ControlID="btnExport3" />
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="btnEmailRender" />
            
         </Triggers>
    </asp:UpdatePanel>
    <table>
     <tr><td>
                <table width="100%">
                                        <tr width="100%">
                                            <td width="100%">
                                                <asp:Panel DefaultButton="btnSubmit" ID="PnlCancellation" Style="display: none" runat="server"
                                                    Height="45%" BackColor="White" BorderStyle="Solid" BorderColor="Black" Width="45%">
                                                    <table width="100%">
                                                        <tr width="100%">
                                                            <td colspan="3" class="stylePageHeading" align="center">
                                                                <asp:Label runat="server"  Text="PO Cancellation" ID="lblPasswordHeader"
                                                                    CssClass="styleDisplayLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3"> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:20%" >
                                                                <asp:Label ID="lblCancReason" ToolTip="Reason for Cancellation" runat="server" CssClass="styleMandatoryLabel" 
                                                                     Text="Reason for Cancellation"></asp:Label>
                                                            </td>
                                                            
                                                            <td align="left" style="width:80%" class="styleFieldAlign">
                                                                 <asp:TextBox ID="txtCancReason" runat="server" Width="80%" Height="50px" TextMode="MultiLine" 
                                                                      ToolTip ="Reason for Cancellation" MaxLength="1000"></asp:TextBox>
                                                               
                                                            </td>
                                                            <td> <asp:RequiredFieldValidator ID="rvCancellation" ValidationGroup="Cancellation" runat="server" 
                                                                      ErrorMessage="Enter Reason for Cancellation"
                                                                      ControlToValidate="txtCancReason" CssClass="styleLoginLabel" Display="None"></asp:RequiredFieldValidator>

                                                            </td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td align="center" colspan="3">
                                                                <asp:Button ID="btnSubmit" ToolTip="Save Cancellation"  ValidationGroup="Cancellation" CausesValidation="true"
                                                                    CssClass="styleSubmitButton"  OnClientClick="return fnCheckPageValidators('Cancellation','false')" 
                                                                     OnClick="btnCancelPO_Click" Text="Submit" runat="server" />
                                                               
                                                                <asp:Button ID="btnCancelPopup" ToolTip="Cancel" CausesValidation="false" OnClick="btnCancelPopup_Click"
                                                                    CssClass="styleSubmitButton" Text="Cancel" runat="server" />

                                                                <asp:ValidationSummary ID="vsCancellation" ShowSummary="true" runat="server" CssClass="styleMandatoryLabel"
                                                                    HeaderText="Correct the following validation(s):  " ValidationGroup="Cancellation" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="3">
                                                                <asp:Label ID="lblErrorMessagePass" runat="server" CssClass="styleMandatoryLabel" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                               
                                            </td>
                                        </tr>
                    </table></td></tr>
        </table>
</asp:Content>
