<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_RPT_InsuranceExpiredReport.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="Reports_S3G_RPT_InsuranceExpiredReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="ContentInsuranceExpiredReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpInsuranceExpiredReport" runat="server">
        <ContentTemplate>
              <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="Insurance Expiry Report" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlInsuranceExpiredReport" runat="server" GroupingText="Filter Criteria"
                            CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDateFrom" runat="server" Text="Expiry From Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <input id="hidDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtDateFrom" runat="server" Width="140px" AutoPostBack="false" 
                                            OnTextChanged="txtDateFrom_TextChanged" ></asp:TextBox>
                                        <asp:Image ID="imgDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderDateFrom" runat="server" Enabled="True"
                                            PopupButtonID="imgDateFrom"  TargetControlID="txtDateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblDateTo" runat="server" Text="Expiry To Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDateTo" runat="server" Width="140px" AutoPostBack="false" 
                                            OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderDateTo" runat="server" Enabled="True"
                                            PopupButtonID="imgDateTo" TargetControlID="txtDateTo">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCustomerName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <uc2:Suggest ID="ddlCustName" runat="server" ServiceMethod="GetCustomerName"
                                            ErrorMessage="Enter Customer Name" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 12%">
                                        <asp:Label ID="lblFunderName" runat="server" Text="Funder Name" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                         <uc2:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunderName" 
                                            ErrorMessage="Enter Funder Name" WatermarkText="--All--"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                    <%--                                <tr>
                                                                        <td class="styleFieldLabel">
                                                                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="styleDisplayLabel" />
                                                                        </td>
                                                                        <td class="styleFieldAlign" style="width: 225px">
                                                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="160px">
                                                                                <asp:ListItem Value="0" Text="--All--" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Value="1" Text="Insured"></asp:ListItem>
                                                                                <asp:ListItem Value="2" Text="Not Insured"></asp:ListItem>
                                                                                <asp:ListItem Value="3" Text="Expired"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td class="styleFieldLabel" style="width: 14%">&nbsp;
                                                                        </td>
                                                                        <td class="styleFieldAlign">&nbsp;
                                                                        </td>
                                                                    </tr>--%>
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
                        <asp:Button ID="btnSearch" runat="server" Text="Go" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnSearch_Click" />
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();" CssClass="styleSubmitButton" OnClick="btnclear_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Panel ID="PnlAssetInsuranceStatus" runat="server" Visible="false" GroupingText="Insurance Expiry Details" ScrollBars="Auto"
                            CssClass="stylePanel">
                            <%--<div id="divAssetInsuranceStatus" style="overflow-x: scroll; overflow-y: auto; width: 98%; height: 100%" runat="server">--%>
                                <asp:GridView ID="grvAssetInsuranceStatusReport" runat="server" Width="100%" EmptyDataText="No Record Found"
                                    RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" AutoGenerateColumns="true">
                                    <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                </asp:GridView>
                     <%--       </div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Button ID="btnPrint" runat="server" Text="Export" UseSubmitBehavior="true" Visible="false" CssClass="styleSubmitButton" OnClick="btnPrint_Click" />
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Label runat="server" Text="" ID="lblErrorMessage" CssClass="styleDisplayLabel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" id="hdnSearch" runat="server" />
                        <input type="hidden" id="hdnOrderBy" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="ddlCustName" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>