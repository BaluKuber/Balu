<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRptPDCAcknowledgement.aspx.cs" Inherits="Reports_S3GRptPDCAcknowledgement"
    Title="PDC Acknowledgement" %>

<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="PDC Acknowledgement Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="PnlInputCriteria" runat="server" GroupingText="Input Criteria" CssClass="stylePanel">
                    <table cellpadding="0" cellspacing="0" style="width: 100%" border="0">
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" ID="lblCustomerName" CssClass="styleReqFieldLabel" Text="Customer Name" ToolTip="Customer Name"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:TextBox ID="txtCustomerName" runat="server" Style="display: none;" MaxLength="80"
                                   Width="80%" CausesValidation="true" ToolTip="Customer Name"></asp:TextBox>
                                <uc2:LOV ID="ucCustomerCodeLov" onfocus="return fnLoadCustomer();" runat="server"
                                    strLOV_Code="CMD" />
                                <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" OnClick="btnLoadCustomer_OnClick"
                                    Style="display: none"  ToolTip="Customer Name"/>
                                <input id="hdnCustID" type="hidden" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvCustomer" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="txtCustomerName" ErrorMessage="Select Customer Name" Display="None"
                                    ValidationGroup="Go" SetFocusOnError="True" Enabled="true">
                                </asp:RequiredFieldValidator>
                            </td>
                             <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel" ToolTip="Line of Business">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlLOB" runat="server" Width="95%" AutoPostBack="true" 
                                    ToolTip="Line of Business" onselectedindexchanged="ddlLOB_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblBranch" Text="Location 1" CssClass="styleDisplayLabel" ToolTip="Location 1"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:DropDownList ID="ddlBranch" runat="server" Width="75%" AutoPostBack="true" 
                                    ToolTip="Location 1" onselectedindexchanged="ddlBranch_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                    <asp:Label ID="lbllocation" runat="server" Text="Location 2" CssClass="styleDisplayLabel" ToolTip="Location 2">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign" width="25%">
                                    <asp:DropDownList ID="ddllocation2" runat="server" OnSelectedIndexChanged="ddllocation2_SelectedIndexChanged"  AutoPostBack="true" Visible="true" Width="95%" ToolTip="Location 2">
                                    </asp:DropDownList>
                                </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblPNum" CssClass="styleReqFieldLabel" Text="Account Number" ToolTip="Account Number"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlPNum" runat="server" Width="75%" AutoPostBack="true" ToolTip="Account Number" OnSelectedIndexChanged="ddlPNum_SelectedIndexChanged1">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPNum" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlPNum" ErrorMessage="Select Account Number" Display="None" ValidationGroup="Go"
                                    SetFocusOnError="True" Enabled="true">
                                </asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlPNum" ErrorMessage="Select Account Number" Display="None" ValidationGroup="Go"
                                    SetFocusOnError="True" Enabled="true" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label runat="server" ID="lblSNum" Text="Sub Account Number" CssClass="styleDisplayLabel" ToolTip="Sub Account Number"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlSNum" runat="server" Width="95%" OnSelectedIndexChanged ="ddlSNum_SelectedIndexChanged" AutoPostBack="true" ToolTip="Sub Account Number">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label ID="lblPDCNo" runat="server"  Text="PDC NO" CssClass="styleReqFieldLabel" ToolTip="PDC NO"></asp:Label>
                                
                            </td>
                            <td class="styleFieldAlign" align="left" width="25%">
                                <asp:DropDownList ID="ddlPDCNo" runat="server" Width="75%" AutoPostBack="true" OnSelectedIndexChanged="ddlPDCNo_SelectedIndexChanged" ToolTip="PDC NO">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPDCNo" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlPDCNo" ErrorMessage="Select PDC NO" Display="None" ValidationGroup="Go"
                                     SetFocusOnError="True" Enabled="true" >
                                </asp:RequiredFieldValidator>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="ddlPDCNo" ErrorMessage="Select PDC NO" Display="None" ValidationGroup="Go"
                                     SetFocusOnError="True" Enabled="true" InitialValue="-1" >
                                </asp:RequiredFieldValidator>
                            </td>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label ID="lblPDCDate" runat="server" Text="PDC Date" CssClass="styleDisplayLabel" ToolTip="PDC Date"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="25%">
                                <asp:TextBox ID="txtPDCDate" runat="server" Width="85%" CausesValidation="true" ToolTip="PDC Date"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="rfvDt" CssClass="styleMandatoryLabel" runat="server"
                                    ControlToValidate="txtPDCDate" Display="None" ValidationGroup="Go" Enabled="false">
                                </asp:RequiredFieldValidator>--%>
                                <%--ErrorMessage="Display PDC Date"--%> 
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="25%">
                                <asp:Label ID="lblDOCPath" runat="server" Text="DOC Path" CssClass="styleDisplayLabel" ToolTip="DOC Path"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width=25%>
                                <asp:TextBox ID="txtDOCPath" runat="server" Width="90%" CausesValidation="true" ReadOnly="true" ToolTip="DOC Path"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="rfvDocPath" CssClass="styleMandatoryLabel" runat="server" 
                                ControlToValidate="txtDOCPath" Display="None" ValidationGroup="Go" Enabled="false"> 
                                </asp:RequiredFieldValidator>--%>
                                <%--ErrorMessage="Display DOC Path"--%>
                            </td>
                            <td class="styleFieldAlign" width=25%>
                            </td>
                             <td class="styleFieldAlign" width=25%>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="10px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td height="8px" align="center">
                <asp:Button ID="btnGo" runat="server" Text="Go" CssClass="styleSubmitButton" ValidationGroup="Go"
                    CausesValidation="true" OnClientClick="return fnCheckPageValidators('Go',false);" OnClick="btnGo_Click" ToolTip="Go" />
                &nbsp; &nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="styleSubmitButton"
                    CausesValidation="false" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" ToolTip="Clear" />
            </td>
        </tr>
        <tr>
            <td height="10px">
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:Label ID="lblAmounts" runat="server" Text="All Amounts are in" Visible="false"
                    CssClass="styleDisplayLabel"></asp:Label>
                <asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlPdcdetails" runat="server" CssClass="stylePanel" GroupingText="PDC Details"
                    Width="100%" Visible="false">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:GridView ID="grvPdcdetails" runat="server" AutoGenerateColumns="False" CssClass="styleInfoLabel"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="PDC Serial No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPdcSNo" runat="server" Text='<%# Bind("PDCSNo") %>' ToolTip="PDC Serial No"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque Number">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeNumber" runat="server" Text='<%# Bind("ChequeNumber") %>' ToolTip="Cheque Number"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cheque Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblChequeDate" runat="server" Text='<%# Bind("ChequeDate") %>' ToolTip="Cheque Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Drawn on Bank">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDrawnbank" runat="server" Text='<%# Bind("DrawnonBank") %>' ToolTip="Drawn on Bank"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Banking Date" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBankingDate" runat="server" Text='<%# Bind("BankingDate") %>' ToolTip="Banking Date"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>' ToolTip="Amount"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="10px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td align="center">
                &nbsp; &nbsp;
                <asp:Button ID="btnSave" runat="server" Text="Save PDC" CssClass="styleSubmitButton"
                    Visible="false" CausesValidation="false" OnClick="btnSave_Click" ToolTip="Save PDC" />
                &nbsp; &nbsp;
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="styleSubmitButton"
                    CausesValidation="false" ValidationGroup="Print" Visible="false" OnClick="btnPrint_Click" ToolTip="Print"/>
                     &nbsp; &nbsp;
                    <asp:Button ID="BtnEMail" runat="server" Text="EMail" CssClass="styleSubmitButton"
                    CausesValidation="false"  Visible="false" OnClick="BtnEMail_Click" ToolTip="EMail" Enabled="false"/>

            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary runat="server" ID="VSPDCAcknow" HeaderText="Please correct the following validation(s):"
                    CssClass="styleSummaryStyle" ShowMessageBox="false" ValidationGroup="Go" ShowSummary="true"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CustomValidator ID="CVPDCAcknowledgement" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
      function fnLoadCustomer() 
     {
        document.getElementById('<%=btnLoadCustomer.ClientID%>').click();
     }
    </script>

</asp:Content>
