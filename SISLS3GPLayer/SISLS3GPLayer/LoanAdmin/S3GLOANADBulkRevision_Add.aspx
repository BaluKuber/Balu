<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GLOANADBulkRevision_Add.aspx.cs" Inherits="LoanAdmin_S3GLOANADBulkRevision_Add"
    Title="" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        function pageLoad() {
            $addHandler($get("hideModalPopupClientButton"), 'click', hideModalPopupViaClient);
        }

        function hideModalPopupViaClient() {
            //ev.preventDefault();        
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
        function onkeypress() {
            var sKeyCode = window.event.keyCode;
            alert(sKeyCode);
        }
        Date.prototype.addDays = function(days) {
            var dat = new Date(this.valueOf())
            return new Date(dat.getTime() + 24 * 60 * 60 * 1000 * days);
            //return dat;
        }

        function validateGapDays(sender, args) {

            var varGapdays = document.getElementById('<%=hdnGapdays.ClientID%>').value;

            var today = new Date();
            var today_new = today.format(sender._format);
            if (varGapdays != '') {

                var gapdays = today.addDays(varGapdays-1);

                var selectedDate = sender._selectedDate;

                var selectedDate_Format = sender._selectedDate.format(sender._format)

                if (selectedDate <= today) {
                    if (today_new != selectedDate_Format) {
                        alert('You cannot select a date less than system date');
                        sender._textbox.set_Value(today.format(sender._format));
                    }
                }
                
               else if (selectedDate > gapdays) {
                    alert('You cannot select a date greater than gap days');
                    sender._textbox.set_Value(gapdays.format(sender._format));
                }
                
            }
            else {
                alert('Gap Days not avilable select line of business');
                sender._textbox.set_Value(today.format(sender._format));
            }
        }
        function CheckIsBranchSelected() {
            var Count = 0;
            //Get target base & child control.
            var TargetBaseControl = document.getElementById('<%= this.grvBranchDetails.ClientID %>');
            // var TargetChildControl = chilId;

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n) {
                if (Inputs[n].type == 'checkbox') {
                    if (Inputs[n].checked) {
                        Count = Count + 1;
                    }
                }


            }
            if (Count == 0) {

                alert('Select atleast one Location for Processing');
                return false;
            }
            else {
                return fnCheckPageValidators();
                 //Added by Ganapathy on 22/Oct/2012 to avoid double save click
                var a = event.srcElement;
                if (a.type == "submit") {
                    a.style.display = 'none';
                }
                //End here
            }

        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlBulkRevisionInformation" Width="99%" runat="server" Style="margin-left: 4px;"
                            GroupingText="Bulk Revision Details" CssClass="stylePanel">
                            <table>
                                <tr>
                                    <td style="width: 72%;">
                                        <table>
                                            <tr>
                                                <td width="20%" align="left" class="styleFieldLabel" valign="middle">
                                                    <asp:Label runat="server" Text="Revision Number" ToolTip="Revision Number" ID="lblRevisionNumber" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td class="styleFieldAlign" align="left" width="30%">
                                                    <asp:TextBox ID="txtRevisionNumber" ToolTip="Revision Number" ReadOnly="true" runat="server" Text=""></asp:TextBox>
                                                </td>
                                                <td width="25%" align="right" class="styleFieldLabel" valign="middle">
                                                    <asp:Label runat="server" ToolTip="Revision Date" Text="Revision Date" ID="lblRevisionDate" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td width="25%" align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRevisionDate" Width="100px" runat="server" ToolTip="Revision Date" Text=""></asp:TextBox>
                                                    &nbsp;
                                                    <asp:Image ID="imgRevisionDate" runat="server" ImageUrl="~/Images/calendaer.gif"
                                                        Visible="false" />
                                                    <cc1:CalendarExtender ID="CalendarExtenderRevisionDate" runat="server" Enabled="false"
                                                        PopupButtonID="imgRevisionDate" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                        TargetControlID="txtRevisionDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvRevisionDate" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="true" runat="server" ControlToValidate="txtRevisionDate" ErrorMessage="Enter the Revision Date"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr width="100%">
                                                <td width="20%" align="left" class="styleFieldLabel" valign="middle">
                                                    <asp:Label runat="server" Text="Line of Business" ToolTip="Line of Business" ID="lblLOB" CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td width="30%" align="left" class="styleFieldAlign">
                                                    <asp:DropDownList ID="ddlLOB" runat="server" Width="163px" ToolTip="Line of Business" AutoPostBack="True" OnSelectedIndexChanged="ddlLOB_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvLineofBusiness" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="true" runat="server" ControlToValidate="ddlLOB" InitialValue="0"
                                                        ErrorMessage="Select the Line Of Business"></asp:RequiredFieldValidator>
                                                </td>
                                                <td width="25%" align="left" class="styleFieldLabel" valign="middle">
                                                    <asp:Label runat="server" Text="Revision Impact %" ToolTip="Revision Impact %" ID="lblRevisionImpactPercent"
                                                        CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td width="25%" align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtRevisionImpactPercent" ToolTip="Revision Impact %" Width="100px" Style="text-align:right" runat="server" MaxLength="6"
                                                        onkeypress="fnAllowNumbersOnly(true,true,this);"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator Display="None" ID="rfvRevisionImapct" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="true" runat="server" ControlToValidate="txtRevisionImpactPercent"
                                                        ErrorMessage="Enter the Revision Impact Percentage"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                            <tr width="100%">
                                                <td width="20%" align="left" class="styleFieldLabel" valign="middle">
                                                    <asp:Label runat="server" Text="Revision Effective Date" ToolTip="Revision Effective Date" ID="lblRevisionEffectiveDate"
                                                        CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td width="30%" align="left" class="styleFieldAlign">
                                                    <asp:TextBox Width="130px" ID="txtRevisionEffectiveDate" ToolTip="Revision Effective Date" runat="server" Text=""></asp:TextBox>
                                                    &nbsp;
                                                    <asp:Image ID="imgRevisionEffectiveDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" PopupButtonID="imgRevisionEffectiveDate"
                                                        OnClientDateSelectionChanged="validateGapDays" TargetControlID="txtRevisionEffectiveDate">
                                                    </cc1:CalendarExtender>
                                                    <asp:RequiredFieldValidator Display="None" ID="rfvRevisionEffectiveDate" CssClass="styleMandatoryLabel"
                                                        SetFocusOnError="true" runat="server" ControlToValidate="txtRevisionEffectiveDate"
                                                        ErrorMessage="Enter the Revision Effective Date"></asp:RequiredFieldValidator>
                                                </td>
                                                <td width="27%" align="left" class="styleFieldLabel" valign="middle">
                                                    <asp:Label runat="server" Text="Intervening Period Interest" ToolTip="Intervening Period Interest" ID="lblInterveningPeriodIntrest"
                                                        CssClass="styleDisplayLabel"></asp:Label>
                                                </td>
                                                <td width="23%" align="left" class="styleFieldAlign">
                                                    <asp:TextBox ID="txtInterveningPeriodIntrest" ToolTip="Intervening Period Interest" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                                    <asp:DropDownList ID="ddlInterveningPeriodIntrest" ToolTip="Intervening Period Interest" runat="server" Width="103px" Visible="false">
                                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        <asp:ListItem Value="2">No</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <%--  <asp:RequiredFieldValidator Display="None" ID="rfvInterveningPeriodIntrest" CssClass="styleMandatoryLabel"
                                                        runat="server" ControlToValidate="ddlInterveningPeriodIntrest" InitialValue="-1"
                                                        SetFocusOnError="true" ErrorMessage="Select Intervening Period Intrest"></asp:RequiredFieldValidator>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 28%;">
                                        <asp:Panel ID="panSchedule" runat="server" GroupingText="Schedule Details" CssClass="stylePanel"
                                            Width="100%">
                                            <table>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:RadioButtonList ID="rbtnSchedule" ToolTip="Schedule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rbtnSchedule_SelectedIndexChanged"
                                                            RepeatDirection="Horizontal">
                                                            <asp:ListItem Selected="True" Text="Schedule at:" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Schedule Now" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblScheduleDate" ToolTip="Schedule Date" runat="server" Text="Schedule Date"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox Width="100px" ID="txtScheduleDate" ToolTip="Schedule Date" runat="server" Text=""></asp:TextBox>
                                                        <asp:Image ID="imgScheduleDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" PopupButtonID="imgScheduleDate"
                                                            OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate" TargetControlID="txtScheduleDate">
                                                        </cc1:CalendarExtender>
                                                        <asp:RequiredFieldValidator Display="None" ID="rfvScheduleDate" CssClass="styleMandatoryLabel"
                                                            SetFocusOnError="true" runat="server" ControlToValidate="txtScheduleDate" ErrorMessage="Enter Schedule Date"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblScheduleTime" ToolTip="Schedule Time" runat="server" Text="Schedule Time"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtScheduleTime" Width="100px" runat="server" MaxLength="8" ToolTip="Should be alteast 5 minutes greater than current time if schedule date is current date"></asp:TextBox>
                                                        <span class="styleMandatory">(HH:MM AM/PM)</span>
                                                        <asp:RequiredFieldValidator Display="None" ID="rfvScheduleTime" CssClass="styleMandatoryLabel"
                                                            runat="server" ControlToValidate="txtScheduleTime" SetFocusOnError="true" ErrorMessage="Enter a Schedule Time"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="revScheduleTime" runat="server" ControlToValidate="txtScheduleTime"
                                                            ErrorMessage="Enter a valid Schedule Time" SetFocusOnError="True" Display="None"
                                                            ValidationExpression="(^([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)|(^([0-9]|[1][0-9]|[2][0-3])(\s{0,1})(AM|PM|am|pm|aM|Am|pM|Pm{2,2})$)"></asp:RegularExpressionValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Button runat="server" ID="btnGo" Text="Go" ToolTip="Go" CssClass="styleSubmitShortButton"
                                            OnClick="btnGo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlBranchDetails" Width="99%" Style="margin-left: 4px;" runat="server"
                            GroupingText="Location Details" CssClass="stylePanel">
                            <div style="overflow: auto; height: 175px; width: 100%" class="container">
                                <asp:GridView ID="grvBranchDetails" runat="server" AutoGenerateColumns="False" Style="margin-left: 3px;"
                                    HeaderStyle-CssClass="styleGridHeader" RowStyle-HorizontalAlign="Center" Width="98%"
                                    OnRowCommand="grvBranchDetails_RowCommand" OnRowDataBound="grvBranchDetails_RowDataBound">
                                    <RowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Select Location" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox AutoPostBack="true" ToolTip="Select" ID="chkCheckBox" runat="server" OnCheckedChanged="chkCheckBox_CheckedChanged"
                                                    Checked='<%#bool.Parse(Convert.ToString(DataBinder.Eval(Container.DataItem, "Select")))%>' />
                                                    <%--Checked='<%#Eval("Select")%>' />--%>
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkCheckBoxHdr" ToolTip="Select All" runat="server" AutoPostBack="true" OnCheckedChanged="chkCheckBoxHdr_CheckedChanged"
                                                    Text="Select All" />
                                            </HeaderTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranch" ToolTip="Location" runat="server" Text='<%#Eval("Location")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                            <ItemStyle HorizontalAlign="Left" Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location_Id" ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBranchId" runat="server" ToolTip="Location ID" Text='<%#Eval("Location_ID")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SJV Number" ItemStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSJVNumber" ToolTip="SJV Number" runat="server" Text='<%#Eval("SJV_Number")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SJV Date" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSJVDate" ToolTip="SJV Date" runat="server" Text='<%#Eval("SJV_Date")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View Account" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Bottom"
                                            ItemStyle-Width="10%">
                                            <ItemStyle VerticalAlign="Middle" />
                                            <ItemTemplate>
                                                <asp:Button ID="btnViewAccount" ToolTip="View" runat="server" CausesValidation="false" CssClass="styleSubmitShortButton"
                                                    Text="View" CommandName="View" CommandArgument='<%#Eval("Location_ID")%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="styleGridHeader" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Records found...
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle CssClass="styleGridHeader" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnProcess" runat="server" ToolTip="Process" CausesValidation="false" CssClass="styleSubmitButton"
                            OnClick="btnProcess_Click" Text="Process" />
                        &nbsp;
                        <asp:Button ID="btnSave" ToolTip="Save" runat="server" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            OnClientClick="return CheckIsBranchSelected();" Text="Save" />
                        &nbsp;
                        <asp:Button ID="btnEmail" runat="server" ToolTip="Email" CausesValidation="false" Enabled="false"
                            CssClass="styleSubmitButton" Text="Email" OnClick="btnEmail_Click" />&nbsp;
                        <asp:Button ID="btnPDF" runat="server" CausesValidation="false" Enabled="false" CssClass="styleSubmitButton"
                            Text="Print" ToolTip="Print" OnClick="btnPDF_Click" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                            OnClick="btnClear_Click" ToolTip="Clear" OnClientClick="return fnConfirmClear();" Text="Clear" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="styleSubmitButton"
                            OnClick="btnCancel_Click" Text="Cancel" ToolTip="Cancel" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ID="hdnGapdays" />
                        <asp:CustomValidator ID="CvBulkRevision" runat="server" ValidationGroup="btnSubmit"
                            Display="None"></asp:CustomValidator>
                        <asp:ValidationSummary ID="vsBulkRevision" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grvAccountDetails" EventName="pageindexchanging" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="updacc" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panPoupUp" runat="server" BackColor="White" Width="50%" Style="display: none;
                max-height: 55%; height: auto;">
                <table width="100%">
                    <tr>
                        <td class="stylePageHeading">
                            <asp:Label runat="server" Text="Account Details" ID="lblAccountDetails"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlPopupAccountDetails" Width="100%" runat="server" Height="100%">
                                <div class="container" style="height: 200px; overflow-x: hidden; overflow-y: scroll;">
                                    <asp:GridView runat="server" ID="grvAccountDetails" AutoGenerateColumns="true" AllowPaging="true"
                                        PagerSettings-Mode="Numeric" PageSize="10" OnPageIndexChanged="grvAccountDetails_PageIndexChanged"
                                        OnPageIndexChanging="grvAccountDetails_PageIndexChanging" Width="97%">
                                        <SelectedRowStyle BackColor="AliceBlue" />
                                        <HeaderStyle CssClass="styleGridHeader" />
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="stylePageHeading">
                            <a id="hideModalPopupClientButton" href="#" onclick="hideModalPopupViaClient();">Close</a>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderAccountDetails" runat="server" TargetControlID="btndummy"
                PopupDragHandleControlID="panPoupUp" PopupControlID="panPoupUp" BackgroundCssClass="styleModalBackground"
                DynamicServicePath="" Enabled="True" BehaviorID="programmaticModalPopupBehavior"
                RepositionMode="RepositionOnWindowResize" X="300" Y="200">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btndummy" Style="display: none;" runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
