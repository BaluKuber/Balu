<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GCLTCollateralValuation.aspx.cs" Inherits="Collateral_S3GCLTCollateralValuation"
    Title="Untitled Page" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/S3GCustomerAddress.ascx" TagName="S3GCustomerAddress"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/PageNavigator.ascx" TagName="PageNavigator" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/LOBMasterView.ascx" TagName="LOV" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function window.onerror() { return true; }


        function Resize() {
            if (document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null) {
                if (document.getElementById('divMenu').style.display == 'none') {
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
                }
                else {
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - 270;
                }
            }
        }

        function showMenu(show) {
            if (show == 'T') {

                if (document.getElementById('divGrid1') != null) {
                    document.getElementById('divGrid1').style.width = "800px";
                    document.getElementById('divGrid1').style.overflow = "scroll";
                }

                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';

                if (document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - 270;
            }
            if (show == 'F') {
                if (document.getElementById('divGrid1') != null) {
                    document.getElementById('divGrid1').style.width = "960px";
                    document.getElementById('divGrid1').style.overflow = "auto";
                }

                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';

                if (document.getElementById('ctl00_ContentPlaceHolder1_divDetails') != null)
                    (document.getElementById('ctl00_ContentPlaceHolder1_divDetails')).style.width = screen.width - document.getElementById('divMenu').style.width - 60;
            }
        }
 
         
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <%-- Design for 1st Row       --%>
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="Collateral Valuation" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        <input id="HidConfirm" type="hidden" runat="server" />
                    </td>
                </tr>
                <%-- Design for space in between Row       --%>
                <tr>
                    <td class="styleFieldLabel">
                        &nbsp;
                    </td>
                </tr>
                <%-- Design for 2nd Row       --%>
                <tr>
                    <td>
                        <cc1:TabContainer ID="tcCollateralValuation" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel"
                            OnActiveTabChanged="tcCollateralValuation_TabIndexChanged" AutoPostBack="false"
                            Width="99%" ScrollBars="None">
                            <%-- Design Customer/Agent selection--%>
                            <cc1:TabPanel runat="server" HeaderText="Customer / Agent" ID="tabCustORAgent" CssClass="tabpan"
                                BackColor="Red" ToolTip="Customer / Agent" Width="99%">
                                <HeaderTemplate>
                                    Customer / Agent</HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%" align="center">
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
                                                <asp:GridView ID="grvPaging" runat="server" AutoGenerateColumns="False" Width="100%">
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
                                                                            <cc1:FilteredTextBoxExtender ID="filterName" runat="server" TargetControlID="txtHeaderSearch1"
                                                                                FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                                                ValidChars=". " />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%--<asp:Label ID="lblAppropriationID" runat="server" Text='<%# Bind("Appropriation_ID") %>'></asp:Label>--%>
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidCA_ID" runat="server" Value='<%# Eval("CA_ID")%>' />
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
                                                                            <cc1:FilteredTextBoxExtender ID="filterLOB" runat="server" TargetControlID="txtHeaderSearch2"
                                                                                FilterType="custom,Numbers,LowercaseLetters,UppercaseLetters" Enabled="True"
                                                                                ValidChars="-/ " />
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
                                                                <%--<asp:CheckBox ID="chkIsActive" Enabled = "false" runat  ="server" Checked ='<%# Bind("Is_Active") %>' />--%>
                                                                <%--<asp:Label ID="lblActive" runat="server" Text='<%# Bind("Name") %>'></asp:Label>--%>
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
                                                <input type="hidden" id="hdnSortDirection" runat="server">
                                                <input type="hidden" id="hdnSortExpression" runat="server" />
                                                <input type="hidden" id="hdnSearch" runat="server" />
                                                <input type="hidden" id="hdnOrderBy" runat="server" />
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
                            <%-- Design for Customer Details--%>
                            <cc1:TabPanel runat="server" HeaderText="General" ID="tabgeneral" CssClass="tabpan"
                                BackColor="Red" ToolTip="General" Width="99%">
                                <HeaderTemplate>
                                    General</HeaderTemplate>
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlCollateral" Width="99%" runat="server" GroupingText="Collateral Capture Details"
                                                    CssClass="stylePanel">
                                                    <%--  <asp:GridView ID="gvCapture" Width ="99%" runat ="server"  AutoGenerateColumns ="true" />--%>
                                                    <table width="100%">
                                                        <%--     <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label8" runat="server" Text="Line of Business" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLOB" runat="server" Text="" ReadOnly ="true"  ToolTip ="Line of Business" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label9" runat="server" Text="Branch" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBranch" runat="server" Text="" ReadOnly ="true" ToolTip ="Branch"  />
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label6" runat="server" Text="Collateral Capture Number" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCapNo" runat="server" Text="" ReadOnly="true" ToolTip="Collateral Capture Number" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label7" runat="server" Text="Collateral Captured Date" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCapDate" runat="server" Text="" ReadOnly="true" ToolTip="Collateral Captured Date" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label10" runat="server" Text="Collateral Valuation Number" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtValNo" runat="server" Text="" ReadOnly="true" ToolTip="Collateral Valuation Number" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label11" runat="server" Text="Collateral Valuation Date" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtValuationDate" runat="server" Text="" ReadOnly="true" ToolTip="Collateral Valuation Date" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                &nbsp; &nbsp;
                                                <%-- For Customer Details --%>
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
                                                <%-- For Collection Agent Details --%>
                                                <asp:Panel ID="pnlAgent" Width="99%" runat="server" GroupingText="Collection Agent Details"
                                                    CssClass="stylePanel">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label1" runat="server" Text="Collection Agent Code" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtACode" runat="server" Text="" ReadOnly="true" ToolTip="Collection Agent Code" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="lblAName" runat="server" Text="Collection Agent Name" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAName" runat="server" Text="" ReadOnly="true" ToolTip="Collection Agent Name" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label2" runat="server" Text="Collection Agent Address" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAaddress" TextMode="MultiLine" runat="server" Text="" ReadOnly="true"
                                                                    ToolTip="Collection Agent Address" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label3" runat="server" Text="City" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAcity" runat="server" Text="" ReadOnly="true" ToolTip="city" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label4" runat="server" Text="State" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAState" runat="server" Text="" ReadOnly="true" ToolTip="State" />
                                                            </td>
                                                            <td class="styleFieldLabel">
                                                                <asp:Label ID="Label5" runat="server" Text="Country" />
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtACountry" runat="server" Text="" ReadOnly="true" ToolTip="Country" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <asp:Panel ID="pnlAccDetails" Width="99%" Height="100%" runat="server" GroupingText="Account Details"
                                                    Visible="false" CssClass="stylePanel" HorizontalAlign="Center">
                                                    <div style="overflow: auto; height: 150px;" id="divAcc">
                                                        <asp:GridView ID="gvAccDetails" runat="server" AutoGenerateColumns="false" Width="99%"
                                                            ShowFooter="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSNoMedium" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label></ItemTemplate>
                                                                    <ItemStyle Width="3%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="Line of Business" DataField="LOB_Code" ItemStyle-Width="15%" />
                                                                <asp:BoundField HeaderText="Prime Account Number" DataField="Prime_Account_Number"
                                                                    ItemStyle-Width="15%" />
                                                                <asp:BoundField HeaderText="Sub Account Number" DataField="Sub_Account_Number" ItemStyle-Width="15%" />
                                                                <asp:BoundField HeaderText="Account Creation Date" DataField="Account_Date" ItemStyle-Width="11%" />
                                                                <asp:BoundField HeaderText="Finance Amount" DataField="Original_Amount_Financed"
                                                                    ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Right" />
                                                                <asp:BoundField HeaderText="Maturity Date" DataField="Maturity_Date" ItemStyle-Width="11%" />
                                                                <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-Width="15%" />
                                                            </Columns>
                                                            <RowStyle HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" align="center">
                                                <asp:Button runat="server" ID="btnSave" ValidationGroup="btnSave" CssClass="styleSubmitButton"
                                                    OnClick="btnSave_Click" Text="Save" OnClientClick="return fnCheckPageValidators('Save');" />
                                                &nbsp;
                                                <%--    OnClientClick="fnCheckPageValidators('btnSave');"--%>
                                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                                                    CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                                                &nbsp;
                                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                                                    CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <%-- Design for High Liquid Securities --%>
                            <cc1:TabPanel runat="server" HeaderText="High Liquid Securities" ID="tabHighLiq"
                                CssClass="tabpan" BackColor="Red" ToolTip="High Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    High Liquid Securities</HeaderTemplate>
                                <ContentTemplate>
                                    &nbsp;
                                    <asp:Panel runat="server" ID="pnlHighLiqDetails" CssClass="stylePanel" GroupingText="High Liquid Security Details"
                                        HorizontalAlign="Center">
                                        <%--<div style =" overflow :scroll;" id="divHigh">--%>
                                        <asp:Label ID="lblHighLiqDetails" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCollSecurities" runat="server" Text="Collateral Securities"></asp:Label><asp:Label
                                                        ID="lblhSlNo" runat="server" Visible="false"></asp:Label><asp:Label ID="lblhMode"
                                                            runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthCollSecurities" runat="server" ReadOnly="true" ToolTip="Collateral Securities"></asp:TextBox><%--<asp:DropDownList ID="ddlhCollSecurities" ToolTip="Collateral Securities" runat="server">
                                                    </asp:DropDownList>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhIssuedBy" runat="server" Text="Issued By"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthIssuedBy" runat="server" MaxLength="20" ReadOnly="true" ToolTip="Issued By"
                                                        Width="100px"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhAsquisitionDate" runat="server" Text="Acquisition Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthAsquisitionDate" runat="server" ReadOnly="true" ToolTip="Acquisition Date"
                                                        Width="90px"></asp:TextBox><cc1:CalendarExtender ID="CEhAsquisitionDate" runat="server"
                                                            Enabled="false" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                            PopupButtonID="Image1" TargetControlID="txthAsquisitionDate">
                                                        </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhMaturityDate" runat="server" Text="Maturity Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthMaturityDate" runat="server" ReadOnly="true" ToolTip="Maturity Date"
                                                        Width="90px"></asp:TextBox><cc1:CalendarExtender ID="CEhMaturityDate" runat="server"
                                                            Enabled="false" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                            PopupButtonID="Image1" TargetControlID="txthMaturityDate">
                                                        </cc1:CalendarExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhFaceValue" runat="server" Text="Face Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthFaceValue" runat="server" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                        ReadOnly="true" Style="text-align: Right" ToolTip="Face Value" Width="90px"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhAvailableUnits" runat="server" Text="Available Units"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthAvailableUnits" runat="server" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                        ReadOnly="true" Style="text-align: Right" ToolTip="Available Units" Width="90px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCurrentValue" runat="server" CssClass="styleReqFieldLabel" Text="Current Value Per Unit"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthCurrentValue" runat="server" AutoPostBack="true" MaxLength="8"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" OnTextChanged="txthCurrentValue_TextChanged"
                                                        Style="text-align: Right" ToolTip="Current Value Per Unit" Width="90px"></asp:TextBox><cc1:FilteredTextBoxExtender
                                                            ID="FtexhCurrentValue" runat="server" Enabled="True" FilterType="custom,Numbers"
                                                            TargetControlID="txthCurrentValue" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvhCurrentValue" runat="server" ControlToValidate="txthCurrentValue"
                                                        Display="None" ErrorMessage="Enter Current Value Per Unit" SetFocusOnError="true"
                                                        ValidationGroup="AddHighLiqSec1">
                                                    
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhCurrentTotalValue" runat="server" Text="Current Total Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthCurrentTotalValue" runat="server" MaxLength="15" onkeypress="fnAllowNumbersOnly(true,false,this)"
                                                        ReadOnly="true" Style="text-align: Right" ToolTip="Current Total Value" Width="90px"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhValuationDate" runat="server" CssClass="styleReqFieldLabel" Text="Valuation Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthValuationDate" runat="server" ReadOnly="true" ToolTip="Valuation Date"></asp:TextBox><cc1:CalendarExtender
                                                        ID="CEhValuationDate" runat="server" Enabled="false" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate"
                                                        PopupButtonID="Image1" TargetControlID="txthValuationDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblhRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txthRemarks" runat="server" onkeypress="trimStartingSpace(this)"
                                                        onkeyup="maxlengthfortxt(100);" TextMode="MultiLine" ToolTip="Remarks">
                                                    
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" height="30px" valign="top" width="100%">
                                                    <asp:Button ID="btnModifyH" runat="server" CssClass="styleSubmitShortButton" OnClick="btnModifyH_Click"
                                                        Text="Modify" ValidationGroup="AddHighLiqSec1" />
                                                    <asp:Button ID="btnClearH" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        OnClick="btnModifyH_Click" Text="Clear" />
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvHighLiqDetails1" runat="server" AutoGenerateColumns="false" OnRowDataBound="gv_RowDataBound"
                                            ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" OnCheckedChanged="rdHSelect_CheckedChanged"
                                                            AutoPostBack="true" Text="" />
                                                        <%----%></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Issued By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssue_By" runat="server" Text='<%#Eval("Issue_By") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acquisition Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAcqDate" runat="server" Text='<%#Eval("Created_On") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Eval("Maturity_Date") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Face Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFaceValue" runat="server" Text='<%#Eval("Unit_Face_Value") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="Available Units" DataField="No_Of_Units" />--%>
                                                <asp:TemplateField HeaderText="Available Units">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAVUnits" runat="server" Text='<%#Eval("No_Of_Units") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Value Per Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <asp:TemplateField HeaderText="Current Total Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotCMV" runat="server" Text='<%# Eval("tot") %>' Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                   
                                                </asp:TemplateField>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Left" />
                                        </asp:GridView>
                                        <asp:GridView ID="gvHighLiqDetails" Visible="false" runat="server" AutoGenerateColumns="false"
                                            OnRowDataBound="gv_RowDataBound" ShowFooter="true" Width="130%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label></ItemTemplate>
                                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Issued By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssue_By" runat="server" Text='<%#Eval("Issue_By") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acquisition Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAcqDate" runat="server" Text='<%#Eval("Created_On") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%# Eval("Maturity_Date") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Face Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFaceValue" runat="server" Text='<%#Eval("Unit_Face_Value") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--<asp:BoundField HeaderText="Available Units" DataField="No_Of_Units" />--%>
                                                <asp:TemplateField HeaderText="Available Units">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAVUnits" runat="server" Text='<%#Eval("No_Of_Units") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Value Per Unit">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"
                                                            Style="text-align: Right; display: none" ToolTip="Current Value Per Unit" AutoPostBack="true"
                                                            OnTextChanged="High_CMV" />
                                                        <cc1:FilteredTextBoxExtender ID="FtexTargetAmount" runat="server" TargetControlID="txtUnitCMV"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                    <FooterStyle Width="9%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <asp:TemplateField HeaderText="Current Total Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotCMV" runat="server" Text='<%# Eval("tot") %>' Style="text-align: Right" />
                                                        <asp:HiddenField ID="hid_Tot" runat="server" Value='<%# Eval("tot") %>' />
                                                        <%--<asp:TextBox id="txtHigTotCMV" runat ="server" Style="text-align: Right" />--%></ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' Style="text-align: Right" />
                                                        <asp:HiddenField ID="hid_GT" runat="server" Value='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    <FooterStyle Width="10%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                        <asp:TextBox ID="txtValDate" runat="server" Width="80px" Text='<%#Eval("Valuation_Date") %>'
                                                            Visible="false" ToolTip="Valuation Date"></asp:TextBox><asp:Image ID="imgValDate"
                                                                runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                                Visible="false" />
                                                        <cc1:CalendarExtender ID="calValDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="imgValDate" TargetControlID="txtValDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtRemarks" Visible="false" Width="100px" Text='<%# Eval("Valuation_Remarks") %>'
                                                            ToolTip="Remarks" TextMode="MultiLine" runat="server" onkeyup="maxlengthfortxt(100);" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Valuate" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <br />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                        <%-- <asp:LinkButton ID="lnkChange"  runat="server" Text="Valuate"  OnClick ="btnChange_Click" 
                                                                                CausesValidation="false"></asp:LinkButton>
                                                       <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" OnClick ="btnUpdate_Click"
                                                                              Visible="false"   CausesValidation="false"></asp:LinkButton>
                                                       <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" OnClick ="btnUndo_Click"
                                                                          Visible="false"       CausesValidation="false"></asp:LinkButton>--%></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle HorizontalAlign="Left" />
                                        </asp:GridView>
                                        <%-- </div> --%></asp:Panel>
                                </ContentTemplate>
                                <%--</asp:UpdatePanel>
                                </ContentTemplate>--%></cc1:TabPanel>
                            <%-- Design for Medium Liquid Securities       --%>
                            <cc1:TabPanel runat="server" HeaderText="Medium Liquid Securities" ID="tabMedSeq"
                                CssClass="tabpan" BackColor="Red" ToolTip="Medium Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    Medium Liquid Securities</HeaderTemplate>
                                <ContentTemplate>
                                    &nbsp;<asp:Panel runat="server" ID="pnlMedLiqDetails" CssClass="stylePanel" GroupingText="Medium Liquid Security Details"
                                        HorizontalAlign="Center">
                                        <%-- <div  style =" overflow:scroll  ;" id="divMed">--%>
                                        <asp:Label ID="lblMedLiqDetails" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMCollSecurities" runat="server" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblMSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblMMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMCollSecurities" runat="server" ReadOnly="true" ToolTip="Collateral Securities"></asp:TextBox>
                                                    <%--<asp:DropDownList ID="ddlMCollSecurities" ToolTip="Collateral Securities" runat="server">
                                                    </asp:DropDownList>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMDescription" TextMode="MultiLine" ReadOnly="true" MaxLength="100"
                                                        ToolTip="Description" onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(100);"
                                                        runat="server">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMModel" runat="server" CssClass="styleReqFieldLabel" Text="Model"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMModel" runat="server" MaxLength="4" ReadOnly="true" Width="50"
                                                        ToolTip="Model"></asp:TextBox>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMYear" runat="server" Text="Year"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMYear" runat="server" MaxLength="4" ReadOnly="true" Width="50px"
                                                        ToolTip="Year" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMRegistrationNo" runat="server" Text="Registration Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMRegistrationNo" runat="server" ReadOnly="true" ToolTip="Registration Number"
                                                        MaxLength="12">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMSerialNo" runat="server" Text="Reference Number"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMSerialNo" runat="server" ReadOnly="true" ToolTip="Reference No"
                                                        Width="90px" MaxLength="12"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMValue" runat="server" Text="Original Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMValue" runat="server" ReadOnly="true" Style="text-align: Right"
                                                        ToolTip="Original Value" Width="90px" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMCurrentMarketValue" runat="server" CssClass="styleReqFieldLabel"
                                                        Text="Current Market Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMCurrentMarketValue" runat="server" Style="text-align: Right"
                                                        Width="90px" ToolTip="Current Market Value" MaxLength="12" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexMCurrentMarketValue" runat="server" TargetControlID="txtMCurrentMarketValue"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars="." />
                                                    <asp:RequiredFieldValidator ID="rfvMCurrentMarketValue" runat="server" ControlToValidate="txtMCurrentMarketValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Current Market Value"
                                                        ValidationGroup="AddMedLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMValuationDate" runat="server" Text="Valuation Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMValuationDate" ReadOnly="true" ToolTip="Valuation Date" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CEMValuationDate" runat="server" Enabled="False" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="Image1"
                                                        TargetControlID="txtMValuationDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblMRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtMRemarks" TextMode="MultiLine" MaxLength="200" ToolTip="Remarks"
                                                        onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(200);" runat="server"></asp:TextBox>
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
                                        <asp:GridView ID="gvMedLiqDetails" runat="server" AutoGenerateColumns="false" Visible="false"
                                            OnRowDataBound="gv_RowDataBound" ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNoMedium" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label></ItemTemplate>
                                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-Width="14%"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Model" DataField="Model" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Year" DataField="Year" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Reg No" DataField="Registration_No" ItemStyle-Width="9%"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Reference Number" DataField="Serial_No" ItemStyle-Width="9%"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <%--   <asp:BoundField HeaderText="Total Units" DataField="" />
                          <asp:BoundField HeaderText="Original Value" DataField="" /> --%>
                                                <%--<asp:TemplateField HeaderText="Total Units">
                             <ItemTemplate>
                             <asp:Label ID="lblAVUnits" runat ="server" Text ="" />
                             </ItemTemplate>                        
                             
                              <ItemStyle  HorizontalAlign="Left"  />
                         </asp:TemplateField> --%>
                                                <asp:TemplateField HeaderText="Original Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgVal" runat="server" Text='<%#Eval("Value") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    <FooterStyle Width="12%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Market Value">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"
                                                            Style="text-align: Right; display: none" ToolTip="Current Market Value" AutoPostBack="true"
                                                            OnTextChanged="Medium_CMV" />
                                                        <cc1:FilteredTextBoxExtender ID="FtexTargetAmount" runat="server" TargetControlID="txtUnitCMV"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' />
                                                        <asp:HiddenField ID="hid_GT" runat="server" Value='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <%--     <asp:TemplateField HeaderText="Total Current Market Value">
                         <ItemTemplate>
                         <asp:Label ID="lblTotCMV" runat ="server" Text ="" Style="text-align: Right" />
                          <asp:TextBox id="txtHigTotCMV" runat ="server" Style="text-align: Right" />
                         </ItemTemplate>
                        
                          <FooterTemplate >
                              <asp:Label ID="lblTotal" runat ="server" Text ="" Style="text-align: Right" />
                          </FooterTemplate>
                           <ItemStyle Width="10%" HorizontalAlign="Right"  />
                          <FooterStyle HorizontalAlign ="Right" />
                        </asp:TemplateField>--%>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                        <asp:TextBox ID="txtValDate" runat="server" Width="80px" Text='<%#  Eval("Valuation_Date") %>'
                                                            Visible="false" ToolTip="Valuation Date"></asp:TextBox><asp:Image ID="imgValDate"
                                                                runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                                Visible="false" />
                                                        <cc1:CalendarExtender ID="calValDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="imgValDate" TargetControlID="txtValDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtRemarks" Text='<%# Eval("Valuation_Remarks") %>' TextMode="MultiLine"
                                                            ToolTip="Remarks" Width="100px" Visible="false" runat="server" onkeyup="maxlengthfortxt(100);" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Valuate" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <%--<RowStyle HorizontalAlign="Center" />--%></asp:GridView>
                                        <asp:GridView ID="gvMedLiqDetails1" runat="server" AutoGenerateColumns="false" OnRowDataBound="gv_RowDataBound"
                                            ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdMSelect_CheckedChanged" Text="" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Description" DataField="Description" ItemStyle-Width="14%"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Model" DataField="Model" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Year" DataField="Year" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Reg No" DataField="Registration_No" ItemStyle-Width="9%"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField HeaderText="Reference Number" DataField="Serial_No" ItemStyle-Width="9%"
                                                    ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField HeaderText="Original Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgVal" runat="server" Text='<%#Eval("Value") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Market Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                            <asp:Label ID="lblTotCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                        Style="text-align: Right" Visible="false" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <%--</div> --%>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <%-- Design for Low Liquid Securities       --%>
                            <cc1:TabPanel runat="server" HeaderText="Low Liquid Securities" ID="tabLow" CssClass="tabpan"
                                BackColor="Red" ToolTip="Low Liquid Securities" Width="99%">
                                <HeaderTemplate>
                                    Low Liquid Securities</HeaderTemplate>
                                <ContentTemplate>
                                    &nbsp;<asp:Panel runat="server" ID="pnlLowLiqDetails" CssClass="stylePanel" GroupingText="Low Liquid Security Details"
                                        HorizontalAlign="Center">
                                        <%-- <div  style =" overflow:scroll  ;" id="divLow">--%>
                                        <asp:Label ID="lblLowLiqDetails" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table width="100%">
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCollSecurities" runat="server" Text="Collateral Securities"></asp:Label><asp:Label
                                                        ID="lblLSlNo" runat="server" Visible="false"></asp:Label><asp:Label ID="lblLMode"
                                                            runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLCollSecurities" runat="server" MaxLength="8" ReadOnly="true"
                                                        Width="120px" ToolTip="Collateral Securities" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLLocationDetails" runat="server" Text="Location"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLLocationDetails" runat="server" ReadOnly="true" ToolTip="Location Details"
                                                        TextMode="MultiLine" MaxLength="60" onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(60);">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLMeasurement" runat="server" Text="Extent(Area)"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLMeasurement" runat="server" Style="text-align: Right" ReadOnly="true"
                                                        MaxLength="8" Width="80px" ToolTip="Extent(Area)" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <asp:TextBox ID="txtLExtent" runat="server" Visible="false" ReadOnly="true" ToolTip="Area"
                                                        Width="80px"></asp:TextBox>
                                                    <%--<asp:DropDownList ID="ddlLMeasurement" CssClass="styleReqFieldLabel" Width="70px"
                                                        runat="server" ToolTip="Extent(Area)">
                                                    </asp:DropDownList>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLUnitRate" runat="server" Text="Unit Rate"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLUnitRate" runat="server" Style="text-align: Right" MaxLength="8"
                                                        ReadOnly="true" Width="120px" ToolTip="Unit Rate" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCurrentValuePerUnit" runat="server" CssClass="styleReqFieldLabel"
                                                        Text="Current Value Per Unit"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLCurrentValuePerUnit" runat="server" Style="text-align: Right"
                                                        MaxLength="9" Width="120px" ToolTip="Current Value Per Unit" AutoPostBack="true"
                                                        OnTextChanged="txtLCurrentValueUnit_TextChanged" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="FtexLCurrentValuePerUnit" runat="server" TargetControlID="txtLCurrentValuePerUnit"
                                                        FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="rfvLCurrentValuePerUnit" runat="server" ControlToValidate="txtLCurrentValuePerUnit"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Current Value Per Unit"
                                                        ValidationGroup="AddLowLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLCurrentTotalValue" runat="server" Text="Current Total Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLCurrentTotalValue" runat="server" Style="text-align: Right"
                                                        Width="90px" ReadOnly="true" ToolTip="Current Total Value" MaxLength="15" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLValuationDate" runat="server" CssClass="styleReqFieldLabel" Text="Valuation Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLValuationDate" ToolTip="Valuation Date" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CELValuationDate" runat="server" Enabled="false" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="Image1"
                                                        TargetControlID="txtLValuationDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblLRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtLRemarks" TextMode="MultiLine" MaxLength="200" ToolTip="Remarks"
                                                        onkeypress="trimStartingSpace(this)" onkeyup="maxlengthfortxt(200);" runat="server"></asp:TextBox>
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
                                            OnRowDataBound="gv_RowDataBound" ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNoMedium" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label></ItemTemplate>
                                                    <ItemStyle Width="3%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Details">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocDetails" runat="server" Text='<%#Eval ("Location_Details") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Measurement">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAVUnits" runat="server" Text='<%#Eval("Measurement") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%#Eval("Unit_Rate") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="11%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVal" runat="server" Text='<%#Eval("Value") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Value Per Unit">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"
                                                            Style="text-align: Right; display: none" ToolTip="Current Value Per Unit" AutoPostBack="true"
                                                            OnTextChanged="Low_CMV" />
                                                        <cc1:FilteredTextBoxExtender ID="FtexTargetAmount" runat="server" TargetControlID="txtUnitCMV"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <asp:TemplateField HeaderText="Current Total Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotCMV" runat="server" Text='<%#Eval("tot") %>' Style="text-align: Right" />
                                                        <asp:HiddenField ID="hid_Tot" runat="server" Value='<%# Eval("tot") %>' />
                                                        <%--<asp:TextBox id="txtHigTotCMV" runat ="server" Style="text-align: Right" />--%></ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' Style="text-align: Right" />
                                                        <asp:HiddenField ID="hid_GT" runat="server" Value='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                        <asp:TextBox ID="txtValDate" runat="server" Width="80px" Text='<%#Eval("Valuation_Date")%>'
                                                            Visible="false" ToolTip="Valuation Date"></asp:TextBox><asp:Image ID="imgValDate"
                                                                runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                                Visible="false" />
                                                        <cc1:CalendarExtender ID="calValDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="imgValDate" TargetControlID="txtValDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtRemarks" Text='<%# Eval("Valuation_Remarks") %>' TextMode="MultiLine"
                                                            ToolTip="Remarks" Width="100px" Visible="false" runat="server" onkeyup="maxlengthfortxt(100);" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Valuate" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <%--<RowStyle HorizontalAlign="Center" />--%></asp:GridView>
                                        <%--</div> --%>
                                        <asp:GridView ID="gvLowLiqDetails1" runat="server" AutoGenerateColumns="false" OnRowDataBound="gv_RowDataBound"
                                            ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdLSelect_CheckedChanged" Text="" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Details">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLocDetails" runat="server" Text='<%#Eval("Location_Details") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Measurement">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAVUnits" runat="server" Text='<%#Eval("Measurement") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Extent(Area)" Visible = "false" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMeasurementarea" Visible = "false" runat="server" Text='<%# Bind("Measurement_Area") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Unit Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitRate" runat="server" Text='<%#Eval("Unit_Rate") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVal" runat="server" Text='<%#Eval("Value") %>' />
                                                    </ItemTemplate>
                                                   
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>--%>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Value Per Unit">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <asp:TemplateField HeaderText="Current Total Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotCMV" runat="server" Text='<%#Eval("tot") %>' Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <%-- Design for Commodity        --%>
                            <cc1:TabPanel runat="server" HeaderText="Commodity Details" ID="tabCommodity" CssClass="tabpan"
                                BackColor="Red" ToolTip=" Commodity Details">
                                <HeaderTemplate>
                                    Commodity Details</HeaderTemplate>
                                <ContentTemplate>
                                    &nbsp;
                                    <asp:Panel runat="server" ID="pnlCommodityDetails" CssClass="stylePanel" GroupingText="Commodity Details"
                                        HorizontalAlign="Center">
                                        <%--  <div  style =" overflow:scroll  ;" id="divCommod">--%>
                                        <asp:Label ID="lblCommodityDetails" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCCollSecurities" runat="server" CssClass="styleReqFieldLabel" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblCSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblCMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCCollSecurities" ReadOnly="true" runat="server" MaxLength="8"
                                                        Width="120px" ToolTip="Collateral Securities" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCDescription" ToolTip="Description" CssClass="styleReqFieldLabel"
                                                        runat="server" Text="Description"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCDescription" TextMode="MultiLine" ReadOnly="true" ToolTip="Description"
                                                        MaxLength="100" onkeyup="maxlengthfortxt(100);" runat="server">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCUnitOfMeasure" CssClass="styleReqFieldLabel" runat="server" Text="Unit Of Measure"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCUnitOfMeasure" ReadOnly="true" runat="server" MaxLength="8"
                                                        Width="120px" ToolTip="Unit Of Measure" onkeypress="fnAllowNumbersOnly(true,false,this)">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCUnitQty" runat="server" CssClass="styleReqFieldLabel" Text="Unit Qty"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCUnitQty" MaxLength="5" Width="50px" runat="server" ReadOnly="true"
                                                        ToolTip="Unit Qty" onkeypress="fnAllowNumbersOnly(false,false,this)"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCUnitPrice" runat="server" CssClass="styleReqFieldLabel" Text="Unit Price"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCUnitPrice" MaxLength="12" Width="120px" ReadOnly="true" ToolTip="Unit Price"
                                                        onkeypress="fnAllowNumbersOnly(true,false,this)" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCCurrentValuePerUnit" runat="server" CssClass="styleRegLabel" Text="Current Value Per Unit"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCCurrentValuePerUnit" ToolTip="Current Value Per Unit" runat="server"
                                                        OnTextChanged="txtCCurrentValueUnit_TextChanged" Width="120px" AutoPostBack = "true" ></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvCCurrentValuePerUnit" runat="server" ControlToValidate="txtCCurrentValuePerUnit"
                                                        Enabled="true" Display="None" SetFocusOnError="true" ErrorMessage="Enter Current Value Per Unit"
                                                        ValidationGroup="AddCOMMODITIESLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCCurrentTotalValue" runat="server" CssClass="styleRegLabel" Text="Current Total Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCCurrentTotalValue" MaxLength="12" ReadOnly = "true" ToolTip="Current Total Value"
                                                        runat="server" onkeypress="fnAllowNumbersOnly(true,false,this)"></asp:TextBox>
                                                       
                                                   <%-- <cc1:FilteredTextBoxExtender ID="FtexCCurrentTotalValue" runat="server" TargetControlID="txtCCurrentTotalValue"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>--%>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCValuationDate" runat="server" CssClass="styleRegLabel" Text="Valuation Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCValuationDate" runat="server" ReadOnly="true" ToolTip="Valuation Date"></asp:TextBox>
                                                    <asp:Image ID="imgCDate" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                        Visible="false" />
                                                    <cc1:CalendarExtender ID="CECValuationDate" runat="server" Enabled="false" Format="dd/MM/yyyy"
                                                        PopupButtonID="Image1" TargetControlID="txtCValuationDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblCRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtCRemarks" TextMode="MultiLine" MaxLength="200" ToolTip="Remarks"
                                                        onkeyup="maxlengthfortxt(200);" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <asp:Button ID="btnModifyC" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddCOMMODITIESLiqSec1" OnClick="btnModifyC_Click" />
                                                    <asp:Button ID="btnClearC" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearC_Click" />
                                                    <%-- <input id="hdnBankId" runat="server" type="hidden"></input>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvCommoDetails" runat="server" Visible="false" AutoGenerateColumns="false"
                                            OnRowDataBound="gv_RowDataBound" ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSNoMedium" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label></ItemTemplate>
                                                    <ItemStyle Width="4%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit of Measurement">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitOfMeasure" runat="server" Text='<%#Eval("Unit_Of_Measure") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAVUnits" runat="server" Text='<%#Eval("Unit_Quantity") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%#Eval("Unit_Price") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--  <asp:TemplateField HeaderText="Total Quantity">
                             <ItemTemplate>
                             <asp:Label ID="lblTotQuantity" runat ="server" Text ="" />
                             </ItemTemplate>                        
                             
                              <ItemStyle  HorizontalAlign="Left"  /> 
                          </asp:TemplateField> --%>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Value Per Unit">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"
                                                            Style="text-align: Right; display: none" ToolTip="Current Value Per Unit" AutoPostBack="true"
                                                            OnTextChanged="Commodity_CMV" />
                                                        <cc1:FilteredTextBoxExtender ID="FtexTargetAmount" runat="server" TargetControlID="txtUnitCMV"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <asp:TemplateField HeaderText="Current Total Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotCMV" runat="server" Text='<%#Eval("tot") %>' Style="text-align: Right" />
                                                        <asp:HiddenField ID="hid_Tot" runat="server" Value='<%# Eval("tot") %>' />
                                                        <%--<asp:TextBox id="txtHigTotCMV" runat ="server" Style="text-align: Right" />--%></ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' Style="text-align: Right" />
                                                        <asp:HiddenField ID="hid_GT" runat="server" Value='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                        <asp:TextBox ID="txtValDate" runat="server" Width="80px" Text='<%# Eval("Valuation_Date") %>'
                                                            Visible="false" ToolTip="Valuation Date"></asp:TextBox><asp:Image ID="imgValDate"
                                                                runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                                Visible="false" />
                                                        <cc1:CalendarExtender ID="calValDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="imgValDate" TargetControlID="txtValDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtRemarks" Text='<%# Eval("Valuation_Remarks") %>' TextMode="MultiLine"
                                                            ToolTip="Remarks" Width="100px" Visible="false" runat="server" onkeyup="maxlengthfortxt(100);" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Valuate" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <%--<RowStyle HorizontalAlign="Center" />--%></asp:GridView>
                                        <%-- </div> --%>
                                        <asp:GridView ID="gvCommoDetails1" runat="server" AutoGenerateColumns="false" OnRowDataBound="gv_RowDataBound"
                                            ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdCSelect_CheckedChanged" Text="" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%#Eval("Description") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit of Measure">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitOfMeasure" runat="server" Text='<%#Eval("Unit_Of_Measure") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAVUnits" runat="server" Text='<%#Eval("Unit_Quantity") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Price">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%#Eval("Unit_Price") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Value Per Unit">
                                                    <ItemTemplate>
                                                        <%-- <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />--%>
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                        <%-- <asp:TextBox ID="txtUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"
                                                            Style="text-align: Right; display: none" ToolTip="Current Value Per Unit" AutoPostBack="true"
                                                            OnTextChanged="Commodity_CMV" />
                                                        <cc1:FilteredTextBoxExtender ID="FtexTargetAmount" runat="server" TargetControlID="txtUnitCMV"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <asp:TemplateField HeaderText="Current Total Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotCMV" runat="server" Text='<%#Eval("tot") %>' Style="text-align: Right" />
                                                        <%--<asp:HiddenField ID="hid_Tot" runat="server" Value='<%# Eval("tot") %>' />--%>
                                                        <%--<asp:TextBox id="txtHigTotCMV" runat ="server" Style="text-align: Right" />--%></ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                        <%--<asp:TextBox ID="txtValDate" runat="server" Width="80px" Text='<%# Eval("Valuation_Date") %>'
                                                            Visible="false" ToolTip="Valuation Date"></asp:TextBox><asp:Image ID="imgValDate"
                                                                runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                                Visible="false" />
                                                        <cc1:CalendarExtender ID="calValDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="imgValDate" TargetControlID="txtValDate">
                                                        </cc1:CalendarExtender>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                        <%-- <asp:TextBox ID="txtRemarks" Text='<%# Eval("Valuation_Remarks") %>' TextMode="MultiLine"
                                                            ToolTip="Remarks" Width="100px" Visible="false" runat="server" onkeyup="maxlengthfortxt(100);" />--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <%-- Design for Financial Investment --%>
                            <cc1:TabPanel runat="server" HeaderText="Financial Security Details" ID="tabFinance"
                                CssClass="tabpan" BackColor="Red" ToolTip="Financial Security Details" Width="99%">
                                <HeaderTemplate>
                                    Financial Securities</HeaderTemplate>
                                <ContentTemplate>
                                    &nbsp;
                                    <asp:Panel runat="server" ID="pnlFinDetails" CssClass="stylePanel" GroupingText="Financial Security Details"
                                        HorizontalAlign="Center">
                                        <asp:Label ID="lblFinDetails" runat="server" Text="No Security Details are Available"
                                            Visible="false" Font-Size="Large" Font-Bold="false" />
                                        <table>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCollSecurities" runat="server" Text="Collateral Securities"></asp:Label>
                                                    <asp:Label ID="lblFSlNo" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblFMode" runat="server" Visible="false"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFCollSecurities" ReadOnly="true" runat="server" MaxLength="8"
                                                        Width="120px" ToolTip="Collateral Securities">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFInsuranceIssuedBy" runat="server" Text="Insurance Issued By"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFInsuranceIssuedBy" ReadOnly="true" MaxLength="20" Width="150px"
                                                        ToolTip="Insurance Issued By" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFPolicyNo" runat="server" Text="Policy No"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFPolicyNo" runat="server" ReadOnly="true" MaxLength="20" Width="150px"
                                                        ToolTip="Policy No"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFPolicyValue" runat="server" Text="Policy Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFPolicyValue" MaxLength="12" Width="120px" ReadOnly="true" ToolTip="Policy Value"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFMaturityDate" runat="server" Text="Maturity Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFMaturityDate" ReadOnly="true" ToolTip="Maturity Date" runat="server"></asp:TextBox>
                                                    <asp:Image ID="imgFMaturityDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                        ToolTip="Select Maturity Date" Visible="false" />
                                                    <cc1:CalendarExtender ID="CEFMaturityDate" runat="server" Enabled="false" Format="dd/MM/yyyy"
                                                        OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" PopupButtonID="Image1"
                                                        TargetControlID="txtFMaturityDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvFMaturityDate" runat="server" ControlToValidate="txtFMaturityDate"
                                                        Display="None" SetFocusOnError="true" Enabled="false" ErrorMessage="Enter Maturity Date"
                                                        ValidationGroup="AddFINANCIALLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFCurrentMarketValue" runat="server" CssClass="styleReqFieldLabel"
                                                        Text="Current Value"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFCurrentMarketValue" MaxLength="12" Width="120px" ToolTip="Current Value"
                                                        runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvFCurrentMarketValue" runat="server" ControlToValidate="txtFCurrentMarketValue"
                                                        Display="None" SetFocusOnError="true" ErrorMessage="Enter Current Value" ValidationGroup="AddFINANCIALLiqSec1">
                                                    </asp:RequiredFieldValidator>
                                                    <cc1:FilteredTextBoxExtender ID="FtexFCurrentMarketValue" runat="server" TargetControlID="txtFCurrentMarketValue"
                                                        FilterType="custom,Numbers" Enabled="True" ValidChars=".">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFValuationDate" runat="server" CssClass="styleRegLabel" Text="Valuation Date"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFValuationDate" runat="server" ReadOnly="true" ToolTip="Valuation Date"></asp:TextBox>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                        Visible="false" />
                                                    <cc1:CalendarExtender ID="CEFValuationDate" runat="server" Enabled="false" Format="dd/MM/yyyy"
                                                        PopupButtonID="Image1" TargetControlID="txtFValuationDate">
                                                    </cc1:CalendarExtender>
                                                </td>
                                                <td class="styleFieldLabel">
                                                    <asp:Label ID="lblFRemarks" runat="server" Text="Remarks"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign">
                                                    <asp:TextBox ID="txtFRemarks" TextMode="MultiLine" MaxLength="200" ToolTip="Remarks"
                                                        onkeyup="maxlengthfortxt(200);" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" width="100%">
                                                    <asp:Button ID="btnModifyF" runat="server" CssClass="styleSubmitShortButton" Text="Modify"
                                                        ValidationGroup="AddFINANCIALLiqSec1" OnClick="btnModifyF_Click" />
                                                    <asp:Button ID="btnClearF" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                        Text="Clear" OnClick="btnClearF_Click" />
                                                    <%-- <input id="hdnBankId" runat="server" type="hidden"></input>--%>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvFinDetails" runat="server" AutoGenerateColumns="false" Visible="false"
                                            OnRowDataBound="gv_RowDataBound" ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label></ItemTemplate>
                                                    <ItemStyle Width="3%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy Issued By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyIssedBy" runat="server" Text='<%#Eval("Insurance_Issued_By") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyNo" runat="server" Text='<%#Eval("Policy_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyValue" runat="server" Text='<%#Eval("Policy_Value") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%#Eval("Maturity_Date") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Left" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right" />
                                                    </FooterTemplate>
                                                    <FooterStyle Width="12%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Market Value ">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"
                                                            Style="text-align: Right; display: none" ToolTip="Current Market Value" AutoPostBack="true"
                                                            OnTextChanged="Finaicial_CMV" />
                                                        <cc1:FilteredTextBoxExtender ID="FtexTargetAmount" runat="server" TargetControlID="txtUnitCMV"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>' Style="text-align: Right" />
                                                        <asp:HiddenField ID="hid_GT" runat="server" Value='<%# Eval("GTotal") %>' />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Total Current Value--%>
                                                <%-- <asp:TemplateField HeaderText="Total Current Market Value">
                         <ItemTemplate>
                         <asp:Label ID="lblTotCMV" runat ="server" Text ="" Style="text-align: Right" />
                           <asp:TextBox id="txtHigTotCMV" runat ="server" Style="text-align: Right" />
                         </ItemTemplate>
                        
                          <FooterTemplate >
                              <asp:Label ID="lblTotal" runat ="server" Text ="" Style="text-align: Right" />
                          </FooterTemplate>
                           <ItemStyle Width="10%" HorizontalAlign="Right"  />
                          <FooterStyle HorizontalAlign ="Right" />
                        </asp:TemplateField>--%>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                        <asp:TextBox ID="txtValDate" runat="server" Width="80px" Text='<%# Eval("Valuation_Date") %>'
                                                            Visible="false" ToolTip="Valuation Date"></asp:TextBox><asp:Image ID="imgValDate"
                                                                runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                                Visible="false" />
                                                        <cc1:CalendarExtender ID="calValDate" runat="server" Enabled="True" Format="dd/MM/yyyy"
                                                            PopupButtonID="imgValDate" TargetControlID="txtValDate">
                                                        </cc1:CalendarExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                        <asp:TextBox ID="txtRemarks" Text='<%# Eval("Valuation_Remarks") %>' TextMode="MultiLine"
                                                            ToolTip="Remarks" Width="100px" Visible="false" runat="server" onkeyup="maxlengthfortxt(100);" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnChange" runat="server" Text="Valuate" CssClass="styleGridShortButton"
                                                            OnClick="btnChange_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUpdate_Click" CausesValidation="false" />
                                                        <asp:Button ID="btnUndo" runat="server" Text="Cancel" Visible="false" CssClass="styleGridShortButton"
                                                            OnClick="btnUndo_Click" CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <%--<RowStyle HorizontalAlign="Center" />--%></asp:GridView>
                                        <asp:GridView ID="gvFinDetails1" runat="server" AutoGenerateColumns="false" OnRowDataBound="gv_RowDataBound"
                                            ShowFooter="true" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdSelect" runat="server" Checked="false" AutoPostBack="true"
                                                            OnCheckedChanged="rdFSelect_CheckedChanged" Text="" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sl.No." Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCollateralTypeID" runat="server" Text='<%# Bind("Collateral_Type_ID") %>'
                                                            Visible="false">
                                                        </asp:Label><asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SlNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Collateral Securities">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCltSecurities" runat="server" Text='<%#Eval("Collateral_Securities_Name") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy Issued By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyIssedBy" runat="server" Text='<%#Eval("Insurance_Issued_By") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyNo" runat="server" Text='<%#Eval("Policy_No") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Policy Value">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPolicyValue" runat="server" Text='<%#Eval("Policy_Value") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Maturity Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMaturityDate" runat="server" Text='<%#Eval("Maturity_Date") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalDisplay" runat="server" Text="Total" Style="text-align: Right"></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Per Unit Current Value--%>
                                                <asp:TemplateField HeaderText="Current Market Value ">
                                                    <ItemTemplate>
                                                        <%--<asp:HiddenField ID="hdnCType_ID" runat="server" Value='<%#Eval("Collateral_Type_ID") %>' />--%>
                                                        <asp:Label ID="lblUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            Style="text-align: Right" />
                                                            <asp:Label ID="lblTotCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                        Style="text-align: Right" Visible="false" />
                                                        <%-- <asp:TextBox ID="txtUnitCMV" runat="server" Text='<%#Eval("Valuation_Current_Market_Value") %>'
                                                            MaxLength="13" onkeypress="fnAllowNumbersOnly(true,false,this)" Width="100px"
                                                            Style="text-align: Right; display: none" ToolTip="Current Market Value" AutoPostBack="true"
                                                            OnTextChanged="Finaicial_CMV" />
                                                        <cc1:FilteredTextBoxExtender ID="FtexTargetAmount" runat="server" TargetControlID="txtUnitCMV"
                                                            FilterType="Custom,Numbers" Enabled="True" ValidChars=".">
                                                        </cc1:FilteredTextBoxExtender>--%>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("GTotal") %>'></asp:Label>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <%--Valuation Date--%>
                                                <asp:TemplateField HeaderText="Valuation Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblValDate" Text='<%# Eval("Valuation_Date") %>' runat="server" />
                                                        <%--<asp:TextBox ID="txtValDate" runat="server" Width="80px" Text='<%# Eval("Valuation_Date") %>'
                                                            Visible="false" ToolTip="Valuation Date"></asp:TextBox>
                                                            <asp:Image ID="imgValDate"
                                                                runat="server" ImageUrl="~/Images/calendaer.gif" ToolTip="Select Valuation Date"
                                                                Visible="false" />
                                                        <cc1:CalendarExtender ID="calValDate" runat="server" Enabled="false" Format="dd/MM/yyyy"
                                                            PopupButtonID="imgValDate" TargetControlID="txtValDate">
                                                        </cc1:CalendarExtender>--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <%--Remarks--%>
                                                <asp:TemplateField HeaderText="Remarks">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Valuation_Remarks") %>'
                                                            Style="text-align: Right" />
                                                        <%--  <asp:TextBox ID="txtRemarks" Text='<%# Eval("Valuation_Remarks") %>' TextMode="MultiLine"
                                                            ToolTip="Remarks" Width="100px" Visible="false" runat="server" onkeyup="maxlengthfortxt(100);" />--%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
            </table>
            <asp:ValidationSummary ID="vgAdd" runat="server" ValidationGroup="vgAdd" CssClass="styleMandatoryLabel"
                HeaderText="Correct the following validation(s):" ShowSummary="true" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddHighLiqSec1"
                CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                ShowSummary="true" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="AddMedLiqSec1"
                CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                ShowSummary="true" />
            <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="AddLowLiqSec1"
                CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                ShowSummary="true" />
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" ValidationGroup="AddCOMMODITIESLiqSec1"
                CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                ShowSummary="true" />
            <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="AddFINANCIALLiqSec1"
                CssClass="styleMandatoryLabel" HeaderText="Correct the following validation(s):"
                ShowSummary="true" />
            <asp:CustomValidator ID="cvCollateralValuation" runat="server" CssClass="styleMandatoryLabel"
                Enabled="true" Width="98%" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
