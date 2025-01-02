<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptCollectionPerformance.aspx.cs" Inherits="Reports_S3GRptCollectionPerformance"
    Title="Collection Performance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .Freezing
        {
            position: relative;
            top: fixed;
            z-index: auto;
            left: auto;
        }
    </style>
    <asp:UpdatePanel ID="udpInsDetails" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Collection Performance Report">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlIntput" runat="server" GroupingText="Input Criteria" CssClass="stylePanel"
                            Width="99%">
                            <table width="98%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblLOB" runat="server" Text="Line of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:DropDownList ID="ddlLOB" runat="server" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RFVLOB" Enabled="true" CssClass="styleMandatoryLabel"
                                            runat="server" InitialValue="0" ControlToValidate="ddlLOB" ValidationGroup="vsDPD"
                                            ErrorMessage="Select the Line of Business" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblProduct" runat="server" Text="Product" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:DropDownList ID="ddlProduct" runat="server" Width="185px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblBranch" runat="server" Text="Location1" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:DropDownList ID="ddlBranch" runat="server" Width="185px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="LblLoc2" runat="server" CssClass="styleDisplayLabel" Text="Location2"></asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <asp:DropDownList ID="ddlLoc2" runat="server" Width="185px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="LblReportBasis" runat="server" CssClass="styleDisplayLabel" Text="Report Basis"> </asp:Label>
                                    </td>
                                    <td class="styleFieldLabel">
                                        <%--                                        <asp:CheckBox ID="rdbRptBasis" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rdbRptBasis_SelectedIndexChanged">--%>
                                        <asp:RadioButtonList ID="rdbRptBasis" runat="server" RepeatDirection="Horizontal"
                                            RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rdbRptBasis_SelectedIndexChanged">
                                            <asp:ListItem Text="Net of Cheque Return" Value="Net of ChequeReturn"></asp:ListItem>
                                            <asp:ListItem Text="Comparative Analysis" Value="Comparative Analysis">
                                            </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td class="styleFieldLabel">
                                    </td>
                                    <td class="styleFieldLabel">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlchkCollaection" runat="server" GroupingText="Search Criteria" CssClass="stylePanel"
                            Width="99%">
                            <table cellpadding="0" cellspacing="0" border="0" width="98%">
                                <%--<tr>
                                    <td>
                                        <asp:Label ID="lblNetofChequeReturn" runat="server" Text="Net of ChequeReturn" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkNetofChequeReturn" runat="server" AutoPostBack="True" OnCheckedChanged="chkNetofChequeReturn_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblComparativeAnalysis" runat="server" Text="Comparative Analysis"
                                            CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkComparativeAnalysis" runat="server" AutoPostBack="True" OnCheckedChanged="chkComparativeAnalysis_CheckedChanged" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlNormal" runat="server" GroupingText="Normal Date range" CssClass="stylePanel">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblNormalFromDate" runat="server" Text="From" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNormalFromDate" ContentEditable="false" AutoPostBack="true" runat="server"
                                                            Width="150px" OnTextChanged="FunDateValidation"></asp:TextBox>
                                                        <asp:Image ID="imgNormalFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                        <cc1:CalendarExtender runat="server" TargetControlID="txtNormalFromDate" PopupButtonID="imgNormalFromDate"
                                                            ID="calNormalFromDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="RqvtxtNormalFromDate" Enabled="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtNormalFromDate" ValidationGroup="vsDPD"
                                                            ErrorMessage="Enter the From Date in Normal Date Range" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblNormalToDate" runat="server" Text="To" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNormalToDate" ContentEditable="false" runat="server" AutoPostBack="true"
                                                            Width="150px" OnTextChanged="FunDateValidation"></asp:TextBox>
                                                        <asp:Image ID="imgNormalToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                        <cc1:CalendarExtender runat="server" Format="MM/yyyy" TargetControlID="txtNormalToDate"
                                                            PopupButtonID="imgNormalToDate" ID="calNormalToDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="RqvtxtNormalToDate" Enabled="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtNormalToDate" ValidationGroup="vsDPD" ErrorMessage="Enter the To Date in Normal Date Range"
                                                            Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Panel ID="PnlCompare" runat="server" CssClass="stylePanel" GroupingText="Comparative Date Range">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblComparativeFromDate" runat="server" Text="From" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtComparativeFromDate" ContentEditable="false" runat="server" Width="150px"></asp:TextBox>
                                                        <asp:Image ID="imgComparativeFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                        <cc1:CalendarExtender runat="server" Format="MM/yyyy" TargetControlID="txtComparativeFromDate"
                                                            PopupButtonID="imgComparativeFromDate" ID="calComparativeFromDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="RqvtxtComparativeFromDate" Enabled="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtComparativeFromDate" ValidationGroup="vsDPD"
                                                            ErrorMessage="Enter the From Date in Comparative Date Range" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblComparativeToDate" runat="server" Text="To" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtComparativeToDate" ContentEditable="false" runat="server" Width="150px"></asp:TextBox>
                                                        <asp:Image ID="imgComparativeToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                        <cc1:CalendarExtender runat="server" Format="MM/yyyy" TargetControlID="txtComparativeToDate"
                                                            PopupButtonID="imgComparativeToDate" ID="calComparativeToDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator ID="RqvtxtComparativeToDate" Enabled="true" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtComparativeToDate" ValidationGroup="vsDPD"
                                                            ErrorMessage="Enter the To Date in Comparative Date Range" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="5">
                        <table>
                            <tr class="styleButtonArea">
                                <td align="center">
                                    <asp:Button ID="btnGo" runat="server" CssClass="styleSubmitButton" OnClick="btnGo_Click"
                                        Text="Go" OnClientClick="return fnCheckPageValidators('vsDPD',false);" CausesValidation="true" ValidationGroup="vsDPD" />
                                    <asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                        Text="Clear" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="5">
                        <table>
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vsDPDRep" runat="server" CssClass="styleMandatoryLabel"
                                        CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="vsDPD" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CustomValidator ID="CVDPD" runat="server" Display="None" ValidationGroup="btnGo"></asp:CustomValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlCollectionAmount" runat="server" GroupingText="Collection Amount"
                            CssClass="stylePanel" Width="99%" ScrollBars="Horizontal" HorizontalAlign="Center">
                            <div id="divDemand" runat="server" style="overflow: auto; height: 300px;">
                                <asp:Label ID="lblCollAmtMsg" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="grvCollectionAmount" runat="server" AutoGenerateColumns="False"
                                    ShowFooter="true" OnRowCreated="grvCollectionAmount_RowCreated" OnPageIndexChanging="grvCollectionAmount_PageIndexChanging"
                                    OnSelectedIndexChanged="grvCollectionAmount_SelectedIndexChanged" PageSize="100">
                                    <Columns>
                                        <asp:TemplateField HeaderText="LOB">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Product")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prime Account No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PANum")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SANum")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Due Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Bind("INSTALLMENTDATE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Receipt Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueDate" runat="server" Text='<%# Bind("RECEIPTDATE")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Due Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDue" runat="server" Text='<%# Bind("PERIOD")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NOD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNOD" runat="server" Text='<%# Bind("NOD")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                            <FooterStyle Width="25%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="0 - 30">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeing0to30" runat="server" Text='<%# Bind("Ageing0to30")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTotal0to30" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="31 - 60">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeing31to60" runat="server" Text='<%# Bind("Ageing31to60")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTotal31to60" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="61 - 90">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeing61to90" runat="server" Text='<%# Bind("Ageing61to90")%>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTotal61to90" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="&gt; 90 ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeingAbove90" runat="server" Text='<%# Bind("AgeingAbove90")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTotalAbove90" runat="server" ReadOnly="true" Width="150px" style="text-align:right" ></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance Due">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalDue" runat="server" Text='<%# Bind("BALDUE")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTotalBalDue" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print"
                                OnClick="btnPrint_Click" />
                        </asp:Panel>
                        <asp:Panel ID="pnlChequeReturn" runat="server" GroupingText="Cheque Return" CssClass="stylePanel"
                            Width="99%" ScrollBars="Horizontal" HorizontalAlign="Center">
                            <div id="div1" runat="server" style="overflow: auto; height: 300px;">
                                <asp:Label ID="lblChqRtnMsg" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="grvChequeReturn" runat="server" AutoGenerateColumns="False" OnRowCreated="grvChequeReturn_RowCreated"
                                    OnPageIndexChanging="grvChequeReturn_PageIndexChanging"
                                    OnSelectedIndexChanged="grvCollectionAmount_SelectedIndexChanged" ShowFooter="true"
                                    PageSize="100">
                                    <Columns>
                                        <asp:TemplateField HeaderText="LOB">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB_Name")%>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                            <FooterStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Product")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=" Prime Account No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PANum")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SANum")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                            <FooterStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Due Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Bind("Due_Date")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Receipt Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDueDate" runat="server" Text='<%# Bind("RECEIPTDATE")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Due Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDue" runat="server" Text='<%# Bind("Due_Amount")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="NOD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNOD" runat="server" Text='<%# Bind("NOD")%>' Width="30px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" Width="30px"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                            <FooterStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Collection Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeing0to30" runat="server" Text='<%# Bind("Ageing0to30")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCMTotal0to30" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque Return Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeReturn0to30" runat="server" Text='<%# Bind("Chq_Ageing0to30")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCRTotal0to30" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Collection Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeing31to60" runat="server" Text='<%# Bind("Ageing31to60")%>'
                                                   Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCMTotal31to60" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque Return Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeReturn31to60" runat="server" Text='<%# Bind("Chq_Ageing31to60")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCRTotal31to60" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Collection Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeing61to90" runat="server" Text='<%# Bind("Ageing61to90")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCMTotal61to90" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque Return Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeReturn61to90" runat="server" Text='<%# Bind("Chq_Ageing61to90")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCRTotal61to90" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Collection Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAgeingAbove90" runat="server" Text='<%# Bind("AgeingAbove90")%>'
                                                   Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCMTotalAbove90" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque Return Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeReturnAbove90" runat="server" Text='<%# Bind("Chq_AgeingAbove90")%>'
                                                   Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtCRTotalAbove90" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Right" />
                                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:Button runat="server" ID="btnChkPrint" CssClass="styleSubmitButton" Text="Print"
                                OnClick="btnChkPrint_Click" />
                        </asp:Panel>
                        <asp:Panel ID="pnlComparative" runat="server" GroupingText="Comparative Analysis"
                            CssClass="stylePanel" Width="99%" ScrollBars="Horizontal" HorizontalAlign="Center">
                            <div id="div2" runat="server" style="overflow: auto; height: 300px;">
                                <asp:Label ID="lblAnlyAmtMsg" runat="server" Visible="false"></asp:Label>
                                <asp:GridView ID="GrvComparativeAnalysis" runat="server" AutoGenerateColumns="false"
                                    OnRowCreated="GrvComparativeAnalysis_RowCreated" Width="100%" OnPageIndexChanging="GrvComparativeAnalysis_PageIndexChanging"
                                    ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="LOB">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("CustomerName")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Location_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocation" runat="server" Text='<%# Bind("Product")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Prime Account No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("AccountNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="25%" />
                                            <FooterStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub Account No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SubAccountNumber")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" Wrap="true" />
                                            <HeaderStyle Width="10%" />
                                            <FooterStyle Width="10%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Period Due">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFirstDue" runat="server" Text='<%# Bind("FstPrdDue")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtFirstDue" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right"  Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right"  Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Second Period Due">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSecondDue" runat="server" Text='<%# Bind("SndPrdDue")%>' Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtSecondDue" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right"  Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right"  Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl030FstPrdClnAmnt" runat="server" Text='<%# Bind("FstprdClnAmt030")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt030FstPrdClnTotal" ReadOnly="true" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Second Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl030SndPrdClnAmnt" runat="server" Text='<%# Bind("SndprdClnAmt030")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt030SndPrdClnTotal" ReadOnly="true" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right"  Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right" Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl3160FstPrdClnAmnt" runat="server" Text='<%# Bind("FstprdClnAmt3160")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt3160FstPrdClnTotal" ReadOnly="true" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right" Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Second Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl3160SndPrdClnAmnt" runat="server" Text='<%# Bind("SndprdClnAmt3160")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt3160SndPrdClnTotal" runat="server" ReadOnly="true" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right" Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl6190FstPrdClnAmnt" runat="server" Text='<%# Bind("FstprdClnAmt6190")%>'
                                                   Width="150px" ></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt6190FstPrdClnTotal" ReadOnly="true" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right" Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Second Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl6190SndPrdClnAmnt" runat="server" Text='<%# Bind("SndprdClnAmt6190")%>'
                                                   Width="150px" ></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txt6190SndPrdClnTotal" ReadOnly="true" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right" Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAbv90FstPrdClnAmnt" runat="server" Text='<%# Bind("FstprdClnAmtAbv90")%>'
                                                   Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtAbv90FstPrdClnTotal" ReadOnly="true" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right" Width="25%"/>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Second Period Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAbv90SndPrdClnAmnt" runat="server" Text='<%# Bind("SndprdClnAmtAbv90")%>'
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtAbv90SndPrdClnTotal" ReadOnly="true" runat="server" Width="150px" style="text-align:right"></asp:TextBox>
                                            </FooterTemplate>
                                            <ItemStyle HorizontalAlign="Right" Width="25%"/>
                                            <FooterStyle HorizontalAlign="Right" Width="25%"/>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:Button runat="server" ID="btnAnlyPrint" CssClass="styleSubmitButton" Text="Print"
                                OnClick="btnAnlyPrint_Click" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
