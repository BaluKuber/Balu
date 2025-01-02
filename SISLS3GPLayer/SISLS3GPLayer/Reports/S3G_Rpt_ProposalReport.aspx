<%@ Page Title="" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3G_Rpt_ProposalReport.aspx.cs" Inherits="Reports_S3G_Rpt_ProposalReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Proposal Report">
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
                        <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Input Criteria" CssClass="stylePanel" Width="100%">
                            <table align="center" width="100%" border="0" cellspacing="0">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStartDate" runat="server" Text="From Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="true"   OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartDate">
                                        </cc1:CalendarExtender>
                                       <%-- <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter From Date" ValidationGroup="Go" CssClass="styleMandatoryLabel"
                                            Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblEnddate" runat="server" Text="To Date" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtenddate" runat="server" AutoPostBack="true"    OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                                        <asp:Image ID="Imgenddate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtenddate" PopupButtonID="Imgenddate">
                                        </cc1:CalendarExtender>
                                        <%-- <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Enter To Date" ValidationGroup="Go" Display="None" SetFocusOnError="True"
                                            CssClass="styleMandatoryLabel" ControlToValidate="txtEndDate">
                                        </asp:RequiredFieldValidator>--%>
                                    </td>

                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleDisplayLabel" ToolTip="Lessee Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" Width="280px" ServiceMethod="GetLesseeNameDetails"
                                            WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLocation" runat="server" Text="Proposal Number" CssClass="styleDisplayLabel" ToolTip="State">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlProposalNo" runat="server" ServiceMethod="GetProposalNo"
                                            WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblTrancheNo" runat="server" Text="Tranche Name" CssClass="styleDisplayLabel" ToolTip="Tranche Name">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlTrancheNo" runat="server" ServiceMethod="GetTrancheNo" WatermarkText="--All--" />
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblRSNo" runat="server" Text="SCH No." CssClass="styleDisplayLabel" ToolTip="SCH No.">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlRSNo" runat="server" ServiceMethod="GetRSNo" WatermarkText="--All--" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStatus" runat="server" Text="Proposal Type"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlProposalType" runat="server" Width="170px">
                                          </asp:DropDownList>
                                    </td>
                                 <td class="styleFieldLabel">
                                        <asp:Label ID="Label1" runat="server" Text="Valid Till" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtValid_ason" runat="server"></asp:TextBox>
                                        <asp:Image ID="imgValid_ason" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtValid_ason" PopupButtonID="imgValid_ason">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <%--<td class="styleFieldLabel">
                                        <asp:Label ID="lblInvoiceNo" runat="server" Text="Invoice No"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="ddlInvoiceNo" runat="server" ServiceMethod="GetInvoiceNo" WatermarkText="--All--" />
                                    </td>--%>
                                </tr>
                                 <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblProposalStatus" runat="server" Text="Proposal Status"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" colspan="3">
                                        <asp:DropDownList ID="ddlProposalStatus" runat="server" Width="170px">
                                            <asp:ListItem Text="Pending" Value="44"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="47"></asp:ListItem>
                                            <asp:ListItem Text="--Select--" Value="0" Selected="True"></asp:ListItem>
                                          </asp:DropDownList>
                                    </td>
                                 </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" CausesValidation="true" ValidationGroup="Go" ToolTip="Go" OnClick="btnOk_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                         <asp:Button runat="server" ID="btnExportExcel_1" CssClass="styleSubmitButton" Text="Export Excel" CausesValidation="false" ValidationGroup="Export" OnClick="btnExportExcel_Click" Visible="false" ToolTip="Export Excel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlVAT" runat="server" CssClass="stylePanel" GroupingText="Proposal Report" Visible="false" Width="100%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 1160px;">
                                <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                                <asp:GridView ID="grvGST" runat="server" OnRowDataBound="grvGST_RowDataBound" Width="100%" FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center">
                                </asp:GridView>
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                       
                        <asp:Button runat="server" ID="btnExportExcel" CssClass="styleSubmitButton" Text="Export Excel" CausesValidation="false"
                            ValidationGroup="Export" OnClick="btnExportExcel_Click"  OnClientClick="return fnExcelExportVal(this)" Visible="false" ToolTip="Export Excel" />
                        
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:ValidationSummary ID="vsRepayment" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Go" />
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                        <asp:CustomValidator ID="CVRepaymentSchedule" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 70;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 275;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
        }

        function fnExcelExportVal(btn) {
            //btn.style.visibility = 'hidden';
            btn.disabled = true;
            //var a = event.srcElement;
            //a.style.display = 'block';
            //a.style.removeAttribute('display');
            return true;
         }

    </script>
</asp:Content>

