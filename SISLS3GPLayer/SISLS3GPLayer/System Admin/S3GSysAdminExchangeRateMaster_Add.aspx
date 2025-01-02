<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" CodeFile="S3GSysAdminExchangeRateMaster_Add.aspx.cs"
    Inherits="System_Admin_S3GSysAdminExchangeRateMaster_Add" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleInfoLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td colspan="2">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Enter the Date"
                                                    ControlToValidate="txtDate" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvDefaultCurrency" runat="server" ErrorMessage="Enter the Default Currency"
                                                    ControlToValidate="txtDefaultCurrency" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvDefaultValue" runat="server" ErrorMessage="Enter the Default Value"
                                                    ControlToValidate="txtDefaultValue" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvExchangeCurrency" runat="server" ErrorMessage="Select the Exchange Currency"
                                                    ControlToValidate="ddlExchangeCurrency" InitialValue="0" SetFocusOnError="true"
                                                    CssClass="styleMandatoryLabel" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvExchangeValue" runat="server" ErrorMessage="Enter the Exchange Value"
                                                    ControlToValidate="txtExchangeValue" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Exchange Rate Should be in the format 9999.99"
                                                    ControlToValidate="txtExchangeValue" SetFocusOnError="True" CssClass="styleMandatoryLabel"
                                                    ClientValidationFunction="CheckDecimal" Display="None"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <cc1:CalendarExtender ID="cexDate" runat="server" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                        TargetControlID="txtDate" PopupButtonID="imgCalRegNoValDate" Enabled="True">
                                    </cc1:CalendarExtender>
                                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtDefaultValue"
                                        FilterType="Numbers" Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                   
                                  
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label CssClass="styleReqFieldLabel" runat="server" ID="lblDate" Text="Date">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                    <asp:Image ID="imgCalRegNoValDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label CssClass="styleReqFieldLabel" runat="server" ID="lblDefaultCurrency" Text="Default Currency">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtDefaultCurrency" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label CssClass="styleReqFieldLabel" runat="server" ID="lblDefaultValue" Text="Default Value">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtDefaultValue" runat="server" MaxLength="6" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label CssClass="styleReqFieldLabel" runat="server" ID="lblExchangeCurrency"
                                        Text="Exchange Currency">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlExchangeCurrency" AutoPostBack ="true" OnSelectedIndexChanged="ddlExchangeCurrency_SelectedIndexChanged" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                    <asp:Label CssClass="styleReqFieldLabel" runat="server" ID="Label1" Text="Exchange Value">
                                    </asp:Label>
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:TextBox ID="txtExchangeValue" runat="server"></asp:TextBox><!--onkeypress="fnAllowNumbersOnly(true,false,this)"!-->
                                   <%-- <cc1:FilteredTextBoxExtender ID="FtextxtExchangeValue" runat="server" TargetControlID="txtExchangeValue"
                                    FilterType="Numbers,Custom" Enabled="True" ValidChars="." FilterMode="ValidChars" >
                                     </cc1:FilteredTextBoxExtender>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" height="5">
                                </td>
                            </tr>
                            <tr>
                                <td class="styleFieldLabel">
                                </td>
                                <td class="styleFieldAlign">
                                    <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                        CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" />
                                  
                                    <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                        CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                                   
                                    <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr class="styleButtonArea">
                                <td colspan="2">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:ValidationSummary CssClass="styleSummaryStyle" ID="vsExchangeRate" runat="server"
                                        ShowMessageBox="false" ShowSummary="true" HeaderText="Please correct the following validation(s):  " />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript" src="../Scripts/NumericTextBox.js"></script>
</asp:Content>

