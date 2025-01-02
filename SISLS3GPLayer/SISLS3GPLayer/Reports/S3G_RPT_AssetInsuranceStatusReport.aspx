<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3G_RPT_AssetInsuranceStatusReport.aspx.cs" MasterPageFile="~/Common/S3GMasterPageCollapse.master" Inherits="Reports_S3G_RPT_AssetInsuranceStatusReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>

<asp:Content ID="ContentAssetInsuranceStatusReport" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="udpAssetInsuranceStatusReport" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHeading" runat="server" EnableViewState="false" Text="Asset Insurance Status Deal wise report" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlAssetInsuranceStatusReport" runat="server" GroupingText="Filter Criteria"
                            CssClass="stylePanel">
                            <table width="100%" cellpadding="0" cellspacing="2px" border="0">
                                <tr>
                                    <td width="100%" colspan="4" align="center">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblDateFrom" runat="server" Text="RS Activation From Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign" style="width: 225px">
                                        <input id="hidDate" type="hidden" runat="server" />
                                        <asp:TextBox ID="txtDateFrom" runat="server" Width="140px" AutoPostBack="false" OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgDateFrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderDateFrom" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateFrom"
                                            TargetControlID="txtDateFrom">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel" style="width: 17%">
                                        <asp:Label ID="lblDateTo" runat="server" Text="RS Activation To Date" CssClass="styleDisplayLabel" />
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtDateTo" runat="server" Width="140px" AutoPostBack="false" OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgDateTo" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtenderDateTo" runat="server" Enabled="True"
                                            OnClientDateSelectionChanged="checkDate_NextSystemDate" PopupButtonID="imgDateTo"
                                            TargetControlID="txtDateTo">
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
                                <tr>
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
                        <table>
                        <tr>
                       <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Go" UseSubmitBehavior="true" CssClass="styleSubmitButton" OnClick="btnSearch_Click" />
                         </td>
                          <td>
                        <asp:Button ID="btnclear" runat="server" Text="Clear" UseSubmitBehavior="true" OnClientClick="return fnConfirmClear();" CssClass="styleSubmitButton" OnClick="btnclear_Click" />
                          </td>
                        <%--<td>
                        <asp:Button ID="btnPrint" runat="server" CssClass="styleSubmitButton" OnClick="btnPrint_Click" Text="Export" UseSubmitBehavior="true" Visible="false" />
                        </td>--%>
                         <td>
                        <asp:ImageButton ID="btnPrint" runat="server" CssClass="styleImgButton" ImageUrl="~/Images/excel_download.png" height="30px"  OnClick="btnPrint_Click" Text="Export"  Visible="true" />
                             </td>
                        </tr>
                       </table>
                    </td>
                </tr>
                <tr>
                    <td width="100%" align="center">
                        <asp:Panel ID="PnlAssetInsuranceStatus" runat="server" Visible="false" GroupingText="Asset Insurance Status Deal Wise Report Details" ScrollBars="Auto"
                            CssClass="stylePanel">
                            <%--<div id="divAssetInsuranceStatus" style="overflow-x: scroll; overflow-y: auto; width: 98%; height: 100%" runat="server">--%>
                                <asp:GridView ID="grvAssetInsuranceStatusReport" runat="server" Width="100%" EmptyDataText="No Record Found"
                                    RowStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" AutoGenerateColumns="true">
                                    <%--        <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" HeaderText="Sl.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSNo" runat="server" ToolTip="S.No" Width="30px" Text='<%#Bind("SNo")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderText="FinancialYear">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFY" ToolTip="FY" Width="100px" runat="server" Text='<%#Bind("FinancialYear") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="130px" HeaderStyle-HorizontalAlign="Center" HeaderText="Disbursement Month">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDisbursementMonth" ToolTip="Disbursement Month" Width="130px" runat="server" Text='<%#Bind("[Disbursement Month]") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" HeaderText="Tranche No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTrancheNo" runat="server" Width="115px" ToolTip="Tranche No" Text='<%#Bind("TrancheNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="110px" HeaderStyle-HorizontalAlign="Center" HeaderText="Rental Schedule No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRentalScheduleNo" runat="server" Width="110px" ToolTip="Rental Schedule No" Text='<%#Bind("RentalScheduleNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" HeaderText="Lessee Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLesseeName" runat="server" Width="160px" ToolTip="Lessee Name" Text='<%#Bind("LesseeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" HeaderText="Total Asset Value(In Rs.)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAssetValue" runat="server" Width="120px" ToolTip="Total Asset Value" Text='<%#Bind("Total Asset Value") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderText="Loss Payee Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLossPayeeName" runat="server" Width="100px" ToolTip="Loss Payee Name" Text='<%#Bind("LossPayeeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" HeaderText="Funder Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFunderName" runat="server" ToolTip="Funder Name" Width="85px" Text='<%#Bind("FunderName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="85px" HeaderStyle-HorizontalAlign="Center" HeaderText="Rental Start Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRentalStartDate" runat="server" ToolTip="Rental Start Date" Width="85px" Text='<%#Bind("Rental Start Dt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderText="Rental End Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRentalEndDate" runat="server" ToolTip="Rental End Date" Width="100px" Text='<%#Bind("Rental End Dt") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" HeaderText="Insurer">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInsurer" runat="server" Width="80px" ToolTip="Insurer" Text='<%#Bind("Insurer") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" HeaderText="Policy/Endorsement Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEndorsementNumber" runat="server" ToolTip="Policy/Endorsement Number" Text='<%#Bind("PolicyEndorsementNo") %>' Width="160px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" HeaderText="Policy Start Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicystartDate" runat="server" ToolTip="Policy Start Date" Text='<%#Bind("PolicyStartDate") %>' Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderText="Policy End Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicyEndDate" runat="server" ToolTip="Policy End Date" Text='<%#Bind("PolicyEndDate") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" HeaderText="Policy Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPolicyAmount" runat="server" ToolTip="Policy Amount" Text='<%#Bind("PolicyAmount") %>' Width="80px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Width="100px" Text='<%#Bind("Remarks") %>' ToolTip="Remarks"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160px" HeaderStyle-HorizontalAlign="Center" HeaderText="Insurance Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInsuranceStatus" runat="server" Width="160px" ToolTip="Insurance Status" Text='<%#Bind("Insurance Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       
                                    </Columns>--%>
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

