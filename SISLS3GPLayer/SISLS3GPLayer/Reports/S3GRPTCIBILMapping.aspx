<%@ Page Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master" AutoEventWireup="true"
    CodeFile="S3GRPTCIBILMapping.aspx.cs" Inherits="Reports_S3GRPTCIBILMapping" Title="S3G - CIBIL Mapping" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0" scrolling="no">
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
            <td>
                <cc1:TabContainer ID="tcRepossession" runat="server" ActiveTabIndex="3" CssClass="styleTabPanel"
                    Width="99%" ScrollBars="Auto" Visible="true">
                    <cc1:TabPanel runat="server" ID="tbCIBILMapping" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            State Mapping
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td width="100%">
                                        <table width="40%" align="center">
                                            <tr>
                                                <td>
                                                    <asp:GridView Width="100%" ID="gvState" runat="server" AutoGenerateColumns="False"
                                                        OnRowDataBound="gvState_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="S3G State">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblS3G_State" runat="server" Text='<%# Eval("S3G_State") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CIBIL State">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCIBILState" runat="server" Text='<%# Eval("CIBIL_State_ID") %>'
                                                                        Visible="false"></asp:Label>
                                                                    <asp:DropDownList ID="ddlState" runat="server">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="Td1" runat="server" align="center">
                                                    <asp:Button runat="server" ID="btnMapState" CssClass="styleSubmitButton" Text="MAP State"
                                                        OnClick="btnMapState_Click" />
                                                    <asp:Button runat="server" ID="btnClearState" CssClass="styleSubmitButton" Text="Cancel"
                                                        CausesValidation="False" OnClick="btnClearState_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel1" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Constitution Mapping
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td width="100%">
                                        <table width="52%" align="center">
                                            <tr>
                                                <td>
                                                    <asp:GridView Width="100%" ID="gvCons" runat="server" AutoGenerateColumns="False"
                                                        OnRowDataBound="gvCons_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("Cons_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="S3G Constitutions">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblConsName" runat="server" Text='<%# Eval("Cons_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CIBIL Constitutions">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCIBILCons" runat="server" Text='<%# Eval("CIBIL_Cons_ID") %>' Visible="false"></asp:Label>
                                                                    <asp:DropDownList ID="ddlState" runat="server">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="Td2" runat="server" align="center">
                                                    <asp:Button runat="server" ID="btnMapCons" CssClass="styleSubmitButton" Text="MAP Constitution"
                                                        OnClick="btnMapCons_Click" />
                                                    <asp:Button runat="server" ID="btnClearCons" CssClass="styleSubmitButton" Text="Cancel"
                                                        CausesValidation="False" OnClick="btnClearCons_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="tbCollateral" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Collateral Securities Mapping
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td width="100%">
                                        <table width="50%" align="center">
                                            <tr>
                                                <td>
                                                    <asp:GridView Width="100%" ID="gvColl" runat="server" AutoGenerateColumns="False"
                                                        OnRowDataBound="gvColl_RowDataBound">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("Col_S3G_ID")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="S3G Collaterals">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCollName" runat="server" Text='<%# Eval("Collateral_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CIBIL Collaterals">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCIBILColl" runat="server" Text='<%# Eval("CIBIL_Sec_ID") %>' Visible="false"></asp:Label>
                                                                    <asp:DropDownList ID="ddlState" runat="server">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="Td3" runat="server" align="center">
                                                    <asp:Button runat="server" ID="btnMapColl" CssClass="styleSubmitButton" Text="MAP Securities"
                                                        OnClick="btnMapColl_Click" />
                                                    <asp:Button runat="server" ID="btnCollClear" CssClass="styleSubmitButton" Text="Cancel"
                                                        CausesValidation="False" OnClick="btnCollClear_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel runat="server" ID="TabPanel2" CssClass="tabpan" BackColor="Red">
                        <HeaderTemplate>
                            Account Type Mapping
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td width="100%">
                                        <div style="width: 780px; height: 235px; overflow-y: auto; overflow-x: hidden">
                                            <div style="width: 780px; height: 235px; overflow-y: hidden; overflow-x: auto">
                                                <asp:GridView ID="gvAccount" runat="server" AutoGenerateColumns="False" DataKeyNames="Sno"
                                                    EditRowStyle-CssClass="styleFooterInfo" ShowFooter="True" OnRowDataBound="gvAccount_RowDataBound"
                                                    OnRowDeleting="gvAccount_DeleteClick" Width="800px">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line of Business">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLOB" runat="server" Text='<%# Bind("LOB") %>'></asp:Label>
                                                                <asp:Label ID="lblLOB_ID" runat="server" Text='<%# Bind("LOB_ID") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlLOBF" runat="server" ToolTip="Line of Business" OnSelectedIndexChanged="ddlLOBF_SelectedIndexChanged"
                                                                    AutoPostBack="true">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvLOB" runat="server" InitialValue="0" ControlToValidate="ddlLOBF"
                                                                    Display="None" ErrorMessage="Select a Line of Business" ValidationGroup="Add">
                                                                </asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="S3G Asset Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetType" runat="server" Text='<%# Bind("ASSET_TYPE_NAME") %>'></asp:Label>
                                                                <asp:Label ID="lblAssetType_ID" runat="server" Text='<%# Bind("ASSET_TYPE_ID") %>'
                                                                    Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlAssetTypeF" runat="server" ToolTip="Asset Type">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="S3G Asset Class">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssetClass" runat="server" Text='<%# Bind("ASSET_CLASS_NAME") %>'></asp:Label>
                                                                <asp:Label ID="lblAssetClass_ID" runat="server" Text='<%# Bind("ASSET_CLASS_ID") %>'
                                                                    Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlAssetClassF" runat="server" ToolTip="Asset Class">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="S3G Product">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProduct" runat="server" Text='<%# Bind("Product_Name") %>'></asp:Label>
                                                                <asp:Label ID="lblProduct_ID" runat="server" Text='<%# Bind("Product_ID") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlProductF" runat="server" ToolTip="Product">
                                                                </asp:DropDownList>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="CIBIL Account Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCIBIL_ID" runat="server" Text='<%# Bind("CIBIL_Asset_ID") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblCIBIL_Type" runat="server" Text='<%# Bind("Asset_Type") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:DropDownList ID="ddlCIBILAccountF" runat="server" ToolTip="CIBIL Account Type">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvCIBILAccountF" runat="server" InitialValue="0"
                                                                    ControlToValidate="ddlCIBILAccountF" Display="None" ErrorMessage="Select CIBIL Account Type"
                                                                    ValidationGroup="Add">
                                                                </asp:RequiredFieldValidator>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <%--<link button />--%>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete"
                                                                    OnClientClick="return confirm('Are you sure want to Delete this record?');" ToolTip="Delete">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="styleGridShortButton"
                                                                    OnClick="btnAdd_Click" Text="Add" ValidationGroup="Add" ToolTip="Add" />
                                                            </FooterTemplate>
                                                            <%--  <EditItemTemplate>
                                                                    <asp:LinkButton ID="lnkUpdate" runat="server" Text="Update" CommandName="Update"
                                                                        ValidationGroup="Add" ToolTip="Update"></asp:LinkButton>&nbsp;|
                                                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel"
                                                                        CausesValidation="false" ToolTip="Cancel"></asp:LinkButton>
                                                                </EditItemTemplate>--%>
                                                            <FooterStyle HorizontalAlign="Center" Width="10%" />
                                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="Td5" runat="server" align="center">
                                        <asp:Button runat="server" ID="btnMapAsset" CssClass="styleSubmitButton" Text="MAP Asset"
                                            OnClick="btnMapAsset_Click" />
                                        <asp:Button runat="server" ID="btnAssetClear" CssClass="styleSubmitButton" Text="Cancel"
                                            CausesValidation="False" OnClick="btnAssetClear_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
