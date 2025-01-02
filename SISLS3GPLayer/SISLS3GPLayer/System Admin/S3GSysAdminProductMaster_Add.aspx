<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminProductMaster_Add.aspx.cs" ValidateRequest="false" Inherits="S3GSysAdminProductMaster_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="mbcbb" %>
<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function fnchksplchar() {
            var key = (typeof window.event != 'undefined') ? window.event.keycode : e.keycode;
            if (key != undefined) {
                alert(key);
                if (((key < '97') || (key > '122')) && ((key < '65') || (key > '90')) && ((key < '48') || (key > '57'))) {
                    event.keycode = 0;
                    return false;
                }
            }
            else {
                var sKeyCode = window.event.keyCode;
                if (((window.event.keyCode < 97) || (window.event.keyCode > 122)) && ((window.event.keyCode < 65) || (window.event.keyCode > 90))) {
                    window.event.keyCode = 0;
                    return false;
                }
            }
        }
        function TabOrder() {
            alert('in');
            if (window.event.keyCode == 9) {
                alert(window.event.keyCode);
                var txtdesc = document.getElementById('ctl00_ContentPlaceHolder1_txtProductDesc');
                //var btncmb=document.getElementById('ctl00_ContentPlaceHolder1_cmbRoleCenterCode_Button');
                document.getElementById(txtdesc.id).focus();
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label runat="server" Text="Line of Business" ID="lblLOB" CssClass="styleReqFieldLabel"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:DropDownList ID="ddlLOB" TabIndex="0" runat="server" Width="210px" onmouseover="ddl_itemchanged(this);"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddLOB_SelectedIndexChanged" ToolTip="Line of Business">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="ddlLOB" InitialValue="0" ErrorMessage="Select a Line of Business"
                                                    Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="lblProductCode" runat="server" CssClass="styleReqFieldLabel" Text="Product Code"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <mbcbb:ComboBox ID="cmbProductCode" runat="server" AppendDataBoundItems="false" AutoCompleteMode="None"
                                                    AutoPostBack="True" CssClass="WindowsStyle1" DropDownStyle="Simple" MaxLength="3"
                                                    Width="183px" OnSelectedIndexChanged="cmbProductCode_SelectedIndexChanged" TabIndex="1"
                                                    ToolTip="Product Code" OnItemInserted="cmbProductCode_ItemInserted">
                                                </mbcbb:ComboBox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvProductCode" CssClass="styleMandatoryLabel" runat="server"
                                                    ControlToValidate="cmbProductCode" InitialValue="0" ErrorMessage="Enter/Select a Product Code"
                                                    SetFocusOnError="True" Display="None">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 232px"></td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="lblProductDesc" runat="server" CssClass="styleReqLabel" Text="Product Description"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtProductDesc" runat="server" MaxLength="40" Style="width: 240px"
                                                    TabIndex="2" ToolTip="Product Description"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvProductDesc" runat="server" ControlToValidate="txtProductDesc"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Product Description"
                                                    Text="Enter Product Description">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <%--  <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblMinAmt" runat="server" CssClass="styleReqFieldLabel" Text="Min Finance Amount"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtMinAmount" runat="server" MaxLength="12" Style="width: 140px"
                                                    ToolTip="Min Finance Amount" TabIndex="3"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="rftxtMinAmount" runat="server" ControlToValidate="txtMinAmount"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Minimum Finance Amount"
                                                    Text="Enter Minimum Finance Amount"  ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>   
                                                <cc1:FilteredTextBoxExtender ID="Minamount" runat="server" TargetControlID="txtMinAmount"
                                                    FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblMaxAmt" runat="server" CssClass="styleReqFieldLabel" Text="Max Finance Amount"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtMaxAmount" runat="server" MaxLength="12" Style="width: 140px"
                                                    ToolTip="Finance Max Amount" TabIndex="4"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfttxtMaxAmount" runat="server" ControlToValidate="txtMaxAmount"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Maximum Finance Amount"
                                                    Text="Enter Maximum Finance Amount" ValidationGroup="Add">
                                                </asp:RequiredFieldValidator>   
                                                <cc1:FilteredTextBoxExtender ID="Maxamount" runat="server" TargetControlID="txtMaxAmount"
                                                    FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblMinTnr" runat="server" CssClass="styleReqFieldLabel" Text="Minimum Tenure"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtMinTnr" runat="server" TabIndex="5" MaxLength="12" Style="width: 100px" ToolTip="Minimum Tenure"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfttxtMinTnr" runat="server" ControlToValidate="txtMinTnr"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Minimum Tenure"  ValidationGroup="Add" 
                                                    Text="Enter Minimum Tenure"></asp:RequiredFieldValidator>   
                                                <cc1:FilteredTextBoxExtender ID="fttxtMinTnr" runat="server" TargetControlID="txtMinTnr"
                                                    FilterType="Numbers" Enabled="true" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="styleFieldLabel">
                                                <asp:Label ID="lblMaxTnr" runat="server" CssClass="styleReqFieldLabel" Text="Maximum Tenure"> </asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:TextBox ID="txtMaxTnr" runat="server" TabIndex="6" MaxLength="12" Style="width: 100px" ToolTip="Maximum Tenure"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="rfttxtMaxTnr" runat="server" ControlToValidate="txtMaxTnr"
                                                    CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Maximum Tenure"
                                                    Text="Enter Maximum Tenure"  ValidationGroup="Add" ></asp:RequiredFieldValidator>   
                                                <cc1:FilteredTextBoxExtender ID="fttxtMaxTnr" runat="server" TargetControlID="txtMaxTnr"
                                                    FilterType="Numbers" Enabled="true" ValidChars="">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active"
                                                    Width="13%"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" ToolTip="Active" Width="40%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="styleFieldLabel" style="width: 232px">
                                                <asp:Label ID="lblOtherCashflow" runat="server" CssClass="styleDisplayLabel" Text="Other Cashflow"></asp:Label>
                                            </td>
                                            <td class="styleFieldAlign">
                                                <asp:CheckBox ID="chkOtherCashflow" runat="server" Checked="false" ToolTip="Active" Width="40%" OnCheckedChanged="chkOtherCashflow_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="grid" runat="server">
                                        <ContentTemplate>
                                            <div align="center">
                                                <asp:GridView ID="gvInflow" runat="server" AutoGenerateColumns="False" EditRowStyle-CssClass="styleFooterInfo"
                                                    ShowFooter="True" OnRowDataBound="gvInflow_RowDataBound" Width="650px" OnRowDeleting="gvInflow_Deleting"
                                                    DataKeyNames="Sno">
                                                    <RowStyle HorizontalAlign="Left" Width="100%" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No." ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="SerialNoHidden" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                <asp:Label ID="lblSerialNo" runat="server" Visible="false" Text='<%# Bind("Sno") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Cash Inflow">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCashInflow" runat="server" Text='<%# Bind("Cashinflow") %>'></asp:Label>
                                                                <asp:Label ID="lblCashInflow_ID" runat="server" Text='<%# Bind("Cashflow_ID") %>'
                                                                    Visible="false"></asp:Label>
                                                                <asp:Label ID="lblchargetypee" runat="server" Text='<%# Bind("chargetype") %>' Visible="false"></asp:Label>
                                                                <%-- <asp:Label ID="cashflowlabel" runat="server" Visible="false" Text='<%# Bind("Cashflow_IDD") %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlCashInflow" AutoPostBack="true" OnSelectedIndexChanged="DropDownCashflow_SelectedIndexChanged"
                                                                    runat="server" ToolTip="Cash Inflow">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ChargeType">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblchargetype" Text='<%# Bind("Charge") %> ' runat="server"></asp:Label>
                                                                <asp:Label ID="lblchargetypevalue" Text='<%# Bind("ChargeType") %>' Visible="false"
                                                                    runat="server">
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlChargeType" AutoPostBack="true" OnSelectedIndexChanged="DropDownchargetype_SelectedIndexChanged"
                                                                    runat="server">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Value" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnAssignIValue" runat="server" CssClass="styleSubmitShortButton"
                                                                    CausesValidation="false" OnClick="btnIAssignValue_Click" ToolTip="Assign Value"
                                                                    Width="50px" Text="Assign" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAssignFValue" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                                    OnClick="btnFAssignValue_Click" ToolTip="Assign Value" Text="Assign" />
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem, "Is_Active").ToString().ToUpper() == "TRUE"%>' />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:CheckBox ID="chkActiveF" runat="server" ToolTip="Active" Checked="true" Enabled="false"></asp:CheckBox>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"
                                                                    OnClientClick="return confirm('Do you want to Delete this record?');" ToolTip="Delete">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="styleGridShortButton"
                                                                    OnClick="btnAdd_Click" Text="Add" ValidationGroup="Grid" ToolTip="Add" />
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Center" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle CssClass="styleFooterInfo" />
                                                </asp:GridView>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <tr>
                            <td>
                                <table align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnSave" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                                Height="26px" OnClick="btnSave_Click" Text="Save" TabIndex="4" ToolTip="Save" OnClientClick="return fnCheckPageValidators('Add');"
                                                ValidationGroup="Add" />
                                            <%-- OnClientClick="return fnCheckPageValidators();"--%>
                                            <asp:Button ID="btnClear" runat="server" class="styleSubmitButton" CausesValidation="false"
                                                OnClick="btnClear_Click" Text="Clear" TabIndex="5" ToolTip="Clear" />
                                            <mbcbb:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Do you want to clear?"
                                                TargetControlID="btnClear">
                                            </mbcbb:ConfirmButtonExtender>
                                            <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                                                OnClick="btnCancel_Click" Text="Cancel" TabIndex="6" ToolTip="Cancel" />
                                        </td>
                                    </tr>
                                </table>
                                <table align="left">
                                    <tr class="styleButtonArea">
                                        <td>
                                            <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="ProductDetailsAdd" runat="server" HeaderText="Correct the following validation(s):"
                                                CssClass="styleMandatoryLabel" ShowSummary="true" ValidationGroup="Add" />
                                            <%--  <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Correct the following validation(s):"
                                                CssClass="styleMandatoryLabel" ShowSummary="true" ValidationGroup="Grid" />--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </td>
                </tr>
                <%-- Added by sathish for grid model popup--%>
            </table>
            <input type="hidden" id="hdnID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <table>
        <tr>
            <td>
                <cc1:ModalPopupExtender ID="ModalPopupExtenderAssignValue" runat="server" TargetControlID="btnModal"
                    PopupControlID="PnlAssignAmount" BackgroundCssClass="styleModalBackground" Enabled="true">
                </cc1:ModalPopupExtender>
                <asp:Panel ID="PnlAssignAmount" Style="display: none; vertical-align: middle" runat="server"
                    BorderStyle="Solid" BackColor="White" Width="640px">
                    <asp:UpdatePanel ID="upPass" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td width="130%">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div id="divTransaction" class="container" runat="server" style="height: 200px; width: 640px;">
                                                        <asp:GridView ID="grvAssignValue" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                            Width="99%" Height="50px" OnRowCommand="grvassign_rowcommand" OnRowDeleting="grv_RowDeleting"
                                                            OnRowDataBound="grvAssignValue_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="SNo" Visible="True">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNo" Text='<% #Bind("SNo")%>' Visible="false" runat="server"></asp:Label>
                                                                        <asp:Label ID="Label1" Style="text-align: right" Text='<%#Container.DataItemIndex+1%>'
                                                                            Visible="true" runat="server"> </asp:Label>
                                                                        <asp:Label ID="cashflowid" runat="server" Text='<% #Bind("cashflowid")%>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="From Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromAmount" ToolTip="Sequence Number" Style="text-align: right"
                                                                            Text='<% #Bind("FromAmount")%>' runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFromAmount" runat="server" Style="text-align: right"></asp:TextBox>
                                                                        <%-- <cc1:FilteredTextBoxExtender ID="fromamount" runat="server" TargetControlID="txtFromAmount"
                                                                            FilterType="Numbers,Custom"  Enabled="true" ValidChars="">
                                                                        </cc1:FilteredTextBoxExtender>--%>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="To Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblToAmount" Visible="true" Style="text-align: right" Text='<% #Bind("ToAmount")%>'
                                                                            runat="server">
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtToamount" runat="server" Style="text-align: right"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="toamount" runat="server" TargetControlID="txtToamount"
                                                                            FilterType="Numbers,Custom" Enabled="true" ValidChars=".">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Percentage/Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblpercentageoramount" Style="text-align: right" Visible="true" Text='<% #Bind("PercentageorAmount")%>'
                                                                            runat="server">
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtpercentageoramount" Style="text-align: right" runat="server"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender ID="Filter" runat="server" TargetControlID="txtpercentageoramount"
                                                                            FilterType="Numbers,Custom" ValidChars="." Enabled="true">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                        <%-- <asp:RegularExpressionValidator ID="Regx" Visible="false" ValidationGroup="PopUpAdd"
                                                                            ValidationExpression="^((100)|(\d{0,2}))$" ControlToValidate="txtpercentageoramount"
                                                                            Display="None" runat="server" ErrorMessage="Enter 1-100 Percentage with Round of 2 "></asp:RegularExpressionValidator>--%>
                                                                        <asp:RangeValidator ID="Regx" runat="server" Type="Double" Display="None" ValidationGroup="PopUpAdd"
                                                                            ControlToValidate="txtpercentageoramount" Visible="false" MaximumValue="100"
                                                                            MinimumValue="0" ErrorMessage="Percentage should not exceed 100%"></asp:RangeValidator>
                                                                    </FooterTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                                                            Style="text-align: right" Text="Delete" OnClientClick="return confirm('Do you want to Delete this record?');"
                                                                            ToolTip="Delete">
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Button ID="btnAddd" runat="server" CausesValidation="true" CommandName="Add"
                                                                            CssClass="styleGridShortButton" Text="Add" ValidationGroup="PopUpAdd" ToolTip="Add" />
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Center" Width="10%" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div align="center">
                                                        <asp:Button ID="btnModalAdd" ToolTip="Save" CausesValidation="false" runat="server"
                                                            Text="Save" class="styleSubmitButton" OnClick="btnModalAdd_Click" />
                                                        <asp:Button ID="btnModalCancel" runat="server" ToolTip="Close" CausesValidation="false"
                                                            Text="Close" class="styleSubmitButton" OnClick="btnModalCancel_Click" />
                                                        <asp:ValidationSummary ID="vsPopUp" runat="server" ShowSummary="true" ValidationGroup="PopUpAdd"
                                                            HeaderText="Correct the following validation(s):" CssClass="styleMandatoryLabel" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:Button ID="btnModal" Style="display: none" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
