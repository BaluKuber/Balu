<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GRptInterestQuery.aspx.cs" Inherits="Reports_S3GRptInterestQuery"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading" width="100%">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Interest Query Report"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldAlign">
                        <table width="100%">
                            <tr>
                                <td width="50%">
                                    <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Informations"
                                        CssClass="stylePanel">
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel" width="35%">
                                                    <asp:Label runat="server" ID="lblCustomerName" CssClass="styleReqFieldLabel" Text="Customer Name"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none;" MaxLength="50"></asp:TextBox>
                                                    <uc2:LOV ID="ucCustomerCodeLov" onblur="return fnLoadCustomer();" runat="server"
                                                        strLOV_Code="CMD" />
                                                    <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" OnClick="btnLoadCustomer_OnClick"
                                                        Style="display: none" /><input id="hdnCustID" type="hidden" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" width="100%">
                                                    <uc1:S3GCustomerAddress ID="ucCustDetails" ShowCustomerName="false" runat="server"
                                                        FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel" width="35%">
                                                    <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"
                                                        ToolTip="Line of Business"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" ValidationGroup="Header"
                                                        OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" ToolTip="Line of Business">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvlob" runat="server" Display="None" ControlToValidate="ddlLOB"
                                                        ToolTip="Line of Business" ValidationGroup="btnGo" InitialValue="-1" CssClass="styleMandatoryLabel"
                                                        ErrorMessage="Select Line Of Business" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr width="35%">
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblStartDateSearch" runat="server" CssClass="styleReqFieldLabel" Text="Cutt off Date"
                                                        ToolTip="Cutt off Date" />
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtStartDateSearch" runat="server" ToolTip="Cut off Date" Width="60%"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDateSearch"
                                                        CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Select Cutt off Date"
                                                        SetFocusOnError="True" ToolTip="Start Date" ValidationGroup="btnGo"></asp:RequiredFieldValidator>
                                                    <asp:Image ID="imgStartDateSearch" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                        PopupButtonID="imgStartDateSearch" TargetControlID="txtStartDateSearch">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td width="50%">
                                    <asp:Panel ID="pnlAccountInfo" runat="server" GroupingText="Account Informations"
                                        CssClass="stylePanel">
                                        <div id="divacc" runat="server" style="height: 150px; overflow: auto; width: 100%;
                                            display: none" align="center">
                                            <asp:GridView ID="grvprimeaccount" runat="server" AutoGenerateColumns="False" BorderWidth="2"
                                                OnRowDataBound="grvprimeaccount_RowDataBound" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sl.No." ItemStyle-Width="2%">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSNO" runat="server" Text="<%#Container.DataItemIndex+1%>" ToolTip="SI.NO"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Account No." ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMLA" runat="server" Text='<%#Eval("PRIMEACCOUNTNO")%>' ToolTip="Account No"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Account No." ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSLA" runat="server" Text='<%#Eval("SUBACCOUNTNO")%>' ToolTip="Sub Account No"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Select All" SortExpression="Sellect All">
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblSelectAll" runat="server" Text="Select All" ToolTip="Select All"></asp:Label>
                                                            <br />
                                                            <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                        </HeaderTemplate>
                                                        <HeaderStyle />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectAccount" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                                                ToolTip="Select Account" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <tr align="center">
                            <td width="100%">
                                <asp:Button ID="btnCalculate" runat="server" CssClass="styleSubmitButton" Text="Calculate"
                                    OnClientClick="return fnCheckPageValidators('btnCalculate',false);" OnClick="btnCalculate_Click"
                                    ValidationGroup="btnGo" ToolTip="Calculate Interest Query" />
                                <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" OnClientClick="return fnCheckPageValidators('btnGo',false);"
                                    OnClick="btnGo_Click" ValidationGroup="btnGo" Visible="false" />
                                <asp:Button ID="btnclear" runat="server" CssClass="styleSubmitButton" Text="Clear"
                                    OnClientClick="return fnConfirmClear();" OnClick="btnclear_Click" />
                            </td>
                        </tr>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td width="100%" colspan="4">
                                    <asp:Panel ID="pnlIRDetails" Visible="false" runat="server" Width="100%" CssClass="stylePanel"
                                        GroupingText="Income Calculation Details">
                                        <asp:GridView runat="server" AutoGenerateColumns="false" ShowFooter="false" AllowPaging="false"
                                            AllowSorting="false" ID="grvIncomeRecognition" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="PANum" ItemStyle-Wrap="true" ItemStyle-Width="120px" HeaderText="Account Number" />
                                                <asp:BoundField DataField="SANum" ItemStyle-Wrap="true" ItemStyle-Width="150px" HeaderText="Sub Account Number" />
                                                <asp:BoundField DataField="Interest_From_Date" HeaderText="Income From Date" HeaderStyle-Wrap="true" />
                                                <asp:BoundField DataField="Interest_To_Date" HeaderText="Income To Date" HeaderStyle-Wrap="true" />
                                                <asp:BoundField DataField="NoofDays" ItemStyle-HorizontalAlign="Right" HeaderText="No of Days" />
                                                <asp:BoundField DataField="Income_Amount" ItemStyle-HorizontalAlign="Right" HeaderText="Income Amount" />
                                            </Columns>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <RowStyle HorizontalAlign="Left" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr align="center">
                                <td width="100%">
                                    <asp:Button ID="btnPrint" runat="server" CssClass="styleSubmitButton" Text="Print"
                                        OnClientClick="return fnCheckPageValidators('btnGo',false);" OnClick="btnPrint_Click"
                                        ValidationGroup="btnGo" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4">
                                    <asp:ValidationSummary ID="vsIntrestQuery" runat="server" CssClass="styleMandatoryLabel"
                                        HeaderText="Correct the following validation(s):" ShowSummary="true" ShowMessageBox="false"
                                        ValidationGroup="btnGo" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        function fnLoadCustomer() {
            document.getElementById("<%= btnLoadCustomer.ClientID %>").click();

        }
        function fnSelectUser(chkSelect, chkSelectAll) {

            var grvprimeaccount = document.getElementById('ctl00_ContentPlaceHolder1_divacc_grvprimeaccount');
            var TargetChildControl = chkSelectAll;
            var selectall = 0;
            var Inputs = gvmail.getElementsByTagName("input");
            if (!chkSelectAccount.checked) {
                chkSelectAll.checked = false;
            }
            else {
                for (var n = 0; n < Inputs.length; ++n) {
                    if (Inputs[n].type == 'checkbox') {
                        if (Inputs[n].checked) {
                            selectall = selectall + 1;
                        }
                    }
                }
                if (selectall == grvprimeaccount.rows.length - 1) {
                    chkSelectAll.checked = true;
                }
            }


        }
        
    </script>

</asp:Content>
