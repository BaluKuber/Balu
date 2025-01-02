<%@  Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" CodeFile="S3GRptRepaymentSchedule.aspx.cs" Inherits="Reports_S3GRptRepaymentSchedule" Title="Repayment Schedule" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" border="0">
        <tr>
            <td class="stylePageHeading">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel" Text="Repayment Schedule Report">
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    <table width="100%">
        <tr>
            <td width="50%">
                <asp:Panel ID="pnlCustomerInformation" runat="server" GroupingText="Customer Information" CssClass="stylePanel" Width="100%">
                    <table align="center" width="100%" border="0" cellspacing="0">
                        <tr>
                            <td class="styleFieldLabel" width="35%">
                                <asp:Label runat="server" ID="lblCustomerName" CssClass="styleReqFieldLabel" Text="Customer Name" ToolTip="Customer Name"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" colspan="2">
                                <asp:TextBox ID="txtCustomerCode" runat="server" Style="display: none;" MaxLength="50" ToolTip="Customer Name"></asp:TextBox>
                                <uc2:LOV ID="ucCustomerCodeLov" onfocus="return fnLoadCustomer();" runat="server" strLOV_Code="CMD" />
                                <asp:Button ID="btnLoadCustomer" runat="server" Text="Load Customer" OnClick="btnLoadCustomer_OnClick" Style="display: none" /><input id="hdnCustID" type="hidden" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" width="100%">
                                <uc1:S3GCustomerAddress ID="ucCustDetails" ShowCustomerName="false" runat="server" FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="height: 17px;" id="divSpace" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td width="50%">
                <asp:Panel ID="pnlRepayment" runat="server" GroupingText="Account Details" CssClass="stylePanel">
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" ToolTip="Line of Business">
                                </asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="30%">
                                <asp:DropDownList ID="ddlLOB" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged" ToolTip="Line of Business">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" ID="lblBranch" Text="Location1" ToolTip="Location1"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlBranch" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" ToolTip="Location1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" ID="lblLocation2" Text="Location2" ToolTip="Location2"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" width="30%">
                                <asp:DropDownList ID="ddlLocation2" runat="server"  AutoPostBack="true"  ToolTip="Location2" Enabled="false" OnSelectedIndexChanged="ddlLocation2_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" ID="lblPNum" CssClass="styleReqFieldLabel" Text="Account Number" ToolTip="Account Number"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="30%">
                                <asp:DropDownList ID="ddlPNum" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlPNum_SelectedIndexChanged" ToolTip="Account Number">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPNum" runat="server" ErrorMessage="Select Account Number" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlPNum">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" ID="lblSNum" Text="Sub Account Number" CssClass="styleDisplayLabel" ToolTip="Sub Account Number"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="30%">
                                <asp:DropDownList ID="ddlSNum" runat="server" ToolTip="Sub Account Number">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvSNum" runat="server" ErrorMessage="Select Sub Account Number" ValidationGroup="Ok" Display="None" SetFocusOnError="True" InitialValue="-1" ControlToValidate="ddlSNum" Enabled="false">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel" width="20%">
                                <asp:Label runat="server" ID="lblProduct" Text="Product" ToolTip="Product"></asp:Label>
                            </td>
                            <td class="styleFieldAlign" align="left" width="30%">
                                <asp:DropDownList ID="ddlProduct" runat="server" AutoPostBack="true" ToolTip="Product">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="styleFieldLabel">
                                <asp:Label runat="server" ID="lblPoff" Text="Print Off" Enabled="false" ToolTip="Print Off"></asp:Label>
                                <asp:CheckBox ID="chkPoff" runat="server" TextAlign="Right" AutoPostBack="true" OnCheckedChanged="chkPoff_CheckedChanged" Enabled="false" ToolTip="Print Off" />
                            </td>

                            <td class="styleFieldLabel" align="left">
                                  <asp:Label runat="server" ID="lblAmort" Text="Amortization" Enabled="false" ToolTip="Amortization"></asp:Label>
                                <asp:CheckBox ID="chkAmort" runat="server" TextAlign="Left" AutoPostBack="true"   OnCheckedChanged="chkAmort_CheckedChanged"
  Enabled="false" ToolTip="Amortization" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                <div style="height: 3px;" id="divSpace" runat="server">
                                </div>
                            </td>
                        </tr>--%>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="2" align="center">
                <asp:Button runat="server" ID="btnOk" CssClass="styleSubmitButton" Text="Go" CausesValidation="true" ValidationGroup="Ok" OnClientClick="return fnCheckPageValidators('Ok',false);" OnClick="btnOk_Click" ToolTip="Go" />
                &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton" Text="Clear" ToolTip="Clear" OnClick="btnClear_Click" OnClientClick="return fnConfirmClear();" />
                <%--&nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false" CssClass="styleSubmitButton" />--%>
            </td>
        </tr>
        <tr>
            <td align="right" colspan="2" width="99%">
                <asp:Label ID="lblAmounts" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
                <%--                <asp:Label ID="lblCurrency" runat="server" Visible="false" CssClass="styleDisplayLabel"></asp:Label>
