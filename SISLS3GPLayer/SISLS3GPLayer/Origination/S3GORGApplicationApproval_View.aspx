<%@ Page Title="S3G - Application Approval" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GORGApplicationApproval_View.aspx.cs" Inherits="S3GORGApplicationApproval_View" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="stylePageHeading">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Text="Application Approval - Details" ID="lblHeading" CssClass="styleInfoLabel">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="styleFieldAlign">
                <table align="center">
                    <tr>
                        <td>
                            <asp:Label ID="lblfromDate" Text="From Date" runat="server"></asp:Label>&nbsp;
                            <asp:TextBox ID="txtFromDate" runat="server" Width="140px" MaxLength="12"></asp:TextBox>
                            <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            <asp:RequiredFieldValidator ID="rfvDateOfIncorp" runat="server" Display="None" ErrorMessage="Please enter the  From Date"
                                ControlToValidate="txtFromDate" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                TargetControlID="txtFromDate" PopupButtonID="imgFromDate" ID="CalendarExtender1"
                                Enabled="True">
                            </cc1:CalendarExtender>
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="To Date" runat="server"></asp:Label>&nbsp;
                            <asp:TextBox ID="txtToDate" runat="server" Width="140px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                            <asp:Image ID="imgToDate" runat="server" ImageUrl="~/Images/calendaer.gif" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                ErrorMessage="Please enter the To Date" ControlToValidate="txtToDate" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <cc1:CalendarExtender runat="server" Format="dd/MM/yyyy" OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                TargetControlID="txtToDate" PopupButtonID="imgToDate" ID="CalendarExtender2"
                                Enabled="True">
                            </cc1:CalendarExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" Text="Go" runat="server" CssClass="styleSubmitButton" OnClick="btnGo_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <br />
                <asp:GridView ID="grvApprovalDetails" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-CssClass="styleGridHeader" Width="33%" OnRowCommand="grvApprovalDetails_RowCommand">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="styleGridHeader"
                            HeaderStyle-Width="8%">
                            <HeaderTemplate>
                                <asp:Label ID="lblEdit" runat="server" Text="Query"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument='<%# Bind("Application_Process_ID") %>'
                                    CssClass="styleGridQuery" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Application Number" ItemStyle-HorizontalAlign="Center"
                            HeaderStyle-CssClass="styleGridHeader" HeaderStyle-Width="25%">
                            <ItemTemplate>
                                <asp:Label ID="lblApplicationNumber" runat="server" Text='<%# Bind("Application_Number") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle CssClass="styleGridHeader" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px;" align="center">
                <span runat="server" id="lblErrorMessage" cssclass="styleMandatory" style="color: Red">
                </span>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 5px;" align="center">
                <asp:Button ID="btnApprove" CssClass="styleSubmitButton" CausesValidation="false"
                    Text="Approval" runat="server" OnClick="btnApprove_Click"></asp:Button>
                &nbsp;
                <%--      <asp:Button ID="btnShowAll" OnClick="btnShowAll_Click" runat="server" Text="Show All"
                    CssClass="styleSubmitButton" />--%>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ValidationSummary ID="vsLOB" runat="server" ShowMessageBox="false" CssClass="styleMandatoryLabel" />
            </td>
        </tr>
    </table>
</asp:Content>
