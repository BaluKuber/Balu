<%@ Control Language="C#" AutoEventWireup="true" CodeFile="S3GCustomerAddress.ascx.cs"
    Inherits="UserControls_S3GCustomerAddress" %>
<asp:MultiView ID="MCustomerView" runat="server" ActiveViewIndex="0">
    <asp:View ID="CustomerDetailV1" runat="server">
        <table cellspacing="0" width="100%" runat="server" id="tbleV1">
            <tr>
                <td id="V1FirstColumn1">
                    <asp:Label ID="lblCustomerCode" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                </td>
                <td id="V1SecondColumn1">
                    <asp:TextBox ID="txtCustomerCode" runat="server" ReadOnly="True" TabIndex ="-1" Width="75%" ToolTip="Customer Code"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td id="V1FirstColumn2">
                    <asp:Label ID="lblCustomerName" runat="server" CssClass="styleDisplayLabel" Text="Customer Name">
                    </asp:Label>
                    &nbsp;
                </td>
                <td id="V1SecondColumn2">
                    <asp:TextBox ID="txtCustomerName" runat="server" TabIndex ="-1" ReadOnly="True" ToolTip="Customer Name"
                        Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" id="V1FirstColumn3">
                    <asp:Label ID="lblAddress" runat="server" CssClass="styleDisplayLabel" Text="Address"></asp:Label>
                </td>
                <td id="V1SecondColumn3">
                    <asp:TextBox ID="txtCusAddress" runat="server" ReadOnly="true" Width="95%" Rows="4"
                        TextMode="MultiLine" TabIndex ="-1" ToolTip="Address"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td id="V1FirstColumn4">
                    <asp:Label ID="lblPhone" runat="server" CssClass="styleDisplayLabel" Text="Phone"></asp:Label>
                </td>
                <td id="V1SecondColumn4">
                    <asp:TextBox ID="txtPhone" runat="server" ReadOnly="True" TabIndex ="-1" Width="44%" ToolTip="Phone"></asp:TextBox>
                    <asp:Label ID="lblMobile" runat="server" CssClass="styleDisplayLabel" Text="[M]"></asp:Label>
                    <asp:TextBox ID="txtMobile" runat="server" TabIndex ="-1" ReadOnly="true" Width="38%" ToolTip="Mobile"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td id="V1FirstColumn5">
                    <asp:Label ID="lblEmail" runat="server" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                </td>
                <td id="V1SecondColumn5">
                    <asp:TextBox ID="txtEmail" runat="server" ReadOnly="True" TabIndex ="-1" Width="95%" ToolTip="EMail ID"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="35%" id="V1FirstColumn6">
                    <asp:Label ID="lblWebsite" runat="server" CssClass="styleDisplayLabel" Text="Website"></asp:Label>
                </td>
                <td id="V1SecondColumn6">
                    <asp:TextBox ID="txtWebSite" runat="server" ReadOnly="True" TabIndex ="-1" Width="95%" ToolTip="Website"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="CustomerDetailV2" runat="server">
        <table cellspacing="0" width="100%" runat="server" id="tblView2">
            <tr>
                <td class="styleFieldLabel" id="FirstColumn1">
                    <asp:Label ID="lblCustomerCode1" runat="server" CssClass="styleDisplayLabel" Text="Customer Code"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="SecondColumn1" width="30%">
                    <asp:TextBox ID="txtCustomerCode1" runat="server" ReadOnly="True" TabIndex ="-1" Width="65%" ToolTip="Customer Code"></asp:TextBox>
                </td>
                <td class="styleFieldLabel" id="ThirdColumn1">
                    <asp:Label ID="lblCustomerName1" runat="server" CssClass="styleDisplayLabel" Text="Customer Name"> </asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn1" width="30%">
                    <asp:TextBox ID="txtCustomerName1" runat="server" ReadOnly="True" TabIndex ="-1" ToolTip="Customer Name"
                        Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="styleFieldLabel" id="FirstColumn2">
                    <asp:Label ID="lblCusAddress1" runat="server" CssClass="styleDisplayLabel" Text="Address"></asp:Label>
                </td>
                <td rowspan="3" class="styleFieldAlign" id="SecondColumn2">
                    <asp:TextBox ID="txtCusAddress1" runat="server" ReadOnly="true" TabIndex ="-1" Width="95%" Rows="4"
                        TextMode="MultiLine" ToolTip="Address"></asp:TextBox>
                </td>
                <td class="styleFieldLabel" id="ThirdColumn2">
                    <asp:Label ID="lblPhone1" runat="server" CssClass="styleDisplayLabel" Text="Phone"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn2">
                    <asp:TextBox ID="txtPhone1" runat="server" ReadOnly="True" Width="44%" TabIndex ="-1" ToolTip="Phone"></asp:TextBox>
                    <asp:Label ID="lblMobile1" runat="server" CssClass="styleDisplayLabel" Text="[M]"></asp:Label>
                    <asp:TextBox ID="txtMobile1" runat="server" ReadOnly="True" Width="37%" TabIndex ="-1" ToolTip="Mobile"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="styleFieldLabel" id="FirstColumn3">
                </td>
                <td class="styleFieldLabel" id="ThirdColumn3">
                    <asp:Label ID="lblEmaail1" runat="server" CssClass="styleDisplayLabel" Text="Email Id"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn3">
                    <asp:TextBox ID="txtEmail1" runat="server" ReadOnly="True" TabIndex ="-1" Width="90%" ToolTip="EMail ID"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="styleFieldLabel" id="FirstColumn4">
                </td>
                <td class="styleFieldLabel" id="ThirdColumn4">
                    <asp:Label ID="lblWebSite1" runat="server" CssClass="styleDisplayLabel" Text="Website"></asp:Label>
                </td>
                <td class="styleFieldAlign" id="FourthColumn4">
                    <asp:TextBox ID="txtWebSite1" runat="server" ReadOnly="True" TabIndex ="-1" Width="90%" ToolTip="Website"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
