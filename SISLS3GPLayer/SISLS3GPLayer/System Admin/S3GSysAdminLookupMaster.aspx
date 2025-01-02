<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GSysAdminLookupMaster.aspx.cs" Inherits="System_Admin_S3GSysAdminLookupMaster"
    Title="S3G - Lookup Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                        <asp:Panel GroupingText="Lookup Values" ID="pnlLookInfo" runat="server"
                            CssClass="stylePanel">
                            <table cellpadding="0" cellspacing="0" width="635px">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Module Name" ID="lblModule" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlModule" runat="server" Style="width: 240px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" TabIndex="3"
                                            onmouseover="ddl_itemchanged(this);">
                                        </asp:DropDownList>
                                        <%--OnTextChanged="ddlModule_TextChanged" --%>
                                    </td>
                                    <td style="width: 209px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="ddlModule" InitialValue="0" ErrorMessage="Select Module Name"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="Program Name" ID="lblProgram" CssClass="styleReqFieldLabel">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlProgram" runat="server" Style="width: 240px" TabIndex="4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"
                                            onmouseover="ddl_itemchanged(this);" ToolTip="--Select--">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 209px">
                                        <asp:RequiredFieldValidator ID="rfvLOB" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="ddlProgram" InitialValue="0" ErrorMessage="Select Program Name"
                                            Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" CssClass="styleReqFieldLabel" Text="Program Name" ID="lblDocType"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                    <asp:DropDownList ID="ddlDocType" onmouseover="ddl_itemchanged(this);" runat="server"
                                        Width="205px" TabIndex="4">
                                    </asp:DropDownList>
                                    </td>
                                    <td style="width: 20%">
                                        <asp:RequiredFieldValidator ID="rfvDocType" runat="server" Display="None" ErrorMessage="Select the Program Name"
                                            ControlToValidate="ddlDocType" SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="styleFieldLabel" style="width: 200px">
                                        <asp:Label runat="server" Text="Field Name" CssClass="styleReqFieldLabel"
                                            ID="lblLookupTypeDesc">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlLookupTypeDesc" runat="server" Style="width: 240px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlLookupTypeDesc_SelectedIndexChanged" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 229px">
                                        <asp:ImageButton ID="imgbtnAdd" ImageUrl="~/Images/downarrow_03.png" runat="server"
                                            Height="20px" Width="20px" OnClick="imgbtnAdd_Click" ToolTip="Add" />
                                        <asp:RequiredFieldValidator ID="rfvLookupTypeDesc" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="ddlLookupTypeDesc" ErrorMessage="Select Field Name"
                                            InitialValue="0" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label runat="server" Text="RCU Type" CssClass="styleReqFieldLabel" ID="lblRCU" Visible="false">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlRCUType" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="ddlRCUType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="rfvRCUType" CssClass="styleMandatoryLabel"
                                            runat="server" ControlToValidate="ddlRCUType" ErrorMessage="Select the RCU Type"
                                            InitialValue="0" Display="None" Enabled="false">
                                        </asp:RequiredFieldValidator>

                                    </td>
                                </tr>

                                <tr>
                                    <td class="styleFieldLabel" style="width: 200px">
                                        <asp:Label runat="server" Text="Lookup Value" CssClass="styleReqFieldLabel" ID="lblLookupName">
                                        </asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtLookupName" runat="server" Style="width: 300px"></asp:TextBox>
                                    </td>
                                    <td style="width: 229px">
                                        <asp:RequiredFieldValidator ID="rfvLookupName" CssClass="styleMandatoryLabel" runat="server"
                                            ControlToValidate="txtLookupName" ErrorMessage="Enter Lookup Value" Display="None" >
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td width="100%" align="center">
                                        <asp:Panel GroupingText="Details" ID="pnlDetail" runat="server"
                                            CssClass="stylePanel" Width="80%">
                                            <div style="height: 235px; overflow-x: hidden; overflow-y: auto;">
                                                <asp:GridView ID="gvLookupMaster" runat="server" AutoGenerateColumns="false" Width="96%"
                                                    OnRowDataBound="gvLookupMaster_RowDataBound" OnRowUpdating="gvLookupMaster_RowUpdating">
                                                    <%--OnRowCommand="gvLookupMaster_RowCommand"--%>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%#Bind("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Field Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLookupCategory" runat="server" Text='<%#Bind("Lookup_Type") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="30%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Lookup Value">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLookupName" runat="server" Text='<%#Bind("Name") %>' Width="95%"  MaxLength="200"/>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="50%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Lookup Desc" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLookupDesc" runat="server" Text='<%#Bind("Lookup_Desc") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="25%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Table Name" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTableName" runat="server" Text='<%#Bind("Table_Name") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="25%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Update">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="Update" OnClick="btnUpdate_Click"
                                                                    CssClass="styleSubmitShortButton" CausesValidation="false"></asp:Button>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr align="center">
                                    <td >
                                        <asp:Button runat="server" ID="btnSave" OnClientClick="return fnCheckPageValidators();"
                                            CssClass="styleSubmitButton" Text="Save" OnClick="btnSave_Click" Enabled="false" />
                                        <asp:Button runat="server" ID="btnClear" CssClass="styleSubmitButton" CausesValidation="false"
                                            Text="Clear" OnClick="btnClear_Click" Enabled="false"/>
                                        <%--<cc1:ConfirmButtonExtender ID="btnClear_ConfirmButtonExtender" runat="server" ConfirmText="Do you want to clear?"
                                                    Enabled="True" TargetControlID="btnClear">
                                                </cc1:ConfirmButtonExtender>--%>
                                        <%-- <asp:Button runat="server" ID="btnCancel" Text="Cancel" CausesValidation="false"
                                                        CssClass="styleSubmitButton" OnClick="btnCancel_Click" />--%>
                                    </td>
                                </tr>
                            </table>                            
                            <table >
                                <tr>
                                    <td colspan="3">
                                        <asp:ValidationSummary CssClass="styleSummaryStyle" runat="server" ID="vsCurrency"
                                            HeaderText="Please correct the following validation(s):  " ShowSummary="true"
                                            ShowMessageBox="false" />
                                    </td>
                                </tr>
                                <tr class="styleButtonArea">
                                    <td colspan="3">
                                        <asp:Label ID="lblErrorMessage" runat="server" CssClass="styleMandatoryLabel"></asp:Label>
                                        <br />
                                    </td>
                                </tr>
                            </table>
            </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
