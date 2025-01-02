<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="S3GLoanAdSysJournal.aspx.cs" Inherits="S3GLoanAdSysJournal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <table width="100%" align="center" cellpadding="0" cellspacing="0">

        <tr>
            <td>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                    <ContentTemplate>

                        <table width="100%" align="center" cellpadding="0" cellspacing="0">

                            <tr>
                                <td class="stylePageHeading">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblHeading"
                                                    CssClass="styleDisplayLabel">
                                                </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" align="center" cellpadding="0" cellspacing="0">

                                        <tr>
                                            <td valign="top">

                                                <table cellpadding="0" cellspacing="0" style="width: 100%">

                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblLOB" runat="server"
                                                                Text="Line of Business"></asp:Label>
                                                        </td>

                                                        <td class="styleFieldAlign" style="padding-left: 10px">

                                                            <asp:DropDownList ID="ddlLOB" ValidationGroup="Header"
                                                                runat="server">
                                                            </asp:DropDownList>

                                                        </td>

                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblMJVNO" runat="server"
                                                                Text="Sys JV Number"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">

                                                            <asp:TextBox ID="txtSJVNo" Enabled="false" runat="server"></asp:TextBox>


                                                        </td>



                                                    </tr>


                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblBranch" runat="server"
                                                                Text="Location"></asp:Label>
                                                        </td>

                                                        <td class="styleFieldAlign" style="padding-left: 10px">

                                                            <asp:DropDownList ID="ddlBranch" ValidationGroup="Header"
                                                                runat="server" AutoPostBack="True">
                                                            </asp:DropDownList>

                                                        </td>

                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="lblMJVDate" runat="server"
                                                                Text="Sys JV Date"></asp:Label>
                                                        </td>
                                                        <td class="styleFieldAlign">

                                                            <asp:TextBox ID="txtSJVDate" runat="server"></asp:TextBox>
                                                            <asp:Image ID="imgSJVDate" runat="server" ImageUrl="~/Images/calendaer.gif" />

                                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                                OnClientDateSelectionChanged="checkDate_NextSystemDate"
                                                                Format="dd/MM/yyyy" PopupButtonID="imgSJVDate" TargetControlID="txtSJVDate">
                                                            </cc1:CalendarExtender>

                                                        </td>



                                                    </tr>

                                                    <tr>
                                                        <td class="styleFieldLabel">
                                                            <asp:Label ID="Label1" runat="server"
                                                                Text="Narration"></asp:Label>
                                                        </td>

                                                        <td class="styleFieldAlign" colspan="3" style="padding-left: 10px">

                                                            <asp:TextBox runat="server" Width="85%" ID="txtNarration"></asp:TextBox>

                                                        </td>

                                                    </tr>

                                                </table>


                                            </td>
                                        </tr>
                                        <tr>

                                            <td style="padding-top: 10px;">

                                                <table width="100%">

                                                    <tr>
                                                        <td width="100%">

                                                            <asp:GridView runat="server" CellPadding="4" CellSpacing="2" ShowFooter="false" AllowPaging="false"
                                                                ID="grvSysJournal" Width="98%" RowStyle-Wrap="false"
                                                                AutoGenerateColumns="false">
                                                                <Columns>
                                                                    <asp:BoundField DataField="CustomerName" HeaderText="Lessee Name" />
                                                                    <asp:BoundField DataField="Reference_Number" ItemStyle-Wrap="false" HeaderText="Rental Schedule No." />
                                                                    <asp:BoundField DataField="Sub_Reference_Number" Visible="false" ItemStyle-Wrap="false" HeaderText="Sub A/c No." />
                                                                    <asp:BoundField DataField="ValueDate" ItemStyle-Wrap="false" HeaderText="Value Date" />
                                                                    <%--<asp:BoundField DataField="TxnType"   ItemStyle-Wrap="false" HeaderText="Txn Type" />--%>
                                                                    <asp:BoundField DataField="GLAcc" ItemStyle-Wrap="false" HeaderText="GL A/c" />
                                                                    <asp:BoundField DataField="SLAcc" HeaderText="SL A/c" />
                                                                    <asp:BoundField DataField="TxnAmount" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" HeaderText="Txn Amount" />
                                                                    <asp:BoundField DataField="TxnType" HeaderStyle-Width="25px" ItemStyle-Wrap="false" HeaderText="" />
                                                                    <asp:BoundField DataField="Occurance" ItemStyle-HorizontalAlign="Right" Visible="false" HeaderText="Occurance" />
                                                                    <asp:BoundField DataField="Dim2" HeaderText="Dim2" />



                                                                </Columns>
                                                            </asp:GridView>

                                                        </td>
                                                    </tr>

                                                </table>

                                            </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>

                            <tr class="styleButtonArea" align="center" style="padding-left: 4px">
                                <td>
                                    <asp:Button runat="server" ID="btnSave" Enabled="false"
                                        CssClass="styleSubmitButton" ValidationGroup="Header" Text="Save" />
                                    &nbsp;<asp:Button runat="server" ID="btnClear" CausesValidation="false"
                                        CssClass="styleSubmitButton" Text="Clear"
                                        OnClientClick="return fnConfirmClear();" />
                                    &nbsp;<asp:Button runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"
                                        CausesValidation="false" CssClass="styleSubmitButton" />


                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Label ID="lblErrorMessage" runat="server"
                                        CssClass="styleMandatoryLabel"></asp:Label></td>
                            </tr>

                        </table>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </td>
        </tr>


    </table>

</asp:Content>


