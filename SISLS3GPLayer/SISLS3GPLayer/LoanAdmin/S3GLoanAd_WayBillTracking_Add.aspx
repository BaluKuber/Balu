<%@ Page Title="Way Bill Tracking" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true" EnableEventValidation="false"
    CodeFile="S3GLoanAd_WayBillTracking_Add.aspx.cs" Inherits="LoanAdmin_S3GLoanAd_WayBillTracking_Add" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc1" TagName="PageNavigator" Src="~/UserControls/PageNavigator.ascx" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<%@ Register TagPrefix="uc3" TagName="LOV" Src="~/UserControls/LOBMasterView.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="Way Bill Tracking- Create" ID="lblHeading" CssClass="styleDisplayLabel">
                                    </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="100%" GroupingText="Way Bill Tracking">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblLesseeName" runat="server" Text="Lessee Name" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlLesseeName" runat="server" ServiceMethod="GetLesseeNameDetails" AutoPostBack="true" Width="200px"
                                            ErrorMessage="Select Lessee Name" ValidationGroup="Main" IsMandatory="true" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblVendorName" runat="server" Text="Vendor Name" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlVendorName" runat="server" ServiceMethod="GetVendorNameDetails" AutoPostBack="true" Width="200px"
                                            ErrorMessage="Select Vendor Name" ValidationGroup="Main" IsMandatory="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblDeliveryState" runat="server" Text="Delivery State" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlDeliveryState" runat="server" ServiceMethod="GetDeliveryStateDetails" AutoPostBack="true" Width="200px"
                                            ErrorMessage="Select Delivery State" ValidationGroup="Main" IsMandatory="true" />
                                    </td>
                                    <td class="styleFieldLabel" style="width: 25%">
                                        <asp:Label ID="lblRSNO" runat="server" Text="Rent Schedule No." CssClass="styleDisplayFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <uc2:Suggest ID="ddlRSNO" runat="server" ServiceMethod="GetRSNODetails" AutoPostBack="true" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lblLCdatefrom" runat="server" Text="Lease Capitilization Date From" CssClass="styleDisplayFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtLCdatefrom" runat="server" Width="200px" TabIndex="5" MaxLength="10"
                                            AutoPostBack="true" OnTextChanged="txtLCdatefrom_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgLCdatefrom" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <%--<asp:RequiredFieldValidator ID="rfvLCdatefrom" CssClass="styleMandatoryLabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="txtLCdatefrom" ErrorMessage="Enter Lease Capitilization Date From" Display="None">
                                        </asp:RequiredFieldValidator>--%>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtLCdatefrom"
                                            PopupButtonID="imgLCdatefrom" Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:Label ID="lblLCdateTo" runat="server" Text="Lease Capitilization Date To" CssClass="styleDisplayFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" style="width: 25%">
                                        <asp:TextBox ID="txtLCdateto" runat="server" Width="200px" TabIndex="6" MaxLength="10" AutoPostBack="true" OnTextChanged="txtLCdateto_TextChanged"></asp:TextBox>
                                        <asp:Image ID="imgLCdateto" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <%--<asp:RequiredFieldValidator ID="rfvLCdateto" CssClass="styleMandatoryLabel" runat="server" ValidationGroup="Main"
                                            ControlToValidate="txtLCdateto" ErrorMessage="Enter Lease Capitilization Date To" Display="None">
                                        </asp:RequiredFieldValidator>--%>
                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtLCdateto"
                                            PopupButtonID="imgLCdateto" Enabled="True">
                                        </cc1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="4" align="center">
                                        <asp:Button runat="server" ID="btnGo" TabIndex="7" CssClass="styleSubmitButton"
                                            Text="Go" OnClick="btnGo_Click" ValidationGroup="Main" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel GroupingText="Invoice Wise Way Bill Tracking Details" ID="pnlAssetDet" runat="server" CssClass="stylePanel" Width="99%">
                            <div id="myDivForPanelScroll" runat="server" style="overflow: scroll; width: 764px">
                                <asp:GridView ID="gvWayBillTracking" runat="server" AutoGenerateColumns="false" ShowFooter="false" Width="1500px" OnRowDataBound="gvWayBillTracking_RowDataBound"
                                     DataKeyNames="RowNumber">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No.">
                                            <ItemTemplate>                                                
                                                <asp:Label ID="lblSNo" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Copy From">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="gchkFrom" runat="server" AutoPostBack="true" OnCheckedChanged="chkFrom_OnCheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Copy To">
                                            <HeaderTemplate>Copy To<asp:CheckBox ID="gchkAll" runat="server" /></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="gchkTo" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OPC State">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLocationCat_Description" runat="server" Text='<%#Bind("LocationCat_Description") %>' ToolTip="RSNO"></asp:Label>
                                                <asp:Label ID="lblLocation_ID" runat="server" Text='<%#Bind("Location_ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Rent Sch No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPANum" runat="server" Text='<%#Bind("PANum") %>'></asp:Label>
                                                <asp:Label ID="lblPA_SA_Ref_ID" runat="server" Text='<%#Bind("PA_SA_Ref_ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice No.">
                                            <ItemTemplate>
                                                <asp:Label ID="lblINVOICE_NO" runat="server" Text='<%#Bind("INVOICE_NO") %>' ToolTip="Filing Due Date"></asp:Label>
                                                <asp:Label ID="lblInvoice_ID" runat="server" Text='<%#Bind("Invoice_ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblINVOICE_DATE" runat="server" Text='<%#Bind("INVOICE_DATE") %>' ToolTip="Invoice Date"></asp:Label>
                                                <asp:Label ID="lblAccount_Activated_Date" runat="server" Text='<%#Bind("Account_Activated_Date") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotal_Bill_Amount" runat="server" Text='<%#Bind("Total_Bill_Amount") %>' ToolTip="Invoice Amount"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntity_Name" runat="server" Text='<%#Bind("Entity_Name") %>'></asp:Label>
                                                <asp:Label ID="lblEntity_ID" runat="server" Text='<%#Bind("Entity_ID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Way Bill No." HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWayBill_No" runat="server" ToolTip="Way Bill No." Text='<%# Eval("WayBill_No") %>' MaxLength="15"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvWayBill_No" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="txtWayBill_No" ErrorMessage="Enter the Way Bill No."
                                                    ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                <cc1:FilteredTextBoxExtender ID="fteWayBill_No" runat="server" TargetControlID="txtWayBill_No" FilterMode="InvalidChars"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                </cc1:FilteredTextBoxExtender>
                                                <%--<asp:Label ID="lblWayBill_No" runat="server" Text='<%# Eval("WayBill_No") %>'
                                                    ToolTip="Way Bill No." />--%>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>
                                            </EditItemTemplate>--%>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Way Bill Issue Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtWayBill_Issue_Date" runat="server" Width="70%" TabIndex="2" MaxLength="12" Text='<%# Eval("WayBill_Issue_Date") %>'></asp:TextBox>
                                                <asp:Image ID="imgWayBill_Issue_Date" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtWayBill_Issue_Date" PopupButtonID="imgWayBill_Issue_Date" Enabled="True"></cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvWayBill_Issue_Date" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="txtWayBill_Issue_Date" ErrorMessage="Enter the Way Bill Issue Date"
                                                    ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                <%--<asp:Label ID="lblWayBill_Issue_Date" runat="server" Text='<%# Eval("WayBill_Issue_Date") %>' ToolTip="Way Bill Issue Date" />--%>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>                                                
                                            </EditItemTemplate>--%>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Recd CounterFoil(Y/N)" HeaderStyle-CssClass="styleGridHeader" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIs_CounterFoil_Received" runat="server" Text='<%#Bind("Is_CounterFoil_Received") %>' Visible="false"></asp:Label>
                                                <asp:DropDownList ID="ddlIs_CounterFoil_Received" ValidationGroup="Footer" runat="server" Width="80%" >
                                                    <asp:ListItem Value="-1">Select</asp:ListItem>
                                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvIs_CounterFoil_Received" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="ddlIs_CounterFoil_Received" ErrorMessage="Enter the Recd Counterfoil(Y/N)" InitialValue="-1"
                                                    ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                <%--<asp:Label ID="lblIs_CounterFoil_Received" runat="server" Text='<%# Eval("Is_CounterFoil_Received_Desc") %>' ToolTip="Recd CounterFoil(Y/N)" />--%>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>                                                                                                
                                            </EditItemTemplate>--%>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Counter Foil Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCounterFoil_Date" AutoPostBack="true" runat="server" Width="70%" TabIndex="2" MaxLength="12" Text='<%# Eval("CounterFoil_Date") %>' OnTextChanged="txtCounterFoil_Date_TextChanged"></asp:TextBox>
                                                <asp:Image ID="imgCounterFoil_Date" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtCounterFoil_Date" PopupButtonID="imgCounterFoil_Date" Enabled="True"></cc1:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvCounterFoil_Date" SetFocusOnError="true" CssClass="styleMandatoryLabel"
                                                    runat="server" ControlToValidate="txtCounterFoil_Date" ErrorMessage="Enter the CounterFoil Date"
                                                    ValidationGroup="Footer" Display="None"></asp:RequiredFieldValidator>
                                                <%--<asp:Label ID="lblCounterFoil_Date" runat="server" Text='<%# Eval("CounterFoil_Date") %>' ToolTip="Recd CounterFoil(Y/N)" />--%>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>                                                
                                            </EditItemTemplate>--%>
                                            <HeaderStyle CssClass="styleGridHeader" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" Width="80%" TabIndex="10" MaxLength="100" TextMode="MultiLine" Text='<%# Eval("Remarks") %>'></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender ID="fteRemarks" runat="server" TargetControlID="txtRemarks" FilterMode="InvalidChars"
                                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers" InvalidChars="!@#$%^&*()_+=-~`<,.>/?';:{}[]|\" Enabled="true">
                                                </cc1:FilteredTextBoxExtender>
                                                <%--<asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' ToolTip="Remarks" />--%>
                                            </ItemTemplate>
                                            <%--<EditItemTemplate>                                                
                                            </EditItemTemplate>--%>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CausesValidation="false"
                                                    ToolTip="Edit">
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update" OnClientClick="Resize()"
                                                    ValidationGroup="Footer" ToolTip="Update"></asp:LinkButton>
                                                <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                    CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                            </EditItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PageNavigator ID="ucCustomPaging" runat="server"></uc1:PageNavigator>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="styleButtonArea">
                    <td align="center">
                        <asp:Button runat="server" ID="btnmove" TabIndex="15" CssClass="styleSubmitButton" OnClick="btnmove_Click"
                            Text="Move"/>
                        <asp:Button runat="server" ID="btnSave" TabIndex="15" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            Text="Save" ValidationGroup="Main" OnClientClick="return fnCheckPageValidators('Main');" Enabled="false" />
                        <asp:Button runat="server" ID="btnClear" CausesValidation="false" CssClass="styleSubmitButton"
                            Text="Clear" OnClientClick="return fnConfirmClear();" TabIndex="16" OnClick="btnClear_Click" />
                        <asp:Button runat="server" ID="btnExport" ValidationGroup="Main" CssClass="styleSubmitButton"
                            Text="Export" TabIndex="17" OnClick="btnExport_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary runat="server" ID="vsUserMgmt" HeaderText="Please correct the following validation(s):"
                            Height="250px" ValidationGroup="Main" CssClass="styleMandatoryLabel" Width="500px" ShowMessageBox="false"
                            ShowSummary="true" />
                        <asp:ValidationSummary ID="vsFooter" runat="server" CssClass="styleMandatoryLabel" Height="250px" ShowMessageBox="false"
                            HeaderText="Correct the following validation(s):" ValidationGroup="Footer" ShowSummary="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td id="tdHeight" runat="server"></td>
                </tr>
            </table>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function GetChildGridResize(ImageType) {
            if (ImageType == "Hide Menu") {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 20;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
            else {
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
                document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
            }
        }
        function pageLoad(s, a) {
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.width = parseInt(screen.width) - 260;
            document.getElementById('<%=myDivForPanelScroll.ClientID %>').style.overflow = "scroll";
        }
        function showMenu(show) {
            if (show == 'T') {
                document.getElementById('divMenu').style.display = 'Block';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                document.getElementById('ctl00_imgShowMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - 260;
            }
            if (show == 'F') {
                document.getElementById('divMenu').style.display = 'none';
                document.getElementById('ctl00_imgHideMenu').style.display = 'none';
                document.getElementById('ctl00_imgShowMenu').style.display = 'Block';
                (document.getElementById('<%=myDivForPanelScroll.ClientID %>')).style.width = screen.width - document.getElementById('divMenu').style.width - 50;
            }
        }

        function Resize() {
            document.getElementById('<%=tdHeight.ClientID %>').style.height = 100;
        }
   
        function CheckOne(CheckboxClientId) {
            var Checkbox = document.getElementById(CheckboxClientId);

            if (Checkbox != null) {
                var CheckBoxInputs = Checkbox.getElementsByTagName("input");
                for (var i = 0; i < CheckBoxInputs.length; i++) {
                    var CurrentIndex;
                    if (CheckBoxInputs[i].checked) {
                        CurrentIndex = i;
                    }
                    if (CheckBoxInputs[i].checked) {
                        CheckBoxInputs[i].checked = false;
                    }
                    CheckBoxInputs[parseInt(CurrentIndex)].checked = true;
                }
            }
        }

    </script>
</asp:Content>