--%>
            </td>
        </tr>
        <tr>
            <td colspan="2" width="100%">
                <asp:Panel ID="pnlAssetDetails" runat="server" CssClass="stylePanel" GroupingText="Asset Information" Visible="false">
                    <asp:Label ID="lblError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <asp:GridView ID="grvAssetDetails" runat="server" Width="100%" AutoGenerateColumns="false"  FooterStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center"  OnRowDataBound="grvAssetDetails_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Asset Desc." ItemStyle-HorizontalAlign="Left" ControlStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAssetDesc" runat="server" Text='<%# Bind("AssetDetails") %>' ToolTip="Asset Description"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SI/Reg. No." ItemStyle-HorizontalAlign="Left" ControlStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegNo" runat="server" Text='<%# Bind("SLRegNo") %>' ToolTip="SI/Reg No"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Finance Amt." ItemStyle-HorizontalAlign="Right" ControlStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblFinanceAmt" runat="server" Text='<%# Bind("AmountFinanced") %>' ToolTip="Finance Amount"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IRR" ItemStyle-HorizontalAlign="Right" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblIRR" runat="server" Text='<%# Bind("IRR") %>' ToolTip="IRR"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Terms" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblTerms" runat="server" Text='<%# Bind("Terms") %>' ToolTip="Terms"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ins. Policy No." ItemStyle-HorizontalAlign="Left" ControlStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyNo" runat="server" Text='<%# Bind("PolicyNo") %>' ToolTip="Insurance Policy No"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Valid Upto" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblValidUpto" runat="server" Text='<%# Bind("ValidUpto") %>' ToolTip="Valid Upto"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Insurer" ItemStyle-HorizontalAlign="Left" ControlStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label ID="lblInsurer" runat="server" Text='<%# Bind("Insurer") %>' ToolTip="Insurer"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Policy Amt." ItemStyle-HorizontalAlign="Right" ControlStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPolicyAmt" runat="server" Text='<%# Bind("PolicyAmount") %>' ToolTip="Policy Amount"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="2">
                <asp:Panel ID="pnlRepayDetails" runat="server" CssClass="stylePanel" GroupingText="Repayment Structure" Visible="false">
                    <asp:Label ID="lblgridError" runat="server" CssClass="styleDisplayLabel"></asp:Label>
                    <div id="divRepayDetails" runat="server" style="overflow: auto; height: 300px; display: none">
                        <asp:GridView ID="grvRepayDetails" runat="server" Width="100%" AutoGenerateColumns="false" FooterStyle-HorizontalAlign="Right" HeaderStyle-CssClass="styleGridHeader" OnRowDataBound="grvRepayDetails_RowDataBound" RowStyle-HorizontalAlign="Center" ShowFooter="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Installment No." FooterStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInstallmentNo"  runat="server" Text='<%# Bind("InstallmentNo") %>' ToolTip="Installment No"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblGrandTotal" runat="server" Text="Grand Total" ToolTip="Grand Total"></asp:Label>
                                    </FooterTemplate>
                                    <HeaderStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Due Date" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInstallmentDate" runat="server" Text='<%# Bind("InstallmentDate") %>' ToolTip="Installment Date"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Installment Amt." ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInstallmentAmt" runat="server" Text='<%# Bind("InstallmentAmount") %>' ToolTip="Installment Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInstallmentAmount" runat="server" ToolTip="Total Installment Amount"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Principal Amt." ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrincipalAmt" runat="server" Text='<%# Bind("PrincipalAmount") %>' ToolTip="Principal Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblPrincipalAmount" runat="server" ToolTip="Total Principal Amount"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Finance Charges" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFinanceCharges" runat="server" Text='<%# Bind("FinanceCharges") %>' ToolTip="Finance Charges"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblFinanceCharges" runat="server" ToolTip="Total Finance Charges"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UMFC" ItemStyle-HorizontalAlign="Right" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUMFC" runat="server" Text='<%# Bind("Umfc") %>' ToolTip="UMFC"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblUMFC" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Insurance Prem." ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInsuranceAmt" runat="server" Text='<%# Bind("InsuranceAmount") %>' ToolTip="Insurance Amount"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblInsuranceAmount" runat="server" ToolTip="Total Insurance Amount"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Others" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOthers" runat="server" Text='<%# Bind("Others") %>' ToolTip="Other Charges"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblOthers" runat="server" ToolTip="Total Other Charges"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VAT Recovery" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVatRec" runat="server" MaxLength="3" Text='<%# Bind("VatRecovery") %>' ToolTip="Vat Recovery"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblVatRecovery" runat="server" ToolTip="Total Vat Recovery"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="VAT Setoff" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVatSetoff" runat="server" MaxLength="3" Text='<%# Bind("TaxSetOff") %>' ToolTip="Vat Setoff"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTaxSetOff" runat="server" ToolTip="Total Vat Setoff"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Service Tax" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblServiceTax" runat="server" MaxLength="3" Text='<%# Bind("Tax") %>' ToolTip="Tax"></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTax" runat="server" ToolTip="Total Tax"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td height="8px">
            </td>
        </tr>
        <tr class="styleButtonArea" style="padding-left: 4px">
            <td colspan="2" align="center">
                <asp:Button runat="server" ID="btnPrint" CssClass="styleSubmitButton" Text="Print" CausesValidation="false" ValidationGroup="Print" OnClick="btnPrint_Click" Visible="false" ToolTip="Print" />
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="2">
                <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server" ErrorMessage="Select Customer Name" ValidationGroup="Ok" Enabled="true" Display="None" SetFocusOnError="True" ControlToValidate="txtCustomerCode">
                                                     
                </asp:RequiredFieldValidator>
                <asp:ValidationSummary ID="vsRepayment" runat="server" CssClass="styleMandatoryLabel" CausesValidation="true" HeaderText="Correct the following validation(s):" ValidationGroup="Ok" />
            </td>
        </tr>
        <tr>
            <td width="100%" colspan="2">
                <asp:CustomValidator ID="CVRepaymentSchedule" runat="server" Display="None" ValidationGroup="btnPrint"></asp:CustomValidator>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
    function fnLoadCustomer()
        {
        document.getElementById("<%= btnLoadCustomer.ClientID %>").click();

        }
        
    </script>

    <%--<style type="text/css">
    .Freezing
    {
      position:relative;
      top:auto;
      z-index:auto;
    }
  </style>--%>
</asp:Content>
