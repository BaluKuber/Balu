<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_RPT_LimitsTracker.aspx.cs" Inherits="Reports_S3G_RPT_LimitsTracker" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Limits Tracker Report">
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
                        <asp:Panel ID="pnlHeaderDetails" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                            <table cellpadding="0" cellspacing="0" style="width: 100%" align="center">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Funder Name" ID="lblFunderName" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="ddlFunderName" runat="server" ServiceMethod="GetFunderList" ToolTip="Funder Name"
                                            IsMandatory="false" />
                                        <%--ErrorMessage="Select the Funder Name" ValidationGroup="vgGo"--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Lessee Name" ID="lblLesseeName" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc3:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeList" ToolTip="Lessee Name"
                                            IsMandatory="false" />
                                        <%--ErrorMessage="Select the Lessee Name" ValidationGroup="vgGo" --%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Last Draw Down Start Date" ID="llblLastDate" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtLastDrawnDate" runat="server" MaxLength="12" ToolTip="Last Draw Down Start Date"></asp:TextBox>
                                        <asp:Image ID="imgLastDrawnDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                        <cc1:CalendarExtender ID="ceLastDrawnDate" runat="server" Enabled="true"
                                            PopupButtonID="imgLastDrawnDate" TargetControlID="txtLastDrawnDate">
                                        </cc1:CalendarExtender>
                                        <%-- <asp:RequiredFieldValidator Display="None" ID="rfvLastDrawnDate" CssClass="styleMandatoryLabel"
                                            SetFocusOnError="true" runat="server" ControlToValidate="txtLastDrawnDate"
                                            ErrorMessage="Enter the Last Draw Down Start Date" ValidationGroup="vgGo"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Last Draw Down End Date" ID="lblEndDate" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtEndDate" runat="server" MaxLength="12" ToolTip="Last Draw Down End Date"></asp:TextBox>
                                        <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                        <cc1:CalendarExtender ID="ceEndDate" runat="server" Enabled="true"
                                            PopupButtonID="imgEndDate" TargetControlID="txtEndDate">
                                        </cc1:CalendarExtender>
                                        <%--<asp:RequiredFieldValidator Display="None" ID="rfvEndDate" CssClass="styleMandatoryLabel"
                                            SetFocusOnError="true" runat="server" ControlToValidate="txtEndDate"
                                            ErrorMessage="Enter the Last Draw Down End Date" ValidationGroup="vgGo"></asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="llblsanctnfromDate" runat="server" Text="Sanction From Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtsanctnfromDate" runat="server" MaxLength="12" ToolTip="Sanction From Date"></asp:TextBox>
                                        <asp:Image ID="imgsanctnfromDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                        <cc1:CalendarExtender ID="ceSanctnFrmDate" runat="server" Enabled="true"
                                            PopupButtonID="imgsanctnfromDate" TargetControlID="txtsanctnfromDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblsanctntoDate" runat="server" Text="Sanction To Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtsanctntoDate" runat="server" MaxLength="12" ToolTip="Sanction To Date"></asp:TextBox>
                                        <asp:Image ID="imgsanctntoDate" runat="server" ImageUrl="~/Images/calendaer.gif" Visible="false" />
                                        <cc1:CalendarExtender ID="ceSanctnToDate" runat="server" Enabled="true"
                                            PopupButtonID="imgsanctntoDate" TargetControlID="txtsanctntoDate">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Limit Availability" ID="lblLmtAvailability" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLmtAvailability" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator Display="None" ID="rfvLimitAvailability" CssClass="styleMandatoryLabel"
                                            SetFocusOnError="true" runat="server" ControlToValidate="ddlLmtAvailability" InitialValue="0"
                                            ErrorMessage="Select the Limit Availability" ValidationGroup="vgGo"></asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr class="styleButtonArea" style="padding-left: 4px">
                                    <td colspan="4" align="center">
                                        <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" Text="Go" ToolTip="Go" OnClick="btnGo_Click" ValidationGroup="vgGo" />
                                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear"
                                            OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlRptDetails" runat="server" CssClass="stylePanel" GroupingText="Limits Tracker Details" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1050px">
                                <asp:GridView ID="grvLmtTrackDtls" runat="server" Width="1300px" AutoGenerateColumns="false"
                                    RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Funder S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvFndrSno" runat="server" Text='<%# Bind("Funder_SNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Funder Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvFndrName" runat="server" Text='<%# Bind("Funder_Name") %>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="180px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lessee S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvLesseeSno" runat="server" Text='<%# Bind("Lessee_SNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lessee Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvLesseeName" runat="server" Text='<%# Bind("Lessee_Name") %>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="180px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Lessee's End User Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvEndCustomerName" runat="server" Text='<%# Bind("EndCustomer_Name") %>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Tranche Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTrancheName" runat="server" Text='<%# Bind("Tranche_Name") %>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sanction No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvSanction_No" runat="server" Text='<%# Bind("Sanctioned_No") %>' Style="word-break: break-all; word-wrap: normal"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Limits Sanctioned">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvSanctionedLimit" runat="server" Text='<%# Bind("Sanctioned_Limit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Transfer Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTransfer_Amount" runat="server" Text='<%# Bind("Transfer_Amount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date of Sanction">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvSanctionDate" runat="server" Text='<%# Bind("Sanctioned_Date") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Note Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvNoteNumber" runat="server" Text='<%# Bind("Note_Number") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Utilized">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvUtilizedLimit" runat="server" Text='<%# Bind("Utilized_Limit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Limits Available">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvBalanceLimit" runat="server" Text='<%# Bind("Balance_Limit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Deal Pipeline">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvDealPipeline" runat="server" Text='<%# Bind("Deal_Pipeline") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Asset Category">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvAssetCategory" runat="server" Text='<%# Bind("Asset_Category") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tenor in Months">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvTenor" runat="server" Text='<%# Bind("Tenor") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Date of Draw Down">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgvDrawdownDate" runat="server" Text='<%# Bind("Expiry_Date") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Left"/>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server" Visible="true"></uc1:PageNavigator>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <span runat="server" id="lblPagingErrorMessage" style="color: Red; font-size: medium"></span>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td colspan="4" align="center">
                        <asp:Button ID="btnExport" runat="server" CssClass="styleSubmitButton" Text="Export" ToolTip="Export"
                            Visible="false" OnClick="btnExport_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vgGo" ValidationGroup="vgGo"
                            HeaderText="Please correct the following validation(s):" Height="400px" CssClass="styleMandatoryLabel"
                            Width="500px" ShowMessageBox="false" ShowSummary="true" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

