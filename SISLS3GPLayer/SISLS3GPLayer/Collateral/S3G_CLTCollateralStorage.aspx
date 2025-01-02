<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3G_CLTCollateralStorage.aspx.cs" Inherits="Collateral_S3G_CLTCollateralStorage"
    Title="S3G - Collateral Storage" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc3" %>
<%--  <%@ Register Src="../UserControls/S3GCustORAgent.ascx" TagName="CA" TagPrefix="uc3" %>--%>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function fnLoadCustomer() {

            document.getElementById('ctl00_ContentPlaceHolder1_tcCollateralValuation_tabgeneral_btnLoadCustomer').click();
        }

        function Trim(strInput) {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }

        function hideModalPopupViaClient() {
            //ev.preventDefault();        
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }


        function funCheckFirstLetterisNumeric(textbox, msg) {

            var FieldValue = new Array();
            FieldValue = textbox.value.trim();
            if (FieldValue.length > 0 && FieldValue.value != '') {
                if (isNaN(FieldValue[0])) {
                    return true;
                }
                else {
                    alert(msg + ' name cannot begin with a number');
                    textbox.focus();
                    textbox.value = '';
                    event.returnValue = false;
                    return false;
                }
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- <table>
                <tr>
                    <td>--%>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="Collateral Storage" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        <input id="HidConfirm" type="hidden" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcCollateralValuation" runat="server" ActiveTabIndex="1" CssClass="styleTabPanel"
                            OnActiveTabChanged="tcCollateralValuation_TabIndexChanged" AutoPostBack="false"
                            Width="100%" ScrollBars="None">
                            <cc1:TabPanel runat="server" HeaderText="Customer / Agent" ID="tabCustORAgent" CssClass="tabpan"
                                BackColor="Red" ToolTip="Customer / Agent" Width="99%">
                                <HeaderTemplate>
                                    Customer / Agent
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table width="99%" align="center">
                                        <tr>
                                            <td align="center">
                                                <asp:RadioButton ID="rbtCustomer" class="styleFieldLabel" runat="server" Checked="True"
                                                    Text="Customer" AutoPostBack="True" OnCheckedChanged="GetCust" />
                                                <asp:RadioButton ID="rbtAgent" class="styleFieldLabel" runat="server" Text="Collection Agent"
                                                    AutoPostBack="True" OnCheckedChanged="GetAgent" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    OnRowDataBound="grvPaging_RowDataBound" OnRowCommand="grvPaging_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <HeaderStyle CssClass="styleGridHeader" />
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblQuery" runat="server" Text="Query"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnQuery" ImageUrl="~/Images/spacer.gif" CssClass="styleGridQuery"
                                                                    CommandArgument='<%# Bind("Collateral_Capture_ID") %>' CommandName="Query" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Modify" SortExpression="Customer_ID" Visible="False">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgbtnEdit" ImageUrl="~/Images/spacer.gif" CssClass="styleGridEdit"
                                                                    AlternateText="Modify" CommandArgument='<%# Bind("Collateral_Capture_ID") %>'
                                                                    runat="server" CommandName="Modify" />
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblModify" runat="server" Text="Modify" CssClass="styleGridHeader"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort2" runat="server" Text="Name" ToolTip="Sort By Name"
                                                                                CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnSort2" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch2" runat="server" AutoPostBack="true"
                                                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%-- <asp:Label ID="lblAppropriationID" runat="server" Text='<%# Bind("Appropriation_ID") %>'></asp:Label>--%>
                                                                <asp:HiddenField ID="hidCA_ID" runat="server" Value='<%# Eval("CA_ID")%>' />
                                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Collateral Capture Number">
                                                            <HeaderTemplate>
                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                    <tr align="center">
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkbtnSort1" runat="server" Text="Collateral Capture Number"
                                                                                ToolTip="Sort By Collateral Capture Number" CssClass="styleGridHeader" OnClick="FunProSortingColumn"> </asp:LinkButton>
                                                                            <asp:ImageButton ID="imgbtnSort1" runat="server" ImageAlign="Middle" CssClass="styleImageSortingAsc" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td>
                                                                            <asp:TextBox AutoCompleteType="None" ID="txtHeaderSearch1" runat="server" AutoPostBack="true"
                                                                                OnTextChanged="FunProHeaderSearch" CssClass="styleSearchBox"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCType_No" runat="server" Text='<%# Bind("Collateral_Tran_No") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Captured Date">
                                                            <ItemTemplate>
                                                                <%-- <%# FormatDate(Eval("Created_On").ToString())%>--%>
                                                                <%--<asp:Label ID="lblAppropriationID" runat="server" Text='<%# Bind("Appropriation_ID") %>'></asp:Label>--%>
                                                                <asp:Label ID="lblCreatedOn" runat="server" Text='<%# Eval("Captured_Date")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkIsActive" runat="server" AutoPostBack="true" OnCheckedChanged="Move" />
                                                                <asp:HiddenField ID="hidCLT_ID" runat="server" Value='<%# Eval("Collateral_Capture_ID")%>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserID" Visible="false" runat="server" Text='<%#Eval("Created_By")%>'></asp:Label>
                                                                <asp:Label ID="lblUserLevelID" Visible="false" runat="server" Text='<%#Eval("User_Level_ID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="styleGridHeader"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <input type="hidden" id="hdnSortDirection" runat="server"></input>
                                                <input id="hdnSortExpression" runat="server" type="hidden"></input>
                                                <input id="hdnSearch" runat="server" type="hidden"></input>
                                                <input id="hdnOrderBy" runat="server" type="hidden"></input>
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <uc3:PageNavigator ID="ucCustomPaging" runat="server" />
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <asp:Button ID="btnCACancel" runat="server" CausesValidation="False" CssClass="styleSubmitButton"
                                                    Text="Cancel" UseSubmitBehavior="False" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <span runat="server" id="lblErrorMessage" style="color: Red; font-size: medium">
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="General" ID="tabgeneral" CssClass="tabpan"
                                BackColor="Red" ToolTip="General" Width="99%">
                                <HeaderTemplate>
                                    General</HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlHeader" Width="99%" runat="server" GroupingText="Customer Details"
                                                    CssClass="stylePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <uc1:S3GCustomerAddress ID="S3GCustomerAddress1" ActiveViewIndex="1" runat="server"
                                                                    FirstColumnStyle="styleFieldLabel" SecondColumnStyle="styleFieldAlign" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlStorage" Width="99%" runat="server" GroupingText="Collateral" CssClass="stylePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCollateralNo" runat="server" CssClass="styleDisplayLabel" Text="Collateral Storage No"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCollateralNo" runat="server" Width="130px" ReadOnly="True"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblCollateralDate" runat="server" CssClass="styleDisplayLabel" Text="Collateral Storage Date"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCollateralDate" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                                                <asp:Label ID="hdnCollateralCapture" runat="server" Visible="False"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblActive" runat="server" CssClass="styleDisplayLabel" Text="Active"
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="ChkActive" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="Td1" runat="server" align="center" colspan="5">
                                                <asp:Button runat="server" ID="btnSave" CssClass="styleSubmitButton" Text="Save"
                                                    OnClientClick="return fnCheckPageValidators();" OnClick="btnSave_Click" />
                                                <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" Text="Clear"
                                                    CausesValidation="False" OnClientClick="return fnConfirmClear();" OnClick="btnClear_Click" />
                                                <asp:Button runat="server" ID="btnCancel" CssClass="styleSubmitButton" CausesValidation="False"
                                                    Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="High Liquid Securities" ID="tabHighLiq"
                                CssClass="tabpan" BackColor="Red" ToolTip="High Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    High Liquid Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlHighLiqDetails" CssClass="stylePanel" GroupingText="High Liquid Security Details"
                                        HorizontalAlign="Center">
                                        <%--<div title="High Liquid Security Details" id="divHigh">--%>
                                        <asp:Label ID="lblHighLiqDetails" runat="server" Text="No High Liquid Security Details are Available"
                                            Visible="False" Font-Size="Small" Font-Bold="False" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCollSecurities" runat="server" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblhSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblhMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthCollSecurities" runat="server" ReadOnly="true" ToolTip="Collateral Securities"></asp:TextBox>
                                                    <%-- <asp:DropDownList ID="ddlhCollSecurities" ToolTip="Collateral Securities" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvhCollSecurities" runat="server" ControlToValidate="ddlhCollSecurities"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Select Collateral Securities"
                                                        InitialValue="0" ValidationGroup="AddHighLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCollateralReferenceNo" runat="server" Text="Collateral Ref. Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthCollRefNo" runat="server" Width="60%" ReadOnly="true" ToolTip="Collateral Ref. Number"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthRemarks" TextMode="MultiLine" MaxLength="100" ToolTip="Remarks"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvhRemarks" runat="server" ControlToValidate="txthRemarks"
                                                        Display="None" SetFocusOnError="true" Enabled="false" ErrorMessage="Enter Remarks">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <cc1:ComboBox ID="txthCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                        AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                        Width="100px" MaxLength="30">
                                                    </cc1:ComboBox>
                                                    
                                                    <%--<asp:RequiredFieldValidator ID="rfvhCity" runat="server" ControlToValidate="txthCity"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter City" ValidationGroup="AddHighLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhAddress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthAddress" TextMode="MultiLine" MaxLength="100" ToolTip="Address"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator ID="rfvhAddress" runat="server" ControlToValidate="txthAddress"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Address" ValidationGroup="AddHighLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhStorage1" runat="server" CssClass="styleReqFieldLabel" Text="Storage1"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthStorage1" TextMode="MultiLine" MaxLength="100" ToolTip="Storage1"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvhStorage1" runat="server" ControlToValidate="txthStorage1"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage1" ValidationGroup="AddHighLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhStorage2" runat="server" CssClass="styleReqFieldLabel" Text="Storage2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthStorage2" TextMode="MultiLine" MaxLength="100" ToolTip="Storage2"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator ID="rfvhStorage2" runat="server" ControlToValidate="txthStorage2"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage2" ValidationGroup="AddHighLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhStorage3" runat="server" CssClass="styleReqFieldLabel" Text="Storage3"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthStorage3" TextMode="MultiLine" MaxLength="100" ToolTip="Storage3"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator ID="rfvhStorage3" runat="server" ControlToValidate="txthStorage3"
                                                        Display="None" SetFocusOnError="true" ValidationGroup="AddHighLiqSec1" ErrorMessage="Enter Storage3">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%" valign="top" height="30px">
                                                    <asp:Button ID="btnModifyH" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddHighLiqSec1" Enabled="false" OnClick="btnModifyH_Click" />
                                                    <asp:Button ID="btnClearH" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearH_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvHighLiqDetails" runat="server" AutoGenerateColumns="False" Visible="false"
                                            Width="100%" OnRowDataBound="gv_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblHighCollateral_Type_ID" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbRemarks" runat="server" TargetControlID="txtRemarks"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Right" />--%>
                                                        <asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <%-- <asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="100px" MaxLength="20" Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbCity" runat="server" TargetControlID="txtCity"
                                                            FilterType="LowercaseLetters,UppercaseLetters">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                        <cc1:ComboBox ID="txtCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                            Width="100px" MaxLength="30" Visible="false">
                                                        </cc1:ComboBox>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbAddress" runat="server" TargetControlID="txtAddress"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%-- <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left" ReadOnly = "true" TextMode = "MultiLine" />--%>
                                                        <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbStorage1" runat="server" TargetControlID="txtStorage1"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%--<asp:TextBox ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left" TextMode = "MultiLine" ReadOnly="true"/>--%>
                                                        <asp:TextBox ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbStorage2" runat="server" TargetControlID="txtStorage2"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%-- <asp:TextBox ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left" TextMode = "MultiLine" ReadOnly="true"/>--%>
                                                        <asp:TextBox ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbStorage3" runat="server" TargetControlID="txtStorage3"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvHighLiqDetails1" runat="server" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdHSelect_CheckedChanged" Text="" />
                                                        <%----%></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateral_Type_ID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                        <%--<asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <%-- <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left" ReadOnly = "true" TextMode = "MultiLine" />--%>
                                                        <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--</div> --%>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Medium Liquid Securities" ID="tabMedSeq"
                                CssClass="tabpan" BackColor="Red" ToolTip="Medium Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    Medium Liquid Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="pnlMedLiqDetails" CssClass="stylePanel" GroupingText="Medium Liquid Security Details"
                                        HorizontalAlign="Center" Width="99%" ScrollBars="None">
                                        <%--<div id="div1">--%>
                                        <asp:Label ID="lblMedLiqDetails" runat="server" Text="No Security Details are Available"
                                            Visible="False" Font-Size="Small" Font-Bold="False" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMCollSecurities" runat="server" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblMSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblMMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMCollSecurities" runat="server" ReadOnly="true" ToolTip="Collateral Securities"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMCollateralReferenceNo" runat="server" Text="Collateral Ref. Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMCollRefNo" runat="server" Width="60%" ReadOnly="true" ToolTip="Collateral Ref. Number"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMRemarks" TextMode="MultiLine" MaxLength="100" ToolTip="Remarks"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <cc1:ComboBox ID="txtMCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                        AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                        Width="100px" MaxLength="30">
                                                    </cc1:ComboBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvMCity" runat="server" ControlToValidate="txtMCity"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter City" ValidationGroup="AddMedLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMAddress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMAddress" TextMode="MultiLine" MaxLength="100" ToolTip="Address"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvMAddress" runat="server" ControlToValidate="txtMAddress"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Address" ValidationGroup="AddMedLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMStorage1" runat="server" CssClass="styleReqFieldLabel" Text="Storage1"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMStorage1" TextMode="MultiLine" MaxLength="100" ToolTip="Storage1"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator ID="rfvMStorage1" runat="server" ControlToValidate="txtMStorage1"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage1" ValidationGroup="AddMedLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMStorage2" runat="server" CssClass="styleReqFieldLabel" Text="Storage2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMStorage2" TextMode="MultiLine" MaxLength="100" ToolTip="Storage2"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvMStorage2" runat="server" ControlToValidate="txtMStorage2"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage2" ValidationGroup="AddMedLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMStorage3" runat="server" CssClass="styleReqFieldLabel" Text="Storage3"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMStorage3" TextMode="MultiLine" MaxLength="100" ToolTip="Storage3"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator ID="rfvMStorage3" runat="server" ControlToValidate="txtMStorage3"
                                                        Display="None" SetFocusOnError="true" ValidationGroup="AddMedLiqSec1" ErrorMessage="Enter Storage3">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%" valign="top" height="30px">
                                                    <asp:Button ID="btnModifyM" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddMedLiqSec1" Enabled="false" OnClick="btnModifyM_Click" />
                                                    <asp:Button ID="btnClearM" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearM_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvMedLiqDetails" runat="server" AutoGenerateColumns="False" Visible="false"
                                            ShowFooter="false" Width="100%" OnRowDataBound="gv_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblHighCollateral_Type_ID" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%-- <asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>'>
                                                                    </asp:LinkButton>--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbLRemarks" runat="server" TargetControlID="txtRemarks"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <cc1:ComboBox ID="txtCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                            Width="100px" MaxLength="30" Visible="false">
                                                        </cc1:ComboBox>
                                                        <%--     <asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="100px" MaxLength="20" Visible="false" />--%>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbLAddress" runat="server" TargetControlID="txtAddress"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbLStorage1" runat="server" TargetControlID="txtStorage1"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbLStorage2" runat="server" TargetControlID="txtStorage2"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbLStorage3" runat="server" TargetControlID="txtStorage3"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvMedLiqDetails1" runat="server" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdMSelect_CheckedChanged" Text="" />
                                                        <%----%></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateral_Type_ID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                        <%--<asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <%-- <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left" ReadOnly = "true" TextMode = "MultiLine" />--%>
                                                        <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%-- </div>--%>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Low Liquid Securities" ID="tabLow" CssClass="tabpan"
                                BackColor="Red" ToolTip="Low Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    Low Liquid Securities</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel ID="pnlLowLiqDetails" runat="server" CssClass="stylePanel" Visible="True"
                                        ScrollBars="Horizontal">
                                        <%-- <div id="div2">--%>
                                        <asp:Label ID="lblLowLiqDetails" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Small" Font-Bold="false" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCollSecurities" runat="server" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblLSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblLMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLCollSecurities" runat="server" ReadOnly="true" ToolTip="Collateral Securities"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCollateralReferenceNo" runat="server" Text="Collateral Ref. Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLCollRefNo" runat="server" Width="60%" ReadOnly="true" ToolTip="Collateral Ref. Number"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLRemarks" TextMode="MultiLine" MaxLength="100" ToolTip="Remarks"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <cc1:ComboBox ID="txtLCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                        AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                        Width="100px" MaxLength="30">
                                                    </cc1:ComboBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvLCity" runat="server" ControlToValidate="txtLCity"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter City" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLAddress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLAddress" TextMode="MultiLine" MaxLength="100" ToolTip="Address"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvLAddress" runat="server" ControlToValidate="txtLAddress"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Address" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLStorage1" runat="server" CssClass="styleReqFieldLabel" Text="Storage1"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLStorage1" TextMode="MultiLine" MaxLength="100" ToolTip="Storage1"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvtxtLStorage1" runat="server" ControlToValidate="txtLStorage1"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage1" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLStorage2" runat="server" CssClass="styleReqFieldLabel" Text="Storage2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLStorage2" TextMode="MultiLine" MaxLength="100" ToolTip="Storage2"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--  <asp:RequiredFieldValidator ID="rfvtxtLStorage2" runat="server" ControlToValidate="txtLStorage2"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage2" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLStorage3" runat="server" CssClass="styleReqFieldLabel" Text="Storage3"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLStorage3" TextMode="MultiLine" MaxLength="100" ToolTip="Storage3"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvtxtLStorage3" runat="server" ControlToValidate="txtLStorage3"
                                                        Display="None" SetFocusOnError="true" ValidationGroup="AddLowLiqSec1" ErrorMessage="Enter Storage3">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%" valign="top" height="30px">
                                                    <asp:Button ID="btnModifyL" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddLowLiqSec1" Enabled="false" OnClick="btnModifyL_Click" />
                                                    <asp:Button ID="btnClearL" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearL_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvLowLiqDetails" runat="server" AutoGenerateColumns="false" Visible="false"
                                            ShowFooter="false" Width="100%" OnRowDataBound="gv_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblHighCollateral_Type_ID" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbMRemarks" runat="server" TargetControlID="txtRemarks"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <%--  <asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="100px" MaxLength="20" Visible="false" />--%>
                                                        <cc1:ComboBox ID="txtCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                            Width="100px" MaxLength="30" Visible="false">
                                                        </cc1:ComboBox>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbMAddress" runat="server" TargetControlID="txtAddress"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbMStorage1" runat="server" TargetControlID="txtStorage1"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbMStorage2" runat="server" TargetControlID="txtStorage2"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbMStorage3" runat="server" TargetControlID="txtStorage3"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvLowLiqDetails1" runat="server" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdLSelect_CheckedChanged" Text="" />
                                                        <%----%></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateral_Type_ID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                        <%--<asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <%-- <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left" ReadOnly = "true" TextMode = "MultiLine" />--%>
                                                        <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%-- </div>--%>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Commodities" ID="tabCommodity" CssClass="tabpan"
                                BackColor="Red" ToolTip=" Commodity Details" Width="99%">
                                <HeaderTemplate>
                                    Commodities</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="tabCommo" CssClass="stylePanel" GroupingText="Commodities Details"
                                        HorizontalAlign="Center" Width="99%" ScrollBars="None">
                                        <%-- <div id="div3">--%>
                                        <asp:Label ID="lblCommodityDetails" runat="server" Text="No Security Details are Available"
                                            Visible="False" Font-Size="Small" Font-Bold="False" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCCollSecurities" runat="server" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblCSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblCMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCCollSecurities" runat="server" ReadOnly="true" ToolTip="Collateral Securities"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCCollateralReferenceNo" runat="server" Text="Collateral Ref. Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCCollRefNo" runat="server" Width="60%" ReadOnly="true" ToolTip="Collateral Ref. Number"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCRemarks" TextMode="MultiLine" MaxLength="100" ToolTip="Remarks"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <cc1:ComboBox ID="txtCCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                        AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                        Width="100px" MaxLength="30">
                                                    </cc1:ComboBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvLCity" runat="server" ControlToValidate="txtLCity"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter City" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCAddress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCAddress" TextMode="MultiLine" MaxLength="100" ToolTip="Address"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvLAddress" runat="server" ControlToValidate="txtLAddress"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Address" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCStorage1" runat="server" CssClass="styleReqFieldLabel" Text="Storage1"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCStorage1" TextMode="MultiLine" MaxLength="100" ToolTip="Storage1"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvtxtLStorage1" runat="server" ControlToValidate="txtLStorage1"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage1" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCStorage2" runat="server" CssClass="styleReqFieldLabel" Text="Storage2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCStorage2" TextMode="MultiLine" MaxLength="100" ToolTip="Storage2"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--  <asp:RequiredFieldValidator ID="rfvtxtLStorage2" runat="server" ControlToValidate="txtLStorage2"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage2" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCStorage3" runat="server" CssClass="styleReqFieldLabel" Text="Storage3"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCStorage3" TextMode="MultiLine" MaxLength="100" ToolTip="Storage3"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvtxtLStorage3" runat="server" ControlToValidate="txtLStorage3"
                                                        Display="None" SetFocusOnError="true" ValidationGroup="AddLowLiqSec1" ErrorMessage="Enter Storage3">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%" valign="top" height="30px">
                                                    <asp:Button ID="btnModifyC" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddLowLiqSec1" Enabled="false" OnClick="btnModifyC_Click" />
                                                    <asp:Button ID="btnClearC" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearC_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvCommoDetails" runat="server" AutoGenerateColumns="False" Visible="false"
                                            ShowFooter="false" Width="99%" OnRowDataBound="gv_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblHighCollateral_Type_ID" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%-- <asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbCRemarks" runat="server" TargetControlID="txtRemarks"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <%--  <asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="100px" MaxLength="20" Visible="false" />--%>
                                                        <cc1:ComboBox ID="txtCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                            Width="100px" MaxLength="30" Visible="false">
                                                        </cc1:ComboBox>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbCAddress" runat="server" TargetControlID="txtAddress"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbCStorage1" runat="server" TargetControlID="txtStorage1"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbCStorage2" runat="server" TargetControlID="txtStorage2"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbCStorage3" runat="server" TargetControlID="txtStorage3"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="gvCommoDetails1" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                                            Width="99%" OnRowDataBound="gv_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdCSelect_CheckedChanged" Text="" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateral_Type_ID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHighCollateral_Type_ID" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%-- <asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <%--  <asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="100px" MaxLength="20" Visible="false" />--%>
                                                        <%-- <cc1:ComboBox ID="txtCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                            Width="100px" MaxLength="30" Visible="false">
                                                        </cc1:ComboBox>--%>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                         <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' />
                                                    </ItemTemplate> 
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle  HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--</div>--%>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" HeaderText="Financial Securities" ID="tabFinance" CssClass="tabpan"
                                BackColor="Red" ToolTip="Financial Security Details" Width="99%">
                                <HeaderTemplate>
                                    Financial Securities
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <%--  <div id="div4">--%>
                                    <asp:Panel runat="server" ID="tabFin" CssClass="stylePanel" GroupingText="Financial Securities"
                                        HorizontalAlign="Center" Width="99%" ScrollBars="None">
                                        <asp:Label ID="lblFinDetails" runat="server" Text="No Security Details are Available"
                                            Visible="False" Font-Size="Small" Font-Bold="False" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCollSecurities" runat="server" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblFSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblFMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFCollSecurities" runat="server" ReadOnly="true" ToolTip="Collateral Securities"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCollateralReferenceNo" runat="server" Text="Collateral Ref. Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFCollRefNo" runat="server" Width="60%" ReadOnly="true" ToolTip="Collateral Ref. Number"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFRemarks" TextMode="MultiLine" MaxLength="100" ToolTip="Remarks"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCity" runat="server" CssClass="styleReqFieldLabel" Text="City"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <cc1:ComboBox ID="txtFCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                        AppendDataBoundItems="true" CaseSensitive="false" AutoCompleteMode="SuggestAppend"
                                                        Width="100px" MaxLength="30">
                                                    </cc1:ComboBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvLCity" runat="server" ControlToValidate="txtLCity"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter City" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFAddress" runat="server" CssClass="styleReqFieldLabel" Text="Address"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFAddress" TextMode="MultiLine" MaxLength="100" ToolTip="Address"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvLAddress" runat="server" ControlToValidate="txtLAddress"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Address" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFStorage1" runat="server" CssClass="styleReqFieldLabel" Text="Storage1"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFStorage1" TextMode="MultiLine" MaxLength="100" ToolTip="Storage1"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvtxtLStorage1" runat="server" ControlToValidate="txtLStorage1"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage1" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFStorage2" runat="server" CssClass="styleReqFieldLabel" Text="Storage2"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFStorage2" TextMode="MultiLine" MaxLength="100" ToolTip="Storage2"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--  <asp:RequiredFieldValidator ID="rfvtxtLStorage2" runat="server" ControlToValidate="txtLStorage2"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Storage2" ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFStorage3" runat="server" CssClass="styleReqFieldLabel" Text="Storage3"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFStorage3" TextMode="MultiLine" MaxLength="100" ToolTip="Storage3"
                                                        onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvtxtLStorage3" runat="server" ControlToValidate="txtLStorage3"
                                                        Display="None" SetFocusOnError="true" ValidationGroup="AddLowLiqSec1" ErrorMessage="Enter Storage3">
                                                    </asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%" valign="top" height="30px">
                                                    <asp:Button ID="btnModifyF" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddLowLiqSec1" Enabled="false" OnClick="btnModifyF_Click" />
                                                    <asp:Button ID="btnClearF" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnModifyF_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvFinDetails" runat="server" AutoGenerateColumns="False" Visible="false"
                                            ShowFooter="false" Width="99%" OnRowDataBound="gv_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblHighCollateral_Type_ID" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%--   <asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(300)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbFremarks" runat="server" TargetControlID="txtRemarks"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <%-- <cc1:FilteredTextBoxExtender ID="ftbRemarks" runat="server" TargetControlID="txtRemarks"
                                                            FilterType="Custom" FilterMode="InvalidChars" InvalidChars="<>&'">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                        <asp:TextBox ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <%-- <asp:TextBox ID="txtCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Left"
                                                                AutoPostBack="false" Width="100px" MaxLength="20" Visible="false" />--%>
                                                        <cc1:ComboBox ID="txtCity" runat="server" CssClass="WindowsStyle" DropDownStyle="DropDown"
                                                            AppendDataBoundItems="true" CaseSensitive="false" AutoPostBack="true" AutoCompleteMode="SuggestAppend"
                                                            Width="100px" MaxLength="30" Visible="false">
                                                        </cc1:ComboBox>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbFAddress" runat="server" TargetControlID="txtAddress"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' Style="text-align: Left"
                                                            ReadOnly="true" TextMode="MultiLine" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbFStorage1" runat="server" TargetControlID="txtStorage1"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbFStorage2" runat="server" TargetControlID="txtStorage2"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            AutoPostBack="false" Width="112px" TextMode="MultiLine" onkeyup="maxlengthfortxt(100)"
                                                            Visible="false" />
                                                        <cc1:FilteredTextBoxExtender ID="ftbFStorage3" runat="server" TargetControlID="txtStorage3"
                                                            FilterType="Custom" FilterMode="ValidChars" ValidChars="/\(),.abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 ">
                                                        </cc1:FilteredTextBoxExtender>
                                                        <asp:TextBox ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' Style="text-align: Left"
                                                            TextMode="MultiLine" ReadOnly="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Add" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%-- </div>--%>
                                        <asp:GridView ID="gvFindetails1" runat="server" AutoGenerateColumns="False" ShowFooter="false"
                                            Width="99%" OnRowDataBound="gv_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdFSelect_CheckedChanged" Text="" />
                                                        <%----%></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateral_Type_ID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <%--<asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />--%>
                                                        <asp:Label ID="lblHighCollateral_Type_ID" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Reference No">
                                                    <ItemTemplate>
                                                        <%--   <asp:LinkButton ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />--%>
                                                        <asp:Label ID="lblCollateralReferenceNo" runat="server" Text='<%#Eval("Collateral_Item_Ref_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="City">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage1">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage1" runat="server" Text='<%#Eval("Storage1") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage2">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage2" runat="server" Text='<%#Eval("Storage2") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Storage3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStorage3" runat="server" Text='<%#Eval("Storage3") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                        <tr>
                            <td>
                                <asp:ValidationSummary ID="vgAdd" runat="server" ValidationGroup="vgAdd" CssClass="styleMandatoryLabel"
                                    HeaderText="Correct the following validation(s):" ShowSummary="true" />
                                <asp:CustomValidator ID="cvCollateralValuation" runat="server" CssClass="styleMandatoryLabel"
                                    Enabled="true" Width="98%" />
                            </td>
                        </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<cc1:ModalPopupExtender ID="ModalPopupExtenderPassword" runat="server" TargetControlID="btnModal"
        PopupControlID="PnlPassword" BackgroundCssClass="styleModalBackground" Enabled="True">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="PnlPassword" Style="display: none; vertical-align: middle" runat="server"
        BorderStyle="Solid" BackColor="White" Width="50%">
        <asp:UpdatePanel ID="upPass" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvCollateralCapture" runat="server" AutoGenerateColumns="false"
                    Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Collateral_ItemRefNo" HeaderText="Collateral Item Referece No" />
                        <asp:BoundField DataField="Capture_Date" HeaderText="Collateral Capture Date" />
                        <asp:BoundField DataField="Units" HeaderText="No of Unit" />
                        <asp:BoundField DataField="FaceValue" HeaderText = "Unit Face  Value" />
                        <asp:BoundField DataField="MarketValue" HeaderText="Current Market Value" />
                        
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnCancelModal" Text="Close" OnClick="btnCancelModal_Click" runat="server" CssClass="styleSubmitButton" />
                <columns>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>--%>
</asp:Content>
