<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_RPT_Inv_Summary.aspx.cs" Inherits="Reports_S3G_RPT_Inv_Summary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="ContentPurchaseInvoiceDetailsReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpPurchaseInvoiceDetailsReport" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="Invoice Summary Report" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="pnlPurchaseInvoiceDetailsReport" runat="server" GroupingText="Filter Criteria"
                            CssClass="stylePanel" Width="100%">
                            <table width="100%" cellpadding="0" style="width: 100%" align="center">
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoicePostingDateFrom" runat="server" Text="Rental Schedule From Date"/>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <input id="hidInvoicePostingDateDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtInvoicePostingDateFrom" runat="server" AutoPostBack="false" OnTextChanged="txtInvoicePostingDateFrom_TextChanged" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgInvoicePostingDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderInvoicePostingDateFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgInvoicePostingDateFrom"
                                            TargetControlID="txtInvoicePostingDateFrom">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" Enabled="false" ControlToValidate="txtInvoicePostingDateFrom"
                                            ErrorMessage="Enter the Account Creation From Date" Display="None" ValidationGroup="Go">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblInvoicePostingDateTo" runat="server" Text="Rental Schedule To Date" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtInvoicePostingDateTo" runat="server" AutoPostBack="false" OnTextChanged="txtInvoicePostingDateTo_TextChanged" Width="140px"></asp:TextBox>
                                        <asp:Image ID="imgInvoicePostingDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderInvoicePostingDateTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgInvoicePostingDateTo"
                                            TargetControlID="txtInvoicePostingDateTo">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvToDate" runat="server" Enabled="false" ControlToValidate="txtInvoicePostingDateTo"
                                            ErrorMessage="Enter the Account Creation To Date" Display="None" ValidationGroup="Go">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"/>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <uc2:Suggest ID="ddlCustName" runat="server" ServiceMethod="GetCustomerName" WatermarkText="--All--"
                                            ErrorMessage="Enter Customer Name" IsMandatory="false"
                                            ItemToValidate="Value" AutoPostBack="true" OnItem_Selected="ddlCustName_Item_Selected" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblTranche" runat="server" Text="Tranche Name"/>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <uc2:Suggest ID="ddlTranche" runat="server" ServiceMethod="GetTrancheName" WatermarkText="--All--"
                                            ErrorMessage="Enter Customer Name" IsMandatory="false"
                                            ItemToValidate="Value" AutoPostBack="true" OnItem_Selected="ddlTranche_Item_Selected" />
                                    </td>

                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblAccFrom" Text="RS Number"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:suggest id="ddlAccountFrom" runat="server" autopostback="True" WatermarkText="--All--"
                                            servicemethod="FunPriBindAccountFrom" errormessage="Enter RS Number"
                                            ismandatory="false" validationgroup="vgGo" tooltip="RS Number" OnItem_Selected="ddlAccountFrom_Item_Selected"
                                            maxlength="50" />



                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" ID="lblAccTo" Visible ="false" Text="RS Number"></asp:Label>
										<asp:Label runat="server" ID="lblState" Text="State"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">

                                        <uc2:suggest id="ddlAccountTo" runat="server" Visible="false" autopostback="True" WatermarkText="--All--"
                                            servicemethod="FunPriBindAccountTo" errormessage="Enter Account From"
                                            ismandatory="false" validationgroup="vgGo" tooltip="Account From"
                                            maxlength="50" />
											<uc2:suggest id="ddlState" runat="server" WatermarkText="--All--"
                                            servicemethod="GetBranchList" errormessage="Enter Account From"
                                            ismandatory="false" validationgroup="vgGo" tooltip="State"
                                            maxlength="50" />

                                    </td>

                                </tr>

                                <tr>
								 <td class="styleFieldLabel">
                                        
                                    </td>
                                    <td>
									
                                    </td>
                                </tr>


                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>


                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" colspan="4" align="center">
                        <asp:Button ID="btnSearch" runat="server" Text="Go" UseSubmitBehavior="true" ValidationGroup="Go" ToolTip="Go" CssClass="styleSubmitButton" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClientClick="return fnConfirmClear();" ToolTip="Clear" OnClick="btnclear_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="PnPurchaseInvoiceDetails" runat="server" GroupingText="Purchase Invoice Details"
                            CssClass="stylePanel" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1050px">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:GridView ID="grvPurchaseInvoiceDetailsReport" runat="server" Width="100%" EmptyDataText="No Record Found"
                                                AutoGenerateColumns="true"
                                                OnRowDataBound="grvPurchaseInvoiceDetailsReport_RowDataBound">
                                            </asp:GridView>
                                            <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center"></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnPrint" runat="server" Text="Export" Visible="false" ToolTip="Export"
                            UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnPrint_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:ValidationSummary ID="VSMRAReport" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td id="tdHeight" runat="server"></td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="ddlCustName" />

        </Triggers>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
        }
        function pageLoad(s, a) {
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
        }
        function showMenu(show) {
            if (show == 'T') {
                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - 260;
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }
        function Resize() {
            document.getElementById('<%=tdHeight.ClientID %>').style.height = 100;
        }
    </script>
</asp:Content>

