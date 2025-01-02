<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GOrgCashflowMaster_Add.aspx.cs" Inherits="Origination_S3GOrgCashflowMaster_Add" %>

<%@ Register TagPrefix="uc3" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="tbCFM" runat="server" border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Cash Flow Serial No." ID="lblCashflowNo"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 25%">
                                    <asp:TextBox ID="txtCashflowNo" runat="server" Width="180px" ReadOnly="true" ToolTip="Cash Flow Serial Number"></asp:TextBox>
                                </td>
                                <td style="width: 5%"></td>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Line of Business" CssClass="styleReqFieldLabel" ID="lblLOB"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 25%">
                                    <asp:DropDownList ID="ddlLOB" runat="server" AutoPostBack="True" ToolTip="Line of Business"
                                        OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5%">
                                    <asp:RequiredFieldValidator ID="rfvLOB" runat="server" ErrorMessage="Select the Line of Business"
                                        ControlToValidate="ddlLOB" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Inflow/Outflow" ID="lblCashflow" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 25%">
                                    <asp:DropDownList ID="ddlCashflow" runat="server" ToolTip="Inflow/Outflow" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlCashflow_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCashflow" runat="server" ErrorMessage="Select the Inflow/Outflow"
                                        ControlToValidate="ddlCashflow" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 5%"></td>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Cash Flow Description" ID="lblCashflowDesc" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 25%">
                                    <asp:TextBox ID="txtCashflowDesc" runat="server" Width="230px" MaxLength="50" ToolTip="CashFlow Description"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtCashflowDesc"
                                        FilterType="Custom , UppercaseLetters, LowercaseLetters, Numbers" ValidChars=" "
                                        Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                                <td style="width: 5%">
                                    <asp:RequiredFieldValidator ID="rfvCashflowDesc" runat="server" ErrorMessage="Enter the Cash Flow Description"
                                        ControlToValidate="txtCashflowDesc" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Cash Flow Flag" ID="lblCashflowFlag" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 25%">
                                    <asp:DropDownList ID="ddlCashflowFlag" runat="server" ToolTip="Cash Flow Flag" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlCashflowFlag_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCashflowflag" runat="server" ErrorMessage="Select the Cash Flow Flag"
                                        ControlToValidate="ddlCashflowFlag" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 5%"></td>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Active" ID="Label7"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="padding-left: 7px; width: 25%">
                                    <asp:CheckBox ID="CbxActive" runat="server" ToolTip="Is Active" />
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>

                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Parent Cashflow" ID="Label11" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 25%">
                                    <asp:DropDownList ID="ddlParentCashflow" Enabled="false" runat="server" ToolTip="Parent Cashflow">
                                    </asp:DropDownList>

                                </td>

                                <td style="width: 5%"></td>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Extend" ID="lblextend"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="padding-left: 7px; width: 25%">
                                    <asp:CheckBox ID="ChkExtended" runat="server" ToolTip="Extend" OnCheckedChanged="ChkExtended_CheckedChanged" AutoPostBack="true" />


                                    <asp:Button ID="btnextend" runat="server" CssClass="styleGridShortButton"
                                        OnClick="btnextend_Click" Text="Extend" ToolTip="Extend" Enabled="false" />

                                    <%--  <asp:Button ID="btnModal" Style="display: none" runat="server" />--%>
                                </td>
                                <td></td>
                                <td style="width: 5%"></td>

                            </tr>
                            <tr>
                                <td style="width: 5%"></td>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="GST Appl.: " ID="lblGST"></asp:Label>
                                    <asp:Label runat="server" Text="" ID="lblGSTAppl"></asp:Label>
                                </td>
                                <td align="left" colspan="2" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="SAC Code: " ID="lblSAC"></asp:Label>
                                    <asp:Label runat="server" Text="" ID="lblSACCOde"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Recovery Type" Visible="false" ID="lblRecoveryType" CssClass="styleReqFieldLabel"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" style="width: 25%">
                                    <asp:DropDownList ID="ddlRecoveryType" Visible="false" runat="server" ToolTip="Recovery Type">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr style="height: 10px">
                                <td></td>
                            </tr>
                            <tr id="trDebit" runat="server">
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Debit" ID="lblDebit" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" colspan="5">
                                    <table id="tblDebit" runat="server" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="center">
                                                <asp:Label runat="server" Text="Type" ID="Label2"></asp:Label>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td align="center" visible="false">
                                                <asp:Label runat="server" Text="GL Account Code" ID="Label1"></asp:Label>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" Text="GL Account Code" ID="Label8"></asp:Label>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" Text="SL Account Code" ID="Label3"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlDType" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlDType_SelectedIndexChanged" ToolTip="Debit Type">
                                                </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>

                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <%-- <asp:DropDownList ID="ddlDAccountFlag" runat="server" Width="250px" ToolTip="Debit GL Code"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlDAccountFlag_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                <%-- <asp:DropDownList ID="ddlDGLCode" runat="server" Width="250px" ToolTip="Debit GL Code"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlDGLCode_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                                <uc3:Suggest ID="ddlDGLCode" AutoPostBack="True" runat="server" ServiceMethod="GetGLCodeDC"
                                                    ValidationGroup="Header" ErrorMessage="Select the GL Account Code(Debit)" IsMandatory="true" Width="250px"
                                                    OnItem_Selected="ddlDGLCode_OnSelectedIndexChanged" />
                                                <%--<asp:RequiredFieldValidator ID="rfDebitAccount" runat="server" ErrorMessage="Select the GL Account Code (Debit)"
                                                    ControlToValidate="ddlDGLCode" InitialValue="0" Display="None" Enabled="false"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <%--                                     <uc3:Suggest ID="ddlDSLCode" runat="server" ServiceMethod="GetSLCodeDebit"
                                                                ValidationGroup="Header" IsMandatory="true" OnItem_Selected="ddlDSLCode_SelectedIndexChanged"/>--%>
                                                <uc3:Suggest ID="ddlDSLCode" AutoPostBack="True" runat="server" ServiceMethod="GetSLCodeD"
                                                    ValidationGroup="Header" ErrorMessage="Select the SL Account Code(Debit)" IsMandatory="true" Width="250px"
                                                    OnItem_Selected="ddlDSLCode_SelectedIndexChanged" />
                                                <%--<asp:DropDownList ID="ddlDSLCode" runat="server" Width="150px" ToolTip="Debit SL Code"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlDSLCode_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 10px">
                                <td></td>
                            </tr>
                            <tr id="trCredit" runat="server">
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" Text="Credit" ID="lblCredit" CssClass="styleDisplayLabel"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" colspan="5">
                                    <table id="tblCredit" runat="server" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="center">
                                                <asp:Label runat="server" Text="Type" ID="Label6"></asp:Label>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td align="center" visible="false">
                                                <asp:Label runat="server" Text="GL Account Code" ID="Label5"></asp:Label>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" Text="GL Account Code" ID="Label4"></asp:Label>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:Label runat="server" Text="SL Account Code" ID="Label9"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlCType" runat="server" Width="150px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlCType_SelectedIndexChanged" ToolTip="Credit Type">
                                                </asp:DropDownList>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>

                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <%--  <asp:DropDownList ID="ddlCAccountFlag" runat="server" Width="250px" ToolTip="Credit GL Code"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlCAccountFlag_SelectedIndexChanged">
                                                </asp:DropDownList>--%>
                                                <%--<asp:DropDownList ID="ddlCGLCode" runat="server" Width="250px" ToolTip="Credit GL Code"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCGLCode_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                                <uc3:Suggest ID="ddlCGLCode" AutoPostBack="True" runat="server" ServiceMethod="GetGLCodeDC"
                                                    ValidationGroup="Header" ErrorMessage="Select the GL Account Code(Credit)" IsMandatory="true" Width="250px"
                                                    OnItem_Selected="ddlCGLCode_SelectedIndexChanged" />
                                                <%--<asp:RequiredFieldValidator ID="rfvCreditAccount" runat="server" ErrorMessage="Select the GL Account Code (Credit)"
                                                    ControlToValidate="ddlCGLCode" InitialValue="0" Display="None" Enabled="false"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <uc3:Suggest ID="ddlCSLCode" AutoPostBack="True" runat="server" ServiceMethod="GetSLCodeC"
                                                    ValidationGroup="Header" ErrorMessage="Select the SL Account Code(Credit)" IsMandatory="true" Width="250px"
                                                    OnItem_Selected="ddlCSLCode_SelectedIndexChanged" />
                                                <%--<asp:DropDownList ID="ddlCSLCode" runat="server" Width="150px" ToolTip="Credit SL Code"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlCSLCode_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 20px">
                                <td></td>
                            </tr>
                            <tr>
                                <td align="left" class="styleFieldLabel" width="20%">
                                    <asp:Label runat="server" ID="Label10"></asp:Label>
                                </td>
                                <td align="left" class="styleFieldAlign" colspan="5">
                                    <table id="Table3" runat="server" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left">
                                                <asp:Label runat="server" Text="Business IRR." ID="lblBusIRR"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="CbxBusIRR" runat="server" AutoPostBack="True" ToolTip="Bussiness IRR"
                                                    OnCheckedChanged="CbxBusIRR_CheckedChanged" />
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" Text="Accounting IRR." ID="lblAccIRR"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="CbxAccIRR" runat="server" AutoPostBack="True" ToolTip="Accounting IRR"
                                                    OnCheckedChanged="CbxAccIRR_CheckedChanged" />
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" Text="Company IRR." ID="lblBoth"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="CbxBoth" runat="server" AutoPostBack="True" ToolTip="Company IRR"
                                                    OnCheckedChanged="CbxBoth_CheckedChanged" />
                                            </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:Label runat="server" Text="All" ID="lblAll"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:CheckBox ID="CbxAll" runat="server" AutoPostBack="True" ToolTip="All" OnCheckedChanged="CbxAll_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 10px">
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Panel runat="server" ID="panGeneralParameters" GroupingText="Program Reference Grid"
                                        CssClass="stylePanel" Style="overflow: hidden">
                                        <div style="height: 190px; overflow-y: auto; overflow-x: hidden" width="100%" class="container">
                                            <asp:GridView ID="gvCashflowProgram" runat="server" Width="100%" AutoGenerateColumns="False"
                                                DataKeyNames="Program_ID" OnRowDataBound="gvCashflowProgram_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProgramid" runat="server" Text='<%# Eval("Program_ID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Program Ref. No." HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProgramRefNo" runat="server" Text='<%# Eval("Program_Ref_No") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle Width="15%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Program Description" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProgramName" runat="server" Text='<%# Eval("Program_Name") %>' />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle Width="50%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Capture" HeaderStyle-CssClass="styleGridHeader" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="CbxCapture" Enabled="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Display" HeaderStyle-CssClass="styleGridHeader" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="CbxDisplay" Enabled="false" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Post" HeaderStyle-CssClass="styleGridHeader">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="CbxPost" ToolTip="Post" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="styleInfoLabel" HorizontalAlign="Center" />
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- <asp:RequiredFieldValidator ID="rfvDGlcode" runat="server" ErrorMessage="Select the GL Account Code(Debit)"
                            ControlToValidate="ddlDGLCode" InitialValue="0" Display="None" Enabled="false"></asp:RequiredFieldValidator>--%>
                        <%--<asp:RequiredFieldValidator ID="rfvDSlcode" runat="server" ErrorMessage="Select the SL Account Code(Debit)"
                            ControlToValidate="ddlDSLCode" InitialValue="0" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvCGlcode" runat="server" ErrorMessage="Select the GL Account Code(Credit)"
                            ControlToValidate="ddlCGLCode" InitialValue="0" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvCSlcode" runat="server" ErrorMessage="Select the SL Account Code(Credit)"
                            ControlToValidate="ddlCSLCode" InitialValue="0" Display="None" ValidationGroup="Save"
                            Enabled="false"></asp:RequiredFieldValidator>--%>
                        <asp:RequiredFieldValidator ID="rfvDType" runat="server" ErrorMessage="Select the Type(Debit)"
                            ControlToValidate="ddlDType" InitialValue="0" Display="None"
                            Enabled="false"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvCType" runat="server" ErrorMessage="Select the Type(Credit)"
                            ControlToValidate="ddlCType" InitialValue="0" Display="None" Enabled="false"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="height: 10px">
                    <td></td>
                </tr>
                <tr>
                    <td id="Td1" runat="server" align="center" colspan="5">
                        <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                            ToolTip="Save" OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                            ToolTip="Clear" CausesValidation="False" OnClick="btnClear_Click" />
                        <cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to Clear?"
                            Enabled="True" TargetControlID="btnClear">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                            ToolTip="Clear" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="vsCashfolw" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):  " />
                        <asp:CustomValidator ID="cvCashflow" runat="server" Display="None" CssClass="styleMandatoryLabel"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>


    <cc1:ModalPopupExtender ID="moecashextend" runat="server" TargetControlID="btnModal" PopupControlID="panelextend"
        BackgroundCssClass="styleModalBackground" Enabled="true">
    </cc1:ModalPopupExtender>

    <asp:Panel runat="server" ID="panelextend" Style="display: none; vertical-align: middle" BorderStyle="Solid" BackColor="White">
        <asp:UpdatePanel ID="upPass" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvdebit" runat="server" ErrorMessage="Select Debit Account Type"
                                            ControlToValidate="ddldebit" InitialValue="0" Display="None" ValidationGroup="Extend"
                                            Enabled="false"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvcredittype" runat="server" ErrorMessage="Select Credit Account Type"
                                            ControlToValidate="ddlcredittype" InitialValue="0" Display="None" Enabled="false" ValidationGroup="Extend"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="vsPopUp" runat="server" ShowSummary="true" ValidationGroup="Extend" Enabled="false"
                                            HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="divTransaction" class="container" runat="server" style="height: 300px; overflow: scroll">
                                            <table width="100%">
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblcash" Text="Cash Flow Flag"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtCash" runat="server" ToolTip="Cash Flow Flag" ReadOnly="true"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblaccount" Text="Debit Account Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <%--<asp:TextBox ID="txtdebit" runat="server" ToolTip="Debit Account Type" ReadOnly="true"></asp:TextBox>--%>
                                                        <asp:DropDownList ID="ddldebit" runat="server" Width="150px"
                                                            ToolTip="Debit Type">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblmulti" Text="Multi GL"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:CheckBox ID="chkmulti" runat="server" ToolTip="is Multi-GL Applicable" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lbldesc" Text="Description"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:TextBox ID="txtdesc" runat="server" ReadOnly="true" ToolTip="Cash Flow Description"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblcredittype" Text="Credit Account Type" CssClass="styleReqFieldLabel"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <%--<asp:TextBox ID="txtcredittype" runat="server" ToolTip="Credit Account Type" ReadOnly="true"></asp:TextBox>--%>
                                                        <asp:DropDownList ID="ddlcredittype" runat="server" Width="150px"
                                                            ToolTip="Credit Type">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <%--     </td>--%>
                                                    <td class="styleFieldLabel">
                                                        <asp:Label runat="server" ID="lblstate" Text="State/Circle"></asp:Label>
                                                    </td>
                                                    <td class="styleFieldAlign">
                                                        <asp:DropDownList ID="ddlstate" runat="server" ToolTip="State/Circle" AutoPostBack="true" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged">
                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="State" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Circle" Value="2"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <table width="100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 3%;"></td>
                                                                <td style="width: 20%;"></td>
                                                                <%--   <td style="width: 5%;"></td>
                                                                 <td style="width: 25%;"></td>--%>
                                                                <%--  <td style="width: 10%;"></td>--%>
                                                                <%--<td style="width: 3%;"></td>styleCustGridHeader--%>
                                                                <td style="width: 35%; border: inherit; font-size: medium;" align="center" class="styleCustGridHeader">Debit</td>
                                                                <%-- <td style="width: 10%;"></td>--%>
                                                                <%--  <td style="width: 10%;"></td>--%>
                                                                <%--  <td style="width: 0%;"></td>--%>
                                                                <td style="width: 35%; border: inherit; font-size: medium" align="center" class="styleCustGridHeader">Credit</td>
                                                                <td style="width: 7%;"></td>
                                                                <%-- <td style="width: 10%;"></td>--%>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                        <asp:GridView ID="gvcashextended" runat="server" AutoGenerateColumns="False" DataKeyNames="Sno"
                                                            EditRowStyle-CssClass="styleFooterInfo" ShowFooter="True" OnRowDataBound="gvcashextended_RowDataBound"
                                                            OnRowDeleting="gvcashextended_DeleteClick" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No.">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="3%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle Width="3%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="State/Circle">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("StateName")%>' ID="lblState"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddl_stateF" runat="server" ToolTip="State/Circle"
                                                                            AutoPostBack="false">
                                                                        </asp:DropDownList>
                                                                    </FooterTemplate>

                                                                    <HeaderStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                                    <FooterStyle Width="20%" HorizontalAlign="Left" />

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="GL Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("GL_Value")%>' ID="lblglcode"></asp:Label>
                                                                        <%--  <asp:TextBox ID="txtglcode" runat="server" Width="120px" ReadOnly="true" ToolTip="GL Code"></asp:TextBox>--%>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <uc3:Suggest ID="ddlExDGLCode" AutoPostBack="true" runat="server" ServiceMethod="GetGLCodeDC" OnItem_Selected="ddlExDGLCode_SelectedIndexChanged" />
                                                                        <%--<uc3:Suggest ID="ddlglcodef" runat="server" ServiceMethod="GetBranchList" AutoPostBack="true"
                                                                                                OnItem_Selected="ddlglcodef_SelectedIndexChanged" ErrorMessage="Select GL Code(Debit)"
                                                                                                ValidationGroup="PopUpAdd" IsMandatory="true" />--%>
                                                                        <%-- ErrorMessage="Select the GL Account Code(Debit)" OnItem_Selected="ddlExDGLCode_OnSelectedIndexChanged"    <asp:TextBox ID="txtglcodef" runat="server" Width="120px" ToolTip="GL Code"></asp:TextBox>--%>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                                    <FooterStyle Width="20%" HorizontalAlign="Left" />

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SL Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("SL_Value")%>' ID="lblslcode"></asp:Label>
                                                                        <%-- <asp:TextBox ID="txtslcode" runat="server" Width="120px" ReadOnly="true" ToolTip="SL Code"></asp:TextBox>--%>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <uc3:Suggest ID="ddlExDSLCode" AutoPostBack="false" runat="server" ServiceMethod="Get_Grid_SLCodeDebit" OnItem_Selected="ddlExDSLCode_SelectedIndexChanged" />
                                                                        <%--ErrorMessage="Select the SL Account Code(Debit)"  <asp:TextBox ID="txtslcodef" runat="server" Width="120px" ToolTip="SL Code"></asp:TextBox>--%>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                    <FooterStyle Width="15%" HorizontalAlign="Left" />

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="GL Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("GL_Value_cr")%>' ID="lblglcodeCr"></asp:Label>
                                                                    </ItemTemplate>

                                                                    <FooterTemplate>
                                                                        <uc3:Suggest ID="ddlExCGLCode" AutoPostBack="true" runat="server" ServiceMethod="GetGLCodeDC" OnItem_Selected="ddlExCGLCode_SelectedIndexChanged" />
                                                                        <%--ErrorMessage="Select the GL Account Code(Credit)"  <asp:TextBox ID="txtglcrf" runat="server" Width="120px" ToolTip="GL Code"></asp:TextBox>--%>
                                                                    </FooterTemplate>

                                                                    <HeaderStyle Width="20%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                                    <FooterStyle Width="20%" HorizontalAlign="Left" />

                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="SL Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" Text='<%#Eval("SL_Value_cr")%>' ID="lblslcodeCr"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <uc3:Suggest ID="ddlExCSLCode" AutoPostBack="false" runat="server" ServiceMethod="Get_Grid_SLCodeCredit" OnItem_Selected="ddlExCSLCode_SelectedIndexChanged" />
                                                                        <%-- ErrorMessage="Select the SL Account Code(Credit)"  <asp:TextBox ID="txtslcrf" runat="server" Width="120px" ToolTip="SL Code"></asp:TextBox>--%>
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="15%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                    <FooterStyle Width="15%" HorizontalAlign="Left" />

                                                                </asp:TemplateField>
                                                                <%--<link button />--%>
                                                                <asp:TemplateField HeaderText="Action">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete" CausesValidation="false"
                                                                            OnClientClick="return confirm('Do you want to Delete this record?');" ToolTip="Delete">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="styleGridShortButton"
                                                                            OnClick="btnAdd_Click" Text="Add" ToolTip="Add" />
                                                                    </FooterTemplate>
                                                                    <%--  <EditItemTemplate>
                                                                    <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                                        ValidationGroup="Add" ToolTip="Update"></asp:LinkButton>&nbsp;|
                                                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                                        CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                                                </EditItemTemplate>--%>
                                                                    <HeaderStyle Width="7%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                    <ItemStyle Width="7%" HorizontalAlign="Left" />
                                                                    <FooterStyle Width="7%" HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnDEVModalAdd" runat="server" Text="Ok" ToolTip="Ok" class="styleSubmitButton"
                                            OnClick="btnDEVModalAdd_Click" ValidationGroup="Extend" />
                                        <asp:Button ID="btnDEVModalCancel" runat="server" Text="Close" OnClick="btnDEVModalCancel_Click"
                                            ToolTip="Close" class="styleSubmitButton" />
                                    </td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                </table>

            </ContentTemplate>
            <%-- <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlstate" EventName="SelectedIndexChanged" />
                               <asp:AsyncPostBackTrigger ControlID="btnSave" />
                                </Triggers>--%>
        </asp:UpdatePanel>
    </asp:Panel>















    <asp:Button ID="btnModal" Style="display: none" runat="server" />
    <script type="text/javascript" language="javascript">

        function verifyString(e) {
            document.getElementById(e).value = document.getElementById(e).value.trim();
        }

        function fnUnselectAllExpectSelected(TargetControl, gRow) {
            var TargetBaseControl = document.getElementById('<%=gvCashflowProgram.ClientID %>');
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            for (var n = 0; n < Inputs.length; ++n) {
                Inputs[n].parentElement.parentElement.parentElement.style.backgroundColor = "white";
                if (Inputs[n].type == 'checkbox' && Inputs[n].uniqueID != TargetControl.uniqueID) {
                    Inputs[n].checked = false;
                }
            }
            if (TargetControl.checked == true) {
                document.getElementById(gRow).style.backgroundColor = "#f5f8ff";
            }
            else {
                document.getElementById(gRow).style.backgroundColor = "white";
            }
        }

    </script>

</asp:Content>
