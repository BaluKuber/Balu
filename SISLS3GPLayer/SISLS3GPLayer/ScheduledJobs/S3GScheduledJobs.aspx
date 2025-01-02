<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GScheduledJobs.aspx.cs" Inherits="ScheduledJobs_S3GScheduledJobs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="100%" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="stylePageHeading">
                        <asp:Label runat="server" Text="Scheduled Jobs" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlScheduleDetails" Width="98%" GroupingText="Schedule Details" CssClass="stylePanel" runat="server">
                            <table width="100%" >
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Location" ID="lblLocation" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" >
                                        <asp:DropDownList ID="ddlLocation" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr >
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Job Description" ID="lblJobDescription" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" >
                                        <asp:TextBox ID="txtJobDescription" runat="server" />
                                        <cc1:FilteredTextBoxExtender ID="ftxtJobDescription" runat="server" Enabled="True"
                                            FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters" TargetControlID="txtJobDescription"
                                            ValidChars="/-&+ ">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                 <tr >
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Job" ID="Label1" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" >
                                        <asp:DropDownList ID="ddlJob" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvJob" runat="server" Display="None" InitialValue="0"
                                            ValidationGroup="btnSave" ErrorMessage="Select a Job" ControlToValidate="ddlJob"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                
                                <tr >
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Job Nature" ID="lblJobNature" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign" >
                                        <asp:DropDownList ID="ddlJobNature" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvddlJobDesc" runat="server" Display="None" InitialValue="0"
                                            ValidationGroup="btnSave" ErrorMessage="Select a Job Nature" ControlToValidate="ddlJobNature"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblStartDate" runat="server" CssClass="styleReqFieldLabel" Text="Start Date"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartDate" runat="server" Width="80px"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender ID="ftxtDocDate" runat="server" Enabled="True" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            TargetControlID="txtStartDate" ValidChars="/-">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:CalendarExtender ID="calDocDate" runat="server" Enabled="True" Format="DD/MM/YYYY"
                                            PopupButtonID="imgDocdate" TargetControlID="txtStartDate" OnClientDateSelectionChanged="checkDate_OnlyPrevSystemDate">
                                        </cc1:CalendarExtender>
                                        <asp:Image ID="imgDocdate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                                        <asp:RequiredFieldValidator ID="rfvDocDate" runat="server" ControlToValidate="txtStartDate"
                                            CssClass="styleMandatoryLabel" Display="None" ErrorMessage="Enter Start Date"
                                            SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    </td></tr><tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Start Time" ID="lblStartTime" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtStartTime" runat="server" Width="100px" />
                                        <asp:RegularExpressionValidator ID="REVScheduleTime" runat="server" ValidationGroup="btnSave"
                                            ErrorMessage="Start Time Should be HH:MM Fomat(24 Hours)" ControlToValidate="txtStartTime"
                                            SetFocusOnError="True" ValidationExpression="^([0]?[1-9]|2[0-4])(:)[0-5][0-9](:)[0-5][0-9]$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvStartTime" runat="server" Display="None" ValidationGroup="btnSave"
                                            ErrorMessage="Select Start Time" ControlToValidate="txtStartTime" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Frequency" ID="lblFrequency" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlFrequency" runat="server" onchange="FunToggleMins()" >
                                        </asp:DropDownList>
                                        &nbsp;<asp:TextBox ID="txtMinutes" Enabled="false" runat="server" Width="50px" />
                                        <cc1:FilteredTextBoxExtender ID="ftxtMinutes" runat="server" Enabled="True" FilterType="Numbers"
                                            TargetControlID="txtMinutes" ValidChars="">
                                        </cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="rfvFrequency" runat="server" Display="None" InitialValue="0"
                                            ValidationGroup="btnSave" ErrorMessage="Select a Frequency" ControlToValidate="ddlFrequency"></asp:RequiredFieldValidator>
                                    </td></tr><tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Holiday" ID="lblHoliday" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:CheckBox ID="chkHoliday" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel" vailgn="top">
                                        <asp:Label runat="server" Text="Remarks" ID="lblRemarks" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" />
                                    </td>
                                    
                                    </tr><tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Active" ID="lblActive" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:CheckBox ID="chkActive" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" CssClass="styleSubmitButton" />
                        <asp:Button ID="btnClear" Text="Clear" runat="server" OnClick="btnClear_Click" CssClass="styleSubmitButton" />
                        <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" CssClass="styleSubmitButton" />
                    </td>
                </tr>
                <tr>
                    <td class="styleFieldLabel">
                        <asp:CustomValidator ID="cvScheduledJobs" runat="server" CssClass="styleReqFieldLabel"
                            Enabled="true" Width="98%" />
                    </td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function FunToggleMins()
        {
            var ddlFrequency = document.getElementById('<%=ddlFrequency.ClientID %>');
            var txtMinutes = document.getElementById('<%=txtMinutes.ClientID %>');
            if(ddlFrequency.value == "1")
            {
                txtMinutes.disabled = false;
                txtMinutes.value = '';
            }
            else
            {
                txtMinutes.disabled = true;
                txtMinutes.value = '';
            }
        }
    </script>
</asp:Content>
