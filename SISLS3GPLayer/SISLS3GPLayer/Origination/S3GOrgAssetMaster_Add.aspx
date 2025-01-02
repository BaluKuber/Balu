<%@ Page Title="S3G - Asset Master" Language="C#" MasterPageFile="~/Common/S3GMasterPageCollapse.master"
    AutoEventWireup="true" CodeFile="~/Origination/S3GOrgAssetMaster_Add.aspx.cs"
    Inherits="Origination_S3G_OrgAssetMaster_Add" %>

<%@ Register Assembly="iCONWebComponents" Namespace="iCON.Web.Components" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc2" TagName="Suggest" Src="~/UserControls/S3GAutoSuggest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="stylePageHeading">
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Asset Master" ID="lblHeading" CssClass="styleDisplayLabel"> </asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding-top: 10px">
                <asp:Panel Visible="true" runat="server" ID="panCategoryCode" GroupingText="Category Code"
                    CssClass="stylePanel">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblhassetcategory" Visible="false" runat="server" Text="Asset Category" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:Label ID="lblrassetcategory" Visible="false" runat="server" Text="Asset Category" CssClass="styleDisplayLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblAssetCategory" Visible="false" runat="server" Text="Asset Category" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:DropDownList ID="ddlAssetCategory" Visible="false" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvAssetCategory" ValidationGroup="CategoryGroup"
                                            runat="server" ErrorMessage="Select the Asset Category" SetFocusOnError="True" InitialValue="0"
                                            ControlToValidate="ddlAssetCategory" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvAssetCategory1" ValidationGroup="Save"
                                            runat="server" ErrorMessage="Select the Asset Category" SetFocusOnError="True" InitialValue="0"
                                            ControlToValidate="ddlAssetCategory" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCode" runat="server" Text="Asset Category Code" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">

                                        <asp:TextBox ID="txtCode" runat="server" ValidationGroup="CategoryGroup" MaxLength="3" Width="100px">
                                        </asp:TextBox>

                                        <asp:RequiredFieldValidator ID="rfvCode" ValidationGroup="CategoryGroup"
                                            runat="server" ErrorMessage="Enter the Asset Category Code" SetFocusOnError="True"
                                            ControlToValidate="txtCode" Display="None"></asp:RequiredFieldValidator>

                                        <cc1:FilteredTextBoxExtender ID="ftexCategoryCode" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            TargetControlID="txtCode" runat="server" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>

                                    </td>
                                </tr>

                                <tr style="display: none" runat="server" id="hsnrow">
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblHSNCode" runat="server" CssClass="styleReqFieldLabel" Text="HSN Code"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <uc2:Suggest ID="txtHSNCode" runat="server" ServiceMethod="GetHSNList" AutoPostBack="true"
                                            ErrorMessage="Select HSN Code"
                                            ValidationGroup="CategoryGroup" IsMandatory="true" />
                                    </td>
                                    <td></td>
                                </tr>


                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblCodeDesc" runat="server" Text="Asset Category Name" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:TextBox ID="txtCodeDescription" runat="server" ValidationGroup="CategoryGroup"
                                            MaxLength="200" Width="200px"></asp:TextBox>

                                         <cc1:FilteredTextBoxExtender ID="FTBCodeDescription" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                            TargetControlID="txtCodeDescription" ValidChars=" ,&,-" InvalidChars="',-,<,>,;" runat="server" Enabled="True">
                                        </cc1:FilteredTextBoxExtender>

                                        <asp:RequiredFieldValidator ID="rfvCodeDescription" ValidationGroup="CategoryGroup" runat="server" ErrorMessage="Enter the Asset Category Description"
                                            SetFocusOnError="True" ControlToValidate="txtCodeDescription" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvDesc" ValidationGroup="Save" runat="server" ErrorMessage="Enter the Asset Category Description"
                                            SetFocusOnError="True" ControlToValidate="txtCodeDescription" Display="None"></asp:RequiredFieldValidator>
                                        <asp:Button ID="btnCategoryGo" Text="Go" runat="server" ValidationGroup="CategoryGroup"
                                            CssClass="styleSubmitShortButton" OnClick="btnCategoryGo_Click" />
                                    </td>
                                    <td></td>
                                </tr>


                                <tr>
                                    <td class="styleFieldLabel">
                                        <asp:Label ID="lblActive" runat="server" Text="Active" CssClass="styleReqFieldLabel"></asp:Label>
                                    </td>
                                    <td class="styleFieldAlign">
                                        <asp:CheckBox runat="server" ID="chkActive" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="3" valign="top" width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td valign="top" width="100%">
                                                    <cc1:TabContainer ID="tcCode" runat="server" ActiveTabIndex="0" CssClass="styleTabPanel" Width="100%" ScrollBars="Auto" AutoPostBack="true" OnActiveTabChanged="chkCategoryType_SelectedIndexChanged">
                                                        <cc1:TabPanel runat="server" HeaderText="Category" ID="tpClassCode" CssClass="tabpan"
                                                            BackColor="Red">
                                                            <HeaderTemplate>
                                                                Category
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <asp:GridView ID="grvAssetClass" runat="server" Width="100%" OnRowDeleting="grvAsset_RowDeleting"
                                                                    AutoGenerateColumns="False" EmptyDataText="No Records found">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Code" HeaderText="Code" />
                                                                        <asp:BoundField DataField="Description" HeaderText="Name" />
                                                                        <asp:TemplateField HeaderText="Remove">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton Text="Remove" runat="server" ID="lnkbtnDelete" CommandName="Delete"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel runat="server" HeaderText="Type" ID="tpMakeCode" CssClass="tabpan"
                                                            BackColor="Red">
                                                            <HeaderTemplate>
                                                                Type
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <asp:GridView ID="grvAssetMake" runat="server" OnRowDeleting="grvAsset_RowDeleting"
                                                                    Width="100%" AutoGenerateColumns="False" EmptyDataText="No Records found">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Code" HeaderText="Code" />
                                                                        <asp:BoundField DataField="Description" HeaderText="Name" />
                                                                        <asp:TemplateField HeaderText="Remove">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton Text="Remove" runat="server" ID="lnkbtnDelete" CommandName="Delete"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel runat="server" HeaderText="Sub Type" ID="tpTypeCode" CssClass="tabpan"
                                                            BackColor="Red">
                                                            <HeaderTemplate>
                                                                Sub Type
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <asp:GridView ID="grvAssetType" runat="server" OnRowDeleting="grvAsset_RowDeleting"
                                                                    Width="100%" AutoGenerateColumns="False" EmptyDataText="No Records found">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="Code" HeaderText="Code" />
                                                                        <asp:BoundField DataField="Description" HeaderText="Name" />
                                                                        <asp:TemplateField HeaderText="Remove">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton Text="Remove" runat="server" ID="lnkbtnDelete" CommandName="Delete"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                    </cc1:TabContainer>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td colspan="3">
                                        <asp:Button ID="btnCategorySubmit" Text="Save" runat="server" CssClass="styleSubmitButton" ValidationGroup="Save"
                                            OnClick="btnCategorySubmit_Click" />&nbsp;
                                        <asp:Button ID="btnCategoryCancel" Text="Cancel" runat="server" CausesValidation="False"
                                            CssClass="styleSubmitButton" OnClick="btnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:ValidationSummary ID="vsCategoryGroup" runat="server" ValidationGroup="CategoryGroup"
                                            CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):" />
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Save"
                                            CssClass="styleMandatoryLabel" HeaderText="Please correct the following validation(s):" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Label ID="hdnHSN" runat="server" Visible ="false" Text="" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        function FunTab() {
            var tab = document.getElementById('<%=tcCode.ClientID %>');
            tab.get_tabs()[1].set_enabled(false);
        }
    </script>
</asp:Content>
