<%@ Page Language="C#" AutoEventWireup="true" CodeFile="S3GLoanAdMonthClosure.aspx.cs"
    Inherits="S3GLoanAdMonthClosure" MasterPageFile="~/Common/S3GMasterPageCollapse.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">
        function Trim(strInput)
        {
            var FieldValue = document.getElementById(strInput).value;
            document.getElementById(strInput).value = FieldValue.trim();
        }
        function fnGetCompensationValue(CompensationValue)
        {
            var exp = /_/gi;
            return parseFloat(CompensationValue.value.replace(exp, '0'));
        }                
        
        var TotalChkBx;
        var Counter;
        window.onload= function()
        {
            //Get total no. of CheckBoxes in side the GridView.
            TotalChkBx = parseInt('<%= this.grvMothEndParam.Rows.Count %>');

            //Get total no. of checked CheckBoxes in side the GridView.
            Counter = 0;
        }
        
        function HeaderClick(CheckBox, chilId) 
        {
            //Get target base & child control.
            var TargetBaseControl = document.getElementById('<%= this.grvMothEndParam.ClientID %>');

            var TargetChildControl = chilId;

            //Get all the control of the type INPUT in the base control.
            var Inputs = TargetBaseControl.getElementsByTagName("input");

            //Checked/Unchecked all the checkBoxes in side the GridView.
            for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' && Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;
            //Reset Counter
            Counter = CheckBox.checked ? TotalChkBx : 0;
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel21" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" ID="lblHeading" CssClass="styleDisplayLabel">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server" CssClass="stylePanel" Width="99%" GroupingText="Closure Details">
                            <table cellspacing="0" width="100%">
                                <tr>
                                    <td style="padding-left: 2%">
                                        <asp:Label ID="lblLineOfBusiness" runat="server" Text="Line Of Business" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlLineofBusiness" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLineOfBusiness_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="styleFieldLabel" style="padding-left: 2%">
                                        <asp:Label ID="lblMonthClosureDate" runat="server" Text="Month Closure Date"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMonthClosureDate" runat="server" ReadOnly="true">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-left: 2%">
                                        <asp:Label ID="lblFinancialYear" runat="server" Text="Financial Year" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlFinacialYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFinacialYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblYearLock" runat="server" Text="Active" Visible="false"></asp:Label>
                                    </td>
                                    <td style="padding-left: 2%">
                                        <asp:Label ID="lblClosureMonth" runat="server" Text="Closure Month" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlClosureMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClosureMonth_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label ID="lblMonthLock" runat="server" Text="Active" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <br />
                                        <asp:GridView ID="grvMothEndParam" runat="server" AutoGenerateColumns="false" Width="100%"
                                            OnRowDataBound="grvMothEndParam_RowDataBound" >
                                            <Columns>
                                                <asp:TemplateField HeaderText="Branch Id" SortExpression="Branch_id" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranchId" runat="server" Text='<%# Bind("Branch_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Branch" SortExpression="Branch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBranch" runat="server" Text='<%# Bind("Branch") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Region_id" SortExpression="Region_id" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRegionId" runat="server" Text='<%# Bind("Region_id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Status").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="StatusCode" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatusCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StatusCode").ToString() %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <%-- 
                                                                DataBinder.Eval(Container.DataItem, "Status").ToString().Replace("False", "Open").Replace("True","Closed")
                                                                   
                                                               --%>
                                                    <%--Checked='<%# Bind("Status") %>'--%>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkHdrMonthLock" Text="Select All" runat="server" CssClass="styleGridHeader"
                                                            CausesValidation="false" onclick="javascript:HeaderClick(this,'chkMonth');" AutoPostBack="True" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkMonth" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelLOB_OnCheckedChanged"
                                                            CausesValidation="false" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="styleGridHeader" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                            ValidationGroup="btnSave" CssClass="styleSubmitButton" OnClick="btnSave_Click"
                            Text="Save" />
                        &nbsp;
                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClientClick="return fnConfirmClear();"
                            CssClass="styleSubmitButton" Text="Clear" OnClick="btnClear_Click" />
                        &nbsp;
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="False"
                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:ValidationSummary ID="vsUTPA" runat="server" CssClass="styleMandatoryLabel"
                            HeaderText="Correct the following validation(s):" ShowSummary="true" ValidationGroup="btnSave" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <div style="height: 1px;">
                            <asp:RequiredFieldValidator ID="rfvLineOfBusiness" runat="server" Display="None"
                                ControlToValidate="ddlLineofBusiness" ValidationGroup="btnSave" InitialValue="0"
                                ErrorMessage="Choose a Line of Business from the list" CssClass="styleMandatoryLabel"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <br />
                            <asp:RequiredFieldValidator ID="rfvClosureMonth" runat="server" Display="None" ControlToValidate="ddlClosureMonth"
                                ValidationGroup="btnSave" InitialValue="0" ErrorMessage="Select Closure Month"
                                CssClass="styleMandatoryLabel" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <br />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
