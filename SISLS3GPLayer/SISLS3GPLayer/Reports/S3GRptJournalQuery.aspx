<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptJournalQuery.aspx.cs" Inherits="Reports_S3GRptJournalQuery" Title="Journal Query" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc3" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function PANum_ItemSelected(sender, e) {
            var hdnPANum = $get('<%= hdnPANum.ClientID %>');
            hdnPANum.value = e.get_value();
         }

         function PANum_ItemPopulated(sender, e) {
             var hdnPANum = $get('<%= hdnPANum.ClientID %>');
             hdnPANum.value = '';
         }
        function GLAccount_ItemSelected(sender, e) {
            var hdnGLAccount = $get('<%= hdnGLAccount.ClientID %>');
            hdnGLAccount.value = e.get_value();
        }

        function GLAccount_ItemPopulated(sender, e) {
            var hdnGLAccount = $get('<%= hdnGLAccount.ClientID %>');
            hdnGLAccount.value = '';
         }
    </script>

    <asp:UpdatePanel ID="upAddress" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Journal Query Report">
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
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel" ToolTip="Line of Business">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="true" Width="65%" ToolTip="Line of Business" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ErrorMessage="Select Line of Business" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlLOB">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Location1" ID="lblBranch" ToolTip="Location" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true" Width="65%" ToolTip="Location1" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <%--                                <asp:RequiredFieldValidator ID="rfvBranch" runat="server" ErrorMessage="Select Location1" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlBranch">
                                </asp:RequiredFieldValidator>
                                        --%>                            </td>
                                </tr>
                                <tr>
                                    <td height="8px"></td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Location2" ID="lblLocation2" ToolTip="Location2" CssClass="styleDisplayLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlLocation2" runat="server" AutoPostBack="true" Width="65%" ToolTip="Location2" Enabled="false" OnSelectedIndexChanged="ddlLocation2_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Location" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlBranch">
                                </asp:RequiredFieldValidator>
                                        --%></td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" ID="lblPNum" CssClass="styleDisplayLabel" Text="Rental Schedule No." ToolTip="Rental Schedule No."></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" align="left" width="30%">
                                        <%-- <asp:DropDownList ID="ddlPNum" runat="server" Width="65%" AutoPostBack="true" OnSelectedIndexChanged="ddlPNum_SelectedIndexChanged" ToolTip="Rental Schedule  No.">
                                </asp:DropDownList>--%>
                                        <%--<asp:RequiredFieldValidator ID="rfvPNum" runat="server" ErrorMessage="Select Account Number" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlPNum">
                                </asp:RequiredFieldValidator>--%>
                                        <%--<uc2:Suggest ID="ddlPNum" runat="server" AutoPostBack="true" ServiceMethod="GetRentalScheduleNo" OnItem_Selected="ddlPNum_SelectedIndexChanged" WatermarkText="--All--" />--%>
                                        <asp:TextBox ID="txtPANum" runat="server" OnTextChanged="txtPANum_TextChanged"
                                            AutoPostBack="true" Width="65%"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="acePANum" MinimumPrefixLength="3" OnClientPopulated="PANum_ItemPopulated"
                                            OnClientItemSelected="PANum_ItemSelected" runat="server" TargetControlID="txtPANum"
                                            ServiceMethod="GetRentalScheduleNo" Enabled="True" CompletionSetCount="5"
                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </cc1:AutoCompleteExtender>
                                        <cc1:TextBoxWatermarkExtender ID="twmePANum" runat="server" TargetControlID="txtPANum"
                                            WatermarkText="--All--">
                                        </cc1:TextBoxWatermarkExtender>
                                        <asp:HiddenField ID="hdnPANum" runat="server" />
                                    </td>

                                </tr>
                                <tr>
                                    <td height="8px"></td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" ID="lblGLAccount" Text="GL Account" CssClass="styleDisplayLabel" ToolTip="GL Account"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" align="left" width="30%">
                                        <%--<asp:DropDownList ID="ddlGLAccount" runat="server" Width="65%" AutoPostBack="true" ToolTip="GL Account" OnSelectedIndexChanged="ddlGLAccount_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                                        <%--<asp:RequiredFieldValidator ID="rfvGLAccount" runat="server" ErrorMessage="Select GL Account Number" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlGLAccount">
                                </asp:RequiredFieldValidator>--%>
                                        <asp:TextBox ID="txtGLAccount" runat="server" OnTextChanged="txtGLAccount_TextChanged"
                                            AutoPostBack="true" Width="65%"></asp:TextBox>
                                        <cc1:AutoCompleteExtender ID="aceGLAccount" MinimumPrefixLength="3" OnClientPopulated="GLAccount_ItemPopulated"
                                            OnClientItemSelected="GLAccount_ItemSelected" runat="server" TargetControlID="txtGLAccount"
                                            ServiceMethod="GetGLAccount" Enabled="True" CompletionSetCount="5"
                                            CompletionListCssClass="CompletionList" DelimiterCharacters=";, :" CompletionListItemCssClass="CompletionListItemCssClass"
                                            CompletionListHighlightedItemCssClass="CompletionListHighlightedItemCssClass"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </cc1:AutoCompleteExtender>
                                        <cc1:TextBoxWatermarkExtender ID="twmeGLAccount" runat="server" TargetControlID="txtGLAccount"
                                            WatermarkText="--All--">
                                        </cc1:TextBoxWatermarkExtender>
                                        <asp:HiddenField ID="hdnGLAccount" runat="server" />
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Denomination" ID="lblDenomination" CssClass="styleDisplayLabel" ToolTip="Denomination">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:DropDownList ID="ddlDenomination" runat="server" Width="65%" ToolTip="Denomination">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="rfvDenomination" runat="server" ErrorMessage="Select Denomination" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="1" ControlToValidate="ddlDenomination">
                                </asp:RequiredFieldValidator>--%>
                                    </td>

                                </tr>
                                <tr>
                                    <td height="8px"></td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="Start Date" ID="lblStartDate" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtStartDate" runat="server" Width="56%"></asp:TextBox>
                                        <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" PopupButtonID="imgStartDate" ID="CalendarExtender1" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Enter the Start Date." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" Text="End Date" ID="lblEndDate" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" width="30%">
                                        <asp:TextBox ID="txtEndDate" runat="server" Width="56%"></asp:TextBox>
                                        <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate" PopupButtonID="imgEndDate" ID="CalendarExtender2" OnClientDateSelectionChanged="checkDate_NextSystemDate">
                                        </cc1:CalendarExtender>
                                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="Enter the End Date." ValidationGroup="Ok" Display="None" SetFocusOnError="True" ControlToValidate="txtEndDate">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="8px"></td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" ID="lblSNum" Text="Sub Account Number" CssClass="styleDisplayLabel" ToolTip="Sub Account Number" Visible="false"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" align="left" width="30%">
                                        <asp:DropDownList ID="ddlSNum" runat="server" Width="65%" AutoPostBack="true" OnSelectedIndexChanged="ddlSNum_SelectedIndexChanged" ToolTip="Sub Account Number" Visible="false">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="rfvSNum" runat="server" ErrorMessage="Select Sub Account Number" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlSNum" Enabled="false">
                                </asp:RequiredFieldValidator>--%>
                                    </td>
                                    <td class="styleFieldLabel" width="20%">
                                        <asp:Label runat="server" ID="lblLAN" CssClass="styleDisplayLabel" Text="Lease Asset Number" ToolTip="Lease Asset Number" Enabled="false" Visible="false"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" align="left" width="30%">
                                        <asp:DropDownList ID="ddlLAN" runat="server" Width="65%" ToolTip="Lease Asset Number" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlLAN_SelectedIndexChanged" Visible="false">
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="rfvLAN" runat="server" ErrorMessage="Select Lease Asset Number" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlLAN">
                                </asp:RequiredFieldValidator>--%>
                                    </td>

                                </tr>
                                <tr>
                                    <td height="8px"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td height="8px"></td>
                </tr>
                <tr class="styleButtonArea" style="padding-left: 4px">
                    <td colspan="4" align="center">
                        <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" OnClientClick="return fnCheckPageValidators('Ok',false);" Text="GO" CausesValidation="true" ValidationGroup="Ok" OnClick="btnOk_Click" />
                        &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlJournalDetails" runat="server" CssClass="stylePanel" GroupingText="Journal Details" Width="100%" Visible="false">
                            <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                            <%--<div id="divJournalDetails" runat="server" style="overflow: auto; height: 300px; display: none">--%>
                            <asp:GridView ID="grvJournalDetails" runat="server" Width="100%" OnRowDataBound="gv_Narration" AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" RowStyle-HorizontalAlign="Center" CellPadding="0" CellSpacing="0" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Doc Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocDate" runat="server" Text='<%# Bind("DocumentDate") %>'></asp:Label>
                                            <asp:HiddenField ID="HidNarration" Value='<%#Bind("Narration") %>' runat="server" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblGrandTotal" runat="server" Text="Total"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                        <FooterStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Value Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblvaluedate" runat="server" Text='<%# Bind("ValueDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc Reference">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocumentReference" runat="server" Text='<%# Bind("DocumentReference") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="GL Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGLAccount" runat="server" Text='<%# Bind("GlAccount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SL Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSLAccount" runat="server" Text='<%# Bind("SLAccount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rental Schedule No.">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPANum" runat="server" Text='<%# Bind("PrimeAccountNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CashFlow Flag">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCFFlag" runat="server" Text='<%# Bind("Due_Flag") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Sub Account No" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSANum" runat="server" Text='<%# Bind("SubAccountNo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                    <%-- <asp:TemplateField HeaderText="LAN">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLAN" runat="server" Text='<%# Bind("Lan") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Debit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDues" runat="server" Text='<%# Bind("Dues") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalDues" runat="server" ToolTip="sum of Dues Amount"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Credit">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReceipts" runat="server" Text='<%# Bind("Receipts") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalReceipts" runat="server" ToolTip="sum of  Receipts Amount"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <uc3:PageNavigator ID="ucCustomPaging" runat="server"></uc3:PageNavigator>
                            <%--</div>--%>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea" style="padding-left: 4px">
                    <td colspan="4" align="center">
                        <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click" />
                        <asp:Button runat="server" ID="BtnExcel" CssClass="styleSubmitButton" Text="Export To Excel" CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnExcel_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsJournal" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="CVJournalDetails" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
